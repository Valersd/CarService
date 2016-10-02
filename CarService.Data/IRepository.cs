using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CarService.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Update(T entity, params Expression<Func<T, object>>[] excludProp);

        void Delete(T entity);

        void Delete(object id);

        void Detach(T entity);

        void Attach(T entity);

    }
}
