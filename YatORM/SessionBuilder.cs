using System;
using System.Collections.Generic;
using System.Configuration;

namespace YatORM
{
    public static class SessionBuilder
    {
        public static SessionBuilderContext WithConnectionString(string connectionString)
        {
            return new SessionBuilderContext(connectionString);
        }

        public static SessionBuilderContext WithConnectionStringName(string connectionStringName)
        {
            var connection = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connection == null)
            {
                throw new ArgumentOutOfRangeException("connectionStringName", "Connection string not found.");
            }

            return WithConnectionString(connection.ConnectionString);
        }

        public class SessionBuilderContext
        {
            private readonly string _connectionString;

            public SessionBuilderContext(string connectionString)
            {
                _connectionString = connectionString;
            }

            public IDataSession BuildSession()
            {
                return new TempEmptySession();
            }
        }

        public class TempEmptySession : IDataSession
        {
            public IEnumerable<TEntity> FindAll<TEntity>()
            {
                yield break;
            }
        }
    }
}