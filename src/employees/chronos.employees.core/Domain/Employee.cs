namespace chronos.employees.core.Domain;

public sealed class Employee
{
    public Ulid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public Ulid? SupervisorId { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Employee()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Employee(
        Ulid id,
        string firstName,
        string lastName,
        Ulid? supervisorId)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        SupervisorId = supervisorId;
    }
}