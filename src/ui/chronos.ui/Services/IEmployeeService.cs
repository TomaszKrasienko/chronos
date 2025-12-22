using chronos.ui.Models;

namespace chronos.ui.Services;

public interface IEmployeeService
{
    Task<IReadOnlyCollection<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(string id);
    Task<string> CreateAsync(CreateEmployeeRequest request);
    Task AssignSupervisorAsync(string employeeId, string supervisorId);
}
