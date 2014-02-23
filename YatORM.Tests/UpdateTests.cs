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
    [TestFixture]
    [Rollback]
    public class UpdateTests
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
        public void Update_SimpleUpdate_ExpectAllRequiredFieldsUpdated()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            CommandRunner.InsertEntity(testEntity);

            const string UpdatedValue = "I am updated";
            testEntity.TestString = UpdatedValue;

            this._session.Update(e => e.Id == testEntity.Id, testEntity);

            var results = CommandRunner.IssueDynamicQuery("Select * from SingleStringTestTable").FirstOrDefault();

            // ReSharper disable once PossibleNullReferenceException
            string foundTestStringValue = results.TestString;

            foundTestStringValue.Should().Be(UpdatedValue);
        }
    }
}