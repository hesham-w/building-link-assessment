namespace Api.Features.Driver.Models;

public class DriverModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string AddressLine1 { get; set; } = null!;

    public string AddressLine2 { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}
