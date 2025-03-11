using PokemonAPIBusinessLayer.Models;

namespace PokemonAPIBusinessLayer.GetPokemonInformationLogic;

/// <summary>
/// Interface for the GetPokemonInformationLogic describing the methods that the logic must implement.
/// </summary>
public interface IGetPokemonInformationLogic
{
    /// <summary>
    /// Tries to get the basic information of a pokemon from the cache db.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public PokemonSpeciesModel? TryGetPokemonBasicInformationFromCache(string pokemonName);

    /// <summary>
    /// Tries to get the basic information of a pokemon from the external api.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <param name="httpClient">httpClient to use for the retrieval</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public Task<PokemonSpeciesModel?> TryGetPokemonBasicInformationFromExternalSources(string pokemonName, HttpClient httpClient);

    /// <summary>
    /// Tries to get the translated information of a pokemon from the cache db.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public PokemonSpeciesModel? TryGetPokemonInformationTranslatedFromCache(string pokemonName);

    /// <summary>
    /// Tries to get the translated information of a pokemon from the external api.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <param name="httpClient">httpClient to use for the retrieval</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    Task<PokemonSpeciesModel?> TryGetPokemonInformationTranslatedFromExternalSources(string pokemonName, HttpClient httpClient);
}