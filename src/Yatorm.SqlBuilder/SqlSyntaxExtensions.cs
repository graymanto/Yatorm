namespace Yatorm.Tools;

public static class SqlSyntaxExtensions
{
    public static string EscapeCharLeft(this SqlSyntax syntax) =>
        syntax switch
        {
            SqlSyntax.SqlServer => "[",
            _ => "\""
        };

    public static string EscapeCharRight(this SqlSyntax syntax) =>
        syntax switch
        {
            SqlSyntax.SqlServer => "]",
            _ => "\""
        };

    public static string EscapeIdentifier(this SqlSyntax syntax, string value) =>
        $"{syntax.EscapeCharLeft()}{value}{syntax.EscapeCharRight()}";
}
