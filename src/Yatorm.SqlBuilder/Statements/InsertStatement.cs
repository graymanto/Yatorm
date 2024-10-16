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
        var columns = string.Join(", ", InsertValues.Select(c => builder.Syntax.EscapeIdentifier(c.Item1)));
        var values = string.Join(", ", InsertValues.Select(i => $"{i.Item2.StringifySqlValue(builder.Syntax)}"));
        var fullTableName = builder.Syntax.EscapeIdentifier(_tableName);

        builder.Builder.AppendLine($"INSERT INTO {fullTableName} ({columns}) VALUES ({values})");

        foreach (var clause in _clauses.Where(c => c is ValuesClause))
        {
            clause.ToSql(builder);
            builder.Builder.AppendLine(",");
        }

        builder.Builder.Length--;
    }
}
