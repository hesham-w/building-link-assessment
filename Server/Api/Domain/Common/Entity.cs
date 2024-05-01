using System.Collections.ObjectModel;

namespace Api.Domain;

public class Entity
{
    public int Id { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    private List<DomainEvent> _domainEvents;

    public ReadOnlyCollection<DomainEvent> _events => _domainEvents?.AsReadOnly();

    protected void AddEvent(DomainEvent @event)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(@event);
    }
}