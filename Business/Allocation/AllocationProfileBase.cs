using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for AllocationProfileBase.
	/// </summary>
	abstract public class AllocationProfileBase:Profile
	{
		//=======
		// FIELDS
		//=======
		private ApplicationSessionTransaction _transaction;
		private SessionAddressBlock _SAB;  
		private eProfileType _profileType;


		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates a new instance of the Allocation Profile
		/// </summary>
		/// <param name="aTransaction">Transaction associated with this profile.</param>
		/// <param name="aKey">A unique integer identifier for the allocation header.</param>
		/// <remarks>
		/// An allocation profile describes an allocation header and its allocation to the stores.
		/// </remarks>
		public AllocationProfileBase(eProfileType aProfileType, ApplicationSessionTransaction aTransaction, int aKey)
			:base(aKey)
		{
			_transaction = aTransaction;
			_SAB = aTransaction.SAB;
			_profileType = aProfileType;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return _profileType;
			}
		}

		public ApplicationSessionTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		//========
		// METHODS
		//========
		#region StoreIndexRID
		/// <summary>
		/// Gets store index value for store RID.
		/// </summary>
		/// <param name="aStoreRID">RID identifier for the store.</param>
		/// <returns>Store Index associated with the storeRID</returns>
		public Index_RID StoreIndex(int aStoreRID)
		{
			Index_RID sIndexRID = _transaction.StoreIndexRID(aStoreRID);
			if (sIndexRID.RID == Include.UndefinedStoreRID)
			{
				// begin MID Track 4214 Identify stores in error messages
				throw new MIDException (eErrorLevel.severe,
					(int)(eMIDTextCode.msg_StoreRIDNotFound),
					string.Format(MIDText.GetText(eMIDTextCode.msg_StoreRIDNotFound),aStoreRID.ToString()));
				//throw new MIDException(eErrorLevel.severe,
				//	(int)(eMIDTextCode.msg_StoreRIDNotFound),
				//	MIDText.GetText(eMIDTextCode.msg_StoreRIDNotFound));
				// end MID Track 4214 Identify stores in error messages
			}
			return sIndexRID;
		}
		#endregion StoreIndexRID

		/// <summary>
		/// Gets Store Subtotal Quantity Allocated for specified store on specified component.
		/// </summary>
		/// <param name="aComponent">Description of the component</param>
		/// <param name="aStoreRID">RID for store</param>
		/// <returns>Quantity Allocated to the store for the specified component.</returns>
		abstract public int GetStoreQtyAllocated(GeneralComponent aComponent, int aStoreRID);
	}
}
