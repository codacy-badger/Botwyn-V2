using Botwyn.Data;
using Botwyn.Modules.Custom;
using Discord.Commands;
using System.Threading.Tasks;
using Botwyn.Handlers;
using Discord;
using System.Text;
using Discord.WebSocket;

namespace Botwyn.Modules
{
    [Group("Account")]
    public class Command_UserAccount : CustomModule
    {
        [Command(RunMode = RunMode.Async), Name("Account"), Summary("Display your Discord and World Of Warcraft Infomation.")]
        public async Task Account()
            => await Account(Context.User);

        [Command(RunMode = RunMode.Async), Name("Account"), Summary("Display your Discord and World Of Warcraft Infomation.")]
        public async Task Account(SocketUser user)
        {
            var account = UserAccounts.GetAccount(user);
            var descripitionBuilder = new StringBuilder();
            var socketGuildUser = (SocketGuildUser)user;
            descripitionBuilder.Append
                ($"__**Discord Info**__\n" +
                $"**Name**: {user.Username}\n" +
                $"**Joined At**: {socketGuildUser.JoinedAt}\n" +
                $"**Current Status**: {socketGuildUser.Status}\n" +
                $"**Current Nickname**: {socketGuildUser.Nickname}\n" +
                $"\n**__Guild Info__**\n" +
                $"**Guild Rank**: {account.GuildRank}\n" +
                $"**Returning For Next Tier**: {account.ReturningForNextRaid}\n" +
                $"**Player Is Trial Raider**: {account.IsTrial}\n" +
                $"**Wow Main**: {account.MainChar}\n" +
                $"**Main Spec**: {account.MainSpec}\n" +
                $"**Main Alt**: {account.WowAlt}\n " +
                $"**Alt's Spec**: {account.WowAltSpec}\n" +
                $"\n__**Report Info**__\n" +
                $"**Created Reports**: {account.ReportMade}\n" +
                $"**User Reports**: {account.OwnReports}\n");

            if (account.AdminReports == 0) descripitionBuilder.Append($"They currently have no Admin reports against them.\n");
            else descripitionBuilder.Append($"**Admin Reports**: {account.AdminReports}\n");

            if (account.RaidsMissedWithReason == 0 && account.RaidsMissedNoReason == 0) descripitionBuilder.Append($"They've Currently not missed any raids.\n");
            if (account.RaidsMissedWithReason > 0) descripitionBuilder.Append($"**Raids Missed With Reason**: {account.RaidsMissedWithReason}\n");
            if (account.RaidsMissedNoReason > 0 && account.RaidsMissedNoReason < 4) descripitionBuilder.Append($"**Raids Missed No Reason**: {account.RaidsMissedNoReason}\n");
            if (account.RaidsMissedNoReason > 4) descripitionBuilder.Append($"\n🛑__**{user.Username.ToUpper()} HAS NOW MISSED {account.RaidsMissedNoReason} RAIDS WITH NO REASON**__🛑");

            var description = descripitionBuilder.ToString();
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username} Guild Account Details")
                .WithCurrentTimestamp()
                .WithColor(Color.DarkTeal)
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithDescription(description);

            await ReplyAsync("", false, embed.Build());
        }

        [Group("Update")]
        public class Command_UpdateUserAccount : CustomModule
        {
            #region Main
            [Command("Main"), Name("Account Update Main"), Summary("Allows you to update your World of Warcraft Main.")]
            public async Task UpdateMain([Summary("The name of your main on World of Warcraft")][Remainder]string main)
                => await UpdateMain(Context.User, main);

            [Command("Main"), Name("Account Update Main"), Summary("Allows you to update your World of Warcraft Main.")]
            public async Task UpdateMain(SocketUser user, [Summary("The name of your main on World of Warcraft")][Remainder]string main)
            {
                UserAccounts.AccountUpdate(user, main, UserAccounts.UpdateType.WowMain);
                await ReplyAsync("", false, EmbedHandler.CreateEmbed("Account Updated", $"**The Main Has Been Set To: __{main}__**", EmbedHandler.EmbedMessageType.Success, true));
            }
            #endregion

            #region GuildRank
            [Command("GuildRank"), Name("Account Update GuildRank"), Summary("Allows you to update your World of Warcraft Guild Rank.")]
            public async Task UpdateWowGuildRank([Summary("The current rank you have in World of Warcraft")][Remainder]string rank)
                => await UpdateWowGuildRank(Context.User, rank);

            [Command("GuildRank"), Name("Account Update GuildRank"), Summary("Allows you to update your World of Warcraft Guild Rank.")]
            public async Task UpdateWowGuildRank(SocketUser user, [Summary("The current rank you have in World of Warcraft")][Remainder]string rank)
            {
                UserAccounts.AccountUpdate(user, rank, UserAccounts.UpdateType.GuildRank);
                await ReplyAsync("", false, EmbedHandler.CreateEmbed("Account Updated", $"**The Guild Rank Has Been Set To: __{rank}__**", EmbedHandler.EmbedMessageType.Success, true));
            }
            #endregion

            #region WoWAlt
            [Command("Alt"), Name("Account Update Alt"), Summary("Allows you to update your World of Warcraft Alt.")]
            public async Task UpdateWowAlt([Summary("The current alt you have in World of Warcraft")][Remainder]string alt)
                => await UpdateWowAlt(Context.User, alt);

            [Command("Alt"), Name("Account Update Alt"), Summary("Allows you to update your World of Warcraft Alt.")]
            public async Task UpdateWowAlt(SocketUser user, [Summary("The current alt you have in World of Warcraft")][Remainder]string alt)
            {
                UserAccounts.AccountUpdate(user, alt, UserAccounts.UpdateType.WowAlt);
                await ReplyAsync("", false, EmbedHandler.CreateEmbed("Account Updated", $"**The WoW Alt Has Been Set To: __{alt}__**", EmbedHandler.EmbedMessageType.Success, true));
            }

            #endregion

            #region Spec
            [Command("Spec"), Name("Account Update Spec"), Summary("Allows you to update your World of Warcraft Spec.")]
            public async Task UpdateWowSpec([Summary("The current spec you are playing in World of Warcraft")][Remainder]string spec)
                => await UpdateWowSpec(Context.User, spec);

            [Command("Spec"), Name("Account Update Spec"), Summary("Allows you to update your World of Warcraft Spec.")]
            public async Task UpdateWowSpec(SocketUser user, [Summary("The current spec you are playing in World of Warcraft")][Remainder]string spec)
            {
                if (spec.ToLower() != "tank" || spec.ToLower() != "dps" || spec.ToLower() != "healer")
                {
                    await ReplyAsync("Sorry the accepted specs are: ``Tank``, ``DPS``, ``Healer``");
                }
                else
                {
                    switch (spec.ToLower())
                    {
                        case "dps":
                            spec = "DPS";
                            break;
                        case "tank":
                            spec = "Tank";
                            break;
                        case "healer":
                            spec = "Healer";
                            break;
                        default:
                            break;
                    }
                    UserAccounts.AccountUpdate(user, spec, UserAccounts.UpdateType.WowMainSpec);
                    await ReplyAsync("", false, EmbedHandler.CreateEmbed("Account Updated", $"**The Current Main Spec Has Been Set To: __{spec}__**", EmbedHandler.EmbedMessageType.Success, true));
                }
                
            }
            #endregion

            #region AltSpec
            [Command("AltSpec"), Name("Account Update AltSpec"), Summary("Allows you to update your World of Warcraft Alt's Spec.")]
            public async Task UpdateWowAltSpec([Summary("The current alt you have in World of Warcraft")][Remainder]string spec)
                => await UpdateWowAltSpec(Context.User, spec);

            [Command("AltSpec"), Name("Account Update AltSpec"), Summary("Allows you to update your World of Warcraft Alt's Spec.")]
            public async Task UpdateWowAltSpec(SocketUser user, [Summary("The current alt you have in World of Warcraft")][Remainder]string spec)
            {
                UserAccounts.AccountUpdate(user, spec, UserAccounts.UpdateType.WowAltSpec);
                await ReplyAsync("", false, EmbedHandler.CreateEmbed("Account Updated", $"**The WoW Alt's Spec Has Been Set To: __{spec}__**", EmbedHandler.EmbedMessageType.Success, true));
            }
            #endregion

            #region Returning For Next Raid
            [Command("Returning"), Name("Account Update Returning"), Summary("Allows you to update your World of Warcraft Returning Status.")]
            public async Task UpdateReturningForNextRaid([Summary("The current alt you have in World of Warcraft")][Remainder]string returning)
                    => await UpdateReturningForNextRaid(Context.User, returning);

            [Command("Returning"), Name("Account Update Returning"), Summary("Allows you to update your World of Warcraft Returning Status.")]
            public async Task UpdateReturningForNextRaid(SocketUser user, [Summary("The current alt you have in World of Warcraft")][Remainder]string returning)
            {
                UserAccounts.AccountUpdate(user, returning, UserAccounts.UpdateType.ReturningForNextRaid);
                await ReplyAsync("", false, EmbedHandler.CreateEmbed("Account Updated", $"**The Returning Status Spec Has Been Set To: __{returning}__**", EmbedHandler.EmbedMessageType.Success, true));
            }
            #endregion
        }

        [Command("Returning")]
        public async Task ReturningMembers()
        {
            var descriptionBuilder = new StringBuilder();
            var returningPlayers = UserAccounts.GetReturningMemebers();
            var returningDPS = UserAccounts.GetSpec(UserAccounts.SpecType.DPS);
            var returningHealers = UserAccounts.GetSpec(UserAccounts.SpecType.Healer);
            var returningTanks = UserAccounts.GetSpec(UserAccounts.SpecType.Tank);
            var trials = UserAccounts.GetTrials();

            foreach (var player in returningPlayers)
            {
                descriptionBuilder.Append($"``{player.MainChar}`` ");
            }
            await ReplyAsync("", false, EmbedHandler.CreateEmbed(
                "Returning Players For Next Tier", $"{descriptionBuilder.ToString()} " +
                $"\n\n We currently have **__{returningPlayers.Count}/20__** Players." +
                $"\n Current Setup: **__{returningTanks.Count}/{returningHealers.Count}/{returningDPS.Count}__**\n\n" +
                $"Tanks Needed: {2 - returningTanks.Count}\n" +
                $"Healers Needed: {4 - returningHealers.Count}\n" +
                $"DPS Required: {14 - returningDPS.Count}\n\n" +
                $"We Current Have {trials.Count} Trial Raiders.", EmbedHandler.EmbedMessageType.Info, true));
        }
    }
}
