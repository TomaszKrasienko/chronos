using chronos.notifications.core.DAL;
using chronos.notifications.core.Domain;
using chronos.notifications.core.Events;
using chronos.notifications.core.Services.NotificationSenders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace chronos.notifications.core.Services;

public interface IContactsServices
{
    Task CreateAsync(
        Ulid id,
        string email,
        Ulid? supervisor,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(
        Ulid id,
        Ulid supervisor,
        CancellationToken cancellationToken = default);
}

internal sealed class ContactsService(
    ILogger<ContactsService> logger,
    NotificationsDbContext dbContext,
    INotificationSenderFactory notificationSender) : IContactsServices
{
    public async Task CreateAsync(
        Ulid id,
        string email,
        Ulid? supervisor,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.Contacts.AnyAsync(x => x.Id == id, cancellationToken))
        {
            logger.LogWarning("Employee with ID: {id} already exists", id);
            return;
        }
        
        var contact = Contact.Create(id, email, supervisor);
        await dbContext.Contacts.AddAsync(contact, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var sender = notificationSender.GetInstance<EmployeeCreated>();

        if (sender is not null)
        {
            await sender.SendAsync(
                new EmployeeCreated(id, email, supervisor),
                cancellationToken);
        }
    }

    public async Task UpdateAsync(Ulid id, Ulid supervisor, CancellationToken cancellationToken = default)
    {
        var contact = await dbContext
            .Contacts
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (contact is null)
        {
            return;
        }
        
        contact.SetSupervisor(supervisor);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}