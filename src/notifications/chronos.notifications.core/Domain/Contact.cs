namespace chronos.notifications.core.Domain;

public sealed class Contact
{
    public Ulid Id { get; private set; }
    public string Email { get; private set; }
    public Ulid Supervisor { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Contact()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Contact(
        Ulid id,
        string email,
        Ulid supervisor)
    {
        Id = id;
        Email = email;
        Supervisor = supervisor;
    }

    public static Contact Create(
        Ulid id,
        string email,
        Ulid? supervisor)
    {
        //If supervisor is empty it should send notification to employee

        return new Contact(
            id,
            email,
            supervisor ?? id);
    }
    
    public void SetSupervisor(Ulid supervisor)
        => Supervisor = supervisor;
}