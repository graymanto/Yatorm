using System.Text;

namespace Yatorm.Tools;

public class SqlBuilder
{
    private readonly List<ISqlStatement> _statements = [];
    private ISqlStatement? _activeStatement;

    internal readonly StringBuilder Builder = new();

    public SqlSyntax Syntax { get; private set; } = SqlSyntax.Standard;

    public SqlBuilder WithSyntax(SqlSyntax syntax)
    {
        Syntax = syntax;
        return this;
    }

    public SqlBuilder Select()
    {
        AddStatement(new SelectStatement("*"));
        return this;
    }

    public SqlBuilder Select(params string[] columns)
    {
        AddStatement(new SelectStatement(columns));
        return this;
    }

    public SqlBuilder Union()
    {
        AddStatement(new LinkingStatement(LinkingStatementType.Union));
        return this;
    }

    public SqlBuilder UnionAll()
    {
        AddStatement(new LinkingStatement(LinkingStatementType.UnionAll));
        return this;
    }

    public SqlBuilder Intersect()
    {
        AddStatement(new LinkingStatement(LinkingStatementType.Intersect));
        return this;
    }

    public SqlBuilder IntersectAll()
    {
        AddStatement(new LinkingStatement(LinkingStatementType.IntersectAll));
        return this;
    }

    public SqlBuilder Except()
    {
        AddStatement(new LinkingStatement(LinkingStatementType.Except));
        return this;
    }

    public SqlBuilder ExceptAll()
    {
        AddStatement(new LinkingStatement(LinkingStatementType.ExceptAll));
        return this;
    }

    public SqlBuilder From(string table, string alias)
    {
        _activeStatement?.AddClause(new FromClause(table, alias));
        return this;
    }

    public SqlBuilder From(string table)
    {
        _activeStatement?.AddClause(new FromClause(table));
        return this;
    }

    public SqlBuilder Where(string column, CompOp op, object value)
    {
        var hasClause = _statements.LastOrDefault()?.LastClause is WhereClause;
        var clauseType = hasClause ? ClauseType.And : ClauseType.Where;
        _statements.LastOrDefault()?.AddClause(new WhereClause(column, op, value, clauseType));
        return this;
    }

    public SqlBuilder And(string column, CompOp op, object value)
    {
        _activeStatement?.AddClause(new WhereClause(column, op, value, ClauseType.And));
        return this;
    }

    public SqlBuilder Or(string column, CompOp op, object value)
    {
        _activeStatement?.AddClause(new WhereClause(column, op, value, ClauseType.Or));
        return this;
    }

    public SqlBuilder AndWrapped(string column, CompOp op, object value, params ISqlClause[] innerStatements)
    {
        _activeStatement?.AddClause(new WrappedClause(column, op, value, ClauseType.And, innerStatements));
        return this;
    }

    public SqlBuilder AndLike(string column, string value)
    {
        _activeStatement?.AddClause(new WhereClause(column, CompOp.Like, value, ClauseType.And));
        return this;
    }

    //TODO: this thould really be generic to enforce type safety so all values are of the same type
    public SqlBuilder AndIn<T>(string column, IEnumerable<T> values)
    {
        _activeStatement?.AddClause(new InClause<T>(column, ClauseType.And, values));
        return this;
    }

    public SqlBuilder OrderBy(params string[] columns)
    {
        _activeStatement?.AddClause(new OrderByClause(columns));
        return this;
    }

    public SqlBuilder OrderBy(params int[] columns)
    {
        _activeStatement?.AddClause(new OrderByClause(columns));
        return this;
    }

    public SqlBuilder GroupBy(params string[] columns)
    {
        _activeStatement?.AddClause(new GroupByClause(columns));
        return this;
    }

    public SqlBuilder GroupBy(params int[] columns)
    {
        _activeStatement?.AddClause(new GroupByClause(columns));
        return this;
    }

    public string ToSql()
    {
        foreach (var statement in _statements)
        {
            statement.ToSql(this);
        }

        return Builder.ToString().TrimEnd();
    }

    private void AddStatement(ISqlStatement statement)
    {
        _statements.Add(statement);
        _activeStatement = statement;
    }
}
