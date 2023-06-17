namespace PokemonBot.BusinessLayer.Exceptions;

public class PokemonNotFoundException : Exception
{
    public PokemonNotFoundException(string identifier) : base($"Pokémon not found with identifier {identifier}")
    {
        Identifier = identifier;
    }

    public string Identifier { get; set; }
}