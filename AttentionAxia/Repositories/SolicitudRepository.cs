using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AttentionAxia.Repositories
{
    public class SolicitudRepository : GenericRepository<Solicitud>
    {
        public SolicitudRepository(AxiaContext context) : base(context)
        {
        }

        public async Task<ResponseDTO> ValidationsOfBusiness(Solicitud solicitud)
        {
            return Responses.SetOkResponse("validado");
        }
    }
}