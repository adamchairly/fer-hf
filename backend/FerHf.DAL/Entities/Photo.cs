using System.ComponentModel.DataAnnotations;

namespace FerHf.DAL.Entities;

public class Photo
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
