using FluentAssertions;
using Yatorm.Tests.Entity;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests
{
    public class DeleteTests
    {
        private readonly IDataSession _session;

        public DeleteTests()
        {
            _session = TestSession.Create();

            var createTableSql = """
                create table deletetesttable (Id INT, TestString TEXT)
                """;

            _session.ExecuteNonQuery(createTableSql);
        }

        private class DeleteTestTable
        {
            public long Id { get; set; }
            public string TestString { get; set; } = "";
        }

        [Fact]
        public void Delete_SingleEntity_ShouldBeDeleted()
        {
            // var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            // CommandRunner.InsertEntity(testEntity);

            _session.Delete<DeleteTestTable>(e => e.Id == 1);

            var results = _session.ExecuteNonQuery("Select * from deletetesttable");

            results.Should().Be(-1);
        }
    }
}
