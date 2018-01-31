
using BusinessModel;
using System;

using System.Collections.Generic;
using System.Linq;

namespace BusinessModel
{
    /// <summary>
    /// abstract repository, basic set for the repository
    /// and some implementation of methods
    /// </summary>
    public abstract class DefaultRepository<T> : IDefaultRepository<T> where T : class
    {
        protected ElectiveContext _electiveContext;

        public DefaultRepository(ElectiveContext electiveContext)
        {
            _electiveContext = electiveContext;
        }

        public DefaultRepository()
        {
        }

        /// <summary>
        /// Method for getting all entities
        /// </summary>
        abstract public IQueryable<T> Get();

        /// <summary>
        /// Method for getting 
        /// entity by id
        /// </summary>
        abstract public T Get(Guid id);

        /// <summary>
        /// Method for storing entity
        /// </summary>
        public void Save()
        {
            _electiveContext.SaveChanges();
        }

        /// <summary>
        /// Method for removing entity
        /// </summary>
        public void Remove(T data)
        {
            _electiveContext.Set<T>().Remove(data);
            Save();

        }

        /// <summary>
        /// Method for adding entity
        /// </summary>
        public void Add(T data)
        {
            _electiveContext.Set<T>().Add(data);
            Save();
        }

        public void ChangeState<U>(U data) where U : class
        {
            _electiveContext.SetModified(data);
            
        }
    }
}
