using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;

namespace PokemonBot.ServiceLayer;

public interface IPokemonTcgServiceLayer
{
    Task<ApiResourceList<PokemonCard>> GetPokemonCard(string? cardName = null, string? setName = null, string? cardNumber = null);
    Task<IReadOnlyList<string>> GetSets();
}