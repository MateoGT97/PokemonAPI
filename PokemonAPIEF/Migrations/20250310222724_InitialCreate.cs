using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonAPIEF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CachedPokemonSpecies",
                columns: table => new
                {
                    SpeciesName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    HabitatName = table.Column<string>(type: "TEXT", nullable: true),
                    IsLegendary = table.Column<bool>(type: "bit", nullable: true),
                    LastCached = table.Column<DateTime>(type: "datetime", nullable: false),
                    TranslatedDescription = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedPokemonSpecies", x => x.SpeciesName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CachedPokemonSpecies");
        }
    }
}
