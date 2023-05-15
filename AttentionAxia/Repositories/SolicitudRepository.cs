using AttentionAxia.Core.Data;
using AttentionAxia.DTOs;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using log4net;
using System;
using System.Data.Entity;
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
                          SprintInicioFechaGeneracion = m.sprintInicio.FechaGeneracion,
                          SprintFinFechaGeneracion = m.sprintFin.FechaGeneracion,
                          NombreArchivo = m.solicitud.NombreArchivo,
                          RutaArchivo = m.solicitud.RutaArchivo,
                          LeadTime = m.solicitud.LeadTime,
                          CycleTime = m.solicitud.CycleTime,
                          FechaCreacion = m.solicitud.FechaCreacionSolicitud,
                          FechaComienzo = m.solicitud.FechaComienzoSolicitud,
                          FechaFinalizacion = m.solicitud.FechaFinalizacionSolicitud
                      }).ToListAsync();
                    foreach (var item in listado)
                    {
                        item.LeadTime = !item.LeadTime.HasValue ? GetLeadTime(item.FechaCreacion) : item.LeadTime;
                        item.CycleTime = !item.CycleTime.HasValue ? GetCycleTime(item.FechaComienzo) : item.CycleTime;
                    }

                    var totalDeRegistros = query.Count();
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

        public short? GetLeadTime(DateTime dateCreated)
        {
            var dateCurrent = DateTime.Now;
            var dateInitial = new DateTime(dateCreated.Year, dateCreated.Month, dateCreated.Day, 0, 0, 0);
            var dateFinal = new DateTime(dateCurrent.Year, dateCurrent.Month, dateCurrent.Day, 0, 0, 0).AddHours(24).AddSeconds(-1);
            var datesHolidays = Context.TablaFestivosColombia.Where(f => f.FechaFestivo >= dateInitial && f.FechaFestivo <= dateFinal).ToList();
            var totalDays = dateCurrent.Subtract(dateCreated).Days + 1;
            var businessDays = Enumerable.Range(0, totalDays).Select(i => dateInitial.AddDays(i)).
                Count(date => !datesHolidays.Any(y => y.FechaFestivo == date) && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);
            return (short)businessDays;
        }


        public short? GetCycleTime(DateTime? dateInProcess)
        {
            if (!dateInProcess.HasValue)
            {
                return null;
            }
            var dateCurrent = DateTime.Now;
            var dateInitial = new DateTime(dateInProcess.Value.Year, dateInProcess.Value.Month, dateInProcess.Value.Day, 0, 0, 0);
            var dateFinal = new DateTime(dateCurrent.Year, dateCurrent.Month, dateCurrent.Day, 0, 0, 0).AddHours(24).AddSeconds(-1);
            var datesHolidays = Context.TablaFestivosColombia.Where(f => f.FechaFestivo >= dateInitial && f.FechaFestivo <= dateFinal).ToList();
            var totalDays = dateCurrent.Subtract(dateInProcess.Value).Days + 1;
            var businessDays = Enumerable.Range(0, totalDays).Select(i => dateInitial.AddDays(i)).
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
                    FechaInicioSprint = solicitud.FechaInicialParse,
                    FechaFinSprint = solicitud.FechaFinalParse.AddHours(24).AddSeconds(-1),
                    Iniciativa = solicitud.Iniciativa,
                    CelulaId = solicitud.CelulaId,
                    FechaCreacionSolicitud = DateTime.Now,
                    Avance = 0
                };
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
                    {
                        return Responses.SetErrorResponse(response.Message);
                    }
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
                FileHelper.DeleteFile(rutaInicial, fileDTO.PathArchivo);
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
                entity.FechaInicioSprint = solicitud.FechaInicialParse;
                entity.FechaFinSprint = solicitud.FechaFinalParse.AddHours(24).AddSeconds(-1);
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
                        return Responses.SetCreateResponse();
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

        private Solicitud InsertDateByState(Solicitud entity)
        {
            if (entity.EstadoId == (int)EstadosSolicitudEnum.EnProgreso && !entity.FechaComienzoSolicitud.HasValue)
            {
                entity.FechaComienzoSolicitud = DateTime.Now;
            }
            else if (entity.EstadoId == (int)EstadosSolicitudEnum.Finalizado && !entity.FechaFinalizacionSolicitud.HasValue)
            {
                entity.FechaFinalizacionSolicitud = DateTime.Now;
            }
            return entity;
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