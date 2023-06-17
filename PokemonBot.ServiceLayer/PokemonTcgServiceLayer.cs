using PokemonBot.Models;
using PokemonTcgSdk.Standard.Features.FilterBuilder.Pokemon;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;

namespace PokemonBot.ServiceLayer;

public class PokemonTcgServiceLayer : IPokemonTcgServiceLayer
{
    private readonly PokemonApiClient _client;

    public PokemonTcgServiceLayer(PokemonTcgSettings pokemonTcgSettings)
    {
        _client = new PokemonApiClient(pokemonTcgSettings.ApiKey);
    }

    public async Task<ApiResourceList<PokemonCard>> GetPokemonCard(string cardId)
    {
        var filter = PokemonFilterBuilder.CreatePokemonFilter().AddId(cardId);
        return await _client.GetApiResourceAsync<PokemonCard>(filter);
    }
}