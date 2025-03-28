FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5252
EXPOSE 7106

ENV ASPNETCORE_URLS=http://+:5252

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
COPY . .
RUN dotnet tool install --local --create-manifest-if-needed dotnet-ef
RUN dotnet tool restore
RUN dotnet ef database update --project PokemonAPIEF/PokemonAPIEF.csproj

FROM build AS publish
RUN dotnet publish "PokemonAPI/PokemonAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokemonAPI.dll"]
