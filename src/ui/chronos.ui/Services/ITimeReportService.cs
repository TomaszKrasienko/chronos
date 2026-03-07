using chronos.ui.Models;

namespace chronos.ui.Services;

public interface ITimeReportService
{
    Task<TimeReport?> GetByEmployeeIdAsync(string employeeId);
    Task<TimeReport?> GetByEmployeeAndPeriodAsync(string employeeId, string period);
    Task GenerateReportFileAsync(string employeeId);
}
