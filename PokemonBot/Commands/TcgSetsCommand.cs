using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class TcgSetsCommand(IPokemonTcgBusinessLayer pokemonTcgBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("tcg-sets", "Get a list of Pokémon Card sets.")]
    public async Task GetSets()
    {
        await DeferAsync();

        try
        {
            var sets = await pokemonTcgBusinessLayer.GetSets();
            var setsToDisplay = string.Join("\n", sets);

            await FollowupAsync(embed: discordFormatter.BuildRegularEmbedWithUserFooter(
                "Sets",
                setsToDisplay,
                Context.User));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"Sets Command Failed: {ex.Message}");
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Error",
                "There was an unhandled error. Please try again.",
                Context.User, imageUrl: botSettings.GhostUrl));
        }
    }
}