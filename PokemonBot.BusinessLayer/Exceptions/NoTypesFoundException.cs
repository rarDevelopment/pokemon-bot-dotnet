using PokeApiNet;

namespace PokemonBot.BusinessLayer.Exceptions;

public class NoTypesFoundException : Exception
{
    public NoTypesFoundException(string identifier, Pokemon pokemon)
        : base($"No types found for Pokémon {pokemon.Name} found with identifier {identifier}")
    {
        Identifier = identifier;
        PokemonSearched = pokemon;
    }

    public string Identifier { get; }

    public Pokemon PokemonSearched { get; }
}