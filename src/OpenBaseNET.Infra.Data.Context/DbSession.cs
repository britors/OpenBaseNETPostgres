using System.Data;
using System.Data.Common;

namespace OpenBaseNET.Infra.Data.Context;

// The DI scope owns the connection. Creating a session does not open it.
public sealed class DbSession(DbConnection connection)
{
    public DbConnection Connection { get; } = connection;
    public DbTransaction? Transaction { get; set; }

    public async Task OpenAsync(CancellationToken cancellationToken = default)
    {
        if (Connection.State != ConnectionState.Open)
            await Connection.OpenAsync(cancellationToken);
    }
}
