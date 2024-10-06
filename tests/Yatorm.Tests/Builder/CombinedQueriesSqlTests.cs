using Yatorm.Tests.TestExtensions;
using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class CombinedQueriesSqlTests
{
    [Fact]
    public void SqlBuilder_Union_ReturnsCorrectSql()
    {
        // Arrange
        var builder = new SqlBuilder();
        var expectedSql = "SELECT \"Id\", \"Name\" FROM \"Products\" UNION SELECT \"Id\", \"Name\" FROM \"Orders\"";

        // Act
        builder.Select("Id", "Name").From("Products").Union().Select("Id", "Name").From("Orders");
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_UnionAll_ReturnsCorrectSql()
    {
        // Arrange
        var builder = new SqlBuilder();
        var expectedSql = "SELECT \"Id\", \"Name\" FROM \"Products\" UNION ALL SELECT \"Id\", \"Name\" FROM \"Orders\"";

        // Act
        builder.Select("Id", "Name").From("Products").UnionAll().Select("Id", "Name").From("Orders");
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_Except_ReturnsCorrectSql()
    {
        // Arrange
        var builder = new SqlBuilder();
        var expectedSql = "SELECT \"Id\", \"Name\" FROM \"Products\" EXCEPT SELECT \"Id\", \"Name\" FROM \"Orders\"";

        // Act
        builder.Select("Id", "Name").From("Products").Except().Select("Id", "Name").From("Orders");
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_ExceptAll_ReturnsCorrectSql()
    {
        // Arrange
        var builder = new SqlBuilder();
        var expectedSql =
            "SELECT \"Id\", \"Name\" FROM \"Products\" EXCEPT ALL SELECT \"Id\", \"Name\" FROM \"Orders\"";

        // Act
        builder.Select("Id", "Name").From("Products").ExceptAll().Select("Id", "Name").From("Orders");
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_Intersect_ReturnsCorrectSql()
    {
        // Arrange
        var builder = new SqlBuilder();
        var expectedSql = "SELECT \"Id\", \"Name\" FROM \"Products\" INTERSECT SELECT \"Id\", \"Name\" FROM \"Orders\"";

        // Act
        builder.Select("Id", "Name").From("Products").Intersect().Select("Id", "Name").From("Orders");
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_IntersectAll_ReturnsCorrectSql()
    {
        // Arrange
        var builder = new SqlBuilder();
        var expectedSql =
            "SELECT \"Id\", \"Name\" FROM \"Products\" INTERSECT ALL SELECT \"Id\", \"Name\" FROM \"Orders\"";

        // Act
        builder.Select("Id", "Name").From("Products").IntersectAll().Select("Id", "Name").From("Orders");
        var sql = builder.ToSql();

        // Assert
        Assert.Equal(expectedSql.NormalizeForComparison(), sql.NormalizeForComparison());
    }
}
