using PokemonBot.Models;
using PokemonTcgSdk.Standard.Features.FilterBuilder.Pokemon;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Set;

namespace PokemonBot.ServiceLayer;

public class PokemonTcgServiceLayer(PokemonTcgSettings pokemonTcgSettings) : IPokemonTcgServiceLayer
{
    private readonly PokemonApiClient _client = new PokemonApiClient(pokemonTcgSettings.ApiKey);

    public async Task<ApiResourceList<PokemonCard>> GetPokemonCard(string? cardName = null, string? setName = null, string? cardNumber = null)
    {
        var filter = PokemonFilterBuilder.CreatePokemonFilter();
        if (!string.IsNullOrEmpty(cardName))
        {
            filter.AddName(cardName);
        }

        if (!string.IsNullOrEmpty(setName))
        {
            filter.AddSetName(setName);
        }

        if (!string.IsNullOrEmpty(cardNumber))
        {
            filter.Add("number", cardNumber);
        }

        return await _client.GetApiResourceAsync<PokemonCard>(take: pokemonTcgSettings.CardLimit, skip: 1, filter);
    }

    public async Task<IReadOnlyList<string>> GetSets()
    {
        var sets = await _client.GetApiResourceAsync<Set>();
        return sets.Results.OrderBy(s => s.ReleaseDate).Select(s => s.Name).ToList();
    }
}