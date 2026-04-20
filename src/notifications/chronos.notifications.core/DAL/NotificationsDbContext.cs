using chronos.notifications.core.Domain;
using chronos.shared.kernel.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;

namespace chronos.notifications.core.DAL;

internal sealed class NotificationsDbContext(
    DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new ValueConverter<Ulid, string>(
            v => v.ToString(),
            v => Ulid.Parse(v));

        var contactIdConverter = new ValueConverter<ContactId, string>(
            v => v.Value.ToString(),
            v => new ContactId(Ulid.Parse(v)));

        var employeeIdConverter = new ValueConverter<EmployeeId, string>(
            v => v.Value.ToString(),
            v => new EmployeeId(Ulid.Parse(v)));

        var notificationMessageIdConverter = new ValueConverter<NotificationMessageId, string>(
            v => v.Value.ToString(),
            v => new NotificationMessageId(Ulid.Parse(v)));

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
            .HasConversion(contactIdConverter);

        modelBuilder
            .Entity<Contact>()
            .Property(x => x.Email)
            .IsRequired()
            .HasElementName("Email");

        modelBuilder
            .Entity<Contact>()
            .PrimitiveCollection(x => x.Subordinates)
            .ElementType()
            .HasConversion(employeeIdConverter);

        modelBuilder
            .Entity<Contact>()
            .OwnsMany(x => x.Messages, message =>
            {
                message.HasElementName("Messages");

                message
                    .Property(x => x.Id)
                    .HasElementName("_id")
                    .HasConversion(notificationMessageIdConverter);

                message
                    .Property(x => x.Topic)
                    .IsRequired()
                    .HasElementName("Topic");

                message
                    .Property(x => x.Message)
                    .IsRequired()
                    .HasElementName("Message");

                message
                    .Property(x => x.CreatedAt)
                    .IsRequired()
                    .HasElementName("CreatedAt");

                message
                    .Property(x => x.ReadAt)
                    .HasElementName("ReadAt");
            });

        modelBuilder
            .Entity<Contact>()
            .Ignore(x => x.DomainEvents);
    }
}
