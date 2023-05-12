using AttentionAxia.Core.Data;
using AttentionAxia.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public class ResponsableRepository : GenericRepository<Responsable>
    {
        public ResponsableRepository(AxiaContext context) : base(context)
        {
        }

        public async Task<object> GetPersonsByLineId(int lineaId)
        {
            return await Table.Where(x => x.LineaPerteneceId == lineaId).Select(x => new
            {
                x.Id,
                Nombre = x.Nombres
            }).ToListAsync();
        }
    }
}