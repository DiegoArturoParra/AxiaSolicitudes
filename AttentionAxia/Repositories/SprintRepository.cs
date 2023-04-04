using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
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
            var res1 = Table.Where(x => x.Periodo == "Q1").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo1 = { "0", "0" };
            if (res1 != null) { ultimaSiglaPeriodo1 = res1.Split('-'); }
            var res2 = Table.Where(x => x.Periodo == "Q2").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo2 = { "0", "0" };
            if (res2 != null) { ultimaSiglaPeriodo2 = res2.Split('-'); }
            var res3 = Table.Where(x => x.Periodo == "Q3").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo3 = { "0", "0" };
            if (res3 != null) { ultimaSiglaPeriodo3 = res3.Split('-'); }
            var res4 = Table.Where(x => x.Periodo == "Q4").OrderByDescending(x => x.Id).Take(1).Select(x => x.Sigla).ToList().FirstOrDefault();
            string[] ultimaSiglaPeriodo4 = { "0", "0" };
            if (res4 != null) { ultimaSiglaPeriodo4 = res4.Split('-'); }

            int valorSiglaFinal = 0;

            if (sprint.Periodo == "Q1")
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo1[1]);
            }
            else if (sprint.Periodo == "Q2")
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo2[1]);
            }
            else if (sprint.Periodo == "Q3")
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo3[1]);
            }
            else
            {
                valorSiglaFinal = Convert.ToInt32(ultimaSiglaPeriodo4[1]);
            }
            var sig = sprint.Sigla;
            for (int i = 0; i <= cantidadSprints - 1; i++)
            {
                valorSiglaFinal++;
                var existe = await AnyWithCondition(x => x.Sigla == sprint.Sigla + "-" + valorSiglaFinal.ToString() && x.Periodo == sprint.Periodo);
                if (existe)
                {
                    response.Message = $"Ya existe un registro con la descripción {sprint.Sigla.ToLower()}";
                    response.IsSuccess = false;
                    break;
                }
                sprint.Sigla = sig + "-" + valorSiglaFinal.ToString();
                sprints.Add(new Sprint()
                {
                    Periodo = sprint.Periodo,
                    Sigla = sprint.Sigla,
                    FechaGeneracion = DateTime.Now,
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
            Context.TablaSprints.RemoveRange(listado);
            await Save();
            return Responses.SetOkResponse($"Se eliminaron los sprints del periodo {period}-{year}");
        }
    }
}