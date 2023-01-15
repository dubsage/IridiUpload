using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace IridiUpload.Memory
{
    class ProtectedElement : Element
    {
        private string _value = "";
        private string _regName;

        public ProtectedElement(string regName)
        {
            _regName = regName;
        }

        public ProtectedElement(string regName, string value)
        {
            _regName = regName;
            _value = value;
        }

        public string Value
        {
            get { if (_value == null) return ""; else return _value; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _value = value;
                saveToR();
            }
        }
        public void loadFromR()
        {
            if (Folder.GetValue(_regName) != null)
            {
                byte[] reg_value = (byte[])Folder.GetValue(_regName);
                byte[] uncrypt = ProtectionData.Unprotect(reg_value);
                _value = Encoding.Unicode.GetString(uncrypt);
            }

            else
            {
                saveToR();
            }
        }

        public void saveToR()
        {
            byte[] bytes = Encoding.Unicode.GetBytes(_value);
            byte[] crypt = ProtectionData.Protect(bytes);
            Folder.SetValue(_regName, crypt, RegistryValueKind.Binary);
        }/*
        public void loadFromR()
        {
            if (Elements.IridiKey.GetValue(_regName) != null)
            {
                byte[] reg_value = (byte[])Elements.IridiKey.GetValue(_regName);
                byte[] uncrypt = ProtectionData.Unprotect(reg_value);
                _value = Encoding.Unicode.GetString(uncrypt);
            }

            else
            {
                saveToR();
            }
        }

        public void saveToR()
        {
            byte[] bytes = Encoding.Unicode.GetBytes(_value);
            byte[] crypt = ProtectionData.Protect(bytes);
            Elements.IridiKey.SetValue(_regName, crypt, RegistryValueKind.Binary);
        }*/
    }
}
