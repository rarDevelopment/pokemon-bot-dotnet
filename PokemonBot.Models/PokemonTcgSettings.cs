namespace PokemonBot.Models;

public class PokemonTcgSettings(string apiKey, int cardLimit)
{
    public string ApiKey { get; set; } = apiKey;
    public int CardLimit { get; set; } = cardLimit;
}