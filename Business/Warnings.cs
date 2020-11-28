using System;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for Warnings.
	/// </summary>

	class MaxLessThanRequestedTotal : MIDException
	{
		public MaxLessThanRequestedTotal() 
			: base (eErrorLevel.warning, 1, MIDText.GetText(eMIDTextCode.msg_MaxLessThanRequestedTotal))
		{
		}
	}

	class MinimumsBroken : MIDException
	{
		public MinimumsBroken() 
			: base (eErrorLevel.warning, 2, MIDText.GetText(eMIDTextCode.msg_MinimumsBroken))
		{
		}
	}

	class SpreadTotalNotEqualToRequestedTotal : MIDException
	{
		public SpreadTotalNotEqualToRequestedTotal() 
			: base (eErrorLevel.warning, 3, MIDText.GetText(eMIDTextCode.msg_SpreadTotNotEqualToReqTot))
		{
		}
	}

	class NoEligibleItems : MIDException
	{
		public NoEligibleItems() 
			: base (eErrorLevel.warning, 4, MIDText.GetText(eMIDTextCode.msg_NoEligibleItems))
		{
		}
	}

	class VGDefinitionsIncomplete : MIDException
	{
		public VGDefinitionsIncomplete() 
			: base (eErrorLevel.warning, 5, MIDText.GetText(eMIDTextCode.msg_VGDefinitionsIncomplete))
		{
		}
	}

	class NoPeriodHasPositiveSalesOrBeginningStock : MIDException
	{
		public NoPeriodHasPositiveSalesOrBeginningStock() 
			: base (eErrorLevel.warning, 6, MIDText.GetText(eMIDTextCode.msg_NoPerHasPosSalesOrBegStock))
		{
		}
	}

	class AverageBeginningStockIsZero : MIDException
	{
		public AverageBeginningStockIsZero() 
			: base (eErrorLevel.warning, 7, MIDText.GetText(eMIDTextCode.msg_AverageBeginningStockIsZero))
		{
		}
	}

	class NoAttributeSetsToPlan : MIDException
	{
		public NoAttributeSetsToPlan() 
			: base (eErrorLevel.warning, 7, MIDText.GetText(eMIDTextCode.msg_pl_NoAttributeSetsToPlan))
		{
		}
	}

	class NoProcessingByFillSizeHoles : MIDException
	{
		public NoProcessingByFillSizeHoles() 
			: base (eErrorLevel.warning, 7, MIDText.GetText(eMIDTextCode.msg_al_NoPositivePercentNeed))
		{
		}
	}

	// Begin Issue 4827 stodd 10.29.2007
	class EndProcessingException : MIDException
	{
		public EndProcessingException(string errMsg) 
			: base (eErrorLevel.warning, 8, errMsg)
		{
		}
	}
	// END Issue 4827
	//Begin TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens

	public class NoSizesToDisplayException : MIDException
	{
		public NoSizesToDisplayException()
			: base(eErrorLevel.warning, 9, MIDText.GetText(eMIDTextCode.msg_al_NoSizesToDisplay))
		{
		}
	}
	//End TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
}
