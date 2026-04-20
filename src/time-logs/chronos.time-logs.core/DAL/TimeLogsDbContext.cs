using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;
using chronos.time_logs.core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.time_logs.core.DAL;

internal sealed class TimeLogsDbContext : DbContext
{
    public DbSet<MonthlyTimeReport> MonthlyTimeReports { get; set; }

    public TimeLogsDbContext(DbContextOptions<TimeLogsDbContext> options) : base(options)
    {
        // Disable transactions for standalone MongoDB (no replica set)
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var employeeIdConverter = new ValueConverter<EmployeeId, string>(
            v => v.Value.ToString(),
            v => new EmployeeId(Ulid.Parse(v)));

        var contractIdConverter = new ValueConverter<ContractId, string>(
            v => v.Value.ToString(),
            v => new ContractId(Ulid.Parse(v)));

        var monthlyTimeReportIdConverter = new ValueConverter<MonthlyTimeReportId, string>(
            v => v.Value.ToString(),
            v => new MonthlyTimeReportId(Ulid.Parse(v)));

        var timeLogIdConverter = new ValueConverter<TimeLogId, string>(
            v => v.Value.ToString(),
            v => new TimeLogId(Ulid.Parse(v)));

        var loggedTimeConverter = new ValueConverter<LoggedTime, long>(
            v => v.Value.Ticks,
            v => LoggedTime.Create(TimeSpan.FromTicks(v)));

        // MonthlyTimeReport configuration
        modelBuilder
            .Entity<MonthlyTimeReport>()
            .ToCollection("monthly_time_reports");

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .HasKey(x => x.Id);

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion(monthlyTimeReportIdConverter);

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.EmployeeId)
            .IsRequired()
            .HasElementName("EmployeeId")
            .HasConversion(employeeIdConverter);

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .OwnsOne(x => x.Period, period =>
            {
                period.Property(p => p.Month).HasElementName("Month");
                period.Property(p => p.Year).HasElementName("Year");
            });

        // PendingTimeLogs - embedded collection
        modelBuilder
            .Entity<MonthlyTimeReport>()
            .OwnsMany(x => x.PendingTimeLogs, pending =>
            {
                pending.HasElementName("PendingTimeLogs");
                ConfigureTimeLogBase(
                    pending,
                    timeLogIdConverter,
                    employeeIdConverter,
                    contractIdConverter,
                    loggedTimeConverter);

                pending
                    .Property(x => x.SupervisorId)
                    .HasElementName("SupervisorId")
                    .HasConversion(employeeIdConverter);
            });

        // AcceptedTimeLogs - embedded collection
        modelBuilder
            .Entity<MonthlyTimeReport>()
            .OwnsMany(x => x.AcceptedTimeLogs, accepted =>
            {
                accepted.HasElementName("AcceptedTimeLogs");
                ConfigureTimeLogBase(
                    accepted,
                    timeLogIdConverter,
                    employeeIdConverter,
                    contractIdConverter,
                    loggedTimeConverter);

                accepted
                    .Property(x => x.AcceptedBy)
                    .HasElementName("AcceptedBy");

                accepted
                    .Property(x => x.AcceptedAt)
                    .HasElementName("AcceptedAt");
            });

        // RejectedTimeLogs - embedded collection
        modelBuilder
            .Entity<MonthlyTimeReport>()
            .OwnsMany(x => x.RejectedTimeLogs, rejected =>
            {
                rejected.HasElementName("RejectedTimeLogs");
                ConfigureTimeLogBase(
                    rejected,
                    timeLogIdConverter,
                    employeeIdConverter,
                    contractIdConverter,
                    loggedTimeConverter);

                rejected
                    .Property(x => x.Reason)
                    .HasElementName("Reason");

                rejected
                    .Property(x => x.RejectedBy)
                    .HasElementName("RejectedBy");

                rejected
                    .Property(x => x.RejectedAt)
                    .HasElementName("RejectedAt");
            });

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Ignore(x => x.TimeLogs);
    }

    private static void ConfigureTimeLogBase<TTimeLog>(
        OwnedNavigationBuilder<MonthlyTimeReport, TTimeLog> builder,
        ValueConverter<TimeLogId, string> timeLogIdConverter,
        ValueConverter<EmployeeId, string> employeeIdConverter,
        ValueConverter<ContractId, string> contractIdConverter,
        ValueConverter<LoggedTime, long> loggedTimeConverter)
        where TTimeLog : TimeLog
    {
        builder
            .Property(x => x.Id)
            .HasElementName("Id")
            .HasConversion(timeLogIdConverter);

        builder
            .Property(x => x.EmployeeId)
            .HasElementName("EmployeeId")
            .HasConversion(employeeIdConverter);

        builder
            .Property(x => x.ContractId)
            .HasElementName("ContractId")
            .HasConversion(contractIdConverter);

        builder
            .Property(x => x.Time)
            .HasElementName("Time")
            .HasConversion(loggedTimeConverter);

        builder
            .Property(x => x.Topic)
            .HasElementName("Topic");

        builder
            .Property(x => x.Notes)
            .HasElementName("Notes");

        builder
            .Property(x => x.CreatedAt)
            .HasElementName("CreatedAt");
    }
}
