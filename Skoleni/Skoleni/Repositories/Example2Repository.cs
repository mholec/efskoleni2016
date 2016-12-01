using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skoleni.Entities;

namespace Skoleni.Repositories
{
    /// <summary>
    /// Ukázka práce s daty, ukládání a change tracking
    /// </summary>
    public class Example2Repository
    {
        private readonly AppContext _db;

        public Example2Repository(AppContext db)
        {
            _db = db;
        }
    }
}