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
                var query = from solicitud in Context.TablaSolicitudes.Include(y => y.SprintInicio).Include(x => x.SprintFin)
                            join celula in Context.TablaCelulas on solicitud.CelulaId equals celula.Id
                            join estado in Context.TablaEstados on solicitud.EstadoId equals estado.Id
                            join responsable in Context.TablaResponsables on solicitud.ResponsableId equals responsable.Id
                            select new
                            {
                                solicitud,
                                estado,
                                responsable,
                                celula,
                            };

                if (!string.IsNullOrWhiteSpace(filtro.FiltroFecha))
                {
                    string[] partes = filtro.FiltroFecha.Split('-');

                    string fechaInicioStr = partes[0].Trim();
                    string fechaFinStr = partes[1].Trim();

                    filtro.FechaInicial = DateTime.ParseExact(fechaInicioStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    filtro.FechaFinal = DateTime.ParseExact(fechaFinStr, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddHours(24).AddSeconds(-1);
                    query = query.Where(x => x.solicitud.FechaInicioPlaneada >= filtro.FechaInicial && x.solicitud.FechaInicioPlaneada <= filtro.FechaFinal);
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

                if (await query.CountAsync() > 0)
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
                          SprintInicio = m.solicitud.SprintInicio == null ? "N/A" : m.solicitud.SprintInicio.Sigla + " " + m.solicitud.SprintInicio.Periodo,
                          SprintFin = m.solicitud.SprintFin == null ? "N/A" : m.solicitud.SprintFin.Sigla + " " + m.solicitud.SprintFin.Periodo,
                          SprintInicioFechaGeneracion = m.solicitud.SprintInicio != null ? m.solicitud.SprintInicio.FechaInicio ?? default(DateTime?) : default(DateTime?),
                          SprintFinFechaGeneracion = m.solicitud.SprintFin != null ? m.solicitud.SprintFin.FechaFin ?? default(DateTime?) : default(DateTime?),
                          NombreArchivo = m.solicitud.NombreArchivo,
                          RutaArchivo = m.solicitud.RutaArchivo,
                          CycleTimeEsperado = m.solicitud.CycleTimePlaneado,
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
                if (solicitud.FechaInicialParse.HasValue && solicitud.FechaInicialParse.HasValue)
                {
                    if (solicitud.FechaFinalParse <= solicitud.FechaInicialParse)
                        return Responses.SetErrorResponse("La fecha fin del sprint no debe ser menor a la inicial.");
                }
                Solicitud entity = new Solicitud()
                {
                    ResponsableId = solicitud.ResponsableId,
                    EstadoId = solicitud.EstadoId,
                    SprintInicioId = solicitud.SprintInicioId,
                    SprintFinId = solicitud.SprintFinId,
                    FechaInicioPlaneada = solicitud.FechaInicialParse,
                    FechaFinPlaneada = solicitud.FechaFinalParse.HasValue ? solicitud.FechaFinalParse.Value.AddHours(24).AddSeconds(-1) : default(DateTime?),
                    Iniciativa = solicitud.Iniciativa,
                    CelulaId = solicitud.CelulaId,
                    FechaCreacionSolicitud = DateTime.Now,
                    Avance = 0
                };

                if (entity.SprintInicioId.HasValue && entity.SprintFinId.HasValue)
                {
                    var cycleTimePlaneado = GetCycleTime(entity.FechaInicioPlaneada, entity.FechaFinPlaneada);
                    if (!cycleTimePlaneado.HasValue)
                        return Responses.SetErrorResponse("No se pudo calcular el tiempo planeado en días.");
                    entity.CycleTimePlaneado = cycleTimePlaneado.Value;
                }
                response = await ValidationsOfBusiness(entity, true);
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

                if (solicitud.FechaFinalParse <= solicitud.FechaInicialParse)
                {
                    return Responses.SetErrorResponse("La fecha fin del sprint no debe ser menor a la inicial.");
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
                var cycleTimePlaneado = GetCycleTime(entity.FechaInicioPlaneada, entity.FechaFinPlaneada);
                if (!cycleTimePlaneado.HasValue)
                    return Responses.SetErrorResponse("No se pudo calcular el tiempo planeado en días.");

                entity.CycleTimePlaneado = cycleTimePlaneado.Value;
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
                if (fileDTO != null)
                {
                    FileHelper.DeleteFile(rutaInicial, fileDTO.PathArchivo);
                }
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
                response = await ValidationsOfBusiness(entity, false, true);
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
                entity.PorcentajeCumplimiento = GetPorcentajeCumplimiento(entity.CycleTimePlaneado.Value, entity.CycleTimeReal);
            }
            return entity;
        }

        private byte GetPorcentajeCumplimiento(short cycleTimePlaneado, short? cycleTimeReal)
        {
            var calculo = (cycleTimePlaneado * 100) / cycleTimeReal.Value;
            return (byte)calculo;
        }

        public async Task<ResponseDTO> ValidationsOfBusiness(Solicitud solicitud, bool IsCreated = false, bool IsEditStatus = false)
        {
            try
            {
                var response = new ResponseDTO
                {
                    IsSuccess = true,
                    Message = "Validaciones correctas."
                };
                if (solicitud.EstadoId == (int)EstadosSolicitudEnum.Finalizado && solicitud.Avance < 100)
                    return Responses.SetErrorResponse($"El avance en estado {EnumConfig.GetDescription(EstadosSolicitudEnum.Finalizado)} debe ser del 100%");

                if (!IsEditStatus)
                {
                    var existeResponsable = await Context.TablaResponsables.AnyAsync(x => x.Id == solicitud.ResponsableId);
                    if (!existeResponsable)
                        return Responses.SetErrorResponse("No existe el responsable.");

                    if (!IsCreated)
                    {
                        var existeSprint = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintInicioId);
                        if (!existeSprint)
                            return Responses.SetErrorResponse("No existe el sprint inicio.");

                        var existeSprintfin = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintFinId);
                        if (!existeSprintfin)
                            return Responses.SetErrorResponse("No existe el sprint fin.");

                    }

                    var existeCelula = await Context.TablaCelulas.AnyAsync(x => x.Id == solicitud.CelulaId);
                    if (!existeCelula)
                        return Responses.SetErrorResponse("No existe la célula.");
                }

                if (IsEditStatus && solicitud.EstadoId == (int)EstadosSolicitudEnum.Finalizado)
                {
                    bool hasSprintInicio = solicitud.SprintInicioId.HasValue;
                    bool hasSprintFin = solicitud.SprintFinId.HasValue;
                    bool tieneSprints = hasSprintInicio && hasSprintFin;
                    if (!tieneSprints)
                        return Responses.SetErrorResponse("Debe asignar sprint de inicio y sprint final en la sección de editar, para calcular el porcentaje de cumplimiento.");
                }

                var existeEstado = await Context.TablaEstados.AnyAsync(x => x.Id == solicitud.EstadoId);
                if (!existeEstado)
                    return Responses.SetErrorResponse("No existe el estado.");

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