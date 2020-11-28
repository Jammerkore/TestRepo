namespace MIDRetail.Windows
{
	partial class MIDExport
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkIncludeFormatting = new System.Windows.Forms.CheckBox();
			this.lblFileName = new System.Windows.Forms.Label();
			this.btnFileName = new System.Windows.Forms.Button();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.sfdFileName = new System.Windows.Forms.SaveFileDialog();
			this.gbxLocation = new System.Windows.Forms.GroupBox();
			this.radFile = new System.Windows.Forms.RadioButton();
			this.radExcel = new System.Windows.Forms.RadioButton();
			this.gbxInclude = new System.Windows.Forms.GroupBox();
			this.radAll = new System.Windows.Forms.RadioButton();
			this.radCurrent = new System.Windows.Forms.RadioButton();
			this.gbxLocation.SuspendLayout();
			this.gbxInclude.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(321, 247);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(402, 247);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkIncludeFormatting
			// 
			this.chkIncludeFormatting.AutoSize = true;
			this.chkIncludeFormatting.Location = new System.Drawing.Point(21, 19);
			this.chkIncludeFormatting.Name = "chkIncludeFormatting";
			this.chkIncludeFormatting.Size = new System.Drawing.Size(75, 17);
			this.chkIncludeFormatting.TabIndex = 6;
			this.chkIncludeFormatting.Text = "Formatting";
			this.chkIncludeFormatting.UseVisualStyleBackColor = true;
			// 
			// lblFileName
			// 
			this.lblFileName.Location = new System.Drawing.Point(31, 63);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(43, 18);
			this.lblFileName.TabIndex = 10;
			this.lblFileName.Text = "Name:";
			this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnFileName
			// 
			this.btnFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFileName.Location = new System.Drawing.Point(382, 61);
			this.btnFileName.Name = "btnFileName";
			this.btnFileName.Size = new System.Drawing.Size(72, 20);
			this.btnFileName.TabIndex = 9;
			this.btnFileName.Text = "Save As...";
			this.btnFileName.Click += new System.EventHandler(this.btnFileName_Click);
			// 
			// txtFileName
			// 
			this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFileName.Location = new System.Drawing.Point(74, 62);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(298, 20);
			this.txtFileName.TabIndex = 8;
			// 
			// gbxLocation
			// 
			this.gbxLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbxLocation.Controls.Add(this.radFile);
			this.gbxLocation.Controls.Add(this.lblFileName);
			this.gbxLocation.Controls.Add(this.radExcel);
			this.gbxLocation.Controls.Add(this.btnFileName);
			this.gbxLocation.Controls.Add(this.txtFileName);
			this.gbxLocation.Location = new System.Drawing.Point(13, 13);
			this.gbxLocation.Name = "gbxLocation";
			this.gbxLocation.Size = new System.Drawing.Size(464, 100);
			this.gbxLocation.TabIndex = 11;
			this.gbxLocation.TabStop = false;
			this.gbxLocation.Text = "Location";
			// 
			// radFile
			// 
			this.radFile.AutoSize = true;
			this.radFile.Location = new System.Drawing.Point(21, 44);
			this.radFile.Name = "radFile";
			this.radFile.Size = new System.Drawing.Size(41, 17);
			this.radFile.TabIndex = 1;
			this.radFile.TabStop = true;
			this.radFile.Text = "File";
			this.radFile.UseVisualStyleBackColor = true;
			this.radFile.CheckedChanged += new System.EventHandler(this.radFile_CheckedChanged);
			// 
			// radExcel
			// 
			this.radExcel.AutoSize = true;
			this.radExcel.Location = new System.Drawing.Point(21, 20);
			this.radExcel.Name = "radExcel";
			this.radExcel.Size = new System.Drawing.Size(51, 17);
			this.radExcel.TabIndex = 0;
			this.radExcel.TabStop = true;
			this.radExcel.Text = "Excel";
			this.radExcel.UseVisualStyleBackColor = true;
			this.radExcel.CheckedChanged += new System.EventHandler(this.radExcel_CheckedChanged);
			// 
			// gbxInclude
			// 
			this.gbxInclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbxInclude.Controls.Add(this.radAll);
			this.gbxInclude.Controls.Add(this.radCurrent);
			this.gbxInclude.Controls.Add(this.chkIncludeFormatting);
			this.gbxInclude.Location = new System.Drawing.Point(13, 130);
			this.gbxInclude.Name = "gbxInclude";
			this.gbxInclude.Size = new System.Drawing.Size(465, 100);
			this.gbxInclude.TabIndex = 12;
			this.gbxInclude.TabStop = false;
			this.gbxInclude.Text = "Include";
			// 
			// radAll
			// 
			this.radAll.AutoSize = true;
			this.radAll.Location = new System.Drawing.Point(21, 69);
			this.radAll.Name = "radAll";
			this.radAll.Size = new System.Drawing.Size(71, 17);
			this.radAll.TabIndex = 8;
			this.radAll.TabStop = true;
			this.radAll.Text = "All Values";
			this.radAll.UseVisualStyleBackColor = true;
			// 
			// radCurrent
			// 
			this.radCurrent.AutoSize = true;
			this.radCurrent.Location = new System.Drawing.Point(21, 45);
			this.radCurrent.Name = "radCurrent";
			this.radCurrent.Size = new System.Drawing.Size(94, 17);
			this.radCurrent.TabIndex = 7;
			this.radCurrent.TabStop = true;
			this.radCurrent.Text = "Current Values";
			this.radCurrent.UseVisualStyleBackColor = true;
			// 
			// MIDExport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(489, 282);
			this.Controls.Add(this.gbxInclude);
			this.Controls.Add(this.gbxLocation);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "MIDExport";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MIDExport";
			this.Load += new System.EventHandler(this.MIDExport_Load);
			this.Controls.SetChildIndex(this.btnOK, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.gbxLocation, 0);
			this.Controls.SetChildIndex(this.gbxInclude, 0);
			this.gbxLocation.ResumeLayout(false);
			this.gbxLocation.PerformLayout();
			this.gbxInclude.ResumeLayout(false);
			this.gbxInclude.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkIncludeFormatting;
		private System.Windows.Forms.Label lblFileName;
		private System.Windows.Forms.Button btnFileName;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.SaveFileDialog sfdFileName;
		private System.Windows.Forms.GroupBox gbxLocation;
		private System.Windows.Forms.RadioButton radFile;
		private System.Windows.Forms.RadioButton radExcel;
		private System.Windows.Forms.GroupBox gbxInclude;
		private System.Windows.Forms.RadioButton radAll;
		private System.Windows.Forms.RadioButton radCurrent;
	}
}
