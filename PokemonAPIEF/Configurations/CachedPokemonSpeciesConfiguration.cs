using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonAPIEF.Models;

namespace PokemonAPIEF.Configurations;

/// <summary>
/// Configuration class for the CachedPokemonSpecies model.
/// </summary>
public class CachedPokemonSpeciesConfiguration : IEntityTypeConfiguration<CachedPokemonSpecies>
{
    /// <summary>
    /// Configures the CachedPokemonSpecies model.
    /// </summary>
    /// <param name="builder">Entity type builder.</param>
    public void Configure(EntityTypeBuilder<CachedPokemonSpecies> builder)
    {
        builder.HasKey(cachedPokemonSpeciesModel => cachedPokemonSpeciesModel.SpeciesName);

        builder.Property(cachedPokemonSpeciesModel => cachedPokemonSpeciesModel.Description)
            .IsUnicode();

        builder.Property(cachedPokemonSpeciesModel => cachedPokemonSpeciesModel.IsLegendary)
            .HasColumnType("bit");

        builder.Property(cachedPokemonSpeciesModel => cachedPokemonSpeciesModel.TranslatedDescription)
            .IsUnicode();

        builder.Property(cachedPokemonSpeciesModel => cachedPokemonSpeciesModel.HabitatName)
            .IsUnicode();

        builder.Property(cachedPokemonSpeciesModel => cachedPokemonSpeciesModel.LastCached)
            .HasColumnType("datetime")
            .IsRequired();
    }
}