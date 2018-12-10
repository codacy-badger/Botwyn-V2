using Discord.Commands;
using Discord.WebSocket;

namespace Botwyn.Modules.Custom
{
    public sealed class CustomContext : SocketCommandContext
    {
        public new SocketGuildUser User { get; }

        public CustomContext(DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
            User = msg.Author as SocketGuildUser;
        }
    }
}
