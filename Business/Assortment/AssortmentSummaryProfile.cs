using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Configuration;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;


namespace MIDRetail.Business
{
	public class AssortmentSummaryProfile : Profile
	{
		private AssortmentSummary _assortmentSummary;
		private StoreGroupProfile _sgp;
		private ProfileList _gradeList;
		private SessionAddressBlock _sab;
        private eStoreAverageBy _storeAverageBy;
		private ApplicationSessionTransaction _trans;
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//private AssortmentBasisReader _basisReader;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		private ProfileList _storeProfileList;
		private List<int> _storeList;
		private List<int> _storeValueList;
		private List<int> _storeSetList = new List<int>();
		private List<double> _storeIndexList = new List<double>();
		private List<double> _avgStoreList = new List<double>();
		private List<int> _storeIntransitList = new List<int>();
		private List<int> _storeNeedList = new List<int>();
		private List<int> _storeOnHandList = new List<int>();	// TT#831-MD - Stodd - Need / Intransit not displayed
        private List<int> _storeVSWOnHandList = new List<int>();
		private List<int> _storeGradeBoundaryList = new List<int>();
        private List<int> _storePlanSalesList = new List<int>();  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
        private List<int> _storePlanStockList = new List<int>();  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
		private double _totalIntransit = 0;		// TT#831-MD - Stodd - Need / Intransit not displayed
		private double _totalOnHand = 0;		// TT#831-MD - Stodd - Need / Intransit not displayed
        private double _totalVSWOnHand = 0;
		private double _totalNeed = 0;			// TT#831-MD - Stodd - Need / Intransit not displayed
		private double _totalPctNeed = 0;		// TT#831-MD - Stodd - Need / Intransit not displayed
		private int _totalSales = 0;			// TT#831-MD - Stodd - Need / Intransit not displayed

		private List<decimal> _storePctNeedList = new List<decimal>();	// TT#831-MD - Stodd - Need / Intransit not displayed
		private Hashtable _setAvgHash = new Hashtable();
		private Hashtable _setTotalHash = new Hashtable();
		private Hashtable _setStoreCountHash = new Hashtable();
		private Hashtable _storeSetHash = new Hashtable();
		private Hashtable _listToStoreHash = new Hashtable();
		private Hashtable _storeToListHash = new Hashtable();
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		private SetGradeStoreXRef _setGradeStoreXRef;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		private DataTable _dtAssortmentStoreSummary;
		private Header _headerAssortData;
		//private List<bool> _isStoreEligibleList = new List<bool>();
		private Hashtable _storeEligibilityHash = new Hashtable();
		private int _anchorNodeRid;
		private AssortmentProfile _assortmentProfile;
		// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
		private eAssortmentBasisLoadedFrom _basisLoadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
        private StoreSalesITHorizon _storeSalesITHorizon; // TT#4345 - MD - Jellis - GA VSW Calculated incorrectly

		// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window

		public AssortmentSummaryProfile(AssortmentProfile ap, SessionAddressBlock sab, ApplicationSessionTransaction trans, StoreGroupProfile sgp, ProfileList gradeList, eStoreAverageBy avgBy)
			: base(ap.Key)
		{
			_sgp = sgp;
			_gradeList = gradeList;
			_sab = sab;
            _storeAverageBy = avgBy;
			_trans = trans;
			_anchorNodeRid = ap.AssortmentAnchorNodeRid;
			_assortmentProfile = ap;
			_assortmentSummary = new AssortmentSummary(this, sgp, gradeList);	// TT#1198-MD - stodd - cannot open assortment - 
			_headerAssortData = new Header();
			_dtAssortmentStoreSummary = _headerAssortData.GetAssortmentStoreSummary(Key);
            _storeSalesITHorizon = new StoreSalesITHorizon(); // TT#4345 - MD - Jellis - GA VSW Calculated incorrectly
            BuildSummary(_sgp.Key);
		}

		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        public AssortmentSummaryProfile(AssortmentProfile ap, SessionAddressBlock sab, ApplicationSessionTransaction trans)
            : base(ap.Key)
        {
            _sab = sab;
            _trans = trans;
            _anchorNodeRid = ap.AssortmentAnchorNodeRid;
            _assortmentProfile = ap;
            _storeSalesITHorizon = new StoreSalesITHorizon(); // TT#4345 - MD - Jellis - GA VSW Calculated incorrectly
        }
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review

		public AssortmentSummaryProfile(SessionAddressBlock sab, StoreGroupProfile sgp, ProfileList gradeList)
			: base(Include.NoRID)
		{
			_sgp = sgp;
			_gradeList = gradeList;
			_sab = sab;
            _storeAverageBy = eStoreAverageBy.AllStores;
			//_trans = trans;
			_assortmentSummary = new AssortmentSummary(this, sgp, gradeList);	// TT#1198-MD - stodd - cannot open assortment - 
			_headerAssortData = new Header();
			_dtAssortmentStoreSummary = _headerAssortData.GetEmptyAssortmentStoreSummaryDataTable();
            _storeSalesITHorizon = new StoreSalesITHorizon(); // TT#4345 - MD - Jellis - GA VSW Calculated incorrectly
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
				return eProfileType.AssortmentSummary;
			}
		}

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		public SetGradeStoreXRef SetGradeStoreXRef
		{
			get
			{
				return _setGradeStoreXRef;
			}
		}

		// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
		public eAssortmentBasisLoadedFrom BasisLoadedFrom
		{
			get
			{
				return _basisLoadedFrom;
			}
			set
			{
				_basisLoadedFrom = value;
			}
		}
		// END TT#1876 - stodd - summary incorrect when coming from selection window

		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        public AssortmentSummary Summary
        {
            get
            {
                return _assortmentSummary;
            }
        }
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review

		// Brgin TT#1137-MD - stodd - summary fields not matching style review - 
        public bool IsGroupAllocation
        {
            get
            {
                if (this._assortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int AnchorNodeRid
        {
            get
            {
                return _anchorNodeRid;
            }
            set
            {
                _anchorNodeRid = value;
            }
        }
		// End TT#1137-MD - stodd - summary fields not matching style review - 

        public ProfileList StoreList
        {
            get
            {
                return _storeProfileList;
            }
        }

		// Begin TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
        public ProfileList Gradelist
        {
            get
            {
                // List of StoreGradeProfile
                return _gradeList;
            }
        }
		// End TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier

   		// Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
		public StoreGroupProfile SGP
        {
            get
            {
                return _sgp;
            }
        }
		// End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
				
		//End TT#2 - JScott - Assortment Planning - Phase 2
		//===========
		// METHODS
		//===========

		

		/// <summary>
		/// Clears the total assortment dataTable of data.
		/// </summary>
		public void ClearAssortmentSummaryTable()	// TT#1876 - stodd
		{
			try
			{
				if (_dtAssortmentStoreSummary == null)
				{
					Header AssortData = new Header();
					_dtAssortmentStoreSummary = AssortData.GetEmptyAssortmentStoreSummaryDataTable();
				}
				else
				{
					_dtAssortmentStoreSummary.Clear();
				}
			}
			catch
			{

				throw;
			}
			//HeaderDataRecord.DeleteTotalAssortment(headerRid);
		}

		/// <summary>
		/// Processes and saves the Assortment Summary values.
		/// Even though only one variable type was requested in the method (_variableType).
		/// All three (Sales, Stock, and Receipts) are figured out and saved.
		/// </summary>
		/// <param name="aSAB"></param>
		/// <param name="aApplicationTransaction"></param>
		/// <param name="aStoreFilterRID"></param>
		/// <param name="asp"></param>
		/// <param name="hierNodeList"></param>
		/// <param name="versionList"></param>
		/// <param name="dateRangeList"></param>
		/// <param name="weightList"></param>
		public DataTable Process(
			ApplicationSessionTransaction trans,
			int anchorNodeRid,
			eAssortmentVariableType selectedAssortmentVariable,
			List<int> hierNodeList,
			List<int> versionList,
			List<int> dateRangeList,
			List<double> weightList,
			bool includeSimStore,
			bool includeIntransit,
			bool includeOnhand,
			bool includeCommitted,
			eStoreAverageBy averageBy,
			bool reprocess,
			bool refreshBasisData
			)
		{
			try
			{
				_dtAssortmentStoreSummary.Clear();
				//=================
				// Fill Store List
				//=================
				FillStoreList();
				// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
				bool refreshBasisNow = refreshBasisData;
				// END TT#1876 - stodd - summary incorrect when coming from selection window

				if (reprocess)
				{

					//===================================
					// read basis store values and total
					//===================================
					//_basisReader = new AssortmentBasisReader(_sab, trans, anchorNodeRid,
					//    hierNodeList, versionList, dateRangeList, weightList, _sgp.Key,
					//    includeSimStore, includeIntransit, includeOnhand,
					//    includeCommitted, _storeProfileList);

					//End TT#2 - JScott - Assortment Planning - Phase 2
					//===================================================================================
					// Even though only one variable type was requested in the method (_variableType).
					// All three (Sales, Stock, & Receipts) are figured out and saved.
					//===================================================================================
					//asp.ClearTotalAssortment();
					//_dtAssortmentStoreSummary.Clear();
					_storeEligibilityHash.Clear();
					//Begin TT#2 - JScott - Assortment Planning - Phase 2
					//_storeEligibilityHash = _basisReader.GetStoreEligibility(selectedAssortmentVariable);
					_storeEligibilityHash = _assortmentProfile.BasisReader.GetStoreEligibility(selectedAssortmentVariable);
					//End TT#2 - JScott - Assortment Planning - Phase 2
					// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
					if (_basisLoadedFrom == eAssortmentBasisLoadedFrom.AssortmentProperties
                        || _basisLoadedFrom == eAssortmentBasisLoadedFrom.GroupAllocation)
					{
						WriteAssortmentStoreEligibility();
					}
					// END TT#1876 - stodd - summary incorrect when coming from selection window

                    // Begin TT#1124-MD - stodd - need not matching style review - 
                    _assortmentSummary.SetTransaction(_trans);
                    _assortmentSummary.SetStoreLists(_storeProfileList, _storeList);
                    // End TT#1124-MD - stodd - need not matching style review - 

					eAssortmentVariableType tempVarType = eAssortmentVariableType.Sales;
					// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
					int variableNumber = ReadBasisVariable(tempVarType, includeSimStore, includeIntransit, includeOnhand, includeCommitted, averageBy, refreshBasisNow);
					// END TT#1876 - stodd - summary incorrect when coming from selection window
					SetAssortmentStoreSummary(variableNumber, tempVarType, _storeList, _storeSetList, _storeIndexList, _storeValueList, _avgStoreList,
                                    _storeIntransitList, _storeNeedList, _storePctNeedList, _storeOnHandList, _storeVSWOnHandList, _storePlanSalesList, _storePlanStockList);	// TT#845-MD - Stodd - add OnHand to Summary  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.

					// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
					// BasisDataReader was being rebuilt with each call.
					refreshBasisNow = false;
					// END TT#1876 - stodd - summary incorrect when coming from selection window

					// Both Stock and Receipts eligibility are built from Stock. 
					// So if the selected variable was NOT sales, then we want to build eligibility now.
					tempVarType = eAssortmentVariableType.Stock;
					// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
					variableNumber = ReadBasisVariable(tempVarType, includeSimStore, includeIntransit, includeOnhand, includeCommitted, averageBy, refreshBasisNow);
					// END TT#1876 - stodd - summary incorrect when coming from selection window
					SetAssortmentStoreSummary(variableNumber, tempVarType, _storeList, _storeSetList, _storeIndexList, _storeValueList, _avgStoreList,
                                    _storeIntransitList, _storeNeedList, _storePctNeedList, _storeOnHandList, _storeVSWOnHandList, _storePlanSalesList, _storePlanStockList);	// TT#845-MD - Stodd - add OnHand to Summary  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.

					tempVarType = eAssortmentVariableType.Receipts;
					// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
					variableNumber = ReadBasisVariable(tempVarType, includeSimStore, includeIntransit, includeOnhand, includeCommitted, averageBy, refreshBasisNow);
					// END TT#1876 - stodd - summary incorrect when coming from selection window
					SetAssortmentStoreSummary(variableNumber, tempVarType, _storeList, _storeSetList, _storeIndexList, _storeValueList, _avgStoreList,
                                    _storeIntransitList, _storeNeedList, _storePctNeedList, _storeOnHandList, _storeVSWOnHandList, _storePlanSalesList, _storePlanStockList);	// TT#845-MD - Stodd - add OnHand to Summary  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.

				}

                //System.IO.StringWriter sw;
                //string output;
                //Debug.WriteLine(" ");
                //Debug.WriteLine("_dtAssortmentStoreSummary");
                //foreach (DataRow row in _dtAssortmentStoreSummary.Rows)
                //{
                //    sw = new System.IO.StringWriter();
                //    foreach (DataColumn col in _dtAssortmentStoreSummary.Columns)
                //        sw.Write(col.ColumnName + ": " + row[col].ToString() + "\t");
                //    output = sw.ToString();
                //    //Console.WriteLine(output);
                //    Debug.WriteLine(output);
                //}

				return _dtAssortmentStoreSummary;



			}
			catch
			{
				throw;
			}
		}

		public void ReinitializeSummary(StoreGroupProfile sgp, ProfileList gradeList)
		{
			_sgp = sgp;
			_assortmentSummary = new AssortmentSummary(this, sgp, gradeList);	// TT#1198-MD - stodd - cannot open assortment - 
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			_assortmentProfile.ClearBasisReader();
			//End TT#2 - JScott - Assortment Planning - Phase 2
		}

		public void RereadStoreSummaryData()
		{
			// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
			if (_basisLoadedFrom == eAssortmentBasisLoadedFrom.UserSelectionCriteria)
			{
			}
			else
			{
				_headerAssortData = new Header();
				_dtAssortmentStoreSummary = _headerAssortData.GetAssortmentStoreSummary(Key);
			}
			// BEGIN TT#1876 - stodd - summary incorrect when coming from selection window
		}

        // Begin TT#2126-MD - JSmith - After REDO Need and Need % changes.  Expected it to be the same as before the REDO.  
        public void RecalculateSummaryValues()
        {
            GetNeed();
            // update Need in datatable
            for (int i = 0; i < _storeList.Count; i++)
            {
                DataRow[] rows = _dtAssortmentStoreSummary.Select("ST_RID = " + _storeList[i].ToString() + " and VARIABLE_TYPE = " + (int)eAssortmentVariableType.Sales);
                if (rows.Length > 0)
                {
                    rows[0]["NEED"] = _storeNeedList[i];
                    rows[0]["PCT_NEED"] = _storePctNeedList[i];
                }
            }
        }
        // End TT#2126-MD - JSmith - After REDO Need and Need % changes.  Expected it to be the same as before the REDO.  

		/// <summary>
		/// Fills Assortment Summary from _dtAssortmentStoreSummary DataTable
		/// </summary>
		public void BuildSummary(int storeGroupRid)
		{
			try
			{
                _sgp = StoreMgmt.StoreGroup_GetFilled(storeGroupRid); //_sab.StoreServerSession.GetStoreGroup(storeGroupRid);	// TT#1165-md - stodd - null ref changing attributes - 
                _setTotalHash.Clear();
                _setAvgHash.Clear();
                _setStoreCountHash.Clear();
				// Begin TT#1124-MD - stodd - need not matching style review - 
                FillStoreList();
                _assortmentSummary.SetStoreLists(_storeProfileList, _storeList);
                _assortmentSummary.SetTransaction(_trans);
				// End TT#1124-MD - stodd - need not matching style review - 
				_storeEligibilityHash = ReadStoreEligiblity();

				// ** IMPORTANT ** Sales MUST be done last. It ends up calcing the 
				// grade index for ALL variables.
				CalcAveragesAndIndexes(eAssortmentVariableType.Stock, storeGroupRid);
				CalcAveragesAndIndexes(eAssortmentVariableType.Receipts, storeGroupRid);
				CalcAveragesAndIndexes(eAssortmentVariableType.Sales, storeGroupRid);

				// BEGIN TT#1877 - Stodd - LV Depth not correct after Creating Placholders
				int reserveStoreRid = Include.NoRID;
				if (_assortmentProfile.AppSessionTransaction != null)
				{
					reserveStoreRid = _assortmentProfile.AppSessionTransaction.ReserveStore.RID;
				}
				// END TT#1877 - Stodd - LV Depth not correct after Creating Placholders

                //===============================================================================
                // Reads the _dtAssortmentStoreSummary to build the _assortmentSummary object.
                //===============================================================================
                if (_dtAssortmentStoreSummary.Rows.Count > 0)
                {
                    ProcessAssortmentSummary(reserveStoreRid);
                }
			}
			catch
			{
				throw;
			}
		}

        /// <summary>
        /// Reads the _dtAssortmentStoreSummary to build the _assortmentSummary object.
        /// </summary>
        /// <param name="reserveStoreRid"></param>
        private void ProcessAssortmentSummary(int reserveStoreRid)
        {
            _assortmentSummary.Clear();
            _assortmentSummary.StoreGroupProfile = _sgp;	// TT#1165-md - stodd - null ref changing attributes - 
            if (_dtAssortmentStoreSummary.Rows.Count > 0)
            {
                foreach (DataRow row in _dtAssortmentStoreSummary.Rows)
                {
                    int stRid = Convert.ToInt32(row["ST_RID"], CultureInfo.CurrentUICulture);
                    int varNum = Convert.ToInt32(row["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
                    int varType = Convert.ToInt32(row["VARIABLE_TYPE"], CultureInfo.CurrentUICulture);
                    int setRid = Convert.ToInt32(row["SGL_RID"], CultureInfo.CurrentUICulture);
                    double index = Convert.ToDouble(row["STORE_GRADE_INDEX"], CultureInfo.CurrentUICulture);
                    int units = Convert.ToInt32(row["UNITS"], CultureInfo.CurrentUICulture);
                    double avgStore = Convert.ToDouble(row["AVERAGE_STORE"], CultureInfo.CurrentUICulture);
                    int intransit = Convert.ToInt32(row["INTRANSIT"], CultureInfo.CurrentUICulture);
                    int need = Convert.ToInt32(row["NEED"], CultureInfo.CurrentUICulture);
                    decimal pctNeed = Convert.ToInt32(row["PCT_NEED"], CultureInfo.CurrentUICulture);
                    int gradeBoundary = Convert.ToInt32(row["STORE_GRADE_BOUNDARY"], CultureInfo.CurrentUICulture);
                    // Begin TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    int planSales = Convert.ToInt32(row["PLAN_SALES_UNITS"], CultureInfo.CurrentUICulture);
                    int planStock = Convert.ToInt32(row["PLAN_STOCK_UNITS"], CultureInfo.CurrentUICulture);
                    // End TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    // Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
                    // For GA, gets the Basis as the store's stock value
                    int basis = 0;
                    if (this._assortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        if (varType != (int)eAssortmentVariableType.Stock)
                        {
                            DataRow[] stockRows = _dtAssortmentStoreSummary.Select("ST_RID = " + stRid.ToString() + " and VARIABLE_TYPE = " + (int)eAssortmentVariableType.Stock);
                            if (stockRows.Length > 0)
                            {
                                basis = Convert.ToInt32(stockRows[0]["UNITS"], CultureInfo.CurrentUICulture);
                            }
                        }
                        else if (varType == (int)eAssortmentVariableType.Sales)
                        {
                            basis = units;
                        }
                    }
                    else
                    {
                        basis = units;
                    }
                    // End TT#952 - MD - stodd - add matrix to Group Allocation Review
                    // BEGIN TT#845-MD - Stodd - add OnHand to Summary
                    int onhand = 0;
                    if (row["ONHAND"] != DBNull.Value)
                    {
                        onhand = Convert.ToInt32(row["ONHAND"]);
                    }
                    // Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
                    int VSWOnhand = 0;
                    if (row["VSW_ONHAND"] != DBNull.Value)
                    {
                        VSWOnhand = Convert.ToInt32(row["VSW_ONHAND"]);
                    }
                    // End TT#952 - MD - stodd - add matrix to Group Allocation Review
                    // END TT#845-MD - Stodd - add OnHand to Summary

                    //Debug.WriteLine("HDR" + Convert.ToInt32(row["HDR_RID"]) + ",ST" + Convert.ToInt32(row["ST_RID"]) + ",VAR" + varNum + "," + units + ",IDX" + index + ",GRD" + gradeBoundary + ",OnHand" + onhand + ",VSWOnHand" + VSWOnhand);

                    // Adds the values to the _dtSummary that is eventually used in the summary area of the assortment view
                    // BEGIN TT#1877 - Stodd - LV Depth not correct after Creating Placholders
                    if (stRid != reserveStoreRid)
                    {
                        _assortmentSummary.Add(varNum, varType, setRid, index, units, avgStore, basis, intransit, onhand, need, pctNeed, gradeBoundary, VSWOnhand, planSales, planStock);	// TT#845-MD - Stodd - add OnHand to Summary  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    }
                    // END TT#1877 - Stodd - LV Depth not correct after Creating Placholders
                }
                _assortmentSummary.FinishSummary();
                //_assortmentSummary.DebugSummary();
            }
        }

		private Hashtable ReadStoreEligiblity()
		{
			Hashtable storeElibibilityHash = new Hashtable();
			DataTable dtStoreEligibility = ReadAssortmentStoreEligibility();
			foreach (DataRow aRow in dtStoreEligibility.Rows)
			{
				int storeRid = int.Parse(aRow["ST_RID"].ToString());
				bool eligible = Include.ConvertCharToBool(char.Parse(aRow["ELIGIBLE"].ToString()));
				storeElibibilityHash.Add(storeRid, eligible);
			}
			return storeElibibilityHash;
		}

		private void CalcAveragesAndIndexes(eAssortmentVariableType varType, int storeGroupRid)
        {
			try
			{
				//int variableNumber = 0;
				//if (_basisReader != null)
				//{
				//    variableNumber = _basisReader.GetVariableNumber(varType);
				//}
				//else
				//{
				//    variableNumber = GetVariableNumber(varType);
				//}

				DataRow[] rows = _dtAssortmentStoreSummary.Select("VARIABLE_TYPE = " + (int)varType);
				if (rows.Length > 0)
				{
                    // Begin TT#3767 - stodd - group allocation view performance 
                    StoreGroupProfile sgp = _sgp;
                    if (_sgp == null)
                    {
                        sgp = StoreMgmt.StoreGroup_GetFilled(storeGroupRid); //_sab.StoreServerSession.GetStoreGroupFilled(storeGroupRid);
                    }
                    // End TT#3767 - stodd - group allocation view performance 

					if (_storeAverageBy == eStoreAverageBy.Set)
					{
						foreach (StoreGroupLevelProfile sglp in sgp.GroupLevels.ArrayList)
						{
							double storeAvg = CalcStoreAverage(sglp.Key, sglp.Stores, rows);
							CalcStoreIndex(sglp.Key, sglp.Stores, rows, storeAvg);
						}
					}
					else
					{
                        // Begin TT#3767 - stodd - group allocation view performance 
                        //ProfileList allStoreList = _sab.StoreServerSession.GetAllStoresList();
                        //double storeAvg = CalcStoreAverage(Include.AllStoreGroupLevelRID, allStoreList, rows);
                        double storeAvg = CalcStoreAverage(Include.AllStoreGroupLevelRID, _storeProfileList, rows);
                        // End TT#3767 - stodd - group allocation view performance 
						foreach (StoreGroupLevelProfile sglp in sgp.GroupLevels.ArrayList)
						{
							CalcStoreIndex(sglp.Key, sglp.Stores, rows, storeAvg);
						}
					}
				}

				//=======================================================
				// Determine grade boundary based upon defined grades
				//=======================================================
				if (varType == eAssortmentVariableType.Sales)
				{
                    //AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();	// TT#1146-MD - stodd - "f" store received allocation - 	// TT#1198-MD - stodd - cannot open assortment - 
					//Begin TT#2 - JScott - Assortment Planning - Phase 2
					_setGradeStoreXRef = new SetGradeStoreXRef();

					//End TT#2 - JScott - Assortment Planning - Phase 2
					foreach (DataRow row in rows)
					{
						// Begin TT#1146-MD - stodd - "f" store received allocation - 
                        int gradeBoundary = 0;	// TT#3810 - stodd - argument out of range on grade index
                        int storeRid = int.Parse(row["ST_RID"].ToString());

						// Begin TT#1198-MD - stodd - cannot open assortment - 
                        if (IsGroupAllocation)
                        {
                            AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();	// TT#1146-MD - stodd - "f" store received allocation - 
                            if (asp.SubtotalMembers.Count > 0)
                            {
								// Begin TT#3810 - stodd - argument out of range on grade index
                                //int index = asp.GetStoreGradeIdx(storeRid);
                                string storeGrade = asp.GetStoreGrade(storeRid);
                                //StoreGradeProfile sgp = (StoreGradeProfile)_gradeList[index];
                                foreach (StoreGradeProfile sgp in _gradeList.ArrayList)
                                {
                                    if (sgp.StoreGrade == storeGrade)
                                    {
                                        gradeBoundary = sgp.Boundary;
                                    }
                                }
                                //grade = sgp.Boundary;
								// End TT#3810 - stodd - argument out of range on grade index
                            }
                            else
                            {
                                gradeBoundary = GetStoreGradeSequence(double.Parse(row["STORE_GRADE_INDEX"].ToString()));		// TT#3810 - stodd - argument out of range on grade index
                            }
                        }
						// End TT#1198-MD - stodd - cannot open assortment - 
                        else
                        {
                            gradeBoundary = GetStoreGradeSequence(double.Parse(row["STORE_GRADE_INDEX"].ToString()));		// TT#3810 - stodd - argument out of range on grade index
                        }
						// End TT#1146-MD - stodd - "f" store received allocation - 
						//Begin TT#2 - JScott - Assortment Planning - Phase 2
						int storeSet = Convert.ToInt32(row["SGL_RID"]);
                        _setGradeStoreXRef.AddXRefIdEntry(storeSet, gradeBoundary, storeRid);		// TT#3810 - stodd - argument out of range on grade index

						//End TT#2 - JScott - Assortment Planning - Phase 2
						DataRow[] stRows = _dtAssortmentStoreSummary.Select("ST_RID = " + storeRid);
						foreach (DataRow stRow in stRows)
						{
                            stRow["STORE_GRADE_BOUNDARY"] = gradeBoundary;		// TT#3810 - stodd - argument out of range on grade index
						}
					}
				}

                //System.IO.StringWriter sw;
                //string output;
                //Debug.WriteLine(" ");
                //Debug.WriteLine("_dtAssortmentStoreSummary");
                //foreach (DataRow row in _dtAssortmentStoreSummary.Rows)
                //{
                //    sw = new System.IO.StringWriter();
                //    foreach (DataColumn col in _dtAssortmentStoreSummary.Columns)
                //        sw.Write(col.ColumnName + ": " + row[col].ToString() + "\t");
                //    output = sw.ToString();
                //    //Console.WriteLine(output);
                //    Debug.WriteLine(output);
                //}
				
			}
			catch
			{
				throw;
			}

        }


		public int GetVariableNumber(eAssortmentVariableType variableType)
		{
			try
			{
				int varNumber = Include.Undefined;
				HierarchyNodeProfile node = _sab.HierarchyServerSession.GetNodeData(_anchorNodeRid, true);	// TT#952 - MD - stodd - add grid to Group Allocation - 
				switch (variableType)
				{
					case eAssortmentVariableType.Sales:
						if (node.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							varNumber = _trans.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
						}
						else
						{
							varNumber = _trans.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Stock:
						if (node.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							varNumber = _trans.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
						}
						else
						{
							varNumber = _trans.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Receipts:
						if (node.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							varNumber = _trans.PlanComputations.PlanVariables.ReceiptRegularUnitsVariable.Key;
						}
						else
						{
							varNumber = _trans.PlanComputations.PlanVariables.ReceiptTotalUnitsVariable.Key;
						}
						break;
					default:
						string msg = MIDText.GetText(eMIDTextCode.msg_as_InvalidVariable);
						msg = msg.Replace("{0}", variableType.ToString());
						throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_as_InvalidVariable, msg);
				}

				return varNumber;
			}
			catch
			{
				throw;
			}
		}

		

		private void FillStoreList()
		{
			try
			{
				if (_storeList == null)
					_storeList = new List<int>();
				_storeList.Clear();
				_storeToListHash.Clear();
				_listToStoreHash.Clear();
                _storeProfileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_sab.StoreServerSession.GetActiveStoresList();
				
				// Place store keys in a list for later use.
				for (int i = 0; i < _storeProfileList.Count; i++)
				{
					StoreProfile store = (StoreProfile)_storeProfileList[i];
					_storeList.Add(store.Key);
					_storeToListHash.Add(store.Key, i);
					_listToStoreHash.Add(i, store.Key);
				}
			}
			catch
			{
				throw;
			}
		}

		private int ReadBasisVariable(eAssortmentVariableType variableType, bool includeSimStore, bool includeIntransit, 
			bool includeOnhand, bool includeCommitted, eStoreAverageBy averageBy, bool refreshBasisData )
		{
			try
			{
				//==========================================
				// Init Store Lists to get ready to fill
				//==========================================
				InitStoreLists(variableType);
				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				//double totalUnits = _basisReader.GetBasisTotalUnits(variableType);
				//Begin TT#2 - stodd 
				if (refreshBasisData)
					_assortmentProfile.BasisReader = null;
				//End TT#2 - stodd 
                double totalUnits = 0;
                AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				// Begin TT#1137-MD - stodd - summary fields not matching style review - 
                if (IsGroupAllocation)
                {
                    //AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    if (variableType == eAssortmentVariableType.Sales)
                    {
                        totalUnits = asp.GetStoreListTotalSalesPlan(_storeProfileList);
                    }
                }
				// End TT#1137-MD - stodd - summary fields not matching style review - 
                else
                {
				    totalUnits = _assortmentProfile.BasisReader.GetBasisTotalUnits(variableType);
                }
				_totalSales = (int)totalUnits;
				//End TT#2 - JScott - Assortment Planning - Phase 2
				//Debug.WriteLine(variableType.ToString() + " " + totalUnits.ToString());
				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				//_storeValueList = _basisReader.GetBasisStoreUnits(variableType);
				//int variableNumber = _basisReader.GetVariableNumber(variableType);
				// Begin TT#1137-MD - stodd - summary fields not matching style review - 
                if (IsGroupAllocation)
                {
                    _storeValueList = new List<int>();
                    //AllocationSubtotalProfile asp = _trans.GetAllocationGrandTotalProfile();  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    if (variableType == eAssortmentVariableType.Sales)
                    {
                        for (int i = 0; i < _storeList.Count; i++)
                        {
                            double sales = asp.GetStoreSalesPlan(_storeList[i]);
                            _storeValueList.Add((int)sales);
                        }
                    }
                    if (variableType == eAssortmentVariableType.Stock)
                    {
                        for (int i = 0; i < _storeList.Count; i++)
                        {
                            double stock = asp.GetStoreStockPlan(_storeList[i]);
                            _storeValueList.Add((int)stock);
                        }
                    }
                    if (variableType == eAssortmentVariableType.Receipts)
                    {
                        for (int i = 0; i < _storeList.Count; i++)
                        {
                            _storeValueList.Add(0);
                        }
                    }
                }
				// End TT#1137-MD - stodd - summary fields not matching style review - 
                else
                {
                    _storeValueList = _assortmentProfile.BasisReader.GetBasisStoreUnits(variableType);
                }

                // Begin TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                for (int i = 0; i < _storeList.Count; i++)
                {
                    double sales = asp.GetStoreSalesPlan(_storeList[i]);
                    _storePlanSalesList[i] = (int)sales;
                    double stock = asp.GetStoreStockPlan(_storeList[i]);
                    _storePlanStockList[i] = (int)stock;
                }
                // End TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.

				int variableNumber = _assortmentProfile.BasisReader.GetVariableNumber(variableType);
				//End TT#2 - JScott - Assortment Planning - Phase 2
				if (variableType == eAssortmentVariableType.Sales)
				{
					GetIntransit();
					GetOnHand();	// TT#845-MD - Stodd - add OnHand to Summary
                    GetVSWOnHand();	// TT#952 - MD - stodd - add matrix to Group Allocation Review
					GetNeed();
				}


				if (variableType == eAssortmentVariableType.Stock ||
					variableType == eAssortmentVariableType.Receipts)
				{
					//============================
					// Include Intransit in value
					//============================
					if (includeIntransit)
					{
						totalUnits += _totalIntransit;
						// Add list to _storeValueList
						for (int i = 0; i < _storeValueList.Count; i++)
						{
							_storeValueList[i] += _storeIntransitList[i];
						}
					}
					//=========================
					// Include OnHand in value
					//=========================
					if (includeOnhand)
					{
						//Begin TT#2 - JScott - Assortment Planning - Phase 2
						//double totalOnHand = _basisReader.GetPlanActualTotalUnits(eAssortmentVariableType.Onhand);
						//List<int> storeOnHandList = _basisReader.GetPlanActualStoreUnits(eAssortmentVariableType.Onhand);
						// BEGIN TT#845-MD - Stodd - add OnHand to Summary
						//_totalOnHand = _assortmentProfile.BasisReader.GetPlanTotalUnits(eAssortmentVariableType.Onhand, Include.FV_ActualRID);
						//_storeOnHandList = _assortmentProfile.BasisReader.GetPlanStoreUnits(eAssortmentVariableType.Onhand, Include.FV_ActualRID);
						//End TT#2 - JScott - Assortment Planning - Phase 2
						totalUnits += _totalOnHand;
						// Add list to _storeValueList
						for (int i = 0; i < _storeValueList.Count; i++)
						{
							_storeValueList[i] += _storeOnHandList[i];
						}
						// END TT#845-MD - Stodd - add OnHand to Summary
					}
				}

				_setTotalHash.Clear();
				_setAvgHash.Clear();
				_setStoreCountHash.Clear();

				//DebugStoreValues(variableType);

				return variableNumber;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Fills global Need variables.
		/// </summary>
		private void GetNeed()
		{
			//====================
			// Get need
			//====================
			// Begin TT#1124-MD - stodd - need not matching style review - 

			// Begin TT#1198-MD - stodd - cannot open assortment - 
			// Begin TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 
            //if (IsGroupAllocation)
            {
                AllocationSubtotalProfile asp = _trans.GetAssortmentGrandTotalProfile();
                for (int i = 0; i < _storeList.Count; i++)
                {
                    double need = asp.GetStoreUnitNeed(_storeList[i]);
                    _storeNeedList[i] = (int)need;
                    double pctNeed = asp.GetStorePercentNeed(_storeList[i]);
                    _storePctNeedList[i] = (Decimal)pctNeed;
                }

                _totalNeed = asp.GetStoreListTotalUnitNeed(_storeProfileList);
                _totalPctNeed = asp.GetStoreListTotalPercentNeed(_storeProfileList);
                //_totalPctNeed = Need.PctUnitNeed(_totalNeed, _totalSales);
            }
            //else
            //{

            //    _totalNeed = _assortmentProfile.BasisReader.GetPlanTotalUnits(eAssortmentVariableType.Need, Include.FV_ActualRID);
            //    _storeNeedList = _assortmentProfile.BasisReader.GetPlanStoreUnits(eAssortmentVariableType.Need, Include.FV_ActualRID);

            //    _totalNeed = _assortmentProfile.BasisReader.GetPlanTotalUnits(eAssortmentVariableType.Need, Include.FV_ActionRID);
            //    _storeNeedList = _assortmentProfile.BasisReader.GetPlanStoreUnits(eAssortmentVariableType.Need, Include.FV_ActionRID);


            //    for (int i = 0; i < _storeList.Count; i++)
            //    {
            //        double pctNeed = Need.PctUnitNeed(_storeNeedList[i], _storeValueList[i]);
            //        _storePctNeedList[i] = (Decimal)pctNeed;
            //    }

            //    _totalPctNeed = Need.PctUnitNeed(_totalNeed, _totalSales);
            //    // End TT#1124-MD - stodd - need not matching style review - 
            //}
			// End TT#831-MD - stodd - Need, Need%, & VSW Onhand incorrect 
			// End TT#1198-MD - stodd - cannot open assortment - 
		}

		// BEGIN TT#845-MD - Stodd - add OnHand to Summary
		private void GetOnHand()
		{
			//====================
			// Get OnHand
			//====================
			//_totalOnHand = _assortmentProfile.BasisReader.GetPlanTotalUnits(eAssortmentVariableType.Onhand, Include.FV_ActualRID);
			//_storeOnHandList = _assortmentProfile.BasisReader.GetPlanStoreUnits(eAssortmentVariableType.Onhand, Include.FV_ActualRID);

			IntransitKeyType[] itkArray = new IntransitKeyType[1];
			itkArray[0] = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
			DateTime dt = new DateTime();
            // Begin TT#2113-MD - JSmith - Matrix Grid values incorrect for the Apply To Section
            if (_assortmentProfile.BeginDay != Include.UndefinedDate)
            {
                dt = _assortmentProfile.BeginDay;
            }
            // End TT#2113-MD - JSmith - Matrix Grid values incorrect for the Apply To Section
			for (int i = 0; i < _storeList.Count; i++)
			{
				int stRid = _storeList[i];
				int stOH = _trans.GetStoreOnHand(_anchorNodeRid, dt, itkArray[0], stRid);
				_storeOnHandList[i] = stOH;
				_totalOnHand += stOH;
			}
		}
		// END TT#845-MD - Stodd - add OnHand to Summary

		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        private void GetVSWOnHand()
        {
            //====================
            // Get OnHand
            //====================
            //_totalOnHand = _assortmentProfile.BasisReader.GetPlanTotalUnits(eAssortmentVariableType.Onhand, Include.FV_ActualRID);
            //_storeOnHandList = _assortmentProfile.BasisReader.GetPlanStoreUnits(eAssortmentVariableType.Onhand, Include.FV_ActualRID);

            IntransitKeyType[] itkArray = new IntransitKeyType[1];
            itkArray[0] = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
            DateTime dt = new DateTime();
            // Begin TT#2113-MD - JSmith - Matrix Grid values incorrect for the Apply To Section
            if (_assortmentProfile.BeginDay != Include.UndefinedDate)
            {
                dt = _assortmentProfile.BeginDay;
            }
            // End TT#2113-MD - JSmith - Matrix Grid values incorrect for the Apply To Section
            for (int i = 0; i < _storeList.Count; i++)
            {
                int stRid = _storeList[i];
                int stOH = _trans.GetStoreImoHistory(_anchorNodeRid, dt, itkArray[0], stRid);
                _storeVSWOnHandList[i] = stOH;
                _totalVSWOnHand += stOH;
            }
        }
		// End TT#952 - MD - stodd - add matrix to Group Allocation Review

		/// <summary>
		/// Fills global Intransit variables.
		/// </summary>
		/// <returns></returns>
		private double GetIntransit()
		{
            _totalIntransit = 0;
			//====================
			// Get Intransit
			//====================
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			//double totalIntransit = _basisReader.GetPlanActualTotalUnits(eAssortmentVariableType.Intransit);
			//_storeIntransitList = _basisReader.GetPlanActualStoreUnits(eAssortmentVariableType.Intransit);
			//_totalIntransit = _assortmentProfile.BasisReader.GetPlanTotalUnits(eAssortmentVariableType.Intransit, Include.FV_ActualRID);
			//_storeIntransitList = _assortmentProfile.BasisReader.GetPlanStoreUnits(eAssortmentVariableType.Intransit, Include.FV_ActualRID);
			//End TT#2 - JScott - Assortment Planning - Phase 2

            IntransitKeyType[] itkArray = new IntransitKeyType[1];
            itkArray[0] = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
            // begin TT#4345 - MD - Jellis - GA VSW calculated incorrectly
            //if (_trans.GetIntransitReader().SetDayRangeForRID(_anchorNodeRid, _horizon_ID))
            //{
            //    StoreSalesITHorizon ssh = GetStoreSalesITHorizons();
            //    _trans.GetIntransitReader().SetStoreIT_DayRange(ssh.StoreRIDList, _anchorNodeRid, ssh.StoreHorizonStart, ssh.StoreHorizonEnd);
            //}
            // end TT#4345 - MD - Jellis - GA VSW calculated incorrectly
            
            for (int i = 0; i < _storeList.Count; i++)
            {
                int stRid = _storeList[i];
                int stInt = _trans.GetStoreInTransit(_anchorNodeRid, GetStoreSalesITHorizons(), itkArray, stRid); // TT#4345 - MD - Jellis - GA VSW calculated incorrectly
                _storeIntransitList[i] = stInt;
                _totalIntransit += stInt;
            }

			return _totalIntransit;
		}

		private StoreSalesITHorizon GetStoreSalesITHorizons()
		{
            // begin TT#4345 - MD - Jellis - GA VSW Calculated incorrectly
            //StoreSalesITHorizon storeSalesITHorizon;
            //int[] storeRID = new int[_trans.StoreIndexRIDArray().Length];
            //int[] horizonStart = new int[storeRID.Length];
            //int[] horizonEnd = new int[storeRID.Length];
            //// Find range of daily intransit to read
            //int startDay = _assortmentProfile.OnHandDayProfile.YearDay;
            //DateTime storeShipDay;
            //for (int i = 0; i < this._storeList.Count; i++)
            //{
            //    Index_RID storeIdxRID = this._trans.StoreIndexRIDArray()[i];
            //    storeRID[i] = storeIdxRID.RID;
            //    horizonStart[i] = startDay;
            //    storeShipDay = _assortmentProfile.AssortmentApplyToDate.Date;
            //    horizonEnd[i] = (storeShipDay.AddDays(-1)).Year * 1000 + (storeShipDay).DayOfYear;
            //}
            //storeSalesITHorizon = new StoreSalesITHorizon(storeRID, horizonStart, horizonEnd);
            //return storeSalesITHorizon;
            if (_storeSalesITHorizon.HorizonID.BeginDayID == 0)
            {
                // Begin TT#2038-MD - JSmith - Intransit is incorrect when the Delivery Week is changed
                //_storeSalesITHorizon = new StoreSalesITHorizon(_trans, _assortmentProfile.OnHandDayProfile.Date, _assortmentProfile.AssortmentApplyToDate.Date);
                if (IsGroupAllocation
                    || _assortmentProfile.ShipToDay == Include.UndefinedDate)
                {
                    _storeSalesITHorizon = new StoreSalesITHorizon(_trans, _assortmentProfile.OnHandDayProfile.Date, _assortmentProfile.AssortmentApplyToDate.Date);
                }
                else
                {
                    _storeSalesITHorizon = new StoreSalesITHorizon(_trans, _assortmentProfile.OnHandDayProfile.Date, _assortmentProfile.ShipToDay);
                }
                // End TT#2038-MD - JSmith - Intransit is incorrect when the Delivery Week is changed
            }
            return _storeSalesITHorizon;
            // end TT#4345 - MD - Jellis - GA VSW Calculated incorrectly
		}

		public void DebugStoreValues(eAssortmentVariableType variableType)
		{
			Debug.WriteLine(" ");
			for (int i = 0; i < _storeValueList.Count; i++)
			{
				Debug.WriteLine("variable" + variableType + " store " + _storeIndexList[i] +
					" set " + _storeSetList[i] +
					" value " + _storeValueList[i] +
					" avg " + _avgStoreList[i] +
					" index " + _storeIndexList[i] +
					"grade boundary " + _storeGradeBoundaryList[i]);
			}
		}


		private void InitStoreLists(eAssortmentVariableType variableType)
		{
			_storeSetList = new List<int>(_storeList.Count);
			_storeIndexList = new List<double>(_storeList.Count);
			_avgStoreList = new List<double>(_storeList.Count);
			_storeIntransitList = new List<int>(_storeList.Count);
			_storeOnHandList = new List<int>(_storeList.Count);	// TT#845-MD - Stodd - add OnHand to Summary
            _storeVSWOnHandList = new List<int>(_storeList.Count);
			_storeNeedList = new List<int>(_storeList.Count);
			_storePctNeedList = new List<decimal>(_storeList.Count);
            _storePlanSalesList = new List<int>(_storeList.Count);  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
            _storePlanStockList = new List<int>(_storeList.Count);  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
			//_isStoreEligibleList = new List<bool>(_storeList.Count);
			if (variableType == eAssortmentVariableType.Sales)
			{
				_storeGradeBoundaryList = new List<int>(_storeList.Count);
			}

			foreach (int i in _storeList)
			{
				_storeSetList.Add(0);
				_storeIndexList.Add(0);
				_avgStoreList.Add(0);
				_storeIntransitList.Add(0);
				_storeOnHandList.Add(0);	// TT#845-MD - Stodd - add OnHand to Summary
                _storeVSWOnHandList.Add(0);
				_storeNeedList.Add(0);
				_storePctNeedList.Add(0);
                _storePlanSalesList.Add(0);  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                _storePlanStockList.Add(0);  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				//_isStoreEligibleList.Add(true);
				if (variableType == eAssortmentVariableType.Sales)
				{
					_storeGradeBoundaryList.Add(0);
				}
			}
		}

		private int GetStoreGradeSequence(double index)
		{
			int grade = 0;
			for (int i = 0; i < _gradeList.Count; i++)
			{
				StoreGradeProfile sgp = (StoreGradeProfile)_gradeList[i];
				if (index >= sgp.Boundary)
				{
					grade = sgp.Boundary;
					break;
				}
				else if (index == 0)
				{
					grade = 0;
					break;
				}
			}
			return grade;
		}

		private void CalcStoreIndex(int setKey, ProfileList storeList, DataRow [] storeRows, double storeAvg)
		{
			try
			{
				//=====================
				// Calc Store Indexes
				//=====================
				foreach (StoreProfile store in storeList.ArrayList)
				{
					double storeIndex = 0.0d;
					foreach (DataRow row in storeRows)
					{
						if (store.Key == int.Parse(row["ST_RID"].ToString()))
						{
							double units = double.Parse(row["UNITS"].ToString());
							storeIndex = 0.0d;
							if (storeAvg != 0.0d)
							{
                                // Begin TT#811-MD - stodd - Rounding was incorrect
								//storeIndex = ((units * 100) / storeAvg) + .5;
								//storeIndex = Math.Round(storeIndex, 2);
                                int interm = (int)(((units * 10000) / storeAvg) + .5);
                                storeIndex = (double)interm / 100;
                                // End TT#811-MD - stodd - Rounding was incorrect
							}
							row["STORE_GRADE_INDEX"] = storeIndex;
							row["AVERAGE_STORE"] = storeAvg;
							row["SGL_RID"] = setKey;
							break;
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}


        private double CalcStoreAverage(int setKey, ProfileList storeList, DataRow [] storeRows)
        {
            try
            {
                //====================
                // Calc Average Store
                //====================
                double totalAmt = 0;
                double storeCount = 0;
                double avg = 0;
                foreach (StoreProfile store in storeList.ArrayList)
                {
                    // Begin TT#1765-MD - JSmith - GA-Matrix Tab -  Index when calcing Index by grade using the store avg based on 95 strs(includes reserve str) should use avg str in the matrix based on 94 strs.
                    if (store.Key == _assortmentProfile.AppSessionTransaction.ReserveStore.RID)
                    {
                        continue;
                    }
                    // End TT#1765-MD - JSmith - GA-Matrix Tab -  Index when calcing Index by grade using the store avg based on 95 strs(includes reserve str) should use avg str in the matrix based on 94 strs.
                    //==========================================================================
                    // If all the stores in a set is sent to this method, some of the stores
                    // may not be in out filtered list. We check first thing to see if the
                    // store is in our list.
                    //==========================================================================
                    // Begin TT#635-MD - stodd - store average was using reserve store
                    int reserveStoreRid = Include.NoRID;
                    if (_assortmentProfile.AppSessionTransaction != null)
                    {
                        reserveStoreRid = _assortmentProfile.AppSessionTransaction.ReserveStore.RID;
                    }
                    // End TT#635-MD - stodd - store average was using reserve store
					if (isStoreEligible(store.Key))
					{
						foreach (DataRow row in storeRows)
						{
                            // Begin TT#635-MD - stodd - store average was using reserve store
							if (store.Key == int.Parse(row["ST_RID"].ToString()))
							{
								//if (int.Parse(row["UNITS"].ToString()) > 0)
								//{
                                if (store.Key != reserveStoreRid)
                                {
                                    storeCount++;
                                    totalAmt += int.Parse(row["UNITS"].ToString());
                                    break;
                                }
								//}
							}
                            // End TT#635-MD - stodd - store average was using reserve store
						}
					}
					
                }
                if (storeCount > 0)
                {
                    // Begin TT#811-MD - stodd - Rounding was incorrect
                    avg = (int)((totalAmt * 100 / storeCount) + .5);
                    //avg = Math.Round(avg, 2);
                    avg = (double)avg / 100;
                    // End TT#811-MD - stodd - Rounding was incorrect

                }
                //Debug.WriteLine("Store avg. Set: " + setKey + " " + totalAmt + " " + storeCount + " " + avg);
                return avg;
            }
            catch
            {
                throw;
            }
        }

		private bool isStoreEligible(int storeRid)
		{
			if (_storeEligibilityHash.ContainsKey(storeRid))
			{
				return (bool)_storeEligibilityHash[storeRid];	
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// Sets the total assortment information and fills the assortment summary information
		/// </summary>
		/// <param name="headerRid"></param>
		/// <param name="variableNumber"></param>
		/// <param name="storeList"></param>
		/// <param name="storeSetList"></param>
		/// <param name="storeGradeIndexList"></param>
		/// <param name="unitsList"></param>
		/// <param name="avgStoreList"></param>
		public void SetAssortmentStoreSummary(
			int variableNumber,
			eAssortmentVariableType variableType,
			List<int> storeList,
			List<int> storeSetList,  //StoreGroupLevelRid
			List<double> storeGradeIndexList,
			List<int> unitsList,
			List<double> avgStoreList,
			List<int> intransitList,
			List<int> needList,
			List<decimal> pctNeedList,
			List<int> onhandList,	// TT#845-MD - Stodd - add OnHand to Summary
            List<int> VSWOnhandList,
            List<int> planSalesList,  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
            List<int> planStockList)  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
		{
			try
			{
				if (storeSetList.Count > 0)
				{
					for (int i = 0; i < storeList.Count; i++)
					{
						// Begin TT#1137-MD - stodd - summary fields not matching style review - 
                        bool isEligible = true;
                        if (IsGroupAllocation)
                        {
                            isEligible = _assortmentProfile.GetStoreIsEligible((int)storeList[i]);
                        }
                        else
                        {
                            isEligible = isStoreEligible((int)storeList[i]);
                        }
                        if (isEligible)
                        {
                            DataRow newRow = _dtAssortmentStoreSummary.NewRow();
                            newRow["HDR_RID"] = this.Key;
                            newRow["ST_RID"] = storeList[i];
                            //newRow["SGL_RID"] = storeSetList[i];
                            newRow["VARIABLE_NUMBER"] = variableNumber;
                            newRow["VARIABLE_TYPE"] = (int)variableType;
                            //newRow["STORE_GRADE_INDEX"] = storeGradeIndexList[i];
                            newRow["UNITS"] = unitsList[i];
                            //newRow["AVERAGE_STORE"] = avgStoreList[i];
                            newRow["INTRANSIT"] = intransitList[i];
                            newRow["NEED"] = needList[i];
                            newRow["PCT_NEED"] = pctNeedList[i];
                            //newRow["STORE_GRADE_BOUNDARY"] = storeGradeBoundaryList[i];
                            newRow["ONHAND"] = onhandList[i];	// TT#845-MD - Stodd - add OnHand to Summary
                            newRow["VSW_ONHAND"] = VSWOnhandList[i];
                            newRow["PLAN_SALES_UNITS"] = planSalesList[i];  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                            newRow["PLAN_STOCK_UNITS"] = planStockList[i];  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                            _dtAssortmentStoreSummary.Rows.Add(newRow);
                        }
                        else
                        {
                            Debug.WriteLine("NOT ELIG " + storeList[i].ToString());
                        }
						// End TT#1137-MD - stodd - summary fields not matching style review - 
					}
				}
			}
			catch
			{

				throw;
			}
		}

		public AssortmentSummaryItemProfile GetAssortmentSummary(int variableNumber, int setRid, int storeGrade)
		{
			try
			{
				AssortmentSummaryItemProfile asip = _assortmentSummary.GetSummary(variableNumber, setRid, storeGrade);

				// Add individual stores
				DataRow[] rows = _dtAssortmentStoreSummary.Select("VARIABLE_NUMBER = " + variableNumber + " and SGL_RID = " + setRid + " and STORE_GRADE_BOUNDARY = " + storeGrade);
				if (rows.Length > 0)
				{
                    // Begin TT#3767 - stodd - group allocation view performance 
					//StoreGroupLevelProfile sgl = _sab.StoreServerSession.GetStoreGroupLevel(setRid);
                    StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_sgp.GroupLevels.FindKey(setRid);
                    // End TT#3767 - stodd - group allocation view performance 

					foreach (DataRow row in rows)
					{
						int stRid = int.Parse(row["ST_RID"].ToString());
						// Begin TT#1225 - stodd
						int reserveStoreRid = Include.NoRID;
						if (_assortmentProfile.AppSessionTransaction != null)
						{
							reserveStoreRid = _assortmentProfile.AppSessionTransaction.ReserveStore.RID;
						}
						if (sgl.Stores.Contains(stRid) && stRid != reserveStoreRid)
						{
							StoreProfile stProf = (StoreProfile)sgl.Stores.FindKey(stRid);
                            // Begin TT#1954-MD - JSmith - Assortment
                            //asip.AddToStoreList(stProf);
                            if (!stProf.SimilarStoreModel)
                            {
                                asip.AddToStoreList(stProf);
                            }
                            // End TT#1954-MD - JSmith - Assortment
						}
						// End TT#1225 - stodd

					}
				}
				return asip;
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1189-md - stodd - adding locking to group allocation
        public AssortmentSummaryItemProfile GetAssortmentSummary(int variableNumber, int setRid)
        {
            try
            {
                AssortmentSummaryItemProfile asip = _assortmentSummary.GetSummary(variableNumber, setRid);

                // Add individual stores
                DataRow[] rows = _dtAssortmentStoreSummary.Select("VARIABLE_NUMBER = " + variableNumber + " and SGL_RID = " + setRid);
                if (rows.Length > 0)
                {
                    // Begin TT#3767 - stodd - group allocation view performance 
                    //StoreGroupLevelProfile sgl = _sab.StoreServerSession.GetStoreGroupLevel(setRid);
                    StoreGroupLevelProfile sgl = (StoreGroupLevelProfile)_sgp.GroupLevels.FindKey(setRid);
                    // End TT#3767 - stodd - group allocation view performance 

                    foreach (DataRow row in rows)
                    {
                        int stRid = int.Parse(row["ST_RID"].ToString());
                        // Begin TT#1225 - stodd
                        int reserveStoreRid = Include.NoRID;
                        if (_assortmentProfile.AppSessionTransaction != null)
                        {
                            reserveStoreRid = _assortmentProfile.AppSessionTransaction.ReserveStore.RID;
                        }
                        if (sgl.Stores.Contains(stRid) && stRid != reserveStoreRid)
                        {
                            StoreProfile stProf = (StoreProfile)sgl.Stores.FindKey(stRid);
                            asip.AddToStoreList(stProf);
                        }
                        // End TT#1225 - stodd

                    }
                }
                return asip;
            }
            catch
            {
                throw;
            }
        }
		// End TT#1189-md - stodd - adding locking to group allocation

		public AssortmentSummaryStoreDetailProfile GetStoreDetail(int varNum, int storeRid)
		{
			AssortmentSummaryStoreDetailProfile stp = new AssortmentSummaryStoreDetailProfile(storeRid);
			try
			{
				DataRow[] rows = this._dtAssortmentStoreSummary.Select("VARIABLE_NUMBER = " + varNum.ToString() + " and " +
					"ST_RID = " + storeRid.ToString() );

				if (rows.Length > 0)
				{
					DataRow row = rows[0];
					stp.StoreRid = Convert.ToInt32(row["ST_RID"]);
					stp.VariableNumber = Convert.ToInt32(row["VARIABLE_NUMBER"]);
					//int vartType = Convert.ToInt32(row["VARIABLE_TYPE"], CultureInfo.CurrentUICulture);
					stp.Set = Convert.ToInt32(row["SGL_RID"]);
					stp.Index = Convert.ToDouble(row["STORE_GRADE_INDEX"]);
					stp.Units = Convert.ToInt32(row["UNITS"]);
					stp.AverageStore = Convert.ToDouble(row["AVERAGE_STORE"]);
					stp.GradeBoundary = Convert.ToInt32(row["STORE_GRADE_BOUNDARY"]);
				}
				return stp;
			}
			catch
			{
				throw;
			}
		}

		public void WriteAssortmentStoreSummary(Header headerData)
		{
			try
			{
				//========================
				// Write Total Assortment
				//========================
				if (headerData.DeleteAssortmentStoreSummary(this.Key))
				{
					headerData.WriteAssortmentStoreSummary(this.Key, _dtAssortmentStoreSummary);
				}
			}
			catch
			{
				throw;
			}
		}

		public void WriteAssortmentStoreEligibility()
		{
			AssortmentDetailData assortDetailData = new AssortmentDetailData();
			try
			{
				if (!assortDetailData.ConnectionIsOpen)
					assortDetailData.OpenUpdateConnection();
				assortDetailData.AssortmentStoreEligibility_Delete(this.Key);

				IDictionaryEnumerator storeEnumerator = _storeEligibilityHash.GetEnumerator();

				while (storeEnumerator.MoveNext())
				{
					int storeRid = (int)storeEnumerator.Key;
					bool isEligible = (bool)storeEnumerator.Value;
					assortDetailData.AssortmentStoreEligibility_Insert(this.Key, storeRid, isEligible);
				}
				assortDetailData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				assortDetailData.CloseUpdateConnection();
			}
		}

		public DataTable ReadAssortmentStoreEligibility()
		{
			AssortmentDetailData assortDetailData = new AssortmentDetailData();
			try
			{
				//if (!assortDetailData.ConnectionIsOpen)
				//    assortDetailData.OpenUpdateConnection();
				
				return assortDetailData.AssortmentStoreEligibility_Read(_assortmentProfile.Key);
			}
			catch
			{
				throw;
			}
			finally
			{
				//assortDetailData.CloseUpdateConnection();
			}
		}

		// Begin TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
        public void SetGradeList(DataTable newGradeList)
        {
            _gradeList = new ProfileList(eProfileType.StoreGrade);
            int seq = 0;
            foreach (DataRow aRow in newGradeList.Rows)
            {
                string gradeCode = aRow["Grade"].ToString().Trim();
                int boundary = int.Parse(aRow["Boundary"].ToString());
                StoreGradeProfile sgp = new StoreGradeProfile(boundary);
                sgp.Boundary = boundary;
                sgp.StoreGrade = gradeCode;
                sgp.BoundaryUnits = boundary;

                _gradeList.Add(sgp);	
            }
        }
		// End TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
	}
	//Begin TT#2 - JScott - Assortment Planning - Phase 2
	//#region
	//public class AssortmentBasisReader
	//{
	//    private PlanCubeGroup _cubeGroup;
	//    private PlanOpenParms _openParms;
	//    private SessionAddressBlock SAB;
	//    private string _computationsMode;
	//    private int _anchorNodeRid;
	//    private List<int> _hierNodeList;
	//    private List<int> _versionList;
	//    private List<int> _dateRangeList;
	//    private List<double> _weightList;
	//    private int _sgRid;
	//    private ApplicationSessionTransaction _trans;
	//    //private int _storeFilterRid;
	//    private ProfileList _storeList;
	//    private bool _inclSimStore;
	//    private bool _inclIntrasit;
	//    private bool _inclOnHand;
	//    private bool _inclCommitted;


	//    public AssortmentBasisReader(SessionAddressBlock sab, ApplicationSessionTransaction trans,
	//        int anchorNodeRid,
	//        List<int> hierNodeList,
	//        List<int> versionList,
	//        List<int> dateRangeList,
	//        List<double> weightList,
	//        int sgRid,
	//        bool inclSimStore,
	//        bool inclIntransit,
	//        bool inclOnHand,
	//        bool inclCommitted,
	//        ProfileList storeList)
	//    {
	//        _anchorNodeRid = anchorNodeRid;
	//        _hierNodeList = hierNodeList;
	//        _versionList = versionList;
	//        _dateRangeList = dateRangeList;
	//        _weightList = weightList;
	//        _trans = trans;
	//        SAB = sab;
	//        _sgRid = sgRid;
	//        _inclSimStore = inclSimStore;
	//        _inclIntrasit = inclIntransit;
	//        _inclOnHand = inclOnHand;
	//        _inclCommitted = inclCommitted;
	//        _storeList = storeList;

	//        _cubeGroup = (PlanCubeGroup)_trans.CreateStorePlanMaintCubeGroup();
	//        _computationsMode = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;

	//        // Build plan data from first basis information
	//        FillOpenParmForPlan();
	//        _openParms.BasisProfileList.Clear();
	//        FillOpenParmForBasis();
	//        ((PlanCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

	//    }

	//    public int GetVariableNumber(eAssortmentVariableType variableType)
	//    {
	//        try
	//        {
	//            int varNumber = Include.Undefined;
	//            switch (variableType)
	//            {
	//                case eAssortmentVariableType.Sales:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                case eAssortmentVariableType.Stock:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                case eAssortmentVariableType.Receipts:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptRegularUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                default:
	//                    string msg = MIDText.GetText(eMIDTextCode.msg_as_InvalidVariable);
	//                    msg = msg.Replace("{0}", variableType.ToString());
	//                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_as_InvalidVariable, msg);
	//            }

	//            return varNumber;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }


	//    public double GetBasisTotalUnits(eAssortmentVariableType variableType)
	//    {
	//        try
	//        {
	//            Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisStoreTotalDateTotal);
	//            PlanCellReference planCellRef = new PlanCellReference((StoreBasisStoreTotalDateTotal)myCube);
	//            SetCommonPlanCellRefIndexes(variableType, planCellRef);
	//            planCellRef[eProfileType.Basis] = 1;
	//            double totalValue = planCellRef.HiddenCurrentCellValue;
	//            return totalValue;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    public double GetPlanActualTotalUnits(eAssortmentVariableType variableType)
	//    {
	//        try
	//        {
	//            Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanStoreTotalDateTotal);
	//            PlanCellReference planCellRef = new PlanCellReference((StorePlanStoreTotalDateTotal)myCube);
	//            SetCommonPlanCellRefIndexes(variableType, planCellRef);
	//            //===========================================================
	//            // NOTE: The version is ALWAYS being overriden to be Actuals
	//            //===========================================================
	//            planCellRef[eProfileType.Version] = Include.FV_ActualRID;
	//            double totalValue = planCellRef.HiddenCurrentCellValue;
	//            return totalValue;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    public List<int> GetBasisStoreUnits(eAssortmentVariableType variableType, List<bool> isStoreEligibleList)
	//    {
	//        try
	//        {
	//            List<int> valueList = new List<int>();
	//            Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisDateTotal);
	//            PlanCellReference planCellRef = new PlanCellReference((StoreBasisDateTotal)myCube);
	//            SetCommonPlanCellRefIndexes(variableType, planCellRef);
	//            planCellRef[eProfileType.Basis] = 1;
	//            ArrayList storeBasisValues = planCellRef.GetCellRefArray(_storeList);
	//            for (int i = 0; i < storeBasisValues.Count; i++)
	//            {
	//                PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
	//                valueList.Add((int)pcr.HiddenCurrentCellValue);
	//            }
	//            return valueList;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }


	//    public List<int> GetPlanActualStoreUnits(eAssortmentVariableType variableType)
	//    {
	//        try
	//        {
	//            List<int> valueList = new List<int>();
	//            Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanDateTotal);
	//            PlanCellReference planCellRef = new PlanCellReference((StorePlanDateTotal)myCube);
	//            SetCommonPlanCellRefIndexes(variableType, planCellRef);
	//            //===========================================================
	//            // NOTE: The version is ALWAYS being overriden to be Actuals
	//            //===========================================================
	//            planCellRef[eProfileType.Version] = Include.FV_ActualRID;
	//            ArrayList storePlanValues = planCellRef.GetCellRefArray(_storeList);
	//            for (int i = 0; i < storePlanValues.Count; i++)
	//            {
	//                PlanCellReference pcr = (PlanCellReference)storePlanValues[i];
	//                valueList.Add((int)pcr.HiddenCurrentCellValue);
	//            }
	//            return valueList;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    private void SetCommonPlanCellRefIndexes(eAssortmentVariableType variableType, PlanCellReference planCellRef)
	//    {
	//        try
	//        {
	//            planCellRef[eProfileType.Version] = _versionList[0];
	//            planCellRef[eProfileType.HierarchyNode] = _hierNodeList[0];
	//            planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

	//            switch (variableType)
	//            {
	//                case eAssortmentVariableType.Sales:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalSalesRegPromoUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalSalesTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                case eAssortmentVariableType.Stock:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.BeginningInventoryRegularUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.BeginningInventoryTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                case eAssortmentVariableType.Receipts:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptRegularUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalReceiptRegularUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptTotalUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalReceiptTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                case eAssortmentVariableType.Intransit:
	//                    planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.IntransitVariable.Key;
	//                    planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalIntransitVariable.Key;
	//                    break;
	//                case eAssortmentVariableType.Onhand:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalOnHandRegularUnitsVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalOnHandTotalUnitsVariable.Key;
	//                    }
	//                    break;
	//                case eAssortmentVariableType.Need:
	//                    if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalNeedRegularVariable.Key;
	//                    }
	//                    else
	//                    {
	//                        planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
	//                        planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalNeedTotalVariable.Key;
	//                    }
	//                    break;
	//                default:
	//                    string msg = MIDText.GetText(eMIDTextCode.msg_as_InvalidVariable);
	//                    msg = msg.Replace("{0}", variableType.ToString());
	//                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_as_InvalidVariable, msg);
	//            }
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    /// <summary>
	//    /// Fills in the plan part of the CubeGroup open parms
	//    /// </summary>
	//    private void FillOpenParmForPlan()
	//    {
	//        _openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _computationsMode);
	//        _openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.AssortmentMethodsUserGeneralAssortment);
	//        _openParms.FunctionSecurityProfile.SetAllowUpdate();

	//        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_anchorNodeRid);
	//        hnp.ChainSecurityProfile.SetReadOnly();
	//        hnp.StoreSecurityProfile.SetReadOnly();
	//        VersionProfile vp = new VersionProfile(_versionList[0]);
	//        vp.StoreSecurity = new VersionSecurityProfile(_versionList[0]);
	//        vp.StoreSecurity.SetReadOnly();
	//        vp.ChainSecurity = new VersionSecurityProfile(_versionList[0]);
	//        vp.ChainSecurity.SetReadOnly();

	//        _openParms.StoreHLPlanProfile.VersionProfile = vp;
	//        _openParms.StoreHLPlanProfile.NodeProfile = hnp;
	//        _openParms.ChainHLPlanProfile.VersionProfile = vp;
	//        _openParms.ChainHLPlanProfile.NodeProfile = hnp;

	//        int maxWeeks = 1;
	//        foreach (int dateRangeRid in _dateRangeList)
	//        {
	//            DateRangeProfile drp = SAB.ApplicationServerSession.Calendar.GetDateRange(dateRangeRid);
	//            ProfileList weekList = SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);
	//            maxWeeks = Math.Max(maxWeeks, weekList.Count);
	//        }

	//        _openParms.DateRangeProfile = SAB.ApplicationServerSession.Calendar.AddDateRangeWithCurrent(maxWeeks);

	//        _openParms.StoreGroupRID = this._sgRid;
	//        _openParms.FilterRID = Include.UndefinedStoreFilter;

	//        //			_openParms.LowLevelsType = this.LowLevelsType;
	//        //			_openParms.LowLevelsOffset = this.LowLevelsOffset;
	//        //			_openParms.LowLevelsSequence = this.LowLevelsSequence;

	//        _openParms.IneligibleStores = true;
	//        //if (!_openParms.IneligibleStores)
	//        //{
	//        //    eligFilter = new EligibilityFilter(this);
	//        //    ApplyFilter(eligFilter, eFilterType.Permanent);
	//        //}

	//        _openParms.SimilarStores = _inclSimStore;

	//        if (_computationsMode != null)
	//        {
	//            _openParms.ComputationsMode = _computationsMode;
	//        }
	//        else
	//        {
	//            _openParms.ComputationsMode = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
	//        }
	//    }

	//    /// <summary>
	//    /// Fills in the basis part of the CubeGroup open parms.
	//    /// </summary>
	//    private void FillOpenParmForBasis()
	//    {
	//        BasisProfile basisProfile;
	//        BasisDetailProfile basisDetailProfile;
	//        int bdpKey = 1;
	//        HierarchyNodeProfile hnp = null;

	//        //=======================
	//        // Set up Basis Profile
	//        //=======================
	//        basisProfile = new BasisProfile(1, null, _openParms);
	//        basisProfile.BasisType = eTyLyType.NonTyLy;

	//        int maxBasis = _hierNodeList.Count;
	//        for (int basisRow = 0; basisRow < maxBasis; basisRow++)
	//        {

	//            hnp = SAB.HierarchyServerSession.GetNodeData(_hierNodeList[basisRow]);

	//            basisDetailProfile = new BasisDetailProfile(bdpKey++, _openParms);
	//            basisDetailProfile.VersionProfile = new VersionProfile(_versionList[basisRow]);
	//            basisDetailProfile.HierarchyNodeProfile = hnp;
	//            DateRangeProfile drp = SAB.ApplicationServerSession.Calendar.GetDateRange(_dateRangeList[basisRow]);
	//            basisDetailProfile.DateRangeProfile = drp;
	//            basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
	//            // The "weights" are really "percents", so we change them to weights.
	//            double wgt = _weightList[basisRow] / 100;
	//            basisDetailProfile.Weight = (float)wgt;
	//            basisProfile.BasisDetailProfileList.Add(basisDetailProfile);

	//        }
	//        _openParms.BasisProfileList.Add(basisProfile);
	//    }
	//}
	//#endregion

	//End TT#2 - JScott - Assortment Planning - Phase 2
	#region
	public class AssortmentSummary
	{
		//=======
		// FIELDS
		//=======

		//private int _headerRid;
		private StoreGroupProfile _storeGroupProfile;
		private ProfileList _variableItemList;
		private ProfileList _storeGradeList;
		private DataTable _dtSummary;
		// Begin TT#1124-MD - stodd - need not matching style review - 
        private ApplicationSessionTransaction _trans;
        List<int> _storeList = null;
        ProfileList _storeProfileList = null;
		// End TT#1124-MD - stodd - need not matching style review - 

        private AssortmentSummaryProfile _assortmentSummaryProfile = null;	// TT#1198-MD - stodd - cannot open assortment - 

		//public int HeaderRid
		//{
		//    get { return _headerRid; }
		//    set { _headerRid = value; }
		//}

		public StoreGroupProfile StoreGroupProfile
		{
			get { return _storeGroupProfile; }
			set { _storeGroupProfile = value; }
		}

		public ProfileList StoreGradeList
		{
			get { return _storeGradeList; }
			set { _storeGradeList = value; }
		}


		//=============
		// CONSTRUCTORS
		//=============

        public AssortmentSummary(AssortmentSummaryProfile assortmentSummaryProfile, StoreGroupProfile storeGroupProfile, ProfileList storeGradeList)	// TT#1198-MD - stodd - cannot open assortment - 
		{
			//_headerRid = headerRid;
            _assortmentSummaryProfile = assortmentSummaryProfile;	// TT#1198-MD - stodd - cannot open assortment - 
			_storeGroupProfile = storeGroupProfile;
			_storeGradeList = storeGradeList;
			_variableItemList = new ProfileList(eProfileType.AssortmentSummaryItem);
			CreateSummaryTable();
		}

		//============
		// Methods
		//============

		/// <summary>
		/// Expects valid varNum, setRid, and storeIndex.
		/// Atuomatically creates and adds to the variable record and the variable/set record. 
		/// </summary>
		/// <param name="varNum"></param>
		/// <param name="setRid"></param>
		/// <param name="storeIndex"></param>
		/// <param name="units"></param>
		/// <param name="avgStore"></param>
		/// <param name="basis"></param>
		/// <param name="intransit"></param>
		/// <param name="need"></param>
		/// <param name="pctNeed"></param>
		public void Add(int varNum, int varType, int setRid, double storeIndex, int units, double avgStore, int basis,
							int intransit, int onhand, int need, decimal pctNeed, int gradeBoundary, int VSWOnhand, int planSales, int planStock)	// TT#845-MD - Stodd - add OnHand to Summary  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
		{
			try
			{
				// Moved up under read
				//int grade = GetStoreGradeSequence(storeIndex);

				bool newRow = false;
				DataRow aRow = GetSummaryRow(varNum, setRid, gradeBoundary, out newRow);

				//============================
				// Variable/Set/Grade record
				//============================
				int oldUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
				aRow["UNITS"] = oldUnits + units;
				int oldNumStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
				aRow["NUM_STORES"] = ++oldNumStores;
				//int oldAvgStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
				aRow["AVG_STORE"] = avgStore;
				//int oldAvgUnits = Convert.ToInt32(aRow["AVG_UNITS"], CultureInfo.CurrentUICulture);
				//aRow["AVG_UNITS"] = 0;
				//int oldIndex = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
				aRow["INDEX"] = storeIndex;
				int oldBasis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
                aRow["BASIS"] = oldBasis + basis;	// TT#952 - MD - stodd - add matrix to Group Allocation Review
				int oldNeed = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
				aRow["NEED"] = oldNeed + need;
                // Begin TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				//int oldPctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
                //aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldUnits + units); // TT#1119 - md -stodd - summary calculations wrong 
                int oldPlanSales = Convert.ToInt32(aRow["PLAN_SALES_UNITS"], CultureInfo.CurrentUICulture);
                aRow["PLAN_SALES_UNITS"] = oldPlanSales + planSales;
                int oldPlanStock = Convert.ToInt32(aRow["PLAN_STOCK_UNITS"], CultureInfo.CurrentUICulture);
                aRow["PLAN_STOCK_UNITS"] = oldPlanStock + planStock;
                if (_assortmentSummaryProfile.IsGroupAllocation)
                {
                    aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldUnits + units); // TT#1119 - md -stodd - summary calculations wrong 
                }
                else
                {
                    aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldPlanSales + planSales + oldPlanStock + planStock);
                }
                // End TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				int oldIntransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
				aRow["INTRANSIT"] = oldIntransit + intransit;
				// BEGIN TT#845-MD - Stodd - add OnHand to Summary
				int oldOnhand = Convert.ToInt32(aRow["ONHAND"]);
				aRow["ONHAND"] = oldOnhand + onhand;
				// END TT#845-MD - Stodd - add OnHand to Summary
                int oldVSWOnhand = Convert.ToInt32(aRow["VSW_ONHAND"]);
                aRow["VSW_ONHAND"] = oldVSWOnhand + VSWOnhand;
				aRow["VARIABLE_TYPE"] = varType;
				if (newRow)
					_dtSummary.Rows.Add(aRow);

				//===================
				// Variable Record
				//===================
				aRow = GetSummaryRow(varNum, Include.Undefined, Include.Undefined, out newRow);
				oldUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
				aRow["UNITS"] = oldUnits + units;
				oldNumStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
				aRow["NUM_STORES"] = ++oldNumStores;
				//int oldAvgStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
				aRow["AVG_STORE"] = avgStore;
				//int oldAvgUnits = Convert.ToInt32(aRow["AVG_UNITS"], CultureInfo.CurrentUICulture);
				//aRow["AVG_UNITS"] = 0;
				//int oldIndex = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
				aRow["INDEX"] = storeIndex;
				oldBasis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
                aRow["BASIS"] = oldBasis + basis;	// TT#952 - MD - stodd - add matrix to Group Allocation Review
				oldNeed = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
				aRow["NEED"] = oldNeed + need;
				//int oldPctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
                // Begin TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                //aRow["PCT_NEED"] =  Need.PctUnitNeed(oldNeed+need, oldUnits+units);
                oldPlanSales = Convert.ToInt32(aRow["PLAN_SALES_UNITS"], CultureInfo.CurrentUICulture);
                aRow["PLAN_SALES_UNITS"] = oldPlanSales + planSales;
                oldPlanStock = Convert.ToInt32(aRow["PLAN_STOCK_UNITS"], CultureInfo.CurrentUICulture);
                aRow["PLAN_STOCK_UNITS"] = oldPlanStock + planStock;
                if (_assortmentSummaryProfile.IsGroupAllocation)
                {
                    aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldUnits + units);
                }
                else
                {
                    aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldPlanSales + planSales + oldPlanStock + planStock);
                }
                // End TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				oldIntransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
				aRow["INTRANSIT"] = oldIntransit + intransit;
				// BEGIN TT#845-MD - Stodd - add OnHand to Summary
				oldOnhand = Convert.ToInt32(aRow["ONHAND"]);
				aRow["ONHAND"] = oldOnhand + onhand;
				// END TT#845-MD - Stodd - add OnHand to Summary
                oldVSWOnhand = Convert.ToInt32(aRow["VSW_ONHAND"]);
                aRow["VSW_ONHAND"] = oldVSWOnhand + VSWOnhand;

				aRow["VARIABLE_TYPE"] = varType;
				if (newRow)
					_dtSummary.Rows.Add(aRow);

				//======================
				// Variable/Set Record
				//======================
				aRow = GetSummaryRow(varNum, setRid, Include.Undefined, out newRow);
				oldUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
				aRow["UNITS"] = oldUnits + units;
				oldNumStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
				aRow["NUM_STORES"] = ++oldNumStores;
				//int oldAvgStore = Convert.ToInt32(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
				aRow["AVG_STORE"] = avgStore;
				//int oldAvgUnits = Convert.ToInt32(aRow["AVG_UNITS"], CultureInfo.CurrentUICulture);
				//aRow["AVG_UNITS"] = 0;
				//int oldIndex = Convert.ToInt32(aRow["INDEX"], CultureInfo.CurrentUICulture);
				aRow["INDEX"] = storeIndex;
				oldBasis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
                aRow["BASIS"] = oldBasis + basis;	// TT#952 - MD - stodd - add matrix to Group Allocation Review
				oldNeed = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
				aRow["NEED"] = oldNeed + need;
				//int oldPctNeed = Convert.ToInt32(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
                // Begin TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                //aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldUnits + units);
                oldPlanSales = Convert.ToInt32(aRow["PLAN_SALES_UNITS"], CultureInfo.CurrentUICulture);
                aRow["PLAN_SALES_UNITS"] = oldPlanSales + planSales;
                oldPlanStock = Convert.ToInt32(aRow["PLAN_STOCK_UNITS"], CultureInfo.CurrentUICulture);
                aRow["PLAN_STOCK_UNITS"] = oldPlanStock + planStock;
                if (_assortmentSummaryProfile.IsGroupAllocation)
                {
                    aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldUnits + units);
                }
                else
                {
                    aRow["PCT_NEED"] = Need.PctUnitNeed(oldNeed + need, oldPlanSales + planSales + oldPlanStock + planStock);
                }
                // End TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
				oldIntransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
				aRow["INTRANSIT"] = oldIntransit + intransit;
				// BEGIN TT#845-MD - Stodd - add OnHand to Summary
				oldOnhand = Convert.ToInt32(aRow["ONHAND"]);
				aRow["ONHAND"] = oldOnhand + onhand;
				// END TT#845-MD - Stodd - add OnHand to Summary
                oldVSWOnhand = Convert.ToInt32(aRow["VSW_ONHAND"]);
                aRow["VSW_ONHAND"] = oldVSWOnhand + VSWOnhand;

				aRow["VARIABLE_TYPE"] = varType;
				if (newRow)
					_dtSummary.Rows.Add(aRow);

			}
			catch
			{
				throw;
			}
		}

        private DataRow GetSummaryRow(int varNum, int setRid, int grade, out bool newRow)
        {
            DataRow aRow = null;
            try
            {
                DataRow[] rows = _dtSummary.Select("VARIABLE_NUMBER = " + varNum.ToString() + " and " +
                    "GROUP_LEVEL_RID = " + setRid.ToString() + " and " +
                    "GRADE = " + grade.ToString());

                newRow = false;
                if (rows.Length == 0)
                {
                    aRow = _dtSummary.NewRow();
                    aRow["VARIABLE_NUMBER"] = varNum;
                    aRow["GROUP_LEVEL_RID"] = setRid;
                    aRow["GRADE"] = grade;
                    aRow["UNITS"] = 0;
                    aRow["NUM_STORES"] = 0;
                    aRow["AVG_STORE"] = 0;
                    aRow["AVG_UNITS"] = 0;
                    aRow["INDEX"] = 0;
                    aRow["BASIS"] = 0;
                    aRow["NEED"] = 0;
                    aRow["PCT_NEED"] = 0;
                    aRow["INTRANSIT"] = 0;
                    aRow["ONHAND"] = 0;		// TT#845-MD - Stodd - add OnHand to Summary
                    aRow["VSW_ONHAND"] = 0;
                    aRow["PLAN_SALES_UNITS"] = 0;  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    aRow["PLAN_STOCK_UNITS"] = 0;  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    newRow = true;
                }
                else
                {
                    aRow = rows[0];
                }
                return aRow;
            }
            catch
            {
                throw;
            }
        }

		// Begin TT#1189-md - stodd - adding locking to group allocation
		private DataRow GetSummaryRow(int varNum, int setRid, out bool newRow)
		{
			DataRow aRow = null;
			try
			{
				DataRow[] rows = _dtSummary.Select("VARIABLE_NUMBER = " + varNum.ToString() + " and " +
					"GROUP_LEVEL_RID = " + setRid.ToString() + " and " +
					"GRADE = -1");

				newRow = false;
				if (rows.Length == 0)
				{
					aRow = _dtSummary.NewRow();
					aRow["VARIABLE_NUMBER"] = varNum;
					aRow["GROUP_LEVEL_RID"] = setRid;
					aRow["GRADE"] = -1;
					aRow["UNITS"] = 0;
					aRow["NUM_STORES"] = 0;
					aRow["AVG_STORE"] = 0;
					aRow["AVG_UNITS"] = 0;
					aRow["INDEX"] = 0;
					aRow["BASIS"] = 0;
					aRow["NEED"] = 0;
					aRow["PCT_NEED"] = 0;
					aRow["INTRANSIT"] = 0;
					aRow["ONHAND"] = 0;		// TT#845-MD - Stodd - add OnHand to Summary
                    aRow["VSW_ONHAND"] = 0;
                    aRow["PLAN_SALES_UNITS"] = 0;  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
                    aRow["PLAN_STOCK_UNITS"] = 0;  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
					newRow = true;
				}
				else
				{
					aRow = rows[0];
				}
				return aRow;
			}
			catch
			{
				throw;
			}
		}
		// End TT#1189-md - stodd - adding locking to group allocation

		/// <summary>
		///  Clears the Assortment Summary class profile list and member profile lists
		/// </summary>
		public void Clear()
		{
			try
			{
				_dtSummary.Clear();
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1124-MD - stodd - need not matching style review - 
        public void SetTransaction(ApplicationSessionTransaction trans)
        {
            _trans = trans;
        }

        public void SetStoreLists(ProfileList storeProfileList, List<int> storeList)
        {
            _storeProfileList = storeProfileList;
            _storeList = storeList;
        }
		// End TT#1124-MD - stodd - need not matching style review - 

		private void CreateSummaryTable()
		{
			_dtSummary = MIDEnvironment.CreateDataTable("Summary Table");

			_dtSummary.Columns.Add(new DataColumn("VARIABLE_NUMBER", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("VARIABLE_TYPE", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("GROUP_LEVEL_RID", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("GRADE", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("UNITS", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("NUM_STORES", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("AVG_STORE", typeof(decimal)));
			_dtSummary.Columns.Add(new DataColumn("AVG_UNITS", typeof(decimal)));
			_dtSummary.Columns.Add(new DataColumn("INDEX", typeof(decimal)));
			_dtSummary.Columns.Add(new DataColumn("BASIS", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("NEED", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("PCT_NEED", typeof(decimal)));
			_dtSummary.Columns.Add(new DataColumn("INTRANSIT", typeof(int)));
			_dtSummary.Columns.Add(new DataColumn("ONHAND", typeof(int)));	// TT#845-MD - Stodd - add OnHand to Summary
            _dtSummary.Columns.Add(new DataColumn("VSW_ONHAND", typeof(int)));
            _dtSummary.Columns.Add(new DataColumn("PLAN_SALES_UNITS", typeof(int)));  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
            _dtSummary.Columns.Add(new DataColumn("PLAN_STOCK_UNITS", typeof(int)));  // TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.

			_dtSummary.PrimaryKey = new DataColumn[] {_dtSummary.Columns["VARIABLE_NUMBER"], 
												 _dtSummary.Columns["GROUP_LEVEL_RID"],
												 _dtSummary.Columns["GRADE"]};

		}

		public AssortmentSummaryItemProfile GetSummary(int variableNumber, int setRid, int storeGrade)
		{
			AssortmentSummaryItemProfile summaryProfile = new AssortmentSummaryItemProfile(variableNumber);
			summaryProfile.VariableNumber = variableNumber;
			summaryProfile.Set = setRid;
			summaryProfile.Grade = storeGrade;
			try
			{
				bool newRow = false;
				DataRow aRow = GetSummaryRow(variableNumber, setRid, storeGrade, out newRow);

				if (aRow != null)
				{
					summaryProfile.NumberOfStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
					summaryProfile.TotalUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
					summaryProfile.AverageStore = Convert.ToDouble(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
					summaryProfile.Index = Convert.ToDouble(aRow["INDEX"], CultureInfo.CurrentUICulture);
					summaryProfile.Basis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
					summaryProfile.Intransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
					summaryProfile.Need = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
					summaryProfile.PctNeed = Convert.ToDecimal(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
					summaryProfile.OnHand = Convert.ToInt32(aRow["ONHAND"]);	// TT#845-MD - Stodd - add OnHand to Summary
                    summaryProfile.VSWOnHand = Convert.ToInt32(aRow["VSW_ONHAND"]);
				}

				return summaryProfile;
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1189-md - stodd - adding locking to group allocation
        public AssortmentSummaryItemProfile GetSummary(int variableNumber, int setRid)
        {
            AssortmentSummaryItemProfile summaryProfile = new AssortmentSummaryItemProfile(variableNumber);
            summaryProfile.VariableNumber = variableNumber;
            summaryProfile.Set = setRid;
            summaryProfile.Grade = -1;
            try
            {
                bool newRow = false;
                DataRow aRow = GetSummaryRow(variableNumber, setRid, out newRow);

                if (aRow != null)
                {
                    summaryProfile.NumberOfStores = Convert.ToInt32(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
                    summaryProfile.TotalUnits = Convert.ToInt32(aRow["UNITS"], CultureInfo.CurrentUICulture);
                    summaryProfile.AverageStore = Convert.ToDouble(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
                    summaryProfile.Index = Convert.ToDouble(aRow["INDEX"], CultureInfo.CurrentUICulture);
                    summaryProfile.Basis = Convert.ToInt32(aRow["BASIS"], CultureInfo.CurrentUICulture);
                    summaryProfile.Intransit = Convert.ToInt32(aRow["INTRANSIT"], CultureInfo.CurrentUICulture);
                    summaryProfile.Need = Convert.ToInt32(aRow["NEED"], CultureInfo.CurrentUICulture);
                    summaryProfile.PctNeed = Convert.ToDecimal(aRow["PCT_NEED"], CultureInfo.CurrentUICulture);
                    summaryProfile.OnHand = Convert.ToInt32(aRow["ONHAND"]);	// TT#845-MD - Stodd - add OnHand to Summary
                    summaryProfile.VSWOnHand = Convert.ToInt32(aRow["VSW_ONHAND"]);
                }

                return summaryProfile;
            }
            catch
            {
                throw;
            }
        }
		// End TT#1189-md - stodd - adding locking to group allocation

		public void FinishSummary()
		{
            try
            {
				// Begin TT#1198-MD - stodd - cannot open assortment - 
                AllocationSubtotalProfile asp = null;
                if (_assortmentSummaryProfile.IsGroupAllocation)
                {
                    asp = _trans.GetAllocationGrandTotalProfile();	// TT#1124-MD - stodd - need not matching style review - 
                }
				// End TT#1198-MD - stodd - cannot open assortment - 
                //=================================================
                // variable only rows (total for assortment)
                //=================================================
                DataRow[] rows = _dtSummary.Select("GROUP_LEVEL_RID = -1 and GRADE = -1");
                foreach (DataRow aRow in rows)
                {
                    // Calc Average Store for variable
                    double units = Convert.ToDouble(aRow["UNITS"], CultureInfo.CurrentUICulture);
                    double noOfStores = Convert.ToDouble(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
                    double averageStore = 0;
                    if (noOfStores > 0)
                    {
                        // Begin TT#811-MD - stodd - Rounding was incorrect
                        //averageStore = ((Double) ((int)(units * 100 / noOfStores) + .5) / 100);
                        //averageStore = ((units / noOfStores) + .005);
                        //averageStore = Math.Round(averageStore, 2);
                        averageStore = (int)((units * 100 / noOfStores) + .5);
                        averageStore = (double)averageStore / 100;
                        // End TT#811-MD - stodd - Rounding was incorrect
                    }
                    aRow["AVG_STORE"] = averageStore;
                    aRow["INDEX"] = 100;

					// begin TT#1198-MD - stodd - cannot open assortment - 
                    if (asp != null)
                    {
                        // Begin TT#1124-MD - stodd - need not matching style review - 
                        double pctNeed = asp.GetStoreListTotalPercentNeed(_storeProfileList);
                        aRow["PCT_NEED"] = pctNeed;
                        // End TT#1124-MD - stodd - need not matching style review - 
                    }
					// End TT#1198-MD - stodd - cannot open assortment - 
                }
                // BEGIN TT#205-MD - stodd - Index wrong
                _dtSummary.AcceptChanges();
                // END TT#205-MD - stodd - Index wrong

                //====================================
                // variable/set rows (Set totals)
                //====================================
                bool newRow = false;
                rows = _dtSummary.Select("GROUP_LEVEL_RID <> -1 and GRADE = -1");
                foreach (DataRow aRow in rows)
                {
                    int varNum = Convert.ToInt32(aRow["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
                    // calc index for set
                    DataRow varRow = GetSummaryRow(varNum, Include.Undefined, Include.Undefined, out newRow);
                    double varAvgStore = Convert.ToDouble(varRow["AVG_STORE"], CultureInfo.CurrentUICulture);
                    // BEGIN TT#205-MD - stodd - Index wrong
                    //double setAvgStore = Convert.ToDouble(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
                    // END TT#205-MD - stodd - Index wrong

                    double units = Convert.ToDouble(aRow["UNITS"], CultureInfo.CurrentUICulture);
                    double noOfStores = Convert.ToDouble(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
                    double index = 0;
                    double averageStore = 0;
                    if (noOfStores > 0)
                    {
                        // Begin TT#811-MD - stodd - Rounding was incorrect
                        //averageStore = ((units / noOfStores) + .005);
                        //averageStore = Math.Round(averageStore, 2);
                        averageStore = (int)((units * 100 / noOfStores) + .5);
                        averageStore = (double)averageStore / 100;
                        // End TT#811-MD - stodd - Rounding was incorrect
                    }
                    if (varAvgStore > 0)
                    {
                        // BEGIN TT#205-MD - stodd - Index wrong
                        //index = (((setAvgStore * 100) / varAvgStore) + .5);
                        index = (((averageStore * 100) / varAvgStore));
                        // END TT#205-MD - stodd - Index wrong
                        index = Math.Round(index, 2);
                    }
                    aRow["INDEX"] = index;
                    aRow["AVG_STORE"] = averageStore;

					// Begin TT#1198-MD - stodd - cannot open assortment - 
                    if (asp != null)
                    {
                        // Begin TT#1124-MD - stodd - need not matching style review - 
                        int groupLevelRid = Convert.ToInt32(aRow["GROUP_LEVEL_RID"]);
                        double pctNeed = asp.GetStoreListTotalPercentNeed(StoreGroupProfile.Key, groupLevelRid);
                        aRow["PCT_NEED"] = pctNeed;
                        // End TT#1124-MD - stodd - need not matching style review -
                    }
					// End TT#1198-MD - stodd - cannot open assortment - 
                }

                // BEGIN TT#205-MD - stodd - Index wrong
                _dtSummary.AcceptChanges();
                // END TT#205-MD - stodd - Index wrong

                //=====================
                // variable/set rows
                //=====================
                newRow = false;
                rows = _dtSummary.Select("GROUP_LEVEL_RID <> -1 and GRADE <> -1");
                foreach (DataRow aRow in rows)
                {
                    int varNum = Convert.ToInt32(aRow["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
                    int setRid = Convert.ToInt32(aRow["GROUP_LEVEL_RID"], CultureInfo.CurrentUICulture);
                    // Calc Average Store for grade
                    double units = Convert.ToDouble(aRow["UNITS"], CultureInfo.CurrentUICulture);
                    double noOfStores = Convert.ToDouble(aRow["NUM_STORES"], CultureInfo.CurrentUICulture);
                    double gradeAvgStore = 0;
                    double index = 0;
                    if (noOfStores > 0)
                    {
                        // BEGIN TT#205-MD - stodd - Index wrong
                        gradeAvgStore = ((units / noOfStores) + .005);
                        // END TT#205-MD - stodd - Index wrong
                        // calc index for grade
                        DataRow setRow = GetSummaryRow(varNum, setRid, Include.Undefined, out newRow);
                        double setAvgStore = Convert.ToDouble(aRow["AVG_STORE"], CultureInfo.CurrentUICulture);
                        if (setAvgStore > 0)
                        {
                            // BEGIN TT#205-MD - stodd - Index wrong
                            index = (((gradeAvgStore * 100) / setAvgStore));
                            // END TT#205-MD - stodd - Index wrong
                        }
                        index = Math.Round(index, 2);
                    }
                    aRow["INDEX"] = index;
                    aRow["AVG_STORE"] = gradeAvgStore;

					// Begin TT#1198-MD - stodd - cannot open assortment - 
                    if (asp != null)
                    {
                        // Begin TT#1124-MD - stodd - need not matching style review - 
                        int groupLevelRid = Convert.ToInt32(aRow["GROUP_LEVEL_RID"]);
                        int grade = Convert.ToInt32(aRow["GRADE"]);
                        StoreGradeProfile gradeProfile = (StoreGradeProfile)_storeGradeList.FindKey(grade);
                        double pctNeed = 0;
                        if (asp.SubtotalMembers.Count > 0)
                        {
                            pctNeed = asp.GetStoreListTotalPercentNeed(StoreGroupProfile.Key, groupLevelRid, gradeProfile.StoreGrade);
                        }
                        aRow["PCT_NEED"] = pctNeed;
                        // End TT#1124-MD - stodd - need not matching style review - 
                    }
					// End TT#1198-MD - stodd - cannot open assortment - 
                }

                // BEGIN TT#831-MD - Stodd - Need / Intransit not displayed
                //=================================================================================
                // Need, PctNeed, & Intransit are only calculated once when Sales is gathered but the same values are
                // true for the other variables as well. This takes the values from the Sales
                // rows and updates the other rows with the Need and PctNeed values.
                //=================================================================================
                foreach (DataRow dRow in _dtSummary.Rows)
                {
                    int varType = Convert.ToInt32(dRow["VARIABLE_TYPE"]);
                    if (varType != (int)eAssortmentVariableType.Sales)
                    {
                        int groupLevelRid = Convert.ToInt32(dRow["GROUP_LEVEL_RID"]);
                        int grade = Convert.ToInt32(dRow["GRADE"]);
                        //int varType = Convert.ToInt32(dRow["VARIABLE_TYPE"]);

                        DataRow[] salesRow = _dtSummary.Select("VARIABLE_TYPE = 1 and GROUP_LEVEL_RID = " + groupLevelRid.ToString() + " and GRADE = " + grade.ToString());
                        if (salesRow.Length > 0)
                        {
                            dRow["NEED"] = salesRow[0]["NEED"];
                            dRow["PCT_NEED"] = salesRow[0]["PCT_NEED"];
                            dRow["INTRANSIT"] = salesRow[0]["INTRANSIT"];
                            dRow["ONHAND"] = salesRow[0]["ONHAND"];	// TT#845-MD - Stodd - add OnHand to Summary
                            dRow["VSW_ONHAND"] = salesRow[0]["VSW_ONHAND"];     // TT#831-MD - stodd - VSW showing 0
                        }
                    }
                }
                // END TT#831-MD - Stodd - Need / Intransit not displayed

                _dtSummary.AcceptChanges();
            }
            catch
            {
                throw;
            }
		}

		//private int GetStoreGradeSequence(int index)
		//{
		//    int grade = 0;
		//    for (int i = 0; i < _storeGradeList.Count; i++)
		//    {
		//        StoreGradeProfile sgp = (StoreGradeProfile)_storeGradeList[i];
		//        if (index > sgp.Boundary)
		//        {
		//            grade = sgp.Boundary;
		//            break;
		//        }
		//        else if (index == 0)
		//        {
		//            grade = 0;
		//            break;
		//        }

		//        //int highBoundary = sgp.Boundary;
		//        //int lowBoundary = 0;
		//        //int j = i + 1;

		//        //if (j == 1)
		//        //{
		//        //    lowBoundary = sgp.Boundary;
		//        //    highBoundary = int.MaxValue;
		//        //}
		//        //else if (j < _storeGradeList.Count)
		//        //{
		//        //    StoreGradeProfile nextSgp = (StoreGradeProfile)_storeGradeList[j];
		//        //    lowBoundary = nextSgp.Boundary;
		//        //}

		//        //if (index <= highBoundary && index > lowBoundary)
		//        //{
		//        //    grade = lowBoundary;
		//        //    break;
		//        //}
		//    }
		//    return grade;
		//}

		public void DebugSummary()
		{
			string line = string.Empty;
			for (int i = 0; i < _dtSummary.Columns.Count; i++)
			{
				string caption = _dtSummary.Columns[i].Caption;
				caption = caption.PadLeft(10);
				line += caption + " ";
			}
			Debug.WriteLine(line);
			foreach (DataRow aRow in _dtSummary.Rows)
			{
				line = string.Empty;
				for (int i = 0; i < _dtSummary.Columns.Count; i++)
				{
					string colValue = aRow[i].ToString();
					int length = _dtSummary.Columns[i].Caption.Length;
					colValue = colValue.PadLeft(length < 10 ? 10 : length);
					line += colValue + " ";
				}
				Debug.WriteLine(line);
			}
		}
	}


	#endregion


	#region AssortmentSummaryItemProfile
	public class AssortmentSummaryItemProfile : Profile
	{
		//=======
		// FIELDS
		//=======
		private int _varNum;
		private int _set;
		private int _grade;
		private int _total;
		private int _noOfStores;
		private double _avgStore;
		private double _index;
		private int _basis;
		private int _intransit;
		private int _onhand;	// TT#845-MD - Stodd - add OnHand to Summary
        private int _VSWOnhand;
		private int _committed;  //TT#1224
		private int _need;
		private decimal _pctNeed;
		private ProfileList _list;

		public int VariableNumber
		{
			get { return _varNum; }
			set { _varNum = value; }
		}
		public int Set
		{
			get { return _set; }
			set { _set = value; }
		}
		public int Grade
		{
			get { return _grade; }
			set { _grade = value; }
		}
		public int TotalUnits
		{
			get { return _total; }
			set { _total = value; }
		}
		public int NumberOfStores
		{
			get { return _noOfStores; }
			set { _noOfStores = value; }
		}
		/// <summary>
		/// The Units for the Average store as calculated by the General Assortment Method.
		/// </summary>
		public double AverageStore
		{
			get { return _avgStore; }
			set { _avgStore = value; }
		}
		public double Index
		{
			get { return _index; }
			set { _index = value; }
		}
		public int Basis
		{
			get { return _basis; }
			set { _basis = value; }
		}
		public int Intransit
		{
			get { return _intransit; }
			set { _intransit = value; }
		}
		// BEGIN TT#845-MD - Stodd - add OnHand to Summary
		public int OnHand
		{
			get { return _onhand; }
			set { _onhand = value; }
		}
		// END TT#845-MD - Stodd - add OnHand to Summary
		// Begin TT#952 - MD - stodd - add matrix to Group Allocation Review
        public int VSWOnHand
        {
            get { return _VSWOnhand; }
            set { _VSWOnhand = value; }
        }
		// ENd TT#952 - MD - stodd - add matrix to Group Allocation Review
		// Begin TT#1224 - stodd
		public int Committed
		{
			get { return _committed; }
			set { _committed = value; }
		}
		// End TT#1224 - stodd
		public int Need
		{
			get { return _need; }
			set { _need = value; }
		}
		public decimal PctNeed
		{
			get { return _pctNeed; }
			set { _pctNeed = value; }
		}
		/// <summary>
		/// List of Store Profiles in Store Group and Grade. 
		/// </summary>
		public ProfileList StoreList
		{
			get { return _list; }
			set { _list = value; }
		}

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentSummaryItemProfile(int key)
			: base(key)
		{
			_varNum = Include.Undefined;
			_set = Include.Undefined;
			_grade = Include.Undefined;
			_total = 0;
			_noOfStores = 0;
			_avgStore = 0;
			_index = 0;
			_intransit = 0;
			_onhand = 0;	// TT#845-MD - Stodd - add OnHand to Summary
            _VSWOnhand = 0;
			_committed = 0; // TT#1224 - stodd
			_need = 0;
			_basis = 0;
			_pctNeed = 0.0m;

			_list = new ProfileList(eProfileType.AssortmentSummaryItem);
		}

		/// <summary>
		/// method assumes you are adding one store's total at a time.
		/// </summary>
		/// <param name="total"></param>
		public void Add(int total, int avgStore)
		{
			_total += total;
			_noOfStores++;
			if (_avgStore == 0)
				_avgStore = avgStore;
		}

		public void AddToStoreList(StoreProfile aStore)
		{
			_list.Add(aStore);
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentSummaryItem;
			}
		}
	}
	#endregion

	#region AssortmentSummaryStoreDetailProfile
	public class AssortmentSummaryStoreDetailProfile : Profile
	{
		//=======
		// FIELDS
		//=======
		private int _stRid;
		private int _varNum;
		private int _set;
		private int _boundary;
		private int _units;
		private double _avgStore;
		private double _index;

		public int StoreRid
		{
			get { return _stRid; }
			set { _stRid = value; }
		}
		public int VariableNumber
		{
			get { return _varNum; }
			set { _varNum = value; }
		}
		public int Set
		{
			get { return _set; }
			set { _set = value; }
		}
		public int GradeBoundary
		{
			get { return _boundary; }
			set { _boundary = value; }
		}
		public int Units
		{
			get { return _units; }
			set { _units = value; }
		}
		/// <summary>
		/// The Units for the Average store as calculated by the General Assortment Method.
		/// </summary>
		public double AverageStore
		{
			get { return _avgStore; }
			set { _avgStore = value; }
		}
		public double Index
		{
			get { return _index; }
			set { _index = value; }
		}

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentSummaryStoreDetailProfile(int key)
			: base(key)
		{
			_varNum = Include.Undefined;
			_set = Include.Undefined;
			_boundary = Include.Undefined;
			_units = 0;
			_avgStore = 0;
			_index = 0;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentSummaryStoreDetail;
			}
		}
	}
	#endregion
	
}
