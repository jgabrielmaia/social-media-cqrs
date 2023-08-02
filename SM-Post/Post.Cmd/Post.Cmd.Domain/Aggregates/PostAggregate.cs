using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates;

public class PostAggregate: AggregateRoot
{
    private bool _active;
    private string _author;
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active
    {
        get => _active;
        set => _active = value; 
    }

    public PostAggregate() { }
    public PostAggregate(Guid id, string author, string message) 
    { 
        RaiseEvent(new PostCreatedEvent
        {
            Id = id,
            Author = author,
            Message = message,
            CreationDate = DateTime.Now,
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _author = @event.Author;
        _active = true;
    }

    public void EditMessage(string message)
    {
        if(!_active)
        {
            throw new InvalidOperationException("You cannot edit an inactive post");
        }

        if(string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException($"{nameof(message)} cannot be empty");
        }
        RaiseEvent(new MessageUpdatedEvent{
            Id = _id,
            Message = message,
        });
    }

    public void Apply(MessageUpdatedEvent @event)
    {
        _id = @event.Id;
    }

    public void LikePost()
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot like an inactive post");
        }

        RaiseEvent(new PostLikedEvent 
        {
            Id = _id,
        });
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void AddComment(string comment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot add a comment to an inactive post");
        }

        if(string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"{nameof(comment)} cannot be empty");
        }

        RaiseEvent(new CommentAddedEvent 
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CommentDate = DateTime.Now,
        });
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
    }

    public void EditComment(Guid commentId, string comment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit a comment of an inactive post");
        }

        if(!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to edit a comment made by another user");
        }

        RaiseEvent(new CommentUpdatedEvent
        {
            Id = _id,
            Comment = comment,
            CommentId = commentId,
            Username = username,
            EditedDate = DateTime.Now
        });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
    }

    public void RemoveComment(Guid commentId, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot remove a comment from an inactive post");
        }

        if(!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a comment made by another user");
        }

        RaiseEvent(new CommentRemovedEvent
        {
            Id = _id,
        });
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void DeletePost(string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot remove a comment from an inactive post");
        }

        if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to delete a post made by another user");
        }

        RaiseEvent(new PostRemovedEvent{
            Id = _id,
        });
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }
}