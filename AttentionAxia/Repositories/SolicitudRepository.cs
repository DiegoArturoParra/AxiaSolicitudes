using AttentionAxia.Core.Data;
using AttentionAxia.DTOs;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using log4net;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.UI;

namespace AttentionAxia.Repositories
{
    public class SolicitudRepository : GenericRepository<Solicitud>
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SolicitudRepository(AxiaContext context) : base(context)
        {
        }

        public async Task<ResponseDTO> ValidationsOfBusiness(CreateSolicitudDTO solicitud)
        {
            try
            {
                var existeResponsable = await Context.TablaResponsables.AnyAsync(x => x.Id == solicitud.ResponsableId);
                if (!existeResponsable)
                    return Responses.SetErrorResponse("No se encuentra el responsable");

                var existeEstado = await Context.TablaEstados.AnyAsync(x => x.Id == solicitud.EstadoId);
                if (!existeEstado)
                    return Responses.SetErrorResponse("No se encuentra el estado");

                var existeSprint = await Context.TablaSprints.AnyAsync(x => x.Id == solicitud.SprintId);
                if (!existeSprint)
                    return Responses.SetErrorResponse("No se encuentra el sprint");

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
                             join estado in Context.TablaEstados on solicitud.EstadoId equals estado.Id
                             join sprint in Context.TablaSprints on solicitud.SprintId equals sprint.Id
                             join responsable in Context.TablaResponsables on solicitud.ResponsableId equals responsable.Id
                             select new
                             {
                                 solicitud,
                                 estado,
                                 sprint,
                                 responsable
                             });
                if (filtro.EstadoSolicitudId.HasValue)
                {
                    query = query.Where(x => x.solicitud.EstadoId == filtro.EstadoSolicitudId.Value);
                }
                if (filtro.ResponsableId.HasValue)
                {
                    query = query.Where(x => x.solicitud.ResponsableId == filtro.ResponsableId.Value);
                }
                if (filtro.SprintId.HasValue)
                {
                    query = query.Where(x => x.solicitud.SprintId == filtro.SprintId.Value);
                }
                if (filtro.CelulaId.HasValue)
                {
                    query = query.Where(x => x.solicitud.Responsable.CelulaPerteneceId == filtro.CelulaId.Value);
                }
                if (filtro.LineaId.HasValue)
                {
                    query = query.Where(x => x.solicitud.Responsable.LineaPerteneceId == filtro.LineaId.Value);
                }

                var listado = await query.OrderBy(x => x.solicitud.Id)
                        .Skip((filtro.Paginacion.Page - 1) * filtro.Paginacion.Page)
                        .Take(filtro.Paginacion.PageSize).Select(m => new SolicitudDTO
                        {
                            Id = m.solicitud.Id,
                            ColorEstado = m.estado.Nivel,
                            Avance = m.solicitud.Avance,
                            Celula = Context.TablaCelulas.Where(y => y.Id == m.responsable.CelulaPerteneceId).Select(s => s.Descripcion).FirstOrDefault(),
                            Estado = m.estado.Descripcion,
                            EstadoId = m.estado.Id,
                            Linea = Context.TablaLineas.Where(y => y.Id == m.responsable.LineaPerteneceId).Select(s => s.Descripcion).FirstOrDefault(),
                            Iniciativa = m.solicitud.Iniciativa,
                            FechaInicial = m.solicitud.FechaInicioSprint,
                            Responsable = m.responsable.Nombres,
                            Sprint = m.sprint.Sigla + " " + m.sprint.Periodo,
                            FechaFinal = m.solicitud.FechaFinSprint
                        }).ToListAsync();


                var totalDeRegistros = query.Count();
                var modelo = new ListarSolicitudDTO();
                modelo.Solicitudes = listado;
                modelo.PaginaActual = filtro.Paginacion.Page;
                modelo.TotalDeRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = filtro.Paginacion.PageSize;
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