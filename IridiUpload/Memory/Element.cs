using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace IridiUpload.Memory
{
    class Element
    {
        const string RPath = "Software\\IridiUpload";

        const string DSendRPath = "Software\\IridiUpload\\HotKeys\\DSend";
        const string UpdateRPath = "Software\\IridiUpload\\HotKeys\\Update";
        const string DUPRPath = "Software\\IridiUpload\\HotKeys\\DUP";

        public RegistryKey Folder;
        public Element()
        {
            Folder = GetRFolder();
        }
        public void ChangeFolder(string path)
        {
            Folder = GetRFolder();
        }
        public void ChangeFolder(int path)
        {
            switch (path)
            {
                case 1:
                    Folder = GetDFolder();
                    break;
                case 2:
                    Folder = GetUpFolder();
                    break;
                case 3:
                    Folder = GetDUPFolder();
                    break;
                default:
                    Folder = GetRFolder();
                    break;
            }
        }

        /*
        virtual public void saveToR(string name)
        {
        }
        virtual public void loadFromR(string name)
        {
        }*/

        public static RegistryKey GetRFolder()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            if (Registry.CurrentUser.OpenSubKey(RPath) == null)
            {
                return currentUserKey.CreateSubKey(RPath);
            }
            else
            {
                return Registry.CurrentUser.OpenSubKey(RPath, true);
            }
        }

        public static RegistryKey GetRFolder(string path_folder)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            if (Registry.CurrentUser.OpenSubKey(path_folder) == null)
            {
                return currentUserKey.CreateSubKey(path_folder);
            }
            else
            {
                return Registry.CurrentUser.OpenSubKey(path_folder, true);
            }
        }

        public static RegistryKey GetDFolder()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            if (Registry.CurrentUser.OpenSubKey(DSendRPath) == null)
            {
                return currentUserKey.CreateSubKey(DSendRPath);
            }
            else
            {
                return Registry.CurrentUser.OpenSubKey(DSendRPath, true);
            }
        }

        public static RegistryKey GetUpFolder()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            if (Registry.CurrentUser.OpenSubKey(UpdateRPath) == null)
            {
                return currentUserKey.CreateSubKey(UpdateRPath);
            }
            else
            {
                return Registry.CurrentUser.OpenSubKey(UpdateRPath, true);
            }
        }

        public static RegistryKey GetDUPFolder()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            if (Registry.CurrentUser.OpenSubKey(DUPRPath) == null)
            {
                return currentUserKey.CreateSubKey(DUPRPath);
            }
            else
            {
                return Registry.CurrentUser.OpenSubKey(DUPRPath, true);
            }
        }
    }
}
