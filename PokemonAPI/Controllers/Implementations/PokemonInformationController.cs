using Microsoft.AspNetCore.Mvc;
using PokemonAPI.BusinessLogic.Interfaces;
using PokemonAPI.BusinessLogic.Enums;
using PokemonAPI.Controllers.Interfaces;
using PokemonAPI.Logging;

namespace PokemonAPI.Controllers.Implementations;

/// <summary>
/// Pokemon Information API Controller that exposes entry points to retrieve information about Pokemons.
/// </summary>
/// <param name="loggerUtility">logger utility Instance that the controller is going to use.</param>
/// <param name="httpClientFactory">httpClientFactory that the controller is going to use to instantiate httpClients.</param>
/// <param name="pokemonSpeciesParser">pokemonspeciesparser instance used by the controller.</param>
[ApiController]
[Route("Pokemon")]
public class PokemonInformationController : ControllerBase, IBasicPokemonInformation, ITranslatePokemonDescription
{
    private readonly ILoggerUtility? _loggerUtility;
    private readonly HttpClient _httpClient;
    private readonly IPokemonSpeciesParser _speciesParser;

    public PokemonInformationController(ILoggerUtility? loggerUtility, IHttpClientFactory httpClientFactory,
        IPokemonSpeciesParser pokemonSpeciesParser)
    {
        _loggerUtility = loggerUtility;
        _httpClient = httpClientFactory.CreateClient();
        _speciesParser = pokemonSpeciesParser;

        loggerUtility?.CreateLog(Directory.GetCurrentDirectory());
    }

    /// <summary>
    /// API entrypoint that takes the name of a pokemon and retrieves the basic information for it, 
    // which includes its name, habitat, description and legendary condition.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon for which the information is being requested.</param>
    /// <returns>An IActionResult instance that in case of positive result contains the model of the pokemon requested.</returns>
    [HttpGet("{pokemonName}")]
    public async Task<IActionResult> GetPokemonBasicInformation(string pokemonName)
    {
        _loggerUtility?.Logger.Information($"Get pokemon basic information request received for Pokemon with name {pokemonName}");
        _speciesParser.BuildingSource = BuildingSource.ExternalAPI;
        _speciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfoFromExternalSources())
            {
                _loggerUtility?.Logger.Information($"Pokemon basic information successfully retrieved from external sources for pokemon named: {pokemonName}");
                return Ok(_speciesParser.Model);
            }
            _loggerUtility?.Logger.Error($"It wasn't possible to retrieve basic information from external sources for pokemon named: {pokemonName}");
            return BadRequest();
        }
        catch (Exception exception)
        {
            _loggerUtility?.Logger.Fatal($"A fatal error happened when trying to retrieve basic information " +
                $"from external sources for pokemon named: {pokemonName}. Exception: {exception}");
            return BadRequest(exception);
        }
    }

    /// <summary>
    /// API entrypoint that takes the name of a pokemon and retrieves the basic information with description translated for it, 
    // which includes its name, habitat, description translated and legendary condition.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon for which the information is being requested.</param>
    /// <returns>An IActionResult instance that in case of positive result contains the model of the pokemon requested.</returns>
    [HttpGet("translated/{pokemonName}")]
    public async Task<IActionResult> GetPokemonBasicInformationWithTranslation(string pokemonName)
    {
        _loggerUtility?.Logger.Information($"GET pokemon basic information with translation received for Pokemon with name {pokemonName}");
        _speciesParser.BuildingSource = BuildingSource.ExternalAPI;
        _speciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfoWithTranslationFromExternalSources())
            {
                _loggerUtility?.Logger.Information($"Pokemon basic information with translation successfully retrieved from external sources for pokemon named: {pokemonName}");
                return Ok(_speciesParser.Model);
            }
            _loggerUtility?.Logger.Error($"It wasn't possible to retrieve basic information with translation from external sources for pokemon named: {pokemonName}");
            return BadRequest();
        }
        catch (Exception exception)
        {
            _loggerUtility?.Logger.Fatal($"A fatal error happened when trying to retrieve basic information with translation" +
                $"from external sources for pokemon named: {pokemonName}. Exception: {exception}");
            return BadRequest(exception);
        }
    }
    
    /// <summary>
    /// Retrieves a PokemonSpeciesModel with its basic information from external sources.
    /// </summary>
    /// <returns>True if the PokemonSpeciesModel was successfully retrieved, False otherwise</returns>
    private async Task<bool> RetrieveBasicInfoFromExternalSources()
    {
        _loggerUtility?.Logger.Information($"Making request for basic pokemon information from external sources for pokemon named: {_speciesParser.Model}");
        HttpResponseMessage response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{_speciesParser.SpeciesName}");
        if (response.IsSuccessStatusCode)
        {
            return _speciesParser.ParseBasicInformationFromStream(response.Content.ReadAsStream());
        }
        return false;
    }

    /// <summary>
    /// Retrieves a PokemonSpeciesModel with its basic information and translation if possible from external sources.
    /// </summary>
    /// <returns>True if the PokemonSpeciesModel was successfully retrieved, False otherwise</returns>
    private async Task<bool> RetrieveBasicInfoWithTranslationFromExternalSources()
    {
        _loggerUtility?.Logger.Information($"Making request for basic pokemon information for translation from external sources for pokemon named: {_speciesParser.Model}");
        if (!await RetrieveBasicInfoFromExternalSources())
        {
            return false;
        }
        string endpoint;
        var payload = new {text = _speciesParser.Model.Description};
        if (_speciesParser.Model.Habitat == "cave" || (_speciesParser.Model.IsLegendary ?? false))
        {
            endpoint = "https://api.funtranslations.com/translate/yoda.json";
        }
        else
        {
            endpoint = "https://api.funtranslations.com/translate/shakespeare.json";
        }
        _loggerUtility?.Logger.Information($"Making request for description translation from external sources for pokemon named: {_speciesParser.Model}");
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, payload);
        if (response.IsSuccessStatusCode)
        {
            return _speciesParser.ParseTranslationFromStream(response.Content.ReadAsStream());
        }
        return false;
    }
}