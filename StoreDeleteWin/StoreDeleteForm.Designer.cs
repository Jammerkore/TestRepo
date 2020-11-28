namespace MIDRetail.StoreDelete
{
	partial class StoreDeleteForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreDeleteForm));
            this.btProcessSD = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.lbSave = new System.Windows.Forms.Label();
            this.tbRowPct = new System.Windows.Forms.TextBox();
            this.tbMaxRowCount = new System.Windows.Forms.TextBox();
            this.tbMinRowCount = new System.Windows.Forms.TextBox();
            this.tbBatchSize = new System.Windows.Forms.TextBox();
            this.tbConProcessDelete = new System.Windows.Forms.TextBox();
            this.tbConProcessCopy = new System.Windows.Forms.TextBox();
            this.btSave = new System.Windows.Forms.Button();
            this.gbAnalysis = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.msgAnalysis = new System.Windows.Forms.Label();
            this.lbAnalysis = new System.Windows.Forms.ListBox();
            this.btProcessA = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btReset = new System.Windows.Forms.Button();
            this.gbConfig.SuspendLayout();
            this.gbAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // btProcessSD
            // 
            this.btProcessSD.Location = new System.Drawing.Point(17, 425);
            this.btProcessSD.Name = "btProcessSD";
            this.btProcessSD.Size = new System.Drawing.Size(133, 23);
            this.btProcessSD.TabIndex = 0;
            this.btProcessSD.Text = "Process Store &Delete";
            this.btProcessSD.UseVisualStyleBackColor = true;
            this.btProcessSD.Click += new System.EventHandler(this.btProcessSD_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(413, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Minimum Row Count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Number of Concurrent Processes for Deletes";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(413, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Maximum Row Count";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(386, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Row Percentage Maximum";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(227, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Batch Size";
            // 
            // gbConfig
            // 
            this.gbConfig.Controls.Add(this.lbSave);
            this.gbConfig.Controls.Add(this.tbRowPct);
            this.gbConfig.Controls.Add(this.label3);
            this.gbConfig.Controls.Add(this.label7);
            this.gbConfig.Controls.Add(this.tbMaxRowCount);
            this.gbConfig.Controls.Add(this.tbMinRowCount);
            this.gbConfig.Controls.Add(this.tbBatchSize);
            this.gbConfig.Controls.Add(this.tbConProcessDelete);
            this.gbConfig.Controls.Add(this.tbConProcessCopy);
            this.gbConfig.Controls.Add(this.btSave);
            this.gbConfig.Controls.Add(this.label2);
            this.gbConfig.Controls.Add(this.label6);
            this.gbConfig.Controls.Add(this.label4);
            this.gbConfig.Location = new System.Drawing.Point(12, 12);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(728, 171);
            this.gbConfig.TabIndex = 8;
            this.gbConfig.TabStop = false;
            this.gbConfig.Text = "Store Delete Settings";
            // 
            // lbSave
            // 
            this.lbSave.AutoSize = true;
            this.lbSave.Location = new System.Drawing.Point(98, 148);
            this.lbSave.Name = "lbSave";
            this.lbSave.Size = new System.Drawing.Size(35, 13);
            this.lbSave.TabIndex = 13;
            this.lbSave.Text = "label8";
            // 
            // tbRowPct
            // 
            this.tbRowPct.Location = new System.Drawing.Point(529, 83);
            this.tbRowPct.Name = "tbRowPct";
            this.tbRowPct.Size = new System.Drawing.Size(77, 20);
            this.tbRowPct.TabIndex = 12;
            this.tbRowPct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbRowPct.TextChanged += new System.EventHandler(this.tbRowPct_TextChanged);
            // 
            // tbMaxRowCount
            // 
            this.tbMaxRowCount.Location = new System.Drawing.Point(529, 58);
            this.tbMaxRowCount.Name = "tbMaxRowCount";
            this.tbMaxRowCount.Size = new System.Drawing.Size(77, 20);
            this.tbMaxRowCount.TabIndex = 12;
            this.tbMaxRowCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbMaxRowCount.TextChanged += new System.EventHandler(this.tbMaxRowCount_TextChanged);
            // 
            // tbMinRowCount
            // 
            this.tbMinRowCount.Location = new System.Drawing.Point(529, 33);
            this.tbMinRowCount.Name = "tbMinRowCount";
            this.tbMinRowCount.Size = new System.Drawing.Size(77, 20);
            this.tbMinRowCount.TabIndex = 11;
            this.tbMinRowCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbMinRowCount.TextChanged += new System.EventHandler(this.tbMinRowCount_TextChanged);
            // 
            // tbBatchSize
            // 
            this.tbBatchSize.Location = new System.Drawing.Point(292, 84);
            this.tbBatchSize.Name = "tbBatchSize";
            this.tbBatchSize.Size = new System.Drawing.Size(54, 20);
            this.tbBatchSize.TabIndex = 9;
            this.tbBatchSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBatchSize.TextChanged += new System.EventHandler(this.tbBatchSize_TextChanged);
            // 
            // tbConProcessDelete
            // 
            this.tbConProcessDelete.Location = new System.Drawing.Point(292, 58);
            this.tbConProcessDelete.Name = "tbConProcessDelete";
            this.tbConProcessDelete.Size = new System.Drawing.Size(54, 20);
            this.tbConProcessDelete.TabIndex = 9;
            this.tbConProcessDelete.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbConProcessDelete.TextChanged += new System.EventHandler(this.tbConProcessDelete_TextChanged);
            // 
            // tbConProcessCopy
            // 
            this.tbConProcessCopy.Location = new System.Drawing.Point(292, 33);
            this.tbConProcessCopy.Name = "tbConProcessCopy";
            this.tbConProcessCopy.Size = new System.Drawing.Size(54, 20);
            this.tbConProcessCopy.TabIndex = 8;
            this.tbConProcessCopy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbConProcessCopy.TextChanged += new System.EventHandler(this.tbConProcessCopy_TextChanged);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(7, 142);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 7;
            this.btSave.Text = "&Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // gbAnalysis
            // 
            this.gbAnalysis.Controls.Add(this.label5);
            this.gbAnalysis.Controls.Add(this.msgAnalysis);
            this.gbAnalysis.Controls.Add(this.lbAnalysis);
            this.gbAnalysis.Controls.Add(this.btProcessA);
            this.gbAnalysis.Location = new System.Drawing.Point(13, 201);
            this.gbAnalysis.Name = "gbAnalysis";
            this.gbAnalysis.Size = new System.Drawing.Size(727, 218);
            this.gbAnalysis.TabIndex = 9;
            this.gbAnalysis.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Messages";
            // 
            // msgAnalysis
            // 
            this.msgAnalysis.AutoSize = true;
            this.msgAnalysis.Location = new System.Drawing.Point(155, 19);
            this.msgAnalysis.Name = "msgAnalysis";
            this.msgAnalysis.Size = new System.Drawing.Size(0, 13);
            this.msgAnalysis.TabIndex = 2;
            // 
            // lbAnalysis
            // 
            this.lbAnalysis.FormattingEnabled = true;
            this.lbAnalysis.Location = new System.Drawing.Point(6, 61);
            this.lbAnalysis.Name = "lbAnalysis";
            this.lbAnalysis.Size = new System.Drawing.Size(715, 147);
            this.lbAnalysis.TabIndex = 1;
            // 
            // btProcessA
            // 
            this.btProcessA.Location = new System.Drawing.Point(6, 14);
            this.btProcessA.Name = "btProcessA";
            this.btProcessA.Size = new System.Drawing.Size(133, 23);
            this.btProcessA.TabIndex = 0;
            this.btProcessA.UseVisualStyleBackColor = true;
            this.btProcessA.Click += new System.EventHandler(this.btProcessA_Click);
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(198, 425);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(116, 23);
            this.btReset.TabIndex = 11;
            this.btReset.Text = "Reset for Restart";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Visible = false;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // StoreDeleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 456);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.gbAnalysis);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btProcessSD);
            this.Controls.Add(this.gbConfig);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StoreDeleteForm";
            this.Load += new System.EventHandler(this.StoreDeleteForm_Load);
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.gbAnalysis.ResumeLayout(false);
            this.gbAnalysis.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btProcessSD;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox gbConfig;
		private System.Windows.Forms.TextBox tbRowPct;
		private System.Windows.Forms.TextBox tbMaxRowCount;
		private System.Windows.Forms.TextBox tbMinRowCount;
		private System.Windows.Forms.TextBox tbBatchSize;
		private System.Windows.Forms.TextBox tbConProcessDelete;
		private System.Windows.Forms.TextBox tbConProcessCopy;
		private System.Windows.Forms.Button btSave;
		private System.Windows.Forms.GroupBox gbAnalysis;
		private System.Windows.Forms.Label msgAnalysis;
		private System.Windows.Forms.ListBox lbAnalysis;
		private System.Windows.Forms.Button btProcessA;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btReset;
		private System.Windows.Forms.Label lbSave;
	}
}

