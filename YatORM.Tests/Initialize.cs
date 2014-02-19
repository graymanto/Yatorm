using System;
using System.Data.SqlClient;
using System.IO;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using NUnit.Framework;

using YatORM.Extensions;
using YatORM.Tests.Constants;
using YatORM.Tests.Settings;
using YatORM.Tests.TestTools;

namespace YatORM.Tests
{

    [SetUpFixture]
    public class Initialize
    {
        private const string DropIfExists = @"IF EXISTS(select * from sys.databases where name='YatTestDb') drop database YatTestDb";

        private readonly string _testDirectory = Path.GetTempPath();

        [SetUp]
        public void Setup()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", _testDirectory);

            CleanUpDatabase();
            CreateDatabase();
            CreateTestSchema();
        }

        [TearDown]
        public void TearDown()
        {
        }

        private void CreateTestSchema()
        {
            var commandSql = ResourceUtils.GetEmbeddedResource(ResourceKeys.TestSchema);

            using (var connection = new SqlConnection(TestSettings.ConnectionString))
            {
                connection.Open();

                var server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(commandSql);
            }
        }

        private void CreateDatabase()
        {
            using (var connection = new SqlConnection(@"server=(localdb)\v11.0"))
            {
                connection.Open();

                string createSql =
                    @"create database [YatTestDb] on Primary (Name = YatTestDb, FILENAME = '{0}\YatTestDb.mdf')".Formatted(
                        _testDirectory);

                var command = new SqlCommand(createSql, connection);
                command.ExecuteNonQuery();
            }
        }

        private void CleanUpDatabase()
        {
            using (var connection = new SqlConnection(@"server=(localdb)\v11.0"))
            {
                connection.Open();
                DropDatabase(connection);
            }

            DeleteTestFiles();
        }

        private void DropDatabase(SqlConnection connection)
        {
            var command = new SqlCommand(DropIfExists, connection);
            command.ExecuteNonQuery();
        }

        private void DeleteTestFiles()
        {
            var dbFiles = Directory.GetFiles(_testDirectory, "YatTestDb*");
            dbFiles.ForEach(File.Delete);
        }
    }
}