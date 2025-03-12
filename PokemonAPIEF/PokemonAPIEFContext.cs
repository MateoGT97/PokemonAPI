using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokemonAPIEF.Configurations;
using PokemonAPIEF.Models;

namespace PokemonAPIEF;

/// <summary>
/// Pokemon API EF context.
/// This class is responsible for managing the connection to the database and the entities.
/// </summary>
public class PokemonAPIEFContext : DbContext, IPokemonAPIEFContext
{
    public DbConnection Connection { get; internal set; }

    public virtual DbSet<CachedPokemonSpecies> CachedPokemonSpecies { get; set; } = null!;

    public PokemonAPIEFContext(string connectionString = "PokemonAPIEFContext")
    {
        string appSettingsRelativePath = "appsettings.json";
        # if DEBUG
        appSettingsRelativePath = "appsettings.Development.json";
        #endif
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile(appSettingsRelativePath,false).Build();

        Connection = new SqliteConnection($"Data Source = {AppDomain.CurrentDomain.BaseDirectory}{configuration.GetConnectionString(connectionString)}");
    }

    /// <summary>
    /// Configures the context.
    /// </summary>
    /// <param name="optionsBuilder">DB Context options vuilder instance</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(Connection);
        }
    }

    /// <summary>
    /// Configures the model.
    /// </summary>
    /// <param name="modelBuilder">Model Builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CachedPokemonSpeciesConfiguration());
    }
}