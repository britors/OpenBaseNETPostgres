# Changelog

## 10.4.0

- Correct project/namespace/solution replacement when generating a named template.
- Remove unconfigured authentication middleware; add a Customer API and initial migration.
- Share the scoped connection between EF Core and Dapper, open connections lazily,
  and release transaction state after commit, rollback or disposal.
- Pass cancellation to Dapper commands and unit-of-work operations.
- Require explicit ordering and validate pagination arguments.
- Retry transient HTTP responses and dispose discarded responses; make POST replay opt-in.
- Register MediatR once per assembly, including void requests and notifications.
- Add request timing logs, remove duplicate console logging, and run validators sequentially.
- Remove Application references to infrastructure adapters and remove unused Polly.Contrib.WaitAndRetry.
- Centralize and update stable NuGet dependencies; pin the .NET 10.0.401 SDK.
- Add regression/integration tests and validate a generated application before NuGet publication.

### Migration notes

- Paginated repository calls must supply `orderBy`; it precedes the `includes` parameter.
- `OneBaseDataBaseContext` now accepts `DbSession` rather than `IConfiguration`.
- `DbSession` no longer opens/disposes its DI-owned connection in its constructor/disposal.
- Unit-of-work methods accept an optional cancellation token. Commit saves pending EF changes.
- HTTP POST retries now require `allowRetry: true` and server-side idempotency support.
- The generated solution now uses the requested application name.
