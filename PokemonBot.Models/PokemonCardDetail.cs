namespace PokemonBot.Models;

public record PokemonCardDetail(string Name, string ImageUrl)
{
    public string Name { get; set; } = Name;
    public string ImageUrl { get; set; } = ImageUrl;
}