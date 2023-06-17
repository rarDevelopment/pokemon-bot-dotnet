namespace PokemonBot.Models;

public class PokemonDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string? Genera { get; set; }
    public IReadOnlyList<string> Types { get; set; } = new List<string>();
    public string Generation { get; set; }
    public string GenerationRegion { get; set; }
    public string? FlavorText { get; set; }
    public IReadOnlyList<KeyValuePair<string, decimal>> Weaknesses { get; set; } = new List<KeyValuePair<string, decimal>>();
    public IReadOnlyList<KeyValuePair<string, decimal>> Resistances { get; set; } = new List<KeyValuePair<string, decimal>>();
    public IReadOnlyList<KeyValuePair<string, decimal>> Immunities { get; set; } = new List<KeyValuePair<string, decimal>>();
    public int Weight { get; set; }
    public int Height { get; set; }
    public string? FlavorTextVersion { get; set; }
    public string? FrontSprite { get; set; }

    public string GetImageUrlToDisplay()
    {
        if (!string.IsNullOrEmpty(Image))
        {
            return Image;
        }

        if (!string.IsNullOrEmpty(FrontSprite))
        {
            return FrontSprite;
        }

        return string.Empty;
    }
}