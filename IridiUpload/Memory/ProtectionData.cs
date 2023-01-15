using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace IridiUpload.Memory
{
    class ProtectionData
    {

        // Create byte array for additional entropy when using Protect method.
        static byte[] s_additionalEntropy = { 9, 8, 7, 6, 5 };

        public static string Protect(string data)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            byte[] crypt = Protect(bytes);
            if (crypt == null) return "";
            else return Encoding.Unicode.GetString(crypt, 0, crypt.Length);
        }

        public static string Unprotect(string data)
        {
            byte[]  bytes = Encoding.Unicode.GetBytes(data);
            byte[] response = Unprotect(bytes);
            if (response == null) return "";
            else return Encoding.Unicode.GetString(response, 0, response.Length);
        }

        public static byte[] Protect(byte[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                // only by the same current user.
                return ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (Exception)
            {
                try
                {
                    return ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.LocalMachine);
                }
                catch (CryptographicException er)
                {
                    Program.Log.Error("Data was not encrypted. An error occurred.\r\n" + er);
                    return null;
                }
            }
        }

        public static byte[] Unprotect(byte[] data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                return ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (Exception)
            {
                try
                {
                    return ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.LocalMachine);
                }
                catch (CryptographicException er)
                {
                    Program.Log.Error("Data was not decrypted. An error occurred.\r\n" + er);
                    return null;
                }
            }
        }

        public static void PrintValues(string data)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            PrintValues(bytes);
        }

        public static void PrintValues(Byte[] myArr)
        {
            string s = "";
            foreach (Byte i in myArr)
            {
                s += "{" + i + "}\t";
            }
            Console.WriteLine("Protected data: " + s);
            Program.Log.Warning("Protected data: " + s);
        }
    }
}
