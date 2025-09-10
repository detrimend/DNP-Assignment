using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly ManagePostsView managePostsView;

    public CreatePostView(IPostRepository postRepository,
        ManagePostsView managePostsView)
    {
        this.postRepository = postRepository;
        this.managePostsView = managePostsView;
    }

    // SinglePostView needs ListPostsView to go back to list after viewing a single post
    // For that reason, after creating a post, we go back to the manage menu
    // Rather than to a single view of the newly created post
    // to be continued
    public async Task CreatePost()
    {
        Console.WriteLine("Creating a new post...");
        Console.WriteLine("Enter your user ID:"); //temp until login implemented
        int userId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter a title:");
        string title = Console.ReadLine();
        Console.WriteLine("Enter the body:");
        string body = Console.ReadLine();
        
        await postRepository.AddAsync(new Entities.Post
        {
            Title = title,
            Body = body,
            UserId = userId
        });
        Console.WriteLine(
            $"Post created successfully with ID: {postRepository.GetMany().Last().Id}");
        await managePostsView.ShowOptions();
    }
}