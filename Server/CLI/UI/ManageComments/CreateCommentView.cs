using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly ListCommentsView listCommentsView;
    private Task<Post> post;

    public CreateCommentView(ICommentRepository commentRepository,
        ListCommentsView listCommentsView, Task<Post> post)
    {
        this.commentRepository = commentRepository;
        this.listCommentsView = listCommentsView;
        this.post = post;
    }

    public async Task CreateComment()
    {
        Console.WriteLine(
            $"Creating a new comment for post [{post.Result.Id}]");
        Console.WriteLine("Enter your user ID:"); //temp until login implemented
        int userId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter the body:");
        string body = Console.ReadLine();

        await commentRepository.AddAsync(new Entities.Comment
        {
            Body = body,
            UserId = userId,
            PostId = post.Result.Id
        });
        Console.WriteLine(
            $"Comment created successfully with ID: {commentRepository.GetMany().Last().Id} \n");
        await listCommentsView.ListComments();
    }
}