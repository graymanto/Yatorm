namespace Yatorm
{
    public class SessionBuilder
    {
        internal IConnectionProvider? ConnectionProvider { get; set; }

        public SessionBuilderContext WithConnectionProvider(IConnectionProvider connectionProvider)
        {
            ConnectionProvider = connectionProvider;
            return new SessionBuilderContext(this);
        }

        // public SessionBuilderContext WithSqlServerConnection(string connectionString)
        // {
        //     ConnectionProvider = new SqlServerConnectionProvider(connectionString);
        //     return new SessionBuilderContext(this);
        // }

        public class SessionBuilderContext(SessionBuilder builder)
        {
            public IDataSession BuildSession()
            {
                builder.ConnectionProvider ??= new SqlServerConnectionProvider();

                return new DataSession(builder.ConnectionProvider);
            }
        }
    }
}
