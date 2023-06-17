using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class CardCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IPokemonTcgBusinessLayer _pokemonTcgBusinessLayer;
    private readonly IDiscordFormatter _discordFormatter;
    private readonly BotSettings _botSettings;
    private readonly ILogger<DiscordBot> _logger;

    public CardCommand(IPokemonTcgBusinessLayer pokemonTcgBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    {
        _pokemonTcgBusinessLayer = pokemonTcgBusinessLayer;
        _discordFormatter = discordFormatter;
        _botSettings = botSettings;
        _logger = logger;
    }

    [SlashCommand("card", "Get a Pokémon Card by specifying its id.")]
    public async Task GetCard(
        [Summary("card_id", "The id of the card you want to see.")] string cardId)
    {
        await DeferAsync();

        try
        {
            var card = await _pokemonTcgBusinessLayer.GetPokemonCard(cardId);

            await FollowupAsync(embed: _discordFormatter.BuildRegularEmbed(
                card.Name,
                "NOTE: This feature is in beta, expect it to change.",
                Context.User,
                imageUrl: card.ImageUrl));
        }
        catch (PokemonCardNotFoundException ex)
        {
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbed("Card Not Found",
                $"No Card was found with the identifier {ex.CardId}",
                Context.User, imageUrl: _botSettings.MissingnoImageUrl));
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Card Command Failed: {ex.Message}", ex);
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbed("Error",
                "There was an unhandled error. Please try again.",
                Context.User, imageUrl: _botSettings.GhostUrl));
        }
    }
}