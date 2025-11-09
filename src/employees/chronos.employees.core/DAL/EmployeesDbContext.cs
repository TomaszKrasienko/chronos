using chronos.employees.core.DAL.EntityTypeConfigurations;
using chronos.employees.core.Domain;
using Microsoft.EntityFrameworkCore;

namespace chronos.employees.core.DAL;

internal sealed class EmployeesDbContext(
    DbContextOptions<EmployeesDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
}
