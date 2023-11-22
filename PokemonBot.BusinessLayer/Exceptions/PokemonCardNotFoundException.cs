namespace PokemonBot.BusinessLayer.Exceptions;

public class PokemonCardNotFoundException(string? cardName = null,
        string? setName = null,
        string? cardNumber = null)
    : Exception($"No card found using criteria: cardName={cardName ?? "(not specified)"} setName={setName ?? "(not specified)"} cardNumber={cardNumber ?? "(not specified)"}")
{
    public string? CardName { get; } = cardName;
    public string? SetName { get; } = setName;
    public string? CardNumber { get; } = cardNumber;
}