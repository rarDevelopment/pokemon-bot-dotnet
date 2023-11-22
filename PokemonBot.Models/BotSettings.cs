namespace PokemonBot.Models;

public class BotSettings(string missingnoImageUrl, string ghostUrl, string helpImage, int totalPokemon)
{
    public string MissingnoImageUrl { get; set; } = missingnoImageUrl;
    public string GhostUrl { get; set; } = ghostUrl;
    public string HelpImage { get; set; } = helpImage;
    public int TotalPokemon { get; set; } = totalPokemon;
}
