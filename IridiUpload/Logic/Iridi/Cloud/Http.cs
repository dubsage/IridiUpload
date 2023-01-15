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
//using Microsoft.AspNetCore.Http;
namespace IridiUpload.Logic.Iridi.Cloud
{
    class Http
    {
        public string GetProjectDefault(string id)
        {
            return GetProject("/my-account/cloud/", id);;
        }

        public string GetProject(string url, string id)
        {
            string payload = "ID=" + id + "&tab=11&ajax=Y&act=get_object&sessid=" + Program.Params.ISessId.Value;
            string referer = Program.Params.Uri + url + "?oid=" + id;
            return Post(url, payload, referer);
        }
        public string Post(string url, string payload, string referer)
        {
            url = Program.Params.Uri + url;
            Program.Log.Informational(string.Format("Post:\r\n{0}", url));
            string response = "";

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            //wr.Timeout = 500;
            wr.Method = "POST";
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.UserAgent = "c# app";
            wr.KeepAlive = true;
            //wr.Referer = referer;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            var cookieContainer = new System.Net.CookieContainer();
            cookieContainer.SetCookies(new System.Uri(Program.Params.Uri), "PHPSESSID=" + Program.Params.IPhpSessId.Value);
            wr.CookieContainer = cookieContainer;

            Stream rs = wr.GetRequestStream();
            /*string formitem = "AUTH_FORM=Y&TYPE=AUTH&backurl=%2F&USER_LOGIN="
                + Program.Params.ILogin.Value + "&USER_PASSWORD="
                + Program.Params.IPassword.Value + "&USER_REMEMBER=Y";*/

            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(payload);
            rs.Write(formitembytes, 0, formitembytes.Length);
            rs.Close();

            //string[] cookies = default;
            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                //WebHeaderCollection headers = wresp.Headers;
                //cookies = headers.GetValues("Set-Cookie");

                //'bitrix_sessid':'
                Stream stream = wresp.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
                /*
                Console.WriteLine(response);
                string pattern = @"(?<='bitrix_sessid':')(.*)(?=')";
                Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matchedTxt = rg.Matches(response);
                if (matchedTxt.Count > 0)
                {
                    if (matchedTxt[0].Value != null && matchedTxt[0].Value != "")
                    {

                        Program.Params.ISessId.Value = matchedTxt[0].Value;
                        Program.Log.Informational("ISessId: " + Program.Params.ISessId.Value);
                        //textLog.Text += memoryName + ": " + strElement.Value;
                    }
                }*/

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
                    wresp.Close();
                    wresp = null;
                }
                else
                {
                    Program.Log.Error(String.Format("Response: HttpWebResponse = null \r\n{0}\r\n{1}", url, ex));
                }
            }
            finally
            {
                wr = null;
            }
            return response;
        }
        public string Get(string url)
        {
            url = Program.Params.Uri + url;
            Program.Log.Informational(string.Format("Get:\r\n{0}", url));
            string response = "";

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            //wr.Timeout = 500;
            wr.Method = "GET";
            wr.UserAgent = "c# app";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            var cookieContainer = new System.Net.CookieContainer();
            cookieContainer.SetCookies(new System.Uri(Program.Params.Uri), "PHPSESSID=" + Program.Params.IPhpSessId.Value);
            wr.CookieContainer = cookieContainer;

            

            //string[] cookies = default;
            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                //WebHeaderCollection headers = wresp.Headers;
                //cookies = headers.GetValues("Set-Cookie");

                //'bitrix_sessid':'
                Stream stream = wresp.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
                /*
                Console.WriteLine(response);
                string pattern = @"(?<='bitrix_sessid':')(.*)(?=')";
                Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matchedTxt = rg.Matches(response);
                if (matchedTxt.Count > 0)
                {
                    if (matchedTxt[0].Value != null && matchedTxt[0].Value != "")
                    {

                        Program.Params.ISessId.Value = matchedTxt[0].Value;
                        Program.Log.Informational("ISessId: " + Program.Params.ISessId.Value);
                        //textLog.Text += memoryName + ": " + strElement.Value;
                    }
                }*/

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
                    wresp.Close();
                    wresp = null;
                }
                else
                {
                    Program.Log.Error(String.Format("Response: HttpWebResponse = null \r\n{0}\r\n{1}", url, ex));
                }
            }
            finally
            {
                wr = null;
            }
            return response;
        }
        public string[] GetCookies(string url)
        {
            Program.Log.Informational(string.Format("Get:\r\n{0}", url));
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

            //wr.Timeout = 500;
            wr.Method = "GET";
            wr.UserAgent = "c# app";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            string[] cookies = default;
            HttpWebResponse wresp = null;
            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                WebHeaderCollection headers = wresp.Headers;
                cookies = headers.GetValues("Set-Cookie");

                //'bitrix_sessid':'
                Stream stream = wresp.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string response = reader.ReadToEnd();
                //Console.WriteLine(response);
                string pattern = @"(?<='bitrix_sessid':')(.*)(?=')";
                Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matchedTxt = rg.Matches(response);
                if (matchedTxt.Count > 0)
                {
                    if (matchedTxt[0].Value != null && matchedTxt[0].Value != "")
                    {

                        Program.Params.ISessId.Value = matchedTxt[0].Value;
                        Program.Log.Informational("Response:");
                        Program.Log.Informational("ISessId: " + Program.Params.ISessId.Value);
                        //textLog.Text += memoryName + ": " + strElement.Value;
                    }
                }


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
                    wresp.Close();
                    wresp = null;
                }
                else
                {
                    Program.Log.Error(String.Format("Response: HttpWebResponse = null \r\n{0}\r\n{1}", url, ex));
                }
            }
            finally
            {
                wr = null;
            }
            return cookies;
        }

        public string ParseCookie(string url, string cookieKey)
        {
            string[] cookies = GetCookies(url);
            if (cookies == null) return "";

            //foreach (string cookie in cookies)
            //{ Program.Log.Debug("all cookies: " + cookie); }

            foreach (string cookie in cookies)
            {
                Program.Log.Debug("set cookies: " + cookie);
                string[] values = cookie.Split(';');
                foreach (string value in values)
                {
                    string cookieKeyStrong = cookieKey + "=";
                    int startPosition = value.IndexOf(cookieKeyStrong);
                    if (startPosition != -1)
                    {
                        string cookieValue = value.Substring(cookieKeyStrong.Length);
                        Program.Log.Informational(cookieKey + ": " + cookieValue);
                        return cookieValue;
                    }
                }
            }
            return "";
        }

        public bool GetPhpSessId()
        {
            Program.Params.IPhpSessId.Value = ParseCookie(Program.Params.Uri + "/", "PHPSESSID");
            if (Program.Params.IPhpSessId.Value != "") return true;
            else return false;
        }
       
    }
}
