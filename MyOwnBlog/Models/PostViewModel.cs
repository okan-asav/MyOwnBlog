namespace MyBlog.Models;

public class PostViewModel
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid[]? TagIds { get; set; } = Array.Empty<Guid>();
    public required string Title { get; set; }
    public required string Content { get; set; }

}
