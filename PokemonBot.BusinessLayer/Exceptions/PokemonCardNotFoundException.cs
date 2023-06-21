namespace PokemonBot.BusinessLayer.Exceptions;

public class PokemonCardNotFoundException : Exception
{
    public string? CardName { get; }
    public string? SetName { get; }
    public string? CardNumber { get; }

    public PokemonCardNotFoundException(string? cardName = null,
        string? setName = null,
        string? cardNumber = null) : base(
        $"No card found using criteria: cardName={cardName ?? "(not specified)"} setName={setName ?? "(not specified)"} cardNumber={cardNumber ?? "(not specified)"}")
    {
        CardName = cardName;
        SetName = setName;
        CardNumber = cardNumber;
    }
}