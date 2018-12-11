using Newtonsoft.Json;

namespace Botwyn.Services
{
    public class BotConfig
    {
        [JsonProperty("discordToken")]
        public string DiscordToken { get; set; }

        public char Prefix { get; set; }
    }
}