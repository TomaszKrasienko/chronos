using chronos.shared.kernel.Identifiers;
using chronos.shared.messaging;

namespace chronos.contracts.core.Events;

public sealed record TimeLogAutomaticallyRejected(
    TimeLogId TimeLogId) : IMessage;