using System;
using System.Data;
using System.Globalization;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinListBar;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinMaskedEdit;

using MIDRetail.Business;   
using MIDRetail.Business.Allocation; 
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for CustomWindowsRoutines.
	/// </summary>
	public class CustomWindowsRoutines : BaseWindowsRoutines
	{
		public CustomWindowsRoutines(SessionAddressBlock aSAB) : base (aSAB)
		{
			
		}

        // BEGIN MID Track #6394 - update custom charactersitic
        public void CheckCustomCharacteristic(DataRow aDataRow)
        {
            
        }
        // END MID Track #6394 - update custom charactersitic
    }
}
