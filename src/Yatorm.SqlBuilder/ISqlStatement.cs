namespace Yatorm.Tools;

public interface ISqlStatement
{
    internal ISqlClause? LastClause { get; }
    void AddClause(ISqlClause clause);
    void ToSql(SqlBuilder builder);
}
