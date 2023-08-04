using Microsoft.AspNetCore.Mvc;
using CQRS.Core.Infrastructure;
using Post.Query.Api.Queries;
using Post.Query.Api.DTOs;
using Post.Common.DTOs;
using Post.Query.Domain.Entities;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController: ControllerBase
{
    private readonly ILogger<PostLookupController> _logger;
    private readonly IQueryDispatcher<PostEntity> _queryDispatcher; 
    private const string SAFE_ERROR_MESSAGE = "Error while processing request to retrive posts";

    public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _logger = logger;        
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPostsAsync()
    {
        try 
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());

            if (posts == null || !posts.Any())
            {
                return NoContent();
            }

            var count = posts.Count;
            return Ok(new PostLookupResponse{
                Posts = posts,
                Message = $"Successfully fetched {count} post{(count > 1 ? "s" : "")}",
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE,
            });
        }
    }

    [HttpGet("byId/{id}")]
    public async Task<ActionResult> GetByPostIdAsync(Guid id)
    {
        try 
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsByIdQuery{
                Id = id
            });

            if (posts == null || !posts.Any())
            {
                return NoContent();
            }

            return Ok(new PostLookupResponse{
                Posts = posts,
                Message = $"Successfully fetched post",
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE + " by id",
            });
        }
    }

    [HttpGet("byAuthor/{author}")]
    public async Task<ActionResult> GetByPostAuthorAsync(string author)
    {
        try 
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery{
                Author = author
            });

            if (posts == null || !posts.Any())
            {
                return NoContent();
            }

            var count = posts.Count;

            return Ok(new PostLookupResponse{
                Posts = posts,
                Message = $"Successfully fetched {count} post{(count > 1 ? "s" : "")} by author",
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE + " by author",
            });
        }
    }

    [HttpGet("withComments")]
    public async Task<ActionResult> GetPostsWithCommentsAsync()
    {
        try 
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());

            if (posts == null || !posts.Any())
            {
                return NoContent();
            }

            var count = posts.Count;

            return Ok(new PostLookupResponse{
                Posts = posts,
                Message = $"Successfully fetched {count} post{(count > 1 ? "s" : "")} with comments",
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE + " with comments",
            });
        }
    }

    [HttpGet("withLikes/{likes}")]
    public async Task<ActionResult> GetPostsWithLikesAsync(int likes)
    {
        try 
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery{
                NumberOfLikes = likes,
            });

            if (posts == null || !posts.Any())
            {
                return NoContent();
            }

            var count = posts.Count;

            return Ok(new PostLookupResponse{
                Posts = posts,
                Message = $"Successfully fetched {count} post{(count > 1 ? "s" : "")} with likes",
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE + " with likes",
            });
        }
    }
}