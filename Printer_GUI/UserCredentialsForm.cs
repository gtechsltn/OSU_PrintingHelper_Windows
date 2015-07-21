using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using Utility;
using MetroFramework.Forms;
using MetroFramework;
using System.Threading.Tasks;

namespace Printer_GUI
{
    public partial class UserCredentialsForm : MetroForm
    {
        private const string SUCCESSFUL_SAVING_MESSAGE = "Your username and password is stored securely!";
        private const string UNSUCCESSFUL_SAVING_MESSAGE = "Fail to store your username and password.";
        private const string INCORRECT_CREDENTIAL_MESSAGE = 
            "Your username does not match your password\r\n\r\n" 
            + "Please re-check your username and password or the Internet connection";

        private ConfigManager manager;

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
            department = comboBox_Department.Text;
            username = textBox_Username.Text;
            password = textBox_Password.Text;
            IDictionary<string, string[]> info = manager.GetServerInfo();
            string address = info[department][0];
            SSH_Print.NetworkHandler handler = new SSH_Print.NetworkHandler(address, username, password);
            if (!(await handler.CheckConnectionAsync())) 
            {
                MetroMessageBox.Show(this, INCORRECT_CREDENTIAL_MESSAGE);
                return;
            }
            SaveCredentials();
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
                MetroMessageBox.Show(this, SUCCESSFUL_SAVING_MESSAGE);
            }
            else
            {
                MetroMessageBox.Show(this, UNSUCCESSFUL_SAVING_MESSAGE);
            }
        }
    }
}
