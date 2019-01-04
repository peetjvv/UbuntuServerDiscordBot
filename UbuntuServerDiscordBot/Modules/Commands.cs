using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UbuntuServerDiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public async Task info()
        {
            var a = new EmbedBuilder();
            a.WithTitle(Context.Client.CurrentUser.Username);
            a.WithDescription("Hi there, I'm a bot designed to show the system info of the PC that I'm running on. I'm written in .Net Core 2.1 and by using the Discord.Net 2.0.0 library.");
            await Context.Message.Channel.SendMessageAsync("", embed: a.Build());
        }

        [Command("help")]
        public async Task help()
        {
            var a = new EmbedBuilder();
            a.WithTitle($"Available commands");
            a.WithDescription($"" +
                $"`{Program.Configuration["prefix"]}info` - Who am I?\n" +
                $"`{Program.Configuration["prefix"]}help` - What can I do? (this message)\n" +
                $"`{Program.Configuration["prefix"]}backuphome` - Backup my Home dir\n" +
                $"`{Program.Configuration["prefix"]}ping` - Am I awake? What's my IP?\n" +
                $"`{Program.Configuration["prefix"]}status` - How is my HP looking?");
            await Context.Message.Channel.SendMessageAsync("", embed: a.Build());
        }

        [Command("backuphome")]
        public async Task backupHome()
        {

        }

        [Command("ping")]
        public async Task ping()
        {
            var message = new StringBuilder("Hi there, ");
            var ipv4 = string.Empty;
            var ipv6 = string.Empty;

            try
            {
                ipv4 = CommandHelpers.GetIPv4Address();
                message.Append($"my IPv4 address is `{ipv4}`");
            }
            catch (Exception ex)
            {
                message.Append("I don't know what my IPv4 address is");
            }

            try
            {
                ipv6 = CommandHelpers.GetIPv6Address();
                if (string.IsNullOrWhiteSpace(ipv4))
                {
                    message.Append(", but");
                }
                else
                {
                    message.Append(" and");
                }
                message.Append($" my IPv6 address is `{ipv6}`");
            }
            catch (Exception ex)
            {
                message.Append(" and I don't know what my IPv6 address is");
                if (string.IsNullOrWhiteSpace(ipv4))
                {
                    message.Append(" either");
                }
            }

            var a = new EmbedBuilder();
            a.WithDescription(message.ToString());
            await Context.Message.Channel.SendMessageAsync("", embed: a.Build());
        }

        [Command("status")]
        public async Task status()
        {

        }
    }
}
