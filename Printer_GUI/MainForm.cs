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
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Principal;
using DataUtility;

namespace Printer_GUI
{
    public partial class MainForm : Form
    {
        const string REGISTRY_ROOT = "*\\shell\\OSU Printer";
        const string URL_PRINTER_LIST = "http://web.cse.ohio-state.edu/~zhante/OSU_printers.json";
        private const string CONFIG_FILE_NAME = "server_config.xml";

        List<Dictionary<string, string>> printerInfo;
        HashSet<string> loadedPrinters;
        RegistryHandler registryHandler;
        bool downloadFinished;

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show("Please run this app with administrator permission.");
                this.Close();
            }
            downloadFinished = false;
            DownloadInformation();
            registryHandler = new RegistryHandler(REGISTRY_ROOT);
        }
        private static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        }       
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
        private void DownloadInformation()
        {
            WebClient downloader = new WebClient();
            downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(onDownloadStringCompleted);
            downloader.DownloadStringAsync(new Uri(URL_PRINTER_LIST));
        }
        private void onDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            downloadFinished = true;
            printerInfo = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(e.Result);
        }
        private void loadPrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!downloadFinished)
            {
                MessageBox.Show("Loading...");
                return;
            }

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

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewCheckBoxCell chk = new DataGridViewCheckBoxCell();
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
            MessageBox.Show("Apply successful!\nRight on any file to print.");
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registryHandler.FullyRemoveRegistry();
            MessageBox.Show("Uninstall successfull!");
            this.Close();
        }

        private void usernamePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CredentialsManager manager = new CredentialsManager(CONFIG_FILE_NAME);

            if (!manager.LoadUserCredentials())
            {
                MessageBox.Show("Cannot find " + CONFIG_FILE_NAME +
                    "\nMake sure it is under the current directory.");
                return;
            }

            UserCredentialsForm form = new UserCredentialsForm(manager);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
    }
}
