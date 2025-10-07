namespace PolyhydraGames.Api.Steam.Models
{
    public class ApiParameter
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool optional { get; set; }
        public string? description { get; set; }
    }
}