using Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class LegacyRegistryHandler
    {
        string baseReg;
        string baseShell;

        const string commandTemplate = "\"{0}\" \"%1\" {1}";
        public LegacyRegistryHandler(string baseRegistry)
        {
            this.baseReg = baseRegistry;
            this.baseShell = baseRegistry + "\\shell";
        }
        public bool CheckRegistryPermission()
        {
            try
            {
                Registry.ClassesRoot.OpenSubKey(baseReg, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            catch (SecurityException)
            {
                return false;
            }
            return true;
        }
        private void CreateRegistry()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(baseShell);
            if (key == null)
            {
                Registry.ClassesRoot.CreateSubKey(baseShell);
                key = Registry.ClassesRoot.OpenSubKey(baseReg, RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.SetValue("subcommands", "", RegistryValueKind.String);
            }
        }
        public HashSet<string> LoadPrintersFromRegistry()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(baseShell);
            if (key != null)
            {
                return new HashSet<string>(key.GetSubKeyNames().OfType<string>().ToList());
            }
            return new HashSet<string>();
        }
        public void WritePrintersToRegistry(HashSet<string> printerList)
        {
            FullyRemoveRegistry();
            if (printerList != null && printerList.Count > 0)
            {
                CreateRegistry();
                RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(baseShell, RegistryKeyPermissionCheck.ReadWriteSubTree);
                foreach (string name in printerList)
                {
                    Debug.WriteLine(name);

                    RegistryKey subkey = shellKey.CreateSubKey(name, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    subkey.SetValue("", name);
                    RegistryKey subkeyCommand = subkey.CreateSubKey("command", RegistryKeyPermissionCheck.ReadWriteSubTree);

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), ConstFields.SSH_PRINT_FILE_NAME);
                    string command = String.Format(commandTemplate, filePath, name);
                    subkeyCommand.SetValue("", command);
                }
            }
        }
        public void FullyRemoveRegistry()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(baseReg, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (key != null)
            {
                key.DeleteSubKeyTree("");
            }
        }
    }
}
