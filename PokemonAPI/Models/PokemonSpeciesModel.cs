namespace PokemonAPI.Models;

/// <summary>
/// Pokemon Species Model
/// </summary>
public class PokemonSpeciesModel
{
    public string? Name { get; set; }
    public string? Habitat { get; set; }
    public bool? IsLegendary { get; set; }
    public string? Description { get; set; }
}