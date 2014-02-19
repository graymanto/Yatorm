using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YatORM
{
    public interface IDataSession
    {
        IEnumerable<TEntity> FindAll<TEntity>() where TEntity : new();

        TEntity Find<TEntity>(Expression<Func<TEntity, bool>> func) where TEntity : new();
    }
}