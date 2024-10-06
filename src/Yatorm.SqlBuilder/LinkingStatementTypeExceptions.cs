namespace Yatorm.Tools;

public static class LinkingStatementTypeExceptions
{
    public static string ToSql(this LinkingStatementType type)
    {
        return type switch
        {
            LinkingStatementType.Union => "UNION",
            LinkingStatementType.UnionAll => "UNION ALL",
            LinkingStatementType.Intersect => "INTERSECT",
            LinkingStatementType.IntersectAll => "INTERSECT ALL",
            LinkingStatementType.Except => "EXCEPT",
            LinkingStatementType.ExceptAll => "EXCEPT ALL",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}