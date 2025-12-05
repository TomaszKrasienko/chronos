using chronos.employees.core.DAL;
using chronos.employees.core.Domain;
using Microsoft.EntityFrameworkCore;

namespace chronos.employees.core.Services;

public interface IEmployeeService
{
    Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        Ulid? supervisorId,
        CancellationToken cancellationToken);
    
    Task AssignSupervisorAsync(
        Ulid employeeId,
        Ulid supervisorId,
        CancellationToken cancellationToken);

    Task<Employee?> GetByIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken);
}

internal sealed class EmployeeService(
    EmployeesDbContext dbContext) : IEmployeeService
{
    public async Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        Ulid? supervisorId,
        CancellationToken cancellationToken)
    {
        var employee = Employee.Create(
            Ulid.NewUlid(),
            firstName,
            lastName,
            supervisorId);
        
        await dbContext.Employees.AddAsync(employee, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task AssignSupervisorAsync(
        Ulid employeeId,
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .SingleOrDefaultAsync(
                e => e.Id == employeeId,
                cancellationToken);
        
        if (employee is null)
        {
            throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
        }

        var doesSupervisorExist = await dbContext.Employees
            .AnyAsync(
                e => e.Id == supervisorId,
                cancellationToken);
        
        if (!doesSupervisorExist)
        {
            throw new InvalidOperationException($"Supervisor with ID {supervisorId} not found.");
        }
        
        employee.ChangeSupervisor(supervisorId);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Employee?> GetByIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .SingleOrDefaultAsync(
                e => e.Id == employeeId,
                cancellationToken);
    }
}