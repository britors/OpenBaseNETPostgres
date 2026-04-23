using OpenBaseNET.Infra.Resilience.Core.Pipelines;
using OpenBaseNET.Infra.Resilience.Database.Pgsql.ExceptionPredicate;
using Polly;
using Npgsql;

namespace OpenBaseNET.Infra.Resilience.Database.Pgsql.Pipelines;

public static class DatabasePipeline
{
    public static readonly ResiliencePipeline AsyncRetryPipeline =
        BasePipeline<PostgresException>.GetAsyncRetryPipeline(PgsqlExceptionPredicate.ShouldRetryOn);
}
