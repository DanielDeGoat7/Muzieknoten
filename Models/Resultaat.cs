using Microsoft.AspNetCore.Identity;

namespace Piano.Models;

public class Resultaat
{
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime datetime { get; set; } = DateTime.Now;

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser? User { get; set; } = null!;

    public int OefeningId { get; set; }
    public virtual Oefening? Oefening { get; set; } = null!;
}