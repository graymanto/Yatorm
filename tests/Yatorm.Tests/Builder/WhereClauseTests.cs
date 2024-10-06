using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class WhereClauseTests
{
    [Fact]
    public void WhereClause_EqualityOperator_ReturnsCorrectSql()
    {
        // Arrange
        var whereClause = new WhereClause("p.Id", CompOp.Eq, 1);
        var builder = new SqlBuilder();

        // Act
        whereClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal("WHERE p.Id = 1", sql);
    }

    [Fact]
    public void WhereClause_GreaterThanOperator_ReturnsCorrectSql()
    {
        // Arrange
        var whereClause = new WhereClause("p.Price", CompOp.Gt, 100);
        var builder = new SqlBuilder();

        // Act
        whereClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal("WHERE p.Price > 100", sql);
    }

    [Fact]
    public void WhereClause_LessThanOperator_ReturnsCorrectSql()
    {
        // Arrange
        var whereClause = new WhereClause("p.Price", CompOp.Lt, 50);
        var builder = new SqlBuilder();

        // Act
        whereClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal("WHERE p.Price < 50", sql);
    }

    [Fact]
    public void WhereClause_AndClause_ReturnsCorrectSql()
    {
        // Arrange
        var whereClause = new WhereClause("p.Name", CompOp.Eq, "Test", ClauseType.And);
        var builder = new SqlBuilder();

        // Act
        whereClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal("AND p.Name = 'Test'", sql);
    }

    [Fact]
    public void WhereClause_WithOrClause_ReturnsCorrectSql()
    {
        // Arrange
        var whereClause = new WhereClause("p.Name", CompOp.Eq, "Test", ClauseType.Or);
        var builder = new SqlBuilder();

        // Act
        whereClause.ToSql(builder);
        var sql = builder.ToSql();

        // Assert
        Assert.Equal("OR p.Name = 'Test'", sql);
    }
}
