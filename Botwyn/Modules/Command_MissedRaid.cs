using Botwyn.Handlers;
using Botwyn.Modules.Custom;
using Botwyn.Services;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Botwyn.Modules
{
    public class Command_MissedRaid : CustomModule
    {
        [Command("MissedRaid"), Name("MissedRaid"), Summary("Used to log if a user has missed a raid.")]
        public async Task MissedRaid(SocketUser user, [Remainder]string reason = "")
        {
            ISocketMessageChannel channel = (ISocketMessageChannel)Context.Guild.GetChannel(514112072326578207);
            if (String.IsNullOrEmpty(reason))
            {
                UserAccounts.AccountUpdate(user, "1", UserAccounts.UpdateType.MissedRaidNoReason);
                await channel.SendMessageAsync("", false, UtilService.Report(Context.User, user, (SocketChannel)Context.Channel, $"**Missed Raid**\n\n**Reason Given**: **__NONE__**"));
                await ReplyAsync($"Account Updated for {user.Username}");
            }
            else
            {
                UserAccounts.AccountUpdate(user, "1", UserAccounts.UpdateType.MissedRaidWithReason);
                await channel.SendMessageAsync("", false, UtilService.Report(Context.User, user, (SocketChannel)Context.Channel, $"**Missed Raid**\n\n**Reason Given**: {reason}"));
                await ReplyAsync($"Account Updated for {user.Username}");
            }
        }
    }
}
