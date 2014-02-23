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
    public class QueryTests
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
        public void Query_SingleSelectNoParameters_ExpectReturnsRequiredEntity()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            CommandRunner.InsertEntity(testEntity);

            var result = _session.GetFromQuery<SingleStringTestTable>("select * from SingleStringTestTable").First();

            result.Id.Should().Be(testEntity.Id);
            result.TestString.Should().Be(testEntity.TestString);
        }

        [Test]
        public void Query_SingleSelectWithParameters_ExpectReturnsRequiredEntity()
        {
            var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
            CommandRunner.InsertEntity(testEntity);

            var result =
                _session.GetFromQuery<SingleStringTestTable>(
                    "select * from SingleStringTestTable where Id=@Id",
                    new { testEntity.Id }).First();

            result.Id.Should().Be(testEntity.Id);
            result.TestString.Should().Be(testEntity.TestString);
        }
    }
}