namespace PokemonBot.BusinessLayer.Exceptions;

public class GenerationNotFoundException(string identifier) : Exception($"Generation not found with identifier {identifier}")
{
    public string Identifier { get; } = identifier;
}