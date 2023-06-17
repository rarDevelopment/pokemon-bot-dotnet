namespace PokemonBot.BusinessLayer.Exceptions;

public class PokemonCardNotFoundException : Exception
{
    public string CardId { get; }
    public PokemonCardNotFoundException(string cardId) : base($"No card found with the identifier {cardId}")
    {
        CardId = cardId;
    }
}