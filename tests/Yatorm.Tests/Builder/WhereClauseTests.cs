using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class WhereClauseTests
{
    [Theory]
    [InlineData("p.Id", CompOp.Eq, 1, ClauseType.Where, "WHERE p.Id = 1")]
    [InlineData("p.Price", CompOp.Gt, 100, ClauseType.Where, "WHERE p.Price > 100")]
    [InlineData("p.Price", CompOp.Gte, 100, ClauseType.Where, "WHERE p.Price >= 100")]
    [InlineData("p.Price", CompOp.Lt, 50, ClauseType.Where, "WHERE p.Price < 50")]
    [InlineData("p.Price", CompOp.Lte, 50, ClauseType.Where, "WHERE p.Price <= 50")]
    [InlineData("p.Name", CompOp.Eq, "Test", ClauseType.And, "AND p.Name = 'Test'")]
    [InlineData("p.Name", CompOp.Eq, "Test", ClauseType.Or, "OR p.Name = 'Test'")]
    public void WhereClause_TableTests_ReturnsCorrectSql(
        string column,
        CompOp op,
        object value,
        ClauseType clauseType,
        string result
    )
    {
        // Arrange
        var whereClause = new WhereClause(column, op, value, clauseType);
        var builder = new SqlBuilder();

        // Act
        whereClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(result, sql);
    }
}
