using System;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// The base allocation business method.
	/// </summary>
	/// <remarks>
	/// This method inherits from the base application method.  
	/// </remarks>
	abstract public class AllocationBaseMethod:ApplicationBaseMethod
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of Allocation Base Method
		/// </summary>
		/// <param name="aMethodRID">RID for the Method.</param>
		/// <param name="aMethodType">Method Type.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public AllocationBaseMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType aMethodType):base(SAB, aMethodRID, aMethodType)
		public AllocationBaseMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType aMethodType, eProfileType aProfileType):base(SAB, aMethodRID, aMethodType, aProfileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{

		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		internal override void VerifyAction(eMethodType aMethodType)
		{
			if (!Enum.IsDefined(typeof(eAllocationMethodType),Convert.ToInt32(base.MethodType, CultureInfo.CurrentUICulture)))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
					MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
			}
		}

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
		internal bool CheckAllocationCriteriaForUserData(AllocationCriteria allocationCriteria)
        {
            if (IsStoreGroupUser(SG_RID))
            {
                return true;
            }

            if (IsStoreGroupUser(allocationCriteria.GradeStoreGroupRID))
            {
                return true;
            }

            if (IsStoreGroupUser(allocationCriteria.CapacityStoreGroupRID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(allocationCriteria.HdrInventory_MERCH_HN_RID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(allocationCriteria.Inventory_MERCH_HN_RID))
            {
                return true;
            }
            
            if (IsHierarchyNodeUser(allocationCriteria.OTSPlanRID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(allocationCriteria.OTSOnHandRID))
            {
                return true;
            }

            return false;
        }
		// End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
	}

}
