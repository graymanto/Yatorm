using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YatORM
{
    public class DataSession : IDataSession
    {
        private readonly string _connectionString;

        private readonly YatDB _db;

        private readonly QueryTranslator _translator = new QueryTranslator();

        public DataSession(string connectionString)
        {
            this._connectionString = connectionString;
            this._db = new YatDB(connectionString);
        }

        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : new()
        {
            var query = "select * from " + typeof(TEntity).Name;
            return _db.GetCommand<TEntity>(query);
        }

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> queryExpression) where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = "select * from " + typeof(TEntity).Name + " where " + querySql;

            return _db.GetCommand<TEntity>(fullSql);
        }

        public TEntity Find<TEntity>(Expression<Func<TEntity, bool>> queryExpression) where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = "select * from " + typeof(TEntity).Name + " where " + querySql;

            return _db.GetSingleItem<TEntity>(fullSql);
        }
    }
}