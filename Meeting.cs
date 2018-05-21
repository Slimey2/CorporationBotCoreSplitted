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

namespace CorporationBotGeneral
{
    [Group("meeting")]
    public class Meeting : ModuleBase
    {



        [Command("list")]
        public async Task MeetingCommand() {

            try
            {
                

                DateTime today = DateTime.Today;
                DateTime currenttime = System.DateTime.Now.ToUniversalTime();

                string endmessage = "It is currently: " + currenttime.ToString() + "UTC" + Environment.NewLine;

                foreach (MeetingObject m in Activation.meetings) {

                    int daysuntilmeetting = (m.Day - (int)today.DayOfWeek + 7) % 7;
                    DateTime nextMeeting = today.AddDays(daysuntilmeetting);
                    DateTime next2;
                    next2 = nextMeeting.AddHours(m.Time);

                    var remainingMeeting = (next2 - currenttime).TotalHours;

                    /*
                     *To be done:
                     * 
                     *  change time to string
                     *  Format the end string to add the name of the meeting
                     *
                     *                   
                 
                 */

                    string meetingtime = TimeSpan.FromHours(remainingMeeting).ToString();
                    var meetingsplit = meetingtime.Split('.');

                    if (remainingMeeting > 0) {

                        endmessage = endmessage + m.Name + " meeting is planned in: " + splittime(meetingsplit) + Environment.NewLine;

                    }
                    else
                    {

                        endmessage = endmessage + m.Name + " meeting has passed. Maybe join the next one." + Environment.NewLine;

                    }



                }


                   


                    if (Context.Channel.Name == "meetings")
                    {
                        await Context.Channel.SendMessageAsync(endmessage);
                    }
                    else
                    {
                        await Context.User.SendMessageAsync(endmessage);

                    }

                

            }
            catch
            {

                await Context.User.SendMessageAsync("Log Meeting Command: If you send a message direct to the Bot please use the command in a discord chat." + Environment.NewLine
                         + "If you did use the command in a chatchannel but you still have an error please check the spelling of you command and try again. If the error still persist please contact Slimey2");


            }
        }

        [Command("add")]
        public async Task addMeeting(String Name, String day, int time) {
            ulong managerId = 84900316213936128;
            IGuildUser user = Context.User as IGuildUser;

            if (user.RoleIds.Contains(managerId))
            {



                int dayintime = 0;

                if (day.ToLower() == "monday")
                {

                    dayintime = 1;
                }
                else if (day.ToLower() == "thuesday")
                {
                    dayintime = 2;

                }
                else if (day.ToLower() == "wednesday")
                {

                    dayintime = 3;
                }
                else if (day.ToLower() == "thursday")
                {

                    dayintime = 4;
                }
                else if (day.ToLower() == "friday")
                {

                    dayintime = 5;
                }
                else if (day.ToLower() == "saturday")
                {

                    dayintime = 6;
                }
                try
                {

                    Activation.meetings.Add(new MeetingObject(Name, dayintime, time));
                }
                catch (Exception ex)
                {
                    await Context.User.SendMessageAsync("Something went wrong: " + ex.Message);
                }
            }
            else {

                await Context.User.SendMessageAsync("You are not permitted to add meetings to the list");


            }


        }


        [Command("remove")]
        public async Task RemoveMeeting(String name) {
            ulong managerId = 84900316213936128;
            IGuildUser user = Context.User as IGuildUser;

            if (user.RoleIds.Contains(managerId))
            {




                foreach (MeetingObject m in Activation.meetings) {

                if (m.Name == name)
                    {

                    Activation.meetings.Remove(m);

                    }


                }
            }
            else
            {

                await Context.User.SendMessageAsync("You are not permitted to remove meetings to the list");


            }



        }




        private String splittime(string[] split) {
            string splittedstring = "";

            if (split.Length > 2)
            {
                splittedstring = split[0] + " days and " + split[1] + " Hours";

            }
            else
            {

                splittedstring = split[0] + " Hours";

            }

            return splittedstring;
        }
    }


    
}
