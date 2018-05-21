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
    [Group("assign")]
    public class Addrole : ModuleBase
    {
        

        [Command("proxy")]
        public async Task AssignProxyTag(IUser adduser, IRole addrole = null) {


            
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
            var rolelist =  Context.Guild.Roles;

            var guildlist = await Context.Guild.GetUsersAsync();

            XmlDocument doc = new XmlDocument();
            doc.Load("ColorConfig.xml");
            XmlNode l1 = doc.FirstChild;

            IRole proxytogive = Context.Guild.GetRole(proxy);
            IRole managertogive = Context.Guild.GetRole(managerId);
            IRole colortogive = null;
            IRole tagtogive = null;
            IRole standardtag = null;
            Boolean authorizedproxy = false;
            int membercount = 0;
            int proxycount = 0;
            string division = "";

            if (!(requser.RoleIds.Contains(proxy)) && (enduser.RoleIds.Contains(corporateerID) || enduser.RoleIds.Contains(corporateerShieldID)))
            {

                if (Context.Channel.Id == recruitchat)
                {

                    while (true)
                    {

                        foreach (IRole role in rolelist)
                        {
                            /*
                             * Set the division tole to give
                             */
                            if (addrole == null)
                            {

                                if (role.Name.Contains("DL") || role.Name.Contains("DH"))
                                {

                                    if (requser.RoleIds.Contains(role.Id))
                                    {

                                        tagtogive = role;
                                        authorizedproxy = true;

                                    }

                                }
                            }
                            else
                            {

                                tagtogive = addrole;

                            }

                            /*
                             * If DL or DH tag is found
                             * Program will look for the correct Color role
                             */

                            if (tagtogive != null)
                            {


                                var split = tagtogive.Name.Split(' ');

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


                                    if (!(tagtogive.Name.Contains("DH")))
                                    {
                                        if (role.Name == division)
                                        {

                                            standardtag = role;

                                        }
                                    }
                                }


                                if (colortogive == null)
                                {
                                    XmlNode list = l1.SelectSingleNode("/colorConfig/attribute[@name='" + division.ToLower() + "']");
                                    if (list != null)
                                    {

                                        ulong id = Convert.ToUInt64(list.InnerText);

                                        if (role.Id == id)
                                        {

                                            colortogive = role;
                                        }
                                    }
                                }
                            }



                        }

                        if (proxytogive != null && managertogive != null && tagtogive != null && colortogive != null && standardtag != null)
                        {

                            break;
                        }
                        else if (proxytogive != null && managertogive != null && tagtogive != null && colortogive != null && (tagtogive.Name.Contains("DH")))
                        {
                            break;

                        }

                    }

                    if (tagtogive.Name.Contains("DH"))
                    {

                        membercount = 1;
                    }
                    else
                    {

                        membercount = CountMembers(standardtag, guildlist);
                        membercount = membercount / 50 + 1;

                    }

                    foreach (IUser u in guildlist)
                    {
                        IGuildUser g = u as IGuildUser;
                        if (g.RoleIds.Contains(proxytogive.Id) && g.RoleIds.Contains(tagtogive.Id))
                        {

                            proxycount = proxycount + 1;

                        }


                    }

                    if (addrole != null)
                    {

                        authorizedproxy = CheckDhAuthorisation(tagtogive, requser);

                    }
                    if (authorizedproxy == true)
                    {
                        if (proxycount < membercount)
                        {



                            await enduser.AddRoleAsync(colortogive);
                            await enduser.AddRoleAsync(tagtogive);

                            await Context.Channel.SendMessageAsync(enduser.Username + " has received: " + colortogive.Name + " , " + tagtogive);

                            await Context.Channel.SendMessageAsync("~assign removecolor \"" + adduser + "\" \"" + colortogive.Name + "\"" );
                            await Context.Channel.SendMessageAsync("~assign proxyroles \"" + adduser + "\"");

                        }
                        else
                        {

                            await Context.Channel.SendMessageAsync("You already have the limite of proxy's based on your member count");

                        }
                    }
                    else {

                        await Context.Channel.SendMessageAsync("You are not authorised to assign this proxy");

                    }


                    

                }
                else {

                    await Context.Channel.SendMessageAsync("You cannot use this here");

                }


            }
            else {

                await Context.Channel.SendMessageAsync("You are a proxy and cannot assign a proxy role to somebody else");

            }

            doc = null;


        }



        [Command("corp")]
        public async Task AssignCorporateerTag(IUser adduser, IRole language = null) {
            try
            {
                IRole endlanguage = null;
                IGuildUser enduser = adduser as IGuildUser;
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
            catch {

                await Context.Channel.SendMessageAsync("Language assignment failed. Moving one");
            }


            ulong corporateerID = 92031682596601856;
            IRole corporateerRole = Context.Guild.GetRole(corporateerID);
            
            AddRoleCorporateer(adduser, corporateerRole);


        }


        [Command("div")]
        public async Task AssignRoleCommand(IUser adduser, IRole role = null) {

            try
            {

                IGuildUser user = Context.User as IGuildUser;
                var userlist = await Context.Guild.GetUsersAsync();
                ulong DHSocialID = 261026068952121347;
                ulong DHSecurityID = 148505388235489282;
                ulong DHExploration = 261025367785996289;
                ulong DHSupport = 231457317030330368;
                ulong DHResources = 261025805033799681;
                ulong DHPR = 61026382950301698;
                ulong DHBusiness = 229989996151439361;
                //ulong proxy = 353283208625913857;
                //ulong corporateerID = 92031682596601856;

                /*Test Values
                 * 
                 *             
                 * */
                //ulong DHSocialID = 346550652207497216;

                string rolename = "";

                if (role != null)
                {
                    string rank = role.Name;
                    var splitted = rank.Split(' ');





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
                }


                if (user.RoleIds.Contains(DHSocialID))
                {


                    if (rolename == "Human Resources" || rolename == "Training" || rolename == "Diplomacy" || rolename == "Recruiter" || CheckSubDivision(Context.Guild.GetRole(DHSocialID), role,user) == true)
                    {

                        AddRoleDH(adduser, role);
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }

                }
                else if (user.RoleIds.Contains(DHSecurityID))
                {


                    if (rolename == "Ground Security" || rolename == "Space Security" || rolename == "CSOC" || rolename == "Repo" || CheckSubDivision(Context.Guild.GetRole(DHSecurityID), role, user) == true)
                    {

                        AddRoleDH(adduser, role);
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }


                }
                else if (user.RoleIds.Contains(DHSupport))
                {



                    if (rolename == "CSAR" || rolename == "Engineering" || CheckSubDivision(Context.Guild.GetRole(DHSupport), role, user) == true)
                    {

                        AddRoleDH(adduser, role);

                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }


                }
                else if (user.RoleIds.Contains(DHExploration))
                {

                    if (rolename == "Prospecting" || rolename == "Cartography" || rolename == "Research" || CheckSubDivision(Context.Guild.GetRole(DHExploration), role, user) == true)
                    {

                        AddRoleDH(adduser, role);
                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }

                }
                else if (user.RoleIds.Contains(DHResources))
                {

                    if (rolename == "Development" || rolename == "Extraction" || rolename == "Transport" || CheckSubDivision(Context.Guild.GetRole(DHResources), role, user) == true)
                    {

                        AddRoleDH(adduser, role);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");

                    }


                }
                else if (user.RoleIds.Contains(DHPR))
                {


                    if (rolename == "e-Sport" || rolename == "Media" || rolename == "Streamer" || CheckSubDivision(Context.Guild.GetRole(DHPR), role, user) == true)
                    {

                        AddRoleDH(adduser, role);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");

                    }

                }
                else if (user.RoleIds.Contains(DHBusiness))
                {


                    if (rolename == "Finance" || rolename == "Trade" || rolename == "Contracts" || CheckSubDivision(Context.Guild.GetRole(DHBusiness), role, user) == true)
                    {

                        AddRoleDH(adduser, role);

                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role");
                    }


                }
                else
                {

                    if (role == null)
                    {

                        AddRole(adduser, userlist);

                    }
                    else
                    {

                        AddRole(adduser, role, userlist);
                    }

                }



            }
            catch (Exception ex) {

                await Context.User.SendMessageAsync("Something went wrong: " +  ex.Message);


            }
            

        }


        [Command("removecolor")]
        public async Task RemoveCOlor(IUser adduser, IRole role) {

            if (Context.User.Id == Context.Client.CurrentUser.Id) {


                DeleteColors(adduser, role);
                await Context.Channel.SendMessageAsync("I removed the rest of the colors");

            }



        }

        [Command("proxyroles")]
        public async Task AssignProxyRoles(IUser adduser) {
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

                await user.AddRoleAsync(Context.Guild.GetRole(proxy));
                await user.AddRoleAsync(Context.Guild.GetRole(managerId));


                await Context.Channel.SendMessageAsync("I have added the needed proxy and manager role");

            }

        }




        private async void AddRoleCorporateer(IUser adduser, IRole role) {
            ulong recruiter = 376500752203644928;
            ulong recruitchat = 262861494360735744;
            IGuildUser user = Context.User as IGuildUser;
            IGuildUser finaluser = adduser as IGuildUser;
            ulong modchat = 110270291644526592;
            ulong managerId = 84900316213936128;
            ulong moderatorID = 84893161574391808;
            ulong boardID = 127310237018357760;
            ulong ceoID = 206089923911090177;
            ulong visitor = 96472823354118144;
            IRole visitorrole = Context.Guild.GetRole(visitor);
            
            IMessageChannel moderatorchat = await Context.Guild.GetChannelAsync(modchat) as IMessageChannel;

            if (Context.Channel.Id == recruitchat)
            {

                if (role.Id == managerId || role.Id == moderatorID || role.Id == boardID || role.Id == ceoID)
                {

                    await moderatorchat.SendMessageAsync("user: " + user.Username + " is trying to add the following role " + role.Name + " to " + adduser.Username);

                }
                else
                {


                    
                    if (user.RoleIds.Contains(recruiter))
                    {


                        await finaluser.AddRoleAsync(role);
                        await finaluser.RemoveRoleAsync(visitorrole);
                        await Context.Channel.SendMessageAsync("I have assigned: " + role.Name + " to " + finaluser.Username);





                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("You are not a recruiter and cannot assign a Corporateer tag");
                    }


                }


            }
            else
            {

                await Context.Channel.SendMessageAsync("You cannot use this command in this channel");

            }





        }

        private async void AddRoleDH(IUser adduser, IRole role) {

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

                if (role.Id == managerId || role.Id == moderatorID || role.Id == boardID || role.Id == ceoID)
                {

                    await moderatorchat.SendMessageAsync("user: " + user.Username + " is trying to add the following role " + role.Name + " to " + adduser.Username);

                }
                else
                {


                    IGuildUser finaluser = adduser as IGuildUser;
                    if (finaluser.RoleIds.Contains(corporateerID) || finaluser.RoleIds.Contains(corporateerShieldID))
                    {
                        

                            await finaluser.AddRoleAsync(role);
                            await Context.Channel.SendMessageAsync("I have assigned: " + role.Name + " to " + finaluser.Username);
                        




                    }
                    else
                    {

                        await Context.Channel.SendMessageAsync("This person has no corporateer tag. Please contact a moderator or Recruiter");
                    }


                }


            }
            else
            {

                await Context.Channel.SendMessageAsync("You cannot use this command in this channel");

            }



        }

        private async void AddRole(IUser adduser, IReadOnlyCollection<IGuildUser> userlist) {

            IGuildUser user = Context.User as IGuildUser;
            IGuildUser enduser = adduser as IGuildUser;
            ulong modchat = 110270291644526592;
            ulong managerId = 84900316213936128;
            ulong moderatorID = 84893161574391808;
            ulong boardID = 127310237018357760;
            ulong ceoID = 206089923911090177;
            ulong corporateerID = 92031682596601856;
            ulong corporateerShieldID = 315095427361800192;
            ulong recruitchat = 262861494360735744;

            /*
             * Test Values
             */
            //ulong recruitchat = 328162927653814273;
            //ulong corporateerID = 326381093081317376;



            if (Context.Channel.Id == recruitchat)
            {

                if (enduser.RoleIds.Contains(corporateerID) || enduser.RoleIds.Contains(corporateerShieldID))
                {

                    IRole togive = null;
                    IRole DLRole = null;
                    int numberDLRole = 0;


                    foreach (ulong id in user.RoleIds)
                    {

                        IRole role = Context.Guild.GetRole(id);
                        if (role.Name.Contains("DL"))
                        {
                            numberDLRole = numberDLRole + 1;
                            DLRole = role;


                        }


                    }
                    string splittedname = DLRole.Name;
                    var splitted = splittedname.Split(' ');

                    if (splitted.Length > 2)
                    {
                        splittedname = splitted[1] + " " + splitted[2];

                    }
                    else
                    {

                        splittedname = splitted[1];
                    }


                    foreach (IRole r in Context.Guild.Roles)
                    {
                        if (r.Name == splittedname)
                        {

                            togive = r;
                        }



                    }

                    await enduser.AddRoleAsync(togive);
                    await Context.Channel.SendMessageAsync("I have assigned: " + togive.Name + " to " + enduser.Username);


                }
                else
                {


                    await Context.Channel.SendMessageAsync("This person has no corporateer tag. Please contact a moderator or Recruiter");

                }
            }
            else
            {

                await Context.Channel.SendMessageAsync("You cannot use this here");

            }




        }


        private async void AddRole(IUser adduser, IRole role, IReadOnlyCollection<IGuildUser> userlist) {
            IGuildUser user = Context.User as IGuildUser;
            ulong modchat = 110270291644526592;
            ulong managerId = 84900316213936128;
            ulong moderatorID = 84893161574391808;
            ulong boardID = 127310237018357760;
            ulong ceoID = 206089923911090177;
            ulong corporateerID = 92031682596601856;
            ulong corporateerShieldID = 315095427361800192;
            ulong recruitchat = 262861494360735744;

            /*
             * Test values
             * */
            //ulong recruitchat = 328162927653814273;
            //ulong corporateerID = 326381093081317376;

            ulong DlID = 0;
            var allroles = Context.Guild.Roles;
            bool authorized = true;
            bool checksubdiv = false;
            string rank = role.Name;
            var splitted = rank.Split(' ');
            string rolename = "";

            try {

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


                if (DlID == 0)
                {

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

                if (Context.Channel.Id == recruitchat) {

                    if (role.Id == managerId || role.Id == moderatorID || role.Id == boardID || role.Id == ceoID)
                    {

                        await moderatorchat.SendMessageAsync("user: " + user.Username + " is trying to add the following role " + role.Name + " to " + adduser.Username);

                    }
                    else {

                        if (authorized == true) {
                            IGuildUser finaluser = adduser as IGuildUser;
                            if (finaluser.RoleIds.Contains(corporateerID) || finaluser.RoleIds.Contains(corporateerShieldID))
                            {
                                if ((user.RoleIds.Contains(DlID) && checksubdiv == false) || CheckSubDivision(Context.Guild.GetRole(DlID), role, user) == true)
                                {

                                    await finaluser.AddRoleAsync(role);
                                    await Context.Channel.SendMessageAsync("I have assigned: " + role.Name + " to " + finaluser.Username);
                                }
                                else {

                                    await Context.Channel.SendMessageAsync("You do not have the permissions to assign this role.");

                                }




                            }
                            else {

                                await Context.Channel.SendMessageAsync("This person has no corporateer tag. Please contact a moderator or Recruiter");
                            }
                        } else
                        {
                            await Context.Channel.SendMessageAsync("Permission Denied! If you are a Proxy, your superior is online and you cannot act as Proxy!" +
                                    Environment.NewLine + "If you are a Proxy but are Authorized to recruite by your superior, please check with your superior for bot Authorization." +
                                    Environment.NewLine + "In any other cases, please check with developer");

                        }


                    }


                } else
                {

                    await Context.Channel.SendMessageAsync("You cannot use this command in this channel");

                }
            }
            catch (Exception e) {

                await Context.Channel.SendMessageAsync("Something went wrong during processing. Here are the inputs: " + adduser + " " + role + Environment.NewLine + "Here is the log: " + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine + "Please leave this log for Slimey to analyze");

            }


        }


        private void DeleteColors(IUser adduser, IRole Nottoremove) {
            IGuildUser user = adduser as IGuildUser;
            var guildlist = Context.Guild.Roles;
            XmlDocument doc = new XmlDocument();
            doc.Load("ColorConfig.xml");
            XmlNode l1 = doc.FirstChild;
            XmlNodeList list = l1.SelectNodes("/colorConfig/attribute");

            foreach (XmlNode x in list) {
                ulong id = Convert.ToUInt64(x.InnerText);
                foreach (IRole role in guildlist) {
                    if (role.Id == id && role.Id != Nottoremove.Id) {

                        user.RemoveRoleAsync(role);

                    }


                }


            }


        }


        private Boolean CheckDLorDHTag(ICommandContext commandcontext, IUser adduser) {
            Boolean result = false;

            IGuildUser user = adduser as IGuildUser;
            var rolesID = user.RoleIds;
            var roles = commandcontext.Guild.Roles;
            foreach (IRole role in roles) {
                if (rolesID.Contains(role.Id)) {
                    if (role.Name.Contains("DL") || role.Name.Contains("DH")) {

                        result = true;
                    }

                }


            }
            


            return result;
        }


        private Boolean CheckDhAuthorisation(IRole role, IUser enduser) {
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


        private int CountMembers(IRole role, IReadOnlyCollection<IGuildUser> guildlist) {
            int membercount = 0;


            foreach (IGuildUser u in guildlist)
                {
                if (u.RoleIds.Contains(role.Id))
                {
                    membercount = membercount + 1;
                }

                }
            
            


            return membercount;
        }

        private Boolean CheckSubDivision(IRole managerRole, IRole subdivRole, IUser user) {
            Boolean authorized = false;
            IGuildUser enduser = user as IGuildUser;
            XmlDocument doc = new XmlDocument();
            doc.Load("SubDivisions.xml");
            XmlNode first = doc.FirstChild;
            XmlNode list = null;
            string manager = "";
            if (managerRole != null)
            {

                manager = managerRole.Name.Replace(" ", String.Empty);
                list = doc.SelectSingleNode("/list/" + manager + "[@subdivision='" + subdivRole.Name + "']");
            }

            

            if (list != null)
            {

                authorized = true;

            }
            else {

                foreach (ulong id in enduser.RoleIds) {
                    IRole role = Context.Guild.GetRole(id);
                    string rolename = role.Name.Replace(" ", String.Empty);
                    list = doc.SelectSingleNode("/list/" + rolename + "[@subdivision='" + subdivRole.Name + "']");

                    if (list != null)
                    {

                        authorized = true;

                    }

                }


            }


            return authorized;
        }






    }
}
