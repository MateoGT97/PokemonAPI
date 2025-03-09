namespace PokemonAPITests;

public class PokemonInformationControllerTests
{
    [Theory]
    [InlineData("mewtwo")]
    public async Task GetsBasicPokemonInformationRealPokemon(string pokemonName)
    {
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData("MEWTWO")]
    public async Task GetsBasicPokemonInformationFakePokemon(string pokemonName)
    {
        throw new NotImplementedException();
    }
}