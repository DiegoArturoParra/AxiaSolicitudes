using AttentionAxia.Core.Data;
using AttentionAxia.Core.Extensions;
using AttentionAxia.DTOs;
using AttentionAxia.DTOs.Filters;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public class SprintRepository : GenericRepository<Sprint>
    {
        public SprintRepository(AxiaContext context) : base(context)
        {

        }

        public async Task<ResponseDTO> CreateMultipeSprints(CreateSprintDTO sprint)
        {
            List<Sprint> sprints = new List<Sprint>();
            ResponseDTO response = new ResponseDTO { IsSuccess = true };

            var periodos = new Dictionary<string, string>
            {
                { "1Q", "0" },
                { "2Q", "0" },
                { "3Q", "0" },
                { "4Q", "0" }
            };

            foreach (var periodo in periodos.Keys.ToList())
            {
                var ultimaSiglaPeriodo = Table
                    .Where(x => x.Periodo == periodo)
                    .OrderByDescending(x => x.Id)
                    .Take(1)
                    .Select(x => x.Sigla)
                    .FirstOrDefault();

                if (ultimaSiglaPeriodo != null)
                {
                    periodos[periodo] = ultimaSiglaPeriodo.Split('-')[1];
                }
            }

            int valorSiglaFinal = Convert.ToInt32(periodos[sprint.Periodo]);

            var filtroFechaInicial = new DateTime(DateTime.Now.Year, 1, 1);
            var filtroFechaFinal = new DateTime(DateTime.Now.Year, 12, 31).AddDays(1).AddSeconds(-1);

            for (int i = 0; i < sprint.CantidadSprints; i++)
            {
                valorSiglaFinal++;

                var sigla = sprint.Sigla.ToUpper() + "-" + valorSiglaFinal.ToString();
                var existe = await AnyWithCondition(x => x.Sigla == sigla && x.Periodo == sprint.Periodo && x.FechaGeneracion >= filtroFechaInicial && x.FechaGeneracion <= filtroFechaFinal);

                if (existe)
                {
                    response.Message = $"Ya existe sprints del periodo {sprint.Periodo} - {DateTime.Now.Year}";
                    response.IsSuccess = false;
                    break;
                }

                var fechaInicio = sprint.FechaInicialParse.AddWeeks(i * sprint.DuracionSprint);
                var nuevoSprint = new Sprint
                {
                    Periodo = sprint.Periodo,
                    Sigla = sigla,
                    FechaInicio = i == 0 ? sprint.FechaInicialParse : fechaInicio,
                    FechaGeneracion = DateTime.Now,
                    IsActivo = true
                };

                nuevoSprint.FechaFin = sprint.DuracionSprint > 1 ? nuevoSprint.FechaInicio.Value.AddWeeks(sprint.DuracionSprint)
                    .AddDays(-3) : nuevoSprint.FechaInicio.Value.AddDays(4);

                sprints.Add(nuevoSprint);
            }

            if (response.IsSuccess)
            {
                Context.TablaSprints.AddRange(sprints);
                await Save();
                response = Responses.SetCreateResponse($"Se crearon satisfactoriamente los {sprint.CantidadSprints} Sprints.");
            }

            return response;
        }

        public async Task<ResponseDTO> DeleteMultipleSprints(string year, string period)
        {
            bool success = int.TryParse(year, out int number);
            DateTime rangoFechaInicial = new DateTime(number, 01, 01, 0, 0, 0, 0);
            DateTime rangoFechaFinal = new DateTime(number, 12, 31, 0, 0, 0, 0).AddHours(24).AddSeconds(-1);
            var listado = await Table.Where(x => x.Periodo.Equals(period) && x.FechaGeneracion >= rangoFechaInicial && x.FechaGeneracion <= rangoFechaFinal)
                .ToListAsync();


            if (!listado.Any())
                return Responses.SetErrorResponse($"No hay sprints para el periodo {period}-{year}");

            if (await Context.TablaSolicitudes.AnyAsync())
            {
                var listaIds = listado.Select(y => y.Id).ToList();
                var haySprintsConTareas = await Context.TablaSolicitudes
                    .Where(x => listaIds.Contains(x.SprintInicioId) || listaIds.Contains(x.SprintFinId))
                    .Select(x => x.Id)
                    .AnyAsync();
                if (haySprintsConTareas)
                    return Responses.SetErrorResponse($"hay sprints para el periodo {period}-{year} vinculados en una iniciativa.");

            }
            Context.TablaSprints.RemoveRange(listado);
            await Save();
            return Responses.SetOkResponse($"Se eliminaron los sprints del periodo {period}-{year}");
        }

        public async Task<DateTime?> GetDateBySprint(bool IsDateInitial, int sprintId)
        {
            if (IsDateInitial)
            {
                return await Table.Where(x => x.Id == sprintId).Select(y => y.FechaInicio).FirstOrDefaultAsync();
            }
            else
            {
                return await Table.Where(x => x.Id == sprintId).Select(y => y.FechaFin).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Sprint>> GetSprintsByFilter(SprintFilterDTO filtro)
        {
            var data = Table;
            if (!string.IsNullOrWhiteSpace(filtro.Period))
            {
                data = data.Where(x => x.Periodo == filtro.Period);
            }

            int numberYear;
            bool success = int.TryParse(filtro.Year, out numberYear);
            if (success)
            {
                DateTime fechaInicial = new DateTime(numberYear, 01, 01, 0, 0, 0, 0);
                DateTime fechaFinal = new DateTime(numberYear, 12, 31, 0, 0, 0, 0).AddHours(24).AddSeconds(-1);
                data = data.Where(x => x.FechaGeneracion >= fechaInicial && x.FechaGeneracion <= fechaFinal);
            }
            return await data.ToListAsync();
        }

        public async Task<ResponseDTO> EditStatusMultipleSprints(string year, string period, bool isActivo)
        {
            int number;
            bool success = int.TryParse(year, out number);
            DateTime fechaInicial = new DateTime(number, 01, 01, 0, 0, 0, 0);
            DateTime fechaFinal = new DateTime(number, 12, 31, 0, 0, 0, 0).AddHours(24).AddSeconds(-1);

            var (activo, mensajeError, mensajeOk) = !isActivo ? (true, "Activos", "inactivaron") : (false, "Inactivos", "activaron");
            var listado = await Table.Where(x => x.Periodo.Equals(period) && x.FechaGeneracion >= fechaInicial && x.FechaGeneracion <= fechaFinal
                                             && x.IsActivo == activo).ToListAsync();


            if (!listado.Any())
                return Responses.SetErrorResponse($"No hay sprints {mensajeError} para el periodo {period}-{year}");

            foreach (var sprint in listado)
            {
                sprint.IsActivo = isActivo;
                SetEntryModified(sprint);
            }

            await Save();
            return Responses.SetOkResponse($"Se {mensajeOk} {listado.Count} sprints del periodo {period}-{year}");
        }
    }
}