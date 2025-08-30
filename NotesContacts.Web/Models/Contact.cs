using System.ComponentModel.DataAnnotations;

namespace NotesContacts.Web.Models;

public sealed class Contact
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(200)]
    public string Name { get; set; } = null!;

    [Required, StringLength(30)]
    [Display(Name = "Mobile Phone")]
    public string Mobile { get; set; } = null!;

    [StringLength(30)]
    [Display(Name = "Alternative Phone")]
    public string? AltPhone { get; set; }

    [EmailAddress, StringLength(254)]
    public string? Email { get; set; }

    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Short Description")]
    public string? Description { get; set; }
}
