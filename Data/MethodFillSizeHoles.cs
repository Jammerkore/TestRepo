using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Insert, Update, Delete, Select properties for table METHOD_FILL_SIZE_HOLES
	/// </summary>
	public class MethodFillSizeHolesData : SizeMethodBaseData
	{

		#region "Member Variables"
		private bool   _PercentInd;
		private double _Available;
		private int _SizeGroupRid;
		private int _SizeCurveGroupRid;
		private int _MerchHnRid;
		private int _MerchPhRid;
		private int _MerchPhlSequence;
		private int _StoreFilterRid;
		//private int _SgRid;
		private eMerchandiseType _MerchandiseType;
		private int _sizeAlternateRid;
		private int _sizeConstraintRid;
		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		private bool _fillSizesToTypeIsSet;
        //private bool _overrideVSWSizeConstraints; // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options  // TT#488 - MD - Jellis - Group Allocation - Field and property that accesses it exists in base object
		private eFillSizesToType _fillSizesToType;
		// End MID Track #4921
        // BEGIN TT#41-MD - GTaylor - UC#2
        private eMerchandiseType _IB_MerchandiseType;
        private int _IB_MERCH_HN_RID;
        private int _IB_MERCH_PH_RID;
        private int _IB_MERCH_PHL_SEQUENCE;
        // END TT#41-MD - GTaylor - UC#2

        //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
        private double _AvgPackDeviationTolerance;
        private double _MaxPackNeedTolerance;
        private bool _overrideAvgPackDevTolerance;  
        private bool _overrideMaxPackNeedTolerance; 
        private bool _packToleranceNoMaxStep;
        private bool _packToleranceStepped;
        //End TT#1636-MD -jsobek -Pre-Pack Fill Size

		#endregion

		#region "Public Properties"
        // BEGIN TT#41-MD - GTaylor - UC#2
        public int IB_MERCH_HN_RID
        {
            get { return _IB_MERCH_HN_RID; }
            set { _IB_MERCH_HN_RID = value; }
        }
        public int IB_MERCH_PH_RID
        {
            get { return _IB_MERCH_PH_RID; }
            set { _IB_MERCH_PH_RID = value; }
        }
        public int IB_MERCH_PHL_SEQ
        { 
            get { return _IB_MERCH_PHL_SEQUENCE; }
            set  { _IB_MERCH_PHL_SEQUENCE = value; }
        }
        public eMerchandiseType IB_MerchandiseType
        {
            get { return _IB_MerchandiseType; }
            set { _IB_MerchandiseType = value; }
        }
        // END TT#41-MD - GTaylor - UC#2

        // TT#488 - MD - Jellis - Group Allocation -- property exists in base object
        //// BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        //public bool OverrideVSWSizeConstraints
        //{
        //    get { return _overrideVSWSizeConstraints; }
        //    set { _overrideVSWSizeConstraints = value; }
        //}
        //// END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
        // TT#488 - MD - Jellis - Group Allocation - property exists in base object

		public double Available
		{
			get
			{
				return _Available;
			}
			set
			{
				_Available = value;
			}
		}

		public bool PercentInd
		{
			get
			{
				return _PercentInd;
			}
			set
			{
				_PercentInd = value;
			}
		}


		public int SizeGroupRid
		{
			get
			{
				return _SizeGroupRid;
			}
			set
			{
				_SizeGroupRid = value;
			}
		}


		public int SizeCurveGroupRid
		{
			get
			{
				return _SizeCurveGroupRid;
			}
			set
			{
				_SizeCurveGroupRid = value;
			}
		}


		public int MerchHnRid
		{
			get
			{
				return _MerchHnRid;
			}
			set
			{
				_MerchHnRid = value;
			}
		}


		public int MerchPhRid
		{
			get
			{
				return _MerchPhRid;
			}
			set
			{
				_MerchPhRid = value;
			}
		}


		public int MerchPhlSequence
		{
			get
			{
				return _MerchPhlSequence;
			}
			set
			{
				_MerchPhlSequence = value;
			}
		}


		public eMerchandiseType MerchandiseType
		{
			get	
			{
				return _MerchandiseType;
			}
			set
			{
				_MerchandiseType = value;
			}
		}

		public int StoreFilterRid
		{
			get
			{
				return _StoreFilterRid;
			}
			set
			{
				_StoreFilterRid = value;
			}
		}

		/// <summary>
		/// Gets or sets the Size Alternate RID.
		/// </summary>
		public int SizeAlternateRid
		{
			get {return _sizeAlternateRid;}
			set {_sizeAlternateRid = value;}
		}

		/// <summary>
		/// Gets or sets the Size Constraint RID.
		/// </summary>
		public int SizeConstraintRid
		{
			get {return _sizeConstraintRid;}
			set {_sizeConstraintRid = value;}
		}

		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		public bool FillSizesToTypeIsSet
		{
			get	{ return _fillSizesToTypeIsSet;}
			set	{ _fillSizesToTypeIsSet = value;}
		}
		public eFillSizesToType FillSizesToType
		{
			get	{ return _fillSizesToType;}
			set	{ _fillSizesToType = value;	}
		}
		// End MID Track #4921

        //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
        public bool OverrideAvgPackDevTolerance
        {
            get { return _overrideAvgPackDevTolerance; }
            set { _overrideAvgPackDevTolerance = value; }
        }

        public bool OverrideMaxPackNeedTolerance
        {
            get { return _overrideMaxPackNeedTolerance; }
            set { _overrideMaxPackNeedTolerance = value; }
        }
        /// <summary>
        /// Gets or sets the Maximum Pack Allocation Need Tolerance.
        /// </summary>
        public double MaxPackNeedTolerance
        {
            get { return _MaxPackNeedTolerance; }
            set { _MaxPackNeedTolerance = value; }
        }

        /// <summary>
        /// Gets or sets the Average Pack Deviation Tolerance.
        /// </summary>
        public double AvgPackDeviationTolerance
        {
            get { return _AvgPackDeviationTolerance; }
            set { _AvgPackDeviationTolerance = value; }
        }
        /// <summary>
        /// Gets or sets Pack Tolerance No-Max Step indicator (True: Add final step where tolerance is set to "no max"; False: do not add a final step)
        /// </summary>
        public bool PackToleranceNoMaxStep
        {
            get { return _packToleranceNoMaxStep; }
            set { _packToleranceNoMaxStep = value; }
        }
        /// <summary>
        /// Gets or sets Pack Tolerance Stepped indicator (True: tolerances are processed from 0 upto the MaxPackNeedTolerance, one pass per tolerance level; False: tolerances are processed in one pass) 
        /// </summary>
        public bool PackToleranceStepped
        {
            get { return _packToleranceStepped; }
            set { _packToleranceStepped = value; }
        }
        //End TT#1636-MD -jsobek -Pre-Pack Fill Size
 
		#endregion

		#region "Constructors"

		/// <summary>
		/// Creates an instance of the MethodFillSizeHolesData class.
		/// </summary>
		public MethodFillSizeHolesData() : base(eMethodType.FillSizeHolesAllocation)
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/// <summary>
		/// Creates an instance of the MethodFillSizeHolesData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		public MethodFillSizeHolesData(TransactionData td) : base(eMethodType.FillSizeHolesAllocation)
		{
			_dba = td.DBA;
		}


		/// <summary>
		/// Creates an instance of the MethodFillSizeHolesData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="method_RID">The record ID of the method</param>
		public MethodFillSizeHolesData(TransactionData td, int method_RID) : base(eMethodType.FillSizeHolesAllocation)
		{
			_dba = td.DBA;
		}

		/// <summary>
		/// Creates an instance of the MethodFillSizeHolesData class.
		/// </summary>
		/// <param name="method_RID">ID of the method to retrieve</param>
		/// <param name="changeType"></param>
		public MethodFillSizeHolesData(int method_RID, eChangeType changeType) : base(eMethodType.FillSizeHolesAllocation)
		{
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateFillSizeHoles(method_RID);
					break;
			}
		}

		#endregion

		#region "Methods"

			/// <summary>
			/// Initializes the member variables of the class MethodFillSizeHolesData.
			/// </summary>
			/// <param name="method_RID"></param>
			/// <returns></returns>
			public bool PopulateFillSizeHoles(int method_RID)
			{
				try
				{
					if (PopulateMethod(method_RID))
					{
                        

						DataTable dtFillSizeHoles = MIDEnvironment.CreateDataTable();
                        dtFillSizeHoles = StoredProcedures.MID_METHOD_FILL_SIZE_HOLES_READ.Read(_dba, METHOD_RID: method_RID);
						
						if(dtFillSizeHoles.Rows.Count != 0)
						{
							DataRow dr = dtFillSizeHoles.Rows[0];

							_MerchandiseType = (eMerchandiseType)(Convert.ToInt32(dr["MERCH_TYPE"].ToString(), CultureInfo.CurrentUICulture));

							if (dr["AVAILABLE_TYPE_IND"] != System.DBNull.Value)
							{
								_PercentInd = Convert.ToBoolean(Convert.ToInt32(dr["AVAILABLE_TYPE_IND"].ToString(),CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture);
							}

							if (dr["AVAILABLE"] != System.DBNull.Value)
							{
								_Available = Convert.ToDouble(dr["AVAILABLE"].ToString(),CultureInfo.CurrentUICulture);
							}

							_SizeGroupRid = Include.NoRID;
							if (dr["SIZE_GROUP_RID"] != System.DBNull.Value)
							{
								_SizeGroupRid = Convert.ToInt32(dr["SIZE_GROUP_RID"].ToString(),CultureInfo.CurrentUICulture);
							}

							_SizeCurveGroupRid = Include.NoRID;
							if (dr["SIZE_CURVE_GROUP_RID"] != System.DBNull.Value)
							{
								_SizeCurveGroupRid = Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"],CultureInfo.CurrentUICulture);
							}

							if (dr["MERCH_HN_RID"] != System.DBNull.Value)
							{
								_MerchHnRid = Convert.ToInt32(dr["MERCH_HN_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								_MerchHnRid = Include.NoRID;
							}

							if (dr["MERCH_PH_RID"] != System.DBNull.Value)
							{
								_MerchPhRid = Convert.ToInt32(dr["MERCH_PH_RID"], CultureInfo.CurrentUICulture);
								_MerchPhlSequence = Convert.ToInt32(dr["MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
							}
							else
							{
								_MerchPhRid = Include.NoRID;
								_MerchPhlSequence = 0;		
							}


							if (dr["STORE_FILTER_RID"] != System.DBNull.Value)
							{
								_StoreFilterRid = Convert.ToInt32(dr["STORE_FILTER_RID"],CultureInfo.CurrentUICulture);
							}
							else
							{
								_StoreFilterRid = Include.NoRID;
							}

							if (dr["SIZE_ALTERNATE_RID"] != System.DBNull.Value)
							{
								this.SizeAlternateRid = Convert.ToInt32(dr["SIZE_ALTERNATE_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.SizeAlternateRid = Include.NoRID;
							}

							if (dr["SIZE_CONSTRAINT_RID"] != System.DBNull.Value)
							{
								this.SizeConstraintRid = Convert.ToInt32(dr["SIZE_CONSTRAINT_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.SizeConstraintRid = Include.NoRID;
							}
							
							// begin Generic Size Curve data
							if (dr["GENCURVE_HCG_RID"] != System.DBNull.Value)
							{
								this.GenCurveHcgRID = Convert.ToInt32(dr["GENCURVE_HCG_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenCurveHcgRID = Include.NoRID;
							}

							if (dr["GENCURVE_HN_RID"] != System.DBNull.Value)
							{
								this.GenCurveHnRID = Convert.ToInt32(dr["GENCURVE_HN_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenCurveHnRID = Include.NoRID;
							}

							if (dr["GENCURVE_PH_RID"] != System.DBNull.Value)
							{
								this.GenCurvePhRID = Convert.ToInt32(dr["GENCURVE_PH_RID"], CultureInfo.CurrentUICulture);
								this.GenCurvePhlSequence= Convert.ToInt32(dr["GENCURVE_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenCurvePhRID = Include.NoRID;
								this.GenCurvePhlSequence = 0;		
							}
						
							this.GenCurveColorInd = Include.ConvertCharToBool(Convert.ToChar(dr["GENCURVE_COLOR_IND"], CultureInfo.CurrentUICulture));
							
							if (dr["GENCURVE_MERCH_TYPE"] != System.DBNull.Value)
							{
								this.GenCurveMerchType = (eMerchandiseType)Convert.ToInt32(dr["GENCURVE_MERCH_TYPE"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenCurveMerchType = eMerchandiseType.Undefined;
							}
							// end Generic Size Curve data
							
							// begin Generic Size Constraint data
							if (dr["GENCONSTRAINT_HCG_RID"] != System.DBNull.Value)
							{
								this.GenConstraintHcgRID = Convert.ToInt32(dr["GENCONSTRAINT_HCG_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenConstraintHcgRID = Include.NoRID;
							}

							if (dr["GENCONSTRAINT_HN_RID"] != System.DBNull.Value)
							{
								this.GenConstraintHnRID = Convert.ToInt32(dr["GENCONSTRAINT_HN_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenConstraintHnRID = Include.NoRID;
							}

							if (dr["GENCONSTRAINT_PH_RID"] != System.DBNull.Value)
							{
								this.GenConstraintPhRID = Convert.ToInt32(dr["GENCONSTRAINT_PH_RID"], CultureInfo.CurrentUICulture);
								this.GenConstraintPhlSequence= Convert.ToInt32(dr["GENCONSTRAINT_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenConstraintPhRID = Include.NoRID;
								this.GenConstraintPhlSequence = 0;		
							}
						
							this.GenConstraintColorInd = Include.ConvertCharToBool(Convert.ToChar(dr["GENCONSTRAINT_COLOR_IND"], CultureInfo.CurrentUICulture));
							
							if (dr["GENCONSTRAINT_MERCH_TYPE"] != System.DBNull.Value)
							{
								this.GenConstraintMerchType = (eMerchandiseType)Convert.ToInt32(dr["GENCONSTRAINT_MERCH_TYPE"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this.GenConstraintMerchType = eMerchandiseType.Undefined;
							}

							// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
							if (dr["NORMALIZE_SIZE_CURVES_IND"] != System.DBNull.Value)
							{
								NormalizeSizeCurvesDefaultIsOverridden = true;
								NormalizeSizeCurves = Include.ConvertCharToBool(Convert.ToChar(dr["NORMALIZE_SIZE_CURVES_IND"], CultureInfo.CurrentUICulture));
							}
							else
							{
								NormalizeSizeCurvesDefaultIsOverridden = false;
							}
							// END MID Track #4826

							// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
							if (dr["FILL_SIZES_TO_TYPE"] != System.DBNull.Value)
							{
								_fillSizesToTypeIsSet = true;
								_fillSizesToType = (eFillSizesToType)(Convert.ToInt32(dr["FILL_SIZES_TO_TYPE"], CultureInfo.CurrentUICulture));
							}
							else
							{
								_fillSizesToTypeIsSet = false;
							}
							// END MID Track #4921
							// end Generic Size Constraint data

                            // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                            if (dr["USE_DEFAULT_CURVE_IND"] != System.DBNull.Value)
                            {
                                UseDefaultCurve = Include.ConvertCharToBool(Convert.ToChar(dr["USE_DEFAULT_CURVE_IND"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                UseDefaultCurve = false;
                            }
                            // End TT#413

                            // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                            if (dr["APPLY_RULES_ONLY_IND"] != System.DBNull.Value)
                            {
                                ApplyRulesOnly = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_RULES_ONLY_IND"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                ApplyRulesOnly = false;
                            }
                            // end TT#2155 - JEllis - Fill Size Holes Null Reference

                            // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                            if (dr["GENCURVE_NSCCD_RID"] != System.DBNull.Value)
                            {
                                this.GenCurveNsccdRID = Convert.ToInt32(dr["GENCURVE_NSCCD_RID"], CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                this.GenCurveNsccdRID = Include.NoRID;
                            }
                            // End TT#413
                            // BEGIN TT#41-MD - GTaylor - UC#2
                            if (dr["IB_MERCH_TYPE"] != System.DBNull.Value)
                               _IB_MerchandiseType = (eMerchandiseType)(Convert.ToInt32(dr["IB_MERCH_TYPE"].ToString(), CultureInfo.CurrentUICulture));
                            // Begin TT#249-MD - JSmith - Object reference error in Fill Size Holes Method
                            else
                            {
                                _IB_MerchandiseType = eMerchandiseType.Undefined;
                            }
                            // End TT#249-MD - JSmith - Object reference error in Fill Size Holes Method
                            if (dr["IB_MERCH_HN_RID"] != System.DBNull.Value)
                                _IB_MERCH_HN_RID = Convert.ToInt32(dr["IB_MERCH_HN_RID"], CultureInfo.CurrentUICulture);
                            else
                                _IB_MERCH_HN_RID = Include.NoRID;
                            if (dr["IB_MERCH_PH_RID"] != System.DBNull.Value)
                            {
                                _IB_MERCH_PH_RID = Convert.ToInt32(dr["IB_MERCH_PH_RID"], CultureInfo.CurrentUICulture);
                                _IB_MERCH_PHL_SEQUENCE = Convert.ToInt32(dr["IB_MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                _IB_MERCH_PH_RID = Include.NoRID;
                                _IB_MERCH_PHL_SEQUENCE = 0;
                            }
                            // END TT#41-MD - GTaylor - UC#2

                            // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                            if (dr["VSW_SIZE_CONSTRAINTS_IND"] != System.DBNull.Value)
                            {
                                OverrideVSWSizeConstraints = Include.ConvertCharToBool(Convert.ToChar(dr["VSW_SIZE_CONSTRAINTS_IND"], CultureInfo.CurrentUICulture));
                                VSWSizeConstraints = (eVSWSizeConstraints)(Convert.ToInt32(dr["VSW_SIZE_CONSTRAINTS"]));
                            }
                            else
                            {
                                OverrideVSWSizeConstraints = false;
                            }
                            // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                            //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                            if (dr["AVG_PACK_DEV_TOLERANCE"] != System.DBNull.Value)
                            {
                                this._AvgPackDeviationTolerance = Convert.ToDouble(dr["AVG_PACK_DEV_TOLERANCE"].ToString(), CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                this._AvgPackDeviationTolerance = Include.DefaultMaxSizeErrorPercent;
                            }

                            if (dr["MAX_PACK_NEED_TOLERANCE"] != System.DBNull.Value)
                            {
                                this._MaxPackNeedTolerance = Convert.ToDouble(dr["MAX_PACK_NEED_TOLERANCE"].ToString(), CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                this._MaxPackNeedTolerance = Include.DefaultPackSizeErrorPercent;
                            }
                            if (dr["OVERRIDE_AVG_PACK_DEV_IND"] != System.DBNull.Value)
                            {
                                OverrideAvgPackDevTolerance = Include.ConvertCharToBool(Convert.ToChar(dr["OVERRIDE_AVG_PACK_DEV_IND"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                OverrideAvgPackDevTolerance = false;
                            }

                            if (dr["OVERRIDE_MAX_PACK_NEED_IND"] != System.DBNull.Value)
                            {
                                OverrideMaxPackNeedTolerance = Include.ConvertCharToBool(Convert.ToChar(dr["OVERRIDE_MAX_PACK_NEED_IND"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                OverrideMaxPackNeedTolerance = false;
                            }

                            if (dr["PACK_TOLERANCE_NO_MAX_STEP_IND"] != System.DBNull.Value)
                            {
                                _packToleranceNoMaxStep = Include.ConvertCharToBool(Convert.ToChar(dr["PACK_TOLERANCE_NO_MAX_STEP_IND"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                _packToleranceNoMaxStep = false;
                            }

                            if (dr["PACK_TOLERANCE_STEPPED_IND"] != System.DBNull.Value)
                            {
                                _packToleranceStepped = Include.ConvertCharToBool(Convert.ToChar(dr["PACK_TOLERANCE_STEPPED_IND"], CultureInfo.CurrentUICulture));
                            }
                            else
                            {
                                _packToleranceStepped = false;
                            }
                            
                            //End TT#1636-MD -jsobek -Pre-Pack Fill Size

							return true;
						}
						else
							return false;
					}
					else
						return false;
				}
				catch
				{
					throw;
				}
			}



			/// <summary>
			/// Does the actual work of the insert or update.  Called from InsertMethod or UpdateMethod.
			/// </summary>
			/// <param name="method_RID"></param>
			/// <param name="td"></param>
			private void PerformInsertUpdate(int method_RID, TransactionData td)
			{
				
				

                int? SIZEGROUPRID_Nullable = null;
                if (_SizeGroupRid != Include.NoRID) SIZEGROUPRID_Nullable = _SizeGroupRid;

                int? SIZECURVEGROUPRID_Nullable = null;
                if (_SizeCurveGroupRid != Include.NoRID) SIZECURVEGROUPRID_Nullable = _SizeCurveGroupRid;

                int? MERCHHNRID_Nullable = null;
                if (_MerchHnRid != Include.NoRID) MERCHHNRID_Nullable = _MerchHnRid;

                int? MERCHPHRID_Nullable = null;
                if (_MerchPhRid != Include.NoRID) MERCHPHRID_Nullable = _MerchPhRid;

                int? MERCHPHLSEQ_Nullable = null;
                if (_MerchPhRid != Include.NoRID) MERCHPHLSEQ_Nullable = _MerchPhlSequence;

                int? STOREFILTERRID_Nullable = null;
                if (_StoreFilterRid != Include.NoRID) STOREFILTERRID_Nullable = _StoreFilterRid;

                int? SIZEALTERNATERID_Nullable = null;
                if (_sizeAlternateRid != Include.NoRID) SIZEALTERNATERID_Nullable = _sizeAlternateRid;

                int? SIZECONSTRAINTRID_Nullable = null;
                if (_sizeConstraintRid != Include.NoRID) SIZECONSTRAINTRID_Nullable = _sizeConstraintRid;

                int? GENCURVE_HCG_RID_Nullable = null;
                if (GenCurveHcgRID != Include.NoRID) GENCURVE_HCG_RID_Nullable = GenCurveHcgRID;

                int? GENCURVE_HN_RID_Nullable = null;
                if (GenCurveHnRID != Include.NoRID) GENCURVE_HN_RID_Nullable = GenCurveHnRID;

                int? GENCURVE_PH_RID_Nullable = null;
                if (GenCurvePhRID != Include.NoRID) GENCURVE_PH_RID_Nullable = GenCurvePhRID;

                int? GENCURVE_PHL_SEQUENCE_Nullable = null;
                if (GenCurvePhRID != Include.NoRID) GENCURVE_PHL_SEQUENCE_Nullable = GenCurvePhlSequence;

                char? GENCURVE_COLOR_IND_Nullable = null;
                if (this.GenCurveColorInd) GENCURVE_COLOR_IND_Nullable = Include.ConvertBoolToChar(this.GenCurveColorInd);

                int? GENCURVE_MERCH_TYPE_Nullable = null;
                if (this.GenCurveMerchType != eMerchandiseType.Undefined) GENCURVE_MERCH_TYPE_Nullable = (int)this.GenCurveMerchType;

                int? GENCONSTRAINT_HCG_RID_Nullable = null;
                if (this.GenConstraintHcgRID != Include.NoRID) GENCONSTRAINT_HCG_RID_Nullable = this.GenConstraintHcgRID;

                int? GENCONSTRAINT_HN_RID_Nullable = null;
                if (this.GenConstraintHnRID != Include.NoRID) GENCONSTRAINT_HN_RID_Nullable = this.GenConstraintHnRID; //TT#1301-MD -jsobek -5.4 Database Foreign Key Violation

                int? GENCONSTRAINT_PH_RID_Nullable = null;
                if (GenConstraintPhRID != Include.NoRID) GENCONSTRAINT_PH_RID_Nullable = this.GenConstraintPhRID;

                int? GENCONSTRAINT_PHL_SEQUENCE_Nullable = null;
                if (GenConstraintPhRID != Include.NoRID) GENCONSTRAINT_PHL_SEQUENCE_Nullable = this.GenConstraintPhlSequence;

                char? GENCONSTRAINT_COLOR_IND_Nullable = null;
                if (this.GenConstraintColorInd) GENCONSTRAINT_COLOR_IND_Nullable = Include.ConvertBoolToChar(this.GenConstraintColorInd);

                int? GENCONSTRAINT_MERCH_TYPE_Nullable = null;
                if (this.GenConstraintMerchType != eMerchandiseType.Undefined) GENCONSTRAINT_MERCH_TYPE_Nullable = (int)this.GenConstraintMerchType;

                char? NORMALIZE_SIZE_CURVES_IND_Nullable = null;
                if (NormalizeSizeCurvesDefaultIsOverridden) NORMALIZE_SIZE_CURVES_IND_Nullable = Include.ConvertBoolToChar(NormalizeSizeCurves);

                int? GENCURVE_NSCCD_RID_Nullable = null;
                if (this.GenCurveNsccdRID != Include.NoRID) GENCURVE_NSCCD_RID_Nullable = this.GenCurveNsccdRID;

                int? IB_MERCH_HN_RID_Nullable = null;
                if (_IB_MERCH_HN_RID != Include.NoRID) IB_MERCH_HN_RID_Nullable = _IB_MERCH_HN_RID;

                int? IB_MERCH_PH_RID_Nullable = null;
                if (_IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PH_RID_Nullable = _IB_MERCH_PH_RID;

                int? IB_MERCH_PHL_SEQUENCE_Nullable = null;
                if (_IB_MERCH_PH_RID != Include.NoRID) IB_MERCH_PHL_SEQUENCE_Nullable = _IB_MERCH_PHL_SEQUENCE;

                char? VSW_SIZE_CONSTRAINTS_IND_Nullable = null;
                if (this.OverrideVSWSizeConstraints) VSW_SIZE_CONSTRAINTS_IND_Nullable = Include.ConvertBoolToChar(OverrideVSWSizeConstraints);

                int? VSW_SIZE_CONSTRAINTS_Nullable = null;
                if (this.OverrideVSWSizeConstraints) VSW_SIZE_CONSTRAINTS_Nullable = (int)this.VSWSizeConstraints;

                //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                double? AVGPACKDEVIATION_Nullable = null;
                if (OverrideAvgPackDevTolerance && this._AvgPackDeviationTolerance != double.MaxValue) AVGPACKDEVIATION_Nullable = this._AvgPackDeviationTolerance;

                double? MAXPACKNEED_Nullable = null;
                if (OverrideMaxPackNeedTolerance && this._MaxPackNeedTolerance != double.MaxValue) MAXPACKNEED_Nullable = this._MaxPackNeedTolerance;


                char? OVERRIDE_AVG_PACK_DEV_IND_Nullable = null;
                if (OverrideAvgPackDevTolerance) OVERRIDE_AVG_PACK_DEV_IND_Nullable = Include.ConvertBoolToChar(OverrideAvgPackDevTolerance);

                char? OVERRIDE_MAX_PACK_NEED_IND_Nullable = null;
                if (OverrideMaxPackNeedTolerance) OVERRIDE_MAX_PACK_NEED_IND_Nullable = Include.ConvertBoolToChar(OverrideMaxPackNeedTolerance);

                char? PACK_TOLERANCE_NO_MAX_STEP_IND_Nullable = null;
                if (OverrideMaxPackNeedTolerance) PACK_TOLERANCE_NO_MAX_STEP_IND_Nullable = Include.ConvertBoolToChar(_packToleranceNoMaxStep);

                char? PACK_TOLERANCE_STEPPED_IND_Nullable = null;
                if (OverrideMaxPackNeedTolerance) PACK_TOLERANCE_STEPPED_IND_Nullable = Include.ConvertBoolToChar(_packToleranceStepped);
                //End TT#1636-MD -jsobek -Pre-Pack Fill Size

                StoredProcedures.SP_MID_MTH_FSH_UPD_INS.Update(td.DBA,
                                                                METHODRID: method_RID,
                                                                AVAILABLE: _Available,
                                                                AVAILABLEIND: Include.ConvertBoolToChar(_PercentInd),
                                                                SIZEGROUPRID: SIZEGROUPRID_Nullable,
                                                                SIZECURVEGROUPRID: SIZECURVEGROUPRID_Nullable,
                                                                MERCHTYPE: (int)MerchandiseType,
                                                                MERCHHNRID: MERCHHNRID_Nullable,
                                                                MERCHPHRID: MERCHPHRID_Nullable,
                                                                MERCHPHLSEQ: MERCHPHLSEQ_Nullable,
                                                                STOREFILTERRID: STOREFILTERRID_Nullable,
                                                                SIZEALTERNATERID: SIZEALTERNATERID_Nullable,
                                                                SIZECONSTRAINTRID: SIZECONSTRAINTRID_Nullable,
                                                                GENCURVE_HCG_RID: GENCURVE_HCG_RID_Nullable,
                                                                GENCURVE_HN_RID: GENCURVE_HN_RID_Nullable,
                                                                GENCURVE_PH_RID: GENCURVE_PH_RID_Nullable,
                                                                GENCURVE_PHL_SEQUENCE: GENCURVE_PHL_SEQUENCE_Nullable,
                                                                GENCURVE_COLOR_IND: GENCURVE_COLOR_IND_Nullable,
                                                                GENCURVE_MERCH_TYPE: GENCURVE_MERCH_TYPE_Nullable,
                                                                GENCONSTRAINT_HCG_RID: GENCONSTRAINT_HCG_RID_Nullable,
                                                                GENCONSTRAINT_HN_RID: GENCONSTRAINT_HN_RID_Nullable,
                                                                GENCONSTRAINT_PH_RID: GENCONSTRAINT_PH_RID_Nullable,
                                                                GENCONSTRAINT_PHL_SEQUENCE: GENCONSTRAINT_PHL_SEQUENCE_Nullable,
                                                                GENCONSTRAINT_COLOR_IND: GENCONSTRAINT_COLOR_IND_Nullable,
                                                                GENCONSTRAINT_MERCH_TYPE: GENCONSTRAINT_MERCH_TYPE_Nullable,
                                                                NORMALIZE_SIZE_CURVES_IND: NORMALIZE_SIZE_CURVES_IND_Nullable,
                                                                FILL_SIZES_TO_TYPE: (int)FillSizesToType,
                                                                USE_DEFAULT_CURVE_IND: Include.ConvertBoolToChar(UseDefaultCurve),
                                                                GENCURVE_NSCCD_RID: GENCURVE_NSCCD_RID_Nullable,
                                                                APPLY_RULES_ONLY_IND: Include.ConvertBoolToChar(ApplyRulesOnly),
                                                                IB_MERCH_TYPE: (int)_IB_MerchandiseType,
                                                                IB_MERCH_HN_RID: IB_MERCH_HN_RID_Nullable,
                                                                IB_MERCH_PH_RID: IB_MERCH_PH_RID_Nullable,
                                                                IB_MERCH_PHL_SEQUENCE: IB_MERCH_PHL_SEQUENCE_Nullable,
                                                                VSW_SIZE_CONSTRAINTS_IND: VSW_SIZE_CONSTRAINTS_IND_Nullable,
                                                                VSW_SIZE_CONSTRAINTS: VSW_SIZE_CONSTRAINTS_Nullable,
                                                                AVGPACKDEVIATION: AVGPACKDEVIATION_Nullable, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                                                MAXPACKNEED: MAXPACKNEED_Nullable, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                                                OVERRIDE_AVG_PACK_DEV_IND: OVERRIDE_AVG_PACK_DEV_IND_Nullable, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                                                OVERRIDE_MAX_PACK_NEED_IND: OVERRIDE_MAX_PACK_NEED_IND_Nullable, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                                                PACK_TOLERANCE_NO_MAX_STEP_IND: PACK_TOLERANCE_NO_MAX_STEP_IND_Nullable, //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                                                PACK_TOLERANCE_STEPPED_IND: PACK_TOLERANCE_STEPPED_IND_Nullable //TT#1636-MD -jsobek -Pre-Pack Fill Size
                                                                );

			}


			/// <summary>
			/// Inserts data associated with the method into METHOD_FILL_SIZE_HOLES 
			/// </summary>
			/// <param name="method_RID">ID of the method to insert</param>
			/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
			/// <returns></returns>
			public bool InsertMethod(int method_RID, TransactionData td)
			{
				bool InsertSuccessfull = true;

				try
				{
					PerformInsertUpdate(method_RID, td);
	
					InsertSuccessfull = true;
				}
				catch(Exception e)
				{
					string exceptionMessage = e.Message;
					InsertSuccessfull = false;
					throw;
				}
				return InsertSuccessfull;
			}


			/// <summary>
			/// Updates data associated with the method into METHOD_FILL_SIZE_HOLES 
			/// </summary>
			/// <param name="method_RID">ID of the method to update</param>
			/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
			/// <returns></returns>
			public bool UpdateMethod(int method_RID, TransactionData td)
			{
				bool UpdateSuccessful = true;
				try
				{

					PerformInsertUpdate(method_RID, td);
		
					UpdateSuccessful = true;
				}
				catch
				{
					UpdateSuccessful = false;
					throw;
				}
				finally
				{
				}
				return UpdateSuccessful;
			
			}

			

			/// <summary>
			/// Deletes data and child data associated with the fill size holes method
			/// </summary>
			/// <param name="method_RID">ID of the method to delete</param>
			/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
			/// <returns></returns>
			public bool DeleteMethod(int method_RID, TransactionData td)
			{
				bool Successfull = true;

				try
				{
                    StoredProcedures.SP_MID_MTH_FSH_DEL_MTH.Delete(td.DBA, METHODRID: method_RID);
		
					Successfull = true;
				}
				catch(Exception e)
				{
					string exceptionMessage = e.Message;
					Successfull = false;
					throw;
				}
				return Successfull;

			}


		#endregion

	}
}
