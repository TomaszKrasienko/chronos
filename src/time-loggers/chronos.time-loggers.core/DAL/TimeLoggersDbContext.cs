using chronos.time_loggers.core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.time_loggers.core.DAL;

internal sealed class TimeLoggersDbContext(
    DbContextOptions<TimeLoggersDbContext> options) : DbContext(options)
{
    public DbSet<TimeLogger> TimeLoggers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

        var timeLoggerStatusConverter = new ValueConverter<TimeLoggerStatus, string>(
            v => v.Value,
            v => TimeLoggerStatus.FromValue(v));

        modelBuilder
            .Entity<TimeLogger>()
            .ToCollection("time_loggers");

        modelBuilder
            .Entity<TimeLogger>()
            .HasKey(x => x.Id);

        modelBuilder
            .Entity<TimeLogger>()
            .Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<TimeLogger>()
            .Property(x => x.TimeSpan)
            .IsRequired()
            .HasElementName("TimeSpan");

        modelBuilder
            .Entity<TimeLogger>()
            .Property(x => x.EmployeeId)
            .IsRequired()
            .HasElementName("EmployeeId")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<TimeLogger>()
            .Property(x => x.Topic)
            .IsRequired()
            .HasElementName("Topic");

        modelBuilder
            .Entity<TimeLogger>()
            .Property(x => x.Notes)
            .HasElementName("Notes");

        modelBuilder
            .Entity<TimeLogger>()
            .Property(x => x.Status)
            .IsRequired()
            .HasElementName("Status")
            .HasConversion(timeLoggerStatusConverter);
    }
}
