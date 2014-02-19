using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using YatORM.Extensions;

namespace YatORM
{
    public class YatDB
    {
        private readonly string _connectionString;

        private readonly DBToTypeConverter _converter = new DBToTypeConverter();

        public YatDB()
        {
            _connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        }

        public YatDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IQueryable<TResult> GetCommand<TResult>(string query) where TResult : new()
        {
            return ExecuteMappedCommand<TResult>(query);
        }

        public TResult GetSingleItem<TResult>(string query) where TResult : new()
        {
            return ExecuteMappedCommand<TResult>(query).FirstOrDefault();
        }

        public TResult GetCommand<TResult>(string query, dynamic parameters)
        {
            return default(TResult);
        }

        public TResult ExecStoredProc<TResult>(string procname, dynamic parameters = null)
        {
            return default(TResult);
        }

        public TResult ExecScalarStoredProc<TResult>(string procname, dynamic parameters = null)
        {
            return default(TResult);
        }

        private IQueryable<TResult> ExecuteMappedCommand<TResult>(
            string commandText, 
            CommandType commandType = CommandType.Text, 
            IEnumerable<SqlParameter> parameters = null) where TResult : new()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = conn.BuildCommand(commandText, parameters, commandType);

                var rdr = cmd.ExecuteReader();

                var result = _converter.ReaderToMappedSequence<TResult>(rdr);

                conn.Close();
                return result;
            }
        }
    }
}