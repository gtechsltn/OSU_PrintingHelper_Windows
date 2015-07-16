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
using Newtonsoft.Json;

namespace Printer_GUI
{
    public partial class MainForm : MetroForm
    {
        RegistryHandler registryHandler;

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!PrivilegeChecker.IsAdministrator())
            {
                MetroMessageBox.Show(this, "Please run this app with administrator permission.");
                this.Close();
            }
            DownloadInformation();
            registryHandler = new RegistryHandler(ConstFields.REGISTRY_ROOT);
        }
        public void DownloadInformation()
        {
            WebClient downloader = new WebClient();
            downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(onDownloadStringCompleted);
            downloader.DownloadStringAsync(new Uri(ConstFields.PRINTER_LIST_URL));
        }
        public void onDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            List<Dictionary<string, string>> printerInfo =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(e.Result);
            UpdateGridView(printerInfo);
        }

        private void button_ApplyChange_Click(object sender, EventArgs e)
        {
            HashSet<string> confirmedPrinter = new HashSet<string>();
            for (int i = 0; i < dataGridView.RowCount; ++i)
            {
                DataGridViewRow row = dataGridView.Rows[i];
                if (!row.Cells[3].Value.Equals(new Boolean()))
                {
                    confirmedPrinter.Add((string)row.Cells[1].Value);
                }
            }
            registryHandler.WritePrintersToRegistry(confirmedPrinter);
            MetroMessageBox.Show(this, "Apply successful!\nRight on any file to print.");
        }

        private void button_Options_Click(object sender, EventArgs e)
        {
            PrintingOptionsForm Form = new PrintingOptionsForm();
            Form.ShowDialog();
        }
        private void button_Credentials_Click(object sender, EventArgs e)
        {
            CredentialsManager manager = new CredentialsManager(ConstFields.CONFIGRATION_FILE_NAME);

            if (!manager.LoadUserCredentials())
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
            registryHandler.FullyRemoveRegistry();
            MetroMessageBox.Show(this, "Uninstall successfull!");
            this.Close();
        }

        /*
         *  Public Controller Interfaces
         */
        public void UpdateGridView(List<Dictionary<string, string>> printerInfo)
        {
            HashSet<string> loadedPrinters;

            this.button_ApplyChange.Enabled = true;
            loadedPrinters = registryHandler.LoadPrintersFromRegistry();

            dataGridView.Rows.Clear();

            foreach (var info in printerInfo)
            {
                dataGridView.Rows.Add();
                int rowIndex = dataGridView.RowCount - 1;
                DataGridViewRow r = dataGridView.Rows[rowIndex];

                r.Cells[0].Value = info["Location"];
                r.Cells[1].Value = info["Name"];
                r.Cells[2].Value = info["Type"];
                r.Cells[3].Value = loadedPrinters.Contains(info["Name"]);
            }
        }

    }
}
