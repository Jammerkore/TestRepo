using System;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.Business
{
	class AllocationSummaryChangeMethods : AssortmentChangeMethods
	{
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of ChangeMethods.
		/// </summary>

		public AllocationSummaryChangeMethods(AllocationSummaryComputations aComputations)
			: base(aComputations)
		{
		}
	}
}
