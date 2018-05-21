using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using CorporationBotCoreGeneral;
using System.Xml;

namespace CorporationBotGeneral
{
    static class Activation
    {


        public static DatabaseArchiving archiveobject;
        public static Boolean stats;
        public static Boolean active = true;
        public static Boolean moddeletation;
        public static List<ulong> languages = new List<ulong>(new ulong[] { 408342925513064448, 408340793984679936, 408341081592299540, 408341084100362240, 408341086617075724, 419406136345886722 });
        public static string HttpAuthorisation = "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImF1ZGllbmNlIjoid2ViIiwiY3JlYXRlZCI6MTUxODM3NzY0NDM5NCwiZXhwIjoxNTE4OTgyNDQ0fQ.BXlX4g8j2ZtCFGXGgb1z4WnhE6gk9S2V2q9a_Oobq5OkHE4VmDw8Onzmna2h0POKveDI_L-FNy_O8qUeUI-T7g";
        public static List<MeetingObject> meetings = new List<MeetingObject>();
        public static Boolean httprequest;



        public static void reloadConfig() {

            XmlDocument doc = new XmlDocument();
            doc.Load("BotConfig.xml");
            XmlNode l1 = doc.FirstChild;
            stats = Convert.ToBoolean(l1.SelectSingleNode("/BotConfig/Stats").InnerText);

            moddeletation = Convert.ToBoolean(l1.SelectSingleNode("/BotConfig/deleteConfig").InnerText);

            httprequest = Convert.ToBoolean(l1.SelectSingleNode("/BotConfig/httpConfig").InnerText);


        }




    }
}
