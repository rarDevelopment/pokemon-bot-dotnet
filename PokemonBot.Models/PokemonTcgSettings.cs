namespace PokemonBot.Models;

public record PokemonTcgSettings(string ApiKey, int CardLimit)
{
    public string ApiKey { get; set; } = ApiKey;
    public int CardLimit { get; set; } = CardLimit;
}