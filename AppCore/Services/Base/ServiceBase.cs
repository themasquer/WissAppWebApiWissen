using AppCore.Repository;
using AppCore.Repository.Base;
using AppCore.UnitOFwork;
using AppCore.UnitOFwork.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AppCore.Services.Base
{
    public abstract class ServiceBase<TEntity> : IDisposable where TEntity : class, new() 
    {
        private DbContext db;
        protected RepositoryBase<TEntity> repository;
        protected UnitOfWorkBase unitOfWork;

        public ServiceBase(DbContext _db)
        {
            db = _db;
            repository = new Repository<TEntity>(db);
            unitOfWork = new UnitOfWork(db);
        }

        // Repository üzerinden herhangi bir filtre olmadan tüm entity'leri liste olarak döner
        public virtual List<TEntity> GetEntities()
        {
            try
            {
                
                return repository.GetEntities();
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository üzerinden parametre olarak gönderilen lambda expression'a göre entity'leri liste olarak döner 
        public virtual List<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                
                return repository.GetEntities(predicate);
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository üzerinden entity için herhangi bir kayıt döndürmeyen bir LINQ query oluşturur ve bu query'i döner
        public virtual IQueryable<TEntity> GetEntityQuery()
        {
            try
            {
                
                return repository.GetEntityQuery();
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository üzerinden parametre olarak gönderilen lambda expression'a göre entity için kayıt döndürmeyen bir LINQ query oluşturur ve bu query'i döner 
        public virtual IQueryable<TEntity> GetEntityQuery(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                
                return repository.GetEntityQuery(predicate);
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository üzerinden lazy loading yapılmayan durumlarda entity için herhangi bir kayıt döndürmeyen bir LINQ query oluşturur, parametre olarak gönderilen bu entity'ye bağlı entity'leri query'ye ekler ve bu query'i döner
        public virtual IQueryable<TEntity> GetEntityQuery(string[] entitiesToInclude)
        {
            try
            {
                
                return repository.GetEntityQuery(entitiesToInclude);
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository üzerinden primary key olan ID'ye göre tek bir entity döner
        public virtual TEntity GetEntity(int id)
        {
            try
            {
                
                return repository.GetEntity(id);
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository üzerinden parametre olarak gönderilen lambda expression'a göre tek bir entity döner
        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                
                return repository.GetEntity(predicate);
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Parametre olarak gönderilen entity'i repository'e ekler ve unit of work üzerinden veritabanına kaydetme işlemini gerçekleştirir
        public virtual TEntity AddEntity(TEntity entity, bool saveChanges = true)
        {
            try
            {
                
                
                repository.AddEntity(entity);
                if (saveChanges)
                    unitOfWork.SaveChanges();
                return entity;
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Parametre olarak gönderilen entity'i repository'de günceller ve unit of work üzerinden veritabanına kaydetme işlemini gerçekleştirir
        public virtual TEntity UpdateEntity(TEntity entity, bool saveChanges = true)
        {
            try
            {
                
                
                repository.UpdateEntity(entity);
                if (saveChanges)
                    unitOfWork.SaveChanges();
                return entity;
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Parametre olarak gönderilen primary key olan ID'ye sahip entity'i repository'den siler ve unit of work üzerinden veritabanına kaydetme işlemini gerçekleştirir
        public virtual int DeleteEntity(int id, bool saveChanges = true)
        {
            try
            {
                
                
                repository.DeleteEntity(id);
                if (saveChanges)
                    unitOfWork.SaveChanges();
                return id;
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Parametre olarak gönderilen entity'i veritabanından siler ve unit of work üzerinden veritabanına kaydetme işlemini gerçekleştirir
        public virtual TEntity DeleteEntity(TEntity entity, bool saveChanges = true)
        {
            try
            {
                
                
                repository.DeleteEntity(entity);
                if (saveChanges)
                    unitOfWork.SaveChanges();
                return entity;
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository'de parametre olarak gönderilen lambda expression'ı karşılayan kayıt olup olmadığını boolean olarak döner
        public virtual bool EntityExists(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                
                if (repository.GetEntityQuery().FirstOrDefault(predicate) == null)
                    return false;
                return true;
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository'deki entity'lerin sayısını döner
        public virtual int GetEntityCount()
        {
            try
            {
                
                return repository.GetEntityQuery().Count();
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        // Repository'deki entity'lerin sayısını gönderilen lambda expression parametresine göre döner
        public virtual int GetEntityCount(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                
                return repository.GetEntityQuery().Count(predicate);
            }
            catch (Exception exc)
            {
                
                throw exc;
            }
        }

        public virtual void SaveChanges()
        {
            try
            {
                unitOfWork.SaveChanges();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #region Dispose
        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                if (unitOfWork != null)
                    unitOfWork.Dispose();
                if (repository != null)
                    repository.Dispose();
                if (db != null)
                    db.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
