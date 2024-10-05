using Yatorm.Tests.TestExtensions;
using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class SimpleSqlTests
{
    [Fact]
    public void SqlBuilder_ExplicitSelectWithWhereAnd_ReturnsCorrectSql()
    {
        // Arrange

        string expectedSql = """
            SELECT "Name", "Price", "Type" 
            FROM "Products" p
            WHERE p.Id = 1
            AND p.Name = 'Test'
            """;

        // Act
        var generatedSql = new SqlBuilder()
            .Select("Name", "Price", "Type")
            .From("Products", "p")
            .Where("p.Id", CompOp.Eq, 1)
            .And("p.Name", CompOp.Eq, "Test")
            .ToSql();

        // Assert

        Assert.Equal(expectedSql.NormalizeForComparison(), generatedSql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_WithWhereAndOrderBy_ReturnsCorrectSql()
    {
        // Arrange

        string expectedSql = """
            SELECT "Name", "Price", "Type" 
            FROM "Products" p
            WHERE p.Id > 0
            AND p.Name = 'Test'
            ORDER BY "Name", "Price"
            """;

        // Act
        var generatedSql = new SqlBuilder()
            .Select("Name", "Price", "Type")
            .From("Products", "p")
            .Where("p.Id", CompOp.Gt, 0)
            .And("p.Name", CompOp.Eq, "Test")
            .OrderBy("Name", "Price")
            .ToSql();

        // Assert

        Assert.Equal(expectedSql.NormalizeForComparison(), generatedSql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_WithWhereAndOrderByNumeric_ReturnsCorrectSql()
    {
        // Arrange

        string expectedSql = """
            SELECT "Name", "Price", "Type" 
            FROM "Products" p
            WHERE p.Id >= 0
            AND p.Name = 'Test'
            ORDER BY 1, 2, 3
            """;

        // Act
        var generatedSql = new SqlBuilder()
            .Select("Name", "Price", "Type")
            .From("Products", "p")
            .Where("p.Id", CompOp.Gte, 0)
            .And("p.Name", CompOp.Eq, "Test")
            .OrderBy(1, 2, 3)
            .ToSql();

        // Assert

        Assert.Equal(expectedSql.NormalizeForComparison(), generatedSql.NormalizeForComparison());
    }

    // SELECT
    //     l_orderkey,
    //     sum( l_extendedprice * ( 1 - l_discount) ) AS revenue,
    //     o_orderdate,
    //     o_shippriority
    // FROM
    //     customer,
    //     orders,
    //     lineitem
    // WHERE
    //     c_mktsegment = 'FURNITURE' AND
    //     c_custkey = o_custkey AND
    //     l_orderkey = o_orderkey AND
    // o_orderdate < 2013-12-21 AND
    // l_shipdate > date 2014-01-06
    // GROUP BY
    //     l_orderkey,
    //     o_orderdate,
    //     o_shippriority
    // ORDER BY
    //     revenue,
    //     o_orderdate;
}
