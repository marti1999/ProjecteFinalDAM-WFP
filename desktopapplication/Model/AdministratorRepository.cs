using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows;
using WSRobaSegonaMa.Models;

namespace desktopapplication.Model
{
    class AdministratorRepository
    {
        public static List<Administrator> getAllAdministrator()
        {
            List<Administrator> lu = new List<Administrator>();
            lu = (List<Administrator>)MakeRequest(string.Concat(Utils.ws, "administrators"), null, "GET", "application/json", typeof(List<Administrator>));
            return lu;
        }

        public static List<Administrator> getAllAdministrators()
        {
            List<Administrator> lu = new List<Administrator>();
            lu = (List<Administrator>)MakeRequest(string.Concat(Utils.ws, "administratorsTot"), null, "GET", "application/json", typeof(List<Administrator>));
            return lu;
        }

        public static Administrator getAdministratorById(int id)
        {
            Administrator r = (Administrator)MakeRequest(string.Concat(Utils.ws, "administrator/" + id), null, "GET", "application/json", typeof(Administrator));
            return r;
        }
        public static Administrator getAdministratorByEmail(string email)
        {
            Administrator r = (Administrator)MakeRequest(string.Concat(Utils.ws, "administratorEmail/" + email), null, "GET", "application/json", typeof(Administrator));
            return r;
        }

        public static Administrator add(Administrator a)
        {
            Administrator r = (Administrator)MakeRequest(string.Concat(Utils.ws, "administrator"), a, "POST", "application/json", typeof(Administrator));
            return r;
        }

        public static Administrator edit(Administrator a)
        {
            Administrator r = (Administrator)MakeRequest(string.Concat(Utils.ws, "administrator/", a.Id), a, "PUT", "application/json", typeof(Administrator));
            return r;
        }
        public static void changeLang(int id, String lang)
        {
            List<object> ar = new List<object>();
            ar.Add(id);
            ar.Add(lang);
            MakeRequest(string.Concat(Utils.ws, "api/administrator/updlang", null), ar, "PUT", "application/json", typeof(void));
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
