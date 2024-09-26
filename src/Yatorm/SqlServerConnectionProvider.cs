using System.Data;
using System.Data.SqlClient;

namespace Yatorm;

public class SqlServerConnectionProvider : IConnectionProvider
{
    private readonly string? _connectionString;

    public SqlServerConnectionProvider() { }

    public SqlServerConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        if (_connectionString != null)
        {
            return new SqlConnection(_connectionString);
        }

        return new SqlConnection();
    }

    public DbSyntax Syntax => DbSyntax.SqlServer;
}
