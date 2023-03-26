using AttentionAxia.Core.Data;
using AttentionAxia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttentionAxia.Repositories
{
    public class ResponsableRepository : GenericRepository<Responsable>
    {
        public ResponsableRepository(AxiaContext context) : base(context)
        {
        }
    }
}