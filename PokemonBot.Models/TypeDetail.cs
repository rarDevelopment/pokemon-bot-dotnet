namespace PokemonBot.Models
{
    public class TypeDetail
    {
        public string Name { get; set; }
        public IEnumerable<string> DoubleDamageFrom { get; set; }
        public IEnumerable<string> HalfDamageFrom { get; set; }
        public IEnumerable<string> NoDamageFrom { get; set; }
        public IEnumerable<string> DoubleDamageTo { get; set; }
        public IEnumerable<string> HalfDamageTo { get; set; }
        public IEnumerable<string> NoDamageTo { get; set; }
    }
}
