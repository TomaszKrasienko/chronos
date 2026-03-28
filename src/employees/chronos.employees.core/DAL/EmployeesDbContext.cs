using chronos.employees.core.Domain;
using chronos.employees.core.Domain.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.employees.core.DAL;

internal sealed class EmployeesDbContext(
    DbContextOptions<EmployeesDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var employeeIdConverter = new ValueConverter<EmployeeId, string>(
            v => v.Value.ToString(),
            v => new EmployeeId(Ulid.Parse(v)));

        modelBuilder
            .Entity<Employee>()
            .ToCollection("employees");

        modelBuilder
            .Entity<Employee>()
            .HasKey(x => x.Id);

        modelBuilder
            .Entity<Employee>()
            .Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion(employeeIdConverter);

        modelBuilder
            .Entity<Employee>()
            .OwnsOne(x => x.FullName, fullName =>
            {
                fullName.Property(f => f.FirstName)
                    .IsRequired()
                    .HasElementName("FirstName");

                fullName.Property(f => f.LastName)
                    .IsRequired()
                    .HasElementName("LastName");
            });

        modelBuilder
            .Entity<Employee>()
            .OwnsOne(x => x.Email, email =>
            {
                email.Property(e => e.Value)
                    .IsRequired()
                    .HasElementName("Email");
            });

        modelBuilder
            .Entity<Employee>()
            .Property(x => x.SupervisorId)
            .HasElementName("SupervisorId")
            .HasConversion(employeeIdConverter);

        modelBuilder
            .Entity<Employee>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}
