using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace CorporationBotGeneral
{


        
        public class Help : ModuleBase
        {

        [Command("help")]
        public async Task UserInfo()
        {
            try
            {
                IGuildUser user = Context.User as IGuildUser;
                var roles = user.RoleIds.ToList();
                ulong managerId = 84900316213936128;
                ulong moderatorID = 84893161574391808;
                ulong boardID = 127310237018357760;
                ulong ceoID = 206089923911090177;
                ulong corporateerID = 92031682596601856;
                ulong proxy = 353283208625913857;

                var author = new EmbedAuthorBuilder { Name = "The Corporation", Url = "https://forum.thecorporateer.com/" };



                var proxyrole = new EmbedFieldBuilder { IsInline = false, Name = "~assign proxy \"[username]\" ", Value = "add proxy tag to given user. Uses DL/DH present on the requester" };
                var proxyroleDH = new EmbedFieldBuilder { IsInline = false, Name = "~assign proxy \"[username]\"  \"[Division]\"", Value = "add proxy tag to given user and the given DL tag. Only usable by DH." };
                var adddivrole = new EmbedFieldBuilder { IsInline = false, Name = "~assign div \"[username]\" \"[Division]\"", Value = "add input div tag to given user" };

                var removeproxyrole = new EmbedFieldBuilder { IsInline = false, Name = "~remove proxy \"[username]\" ", Value = "remove proxy tag to given user. Uses DL/DH present on the requester" };
                var removeproxyroleDH = new EmbedFieldBuilder { IsInline = false, Name = "~remove proxy \"[username]\" \"[Division]\"", Value = "remove proxy tag to given user and the given DL tag. Only usable by DH." };
                var removedivrole = new EmbedFieldBuilder { IsInline = false, Name = "~remove div \"[username]\" \"[Division]\"", Value = "remove input div tag to given user" };


                var meetinglist = new EmbedFieldBuilder { IsInline = true, Name = "~meeting list", Value = "Display current system time and time until next meeting." };
                var meetingadd = new EmbedFieldBuilder { IsInline = true, Name = "~meeting add", Value = "Adds a meeting to the meeting list." };
                var meetingremove = new EmbedFieldBuilder { IsInline = true, Name = "~meeting remove", Value = "Removes a meeting to the meeting list." };

                var info = new EmbedFieldBuilder { IsInline = true, Name = "~info \"[Division/Department]\" or \"General\"/*Empty*", Value = "Display general or leadership info about the division/department. Example: ~Info " };
                var report = new EmbedFieldBuilder { IsInline = true, Name = "~report \"[Message]\" ", Value = "Can be used to report people when a moderator is not online. Message can contain an attachment " };


                var count = new EmbedFieldBuilder { IsInline = true, Name = "~member count \"[Division]\"", Value = "display amount of members with tag given as input" };
                var member = new EmbedFieldBuilder { IsInline = true, Name = "~member list \"[Division]\"", Value = "display the names of members with input tag" };

                var corporateer = new EmbedFieldBuilder { IsInline = true, Name = "~assign corp \"[Username]\" \"[Language]\"", Value = "display the names of members with input tag. [Language] is optional (English = Default)" };
                var changeToMinor = new EmbedFieldBuilder { IsInline = true, Name = "~minor \"[Username]\"", Value = "Changes Corporateer tag to Corporateer Shield tag" };
                var changeToAdult = new EmbedFieldBuilder { IsInline = true, Name = "~adult \"[Username]\"", Value = "Changes Corporateer Shield tag to Corporateer tag" };
                var language = new EmbedFieldBuilder { IsInline = true, Name = "~language \"[Username]\" \"[Language]\" ", Value = "add a language to a user. [Language] is optional (English = Default)" };


                var collectstats = new EmbedFieldBuilder { IsInline = true, Name = "~collectstats", Value = "collects the stores statistics from the Database and sends them in .CSV file" };
                var collectmod = new EmbedFieldBuilder { IsInline = true, Name = "~collectmodactions", Value = "collects the moderator actions from the Database and sends them in .CSV file" };
                var actstats = new EmbedFieldBuilder { IsInline = true, Name = "~activate", Value = "Activates the collections of statistics towards the Database" };
                var deacstats = new EmbedFieldBuilder { IsInline = true, Name = "~deactivate", Value = "Deactivate the collections of statistics towards the Database" };


                var drama = new EmbedFieldBuilder { IsInline = true, Name = "~drama \"[Username \" ", Value = "Collects the drama that was reported to the Database. [Username] is optional (Default = collects all)" };
                var startDelMod = new EmbedFieldBuilder { IsInline = true, Name = "~startdeletemod", Value = "Activates the collection on messages that are deleted" };
                var stopDelMod = new EmbedFieldBuilder { IsInline = true, Name = "~stopdeletemod", Value = "Deactivate the collection on messages that are deleted" };
                var deactivate = new EmbedFieldBuilder { IsInline = true, Name = "~deactivatecommands", Value = "Deactivate the bot commands" };
                var activate = new EmbedFieldBuilder { IsInline = true, Name = "~activatecommands", Value = "Activate the bot commands" };
                var addlanguage = new EmbedFieldBuilder { IsInline = true, Name = "~addlanguage", Value = "Add a language to the list of authorized languages" };
                var cleanup = new EmbedFieldBuilder { IsInline = true, Name = "~cleanup", Value = "Deletes all records in the given database." };




                List<EmbedFieldBuilder> fieldslist = new List<EmbedFieldBuilder>();
                var footer = new EmbedFooterBuilder { Text = "The Corporation™ | Any abuse/hacking/breaking of those commands will not be tolerated and could result in a permanent ban" };

                if (roles.Contains(corporateerID))
                {
                    fieldslist.Add(meetinglist);
                    fieldslist.Add(info);
                    fieldslist.Add(report);


                }
                if (roles.Contains(managerId) && !(roles.Contains(proxy)))
                {

                    fieldslist.Add(count);
                    fieldslist.Add(member);
                    fieldslist.Add(proxyrole);
                    fieldslist.Add(proxyroleDH);
                    fieldslist.Add(removeproxyrole);
                    fieldslist.Add(removeproxyroleDH);
                    fieldslist.Add(removedivrole);
                    fieldslist.Add(adddivrole);
                    fieldslist.Add(meetingadd);
                    fieldslist.Add(meetingremove);



                }
                if (roles.Contains(managerId) && roles.Contains(proxy))
                {

                    fieldslist.Add(count);
                    fieldslist.Add(member);
                    fieldslist.Add(removedivrole);
                    fieldslist.Add(adddivrole);
                    fieldslist.Add(meetingadd);
                    fieldslist.Add(meetingremove);


                }
                if (roles.Contains(moderatorID) || roles.Contains(ceoID))
                {
                    fieldslist.Add(drama);
                    fieldslist.Add(deactivate);
                    fieldslist.Add(activate);
                    fieldslist.Add(addlanguage);
                    fieldslist.Add(cleanup);

                }
                if (roles.Contains(boardID))
                {
                    fieldslist.Add(collectstats);
                    fieldslist.Add(collectmod);
                    fieldslist.Add(actstats);
                    fieldslist.Add(deacstats);


                }
                var ember = new EmbedBuilder { Title = "Bot Command list", Description = "Hello , this is the list of all the general commands you can use", ThumbnailUrl = "https://media.discordapp.net/attachments/229700730611564545/367320352000573451/CorpLogo.png?width=665&height=610", Author = author, Fields = fieldslist, Footer = footer };
                await Context.User.SendMessageAsync("", false, ember);






            }
            catch(Exception ex)
            {

                await Context.User.SendMessageAsync("Log Help Command: If you send a message direct to the Bot please use the command in a discord chat." + Environment.NewLine
                    + "If you did use the command in a chatchannel but you still have an error please check the spelling of you command and try again. If the error still persist please contact Slimey2");


            }


        }



        

    }
}
