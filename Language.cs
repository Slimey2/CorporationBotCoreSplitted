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
    public class Language : ModuleBase
    {

        [Command("language")]
        public async Task AddLanguageRole(IUser user, IRole language = null)
        {
            IRole endlanguage = null;
            IGuildUser enduser = user as IGuildUser;
            IGuildUser requser = Context.User as IGuildUser;
            ulong recruiter = 376500752203644928;
            ulong recruitchat = 262861494360735744;

            if (Context.Channel.Id == recruitchat)
            {

                if (requser.RoleIds.Contains(recruiter))
                {

                    if (language == null)
                    {
                        endlanguage = Context.Guild.GetRole(408342925513064448);
                        await enduser.AddRoleAsync(endlanguage);
                        await Context.Channel.SendMessageAsync("Default english language tag was assigned");

                    }
                    else
                    {

                        if (Activation.languages.Contains(language.Id))
                        {
                            endlanguage = Context.Guild.GetRole(language.Id);
                            await enduser.AddRoleAsync(endlanguage);
                            await Context.Channel.SendMessageAsync("Assigned following language: " + language.Name);

                        }
                        else
                        {
                            endlanguage = Context.Guild.GetRole(408342925513064448);
                            await Context.Channel.SendMessageAsync("Selected language is not authorized yet. Please contact moderators to add language to the authorized list. Default English was added");
                            await enduser.AddRoleAsync(endlanguage);

                        }

                    }
                    
                }
                else
                {

                    await Context.Channel.SendMessageAsync("You are not authorized to use this command");

                }

            }
            else
            {

                await Context.Channel.SendMessageAsync("You cannot use this here");

            }


        }



    }
}
