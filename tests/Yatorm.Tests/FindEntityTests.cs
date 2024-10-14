using FluentAssertions;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests
{
    public class FindEntityTests
    {
        private readonly IDataSession _session;

        public FindEntityTests()
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
        }

        // [Fact]
        // public void Find_NothingInTable_ExpectReturnsNull()
        // {
        //     long anyInt = 5;
        //
        //     var isNull = _session.Single<TestTable>(s => s.Id == anyInt);
        //
        //     isNull.Should().BeNull();
        // }

        // [Fact]
        // public void Find_WrongEntryInTable_ExpectReturnsNull()
        // {
        //     for (int i = 0; i < 3; i++)
        //     {
        //         var insertSql =
        //             $@"
        //             insert into testtable (Id, TestString) values ({i}, 'test string {i}')
        //         ";
        //
        //         _session.ExecuteNonQuery(insertSql);
        //     }
        //     var isNull = _session.Single<TestTable>(s => s.Id == 5);
        //
        //     isNull.Should().BeNull();
        // }

        [Fact]
        public void Find_ByIdOnly_ExpectFindsCorrectEntity()
        {
            for (int i = 0; i < 3; i++)
            {
                var insertSql =
                    $@"
                insert into testtable (Id, TestString) values ({i}, 'test string {i}')
            ";

                _session.ExecuteNonQuery(insertSql);
            }

            var entity = _session.Single<TestTable>(s => s.Id == 2);

            entity.Should().NotBeNull();
            entity.Id.Should().Be(2);
            entity.TestString.Should().Be("test string 2");
        }

        [Fact]
        public void Find_ByIdOnlyUsingConstant_ExpectFindsCorrectEntity()
        {
            for (int i = 0; i < 3; i++)
            {
                var insertSql =
                    $@"
                insert into testtable (Id, TestString) values ({i}, 'test string {i}')
            ";

                _session.ExecuteNonQuery(insertSql);
            }

            var entity = _session.Single<TestTable>(s => s.TestString == "test string 1");

            entity.Should().NotBeNull();
            entity.Id.Should().Be(1);
            entity.TestString.Should().Be("test string 1");
        }

        [Fact]
        public void Find_ByIdAndStringField_ExpectFindsCorrectEntity()
        {
            for (int i = 0; i < 3; i++)
            {
                var insertSql =
                    $@"
                insert into testtable (Id, TestString) values ({i}, 'test string {i}')
            ";

                _session.ExecuteNonQuery(insertSql);
            }

            var entity = _session.Single<TestTable>(s => s.Id == 1 && s.TestString == "test string 1");

            entity.Should().NotBeNull();
            entity.Id.Should().Be(1);
            entity.TestString.Should().Be("test string 1");
        }

        [Fact]
        public void Find_ByIdOrStringField_ExpectFindsCorrectEntity()
        {
            for (int i = 0; i < 3; i++)
            {
                var insertSql =
                    $@"
                insert into testtable (Id, TestString) values ({i}, 'test string {i}')
            ";

                _session.ExecuteNonQuery(insertSql);
            }

            var entity = _session.Single<TestTable>(s => s.Id == 1 || s.TestString == "test string 1");

            entity.Should().NotBeNull();
            entity.Id.Should().Be(1);
            entity.TestString.Should().Be("test string 1");
        }

        // [Fact]
        // public void Find_ByBigIntField_ExpectFindsEntity()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     long testBigInt = testEntity.TestBigInt;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestBigInt == testBigInt);
        //
        //     entity.Should().NotBeNull();
        //     entity.TestBigInt.Should().Be(testEntity.TestBigInt);
        //     entity.Id.Should().Be(testEntity.Id);
        // }
        //
        // [Fact]
        // public void Find_ByDateTimeField_ExpectFindsEntity()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     var queryField = testEntity.TestDate;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestDate == queryField);
        //
        //     entity.Should().NotBeNull();
        //     entity.TestDate.Should().Be(testEntity.TestDate);
        //     entity.Id.Should().Be(testEntity.Id);
        // }
        //
        // [Fact]
        // public void Find_ByIntGreaterThan_ExpectFindsEntity()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     const int QueryField = 1;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestInt > QueryField);
        //
        //     entity.Should().NotBeNull();
        //     entity.Id.Should().Be(testEntity.Id);
        // }
        //
        // [Fact]
        // public void Find_ByIntGreaterEquals_ExpectFindsEntity()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     var queryField = testEntity.TestInt;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestInt >= queryField);
        //
        //     entity.Should().NotBeNull();
        //     entity.Id.Should().Be(testEntity.Id);
        // }
        //
        // [Fact]
        // public void Find_ByIntGreaterThanWithNoMatch_ExpectReturnsNull()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     const int QueryField = 10;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestInt > QueryField);
        //
        //     entity.Should().BeNull();
        // }
        //
        // [Fact]
        // public void Find_ByIntLessThan_ExpectFindsEntity()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     const int QueryField = 10;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestInt < QueryField);
        //
        //     entity.Should().NotBeNull();
        //     entity.Id.Should().Be(testEntity.Id);
        // }
        //
        // [Fact]
        // public void Find_ByIntLessThanEquals_ExpectFindsEntity()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     var queryField = testEntity.TestInt;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestInt <= queryField);
        //
        //     entity.Should().NotBeNull();
        //     entity.Id.Should().Be(testEntity.Id);
        // }
        //
        // [Fact]
        // public void Find_ByIntLessThanWithNoMatch_ExpectReturnsNull()
        // {
        //     var testEntity = CreateTypeTestEntity();
        //     CommandRunner.InsertEntity(testEntity);
        //
        //     const int QueryField = 0;
        //
        //     var entity = _session.Single<TypeTestTable>(t => t.TestInt < QueryField);
        //
        //     entity.Should().BeNull();
        // }
        //
        // private TypeTestTable CreateTypeTestEntity()
        // {
        //     return new TypeTestTable
        //     {
        //         Id = Guid.NewGuid(),
        //         TestBigInt = 5,
        //         TestDate = new DateTime(2014, 1, 1),
        //         TestInt = 2,
        //         TestNullBigInt = 4,
        //         TestNullDate = new DateTime(2014, 1, 2),
        //         TestNullInt = 7,
        //         TestString = "78910",
        //     };
        // }
    }
}
