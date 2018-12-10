using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Botwyn.Objects;
using Discord;
using Discord.WebSocket;
using Victoria;
using Victoria.Entities.Enums;
using Victoria.Entities;

namespace Botwyn.Services
{
    public sealed class AudioService : BaseService
    {
        private readonly Lazy<Random> _lazyRandom
            = new Lazy<Random>();

        private readonly Lazy<ConcurrentDictionary<ulong, AudioOptions>> _lazyOptions
            = new Lazy<ConcurrentDictionary<ulong, AudioOptions>>();

        private Random Random
            => _lazyRandom.Value;

        private ConcurrentDictionary<ulong, AudioOptions> Options
            => _lazyOptions.Value;

        public async Task<string> JoinAsync(SocketGuildUser user, IMessageChannel textChannel)
        {
            if (user.VoiceChannel == null)
                return "You must be connected to a voice channel.";

            if (Options.TryGetValue(user.Guild.Id, out var options) && options.Summoner.Id != user.Id)
                return $"I can't join another voice channel till {options.Summoner} disconnects me.";

            await Lavalink.DefaultNode.ConnectAsync(user.VoiceChannel, textChannel);
            Options.TryAdd(user.Guild.Id, new AudioOptions
            {
                Summoner = user,
                Voters = new HashSet<ulong>()
            });
            return $"Now connected to {user.VoiceChannel.Name} and bound to {textChannel.Name}. Get Ready For Betrays...";
        }

        public async Task<string> LeaveAsync(ulong guildId)
        {
            try
            {
                var player = Lavalink.DefaultNode.GetPlayer(guildId);
                if (player.IsPlaying)
                    await player.StopAsync();
                var name = player.VoiceChannel.Name;
                await Lavalink.DefaultNode.DisconnectAsync(guildId);
                return $"I've left {name}. Thank you for playing moosik.";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> PlayAsync(ulong guildId, string query = null)
        {
            try
            {
                var player = Lavalink.DefaultNode.GetPlayer(guildId);
                LavaTrack track;

                if (string.IsNullOrWhiteSpace(query))
                {
                    if (player.Queue.Count < 1)
                        return "Queue is empty. Please queue something first.";
                    track = player.Queue.Dequeue();
                }
                else
                {
                    var search = await Lavalink.DefaultNode.SearchYouTubeAsync(query);

                    if (search.LoadResultType == LoadResultType.NoMatches)
                        return $"BAMBOOZLED! I wasn't able to find anything for {query}.";

                    track = search.Tracks.FirstOrDefault();
                }

                if (player.IsPlaying && !(player.CurrentTrack is null))
                {
                    player.Queue.Enqueue(track);
                    return $"{track.Title} has been added to queue.";
                }

                await player.PlayAsync(track);
                return $"**Now Playing:** {track.Title} https://media.giphy.com/media/T7ukTzXQVmWqI/giphy.gif";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        public async Task<Embed> List(ulong guildId)
        {
            var embed = new EmbedBuilder();
            var player = Lavalink.DefaultNode.GetPlayer(guildId);
            if (player.Queue.Count < 1 && player.CurrentTrack == null)
            {
                await player.TextChannel.SendMessageAsync("Sorry there doesn't seem to be anything in the Queue.");
                return null;
            }
            else
            {
                StringBuilder list = new StringBuilder();
                string nowPlaying;
                if (player.Queue.Count < 1 && player.CurrentTrack != null)
                {
                    embed.Title = $"Now Playing: {player.CurrentTrack.Title}";
                    embed.Description = "Nothing else queued.";
                    return embed.Build();
                }
                else
                {
                    nowPlaying = ($"Now Playing: {player.CurrentTrack.Title}");
                    var trackNumber = 1;
                    foreach (var track in player.Queue.Items)
                    {
                        list.Append($"**{trackNumber}**: {track.Title}{Environment.NewLine}");
                        trackNumber++;
                    }
                    embed.Title = "Musik Queue";
                    embed.AddField(nowPlaying, $"**Queue**{Environment.NewLine}{list.ToString()}");
                    embed.Color = Color.DarkMagenta;
                    trackNumber = 1;
                }
            }


            return embed.Build();
        }

        public Embed Status(ulong guildId)
        {
            var player = Lavalink.DefaultNode.GetPlayer(guildId);
            if (player.CurrentTrack == null)
            {
                var errorEmbed = new EmbedBuilder { Title = "Error", Description = "There is either nothing playing or another error has occured."};
                return errorEmbed.Build();
            }
            var embed = new EmbedBuilder {
                Title = $"Now Playing: {player.CurrentTrack.Title}",
                Description = $"Length: {player.CurrentTrack.Position}{Environment.NewLine}[Link]({player.CurrentTrack.Uri})",
                Color = Color.DarkMagenta
            };
            return embed.Build();
        }

        public Embed Help()
        {
            var embed = new EmbedBuilder
            {
                Title = "Music Bot Commands",
                Description = $"Below is a list of command that work with Botwyn for music. Only use in the #music-spam channel.{Environment.NewLine}" +
                $"**Please Note! You have to make sure the bot isn't actively in another channel before usage. If you use !Join while it's another channel, it will not work.**",
                Color = Color.DarkMagenta
            };

            embed.AddField($"!Join", $"Please ensure you use this command first to have the bot join your voice channel.", true);
            embed.AddField($"!Leave", $"Use this once you have done listening and want the bot to leave the voice channel.", true);
            embed.AddField($"!Play", $"**Usage:** ``!play song name`` or ``!play youtube url``", true);
            embed.AddField($"!Pause", $"Pauses the currently playing song.", true);
            embed.AddField($"!Resume", $"Resumes the song that had been paused.", true);
            embed.AddField($"!Skip", $"This will skip the song. ``Currently has a vote feature (At-least 60% of the active users in the voice channel have to !skip).``", true);
            embed.AddField($"!list", $"Lists all songs that are in the queue.", true);
            embed.AddField($"!Volume", $"**Usage:** ``!volume 0-149``", true);
            embed.AddField($"!Stop", $"Stop music playback and clears the queue.", true);

            return embed.Build();
        }

        public async Task<string> SkipAsync(ulong guildId, SocketGuildUser user)
        {
            try
            {
                var player = Lavalink.DefaultNode.GetPlayer(guildId);
                Options.TryGetValue(guildId, out var options);

                if (player.Queue.Count < 1)
                    return "This is the last song, nothing to skip.";

                if (options.Voters.Contains(user.Id))
                    return "You've already voted. Please don't vote again.";

                options.VotedTrack = player.Queue.Peek();
                options.Voters.Add(user.Id);
                var perc = options.Voters.Count / user.VoiceChannel.Users.Count(x => !x.IsBot) * 100;

                if (perc < 60)
                    return "We definitely need more votes to skip this song.";

                var track = player.CurrentTrack;
                await player.SkipAsync();
                options.VotedTrack = null;
                options.Voters.Clear();

                return $"**Skipped:** {track.Title} {Environment.NewLine} **Now Playing:** {player.CurrentTrack.Title}.";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> Pause(ulong guildId)
        {
            try
            {
                var player = Lavalink.DefaultNode.GetPlayer(guildId);
                await player.PauseAsync();
                return $"**Paused:** {player.CurrentTrack.Title}, what a bamboozle.";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> Resume(ulong guildId)
        {
            try
            {
                var player = Lavalink.DefaultNode.GetPlayer(guildId);
                await player.ResumeAsync();
                return $"**Resumed:** {player.CurrentTrack.Title}";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> Volume(ulong guildId, int volume)
        {
            if (volume >= 150 || volume <= 0)
            {
                return $"Volume must be between 0 and 150.";
            }
            try
            {
                var player = Lavalink.DefaultNode.GetPlayer(guildId);
                await player.SetVolumeAsync(volume);
                return $"Volume has been set to {volume}.";
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> StopAsync(ulong guildId)
        {
            var player = Lavalink.DefaultNode.GetPlayer(guildId);
            if (player.IsPlaying)
            {
                await player.StopAsync();
                player.Queue.Clear();
                return "Moosik has stopped and the queue has been Mlemed.";
            }
            else return "Nothing is playing... Derp.";
        }

        private LavaTrack RandomTrack(ulong guildId)
        {
            var player = Lavalink.DefaultNode.GetPlayer(guildId);
            return player.Queue.Items.ToList()[Random.Next(player.Queue.Count)];
        }



        // Events

        public  Task OnFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
                => Task.CompletedTask;

        /*public async Task OnFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
        {
            if (reason != TrackReason.Finished)
                return;
            LavaTrack nextTrack = null;

            player.Queue.Remove(track);
            Options.TryGetValue(player.Guild.Id, out var options);

            if (nextTrack is null)
            {
                await player.TextChannel.SendMessageAsync("Queue has been completed!");
                await Lavalink.DefaultNode.DisconnectAsync(context.Guild.Id);
                return;
            }

            nextTrack = options.RepeatTrack ? track :
                options.Shuffle ? RandomTrack(context.Guild.Id) : player.Queue.Dequeue();

            await player.PlayAsync(nextTrack);
            await player.TextChannel.SendMessageAsync($"**Now Playing:** {nextTrack.Title}, catto approves.");
        }
        */

        public Task OnException(LavaPlayer player, LavaTrack track, string error)
        {
            LoggingService.LogCritical("LavaLink", $"{error} thrown in {player.VoiceChannel} when playing {track.Title}.");
            return Task.CompletedTask;
        }
    }
}

