using Microsoft.AspNetCore.Mvc;

namespace PokemonAPI.Controllers.Interfaces;

/// <summary>
/// Interface that describes a method to get Pokemon basic information.
/// </summary>
public interface IBasicPokemonInformation
{
    /// <summary>
    /// API entrypoint that takes the name of a pokemon and retrieves the basic information for it, 
    // which includes its name, habitat, description and legendary condition.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon for which the information is being requested.</param>
    /// <returns>An IActionResult instance that in case of positive result contains the model of the pokemon requested.</returns>
    public Task<IActionResult> GetPokemonBasicInformation(string pokemonName);
}