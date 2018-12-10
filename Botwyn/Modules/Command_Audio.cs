using Botwyn.Modules.Custom;
using Botwyn.Services;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Botwyn.Modules
{
    public sealed class AudioModule : CustomModule
    {
        public AudioService AudioService { get; set; }

        [Command("Join")]
        [Name("Join")][Summary("Joins the voice channel You're in.")]
        public async Task JoinAsync()
            => await ReplyAsync(await AudioService.JoinAsync((SocketGuildUser)Context.User, Context.Channel));

        [Command("Leave")]
        [Name("Leave")]
        [Summary("Leaves the voice channel You're in.")]
        public async Task LeaveAsync()
            => await ReplyAsync(await AudioService.LeaveAsync(Context.Guild.Id));

        [Command("Play")]
        [Name("Play")]
        [Summary("Play's a song you request by name or URL.")]
        public async Task PlayAsync([Summary("Can be either a search like ``Counting Stars - OneRepublic`` or a youtube url.")][Remainder] string query)
            => await ReplyAsync(await AudioService.PlayAsync(Context.Guild.Id, query));

        [Command("Stop")]
        [Name("Stop")]
        [Summary("Stop's the music and clears the queue.")]
        public async Task StopAsync()
            => await ReplyAsync(await AudioService.StopAsync(Context.Guild.Id));

        [Command("Skip")]
        [Name("Skip")]
        [Summary("Skips the current song.")]
        public async Task Skip()
            => await ReplyAsync(await AudioService.SkipAsync(Context.Guild.Id, (SocketGuildUser)Context.User));

        [Command("Pause")]
        [Name("Pause")]
        [Summary("Pauses the song you're playing.")]
        public async Task Pause()
            => await ReplyAsync( await AudioService.Pause(Context.Guild.Id));

        [Command("Resume")]
        [Name("Resume")]
        [Summary("Resumes the song you paused.")]
        public async Task Resume()
            => await ReplyAsync(await AudioService.Resume(Context.Guild.Id));

        [Command("List")]
        [Name("List")]
        [Summary("Lists the songs in the Queue.")]
        public async Task List()
            => await ReplyAsync("", false, await AudioService.List(Context.Guild.Id));

        [Command("Status")]
        [Name("Status")]
        [Summary("Displays the music status. (WIP)")]
        public Task Status()
            => ReplyAsync("", false, AudioService.Status(Context.Guild.Id));

        [Command("Music")]
        [Name("Music")]
        [Summary("Displays Music specific help.")]
        public Task MusicHelp()
            => ReplyAsync("", false, AudioService.Help());

        [Command("Volume")]
        [Name("Volume")]
        [Summary("Allows you to set the volume (1 - 149)")]
        public async Task VolumeAsync([Summary("The volume you want to set the music output to.")]int volume)
            => await ReplyAsync(await AudioService.Volume(Context.Guild.Id, volume));
    }
}
