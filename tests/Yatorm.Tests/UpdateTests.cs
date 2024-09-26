using FluentAssertions;
using Yatorm.Tests.Entity;
using Yatorm.Tests.Settings;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests;

public class UpdateTests
{
    private IDataSession _session;

    public UpdateTests()
    {
        _session = TestSession.Create();

        CommandRunner.ClearEntityTable<SingleStringTestTable>();
        CommandRunner.ClearEntityTable<TypeTestTable>();
    }

    [Fact]
    public void Update_SimpleUpdate_ExpectAllRequiredFieldsUpdated()
    {
        var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };
        CommandRunner.InsertEntity(testEntity);

        const string UpdatedValue = "I am updated";
        testEntity.TestString = UpdatedValue;

        _session.Update(e => e.Id == testEntity.Id, testEntity);

        var results = CommandRunner.IssueDynamicQuery("Select * from SingleStringTestTable").FirstOrDefault();

        // ReSharper disable once PossibleNullReferenceException
        string foundTestStringValue = results.TestString;

        foundTestStringValue.Should().Be(UpdatedValue);
    }
}
