using CorporationBotGeneral;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CorporationBotCoreGeneral
{
    public class SendHttpRequest
    {

        public string Sendhttprequest(String user, List<String> division, string requesttype) {
            string error = "";
            string activation = null;
            string url = null;
            try
            {


                string data = "";
                XmlDocument doc = new XmlDocument();
                doc.Load("Config.xml");
                XmlNode first = doc.FirstChild;
                activation = first.SelectSingleNode("/GeneralConfig/HttpConfig/Authorisation").InnerText;

                if (requesttype == "login")
                {
                    url = first.SelectSingleNode("/GeneralConfig/HttpConfig/Urllogin").InnerText;
                }
                else if (requesttype == "usertribute")
                {

                    url = first.SelectSingleNode("/GeneralConfig/HttpConfig/Urltribute").InnerText;
                }
                else if (requesttype == "allinfluences") {

                    url = first.SelectSingleNode("/GeneralConfig/HttpConfig/Urlinfluence").InnerText;

                }

                string responseText = "";
                string endText = "```";
                user = user.Replace("\"", String.Empty);
                user = Regex.Replace(user, @"\p{Cs}", "");
                HttpClient client = new HttpClient();

                if (requesttype == "login")
                {
                    data = "{\"name\": \"" + user + "\",\"divisions\":[";

                    foreach (String name in division)
                    {

                        data = data + "\"" + name + "\",";


                    }

                    data = data.TrimEnd(',');
                    data = data + "]}";

                }
                else if (requesttype == "usertribute" || requesttype == "allinfluences") {

                    data = "{\"name\": \"" + user + "\"}";

                }

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(data);
                var request2 = (HttpWebRequest)WebRequest.Create(url);
                request2.Method = "POST";
                request2.Headers.Add(HttpRequestHeader.Authorization.ToString(), activation);
                request2.ContentType = "application/json";
                request2.ContentLength = byte1.Length;
                Stream newStream = request2.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);

                var response = (HttpWebResponse)request2.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }

                if (requesttype == "login" || requesttype == "usertribute")
                {


                    responseText = responseText.Replace("{", string.Empty);

                    responseText = responseText.Replace("}", string.Empty);

                    responseText = responseText.Replace("\"", string.Empty);

                    var firstsplit = responseText.Split(",");
                    foreach (string s in firstsplit)
                    {

                        endText = endText + s + Environment.NewLine;



                    }
                }
                else if (requesttype == "allinfluences") {

                    responseText = responseText.Replace("{", string.Empty);
                    responseText = responseText.Replace("\"", string.Empty);
                    var firstsplit = responseText.Split("},");

                    foreach (string s in firstsplit) {
                        var secondsplit = s.Split(",");


                        if ((!secondsplit[2].Contains("amount:0"))) {


                            endText = endText + s + Environment.NewLine;

                        }



                    }

                    endText = endText.Replace("[", string.Empty);
                    endText = endText.Replace("]", String.Empty);
                    endText = endText.Replace("}", String.Empty);



                }
                endText = endText + "```";

                return endText;
            }
            catch (Exception ex) {


                error = ex.Message;

            }

            return error;

        }


        public String SendInfluence(String sender, String receiver, int amount, string type){
            string error = "";
            string activation = null;
            string url = null;

            try {

                string data = "";
                XmlDocument doc = new XmlDocument();
                doc.Load("Config.xml");
                XmlNode first = doc.FirstChild;
                activation = first.SelectSingleNode("/GeneralConfig/HttpConfig/Authorisation").InnerText;
                url = first.SelectSingleNode("/GeneralConfig/HttpConfig/Sendinfluence").InnerText;


                string responseText = "";
                string endText = "```";
                sender = sender.Replace("\"", String.Empty);
                sender = Regex.Replace(sender, @"\p{Cs}", "");

                receiver = receiver.Replace("\"", String.Empty);
                receiver = Regex.Replace(receiver, @"\p{Cs}", "");


                HttpClient client = new HttpClient();


                data = "{\"sender\":\"" + sender + "\",\"receiver\":\"" + receiver + "\",\"amount\":" + amount + ",\"type\":\"" + type + "\"}";


                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(data);
                var request2 = (HttpWebRequest)WebRequest.Create(url);
                request2.Method = "POST";
                request2.Headers.Add(HttpRequestHeader.Authorization.ToString(), activation);
                request2.ContentType = "application/json";
                request2.ContentLength = byte1.Length;
                Stream newStream = request2.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);

                var response = (HttpWebResponse)request2.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }

                responseText = responseText.Replace("{", string.Empty);

                responseText = responseText.Replace("}", string.Empty);

                responseText = responseText.Replace("\"", string.Empty);

                responseText = responseText.Replace("message:", string.Empty);

                error = responseText;
            }
            catch (Exception ex) {


                error = ex.Message;

            }






            return error;
        }



    }
}
