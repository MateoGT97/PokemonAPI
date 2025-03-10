using Microsoft.AspNetCore.Mvc;

namespace PokemonAPI.Controllers.Interfaces;

/// <summary>
/// Interface that describes a method to get Pokemon information with translated description.
/// </summary>
public interface ITranslatePokemonDescription
{
    /// <summary>
    /// API entrypoint that takes the name of a pokemon and retrieves the basic information with description translated for it, 
    // which includes its name, habitat, description translated and legendary condition.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon for which the information is being requested.</param>
    /// <returns>An IActionResult instance that in case of positive result contains the model of the pokemon requested.</returns>
    public Task<IActionResult> GetPokemonBasicInformationWithTranslation(string pokemonName);
}