using System.ComponentModel.DataAnnotations;

namespace NotesContacts.Web.Models.ViewModels;

public sealed class NoteForm
{
    public Guid? Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = null!;

    [Required, DataType(DataType.MultilineText)]
    public string Text { get; set; } = null!;

    [Display(Name = "Tags (comma-separated)")]
    public string? TagsCsv { get; set; }
}
