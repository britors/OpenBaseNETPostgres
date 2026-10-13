using Dapper;
using OpenBaseNET.Infra.Resilience.Database.Pgsql.Pipelines;
using System.Data;

namespace OpenBaseNET.Infra.Dapper.Extension;

public static class DapperExtension
{
    public static async Task<int> ExecuteAsyncWithRetry(
        this IDbConnection connection,
        CancellationToken cancellationToken,
        string? sql = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        object? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentException($"'{nameof(sql)}' cannot be null or empty.", nameof(sql));

        // An aborted transaction must be retried as a whole by its owner.
        if (transaction is not null)
            return await connection.ExecuteAsync(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: cancellationToken));

        return await DatabasePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await connection.ExecuteAsync(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: token)),
            cancellationToken);
    }

    public static async Task<IEnumerable<T>> QueryAsyncWithRetry<T>(
        this IDbConnection connection,
        CancellationToken cancellationToken,
        string? sql = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        object? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentException($"'{nameof(sql)}' cannot be null or empty.", nameof(sql));

        // An aborted transaction must be retried as a whole by its owner.
        if (transaction is not null)
            return await connection.QueryAsync<T>(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: cancellationToken));

        return await DatabasePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await connection.QueryAsync<T>(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: token)),
            cancellationToken);
    }

    public static async Task<T?> QueryFirstOrDefaultAsyncWithRetry<T>(
        this IDbConnection connection,
        CancellationToken cancellationToken,
        string? sql = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        object? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentException($"'{nameof(sql)}' cannot be null or empty.", nameof(sql));

        // An aborted transaction must be retried as a whole by its owner.
        if (transaction is not null)
            return await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: cancellationToken));

        return await DatabasePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: token)),
            cancellationToken);
    }

    public static async Task<T?> QuerySingleOrDefaultAsyncWithRetry<T>(
        this IDbConnection connection,
        CancellationToken cancellationToken,
        string? sql = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        object? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentException($"'{nameof(sql)}' cannot be null or empty.", nameof(sql));

        // An aborted transaction must be retried as a whole by its owner.
        if (transaction is not null)
            return await connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: cancellationToken));

        return await DatabasePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
                transaction, commandTimeout, commandType, cancellationToken: token)),
            cancellationToken);
    }
}