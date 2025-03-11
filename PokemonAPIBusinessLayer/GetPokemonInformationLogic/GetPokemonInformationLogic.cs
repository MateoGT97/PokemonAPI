using System.Net.Http.Json;
using PokemonAPIBusinessLayer.Logging;
using PokemonAPIBusinessLayer.Models;
using PokemonAPIBusinessLayer.PokemonRepository;
using PokemonAPIBusinessLayer.SpeciesParser;
using PokemonAPIEF.Models;

namespace PokemonAPIBusinessLayer.GetPokemonInformationLogic;

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

    public async Task<PokemonSpeciesModel?> TryGetPokemonBasicInformationFromExternalSources(string pokemonName, HttpClient httpClient)
    {
        SpeciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfoFromExternalSources(httpClient))
            {
                LoggerUtility?.Logger.Information($"Pokemon basic information successfully retrieved from external sources for pokemon named: {pokemonName}");
                PokemonCacheRepository.AddOrUpdateCachedPokemonSpecies(new CachedPokemonSpecies()
                {
                    SpeciesName = pokemonName,
                    HabitatName = SpeciesParser.Model.Habitat,
                    Description = SpeciesParser.Model.Description,
                    IsLegendary = SpeciesParser.Model.IsLegendary,
                    LastCached = DateTime.Now
                });
                return SpeciesParser.Model;
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

    public PokemonSpeciesModel? TryGetPokemonInformationTranslatedFromCache(string pokemonName)
    {
        CachedPokemonSpecies? cachedModel = PokemonCacheRepository.GetCachedPokemonSpeciesByName(pokemonName);
        if (cachedModel != null && cachedModel.LastCached > DateTime.Now.AddHours(-1) && cachedModel.TranslatedDescription != null)
        {
            LoggerUtility?.Logger.Information($"Pokemon basic information successfully retrieved from cache for pokemon named: {pokemonName}");
            return new(){
                Name = cachedModel.SpeciesName,
                Habitat = cachedModel.HabitatName,
                Description = cachedModel.TranslatedDescription,
                IsLegendary = cachedModel.IsLegendary
            };
        }
        return null;
    }

    public async Task<PokemonSpeciesModel?> TryGetPokemonInformationTranslatedFromExternalSources(string pokemonName, HttpClient httpClient)
    {
        SpeciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfoWithTranslationFromExternalSources(httpClient))
            {
                LoggerUtility?.Logger.Information($"Pokemon basic information with translation successfully retrieved from external sources for pokemon named: {pokemonName}");
                PokemonCacheRepository.AddOrUpdateCachedPokemonSpecies(new CachedPokemonSpecies()
                {
                    SpeciesName = pokemonName,
                    HabitatName = SpeciesParser.Model.Habitat,
                    TranslatedDescription = SpeciesParser.Model.Description,
                    IsLegendary = SpeciesParser.Model.IsLegendary,
                    LastCached = DateTime.Now
                });
                return SpeciesParser.Model;
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
        if (SpeciesParser.Model.Habitat == "cave" || (SpeciesParser.Model.IsLegendary ?? false))
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
        return false;
    }
}