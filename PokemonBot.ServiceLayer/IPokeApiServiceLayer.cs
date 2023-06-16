using PokeApiNet;
using Type = PokeApiNet.Type;

namespace PokemonBot.ServiceLayer;

public interface IPokeApiServiceLayer
{
    Task<Pokemon> GetPokemon(string identifier);
    Task<Type> GetType(string identifier);
    Task<PokemonSpecies> GetPokemonSpecies(string identifier);
    Task<Generation> GetGeneration(string generationName);
}