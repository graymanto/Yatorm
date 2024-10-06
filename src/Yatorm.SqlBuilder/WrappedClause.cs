namespace Yatorm.Tools;

public class WrappedClause : ISqlClause
{
    private readonly string _column;
    private readonly CompOp _op;
    private readonly ClauseType _clauseType;
    private readonly object _value;
    private readonly ISqlClause[] _innerStatements;

    public WrappedClause(
        string column,
        CompOp op,
        object value,
        ClauseType clauseType,
        params ISqlClause[] innerStatements
    )
    {
        _column = column;
        _op = op;
        _clauseType = clauseType;
        _value = value;
        _innerStatements = innerStatements;
    }

    private string KeyWord => _clauseType.ToSql();

    public void ToSql(SqlBuilder builder)
    {
        var stringifiedValue =
            _value.GetType().IsNumericType() || _value.IsParameterBinding() ? _value.ToString() : $"'{_value}'";

        builder.Builder.AppendLine($"{KeyWord} (");
        builder.Builder.AppendLine($"{_column} {_op.ToSql()} {stringifiedValue}");

        foreach (var statement in _innerStatements)
        {
            statement.ToSql(builder);
        }

        builder.Builder.AppendLine(")");
    }
}
