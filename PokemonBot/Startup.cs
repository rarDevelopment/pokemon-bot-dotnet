global using Discord;
global using Discord.Interactions;
global using Discord.WebSocket;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
using DiscordDotNetUtilities;
using DiscordDotNetUtilities.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonBot;
using PokemonBot.Models;
using Serilog;
using System.Reflection;
using PokemonBot.BusinessLayer;
using PokemonBot.ServiceLayer;

var builder = new HostBuilder();

builder.ConfigureAppConfiguration(options
    => options.AddJsonFile("appsettings.json")
        .AddUserSecrets(Assembly.GetEntryAssembly(), true)
        .AddEnvironmentVariables())
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.AddEnvironmentVariables(prefix: "DOTNET_");
    });

var loggerConfig = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File($"logs/log-{DateTime.Now:dd.MM.yy_HH.mm}.log")
    .CreateLogger();

builder.ConfigureServices((host, services) =>
{
    services.AddLogging(options => options.AddSerilog(loggerConfig, dispose: true));
    services.AddSingleton(new DiscordSocketClient(
        new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All,
            FormatUsersInBidirectionalUnicode = false,
            AlwaysDownloadUsers = true,
            LogGatewayIntentWarnings = false
        }));

    var discordSettings = new DiscordSettings(host.Configuration["Discord:BotToken"]!);
    var versionSettings = new VersionSettings(host.Configuration["Version:VersionNumber"]!);
    var botSettings = new BotSettings(host.Configuration["Bot:MissingnoImageUrl"]!, host.Configuration["Bot:GhostUrl"]!,
    host.Configuration["Bot:HelpImage"]!, Convert.ToInt32(host.Configuration["Bot:TotalPokemon"]!));
    var pokemonTcgSettings = new PokemonTcgSettings(host.Configuration["PokemonTcg:ApiKey"]!, Convert.ToInt32(host.Configuration["PokemonTcg:CardLimit"]!));

    services.AddSingleton(discordSettings);
    services.AddSingleton(versionSettings);
    services.AddSingleton(botSettings);
    services.AddSingleton(pokemonTcgSettings);

    services.AddScoped<IDiscordFormatter, DiscordFormatter>();
    services.AddScoped<IPokemonBusinessLayer, PokemonBusinessLayer>();
    services.AddScoped<IPokeApiServiceLayer, PokeApiServiceLayer>();
    services.AddScoped<IPokemonTcgBusinessLayer, PokemonTcgBusinessLayer>();
    services.AddScoped<IPokemonTcgServiceLayer, PokemonTcgServiceLayer>();

    services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));

    services.AddSingleton<InteractionHandler>();

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DiscordBot).GetTypeInfo().Assembly));

    services.AddHostedService<DiscordBot>();
});

var app = builder.Build();

await app.RunAsync();
