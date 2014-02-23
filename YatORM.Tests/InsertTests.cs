using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using YatORM.Tests.Entity;
using YatORM.Tests.Settings;
using YatORM.Tests.TestTools;

namespace YatORM.Tests
{
    [TestFixture]
    public class InsertTests
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
        public void Insert_SingleEntity_ExpectFindInTable()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };

            this._session.Insert(testEntity);

            var results = CommandRunner.IssueDynamicQuery("Select * from SingleStringTestTable").ToList();

            results.Count().Should().Be(1);

            var firstResult = results.First();

            Assert.AreEqual(testEntity.Id, firstResult.Id);
            Assert.AreEqual(testEntity.TestString, firstResult.TestString);
        }
    }
}