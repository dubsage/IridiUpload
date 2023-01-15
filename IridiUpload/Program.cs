using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IridiUpload
{
    static class Program
    {
        public static Memory.Elements Params;
        public static Utility.Logging Log;
        public static Logic.HardServer.Data HS;
        public static Logic.Iridi.Cloud.Data ICloud;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
