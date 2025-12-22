using chronos.employees.core.DAL;
using chronos.employees.core.Domain;
using chronos.employees.core.Events;
using chronos.employees.core.Exceptions;
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
}

internal sealed class EmployeeService(
    EmployeesDbContext dbContext,
    IMessagePublisher messagePublisher) : IEmployeeService
{
    public async Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken)
    {
        var employee = Employee.Create(
            Ulid.NewUlid(),
            firstName,
            lastName,
            email,
            supervisorId);

        if (await dbContext.Employees.AnyAsync(x
                => x.FirstName == firstName
                && x.LastName == lastName, cancellationToken))
        {
            throw new EmployeeAlreadyExistsException(firstName, lastName);
        }

        var @event = new EmployeeCreated(
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.SupervisorId);

        await dbContext.Employees.AddAsync(employee, cancellationToken);
        await messagePublisher.Send(@event, cancellationToken);
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
            throw new EmployeeNotFoundException(employeeId);
        }

        var doesSupervisorExist = await dbContext.Employees
            .AnyAsync(
                e => e.Id == supervisorId,
                cancellationToken);

        if (!doesSupervisorExist)
        {
            throw new SupervisorEmployeeNotFoundException(supervisorId);
        }

        employee.ChangeSupervisor(supervisorId);
        
        var @event = new SupervisorAssigned(
            employeeId,
            supervisorId);
        
        await messagePublisher.Send(@event, cancellationToken);
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
            .Where(e => e.SupervisorId == supervisorId)
            .ToListAsync(cancellationToken);
    }
}