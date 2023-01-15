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

namespace IridiUpload.Logic.Iridi.Cloud
{
    class Data
    {
        Http _http = new Http();
        Authentication auth = new Authentication();
        //Interface _data = null;
        Logic.Iridi.Cloud.Interface.TIridiAuth data;
        Upload upload = new Upload();

        public List<Interface.TObjectFolder> objectFolders = new List<Interface.TObjectFolder>();
        List<Interface.TObject> objecs = new List<Interface.TObject>();

        public int ObjectsCount { get; set; }
        public int ProjectsCount { get; set; }

        public Data()
        {

            

        }
        public bool UploadFile()
        {
            Program.Params.File.Update();
            return upload.UploadFile();
        }
        public bool GetProjects()
        {
            ProjectsCount = 0;
            foreach (var folder in objectFolders)
            {
                //Program.Log.Informational("Iridi folder: " + folder.name);
                //Program.Log.Informational("Number: " + folder.id);
                foreach (var obj in folder.objects)
                {
                    //Program.Log.Informational("\tIridi object: " + obj.name);
                    //Program.Log.Informational("\tNumber: " + obj.id);
                    string html = _http.GetProjectDefault(obj.id);

                    string projects = @"(?<=data-val="")(.*?)(?="").*\n.*(?<=data-id="")(.*?)(?="")";
                    Regex rg_projects = new Regex(projects, RegexOptions.IgnoreCase);
                    MatchCollection matched_projects = rg_projects.Matches(html);

                    List<Interface.TProject> projectsI = new List<Interface.TProject>();
                    foreach (Match match in matched_projects)
                    {
                        if (match.Groups.Count == 3)
                        {
                            Interface.TProject projectI = new Interface.TProject();
                            projectI.name = match.Groups[1].Value;
                            projectI.id = match.Groups[2].Value;
                            projectI.position = match.Index;

                            projectsI.Add(projectI);
                            ProjectsCount++;
                        }
                    }
                    obj.projects = projectsI.ToArray();
                    //Program.Log.Colour();
                }
            }

            Program.Log.Informational("Response:");
            Program.Log.Informational("Iridi cloud folders: ");
            foreach (var folder in objectFolders)
            {
                Program.Log.Informational("\tfolder name: " + folder.name);
                Program.Log.Informational("\tfolder id: " + folder.id);
                Program.Log.Informational("\tIridi cloud objects: ");
                foreach (var obj in folder.objects)
                {
                    Program.Log.Informational("\t\tobject name: " + obj.name);
                    Program.Log.Informational("\t\tobject id: " + obj.id);
                    Program.Log.Informational("\t\tIridi cloud projects: ");
                    foreach (var project in obj.projects)
                    {
                        Program.Log.Informational("\t\t\tproject name: " + project.name);
                        Program.Log.Informational("\t\t\tproject id: " + project.id);
                        Program.Log.Informational("\t\t\t---end---");
                    }
                    Program.Log.Informational("\t\t---end---");
                }
                Program.Log.Informational("\t---end---");
            }
            return true;
        }
        public bool GetObjects()
        {
            string html = _http.Get("/my-account/cloud/");

            //posthash
            string pattern_post_hash = @"(?<=<input\sid=""posthash""\shidden\stype=""text""\svalue="")(.*?)(?="">)";
            Regex rg_post_hash = new Regex(pattern_post_hash, RegexOptions.IgnoreCase);
            MatchCollection matched_post_hash = rg_post_hash.Matches(html);
            if (matched_post_hash.Count > 0)
            {
                Program.Params.IPostHash.Value = matched_post_hash[0].Value;
            }
            Program.Log.Informational("Response:");
            Program.Log.Informational("Post hash: " + Program.Params.IPostHash.Value);

            //postuser
            string pattern_post_user = @"(?<=<input\sid=""postuser""\shidden\stype=""text""\svalue="")(.*?)(?="">)";
            Regex rg_post_user = new Regex(pattern_post_user, RegexOptions.IgnoreCase);
            MatchCollection matched_post_user = rg_post_user.Matches(html);
            if (matched_post_user.Count > 0)
            {
                Program.Params.IPostUser.Value = matched_post_user[0].Value;
            }
            Program.Log.Informational("Post user: " + Program.Params.IPostUser.Value);

            //folders and objects
            objectFolders = new List<Interface.TObjectFolder>();
            ProjectsCount = 0;
            ObjectsCount = 0;
            string pattern_folders = @"(?<=<div.*\n.*\n.*\n.*<div\s*.*\s*class=""text_item""\s*.*\s*>)(.*?)(?=</div>)";
            string pattern_objects = @"(?<=<li.*\n.*\n.*\n.*<div\s*.*\s*class=""text_item""\s*.*\s*>)(.*?)(?=</div>)";
            string pattern_ids = @"(?<=data-id="")(.*?)(?="">)";

            Regex rg_folders = new Regex(pattern_folders, RegexOptions.IgnoreCase);
            Regex rg_objects = new Regex(pattern_objects, RegexOptions.IgnoreCase);
            Regex rg_ids = new Regex(pattern_ids, RegexOptions.IgnoreCase);

            MatchCollection matched_folders = rg_folders.Matches(html);
            MatchCollection matched_objects = rg_objects.Matches(html);
            MatchCollection matched_ids = rg_ids.Matches(html);       

            if (matched_folders.Count > 0)
            {
                //int Counter = 0;
                int Left = matched_folders[0].Index;
                objectFolders.Add(new Interface.TObjectFolder());
                objectFolders[0].name = matched_folders[0].Value;
                objectFolders[0].position = matched_folders[0].Index;

                int i = 1;

                for (i = 1; i < matched_folders.Count; i++)
                {
                    objectFolders.Add(new Interface.TObjectFolder());
                    objectFolders[i].name = matched_folders[i].Value;
                    objectFolders[i].position = matched_folders[i].Index;
                    int Right = matched_folders[i].Index;

                    List<Interface.TObject> obj_es = new List<Interface.TObject>();
                    for (int j = ObjectsCount; j < matched_objects.Count; j++)
                    {
                        if (matched_objects[j].Index > Left && matched_objects[j].Index < Right)
                        {
                            Interface.TObject obj = new Interface.TObject();
                            obj.name = matched_objects[j].Value;
                            obj.position = matched_objects[j].Index;
                            obj_es.Add(obj);
                            //Counter++;
                            ObjectsCount++;
                        }
                    }
                    objectFolders[i - 1].objects = obj_es.ToArray();
                    Left = matched_folders[i].Index;
                }

                List<Interface.TObject> objes = new List<Interface.TObject>();
                for (int j = ObjectsCount; j < matched_objects.Count; j++)
                {
                    if (matched_objects[j].Index > Left)
                    {
                        Interface.TObject obj = new Interface.TObject();
                        obj.name = matched_objects[j].Value;
                        obj.position = matched_objects[j].Index;
                        objes.Add(obj);
                        ObjectsCount++;
                    }
                }
                objectFolders[i - 1].objects = objes.ToArray();
            }

            if (objectFolders != null && objectFolders.Count > 0)
            {
                int Counter = 0;
                for (int i = 0; i < objectFolders.Count; i++)
                {
                    if (matched_ids.Count > Counter)
                    {
                        objectFolders[i].id = matched_ids[Counter].Value;
                        Counter++;
                    }

                    if (objectFolders[i].objects != null)
                    {
                        for (int j = 0; j < objectFolders[i].objects.Length; j++)
                        {
                            if (matched_ids.Count > Counter)
                            {
                                objectFolders[i].objects[j].id = matched_ids[Counter].Value;
                                Counter++;
                            }
                        }
                    }
                }
            }

            //checking
            string GLOBAL_OBJECTSSTRMATCH = @"(?<=var\s*GLOBAL_OBJECTS\s*=\s*{)(.*?)(?=};)";

            Regex rg_GLOBAL_OBJECTS = new Regex(GLOBAL_OBJECTSSTRMATCH, RegexOptions.IgnoreCase);

            MatchCollection matched_GLOBAL_OBJECTSSTR = rg_GLOBAL_OBJECTS.Matches(html);

            //objectFolders = new List<Interface.TObjectFolder>();

            string GLOBAL_OBJECTSSTR = "";
            if (matched_GLOBAL_OBJECTSSTR.Count > 0)
            {
                GLOBAL_OBJECTSSTR = matched_GLOBAL_OBJECTSSTR[0].Value;
                //Program.Log.Informational("GLOBAL_OBJECTSSTR: " + GLOBAL_OBJECTSSTR);
            }

            string components_match = @"""(.*?)"":{""name"":""(.*?)"",""folder_id"":""(.*?)""}";

            Regex rg_components = new Regex(components_match, RegexOptions.IgnoreCase);

            MatchCollection matched_components = rg_components.Matches(GLOBAL_OBJECTSSTR);
            //Program.Log.Informational("ObjectsCount: " + ObjectsCount);
            //Program.Log.Informational("matched_components.Count: " + matched_components.Count);
            if (ObjectsCount == matched_components.Count)
            {
                int Counter = 0;
                foreach (var folder in objectFolders)
                {
                    foreach (var obj in folder.objects)
                    {
                        //Program.Log.Informational("Groups[1]: " + matched_components[Counter].Groups[1].Value);
                        //Program.Log.Informational("Groups[2]: " + matched_components[Counter].Groups[2].Value);
                        //Program.Log.Informational("Groups[3]: " + matched_components[Counter].Groups[3].Value);
                        if (matched_components[Counter].Groups.Count == 4 &&
                             folder.id == matched_components[Counter].Groups[3].Value &&
                             obj.name == matched_components[Counter].Groups[2].Value &&
                             obj.id == matched_components[Counter].Groups[1].Value)
                        {
                            Counter++;
                        }
                        else return false;
                    }
                }
            }
            else return false;

            //Program.Log.Informational("Response:");
            Program.Log.Informational("Iridi cloud folders: ");
            foreach (var folder in objectFolders)
            {
                Program.Log.Informational("\tfolder name: " + folder.name);
                Program.Log.Informational("\tfolder id: " + folder.id);
                Program.Log.Informational("\tIridi cloud objects: ");
                foreach (var obj in folder.objects)
                {
                    Program.Log.Informational("\t\tobject name: " + obj.name);
                    Program.Log.Informational("\t\tobject id: " + obj.id);
                    Program.Log.Informational("\t\t---end---");
                }
                Program.Log.Informational("\t---end---");
            }
            //Program.Log.Colour();

            return true;
        }
        public string Name()
        {
            if (data == null) return "";
            return data.sess.SESS_AUTH.NAME;
        }
        public string Email()
        {
            if (data == null) return "";
            return data.sess.SESS_AUTH.EMAIL;
        }
        public string IP()
        {
            if (data == null) return "";
            return data.sess.SESS_IP;
        }
        public string Country()
        {
            if (data == null) return "";
            return data.sess.SESS_COUNTRY_ID;
        }
        public bool Auth()
        {
            //Logic.Iridi.Cloud.Authentication auth = new Logic.Iridi.Cloud.Authentication();
            string response = auth.HttpAuth();
            data =
                JsonConvert.DeserializeObject<Logic.Iridi.Cloud.Interface.TIridiAuth>(response);
            //auth = null;

            if (data == null) return false;

            Program.Log.Informational("Response:");
            if (data.status == 0)
                Program.Log.Informational("status: ok");
            else
                Program.Log.Informational("status: " + data.status);
            //Program.Log.Informational("uid: " + data.uid);
            //Program.Log.Informational("BX_SESSION_SIGN: " + data.sess.BX_SESSION_SIGN);
            //Program.Log.Informational("fixed_session_id: " + data.sess.fixed_session_id);
            //Program.Log.Informational("SESS_GUEST_ID: " + data.sess.SESS_GUEST_ID);
            //Program.Log.Informational("SESS_SESSION_ID: " + data.sess.SESS_SESSION_ID);
            Program.Log.Informational("user id: " + data.sess.SESS_AUTH.USER_ID);
            //Program.Log.Informational("PASSWORD_HASH: " + data.sess.SESS_AUTH.PASSWORD_HASH);

            if (data.status == 0) return true;
            else
            {
                Program.Log.Informational("Authentication failed");
                return false;
            }
        }
        public bool GetPhpSessionId()
        {
            return _http.GetPhpSessId();
        }
        public bool Connect()
        {
            if (!GetPhpSessionId()) return false;
            if (!Auth()) return false;
            return true;
        }
    }
}
