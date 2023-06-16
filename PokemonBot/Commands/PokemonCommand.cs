using DiscordDotNetUtilities.Interfaces;
using PokemonBot.BusinessLayer;

namespace PokemonBot.Commands;

public class PokemonCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IDiscordFormatter _discordFormatter;
    private readonly IPokemonBusinessLayer _pokemonBusinessLayer;

    public PokemonCommand(IDiscordFormatter discordFormatter, IPokemonBusinessLayer pokemonBusinessLayer)
    {
        _discordFormatter = discordFormatter;
        _pokemonBusinessLayer = pokemonBusinessLayer;
    }

    [SlashCommand("pokemon", "Allow the users currently in the specified role to administrate the bot.")]
    public async Task Pokemon(
        [Summary("name_or_number", "The Pokémon name or ID that you're searching for.")] string nameOrNumber)
    {
        await DeferAsync();

        var pokemon = await _pokemonBusinessLayer.GetPokemon(nameOrNumber);

        await FollowupAsync(embed: _discordFormatter.BuildRegularEmbed(pokemon.Name, $"found", Context.User));
    }
}