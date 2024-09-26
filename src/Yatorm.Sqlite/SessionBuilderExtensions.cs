namespace Yatorm.Sqlite;

public static class SessionBuilderExtensions
{
    public static SessionBuilder.SessionBuilderContext WithSqliteConnection(
        this SessionBuilder builder,
        string connectionString
    )
    {
        builder.ConnectionProvider = new SqliteConnectionProvider(connectionString);
        return new SessionBuilder.SessionBuilderContext(builder);
    }

    public static SessionBuilder.SessionBuilderContext WithSqliteMemoryConnection(this SessionBuilder builder)
    {
        builder.ConnectionProvider = new SqlServerConnectionProvider();
        return new SessionBuilder.SessionBuilderContext(builder);
    }
}
