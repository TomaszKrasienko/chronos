using chronos.employees.core.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace chronos.employees.core.Services;

internal sealed class CachedEmployeeService(
    ILogger<CachedEmployeeService> logger,
    IEmployeeService employeeService,
    IMemoryCache memoryCache) : IEmployeeService, IEmployeeCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public async Task<Employee> CreateAsync(
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken)
    {
        var employee = await employeeService.CreateAsync(
            firstName,
            lastName,
            email,
            supervisorId,
            cancellationToken);

        var cacheKey = GetCacheKey(employee.Id);
        memoryCache.Set(cacheKey, employee, CacheDuration);

        return employee;
    }

    public Task AssignSupervisorAsync(
        Ulid employeeId,
        Ulid supervisorId,
        CancellationToken cancellationToken)
        => employeeService.AssignSupervisorAsync(
            employeeId,
            supervisorId,
            cancellationToken);
    

    public async Task<Employee?> GetByIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting employee with ID {employeeId} from cache", employeeId);
        var cacheKey = GetCacheKey(employeeId);

        if (memoryCache.TryGetValue<Employee>(cacheKey, out var cachedEmployee))
        {
            return cachedEmployee;
        }

        var employee = await employeeService.GetByIdAsync(
            employeeId,
            cancellationToken);

        if (employee is not null)
        {
            memoryCache.Set(cacheKey, employee, CacheDuration);
        }

        return employee;
    }

    public Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken)
        => employeeService.GetAllAsync(cancellationToken);

    public Task<IReadOnlyCollection<Employee>> GetSubordinatesAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken)
        => employeeService.GetSubordinatesAsync(
            supervisorId,
            cancellationToken);

    Task IEmployeeCache.CreateAsync(
        Ulid id,
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken)
    {
        var employee = Employee.Create(id, firstName, lastName, email, supervisorId);
        var cacheKey = GetCacheKey(employee.Id);
        memoryCache.Set(cacheKey, employee, CacheDuration);
        return Task.CompletedTask;
    }

    private static string GetCacheKey(Ulid employeeId) => $"employee:{employeeId}";
}