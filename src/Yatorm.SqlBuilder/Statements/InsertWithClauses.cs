namespace Yatorm.Tools;

public class InsertWithClauses : ISqlStatement
{
    private readonly string _tableName;
    private readonly string[] _columns;
    private readonly List<ISqlClause> _clauses = new();

    public InsertWithClauses(string tableName, string[] columns)
    {
        _tableName = tableName;
        _columns = columns;
    }

    public ISqlClause? LastClause => _clauses.LastOrDefault();

    public void AddClause(ISqlClause clause) => _clauses.Add(clause);

    public void ToSql(SqlBuilder builder)
    {
        var columns = string.Join(", ", _columns.Select(c => builder.Syntax.EscapeIdentifier(c)));
        var fullTableName = builder.Syntax.EscapeIdentifier(_tableName);

        builder.Builder.AppendLine($"INSERT INTO {fullTableName} ({columns})");

        if (_clauses.Any(c => c is ValuesClause))
        {
            builder.Builder.Append("VALUES ");
            foreach (var clause in _clauses.Where(c => c is ValuesClause))
            {
                clause.ToSql(builder);
                builder.Builder.AppendLine(",");
            }

            builder.Builder.Length -= 2;
        }
        else
        {
            foreach (var clause in _clauses)
            {
                clause.ToSql(builder);
            }
        }
    }
}
