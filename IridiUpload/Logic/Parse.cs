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
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Diagnostics;

using System.Text.RegularExpressions;

namespace IridiUpload.Logic
{
    class Parse
    {
        public static void All(string textHeader, string textPayload, RichTextBox textLog)
        {
            /*
            Header(Program.Params.URL, nameof(Program.Params.URL), "URL", textLog, textHeader);
            Header(Program.Params.Accept, nameof(Program.Params.Accept), "accept", textLog, textHeader);
            Header(Program.Params.Cookie, nameof(Program.Params.Cookie), "cookie", textLog, textHeader);
            Header(Program.Params.Referer, nameof(Program.Params.Referer), "referer", textLog, textHeader);
            Header(Program.Params.UserAgent, nameof(Program.Params.UserAgent), "user-agent", textLog, textHeader);

            Payload(Program.Params.UserID, nameof(Program.Params.UserID), "user_id", textLog, textPayload);
            Payload(Program.Params.Posthash, nameof(Program.Params.Posthash), "posthash", textLog, textPayload);
            Payload(Program.Params.Act, nameof(Program.Params.Act), "act", textLog, textPayload);
            Payload(Program.Params.ProjectSetRewrite, nameof(Program.Params.ProjectSetRewrite), "project_set_rewrite", textLog, textPayload);
            Payload(Program.Params.Counter, nameof(Program.Params.Counter), "id", textLog, textPayload);

            //WinForms.Colorize.RichEditColour.ColourIt(textLog);
            Program.Log.Colour();*/
        }

        public static void Header(Memory.StringElement strElement, string memoryName, string headerName, RichTextBox textLog, string data)
        {
            string pattern = @"(?<=" + headerName + @": )(.*)(?=\r\n)";
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchedTxt = rg.Matches(data);
            if (matchedTxt.Count > 0)
            {
                if (matchedTxt[0].Value != null && matchedTxt[0].Value != "")
                {
                    if (headerName == "cookie")
                    {
                        string cookieT = matchedTxt[0].Value;
                        strElement.Value = Regex.Replace(cookieT, ";", ",");
                    }
                    else
                    {
                        strElement.Value = matchedTxt[0].Value;
                    }
                    textLog.AppendText(memoryName + ": " + strElement.Value + "\r\n");
                    //textLog.Text += memoryName + ": " + strElement.Value;
                }
            }
        }

        public static void Payload(Memory.StringElement strElement, string memoryName, string payloadName, RichTextBox textLog, string data)
        {
            string pattern = @"(?<=\x22" + payloadName + @"\x22\r\n\r\n)(.*)(?=\r\n)";
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchedTxt = rg.Matches(data);
            if (matchedTxt.Count > 0)
            {
                if (matchedTxt[0].Value != null && matchedTxt[0].Value != "")
                {
                    strElement.Value = matchedTxt[0].Value;
                    textLog.AppendText(memoryName + ": " + strElement.Value + "\r\n");
                    //textLog.Text += memoryName + ": " + strElement.Value;
                }
            }
        }

        public static void Payload(Memory.IntElement strElement, string memoryName, string payloadName, RichTextBox textLog, string data)
        {
            string pattern = @"(?<=\x22" + payloadName + @"\x22\r\n\r\n)(.*)(?=\r\n)";
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchedTxt = rg.Matches(data);
            if (matchedTxt.Count > 0)
            {
                if (matchedTxt[0].Value != null && matchedTxt[0].Value != "")
                {
                    strElement.Value = Int32.Parse(matchedTxt[0].Value) + 1;
                    textLog.AppendText(memoryName + ": " + strElement.Value + "\r\n");
                    //textLog.Text += memoryName + ": " + strElement.Value;
                }
            }
        }
    }
}
