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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button_ApplyChange = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadPrinterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usernamePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.PrinterLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrinterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrinterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Loaded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ApplyChange
            // 
            this.button_ApplyChange.Enabled = false;
            this.button_ApplyChange.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ApplyChange.Location = new System.Drawing.Point(562, 355);
            this.button_ApplyChange.Name = "button_ApplyChange";
            this.button_ApplyChange.Size = new System.Drawing.Size(75, 23);
            this.button_ApplyChange.TabIndex = 0;
            this.button_ApplyChange.Text = "Apply";
            this.button_ApplyChange.UseVisualStyleBackColor = true;
            this.button_ApplyChange.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPrinterToolStripMenuItem,
            this.uninstallToolStripMenuItem,
            this.usernamePasswordToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(649, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadPrinterToolStripMenuItem
            // 
            this.loadPrinterToolStripMenuItem.Name = "loadPrinterToolStripMenuItem";
            this.loadPrinterToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.loadPrinterToolStripMenuItem.Text = "Load Printer";
            this.loadPrinterToolStripMenuItem.Click += new System.EventHandler(this.loadPrinterToolStripMenuItem_Click);
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.uninstallToolStripMenuItem.Text = "Uninstall";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // usernamePasswordToolStripMenuItem
            // 
            this.usernamePasswordToolStripMenuItem.Name = "usernamePasswordToolStripMenuItem";
            this.usernamePasswordToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.usernamePasswordToolStripMenuItem.Text = "Account Info";
            this.usernamePasswordToolStripMenuItem.Click += new System.EventHandler(this.usernamePasswordToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PrinterLocation,
            this.PrinterName,
            this.PrinterType,
            this.Loaded});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView.Location = new System.Drawing.Point(12, 27);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView.Size = new System.Drawing.Size(625, 322);
            this.dataGridView.TabIndex = 2;
            // 
            // PrinterLocation
            // 
            this.PrinterLocation.HeaderText = "Location";
            this.PrinterLocation.Name = "PrinterLocation";
            this.PrinterLocation.ReadOnly = true;
            // 
            // PrinterName
            // 
            this.PrinterName.HeaderText = "Name";
            this.PrinterName.Name = "PrinterName";
            this.PrinterName.ReadOnly = true;
            // 
            // PrinterType
            // 
            this.PrinterType.HeaderText = "Type";
            this.PrinterType.Name = "PrinterType";
            this.PrinterType.ReadOnly = true;
            // 
            // Loaded
            // 
            this.Loaded.HeaderText = "Loaded";
            this.Loaded.Name = "Loaded";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 390);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.button_ApplyChange);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Printer GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ApplyChange;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadPrinterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrinterType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Loaded;
        private System.Windows.Forms.ToolStripMenuItem usernamePasswordToolStripMenuItem;
    }
}

