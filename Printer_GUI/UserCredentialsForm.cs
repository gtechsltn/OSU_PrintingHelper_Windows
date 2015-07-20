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

namespace Printer_GUI
{
    public partial class UserCredentialsForm : MetroForm
    {
        private const string SUCCESSFUL_SAVING_MESSAGE = "Your username and password is stored securely!";
        private const string UNSUCCESSFUL_SAVING_MESSAGE = "Fail to store your username and password.";
        private ConfigManager manager;
        public UserCredentialsForm(ConfigManager manager)
        {
            this.manager = manager;
            InitializeComponent();
            textBox_Password.PasswordChar = '*';
            comboBox_Department.SelectedIndex = 0;
        }

        private void button_StorePassword_Click(object sender, EventArgs e)
        {
            string department = comboBox_Department.Text;
            string username = textBox_Username.Text;
            string password = textBox_Password.Text;

            if (manager.SaveCredentials(department, username, password))
            {
                MetroMessageBox.Show(this, SUCCESSFUL_SAVING_MESSAGE);
            }
            else
            {
                MetroMessageBox.Show(this, UNSUCCESSFUL_SAVING_MESSAGE);
            }
        }

        private void comboBox_Department_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_StorePassword.Enabled = true;
            string department = comboBox_Department.Text;
            textBox_Username.Text = manager.GetUserName(department);
            textBox_Password.Text = manager.GetPassword(department);
        }
    }
}
