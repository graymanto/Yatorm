using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Yatorm.Extensions;

namespace Yatorm;

public static class IDbConnectionExtensions
{
    public static IDbCommand BuildCommand(
        this IDbConnection conn,
        string commandText,
        IEnumerable<SqlParameter>? parameters = null,
        CommandType commandType = CommandType.StoredProcedure
    )
    {
        var cmd = conn.CreateCommand();
        cmd.CommandType = commandType;
        cmd.CommandText = commandText;

        parameters?.ForEach(p => cmd.Parameters.Add(p));

        return cmd;
    }

    public static IQueryable<TResult> Query<TResult>(this IDbConnection connection, string query)
        where TResult : new() => connection.ExecuteMappedCommand<TResult>(query);

    public static Task<IQueryable<TResult>> QueryAsync<TResult>(this IDbConnection connection, string query)
        where TResult : new() => connection.ExecuteMappedCommandAsync<TResult>(query);

    public static IQueryable<TResult> Query<TResult>(this IDbConnection connection, string query, dynamic? parameters)
        where TResult : new()
    {
        var commandParams = parameters == null ? null : DBToTypeConverter.TransformClassToSqlParameters(parameters);
        return ExecuteMappedCommand<TResult>(connection, query, CommandType.Text, commandParams);
    }

    public static Task<IQueryable<TResult>> QueryAsync<TResult>(
        this IDbConnection connection,
        string query,
        dynamic? parameters
    )
        where TResult : new()
    {
        var commandParams = parameters == null ? null : DBToTypeConverter.TransformClassToSqlParameters(parameters);
        return ExecuteMappedCommandAsync<TResult>(connection, query, CommandType.Text, commandParams);
    }

    public static TResult Single<TResult>(this IDbConnection connection, string query)
        where TResult : new() => ExecuteMappedCommand<TResult>(connection, query).First();

    public static async Task<TResult> SingleAsync<TResult>(this IDbConnection connection, string query)
        where TResult : new() => (await ExecuteMappedCommandAsync<TResult>(connection, query)).First();

    public static IQueryable<TResult> ExecStoredProc<TResult>(
        this IDbConnection connection,
        string procname,
        dynamic? parameters
    )
        where TResult : new()
    {
        var commandParams = parameters == null ? null : DBToTypeConverter.TransformClassToSqlParameters(parameters);

        return ExecuteMappedCommand<TResult>(connection, procname, CommandType.StoredProcedure, commandParams);
    }

    public static TResult? ExecScalarStoredProc<TResult>(
        this IDbConnection connection,
        string procname,
        object? parameters = null
    )
        where TResult : new()
    {
        var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = procname;

        if (parameters != null)
        {
            var commandParams = DBToTypeConverter.TransformClassToSqlParameters(parameters);
            if (commandParams != null)
            {
                commandParams.ForEach(p => cmd.Parameters.Add(p));
            }
        }

        var result = cmd.ExecuteScalar();

        // TODO: better conversion
        return (TResult)Convert.ChangeType(result, typeof(TResult))!;
    }

    public static int ExecNonQueryProc(this IDbConnection connection, string procname, object? parameters = null)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = procname;

        if (parameters != null)
        {
            var commandParams = DBToTypeConverter.TransformClassToSqlParameters(parameters);
            if (commandParams != null)
            {
                commandParams.ForEach(p => cmd.Parameters.Add(p));
            }
        }

        return cmd.ExecuteNonQuery();
    }

    public static int ExecuteNonQuery(this IDbConnection connection, string query, object? parameters = null)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = query;

        if (parameters != null)
        {
            var commandParams =
                parameters as IEnumerable<SqlParameter> ?? DBToTypeConverter.TransformClassToSqlParameters(parameters);

            if (commandParams != null)
            {
                commandParams.ForEach(p => cmd.Parameters.Add(p));
            }
        }

        return cmd.ExecuteNonQuery();
    }

    public static async Task<int> ExecuteNonQueryAsync(
        this IDbConnection connection,
        string query,
        object? parameters = null
    )
    {
        var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = query;

        if (parameters != null)
        {
            var commandParams =
                parameters as IEnumerable<SqlParameter> ?? DBToTypeConverter.TransformClassToSqlParameters(parameters);

            if (commandParams != null)
            {
                commandParams.ForEach(p => cmd.Parameters.Add(p));
            }
        }

        var dbCommand = (DbCommand)cmd;
        return await dbCommand.ExecuteNonQueryAsync();
    }

    private static async Task<IQueryable<TResult>> ExecuteMappedCommandAsync<TResult>(
        this IDbConnection connection,
        string commandText,
        CommandType commandType = CommandType.Text,
        IEnumerable<SqlParameter>? parameters = null
    )
        where TResult : new()
    {
        var cmd = connection.BuildCommand(commandText, parameters, commandType);
        var dbCommand = (DbCommand)cmd;

        var rdr = await dbCommand.ExecuteReaderAsync();

        var resultSet = rdr.GetMappedSequence<TResult>();

        return resultSet;
    }

    private static IQueryable<TResult> ExecuteMappedCommand<TResult>(
        this IDbConnection connection,
        string commandText,
        CommandType commandType = CommandType.Text,
        IEnumerable<SqlParameter>? parameters = null
    )
        where TResult : new()
    {
        var cmd = connection.BuildCommand(commandText, parameters, commandType);

        var rdr = cmd.ExecuteReader();

        var resultSet = rdr.GetMappedSequence<TResult>();

        return resultSet;
    }
}
