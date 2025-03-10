using PokemonAPI.BusinessLogic.Enums;
using PokemonAPI.Models;

namespace PokemonAPI.BusinessLogic.Interfaces;

public interface IPokemonSpeciesParser
{
    public BuildingSource? BuildingSource { get; set; }
    public PokemonSpeciesModel Model { get; set; }
    public string? SpeciesName { get; set; }
    public bool ParseBasicInformationFromStream(Stream? speciesStream);
}