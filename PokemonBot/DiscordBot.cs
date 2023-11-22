using Microsoft.Extensions.Hosting;
using PokemonBot.Models;

namespace PokemonBot;

public class DiscordBot(DiscordSocketClient client,
        InteractionService interactions,
        ILogger<DiscordBot> logger,
        InteractionHandler interactionHandler,
        DiscordSettings discordSettings)
    : BackgroundService
{
    private readonly ILogger _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        client.Ready += ClientReady;

        client.Log += LogAsync;
        interactions.Log += LogAsync;

        await interactionHandler.InitializeAsync();

        await client.LoginAsync(TokenType.Bot, discordSettings.BotToken);

        await client.StartAsync();
    }

    private async Task ClientReady()
    {
        _logger.LogInformation($"Logged as {client.CurrentUser}");

        await interactions.RegisterCommandsGloballyAsync();
    }

    public async Task LogAsync(LogMessage msg)
    {
        var severity = msg.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };

        _logger.Log(severity, msg.Exception, msg.Message);

        await Task.CompletedTask;
    }
}