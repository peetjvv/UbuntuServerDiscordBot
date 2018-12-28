using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace UbuntuServerDiscordBot.DiscordBot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private IServiceProvider _provider;

        private RequestOptions _reqOptions;

        public CommandHandlingService(IServiceProvider provider, DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
            _provider = provider;

            _reqOptions = RequestOptions.Default;
            _reqOptions.Timeout = 30000;

            _client.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            // Add additional initialization code here...
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // ignore system messages and messages from bots (this bot or other ones)
            if (!(rawMessage is SocketUserMessage message) || message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;

            // ignore messages not mentioning this bot
            if (!message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if (result.Error.HasValue)
            {
                if (result.Error.Value == CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync($"Unknown Command, type '{"help"}' for a list commands");
                }
                else
                {
                    await context.Channel.SendMessageAsync($"Error: '{result.ErrorReason}'");
                }
            }
        }
    }
}
