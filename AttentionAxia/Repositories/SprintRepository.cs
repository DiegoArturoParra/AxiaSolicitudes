using AttentionAxia.Core.Data;
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

        public async Task<ResponseDTO> CreateMultipeSprints(Sprint sprint, int cantidadSprints)
        {
            List<Sprint> sprints = new List<Sprint>();
            ResponseDTO response = new ResponseDTO();
            response.IsSuccess = true;
            var res1 = Table.Where(x => x.Periodo == "1Q").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo1 = { "0", "0" };
            if (res1 != null) { ultimaSiglaPeriodo1 = res1.Split('-'); }
            var res2 = Table.Where(x => x.Periodo == "2Q").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo2 = { "0", "0" };
            if (res2 != null) { ultimaSiglaPeriodo2 = res2.Split('-'); }
            var res3 = Table.Where(x => x.Periodo == "3Q").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo3 = { "0", "0" };
            if (res3 != null) { ultimaSiglaPeriodo3 = res3.Split('-'); }
            var res4 = Table.Where(x => x.Periodo == "4Q").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo4 = { "0", "0" };
            if (res4 != null) { ultimaSiglaPeriodo4 = res4.Split('-'); }

            int valorSiglaFinal = 0;

            if (sprint.Periodo == "1Q")
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo1[1]);
            }
            else if (sprint.Periodo == "2Q")
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo2[1]);
            }
            else if (sprint.Periodo == "3Q")
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo3[1]);
            }
            else
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo4[1]);
            }
            var sig = sprint.Sigla;
            DateTime fechaInicial = new DateTime(DateTime.Now.Year, 01, 01, 0, 0, 0, 0);
            DateTime fechaFinal = new DateTime(DateTime.Now.Year, 12, 31, 0, 0, 0, 0).AddHours(24).AddSeconds(-1);
            for (int i = 0; i <= cantidadSprints - 1; i++)
            {
                valorSiglaFinal++;
                var existe = await AnyWithCondition(x => x.Sigla == sprint.Sigla + "-" + valorSiglaFinal.ToString() && x.Periodo == sprint.Periodo
                && x.FechaGeneracion >= fechaInicial && x.FechaGeneracion <= fechaFinal);
                if (existe)
                {
                    response.Message = $"Ya existe sprints de el periodo {sprint.Periodo}-{DateTime.Now.Year}";
                    response.IsSuccess = false;
                    break;
                }
                sprint.Sigla = sig.ToUpper() + "-" + valorSiglaFinal.ToString();
                sprints.Add(new Sprint()
                {
                    Periodo = sprint.Periodo,
                    Sigla = sprint.Sigla,
                    FechaGeneracion = DateTime.Now,
                    IsActivo = true,
                });
            }
            if (response.IsSuccess)
            {
                Context.TablaSprints.AddRange(sprints);
                await Save();
                response = Responses.SetCreateResponse($"Se crearon satisfactoriamente los: {cantidadSprints} Sprints.");
            }
            return response;
        }

        public async Task<ResponseDTO> DeleteMultipeSprints(string year, string period)
        {
            int number;
            bool success = int.TryParse(year, out number);
            DateTime fechaInicial = new DateTime(number, 01, 01, 0, 0, 0, 0);
            DateTime fechaFinal = new DateTime(number, 12, 31, 0, 0, 0, 0).AddHours(24).AddSeconds(-1);
            var listado = await Table.Where(x => x.Periodo.Equals(period) && x.FechaGeneracion >= fechaInicial && x.FechaGeneracion <= fechaFinal)
                .ToListAsync();


            if (listado.Count == 0)
            {
                return Responses.SetErrorResponse($"No hay sprints para el periodo {period}-{year}");
            }

            if (await Context.TablaSolicitudes.AnyAsync())
            {
                var listaIds = listado.Select(y => y.Id).ToList();
                var haySprintsConTareas = await Context.TablaSolicitudes
                    .Where(x => listaIds.Contains(x.SprintInicioId) || listaIds.Contains(x.SprintFinId))
                    .Select(x => x.Id)
                    .AnyAsync();
                if (haySprintsConTareas)
                {
                    return Responses.SetErrorResponse($"hay sprints para el periodo {period}-{year} vinculados en una iniciativa.");
                }
            }
            Context.TablaSprints.RemoveRange(listado);
            await Save();
            return Responses.SetOkResponse($"Se eliminaron los sprints del periodo {period}-{year}");
        }
    }
}