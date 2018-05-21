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
using System.IO.Compression;

namespace CorporationBotCoreGeneral
{
    public class Moderation : ModuleBase
    {

        [Command("drama")]
        public async Task DramaCollect(IUser user = null) {
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;
            string result = "";
            IGuildUser finaluser = Context.User as IGuildUser;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (user == null)
                {
                    result = Activation.archiveobject.collectDrama();
                }
                else if (user != null)
                {
                    result = Activation.archiveobject.collectDrama(user);

                }
                if (result == "succes")
                {
                    if (Context.Channel.Id == mod)
                    {


                        await Context.Channel.SendFileAsync("Drama.csv");
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You cannot use this here");

                    }
                }
                else
                {

                    await Context.Channel.SendMessageAsync(result);

                }
            }
            else {

                await Context.Channel.SendMessageAsync("You do not have the permissions to use this command");

            }

        }

        [Command("startdeletemod")]
        public async Task StartDeleteModerator() {

            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;

            XmlDocument doc = new XmlDocument();
            doc.Load("BotConfig.xml");
            XmlNode l1 = doc.FirstChild;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    l1.SelectSingleNode("/BotConfig/deleteConfig").InnerText = "True";
                    doc.Save("BotConfig.xml");
                    doc = null;
                    await Context.Channel.SendMessageAsync("Logging on deletions in chat has now been activated");
                    Activation.reloadConfig();


                }
                else {

                    await Context.Channel.SendFileAsync("You cannot use this here");

                }


            }
            else {

                await Context.Channel.SendFileAsync("You do not have the permissions to use this command");

            }

            


        }

        [Command("stopdeletemod")]
        public async Task StopDeleteModerator()
        {

            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;

            XmlDocument doc = new XmlDocument();
            doc.Load("BotConfig.xml");
            XmlNode l1 = doc.FirstChild;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    l1.SelectSingleNode("/BotConfig/deleteConfig").InnerText = "False";
                    doc.Save("BotConfig.xml");
                    doc = null;
                    await Context.Channel.SendMessageAsync("Logging on deletions in chat has now been activated");
                    Activation.reloadConfig();

                }
                else
                {

                    await Context.Channel.SendFileAsync("You cannot use this here");

                }


            }
            else
            {

                await Context.Channel.SendFileAsync("You do not have the permissions to use this command");

            }


        }



        [Command("deactivatecommands")]
        public async Task DeactivateCommands() {

            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;

            /*
             * Test Values 
             */
            //ulong mod = 337141993362685952;
            //ulong moderatorID = 326381123984949259;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    Activation.active = false;
                    await Context.Channel.SendMessageAsync("Commands have been deactivated");


                }
                else {

                    await Context.Channel.SendFileAsync("You cannot use this here");

                }


            }
            else {

                await Context.Channel.SendFileAsync("You do not have the permissions to use this command");
            }


        }

        [Command("activatecommands")]
        public async Task ActivateCommands()
        {

            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    Activation.active = true;
                    await Context.Channel.SendMessageAsync("Bot commands have now been activated.");


                }
                else {

                    await Context.Channel.SendMessageAsync("You cannot use this here");

                }

            }
            else {

                await Context.Channel.SendMessageAsync("You do not have the permissions to use this command");
            }


        }


        [Command("addlanguage")]
        public async Task AddLanguageCommand(IRole language)
        {

            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;


            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    Activation.languages.Add(language.Id);
                    await Context.Channel.SendMessageAsync("I have added the following language to the authorised languages: " + language.Name);



                }
                else
                {

                    await Context.Channel.SendMessageAsync("You cannot use this here");

                }
            }
            else
            {

                await Context.Channel.SendMessageAsync("You do not have the permissions to use this command");
            }


        }

        [Command("cleanup")]
        public async Task CleanUpDB(String db) {
            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong ceoID = 206089923911090177;

            if (finaluser.Id == 104934438042910720 || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {
                    if (db == "logging" || db == "messages" || db == "statistics")
                    {
                        Activation.archiveobject.CleanUp(db);
                        await Context.Channel.SendMessageAsync("I have cleaned up "+ db + ".");
                    }
                    else{

                        await Context.Channel.SendMessageAsync("The input is not a current Database");

                    }


                }
                else
                {

                    await Context.Channel.SendMessageAsync("You cannot use this here");

                }
            }
            else
            {

                await Context.Channel.SendMessageAsync("You do not have the permissions to use this command");
            }


        }

        [Command("auth")]
        public async Task SetAuthorisation(String auth) {

            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);
            IGuildUser slimey = guild.GetUserAsync(104934438042910720) as IGuildUser;

            if (Context.User.Id == 104934438042910720 || Context.User.Id == 80708876743217152)
            {

                XmlDocument doc = new XmlDocument();
                doc.Load("Config.xml");
                XmlNode first = doc.FirstChild;
                XmlNode authentication = first.SelectSingleNode("/GeneralConfig/HttpConfig/Authorisation");
                authentication.InnerText = auth;

                doc.Save("Config.xml");

                await Context.Channel.SendMessageAsync("Authentication key was changed to: " + authentication.InnerText);

            }
            else {

                await slimey.SendMessageAsync(Context.User.Username + " attempted to change the authorisation string for the influence system!");
                await Context.Channel.SendMessageAsync("You are not authorised to use this command!");

            }



        }

        [Command("url")]
        public async Task SetUrl(String url)
        {

            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);
            IGuildUser slimey = guild.GetUserAsync(104934438042910720) as IGuildUser;

            if (Context.User.Id == 104934438042910720 || Context.User.Id == 80708876743217152)
            {

                XmlDocument doc = new XmlDocument();
                doc.Load("Config.xml");
                XmlNode first = doc.FirstChild;
                XmlNode authentication = first.SelectSingleNode("/GeneralConfig/HttpConfig/Url");
                authentication.InnerText = url;

                doc.Save("Config.xml");

                await Context.Channel.SendMessageAsync("Authentication key was changed to: " + authentication.InnerText);

            }
            else
            {

                await slimey.SendMessageAsync(Context.User.Username + " attempted to change the url string for the influence system!");
                await Context.Channel.SendMessageAsync("You are not authorised to use this command!");

            }


        }

        [Command("openlogin")]
        public async Task StartLogRequest() {

            //IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);

            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);

            IGuildUser slimey = guild.GetUserAsync(104934438042910720) as IGuildUser;

            XmlDocument doc = new XmlDocument();
            doc.Load("BotConfig.xml");
            XmlNode l1 = doc.FirstChild;


            if (Context.User.Id == 104934438042910720 || Context.User.Id == 80708876743217152)
            {
                l1.SelectSingleNode("/BotConfig/httpConfig").InnerText = "True";
                doc.Save("BotConfig.xml");
                doc = null;

                await Context.Channel.SendMessageAsync("Login request has been activated");
                Activation.reloadConfig();
            }
            else
            {

                await slimey.SendMessageAsync(Context.User.Username + " attempted to change the activation status for the influence system!");
                await Context.Channel.SendMessageAsync("You are not authorised to use this command!");

            }


        }

        [Command("closelogin")]
        public async Task StopLogRequest()
        {
            //IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);


            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);
            IGuildUser slimey = guild.GetUserAsync(104934438042910720) as IGuildUser;

            XmlDocument doc = new XmlDocument();
            doc.Load("BotConfig.xml");
            XmlNode l1 = doc.FirstChild;

            if (Context.User.Id == 104934438042910720 || Context.User.Id == 80708876743217152)
            {
                l1.SelectSingleNode("/BotConfig/httpConfig").InnerText = "False";
                doc.Save("BotConfig.xml");
                doc = null;

                await Context.Channel.SendMessageAsync("Login request has been deactivated");
                Activation.reloadConfig();
            }
            else
            {

                await slimey.SendMessageAsync(Context.User.Username + " attempted to change the activation status for the influence system!");
                await Context.Channel.SendMessageAsync("You are not authorised to use this command!");

            }


        }

        [Command("getconfig")]
        public async Task getConfigFile(string file) {
            IGuildUser finaluser = Context.User as IGuildUser;
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;

            if (finaluser.Id == 104934438042910720 || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    if (file.ToLower() == "botconfig")
                    {

                        await Context.Channel.SendFileAsync("BotConfig.xml");

                    }
                    else if (file.ToLower() == "colorconfig")
                    {

                        await Context.Channel.SendFileAsync("ColorConfig.xml");

                    }
                    else if (file.ToLower() == "config")
                    {

                        await Context.Channel.SendFileAsync("Config.xml");

                    }
                    else if (file.ToLower() == "imageconfig")
                    {

                        await Context.Channel.SendFileAsync("ImageConfig.xml");

                    }
                    else if (file.ToLower() == "subdivisions")
                    {

                        await Context.Channel.SendFileAsync("SubDivisions.xml");

                    }

                }
                else
                {

                    await Context.Channel.SendMessageAsync("You cannot use this here");

                }

            }
            else
            {

                await Context.Channel.SendMessageAsync("You do not have the permissions to use this command");

            }

        }


    }
}
