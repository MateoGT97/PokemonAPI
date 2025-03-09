using PokemonAPI.BusinessLogic.Enums;
using PokemonAPI.BusinessLogic.Interfaces;
using PokemonAPI.Helpers;
using PokemonAPI.Models;
using System.Text.Json.Nodes;

namespace PokemonAPI.BusinessLogic.Implementations;

/// <summary>
/// Custom parser for PokemonSpeciesModel class
/// </summary>
public class PokemonSpeciesParser : IPokemonSpeciesParser
{
    public BuildingSource? BuildingSource { get; set; }
    public PokemonSpeciesModel Model { get; set; } = new PokemonSpeciesModel();
    public string? SpeciesName { get; set; }

    /// <summary>
    /// Parses basic information of a Pokemon from a stream
    /// </summary>
    /// <param name="speciesStream">The stream from which to parse the basic information.</param>
    /// <returns>object with parsed Information</returns>
    public bool ParseBasicInformationFromStream(Stream? speciesStream)
    {
        if (speciesStream == null)
        {
            return false;
        }

        try
        {
            JsonNode? rootSpeciesNode = JsonNode.Parse(speciesStream);
            if (BuildingSource == Enums.BuildingSource.ExternalAPI)
            {
                return BasicInformationFromExternalJsonNode(rootSpeciesNode);
            }
            return false;
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

        Model.Name = SpeciesName;
        Model.IsLegendary = isLegendary;
        Model.Habitat = habitatName;
        Model.Description = description;
        return true;
    }

}