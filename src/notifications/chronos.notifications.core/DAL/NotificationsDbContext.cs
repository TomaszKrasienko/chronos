using chronos.notifications.core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.notifications.core.DAL;

internal sealed class NotificationsDbContext(
    DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<NotificationMessage> NotificationMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

        modelBuilder
            .Entity<Contact>()
            .ToCollection("contacts");

        modelBuilder
            .Entity<Contact>()
            .HasKey(x => x.Id);

        modelBuilder
            .Entity<Contact>()
            .Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<Contact>()
            .Property(x => x.Email)
            .IsRequired()
            .HasElementName("Email");

        modelBuilder
            .Entity<Contact>()
            .Property(x => x.Supervisor)
            .HasElementName("Supervisor")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<NotificationMessage>()
            .ToCollection("notification_messages");

        modelBuilder
            .Entity<NotificationMessage>()
            .HasKey(x => x.Id);

        modelBuilder
            .Entity<NotificationMessage>()
            .Property(x => x.Id)
            .HasElementName("_id")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<NotificationMessage>()
            .Property(x => x.EmployeeId)
            .IsRequired()
            .HasElementName("EmployeeId")
            .HasConversion(ulidConverter);

        modelBuilder
            .Entity<NotificationMessage>()
            .Property(x => x.Topic)
            .IsRequired()
            .HasElementName("Topic");

        modelBuilder
            .Entity<NotificationMessage>()
            .Property(x => x.Message)
            .IsRequired()
            .HasElementName("Message");

        modelBuilder
            .Entity<NotificationMessage>()
            .Property(x => x.CreatedAt)
            .IsRequired()
            .HasElementName("CreatedAt");

        modelBuilder
            .Entity<NotificationMessage>()
            .Property(x => x.ReadAt)
            .HasElementName("ReadAt");
    }
}
