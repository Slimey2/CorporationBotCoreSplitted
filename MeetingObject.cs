using System;
using System.Collections.Generic;
using System.Text;

namespace CorporationBotCoreGeneral
{
    class MeetingObject
    {

        private string name;
        private int day;
        private int time;

        public MeetingObject(string name, int day, int time) {

            this.name = name;
            this.day = day;
            this.time = time;


        }

        public string Name
        {

            get { return name; }


        }

        public int Day {

            get { return day; }
        }

        public int Time {

            get { return time; }

        }




    }
}
