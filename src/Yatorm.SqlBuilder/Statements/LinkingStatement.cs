namespace Yatorm.Tools;

public class LinkingStatement(LinkingStatementType type) : ISqlStatement
{
    public ISqlClause? LastClause => null;

    public void AddClause(ISqlClause clause)
    {
        throw new InvalidOperationException("Linking statements do not support clauses. Add Select instead");
    }

    public void ToSql(SqlBuilder builder) => builder.Builder.AppendLine(type.ToSql());
}
