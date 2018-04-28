using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessModel
{
    /// <summary>
    /// Base interface for all repositories
    /// </summary>
    public interface IDefaultRepository<T>
    {
        IEnumerable<T> Get();

        void Add(T data);

        void Remove(T data);

        T Get(Guid id);

        void Save();

        void ChangeState<U>(U data) where U : class;
    }
}
