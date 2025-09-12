using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly ManagePostsView managePostsView;
    private bool running;

    private SinglePostView singlePostView;

    public ListPostsView(IPostRepository postRepository,
        ICommentRepository commentRepository, ManagePostsView managePostsView)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
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

            Console.WriteLine(
                "\n Please enter a number corresponding with your selection:");
            Console.WriteLine("(1) Select post \n" +
                              "(0) Go back");
            int? selection = int.Parse(Console.ReadLine());

            switch (selection)
            {
                case 1:
                    Console.WriteLine("Enter [ID] to select a post");
                    string postSelection = Console.ReadLine();
                    Task<Post> selectedPost =
                        postRepository.GetSingleAsync(int.Parse(postSelection));
                    if (selectedPost is null)
                    {
                        Console.WriteLine("Post not found");
                        break;
                    }

                    if (singlePostView is null)
                    {
                        singlePostView = new SinglePostView(commentRepository,
                            selectedPost, this);
                    }

                    await singlePostView.ShowPost();
                    break;

                default: running = false; break;
            }
        }

        managePostsView.ShowOptions();
    }
}