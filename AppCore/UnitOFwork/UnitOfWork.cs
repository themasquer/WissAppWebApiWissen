using AppCore.Repository;
using AppCore.Repository.Base;
using AppCore.UnitOFwork.Base;
using System.Data.Entity;

namespace AppCore.UnitOFwork
{
    public class UnitOfWork : UnitOfWorkBase
    {
        public UnitOfWork(DbContext _db) : base(_db)
        {
            
        }
    }
}
