using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;
    private readonly CliApp cliApp;
    private bool running;

    private ListPostsView listPostsView;
    private CreatePostView createPostView;

    public ManagePostsView(IPostRepository postRepository, CliApp cliApp)
    {
        this.postRepository = postRepository;
        this.cliApp = cliApp;
    }

    public async Task ShowOptions()
    {
        running = true;
        while (running)
        {
            Console.WriteLine(
                "Please enter a number corresponding with your selection:");
            Console.WriteLine("(1) View posts \n" +
                              "(2) Create post \n" +
                              "(0) Go back");
            int? selection = int.Parse(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    if (listPostsView is null)
                    { 
                        listPostsView = new ListPostsView(postRepository, this);
                    }
                    await listPostsView.ListPosts();
                    break;
                case 2:
                    if (createPostView is null)
                    {
                        createPostView = new CreatePostView(postRepository, this);
                    }
                    await createPostView.CreatePost();
                    break;
                default:
                    running = false;
                    break;
            }
        }

        await cliApp.StartAsync();
    }
}