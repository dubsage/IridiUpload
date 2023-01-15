using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;

namespace IridiUpload.Memory
{
    class IntElement : Element
    {
        private int _value = 0;
        private string _regName;

        public IntElement(string regName)
        {
            _regName = regName;
        }
        public IntElement(string regName, int value)
        {
            _regName = regName;
            _value = value;
        }
        public int  Value
        {
            get { return _value; }
            set
            {
                //...
                _value = value;
                saveToR();
            }
        }

        public bool Bool()
        {
            if (Value > 0) return true;
                    return false;
        }
        public void Set (bool value)
        {
            if (value) Value = 1;
            else Value = 0;
        }
        
            public void loadFromR()
        {
            if (Folder.GetValue(_regName) != null)
            {
                _value = (int)Folder.GetValue(_regName);
            }
            else
            {
                Folder.SetValue(_regName, _value, RegistryValueKind.DWord);
            }
        }
        public void saveToR()
        {
            Folder.SetValue(_regName, _value, RegistryValueKind.DWord);
        }

        /*
        public void loadFromR()
        {            
            if (Elements.IridiKey.GetValue(_regName) != null)
            {
                _value = (int)Elements.IridiKey.GetValue(_regName); 
            }
            else
            {
                Elements.IridiKey.SetValue(_regName, _value, RegistryValueKind.DWord);
            }
        }*/
        /*
        public void saveToR()
        {
            Elements.IridiKey.SetValue(_regName, _value, RegistryValueKind.DWord);
        }*/
    }
}
