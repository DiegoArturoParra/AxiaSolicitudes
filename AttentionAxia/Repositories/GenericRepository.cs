﻿using AttentionAxia.Core;
using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttentionAxia.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {

        private IDbSet<T> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        private readonly bool _isTesting;
        private readonly bool _isProduction;
        public GenericRepository(AxiaContext context)
        {
            _isTesting = Convert.ToBoolean(ConfigurationManager.AppSettings[EnvironmentConfig.ENVIRONMENT_TESTING]);
            _isProduction = Convert.ToBoolean(ConfigurationManager.AppSettings[EnvironmentConfig.ENVIRONMENT_PRODUCTION]);
            _isDisposed = false;
            Context = context;
        }

        protected AxiaContext Context { get; set; }

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

        private AxiaContext GetContext()
        {
            string connectionString;
            if (_isProduction)
            {
                connectionString = ConfigurationManager.ConnectionStrings[EnvironmentConfig.NAME_CONNECTION_PRODUCTION].ConnectionString;
                var desencryptConnection = HashHelper.GenerateDecrypt(connectionString);
                return new AxiaContext(desencryptConnection);
            }
            else if (_isTesting)
            {
                connectionString = ConfigurationManager.ConnectionStrings[EnvironmentConfig.NAME_CONNECTION_TESTING].ConnectionString;
                var desencryptConnection = HashHelper.GenerateDecrypt(connectionString);
                return new AxiaContext(desencryptConnection);
            }
            return new AxiaContext();
        }

        #region get list all
        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.ToListAsync();

        }
        #endregion

        #region Find object by Id
        public virtual T FindById(object id)
        {
            return Entities.Find(id);
        }
        #endregion

        #region get de un objeto con condiciones 
        public async Task<T> GetWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return await Entities.Where(whereCondition).FirstOrDefaultAsync();
        }
        #endregion

        #region existe por condiciones 
        public async Task<bool> AnyWithCondition(Expression<Func<T, bool>> whereCondition)
        {
            return await Entities.AnyAsync(whereCondition);
        }
        #endregion

        #region Insert in BD 
        public virtual void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (Context == null || _isDisposed)
                Context = GetContext();
            Entities.Add(entity);
        }
        #endregion

        #region Update in BD 
        public virtual void Update(T entity)
        {

            if (entity == null)
                throw new ArgumentNullException("entity");
            if (Context == null || _isDisposed)
                Context = GetContext();
            SetEntryModified(entity);
        }
        #endregion

        #region Delete in BD 
        public virtual void Delete(T entity)
        {

            if (entity == null)
                throw new ArgumentNullException("entity");
            if (Context == null || _isDisposed)
                Context = GetContext();
            Entities.Remove(entity);
        }
        public virtual void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region Save transaction in BD 
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
                        _errorMessage += Environment.NewLine + string.Format("Propiedad: {0} Error: {1}",
                            validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        #endregion
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }
    }
}