using chronos.ui.Models;

namespace chronos.ui.Services;

public interface ITimeLogService
{
    Task<List<TimeLog>> GetAllAsync();
    Task<List<TimeLog>> GetByEmployeeIdAsync(string employeeId);
    Task<List<TimeLog>> GetMyTimeLogsAsync(string employeeId);
    Task<List<EmployeeTimeLogs>> GetSubordinatesTimeLogsAsync(string supervisorId);
    Task<string> CreateAsync(CreateTimeLogRequest request, string employeeId);
    Task ApproveAsync(string timeLogId);
    Task AcceptAsync(string timeLogId, string supervisorId);
    Task RejectAsync(string timeLogId, string supervisorId);
}
