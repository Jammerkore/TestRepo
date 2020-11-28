using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentSummaryGroupLevel : AssortmentSummaryCube
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _headerHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentCube.
		/// </param>

		public AssortmentSummaryGroupLevel(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, PlanCubeAttributesFlagValues.GroupTotal, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			_headerHash = new Hashtable();
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		override public eCubeType CubeType
		{
			get
			{
				return eCubeType.AssortmentSummaryGroupLevel;
			}
		}

		/// <summary>
		/// Abstract property returns the eProfileType for the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public eProfileType VariableProfileType
		{
			get
			{
				return eProfileType.AssortmentSummaryVariable;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that initializes the Cube.
		/// </summary>

		override public void InitializeCube()
		{
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Level Total cube.
		/// </returns>

		override public eCubeType GetComponentGroupLevelCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Total cube.
		/// </returns>

		override public eCubeType GetComponentTotalCubeType()
		{
			return eCubeType.AssortmentSummaryTotal;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Placeholder cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentPlaceholderCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentHeaderCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the summary cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSummaryCubeType()
		{
			return eCubeType.AssortmentSummaryGroupLevel;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSubTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		override public eCubeType GetTotalCubeType()
		{
            // Begin TT#1119 - md -stodd - summary calculations wrong 
            if (AssortmentCubeGroup.AssortmentType == eAssortmentType.PreReceipt)
            {
                return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, 0);
            }
            else
            {
                return new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, 0);
            }
            // End TT#1119 - md -stodd - summary calculations wrong 
		}

		/// <summary>
		/// Abstract method that returns the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public ComputationVariableProfile GetVariableProfile(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (ComputationVariableProfile)MasterAssortmentSummaryVariableProfileList.FindKey(aCompCellRef[eProfileType.AssortmentSummaryVariable]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a Cell is read.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A AssortmentCellReference object that identifies the AssortmentCubeCell to read.
		/// </param>

		override public void ReadCell(AssortmentCellReference aAssrtCellRef)
		{
		}

		// BEGIN TT#2 - stodd
		public void LoadCube(DataTable aSummaryTable)
		{
			LoadCube(aSummaryTable, false);
		}
		// END TT#2 - stodd

		public void LoadCube(DataTable aSummaryTable, bool reloadFromDB)
		{
			ProfileList varProfList;
			AssortmentCellReference assrtCellRef;
			int strGrpLvlRID;
			int strGrdRID;
            double aValue;	// TT#3815 - Set totals in group allocation summary area of matrix grid are missing after running Group Allocation Method - 

			try
			{
				if (aSummaryTable.Rows.Count > 0)
				{
					varProfList = AssortmentCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList;

					assrtCellRef = (AssortmentCellReference)CreateCellReference();
					assrtCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

					foreach (System.Data.DataRow dataRow in aSummaryTable.Rows)
					{
						strGrpLvlRID = Convert.ToInt32(dataRow["GROUP_LEVEL_RID"], CultureInfo.CurrentUICulture);
						strGrdRID = Convert.ToInt32(dataRow["GRADE_RID"], CultureInfo.CurrentUICulture);

						if (strGrpLvlRID != -1 && strGrdRID == -1)
						{
							assrtCellRef[eProfileType.StoreGroupLevel] = Convert.ToInt32(dataRow["GROUP_LEVEL_RID"], CultureInfo.CurrentUICulture);

							foreach (AssortmentSummaryVariableProfile varProf in varProfList)
							{
								if (varProf.DatabaseColumnName != null)
								{
									assrtCellRef[eProfileType.AssortmentSummaryVariable] = varProf.Key;

									if (!assrtCellRef.isCellLoadedFromDB || reloadFromDB)
									{
										// Begin TT#3815 - Set totals in group allocation summary area of matrix grid are missing after running Group Allocation Method - 
                                        aValue = Convert.ToDouble(dataRow[varProf.DatabaseColumnName]);
                                        assrtCellRef.SetLoadCellValue(aValue, false);
                                        //assrtCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture), false);
										// End TT#3815 - Set totals in group allocation summary area of matrix grid are missing after running Group Allocation Method - 
										assrtCellRef.isCellLoadedFromDB = true;
									}
								}
							}
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
			AssortmentCellReference AssortCellRef;
			//HierarchyNodeProfile nodeProf;
			//VersionProfile versProf;
			AssortmentSummaryVariableProfile summaryVarProf;
			QuantityVariableProfile quantityVarProf;

			try
			{
				AssortCellRef = (AssortmentCellReference)aCompCellRef;
				//nodeProf = GetHierarchyNodeProfile(planCellRef);
				//versProf = GetVersionProfile(planCellRef);
				summaryVarProf = (AssortmentSummaryVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentSummaryVariable]);
				quantityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

				return "Assortment Summary Group Level" +
					", Store Group Level " + AssortCellRef[eProfileType.StoreGroupLevel] +
					", Summary \"" + summaryVarProf.VariableName + "\"" +
					", Quantity Variable \"" + quantityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}