using PokemonBot.Models;
using PokemonBot.ServiceLayer;

namespace PokemonBot.BusinessLayer
{
    public class PokemonBusinessLayer : IPokemonBusinessLayer
    {
        private const string Language = "en";
        private readonly IPokeApiServiceLayer _pokeApiServiceLayer;
        public PokemonBusinessLayer(IPokeApiServiceLayer pokeApiServiceLayer)
        {
            _pokeApiServiceLayer = pokeApiServiceLayer;
        }

        public async Task<PokemonDetail> GetPokemon(string identifier)
        {
            var identifierFixed = FixSpecialCases(identifier);
            var pokemon = await _pokeApiServiceLayer.GetPokemon(identifierFixed);
            var species = await _pokeApiServiceLayer.GetPokemonSpecies(identifierFixed);
            var generation = await _pokeApiServiceLayer.GetGeneration(species.Generation.Name);

            var flavorTextOptions = species.FlavorTextEntries.Where(f => f.Language.Name == Language).ToList();
            var flavorText = flavorTextOptions.Any() ? flavorTextOptions[GetRandomIndex(0, flavorTextOptions.Count)].FlavorText : null;

            return new PokemonDetail
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Image = pokemon.Sprites.Other.OfficialArtwork.FrontDefault,
                Genera = species.Genera.FirstOrDefault(g => g.Language.Name == Language)?.Genus,
                Types = pokemon.Types.OrderBy(t => t.Slot).Select(t => t.Type.Name).ToList(),
                Generation = generation.Name,
                GenerationRegion = generation.MainRegion.Name,
                FlavorText = flavorText
            };
        }

        private int GetRandomIndex(int start, int end)
        {
            return new Random().Next(start, end);
        }

        private string ToTitleCase(string text)
        {
            var fixedTextPieces = text
                .ToLower()
                .Split(" ")
                .Select(s => s[..1].ToUpper() + s[1..].ToLower());

            return string.Join(" ", fixedTextPieces);
        }

        private string FixSpecialCases(string identifier)
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
}
