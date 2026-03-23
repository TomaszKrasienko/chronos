using chronos.contracts.core.Domain;
using chronos.contracts.core.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace chronos.contracts.core.DAL;

internal sealed class ContractsRepository(ContractsDbContext dbContext) : IContractsRepository
{
    public async Task<Contract?> GetByIdAsync(ContractId id, CancellationToken cancellationToken = default)
        => await dbContext.Contracts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> ExistsByCompanyNameAsync(string companyName, CancellationToken cancellationToken = default)
        => await dbContext.Contracts.AnyAsync(x => x.CompanyDetails.Name == companyName, cancellationToken);

    public async Task AddAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        await dbContext.Contracts.AddAsync(contract, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        dbContext.Contracts.Update(contract);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
