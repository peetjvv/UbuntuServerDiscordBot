using Discord;
using Discord.Commands;
using Discord.Webhook;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UbuntuServerDiscordBot.DiscordBot.Services;

namespace UbuntuServerDiscordBot.DiscordBot
{
    public class Bot: IDisposable
    {
        private DiscordSocketClient _client;
        private IConfiguration _config;
        private ILogger _logger;

        public Bot(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            var services = ConfigureServices();
            services.GetRequiredService<LogService>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);

            await _client.LoginAsync(TokenType.Bot, _config["bot:token"]);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                // Logging
                .AddLogging()
                .AddSingleton<LogService>()
                // Extra
                .AddSingleton(_config)
                // Add additional services here...
                // Build
                .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            switch (log.Severity)
            {
                case LogSeverity.Critical:
                    _logger.LogCritical(log.Exception, log.Message);
                    break;
                case LogSeverity.Error:
                    _logger.LogError(log.Exception, log.Message);
                    break;
                case LogSeverity.Warning:
                    _logger.LogWarning(log.Exception, log.Message);
                    break;
                case LogSeverity.Info:
                    _logger.LogInformation(log.Exception, log.Message);
                    break;
                case LogSeverity.Verbose:
                    _logger.LogTrace(log.Exception, log.Message);
                    break;
                case LogSeverity.Debug:
                    _logger.LogDebug(log.Exception, log.Message);
                    break;
                default:
                    throw new Exception("Unknown log severity for log: " + log);
            }
            return Task.CompletedTask;
        }
        
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is {_client.LoginState}!");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            var logoutTask = _client.LogoutAsync();
            logoutTask.Wait();
            _client.Dispose();
        }
    }
}
