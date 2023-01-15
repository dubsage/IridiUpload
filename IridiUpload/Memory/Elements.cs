using System;
//using System.IO;
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

namespace IridiUpload.Memory
{
    class THotKey
    {
        public IntElement Ctrl = new IntElement(nameof(Ctrl));
        public IntElement Alt = new IntElement(nameof(Alt));
        public IntElement Shift = new IntElement(nameof(Shift));
        public IntElement Win = new IntElement(nameof(Win));
        public IntElement Use = new IntElement(nameof(Use));
        public IntElement Sign = new IntElement(nameof(Sign));

        public THotKey()
        {
        }
        public void Folder(int i)
        {
            Ctrl.ChangeFolder(i);
            Alt.ChangeFolder(i);
            Shift.ChangeFolder(i);
            Win.ChangeFolder(i);
            Use.ChangeFolder(i);
            Sign.ChangeFolder(i);
        }
        public void Set(bool ctrl, bool shift, bool alt, bool win, bool use, int sign)
        {
            //Console.WriteLine("ctrl " + ctrl);
            Ctrl.Set(ctrl);
            Alt.Set(alt);
            Shift.Set(shift);
            Win.Set(win);
            Use.Set(use);
            Sign.Value = sign;
        }
        public void Load()
        {
            Ctrl.loadFromR();
            Alt.loadFromR();
            Shift.loadFromR();
            Win.loadFromR();
            Use.loadFromR();
            Sign.loadFromR();
        }
        
    }
    class THotKeys
    {
        public THotKey DSend = new THotKey();
        public THotKey Update = new THotKey();
        public THotKey DUP = new THotKey();
        public THotKeys()
        {
            DSend.Folder(1);
            Update.Folder(2);
            DUP.Folder(3);
        }
        public void Load()
        {
            DSend.Load();
            Update.Load();
            DUP.Load();
        }
    }


    class TFile
    {
        public StringElement Path = new StringElement("FilePath");
        public string Name { get; set; }
        public long Size { get; set; }
        public void Update()
        {
            if (System.IO.File.Exists(Path.Value))
            {
                Name = System.IO.Path.GetFileName(Path.Value);
                Size = new System.IO.FileInfo(Path.Value).Length;
            }
        }
        public void Trace()
        {
            Program.Log.Warning("File:");
            Program.Log.Warning("\tPath: " + Path.Value);
            Program.Log.Warning("\tName: " + Name);
            Program.Log.Warning("\tSize: " + Size);
        }
        public void Load()
        {
            Path.loadFromR();
            if (System.IO.File.Exists(Path.Value))
            {
                Update();
                //Name = System.IO.Path.GetFileName(Path.Value);
                //Size = new System.IO.FileInfo(Path.Value).Length;
            }
            else
            {
                Program.Log.Warning("Wrong file path.");
            }
        }
        public void Set(string file_path)
        {
            if (file_path != Path.Value && System.IO.File.Exists(file_path))
            {
                Path.Value = file_path;
                Update();
                //Name = System.IO.Path.GetFileName(Path.Value);
                //Size = new System.IO.FileInfo(Path.Value).Length;
                Trace();
                //Program.Log.Colour();
            }
        }
    }
    class TSelected
    {
        public StringElement Object = new StringElement("ISelectedObject");
        public StringElement Folder = new StringElement("ISelectedFolder");
        public StringElement Project = new StringElement("ISelectedProject");
        public void Load()
        {
            Object.loadFromR();
            Folder.loadFromR();
            Project.loadFromR();
        }
        public string Id = "";
    }
    class Elements
    {
        public THotKeys HotKeys = new THotKeys();

        //hard server data
        public readonly string HSSelectTag = "#SELECTED#";
        public int HSAObject = 0;
        public string HSObjectCurrent = "";

        //public StringElement HSPId = new StringElement(nameof(HSPId),"0");   
        public ProtectedElement HSPassword = new ProtectedElement(nameof(HSPassword));
        //public StringElement HSPassword = new StringElement(nameof(HSPassword));
        public StringElement HSIP = new StringElement(nameof(HSIP));
        public StringElement HSObject = new StringElement(nameof(HSObject));
        public StringElement HSProject = new StringElement(nameof(HSProject));
        public StringElement HSIrSessionId = new StringElement(nameof(HSIrSessionId));


        //iridi parameters
        public TSelected Selected = new TSelected();

        public StringElement IPostHash = new StringElement(nameof(IPostHash), "");
        public StringElement IPostUser = new StringElement(nameof(IPostUser), "");

        public readonly string Uri = "https://iridi.com";
        public readonly string UploadUrl = "/bongo_v2/endpoint.php";

        public ProtectedElement IPassword = new ProtectedElement(nameof(IPassword), "");
        //public StringElement IPassword = new StringElement(nameof(IPassword), "");
        public StringElement ILogin = new StringElement(nameof(ILogin), "");
        public StringElement IPhpSessId = new StringElement(nameof(IPhpSessId), "");
        public StringElement ISessId = new StringElement(nameof(ISessId), "");

        //registry folder
        //public static RegistryKey IridiKey;

        //public static RegistryKey DSendKey;
        //public static RegistryKey UpdateKey;
        //public static RegistryKey DUPKey;

        //program parameters
        public IntElement ShowHelp = new IntElement(nameof(ShowHelp), 1);
        public IntElement IsTrace = new IntElement(nameof(IsTrace), 1);

        public TFile File = new TFile();
        /*
        //file
        public StringElement FilePath   = new StringElement(nameof(FilePath));
        public string FileName { get; set; }
        public long FileSize { get; set; }*/

        //payloads
        public IntElement Counter = new IntElement(nameof(Counter));
        public StringElement UserID = new StringElement(nameof(UserID));
        public StringElement Posthash = new StringElement(nameof(Posthash));
        public readonly string Act = "reload";
        //public StringElement Act = new StringElement(nameof(Act), "reload");
        public readonly string ProjectSetRewrite = "skip_rewrite";
        //public StringElement ProjectSetRewrite = new StringElement(nameof(ProjectSetRewrite), "skip_rewrite");
        public readonly string ParamName = "qqfile";
        public readonly string ContentType = "application/octet-stream";
        //headers
        public StringElement URL = new StringElement(nameof(URL), "https://iridi.com/bongo_v2/endpoint.php");
        public StringElement UserAgent = new StringElement(nameof(UserAgent), "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Mobile Safari/537.36");
        public StringElement Accept = new StringElement(nameof(Accept), "application/json");
        public StringElement Referer = new StringElement(nameof(Referer));
        public StringElement Cookie = new StringElement(nameof(Cookie));



        //public Elements(RichTextBox textLog, TextBox textPath)
            public Elements()
        {          
            
            }
        public void Load()
        {
            //IridiKey = IridiUpload.Memory.Element.GetRFolder();

            //DSendKey = IridiUpload.Memory.Element.GetDFolder();
            //UpdateKey = IridiUpload.Memory.Element.GetUpFolder();
            //DUPKey = IridiUpload.Memory.Element.GetDUPFolder();

            HotKeys.Load();

            IPassword.loadFromR();
            ILogin.loadFromR();

            ShowHelp.loadFromR();
            IsTrace.loadFromR();

            //Hard Server Data
            //HSPId.loadFromR();
            HSPassword.loadFromR();
            HSIP.loadFromR();
            HSObject.loadFromR();
            HSProject.loadFromR();
            HSIrSessionId.loadFromR();

            //Iridi Cloud Data
            Selected.Load();
            IPostHash.loadFromR();
            IPostUser.loadFromR();
            IPhpSessId.loadFromR();
            ISessId.loadFromR();
            Counter.loadFromR();
            UserID.loadFromR();
            Posthash.loadFromR();
            //Act.loadFromR();
            //ProjectSetRewrite.loadFromR();
            URL.loadFromR();
            UserAgent.loadFromR();
            Accept.loadFromR();
            Referer.loadFromR();
            Cookie.loadFromR();

            File.Load();

            //FilePath.loadFromR();
            //ParseFilePath(textLog, textPath);

            //TraceParams(textLog);
            TraceParams();
        }

        //public void TraceParams(RichTextBox textLog)
                public void TraceParams()
            {
                File.Trace();
            //Iridi Cloud Data
            Program.Log.Warning("Iridi cloud:");
            Program.Log.Warning("\tSelected folder: " + Selected.Folder.Value);
            Program.Log.Warning("\tSelected object: " + Selected.Object.Value);
            Program.Log.Warning("\tSelected project: " + Selected.Project.Value);
            //Program.Log.Warning("\tCurrent PID: " + Selected.Id);

            //Program.Log.Warning("Iridi cloud current PID: " + UserID.Value);
            Program.Log.Warning("\tPhpSessId: " + IPhpSessId.Value);
            Program.Log.Warning("\tSessId: " + ISessId.Value);
            Program.Log.Warning("\tPostHash: " + IPostHash.Value);

            //textLog.Text += "IUserID: " + UserID.Value + Environment.NewLine;
            //textLog.Text += "IPhpSessId: " + IPhpSessId.Value + Environment.NewLine;
            //textLog.Text += "ISessId: " + ISessId.Value + Environment.NewLine;

            //textLog.Text += "Counter: " + Counter.Value + Environment.NewLine;

            //textLog.Text += "IPosthash: " + Posthash.Value + Environment.NewLine;
            //textLog.Text += "Act: " + Act.Value + Environment.NewLine;
            //textLog.Text += "ProjectSetRewrite: " + ProjectSetRewrite.Value + Environment.NewLine;
            //textLog.Text += "URL: " + URL.Value + Environment.NewLine;
            //textLog.Text += "UserAgent: " + UserAgent.Value + Environment.NewLine;
            //textLog.Text += "Accept: " + Accept.Value + Environment.NewLine;
            //textLog.Text += "Referer: " + Referer.Value + Environment.NewLine;
            //textLog.Text += "Cookie: " + Cookie.Value + Environment.NewLine;

            //Hard Server Data
            //textLog.Text += "HSPassword: " + HSPassword.Value + Environment.NewLine;
            Program.Log.Warning("Server:");
            Program.Log.Warning("\tSelected object: " + HSObject.Value);
            Program.Log.Warning("\tSelected project: " + HSProject.Value);
            Program.Log.Warning("\tIP: " + HSIP.Value);
            Program.Log.Warning("\tIrSessionId: " + HSIrSessionId.Value);


            //textLog.Text += "HSIP: " + HSIP.Value + Environment.NewLine;
            //textLog.Text += "HSObject: " + HSObject.Value + Environment.NewLine;
            //textLog.Text += "HSProject: " + HSProject.Value + Environment.NewLine;
            //textLog.Text += "HSPId: " + HSPId.Value + Environment.NewLine;
            //textLog.Text += "HSIrSessionId: " + HSIrSessionId.Value + Environment.NewLine;

            //WinForms.Colorize.RichEditColour.ColourIt(textLog);
            
            
            //Program.Log.Colour();
        }

        //public void ParseFilePath(RichTextBox textLog, TextBox textPath)
          /*  public void ParseFilePath()
        {
            if (File.Exists(FilePath.Value))
            {
                //Params.FilePath.Value = textPath.Text;
                FileName = Path.GetFileName(FilePath.Value);
                FileSize = new System.IO.FileInfo(FilePath.Value).Length;

                Program.Log.Warning("File:");
                Program.Log.Warning("\tPath: " + FilePath.Value);
                Program.Log.Warning("\tName: " + FileName);
                Program.Log.Warning("\tSize: " + FileSize);

                //textLog.Text += "File path: " + FilePath.Value + "\r\n";
                //textLog.Text += "File name: " + FileName + "\r\n";
                //textLog.Text += "File size: " + FileSize + "\r\n";

                //WinForms.Colorize.RichEditColour.ColourIt(textLog);
                Program.Log.Colour();
            }
            else
            {
                Program.Log.Warning("Wrong file path.");
                //textLog.Text += "Wrong file path\r\n";
            }

            //textPath.Text = FilePath.Value;
        }

        public void ParseFilePath(string tryFilePath)
        {
            if (File.Exists(tryFilePath))
            {
                FilePath.Value = tryFilePath;
                FileName = Path.GetFileName(FilePath.Value);
                FileSize = new System.IO.FileInfo(FilePath.Value).Length;

                Program.Log.Warning("File:");
                Program.Log.Warning("\tPath: " + FilePath.Value);
                Program.Log.Warning("\tName: " + FileName);
                Program.Log.Warning("\tSize: " + FileSize);

                //textLog.Text += "File path: " + FilePath.Value + "\r\n";
                //textLog.Text += "File name: " + FileName + "\r\n";
                //textLog.Text += "File size: " + FileSize + "\r\n";

                //WinForms.Colorize.RichEditColour.ColourIt(textLog);
                //Program.Log.Colour();
            }
        }*/
    }
}
