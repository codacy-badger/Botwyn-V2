using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Botwyn.Modules.Custom;
using Discord.Commands;

namespace Botwyn
{
    public class Command_Test : CustomModule
    {
        [Command("say"), Summary("Echos a message.")]
        [Name("say")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }
    }
}
