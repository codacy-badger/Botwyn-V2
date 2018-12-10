using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using System.Linq;
using Botwyn.Modules.Custom;
using Botwyn.Services;

namespace Botwyn.Modules
{
    public class Command_Help : CustomModule
    {
        public CommandService Service { get; set; }
        public HelpService HelpService = new HelpService();

        [Command("Help", RunMode = RunMode.Async), Name("Help"), Summary("Displays help to the user.")]
        public async Task Help()
        {
            if (UtilService.IsChanBlacklisted(Context.Channel))
            {
                await Context.Message.DeleteAsync();
                LoggingService.Log("blacklist", LogSeverity.Info, "Command used in wrong channel.");
                return;
            }

            await ReplyAsync("", false, await HelpService.GetAllCommandsAsync(Context, Service, Context.Channel));
            // await Context.User.SendMessageAsync("", false, await HelpService.CommandsAsync(Context, Service, Context.Channel));
        }

        [Command("help", RunMode = RunMode.Async), Name("Help"), Summary("Displays help to the user.")]
        public async Task Help([Summary("The command you want help for.")][Remainder]string command)
        {
            if (UtilService.IsChanBlacklisted(Context.Channel))
            {
                await Context.Message.DeleteAsync();
                LoggingService.Log("blacklist", LogSeverity.Info, "Command used in wrong channel.");
                return;
            }

            await ReplyAsync("", false, HelpService.GetSingleCommandInfoAsync(command, Context, Service, Context.Channel));
            // await Context.User.SendMessageAsync("", false, await HelpService.CommandsAsync(Context, Service, Context.Channel));
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            LoggingService.LogInformation("command", $"{Context.User.Username} used {command.Name}");
        }
    }
}

