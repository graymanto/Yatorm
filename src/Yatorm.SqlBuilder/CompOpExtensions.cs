namespace Yatorm.Tools;

public static class CompOpExtensions
{
    public static string ToSql(this CompOp op) =>
        op switch
        {
            CompOp.Eq => "=",
            CompOp.Neq => "!=",
            CompOp.Gt => ">",
            CompOp.Gte => ">=",
            CompOp.Lt => "<",
            CompOp.Lte => "<=",
            CompOp.Like => "LIKE",
            CompOp.Is => "IS",
            CompOp.IsNot => "IS NOT",
            _ => throw new NotImplementedException()
        };
}
