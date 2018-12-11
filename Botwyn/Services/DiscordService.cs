﻿using System;
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
using Botwyn.Handlers;

namespace Botwyn.Services
{
    public sealed class DiscordService : BaseService
    {
        public async Task InitializeAsync(Assembly assembly)
        {
            GlobalProperties.Initialize();
            await CommandService.AddModulesAsync(assembly);
            await RestClient.LoginAsync(TokenType.Bot, GlobalProperties.Config.DiscordToken);
            await SocketClient.LoginAsync(TokenType.Bot, GlobalProperties.Config.DiscordToken);
            await SocketClient.StartAsync();

            HookEvents();
        }

        private void HookEvents()
        {
            Lavalink.Log += OnLog;
            SocketClient.Log += OnLog;
            SocketClient.Ready += OnReadyAsync;
            SocketClient.MessageReceived += OnMessageAsync;
            SocketClient.ReactionAdded += OnReactionAdded;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if(reaction.MessageId == GlobalProperties.ReactionMessageID)
            {
                var sendChannel = (ISocketMessageChannel)SocketClient.GetChannel(521861918760370177);
                if(reaction.Emote.Name == "👌")
                {
                    
                }
            }
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
            if (!message.HasCharPrefix(GlobalProperties.Config.Prefix, ref argPos))
                return Task.CompletedTask;
            var context = new CustomContext(SocketClient, message);

            var result = CommandService.ExecuteAsync(context, argPos, Provider, MultiMatchHandling.Best);
            if (!result.Result.IsSuccess && socketMessage.Channel.Id != 504318315091722270) 
            {
                context.Channel.SendMessageAsync(result.Result.ErrorReason);
                context.Channel.SendMessageAsync(result.Result.Error.Value.ToString());
            }

            return result;
        }
    }
}
