using System.ComponentModel.DataAnnotations;

namespace NotesContacts.Web.Models;

public sealed class Note
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(200)]
    public string Title { get; set; } = null!;

    [Required, DataType(DataType.MultilineText)]
    public string Text { get; set; } = null!;

    [Display(Name = "Created At")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Display(Name = "Tags")]
    public string[] Tags { get; set; } = Array.Empty<string>();
}
