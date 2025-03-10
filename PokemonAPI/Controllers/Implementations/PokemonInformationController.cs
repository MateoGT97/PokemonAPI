using Microsoft.AspNetCore.Mvc;
using PokemonAPI.BusinessLogic.Interfaces;
using PokemonAPI.BusinessLogic.Enums;
using PokemonAPI.Models;

namespace PokemonAPI.Controllers;

[ApiController]
[Route("Pokemon")]
public class PokemonInformationController(ILogger<PokemonInformationController>? logger, IHttpClientFactory httpClientFactory,
    IPokemonSpeciesParser pokemonSpeciesParser) : ControllerBase
{
    private readonly ILogger<PokemonInformationController>? _logger = logger;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    private readonly IPokemonSpeciesParser _speciesParser = pokemonSpeciesParser;

    [HttpGet("{pokemonName}")]
    public async Task<IActionResult> GetPokemonBasicInformation(string pokemonName)
    {
        _logger?.LogInformation($"Get pokemon basic information request received for Pokemon with name {pokemonName}");
        _speciesParser.BuildingSource = BuildingSource.ExternalAPI;
        _speciesParser.SpeciesName = pokemonName;
        try
        {
            if (await RetrieveBasicInfo())
            {
                return Ok(_speciesParser.Model);
            }
            return BadRequest();
        }
        catch (Exception exception)
        {
            return BadRequest(exception);
        }
    }
    
    private async Task<bool> RetrieveBasicInfo()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{_speciesParser.SpeciesName}");
        if (response.IsSuccessStatusCode)
        {
            return _speciesParser.ParseBasicInformationFromStream(response.Content.ReadAsStream());
        }
        return false;
    }

}