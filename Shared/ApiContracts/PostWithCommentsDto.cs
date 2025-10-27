namespace ApiContracts;

public class PostWithCommentsDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int UserId { get; set; }
    public required IEnumerable<CommentDto> Comments { get; set; }
}