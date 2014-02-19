using System.Configuration;
using System.Data.SqlClient;

using NUnit.Framework;

using YatORM.Tests.Settings;

namespace YatORM.Tests
{
    [TestFixture]
    public class SanityCheckTest
    {
        [Test]
        public void SimpleInsertDelete()
        {
            using (var connection = new SqlConnection(TestSettings.ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "insert into SingleStringTestTable (TestString) values ('TestString')", 
                    connection);

                command.ExecuteNonQuery();
            }
        }
    }
}