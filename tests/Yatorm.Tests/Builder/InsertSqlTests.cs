using Yatorm.Tests.TestExtensions;
using Yatorm.Tools;

namespace Yatorm.Tests.Builder;

public class InsertSqlTests
{
    [Fact]
    public void SqlBuilder_InsertWithSubSelect_ReturnsCorrectSql()
    {
        // Arrange

        string expectedSql = """
            INSERT INTO "RelatedProducts" ("Name", "Price", "Type")
            SELECT "Name", "Price", "Type" 
            FROM "Products" p
            WHERE p.Id = 1
            AND p.Name = 'Test'
            """;

        // Act
        var generatedSql = new SqlBuilder()
            .Insert("RelatedProducts", ["Name", "Price", "Type"])
            .Select("Name", "Price", "Type")
            .From("Products", "p")
            .Where("p.Id", CompOp.Eq, 1)
            .And("p.Name", CompOp.Eq, "Test")
            .ToSql();

        // Assert

        Assert.Equal(expectedSql.NormalizeForComparison(), generatedSql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_InsertWithValues_ReturnsCorrectSql()
    {
        // Arrange

        string expectedSql = """
            INSERT INTO "RelatedProducts" ("Name", "Price", "Type")
            VALUES ('Product1', 9.99, 'Type1')
            """;

        // Act
        var generatedSql = new SqlBuilder()
            .Insert("RelatedProducts", [("Name", "Product1"), ("Price", 9.99), ("Type", "Type1")])
            .ToSql();

        // Assert

        Assert.Equal(expectedSql.NormalizeForComparison(), generatedSql.NormalizeForComparison());
    }

    [Fact]
    public void SqlBuilder_InsertWithMultipleValues_ReturnsCorrectSql()
    {
        // Arrange

        string expectedSql = """
            INSERT INTO "RelatedProducts" ("Name", "Price", "Type")
            VALUES 
                ('Product1', 9.99, 'Type1'),
                ('Product2', 9.98, 'Type2'),
                ('Product3', 9.97, 'Type3'),
                ('Product4', 9.96, 'Type4')
            """;

        // Act
        var generatedSql = new SqlBuilder()
            .Insert("RelatedProducts", ["Name", "Price", "Type"])
            .Values("Product1", 9.99, "Type1")
            .Values("Product2", 9.98, "Type2")
            .Values("Product3", 9.97, "Type3")
            .Values("Product4", 9.96, "Type4")
            .ToSql();

        // Assert

        Assert.Equal(expectedSql.NormalizeForComparison(), generatedSql.NormalizeForComparison());
    }
}
