using System;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	[Serializable]
	public class AllocationAction:ApplicationBaseAction
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============
		public AllocationAction(eMethodType aAllocationAction)
			:base(-(int)aAllocationAction, aAllocationAction)
		{
		}
		//===========
		// PROPERTIES
		//===========
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.AllocationAction;
			}
		}

		//========
		// METHODS
		//========
		public override void ProcessMethod(ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
		}

		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aAllocationWorkFlowStep, 
			Profile aAllocationProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
			AllocationWorkFlowStep awfs = (AllocationWorkFlowStep) aAllocationWorkFlowStep;
            AllocationProfile ap = (AllocationProfile) aAllocationProfile;
//			if (ap.Action((eAllocationMethodType)base.MethodType, awfs.Component, awfs.TolerancePercent, awfs._storeFilterRID,WriteToDB))
			if (ap.Action((eAllocationMethodType)base.MethodType, awfs.Component, awfs.TolerancePercent, aStoreFilterRID,WriteToDB))
			{
				aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
			}
			else
			{
				aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
			}
		}
		internal override void VerifyAction(eMethodType aMethodType)
		{
			if (Enum.IsDefined(typeof(eMethodTypeUI),(eMethodTypeUI)aMethodType))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_ActionCannotHaveUI,
					MIDText.GetText(eMIDTextCode.msg_ActionCannotHaveUI));
			}

		}

	}
}
