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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace IridiUpload
{

    public partial class Form1 : Form
    {
        System.Windows.Forms.Keys DKey = 0;
        System.Windows.Forms.Keys UpKey = 0;
        System.Windows.Forms.Keys DUPKey = 0;

        Utility.KeyboardHook hookD = new Utility.KeyboardHook(@"""send to cloud""");
        Utility.KeyboardHook hookUp = new Utility.KeyboardHook(@"""update on server""");
        Utility.KeyboardHook hookDUP = new Utility.KeyboardHook(@"""send and update""");

        //int countD = 0;
        void hookD_KeyPressed(object sender, Utility.KeyPressedEventArgs e)
        {
            //Console.WriteLine(countD++ + " D");
            string message = DateTime.Now.ToString(@"h:mm:ss tt") + @" Got ""send to cloud"" shortcut.";
            Program.Log.Notice(message);
            if (chDUse.Checked) btnSendIridi.PerformClick();
        }

        //int countUp = 0;
        void hookUp_KeyPressed(object sender, Utility.KeyPressedEventArgs e)
        {
            string message = DateTime.Now.ToString(@"h:mm:ss tt") + @" Got ""update on server"" shortcut.";
            Program.Log.Notice(message);
            if (chUpUse.Checked) btnUpdate.PerformClick();
        }

        //int countDUP = 0;
        void hookDUP_KeyPressed(object sender, Utility.KeyPressedEventArgs e)
        {
            string message = DateTime.Now.ToString(@"h:mm:ss tt") + @" Got ""send and update"" shortcut.";
            Program.Log.Notice(message);
            if (chDUPUse.Checked) btnSendAndUpdate.PerformClick();
        }
        public Form1()
        {
            //ServicePointManager.DefaultConnectionLimit = 1000000000;
            InitializeComponent();

            hookD.KeyPressed += new EventHandler<Utility.KeyPressedEventArgs>(hookD_KeyPressed);
            hookUp.KeyPressed += new EventHandler<Utility.KeyPressedEventArgs>(hookUp_KeyPressed);
            hookDUP.KeyPressed += new EventHandler<Utility.KeyPressedEventArgs>(hookDUP_KeyPressed);

            Program.HS = new Logic.HardServer.Data();
            Program.ICloud = new Logic.Iridi.Cloud.Data();

            dataProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataProjects.MultiSelect = false;
            dataProjects.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dataProjects_RowPrePaint);

            dataIProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataIProjects.MultiSelect = false;
            dataIProjects.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dataIProjects_RowPrePaint);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textPath.Text = openFileDialog1.FileName;
            }
        }



        private void textPath_TextChanged(object sender, EventArgs e)
        {
            if (Program.Params != null)
                Program.Params.File.Set(textPath.Text);
        }

        void ShowHelp()
        {
            new FHelp().ShowDialog();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Program.Log = new Utility.Logging(textLog);
            Program.Params = new Memory.Elements();
            Program.Params.Load();


            hookD.Grab = false;
            hookUp.Grab = false;
            hookDUP.Grab = false;

            chDCtrl.Checked = Program.Params.HotKeys.DSend.Ctrl.Bool();
            chDAlt.Checked = Program.Params.HotKeys.DSend.Alt.Bool();
            chDShift.Checked = Program.Params.HotKeys.DSend.Shift.Bool();
            chDWin.Checked = Program.Params.HotKeys.DSend.Win.Bool();
            chDUse.Checked = Program.Params.HotKeys.DSend.Use.Bool();
            DKey = (Keys)Program.Params.HotKeys.DSend.Sign.Value;
            if (Program.Params.HotKeys.DSend.Sign.Value == 0) textDKey.Text = "";
            else textDKey.Text = DKey.ToString();
            hookD.Grab = true;

            if (chDCtrl.Checked || chDAlt.Checked || chDShift.Checked || chDWin.Checked || textDKey.Text != "")
                SetDKeys();

            chUpCtrl.Checked = Program.Params.HotKeys.Update.Ctrl.Bool();
            chUpAlt.Checked = Program.Params.HotKeys.Update.Alt.Bool();
            chUpShift.Checked = Program.Params.HotKeys.Update.Shift.Bool();
            chUpWin.Checked = Program.Params.HotKeys.Update.Win.Bool();
            chUpUse.Checked = Program.Params.HotKeys.Update.Use.Bool();
            UpKey = (Keys)Program.Params.HotKeys.Update.Sign.Value;
            if (Program.Params.HotKeys.Update.Sign.Value == 0) textUpKey.Text = "";
            else textUpKey.Text = UpKey.ToString();
            hookUp.Grab = true;

            if (chUpCtrl.Checked || chUpAlt.Checked || chUpShift.Checked || chUpWin.Checked || textUpKey.Text != "")
                SetUpKeys();


            chDUPCtrl.Checked = Program.Params.HotKeys.DUP.Ctrl.Bool();
            chDUPAlt.Checked = Program.Params.HotKeys.DUP.Alt.Bool();
            chDUPShift.Checked = Program.Params.HotKeys.DUP.Shift.Bool();
            chDUPWin.Checked = Program.Params.HotKeys.DUP.Win.Bool();
            chDUPUse.Checked = Program.Params.HotKeys.DUP.Use.Bool();
            DUPKey = (Keys)Program.Params.HotKeys.DUP.Sign.Value;
            if (Program.Params.HotKeys.DUP.Sign.Value == 0) textDUPKey.Text = "";
            else textDUPKey.Text = DUPKey.ToString();
            hookDUP.Grab = true;

            if (chDUPCtrl.Checked || chDUPAlt.Checked || chDUPShift.Checked || chDUPWin.Checked || textDUPKey.Text != "")
                SetDUPKeys();



            textPath.Text = Program.Params.File.Path.Value;

            textHSIP.Text = Program.Params.HSIP.Value;
            textHSPassword.Text = Program.Params.HSPassword.Value;

            textILogin.Text = Program.Params.ILogin.Value;
            textIPass.Text = Program.Params.IPassword.Value;

            if (Program.Params.HSIP.Value != "" && Program.Params.HSPassword.Value != "")
            {
                Program.HS.Auth();
            }

            //Program.Log.Colour();

            if (Program.Params.ShowHelp.Value == 1)
            {
                ShowHelp();
            }
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {         
            //Logic.Parse.All(textHeader.Text, textPayload.Text, textLog);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!ICloudProjects()) Program.Log.Warning("Iridi Cloud projects were not received");
            else
            {
                if (Program.Params.Selected.Id != "wrong")
                {
                    Program.Log.Warning("Selected.Id: " + Program.Params.Selected.Id);
                    Program.ICloud.UploadFile();
                }
            }
            //Program.Log.Colour();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            Program.Params.TraceParams();
            //Program.Log.Colour();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Program.Params.HSObject.Value == "" || Program.Params.HSProject.Value == "")
            {
                Program.Log.Error("Project didnt seleced");
                Program.Log.Notice("Object name: " + Program.Params.HSObject.Value);
                Program.Log.Notice("Project name: " + Program.Params.HSProject.Value);
                //Program.Log.Colour();
                return;
            }

            if (HSGetObjects())
                Program.HS.SendCommandToDowloaded();

            //Program.Log.Colour();
        }
        
        void HSHighLiteSelected()
        {
            foreach (DataGridViewRow row in dataProjects.Rows)
            {
                if (row.Cells[1].Value.ToString() == Program.Params.HSObject.Value
                    && row.Cells[2].Value.ToString() == Program.Params.HSProject.Value)
                    row.Cells[0].Value = Program.Params.HSSelectTag;
                else
                    row.Cells[0].Value = "";
            }
        }

        void IHighLiteSelected()
        {
            foreach (DataGridViewRow row in dataIProjects.Rows)
            {
                string selected = "";

                if (row.Cells[1].Value.ToString() == Program.Params.Selected.Folder.Value &&
                    row.Cells[3].Value.ToString() == Program.Params.Selected.Object.Value &&
                    row.Cells[5].Value.ToString() == Program.Params.Selected.Project.Value)
                {
                    selected = Program.Params.HSSelectTag;
                }
                row.Cells[0].Value = selected;
            }
        }
        bool HSGetObjects()
        {
            if (Program.Params.HSPassword.Value != textHSPassword.Text ||
                Program.Params.HSIP.Value != textHSIP.Text)
            {
                Program.Params.HSPassword.Value = textHSPassword.Text;
                Program.Params.HSIP.Value = textHSIP.Text;

                Program.HS.Auth();
            }

            if (!Program.HS.Authentification)
            {
                Program.Log.Notice("Authentication is failed");
                return false;
            }

            Program.HS.GetProjects(dataProjects);

            //HSHighLiteSelected();

            lblAttachedObject.Text = Program.HS.AttachedObject();
            lblUserName.Text = Program.HS.UserName();
            lblUserLogin.Text = Program.HS.UserLogin();
            lblLastUpdate.Text = Program.HS.LastUpdate();

            return true;
        }
        private void btnHSGetObjects_Click(object sender, EventArgs e)
        {
            HSGetObjects();
            //Program.Log.Colour(); 
        }
        
        private void timerHS_Tick(object sender, EventArgs e)
        {
            if (Program.Params.HSIP.Value != "" && Program.Params.HSPassword.Value != "")
                Program.HS.Auth();
                //HttpHSAuth();
        }


        private void dataProjects_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        private void btnHSSelect_Click(object sender, EventArgs e)
        {
            int index = -1;
            Int32 selectedCellCount = dataProjects.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                if (dataProjects.AreAllCellsSelected(true))
                {
                    MessageBox.Show("All cells are selected", "Selected Cells");
                }
                else
                {
                    index = dataProjects.SelectedCells[0].RowIndex;
                    Program.Params.HSObject.Value = dataProjects.Rows[index].Cells[1].Value.ToString();
                    Program.Params.HSProject.Value = dataProjects.Rows[index].Cells[2].Value.ToString();

                    //Program.Params.HSPId.Value = dataProjects.Rows[index].Cells[2].Value.ToString();


                    Program.Log.Notice("Selected: ");
                    Program.Log.Notice("\tObject name: " + Program.Params.HSObject.Value);
                    Program.Log.Notice("\tProject name: " + Program.Params.HSProject.Value);
                    //Program.Log.Notice("Pid: " + Program.Params.HSPId.Value);

                    //Program.Log.Colour();
                    //WinForms.Colorize.RichEditColour.ColourIt(textLog);
                }
            }
            else MessageBox.Show("No cells are selected", "Selected Cells");
            HSHighLiteSelected();
        }

        private void textLog_TextChanged(object sender, EventArgs e)
        {
            if (textLog.Text.Length > 10000)
            {
                textLog.Text = "The oldest data was removed. Continue...\r\n" + textLog.Text.Substring(1000);
                Program.Log.Colour();
            }
        }





        public bool ICloudProjects()
        {
            if (textILogin.Text != "")
            {
                Program.Params.ILogin.Value = textILogin.Text;
            }
            if (textIPass.Text != "")
            {
                Program.Params.IPassword.Value = textIPass.Text;
            }
            Program.Params.Selected.Id = "wrong";

            if (!Program.ICloud.GetPhpSessionId()) return false;
            if (!Program.ICloud.Auth()) return false;
            if (!Program.ICloud.GetObjects()) return false;
            if (!Program.ICloud.GetProjects()) return false;

            //dataIProjects.ClearSelection();
            dataIProjects.Rows.Clear();
            
            foreach (Logic.Iridi.Cloud.Interface.TObjectFolder folder in Program.ICloud.objectFolders)
            {
                foreach (Logic.Iridi.Cloud.Interface.TObject obj in folder.objects)
                {
                    foreach (Logic.Iridi.Cloud.Interface.TProject project in obj.projects)
                    {
                        string selected = "";

                        if (folder.name == Program.Params.Selected.Folder.Value &&
                            obj.name == Program.Params.Selected.Object.Value &&
                            project.name == Program.Params.Selected.Project.Value)
                        {
                            selected = Program.Params.HSSelectTag;
                            Program.Params.Selected.Id = project.id;
                        }

                        dataIProjects.Rows.Add(selected, folder.name, folder.id, obj.name, obj.id, project.name, project.id);
                    }
                }
            }
            lblIName.Text = Program.ICloud.Name();
            lblIEmail.Text = Program.ICloud.Email();
            lblIIP.Text = Program.ICloud.IP();
            lblICountry.Text = Program.ICloud.Country();

            return true;
        }
        private void btnGetProjects_Click(object sender, EventArgs e)
        {
            if (!ICloudProjects()) Program.Log.Warning("Iridi Cloud projects were not received");
            //Program.Log.Colour();
        }

        private void btnISelect_Click(object sender, EventArgs e)
        {
            int index = -1;
            Int32 selectedCellCount = dataIProjects.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                if (dataIProjects.AreAllCellsSelected(true))
                {
                    MessageBox.Show("All cells are selected", "Selected Cells");
                }
                else
                {
                    index = dataIProjects.SelectedCells[0].RowIndex;
                    Program.Params.Selected.Folder.Value = dataIProjects.Rows[index].Cells[1].Value.ToString();
                    Program.Params.Selected.Object.Value = dataIProjects.Rows[index].Cells[3].Value.ToString();
                    Program.Params.Selected.Project.Value = dataIProjects.Rows[index].Cells[5].Value.ToString();
                    //Program.Params.Selected.Id = dataIProjects.Rows[index].Cells[6].Value.ToString();

                    //Program.Params.HSPId.Value = dataProjects.Rows[index].Cells[2].Value.ToString();


                    Program.Log.Notice("Selected iridi project: ");
                    Program.Log.Notice("\tFolder name: " + Program.Params.Selected.Folder.Value);
                    Program.Log.Notice("\tObject name: " + Program.Params.Selected.Object.Value);
                    Program.Log.Notice("\tProject name: " + Program.Params.Selected.Project.Value);
                    //Program.Log.Notice("Pid: " + Program.Params.HSPId.Value);

                    //Program.Log.Colour();
                    //WinForms.Colorize.RichEditColour.ColourIt(textLog);
                }
            }
            else MessageBox.Show("No cells are selected", "Selected Cells");
            IHighLiteSelected();
        }

        private void dataIProjects_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        private void btnSendAndUpdate_Click(object sender, EventArgs e)
        {
            if (!ICloudProjects()) Program.Log.Warning("Iridi Cloud projects were not received");
            else
            {
                if (Program.Params.Selected.Id != "wrong")
                {
                    Program.Log.Warning("Selected.Id: " + Program.Params.Selected.Id);
                    if (Program.ICloud.UploadFile())
                    {

                        if (Program.Params.HSObject.Value == "" || Program.Params.HSProject.Value == "")
                        {
                            Program.Log.Error("Project didnt selected");
                            Program.Log.Notice("Object name: " + Program.Params.HSObject.Value);
                            Program.Log.Notice("Project name: " + Program.Params.HSProject.Value);
                            //Program.Log.Colour();
                            return;
                        }

                        if (HSGetObjects())
                            Program.HS.SendCommandToDowloaded();
                    }
                }
                else
                {
                    Program.Log.Warning("Selected project could not be found.");
                    Program.Log.Warning("ICloud folder: " + Program.Params.Selected.Folder.Value);
                    Program.Log.Warning("ICloud object: " + Program.Params.Selected.Object.Value);
                    Program.Log.Warning("ICloud project: " + Program.Params.Selected.Project.Value);
                }
            }
            //Program.Log.Colour();
        }
        void DR()
        {
            Program.Params.HotKeys.DSend.Set(chDCtrl.Checked, chDShift.Checked, chDAlt.Checked,
                chDWin.Checked, chDUse.Checked, (int)DKey);
        }
        void UpR()
        {
            Program.Params.HotKeys.Update.Set(chUpCtrl.Checked, chUpShift.Checked, chUpAlt.Checked,
                chUpWin.Checked, chUpUse.Checked, (int)UpKey);
        }
        void DUPR()
        {
            Program.Params.HotKeys.DUP.Set(chDUPCtrl.Checked, chDUPShift.Checked, chDUPAlt.Checked,
                chDUPWin.Checked, chDUPUse.Checked, (int)DUPKey);
        }
        void SetDKeys()
        {
            //KeyEventArgs e;
            //e = (KeyEventArgs)textUpKey.Text;
            Utility.ModifierKeys modifier = 0;
            if (chDCtrl.Checked) modifier |= Utility.ModifierKeys.Control;   
            if (chDShift.Checked) modifier |= Utility.ModifierKeys.Shift;
            if (chDAlt.Checked) modifier |= Utility.ModifierKeys.Alt;
            if (chDWin.Checked) modifier |= Utility.ModifierKeys.Win;

            if (hookD.Grab)
            {
                DR(); hookD.Reg(modifier, DKey);
            }
        }

        void SetUpKeys()
        {
            //Console.WriteLine("asd " + UpKey.ToString());
            //Console.WriteLine("asd " + (int)UpKey);
            Utility.ModifierKeys modifier = 0;
            if (chUpCtrl.Checked) modifier |= Utility.ModifierKeys.Control;
            if (chUpShift.Checked) modifier |= Utility.ModifierKeys.Shift;
            if (chUpAlt.Checked) modifier |= Utility.ModifierKeys.Alt;
            if (chUpWin.Checked) modifier |= Utility.ModifierKeys.Win;

            if (hookUp.Grab)
            {
                UpR();
                hookUp.Reg(modifier, UpKey);
            }
        }

        void SetDUPKeys()
        {
            Utility.ModifierKeys modifier = 0;
            if (chDUPCtrl.Checked) modifier |= Utility.ModifierKeys.Control;
            if (chDUPShift.Checked) modifier |= Utility.ModifierKeys.Shift;
            if (chDUPAlt.Checked) modifier |= Utility.ModifierKeys.Alt;
            if (chDUPWin.Checked) modifier |= Utility.ModifierKeys.Win;

            //IntPtr myWindowHandle = IntPtr(this.Handle);
            //IWin32Window ^ w = Control::FromHandle(myWindowHandle);
            if (hookDUP.Grab)
            {
                DUPR(); hookDUP.Reg(modifier, DUPKey);
            }
            //hookDUP.RegisterHotKey(modifier, DUPKey, Control::FromHandle(IntPtr(this.Handle)))
        }

        private void textDKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) { chDCtrl.Checked = !chDCtrl.Checked; return; }
            if (e.KeyCode == Keys.ShiftKey) { chDShift.Checked = !chDShift.Checked; return; }
            if (e.KeyCode == Keys.Menu) { chDAlt.Checked = !chDAlt.Checked; return; }
            if (e.KeyCode == Keys.LWin) { chDWin.Checked = !chDWin.Checked; return; }
            if (e.KeyCode == Keys.RWin) { chDWin.Checked = !chDWin.Checked; return; }
            DKey = e.KeyCode;
            textDKey.Text = e.KeyCode.ToString();
        }

        private void chDCtrl_CheckedChanged(object sender, EventArgs e)
        {
            SetDKeys();
        }

        private void chDShift_CheckedChanged(object sender, EventArgs e)
        {
            SetDKeys();
        }

        private void chDAlt_CheckedChanged(object sender, EventArgs e)
        {
            SetDKeys();
        }

        private void chDWin_CheckedChanged(object sender, EventArgs e)
        {
            SetDKeys();
        }

        private void textDKey_TextChanged(object sender, EventArgs e)
        {
            if (textDKey.Text == "") DKey = 0;
            SetDKeys();
        }

        private void chUpCtrl_CheckedChanged(object sender, EventArgs e)
        {
            SetUpKeys();
        }

        private void chUpShift_CheckedChanged(object sender, EventArgs e)
        {
            SetUpKeys();
        }

        private void chUpAlt_CheckedChanged(object sender, EventArgs e)
        {
            SetUpKeys();
        }

        private void chUpWin_CheckedChanged(object sender, EventArgs e)
        {
            SetUpKeys();
        }

        private void textUpKey_TextChanged(object sender, EventArgs e)
        {
            if (textUpKey.Text == "") UpKey = 0;
            SetUpKeys();
        }

        private void textUpKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) { chUpCtrl.Checked = !chUpCtrl.Checked; return; }
            if (e.KeyCode == Keys.ShiftKey) { chUpShift.Checked = !chUpShift.Checked; return; }
            if (e.KeyCode == Keys.Menu) { chUpAlt.Checked = !chUpAlt.Checked; return; }
            if (e.KeyCode == Keys.LWin) { chUpWin.Checked = !chUpWin.Checked; return; }
            if (e.KeyCode == Keys.RWin) { chUpWin.Checked = !chUpWin.Checked; return; }

            UpKey = e.KeyCode;
            textUpKey.Text = e.KeyCode.ToString();
        }

        private void btnDDelete_Click(object sender, EventArgs e)
        {
            hookD.Grab = false;
            chDAlt.Checked = false;
            chDShift.Checked = false;
            chDCtrl.Checked = false;
            chDWin.Checked = false;
            textDKey.Text = "";
            hookD.UnReg();
            hookD.Grab = true;
            DR();
        }

        private void btnUpDelete_Click(object sender, EventArgs e)
        {
            hookUp.Grab = false;
            chUpAlt.Checked = false;
            chUpShift.Checked = false;
            chUpCtrl.Checked = false;
            chUpWin.Checked = false;
            textUpKey.Text = "";
            hookUp.UnReg();
            hookUp.Grab = true;
            UpR();
        }

        private void btnDUPDelete_Click(object sender, EventArgs e)
        {
            hookDUP.Grab = false;
            chDUPAlt.Checked = false;
            chDUPShift.Checked = false;
            chDUPCtrl.Checked = false;
            chDUPWin.Checked = false;
            textDUPKey.Text = "";
            hookDUP.UnReg();
            hookDUP.Grab = true;
            DUPR();
        }

        private void textDUPKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) { chDUPCtrl.Checked = !chDUPCtrl.Checked; return; }
            if (e.KeyCode == Keys.ShiftKey) { chDUPShift.Checked = !chDUPShift.Checked; return; }
            if (e.KeyCode == Keys.Menu) { chDUPAlt.Checked = !chDUPAlt.Checked; return; }
            if (e.KeyCode == Keys.LWin) { chDUPWin.Checked = !chDUPWin.Checked; return; }
            if (e.KeyCode == Keys.RWin) { chDUPWin.Checked = !chDUPWin.Checked; return; }
            DUPKey = e.KeyCode;
            textDUPKey.Text = e.KeyCode.ToString();
        }

        private void chDUPCtrl_CheckedChanged(object sender, EventArgs e)
        {
            SetDUPKeys();
        }

        private void chDUPShift_CheckedChanged(object sender, EventArgs e)
        {
            SetDUPKeys();
        }

        private void chDUPAlt_CheckedChanged(object sender, EventArgs e)
        {
            SetDUPKeys();
        }

        private void chDUPWin_CheckedChanged(object sender, EventArgs e)
        {
            SetDUPKeys();
        }

        private void textDUPKey_TextChanged(object sender, EventArgs e)
        {
            if (textDUPKey.Text == "") DUPKey = 0;
            SetDUPKeys();
        }

        private void chDUse_CheckedChanged(object sender, EventArgs e)
        {
            if (hookD.Grab) DR();
        }

        private void chUpUse_CheckedChanged(object sender, EventArgs e)
        {
            if (hookUp.Grab) UpR();
        }

        private void chDUPUse_CheckedChanged(object sender, EventArgs e)
        {
            if (hookDUP.Grab) DUPR();
        }
    }
}
