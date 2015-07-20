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
        ConfigManager loader;
        IList<IDictionary<string, string>> printerInfo;
        public MainForm()
        {
            if (!PrivilegeChecker.IsAdministrator())
            {
                MessageBox.Show(this, "Please run this app with administrator permission.");
                this.Close();
            }
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            loader = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);
            DownloadInformation();
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
            printerInfo = JsonConvert.DeserializeObject<IList<IDictionary<string, string>>>(e.Result);
            UpdateGridView(printerInfo);
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
            MetroMessageBox.Show(this, "Uninstall successfull!");
            this.Close();
        }
        private void button_Install_Click(object sender, EventArgs e)
        {
            ShellExtensionHandler.Install();
            MetroMessageBox.Show(this, "Install successfull!");
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

                r.Cells[0].Value = info["Location"];
                r.Cells[1].Value = info["Name"];
                r.Cells[2].Value = info["Type"];
                r.Cells[3].Value = printerNameSet.Contains(info["Name"]);
            }
        }

    }
}
