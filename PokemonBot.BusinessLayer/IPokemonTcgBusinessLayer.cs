using PokemonBot.Models;

namespace PokemonBot.BusinessLayer;

public interface IPokemonTcgBusinessLayer
{
    Task<PokemonCardDetail> GetPokemonCard(string? cardName = null, string? setName = null, string? cardNumber = null);
    Task<IReadOnlyList<string>> GetSets();
}