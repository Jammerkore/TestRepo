using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
	public class RowColChooserPanel : UserControl
	{
		//=======
		// FIELDS
		//=======

		protected ArrayList _headers;
		protected bool _oneHeaderRequired;

		//=============
		// CONSTRUCTORS
		//=============

		public RowColChooserPanel()
		{
			_headers = null;
			_oneHeaderRequired = false;
		}

		public RowColChooserPanel(ArrayList aHeaders, bool aOneHeaderRequired)
		{
			_headers = aHeaders;
			_oneHeaderRequired = aOneHeaderRequired;
		}

		//===========
		// PROPERTIES
		//===========

		virtual public bool isChanged
		{
			get
			{
				throw new Exception("Unassigned function: RowColChooserPanel::isChanged(get)");
			}
		}

		public ArrayList Headers
		{
			get
			{
				return _headers;
			}
		}

		public bool OneHeaderRequired
		{
			get
			{
				return _oneHeaderRequired;
			}
		}

		//========
		// METHODS
		//========

		virtual public void FillControl()
		{
			throw new Exception("Unassigned function: RowColChooserPanel::FillControl");
		}

		virtual public bool ValidateData()
		{
			throw new Exception("Unassigned function: RowColChooserPanel::ValidateData");
		}

		virtual public void UpdateData()
		{
			throw new Exception("Unassigned function: RowColChooserPanel::UpdateData");
		}
//Begin Modification - JScott - Export Method - Fix 1

		virtual public void ResetChangedFlag()
		{
			throw new Exception("Unassigned function: RowColChooserPanel::UpdateData");
		}
//End Modification - JScott - Export Method - Fix 1
	}
}

