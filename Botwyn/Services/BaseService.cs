using System;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using HyperEx;
using Victoria;

namespace Botwyn.Services
{
    [Inject]
    public class BaseService
    {
        public Lavalink Lavalink { get; set; }
        public IServiceProvider Provider { get; set; }
        public AudioService AudioService { get; set; }
        public DiscordRestClient RestClient { get; set; }
        public DiscordSocketClient SocketClient { get; set; }
        public CommandService CommandService { get; set; }
    }
}
