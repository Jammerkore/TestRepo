using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Business
{
	/// <summary>
	/// The FormulasAndSpreads class is where the Formula and Spread routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the Formula and Spread routines are defined.  A formula or spread is defined in a FormulaSpreadProfile, which contains
	/// an Id, a name, and a FormulaSpreadDelegate that points to a method within this class that executes the formula or spread.  This method will contain
	/// all the logic to calculate or spread values as required.
	/// </remarks>

	public class AllocationSummaryFormulasAndSpreads : AssortmentFormulasAndSpreads
	{
		//=======
		// FIELDS
		//=======

		protected int _seq;

		//-------------------------------------------
		#region Spreads
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		protected FormulaProfile _init_Null;
		protected FormulaProfile _init_HeaderDetailUnits;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Generic Total Inits
		//-------------------------------------------

		protected FormulaProfile _init_SumDetail;
		protected FormulaProfile _init_AvgDetail;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Low-level Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparative Inits
		//-------------------------------------------

		protected FormulaProfile _init_PctToSet;
		protected FormulaProfile _init_PctToAll;
		protected FormulaProfile _init_TotalPct;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of FormulasAndSpreads.
		/// </summary>

		public AllocationSummaryFormulasAndSpreads(AllocationSummaryComputations aComputations)
			: base(aComputations)
		{
			_seq = 1;

			//-------------------------------------------
			#region Spreads
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Inits
			//-------------------------------------------

			_init_Null = new clsInit_Null(aComputations, _seq++);
			_init_HeaderDetailUnits = new clsInit_HeaderDetailUnits(aComputations, _seq++);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Generic Total Inits
			//-------------------------------------------

			_init_SumDetail = new clsInit_SumDetail(aComputations, _seq++);
			_init_AvgDetail = new clsInit_AvgDetail(aComputations, _seq++);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Time Total Inits
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Set/Store Inits
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Period Inits
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Low-level Inits
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Comparative Inits
			//-------------------------------------------

			_init_PctToSet = new clsInit_PctToSet(aComputations, _seq++);
			_init_PctToAll = new clsInit_PctToAll(aComputations, _seq++);
			_init_TotalPct = new clsInit_TotalPct(aComputations, _seq++);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Change Formulas
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Time Total Change Formulas
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

		}

		//===========
		// PROPERTIES
		//===========

		//-------------------------------------------
		#region Spreads
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		public FormulaProfile Init_Null { get { return _init_Null; } }
		public FormulaProfile Init_HeaderDetailUnits { get { return _init_HeaderDetailUnits; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Generic Total Inits
		//-------------------------------------------

		public FormulaProfile Init_SumDetail { get { return _init_SumDetail; } }
		public FormulaProfile Init_AvgDetail { get { return _init_AvgDetail; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Low-level Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparative Inits
		//-------------------------------------------

		public FormulaProfile Init_PctToSet { get { return _init_PctToSet; } }
		public FormulaProfile Init_PctToAll { get { return _init_PctToAll; } }
		public FormulaProfile Init_TotalPct { get { return _init_TotalPct; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========

		//-------------------------------------------
		#region Base Classes
		//-------------------------------------------

		abstract private class AssortmentFormulaProfile : FormulaProfile
		{
			//=======
			// FIELDS
			//=======

			AllocationSummaryComputations _computations;

			//=============
			// CONSTRUCTORS
			//=============

			public AssortmentFormulaProfile(AllocationSummaryComputations aComputations, int aKey, string aName)
				: base(aKey, aName)
			{
				_computations = aComputations;
			}

			//===========
			// PROPERTIES
			//===========

			protected AllocationSummaryDetailVariables AssortmentDetailVariables
			{
				get
				{
					return (AllocationSummaryDetailVariables)_computations.AssortmentDetailVariables;
				}
			}

			protected AllocationSummarySummaryVariables AssortmentSummaryVariables
			{
				get
				{
					return (AllocationSummarySummaryVariables)_computations.AssortmentSummaryVariables;
				}
			}

			protected AllocationSummaryQuantityVariables AssortmentQuantityVariables
			{
				get
				{
					return (AllocationSummaryQuantityVariables)_computations.AssortmentQuantityVariables;
				}
			}

			protected AssortmentToolBox ToolBox
			{
				get
				{
					return (AssortmentToolBox)_computations.ToolBox;
				}
			}

			//========
			// METHODS
			//========

			//Begin Track #5752 - JScott - Calculation Time
			//override public eComputationFormulaReturnType Execute(ComputationScheduleEntry aCompSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			//{
			//    try
			//    {
			//        return Execute((AssortmentScheduleFormulaEntry)aCompSchdEntry, aGetCellMode, aSetCellMode);
			//    }
			//    catch (Exception exc)
			//    {
			//        string message = exc.ToString();
			//        throw;
			//    }
			//}
			override public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller)
			{
				AssortmentScheduleFormulaEntry schedEntry;

				try
				{
					return Execute((AssortmentScheduleFormulaEntry)aSchdEntry, aGetCellMode, aSetCellMode);
				}
				catch (CellPendingException exc)
				{
					schedEntry = (AssortmentScheduleFormulaEntry)aSchdEntry;
					schedEntry.LastPendingCell = exc.ComputationCellReference;

					return eComputationFormulaReturnType.Pending;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			//End Track #5752 - JScott - Calculation Time

			abstract public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode);
		}

		abstract private class AssortmentSpreadProfile : SpreadProfile
		{
			//=======
			// FIELDS
			//=======

			AllocationSummaryComputations _computations;

			//=============
			// CONSTRUCTORS
			//=============

			public AssortmentSpreadProfile(AllocationSummaryComputations aComputations, int aKey, string aName)
				: base(aKey, aName)
			{
				_computations = aComputations;
			}

			//===========
			// PROPERTIES
			//===========

			protected AllocationSummaryDetailVariables AssortmentDetailVariables
			{
				get
				{
					return (AllocationSummaryDetailVariables)_computations.AssortmentDetailVariables;
				}
			}

			protected AllocationSummarySummaryVariables AssortmentSummaryVariables
			{
				get
				{
					return (AllocationSummarySummaryVariables)_computations.AssortmentSummaryVariables;
				}
			}

			protected AllocationSummaryQuantityVariables AssortmentQuantityVariables
			{
				get
				{
					return (AllocationSummaryQuantityVariables)_computations.AssortmentQuantityVariables;
				}
			}

			protected AssortmentToolBox ToolBox
			{
				get
				{
					return (AssortmentToolBox)_computations.ToolBox;
				}
			}

			//========
			// METHODS
			//========

			//Begin Track #5752 - JScott - Calculation Time
			//override public eComputationFormulaReturnType Execute(ComputationScheduleEntry aCompSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			//{
			//    try
			//    {
			//        return Execute((AssortmentScheduleSpreadEntry)aCompSchdEntry, aGetCellMode, aSetCellMode);
			//    }
			//    catch (Exception exc)
			//    {
			//        string message = exc.ToString();
			//        throw;
			//    }
			//}
			override public eComputationFormulaReturnType ExecuteCalc(ComputationScheduleEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, string aCaller)
			{
				AssortmentScheduleSpreadEntry schedEntry;

				try
				{
					return Execute((AssortmentScheduleSpreadEntry)aSchdEntry, aGetCellMode, aSetCellMode);
				}
				catch (CellPendingException exc)
				{
					schedEntry = (AssortmentScheduleSpreadEntry)aSchdEntry;
					schedEntry.LastPendingCell = exc.ComputationCellReference;

					return eComputationFormulaReturnType.Pending;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			//End Track #5752 - JScott - Calculation Time

			override public ArrayList GetSpreadToCellReferenceList(ComputationCellReference aCompCellRef)
			{
				try
				{
					return GetSpreadToCellReferenceList((AssortmentCellReference)aCompCellRef);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			abstract public eComputationFormulaReturnType Execute(AssortmentScheduleSpreadEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode);

			abstract public ArrayList GetSpreadToCellReferenceList(AssortmentCellReference aAssrtCellRef);
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Base/Abstract Formulas
		//-------------------------------------------

		private class clsFormula_Sum : AssortmentFormulaProfile
		{
			public clsFormula_Sum(AllocationSummaryComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, ToolBox.SumDetailComponents(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsFormula_Average : AssortmentFormulaProfile
		{
			public clsFormula_Average(AllocationSummaryComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					//ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, ToolBox.CalculateAverage(aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, true));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsFormula_AverageValue : AssortmentFormulaProfile
		{
			public clsFormula_AverageValue(AllocationSummaryComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					//ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, ToolBox.CalculateAverage(aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, QuantityVariables.Value, true));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Abstract Spreads
		//-------------------------------------------

		abstract private class clsSpread_Base : AssortmentSpreadProfile
		{
			public clsSpread_Base(AllocationSummaryComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return false;
				}
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleSpreadEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double spreadValue;

				try
				{
					spreadValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

					ToolBox.ExecutePctContributionSpread(
						aAssrtSchdEntry,
						aSetCellMode,
						aAssrtSchdEntry.SpreadCellRefList,
						aAssrtSchdEntry.AssortmentCellRef.GetFormatTypeVariableProfile().NumDecimals,
						spreadValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Spreads
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		private class clsInit_Null : AssortmentFormulaProfile
		{
			public clsInit_Null(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "Null Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					ToolBox.SetCellNull(aAssrtSchdEntry.AssortmentCellRef);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_HeaderDetailUnits : AssortmentFormulaProfile
		{
			public clsInit_HeaderDetailUnits(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "HeaderDetailUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				int packRID;

				try
				{
					if (aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.useHeaderAllocation(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]))
					{
						packRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];

						// BEGIN TT#779-MD - Stodd - New placeholder does not hold total units
						if (packRID == int.MaxValue)
						{
							//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
							//    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedColorUnits(
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor],
							//        aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
							newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
								aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedColorUnits(
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor],
									aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
						}
						else
						{
							//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
							//    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedUnits(
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack],
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor],
							//        aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
							//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
							newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
								aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedUnits(
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack],
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor],
									aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
						}
						// END TT#779-MD - Stodd - New placeholder does not hold total units
						ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
					}

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Generic Total Inits
		//-------------------------------------------

		private class clsInit_SumDetail : clsFormula_Sum
		{
			public clsInit_SumDetail(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "SumDetail Init")
			{
			}
		}

		private class clsInit_AvgDetail : clsFormula_AverageValue
		{
			public clsInit_AvgDetail(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "AvgDetail Init")
			{
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Low-level Inits
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparative Inits
		//-------------------------------------------

		private class clsInit_PctToSet : AssortmentFormulaProfile
		{
			public clsInit_PctToSet(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "PctToSet Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double setValue;

				try
				{
					setValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetComponentGroupLevelCubeType(), AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) / setValue * 100;
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_PctToAll : AssortmentFormulaProfile
		{
			public clsInit_PctToAll(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "PctToAll Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double allValue;

				try
				{
					allValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetComponentTotalCubeType(), AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) / allValue * 100;
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_TotalPct : AssortmentFormulaProfile
		{
			public clsInit_TotalPct(AllocationSummaryComputations aComputations, int aKey)
				: base(aComputations, aKey, "TotalPct Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double totValue;

				try
				{
					totValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) / totValue * 100;
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------
	}
}
