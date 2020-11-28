using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	[Serializable]
	public class AssortmentHeaderProfile : AllocationHeaderProfile
	{
		public AssortmentHeaderProfile(int aKey)
			: base(aKey)
		{

		}

		public AssortmentHeaderProfile(string headerID, int aKey)
			: base(aKey)
		{
			HeaderID = headerID;
		}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentHeader;
			}
		}
	}
}
