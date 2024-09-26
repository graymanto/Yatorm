using System.Reflection;
using System.Text;
using Yatorm.Extensions;

namespace Yatorm
{
    public class SqlGenerator
    {
        private const string ParameterFormat = "@{0},";

        private const string ColumnNameFormat = "[{0}],";

        public static string GetInsertStatementForEntity<TEntity>()
        {
            var entityType = typeof(TEntity);
            const string InsertCommand = "insert into [{0}] ({1}) values ({2})";

            var tableName = entityType.Name;
            var tableColumns = FormatTableColumnNames(entityType);
            var parameterNames = FormatParameterNames(entityType);

            return InsertCommand.Formatted(tableName, tableColumns, parameterNames);
        }

        public static string GetUpdateStatementForEntity<TEntity>(TEntity entity, IEnumerable<string> excludeColumns)
        {
            var entityType = typeof(TEntity);
            const string UpdateCommand = "update [{0}] set {1}";

            var tableName = entityType.Name;
            var updateValues = BuildUpdateValues(entity.GetType(), excludeColumns);

            return UpdateCommand.Formatted(tableName, updateValues);
        }

        private static string BuildUpdateValues(Type entityType, IEnumerable<string> excludeColumns)
        {
            var requiredProps = entityType.GetProperties().Where(p => !excludeColumns.Contains(p.Name));
            return BuildStringOverPropertyNames(requiredProps, "[{0}] = @{0},");
        }

        private static string FormatTableColumnNames(Type entityType)
        {
            return BuildStringOverPropertyNames(entityType, ColumnNameFormat);
        }

        private static string FormatParameterNames(Type entityType)
        {
            return BuildStringOverPropertyNames(entityType, ParameterFormat);
        }

        private static string BuildStringOverPropertyNames(Type entityType, string formatString)
        {
            return BuildStringOverPropertyNames(entityType.GetProperties(), formatString);
        }

        private static string BuildStringOverPropertyNames(IEnumerable<PropertyInfo> properties, string formatString)
        {
            var builder = new StringBuilder();

            properties.ForEach(p => builder.AppendFormat(formatString, p.Name));

            builder.RemoveCharsFromEnd(1);
            return builder.ToString();
        }
    }
}
