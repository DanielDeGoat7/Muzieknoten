using Microsoft.AspNetCore.Identity;

namespace Piano.Models;

public class Leerling
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public Niveau? Niveau { get; set; }
    public int? KlasId { get; set; }
    public string IdentityUserId { get; set; } = string.Empty;
    public virtual IdentityUser? IdentityUser { get; set; }
    public virtual Klas? Klas { get; set; }

}
