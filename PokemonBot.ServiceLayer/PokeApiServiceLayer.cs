using PokeApiNet;
using Type = PokeApiNet.Type;

namespace PokemonBot.ServiceLayer;

public class PokeApiServiceLayer : IPokeApiServiceLayer
{
    private readonly PokeApiClient _pokeApiClient;

    public PokeApiServiceLayer()
    {
        _pokeApiClient = new PokeApiClient();
    }

    public async Task<Pokemon> GetPokemon(string identifier)
    {
        return await _pokeApiClient.GetResourceAsync<Pokemon>(identifier);
    }

    public async Task<Type> GetType(string identifier)
    {
        return await _pokeApiClient.GetResourceAsync<Type>(identifier);
    }

    public async Task<PokemonSpecies> GetPokemonSpecies(string identifier)
    {
        return await _pokeApiClient.GetResourceAsync<PokemonSpecies>(identifier);
    }

    public async Task<Generation> GetGeneration(string generationName)
    {
        return await _pokeApiClient.GetResourceAsync<Generation>(generationName);
    }
}