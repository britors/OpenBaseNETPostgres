using Npgsql;
using OpenBaseNET.Infra.Resilience.Core.Pipelines;
using OpenBaseNET.Infra.Resilience.Database.Postres.ExceptionPredicate;
using Polly;

namespace OpenBaseNET.Infra.Resilience.Database.Postres.Pipelines;

public static class DatabasePipeline
{
    public static readonly ResiliencePipeline AsyncRetryPipeline =
        BasePipeline<PostgresException>.GetAsyncRetryPipeline(PostgresExceptionPredicate.ShouldRetryOn);
}