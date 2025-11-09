using chronos.employees.core.Domain;

namespace chronos.employees.core.Services;

public interface IEmployeeService
{
    Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        Ulid? supervisorId);
}

internal sealed class EmployeeService : IEmployeeService
{
    public Task<Employee> CreateAsync(string firstName, string lastName, Ulid? supervisorId)
    {
        throw new NotImplementedException();
    }
}