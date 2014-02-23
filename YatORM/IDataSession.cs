using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YatORM
{
    public interface IDataSession
    {
        IEnumerable<TEntity> FindAll<TEntity>() where TEntity : new();

        IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> queryExpression) where TEntity : new();

        TEntity Find<TEntity>(Expression<Func<TEntity, bool>> queryExpression) where TEntity : new();

        IEnumerable<TEntity> GetFromProcedure<TEntity>(string procedureName, dynamic parameters = null) where TEntity : new();

        int InvokeProcedure(string procedureName, object parameters = null);

        IEnumerable<TEntity> GetFromQuery<TEntity>(string query, dynamic parameters = null) where TEntity : new();

        bool Insert<TEntity>(TEntity item);

        bool Delete<TEntity>(Expression<Func<TEntity, bool>> queryExpression);

        bool Update<TEntity>(Expression<Func<TEntity, bool>> queryExpression, TEntity entity);
    }
}