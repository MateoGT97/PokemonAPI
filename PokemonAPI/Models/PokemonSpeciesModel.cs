namespace PokemonAPI.Models;
public class PokemonSpeciesModel
{
    public string? Name { get; internal set; }
    public string? Habitat { get; internal set; }
    public bool? IsLegendary { get; internal set; }
    public string? Description { get; internal set; }
}