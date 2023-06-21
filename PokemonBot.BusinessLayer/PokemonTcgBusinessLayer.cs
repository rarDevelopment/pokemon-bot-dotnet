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

    public async Task<List<PokemonCardDetail>> GetPokemonCards(string? cardName = null, string? setName = null,
        string? cardNumber = null)
    {
        var cards = await _pokemonTcgServiceLayer.GetPokemonCard(cardName, setName, cardNumber);
        if (cards.Results.Count == 0)
        {
            throw new PokemonCardNotFoundException(cardName, setName, cardNumber);
        }

        return cards.Results.OrderBy(card => card.Number).Select(card => new PokemonCardDetail
        {
            Name = card.Name,
            ImageUrl = card.Images.Large.ToString()
        }).ToList();
    }

    public async Task<IReadOnlyList<string>> GetSets()
    {
        var sets = await _pokemonTcgServiceLayer.GetSets();
        return sets;
    }
}