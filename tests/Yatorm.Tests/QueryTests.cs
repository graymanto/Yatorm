using FluentAssertions;
using Yatorm.Tests.Entity;
using Yatorm.Tests.Settings;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests
{
    public class QueryTests
    {
        private IDataSession _session;

        public QueryTests()
        {
            _session = TestSession.Create();

            var createTableSql = """
                create table testtable (Id INT, TestString TEXT)
                """;

            _session.ExecuteNonQuery(createTableSql);
        }

        private class TestTable
        {
            public long Id { get; set; }
            public string TestString { get; set; } = "";
        };

        [Fact]
        public void Query_SingleSelectNoParameters_ExpectReturnsRequiredEntity()
        {
            for (int i = 0; i < 3; i++)
            {
                var insertSql =
                    $@"
                insert into testtable (Id, TestString) values ({i}, 'test string {i}')
            ";

                _session.ExecuteNonQuery(insertSql);
            }

            // var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            // CommandRunner.InsertEntity(testEntity);

            var result = _session.GetFromQuery<TestTable>("select * from TestTable").First();

            result.Id.Should().Be(0);
            result.TestString.Should().Be("test string 0");
        }

        // TODO: need to use the right kind of parameters
        // [Fact]
        // public void Query_SingleSelectWithParameters_ExpectReturnsRequiredEntity()
        // {
        //     for (int i = 0; i < 3; i++)
        //     {
        //         var insertSql =
        //             $@"
        //         insert into testtable (Id, TestString) values ({i}, 'test string {i}')
        //     ";
        //
        //         _session.ExecuteNonQuery(insertSql);
        //     }
        //
        //     var queryEntity = new TestTable { Id = 1, TestString = "test string 1" };
        //
        //     var result = _session
        //         .GetFromQuery<TestTable>("select * from TestTable where Id=@Id", new { queryEntity.Id })
        //         .First();
        //
        //     result.Id.Should().Be(queryEntity.Id);
        //     result.TestString.Should().Be(queryEntity.TestString);
        // }
    }
}
