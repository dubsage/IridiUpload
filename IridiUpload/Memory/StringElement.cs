using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace IridiUpload.Memory
{
    class StringElement : Element
    {
        private string _value = "";
        private string _regName;

        public StringElement(string regName)
        {
            _regName = regName;
        }

        public StringElement(string regName, string value)
        {
            _regName = regName;
            _value = value;
        }

        public string Value
        {
            //get { if (_value == "") return "undefined"; else return _value; }
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
                _value = (string)Folder.GetValue(_regName);
            }

            else
            {
                saveToR();
                //Elements.IridiKey.SetValue(_regName, _value, RegistryValueKind.String);
            }
        }

        public void saveToR()
        {
            Folder.SetValue(_regName, _value, RegistryValueKind.String);
        }/*
        public void loadFromR()
        {
            if (Elements.IridiKey.GetValue(_regName) != null)
            {
                _value = (string)Elements.IridiKey.GetValue(_regName);
            }

            else
            {
                saveToR();
                //Elements.IridiKey.SetValue(_regName, _value, RegistryValueKind.String);
            }
        }

        public void saveToR()
        {
            Elements.IridiKey.SetValue(_regName, _value, RegistryValueKind.String);
        }*/
    }
}
