using AppCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services
{
    public class Service<TEntity> : ServiceBase<TEntity> where TEntity : class, new()
    {
        public Service(DbContext _db) : base(_db)
        {

        }
    }
}
