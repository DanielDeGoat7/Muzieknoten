using Microsoft.EntityFrameworkCore;
using Piano.Models;

namespace Piano.Data;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
    }
    public DbSet<Leerling> Leerlingen { get; set; }
}