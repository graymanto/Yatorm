namespace Yatorm.Tools;

public static class SqlBuilderExtensions
{
    /// <summary>
    /// Adds a WHERE clause to the SQL statement only if the given condition is true.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="column"></param>
    /// <param name="op"></param>
    /// <param name="value"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static SqlBuilder WhereOnCond(
        this SqlBuilder builder,
        string column,
        CompOp op,
        object value,
        bool condition
    ) => condition ? builder.Where(column, op, value) : builder;

    /// <summary>
    /// Adds a AND clause to the SQL statement only if the given condition is true.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="column"></param>
    /// <param name="op"></param>
    /// <param name="value"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static SqlBuilder AndOnCond(
        this SqlBuilder builder,
        string column,
        CompOp op,
        object value,
        bool condition
    ) => condition ? builder.And(column, op, value) : builder;
}
