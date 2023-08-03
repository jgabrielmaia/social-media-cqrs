using CQRS.Core.Domain;

namespace CQRS.Core.Handlers;

public interface IEventSourcingHandler<TAggregate>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<TAggregate> GetByIdAsync(Guid aggregateId);
}