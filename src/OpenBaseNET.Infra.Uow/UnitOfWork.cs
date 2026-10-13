using Microsoft.EntityFrameworkCore;
using OpenBaseNET.Infra.Data.Context;
using OpenBaseNET.Infra.Uow.Interfaces;

namespace OpenBaseNET.Infra.Uow;

public sealed class UnitOfWork(DbSession session, OneBaseDataBaseContext context)
    : IUnitOfWork, IDisposable, IAsyncDisposable
{
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (session.Transaction is not null)
            throw new InvalidOperationException("A transaction is already active.");

        await session.OpenAsync(cancellationToken);
        var transaction = await session.Connection.BeginTransactionAsync(cancellationToken);
        try
        {
            await context.Database.UseTransactionAsync(transaction, cancellationToken);
            session.Transaction = transaction;
        }
        catch
        {
            await transaction.DisposeAsync();
            throw;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var transaction = session.Transaction
            ?? throw new InvalidOperationException("No transaction is active.");
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        await DisposeAsync();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        var transaction = session.Transaction
            ?? throw new InvalidOperationException("No transaction is active.");
        await transaction.RollbackAsync(cancellationToken);
        await DisposeAsync();
    }

    public void Dispose()
    {
        if (session.Transaction is not { } transaction) return;
        context.Database.UseTransaction(null);
        session.Transaction = null;
        transaction.Dispose();
        context.ChangeTracker.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (session.Transaction is not { } transaction) return;
        await context.Database.UseTransactionAsync(null);
        session.Transaction = null;
        await transaction.DisposeAsync();
        context.ChangeTracker.Clear();
    }
}
