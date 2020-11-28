namespace MIDRetail.Windows
{
    partial class IndexToAverageDialog
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
			this.rdoTotal = new System.Windows.Forms.RadioButton();
			this.rdoSetTotal = new System.Windows.Forms.RadioButton();
			this.rdoGrades = new System.Windows.Forms.RadioButton();
			this.txtTotal = new System.Windows.Forms.TextBox();
			this.grdGrades = new System.Windows.Forms.DataGridView();
			this.Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Average = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.gbSpreadOption = new System.Windows.Forms.GroupBox();
			this.rbSpreadByIndex = new System.Windows.Forms.RadioButton();
			this.rbSmooth = new System.Windows.Forms.RadioButton();
			this.gbSelection = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdGrades)).BeginInit();
			this.gbSpreadOption.SuspendLayout();
			this.gbSelection.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// rdoTotal
			// 
			this.rdoTotal.AutoSize = true;
			this.rdoTotal.Location = new System.Drawing.Point(9, 21);
			this.rdoTotal.Name = "rdoTotal";
			this.rdoTotal.Size = new System.Drawing.Size(52, 17);
			this.rdoTotal.TabIndex = 1;
			this.rdoTotal.TabStop = true;
			this.rdoTotal.Text = "Total:";
			this.rdoTotal.UseVisualStyleBackColor = true;
			this.rdoTotal.CheckedChanged += new System.EventHandler(this.rdoTotal_CheckedChanged);
			// 
			// rdoSetTotal
			// 
			this.rdoSetTotal.AutoSize = true;
			this.rdoSetTotal.Location = new System.Drawing.Point(76, 21);
			this.rdoSetTotal.Name = "rdoSetTotal";
			this.rdoSetTotal.Size = new System.Drawing.Size(71, 17);
			this.rdoSetTotal.TabIndex = 3;
			this.rdoSetTotal.TabStop = true;
			this.rdoSetTotal.Text = "Set Total:";
			this.rdoSetTotal.UseVisualStyleBackColor = true;
			this.rdoSetTotal.CheckedChanged += new System.EventHandler(this.rdoSetTotal_CheckedChanged);
			// 
			// rdoGrades
			// 
			this.rdoGrades.AutoSize = true;
			this.rdoGrades.Location = new System.Drawing.Point(166, 21);
			this.rdoGrades.Name = "rdoGrades";
			this.rdoGrades.Size = new System.Drawing.Size(62, 17);
			this.rdoGrades.TabIndex = 5;
			this.rdoGrades.TabStop = true;
			this.rdoGrades.Text = "Grades:";
			this.rdoGrades.UseVisualStyleBackColor = true;
			this.rdoGrades.CheckedChanged += new System.EventHandler(this.rdoGrades_CheckedChanged);
			// 
			// txtTotal
			// 
			this.txtTotal.Location = new System.Drawing.Point(44, 48);
			this.txtTotal.Name = "txtTotal";
			this.txtTotal.Size = new System.Drawing.Size(126, 20);
			this.txtTotal.TabIndex = 2;
			// 
			// grdGrades
			// 
			this.grdGrades.AllowUserToAddRows = false;
			this.grdGrades.AllowUserToDeleteRows = false;
			this.grdGrades.AllowUserToResizeColumns = false;
			this.grdGrades.AllowUserToResizeRows = false;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			this.grdGrades.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
			this.grdGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grdGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdGrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Grade,
            this.Average});
			this.grdGrades.Location = new System.Drawing.Point(44, 79);
			this.grdGrades.MultiSelect = false;
			this.grdGrades.Name = "grdGrades";
			this.grdGrades.RowHeadersVisible = false;
			this.grdGrades.Size = new System.Drawing.Size(190, 189);
			this.grdGrades.TabIndex = 6;
			this.grdGrades.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdGrades_DataError);
			// 
			// Grade
			// 
			this.Grade.HeaderText = "Grade";
			this.Grade.Name = "Grade";
			this.Grade.ReadOnly = true;
			this.Grade.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// Average
			// 
			this.Average.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Average.HeaderText = "Average";
			this.Average.Name = "Average";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(227, 370);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(146, 370);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "&Ok";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// gbSpreadOption
			// 
			this.gbSpreadOption.Controls.Add(this.rbSmooth);
			this.gbSpreadOption.Controls.Add(this.rbSpreadByIndex);
			this.gbSpreadOption.Location = new System.Drawing.Point(12, 12);
			this.gbSpreadOption.Name = "gbSpreadOption";
			this.gbSpreadOption.Size = new System.Drawing.Size(285, 59);
			this.gbSpreadOption.TabIndex = 9;
			this.gbSpreadOption.TabStop = false;
			this.gbSpreadOption.Text = "Spread Option";
			// 
			// rbSpreadByIndex
			// 
			this.rbSpreadByIndex.AutoSize = true;
			this.rbSpreadByIndex.Location = new System.Drawing.Point(9, 24);
			this.rbSpreadByIndex.Name = "rbSpreadByIndex";
			this.rbSpreadByIndex.Size = new System.Drawing.Size(102, 17);
			this.rbSpreadByIndex.TabIndex = 0;
			this.rbSpreadByIndex.TabStop = true;
			this.rbSpreadByIndex.Text = "Spread by Index";
			this.rbSpreadByIndex.UseVisualStyleBackColor = true;
			this.rbSpreadByIndex.CheckedChanged += new System.EventHandler(this.rbSpreadByIndex_CheckedChanged);
			// 
			// rbSmooth
			// 
			this.rbSmooth.AutoSize = true;
			this.rbSmooth.Location = new System.Drawing.Point(166, 24);
			this.rbSmooth.Name = "rbSmooth";
			this.rbSmooth.Size = new System.Drawing.Size(61, 17);
			this.rbSmooth.TabIndex = 1;
			this.rbSmooth.TabStop = true;
			this.rbSmooth.Text = "Smooth";
			this.rbSmooth.UseVisualStyleBackColor = true;
			this.rbSmooth.CheckedChanged += new System.EventHandler(this.rbSmooth_CheckedChanged);
			// 
			// gbSelection
			// 
			this.gbSelection.Controls.Add(this.label1);
			this.gbSelection.Controls.Add(this.rdoSetTotal);
			this.gbSelection.Controls.Add(this.rdoTotal);
			this.gbSelection.Controls.Add(this.rdoGrades);
			this.gbSelection.Controls.Add(this.grdGrades);
			this.gbSelection.Controls.Add(this.txtTotal);
			this.gbSelection.Location = new System.Drawing.Point(12, 74);
			this.gbSelection.Name = "gbSelection";
			this.gbSelection.Size = new System.Drawing.Size(285, 286);
			this.gbSelection.TabIndex = 10;
			this.gbSelection.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(176, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "(Average)";
			// 
			// IndexToAverageDialog
			// 
			this.AllowDragDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(309, 405);
			this.Controls.Add(this.gbSpreadOption);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.gbSelection);
			this.Name = "IndexToAverageDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Spread Average";
			this.Load += new System.EventHandler(this.IndexToAverageDialog_Load);
			this.Controls.SetChildIndex(this.gbSelection, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.gbSpreadOption, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdGrades)).EndInit();
			this.gbSpreadOption.ResumeLayout(false);
			this.gbSpreadOption.PerformLayout();
			this.gbSelection.ResumeLayout(false);
			this.gbSelection.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoTotal;
        private System.Windows.Forms.RadioButton rdoSetTotal;
        private System.Windows.Forms.RadioButton rdoGrades;
		private System.Windows.Forms.TextBox txtTotal;
		private System.Windows.Forms.DataGridView grdGrades;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.DataGridViewTextBoxColumn Grade;
		private System.Windows.Forms.DataGridViewTextBoxColumn Average;
		private System.Windows.Forms.GroupBox gbSpreadOption;
		private System.Windows.Forms.RadioButton rbSmooth;
		private System.Windows.Forms.RadioButton rbSpreadByIndex;
		private System.Windows.Forms.GroupBox gbSelection;
		private System.Windows.Forms.Label label1;
    }
}