using AttentionAxia.DTOs;
using AttentionAxia.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AttentionAxia.ExternalServices
{
    public class PublicHolidayService
    {
        public async Task<IEnumerable<Festivo>> DatesHoliday(int? year)
        {
            List<Festivo> dates = new List<Festivo>();
            using (var httpClient = new HttpClient())
            {
                if (year == null)
                {
                    year = DateTime.Now.Year;
                }
                using (var response = await httpClient.GetAsync($"https://date.nager.at/api/v3/publicholidays/{year}/CO"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var publicHolidays = JsonConvert.DeserializeObject<List<PublicHolidayDTO>>(json);
                        dates = publicHolidays.Where(x => x.Name.ToUpper() != "CARNIVAL").Select(o => new Festivo
                        {
                            FechaFestivo = o.Date
                        }).ToList();
                    }
                }
            }
            return dates;
        }
    }
}