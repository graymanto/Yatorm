using System.Data;
using Microsoft.Data.Sqlite;

namespace Yatorm.Sqlite;

public class SqliteConnectionProvider : IConnectionProvider
{
    private readonly string? _dataSource;

    public SqliteConnectionProvider(string? dataSource)
    {
        _dataSource = dataSource;
    }

    public SqliteConnectionProvider(string dataSource, string? dataSource1)
    {
        _dataSource = dataSource1;
    }

    public IDbConnection GetConnection()
    {
        return _dataSource != null ? new SqliteConnection(_dataSource) : new SqliteConnection();
    }

    public DbSyntax Syntax => DbSyntax.Sqlite;
}
