using PokemonAPIEF;
using PokemonAPIEF.Models;

namespace PokemonAPIBusinessLayer.PokemonRepository;
public class PokemonCacheRepository(PokemonAPIEFContext pokemonAPIEFContext) : IPokemonCacheRepository
{
    public PokemonAPIEFContext PokemonAPIEFContext { get; set; } = pokemonAPIEFContext;

    /// <summary>
    /// Adds a new cached Pokemon species to the cache or updates an existing one.
    /// </summary>
    /// <param name="pokemonSpecies">CachedPokemonSpecies to add.</param>
    public void AddOrUpdateCachedPokemonSpecies(CachedPokemonSpecies pokemonSpecies)
    {
        PokemonAPIEFContext.ChangeTracker.Clear();
        CachedPokemonSpecies? existingPokemonSpecies = GetCachedPokemonSpeciesByName(pokemonSpecies.SpeciesName);
        if(existingPokemonSpecies != null)
        {
            PokemonAPIEFContext.ChangeTracker.Clear();
            PokemonAPIEFContext.CachedPokemonSpecies.Update(pokemonSpecies);
        }
        else
        {
            PokemonAPIEFContext.ChangeTracker.Clear();
            PokemonAPIEFContext.CachedPokemonSpecies.Add(pokemonSpecies);
        }
        PokemonAPIEFContext.SaveChanges();
    }

    /// <summary>
    /// Get a cached Pokemon by its name.
    /// </summary>
    /// <param name="speciesName">Pokemon species name</param>
    /// <returns>CachedPokemonEspecies, null if not found.</returns>
    public CachedPokemonSpecies? GetCachedPokemonSpeciesByName(string speciesName)
    {
        PokemonAPIEFContext.ChangeTracker.Clear();
        return PokemonAPIEFContext.CachedPokemonSpecies.Find(speciesName);
    }

    /// <summary>
    /// Remove a cached Pokemon species from the cache by its name.
    /// </summary>
    /// <param name="speciesName">Name of the Pokemon species to remove.</param>
    public void RemoveCachedPokemonSpeciesByName(string speciesName)
    {
        PokemonAPIEFContext.ChangeTracker.Clear();
        CachedPokemonSpecies? pokemonSpecies = GetCachedPokemonSpeciesByName(speciesName);
        if(pokemonSpecies != null)
        {
            PokemonAPIEFContext.CachedPokemonSpecies.Remove(pokemonSpecies);
            PokemonAPIEFContext.SaveChanges();
        }
    }
}