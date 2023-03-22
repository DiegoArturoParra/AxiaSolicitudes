using AttentionAxia.Core.Data;
using AttentionAxia.Models;
using Microsoft.Ajax.Utilities;

namespace AttentionAxia.Repositories
{
    public class CelulaRepository : GenericRepository<Celula>
    {
        public CelulaRepository(AxiaContext context) : base(context)
        {

        }
    }
}