using System;

using FluentAssertions;

using NUnit.Framework;

using YatORM.Tests.Attributes;
using YatORM.Tests.Entity;
using YatORM.Tests.Settings;
using YatORM.Tests.TestTools;

namespace YatORM.Tests
{
    [TestFixture, Rollback]
    public class DeleteTests
    {
        private IDataSession _session;

        [SetUp]
        public void Setup()
        {
            this._session = SessionBuilder.WithConnectionString(TestSettings.ConnectionString).BuildSession();

            CommandRunner.ClearEntityTable<SingleStringTestTable>();
            CommandRunner.ClearEntityTable<TypeTestTable>();
        }

        [Test]
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