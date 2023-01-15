using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IridiUpload
{
    public partial class FHelp : Form
    {
        //public static int IsShowForm = 1;
        public FHelp()
        {
            InitializeComponent();

            if (Program.Params.ShowHelp.Value == 1) checkBox1.Checked = false;
            else checkBox1.Checked = true;


            /*
            textH.Select(76, 54);
            textH.SelectionColor = Color.LightGray;

            textH.Select(988, 334); 
            textH.SelectionFont = new Font(textH.Font, FontStyle.Bold);

            textH.Select(0, 0);*/

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) 
                Program.Params.ShowHelp.Value = 0;
            else 
                Program.Params.ShowHelp.Value = 1;
        }

        private void FHelp_Load(object sender, EventArgs e)
        {

        }
    }
}
