using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CorporationBotGeneral
{
    class Shutdown : ModuleBase
    {

        [Command("shutdown")]
        public async Task ShutdownCommand() {
            ulong modchat = 110270291644526592;
            IMessageChannel moderatorchat = await Context.Guild.GetChannelAsync(modchat) as IMessageChannel;
            IGuildUser user = Context.User as IGuildUser;
            ulong moderatorID = 84893161574391808;
            //ulong boardID = 127310237018357760;
            ulong ceoID = 206089923911090177;

            if (user.RoleIds.Contains(moderatorID) || user.RoleIds.Contains(ceoID))
            {
                await moderatorchat.SendMessageAsync("Emergency Shutdown initiated!");
                System.Environment.Exit(1);
            }

        }

    }
}
