using PokemonBot.Models;

namespace PokemonBot.BusinessLayer;

public interface IPokemonBusinessLayer
{
    Task<PokemonDetail> GetPokemon(string identifier);
}