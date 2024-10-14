namespace Yatorm.Tools;

public interface IWrappedClause
{
    public void AddClause(ISqlClause clause);
}

public class WrappedClause : ISqlClause, IWrappedClause
{
    private readonly string _column;
    private readonly CompOp _op;
    private readonly ClauseType _clauseType;
    private readonly object _value;
    private readonly List<ISqlClause> _innerStatements = [];

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
        _innerStatements.AddRange(innerStatements);
    }

    private string KeyWord => _clauseType.ToSql();

    public void AddClause(ISqlClause clause) => _innerStatements.Add(clause);

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

    public class NestedClause
    {
        private readonly IWrappedClause _wrappedClause;
        private readonly ClauseType _clauseType;

        public NestedClause(IWrappedClause wrappedClause, ClauseType clauseType)
        {
            _wrappedClause = wrappedClause;
            _clauseType = clauseType;
        }

        public IWrappedClause Where(string column, CompOp op, object value)
        {
            var wrappedClause = new WrappedClause(column, op, value, _clauseType);
            _wrappedClause.AddClause(wrappedClause);
            return wrappedClause;
        }

        public IWrappedClause Where(string sql)
        {
            var wrappedClause = new RawWrappedClause(sql, _clauseType);
            _wrappedClause.AddClause(wrappedClause);
            return wrappedClause;
        }
    }
}

public static class WrappedClauseExtensions
{
    public static IWrappedClause And(this IWrappedClause clause, string column, CompOp op, object value)
    {
        clause.AddClause(new WhereClause(column, op, value, ClauseType.And));
        return clause;
    }

    public static IWrappedClause Or(this IWrappedClause clause, string column, CompOp op, object value)
    {
        clause.AddClause(new WhereClause(column, op, value, ClauseType.Or));
        return clause;
    }

    public static IWrappedClause And(this IWrappedClause clause, Action<WrappedClause.NestedClause> nested)
    {
        var nestedClause = new WrappedClause.NestedClause(clause, ClauseType.And);
        nested(nestedClause);
        return clause;
    }

    public static IWrappedClause Or(this IWrappedClause clause, Action<WrappedClause.NestedClause> nested)
    {
        var nestedClause = new WrappedClause.NestedClause(clause, ClauseType.Or);
        nested(nestedClause);
        return clause;
    }
}

public class RawWrappedClause : ISqlClause, IWrappedClause
{
    private readonly string _sql;
    private readonly ClauseType _clauseType;
    private readonly List<ISqlClause> _innerStatements = [];

    public RawWrappedClause(string sql, ClauseType clauseType, params ISqlClause[] innerStatements)
    {
        _sql = sql;
        _clauseType = clauseType;
        _innerStatements.AddRange(innerStatements);
    }

    private string KeyWord => _clauseType.ToSql();

    public void AddClause(ISqlClause clause) => _innerStatements.Add(clause);

    public void ToSql(SqlBuilder builder)
    {
        builder.Builder.AppendLine($"{KeyWord} (");
        builder.Builder.AppendLine(_sql);

        foreach (var statement in _innerStatements)
        {
            statement.ToSql(builder);
        }

        builder.Builder.AppendLine(")");
    }

    public class NestedClause
    {
        private readonly RawWrappedClause _wrappedClause;
        private readonly ClauseType _clauseType;

        public NestedClause(RawWrappedClause wrappedClause, ClauseType clauseType)
        {
            _wrappedClause = wrappedClause;
            _clauseType = clauseType;
        }

        public IWrappedClause Where(string column, CompOp op, object value)
        {
            var wrappedClause = new WrappedClause(column, op, value, _clauseType);
            _wrappedClause.AddClause(wrappedClause);
            return wrappedClause;
        }

        public IWrappedClause Where(string sql)
        {
            var wrappedClause = new RawWrappedClause(sql, _clauseType);
            _wrappedClause.AddClause(wrappedClause);
            return wrappedClause;
        }
    }
}
