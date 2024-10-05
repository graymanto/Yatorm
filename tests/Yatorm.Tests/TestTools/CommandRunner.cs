using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Yatorm.Extensions;
using Yatorm.Tests.Settings;

namespace Yatorm.Tests.TestTools
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
            var insertCommand = TestSqlBuilder.InsertStatement<TEntity>();
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
            var insertCommand = TestSqlBuilder.InsertStatement<TEntity>();

            var parameters = items.Select(MakeQueryParametersFromEntity);

            using var conn = new SqlConnection(TestSettings.ConnectionString);
            conn.Open();
            // ReSharper disable once AccessToDisposedClosure
            parameters.ForEach(p => IssueNonQueryInternal(conn, insertCommand, p));
        }

        /// <summary>
        /// Clears the data out of the table for an entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public static void ClearEntityTable<TEntity>()
        {
            var command = TestSqlBuilder.TruncateStatement<TEntity>();

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
            using var conn = new SqlConnection(TestSettings.ConnectionString);
            conn.Open();
            return IssueNonQueryInternal(conn, command, parameters);
        }

        /// <summary>
        /// Issues a query and returns the results in dynamic objects.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> IssueDynamicQuery(string command)
        {
            var results = new List<dynamic>();

            using var conn = new SqlConnection(TestSettings.ConnectionString);
            conn.Open();
            var cmd = new SqlCommand(command, conn);
            var reader = cmd.ExecuteReader();

            IList<string> colNames = null;

            while (reader.Read())
            {
                if (colNames == null)
                    colNames = reader.GetColumnNames().ToList();
                results.Add(GetDynamicEntryFromReaderRow(reader, colNames));
            }

            return results;
        }

        private static dynamic GetDynamicEntryFromReaderRow(IDataReader reader, IEnumerable<string> readerColumns)
        {
            var result = new ExpandoObject();

            var resultAsDictionary = result as IDictionary<string, object>;

            readerColumns
                .Select((c, i) => new KeyValuePair<string, object>(c, reader.GetValue(i)))
                .ForEach(resultAsDictionary.Add);

            return result;
        }

        private static int IssueNonQueryInternal(
            SqlConnection connection,
            string command,
            IEnumerable<SqlParameter> parameters = null
        )
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
