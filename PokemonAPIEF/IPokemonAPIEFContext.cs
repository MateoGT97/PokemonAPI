using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PokemonAPIEF.Models;

namespace PokemonAPIEF;
/// <summary>
/// Interface for the Pokemon API EF context.
/// </summary>
public interface IPokemonAPIEFContext
{
    public DbSet<CachedPokemonSpecies> CachedPokemonSpecies { get; set; }
    public DbConnection Connection { get; }
}