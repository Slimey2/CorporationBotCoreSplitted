using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using CorporationBotCoreGeneral;
using System.Xml;

namespace CorporationBotCoreGeneral
{
    [Group("subdiv")]
    public class SubDivisionHandling : ModuleBase
    {

        [Command("add")]
        public async Task AddSubdivision(IRole managerRole, IRole subdivRole) {
            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;
            IGuildUser finaluser = Context.User as IGuildUser;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {


                    XmlDocument doc = new XmlDocument();
                    doc.Load("SubDivisions.xml");
                    XmlNode first = doc.FirstChild;
                    string manager = managerRole.Name.Replace(" ", String.Empty);


                    XmlNode list = doc.SelectSingleNode("/list/" + manager + "[@subdivision='" + subdivRole.Name + "']");

                    if (list == null)
                    {
                        XmlElement member = doc.CreateElement(manager);

                        XmlAttribute attribute = doc.CreateAttribute("subdivision");
                        attribute.InnerText = subdivRole.Name;
                        member.Attributes.Append(attribute);

                        first.AppendChild(member);


                    }

                    doc.Save("SubDivisions.xml");

                    await Context.Channel.SendMessageAsync("Following sub division " + subdivRole.Name + " has been added under " + managerRole.Name);
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

        [Command("delete")]
        public async Task RemoveSubdivision(IRole managerRole, IRole subdivRole) {

            ulong mod = 110270291644526592;
            ulong moderatorID = 84893161574391808;
            ulong ceoID = 206089923911090177;
            IGuildUser finaluser = Context.User as IGuildUser;

            if (finaluser.RoleIds.Contains(moderatorID) || finaluser.RoleIds.Contains(ceoID))
            {

                if (Context.Channel.Id == mod)
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load("SubDivisions.xml");
                    XmlNode first = doc.FirstChild;
                    string manager = managerRole.Name.Replace(" ", String.Empty);


                    XmlNode list = doc.SelectSingleNode("/list/" + manager + "[@subdivision='" + subdivRole.Name + "']");

                    if (list != null)
                    {
                        first.RemoveChild(list);


                    }


                    doc.Save("SubDivisions.xml");

                    await Context.Channel.SendMessageAsync("Following sub division " + subdivRole.Name + " has been removed under " + managerRole.Name);

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
