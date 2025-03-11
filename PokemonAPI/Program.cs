using Microsoft.EntityFrameworkCore;
using PokemonAPI.Policies;
using PokemonAPIBusinessLayer.GetPokemonInformationLogic;
using PokemonAPIBusinessLayer.Logging;
using PokemonAPIBusinessLayer.PokemonRepository;
using PokemonAPIBusinessLayer.SpeciesParser;
using PokemonAPIEF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PokemonAPIEFContext>(options => options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddSingleton<ILoggerUtility, LoggerUtility>();
builder.Services.AddTransient<IPokemonSpeciesParser, PokemonSpeciesParser>();
builder.Services.AddTransient<IGetPokemonInformationLogic, GetPokemonInformationLogic>();
builder.Services.AddTransient<IPokemonCacheRepository, PokemonCacheRepository>();
builder.Services.AddHttpClient("PokemonHTTPCLient")
    .SetHandlerLifetime(TimeSpan.FromSeconds(300))
    .AddPolicyHandler(HttpCLientPolicies.DefineRetryPolicy()) //retrying policy
    .AddPolicyHandler(HttpCLientPolicies.DefineCircuitBreakerPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();