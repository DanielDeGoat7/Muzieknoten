using Microsoft.AspNetCore.Identity;

namespace Piano.Models;

public class Docent
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public string IdentityUserId { get; set; } = string.Empty;
    public virtual IdentityUser? IdentityUser { get; set; }
    public List<Klas> Klassen { get; set; } = new List<Klas>();

}