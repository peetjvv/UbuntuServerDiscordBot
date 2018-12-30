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
        [Command("ping")]
        public async Task ping()
        {
            var a = new EmbedBuilder();
            a.WithDescription($"I'm currently chilling at: {CommandHelpers.GetIPv4Address()}");
            await Context.Message.Channel.SendMessageAsync("", embed: a);
        }
    }
}
