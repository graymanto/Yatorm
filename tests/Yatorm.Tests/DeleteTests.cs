using FluentAssertions;
using Yatorm.Tests.Entity;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests
{
    public class DeleteTests
    {
        private IDataSession _session;

        public DeleteTests()
        {
            _session = TestSession.Create();

            CommandRunner.ClearEntityTable<SingleStringTestTable>();
            CommandRunner.ClearEntityTable<TypeTestTable>();
        }

        [Fact]
        public void Delete_SingleEntity_ShouldBeDeleted()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            CommandRunner.InsertEntity(testEntity);

            this._session.Delete<SingleStringTestTable>(e => e.Id == testEntity.Id);

            var results = CommandRunner.IssueNonQuery("Select * from SingleStringTestTable");

            results.Should().Be(-1);
        }
    }
}
