namespace MIDRetail.Windows.Controls
{
	partial class MIDToolbarRadioButton
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rbOne = new System.Windows.Forms.RadioButton();
			this.rbTwo = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// rbOne
			// 
			this.rbOne.AutoSize = true;
			this.rbOne.Checked = true;
			this.rbOne.Location = new System.Drawing.Point(4, 2);
			this.rbOne.Name = "rbOne";
			this.rbOne.Size = new System.Drawing.Size(85, 17);
			this.rbOne.TabIndex = 0;
			this.rbOne.TabStop = true;
			this.rbOne.Text = "radioButton1";
			this.rbOne.UseVisualStyleBackColor = true;
			// 
			// rbTwo
			// 
			this.rbTwo.AutoSize = true;
			this.rbTwo.Location = new System.Drawing.Point(89, 2);
			this.rbTwo.Name = "rbTwo";
			this.rbTwo.Size = new System.Drawing.Size(85, 17);
			this.rbTwo.TabIndex = 1;
			this.rbTwo.Text = "radioButton2";
			this.rbTwo.UseVisualStyleBackColor = true;
			// 
			// MIDToolbarRadioButton
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.rbTwo);
			this.Controls.Add(this.rbOne);
			this.Name = "MIDToolbarRadioButton";
			this.Size = new System.Drawing.Size(195, 22);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rbOne;
		private System.Windows.Forms.RadioButton rbTwo;
	}
}
