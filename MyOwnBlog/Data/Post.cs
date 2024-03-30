
namespace MyBlog.Data;

public class Post
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public User? User { get; set; }
    public Category? Category { get; set; }
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
}