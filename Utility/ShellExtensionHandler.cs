﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utility
{
    public static class ShellExtensionHandler
    {
        private static string GetRegAsmPath()
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                return ConstFields.REGASM_PATH_64BIT;
            }
            return ConstFields.REGASM_PATH_32BIT;
        }
        public static bool CheckInstallationStatus()
        {
            try
            {
                Type comType = Type.GetTypeFromCLSID(new Guid(ConstFields.SHELL_EXT_GUID));
                Activator.CreateInstance(comType);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void Install()
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = GetRegAsmPath();
            p.StartInfo.Arguments = ConstFields.PRINTER_SHELL_EXT_FILE_NAME + " /codebase";
            p.Start();
        }
        public static void Uninstall()
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = GetRegAsmPath();
            p.StartInfo.Arguments = "/u " + ConstFields.PRINTER_SHELL_EXT_FILE_NAME + " /codebase";
            p.Start();

            RestartExplorer();
        }
        public static void RestartExplorer()
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            p.StartInfo.FileName = "taskkill";
            p.StartInfo.Arguments = " /IM explorer.exe /F";
            p.Start();
            p.WaitForExit();

            Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
        }
    }
}
