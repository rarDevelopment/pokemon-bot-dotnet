namespace PokemonBot.Models
{
    public record TypeDetail(
        string Name,
        IEnumerable<string> DoubleDamageFrom,
        IEnumerable<string> HalfDamageFrom,
        IEnumerable<string> NoDamageFrom,
        IEnumerable<string> DoubleDamageTo,
        IEnumerable<string> HalfDamageTo,
        IEnumerable<string> NoDamageTo)
    {
        public string Name { get; set; } = Name;
        public IEnumerable<string> DoubleDamageFrom { get; set; } = DoubleDamageFrom;
        public IEnumerable<string> HalfDamageFrom { get; set; } = HalfDamageFrom;
        public IEnumerable<string> NoDamageFrom { get; set; } = NoDamageFrom;
        public IEnumerable<string> DoubleDamageTo { get; set; } = DoubleDamageTo;
        public IEnumerable<string> HalfDamageTo { get; set; } = HalfDamageTo;
        public IEnumerable<string> NoDamageTo { get; set; } = NoDamageTo;
    }
}
