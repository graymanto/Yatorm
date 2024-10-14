namespace Yatorm.Tools;

public class InsertStatement : ISqlStatement
{
    private readonly string _tableName;
    private readonly List<ISqlClause> _clauses = new();

    private (string, object?)[] InsertValues { get; }

    // public static InsertStatement FromType<T>()
    // {
    //     return new InsertStatement();
    // }

    public InsertStatement(string tableName, (string, object?)[] insertValues)
    {
        _tableName = tableName;
        InsertValues = insertValues;
    }

    public ISqlClause? LastClause => _clauses.LastOrDefault();

    public void AddClause(ISqlClause clause) => _clauses.Add(clause);

    public void ToSql(SqlBuilder builder)
    {
        var escapeLeft = builder.Syntax.EscapeCharLeft();
        var escapeRight = builder.Syntax.EscapeCharRight();

        var columns = string.Join(", ", InsertValues.Select(c => $"{escapeLeft}{c.Item1}{escapeRight}"));
        var values = string.Join(", ", InsertValues.Select(i => $"{i.Item2.StringifySqlValue(builder.Syntax)}"));
        var fullTableName = $"{escapeLeft}{_tableName}{escapeRight}";

        builder.Builder.AppendLine($"INSERT INTO {fullTableName} ({columns}) VALUES ({values})");

        // TODO: not yet valid for insert statements. Need to complete version with clauses
        foreach (var clause in _clauses)
        {
            clause.ToSql(builder);
        }
    }
}
