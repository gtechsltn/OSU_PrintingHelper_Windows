namespace Printer_GUI
{
    partial class UserCredentialsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_Username = new System.Windows.Forms.TextBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.label_Username = new System.Windows.Forms.Label();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_Department = new System.Windows.Forms.Label();
            this.button_StorePassword = new System.Windows.Forms.Button();
            this.comboBox_Department = new MetroFramework.Controls.MetroComboBox();
            this.SuspendLayout();
            // 
            // textBox_Username
            // 
            this.textBox_Username.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Username.Location = new System.Drawing.Point(119, 85);
            this.textBox_Username.Name = "textBox_Username";
            this.textBox_Username.Size = new System.Drawing.Size(143, 25);
            this.textBox_Username.TabIndex = 0;
            // 
            // textBox_Password
            // 
            this.textBox_Password.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Password.Location = new System.Drawing.Point(119, 129);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(143, 25);
            this.textBox_Password.TabIndex = 1;
            // 
            // label_Username
            // 
            this.label_Username.AutoSize = true;
            this.label_Username.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Username.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label_Username.Location = new System.Drawing.Point(12, 88);
            this.label_Username.Name = "label_Username";
            this.label_Username.Size = new System.Drawing.Size(67, 17);
            this.label_Username.TabIndex = 3;
            this.label_Username.Text = "Username";
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Password.Location = new System.Drawing.Point(12, 132);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(64, 17);
            this.label_Password.TabIndex = 4;
            this.label_Password.Text = "Password";
            // 
            // label_Department
            // 
            this.label_Department.AutoSize = true;
            this.label_Department.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Department.Location = new System.Drawing.Point(12, 174);
            this.label_Department.Name = "label_Department";
            this.label_Department.Size = new System.Drawing.Size(77, 17);
            this.label_Department.TabIndex = 5;
            this.label_Department.Text = "Department";
            // 
            // button_StorePassword
            // 
            this.button_StorePassword.Enabled = false;
            this.button_StorePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_StorePassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_StorePassword.Location = new System.Drawing.Point(197, 223);
            this.button_StorePassword.Name = "button_StorePassword";
            this.button_StorePassword.Size = new System.Drawing.Size(75, 27);
            this.button_StorePassword.TabIndex = 6;
            this.button_StorePassword.Text = "Store";
            this.button_StorePassword.UseVisualStyleBackColor = true;
            this.button_StorePassword.Click += new System.EventHandler(this.button_StorePassword_Click);
            // 
            // comboBox_Department
            // 
            this.comboBox_Department.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Department.FormattingEnabled = true;
            this.comboBox_Department.ItemHeight = 23;
            this.comboBox_Department.Items.AddRange(new object[] {
            "CSE",
            "ECE"});
            this.comboBox_Department.Location = new System.Drawing.Point(118, 170);
            this.comboBox_Department.Name = "comboBox_Department";
            this.comboBox_Department.Size = new System.Drawing.Size(143, 29);
            this.comboBox_Department.TabIndex = 2;
            this.comboBox_Department.UseSelectable = true;
            this.comboBox_Department.SelectedIndexChanged += new System.EventHandler(this.comboBox_Department_SelectedIndexChanged);
            // 
            // UserCredentialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_StorePassword);
            this.Controls.Add(this.label_Department);
            this.Controls.Add(this.label_Password);
            this.Controls.Add(this.label_Username);
            this.Controls.Add(this.comboBox_Department);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.textBox_Username);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserCredentialsForm";
            this.Text = "User Credentials";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Username;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Label label_Username;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Department;
        private System.Windows.Forms.Button button_StorePassword;
        private MetroFramework.Controls.MetroComboBox comboBox_Department;
    }
}