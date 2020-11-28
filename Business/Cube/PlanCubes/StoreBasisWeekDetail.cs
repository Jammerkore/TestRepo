using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the StoreBasisWeekDetail.
	/// </summary>
	/// <remarks>
	/// The StoreBasisWeekDetail defines the values for the sum of the time periods for a variable.
	/// </remarks>

	public class StoreBasisWeekDetail : PlanBasisWeekDetailCube
	{
		//=======
		// FIELDS
		//=======

		private StoreBasisDetail _storeBasisDetail;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StoreBasisWeekDetail, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StoreBasisWeekDetail is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StoreBasisWeekDetail is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this StoreBasisWeekDetail is a part of.
		/// </param>

		public StoreBasisWeekDetail(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			int aCubePriority)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Store, aCubePriority)
		{
			CubeDefinition cubeDef;

			cubeDef = new CubeDefinition(
				new DimensionDefinition(eProfileType.Version, MasterVersionProfileList.Count),
				new DimensionDefinition(eProfileType.HierarchyNode, MasterNodeProfileList.Count),
				new DimensionDefinition(eProfileType.Basis, PlanCubeGroup.GetMasterProfileList(eProfileType.Basis).Count),
				new DimensionDefinition(eProfileType.BasisDetail, PlanCubeGroup.OpenParms.GetMaximumBasisDetailCount()),
				new DimensionDefinition(eProfileType.BasisHierarchyNode, 1),
				new DimensionDefinition(eProfileType.BasisVersion, 1),
				new DimensionDefinition(eProfileType.Week, PlanCubeGroup.GetMasterProfileList(eProfileType.Week).Count),
				new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreDetailCube | QuantityLevelFlagValues.HighLevel))),
				new DimensionDefinition(eProfileType.Variable, MasterVariableProfileList.Count),
				new DimensionDefinition(eProfileType.Store, PlanCubeGroup.GetFilteredProfileList(eProfileType.Store).Count));

			_storeBasisDetail = (StoreBasisDetail)aPlanCubeGroup.GetCube(eCubeType.StoreBasisDetail);

			if (_storeBasisDetail == null)
			{
				_storeBasisDetail = new StoreBasisDetail(SAB, Transaction, PlanCubeGroup, cubeDef, aPlanCubeGroup.OpenParms.SimilarStores);
				aTransaction.PlanComputations.PlanCubeInitialization.StoreBasisDetail(_storeBasisDetail, aPlanCubeGroup.OpenParms.GetDisplayType(SAB.ApplicationServerSession));
				aPlanCubeGroup.SetCube(eCubeType.StoreBasisDetail, _storeBasisDetail);
			}
			else
			{
				_storeBasisDetail.ExpandDimensionSize(cubeDef);
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		public override eCubeType CubeType
		{
			get
			{
				return eCubeType.StoreBasisWeekDetail;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that returns the eCubeType of the Chain Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Chain Detail cube.
		/// </returns>

		override public eCubeType GetChainDetailCubeType()
		{
			return eCubeType.ChainBasisWeekDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Low-Level Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Detail cube.
		/// </returns>

		override public eCubeType GetLowLevelDetailCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns the eCubeType of the Low-Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Total cube.
		/// </returns>

		override public eCubeType GetLowLevelTotalCubeType()
		{
			return eCubeType.StoreBasisLowLevelTotalWeekDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Detail cube.
		/// </returns>

		override public eCubeType GetStoreDetailCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Returns the eCubeType of the Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Group Total cube.
		/// </returns>

		override public eCubeType GetGroupTotalCubeType()
		{
			return eCubeType.StoreBasisGroupTotalWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		override public eCubeType GetStoreTotalCubeType()
		{
			return eCubeType.StoreBasisStoreTotalWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		override public eCubeType GetDateTotalCubeType()
		{
			return eCubeType.StoreBasisDateTotal;
		}

		//Begin Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Abstract method that returns the eCubeType of the Plan cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Plan cube.
		/// </returns>

		override public eCubeType GetPlanCubeType()
		{
			return eCubeType.StorePlanWeekDetail;
		}

		//End Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Returns the eCubeType of the Basis cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Basis cube.
		/// </returns>

		override public eCubeType GetBasisCubeType()
		{
			return eCubeType.StoreBasisWeekDetail;
		}

		public override void Clear()
		{
			try
			{
				_storeBasisDetail.Clear();
				base.Clear ();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the store status for the Cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to process.
		/// </param>
		/// <returns>
		/// The eStoreStatus value that describes the store status of the Cell.
		/// </returns>

		override public eStoreStatus GetStoreStatus(PlanCellReference aPlanCellRef)
		{
			try
			{
				return aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is eligible.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that point to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is eligible.
		/// </returns>

		override public bool isStoreIneligible(PlanCellReference aPlanCellRef)
		{
			VariableProfile varProf;
			int nodeRID;

			try
			{
				//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				//if (aPlanCellRef.isCellActual)
				//{
				//    return false;
				//}
				//else
				//{
				//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				varProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
				nodeRID = GetHierarchyNodeProfile(aPlanCellRef).Key;

				switch (varProf.EligibilityType)
				{
					case eEligibilityType.Sales:
						return !CubeGroup.Transaction.GetStoreEligibilityForSales(nodeRID, aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]);
					case eEligibilityType.Stock:
						return !CubeGroup.Transaction.GetStoreEligibilityForStock(nodeRID, aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]);
					case eEligibilityType.Either:
						return !CubeGroup.Transaction.GetStoreEligibilityForSales(nodeRID, aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]) &&
							!CubeGroup.Transaction.GetStoreEligibilityForStock(nodeRID, aPlanCellRef[eProfileType.Store], aPlanCellRef[eProfileType.Week]);
					default:
						return false;
				}
				//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
				//}
				//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is closed.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is closed.
		/// </returns>

		override public bool isStoreClosed(PlanCellReference aPlanCellRef)
		{
			return false;
		}

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database.
		/// </summary>
		/// <param name="aVarProf">
		/// The VariableProfile containing the variable to inspect
		/// </param>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		override public bool isDatabaseVariable(ComputationVariableProfile aVarProf, ComputationCellReference aCompCellRef)
		{
			try
			{
				return aVarProf.isDatabaseVariable(eVariableCategory.Store, ((PlanCellReference)aCompCellRef).GetVersionProfile().Key, eCalendarDateType.Week);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads the PlanCubeCell from the database.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to read.
		/// </param>

		override public void ReadCell(PlanCellReference aPlanCellRef)
		{
		}

		/// <summary>
		/// Returns true if any cell for the given ProfileList of PlanProfiles has changed.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// A ProfileList of PlanProfiles to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		override public bool hasAnyPlanChanged(ProfileList aPlanProfileList)
		{
			try
			{
				foreach (PlanProfile planProf in aPlanProfileList)
				{
					if (hasPlanChanged(planProf))
					{
						return true;
					}
				}
			
				return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns true if any cell for the given PlanProfile has changed.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		override public bool hasPlanChanged(PlanProfile aPlanProfile)
		{
			return false;
		}

		/// <summary>
		/// Returns a string describing the given PlanCellReference
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A string describing the PlanCellReference.
		/// </returns>

		override public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			PlanCellReference planCellRef;
			HierarchyNodeProfile nodeProf;
			VersionProfile versProf;
			WeekProfile weekProf;
			QuantityVariableProfile qtyVarProf;
			VariableProfile varProf;
			StoreProfile storeProf;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;
				nodeProf = GetHierarchyNodeProfile(planCellRef);
				versProf = GetVersionProfile(planCellRef);
				weekProf = _SAB.ApplicationServerSession.Calendar.GetWeek(planCellRef[eProfileType.Week]);
				qtyVarProf = (QuantityVariableProfile)Transaction.PlanComputations.PlanQuantityVariables.QuantityVariableProfileList.FindKey(planCellRef[eProfileType.QuantityVariable]);
				varProf = (VariableProfile)Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(planCellRef[eProfileType.Variable]);
				storeProf = (StoreProfile)PlanCubeGroup.GetMasterProfileList(eProfileType.Store).FindKey(planCellRef[eProfileType.Store]);

				return "Store Basis Week Detail" +
					", Node \"" + nodeProf.Text + "\"" +
					", Version \"" + versProf.Description + "\"" +
					", Basis " + planCellRef[eProfileType.Basis] +
					", Week \"" + weekProf.Text() + "\"" +
					", Quantity \"" + qtyVarProf.VariableName + "\"" +
					", Variable \"" + varProf.VariableName + "\"" +
					", Store \"" + storeProf.Text + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// A ProfileList of PlanProfiles to read.
		/// </param>
		/// <param name="aSimilarStores">
		/// Indicates whether to use Similar Store calculations.
		/// </param>

		public void ReadAndLoadCube(ProfileList aPlanProfileList, bool aSimilarStores)
		{
			try
			{
				foreach (PlanProfile planProf in aPlanProfileList)
				{
					ReadAndLoadCube(planProf, aSimilarStores, 0);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// A ProfileList of PlanProfiles to read.
		/// </param>
		/// <param name="aSimilarStores">
		/// Indicates whether to use Similar Store calculations.
		/// </param>
		/// <param name="alignToPlanWeek">
		/// The week to align the basis to.
		/// </param>

		public void ReadAndLoadCube(ProfileList aPlanProfileList, bool aSimilarStores, int alignToPlanWeek)
		{
			try
			{
				foreach (PlanProfile planProf in aPlanProfileList)
				{
					ReadAndLoadCube(planProf, aSimilarStores, alignToPlanWeek);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to read.
		/// </param>
		/// <param name="aSimilarStores">
		/// Indicates whether to use Similar Store calculations.
		/// </param>

		public void ReadAndLoadCube(PlanProfile aPlanProfile, bool aSimilarStores)
		{
			try
			{
				ReadAndLoadCube(aPlanProfile, aSimilarStores, 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to read.
		/// </param>
		/// <param name="aSimilarStores">
		/// Indicates whether to use Similar Store calculations.
		/// </param>
		/// <param name="alignToPlanWeek">
		/// The week to align the basis to.
		/// </param>

		public void ReadAndLoadCube(PlanProfile aPlanProfile, bool aSimilarStores, int alignToPlanWeek)
		{
			try
			{
				_storeBasisDetail.ReadAndLoadCube(
					aPlanProfile.VersionProfile,
					aPlanProfile.NodeProfile.Key,
					PlanCubeGroup.OpenParms.GetBasisProfileList(PlanCubeGroup, aPlanProfile.NodeProfile.Key, aPlanProfile.VersionProfile.Key),
					alignToPlanWeek);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
