using Botwyn.Modules.Custom;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Botwyn.Objects;
using System.Net;
using Discord;

namespace Botwyn.Modules
{
    public class Command_Affixes : CustomModule
    {
        [Command("affix"), Summary("Displays this weeks M+ Affixes."), Name("Affix")]
        public async Task Affixes()
        {
            var dataUrl = "https://raider.io/api/v1/mythic-plus/affixes?region=eu&locale=en";
            var rawData = new WebClient().DownloadString(dataUrl);
            var jsonData = JsonConvert.DeserializeObject<Affixes>(rawData);
            var details = new StringBuilder();
            var embed = new EmbedBuilder().
                WithTitle($"Region: {jsonData.Region.ToUpper()} - {jsonData.Title}")
                .WithThumbnailUrl("https://media.forgecdn.net/avatars/117/23/636399071197048271.png")
                .WithFooter("Powered by Derps & Raider.IO", "https://media.forgecdn.net/avatars/117/23/636399071197048271.png")
                .WithTimestamp(DateTime.UtcNow)
                .WithColor(Color.DarkRed);

            foreach (var affix in jsonData.AffixDetails)
            {
                details.Append($"**[{affix.Name}]({affix.WowheadUrl})**\n{affix.Description}\n\n");
            }
            embed.WithDescription(details.ToString());

            await ReplyAsync("", false, embed.Build());
        }
    }
}
