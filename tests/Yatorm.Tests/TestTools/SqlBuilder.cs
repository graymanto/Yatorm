﻿using System.Text;
using Yatorm.Extensions;

namespace Yatorm.Tests.TestTools
{
    internal class SqlBuilder
    {
        private const string ParameterFormat = "@{0},";

        private const string ColumnNameFormat = "[{0}],";

        public static string InsertStatement<TEntity>()
        {
            var entityType = typeof(TEntity);
            const string InsertCommand = "insert into [{0}] ({1}) values ({2})";

            var tableName = entityType.Name;
            var tableColumns = FormatTableColumnNames(entityType);
            var parameterNames = FormatParameterNames(entityType);

            return InsertCommand.Formatted(tableName, tableColumns, parameterNames);
        }

        public static string TruncateStatement<TEntity>()
        {
            const string TruncateFormat = "truncate table [{0}]";

            return TruncateFormat.Formatted(typeof(TEntity).Name);
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
            var builder = new StringBuilder();

            entityType.GetProperties().ForEach(p => builder.AppendFormat(formatString, p.Name));

            builder.RemoveCharsFromEnd(1);
            return builder.ToString();
        }
    }
}
