using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for OTSPlanInventoryMethod.
	/// CURRENTLY THIS CLASS IS NOT USED...
	/// </summary>
	public class OTSPlanInventoryMethod : OTSPlanBaseMethod
	{

		/// <summary>
		/// Gets the ProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodInventory;
			}
		}


		public OTSPlanInventoryMethod(SessionAddressBlock SAB, int aMethodRID): base (SAB, 
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//aMethodRID, eMethodType.OTSPlan)
		aMethodRID, eMethodType.OTSPlan, eProfileType.MethodOTSPlan)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{

		}

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			this.ProcessAction(
				aApplicationTransaction.SAB,
				aApplicationTransaction,
				null,
				methodProfile,
				true,
				Include.NoRID);

		}

		public override void ProcessAction(SessionAddressBlock aSAB, ApplicationSessionTransaction aApplicationTransaction, ApplicationWorkFlowStep aWorkFlowStep, Profile aProfile, bool WriteToDB, int aStoreFilterRID)
		{
		}

		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
		{
			OTSPlanInventoryMethod newOTSPlanInventoryMethod = null;

			try
			{
				newOTSPlanInventoryMethod = (OTSPlanInventoryMethod)this.MemberwiseClone();
				return newOTSPlanInventoryMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Returns a flag identifying if the user can update the data on the method.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aUserRID">
		/// The internal key of the user</param>
		/// <returns>
		/// A flag.
		/// </returns>
		override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
		{
			try
			{
				// this is verified as part of the OTSPlanMethod
				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
        // End MID Track 4858

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSPlan);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            throw new NotImplementedException("MethodGetData is not implemented");
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(ROOverrideLowLevel overrideLowLevel, out bool successful, ref string message)
        {
            successful = true;

            throw new NotImplementedException("MethodGetOverrideModelList is not implemented");
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
