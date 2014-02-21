using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using YatORM.Tests.Constants;
using YatORM.Tests.Entity;
using YatORM.Tests.Settings;
using YatORM.Tests.TestTools;

namespace YatORM.Tests
{
    [TestFixture]
    public class ProcedureTests
    {
        private readonly TypeTestTable _testType = new TypeTestTable
                                              {
                                                  Id = Guid.NewGuid(),
                                                  TestBigInt = 5,
                                                  TestDate = new DateTime(2014, 1, 1),
                                                  TestInt = 2,
                                                  TestNullBigInt = 4,
                                                  TestNullDate = new DateTime(2014, 1, 2),
                                                  TestNullInt = 7,
                                                  TestString = "78910"
                                              };

        private IDataSession _session;

        [SetUp]
        public void Setup()
        {
            this._session = SessionBuilder.WithConnectionString(TestSettings.ConnectionString).BuildSession();

            CommandRunner.ClearEntityTable<SingleStringTestTable>();
            CommandRunner.ClearEntityTable<TypeTestTable>();
        }

        [Test]
        public void GetFromProcedure_SingleEntity_ReturnsCorrectlyMappedEntity()
        {
            CommandRunner.InsertEntity(_testType);

            var results = _session.GetFromProcedure<TypeTestTable>(StoredProcedureNames.TestGet).ToList();

            results.Should().HaveCount(1);

            var testType = results.First();

            CheckTestTypesAreEqual(testType);
        }

        [Test]
        public void GetFromProcedure_WithProcParamSingleEntity_ReturnsCorrectlyMappedEntity()
        {
            CommandRunner.InsertEntity(_testType);

            var results =
                _session.GetFromProcedure<TypeTestTable>(
                    StoredProcedureNames.TestGetWithParam,
                    new TestGetInputs { Id = _testType.Id }).ToList();

            results.Should().HaveCount(1);

            var testType = results.First();

            CheckTestTypesAreEqual(testType);
        }

        [Test]
        public void GetFromProcedure_WithAnonProcParamSingleEntity_ReturnsCorrectlyMappedEntity()
        {
            CommandRunner.InsertEntity(_testType);

            var results =
                _session.GetFromProcedure<TypeTestTable>(
                    StoredProcedureNames.TestGetWithParam,
                    new { Id = _testType.Id }).ToList();

            results.Should().HaveCount(1);

            var testType = results.First();

            CheckTestTypesAreEqual(testType);
        }

        public class TestGetInputs
        {
            public Guid Id { get; set; }
        }

        private void CheckTestTypesAreEqual(TypeTestTable queryResult)
        {
            queryResult.Id.Should().Be(_testType.Id);
            queryResult.TestBigInt.Should().Be(_testType.TestBigInt);
            queryResult.TestDate.Should().Be(_testType.TestDate);
            queryResult.TestInt.Should().Be(_testType.TestInt);
            queryResult.TestNullBigInt.Should().Be(_testType.TestNullBigInt);
            queryResult.TestNullDate.Should().Be(_testType.TestNullDate);
            queryResult.TestNullInt.Should().Be(_testType.TestNullInt);

            // TODO: the trim here is a bug
            queryResult.TestString.Trim().Should().Be(_testType.TestString);
        }
    }
}