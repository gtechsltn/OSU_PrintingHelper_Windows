namespace Printer_GUI
{
    partial class PrintingOptionsForm
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
            this.checkedListBox_PrintingOptions = new System.Windows.Forms.CheckedListBox();
            this.button_SavePrintingOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBox_PrintingOptions
            // 
            this.checkedListBox_PrintingOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox_PrintingOptions.FormattingEnabled = true;
            this.checkedListBox_PrintingOptions.Location = new System.Drawing.Point(23, 63);
            this.checkedListBox_PrintingOptions.Name = "checkedListBox_PrintingOptions";
            this.checkedListBox_PrintingOptions.Size = new System.Drawing.Size(271, 204);
            this.checkedListBox_PrintingOptions.TabIndex = 0;
            // 
            // button_SavePrintingOptions
            // 
            this.button_SavePrintingOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_SavePrintingOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SavePrintingOptions.Location = new System.Drawing.Point(219, 283);
            this.button_SavePrintingOptions.Name = "button_SavePrintingOptions";
            this.button_SavePrintingOptions.Size = new System.Drawing.Size(75, 32);
            this.button_SavePrintingOptions.TabIndex = 1;
            this.button_SavePrintingOptions.Text = "Save";
            this.button_SavePrintingOptions.UseVisualStyleBackColor = true;
            this.button_SavePrintingOptions.Click += new System.EventHandler(this.button_SavePrintingOptions_Click);
            // 
            // PrintingOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 338);
            this.Controls.Add(this.button_SavePrintingOptions);
            this.Controls.Add(this.checkedListBox_PrintingOptions);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintingOptionsForm";
            this.Text = "Printing Options";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox_PrintingOptions;
        private System.Windows.Forms.Button button_SavePrintingOptions;

    }
}