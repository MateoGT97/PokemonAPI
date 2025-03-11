using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonAPIBusinessLayer.Models;
using PokemonAPIBusinessLayer.PokemonRepository;
using PokemonAPIBusinessLayer.SpeciesParser;
using PokemonAPI.Controllers.Implementations;
using PokemonAPIBusinessLayer.GetPokemonInformationLogic;

namespace PokemonAPITests;

public class PokemonInformationControllerTests
{
    private IPokemonSpeciesParser _parser = new PokemonSpeciesParser();
    [Theory]
    [InlineData("mewtwo")]
    public async Task GetsBasicPokemonInformationRealPokemon(string pokemonName)
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(mock => mock.CreateClient("")).Returns(new HttpClient());
        var cachedPokemonInformationRepository = new PokemonCacheRepository(new PokemonAPIEF.PokemonAPIEFContext());
        var getPokemonInformationLogic = new GetPokemonInformationLogic(null, cachedPokemonInformationRepository, _parser);
        var pokemonInformationController = new PokemonInformationController(null, httpClientFactoryMock.Object, getPokemonInformationLogic);

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
        var cachedPokemonInformationRepository = new PokemonCacheRepository(new PokemonAPIEF.PokemonAPIEFContext());
        var getPokemonInformationLogic = new GetPokemonInformationLogic(null, cachedPokemonInformationRepository, _parser);
        var pokemonInformationController = new PokemonInformationController(null, httpClientFactoryMock.Object, getPokemonInformationLogic);

        // Act
        var result = await pokemonInformationController.GetPokemonBasicInformation(pokemonName);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("mewtwo")]
    public async Task GetsBasicTranslationInformationRealPokemon(string pokemonName)
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(mock => mock.CreateClient("")).Returns(new HttpClient());
        var cachedPokemonInformationRepository = new PokemonCacheRepository(new PokemonAPIEF.PokemonAPIEFContext());
        var getPokemonInformationLogic = new GetPokemonInformationLogic(null, cachedPokemonInformationRepository, _parser);
        var pokemonInformationController = new PokemonInformationController(null, httpClientFactoryMock.Object, getPokemonInformationLogic);

        // Act
        var result = await pokemonInformationController.GetPokemonBasicInformationWithTranslation(pokemonName);

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
    public async Task GetsBasictranslationInformationFakePokemon(string pokemonName)
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(mock => mock.CreateClient("")).Returns(new HttpClient());
        var cachedPokemonInformationRepository = new PokemonCacheRepository(new PokemonAPIEF.PokemonAPIEFContext());
        var getPokemonInformationLogic = new GetPokemonInformationLogic(null, cachedPokemonInformationRepository, _parser);
        var pokemonInformationController = new PokemonInformationController(null, httpClientFactoryMock.Object, getPokemonInformationLogic);

        // Act
        var result = await pokemonInformationController.GetPokemonBasicInformationWithTranslation(pokemonName);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<BadRequestResult>(result);
    }
}