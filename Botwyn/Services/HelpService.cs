using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static Botwyn.Handlers.EmbedHandler;

namespace Botwyn.Services
{
    public class HelpService
    {
        public Embed GetSingleCommandInfoAsync(string command, ICommandContext context, CommandService Service, ISocketMessageChannel channel)
        {
            string prefix = "!";
            var embed = new EmbedBuilder().WithColor(Color.DarkMagenta);
            var result = Service.Search(context, command);
            var descriptionBuilder = new StringBuilder();

            if (result.IsSuccess)
            {
                var cmdResult = result.Commands.FirstOrDefault(x => x.Command.Name.ToLower() == command.ToLower());
                embed.WithTitle($"Here is further info about the command: {cmdResult.Command.Name}");

                descriptionBuilder.Append($"\nName: ``{prefix}{cmdResult.Command.Name}`` \nSummary: {cmdResult.Command.Summary}\n");

                if (cmdResult.Command.Parameters != null)
                {
                    foreach (var param in cmdResult.Command.Parameters)
                    {
                        descriptionBuilder.Append($"\n**Param**: {param.Name} \n{(String.IsNullOrEmpty(param.Summary) ? "" : $"Summary: {param.Summary}")}");
                    }
                    descriptionBuilder.Append($"\n\n**Usage: ``{prefix}{cmdResult.Command.Name} ");
                    foreach (var param in cmdResult.Command.Parameters)
                    {
                        descriptionBuilder.Append($"{param.Name} ");
                    }
                    descriptionBuilder.Append("``**");
                }
                else
                {
                    descriptionBuilder.Append($"**Usage: ``{prefix}{cmdResult.Command.Name}``**");
                }
                embed.WithDescription(descriptionBuilder.ToString());
                return embed.Build();
            }
            else
            {
                return CreateEmbed("ERROR", "Your Command Seems Incorrect.", EmbedMessageType.Error, true);
            }
            
        }

        public async Task<Embed> GetAllCommandsAsync(ICommandContext context, CommandService Service, ISocketMessageChannel channel)
        {
            string prefix = "!";
            var embed = new EmbedBuilder()
            .WithTitle("These are the commands avilable!")
            .WithColor(Color.DarkMagenta);

            var builtDescription = new StringBuilder();
            foreach (var module in Service.Commands)
            {
                string command = null;

                var result = await module.CheckPreconditionsAsync(context);
                if (result.IsSuccess)
                {
                    command += $"{prefix}{module.Name}\n";
                   // summary += $"{module.Summary}";
                }

                if (!string.IsNullOrWhiteSpace(command))
                {
                    builtDescription.Append($"``{command}`` ");
                }
            }

            embed.WithDescription($"{builtDescription.ToString()}\n\n **You can also use the command: ``{prefix}Help (CommandName)`` to find out more info on how to use a command.**");
     
            return embed.Build();
        }
    }
}
