using PokemonBot.Models;

namespace PokemonBot.BusinessLayer;

public interface IPokemonTcgBusinessLayer
{
    Task<List<PokemonCardDetail>> GetPokemonCards(string? cardName = null, string? setName = null,
        string? cardNumber = null);
    Task<IReadOnlyList<string>> GetSets();
}