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

namespace CorporationBotGeneral
{
     public class Info : ModuleBase
    {
        
        [Command("info")]
        public async Task InfoCommand(String s = "General") {
            s = s.ToLower();
            if (s == "General" || s == "") {

                infoCorporation();
            }
            else
            {


                if (s == "social")
                {
                    string[] divs = new String[] { "DL Human Resources", "DL Training", "DL Diplomacy" };
                    infoDivision(divs, s);

                }
                else if (s == "security")
                {
                    string[] divs = new String[] { "DL CSOC", "DL Ground Security", "DL Space Security", "DL Repo" };
                    infoDivision(divs, s);

                }
                else if (s == "support")
                {
                    string[] divs = new String[] { "DL CSAR", "DL Engineering", "DL IT" };
                    infoDivision(divs, s);

                }
                else if (s == "exploration")
                {
                    string[] divs = new String[] { "DL Prospecting", "DL Cartography", "DL Research" };
                    infoDivision(divs, s);

                }
                else if (s == "resources")
                {
                    string[] divs = new String[] { "DL Development", "DL Extraction", "DL Transport" };
                    infoDivision(divs, s);

                }
                else if (s == "public relations")
                {
                    string[] divs = new String[] { "DL e-Sports", "DL Media" };
                    infoDivision(divs, s);

                }
                else if (s == "business")
                {
                    string[] divs = new String[] { "DL Finance", "DL Trade", "DL Contracts" };
                    infoDivision(divs, s);

                }
                else if (s != "business" && s != "public relations" && s != "resources" && s != "exploration" && s != "support" && s != "security" && s != "social")
                {
                    infodivision(s);
                }
                

            }


            


        }



        private async void infoCorporation()
        {
            IGuildUser user = Context.User as IGuildUser;
            try
            {
                IGuild corporation = await Context.Client.GetGuildAsync(82021499682164736);
                IRole CEO = corporation.GetRole(206089923911090177);
                IRole BOARD = corporation.GetRole(127310237018357760);
                ulong ceo = 206089923911090177;
                ulong board = 127310237018357760;
                ulong corporateer = 92031682596601856;


                List<EmbedFieldBuilder> fieldslist = new List<EmbedFieldBuilder>();
                var footer = new EmbedFooterBuilder { Text = "The Corporation™ | Any abuse/hacking/breaking of those commands will not be tolerated and could result in a permanent ban" };
                var author = new EmbedAuthorBuilder { Name = "The Corporation", Url = "https://forum.thecorporateer.com/" };
                var ember = new EmbedBuilder { Title = "General information", Description = "This is the information about The Corporation " + Environment.NewLine + "If you need any extra information about The Corporation, you can contact Board,Managers,Moderators or Human Resources representatives", ThumbnailUrl = "https://media.discordapp.net/attachments/229700730611564545/367320352000573451/CorpLogo.png?width=665&height=610", Author = author, Fields = fieldslist, Footer = footer };
                

                

                
                var userlist = await Context.Guild.GetUsersAsync();
                
                
                foreach (IGuildUser u in userlist)
                {

                    if (u.RoleIds.Contains(ceo))
                    {
                        fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = "CEO", Value = u.Username });
                        

                    }

                }

                string users = "";
                foreach (IGuildUser u in userlist)
                {

                    if (u.RoleIds.Contains(board))
                    {
                        users = users + u.Username + Environment.NewLine;
                        

                    }

                }
                fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = "Board", Value = users });



                
                if (user.RoleIds.Contains(corporateer))
                {
                    fieldslist.Add(new EmbedFieldBuilder { IsInline = false, Name = "Additional info", Value = "To login on the website use the following login information on the first prompt: user= corp ; pw: bettertomorrow" });
                    
                }
                await Context.User.SendMessageAsync("", false, ember);

            }
            catch(Exception ex)
            {

                await user.SendMessageAsync("Log Infodivision(general) function: Hello there, if you send a message direct to the Bot please use the command in a discord chat." + Environment.NewLine
                   + "If you did use the command in a chatchannel but you still have an error please check the spelling of you command and try again. If the error still persist please contact my creator Slimey2");


            }


        }


        private async void infodivision(string s)
        {
            IGuildUser user = Context.User as IGuildUser;
            try
            {
                string url = "";
                var splited = s.Split(' ');
                XmlDocument doc = new XmlDocument();
                doc.Load("ImageConfig.xml");
                XmlNode l1 = doc.FirstChild;
                XmlNodeList list = l1.ChildNodes;
                foreach (XmlNode node in list) {
                    if (node.Name == splited[0]) {
                        url = node.InnerText;

                    }
                }



                List<EmbedFieldBuilder> fieldslist = new List<EmbedFieldBuilder>();
                var footer = new EmbedFooterBuilder { Text = "The Corporation™ | Any abuse/hacking/breaking of those commands will not be tolerated and could result in a permanent ban" };
                var author = new EmbedAuthorBuilder { Name = "The Corporation", Url = "https://forum.thecorporateer.com/" };
                var ember = new EmbedBuilder { Title = "Division information", Description = "This is the information about The Corporation " + Environment.NewLine + "If you need any extra information about this division, you can contact the DH, the DL or the Proxy's", ThumbnailUrl = "https://media.discordapp.net/attachments/229700730611564545/367320352000573451/CorpLogo.png?width=665&height=610", Author = author, Fields = fieldslist, Footer = footer, ImageUrl= url };
                

                var userlist = await Context.Guild.GetUsersAsync();
                var roles = Context.Guild.Roles;
                ulong roleDL = 0;
                ulong assistant = 353283208625913857;
                foreach (IRole r in roles)
                {
                    if (r.Name == "DL " + s)
                    {
                        roleDL = r.Id;

                    }

                }

                foreach (IGuildUser u in userlist)
                {
                    if (u.RoleIds.Contains(roleDL) && !(u.RoleIds.Contains(assistant)))
                    {
                        fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = "Division Leader", Value = u.Username });
                        

                    }

                }
                string users = "";
                foreach (IGuildUser u in userlist)
                {

                    if (u.RoleIds.Contains(roleDL) && (u.RoleIds.Contains(assistant)))
                    {
                        users = users + u.Username + Environment.NewLine;


                    }

                }
            if (users != "")
            {
                fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = "Proxy", Value = users });
            }
                await Context.User.SendMessageAsync("", false, ember);
                doc = null;
            }
            catch(Exception ex)
            {

                await Context.User.SendMessageAsync("Log Infodivision(division) function: Hello there, if you send a message direct to the Bot please use the command in a discord chat." + Environment.NewLine
                    + "If you did use the command in a chatchannel but you still have an error please check the spelling of you command and try again. If the error still persist please contact my creator Slimey2" +Environment.NewLine + ex.Message);


            }

            

        }




        private async void infoDivision(string[] divisions, string s)
        {
            try
            {

                string url = "";
            var splited = s.Split(' ');
            XmlDocument doc = new XmlDocument();
            doc.Load("ImageConfig.xml");
            XmlNode l1 = doc.FirstChild;
            XmlNodeList list = l1.ChildNodes;
            foreach (XmlNode node in list)
            {
                if (node.Name == splited[0])
                {
                    url = node.InnerText;

                }
            }

            List<EmbedFieldBuilder> fieldslist = new List<EmbedFieldBuilder>();
            var footer = new EmbedFooterBuilder { Text = "The Corporation™ | Any abuse/hacking/breaking of those commands will not be tolerated and could result in a permanent ban" };
            var author = new EmbedAuthorBuilder { Name = "The Corporation", Url = "https://forum.thecorporateer.com/" };
            var ember = new EmbedBuilder { Title = "Department information", Description = "This is the information about The Corporation " + Environment.NewLine + "If you need any extra information about this division, you can contact the DH, the DL or the Proxy's", ThumbnailUrl = "https://media.discordapp.net/attachments/229700730611564545/367320352000573451/CorpLogo.png?width=665&height=610", Author = author, Fields = fieldslist, Footer = footer, ImageUrl = url };

            if (s == "PR")
            {

                s = "Public Relations";

            }
            ulong roleDH = 0;
            ulong assistant = 353283208625913857;
            List<IRole> divRoles = new List<IRole>();

            var roles = Context.Guild.Roles;
            var userlist = await Context.Guild.GetUsersAsync();
            foreach (IRole r in roles) {
                if (r.Name == "DH " + s) {
                    roleDH = r.Id;

                }

            }


            foreach (string x in divisions) {
                foreach (IRole r in roles) {
                    if (r.Name == x) {

                        divRoles.Add(r);
                    }


                }
            }


                
                string tobesend = "";
                bool DHfiled = false;

                foreach (IGuildUser u in userlist)
                {
                    
                    //Role roleDH = e.Server.FindRoles("DH " + e.Args[0].ToString()).First();
                    if (u.RoleIds.Contains(roleDH) && DHfiled == false)
                    {

                        //await e.Channel.SendMessage("Department Head for this Departement is: " + u.Name);
                        
                        fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = "Department Head", Value = u.Username });
                        DHfiled = true;
                    }

                }


                foreach (IRole a in divRoles)
                {

                    //await e.Channel.SendMessage("Division: " + divi);
                    tobesend = tobesend + "Division: " + a.Name + Environment.NewLine;
                //fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = a.Name, Value = " " });
                string division = a.Name;
                var splitdiv = division.Split(' ');

                    foreach (IGuildUser u in userlist)
                    {
                        if (u.RoleIds.Contains(a.Id) && !(u.RoleIds.Contains(assistant)))
                        {
                           
                            fieldslist.Add(new EmbedFieldBuilder { IsInline = false, Name = splitdiv[1] + " Leader", Value = u.Username });

                        }


                    }
                    string users = "";
                    foreach (IGuildUser u in userlist)
                    {
                         if (u.RoleIds.Contains(a.Id) && (u.RoleIds.Contains(assistant)))
                        {
                                                     
                            users = users + u.Username + Environment.NewLine;
                        }

                    }
                if (users != "")
                {
                    fieldslist.Add(new EmbedFieldBuilder { IsInline = true, Name = splitdiv[1] + " Proxy", Value = users });
                }
                }


                await Context.User.SendMessageAsync("", false, ember);

                doc = null;

            }
            catch(Exception ex)
            {

                await Context.User.SendMessageAsync("Log InfoDivision(Department) function: If you send a message direct to the Bot please use the command in a discord chat." + Environment.NewLine
                    + "If you did use the command in a chatchannel but you still have an error please check the spelling of you command and try again. If the error still persist please contact Slimey2");


            }

        }






    }
        
}
