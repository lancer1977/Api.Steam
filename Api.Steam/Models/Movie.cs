namespace PolyhydraGames.Api.Steam.Models;

public class Movie
{
    public int id { get; set; }
    public string name { get; set; }
    public string thumbnail { get; set; }
    public Webm webm { get; set; }
    public Mp4 mp4 { get; set; }
    public bool highlight { get; set; }
}