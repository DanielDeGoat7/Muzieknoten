namespace Piano.Models;

public class Klas
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public List<Leerling> Leerlingen { get; set; } = new List<Leerling>();
    public int DocentId { get; set; }
    public Docent? Docent { get; set; }
}