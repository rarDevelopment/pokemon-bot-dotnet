namespace PokemonBot.Models;

public record VersionSettings(string VersionNumber)
{
    public string VersionNumber { get; set; } = VersionNumber;
}