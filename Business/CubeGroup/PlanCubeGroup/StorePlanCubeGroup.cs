using System;
using System.Collections;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// This abstract class defines the StorePlanCubeGroup.
	/// </summary>
	/// <remarks>
	/// The StorePlanCubeGroup defines a PlanCubeGroup that will create Store Cubes.
	/// </remarks>

	abstract public class StorePlanCubeGroup : PlanCubeGroup
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StorePlanCubeGroup, using the given SessionAddressBlock and Transaction.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StorePlanMainCubeGroup is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StorePlanMainCubeGroup is a part of.
		/// </param>

		public StorePlanCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Opens a StorePlanCubeGroup and performs any routines specific to Store CubeGroups.
		/// </summary>
		/// <param name="aOpenParms">
		/// The PlanOpenParms object that contains information about the plan.
		/// </param>

		override public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
			ProfileList storeProfileList;

			try
			{
				base.OpenCubeGroup(aOpenParms);

				//==================
				// Initialize fields
				//==================

				storeProfileList = GetFilteredProfileList(eProfileType.Store);

				//===============================
				// Build Similar Store Model Hash
				//===============================

				if (_openParms.SimilarStores)
				{
					foreach (StoreProfile storeProf in storeProfileList)
					{
						if (storeProf.SimilarStoreModel)
						{
							_simStoreModelHash.Add(storeProf.Key, null);
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
