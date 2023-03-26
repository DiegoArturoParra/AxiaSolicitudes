using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        T FindById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Save();
        Task<bool> AnyWithCondition(Expression<Func<T, bool>> whereCondition);
        Task<T> GetWithCondition(Expression<Func<T, bool>> whereCondition);
    }

}