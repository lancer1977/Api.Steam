namespace PolyhydraGames.Api.Steam.Models
{
    public class ApiInterface
    {
        public string name { get; set; }
        public List<ApiMethod> methods { get; set; } = new();
    }
}