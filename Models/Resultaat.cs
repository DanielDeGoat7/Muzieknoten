namespace Piano.Models;

public class Resultaat
{
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime datetime { get; set; } = DateTime.Now;
    public int LeerlingId { get; set; }
    public int OefeningId { get; set; }
}