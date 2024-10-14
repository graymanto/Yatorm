namespace Yatorm.Tools;

public static class ClauseTypeExtensions
{
    public static string ToSql(this ClauseType clauseType) =>
        clauseType switch
        {
            ClauseType.Where => "WHERE",
            ClauseType.And => "AND",
            ClauseType.Or => "OR",
            _ => throw new ArgumentOutOfRangeException(nameof(clauseType), clauseType, null)
        };
}