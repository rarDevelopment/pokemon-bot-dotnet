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

    public async Task<PokemonCardDetail> GetPokemonCard(string? cardName = null, string? setName = null, string? cardNumber = null)
    {
        var cards = await _pokemonTcgServiceLayer.GetPokemonCard(cardName, setName, cardNumber);
        if (cards.Results.Count == 0)
        {
            throw new PokemonCardNotFoundException(cardName, setName, cardNumber);
        }

        var card = cards.Results[0];

        return new PokemonCardDetail
        {
            Name = card.Name,
            ImageUrl = card.Images.Large.ToString()
        };
    }

    public async Task<IReadOnlyList<string>> GetSets()
    {
        var sets = await _pokemonTcgServiceLayer.GetSets();
        return sets;
    }
}