using PokemonAPIBusinessLayer.Helpers;
using PokemonAPIBusinessLayer.Models;
using PokemonAPIEF.Models;
using System.Text.Json.Nodes;

namespace PokemonAPIBusinessLayer.SpeciesParser;

/// <summary>
/// Custom parser for PokemonSpeciesModel class
/// </summary>
public class PokemonSpeciesParser : IPokemonSpeciesParser
{
    public CachedPokemonSpecies Model { get; set; } = new CachedPokemonSpecies();
    public string? SpeciesName { get; set; }

    /// <summary>
    /// Parses basic information of a Pokemon from a stream
    /// </summary>
    /// <param name="speciesStream">The stream from which to parse the basic information.</param>
    /// <returns>True if successfully parsed, False otherwise</returns>
    public bool ParseBasicInformationFromStream(Stream? speciesStream)
    {
        if (speciesStream == null)
        {
            return false;
        }

        try
        {
            JsonNode? rootSpeciesNode = JsonNode.Parse(speciesStream);
            return BasicInformationFromExternalJsonNode(rootSpeciesNode);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Parses the translation of the descriptioon for the Pokemon, 
    // if not possible to translate, the description remains as the standard one
    /// </summary>
    /// <param name="translationStream">The stream from which to parse the description translation.</param>
    /// <returns>True if successfully parsed, False otherwise</returns>
    public bool ParseTranslationFromStream(Stream? translationStream)
    {
        if (translationStream == null)
        {
            return false;
        }
        try
        {
            JsonNode? rootSpeciesNode = JsonNode.Parse(translationStream);
            return TranslationFromExternalJsonNode(rootSpeciesNode);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Parses a PokemonSpeciesModel's basic info from a JsonNode
    /// </summary>
    /// <param name="rootJsonNode">Pokemon root JsonNode</param>
    /// <returns>True if successfully parsed, False otherwise</returns>
    private bool BasicInformationFromExternalJsonNode(JsonNode? rootJsonNode)
    {
        if (rootJsonNode == null)
        {
            return false;
        }
        /* Assuming to take the first flavor entry? seems like it varies depending on game's version
        a specific version could requested on the entry point as a future development */
        string description = rootJsonNode["flavor_text_entries"]?[0]?["flavor_text"]?
            .ToString().SanitizeStringFromSpecialCharacters() ?? "No Description found for pokemon species";
        string habitatName = rootJsonNode["habitat"]?["name"]?
            .ToString().SanitizeStringFromSpecialCharacters() ?? "Habitat not found";
        bool? isLegendary = rootJsonNode["is_legendary"]?.GetValue<bool>();

        Model.SpeciesName = SpeciesName!;
        Model.IsLegendary = isLegendary;
        Model.HabitatName = habitatName;
        Model.Description = description;
        return true;
    }
    
    /// <summary>
    /// Parses a PokemonSpeciesModel's description's translation from a JsonNode
    /// </summary>
    /// <param name="rootTranslationNode">Translation root JsonNode</param>
    /// <returns>True if successfully parsed, False otherwise</returns>
    private bool TranslationFromExternalJsonNode(JsonNode? rootTranslationNode)
    {
        if (rootTranslationNode == null)
        {
            return false;
        }
        string? translation = rootTranslationNode["contents"]?["translated"]?
            .ToString().SanitizeStringFromSpecialCharacters() ?? Model.Description;

        Model.TranslatedDescription = translation;
        return true;
    }
}