using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading;

namespace IridiUpload.Logic.HardServer
{
    class Http
    {
        public string Get(string urlAdd)
        {
            //int result = 0;
            string ip = Program.Params.HSIP.Value;
            string irSessionId = Program.Params.HSIrSessionId.Value;
            string url = "http://" + ip + ":8888/" + urlAdd;

            Program.Log.Informational(string.Format("Get:\r\n{0}\r\nused ir-session-id: {1}", url, irSessionId));

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

            wr.Timeout = 500;
            wr.Method = "GET";
            wr.UserAgent = "c# app";
            //wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36 Edg/108.0.1462.76";
            wr.Referer = "http://" + ip + ":8888/html/main";
            wr.Accept = "application/json";


            var cookieContainer = new System.Net.CookieContainer();
            cookieContainer.SetCookies(new System.Uri("http://" + ip), "ir-session-id=" + irSessionId);
            wr.CookieContainer = cookieContainer;

            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            string responseJson = "";
            //WebResponse wresp = null;
            HttpWebResponse wresp = null;
            try
            {
                //wresp = wr.GetResponse();
                wresp = (HttpWebResponse)wr.GetResponse();

                //HttpWebResponse response = (HttpWebResponse)wr.GetResponse();

                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseJson = reader2.ReadToEnd();

                Program.Log.Debug(String.Format("Response: {0}\r\n{1}", (int)wresp.StatusCode, responseJson));
                wresp.GetResponseStream().Close();
                wr.GetResponse().Close();
                wresp.Close();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.GetResponseStream().Close();
                    wr.GetResponse().Close();
                    Program.Log.Error(String.Format("Response: {0} \r\n{1}\r\n{2}", (int)((HttpWebResponse)wr.GetResponse()).StatusCode, url, ex));
                    //String.Format("Response: {0} \r\n" + url + "\r\nis: {1}", (int)response.StatusCode, reader2.ReadToEnd());
                    //textLog.AppendText(String.Format("Error: Response: {0} \r\n{1}\r\n{2}", (int)((HttpWebResponse)wr.GetResponse()).StatusCode, url, ex));
                    wresp.Close();
                    wresp = null;
                }
                else
                {
                    Program.Log.Error(String.Format("Response: HttpWebResponse = null \r\n{0}\r\n{1}", url, ex));
                    //textLog.AppendText(String.Format("Error: Response: null \r\n{0}\r\n{1}", url, ex));
                }
                //result = -2;
            }
            finally
            {
                wr = null;

            }
            //Program.Log.Colour();
            //WinForms.Colorize.RichEditColour.ColourIt(textLog);
            return responseJson;
        }
    }
}
