namespace PokemonBot.Models;

public class BotSettings
{
    public BotSettings(string missingnoImageUrl, string ghostUrl, string helpImage, int totalPokemon)
    {
        MissingnoImageUrl = missingnoImageUrl;
        GhostUrl = ghostUrl;
        HelpImage = helpImage;
        TotalPokemon = totalPokemon;
    }

    public string MissingnoImageUrl { get; set; }
    public string GhostUrl { get; set; }
    public string HelpImage { get; set; }
    public int TotalPokemon { get; set; }
}
