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

namespace IridiUpload.Trash
{
    class DeleteIt
    {
        /*
       public static bool IsGZipSupported()
       {
           string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
           if (!string.IsNullOrEmpty(AcceptEncoding) &&
                   (AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate")))
               return true;
           return false;
       }*//*
        /// <summary>
        /// Sets up the current page or handler to use GZip through a Response.Filter
        /// IMPORTANT:  
        /// You have to call this method before any output is generated!
        /// </summary>
        public static void GZipEncodePage()
        {
            HttpResponse Response = HttpContext.Current.Response;

            if (IsGZipSupported())
            {
                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

                if (AcceptEncoding.Contains("gzip"))
                {
                    Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                    Response.Headers.Remove("Content-Encoding");
                    Response.AppendHeader("Content-Encoding", "gzip");
                }
                else
                {
                    Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                    Response.Headers.Remove("Content-Encoding");
                    Response.AppendHeader("Content-Encoding", "deflate");
                }
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            Response.AppendHeader("Vary", "Content-Encoding");
        }*/

        /*
string httpWebRequestDump(HttpWebRequest hwr)
{
    return JsonConvert.SerializeObject(hwr, Formatting.Indented);
}*/
        /*
        public string[] parseResponse(string addUrl, string[] keys)
        {
            List<string> fieldValues = new List<string>();

            string responseStr = HttpHSGet(addUrl);
            if (keys == null) return new string[] { "" };
            if (responseStr != null && responseStr != "")
            {
                try
                {
                    var responseDoc = JsonDocument.Parse(responseStr);
                    JsonElement responseRoot = responseDoc.RootElement;
                    foreach (string key in keys)
                    {
                        if (key == "") continue;
                        string fieldValue = responseRoot.GetProperty(key).ToString();
                        fieldValues.Add(fieldValue);
                        Program.Log.Debug(key + ": " + fieldValue);
                    }
                }
                catch (Exception e) { Program.Log.Error(addUrl + "\r\n" + e.Message); }        
            }
            return fieldValues.ToArray();
        }*/
       
    }
}
