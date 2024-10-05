using System.Text;

namespace Yatorm.Tools;

public class SqlBuilder
{
    private readonly List<ISqlStatement> _statements = [];

    internal readonly StringBuilder Builder = new();

    public SqlSyntax Syntax { get; private set; } = SqlSyntax.Standard;

    public SqlBuilder WithSyntax(SqlSyntax syntax)
    {
        Syntax = syntax;
        return this;
    }

    public SqlBuilder Select()
    {
        _statements.Add(new SelectStatement("*"));
        return this;
    }

    public SqlBuilder Select(params string[] columns)
    {
        _statements.Add(new SelectStatement(columns));
        return this;
    }

    public SqlBuilder From(string table, string alias)
    {
        _statements.LastOrDefault()?.AddClause(new FromClause(table, alias));
        return this;
    }

    public SqlBuilder From(string table)
    {
        _statements.LastOrDefault()?.AddClause(new FromClause(table));
        return this;
    }

    public SqlBuilder Where(string column, CompOp op, object value)
    {
        var hasClause = _statements.LastOrDefault()?.LastClause is WhereClause;
        _statements.LastOrDefault()?.AddClause(new WhereClause(column, op, value, hasClause));
        return this;
    }

    public SqlBuilder And(string column, CompOp op, object value)
    {
        _statements.LastOrDefault()?.AddClause(new WhereClause(column, op, value, true));
        return this;
    }

    public SqlBuilder OrderBy(params string[] columns)
    {
        _statements.LastOrDefault()?.AddClause(new OrderByClause(columns));
        return this;
    }

    public SqlBuilder OrderBy(params int[] columns)
    {
        _statements.LastOrDefault()?.AddClause(new OrderByClause(columns));
        return this;
    }

    public string ToSql()
    {
        _statements.LastOrDefault()?.ToSql(this);
        return Builder.ToString().TrimEnd();
    }
}
