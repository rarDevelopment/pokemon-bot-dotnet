using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;
using PokemonBot.ServiceLayer;

namespace PokemonBot.BusinessLayer;

public class PokemonTcgBusinessLayer : IPokemonTcgBusinessLayer
{
    private readonly IPokemonTcgServiceLayer _pokemonTcgServiceLayer;

    public PokemonTcgBusinessLayer(IPokemonTcgServiceLayer pokemonTcgServiceLayer)
    {
        _pokemonTcgServiceLayer = pokemonTcgServiceLayer;
    }

    public async Task<PokemonCardDetail> GetPokemonCard(string cardId)
    {
        var cards = await _pokemonTcgServiceLayer.GetPokemonCard(cardId);
        if (cards.Results.Count == 0)
        {
            throw new PokemonCardNotFoundException(cardId);
        }

        var card = cards.Results[0];

        return new PokemonCardDetail
        {
            Name = card.Name,
            ImageUrl = card.Images.Large.ToString()
        };
    }
}