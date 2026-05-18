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

        modelBuilder.Entity<Docent>(entity =>
        {
            entity.HasOne(d => d.IdentityUser)
                .WithMany()
                .HasForeignKey(d => d.IdentityUserId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Klas>()
            .HasOne(k => k.Docent)
            .WithMany(d => d.Klassen)
            .HasForeignKey(k => k.DocentIdentityUserId)
            .HasPrincipalKey(d => d.IdentityUserId);

        modelBuilder.Entity<Resultaat>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Resultaat>()
            .HasOne(r => r.Oefening)
            .WithMany()
            .HasForeignKey(r => r.OefeningId);

        modelBuilder.Entity<Leerling>()
            .HasOne(l => l.Klas)
            .WithMany(k => k.Leerlingen)
            .HasForeignKey(l => l.KlasId);

        modelBuilder.Entity<Leerling>()
            .HasOne(l => l.IdentityUser)
            .WithMany()
            .HasForeignKey(l => l.IdentityUserId);

        modelBuilder.Entity<Oefening>().HasData(
            new Oefening { Id = 1, Naam = "Treble Clef", Niveau = Niveau.Beginner },
            new Oefening { Id = 2, Naam = "Bass Clef", Niveau = Niveau.Beginner },
            new Oefening { Id = 3, Naam = "Beide Clefs", Niveau = Niveau.Beginner }
        );

    }
}