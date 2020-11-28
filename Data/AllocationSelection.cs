using System;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Text;
using MIDRetail.DataCommon;
using System.ComponentModel;
using System.IO;
using System.Globalization;


namespace MIDRetail.Data
{
    // begin TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
    public struct HeaderRID_Type
    {
        private int _headerRID;
        private bool _assortmentType;
        public HeaderRID_Type(int aHeaderRID, bool aIsAssortmentType)
        {
            _headerRID = aHeaderRID;
            _assortmentType = aIsAssortmentType;
        }
        public int HeaderRID
        {
            get { return _headerRID; }
        }
        public bool HeaderIsAssortment
        {
            get { return _assortmentType; }
        }
    }
    // end TT#916 - MD- Jellis - Invalid attemp to instantiate an assortment

	public partial class AllocationSelection: DataLayer
	{
        public AllocationSelection() : base()
        {
            _userID = -1;
            Initialize();
        }
        public AllocationSelection(int aUserID) : base()
        {
            UserID = aUserID;
        }
       
		private int _userID;
		public int UserID
		{
			get{ return _userID;}
			set
			{ 
				_userID = value;
				ReadDefaultsFromDatabase();
				ReadDefaultHeaderIDsFromDatabase();
				_2ndGroupBy = (int)eAllocationSizeView2ndGroupBy.Size;
				// Change to make Size Matrix the default selection
				//_viewIsSequential = true;
				_viewIsSequential = false;
				_analysisOnly = false;
                _dtUserAllocBasis = ReadUserAllocationBasis();  // TT#638 - RMatelic - Style Review - Add Basis Variables
			}
		}

		private eAllocationSelectionViewType _viewType;
		public eAllocationSelectionViewType ViewType
		{
			get{ return _viewType;}
			set{ _viewType = value;}
		}
									  
		private int _storeAttributeID;
		public int StoreAttributeID
		{
			get{ return _storeAttributeID;}
			set{ _storeAttributeID = value;}
		}

		private int _filterID;
		public int FilterID
		{
			get{ return _filterID; }
			set{ _filterID = value; }
		}
        
		private int _groupBy;
		public int GroupBy
		{
			get{ return _groupBy; }
			set{ _groupBy = value; }
		}

		private int _2ndGroupBy;
		public int SecondaryGroupBy
		{
			get{ return _2ndGroupBy; }
			set{ _2ndGroupBy = value; }
		}

		private bool _viewIsSequential;
		public bool ViewIsSequential
		{
			get{ return _viewIsSequential; }
			set{ _viewIsSequential = value; }
		}
        
        // Begin TT#456 - Add Views to Size Review : change viewID to viewRID 
		private int _viewRID;
		public int ViewRID
		{
			get{ return _viewRID;}
			set{ _viewRID = value;}
		}
        // End TT#456

		private int _needAnalysisPeriodBeginRID;
		public int NeedAnalysisPeriodBeginRID
		{
			get{ return _needAnalysisPeriodBeginRID; }
			set{ _needAnalysisPeriodBeginRID = value; }
		}

		private int _needAnalysisPeriodEndRID;
		public int NeedAnalysisPeriodEndRID
		{
			get{ return _needAnalysisPeriodEndRID; }
			set{ _needAnalysisPeriodEndRID = value; }
		}

		private int _needAnalysisHNID;
		public int NeedAnalysisHNID
		{
			get{ return _needAnalysisHNID; }
			set{ _needAnalysisHNID = value; }
		}

		private bool _includeIneligibleStores;
		public bool IncludeIneligibleStores
		{
			get{ return _includeIneligibleStores; }
			set{ _includeIneligibleStores = value; }
		}

		private DataTable _filterTable;
		public DataTable FilterTable 
		{
			get{ return _filterTable; }
			set{ _filterTable = value; }
		}

        // begin TT#916 - MD - Jellis - Invalid attempt to instantiate an Assortment
        //private int [] _allocationHeaderRIDs;
        //public int [] AllocationHeaderRIDS
        //{
        //    get{ return _allocationHeaderRIDs; }
        //    set{ _allocationHeaderRIDs = value; }
        //}
        private HeaderRID_Type[] _allocationHeaderRIDsTypes;
        public HeaderRID_Type[] AllocationHeaderRIDsTypes
        {
            get { return _allocationHeaderRIDsTypes; }
            set { _allocationHeaderRIDsTypes = value; }
        }
        // end TT#916 - MD - Jellis - Invalid attemtp to instantiate an Assortment
		private MIDEnqueue _midNQ;
		
		private bool _analysisOnly;
		public bool AnalysisOnly
		{
			get{ return _analysisOnly; }
			set{ _analysisOnly = value; }
		}
		// BEGIN MID Track #2959 - add Size Need Analysis
		private int _sizeCurveRID = Include.NoRID;
		public int SizeCurveRID
		{
			get{ return _sizeCurveRID; }
			set{ _sizeCurveRID = value; }
		}

		private int _sizeConstraintRID = Include.NoRID;
		public int SizeConstraintRID
		{
			get{ return _sizeConstraintRID; }
			set{ _sizeConstraintRID = value; }
		}
		
		private int _sizeAlternateRID = Include.NoRID;
		public int SizeAlternateRID
		{
			get{ return _sizeAlternateRID; }
			set{ _sizeAlternateRID = value; }
		}
		// END MID Track #2959

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        private DataTable _dtUserAllocBasis;
        public DataTable DTUserAllocBasis
        {
            get { return _dtUserAllocBasis; }
            set { _dtUserAllocBasis = value; }
        }
        // End TT#638



		public void Initialize()
		{
			ViewType = eAllocationSelectionViewType.Style;
			StoreAttributeID = Include.NoRID;
			FilterID = Include.NoRID;
            // Begin TT#3800 - stodd - Selected GroupBy is not implemented for Style View error
            //GroupBy = 1;
            GroupBy = (int)eAllocationStyleViewGroupBy.Header;
            // End TT#3800 - stodd - Selected GroupBy is not implemented for Style View error
			ViewRID = Include.NoRID;
			IncludeIneligibleStores = false;
			NeedAnalysisPeriodBeginRID = Include.NoRID;
			NeedAnalysisPeriodEndRID = Include.NoRID;
			NeedAnalysisHNID = Include.NoRID;
            //_allocationHeaderRIDs = new int[0];               // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
            _allocationHeaderRIDsTypes = new HeaderRID_Type[0]; // TT#916 - MD - Jellis - Invalid attempt to instantiate assortment
			_midNQ = new MIDEnqueue();
			_2ndGroupBy = (int)eAllocationSizeView2ndGroupBy.Size;
            _viewIsSequential = true;
			_analysisOnly = false;
			
			// BEGIN MID Track #2959 - add Size Need Analysis
			_sizeCurveRID = Include.NoRID;
			_sizeConstraintRID = Include.NoRID;
			_sizeAlternateRID = Include.NoRID; 
			// END MID Track #2959
		}

		private void ReadDefaultsFromDatabase()
		{

			try
			{
                //TT#2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_USER_ALLOCATION_READ.Read(_dba, USER_RID: UserID);
			
				if (dt.Rows.Count == 0)
				{
					Initialize();
				}
				else
				{
					ViewType = (eAllocationSelectionViewType)Convert.ToInt32(dt.Rows[0]["VIEW_TYPE"], CultureInfo.CurrentUICulture);
					StoreAttributeID = DBNullToMinusOne(dt.Rows[0]["SG_RID"]);
					FilterID = DBNullToMinusOne(dt.Rows[0]["FILTER_RID"]);
					GroupBy = Convert.ToInt32(dt.Rows[0]["GROUP_BY_ID"], CultureInfo.CurrentUICulture );
					ViewRID = DBNullToMinusOne(dt.Rows[0]["VIEW_RID"]);
					IncludeIneligibleStores = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["INCLUDE_INELIGIBLE_STORES"], CultureInfo.CurrentUICulture));
					NeedAnalysisPeriodBeginRID = DBNullToMinusOne(dt.Rows[0]["BEGIN_CDR_RID"]);
					NeedAnalysisPeriodEndRID = DBNullToMinusOne(dt.Rows[0]["END_CDR_RID"]);
					NeedAnalysisHNID = DBNullToMinusOne(dt.Rows[0]["HN_RID"]);
				}	
			}
			catch (FileNotFoundException ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		private void ReadDefaultHeaderIDsFromDatabase()
		{
			int i;
			try
			{
                //TT#2354 - removed nolock because it causes concurrency issues
                DataTable dt = StoredProcedures.MID_USER_ALLOCATION_HEADERS_READ.Read(_dba, USER_RID: UserID); //5.4 Update

                //_allocationHeaderRIDs = new int[dt.Rows.Count];  // TT#916 - MD - Jellis - Invalid attempt to instantiate an assortment
                _allocationHeaderRIDsTypes = new HeaderRID_Type[dt.Rows.Count]; // TT#916 - MD - Jellis - Invalid attemp to instantiat an assortment
			
				
				if (dt.Rows.Count == 0)
				{
				}
				else
				{
					for (i=0;i<dt.Rows.Count;i++)
					{
                        // begin TT#916 - MD - Jellis - Invalid attempt to instantiate an Assortment
                        //_allocationHeaderRIDs[i] = Convert.ToInt32(dt.Rows[i]["HDR_RID"], CultureInfo.CurrentUICulture );                      
                        _allocationHeaderRIDsTypes[i]
                            = new HeaderRID_Type
                                (
                                Convert.ToInt32(dt.Rows[i]["HDR_RID"], CultureInfo.CurrentUICulture),
                                Convert.ToBoolean(dt.Rows[i]["Assortment"], CultureInfo.CurrentUICulture)
                                );
                        // end TT#916 - MD- Jellis - Invalid attempt to instantiate an Assortment
					}	
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        private DataTable ReadUserAllocationBasis()
        {
            try
            {
                //Begin TT#1273-MD -jsobek -Allocation>Review>Select window not holding basis information.
                //DataTable dtBasis = MIDEnvironment.CreateDataTable("UserAllocBasis");

                //dtBasis.Columns.Add("BasisSequence", System.Type.GetType("System.Int32"));
                //dtBasis.Columns.Add("BasisHNRID", System.Type.GetType("System.Int32"));
                //dtBasis.Columns.Add("BasisPHRID", System.Type.GetType("System.Int32"));
                //dtBasis.Columns.Add("BasisPHLSequence", System.Type.GetType("System.Int32"));
                //dtBasis.Columns.Add("BasisFVRID", System.Type.GetType("System.Int32"));
                //dtBasis.Columns.Add("CdrRID", System.Type.GetType("System.Int32"));
                //dtBasis.Columns.Add("Weight", System.Type.GetType("System.Double"));

                DataTable dtBasis = StoredProcedures.MID_USER_ALLOCATION_BASIS_READ.Read(_dba, USER_RID: UserID);
                //End TT#1273-MD -jsobek -Allocation>Review>Select window not holding basis information.
                
                dtBasis.TableName = "UserAllocBasis";

                dtBasis.Columns[0].ColumnName = "BasisSequence";
                dtBasis.Columns[1].ColumnName = "BasisHNRID";
                dtBasis.Columns[2].ColumnName = "BasisPHRID";
                dtBasis.Columns[3].ColumnName = "BasisPHLSequence";
                dtBasis.Columns[4].ColumnName = "BasisFVRID";
                dtBasis.Columns[5].ColumnName = "CdrRID";
                dtBasis.Columns[6].ColumnName = "Weight";

                dtBasis.AcceptChanges();

                return dtBasis;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
        // End TT#638  

		private int DBNullToMinusOne(object aDc)
		{
			if (Convert.IsDBNull(aDc))
			{
				return -1;
			}
			else
			{
				return Convert.ToInt32(aDc, CultureInfo.CurrentUICulture);
			}
		}

        //Begin TT#1310-MD -jsobek -Error when adding a new Store -unused function
        //private string MinusOneToNull(int aInt)
        //{
        //    if (aInt == -1)
        //    {
        //        return "null";
        //    }
        //    else
        //    {
        //        return aInt.ToString("0", CultureInfo.CurrentUICulture);
        //    }
        //}
        //End TT#1310-MD -jsobek -Error when adding a new Store -unused function

        private int? MinusOneToNullable(int aInt)
        {
            if (aInt == -1)
            {
                return null;
            }
            else
            {
                return aInt;
            }
        }

		public void SaveDefaultsToDatabase()
		{
			//StringBuilder SQL;
			eAllocationSelectionViewType vwType;
			int grpBy;
			try
			{	// Do not update database row with Velocity - get existing values
				vwType = ViewType;
				grpBy = GroupBy;
				if (ViewType == eAllocationSelectionViewType.Velocity)
				{
					DataTable dt = GetCurrentColumns();
					if (dt != null && dt.Rows.Count > 0)
					{
						vwType = (eAllocationSelectionViewType)Convert.ToInt32(dt.Rows[0]["VIEW_TYPE"], CultureInfo.CurrentUICulture);
						grpBy = Convert.ToInt32(dt.Rows[0]["GROUP_BY_ID"], CultureInfo.CurrentUICulture );
					}
				}
				_dba.OpenUpdateConnection();
				
				if (_analysisOnly)
				{
                    StoredProcedures.MID_USER_ALLOCATION_UPDATE.Update(_dba, 
                                                                       USER_RID: UserID,
                                                                       VIEW_TYPE: (int)vwType,
                                                                       SG_RID: MinusOneToNullable(StoreAttributeID),
                                                                       FILTER_RID: MinusOneToNullable(FilterID),
                                                                       GROUP_BY_ID: MinusOneToNullable(grpBy),
                                                                       VIEW_RID: MinusOneToNullable(ViewRID),
                                                                       INCLUDE_INELIGIBLE_STORES: Include.ConvertBoolToChar(IncludeIneligibleStores),
                                                                       BEGIN_CDR_RID: MinusOneToNullable(NeedAnalysisPeriodBeginRID),
                                                                       END_CDR_RID: MinusOneToNullable(NeedAnalysisPeriodEndRID),
                                                                       HN_RID: MinusOneToNullable(NeedAnalysisHNID)
                                                                       );
				}
				else
				{
                    StoredProcedures.MID_USER_ALLOCATION_DELETE_AND_INSERT.Insert(_dba, 
                                                                                  USER_RID: UserID,
                                                                                  VIEW_TYPE: (int)vwType,
                                                                                  SG_RID: MinusOneToNullable(StoreAttributeID),
                                                                                  FILTER_RID: MinusOneToNullable(FilterID),
                                                                                  GROUP_BY_ID: MinusOneToNullable(grpBy),
                                                                                  VIEW_RID: MinusOneToNullable(ViewRID),
                                                                                  INCLUDE_INELIGIBLE_STORES: Include.ConvertBoolToChar(IncludeIneligibleStores),
                                                                                  BEGIN_CDR_RID: MinusOneToNullable(NeedAnalysisPeriodBeginRID),
                                                                                  END_CDR_RID: MinusOneToNullable(NeedAnalysisPeriodEndRID),
                                                                                  HN_RID: MinusOneToNullable(NeedAnalysisHNID)
                                                                                  );

                    //Begin TT#916 - Jellis - Invalid attempt to instantiate assortment
                    //foreach (int RID in _allocationHeaderRIDs)
                    //{
                    //    StoredProcedures.MID_USER_ALLOCATION_HEADERS_INSERT.Insert(_dba, 
                    //                                                               USER_RID: UserID,
                    //                                                               HDR_RID: RID
                    //                                                              );
                    //}
                    foreach (HeaderRID_Type headerRID_Type in _allocationHeaderRIDsTypes) 
                    {   
                        StoredProcedures.MID_USER_ALLOCATION_HEADERS_INSERT.Insert(_dba,
                                                                                   USER_RID: UserID,
                                                                                   HDR_RID: headerRID_Type.HeaderRID
                                                                                  );
                    }
                    //End TT#916 - Jellis - Invalid attempt to instantiate assortment

                    // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                    DataView dv = new DataView();
                    dv.Table = DTUserAllocBasis;
                    for (int i = 0; i < dv.Count; i++)
                    {
                        int basis_PH_RID = (int)dv[i]["BasisPHRID"];
                        int basis_PHL_Sequence = (int)dv[i]["BasisPHLSequence"];
                        int? basis_PH_RID_Nullable = null;
                        if (basis_PH_RID != Include.NoRID) basis_PH_RID_Nullable = basis_PH_RID;
                        int? basis_PHL_Sequence_Nullable = null;
                        if (basis_PH_RID != Include.NoRID) basis_PHL_Sequence_Nullable = basis_PHL_Sequence;

                        StoredProcedures.MID_USER_ALLOCATION_BASIS_INSERT.Insert(_dba, 
                                                                                 USER_RID: UserID,
                                                                                 BASIS_SEQUENCE: (int)dv[i]["BasisSequence"],
                                                                                 BASIS_HN_RID: MinusOneToNullable((int)dv[i]["BasisHNRID"]),
                                                                                 BASIS_PH_RID: basis_PH_RID_Nullable,
                                                                                 BASIS_PHL_SEQUENCE: basis_PHL_Sequence_Nullable,
                                                                                 BASIS_FV_RID: MinusOneToNullable((int)dv[i]["BasisFVRID"]),
                                                                                 CDR_RID: (int)dv[i]["cdrRID"],
                                                                                 SALES_WEIGHT: (double)dv[i]["Weight"]
                                                                                 );
                    }
                    // End TT#638  
				}
				_dba.CommitData();
				_dba.CloseUpdateConnection();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				_dba.RollBack();
				_dba.CloseUpdateConnection();
				throw;
			}
		}

        

		private DataTable GetCurrentColumns()
		{
			try
			{
                //TT#2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_USER_ALLOCATION_READ_CURRENT_COLUMNS.Read(_dba, USER_RID: UserID);
			}
			catch (FileNotFoundException ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		public DataTable GetDistinctNodeUsersSelection(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_USER_ALLOCATION_READ_DISTINCT_USERS_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


	}
}
