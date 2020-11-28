using System;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	/// <summary>
	/// A class when the method type is not known.
	/// </summary>
	/// <remarks>
	/// This method inherits from the base application method.  
	/// </remarks>
	[Serializable]
	public class UnknownMethod:ApplicationBaseMethod
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
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public UnknownMethod(SessionAddressBlock SAB, int aMethodRID):base(SAB, aMethodRID,(eMethodType)(-1))
		public UnknownMethod(SessionAddressBlock SAB, int aMethodRID):base(SAB, aMethodRID,(eMethodType)(-1),(eProfileType)(-1))
		//End TT#523 - JScott - Duplicate folder when new folder added
		{

		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the ProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodOTSPlan;
			}
		}

		//========
		// METHODS
		//========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		internal override void VerifyAction(eMethodType aMethodType)
		{
			
		}

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
			UnknownMethod newUnknownMethod = null;

			try
			{
				newUnknownMethod = (UnknownMethod)this.MemberwiseClone();
				return newUnknownMethod;
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
			return false;
		}
        // End MID Track 4858

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            return null;
        }

        override public ROMethodProperties MethodGetData(bool processingApply)
        {
            throw new NotImplementedException("MethodGetData is not implemented");
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, bool processingApply)
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
