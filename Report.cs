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

namespace CorporationBotCoreGeneral
{
    public class Report : ModuleBase
    {

        [Command("report")]
        public async Task ReportUser(String message) {

            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);
            ulong mod = 110270291644526592;

            /*
             * Test Values 
             * 
             */
            //IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);
            //ulong mod = 337141993362685952;

            string endmessage = "The following report was received by " + Context.User.Username + Environment.NewLine +
                "```" + message + "``` Attachments bellow if they were included.";


            var attachments = Context.Message.Attachments;
            IMessageChannel modchat = await guild.GetChannelAsync(mod) as IMessageChannel;

            await modchat.SendMessageAsync(endmessage);
            foreach (IAttachment a in attachments) {
                await modchat.SendMessageAsync(a.ProxyUrl);

                

            }

            await Context.Channel.SendMessageAsync("Your report has been forwarded to the moderation team. Please keep in mind that making a report doesn't guarantee punishment against the reported person");


        }


    }
}
