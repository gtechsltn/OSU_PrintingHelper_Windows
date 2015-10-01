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
using Printer_GUI.Properties;

namespace Printer_GUI
{
    public enum ColumnFields { Department = 0, Location, Name, Type };
    public partial class MainForm : MetroForm
    {
        private ConfigManager loader;
        private IList<Dictionary<string, string>> _printerInfo;
        private event EventHandler DownloadCompletedEventHandler;

        public MainForm()
        {
            if (!ShellExtensionHandler.CheckInstallationStatus())
            {
                ShellExtensionHandler.Install();
            }

            DownloadCompletedEventHandler += OnPrinterInformationDownloaded;
            new JsonDownloader<IList<Dictionary<string, string>>>(
                new Uri(ConstFields.PRINTER_LIST_URL), DownloadCompletedEventHandler);

            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            loader = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);
            if (_printerInfo != null)
            {
                UpdateGridView(_printerInfo);
            }
        }
        private void OnPrinterInformationDownloaded(object sender, EventArgs e)
        {
            _printerInfo = (IList<Dictionary<string, string>>)sender;
            if (this.dataGridView != null)
            {
                UpdateGridView(_printerInfo);
            }
        }
        private void button_ApplyChange_Click(object sender, EventArgs e)
        {
            IList<Dictionary<string, string>> confirmedPrinter =
                new List<Dictionary<string, string>>();
            for (int i = 0; i < dataGridView.RowCount; ++i)
            {
                DataGridViewRow row = dataGridView.Rows[i];
                if (!row.Cells[row.Cells.Count - 1].Value.Equals(new Boolean()))
                {
                    confirmedPrinter.Add(_printerInfo[i]);
                }
            }
            loader.SaveAllLoadedPrinters(confirmedPrinter);
            MetroMessageBox.Show(this, Resources.SavePrinterSuccessfulPrompt);
        }

        private void button_Options_Click(object sender, EventArgs e)
        {
            PrintingOptionsForm Form = new PrintingOptionsForm();
            Form.ShowDialog();
        }
        private void button_Credentials_Click(object sender, EventArgs e)
        {
            ConfigManager manager = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);

            if (manager.GetUserCredentials() == null)
            {
                MetroMessageBox.Show(this, Resources.ConfigurationFileNotFoundPrompt);
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
            MetroMessageBox.Show(this, Resources.ApplicationUninstalledPrompt);
            this.Close();
        }
        public void UpdateGridView(IList<Dictionary<string, string>> printerInfo)
        {
            this.button_ApplyChange.Enabled = true;
            dataGridView.Rows.Clear();

            IList<Dictionary<string, string>> loadedPrinters = loader.GetAllLoadedPrinters();
            ISet<string> printerNameSet = new HashSet<string>();
            foreach (IDictionary<string, string> p in loadedPrinters)
            {
                if (p.ContainsKey(ConstFields.PRINTER_IDENTIFIER_FILED))
                {
                    printerNameSet.Add(p[ConstFields.PRINTER_IDENTIFIER_FILED]);
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
                r.Cells[r.Cells.Count - 1].Value =
                    printerNameSet.Contains(info[ConstFields.PRINTER_IDENTIFIER_FILED]);
            }
        }

    }
}
