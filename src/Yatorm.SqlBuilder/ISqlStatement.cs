namespace Yatorm.Tools;

public interface ISqlStatement
{
    ISqlClause? LastClause { get; }
    void AddClause(ISqlClause clause);
    void ToSql(SqlBuilder builder);
}