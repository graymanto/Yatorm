using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using YatORM.Extensions;

namespace YatORM
{
    public class YatDB
    {
        private readonly string _connectionString;

        public YatDB()
        {
            _connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        }

        public YatDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TResult GetCommand<TResult>(string query, CommandType type = CommandType.Text)
        {
            return default(TResult);
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

        private IQueryable<TInput> ExecuteMappedCommand<TInput>(string queryName, CommandType commandType = CommandType.Text,
            IEnumerable<SqlParameter> parameters = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = conn.BuildCommand(queryName, parameters);

                var rdr = cmd.ExecuteReader();

                //var result = _converter.ReaderToMappedSequence<T>(rdr);

                conn.Close();
                return result;
            }
        }

        /// <summary>
        /// Builds a sql command.
        /// </summary>
        /// <param name="conn">The sql connection.</param>
        /// <param name="queryName">Name of the query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private SqlCommand BuildCommand(SqlConnection conn, string queryName, CommandType commandType = CommandType.Text,
            IEnumerable<SqlParameter> parameters = null)
        {
            var cmd = new SqlCommand("dbo." + queryName, conn) { CommandType = commandType};

            if (parameters != null)
            {
		cmd.Parameters.AddRange(parameters.ToArray());
            }
            return cmd;
        }
    }
}
