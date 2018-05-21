using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Xml;
using CorporationBotGeneral;

namespace CorporationBotCoreGeneral
{
    [Group("changeto")]
    public class Recruiters : ModuleBase
    {
        [Command("minor")]
        public async Task ChangeToMinor(IUser user) {

            IGuildUser requser = Context.User as IGuildUser;
            ulong recruiter = 376500752203644928;
            ulong recruitchat = 262861494360735744;

            if (Context.Channel.Id == recruitchat)
            {

                if (requser.RoleIds.Contains(recruiter))
                {


                    ulong corporateerID = 92031682596601856;
                    ulong corporateerShieldID = 315095427361800192;
                    IGuildUser enduser = user as IGuildUser;

                    IRole corpRole = Context.Guild.GetRole(corporateerID);
                    IRole MinorCorpRole = Context.Guild.GetRole(corporateerShieldID);

                    await enduser.RemoveRoleAsync(corpRole);
                    await enduser.AddRoleAsync(MinorCorpRole);

                    await Context.Channel.SendMessageAsync(enduser.Username + " was changed from " + corpRole.Name + " to " + MinorCorpRole.Name);
                }
                else
                {

                    await Context.Channel.SendMessageAsync("You are not authorized to use this command");

                }
            }
            else {

                await Context.Channel.SendMessageAsync("You cannot use this here");

            }

        }

        [Command("adult")]
        public async Task ChangeToAdult(IUser user) {

            IGuildUser requser = Context.User as IGuildUser;
            ulong recruiter = 376500752203644928;
            ulong recruitchat = 262861494360735744;

            if (Context.Channel.Id == recruitchat)
            {

                if (requser.RoleIds.Contains(recruiter))
                {

                    ulong corporateerID = 92031682596601856;
                    ulong corporateerShieldID = 315095427361800192;
                    IGuildUser enduser = user as IGuildUser;

                    IRole corpRole = Context.Guild.GetRole(corporateerID);
                    IRole MinorCorpRole = Context.Guild.GetRole(corporateerShieldID);

                    await enduser.AddRoleAsync(corpRole);
                    await enduser.RemoveRoleAsync(MinorCorpRole);


                    await Context.Channel.SendMessageAsync(enduser.Username + " was changed from " + MinorCorpRole.Name + " to " + corpRole.Name);

                }
                else {

                    await Context.Channel.SendMessageAsync("You are not authorized to use this command");

                }

            }
            else {

                await Context.Channel.SendMessageAsync("You cannot use this here");

            }



        }


        


    }
}
