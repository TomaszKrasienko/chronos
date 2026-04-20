using chronos.notifications.core.Domain;
using chronos.shared.kernel.Identifiers;

namespace chronos.notifications.core.DAL;

/// <summary>
/// Repository for managing contacts.
/// </summary>
public interface IContactsRepository
{
    /// <summary>
    /// Gets a contact by identifier.
    /// </summary>
    /// <param name="id">The contact identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The contact if found, otherwise null.</returns>
    Task<Contact?> GetByIdAsync(ContactId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new contact.
    /// </summary>
    /// <param name="contact">The contact to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(Contact contact, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="contact">The contact to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default);
}
