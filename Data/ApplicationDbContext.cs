using Microsoft.EntityFrameworkCore;
using Piano.Models;

namespace Piano.Data;

public class ApplicationDbContext : DbContext
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

        modelBuilder.Entity<Resultaat>().HasData(
            new Resultaat { Id = 1, Score = 85, datetime = new DateTime(2026, 4, 14), LeerlingId = 1, OefeningId = 1 },
            new Resultaat { Id = 2, Score = 92, datetime = new DateTime(2026, 4, 14), LeerlingId = 2, OefeningId = 2 },
            new Resultaat { Id = 3, Score = 98, datetime = new DateTime(2026, 4, 14), LeerlingId = 3, OefeningId = 3 }
        );

    }
}