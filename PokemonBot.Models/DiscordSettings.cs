namespace PokemonBot.Models;

public class DiscordSettings(string botToken)
{
    public string BotToken { get; set; } = botToken;
}