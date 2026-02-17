using Microsoft.AspNetCore.Identity;

namespace FerHf.DAL.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
