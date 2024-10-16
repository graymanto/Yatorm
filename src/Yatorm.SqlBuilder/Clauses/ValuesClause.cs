namespace Yatorm.Tools;

public class ValuesClause : ISqlClause
{
    private readonly object?[] _values;

    public ValuesClause(object?[] values)
    {
        _values = values;
    }

    public void ToSql(SqlBuilder builder)
    {
        var values = string.Join(", ", _values.Select(v => $"{v.StringifySqlValue(builder.Syntax)}"));
        builder.Builder.Append($"({values})");
    }
}
