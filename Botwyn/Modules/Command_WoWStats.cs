using System;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Botwyn.Services;
using System.IO;
using Botwyn.Modules.Custom;
using System.Net;
using Botwyn.Objects;

namespace Botwyn.Modules
{
    public class Command_WoWStats : CustomModule
    {
        [Command("stats", RunMode = RunMode.Async)]
        [Summary("(WIP) Retrieves Character Stats For a World Of Warcraft Player")]
        [Name("stats")]
        public async Task WowStats([Summary("The name of character you want to find stats for.")]string character, [Summary("The server the character you're searching for is on. (Default is Draenor)")]string server = "draenor")
        {
            var dataUrl = $"https://raider.io/api/v1/characters/profile?region=eu&realm={server}&name={character}&fields=gear%2Cguild%2Craid_progression%2Cmythic_plus_scores";
            var rawData = new WebClient().DownloadString(dataUrl);
            var player = JsonConvert.DeserializeObject<MythicPlus>(rawData);
            var embed = new EmbedBuilder();
            var raiderioURL = $"https://raider.io/characters/eu/{server}/{character}";
            var armoryURL = $"https://worldofwarcraft.com/en-gb/character/{server}/{character}/";
            var wowanalyzeURL = $"https://www.wowanalyzer.com/character/EU/{server}/{character}/";

            //Title
            embed.Title = $"{player.name} {player.realm} EU | Character Info";

            //Description
            embed.Description = $"**Name & Guild**: {player.name} | {player.guild.name}\n" +
                $"**Links**: [Raider.IO]({raiderioURL}) | [Armory]({armoryURL}) | [WowAnalzyer]({wowanalyzeURL})\n" +
                $"**Class**: {player.race}, {player.active_spec_name} {player._class}\n" +
                $"**Item Level**: Equipped: {player.gear.item_level_equipped} | Overall: {player.gear.item_level_total}\n" +
                $"**Raid Progression (Uldir)**: {player.raid_progression.uldir.summary}\n" +
                $"**Mythic+**: {player.mythic_plus_scores.all}";

            //thumbnail
            embed.ThumbnailUrl = $"{player.thumbnail_url}";

            //color
            if (player.faction.ToLower() == "horde") { embed.Color = Color.DarkRed; } else { embed.Color = Color.DarkBlue; }

            await ReplyAsync("", false, embed.Build());
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            LoggingService.LogInformation("command", $"{Context.User.Username} used {command.Name}");
        }
    }
}

