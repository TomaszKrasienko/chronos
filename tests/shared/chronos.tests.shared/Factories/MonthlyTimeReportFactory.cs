using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;
using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="MonthlyTimeReport"/> instances in tests.
/// </summary>
public static class MonthlyTimeReportFactory
{
    public static MonthlyTimeReport Create(
        EmployeeId? employeeId = null,
        ReportPeriod? period = null)
        => MonthlyTimeReport.Create(
            employeeId ?? EmployeeId.New(),
            period ?? ReportPeriodFactory.Create());

    public static MonthlyTimeReport CreateWithTimeLog(
        EmployeeId? employeeId = null,
        ReportPeriod? period = null,
        ContractId? contractId = null,
        LoggedTime? time = null,
        string topic = "Development",
        string? notes = null,
        EmployeeId? supervisorId = null,
        TimeProvider? timeProvider = null)
    {
        var report = Create(employeeId, period);
        report.AddTimeLog(
            contractId ?? ContractId.New(),
            time ?? LoggedTimeFactory.Create(),
            topic,
            notes,
            supervisorId ?? EmployeeId.New(),
            timeProvider ?? TimeProvider.System);
        return report;
    }
}
