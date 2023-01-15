using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Web;
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

namespace IridiUpload.Logic.Iridi.Cloud
{
    class Authentication
    {
        public string  HttpAuth()
        {
            //int result = 0;
            //string ip = Program.Params.HSIP.Value;
            string responseJson = "";

            string login = Program.Params.ILogin.Value;
            string pass = Program.Params.IPassword.Value;
            if (login == "" || pass == "") return "";

            string urlAdd = "/signup/auth_ajax.php";
            string url = Program.Params.Uri + urlAdd;

            Program.Log.Informational(string.Format("Post: Authentication\r\n{0}; password={1}", url, "***"));

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

            wr.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            wr.Method = "POST";
            wr.UserAgent = "c# app";
            //wr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
            wr.Referer = Program.Params.Uri;
            //wr.Accept = "application/json";
            //wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.AllowAutoRedirect = false;

            var cookieContainer = new System.Net.CookieContainer();

            cookieContainer.SetCookies(new System.Uri(Program.Params.Uri), "PHPSESSID=" + Program.Params.IPhpSessId.Value);
            wr.CookieContainer = cookieContainer;

            Stream rs = wr.GetRequestStream();
            string formitem = "AUTH_FORM=Y&TYPE=AUTH&backurl=%2F&USER_LOGIN=" 
                + Program.Params.ILogin.Value+"&USER_PASSWORD=" 
                + Program.Params.IPassword.Value+"&USER_REMEMBER=Y";

            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
            rs.Write(formitembytes, 0, formitembytes.Length);
            rs.Close();

            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                //Stream stream2 = wresp.GetResponseStream();

                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseJson = reader2.ReadToEnd();

                Program.Log.Debug(String.Format("Response: {0}\r\n{1}", (int)wresp.StatusCode, responseJson));

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
            return responseJson;
        }
    }
}
