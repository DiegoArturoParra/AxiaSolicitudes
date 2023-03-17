using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Save();
    }

}