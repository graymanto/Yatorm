using FluentAssertions;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests
{
    public class FindAllEntityTests
    {
        private readonly IDataSession _session;

        public FindAllEntityTests()
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
        public void FindAll_SingleEntity_ExpectFindsItem()
        {
            // Arrange

            for (int i = 0; i < 10; i++)
            {
                var insertSql =
                    $@"
                    insert into testtable (Id, TestString) values ({i}, 'test string {i}')
                ";

                _session.ExecuteNonQuery(insertSql);
            }
            var testIdValue = 0;
            var testStringValue = $"test string {testIdValue}";

            // Act

            var allEntities = _session.FindAll<TestTable>().ToList();

            // Assert
            allEntities.Should().HaveCount(10);

            var foundEntity = allEntities.First();

            foundEntity.Id.Should().Be(testIdValue);
            foundEntity.TestString.Should().Be(testStringValue);
        }

        [Fact]
        public void FindAll_MultipleEntity_ExpectFindsItems()
        {
            // Arrange

            for (int i = 1; i < 3; i++)
            {
                var insertSql =
                    $@"
                    insert into testtable (Id, TestString) values ({i}, 'test string {i}')
                ";

                _session.ExecuteNonQuery(insertSql);
            }
            var testStringValue = "test string 1";
            var testStringValue2 = "test string 2";

            // var testEntity = new TestTable { Id = testIdValue, TestString = testStringValue };
            // var testEntity2 = new TestTable { Id = testIdValue2, TestString = testStringValue2 };
            //
            // CommandRunner.InsertEntities([testEntity, testEntity2]);

            var allEntities = _session.FindAll<TestTable>().ToList();

            allEntities.Should().HaveCount(2);

            var foundEntity = allEntities.FirstOrDefault(e => e.Id == 1);
            foundEntity.Should().NotBeNull();
            foundEntity!.TestString.Should().Be(testStringValue);

            foundEntity = allEntities.FirstOrDefault(e => e.Id == 2);
            foundEntity.Should().NotBeNull();
            foundEntity!.TestString.Should().Be(testStringValue2);
        }

        [Fact]
        public void FindAll_EmptyTable_ExpectEmptyEnumerable()
        {
            var allEntities = _session.FindAll<TestTable>().ToList();

            allEntities.Should().NotBeNull();
            allEntities.Should().HaveCount(0);
        }
    }
}
