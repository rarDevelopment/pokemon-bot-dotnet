using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class TcgCardCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IPokemonTcgBusinessLayer _pokemonTcgBusinessLayer;
    private readonly IDiscordFormatter _discordFormatter;
    private readonly BotSettings _botSettings;
    private readonly ILogger<DiscordBot> _logger;

    public TcgCardCommand(IPokemonTcgBusinessLayer pokemonTcgBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    {
        _pokemonTcgBusinessLayer = pokemonTcgBusinessLayer;
        _discordFormatter = discordFormatter;
        _botSettings = botSettings;
        _logger = logger;
    }

    [SlashCommand("tcg-card", "Get a Pokémon Card by specifying its id.")]
    public async Task GetCard(
        [Summary("card_name", "The name of the card you want to see.")] string? cardName = null,
        [Summary("set_name", "The name of the set that contains the card(s) you're looking for.")] string? setName = null,
        [Summary("card_number", "The number of the card in the set you're looking for.")] string? cardNumber = null)
    {
        await DeferAsync();

        try
        {
            var card = await _pokemonTcgBusinessLayer.GetPokemonCard(cardName, setName, cardNumber);

            await FollowupAsync(embed: _discordFormatter.BuildRegularEmbed(
                card.Name,
                "",
                Context.User,
                imageUrl: card.ImageUrl));
        }
        catch (PokemonCardNotFoundException ex)
        {
            _logger.Log(LogLevel.Information, $"Card Not Found: {ex.Message}");
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbed("Card Not Found",
                "No Card was found with that criteria. Please try again.",
                Context.User, imageUrl: _botSettings.MissingnoImageUrl));
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Card Command Failed: {ex.Message}");
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbed("Error",
                "There was an unhandled error. Please try again.",
                Context.User, imageUrl: _botSettings.GhostUrl));
        }
    }
}