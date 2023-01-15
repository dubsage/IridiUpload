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
using System.Collections;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Web.Script.Serialization;

namespace IridiUpload.Logic.Iridi.Cloud
{
    class Upload
    {
        public bool UploadFile()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("user_id", Program.Params.IPostUser.Value);
            nvc.Add("posthash", Program.Params.IPostHash.Value);
            nvc.Add("id", Program.Params.Selected.Id);
            nvc.Add("act", Program.Params.Act);
            nvc.Add("project_set_rewrite", Program.Params.ProjectSetRewrite);

            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            nvc.Add("qquuid", myuuidAsString);
            nvc.Add("qqfilename", Program.Params.File.Name);
            nvc.Add("qqtotalfilesize", Program.Params.File.Size.ToString());
            
            string response = HttpUploadFile(nvc);
            //Program.Log.Warning("response: " + response);
            string pattern = @"(?<=""success"":)(.*?)(?=,)";
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchedTxt = rg.Matches(response);
            if (matchedTxt.Count > 0)
            {
                //Program.Log.Warning("matchedTxt[0].Value: " + matchedTxt[0].Value);
                if (matchedTxt[0].Value == "true")
                {
                    string patternPID = @"(?<=\x22pid\x22:)(\d*?)(?=,)";
                    Regex rgPID = new Regex(patternPID, RegexOptions.IgnoreCase);
                    MatchCollection matchedPID = rgPID.Matches(response);
                    if (matchedPID.Count > 0 && matchedPID[0].Value != null && matchedPID[0].Value != "")
                    {
                        //Program.Params.Counter.Value = Int32.Parse(matchedPID[0].Value);
                        string newPID = matchedPID[0].Value;
                        Program.Log.Warning("Iridi Cloud upload successful.");
                        Program.Log.Warning("\tSelected folder: " + Program.Params.Selected.Folder.Value);
                        Program.Log.Warning("\tSelected object: " + Program.Params.Selected.Object.Value);
                        Program.Log.Warning("\tSelected project: " + Program.Params.Selected.Project.Value);
                        Program.Log.Warning("\tCurrent PID: " + Program.Params.Selected.Id);
                        Program.Log.Warning("\tNext PID: " + newPID);
                        return true;
                    }     
                }
            }
            Program.Log.Warning("Iridi Cloud upload failed.");
            Program.Log.Warning("\tSelected folder: " + Program.Params.Selected.Folder.Value);
            Program.Log.Warning("\tSelected object: " + Program.Params.Selected.Object.Value);
            Program.Log.Warning("\tSelected project: " + Program.Params.Selected.Project.Value);
            Program.Log.Warning("\tCurrent PID: " + Program.Params.Selected.Id);

            //Program.Log.Warning("Upload was not successful.");
            //Program.Log.Warning("Upload was not successful.");
            //Program.Log.Warning("Upload was not successful.");
            //Console.WriteLine("Upload was not successful.");
            //HttpUploadFile(Program.Params.URL.Value, Program.Params.FilePath.Value, Program.Params.FileName, "qqfile", "application/octet-stream", nvc);
            return false;
        }
        //public void HttpUploadFile(string url, string file, string fileName, string paramName, string contentType, NameValueCollection nvc)
        public string HttpUploadFile(NameValueCollection nvc)
        {
            //textLog.Text += (string.Format("Uploading {0} to {1}", file, url));
            //textLog.AppendText(string.Format("Uploading {0} to {1}", file, url) + "\r\n");
            string url = Program.Params.Uri + Program.Params.UploadUrl;
            string file = Program.Params.File.Path.Value;

            string response = "";
            Program.Log.Informational(string.Format("Uploading: {0} to {1}", file, url));

            string boundary = "----WebKitFormBoundary1" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.UserAgent = "c# app";// Program.Params.UserAgent.Value;
            //wr.Referer = Program.Params.Referer.Value;
            //wr.Accept = Program.Params.Accept.Value;

            //var cookieContainer = new System.Net.CookieContainer();
            //cookieContainer.SetCookies(new System.Uri(Program.Params.Uri), Program.Params.Cookie.Value);
            //wr.CookieContainer = cookieContainer;
            var cookieContainer = new System.Net.CookieContainer();
            cookieContainer.SetCookies(new System.Uri(Program.Params.Uri), "PHPSESSID=" + Program.Params.IPhpSessId.Value);
            wr.CookieContainer = cookieContainer;

            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, Program.Params.ParamName, Program.Params.File.Name, Program.Params.ContentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            //textLog.Text = wr.RequestUri.ToString();
            //WinForms.Colorize.RichEditColour.ColourIt(textLog);
            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream = wresp.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                response = reader.ReadToEnd();

                

               /*
               var serializer = new JavaScriptSerializer();
               serializer.RegisterConverters(new[] { new Utility.DynamicJsonConverter() });

               dynamic obj = serializer.Deserialize(response, typeof(object));
               Console.WriteLine(obj);
               Console.WriteLine("obj.success: " + obj.success);
               Console.WriteLine("obj.pid: " + obj.pid);
               //var list = obj.Where(q => q.name == "object").ToList();
               Console.WriteLine("obj.object.NAME: " + obj.GetValue("object").NAME);*/

               //dynamic data = Json.Decode(response);
               /*
               string answer = String.Format("File uploaded, server response is: {0}", reader2.ReadToEnd());
               textLog.AppendText(answer + "\r\n");

               string pattern = @"(?<=\x22pid\x22:)(\d*)(?=,)";
               Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
               MatchCollection matchedTxt = rg.Matches(answer);
               if (matchedTxt.Count > 0 && matchedTxt[0].Value != null && matchedTxt[0].Value != "")
               {
                   Program.Params.Counter.Value = Int32.Parse(matchedTxt[0].Value);
                   textLog.AppendText("Counter: " + Program.Params.Counter.Value + "\r\n");
               }
               else
               {
                   //Program.Params.Counter.Value++;
               }*/

               wresp.GetResponseStream().Close();
                wr.GetResponse().Close();
                wresp.Close();
            }
            catch (Exception ex)
            {
                //textLog.AppendText("Error uploading file " + ex + "\r\n");
                //textLog.Text += ("Error uploading file " + ex);
                if (wresp != null)
                {
                    wresp.GetResponseStream().Close();
                    wr.GetResponse().Close();
                    Program.Log.Error(String.Format("Error uploading file Response: {0} \r\n{1}\r\n{2}", (int)((HttpWebResponse)wr.GetResponse()).StatusCode, url, ex));
                    wresp.Close();
                    wresp = null;
                }
                else
                {
                    Program.Log.Error(String.Format("Error uploading file Response: HttpWebResponse = null \r\n{0}\r\n{1}", url, ex));
                }
            }
            finally
            {
                wr = null;

            }
            return response;
            //Program.Log.Colour();
            //WinForms.Colorize.RichEditColour.ColourIt(textLog);
        }
    }
}
