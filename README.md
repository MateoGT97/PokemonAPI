# Pokemon API

This small .NET API allows you to retrieve basic information for a requested Pokemon Species.

The Api exposes two entry points:

1. **GET /pokemon/{pokemonName}**
which retrieves information about the pokemon Description, Habitat Name and legendary status.

2. **GET /pokemon/translated/{pokemonName}**
which retrieves information about the pokemon Habitat Name, legendary status and a description where if the translated description was possible to retrieve, it contains this, if not, then the basic description is returned.

***Note:*** The pokemon names of the external APIs used are case sensitive. 

## How to run the API (assuming you want to debug):
1. Clone the repository.
2. Download the dotnet 8.0, you can get it from the following site https://dotnet.microsoft.com/en-us/download/dotnet/8.0.
3. Assuming you want to debug the API, make a copy of the appsettings.json rename the copy to appsettings.Development.json.
4. Install the ef tools by running `dotnet tool install --global dotnet-ef` if you want to install them locally instead run `dotnet tool install --local --create-manifest-if-needed dotnet-ef`.
5. Create db by applying migrations: Run `dotnet ef database update --project PokemonAPIEF/PokemonAPIEF.csproj`.
6. Run the API, you can run it by going to the root folder and executing `dotnet run --project PokemonAPI/PokemonAPI.csproj`

## How to run the API with Docker
Either run the docker-compose files included on the repo or build the image with `docker build -t <ImageName> -f <Dockerfile or Dockerfile.Development> .` and then run it with `docker run -p 5252:5252 --name <ImageName> pokemon-api`

## Things to consider for a production scenario.
1. A largue test battery with more fine granulated tests can be implemented.
2. Many of the possible configurations are hard-coded but can be modified to read from a configuration file like appsettings.json E.g: the retry and short circuit policies setup, or logs path/directory structure.
3. Logs are currently created on a json file on a diretory tree depending on the time and date, this logs however currently do not have a mechanism to be archived on a database and or be removed, potentially leading to storage problems, this can be solved by archiving logs on a db and deleting after a condition is met.
4. Policies can be set to impose a rate limit the ammount of requests made to the controller preventing DoS attacks, this can be made using the AddRateLimiter when building the app services.