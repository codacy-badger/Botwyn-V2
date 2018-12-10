using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Botwyn.Modules.Custom;
using Victoria;

namespace Botwyn.Services
{
    public sealed class DiscordService : BaseService
    {
        
        public async Task InitializeAsync(Assembly assembly)
        {
            await CommandService.AddModulesAsync(assembly, Provider);
            var json = string.Empty;
            BotConfig cfg = new BotConfig();
            if (!File.Exists("config.json"))
            {
                json = JsonConvert.SerializeObject(cfg);
                File.WriteAllText("config.json", json, new UTF8Encoding(false));
                Console.WriteLine("Config file was not found, a new one was generated. Fill it with proper values and rerun this program");
                Console.ReadKey();

                return;
            }
            json = File.ReadAllText("config.json", new UTF8Encoding(false));
            cfg = JsonConvert.DeserializeObject<BotConfig>(json);


            await RestClient.LoginAsync(TokenType.Bot, cfg.DiscordToken);
            await SocketClient.LoginAsync(TokenType.Bot, cfg.DiscordToken);
            await SocketClient.StartAsync();

            HookEvents();
        }

        private void HookEvents()
        {
            Lavalink.Log += OnLog;
            SocketClient.Log += OnLog;
            SocketClient.Ready += OnReadyAsync;
            SocketClient.MessageReceived += OnMessageAsync;
        }

        private Task OnLog(LogMessage log)
        {
            LoggingService.LogCritical(log.Source, log.Message);
            return Task.CompletedTask;
        }

        private async Task OnReadyAsync()
        {
            var node = await Lavalink.AddNodeAsync(SocketClient);
            node.TrackFinished += AudioService.OnFinished;
            node.TrackException += AudioService.OnException;
            await SocketClient.SetGameAsync("Ohai there. BLEP!");
        }

        private Task OnMessageAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message) || message.Author.IsBot || message.Author.IsWebhook
    || message.Channel is IPrivateChannel)
                return Task.CompletedTask;

            var argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos))
                return Task.CompletedTask;
            var context = new CustomContext(SocketClient, message);

            var result = CommandService.ExecuteAsync(context, argPos, Provider, MultiMatchHandling.Best);
            if (!result.Result.IsSuccess && socketMessage.Channel.Id != 504318315091722270) 
            {
                context.Channel.SendMessageAsync(result.Result.ErrorReason);
            }

            return result;
        }
    }
}
