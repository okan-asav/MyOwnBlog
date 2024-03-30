
namespace MyBlog.Data;

public class Tag
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
}

