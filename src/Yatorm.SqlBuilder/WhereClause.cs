namespace Yatorm.Tools;

public class WhereClause : ISqlClause
{
    private readonly ClauseType _clauseType;

    public WhereClause(string column, CompOp op, object value, ClauseType clauseType = ClauseType.Where)
    {
        _clauseType = clauseType;
        Column = column;
        Op = op;
        Value = value;
    }

    private string Column { get; }
    private CompOp Op { get; }
    private object Value { get; }

    private string KeyWord => _clauseType.ToSql();

    public void ToSql(SqlBuilder builder)
    {
        string? stringifiedValue =
            Value.GetType().IsNumericType() || Value.IsParameterBinding() ? Value.ToString() : $"'{Value}'";

        builder.Builder.AppendLine($"{KeyWord} {Column} {Op.ToSql()} {stringifiedValue}");
    }
}
