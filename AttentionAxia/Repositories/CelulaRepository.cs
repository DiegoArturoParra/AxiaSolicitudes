﻿using AttentionAxia.Core.Data;
using AttentionAxia.Models;

namespace AttentionAxia.Repositories
{
    public class CelulaRepository : GenericRepository<Celula>
    {
        public CelulaRepository(AxiaContext context) : base(context)
        {

        }
    }
}