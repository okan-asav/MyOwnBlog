using Microsoft.AspNetCore.Identity;


namespace MyBlog.Data;

public class User : IdentityUser<Guid>
{
    public required string Name { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
}
