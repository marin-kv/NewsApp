using Microsoft.AspNetCore.Identity;

namespace NewsApp.Models;

public partial class User : IdentityUser<int>
{
    public virtual Author? Author { get; set; } = null!;
}
