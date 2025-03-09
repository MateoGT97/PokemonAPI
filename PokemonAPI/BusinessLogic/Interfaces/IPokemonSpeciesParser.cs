using PokemonAPI.BusinessLogic.Enums;
using PokemonAPI.Models;

namespace PokemonAPI.BusinessLogic.Interfaces;

/// <summary>
/// Interface for Custom parser for PokemonSpeciesModel class
/// </summary>
public interface IPokemonSpeciesParser
{
    public BuildingSource? BuildingSource { get; set; }
    public PokemonSpeciesModel Model { get; set; }
    public string? SpeciesName { get; set; }
    /// <summary>
    /// Parses basic information of a Pokemon from a stream
    /// </summary>
    /// <param name="speciesStream">The stream from which to parse the basic information.</param>
    /// <returns>True if successfully parsed, False otherwise</returns>
    public bool ParseBasicInformationFromStream(Stream? speciesStream);
    
    /// <summary>
    /// Parses the translation of the descriptioon for the Pokemon, 
    // if not possible to translate, the description remains as the standard one
    /// </summary>
    /// <param name="translationStream">The stream from which to parse the description translation.</param>
    /// <returns>True if successfully parsed, False otherwise</returns>
    public bool ParseTranslationFromStream(Stream? translationStream);
}