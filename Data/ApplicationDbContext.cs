using Microsoft.EntityFrameworkCore;
using Piano.Models;

namespace Piano.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Leerling> Leerlingen { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Leerling>().HasData(
            new Leerling { Id = 1, Naam = "Anna Jansen", Niveau = Niveau.Beginner },
            new Leerling { Id = 2, Naam = "Bram de Vries", Niveau = Niveau.Gevorderd },
            new Leerling { Id = 3, Naam = "Sofia Bakker", Niveau = Niveau.Expert }
        );


    }
}