using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class TypeCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IPokemonBusinessLayer _pokemonBusinessLayer;
    private readonly IDiscordFormatter _discordFormatter;
    private readonly BotSettings _botSettings;
    private readonly ILogger<DiscordBot> _logger;

    public TypeCommand(IPokemonBusinessLayer pokemonBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    {
        _pokemonBusinessLayer = pokemonBusinessLayer;
        _discordFormatter = discordFormatter;
        _botSettings = botSettings;
        _logger = logger;
    }

    [SlashCommand("type", "Get a Type by specifying its name.")]
    public async Task Type(
        [Summary("type_name", "The name of the type that you're searching for.")] string typeName)
    {
        await DeferAsync();

        try
        {
            var type = (await _pokemonBusinessLayer.GetType(typeName));

            var embedFieldBuilders = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "2x Damage From",
                    Value = type.DoubleDamageFrom.Any() ? string.Join(", ", type.DoubleDamageFrom.Select(t => t.ToTitleCase())) : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "2x Damage To",
                    Value = type.DoubleDamageTo.Any() ? string.Join(", ", type.DoubleDamageTo.Select(t => t.ToTitleCase())) : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "1/2x Damage From",
                    Value = type.HalfDamageFrom.Any() ? string.Join(", ", type.HalfDamageFrom.Select(t => t.ToTitleCase())) : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "1/2x Damage To",
                    Value = type.HalfDamageTo.Any() ? string.Join(", ", type.HalfDamageTo.Select(t => t.ToTitleCase())) : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "0x Damage From",
                    Value = type.NoDamageFrom.Any() ? string.Join(", ", type.NoDamageFrom.Select(t => t.ToTitleCase())) : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "0x Damage To",
                    Value = type.NoDamageTo.Any() ? string.Join(", ", type.NoDamageTo.Select(t => t.ToTitleCase())) : "None",
                    IsInline = false
                },
            };

            await FollowupAsync(embed: _discordFormatter.BuildRegularEmbed(
                type.Name.ToTitleCase(),
                "",
                Context.User,
                embedFieldBuilders));
        }
        catch (TypeNotFoundException ex)
        {
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbed("Type Not Found",
                $"No Type was found with the identifier {ex.TypeName}",
                Context.User, imageUrl: _botSettings.MissingnoImageUrl));
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Type Command Failed: {ex.Message}", ex);
            await FollowupAsync(embed: _discordFormatter.BuildErrorEmbed("Error",
                $"There was an unhandled error. Please try again.",
                Context.User, imageUrl: _botSettings.GhostUrl));
        }
    }
}