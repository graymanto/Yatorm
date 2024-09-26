using Yatorm.Sqlite;

namespace Yatorm.Tests.TestTools;

public class TestSession
{
    public static IDataSession Create()
    {
        var builder = new SessionBuilder();
        return builder.WithSqliteMemoryConnection().BuildSession();
    }
}
