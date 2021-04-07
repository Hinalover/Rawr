namespace Load_SimC_DBC
{
    partial class Form1
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
            this.btn_DBCLive = new System.Windows.Forms.Button();
            this.btn_DBCPTR = new System.Windows.Forms.Button();
            this.btn_Copy = new System.Windows.Forms.Button();
            this.txt_DBC = new System.Windows.Forms.TextBox();
            this.txt_Converted = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_DBCLive
            // 
            this.btn_DBCLive.Location = new System.Drawing.Point(133, 14);
            this.btn_DBCLive.Name = "btn_DBCLive";
            this.btn_DBCLive.Size = new System.Drawing.Size(141, 49);
            this.btn_DBCLive.TabIndex = 0;
            this.btn_DBCLive.Text = "Live";
            this.btn_DBCLive.UseVisualStyleBackColor = true;
            this.btn_DBCLive.Click += new System.EventHandler(this.btn_DBCLive_Click);
            // 
            // btn_DBCPTR
            // 
            this.btn_DBCPTR.Location = new System.Drawing.Point(345, 15);
            this.btn_DBCPTR.Name = "btn_DBCPTR";
            this.btn_DBCPTR.Size = new System.Drawing.Size(141, 47);
            this.btn_DBCPTR.TabIndex = 1;
            this.btn_DBCPTR.Text = "PTR";
            this.btn_DBCPTR.UseVisualStyleBackColor = true;
            this.btn_DBCPTR.Click += new System.EventHandler(this.btn_DBCPTR_Click);
            // 
            // btn_Copy
            // 
            this.btn_Copy.Location = new System.Drawing.Point(864, 15);
            this.btn_Copy.Name = "btn_Copy";
            this.btn_Copy.Size = new System.Drawing.Size(140, 47);
            this.btn_Copy.TabIndex = 3;
            this.btn_Copy.Text = "Copy";
            this.btn_Copy.UseVisualStyleBackColor = true;
            this.btn_Copy.Click += new System.EventHandler(this.btn_Copy_Click);
            // 
            // txt_DBC
            // 
            this.txt_DBC.Location = new System.Drawing.Point(12, 69);
            this.txt_DBC.Multiline = true;
            this.txt_DBC.Name = "txt_DBC";
            this.txt_DBC.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_DBC.Size = new System.Drawing.Size(593, 387);
            this.txt_DBC.TabIndex = 4;
            this.txt_DBC.WordWrap = false;
            // 
            // txt_Converted
            // 
            this.txt_Converted.Location = new System.Drawing.Point(637, 70);
            this.txt_Converted.Multiline = true;
            this.txt_Converted.Name = "txt_Converted";
            this.txt_Converted.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Converted.Size = new System.Drawing.Size(593, 387);
            this.txt_Converted.TabIndex = 5;
            this.txt_Converted.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 469);
            this.Controls.Add(this.txt_Converted);
            this.Controls.Add(this.txt_DBC);
            this.Controls.Add(this.btn_Copy);
            this.Controls.Add(this.btn_DBCPTR);
            this.Controls.Add(this.btn_DBCLive);
            this.Name = "Form1";
            this.Text = "SimulationCraft DBC Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_DBCLive;
        private System.Windows.Forms.Button btn_DBCPTR;
        private System.Windows.Forms.Button btn_Copy;
        private System.Windows.Forms.TextBox txt_DBC;
        private System.Windows.Forms.TextBox txt_Converted;
    }
}

