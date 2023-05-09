using AttentionAxia.Core.Data;
using AttentionAxia.ExternalServices;
using AttentionAxia.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public class FestivosRepository : GenericRepository<Festivo>
    {
        public FestivosRepository(AxiaContext context) : base(context)
        {
        }
        public async Task InsertHolidays()
        {
            List<Festivo> festivos = new List<Festivo>();

            var holidays = new PublicHolidayService();
            var festivos2023 = await holidays.DatesHoliday(2023);
            var festivos2024 = await holidays.DatesHoliday(2024);
            var festivos2025 = await holidays.DatesHoliday(2025);

            festivos.AddRange(festivos2023);
            festivos.AddRange(festivos2024);
            festivos.AddRange(festivos2025);

            Context.TablaFestivosColombia.AddRange(festivos);
            await Save();
        }
    }
}