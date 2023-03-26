using AttentionAxia.Core.Data;
using AttentionAxia.Models;

namespace AttentionAxia.Repositories
{
    public class SprintRepository : GenericRepository<Sprint>
    {
        public SprintRepository(AxiaContext context) : base(context)
        {

        }
    }
}