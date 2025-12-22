namespace chronos.ui.Services;

public sealed class AppStateService
{
    private Models.AppContext _currentContext = Models.AppContext.Employee;
    private string? _currentEmployeeId;

    public event Action? OnChange;

    public Models.AppContext CurrentContext
    {
        get => _currentContext;
        set
        {
            if (_currentContext != value)
            {
                _currentContext = value;
                NotifyStateChanged();
            }
        }
    }

    public string? CurrentEmployeeId
    {
        get => _currentEmployeeId;
        set
        {
            if (_currentEmployeeId != value)
            {
                _currentEmployeeId = value;
                NotifyStateChanged();
            }
        }
    }

    public bool IsEmployee => CurrentContext == Models.AppContext.Employee;
    public bool IsSupervisor => CurrentContext == Models.AppContext.Supervisor;
    public bool IsAdmin => CurrentContext == Models.AppContext.Admin;

    public bool CanApproveTimeLogs => IsSupervisor || IsAdmin;
    public bool CanManageEmployees => IsAdmin;
    public bool CanViewAllReports => IsSupervisor || IsAdmin;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
