using System;
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

        public IQueryable<TResult> GetCommand<TResult>(string query, dynamic parameters) where TResult : new()
        {
            var commandParams = parameters == null ? null : DBToTypeConverter.TransformClassToSqlParameters(parameters);
            return ExecuteMappedCommand<TResult>(query, CommandType.Text, commandParams);
        }

        public TResult GetSingleItem<TResult>(string query) where TResult : new()
        {
            return ExecuteMappedCommand<TResult>(query).FirstOrDefault();
        }

        public IQueryable<TResult> ExecStoredProc<TResult>(string procname, dynamic parameters) where TResult : new()
        {
            var commandParams = parameters == null ? null : DBToTypeConverter.TransformClassToSqlParameters(parameters);

            return ExecuteMappedCommand<TResult>(procname, CommandType.StoredProcedure, commandParams);
        }

        public TResult ExecScalarStoredProc<TResult>(string procname, object parameters = null) where TResult : new()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(procname, conn) { CommandType = CommandType.StoredProcedure };

                if (parameters != null)
                {
                    var commandParams = DBToTypeConverter.TransformClassToSqlParameters(parameters);
                    if (commandParams != null)
                    {
                        commandParams.ForEach(p => cmd.Parameters.Add(p));
                    }
                }

                var result = cmd.ExecuteScalar();

                conn.Close();

                // TODO: better conversion
                return (TResult)Convert.ChangeType(result, typeof(TResult));
            }
        }

        public int ExecNonQueryProc(string procname, object parameters = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(procname, conn) { CommandType = CommandType.StoredProcedure };

                if (parameters != null)
                {
                    var commandParams = DBToTypeConverter.TransformClassToSqlParameters(parameters);
                    if (commandParams != null)
                    {
                        commandParams.ForEach(p => cmd.Parameters.Add(p));
                    }
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string query, object parameters = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn) { CommandType = CommandType.Text };

                if (parameters != null)
                {
                    var commandParams = parameters is IEnumerable<SqlParameter>
                                            ? (IEnumerable<SqlParameter>)parameters
                                            : DBToTypeConverter.TransformClassToSqlParameters(parameters);
                    if (commandParams != null)
                    {
                        commandParams.ForEach(p => cmd.Parameters.Add(p));
                    }
                }

                return cmd.ExecuteNonQuery();
            }
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

                var resultSet = rdr.GetMappedSequence<TResult>();

                conn.Close();
                return resultSet;
            }
        }
    }
}