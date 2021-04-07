namespace Rawr.SimCItemConverter
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
            this.btnLive = new System.Windows.Forms.Button();
            this.btnBeta = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLive
            // 
            this.btnLive.Location = new System.Drawing.Point(12, 12);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(141, 53);
            this.btnLive.TabIndex = 0;
            this.btnLive.Text = "Live";
            this.btnLive.UseVisualStyleBackColor = true;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // btnBeta
            // 
            this.btnBeta.Location = new System.Drawing.Point(159, 12);
            this.btnBeta.Name = "btnBeta";
            this.btnBeta.Size = new System.Drawing.Size(141, 53);
            this.btnBeta.TabIndex = 1;
            this.btnBeta.Text = "Beta";
            this.btnBeta.UseVisualStyleBackColor = true;
            this.btnBeta.Click += new System.EventHandler(this.btnBeta_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 263);
            this.Controls.Add(this.btnBeta);
            this.Controls.Add(this.btnLive);
            this.Name = "Form1";
            this.Text = "Simulationcraft Item Converter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLive;
        private System.Windows.Forms.Button btnBeta;
    }
}

