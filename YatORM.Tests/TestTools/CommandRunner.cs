using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using YatORM.Tests.Settings;

namespace YatORM.Tests.TestTools
{
    internal class CommandRunner
    {
        /// <summary>
        /// Inserts a single entity into the test database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int InsertEntity<TEntity>(TEntity item)
        {
            var insertCommand = SqlBuilder.InsertStatement<TEntity>();
            var parameters = MakeQueryParametersFromEntity(item);

            return IssueNonQuery(insertCommand, parameters);
        }

        /// <summary>
        /// Inserts multiple entities into the test database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="items"></param>
        public static void InsertEntities<TEntity>(IEnumerable<TEntity> items)
        {
            var insertCommand = SqlBuilder.InsertStatement<TEntity>();

            var parameters = items.Select(MakeQueryParametersFromEntity);

            using (var conn = new SqlConnection(TestSettings.ConnectionString))
            {
                conn.Open();
                parameters.AsParallel().ForAll(p => IssueNonQueryInternal(conn, insertCommand, p));
            }
        }

        /// <summary>
        /// Clears the data out of the table for an entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public static void ClearEntityTable<TEntity>()
        {
            var command = SqlBuilder.TruncateStatement<TEntity>();

            IssueNonQuery(command);
        }

        /// <summary>
        /// Issues a non query against the test database.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int IssueNonQuery(string command, IEnumerable<SqlParameter> parameters = null)
        {
            using (var conn = new SqlConnection(TestSettings.ConnectionString))
            {
                conn.Open();
                return IssueNonQueryInternal(conn, command, parameters);
            }
        }

        private static int IssueNonQueryInternal(
            SqlConnection connection, 
            string command, 
            IEnumerable<SqlParameter> parameters = null)
        {
            using (var cmd = new SqlCommand(command, connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                return cmd.ExecuteNonQuery();
            }
        }

        private static IEnumerable<SqlParameter> MakeQueryParametersFromEntity<TEntity>(TEntity item)
        {
            return from prop in typeof(TEntity).GetProperties()
                   let propValue = prop.GetValue(item, null)
                   select new SqlParameter("@" + prop.Name, propValue ?? DBNull.Value);
        }
    }
}