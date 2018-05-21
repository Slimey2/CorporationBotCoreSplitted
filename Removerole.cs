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
using System.Threading;

namespace CorporationBotGeneral
{
    [Group("remove")]
    public class Removerole : ModuleBase
    {

        [Command("proxy")]
        public async Task RemoveProxyCommand(IUser adduser, IRole removerole = null) {

           

            
            ulong recruitchat = 262861494360735744;
            ulong managerId = 84900316213936128;
            ulong proxy = 353283208625913857;
            ulong corporateerID = 92031682596601856;
            ulong corporateerShieldID = 315095427361800192;



            /*
             * Test Values
             */
            //ulong proxy = 397780563462979594;
            //ulong managerId = 332898917631000576;
            //ulong recruitchat = 328162927653814273;
            //ulong corporateerID = 326381093081317376;


            IGuildUser requser = Context.User as IGuildUser;
            IGuildUser enduser = adduser as IGuildUser;
            var rolelist = Context.Guild.Roles;

            var guildlist = await Context.Guild.GetUsersAsync();

            XmlDocument doc = new XmlDocument();
            doc.Load("ColorConfig.xml");
            XmlNode l1 = doc.FirstChild;

            IRole proxytoremove = Context.Guild.GetRole(353283208625913857);
            IRole managertoremove = Context.Guild.GetRole(84900316213936128);
            IRole colortoremove = null;
            IRole tagtoremove = null;
            IRole standardtag = null;
            Boolean authorizedproxy = false;

            string division = "";

            if (!(requser.RoleIds.Contains(proxy)) && (enduser.RoleIds.Contains(corporateerID) || enduser.RoleIds.Contains(corporateerShieldID)) && enduser.RoleIds.Contains(proxy))
            {
                if (Context.Channel.Id == recruitchat)
            {

                while (true)
                {

                    foreach (IRole role in rolelist)
                    {

                        if (removerole == null)
                        {

                            if (role.Name.Contains("DL") || role.Name.Contains("DH"))
                            {

                                if (requser.RoleIds.Contains(role.Id))
                                {

                                    tagtoremove = role;
                                    authorizedproxy = true;

                                }

                            }
                        }
                        else
                        {

                            tagtoremove = removerole;

                        }


                        /*
                            * If DL or DH tag is found
                            * Program will look for the correct Color role
                        */

                        if (tagtoremove != null)
                        {


                            var split = tagtoremove.Name.Split(' ');

                            if (standardtag == null)
                            {
                                if (split.Length > 2)
                                {
                                    division = split[1] + " " + split[2];

                                }
                                else
                                {

                                    division = split[1];
                                }


                                if (!(tagtoremove.Name.Contains("DH")))
                                {
                                    if (role.Name == division)
                                    {

                                        standardtag = role;

                                    }
                                }
                            }


                            if (colortoremove == null)
                            {
                                XmlNode list = l1.SelectSingleNode("/colorConfig/attribute[@name='" + division.ToLower() + "']");
                                if (list != null)
                                {

                                    ulong id = Convert.ToUInt64(list.InnerText);

                                    if (role.Id == id)
                                    {

                                        colortoremove = role;
                                    }
                                }
                            }
                        }


                    }


                    if (proxytoremove != null && managertoremove != null && tagtoremove != null && colortoremove != null && standardtag != null)
                    {

                        break;
                    }
                    else if (proxytoremove != null && managertoremove != null && tagtoremove != null && colortoremove != null && (tagtoremove.Name.Contains("DH")))
                    {
                        break;

                    }


                }

                if (removerole != null)
                {

                    authorizedproxy = CheckDhAuthorisation(tagtoremove, requser);

                }

                if (authorizedproxy == true)
                {


                    await enduser.RemoveRoleAsync(colortoremove);
                    await enduser.RemoveRoleAsync(tagtoremove);
                    

                    await Context.Channel.SendMessageAsync(enduser.Username + " has lost: " + managertoremove.Name + " , " + proxytoremove + " , " + colortoremove.Name + " , " + tagtoremove);
                    await Context.Channel.SendMessageAsync("~remove proxyroles \"" + adduser + "\"");


                    }
                else
                {

                    await Context.Channel.SendMessageAsync("You cannot remove this proxy");

                }
            }
            else {

                await Context.Channel.SendMessageAsync("You cannot use this here");

            }
            }
            else
            {

                await Context.Channel.SendMessageAsync("You are a proxy and cannot assign a proxy role to somebody else");

            }



        }




        [Command("div")]
        public async Task RemoveroleCommand(IUser adduser, IRole role) {

            IGuildUser user = Context.User as IGuildUser;
            var userlist = await Context.Guild.GetUsersAsync();
            ulong DHSocialID = 261026068952121347;
            ulong DHSecurityID = 148505388235489282;
            ulong DHExploration = 261025367785996289;
            ulong DHSupport = 231457317030330368;
            ulong DHResources = 261025805033799681;
            ulong DHPR = 61026382950301698;
            ulong DHBusiness = 229989996151439361;

            string rank = role.Name;
            var splitted = rank.Split(' ');
            string rolename = "";



            if (splitted[0] == "UL")
            {
                if (splitted.Length > 2)
                {
                    rolename = splitted[1] + " " + splitted[2];

                }
                else
                {

                    rolename = splitted[1];
                }

            }
            else
            {

                rolename = role.Name;
            }


            if (user.RoleIds.Contains(DHSocialID))
            {



                    if (rolename == "Human Resources" || rolename == "Training" || rolename == "Diplomacy" || CheckSubDivision(Context.Guild.GetRole(DHSocialID), role) == true)
                    {

                        RemoveRoleDH(adduser, role);
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }
                

            }
            else if (user.RoleIds.Contains(DHSecurityID))
            {



                    if (rolename == "Ground Security" || rolename == "Space Security" || rolename == "CSOC" || rolename == "Repo" || CheckSubDivision(Context.Guild.GetRole(DHSecurityID), role) == true)
                    {

                        RemoveRoleDH(adduser, role);
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }



            }
            else if (user.RoleIds.Contains(DHSupport))
            {



                    if (rolename == "CSAR" || rolename == "Engineering" || CheckSubDivision(Context.Guild.GetRole(DHSupport), role) == true)
                    {

                        RemoveRoleDH(adduser, role);

                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }


            }
            else if (user.RoleIds.Contains(DHExploration))
            {




                    if (rolename == "Prospecting" || rolename == "Cartography" || rolename == "Research" || CheckSubDivision(Context.Guild.GetRole(DHExploration), role) == true)
                    {

                        RemoveRoleDH(adduser, role);
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }


            }
            else if (user.RoleIds.Contains(DHResources))
            {



                    if (rolename == "Development" || rolename == "Extraction" || rolename == "Transport" || CheckSubDivision(Context.Guild.GetRole(DHResources), role) == true)
                    {

                        RemoveRoleDH(adduser, role);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");

                    }


            }
            else if (user.RoleIds.Contains(DHPR))
            {


 
                    if (rolename == "e-Sport" || rolename == "Media" || rolename == "Streamer" || CheckSubDivision(Context.Guild.GetRole(DHPR), role) == true)
                    {

                        RemoveRoleDH(adduser, role);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");

                    }


            }
            else if (user.RoleIds.Contains(DHBusiness))
            {




                    if (rolename == "Finance" || rolename == "Trade" || rolename == "Contracts" || CheckSubDivision(Context.Guild.GetRole(DHBusiness), role) == true)
                    {

                        RemoveRoleDH(adduser, role);

                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }


            }
            else { RemoveRole(adduser, role,userlist); }


        }

        [Command("proxyroles")]
        public async Task AssignProxyRoles(IUser adduser)
        {
            IGuildUser user = adduser as IGuildUser;

            ulong managerId = 84900316213936128;
            ulong proxy = 353283208625913857;


            /*
             * Test Values
             */
            //ulong proxy = 397780563462979594;
            //ulong managerId = 332898917631000576;

            if (Context.User.Id == Context.Client.CurrentUser.Id)
            {

                await user.RemoveRoleAsync(Context.Guild.GetRole(proxy));
                await user.RemoveRoleAsync(Context.Guild.GetRole(managerId));


                await Context.Channel.SendMessageAsync("I have added the needed proxy and manager role");

            }

        }




        private async void RemoveRoleDH(IUser adduser, IRole role)
        {

            IGuildUser user = Context.User as IGuildUser;
            ulong modchat = 110270291644526592;
            ulong managerId = 84900316213936128;
            ulong moderatorID = 84893161574391808;
            ulong boardID = 127310237018357760;
            ulong ceoID = 206089923911090177;
            ulong corporateerID = 92031682596601856;
            ulong corporateerShieldID = 315095427361800192;
            ulong recruitchat = 262861494360735744;

            var allroles = Context.Guild.Roles;



            IMessageChannel moderatorchat = await Context.Guild.GetChannelAsync(modchat) as IMessageChannel;

            if (Context.Channel.Id == recruitchat)
            {

                if (role.Id == managerId || role.Id == moderatorID || role.Id == boardID || role.Id == ceoID || role.Id == corporateerID)
                {

                    await moderatorchat.SendMessageAsync("user: " + user.Username + " is trying to remove the following role " + role.Name + " to " + adduser.Username);

                }
                else
                {


                    IGuildUser finaluser = adduser as IGuildUser;
                    if (finaluser.RoleIds.Contains(corporateerID) || finaluser.RoleIds.Contains(corporateerShieldID))
                    {


                        await finaluser.RemoveRoleAsync(role);
                        await Context.Channel.SendMessageAsync("I have removed: " + role.Name + " to " + finaluser.Username);





                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("This person has no corporateer tag. Please contact a moderator");
                    }


                }


            }
            else
            {

                await Context.Channel.SendMessageAsync("You cannot use this command in this channel");

            }



        }


        private async void RemoveRole(IUser adduser, IRole role, IReadOnlyCollection<IGuildUser> userlist)
        {
            IGuildUser user = Context.User as IGuildUser;
            ulong modchat = 110270291644526592;
            ulong managerId = 84900316213936128;
            ulong moderatorID = 84893161574391808;
            ulong boardID = 127310237018357760;
            ulong ceoID = 206089923911090177;
            ulong corporateerID = 92031682596601856;
            ulong corporateerShieldID = 315095427361800192;
            ulong recruitchat = 262861494360735744;
            ulong dlMedia = 261745098881105921;
            ulong streamer = 308356281050071040;


            ulong DlID = 0;
            var allroles = Context.Guild.Roles;
            string rank = role.Name;
            var splitted = rank.Split(' ');
            string rolename = "";
            bool checksubdiv = false;

            if (splitted[0] == "UL")
            {
                if (splitted.Length > 2)
                {
                    rolename = splitted[1] + " " + splitted[2];

                }
                else
                {

                    rolename = splitted[1];
                }

            }
            else
            {

                rolename = role.Name;
            }

            foreach (IRole r in allroles)
            {
                if (r.Name == "DL " + rolename)
                {
                    


                    DlID = r.Id;
                }

            }


            if (DlID == 0) {

                foreach (IRole r in allroles)
                {
                    if (r.Name.Contains("DL") && user.RoleIds.Contains(r.Id))
                    {



                        DlID = r.Id;
                        checksubdiv = true;
                    }

                }




            }


            IMessageChannel moderatorchat = await Context.Guild.GetChannelAsync(modchat) as IMessageChannel;

            if (Context.Channel.Id == recruitchat)
            {

                if (role.Id == managerId || role.Id == moderatorID || role.Id == boardID || role.Id == ceoID || role.Id == corporateerID)
                {

                    await moderatorchat.SendMessageAsync("user: " + user.Username + " is trying to remove the following role " + role.Name + " to " + adduser.Username);

                }
                else
                {


                        IGuildUser finaluser = adduser as IGuildUser;
                    if (finaluser.RoleIds.Contains(corporateerID) || finaluser.RoleIds.Contains(corporateerShieldID))
                    {
                        if ((user.RoleIds.Contains(DlID) && checksubdiv==false )|| CheckSubDivision(Context.Guild.GetRole(DlID), role) == true)
                        {

                            await finaluser.RemoveRoleAsync(role);
                            await Context.Channel.SendMessageAsync("I have removed: " + role.Name + " to " + finaluser.Username);
                        }
                        else
                        {

                            await Context.Channel.SendMessageAsync("You do not have the permissions to remove this role.");

                        }




                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("This person has no corporateer tag. Please contact a moderator");
                    }



                }


            }
            else
            {

                await Context.Channel.SendMessageAsync("You cannot use this command in this channel");

            }


        }


        private Boolean CheckDhAuthorisation(IRole role, IUser enduser)
        {
            //Test Values
            //ulong DHSocialID = 346550652207497216;

            ulong DHSocialID = 261026068952121347;
            ulong DHSecurityID = 148505388235489282;
            ulong DHExploration = 261025367785996289;
            ulong DHSupport = 231457317030330368;
            ulong DHResources = 261025805033799681;
            ulong DHPR = 61026382950301698;
            ulong DHBusiness = 229989996151439361;
            IGuildUser user = enduser as IGuildUser;
            string rolename = role.Name;
            Boolean authorized = false;

            if (user.RoleIds.Contains(DHSocialID))
            {


                if (rolename == "DL Human Resources" || rolename == "DL Training" || rolename == "DL Diplomacy")
                {

                    authorized = true;
                }

            }
            else if (user.RoleIds.Contains(DHSecurityID))
            {


                if (rolename == "DL Ground Security" || rolename == "DL Space Security" || rolename == "DL CSOC" || rolename == "DL Repo")
                {

                    authorized = true;
                }


            }
            else if (user.RoleIds.Contains(DHSupport))
            {



                if (rolename == "DL CSAR" || rolename == "DL Engineering")
                {

                    authorized = true;

                }


            }
            else if (user.RoleIds.Contains(DHExploration))
            {

                if (rolename == "DL Prospecting" || rolename == "DL Cartography" || rolename == "DL Research")
                {

                    authorized = true;
                }


            }
            else if (user.RoleIds.Contains(DHResources))
            {

                if (rolename == "DL Development" || rolename == "DL Extraction" || rolename == "DL Transport")
                {

                    authorized = true;
                }

            }
            else if (user.RoleIds.Contains(DHPR))
            {


                if (rolename == "DL e-Sport" || rolename == "DL Media" || rolename == "DL Streamer")
                {

                    authorized = true;
                }


            }
            else if (user.RoleIds.Contains(DHBusiness))
            {


                if (rolename == "DL Finance" || rolename == "DL Trade" || rolename == "DL Contracts")
                {

                    authorized = true;

                }


            }

            return authorized;


        }


        private Boolean CheckSubDivision(IRole managerRole, IRole subdivRole)
        {
            Boolean authorized = false;

            XmlDocument doc = new XmlDocument();
            doc.Load("SubDivisions.xml");
            XmlNode first = doc.FirstChild;
            string manager = managerRole.Name.Replace(" ", String.Empty);


            XmlNode list = doc.SelectSingleNode("/list/" + manager + "[@subdivision='" + subdivRole.Name + "']");

            if (list != null)
            {

                authorized = true;

            }


            return authorized;
        }


    }
}
