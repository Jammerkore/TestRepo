using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for FilterWizardControl.
	/// </summary>
	public class FilterWizardControl : System.Windows.Forms.UserControl, IDisposable
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilterWizardControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
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

				if (_nextButtonStatusChanged != null)
				{
					foreach (NextButtonStatusEventHandler handler in _nextButtonStatusChanged.GetInvocationList())
					{
						_nextButtonStatusChanged -= handler;
					}
				}

				Include.DisposeControls(this.Controls);
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		public delegate void NextButtonStatusEventHandler(object source, NextButtonStatusEventArgs e);
		public event NextButtonStatusEventHandler _nextButtonStatusChanged;
		protected frmFilterWizard _parentForm;

		public event NextButtonStatusEventHandler NextButtonStatusChanged 
		{
			add 
			{
				_nextButtonStatusChanged += value;
			}
			remove
			{
				_nextButtonStatusChanged -= value;
			}
		}

		public void FireNextButtonStatusChangedEvent(bool aIsNextEnabled)
		{
			if (_nextButtonStatusChanged != null)
			{
				_nextButtonStatusChanged(this, new NextButtonStatusEventArgs(aIsNextEnabled));
			}
		}
	}
}
