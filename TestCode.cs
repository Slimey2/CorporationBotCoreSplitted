using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Xml;

namespace CorporationBotCoreGeneral
{
    public class TestCode : ModuleBase
    {
        [Command("Test")]
        public async Task TestCodeMedthode() {

            IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);

            IGuildUser user = guild.GetUserAsync(Context.User.Id) as IGuildUser;
            SocketUser user2 = await guild.GetUserAsync(Context.User.Id) as SocketUser;
            IDMChannel channel = await user2.GetOrCreateDMChannelAsync();

            await channel.SendFileAsync("Config.xml");



        }






    }
}
