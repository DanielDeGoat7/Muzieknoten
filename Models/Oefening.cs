namespace Piano.Models;

public class Oefening
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public Niveau Niveau { get; set; }
}