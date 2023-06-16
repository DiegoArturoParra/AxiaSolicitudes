using AttentionAxia.Core.Data;
using AttentionAxia.ExternalServices;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public class FestivosRepository : GenericRepository<Festivo>
    {
        public FestivosRepository(AxiaContext context) : base(context)
        {
        }
        public async Task<ResponseDTO> InsertHolidays()
        {
            List<Festivo> festivos = new List<Festivo>();

            var yearCurrent = DateTime.Now;
            var holidays = new PublicHolidayService();

            var fechaFinal = yearCurrent.AddYears(2).AddHours(24).AddSeconds(-1);
            var existenFestivos = await AnyWithCondition(y => y.FechaFestivo >= yearCurrent && y.FechaFestivo <= fechaFinal);
            if (existenFestivos)
                return Responses.SetErrorResponse("Ya existen los festivos para los años asignados.");

            var festivosInicial = await holidays.DatesHoliday(yearCurrent.Year);
            var festivosNext1 = await holidays.DatesHoliday(yearCurrent.AddYears(1).Year);
            var festivosNext2 = await holidays.DatesHoliday(yearCurrent.AddYears(2).Year);

            festivos.AddRange(festivosInicial);
            festivos.AddRange(festivosNext1);
            festivos.AddRange(festivosNext2);

            Context.TablaFestivosColombia.AddRange(festivos);
            await Save();
            return Responses.SetCreateResponse($"Se han creado satisfactoriamente los días festivos del año {yearCurrent.Year},{yearCurrent.AddYears(1).Year} y {yearCurrent.AddYears(2).Year}");
        }
    }
}