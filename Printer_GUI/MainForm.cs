using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

using MetroFramework.Forms;
using MetroFramework;

using Utility;

namespace Printer_GUI
{
    public enum ColumnFields { Department = 0, Location, Name, Type };
    public partial class MainForm : MetroForm
    {
        private ConfigManager loader;
        private IList<IDictionary<string, string>> printerInfo;
        private event EventHandler DownloadCompletedEventHandler;

        public MainForm()
        {
            PrivilegeCheck();

            DownloadCompletedEventHandler += OnPrinterInformationDownloaded;
            JsonDownloader<IList<IDictionary<string, string>>> Downloader
                = new JsonDownloader<IList<IDictionary<string, string>>>(DownloadCompletedEventHandler);

            InitializeComponent();
        }
        private void PrivilegeCheck()
        {
            if (!PrivilegeChecker.IsAdministrator())
            {
                MessageBox.Show(this, "Please run this app with administrator permission.");
                this.Close();
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            loader = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);
            if (printerInfo != null)
            {
                UpdateGridView(printerInfo);
            }
        }
        private void OnPrinterInformationDownloaded(object sender, EventArgs e)
        {
            printerInfo = (IList<IDictionary<string, string>>)sender;
            if (this.dataGridView != null)
            {
                UpdateGridView(printerInfo);
            }
        }
        private void button_ApplyChange_Click(object sender, EventArgs e)
        {
            IList<IDictionary<string, string>> confirmedPrinter =
                new List<IDictionary<string, string>>();
            for (int i = 0; i < dataGridView.RowCount; ++i)
            {
                DataGridViewRow row = dataGridView.Rows[i];
                if (!row.Cells[3].Value.Equals(new Boolean()))
                {
                    confirmedPrinter.Add(printerInfo[i]);
                }
            }
            loader.SaveAllLoadedPrinters(confirmedPrinter);
            MetroMessageBox.Show(this, "Apply successful!\nRight on any file to print.");
        }

        private void button_Options_Click(object sender, EventArgs e)
        {
            PrintingOptionsForm Form = new PrintingOptionsForm();
            Form.ShowDialog();
        }
        private void button_Credentials_Click(object sender, EventArgs e)
        {
            ConfigManager manager = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);

            if (manager.LoadUserCredentials() == null)
            {
                MetroMessageBox.Show(this, "Cannot find " + ConstFields.CONFIGRATION_FILE_NAME
                    + " in the current directory.\n" +
                    "Or the configuration file is corrupted.");
                return;
            }

            UserCredentialsForm form = new UserCredentialsForm(manager);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
        private void button_About_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
        private void button_Uninstall_Click(object sender, EventArgs e)
        {
            ShellExtensionHandler.Uninstall();
            MetroMessageBox.Show(this, "Application Uninstalled");
            this.Close();
        }
        private void button_Install_Click(object sender, EventArgs e)
        {
            ShellExtensionHandler.Install();
            MetroMessageBox.Show(this, "Application Installed");
        }

        /*
         *  Public Controller Interfaces
         */
        public void UpdateGridView(IList<IDictionary<string, string>> printerInfo)
        {
            this.button_ApplyChange.Enabled = true;
            dataGridView.Rows.Clear();

            IList<IDictionary<string, string>> loadedPrinters = loader.GetAllLoadedPrinters();
            ISet<string> printerNameSet = new HashSet<string>();
            foreach (IDictionary<string, string> p in loadedPrinters)
            {
                if (p.ContainsKey("Name"))
                {
                    printerNameSet.Add(p["Name"]);
                }
            }

            foreach (var info in printerInfo)
            {
                dataGridView.Rows.Add();
                int rowIndex = dataGridView.RowCount - 1;
                DataGridViewRow r = dataGridView.Rows[rowIndex];

                foreach (ColumnFields field in Enum.GetValues(typeof(ColumnFields)))
                {
                    if (info.ContainsKey(field.ToString()))
                    {
                        r.Cells[(int)field].Value = info[field.ToString()];
                    }
                }
                r.Cells[r.Cells.Count - 1].Value = printerNameSet.Contains(info["Name"]);
            }
        }

    }
}
