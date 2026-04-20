using chronos.contracts.core.Domain;
using chronos.shared.kernel.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.contracts.core.DAL;

internal sealed class ContractsDbContext(
    DbContextOptions<ContractsDbContext> options) : DbContext(options)
{
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var contractIdConverter = new ValueConverter<ContractId, string>(
            v => v.Value.ToString(),
            v => new ContractId(Ulid.Parse(v)));
        
        var employeeIdConverter = new ValueConverter<EmployeeId, string>(
            v => v.Value.ToString(),
            v => new EmployeeId(Ulid.Parse(v)));

        var dateOnlyConverter = new ValueConverter<DateOnly, string>(
            v => v.ToString("yyyy-MM-dd"),
            v => DateOnly.Parse(v));

        modelBuilder
            .Entity<Contract>()
            .ToCollection("contracts");

        modelBuilder
            .Entity<Contract>()
            .HasKey(x => x.Id);

        modelBuilder
            .Entity<Contract>()
            .Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion(contractIdConverter);

        modelBuilder
            .Entity<Contract>()
            .OwnsOne(x => x.CompanyDetails, companyDetails =>
            {
                companyDetails.Property(c => c.Name)
                    .IsRequired()
                    .HasElementName("CompanyName");
            });

        modelBuilder
            .Entity<Contract>()
            .OwnsOne(x => x.ContractPeriod, contractPeriod =>
            {
                contractPeriod.Property(c => c.AssignmentDate)
                    .IsRequired()
                    .HasElementName("AssignmentDate")
                    .HasConversion(dateOnlyConverter);

                contractPeriod.Property(c => c.ClosingDate)
                    .HasElementName("ClosingDate")
                    .HasConversion(dateOnlyConverter);
            });

        modelBuilder
            .Entity<Contract>()
            .OwnsMany(x => x.Employees, employee =>
            {
                employee.Property(e => e.Id)
                    .HasElementName("_id")
                    .HasConversion(employeeIdConverter);

                employee.Property(e => e.AllocatedHours)
                    .IsRequired()
                    .HasElementName("AllocatedHours");

                employee.OwnsOne(e => e.AssignmentPeriod, assignmentPeriod =>
                {
                    assignmentPeriod.Property(a => a.From)
                        .IsRequired()
                        .HasElementName("From")
                        .HasConversion(dateOnlyConverter);

                    assignmentPeriod.Property(a => a.To)
                        .IsRequired()
                        .HasElementName("To")
                        .HasConversion(dateOnlyConverter);
                });
            });
    }
}
