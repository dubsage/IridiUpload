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
    class Data
    {
        Authentication auth = new Authentication();
        public bool Authentification { get; set; }
        Http _http = new Http();
        Interface _data = null;
        public Data()
        {
            Authentification = false;
        }
        //"json/user_update/cloud/user/update/set"
        //Interface.UserUpdate
        public T Get<T>(string url) where T : Interface.TBase, new()
        {
            try
            {
                string response = _http.Get(url);
                T data = JsonConvert.DeserializeObject<T>(response);
                if (data != null) data.Trace();
                return data;
            }
            catch (Exception e)
            {
                Program.Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + " " + e);
            }
            return default(T);
        }

        void ObjectUpdate()
        {
            try
            {
                string response = _http.Get("json/updateobjects/cloud/objects/update/get");
                Interface.TObjectUpdate objectUpdate = JsonConvert.DeserializeObject<Interface.TObjectUpdate>(response);
                objectUpdate.Trace();
            }
            catch (Exception e)
            {
                Program.Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + " " + e);
            }
        }

        void GetUser()
        {
            try
            {
                string response = _http.Get("json/user/cloud/user/get");
                Interface.TUser user = JsonConvert.DeserializeObject<Interface.TUser>(response);
                user.Trace();
                
                string message = "login: " + user.user_login + "\r\n";
                message += "name: " + user.user_name + "\r\n";
                message += "last update: " + user.user_last_update + "\r\n";
                message += "object: " + user.attached_object + "\r\n";

                Program.Params.HSAObject = user.attached_object;
            }
            catch (Exception e)
            {
                Program.Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + " " + e);
            }
        }

        public string UserLogin()
        {
            if (_data != null && _data.User != null && _data.User.user_login != null)
                return _data.User.user_login;
            return "";
        }

        public string UserName()
        {
            if (_data != null && _data.User != null && _data.User.user_name != null)
                return _data.User.user_name;
            return "";
        }

        public string LastUpdate()
        {
            if (_data != null && _data.User != null && _data.User.user_last_update != null)
                return _data.User.user_last_update;
            return "";
        }

        public string AttachedObject()
        {
            if (_data != null && _data.User != null && _data.Object != null)
            {
                foreach(var obj in _data.Object.objects)
                {
                    if (obj.object_id == _data.User.attached_object)
                    {
                        return obj.object_name;
                    }
                }
            }
            return "";
        }

        public bool Auth()
        {
            //Authentication auth = new Authentication();
            return Authentification = auth.HttpHSAuth();
            //auth = null;
        }

        public void SendCommandToDowloaded()
        {
            foreach (var obj in _data.Object.objects)
            {
                foreach (var project in obj.projects)
                {
                    if (obj.object_name== Program.Params.HSObject.Value
                    && project.project_name == Program.Params.HSProject.Value)
                    {
                        //Program.Params.HSPId.Value = project.project_id;
                        _data.ProjectDownloadedStatus = Get<Interface.TProjectDownloadedStatus>
                            ("json/download/cloud/objects/object/project/download/get?id=" + project.project_id);

                        if (_data.ProjectDownloadedStatus.project_status == 1)
                            Program.Log.Notice("Project downloaded");
                        else
                            Program.Log.Notice("Project didnt downloaded. Status: " + _data.ProjectDownloadedStatus.project_status);

                        //WinForms.Colorize.RichEditColour.ColourIt(textLog);
                        return;

                    }
                }
            }
            Program.Log.Error("Project didnt Finded. Try to select another");
            Program.Log.Notice("Object name: " + Program.Params.HSObject.Value);
            Program.Log.Notice("Project name: " + Program.Params.HSProject.Value);

            /*
        foreach (DataGridViewRow row in dataProjects.Rows)
        {
            if (row.Cells[0].Value.ToString() == Program.Params.HSObject.Value
                && row.Cells[1].Value.ToString() == Program.Params.HSProject.Value)
            {
                Program.Params.HSPId.Value = row.Cells[2].Value.ToString();
                HttpHSGet("json/download/cloud/objects/object/project/download/get?id=" + Program.Params.HSPId.Value);

                string response = HttpHSGet("json/download/cloud/objects/object/project/download/get?id=" + Program.Params.HSPId.Value);

                HSProjectDownloadedStatus hsPDStatus = JsonConvert.DeserializeObject<HSProjectDownloadedStatus>(response);

                if (hsPDStatus.project_status == 1)
                    Program.Log.Notice("Project downloaded");
                else
                    Program.Log.Notice("Project didnt downloaded. Status: " + hsPDStatus.project_status);

                WinForms.Colorize.RichEditColour.ColourIt(textLog);
                return;
            }
        }

        Program.Log.Error("Project didnt Finded. Try to select another");
        Program.Log.Notice("Object name: " + Program.Params.HSObject.Value);
        Program.Log.Notice("Project name: " + Program.Params.HSProject.Value);*/
        }
        public void GetProjects(DataGridView dataProjects)
        {
            _data = new Interface();
            _data.UserUpdate = Get<Interface.TUserUpdate>("json/user_update/cloud/user/update/set");

            //Program.Log.Colour();
            Thread.Sleep(500);
            _data.ObjectUpdate = Get<Interface.TObjectUpdate>("json/updateobjects/cloud/objects/update/get");

            //Program.Log.Colour();
            Thread.Sleep(500);
            _data.User = Get<Interface.TUser>("json/user/cloud/user/get");

            dataProjects.Rows.Clear();
            for (int i = 0; i < 20; i++)
            {
                bool finded = false;

                //Program.Log.Colour();
                Thread.Sleep(500);
                _data.Object = Get<Interface.TObject>("json/objects/cloud/objects/get");

                if (_data.Object == null) return;
                if (_data.Object.objects_last_update != "") finded = true;

                List<Interface.TObjectDetail> objects = new List<Interface.TObjectDetail>();
                foreach (Interface.TObjectDetail obj in _data.Object.objects)
                {
                    finded = true;
                    Interface.TObjectDetail elementObject = Get<Interface.TObjectDetail>("json/projects/cloud/objects/object/get?id=" + obj.object_id);
                    foreach (Interface.TProject project in elementObject.projects)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        if (elementObject.object_name == Program.Params.HSObject.Value &&
                            project.project_name == Program.Params.HSProject.Value)
                            dataProjects.Rows.Add(Program.Params.HSSelectTag, elementObject.object_name, project.project_name, project.project_id);
                        else
                            dataProjects.Rows.Add("", elementObject.object_name, project.project_name, project.project_id);
                    }
                    objects.Add(elementObject);
                }
                if (finded)
                {
                    _data.Object.objects = objects.ToArray();
                    break;
                }
            }
        }
    }
}
