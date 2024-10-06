namespace Yatorm.Tools;

public class GroupByClause : ISqlClause
{
    private readonly string[]? _columns;
    private readonly int[]? _numericColumns;

    public GroupByClause(params string[] columns)
    {
        _columns = columns;
    }

    public GroupByClause(params int[] columns)
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
            builder.Builder.AppendLine($"GROUP BY {columns}");
        }
        else if (_numericColumns is not null)
        {
            var columns = string.Join(", ", _numericColumns);
            builder.Builder.AppendLine($"GROUP BY {columns}");
        }
    }
}
