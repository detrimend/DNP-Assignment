using CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;
    private readonly Task<Post> post;
    private readonly SinglePostView singlePostView;
    private bool running;

    private CreateCommentView createCommentView;
    private SingleCommentView singleCommentView;

    public ListCommentsView(ICommentRepository commentRepository,
        Task<Post> post, SinglePostView singlePostView)
    {
        this.commentRepository = commentRepository;
        this.post = post;
        this.singlePostView = singlePostView;
    }

    public async Task ListComments()
    {
        running = true;
        while (running)
        {
            Console.WriteLine(
                $"Listing comments for post '{post.Result.Title}' [{post.Result.Id}]");
            foreach (var comment in commentRepository.GetMany()
                         .Where(c => c.PostId == post.Result.Id))
            {
                Console.WriteLine(
                    $"* {comment.UserId}: {comment.Body} [{comment.Id}]"); // To display username later
            }

            Console.WriteLine(
                "\n Please enter a number corresponding with your selection:");
            Console.WriteLine("(1) Create new comment \n" +
                              "(2) Select comment \n" +
                              "(0) Go back");
            int? selection = int.Parse(Console.ReadLine());

            switch (selection)
            {
                case 1:
                    if (createCommentView is null)
                    {
                        createCommentView =
                            new CreateCommentView(commentRepository, this,
                                post);
                    }

                    await createCommentView.CreateComment();
                    break;
                case 2:
                    Console.WriteLine("Enter [ID] to select a comment");
                    string commentSelection = Console.ReadLine();
                    Task<Comment> selectedComment =
                        commentRepository.GetSingleAsync(
                            int.Parse(commentSelection));
                    if (selectedComment is null)
                    {
                        Console.WriteLine("Comment not found");
                        break;
                    }

                    if (singleCommentView is null)
                    {
                        singleCommentView =
                            new SingleCommentView(selectedComment, this);
                    }

                    await singleCommentView.ShowComment();
                    break;
                default: running = false; break;
            }
        }

        singlePostView.ShowPost();
    }
}