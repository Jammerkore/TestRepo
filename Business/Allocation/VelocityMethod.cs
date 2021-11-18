using System;
using System.Collections;
using System.Collections.Generic;  // TT#586 Velocity variables not calculated correctly
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//
// Module completely reorganized
//
// (CSMITH) - END MID Track #2410
	public class VelocityMethod:AllocationBaseMethod
	{
		// ======
		// Fields
		// ======

		private VelocityMethodData _VMD;

		private DataSet _dsVelocity = null;

		// BEG MID Change j.ellis Add Audit Message
		private string _globalUserTypeText;
		// END MID Change j.ellis Add Audit Message

		// ========
		// Counters
		// ========
		private int _HdrCnt = 0;
		private int _PSTRows = 0;
		private int _SGLRows = 0;
		private int _BasisRows = 0;
		private int _GradeRows = 0;
        //private int _SlsPrdRows = 0;
        // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
        private double _BasisSalesorStock = 0;
        private double _BasisSalesorStockOHandIT = 0; 
        private bool _StockInUse; 
        private bool _StockRuleAdd;
        // END TT#3518 - AGallagher - Velocity Enhancements - Part 2

		// =================
		// Matrix Fixed Data
		// =================
		private int _SG_RID;
		private int _OTS_Begin_CDR_RID;
		private int _OTS_ShipTo_CDR_RID;
		private int _OTS_Plan_MdseHnRID;
		private int _OTS_Plan_ProdHnRID;
		private int _OTS_Plan_ProdHnLvlSeq;
        private int _TotalPreSizeAllocated = 0;  // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 

		private bool _TrendPctContribution;
		private bool _UseSimilarStoreHistory;
		private bool _CalculateAverageUsingChain;
		private bool _DetermineShipQtyUsingBasis;
		// Begin Track #6074
		private bool _gradesByBasisInd;
		// End Track #6074

        private eVelocityMethodGradeVariableType _gradeVariableType; //TT#855-MD -jsobek -Velocity Enhancements
        private char _balanceToHeaderInd; //TT#855-MD -jsobek -Velocity Enhancements

        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        private int _ApplyCalcMin;
        private int _ApplyCalcMax;
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private double _IBApplyCalcMin;
        private double _IBApplyCalcMax;
        // END TT#1287 - AGallagher - Inventory Min/Max

        private string _ApplyGrade;   // TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores  
        
		// ==========
		// Work Areas
		// ==========
		private bool _isInteractive;

		private GeneralComponent _component;

		private AllocationProfile _alocProfile;

		private ApplicationSessionTransaction _applicationTransaction;

		// ==========
		// Store Data
		// ==========
        Hashtable _storeData = null;
        //tt#152 - velocity balance - apicchetti
        private bool _balance;
        //tt#152 - velocity balance - apicchetti
        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        private bool _reconcile;
        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        private bool _bypassbal;  // TT#673 - AGallagher - Velocity - Disable Balance option on WUB header 
        private int _SpreadRound = 0;  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
        private bool _spreadAct; // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
        private bool _spreadDec; // TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        private char _ApplyMinMaxInd;
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private bool HInventoryInd;
        private char _InventoryInd; 
        private int _MERCH_HN_RID;
		private int _MERCH_PH_RID;
		private int _MERCH_PHL_SEQ;
        private char _IBInventoryInd;
        private int _IBMERCH_HN_RID;
        private int _IBMERCH_PH_RID;
        private int _IBMERCH_PHL_SEQ;
        private eMerchandiseType _merchandiseType;
        // END TT#1287 - AGallagher - Inventory Min/Max

        // begin TT#533 Velocity variables calculated incorrectly
        ////tt#153 - velocity matrix variables - apicchetti
        //private double _totalSales_Set;
        //private double _totalSales_All;
        //private double _avgSales_All;
        //private int _totalStores_All;
        //private double _avgSalesIdx_All;
        ////tt#153 - velocity matrix variables - apicchetti
        // end TT#533 Velocity variables calculated incorrectly 
        private bool _basisChangesMade;	// TT#4522 - stodd - velocity matrix wrong
        private bool _dataLoaded = false;


		// ===========
		// Header Data
		// ===========
		Hashtable _headerData = null;

		// =================
		// Basis Merchandise
		// =================
		Hashtable _basisMdseData = null;

		// ==================
		// Basis Time Periods
		// ==================
		//Hashtable _basisTimeData = null;

		// =======================
		// Grades and Lower Limits
		// =======================
		Hashtable _gradeLowLimData = null;
		Hashtable _lowLimGradeData = null;
        SortedList _lowLimSortedGradeData = null;
        //bool _velocityGradesCompared = false;   // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
        //bool _velocityGradesMatch = false;   // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

		// =====================
		// Pct Sell Thru Indices
		// =====================
		Hashtable _pctSellThruData = null;
		SortedList _pctSellThruSortedData = null;

        // ===========
		// Matrix Data
		// ===========
		Hashtable _groupLvlCellData = null;
		Hashtable _groupLvlGradData = null;
		Hashtable _groupLvlMtrxData = null;

        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
        public Hashtable GroupLvlMtrxData
        {
            get { return _groupLvlMtrxData; }
        }
        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

        // ================================
		// Basis Merchandise Node structure
		// ================================
		public class BasisMdseNode
		{
			private int _sequence;
			private int _basisFVRID;
			private int _basisMdseHnRID;
			private int _basisProdHnRID;
			private int _basisProdHnLvlSeq;
			// BEGIN Issue 4818
			private int _basisTimeCdrRID;
			private double _basisWeightFactor;
			// END Issue 4818
			
			public int Sequence
			{
				get
				{
					return _sequence;
				}

				set
				{
					_sequence = value;
				}
			}

			public int BasisFVRID
			{
				get
				{
					return _basisFVRID;
				}

				set
				{
					_basisFVRID = value;
				}
			}

			public int BasisMdseHnRID
			{
				get
				{
					return _basisMdseHnRID;
				}

				set
				{
					_basisMdseHnRID = value;
				}
			}

			public int BasisProdHnRID
			{
				get
				{
					return _basisProdHnRID;
				}

				set
				{
					_basisProdHnRID = value;
				}
			}

			public int BasisProdHnLvlSeq
			{
				get
				{
					return _basisProdHnLvlSeq;
				}

				set
				{
					_basisProdHnLvlSeq = value;
				}
			}

			// BEGIN Issue 4818
			public int BasisTimeCdrRID
			{
				get
				{
					return _basisTimeCdrRID;
				}

				set
				{
					_basisTimeCdrRID = value;
				}
			}

			public double BasisWeightFactor
			{
				get
				{
					return _basisWeightFactor;
				}

				set
				{
					_basisWeightFactor = value;
				}
			}
			// END Issue 4818

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the BasisMdseNode structure
			/// </summary>
			public BasisMdseNode()
			{
			}

			public BasisMdseNode Copy()
			{
				try
				{
					BasisMdseNode bmn = (BasisMdseNode)this.MemberwiseClone();
					bmn.Sequence = Sequence;
					bmn.BasisFVRID = BasisFVRID;
					bmn.BasisMdseHnRID = BasisMdseHnRID;
					bmn.BasisProdHnRID = BasisProdHnRID;
					bmn.BasisProdHnLvlSeq = BasisProdHnLvlSeq;
					bmn.BasisTimeCdrRID = BasisTimeCdrRID;
					bmn.BasisWeightFactor = BasisWeightFactor;
					return bmn;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// BEGIN Issue 4818
//		// ===========================
//		// Basis Time Period structure
//		// ===========================
//		public class BasisTimePrd
//		{
//			private int _sequence;
//			private int _basisTimeCdrRID;
//			private double _basisWeightFactor;
//
//			public int Sequence
//			{
//				get
//				{
//					return _sequence;
//				}
//
//				set
//				{
//					_sequence = value;
//				}
//			}
//
//			public int BasisTimeCdrRID
//			{
//				get
//				{
//					return _basisTimeCdrRID;
//				}
//
//				set
//				{
//					_basisTimeCdrRID = value;
//				}
//			}
//
//			public double BasisWeightFactor
//			{
//				get
//				{
//					return _basisWeightFactor;
//				}
//
//				set
//				{
//					_basisWeightFactor = value;
//				}
//			}
//
//			// ===========
//			// Constructor
//			// ===========
//			/// <summary>
//			/// Creates an instance of the BasisTimePrd structure
//			/// </summary>
//			public BasisTimePrd()
//			{
//			}
//
//			public BasisTimePrd Copy(Session aSession, bool aCloneDateRanges)
//			{
//				try
//				{
//					BasisTimePrd btp = (BasisTimePrd)this.MemberwiseClone();
//					btp.Sequence = Sequence;
//					if (aCloneDateRanges &&
//						BasisTimeCdrRID != Include.UndefinedCalendarDateRange)
//					{
//						btp.BasisTimeCdrRID = aSession.Calendar.GetDateRangeClone(BasisTimeCdrRID).Key;
//					}
//					else
//					{
//						btp.BasisTimeCdrRID = BasisTimeCdrRID;
//					}
//					btp.BasisWeightFactor = BasisWeightFactor;
//
//					return btp;
//				}
//				catch (Exception exc)
//				{
//					string message = exc.ToString();
//					throw;
//				}
//			}
//		}
		// END Issue 4818

		// ===========================
		// Grade Lower Limit structure
		// ===========================
		public class GradeLowLimit
		{
			private int _row;
			private string _grade;
			private int _lowerLimit;
            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            private int _allocMin;
            private int _allocMax;
            private int _allocAdMin;
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)


           	public int Row
			{
				get
				{
					return _row;
				}

				set
				{
					_row = value;
				}
			}

			public string Grade
			{
				get
				{
					return _grade;
				}

				set
				{
					_grade = value;
				}
			}

			public int LowerLimit
			{
				get
				{
					return _lowerLimit;
				}

				set
				{
					_lowerLimit = value;
				}
			}

            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            public int AllocMin
            {
                get
                {
                    return _allocMin;
                }

                set
                {
                    _allocMin = value;
                }
            }

            public int AllocMax
            {
                get
                {
                    return _allocMax;
                }

                set
                {
                    _allocMax = value;
                }
            }

            public int AllocAdMin
            {
                get
                {
                    return _allocAdMin;
                }

                set
                {
                    _allocAdMin = value;
                }
            }
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

            // ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the GradeLowlimit structure
			/// </summary>
			public GradeLowLimit()
			{
			}

			public GradeLowLimit Copy()
			{
				try
				{
					GradeLowLimit gll = (GradeLowLimit)this.MemberwiseClone();
					gll.Row = Row;
					gll.Grade = Grade;
					gll.LowerLimit = LowerLimit;

                    // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    gll.AllocMin = AllocMin;
                    gll.AllocMax = AllocMax;
                    gll.AllocAdMin = AllocAdMin;
                    // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    
					return gll;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        // =============================
		// Pct Sell Thru Index structure
		// =============================
		public class PctSellThruIndex
		{
			private int _row;
			private int _sellThruIndex;

			public int Row
			{
				get
				{
					return _row;
				}

				set
				{
					_row = value;
				}
			}

			public int SellThruIndex
			{
				get
				{
					return _sellThruIndex;
				}

				set
				{
					_sellThruIndex = value;
				}
			}

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the PctSellThruIndex structure
			/// </summary>
			public PctSellThruIndex()
			{
			}

			public PctSellThruIndex Copy()
			{
				try
				{
					PctSellThruIndex sti = (PctSellThruIndex)this.MemberwiseClone();
					sti.Row = Row;
					sti.SellThruIndex = SellThruIndex;

					return sti;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        // ==========================
		// Group Lvl Grade structure
		// ==========================
		public class GroupLvlGrade
		{
			private string _grade;
			private int _totGrpBasisSales;
			private double _avgGrpBasisSales;
			private double _avgGrpBasisSalesIndex;
			private double _avgGrpBasisSalesPctTot;

            // begin MID TT#587 Velocity Variables not calculated correctly
            private double _grpNumberOfStores;
            private double _grpAvgStock;
            private double _grpStockPercentOfTotal;
            private double _grpAllocationPercentOfTotal;
            // end MID TT#587 Velocity Variables not calculated correctly

			private int _totChnBasisSales;
			private double _avgChnBasisSales;
			private double _avgChnBasisSalesIndex;
			private double _avgChnBasisSalesPctTot;

            // begin MID TT#587 Velocity Variables not calculated correctly
            private double _chnNumberOfStores;
            private double _chnAvgStock;
            private double _chnStockPercentOfTotal;
            private double _chnAllocationPercentOfTotal;
            ////BEGIN MID TT#153 apicchetti
            //private double _totNumberOfStores;
            //private double _avgStock;
            //private double _stockPercentOfTotal;
            //private double _allocationPercentOfTotal;
            ////END MID TT#153 apicchetti
            // end MID TT#587 Velocity Variables not calculated correctly

			public string Grade
			{
				get
				{
					return _grade;
				}

				set
				{
					_grade = value;
				}
			}

			public int TotGrpBasisSales
			{
				get
				{
					return _totGrpBasisSales;
				}

				set
				{
					_totGrpBasisSales = value;
				}
			}

			public double AvgGrpBasisSales
			{
				get
				{
					return _avgGrpBasisSales;
				}

				set
				{
					_avgGrpBasisSales = value;
				}
			}

			public double AvgGrpBasisSalesIndex
			{
				get
				{
					return _avgGrpBasisSalesIndex;
				}

				set
				{
					_avgGrpBasisSalesIndex = value;
				}
			}

			public double AvgGrpBasisSalesPctTot
			{
				get
				{
					return _avgGrpBasisSalesPctTot;
				}

				set
				{
					_avgGrpBasisSalesPctTot = value;
				}
			}

			public int TotChnBasisSales
			{
				get
				{
					return _totChnBasisSales;
				}

				set
				{
					_totChnBasisSales = value;
				}
			}

			public double AvgChnBasisSales
			{
				get
				{
					return _avgChnBasisSales;
				}

				set
				{
					_avgChnBasisSales = value;
				}
			}

			public double AvgChnBasisSalesIndex
			{
				get
				{
					return _avgChnBasisSalesIndex;
				}

				set
				{
					_avgChnBasisSalesIndex = value;
				}
			}

			public double AvgChnBasisSalesPctTot
			{
				get
				{
					return _avgChnBasisSalesPctTot;
				}

				set
				{
					_avgChnBasisSalesPctTot = value;
				}
			}


            // begin TT#587 Velocity Variables not calculated correctly
            public double GroupNumberOfStores
            {
                get
                {
                    return _grpNumberOfStores;
                }

                set
                {
                    _grpNumberOfStores = value;
                }
            }

            public double GroupAverageStock
            {
                get
                {
                    return _grpAvgStock;
                }
                set
                {
                    _grpAvgStock = value;
                }
            }

            public double GroupStockPercentOfTotal
            {
                get
                {
                    return _grpStockPercentOfTotal;
                }
                set
                {
                    _grpStockPercentOfTotal = value;
                }
            }

            public double GroupAllocationPercentOfTotal
            {
                get
                {
                    return _grpAllocationPercentOfTotal;
                }
                set
                {
                    _grpAllocationPercentOfTotal = value;
                }
            }
            public double ChainNumberOfStores
            {
                get
                {
                    return _chnNumberOfStores;
                }

                set
                {
                    _chnNumberOfStores = value;
                }
            }

            public double ChainAverageStock
            {
                get
                {
                    return _chnAvgStock;
                }
                set
                {
                    _chnAvgStock = value;
                }
            }

            public double ChainStockPercentOfTotal
            {
                get
                {
                    return _chnStockPercentOfTotal;
                }
                set
                {
                    _chnStockPercentOfTotal = value;
                }
            }

            public double ChainAllocationPercentOfTotal
            {
                get
                {
                    return _chnAllocationPercentOfTotal;
                }
                set
                {
                    _chnAllocationPercentOfTotal = value;
                }
            }
            ////BEGIN TT#153 apicchetti
            //public double TotalNumberOfStores
            //{
            //    get
            //    {
            //        return _totNumberOfStores;
            //    }

            //    set
            //    {
            //        _totNumberOfStores = value;
            //    }
            //}

            //public double AverageStock
            //{
            //    get
            //    {
            //        return _avgStock;
            //    }
            //    set
            //    {
            //        _avgStock = value;
            //    }
            //}

            //public double StockPercentOfTotal
            //{
            //    get
            //    {
            //        return _stockPercentOfTotal;
            //    }
            //    set
            //    {
            //        _stockPercentOfTotal = value;
            //    }
            //}

            //public double AllocationPercentOfTotal
            //{
            //    get
            //    {
            //        return _allocationPercentOfTotal;
            //    }
            //    set
            //    {
            //        _allocationPercentOfTotal = value;
            //    }
            //}
            ////END TT#153 apicchetti
            // end TT#587 Velocity Variables not calculated correctly

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the GroupLvlGrade structure
			/// </summary>
			public GroupLvlGrade()
			{
			}

			public GroupLvlGrade Copy()
			{
				try
				{
					GroupLvlGrade glg = (GroupLvlGrade)this.MemberwiseClone();
					glg.Grade = Grade;
					glg.TotGrpBasisSales = TotGrpBasisSales;
					glg.AvgGrpBasisSales = AvgGrpBasisSales;
					glg.AvgGrpBasisSalesIndex = AvgGrpBasisSalesIndex;
					glg.AvgGrpBasisSalesPctTot = AvgGrpBasisSalesPctTot;
					glg.TotChnBasisSales = TotChnBasisSales;
					glg.AvgChnBasisSales = AvgChnBasisSales;
					glg.AvgChnBasisSalesIndex = AvgChnBasisSalesIndex;
					glg.AvgChnBasisSalesPctTot = AvgChnBasisSalesPctTot;

                    // begin TT#587 Velocity Variables not calculated correctly
                    glg.GroupNumberOfStores = GroupNumberOfStores;
                    glg.GroupAverageStock = GroupAverageStock;
                    glg.GroupAllocationPercentOfTotal = GroupAllocationPercentOfTotal;
                    glg.GroupStockPercentOfTotal = GroupStockPercentOfTotal;
                    glg.ChainNumberOfStores = ChainNumberOfStores;
                    glg.ChainAverageStock = ChainAverageStock;
                    glg.ChainAllocationPercentOfTotal = ChainAllocationPercentOfTotal;
                    glg.ChainStockPercentOfTotal = ChainStockPercentOfTotal;
                    ////BEGIN TT#153 – add variables to velocity matrix - apicchetti
                    //glg.TotalNumberOfStores = TotalNumberOfStores;
                    //glg.AverageStock = AverageStock;
                    //glg.AllocationPercentOfTotal = AllocationPercentOfTotal;
                    //glg.StockPercentOfTotal = StockPercentOfTotal;
                    ////END TT#153 – add variables to velocity matrix - apicchetti
                    // end TT#587 Velocity Variables not calculated correctly

                    return glg;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        // begin TT#586 Velocity variables not calculated correctly
        // ========================
        // Group Sell Thru Total Cell structure
        // ========================
        public class GroupSellThruTotalCell
        {
            private int _key;
            private int _sellThruIndex;
            private int _sellThruGrpStores;
            private double _sellThruGrpSales;
            private double _sellThruGrpOnHand;
            private int _sellThruChnStores;
            private double _sellThruChnSales;
            private double _sellThruChnOnHand;

            public int Key
            {
                get
                {
                    return _key;
                }

                set
                {
                    _key = value;
                }
            }


            public int SellThruIndex
            {
                get
                {
                    return _sellThruIndex;
                }

                set
                {
                    _sellThruIndex = value;
                }
            }

            public int SellThruGrpStores
            {
                get
                {
                    return _sellThruGrpStores;
                }

                set
                {
                    _sellThruGrpStores = value;
                }
            }

            public double SellThruGrpSales
            {
                get
                {
                    return _sellThruGrpSales;
                }

                set
                {
                    _sellThruGrpSales = value;
                }
            }

            public double SellThruGrpOnHand
            {
                get
                {
                    return _sellThruGrpOnHand;
                }

                set
                {
                    _sellThruGrpOnHand = value;
                }
            }

            public double SellThruGrpAvgWOS
            {
                get
                {
                    if (_sellThruGrpSales != 0)
                    {
                        return _sellThruGrpOnHand / _sellThruGrpSales;
                    }
                    return 0;
                }
            }

            public int SellThruChnStores
            {
                get
                {
                    return _sellThruChnStores;
                }

                set
                {
                    _sellThruChnStores = value;
                }
            }

            public double SellThruChnSales
            {
                get
                {
                    return _sellThruChnSales;
                }

                set
                {
                    _sellThruChnSales = value;
                }
            }

            public double SellThruChnOnHand
            {
                get
                {
                    return _sellThruChnOnHand;
                }

                set
                {
                    _sellThruChnOnHand = value;
                }
            }

            public double SellThruChnAvgWOS
            {
                get
                {
                    if (_sellThruChnSales != 0)
                    {
                        return _sellThruChnOnHand / _sellThruChnSales;
                    }
                    return 0;
                }
            }


            // ===========
            // Constructor
            // ===========
            /// <summary>
            /// Creates an instance of the GroupSellThruTotalCell structure
            /// </summary>
            public GroupSellThruTotalCell()
            {
            }

            public GroupSellThruTotalCell Copy()
            {
                GroupSellThruTotalCell gsttc = (GroupSellThruTotalCell)this.MemberwiseClone();
                gsttc.Key = Key;
                gsttc.SellThruIndex = SellThruIndex;
                gsttc.SellThruGrpStores = SellThruGrpStores;
                gsttc.SellThruGrpSales = SellThruGrpSales;
                gsttc.SellThruGrpOnHand = SellThruGrpOnHand;
                gsttc.SellThruChnStores = SellThruChnStores;
                gsttc.SellThruChnSales = SellThruChnSales;
                gsttc.SellThruChnOnHand = SellThruChnOnHand;

                return gsttc;
            }
        }
        // end TT#586 Velocity variables not calculated correctly

		// ========================
		// Group Lvl Cell structure
		// ========================
		public class GroupLvlCell
		{
			private string _key;
			private int _boundary;
			private int _sellThruIndex;
			private int _cellGrpStores;
			private double _cellGrpSales;
			private double _cellGrpOnHand;
			private double _cellGrpAvgWOS;
			private int _cellChnStores;
			private double _cellChnSales;
			private double _cellChnOnHand;
			private double _cellChnAvgWOS;
			private double _cellRuleQty;
            private double _cellRuleTypeQty; // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
			private eVelocityRuleType _cellRuleType;

			public string Key
			{
				get
				{
					return _key;
				}

				set
				{
					_key = value;
				}
			}

			public int Boundary
			{
				get
				{
					return _boundary;
				}

				set
				{
					_boundary = value;
				}
			}

			public int SellThruIndex
			{
				get
				{
					return _sellThruIndex;
				}

				set
				{
					_sellThruIndex = value;
				}
			}

			public int CellGrpStores
			{
				get
				{
					return _cellGrpStores;
				}

				set
				{
					_cellGrpStores = value;
				}
			}

			public double CellGrpSales
			{
				get
				{
					return _cellGrpSales;
				}

				set
				{
					_cellGrpSales = value;
				}
			}

			public double CellGrpOnHand
			{
				get
				{
					return _cellGrpOnHand;
				}

				set
				{
					_cellGrpOnHand = value;
				}
			}

			public double CellGrpAvgWOS
			{
				get
				{
					return _cellGrpAvgWOS;
				}

				set
				{
					_cellGrpAvgWOS = value;
				}
			}

			public int CellChnStores
			{
				get
				{
					return _cellChnStores;
				}

				set
				{
					_cellChnStores = value;
				}
			}

			public double CellChnSales
			{
				get
				{
					return _cellChnSales;
				}

				set
				{
					_cellChnSales = value;
				}
			}

			public double CellChnOnHand
			{
				get
				{
					return _cellChnOnHand;
				}

				set
				{
					_cellChnOnHand = value;
				}
			}

			public double CellChnAvgWOS
			{
				get
				{
					return _cellChnAvgWOS;
				}

				set
				{
					_cellChnAvgWOS = value;
				}
			}

			public double CellRuleQty
			{
				get
				{
					return _cellRuleQty;
				}

				set
				{
					_cellRuleQty = value;
				}
			}

            // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            public double CellRuleTypeQty
            {
                get
                {
                    return _cellRuleTypeQty;
                }

                set
                {
                    _cellRuleTypeQty = value;
                }
            }
            // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

            public eVelocityRuleType CellRuleType
			{
				get
				{
					return _cellRuleType;
				}

				set
				{
					_cellRuleType = value;
				}
			}

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the GroupLvlCell structure
			/// </summary>
			public GroupLvlCell()
			{
			}

			public GroupLvlCell Copy()
			{
				try
				{
					GroupLvlCell glc = (GroupLvlCell)this.MemberwiseClone();
					glc.Key = Key;
					glc.Boundary = Boundary;
					glc.SellThruIndex = SellThruIndex;
					glc.CellGrpStores = CellGrpStores;
					glc.CellGrpSales = CellGrpSales;
					glc.CellGrpOnHand = CellGrpOnHand;
					glc.CellGrpAvgWOS = CellGrpAvgWOS;
					glc.CellChnStores = CellChnStores;
					glc.CellChnSales = CellChnSales;
					glc.CellChnOnHand = CellChnOnHand;
					glc.CellChnAvgWOS = CellChnAvgWOS;
					glc.CellRuleQty = CellRuleQty;
                    glc.CellRuleTypeQty = CellRuleTypeQty; // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                    glc.CellRuleType = CellRuleType;

					return glc;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// ==========================
		// Group Lvl Matrix structure
		// ==========================
		public class GroupLvlMatrix
		{
			private int _sglRID;
			private int _noOnHandBasisStores;
			private int _noOnHandStyleStores;
			private double _noOnHandRuleQty;
			private eVelocityRuleType _noOnHandRuleType;
            // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
            private char _modeInd;
            private double _averageQty;
            private eVelocityRuleRequiresQuantity _averageRule;
            private char _spreadInd;
            // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
			private double _grpSales;
			private double _grpOnHand;
			private double _grpAvgWOS;
			private double _grpPctSellThru;
			private double _chnSales;
			private double _chnOnHand;
			private double _chnAvgWOS;
			private double _chnPctSellThru;
			private Hashtable _gradeSales;
            private Hashtable _matrixCells;  
            private Dictionary<int, GroupSellThruTotalCell> _matrixSellThruTotalCells; // TT#586 Velocity variables not calculated correctly
            // begin TT#533 Velocity variables not calculated correctly
            private int _eligibleBasisStores;
            private int _eligibleChainStores;
            private int _grpBasisSales;
            private int _grpBasisStock;
            private int _chnBasisSales;
            private int _chnBasisStock;
            private double _avgGrpBasisSalesIndex;
            private double _avgGrpBasisSalesPctTot;
            private double _avgChnBasisSalesIndex;
            private double _avgChnBasisSalesPctTot;
            private double _grpStockPercentOfTotal;
            private double _grpAllocationPercentOfTotal;
            private double _chnStockPercentOfTotal;
            private double _chnAllocationPercentOfTotal;
             // end TT#533 Velocity variables not calculated correctly
            private double _grpAvgBasisStock; // TT#587 Velocity Variables calculated incorrectly
            private double _grpAvgBasisSales; // TT#587 Velocity Variables calculated incorrectly
            private double _chnAvgBasisStock; // TT#587 Velocity Variables calculated incorrectly
            private double _chnAvgBasisSales; // TT#587 Velocity Variables calculated incorrectly
            public int SglRID
			{
				get
				{
					return _sglRID;
				}

				set
				{
					_sglRID = value;
				}
			}
            // begin TT#533 Velocity variables not calculated correctly
            // begin TT#587 Velocity variables calculated incorrectly
            //public int MatrixBasisStores
            //{
            //    //get { return _eligibleBasisStores - _noOnHandBasisStores; }  // TT#586 Total Number of stores wrong (sometimes negative)
            //    get { return _eligibleBasisStores; }                           // TT#586 Total Number of stores wrong (sometimes negative)
            //}
            // end TT#587 Velocity variables calculated incorrectly
            public int EligibleBasisStores
            {
                get { return _eligibleBasisStores; }
                set { _eligibleBasisStores = value; }
            }
            public int GrpBasisSales
            {
                get { return _grpBasisSales; }
                set { _grpBasisSales = value; }
            }
            public int GrpBasisStock
            {
                get { return _grpBasisStock; }
                set { _grpBasisStock = value; }
            }
            public int ChnBasisSales
            {
                get { return _chnBasisSales; }
                set { _chnBasisSales = value; }
            }
            public int ChnBasisStock
            {
                get { return _chnBasisStock; }
                set { _chnBasisStock = value; }
            }
            // TT#587 Velocity Variables calculated incorrectly
            //public double MatrixAvgGrpBasisSales
            //{
            //    get
            //    {
            //        if (MatrixBasisStores > 0)
            //        {
            //            return _grpBasisSales / (double)MatrixBasisStores;
            //        }
            //        return 0;
            //    }
            //}
            // end TT#587 Velocity Variables calculated incorrectly
            public double AvgGrpBasisSales
            {
                get
                {
                    return _grpAvgBasisSales; // TT#587 Velocity Variables calculated incorrectly
                }
                // begin TT#587 Velocity Variables calculated incorrectly
                set
                {
                    _grpAvgBasisSales = value;
                }
                // end TT#587 Velocity Variables calculated incorrectly
            }
            public double AvgGrpBasisStock
            {
                get
                {
                    return _grpAvgBasisStock; // TT#587 Velocity Variables calculated incorrectly
                }
                // begin TT#587 Velocity Variables calculated incorrectly
                set
                {
                    _grpAvgBasisStock = value;
                }
                // end TT#587 Velocity Variables calculated incorrectly
            }
            public double AvgGrpBasisSalesIndex
            {
                get { return _avgGrpBasisSalesIndex; }
                set { _avgGrpBasisSalesIndex = value; }
            }
            public double AvgGrpBasisSalesPctTot
            {
                get { return _avgGrpBasisSalesPctTot; }
                set { _avgGrpBasisSalesPctTot = value; }
            }
            public double AvgChnBasisSales
            {
                get
                {
                    return _chnAvgBasisSales;   // TT#587 Velocity Variables calculated incorrectly
                }
                // begin TT#587 Velocity Variables calculated incorrectly
                set
                {
                    _chnAvgBasisSales = value;
                }
                // end TT#587 Velocity Variables calculated incorrectly
            }
            public double AvgChnBasisStock
            {
                get
                {
                    return _chnAvgBasisStock; // TT#587 Velocity Variables cacluated incorrectly
                }
                // begin Velocity Variables calculated incorrectly
                set
                {
                    _chnAvgBasisStock = value;
                }
                // end TT#587 Velocity Variables calculated incorrectly
            }
            public double AvgChnBasisSalesIndex
            {
                get { return _avgChnBasisSalesIndex; }
                set { _avgChnBasisSalesIndex = value; }
            }
            public double AvgChnBasisSalesPctTot
            {
                get { return _avgChnBasisSalesPctTot; }
                set { _avgChnBasisSalesPctTot = value; }
            }
            public double GrpStockPercentOfTotal
            {
                get { return _grpStockPercentOfTotal; }
                set { _grpStockPercentOfTotal = value; }
            }
            public double GrpAllocationPercentOfTotal
            {
                get { return _grpAllocationPercentOfTotal; }
                set { _grpAllocationPercentOfTotal = value; }
            }
            public double ChnStockPercentOfTotal
            {
                get { return _chnStockPercentOfTotal; }
                set { _chnStockPercentOfTotal = value; }
            }
            public double ChnAllocationPercentOfTotal
            {
                get { return _chnAllocationPercentOfTotal; }
                set { _chnAllocationPercentOfTotal = value; }
            }
            // end TT#533 Velocity variables not calculated correctly

			public int NoOnHandBasisStores
			{
				get
				{
					return _noOnHandBasisStores;
				}

				set
				{
					_noOnHandBasisStores = value;
				}
			}

			public int NoOnHandStyleStores
			{
				get
				{
					return _noOnHandStyleStores;
				}

				set
				{
					_noOnHandStyleStores = value;
				}
			}

			public double NoOnHandRuleQty
			{
				get
				{
					return _noOnHandRuleQty;
				}

				set
				{
					_noOnHandRuleQty = value;
				}
			}

			public eVelocityRuleType NoOnHandRuleType
			{
				get
				{
					return _noOnHandRuleType;
				}

				set
				{
					_noOnHandRuleType = value;
				}
			}
            // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
            public char ModeInd
            {
                get
                {
                    return _modeInd;
                }

                set
                {
                    _modeInd = value;
                }
            }

            public double AverageQty
            {
                get
                {
                    return _averageQty;
                }

                set
                {
                    _averageQty = value;
                }
            }

            public eVelocityRuleRequiresQuantity AverageRule
            {
                get
                {
                    return _averageRule;
                }

                set
                {
                    _averageRule = value;
                }
            }

            public char SpreadInd
            {
                get
                {
                    return _spreadInd;
                }

                set
                {
                    _spreadInd = value;
                }
            }
            // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
			public double GroupSales
			{
				get
				{
					return _grpSales;
				}

				set
				{
					_grpSales = value;
				}
			}

			public double GroupOnHand
			{
				get
				{
					return _grpOnHand;
				}

				set
				{
					_grpOnHand = value;
				}
			}

			public double GroupAvgWOS
			{
				get
				{
					return _grpAvgWOS;
				}

				set
				{
					_grpAvgWOS = value;
				}
			}

			public double GroupPctSellThru
			{
				get
				{
					return _grpPctSellThru;
				}

				set
				{
					_grpPctSellThru = value;
				}
			}

			public double ChainSales
			{
				get
				{
					return _chnSales;
				}

				set
				{
					_chnSales = value;
				}
			}

			public double ChainOnHand
			{
				get
				{
					return _chnOnHand;
				}

				set
				{
					_chnOnHand = value;
				}
			}

			public double ChainAvgWOS
			{
				get
				{
					return _chnAvgWOS;
				}

				set
				{
					_chnAvgWOS = value;
				}
			}

			public double ChainPctSellThru
			{
				get
				{
					return _chnPctSellThru;
				}

				set
				{
					_chnPctSellThru = value;
				}
			}

			public Hashtable GradeSales
			{
				get
				{
					return _gradeSales;
				}

				set
				{
					_gradeSales = value;
				}
			}

			public Hashtable MatrixCells
			{
				get
				{
					return _matrixCells;
				}

				set
				{
					_matrixCells = value;
				}
			}
            // begin TT#586 Velocity variables not calculated correctly
            public Dictionary<int, GroupSellThruTotalCell> MatrixSellThruTotalCells
            {
                get
                {
                    return _matrixSellThruTotalCells;
                }
                set
                {
                    _matrixSellThruTotalCells = value;
                }
            }
            // end TT#586 Velocity variables not calculated correctly

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the GroupLvlMatrix structure
			/// </summary>
			public GroupLvlMatrix()
			{
			}

			public GroupLvlMatrix Copy()
			{
				try
				{
					GroupLvlMatrix glm = (GroupLvlMatrix)this.MemberwiseClone();
					glm.SglRID = SglRID;
					glm.NoOnHandBasisStores = NoOnHandBasisStores;
					glm.NoOnHandStyleStores = NoOnHandStyleStores;
					glm.NoOnHandRuleQty = NoOnHandRuleQty;
					glm.NoOnHandRuleType = NoOnHandRuleType;
                    // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
                    glm.ModeInd = ModeInd;
                    glm.AverageRule = AverageRule;
                    glm.AverageQty = AverageQty;
                    glm.SpreadInd = SpreadInd;
                    // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
					glm.GroupSales = GroupSales;
					glm.GroupOnHand = GroupOnHand;
					glm.GroupAvgWOS = GroupAvgWOS;
					glm.GroupPctSellThru = GroupPctSellThru;
					glm.ChainSales = ChainSales;
					glm.ChainOnHand = ChainOnHand;
					glm.ChainAvgWOS = ChainAvgWOS;
					glm.ChainPctSellThru = ChainPctSellThru;
                    // begin TT#533 velocity variables not calculated correctly
                    glm.EligibleBasisStores = EligibleBasisStores;
                    glm.GrpBasisSales = GrpBasisSales;
                    glm.GrpBasisStock = GrpBasisStock;
                    glm.ChnBasisSales = ChnBasisSales;
                    glm.ChnBasisStock = ChnBasisStock;
                    glm.AvgGrpBasisSalesIndex = AvgGrpBasisSalesIndex;
                    glm.AvgGrpBasisSalesPctTot = AvgGrpBasisSalesPctTot;
                    glm.AvgChnBasisSalesIndex = AvgChnBasisSalesIndex;
                    glm.AvgChnBasisSalesPctTot = AvgChnBasisSalesPctTot;
                    glm.GrpStockPercentOfTotal = GrpStockPercentOfTotal;
                    glm.ChnStockPercentOfTotal = ChnStockPercentOfTotal;
                    // begin TT#587 Velocity Variable not calculated correctly
                    glm.AvgChnBasisSales = AvgChnBasisSales;
                    glm.AvgChnBasisStock = AvgChnBasisStock;
                    glm.AvgGrpBasisSales = AvgChnBasisSales;
                    glm.AvgGrpBasisStock = AvgGrpBasisStock;
                    // end TT#587 Velocity Variable not calculated correctly 
                    // end TT#533 velocity variables not calculated correctly
					glm.GradeSales = new Hashtable();
					foreach (GroupLvlGrade glg in GradeSales.Values)
					{
						GroupLvlGrade newglg = glg.Copy();
						glm.GradeSales.Add(newglg.Grade, newglg);
					}
					glm.MatrixCells = new Hashtable();
					foreach (GroupLvlCell glc in MatrixCells.Values)
					{
						GroupLvlCell newglc = glc.Copy();
						glm.MatrixCells.Add(newglc.Key, newglc);
					}
                    // begin TT#586 Velocity variables not calculated correctly
                    glm.MatrixSellThruTotalCells = new Dictionary<int, GroupSellThruTotalCell>();

                    foreach (GroupSellThruTotalCell gsttc in MatrixSellThruTotalCells.Values)
                    {
                        GroupSellThruTotalCell newGsttc = gsttc.Copy();
                        glm.MatrixSellThruTotalCells.Add(gsttc.Key, newGsttc);
                    }
                    // end TT#586 Velocity variables not calculated correctly 

					return glm;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		// ===========================
		// Header Data Value structure
		// ===========================
		public class HeaderDataValue
		{
			private int _headerRID;
			private AllocationProfile _aloctnProfile;
			private int _noOnHandStores;
			private Hashtable _storeValues;

			public int HeaderRID
			{
				get
				{
					return _headerRID;
				}

				set
				{
					_headerRID = value;
				}
			}

			public AllocationProfile AloctnProfile
			{
				get
				{
					return _aloctnProfile;
				}

				set
				{
					_aloctnProfile = value;
				}
			}

			public int NoOnHandStores
			{
				get
				{
					return _noOnHandStores;
				}

				set
				{
					_noOnHandStores = value;
				}
			}

			public Hashtable StoreValues
			{
				get
				{
					return _storeValues;
				}

				set
				{
					_storeValues = value;
				}
			}

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the HeaderDataValue structure
			/// </summary>
			public HeaderDataValue()
			{
			}
		}

      	// ==========================
		// Store Data Value structure
		// ==========================
		public class StoreDataValue
		{
			private int _storeRID;
			private int _groupLvlRID;
			private bool _isReserve;
			private bool _isEligible;
			private string _grade;
			private int _gradeIDX;
			// Begin TT #91 stodd
			private string _basisGrade;
			private int _basisGradeIDX;
			// End TT #91
			private DateTime _shipDay;
			private double _need;
			private double _pctNeed;
			private int _capacity;        // MID Track 4298 Integer types showing values with decimals
			private int _primaryMax;      // MID Track 4298 Integer types showing values with decimals
			private int _qtyAllocated; // MID Track 4298 Integer types showing values with decimals
			private int _preSizeAllocated; // MID Track 4282 Velocity overlays Fill Size Holes Allocation
			private double _planSales;
			private double _planStock;
			private double _basisSales;
			private double _basisStock;
			private double _basisOnHand;
			private double _basisOHandIT;
            private double _basisVSWOnHand;   // TT#1401 - AGallagher - VSW
			private double _basisIntransit;
			private double _styleSales;
			private double _styleStock;
			private double _styleOnHand;
			private double _styleOHandIT;
            private double _styleVSWOnHand;   // TT#1401 - AGallagher - VSW
			private double _styleIntransit;
			private double _avgWeeklySales;
			private double _avgWeeklyStock;
            private double _avgWeeklySupply; // TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
           	private bool _usrRule;
			private double _ruleQty;
            private double _ruleTypeQty; // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            private int _willShip;    // MID Track 4298 Integer types showing values with decimals
			private int _transfer;    // MID Track 4298 Integer types showing values with decimals
			private bool _reCalcWillShip;
			private eVelocityRuleType _ruleType;
			private bool _sglRule;
			private string _sglGrpCellKey;
			private string _sglGrpVelocityGrade;
			private double _sglGrpVelocityGradeIDX;
			private double _sglGrpPctSellThruIndex;
            private int _sglGrpPctSellThruRow; // TT#586 Velocity variables not calculated correctly
			private string _sglChnCellKey;
			private string _sglChnVelocityGrade;
			private double _sglChnVelocityGradeIDX;
			private double _sglChnPctSellThruIndex;
            private int _sglChnPctSellThruRow; // TT#586 Velocity variables not calculated correctly
			private bool _totRule;
			private string _totGrpCellKey;
			private string _totGrpVelocityGrade;
			private double _totGrpVelocityGradeIDX;
			private double _totGrpPctSellThruIndex;
            private int _totGrpPctSellThruRow;  // TT#586 Velocity Variables not calculated correctly
			private string _totChnCellKey;
			private string _totChnVelocityGrade;
			private double _totChnVelocityGradeIDX;
			private double _totChnPctSellThruIndex;
            private int _totChnPctSellThruRow; // TT#586 Velocity Variables not calculated correctly

            //tt#152 - velocity balance - apicchetti
            private double _initRuleQty;
            private int _initWillShip;
            private eVelocityRuleType _initRuleType;
            //tt#152 - velocity balance - apicchetti
            
            private double _initRuleTypeQty; // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

            // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
            private eVelocityRuleType _spreadRuleType;
            private double _spreadRuleQty;
            private double _spreadRuleTypeQty;
            // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            private double _ibbasisOHandIT;
            // END TT#1287 - AGallagher - Inventory Min/Max

            //Begin TT#855-MD -jsobek -Velocity Enhancements
            private int _velocityGradeMinimum;
            private int _velocityGradeAdMinimum;
            private int _velocityGradeMaximum;
            private double _velocityBalanceHeaderProportionalIndex;
            private bool _isManuallyAllocated;
            private bool _isSimilarStoreModel;
            //End TT#855-MD -jsobek -Velocity Enhancements
        
			public int StoreRID
			{
				get
				{
					return _storeRID;
				}

				set
				{
					_storeRID = value;
				}
			}

			public int GrpLvlRID
			{
				get
				{
					return _groupLvlRID;
				}

				set
				{
					_groupLvlRID = value;
				}
			}

			public bool IsReserve
			{
				get
				{
					return _isReserve;
				}

				set
				{
					_isReserve = value;
				}
			}

			public bool IsEligible
			{
				get
				{
					return _isEligible;
				}

				set
				{
					_isEligible = value;
				}
			}

			public string Grade
			{
				get
				{
					return _grade;
				}

				set
				{
					_grade = value;
				}
			}

			public int GradeIDX
			{
				get
				{
					return _gradeIDX;
				}

				set
				{
					_gradeIDX = value;
				}
			}

			// Begin TT #91 - stodd
			public string BasisGrade
			{
				get
				{
					return _basisGrade;
				}

				set
				{
					_basisGrade = value;
				}
			}

			public int BasisGradeIDX
			{
				get
				{
					return _basisGradeIDX;
				}

				set
				{
					_basisGradeIDX = value;
				}
			}
			// End TT #91

			public DateTime ShipDay
			{
				get
				{
					return _shipDay;
				}

				set
				{
					_shipDay = value;
				}
			}

			public double Need
			{
				get
				{
					return _need;
				}

				set
				{
					_need = value;
				}
			}

			public double PctNeed
			{
				get
				{
					return _pctNeed;
				}

				set
				{
					_pctNeed = value;
				}
			}

			public int Capacity  // MID Track 4298 Integer types showing with decimals
			{
				get
				{
					return _capacity;
				}

				set
				{
					_capacity = value;
				}
			}

			public int PrimaryMaximum // MID Track 4298 Integer types showing with decimals
			{
				get
				{
					return _primaryMax;
				}

				set
				{
					_primaryMax = value;
				}
			}

			public int QtyAllocated // MID Track 4298 Integer types showing with decimals
			{
				get
				{
					return _qtyAllocated;
				}

				set
				{
					_qtyAllocated = value;
				}
			}

			// begin MID Track 4282 Velocity overlays Fill Size Holes allocation
			/// <summary>
			/// Gets or sets units pre-allocated by size
			/// </summary>
			public int PreSizeAllocated
			{
				get
				{
					return _preSizeAllocated;
				}
				set
				{
					_preSizeAllocated = value;
				}
			}
			// end MID Track 4282 Velocity overlays Fill Size Holes allocation

			public double PlanSales
			{
				get
				{
					return _planSales;
				}

				set
				{
					_planSales = value;
				}
			}

			public double PlanStock
			{
				get
				{
					return _planStock;
				}

				set
				{
					_planStock = value;
				}
			}

			public double BasisSales
			{
				get
				{
					return _basisSales;
				}

				set
				{
					_basisSales = value;
				}
			}

			public double BasisStock
			{
				get
				{
					return _basisStock;
				}

				set
				{
					_basisStock = value;
				}
			}

			public double BasisOnHand
			{
				get
				{
					return _basisOnHand;
				}

				set
				{
					_basisOnHand = value;
				}
			}

			public double BasisOHandIT
			{
				get
				{
					return _basisOHandIT;
				}

				set
				{
					_basisOHandIT = value;
				}
			}

			public double BasisIntransit
			{
				get
				{
					return _basisIntransit;
				}

				set
				{
					_basisIntransit = value;
				}
			}

            // BEGIN TT#1401 - AGallagher - VSW
            public double BasisVSWOnHand
            {
                get
                {
                    return _basisVSWOnHand;
                }

                set
                {
                    _basisVSWOnHand = value;
                }
            }
            // END TT#1401 - AGallagher - VSW


			public double StyleSales
			{
				get
				{
					return _styleSales;
				}

				set
				{
					_styleSales = value;
				}
			}

			public double StyleStock
			{
				get
				{
					return _styleStock;
				}

				set
				{
					_styleStock = value;
				}
			}

			public double StyleOnHand
			{
				get
				{
					return _styleOnHand;
				}

				set
				{
					_styleOnHand = value;
				}
			}

			public double StyleOHandIT
			{
				get
				{
					return _styleOHandIT;
				}

				set
				{
					_styleOHandIT = value;
				}
			}

            // BEGIN TT#1401 - AGallagher - VSW
            public double StyleVSWOnHand
            {
                get
                {
                    return _styleVSWOnHand;
                }

                set
                {
                    _styleVSWOnHand = value;
                }
            }
            // END TT#1401 - AGallagher - VSW

			public double StyleIntransit
			{
				get
				{
					return _styleIntransit;
				}

				set
				{
					_styleIntransit = value;
				}
			}

			public double AvgWeeklySales
			{
				get
				{
					return _avgWeeklySales;
				}

				set
				{
					_avgWeeklySales = value;
				}
			}

			public double AvgWeeklyStock
			{
				get
				{
					return _avgWeeklyStock;
				}

				set
				{
					_avgWeeklyStock = value;
				}
			}

            
            // BEGIN TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)           
            public double AvgWeeklySupply
			{
				get
				{
					return _avgWeeklySupply;
				}

				set
				{
					_avgWeeklySupply = value;
				}
			}
            // END TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
            public bool UserRule
			{
				get
				{
					return _usrRule;
				}

				set
				{
					_usrRule = value;
				}
			}

			public double RuleQty
			{
				get
				{
					return _ruleQty;
				}

				set
				{
					_ruleQty = value;
				}
			}

            // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            public double RuleTypeQty
            {
                get
                {
                    return _ruleTypeQty;
                }

                set
                {
                    _ruleTypeQty = value;
                }
            }
            // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

			public int WillShip // MID Track 4298 Integer types showing with decimals
			{
				get
				{
					return _willShip;
				}

				set
				{
					_willShip = value;
				}
			}

            public int InitialWillShip //tt#152 - Velocity balance - apicchetti
            {
                get
                {
                    return _initWillShip;
                }
                set
                {
                    _initWillShip = value;
                }
            }

            public double InitialRuleQuantity //tt#152 - Velocity balance - apicchetti
            {
                get
                {
                    return _initRuleQty;
                }
                set
                {
                    _initRuleQty = value;
                }
            }

            public eVelocityRuleType InitialRuleType //tt#152 - Velocity balance - apicchetti
            {
                get
                {
                    return _initRuleType;
                }
                set
                {
                    _initRuleType = value;
                }
            }

            // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            public double InitialRuleTypeQty
            {
                get
                {
                    return _initRuleTypeQty;
                }
                set
                {
                    _initRuleTypeQty = value;
                }
            }
            // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

            // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing

            public eVelocityRuleType SpreadRuleType 
            {
                get
                {
                    return _spreadRuleType;
                }
                set
                {
                    _spreadRuleType = value;
                }
            }

            public double SpreadRuleTypeQty
            {
                get
                {
                    return _spreadRuleTypeQty;
                }
                set
                {
                    _spreadRuleTypeQty = value;
                }
            }
            public double SpreadRuleQty
            {
                get
                {
                    return _spreadRuleQty;
                }
                set
                {
                    _spreadRuleQty = value;
                }
            }

            // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            public double IBBasisOHandIT
            {
                get
                {
                    return _ibbasisOHandIT;
                }

                set
                {
                    _ibbasisOHandIT = value;
                }
            }
            // END TT#1287 - AGallagher - Inventory Min/Max

            public int Transfer // MID Track 4298 Integer types showing with decimals
			{
				get
				{
					return _transfer;
				}

				set
				{
					_transfer = value;
				}
			}

			public bool ReCalcWillShip
			{
				get
				{
					return _reCalcWillShip;
				}

				set
				{
					_reCalcWillShip = value;
				}
			}

			public eVelocityRuleType RuleType
			{
				get
				{
					return _ruleType;
				}

				set
				{
					_ruleType = value;
				}
			}

			public bool SGLRule
			{
				get
				{
					return _sglRule;
				}

				set
				{
					_sglRule = value;
				}
			}

			public string SGLGrpCellKey
			{
				get
				{
					return _sglGrpCellKey;
				}

				set
				{
					_sglGrpCellKey = value;
				}
			}

			public string SGLGrpVelocityGrade
			{
				get
				{
					return _sglGrpVelocityGrade;
				}

				set
				{
					_sglGrpVelocityGrade = value;
				}
			}

			public double SGLGrpVelocityGradeIDX
			{
				get
				{
					return _sglGrpVelocityGradeIDX;
				}

				set
				{
					_sglGrpVelocityGradeIDX = value;
				}
			}

			public double SGLGrpPctSellThruIndex
			{
				get
				{
					return _sglGrpPctSellThruIndex;
				}

				set
				{
					_sglGrpPctSellThruIndex = value;
				}
			}

            // begin TT#586 Velocity variables not calculated correctly
            public int SGLGrpPctSellThruRow
            {
                get { return _sglGrpPctSellThruRow; }
                set { _sglGrpPctSellThruRow = value; }
            }
            // end TT#586 Velocity variables not calculated correctly

			public string SGLChnCellKey
			{
				get
				{
					return _sglChnCellKey;
				}

				set
				{
					_sglChnCellKey = value;
				}
			}

			public string SGLChnVelocityGrade
			{
				get
				{
					return _sglChnVelocityGrade;
				}

				set
				{
					_sglChnVelocityGrade = value;
				}
			}

			public double SGLChnVelocityGradeIDX
			{
				get
				{
					return _sglChnVelocityGradeIDX;
				}

				set
				{
					_sglChnVelocityGradeIDX = value;
				}
			}

			public double SGLChnPctSellThruIndex
			{
				get
				{
					return _sglChnPctSellThruIndex;
				}

				set
				{
					_sglChnPctSellThruIndex = value;
				}
			}

            // begin TT#586 Velocity variables not calculated correctly
            public int SGLChnPctSellThruRow
            {
                get { return _sglChnPctSellThruRow; }
                set { _sglChnPctSellThruRow = value; }
            }
            // end TT#586 Velocity variables not calculated correctly

			public bool TotRule
			{
				get
				{
					return _totRule;
				}

				set
				{
					_totRule = value;
				}
			}

			public string TotGrpCellKey
			{
				get
				{
					return _totGrpCellKey;
				}

				set
				{
					_totGrpCellKey = value;
				}
			}

			public string TotGrpVelocityGrade
			{
				get
				{
					return _totGrpVelocityGrade;
				}

				set
				{
					_totGrpVelocityGrade = value;
				}
			}

			public double TotGrpVelocityGradeIDX
			{
				get
				{
					return _totGrpVelocityGradeIDX;
				}

				set
				{
					_totGrpVelocityGradeIDX = value;
				}
			}

			public double TotGrpPctSellThruIndex
			{
				get
				{
					return _totGrpPctSellThruIndex;
				}

				set
				{
					_totGrpPctSellThruIndex = value;
				}
			}

            // begin TT#586 Velocity variables not calculated correctly
            public int TotGrpPctSellThruRow
            {
                get { return _totGrpPctSellThruRow; }
                set { _totGrpPctSellThruRow = value; }
            }
            // end TT#586 Velocity variables not calculated correctly

			public string TotChnCellKey
			{
				get
				{
					return _totChnCellKey;
				}

				set
				{
					_totChnCellKey = value;
				}
			}

			public string TotChnVelocityGrade
			{
				get
				{
					return _totChnVelocityGrade;
				}

				set
				{
					_totChnVelocityGrade = value;
				}
			}

			public double TotChnVelocityGradeIDX
			{
				get
				{
					return _totChnVelocityGradeIDX;
				}

				set
				{
					_totChnVelocityGradeIDX = value;
				}
			}

			public double TotChnPctSellThruIndex
			{
				get
				{
					return _totChnPctSellThruIndex;
				}

				set
				{
					_totChnPctSellThruIndex = value;
				}
			}

            // begin TT#586 Velocity variables not calculated correctly
            public int TotChnPctSellThruRow
            {
                get { return _totChnPctSellThruRow; }
                set { _totChnPctSellThruRow = value; }
            }
            // end TT#586 Velocity variables not calculated correctly

            //Begin TT#855-MD -jsobek -Velocity Enhancments
            public int VelocityGradeMinimum
            {
                get { return _velocityGradeMinimum; }
                set { _velocityGradeMinimum = value; }
            }
            public int VelocityGradeAdMinimum
            {
                get { return _velocityGradeAdMinimum; }
                set { _velocityGradeAdMinimum = value; }
            }
            public int VelocityGradeMaximum
            {
                get { return _velocityGradeMaximum; }
                set { _velocityGradeMaximum = value; }
            }
            public double VelocityBalanceHeaderProportionalIndex
            {
                get { return _velocityBalanceHeaderProportionalIndex; }
                set { _velocityBalanceHeaderProportionalIndex = value; }
            }
            public bool IsManuallyAllocated
            {
                get { return _isManuallyAllocated; }
                set { _isManuallyAllocated = value; }
            }
            public bool IsSimilarStoreModel
            {
                get { return _isSimilarStoreModel; }
                set { _isSimilarStoreModel = value; }
            }
            //End TT#855-MD -jsobek -Velocity Enhancments

			// ===========
			// Constructor
			// ===========
			/// <summary>
			/// Creates an instance of the StoreDataValue structure
			/// </summary>
			public StoreDataValue()
			{
			}
		}

		// ===========
		// Constructor
		// ===========
		/// <summary>
		/// Creates an instance of the Velocity Method
		/// </summary>
		/// <param name="aMethodRID">RID that identifies the method</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public VelocityMethod(SessionAddressBlock aSAB, int aMethodRID):base(aSAB, aMethodRID, eMethodType.Velocity)
		public VelocityMethod(SessionAddressBlock aSAB, int aMethodRID):base(aSAB, aMethodRID, eMethodType.Velocity, eProfileType.MethodVelocity)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_component = null;

			_isInteractive = false;

			_VMD = new VelocityMethodData(aMethodRID);

			_headerData = new Hashtable();

			if (base.Filled)
			{
				_VMD.PopulateVelocity(aMethodRID);
				_SG_RID = _VMD.StoreGroup_RID;
				_OTS_Plan_MdseHnRID = _VMD.OTS_Plan_HN_RID;
				_OTS_Plan_ProdHnRID = _VMD.OTS_Plan_PH_RID;
				_OTS_Begin_CDR_RID = _VMD.OTS_Begin_CDR_RID;
				_OTS_Plan_ProdHnLvlSeq = _VMD.OTS_Plan_PHL_SEQ;
				_OTS_ShipTo_CDR_RID = _VMD.OTS_Ship_To_CDR_RID;
				_TrendPctContribution = Include.ConvertCharToBool(_VMD.Trend_Percent);
				_UseSimilarStoreHistory = Include.ConvertCharToBool(_VMD.Sim_Store_Ind);
				_CalculateAverageUsingChain = Include.ConvertCharToBool(_VMD.Avg_Using_Chain_Ind);
				_DetermineShipQtyUsingBasis = Include.ConvertCharToBool(_VMD.Ship_Using_Basis_Ind);

                // Begin TT#313 - JSmith -  balance does not remain checked
                _balance = Include.ConvertCharToBool(_VMD.Balance_Ind);
                // End TT#313
                

                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                _ApplyMinMaxInd = _VMD.Apply_Min_Max_Ind; 
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

                // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                _reconcile = Include.ConvertCharToBool(_VMD.Reconcile_Ind);
                // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                _InventoryInd = _VMD.Inventory_Ind;
                _MERCH_HN_RID = _VMD.MERCH_HN_RID;
                _MERCH_PH_RID = _VMD.MERCH_PH_RID;
                _MERCH_PHL_SEQ = _VMD.MERCH_PHL_SEQ;
                // END TT#1287 - AGallagher - Inventory Min/Max

                _gradeVariableType = _VMD.GradeVariableType; //TT#855-MD -jsobek -Velocity Enhancements
                _balanceToHeaderInd = _VMD.BalanceToHeaderInd; //TT#855-MD -jsobek -Velocity Enhancements

                // Begin Track #6074
				// Begin TT # 91 - stodd
				//_gradesByBasisInd = _VMD.GradesByBasisInd;
				// End TT # 91 - stodd
				// End Track #6074
			}
			else
			{
				_OTS_Plan_ProdHnLvlSeq = 0;
				_TrendPctContribution = false;
				_UseSimilarStoreHistory = false;
				_CalculateAverageUsingChain = true;
				_DetermineShipQtyUsingBasis = true;
				_OTS_Begin_CDR_RID = Include.NoRID;
				_OTS_ShipTo_CDR_RID = Include.NoRID;
				_OTS_Plan_ProdHnRID = Include.NoRID;
				_OTS_Plan_MdseHnRID = Include.DefaultPlanHnRID;
				_SG_RID = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
				// Begin Track #6074
				// Begin TT # 91 - stodd
				//_gradesByBasisInd = SAB.ApplicationServerSession.GlobalOptions.DefaultGradesByBasis;
				// End TT # 91 - stodd
				// End Track #6074
                // Begin TT#313 - JSmith -  balance does not remain checked
                _balance = false;
                // End TT#313
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                _ApplyMinMaxInd = 'N';
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                _reconcile = false;
                // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                _InventoryInd = 'A';
                // BEGIN TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
                //_MERCH_HN_RID = Include.NoRID;
                //_MERCH_PH_RID = 1;
                //_MERCH_PHL_SEQ = 5;

                HierarchyProfile hp = aSAB.HierarchyServerSession.GetMainHierarchyData();

                for (int levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
                    //hlp.LevelID is level name 
                    //hlp.Level is level number 
                    //hlp.LevelType is level type 
                    if (hlp.LevelType == eHierarchyLevelType.Style)
                    {
                        //this._MerchType = eMerchandiseType.HierarchyLevel;
                        _MERCH_PHL_SEQ = hlp.Level;
                        _MERCH_PH_RID = hp.Key;
                        _MERCH_HN_RID = Include.NoRID;
                        //styleFound = true;
                        break;
                    }
                }
                // END TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results

                // END TT#1287 - AGallagher - Inventory Min/Max

            }

			_dsVelocity = _VMD.GetVelocityChildData();

			LoadDataArrays();


			// BEG MID Change j.ellis Add Audit Message
			_globalUserTypeText = null;
			// END MID Change j.ellis Add Audit Message

		}

		// ==========
		// Properties
		// ==========
		/// <summary>
		/// Gets the Profile Type
		/// </summary>
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodVelocity;
			}
		}

		/// <summary>
		/// Gets or sets the Component Type
		/// </summary>
		public GeneralComponent Component
		{
			get
			{
				if (_component == null)
				{
					_component = new GeneralComponent(eGeneralComponentType.Total);
				}

				return _component;
			}

			set
			{
				_component = value;
			}
		}

		/// <summary>
		/// Gets or sets the Interactive flag
		/// </summary>
		public bool IsInteractive
		{
			get
			{
				return _isInteractive;
			}

			set
			{
				_isInteractive = value;
			}
		}

		/// <summary>
		/// Gets or sets the Allocation Profile
		/// </summary>
		public AllocationProfile AlocProfile
		{
			get
			{
				return _alocProfile;
			}

			set
			{
				_alocProfile = value;
			}
		}

		/// <summary>
		/// Gets or sets the Application Session Transaction
		/// </summary>
		public ApplicationSessionTransaction AST
		{
			get
			{
				return _applicationTransaction;
			}

			set
			{
				_applicationTransaction = value;
			}
		}

		/// <summary>
		/// Gets or sets the Store Group RID
		/// </summary>
		public int StoreGroupRID
		{
			get
			{
				return _SG_RID;
			}

			set
			{
				_SG_RID = value;
			}
		}

		/// <summary>
		/// Gets or sets OTS Plan RID
		/// </summary>
		public int OTSPlanHNRID
		{
			get
			{
				return _OTS_Plan_MdseHnRID;
			}

			set
			{
				_OTS_Plan_MdseHnRID = value;
			}
		}

		/// <summary>
		/// Gets or sets OTS Plan product hierarchy level reference used to dynamically find the allocation plan
		/// </summary>
		public int OTSPlanPHRID
		{
			get
			{
				return _OTS_Plan_ProdHnRID;
			}

			set
			{
				_OTS_Plan_ProdHnRID = value;
			}
		}

		public int OTSPlanPHLSeq
		{
			get
			{
				return _OTS_Plan_ProdHnLvlSeq;
			}

			set
			{
				_OTS_Plan_ProdHnLvlSeq = value;
			}
		}

		/// <summary>
		/// Gets or sets the Begin Plan Date
		/// </summary>
		public int OTS_Begin_CDR_RID
		{
			get
			{
				return _OTS_Begin_CDR_RID;
			}

			set
			{
				_OTS_Begin_CDR_RID = value;
			}
		}

		/// <summary>
		/// Gets or sets the Ship Date
		/// </summary>
		public int OTS_Ship_To_CDR_RID
		{
			get
			{
				return _OTS_ShipTo_CDR_RID;
			}

			set
			{
				_OTS_ShipTo_CDR_RID = value;
			}
		}

		/// <summary>
		/// Gets or sets the Use Similar Store History flag
		/// </summary>
		public bool UseSimilarStoreHistory
		{
			get
			{
				return _UseSimilarStoreHistory;
			}

			set
			{
				_UseSimilarStoreHistory = value;
			}
		}

		/// <summary>
		/// Gets or sets the Calculate Average Using Chain flag
		/// </summary>
		public bool CalculateAverageUsingChain
		{
			get
			{
				return _CalculateAverageUsingChain;
			}

			set
			{
				_CalculateAverageUsingChain = value;
			}
		}


		/// <summary>
		/// Gets or sets the Determine Ship Qty Using Basis flag
		/// </summary>
		public bool DetermineShipQtyUsingBasis
		{
			get
			{
				return _DetermineShipQtyUsingBasis;
			}

			set
			{
				_DetermineShipQtyUsingBasis = value;
			}
		}

		/// <summary>
		/// Gets or sets the if the basis data is used to determine store grades
		/// </summary>
		public bool GradesByBasisInd
		{
			get
			{
				return _gradesByBasisInd;
			}

			set
			{
				_gradesByBasisInd = value;
			}
		}
        //Begin TT#855-MD -jsobek -Velocity Enhancements
        public eVelocityMethodGradeVariableType GradeVariableType
        {
            get
            {
                return _gradeVariableType;
            }

            set
            {
                _gradeVariableType = value;
            }
        }
        public char BalanceToHeaderInd
        {
            get
            {
                return _balanceToHeaderInd;
            }

            set
            {
                _balanceToHeaderInd = value;
            }
        }
        //End TT#855-MD -jsobek -Velocity Enhancements
        
		/// <summary>
		/// Gets or sets the Trend Pct Contribution flag
		/// </summary>
		public bool TrendPctContribution
		{
			get
			{
				return _TrendPctContribution;
			}

			set
			{
				_TrendPctContribution = value;
			}
		}

		/// <summary>
		/// Gets or sets the Velocity Child Occurs data
		/// </summary>
		public DataSet DSVelocity
		{
			get
			{
				return _dsVelocity;
			}

			set
			{
				_dsVelocity = value;
			}
		}

        //tt#152 - Velocity Balance - apicchetti
        /// <summary>
        /// Gets or sets the flag that will be analyzed to balance the velocity results or not
        /// </summary>
        public bool Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
            }
        }
        //tt#152 - Velocity Balance - apicchetti

        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
        public char ApplyMinMaxInd
        {
            get
            {
                return _ApplyMinMaxInd;
            }
            set
            {
                _ApplyMinMaxInd= value;
            }
        }
        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public char InventoryInd
        {
            get
            {
                return _InventoryInd;
            }
            set
            {
                _InventoryInd = value;
            }
        }
        public int MERCH_HN_RID
        {
            get
            {
                return _MERCH_HN_RID;
            }

            set
            {
                _MERCH_HN_RID = value;
            }
        }

        public int MERCH_PH_RID
        {
            get
            {
                return _MERCH_PH_RID;
            }

            set
            {
                _MERCH_PH_RID = value;
            }
        }

        public int MERCH_PHL_SEQ
        {
            get
            {
                return _MERCH_PHL_SEQ;
            }

            set
            {
                _MERCH_PHL_SEQ = value;
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max


        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
        public bool Reconcile
        {
            get
            {
                return _reconcile;
            }
            set
            {
                _reconcile = value;
            }
        }
        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        public eMerchandiseType MerchandiseType
        {
            get { return _merchandiseType; }
            set { _merchandiseType = value; }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max
        

        // begin TT#533 Velocity variables calculatd incorrectly
        ////tt#153 - velocity matrix variables - apicchetti
        //public double TotalSales_AllStores
        //{
        //    get
        //    {
        //        return _totalSales_All;
        //    }
        //}

        //public double AvgSales_AllStores
        //{
        //    get
        //    {
        //        return _avgSales_All;
        //    }
        //}

        //public double AvgSalesIndex_AllStores
        //{
        //    get
        //    {
        //        return _avgSalesIdx_All;
        //    }
        //}

        //public int TotalStores_AllStores
        //{
        //    get
        //    {
        //        return _totalStores_All;
        //    }
        //}
        ////tt#153 - velocity matrix variables - apicchetti
        // end TT#533 velocity variables calculated incorrectly

		// Begin TT#4522 - stodd - velocity matrix wrong
        public bool BasisChangesMade
        {
            get { return _basisChangesMade; }
            set { _basisChangesMade = value; }
        }
		// End TT#4522 - stodd - velocity matrix wrong

        // =======
		// Methods
		// =======

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsStoreGroupUser(_SG_RID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(_OTS_Plan_MdseHnRID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(_MERCH_HN_RID))
            {
                return true;
            }

            foreach (DataRow dr in _dsVelocity.Tables["Basis"].Rows)
            {
                if (dr["BasisHNRID"] != DBNull.Value
                    && IsHierarchyNodeUser(Convert.ToInt32(dr["BasisHNRID"])))
                {
                    return true;
                }
            }
 
            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		/// <summary>
		/// Load the data arrays from the data source tables
		/// </summary>
		public void LoadDataArrays()
		{
			_BasisRows = _dsVelocity.Tables["Basis"].Rows.Count;

			if (_BasisRows > 0)
			{
				LoadBasis();
			}

//			_SlsPrdRows = _dsVelocity.Tables["SalesPeriod"].Rows.Count;
//
//			if (_SlsPrdRows > 0)
//			{
//				LoadSalesPeriod();
//			}

			_GradeRows = _dsVelocity.Tables["VelocityGrade"].Rows.Count;

            if (_GradeRows > 0)
            {
                LoadVelocityGrade();
            }

           	_PSTRows = _dsVelocity.Tables["SellThru"].Rows.Count;

			if (_PSTRows > 0)
			{
				LoadSellThru();
			}

			if (_GradeRows > 0 && _PSTRows > 0)
			{
				LoadMatrix();
			}
		}

		// ====================================
		// Load the Basis Merchandise hashtable
		// ====================================
		private void LoadBasis()
		{
			_basisMdseData = new Hashtable();
			_basisMdseData.Clear();

			foreach(DataRow bRow in _dsVelocity.Tables["Basis"].Rows)
			{
				BasisMdseNode bmn = new BasisMdseNode();

				bmn.Sequence = Convert.ToInt32(bRow["BasisSequence"], CultureInfo.CurrentUICulture);
				bmn.BasisFVRID = Convert.ToInt32(bRow["BasisFVRID"], CultureInfo.CurrentUICulture);
				bmn.BasisMdseHnRID = Convert.ToInt32(bRow["BasisHNRID"], CultureInfo.CurrentUICulture);
				bmn.BasisProdHnRID = Convert.ToInt32(bRow["BasisPHRID"], CultureInfo.CurrentUICulture);
				bmn.BasisProdHnLvlSeq = Convert.ToInt32(bRow["BasisPHLSequence"], CultureInfo.CurrentUICulture);
				bmn.BasisTimeCdrRID = Convert.ToInt32(bRow["cdrRID"], CultureInfo.CurrentUICulture);
				bmn.BasisWeightFactor = Convert.ToDouble(bRow["Weight"], CultureInfo.CurrentUICulture);

				_basisMdseData.Add(bmn.Sequence, bmn);
			}
		}

//		// =====================================
//		// Load the Basis Time Periods hashtable
//		// =====================================
//		private void LoadSalesPeriod()
//		{
//			_basisTimeData = new Hashtable();
//			_basisTimeData.Clear();
//
//			foreach(DataRow spRow in _dsVelocity.Tables["SalesPeriod"].Rows)
//			{
//				BasisTimePrd btp = new BasisTimePrd();
//
//				btp.Sequence = Convert.ToInt32(spRow["PeriodSequence"], CultureInfo.CurrentUICulture);
//				btp.BasisTimeCdrRID = Convert.ToInt32(spRow["cdrRID"], CultureInfo.CurrentUICulture);
//				btp.BasisWeightFactor = Convert.ToDouble(spRow["Weight"], CultureInfo.CurrentUICulture);
//
//				_basisTimeData.Add(btp.Sequence, btp);
//			}
//		}

		// ==========================================
		// Load the Grades and Lower Limits hashtable
		// ==========================================
		private void LoadVelocityGrade()
		{
			_gradeLowLimData = new Hashtable();
			_gradeLowLimData.Clear();

			_lowLimGradeData = new Hashtable();
			_lowLimGradeData.Clear();

            _lowLimSortedGradeData = new SortedList();
			_lowLimSortedGradeData.Clear();
            
            foreach(DataRow vgRow in _dsVelocity.Tables["VelocityGrade"].Rows)
			{
				GradeLowLimit gll = new GradeLowLimit();

				gll.Row = Convert.ToInt32(vgRow["RowPosition"], CultureInfo.CurrentUICulture);
				gll.Grade = vgRow["Grade"].ToString();
				gll.LowerLimit = Convert.ToInt32(vgRow["Boundary"], CultureInfo.CurrentUICulture);
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                if (vgRow["MinStock"] == DBNull.Value) 
                    { gll.AllocMin = 0; }
                   else
                    { gll.AllocMin = Convert.ToInt32(vgRow["MinStock"], CultureInfo.CurrentUICulture); }
                if (vgRow["MaxStock"] == DBNull.Value)
                    { gll.AllocMax = int.MaxValue; }
                   else
                    { gll.AllocMax = Convert.ToInt32(vgRow["MaxStock"], CultureInfo.CurrentUICulture); }
                if (vgRow["MinAd"] == DBNull.Value)
                    { gll.AllocAdMin = 0; }
                   else
                    { gll.AllocAdMin = Convert.ToInt32(vgRow["MinAd"], CultureInfo.CurrentUICulture); }
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                _gradeLowLimData.Add(gll.Grade, gll);
				_lowLimGradeData.Add(gll.LowerLimit, gll);
                _lowLimSortedGradeData.Add(-gll.LowerLimit, gll);
            }
		}

        // ========================================
		// Load the Pct Sell Thru Indices hashtable
		// ========================================
		private void LoadSellThru()
		{
			_pctSellThruData = new Hashtable();
			_pctSellThruData.Clear();

			_pctSellThruSortedData = new SortedList();
			_pctSellThruSortedData.Clear();

			foreach(DataRow stRow in _dsVelocity.Tables["SellThru"].Rows)
			{
				PctSellThruIndex psti = new PctSellThruIndex();

				psti.Row = Convert.ToInt32(stRow["RowPosition"], CultureInfo.CurrentUICulture);
				psti.SellThruIndex = Convert.ToInt32(stRow["SellThruIndex"], CultureInfo.CurrentUICulture);

				_pctSellThruData.Add(psti.SellThruIndex, psti);
				_pctSellThruSortedData.Add(-psti.SellThruIndex, psti);
			}
		}

		// ==========================
		// Load the Matrix hashtables
		// ==========================
		private void LoadMatrix()
		{
			//int LoopCount = 0;  // TT#533 velocity variables not calculated correctly
			int MatrixCount = 0;
            
            // begin TT#533 velocity variables not calculatd correctly
			//string groupSelect = null;
			//string matrixSelect = null;
            // TT#533 velocity variables not calculated correctly

			StoreGroupProfile sgp = null;

			// ============================
			// Create full matrix hashtable
			// ============================
			_groupLvlMtrxData = new Hashtable();
			_groupLvlMtrxData.Clear();

			// ======================
			// Get store group levels
			// ======================
            sgp = StoreMgmt.StoreGroup_GetFilled(_SG_RID); //SAB.StoreServerSession.GetStoreGroupFilled(_SG_RID);

			_SGLRows = sgp.GroupLevels.Count;
			MatrixCount = _SGLRows + 1;

			// =============================
			// UGLY, but it apparently works
			// =============================

			// begin TT#533 velocity variables not calculated correctly
            // restructured code
            //			LoopCount = 1;

            //while(LoopCount <= MatrixCount)
			//{
			//	if (LoopCount < MatrixCount)
			//	{
            // end TT#533 velocity variables not calculated correctly
            // ==================================
			// Add entries for store group levels
			// ==================================
            GroupLvlMatrix glm;
			foreach(StoreGroupLevelProfile sglp in sgp.GroupLevels)
			{
                // begin TT#533 velocity variables not calculated correctly
                glm = GetGroupLvlMatrix(sglp.Key);
                //glm.NoOnHandRuleQty = Include.NoRuleQty;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule
                //glm.NoOnHandRuleType = eVelocityRuleType.None;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule
                // moved creation/initialization of glm to GetInitialGroupLvlMatrix();
                _groupLvlMtrxData.Add(glm.SglRID, glm);
			}
		    // ==========================
			// Add entry for total matrix
			// ==========================
            glm = GetGroupLvlMatrix(Include.TotalMatrixLevelRID);
            //glm.NoOnHandRuleQty = Include.NoRuleQty;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule
            //glm.NoOnHandRuleType = eVelocityRuleType.None;   // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule
            _groupLvlMtrxData.Add(glm.SglRID, glm);
		}

        // begin TT#533 velocity variables not calculated correctly
        private GroupLvlMatrix GetGroupLvlMatrix(int aSglRID)
        {
            GroupLvlMatrix glm = new GroupLvlMatrix();
            glm.SglRID = aSglRID;
            InitializeGroupLvlMatrix(glm);
            glm.NoOnHandRuleQty = Include.NoRuleQty;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule
            glm.NoOnHandRuleType = eVelocityRuleType.None;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule
            string groupSelect = "SglRID = " + glm.SglRID;
            DataRow[] groupRows = _dsVelocity.Tables["GroupLevel"].Select(groupSelect, null, DataViewRowState.CurrentRows);

            // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
            glm.ModeInd = 'N';  
            glm.SpreadInd = 'S';  
            // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
            if (groupRows.Length > 0)
            {
                if (!Convert.IsDBNull(groupRows[0]["NoOnHandQty"]))
                {
                    glm.NoOnHandRuleQty = Convert.ToDouble(groupRows[0]["NoOnHandQty"], CultureInfo.CurrentUICulture);
                }

                if (!Convert.IsDBNull(groupRows[0]["NoOnHandRule"]))
                {
                    glm.NoOnHandRuleType = (eVelocityRuleType)Convert.ToInt32(groupRows[0]["NoOnHandRule"], CultureInfo.CurrentUICulture);
                }
                // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) 
                if (!Convert.IsDBNull(groupRows[0]["ModeInd"]))
                {
                    glm.ModeInd = Convert.ToChar(groupRows[0]["ModeInd"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(groupRows[0]["AverageRule"]))
                {
                    glm.AverageRule = (eVelocityRuleRequiresQuantity)Convert.ToInt32(groupRows[0]["AverageRule"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(groupRows[0]["AverageQty"]))
                {
                    glm.AverageQty = Convert.ToDouble(groupRows[0]["AverageQty"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(groupRows[0]["SpreadInd"]))
                {
                    glm.SpreadInd = Convert.ToChar(groupRows[0]["SpreadInd"], CultureInfo.CurrentUICulture);
                }
                // END TT#637 - AGallagher - Velocity - Spread Average (#7) 
            }
            _groupLvlGradData = new Hashtable();

            _groupLvlCellData = new Hashtable();
			// begin TT#586 Velocity variables not calculated correctly
            glm.MatrixSellThruTotalCells = new Dictionary<int, GroupSellThruTotalCell>();
            foreach (PctSellThruIndex psti in _pctSellThruData.Values)
            {
                GroupSellThruTotalCell gsttc = new GroupSellThruTotalCell();
                gsttc.SellThruIndex = psti.SellThruIndex;
                InitializeGroupSellThruTotalCell(gsttc);
                gsttc.Key = psti.Row;
                glm.MatrixSellThruTotalCells.Add(gsttc.Key, gsttc);
            }
            // end TT#586 Velocity variables not calculated correctly

            foreach (GradeLowLimit gll in _gradeLowLimData.Values)
            {
                GroupLvlGrade glg = new GroupLvlGrade();
                glg.Grade = gll.Grade;
                InitializeGroupLvlGrade(glg);
                _groupLvlGradData.Add(glg.Grade, glg);

                foreach (PctSellThruIndex psti in _pctSellThruData.Values)
                {
                    GroupLvlCell glc = new GroupLvlCell();
                    glc.Boundary = gll.LowerLimit;
                    glc.SellThruIndex = psti.SellThruIndex;
                    InitializeGroupLvlCell(glc);
                    glc.CellRuleQty = Include.NoRuleQty;
                    glc.CellRuleType = eVelocityRuleType.None;

                    glc.Key = MatrixCellKey(gll.Row, psti.Row);

                    string matrixSelect = "SglRID = " + glm.SglRID + " AND Boundary = " + gll.LowerLimit + " AND SellThruIndex = " + psti.SellThruIndex;
                    DataRow[] matrixRows = _dsVelocity.Tables["VelocityMatrix"].Select(matrixSelect, null, DataViewRowState.CurrentRows);

                    if (matrixRows.Length > 0)
                    {
                        if (!Convert.IsDBNull(matrixRows[0]["VelocityQty"]))
                        {
                            glc.CellRuleQty = Convert.ToDouble(matrixRows[0]["VelocityQty"], CultureInfo.CurrentUICulture);
                            glc.CellRuleTypeQty = Convert.ToDouble(matrixRows[0]["VelocityQty"], CultureInfo.CurrentUICulture); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        }

                        if (!Convert.IsDBNull(matrixRows[0]["VelocityRule"]))
                        {
                            glc.CellRuleType = (eVelocityRuleType)Convert.ToInt32(matrixRows[0]["VelocityRule"], CultureInfo.CurrentUICulture);
                        }
                    }

                    _groupLvlCellData.Add(glc.Key, glc);
                }

                glm.GradeSales = _groupLvlGradData;
                glm.MatrixCells = _groupLvlCellData;
            }
            return glm;
        }
        private void InitializeGroupLvlMatrix(GroupLvlMatrix aGLM)
        {
            aGLM.GroupSales = 0.0;
            aGLM.GroupOnHand = 0.0;
            aGLM.GroupAvgWOS = 0.0;
            aGLM.GroupPctSellThru = 0.0;

            aGLM.ChainSales = 0.0;
            aGLM.ChainOnHand = 0.0;
            aGLM.ChainAvgWOS = 0.0;
            aGLM.ChainPctSellThru = 0.0;

            aGLM.NoOnHandBasisStores = 0;
            aGLM.NoOnHandStyleStores = 0;
					

            aGLM.EligibleBasisStores = 0;
            aGLM.GrpBasisSales = 0;
            aGLM.GrpBasisStock = 0;
            aGLM.ChnBasisSales = 0;
            aGLM.ChnBasisStock = 0;
            aGLM.AvgGrpBasisSalesIndex = 0.0;
            aGLM.AvgGrpBasisSalesPctTot = 0.0;
            aGLM.AvgChnBasisSalesIndex = 0.0;
            aGLM.AvgChnBasisSalesPctTot = 0.0;
            aGLM.GrpStockPercentOfTotal = 0.0;
            aGLM.ChnStockPercentOfTotal = 0.0;
            // begin TT#587 Velocity Variables not calculated correctly
            aGLM.AvgChnBasisSales = 0.0;
            aGLM.AvgChnBasisStock = 0.0;
            aGLM.AvgGrpBasisSales = 0.0;
            aGLM.AvgGrpBasisStock = 0.0;
            // end TT#587 Velocity Variables not calculated correctly
        }
        private void InitializeGroupLvlGrade(GroupLvlGrade aGLG)
        {
            aGLG.TotGrpBasisSales = 0;
            aGLG.AvgGrpBasisSales = 0.0;
            aGLG.AvgGrpBasisSalesIndex = 0.0;
            aGLG.AvgGrpBasisSalesPctTot = 0.0;

            aGLG.TotChnBasisSales = 0;
            aGLG.AvgChnBasisSales = 0.0;
            aGLG.AvgChnBasisSalesIndex = 0.0;
            aGLG.AvgChnBasisSalesPctTot = 0.0;

        }
        private void InitializeGroupLvlCell(GroupLvlCell aGLC)
        {
            aGLC.CellGrpStores = 0;
            aGLC.CellGrpSales = 0.0;
            aGLC.CellGrpOnHand = 0.0;
            aGLC.CellGrpAvgWOS = 0.0;

            aGLC.CellChnStores = 0;
            aGLC.CellChnSales = 0.0;
            aGLC.CellChnOnHand = 0.0;
            aGLC.CellChnAvgWOS = 0.0;

        }
        // end TT#533 velocity variables not calculated correctly

        // begin TT#586 velocity variables not calculated correctly
        private void InitializeGroupSellThruTotalCell(GroupSellThruTotalCell aGSTTC)
        {
            aGSTTC.SellThruChnOnHand = 0.0;
            aGSTTC.SellThruChnSales = 0.0;
            aGSTTC.SellThruChnStores = 0;

            aGSTTC.SellThruGrpOnHand = 0.0;
            aGSTTC.SellThruGrpSales = 0.0;
            aGSTTC.SellThruGrpStores = 0;
        }
        // end TT#586 velocity variables not calculated correctly

        //BEGIN tt#153 - velocity matrix variables - apicchetti
        // begin TT#533 velocity variables calculated incorrectly
        //public double GetTotalSales_Set(ApplicationSessionTransaction trans, int Attrib, int AttribSet)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityTtlBasisSales(Attrib, AttribSet, null);
        //}

        //public int GetTotalStores_Set(ApplicationSessionTransaction trans, int Attrib, int AttribSet)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return (int)asp.GetStoreListVelocityTotalNumberOfStores(Attrib, AttribSet, null);
        //}

        //public double GetTotalSales_All(ApplicationSessionTransaction trans)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityTtlBasisSales(Include.NoRID, Include.NoRID, null);
        //}

        //public int GetTotalStores_All(ApplicationSessionTransaction trans)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return (int)asp.GetStoreListVelocityTotalNumberOfStores(Include.NoRID, Include.NoRID, null);
        //}

        //public double GetAvgSalesIndex_Set(ApplicationSessionTransaction trans, int Attrib, int AttribSet)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityAvgBasisIndex(Attrib, AttribSet, null);
        //}

        //public double GetStockPercentOfTotal(ApplicationSessionTransaction trans, int Attrib, int AttribSet)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityStockPercentOfTotal(Attrib, AttribSet, null);
        //}

        //public double GetTotalStock_Set(ApplicationSessionTransaction trans, int Attrib, int AttribSet)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityTtlBasisStock(Attrib, AttribSet, null);
        //}

        //public double GetTotalStock_All(ApplicationSessionTransaction trans)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityTtlBasisStock(Include.NoRID, Include.NoRID, null);
        //}
        // end TT#533 Velocity variables calculated incorrectly

        // begin TT#586 Velocity Variables not calculated correctly
        //public double GetAllocated_Set(ApplicationSessionTransaction trans, int Attrib, int AttribSet)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityAllocationPercentOfTotal(Attrib, AttribSet, null);
        //}

        //public double GetAllocated_All(ApplicationSessionTransaction trans)
        //{
        //    AllocationSubtotalProfile asp = trans.GetAllocationGrandTotalProfile();
        //    return asp.GetStoreListVelocityAllocationPercentOfTotal(Include.NoRID, Include.NoRID, null);
        //}
        public double GetSetAllocatedPctOfTotal(int aAttribute, int aSet, string aGrade)
        {
            AllocationSubtotalProfile asp = _applicationTransaction.GetAllocationGrandTotalProfile();
            return asp.GetStoreListVelocityAllocationPercentOfTotal(aAttribute, aSet, aGrade);
        }
        // end TT#586 Velocity variables not calculated correctly

        public Hashtable GetTotalNumberStores_SellThru(ApplicationSessionTransaction trans, int Attrib, int AttribSet, ArrayList Stores_sellthru_data)
        {
            Hashtable htReturn = new Hashtable();

            DataTable dtStoresSellThruSet = (DataTable)Stores_sellthru_data[1];
            DataTable dtStoresSellThru = (DataTable)Stores_sellthru_data[1];

            DataTable tempTable = new DataTable();

            if (Stores_sellthru_data.Count > 0)
            {
                DataView dvStoresSellThru;
                if (AttribSet == 0)
                {
                    dvStoresSellThru = new DataView(dtStoresSellThru, "SellThruIdx <> -1",
                        "SellThruIdx DESC", DataViewRowState.CurrentRows);
                }
                else
                {
                    dvStoresSellThru = new DataView(dtStoresSellThruSet, "SellThruIdx <> -1",
                        "SellThruIdx DESC", DataViewRowState.CurrentRows);
                }

                tempTable = dvStoresSellThru.ToTable(true, "SellThruIdx");

                for (int intSSTRow = 0; intSSTRow < tempTable.Rows.Count; intSSTRow++)
                {
                    string strSellThruIdx = tempTable.Rows[intSSTRow]["SellThruIdx"].ToString().Trim();

                    DataRow[] drStoresSellThru;
                    if (AttribSet == 0)
                    {
                        drStoresSellThru = dtStoresSellThru.Select("SellThruIdx = " + strSellThruIdx);
                    }
                    else
                    {
                        drStoresSellThru = dtStoresSellThruSet.Select("SellThruIdx = " + strSellThruIdx + "  and GroupKey = " + AttribSet);
                    }

                    htReturn.Add(strSellThruIdx, drStoresSellThru.Length);

                }
            }


            return htReturn;
        }

        public Hashtable GetAvgWOS_SellThru(ApplicationSessionTransaction trans, int Attrib, int AttribSet, ArrayList Stores_sellthru_data)
        {
            Hashtable htReturn = new Hashtable();

            DataTable dtStoresSellThruSet = (DataTable)Stores_sellthru_data[1];
            DataTable dtStoresSellThru = (DataTable)Stores_sellthru_data[1];

            DataTable tempTable = new DataTable();

            if (Stores_sellthru_data.Count > 0)
            {
                DataView dvStoresSellThru;
                if (AttribSet == 0)
                {
                    dvStoresSellThru = new DataView(dtStoresSellThru, "SellThruIdx <> -1",
                        "SellThruIdx DESC", DataViewRowState.CurrentRows);
                }
                else
                {
                    dvStoresSellThru = new DataView(dtStoresSellThruSet, "SellThruIdx <> -1",
                        "SellThruIdx DESC", DataViewRowState.CurrentRows);
                }

                tempTable = dvStoresSellThru.ToTable(true, "SellThruIdx");

                for (int intSSTRow = 0; intSSTRow < tempTable.Rows.Count; intSSTRow++)
                {
                    string strSellThruIdx = tempTable.Rows[intSSTRow]["SellThruIdx"].ToString().Trim();

                    DataRow[] drStoresSellThru;
                    if (AttribSet == 0)
                    {
                        drStoresSellThru = dtStoresSellThru.Select("SellThruIdx = " + strSellThruIdx);
                    }
                    else
                    {
                        drStoresSellThru = dtStoresSellThruSet.Select("SellThruIdx = " + strSellThruIdx + "  and GroupKey = " + AttribSet);
                    }

                    double dbTotalAvgWOS = 0;
                    bool row_present = false;
                    foreach (DataRow row in drStoresSellThru)
                    {
                        dbTotalAvgWOS = dbTotalAvgWOS + Convert.ToDouble(row["AvgWOS"]);
                        row_present = true;
                    }

                    double dbAvgWOS = 0;
                    if(row_present == true)
                    {
                        dbAvgWOS = dbTotalAvgWOS / drStoresSellThru.Length;
                    }

                    htReturn.Add(strSellThruIdx, dbAvgWOS);
                }

            }
            return htReturn;
        }
        //END tt#153 - velocity matrix variables - apicchetti

		public override void ProcessMethod(ApplicationSessionTransaction aApplicationTransaction, int aStoreFilter, Profile methodProfile)
		{
			bool continueProcess = true;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
			aApplicationTransaction.ResetAllocationActionStatus();

			_headerData.Clear();

			// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
			foreach(AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
			{
				// This is to adjust the headers that get processed when running velocity interactively against an assortment.
				//if (aApplicationTransaction.UseAssortmentSelectedHeaders)
				//{
				//    continueProcess = false;
				//    if (aApplicationTransaction.AssortmentSelectedHdrList.Contains(ap.Key))
				//    {
						continueProcess = true;
				//    }
				//}

				if (continueProcess)
				{
					AllocationWorkFlowStep awfs = new AllocationWorkFlowStep(this,
						//new GeneralComponent(eGeneralComponentType.Total), // MID Track 3182 Velocity not recognizing specific component
						this.Component, // MID Track 3182 Velocity Not Recognizing specific component
						false,
						true,
						aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
						aStoreFilter,
						-1);

					ProcessAction(aApplicationTransaction.SAB, aApplicationTransaction, awfs, ap, true, Include.NoRID);
				}
			}
			// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		}

		public override void ProcessAction(SessionAddressBlock aSAB,
			ApplicationSessionTransaction aAST,
			ApplicationWorkFlowStep aAWFS,
			Profile aAP,      // MID Track 4094 'color' not recognized by velocity
			bool WriteToDB,
			int aStoreFilterRID)
		{
			string glcKey = null;
			string message = null;

			double chainSales = 0.0;
			double chainOnHand = 0.0;
            // begin TT#533 velocity variable not calculated correctly
            int chainBasisSales = 0;
            int chainBasisStock = 0;
            // end TT#533 velocity variable not calculated correctly

			int grpLvlRID = Include.NoRID;
			int headerRID = Include.NoRID;
			int storeRID = Include.UndefinedStoreRID;

			StoreGroupProfile sgp = null;

			SessionAddressBlock SAB = null;

			IntransitKeyType intransKeyType;

			AllocationWorkFlowStep AWFS = null;

			bool saveCalculateAverageUsingChain;

			AllocationSubtotalProfile asp = null;

			SAB = (SessionAddressBlock)aSAB;

			AST = (ApplicationSessionTransaction)aAST;

			AllocationColorSizeComponent preSize_ColorSizeComponent; // MID Track 4282 Velocity overlays Fill Size Holes allocation

			AST.ResetVelocity(this);

			AWFS = (AllocationWorkFlowStep)aAWFS;

            // begin TT#587 Velocity Variables not calculated correctly
            //double storesAvgWeeklySalesSet = 0;
            //double storeStoreBasisStockSet = 0;
            //double storesAvgWeeklySales = 0;
            //double storeStoreBasisStock = 0;
            // end TT#587 Velocity Variables not calculated correctly
            
            // begin TT#586 Velocity Variables not calculating correctly
            ////BEGIN tt#153 - Velocity Matrix Variables - apicchetti
            //DataTable store_sell_thrus_set = new DataTable();
            //DataColumn col = new DataColumn();
            //col.DataType = System.Type.GetType("System.Int32");
            //col.ColumnName = "SellThruIdx";
            //store_sell_thrus_set.Columns.Add(col);
            //col = new DataColumn();
            //col.DataType = System.Type.GetType("System.Double");
            //col.ColumnName = "AvgWOS";
            //store_sell_thrus_set.Columns.Add(col);
            //col = new DataColumn();
            //col.DataType = System.Type.GetType("System.String");
            //col.ColumnName = "GroupKey";
            //store_sell_thrus_set.Columns.Add(col);

            //DataTable store_sell_thrus = new DataTable();
            //col = new DataColumn();
            //col.DataType = System.Type.GetType("System.Int32");
            //col.ColumnName = "SellThruIdx";
            //store_sell_thrus.Columns.Add(col);
            //col = new DataColumn();
            //col.DataType = System.Type.GetType("System.Double");
            //col.ColumnName = "AvgWOS";
            //store_sell_thrus.Columns.Add(col);
            //col = new DataColumn();
            //col.DataType = System.Type.GetType("System.String");
            //col.ColumnName = "GroupKey";
            //store_sell_thrus.Columns.Add(col);
            ////END tt#153 - Velocity Matrix Variables - apicchetti
            // end TT#586 Velocity Variables not calculating correctly

			this.Component = AWFS.Component; // MID Track 3182 Velocity not recognizing specific Component
			Audit audit = SAB.ApplicationServerSession.Audit;
			// begin MID Track 4094 'color' not recognized by velocity
            // begin TT#421 Detail packs/bulk not allocated by Size Need Method.
            //AllocationProfile ap = (AllocationProfile) aAllocationProfile;
            AlocProfile = aAP as AllocationProfile;
            if (AlocProfile == null)
            {
                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
            }
            // end TT#421 Detail packs/bulk not allocated by Size Need Method.
			try
			{
				if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)AWFS._method.MethodType))
				{
					throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_WorkflowTypeInvalid), MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
				}
                // begin TT#421 Detail packs/bulk not allocated by size need
                //if (eProfileType.Allocation != aAP.ProfileType)
                //{
                //	throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_NotAllocationProfile), MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile));
                //}
                //AlocProfile = (AllocationProfile)aAP; // MID Track 4094 'color' not recognized by velocity
                // end TT#421 Detail packs/bulk not allocated by size need
                // BEGIN TT#673 - AGallagher - Velocity - Disable Balance option on WUB header 
                _bypassbal = false;
				//BEGIN TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                if (AlocProfile.HeaderType == eHeaderType.WorkupTotalBuy || AlocProfile.HeaderType == eHeaderType.Placeholder)
				//END TT#546-MD - stodd -  Change Allocation to treat a placeholder like a WorkUpBuy 
                    {
                        _bypassbal = true;    
                    }
                // END TT#673 - AGallagher - Velocity - Disable Balance option on WUB header  
                //if (((AllocationProfile)aAP).HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
                if (AlocProfile.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
				{
					string msg = string.Format(
						audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction,false), MIDText.GetTextOnly((int)AlocProfile.HeaderAllocationStatus));
					audit.Add_Msg(
						eMIDMessageLevel.Warning,eMIDTextCode.msg_HeaderStatusDisallowsAction,
						this.Name + " " + (AlocProfile.HeaderID + " " + msg),    // MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
						this.GetType().Name);
					AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.NoActionPerformed);
				}
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                else if (AlocProfile.IsMasterHeader && AlocProfile.DCFulfillmentProcessed)
                {
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), AlocProfile.HeaderID);
                    audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                    AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.NoActionPerformed);
                }
                else if (AlocProfile.IsSubordinateHeader && !AlocProfile.DCFulfillmentProcessed)
                {
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), AlocProfile.HeaderID);
                    audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                    AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.NoActionPerformed);
                }
                // End TT#1966-MD - JSmith- DC Fulfillment
				else
				{
					// begin MID Track 4442 Ad Min, Min and Max only valid on Total Component
					if (this.Component.ComponentType != eComponentType.Total)
					{
						string message2;
						bool totalComponentRule = false;
						foreach (GroupLvlMatrix glm in _groupLvlMtrxData.Values)
						{
							if (Enum.IsDefined(typeof(eRuleTypeOnlyValidOnTotalComponent),(eRuleTypeOnlyValidOnTotalComponent)glm.NoOnHandRuleType))
							{
								totalComponentRule = true;
								message2 = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal),AlocProfile.HeaderID, this.Name, MIDText.GetTextOnly((int)glm.NoOnHandRuleType));
                                audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal, message2, this.GetType().Name, false);
							}
							foreach(GroupLvlCell cell in glm.MatrixCells.Values)
							{
								if (Enum.IsDefined(typeof(eRuleTypeOnlyValidOnTotalComponent),(eRuleTypeOnlyValidOnTotalComponent)cell.CellRuleType))
								{
									totalComponentRule = true;
									message2 = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal),AlocProfile.HeaderID, this.Name, MIDText.GetTextOnly((int)cell.CellRuleType));
                                    audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_RuleNotValidWhenComponentNotTotal, message2, this.GetType().Name, false);
								}
							}
							if (totalComponentRule)
							{
								throw new MIDException(eErrorLevel.severe,
									(int)eMIDTextCode.msg_al_FoundInvalidRuleForNonTotalComponent,
									MIDText.GetTextOnly(eMIDTextCode.msg_al_FoundInvalidRuleForNonTotalComponent));
							}
						}
					}
					// end MID Track 4442 Ad Min, Min and Max only valid on Total Component
					// END MID Track 3011 Headers with status Rec'd OUT of Bal are being processed
                    //HierarchyNodeProfile styleHierarchyNodeProfile = AST.GetNodeData(AlocProfile.StyleHnRID);  // TT#1607 - JEllis - Inventory Basis wrong when Header has mult color
					bool colorBasisError = false;
					bool exactlyOneColor = false;
					switch (this.Component.ComponentType)
					{
						case (eComponentType.SpecificColor):
						{
							exactlyOneColor = true;
							break;
						}
						case (eComponentType.SpecificPack):
						{
							AllocationPackComponent apc = (AllocationPackComponent)this.Component;
							PackHdr ph = AlocProfile.GetPackHdr(apc.PackName);
							if (ph.PackColors.Count == 1)
							{
								exactlyOneColor = true;
							}
							break;
						}
						case (eComponentType.Total):
						{
							if (AlocProfile.StyleHnRID != AlocProfile.PlanLevelStartHnRID)
							{
								exactlyOneColor = true;
							}
							break;
						}
					}
					if (!exactlyOneColor)  // there is no color or more than one color on this header
					{
                        int colorLevelSeq = AST.GetColorLevelSequence();                                             // TT#1607 - JEllis - Inventory Basis wrong when Header has mult color
						DataTable dtBasis = AST.Velocity.DSVelocity.Tables["Basis"];
						int basisHnRID;
						int basisPHRID;
						int basisPHLSeq;
						foreach (DataRow dr in dtBasis.Rows)
						{
							basisHnRID = Convert.ToInt32(dr["BasisHNRID"],CultureInfo.CurrentUICulture);
							if (basisHnRID == Include.NoRID)
							{
								basisPHRID = Convert.ToInt32(dr["BasisPHRID"],CultureInfo.CurrentUICulture);
								if (basisPHRID != Include.NoRID)
								{
									basisPHLSeq = Convert.ToInt32(dr["BasisPHLSequence"],CultureInfo.CurrentUICulture);
                                    // begin TT#1607 - JEllis - Inventory Basis is wrong when mult color
                                    //if (styleHierarchyNodeProfile.NodeLevel < basisPHLSeq)
                                    //{
                                        //string msg = string.Format(
                                        //    audit.GetText(eMIDTextCode.msg_al_DynamicColorBasisInvalid,false), MIDText.GetTextOnly((int)eHierarchyLevelType.Color));
                                        //audit.Add_Msg(
                                        //    eMIDMessageLevel.Warning,eMIDTextCode.msg_al_DynamicColorBasisInvalid,
                                        //    (AlocProfile.HeaderID + " " + msg),  
                                        //    this.GetType().Name);
                                    if (basisPHLSeq == colorLevelSeq)
                                    {
                                        string msg = string.Format(
                                            audit.GetText(
                                            eMIDTextCode.msg_al_DynamicColorBasisInvalid, false),
                                            AlocProfile.HeaderID,
                                            MIDText.GetTextOnly(eMIDTextCode.lbl_Basis),
                                            MIDText.GetTextOnly((int)eMethodType.Velocity),
                                            MIDText.GetTextOnly((int)eHierarchyLevelType.Color));
                                        audit.Add_Msg(
                                            MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_DynamicColorBasisInvalid),
                                            eMIDTextCode.msg_al_DynamicColorBasisInvalid,
                                            msg,
                                            GetType().Name);
                                        // end TT#1607 - JEllis - Inventory Basis is wrong when multi color
										AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.VelocityBasisError);
										colorBasisError = true;
										break;
									}
								}
							}
						}
                        // begin TT#1607 - JEllis - Inventory Basis wrong when Header has mult color
                        if (_ApplyMinMaxInd == 'V'
                            && _InventoryInd == 'I'
                            && _MERCH_HN_RID == Include.NoRID
                            && _MERCH_PH_RID != Include.NoRID
                            && _MERCH_PHL_SEQ == colorLevelSeq)
                        {
                            string msg = string.Format(
                                audit.GetText(
                                eMIDTextCode.msg_al_DynamicColorBasisInvalid, false),
                                AlocProfile.HeaderID,
                                MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis),
                                MIDText.GetTextOnly((int)eMethodType.Velocity),
                                MIDText.GetTextOnly((int)eHierarchyLevelType.Color));
                            audit.Add_Msg(
                                MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_DynamicColorBasisInvalid),
                                eMIDTextCode.msg_al_DynamicColorBasisInvalid,
                                msg,
                                GetType().Name);
                            AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.VelocityBasisError);
                            colorBasisError = true;
                        }
                        // end TT#1607 - JEllis - Inventory Basis wrong when Header has mult color
					}
					if (!colorBasisError)
					{
						// end MID Track 4094 'color' not recognized in velocity
						if (!IsInteractive)
						{
							// ========================================
							// Reset tables to process multiple headers
							// ========================================
							_HdrCnt = 0;
							_headerData.Clear();

							foreach(GroupLvlMatrix glm in _groupLvlMtrxData.Values)
							{
								// begin TT#530 Velocity variables not calculated correctly
                                // removed initial code to common area
                                InitializeGroupLvlMatrix(glm);
                                // end TT#530 Velocity variables not calculated correctly

                                // begin TT#586 Velocity variables not calculated correctly
                                foreach (PctSellThruIndex psti in _pctSellThruData.Values)
                                {
                                    GroupSellThruTotalCell gsttc = glm.MatrixSellThruTotalCells[psti.Row];
                                    InitializeGroupSellThruTotalCell(gsttc);
                                }
                                // end TT#586 Velocity variables not calculated correctly

								_groupLvlGradData = glm.GradeSales;
								_groupLvlCellData = glm.MatrixCells;

								foreach(GradeLowLimit gll in _gradeLowLimData.Values)
								{
									GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[gll.Grade];

									// begin TT#530 Velocity variables not calculated correclty
                                    // removed initial code to common area
                                    InitializeGroupLvlGrade(glg);
                                    // end TT#530 Velocity variables not calculated correctly

									foreach(PctSellThruIndex psti in _pctSellThruData.Values)
									{
										glcKey = MatrixCellKey(gll.Row, psti.Row);
										GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[glcKey];
                                        // begin TT#530 Velocity variables not calculated correctly
                                        // removed initial code to common area
                                        InitializeGroupLvlCell(glc);
                                        // end TT#530 Velocity variables not calculated correctly

									}
								}
							}
						}

						//try
						//{
						//	if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)AWFS._method.MethodType))
						//	{
						//		throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_WorkflowTypeInvalid), MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
						//	}

						//if (eProfileType.Allocation != AlocProfile.ProfileType)
						//{
						//	throw new MIDException(eErrorLevel.severe, (int)(eMIDTextCode.msg_NotAllocationProfile), MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile));
						//}

						//AlocProfile = (AllocationProfile)aAP;  // MID Track 4094 
						if (AlocProfile.NeedAllocationPerformed || AlocProfile.StyleIntransitUpdated)
						{
							// begin MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
							//message = MIDText.GetText(eMIDTextCode.msg_GeneralHeaderIgnored);
							//message = message.Replace("General", "Velocity");
							//throw new MIDException(eErrorLevel.warning, (int)(eMIDTextCode.msg_GeneralHeaderIgnored), message);
						    message = 
								string.Format(
								MIDText.GetText(eMIDTextCode.msg_MethodIgnored),
								AlocProfile.HeaderID, 
								MIDText.GetTextOnly(eMIDTextCode.frm_VelocityMethod), 
								this.Name);
							throw new MIDException(
								eErrorLevel.warning, 
								(int)eMIDTextCode.msg_MethodIgnored, 
								message);
							// end MID Track 5778 - Scheduler "Run Now" feature gets Error in Audit
						}

						HeaderDataValue hdv = new HeaderDataValue();
						headerRID = AlocProfile.Key;
						hdv.HeaderRID = headerRID;
						hdv.AloctnProfile = (AllocationProfile)AlocProfile;
						hdv.NoOnHandStores = 0;

						_storeData = new Hashtable();
						_storeData.Clear();

						asp = AST.GetAllocationGrandTotalProfile();

						//========================================================================================
						// stodd 10.31.2007
						// The asp.VelocityStyleHnRID comes from header[0] in the ASP. During a normal velocity
						// we want it to use the current headers styleHnRID.
						//========================================================================================
						if (!IsInteractive)
						{
                            // begin TT#675 - MD - JEllis - Velocity Ship To Header should use Onhand Source when overridden, else Color when one color and Style When multiple colors
                            //if (AlocProfile.StyleHnRID != Include.NoRID)
                            //{
                            //    asp.VelocityStyleHnRID = AlocProfile.StyleHnRID;
                            //}
                            //else if (AlocProfile.OnHandHnRID != Include.NoRID)
                            //{
                            //    asp.VelocityStyleHnRID = AlocProfile.OnHandHnRID;
                            //}
                            asp.VelocityStyleHnRID = AlocProfile.VelocityHeaderOnhandHnRID;
                            // end TT#675 - MD - JEllis - Velocity Ship To Header should use Onhand Source when overridden, else Color when one color and Style When multiple colors
						}

						intransKeyType = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
						// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
						switch (Component.ComponentType)
						{
							case (eComponentType.Total):
							case (eComponentType.Bulk):
							{
								preSize_ColorSizeComponent = new AllocationColorSizeComponent(
									new GeneralComponent(eGeneralComponentType.AllColors),
									new GeneralComponent(eGeneralComponentType.AllSizes));
								break;
							}
							case (eComponentType.DetailType):
							{
								if (AlocProfile.BulkIsDetail)
								{
									preSize_ColorSizeComponent = new AllocationColorSizeComponent(
										new GeneralComponent(eGeneralComponentType.AllColors),
										new GeneralComponent(eGeneralComponentType.AllSizes));
								}
								else
								{
									preSize_ColorSizeComponent = null;
								}
								break;
							}
							case (eComponentType.SpecificColor):
							{
								preSize_ColorSizeComponent = new AllocationColorSizeComponent(
									Component,
									new GeneralComponent(eGeneralComponentType.AllSizes));
								break;
							}
							default:
							{
								preSize_ColorSizeComponent = null;
								break;
							}
						}
						// end MID Track 4282 Velocity overlays Fill Size Holes Allocation

						// ====================================
						// Get list of group levels with stores
						// ====================================
                        sgp = StoreMgmt.StoreGroup_GetFilled(_SG_RID); //SAB.StoreServerSession.GetStoreGroupFilled(_SG_RID);
						AST.CurrentStoreGroupProfile = sgp;

						// ===================
						// Retrieve store data
						// ===================
						// BEGIN MID Track 3182 Velocity not recognizing specific component
						GeneralComponent subtotalComponent;
						if (Component.ComponentType == eComponentType.SpecificPack)
						{
							PackHdr ph = AlocProfile.GetPackHdr(((AllocationPackComponent)_component).PackName);
                            subtotalComponent = new AllocationPackComponent(AlocProfile.GetSubtotalPackName(ph)); // TT#675 - MD - Jellis - Velocity Spread Average Null Reference
                            //subtotalComponent = new AllocationPackComponent(ph.SubtotalPackName);  // TT#675 - MD - Jellis - Velocity Spread Average Null Reference
						}
						else
						{
							subtotalComponent = Component;
						}
						// END MID Track 3182 Velocity not recognizing specific component

						foreach(StoreGroupLevelProfile sglp in sgp.GroupLevels)
						{
							grpLvlRID = sglp.Key;

							foreach(StoreProfile sp in sglp.Stores)
							{
								// ==========
								// Store Data
								// ==========
								StoreDataValue sdv = new StoreDataValue();

								storeRID = sp.Key;
								sdv.StoreRID = storeRID;
								sdv.GrpLvlRID = grpLvlRID;

								if (sp.Key == AST.GlobalOptions.ReserveStoreRID)
								{
									sdv.IsReserve = true;
								}
								else
								{
									sdv.IsReserve = false;
								}

								sdv.IsEligible = AlocProfile.GetStoreIsEligible(storeRID);
                                sdv.IsSimilarStoreModel = sp.SimilarStoreModel;  //TT#855-MD -jsobek -Velocity Enhancments

								sdv.Grade = asp.GetStoreGrade(storeRID);
								sdv.GradeIDX = asp.GetStoreGradeIdx(storeRID);

								sdv.ShipDay = AlocProfile.GetStoreShipDay(storeRID);

								sdv.Need = asp.GetStoreUnitNeed(storeRID);
								sdv.PctNeed = asp.GetStorePercentNeed(storeRID);

                                //sdv.Capacity = AlocProfile.GetStoreCapacityMaximum(storeRID); // TT#1074 - MD - Jellis- Group Allocation - Inventory Min Max Broken
                                sdv.Capacity = AlocProfile.GetStoreCapacityMaximum(Component, AlocProfile.StoreIndex(storeRID), true); // TT#1074 - MD - Jellis - Group Allocation MD - Jellis - Inventory Min Max broken
                            
								sdv.PrimaryMaximum = AlocProfile.GetStorePrimaryMaximum(Component, storeRID);

								sdv.QtyAllocated = asp.GetStoreQtyAllocated(subtotalComponent, storeRID); // MID Track 3182 Velocity not recognizing specific component

								// begin MID Track 4282 Velocity overlays Fill Size Holes allocation
								if (preSize_ColorSizeComponent == null)
								{
									sdv.PreSizeAllocated = 0;
								}
								else
								{
									sdv.PreSizeAllocated = 
										AlocProfile.GetStoreQtyAllocated(preSize_ColorSizeComponent, storeRID);
                                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                                    _TotalPreSizeAllocated = _TotalPreSizeAllocated + sdv.PreSizeAllocated;
                                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options  
								}
								// end MID Track 4282 Velocity overlays Fill Size Holes allocation


								sdv.PlanSales = asp.GetStoreSalesPlan(storeRID);

								sdv.PlanStock = asp.GetStoreStockPlan(storeRID);

								sdv.BasisSales = asp.GetStoreBasisSales(storeRID);

								sdv.BasisStock = asp.GetStoreBasisStock(storeRID);

								sdv.BasisOnHand = asp.GetStoreBasisOnHand(storeRID);
								if (sdv.BasisOnHand < 0.0)
								{
									sdv.BasisOnHand = 0.0;
								}

								sdv.BasisIntransit = asp.GetStoreBasisInTransit(storeRID);
								if (sdv.BasisIntransit < 0.0)
								{
									sdv.BasisIntransit = 0.0;
								}

								sdv.BasisOHandIT = sdv.BasisOnHand + sdv.BasisIntransit;
								if (sdv.BasisOHandIT < 0.0)
								{
									sdv.BasisOHandIT = 0.0;
								}

                                // BEGIN TT#1401 - AGallagher - VSW
                                if (AlocProfile.HeaderType != eHeaderType.IMO)
                                    {sdv.BasisVSWOnHand = asp.GetStoreBasisImoHistory(storeRID);}
                                    else
                                    {sdv.BasisVSWOnHand = 0.0;}
                                if (sdv.BasisVSWOnHand < 0.0)
                                {
                                    sdv.BasisVSWOnHand = 0.0;
                                }
                                sdv.BasisOHandIT = sdv.BasisOHandIT + sdv.BasisVSWOnHand;
                                // END TT#1401 - AGallagher - VSW

								sdv.StyleSales = asp.GetStoreBasisSales(storeRID);

								sdv.StyleStock = asp.GetStoreBasisStock(storeRID);

								sdv.StyleOnHand = asp.GetStoreStyleOnHand(intransKeyType, storeRID);
								if (sdv.StyleOnHand < 0.0)
								{
									sdv.StyleOnHand = 0.0;
								}

								sdv.StyleIntransit = asp.GetStoreStyleInTransit(intransKeyType, storeRID);
								if (sdv.StyleIntransit < 0.0)
								{
									sdv.StyleIntransit = 0.0;
								}

								sdv.StyleOHandIT = sdv.StyleOnHand + sdv.StyleIntransit;
								if (sdv.StyleOHandIT < 0.0)
								{
									sdv.StyleOHandIT = 0.0;
								}
                                // BEGIN TT#1401 - AGallagher - VSW
                                if (AlocProfile.HeaderType != eHeaderType.IMO)
                                //BEGIN TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                                {
                                    sdv.StyleVSWOnHand = asp.GetStoreImoHistory(AlocProfile, intransKeyType, storeRID, asp.VelocityStyleHnRID);
                                }
                                //END TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                                else
                                    { sdv.StyleVSWOnHand = 0.0; }
                                if (sdv.StyleVSWOnHand < 0.0)
                                {
                                    sdv.StyleVSWOnHand = 0.0;
                                }
                                sdv.StyleOHandIT = + sdv.StyleOHandIT + sdv.StyleVSWOnHand;
                                // END TT#1401 - AGallagher - VSW

                                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                                _IBInventoryInd = 'A';  
                                if (_ApplyMinMaxInd == 'S')
                                {
                                    HInventoryInd = AlocProfile.GradeInventoryMinimumMaximum;
                                    _IBMERCH_HN_RID = AlocProfile.GradeInventoryBasisHnRID;
                                    if (HInventoryInd == true)
                                    { 
                                        _IBInventoryInd = 'I'; 
                                    }
                                    else
                                    {
                                        _IBInventoryInd = 'A';
                                    }
                                }
                                if (_ApplyMinMaxInd == 'V')
                                {
                                    // BEGIN TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
                                    //_InventoryInd = _VMD.Inventory_Ind;
                                    //_MERCH_HN_RID = _VMD.MERCH_HN_RID;
                                    //_MERCH_PH_RID = _VMD.MERCH_PH_RID;
                                    //_MERCH_PHL_SEQ = _VMD.MERCH_PHL_SEQ;
                                    // END TT#1566 - AGallagher - Velocity inventory basis is sub class (same as ots forecast)  Not getting expected results
                                    _IBInventoryInd = _InventoryInd;
                                    _IBMERCH_HN_RID = _MERCH_HN_RID;
                                    _IBMERCH_PH_RID = _MERCH_PH_RID;
                                    _IBMERCH_PHL_SEQ = _MERCH_PHL_SEQ;
                                }
                                if (_IBInventoryInd == 'I' && _ApplyMinMaxInd == 'S')
                                {
                                    sdv.IBBasisOHandIT = AlocProfile.GetStoreInventoryBasis(_IBMERCH_HN_RID, storeRID, true); // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max broken
								    if (sdv.IBBasisOHandIT < 0.0)
								    {
									    sdv.IBBasisOHandIT = 0.0;
								    }
                                }
                                if (_IBInventoryInd == 'I' && _ApplyMinMaxInd == 'V')
                                {
                                    //sdv.IBBasisOHandIT = AlocProfile.GetStoreInventoryBasis(_IBMERCH_HN_RID, _IBMERCH_PH_RID, _IBMERCH_PHL_SEQ, storeRID); // TT#1607 - JEllis - Inventory Basis wrong when multiple colors
                                    sdv.IBBasisOHandIT = AlocProfile.GetStoreInventoryBasis(eMethodType.Velocity, _IBMERCH_HN_RID, _IBMERCH_PH_RID, _IBMERCH_PHL_SEQ, storeRID, true); // TT#1607 - JEllis - Inventory Basis wrong when multiple colors // TT#1074 - MD - Jellis - Group Allocation Inventory Min Max Broken
                                    if (sdv.IBBasisOHandIT < 0.0)
                                    {
                                        sdv.IBBasisOHandIT = 0.0;
                                    }
                                }
                                // END TT#1287 - AGallagher - Inventory Min/Max
                                
                                sdv.AvgWeeklySales = asp.GetStoreAvgWeeklySales(storeRID);

								sdv.AvgWeeklyStock = asp.GetStoreAvgWeeklyStock(storeRID);

                                sdv.AvgWeeklySupply = (sdv.BasisOnHand + sdv.BasisIntransit) / sdv.AvgWeeklySales;  // TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

								sdv.UserRule = false;
								sdv.ReCalcWillShip = false;
								sdv.RuleQty = Include.NoRuleQty;
                                sdv.RuleTypeQty = Include.NoRuleQty; // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                                // begin MID Track 4298 Integer types showing values with decimals
								//sdv.WillShip = Include.NoRuleQty;
								//sdv.Transfer = Include.NoRuleQty;
								sdv.WillShip = 0;
								sdv.Transfer = 0;
								// end MID Track 4298 Integer types showing values with decimals
								sdv.RuleType = eVelocityRuleType.None;

                                // ===========================
								// Store data for group matrix
								// ===========================
								sdv.SGLRule = false;

								saveCalculateAverageUsingChain = CalculateAverageUsingChain;

								// ===================================================
								// Store data for group matrix based on group averages
								// ===================================================
								CalculateAverageUsingChain = false;

								sdv.SGLGrpVelocityGrade = asp.GetStoreVelocityGrade(storeRID);
								sdv.SGLGrpVelocityGradeIDX = asp.GetStoreVelocityGradeIDX(storeRID);
								sdv.SGLGrpPctSellThruIndex = AST.GetStorePctSellThruIdx(storeRID);

								GradeLowLimit sglGrpGLL = (GradeLowLimit)_gradeLowLimData[sdv.SGLGrpVelocityGrade];

								// Begin TT #91 - stodd
								sdv.BasisGrade = sdv.SGLGrpVelocityGrade;
								sdv.BasisGradeIDX = sglGrpGLL.Row;

                                // begin TT#586 Velocity Variables not calculating correctly
                                ////tt153 velocity matrix variable - apicchetti
                                //DataRow store_sell_thrus_row_set = store_sell_thrus_set.NewRow();
                                ////tt153 velocity matrix variable - apicchetti
                                // end TT#586 Velocity Variables not calculating correctly

								//Debug.WriteLine(sdv.StoreRID + " " + sdv.Grade + " " + sdv.GradeIDX + " "
								//    + sdv.BasisGrade + " " + sdv.BasisGradeIDX);
								//// Begin track 6074 stodd
								//if (GradesByBasisInd)
								//{
								//    sdv.Grade = sdv.SGLGrpVelocityGrade;
								//    sdv.GradeIDX = sglGrpGLL.Row;
								//}
								//// End Track 6074
								// End TT #91
								foreach(PctSellThruIndex psti in _pctSellThruSortedData.Values)
								{
									if ((sdv.SGLGrpPctSellThruIndex >= (double)psti.SellThruIndex) ||
										(double)psti.SellThruIndex == 0)
									{
										sdv.SGLGrpCellKey = MatrixCellKey(sglGrpGLL.Row, psti.Row);
                                        sdv.SGLGrpPctSellThruRow = psti.Row; // TT#586 Velocity Variables not calculated correctly

                                        // begin TT#586 Velocity Variables not calculated correctly
                                        ////tt153 velocity matrix variable - apicchetti
                                        //store_sell_thrus_row_set["SellThruIdx"] = psti.SellThruIndex;
                                        ////tt153 velocity matrix variable - apicchetti
                                        // end TT#586 Velocity Variables not calculated correctly
										break;
									}
								}

								GroupLvlMatrix sglGrpGLM = (GroupLvlMatrix)_groupLvlMtrxData[sdv.GrpLvlRID];

								_groupLvlCellData = sglGrpGLM.MatrixCells;

								GroupLvlCell sglGrpGLC = (GroupLvlCell)_groupLvlCellData[sdv.SGLGrpCellKey];

								if (sdv.IsEligible)
								{
									if (!sdv.IsReserve)
									{
										if (_HdrCnt < 1)
										{
                                            // begin TT#586 Velocity variables not calculated correctly
                                            GroupSellThruTotalCell gsttc = sglGrpGLM.MatrixSellThruTotalCells[sdv.SGLGrpPctSellThruRow];
                                            // end TT#586 Velocity variables not calculated correctly

                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                            {
                                                _BasisSalesorStock = sdv.BasisSales;
                                                _BasisSalesorStockOHandIT = sdv.BasisOHandIT;
                                            }
                                            else
                                            {
                                                _BasisSalesorStock = sdv.BasisStock;
                                                if (DetermineShipQtyUsingBasis)
                                                { _BasisSalesorStockOHandIT = sdv.BasisOHandIT; }
                                                else
                                                { _BasisSalesorStockOHandIT = sdv.StyleOHandIT; }
                                            }
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2

											// (CSMITH) - BEG MID Track #2761: Str grades do not match
                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
											//if (sdv.BasisOHandIT <= 0.0)
                                            if (_BasisSalesorStockOHandIT <= 0.0)
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
											{
                                                // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
												//if (sdv.BasisSales <= 0.0)
                                                if (_BasisSalesorStock <= 0.0)
                                                // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
												{
													sglGrpGLM.NoOnHandBasisStores = (int)sglGrpGLM.NoOnHandBasisStores + 1;
												}
												else
												{
													sglGrpGLC.CellGrpStores = (int)sglGrpGLC.CellGrpStores + 1;
                                                    sglGrpGLM.EligibleBasisStores++;           // TT#530 Velocity variables not calculated correctly
                                                    sglGrpGLM.GrpBasisSales += (int)sdv.BasisSales; // TT#530 Velocity variables not calculated correctly
                                                    sglGrpGLM.GrpBasisStock += (int)sdv.BasisStock;      // TT#530 Velocity variabled not calculated correctly
                                                    gsttc.SellThruGrpStores++;  // TT#586 Velocity variables not calculated correctly
                                                }
											}
											else
											{
												sglGrpGLC.CellGrpStores = (int)sglGrpGLC.CellGrpStores + 1;
                                                sglGrpGLM.EligibleBasisStores++;           // TT#530 Velocity variables not calculated correctly
                                                sglGrpGLM.GrpBasisSales += (int)sdv.BasisSales; // TT#530 Velocity variables not calculated correctly
                                                sglGrpGLM.GrpBasisStock += (int)sdv.BasisStock;      // TT#530 Velocity variabled not calculated correctly
                                                gsttc.SellThruGrpStores++;  // TT#586 Velocity variables not calculated correctly
											}
											// (CSMITH) - END MID Track #2761

                                            
											if (sdv.StyleOHandIT <= 0.0)
											{
												sglGrpGLM.NoOnHandStyleStores = (int)sglGrpGLM.NoOnHandStyleStores + 1;
											}

											sglGrpGLC.CellGrpSales = (double)sglGrpGLC.CellGrpSales + (double)sdv.AvgWeeklySales;
											sglGrpGLC.CellGrpOnHand = (double)sglGrpGLC.CellGrpOnHand + (double)sdv.BasisOHandIT;

											sglGrpGLM.GroupSales = (double)sglGrpGLM.GroupSales + (double)sdv.AvgWeeklySales;
											sglGrpGLM.GroupOnHand = (double)sglGrpGLM.GroupOnHand + (double)sdv.BasisOHandIT;

                                            // begin TT#586 Velocity variables not calculated correctly
                                            gsttc.SellThruGrpSales += (double)sdv.AvgWeeklySales;
                                            gsttc.SellThruGrpOnHand += (double)sdv.BasisOHandIT;
                                            ////begin #153 - velocity matrix variables - apicchetti
                                            //if (sdv.BasisOHandIT > 0.0)
                                            //{
                                            //    storesAvgWeeklySalesSet = sdv.AvgWeeklySales;

                                            //    storeStoreBasisStockSet = sdv.BasisOHandIT;

                                            //    if (storesAvgWeeklySalesSet == 0)
                                            //    {
                                            //        store_sell_thrus_row_set["AvgWOS"] = 0;
                                            //    }
                                            //    else
                                            //    {
                                            //        store_sell_thrus_row_set["AvgWOS"] = (double)storeStoreBasisStockSet / (double)storesAvgWeeklySalesSet;
                                            //    }

                                            //    store_sell_thrus_row_set["GroupKey"] = sglp.Key;

                                            //    store_sell_thrus_set.Rows.Add(store_sell_thrus_row_set);

                                            //}
                                            ////end #153 - velocity matrix variables - apicchetti
                                            // end // TT#586 Velocity variables not calculated correctly


										}

										if (sdv.StyleOHandIT <= 0.0)
										{
											hdv.NoOnHandStores = (int)hdv.NoOnHandStores + 1;
										}
									}
								}

								// ===================================================
								// Store data for group matrix based on chain averages
								// ===================================================
								CalculateAverageUsingChain = true;

								sdv.SGLChnVelocityGrade = asp.GetStoreVelocityGrade(storeRID);
								sdv.SGLChnVelocityGradeIDX = asp.GetStoreVelocityGradeIDX(storeRID);
								sdv.SGLChnPctSellThruIndex = AST.GetStorePctSellThruIdx(storeRID);

								GradeLowLimit sglChnGLL = (GradeLowLimit)_gradeLowLimData[sdv.SGLChnVelocityGrade];

								foreach(PctSellThruIndex psti in _pctSellThruSortedData.Values)
								{
									if ((sdv.SGLChnPctSellThruIndex >= (double)psti.SellThruIndex) ||
										(double)psti.SellThruIndex == 0)
									{
										sdv.SGLChnCellKey = MatrixCellKey(sglChnGLL.Row, psti.Row);
                                        sdv.SGLChnPctSellThruRow = psti.Row;  // TT#586 Velocity variables not calculated correctly
										break;
									}
								}

								GroupLvlMatrix sglChnGLM = (GroupLvlMatrix)_groupLvlMtrxData[sdv.GrpLvlRID];

								_groupLvlCellData = sglChnGLM.MatrixCells;

								GroupLvlCell sglChnGLC = (GroupLvlCell)_groupLvlCellData[sdv.SGLChnCellKey];

								if (sdv.IsEligible)
								{
									if (!sdv.IsReserve)
									{
										if (_HdrCnt < 1)
										{
                                            // begin TT#586 Velocity variables not calculated correctly
                                            GroupSellThruTotalCell gsttc = sglChnGLM.MatrixSellThruTotalCells[sdv.SGLChnPctSellThruRow];  // TT#587 Velocity Store Counts wrong
                                            // end TT#586 Velocity variables not calculated correctly

                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                            {
                                                _BasisSalesorStock = sdv.BasisSales;
                                                _BasisSalesorStockOHandIT = sdv.BasisOHandIT;
                                            }
                                            else
                                            {
                                                _BasisSalesorStock = sdv.BasisStock;
                                                if (DetermineShipQtyUsingBasis)
                                                { _BasisSalesorStockOHandIT = sdv.BasisOHandIT; }
                                                else
                                                { _BasisSalesorStockOHandIT = sdv.StyleOHandIT; }
                                            }
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2

                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            //if (sdv.BasisOHandIT <= 0.0)
                                            if (_BasisSalesorStockOHandIT <= 0.0)
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
											{
                                                // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                //if (sdv.BasisSales <= 0.0)
                                                if (_BasisSalesorStock <= 0.0)
                                                // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
												{
												}
												else
												{
													sglChnGLC.CellChnStores = (int)sglChnGLC.CellChnStores + 1;
                                                    gsttc.SellThruChnStores++; // TT#586 Velocity Variables not calculated correctly
												}
											}
											else
											{
												sglChnGLC.CellChnStores = (int)sglChnGLC.CellChnStores + 1;
                                                gsttc.SellThruChnStores++; // TT#586 Velocity Variables not calculated correctly
											}
											// (CSMITH) - END MID Track #2761

											sglChnGLC.CellChnSales = (double)sglChnGLC.CellChnSales + (double)sdv.AvgWeeklySales;
											sglChnGLC.CellChnOnHand = (double)sglChnGLC.CellChnOnHand + (double)sdv.BasisOHandIT;
                                            // begin TT#586 Velocity Variables not calculated correctly
                                            gsttc.SellThruChnSales += (double)sdv.AvgWeeklySales;
                                            gsttc.SellThruChnOnHand += (double)sdv.BasisOHandIT;
                                            // end TT#586 Velocity Variables not Calculated correctly
										}
									}
								}

								CalculateAverageUsingChain = saveCalculateAverageUsingChain;

								// ===========================
								// Store data for total matrix
								// ===========================
								sdv.TotRule = false;

								saveCalculateAverageUsingChain = CalculateAverageUsingChain;

								// ===================================================
								// Store data for total matrix based on group averages
								// ===================================================
                                //CalculateAverageUsingChain = true;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
                                CalculateAverageUsingChain = false;  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
								sdv.TotGrpVelocityGrade = asp.GetStoreVelocityGrade(storeRID);
								sdv.TotGrpVelocityGradeIDX = asp.GetStoreVelocityGradeIDX(storeRID);
								sdv.TotGrpPctSellThruIndex = AST.GetStorePctSellThruIdx(storeRID);

								GradeLowLimit totGrpGLL = (GradeLowLimit)_gradeLowLimData[sdv.TotGrpVelocityGrade];

                                // begin TT#586 Velocity Variables not calculating correctly
                                ////tt153 velocity matrix variable - apicchetti
                                //DataRow store_sell_thrus_row = store_sell_thrus.NewRow();
                                ////tt153 velocity matrix variable - apicchetti
                                // end TT#586 Velocity Variables not calculating correctly

								foreach(PctSellThruIndex psti in _pctSellThruSortedData.Values)
								{
									if ((sdv.TotGrpPctSellThruIndex >= (double)psti.SellThruIndex) ||
										(double)psti.SellThruIndex == 0)
									{
										sdv.TotGrpCellKey = MatrixCellKey(totGrpGLL.Row, psti.Row);
                                        sdv.TotGrpPctSellThruRow = psti.Row; // TT#586 Velocity Variables not calculated correctly

                                        // begin TT#586 Velocity Variables not calculated correctly
                                        ////tt153 velocity matrix variable - apicchetti
                                        //store_sell_thrus_row["SellThruIdx"] = psti.SellThruIndex;
                                        ////tt153 velocity matrix variable - apicchetti
                                        // end TT#586 Velocity Variables not calculated correctly
										break;
									}
								}

								GroupLvlMatrix totGrpGLM = (GroupLvlMatrix)_groupLvlMtrxData[Include.TotalMatrixLevelRID];

								_groupLvlCellData = totGrpGLM.MatrixCells;

								GroupLvlCell totGrpGLC = (GroupLvlCell)_groupLvlCellData[sdv.TotGrpCellKey];

								if (sdv.IsEligible)
								{
									if (!sdv.IsReserve)
									{
										if (_HdrCnt < 1)
										{
                                            // begin TT#586 Velocity variables not calculated correctly
                                            GroupSellThruTotalCell gsttc = totGrpGLM.MatrixSellThruTotalCells[sdv.TotGrpPctSellThruRow];
                                            // end TT#586 Velocity variables not calculated correctly

                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                            {
                                                _BasisSalesorStock = sdv.BasisSales;
                                                _BasisSalesorStockOHandIT = sdv.BasisOHandIT;
                                            }
                                            else
                                            {
                                                _BasisSalesorStock = sdv.BasisStock;
                                                if (DetermineShipQtyUsingBasis)
                                                { _BasisSalesorStockOHandIT = sdv.BasisOHandIT; }
                                                else
                                                { _BasisSalesorStockOHandIT = sdv.StyleOHandIT; }
                                            }
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2

                                            // (CSMITH) - BEG MID Track #2761: Str grades do not match
											// BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            //if (sdv.BasisOHandIT <= 0.0)
                                            if (_BasisSalesorStockOHandIT <= 0.0)
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
											{
                                                // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                //if (sdv.BasisSales <= 0.0)
                                                if (_BasisSalesorStock <= 0.0)
                                                // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
												{
													totGrpGLM.NoOnHandBasisStores = (int)totGrpGLM.NoOnHandBasisStores + 1;
												}
												else
												{
													totGrpGLC.CellGrpStores = (int)totGrpGLC.CellGrpStores + 1;
                                                    totGrpGLM.EligibleBasisStores++;  // TT#530 Velocity Variables not calculated correctly
                                                    totGrpGLM.GrpBasisSales += (int)sdv.BasisSales; // TT#530 Velocity variables not calculated correctly
                                                    totGrpGLM.GrpBasisStock += (int)sdv.BasisStock;      // TT#530 Velocity variabled not calculated correctly
                                                    gsttc.SellThruGrpStores++;  // TT#586 Velocity variables not calculated correctly
  												}
											}
											else
											{
												totGrpGLC.CellGrpStores = (int)totGrpGLC.CellGrpStores + 1;
                                                totGrpGLM.EligibleBasisStores++;  // TT#530 Velocity Variables not calculated correctly
                                                totGrpGLM.GrpBasisSales += (int)sdv.BasisSales; // TT#530 Velocity variables not calculated correctly
                                                totGrpGLM.GrpBasisStock += (int)sdv.BasisStock;      // TT#530 Velocity variabled not calculated correctly
                                                gsttc.SellThruGrpStores++;  // TT#586 Velocity variables not calculated correctly
                                            }
											// (CSMITH) - END MID Track #2761

											if (sdv.StyleOHandIT <= 0.0)
											{
												totGrpGLM.NoOnHandStyleStores = (int)totGrpGLM.NoOnHandStyleStores + 1;
											}

											chainSales = (double)chainSales + (double)sdv.AvgWeeklySales;
											chainOnHand = (double)chainOnHand + (double)sdv.BasisOHandIT;
                                            // begin TT#533 velocity variables not calculated correctly
                                            chainBasisSales += (int)sdv.BasisSales;
                                            chainBasisStock += (int)sdv.BasisStock;
                                            // end TT#533 velocity variables not calculated correctly

											totGrpGLC.CellGrpSales = (double)totGrpGLC.CellGrpSales + (double)sdv.AvgWeeklySales;
											totGrpGLC.CellGrpOnHand = (double)totGrpGLC.CellGrpOnHand + (double)sdv.BasisOHandIT;

											totGrpGLM.GroupSales = (double)totGrpGLM.GroupSales + (double)sdv.AvgWeeklySales;
											totGrpGLM.GroupOnHand = (double)totGrpGLM.GroupOnHand + (double)sdv.BasisOHandIT;

                                            // begin TT#586 Velocity variables not calculated correctly
                                            gsttc.SellThruGrpSales += (double)sdv.AvgWeeklySales;
                                            gsttc.SellThruGrpOnHand += (double)sdv.BasisOHandIT;
                                            // end TT#586 Velocity variables not calculated correctly

                                            // begin TT#586 Velocity variables not calculated correctly
                                            ////begin #153 - velocity matrix variables - apicchetti
                                            //if (sdv.BasisOHandIT > 0.0)
                                            //{
                                            //    //storesAvgWeeklySalesSet = chainSales;

                                                //storeStoreBasisStockSet = chainOnHand;

                                            //    if (storesAvgWeeklySalesSet == 0)
                                            //    {
                                            //        store_sell_thrus_row["AvgWOS"] = 1;
                                            //    }
                                            //    else
                                            //    {
                                            //        store_sell_thrus_row["AvgWOS"] = (double)storeStoreBasisStockSet / (double)storesAvgWeeklySalesSet;
                                            //    }

                                            //    store_sell_thrus_row["GroupKey"] = sglp.Key;

                                            //    store_sell_thrus.Rows.Add(store_sell_thrus_row);

                                            //}
                                            ////end #153 - velocity matrix variables - apicchetti
                                            // end TT#586 Velocity variables not calculated correctly
                                        }
                                    }
                                }

								// ===================================================
								// Store data for total matrix based on chain averages
								// ===================================================
								CalculateAverageUsingChain = true;

								sdv.TotChnVelocityGrade = asp.GetStoreVelocityGrade(storeRID);
								sdv.TotChnVelocityGradeIDX = asp.GetStoreVelocityGradeIDX(storeRID);
								sdv.TotChnPctSellThruIndex = AST.GetStorePctSellThruIdx(storeRID);

								GradeLowLimit totChnGLL = (GradeLowLimit)_gradeLowLimData[sdv.TotChnVelocityGrade];

								foreach(PctSellThruIndex psti in _pctSellThruSortedData.Values)
								{
									if ((sdv.TotChnPctSellThruIndex >= (double)psti.SellThruIndex) ||
										(double)psti.SellThruIndex == 0)
									{
										sdv.TotChnCellKey = MatrixCellKey(totChnGLL.Row, psti.Row);
                                        sdv.TotChnPctSellThruRow = psti.Row;  // TT#586 Velocity Variables not calculated correctly
										break;
									}
								}

								GroupLvlMatrix totChnGLM = (GroupLvlMatrix)_groupLvlMtrxData[Include.TotalMatrixLevelRID];

								_groupLvlCellData = totChnGLM.MatrixCells;

								GroupLvlCell totChnGLC = (GroupLvlCell)_groupLvlCellData[sdv.TotChnCellKey];

								if (sdv.IsEligible)
								{
									if (!sdv.IsReserve)
									{
										if (_HdrCnt < 1)
										{
                                            // begin TT#586 Velocity variables not calculated correctly
                                            GroupSellThruTotalCell gsttc = totChnGLM.MatrixSellThruTotalCells[sdv.TotChnPctSellThruRow];
                                            // end TT#586 Velocity variables not calculated correctly

                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                            {
                                                _BasisSalesorStock = sdv.BasisSales;
                                                _BasisSalesorStockOHandIT = sdv.BasisOHandIT;
                                            }
                                            else
                                            {
                                                _BasisSalesorStock = sdv.BasisStock;
                                                if (DetermineShipQtyUsingBasis)
                                                { _BasisSalesorStockOHandIT = sdv.BasisOHandIT; }
                                                else
                                                { _BasisSalesorStockOHandIT = sdv.StyleOHandIT; }
                                            }
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2

                                            // (CSMITH) - BEG MID Track #2761: Str grades do not match
                                            // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                            //if (sdv.BasisOHandIT <= 0.0)
                                            if (_BasisSalesorStockOHandIT <= 0.0)
                                            // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
											{
                                                // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                //if (sdv.BasisSales <= 0.0)
                                                if (_BasisSalesorStock <= 0.0)
                                                // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
												{
												}
												else
												{
													totChnGLC.CellChnStores = (int)totChnGLC.CellChnStores + 1;
                                                    gsttc.SellThruChnStores++;  // TT#586 Velocity Variables not calculated correctly
												}
											}
											else
											{
												totChnGLC.CellChnStores = (int)totChnGLC.CellChnStores + 1;
                                                gsttc.SellThruChnStores++;  // TT#586 Velocity Variables not calculated correctly
                                            }
											// (CSMITH) - END MID Track #2761

											totChnGLC.CellChnSales = (double)totChnGLC.CellChnSales + (double)sdv.AvgWeeklySales;
											totChnGLC.CellChnOnHand = (double)totChnGLC.CellChnOnHand + (double)sdv.BasisOHandIT;
                                            // begin TT#586 Velocity Variables not calculated correctly
                                            gsttc.SellThruChnSales += (double)sdv.AvgWeeklySales;
                                            gsttc.SellThruChnOnHand += (double)sdv.BasisOHandIT;
                                            // end TT#586 Velocity Variables not calculated correctly
										}
									}
								}

								CalculateAverageUsingChain = saveCalculateAverageUsingChain;


                                //Begin TT#855-MD -jsobek -Velocity Enhancments
                                //sdv.VelocityGradeMinimum = AlocProfile.GetGradeMinimum(sdv.GradeIDX); 
                                //sdv.VelocityGradeMaximum = AlocProfile.GetGradeMaximum(sdv.GradeIDX);
                                int velocityGradeMinimum; 
                                int velocityGradeAdMinimum;
                                int velocityGradeMaximum;
                                
                                switch (_ApplyMinMaxInd)
                                {
                                    //case 'N':
                                    //    break;
                                    case 'S':
                                        velocityGradeMinimum = AlocProfile.GetGradeMinimum(sdv.GradeIDX);
                                        velocityGradeMaximum = AlocProfile.GetGradeMaximum(sdv.GradeIDX);
                                        velocityGradeAdMinimum = 0;

                                        break;
                                    default: //case 'V':
                                        if (AST.VelocityCalculateAverageUsingChain)
                                        {
                                            //Use Chain Total
                                            velocityGradeMinimum = totChnGLL.AllocMin;
                                            velocityGradeAdMinimum = totChnGLL.AllocAdMin;
                                            velocityGradeMaximum = totChnGLL.AllocMax;
                                        }
                                        else
                                        {
                                            //Use Group Total
                                            velocityGradeMinimum = totGrpGLL.AllocMin;
                                            velocityGradeAdMinimum = totGrpGLL.AllocAdMin;
                                            velocityGradeMaximum = totGrpGLL.AllocMax;
                                        }
                                        break;
                                }
                                if (_IBInventoryInd == 'I') //Inventory
                                {
                                    velocityGradeMinimum = velocityGradeMinimum - (int)sdv.IBBasisOHandIT;
                                    velocityGradeMaximum = velocityGradeMaximum - (int)sdv.IBBasisOHandIT;

                                    if (velocityGradeMinimum < 0) velocityGradeMinimum = 0;
                                    if (velocityGradeMaximum < 0) velocityGradeMaximum = 0;
                                }
                                sdv.VelocityGradeMinimum = velocityGradeMinimum;
                                sdv.VelocityGradeAdMinimum = velocityGradeAdMinimum;
                                sdv.VelocityGradeMaximum = velocityGradeMaximum;
                                sdv.IsManuallyAllocated = AlocProfile.GetStoreItemIsManuallyAllocated(eAllocationSummaryNode.Total, storeRID);
                                //End TT#855-MD -jsobek -Velocity Enhancments

								// ===========================
								// Add store data to hashtable
								// ===========================
								_storeData.Add(sdv.StoreRID, sdv);
							}

							hdv.StoreValues = _storeData;
						}

						_headerData.Add(hdv.HeaderRID, hdv);
						_HdrCnt++;

						_storeData = hdv.StoreValues;

                        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                        foreach (GroupLvlMatrix glm in _groupLvlMtrxData.Values)
                        {
                            if (glm.ModeInd == 'A')
                            {
                                Hashtable groupHash = new Hashtable();
                                if (glm.SpreadInd == 'S')
                                { _spreadAct = true; }
                                else
                                { _spreadAct = false; }
                                switch (glm.AverageRule)
                                {
                                    case (eVelocityRuleRequiresQuantity.ShipUpToQty):
                                        _spreadDec = false; // TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                        foreach (StoreDataValue sdv in _storeData.Values)
                                        {
                                            // if (sdv.GrpLvlRID == glm.SglRID) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            if (sdv.GrpLvlRID == glm.SglRID && sdv.IsEligible) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            {
                                                sdv.SpreadRuleType = (eVelocityRuleType)glm.AverageRule;
                                                if (CalculateAverageUsingChain)
                                                {
                                                    glm.AvgChnBasisSales = asp.GetStoreListVelocityAvgBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null);
                                                    // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    glm.AvgChnBasisStock = asp.GetStoreListVelocityAvgBasisStock(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null);
                                                    //sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgChnBasisSales));
                                                    if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgChnBasisSales)); }
                                                    else
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklyStock / glm.AvgChnBasisStock)); }
                                                    // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                       { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                else
                                                {
                                                    glm.AvgGrpBasisSales = asp.GetStoreListVelocityAvgBasisSales(_SG_RID, glm.SglRID, null);
                                                    // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    glm.AvgGrpBasisStock = asp.GetStoreListVelocityAvgBasisStock(_SG_RID, glm.SglRID, null);
                                                    //sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgGrpBasisSales));
                                                    if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgGrpBasisSales)); }
                                                    else
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklyStock / glm.AvgGrpBasisStock)); }
                                                    // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                _SpreadRound = (int)sdv.SpreadRuleQty;
                                                sdv.SpreadRuleQty = _SpreadRound;
                                                sdv.SpreadRuleTypeQty = sdv.SpreadRuleQty;
                                                // BEGIN TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                                //AddToStoreSortList(ref groupHash, sdv);
                                                if (CalculateAverageUsingChain)
                                                {
                                                  AddToStoreSortListChain(ref groupHash, sdv);
                                                }
                                                    else
                                                {
                                                  AddToStoreSortListSet(ref groupHash, sdv); 
                                                }
                                                // END TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected

                                            }
                                        }
                                        break;
                                    case (eVelocityRuleRequiresQuantity.AbsoluteQuantity):
                                        _spreadDec = false; // TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                        foreach (StoreDataValue sdv in _storeData.Values)
                                        {
                                            // if (sdv.GrpLvlRID == glm.SglRID) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            if (sdv.GrpLvlRID == glm.SglRID && sdv.IsEligible) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            {
                                                sdv.SpreadRuleType = (eVelocityRuleType)glm.AverageRule;
                                                if (CalculateAverageUsingChain)
                                                {
                                                    glm.AvgChnBasisSales = asp.GetStoreListVelocityAvgBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null);
                                                    // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    glm.AvgChnBasisStock = asp.GetStoreListVelocityAvgBasisStock(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null);
                                                    //sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgChnBasisSales));
                                                    if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgChnBasisSales)); }
                                                    else
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklyStock / glm.AvgChnBasisStock)); }
                                                    // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                else
                                                {
                                                    glm.AvgGrpBasisSales = asp.GetStoreListVelocityAvgBasisSales(_SG_RID, glm.SglRID, null);
                                                    // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    glm.AvgGrpBasisStock = asp.GetStoreListVelocityAvgBasisStock(_SG_RID, glm.SglRID, null);                                                    
                                                    //sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgGrpBasisSales));
                                                    if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.BasisSales / glm.AvgGrpBasisSales)); }
                                                    else
                                                    { sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklyStock / glm.AvgGrpBasisStock)); }
                                                    // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                _SpreadRound = (int)sdv.SpreadRuleQty;
                                                sdv.SpreadRuleQty = _SpreadRound;
                                                sdv.SpreadRuleTypeQty = sdv.SpreadRuleQty;
                                                // BEGIN TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                                //AddToStoreSortList(ref groupHash, sdv);
                                                if (CalculateAverageUsingChain)
                                                {
                                                    AddToStoreSortListChain(ref groupHash, sdv);
                                                }
                                                else
                                                {
                                                    AddToStoreSortListSet(ref groupHash, sdv);
                                                }
                                                // END TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                            }
                                        }
                                        break;
                                    case (eVelocityRuleRequiresQuantity.WeeksOfSupply):
                                        _spreadDec = true; // TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                        foreach (StoreDataValue sdv in _storeData.Values)
                                        {
                                            // if (sdv.GrpLvlRID == glm.SglRID) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            if (sdv.GrpLvlRID == glm.SglRID && sdv.IsEligible) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            {
                                                sdv.SpreadRuleType = (eVelocityRuleType)glm.AverageRule;
                                                if (CalculateAverageUsingChain)
                                                {
                                                    glm.ChainSales = (double)chainSales;
                                                    glm.ChainOnHand = (double)chainOnHand;

                                                    if (glm.ChainSales > Include.NoRuleQty)
                                                    {
                                                        glm.ChainAvgWOS = (double)glm.ChainOnHand / (double)glm.ChainSales;
                                                    }
                                                    sdv.AvgWeeklySupply = (sdv.BasisOnHand + sdv.BasisIntransit) / sdv.AvgWeeklySales;
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsInfinity(sdv.AvgWeeklySupply) || double.IsNaN(sdv.AvgWeeklySupply))
                                                    { sdv.AvgWeeklySupply = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklySupply / glm.ChainAvgWOS));
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                else
                                                {
                                                    if (glm.GroupSales > Include.NoRuleQty)
                                                    {
                                                        glm.GroupAvgWOS = (double)glm.GroupOnHand / (double)glm.GroupSales;
                                                    }
                                                    sdv.AvgWeeklySupply = (sdv.BasisOnHand + sdv.BasisIntransit) / sdv.AvgWeeklySales;
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsInfinity(sdv.AvgWeeklySupply) || double.IsNaN(sdv.AvgWeeklySupply))
                                                    { sdv.AvgWeeklySupply = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklySupply / glm.GroupAvgWOS));
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                // BEGIN TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                // _SpreadRound = (int)sdv.SpreadRuleQty;
                                                // sdv.SpreadRuleQty = _SpreadRound;
                                                // sdv.SpreadRuleTypeQty = sdv.SpreadRuleQty;
                                                sdv.SpreadRuleTypeQty = Math.Round(sdv.SpreadRuleQty, 2);
                                                sdv.SpreadRuleQty = sdv.SpreadRuleTypeQty;
                                                // END TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                // BEGIN TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                                //AddToStoreSortList(ref groupHash, sdv);
                                                if (CalculateAverageUsingChain)
                                                {
                                                    AddToStoreSortListChain(ref groupHash, sdv);
                                                }
                                                else
                                                {
                                                    AddToStoreSortListSet(ref groupHash, sdv);
                                                }
                                                // END TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                            }
                                        }
                                        break;
                                    case (eVelocityRuleRequiresQuantity.ForwardWeeksOfSupply):
                                        _spreadDec = true; // TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                        foreach (StoreDataValue sdv in _storeData.Values)
                                        {
                                            // if (sdv.GrpLvlRID == glm.SglRID) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            if (sdv.GrpLvlRID == glm.SglRID && sdv.IsEligible) // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                            {
                                                sdv.SpreadRuleType = (eVelocityRuleType)glm.AverageRule;
                                                if (CalculateAverageUsingChain)
                                                {
                                                    glm.ChainSales = (double)chainSales;
                                                    glm.ChainOnHand = (double)chainOnHand;

                                                    if (glm.ChainSales > Include.NoRuleQty)
                                                    {
                                                        glm.ChainAvgWOS = (double)glm.ChainOnHand / (double)glm.ChainSales;
                                                    }
                                                    sdv.AvgWeeklySupply = (sdv.BasisOnHand + sdv.BasisIntransit) / sdv.AvgWeeklySales;
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsInfinity(sdv.AvgWeeklySupply) || double.IsNaN(sdv.AvgWeeklySupply))
                                                    { sdv.AvgWeeklySupply = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklySupply / glm.ChainAvgWOS));
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                else
                                                {
                                                    if (glm.GroupSales > Include.NoRuleQty)
                                                    {
                                                        glm.GroupAvgWOS = (double)glm.GroupOnHand / (double)glm.GroupSales;
                                                    }
                                                    sdv.AvgWeeklySupply = (sdv.BasisOnHand + sdv.BasisIntransit) / sdv.AvgWeeklySales;
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsInfinity(sdv.AvgWeeklySupply) || double.IsNaN(sdv.AvgWeeklySupply))
                                                    { sdv.AvgWeeklySupply = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    sdv.SpreadRuleQty = (glm.AverageQty * (sdv.AvgWeeklySupply / glm.GroupAvgWOS));
                                                    // BEGIN TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                    if (double.IsNaN(sdv.SpreadRuleQty) || double.IsInfinity(sdv.SpreadRuleQty))
                                                    { sdv.SpreadRuleQty = 0; }
                                                    // END TT#960 - AGallagher - Velocity - Spread Average (#7) -Processed Velocity interactively then went to change the basis from OTS Forecast level to a Style and receive a System Arugument Out of Range Error
                                                }
                                                // BEGIN TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                // _SpreadRound = (int)sdv.SpreadRuleQty;
                                                // sdv.SpreadRuleQty = _SpreadRound;
                                                // sdv.SpreadRuleTypeQty = sdv.SpreadRuleQty;
                                                sdv.SpreadRuleTypeQty = Math.Round(sdv.SpreadRuleQty, 2);
                                                sdv.SpreadRuleQty = sdv.SpreadRuleTypeQty;
                                                // END TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                // BEGIN TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                                //AddToStoreSortList(ref groupHash, sdv);
                                                if (CalculateAverageUsingChain)
                                                {
                                                    AddToStoreSortListChain(ref groupHash, sdv);
                                                }
                                                else
                                                {
                                                    AddToStoreSortListSet(ref groupHash, sdv);
                                                }
                                                // END TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
                                            }
                                        }
                                        break;
                                }
                                if (groupHash.Count > 0)
                                {
                                    GroupLvlCell glc;
                                    foreach (int sglRID in groupHash.Keys)
                                    {
                                        SortedList sl = (SortedList)groupHash[sglRID];
                                        {
                                            foreach (string cellKey in sl.Keys)
                                            {
                                                double cellRuleQtyTotal = 0;
                                                double storeTotal = 0;   // TT#3114 - AGallagher - Velocity Average Mode Smooth WOS or FWOS the Reserve store is used in the Average of the 'g' grade stores.  Would not expect it to be included.
                                                Hashtable ht = (Hashtable)sl[cellKey];
                                                foreach (StoreDataValue sdv in ht.Values)
                                                {
                                                    // BEGIN TT#3114 - AGallagher - Velocity Average Mode Smooth WOS or FWOS the Reserve store is used in the Average of the 'g' grade stores.  Would not expect it to be included.
                                                    //cellRuleQtyTotal = cellRuleQtyTotal + sdv.SpreadRuleQty;
                                                    if (sdv.StoreRID != SAB.ApplicationServerSession.GlobalOptions.ReserveStoreRID)  
                                                    {cellRuleQtyTotal = cellRuleQtyTotal + sdv.SpreadRuleQty;
                                                    storeTotal = storeTotal + 1;}
                                                    // END TT#3114 - AGallagher - Velocity Average Mode Smooth WOS or FWOS the Reserve store is used in the Average of the 'g' grade stores.  Would not expect it to be included.
                                                }
                                                // BEGIN TT#3114 - AGallagher - Velocity Average Mode Smooth WOS or FWOS the Reserve store is used in the Average of the 'g' grade stores.  Would not expect it to be included.
                                                //double avg = cellRuleQtyTotal / ht.Count;
                                                double avg = cellRuleQtyTotal / storeTotal;
                                                // END TT#3114 - AGallagher - Velocity Average Mode Smooth WOS or FWOS the Reserve store is used in the Average of the 'g' grade stores.  Would not expect it to be included.
                                                // BEGIN TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                //_SpreadRound = (int)avg;
                                                //glc = (GroupLvlCell)glm.MatrixCells[cellKey];
                                                //glc.CellRuleType = (eVelocityRuleType)glm.AverageRule;
                                                //glc.CellRuleTypeQty = _SpreadRound;
                                                //glc.CellRuleQty = _SpreadRound;
                                                if (_spreadDec == false)
                                                {
                                                    _SpreadRound = (int)avg;
                                                    glc = (GroupLvlCell)glm.MatrixCells[cellKey];
                                                    glc.CellRuleType = (eVelocityRuleType)glm.AverageRule;
                                                    glc.CellRuleTypeQty = _SpreadRound;
                                                    glc.CellRuleQty = _SpreadRound;
                                                }
                                                else
                                                {
                                                    glc = (GroupLvlCell)glm.MatrixCells[cellKey];
                                                    glc.CellRuleType = (eVelocityRuleType)glm.AverageRule;
                                                    glc.CellRuleTypeQty = Math.Round(avg, 2);
                                                    glc.CellRuleQty = glc.CellRuleTypeQty;
                                                }
                                                //if (_spreadAct == true)
                                                if (_spreadAct == true && _spreadDec == false)
                                                // END TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                {
                                                    foreach (StoreDataValue sdv in ht.Values)
                                                    {
                                                        sdv.SpreadRuleQty = _SpreadRound;
                                                        sdv.SpreadRuleTypeQty = _SpreadRound;
                                                    }
                                                }
                                                // BEGIN TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                                if (_spreadAct == true && _spreadDec == true)
                                                {
                                                    foreach (StoreDataValue sdv in ht.Values)
                                                    {
                                                        sdv.SpreadRuleQty = Math.Round(avg, 2);
                                                        sdv.SpreadRuleTypeQty = sdv.SpreadRuleQty;
                                                    }
                                                }
                                                // END TT#946 - AGallagher - Velocity - Matrix Mode = Avg, Rule = WOS or FWOS , would prefer to calcuate the Rule Type Qty with 2 decimal places rounded
                                            }
                                        }
                                    }
                                }
                            }             
                        }
                        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing  
                        
						foreach(StoreDataValue sdv in _storeData.Values)
						{
							// ===================================================
							// Set initial store rule and quantity for group level
							// ===================================================
                            //BEGIN TT#4245 - DOConnell - VSW header type over allocates in Velocity; outside of stores in the VSW location
                            if (_alocProfile.GetIncludeStoreInAllocation(sdv.StoreRID))
                            {
                                SetStoreVelocityMatrixRule(hdv.HeaderRID, sdv.StoreRID, sdv.GrpLvlRID);

                                // ====================================================
                                // Set initial store rule and quantity for total matrix
                                // ====================================================
                                SetStoreVelocityMatrixRule(hdv.HeaderRID, sdv.StoreRID, Include.TotalMatrixLevelRID);
                            }
                            //END TT#4245 - DOConnell - VSW header type over allocates in Velocity; outside of stores in the VSW location
						}

                        // BEGIN TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocates
                        foreach (StoreDataValue sdv in _storeData.Values)
                        {
                            sdv.InitialRuleType = sdv.RuleType;
                            sdv.InitialRuleQuantity = sdv.RuleQty;
                            sdv.InitialRuleTypeQty = sdv.RuleTypeQty;
                            sdv.InitialWillShip = sdv.WillShip;
                        }
                        // END TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocates

                        // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                        // BEGIN TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
                        //if (_balance == true && _bypassbal == false)
                        if (_reconcile == true)
                        // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                        {
						    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                            //ApplySoftRulesToStores(AST);
							ApplySoftRulesToStores(AST, hdv);
							// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                            foreach (StoreDataValue sdv in _storeData.Values)
                            {
                             switch (this.Component.ComponentType)
                            {
                                case (eComponentType.Total):
                                    sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.SpecificPack):
                                    sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.GenericType):
                                    sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.DetailType):
                                    sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.SpecificColor):
                                    sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.Bulk):
                                    sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                            }
                            }
                            AlocProfile.RemoveSoftChosenRule();
                        }

                        // BEGIN TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocates
                        //foreach (StoreDataValue sdv in _storeData.Values)
                        //{
                        //    sdv.InitialRuleType = sdv.RuleType;
                        //    sdv.InitialRuleQuantity = sdv.RuleQty;
                        //    sdv.InitialRuleTypeQty = sdv.RuleTypeQty;
                        //    sdv.InitialWillShip = sdv.WillShip;
                        //}
                        // END TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocates
                        // END TT#842 - AGallagher - Velocity - Velocity Balance - with Layers

                        //tt#152 - Velocity Balance - apicchetti
                        // BEGIN TT#673 - AGallagher - Velocity - Disable Balance option on WUB header 
                        //if (_balance == true)
                        if (_balanceToHeaderInd == '1' && _bypassbal == false)
                        {
                            PerformBalanceToHeader(hdv);
                        }
                        else
                        {
                            if (_balance == true && _bypassbal == false)
                            // END TT#673 - AGallagher - Velocity - Disable Balance option on WUB header 
                            {
                                PerformBalance(hdv);
                            }
                        }
                        // BEGIN TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
                        //else
                        //{
                        //    foreach (StoreDataValue sdv in _storeData.Values)
                        //    {
                        //        sdv.InitialRuleType = sdv.RuleType;
                        //        sdv.InitialRuleQuantity = sdv.RuleQty;
                        //        sdv.InitialRuleTypeQty = sdv.RuleTypeQty;
                        //        sdv.InitialWillShip = sdv.WillShip;
                        //    }
                        //}
                        // END TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
                        //tt#152 - Velocity Balance - apicchetti

                        // =========================
                        // Update each matrix values
                        // =========================
                        saveCalculateAverageUsingChain = CalculateAverageUsingChain; // TT#587 Velocity Store Counts Wrong (as well as Set Sales Index)

						foreach(GroupLvlMatrix glm in _groupLvlMtrxData.Values)
						{
							if (glm.GroupSales > Include.NoRuleQty)
							{
								glm.GroupAvgWOS = (double)glm.GroupOnHand / (double)glm.GroupSales;
							}

                            CalculateAverageUsingChain = false;   // TT#587 Velocity Store Counts WRong (as well as Set Sales Index)
                            glm.AvgGrpBasisSalesIndex = asp.GetStoreListVelocityAvgBasisIndex(_SG_RID, glm.SglRID, null);         // TT#533 velocity variables not calculated correctly
                            glm.AvgGrpBasisSalesPctTot = asp.GetStoreListVelocityBasisPctToTotal(_SG_RID, glm.SglRID, null);      // TT#533 velocity variables not calculated correctly
                            glm.GrpStockPercentOfTotal = asp.GetStoreListVelocityStockPercentOfTotal(_SG_RID, glm.SglRID, null);  // TT#533 velocity variables not calculated correctly
                            // begin TT#587 Velocity Variables not calculated correctly
                            glm.AvgGrpBasisSales = asp.GetStoreListVelocityAvgBasisSales(_SG_RID, glm.SglRID, null);
                            glm.AvgGrpBasisStock = asp.GetStoreListVelocityAvgBasisStock(_SG_RID, glm.SglRID, null);
                            // end TT#587 Velocity Variables not calculated correctly

                            if (glm.ChnBasisSales == 0)  // BEGIN TT#809 - AGallagher - Velocity - Velocity if allocating more than 1 header in the Velocity Matrix Total Sales Total = 0
                                { glm.ChnBasisSales = chainBasisSales; }                                                                  // TT#533 velocity variables not calculated correctly
                            if (glm.ChnBasisSales == 0)  // BEGIN TT#809 - AGallagher - Velocity - Velocity if allocating more than 1 header in the Velocity Matrix Total Sales Total = 0
                                { glm.ChnBasisStock = chainBasisStock; }                                                                  // TT#533 velocity variables not calculated correctly
                            
                            // begin TT#587 Velocity Store Counts Wrong (as well as Set Sales Index)
                            CalculateAverageUsingChain = true;
                            glm.AvgChnBasisSalesIndex = asp.GetStoreListVelocityAvgBasisIndex(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null); // TT#533 velocity variables not calculated correctly
                            glm.ChnStockPercentOfTotal = asp.GetStoreListVelocityStockPercentOfTotal(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null); // TT#533 velocity variables not calculated correctly
                            glm.AvgChnBasisSales = asp.GetStoreListVelocityAvgBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null);
                            glm.AvgChnBasisStock = asp.GetStoreListVelocityAvgBasisStock(Include.AllStoreFilterRID, Include.AllStoreFilterRID, null);
                            glm.AvgChnBasisSalesPctTot = asp.GetStoreListVelocityBasisPctToTotal(_SG_RID, glm.SglRID, null);
                            // end TT#587 Velocity Store Counts Wrong (as well as Set Sales Index)

                            glm.GroupPctSellThru = AST.GetStoreGrpPctSellThru(_SG_RID, glm.SglRID);

							glm.ChainSales = (double)chainSales;
							glm.ChainOnHand = (double)chainOnHand;

							if (glm.ChainSales > Include.NoRuleQty)
							{
								glm.ChainAvgWOS = (double)glm.ChainOnHand / (double)glm.ChainSales;
							}

							glm.ChainPctSellThru = AST.GetStoreGrpPctSellThru(Include.AllStoreFilterRID, Include.AllStoreFilterRID);

							_groupLvlGradData = glm.GradeSales;
							_groupLvlCellData = glm.MatrixCells;

							foreach(GradeLowLimit gll in _gradeLowLimData.Values)
							{
								GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[gll.Grade];

                                CalculateAverageUsingChain = false;   // TT#587 Velocity Store Counts WRong (as well as Set Sales Index)
                                glg.TotGrpBasisSales = asp.GetStoreListVelocityTtlBasisSales(_SG_RID, glm.SglRID, glg.Grade);
								glg.AvgGrpBasisSales = asp.GetStoreListVelocityAvgBasisSales(_SG_RID, glm.SglRID, glg.Grade);

                                glg.AvgGrpBasisSalesIndex = asp.GetStoreListVelocityAvgBasisIndex(_SG_RID, glm.SglRID, glg.Grade);
								glg.AvgGrpBasisSalesPctTot = asp.GetStoreListVelocityBasisPctToTotal(_SG_RID, glm.SglRID, glg.Grade);

                                // begin TT#587 Velocity Variables not calculated correctly
                                //glg.TotChnBasisSales = asp.GetStoreListVelocityTtlBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                //glg.AvgChnBasisSales = asp.GetStoreListVelocityAvgBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                glg.GroupNumberOfStores = asp.GetStoreListVelocityTotalNumberOfStores(_SG_RID, glm.SglRID, glg.Grade);
                                glg.GroupAverageStock = asp.GetStoreListVelocityAvgBasisStock(_SG_RID, glm.SglRID, glg.Grade);
                                glg.GroupStockPercentOfTotal = asp.GetStoreListVelocityStockPercentOfTotal(_SG_RID, glm.SglRID, glg.Grade);
                                glg.GroupAllocationPercentOfTotal = asp.GetStoreListVelocityAllocationPercentOfTotal(_SG_RID, glm.SglRID, glg.Grade);
                               
                                CalculateAverageUsingChain = true;
                                glg.AvgChnBasisSalesIndex = asp.GetStoreListVelocityAvgBasisIndex(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                glg.AvgChnBasisSalesPctTot = asp.GetStoreListVelocityBasisPctToTotal(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                glg.TotChnBasisSales = asp.GetStoreListVelocityTtlBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                glg.AvgChnBasisSales = asp.GetStoreListVelocityAvgBasisSales(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                //glg.ChainNumberOfStores = asp.GetStoreListVelocityTotalNumberOfStores(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);  // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
                                glg.ChainNumberOfStores = asp.GetStoreListVelocityTotalNumberOfStores(_SG_RID, glm.SglRID, glg.Grade); // TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
                                glg.ChainAverageStock = asp.GetStoreListVelocityAvgBasisStock(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                glg.ChainStockPercentOfTotal = asp.GetStoreListVelocityStockPercentOfTotal(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                glg.ChainAllocationPercentOfTotal = asp.GetStoreListVelocityAllocationPercentOfTotal(Include.AllStoreFilterRID, Include.AllStoreFilterRID, glg.Grade);
                                ////BEGIN TT#153 – add variables to velocity matrix - apicchetti
                                //glg.TotalNumberOfStores = asp.GetStoreListVelocityTotalNumberOfStores(_SG_RID, glm.SglRID, glg.Grade);
                                //glg.AverageStock = asp.GetStoreListVelocityAvgBasisStock(_SG_RID, glm.SglRID, glg.Grade);
                                //glg.StockPercentOfTotal = asp.GetStoreListVelocityStockPercentOfTotal(_SG_RID, glm.SglRID, glg.Grade);
                                //glg.AllocationPercentOfTotal = asp.GetStoreListVelocityAllocationPercentOfTotal(_SG_RID, glm.SglRID, glg.Grade);
                                ////END TT#153 – add variables to velocity matrix - apicchetti
                                // end TT#587 Velocity Variables not calculated correctly

								foreach(PctSellThruIndex psti in _pctSellThruData.Values)
								{
									glcKey = MatrixCellKey(gll.Row, psti.Row);

									GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[glcKey];

									if (glc.CellGrpSales > Include.NoRuleQty)
									{
										glc.CellGrpAvgWOS = (double)glc.CellGrpOnHand / (double)glc.CellGrpSales;
									}

									if (glc.CellChnSales > Include.NoRuleQty)
									{
										glc.CellChnAvgWOS = (double)glc.CellChnOnHand / (double)glc.CellChnSales;
									}
								}
							}
						}
                        CalculateAverageUsingChain = saveCalculateAverageUsingChain;   // TT#587 Velocity Store Counts WRong (as well as Set Sales Index)

						if(!IsInteractive)
						{
							ApplyRulesToStores(AST);
						}
						AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					}
				}

                // begin TT#586 Velocity variables not calculated correctly
                ////tt#153 - velocity matrix variables - apicchetti
                //AST.StoresSellThru.Add(store_sell_thrus_set);
                //AST.StoresSellThru.Add(store_sell_thrus);
                ////tt#153 - velocity matrix variables - apicchetti
                // end TT#586 Velocity variables not calculated correctly

                

			}
			catch
			{
				AST.SetAllocationActionStatus(AlocProfile.Key, eAllocationActionStatus.ActionFailed);

				throw;
			}

			finally
			{
                //foreach (StoreDataValue sdv in _storeData.Values)
                //{
                //    Debug.Print("Store: " + sdv.StoreRID + " Allocation: " + sdv.WillShip);
                //}
			}
		}

        // BEGIN TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
        // BEGIN TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
        //private void AddToStoreSortList(ref Hashtable aGroupHash, StoreDataValue aSDV)
        private void AddToStoreSortListSet(ref Hashtable aGroupHash, StoreDataValue aSDV)
        // END TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
        {
            try
            {
                SortedList cellKeySL;
                Hashtable storeData; 
                if (!aGroupHash.ContainsKey(aSDV.GrpLvlRID))
                {
                    cellKeySL = new SortedList();
                    storeData = new Hashtable();
                    storeData.Add(aSDV.StoreRID, aSDV);
                    cellKeySL.Add(aSDV.SGLGrpCellKey, storeData);
                    aGroupHash.Add(aSDV.GrpLvlRID, cellKeySL);
                }
                else
                {
                    cellKeySL = (SortedList)aGroupHash[aSDV.GrpLvlRID];
                    if (!cellKeySL.ContainsKey(aSDV.SGLGrpCellKey))
                    {
                        storeData = new Hashtable();
                        storeData.Add(aSDV.StoreRID, aSDV);
                        cellKeySL.Add(aSDV.SGLGrpCellKey, storeData);
                    }
                    else
                    {
                        storeData = (Hashtable)cellKeySL[aSDV.SGLGrpCellKey];
                        if (!storeData.ContainsKey(aSDV.StoreRID))
                        {
                             storeData.Add(aSDV.StoreRID, aSDV);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        // END TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 

        // BEGIN TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected
         private void AddToStoreSortListChain(ref Hashtable aGroupHash, StoreDataValue aSDV)
        {
            try
            {
                SortedList cellKeySL;
                Hashtable storeData; 
				 if (!aGroupHash.ContainsKey(aSDV.GrpLvlRID))
                {
                    cellKeySL = new SortedList();
                    storeData = new Hashtable();
                    storeData.Add(aSDV.StoreRID, aSDV);
                    cellKeySL.Add(aSDV.SGLChnCellKey, storeData);
                    aGroupHash.Add(aSDV.GrpLvlRID, cellKeySL);
                }
                else
                {
                    cellKeySL = (SortedList)aGroupHash[aSDV.GrpLvlRID];
                    if (!cellKeySL.ContainsKey(aSDV.SGLChnCellKey))
                    {
                        storeData = new Hashtable();
                        storeData.Add(aSDV.StoreRID, aSDV);
                        cellKeySL.Add(aSDV.SGLChnCellKey, storeData);
                    }
                    else
                    {
                        storeData = (Hashtable)cellKeySL[aSDV.SGLChnCellKey];
                        if (!storeData.ContainsKey(aSDV.StoreRID))
                        {
                             storeData.Add(aSDV.StoreRID, aSDV);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        // END TT#1081 - AGallagher - Velocity - Velocity Average Spread Index with different Attribute Sets does not populate the Matrix as expected


		// BEGIN Issue 4778 stodd 10.17.2007
		public bool IsBasisDynamic()
		{
			bool isBasisDynamic = false;
			DataTable dtBasis = DSVelocity.Tables["Basis"];
			int basisHnRID;
			foreach (DataRow dr in dtBasis.Rows)
			{
				basisHnRID = Convert.ToInt32(dr["BasisHNRID"],CultureInfo.CurrentUICulture);
				if (basisHnRID == Include.NoRID)
				{
					isBasisDynamic = true;
					break;
				}
			}
			return isBasisDynamic;
		}
		// END Issue 4778

		override public void Update(TransactionData td)
		{
 			if (_VMD == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_VMD = new VelocityMethodData(td, base.Key);
			}

			_VMD.StoreGroup_RID = _SG_RID;
			_VMD.OTS_Plan_HN_RID = _OTS_Plan_MdseHnRID;
			_VMD.OTS_Plan_PH_RID  = _OTS_Plan_ProdHnRID;
			_VMD.OTS_Begin_CDR_RID = _OTS_Begin_CDR_RID;
			_VMD.OTS_Ship_To_CDR_RID = _OTS_ShipTo_CDR_RID;
			_VMD.OTS_Plan_PHL_SEQ  = _OTS_Plan_ProdHnLvlSeq;
			_VMD.Sim_Store_Ind = Include.ConvertBoolToChar(_UseSimilarStoreHistory);
			_VMD.Avg_Using_Chain_Ind = Include.ConvertBoolToChar(_CalculateAverageUsingChain);
			_VMD.Ship_Using_Basis_Ind = Include.ConvertBoolToChar(_DetermineShipQtyUsingBasis);
			_VMD.Trend_Percent = Include.ConvertBoolToChar(_TrendPctContribution);
			_VMD.DSVelocity = _dsVelocity;
			// Begin Track #6074
			// Begin TT # 91 - stodd
			//_VMD.GradesByBasisInd = _gradesByBasisInd;
			// End TT # 91 - stodd
			// End Track #6074

            // Begin TT#313 - JSmith -  balance does not remain checked
            _VMD.Balance_Ind = Include.ConvertBoolToChar(_balance);
            // End TT#313

            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            _VMD.Apply_Min_Max_Ind = _ApplyMinMaxInd;
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)

            // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
            _VMD.Reconcile_Ind = Include.ConvertBoolToChar(_reconcile);
            // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            _VMD.Inventory_Ind = _InventoryInd;
            _VMD.MERCH_HN_RID = _MERCH_HN_RID;
            _VMD.MERCH_PH_RID = _MERCH_PH_RID;
            _VMD.MERCH_PHL_SEQ = _MERCH_PHL_SEQ; 
            // END TT#1287 - AGallagher - Inventory Min/Max

            _VMD.GradeVariableType =  _gradeVariableType; //TT#855-MD -jsobek -Velocity Enhancements
            _VMD.BalanceToHeaderInd = _balanceToHeaderInd; //TT#855-MD -jsobek -Velocity Enhancements

			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_VMD.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
                        // make sure the key in the data layer is the same
                        _VMD.Velocity_Method_RID = base.Key;
						_VMD.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_VMD.DeleteMethod(base.Key, td);
						base.Update(td);
						break;
				}
			}

			catch
			{
				throw;
			}

			finally
			{
			}
		}

		/// <summary>
		/// Applies the velocity rules to each store
		/// </summary>
		/// <param name="aAST">Application Session Transaction</param>
		/// <remarks>Apply the velocity rules to the stores</remarks>
		public void ApplyRulesToStores(ApplicationSessionTransaction aAST)
		{
			// BEG MID Change j.ellis Add Audit Message
			string infoMsg;

			ApplicationServerSession session;

			session = SAB.ApplicationServerSession;
			// END MID Change j.ellis Add Audit Message

			int RuleQty;
			int StoreRID;

			bool RuleValid;

			AllocationProfile ap = null;

			RuleAllocationProfile rap = null;

			eRuleType RuleType = eRuleType.None;

			foreach(HeaderDataValue hdv in _headerData.Values)
			{
				_storeData = hdv.StoreValues;

				ap = (AllocationProfile)hdv.AloctnProfile;

                // Begin TT#2091-MD - JSmith - PH qty doubles after canceling the header allocation twice.  Would not expect the PH qty to double.
                if (ap.AsrtRID != Include.NoRID)
                {
                    ap.ActivateAssortment = true;
                }
                // End TT#2091-MD - JSmith - PH qty doubles after canceling the header allocation twice.  Would not expect the PH qty to double.

				// BEG MID Change j.ellis Add Audit Message
				if (_globalUserTypeText == null)
				{
					if (this.GlobalUserType == eGlobalUserType.Global)
					{
						_globalUserTypeText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationMethodsGlobalVelocity);
					}
					else
					{
						_globalUserTypeText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationMethodsUserVelocity);
					}
				}

				infoMsg = string.Format(session.Audit.GetText(eMIDTextCode.msg_al_BeginProcessRule, false),	_globalUserTypeText, this.Name, ap.HeaderID);

				session.Audit.Add_Msg(eMIDMessageLevel.Information,	eMIDTextCode.msg_al_BeginProcessRule, infoMsg, this.GetType().Name);
				// END MID Change j.ellis Add Audit Message

				rap = new RuleAllocationProfile(Key, aAST, ap.HeaderRID, Component);

				foreach(StoreDataValue sdv in _storeData.Values)
				{
					RuleValid = true;

					StoreRID = sdv.StoreRID;

					RuleQty = sdv.WillShip;  // MID Track 4298 Integer types showing decimal values

                   	switch (sdv.RuleType)
					{
						case eVelocityRuleType.Out:
							RuleType = eRuleType.Exclude;
							break;
						case eVelocityRuleType.None:
							RuleType = eRuleType.None;
							break;
						case eVelocityRuleType.ShipUpToQty:
							RuleType = eRuleType.ShipUpToQty;
							break;
						case eVelocityRuleType.WeeksOfSupply:
							RuleType = eRuleType.WeeksOfSupply;
							break;
						case eVelocityRuleType.AbsoluteQuantity:
							RuleType = eRuleType.AbsoluteQuantity;
							break;
						case eVelocityRuleType.ForwardWeeksOfSupply:
							RuleType = eRuleType.ForwardWeeksOfSupply;
							break;
						case eVelocityRuleType.Minimum:
							RuleType = eRuleType.Minimum;
							break;
						case eVelocityRuleType.Maximum:
							RuleType = eRuleType.Maximum;
							break;
						case eVelocityRuleType.AdMinimum:
							RuleType = eRuleType.AdMinimum;
							break;
                        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        //case eVelocityRuleType.MinimumBasis:
                        //    RuleType = eRuleType.MinimumBasis;
                        //    break;
                        //case eVelocityRuleType.MaximumBasis:
                        //    RuleType = eRuleType.MaximumBasis;
                        //    break;
                        //case eVelocityRuleType.AdMinimumBasis:
                        //    RuleType = eRuleType.AdMinimumBasis;
                        //    break;
                        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
						case eVelocityRuleType.ColorMinimum:
							RuleType = eRuleType.ColorMinimum;
							break;
						case eVelocityRuleType.ColorMaximum:
							RuleType = eRuleType.ColorMaximum;
							break;
						default:
							RuleValid = false;
							break;
					}

					if (RuleValid)
					{
                        rap.SetStoreRuleAllocation(StoreRID, RuleType, RuleQty);
					}
				}

                if (!rap.WriteRuleAllocation())
                {
                	aAST.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
				}
				else
				{
                    bool overridereconcillation = false;
                    int auditQtyAllocatedByProcess = ap.GetQtyAllocated(Component);
                    // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                    if (_balance == true && _bypassbal == false)
                        { overridereconcillation = true; }
                    //if (!ap.DetermineChosenRule(Component, rap.LayerID))
                    if (!ap.DetermineChosenRule(Component, rap.LayerID, overridereconcillation))
                    // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                   	{
						aAST.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
					}
					else
					{
						aAST.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
						// begin MID Track 4448 AnF Audit Enhancement
						string packOrColorComponentName = null;
						switch (this.Component.ComponentType)
						{
							case (eComponentType.Total):
							case (eComponentType.GenericType):
							case (eComponentType.DetailType):
							case (eComponentType.Bulk):
							{
								break;
							}
							case (eComponentType.SpecificColor):
							{
								packOrColorComponentName = aAST.GetColorCodeProfile(((AllocationColorOrSizeComponent)Component).ColorRID).ColorCodeName;
								break;
							}
							case (eComponentType.SpecificPack):
							{
								packOrColorComponentName = ((AllocationPackComponent)Component).PackName;
								break;
							}
							default:
							{
								break;
							}
						}
						aAST.WriteAllocationAuditInfo
							(ap.Key,
							0,
							this.MethodType,
							this.Key,
							this.Name,
							this.Component.ComponentType,
							packOrColorComponentName,
							null,                 // MID Track 4448 AnF Audit Enhancement
							ap.GetQtyAllocated(Component) - auditQtyAllocatedByProcess, // MID Track 4448 AnF Audit Report Enhancement
							ap.GetCountStoresWithRuleLayer(Component, rap.LayerID)); // MID Track 4448 AnF Audit Report Enhancement
						// end MID Track 4448 AnF Audit Enhancement	
					}
                }

                // Begin TT#2091-MD - JSmith - PH qty doubles after canceling the header allocation twice.  Would not expect the PH qty to double.
                ap.ActivateAssortment = false;
                // End TT#2091-MD - JSmith - PH qty doubles after canceling the header allocation twice.  Would not expect the PH qty to double.

				// BEG MID Change j.ellis Add Audit Message
				infoMsg = string.Format(session.Audit.GetText(eMIDTextCode.msg_al_EndProcessRule, false), _globalUserTypeText, this.Name, ap.HeaderID);

				session.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_al_BeginProcessRule, infoMsg, this.GetType().Name);
				// END MID Change j.ellis Add Audit Message
			}
		}

		/// <summary>
		/// Applies the velocity soft rules to each store
		/// </summary>
		/// <param name="aAST">Application Session Transaction</param>
		/// <remarks>Apply the velocity soft rules to the stores</remarks>
		// BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
		//public bool ApplySoftRulesToStores(ApplicationSessionTransaction aAST)
		public bool ApplySoftRulesToStores(ApplicationSessionTransaction aAST, HeaderDataValue aHdv) 
		// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
		{
			// BEG MID Change j.ellis Add Audit Message
			string infoMsg;

			ApplicationServerSession session;

			session = SAB.ApplicationServerSession;
			// END MID Change j.ellis Add Audit Message

			int RuleQty;
			int StoreRID;

			bool RuleValid;
			bool applyOkay = true;

			AllocationProfile ap = null;

			RuleAllocationProfile rap = null;

			eRuleType RuleType = eRuleType.None;

            // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
			//foreach(HeaderDataValue hdv in _headerData.Values)
			//{
			HeaderDataValue hdv = aHdv;   
            // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
				_storeData = hdv.StoreValues;

				ap = (AllocationProfile)hdv.AloctnProfile;

				// BEG MID Change j.ellis Add Audit Message
				if (_globalUserTypeText == null)
				{
					if (this.GlobalUserType == eGlobalUserType.Global)
					{
						_globalUserTypeText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationMethodsGlobalVelocity);
					}
					else
					{
						_globalUserTypeText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationMethodsUserVelocity);
					}
				}

				infoMsg = string.Format(session.Audit.GetText(eMIDTextCode.msg_al_BeginInteractiveRule, false), _globalUserTypeText, this.Name, ap.HeaderID);

				session.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_al_BeginInteractiveRule, infoMsg, this.GetType().Name);
				// END MID Change j.ellis Add Audit Message

				rap = new RuleAllocationProfile(Key, aAST, ap.HeaderRID, Component);

				foreach(StoreDataValue sdv in _storeData.Values)
				{
					RuleValid = true;

					StoreRID = sdv.StoreRID;

					RuleQty = sdv.WillShip; // MID Track 4298 Integer type showing decimal values

					switch (sdv.RuleType)
					{
						case eVelocityRuleType.Out:
							RuleType = eRuleType.Exclude;
							break;
						case eVelocityRuleType.None:
							RuleType = eRuleType.None;
							break;
						case eVelocityRuleType.ShipUpToQty:
							RuleType = eRuleType.ShipUpToQty;
							break;
						case eVelocityRuleType.WeeksOfSupply:
							RuleType = eRuleType.WeeksOfSupply;
							break;
						case eVelocityRuleType.AbsoluteQuantity:
							RuleType = eRuleType.AbsoluteQuantity;
							break;
						case eVelocityRuleType.ForwardWeeksOfSupply:
							RuleType = eRuleType.ForwardWeeksOfSupply;
							break;
						case eVelocityRuleType.Minimum:
							RuleType = eRuleType.Minimum;
							break;
						case eVelocityRuleType.Maximum:
							RuleType = eRuleType.Maximum;
							break;
						case eVelocityRuleType.AdMinimum:
							RuleType = eRuleType.AdMinimum;
							break;
                        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        //case eVelocityRuleType.MinimumBasis:
                        //    RuleType = eRuleType.MinimumBasis;
                        //    break;
                        //case eVelocityRuleType.MaximumBasis:
                        //    RuleType = eRuleType.MaximumBasis;
                        //    break;
                        //case eVelocityRuleType.AdMinimumBasis:
                        //    RuleType = eRuleType.AdMinimumBasis;
                        //    break;
                        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
						case eVelocityRuleType.ColorMinimum:
							RuleType = eRuleType.ColorMinimum;
							break;
						case eVelocityRuleType.ColorMaximum:
							RuleType = eRuleType.ColorMaximum;
							break;
						default:
							RuleValid = false;
							break;
					}

					if (RuleValid)
					{
						rap.SetStoreRuleAllocation(StoreRID, RuleType, RuleQty);
					}
				}

				if (!ap.DetermineSoftChosenRule(rap))
				{
					applyOkay = false;
				}

				// BEG MID Change j.ellis Add Audit Message
				infoMsg = string.Format(session.Audit.GetText(eMIDTextCode.msg_al_EndInteractiveRule, false), _globalUserTypeText, this.Name, ap.HeaderID);

				session.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_al_EndInteractiveRule, infoMsg, this.GetType().Name);
				// END MID Change j.ellis Add Audit Message
			//}  // TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results

			return applyOkay;
		}

		/// <summary>
		/// Saves the latest velocity soft rules
		/// </summary>
		/// <remarks>Saves the latest velocity soft rules to the database</remarks>
		public void SaveSoftRules()
		{

			AllocationProfile ap = null;

			foreach(HeaderDataValue hdv in _headerData.Values)
			{
				ap = (AllocationProfile)hdv.AloctnProfile;

				ap.SaveSoftChosenRule();
			}
		}

		/// <summary>
		/// Removes the latest velocity soft rules
		/// </summary>
		/// <remarks>Removes the latest velocity soft rules from the stores</remarks>
		public void RemoveSoftRules()
		{

			AllocationProfile ap = null;

			foreach(HeaderDataValue hdv in _headerData.Values)
			{
				ap = (AllocationProfile)hdv.AloctnProfile;

				ap.RemoveSoftChosenRule();
			}
		}

		/// <summary>
		/// Resets store velocity rule data
		/// </summary>
		/// <remarks>Resets store velocity rule data</remarks>
		public void RefreshStoreData()
		{
			foreach(HeaderDataValue hdv in _headerData.Values)
			{
				_storeData = hdv.StoreValues;

				foreach(StoreDataValue sdv in _storeData.Values)
				{
					sdv.SGLRule = false;
					sdv.TotRule = false;
					sdv.UserRule = false;

					sdv.ReCalcWillShip = false;

					sdv.RuleQty = Include.NoRuleQty;
                    sdv.RuleTypeQty = Include.NoRuleQty;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) 
					// begin MID Track 4298 Integer Types showing decimal values
					//sdv.WillShip = Include.NoRuleQty;
					//sdv.Transfer = Include.NoRuleQty;
					sdv.WillShip = 0;
					sdv.Transfer = 0;
					// end MID Track 4298 Integer Types showing decimal values
					sdv.RuleType = eVelocityRuleType.None;

					SetStoreVelocityMatrixRule(hdv.HeaderRID, sdv.StoreRID, sdv.GrpLvlRID);

					SetStoreVelocityMatrixRule(hdv.HeaderRID, sdv.StoreRID, Include.TotalMatrixLevelRID);
                // BEGIN TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
                }  
                }
            AllocationProfile ap; // TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
            foreach (HeaderDataValue hdv in _headerData.Values)
                {
                    ap = hdv.AloctnProfile; // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                    _storeData = hdv.StoreValues;
                    foreach (StoreDataValue sdv in _storeData.Values)
                        {
                            sdv.InitialRuleType = sdv.RuleType;
                            sdv.InitialRuleQuantity = sdv.RuleQty;
                            sdv.InitialRuleTypeQty = sdv.RuleTypeQty;
                            sdv.InitialWillShip = sdv.WillShip;
                        }
                    // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                    //if (_balance == true && _bypassbal == false)
                    if (_reconcile == true)
                    // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                    {
					    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                        //ApplySoftRulesToStores(AST);
						ApplySoftRulesToStores(AST, hdv); 
						// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                        foreach (StoreDataValue sdv in _storeData.Values)
                        {
                            switch (this.Component.ComponentType)
                            {
                                case (eComponentType.Total):
                                    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.SpecificPack):
                                    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.GenericType):
                                    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.DetailType):
                                    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID)
                                    sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.SpecificColor):
                                    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.Bulk):
                                    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    //sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleQty = sdv.RuleTypeQty;
                                    //sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                            }
                        }
                        // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                        //AlocProfile.RemoveSoftChosenRule();
                        ap.RemoveSoftChosenRule(); 
                        // END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                    }
                    // END TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
                                        
                    //tt#152 - Velocity Balance - apicchetti
                    // BEGIN TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
                    //if (_balance == true)
                    if (_balanceToHeaderInd == '1' && _bypassbal == false)
                    {
                        PerformBalanceToHeader(hdv);
                    }
                    else
                    {
                        if (_balance == true && _bypassbal == false)
                        // END TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
                        {
                            PerformBalance(hdv);
                        }
                    }
                    //tt#152 - Velocity Balance - apicchetti
                    //} // TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
            }
        }

        // begin TT#533 velocity variables not calculated correctly
        public bool IncludeStoreOnMatrix(int aStoreRID)
        {
            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];
            if (sdv.IsEligible
                && (sdv.BasisSales > 0
                    || sdv.BasisOHandIT > 0))
            {
                return true;
            }
            return false;
        }
        // end TT#533 velocity variables not calculated correctly

		/// <summary>
		/// Gets the Velocity Grade Code for a given index value
		/// </summary>
		/// <param name="aStoreVelocityGradeIDX">Velocity Grade Index Value</param>
		/// <returns>Velocity Grade Code (grade for low limit of '0' returned when no match found)</returns>
		public string VelocityGrade(double aStoreVelocityGradeIDX)
		{
			int defEntry = _lowLimSortedGradeData.Count - 1;

			foreach(GradeLowLimit gll in _lowLimSortedGradeData.Values)
			{
				if (aStoreVelocityGradeIDX >= (double)gll.LowerLimit)
				{
					return gll.Grade;
				}
			}

			GradeLowLimit GLL = (GradeLowLimit)_lowLimSortedGradeData.GetByIndex(defEntry);

			return GLL.Grade;
		}

		/// <summary>
		/// Gets the Matrix Cell Key based on the Grade row and Sell Thru Index row
		/// </summary>
		/// <param name="aRow">Grade Row</param>
		/// <param name="aCol">Sell Thru Index Row</param>
		/// <returns>String combining the Grade row and Sell Thru Index row separated by a comma.</returns>
		public string MatrixCellKey(int aRow, int aCol)
		{
			return aRow.ToString().Trim() + "," + aCol.ToString().Trim();
		}

		/// <summary>
		/// Gets a header no onhand store count
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <returns>Header No OnHand Store Count</returns>
		/// <remarks>Retrieves a header no onhand store count</remarks>
		public int GetHeaderNoOnHandStores(int aHdrRID)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			return hdv.NoOnHandStores;
		}

		/// <summary>
		/// Gets a matrix no onhand store count
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix No OnHand Store Count</returns>
		/// <remarks>Retrieves a matrix no onhand store count</remarks>
		public int GetMatrixNoOnHandStores(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.NoOnHandBasisStores;
		}

		/// <summary>
		/// Gets a matrix group sales
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Group Sales</returns>
		/// <remarks>Retrieves a matrix group sales</remarks>
		public double GetMatrixGroupSales(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.GroupSales;
		}

		/// <summary>
		/// Gets a matrix group onhand
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Group OnHand</returns>
		/// <remarks>Retrieves a matrix group onhand</remarks>
		public double GetMatrixGroupOnHand(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.GroupOnHand;
		}

		/// <summary>
		/// Gets a matrix group average weeks of supply
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Group Average Weeks of Supply</returns>
		/// <remarks>Retrieves a matrix group weeks of supply</remarks>
		public double GetMatrixGroupAvgWOS(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.GroupAvgWOS;
		}

		/// <summary>
		/// Gets a matrix group percent sell thru
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Group Percent Sell Thru</returns>
		/// <remarks>Retrieves a matrix group percent sell thru</remarks>
		public double GetMatrixGroupPctSellThru(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.GroupPctSellThru;
		}

		/// <summary>
		/// Gets a matrix chain sales
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Chain Sales</returns>
		/// <remarks>Retrieves a matrix chain sales</remarks>
		public double GetMatrixChainSales(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.ChainSales;
		}

		/// <summary>
		/// Gets a matrix chain onhand
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Chain OnHand</returns>
		/// <remarks>Retrieves a matrix chain onhand</remarks>
		public double GetMatrixChainOnHand(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.ChainOnHand;
		}

		/// <summary>
		/// Gets a matrix chain average weeks of supply
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Chain Average Weeks of Supply</returns>
		/// <remarks>Retrieves a matrix chain weeks of supply</remarks>
		public double GetMatrixChainAvgWOS(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.ChainAvgWOS;
		}

		/// <summary>
		/// Gets a matrix chain percent sell thru
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <returns>Matrix Chain Percent Sell Thru</returns>
		/// <remarks>Retrieves a matrix chain percent sell thru</remarks>
		public double GetMatrixChainPctSellThru(int aGrpLvlRID)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			return glm.ChainPctSellThru;
		}

		/// <summary>
		/// Gets a matrix grade total basis sales
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aGrade">Velocity Grade</param>
		/// <returns>Matrix Grade Total Basis Sales</returns>
		/// <remarks>Retrieves a matrix grade total basis sales</remarks>
		public int GetMatrixGradeTotBasisSales(int aGrpLvlRID, string aGrade) 
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            // begin TT#533 velocity variables not calculated correctly
            if (aGrade == null)
            {
                // begin TT#587 Matrix Total Incorrect
                if (CalculateAverageUsingChain)
                {
                    return (int)glm.ChnBasisSales;
                }
                // end TT#587 Matrix Total Incorrect
                return (int)glm.GrpBasisSales;
            }
            // end TT#533 velocity variables not calculated correctly


			_groupLvlGradData = glm.GradeSales;

			GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Matrix Total Incorrect
            if (CalculateAverageUsingChain)
            {
                return glg.TotChnBasisSales;
            }
            // end TT#587 Matrix Total Incorrect
            return glg.TotGrpBasisSales;
		}

		/// <summary>
		/// Gets a matrix grade average basis sales
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aGrade">VelocityGrade</param>
		/// <returns>Matrix Grade Average Basis Sales</returns>
		/// <remarks>Retrieves a matrix grade average basis sales</remarks>
		public double GetMatrixGradeAvgBasisSales(int aGrpLvlRID, string aGrade)  
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            
            // begin TT#533 Velocity variables not calculated correctly
            if (aGrade == null)
            {
                // begin TT#587 Matrix Total Incorrect
                if (CalculateAverageUsingChain)
                {
                    return glm.AvgChnBasisSales;
                }
                //return glm.MatrixAvgGrpBasisSales; 
                return glm.AvgGrpBasisSales;
                // end TT#587 Matrix Total Incorrect
            }
            // end TT#533 Velocity variables not calculated correctly

			_groupLvlGradData = glm.GradeSales;

			GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Matrix Total Incorrect
            if (CalculateAverageUsingChain)
            {
                return glg.AvgChnBasisSales;
            }
            // end TT#587 Matrix Total Incorrect
            return glg.AvgGrpBasisSales;
		}

		/// <summary>
		/// Gets a matrix grade average basis sales index
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aGrade">Velocity Grade </param>
		/// <returns>Matrix Grade Average Basis Sales Index</returns>
		/// <remarks>Retrieves a matrix grade average basis sales index</remarks>
        public double GetMatrixGradeAvgBasisSalesIdx(int aGrpLvlRID, string aGrade)  
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            // begin TT#533 velocity variables calculated incorrectly
            if (aGrade == null)
            {
                // begin TT#587 Matrix Total Incorrect
                if (CalculateAverageUsingChain)
                {
                    return glm.AvgChnBasisSalesIndex;
                }
                // end TT#587 Matrix Total Incorrect
                return glm.AvgGrpBasisSalesIndex;
            }
            // end TT#533 velocity variables calculated incorrectly

			_groupLvlGradData = glm.GradeSales;

			GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Matrix Total Incorrect
            if (CalculateAverageUsingChain)
            {
                return glg.AvgChnBasisSalesIndex;
            }
            // end TT#587 Matrix Total Incorrect

            return glg.AvgGrpBasisSalesIndex;
		}

        //BEGIN TT#153 – add variables to velocity matrix - apicchetti
        /// <summary>
        /// Gets a matrix grade total number of stores
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aGrade">Velocity Grade </param>
        /// <returns>Matrix Grade Total Number of Stores</returns>
        /// <remarks>Retrieves a matrix total number of stores</remarks>
        public double GetMatrixGradeTotalNumberOfStores(int aGrpLvlRID, string aGrade)
        {
            GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            // begin TT#533 velocity variables not calculated correctly
            if (aGrade == null)
            {
                // BEGIN TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
                if (CalculateAverageUsingChain)
                {
                    //return glm.;
                }
                // END TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
                //return glm.MatrixBasisStores;  // TT#587 Velocity variables calculated incorrectly
                return glm.EligibleBasisStores;  // TT#587 Velocity variables calcualted incorrectly
            }
            // end TT#533 velocity variables not calculated correctly

            _groupLvlGradData = glm.GradeSales;

            GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Velocity Variables not calculated correctly
            // BEGIN TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
            if (CalculateAverageUsingChain)
            {
                return glg.ChainNumberOfStores;
            }
            return glg.GroupNumberOfStores;
            // END TT#686 - AGallagher - Velocity - WUB Header w/Velocity - not honoring rule - totals still not correct
            //return glg.TotalNumberOfStores;
            // end TT#587 Velocity Variables not calculated correctly
        }

        /// <summary>
        /// Gets a matrix grade average stock
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aGrade">Velocity Grade </param>
        /// <returns>Matrix Grade Average Stock</returns>
        /// <remarks>Retrieves a matrix total number of stores</remarks>
        public double GetMatrixGradeAvgStock(int aGrpLvlRID, string aGrade)
        {
            GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            // begin TT#533 velocity variables not calculated correctly
            if (aGrade == null)
            {
                // begin TT#587 Velocity Variables not calculated correctly
                if (CalculateAverageUsingChain)
                {
                    return glm.AvgChnBasisStock;
                }
                // end TT#587 Velocity Variables not calculated correctly
                return glm.AvgGrpBasisStock;  
            }
            // end TT#533 velocity variables not calculated correctly

            _groupLvlGradData = glm.GradeSales;

            GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Velocity Variables not calculated correctly
            if (CalculateAverageUsingChain)
            {
                return glg.ChainAverageStock;
            }
            return glg.GroupAverageStock;
            //return glg.AverageStock;
            // end TT#587 Velocity not calculated correcly
        }

        /// <summary>
        /// Gets a matrix grade stock percentage of total
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aGrade">Velocity Grade </param>
        /// <returns>Matrix Grade Average Stock</returns>
        /// <remarks>Retrieves a matrix stock percentage of total</remarks>
        public double GetMatrixGradeStockPercentageOfTotal(int aGrpLvlRID, string aGrade)
        {
            GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            // begin TT#533 velocity variables not calculated correctly
            if (aGrade == null)
            {
                // begin TT#587 Velocity Variables not calculated correctly
                if (CalculateAverageUsingChain)
                {
                    return glm.ChnStockPercentOfTotal;
                }
                // end TT#587 Velocity Variables not calculated correctly
                return glm.GrpStockPercentOfTotal; 
            }
            // end TT#533 velocity variables not calculated correctly

            _groupLvlGradData = glm.GradeSales;

            GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Velocity Variables calculated incorrectly
            if (CalculateAverageUsingChain)
            {
                return glg.ChainStockPercentOfTotal;
            }
            return glg.GroupStockPercentOfTotal;
            //return glg.StockPercentOfTotal;
            // end TT#587 Velocity Variables calculated incorrectly
        }

        /// <summary>
        /// Gets a matrix grade allocation percentage of total
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aGrade">Velocity Grade </param>
        /// <returns>Matrix Grade Allocation Percentage of Totral</returns>
        /// <remarks>Retrieves a matrix Allocation Percentage of Totral</remarks>
        public double GetMatrixGradeAllocationPercentageOfTotal(int aGrpLvlRID, string aGrade)
        {
            GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

            _groupLvlGradData = glm.GradeSales;

            GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Velocity Variables not calculated correctly
            if (CalculateAverageUsingChain)
            {
                return glg.ChainAllocationPercentOfTotal;
            }
            return glg.GroupAllocationPercentOfTotal;
            //return glg.AllocationPercentOfTotal;
            // end TT#587 Velocity Variables not calculated correctly
        }
        //END TT#153 – add variables to velocity matrix - apicchetti


        /// <summary>
		/// Gets a matrix grade average basis sales percent to total
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aGrade">VelocityGrade</param>
		/// <returns>Matrix Grade Average Basis Sales Percent to Total</returns>
		/// <remarks>Retrieves a matrix grade average basis sales percent to total</remarks>
        public double GetMatrixGradeAvgBasisSalesPctTot(int aGrpLvlRID, string aGrade) 
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];
            // begin TT#533 velocity variables not calculated correctly
            if (aGrade == null)
            {
                // begin TT#587 Matrix Total Incorrect
                if (CalculateAverageUsingChain)
                {
                    return glm.AvgChnBasisSalesPctTot;
                }
                // end TT#587 Matrix Total incorrect
                return glm.AvgGrpBasisSalesPctTot;  
            }
            // end TT#533 velocity variables not calculated correctly
			_groupLvlGradData = glm.GradeSales;

			GroupLvlGrade glg = (GroupLvlGrade)_groupLvlGradData[aGrade];

            // begin TT#587 Matrix Total Incorrect
            if (CalculateAverageUsingChain)
            {
                return glg.AvgChnBasisSalesPctTot;
            }
            // end TT#587 Matrix Total incorrect
            return glg.AvgGrpBasisSalesPctTot;
		}

		/// <summary>
		/// Gets a matrix cell store count
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aBoundary">Grade Boundary</param>
		/// <param name="aSellThruIndex">Sell Thru Index</param>
		/// <returns>Matrix Cell Store Count</returns>
		/// <remarks>Retrieves a matrix cell store count</remarks>
        public int GetMatrixCellStores(int aGrpLvlRID, int aBoundary, int aSellThruIndex)  
        {
			string glcKey = null;

			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			_groupLvlCellData = glm.MatrixCells;

            GradeLowLimit gll = (GradeLowLimit)_lowLimGradeData[aBoundary];  
            PctSellThruIndex psti = (PctSellThruIndex)_pctSellThruData[aSellThruIndex];

			glcKey = MatrixCellKey(gll.Row, psti.Row);

			GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[glcKey];

            // begin TT#586 Store counts differ between Matrix and Detail
            if (CalculateAverageUsingChain)
            {
                return glc.CellChnStores;
            }
            // end TT#586 Store Counts differ between Matrix and Detail
            return glc.CellGrpStores;
		}

		/// <summary>
		/// Gets a matrix cell average weeks of supply
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aBoundary">Grade Boundary</param>
		/// <param name="aSellThruIndex">Sell Thru Index</param>
		/// <returns>Matrix Cell Average Weeks of Supply</returns>
		/// <remarks>Retrieves a matrix cell average weeks of supply</remarks>
        // begin TT#586 STore Counts differe between detail and matrix
		//public double GetMatrixCellAvgWOS(int aGrpLvlRID, int aBoundary, int aSellThruIndex, bool aDisplay)  // TT#586 Velocity variables not calculated correctly
        public double GetMatrixCellAvgWOS(int aGrpLvlRID, int aBoundary, int aSellThruIndex)   
        
            // end TT#586 Store counts differ between detial and matrix
		{
			string glcKey = null;

			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			_groupLvlCellData = glm.MatrixCells;

            GradeLowLimit gll = (GradeLowLimit)_lowLimGradeData[aBoundary];  
                        
			PctSellThruIndex psti = (PctSellThruIndex)_pctSellThruData[aSellThruIndex];

			glcKey = MatrixCellKey(gll.Row, psti.Row);

			GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[glcKey];

            // begin TT3586 Store counts differ between detail and matrix
            //if (!CalculateAverageUsingChain       // TT#586 Velocity variables not calculated correctly
            //    || aDisplay)                      // TT#586 Velocity variables not calculated correctly
            if (!CalculateAverageUsingChain)
                // end TT#586 Store counts differ between detail and matrix
			{
				return glc.CellGrpAvgWOS;
			}
			else
			{
				return glc.CellChnAvgWOS;
			}
		}

        // begin TT#586 Velocity variables not calculated correctly
        /// <summary>
        /// Gets a sell thru total store count
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aSellThruIndex">Sell Thru Index</param>
        /// <returns>Sell Thru Store Count</returns>
        public int GetSellThruTotalStores(int aGrpLvlRID, int aSellThruIndex)
        {
            GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];
            PctSellThruIndex psti = (PctSellThruIndex)_pctSellThruData[aSellThruIndex];
            // begin TT#587 Velocity Store Counts Incorrect
            if (CalculateAverageUsingChain)
            {
                return glm.MatrixSellThruTotalCells[psti.Row].SellThruChnStores;
            }
            // end TT#587 Velocity Store Counts Incorrec
            return glm.MatrixSellThruTotalCells[psti.Row].SellThruGrpStores;
        }

        /// <summary>
        /// Gets a sell thru average weeks of supply for given group level
        /// </summary>
        /// <param name="aGrpLvlRID">Group Level RID</param>
        /// <param name="aSellThruIndex">Sell Thru Index</param>
        /// <returns>Sell Thru Average Weeks of Supply</returns>
        public double GetSellThruAvgWOS(int aGrpLvlRID, int aSellThruIndex)
        {
            GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];
            PctSellThruIndex psti = (PctSellThruIndex)_pctSellThruData[aSellThruIndex];

            // begin TT#587 Matrix Total Incorrect
            if (CalculateAverageUsingChain)
            {
                return glm.MatrixSellThruTotalCells[psti.Row].SellThruChnAvgWOS;
            }
            // end TT#587 Matrix Total Incorrec
            return glm.MatrixSellThruTotalCells[psti.Row].SellThruGrpAvgWOS;        
        }
        // end TT#586 Velocity variables not calculated correctly

		/// <summary>
		/// Sets a matrix no onhand rule type
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aRuleType">No OnHand Rule Type</param>
		/// <remarks>Updates a matrix no onhand rule type</remarks>
		public void SetMatrixNoOnHandRuleType(int aGrpLvlRID, eVelocityRuleType aRuleType)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			glm.NoOnHandRuleType = aRuleType;
		}

		/// <summary>
		/// Sets a matrix no onhand rule quantity
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aRuleQty">No OnHand Rule Quantity</param>
		/// <remarks>Updates a matrix no onhand rule quantity</remarks>
		public void SetMatrixNoOnHandRuleQty(int aGrpLvlRID, double aRuleQty)
		{
			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			glm.NoOnHandRuleQty = aRuleQty;
		}

		/// <summary>
		/// Sets a matrix cell rule type
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aBoundary">Grade Boundary</param>
		/// <param name="aSellThruIndex">Sell Thru Index</param>
		/// <param name="aRuleType">Cell Rule Type</param>
		/// <remarks>Updates a matrix cell rule type</remarks>
		public void SetMatrixCellType(int aGrpLvlRID, int aBoundary, int aSellThruIndex, eVelocityRuleType aRuleType)
		{
			string glcKey = null;

			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			_groupLvlCellData = glm.MatrixCells;

			GradeLowLimit gll = (GradeLowLimit)_lowLimGradeData[aBoundary];

			PctSellThruIndex psti = (PctSellThruIndex)_pctSellThruData[aSellThruIndex];

			glcKey = MatrixCellKey(gll.Row, psti.Row);

			GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[glcKey];

			glc.CellRuleType = aRuleType;
		}

		/// <summary>
		/// Sets a matrix cell rule quantity
		/// </summary>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <param name="aBoundary">Grade Boundary</param>
		/// <param name="aSellThruIndex">Sell Thru Index</param>
		/// <param name="aRuleQty">Cell Rule Quantity</param>
		/// <remarks>Updates a matrix cell rule quantity</remarks>
		public void SetMatrixCellQty(int aGrpLvlRID, int aBoundary, int aSellThruIndex, double aRuleQty)
		{
			string glcKey = null;

			GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

			_groupLvlCellData = glm.MatrixCells;

			GradeLowLimit gll = (GradeLowLimit)_lowLimGradeData[aBoundary];

			PctSellThruIndex psti = (PctSellThruIndex)_pctSellThruData[aSellThruIndex];

			glcKey = MatrixCellKey(gll.Row, psti.Row);

			GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[glcKey];

			glc.CellRuleQty = aRuleQty;
		}

		/// <summary>
		/// Updates stores with matrix changes
		/// </summary>
		/// <remarks>Update stores when matrix changes are made</remarks>
		public void ApplyMatrixChanges()
		{
            foreach (GroupLvlMatrix glm in _groupLvlMtrxData.Values)
            {
                foreach (HeaderDataValue hdv in _headerData.Values)
                {
                    _storeData = hdv.StoreValues;

                    foreach (StoreDataValue sdv in _storeData.Values)
                    {
                        if (sdv.GrpLvlRID == glm.SglRID)
                        {
                            SetStoreVelocityMatrixRule(hdv.HeaderRID, sdv.StoreRID, glm.SglRID);
                        }
                        else
                        {
                            if (glm.SglRID == Include.TotalMatrixLevelRID)
                            {
                                SetStoreVelocityMatrixRule(hdv.HeaderRID, sdv.StoreRID, glm.SglRID);
                            }
                        }
                    }
                }
            }

            // BEGIN TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
            AllocationProfile ap; // TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
            foreach (HeaderDataValue hdv in _headerData.Values)
                {
                    ap = hdv.AloctnProfile; // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                    _storeData = hdv.StoreValues;
                   
                    foreach (StoreDataValue sdv in _storeData.Values)
                        {
                            sdv.InitialRuleType = sdv.RuleType;
                            sdv.InitialRuleQuantity = sdv.RuleQty;
                            sdv.InitialRuleTypeQty = sdv.RuleTypeQty;
                            sdv.InitialWillShip = sdv.WillShip;
                        }
                    // BEGIN TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                    //if (_balance == true && _bypassbal == false)
                    if (_reconcile == true)
                    // END TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
                    {
					    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                        //ApplySoftRulesToStores(AST);
						ApplySoftRulesToStores(AST, hdv);
						// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                        foreach (StoreDataValue sdv in _storeData.Values)
                        {
                            switch (this.Component.ComponentType)
                            {
                                case (eComponentType.Total):
								    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
									sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    // sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
									// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    // sdv.RuleQty = sdv.RuleTypeQty;
                                    // sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Total, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.SpecificPack):
								    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
									sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    // sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
									// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    // sdv.RuleQty = sdv.RuleTypeQty;
                                    // sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationPackComponent)Component).PackName, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.GenericType):
								    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
									sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    // sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
									// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    // sdv.RuleQty = sdv.RuleTypeQty;
                                    // sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.GenericType, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.DetailType):
								    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
									sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    // sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
									// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    // sdv.RuleQty = sdv.RuleTypeQty;
                                    // sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.DetailType, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.SpecificColor):
								    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
									sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    // sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
									// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    // sdv.RuleQty = sdv.RuleTypeQty;
                                    // sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                                case (eComponentType.Bulk):
								    // BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    //sdv.RuleType = (eVelocityRuleType)AlocProfile.GetStoreChosenRuleType(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    //sdv.RuleTypeQty = AlocProfile.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
									sdv.RuleType = (eVelocityRuleType)ap.GetStoreChosenRuleType(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    // BEGIN TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    // sdv.RuleTypeQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
									// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                                    // sdv.RuleQty = sdv.RuleTypeQty;
                                    // sdv.WillShip = Convert.ToInt32(sdv.RuleTypeQty);
                                    sdv.RuleQty = ap.GetStoreChosenRuleQtyAllocated(eAllocationSummaryNode.Bulk, sdv.StoreRID);
                                    sdv.WillShip = Convert.ToInt32(sdv.RuleQty);
                                    // END TT#3116 - AGallagher - Velocity when Reconcile is used Velocity Rule Type Qty gets overlaid
                                    break;
                            }
                        }
						// BEGIN TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                        //AlocProfile.RemoveSoftChosenRule();
						ap.RemoveSoftChosenRule(); 
						// END TT#2556 - AGallagher/JEllis - Velocity when allocating more than 1 header not getting expected results
                    }
                    // END TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate

                    //tt#152 - Velocity Balance - apicchetti
                    // BEGIN TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
                    //if (_balance == true)
                    if (_balanceToHeaderInd == '1' && _bypassbal == false)
                    {
                        PerformBalanceToHeader(hdv);
                    }
                    else
                    {
                        if (_balance == true && _bypassbal == false)
                        // END TT#1082 - AGallagher - Velocity - Velocity Balance - wos rule and ship up to rule not reconciling as expected- balance then over allocate
                        {
                            PerformBalance(hdv);
                        }
                    }
                    //tt#152 - Velocity Balance - apicchetti
                }
        }

		/// <summary>
		/// Gets a store rule quantity
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Rule Quantity for the store</returns>
		/// <remarks>Retrieves a store rule quantity for a particular header</remarks>
		public double GetStoreVelocityRuleQty(int aHdrRID, int aStoreRID)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			return sdv.RuleQty;
		}

        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
        public double GetStoreVelocityRuleTypeQty(int aHdrRID, int aStoreRID)
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            return sdv.RuleTypeQty;
        }
        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

        // BEGIN TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)  
        public double GetStoreAvgWeeklySupply(int aStoreRID)
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[((AllocationProfile)AST.GetAllocationProfileList().ArrayList[0]).Key];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            if (sdv.AvgWeeklySales > 0.0)
                sdv.AvgWeeklySupply = (sdv.BasisOnHand + sdv.BasisIntransit) / sdv.AvgWeeklySales;
                else
                sdv.AvgWeeklySupply = 0.0;
            
            return sdv.AvgWeeklySupply; 
        }
         
        public double GetStoreGrpAvgWeeklySupply(int aStoreGRPRID, int aStoreGRPLVLRID)
        {
            double GroupAvgWeeklySales = 0;
            double GroupOnHand = 0;
            double RetGroupAvgWeeklySales = 0;

            ProfileList SPL = AST.GetActiveStoresInGroup(aStoreGRPRID, aStoreGRPLVLRID);
            foreach (StoreProfile sp in SPL)
            {
                StoreDataValue sdv = (StoreDataValue)_storeData[sp.Key];
                if (sdv.IsEligible)
                {
                    if (!sdv.IsReserve)
                    {
                        GroupAvgWeeklySales = (double)GroupAvgWeeklySales + (double)sdv.AvgWeeklySales;
                        GroupOnHand = (double)GroupOnHand + (double)sdv.BasisOHandIT;
                    }
                }
            }
                if (GroupAvgWeeklySales > 0.0)
                    RetGroupAvgWeeklySales = GroupOnHand / GroupAvgWeeklySales;
                else
                    RetGroupAvgWeeklySales = 0.0;
           
            return RetGroupAvgWeeklySales;
       }
       // END TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

        // BEGIN TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
        public double GetStoreGrpAvgWeeklySales(int aStoreGRPRID, int aStoreGRPLVLRID)
        {
            double GroupAvgWeeklySales = 0;
            
            ProfileList SPL = AST.GetActiveStoresInGroup(aStoreGRPRID, aStoreGRPLVLRID);
            foreach (StoreProfile sp in SPL)
            {
                StoreDataValue sdv = (StoreDataValue)_storeData[sp.Key];
                if (sdv.IsEligible)
                {
                    if (!sdv.IsReserve)
                    {
                        GroupAvgWeeklySales = (double)GroupAvgWeeklySales + (double)sdv.AvgWeeklySales;
                    }
                }
            }

            return GroupAvgWeeklySales;
        }

        public double GetStoreGrpAvgWeeklyStock(int aStoreGRPRID, int aStoreGRPLVLRID)
        {
            double GroupAvgWeeklyStock = 0;

            ProfileList SPL = AST.GetActiveStoresInGroup(aStoreGRPRID, aStoreGRPLVLRID);
            foreach (StoreProfile sp in SPL)
            {
                StoreDataValue sdv = (StoreDataValue)_storeData[sp.Key];
                if (sdv.IsEligible)
                {
                    if (!sdv.IsReserve)
                    {
                        GroupAvgWeeklyStock = (double)GroupAvgWeeklyStock + (double)sdv.AvgWeeklyStock;
                    }
                }
            }

            return GroupAvgWeeklyStock;
        }

        public double GetStoreGrpBasisSales(int aStoreGRPRID, int aStoreGRPLVLRID)
        {
            double GroupAvgBasisSales = 0;

            ProfileList SPL = AST.GetActiveStoresInGroup(aStoreGRPRID, aStoreGRPLVLRID);
            foreach (StoreProfile sp in SPL)
            {
                StoreDataValue sdv = (StoreDataValue)_storeData[sp.Key];
                if (sdv.IsEligible)
                {
                    if (!sdv.IsReserve)
                    {
                        GroupAvgBasisSales = (double)GroupAvgBasisSales + (double)sdv.BasisSales;
                    }
                }
            }

            return GroupAvgBasisSales;
        }

        public double GetStoreGrpBasisStock(int aStoreGRPRID, int aStoreGRPLVLRID)
        {
            double GroupAvgBasisStock = 0;

            ProfileList SPL = AST.GetActiveStoresInGroup(aStoreGRPRID, aStoreGRPLVLRID);
            foreach (StoreProfile sp in SPL)
            {
                StoreDataValue sdv = (StoreDataValue)_storeData[sp.Key];
                if (sdv.IsEligible)
                {
                    if (!sdv.IsReserve)
                    {
                        GroupAvgBasisStock = (double)GroupAvgBasisStock + (double)sdv.BasisStock;
                    }
                }
            }

            return GroupAvgBasisStock;
        }

        public double GetStoreGrpPctSellThru(int aStoreGRPRID, int aStoreGRPLVLRID)
        {
            
            double GroupAvgWeeklySales = 0;
            double GroupAvgWeeklyStock = 0;
            double GroupAvgPctSellThru = 0;

            ProfileList SPL = AST.GetActiveStoresInGroup(aStoreGRPRID, aStoreGRPLVLRID);
            foreach (StoreProfile sp in SPL)
            {
                StoreDataValue sdv = (StoreDataValue)_storeData[sp.Key];
                if (sdv.IsEligible)
                {
                    if (!sdv.IsReserve)
                    {
                        GroupAvgWeeklySales = (double)GroupAvgWeeklySales + (double)sdv.AvgWeeklySales;
                        GroupAvgWeeklyStock = (double)GroupAvgWeeklyStock + (double)sdv.AvgWeeklyStock;
                    }
                }
            }
            // Begin TT#1013 - RMatelic - Changed the Attribute in the Velocity Store Detail Screen and received a "System Overflow Exception".
            //if (GroupAvgWeeklySales > 0.0)
            if (GroupAvgWeeklyStock > 0.0)
            // End TT#1013
                GroupAvgPctSellThru = GroupAvgWeeklySales / GroupAvgWeeklyStock;
            else
                GroupAvgPctSellThru = 0.0;

            return GroupAvgPctSellThru;
        }
        // End TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
    

		/// <summary>
		/// Gets a store will ship quantity
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Will Ship Quantity for the store</returns>
		/// <remarks>Retrieves a store will ship quantity</remarks>
		public int GetStoreVelocityRuleResult(int aHdrRID, int aStoreRID) // MID Track 4298 Integer types showing decimal values
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			if (sdv.ReCalcWillShip)
			{
                // UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty, sdv.RuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
			}

			return sdv.WillShip;
		}


        /// <summary>
        /// Gets a store initial rule quantity
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Initial Rule Quantity for the store</returns>
        /// <remarks>Retrieves a store initial rule quantity</remarks>
        public double GetStoreVelocityInitialRuleQty(int aHdrRID, int aStoreRID) // tt #152 Velocity Balance
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            if (sdv.ReCalcWillShip)
            {
                // UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty, sdv.RuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            }

            return sdv.InitialRuleQuantity;
        }

        /// <summary>
        /// Gets a store initial will ship
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Initial Will Ship for the store</returns>
        /// <remarks>Retrieves a store initial will ship</remarks>
        public double GetStoreVelocityInitialWillShip(int aHdrRID, int aStoreRID) // tt #152 Velocity Balance
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            if (sdv.ReCalcWillShip)
            {
                //UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty, sdv.RuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            }

            return sdv.InitialWillShip;
        }

        /// <summary>
        /// Gets a store initial rule type
        /// </summary>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aStoreRID">Store RID</param>
        /// <returns>Initial Rule Type for the store</returns>
        /// <remarks>Retrieves a store initial rule type</remarks>
        public eVelocityRuleType GetStoreVelocityInitialRuleType(int aHdrRID, int aStoreRID) // tt #152 Velocity Balance
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            if (sdv.ReCalcWillShip)
            {
                // UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty, sdv.RuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
            }

            return sdv.InitialRuleType;
        }

        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        public double GetStoreVelocityInitialRuleTypeQty(int aHdrRID, int aStoreRID) 
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            if (sdv.ReCalcWillShip)
            {
                UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty, sdv.RuleTypeQty);  
            }

            return sdv.InitialRuleTypeQty;
        }
        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

		/// <summary>
		/// Gets a store transfer quantity
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Transfer Quantity for the store</returns>
		/// <remarks>Retrieves a store transfer quantity</remarks>
		public int GetStoreVelocityTransferQty(int aHdrRID, int aStoreRID) // MID Track 4298 Integer Types showing decimal value
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			return sdv.Transfer;
		}

		/// <summary>
		/// Gets a store rule type
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Rule Type for the store</returns>
		/// <remarks>Retrieves a store rule type</remarks>
		public eVelocityRuleType GetStoreVelocityRuleType(int aHdrRID, int aStoreRID)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			return sdv.RuleType;
		}

		/// <summary>
		/// Sets a store rule quantity
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aRuleQty">Rule Quantity</param>
		/// <remarks>Updates a store rule quantity</remarks>
		public void SetStoreVelocityRuleQty(int aHdrRID, int aStoreRID, double aRuleQty)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			if (aRuleQty != sdv.RuleQty)
			{
				sdv.RuleQty = aRuleQty;
				sdv.ReCalcWillShip = true;
			}
		}

        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        public void SetStoreVelocityRuleTypeQty(int aHdrRID, int aStoreRID, double aRuleTypeQty)
        {
            HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

            _storeData = hdv.StoreValues;

            StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

            if (aRuleTypeQty != sdv.RuleTypeQty)
            {
                sdv.RuleTypeQty = aRuleTypeQty;
                sdv.RuleQty = sdv.RuleTypeQty;
                sdv.ReCalcWillShip = true;
            }
        }
        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

        /// <summary>
		/// Sets a store rule type
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <remarks>Updates a store rule type</remarks>
		public void SetStoreVelocityRuleType(int aHdrRID, int aStoreRID, eVelocityRuleType aRuleType)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			if (aRuleType != sdv.RuleType)
			{
				sdv.RuleType = aRuleType;
				sdv.ReCalcWillShip = true;
			}
		}

		/// <summary>
		/// Sets a store rule type and quantity
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aRuleQty">Rule Quantity</param>
		/// <remarks>Updates a store rule type and quantity</remarks>
        // public void SetStoreVelocityRuleTypeAndQty(int aHdrRID, int aStoreRID, eVelocityRuleType aRuleType, double aRuleQty)  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        public void SetStoreVelocityRuleTypeAndQty(int aHdrRID, int aStoreRID, eVelocityRuleType aRuleType, double aRuleQty, double aRuleTypeQty)  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			if (aRuleType != sdv.RuleType)
			{
				sdv.RuleType = aRuleType;
				sdv.ReCalcWillShip = true;
			}

            if (aRuleQty != sdv.RuleQty)
            {
                sdv.RuleQty = aRuleQty;
                sdv.ReCalcWillShip = true;
            }

            // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)            
            if (aRuleTypeQty != sdv.RuleTypeQty)
            {
                sdv.RuleTypeQty = aRuleTypeQty;
                sdv.RuleQty = sdv.RuleTypeQty;
                sdv.ReCalcWillShip = true;
            }
            // End TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
		}

		/// <summary>
		/// Updates stores with detail changes
		/// </summary>
		/// <remarks>Update stores when detail changes are made</remarks>
		public void ApplyDetailChanges()
		{
			foreach(HeaderDataValue hdv in _headerData.Values)
			{
				_storeData = hdv.StoreValues;

				foreach(StoreDataValue sdv in _storeData.Values)
				{
					if (sdv.ReCalcWillShip)
					{
                        // UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        UpdateStoreWillShip(hdv.HeaderRID, sdv.StoreRID, sdv.RuleType, sdv.RuleQty, sdv.RuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
					}
				}
			}
		}

		/// <summary>
		/// Calculate a store will ship quantity
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aRuleQty">Rule Quantity</param>
		/// <remarks>Updates a store will ship quantity</remarks>
        // public void UpdateStoreWillShip(int aHdrRID, int aStoreRID, eVelocityRuleType aRuleType, double aRuleQty)  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        public void UpdateStoreWillShip(int aHdrRID, int aStoreRID, eVelocityRuleType aRuleType, double aRuleQty, double aRuleTypeQty)  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
		{
			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			if (sdv.IsEligible && !sdv.IsReserve)
			{
                // UpdateOneStore(false, aHdrRID, aStoreRID, sdv.GrpLvlRID, aRuleType, aRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                UpdateOneStore(false, aHdrRID, aStoreRID, sdv.GrpLvlRID, aRuleType, aRuleQty, aRuleTypeQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
			}
		}

		/// <summary>
		/// Sets a store rule type and quantity in a specified matrix
		/// </summary>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aGrpLvlRID">Group Level RID</param>
		/// <remarks>Updates a store rule type and quantity in a specified matrix</remarks>
		public void SetStoreVelocityMatrixRule(int aHdrRID, int aStoreRID, int aGrpLvlRID)
		{
			string CellKey = null;

			bool SetStoreRule = true;

			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			if (sdv.IsEligible && !sdv.IsReserve)
			{
				if (!sdv.UserRule)
				{
					if (aGrpLvlRID == Include.TotalMatrixLevelRID)
					{
						if (sdv.SGLRule)
						{
							SetStoreRule = false;
						}
						else
						{
							if (!CalculateAverageUsingChain)
							{
								CellKey = sdv.TotGrpCellKey;
							}
							else
							{
								CellKey = sdv.TotChnCellKey;
							}
						}
					}
					else
					{
						if (!CalculateAverageUsingChain)
						{
							CellKey = sdv.SGLGrpCellKey;
						}
						else
						{
							CellKey = sdv.SGLChnCellKey;
						}
					}
				}
				else
				{
					SetStoreRule = false;
				}
			}
			else
			{
				SetStoreRule = false;
			}

			if (SetStoreRule)
			{
				GroupLvlMatrix glm = (GroupLvlMatrix)_groupLvlMtrxData[aGrpLvlRID];

				_groupLvlCellData = glm.MatrixCells;

				GroupLvlCell glc = (GroupLvlCell)_groupLvlCellData[CellKey];

				if (DetermineShipQtyUsingBasis)
				{
                    // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                    if (_gradeVariableType != eVelocityMethodGradeVariableType.Stock)
                    {
                        _BasisSalesorStock = sdv.BasisSales;
                    }
                    else
                    {
                        _BasisSalesorStock = sdv.BasisStock;
                    }
                    // END TT#3518 - AGallagher - Velocity Enhancements - Part 2

					if (sdv.BasisOHandIT <= 0.0)
					{
                        // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                        //if (sdv.BasisSales > 0)
                        if (_BasisSalesorStock > 0)
                        // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
						{
                            // UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            // BEGIN TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
                            //if (glm.ModeInd == 'N')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            //else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            if (glm.ModeInd == 'A')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty, true);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing // TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Rules
                            else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                                UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            // END TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
						}
						else
						{
                            // UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glm.NoOnHandRuleType, glm.NoOnHandRuleQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glm.NoOnHandRuleType, glm.NoOnHandRuleQty, glm.NoOnHandRuleQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
						}
					}
					else
					{
                        // UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        // BEGIN TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
                        //if (glm.ModeInd == 'N')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                        //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        //else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                        //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                        if (glm.ModeInd == 'A')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                            UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty, true);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing // TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Rules
                        else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        // END TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
					}
				}
				else
				{
					if (sdv.StyleOHandIT <= 0.0)
					{
                        // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                        _StockInUse = false;
                        _StockRuleAdd = false;
                        if (_gradeVariableType == eVelocityMethodGradeVariableType.Stock)
                        {
                            _StockInUse = true;
                            if (sdv.BasisStock > 0)
                            {
                                 _StockRuleAdd = true;
                            }
                        }
                        // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
                        // BEGIN TT#921 - AGallagher - Velocity - Velocity Stores with no on hand not meeting expectations
                        // if (sdv.StyleSales > 0.0)
                        // BEGIN TT#3518 - AGallagher - Velocity Enhancements - Part 2
                        //if (sdv.StyleSales > 0.0 || (sdv.BasisOHandIT > 0.0))
                        if ((sdv.StyleSales > 0.0 && _StockInUse == false) || (sdv.BasisOHandIT > 0.0 && _StockInUse == false) || (_StockInUse == true && _StockRuleAdd == true))
                        // END TT#3518 - AGallagher - Velocity Enhancements - Part 2
                        // END TT#921 - AGallagher - Velocity - Velocity Stores with no on hand not meeting expectations
						{
                            // UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            // BEGIN TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
                            //if (glm.ModeInd == 'N')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            //else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                            if (glm.ModeInd == 'A')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                                UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty, true);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing // TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Rules
                            else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                                UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            // END TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
						}
						else
						{
                            // UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glm.NoOnHandRuleType, glm.NoOnHandRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glm.NoOnHandRuleType, glm.NoOnHandRuleQty, glm.NoOnHandRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
						}
					}
					else
					{
                        // UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        // BEGIN TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
                        //if (glm.ModeInd == 'N')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                        //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        //else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                        //UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                        if (glm.ModeInd == 'A')  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing
                            UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, sdv.SpreadRuleType, sdv.SpreadRuleQty, sdv.SpreadRuleTypeQty, true);  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing // TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Rules
                        else  // TT#637 - AGallagher - Velocity - Spread Average (#7) - Processing 
                            UpdateOneStore(true, aHdrRID, aStoreRID, aGrpLvlRID, glc.CellRuleType, glc.CellRuleQty, glc.CellRuleTypeQty); // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        // END TT#857 - AGallagher - Velocity - Velocity Min/Max returning Rule of zero
					}
				}
			}
		}

		/// <summary>
		/// Calculate the new will ship quantity for one store
		/// </summary>
		/// <param name="aFromMatrix">Switch indicating calling method</param>
		/// <param name="aHdrRID">Header RID</param>
		/// <param name="aStoreRID">Store RID</param>
        /// <param name="aGrpLvlRID">Attribute Set to which this store belongs</param>
		/// <param name="aRuleType">Rule Type</param>
		/// <param name="aRuleQty">Rule Quantity</param>
		/// <remarks>Calculates a store will ship quantity</remarks>
        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
        // public void UpdateOneStore(bool aFromMatrix, int aHdrRID, int aStoreRID, int aGrpLvlRID, eVelocityRuleType aRuleType, double aRuleQty)
        public void UpdateOneStore(bool aFromMatrix, int aHdrRID, int aStoreRID, int aGrpLvlRID, eVelocityRuleType aRuleType, double aRuleQty, double aRuleTypeQty)
        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
		{
            // begin TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Logic
            UpdateOneStore(aFromMatrix, aHdrRID, aStoreRID, aGrpLvlRID, aRuleType, aRuleQty, aRuleTypeQty, false);
        }
        /// <summary>
        /// Calculate the new will ship quantity for one store
        /// </summary>
        /// <param name="aFromMatrix">Switch indicating calling method</param>
        /// <param name="aHdrRID">Header RID</param>
        /// <param name="aStoreRID">Store RID</param>
        /// <param name="aGrpLvlRID">Attribute Set to which this store belongs</param>
        /// <param name="aRuleType">Rule Type</param>
        /// <param name="aRuleQty">Rule Quantity</param>
        /// <param name="aSpreadRule">True:  rule is result of a spread; False: rule is not result of spread</param>
        /// <remarks>Calculates a store will ship quantity</remarks>
        public void UpdateOneStore(bool aFromMatrix, int aHdrRID, int aStoreRID, int aGrpLvlRID, eVelocityRuleType aRuleType, double aRuleQty, double aRuleTypeQty, bool aSpreadRule)
        {
            // end TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Logic
			int Minimum = 0;
			int Maximum = 0;
			int Multiple = 0;
			int AdMinimum = 0;
			int ColorMinimum = 0;
			int ColorMaximum = 0;

			double OnHand = Include.NoRuleQty;
			//double CalcQty = Include.NoRuleQty; // MID Track 4298 Integer Type showing fractional decimal
			int CalcQty = 0; // MID Track 4298 Integer Type showing fractional decimal
			double RuleQty = Include.NoRuleQty;
			// begin MID Track 4298 Integer type showing decimal values
			//double XferQty = Include.NoRuleQty;
			//double WillShip = Include.NoRuleQty;
			int XferQty = 0;
			int WillShip = 0;
			// end MID Track 4298 Integer type showing decimal value
			double AvgWeeklySales = Include.NoRuleQty;

			eVelocityRuleType RuleType = eVelocityRuleType.None;

			RuleQty = aRuleQty;

			RuleType = aRuleType;

			HeaderDataValue hdv = (HeaderDataValue)_headerData[aHdrRID];

			AllocationProfile ap = hdv.AloctnProfile;

			_storeData = hdv.StoreValues;

			StoreDataValue sdv = (StoreDataValue)_storeData[aStoreRID];

			AvgWeeklySales = sdv.AvgWeeklySales;

			// BEGIN TT#616 - STodd - pack rounding
			bool genericPackRounding = false;
			double fstPackRounding = Include.DefaultGenericPackRounding1stPackPct;
			double nthPackRounding = Include.DefaultGenericPackRoundingNthPackPct;
			int packRoundingMultiple = 0;
			// End TT#616 - STodd - pack rounding
            switch (Component.ComponentType)
			{
				case eComponentType.Total:
					Multiple = ap.GetMultiple(Component);
					// BEGIN TT#616 - STodd - pack rounding
					// if only generic packs...
					if (ap.GenericPackCount > 0 && ap.NonGenericPackCount == 0 && ap.BulkColorCount == 0)
					{
						foreach (PackHdr ph in ap.Packs.Values)
						{
							if (ph.GenericPack)
							{
								genericPackRounding = ap.DoesGenericPackRoundingApply(ref fstPackRounding, ref nthPackRounding, Multiple);
								if (genericPackRounding)
								{
									// If generic pack rounding can be applied, change multiple to 1 for Velocity.
									// Pack rounding will handle it when it is allocated.
									Multiple = 1;
									break;
								}
							}
						}
					}
					// END TT#616 - STodd - pack rounding
					break;
				case eComponentType.Bulk:
					Multiple = ap.BulkMultiple;
					break;
				case eComponentType.SpecificPack:
					Multiple = ap.GetPackMultiple(((AllocationPackComponent)Component).PackName);
					// BEGIN TT#616 - STodd - pack rounding
					PackHdr phdr = ap.GetPackHdr(((AllocationPackComponent)Component).PackName);
					if (phdr.GenericPack)
					{
						genericPackRounding = ap.DoesGenericPackRoundingApply(ref fstPackRounding, ref nthPackRounding, Multiple);
						if (genericPackRounding)
						{
							// If generic pack rounding can be applied, change multiple to 1 for Velocity.
							// Pack rounding will handle it when it is allocated.
							packRoundingMultiple = 1;
							break;
						}
					}
					// END TT#616 - STodd - pack rounding

					// begin MID Track 4060 Pack Components get allocated times their multiple
                    //if (!aFromMatrix)  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
                    //{                  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
						// on store detail rule quantity is number of packs
						// on velocity matrix rule quantity is units
                    //    RuleQty = (double)((int)(RuleQty * (double)Multiple)); // MID Track 4298 integer type showing decimal value  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
                    //}                  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug      
					// end MID Track 4060 Pack Components get allocated times their multiple
					break;
				case eComponentType.SpecificColor:
//                  NOTE: GetColorMulitple may be mispelled
					Multiple = ap.GetColorMulitple(((AllocationColorOrSizeComponent)Component).ColorRID);
					break;
				default:
					Multiple = 1;
					break;
			}

			if (DetermineShipQtyUsingBasis)
			{
				OnHand = sdv.BasisOHandIT;
			}
			else
			{
				OnHand = sdv.StyleOHandIT;
			}

			switch (RuleType)
			{
				// ========================================================
				// These rule types can be applied to all header components
				// ========================================================
				case eVelocityRuleType.Out:
					// begin MID Track 4298 Integer type value showing fractional value
					//CalcQty = Include.NoRuleQty;
					//XferQty = Include.NoRuleQty;
					//WillShip = Include.NoRuleQty;
					CalcQty = 0;
					XferQty = 0;
					WillShip = 0;
                    sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
					// end MID Track 4298 Integer type value showing fractional value
					break;
				case eVelocityRuleType.None:
					// begin MID Track 4298 Integer type value showing fractional value
					//CalcQty = Include.NoRuleQty;
					//XferQty = Include.NoRuleQty;
					//WillShip = Include.NoRuleQty;
					CalcQty = 0;
					XferQty = 0;
					WillShip = 0;
                    sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) 
					// end MID Track 4298 Integer type value showing fractional value
					break;
				case eVelocityRuleType.ShipUpToQty:
                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                    if (IsInteractive)
                        {
                        sdv.RuleTypeQty = RuleQty;  
                        }
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                   	CalcQty = CalcShipUpToQty(RuleQty);
					XferQty = CalcXferQty(CalcQty, OnHand, sdv.PreSizeAllocated); // MID track 4282 Velocity overlays Fill Size Holes allocation
					// begin MID Track 4298 Integer type value showing fractional value
					if (XferQty < 0)
					{
						WillShip = 0;
					}
					else
					{
						// BEGIN TT#616 - STodd - pack rounding
						if (packRoundingMultiple > 0)
						{
							WillShip = CalcWillShipQty(CalcQty, OnHand, packRoundingMultiple, sdv.PreSizeAllocated);  // MID Traack 4282 Velocity overlays Fill Size Holes allocation
						}
						else
						{
							WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple, sdv.PreSizeAllocated);  // MID Traack 4282 Velocity overlays Fill Size Holes allocation
						}
						// End TT#616 - STodd - pack rounding
					}
					//if (XferQty < Include.NoRuleQty)
					//{
					//	WillShip = Include.NoRuleQty;
					//}
					//else
					//{
					//	WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple);
					//}
					// end MID track 4298 Integer type value showing fractional value
                    // BEGIN TT#1212 - AGallagher - Velocity - Velocity Min/Max needs to be applied to the Velocity Ship Up To rule 
                    // same as BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    if (_IBInventoryInd == 'A')
                    {
                    // END TT#1287 - AGallagher - Inventory Min/Max
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                break;
                            case 'S':
                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                                //_ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                                //_ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                _ApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                _ApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                                Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _ApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;  
                                    sdv.RuleTypeQty = Include.NoRuleQty;  
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _ApplyCalcMax)
                                { WillShip = _ApplyCalcMax; }

                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    {
                                        _ApplyCalcMin = gll.AllocMin;
                                        _ApplyCalcMax = gll.AllocMax;
                                    }

                                }

                                Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _ApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _ApplyCalcMax)
                                { WillShip = _ApplyCalcMax; }

                                break;
                        }
                    } // TT#1287 - AGallagher - Inventory Min/Max
                    // same as END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // END TT#1212 - AGallagher - Velocity - Velocity Min/Max needs to be applied to the Velocity Ship Up To rule 
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    // IBSUT
                    if (_IBInventoryInd == 'I')
                    {
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                break;
                            case 'S':

                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
								// Begin TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                                _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
								// End TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 

                                //_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                //_IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                _IBApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                _IBApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }
                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _IBApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _IBApplyCalcMax)
                                { WillShip = (int)_IBApplyCalcMax; }

                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    {
                                        _ApplyCalcMin = gll.AllocMin;
                                        _ApplyCalcMax = gll.AllocMax;
                                    }

                                }

                                _IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }
                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _IBApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _IBApplyCalcMax)
                                { WillShip = (int)_IBApplyCalcMax; }

                                break;
                        } 
                    }
                    // END TT#1287 - AGallagher - Inventory Min/Max
					break;
				case eVelocityRuleType.WeeksOfSupply:
                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                    if (IsInteractive) 
                        {
                        sdv.RuleTypeQty = RuleQty;  
                        }
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
					CalcQty = CalcWOSQty(AvgWeeklySales, RuleQty);
					XferQty = CalcXferQty(CalcQty, OnHand, sdv.PreSizeAllocated); // MID Track 4282 Velocity overlays Fill Size Holes allocation
					// begin MID Track 4298 Integer type value showing fractional value
					if (XferQty < 0)
					{
						WillShip = 0;
					}
					else
					{
						// BEGIN TT#616 - STodd - pack rounding
						if (packRoundingMultiple > 0)
						{
							WillShip = CalcWillShipQty(CalcQty, OnHand, packRoundingMultiple, sdv.PreSizeAllocated); // MID track 4282 Velocity overlays Fill Size Holes allocation
						}
						else
						{
							WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple, sdv.PreSizeAllocated); // MID track 4282 Velocity overlays Fill Size Holes allocation
						}
						// End TT#616 - STodd - pack rounding
						//WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple, sdv.PreSizeAllocated); // MID track 4282 Velocity overlays Fill Size Holes allocation
					}
					//if (XferQty < Include.NoRuleQty)
					//{
					//	WillShip = Include.NoRuleQty;
					//}
					//else
					//{
					//	WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple);
					//}
					// end MID Track 4298 Integer type value showing fractional value
                    

                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    if (_IBInventoryInd == 'A')
                    {
                    // END TT#1287 - AGallagher - Inventory Min/Max
                    // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    switch (_ApplyMinMaxInd)
                    {
                        case 'N':
                            break;
                        case 'S':
                            // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                            // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                            //_ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                            //_ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
                            Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                            _ApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                            _ApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                            // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                            Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                            Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                            if (WillShip < _ApplyCalcMin)
                                { 
                                WillShip = 0;
                                RuleType = eVelocityRuleType.None;
                                // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                sdv.RuleQty = Include.NoRuleQty;
                                sdv.RuleTypeQty = Include.NoRuleQty;
                                CalcQty = 0;
                                // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                            if (WillShip > _ApplyCalcMax)
                               { WillShip = _ApplyCalcMax; }

                            break;
                        case 'V':
                            // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                            if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                            else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                            // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                            foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                            {
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                //if (sdv.BasisGrade == gll.Grade)
                                if (_ApplyGrade == gll.Grade)
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    { _ApplyCalcMin = gll.AllocMin;
                                     _ApplyCalcMax = gll.AllocMax; }
                                
                            }

                            Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                            Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                            if (WillShip < _ApplyCalcMin)
                                { 
                                WillShip = 0;
                                RuleType = eVelocityRuleType.None;
                                // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                sdv.RuleQty = Include.NoRuleQty;
                                sdv.RuleTypeQty = Include.NoRuleQty;
                                CalcQty = 0;
                                // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                            if (WillShip > _ApplyCalcMax)
                                { WillShip = _ApplyCalcMax; }
                                
                            break;
                    }
                    } // TT#1287 - AGallagher - Inventory Min/Max
                    // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    // IBWOS
                    if (_IBInventoryInd == 'I')
                    {
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                break;
                            case 'S':

                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
								// Begin TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                                _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
								// End TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 

                                //_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                //_IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                _IBApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                _IBApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }
                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _IBApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _IBApplyCalcMax)
                                { WillShip = (int)_IBApplyCalcMax; }

                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    {
                                        _ApplyCalcMin = gll.AllocMin;
                                        _ApplyCalcMax = gll.AllocMax;
                                    }

                                }

                                _IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }
                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _IBApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _IBApplyCalcMax)
                                { WillShip = (int)_IBApplyCalcMax; }

                                break;
                        }
                    }
                    // END TT#1287 - AGallagher - Inventory Min/Max
					break;
				case eVelocityRuleType.AbsoluteQuantity:
                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                    if (IsInteractive)
                        {
                        sdv.RuleTypeQty = RuleQty;  
                        }
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
					//XferQty = Include.NoRuleQty; // MID Track 4298 Integer type value showing fractional value
					XferQty = 0; // MID track 4298 Integer type value showing fractional value
					CalcQty = CalcAbsQty(RuleQty);
					WillShip = CalcQty;
                    // begin TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Rules
                    if (aSpreadRule)
                    {
                        if (_IBInventoryInd == 'A')
                        {
                            switch (_ApplyMinMaxInd)
                            {
                                case 'N':
                                    break;
                                case 'S':
                                    // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                    // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
									// Begin TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                    _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                                    _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
									// End TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                    Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                    _ApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                    _ApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                    // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                                    Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                    Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                    if (WillShip < _ApplyCalcMin)
                                    {
                                        WillShip = 0;
                                        RuleType = eVelocityRuleType.None;
                                        sdv.RuleQty = Include.NoRuleQty;
                                        sdv.RuleTypeQty = Include.NoRuleQty;
                                        CalcQty = 0;
                                    }
                                    if (WillShip > _ApplyCalcMax)
                                    { 
                                        WillShip = _ApplyCalcMax; 
                                    }
                                    break;
                                case 'V':
                                    if (CalculateAverageUsingChain)
                                    { 
                                        _ApplyGrade = sdv.SGLChnVelocityGrade;
                                    }
                                    else
                                    { 
                                        _ApplyGrade = sdv.SGLGrpVelocityGrade; 
                                    }
                                    foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                    {
                                        if (_ApplyGrade == gll.Grade)
                                        {
                                            _ApplyCalcMin = gll.AllocMin;
                                            _ApplyCalcMax = gll.AllocMax;
                                        }
                                    }

                                    Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                    Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                    if (WillShip < _ApplyCalcMin)
                                    {
                                        WillShip = 0;
                                        RuleType = eVelocityRuleType.None;
                                        sdv.RuleQty = Include.NoRuleQty;
                                        sdv.RuleTypeQty = Include.NoRuleQty;
                                        CalcQty = 0;
                                    }
                                    if (WillShip > _ApplyCalcMax)
                                    {
                                        WillShip = _ApplyCalcMax;
                                    }
                                    break;
                            }
                        } 
                        if (_IBInventoryInd == 'I')
                        {
                            switch (_ApplyMinMaxInd)
                            {
                                case 'N':
                                    break;
                                case 'S':

                                    // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                    // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
									// Begin TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                    _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                                    _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
									// End TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                    //_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                    //_IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;
                                    Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                    _IBApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                    _IBApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                    // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                    if (_IBApplyCalcMin < 0.0)
                                    {
                                        _IBApplyCalcMin = 0.0;
                                    }
                                    if (_IBApplyCalcMax < 0.0)
                                    {
                                        _IBApplyCalcMax = 0.0;
                                    }

                                    Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                    Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                    if (WillShip < _IBApplyCalcMin)
                                    {
                                        WillShip = 0;
                                        RuleType = eVelocityRuleType.None;
                                        sdv.RuleQty = Include.NoRuleQty;
                                        sdv.RuleTypeQty = Include.NoRuleQty;
                                        CalcQty = 0;
                                    }

                                    if (WillShip > _IBApplyCalcMax)
                                    { 
                                        WillShip = (int)_IBApplyCalcMax; 
                                    }
                                    break;
                                case 'V':
                                    if (CalculateAverageUsingChain)
                                    { 
                                        _ApplyGrade = sdv.SGLChnVelocityGrade; 
                                    }
                                    else
                                    {
                                        _ApplyGrade = sdv.SGLGrpVelocityGrade;
                                    }
                                    foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                    {
                                        if (_ApplyGrade == gll.Grade)
                                        {
                                            _ApplyCalcMin = gll.AllocMin;
                                            _ApplyCalcMax = gll.AllocMax;
                                        }
                                    }
                                    _IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                    _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;
                                    if (_IBApplyCalcMin < 0.0)
                                    {
                                        _IBApplyCalcMin = 0.0;
                                    }
                                    if (_IBApplyCalcMax < 0.0)
                                    {
                                        _IBApplyCalcMax = 0.0;
                                    }

                                    Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                    Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                    if (WillShip < _IBApplyCalcMin)
                                    {
                                        WillShip = 0;
                                        RuleType = eVelocityRuleType.None;
                                        sdv.RuleQty = Include.NoRuleQty;
                                        sdv.RuleTypeQty = Include.NoRuleQty;
                                        CalcQty = 0;
                                    }
                                    if (WillShip > _IBApplyCalcMax)
                                    { 
                                        WillShip = (int)_IBApplyCalcMax; 
                                    }
                                    break;
                            }
                        }
                    }
                    // end TT#675 - MD - Jellis - Apply Inventory Min Max Logic to Spread Rules
					break;
				case eVelocityRuleType.ForwardWeeksOfSupply:
                    // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                    if (IsInteractive)
                    {
                        sdv.RuleTypeQty = RuleQty;
                    }
                    // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
					CalcQty = CalcFwdWOSQty(sdv.StoreRID, RuleQty);
					XferQty = CalcXferQty(CalcQty, OnHand, sdv.PreSizeAllocated); // MID track 4282 velocity overlays Fill Size Holes allocation
					// begin MID Track 4298 Integer type value showing fractional value
					if (XferQty < 0)
					{
						WillShip = 0;
					}
					else
					{
						// BEGIN TT#616 - STodd - pack rounding
						if (packRoundingMultiple > 0)
						{
							WillShip = CalcWillShipQty(CalcQty, OnHand, packRoundingMultiple, sdv.PreSizeAllocated); // MID track 4282 velocity overlays Fill Size Holes allocation
						}
						else
						{
							WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple, sdv.PreSizeAllocated); // MID track 4282 velocity overlays Fill Size Holes allocation
						}
						//WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple, sdv.PreSizeAllocated); // MID track 4282 velocity overlays Fill Size Holes allocation
						// End TT#616 - STodd - pack rounding
					}
					//if (XferQty < Include.NoRuleQty)
					//{
					//	WillShip = Include.NoRuleQty;
					//}
					//else
					//{
					//	WillShip = CalcWillShipQty(CalcQty, OnHand, Multiple);
					//}
					// end MID Track 4298 Integer type value showing fractional value
                    
                    
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    if (_IBInventoryInd == 'A')
                    {
                    // END TT#1287 - AGallagher - Inventory Min/Max
                    // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    switch (_ApplyMinMaxInd)
                    {
                        case 'N':
                            break;
                        case 'S':
                            // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                            // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                            //_ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                            //_ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
                            Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                            _ApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                            _ApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                            // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                            Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                            Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                            if (WillShip < _ApplyCalcMin)
                               { 
                               WillShip = 0;
                               RuleType = eVelocityRuleType.None;
                               // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                               sdv.RuleQty = Include.NoRuleQty;
                               sdv.RuleTypeQty = Include.NoRuleQty;
                               CalcQty = 0;
                               // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                               }

                            if (WillShip > _ApplyCalcMax)
                                { WillShip = _ApplyCalcMax; }

                            break;
                        case 'V':
                            // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                            if (CalculateAverageUsingChain)
                            { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                            else
                            { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                            // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                            foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                            {
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                //if (sdv.BasisGrade == gll.Grade)
                                if (_ApplyGrade == gll.Grade)
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                   { _ApplyCalcMin = gll.AllocMin;
                                     _ApplyCalcMax = gll.AllocMax; }

                            }

                            Minimum = _ApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                            Maximum = _ApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                            if (WillShip < _ApplyCalcMin)
                                { 
                                WillShip = 0;
                                RuleType = eVelocityRuleType.None;
                                // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                sdv.RuleQty = Include.NoRuleQty;
                                sdv.RuleTypeQty = Include.NoRuleQty;
                                CalcQty = 0;
                                // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                            
                            if (WillShip > _ApplyCalcMax)
                                { WillShip = _ApplyCalcMax; }
                                
                            break;
                    }
                    } // TT#1287 - AGallagher - Inventory Min/Max
                    // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    // IBFWOS
                    if (_IBInventoryInd == 'I')
                    {
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                break;
                            case 'S':
                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
								// Begin TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
                                _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
								// End TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
                                //_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                //_IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                _IBApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                _IBApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }
                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _IBApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _IBApplyCalcMax)
                                { WillShip = (int)_IBApplyCalcMax; }
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    {
                                        _ApplyCalcMin = gll.AllocMin;
                                        _ApplyCalcMax = gll.AllocMax;
                                    }

                                }

                                _IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }
                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                Minimum = (int)_IBApplyCalcMin;  // TT#1735-MD - JSmith - Velocity and Capacity
                                Maximum = (int)_IBApplyCalcMax;  // TT#1735-MD - JSmith - Velocity and Capacity

                                if (WillShip < _IBApplyCalcMin)
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }

                                if (WillShip > _IBApplyCalcMax)
                                { WillShip = (int)_IBApplyCalcMax; }

                                break;
                        }
                    }
                    // END TT#1287 - AGallagher - Inventory Min/Max
					break;
				// ==================================================================
				// These rule types can only be applied to the Total header component
				// ==================================================================
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                //case eVelocityRuleType.MinimumBasis:
                //    // Begin TT # 91 - stodd

                //    //AllocationSubtotalProfile asp = AST.GetAllocationGrandTotalProfile();
                //    //_lowLimSortedGradeData


                //    XferQty = 0; 
                //    Minimum = ap.GetGradeMinimum(sdv.BasisGradeIDX);
                //    CalcQty = CalcMinBasisQty(Minimum);
                //    WillShip = CalcQty;
                //    sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                //    break;
                //    // End TT # 91 - stodd
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
				case eVelocityRuleType.Minimum:
					// begin MID Track 4442 Ad Min Rule Qty not displayed in Velocity Store Detail
					//if (Component.ComponentType == eComponentType.Total)
					//{
						//XferQty = Include.NoRuleQty;  // MID Track 4298 Integer type showing fractional value
						XferQty = 0; // MID Track 4298 Integer type showing fractional value
                        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        //Minimum = ap.GetGradeMinimum(sdv.GradeIDX);
                     // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    if (_IBInventoryInd == 'A')
                    {
                    // END TT#1287 - AGallagher - Inventory Min/Max
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                Minimum = ap.GetGradeMinimum(sdv.GradeIDX);
                                break;
                            case 'S':
                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                                //Minimum = ap.GetGradeMinimum(sdv.GradeIDX);
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                Minimum = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    { Minimum = gll.AllocMin; }
                                }
                                break;
                        }
                    } // TT#1287 - AGallagher - Inventory Min/Max
                        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    // IBMIN
                    if (_IBInventoryInd == 'I')
                    {
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                Minimum = ap.GetGradeMinimum(sdv.GradeIDX);
                                break;
                            case 'S':

                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                                _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);	// TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 

                                //_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                _IBApplyCalcMin = ap.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                //_IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }

                                if (_IBApplyCalcMin > 0.0)
                                {
                                    Minimum = (int)_IBApplyCalcMin;}
                                else
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    {
                                        _ApplyCalcMin = gll.AllocMin;
                                    }

                                }

                                _IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMin < 0.0)
                                {
                                    _IBApplyCalcMin = 0.0;
                                }

                                if (_IBApplyCalcMin > 0.0)
                                {
                                    Minimum = (int)_IBApplyCalcMin;
                                }
                                else
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                                break;
                        }
                    }
                    // END TT#1287 - AGallagher - Inventory Min/Max
						CalcQty = CalcMinQty(Minimum);
						WillShip = CalcQty;
                        sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
					//}
					// end MID Track 4442 Ad Min Rule Qty not displayed in Velocity Store Detail
                          
                        
					break;
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                //case eVelocityRuleType.MaximumBasis:
                //    // Begin TT # 91 - stodd
                //    Maximum = ap.GetGradeMaximum(sdv.BasisGradeIDX);
                //    if (Maximum == Include.LargestIntegerMaximum)
                //    {
                //        CalcQty = 0;
                //        XferQty = 0;
                //        WillShip = 0;
                //        sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                //        RuleType = eVelocityRuleType.None;
                //    }
                //    else
                //    {
                //        XferQty = 0; 
                //        CalcQty = CalcMaxBasisQty(Maximum);
                //        WillShip = CalcQty;
                //        sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                //    }
                //    break;
                //    // End TT # 91 - stodd
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                
				case eVelocityRuleType.Maximum:
					// begin MID Track 4442 Ad Min Rule Qty not displayed in Velocity Store Detail
					//if (Component.ComponentType == eComponentType.Total)
					//{
                        // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        //Maximum = ap.GetGradeMaximum(sdv.GradeIDX);
                     // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    if (_IBInventoryInd == 'A')
                    {
                    // END TT#1287 - AGallagher - Inventory Min/Max
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                Maximum = ap.GetGradeMaximum(sdv.GradeIDX);
                                break;
                            case 'S':
                                
                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                                // Maximum = ap.GetGradeMaximum(sdv.GradeIDX);
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                Maximum = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    { Maximum = gll.AllocMax; }
                                }
                                break;
                        }
                    } // TT#1287 - AGallagher - Inventory Min/Max
                        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    // IBMAX
                    if (_IBInventoryInd == 'I')
                    {
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                Maximum = ap.GetGradeMaximum(sdv.GradeIDX);
                                break;
                            case 'S':

                                // begin TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum
                                // NOTE:  For GA:  the store minimum/maximum may be adjusted to account for member headers; so get these using the individual header where adjustments are already made
                                _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);	// TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 

                                ////_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                //_IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;
                                Index_RID storeIdxRID = ap.StoreIndex(aStoreRID);
                                _IBApplyCalcMax = ap.GetStoreMaximum(eAllocationSummaryNode.Total, storeIdxRID, false);
                                // end TT#4208 -  MD - Jellis - GA Velocity allocates less than Minimum

                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                if (_IBApplyCalcMax > 0.0)
                                {
                                    Maximum = (int)_IBApplyCalcMax;
                                }
                                else
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    { _ApplyCalcMax = gll.AllocMax; }
                                }
                                _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                if (_IBApplyCalcMax > 0.0)
                                {
                                    Maximum = (int)_IBApplyCalcMax;
                                }
                                else
                                {
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                                break;
                        }
                    }
                    // END TT#1287 - AGallagher - Inventory Min/Max
						if (Maximum == Include.LargestIntegerMaximum)
						{
							// begin MID Track 4298 Integer type showing fractional value
							//CalcQty = Include.NoRuleQty;
							//XferQty = Include.NoRuleQty;
							//WillShip = Include.NoRuleQty;
                            //CalcQty = 0;  // TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
							XferQty = 0;
							WillShip = 0;
                            //sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) // TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process 
							// end MID Track 4298 Integer type showing fractional value
							RuleType = eVelocityRuleType.None;
                            // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                            sdv.RuleQty = Include.NoRuleQty;
                            sdv.RuleTypeQty = Include.NoRuleQty;
                            CalcQty = 0;
                            // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
						}
						else
						{
							//XferQty = Include.NoRuleQty; // MID Track 4298 Integer type showing fractional value
							XferQty = 0; // MID Track 4298 Integer type showing fractional value
							CalcQty = CalcMaxQty(Maximum);
							WillShip = CalcQty;
                            sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
						}
					//} 
					// end MID Track 4442 Ad Min Rule Qty not displayed in Velocity Store Detail
                   break;
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                //case eVelocityRuleType.AdMinimumBasis:
                //    // Begin TT # 91 - stodd
                //    XferQty = 0; 
                //    AdMinimum = ap.GetGradeAdMinimum(sdv.BasisGradeIDX);
                //    CalcQty = CalcAdMinBasisQty(AdMinimum); 
                //    WillShip = CalcQty;
                //    sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
                //    break;
                //    // End TT # 91 - stodd
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
				case eVelocityRuleType.AdMinimum:
					// begin MID Track 4442 Ad Min Rule Qty not displayed in Velocity Store Detail
					//if (Component.ComponentType == eComponentType.Total)
					//{
						//XferQty = Include.NoRuleQty; // MID Track 4298 Integer type showing fractional value
						XferQty = 0; // MID Track 4298 Integer type showing fractional value
						// BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                        //AdMinimum = ap.GetGradeAdMinimum(sdv.GradeIDX);
                     // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    if (_IBInventoryInd == 'A')
                    {
                    // END TT#1287 - AGallagher - Inventory Min/Max
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                AdMinimum = ap.GetGradeAdMinimum(sdv.GradeIDX);
                                break;
                            case 'S':
                                AdMinimum = ap.GetGradeAdMinimum(sdv.GradeIDX);
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    { AdMinimum = gll.AllocAdMin; }
                                }
                                break;
                        }
                    } // TT#1287 - AGallagher - Inventory Min/Max
                        // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                    // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                    // IBADMIN
                    if (_IBInventoryInd == 'I')
                    {
                        switch (_ApplyMinMaxInd)
                        {
                            case 'N':
                                AdMinimum = ap.GetGradeAdMinimum(sdv.GradeIDX);
                                break;
                            case 'S':
                                AdMinimum = ap.GetGradeAdMinimum(sdv.GradeIDX);
                                _ApplyCalcMax = ap.GetGradeAdMinimum(sdv.GradeIDX);

                                //_IBApplyCalcMin = _ApplyCalcMin - sdv.IBBasisOHandIT;
                                _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                if (_IBApplyCalcMax > 0.0)
                                {
                                    AdMinimum = (int)_IBApplyCalcMax;
                                }
                                else
                                {
                                    AdMinimum = 0;
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                                break;
                            case 'V':
                                // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                if (CalculateAverageUsingChain)
                                { _ApplyGrade = sdv.SGLChnVelocityGrade; }
                                else
                                { _ApplyGrade = sdv.SGLGrpVelocityGrade; }
                                // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                foreach (GradeLowLimit gll in _gradeLowLimData.Values)
                                {
                                    // BEGIN TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    //if (sdv.BasisGrade == gll.Grade)
                                    if (_ApplyGrade == gll.Grade)
                                    // END TT#864 - AGallagher - Velocity - Velocity Grade Min/Max - applying the incorrect Min/Max values on some stores
                                    {   AdMinimum = gll.AllocAdMin; 
                                        _ApplyCalcMax = gll.AllocAdMin; }
                                }
                                _IBApplyCalcMax = _ApplyCalcMax - sdv.IBBasisOHandIT;

                                if (_IBApplyCalcMax < 0.0)
                                {
                                    _IBApplyCalcMax = 0.0;
                                }

                                if (_IBApplyCalcMax > 0.0)
                                {
                                    AdMinimum = (int)_IBApplyCalcMax;
                                }
                                else
                                {
                                    AdMinimum = 0;
                                    WillShip = 0;
                                    RuleType = eVelocityRuleType.None;
                                    // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                    sdv.RuleQty = Include.NoRuleQty;
                                    sdv.RuleTypeQty = Include.NoRuleQty;
                                    CalcQty = 0;
                                    // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                                }
                                break;
                        }
                    }
                    // END TT#1287 - AGallagher - Inventory Min/Max

                    Minimum = AdMinimum;  // TT#1735-MD - JSmith - Velocity and Capacity
                    Maximum = int.MaxValue;  // TT#1735-MD - JSmith - Velocity and Capacity

						//XferQty = CalcXferQty(CalcQty, OnHand); // MID Track 4128 AD Min not displayed in Velocity
						CalcQty = CalcAdMinQty(AdMinimum); // MID Track 4128 AD Min not displayed in Velocity
						WillShip = CalcQty;
                        sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
					//}
					// end MID Track 4442 Ad Min Rule Qty not displayed in Velocity Store Detail
                    break;
				// ===================================================================
				// These rule types can only be applied to the header color components
				// ===================================================================
				case eVelocityRuleType.ColorMinimum:
					if (Component.ComponentType == eComponentType.SpecificColor)
					{
						//XferQty = Include.NoRuleQty;  // MID Track 4298 Integer type showing fractional value
						XferQty = 0; // MID track 4298 Integer type showing fractional value
						//ColorMinimum = ap.GetGradeColorMinimum(sdv.GradeIDX);  // MID Track 6193 Color Min/Max not working in velocity
						ColorMinimum = ap.GetStoreColorMinimum(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID); // MID Track 6193 Color Min/Max not working in velocity
						CalcQty = CalcColorMinQty(ColorMinimum);
						WillShip = CalcQty;
                        sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  

                        Minimum = ColorMinimum;  // TT#1735-MD - JSmith - Velocity and Capacity
                        Maximum = int.MaxValue;  // TT#1735-MD - JSmith - Velocity and Capacity
					}
					break;
				case eVelocityRuleType.ColorMaximum:
					if (Component.ComponentType == eComponentType.SpecificColor)
					{
						//ColorMaximum = ap.GetGradeColorMaximum(sdv.GradeIDX); // MID Track 6193 Color Min/Max not working in velocity
						ColorMaximum = ap.GetStoreColorMaximum(((AllocationColorOrSizeComponent)Component).ColorRID, sdv.StoreRID); // MID Track 6193 Color Min/Max not working in velocity
						if (ColorMaximum == Include.LargestIntegerMaximum)
						{
							// begin MID Track 4298 Integer type showing fractional value
                            // CalcQty = 0;  // TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
							XferQty = 0;
							WillShip = 0;
                            // sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  // TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process  
							//CalcQty = Include.NoRuleQty;
							//XferQty = Include.NoRuleQty;
							//WillShip = Include.NoRuleQty;
							// end MID track 4298 Integer type showing fractional value
							RuleType = eVelocityRuleType.None;
                            // BEGIN TT#2063 - AGallagher - Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
                            sdv.RuleQty = Include.NoRuleQty;
                            sdv.RuleTypeQty = Include.NoRuleQty;
                            CalcQty = 0;
                            // END TT#2063 - AGallagher -Velocity Store Detail Showing No Allocation On 1st Process, But Allocating on 2nd Process
						}
						else
						{
							//XferQty = Include.NoRuleQty; // MID Track 4298 Integer type showing fractional value
							XferQty = 0;
							CalcQty = CalcColorMaxQty(ColorMaximum);
							WillShip = CalcQty;
                            sdv.RuleTypeQty = 0;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)  
						}

                        Minimum = 0;  // TT#1735-MD - JSmith - Velocity and Capacity
                        Maximum = ColorMaximum;  // TT#1735-MD - JSmith - Velocity and Capacity
					}
					break;
			}

            WillShip = Math.Min(WillShip, Math.Min(sdv.Capacity, sdv.PrimaryMaximum)); // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity

            // Begin TT#1735-MD - JSmith - Velocity and Capacity
            if (WillShip > 0)
            {
                if (Minimum > 0 && WillShip < Minimum)
                {
                    WillShip = 0;
                    RuleType = eVelocityRuleType.None;
                    sdv.RuleQty = Include.NoRuleQty;
                    sdv.RuleTypeQty = Include.NoRuleQty;
                    CalcQty = 0;
                }

                if (Maximum > 0 && Maximum < int.MaxValue && WillShip > Maximum)
                {
                    WillShip = Maximum;
                }
            }

            //// Begin TT#4715 - JSmith - GA - Velocity - capacity stores not getting expected results
            //// Make sure value is between min and max

            //// Begin TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 
            //if (_ApplyCalcMin == 0)
            //{
            //    _ApplyCalcMin = ap.GetGradeMinimum(sdv.GradeIDX);
            //}
            //if (_ApplyCalcMax == 0)
            //{
            //    _ApplyCalcMax = ap.GetGradeMaximum(sdv.GradeIDX);
            //}
            //// End TT#1732-MD - GA - After velocity one or more headers are over allocated and one or more headers are under allocated. 

            //if (WillShip > 0 &&
            //    _ApplyMinMaxInd != 'N')
            //{
            //    if (_IBInventoryInd == 'A')
            //    {
            //        if (WillShip < _ApplyCalcMin)
            //        {
            //            WillShip = 0;
            //            RuleType = eVelocityRuleType.None;
            //            sdv.RuleQty = Include.NoRuleQty;
            //            sdv.RuleTypeQty = Include.NoRuleQty;
            //            CalcQty = 0;
            //        }
            //        if (WillShip > _ApplyCalcMax)
            //        { 
            //            WillShip = _ApplyCalcMax; 
            //        }
            //    }
            //    else if (_IBInventoryInd == 'I')
            //    {
            //        if (WillShip < _IBApplyCalcMin)
            //        {
            //            WillShip = 0;
            //            RuleType = eVelocityRuleType.None;
            //            sdv.RuleQty = Include.NoRuleQty;
            //            sdv.RuleTypeQty = Include.NoRuleQty;
            //            CalcQty = 0;
            //        }

            //        if (WillShip > _IBApplyCalcMax)
            //        { 
            //            WillShip = (int)_IBApplyCalcMax; 
            //        }
            //    }
            //}
            //// End TT#4715 - JSmith - GA - Velocity - capacity stores not getting expected results
            // End TT#1735-MD - JSmith - Velocity and Capacity

			if (aFromMatrix)
			{
				// ======================================
				// Called from SetStoreVelocityMatrixRule
				// ======================================
                //if (WillShip <= sdv.Capacity && WillShip <= sdv.PrimaryMaximum) // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity
                //{                                                                 // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity
					// begin MID Track 4060 Pack Components get allocated times their multiple
					if (Component.ComponentType == eComponentType.SpecificPack)
					{
						// BEGIN TT#616 - STodd - pack rounding
						// if only generic packs...
						if (ap.GenericPackCount > 0 && ap.NonGenericPackCount == 0 && ap.BulkColorCount == 0)
						{
							PackHdr ph = ap.GetPackHdr(((AllocationPackComponent)Component).PackName);
							if (ph.GenericPack)
							{
								genericPackRounding = ap.DoesGenericPackRoundingApply(ref fstPackRounding, ref nthPackRounding, Multiple);
								if (genericPackRounding)
								{
									int packsAllocated = (int)((double)WillShip / (double)ph.PackMultiple);
									int remainderUnits = (int)((double)WillShip % (double)ph.PackMultiple);
								    double remainder = (double)remainderUnits / (double)ph.PackMultiple;
								    if (packsAllocated == 0)
								    {
								        // Normal rounding applies
										if (fstPackRounding == Include.DefaultGenericPackRounding1stPackPct)
								        {
											// Do nothing. Let the allocation stand as is.
								        }
								        // Pack rounding applies
										else if (remainder >= fstPackRounding)
								        {
								            packsAllocated++;
											WillShip = packsAllocated * ph.PackMultiple;
								            //Check for over allocation
								            //while (unitsAllocated > unitsToSpread)
								            //{
								            //    unitsAllocated -= ph.PackMultiple;
								            //    packsAllocated--;
								            //}
											if (WillShip < 0 || packsAllocated < 0)
								            {
												WillShip = 0;
								                packsAllocated = 0;
								            }
								        }
								    }
									else if (packsAllocated >= 1)
									{
										// Normal rounding applies
										if (nthPackRounding == Include.DefaultGenericPackRoundingNthPackPct)
										{
											// Do nothing. Let the allocation stand as is.
										}
										// Pack rounding applies
										else if (remainder >= nthPackRounding)
										{
											packsAllocated++;
											WillShip = packsAllocated * ph.PackMultiple;
											// Check for over allocation
											//while (unitsAllocated > unitsToSpread)
											//{
											//    unitsAllocated -= ph.PackMultiple;
											//    packsAllocated--;
											//}
											if (WillShip < 0 || packsAllocated < 0)
											{
												WillShip = 0;
												packsAllocated = 0;
											}
										}
									}
								}
							}
						}
						// END TT#616 - STodd - pack rounding

						sdv.RuleQty = (double)((int)(CalcQty / Multiple)); // MID Track Integer type showing fractional value
						sdv.Transfer = XferQty / Multiple;
						sdv.WillShip = WillShip / Multiple;
					}
					else
					{
						// end MID Track 4060 Pack Components get allocated times their multiple
						sdv.RuleQty = CalcQty;
						sdv.Transfer = XferQty;
						sdv.WillShip = WillShip;
					}   // MID Track 4060 Pack Components get allocated times their multiple
					sdv.RuleType = RuleType;
					sdv.ReCalcWillShip = false;

					if (RuleType != eVelocityRuleType.None)
					{
						if (aGrpLvlRID == Include.TotalMatrixLevelRID)
						{
							sdv.TotRule = true;
							sdv.SGLRule = false;
						}
						else
						{
							sdv.SGLRule = true;
							sdv.TotRule = false;
						}
					}
					else
					{
						sdv.SGLRule = false;
						sdv.TotRule = false;
					}
                //} // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity
			}
			else
			{
				// ===============================
				// Called from UpdateStoreWillShip
				// ===============================
                //if (WillShip <= sdv.Capacity && WillShip <= sdv.PrimaryMaximum) // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity
                //{                                                               // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity
					sdv.SGLRule = false;
					sdv.TotRule = false;
					// begin MID Track 4060 Pack Components get allocated times their multiple
					if (Component.ComponentType == eComponentType.SpecificPack)
					{
                        sdv.RuleQty = (double)((int)(CalcQty / Multiple)); // MID Track 4298 integer type showing fractional value
                        sdv.Transfer = XferQty / Multiple;
						sdv.WillShip = WillShip / Multiple;
					}
					else
					{
						// end MID Track 4060 Pack Components get allocated times their multiple
                        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug  
						// sdv.RuleQty = (double)((int)(CalcQty / Multiple)); // MID Track 4298 integer type showing fractional value
                        sdv.RuleQty = CalcQty;
                        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug
						sdv.Transfer = XferQty;
						sdv.WillShip = WillShip;
					}   // MID Track 4060 Pack Components get allocated times their multiple
					sdv.RuleType = RuleType;
					sdv.ReCalcWillShip = false;

					if (RuleType != eVelocityRuleType.None)
					{
						sdv.UserRule = true;
					}
					else
					{
						sdv.UserRule = false;
					}
                //} // TT#4337 - MD - GA Velocity Rule Rejected due to Capacity
			}

            // Begin TT#1711-MD - stodd - remove check against allocation maximum.
            // Commented out TT#4210 changes.
            // begin TT#4210 - MD - Jellis - Qty Allocated cannot be negative
            // NOTE:  For a group allocation, there is no mechanism to "override" the group min/max values and associated Header Min/Max values
            //        In other words, these min/max values are "always" observed.  Therefore, the following code forces all will ship values to observe these values
            //if (_alocProfile is AssortmentProfile
            //    && _alocProfile.AsrtType == (int)eAssortmentType.GroupAllocation)
            //{
            //    sdv.WillShip = Math.Min(sdv.WillShip, _alocProfile.GetStoreMaximum(eAllocationSummaryNode.Total, sdv.StoreRID, false));
            //}
            // end TT#4210 - MD - Jellis - Qty Allocated cannot be negative
            // End TT#1711-MD - stodd - remove check against allocation maximum.

		}

        //Begin TT#855-MD -jsobek -Velocity Enhancements -unused function
        //private bool DoPlanAndBasisGradesMatch()
        //{
        //    bool match = false;
        //    //bool _velocityGradesCompared = false;
        //    //bool _velocityGradesMatch = false;

        //    //AllocationSubtotalProfile asp = AST.GetAllocationGrandTotalProfile();
        //    //asp.GradeList
        //    //_lowLimSortedGradeData
        //    return match;
        //}
        //End TT#855-MD -jsobek -Velocity Enhancements -unused function

		/// <summary>
		/// Calculate store rule quantity based on an absolute value
		/// </summary>
		/// <param name="aRuleQty">Rule Absolute Value</param>
		/// <returns>Calculated Absolute Quantity</returns>
		/// <remarks>Calculates the actual absolute quantity based on an absolute value</remarks>
		public int CalcAbsQty(double aRuleQty) // MID track 4298 Integer type showing fractional value
		{
			//double AbsoluteQty = Include.NoRuleQty; // MID track 4298 Integer type showing fractional value

			int AbsoluteQty = (int)aRuleQty; // MID track 4298 Integer type showing fractional value

			return AbsoluteQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on a ship up to value
		/// </summary>
		/// <param name="aRuleQty">Rule Ship Up To Value</param>
		/// <returns>Calculated Ship Up To Quantity</returns>
		/// <remarks>Calculates the actual ship up to quantity based on a ship up to value</remarks>
		public int CalcShipUpToQty(double aRuleQty)
		{
			//double ShipUpToQty = Include.NoRuleQty; // MID track 4298 Integer type showing fractional value

			int ShipUpToQty = (int)aRuleQty; // MID Track 4298 Integer type showing fractional value

			return ShipUpToQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on weeks of supply value
		/// </summary>
		/// <param name="aAvgWeeklySales">Average Weekly Sales</param>
		/// <param name="aRuleQty">Rule Weeks of Supply Value</param>
		/// <returns>Calculated Weeks of Supply Quantity</returns>
		/// <remarks>Calculates the actual weeks of supply quantity based on a weeks of supply value</remarks>
		public int CalcWOSQty(double aAvgWeeklySales, double aRuleQty) // MID track 4298 integer type showing fractional value
		{
			//double WeeksOfSupplyQty = Include.NoRuleQty;

			int WeeksOfSupplyQty = (int)(aAvgWeeklySales * aRuleQty + .5d); // MID Track 4298 integer type showing fractional value 

			return WeeksOfSupplyQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on forward weeks of supply value
		/// </summary>
		/// <param name="aStoreRID">Store RID</param>
		/// <param name="aRuleQty">Rule Forward Weeks of Supply Value</param>
		/// <returns>Calculated Forward Weeks of Supply Quantity</returns>
		/// <remarks>Calculates the actual forward weeks of supply quantity based on a forward weeks of supply value</remarks>
		public int CalcFwdWOSQty(int aStoreRID, double aRuleQty) // MID Track 4298 integer type showing fractional value
		{
			DayProfile FwdWOSBeginDay = null;
			DayProfile FwdWOSTargetDay = null;

			AllocationSubtotalProfile asp = null;

			//double FwdWeeksOfSupplyQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value

			DateTime AdjustedBeginDay = Include.UndefinedDate;

			asp = AST.GetAllocationGrandTotalProfile();

			FwdWOSBeginDay = asp.OnHandDayProfile;

			AdjustedBeginDay = FwdWOSBeginDay.Date.AddDays(aRuleQty * 7.0);

			FwdWOSTargetDay = AST.SAB.ApplicationServerSession.Calendar.GetDay(AdjustedBeginDay);

			// BEGIN Issue 4778 stodd 10.19.2007
			int FwdWeeksOfSupplyQty = AST.GetStoreOTSSalesPlan(
                aStoreRID, 
                this._alocProfile.PlanHnRID,
                this._alocProfile.GetCubeEligibilityNode(),
                FwdWOSBeginDay, 
                FwdWOSTargetDay, 
                asp.PlanFactor); // MID Track 4298 integer type showing fractional value
			// END Issue 4778

			return FwdWeeksOfSupplyQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on grade minimum value
		/// </summary>
		/// <param name="aRuleQty">Grade Minimum Value</param>
		/// <returns>Calculated Grade Minimum Quantity</returns>
		/// <remarks>Calculates the actual grade minimum quantity based on a grade minimum value</remarks>
		public int CalcMinQty(double aRuleQty) // MID Track 4298 integer type showing fractional value
		{
			//double MinQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value

			int MinQty = (int)aRuleQty; // MID Track 4298 integer type showing fractional value

			return MinQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on grade maximum value
		/// </summary>
		/// <param name="aRuleQty">Grade Maximum Value</param>
		/// <returns>Calculated Grade Maximum Quantity</returns>
		/// <remarks>Calculates the actual grade maximum quantity based on a grade maximum value</remarks>
		public int CalcMaxQty(double aRuleQty) // MID track 4298 integer type showing fractional value
		{
			//double MaxQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value

			int MaxQty = (int)aRuleQty; // MID track 4298 integer type showing fractional value

			return MaxQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on grade ad minimum value
		/// </summary>
		/// <param name="aRuleQty">Grade Ad Minimum Value</param>
		/// <returns>Calculated Grade Ad Minimum Quantity</returns>
		/// <remarks>Calculates the actual grade ad minimum quantity based on a grade ad minimum value</remarks>
		public int CalcAdMinQty(double aRuleQty) // MID track 4298 integer type showing fractional value
		{
			//double AdMinQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value

			int AdMinQty = (int)aRuleQty; // MID track 4298 integer type showing fractional value

			return AdMinQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on color minimum value
		/// </summary>
		/// <param name="aRuleQty">Color Minimum Value</param>
		/// <returns>Calculated Color Minimum Quantity</returns>
		/// <remarks>Calculates the actual color minimum quantity based on a color minimum value</remarks>
		public int CalcColorMinQty(double aRuleQty) // MID track 4298 integer type showing fractional value
		{
			//double ColorMinQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value

			int ColorMinQty = (int)aRuleQty; // MID track 4298 integer type showing fractional value

			return ColorMinQty;
		}

		/// <summary>
		/// Calculate a store rule quantity based on color maximum value
		/// </summary>
		/// <param name="aRuleQty">Color Maximum Value</param>
		/// <returns>Calculated Color Maximum Quantity</returns>
		/// <remarks>Calculates the actual color maximum quantity based on a color maximum value</remarks>
		public int CalcColorMaxQty(double aRuleQty) // MID track 4298 integer type showing fractional value 
		{
			//double ColorMaxQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value

			int ColorMaxQty = (int)aRuleQty; // MID track 4298 integer type showing fractional value

			return ColorMaxQty;
		}

		/// <summary>
		/// Calculate a store transfer quantity
		/// </summary>
		/// <param name="aQty">Calculated Rule Quantity</param>
		/// <param name="aOnHand">Store OnHand and Intransit</param>
		/// <param name="aPreSizeAllocated">Store units preallocated by size</param>
		/// <returns>Calculated Transfer Quantity</returns>
		/// <remarks>Calculates the transfer quantity based on the rule quantity and onhand</remarks>
		// begin MID Track 4282 Velocity overlays Fill Size Holes allocation
		//public int CalcXferQty(double aQty, double aOnHand) // MID track 4298 integer type showing fractional value
	    public int CalcXferQty(double aQty, double aOnHand, int aPreSizeAllocated)
			// end MID Track 4282 Velocity overlays Fill Sizde Holes Allocation
		{
			//double XferQty = Include.NoRuleQty; // MID track 4298 integer type showing fractional value
            int XferQty; // MID track 4298 integer type showing fractional value
			if (aOnHand > Include.NoRuleQty)
			{
				XferQty = (int)(aQty - aOnHand); // MID track 4298 integer type showing fractional value
			}
			else
			{
				XferQty = (int)aQty; // MID track 4298 integer type showing fractional value
			}
            XferQty -= aPreSizeAllocated; // MID Track 4282 Velocity overlays Fill Size Holes allocation
			return XferQty;
		}

		/// <summary>
		/// Calculate a store will ship quantity
		/// </summary>
		/// <param name="aQty">Calculated Rule Quantity</param>
		/// <param name="aOnHand">Store OnHand and Intransit</param>
		/// <param name="aMultiple">Component Multiple</param>
		/// <param name="aPreSizeAllocated">Store units preallocated by size</param>
		/// <returns>Calculated Will Ship Quantity</returns>
		/// <remarks>Calculates the will ship quantity based on the onhand and the component multiple</remarks>
		// begin MID Track 4282 Velocity overlays Fill Size Holes allocation
		//public int CalcWillShipQty(int aQty, double aOnHand, int aMultiple) // MID track 4298 integer type showing fractional value
	    public int CalcWillShipQty(int aQty, double aOnHand, int aMultiple, int aPreSizeAllocated)
			// end MID Track 4282 Velocity overlays Fill Size Holes allocation
		{
			// begin MID track 4298 Integer type showing fractional value
			//double AvailQty = Include.NoRuleQty;
			//double WillShip = Include.NoRuleQty;
			//double AdjustedQty = Include.NoRuleQty;
			int AvailQty;
			int WillShip;
			int AdjustedQty;
			// end MID track 4298 Integer type showing fractional value

			if (aOnHand > Include.NoRuleQty)
			{
				AvailQty = (int)((double)aQty - aOnHand);
			}
			else
			{
				AvailQty = aQty;
			}
            AvailQty -= aPreSizeAllocated; // MID Track 4282 Velocity overlays Fill Size Holes allocation
			//AdjustedQty = Math.Round((AvailQty / (double)aMultiple), 0); // MID track 4298 integer type showing  fractional value
			AdjustedQty = (int)((double)AvailQty / (double)aMultiple); // MID track 4298 integer type showing fractional value

			WillShip = AdjustedQty * aMultiple;

			// MID track 4298 integer type showing fractional value
			if (WillShip < 0)
			{
				WillShip = 0;
			}
			//if (WillShip < Include.NoRuleQty)
			//{
			//	WillShip = Include.NoRuleQty;
			//}
			// end MID track 4298 integer type showing fractional value

			return WillShip;
		}

		// Begin TT # 91 - stodd
		public int CalcMinBasisQty(double aRuleQty) 
		{
			int MinBasisQty = (int)aRuleQty;
			return MinBasisQty;
		}

		public int CalcMaxBasisQty(double aRuleQty) 
		{
			int MaxBasisQty = (int)aRuleQty; 
			return MaxBasisQty;
		}

		public int CalcAdMinBasisQty(double aRuleQty) 
		{
			int AdMinBasisQty = (int)aRuleQty; 
			return AdMinBasisQty;
		}
		// End TT # 91


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
			VelocityMethod newVelocityMethod = null;
			int maxRows;
			int basisDateRangeRid;

			try
			{
				newVelocityMethod = (VelocityMethod)this.MemberwiseClone();
				newVelocityMethod.CalculateAverageUsingChain = CalculateAverageUsingChain;
				newVelocityMethod.Component = Component;
				newVelocityMethod.DetermineShipQtyUsingBasis = DetermineShipQtyUsingBasis;
                // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                newVelocityMethod.ApplyMinMaxInd = ApplyMinMaxInd;
                // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
                newVelocityMethod.InventoryInd = InventoryInd;  // TT#1287 - AGallagher - Inventory Min/Max
				newVelocityMethod.DSVelocity = DSVelocity.Copy();
				// BEGIN Issue 4818
				if (aCloneDateRanges)
				{
					maxRows = newVelocityMethod.DSVelocity.Tables["Basis"].Rows.Count;  
					for (int row=0;row<maxRows;row++)
					{
						basisDateRangeRid = Convert.ToInt32(DSVelocity.Tables["Basis"].Rows[row]["cdrRID"], CultureInfo.CurrentUICulture);
						newVelocityMethod.DSVelocity.Tables["Basis"].Rows[row]["cdrRID"] = aSession.Calendar.GetDateRangeClone(basisDateRangeRid).Key;
					}
				}
				// END Issue 4818
				newVelocityMethod.IsInteractive = IsInteractive;
				newVelocityMethod.Method_Change_Type = eChangeType.none;
				newVelocityMethod.Method_Description = Method_Description;
				newVelocityMethod.MethodStatus = MethodStatus;
				newVelocityMethod.Name = Name;
				if (aCloneDateRanges &&
					OTS_Begin_CDR_RID != Include.UndefinedCalendarDateRange)
				{
					newVelocityMethod.OTS_Begin_CDR_RID = aSession.Calendar.GetDateRangeClone(OTS_Begin_CDR_RID).Key;
				}
				else
				{
					newVelocityMethod.OTS_Begin_CDR_RID = OTS_Begin_CDR_RID;
				}
				if (aCloneDateRanges &&
					OTS_Ship_To_CDR_RID != Include.UndefinedCalendarDateRange)
				{
					newVelocityMethod.OTS_Ship_To_CDR_RID = aSession.Calendar.GetDateRangeClone(OTS_Ship_To_CDR_RID).Key;
				}
				else
				{
					newVelocityMethod.OTS_Ship_To_CDR_RID = OTS_Ship_To_CDR_RID;
				}
				newVelocityMethod.OTSPlanHNRID = OTSPlanHNRID;
				newVelocityMethod.OTSPlanPHLSeq = OTSPlanPHLSeq;
				newVelocityMethod.OTSPlanPHRID = OTSPlanPHRID;
                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                newVelocityMethod.MERCH_HN_RID = MERCH_HN_RID;
                newVelocityMethod.MERCH_PHL_SEQ = MERCH_PHL_SEQ;
                newVelocityMethod.MERCH_PH_RID = MERCH_PH_RID;
                // END TT#1287 - AGallagher - Inventory Min/Max
				newVelocityMethod.SG_RID = SG_RID;
				newVelocityMethod.StoreGroupRID = StoreGroupRID;
				newVelocityMethod.TrendPctContribution = TrendPctContribution;
				newVelocityMethod.User_RID = User_RID;
				newVelocityMethod.UseSimilarStoreHistory = UseSimilarStoreHistory;
				newVelocityMethod.Virtual_IND = Virtual_IND;
                newVelocityMethod.Template_IND = Template_IND;

                newVelocityMethod.LoadDataArrays();
//				CopyDataArrays(newVelocityMethod, aSession, aCloneDateRanges);

				return newVelocityMethod;
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
			return true;
		}
		// End MID Track 4858

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;
            VersionSecurityProfile versionSecurity;

            DataTable dtBasis = DSVelocity.Tables["Basis"];
            int basisHnRID, basisFVRID;
            foreach (DataRow dr in dtBasis.Rows)
            {
                basisFVRID = Convert.ToInt32(dr["BasisFVRID"], CultureInfo.CurrentUICulture);
                versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(basisFVRID, (int)eSecurityTypes.Store);
                if (!versionSecurity.AllowView)
                {
                    return false;
                }

                basisHnRID = Convert.ToInt32(dr["BasisHNRID"], CultureInfo.CurrentUICulture);
                if (basisHnRID != Include.NoRID)
                {
                    hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(basisHnRID, (int)eSecurityTypes.Store);
                    if (!hierNodeSecurity.AllowView)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        // BEGIN RO-3890 - JSmith

        private System.Data.DataTable _merchandiseDataTable;
        private HierarchyProfile _hp = null;
        private int _currentAttributeSet = Include.NoRID;
        private DataTable _compDataTable = null;
        private bool _needToLockHeaders = false;
        private bool _statisticsCalculated = false;
        private bool _updateFromStoreDetail = false;
        private bool _clearMatrix = false;
        private bool _matrixProcessed = false;
        private bool _matrixEverProcessed = false;
        int _velocityGradesMerchandiseKey = 0;
        bool _populateVelocityGrades = false;
        bool _attributeChanged = false;

        private HierarchyProfile HP
        {
            get
            {
                if (_hp == null)
                {
                    _hp = SAB.HierarchyServerSession.GetMainHierarchyData();
                }
                return _hp;
            }
        }

        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalVelocity);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserVelocity);
            }

        }

        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;
            if (!_dataLoaded)
            {
                _basisChangesMade = true;
                _dataLoaded = true;
            }

            BuildDataTables();

            eVelocityCalculateAverageUsing calculateAverageUsing;
            if (_CalculateAverageUsingChain)
            {
                calculateAverageUsing = eVelocityCalculateAverageUsing.AllStores;
            }
            else
            {
                calculateAverageUsing = eVelocityCalculateAverageUsing.AttributeSetAverage;
            }

            eVelocityDetermineShipQtyUsing determineShipQtyUsing;
            if (_DetermineShipQtyUsingBasis)
            {
                determineShipQtyUsing = eVelocityDetermineShipQtyUsing.Basis;
            }
            else
            {
                determineShipQtyUsing = eVelocityDetermineShipQtyUsing.Header;
            }

            eVelocityApplyMinMaxType applyMinMaxType = eVelocityApplyMinMaxType.None;
            switch (ApplyMinMaxInd)
            {
                case 'N':
                    applyMinMaxType = eVelocityApplyMinMaxType.None;
                    break;
                case 'S':
                    applyMinMaxType = eVelocityApplyMinMaxType.StoreGrade;
                    break;
                case 'V':
                    applyMinMaxType = eVelocityApplyMinMaxType.VelocityGrade;
                    break;
            }
            bool balanceToHeader = false;
            if (BalanceToHeaderInd == '1')
            {
                balanceToHeader = true;
            }
            else
            {
                balanceToHeader = false;
            }

            if (MERCH_HN_RID != Include.NoRID)
            {
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(MERCH_HN_RID, true, true);
                AddNodeToMerchandise(hnp);
            }

            eMinMaxType minMaxType;
            if (InventoryInd == 'I')
            {
                minMaxType = eMinMaxType.Inventory;
            }
            else
            {
                minMaxType = eMinMaxType.Allocation;
            }
            eMerchandiseType merchandiseType = eMerchandiseType.Undefined;

            if (MERCH_HN_RID != Include.NoRID)
            {
                merchandiseType = eMerchandiseType.Node;
            }
            else
            {
                if (MERCH_PH_RID != Include.NoRID)
                {
                    merchandiseType = eMerchandiseType.HierarchyLevel;
                }
                else
                {
                    merchandiseType = eMerchandiseType.OTSPlanLevel;
                }
            }

            ProfileList setList = StoreMgmt.StoreGroup_GetLevelListViewList(_SG_RID);
            if (_currentAttributeSet == Include.NoRID)
            {
                _currentAttributeSet = ((StoreGroupLevelListViewProfile)setList.ArrayList[0]).Key;
            }

            KeyValuePair<int, string> attributeSet;
            if (_currentAttributeSet == Include.TotalMatrixLevelRID)
            {
                attributeSet = new KeyValuePair<int, string>(Include.TotalMatrixLevelRID, MIDText.GetTextOnly(eMIDTextCode.lbl_TotalMatrix));
            }
            else
            {
                attributeSet = GetName.GetAttributeSetName(key: _currentAttributeSet);
            }

            if (GradeVariableType != eVelocityMethodGradeVariableType.Stock)
            {
                GradeVariableType = eVelocityMethodGradeVariableType.Sales;
            }

            ROMethodAllocationVelocityProperties method = new ROMethodAllocationVelocityProperties(
                method: GetName.GetMethod(method: this),
                description: Method_Description,
                userKey: User_RID,
                calculateAverageUsing: EnumTools.VerifyEnumValue(calculateAverageUsing),
                determineShipQtyUsing: EnumTools.VerifyEnumValue(determineShipQtyUsing),
                applyMinMaxType: EnumTools.VerifyEnumValue(applyMinMaxType),
                gradeVariableType: EnumTools.VerifyEnumValue(_gradeVariableType),
                useSimilarStoreHistory: _UseSimilarStoreHistory,
                balance: _balance,
                balanceToHeader: balanceToHeader,
                reconcile: _reconcile,
                inventoryIndicator: minMaxType,
                inventoryMinMaxMerchType: merchandiseType,
                inventoryMinMaxMerchandise: GetName.GetLevelKeyValuePair(merchandiseType: EnumTools.VerifyEnumValue(merchandiseType),
                                                                      nodeRID: MERCH_HN_RID,
                                                                      merchPhRID: MERCH_PH_RID,
                                                                      merchPhlSequence: MERCH_PHL_SEQ,
                                                                      SAB: SAB),

                inventoryMinMaxMerchandiseHierarchy: new KeyValuePair<int, int>(MERCH_PH_RID, MERCH_PHL_SEQ),
                attribute: GetName.GetAttributeName(key: _SG_RID),
                attributeSet: attributeSet,
                isTemplate: Template_IND
                );

            foreach (StoreGroupLevelListViewProfile attrSet in setList)
            {
                method.AttributeSetList.Add(new KeyValuePair<int, string>(attrSet.Key, attrSet.Name));
            }
            // add total matrix
            string totalMatrixText = MIDText.GetTextOnly(eMIDTextCode.lbl_TotalMatrix);
            method.AttributeSetList.Add(new KeyValuePair<int, string>(Include.TotalMatrixLevelRID, totalMatrixText));

            // matrix is being reset based on different merchandise selected to build the velocity grades
            if (_populateVelocityGrades)
            {
                VelocityGrades_InitialPopulate(
                    velocityGradesMerchandiseKey: _velocityGradesMerchandiseKey,
                    setList: setList
                    );
                _populateVelocityGrades = false;
                _attributeChanged = false;
            }

            if (_velocityGradesMerchandiseKey != Include.NoRID)
            {
                method.VelocityGradesMerchandise = GetName.GetMerchandiseName(nodeRID: _velocityGradesMerchandiseKey,
                    SAB: SAB);
            }


            AddBasis(method: method);
            AddMerchandiseList(method: method);
            AddVelocityGrades(method: method);
            AddSellThruValues(method: method);
            AddNoOnhandRules(method: method);
            AddOnhandRules(method: method);
            AddMatrixRules(method: method);
            AddMatrixAttributeSets(method: method);
            GetMatrixViews(method: method);
            GetMatrixViewColumns(method: method);
            
            if (IsInteractive)
            {
                if (!SetupInteractive(method: method, message: ref message))
                {
                    successful = false;
                }
            }
            method.Interactive = IsInteractive;

            AST.VelocityStyleReviewLastDisplayed = false;

            return method;
        }

        /// <summary>
        /// Populates velocity grade values when the Merchandise Node changes
        /// </summary>
        /// <param name="velocityGradesMerchandiseKey">The merchandise key to use to retrieve velocity grades</param>
        /// <param name="setList">List of attribute sets</param>
        private void VelocityGrades_InitialPopulate(
            int velocityGradesMerchandiseKey,
             ProfileList setList)
        {
            VelocityGradeList velocityGradeList = null;
            SellThruPctList sellThruPctList = null;

            try
            {
                if (velocityGradesMerchandiseKey > 0)
                {
                    // build velocity grades
                    int count = 0;

                    _dsVelocity.Tables["VelocityGrade"].Clear();
                    _dsVelocity.Tables["VelocityGrade"].AcceptChanges();

                    if (velocityGradesMerchandiseKey != Include.NoRID)
                    {
                        velocityGradeList = SAB.HierarchyServerSession.GetVelocityGradeList(velocityGradesMerchandiseKey, true);
                    }
                    else // no merchandise, so returned cleared grades
                    {
                        return;
                    }

                    foreach (VelocityGradeProfile vgp in velocityGradeList)
                    {
                        _dsVelocity.Tables["VelocityGrade"].Rows.Add(new object[] { count, vgp.VelocityGrade, vgp.Boundary, vgp.VelocityMinStock, vgp.VelocityMaxStock, vgp.VelocityMinAd });
                        ++count;
                    }

                    foreach (DataRow velocityGradeRow in _dsVelocity.Tables["VelocityGrade"].Rows)
                    {
                        if (Convert.ToInt32(velocityGradeRow["MinStock"]) == -1)
                        {
                            velocityGradeRow["MinStock"] = DBNull.Value;
                        }
                        if (Convert.ToInt32(velocityGradeRow["MaxStock"]) == -1)
                        {
                            velocityGradeRow["MaxStock"] = DBNull.Value;
                        }
                        if (Convert.ToInt32(velocityGradeRow["MinAd"]) == -1)
                        {
                            velocityGradeRow["MinAd"] = DBNull.Value;
                        }
                    }

                    _dsVelocity.Tables["VelocityGrade"].DefaultView.RowFilter = null;

                    // build sell thru percentages
                    count = 0;

                    _dsVelocity.Tables["SellThru"].Clear();
                    _dsVelocity.Tables["SellThru"].AcceptChanges();

                    sellThruPctList = SAB.HierarchyServerSession.GetSellThruPctList(velocityGradesMerchandiseKey, false);
                    foreach (SellThruPctProfile stpp in sellThruPctList)
                    {
                        _dsVelocity.Tables["SellThru"].Rows.Add(new object[] { count, stpp.SellThruPct });
                        ++count;
                    }
                }

                // build attribute sets
                DataRow setRow = null;

                if (_attributeChanged)
                {
                    _dsVelocity.Tables["GroupLevel"].Clear();
                    _dsVelocity.Tables["GroupLevel"].AcceptChanges();

                    foreach (StoreGroupLevelListViewProfile attributeSet in setList)
                    {
                        setRow = _dsVelocity.Tables["GroupLevel"].NewRow();
                        setRow["SglRID"] = attributeSet.Key;
                        setRow["NoOnHandRule"] = (int)eVelocityRuleType.None;
                        setRow["NoOnHandQty"] = System.DBNull.Value;
                        setRow["ModeInd"] = 'N';
                        setRow["AverageRule"] = (int)eVelocityRuleType.None;
                        setRow["AverageQty"] = System.DBNull.Value;
                        setRow["SpreadInd"] = 'S';
                        _dsVelocity.Tables["GroupLevel"].Rows.Add(setRow);
                    }
                }

                // reload data with new grades and sell thru percents
                LoadDataArrays();

            }
            catch
            {
                throw;
            }
        }

        private void AddBasis (ROMethodAllocationVelocityProperties method)
        {
            HierarchyNodeProfile hnp;
            ROBasisWithLevelDetailProfile basisDetailProfile;
            eMerchandiseType merchandiseType = eMerchandiseType.Undefined;
            string merchandiseText = null;
            int basisID = Include.Undefined;
            int iMerchandiseId = Include.Undefined;
            int iVersionId = Include.Undefined;
            int merchandisePhRid = Include.Undefined;
            int merchPhlSequence = Include.Undefined;
            int merchOffset = Include.Undefined;
            string sVersion = null;
            int iDateRangeID = Include.Undefined;
            string sDateRange = null;

            KeyValuePair<int, string> workKVP;

            foreach (DataRow row in _dsVelocity.Tables["Basis"].Rows)
            {
                merchandiseType = eMerchandiseType.Undefined;
                merchandiseText = null;
                merchandisePhRid = Include.Undefined;
                merchPhlSequence = Include.Undefined;
                merchOffset = Include.Undefined;
                iMerchandiseId = Include.Undefined;
                iVersionId = Include.Undefined;
                sVersion = null;
                iDateRangeID = Include.Undefined;

                basisID = (int)row["BasisSequence"];
                iMerchandiseId = (int)row["BasisHNRID"];
                merchandisePhRid = (int)row["BasisPHRID"];
                merchPhlSequence = (int)row["BasisPHLSequence"];

                if (iMerchandiseId != Include.NoRID)
                {
                    hnp = SAB.HierarchyServerSession.GetNodeData(iMerchandiseId, true, true);
                    AddNodeToMerchandise(hnp);
                }

                if (iMerchandiseId != Include.NoRID)
                {
                    for (int i = 0; i < _merchandiseDataTable.Rows.Count; i++)
                    {
                        DataRow listRow = _merchandiseDataTable.Rows[i];
                        if ((int)listRow["key"] == iMerchandiseId)
                        {
                            merchandiseText = Convert.ToString(listRow["text"]);
                            merchandiseType = eMerchandiseType.Node;
                        }
                    }
                }
                else if (merchandisePhRid != Include.NoRID)
                {
                    merchandiseText = GetMerchandiseText(merchPhlSequence);
                    merchandiseType = eMerchandiseType.HierarchyLevel;
                }
                else
                {
                    merchandiseText = GetMerchandiseText(0);
                    merchandiseType = eMerchandiseType.OTSPlanLevel;
                }

                iVersionId = (int)row["BasisFVRID"];
                workKVP = GetName.GetVersion(iVersionId, SAB);
                sVersion = workKVP.Value;
                iDateRangeID = (int)row["cdrRID"];

                if (iDateRangeID != Include.UndefinedCalendarDateRange)
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, iDateRangeID);
                }
                else
                {
                    workKVP = GetName.GetCalendarDateRange(iDateRangeID, SAB, SAB.ClientServerSession.Calendar.CurrentDate);
                }

                sDateRange = workKVP.Value;

                float fWeight = Convert.ToSingle(row["Weight"]);

                basisDetailProfile = new ROBasisWithLevelDetailProfile(
                    iBasisId: basisID,
                    iMerchandiseId: iMerchandiseId,
                    sMerchandise: merchandiseText,
                    iVersionId: iVersionId,
                    sVersion: sVersion,
                    iDaterangeId: iDateRangeID,
                    sDateRange: sDateRange,
                    sPicture: string.Empty,
                    fWeight: fWeight, 
                    bIsIncluded: true, 
                    sIncludeButton: string.Empty,
                    merchandiseType: EnumTools.VerifyEnumValue(merchandiseType),
                    iMerchPhRId: merchandisePhRid,
                    iMerchPhlSequence: merchPhlSequence,
                    iMerchOffset: merchOffset
                    );

                method.BasisProfiles.Add(basisDetailProfile);

                
            }

            // add list of versions for basis drop down
            ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
            foreach (VersionProfile versionProfile in versionProfList)
            {
                method.BasisVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

        }

        private string GetMerchandiseText(int seq)
        {
            try
            {
                string merchandise = null;
                DataRow myDataRow;
                for (int levIndex = 0;
                    levIndex < _merchandiseDataTable.Rows.Count; levIndex++)
                {
                    myDataRow = _merchandiseDataTable.Rows[levIndex];
                    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                    {
                        merchandise = Convert.ToString(myDataRow["text"]);
                        break;
                    }
                }

                return merchandise;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void AddNodeToMerchandise(HierarchyNodeProfile hnp)
        {
            bool basisNodeInList;
            try
            {
                DataRow row;
                basisNodeInList = false;
                int nodeRID = Include.NoRID;
                for (int levIndex = 0;
                    levIndex < _merchandiseDataTable.Rows.Count; levIndex++)
                {
                    row = _merchandiseDataTable.Rows[levIndex];
                    if ((eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
                    {
                        nodeRID = (Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture));
                        if (hnp.Key == nodeRID)
                        {
                            basisNodeInList = true;
                            break;
                        }
                    }
                }
                if (!basisNodeInList)
                {
                    row = _merchandiseDataTable.NewRow();
                    row["seqno"] = _merchandiseDataTable.Rows.Count;
                    row["leveltypename"] = eMerchandiseType.Node;
                    row["text"] = hnp.Text;
                    row["key"] = hnp.Key;
                    _merchandiseDataTable.Rows.Add(row);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BuildDataTables()
        {
            try
            {
                _merchandiseDataTable = MIDEnvironment.CreateDataTable();
                DataColumn myDataColumn;
                DataRow myDataRow;
                // Create new DataColumn, set DataType, ColumnName and add to DataTable.  
                // Level sequence number
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "seqno";
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = true;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                // Create second column - enum name.
                //Create Merchandise types - eMerchandiseType
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "leveltypename";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                // Create third column - text
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.String");
                myDataColumn.ColumnName = "text";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                // Create fourth column - Key
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "key";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchandiseDataTable.Columns.Add(myDataColumn);

                //Default Selection to OTSPlanLevel
                myDataRow = _merchandiseDataTable.NewRow();
                myDataRow["seqno"] = 0;
                myDataRow["leveltypename"] = eMerchandiseType.OTSPlanLevel;
                myDataRow["text"] = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
                myDataRow["key"] = Include.Undefined;
                _merchandiseDataTable.Rows.Add(myDataRow);

                for (int levelIndex = 1; levelIndex <= HP.HierarchyLevels.Count; levelIndex++)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)HP.HierarchyLevels[levelIndex];

                    if (hlp.LevelType != eHierarchyLevelType.Size)
                    {
                        myDataRow = _merchandiseDataTable.NewRow();
                        myDataRow["seqno"] = hlp.Level;
                        myDataRow["leveltypename"] = eMerchandiseType.HierarchyLevel;
                        myDataRow["text"] = hlp.LevelID;
                        myDataRow["key"] = hlp.Key;
                        _merchandiseDataTable.Rows.Add(myDataRow);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddMerchandiseList(ROMethodAllocationVelocityProperties method)
        {
            ROMerchandiseListEntry merchandiseListEntry;
            eMerchandiseType merchandiseType = eMerchandiseType.Undefined;
            int sequenceNumber = Include.Undefined;
            string text = null;
            int key = Include.Undefined;

            foreach (DataRow row in _merchandiseDataTable.Rows)
            {
                sequenceNumber = (int)row["seqno"];
                merchandiseType = (eMerchandiseType)(int)row["leveltypename"];
                text = Convert.ToString(row["text"]);
                key = (int)row["key"];

                merchandiseListEntry = new ROMerchandiseListEntry(
                    sequenceNumber: sequenceNumber,
                    merchandiseType: merchandiseType,
                    text: text,
                    key: key
                    );

                method.MerchandiseList.Add(merchandiseListEntry);
            }
        }

        private void AddVelocityGrades(ROMethodAllocationVelocityProperties method)
        {
            ROAllocationVelocityGrade velocityGrade; 

            DataTable velocityGradesDataTable = _dsVelocity.Tables["VelocityGrade"];
            velocityGradesDataTable.DefaultView.Sort = "Boundary DESC";

            foreach (DataRowView row in velocityGradesDataTable.DefaultView)
            {
                velocityGrade = new ROAllocationVelocityGrade();
                velocityGrade.StoreGrade = new KeyValuePair<int, string>(Convert.ToInt32(row["Boundary"]), Convert.ToString(row["Grade"], CultureInfo.CurrentUICulture));

                if (row["MinStock"] != System.DBNull.Value)
                {
                    velocityGrade.Minimum = Convert.ToInt32(row["MinStock"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    velocityGrade.Minimum = null;
                }

                if (row["MaxStock"] != System.DBNull.Value)
                {
                    velocityGrade.Maximum = Convert.ToInt32(row["MaxStock"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    velocityGrade.Maximum = null;
                }

                if (row["MinAd"] != System.DBNull.Value)
                {
                    velocityGrade.AdMinimum = Convert.ToInt32(row["MinAd"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    velocityGrade.AdMinimum = null;
                }

                method.VelocityGradeList.Add(velocityGrade);

            }
        }

        private void AddSellThruValues(ROMethodAllocationVelocityProperties method)
        {
            ROSellThruList sellThru;
            int sellThruIndex;
            string columnHeading;

            Dictionary<int, string> sellThruColumnHeadings = BuildSellThruColumnHeadings();

            DataTable sellThruPctsDataTable = _dsVelocity.Tables["SellThru"];
            sellThruPctsDataTable.DefaultView.Sort = "SellThruIndex DESC";

            foreach (DataRowView row in sellThruPctsDataTable.DefaultView)
            {
                sellThruIndex = Convert.ToInt32(row["SellThruIndex"]);
                if (!sellThruColumnHeadings.TryGetValue(sellThruIndex, out columnHeading))
                {
                    columnHeading = string.Empty;
                }

                sellThru = new ROSellThruList(
                    sellThru: sellThruIndex,
                    sellThruHeading: columnHeading
                    );

                method.SellThruList.Add(sellThru);
            }
        }

        private void AddNoOnhandRules(ROMethodAllocationVelocityProperties method)
        {
            method.NoOnHandRuleList.Add(new KeyValuePair<int, string>((int)eVelocityRuleType.None, MIDText.GetTextOnly((int)eVelocityRuleType.None)));
            DataTable velocityRules = MIDText.GetLabels((int)eVelocityRuleType.WeeksOfSupply, (int)eVelocityRuleType.ForwardWeeksOfSupply);

            foreach (DataRow row in velocityRules.Rows)
            {
                method.NoOnHandRuleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"])));
            }
        }

        private void AddOnhandRules(ROMethodAllocationVelocityProperties method)
        {
            method.OnHandRuleList.Add(new KeyValuePair<int, string>((int)eVelocityRuleType.None, MIDText.GetTextOnly((int)eVelocityRuleType.None)));
            DataTable velocityRules = MIDText.GetLabels((int)eVelocityRuleType.WeeksOfSupply, (int)eVelocityRuleType.ForwardWeeksOfSupply);

            foreach (DataRow row in velocityRules.Rows)
            {
                method.OnHandRuleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"])));
            }
        }

        private void AddMatrixRules(ROMethodAllocationVelocityProperties method)
        {
            DataTable velocityRules = MIDText.GetLabels((int)eVelocityRuleRequiresQuantity.WeeksOfSupply, (int)eVelocityRuleRequiresQuantity.ShipUpToQty);

            foreach (DataRow row in velocityRules.Rows)
            {
                method.MatrixModeRuleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"])));
            }
            method.MatrixModeRuleList.Add(new KeyValuePair<int, string>((int)eVelocityRuleRequiresQuantity.AbsoluteQuantity, MIDText.GetTextOnly((int)eVelocityRuleRequiresQuantity.AbsoluteQuantity)));
            method.MatrixModeRuleList.Add(new KeyValuePair<int, string>((int)eVelocityRuleRequiresQuantity.ForwardWeeksOfSupply, MIDText.GetTextOnly((int)eVelocityRuleRequiresQuantity.ForwardWeeksOfSupply)));
        }

        private void AddMatrixAttributeSets(ROMethodAllocationVelocityProperties method)
        {
            ROMethodAllocationVelocityAttributeSet attributeSet = null;
            eVelocityMatrixMode matrixMode;
            eVelocitySpreadOption spreadOption;
            KeyValuePair<int, string> attributeSetKVP;
            double? noOnHandRuleValue;
            double? matrixModeAverageRuleValue;
            double? allStoresAverageWOS;
            double? allStoresSellThruPercent;
            double? averageWOS;
            double? sellThruPercent;
            GroupLvlMatrix glm = null;

            if (_groupLvlMtrxData != null)
            {
                foreach (GroupLvlMatrix groupLevelMatrix in _groupLvlMtrxData.Values)
                {
                    if (groupLevelMatrix.SglRID == method.AttributeSet.Key)
                    {
                        glm = groupLevelMatrix;
                        break;
                    }
                }
            }
            else  // try to get the data from the data set
            {
                if (_dsVelocity.Tables["GroupLevel"].Rows.Count > 0)
                {
                    string selectString = "SglRID=" + method.AttributeSet.Key;
                    DataRow[] groupRows = _dsVelocity.Tables["GroupLevel"].Select(selectString);
                    if (groupRows.Length > 0)
                    {
                        // build class from datatable
                        glm = new GroupLvlMatrix();
                        glm.SglRID = method.AttributeSet.Key;
                        InitializeGroupLvlMatrix(glm);
                        glm.NoOnHandRuleQty = Include.NoRuleQty;
                        glm.NoOnHandRuleType = eVelocityRuleType.None;
                        if (!Convert.IsDBNull(groupRows[0]["NoOnHandQty"]))
                        {
                            glm.NoOnHandRuleQty = Convert.ToDouble(groupRows[0]["NoOnHandQty"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(groupRows[0]["NoOnHandRule"]))
                        {
                            glm.NoOnHandRuleType = (eVelocityRuleType)Convert.ToInt32(groupRows[0]["NoOnHandRule"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(groupRows[0]["ModeInd"]))
                        {
                            glm.ModeInd = Convert.ToChar(groupRows[0]["ModeInd"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(groupRows[0]["AverageRule"]))
                        {
                            glm.AverageRule = (eVelocityRuleRequiresQuantity)Convert.ToInt32(groupRows[0]["AverageRule"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(groupRows[0]["AverageQty"]))
                        {
                            glm.AverageQty = Convert.ToDouble(groupRows[0]["AverageQty"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(groupRows[0]["SpreadInd"]))
                        {
                            glm.SpreadInd = Convert.ToChar(groupRows[0]["SpreadInd"], CultureInfo.CurrentUICulture);
                        } 
                    }
                }
            }

            if (glm != null)
            {
                if (glm.ModeInd == 'A')
                {
                    matrixMode = eVelocityMatrixMode.Average;
                    matrixModeAverageRuleValue = glm.AverageQty;
                }
                else
                {
                    matrixMode = eVelocityMatrixMode.Normal;
                    matrixModeAverageRuleValue = null;
                }

                if (glm.SpreadInd == 'S')
                {
                    spreadOption = eVelocitySpreadOption.Smooth;
                }
                else
                {
                    spreadOption = eVelocitySpreadOption.Index;
                }

                if (glm.SglRID == 0)
                {
                    attributeSetKVP = new KeyValuePair<int, string>(glm.SglRID, "Total Matrix");
                }
                else
                {
                    attributeSetKVP = GetName.GetAttributeSetName(key: glm.SglRID);
                }

                if (glm.NoOnHandRuleType == eVelocityRuleType.None
                    || glm.NoOnHandRuleType == eVelocityRuleType.Out
                    || glm.NoOnHandRuleQty == Include.NoRuleQty)
                {
                    noOnHandRuleValue = null;
                }
                else
                {
                    noOnHandRuleValue = glm.NoOnHandRuleQty;
                }

                allStoresAverageWOS = null;
                averageWOS = null;
                if (_statisticsCalculated)
                {
                    allStoresAverageWOS = _applicationTransaction.VelocityGetMatrixChainAvgWOS(glm.SglRID);
                    averageWOS = _applicationTransaction.VelocityGetMatrixGroupAvgWOS(glm.SglRID);
                }

                allStoresSellThruPercent = null;
                sellThruPercent = null;
                if (_statisticsCalculated)
                {
                    allStoresSellThruPercent = _applicationTransaction.VelocityGetMatrixChainPctSellThru(glm.SglRID);
                    sellThruPercent = _applicationTransaction.VelocityGetMatrixGroupPctSellThru(glm.SglRID);
                }

                attributeSet = new ROMethodAllocationVelocityAttributeSet(
                    attributeSet: attributeSetKVP,
                    noOnHandRule: EnumTools.VerifyEnumValue(glm.NoOnHandRuleType),
                    noOnHandRuleValue: noOnHandRuleValue,
                    matrixMode: EnumTools.VerifyEnumValue(matrixMode),
                    matrixModeAverageRule: EnumTools.VerifyEnumValue(glm.AverageRule),
                    matrixModeAverageRuleValue: matrixModeAverageRuleValue,
                    spreadOption: EnumTools.VerifyEnumValue(spreadOption),
                    allStoresAverageWOS: allStoresAverageWOS,
                    allStoresSellThruPercent: allStoresSellThruPercent,
                    averageWOS: averageWOS,
                    sellThruPercent: sellThruPercent
                );

                if (_groupLvlMtrxData != null)
                {
                    AddVelocityGradeValues(method: method, attributeSet: attributeSet, glm: glm);
                }

                method.MatrixAttributeSetValues = attributeSet;
            }

            if (attributeSet == null) // set defaults if no entry
            {
                attributeSetKVP = GetName.GetAttributeSetName(key: method.AttributeSet.Key);

                attributeSet = new ROMethodAllocationVelocityAttributeSet(
                        attributeSet: attributeSetKVP,
                        noOnHandRule: eVelocityRuleType.None,
                        noOnHandRuleValue: null,
                        matrixMode: eVelocityMatrixMode.Normal,
                        matrixModeAverageRule: eVelocityRuleRequiresQuantity.AbsoluteQuantity,
                        matrixModeAverageRuleValue: null,
                        spreadOption: eVelocitySpreadOption.Smooth,
                        allStoresAverageWOS: null,
                        allStoresSellThruPercent: null,
                        averageWOS: null,
                        sellThruPercent: null
                    );

                method.MatrixAttributeSetValues = attributeSet;
            }
        }

        private Dictionary<int, string> BuildSellThruColumnHeadings()
        {
            Dictionary<int, string> sellThruColumnHeadings = new Dictionary<int, string>();
            string colCaption = string.Empty;
            int index = 0, prevIndex = 0;

            DataTable sellThruPctsDataTable = _dsVelocity.Tables["SellThru"];
            sellThruPctsDataTable.DefaultView.Sort = "SellThruIndex DESC";

            for (int i = 0; i < sellThruPctsDataTable.DefaultView.Count; i++)
            {
                DataRowView dr = sellThruPctsDataTable.DefaultView[i];
                index = (int)dr["SellThruIndex"];
                if (i == 0)
                {
                    colCaption = ">" + Convert.ToString(index, CultureInfo.CurrentUICulture);
                }
                else if (i == sellThruPctsDataTable.DefaultView.Count - 1)
                {
                    colCaption = Convert.ToString(index, CultureInfo.CurrentUICulture) + "-"
                        + Convert.ToString(prevIndex, CultureInfo.CurrentUICulture);
                }
                else
                {
                    colCaption = Convert.ToString(index + 1, CultureInfo.CurrentUICulture) + "-"
                        + Convert.ToString(prevIndex, CultureInfo.CurrentUICulture);
                }

                prevIndex = index;

                sellThruColumnHeadings.Add(index, colCaption);
            }

            return sellThruColumnHeadings;
        }

        private void AddVelocityGradeValues(ROMethodAllocationVelocityProperties method, 
            ROMethodAllocationVelocityAttributeSet attributeSet,
            GroupLvlMatrix glm)
        {
            ROMethodAllocationVelocityMatrixVelocityGrade matrixVelocityGrade;
            int setValue, boundary;
            string grade;
            int? totalSales;
            double? avgSales;
            double? pctTotalSales;
            double? avgSalesIdx;
            double? totalNumStores;
            double? avgStock;
            double? stockPercentOfTotal;
            double? allocationPercentOfTotal;
            int velocityGradeCount = 0;

            setValue = attributeSet.AttributeSet.Key;

            Hashtable matrixCells_HT = (Hashtable)glm.MatrixCells;
            SortedList matrixCells = new SortedList();

            foreach (string key in matrixCells_HT.Keys)
            {
                matrixCells.Add(key, matrixCells_HT[key]);
            }

            int sellThruIndexCount = method.VelocityGradeList.Count == 0 ? 0 : matrixCells.Count / method.VelocityGradeList.Count;

            foreach (ROAllocationVelocityGrade velocityGrade in method.VelocityGradeList)
            {
                grade = velocityGrade.StoreGrade.Value;
                boundary = velocityGrade.StoreGrade.Key;

                if (_statisticsCalculated)
                {
                    totalSales = _applicationTransaction.VelocityGetMatrixGradeTotBasisSales(setValue, grade);
                    avgSales = _applicationTransaction.VelocityGetMatrixGradeAvgBasisSales(setValue, grade);
                    pctTotalSales = _applicationTransaction.VelocityGetMatrixGradeAvgBasisSalesPctTot(setValue, grade);
                    avgSalesIdx = _applicationTransaction.VelocityGetMatrixGradeAvgBasisSalesIdx(setValue, grade);
                    totalNumStores = _applicationTransaction.VelocityGetMatrixGradeTotalNumberOfStores(setValue, grade);
                    avgStock = _applicationTransaction.VelocityGetMatrixGradeAvgStock(setValue, grade);
                    stockPercentOfTotal = _applicationTransaction.VelocityGetMatrixGradeStockPercentOfTotal(setValue, grade);
                    allocationPercentOfTotal = _applicationTransaction.VelocityGetMatrixGradeAllocationPercentOfTotal(setValue, grade);
                }
                else
                {
                    totalSales = null;
                    avgSales = null;
                    pctTotalSales = null;
                    avgSalesIdx = null;
                    totalNumStores = null;
                    avgStock = null;
                    stockPercentOfTotal = null;
                    allocationPercentOfTotal = null;
                }

                matrixVelocityGrade = new ROMethodAllocationVelocityMatrixVelocityGrade(velocityGrade: velocityGrade.StoreGrade,
                    totalSales: totalSales,
                    avgSales: avgSales,
                    pctTotalSales: pctTotalSales,
                    avgSalesIdx: avgSalesIdx,
                    totalNumStores: totalNumStores,
                    avgStock: avgStock,
                    stockPercentOfTotal: stockPercentOfTotal,
                    allocationPercentOfTotal: allocationPercentOfTotal
                    );

                AddVelocityGradeSellThruIndexCells(matrixVelocityGrade: matrixVelocityGrade,
                    matrixCells: matrixCells,
                    currentVelocityGrade: velocityGradeCount,
                    setValue: setValue,
                    sellThruIndexCount: sellThruIndexCount);

                attributeSet.MatrixGradeValues.Add(matrixVelocityGrade);

                ++velocityGradeCount;
            }

            //Add Total line
            if (_matrixProcessed)
            {
                KeyValuePair<int, string> totals = new KeyValuePair<int, string>(int.MaxValue, "Total:");

                if (_applicationTransaction != null
                    && _applicationTransaction.VelocityCriteriaExists == true)
                {
                    totalSales = _applicationTransaction.VelocityGetMatrixGradeTotBasisSales(setValue, null);
                    avgSales = _applicationTransaction.VelocityGetMatrixGradeAvgBasisSales(setValue, null);
                    pctTotalSales = _applicationTransaction.VelocityGetMatrixGradeAvgBasisSalesPctTot(setValue, null);
                    avgSalesIdx = _applicationTransaction.VelocityGetMatrixGradeAvgBasisSalesIdx(setValue, null);
                    totalNumStores = _applicationTransaction.VelocityGetMatrixGradeTotalNumberOfStores(setValue, null);
                    avgStock = _applicationTransaction.VelocityGetMatrixGradeAvgStock(setValue, null);
                    stockPercentOfTotal = _applicationTransaction.VelocityGetMatrixGradeStockPercentOfTotal(setValue, null);
                    allocationPercentOfTotal = _applicationTransaction.Velocity.GetSetAllocatedPctOfTotal(method.Attribute.Key, setValue, null);
                }
                else
                {
                    totalSales = null;
                    avgSales = null;
                    pctTotalSales = null;
                    avgSalesIdx = null;
                    totalNumStores = null;
                    avgStock = null;
                    stockPercentOfTotal = null;
                    allocationPercentOfTotal = null;
                }

                matrixVelocityGrade = new ROMethodAllocationVelocityMatrixVelocityGrade(velocityGrade: totals,
                    totalSales: totalSales,
                    avgSales: avgSales,
                    pctTotalSales: pctTotalSales,
                    avgSalesIdx: avgSalesIdx,
                    totalNumStores: totalNumStores,
                    avgStock: avgStock,
                    stockPercentOfTotal: stockPercentOfTotal,
                    allocationPercentOfTotal: allocationPercentOfTotal
                );

                AddVelocityTotalSellThruIndexCells(matrixVelocityGrade: matrixVelocityGrade,
                    setValue: setValue);

                attributeSet.MatrixGradeValues.Add(matrixVelocityGrade);
            }
        }

        private void AddVelocityGradeSellThruIndexCells(ROMethodAllocationVelocityMatrixVelocityGrade matrixVelocityGrade,
            SortedList matrixCells,
            int currentVelocityGrade,
            int sellThruIndexCount,
            int setValue
            )
        {
            ROMethodAllocationVelocityMatrixCell matrixCell;
            GroupLvlCell glc;
            string cellKey;
            int? numberOfStores;
            double? averageWOS;
            eVelocityRuleType cellRule;
            double? cellRuleValue;

            for (int i = 0; i < sellThruIndexCount; i++)
            {
                cellKey = currentVelocityGrade.ToString() + "," + i.ToString();
                glc = (GroupLvlCell)matrixCells[cellKey];

                if (_statisticsCalculated)
                {
                    numberOfStores = _applicationTransaction.VelocityGetMatrixCellStores(setValue, glc.Boundary, glc.SellThruIndex);
                    averageWOS = _applicationTransaction.VelocityGetMatrixCellAvgWOS(setValue, glc.Boundary, glc.SellThruIndex);
                    cellRule = glc.CellRuleType;
                    cellRuleValue = glc.CellRuleQty;
                }
                else
                {
                    numberOfStores = glc.CellGrpStores;
                    averageWOS = glc.CellGrpAvgWOS;
                    cellRule = glc.CellRuleType;
                    cellRuleValue = glc.CellRuleQty;
                }

                matrixCell = new ROMethodAllocationVelocityMatrixCell(boundary: glc.Boundary,
                    sellThruIndex: glc.SellThruIndex,
                    ruleType: EnumTools.VerifyEnumValue(cellRule),
                    ruleValue: cellRuleValue,
                    numberOfStores: numberOfStores,
                    averageWOS: averageWOS
                    );

                matrixVelocityGrade.MatrixGradeCells.Add(matrixCell);
            }
        }

        private void AddVelocityTotalSellThruIndexCells(ROMethodAllocationVelocityMatrixVelocityGrade matrixVelocityGrade,
            int setValue
            )
        {
            DataTable sellThruPctsDataTable = _dsVelocity.Tables["SellThru"];
            int sellThruIdx;
            ROMethodAllocationVelocityMatrixCell matrixCell;
            int? numberOfStores;
            double? averageWOS;
            eVelocityRuleType cellRule;
            double? cellRuleValue;

            for (int sellThruRow = 0; sellThruRow < sellThruPctsDataTable.Rows.Count; sellThruRow++)
            {
                if (_applicationTransaction != null
                    && _applicationTransaction.VelocityCriteriaExists == true)
                {
                    sellThruIdx = Convert.ToInt32(sellThruPctsDataTable.Rows[sellThruRow]["SellThruIndex"]);
                    numberOfStores = _applicationTransaction.VelocityGetSellThruTotalStores(setValue, sellThruIdx);
                    averageWOS = _applicationTransaction.VelocityGetSellThruAvgWOS(setValue, sellThruIdx);
                    cellRule = eVelocityRuleType.None;
                    cellRuleValue = null;
                }
                else
                {
                    sellThruIdx = Convert.ToInt32(sellThruPctsDataTable.Rows[sellThruRow]["SellThruIndex"]);
                    numberOfStores = 0;
                    averageWOS = 0;
                    cellRule = eVelocityRuleType.None;
                    cellRuleValue = null;
                }

                matrixCell = new ROMethodAllocationVelocityMatrixCell(boundary: int.MaxValue,
                    sellThruIndex: sellThruIdx,
                    ruleType: EnumTools.VerifyEnumValue(cellRule),
                    ruleValue: cellRuleValue,
                    numberOfStores: numberOfStores,
                    averageWOS: averageWOS
                    );

                matrixVelocityGrade.MatrixGradeCells.Add(matrixCell);
            }
        }

        private void GetMatrixViews(ROMethodAllocationVelocityProperties method)
        {
            DataTable dtViews = new DataTable();
            ArrayList userRIDList = new ArrayList();
            GridViewData gridViewData = new GridViewData();
            UserGridView userGridView = new UserGridView();

            try
            {
                FunctionSecurityProfile globalViewSecurity;
                FunctionSecurityProfile userViewSecurity;
                eLayoutID layoutID;

                globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalVelocity);
                userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserVelocity);
                layoutID = eLayoutID.sizeReviewGrid;

                if (globalViewSecurity.AllowView)
                {
                    userRIDList.Add(Include.GlobalUserRID);
                }
                if (userViewSecurity.AllowView)
                {
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                if (userRIDList.Count > 0)
                {
                    dtViews = gridViewData.GridView_Read((int)eLayoutID.velocityMatrixGrid, userRIDList, true);

                    dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, layoutID, string.Empty });
                    dtViews.PrimaryKey = new DataColumn[] { dtViews.Columns["VIEW_RID"] };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            method.MatrixViews = DataTableToKeyValues(dtViews, "VIEW_RID", "VIEW_ID");
            if (method.MatrixViews.Count > 0)
            {
                userGridView = new UserGridView();

                int viewRID = userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.velocityMatrixGrid);
                if (viewRID != Include.NoRID)
                {
                    method.MatrixSelectedView = method.MatrixViews.Find(kvp => kvp.Key == viewRID);
                }
                if (!method.MatrixSelectedViewIsSet)
                {
                    method.MatrixSelectedView = new KeyValuePair<int, string>(method.MatrixViews[0].Key, method.MatrixViews[0].Value);
                }
            }

        }

        private void GetMatrixViewColumns(ROMethodAllocationVelocityProperties method)
        {
            string colKey, errMessage;
            bool isHidden;

            GridViewData gridViewData = new GridViewData();

            if (!method.MatrixSelectedViewIsSet)
            {
                method.MatrixViewColumns.Add(new KeyValuePair<string, bool>("Grade", true));
                method.MatrixViewColumns.Add(new KeyValuePair<string, bool>("Boundary", true));
                return;
            }

            DataTable dtGridViewDetail = gridViewData.GridViewDetail_Read(method.MatrixSelectedView.Key);

            if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
            {
                errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_GridViewDoesNotExist);
                return;
            }

            dtGridViewDetail.DefaultView.Sort = "VISIBLE_POSITION";

            foreach (DataRowView row in dtGridViewDetail.DefaultView)
            {
                colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                isHidden = Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                method.MatrixViewColumns.Add(new KeyValuePair<string, bool>(colKey, !isHidden));
            }
        }

        private bool SetupInteractive(ROMethodAllocationVelocityProperties method, ref string message)
        {
            bool successful = true;

            if (_applicationTransaction != null && _applicationTransaction.StyleView != null)
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CloseStyleReviewForInteractive);
                return false;
            }

            if (!GetComponents(method: method, message: ref message))
            {
                return false;
            }

            if (_needToLockHeaders)
            {
                if (!LockHeaders(ref message))
                {
                    return false;
                }
                else
                {
                    _needToLockHeaders = false;
                }
            }

            return successful;
        }

        private bool InInteractiveMode()
        {
            return (_applicationTransaction != null && _applicationTransaction.AllocationCriteriaExists && _applicationTransaction.StyleView != null) ? true : false;
        }

        private bool GetComponents(ROMethodAllocationVelocityProperties method, ref string message)
        {
            SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

            if (selectedHeaderList.Count == 0)
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace);
                method.Interactive = false;
                return false;
            }

            _compDataTable = MIDText.GetLabels((int)eVelocityMethodComponentType.Total, (int)eVelocityMethodComponentType.Bulk);
            for (int i = _compDataTable.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = _compDataTable.Columns[i];
                if (dc.ColumnName != "TEXT_CODE" && dc.ColumnName != "TEXT_VALUE")
                {
                    _compDataTable.Columns.Remove(dc);
                }
            }
            foreach (DataRow dr in _compDataTable.Rows)
            {
                eVelocityMethodComponentType vmct = (eVelocityMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                if (!Enum.IsDefined(typeof(eVelocityMethodComponentType), vmct))
                {
                    dr.Delete();
                }
            }

            _compDataTable.AcceptChanges();

            DataColumn col = new DataColumn();
            col.DataType = System.Type.GetType("System.Int32");
            col.ColumnName = "CompType";
            _compDataTable.Columns.Add(col);
            foreach (DataRow dr in _compDataTable.Rows)
            {
                dr["CompType"] = dr["TEXT_CODE"];
            }

            DataColumn[] PrimaryKeyColumn = new DataColumn[1];
            PrimaryKeyColumn[0] = _compDataTable.Columns["TEXT_CODE"];
            _compDataTable.PrimaryKey = PrimaryKeyColumn;

            GetPacksAndColors(selectedHeaderList: selectedHeaderList);

            method.Components = DataTableToKeyValues(_compDataTable, "TEXT_CODE", "TEXT_VALUE");

            return true;
        }

        private void GetPacksAndColors(SelectedHeaderList selectedHeaderList)
        {
            Header header;
            try
            {
                bool colorsExist = false;
                bool packFound;

                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    header = new Header();
                    DataTable dtPacks = header.GetPacks(shp.Key);
                    DataTable dtBulkColors = header.GetBulkColors(shp.Key);
                    if (dtPacks.Rows.Count > 0)
                    {
                        foreach (DataRow pRow in dtPacks.Rows)
                        {
                            packFound = false;
                            string dtPackName = pRow["HDR_PACK_NAME"].ToString();

                            foreach (DataRow compRow in _compDataTable.Rows)
                            {
                                string compPackName = compRow["TEXT_VALUE"].ToString();
                                eComponentType compType = (eComponentType)compRow["CompType"];
                                if (compPackName == dtPackName && compType == eComponentType.SpecificPack)
                                {
                                    packFound = true;
                                    break;
                                }
                            }
                            if (!packFound)
                                _compDataTable.Rows.Add(new object[] { (int) pRow["HDR_PACK_RID"], pRow["HDR_PACK_NAME"],
                                                                      (int) eComponentType.SpecificPack});
                        }
                    }
                    if (dtBulkColors.Rows.Count > 0)
                    {
                        colorsExist = true;
                        foreach (DataRow cRow in dtBulkColors.Rows)
                        {
                            int colorKey = Convert.ToInt32(cRow["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                            if (!_compDataTable.Rows.Contains(colorKey))
                            {
                                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorKey);
                                _compDataTable.Rows.Add(new object[] {colorKey, ccp.ColorCodeName,
                                                                          (int) eComponentType.SpecificColor});
                            }
                        }
                    }
                }
                if (!colorsExist)
                {
                    RemoveBulk();
                }

                _compDataTable.AcceptChanges();

            }
            catch (Exception)
            {
               throw;
            }

        }

        private void RemoveBulk()
        {
            try
            {
                foreach (DataRow dr in _compDataTable.Rows)
                {
                    eVelocityMethodComponentType vmct = (eVelocityMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                    if (vmct == eVelocityMethodComponentType.Bulk)
                    {
                        dr.Delete();
                        break;
                    }
                }
                _compDataTable.AcceptChanges();
            }

            catch (Exception)
            {
                throw;
            }
        }

        private bool LockHeaders(ref string message)
        {
            string enqMessage = string.Empty;
            if (!_applicationTransaction.EnqueueSelectedHeaders(out enqMessage))
            {
                if (message.Length > 0)
                {
                    message += Environment.NewLine + enqMessage;
                }
                else
                {
                    message = enqMessage;
                }

                return false;
            }

            return true;
        }

        private bool ProcessInteractive(ref string message)  
        {
            bool ErrorFound = false;
            try
            {
                if (!_updateFromStoreDetail)
                {

                    if (Key == Include.NoRID)
                    {
                        Key = Include.UndefinedVelocityMethodRID;
                    }

                }
                else
                {
                    _applicationTransaction.ResetVelocityMethod();
                }

                //int assrtRid = AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID;
                int assrtRid = Include.NoRID;
                if (assrtRid != Include.NoRID)
                {
                    _applicationTransaction = SAB.AssortmentTransactionEvent.GetAssortmentTransaction(this, assrtRid);

                    if (_applicationTransaction.DataState == eDataState.ReadOnly)
                    {
                        throw new GroupAllocationAssortmentReadOnlyException(MIDText.GetText(eMIDTextCode.msg_as_NoProcessingMethodReadOnly),
                            AssortmentActiveProcessToolbarHelper.ActiveProcess.screenType,
                            AssortmentActiveProcessToolbarHelper.ActiveProcess.screenTitle);
                    }
                }
                else
                {
                    if (_applicationTransaction == null)
                    {
                        _applicationTransaction = SAB.ApplicationServerSession.CreateTransaction();
                    }
                    SelectedHeaderList selectedHeaderList = (SelectedHeaderList)_applicationTransaction.GetProfileList(eProfileType.SelectedHeader);
                    _applicationTransaction.LoadHeadersInTransaction(selectedHeaderList);

                }

                //=======================================================================================
                // This only needs to be checked if headers are selected from the allocation workspace.
                // You can tell that is true if the assrtRid == Include.NoRID.
                //=======================================================================================
                if (assrtRid == Include.NoRID)
                {
                    ArrayList headerInGAList = new ArrayList();
                    if (_applicationTransaction.ContainsGroupAllocationHeaders(ref headerInGAList))
                    {
                        throw new HeaderInGroupAllocationException(MIDText.GetText(eMIDTextCode.msg_al_HeaderBelongsToGroupAllocation), headerInGAList);
                    }
                }
                _applicationTransaction.CreateVelocityMethod(SAB, Key);
                UpdateTransData();
                _applicationTransaction.VelocityLoadDataArrays();
                _applicationTransaction.Velocity.BasisChangesMade = _basisChangesMade;
                _applicationTransaction.ProcessInteractiveVelocity();

                eAllocationActionStatus actionStatus = _applicationTransaction.AllocationActionAllHeaderStatus;
                if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    message = MIDText.GetTextOnly((int)actionStatus);
                    return false;
                }

                if (!ErrorFound)
                {
                    _statisticsCalculated = true;
                    _matrixProcessed = true;
                    _matrixEverProcessed = true; 
                    _basisChangesMade = false;
                    _groupLvlMtrxData = _applicationTransaction.Velocity.GroupLvlMtrxData;
                }

                return true; 
            }

            catch (HeaderInGroupAllocationException err)
            {
                string headerListMsg = string.Empty;
                foreach (string headerId in err.HeaderList)
                {
                    if (headerListMsg.Length > 0)
                        headerListMsg += ", " + headerId;
                    else
                        headerListMsg = " " + headerId;
                }
               message = err.Message + headerListMsg;
                return false;
            }
            catch (GroupAllocationAssortmentReadOnlyException err)
            {
                string errorMessage = err.Message;
                errorMessage = errorMessage.Replace("{0}", err.GroupType);
                errorMessage = errorMessage.Replace("{1}", err.GroupName);

                message = errorMessage;
                return false;
            }

            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
         }

        private void UpdateTransData()
        {
            _applicationTransaction.VelocityStoreGroupRID = StoreGroupRID;
            _applicationTransaction.VelocityOTSPlanHNRID = OTSPlanHNRID;
            _applicationTransaction.VelocityOTSPlanPHRID = OTSPlanPHRID;
            _applicationTransaction.VelocityOTSPlanPHLSeq = OTSPlanPHLSeq;
            _applicationTransaction.VelocityOTS_Begin_CDR_RID = OTS_Begin_CDR_RID;
            _applicationTransaction.VelocityOTS_Ship_To_CDR_RID = OTS_Ship_To_CDR_RID;
            _applicationTransaction.VelocityUseSimilarStoreHistory = UseSimilarStoreHistory;
            _applicationTransaction.VelocityCalculateAverageUsingChain = CalculateAverageUsingChain;
            _applicationTransaction.VelocityCalculateGradeVariableType = GradeVariableType; 
            _applicationTransaction.VelocityBalanceToHeaderInd = BalanceToHeaderInd; 
            _applicationTransaction.VelocityDetermineShipQtyUsingBasis = DetermineShipQtyUsingBasis;
            _applicationTransaction.VelocityTrendPctContribution = TrendPctContribution;
            _applicationTransaction.VelocityApplyMinMaxInd = ApplyMinMaxInd;
            _applicationTransaction.VelocityInventoryInd = InventoryInd;
            _applicationTransaction.Velocity.MERCH_PHL_SEQ = MERCH_PHL_SEQ;
            _applicationTransaction.Velocity.MERCH_PH_RID = MERCH_PH_RID;
            _applicationTransaction.Velocity.MERCH_HN_RID = MERCH_HN_RID;
            _applicationTransaction.VelocityDSVelocity = DSVelocity;
            _applicationTransaction.Velocity.Component = Component;  
            _applicationTransaction.Velocity.Balance = Balance; 
            _applicationTransaction.Velocity.Reconcile = Reconcile;  
                                                               
            if (BasisChangesMade)
            {
                _applicationTransaction.ClearAllocationCubeGroup();
            }

            _applicationTransaction.VelocityIsInteractive = true;
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodAllocationVelocityProperties method = (ROMethodAllocationVelocityProperties)methodProperties;

            Template_IND = methodProperties.IsTemplate;
            CalculateAverageUsingChain = method.CalculateAverageUsing == eVelocityCalculateAverageUsing.AllStores;
            DetermineShipQtyUsingBasis = method.DetermineShipQtyUsing == eVelocityDetermineShipQtyUsing.Basis;

            switch (method.ApplyMinMaxType)
            {
                case eVelocityApplyMinMaxType.StoreGrade:
                    ApplyMinMaxInd = 'S';
                    break;
                case eVelocityApplyMinMaxType.VelocityGrade:
                    ApplyMinMaxInd = 'V';
                    break;
                default:
                    ApplyMinMaxInd = 'N';
                    break;
            }

            GradeVariableType = method.GradeVariableType;

            UseSimilarStoreHistory = method.UseSimilarStoreHistory;
            Balance = method.Balance;
            BalanceToHeaderInd = method.BalanceToHeader ? '1' : '0';
            Reconcile = method.Reconcile;

            switch (method.InventoryIndicator)
            {
                case eMinMaxType.Inventory:
                    InventoryInd = 'I';
                    break;
                default:
                    InventoryInd = 'A';
                    break;
            }

            if (method.InventoryIndicator == eMinMaxType.Inventory)
            {
                MerchandiseType = method.InventoryMinMaxMerchType;
                switch (MerchandiseType)
                {
                    case eMerchandiseType.Node:
                        MERCH_HN_RID = method.InventoryMinMaxMerchandise.Key;
                        break;
                    case eMerchandiseType.HierarchyLevel:
                        MERCH_PHL_SEQ = method.InventoryMinMaxMerchandiseHierarchy.Value;
                        MERCH_PH_RID = HP.Key;
                        MERCH_HN_RID = Include.NoRID;
                        break;
                    case eMerchandiseType.OTSPlanLevel:
                        MERCH_HN_RID = Include.NoRID;
                        MERCH_PH_RID = Include.NoRID;
                        MERCH_PHL_SEQ = 0;
                        break;
                }
            }
            else
            {
                MERCH_HN_RID = Include.NoRID;
                MERCH_PH_RID = Include.NoRID;
                MERCH_PHL_SEQ = 0;
            }

            OTSPlanHNRID = Include.NoRID;
            OTSPlanPHRID = Include.NoRID;
            OTSPlanPHLSeq = 0;
            OTS_Begin_CDR_RID = Include.UndefinedCalendarDateRange;
            OTS_Ship_To_CDR_RID = Include.UndefinedCalendarDateRange;
            if (_SG_RID != method.Attribute.Key
                && !method.AddingMethod)
            {
                _matrixProcessed = false;
                method.VelocityAction = eVelocityAction.ClearMatrix;
            }

            if (_SG_RID != method.Attribute.Key)
            {
                // is attribute is changed, rebuild matrix data
				_populateVelocityGrades = true;
                _attributeChanged = true;
                // two different fields.  No idea why.  But, setting both since both are referenced
                _SG_RID = method.Attribute.Key;
                SG_RID = method.Attribute.Key;
                method.VelocityAction = eVelocityAction.ClearMatrix;
            }
            _currentAttributeSet = method.AttributeSet.Key;

            if (_dsVelocity == null)
            {
                _dsVelocity = _VMD.GetVelocityChildData();
            }

            ProfileList attributeSetList = StoreMgmt.StoreGroup_GetLevelListViewList(method.Attribute.Key);

            SetComponent(method: method);
            SetBasis(method: method);
            bool velocityBoundaryChanged;
            bool velocityGradesChanged = SetVelocityGrades(method: method, velocityBoundaryChanged: out velocityBoundaryChanged);
            if (velocityGradesChanged
                && !method.AddingMethod)
            {
                _matrixProcessed = false;
                method.VelocityAction = eVelocityAction.ClearMatrix;
            }
            SetSellThruValues(method: method);
            SetAttributeSet(method: method, setList: attributeSetList.ArrayList);
            SetVelocityMatrix(method: method, setList: attributeSetList.ArrayList, velocityBoundaryChanged: velocityBoundaryChanged);

            if (method.Interactive
                && !IsInteractive)
            {
                _needToLockHeaders = true;
            }
            IsInteractive = method.Interactive;

            // If merchandise key is provided, set flag to populate velocity grades during get
            // If key is -1 store grades will be cleared
            if (method.VelocityGradesMerchandiseIsSet
                && method.VelocityGradesMerchandise.Key != _velocityGradesMerchandiseKey)
            {
                _velocityGradesMerchandiseKey = method.VelocityGradesMerchandise.Key;
                _populateVelocityGrades = true;
                method.VelocityAction = eVelocityAction.ClearMatrix;
                // Clear the rule values to rebuild matrix
                _dsVelocity.Tables["VelocityMatrix"].Clear();
                _dsVelocity.Tables["VelocityMatrix"].AcceptChanges();
            }
            else
            {
                _velocityGradesMerchandiseKey = Include.NoRID;
            }

            LoadDataArrays();

            if (method.VelocityAction == eVelocityAction.ClearMatrix)
            {
                _matrixProcessed = false;
                _statisticsCalculated = false;
            }
            else if (IsInteractive
                && method.VelocityAction == eVelocityAction.ProcessInteractive)
            {
                if (!ProcessInteractive(ref message))
                {
                    return false;
                }
            }
            Template_IND = method.IsTemplate;

            return true;
        }

        private void SetComponent(ROMethodAllocationVelocityProperties method)
        {
            int compValue;
            eComponentType compType;
            if (method.Interactive
                && method.SelectedComponentIsSet)
            {
                //compValue = Convert.ToInt32(cboComponent.SelectedValue, CultureInfo.CurrentUICulture);
                compValue = method.SelectedComponent.Key;

                foreach (DataRow row in _compDataTable.Rows)
                {
                    if ((int)row["TEXT_CODE"] == compValue)
                    {
                        compType = (eComponentType)row["CompType"];
                        switch (compType)
                        {
                            case eComponentType.SpecificPack:
                                Component = new AllocationPackComponent(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture));
                                break;
                            case eComponentType.SpecificColor:
                                Component = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, compValue);
                                break;
                            case eComponentType.Total:
                                Component = new GeneralComponent(eGeneralComponentType.Total);
                                break;
                            case eComponentType.Bulk:
                                Component = new GeneralComponent(eGeneralComponentType.Bulk);
                                break;
                        }
                        break;
                    }
                }
            }
            else
            {
                Component = new GeneralComponent(eGeneralComponentType.Total);
            }
        }

        private void SetBasis(ROMethodAllocationVelocityProperties method)
        {
            DataTable dt = _dsVelocity.Tables["Basis"];
            DataRow row;
            int rowIndex = 0;

            // adjust values from FE
            foreach (ROBasisWithLevelDetailProfile basisDetailProfile in method.BasisProfiles)
            {
                // if hierarchy level, the FE does not have the correct organizational key
                // override the hierarchy key to the key of the organizational hierarchy
                if (basisDetailProfile.MerchPhlSequence > 0)
                {
                    basisDetailProfile.MerchPhRId = HP.Key;
                }
            }

            _basisChangesMade = false;
            if (dt.Rows.Count != method.BasisProfiles.Count)
            {
                _basisChangesMade = true;
            }
            else
            {
                foreach (ROBasisWithLevelDetailProfile basisDetailProfile in method.BasisProfiles)
                {
                    if (rowIndex < dt.Rows.Count)
                    {
                        row = dt.Rows[rowIndex];

                        if (Convert.ToInt32(row["BasisHNRID"]) != basisDetailProfile.MerchandiseId)
                        {
                            _basisChangesMade = true;
                        }
                        if (Convert.ToInt32(row["BasisPHRID"]) != basisDetailProfile.MerchPhRId)
                        {
                            _basisChangesMade = true;
                        }
                        if (Convert.ToInt32(row["BasisPHLSequence"]) != basisDetailProfile.MerchPhlSequence)
                        {
                            _basisChangesMade = true;
                        }
                        if (Convert.ToInt32(row["BasisFVRID"]) != basisDetailProfile.VersionId)
                        {
                            _basisChangesMade = true;
                        }
                        if (Convert.ToInt32(row["cdrRID"]) != basisDetailProfile.DateRangeId)
                        {
                            _basisChangesMade = true;
                        }
                        if (Convert.ToDouble(row["Weight"]) != basisDetailProfile.Weight)
                        {
                            _basisChangesMade = true;
                        }

                        if (_basisChangesMade)
                        {
                            break;
                        }
                    }
                    else
                    {
                        _basisChangesMade = true;
                        break;
                    }
                    ++rowIndex;
                }
            }

            if (dt.Rows.Count > 0)
            {
                dt.Clear();
            }

            int sequence = 0;
            foreach (ROBasisWithLevelDetailProfile basisDetailProfile in method.BasisProfiles)
            {
                row = dt.NewRow();

                row["BasisSequence"] = sequence;
                row["BasisHNRID"] = basisDetailProfile.MerchandiseId;
                row["BasisPHRID"] = basisDetailProfile.MerchPhRId;
                row["BasisPHLSequence"] = basisDetailProfile.MerchPhlSequence;
                row["BasisFVRID"] = basisDetailProfile.VersionId;
                row["cdrRID"] = basisDetailProfile.DateRangeId;
                row["Weight"] = basisDetailProfile.Weight;

                dt.Rows.Add(row);
                ++sequence;
            }
        }

        private bool SetVelocityGrades(ROMethodAllocationVelocityProperties method, out bool velocityBoundaryChanged)
        {
            bool velocityGradesChanged = false;
            velocityBoundaryChanged = false;
            DataTable dt = _dsVelocity.Tables["VelocityGrade"];
            DataRow row;
            int rowIndex = 0;

            if (dt.Rows.Count != method.VelocityGradeList.Count)
            {
                velocityGradesChanged = true;
            }
            else
            {
                foreach (ROAllocationVelocityGrade velocityGrade in method.VelocityGradeList)
                {
                    if (rowIndex < dt.Rows.Count)
                    {
                        row = dt.Rows[rowIndex];
                        if (Convert.ToString(row["Grade"]) != velocityGrade.StoreGrade.Value)
                        {
                            velocityGradesChanged = true;
                        }
                        if (Convert.ToInt32(row["Boundary"]) != velocityGrade.StoreGrade.Key)
                        {
                            velocityGradesChanged = true;
                            velocityBoundaryChanged = true;
                        }
                        int? minStock = null;
                        int? inputMinStock = null;
                        if (row["MinStock"] != System.DBNull.Value)
                        {
                            minStock = Convert.ToInt32(row["MinStock"]);
                        }
                        if (velocityGrade.MinimumIsSet)
                        {
                            inputMinStock = (int)velocityGrade.Minimum;
                        }
                        if (minStock != inputMinStock)
                        {
                            velocityGradesChanged = true;
                        }

                        int? maxStock = null;
                        int? inputMaxStock = null;
                        if (row["MaxStock"] != System.DBNull.Value)
                        {
                            maxStock = Convert.ToInt32(row["MaxStock"]);
                        }
                        if (velocityGrade.MaximumIsSet)
                        {
                            inputMaxStock = (int)velocityGrade.Maximum;
                        }
                        if (maxStock != inputMaxStock)
                        {
                            velocityGradesChanged = true;
                        }

                        int? adMin = null;
                        int? inputAdMin = null;
                        if (row["MinAd"] != System.DBNull.Value)
                        {
                            adMin = Convert.ToInt32(row["MinAd"]);
                        }
                        if (velocityGrade.AdMinimumIsSet)
                        {
                            inputAdMin = velocityGrade.AdMinimum;
                        }
                        if (adMin != inputAdMin)
                        {
                            velocityGradesChanged = true;
                        }

                        if (velocityGradesChanged)
                        {
                            break;
                        }

                    }
                    else
                    {
                        velocityGradesChanged = true;
                        break;
                    }
                    ++rowIndex;
                }
            }

            if (dt.Rows.Count > 0)
            {
                dt.Clear();
            }

            int sequence = 0;
            foreach (ROAllocationVelocityGrade velocityGrade in method.VelocityGradeList)
            {
                row = dt.NewRow();

                row["RowPosition"] = sequence;
                row["Grade"] = velocityGrade.StoreGrade.Value;
                row["Boundary"] = velocityGrade.StoreGrade.Key;
                if (velocityGrade.MinimumIsSet)
                {
                    row["MinStock"] = velocityGrade.Minimum;
                }
                if (velocityGrade.MaximumIsSet)
                {
                    row["MaxStock"] = velocityGrade.Maximum;
                }
                if (velocityGrade.AdMinimumIsSet)
                {
                    row["MinAd"] = velocityGrade.AdMinimum;
                }

                dt.Rows.Add(row);
                ++sequence;
            }

            return velocityGradesChanged;
        }

        private void SetSellThruValues(ROMethodAllocationVelocityProperties method)
        {
            DataTable dt = _dsVelocity.Tables["SellThru"];
            DataRow row;

            if (dt.Rows.Count > 0)
            {
                dt.Clear();
            }

            int sequence = 0;
            foreach (ROSellThruList sellThru in method.SellThruList)
            {
                row = dt.NewRow();

                row["RowPosition"] = sequence;
                row["SellThruIndex"] = sellThru.Sell_Thru;

                dt.Rows.Add(row);
                ++sequence;
            }
        }

        private void SetAttributeSet(ROMethodAllocationVelocityProperties method, ArrayList setList)
        {
            DataTable dt = _dsVelocity.Tables["GroupLevel"];
            DataRow row;

            // remove attribute set row to rebuild
            if (dt.Rows.Count > 0)
            {
                string selectString = "SglRID=" + method.MatrixAttributeSetValues.AttributeSet.Key;
                DataRow[] attributeSetRows = dt.Select(selectString);
                foreach (var attributeSetDataRow in attributeSetRows)
                {
                    attributeSetDataRow.Delete();
                }
                dt.AcceptChanges();
            }

            ROMethodAllocationVelocityAttributeSet attributeSet;
            attributeSet = method.MatrixAttributeSetValues;

            row = dt.NewRow();

            row["SglRID"] = attributeSet.AttributeSet.Key;
            row["NoOnHandRule"] = attributeSet.NoOnHandRule.GetHashCode();
            if (attributeSet.NoOnHandRuleValueIsSet)
            {
                row["NoOnHandQty"] = attributeSet.NoOnHandRuleValue;
            }
            if (attributeSet.MatrixMode == eVelocityMatrixMode.Average)
            {
                row["ModeInd"] = 'A';
                row["AverageRule"] = attributeSet.MatrixModeAverageRule.GetHashCode();
                row["AverageQty"] = attributeSet.MatrixModeAverageRuleValue;
            }
            else
            {
                row["ModeInd"] = 'N';
            }

            if (attributeSet.SpreadOption == eVelocitySpreadOption.Index)
            {
                row["SpreadInd"] = 'I';
            }
            else
            {
                row["SpreadInd"] = 'S';
            }

            dt.Rows.Add(row);

        }

        private void SetVelocityMatrix(
            ROMethodAllocationVelocityProperties method, 
            ArrayList setList,
            bool velocityBoundaryChanged
            )
        {
            if (method.VelocityAction == eVelocityAction.ClearMatrix)
            {
                // clear matrix datatable
                _dsVelocity.Tables["VelocityMatrix"].Clear();
                _dsVelocity.Tables["VelocityMatrix"].AcceptChanges();
                return;
            }

            DataTable dt = _dsVelocity.Tables["VelocityMatrix"];
            DataRow row;

            // remove attribute set rows to rebuild
            if (dt.Rows.Count > 0)
            {
                string selectString = "SglRID=" + method.MatrixAttributeSetValues.AttributeSet.Key;
                DataRow[] attributeSetRows = dt.Select(selectString);
                foreach (var attributeSetDataRow in attributeSetRows)
                {
                    attributeSetDataRow.Delete();
                }
                dt.AcceptChanges();
                // remove rows that do not match boundaries
                if (velocityBoundaryChanged)
                {
                    int boundary;
                    List<int> boundaries = new List<int>();
                    List<DataRow> rowsToDelete = new List<DataRow>();

                    foreach (DataRow velocityGradeRow in _dsVelocity.Tables["VelocityGrade"].Rows)
                    {
                        boundaries.Add(Convert.ToInt32(velocityGradeRow["Boundary"], CultureInfo.CurrentUICulture));
                    }
                    foreach (DataRow matrixRow in dt.Rows)
                    {
                        boundary = Convert.ToInt32(matrixRow["Boundary"], CultureInfo.CurrentUICulture);
                        if (!boundaries.Contains(boundary))
                        {
                            rowsToDelete.Add(matrixRow);
                        }
                    }
                    foreach (var matrixRow in rowsToDelete)
                    {
                        matrixRow.Delete();
                    }
                    dt.AcceptChanges();
                }
            }

            ROMethodAllocationVelocityAttributeSet attributeSet;
            attributeSet = method.MatrixAttributeSetValues;

            foreach (ROMethodAllocationVelocityMatrixVelocityGrade matrixVelocityGrade in attributeSet.MatrixGradeValues)
            {
                foreach (ROMethodAllocationVelocityMatrixCell matrixCell in matrixVelocityGrade.MatrixGradeCells)
                {
                    row = dt.NewRow();

                    row["SglRID"] = attributeSet.AttributeSet.Key;
                    row["Boundary"] = matrixVelocityGrade.VelocityGrade.Key;
                    row["SellThruIndex"] = matrixCell.SellThruIndex;
                    if (method.VelocityAction == eVelocityAction.ClearMatrix)
                    {
                        row["VelocityRule"] = System.DBNull.Value;
                        row["VelocityQty"] = System.DBNull.Value;
                    }
                    else if (attributeSet.OnHandRuleIsSet)
                    {
                        row["VelocityRule"] = attributeSet.OnHandRule.GetHashCode();
                        if (attributeSet.OnHandRuleIsSet)
                        {
                            row["VelocityQty"] = attributeSet.OnHandRuleValue;
                        }
                        else
                        {
                            row["VelocityQty"] = System.DBNull.Value;
                        }
                    }
                    else
                    {
                        row["VelocityRule"] = matrixCell.RuleType.GetHashCode();
                        if (matrixCell.RuleValueIsSet)
                        {
                            row["VelocityQty"] = matrixCell.RuleValue;
                        }
                    }

                    if (method.VelocityAction == eVelocityAction.ClearMatrix)
                    {
                        row["Stores"] = System.DBNull.Value;
                        row["AvgWOS"] = System.DBNull.Value;
                    }
                    else
                    {
                        row["Stores"] = matrixCell.NumberOfStores;
                        row["AvgWOS"] = matrixCell.AverageWOS;
                    }

                    dt.Rows.Add(row);
                }
            }

        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-3890 - JSmith

        //		/// <summary>
        //		/// Copy the data arrays from the data source tables
        //		/// </summary>
        //		public void CopyDataArrays(VelocityMethod newVelocityMethod, Session aSession, bool aCloneDateRanges)
        //		{
        //			
        //			if (_basisMdseData.Count > 0)
        //			{
        //				CopyBasis(newVelocityMethod);
        //			}
        //
        //			if (_basisTimeData.Count > 0)
        //			{
        //				CopySalesPeriod(newVelocityMethod, aSession, aCloneDateRanges);
        //			}
        //
        //			if (_gradeLowLimData.Count > 0)
        //			{
        //				CopyVelocityGrade(newVelocityMethod);
        //			}
        //
        //			if (_pctSellThruData.Count > 0)
        //			{
        //				CopySellThru(newVelocityMethod);
        //			}
        //
        //			if (_groupLvlMtrxData.Count > 0)
        //			{
        //				CopyMatrix(newVelocityMethod);
        //			}
        //		}
        //
        //		// ====================================
        //		// Copy the Basis Merchandise hashtable
        //		// ====================================
        //		private void CopyBasis(VelocityMethod newVelocityMethod)
        //		{
        //			newVelocityMethod._basisMdseData = new Hashtable();
        //			
        //			foreach(BasisMdseNode bmn in _basisMdseData.Values)
        //			{
        //				BasisMdseNode newbmn = bmn.Copy();
        //
        //				newVelocityMethod._basisMdseData.Add(newbmn.Sequence, newbmn);
        //			}
        //		}
        //
        //		// =====================================
        //		// Copy the Basis Time Periods hashtable
        //		// =====================================
        //		private void CopySalesPeriod(VelocityMethod newVelocityMethod, Session aSession, bool aCloneDateRanges)
        //		{
        //			newVelocityMethod._basisTimeData = new Hashtable();
        //
        //			foreach(BasisTimePrd btp in _basisTimeData.Values)
        //			{
        //				BasisTimePrd newbtp = btp.Copy(aSession, aCloneDateRanges);
        //
        //				newVelocityMethod._basisTimeData.Add(newbtp.Sequence, newbtp);
        //			}
        //		}
        //
        //		// ==========================================
        //		// Copy the Grades and Lower Limits hashtable
        //		// ==========================================
        //		private void CopyVelocityGrade(VelocityMethod newVelocityMethod)
        //		{
        //			newVelocityMethod._gradeLowLimData = new Hashtable();
        //
        //			newVelocityMethod._lowLimGradeData = new Hashtable();
        //
        //			newVelocityMethod._lowLimSortedGradeData = new SortedList();
        //
        //			foreach(GradeLowLimit gll in _gradeLowLimData.Values)
        //			{
        //				GradeLowLimit newgll = gll.Copy();
        //
        //				newVelocityMethod._gradeLowLimData.Add(newgll.Grade, newgll);
        //				newVelocityMethod._lowLimGradeData.Add(newgll.LowerLimit, newgll);
        //				newVelocityMethod._lowLimSortedGradeData.Add(-newgll.LowerLimit, newgll);
        //			}
        //		}
        //
        //		// ========================================
        //		// Copy the Pct Sell Thru Indices hashtable
        //		// ========================================
        //		private void CopySellThru(VelocityMethod newVelocityMethod)
        //		{
        //			newVelocityMethod._pctSellThruData = new Hashtable();
        //
        //			newVelocityMethod._pctSellThruSortedData = new SortedList();
        //
        //			foreach(PctSellThruIndex psti in _pctSellThruData.Values)
        //			{
        //				PctSellThruIndex newpsti = psti.Copy();
        //
        //				newVelocityMethod._pctSellThruData.Add(newpsti.SellThruIndex, newpsti);
        //				newVelocityMethod._pctSellThruSortedData.Add(-newpsti.SellThruIndex, newpsti);
        //			}
        //		}
        //
        //		// ==========================
        //		// Copy the Matrix hashtables
        //		// ==========================
        //		private void CopyMatrix(VelocityMethod newVelocityMethod)
        //		{
        //			newVelocityMethod._groupLvlMtrxData = new Hashtable();
        //			
        //			foreach (GroupLvlMatrix glm in _groupLvlMtrxData.Values)
        //			{
        //				GroupLvlMatrix newglm = glm.Copy();
        //				
        //				newVelocityMethod._groupLvlGradData = new Hashtable();
        //				foreach (GroupLvlGrade glg in _groupLvlGradData.Values)
        //				{
        //					GroupLvlGrade newglg = glg.Copy();
        //
        //					newVelocityMethod._groupLvlGradData.Add(newglg.Grade, newglg);
        //				}
        //				
        //				newVelocityMethod._groupLvlCellData = new Hashtable();
        //				foreach (GroupLvlCell glc in _groupLvlCellData.Values)
        //				{
        //					GroupLvlCell newglc = glc.Copy();
        //
        //					newVelocityMethod._groupLvlCellData.Add(newglc.Key, newglc);
        //				}
        //
        //				glm.GradeSales = newVelocityMethod._groupLvlGradData;
        //				glm.MatrixCells = newVelocityMethod._groupLvlCellData;
        //			}
        //		}

        //Begin TT#855-MD -jsobek -Velocity Enhancements
        private void PerformBalanceToHeader(HeaderDataValue hdv)
        {
            List<StoreDataValue> participatingStoreList = BalanceToHeader_DetermineParticipatingStores();
            if (participatingStoreList.Count > 0)
            {
                int remainingUnitsToAllocate = BalanceToHeader_DetermineRemainingUnitsToAllocate(hdv);
                int numberOfSpreadIterations = 2;
                int currentIteration = 1;

                while ((remainingUnitsToAllocate > 0) && (currentIteration <= numberOfSpreadIterations))
                {
                    BalanceToHeader_SpreadRemaining(ref participatingStoreList, ref remainingUnitsToAllocate);
                    currentIteration++;
                }
                //do final proportional spread if needed
                if (remainingUnitsToAllocate > 0)
                {
                    BalanceToHeader_DoFinalProportionalSpread(participatingStoreList, remainingUnitsToAllocate);
                }
            }
        }
        private List<StoreDataValue> BalanceToHeader_DetermineParticipatingStores()
        {
            List<StoreDataValue> participatingStoreList = new List<StoreDataValue>();

            foreach (StoreDataValue sdv in _storeData.Values)
            {
                if (BalanceToHeader_IsStoreAllowedToParticipate(sdv) == true)
                {
                    participatingStoreList.Add(sdv);
                }
            }

            return participatingStoreList;
        }
        private bool BalanceToHeader_IsStoreAllowedToParticipate(StoreDataValue sdv)
        {
            //Omit non-eligible stores
            //Omit reserve store
            //Omit stores that are marked as similar store model
            //Omit any stores where a user manually entered a quantity
            //Omit "out" stores
            //Omit stores that have a winning "quantity" rule applied from prior layers
            //Include stores with minimums applied
            //Include stores with a "Prior Header" rule applied
            //Omit any stores with quantity allocated equal to max quantity
            if (sdv.IsEligible == false)
            {
                return false;

            }
            if (sdv.IsReserve)
            {
                return false;
            }
            if (sdv.IsSimilarStoreModel)
                return false;

            if (sdv.IsManuallyAllocated)
            {
                return false;
            }

            // BEGIN TT#920-MD - AGallagher - Velocity - Velocity Header Balance Checked - Over Capacity Stores participate in the Balance.  Would not expect over capacity stores to participate.
            if (sdv.Capacity == 0)
            {
                return false;
            }
            // END TT#920-MD - AGallagher - Velocity - Velocity Header Balance Checked - Over Capacity Stores participate in the Balance.  Would not expect over capacity stores to participate.

            int willship = sdv.WillShip;
            int storeMax = sdv.VelocityGradeMaximum;
            if (sdv.WillShip >= storeMax)
            {
                return false;
            }

            eVelocityRuleType ruleType = sdv.RuleType;
            if (ruleType == eVelocityRuleType.Out)
            {
                return false;
            }
            if (BalanceToHeader_IsVelocityRuleAQuantityRule(ruleType))
            {
                return false;
            }
            if (BalanceToHeader_IsVelocityRuleAPriorHeaderRule(ruleType))
            {
                return false;
            }

            return true;
        }
        private bool BalanceToHeader_IsVelocityRuleAQuantityRule(eVelocityRuleType ruleType)
        {
            if (ruleType == eVelocityRuleType.AbsoluteQuantity)
                return true;
            else if (ruleType == eVelocityRuleType.ColorMaximum)
                return true;
            else if (ruleType == eVelocityRuleType.ForwardWeeksOfSupply)
                return true;
            else if (ruleType == eVelocityRuleType.Maximum)
                return true;
            else if (ruleType == eVelocityRuleType.ShipUpToQty)
                return true;
            else if (ruleType == eVelocityRuleType.WeeksOfSupply)
                return true;
            else
                return false;
        }
        private bool BalanceToHeader_IsVelocityRuleAPriorHeaderRule(eVelocityRuleType ruleType)
        {
            if (ruleType == eVelocityRuleType.Exact)
                return true;
            else if (ruleType == eVelocityRuleType.Fill)
                return true;
            else if (ruleType == eVelocityRuleType.ProportionalAllocated)
                return true;
            else
                return false;
        }
        private int BalanceToHeader_DetermineRemainingUnitsToAllocate(HeaderDataValue hdv)
        {
            int intTtlQtyToAllocate;
            switch (Component.ComponentType)
            {
                case eComponentType.Total:
                    intTtlQtyToAllocate = hdv.AloctnProfile.TotalUnitsToAllocate;
                    intTtlQtyToAllocate = intTtlQtyToAllocate - _TotalPreSizeAllocated;
                    if (intTtlQtyToAllocate < 1)
                    { 
                        intTtlQtyToAllocate = 0; 
                    }

                    break;
                case eComponentType.Bulk:
                    intTtlQtyToAllocate = hdv.AloctnProfile.BulkUnitsToAllocate;
                    intTtlQtyToAllocate = intTtlQtyToAllocate - _TotalPreSizeAllocated;
                    if (intTtlQtyToAllocate < 1)
                    { 
                        intTtlQtyToAllocate = 0; 
                    }
                    break;
                case eComponentType.SpecificPack:
                    string packName = ((AllocationPackComponent)Component).PackName;
                    intTtlQtyToAllocate = hdv.AloctnProfile.GetPacksToAllocate(packName);
                    break;
                case eComponentType.SpecificColor:
                    int colorRID = ((AllocationColorOrSizeComponent)Component).ColorRID;
                    intTtlQtyToAllocate = hdv.AloctnProfile.GetColorUnitsToAllocate(colorRID);
                    intTtlQtyToAllocate = intTtlQtyToAllocate - _TotalPreSizeAllocated;
                    if (intTtlQtyToAllocate < 1)
                    { 
                        intTtlQtyToAllocate = 0; 
                    }
                    break;
                default:
                    intTtlQtyToAllocate = hdv.AloctnProfile.TotalUnitsToAllocate;
                    break;
            }
            int intWillShipTotal = 0;
            foreach (StoreDataValue sdv in _storeData.Values)
            {
                intWillShipTotal = intWillShipTotal + sdv.WillShip;
            }

            int remainingUnitsToAllocate;
            remainingUnitsToAllocate = intTtlQtyToAllocate - intWillShipTotal;
            if (remainingUnitsToAllocate < 0)
            {
                remainingUnitsToAllocate = 0;
            }

            return remainingUnitsToAllocate;
        }
        /// <summary>
        /// spread like need based on velocity grade index
        /// </summary>
        /// <param name="participatingStoreList"></param>
        /// <param name="remainingUnitsToAllocate"></param>
        private void BalanceToHeader_SpreadRemaining(ref List<StoreDataValue> participatingStoreList, ref int remainingUnitsToAllocate)
        {
            int remainingUnits = remainingUnitsToAllocate;

            //Sort the store list from smallest index to largest index - so the smaller indexes do not lose out and get nothing
            participatingStoreList.Sort(BalanceToHeader_CompareStoreDataValuesByVelocityIndex);
        
            //Calculate the basis total - it is the sum of all velocity indexes in play
            double aBasisTotal = 0;
            foreach (StoreDataValue sdv in participatingStoreList)
            {
                //Both Chain and Set Grade Indexes are set to the same thing at this point, so it doesnt matter which one we pick.
                //They are determined by this method in the Allocation Subtotal profile: GetStoreVelocityGradeIDX
                double velocityGradeIndex = sdv.TotChnVelocityGradeIDX; 
               
             
                aBasisTotal += velocityGradeIndex;
            }

            foreach (StoreDataValue sdv in participatingStoreList)
            {
                int additionalUnitsToAllocateToThisStore = 0;

                double velocityGradeIndex = sdv.TotChnVelocityGradeIDX; 

                if (aBasisTotal > 0)
                {
                    additionalUnitsToAllocateToThisStore =
                        (int)((velocityGradeIndex
                        * (double)remainingUnits
                        / aBasisTotal) + 0.5d);
                    if (additionalUnitsToAllocateToThisStore > remainingUnits)
                    {
                        additionalUnitsToAllocateToThisStore = remainingUnits;
                    }

                    BalanceToHeader_ApplyMinMaxAndCapacity(ref additionalUnitsToAllocateToThisStore, remainingUnits, sdv);  // TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity
                }

                remainingUnits -= additionalUnitsToAllocateToThisStore;
                aBasisTotal -= velocityGradeIndex;

                //Add to the WillShip total
                sdv.WillShip += additionalUnitsToAllocateToThisStore;
            }

            //remove stores that are now at maximum quantity from the list
            int[] removeStoreFlags = new int[participatingStoreList.Count];
            int i = 0;
            foreach (StoreDataValue sdv in participatingStoreList)
            {
                int storeMax = sdv.VelocityGradeMaximum;
                if (sdv.WillShip >= storeMax)
                {
                    //participatingStoreList.Remove(sdv);
                    removeStoreFlags[i] = 1;
                }
                // BEGIN TT#920-MD - AGallagher - Velocity - Velocity Header Balance Checked - Over Capacity Stores participate in the Balance.  Would not expect over capacity stores to participate.
                if (sdv.WillShip >= sdv.Capacity)
                {
                    removeStoreFlags[i] = 1;
                }
                // END TT#920-MD - AGallagher - Velocity - Velocity Header Balance Checked - Over Capacity Stores participate in the Balance.  Would not expect over capacity stores to participate.

                i++;
            }
            for (int j = 0; j < participatingStoreList.Count; j++)
            {
                if (removeStoreFlags[j] == 1)
                {
                    participatingStoreList.RemoveAt(j);
                }
            }

            remainingUnitsToAllocate = remainingUnits;
        }
        private void BalanceToHeader_ApplyMinMaxAndCapacity(ref int additionalUnitsToAllocateToThisStore, int remainingUnits, StoreDataValue sdv) // TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity
        {
            //If less than store grade minimum - raise upto minimum only if the velocity rule "Minimum" is applied, otherwise set to zero for that store
            int storeMin = sdv.VelocityGradeMinimum;
            eVelocityRuleType ruleType = sdv.RuleType;
            if (ruleType == eVelocityRuleType.AdMinimum)
            {
                storeMin = sdv.VelocityGradeAdMinimum;
            }

            bool honorMin = false;
            bool honorMax = false;

            if (_ApplyMinMaxInd == 'N') //None
            {
                if (ruleType == eVelocityRuleType.AdMinimum || ruleType == eVelocityRuleType.Minimum)
                {
                    honorMin = true;
                }
            }
            else
            {
                //Store Grade Min OR Velocity Grade Min
                honorMin = true;
                honorMax = true;
            }

            if (honorMin)
            {
                int newWillShip = sdv.WillShip + additionalUnitsToAllocateToThisStore;	// TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity
                if (newWillShip < storeMin)												// TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity
                {
                    if (remainingUnits >= storeMin)
                    {
                        additionalUnitsToAllocateToThisStore = storeMin;
                    }
                    else
                    {
                        additionalUnitsToAllocateToThisStore = 0;
                    }
                }
            }

            // Begin TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity
            if (sdv.Capacity > 0)
            {
                int newWillShip = sdv.WillShip + additionalUnitsToAllocateToThisStore;
                
                if (sdv.Capacity < storeMin && honorMin)
                {
                    additionalUnitsToAllocateToThisStore = 0;
                }
                else if (sdv.Capacity < newWillShip)
                {
                    additionalUnitsToAllocateToThisStore = sdv.Capacity - sdv.WillShip;
                }
            }
            // End TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity

            if (honorMax)
            {
                //Do not go over the store grade maximum either
                int storeMax = sdv.VelocityGradeMaximum;
                int newWillShip = sdv.WillShip + additionalUnitsToAllocateToThisStore;
                if (newWillShip > storeMax)
                {
                    additionalUnitsToAllocateToThisStore = storeMax - sdv.WillShip;
                }
            }
        }
        private void BalanceToHeader_DoFinalProportionalSpread(List<StoreDataValue> participatingStoreList, int remainingUnitsToAllocate)
        {
            if (remainingUnitsToAllocate <= 0)
                return;

            //calculate the proportional index for each participating store
            //double[] proportionalIndexes = new double[participatingStoreList.Count];
            int totalAvailable = 0;
            foreach (StoreDataValue sdv in participatingStoreList)
            {
                int storeMax = sdv.VelocityGradeMaximum;
                int willShip = sdv.WillShip;
                int availableForThisStore = storeMax - willShip;
                totalAvailable += availableForThisStore;
            }
            if (totalAvailable > 0)
            {
                double aBasisTotal = 0;
                int i = 0;
                foreach (StoreDataValue sdv in participatingStoreList)
                {
                    int storeMax = sdv.VelocityGradeMaximum;
                    int willShip = sdv.WillShip;
                    int availableForThisStore = storeMax - willShip;
                    double proportionalIndex = (Convert.ToDouble(availableForThisStore) / Convert.ToDouble(totalAvailable));
                    //proportionalIndexes[i] = proportionalIndex;
                    sdv.VelocityBalanceHeaderProportionalIndex = proportionalIndex;
                    aBasisTotal += proportionalIndex;
                    i++;
                }
                //Sort stores by smallest proportional index first, so stores with smaller indexes do not lose out and get nothing
                participatingStoreList.Sort(BalanceToHeader_CompareStoreDataValuesByProportionalIndex);
                foreach (StoreDataValue sdv in participatingStoreList)
                {
                    int additionalUnitsToAllocateToThisStore = 0;

                    double proportionalIndex = sdv.VelocityBalanceHeaderProportionalIndex;

                    if (aBasisTotal > 0)
                    {
                        additionalUnitsToAllocateToThisStore =
                            (int)((proportionalIndex
                            * (double)remainingUnitsToAllocate
                            / aBasisTotal) + 0.5d);
                        if (additionalUnitsToAllocateToThisStore > remainingUnitsToAllocate)
                        {
                            additionalUnitsToAllocateToThisStore = remainingUnitsToAllocate;
                        }
                        BalanceToHeader_ApplyMinMaxAndCapacity(ref additionalUnitsToAllocateToThisStore, remainingUnitsToAllocate, sdv);    // TT#4787 - stodd - GA- Velocity with Header Balance checked - store 1105 exceeds capacity
                    }
                    remainingUnitsToAllocate -= additionalUnitsToAllocateToThisStore;
                    aBasisTotal -= proportionalIndex;

                    //Add to the WillShip total
                    sdv.WillShip += additionalUnitsToAllocateToThisStore;
                }
            }

        }
        private static int BalanceToHeader_CompareStoreDataValuesByProportionalIndex(StoreDataValue x, StoreDataValue y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                if (y == null)
                // ...and y is null, x is greater
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare
                    int retval = x.VelocityBalanceHeaderProportionalIndex.CompareTo(y.VelocityBalanceHeaderProportionalIndex);
                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        private static int BalanceToHeader_CompareStoreDataValuesByVelocityIndex(StoreDataValue x, StoreDataValue y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                if (y == null)
                // ...and y is null, x is greater
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare
                    int retval = x.TotChnVelocityGradeIDX.CompareTo(y.TotChnVelocityGradeIDX);
                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        //End TT#855-MD -jsobek -Velocity Enhancements 

        //BEGIN tt#152 - Velocity Balance - apicchetti
        private void PerformBalance(HeaderDataValue hdv)
        {

            //grade list
            ArrayList alGrades = hdv.AloctnProfile.GradeList;

            //list of lists
            ArrayList alLists = new ArrayList();

            //get the totals and will ship totals
            // begin TT#509 Velocity Balance not working
            //int intTtlUnitsToAllocate = hdv.AloctnProfile.TotalUnitsToAllocate;
            int intTtlQtyToAllocate;
            int multiple = 1;
            switch (Component.ComponentType)
            {
                case eComponentType.Total:
                    multiple = hdv.AloctnProfile.GetMultiple(Component);
                    intTtlQtyToAllocate = hdv.AloctnProfile.TotalUnitsToAllocate;
                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                    intTtlQtyToAllocate = intTtlQtyToAllocate - _TotalPreSizeAllocated;
                    if (intTtlQtyToAllocate < 1)
                        { intTtlQtyToAllocate = 0; }
                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                    break;
                case eComponentType.Bulk:
                    multiple = hdv.AloctnProfile.BulkMultiple;
                    intTtlQtyToAllocate = hdv.AloctnProfile.BulkUnitsToAllocate;
                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                    intTtlQtyToAllocate = intTtlQtyToAllocate - _TotalPreSizeAllocated;
                    if (intTtlQtyToAllocate < 1)
                        { intTtlQtyToAllocate = 0; }
                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                    break;
                case eComponentType.SpecificPack:
                    string packName = ((AllocationPackComponent)Component).PackName;
                    //multiple = hdv.AloctnProfile.GetPackMultiple(packName);
                    multiple = 1;  // TT#509  (ship quantities are in number of multiples.
                    intTtlQtyToAllocate = hdv.AloctnProfile.GetPacksToAllocate(packName);
                    break;
                case eComponentType.SpecificColor:
                    //                  NOTE: GetColorMulitple may be mispelled
                    int colorRID = ((AllocationColorOrSizeComponent)Component).ColorRID;
                    multiple = hdv.AloctnProfile.GetColorMulitple(colorRID);
                    intTtlQtyToAllocate = hdv.AloctnProfile.GetColorUnitsToAllocate(colorRID);
                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                    intTtlQtyToAllocate = intTtlQtyToAllocate - _TotalPreSizeAllocated;
                    if (intTtlQtyToAllocate < 1)
                        { intTtlQtyToAllocate = 0; }
                    // TT#1391 -Jellis/AlanG -  TMW Balance Size With Constraints Other Options 
                    break;
                default:
                    multiple = 1;
                    intTtlQtyToAllocate = hdv.AloctnProfile.TotalUnitsToAllocate;
                    break;
            }
            // end TT509 Velocity Balance not working 

            int intWillShipTotal = 0;
            foreach (StoreDataValue sdv in _storeData.Values)
            {
                //calculate overall will ship total
                intWillShipTotal = intWillShipTotal + sdv.WillShip;

                //set initial values
                // BEGIN TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
                //sdv.InitialRuleQuantity = sdv.RuleQty;
                //sdv.InitialRuleType = sdv.RuleType;
                //sdv.InitialWillShip = sdv.WillShip;
                // END TT#842 - AGallagher - Velocity - Velocity Balance - with Layers
            }

            //if (intWillShipTotal > intTtlUnitsToAllocate)  // TT#509 Velocity Balance not working
            if (intWillShipTotal > intTtlQtyToAllocate)      // TT#509 Velocity Balance not working
            {
                //over allocated amount
                //int intOverAmt = intWillShipTotal - intTtlUnitsToAllocate;  // TT#509 Velocity Balance not working
                int intOverAmt = intWillShipTotal - intTtlQtyToAllocate;      // TT#509 Velocity Balance not working

                //get stores to sort
                ArrayList alNoOnHand = new ArrayList();
                ArrayList alWithOnHand = new ArrayList();

                //enumerate and loop thru the store values
                IDictionaryEnumerator storesEnum = _storeData.GetEnumerator();
                while (storesEnum.MoveNext())
                {
                    StoreDataValue sStore = (StoreDataValue)storesEnum.Value;

                    if (sStore.WillShip != 0)
                    {
                        // if (sStore.BasisOnHand == 0)       // TT#509 Velocity Balance not working
                        // begin TT#647 Velocity Balance incorrectly identifies noonhand stores
                        //if (sStore.BasisOHandIT == 0)         // TT#509 Velocity Balance not working
                        if (sStore.BasisOHandIT == 0
                            // Begin TT#773 - JSmith - Velocity -> balance is checked-> 3 stores with no on hand are assigned a rule- should be considered first before stores in the matrix - they are not
                            //&& sStore.BasisSales <- 0)
                            && sStore.BasisSales <= 0)
                            // End TT#773
                            // end TT#647 Velocity Balance incorrectly identifies noonhand stores
                        {
                            if (sStore.IsEligible == true)
                            {
                                StoreProfile sProfile = new StoreProfile(Convert.ToInt32(storesEnum.Key));
                                alNoOnHand.Add(sStore);
                            }
                        }
                        else
                        {
                            if (sStore.IsEligible == true)
                            {
                                StoreProfile sProfile = new StoreProfile(Convert.ToInt32(storesEnum.Key));
                                alWithOnHand.Add(sStore);
                            }
                        }
                    }
                }

                //sort stores
                MIDGenericSortItem[] sortArray = new MIDGenericSortItem[alNoOnHand.Count];
                for (int intNoOnHand = 0; intNoOnHand < alNoOnHand.Count; intNoOnHand++)
                {
                    sortArray[intNoOnHand].Item = intNoOnHand;
					// Begin TT349 - stodd - Highest volume store not getting rule in balance
					// Begin TT353 - incorrect sorting in velocity balance
                    sortArray[intNoOnHand].SortKey = new double[3];        // TT#509  Velocity Balance not working
					// End TT353 - incorrect sorting in velocity balance
					// End TT349 - stodd - Highest volume store not getting rule in balance

                    StoreDataValue store_value = (StoreDataValue)alNoOnHand[intNoOnHand];

					// Begin TT349 - stodd - Highest volume store not getting rule in balance
                    //sortArray[intNoOnHand].SortKey[0] = -store_value.BasisGradeIDX;
					// Begin TT353 - incorrect sorting in velocity balance
					sortArray[intNoOnHand].SortKey[0] = store_value.BasisSales;
					// End TT353 - incorrect sorting in velocity balance
                    sortArray[intNoOnHand].SortKey[1] = store_value.SGLChnPctSellThruIndex;
                    sortArray[intNoOnHand].SortKey[2] = _applicationTransaction.GetRandomDouble();  // TT#509 Velocity Balance not working
					// End TT349 - stodd - Highest volume store not getting rule in balance

                }
                Array.Sort(sortArray, new MIDGenericSortDescendingComparer());

                MIDGenericSortItem[] sortArray1 = new MIDGenericSortItem[alWithOnHand.Count];
                for (int intWithOnHand = 0; intWithOnHand < alWithOnHand.Count; intWithOnHand++)
                {
                    sortArray1[intWithOnHand].Item = intWithOnHand;
					// Begin TT349 - stodd - Highest volume store not getting rule in balance
					// Begin TT353 - incorrect sorting in velocity balance
                    sortArray1[intWithOnHand].SortKey = new double[3];  // TT#509 Velocity Balance not working
					// End TT353 - incorrect sorting in velocity balance
					// End TT349 - stodd - Highest volume store not getting rule in balance

                    StoreDataValue store_value = (StoreDataValue)alWithOnHand[intWithOnHand];

					// Begin TT349 - stodd - Highest volume store not getting rule in balance
                    //sortArray1[intWithOnHand].SortKey[0] = -store_value.BasisGradeIDX;
					// Begin TT353 - incorrect sorting in velocity balance
					sortArray1[intWithOnHand].SortKey[0] = store_value.BasisSales;
					// End TT353 - incorrect sorting in velocity balance
                    sortArray1[intWithOnHand].SortKey[1] = store_value.SGLChnPctSellThruIndex;
                    sortArray1[intWithOnHand].SortKey[2] = _applicationTransaction.GetRandomDouble();  // TT#509 Velocity Balance not working
                    // End TT349 - stodd - Highest volume store not getting rule in balance

                }
                Array.Sort(sortArray1, new MIDGenericSortDescendingComparer());

                //concatenate the lists
                ArrayList alSortedStores = new ArrayList();
                for (int intSortStore = 0; intSortStore < sortArray.Length; intSortStore++)
                {
                    alSortedStores.Add(alNoOnHand[sortArray[intSortStore].Item]);
                    // begin TT#509 Velocity Balance not working
                    //StoreDataValue sdv_x = (StoreDataValue)alNoOnHand[sortArray[intSortStore].Item];
                    //Debug.Print(sdv_x.StoreRID + "," + sdv_x.BasisGradeIDX + "," + sdv_x.BasisOnHand + "," 
                    //    + sdv_x.SGLChnPctSellThruIndex + "," + sdv_x.RuleQty + "," + sdv_x.BasisSales);
                    // end TT#509 Velocity Balance not working
                }
                for (int intSortStore = 0; intSortStore < sortArray1.Length; intSortStore++)
                {
                    alSortedStores.Add(alWithOnHand[sortArray1[intSortStore].Item]);
                    // begin TT#509 Velocity Balance not working
                    //StoreDataValue sdv_y = (StoreDataValue)alWithOnHand[sortArray1[intSortStore].Item];
                    //Debug.Print(sdv_y.StoreRID + "," + sdv_y.BasisGradeIDX + "," + sdv_y.BasisOnHand + "," 
                        //+ sdv_y.SGLChnPctSellThruIndex + "," + sdv_y.RuleQty + "," + sdv_y.BasisSales);
                    // end TT#509 Velocity Balance not working
                }

                //traverse the list, from the bottom up
                //bool blRemainderToSpread = false; // TT#327 Velocity Balance Option does not observe Multiple
                //int intRemainderToSpread = 0;     // TT#327 Velocity Balance Option does not observe Multiple
                for(int intStore = alSortedStores.Count - 1; intStore >= 0; intStore--)
                {
                    StoreDataValue sStore = (StoreDataValue)alSortedStores[intStore];

					//Debug.WriteLine(intStore + " " + sStore.StoreRID + " SET " + sStore.GrpLvlRID + " GR IDX " + sStore.BasisGradeIDX + " B SALES " + sStore.BasisSales + 
					//    "SELL THRU " + sStore.SGLChnPctSellThruIndex + " GR " + sStore.Grade);

                    intWillShipTotal = intWillShipTotal - sStore.WillShip;
                    sStore.WillShip = 0;
                    sStore.RuleType = eVelocityRuleType.None;
                    sStore.RuleTypeQty = 0;   // TT#1101 - AGallagher - When Balance is active and Velocity Rule of None should have Velocity Rule Type Qty of zero
                    sStore.RuleQty = 0;

                    // begin TT#327 Velocity Balance Option does not observe Multiple
                    //if (intWillShipTotal > intTtlUnitsToAllocate)   // TT#509 Velocity Balance not working
                    if (intWillShipTotal > intTtlQtyToAllocate)       // TT#509 Velocity Balance not working
                    {
                        continue;
                    }
                    break;
                    //if (intWillShipTotal == intTtlUnitsToAllocate)
                    //{
                    //    break;
                    //}
                    //else if (intWillShipTotal < intTtlUnitsToAllocate)
                    //{
                    //   intRemainderToSpread = intTtlUnitsToAllocate - intWillShipTotal;
                    //    blRemainderToSpread = true;
                    //    break;
                    //}
                    // end TT#327 Velocity Balance Option does not observe Multiple
                }

                // begin TT#327 Velocity Balance Option does not observe Multiple
                // begin TT#509 Velocity Balance not working as expected
                //if (intWillShipTotal != intTtlUnitsToAllocate) 
                //{
                    //int multiple = 1;
                    //switch (Component.ComponentType)
                    //{
                    //    case eComponentType.Total:
                    //        multiple = _alocProfile.GetMultiple(Component);
                    //        break;
                    //    case eComponentType.Bulk:
                    //        multiple = _alocProfile.BulkMultiple;
                    //        break;
                    //    case eComponentType.SpecificPack:
                    //        multiple = _alocProfile.GetPackMultiple(((AllocationPackComponent)Component).PackName);
                    //        break;
                    //    case eComponentType.SpecificColor:
                    //        //                  NOTE: GetColorMulitple may be mispelled
                    //        multiple = _alocProfile.GetColorMulitple(((AllocationColorOrSizeComponent)Component).ColorRID);
                    //        break;
                    //    default:
                    //        multiple = 1;
                    //        break;
                    //}
                    //MIDGenericSortItem[] sortStores = new MIDGenericSortItem[_storeData.Count];
                    //int storePosition = 0;
                    //foreach (StoreDataValue sdv in _storeData.Values)
                    //{
                    //    sortStores[storePosition].Item = sdv.StoreRID;
                    //    sortStores[storePosition].SortKey = new double[1];
                    //    sortStores[storePosition].SortKey[0] = -sdv.WillShip;  // negate the willship quantity so the sort is ascending
                    //    storePosition++;
                    //}
                    //Array.Sort(sortStores, new MIDGenericSortDescendingComparer());

                // Begin TT#851 - RMatelic - Velocity Method with Balance checked and run in a Task List will not stop running
                //if (intWillShipTotal != intTtlQtyToAllocate)
                if (intWillShipTotal > 0 && intWillShipTotal != intTtlQtyToAllocate)
                // End TT#851
                {
                    // end TT#509 Velocity Balance not working as expected
                    int willShip;
                    int willShipHigh;
                    int willShipLow;
                    // begin TT#509 Velocity Balance not working as expected
                    //foreach (MIDGenericSortItem mgsi in sortStores)
                    //{

                    //    StoreDataValue sdv = (StoreDataValue)_storeData[mgsi.Item];
                    // begin TT#527 Velocity Balance Null Reference
                    //for (int item = 0; item < alSortedStores.Count; item++)
                    //{
                    //    StoreDataValue sdv = (StoreDataValue)_storeData[item];

                    // begin TT#509 Velocity balance:  Change to give each store 1 additional multiple in rank order until left over gone (repeat loops till gone)
                    int countStoresWithUnits = 0;
                    foreach (StoreDataValue sdv in alSortedStores)
                    {
                        if (sdv.WillShip > 0)
                        {
                            countStoresWithUnits++;
                        }
                    }
                    // Begin TT#851 - RMatelic - Velocity Method with Balance checked and run in a Task List will not stop running
                    if (countStoresWithUnits > 0)
                    {
                    // End TT#851
                        while (intWillShipTotal < intTtlQtyToAllocate)
                        {
                            int leftOverQty = intTtlQtyToAllocate - intWillShipTotal;
                            int qtyToGive = multiple;
                            if (countStoresWithUnits > 0)
                            {
                                qtyToGive = leftOverQty / countStoresWithUnits;
                                if (qtyToGive < multiple)
                                {
                                    qtyToGive = multiple;
                                }
                                else
                                {
                                    qtyToGive = qtyToGive / multiple;
                                    qtyToGive = qtyToGive * multiple;
                                }
                                if (qtyToGive > leftOverQty)
                                {
                                    qtyToGive = leftOverQty;
                                }
                            }

                            foreach (StoreDataValue sdv in alSortedStores)
                            {
                                if (sdv.WillShip > 0)
                                {
                                    intWillShipTotal += qtyToGive;
                                    sdv.WillShip += qtyToGive;
                                    leftOverQty -= qtyToGive;
                                    if (leftOverQty < 1)
                                    {
                                        if (leftOverQty < 0)
                                        {
                                            //sdv.WillShip = sdv.WillShip - leftOverQty; // put back any reduction // TT#406 Unhandled exception when checking / unchecking balance
                                            sdv.WillShip = sdv.WillShip + leftOverQty;   // put back any reduction // TT#406 Unhandled exception when checking / unchecking balance
                                            intWillShipTotal += leftOverQty;
                                        }
                                        break;
                                    }
                                }
                                // Begin TT#851 - RMatelic - Velocity Method with Balance checked and run in a Task List will not stop running
                                //else
                                //{
                                //    break;
                                //}
                                // End TT#851
                            }
                        }
                    }
                }
                //    foreach (StoreDataValue sdv in alSortedStores)    
                //    {
                //        // end TT#527 Velocity Balance Null Reference
                //    // end TT#509 Velocity Balance not working as expected
                //        if (intWillShipTotal > 0)
                //        {
                //            willShip =
                //                (int)(((double)sdv.WillShip
                //                * (double)intTtlQtyToAllocate  // TT#509 Velocity Balance not working
                //                / (double)intWillShipTotal)
                //                + .5d);
                //            // begin TT#509 Velocity Balance not working
                //            //       have already rounded for the multiple of 1.
                //            if (multiple > 1)
                //            {
                //                willShipLow =
                //                    willShip
                //                    / multiple;
                //                willShipLow =
                //                    willShipLow
                //                    * multiple;
                //                willShipHigh =
                //                    willShipLow + multiple;
                //                if ((willShip - willShipLow) < (willShipHigh - willShip))
                //                {
                //                    willShip = willShipLow;
                //                }
                //                else
                //                {
                //                    willShip = willShipHigh;
                //                }
                //            }
                //            if (willShip == sdv.WillShip
                //                && sdv.WillShip > 0
                //                && intWillShipTotal > 0
                //                && intWillShipTotal < intTtlQtyToAllocate)
                //            {
                //                willShip = willShip + multiple;
                //            }
                //            // end TT#509 Velocity Balance not working 
                //            if (willShip > intTtlQtyToAllocate)  // TT#509 Velocity Balance not working
                //            {
                //                willShip = intTtlQtyToAllocate;  // TT#509 Velocity Balance not working
                //            }
                //        }
                //        else
                //        {
                //            willShip = 0;
                //        }

                //        intWillShipTotal -= sdv.WillShip;
                //        sdv.WillShip = willShip;
                //        intTtlQtyToAllocate -= sdv.WillShip;    // TT#509 Velocity Balance not working
                //    }

                //}
                // end TT#509 Velocity Balance change balance


                // //nix the zero (0) allocated stores
                //ArrayList alSortedAllocatedStores = new ArrayList();
                //foreach (StoreDataValue sStore in alSortedStores)
                //{
                //    if (sStore.WillShip != 0)
                //    {
                //        alSortedAllocatedStores.Add(sStore);
                //    }
                //}
                //
                // //spread the remainder if there is one
                //bool blMoreToSpread = true;
                //if (blRemainderToSpread == true)
                //{
                //    while (blMoreToSpread == true)
                //    {
                //        foreach (StoreDataValue sStore in alSortedAllocatedStores)
                //        {
                //            double already_allocated = Convert.ToDouble(sStore.WillShip);
                //            double overage_to_allocate = (already_allocated * intRemainderToSpread) / intTtlUnitsToAllocate;
                //            int intOver = Convert.ToInt32(Math.Ceiling(overage_to_allocate));
                //            sStore.WillShip = sStore.WillShip + intOver;
                //            intRemainderToSpread = intRemainderToSpread - intOver;
                //            if (intRemainderToSpread <= 0)
                //            {
                //                blMoreToSpread = false;
                //                break;
                //            }
                //            intTtlUnitsToAllocate = intTtlUnitsToAllocate - intOver;
                //        }
                //
                //        if (alSortedAllocatedStores.Count == 0)
                //        {
                //            blMoreToSpread = false;
                //            break;
                //        }
                //    }
                //}
                // end TT#327 Velocity Balance Option does not observe Multiple
            }
        }
        //END tt#152 - Velocity Balance - apicchetti
    }


}
