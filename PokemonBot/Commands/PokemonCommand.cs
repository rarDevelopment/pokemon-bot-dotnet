﻿using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;

namespace PokemonBot.Commands;

public class PokemonCommand(IPokemonBusinessLayer pokemonBusinessLayer,
        IDiscordFormatter discordFormatter,
        BotSettings botSettings,
        ILogger<DiscordBot> logger)
    : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("pokemon", "Get a Pokémon by specifying their number, name, or neither to get a random Pokémon.")]
    public async Task Pokemon(
        [Summary("name_or_number", "The Pokémon name or ID that you're searching for.")] string? nameOrNumber = "")
    {
        await DeferAsync();

        try
        {
            if (string.IsNullOrEmpty(nameOrNumber))
            {
                nameOrNumber = new Random().Next(1, botSettings.TotalPokemon).ToString();
            }

            var pokemon = (await pokemonBusinessLayer.GetPokemon(nameOrNumber))!;
            var pokedexEntryVersion = pokemon.FlavorTextVersion != null
                ? $"({pokemon.FlavorTextVersion.CleanVersionName()})"
                : "";

            var embedFieldBuilders = new List<EmbedFieldBuilder>
            {
                new() { Name = "ID", Value = pokemon.Id.ToString("D4"), IsInline = false },
                new()
                {
                    Name = $"Type{(pokemon.Types.Count > 1 ? "s" : "")}",
                    Value = string.Join(", ", pokemon.Types.Select(t => t.ToTitleCase())),
                    IsInline = false
                },
                new() { Name = "Category", Value = pokemon.Genera, IsInline = true },
                new()
                {
                    Name = "Weak To",
                    Value = pokemon.Weaknesses.Any()
                        ? string.Join(", ", pokemon.Weaknesses.Select(w => $"{w.Key.ToTitleCase()} ({w.Value}x)"))
                        : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "Resistant To",
                    Value = pokemon.Resistances.Any()
                        ? string.Join(", ", pokemon.Resistances.Select(r => $"{r.Key.ToTitleCase()} ({r.Value}x)"))
                        : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "Immune To",
                    Value = pokemon.Immunities.Any()
                        ? string.Join(", ", pokemon.Immunities.Select(i => i.Key.ToTitleCase()))
                        : "None",
                    IsInline = false
                },
                new()
                {
                    Name = "Height",
                    Value = $"{pokemon.Height}m",
                    IsInline = false,
                },
                new()
                {
                    Name = "Weight",
                    Value = $"{pokemon.Weight}kg",
                    IsInline = false,
                },
                new()
                {
                    Name = "First Appearance",
                    Value = pokemon.Generation.CleanGenerationName(),
                    IsInline = false,
                },
                new()
                {
                    Name = $"Pokédex Entry {pokedexEntryVersion}",
                    Value = pokemon.FlavorText,
                    IsInline = false
                }
            };

            var embedFooterBuilder = new EmbedFooterBuilder().WithText(pokemon.Name.CleanGeneralName());

            if (!string.IsNullOrEmpty(pokemon.FrontSprite))
            {
                embedFooterBuilder.WithIconUrl(pokemon.FrontSprite);
            }

            await FollowupAsync(embed: discordFormatter.BuildRegularEmbed(
                pokemon.Name.CleanGeneralName(),
                "",
                embedFooterBuilder,
                embedFieldBuilders,
                imageUrl: pokemon.GetImageUrlToDisplay()));
        }
        catch (PokemonNotFoundException ex)
        {
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Pokémon Not Found",
                $"No Pokémon was found with the identifier {ex.Identifier}",
                Context.User, imageUrl: botSettings.MissingnoImageUrl));
        }
        catch (NoTypesFoundException ex)
        {
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Types Not Found",
                $"The types were not found for the Pokémon {ex.PokemonSearched.Name} with identifier {ex.Identifier}",
                Context.User, imageUrl: botSettings.MissingnoImageUrl));
        }
        catch (GenerationNotFoundException ex)
        {
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Generation Not Found",
                $"No generation was found with the identifier {ex.Identifier}",
                Context.User, imageUrl: botSettings.MissingnoImageUrl));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"Pokémon Command Failed: {ex.Message}", ex);
            await FollowupAsync(embed: discordFormatter.BuildErrorEmbedWithUserFooter("Error",
                "There was an unhandled error. Please try again.",
                Context.User, imageUrl: botSettings.GhostUrl));
        }
    }
}