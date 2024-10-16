using Yatorm.Tests.TestExtensions;
using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class InsertStatementTests
{
    [Fact]
    public void InsertStatement_SingleColumn_ReturnsCorrectSql()
    {
        // Arrange
        var insertValues = new (string, object?)[] { ("Name", "Product1") };
        var statement = new InsertStatement("Products", insertValues);
        var builder = new SqlBuilder();

        // Act
        statement.ToSql(builder);
        var sql = builder.ToSql();
        var expectedSql = "INSERT INTO \"Products\" (\"Name\") VALUES ('Product1')";

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    // TODO: fix
    [Fact]
    public void InsertStatement_MultipleColumns_ReturnsCorrectSql()
    {
        // Arrange
        var insertValues = new (string, object?)[] { ("Name", "Product1"), ("Price", 9.99) };
        var statement = new InsertStatement("Products", insertValues);
        var builder = new SqlBuilder();

        // Act
        statement.ToSql(builder);
        var sql = builder.ToSql();
        var expectedSql = "INSERT INTO \"Products\" (\"Name\", \"Price\") VALUES ('Product1', 9.99)";

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void InsertStatement_WithNullValue_ReturnsCorrectSql()
    {
        // Arrange
        var insertValues = new (string, object?)[] { ("Name", null) };
        var statement = new InsertStatement("Products", insertValues);
        var builder = new SqlBuilder();

        // Act
        statement.ToSql(builder);
        var sql = builder.ToSql();
        var expectedSql = "INSERT INTO \"Products\" (\"Name\") VALUES (NULL)";

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void InsertStatement_WithIntegerValue_ReturnsCorrectSql()
    {
        // Arrange
        var statement = new InsertStatement("Products", [("Id", 1), ("Name", "Product1")]);
        var builder = new SqlBuilder();

        // Act
        statement.ToSql(builder);
        var sql = builder.ToSql();
        var expectedSql = "INSERT INTO \"Products\" (\"Id\", \"Name\") VALUES (1, 'Product1')";

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void InsertStatement_WithBooleanValueSqlServer_ReturnsCorrectSql()
    {
        // Arrange
        var insertValues = new (string, object?)[] { ("IsActive", true) };
        var statement = new InsertStatement("Products", insertValues);
        var builder = new SqlBuilder().WithSyntax(SqlSyntax.SqlServer);

        // Act
        statement.ToSql(builder);
        var sql = builder.ToSql();
        var expectedSql = "INSERT INTO [Products] ([IsActive]) VALUES (1)";

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void InsertStatement_WithBooleanValuePostgres()
    {
        // Arrange
        var insertValues = new (string, object?)[] { ("IsActive", true) };
        var statement = new InsertStatement("Products", insertValues);
        var builder = new SqlBuilder().WithSyntax(SqlSyntax.Postgres);

        // Act
        statement.ToSql(builder);
        var sql = builder.ToSql();
        var expectedSql = "INSERT INTO \"Products\" (\"IsActive\") VALUES (true)";

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }
}
