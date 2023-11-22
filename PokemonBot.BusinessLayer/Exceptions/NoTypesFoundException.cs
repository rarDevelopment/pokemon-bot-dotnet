using PokeApiNet;

namespace PokemonBot.BusinessLayer.Exceptions;

public class NoTypesFoundException(string identifier, Pokemon pokemon) : Exception($"No types found for Pokémon {pokemon.Name} found with identifier {identifier}")
{
    public string Identifier { get; } = identifier;

    public Pokemon PokemonSearched { get; } = pokemon;
}