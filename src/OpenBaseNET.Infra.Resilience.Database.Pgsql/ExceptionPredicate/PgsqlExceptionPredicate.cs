using Npgsql;

namespace OpenBaseNET.Infra.Resilience.Database.Pgsql.ExceptionPredicate;

internal static class PgsqlExceptionPredicate
{

    internal static bool ShouldRetryOn(PostgresException exception)
    {
        return exception.SqlState switch
        {
            DeadlockDetected
                or SerializationFailure
                or StatementCompletionUnknown
                or ConnectionException
                or ConnectionDoesNotExist
                or ConnectionFailure
                or ProtocolViolation
                or CannotConnectNow
                or AdminShutdown
                or CrashShutdown
                or TooManyConnections
                or OutOfMemory
                or DiskFull
                or InsufficientResources
                or ConfigurationLimitExceeded
                or QueryCanceled
                or OperatorIntervention => true,
            _ => false
        };
    }
    
    #region constantes

    // Class 40 — Transaction Rollback
    private const string DeadlockDetected = "40P01";
    private const string SerializationFailure = "40001";
    private const string StatementCompletionUnknown = "40003";
    
    // Class 08 — Connection Exception
    private const string ConnectionException = "08000";
    private const string ConnectionDoesNotExist = "08003";
    private const string ConnectionFailure = "08006";
    private const string ProtocolViolation = "08P01";
    
    // Class 53 — Insufficient Resources
    private const string InsufficientResources = "53000";
    private const string DiskFull = "53100";
    private const string OutOfMemory = "53200";
    private const string TooManyConnections = "53300";
    private const string ConfigurationLimitExceeded = "53400";
    
    // Class 57 — Operator Intervention
    private const string OperatorIntervention = "57000";
    private const string QueryCanceled = "57014";
    private const string AdminShutdown = "57P01";
    private const string CrashShutdown = "57P02";
    private const string CannotConnectNow = "57P03";

    #endregion constantes
}
