using FluentAssertions;
using Yatorm.Tests.Entity;
using Yatorm.Tests.TestTools;

namespace Yatorm.Tests;

public class InsertTests
{
    private IDataSession _session;

    public InsertTests()
    {
        _session = TestSession.Create();

        CommandRunner.ClearEntityTable<SingleStringTestTable>();
        CommandRunner.ClearEntityTable<TypeTestTable>();
    }

    [Fact]
    public void Insert_SingleEntity_ExpectFindInTable()
    {
        var testEntity = new SingleStringTestTable { Id = Guid.NewGuid(), TestString = "Any string" };

        this._session.Insert(testEntity);

        var results = CommandRunner.IssueDynamicQuery("Select * from SingleStringTestTable").ToList();

        results.Count().Should().Be(1);

        var firstResult = results.First();

        Assert.Equal(testEntity.Id, firstResult.Id);
        Assert.Equal(testEntity.TestString, firstResult.TestString);
    }
}
