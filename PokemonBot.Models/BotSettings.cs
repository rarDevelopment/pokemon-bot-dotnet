namespace PokemonBot.Models;

public record BotSettings(string MissingnoImageUrl, string GhostUrl, string HelpImage, int TotalPokemon)
{
    public string MissingnoImageUrl { get; set; } = MissingnoImageUrl;
    public string GhostUrl { get; set; } = GhostUrl;
    public string HelpImage { get; set; } = HelpImage;
    public int TotalPokemon { get; set; } = TotalPokemon;
}
