using chronos.employees.core.Domain;
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
        var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

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
            .HasConversion(ulidConverter);
        
        modelBuilder
            .Entity<Employee>()
            .Property(x => x.FirstName)
            .IsRequired()
            .HasElementName("FirstName");

        modelBuilder
            .Entity<Employee>()
            .Property(x => x.LastName)
            .IsRequired()
            .HasElementName("LastName");

        modelBuilder
            .Entity<Employee>()
            .Property(x => x.SupervisorId)
            .HasElementName("SupervisorId")
            .HasConversion(ulidConverter);
    }
}
