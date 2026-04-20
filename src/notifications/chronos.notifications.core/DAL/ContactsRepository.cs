using chronos.notifications.core.Domain;
using chronos.shared.kernel.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace chronos.notifications.core.DAL;

internal sealed class ContactsRepository(NotificationsDbContext dbContext) : IContactsRepository
{
    /// <inheritdoc />
    public async Task<Contact?> GetByIdAsync(ContactId id, CancellationToken cancellationToken = default)
        => await dbContext.Contacts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task AddAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        await dbContext.Contacts.AddAsync(contact, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
