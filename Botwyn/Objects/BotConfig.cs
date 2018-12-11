using Newtonsoft.Json;

namespace Botwyn.Services
{
    internal class BotConfig
    {
        [JsonProperty("discordToken")]
        public string DiscordToken { get; private set; } = string.Empty;
        
        [JsonProperty("wowToken")]
        public string WoWToken { get; private set; } = string.Empty;

        public char Token { get; set; }
    }
}