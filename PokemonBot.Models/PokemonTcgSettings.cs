namespace PokemonBot.Models;

public class PokemonTcgSettings
{
    public PokemonTcgSettings(string apiKey, int cardLimit)
    {
        CardLimit = cardLimit;
        ApiKey = apiKey;
    }

    public string ApiKey { get; set; }
    public int CardLimit { get; set; }
}