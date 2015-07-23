namespace Printer_GUI
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_ApplyChange = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.dataGridView = new MetroFramework.Controls.MetroGrid();
            this.button_Options = new System.Windows.Forms.Button();
            this.button_About = new System.Windows.Forms.Button();
            this.button_Uninstall = new System.Windows.Forms.Button();
            this.button_Credentials = new System.Windows.Forms.Button();
            this.button_Install = new System.Windows.Forms.Button();
            this.Department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrinterLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrinterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrinterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Loaded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ApplyChange
            // 
            this.button_ApplyChange.Enabled = false;
            this.button_ApplyChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ApplyChange.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ApplyChange.Location = new System.Drawing.Point(556, 367);
            this.button_ApplyChange.Name = "button_ApplyChange";
            this.button_ApplyChange.Size = new System.Drawing.Size(84, 30);
            this.button_ApplyChange.TabIndex = 0;
            this.button_ApplyChange.Text = "Apply";
            this.button_ApplyChange.UseVisualStyleBackColor = true;
            this.button_ApplyChange.Click += new System.EventHandler(this.button_ApplyChange_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Department,
            this.PrinterLocation,
            this.PrinterName,
            this.PrinterType,
            this.Loaded});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView.Location = new System.Drawing.Point(13, 85);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView.Size = new System.Drawing.Size(637, 276);
            this.dataGridView.TabIndex = 2;
            // 
            // button_Options
            // 
            this.button_Options.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Options.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Options.Location = new System.Drawing.Point(240, 27);
            this.button_Options.Name = "button_Options";
            this.button_Options.Size = new System.Drawing.Size(77, 30);
            this.button_Options.TabIndex = 3;
            this.button_Options.Text = "Options";
            this.button_Options.UseVisualStyleBackColor = true;
            this.button_Options.Click += new System.EventHandler(this.button_Options_Click);
            // 
            // button_About
            // 
            this.button_About.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_About.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_About.Location = new System.Drawing.Point(464, 27);
            this.button_About.Name = "button_About";
            this.button_About.Size = new System.Drawing.Size(77, 30);
            this.button_About.TabIndex = 4;
            this.button_About.Text = "About";
            this.button_About.UseVisualStyleBackColor = true;
            this.button_About.Click += new System.EventHandler(this.button_About_Click);
            // 
            // button_Uninstall
            // 
            this.button_Uninstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Uninstall.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Uninstall.Location = new System.Drawing.Point(573, 27);
            this.button_Uninstall.Name = "button_Uninstall";
            this.button_Uninstall.Size = new System.Drawing.Size(77, 30);
            this.button_Uninstall.TabIndex = 5;
            this.button_Uninstall.Text = "Uninstall";
            this.button_Uninstall.UseVisualStyleBackColor = true;
            this.button_Uninstall.Click += new System.EventHandler(this.button_Uninstall_Click);
            // 
            // button_Credentials
            // 
            this.button_Credentials.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Credentials.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Credentials.Location = new System.Drawing.Point(350, 27);
            this.button_Credentials.Name = "button_Credentials";
            this.button_Credentials.Size = new System.Drawing.Size(77, 30);
            this.button_Credentials.TabIndex = 6;
            this.button_Credentials.Text = "Credentials";
            this.button_Credentials.UseVisualStyleBackColor = true;
            this.button_Credentials.Click += new System.EventHandler(this.button_Credentials_Click);
            // 
            // button_Install
            // 
            this.button_Install.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Install.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Install.Location = new System.Drawing.Point(13, 367);
            this.button_Install.Name = "button_Install";
            this.button_Install.Size = new System.Drawing.Size(75, 30);
            this.button_Install.TabIndex = 7;
            this.button_Install.Text = "Install";
            this.button_Install.UseVisualStyleBackColor = true;
            this.button_Install.Click += new System.EventHandler(this.button_Install_Click);
            // 
            // Department
            // 
            this.Department.FillWeight = 74.23856F;
            this.Department.HeaderText = "Dept";
            this.Department.Name = "Department";
            this.Department.ReadOnly = true;
            this.Department.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // PrinterLocation
            // 
            this.PrinterLocation.FillWeight = 203.0456F;
            this.PrinterLocation.HeaderText = "Location";
            this.PrinterLocation.Name = "PrinterLocation";
            this.PrinterLocation.ReadOnly = true;
            // 
            // PrinterName
            // 
            this.PrinterName.FillWeight = 74.23856F;
            this.PrinterName.HeaderText = "Name";
            this.PrinterName.Name = "PrinterName";
            this.PrinterName.ReadOnly = true;
            // 
            // PrinterType
            // 
            this.PrinterType.FillWeight = 74.23856F;
            this.PrinterType.HeaderText = "Type";
            this.PrinterType.Name = "PrinterType";
            this.PrinterType.ReadOnly = true;
            // 
            // Loaded
            // 
            this.Loaded.FillWeight = 74.23856F;
            this.Loaded.HeaderText = "Loaded";
            this.Loaded.Name = "Loaded";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 412);
            this.Controls.Add(this.button_Install);
            this.Controls.Add(this.button_Credentials);
            this.Controls.Add(this.button_Uninstall);
            this.Controls.Add(this.button_About);
            this.Controls.Add(this.button_Options);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.button_ApplyChange);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Printer GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroGrid dataGridView;

        private System.Windows.Forms.Button button_ApplyChange;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button button_Options;
        private System.Windows.Forms.Button button_About;
        private System.Windows.Forms.Button button_Uninstall;
        private System.Windows.Forms.Button button_Credentials;
        private System.Windows.Forms.Button button_Install;
        private System.Windows.Forms.DataGridViewTextBoxColumn Department;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Loaded;
    }
}

