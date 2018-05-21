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
    [Group("member")]
    public class Count : ModuleBase
    {

        [Command("count")]
        public async Task CountCommand(IRole role) {
            ulong manager = 84900316213936128;
            IGuildUser enduser = Context.User as IGuildUser;
            int membercount = 0;
            if (enduser.RoleIds.Contains(manager))
            {
                var guildlist = await Context.Guild.GetUsersAsync();

                foreach (IGuildUser u in guildlist)
                {
                    if(u.RoleIds.Contains(role.Id))
                    membercount = membercount + 1;

                }

                await Context.User.SendMessageAsync("This division has " + membercount + " members");
            }
            else {

                await Context.User.SendMessageAsync("You do not have permission to use this command");

            }

        }

        [Command("list")]
        public async Task ListCommand(IRole role) {
            ulong manager = 84900316213936128;

            /*
             * Test Values
             */
            //ulong manager = 332898917631000576;
            IGuildUser enduser = Context.User as IGuildUser;
            var guildlist = await Context.Guild.GetUsersAsync();
            string list = "";

            if (enduser.RoleIds.Contains(manager))
            {

                foreach (IUser u in guildlist) {
                    IGuildUser user = u as IGuildUser;

                    if (user.RoleIds.Contains(role.Id)) {

                        list = list + user.Username + Environment.NewLine;

                    }



                }

                await Context.User.SendMessageAsync(list);



            }
            else {

                await Context.User.SendMessageAsync("You do not have permissions to use this command");

            }





        }


    }
}
