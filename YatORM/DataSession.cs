using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YatORM
{
    public class DataSession : IDataSession
    {
        private readonly YatDB _db;

        private readonly QueryTranslator _translator = new QueryTranslator();

        public DataSession(string connectionString)
        {
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

        public IEnumerable<TEntity> FindAllQuery<TEntity>(Expression<Func<IEnumerable<TEntity>, IEnumerable<TEntity>>> queryExpression) where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = "select * from " + typeof(TEntity).Name + querySql;

            return _db.GetCommand<TEntity>(fullSql);
        }

        public TEntity Find<TEntity>(Expression<Func<TEntity, bool>> queryExpression) where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = "select * from " + typeof(TEntity).Name + " where " + querySql;

            return _db.GetSingleItem<TEntity>(fullSql);
        }

        public IEnumerable<TEntity> GetFromProcedure<TEntity>(
            string procedureName, dynamic parameters = null) where TEntity : new()
        {
            return _db.ExecStoredProc<TEntity>(procedureName, parameters);
        }

        public int InvokeProcedure(string procedureName, object parameters = null)
        {
            return _db.ExecNonQueryProc(procedureName, parameters);
        }

        public IEnumerable<TEntity> GetFromQuery<TEntity>(string query, dynamic parameters = null) where TEntity : new()
        {
            return _db.GetCommand<TEntity>(query, parameters);
        }

        public bool Insert<TEntity>(TEntity item)
        {
            var sqlTemplate = SqlGenerator.GetInsertStatementForEntity<TEntity>();

            return _db.ExecuteNonQuery(sqlTemplate, item) == 1;
        }

        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> queryExpression)
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = "delete from " + typeof(TEntity).Name + " where " + querySql;

            return _db.ExecuteNonQuery(fullSql) == 1;
        }

        public bool Update<TEntity>(Expression<Func<TEntity, bool>> queryExpression, TEntity entity)
        {
            var querySql = _translator.Translate(queryExpression);
            IEnumerable<string> excludeColumns = new[] { "Id" };
            var sqlTemplate = SqlGenerator.GetUpdateStatementForEntity(entity, excludeColumns);

            var fullSql = sqlTemplate + " where " + querySql;

            var parameters = DBToTypeConverter.TransformClassToSqlParameters(entity, excludeColumns);

            return _db.ExecuteNonQuery(fullSql, parameters) == 1;
        }
    }
}