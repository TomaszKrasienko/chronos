using chronos.ui.Models;

namespace chronos.ui.Services;

public interface ITimeReportService
{
    Task<List<TimeReport>> GetAllAsync();
    Task<TimeReport?> GetByEmployeeAndPeriodAsync(string employeeId, string period);
}
