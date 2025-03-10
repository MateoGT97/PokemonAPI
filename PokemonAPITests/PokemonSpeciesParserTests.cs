using System.Text;
using PokemonAPI.BusinessLogic.Enums;
using PokemonAPI.BusinessLogic.Implementations;
using PokemonAPI.Models;

namespace PokemonAPITests;

public class PokemonSpeciesParserTests
{
    string _theoryFilesInfo = "./TheoryFiles";
    [Theory]
    [InlineData("mewtwo","ParseBasicRealInfoMewTwo.json")]
    public void ParseBasicRealInformationFromExternalStream(string pokemonName, string realInformationJsonPath)
    {
        // Arrange
        var realInfoStream = File.OpenRead($"{_theoryFilesInfo}/{realInformationJsonPath}");
        var realInfoSpeciesParser = new PokemonSpeciesParser(){ SpeciesName = pokemonName,BuildingSource = BuildingSource.ExternalAPI};

        var fakeInfoStream = new MemoryStream(Encoding.UTF8.GetBytes("Oops!"));
        var fakeInfoSpeciesParser = new PokemonSpeciesParser(){ SpeciesName = pokemonName,BuildingSource = BuildingSource.ExternalAPI};

        // Act
        var resultRealStream = realInfoSpeciesParser.ParseBasicInformationFromStream(realInfoStream);
        var resultFakeStream = fakeInfoSpeciesParser.ParseBasicInformationFromStream(fakeInfoStream);

        // Assert
        Assert.False(resultFakeStream);
        Assert.True(resultRealStream);

        Assert.IsAssignableFrom<PokemonSpeciesModel>(fakeInfoSpeciesParser.Model);
        Assert.IsAssignableFrom<PokemonSpeciesModel>(realInfoSpeciesParser.Model);

        Assert.NotNull(realInfoSpeciesParser.Model?.Name);
        Assert.NotNull(realInfoSpeciesParser.Model?.Description);
        Assert.NotNull(realInfoSpeciesParser.Model?.Habitat);
        Assert.NotNull(realInfoSpeciesParser.Model?.IsLegendary);

        Assert.Null(fakeInfoSpeciesParser.Model?.Name);
        Assert.Null(fakeInfoSpeciesParser.Model?.Description);
        Assert.Null(fakeInfoSpeciesParser.Model?.Habitat);
        Assert.Null(fakeInfoSpeciesParser.Model?.IsLegendary);
    }
}