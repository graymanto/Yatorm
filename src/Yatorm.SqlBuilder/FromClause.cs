namespace Yatorm.Tools;

public class FromClause : ISqlClause
{
    public FromClause(string table, string? alias = null)
    {
        if (string.IsNullOrEmpty(table))
        {
            throw new ArgumentException("Table name cannot be null or empty.", nameof(table));
        }

        Table = table;
        Alias = alias;
    }

    private string Table { get; }
    private string? Alias { get; }

    public void ToSql(SqlBuilder builder)
    {
        var escapeLeft = builder.Syntax.EscapeCharLeft();
        var escapeRight = builder.Syntax.EscapeCharRight();

        builder.Builder.Append($"FROM {escapeLeft}{Table}{escapeRight}");

        if (!string.IsNullOrEmpty(Alias))
        {
            builder.Builder.Append($" {Alias}");
        }

        builder.Builder.AppendLine();
    }
}
