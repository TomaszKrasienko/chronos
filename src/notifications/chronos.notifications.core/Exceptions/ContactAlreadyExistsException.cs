using System.Net;
using chronos.shared.kernel.Exceptions;

namespace chronos.notifications.core.Exceptions;

public sealed class ContactAlreadyExistsException(Ulid employeeId)
    : ChronosException("contact_already_exists", [employeeId.ToString()])
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}
