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

namespace IridiUpload.WinForms.Colorize
{
    class RichEditColour
    {
        //public int _startPosition = 0;
        //public int _appendLength = 0;
        RichTextBox _textLog;
        public RichEditColour(RichTextBox textLog)
        {
            _textLog = textLog;
        }

        public readonly string[] DictionaryOrange = //new string[100]
        {
                "---end---",
                "objects:",
                "projects:",
                "Selected:",
                "Iridi cloud:",
                "File:",
                "Server:",
                "Iridi cloud projects:",
                "Iridi cloud objects:",
                "Iridi cloud folders:",
                "The oldest data was removed. Continue...",
                "Try to register next hot key:"

            };

        public readonly string[] DictionaryBrown = //new string[100]
            {
                "Response:",
                "Get:",
                "Post:",
                "Parsed response:",
                "Selected.Id:",
                @"""send to cloud""",
@"""update on server""",
@"""send and update"""
            


            };

        public readonly string[] DictionaryRed = //new string[100]
            {
                "Error:",
                "Project didnt downloaded.",
                "Project didnt Finded.",
                "Authentication failed",
                "Couldn’t register the hot key.",
                "Iridi Cloud projects were not received"
            };

        public readonly string[] DictionaryGreen = //new string[100]
            {
            "Please try another.",
            "Got",
            "shortcut.",
    "project id:",
    "project name:",
    "object id:",
    "object name:",
    "folder id:",
    "folder name:",
    "Next PID:",
    "",
    "",
                "Path:",
            "Name:",
            "Size:",
            "Selected object:",
            "Selected folder:",
            "IP:",
            "Selected project:",
                "Object name:",
                "Project name:",
                "File path:",
                "File name:",
                "File size:",
                "Counter:",
                "URL: ",
                "Accept:",
                "Referer:",
                "Cookie:",
                "UserAgent:",
                "ProjectSetRewrite:",
                "Act:",
                "Posthash:",
                "UserID:",
                "Uploading ",
                "File uploaded, server response is: ",
                "\"success\"",
                "\"uuid\"",
                "\"new_studio_project\"",
                "\"user_id\"",
                "\"object\"",
                "\"projects\"",
                "\"count\"",
                "\"pid\"",
                "\"project_set_rewrite\"",
                "\"uploadName\"",
                "Error uploading file ",

                //iridi cloud
                "PostHash:",
                "Folder:",
            "Object:",
            "Project:",
            "PhpSessId:",
            "SessId:",
            "Current PID:",
            "status:",
            "user id:",
            "Post hash:",
            "Post user:",
                "IUserID:",
                "PHPSESSID:",
                "IPhpSessId:",
                "ISessId:",
                "IPosthash:",
                "IPHPSESSID:",
                "set cookies:",

                //Hard Server Data
                "HSPassword",
                "HSIP",
                "HSObject",
                "HSProject",
                "IrSessionId",
                "HSIrSessionId",
                "HSPId",
                "Project downloaded",

                //Hard Server Selected
                "",
                //"Pid: ",

                //HS Response
                "user_status:",
                "update_user_status:",
                "user_last_update:",
                "user_login:",
                "user_name:",
                "test_mode_status:",
                "attached_object:",
                "object_id:",
                "object_type:",
                "object_name:",
                "objects_status:",
                "ir-session-id:",
                "used ir-session-id:",
                "project_id:",
                "project_name:",
                "project_upid:",
                "project_status:",
                "objects_last_update:",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            };

        void Colour(string[] dictionary, System.Drawing.Color color)
        {
            for (int i = 0; i < dictionary.Length; i++)
            {
                if (dictionary[i] == "") continue;
                int length = dictionary[i].Length;
                Regex myRegex = new Regex(dictionary[i]);
                foreach (Match match in myRegex.Matches(_textLog.Text))
                {
                    _textLog.Select(match.Index, length);
                    _textLog.SelectionColor = color;
                }
            }
        }

        void Colour(string[] dictionary, System.Drawing.Color color, string append)
        {
            //fucked richtextbox removes \r characters =(((((
            append = append.Replace("\r", "");

            string text = _textLog.Text;
            int textLength = text.Length;
            int appendLength = append.Length;
            int startPosition = textLength - appendLength;

            for (int i = 0; i < dictionary.Length; i++)
            {
                if (dictionary[i] == "") continue;
                int length = dictionary[i].Length;
                Regex myRegex = new Regex(dictionary[i]);
                foreach (Match match in myRegex.Matches(append))
                {
                    _textLog.Select(match.Index + startPosition, length);
                    _textLog.SelectionColor = color;
                }
            }
        }

        public void ScrollDown()
        {
            _textLog.SelectionStart = _textLog.Text.Length;
            _textLog.ScrollToCaret();
        }

        public void Colour()
        {
            Colour(DictionaryOrange, Color.Orange);
            Colour(DictionaryRed, Color.Red);
            Colour(DictionaryBrown, Color.Brown);
            Colour(DictionaryGreen, Color.Green);

            ScrollDown();
        }

        public void Colour(string append)
        {
            Colour(DictionaryOrange, Color.Orange, append);
            Colour(DictionaryRed, Color.Red, append);
            Colour(DictionaryBrown, Color.Brown, append);
            Colour(DictionaryGreen, Color.Green, append);

            ScrollDown();
        }
    }
}
