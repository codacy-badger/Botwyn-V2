using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Botwyn.Modules.Custom;

namespace Botwyn.Modules
{
    public class Command_WowAnalyze : CustomModule
    {
        [Command("analyze")]
        [Summary("Displays a link to WoW Analyser For the specified WoW Champion.")]
        [Name("Analyze")]
        public async Task AnalyzeMe([Summary("The Name of the Champion you want a link for.")]string championName)
        {
            var url = $"https://www.wowanalyzer.com/character/EU/Draenor/{championName}/";
            var embed = new EmbedBuilder()
                .WithTitle("WoW Analyzer Link Generator")
                .WithDescription($"Here is a link to your WoW Analazer Page. {System.Environment.NewLine}Please note that only public logs can be seen by WoW Analyser.{System.Environment.NewLine}[**Click Here to Load {championName}'s WoW Analyzer Profile**]({url})")
                .WithCurrentTimestamp()
                .WithColor(Color.DarkMagenta)
                .WithFooter("Powered By Bleps & WoW Analyzer")
                .WithThumbnailUrl("https://wowanalyzer.com/favicon.png")
                .Build();


            await Context.Channel.SendMessageAsync("", false, embed);

        }
    }
}
