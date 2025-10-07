namespace PolyhydraGames.Api.Steam.Models
{
    public class ApiMethod
    {
        public string name { get; set; }
        public int version { get; set; }
        public string httpmethod { get; set; }
        public string? description { get; set; }
        public List<ApiParameter>? parameters { get; set; }
    }
}