using desktopapplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace desktopapplication.Model.objects
{
    class announcementRepository
    {
        private static string ws1 = Utils.ws;
        public static List<Announcement> getAllAnnouncements()
        {
            List<Announcement> lu = new List<Announcement>();
            lu = (List<Announcement>)MakeRequest(string.Concat(ws1, "announcements"), null, "GET", "application/json", typeof(List<Announcement>));
            return lu;
        }
        
        public static ObservableCollection<Recipient> getRecipients()
        {
            ObservableCollection<Recipient> r = new ObservableCollection<Recipient>();
            r = (ObservableCollection<Recipient>)MakeRequest(string.Concat(ws1, "recipients"), null, "GET", "application/json", typeof(ObservableCollection<Recipient>));
            return r;
        }

        public static void addAnnouncement(Announcement c) {
            Console.WriteLine(c.ToString());
        Announcement a = (Announcement) MakeRequest(string.Concat(ws1, "announcement"), c, "POST", "application/json", typeof(Announcement));

        }

        public static object MakeRequest(string requestUrl, object JSONRequest, string JSONmethod, string JSONContentType, Type JSONResponseType)
        //  requestUrl: Url completa del Web Service, amb l'opció sol·licitada
        //  JSONrequest: objecte que se li passa en el body (només per a POST/PUT)
        //  JSONmethod: "GET"/"POST"/"PUT"/"DELETE"
        //  JSONContentType: "application/json" en els casos que el Web Service torni objectes
        //  JSONRensponseType:  tipus d'objecte que torna el Web Service (typeof(tipus))
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest; //WebRequest WR = WebRequest.Create(requestUrl);   
                string sb = JsonConvert.SerializeObject(JSONRequest);
                request.Method = JSONmethod;  // "GET"/"POST"/"PUT"/"DELETE";  

                if (JSONmethod != "GET")
                {
                    request.ContentType = JSONContentType; // "application/json";   
                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    object objResponse = null;

                    objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);
                    return objResponse;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
