using PokemonBot.Models;

namespace PokemonBot.BusinessLayer;

public interface IPokemonTcgBusinessLayer
{
    Task<PokemonCardDetail> GetPokemonCard(string cardId);
}