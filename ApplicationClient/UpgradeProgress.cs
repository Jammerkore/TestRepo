using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MIDRetail.Client
{
	/// <summary>
	/// Summary description for UpgradeProgress.
	/// </summary>
	public class frmUpgradeProgress : System.Windows.Forms.Form
	{
		private string _titleRoot = null;
		private string _percentCompleteMsg = null;
		private System.Windows.Forms.ProgressBar upgradeProgressBar;
		private System.Windows.Forms.Label label;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Sets the text of the title the status bar.
		/// </summary>
		public string Title 
		{
			set { _titleRoot = value ; }
		}

		/// <summary>
		/// Sets the text of the label above the status bar.
		/// </summary>
		public string labelText 
		{
			set 
			{ 
				label.Text = value ; 
				this.Refresh();
			}
		}

		/// <summary>
		/// Sets the value for the UpgradeProgress of the status bar.
		/// </summary>
		public int SetValue 
		{
			set 
			{ 
				upgradeProgressBar.Value = value ;
				this.Text = _titleRoot + " " + String.Format( _percentCompleteMsg + " {0}%", (upgradeProgressBar.Value * 100 ) / (upgradeProgressBar.Maximum - upgradeProgressBar.Minimum) );
			}
		}

		/// <summary>
		/// Sets the value for the UpgradeProgress of the status bar.
		/// </summary>
		public int SetMaxValue 
		{
			set 
			{ 
				upgradeProgressBar.Maximum = value ;
			}
		}

		public frmUpgradeProgress(int minimumValue, int maximumValue)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			upgradeProgressBar.Minimum = minimumValue;
			upgradeProgressBar.Maximum = maximumValue;
			_percentCompleteMsg = "Percent Complete";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.upgradeProgressBar = new System.Windows.Forms.ProgressBar();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// upgradeProgressBar
			// 
			this.upgradeProgressBar.Location = new System.Drawing.Point(16, 32);
			this.upgradeProgressBar.Name = "upgradeProgressBar";
			this.upgradeProgressBar.Size = new System.Drawing.Size(320, 23);
			this.upgradeProgressBar.TabIndex = 0;
			// 
			// label
			// 
			this.label.Location = new System.Drawing.Point(16, 80);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(320, 56);
			this.label.TabIndex = 3;
			this.label.Text = "label";
			// 
			// frmUpgradeProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 150);
			this.Controls.Add(this.label);
			this.Controls.Add(this.upgradeProgressBar);
			this.Name = "frmUpgradeProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "UpgradeProgress";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmUpgradeProgress_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Requests the UpgradeProgress form to close
		/// </summary>
		public void CloseForm()
		{
			this.Close();
		}

		private void frmUpgradeProgress_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmUpgradeProgress_Closing);
		}
	}
}
