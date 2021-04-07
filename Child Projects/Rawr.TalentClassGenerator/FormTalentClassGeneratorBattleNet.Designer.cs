namespace Rawr.TalentClassGenerator
{
	partial class FormTalentClassGeneratorBattleNet
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
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.buttonGenerateCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxCode
            // 
            this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCode.Location = new System.Drawing.Point(12, 80);
            this.textBoxCode.Multiline = true;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCode.Size = new System.Drawing.Size(498, 383);
            this.textBoxCode.TabIndex = 3;
            // 
            // buttonGenerateCode
            // 
            this.buttonGenerateCode.Location = new System.Drawing.Point(12, 51);
            this.buttonGenerateCode.Name = "buttonGenerateCode";
            this.buttonGenerateCode.Size = new System.Drawing.Size(87, 23);
            this.buttonGenerateCode.TabIndex = 1;
            this.buttonGenerateCode.Text = "Generate Code";
            this.buttonGenerateCode.UseVisualStyleBackColor = true;
            this.buttonGenerateCode.Click += new System.EventHandler(this.buttonGenerateCode_Click);
            // 
            // FormTalentClassGeneratorBattleNet
            // 
            this.AcceptButton = this.buttonGenerateCode;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 475);
            this.Controls.Add(this.buttonGenerateCode);
            this.Controls.Add(this.textBoxCode);
            this.Name = "FormTalentClassGeneratorBattleNet";
            this.Text = "Talent Class Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.TextBox textBoxCode;
		private System.Windows.Forms.Button buttonGenerateCode;


	}
}

