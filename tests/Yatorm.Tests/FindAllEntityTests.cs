using FluentAssertions;
using Yatorm.Tests.Entity;
using Yatorm.Tests.TestTools;

// ReSharper disable PossibleMultipleEnumeration

namespace Yatorm.Tests
{
    public class FindAllEntityTests
    {
        private readonly IDataSession _session;

        public FindAllEntityTests()
        {
            _session = TestSession.Create();

            CommandRunner.ClearEntityTable<SingleStringTestTable>();
        }

        [Fact]
        public void FindAll_SingleEntity_ExpectFindsItem()
        {
            var testIdValue = Guid.NewGuid();
            var testStringValue = Guid.NewGuid().ToString();

            var testEntity = new SingleStringTestTable { Id = testIdValue, TestString = testStringValue };

            CommandRunner.InsertEntity(testEntity);

            var allEntities = _session.FindAll<SingleStringTestTable>().ToList();

            allEntities.Should().HaveCount(1);

            var foundEntity = allEntities.First();

            foundEntity.Id.Should().Be(testIdValue);
            foundEntity.TestString.Should().Be(testStringValue);
        }

        [Fact]
        public void FindAll_MultipleEntity_ExpectFindsItems()
        {
            var testIdValue = Guid.NewGuid();
            var testStringValue = Guid.NewGuid().ToString();

            var testIdValue2 = Guid.NewGuid();
            var testStringValue2 = Guid.NewGuid().ToString();

            var testEntity = new SingleStringTestTable { Id = testIdValue, TestString = testStringValue };
            var testEntity2 = new SingleStringTestTable { Id = testIdValue2, TestString = testStringValue2 };

            CommandRunner.InsertEntities(new[] { testEntity, testEntity2 });

            var allEntities = _session.FindAll<SingleStringTestTable>().ToList();

            allEntities.Should().HaveCount(2);

            var foundEntity = allEntities.FirstOrDefault(e => e.Id == testIdValue);
            foundEntity.Should().NotBeNull();
            foundEntity.TestString.Should().Be(testStringValue);

            foundEntity = allEntities.FirstOrDefault(e => e.Id == testIdValue2);
            foundEntity.Should().NotBeNull();
            foundEntity.TestString.Should().Be(testStringValue2);
        }

        [Fact]
        public void FindAll_EmptyTable_ExpectEmptyEnumerable()
        {
            var allEntities = _session.FindAll<SingleStringTestTable>();

            allEntities.Should().NotBeNull();
            allEntities.Should().HaveCount(0);
        }
    }
}
