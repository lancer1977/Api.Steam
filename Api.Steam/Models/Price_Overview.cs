namespace PolyhydraGames.Api.Steam.Models;

public class Price_Overview
{
    public string currency { get; set; }
    public int initial { get; set; }
    public int final { get; set; }
    public int discount_percent { get; set; }
    public string initial_formatted { get; set; }
    public string final_formatted { get; set; }
}