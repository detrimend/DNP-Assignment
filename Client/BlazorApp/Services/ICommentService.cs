using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddAsync(CreateCommentDto request);
    Task UpdateAsync(CommentDto request);
    Task DeleteAsync(int id);
    Task<CommentDto> GetSingleAsync(int id);
    IQueryable<CommentDto> GetMany();
}