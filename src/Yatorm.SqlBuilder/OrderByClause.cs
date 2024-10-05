namespace Yatorm.Tools;

public class OrderByClause : ISqlClause
{
    private readonly string[]? _columns;
    private readonly int[]? _numericColumns;

    public OrderByClause(params string[] columns)
    {
        _columns = columns;
    }

    public OrderByClause(params int[] columns)
    {
        _numericColumns = columns;
    }

    public void ToSql(SqlBuilder builder)
    {
        var escapeLeft = builder.Syntax.EscapeCharLeft();
        var escapeRight = builder.Syntax.EscapeCharRight();

        if (_columns is not null)
        {
            var columns = string.Join(", ", _columns.Select(c => $"{escapeLeft}{c}{escapeRight}"));
            builder.Builder.AppendLine($"ORDER BY {columns}");
        }
        else if (_numericColumns is not null)
        {
            var columns = string.Join(", ", _numericColumns);
            builder.Builder.AppendLine($"ORDER BY {columns}");
        }
    }
}