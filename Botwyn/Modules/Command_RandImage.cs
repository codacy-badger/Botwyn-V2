using Botwyn.Modules.Custom;
using Botwyn.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Botwyn.Modules
{
    public class Command_RandImage : CustomModule
    {
        [Command("Cat", RunMode = RunMode.Async)]
        [Summary("Displays a random Cat image to the user.")]
        [Name("Cat")]
        public async Task Cat()
            => await ReplyAsync("", false, UtilService.FetchRandImage("cat"));

        [Command("Dog", RunMode = RunMode.Async)]
        [Summary("Displays a random Dog image to the user.")]
        [Name("Dog")]
        public async Task Dog()
            => await ReplyAsync("", false, UtilService.FetchRandImage("dog"));

        [Command("meme", RunMode = RunMode.Async)]
        [Summary("Displays a random Meme image to the user.")]
        [Name("Meme")]
        public async Task Meme()
            => await ReplyAsync("", false, UtilService.FetchRandImage("meme"));
    }
}
