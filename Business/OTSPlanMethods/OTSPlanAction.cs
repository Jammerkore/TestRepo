using System;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	[Serializable]
	public class OTSPlanAction:ApplicationBaseAction
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============
		public OTSPlanAction(eMethodType aOTSPlanAction)
			:base(-(int)aOTSPlanAction, aOTSPlanAction)
		{
		}
		//===========
		// PROPERTIES
		//===========
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.OTSPlanAction;
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
			ApplicationWorkFlowStep aOTSPlanWorkFlowStep, 
			Profile aOTSPlanProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
			OTSPlanWorkFlowStep awfs = (OTSPlanWorkFlowStep) aOTSPlanWorkFlowStep;
			OTSPlanProfile ap = (OTSPlanProfile) aOTSPlanProfile;
			//			if (ap.Action((eOTSPlanMethodType)base.MethodType, awfs.Component, awfs.TolerancePercent, awfs._storeFilterRID,WriteToDB))
//			if (ap.Action((eOTSPlanMethodType)base.MethodType, awfs.Component, awfs.TolerancePercent, aStoreFilterRID,WriteToDB))
//			{
//				aApplicationTransaction.SetOTSPlanActionStatus(ap.Key, eOTSPlanActionStatus.ActionCompletedSuccessfully);
//			}
//			else
//			{
//				aApplicationTransaction.SetOTSPlanActionStatus(ap.Key, eOTSPlanActionStatus.ActionFailed);
//			}
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
