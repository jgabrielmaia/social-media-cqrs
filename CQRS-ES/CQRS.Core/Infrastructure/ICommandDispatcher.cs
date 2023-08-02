using CQRS.Core.Commands;

namespace CQRS.Core.Infrastructure;

public interface ICommandDispatcher 
{
    void RegisterHandler<TCommand>(Func<TCommand, Task> handler) where TCommand: BaseCommand;
    Task SendAsync(BaseCommand command);
}