using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
	public partial class MIDToolbarRadioButton : UserControl
	{
		public string RadioButton1Text
		{
			get { return rbOne.Text; }
			set { rbOne.Text = value; }
		}

		public string RadioButton2Text
		{
			get { return rbTwo.Text; }
			set { rbTwo.Text = value; }
		}

		public bool IsButton1Checked
		{
			get { return rbOne.Checked; }
		}

		public bool IsButton2Checked
		{
			get { return rbTwo.Checked; }
		}

		public RadioButton Button1
		{
			get { return rbOne; }
		}

		public RadioButton Button2
		{
			get { return rbTwo; }
		}

		public MIDToolbarRadioButton()
		{
			InitializeComponent();
		}

		public void CheckButton1()
		{
			rbOne.Checked = true;
		}

		public void CheckButton2()
		{
			rbTwo.Checked = true;
		}
	}
}
