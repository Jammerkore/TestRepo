using System;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the StorePlanDateTotal.
	/// </summary>
	/// <remarks>
	/// The StorePlanDateTotal defines the values for the sum of the time periods for a variable.
	/// </remarks>

	public class StorePlanDateTotal : PlanPlanDateTotalCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StorePlanDateTotal, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StorePlanDateTotal is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StorePlanDateTotal is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this StorePlanDateTotal is a part of.
		/// </param>

		public StorePlanDateTotal(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			int aCubePriority,
			bool aReadOnly,
			bool aCheckNodeSecurity)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, PlanCubeAttributesFlagValues.Store, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
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
				return eCubeType.StorePlanDateTotal;
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
			return eCubeType.ChainPlanDateTotal;
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
		/// Returns the eCubeType of the week Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the week Detail cube.
		/// </returns>

		override public eCubeType GetWeekDetailCubeType()
		{
			return eCubeType.StorePlanWeekDetail;
		}

		/// <summary>
		/// Returns the eCubeType of the Low-Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Total cube.
		/// </returns>

		override public eCubeType GetLowLevelTotalCubeType()
		{
			return eCubeType.StorePlanLowLevelTotalDateTotal;
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
			return eCubeType.StorePlanGroupTotalDateTotal;
		}

		/// <summary>
		/// Returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		override public eCubeType GetStoreTotalCubeType()
		{
			return eCubeType.StorePlanStoreTotalDateTotal;
		}

		/// <summary>
		/// Returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		override public eCubeType GetDateTotalCubeType()
		{
			return eCubeType.None;
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
			return eCubeType.StorePlanDateTotal;
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
			return eCubeType.StoreBasisDateTotal;
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
			return eStoreStatus.None;
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
			try
			{
				return aPlanCellRef.GetComponentDetailStoreIneligible();
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
			try
			{
				return aPlanCellRef.GetComponentDetailStoreClosed();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
			return false;
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
			QuantityVariableProfile qtyVarProf;
			TimeTotalVariableProfile timeTotVarProf;
			StoreProfile storeProf;

			try
			{
				planCellRef = (PlanCellReference)aCompCellRef;
				nodeProf = GetHierarchyNodeProfile(planCellRef);
				versProf = GetVersionProfile(planCellRef);
				qtyVarProf = (QuantityVariableProfile)Transaction.PlanComputations.PlanQuantityVariables.QuantityVariableProfileList.FindKey(planCellRef[eProfileType.QuantityVariable]);
				timeTotVarProf = (TimeTotalVariableProfile)Transaction.PlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList.FindKey(planCellRef[eProfileType.TimeTotalVariable]);
				storeProf = (StoreProfile)PlanCubeGroup.GetMasterProfileList(eProfileType.Store).FindKey(planCellRef[eProfileType.Store]);

				return "Store Plan Date Total" +
					", Node \"" + nodeProf.Text + "\"" +
					", Version \"" + versProf.Description + "\"" +
					", Quantity \"" + qtyVarProf.VariableName + "\"" +
					", Total Variable \"" + timeTotVarProf.VariableName + "\"" +
					", Store \"" + storeProf.Text + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        public void ExtractCube(ExtractOptions aExtractOptions)
        {
            PlanCellReference planCellRef;
            Dictionary<TimeTotalVariableProfile, double> valueCol;
            Dictionary<TimeTotalVariableProfile, string> stringCol;
            PlanWaferCell waferCell;
            string attributeSet = string.Empty;
            int writeCount;

            try
            {
                if (!PlanCubeGroup.ROExtractEnabled)
                {
                    return;
                }

                string timePeriod = PlanCubeGroup.OpenParms.DateRangeProfile.DisplayDate;

                PlanCubeGroup.ROExtractData.OpenUpdateConnection();

                try
                {
                    PlanCubeGroup.ROExtractData.Variable_Init();
                    writeCount = 0;

                    valueCol = new Dictionary<TimeTotalVariableProfile, double>();
                    stringCol = new Dictionary<TimeTotalVariableProfile, string>();

                    planCellRef = (PlanCellReference)CreateCellReference();
                    planCellRef[eProfileType.Version] = PlanCubeGroup.OpenParms.StoreHLPlanProfile.VersionProfile.Key;
                    planCellRef[eProfileType.HierarchyNode] = PlanCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.Key;
                    planCellRef[eProfileType.QuantityVariable] = CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                    //foreach (StoreProfile storeProf in PlanCubeGroup.GetMasterProfileList(eProfileType.Store))
                    foreach (StoreProfile storeProf in PlanCubeGroup.GetFilteredProfileList(eProfileType.Store))
                    {
                        // include attribute and set name if attribute sets are included
                        if (aExtractOptions.AttributeSet)
                        {
                            if (!aExtractOptions.StoreAttributeSets.TryGetValue(storeProf.Key, out attributeSet))
                            {
                                attributeSet = string.Empty;
                            }
                        }

                        valueCol.Clear();
                        stringCol.Clear();

                        planCellRef[eProfileType.Store] = storeProf.Key;

                        foreach (TimeTotalVariableProfile varProf in PlanCubeGroup.TotalVariables.GetStoreTimeTotalVariableList())
                        {
                            // skip variables not selected
                            if (!aExtractOptions.TimeTotalVarProfList.Contains(varProf.Key))
                            {
                                continue;
                            }

                            planCellRef[eProfileType.Variable] = varProf.ParentVariableProfile.Key;
                            planCellRef[eProfileType.TimeTotalVariable] = varProf.Key;

                            if (varProf.FormatType == eValueFormatType.GenericNumeric)
                            {
                                if (planCellRef.CurrentCellValue != 0
                                    || !aExtractOptions.ExcludeZeroValues
                                    || planCellRef.isCellChanged)
                                {
                                    valueCol.Add(varProf, planCellRef.CurrentCellValue);
                                }
                            }
                            else
                            {
                                waferCell = new PlanWaferCell(planCellRef, planCellRef.CurrentCellValue, "1", "1", false);
                                if (!string.IsNullOrEmpty(waferCell.ValueAsString)
                                    || !aExtractOptions.ExcludeZeroValues
                                    || planCellRef.isCellChanged)
                                {
                                    stringCol.Add(varProf, waferCell.ValueAsString);
                                }
                            }
                        }

                        if (valueCol.Count > 0
                            || stringCol.Count > 0)
                        {
                            string nodeID = PlanCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.NodeID;
                            string qualifiedNodeID = PlanCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.QualifiedNodeID;

                            PlanCubeGroup.ROExtractData.Planning_Stores_Total_Insert(
                                string.IsNullOrEmpty(qualifiedNodeID) ? nodeID : qualifiedNodeID,
                                timePeriod,
                                storeProf.StoreId,
                                PlanCubeGroup.OpenParms.StoreHLPlanProfile.VersionProfile.Description,
                                aExtractOptions.Attribute,
                                attributeSet,
                                aExtractOptions.FilterName,
                                valueCol,
                                stringCol);

                            writeCount += (valueCol.Count + stringCol.Count);

                            if (writeCount > MIDConnectionString.CommitLimit)
                            {
                                PlanCubeGroup.ROExtractData.Planning_Stores_Total_Update();
                                PlanCubeGroup.ROExtractData.CommitData();
                                PlanCubeGroup.ROExtractData.Variable_Init();
                                writeCount = 0;
                            }
                        }
                    }

                    if (writeCount > 0)
                    {
                        PlanCubeGroup.ROExtractData.Planning_Stores_Total_Update();
                    }

                    PlanCubeGroup.ROExtractData.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    if (PlanCubeGroup.ROExtractData.ConnectionIsOpen)
                    {
                        PlanCubeGroup.ROExtractData.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration
    }
}
