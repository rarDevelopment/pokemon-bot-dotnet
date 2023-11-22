using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class TypeCommand(IPokemonBusinessLayer pokemonBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("type", "Get a Type by specifying its name.")]
    public async Task Type(
        [Summary("type_name", "The name of the type that you're searching for.")] string typeName)
    {
        await DeferAsync();

        try
        {
            var type = await pokemonBusinessLayer.GetType(typeName);

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

            await FollowupAsync(embed: discordFormatter.BuildRegularEmbedWithUserFooter(
                type.Name.ToTitleCase(),
                "",
                Context.User,
                embedFieldBuilders));
        }
        catch (TypeNotFoundException ex)
        {
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Type Not Found",
                $"No Type was found with the identifier {ex.TypeName}",
                Context.User, imageUrl: botSettings.MissingnoImageUrl));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"Type Command Failed: {ex.Message}", ex);
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Error",
                $"There was an unhandled error. Please try again.",
                Context.User, imageUrl: botSettings.GhostUrl));
        }
    }
}