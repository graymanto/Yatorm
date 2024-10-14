namespace Yatorm.Tools;

public interface ISqlClause
{
    void ToSql(SqlBuilder builder);
}