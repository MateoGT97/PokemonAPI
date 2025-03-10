using PokemonAPI.BusinessLogic.Implementations;
using PokemonAPI.BusinessLogic.Interfaces;
using PokemonAPI.Logging;
using PokemonAPI.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPokemonSpeciesParser, PokemonSpeciesParser>();
builder.Services.AddSingleton<ILoggerUtility, LoggerUtility>();
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