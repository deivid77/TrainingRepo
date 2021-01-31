using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public class EFRepository : IRepository
    {

        DbContext Context;

        // Recibamos una instancia del contexto al crear la clase
        public EFRepository(DbContext context)
        {
            this.Context = context;
        }

        public TEntity Create<TEntity>(TEntity toCreate) where TEntity : class
        {
            TEntity Result = default(TEntity);
            try
            {
                Context.Set<TEntity>().Add(toCreate);
                Context.SaveChanges();
                Result = toCreate;
            }
            catch(Exception ex)
            {
                string excepcion = ex.Message.ToString();   //TODO: Logging
            }
            return Result;
        }
        public bool Delete<TEntity>(TEntity toDelete) where TEntity : class
        {
            bool Result = false;
            try
            {
                Context.Entry<TEntity>(toDelete).State = EntityState.Deleted;
                Result = Context.SaveChanges() > 0;
            }
            catch { }
            return Result;
        }

        public bool Update<TEntity>(TEntity toUpdate) where TEntity : class
        {
            bool Result = false;
            try
            {
                Context.Entry<TEntity>(toUpdate).State =
                EntityState.Modified;
                Result = Context.SaveChanges() > 0;
            }
            catch { }
            return Result;
        }

        public TEntity Retrieve<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            TEntity Result = null;
            try
            {
                Result = Context.Set<TEntity>().FirstOrDefault(criteria);
            }
            catch { }
            return Result;
        }

        public List<TEntity> Filter<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            List<TEntity> Result = null;
            try
            {
                return Context.Set<TEntity>().Where(criteria).ToList();
            }
            catch { }
            return Result;
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
