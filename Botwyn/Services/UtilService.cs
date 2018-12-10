using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using Botwyn.Objects;

namespace Botwyn.Services
{
    public static class UtilService
    {
        public static Embed Report(SocketUser rUser, SocketUser sUser, SocketChannel channel, string reason)
        {
            var reportEmbed = new EmbedBuilder()
                .WithTitle("Report")
                .WithCurrentTimestamp()
                .WithColor(Color.DarkRed)
                .WithThumbnailUrl(rUser.GetAvatarUrl())
                .AddField("Reported User", $"{rUser.Username} with ID {rUser.Id}")
                .AddField("Reported By", $"{sUser.Username} with ID {sUser.Id}")
                .AddField("Reason", $"{reason}")
                .AddField("Channel", $"{channel}")
                .AddField("Time", DateTime.UtcNow);

            return reportEmbed.Build();
        }

        public static bool IsOfficer(IReadOnlyCollection<SocketRole> role)
        {
            if (role.Contains(role.FirstOrDefault(x => x.Name == "Recruitment Officer")))
                return true;
            return false;
        }

        public static bool IsRaider(IReadOnlyCollection<SocketRole> role)
        {
            if (role.Contains(role.FirstOrDefault(x => x.Name == "Main Raider")))
                return true;
            return false;
        }

        public static bool IsChanBlacklisted(ISocketMessageChannel channel)
        {
            ulong raidbotsChannel = 504318315091722270;
            if (channel.Id == raidbotsChannel)
                return true;
            return false;
        }

        public static Embed KickReport(SocketUser kUser, SocketUser sUser, SocketChannel channel, string reason)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Kicked User")
                .WithCurrentTimestamp()
                .WithColor(Color.DarkRed)
                .WithThumbnailUrl(kUser.GetAvatarUrl())
                .WithDescription($"{sUser.Username} has kicked {kUser.Username} {Environment.NewLine}For Reason: {reason}");

            return embed.Build();
        }

        public static Embed BanReporter(SocketUser bUser, SocketUser sUser, SocketChannel channel, string reason)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Banned User")
                .WithCurrentTimestamp()
                .WithColor(Color.DarkRed)
                .WithThumbnailUrl(bUser.GetAvatarUrl())
                .WithDescription($"{sUser.Username} has banned {bUser.Username} {Environment.NewLine}For Reason: {reason}");

            return embed.Build();
        }

        public static Embed FetchRandImage(string type)
        {
            var embed = new EmbedBuilder();
            string url = String.Empty;
            string jsonData = String.Empty;

            switch (type.ToLower())
            {
                case "cat":
                    url = "http://aws.random.cat/meow";
                    jsonData = new WebClient().DownloadString(url);
                    RandomCat randCat = JsonConvert.DeserializeObject<RandomCat>(jsonData);
                    return embed.WithTitle("Cat").WithImageUrl(randCat.ImageUrl).Build();
                case "dog":
                    url = "https://dog.ceo/api/breeds/image/random";
                    jsonData = new WebClient().DownloadString(url);
                    RandomDog randDog = JsonConvert.DeserializeObject<RandomDog>(jsonData);
                    return embed.WithTitle("Dog").WithImageUrl(randDog.ImageUrl).Build();
                case "meme":
                    url = "https://api-to.get-a.life/meme";
                    jsonData = new WebClient().DownloadString(url);
                    RandomMeme randMeme = JsonConvert.DeserializeObject<RandomMeme>(jsonData);
                    return embed.WithTitle("Meme").WithDescription(randMeme.Text).WithImageUrl(randMeme.ImageUrl).Build();
                default:
                    return embed.WithTitle("ERROR").Build();
            }
        }
    }
}
