using System.Data;

namespace Yatorm;

public interface IConnectionProvider
{
    IDbConnection GetConnection();
    DbSyntax Syntax { get; }
}
