using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Controllers.Interfaces;
using PokemonAPIBusinessLayer.GetPokemonInformationLogic;
using PokemonAPIBusinessLayer.Logging;
using PokemonAPIBusinessLayer.Models;
using PokemonAPIBusinessLayer.SpeciesParser;

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
    private readonly IGetPokemonInformationLogic _getPokemonInformationLogic;

    public PokemonInformationController(ILoggerUtility? loggerUtility, IHttpClientFactory httpClientFactory,
        IGetPokemonInformationLogic getPokemonInformationLogic)
    {
        _loggerUtility = loggerUtility;
        _httpClient = httpClientFactory.CreateClient();
        _getPokemonInformationLogic = getPokemonInformationLogic;

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
        PokemonSpeciesModel? model = _getPokemonInformationLogic.TryGetPokemonBasicInformationFromCache(pokemonName);
        if (model != null)
        {
            return Ok(model);
        }
        model = await _getPokemonInformationLogic.TryGetPokemonBasicInformationFromExternalSources(pokemonName, _httpClient);
        if (model != null)
        {
            return Ok(model);
        }
        return BadRequest();
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
        PokemonSpeciesModel? model = _getPokemonInformationLogic.TryGetPokemonInformationTranslatedFromCache(pokemonName);
        if (model != null)
        {
            return Ok(model);
        }
        model = await _getPokemonInformationLogic.TryGetPokemonInformationTranslatedFromExternalSources(pokemonName, _httpClient);
        if (model != null)
        {
            return Ok(model);
        }
        return BadRequest();
    }
}