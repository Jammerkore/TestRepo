using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Windows.Controls
{
	/// <summary>
	/// Summary description for Progress.
	/// </summary>
	public class frmProgress : System.Windows.Forms.Form
	{
		// add event when ok button is clicked
		public delegate void ProgressOKClickedEventHandler(object source, ProgressOKClickedEventArgs e);
		public event ProgressOKClickedEventHandler OnProgressOKClickedHandler;

		private string _titleRoot = null;
		private string _percentCompleteMsg = null;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ProgressBar progressBar;
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
		/// Sets the value for the progress of the status bar.
		/// </summary>
		public int SetValue 
		{
			set 
			{
                if (value <= progressBar.Maximum)
                {
                    progressBar.Value = value;
                    this.Text = _titleRoot + " " + String.Format(_percentCompleteMsg + " {0}%", (progressBar.Value * 100) / (progressBar.Maximum - progressBar.Minimum));
                }
			}
		}

		/// <summary>
		/// Sets the value for the progress of the status bar.
		/// </summary>
		public int SetMaxValue
		{
			set
			{
				progressBar.Maximum = value;
				this.Text = _titleRoot + " " + String.Format(_percentCompleteMsg + " {0}%", (progressBar.Value * 100) / (progressBar.Maximum - progressBar.Minimum));
			}
		}

		public frmProgress(int minimumValue, int maximumValue)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			progressBar.Minimum = minimumValue;
			progressBar.Maximum = maximumValue;
			this.btnOK.Enabled = false;
			_percentCompleteMsg = MIDText.GetTextOnly(eMIDTextCode.msg_PercentComplete);
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
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btnOK = new System.Windows.Forms.Button();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(16, 32);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(320, 23);
			this.progressBar.TabIndex = 0;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(143, 144);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label
			// 
			this.label.Location = new System.Drawing.Point(16, 80);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(320, 32);
			this.label.TabIndex = 3;
			this.label.Text = "label";
			// 
			// frmProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(360, 190);
			this.Controls.Add(this.label);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.progressBar);
			this.Name = "frmProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Progress";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmProgress_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Requests the progress form to close
		/// </summary>
		public void CloseForm()
		{
			this.Close();
		}

		public void EnableOKButton()
		{
			this.btnOK.Enabled = true;
		}

		public void DisableOKButton()
		{
			this.btnOK.Enabled = false;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			ProgressOKClickedEventArgs ea = new ProgressOKClickedEventArgs();
			if (OnProgressOKClickedHandler != null)  // throw event to so caller knows button clicked
			{
				OnProgressOKClickedHandler(this, ea);
			}
			this.Close();
		}

		private void frmProgress_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
			this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmProgress_Closing);
		}
	}

	public class ProgressOKClickedEventArgs : EventArgs
	{
		bool _formClosing;
		
		public ProgressOKClickedEventArgs()
		{
			_formClosing = true;
		}
		public bool FormClosing 
		{
			get { return _formClosing ; }
			set { _formClosing = value; }
		}
	}
}
