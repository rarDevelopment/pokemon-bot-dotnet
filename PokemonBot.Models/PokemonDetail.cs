namespace PokemonBot.Models;

public class PokemonDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string? Genera { get; set; }
    public IReadOnlyList<string> Types { get; set; }
    public string Generation { get; set; }
    public string GenerationRegion { get; set; }
    public string? FlavorText { get; set; }
}