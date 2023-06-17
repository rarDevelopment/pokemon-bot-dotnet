using PokeApiNet;
using PokemonBot.BusinessLayer.Exceptions;
using PokemonBot.Models;
using PokemonBot.ServiceLayer;
using Type = PokeApiNet.Type;

namespace PokemonBot.BusinessLayer;

public class PokemonBusinessLayer : IPokemonBusinessLayer
{
    private const string Language = "en";
    private readonly IPokeApiServiceLayer _pokeApiServiceLayer;
    public PokemonBusinessLayer(IPokeApiServiceLayer pokeApiServiceLayer)
    {
        _pokeApiServiceLayer = pokeApiServiceLayer;
    }

    public async Task<PokemonDetail?> GetPokemon(string identifier)
    {
        var identifierFixed = FixSpecialCases(identifier);
        var pokemon = await _pokeApiServiceLayer.GetPokemon(identifierFixed);
        var species = await _pokeApiServiceLayer.GetPokemonSpecies(identifierFixed);
        if (pokemon == null || species == null)
        {
            throw new PokemonNotFoundException(identifier);
        }

        var types = await GetTypes(pokemon);

        if (types.Count == 0)
        {
            throw new NoTypesFoundException(identifier, pokemon);
        }

        var generation = await _pokeApiServiceLayer.GetGeneration(species.Generation.Name);

        if (generation == null)
        {
            throw new GenerationNotFoundException(species.Generation.Name);
        }

        var typeEffectivenessDictionary = new Dictionary<string, decimal>();
        typeEffectivenessDictionary.AddTypeEffectiveness(types[0].DamageRelations.DoubleDamageFrom, 2);
        typeEffectivenessDictionary.AddTypeEffectiveness(types[0].DamageRelations.HalfDamageFrom, 0.5m);
        typeEffectivenessDictionary.AddTypeEffectiveness(types[0].DamageRelations.NoDamageFrom, 0);

        if (types.Count > 1)
        {
            typeEffectivenessDictionary.AddTypeEffectiveness(types[1].DamageRelations.DoubleDamageFrom, 2);
            typeEffectivenessDictionary.AddTypeEffectiveness(types[1].DamageRelations.HalfDamageFrom, 0.5m);
            typeEffectivenessDictionary.AddTypeEffectiveness(types[1].DamageRelations.NoDamageFrom, 0);
        }

        var flavorTextLanguageEntries = species.FlavorTextEntries.Where(f => f.Language.Name == Language).ToList();
        var flavorTextOption = flavorTextLanguageEntries.Any() ? flavorTextLanguageEntries[GetRandomIndex(0, flavorTextLanguageEntries.Count)] : null;

        return new PokemonDetail
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Image = pokemon.Sprites.Other.OfficialArtwork.FrontDefault,
            Genera = species.Genera.FirstOrDefault(g => g.Language.Name == Language)?.Genus,
            Types = pokemon.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList(),
            Weaknesses = typeEffectivenessDictionary.Where(t => t.Value > 1).ToList(),
            Resistances = typeEffectivenessDictionary.Where(t => t.Value is < 1 and > 0).ToList(),
            Immunities = typeEffectivenessDictionary.Where(t => t.Value == 0).ToList(),
            Generation = generation.Name,
            GenerationRegion = generation.MainRegion.Name,
            FlavorText = flavorTextOption?.FlavorText ?? null,
            FlavorTextVersion = flavorTextOption?.Version.Name ?? null,
            Weight = pokemon.Weight,
            Height = pokemon.Height,
            FrontSprite = pokemon.Sprites.FrontDefault,
        };
    }

    private async Task<IReadOnlyList<Type>> GetTypes(Pokemon pokemon)
    {
        var types = new List<Type>();
        foreach (var pokemonType in pokemon.Types)
        {
            var type = await _pokeApiServiceLayer.GetType(pokemonType.Type.Name);
            if (type != null)
            {
                types.Add(type);
            }
        }

        return types;
    }

    private static int GetRandomIndex(int start, int end)
    {
        return new Random().Next(start, end);
    }

    private static string FixSpecialCases(string identifier)
    {
        var identifierCleaned = identifier.Trim().ToLower();
        var identifierWithoutSpaces = identifierCleaned.Replace(" ", "");

        if (identifierWithoutSpaces == "nidoran♀️" || identifierWithoutSpaces == "nidoran\u2640")
        {
            return "nidoran-f";
        }
        if (identifierWithoutSpaces == "nidoran♂️" || identifierWithoutSpaces == "nidoran\u2642")
        {
            return "nidoran-m";
        }
        if (identifierWithoutSpaces.Replace(".", "") == "mrmime")
        {
            return "mr-mime";
        }
        if (identifierWithoutSpaces.Replace(".", "") == "mimejr")
        {
            return "mime-jr";
        }
        if (identifierWithoutSpaces.Replace(".", "") == "mrrime")
        {
            return "mr-rime";
        }
        if (identifierWithoutSpaces.Replace("'", "") == "farfetchd")
        {
            return "farfetchd";
        }
        if (identifierWithoutSpaces.Replace("'", "") == "sirfetchd")
        {
            return "sirfetchd";
        }
        if (identifierWithoutSpaces.Replace(":", "") == "typenull")
        {
            return "type-null";
        }

        return identifierCleaned.Normalize();
    }
}