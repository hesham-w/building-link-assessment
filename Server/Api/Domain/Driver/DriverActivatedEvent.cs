
namespace Api.Domain;

public class DriverActivatedEvent : DomainEvent
{
    public int DriverId { get; set; }

    public DriverActivatedEvent(int driverId)
    {
        DriverId = driverId;
    }
}