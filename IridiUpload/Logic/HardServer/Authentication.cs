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
    class Authentication
    {
        public bool HttpHSAuth()
        {
            int result = 0;
            string ip = Program.Params.HSIP.Value;

            string password = Program.Params.HSPassword.Value;
            if (ip == "" || password == "") return false;

            string urlAdd = "html/login";
            string url = "http://" + ip + ":8888/" + urlAdd;

            Program.Log.Informational(string.Format("Post: Authentication\r\n{0}", url));

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Timeout = 500;
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.Method = "POST";
            wr.UserAgent = "c# app";
            //wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
            wr.Referer = url;
            wr.Accept = "application/json";
            //wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.AllowAutoRedirect = false;

            try
            {
                Stream rs = wr.GetRequestStream();

                string formitem = "Password=" + password + "&name=authform&Login=admin";
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
                rs.Close();
            }
            catch (Exception e)
            {
                Program.Log.Error("HSAUTH wr.GetRequestStream() " + e);
            }

            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                //Stream stream2 = wresp.GetResponseStream();

                WebHeaderCollection headers = wresp.Headers;
                string[] str = headers.GetValues("Set-Cookie");
                if (str.Length > 0)
                {
                    string[] cookie = str[0].Split(';');
                    for (int i = 0; i < cookie.Length; i++)
                    {
                        int starPosition = cookie[i].IndexOf("ir-session-id=");
                        if (starPosition != -1)
                        {
                            result = 3;
                            Program.Params.HSIrSessionId.Value = cookie[i].Substring(14);
                            Program.Log.Informational("HSIrSessionId: " + Program.Params.HSIrSessionId.Value);
                        }
                    }
                }

                wresp.GetResponseStream().Close();
                wr.GetResponse().Close();
                wresp.Close();
                //result = 1;
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.GetResponseStream().Close();
                    wr.GetResponse().Close();
                    Program.Log.Error(String.Format("Hard Server Authentication Response: {0} \r\n{1}\r\n{2}", (int)((HttpWebResponse)wr.GetResponse()).StatusCode, url, ex));
                    wresp.Close();
                    wresp = null;
                }
                else
                {
                    Program.Log.Error(String.Format("Hard Server Authentication Response: HttpWebResponse = null \r\n{0}\r\n{1}", url, ex));
                }
                //result = -2;
            }
            finally
            {
                wr = null;
            }
            //Program.Log.Colour();
            //return result;
            if (result == 3) return true;
            else return false;
        }
    }
}
