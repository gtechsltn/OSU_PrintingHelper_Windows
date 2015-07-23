using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;
using System.Resources;
using System.Reflection;
using MetroFramework.Forms;
using MetroFramework;
using Printer_GUI.Properties;

namespace Printer_GUI
{
    public partial class PrintingOptionsForm : MetroForm
    {
        ConfigManager Loader;
        public PrintingOptionsForm()
        {
            InitializeComponent();

            Loader = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);
            foreach (Tuple<string, bool> command in Loader.GetAllPrintingOptions())
            {
                this.checkedListBox_PrintingOptions.Items.Add(command.Item1);
                this.checkedListBox_PrintingOptions.SetItemChecked(
                    this.checkedListBox_PrintingOptions.Items.Count - 1, command.Item2);
            }
        }

        private void button_SavePrintingOptions_Click(object sender, EventArgs e)
        {
            ISet<string> CheckedOptions = new HashSet<string>();
            foreach (var Item in this.checkedListBox_PrintingOptions.CheckedItems)
            {
                CheckedOptions.Add(Item.ToString());
            }
            string Message = Resources.OptionSuccessfulSavedPrompt;
            if (!Loader.SaveEnabledPrintingOptions(CheckedOptions))
            {
                Message = Resources.OptionFailedSavedPrompt;
            }
            MetroMessageBox.Show(this, Message);
        }
    }

}
