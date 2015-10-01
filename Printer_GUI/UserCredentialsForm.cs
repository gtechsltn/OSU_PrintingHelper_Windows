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
        private ConfigManager _manager;
        private SSH_Print.NetworkHandler _handler;

        private string _department;
        private string _username;
        private string _password;

        public UserCredentialsForm(ConfigManager manager)
        {
            this._manager = manager;
            InitializeComponent();
            textBox_Password.PasswordChar = '*';
            comboBox_Department.SelectedIndex = 0;
        }

        private async void button_StorePassword_Click(object sender, EventArgs e)
        {
            if (_handler != null)
            {
                return;
            }

            _department = comboBox_Department.Text;
            _username = textBox_Username.Text;
            _password = textBox_Password.Text;
            IDictionary<string, string[]> info = _manager.GetServerInfo();
            string address = info[_department][0];
            _handler = new SSH_Print.NetworkHandler(address, _username, _password);
            if (!(await _handler.CheckConnectionAsync())) 
            {
                MetroMessageBox.Show(this, Resources.CredentialIncorrectPrompt);
            }
            else
            {
                SaveCredentials();
            }
            _handler = null;
        }

        private void comboBox_Department_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_StorePassword.Enabled = true;
            string department = comboBox_Department.Text;
            textBox_Username.Text = _manager.GetUserName(department);
            textBox_Password.Text = _manager.GetPassword(department);
        }

        private void SaveCredentials()
        {
            if (_manager.SaveCredentials(_department, _username, _password))
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
