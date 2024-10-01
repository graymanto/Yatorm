using System.Data;
using System.Linq.Expressions;

namespace Yatorm
{
    public class DataSession(IConnectionProvider connectionProvider) : IDataSession, IDisposable
    {
        private readonly IDbConnection _connection = connectionProvider.GetConnection();

        private readonly QueryTranslator _translator = new();

        private IDbConnection OpenConnection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        public IEnumerable<TEntity> FindAll<TEntity>()
            where TEntity : new()
        {
            var query = $"select * from {typeof(TEntity).Name}";
            return OpenConnection.Query<TEntity>(query);
        }

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> queryExpression)
            where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = $"select * from {typeof(TEntity).Name} where {querySql}";

            return _connection.Query<TEntity>(fullSql);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> queryExpression)
            where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = $"select * from {typeof(TEntity).Name} where {querySql}";

            return await _connection.QueryAsync<TEntity>(fullSql);
        }

        public IEnumerable<TEntity> FindAllQuery<TEntity>(
            Expression<Func<IEnumerable<TEntity>, IEnumerable<TEntity>>> queryExpression
        )
            where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = $"select * from {typeof(TEntity).Name}{querySql}";

            return _connection.Query<TEntity>(fullSql);
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> queryExpression)
            where TEntity : new()
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = $"select * from {typeof(TEntity).Name} where {querySql}";

            return OpenConnection.Single<TEntity>(fullSql);
        }

        public Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> queryExpression)
            where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFromProcedure<TEntity>(string procedureName, dynamic? parameters = null)
            where TEntity : new()
        {
            return IDbConnectionExtensions.ExecStoredProc<TEntity>(_connection, procedureName, parameters);
        }

        public int InvokeProcedure(string procedureName, object? parameters = null)
        {
            return _connection.ExecNonQueryProc(procedureName, parameters);
        }

        public IEnumerable<TEntity> GetFromQuery<TEntity>(string query, dynamic? parameters = null)
            where TEntity : new()
        {
            EnumerableQuery queryable = IDbConnectionExtensions.Query<TEntity>(_connection, query, parameters);
            return queryable as IQueryable<TEntity> ?? Enumerable.Empty<TEntity>();
        }

        public bool Insert<TEntity>(TEntity? item)
        {
            var sqlTemplate = SqlGenerator.GetInsertStatementForEntity<TEntity>();

            return _connection.ExecuteNonQuery(sqlTemplate, item) == 1;
        }

        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> queryExpression)
        {
            var querySql = _translator.Translate(queryExpression);
            var fullSql = $"delete from {typeof(TEntity).Name} where {querySql}";

            return _connection.ExecuteNonQuery(fullSql) == 1;
        }

        public bool Update<TEntity>(Expression<Func<TEntity, bool>> queryExpression, TEntity entity)
        {
            var querySql = _translator.Translate(queryExpression);
            IEnumerable<string> excludeColumns = ["Id"];
            var sqlTemplate = SqlGenerator.GetUpdateStatementForEntity(entity, excludeColumns);

            var fullSql = sqlTemplate + " where " + querySql;

            var parameters = DBToTypeConverter.TransformClassToSqlParameters(entity, excludeColumns);

            return _connection.ExecuteNonQuery(fullSql, parameters) == 1;
        }

        public int ExecuteNonQuery(string query, object? parameters = null)
        {
            return OpenConnection.ExecuteNonQuery(query, parameters);
        }

        public Task<int> ExecuteNonQueryAsync(string query, object? parameters = null) =>
            _connection.ExecuteNonQueryAsync(query, parameters);

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
