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
    [Group("influence")]
    public class LoginRequest : ModuleBase
    {

        [Command("login")]
        public async Task RequestLogin() {

            //TEst Values
            //IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);
            //ulong corporateerID = 326381093081317376;
            //ulong modchat = 337141993362685952;


            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);

            IGuildUser enduser = await guild.GetUserAsync(Context.User.Id) as IGuildUser;
            SocketUser user2 = await guild.GetUserAsync(Context.User.Id) as SocketUser;
            IDMChannel channel = await user2.GetOrCreateDMChannelAsync();

            if (Activation.httprequest == true)
            {
                string requesttype = "login";
                ulong corporateerID = 92031682596601856;
                ulong corporateerShieldID = 315095427361800192;
                ulong modchat = 110270291644526592;



                List<String> division = new List<string>();

                XmlDocument doc = new XmlDocument();
                doc.Load("ColorConfig.xml");
                XmlNode l1 = doc.FirstChild;

                foreach (ulong l in enduser.RoleIds)
                {
                    IRole role = guild.GetRole(l);
                    XmlNode tag = l1.SelectSingleNode("/colorConfig/attribute[@name='" + role.Name.ToLower() + "']");

                    if (tag != null)
                    {

                        division.Add(role.Name);

                    }

                }

                IMessageChannel moderatorchat = await guild.GetChannelAsync(modchat) as IMessageChannel;

                SendHttpRequest request = new SendHttpRequest();


                if (enduser.RoleIds.Contains(corporateerID) || enduser.RoleIds.Contains(corporateerShieldID))
                {
                    if (enduser.Nickname != null)
                    {
                        await channel.SendMessageAsync(request.Sendhttprequest(enduser.Nickname, division,requesttype));
                    }
                    else {

                        await channel.SendMessageAsync(request.Sendhttprequest(enduser.Username, division, requesttype));

                    }
                }
                else
                {

                    await moderatorchat.SendMessageAsync("User " + enduser.Username + " is trying to request a login for the influence system but is not a Corporateer!");

                }
            }
            else {

                await channel.SendMessageAsync("You cannot use this command yet. Please wait official authorisation. Thank you");


            }
            
            

        }

        [Command("get")]
        public async Task getTribute() {
            //TEst Values
            //IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);
            //ulong corporateerID = 326381093081317376;
            //ulong modchat = 337141993362685952;


            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);

            IGuildUser enduser = await guild.GetUserAsync(Context.User.Id) as IGuildUser;
            SocketUser user2 = await guild.GetUserAsync(Context.User.Id) as SocketUser;
            IDMChannel channel = await user2.GetOrCreateDMChannelAsync();

            if (Activation.httprequest == true)
            {
                string requesttype = "usertribute";
                ulong corporateerID = 92031682596601856;
                ulong corporateerShieldID = 315095427361800192;
                ulong modchat = 110270291644526592;


                List<String> division = new List<string>();

                IMessageChannel moderatorchat = await guild.GetChannelAsync(modchat) as IMessageChannel;

                SendHttpRequest request = new SendHttpRequest();


                if (enduser.RoleIds.Contains(corporateerID) || enduser.RoleIds.Contains(corporateerShieldID))
                {
                    if (enduser.Nickname != null)
                    {
                        await channel.SendMessageAsync(request.Sendhttprequest(enduser.Nickname, division, requesttype));
                    }
                    else
                    {

                        await channel.SendMessageAsync(request.Sendhttprequest(enduser.Username, division,requesttype));

                    }
                }
                else
                {

                    await moderatorchat.SendMessageAsync("User " + enduser.Username + " is trying to request some data from the influence system while not authorised!");

                }
            }
            else
            {

                await channel.SendMessageAsync("You cannot use this command yet. Please wait official authorisation. Thank you");


            }


        }

        [Command("all")]
        public async Task getInfluence()
        {

            /*
               * Test Values
               * 
               * */

            //ulong corporateerID = 326381093081317376;
            //IGuild guild = await Context.Client.GetGuildAsync(326378973934256129);
            //ulong modchat = 337141993362685952;



            IGuild guild = await Context.Client.GetGuildAsync(82021499682164736);
            IGuildUser enduser = await guild.GetUserAsync(Context.User.Id) as IGuildUser;
            SocketUser user2 = await guild.GetUserAsync(Context.User.Id) as SocketUser;
            IDMChannel channel = await user2.GetOrCreateDMChannelAsync();




            if (Activation.httprequest == true)
            {
                string requesttype = "allinfluences";
                ulong corporateerID = 92031682596601856;
                ulong corporateerShieldID = 315095427361800192;
                ulong modchat = 110270291644526592;


                List<String> division = new List<string>();

                IMessageChannel moderatorchat = await guild.GetChannelAsync(modchat) as IMessageChannel;

                SendHttpRequest request = new SendHttpRequest();


                if (enduser.RoleIds.Contains(corporateerID) || enduser.RoleIds.Contains(corporateerShieldID))
                {
                    if (enduser.Nickname != null)
                    {
                        await channel.SendMessageAsync(request.Sendhttprequest(enduser.Nickname, division, requesttype));
                    }
                    else
                    {

                        await channel.SendMessageAsync(request.Sendhttprequest(enduser.Username, division, requesttype));

                    }
                }
                else
                {

                    await moderatorchat.SendMessageAsync("User " + enduser.Username + " is trying to request some data from the influence system while not authorised!");

                }
            }
            else
            {

                await channel.SendMessageAsync("You cannot use this command yet. Please wait official authorisation. Thank you");


            }


        }


    }
}
