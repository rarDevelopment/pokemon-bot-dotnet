namespace PokemonBot.BusinessLayer.Exceptions;

public class GenerationNotFoundException : Exception
{
    public GenerationNotFoundException(string identifier) : base($"Generation not found with identifier {identifier}")
    {
        Identifier = identifier;
    }

    public string Identifier { get; }
}