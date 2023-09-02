using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class TcgSetsCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IPokemonTcgBusinessLayer _pokemonTcgBusinessLayer;
    private readonly IDiscordFormatter _discordFormatter;
    private readonly BotSettings _botSettings;
    private readonly ILogger<DiscordBot> _logger;

    public TcgSetsCommand(IPokemonTcgBusinessLayer pokemonTcgBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    {
        _pokemonTcgBusinessLayer = pokemonTcgBusinessLayer;
        _discordFormatter = discordFormatter;
        _botSettings = botSettings;
        _logger = logger;
    }

    [SlashCommand("tcg-sets", "Get a list of Pokémon Card sets.")]
    public async Task GetSets()
    {
        await DeferAsync();

        try
        {
            var sets = await _pokemonTcgBusinessLayer.GetSets();
            var setsToDisplay = string.Join("\n", sets);

            await FollowupAsync(embed: _discordFormatter.BuildRegularEmbedWithUserFooter(
                "Sets",
                setsToDisplay,
                Context.User));
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Sets Command Failed: {ex.Message}");
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbedWithUserFooter("Error",
                "There was an unhandled error. Please try again.",
                Context.User, imageUrl: _botSettings.GhostUrl));
        }
    }
}