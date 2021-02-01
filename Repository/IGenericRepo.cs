using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace klassycafe.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        T FindByID(object EntityID);
        IEnumerable<T> Select(Expression<Func<T, bool>> Filter = null);
        void Insert(T Entity);
        void Update(T Entity);
        T FindByLambda(Expression<Func<T, bool>> Filter = null);
        void Delete(object EntityID);
        void Delete(T Entity);
        void Save();
    }
}