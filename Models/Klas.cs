using Microsoft.AspNetCore.Identity;

namespace Piano.Models;

public class Klas
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public List<Leerling> Leerlingen { get; set; } = new List<Leerling>();
    public string DocentIdentityUserId { get; set; } = string.Empty;
    public virtual Docent? Docent { get; set; }
}