using OpenBaseNET.Domain.Entities;
using OpenBaseNET.Domain.Interfaces.Repositories;
using OpenBaseNET.Infra.Data.Context;

namespace OpenBaseNET.Infra.Data.Repositories;

public sealed class CustomerRepository(DbSession session, OneBaseDataBaseContext context)
    : RepositoryBase<Customer>(session, context), ICustomerRepository, IDataRepository;
