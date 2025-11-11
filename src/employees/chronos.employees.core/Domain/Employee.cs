namespace chronos.employees.core.Domain;

public sealed class Employee
{
    public Ulid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Ulid? SupervisorId { get; private set; }

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
    
    public static Employee Create(
        Ulid id,
        string firstName,
        string lastName,
        Ulid? supervisorId) 
        => new(
            id,
            firstName,
            lastName,
            supervisorId);

    public void ChangeSupervisor(Ulid supervisorId)
    {
        SupervisorId = supervisorId;
    }
}