#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PokemonBot/PokemonBot.csproj", "PokemonBot/"]
COPY ["PokemonBot.Models/PokemonBot.Models.csproj", "PokemonBot.Models/"]
RUN dotnet restore "PokemonBot/PokemonBot.csproj"
COPY . .
WORKDIR "/src/PokemonBot"
RUN dotnet build "PokemonBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokemonBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokemonBot.dll"]
