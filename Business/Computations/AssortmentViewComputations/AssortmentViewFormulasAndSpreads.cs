using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using System.Diagnostics;

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

	public class AssortmentViewFormulasAndSpreads : AssortmentFormulasAndSpreads
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
		#region Spreads
		//-------------------------------------------

		protected SpreadProfile _spread_ComponentTotal_TotalUnits;
        protected SpreadProfile _spread_ComponentTotal_Reserve;		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
		protected SpreadProfile _spread_ComponentGroupLevel_TotalUnits;
		protected SpreadProfile _spread_ComponentGroupLevel_AvgUnits;
		protected SpreadProfile _spread_ComponentGrade_TotalUnits;
		protected SpreadProfile _spread_ComponentGrade_AvgUnits;
		protected Hashtable _spread_ComponentSubTotal_TotalUnits;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		protected FormulaProfile _init_Null;

		// Detail Inits

		protected FormulaProfile _init_HeaderColorDetail_TotalUnits;
		protected FormulaProfile _init_ColorDetail_UnitCost;
		protected FormulaProfile _init_ColorDetail_UnitRetail;
        protected FormulaProfile _init_HeaderTotalDetail_HeaderUnits;


		// Component Grade/GroupLevel Inits

		protected FormulaProfile _init_ComponentGradeGroupLevel_AvgUnits;
		protected FormulaProfile _init_ComponentGradeGroupLevel_Index;
        protected FormulaProfile _init_ComponentGroupLevel_NumStoresAlloc;		// TT#4294 - stodd - Average Units in Matrix Enahancement
        protected FormulaProfile _init_ComponentGrade_NumStoresAlloc;		// TT#4294 - stodd - Average Units in Matrix Enahancement


		// Component Total Inits

		protected FormulaProfile _init_ComponentHeaderTotal_HeaderUnits;
		protected FormulaProfile _init_ComponentHeaderTotal_Balance;
		protected FormulaProfile _init_ComponentHeaderTotal_Intransit;	// TT#1225 - stodd - intransit
		protected FormulaProfile _init_ComponentHeaderTotal_OnHand;	// TT#2148 - stodd - intransit
		protected FormulaProfile _init_ComponentHeaderTotal_Reserve;	// TT#2148 - stodd - intransit

		protected FormulaProfile _init_ComponentHeaderTotal_Committed;	// TT#1224 - stodd - committed
		protected FormulaProfile _init_ComponentTotal_TotalCost;
		protected FormulaProfile _init_ComponentTotal_TotalRetail;
		protected FormulaProfile _init_ComponentTotal_MUPct;
		protected FormulaProfile _init_ComponentTotal_Committed;		// TT#1224 - stodd - committed
		protected FormulaProfile _init_ComponentTotal_Intransit;		// TT#1225 - stodd - intransit
		protected FormulaProfile _init_ComponentTotal_OnHand;		// TT#2148 - stodd - intransit
		protected FormulaProfile _init_ComponentTotal_Reserve;		// TT#2148 - stodd - intransit
        protected FormulaProfile _init_ComponentTotal_Balance;		// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 	

		//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
		protected FormulaProfile _init_ComponentTotal_TotalAvgTotalUnitCost;
		protected FormulaProfile _init_ComponentTotal_TotalAvgTotalUnitRetail;
		//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
		protected FormulaProfile _init_ComponentTotal_AvgUnits;
        protected FormulaProfile _init_ComponentTotal_NumStoresAlloc;		// TT#4294 - stodd - Average Units in Matrix Enahancement

		// Summary Inits

		protected FormulaProfile _init_SummaryGradeGroupLevel_AvgUnits;
		protected FormulaProfile _init_SummaryTotal_AvgUnits;

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

		protected FormulaProfile _init_ComponentGradeGroupLevel_TotalPct;
		protected FormulaProfile _init_ComponentTotal_TotalPct;
		protected FormulaProfile _init_PctToSet;
		protected FormulaProfile _init_PctToAll;
		protected FormulaProfile _init_Balance;
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		protected FormulaProfile _init_Total;
		protected FormulaProfile _init_AvgUnits;
		protected FormulaProfile _init_TotalPctToAll;
		protected FormulaProfile _init_Total_TotalPct;
		protected FormulaProfile _init_TotalPct;

		// END TT#2148 - stodd - Assortment totals do not include header values
		protected FormulaProfile _init_Difference;
		// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//protected FormulaProfile _init_Comb_TotalPct;
		// END TT#2150 - stodd - totals not showing in main matrix grid

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

        protected FormulaProfile _formula_ComponentGrade_TotalUnits_AvgUnits;
		// BEGIN TT#1636 - stodd - index not re-calcing
		protected FormulaProfile _formula_ComponentGrade_Index_AvgUnits;
		// END TT#1636 - stodd - index not re-calcing
		protected FormulaProfile _formula_ComponentGrade_TotalUnits_Index;
		protected FormulaProfile _formula_ComponentGrade_TotalUnits_Difference;
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		//protected FormulaProfile _formula_ComponentGrade_TotalUnits_SummaryAvgUnits;
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		protected FormulaProfile _formula_Calculate_AvgUnits_From_TotalUnits;
        protected FormulaProfile _formula_Calculate_TotalPct_From_TotalUnits;	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
        //protected FormulaProfile _formula_Calculate_Balance_From_TotalUnits;	// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 

		// BEGIN TT#1636 - stodd - index not re-calcing
		protected FormulaProfile _formula_Calculate_Index_From_AvgUnits;
		// END TT#1636 - stodd - index not re-calcing
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		protected FormulaProfile _formula_ComponentTotal_TotalUnits_AvgUnits;
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		// BEGIN TT#1636 - stodd - index not re-calcing
		protected FormulaProfile _formula_ComponentTotal_Index_AvgUnits;
		// END TT#1636 - stodd - index not re-calcing

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

		public AssortmentViewFormulasAndSpreads(AssortmentViewComputations aComputations)
			: base(aComputations)
		{
			_computations = aComputations;
			_seq = 1;

			//-------------------------------------------
			#region Spreads
			//-------------------------------------------

			_spread_ComponentTotal_TotalUnits = new clsSpread_ComponentTotal_TotalUnits(aComputations, _seq++);
            _spread_ComponentTotal_Reserve = new clsSpread_ComponentTotal_Reserve(aComputations, _seq++);		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
			_spread_ComponentGroupLevel_TotalUnits = new clsSpread_ComponentGroupLevel_TotalUnits(aComputations, _seq++);
			_spread_ComponentGrade_TotalUnits = new clsSpread_ComponentGrade_TotalUnits(aComputations, _seq++);
			_spread_ComponentSubTotal_TotalUnits = new Hashtable();
			_spread_ComponentGroupLevel_AvgUnits = new clsSpread_ComponentGroupLevel_AvgUnits(aComputations, _seq++);
			_spread_ComponentGrade_AvgUnits = new clsSpread_ComponentGrade_AvgUnits(aComputations, _seq++);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Inits
			//-------------------------------------------

			_init_Null = new clsInit_Null(aComputations, _seq++);

			// Detail Inits

			_init_HeaderColorDetail_TotalUnits = new clsInit_HeaderColorDetail_TotalUnits(aComputations, _seq++);
			_init_ColorDetail_UnitCost = new clsInit_ColorDetail_UnitCost(aComputations, _seq++);
			_init_ColorDetail_UnitRetail = new clsInit_ColorDetail_UnitRetail(aComputations, _seq++);
            _init_HeaderTotalDetail_HeaderUnits = new clsInit_HeaderTotalDetail_HeaderUnits(aComputations, _seq++);

			// Component Grade/GroupLevel Inits

			_init_ComponentGradeGroupLevel_AvgUnits = new clsInit_ComponentGradeGroupLevel_AvgUnits(aComputations, _seq++);
			_init_ComponentGradeGroupLevel_Index = new clsInit_ComponentGradeGroupLevel_Index(aComputations, _seq++);
            _init_ComponentGroupLevel_NumStoresAlloc = new clsInit_ComponentGroupLevel_NumStoresAlloc(aComputations, _seq++);		// TT#4294 - stodd - Average Units in Matrix Enahancement
            _init_ComponentGrade_NumStoresAlloc = new clsInit_ComponentGrade_NumStoresAlloc(aComputations, _seq++);		// TT#4294 - stodd - Average Units in Matrix Enahancement


			// Component Total Inits

			_init_ComponentHeaderTotal_HeaderUnits = new clsInit_ComponentHeaderTotal_HeaderUnits(aComputations, _seq++);
			_init_ComponentHeaderTotal_Balance = new clsInit_ComponentHeaderTotal_Balance(aComputations, _seq++);
			_init_ComponentHeaderTotal_Intransit = new clsInit_ComponentHeaderTotal_Intransit(aComputations, _seq++);	// TT#1225 - stodd - intransit
			_init_ComponentHeaderTotal_OnHand = new clsInit_ComponentHeaderTotal_OnHand(aComputations, _seq++);	// TT#2148 - stodd - 
			_init_ComponentHeaderTotal_Reserve = new clsInit_ComponentHeaderTotal_Reserve(aComputations, _seq++);	// TT#2148 - stodd - 


			_init_ComponentHeaderTotal_Committed = new clsInit_ComponentHeaderTotal_Committed(aComputations, _seq++);	// TT#1224 - stodd - committed
			_init_ComponentTotal_TotalCost = new clsInit_ComponentTotal_TotalCost(aComputations, _seq++);
			_init_ComponentTotal_TotalRetail = new clsInit_ComponentTotal_TotalRetail(aComputations, _seq++);
			_init_ComponentTotal_MUPct = new clsInit_ComponentTotal_MUPct(aComputations, _seq++);
			_init_ComponentTotal_Committed = new clsInit_ComponentTotal_Committed(aComputations, _seq++);				// TT#1224 - stodd - committed
			_init_ComponentTotal_Intransit = new clsInit_ComponentTotal_Intransit(aComputations, _seq++);				// TT#1225 - stodd - intransit
			_init_ComponentTotal_OnHand = new clsInit_ComponentTotal_OnHand(aComputations, _seq++);				// TT#2148 - stodd - 
			_init_ComponentTotal_Reserve = new clsInit_ComponentTotal_Reserve(aComputations, _seq++);				// TT#2148 - stodd - 
            _init_ComponentTotal_Balance = new clsInit_ComponentTotal_Balance(aComputations, _seq++);			// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 			 


			//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
			_init_ComponentTotal_TotalAvgTotalUnitCost = new clsInit_ComponentTotal_TotalAvgTotalUnitCost(aComputations, _seq++);
			_init_ComponentTotal_TotalAvgTotalUnitRetail = new clsInit_ComponentTotal_TotalAvgTotalUnitRetail(aComputations, _seq++);
			//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 
			
			_init_ComponentTotal_AvgUnits = new clsInit_ComponentTotal_AvgUnits(aComputations, _seq++);
            _init_ComponentTotal_NumStoresAlloc = new clsInit_ComponentTotal_NumStoresAlloc(aComputations, _seq++);		// TT#4294 - stodd - Average Units in Matrix Enahancement


			// Summary Inits

			_init_SummaryGradeGroupLevel_AvgUnits = new clsInit_SummaryGradeGroupLevel_AvgUnits(aComputations, _seq++);
			_init_SummaryTotal_AvgUnits = new clsInit_SummaryTotal_AvgUnits(aComputations, _seq++);

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

			_init_ComponentGradeGroupLevel_TotalPct = new clsInit_ComponentGradeGroupLevel_TotalPct(aComputations, _seq++);
			_init_ComponentTotal_TotalPct = new clsInit_ComponentTotal_TotalPct(aComputations, _seq++);
			_init_PctToSet = new clsInit_PctToSet(aComputations, _seq++);
			_init_PctToAll = new clsInit_PctToAll(aComputations, _seq++);
			_init_Balance = new clsInit_Balance(aComputations, _seq++);
			// BEGIN TT#2148 - stodd - Assortment totals do not include header values
			_init_Total = new clsInit_Total(aComputations, _seq++);
			_init_AvgUnits = new clsInit_AvgUnits(aComputations, _seq++);
			_init_TotalPctToAll = new clsInit_TotalPctToAll(aComputations, _seq++);
			_init_Total_TotalPct = new clsInit_Total_TotalPct(aComputations, _seq++);
			_init_TotalPct = new clsInit_TotalPct(aComputations, _seq++);

			// END TT#2148 - stodd - Assortment totals do not include header values
			_init_Difference = new clsInit_Difference(aComputations, _seq++);
			// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
			//_init_Comb_TotalPct = new clsInit_Comb_TotalPct(aComputations, _seq++);
			// END TT#2150 - stodd - totals not showing in main matrix grid

			//-------------------------------------------
			#endregion
			//-------------------------------------------

            _formula_ComponentGrade_TotalUnits_AvgUnits = new clsFormula_ComponentGrade_TotalUnits_AvgUnits(aComputations, _seq++);
			// BEGIN TT#1636 - stodd index not re-calcing
			_formula_ComponentGrade_Index_AvgUnits = new clsFormula_ComponentGrade_Index_AvgUnits(aComputations, _seq++);
			// END TT#1636 - stodd index not re-calcing
			_formula_ComponentGrade_TotalUnits_Index = new clsFormula_ComponentGrade_TotalUnits_Index(aComputations, _seq++);
			_formula_ComponentGrade_TotalUnits_Difference = new clsFormula_ComponentGrade_TotalUnits_Difference(aComputations, _seq++);
			//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
			//_formula_ComponentGrade_TotalUnits_SummaryAvgUnits = new clsFormula_ComponentGrade_TotalUnits_SummaryAvgUnits(aComputations, _seq++);
			//End TT#1196 - JScott - Average units in the summary section should spread when changed
			_formula_Calculate_AvgUnits_From_TotalUnits = new clsFormula_Calculate_AvgUnits_From_TotalUnits(aComputations, _seq++);
            _formula_Calculate_TotalPct_From_TotalUnits = new clsFormula_Calculate_TotalPct_From_TotalUnits(aComputations, _seq++);	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
            //_formula_Calculate_Balance_From_TotalUnits = new clsFormula_Calculate_Balance_From_TotalUnits(aComputations, _seq++);	

			// BEGIN TT#1636 - stodd index not re-calcing
			_formula_Calculate_Index_From_AvgUnits = new clsFormula_Calculate_Index_From_AvgUnits(aComputations, _seq++);
			// End TT#1636 - stodd index not re-calcing
			//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
            _formula_ComponentTotal_TotalUnits_AvgUnits = new clsFormula_ComponentTotal_TotalUnits_AvgUnits(aComputations, _seq++);
			//End TT#1196 - JScott - Average units in the summary section should spread when changed
			// BEGIN TT#1636 - stodd index not re-calcing
			_formula_ComponentTotal_Index_AvgUnits = new clsFormula_ComponentTotal_Index_AvgUnits(aComputations, _seq++);
			// END TT#1636 - stodd index not re-calcing


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

		public SpreadProfile Spread_ComponentTotal_TotalUnits { get { return _spread_ComponentTotal_TotalUnits; } }
        public SpreadProfile Spread_ComponentTotal_Reserve { get { return _spread_ComponentTotal_Reserve; } }		// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

		public SpreadProfile Spread_ComponentGroupLevel_TotalUnits { get { return _spread_ComponentGroupLevel_TotalUnits; } }
		public SpreadProfile Spread_ComponentGroupLevel_AvgUnits { get { return _spread_ComponentGroupLevel_AvgUnits; } }
		public SpreadProfile Spread_ComponentGrade_TotalUnits { get { return _spread_ComponentGrade_TotalUnits; } }
		public SpreadProfile Spread_ComponentGrade_AvgUnits { get { return _spread_ComponentGrade_AvgUnits; } }


		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		public FormulaProfile Init_Null { get { return _init_Null; } }

		// Detail Inits

		public FormulaProfile Init_HeaderColorDetail_TotalUnits { get { return _init_HeaderColorDetail_TotalUnits; } }
		public FormulaProfile Init_ColorDetail_UnitCost { get { return _init_ColorDetail_UnitCost; } }
		public FormulaProfile Init_ColorDetail_UnitRetail { get { return _init_ColorDetail_UnitRetail; } }
        public FormulaProfile Init_HeaderTotalDetail_HeaderUnits { get { return _init_HeaderTotalDetail_HeaderUnits; } }

		// Component Grade/GroupLevel Inits

		public FormulaProfile Init_ComponentGradeGroupLevel_AvgUnits { get { return _init_ComponentGradeGroupLevel_AvgUnits; } }
		public FormulaProfile Init_ComponentGradeGroupLevel_Index { get { return _init_ComponentGradeGroupLevel_Index; } }

        public FormulaProfile Init_ComponentGroupLevel_NumStoresAlloc { get { return _init_ComponentGroupLevel_NumStoresAlloc; } }	// TT#4294 - stodd - Average Units in Matrix Enahancement
        public FormulaProfile Init_ComponentGrade_NumStoresAlloc { get { return _init_ComponentGrade_NumStoresAlloc; } }	// TT#4294 - stodd - Average Units in Matrix Enahancement

		// Component Total Inits

		public FormulaProfile Init_ComponentHeaderTotal_HeaderUnits { get { return _init_ComponentHeaderTotal_HeaderUnits; } }
		public FormulaProfile Init_ComponentHeaderTotal_Balance { get { return _init_ComponentHeaderTotal_Balance; } }
		public FormulaProfile Init_ComponentHeaderTotal_Intransit { get { return _init_ComponentHeaderTotal_Intransit; } }	// TT#1225 - stodd - intransit
		public FormulaProfile Init_ComponentHeaderTotal_OnHand { get { return _init_ComponentHeaderTotal_OnHand; } }	// TT#2148 - stodd - intransit
		public FormulaProfile Init_ComponentHeaderTotal_Reserve { get { return _init_ComponentHeaderTotal_Reserve; } }	// TT#2148 - stodd - intransit


		public FormulaProfile Init_ComponentHeaderTotal_Committed { get { return _init_ComponentHeaderTotal_Committed; } }	// TT#1224 - stodd - committed
		public FormulaProfile Init_ComponentTotal_TotalCost { get { return _init_ComponentTotal_TotalCost; } }
		public FormulaProfile Init_ComponentTotal_TotalRetail { get { return _init_ComponentTotal_TotalRetail; } }
		public FormulaProfile Init_ComponentTotal_MUPct { get { return _init_ComponentTotal_MUPct; } }
		public FormulaProfile Init_ComponentTotal_Committed { get { return _init_ComponentTotal_Committed; } }					// TT#1224 - stodd - committed
		public FormulaProfile Init_ComponentTotal_Intransit { get { return _init_ComponentTotal_Intransit; } }					// TT#1225 - stodd - intransit
		public FormulaProfile Init_ComponentTotal_OnHand { get { return _init_ComponentTotal_OnHand; } }					// TT#2148 - stodd - intransit
		public FormulaProfile Init_ComponentTotal_Reserve { get { return _init_ComponentTotal_Reserve; } }					// TT#2148 - stodd - intransit
        public FormulaProfile Init_ComponentTotal_Balance { get { return _init_ComponentTotal_Balance; } }					// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated



		public FormulaProfile Init_ComponentTotal_AvgUnits { get { return _init_ComponentTotal_AvgUnits; } }
        public FormulaProfile Init_ComponentTotal_NumStoresAlloc { get { return _init_ComponentTotal_NumStoresAlloc; } }	// TT#4294 - stodd - Average Units in Matrix Enahancement
		//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
		public FormulaProfile Init_ComponentTotal_TotalAvgTotalUnitCost { get { return _init_ComponentTotal_TotalAvgTotalUnitCost; } }
		public FormulaProfile Init_ComponentTotal_TotalAvgTotalUnitRetail { get { return _init_ComponentTotal_TotalAvgTotalUnitRetail; } }
		//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 

		// Summary Inits

		public FormulaProfile Init_SummaryGradeGroupLevel_AvgUnits { get { return _init_SummaryGradeGroupLevel_AvgUnits; } }
		public FormulaProfile Init_SummaryTotal_AvgUnits { get { return _init_SummaryTotal_AvgUnits; } }

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

		public FormulaProfile Init_ComponentGradeGroupLevel_TotalPct { get { return _init_ComponentGradeGroupLevel_TotalPct; } }
		public FormulaProfile Init_ComponentTotal_TotalPct { get { return _init_ComponentTotal_TotalPct; } }
		public FormulaProfile Init_PctToSet { get { return _init_PctToSet; } }
		public FormulaProfile Init_PctToAll { get { return _init_PctToAll; } }
		public FormulaProfile Init_Balance { get { return _init_Balance; } }
		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		public FormulaProfile Init_Total { get { return _init_Total; } }
		public FormulaProfile Init_AvgUnits { get { return _init_AvgUnits; } }
		public FormulaProfile Init_TotalPctToAll { get { return _init_TotalPctToAll; } }
		public FormulaProfile Init_Total_TotalPct { get { return _init_Total_TotalPct; } }
		public FormulaProfile Init_TotalPct { get { return _init_TotalPct; } }


		// END TT#2148 - stodd - Assortment totals do not include header values
		public FormulaProfile Init_Difference { get { return _init_Difference; } }
		// BEGIN TT#2150 - stodd - totals not showing in main matrix grid
		//public FormulaProfile Init_Comb_TotalPct { get { return _init_TotalPct; } }
		// END TT#2150 - stodd - totals not showing in main matrix grid
		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Formulas
		//-------------------------------------------

        public FormulaProfile Formula_ComponentGrade_TotalUnits_AvgUnits { get { return _formula_ComponentGrade_TotalUnits_AvgUnits; } }
		// BEGIN TT#1636 - stodd - index not calculating
		public FormulaProfile Formula_ComponentGrade_Index_AvgUnits { get { return _formula_ComponentGrade_Index_AvgUnits; } }
		// END TT#1636 - stodd - index not calculating
		public FormulaProfile Formula_ComponentGrade_TotalUnits_Index { get { return _formula_ComponentGrade_TotalUnits_Index; } }
		public FormulaProfile Formula_ComponentGrade_TotalUnits_Difference { get { return _formula_ComponentGrade_TotalUnits_Difference; } }
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		//public FormulaProfile Formula_ComponentGrade_TotalUnits_SummaryAvgUnits { get { return _formula_ComponentGrade_TotalUnits_SummaryAvgUnits; } }
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		public FormulaProfile Formula_Calculate_AvgUnits_From_TotalUnits { get { return _formula_Calculate_AvgUnits_From_TotalUnits; } }
        public FormulaProfile Formula_Calculate_TotalPct_From_TotalUnits { get { return _formula_Calculate_TotalPct_From_TotalUnits; } }	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
        //public FormulaProfile Formula_Calculate_Balance_From_TotalUnits { get { return _formula_Calculate_Balance_From_TotalUnits; } }	// TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated

		// BEGIN TT#1636 - stodd - index not calculating
		public FormulaProfile Formula_Calculate_Index_From_AvgUnits { get { return _formula_Calculate_Index_From_AvgUnits; } }
		// END TT#1636 - stodd - index not calculating
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
        public FormulaProfile Formula_ComponentTotal_TotalUnits_AvgUnits { get { return _formula_ComponentTotal_TotalUnits_AvgUnits; } }
		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		// BEGIN TT#1636 - stodd - index not calculating
		public FormulaProfile Formula_ComponentTotal_Index_AvgUnits { get { return _formula_ComponentTotal_Index_AvgUnits; } }
		// END TT#1636 - stodd - index not calculating

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

		public SpreadProfile Spread_ComponentSubTotal_TotalUnits(int aLevel)
		{
			SpreadProfile subTotalSpread;

			try
			{
				subTotalSpread = (SpreadProfile)_spread_ComponentSubTotal_TotalUnits[cSubTotalSeq + aLevel];

				if (subTotalSpread == null)
				{
					subTotalSpread = new clsSpread_ComponentSubTotal_TotalUnits(_computations, cSubTotalSeq + aLevel);
					_spread_ComponentSubTotal_TotalUnits[cSubTotalSeq + aLevel] = subTotalSpread;
				}

				return subTotalSpread;
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

		abstract private class AssortmentFormulaProfile : FormulaProfile
		{
			//=======
			// FIELDS
			//=======

			AssortmentViewComputations _computations;

			//=============
			// CONSTRUCTORS
			//=============

			public AssortmentFormulaProfile(AssortmentViewComputations aComputations, int aKey, string aName)
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

			protected AssortmentViewTotalVariables AssortmentTotalVariables
			{
				get
				{
					return (AssortmentViewTotalVariables)_computations.AssortmentTotalVariables;
				}
			}

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

			abstract public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode);
		}

		abstract private class AssortmentSpreadProfile : SpreadProfile
		{
			//=======
			// FIELDS
			//=======

			AssortmentViewComputations _computations;

			//=============
			// CONSTRUCTORS
			//=============

			public AssortmentSpreadProfile(AssortmentViewComputations aComputations, int aKey, string aName)
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
			public clsFormula_Sum(AssortmentViewComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					//if (aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.CubeType.Level == 1)
					//{
					//    aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.QuantityVariableProfileType] = AssortmentQuantityVariables.Total.Key;
					//}
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
			public clsFormula_Average(AssortmentViewComputations aComputations, int aKey, string aName)
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
			public clsFormula_AverageValue(AssortmentViewComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, ToolBox.CalculateAverage(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Value));


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

		abstract private class clsSpread_BaseMultiple : AssortmentSpreadProfile
		{
			Hashtable _cellRefHash;

			public clsSpread_BaseMultiple(AssortmentViewComputations aComputations, int aKey, string aName)
				: base(aComputations, aKey, aName)
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleSpreadEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				int newTotal;
				//BEGIN TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
				int detailTotal = 0;
				int compChangeCount = 0;
				//END TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception  
				// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                bool hasStores = false;
                int numStores = 0;
				// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid

				try
				{
					newTotal = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, aSetCellMode, aAssrtSchdEntry.ComputationCellRef, aAssrtSchdEntry.ComputationCellRef.isCellHidden);

					_cellRefHash = new Hashtable();

					
					foreach (AssortmentCellReference cellRef in aAssrtSchdEntry.SpreadCellRefList)
					{
						// BEGIN TT#769-MD - Stodd - matrix spreads to grades with no stores
						//================================================================================================
						// This determines if the detail cell contains a Store Group Level and a Grade,
						// if so, the Store Group Level / Grade combination is checked to see if it has any stores.
						// If it does, the detail cell is added to the detail list. If not, it is skipped.
						//================================================================================================
						int grade = cellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);
						int sgl = cellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);

						// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
						//bool hasStores = true;
                        hasStores = true;
						// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
						if (grade > -1 && sgl > -1)
						{
                            hasStores = HasStores(aAssrtSchdEntry, aGetCellMode, aSetCellMode, ref numStores, cellRef);	// TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
						}
						// END TT#769-MD - Stodd - matrix spreads to grades with no stores
						//BEGIN TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
						if (!cellRef.isCellBlocked && hasStores)	// TT#769-MD - Stodd - matrix spreads to grades with no stores
						{
							// Begin TT#1756-MD - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid 
                            // Typically, in this situation the cellRef is "Fixed" because all of the stores are "out'd", so none can recieve any spread.
                            if (!cellRef.isCellFixed)
                            {
                                _cellRefHash.Add(cellRef, cellRef);
                                detailTotal += Convert.ToInt32(cellRef.CurrentCellValue);
                                if (cellRef.isCellCompChanged)
                                {
                                    compChangeCount++;
                                }
                            }
							// End TT#1756-MD - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid 
						}
						//END TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
					}
					
					//BEGIN TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   
					//================================================================================================
					// If all of the detail cells have been changed by computations AND
					// the total of the detail already equals the new total,
					// there is no reason to spread. If a spread is done, a "nothingToSpread" error will result.
					// This specifically happens when you enter "Total %" for ALL placeholders.
					// this changes all of the total units and there is not reason to spread the grand total 
					// back to them.
					//================================================================================================
					// Begin TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					if ( (newTotal == detailTotal && compChangeCount == _cellRefHash.Count)
                        || (_cellRefHash.Count == 0 && aAssrtSchdEntry.AssortmentCellRef.GetDimensionProfileTypeIndex(eProfileType.AssortmentTotalVariable) > -1 
                            && aAssrtSchdEntry.AssortmentCellRef[eProfileType.AssortmentTotalVariable] == (int)eAssortmentTotalVariables.ReserveUnits))
					// End TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
					{
						// skip spread
					}
					else
					{
						intSpreadToDetail(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.ComputationCellRef, newTotal);
					}
					//END TT#654-MD - stodd - type in % to total in the total section, hit enter twice receive a Nothing to Spread Exception   

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
            private bool HasStores(AssortmentScheduleSpreadEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, ref int numStores, AssortmentCellReference cellRef)
            {
                bool hasStores = true;
                AssortmentCellReference copyCellRef = null;
                ComputationCube summCube = (ComputationCube)cellRef.Cube.CubeGroup.GetCube(cellRef.AssortmentCube.GetSummaryCubeType());
                copyCellRef = (AssortmentCellReference)cellRef.Copy();
                copyCellRef[cellRef.QuantityVariableProfileType] = AssortmentQuantityVariables.Value.Key;
                ComputationCellReference summCellRef = (ComputationCellReference)summCube.CreateCellReference(copyCellRef);

                // Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                {
                    ProfileList outStoreList = null;
                    bool doesSetGradeContainStores = false;
                    numStores = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(cellRef, false, out outStoreList, out doesSetGradeContainStores);
                    // Begin TT#1678-MD - stodd - GA Allocation -> Matrix Tab-> enter Average Units and apply-> changes to zero
                    if (numStores == 0 && doesSetGradeContainStores)
                    {
                        summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
                        numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false);
                    }
                    // End TT#1678-MD - stodd - GA Allocation -> Matrix Tab-> enter Average Units and apply-> changes to zero
                }
                else
                {
                    numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(summCellRef), AssortmentSummaryVariables.NumStores, false);
                }
                // End TT#4294 - stodd - Average Units in Matrix Enahancement
                if (numStores < 1)
                {
                    hasStores = false;
                }
                return hasStores;
            }
			// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid

			override public ArrayList GetSpreadToCellReferenceList(AssortmentCellReference aAssrtCellRef)
			{
				ArrayList detList;

				try
				{
					detList = new ArrayList();
					intRecurseDetailCells(aAssrtCellRef, detList);
					return detList;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			override public bool CascadeChangeMethods
			{
				get
				{
					return true;
				}
			}

            private void intRecurseDetailCells(AssortmentCellReference aAssrtCellRef, ArrayList aSpreadList)
			{
				ArrayList detList;

				try
				{
					detList = ToolBox.GetSpreadDetailCellRefArray(aAssrtCellRef, aAssrtCellRef.isCellHidden);

					aSpreadList.AddRange(detList);
                }
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			private int intSpreadToDetail(AssortmentScheduleSpreadEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, ComputationCellReference aFromCellRef, int aTotalUnits)
			{
				ArrayList allDetList;
				ArrayList detList;
				int oldTotal = 0;
				int newTotal = 0;
				int excludedTotal;
				int spreadTotal = 0;	// TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
				int oldUnits;
				int newUnits;
				int multiple;
				int bulkMultiple;
				int multipleTotal;
				SortedList sortList;
				int hdrRID;
				int packRID;
                IDictionaryEnumerator sortEnum;
				MultipleSortKey outSortKey;

				int i;
				int spreadValueSign;
				int[] origValueArray;
				int[] oldValueArray;
				int[] newValueArray;
				System.Collections.BitArray spreadToFlag;
				int excluded = 0;
				int excludedOldTotal = 0;
				int excludedNewTotal = 0;
                //int numStores = 0;

				try
				{
					// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                    if (_cellRefHash == null || _cellRefHash.Count == 0)
                    {
                        return spreadTotal;
                    }
					// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
					allDetList = ToolBox.GetSpreadDetailCellRefArray(aFromCellRef, aFromCellRef.isCellHidden);

					newTotal = aTotalUnits;
					detList = new ArrayList();

					foreach (AssortmentCellReference cellRef in allDetList)
					{
						if (_cellRefHash.Contains(cellRef))
						{
							detList.Add(cellRef);
						}
					}

					if (detList.Count > 0)
					{
                        if (((AssortmentCellReference)detList[0]).AssortmentCube.CubeType == eCubeType.AssortmentHeaderColorDetail ||
                            ((AssortmentCellReference)detList[0]).AssortmentCube.CubeType == eCubeType.AssortmentPlaceholderColorDetail)
                        {
							oldTotal = 0;
							excludedTotal = 0;
							multipleTotal = 0;
							sortList = new SortedList();

							foreach (AssortmentCellReference cellRef in detList)
							{
								if (_cellRefHash.Contains(cellRef))
								{
									oldUnits = (int)cellRef.CurrentCellValue;
									oldTotal += oldUnits;

									if (cellRef.isCellLocked ||
										cellRef.isCellFixed ||
										cellRef.isCellBlocked ||
										cellRef.isCellCompChanged ||
										cellRef.isCellReadOnly ||
										cellRef.isCellExcludedFromSpread)
									{
										excludedTotal += oldUnits;
									}
									else
									{
										hdrRID = cellRef[cellRef.AssortmentCube.HeaderProfileType];
										packRID = cellRef[eProfileType.HeaderPack];

										if (cellRef.AssortmentCube.isColorDefined)
										{
											if (packRID == int.MaxValue)
											{
												multiple = cellRef.AssortmentCube.AssortmentCubeGroup.GetColorMultiple(hdrRID, packRID, cellRef[eProfileType.HeaderPackColor]);
												bulkMultiple = cellRef.AssortmentCube.AssortmentCubeGroup.GetPackMultiple(hdrRID, packRID);
												// Begin TT#2167 - stodd - zeroed out placeholder causes large negative numbers in difference 
												if (multiple < 1)
												{
													multiple = 1;
												}
												if (bulkMultiple < 1)
												{
													bulkMultiple = 1;
												}
												// END TT#2167 - stodd - zeroed out placeholder causes large negative numbers in difference 
												sortList.Add(new MultipleSortKey(cellRef, packRID, oldUnits, multiple, bulkMultiple), null);
											}
											else
											{
												multiple = cellRef.AssortmentCube.AssortmentCubeGroup.GetColorMultiple(hdrRID, packRID, cellRef[eProfileType.HeaderPackColor]);
												// Begin TT#2167 - stodd - zeroed out placeholder causes large negative numbers in difference 
												if (multiple < 1)
												{
													multiple = 1;
												}
												// END TT#2167 - stodd - zeroed out placeholder causes large negative numbers in difference 
												sortList.Add(new MultipleSortKey(cellRef, packRID, oldUnits, multiple), null);
											}
										}
										else
										{
											multiple = cellRef.AssortmentCube.AssortmentCubeGroup.GetPackMultiple(hdrRID, packRID);
											// Begin TT#2167 - stodd - zeroed out placeholder causes large negative numbers in difference 
											if (multiple < 1)
											{
												multiple = 1;
											}
											// END TT#2167 - stodd - zeroed out placeholder causes large negative numbers in difference 
											sortList.Add(new MultipleSortKey(cellRef, packRID, oldUnits, multiple), null);
										}

										multipleTotal += multiple;
									}
								}
							}

							if (sortList.Count > 0)
							{
								newTotal -= excludedTotal;
								oldTotal -= excludedTotal;
								spreadTotal = 0;

								sortEnum = sortList.GetEnumerator();

								double detailsRemaining = sortList.Count;	// TT#665-MD - stodd - asst with placeholder and placeholder colors when input avg units in the matrix units not spread evenly to placeholder colors.
								while (sortEnum.MoveNext())
								{
									outSortKey = (MultipleSortKey)sortEnum.Key;
									oldUnits = (int)outSortKey.CellReference.CurrentCellValue;

									if (oldTotal > 0)
									{
										newUnits = (int)((((double)newTotal * (double)oldUnits) / (double)oldTotal) + .5d);
									}
									else
									{
										//BEGIN TT#665-MD - stodd - asst with placeholder and placeholder colors when input avg units in the matrix units not spread evenly to placeholder colors.
										if (outSortKey.PackRID == int.MaxValue)	// BULK COLORS
										{
											if (detList.Count == 1)
											{
												newUnits = newTotal;
											}
											else
											{
												newUnits = (int)(((double)newTotal / (double)detailsRemaining--) + .5d);
											}
										}
										//END TT#665-MD - stodd - asst with placeholder and placeholder colors when input avg units in the matrix units not spread evenly to placeholder colors.
										else // PACK COLORS
										{
											newUnits = (int)((((double)newTotal * (double)outSortKey.Mulitple) / (double)multipleTotal) + .5d);
										}
									}

									if (outSortKey.Mulitple != 0)
									{
										newUnits = (int)(((double)newUnits / (double)outSortKey.Mulitple) + .5d);
										newUnits = newUnits * outSortKey.Mulitple;
									}
									else if (outSortKey.CellReference.AssortmentCube.isColorDefined && outSortKey.PackRID == int.MaxValue)
									{
                                        if (detList.Count == 1)
										{
											newUnits = (int)(((double)newUnits / (double)outSortKey.BulkMulitple) + .5d);
											newUnits = newUnits * outSortKey.BulkMulitple;
										}
									}

									if (newUnits > newTotal)
									{
										newUnits = (int)(((double)newTotal / (double)outSortKey.Mulitple) + .5d);
										newUnits = newUnits * outSortKey.Mulitple;
									}

									newTotal -= newUnits;
									oldTotal -= oldUnits;
									spreadTotal += newUnits;

									ToolBox.SetCellValue(aSetCellMode, outSortKey.CellReference, newUnits);
								}

								return spreadTotal;
							}
							else
							{
								throw new NothingToSpreadException();
							}
						}
						else
						{
							origValueArray = new int[detList.Count];
							oldValueArray = new int[detList.Count];
							newValueArray = new int[detList.Count];
							spreadToFlag = new System.Collections.BitArray(detList.Count, true);
							spreadTotal = 0;

							for (i = 0; i < detList.Count; i++)
							{
								origValueArray[i] = Convert.ToInt32(((AssortmentCellReference)detList[i]).CurrentCellValue);
								oldValueArray[i] = origValueArray[i];
								newValueArray[i] = origValueArray[i];
								oldTotal += origValueArray[i];
							}

							spreadValueSign = System.Math.Sign(oldTotal);

							for (i = 0; i < detList.Count; i++)
							{
								if (System.Math.Sign(oldValueArray[i]) != spreadValueSign)
								{
									oldTotal -= oldValueArray[i];
									oldValueArray[i] = 0;
								}
							}

							for (i = 0; i < detList.Count; i++)
							{

								if (((AssortmentCellReference)detList[i]).isCellLocked ||
									((AssortmentCellReference)detList[i]).isCellFixed ||
									((AssortmentCellReference)detList[i]).isCellBlocked ||
									((AssortmentCellReference)detList[i]).isCellCompChanged ||
									((AssortmentCellReference)detList[i]).isCellReadOnly ||
									((AssortmentCellReference)detList[i]).isCellExcludedFromSpread)
								{
									excludedNewTotal = excludedNewTotal + newValueArray[i];
									excludedOldTotal = excludedOldTotal + oldValueArray[i];
									spreadToFlag[i] = false;
									excluded++;
								}
							}

							if (excluded == detList.Count)
							{
								throw new NothingToSpreadException();
							}

							oldTotal -= excludedOldTotal;

							if (oldTotal == 0)
							{
								for (i = 0; i < detList.Count; i++)
								{
									if (spreadToFlag[i])
									{
										oldValueArray[i] = 1;
										oldTotal = oldTotal + 1;
									}
								}
							}

							newTotal -= excludedNewTotal;

							for (i = 0; i < detList.Count; i++)
							{
								if (spreadToFlag[i])
								{
									if (oldTotal != 0)
									{
										newValueArray[i] = (int)(newTotal * (Convert.ToDecimal(oldValueArray[i]) / Convert.ToDecimal(oldTotal)));
										oldTotal -= oldValueArray[i];
									}
									else
									{
										newValueArray[i] = 0;
									}

									if (newValueArray[i] != origValueArray[i])
									{
										//newValueArray[i] = intSpreadToDetail(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (AssortmentCellReference)detList[i], newValueArray[i]);
										ToolBox.SetCellValue(aSetCellMode, (AssortmentCellReference)detList[i], (double)newValueArray[i]);
									}

									newTotal -= newValueArray[i];
									spreadTotal += newValueArray[i];
								}
							}

							return spreadTotal;
						}
					}
					else
					{
						throw new NothingToSpreadException();
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
		#region Spreads
		//-------------------------------------------

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//TODO: Modifiy PackToColor Spread to adhere to pack multiples
		//private class clsSpread_PackToColor : clsSpread_BaseMultiple
		//{
		//    public clsSpread_PackToColor(AssortmentViewComputations aComputations, int aKey)
		//        : base(aComputations, aKey, "Spread_PackToColor Spread")
		//    {
		//    }

		//    override public eComputationFormulaReturnType Execute(AssortmentScheduleSpreadEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
		//    {
		//        int packRID;
		//        int numMultiples;
		//        int totalUnits;
		//        double newValue = 0;
		//        AssortmentSpread assrtSpread;
		//        ArrayList compCellValueList;
		//        ArrayList compCellRefList;
		//        int i;
		//        bool tempLocksIgnored;

		//        try
		//        {
		//            packRID = aAssrtSchdEntry.ComputationCellRef[eProfileType.HeaderPack];
		//            if (packRID != int.MaxValue)
		//            {
		//                numMultiples = (int)((ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) /
		//                    aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetPackMultiple(
		//                        aAssrtSchdEntry.ComputationCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType],
		//                        aAssrtSchdEntry.ComputationCellRef[eProfileType.HeaderPack])) + .5);

		//                foreach (AssortmentCellReference cellRef in aAssrtSchdEntry.SpreadCellRefList)
		//                {
		//                    if (!cellRef.isCellLocked &&
		//                        !cellRef.isCellFixed &&
		//                        !cellRef.isCellBlocked &&
		//                        !cellRef.isCellProtected &&
		//                        !cellRef.isCellIneligible &&
		//                        !cellRef.isCellClosed &&

		//                        !cellRef.isCellReadOnly &&
		//                        !cellRef.isCellExcludedFromSpread)
		//                    {
		//                        totalUnits = numMultiples * aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetColorMultiple(
		//                                cellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType],
		//                                cellRef[eProfileType.HeaderPack],
		//                                cellRef[eProfileType.HeaderPackColor]);


		//                        if (!cellRef.isCellCompChanged)
		//                        {
		//                            ToolBox.SetCellValue(aSetCellMode, cellRef, totalUnits);
		//                        }
		//                        else
		//                        {
		//                            if (ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, cellRef, false) != totalUnits)
		//                            {
		//                                throw new CellUnavailableException();
		//                            }
		//                        }
		//                    }
		//                }
		//            }
		//            else
		//            {
		//                newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

		//                assrtSpread = new AssortmentSpread();

		//                assrtSpread.ExecuteSimpleSpread(
		//                    newValue,
		//                    aAssrtSchdEntry.SpreadCellRefList,
		//                    0,
		//                    out compCellValueList,
		//                    out compCellRefList,
		//                    out tempLocksIgnored);

		//                for (i = 0; i < compCellRefList.Count; i++)
		//                {
		//                    ToolBox.SetCellValue(aSetCellMode, (ComputationCellReference)compCellRefList[i], tempLocksIgnored, (double)compCellValueList[i]);
		//                }
		//            }

		//            return eComputationFormulaReturnType.Successful;
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }

		//    override public ArrayList GetSpreadToCellReferenceList(AssortmentCellReference aAssrtCellRef)
		//    {
		//        try
		//        {
		//            return ToolBox.GetSpreadDetailCellRefArray(aAssrtCellRef, false);
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }

		//    override public bool CascadeChangeMethods
		//    {
		//        get
		//        {
		//            return false;
		//        }
		//    }
		//}

		//End TT#2 - JScott - Assortment Planning - Phase 2
		private class clsSpread_ComponentTotal_TotalUnits : clsSpread_BaseMultiple
		{
			public clsSpread_ComponentTotal_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_TotalUnits Spread")
			{
			}
        }

		// Begin TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
        private class clsSpread_ComponentTotal_Reserve : clsSpread_BaseMultiple
        {
            public clsSpread_ComponentTotal_Reserve(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentTotal_Reserve Spread")
            {
            }
        }
		// End TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

		private class clsSpread_ComponentGroupLevel_TotalUnits : clsSpread_BaseMultiple
		{
			public clsSpread_ComponentGroupLevel_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGroupLevel_TotalUnits Spread")
			{
			}
        }

		private class clsSpread_ComponentGrade_TotalUnits : clsSpread_BaseMultiple
		{

			public clsSpread_ComponentGrade_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGrade_TotalUnits Spread")
			{
			}
        }

		private class clsSpread_ComponentGroupLevel_AvgUnits : clsSpread_BaseMultiple
		{
			public clsSpread_ComponentGroupLevel_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGroupLevel_AvgUnits Spread")
			{
			}
        }

		private class clsSpread_ComponentGrade_AvgUnits : clsSpread_BaseMultiple
		{

			public clsSpread_ComponentGrade_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGrade_AvgUnits Spread")
			{
			}
        }

		private class clsSpread_ComponentSubTotal_TotalUnits : clsSpread_BaseMultiple
		{

			public clsSpread_ComponentSubTotal_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentSubTotal_TotalUnits Spread")
			{
			}
        }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Inits
		//-------------------------------------------

		private class clsInit_Null : AssortmentFormulaProfile
		{
			public clsInit_Null(AssortmentViewComputations aComputations, int aKey)
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

		private class clsInit_HeaderColorDetail_TotalUnits : AssortmentFormulaProfile
		{
			public clsInit_HeaderColorDetail_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "HeaderColorDetail_TotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				int packRID;
				int colorRID;

				try
				{
					if (aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.useHeaderAllocation(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]))
					{
						packRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];
						colorRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];
                        
                        // Begin TT#2 - RMatelic - Assortment Planning - header units not displaying in correct grade column on the screen matrix
                        ProfileList sgll = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
						// BEGIN TT#771-MD - Stodd - null reference exception
                        //AllocationProfile ap = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]);
						AllocationProfile ap = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]);
                        //AssortmentProfile asp = (AssortmentProfile)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(ap.AsrtRID);
						AssortmentProfile asp = (AssortmentProfile)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(ap.AsrtRID);
						// END TT#771-MD - Stodd - null reference exception
						ProfileList gradeList = asp.GetAssortmentStoreGrades();	// TT#488-MD - STodd - Group Allocation
                        ProfileList storeList = new ProfileList(eProfileType.Store);
                        foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
                        {
                            if (sglp.Key == aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel])
                            {
                                foreach (StoreGradeProfile sgp in gradeList.ArrayList)
                                {
                                    if (sgp.Key == aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade])
                                    {
                                        ProfileList gradeStoreList = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetStoresInSetGrade(sglp.Key, sgp.Key);
                                        foreach (StoreProfile sp in gradeStoreList)
                                        {
                                            if (sp.Key != aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.ReserveStore.RID)
                                            {
                                                storeList.Add(sp);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // End TT#2

                        if (packRID == int.MaxValue)
						{
							if (colorRID == int.MaxValue)
							{
                                // Begin TT#2 - RMatelic - Assortment Planning - header units not displaying in correct grade column on the screen matrix
								// Begin TT#1225 - stodd - intransit
								//throw new Exception("Unfinished coding");
                                //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                                //    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedUnits(
                                //        aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
                                //        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
                                //        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
								// End TT#1225 - stodd - intransit
                               
                                newValue = ap.GetAllocatedUnits(storeList);
                                // End TT#2
							}
							else
							{
                                // Begin TT#2 - RMatelic - Assortment Planning - header units not displaying in correct grade column on the screen matrix
                                //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                                //    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedColorUnits(
                                //        colorRID,
                                //        aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
                                //        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
                                //        aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
                                
                                newValue = ap.GetAllocatedColorUnits(colorRID, storeList);
                                // End TT#2
							}
						}
						else
						{
                            // Begin TT#2 - RMatelic - Assortment Planning - allocate headers attached to placeholders
                            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                            //        aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedUnits(
                            //            packRID,
                            //            colorRID,
                            //            aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
                            //            aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
                            //            aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
                            // Begin TT#2 - RMatelic - Assortment Planning - header units not displaying in correct grade column on the screen matrix
                            //if (colorRID == int.MaxValue)
                            //{
                            //    newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                            //       aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedPackUnits(
                            //           packRID,
                            //           aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
                            //           aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
                            //           aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
                            //}
                            //else
                            //{
                            //    newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                            //        aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedUnits(
                            //            packRID,
                            //            colorRID,
                            //            aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.CurrentStoreGroupProfile.Key,
                            //            aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGroupLevel],
                            //            aAssrtSchdEntry.AssortmentCellRef[eProfileType.StoreGrade]);
                            //}
                            //// End TT#2

                            if (colorRID == int.MaxValue)
                            {
                                newValue = ap.GetAllocatedPackUnits(packRID, storeList);
                            }
                            else
                            {
                                newValue = ap.GetAllocatedUnits(packRID, colorRID, storeList);
                            }
                            // End TT#2
						}

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

		private class clsInit_ColorDetail_UnitCost : AssortmentFormulaProfile
		{
			public clsInit_ColorDetail_UnitCost(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ColorDetail_UnitCost Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
                    // Begin TT#1513-MD - RMatelic - ASST- Dimension not defined on cube error when drag/drop header onto placeholder
					// BEGIN TT#771-MD - Stodd - null reference exception
					//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
					//	aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType]).UnitCost;
                    //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //    aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType]).UnitCost;
					// END TT#771-MD - Stodd - null reference exception
                    if (aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetDimensionProfileTypeIndex(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType) == -1)
                    {
                    }
                    else
                    {
                        newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                        aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType]).UnitCost;
                 
                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
                    }
                    // End TT#1513-MD

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_ColorDetail_UnitRetail : AssortmentFormulaProfile
		{
			public clsInit_ColorDetail_UnitRetail(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ColorDetail_UnitRetail Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
                    // Begin TT#1513-MD - RMatelic - ASST- Dimension not defined on cube error when drag/drop header onto placeholder
					// BEGIN TT#771-MD - Stodd - null reference exception
					//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
					//	aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType]).UnitRetail;
                    //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //    aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType]).UnitRetail;
					// END TT#771-MD - Stodd - null reference exception

                    if (aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetDimensionProfileTypeIndex(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType) == -1)
                    {
                    }
                    else
                    {
                        newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                                   aAssrtSchdEntry.AssortmentCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType]).UnitRetail;
                        
                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
                    } 
					// End TT#1513-MD

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        private class clsInit_HeaderTotalDetail_HeaderUnits : AssortmentFormulaProfile
        {
			public clsInit_HeaderTotalDetail_HeaderUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "HeaderTotalDetail_HeaderUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				int packRID;
				int colorRID;

				try
				{
					newValue = 0;

					if (!aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.useHeaderAllocation(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]))
					{
						packRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];
						colorRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];

						if (packRID == int.MaxValue)
						{
							if (colorRID == int.MaxValue)
							{
								// BEGIN TT#771-MD - Stodd - null reference exception
								//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                                //    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).TotalUnitsToAllocate;
								newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).TotalUnitsToAllocate;
								// END TT#771-MD - Stodd - null reference exception
                            }
							else
							{
								// BEGIN TT#771-MD - Stodd - null reference exception
								//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
								//    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetColorUnitsToAllocate(
								//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor]);
								newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetColorUnitsToAllocate(
										aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor]);
								// END TT#771-MD - Stodd - null reference exception
							}
						}
						else
						{
							if (colorRID == int.MaxValue)
							{
								// BEGIN TT#771-MD - Stodd - null reference exception
								//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
								//    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetUnitsToAllocateByPack(
								//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack]);
								newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetUnitsToAllocateByPack(
										aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack]);
								// END TT#771-MD - Stodd - null reference exception
							}
							else
							{
								// BEGIN TT#771-MD - Stodd - null reference exception
								//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
								//    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetColorUnitsToAllocateByPack(
								//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack],
								//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor]);
								newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
									aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetColorUnitsToAllocateByPack(
										aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack],
										aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor]);
								// END TT#771-MD - Stodd - null reference exception
							}
						}
					}

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

		private class clsInit_ComponentGradeGroupLevel_AvgUnits : AssortmentFormulaProfile
		{
			public clsInit_ComponentGradeGroupLevel_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGradeGroupLevel_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;

				try
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        //int storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef);
                        int storeCnt = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.NumStoresAllocated, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);


                        if (storeCnt == 0)
                        {
                            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, 0);        // 5555
                        }
                        else
                        {
                            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                                ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, false) / storeCnt);
                        }
                    }
                    else
                    {
                        summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, false) /
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false));

                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
                    aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.SumDetailCellLocks(aAssrtSchdEntry.AssortmentCellRef);	// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need
					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
        private class clsInit_ComponentGroupLevel_NumStoresAlloc : AssortmentFormulaProfile
        {
            public clsInit_ComponentGroupLevel_NumStoresAlloc(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentGroupLevel_NumStoresAlloc Init")
            {
            }

            override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                int storeCnt = 0;
                try
                {
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        int packIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
                        int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                        //int groupLevelIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);
                        //int gradeIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);

                        ArrayList cellRefList = null;
                        ArrayList cellRefList2 = null;
                        ProfileList outStoreList = null;

                        // If the cell has no pack or no header, we can't figure out which component it represents
                        if (packIndex == -1 || headerIndex == -1)
                        {
                            // get the detail cells
                            cellRefList = aAssrtSchdEntry.AssortmentCellRef.GetDetailCellRefArray(eCubeType.AssortmentComponentHeaderGrade, false);
                            // the cells in this cell list do NOT contain header and pack, so we get the details for one of the cells in the list.
                            if (cellRefList.Count > 0)
                            {
                                cellRefList2 = ((AssortmentCellReference)cellRefList[0]).GetDetailCellRefArray(eCubeType.AssortmentHeaderColorDetail, false);
                                // If just one detail cell, then return number of stores base upon it's coordinates.
                                // Note: The details cells contain Grade, which we want to ignore, thus the "true" in the parameters below.
                                if (cellRefList2.Count == 1)
                                {
									// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                                    bool doesSetGradeContainStores = false;
                                    storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation((AssortmentCellReference)cellRefList2[0], true, out outStoreList, out doesSetGradeContainStores);
									// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                                }
                                // If more than one detail cell, we want to count only the distinct stores for each cell's coordinates.
                                else if (cellRefList2.Count > 1)
                                {
                                    storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(cellRefList2, true);
                                }
                                else
                                {
                                    storeCnt = 0;
                                }
                            }
                            else
                            { 
                                storeCnt = 0;
                            }
                        }
                        else
                        {
                            storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef, out outStoreList);
                        }

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, storeCnt);
                    }

                    //aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.SumDetailCellLocks(aAssrtSchdEntry.AssortmentCellRef);	// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        private class clsInit_ComponentGrade_NumStoresAlloc : AssortmentFormulaProfile
        {
            public clsInit_ComponentGrade_NumStoresAlloc(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentGrade_NumStoresAlloc Init")
            {
            }

            override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                int storeCnt = 0;
                ProfileList outStoreList = null;
                try
                {
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        int packIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
                        int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                        //int groupLevelIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);
                        //int gradeIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);

                        ArrayList cellRefList = null;
                        // If the cell has no pack or no header, we can't figure out which component it represents
                        if (packIndex == -1 || headerIndex == -1)
                        {
                            // get the detail cells
                            cellRefList = aAssrtSchdEntry.AssortmentCellRef.GetDetailCellRefArray(eCubeType.AssortmentHeaderColorDetail, false);
                            // If just one detail cell, then return number of stores base upon it's coordinates.
                            if (cellRefList.Count == 1)
                            {
                                storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation((AssortmentCellReference)cellRefList[0], out outStoreList);
                            }
                            // If more than one detail cell, we want to count only the distinct stores for each cell's coordinates.
                            else if (cellRefList.Count > 1)
                            {
                                storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(cellRefList);
                            }
                            else
                            {
                                storeCnt = 0;
                            }
                        }
                        else
                        {
                            storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef, out outStoreList);
                        }

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, storeCnt);
                    }

                    //aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.SumDetailCellLocks(aAssrtSchdEntry.AssortmentCellRef);	// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

		// End TT#4294 - stodd - Average Units in Matrix Enahancement

		private class clsInit_ComponentGradeGroupLevel_Index : AssortmentFormulaProfile
		{
			public clsInit_ComponentGradeGroupLevel_Index(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGradeGroupLevel_Index Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;

				try
				{
					summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) /
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false) * 100);

                    aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.SumDetailCellLocks(aAssrtSchdEntry.AssortmentCellRef);	// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need


					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_ComponentHeaderTotal_HeaderUnits : clsFormula_Sum
		{
			public clsInit_ComponentHeaderTotal_HeaderUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentHeaderTotal_HeaderUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ArrayList cellRefList;
				double newValue;

				try
				{
					if (!aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.useHeaderAllocation(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]))
					{
						cellRefList = aAssrtSchdEntry.AssortmentCellRef.GetDetailCellRefArray(eCubeType.AssortmentHeaderTotalDetail, true);
						newValue = 0;

						foreach (AssortmentCellReference cellRef in cellRefList)
						{
							newValue += ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, cellRef, true);
						}

						ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

						return eComputationFormulaReturnType.Successful;
					}
					else
					{
						return base.Execute(aAssrtSchdEntry, aGetCellMode, aSetCellMode);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_ComponentHeaderTotal_Balance : clsFormula_Sum
		{
			public clsInit_ComponentHeaderTotal_Balance(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentHeaderTotal_Balance Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue = 0;

				try
				{
					// Begin TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 
					// Begin TT#4369 - stodd - Get "Bulk color not defined" error when Pack is removed from heading columns and one of the headers contains a pack color  
                    //int colorIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
                    int packIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
                    int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                    //int styleIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Day);
                    //int parentOfStyleIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.BasisWeek);

                    //if (headerIndex != -1)
                    //{
                    //    AllocationProfile ap = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]);

                    //    if (ap != null)
                    //    {
                    //        // PACK
                    //        if (packIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    //        {
                    //            int packRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];
                    //            PackHdr packHdr = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetPackHdr(packRID);
                    //            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedBalance(packHdr);
                    //            newValue = newValue * packHdr.PackMultiple;
                    //        }
                    //        // COLOR
                    //        else if (colorIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                    //        {
                    //            int colorRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];
                    //            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                            aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedBalance(colorRID);
                    //        }                       
                    //        // TOTAL
                    //        else
                    //        {
                    //            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedBalance(eAllocationSummaryNode.Total);
                    //        }

                    //        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
                    //    }	// TT#3802 
                    //}
                    //// End TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 
                    //else
                    //{
                    //    AllocationProfileList apl = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfileList();
                    //    newValue = 0;

                    //    if (parentOfStyleIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.BasisWeek] != int.MaxValue)
                    //    {
                    //        int parentOfStyleRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.BasisWeek];
                    //        apl = GetHeaderParentOfStyleMatch(parentOfStyleRid, apl);
                    //    }

                    //    if (styleIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.Day] != int.MaxValue)
                    //    {
                    //        int styleRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.Day];
                    //        apl = GetHeaderStyleMatch(styleRid, apl);
                    //    }

                    //    int packRid = -1;
                    //    int colorRid = -1;
                    //    if (colorIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                    //    {
                    //        colorRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];
                    //    }
                    //    if (packIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    //    {
                    //        packRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];
                    //    }

                    //    newValue = GetHeaderPackColorMatch(packRid, colorRid, apl);
                    //    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
                    //}


                    ArrayList cellRefList = null;
                    if (packIndex == -1 || headerIndex == -1)
                    {
                        cellRefList = aAssrtSchdEntry.AssortmentCellRef.GetDetailCellRefArray(eCubeType.AssortmentHeaderTotalDetail, false);

                        // If just one detail cell, then return balances based upon it's coordinates.
                        if (cellRefList.Count == 1)
                        {
                            newValue = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetAllocationBalance((AssortmentCellReference)cellRefList[0]);
                        }
                        // If more than one detail cell, return agregate balance for each cell's coordinates.
                        else if (cellRefList.Count > 1)
                        {
                            newValue = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetAllocationBalance(cellRefList);
                        }
                        else
                        {
                            newValue = 0;
                        }
                    }
                    else
                    {
                        newValue = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetAllocationBalance(aAssrtSchdEntry.AssortmentCellRef);
                    }

                    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
					// End TT#4369 - stodd - Get "Bulk color not defined" error when Pack is removed from heading columns and one of the headers contains a pack color  
                    return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

            private AllocationProfileList GetHeaderParentOfStyleMatch(int parentOfStyleRid, AllocationProfileList apl)
            {
                AllocationProfileList parentOfStyleList = new AllocationProfileList(eProfileType.AssortmentMember);

                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }

                    HierarchyNodeProfile hnParent = ap.AppSessionTransaction.GetParentNodeData(ap.StyleHnRID);

                    if (hnParent.Key == parentOfStyleRid)
                    {
                        parentOfStyleList.Add(ap);
                    }
                }

                return parentOfStyleList;
            }

            private AllocationProfileList GetHeaderStyleMatch(int styleRid, AllocationProfileList apl)
            {
                AllocationProfileList styleList = new AllocationProfileList(eProfileType.AssortmentMember);


                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }

                    if (ap.StyleHnRID == styleRid)
                    {
                        styleList.Add(ap);
                    }
                }

                return styleList;
            }

            private double GetHeaderPackColorMatch(int packRid, int colorRid, AllocationProfileList apl)
            {
                double newValue = 0;

                // PACK
                if (packRid != -1)
                {

                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }

                        PackHdr packHdr = ap.GetPackHdr(packRid);

                        if (packHdr != null)
                        {
                            double subValue = ap.GetAllocatedBalance(packHdr);
                            newValue += subValue * packHdr.PackMultiple;
                        }
                    }
                }
                // COLOR
                else if (colorRid != -1)
                {
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }

                        if (ap.BulkColorIsOnHeader(colorRid))
                        {
                            newValue += ap.GetAllocatedBalance(colorRid);
                        }
                    }
                }
                // TOTAL
                else
                {
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }
                        newValue += ap.GetAllocatedBalance(eAllocationSummaryNode.Total);
                    }
                }


                return newValue;
            }



		} //====

       

		// Begin TT#1225 - stodd - intransit
		private class clsInit_ComponentHeaderTotal_Intransit : AssortmentFormulaProfile
		{
			public clsInit_ComponentHeaderTotal_Intransit(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentHeaderTotal_Intransit Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try

				{
					// Begin TT#1224 - stodd - committed
					DayProfile applyToDay = ((AssortmentProfile)((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).DefaultAllocationProfile).AssortmentApplyToDate;
					// End TT#1224 - stodd - committed
					int colorIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
					if (headerIndex != -1)
					{
					// END TT#2148 - stodd - Assortment totals do not include header values
						// BEGIN TT#771-MD - Stodd - null reference exception
						//int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//                    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).StyleHnRID;
						int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
											aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).StyleHnRID;
						// END TT#771-MD - Stodd - null reference exception
						if (colorIndex == -1 || aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor] == int.MaxValue)
						{
							IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							// Begin TT#1224 - stodd - committed
							// BEGIN TT#771-MD - Stodd - null reference exception
							//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
							//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalInTransit(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, styleHnRid, applyToDay);
							newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
											aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalInTransit(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, styleHnRid, applyToDay);

							// END TT#771-MD - Stodd - null reference exception
							// End TT#1224 - stodd - committed
						}
						else
						{
							IntransitKeyType ikt = new IntransitKeyType(aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor], Include.IntransitKeyTypeNoSize);
							// Begin TT#1224 - stodd - committed
							// BEGIN TT#771-MD - Stodd - null reference exception
							//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
							//				aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalInTransit(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, styleHnRid, applyToDay);
							newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
											aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalInTransit(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, styleHnRid, applyToDay);
							// END TT#771-MD - Stodd - null reference exception
							// End TT#1224 - stodd - committed
						}

						ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					}
					// END TT#2148 - stodd - Assortment totals do not include header values

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// End TT#1225 - stodd - intransit

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsInit_ComponentHeaderTotal_OnHand : AssortmentFormulaProfile
		{
			public clsInit_ComponentHeaderTotal_OnHand(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentHeaderTotal_OnHand Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					//DayProfile applyToDay = ((AssortmentProfile)((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).DefaultAllocationProfile).AssortmentApplyToDate;
					int colorIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
					int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
					if (headerIndex != -1)
					{
						//int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//                    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).StyleHnRID;
						if (colorIndex == -1 || aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor] == int.MaxValue)
						{
							IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							// BEGIN TT#771-MD - Stodd - null reference exception
							//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
							//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);
							// Begin TT#1120-MD - stodd - OnHand no footing to anything - 
                            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                            //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);

                            AllocationProfile ap = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]);
                            AllocationSubtotalProfile asp = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationGrandTotalProfile();
                            newValue = asp.GetStoreListTotalOnHand(ap, ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);
							// End TT#1120-MD - stodd - OnHand no footing to anything - 

							// END TT#771-MD - Stodd - null reference exception
						}
						else
						{
                            int color = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];
							IntransitKeyType ikt = new IntransitKeyType(aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor], Include.IntransitKeyTypeNoSize);
							// BEGIN TT#771-MD - Stodd - null reference exception
							//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
							//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);
							// Begin TT#1120-MD - stodd - OnHand no footing to anything - 
                            AllocationProfile ap = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]);
                            AllocationSubtotalProfile asp = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationGrandTotalProfile();
                            newValue = asp.GetStoreListTotalOnHand(ap, ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);

                            //ap.OnHandHnRID = ap.StyleHnRID;
                            //newValue = ap.GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);

                            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                            //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);
							// End TT#1120-MD - stodd - OnHand no footing to anything - 
							// END TT#771-MD - Stodd - null reference exception
						}

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

		private class clsInit_ComponentHeaderTotal_Reserve : AssortmentFormulaProfile
		{
            private Index_RID reserveStore;	// TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix

			public clsInit_ComponentHeaderTotal_Reserve(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentHeaderTotal_Reserve Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					// Begin TT#4369 - stodd - Get "Bulk color not defined" error when Pack is removed from heading columns and one of the headers contains a pack color  
					//DayProfile applyToDay = ((AssortmentProfile)((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).DefaultAllocationProfile).AssortmentApplyToDate;
					//int colorIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
					// BEGIN TT#832-MD - Stodd - reserve units not displayed
					int packIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
					int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
					// Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    //int styleIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Day);
                    //int parentOfStyleIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.BasisWeek);
					// End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    //reserveStore = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.ReserveStore;
                    //if (reserveStore.RID == Include.NoRID)
                    //{
                    //    newValue = 0;
                    //}
                    //else if (headerIndex != -1)
                    //{
                    //    //int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                    //    //                    aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).StyleHnRID;
                    //    // Begin TT#3802 - stodd - error when removing header from group allocation - 
                    //    AllocationProfile ap = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]);
                    //    // COLOR
                    //    if (ap != null)
                    //    {
                    //    // End TT#3802 - stodd - error when removing header from group allocation - 
                    //        // PACK
                    //        if (packIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    //        {
                    //            int packRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];
                    //            PackHdr packHdr = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetPackHdr(packRID);
                    //            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreQtyAllocated(packHdr.PackName, reserveStore);
                    //            newValue = newValue * packHdr.PackMultiple;
                    //        }
                    //        // Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    //        // Color
                    //        else if (colorIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                    //        {
                    //            int colorRID = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];
                    //            // BEGIN TT#771-MD - Stodd - null reference exception
                    //            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                    //            //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetReserveQty(colorRID);
                    //            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //            //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetReserveQty(colorRID);
                    //            // END TT#771-MD - Stodd - null reference exception
                    //            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                            aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreQtyAllocated(colorRID, reserveStore);
                    //        }
                    //        // End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    //        // TOTAL
                    //        else
                    //        {
                    //            //IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
                    //            // BEGIN TT#771-MD - Stodd - null reference exception
                    //            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
                    //            //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetReserveQty(eAllocationSummaryNode.Total);
                    //            //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //            //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetReserveQty(eAllocationSummaryNode.Total);
                    //            // END TT#771-MD - Stodd - null reference exception
                    //            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                    //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore);
                    //        }
                    //        // END TT#832-MD - Stodd - reserve units not displayed

                    //        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
                    //    }	// TT#3802 
                    //}
                    //// Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                    //else
                    //{
                    //    AllocationProfileList apl = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfileList();
                    //    newValue = 0;

                    //    if (parentOfStyleIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.BasisWeek] != int.MaxValue)
                    //    {
                    //        int parentOfStyleRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.BasisWeek];
                    //        apl = GetHeaderParentOfStyleMatch(parentOfStyleRid, apl);
                    //    }

                    //    if (styleIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.Day] != int.MaxValue)
                    //    {
                    //        int styleRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.Day];
                    //        apl = GetHeaderStyleMatch(styleRid, apl);
                    //    }

                    //    int packRid = -1;
                    //    int colorRid = -1;
                    //    if (colorIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                    //    {
                    //        colorRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPackColor];
                    //    }
                    //    if (packIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    //    {
                    //        packRid = aAssrtSchdEntry.AssortmentCellRef[eProfileType.HeaderPack];
                    //    }

                    //    newValue = GetHeaderPackColorMatch(packRid, colorRid, apl);
                    //    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

                    //    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
                    //}
					// End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix

                    ArrayList cellRefList = null;
                    if (packIndex == -1 || headerIndex == -1)
                    {
                        cellRefList = aAssrtSchdEntry.AssortmentCellRef.GetDetailCellRefArray(eCubeType.AssortmentHeaderTotalDetail, false);

                        // If just one detail cell, then return balances based upon it's coordinates.
                        if (cellRefList.Count == 1)
                        {
                            newValue = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetReserve((AssortmentCellReference)cellRefList[0]);
                        }
                        // If more than one detail cell, return agregate balance for each cell's coordinates.
                        else if (cellRefList.Count > 1)
                        {
                            newValue = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetReserve(cellRefList);
                        }
                        else
                        {
                            newValue = 0;
                        }
                    }
                    else
                    {
                        newValue = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetReserve(aAssrtSchdEntry.AssortmentCellRef);
                    }

                    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);
					// End TT#4369 - stodd - Get "Bulk color not defined" error when Pack is removed from heading columns and one of the headers contains a pack color  
					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			// Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
            private AllocationProfileList GetHeaderParentOfStyleMatch(int parentOfStyleRid, AllocationProfileList apl)
            {
                AllocationProfileList parentOfStyleList = new AllocationProfileList(eProfileType.AssortmentMember);

                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }

                    HierarchyNodeProfile hnParent = ap.AppSessionTransaction.GetParentNodeData(ap.StyleHnRID);

                    if (hnParent.Key == parentOfStyleRid)
                    {
                        parentOfStyleList.Add(ap);
                    }
                }

                return parentOfStyleList;
            }

            private AllocationProfileList GetHeaderStyleMatch(int styleRid, AllocationProfileList apl)
            {
                AllocationProfileList styleList = new AllocationProfileList(eProfileType.AssortmentMember);


                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }

                    if (ap.StyleHnRID == styleRid)
                    {
                        styleList.Add(ap);
                    }
                }

                return styleList;
            }

            private double GetHeaderPackColorMatch(int packRid, int colorRid, AllocationProfileList apl)
            {
                double newValue = 0;

                // PACK
                if (packRid != -1)
                {

                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }

                        PackHdr packHdr = ap.GetPackHdr(packRid);

                        if (packHdr != null)
                        {
                            double subValue = ap.GetStoreQtyAllocated(packHdr.PackName, reserveStore);
                            newValue += subValue * packHdr.PackMultiple;
                        }
                    }
                }
                // COLOR
                else if (colorRid != -1)
                {
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }

                        if (ap.BulkColorIsOnHeader(colorRid))
                        {
                            newValue += ap.GetStoreQtyAllocated(colorRid, reserveStore);
                        }
                    }
                }
                // TOTAL
                else
                {
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }

                        newValue += ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore);
                    }
                }


                return newValue;
            }
		}
		// End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
		// END TT#2148 - stodd - Assortment totals do not include header values

		// Begin TT#1224 - stodd - Committed
		private class clsInit_ComponentHeaderTotal_Committed : AssortmentFormulaProfile
		{
			public clsInit_ComponentHeaderTotal_Committed(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentHeaderTotal_Committed Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					newValue = 0;
					//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
					//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).TotalUnitsToAllocate;
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
		// End TT#1224 - stodd - Committed

		private class clsInit_ComponentTotal_TotalCost : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_TotalCost(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_TotalCost Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                    //    ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, false) *
                    //    ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitCost, false));

                    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                        ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) *
                        ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitCost, aAssrtSchdEntry.AssortmentCellRef.isCellHidden));
                    // End TT#1498-MD
					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//BEGIN TT#441-MD - stodd - Total Retail and Total Cost are not correct 
		// These are currently not used, but I left them in in case they are needed in the future.
		private class clsInit_ComponentTotal_TotalAvgTotalUnitCost : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_TotalAvgTotalUnitCost(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_TotalAvgTotalUnitCost Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalCost, false) /
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, false)
						);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_ComponentTotal_TotalAvgTotalUnitRetail : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_TotalAvgTotalUnitRetail(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_TotalAvgTotalUnitRetail Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalRetail, false) /
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, false)
						);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		//END TT#441-MD - stodd - Total Retail and Total Cost are not correct 

		private class clsInit_ComponentTotal_TotalRetail : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_TotalRetail(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_TotalRetail Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				try
				{
                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    //double totUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                    //double unitRetail = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitRetail, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                        ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) *
                        ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitRetail, aAssrtSchdEntry.AssortmentCellRef.isCellHidden));
                    // End TT#1498-MD
					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_ComponentTotal_MUPct : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_MUPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_MUPct Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double unitRetail;
                double totalRetail;  // TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
				double newValue;

				try
				{
                    // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                    eCubeType cubeType = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType;
                    if (cubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal || cubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal)
                    {
                        totalRetail = ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalRetail, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        newValue = ((totalRetail - ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalCost, aAssrtSchdEntry.AssortmentCellRef.isCellHidden)) / totalRetail) * 100;
                    }
                    else
                    {
                        // Begin TT#1498-MD - RMatelic - ASST - MU% not calcing on the fly for the Detail section
                        //unitRetail = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitRetail, false);
                        //newValue = ((unitRetail - ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitCost, false)) / unitRetail) * 100;
                        unitRetail = ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitRetail, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        newValue = ((unitRetail - ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.UnitCost, aAssrtSchdEntry.AssortmentCellRef.isCellHidden)) / unitRetail) * 100;
                        // End TT#1498-MD 
                    }
                    // End TT#1498-MD 
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

		// Begin TT#1224 - stodd - Committed
		private class clsInit_ComponentTotal_Committed : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_Committed(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_Committed Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue = 0;

				try
				{
					//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
					//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).TotalUnitsToAllocate;

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
		// End TT#1224 - stodd - Committed

		// Begin TT#1225 - stodd - intransit
		private class clsInit_ComponentTotal_Intransit : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_Intransit(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_Intransit Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					DayProfile applyToDay = ((AssortmentProfile)((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).DefaultAllocationProfile).AssortmentApplyToDate;
					int placeholderIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);
					if (placeholderIndex == -1)
					{
						newValue = 0;
					}
					else
					{
						IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
						// BEGIN TT#771-MD - Stodd - null reference exception
						//int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//        aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).StyleHnRID;
						int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
								aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).StyleHnRID;
						//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetStoreListTotalInTransit(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, styleHnRid, applyToDay);
						newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
										aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetStoreListTotalInTransit(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, styleHnRid, applyToDay);
						// END TT#771-MD - Stodd - null reference exception
					}

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
		// End TT#1225 - stodd - intransit

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsInit_ComponentTotal_OnHand : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_OnHand(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_OnHand Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;

				try
				{
					//DayProfile applyToDay = ((AssortmentProfile)((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).DefaultAllocationProfile).AssortmentApplyToDate;
					int placeholderIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);
					if (placeholderIndex == -1)
					{
						newValue = 0;
					}
					else
					{
						IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
						//int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).StyleHnRID;

						// BEGIN TT#771-MD - Stodd - null reference exception
						//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);
                        // Begin TT#1976-MD - JSmith - Matrix Tab - On Hand cannot validate
                        //newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                        //                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID);

                        int OnHandHnRID = Include.NoRID;

                        AllocationProfile assortmentMemberProfile = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                                        aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]);

                        HierarchyNodeProfile styleNode = assortmentMemberProfile.AppSessionTransaction.GetNodeData(assortmentMemberProfile.StyleHnRID);

                        if (!styleNode.IsVirtual)
                        {
                            OnHandHnRID = assortmentMemberProfile.StyleHnRID;
                        }
                        newValue = assortmentMemberProfile.GetStoreListTotalOnHand(ikt, Include.AllStoreGroupRID, Include.AllStoreGroupLevelRID, OnHandHnRID);
                        // End TT#1976-MD - JSmith - Matrix Tab - On Hand cannot validate
						// END TT#771-MD - Stodd - null reference exception
					}

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

		private class clsInit_ComponentTotal_Reserve : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_Reserve(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_Reserve Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue = 0;	// TT#1229-MD - stodd - cascade unlock - 

				try
				{
					//DayProfile applyToDay = ((AssortmentProfile)((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).DefaultAllocationProfile).AssortmentApplyToDate;
					int placeholderIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);
                    int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);	// TT#1229-MD - stodd - cascade unlock - 
					// BEGIN TT#832-MD - Stodd - reserve units not displayed
					Index_RID reserveStore = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.ReserveStore;
					// END TT#832-MD - Stodd - reserve units not displayed
					if (placeholderIndex == -1)
					{
						newValue = 0;
					}
					else
					{
						//IntransitKeyType ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
						//int styleHnRid = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).StyleHnRID;

						// BEGIN TT#771-MD - Stodd - null reference exception
						//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAllocationProfile(
						//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetReserveQty(eAllocationSummaryNode.Total);
						// BEGIN TT#832-MD - Stodd - reserve units not displayed
						//newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
						//                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetReserveQty(eAllocationSummaryNode.Total);
						// END TT#771-MD - Stodd - null reference exception
						// Begin TT#864 - MD - stodd - obj ref adding headers to post receipt - 
                        if (headerIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader] != int.MaxValue)	// TT#1229-MD - stodd - cascade unlock - 
						{
							newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
								aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore);
						}
						else
						{
							newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
								aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore);
						}
						// End TT#864 - MD - stodd - obj ref adding headers to post receipt - 
						// END TT#832-MD - Stodd - reserve units not displayed
					}

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
		// END TT#2148 - stodd - Assortment totals do not include header values

		// Begin TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 
        private class clsInit_ComponentTotal_Balance : AssortmentFormulaProfile
        {
            public clsInit_ComponentTotal_Balance(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentTotal_Balance Init")
            {
            }

            override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                double newValue = 0;	// TT#1229-MD - stodd - cascade unlock - 

                try
                {
                    int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);	// TT#1229-MD - stodd - cascade unlock - 
                    int placeholderIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);
                    if (placeholderIndex == -1)
                    {
                        newValue = 0;
                    }
                    else
                    {
                        if (headerIndex != -1 && aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader] != int.MaxValue)	// TT#1229-MD - stodd - cascade unlock - 
                        {
                            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                                aAssrtSchdEntry.AssortmentCellRef[eProfileType.AllocationHeader]).GetAllocatedBalance(eAllocationSummaryNode.Total);
                        }
                        else
                        {
                            newValue = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.Transaction.GetAssortmentMemberProfile(
                                aAssrtSchdEntry.AssortmentCellRef[eProfileType.PlaceholderHeader]).GetAllocatedBalance(eAllocationSummaryNode.Total);
                        }
                    }

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
		// End TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated - 

		private class clsInit_ComponentTotal_AvgUnits : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;

				try
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        //int storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef);
                        int storeCnt = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.NumStoresAllocated, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);


                        if (storeCnt == 0)
                        {
                            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, 0);    // 7777

                        }
                        else
                        {
                            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                                ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, false) / storeCnt);
                        }
                    }
                    else
                    {
                        summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, false) /
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false));
                    }
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
        private class clsInit_ComponentTotal_NumStoresAlloc : AssortmentFormulaProfile
        {
            public clsInit_ComponentTotal_NumStoresAlloc(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "ComponentTotal_NumStoresAlloc Init")
            {
            }

            override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                int storeCnt = 0;
                ProfileList outStoreList = null;

                try
                {
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        int packIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
                        int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);

                        ArrayList cellRefList = null;
                        if (packIndex == -1 || headerIndex == -1)
                        {
                            cellRefList = aAssrtSchdEntry.AssortmentCellRef.GetDetailCellRefArray(eCubeType.AssortmentHeaderTotalDetail, false);
	
                            // If just one detail cell, then return number of stores base upon it's coordinates.
                            if (cellRefList.Count == 1)
                            {
                                storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation((AssortmentCellReference)cellRefList[0], out outStoreList);
                            }
                            // If more than one detail cell, we want to count only the distinct stores for each cell's coordinates.
                            else if (cellRefList.Count > 1)
                            {
                                storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(cellRefList);
                            }
                            else
                            {
                                storeCnt = 999;  
                            }
                        }
                        else
                        {
                            storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef, out outStoreList);
                        }
                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, storeCnt);
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
		// End TT#4294 - stodd - Average Units in Matrix Enahancement

		private class clsInit_SummaryGradeGroupLevel_AvgUnits : AssortmentFormulaProfile
		{
			public clsInit_SummaryGradeGroupLevel_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "SummaryGradeGroupLevel_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube totCube;

				try
				{
					totCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetTotalCubeType());

					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					//ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
					//    ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentDetailVariables.TotalUnits, false) /
					//    ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentSummaryVariables.NumStores, false));
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentDetailVariables.AvgUnits, false));
					//End TT#1196 - JScott - Average units in the summary section should spread when changed

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class clsInit_SummaryTotal_AvgUnits : AssortmentFormulaProfile
		{
			public clsInit_SummaryTotal_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "SummaryTotal_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube totCube;

				try
				{
					totCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetTotalCubeType());

					//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
					//ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
					//    ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentTotalVariables.TotalUnits, false) /
					//    ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentSummaryVariables.NumStores, false));
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentTotalVariables.AvgUnits, false));
					//End TT#1196 - JScott - Average units in the summary section should spread when changed

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
			public clsInit_SumDetail(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "SumDetail Init")
			{
			}
		}


        private class clsInit_AvgDetail : clsFormula_AverageValue
		{
			public clsInit_AvgDetail(AssortmentViewComputations aComputations, int aKey)
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

		private class clsInit_ComponentGradeGroupLevel_TotalPct : AssortmentFormulaProfile
		{
			public clsInit_ComponentGradeGroupLevel_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentGradeGroupLevel_TotalPct Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue = 0, itemValue=0 ;
				double totValue = 0;
				// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                AssortmentCellReference totCellRef;
                AssortmentCube totCube;
				// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.


				try
				{
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    //Debug.WriteLine("clsInit_ComponentGradeGroupLevel_TotalPct detail CUBE " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + " LVL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level
                        //+ " KEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys);

                    int headerIndex = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);

                    if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotal
                       || aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotal)
                    {
                        itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        if (itemValue > 0)
                        {
                            newValue = 100;
                        }
                    }
                    else
                    {
                        //totValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, eCubeType.AssortmentSubTotal, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

                        totCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSubTotalCubeType());
                        //Debug.WriteLine("clsInit_ComponentGradeGroupLevel_TotalPct total1 CUBE " + totCube.CubeType.Id + " LVL " + totCube.CubeType.Level);
                        totCellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);


                        if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal
                            || aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
                        {
                            if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            else
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }

                            newValue = itemValue / totValue * 100;

                            //Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
                            //    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
                            //    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total_TotalPct1");

                        }
                        else
                        {
                            if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            else
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }

                            itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            newValue = itemValue / totValue * 100;

                            //Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
                            //    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
                            //    " TOTAL CUBETYPE/LEVEL " + totCube.CubeType.Id + "/" + totCube.CubeType.Level +
                            //    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total_TotalPct2");
                        }
                    }
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.

                    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

					//Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue + " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_ComponentGradeGroupLevel_TotalPct");

                    aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.SumDetailCellLocks(aAssrtSchdEntry.AssortmentCellRef);	// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need


					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
        private bool IsTotalVariable(AssortmentCellReference cellRef)
        {
            bool isTotal = false;
            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.AssortmentTotalVariable) > -1)
            {
                isTotal = true;
            }

            return isTotal;
        }
		// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.

		private class clsInit_ComponentTotal_TotalPct : AssortmentFormulaProfile
		{
			public clsInit_ComponentTotal_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "ComponentTotal_TotalPct Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue = 0, itemValue = 0;
				double totValue;
				// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                AssortmentCellReference totCellRef;
                AssortmentCube totCube;
				// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.

				try
				{
					// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotal
                        || aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotal)
                    {
                         itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                         if (itemValue > 0)
                         {
						 	// Begin TT#3772 - stodd - Total % in matrix Showing Incorrect - 
                            totCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSubTotalCubeType());
                            totCellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);
                            if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            else
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }

                            newValue = itemValue / totValue * 100;
							// End TT#3772 - stodd - Total % in matrix Showing Incorrect - 
                         }
                    }
                    else
                    {
                        //Debug.WriteLine("clsInit_ComponentTotal_TotalPct detail CUBE " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + " LVL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level);
                        //totValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, eCubeType.AssortmentSubTotal, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        //itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        //newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) / totValue * 100;

                        totCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSubTotalCubeType());
                        totCellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);
                        //Debug.WriteLine("clsInit_ComponentTotal_TotalPct total1 CUBE " + totCube.CubeType.Id + " LVL " + totCube.CubeType.Level);


                        if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal
                            || aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
                        {
                            if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            else
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

                            newValue = itemValue / totValue * 100;

                            //Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
                            //    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
                            //    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_TotalPct1");
                        }
                        else
                        {
                            if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            else
                            {
                                totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            }
                            itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

                            newValue = itemValue / totValue * 100;

                            //Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
                            //    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
                            //    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_TotalPct2");
                        }
                    }
					// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
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

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsInit_TotalPct : AssortmentFormulaProfile
		{
			public clsInit_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "TotalPct Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue, oldValue;
				double totValue, itemValue;
				AssortmentCellReference totCellRef;
				AssortmentCube totCube;

				try
				{
                    aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.InitCellValue(aAssrtSchdEntry.AssortmentCellRef);	// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need


					totCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetTotalCubeType());
					totCellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

					if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal
						|| aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
					{
						// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
						// Tells us this total is for a header (and not a placeholder)
						if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
						{
							totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						}
						else
						{
							totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						}
						// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
						itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

						newValue = itemValue / totValue * 100;

						//eCubeType cubeType = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType;
						////Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue + " (" + altTotValue + " / " + altTotValue1 + ") " +
						//Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
						//    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
						//    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_TotalPct1");
					}
					else
					{
						// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
						// Tells us this total is for a header (and not a placeholder)
						if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
						{
							totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						}
						else
						{
                            totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						}
						// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
                        itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

						// Begin TT#3750 - stodd - "Total %" not locking
                        if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                        {
                            if (itemValue > 0 && totValue > 0)
                            {
                                newValue = 100.00;
                            }
                            else
                            {
                                newValue = itemValue / totValue * 100;
                            }
                        }
                        else
                        {
                            newValue = itemValue / totValue * 100;
                        }
						// End TT#3750 - stodd - "Total %" not locking
						
						//eCubeType cubeType = aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType;
						////Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue + " (" + altTotValue + " / " + altTotValue1 + ") " +
						//Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
						//    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
						//    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_TotalPct2");
					}
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
		// END TT#2148 - stodd - Assortment totals do not include header values

		private class clsInit_PctToSet : AssortmentFormulaProfile
		{
			public clsInit_PctToSet(AssortmentViewComputations aComputations, int aKey)
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
			public clsInit_PctToAll(AssortmentViewComputations aComputations, int aKey)
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

		private class clsInit_Balance : AssortmentFormulaProfile
		{
			public clsInit_Balance(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Balance Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				AssortmentCellReference smryCellRef;
				AssortmentCube smryCube;

				try
				{
					smryCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
					smryCellRef = (AssortmentCellReference)smryCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);


					// BEGIN TT#2148 - stodd - Assortment totals do not include header values
					newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, smryCellRef, AssortmentSummaryVariables.Units, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) -
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

					// END TT#2148 - stodd - Assortment totals do not include header values
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

		// BEGIN TT#2148 - stodd - Assortment totals do not include header values
		private class clsInit_Total : AssortmentFormulaProfile
		{
			public clsInit_Total(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Total Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				AssortmentCellReference hdrCellRef;
				AssortmentCube hdrCube;
				AssortmentTotalVariableProfile totalVarProf;
				AssortmentDetailVariableProfile detailVarProf;

				try
				{
					hdrCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetComponentHeaderCubeType());
					hdrCellRef = (AssortmentCellReference)hdrCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);
					if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal)
					{
						totalVarProf = (AssortmentTotalVariableProfile)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AssortmentTotalVariable]);

						//newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, hdrCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) +
						//           ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

						//double hdrVal = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, hdrCellRef, detailVarProf, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						double hdrVal = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, hdrCellRef, totalVarProf, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						double phVal = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						newValue = hdrVal + phVal;
                        //Debug.WriteLine(" TOT1 Cube " + hdrCube.ToString() + " Value " + newValue + " hdr " + hdrVal + " PH " + phVal + " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total");


						//newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, hdrCellRef, detailVarProf, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) +
						//            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

					}
					else
					{
						detailVarProf = (AssortmentDetailVariableProfile)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey(aAssrtSchdEntry.AssortmentCellRef[eProfileType.AssortmentDetailVariable]);

						double hdrVal = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, hdrCellRef, detailVarProf, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						double phVal = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

						newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, hdrCellRef, detailVarProf, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) +
								   ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        //Debug.WriteLine(" TOT2 Cube " + hdrCube.ToString() + " Value " + newValue + " hdr " + hdrVal + " PH " + phVal + " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total");


					}
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

		private class clsInit_AvgUnits : AssortmentFormulaProfile
		{
			public clsInit_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "AvgtUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue, total, numStores;
				AssortmentCellReference hdrCellRef, asrtCellRef;
				AssortmentCube hdrCube;
				//AssortmentCellReference smryCellRef;
				AssortmentCube summCube;
                ProfileList outStoreList = null;	// Begin TT#4294 - stodd - Average Units in Matrix Enahancement

				try
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal)
                        {
                            total = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }
                        else
                        {
                            total = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }
                        asrtCellRef = (AssortmentCellReference)aAssrtSchdEntry.AssortmentCellRef.Copy();
                        asrtCellRef[aAssrtSchdEntry.AssortmentCellRef.QuantityVariableProfileType] = AssortmentQuantityVariables.Value.Key;

                        //numStores = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(asrtCellRef), AssortmentSummaryVariables.NumStores, false);
                        numStores = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef, out outStoreList);
                        if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal)
                        {
                            numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.NumStoresAllocated, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }
                        else
                        {
                            numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.NumStoresAllocated, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }

                        if (numStores == 0)
                        {
                            newValue = 0;   // 9999

                        }
                        else
                        {
                            newValue = total / numStores;

                        }

                    }
                    else
                    {
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
                        summCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
                        //smryCellRef = (AssortmentCellReference)smryCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

                        hdrCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetComponentHeaderCubeType());
                        hdrCellRef = (AssortmentCellReference)hdrCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);
                        if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal)
                        {
                            total = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            asrtCellRef = (AssortmentCellReference)aAssrtSchdEntry.AssortmentCellRef.Copy();
                            asrtCellRef[aAssrtSchdEntry.AssortmentCellRef.QuantityVariableProfileType] = AssortmentQuantityVariables.Value.Key;
                            numStores = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(asrtCellRef), AssortmentSummaryVariables.NumStores, false);
                            newValue = total / numStores;
                        }
                        else
                        {
                            total = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            asrtCellRef = (AssortmentCellReference)aAssrtSchdEntry.AssortmentCellRef.Copy();
                            asrtCellRef[aAssrtSchdEntry.AssortmentCellRef.QuantityVariableProfileType] = AssortmentQuantityVariables.Value.Key;
                            numStores = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(asrtCellRef), AssortmentSummaryVariables.NumStores, false);
                            newValue = total / numStores;
                        }
                    }	// TT#4294 - stodd - Average Units in Matrix Enahancement
                    
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

		private class clsInit_TotalPctToAll : AssortmentFormulaProfile
		{
			public clsInit_TotalPctToAll(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "TotalPctToAll Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double newValue;
				double allValue;

				try
				{
					allValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetComponentTotalCubeType(), AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) / allValue * 100;
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

		private class clsInit_Total_TotalPct : AssortmentFormulaProfile
		{
			public clsInit_Total_TotalPct(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Total_TotalPct Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				//double oldValue;
				//AssortmentCellReference summCellRef;
				//AssortmentCube summCube;
				double newValue;
				double totValue, itemValue;
				AssortmentCellReference totCellRef;
				AssortmentCube totCube;

				try
				{

					//totValue = ToolBox.GetTotalOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, eCubeType.AssortmentSubTotal, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					//itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					//newValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden) / totValue * 100;
					//ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

					// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
					//totCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetTotalCubeType());
					totCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetTotalCubeType());	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
					// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
					totCellRef = (AssortmentCellReference)totCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

					if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal
						|| aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
					{
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
						// Begin TT#1240 - stodd - Error when opening assortment with Total % selected - 
                        //if (totCube.HeaderProfileType == eProfileType.AllocationHeader || totCube.HeaderProfileType == eProfileType.PlaceholderHeader)
                        if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                        {
                            totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            //itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                            itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

                        }
                        else
                        {
                            totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

                            //totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

                            itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }

                        //itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						// End TT#1240 - stodd - Error when opening assortment with Total % selected - 
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        newValue = itemValue / totValue * 100;

						//Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
						//    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
						//    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total_TotalPct1");

					}
					else
					{
						// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        if (totCube.HeaderProfileType == eProfileType.AllocationHeader)
                        {
                            totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }
                        else
                        {
                            totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, totCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
                        }
						// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                        itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
						newValue = itemValue / totValue * 100;

						//Debug.WriteLine(" TotalPct " + newValue + " ITEM " + itemValue + " TOTAL " + totValue +
						//    " CUBETYPE/LEVEL " + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Id + "/" + aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType.Level +
						//    " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total_TotalPct2");

					}
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}

				//summCube = (AssortmentCube)aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
				//summCellRef = (AssortmentCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

				//try
				//{
				//    if (aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentPlaceholderTotalSubTotal
				//        || aAssrtSchdEntry.AssortmentCellRef.Cube.CubeType == eCubeType.AssortmentComponentHeaderTotalSubTotal)
				//    {
				//        totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, summCellRef, AssortmentSummaryVariables.Units, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
				//        itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
				//        newValue = itemValue / totValue * 100;

				//        Debug.WriteLine("ITEM " + itemValue + " TOTAL " + totValue + " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total_TotalPct");

				//    }
				//    else
				//    {
				//        totValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, summCellRef, AssortmentSummaryVariables.Units, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
				//        itemValue = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, AssortmentQuantityVariables.Total, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
				//        newValue = itemValue / totValue * 100;

				//        Debug.WriteLine("ITEM " + itemValue + " TOTAL " + totValue + " CELLKEYS " + aAssrtSchdEntry.AssortmentCellRef.CellKeys + " CLASS " + "clsInit_Total_TotalPct");

				//    }
				//    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

				//    return eComputationFormulaReturnType.Successful;
				//}
				//catch (Exception exc)
				//{
				//    string message = exc.ToString();
				//    throw;
				//}
			}
		}
		// END TT#2148 - stodd - Assortment totals do not include header values

		private class clsInit_Difference : AssortmentFormulaProfile
		{
			public clsInit_Difference(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Difference Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				AssortmentCellReference asrtCellRef;
				ArrayList hdrList;
				double totalHdrVal;

				try
				{
					asrtCellRef = (AssortmentCellReference)aAssrtSchdEntry.AssortmentCellRef.Copy();
					asrtCellRef[aAssrtSchdEntry.AssortmentCellRef.QuantityVariableProfileType] = AssortmentQuantityVariables.Value.Key;
					hdrList = asrtCellRef.GetDetailCellRefArray(asrtCellRef.AssortmentCube.GetComponentHeaderCubeType(), false);
					totalHdrVal = 0;

					foreach (ComputationCellReference cellRef in hdrList)
					{
						totalHdrVal += ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, cellRef, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);
					}

					//ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, asrtCellRef, true, false) - totalHdrVal);
					double x = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, asrtCellRef, true, false);
					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, asrtCellRef, true, false) );


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

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        //private class clsFormula_CalculatePackFromColor : AssortmentFormulaProfile
        //{
        //    public clsFormula_CalculatePackFromColor(AssortmentViewComputations aComputations, int aKey)
        //        : base(aComputations, aKey, "Formula_CalculatePackFromColor Init")
        //    {
        //    }

        //    override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
        //    {
        //        int packRID;
        //        int numMultiples;
        //        int packMultiple;
        //        double newValue;
        //        ComputationCellReference totCellRef;
        //        ArrayList dtlCellRefList;
        //        double colorTot;
        //        int bulkMultiple;

        //        try
        //        {
        //            packRID = aAssrtSchdEntry.ChangedCompCellRef[eProfileType.HeaderPack];

        //            if (packRID != int.MaxValue)
        //            {
        //                numMultiples = (int)((ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.ChangedCompCellRef, aAssrtSchdEntry.ChangedCompCellRef.isCellHidden) /
        //                    aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetColorMultiple(
        //                        aAssrtSchdEntry.ChangedCompCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType],
        //                        aAssrtSchdEntry.ChangedCompCellRef[eProfileType.HeaderPack],
        //                        aAssrtSchdEntry.ChangedCompCellRef[eProfileType.HeaderPackColor])) + .5);

        //                packMultiple = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetPackMultiple(
        //                        aAssrtSchdEntry.ChangedCompCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType],
        //                        aAssrtSchdEntry.ChangedCompCellRef[eProfileType.HeaderPack]);

        //                newValue = numMultiples * packMultiple;
        //            }
        //            else
        //            {
        //                ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.ChangedCompCellRef, aAssrtSchdEntry.ChangedCompCellRef.isCellHidden);

        //                totCellRef = ToolBox.GetTotalOperandCell(aAssrtSchdEntry, aSetCellMode, (AssortmentCellReference)aAssrtSchdEntry.ChangedCompCellRef, ((AssortmentCellReference)aAssrtSchdEntry.ChangedCompCellRef).AssortmentCube.GetDetailPackCubeType(), AssortmentQuantityVariables.Value, aAssrtSchdEntry.ChangedCompCellRef.isCellHidden);
        //                dtlCellRefList = totCellRef.GetDetailCellRefArray(((AssortmentCellReference)aAssrtSchdEntry.ChangedCompCellRef).AssortmentCube.GetDetailColorCubeType(), false);
        //                colorTot = 0;

        //                foreach (ComputationCellReference cellRef in dtlCellRefList)
        //                {
        //                    colorTot += ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, cellRef, false, aAssrtSchdEntry.ChangedCompCellRef.isCellHidden);
        //                }

        //                bulkMultiple = aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.AssortmentCubeGroup.GetPackMultiple(
        //                        aAssrtSchdEntry.ChangedCompCellRef[aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.HeaderProfileType],
        //                        aAssrtSchdEntry.ChangedCompCellRef[eProfileType.HeaderPack]);

        //                numMultiples = (int)Math.Ceiling(colorTot / bulkMultiple);

        //                newValue = numMultiples * bulkMultiple;
        //            }

        //            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, newValue);

        //            return eComputationFormulaReturnType.Successful;
        //        }
        //        catch (Exception exc)
        //        {
        //            string message = exc.ToString();
        //            throw;
        //        }
        //    }
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        private class clsFormula_ComponentGrade_TotalUnits_Difference : AssortmentFormulaProfile
		{
			public clsFormula_ComponentGrade_TotalUnits_Difference(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_ComponentGrade_TotalUnits_Difference Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				double change;

				try
				{
					change = ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Previous, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Difference, false) -
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, eGetCellMode.Current, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentQuantityVariables.Difference, false);

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, false, false) - change);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
        }

		private class clsFormula_ComponentGrade_TotalUnits_AvgUnits : AssortmentFormulaProfile
		{
			public clsFormula_ComponentGrade_TotalUnits_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_ComponentGrade_TotalUnits_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;
                int numStores = 0;	// TT#4294 - stodd - Average Units in Matrix Enahancement

				try
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        //numStores = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef);
                        numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.NumStoresAllocated, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);

						// Begin TT#1678-MD - stodd - GA Allocation -> Matrix Tab-> enter Average Units and apply-> changes to zero
                        if (numStores == 0)
                        {
                            summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
                            numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false);
                        }
						// End TT#1678-MD - stodd - GA Allocation -> Matrix Tab-> enter Average Units and apply-> changes to zero

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) * numStores);
                    }
                    else
                    {
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
                        summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) *
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false));
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

		// BEGIN TT#1636 - stodd - Index not Recalced
		private class clsFormula_ComponentGrade_Index_AvgUnits : AssortmentFormulaProfile
		{
			public clsFormula_ComponentGrade_Index_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_ComponentGrade_Index_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;

				try
				{
					summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

					double avgUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) * 100;
					double sumAvgUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false);
					double index = avgUnits / sumAvgUnits;

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						(ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) /
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false)) * 100);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// END TT#1636 - stodd - Index not Recalced

		private class clsFormula_ComponentGrade_TotalUnits_Index : AssortmentFormulaProfile
		{
			public clsFormula_ComponentGrade_TotalUnits_Index(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_ComponentGrade_TotalUnits_Index Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;
				ComputationCellReference summCellRef;

				try
				{
					summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
					summCellRef = (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.Index, false) *
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, summCellRef, AssortmentSummaryVariables.AvgStore, false) / 100 *
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, summCellRef, AssortmentSummaryVariables.NumStores, false));

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
        }

		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
		//private class clsFormula_ComponentGrade_TotalUnits_SummaryAvgUnits : AssortmentFormulaProfile
		//{
		//    public clsFormula_ComponentGrade_TotalUnits_SummaryAvgUnits(AssortmentViewComputations aComputations, int aKey)
		//        : base(aComputations, aKey, "ComponentGrade_TotalUnits_SummaryAvgUnits Init")
		//    {
		//    }

		//    override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
		//    {
		//        ComputationCube summCube;
		//        ComputationCellReference summCellRef;

		//        try
		//        {
		//            summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());
		//            summCellRef = (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

		//            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
		//                ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, summCellRef, AssortmentSummaryVariables.AvgUnits, false) *
		//                ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, summCellRef, AssortmentSummaryVariables.NumStores, false));

		//            return eComputationFormulaReturnType.Successful;
		//        }
		//        catch (Exception exc)
		//        {
		//            string message = exc.ToString();
		//            throw;
		//        }
		//    }
		//}

		//End TT#1196 - JScott - Average units in the summary section should spread when changed
		private class clsFormula_Calculate_AvgUnits_From_TotalUnits : AssortmentFormulaProfile
		{
			public clsFormula_Calculate_AvgUnits_From_TotalUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_Calculate_AvgUnits_From_TotalUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;
                int numStores = 0;	// TT#4294 - stodd - Average Units in Matrix Enahancement

				try
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        //numStores = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef);
                        numStores = (int)ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.NumStoresAllocated, AssortmentQuantityVariables.Value, aAssrtSchdEntry.AssortmentCellRef.isCellHidden);


                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, false) / numStores);
                    }
                    else
                    {
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
                        summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, false) /
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false));
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
		// BEGIN TT#1636 - stodd - index not recalcing
		private class clsFormula_Calculate_Index_From_AvgUnits : AssortmentFormulaProfile
		{
			public clsFormula_Calculate_Index_From_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_Calculate_Index_From_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;

				try
				{
					summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

					double avgUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) * 100;
					double sumAvgUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false);
					double index = avgUnits / sumAvgUnits;

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						(ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) /
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false)) * 100);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// End TT#1636 - stodd - index not recalcing
		//Begin TT#1196 - JScott - Average units in the summary section should spread when changed

		// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
        private class clsFormula_Calculate_TotalPct_From_TotalUnits : AssortmentFormulaProfile
        {
            public clsFormula_Calculate_TotalPct_From_TotalUnits(AssortmentViewComputations aComputations, int aKey)
                : base(aComputations, aKey, "Formula_Calculate_TotalPct_From_TotalUnits Init")
            {
            }

            override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                ComputationCube totalCube;

                try
                {
                    totalCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSubTotalCubeType());
                    ComputationCellReference aTotCell = (ComputationCellReference)totalCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);

                    // Begin TT#1954-MD - JSmith - Assortment
					// Verify total cube coordinates before attempting calculation to avoid throwing InvalidLogicalCoordinate exception
                    ComputationCellReference cellRef = (ComputationCellReference)totalCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef);
                    for (int i = 0; i < cellRef.CellCoordinates.NumIndices; i++)
                    {
                        if (cellRef.CellCoordinates.GetRawCoordinate(i) == -1)
                        {
                            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, 100.00);
                            return eComputationFormulaReturnType.Successful;
                        }
                    }
					// End TT#1954-MD - JSmith - Assortment

                    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                        ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.TotalUnits, false) /
						// Begin TT#1954-MD - JSmith - Assortment
                        //ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)totalCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentDetailVariables.TotalUnits, false));
                        ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, cellRef, AssortmentDetailVariables.TotalUnits, false));
						// End TT#1954-MD - JSmith - Assortment

                    return eComputationFormulaReturnType.Successful;
                }
                catch (MIDException mexc)
                {
                    // When adding style or planlevel to views, those corrdinates will become -1 (invalid) for the total cube.
                    // Catch it and make those 100%.
                    if (mexc.ErrorNumber == (int)eMIDTextCode.msg_pl_InvalidLogicalCoordinate)
                    {
                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, 100.00);
                        return eComputationFormulaReturnType.Successful;
                    }
                    else
                    {
                        throw;
                    }

                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
		// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.

		private class clsFormula_ComponentTotal_TotalUnits_AvgUnits : AssortmentFormulaProfile
		{
			public clsFormula_ComponentTotal_TotalUnits_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_ComponentTotal_TotalUnits_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;
                ProfileList outStoreList = null;	// TT#4294 - stodd - Average Units in Matrix Enahancement

				try
				{
					// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
                    if (((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).AssortmentType == eAssortmentType.GroupAllocation)
                    {
                        int storeCnt = ((AssortmentCubeGroup)aAssrtSchdEntry.AssortmentCellRef.Cube.CubeGroup).GetCountOfStoresWithAllocation(aAssrtSchdEntry.AssortmentCellRef, out outStoreList);

                        //if (storeCnt == 0)
                        //{
                        //    ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, 5555);
                        //}
                        //else
                        //{
                            ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                                ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.AvgUnits, false) * storeCnt);
                        //}
                    }
                    else
                    {
					// End TT#4294 - stodd - Average Units in Matrix Enahancement
                        summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

                        ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.AvgUnits, false) *
                            ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.NumStores, false));
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

		//End TT#1196 - JScott - Average units in the summary section should spread when changed

		// BEGIN TT#1636 - stodd - Index not recalced
		private class clsFormula_ComponentTotal_Index_AvgUnits : AssortmentFormulaProfile
		{
			public clsFormula_ComponentTotal_Index_AvgUnits(AssortmentViewComputations aComputations, int aKey)
				: base(aComputations, aKey, "Formula_ComponentTotal_Index_AvgUnits Init")
			{
			}

			override public eComputationFormulaReturnType Execute(AssortmentScheduleFormulaEntry aAssrtSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
			{
				ComputationCube summCube;

				try
				{
					summCube = (ComputationCube)aAssrtSchdEntry.ComputationCellRef.Cube.CubeGroup.GetCube(aAssrtSchdEntry.AssortmentCellRef.AssortmentCube.GetSummaryCubeType());

					double avgUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentDetailVariables.AvgUnits, false) * 100;
					double sumAvgUnits = ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false);
					double index = avgUnits / sumAvgUnits;

					ToolBox.SetCellValue(aSetCellMode, aAssrtSchdEntry.AssortmentCellRef,
						(ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, aAssrtSchdEntry.AssortmentCellRef, AssortmentTotalVariables.AvgUnits, false) /
						ToolBox.GetOperandCellValue(aAssrtSchdEntry, aGetCellMode, aSetCellMode, (ComputationCellReference)summCube.CreateCellReference(aAssrtSchdEntry.AssortmentCellRef), AssortmentSummaryVariables.AvgStore, false)) * 100);

					return eComputationFormulaReturnType.Successful;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		// BEGIN TT#1636 - stodd - Index not recalced

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Formulas
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Miscellaneous Classes
		//-------------------------------------------

		private class MultipleSortKey : IComparable
		{
			private AssortmentCellReference _cellRef;
			private int _comparePackRID;
			private int _packRID;
			private double _units;
			private int _multiple;
			private int _bulkMultiple;
			private int _random;

			public MultipleSortKey(AssortmentCellReference aCellRef, int aPackRID, double aUnits, int aMultiple)
			{
				Initialize(aCellRef, aPackRID, aUnits, aMultiple, 0);
			}

			public MultipleSortKey(AssortmentCellReference aCellRef, int aPackRID, double aUnits, int aMultiple, int aBulkMultiple)
			{
				Initialize(aCellRef, aPackRID, aUnits, aMultiple, aBulkMultiple);
			}

			public AssortmentCellReference CellReference
			{
				get
				{
					return _cellRef;
				}
			}

			public int PackRID
			{
				get
				{
					return _packRID;
				}
			}

			public int Mulitple
			{
				get
				{
					return _multiple;
				}
			}

			public int BulkMulitple
			{
				get
				{
					return _bulkMultiple;
				}
			}

			public void Initialize(AssortmentCellReference aCellRef, int aPackRID, double aUnits, int aMultiple, int aBulkMultiple)
			{
				_cellRef = aCellRef;

				if (aPackRID != int.MaxValue)
				{
					_comparePackRID = aPackRID;
				}
				else
				{
					_comparePackRID = -1;
				}

				_packRID = aPackRID;
				_units = aUnits;
				_multiple = aMultiple;
				_bulkMultiple = aBulkMultiple;
				_random = aCellRef.ComputationCube.Transaction.GetRandomInteger();
			}

			public int CompareTo(object obj)
			{
				MultipleSortKey compObj;
				int retCode;

				try
				{
					compObj = (MultipleSortKey)obj;

					if (_comparePackRID > compObj._comparePackRID)
					{
						retCode = 1;
					}
					else if (_comparePackRID < compObj._comparePackRID)
					{
						retCode = -1;
					}
					else
					{
						if (_units > compObj._units)
						{
							retCode = 1;
						}
						else if (_units < compObj._units)
						{
							retCode = -1;
						}
						else
						{
							if (_multiple > compObj._multiple)
							{
								retCode = 1;
							}
							else if (_multiple < compObj._multiple)
							{
								retCode = -1;
							}
							else
							{
								if (_random > compObj._random)
								{
									retCode = 1;
								}
								else if (_random < compObj._random)
								{
									retCode = -1;
								}
								else
								{
									retCode = 0;
								}
							}
						}
					}

					return retCode * -1;
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
	}
}
