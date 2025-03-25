namespace PokemonBot.Models;

public record DiscordSettings(string BotToken)
{
    public string BotToken { get; set; } = BotToken;
}