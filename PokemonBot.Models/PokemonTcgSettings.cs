namespace PokemonBot.Models;

public class PokemonTcgSettings
{
    public PokemonTcgSettings(string apiKey)
    {
        ApiKey = apiKey;
    }

    public string ApiKey { get; set; }
}