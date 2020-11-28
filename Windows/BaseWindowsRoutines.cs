using System;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinListBar;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinMaskedEdit;

using MIDRetail.Business;   
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for BaseWindowsRoutines.
	/// </summary>
	abstract public class BaseWindowsRoutines
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		SessionAddressBlock _SAB;
		#endregion Fields

		public BaseWindowsRoutines(SessionAddressBlock aSAB)
		{
			_SAB = aSAB;	
		}

		virtual public void SetElibigilityPMPlusSalesColVisibility(UltraGrid aGrid)
		{
			aGrid.DisplayLayout.Bands["Sets"].Columns["PMPlusSales"].Hidden = true;
			aGrid.DisplayLayout.Bands["Stores"].Columns["PMPlusSales"].Hidden = true;
		}
	}
}
