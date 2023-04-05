using AttentionAxia.Core.Data;
using AttentionAxia.Models;

namespace AttentionAxia.Repositories
{
    public class EstadoRepository : GenericRepository<EstadoSolicitud>
    {
        public EstadoRepository(AxiaContext context) : base(context)
        {

        }
    }
}