using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace CorporationBotGeneral
{
    class DatabaseArchiving
    {

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        public DatabaseArchiving() {
            Initialize();

        }

        private void Initialize()
        {
            try
            {


                XmlDocument doc = new XmlDocument();
                doc.Load("Config.xml");
                XmlNode first = doc.FirstChild;
                server = first.SelectSingleNode("/GeneralConfig/DBConfig/ServerConfig").InnerText;
                database = first.SelectSingleNode("/GeneralConfig/DBConfig/DatabaseConfig").InnerText;
                uid = first.SelectSingleNode("/GeneralConfig/DBConfig/UsernameConfig").InnerText;
                password = first.SelectSingleNode("/GeneralConfig/DBConfig/PasswordConfig").InnerText;
                port = first.SelectSingleNode("/GeneralConfig/DBConfig/PortConfig").InnerText;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            string connectionString = "";
            if (port == "")
            {
                connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            }
            else {

                connectionString = "SERVER=" + server + ";" + "Port="+ port + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            }
            Console.WriteLine(connectionString);
            try
            {
                connection = new MySqlConnection(connectionString);
            }
            catch (Exception ex) {
                Console.WriteLine("Something went wrong with the connection string. I can do without for the moment. " + ex.Message);

            }


        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }


        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void InsertModeration(SocketUserMessage message)
        {
            
            string receivedmessages = message.Content;


            string endmessage = message.ToString();
            endmessage = endmessage.Replace('"', '\'');

            var message2 = message as IMessage;
            endmessage = Regex.Replace(endmessage, @"\p{Cs}", "");
            
            
            string query = "INSERT INTO statdb.messages (Time, User, Channel, Text, Drama) VALUES( NOW(), \"" + message.Author.Username +"\", \"" + message.Channel.Name +"\" , \"" + endmessage +"\" , False)";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();
                    
                }
                catch (MySqlException ex){

                    string query2 = "INSERT INTO statdb.messages (Time, User, Channel, Text) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\", \"Message contained an emijo and has been skipped for now\" )" ;
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);

                    cmd2.ExecuteNonQuery();
                }

                //close connection
                this.CloseConnection();
            }
        }


        public void InsertStats(SocketUserMessage message)
        {
            
            string receivedmessages = message.Content;


            string endmessage = message.ToString();
            endmessage = endmessage.Replace('"', '\'');

            var message2 = message as IMessage;
            endmessage = Regex.Replace(endmessage, @"\p{Cs}", "");

            string query = "INSERT INTO statdb.statistics (Time, User, Channel, Text) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\" , \"" + endmessage + "\")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {

                    string query2 = "INSERT INTO statdb.statistics (Time, User, Channel, Text) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\", \"Message contained an emijo and has been skipped for now\" )";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);

                    cmd2.ExecuteNonQuery();
                }

                //close connection
                this.CloseConnection();
            }
        }

        private void TestConncetion()
        {

            if (this.OpenConnection() == true)
            {

                Console.WriteLine("Connection was succesfull");

            }
            else {

                Console.WriteLine("Connection was not succesfull but no exceptions thrown");

            }
            this.CloseConnection();

        }




        public void checkNumber() {
            MySqlDataReader mes =null;
            List<Messages> messageList = new List<Messages>();
            int numberofmessages = 0;
                       
            string query = "select count(*) as amount, channel from statdb.statistics group by Channel; ";


            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                
                //Execute command
                try
                {
                    mes =  cmd.ExecuteReader();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
 
                }

                
            }

            while (mes.Read()) {

                numberofmessages = Convert.ToInt32(mes[0].ToString());

            }
                      
            this.CloseConnection();


            if (numberofmessages > 3000) {
                query = "delete from statdb.statistics where MessageID >0; commit;";

                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    try
                    {
                        mes = cmd.ExecuteReader();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);

                    }

                    //close connection
                    this.CloseConnection();

                }



            }


        }



        public String collectMessages()
        {
            MySqlDataReader dr = null;
            List<Messages> messageList = new List<Messages>();

            string outgoingmessage = "";
            string query = "select Time,User,Channel from statdb.statistics; ";
            string query2 = "update statdb.statistics SET used= 1 where used=0";


            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    dr = cmd.ExecuteReader();
                    outgoingmessage = "succes";

                }
                catch (MySqlException ex)
                {
                    outgoingmessage = ex.Message;

                }

                
                
            }
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter("Statistics.csv"))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    fs.Write(name + ",");
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        fs.Write(value + ",");
                    }
                    fs.WriteLine();
                }

                fs.Close();
            }

            this.CloseConnection();

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query2, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    outgoingmessage = ex.Message;
                }
            }


            this.CloseConnection();

            return outgoingmessage;

        }



        public string collectDrama()
        {
            MySqlDataReader dr = null;
            List<Messages> messageList = new List<Messages>();

            string outgoingmessage = "";
            string query = "select * from statdb.messages where Drama = 1; ";

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    dr = cmd.ExecuteReader();
                    outgoingmessage = "succes";

                }
                catch (MySqlException ex)
                {
                    outgoingmessage = ex.Message;

                }



                

            }

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter("Drama.csv"))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    fs.Write(name + ",");
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        fs.Write(value + ",");
                    }
                    fs.WriteLine();
                }

                fs.Close();
            }

            this.CloseConnection();

            return outgoingmessage;

        }



        public string collectDrama(IUser user)
        {
            MySqlDataReader dr = null;
            List<Messages> messageList = new List<Messages>();

            string outgoingmessage = "";
            string query = "select * from statdb.messages where Drama = 1 and User = \"" + user.Username + "\"; ";

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    dr = cmd.ExecuteReader();
                    outgoingmessage = "succes";

                }
                catch (MySqlException ex)
                {
                    outgoingmessage = ex.Message;

                }



            }

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter("Drama.csv"))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    fs.Write(name + ",");
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        fs.Write(value + ",");
                    }
                    fs.WriteLine();
                }

                fs.Close();
            }

            this.CloseConnection();

            return outgoingmessage;

        }



        public string collectModActions()
        {
            MySqlDataReader dr = null;
            List<Messages> messageList = new List<Messages>();

            string outgoingmessage = "";
            string query = "select * from statdb.messages where Drama = 0; ";

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    dr = cmd.ExecuteReader();
                    outgoingmessage = "succes";

                }
                catch (MySqlException ex)
                {
                    outgoingmessage = ex.Message;

                }


            }

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter("ModActions.csv"))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    fs.Write(name + ",");
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        fs.Write(value + ",");
                    }
                    fs.WriteLine();
                }

                fs.Close();
            }

            this.CloseConnection();

            return outgoingmessage;

        }




        public void InsertDrama(IUserMessage message)
        {

            string receivedmessages = message.Content;


            string endmessage = message.ToString();
            endmessage = endmessage.Replace('"', '\'');

            var message2 = message as IMessage;
            endmessage = Regex.Replace(endmessage, @"\p{Cs}", "");


            string query = "INSERT INTO statdb.messages (Time, User, Channel, Text, Drama) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\" , \"" + endmessage + "\", True)";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    string query2 = "INSERT INTO statdb.messages (Time, User, Channel, Text) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\", \"Message contained an emijo and has been skipped for now\" )";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);

                    cmd2.ExecuteNonQuery();
                }

                //close connection
                this.CloseConnection();
            }
        }

        public void InsertLogging(IMessage message)
        {

            string receivedmessages = message.Content;


            string endmessage = message.ToString();
            endmessage = endmessage.Replace('"', '\'');

            var message2 = message as IMessage;
            endmessage = Regex.Replace(endmessage, @"\p{Cs}", "");


            string query = "INSERT INTO statdb.logging (Time, UserName, Channel, Message) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\" , \"" + endmessage + "\")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    string query2 = "INSERT INTO statdb.messages (Time, UserName, Channel, Message) VALUES( NOW(), \"" + message.Author.Username + "\", \"" + message.Channel.Name + "\", \"Message contained an emijo and has been skipped for now\" )";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);

                    cmd2.ExecuteNonQuery();
                }

                //close connection
                this.CloseConnection();
            }
        }

        public void CleanUp(String table) {
            MySqlDataReader mes = null;
            string query = "delete from statdb." + table +  "; commit;";

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    mes = cmd.ExecuteReader();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);

                }

                //close connection
                this.CloseConnection();

            }





        }


    }


}
