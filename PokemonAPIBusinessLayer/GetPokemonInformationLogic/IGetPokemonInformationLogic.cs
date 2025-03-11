using PokemonAPIBusinessLayer.Models;

namespace PokemonAPIBusinessLayer.GetPokemonInformationLogic
{
    /// <summary>
    /// Interface for the GetPokemonInformationLogic describing the methods that the logic must implement.
    /// </summary>
    public interface IGetPokemonInformationLogic
    {
        public PokemonSpeciesModel? TryGetPokemonBasicInformationFromCache(string pokemonName);
        public Task<PokemonSpeciesModel?> TryGetPokemonBasicInformationFromExternalSources(string pokemonName, HttpClient httpClient);
        public PokemonSpeciesModel? TryGetPokemonInformationTranslatedFromCache(string pokemonName);
        Task<PokemonSpeciesModel?> TryGetPokemonInformationTranslatedFromExternalSources(string pokemonName, HttpClient httpClient);
    }
}