using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using MetroFramework.Forms;
using MetroFramework;
using System.Threading.Tasks;
using Printer_GUI.Properties;

using Utility;

namespace Printer_GUI
{
    public partial class UserCredentialsForm : MetroForm
    {
        private ConfigManager manager;
        private SSH_Print.NetworkHandler handler;

        private string department;
        private string username;
        private string password;

        public UserCredentialsForm(ConfigManager manager)
        {
            this.manager = manager;
            InitializeComponent();
            textBox_Password.PasswordChar = '*';
            comboBox_Department.SelectedIndex = 0;
        }

        private async void button_StorePassword_Click(object sender, EventArgs e)
        {
            if (handler != null)
            {
                return;
            }

            department = comboBox_Department.Text;
            username = textBox_Username.Text;
            password = textBox_Password.Text;
            IDictionary<string, string[]> info = manager.GetServerInfo();
            string address = info[department][0];
            handler = new SSH_Print.NetworkHandler(address, username, password);
            if (!(await handler.CheckConnectionAsync())) 
            {
                MetroMessageBox.Show(this, Resources.CredentialIncorrectPrompt);
            }
            else
            {
                SaveCredentials();
            }
            handler = null;
        }

        private void comboBox_Department_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_StorePassword.Enabled = true;
            string department = comboBox_Department.Text;
            textBox_Username.Text = manager.GetUserName(department);
            textBox_Password.Text = manager.GetPassword(department);
        }

        private void SaveCredentials()
        {
            if (manager.SaveCredentials(department, username, password))
            {
                MetroMessageBox.Show(this, Resources.CredentialSuccessfulSavdPrompt);
            }
            else
            {
                MetroMessageBox.Show(this, Resources.CredentialFailedSavdPrompt);
            }
        }
    }
}
