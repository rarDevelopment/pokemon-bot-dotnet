using PokeApiNet;
using System.Text.RegularExpressions;
using Type = PokeApiNet.Type;

namespace PokemonBot.Models
{
    public static class Helpers
    {
        public static void AddTypeEffectiveness(this Dictionary<string, decimal> typeEffectivenessDictionary,
            List<NamedApiResource<Type>> damageRelations, decimal multiplier)
        {
            foreach (var damageRelation in damageRelations)
            {
                if (typeEffectivenessDictionary.ContainsKey(damageRelation.Name))
                {
                    typeEffectivenessDictionary[damageRelation.Name] *= multiplier;
                }
                else
                {
                    typeEffectivenessDictionary.Add(damageRelation.Name, multiplier);
                }
            }
        }

        public static string ToTitleCase(this string text)
        {
            var fixedTextPieces = text
                .ToLower()
                .Split(" ")
                .Select(s => s[..1].ToUpper() + s[1..].ToLower());

            return string.Join(" ", fixedTextPieces);
        }

        public static string CleanVersionName(this string versionName)
        {
            return versionName.CleanGeneralName()
                .Replace("Lets", "Let's")
                .Replace("Leafgreen", "LeafGreen")
                .Replace("Firered", "FireRed")
                .Replace("Soulsilver", "SoulSilver")
                .Replace("Heartgold", "HeartGold");
        }

        public static string CleanGeneralName(this string nameToClean)
        {
            return nameToClean.ToLower()
                .Replace("-", " ")
                .ToTitleCase();
        }

        public static string CleanGenerationName(this string nameToClean)
        {
            var regex = new Regex("^M{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$", RegexOptions.IgnoreCase);

            var fixedTextPieces = nameToClean
                .CleanGeneralName()
                .ToLower()
                .Split(" ")
                .Select(s =>
                {
                    if (regex.IsMatch(s))
                    {
                        return s.ToUpper();
                    }
                    return s[..1].ToUpper() + s[1..].ToLower();
                });

            return string.Join(" ", fixedTextPieces);
        }
    }
}
