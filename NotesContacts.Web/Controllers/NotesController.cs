using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesContacts.Web.Data;
using NotesContacts.Web.Models;
using NotesContacts.Web.Models.ViewModels;

namespace NotesContacts.Web.Controllers;

public sealed class NotesController : Controller
{
    private readonly AppDbContext _db;

    public NotesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var notes = await _db.Notes
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
        return View(notes);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note is null) return NotFound();
        return View(note);
    }

    public IActionResult Create()
    {
        return View(new NoteForm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NoteForm form)
    {
        if (!ModelState.IsValid) return View(form);

        var tags = ParseTags(form.TagsCsv);
        var entity = new Note
        {
            Title = form.Title,
            Text = form.Text,
            CreatedAt = DateTimeOffset.UtcNow,
            Tags = tags
        };
        _db.Notes.Add(entity);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = entity.Id });
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note is null) return NotFound();

        var form = new NoteForm
        {
            Id = note.Id,
            Title = note.Title,
            Text = note.Text,
            TagsCsv = note.Tags is { Length: > 0 } ? string.Join(", ", note.Tags) : null
        };
        return View(form);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NoteForm form)
    {
        if (!ModelState.IsValid) return View(form);

        var note = await _db.Notes.FindAsync(id);
        if (note is null) return NotFound();

        note.Title = form.Title;
        note.Text = form.Text;
        note.Tags = ParseTags(form.TagsCsv);

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note is null) return NotFound();
        return View(note);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note is null) return NotFound();
        _db.Notes.Remove(note);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private static string[] ParseTags(string? csv)
        => string.IsNullOrWhiteSpace(csv)
            ? Array.Empty<string>()
            : csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                 .Where(t => !string.IsNullOrWhiteSpace(t))
                 .Distinct(StringComparer.OrdinalIgnoreCase)
                 .ToArray();
}
