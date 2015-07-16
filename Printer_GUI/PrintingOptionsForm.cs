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

namespace Printer_GUI
{
    public partial class PrintingOptionsForm : MetroForm
    {
        ConfigLoader Loader;
        const string FailingSaved = "A Problem was encoutered while saving.\n"
            + "Please make sure the configuration file is readable and writable";
        const string SuccessfulSaved = "Options saved successufully!";
        public PrintingOptionsForm()
        {
            InitializeComponent();

            Loader = new ConfigLoader(ConstFields.CONFIGRATION_FILE_NAME);
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
            string Message = SuccessfulSaved;
            if (!Loader.SetEnabledPrintingOptions(CheckedOptions))
            {
                Message = FailingSaved;
            }
            MetroMessageBox.Show(this, Message);
        }
    }

}
