namespace Yatorm.Tools;

public class SelectStatement : ISqlStatement
{
    private readonly List<ISqlClause> _clauses = new();

    private string[] Columns { get; }

    public static SelectStatement FromType<T>()
    {
        return new SelectStatement();
    }

    public SelectStatement(params string[] columns)
    {
        Columns = columns;
    }

    public ISqlClause? LastClause => _clauses.LastOrDefault();

    public void AddClause(ISqlClause clause) => _clauses.Add(clause);

    public void ToSql(SqlBuilder builder)
    {
        var escapeLeft = builder.Syntax.EscapeCharLeft();
        var escapeRight = builder.Syntax.EscapeCharRight();

        var columns = string.Join(", ", Columns.Select(c => $"{escapeLeft}{c}{escapeRight}"));

        builder.Builder.AppendLine($"SELECT {columns}");

        foreach (var clause in _clauses)
        {
            clause.ToSql(builder);
        }
    }
}
