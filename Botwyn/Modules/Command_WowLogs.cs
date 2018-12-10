using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using Botwyn.Modules.Custom;
using Botwyn.Services;
using Discord.WebSocket;
using System.Linq;

namespace Botwyn.Modules
{
    public class Command_WowLogs : CustomModule
    {
        [Command("wclog", RunMode = RunMode.Async)]
        [Summary("Posts Warcraft Logs into #Logs.")]
        [Name("wclog")]
        public async Task LogPoster([Summary("The Warcraftlog url of the fight you want to add to the #logs channel.")]string url)
        {
            var user = (SocketGuildUser)Context.User;
            if (!UtilService.IsOfficer(user.Roles) || !UtilService.IsRaider(user.Roles))
            { await ReplyAsync("You do not have the required role for this command."); return; }

            var channel = Context.Guild.GetTextChannel(504316725500575745);

            if (!url.Contains("reports/"))
            {
                await ReplyAsync("That isn't a valid warcraftlogs url.");
            }
            else
            {
                var ID = url.Substring(url.LastIndexOf("reports/") + 8);
                var wowAnalyzerUrl = $"https://www.wowanalyzer.com/report/{ID}";
                var wipefestUrl = $"https://www.wipefest.net/report/{ID}";
                var embed = new EmbedBuilder()
                    .WithColor(Color.DarkTeal)
                    .WithTitle("BFA Guild Raid LOG")
                    .WithThumbnailUrl("https://dmszsuqyoe6y6.cloudfront.net/img/common/warcraft-logo.png")
                    .WithDescription($"Added: {DateTime.UtcNow}\n\n**Links:** [Warcraft Log]({url}) | [Wow Analyzer]({wowAnalyzerUrl}) | [WipeFest]({wipefestUrl})\n")
                    .WithFooter("Powered by Bleps, WarcraftLogs, Wow Analyzer & WipeFest", "https://dmszsuqyoe6y6.cloudfront.net/img/common/warcraft-logo.png")
                    .Build();

                await channel.SendMessageAsync("", false, embed);
            }


        }
    }
}
