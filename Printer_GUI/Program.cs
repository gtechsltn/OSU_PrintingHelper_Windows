using Printer_GUI.Properties;
using System;
using System.Windows.Forms;
using Utility;

namespace Printer_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!PrivilegeChecker.IsAdministrator())
            {
                MessageBox.Show(Resources.NonAdminPrompt);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
