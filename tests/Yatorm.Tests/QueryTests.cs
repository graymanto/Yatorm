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

            CommandRunner.ClearEntityTable<SingleStringTestTable>();
            CommandRunner.ClearEntityTable<TypeTestTable>();
        }

        [Fact]
        public void Query_SingleSelectNoParameters_ExpectReturnsRequiredEntity()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            CommandRunner.InsertEntity(testEntity);

            var result = _session.GetFromQuery<SingleStringTestTable>("select * from SingleStringTestTable").First();

            result.Id.Should().Be(testEntity.Id);
            result.TestString.Should().Be(testEntity.TestString);
        }

        [Fact]
        public void Query_SingleSelectWithParameters_ExpectReturnsRequiredEntity()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            CommandRunner.InsertEntity(testEntity);

            var result = _session
                .GetFromQuery<SingleStringTestTable>(
                    "select * from SingleStringTestTable where Id=@Id",
                    new { testEntity.Id }
                )
                .First();

            result.Id.Should().Be(testEntity.Id);
            result.TestString.Should().Be(testEntity.TestString);
        }
    }
}
