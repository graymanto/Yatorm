using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using YatORM.Tests.Attributes;
using YatORM.Tests.Entity;
using YatORM.Tests.Settings;
using YatORM.Tests.TestTools;

namespace YatORM.Tests
{
    [TestFixture, Rollback]
    public class FindAllEntityTests
    {
        private IDataSession _session;

        [SetUp]
        public void Setup()
        {
            _session = SessionBuilder.WithConnectionString(TestSettings.ConnectionString).BuildSession();

            CommandRunner.ClearEntityTable<SingleStringTestTable>();
        }

        [Test]
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

        [Test]
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
            // ReSharper disable once PossibleNullReferenceException
            foundEntity.TestString.Should().Be(testStringValue);

            foundEntity = allEntities.FirstOrDefault(e => e.Id == testIdValue2);
            foundEntity.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            foundEntity.TestString.Should().Be(testStringValue2);
        }

        [Test]
        public void FindAll_EmptyTable_ExpectEmptyEnumerable()
        {
            var allEntities = _session.FindAll<SingleStringTestTable>();

            // ReSharper disable PossibleMultipleEnumeration
            allEntities.Should().NotBeNull();
            allEntities.Should().HaveCount(0);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}