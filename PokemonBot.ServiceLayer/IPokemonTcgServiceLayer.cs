using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;

namespace PokemonBot.ServiceLayer;

public interface IPokemonTcgServiceLayer
{
    Task<ApiResourceList<PokemonCard>> GetPokemonCard(string cardId);
}