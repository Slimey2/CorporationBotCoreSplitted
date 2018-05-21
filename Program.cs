using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using CorporationBotCoreGeneral;
using System.Xml;

namespace CorporationBotGeneral
{
    class Program
    {

        private CommandService commands;
        private DiscordSocketClient client;
        private IServiceProvider services;
        private DatabaseArchiving archive;
        private DiscordSocketConfig discordconfig;
        private String lastgame = "";

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            try
            {
                discordconfig = new DiscordSocketConfig();
                discordconfig.MessageCacheSize = 10;
                client = new DiscordSocketClient(discordconfig);
                commands = new CommandService();

                XmlDocument doc = new XmlDocument();
                doc.Load("BotConfig.xml");
                XmlNode l1 = doc.FirstChild;
                Activation.stats = Convert.ToBoolean(l1.SelectSingleNode("/BotConfig/Stats").InnerText);

                Activation.moddeletation = Convert.ToBoolean(l1.SelectSingleNode("/BotConfig/deleteConfig").InnerText);

                Activation.httprequest = Convert.ToBoolean(l1.SelectSingleNode("/BotConfig/httpConfig").InnerText);



                string token = "MzI2MDcwNzU2NTAyOTI5NDA4.DXXq2Q.y8KQEUUR_-6ZJDNv7ifUmoD2R-4";
                //string token = "Mzc1Mjk3NTAzOTY2MjY1MzQ2.DQQYrQ.VF4NjvgpQCU-ksLlZF4on7zPIZg";
                services = new ServiceCollection()
                        .BuildServiceProvider();
                
                archive = new DatabaseArchiving();
                Activation.archiveobject = archive;
                await InstallCommands();

                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
                
                await Task.Delay(-1);
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
            }
        }

            public async Task InstallCommands()
    {

            // Hook the MessageReceived Event into our Command Handler
            client.MessageReceived += HandleCommand;
            client.GuildMemberUpdated += StreamingMethode;
            client.UserJoined += UserJoinedMethod;
            client.ReactionAdded += ReactionAddedMethod;
            
            client.MessageDeleted += MessageDeletedMethod;
           // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
    }

    public async Task HandleCommand(SocketMessage messageParam)
    {

            XmlDocument doc = new XmlDocument();
            doc.Load("BotConfig.xml");
            XmlNode l1 = doc.FirstChild;
            XmlNodeList list = l1.ChildNodes;
            string active = list.Item(1).InnerText;
            
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
         
        if (message == null) return;
        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;

            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('~', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
            {
                                if (message.ToString().Contains("[moderator]") || message.ToString().Contains("[Moderator]"))
                                {
                                    archive.InsertModeration(message);
                                }
                                else if (Activation.stats == true)
                                {

                                    archive.InsertStats(message);
                                }

                                
            
            
                 
            }
            else
            {
                if (Activation.active == false && !(message.Content.ToLower().Contains("activatecommands")))
                {
                    ISocketMessageChannel channel = message.Channel;
                    await channel.SendMessageAsync("Commands have been deactivated. Please so not attempt again");


                }
                else
                {
                    // Create a Command Context
                    var context = new CommandContext(client, message);
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed successfully)
                
                var result = await commands.ExecuteAsync(context, argPos, services);

                
                if (!result.IsSuccess)
                    {

                        if (context.Channel.Id == 262861494360735744 || context.Channel.Id == 328162927653814273)
                        {

                            await context.Channel.SendMessageAsync(result.ErrorReason);
                        }
                    }
                else if (result.IsSuccess)
                    {

                    
                    
                    }
                }
            }
        }

        public async Task ReactionAddedMethod(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction) {
            //Test Values
            //ulong corporation = 326378973934256129;


            ulong corporation = 82021499682164736;

            IMessageChannel finalchannel = client.GetGuild(corporation).GetChannel(channel.Id) as IMessageChannel;
            IUserMessage message = await finalchannel.GetMessageAsync(cache.Id) as IUserMessage;
            if (reaction.Emote.Name.Contains("Drama") || reaction.Emote.Name.Contains("drama")) {
                


                try
                {

                    archive.InsertDrama(message);
                    await message.AddReactionAsync(new Emoji("\uD83D\uDC6E"));
                }
                catch (Exception ex)
                {
                    await finalchannel.SendMessageAsync(ex.Message);
                }
            } else if (reaction.Emote.Name.Contains("1INF")) {

                IGuildUser reactionuser = client.GetGuild(corporation).GetUser(reaction.UserId);
                IGuildUser messageuser = client.GetGuild(corporation).GetUser(message.Author.Id);
                string user1 = "";
                string user2 = "";

                if (messageuser.Nickname != null) {
                    user1 = messageuser.Nickname;

                }
                else
                {

                    user1 =messageuser.Username;
                }


                if (reactionuser.Nickname != null)
                {
                    user2 = reactionuser.Nickname;

                }
                else
                {

                    user2 = reactionuser.Username;
                }


                SendHttpRequest request = new SendHttpRequest();

                await reactionuser.SendMessageAsync(request.SendInfluence(user2, user1, 1, "INFLUENCE"));

            } else if (reaction.Emote.Name.Contains("5INF")) {

                IGuildUser reactionuser = client.GetGuild(corporation).GetUser(reaction.UserId);
                IGuildUser messageuser = client.GetGuild(corporation).GetUser(message.Author.Id);
                string user1 = "";
                string user2 = "";

                if (messageuser.Nickname != null)
                {
                    user1 = messageuser.Nickname;

                }
                else
                {

                    user1 = messageuser.Username;
                }


                if (reactionuser.Nickname != null)
                {
                    user2 = reactionuser.Nickname;

                }
                else
                {

                    user2 = reactionuser.Username;
                }


                SendHttpRequest request = new SendHttpRequest();

                await reactionuser.SendMessageAsync(request.SendInfluence(user2, user1, 5, "INFLUENCE"));




            } else if (reaction.Emote.Name.Contains("10INF")) {


                IGuildUser reactionuser = client.GetGuild(corporation).GetUser(reaction.UserId);
                IGuildUser messageuser = client.GetGuild(corporation).GetUser(message.Author.Id);
                string user1 = "";
                string user2 = "";

                if (messageuser.Nickname != null)
                {
                    user1 = messageuser.Nickname;

                }
                else
                {

                    user1 = messageuser.Username;
                }


                if (reactionuser.Nickname != null)
                {
                    user2 = reactionuser.Nickname;

                }
                else
                {

                    user2 = reactionuser.Username;
                }


                SendHttpRequest request = new SendHttpRequest();

                await reactionuser.SendMessageAsync(request.SendInfluence(user2, user1, 10, "INFLUENCE"));



            }
            

 

        }



        public async Task MessageDeletedMethod(Cacheable<IMessage, ulong> cache, ISocketMessageChannel channel)
        {

            if (Activation.moddeletation == true)
            {
                if (cache.Value != null)
                {
                    ulong corporation = 326378973934256129;
                    IMessageChannel finalchannel = client.GetGuild(corporation).GetChannel(channel.Id) as IMessageChannel;
                    IMessage message = cache.Value;
                    try
                    {

                        archive.InsertLogging(message);

                    }
                    catch (Exception ex)
                    {
                        await finalchannel.SendMessageAsync(ex.Message);
                    }
                }
            }


        }




        public async Task UserJoinedMethod(SocketGuildUser arg) {
            ulong visitor = 96472823354118144;
            ulong corporation = 82021499682164736;
            var roles = client.GetGuild(corporation).Roles;
            IRole visitorRole = null;

            string url = "";
            XmlDocument doc = new XmlDocument();
            doc.Load("ImageConfig.xml");
            XmlNode l1 = doc.FirstChild;
            XmlNodeList list = l1.ChildNodes;
            foreach (XmlNode node in list)
            {
                if (node.Name == "visitor")
                {
                    url = node.InnerText;

                }
            }


            foreach (IRole r in roles) {
                if (r.Id == visitor) {
                    visitorRole = r;
                }

            }

            await arg.AddRoleAsync(visitorRole);
            List<EmbedFieldBuilder> fieldslist = new List<EmbedFieldBuilder>();
            var footer = new EmbedFooterBuilder { Text = "The Corporation™ | Any abuse/hacking/breaking of those commands will not be tolerated and could result in a permanent ban" };
            var author = new EmbedAuthorBuilder { Name = "The Corporation", Url = "https://forum.thecorporateer.com/" };
            var ember = new EmbedBuilder { Title = "General information", Description = "Welcome on the discord of The Corporation " + Environment.NewLine + "Here is some topics that might be usefull", ThumbnailUrl = "https://media.discordapp.net/attachments/229700730611564545/367320352000573451/CorpLogo.png?width=665&height=610", Author = author, Fields = fieldslist, Footer = footer };
            fieldslist.Add(new EmbedFieldBuilder { IsInline = false, Name = "Full Registration", Value = "Post your RSI handle in #registration and post your Discord handle (<name>#<numbers>) in the following spectrum post: https://robertsspaceindustries.com/spectrum/community/CORP/forum/51550/thread/registration-1" });
            fieldslist.Add(new EmbedFieldBuilder { IsInline = false, Name = "Diplomacy", Value = "If you are a diplomat, I wish you most welcome. Please contact Weyland, Mattari or any Board member (users with black names) to start our diplomatic relations." + Environment.NewLine + "You are also welcome to introduce yourself in #lobby chat" });
            fieldslist.Add(new EmbedFieldBuilder { IsInline = false, Name = "Questions", Value = "If you have any questions, please ask them in #lobby chat. Our fellow corporateers will gladly help you out." + Environment.NewLine + "A Recruiter will contact you to help you through the process" });


            await arg.SendMessageAsync("", false, ember);




        }

        public async Task StreamingMethode(SocketGuildUser olduser, SocketGuildUser Newuser) {
            ulong streamingchannel = 368481614340292623;
            ulong corporation = 82021499682164736;
            IMessageChannel channel = client.GetGuild(corporation).GetChannel(streamingchannel) as IMessageChannel;
            var roles = olduser.Roles;
            bool post = false;
            foreach (SocketRole r in roles) {
                if (r.Id == 308356281050071040 && lastgame != Newuser.Game.Value.Name) {
                    post = true;

                }

            }

            if (Newuser.Game.HasValue == true && post == true) {
                Game game = Newuser.Game.Value;
                if (game.StreamType.ToString() == "Twitch") {

                    await channel.SendMessageAsync("Streamer " + Newuser.Username + " just went live! Playing " + game.Name + "at: " + game.StreamUrl);
                    lastgame = Newuser.Game.Value.Name;

                }

            }
 /*           
            if (Newuser.Status.ToString() == "Offline") {

                XmlDocument doc = new XmlDocument();
                doc.Load("LastSeen.xml");
                XmlNode first = doc.FirstChild;

                XmlNode list = doc.SelectSingleNode("/list/" + Newuser.Username);

                if (list == null) {
                    XmlElement member = doc.CreateElement(Newuser.Username);

                    XmlAttribute attribute = doc.CreateAttribute("lastseen");
                    attribute.InnerText = DateTime.Now.ToShortDateString();
                    member.Attributes.Append(attribute);

                    first.AppendChild(member);


                }

                doc.Save("LastSeen.xml");

            }
            */
        }


    }





}
