namespace Yatorm.Tools;

public class InClause<T> : ISqlClause
{
    private readonly ClauseType _clauseType;
    private readonly IEnumerable<T> _values;
    private readonly string _column;

    public InClause(string column, ClauseType clauseType, IEnumerable<T> values)
    {
        _clauseType = clauseType;
        _column = column;
        _values = values;
    }

    private string KeyWord => _clauseType.ToSql();

    public void ToSql(SqlBuilder builder)
    {
        string stringifiedValues = _values.StringifySqlValues().JoinWithComma();

        builder.Builder.AppendLine($"{KeyWord} {_column} IN ({stringifiedValues})");
    }
}
