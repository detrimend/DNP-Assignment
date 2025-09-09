using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    private readonly ManagePostsView managePostsView;
    private bool running;
    
    private SinglePostView singlePostView;

    public ListPostsView(IPostRepository postRepository,
        ManagePostsView managePostsView)
    {
        this.postRepository = postRepository;
        this.managePostsView = managePostsView;
    }

    public async Task ListPosts()
    {
        running = true;
        while (running)
        {
            Console.WriteLine("Listing posts... ('title' [ID])");
            foreach (var post in postRepository.GetMany())
            {
                Console.WriteLine($"* '{post.Title}' [{post.Id}]");
            }

            Console.WriteLine("Enter [ID] to select a post \n" +
                              "Enter 'exit' to return");
            string selection = Console.ReadLine();

            if (selection == "exit")
            {
                running = false;
                break;
            }

            Task<Post> selectedPost =
                postRepository.GetSingleAsync(int.Parse(selection));
            if (singlePostView is null)
            {
                singlePostView = new SinglePostView(selectedPost, this);
            }

            await singlePostView.ShowPost();
        }

        managePostsView.ShowOptions();
    }
}