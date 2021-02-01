using klassycafe.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;

namespace klassycafe.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private klassycafeEntities _context;
        private DbSet<T> _dbSet;
        private bool _disposed = false;

        public GenericRepo(klassycafeEntities context)
        {
            _context = context;
            _dbSet = _context.Set<T>();;
        }

        public void Delete(object EntityID)
        {
            T entityToDelete = _dbSet.Find(EntityID);
            Delete(entityToDelete);
        }

        public void Delete(T Entity)
        {
            if (_context.Entry(Entity).State == EntityState.Detached)
            {
                _dbSet.Attach(Entity);
            }
            _dbSet.Remove(Entity);
        }

        public T FindByID(object EntityID)
        {
            return _dbSet.Find(EntityID);
        }

        public T FindByLambda(System.Linq.Expressions.Expression<Func<T, bool>> Filter = null)
        {
            if (Filter != null)
            {
                return _dbSet.FirstOrDefault(Filter);
            }
            return null;
        }

        public void Insert(T Entity)
        {
            _dbSet.Add(Entity);
        }

        public IEnumerable<T> Select(System.Linq.Expressions.Expression<Func<T, bool>> Filter = null)
        {
            if (Filter != null)
            {
                return _dbSet.Where(Filter);
            }
            return _dbSet;
        }

        public void Update(T Entity)
        {
            _dbSet.Attach(Entity);
            _context.Entry(Entity).State = EntityState.Modified;
        }

        public void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            using (TransactionScope tScope = new TransactionScope())
            {
                _context.SaveChanges();
                tScope.Complete();
            }
        }
    }
}