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
    class RewardRepository
    {
        public static List<Reward> getAllReward()
        {
            List<Reward> lu = new List<Reward>();
            lu = (List<Reward>)MakeRequest(string.Concat(Utils.ws, "rewards"), null, "GET", "application/json", typeof(List<Reward>));
            return lu;
        }

        public static Reward setRewardWithLang(int id, Requestor re)
        {
            Reward r = (Reward)MakeRequest(string.Concat(Utils.ws, "reward/"+id), re, "PUT", "application/json", typeof(Reward));
            return r;
        }
        public static Reward insertRewardWithLang(Requestor re)
        {
            Reward r = (Reward)MakeRequest(string.Concat(Utils.ws, "rewards"), re, "POST", "application/json", typeof(Reward));
            return r;
        }

        public static void deactivateReward(int id)
        {
            Reward r = (Reward)MakeRequest(string.Concat(Utils.ws, "reward/" + id), null, "DELETE", "application/json", typeof(Reward));
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
