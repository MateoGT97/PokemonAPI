using System.Net.Http.Json;
using PokemonAPIBusinessLayer.Logging;
using PokemonAPIBusinessLayer.Models;
using PokemonAPIBusinessLayer.PokemonRepository;
using PokemonAPIBusinessLayer.SpeciesParser;
using PokemonAPIEF.Models;

namespace PokemonAPIBusinessLayer.GetPokemonInformationLogic;

/// <summary>
/// Class that handles the logic to retrieve pokemon information from the cache db or external APIs.
/// </summary>
public class GetPokemonInformationLogic : IGetPokemonInformationLogic
{
    public IPokemonCacheRepository PokemonCacheRepository { get; set; } = null!;
    public IPokemonSpeciesParser SpeciesParser { get; set; } = null!;
    public ILoggerUtility? LoggerUtility { get; set; } = null!;  

    public GetPokemonInformationLogic(ILoggerUtility? loggerUtility, IPokemonCacheRepository pokemonCacheRepository, IPokemonSpeciesParser speciesParser)
    {
        LoggerUtility = loggerUtility;
        PokemonCacheRepository = pokemonCacheRepository;
        SpeciesParser = speciesParser;

        LoggerUtility?.CreateLog(Directory.GetCurrentDirectory());
    }
    
    /// <summary>
    /// Tries to get the basic information of a pokemon from the cache db.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public PokemonSpeciesModel? TryGetPokemonBasicInformationFromCache(string pokemonName)
    {
        CachedPokemonSpecies? cachedModel = PokemonCacheRepository.GetCachedPokemonSpeciesByName(pokemonName);
        if (cachedModel != null && cachedModel.LastCached > DateTime.Now.AddHours(-1))
        {
            LoggerUtility?.Logger.Information($"Pokemon basic information successfully retrieved from cache for pokemon named: {pokemonName}");
            return new(){
                Name = cachedModel.SpeciesName,
                Habitat = cachedModel.HabitatName,
                Description = cachedModel.Description,
                IsLegendary = cachedModel.IsLegendary
            };
        }
        return null;
    }

    /// <summary>
    /// Tries to get the basic information of a pokemon from the external api.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <param name="httpClient">httpClient to use for the retrieval</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public async Task<PokemonSpeciesModel?> TryGetPokemonBasicInformationFromExternalSources(string pokemonName, HttpClient httpClient)
    {
        SpeciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfoFromExternalSources(httpClient))
            {
                LoggerUtility?.Logger.Information($"Pokemon basic information successfully retrieved from external sources for pokemon named: {pokemonName}");
                SpeciesParser.Model.LastCached = DateTime.Now;
                PokemonCacheRepository.AddOrUpdateCachedPokemonSpecies(SpeciesParser.Model);
                return new(){
                    Name = SpeciesParser.Model.SpeciesName,
                    Habitat = SpeciesParser.Model.HabitatName,
                    Description = SpeciesParser.Model.Description,
                    IsLegendary = SpeciesParser.Model.IsLegendary
                };
            }
            LoggerUtility?.Logger.Error($"It wasn't possible to retrieve basic information from external sources for pokemon named: {pokemonName}");
            return null;
        }
        catch (Exception exception)
        {
            LoggerUtility?.Logger.Fatal($"A fatal error happened when trying to retrieve basic information " +
                $"from external sources for pokemon named: {pokemonName}. Exception: {exception}");
            return null;
        }
    }

    /// <summary>
    /// Tries to get the translated information of a pokemon from the cache db.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public PokemonSpeciesModel? TryGetPokemonInformationTranslatedFromCache(string pokemonName)
    {
        CachedPokemonSpecies? cachedModel = PokemonCacheRepository.GetCachedPokemonSpeciesByName(pokemonName);
        if (cachedModel != null && cachedModel.LastCached > DateTime.Now.AddHours(-1) && cachedModel.TranslatedDescription != null)
        {
            LoggerUtility?.Logger.Information($"Pokemon basic information successfully retrieved from cache for pokemon named: {pokemonName}");
            return new(){
                Name = cachedModel.SpeciesName,
                Habitat = cachedModel.HabitatName,
                Description = cachedModel.TranslatedDescription ?? cachedModel.Description,
                IsLegendary = cachedModel.IsLegendary
            };
        }
        return null;
    }

    /// <summary>
    /// Tries to get the translated information of a pokemon from the external api.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon species to retrieve</param>
    /// <param name="httpClient">httpClient to use for the retrieval</param>
    /// <returns>PokemonSpecies model retrieved, null if it was not possible to retrieve</returns>
    public async Task<PokemonSpeciesModel?> TryGetPokemonInformationTranslatedFromExternalSources(string pokemonName, HttpClient httpClient)
    {
        SpeciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfoWithTranslationFromExternalSources(httpClient))
            {
                LoggerUtility?.Logger.Information($"Pokemon basic information with translation successfully retrieved from external sources for pokemon named: {pokemonName}");
                SpeciesParser.Model.LastCached = DateTime.Now;
                PokemonCacheRepository.AddOrUpdateCachedPokemonSpecies(SpeciesParser.Model);
                return new(){
                    Name = SpeciesParser.Model.SpeciesName,
                    Habitat = SpeciesParser.Model.HabitatName,
                    Description = SpeciesParser.Model.TranslatedDescription ?? SpeciesParser.Model.Description,
                    IsLegendary = SpeciesParser.Model.IsLegendary
                };
            }
            LoggerUtility?.Logger.Error($"It wasn't possible to retrieve basic information with translation from external sources for pokemon named: {pokemonName}");
            return null;
        }
        catch (Exception exception)
        {
            LoggerUtility?.Logger.Fatal($"A fatal error happened when trying to retrieve basic information with translation" +
                $"from external sources for pokemon named: {pokemonName}. Exception: {exception}");
            return null;
        }
    }

    /// <summary>
    /// Retrieves a PokemonSpeciesModel with its basic information from external sources.
    /// </summary>
    /// <returns>True if the PokemonSpeciesModel was successfully retrieved, False otherwise</returns>
    private async Task<bool> RetrieveBasicInfoFromExternalSources(HttpClient httpClient)
    {
        LoggerUtility?.Logger.Information($"Making request for basic pokemon information from external sources for pokemon named: {SpeciesParser.Model}");
        HttpResponseMessage response = await httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{SpeciesParser.SpeciesName}");
        if (response.IsSuccessStatusCode)
        {
            return SpeciesParser.ParseBasicInformationFromStream(response.Content.ReadAsStream());
        }
        return false;
    }

    /// <summary>
    /// Retrieves a PokemonSpeciesModel with its basic information and translation if possible from external sources.
    /// </summary>
    /// <returns>True if the PokemonSpeciesModel was successfully retrieved, False otherwise</returns>
    private async Task<bool> RetrieveBasicInfoWithTranslationFromExternalSources(HttpClient httpClient)
    {
        LoggerUtility?.Logger.Information($"Making request for basic pokemon information for translation from external sources for pokemon named: {SpeciesParser.Model}");
        if (!await RetrieveBasicInfoFromExternalSources(httpClient))
        {
            return false;
        }
        string endpoint;
        var payload = new {text = SpeciesParser.Model.Description};
        if (SpeciesParser.Model.HabitatName == "cave" || (SpeciesParser.Model.IsLegendary ?? false))
        {
            endpoint = "https://api.funtranslations.com/translate/yoda.json";
        }
        else
        {
            endpoint = "https://api.funtranslations.com/translate/shakespeare.json";
        }
        LoggerUtility?.Logger.Information($"Making request for description translation from external sources for pokemon named: {SpeciesParser.Model}");
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, payload);
        if (response.IsSuccessStatusCode)
        {
            return SpeciesParser.ParseTranslationFromStream(response.Content.ReadAsStream());
        }
        return true;
    }
}