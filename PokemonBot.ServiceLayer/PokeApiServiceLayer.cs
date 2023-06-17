using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PokeApiNet;
using Type = PokeApiNet.Type;

namespace PokemonBot.ServiceLayer;

public class PokeApiServiceLayer : IPokeApiServiceLayer
{
    private readonly PokeApiClient _pokeApiClient;
    private readonly Logger<PokeApiServiceLayer> _logger;

    public PokeApiServiceLayer()
    {
        _pokeApiClient = new PokeApiClient();
        _logger = new Logger<PokeApiServiceLayer>(new NullLoggerFactory());
    }

    public async Task<Pokemon?> GetPokemon(string identifier)
    {
        return await Get<Pokemon>(identifier);
    }

    public async Task<Type?> GetType(string identifier)
    {
        return await Get<Type>(identifier);
    }

    public async Task<PokemonSpecies?> GetPokemonSpecies(string identifier)
    {
        return await Get<PokemonSpecies>(identifier);
    }

    public async Task<Generation?> GetGeneration(string generationName)
    {
        return await Get<Generation>(generationName);
    }

    private async Task<T?> Get<T>(string searchTerm) where T : NamedApiResource
    {
        try
        {
            return await _pokeApiClient.GetResourceAsync<T>(searchTerm);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Not found {searchTerm} - {ex.Message}");
            return null;
        }
    }
}