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
using UbuntuServerDiscordBot.Services;

namespace UbuntuServerDiscordBot
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
            var clientConfig = new DiscordSocketConfig();
            clientConfig.ConnectionTimeout = int.Parse(_config["timeout"]);
            _client = new DiscordSocketClient(clientConfig);

            var services = ConfigureServices();
            services.GetRequiredService<LogService>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);

            _client.Ready += Client_Ready;

            await _client.LoginAsync(TokenType.Bot, _config["botToken"]);
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

        public async Task Client_Ready()
        {
            var ipv4 = string.Empty;
            var ipv6 = string.Empty;

            try
            {
                ipv4 = CommandHelpers.GetIPv4Address();
            }
            catch (Exception ex)
            {
                ipv4 = "IPv4 address not available";
            }

            try
            {
                ipv6 = CommandHelpers.GetIPv6Address();
            }
            catch (Exception ex)
            {
                ipv6 = "IPv6 address not available";
            }

            var a = new EmbedBuilder();
            a.WithDescription($"Hi, I just started up and can be found at `{ipv4}` or `{ipv6}`");
            a.WithTimestamp(DateTime.UtcNow);
            await ((ISocketMessageChannel)_client.GetChannel(ulong.Parse(_config["defaultChannel"]))).SendMessageAsync("", embed: a.Build());
        }

        private Task LoginStateAsync()
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
