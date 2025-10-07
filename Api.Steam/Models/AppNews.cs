namespace PolyhydraGames.Api.Steam.Models
{
    public class AppNews
    {
        public int AppId { get; set; }
        public List<NewsItem> NewsItems { get; set; }
    }
}