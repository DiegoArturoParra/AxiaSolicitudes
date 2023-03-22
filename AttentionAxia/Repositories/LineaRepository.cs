using AttentionAxia.Core.Data;
using AttentionAxia.Models;

namespace AttentionAxia.Repositories
{
    public class LineaRepository : GenericRepository<Linea>
    {
        public LineaRepository(AxiaContext context) : base(context)
        {
        }
    }
}