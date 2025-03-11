using PokemonAPIEF.Models;

namespace PokemonAPIBusinessLayer.PokemonRepository;
/// <summary>
/// Interface for the PokemonCacheRepository describing the methods that the repository must implement.
/// </summary>
public interface IPokemonCacheRepository
{
    /// <summary>
    /// Adds a new cached Pokemon species to the cache or updates an existing one.
    /// </summary>
    /// <param name="species">CachedPokemonSpecies to add.</param>
    public void AddOrUpdateCachedPokemonSpecies(CachedPokemonSpecies species);

    /// <summary>
    /// Get a cached Pokemon by its name.
    /// </summary>
    /// <param name="speciesName">Pokemon species name</param>
    /// <returns>CachedPokemonEspecies, null if not found.</returns>
    public CachedPokemonSpecies? GetCachedPokemonSpeciesByName(string speciesName);

    /// <summary>
    /// Remove a cached Pokemon species from the cache by its name.
    /// </summary>
    /// <param name="speciesName">Name of the Pokemon species to remove.</param>
    public void RemoveCachedPokemonSpeciesByName(string speciesName);
}