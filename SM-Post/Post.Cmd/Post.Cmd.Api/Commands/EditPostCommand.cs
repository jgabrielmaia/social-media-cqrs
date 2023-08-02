using CQRS.Core.Messages;

namespace Post.Cmd.Api.Commands;

public class EditPostCommand : BaseCommand 
{
    public string Message { get; set; }
}