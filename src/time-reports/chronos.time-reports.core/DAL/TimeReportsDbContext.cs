using chronos.time_reports.core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.time_reports.core.DAL;

internal sealed class TimeReportsDbContext(
    DbContextOptions<TimeReportsDbContext> options) : DbContext(options)
{
    public DbSet<MonthlyTimeReport> MonthlyTimeReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

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
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.EmployeeId)
            .IsRequired()
            .HasElementName("EmployeeId")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.Summary)
            .IsRequired()
            .HasElementName("Summary");

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.RejectedTimeLoggerIds)
            .IsRequired()
            .HasElementName("RejectedTimeLoggerIds");

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.AcceptedTimeLoggerIds)
            .IsRequired()
            .HasElementName("AcceptedTimeLoggerIds");

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.Month)
            .IsRequired()
            .HasElementName("Month");

        modelBuilder
            .Entity<MonthlyTimeReport>()
            .Property(x => x.Year)
            .IsRequired()
            .HasElementName("Year");
    }
}