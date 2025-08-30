using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesContacts.Web.Data;
using NotesContacts.Web.Models;

namespace NotesContacts.Web.Controllers;

public sealed class ContactsController : Controller
{
    private readonly AppDbContext _db;

    public ContactsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var items = await _db.Contacts
            .OrderBy(c => c.Name)
            .ToListAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();
        return View(contact);
    }

    public IActionResult Create() => View(new Contact());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contact model)
    {
        if (!ModelState.IsValid) return View(model);

        _db.Contacts.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();
        return View(contact);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Contact model)
    {
        if (!ModelState.IsValid) return View(model);

        var entity = await _db.Contacts.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Name = model.Name;
        entity.Mobile = model.Mobile;
        entity.AltPhone = model.AltPhone;
        entity.Email = model.Email;
        entity.Description = model.Description;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();
        return View(contact);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();
        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
