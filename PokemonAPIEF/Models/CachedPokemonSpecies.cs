namespace PokemonAPIEF.Models;
/// <summary>
/// Model to represent a cached Pokemon species.
/// </summary>
public class CachedPokemonSpecies
{
    public string? Description { get; set; } = null!;
    public string? HabitatName { get; set; } = null!;
    public bool? IsLegendary { get; set; } = null;
    public DateTime LastCached { get; set; }
    public string SpeciesName { get; set; } = null!;
    public string? TranslatedDescription { get; set; } = null!;
}