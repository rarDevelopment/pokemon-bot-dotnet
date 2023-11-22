namespace PokemonBot.BusinessLayer.Exceptions;

public class PokemonNotFoundException(string identifier) : Exception($"Pokémon not found with identifier {identifier}")
{
    public string Identifier { get; } = identifier;
}