using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class TcgCardCommand : InteractionModuleBase<SocketInteractionContext>
{
    private const string EmptyStringValue = "{{NONE}}";
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

    [SlashCommand("tcg-card", "Get a Pokémon Card with search criteria.")]
    public async Task GetCard(
        [Summary("card_name", "The name of the card you want to see.")] string? cardName = null,
        [Summary("set_name", "The name of the set that contains the card(s) you're looking for.")] string? setName = null,
        [Summary("card_number", "The number of the card in the set you're looking for.")] string? cardNumber = null)
    {
        await DeferAsync();

        try
        {
            var cards = await _pokemonTcgBusinessLayer.GetPokemonCards(GetStringOrNull(cardName), GetStringOrNull(setName), GetStringOrNull(cardNumber));

            var buttonBuilder = new ComponentBuilder();

            if (cards.Count > 1)
            {
                buttonBuilder.WithButton("Next", $"currentIndexNext:{0}_{GetStringOrEmptyValue(cardName)}_{GetStringOrEmptyValue(setName)}_{GetStringOrEmptyValue(cardNumber)}", emote: new Emoji("➡️"));
            }

            await FollowupAsync(embed: GetCardEmbed(cards, 0), components: buttonBuilder.Build());
        }
        catch (PokemonCardNotFoundException ex)
        {
            _logger.Log(LogLevel.Information, $"Card Not Found: {ex.Message}");
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbedWithUserFooter("Card Not Found",
                "No Card was found with that criteria. Please try again.",
                Context.User, imageUrl: _botSettings.MissingnoImageUrl));
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Card Command Failed: {ex.Message}");
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbedWithUserFooter("Error",
                "There was an unhandled error. Please try again.",
                Context.User, imageUrl: _botSettings.GhostUrl));
        }
    }

    private Embed GetCardEmbed(IReadOnlyList<PokemonCardDetail> cards, int index)
    {
        var card = cards[index];
        return _discordFormatter.BuildRegularEmbedWithUserFooter(
            card.Name,
            $"{index + 1}/{cards.Count}",
            Context.User,
            imageUrl: card.ImageUrl);
    }

    [ComponentInteraction("currentIndexNext:*_*_*_*")]
    public async Task NextButton(int currentIndex, string cardName, string setName, string cardNumber)
    {
        await DeferAsync();

        // TODO: figure out how to load this data in again (or paginate it without doing so if possible)
        var cards = await _pokemonTcgBusinessLayer.GetPokemonCards(GetStringOrNull(cardName), GetStringOrNull(setName), GetStringOrNull(cardNumber));

        var newIndex = currentIndex + 1;

        var buttonBuilder = new ComponentBuilder();

        if (currentIndex >= 0)
        {
            buttonBuilder.WithButton("Previous", $"currentIndexPrev:{newIndex}_{GetStringOrEmptyValue(cardName)}_{GetStringOrEmptyValue(setName)}_{GetStringOrEmptyValue(cardNumber)}", emote: new Emoji("⬅️"));
        }

        if (newIndex + 1 < cards.Count)
        {
            buttonBuilder.WithButton("Next", $"currentIndexNext:{newIndex}_{GetStringOrEmptyValue(cardName)}_{GetStringOrEmptyValue(setName)}_{GetStringOrEmptyValue(cardNumber)}", emote: new Emoji("➡️"));
        }

        await Context.Interaction.ModifyOriginalResponseAsync(properties =>
        {
            properties.Embed = GetCardEmbed(cards, newIndex);
            properties.Components = buttonBuilder.Build();
        });
    }

    [ComponentInteraction("currentIndexPrev:*_*_*_*")]
    public async Task PreviousButton(int currentIndex, string cardName, string setName, string cardNumber)
    {
        await DeferAsync();

        // TODO: figure out how to load this data in again (or paginate it without doing so if possible)
        var cards = await _pokemonTcgBusinessLayer.GetPokemonCards(GetStringOrNull(cardName), GetStringOrNull(setName), GetStringOrNull(cardNumber));

        var buttonBuilder = new ComponentBuilder();

        var newIndex = currentIndex - 1;

        if (newIndex - 1 >= 0)
        {
            buttonBuilder.WithButton("Previous", $"currentIndexPrev:{newIndex}_{GetStringOrEmptyValue(cardName)}_{GetStringOrEmptyValue(setName)}_{GetStringOrEmptyValue(cardNumber)}", emote: new Emoji("⬅️"));
        }

        if (currentIndex < cards.Count)
        {
            buttonBuilder.WithButton("Next", $"currentIndexNext:{newIndex}_{GetStringOrEmptyValue(cardName)}_{GetStringOrEmptyValue(setName)}_{GetStringOrEmptyValue(cardNumber)}", emote: new Emoji("➡️"));
        }

        await Context.Interaction.ModifyOriginalResponseAsync(properties =>
        {
            properties.Embed = GetCardEmbed(cards, newIndex);
            properties.Components = buttonBuilder.Build();
        });
    }

    public static string GetStringOrEmptyValue(string? s = null)
    {
        return s ?? EmptyStringValue;
    }

    public static string? GetStringOrNull(string s)
    {
        return s == EmptyStringValue ? null : s;
    }
}