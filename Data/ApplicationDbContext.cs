using Microsoft.EntityFrameworkCore;
using Piano.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Piano.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Leerling> Leerlingen { get; set; }
    public DbSet<Docent> Docenten { get; set; }
    public DbSet<Klas> Klassen { get; set; }
    public DbSet<Oefening> Oefeningen { get; set; }
    public DbSet<Resultaat> Resultaten { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Klas>()
            .HasOne(k => k.Docent)
            .WithMany()
            .HasForeignKey(k => k.DocentId);

        modelBuilder.Entity<Resultaat>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Resultaat>()
            .HasOne(r => r.Oefening)
            .WithMany()
            .HasForeignKey(r => r.OefeningId);

        modelBuilder.Entity<Leerling>().HasData(
            new Leerling { Id = 1, Naam = "Anna Jansen", Niveau = Niveau.Beginner, KlasId = 1 },
            new Leerling { Id = 2, Naam = "Bram de Vries", Niveau = Niveau.Gevorderd, KlasId = 1 },
            new Leerling { Id = 3, Naam = "Sofia Bakker", Niveau = Niveau.Expert, KlasId = 2 }
        );

        modelBuilder.Entity<Docent>().HasData(
            new Docent { Id = 1, Naam = "Daniël Dedden" },
            new Docent { Id = 2, Naam = "Jan de Boer" }
        );

        modelBuilder.Entity<Klas>().HasData(
            new Klas { Id = 1, Naam = "PianoX1", DocentId = 1 },
            new Klas { Id = 2, Naam = "PianoX2", DocentId = 2 }
        );

        modelBuilder.Entity<Oefening>().HasData(
            new Oefening { Id = 1, Naam = "Oefening 1", Niveau = Niveau.Beginner },
            new Oefening { Id = 2, Naam = "Oefening 2", Niveau = Niveau.Gevorderd },
            new Oefening { Id = 3, Naam = "Oefening 3", Niveau = Niveau.Expert }
        );

    }
}