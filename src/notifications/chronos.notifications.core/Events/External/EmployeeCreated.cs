using chronos.notifications.core.DAL;
using chronos.notifications.core.Domain;
using chronos.shared.kernel.Exceptions;
using chronos.shared.kernel.Identifiers;
using Microsoft.Extensions.Logging;

namespace chronos.notifications.core.Events.External;

/// <summary>
/// Integration event raised when a new employee is created.
/// </summary>
/// <param name="Id">The employee identifier.</param>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Email">The email address.</param>
/// <param name="SupervisorId">The supervisor identifier, if any.</param>
public sealed record EmployeeCreated(
    Ulid Id,
    string FirstName,
    string LastName,
    string Email,
    Ulid? SupervisorId);

/// <summary>
/// Handler for <see cref="EmployeeCreated"/> integration event.
/// </summary>
public interface IEmployeeCreatedEventHandler
{
    /// <summary>
    /// Handles the employee created event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task HandleAsync(EmployeeCreated @event, CancellationToken cancellationToken = default);
}

internal sealed class EmployeeCreatedEventHandler(
    ILogger<EmployeeCreatedEventHandler> logger,
    IContactsRepository contactsRepository) : IEmployeeCreatedEventHandler
{
    /// <inheritdoc />
    public async Task HandleAsync(EmployeeCreated @event, CancellationToken cancellationToken = default)
    {
        var contactId = new ContactId(@event.Id);

        var contact = Contact.Create(contactId, @event.Email);
        await contactsRepository.AddAsync(contact, cancellationToken);

        logger.LogInformation(
            "Created contact for employee {EmployeeId} with email {Email}.",
            @event.Id,
            @event.Email);

        if (@event.SupervisorId.HasValue)
        {
            await AddSubordinateToSupervisorAsync(@event.Id, @event.SupervisorId.Value, cancellationToken);
        }
    }

    private async Task AddSubordinateToSupervisorAsync(
        Ulid employeeId,
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var supervisorContactId = new ContactId(supervisorId);
        var supervisorContact = await contactsRepository.GetByIdAsync(supervisorContactId, cancellationToken);

        if (supervisorContact is null)
        {
            throw new NotFoundException(nameof(Contact), [supervisorId.ToString()]);
        }

        supervisorContact.AddSubordinate(new EmployeeId(employeeId));
        await contactsRepository.UpdateAsync(supervisorContact, cancellationToken);

        logger.LogInformation(
            "Added employee {EmployeeId} as subordinate to supervisor {SupervisorId}.",
            employeeId,
            supervisorId);
    }
}
