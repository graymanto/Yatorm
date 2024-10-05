namespace Yatorm.Tools;

public class WhereClause : ISqlClause
{
    private readonly bool _isAnd;

    public WhereClause(string column, CompOp op, object value, bool isAnd = false)
    {
        _isAnd = isAnd;
        Column = column;
        Op = op;
        Value = value;
    }

    private string Column { get; }
    private CompOp Op { get; }
    private object Value { get; }

    private string KeyWord => _isAnd ? "AND" : "WHERE";

    public void ToSql(SqlBuilder builder)
    {
        string? stringifiedValue =
            Value.GetType().IsNumericType() || Value.IsParameterBinding() ? Value.ToString() : $"'{Value}'";

        builder.Builder.AppendLine($"{KeyWord} {Column} {Op.ToSql()} {stringifiedValue}");
    }
}