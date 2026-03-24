using chronos.shared.kernel.Identifiers;
using chronos.time_logs.core.Domain;
using chronos.time_logs.core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.time_logs.core.DAL;

internal sealed class TimeLogsDbContext(
    DbContextOptions<TimeLogsDbContext> options) : DbContext(options)
{
    public DbSet<MonthlyTimeReport> MonthlyTimeReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

        var monthlyTimeReportIdConverter = new ValueConverter<MonthlyTimeReportId, string>(
            v => v.Value.ToString(),
            v => new MonthlyTimeReportId(Ulid.Parse(v)));

        var timeLogIdConverter = new ValueConverter<TimeLogId, string>(
            v => v.Value.ToString(),
            v => new TimeLogId(Ulid.Parse(v)));

        var loggedHoursConverter = new ValueConverter<LoggedHours, long>(
            v => v.Value.Ticks,
            v => LoggedHours.Create(TimeSpan.FromTicks(v)));

        var reportPeriodMonthConverter = new ValueConverter<ReportPeriod, int>(
            v => v.Month + v.Year * 100,
            v => ReportPeriod.Create(v % 100, v / 100));

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
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .OwnsOne(x => x.Period, period =>
            {
                period.Property(p => p.Month).HasElementName("Month");
                period.Property(p => p.Year).HasElementName("Year");
            });

        // TimeLog hierarchy configuration with discriminator
        modelBuilder
            .Entity<MonthlyTimeReport>()
            .OwnsMany(x => x.TimeLogs, timeLog =>
            {
                timeLog.HasKey(t => t.Id);

                timeLog.Property(t => t.Id)
                    .HasElementName("_id")
                    .HasConversion(timeLogIdConverter);

                timeLog.Property(t => t.EmployeeId)
                    .IsRequired()
                    .HasElementName("EmployeeId")
                    .HasConversion(ulidConverter);

                timeLog.Property(t => t.ContractId)
                    .IsRequired()
                    .HasElementName("ContractId")
                    .HasConversion(ulidConverter);

                timeLog.Property(t => t.Hours)
                    .IsRequired()
                    .HasElementName("Hours")
                    .HasConversion(loggedHoursConverter);

                timeLog.Property(t => t.Topic)
                    .IsRequired()
                    .HasElementName("Topic");

                timeLog.Property(t => t.Notes)
                    .HasElementName("Notes");

                timeLog.Property(t => t.CreatedAt)
                    .IsRequired()
                    .HasElementName("CreatedAt");
            });
    }
}
