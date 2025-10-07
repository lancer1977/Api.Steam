namespace PolyhydraGames.Api.Steam.Models
{
    public class NewsItem
    {
        public string Gid { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Contents { get; set; }
        public string FeedLabel { get; set; }
        public long Date { get; set; }
    }
}