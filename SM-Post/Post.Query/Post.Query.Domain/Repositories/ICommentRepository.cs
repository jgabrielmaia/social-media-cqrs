using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ICommentRepository
{
    Task CreateAsync(CommentEntity post);
    Task UpdateAsync(CommentEntity post);
    Task GetByIdAsync(Guid postId);
    Task DeleteAsync(Guid postId);
}