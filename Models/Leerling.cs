namespace Piano.Models
{
    public class Leerling(int id, string naam, string? niveau)
    {
        public int Id { get; set; } = id;
        public string Naam { get; set; } = naam;
        public string? Niveau { get; set; } = niveau;
    }
}