using AttentionAxia.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AttentionAxia.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {

        private IDbSet<T> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;

        public GenericRepository(AxiaContext context)
        {
            _isDisposed = false;
            Context = context;
        }

        public AxiaContext Context { get; set; }
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        private IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = Context.Set<T>();
                }
                return _entities;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.ToListAsync();

        }

        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (Context == null || _isDisposed)
                Context = new AxiaContext();
            Entities.Add(entity);
        }
        public void Update(T entity)
        {

            if (entity == null)
                throw new ArgumentNullException("entity");
            if (Context == null || _isDisposed)
                Context = new AxiaContext();
            SetEntryModified(entity);
        }

        public void Delete(T entity)
        {

            if (entity == null)
                throw new ArgumentNullException("entity");
            if (Context == null || _isDisposed)
                Context = new AxiaContext();
            Entities.Remove(entity);
        }
        public virtual void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> Save()
        {
            try
            {
                return await Context.SaveChangesAsync() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                            validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }
    }
}