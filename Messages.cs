using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorporationBotGeneral
{
    class Messages
    {
        private int amount;
        private string channel;

        public Messages(int amount, string channel) {
            this.amount = amount;
            this.channel = channel;

        }

        public int Amount {

            get { return amount; }
            set { amount = value; }

        }

        public string Channel {
            get { return channel; }
            set { channel = value; }

        }


    }
}
