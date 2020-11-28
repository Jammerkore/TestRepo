using System;
using System.Collections;
using System.Collections.Generic;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ChangeMethods class is where the change routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the change routines are defined.  A change routine is defined as a ChangeMethodDelegate that points to a
	/// method within this class that executes the change rules.  This method will contain all the logic to update appropriate values when a Cell is changed.
	/// </remarks>

	public class AssortmentViewChangeMethods : AssortmentChangeMethods
	{
		//==========
		// CONSTANTS
		//==========

		const int cSubTotalSeq = 5000;

		//=======
		// FIELDS
		//=======

		AssortmentViewComputations _computations;
		protected int _seq;

		//-------------------------------------------
		#region Autototals
		//-------------------------------------------

		protected AssortmentChangeMethodProfile _change_Autototal_InitOnly;
		protected AssortmentChangeMethodProfile _change_Autototal_SpreadLock;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Total Spreads
		//-------------------------------------------

		protected Hashtable _change_Primary_ComponentSubTotal_TotalUnits;
		protected Hashtable _change_Primary_ComponentSubTotal_Difference;
		protected Hashtable _change_Primary_ComponentSubTotal_AvgUnits;
		protected Hashtable _change_Primary_ComponentSubTotal_Index;
        protected Hashtable _change_Primary_ComponentSubTotal_TotalPct;		// TT#3852 - stodd - Total % not recalculating when total units change - 


		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Rules
		//-------------------------------------------

		protected AssortmentChangeMethodProfile _change_Primary_PlaceholderColorDetail_TotalUnits;
		// BEGIN TT#1636 - stodd - index not re-calcing
		protected AssortmentChangeMethodProfile _change_Primary_PlaceholderColorDetail_Index;
		protected AssortmentChangeMethodProfile _change_Primary_PlaceholderColorDetail_AvgUnits;
		// END TT#1636 - stodd - index not re-calcing
        protected AssortmentChangeMethodProfile _change_Primary_HeaderColorDetail_TotalUnits;
		// BEGIN TT#1636 - stodd - index not re-calcing
		protected AssortmentChangeMethodProfile _change_Primary_HeaderColorDetail_Index;
		// END TT#1636 - stodd - index not re-calcing
        protected AssortmentChangeMethodProfile _change_Primary_ComponentGrade_TotalUnits;
		protected AssortmentChangeMethodProfile _change_Primary_ComponentGroupLevel_TotalUnits;
        protected AssortmentChangeMethodProfile _change_Primary_ComponentGroupLevel_AvgUnits;	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
        protected AssortmentChangeMethodProfile _change_Primary_ComponentGroupLevel_Index;		// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

		protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_TotalUnits;
		protected AssortmentChangeMethodProfile _change_Primary_ComponentGradeGroupLevel_Difference;
		protected AssortmentChangeMethodProfile _change_Primary_ComponentGradeGroupLevel_AvgUnits;
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_AvgUnits;
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		// BEGIN TT#2149 - stodd - Difference total does not equal All Store Set
		protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_Difference;
		// END TT#2149 - stodd - Difference total does not equal All Store Set
        protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_Reserve;		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
        protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_BalanceUnits;	// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated		 

		protected AssortmentChangeMethodProfile _change_Primary_ComponentGradeGroupLevel_Index;
		protected AssortmentChangeMethodProfile _change_Primary_SummaryGradeGroupLevel_AvgUnits;
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		protected AssortmentChangeMethodProfile _change_Primary_SummaryTotal_AvgUnits;
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		protected AssortmentChangeMethodProfile _change_Primary_Total_Spread;
		// END TT#2148 - stodd - Assortment totals do not include header values
        // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
        protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_UnitCost;
        protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_UnitRetail; 
        // End TT#1498-MD
		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparatives
		//-------------------------------------------

		//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		//protected AssortmentChangeMethodProfile _change_Primary_TotalPct;
		protected AssortmentChangeMethodProfile _change_Primary_Detail_TotalPct;
		protected AssortmentChangeMethodProfile _change_Primary_Total_TotalPct;
		//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		protected AssortmentChangeMethodProfile _change_Primary_PctToSet;
		protected AssortmentChangeMethodProfile _change_Primary_PctToAll;
		//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_Balance;
		//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_Total;
		protected AssortmentChangeMethodProfile _change_Primary_TotalTotal_TotalPct;
		// END TT#2148 - stodd - Assortment totals do not include header values
        protected AssortmentChangeMethodProfile _change_Primary_ComponentTotal_TotalReserve;	// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 


		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of ChangeMethods.
		/// </summary>

		public AssortmentViewChangeMethods(AssortmentViewComputations aComputations)
			: base(aComputations)
		{
			_computations = aComputations;
			_seq = 1;

			//-------------------------------------------
			#region Autototals
			//-------------------------------------------

			_change_Autototal_InitOnly = new clsChange_Autototal_InitOnly(aComputations, _seq++);
			_change_Autototal_SpreadLock = new clsChange_Autototal_SpreadLock(aComputations, _seq++);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Total Spreads
			//-------------------------------------------

			_change_Primary_ComponentSubTotal_TotalUnits = new Hashtable();
			_change_Primary_ComponentSubTotal_Difference = new Hashtable();
			_change_Primary_ComponentSubTotal_AvgUnits = new Hashtable();
			_change_Primary_ComponentSubTotal_Index = new Hashtable();

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Change Rules
			//-------------------------------------------

			_change_Primary_PlaceholderColorDetail_TotalUnits = new clsChange_Primary_PlaceholderColorDetail_TotalUnits(aComputations, _seq++);
			// BEGIN TT#1636 - stodd - index not re-calcing
			_change_Primary_PlaceholderColorDetail_Index = new clsChange_Primary_PlaceholderColorDetail_Index(aComputations, _seq++);
			_change_Primary_PlaceholderColorDetail_AvgUnits = new clsChange_Primary_PlaceholderColorDetail_AvgUnits(aComputations, _seq++);
			// END TT#1636 - stodd - index not re-calcing
            _change_Primary_HeaderColorDetail_TotalUnits = new clsChange_Primary_HeaderColorDetail_TotalUnits(aComputations, _seq++);
			// BEGIN TT#1636 - stodd - index not re-calcing
			_change_Primary_HeaderColorDetail_Index = new clsChange_Primary_HeaderColorDetail_Index(aComputations, _seq++);
			// END TT#1636 - stodd - index not re-calcing
			_change_Primary_ComponentGrade_TotalUnits = new clsChange_Primary_ComponentGrade_TotalUnits(aComputations, _seq++);
			_change_Primary_ComponentGroupLevel_TotalUnits = new clsChange_Primary_ComponentGroupLevel_TotalUnits(aComputations, _seq++);
            _change_Primary_ComponentGroupLevel_AvgUnits = new clsChange_Primary_ComponentGroupLevel_AvgUnits(aComputations, _seq++);	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
            _change_Primary_ComponentGroupLevel_Index = new clsChange_Primary_ComponentGroupLevel_Index(aComputations, _seq++);			// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values


			_change_Primary_ComponentTotal_TotalUnits = new clsChange_Primary_ComponentTotal_TotalUnits(aComputations, _seq++);
			_change_Primary_ComponentGradeGroupLevel_Difference = new clsChange_Primary_ComponentGradeGroupLevel_Difference(aComputations, _seq++);
			_change_Primary_ComponentGradeGroupLevel_AvgUnits = new clsChange_Primary_ComponentGradeGroupLevel_AvgUnits(aComputations, _seq++);
			//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
			_change_Primary_ComponentTotal_AvgUnits = new clsChange_Primary_ComponentTotal_AvgUnits(aComputations, _seq++);
			//End TT#1196 - JScott - Average units in the summary section should spread when changed
			_change_Primary_ComponentGradeGroupLevel_Index = new clsChange_Primary_ComponentGradeGroupLevel_Index(aComputations, _seq++);
			// BEGIN TT#2149 - stodd - Difference total does not equal All Store Set
			_change_Primary_ComponentTotal_Difference = new clsChange_Primary_ComponentTotal_Difference(aComputations, _seq++);
			// END TT#2149 - stodd - Difference total does not equal All Store Set

            _change_Primary_ComponentTotal_Reserve = new clsChange_Primary_ComponentTotal_Reserve(aComputations, _seq++);	// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units -
            _change_Primary_ComponentTotal_BalanceUnits = new clsChange_Primary_ComponentTotal_BalanceUnits(aComputations, _seq++);		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated


			_change_Primary_SummaryGradeGroupLevel_AvgUnits = new clsChange_Primary_SummaryGradeGroupLevel_AvgUnits(aComputations, _seq++);
			//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
			_change_Primary_SummaryTotal_AvgUnits = new clsChange_Primary_SummaryTotal_AvgUnits(aComputations, _seq++);
			//End TT#1196 - JScott - Average units in the summary section should spread when changed
			// BEGIN TT#2148 - stodd - Assortment totals do not include header values
			_change_Primary_Total_Spread = new clsChange_Primary_Total_Spread(aComputations, _seq++);
			// END TT#2148 - stodd - Assortment totals do not include header values
            // TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
            _change_Primary_ComponentTotal_UnitCost = new clsChange_Primary_ComponentTotal_UnitCost(aComputations, _seq++);
            _change_Primary_ComponentTotal_UnitRetail = new clsChange_Primary_ComponentTotal_UnitRetail(aComputations, _seq++); 
            // End TT#1498-MD
			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Period Change Rules
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Time Total Change Rules
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Set/Store Change Rules
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Comparatives
			//-------------------------------------------

			//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
			//_change_Primary_TotalPct = new clsChange_Primary_TotalPct(aComputations, _seq++);
			_change_Primary_Detail_TotalPct = new clsChange_Primary_Detail_TotalPct(aComputations, _seq++);
			_change_Primary_Total_TotalPct = new clsChange_Primary_Total_TotalPct(aComputations, _seq++);
			//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
			_change_Primary_PctToSet = new clsChange_Primary_PctToSet(aComputations, _seq++);
			_change_Primary_PctToAll = new clsChange_Primary_PctToAll(aComputations, _seq++);
			//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
			_change_Primary_ComponentTotal_Balance = new clsChange_Primary_ComponentTotal_Balance(aComputations, _seq++);
			//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
			// BEGIN TT#2148 - stodd - Assortment totals do not include header values
			_change_Primary_ComponentTotal_Total = new clsChange_Primary_ComponentTotal_Total(aComputations, _seq++);
			_change_Primary_TotalTotal_TotalPct = new clsChange_Primary_TotalTotal_TotalPct(aComputations, _seq++);
            _change_Primary_ComponentTotal_TotalReserve = new clsChange_Primary_ComponentTotal_TotalReserve(aComputations, _seq++);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 


			// BEGIN TT#2148 - stodd - Assortment totals do not include header values


			//-------------------------------------------
			#endregion
			//-------------------------------------------
		}

		//===========
		// PROPERTIES
		//===========

		//-------------------------------------------
		#region Autototals
		//-------------------------------------------

		public AssortmentChangeMethodProfile Change_Autototal_InitOnly { get { return _change_Autototal_InitOnly; } }
		public AssortmentChangeMethodProfile Change_Autototal_SpreadLock { get { return _change_Autototal_SpreadLock; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Total Spreads
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Rules
		//-------------------------------------------

		public AssortmentChangeMethodProfile Change_Primary_PlaceholderColorDetail_TotalUnits { get { return _change_Primary_PlaceholderColorDetail_TotalUnits; } }
		// BEGIN TT#1636 - stodd - index not re-calcing
		public AssortmentChangeMethodProfile Change_Primary_PlaceholderColorDetail_Index { get { return _change_Primary_PlaceholderColorDetail_Index; } }
		public AssortmentChangeMethodProfile Change_Primary_PlaceholderColorDetail_AvgUnits { get { return _change_Primary_PlaceholderColorDetail_AvgUnits; } }
		// END TT#1636 - stodd - index not re-calcing
        public AssortmentChangeMethodProfile Change_Primary_HeaderColorDetail_TotalUnits { get { return _change_Primary_HeaderColorDetail_TotalUnits; } }
		// BEGIN TT#1636 - stodd - index not re-calcing
		public AssortmentChangeMethodProfile Change_Primary_HeaderColorDetail_Index { get { return _change_Primary_HeaderColorDetail_Index; } }
		// END TT#1636 - stodd - index not re-calcing
        public AssortmentChangeMethodProfile Change_Primary_ComponentGrade_TotalUnits { get { return _change_Primary_ComponentGrade_TotalUnits; } }
		public AssortmentChangeMethodProfile Change_Primary_ComponentGroupLevel_TotalUnits { get { return _change_Primary_ComponentGroupLevel_TotalUnits; } }

        public AssortmentChangeMethodProfile Change_Primary_ComponentGroupLevel_AvgUnits { get { return _change_Primary_ComponentGroupLevel_AvgUnits; } }	// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
        public AssortmentChangeMethodProfile Change_Primary_ComponentGroupLevel_Index { get { return _change_Primary_ComponentGroupLevel_Index; } }			// TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values


		public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_TotalUnits { get { return _change_Primary_ComponentTotal_TotalUnits; } }
		public AssortmentChangeMethodProfile Change_Primary_ComponentGradeGroupLevel_Difference { get { return _change_Primary_ComponentGradeGroupLevel_Difference; } }
		public AssortmentChangeMethodProfile Change_Primary_ComponentGradeGroupLevel_AvgUnits { get { return _change_Primary_ComponentGradeGroupLevel_AvgUnits; } }
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_AvgUnits { get { return _change_Primary_ComponentTotal_AvgUnits; } }
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		// BEGIN TT#2149 - stodd - Difference total does not equal All Store Set
		public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_Difference { get { return _change_Primary_ComponentTotal_Difference; } }
		// END TT#2149 - stodd - Difference total does not equal All Store Set

        public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_Reserve { get { return _change_Primary_ComponentTotal_Reserve; } }		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
        public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_BalanceUnits { get { return _change_Primary_ComponentTotal_BalanceUnits; } }		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated


		public AssortmentChangeMethodProfile Change_Primary_ComponentGradeGroupLevel_Index { get { return _change_Primary_ComponentGradeGroupLevel_Index; } }
		public AssortmentChangeMethodProfile Change_Primary_SummaryGradeGroupLevel_AvgUnits { get { return _change_Primary_SummaryGradeGroupLevel_AvgUnits; } }
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		public AssortmentChangeMethodProfile Change_Primary_SummaryTotal_AvgUnits { get { return _change_Primary_SummaryTotal_AvgUnits; } }
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		public AssortmentChangeMethodProfile Change_Primary_Total_Spread { get { return _change_Primary_Total_Spread; } }
		// END TT#2148 - stodd - Assortment totals do not include header values
        // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
        public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_UnitCost { get { return _change_Primary_ComponentTotal_UnitCost; } }
        public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_UnitRetail { get { return _change_Primary_ComponentTotal_UnitRetail; } }  
        // End TT#1498-MD

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparatives
		//-------------------------------------------

		//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		//public AssortmentChangeMethodProfile Change_Primary_TotalPct { get { return _change_Primary_TotalPct; } }
		public AssortmentChangeMethodProfile Change_Primary_Detail_TotalPct { get { return _change_Primary_Detail_TotalPct; } }
		public AssortmentChangeMethodProfile Change_Primary_Total_TotalPct { get { return _change_Primary_Total_TotalPct; } }
		//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		public AssortmentChangeMethodProfile Change_Primary_PctToSet { get { return _change_Primary_PctToSet; } }
		public AssortmentChangeMethodProfile Change_Primary_PctToAll { get { return _change_Primary_PctToAll; } }
		//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_Balance { get { return _change_Primary_ComponentTotal_Balance; } }
		//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_Total { get { return _change_Primary_ComponentTotal_Total; } }
		public AssortmentChangeMethodProfile Change_Primary_TotalTotal_TotalPct { get { return _change_Primary_TotalTotal_TotalPct; } }
		// END TT#2148 - stodd - Assortment totals do not include header values
        public AssortmentChangeMethodProfile Change_Primary_ComponentTotal_TotalReserve { get { return _change_Primary_ComponentTotal_TotalReserve; } }		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 


		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//========
		// METHODS
		//========

		public AssortmentChangeMethodProfile Change_Primary_ComponentSubTotal_TotalUnits(int aLevel)
		{
			AssortmentChangeMethodProfile subTotalMthd;

			try
			{
				subTotalMthd = (AssortmentChangeMethodProfile)_change_Primary_ComponentSubTotal_TotalUnits[cSubTotalSeq + aLevel];

				if (subTotalMthd == null)
				{
					subTotalMthd = new clsChange_Primary_ComponentSubTotal_TotalUnits(_computations, cSubTotalSeq + aLevel, aLevel);
					_change_Primary_ComponentSubTotal_TotalUnits[cSubTotalSeq + aLevel] = subTotalMthd;
				}

				return subTotalMthd;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public AssortmentChangeMethodProfile Change_Primary_ComponentSubTotal_Difference(int aLevel)
		{
			AssortmentChangeMethodProfile subTotalMthd;

			try
			{
				subTotalMthd = (AssortmentChangeMethodProfile)_change_Primary_ComponentSubTotal_Difference[cSubTotalSeq + aLevel];

				if (subTotalMthd == null)
				{
					subTotalMthd = new clsChange_Primary_ComponentSubTotal_Difference(_computations, cSubTotalSeq + aLevel, aLevel);
					_change_Primary_ComponentSubTotal_Difference[cSubTotalSeq + aLevel] = subTotalMthd;
				}

				return subTotalMthd;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public AssortmentChangeMethodProfile Change_Primary_ComponentSubTotal_AvgUnits(int aLevel)
		{
			AssortmentChangeMethodProfile subTotalMthd;

			try
			{
				subTotalMthd = (AssortmentChangeMethodProfile)_change_Primary_ComponentSubTotal_AvgUnits[cSubTotalSeq + aLevel];

				if (subTotalMthd == null)
				{
					subTotalMthd = new clsChange_Primary_ComponentSubTotal_AvgUnits(_computations, cSubTotalSeq + aLevel, aLevel);
					_change_Primary_ComponentSubTotal_AvgUnits[cSubTotalSeq + aLevel] = subTotalMthd;
				}

				return subTotalMthd;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public AssortmentChangeMethodProfile Change_Primary_ComponentSubTotal_Index(int aLevel)
		{
			AssortmentChangeMethodProfile subTotalMthd;

			try
			{
				subTotalMthd = (AssortmentChangeMethodProfile)_change_Primary_ComponentSubTotal_Index[cSubTotalSeq + aLevel];

				if (subTotalMthd == null)
				{
					subTotalMthd = new clsChange_Primary_ComponentSubTotal_Index(_computations, cSubTotalSeq + aLevel, aLevel);
					_change_Primary_ComponentSubTotal_Index[cSubTotalSeq + aLevel] = subTotalMthd;
				}

				return subTotalMthd;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}



		//========
		// CLASSES
		//========

		//-------------------------------------------
		#region Base Classes
		//-------------------------------------------

		abstract public class AssortmentChangeMethodProfile : ChangeMethodProfile
		{
			//=======
			// FIELDS
			//=======

			AssortmentViewComputations _computations;

			//=============
			// CONSTRUCTORS
			//=============

			public AssortmentChangeMethodProfile(AssortmentViewComputations aComputations, int aKey, string aName)
				: base(aKey, aName)
			{
				_computations = aComputations;
			}

			//===========
			// PROPERTIES
			//===========

			protected AssortmentViewDetailVariables AssortmentDetailVariables
			{
				get
				{
					return (AssortmentViewDetailVariables)_computations.AssortmentDetailVariables;
				}
			}

			//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
			protected AssortmentViewTotalVariables AssortmentTotalVariables
			{
				get
				{
					return (AssortmentViewTotalVariables)_computations.AssortmentTotalVariables;
				}
			}

			//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
			protected AssortmentViewSummaryVariables AssortmentSummaryVariables
			{
				get
				{
					return (AssortmentViewSummaryVariables)_computations.AssortmentSummaryVariables;
				}
			}

			protected AssortmentViewQuantityVariables AssortmentQuantityVariables
			{
				get
				{
					return (AssortmentViewQuantityVariables)_computations.AssortmentQuantityVariables;
				}
			}

			protected AssortmentViewFormulasAndSpreads FormulasAndSpreads
			{
				get
				{
					return (AssortmentViewFormulasAndSpreads)_computations.FormulasAndSpreads;
				}
			}

			protected AssortmentViewChangeMethods ChangeMethods
			{
				get
				{
					return (AssortmentViewChangeMethods)_computations.ChangeMethods;
				}
			}

			protected AssortmentToolBox ToolBox
			{
				get
				{
					return (AssortmentToolBox)_computations.ToolBox;
				}
			}

			override public void Execute(ComputationSchedule aCompSchd, ComputationCellReference aCompCellRef)
			{
				try
				{
					Execute(aCompSchd, (AssortmentCellReference)aCompCellRef);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			abstract public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef);
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Abstract Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Autototals
		//-------------------------------------------

		private class clsChange_Autototal_InitOnly : AssortmentChangeMethodProfile
		{
			public clsChange_Autototal_InitOnly(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "AutototalInitOnly Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
                    if (aAssrtCellRef.isCellInitialized)
                    {
                        ToolBox.InsertAutoTotalFormula(aCompSchd, aAssrtCellRef);
                    }
                }
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Autototal_SpreadLock : AssortmentChangeMethodProfile
		{
			public clsChange_Autototal_SpreadLock(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "AutototalSpreadLock Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				ChangeMethodProfile chngProf;

				try
				{
					if (aAssrtCellRef.isCellLocked || aAssrtCellRef.isCellFixed || aAssrtCellRef.isCellCompLocked)
					{
						chngProf = aAssrtCellRef.AssortmentCube.GetPrimaryChangeMethodProfile(aAssrtCellRef);

						if (chngProf != null)
						{
							chngProf.ExecuteChangeMethod(aCompSchd, aAssrtCellRef, this.Name);
						}
					}
                    else
                    {
                        if (aAssrtCellRef.isCellInitialized)
                        {
                            ToolBox.InsertAutoTotalFormula(aCompSchd, aAssrtCellRef);
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

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Total Spreads
		//-------------------------------------------

		private class clsChange_Primary_ComponentSubTotal_TotalUnits : AssortmentChangeMethodProfile
		{
			private int _level;

			public clsChange_Primary_ComponentSubTotal_TotalUnits(AssortmentViewComputations aComputations, int aKey, int alevel)
				: base(aComputations, aKey, "ComponentSubTotal_TotalUnits Change Method")
			{
				_level = alevel;
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
                // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                ComputationCube totCube;
                AssortmentCellReference cellRef;
                AssortmentCellReference assrtCellRef;	// TT#3852 - stodd - Total % not recalculating when total units change

                double newValue;
                // End TT#2
				try
				{
					
                    ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Spread_ComponentSubTotal_TotalUnits(_level));
                    // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                    if (aAssrtCellRef.CurrentCellValue != aAssrtCellRef.PreviousCellValue && aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGradeSubTotal)
                    {
                        totCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetTotalCubeType());
                        cellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtCellRef);
                        newValue = cellRef.CurrentCellValue - (aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue);
                        if (newValue < 0)
                        {
                            newValue = 0;
                        }
                        ToolBox.SetCellValue(eSetCellMode.Entry, cellRef, AssortmentDetailVariables.TotalUnits, newValue);

						// Begin TT#3852 - stodd - Total % not recalculating when total units change
                        // The code below causes the total % for any other headers listed in the matrix
                        if (_level != 0)
                        {
							// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                            AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                            copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                            if (!copyCellRef.isCellUserChanged)
                            {
                                ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                            }

                            //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                            //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                            //newValue += assrtCellRef.CurrentCellValue;
                            //ToolBox.SetCellCompLocked(aAssrtCellRef);
                            //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
							// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        }
						// End TT#3852 - stodd - Total % not recalculating when total units change

                    }

                    
                    // End TT#2
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_ComponentSubTotal_Difference : AssortmentChangeMethodProfile
		{
			private int _level;

			public clsChange_Primary_ComponentSubTotal_Difference(AssortmentViewComputations aComputations, int aKey, int alevel)
				: base(aComputations, aKey, "ComponentSubTotal_Difference Change Method")
			{
				_level = alevel;
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_Difference, AssortmentQuantityVariables.Value);
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentQuantityVariables.Value, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_ComponentSubTotal_AvgUnits : AssortmentChangeMethodProfile
		{
			private int _level;

			public clsChange_Primary_ComponentSubTotal_AvgUnits(AssortmentViewComputations aComputations, int aKey, int alevel)
				: base(aComputations, aKey, "ComponentSubTotal_AvgUnits Change Method")
			{
				_level = alevel;
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				// Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                AssortmentCellReference assrtCellRef;
                double newValue = 0;
				// End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_AvgUnits, AssortmentDetailVariables.TotalUnits);
					// BEGIN TT#1636 - stodd - Index not recalcing
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_Index_AvgUnits, AssortmentDetailVariables.Index);
					// BEGIN TT#1636 - stodd - Index not recalcing
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentSubTotal_TotalUnits(_level));

					// Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    if (_level != 0 || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGrade || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
					// End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_ComponentSubTotal_Index : AssortmentChangeMethodProfile
		{
			private int _level;

			public clsChange_Primary_ComponentSubTotal_Index(AssortmentViewComputations aComputations, int aKey, int alevel)
				: base(aComputations, aKey, "ComponentSubTotal_Index Change Method")
			{
				_level = alevel;
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				// Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                AssortmentCellReference assrtCellRef;
                double newValue;
				// End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
				
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_Index, AssortmentDetailVariables.TotalUnits);
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentSubTotal_TotalUnits(_level));

                    // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    // The code below causes the total % for any other headers listed in the matrix
                    if (_level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGrade || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }
						
                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
                    // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

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
		#region Variable Change Rules
		//-------------------------------------------

		private class clsChange_Primary_PlaceholderColorDetail_TotalUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_PlaceholderColorDetail_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "PlaceholderColorDetail_Primary Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_AvgUnits_From_TotalUnits, AssortmentDetailVariables.AvgUnits);
					// BEGIN TT#1636 - stodd - Index not recalcing
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_Index_From_AvgUnits, AssortmentDetailVariables.Index);
					// END TT#1636 - stodd - Index not recalcing
                    ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_TotalPct_From_TotalUnits, AssortmentDetailVariables.TotalPct);	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                    ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentQuantityVariables.Difference);
                    // End TT#2
                }
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// BEGIN TT#1636 - stodd - index not re-calcing
		private class clsChange_Primary_PlaceholderColorDetail_Index : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_PlaceholderColorDetail_Index(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "PlaceholderColorDetail_Primary Change Method Index")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_Index_From_AvgUnits, AssortmentDetailVariables.Index);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_PlaceholderColorDetail_AvgUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_PlaceholderColorDetail_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "PlaceholderColorDetail_Primary Change Method AvgUnits")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_Index_From_AvgUnits, AssortmentDetailVariables.Index);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#1636 - stodd - index not re-calcing

        private class clsChange_Primary_HeaderColorDetail_TotalUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_HeaderColorDetail_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "HeaderColorDetail_Primary Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					// BEGIN TT#1636 - stodd - Index not recalcing
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_AvgUnits_From_TotalUnits, AssortmentDetailVariables.AvgUnits);
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_Index_From_AvgUnits, AssortmentDetailVariables.Index);
                    ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_TotalPct_From_TotalUnits, AssortmentDetailVariables.TotalPct);	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					//ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentQuantityVariables.Difference);
					// END TT#1636 - stodd - Index not recalcing

                }
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        private class clsChange_Primary_ComponentGrade_TotalUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentGrade_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGrade_TotalUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                ComputationCube totCube;
                AssortmentCellReference assrtCellRef;	// TT#3852 - stodd - Total % not recalculating when total units change
                double newValue;
                // End TT#2
				try
				{
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);
                    // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                    if (aAssrtCellRef.isCellUserChanged && aAssrtCellRef.CurrentCellValue != aAssrtCellRef.PreviousCellValue && aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGrade)
                    {
                        // Begin TT#1186-MD - stodd - dimension not defined error
                        // For Group Allocation (and probably post-receipt assortment, there is no placeholder dimension defined on cell ref
                        int placeholderIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);
                        if (placeholderIndex != -1)
                        {
                            eCubeType cubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGrade);
                            totCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(cubeType);
                            AssortmentCellReference asrtCellRef2 = new AssortmentCellReference((AssortmentComponentPlaceholderGrade)totCube);
                            asrtCellRef2[eProfileType.PlaceholderHeader] = aAssrtCellRef[eProfileType.PlaceholderHeader];
                            asrtCellRef2[eProfileType.AllocationHeader] = int.MaxValue;
                            asrtCellRef2[eProfileType.HeaderPack] = aAssrtCellRef[eProfileType.HeaderPack];
                            asrtCellRef2[eProfileType.HeaderPackColor] = aAssrtCellRef[eProfileType.HeaderPackColor];
                            asrtCellRef2[eProfileType.StoreGroupLevel] = aAssrtCellRef[eProfileType.StoreGroupLevel];
                            asrtCellRef2[eProfileType.StoreGrade] = aAssrtCellRef[eProfileType.StoreGrade];
                            asrtCellRef2[eProfileType.AssortmentDetailVariable] = aAssrtCellRef[eProfileType.AssortmentDetailVariable];
                            asrtCellRef2[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;

                            newValue = asrtCellRef2.CurrentCellValue - (aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue);
                            if (newValue < 0)
                            {
                                newValue = 0;
                            }
                            ToolBox.SetCellValue(eSetCellMode.Entry, asrtCellRef2, AssortmentDetailVariables.TotalUnits, newValue);
                        }
                        // End TT#1186-MD - stodd - dimension not defined error
						// Begin TT#3852 - stodd - Total % not recalculating when total units change
                        // The code below causes the total % for any other headers listed in the matrix
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3852 - stodd - Total % not recalculating when total units change
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
                    // End TT#2
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// BEGIN TT#1636 - stodd - index not recalcing
		private class clsChange_Primary_HeaderColorDetail_Index : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_HeaderColorDetail_Index(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "HeaderColorDetail_Primary Change Method Index")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_Calculate_Index_From_AvgUnits, AssortmentDetailVariables.Index);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#1636 - stodd - index not recalcing


		private class clsChange_Primary_ComponentGroupLevel_TotalUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentGroupLevel_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "SetTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
                // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                ComputationCube totCube;
                AssortmentCellReference cellRef;
                AssortmentCellReference assrtCellRef;	// TT#3852 - stodd - Total % not recalculating when total units change - 
                double newValue;
                // End TT#2
				try
				{
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Spread_ComponentGroupLevel_TotalUnits);
                    // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                    if (aAssrtCellRef.CurrentCellValue != aAssrtCellRef.PreviousCellValue && aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevelSubTotal)
                    {
                        totCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetTotalCubeType());
                        cellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtCellRef);
                        newValue = cellRef.CurrentCellValue - (aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue);
                        if (newValue < 0)
                        {
                            newValue = 0;
                        }
                        ToolBox.SetCellValue(eSetCellMode.Entry, cellRef, AssortmentDetailVariables.TotalUnits, newValue);
                    }
                    // End TT#2

					// Begin TT#3852 - stodd - Total % not recalculating when total units change - 
                    // The code below causes the total % for any other headers listed in the matrix


                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevel || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGroupLevel))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
					// End TT#3852 - stodd - Total % not recalculating when total units change - 

				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
        private class clsChange_Primary_ComponentGroupLevel_AvgUnits : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentGroupLevel_AvgUnits(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentGroupLevel_AvgUnits Change Method")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                AssortmentCellReference assrtCellRef;
                double newValue = 0;
                try
                {
                    ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_AvgUnits, AssortmentDetailVariables.TotalUnits);
                    ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_Index_AvgUnits, AssortmentDetailVariables.Index);
                    // Begin TT#1262-MD - stodd - POST-Reciept: enter average units in SET total grid and nothing changes.
                    //ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);
                    ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGroupLevel_TotalUnits);
                    // End TT#1262-MD - stodd - POST-Reciept: enter average units in SET total grid and nothing changes.

                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevel || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGroupLevel))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }

                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        private class clsChange_Primary_ComponentGroupLevel_Index : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentGroupLevel_Index(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentGroupLevel_Index Change Method")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                AssortmentCellReference assrtCellRef;
                double newValue;

                try
                {
                    ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_Index, AssortmentDetailVariables.TotalUnits);
                    // Begin TT#1263-MD - stodd -  POST-Receipt: enter Index in SET total grid and nothing changes.
                    //ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);
                    ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGroupLevel_TotalUnits);
                    // End TT#1263-MD - stodd -  POST-Receipt: enter Index in SET total grid and nothing changes.

                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGroupLevel || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGroupLevel))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }

                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

		private class clsChange_Primary_ComponentTotal_TotalUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentTotal_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Component Total Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
                // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                ComputationCube totCube;
                AssortmentCellReference cellRef;
                AssortmentCellReference assrtCellRef;		// TT#3852 - stodd - Total % not recalculating when total units change - 
                double newValue;
                // End TT#2
				try
				{
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Spread_ComponentTotal_TotalUnits);
                    // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                    if (aAssrtCellRef.CurrentCellValue != aAssrtCellRef.PreviousCellValue && aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
                    {
                        totCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetTotalCubeType());
                        cellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtCellRef);
                        newValue = cellRef.CurrentCellValue - (aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue);
                        if (newValue < 0)
                        {
                            newValue = 0;
                        }
                        ToolBox.SetCellValue(eSetCellMode.Entry, cellRef, AssortmentTotalVariables.TotalUnits, newValue);
                    }
                    // End TT#2

					// Begin TT#3852 - stodd - Total % not recalculating when total units change - 
                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderTotal || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderTotal))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentTotalVariable] = AssortmentTotalVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;

                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentTotalVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
					// End TT#3852 - stodd - Total % not recalculating when total units change - 

				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// Begin TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
        private class clsChange_Primary_ComponentTotal_TotalReserve : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentTotal_TotalReserve(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "Component Total Change Method - Total Reserve")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                // Begin TT#2 - RMatelic - Assortment Planning - Matrix placeholder quantities not displaying correct values
                //ComputationCube totCube;
                //AssortmentCellReference cellRef;
                //double newValue;
                // End TT#2

                ComputationCube headerCube;
                ComputationCube placeholderCube;
                AssortmentCellReference totCellRef;
                AssortmentCellReference totPlaceholderCellRef;
                AssortmentCellReference totHeaderCellRef;
                AssortmentCellReference placeholderCellRef;
                AssortmentCellReference headerCellRef;

                try
                {
                    headerCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetComponentHeaderCubeType());
                    totHeaderCellRef = (AssortmentCellReference)headerCube.CreateCellReference(aAssrtCellRef);
                    placeholderCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetComponentPlaceholderCubeType());
                    totPlaceholderCellRef = (AssortmentCellReference)placeholderCube.CreateCellReference(aAssrtCellRef);

                    headerCellRef = (AssortmentCellReference)totHeaderCellRef.Copy();
                    headerCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;
                    placeholderCellRef = (AssortmentCellReference)totPlaceholderCellRef.Copy();
                    placeholderCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;

                    double tempSprdVal = aAssrtCellRef.CurrentCellValue - headerCellRef.CurrentCellValue;

                    ToolBox.SetCellValue(eSetCellMode.Entry, placeholderCellRef, tempSprdVal);

                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        private class clsChange_Primary_ComponentTotal_Reserve : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentTotal_Reserve(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "Component Total Change Method - Reserve")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
				try
				{
					int colorIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
					int packIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
					int headerIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
					Index_RID reserveStore = aAssrtCellRef.AssortmentCube.Transaction.ReserveStore;
					if (reserveStore.RID == Include.NoRID)
					{
						//newValue = 0;
					}
                    else if (headerIndex != -1)
                    {
                        ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Spread_ComponentTotal_Reserve);

                        //int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                        //                    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).StyleHnRID;

                        //// COLOR
                        //if (colorIndex != -1 && aAssrtCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                        //{
                        //    int colorRID = aAssrtCellRef[eProfileType.HeaderPackColor];
                        //    aAssrtCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                        //                    aAssrtCellRef[eProfileType.AllocationHeader]).SetStoreQtyAllocated(colorRID, reserveStore.RID, (int)aAssrtCellRef.CurrentCellValue);
                        //}
                        //// PACK
                        //else if (packIndex != -1 && aAssrtCellRef[eProfileType.HeaderPack] != int.MaxValue)
                        //{
                        //    int packRID = aAssrtCellRef[eProfileType.HeaderPack];
                        //    PackHdr packHdr = aAssrtCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtCellRef[eProfileType.AllocationHeader]).GetPackHdr(packRID);
                        //    aAssrtCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtCellRef[eProfileType.AllocationHeader]).SetStoreQtyAllocated(packHdr.PackName, reserveStore.RID, (int)aAssrtCellRef.CurrentCellValue);
                        //    //newValue = newValue * packHdr.PackMultiple;
                        //}
                        //// TOTAL
                        //else
                        //{
                        //    aAssrtCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                        //        aAssrtCellRef[eProfileType.AllocationHeader]).SetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore.RID, (int)aAssrtCellRef.CurrentCellValue);
                        //}
                    }
                    else
                    {
                        ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Spread_ComponentTotal_Reserve);
                        //ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.ReserveUnits, FormulasAndSpreads.Spread_ComponentTotal_Reserve);
                        //(((AssortmentProfile)((AssortmentCubeGroup)aAssrtCellRef.Cube.CubeGroup).DefaultAllocationProfile)).SetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore.RID, (int)aAssrtCellRef.CurrentCellValue);


                    }
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
		// End TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

		// Begin TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated
        private class clsChange_Primary_ComponentTotal_BalanceUnits : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentTotal_BalanceUnits(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "Component Total Change Method - BalanceUnits")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                double newValue = 0;
                try
                {
                    int colorIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
                    int packIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
                    int headerIndex = aAssrtCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);

                    AllocationProfile ap = aAssrtCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtCellRef[eProfileType.AllocationHeader]);
                    newValue = ap.TotalUnitsToAllocate - ap.TotalUnitsAllocated;

                    ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, newValue);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        // End TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated


		private class clsChange_Primary_ComponentGradeGroupLevel_Difference : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentGradeGroupLevel_Difference(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGradeGroupLevel_Difference Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_Difference, AssortmentQuantityVariables.Value);
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentQuantityVariables.Value, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_ComponentGradeGroupLevel_AvgUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentGradeGroupLevel_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGradeGroupLevel_AvgUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				// Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                AssortmentCellReference assrtCellRef;
                double newValue = 0;
				// End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
				
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_AvgUnits, AssortmentDetailVariables.TotalUnits);
					// BEGIN TT#1636 - stodd - recalc Index
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_Index_AvgUnits, AssortmentDetailVariables.Index);
					// END TT#1636 - stodd - recalc Index
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);

                    // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGrade || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
                    // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        private class clsChange_Primary_ComponentGradeGroupLevel_TotalPct : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentGradeGroupLevel_TotalPct(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentGradeGroupLevel_TotalPct Change Method")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                AssortmentCellReference assrtCellRef;
                double newValue = 0;
                // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

                try
                {
                    //ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_AvgUnits, AssortmentDetailVariables.TotalUnits);
                    //// BEGIN TT#1636 - stodd - recalc Index
                    //ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_Index_AvgUnits, AssortmentDetailVariables.Index);
                    //// END TT#1636 - stodd - recalc Index
                    //ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);

                    // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGrade || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
                    // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		private class clsChange_Primary_ComponentTotal_AvgUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentTotal_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_AvgUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				// Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                AssortmentCellReference assrtCellRef;
                double newValue;
				// End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentTotal_TotalUnits_AvgUnits, AssortmentTotalVariables.TotalUnits);
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentTotal_TotalUnits);

                    // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderTotal || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderTotal)
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentTotalVariable] = AssortmentTotalVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentTotalVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
                    // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		private class clsChange_Primary_ComponentGradeGroupLevel_Index : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentGradeGroupLevel_Index(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGradeGroupLevel_Index Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				// Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                AssortmentCellReference assrtCellRef;
                double newValue;
				// End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_Index, AssortmentDetailVariables.TotalUnits);
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);

                    // Begin TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values
                    // The code below causes the total % for any other headers listed in the matrix
                    if (aAssrtCellRef.AssortmentCube.CubeType.Level != 0
                        || (aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentHeaderGrade || aAssrtCellRef.AssortmentCube.CubeType == eCubeType.AssortmentComponentPlaceholderGrade))
                    {
						// Begin TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                        AssortmentCellReference copyCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
                        copyCellRef[eProfileType.AssortmentDetailVariable] = AssortmentDetailVariables.TotalPct.Key;
                        if (!copyCellRef.isCellUserChanged)
                        {
                            ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalPct);
                        }

                        //assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
                        //newValue = aAssrtCellRef.CurrentCellValue - aAssrtCellRef.PreviousCellValue;
                        //newValue += assrtCellRef.CurrentCellValue;
                        //ToolBox.SetCellCompLocked(aAssrtCellRef);
                        //ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
						// End TT#3909 - GA matrix - Balance chaning for a row when NOT adjusting that row - 
                    }
                    // End TT#3857 - stodd - Changing the Average Units for a header does not recalc the other headers Total % values

				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_SummaryGradeGroupLevel_AvgUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_SummaryGradeGroupLevel_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "SummaryGradeGroupLevel_AvgUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				ComputationCube totCube;
				AssortmentCellReference cellRef;

				try
				{
					totCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetTotalCubeType());
					cellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtCellRef);

					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					//ToolBox.InsertFormula(aCompSchd, cellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_SummaryAvgUnits, AssortmentDetailVariables.TotalUnits);
					//ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, FormulasAndSpreads.Spread_ComponentSubTotal_TotalUnits(0));
					ToolBox.SetCellValue(eSetCellMode.Entry, cellRef, AssortmentDetailVariables.AvgUnits, aAssrtCellRef.CurrentCellValue);
					//End TT#1196 - JScott - Average units in the summary section should spread when changed
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		private class clsChange_Primary_SummaryTotal_AvgUnits : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_SummaryTotal_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "SummaryTotal_AvgUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				ComputationCube totCube;
				AssortmentCellReference cellRef;

				try
				{
					totCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetTotalCubeType());
					cellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtCellRef);

					ToolBox.SetCellValue(eSetCellMode.Entry, cellRef, AssortmentTotalVariables.AvgUnits, aAssrtCellRef.CurrentCellValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End TT#1196 - JScott - Average units in the summary section should spread when changed

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsChange_Primary_Total_Spread : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_Total_Spread(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Total_Spread Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{

				ComputationCube headerCube;
				ComputationCube placeholderCube;
				AssortmentCellReference totPlaceholderCellRef;
				AssortmentCellReference totHeaderCellRef;
				AssortmentCellReference placeholderCellRef;
				AssortmentCellReference headerCellRef;

				try
				{
					headerCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetComponentHeaderCubeType());
					totHeaderCellRef = (AssortmentCellReference)headerCube.CreateCellReference(aAssrtCellRef);
					placeholderCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetComponentPlaceholderCubeType());
					totPlaceholderCellRef = (AssortmentCellReference)placeholderCube.CreateCellReference(aAssrtCellRef);

					headerCellRef = (AssortmentCellReference)totHeaderCellRef.Copy();
					headerCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;
					placeholderCellRef = (AssortmentCellReference)totPlaceholderCellRef.Copy();
					placeholderCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;

					double origHdrVal = headerCellRef.CurrentCellValue;
					double origPlaceholderVal = placeholderCellRef.CurrentCellValue;
					double origTotalVal = origHdrVal + origPlaceholderVal;
					double newTotalValue = aAssrtCellRef.CurrentCellValue;
					double intermediateValue = 0;
					List<double> detailList = new List<double>();
					detailList.Add(origHdrVal);
					detailList.Add(origPlaceholderVal);

					//double tempVal = 0;
					for (int j = 0; j < detailList.Count; j++)
					{
						if (origTotalVal > +0)
						{
							intermediateValue = ((double)detailList[j] * (double)newTotalValue) / (double)origTotalVal;
							//if (intermediateValue < 0)
							//{
							//    tempVal = (intermediateValue - .5d);
							//}
							//else
							//{
							//    tempVal = (intermediateValue + .5d);
							//}
							origTotalVal -= detailList[j];
							detailList[j] = intermediateValue;
							newTotalValue -= detailList[j];
						}
					}

					// BEGIN TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab
					if (origHdrVal == 0 && origPlaceholderVal == 0)
					{
						detailList[1] = newTotalValue;
					}
					// END TT#2503 - stodd - Cannot key in Avg Str Units in Assortment Tab

					ToolBox.SetCellValue(eSetCellMode.Entry, headerCellRef, detailList[0]);
					ToolBox.SetCellValue(eSetCellMode.Entry, placeholderCellRef, detailList[1]);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#2148 - stodd - Assortment totals do not include header values

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparatives
		//-------------------------------------------

		//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		//private class clsChange_Primary_TotalPct : AssortmentChangeMethodProfile
		private class clsChange_Primary_Detail_TotalPct : AssortmentChangeMethodProfile
		//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		{
			//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
			//public clsChange_Primary_TotalPct(AssortmentViewComputations aComputations, int aKey)
			//    : base(aComputations, aKey, "TotalPct Change Method")
			public clsChange_Primary_Detail_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Detail TotalPct Change Method")
			//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				AssortmentCellReference assrtCellRef;
				double newValue;

				try
				{
					assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
					newValue = aAssrtCellRef.CurrentCellValue * assrtCellRef.CurrentCellValue / 100;
					
					ToolBox.SetCellCompLocked(assrtCellRef);
					ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//Begin TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException
		private class clsChange_Primary_Total_TotalPct : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_Total_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Total TotalPct Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				AssortmentCellReference assrtCellRef;
				double newValue;

				try
				{
					assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentSubTotal, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
					newValue = aAssrtCellRef.CurrentCellValue * assrtCellRef.CurrentCellValue / 100;

					ToolBox.SetCellCompLocked(assrtCellRef);
					ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, AssortmentTotalVariables.TotalUnits, newValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End TT#1172 - JScott - Enter Total Total % and receive System.NullReferenceException

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsChange_Primary_TotalTotal_TotalPct : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_TotalTotal_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "TotalTotal TotalPct Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				AssortmentCellReference assrtCellRef;
				//double newValue;

				double newValue;

				try
				{
					if (aAssrtCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal
						|| aAssrtCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
					{
						assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentTotal, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtCellRef.isCellHidden);
						newValue = aAssrtCellRef.CurrentCellValue * assrtCellRef.CurrentCellValue / 100;
					}
					else
					{
						assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtCellRef.isCellHidden);
						newValue = aAssrtCellRef.CurrentCellValue * assrtCellRef.CurrentCellValue / 100;
					}

					//ToolBox.SetCellCompLocked(aAssrtCellRef);
					ToolBox.SetCellValue(eSetCellMode.Entry, assrtCellRef, AssortmentTotalVariables.TotalUnits, newValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#2148 - stodd - Assortment totals do not include header values

		private class clsChange_Primary_PctToSet : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_PctToSet(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "PctToSet Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				AssortmentCellReference assrtCellRef;
				double newValue;

				try
				{
					assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentComponentGroupLevel, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
					newValue = aAssrtCellRef.CurrentCellValue * assrtCellRef.CurrentCellValue / 100;

					ToolBox.SetCellCompLocked(assrtCellRef);
					ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsChange_Primary_PctToAll : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_PctToAll(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "PctToAll Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				AssortmentCellReference assrtCellRef;
				double newValue;

				try
				{
					assrtCellRef = ToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aAssrtCellRef, eCubeType.AssortmentComponentTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtCellRef.isCellHidden);
					newValue = aAssrtCellRef.CurrentCellValue * assrtCellRef.CurrentCellValue / 100;

					ToolBox.SetCellCompLocked(assrtCellRef);
					ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, AssortmentDetailVariables.TotalUnits, newValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// BEGIN TT#2149 - stodd - Difference total does not equal All Store Set
		private class clsChange_Primary_ComponentTotal_Difference : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentTotal_Difference(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_Difference Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				try
				{
					ToolBox.InsertFormula(aCompSchd, aAssrtCellRef, FormulasAndSpreads.Formula_ComponentGrade_TotalUnits_Difference, AssortmentQuantityVariables.Value);
					ToolBox.InsertSpread(aCompSchd, aAssrtCellRef, AssortmentQuantityVariables.Value, FormulasAndSpreads.Spread_ComponentGrade_TotalUnits);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#2149 - stodd - Difference total does not equal All Store Set

		//Begin TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference
		private class clsChange_Primary_ComponentTotal_Balance : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentTotal_Balance(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal Balance Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				ComputationCube summCube;
				AssortmentCellReference summCellRef;
				AssortmentCellReference totCellRef;

				try
				{
					summCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetSummaryCubeType());

					summCellRef = (AssortmentCellReference)summCube.CreateCellReference(aAssrtCellRef);
					summCellRef[eProfileType.AssortmentSummaryVariable] = AssortmentSummaryVariables.Units.Key;
					summCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;

					totCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					totCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Total.Key;
					// END TT#2148 - stodd - Assortment totals do not include header values

					ToolBox.SetCellValue(eSetCellMode.Entry, totCellRef, summCellRef.CurrentCellValue - aAssrtCellRef.CurrentCellValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//End TT#1204 - JScott - When enterring units on the total line of a grade, the difference line does not change to reflect the new difference

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsChange_Primary_ComponentTotal_Total : AssortmentChangeMethodProfile
		{
			public clsChange_Primary_ComponentTotal_Total(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal Total Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
			{
				ComputationCube headerCube;
				ComputationCube placeholderCube;
				AssortmentCellReference totCellRef;
				AssortmentCellReference totPlaceholderCellRef;
				AssortmentCellReference totHeaderCellRef;
				AssortmentCellReference placeholderCellRef;
				AssortmentCellReference headerCellRef;


				try
				{
					headerCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetComponentHeaderCubeType());
					totHeaderCellRef = (AssortmentCellReference)headerCube.CreateCellReference(aAssrtCellRef);
					placeholderCube = (ComputationCube)aAssrtCellRef.Cube.CubeGroup.GetCube(aAssrtCellRef.AssortmentCube.GetComponentPlaceholderCubeType());
					totPlaceholderCellRef = (AssortmentCellReference)placeholderCube.CreateCellReference(aAssrtCellRef);

					//summCellRef = (AssortmentCellReference)summCube.CreateCellReference(aAssrtCellRef);
					//summCellRef[eProfileType.AssortmentSummaryVariable] = AssortmentSummaryVariables.Units.Key;
					//summCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;

					headerCellRef = (AssortmentCellReference)totHeaderCellRef.Copy();
					headerCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;
					placeholderCellRef = (AssortmentCellReference)totPlaceholderCellRef.Copy();
					placeholderCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentQuantityVariables.Value.Key;

					double tempSprdVal = aAssrtCellRef.CurrentCellValue - headerCellRef.CurrentCellValue;

					//newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, smryCellRef, AssortmentSummaryVariables.Units, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) -
					//            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

					// END TT#2148 - stodd - Assortment totals do not include header values
					//ToolBox.SetCellValue(eSetCellMode.Entry, headerCellRef, tempSprdVal);
					ToolBox.SetCellValue(eSetCellMode.Entry, placeholderCellRef, tempSprdVal);


					//ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, summCellRef.CurrentCellValue - aAssrtCellRef.CurrentCellValue);
					//ToolBox.SetCellValue(eSetCellMode.Entry, aAssrtCellRef, summCellRef.CurrentCellValue - aAssrtCellRef.CurrentCellValue);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#2148 - stodd - Assortment totals do not include header values
        // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
        private class clsChange_Primary_ComponentTotal_UnitCost : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentTotal_UnitCost(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentTotal_UnitCost Change Method")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                try
                {
                    ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.TotalCost);
                    ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.MUPct);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        private class clsChange_Primary_ComponentTotal_UnitRetail : AssortmentChangeMethodProfile
        {
            public clsChange_Primary_ComponentTotal_UnitRetail(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentTotal_UnitRetail Change Method")
            {
            }

            override public void Execute(ComputationSchedule aCompSchd, AssortmentCellReference aAssrtCellRef)
            {
                try
                {
                    ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.TotalRetail);
                    ToolBox.InsertInitFormula(aCompSchd, aAssrtCellRef, AssortmentTotalVariables.MUPct);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        // End TT#1498-MD
		//-------------------------------------------
		#endregion
		//-------------------------------------------
	}
}
