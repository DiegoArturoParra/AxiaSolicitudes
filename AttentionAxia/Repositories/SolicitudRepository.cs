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

        public async Task<ResponseDTO> InsertWithArchive(Solicitud entity, HttpPostedFileBase file, string rutaInicial)
        {
            FileDTO fileDTO = null;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    FileHelper.FolderIsExist(rutaInicial, GetConstants.CARPETA_ARCHIVOS_SOLICITUDES);
                    var response = FileHelper.SaveFile(file, rutaInicial, GetConstants.CARPETA_ARCHIVOS_SOLICITUDES, file.FileName);
                    if (!response.IsSuccess)
                    {
                        return Responses.SetErrorResponse(response.Message);
                    }
                    fileDTO = (FileDTO)response.Data;
                    entity.RutaArchivo = fileDTO.PathArchivo;
                    entity.NombreArchivo = fileDTO.NombreArchivo;
                    Insert(entity);
                    await Save();
                    return Responses.SetCreateResponse();
                }
                else
                {
                    Insert(entity);
                    await Save();
                    return Responses.SetCreateResponse();
                }
            }
            catch (Exception ex)
            {
                FileHelper.DeleteFile(rutaInicial, fileDTO.PathArchivo);
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
            }
        }

        public async Task<ResponseDTO> UpdateWithArchive(Solicitud entity, HttpPostedFileBase file, string rutaInicial)
        {
            try
            {
                Update(entity);
                await Save();
                return Responses.SetOkResponse("Edición satisfactoriamente.");
            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
            }
        }

        public async Task<ResponseDTO> ValidationsOfBusiness(CreateSolicitudDTO solicitud)
        {
            try
            {
                var existeResponsable = await Context.TablaResponsables.AnyAsync(x => x.Id == solicitud.ResponsableId);
                if (!existeResponsable)
                    return Responses.SetErrorResponse("No existe el responsable");

                var existeEstado = await Context.TablaEstados.AnyAsync(x => x.Id == solicitud.EstadoId);
                if (!existeEstado)
                    return Responses.SetErrorResponse("No existe el estado");

                var existeSprint = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintInicioId);
                if (!existeSprint)
                    return Responses.SetErrorResponse("No existe el sprint inicio");

                var existeSprintfin = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintFinId);
                if (!existeSprintfin)
                    return Responses.SetErrorResponse("No existe el sprint fin");

                var existeCelula = await Context.TablaCelulas.AnyAsync(x => x.Id == solicitud.CelulaId);
                if (!existeCelula)
                    return Responses.SetErrorResponse("No existe la célula");

                return Responses.SetOkResponse("validado");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Responses.SetInternalServerErrorResponse(ex, ex.Message);
                throw;
            }

        }

        public async Task<ListarSolicitudDTO> GetSolicitudes(SolicitudFilterDTO filtro)
        {
            try
            {
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
                                 celula
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
                            FechaInicial = m.solicitud.FechaInicioSprint,
                            Responsable = m.responsable.Nombres,
                            SprintInicio = m.sprintInicio.Sigla + " " + m.sprintInicio.Periodo,
                            SprintFin = m.sprintFin.Sigla + " " + m.sprintFin.Periodo,
                            FechaFinal = m.solicitud.FechaFinSprint,
                            SprintInicioFechaGeneracion = m.sprintInicio.FechaGeneracion,
                            SprintFinFechaGeneracion = m.sprintFin.FechaGeneracion

                        }).ToListAsync();


                var totalDeRegistros = query.Count();
                var modelo = new ListarSolicitudDTO();
                modelo.Solicitudes = listado;
                modelo.PaginaActual = filtro.Page;
                modelo.TotalDeRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = filtro.PageSize;
                modelo.ValoresQueryString = new RouteValueDictionary();
                modelo.ValoresQueryString["FechaInicial"] = filtro.FechaInicial;
                modelo.ValoresQueryString["FechaFinal"] = filtro.FechaFinal;
                modelo.ValoresQueryString["Avance"] = filtro.Avance;
                return modelo;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
                throw;
            }
        }
    }
}