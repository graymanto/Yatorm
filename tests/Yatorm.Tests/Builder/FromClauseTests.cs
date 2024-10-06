using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class FromClauseTests
{
    [Theory]
    [InlineData("Products", null, SqlSyntax.Standard, "FROM \"Products\"")]
    [InlineData("Products", "p", SqlSyntax.Standard, "FROM \"Products\" p")]
    [InlineData("Orders", null, SqlSyntax.SqlServer, "FROM [Orders]")]
    [InlineData("Orders", "o", SqlSyntax.SqlServer, "FROM [Orders] o")]
    public void FromClause_TableTests_ReturnsCorrectSql(string table, string? alias, SqlSyntax syntax, string result)
    {
        // Arrange
        var fromClause = new FromClause(table, alias);
        var builder = new SqlBuilder().WithSyntax(syntax);

        // Act
        fromClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(result, sql);
    }

    [Fact]
    public void FromClause_NullOrEmptyTable_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new FromClause(string.Empty));
    }
}
