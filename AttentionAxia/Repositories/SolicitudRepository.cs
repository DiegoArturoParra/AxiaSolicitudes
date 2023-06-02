using AttentionAxia.Core.Data;
using AttentionAxia.DTOs;
using AttentionAxia.DTOs.Filters;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using log4net;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace AttentionAxia.Repositories
{
    public class SolicitudRepository : GenericRepository<Solicitud>
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SolicitudRepository(AxiaContext context) : base(context)
        {
        }

        public async Task<ListarSolicitudDTO> GetSolicitudes(SolicitudFilterDTO filtro)
        {
            try
            {
                var modelo = new ListarSolicitudDTO();
                var query = (from solicitud in Context.TablaSolicitudes
                             join celula in Context.TablaCelulas on solicitud.CelulaId equals celula.Id
                             join estado in Context.TablaEstados on solicitud.EstadoId equals estado.Id
                             join sprintInicio in Context.TablaSprints on solicitud.SprintInicioId equals sprintInicio.Id
                             join sprintFin in Context.TablaSprints on solicitud.SprintFinId equals sprintFin.Id
                             join responsable in Context.TablaResponsables on solicitud.ResponsableId equals responsable.Id
                             select new
                             {
                                 solicitud,
                                 estado,
                                 sprintInicio,
                                 sprintFin,
                                 responsable,
                                 celula,
                             });
                if (!string.IsNullOrWhiteSpace(filtro.FiltroFecha))
                {
                    string[] partes = filtro.FiltroFecha.Split('-');

                    string fechaInicioStr = partes[0].Trim();
                    string fechaFinStr = partes[1].Trim();

                    filtro.FechaInicial = DateTime.ParseExact(fechaInicioStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    filtro.FechaFinal = DateTime.ParseExact(fechaFinStr, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddHours(24).AddSeconds(-1);
                    query = query.Where(x => x.solicitud.FechaCreacionSolicitud >= filtro.FechaInicial && x.solicitud.FechaCreacionSolicitud <= filtro.FechaFinal);
                }

                if (filtro.Estado.HasValue && filtro.Estado.Value > 0)
                {
                    query = query.Where(x => x.solicitud.EstadoId == filtro.Estado.Value);
                }
                if (filtro.Responsable.HasValue && filtro.Responsable.Value > 0)
                {
                    query = query.Where(x => x.solicitud.ResponsableId == filtro.Responsable.Value);
                }
                if (filtro.Sprint.HasValue && filtro.Sprint.Value > 0)
                {
                    query = query.Where(x => x.solicitud.SprintInicioId == filtro.Sprint.Value);
                }
                if (filtro.Celula.HasValue && filtro.Celula.Value > 0)
                {
                    query = query.Where(x => x.solicitud.CelulaId == filtro.Celula.Value);
                }
                if (filtro.Linea.HasValue && filtro.Linea.Value > 0)
                {
                    query = query.Where(x => x.solicitud.Responsable.LineaPerteneceId == filtro.Linea.Value);
                }

                if (query.Any())
                {
                    var listado = await query.OrderBy(x => x.solicitud.Id)
                      .Skip((filtro.Page - 1) * filtro.Page)
                      .Take(filtro.PageSize).Select(m => new SolicitudDTO
                      {
                          Id = m.solicitud.Id,
                          ColorEstado = m.estado.Nivel,
                          Avance = m.solicitud.Avance,
                          Celula = m.celula.Descripcion,
                          Estado = m.estado.Descripcion,
                          EstadoId = m.estado.Id,
                          Linea = Context.TablaLineas.Where(y => y.Id == m.responsable.LineaPerteneceId).Select(s => s.Descripcion).FirstOrDefault(),
                          Iniciativa = m.solicitud.Iniciativa,
                          Responsable = m.responsable.Nombres,
                          SprintInicio = m.sprintInicio.Sigla + " " + m.sprintInicio.Periodo,
                          SprintFin = m.sprintFin.Sigla + " " + m.sprintFin.Periodo,
                          SprintInicioFechaGeneracion = m.sprintInicio.FechaInicio.HasValue ? m.sprintInicio.FechaInicio.Value : default,
                          SprintFinFechaGeneracion = m.sprintFin.FechaFin.HasValue ? m.sprintFin.FechaFin.Value : default,
                          NombreArchivo = m.solicitud.NombreArchivo,
                          RutaArchivo = m.solicitud.RutaArchivo,
                          LeadTime = m.solicitud.LeadTime,
                          CycleTimeReal = m.solicitud.CycleTimeReal,
                          FechaCreacion = m.solicitud.FechaCreacionSolicitud,
                          FechaComienzo = m.solicitud.FechaInicioReal,
                          FechaFinalizacion = m.solicitud.FechaFinReal,
                          PorcentajeDeCumplimiento = m.solicitud.PorcentajeCumplimiento,
                      }).ToListAsync();
                    foreach (var item in listado)
                    {
                        item.LeadTime = !item.LeadTime.HasValue ? GetLeadTime(item.FechaCreacion, null) : item.LeadTime;
                        item.CycleTimeReal = !item.CycleTimeReal.HasValue ? GetCycleTime(item.FechaComienzo, null) : item.CycleTimeReal;
                    }

                    int totalDeRegistros = query.Count();
                    modelo.Solicitudes = listado;
                    modelo.PaginaActual = filtro.Page;
                    modelo.TotalDeRegistros = totalDeRegistros;
                    modelo.RegistrosPorPagina = filtro.PageSize;
                    modelo.ValoresQueryString = new RouteValueDictionary
                    {
                        ["FechaInicial"] = filtro.FechaInicial,
                        ["FechaFinal"] = filtro.FechaFinal,
                        ["Avance"] = filtro.Avance
                    };
                }
                return modelo;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
        }

        public short? GetLeadTime(DateTime dateCreated, DateTime? dateFinish)
        {
            DateTime dateCurrent = DateTime.Now;
            if (dateFinish.HasValue)
            {
                dateCurrent = dateFinish.Value;
            }

            DateTime dateInitial = new DateTime(dateCreated.Year, dateCreated.Month, dateCreated.Day, 0, 0, 0);
            DateTime dateFinal = new DateTime(dateCurrent.Year, dateCurrent.Month, dateCurrent.Day, 0, 0, 0).AddHours(24).AddSeconds(-1);
            var datesHolidays = Context.TablaFestivosColombia.Where(f => f.FechaFestivo >= dateInitial && f.FechaFestivo <= dateFinal).ToList();
            int totalDays = dateCurrent.Subtract(dateCreated).Days + 1;
            int businessDays = Enumerable.Range(0, totalDays).Select(i => dateInitial.AddDays(i)).
                Count(date => !datesHolidays.Any(y => y.FechaFestivo == date) && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);
            return (short)businessDays;
        }


        public short? GetCycleTime(DateTime? dateInProcess, DateTime? dateFinish)
        {
            if (!dateInProcess.HasValue)
            {
                return null;
            }
            DateTime dateCurrent = DateTime.Now;
            if (dateFinish.HasValue)
            {
                dateCurrent = dateFinish.Value;
            }
            DateTime dateInitial = new DateTime(dateInProcess.Value.Year, dateInProcess.Value.Month, dateInProcess.Value.Day, 0, 0, 0);
            DateTime dateFinal = new DateTime(dateCurrent.Year, dateCurrent.Month, dateCurrent.Day, 0, 0, 0).AddHours(24).AddSeconds(-1);
            var datesHolidays = Context.TablaFestivosColombia.Where(f => f.FechaFestivo >= dateInitial && f.FechaFestivo <= dateFinal).ToList();
            int totalDays = dateCurrent.Subtract(dateInProcess.Value).Days + 1;
            int businessDays = Enumerable.Range(0, totalDays).Select(i => dateInitial.AddDays(i)).
                Count(date => !datesHolidays.Any(y => y.FechaFestivo == date) && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);
            return (short)businessDays;
        }

        public async Task<ResponseDTO> InsertWithArchive(CreateSolicitudDTO solicitud, HttpPostedFileBase file, string rutaInicial)
        {
            FileDTO fileDTO = null;
            try
            {
                ResponseDTO response = new ResponseDTO();
                Solicitud entity = new Solicitud()
                {
                    ResponsableId = solicitud.ResponsableId,
                    EstadoId = solicitud.EstadoId,
                    SprintInicioId = solicitud.SprintInicioId,
                    SprintFinId = solicitud.SprintFinId,
                    FechaInicioPlaneada = solicitud.FechaInicialParse,
                    FechaFinPlaneada = solicitud.FechaFinalParse.AddHours(24).AddSeconds(-1),
                    Iniciativa = solicitud.Iniciativa,
                    CelulaId = solicitud.CelulaId,
                    FechaCreacionSolicitud = DateTime.Now,
                    Avance = 0
                };
                var cycleTimePlaneado = GetCycleTime(entity.FechaInicioPlaneada, entity.FechaFinPlaneada);
                if (!cycleTimePlaneado.HasValue)
                    return Responses.SetErrorResponse("No se pudo calcular el tiempo planeado en días.");

                entity.CycleTimePlaneado = cycleTimePlaneado.Value;
                response = await ValidationsOfBusiness(entity);
                if (response.IsSuccess)
                {
                    if (file == null)
                    {
                        Insert(entity);
                        await Save();
                        return Responses.SetCreateResponse();
                    }
                    FileHelper.FolderIsExist(rutaInicial, GetConstants.CARPETA_ARCHIVOS_SOLICITUDES);
                    response = FileHelper.SaveFile(file, rutaInicial, GetConstants.CARPETA_ARCHIVOS_SOLICITUDES, file.FileName);
                    if (!response.IsSuccess)
                        return Responses.SetErrorResponse(response.Message);

                    fileDTO = (FileDTO)response.Data;
                    entity.RutaArchivo = fileDTO.PathArchivo;
                    entity.NombreArchivo = fileDTO.NombreArchivo;
                    Insert(entity);
                    await Save();
                    response = Responses.SetCreateResponse();
                }
                return response;
            }
            catch (Exception ex)
            {
                if (fileDTO != null)
                {
                    FileHelper.DeleteFile(rutaInicial, fileDTO.PathArchivo);
                }
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
            }
        }
        public async Task<ResponseDTO> UpdateWithArchive(EditSolicitudDTO solicitud, HttpPostedFileBase file, string rutaInicial)
        {
            FileDTO fileDTO = null;
            try
            {
                var response = new ResponseDTO();
                var entity = FindById(solicitud.SolicitudId);
                if (entity == null)
                {
                    return Responses.SetErrorResponse("No existe la solicitud.");
                }
                entity.ResponsableId = solicitud.ResponsableId;
                entity.EstadoId = solicitud.EstadoId;
                entity.SprintInicioId = solicitud.SprintInicioId;
                entity.SprintFinId = solicitud.SprintFinId;
                entity.FechaInicioPlaneada = solicitud.FechaInicialParse;
                entity.FechaFinPlaneada = solicitud.FechaFinalParse.AddHours(24).AddSeconds(-1);
                entity.Iniciativa = solicitud.Iniciativa;
                entity.CelulaId = solicitud.CelulaId;
                entity.Avance = solicitud.Avance;

                response = await ValidationsOfBusiness(entity);
                if (response.IsSuccess)
                {
                    entity = InsertDateByState(entity);
                    if (file == null)
                    {
                        Update(entity);
                        await Save();
                        return Responses.SetOkResponse("Edición satisfactoriamente.");
                    }
                    FileHelper.FolderIsExist(rutaInicial, GetConstants.CARPETA_ARCHIVOS_SOLICITUDES);
                    if (!string.IsNullOrWhiteSpace(entity.RutaArchivo))
                    {
                        FileHelper.DeleteFile(rutaInicial, entity.RutaArchivo);
                    }
                    response = FileHelper.SaveFile(file, rutaInicial, GetConstants.CARPETA_ARCHIVOS_SOLICITUDES, file.FileName);
                    if (!response.IsSuccess)
                    {
                        return Responses.SetErrorResponse(response.Message);
                    }
                    fileDTO = (FileDTO)response.Data;
                    entity.RutaArchivo = fileDTO.PathArchivo;
                    entity.NombreArchivo = fileDTO.NombreArchivo;
                    Update(entity);
                    await Save();
                    response = Responses.SetOkResponse("Edición satisfactoriamente.");
                }
                return response;
            }
            catch (Exception ex)
            {
                FileHelper.DeleteFile(rutaInicial, fileDTO.PathArchivo);
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
            }
        }


        public async Task<ResponseDTO> UpdateByState(EditSolicitudByStateDTO editSolicitud)
        {
            try
            {
                var response = new ResponseDTO();
                var entity = FindById(editSolicitud.SolicitudId);
                if (entity == null)
                {
                    return Responses.SetErrorResponse("No existe la solicitud.");
                }
                entity.EstadoId = editSolicitud.EstadoId;
                entity.Avance = editSolicitud.Avance;
                response = await ValidationsOfBusiness(entity);
                if (response.IsSuccess)
                {
                    entity = InsertDateByState(entity);
                    Update(entity);
                    await Save();
                    response = Responses.SetOkResponse("Edición satisfactoriamente.");
                }
                return response;
            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
            }
        }

        private Solicitud InsertDateByState(Solicitud entity)
        {
            if (entity.EstadoId == (int)EstadosSolicitudEnum.EnProgreso && !entity.FechaInicioReal.HasValue)
            {
                entity.FechaInicioReal = DateTime.Now;
            }
            else if (entity.EstadoId == (int)EstadosSolicitudEnum.Finalizado && !entity.FechaFinReal.HasValue)
            {
                entity.FechaFinReal = DateTime.Now;
                entity.LeadTime = GetLeadTime(entity.FechaCreacionSolicitud, entity.FechaFinReal);
                entity.CycleTimeReal = GetCycleTime(entity.FechaInicioReal, entity.FechaFinReal);
                entity.PorcentajeCumplimiento = GetPorcentajeCumplimiento(entity.CycleTimePlaneado, entity.CycleTimeReal);
            }
            return entity;
        }

        private byte GetPorcentajeCumplimiento(short cycleTimePlaneado, short? cycleTimeReal)
        {
            var calculo = (cycleTimePlaneado * 100) / cycleTimeReal.Value;
            return (byte)calculo;
        }

        public async Task<ResponseDTO> ValidationsOfBusiness(Solicitud solicitud)
        {
            try
            {
                var response = new ResponseDTO
                {
                    IsSuccess = true,
                    Message = "Validaciones correctas."
                };
                var existeResponsable = await Context.TablaResponsables.AnyAsync(x => x.Id == solicitud.ResponsableId);
                if (!existeResponsable)
                    response = Responses.SetErrorResponse("No existe el responsable.");

                var existeEstado = await Context.TablaEstados.AnyAsync(x => x.Id == solicitud.EstadoId);
                if (!existeEstado)
                    response = Responses.SetErrorResponse("No existe el estado.");

                var existeSprint = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintInicioId);
                if (!existeSprint)
                    response = Responses.SetErrorResponse("No existe el sprint inicio.");

                var existeSprintfin = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintFinId);
                if (!existeSprintfin)
                    response = Responses.SetErrorResponse("No existe el sprint fin.");

                var existeCelula = await Context.TablaCelulas.AnyAsync(x => x.Id == solicitud.CelulaId);
                if (!existeCelula)
                    response = Responses.SetErrorResponse("No existe la célula.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
            }
        }
    }
}