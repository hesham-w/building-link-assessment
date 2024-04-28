namespace Api.Domain;

public class Driver : Entity
{
    public string Name { get; set; } = null!;

    public string AddressLine1 { get; set; } = null!;

    public string AddressLine2 { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}
