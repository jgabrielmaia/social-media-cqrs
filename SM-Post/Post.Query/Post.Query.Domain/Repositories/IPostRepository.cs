using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface IPostRepository
{
    Task CreateAsync(PostEntity post);
    Task UpdateAsync(PostEntity post);
    Task DeleteAsync(Guid postId);
    Task<List<PostEntity>> ListAllAsync(Guid postId);
    Task<List<PostEntity>> ListByAuthorAsync(Guid postId);
    Task<List<PostEntity>> ListWithLikesAsync(Guid postId);
    Task<List<PostEntity>> ListWithCommentsAsync(Guid postId);
}