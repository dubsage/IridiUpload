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

namespace IridiUpload.Utility
{
    class Logging
    {
        WinForms.Colorize.RichEditColour Edit;
        RichTextBox _textLog;
        bool ShowWarning = true;
        bool ShowInformational = true;
        bool ShowDebug = false;

        public Logging (RichTextBox textLog)
        {
            _textLog = textLog;
            Edit = new WinForms.Colorize.RichEditColour(_textLog);
        }
        
        public void Colour()
        {
            Edit.Colour();
        }
        public void Colour(string append)
        {
            Edit.Colour(append);
        }
        public void ScrollDown()
        {
            if (_textLog != null)
            {
                _textLog.SelectionStart = _textLog.Text.Length;
                _textLog.ScrollToCaret();
            }  
        }

        public RichTextBox GetWindow()
        {
            return _textLog;
        }

        public void Init(RichTextBox textLog)
        {
            _textLog = textLog;
        }

        void Append(string message)
        {
            //int length = (message + Environment.NewLine).Length;
            _textLog.AppendText(message + Environment.NewLine);

            Edit.Colour(message + Environment.NewLine);

            ScrollDown();
        }
        public void Emergency(string message)
        {
            Append(message);
            //_textLog.AppendText(message + Environment.NewLine);
            //ScrollDown();
        }

        public void Alert(string message)
        {
            Append(message);
            //_textLog.AppendText(message + Environment.NewLine);
            //ScrollDown();
        }

        public void Critical(string message)
        {
            Append(message);
            //_textLog.AppendText(message + Environment.NewLine);
            //ScrollDown();
        }

        public void Error(string message)
        {
            Append("Error: " + message);
            //_textLog.AppendText("Error: " + message + Environment.NewLine);
            //ScrollDown();
        }

        public void Notice(string message)
        {
            Append(message);
            //_textLog.AppendText(message + Environment.NewLine);
            //ScrollDown();
        }

        public void Warning(string message)
        {
            if (ShowWarning)
            {
                Append(message);
                //_textLog.AppendText(message + Environment.NewLine);
                //ScrollDown();
            }
        }

        public void Informational(string message)
        {
            if (ShowInformational)
            {
                Append(message);
                //_textLog.AppendText(message + Environment.NewLine);
                //ScrollDown();
            }
        }

        public void Debug(string message)
        {
            if (ShowDebug)
            {
                Append(message);
                //_textLog.AppendText(message + Environment.NewLine);
                //ScrollDown();
            }
        }

    }
}
