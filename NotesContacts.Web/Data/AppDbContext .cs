using Microsoft.EntityFrameworkCore;
using NotesContacts.Web.Models;

namespace NotesContacts.Web.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>()
            .Property(n => n.Tags)
            .HasColumnType("text[]");

        modelBuilder.Entity<Note>().HasIndex(n => n.Title);
        modelBuilder.Entity<Contact>().HasIndex(c => c.Name);
        modelBuilder.Entity<Contact>().HasIndex(c => c.Mobile);
    }
}
