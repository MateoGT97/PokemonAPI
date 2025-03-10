using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonAPI.BusinessLogic.Implementations;
using PokemonAPI.BusinessLogic.Interfaces;
using PokemonAPI.Controllers;
using PokemonAPI.Models;

namespace PokemonAPITests;

public class PokemonInformationControllerTests
{
    IPokemonSpeciesParser _parser = new PokemonSpeciesParser();
    [Theory]
    [InlineData("mewtwo")]
    public async Task GetsBasicPokemonInformationRealPokemon(string pokemonName)
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(mock => mock.CreateClient("")).Returns(new HttpClient());
        var pokemonInformationController = new PokemonInformationController(null, httpClientFactoryMock.Object, _parser);

        // Act
        var result = await pokemonInformationController.GetPokemonBasicInformation(pokemonName);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IActionResult>(result);

        Assert.NotNull((result as ObjectResult)?.Value);
        Assert.IsAssignableFrom<PokemonSpeciesModel>((result as ObjectResult)?.Value);
        Assert.NotNull(((result as ObjectResult)?.Value as PokemonSpeciesModel)?.Name);
        Assert.NotNull(((result as ObjectResult)?.Value as PokemonSpeciesModel)?.Description);
        Assert.NotNull(((result as ObjectResult)?.Value as PokemonSpeciesModel)?.Habitat);
        Assert.NotNull(((result as ObjectResult)?.Value as PokemonSpeciesModel)?.IsLegendary);
    }

    [Theory]
    [InlineData("MEWTWO")]
    public async Task GetsBasicPokemonInformationFakePokemon(string pokemonName)
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(mock => mock.CreateClient("")).Returns(new HttpClient());
        var pokemonInformationController = new PokemonInformationController(null, httpClientFactoryMock.Object, _parser);

        // Act
        var result = await pokemonInformationController.GetPokemonBasicInformation(pokemonName);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<BadRequestResult>(result);
    }
}