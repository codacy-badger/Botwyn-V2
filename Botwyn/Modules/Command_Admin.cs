using Botwyn.Handlers;
using Botwyn.Modules.Custom;
using Botwyn.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Botwyn.Precondtions;
using Botwyn.Data;

namespace Botwyn.Modules
{
    public class Command_Admin : CustomModule
    {
        [Command("Report"), Name("Report"), Summary("used to report a user if they've broken a rule.")]
        public async Task Reporter([Summary("The user who you're reporting. (@ them)")]SocketUser reportedUser, [Summary("The reason you're reporting them.")][Remainder]string reason)
        {
            ISocketMessageChannel channel = (ISocketMessageChannel)Context.Guild.GetChannel(514112072326578207);
            var requestingUser = (SocketGuildUser)Context.User;
            if (UtilService.IsOfficer(requestingUser.Roles))
                UserAccounts.AccountUpdate(reportedUser, "1" ,UserAccounts.UpdateType.AdminReport);
            else UserAccounts.AccountUpdate(reportedUser, "1", UserAccounts.UpdateType.UserReport);
            UserAccounts.AccountUpdate(Context.User, "1", UserAccounts.UpdateType.ReportMade);
            await channel.SendMessageAsync("", false, UtilService.Report(reportedUser, Context.User, (SocketChannel)Context.Channel, reason));
        }

        [Command("Kick", RunMode = RunMode.Async), Name("Kick"), Summary("Kicks a user (Requires at-least Recruitment Officer role).")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick([Summary("The user you want to kick. (@ them)")]SocketUser kUser, [Summary("The Reason you're kicking them.")][Remainder]string reason)
        {
            var requestingUser = (SocketGuildUser)Context.User;
            var kickedUser = (IGuildUser)kUser;
            var reportChannel = (ISocketMessageChannel)Context.Guild.GetChannel(514112072326578207);
            if (UtilService.IsOfficer(requestingUser.Roles))
            {
                await kickedUser.KickAsync(reason);
                await ReplyAsync($"{kickedUser.Username} with ID {kUser.Id} has been kicked from {Context.Guild.Name}");
                await reportChannel.SendMessageAsync("", false, UtilService.KickReport(kUser, Context.User, (SocketChannel)Context.Channel, reason));
                LoggingService.Log("admin", LogSeverity.Info, $"{Context.User.Username} has kicked {kickedUser.Username}");
                return;
            }
            else
            {
                await ReplyAsync("You do not have the required role to do that.");
                LoggingService.Log("admin", LogSeverity.Warning, $"{Context.User.Username} Requested a command without permision.");
            }
        }

        [Command("Ban", RunMode = RunMode.Async), Name("Ban"), Summary("Bans a user from the server, requires at-least Recruitment Officer Rank.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban([Summary("The user you want to ban. (@ them)")]SocketUser bUser, [Summary("The reason you're banning them.")][Remainder]string reason)
        {
            var requestingUser = (SocketGuildUser)Context.User;
            var bannedUser = (IGuildUser)bUser;
            var reportChannel = (ISocketMessageChannel)Context.Guild.GetChannel(514112072326578207);
            if (UtilService.IsOfficer(requestingUser.Roles))
            {
                await bannedUser.BanAsync(2, reason);
                await ReplyAsync($"{bannedUser.Username} with ID {bUser.Id} has been banned from {Context.Guild.Name}");
                await reportChannel.SendMessageAsync("", false, UtilService.BanReporter(bUser, Context.User, (SocketChannel)Context.Channel, reason));
                LoggingService.Log("admin", LogSeverity.Info, $"{Context.User.Username} has banned {bannedUser.Username}");
                return;
            }
            else
            {
                await ReplyAsync("You do not have the required role to do that.");
                LoggingService.Log("admin", LogSeverity.Warning, $"{Context.User.Username} Requested a command without permision.");
            }
        }

        [Command("Unban", RunMode = RunMode.Async), Name("Unban"), Summary("Unbans a user (if they're banned), requires at least Officer Rank.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Unban([Summary("The User you want to unban. (@ them)")]SocketUser bUser, [Summary("The reason you are unbanning them.")][Remainder]string reason)
        {
            await ReplyAsync("Not implemented yet.");
        }

        [Command("Purgeall", RunMode = RunMode.Async), Name("Purgeall"), Summary("Removes ``X`` amount of messages from the channel the command is used in. (Default is 50)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge([Summary("The ammount of messages you want to purge.")]int amount = 50)
            => await (Context.Message.Channel as SocketTextChannel).DeleteMessagesAsync(await Context.Message.Channel.GetMessagesAsync(amount + 1).FlattenAsync());

        [Command("Purgeuser"), Name("Purgeuser"), Summary("Removes ``x`` amount of a user's last messages. (Default amount to purge is 100)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Clear([Summary("The user who's messages you want to purge from the channel. (@ them)")]SocketGuildUser user, [Summary("The amount you want to purge.")]int amountOfMessagesToDelete = 100)
        {
            if (user == Context.User)
                amountOfMessagesToDelete++; //Because it will count the purge command as a message

            var messages = await Context.Message.Channel.GetMessagesAsync(amountOfMessagesToDelete).FlattenAsync();

            var result = messages.Where(x => x.Author.Id == user.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));

            await (Context.Message.Channel as SocketTextChannel).DeleteMessagesAsync(result);

        }

        [Command("Game"), Alias("ChangeGame", "SetGame"), Name("SetGame"), Summary("Change what the bot is currently playing."), Cooldown(10, true)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SetGame([Summary("What you want the bot to display as playing.")][Remainder] string gamename)
        {
            await Context.Client.SetGameAsync(gamename);
            await ReplyAsync($"Changed game to `{gamename}`");
        }

        [Command("Announce"), Name("Announce"), Summary("Make A Announcement")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Announce([Summary("The announcement text you want to display.")][Remainder]string announcement)
        {
            var embed = EmbedHandler.CreateEmbed("Announcement By " + Context.Message.Author, announcement, EmbedHandler.EmbedMessageType.Info, true);

            await Context.Channel.SendMessageAsync("", false, embed);
            await Context.Message.DeleteAsync();
        }
    }
}
