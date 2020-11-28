using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for MethodSizeNeed.
	/// </summary>
	public class MethodSizeNeedData : SizeMethodBaseData
	{
	
		#region "Member Variables"
 			private int _SizeGroupRid;
			private int _SizeCurveGroupRid;
			private int _MerchHnRid;
			private int _MerchPhRid;
			private int _MerchPhlSequence;
			//private bool _SizeFringeInd;
			//private bool _EquateSizeInd;
			private double _AvgPackDeviationTolerance;
			private double _MaxPackNeedTolerance;
			private eMerchandiseType _MerchType;
			private int _sizeAlternateRid;
			private int _sizeConstraintRid;
 			// private int _sizeFringeRid; // MID Track 3619 Remove Fringe
            private bool _overrideAvgPackDevTolerance;  // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
            private bool _overrideMaxPackNeedTolerance; // End TT#356
            //private bool _overrideVSWSizeConstraints; // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options   // TT#488 - mD - Jellis - Group Allocation - Field and Method exist in base object  
	        // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
	        private bool _packToleranceNoMaxStep;
	        private bool _packToleranceStepped;
	        // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            // BEGIN TT#41-MD - GTaylor - UC#2
            private eMerchandiseType _IB_MerchandiseType;
            private int _IB_MERCH_HN_RID;
            private int _IB_MERCH_PH_RID;
            private int _IB_MERCH_PHL_SEQUENCE;
            // END TT#41-MD - GTaylor - UC#2
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
            set { _IB_MERCH_PHL_SEQUENCE = value; }
        }
        public eMerchandiseType IB_MerchandiseType
        {
            get { return _IB_MerchandiseType; }
            set { _IB_MerchandiseType = value; }
        }
        // END TT#41-MD - GTaylor - UC#2
        /// <summary>
		/// Gets or sets Merchandise Type.
		/// </summary>
		public eMerchandiseType MerchType
		{
			get {return _MerchType;}
			set {_MerchType = value;}
		}

		/// <summary>
		/// Gets or sets Size Group RID.
		/// </summary>
		public int SizeGroupRid
		{
			get{return _SizeGroupRid;}
			set{_SizeGroupRid = value;}
		}

		/// <summary>
		/// Gets or sets Size Curve Group RID.
		/// </summary>
		public int SizeCurveGroupRid
		{
			get{return _SizeCurveGroupRid;}
			set{_SizeCurveGroupRid = value;}
		}

		/// <summary>
		/// Gets or sets the Maximum Pack Allocation Need Tolerance.
		/// </summary>
		public double MaxPackNeedTolerance
		{
			get {return _MaxPackNeedTolerance;}
			set {_MaxPackNeedTolerance = value;}
		}

		/// <summary>
		/// Gets or sets the Average Pack Deviation Tolerance.
		/// </summary>
		public double AvgPackDeviationTolerance
		{
			get {return _AvgPackDeviationTolerance;}
			set {_AvgPackDeviationTolerance = value;}
		}

        // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
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
        // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement

		/// <summary>
		/// Gets or sets the Merchandise Product Hierarchy Level Sequence.
		/// </summary>
		public int MerchPhlSequence
		{
			get {return _MerchPhlSequence;}
			set {_MerchPhlSequence = value;}
		}

		/// <summary>
		/// Gets or sets the Merchandise Hierarchy Node RID.
		/// </summary>
		public int MerchHnRid
		{
			get {return _MerchHnRid;}
			set {_MerchHnRid = value;}
		}

		/// <summary>
		/// Gets or sets the Merchandise Product Hierarchy RID.
		/// </summary>
		public int MerchPhRid
		{
			get {return _MerchPhRid;}
			set {_MerchPhRid = value;}
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

		// begin MID Track 3619 Remove Fringe
		// /// <summary>
		// /// Gets or sets the Size Fringe RID.
		// /// </summary>
		//public int SizeFringeRid
		//{
		//	get {return _sizeFringeRid;}
		//	set {_sizeFringeRid = value;}
		//}
		// end MID Track 3619 Remove Fringe

        // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
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
        // End TT#356 
        // begin TT#488 - MD - Jellis - Group Allocation - property exists in base object
        //// BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        //public bool OverrideVSWSizeConstraints
        //{
        //    get { return _overrideVSWSizeConstraints; }
        //    set { _overrideVSWSizeConstraints = value; }
        //}
        //// END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
        // end TT#488 - MD - Jellis - Group Allocation - property exists in base object
		#endregion

		#region "Constructors"

		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		public MethodSizeNeedData() : base(eMethodType.SizeNeedAllocation)
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		public MethodSizeNeedData(TransactionData td) : base(eMethodType.SizeNeedAllocation)
		{
			_dba = td.DBA;
		}

		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="method_RID">The record ID of the method</param>
		public MethodSizeNeedData(TransactionData td, int method_RID) : base(eMethodType.SizeNeedAllocation)
		{
			_dba = td.DBA;
		}

		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		/// <param name="method_RID">ID of the method to retrieve</param>
		/// <param name="changeType"></param>
		public MethodSizeNeedData(int method_RID, eChangeType changeType) : base(eMethodType.SizeNeedAllocation)
		{
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateSizeNeed(method_RID);
					break;
			}
		}

	#endregion

		#region "Methods"

			/// <summary>
			/// Initializes the member variables of the class MethodBasisSizeData.
			/// </summary>
			/// <param name="method_RID"></param>
			/// <returns></returns>
			public bool PopulateSizeNeed(int method_RID)
			{
				try
				{
					if (PopulateMethod(method_RID))
					{

						DataTable dtSizeNeed = MIDEnvironment.CreateDataTable();

                        dtSizeNeed = StoredProcedures.MID_METHOD_SIZE_NEED_READ.Read(_dba, METHOD_RID: method_RID);

                        //dtSizeNeed = _dba.ExecuteSQLQuery( SQLCommand.ToString(), "MethodSizeNeed" );
							
						if(dtSizeNeed.Rows.Count != 0)
						{
							DataRow dr = dtSizeNeed.Rows[0];

							if (dr["SIZE_GROUP_RID"] != System.DBNull.Value)
							{
								this._SizeGroupRid = Convert.ToInt32(dr["SIZE_GROUP_RID"].ToString(),CultureInfo.CurrentUICulture);
							}
							else
							{
								this._SizeGroupRid = Include.NoRID;
							}

							if (dr["SIZE_CURVE_GROUP_RID"] != System.DBNull.Value)
							{
								this._SizeCurveGroupRid = Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"].ToString(),CultureInfo.CurrentUICulture);
							}
							else
							{
								this._SizeCurveGroupRid = Include.NoRID;
							}

							if (dr["AVG_PACK_DEV_TOLERANCE"] != System.DBNull.Value)
							{
								this._AvgPackDeviationTolerance = Convert.ToDouble(dr["AVG_PACK_DEV_TOLERANCE"].ToString(),CultureInfo.CurrentUICulture);
							}
							else
							{
								this._AvgPackDeviationTolerance = Include.DefaultMaxSizeErrorPercent;
							}

							if (dr["MAX_PACK_NEED_TOLERANCE"] != System.DBNull.Value)
							{
								this._MaxPackNeedTolerance = Convert.ToDouble(dr["MAX_PACK_NEED_TOLERANCE"].ToString(),CultureInfo.CurrentUICulture);
							}
							else
							{
								this._MaxPackNeedTolerance = Include.DefaultPackSizeErrorPercent;
							}

							this._MerchType = (eMerchandiseType)(Convert.ToInt32(dr["MERCH_TYPE"].ToString(), CultureInfo.CurrentUICulture));

							
							if (dr["MERCH_HN_RID"] != System.DBNull.Value)
							{
								this._MerchHnRid = Convert.ToInt32(dr["MERCH_HN_RID"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this._MerchHnRid = Include.NoRID;
							}

							if (dr["MERCH_PH_RID"] != System.DBNull.Value)
							{
								this._MerchPhRid = Convert.ToInt32(dr["MERCH_PH_RID"], CultureInfo.CurrentUICulture);
								this._MerchPhlSequence = Convert.ToInt32(dr["MERCH_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
							}
							else
							{
								this._MerchPhRid = Include.NoRID;
								this._MerchPhlSequence = 0;		
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

							// begin MID Track 3619 Remove Fringe
							//if (dr["SIZE_FRINGE_RID"] != System.DBNull.Value)
							//{
							//	this.SizeFringeRid = Convert.ToInt32(dr["SIZE_FRINGE_RID"], CultureInfo.CurrentUICulture);
							//}
							//else
							//{
							//	this.SizeFringeRid = Include.NoRID;
							//}
							// end MID Track 3619 Remove Fringe
						
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
							// end Generic Size Constraint data

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

                            // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
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
                            // End TT#356 

                            // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                            _packToleranceNoMaxStep = Include.ConvertCharToBool(Convert.ToChar(dr["PACK_TOLERANCE_NO_MAX_STEP_IND"], CultureInfo.CurrentUICulture));
                            _packToleranceStepped = Include.ConvertCharToBool(Convert.ToChar(dr["PACK_TOLERANCE_STEPPED_IND"], CultureInfo.CurrentUICulture));
                            // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement

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
                            // end TT#2155 - Jellis - Fill Size Holes Null Reference

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
                            // Begin TT#248-MD - JSmith - Object reference error in Size Need Method
                            else
                            {
                                _IB_MerchandiseType = eMerchandiseType.Undefined;
                            }
                            // End TT#248-MD - JSmith - Object reference error in Size Need Method
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
			private void PerformUpdateInsert(int method_RID, TransactionData td)
			{

                //Begin TT#1236-MD -jsobek -Size Need Foreign Key Error
                //int SIZEGROUPRID = 0;
                //if (this._SizeGroupRid != Include.NoRID) SIZEGROUPRID = this._SizeGroupRid;

                //int SIZECURVEGROUPRID = 0;
                //if (this._SizeCurveGroupRid != Include.NoRID) SIZECURVEGROUPRID = this._SizeCurveGroupRid;

                //int MERCHHNRID = 0;
                //if (_MerchHnRid != Include.NoRID) MERCHHNRID = _MerchHnRid;

                //int MERCHPHRID = 0;
                //if (_MerchPhRid != Include.NoRID) MERCHPHRID = this._MerchPhRid;

                //int MERCHPHLSEQ = 0;
                //if (_MerchPhRid != Include.NoRID) MERCHPHRID = this._MerchPhlSequence;
                int? SIZEGROUPRID_Nullable = null;
                if (this._SizeGroupRid != Include.NoRID) SIZEGROUPRID_Nullable = this._SizeGroupRid;

                int? SIZECURVEGROUPRID_Nullable = null;
                if (this._SizeCurveGroupRid != Include.NoRID) SIZECURVEGROUPRID_Nullable = this._SizeCurveGroupRid;

                int? MERCHHNRID_Nullable = null;
                if (_MerchHnRid != Include.NoRID) MERCHHNRID_Nullable = _MerchHnRid;

                int? MERCHPHRID_Nullable = null;
                if (_MerchPhRid != Include.NoRID) MERCHPHRID_Nullable = this._MerchPhRid;

                int? MERCHPHLSEQ_Nullable = null;
                if (_MerchPhRid != Include.NoRID) MERCHPHLSEQ_Nullable = this._MerchPhlSequence;
                

                double? AVGPACKDEVIATION_Nullable = null;
                if (OverrideAvgPackDevTolerance && this._AvgPackDeviationTolerance != double.MaxValue) AVGPACKDEVIATION_Nullable = this._AvgPackDeviationTolerance;

                double? MAXPACKNEED_Nullable = null;
                if (OverrideMaxPackNeedTolerance && this._MaxPackNeedTolerance != double.MaxValue) MAXPACKNEED_Nullable = this._MaxPackNeedTolerance;

                //int SIZEALTERNATERID = 0;
                //if (this._sizeAlternateRid != Include.NoRID) SIZEALTERNATERID =  this._sizeAlternateRid;

                //int SIZECONSTRAINTRID = 0;
                //if (this._sizeConstraintRid != Include.NoRID) SIZECONSTRAINTRID = this._sizeConstraintRid;
                int? SIZEALTERNATERID_Nullable = null;
                if (this._sizeAlternateRid != Include.NoRID) SIZEALTERNATERID_Nullable = this._sizeAlternateRid;

                int? SIZECONSTRAINTRID_Nullable = null;
                if (this._sizeConstraintRid != Include.NoRID) SIZECONSTRAINTRID_Nullable = this._sizeConstraintRid;
                //End TT#1236-MD -jsobek -Size Need Foreign Key Error

                int? GENCURVE_HCG_RID_Nullable = null;
                if (this.GenCurveHcgRID != Include.NoRID) GENCURVE_HCG_RID_Nullable = this.GenCurveHcgRID;

                int? GENCURVE_HN_RID_Nullable = null;
                if (this.GenCurveHnRID != Include.NoRID) GENCURVE_HN_RID_Nullable = this.GenCurveHnRID;

                int? GENCURVE_PH_RID_Nullable = null;
                if (GenCurvePhRID != Include.NoRID) GENCURVE_PH_RID_Nullable = this.GenCurvePhRID;

                int? GENCURVE_PHL_SEQUENCE_Nullable = null;
                if (GenCurvePhRID != Include.NoRID) GENCURVE_PHL_SEQUENCE_Nullable = this.GenCurvePhlSequence;

                char? GENCURVE_COLOR_IND_Nullable = null;
                if (this.GenCurveColorInd) GENCURVE_COLOR_IND_Nullable = Include.ConvertBoolToChar(this.GenCurveColorInd);

                int? GENCURVE_MERCH_TYPE_Nullable = null;
                if (this.GenCurveMerchType != eMerchandiseType.Undefined) GENCURVE_MERCH_TYPE_Nullable = (int)this.GenCurveMerchType;

                int? GENCONSTRAINT_HCG_RID_Nullable = null;
                if (this.GenConstraintHcgRID != Include.NoRID) GENCONSTRAINT_HCG_RID_Nullable = this.GenConstraintHcgRID;

                int? GENCONSTRAINT_HN_RID_Nullable = null;
                if (this.GenConstraintHnRID != Include.NoRID) GENCONSTRAINT_HN_RID_Nullable = this.GenConstraintHnRID;

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

                char? OVERRIDE_AVG_PACK_DEV_IND_Nullable = null;
                if (OverrideAvgPackDevTolerance) OVERRIDE_AVG_PACK_DEV_IND_Nullable = Include.ConvertBoolToChar(OverrideAvgPackDevTolerance);

                char? OVERRIDE_MAX_PACK_NEED_IND_Nullable = null;
                if (OverrideMaxPackNeedTolerance) OVERRIDE_MAX_PACK_NEED_IND_Nullable = Include.ConvertBoolToChar(OverrideMaxPackNeedTolerance);

                int? GENCURVE_NSCCD_RID_Nullable = null;
                if (this.GenCurveNsccdRID != Include.NoRID) GENCURVE_NSCCD_RID_Nullable = this.GenCurveNsccdRID;

                char? PACK_TOLERANCE_NO_MAX_STEP_IND_Nullable = null;
                if (OverrideMaxPackNeedTolerance) PACK_TOLERANCE_NO_MAX_STEP_IND_Nullable = Include.ConvertBoolToChar(_packToleranceNoMaxStep);

                char? PACK_TOLERANCE_STEPPED_IND_Nullable = null;
                if (OverrideMaxPackNeedTolerance) PACK_TOLERANCE_STEPPED_IND_Nullable = Include.ConvertBoolToChar(_packToleranceStepped);

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
              

                StoredProcedures.SP_MID_MTH_SZ_NEED_UPD_INS.Insert(td.DBA,
                                                               METHODRID: method_RID,
                                                               SIZEGROUPRID: SIZEGROUPRID_Nullable,  //TT#1236-MD -jsobek -Size Need Foreign Key Error
                                                               SIZECURVEGROUPRID: SIZECURVEGROUPRID_Nullable,  //TT#1236-MD -jsobek -Size Need Foreign Key Error
                                                               MERCHTYPE: (int)this._MerchType,
                                                               MERCHHNRID: MERCHHNRID_Nullable,  //TT#1236-MD -jsobek -Size Need Foreign Key Error
                                                               MERCHPHRID: MERCHPHRID_Nullable,  //TT#1236-MD -jsobek -Size Need Foreign Key Error
                                                               MERCHPHLSEQ: MERCHPHLSEQ_Nullable,  //TT#1236-MD -jsobek -Size Need Foreign Key Error
                                                               AVGPACKDEVIATION: AVGPACKDEVIATION_Nullable,
                                                               MAXPACKNEED: MAXPACKNEED_Nullable,
                                                               SIZEALTERNATERID: SIZEALTERNATERID_Nullable, //TT#1236-MD -jsobek -Size Need Foreign Key Error
                                                               SIZECONSTRAINTRID: SIZECONSTRAINTRID_Nullable, //TT#1236-MD -jsobek -Size Need Foreign Key Error
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
                                                               OVERRIDE_AVG_PACK_DEV_IND: OVERRIDE_AVG_PACK_DEV_IND_Nullable,
                                                               OVERRIDE_MAX_PACK_NEED_IND: OVERRIDE_MAX_PACK_NEED_IND_Nullable,
                                                               USE_DEFAULT_CURVE_IND: Include.ConvertBoolToChar(UseDefaultCurve),
                                                               GENCURVE_NSCCD_RID: GENCURVE_NSCCD_RID_Nullable,
                                                               PACK_TOLERANCE_NO_MAX_STEP_IND: PACK_TOLERANCE_NO_MAX_STEP_IND_Nullable,
                                                               PACK_TOLERANCE_STEPPED_IND: PACK_TOLERANCE_STEPPED_IND_Nullable,
                                                               APPLY_RULES_ONLY_IND: Include.ConvertBoolToChar(ApplyRulesOnly),
                                                               IB_MERCH_TYPE: (int)_IB_MerchandiseType,
                                                               IB_MERCH_HN_RID: IB_MERCH_HN_RID_Nullable,
                                                               IB_MERCH_PH_RID: IB_MERCH_PH_RID_Nullable,
                                                               IB_MERCH_PHL_SEQUENCE: IB_MERCH_PHL_SEQUENCE_Nullable,
                                                               VSW_SIZE_CONSTRAINTS_IND: VSW_SIZE_CONSTRAINTS_IND_Nullable,
                                                               VSW_SIZE_CONSTRAINTS: VSW_SIZE_CONSTRAINTS_Nullable
                                                               );
			}


			/// <summary>
			/// Inserts data associated with the method into METHOD_BASIS_SIZE_ALLOCATION
			/// </summary>
			/// <param name="method_RID">ID of the method to insert</param>
			/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
			/// <returns></returns>
			public bool InsertMethod(int method_RID, TransactionData td)
			{
				bool InsertSuccessfull = true;

				try
				{
					PerformUpdateInsert(method_RID, td);
		
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
			/// Updates data associated with the method into METHOD_BASIS_SIZE_ALLOCATION
			/// </summary>
			/// <param name="method_RID">ID of the method to update</param>
			/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
			/// <returns></returns>
			public bool UpdateMethod(int method_RID, TransactionData td)
			{
				bool UpdateSuccessful = true;
				try
				{
					PerformUpdateInsert(method_RID, td);
			
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
			/// Deletes data and child data associated with the basis size method
			/// </summary>
			/// <param name="method_RID">ID of the method to delete</param>
			/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
			/// <returns></returns>
			public bool DeleteMethod(int method_RID, TransactionData td)
			{
				bool Successfull = true;

				try
				{
                    StoredProcedures.SP_MID_MTH_SZ_NEED_DEL_MTH.Delete(td.DBA, METHODRID: method_RID);
			
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

		public DataTable GetMethodsByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_SIZE_NEED_READ_FROM_NODE.Read(_dba, MERCH_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
