using System.Text;
using PokemonAPIBusinessLayer.Models;
using PokemonAPIBusinessLayer.SpeciesParser;
using PokemonAPIEF.Models;

namespace PokemonAPITests;

public class PokemonSpeciesParserTests
{
    private string _theoryFilesInfo = "./TheoryFiles";
    [Theory]
    [InlineData("mewtwo","ParseBasicRealInfoMewTwo.json")]
    public void ParseBasicRealInformationFromExternalStream(string pokemonName, string realInformationJsonPath)
    {
        // Arrange
        var realInfoStream = File.OpenRead($"{_theoryFilesInfo}/{realInformationJsonPath}");
        var realInfoSpeciesParser = new PokemonSpeciesParser(){ SpeciesName = pokemonName};

        var fakeInfoStream = new MemoryStream(Encoding.UTF8.GetBytes("Oops!"));
        var fakeInfoSpeciesParser = new PokemonSpeciesParser(){ SpeciesName = pokemonName};

        // Act
        var resultRealStream = realInfoSpeciesParser.ParseBasicInformationFromStream(realInfoStream);
        var resultFakeStream = fakeInfoSpeciesParser.ParseBasicInformationFromStream(fakeInfoStream);

        // Assert
        Assert.False(resultFakeStream);
        Assert.True(resultRealStream);

        Assert.IsAssignableFrom<CachedPokemonSpecies>(fakeInfoSpeciesParser.Model);
        Assert.IsAssignableFrom<CachedPokemonSpecies>(realInfoSpeciesParser.Model);

        Assert.NotNull(realInfoSpeciesParser.Model?.SpeciesName);
        Assert.NotNull(realInfoSpeciesParser.Model?.Description);
        Assert.NotNull(realInfoSpeciesParser.Model?.HabitatName);
        Assert.NotNull(realInfoSpeciesParser.Model?.IsLegendary);

        Assert.Null(fakeInfoSpeciesParser.Model?.SpeciesName);
        Assert.Null(fakeInfoSpeciesParser.Model?.Description);
        Assert.Null(fakeInfoSpeciesParser.Model?.HabitatName);
        Assert.Null(fakeInfoSpeciesParser.Model?.IsLegendary);
    }

    [Theory]
    [InlineData("TranslatedMewTwo.json")]
    public void ParseTranslatedRealInformationFromExternalStream(string translatedRealInformationJsonPath)
    {
        // Arrange
        var realTranslationStream = File.OpenRead($"{_theoryFilesInfo}/{translatedRealInformationJsonPath}");
        var realInfoSpeciesParser = new PokemonSpeciesParser();
        realInfoSpeciesParser.Model.TranslatedDescription = "A Description";

        var fakeInfoStream = new MemoryStream(Encoding.UTF8.GetBytes("Oops!"));
        var fakeInfoSpeciesParser = new PokemonSpeciesParser();
        fakeInfoSpeciesParser.Model.TranslatedDescription = "A Description";

        // Act
        var resultRealStream = realInfoSpeciesParser.ParseTranslationFromStream(realTranslationStream);
        var resultFakeStream = fakeInfoSpeciesParser.ParseTranslationFromStream(fakeInfoStream);

        // Assert
        Assert.False(resultFakeStream);
        Assert.True(resultRealStream);

        Assert.IsAssignableFrom<CachedPokemonSpecies>(fakeInfoSpeciesParser.Model);
        Assert.IsAssignableFrom<CachedPokemonSpecies>(realInfoSpeciesParser.Model);

        Assert.NotEqual(realInfoSpeciesParser.Model.TranslatedDescription, fakeInfoSpeciesParser.Model.TranslatedDescription);
    }
}