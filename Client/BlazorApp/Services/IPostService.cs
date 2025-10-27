using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddAsync(CreatePostDto request);
    Task UpdateAsync(PostDto request);
    Task DeleteAsync(int id);
    Task<object> GetSingleAsync(int id, bool includeComments);
    IQueryable<PostDto> GetMany();
}