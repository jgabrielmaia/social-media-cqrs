using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

    public void RegisterHandler<TCommand>(Func<TCommand, Task> handler) where TCommand : BaseCommand
    {
        if(_handlers.ContainsKey(typeof(TCommand)))
        {
            throw new IndexOutOfRangeException("Cannot register the same command handler twice.");
        }

        _handlers.Add(typeof(TCommand), command => handler((TCommand) command));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task> handler)){
            await handler(command);
        }
        else 
        {
            throw new ArgumentNullException(nameof(handler), "No handler was registered.");
        }
    }
}
