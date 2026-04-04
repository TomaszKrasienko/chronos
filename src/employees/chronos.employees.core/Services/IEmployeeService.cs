using chronos.employees.core.DAL;
using chronos.employees.core.Domain;
using chronos.employees.core.Domain.Identifiers;
using chronos.employees.core.Domain.ValueObjects;
using chronos.employees.core.Events;
using chronos.shared.kernel.Exceptions;
using chronos.shared.messaging;
using Microsoft.EntityFrameworkCore;

namespace chronos.employees.core.Services;

/// <summary>
/// Service for managing employee operations
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Creates a new employee
    /// </summary>
    /// <param name="firstName">Employee's first name</param>
    /// <param name="lastName">Employee's last name</param>
    /// <param name="email">Employee's email</param>
    /// <param name="supervisorId">Optional supervisor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created employee</returns>
    Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Assigns a supervisor to an employee
    /// </summary>
    /// <param name="employeeId">Employee's ID</param>
    /// <param name="supervisorId">Supervisor's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AssignSupervisorAsync(
        Ulid employeeId,
        Ulid supervisorId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an employee by ID
    /// </summary>
    /// <param name="employeeId">Employee's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Employee if found, otherwise null</returns>
    Task<Employee?> GetByIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all employees
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all employees</returns>
    Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all subordinates for a supervisor
    /// </summary>
    /// <param name="supervisorId">Supervisor's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of employees reporting to the supervisor</returns>
    Task<IReadOnlyCollection<Employee>> GetSubordinatesAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an employee (soft delete)
    /// </summary>
    /// <param name="employeeId">Employee's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAsync(Ulid employeeId, CancellationToken cancellationToken);
}

internal sealed class EmployeeService(
    EmployeesDbContext dbContext,
    IMessageDispatcher messageDispatcher) : IEmployeeService
{
    public async Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken)
    {
        var fullName = FullName.Create(firstName, lastName);
        var emailVo = Email.Create(email);
        var employee = Employee.Create(fullName, emailVo);

        if (supervisorId.HasValue)
        {
            employee.AssignSupervisor(new EmployeeId(supervisorId.Value));
        }

        if (await dbContext.Employees.AnyAsync(x
                => x.FullName.FirstName == firstName
                && x.FullName.LastName == lastName, cancellationToken))
        {
            throw new NotUniqueException("employee", [firstName, lastName]);
        }

        var @event = new EmployeeCreated(
            employee.Id.Value,
            employee.FullName.FirstName,
            employee.FullName.LastName,
            employee.Email.Value,
            employee.SupervisorId?.Value);

        await dbContext.Employees.AddAsync(employee, cancellationToken);
        await messageDispatcher.Send(@event, cancellationToken);
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
                e => e.Id == new EmployeeId(employeeId),
                cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException("employee", [employeeId.ToString()]);
        }

        var doesSupervisorExist = await dbContext.Employees
            .AnyAsync(
                e => e.Id == new EmployeeId(supervisorId),
                cancellationToken);

        if (!doesSupervisorExist)
        {
            throw new NotFoundException("supervisor_employee", [supervisorId.ToString()]);
        }

        employee.AssignSupervisor(new EmployeeId(supervisorId));

        var @event = new SupervisorAssigned(
            employeeId,
            supervisorId);

        await messageDispatcher.Send(@event, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Employee?> GetByIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .SingleOrDefaultAsync(
                e => e.Id == new EmployeeId(employeeId),
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Employee>> GetSubordinatesAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .Where(e => e.SupervisorId == new EmployeeId(supervisorId))
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Ulid employeeId, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .SingleOrDefaultAsync(
                e => e.Id == new EmployeeId(employeeId),
                cancellationToken);

        if (employee is null)
        {
            return;
        }

        employee.Delete();

        var @event = new EmployeeDeleted(employeeId);

        await messageDispatcher.Send(@event, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
