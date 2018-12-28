using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UbuntuServerDiscordBot.DiscordBot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("awake")]
        public async Task awake()
        {
            var a = new EmbedBuilder();
            a.WithDescription($"Hi, I just returned from hibernation and can be found here: {CommandHelpers.GetIPv4Address()}");
            a.WithTimestamp(DateTime.UtcNow);
            await Context.Message.Channel.SendMessageAsync("", embed: a);
        }

        [Command("ping")]
        public async Task ping()
        {
            var a = new EmbedBuilder();
            a.WithDescription($"Hi, I'm currently chilling at: {CommandHelpers.GetIPv4Address()}");
            await Context.Message.Channel.SendMessageAsync("", embed: a);
        }
    }
}
