using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for SizeMethodBase.
	/// </summary>
	public class AllocationSizeBaseMethod : AllocationBaseMethod
	{
        private Audit _audit; // MID Track 4372 Generic Size Constraints
		private CollectionRuleSets _SetsCollection;
		private CollectionDecoder _rulesDecoder;
		private Hashtable _rulesStoreGroupLevelHash;
		private DataSet _MethodConstraints;
		private bool _constraintsLoaded;
		private bool _GetSizes;
		//private int _SgRid;
		private int	_SizeGroupRid;
		private int _SizeCurveGroupRid;
		private eGetSizes _getSizesUsing;
		private eGetDimensions _getDimensionsUsing;
		private SizeMethodBaseData _sizeMethodBaseData;
		private MIDTimer _timer = new MIDTimer();
		private int _genCurveCharGroupRID;				// Generic Size Curve BEGIN
		private string _genCurveCharGroupID = null;
		private AllocationMerchBin _genCurve; 
		private bool _genCurveColorInd;			
		private eMerchandiseType _genCurveMerchType;	
		private char _delimiter; 
		private Header _header;
		private SizeCurve _sizeCurveData;
		private ApplicationSessionTransaction _trans;
		private Hashtable _genCurveHash;				// Generic Size Curve END
		private int _genConstraintCharGroupRID;				// Generic Size Constraint BEGIN
        //private string _genConstraintCharGroupID = null;
		private AllocationMerchBin _genConstraint; 
		private bool _genConstraintColorInd;			
		private eMerchandiseType _genConstraintMerchType;	
		private SizeModelData _sizeModelData;
		private Hashtable _genConstraintHash;				// Generic Size Constraint END
		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		private bool _normalizeSizeCurvesDefaultIsOverridden;
		private bool _normalizeSizeCurves;
		private GlobalOptionsProfile _globalOptions = null;
		// END MID Track #4826
        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        private bool _useDefaultCurve;
        // End TT#413
        private bool _applyRulesOnly; // TT#2155 - JEllis - Fill Size Holes Null Reference
        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        private int _genCurveNsccdRID;
        private string _genCurveNsccdCurveName = null;
        private MerchandiseHierarchyData _merchHierData;
        // End TT#413
        private DataSet _dsBackup = null;
        private bool _constraintRollback = false;

		#region Properties
		/// <summary>
		/// Gets Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get {return eProfileType.None;}
		}

		/// <summary>
		/// Gets or sets an instance of SizeMethodBaseData.
		/// </summary>
		public SizeMethodBaseData SMBD
		{
			get {return _sizeMethodBaseData;}
			set {_sizeMethodBaseData = value;}
		}

		/// <summary>
		/// Gets collection of Sets containing All Colors, Colors, Size Dimensions, and Sizes.
		/// </summary>
		public CollectionRuleSets RulesCollection
		{
			get { return _SetsCollection; }
		}

		public CollectionDecoder RulesDecoder
		{
			get { return _rulesDecoder; }
		}

		public Hashtable RulesStoreGroupLevelHash
		{
			get { return _rulesStoreGroupLevelHash; }
		}

		/// <summary>
		/// Gets or sets dataset of min/max constraints for the method.
		/// </summary>
		public DataSet MethodConstraints
		{
			get {return _MethodConstraints;}
			set {_MethodConstraints = value;}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the constraints have been loaded.
		/// </summary>
		public bool ConstraintsLoaded
		{
			get {return _constraintsLoaded;}
			set {_constraintsLoaded = value;}
		}

		/// <summary>
		/// Gets or sets whether to include size data in MethodConstraints dataset.
		/// </summary>
		public bool GetSizes
		{
			get {return _GetSizes;}
			set {_GetSizes = value;}
		}

		public eGetSizes GetSizesUsing
		{
			get {return _getSizesUsing;}
			set {_getSizesUsing = value;}
		}

		public eGetDimensions GetDimensionsUsing
		{
			get {return _getDimensionsUsing;}
			set {_getDimensionsUsing = value;}
		}
		/// <summary>
		/// Gets or sets Size Group RID.
		/// </summary>
		public int SizeGroupRid
		{
			get	{return _SizeGroupRid;}
			set {_SizeGroupRid = value;}
		}

		/// <summary>
		/// Gets or sets Size Curve Group RID.
		/// </summary>
		public int SizeCurveGroupRid
		{
			get {return _SizeCurveGroupRid;}
			set {_SizeCurveGroupRid = value;}
		}
		
		/// <summary>
		/// Gets or sets Generic Size Curve Header Characteristic Group RID
		/// </summary>
		public int GenCurveCharGroupRID
		{
			get {return  _genCurveCharGroupRID;}
			set { _genCurveCharGroupRID = value;}
		}
		
		/// <summary>
		/// Gets or sets a Node RID for Generic Size Curve Name 
		/// </summary>
		public int GenCurveHnRID
		{
			get
			{
				return  _genCurve.MdseHnRID;
			}
			set
			{
				_genCurve.SetMdseHnRID(value);
			}
		}

		/// <summary>
		/// Gets or sets hierarchy level for Generic Size Curve Name
        /// </summary>
		public int GenCurvePhRID
		{
			get
			{
				return  _genCurve.ProductHnLvlRID;
			}
			set
			{
				_genCurve.SetProductHnLvlRID(value);
			}
		}
		public int GenCurvePhlSequence
		{
			get
			{
				return  _genCurve.ProductHnLvlSeq;
			}
			set
			{
				_genCurve.SetProductHnLvlSeq(value);
			}
		}

		/// <summary>
		/// Gets or sets Generic Size Curve Color Code indicator
		/// </summary>
		public bool GenCurveColorInd
		{
			get {return  _genCurveColorInd;}
			set { _genCurveColorInd = value;}
		}

		/// <summary>
		/// Gets or sets Generic Size Curve Merch Type; needed to distinguish
		/// between OTSPlanLevel and no selection  
		/// </summary>
		public eMerchandiseType GenCurveMerchType
		{
			get {return  _genCurveMerchType;}
			set { _genCurveMerchType = value;}
		}
		
		/// <summary>
		/// Gets or sets Generic Size Curve Hashtable containing header RID and size curve group name
		/// between OTSPlanLevel and no selection  
		/// </summary>
		public Hashtable GenCurveHash
		{
			get {return  _genCurveHash;}
			set { _genCurveHash = value;}
		}
		
		//////////////////////////////////////////
		/// <summary>
		/// Gets or sets Generic Size Constraint Header Characteristic Group RID
		/// </summary>
		public int GenConstraintCharGroupRID
		{
			get {return  _genConstraintCharGroupRID;}
			set { _genConstraintCharGroupRID = value;}
		}
		
		/// <summary>
		/// Gets or sets a Node RID for Generic Size Constraint Name 
		/// </summary>
		public int GenConstraintHnRID
		{
			get
			{
				return  _genConstraint.MdseHnRID;
			}
			set
			{
				_genConstraint.SetMdseHnRID(value);
			}
		}

		/// <summary>
		/// Gets or sets hierarchy level for Generic Size Constraint Name
        /// </summary>
        public int GenConstraintPhRID
		{
			get
			{
				return  _genConstraint.ProductHnLvlRID;
			}
			set
			{
				_genConstraint.SetProductHnLvlRID(value);
			}
		}
		public int GenConstraintPhlSequence
		{
			get
			{
				return  _genConstraint.ProductHnLvlSeq;
			}
			set
			{
				_genConstraint.SetProductHnLvlSeq(value);
			}
		}

		/// <summary>
		/// Gets or sets Generic Size Constraint Color Code indicator
		/// </summary>
		public bool GenConstraintColorInd
		{
			get {return  _genConstraintColorInd;}
			set { _genConstraintColorInd = value;}
		}

		/// <summary>
		/// Gets or sets Generis Size Constraint Merch Type; needed to distinguish
		/// between OTSPlanLevel and no selection  
		/// </summary>
		public eMerchandiseType GenConstraintMerchType
		{
			get {return  _genConstraintMerchType;}
			set { _genConstraintMerchType = value;}
		}
		
		/// <summary>
		/// Gets or sets Generic Size Constraint Hashtable containing header RID ans size Constraint name
		/// between OTSPlanLevel and no selection  
		/// </summary>
		public Hashtable GenConstraintHash
		{
			get {return  _genConstraintHash;}
			set { _genConstraintHash = value;}
		}

		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		public bool NormalizeSizeCurvesDefaultIsOverridden
		{
			get	{ return _normalizeSizeCurvesDefaultIsOverridden;}
			set	{ _normalizeSizeCurvesDefaultIsOverridden = value;	}
		}
		public bool NormalizeSizeCurves
		{
			get	
			{ 
				if (_normalizeSizeCurvesDefaultIsOverridden)
				{
					return _normalizeSizeCurves;
				}
				else // if not overridden, return value from global options
				{
					return GlobalOptions.NormalizeSizeCurves;
				}
			}
			set	{ _normalizeSizeCurves = value;	}
		}

        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        public bool UseDefaultCurve
        {
            get { return _useDefaultCurve; }
            set { _useDefaultCurve = value; }
        }
        // End TT#413

        // begin TT#2155 - Jellis - Fill Size Holes Null Reference
        public bool ApplyRulesOnly
        {
            get { return _applyRulesOnly; }
            set { _applyRulesOnly = value; }
        }
        // end TT#2155 - JEllis - Fill Size Holes Null Reference

        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        public int GenCurveNsccdRID
        {
            get { return _genCurveNsccdRID; }
            set { _genCurveNsccdRID = value; }
        }
        // End TT#413

		protected GlobalOptionsProfile GlobalOptions
		{
			get	
			{ 
				if (_globalOptions == null)
				{
                    // Begin TT#5124 - JSmith - Performance
                    //_globalOptions= new GlobalOptionsProfile(Include.NoRID);
                    //_globalOptions.LoadOptions();
                    _globalOptions = SAB.ApplicationServerSession.GlobalOptions;
                    // End TT#5124 - JSmith - Performance
				}
				return _globalOptions;
			}
		}
		// END MID Track #4826

        // Begin Development TT#13 - JSmith - Basis Size method give Null reference error
        public ApplicationSessionTransaction Trans
        {
            set 
            {
                if (_trans == null)
                {
                    _trans = value;
                }
            }
        }
        // End Development TT#13
		//////////////////////////////////////////

        protected DataSet DataSetBackup
        {
            get { return _dsBackup; }
            set { _dsBackup = value; }
        }

        protected bool ConstraintRollback
        {
            get { return _constraintRollback; }
            set { _constraintRollback = value; }
        }
		#endregion

		/// <summary>
		/// Creates an instance of Allocation Base Method
		/// </summary>
		/// <param name="SAB">Session Address Block</param>
		/// <param name="aMethodRID">RID for the Method.</param>
		/// <param name="aMethodType">Method Type.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public AllocationSizeBaseMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType aMethodType):base(SAB, aMethodRID, aMethodType)
		public AllocationSizeBaseMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType aMethodType, eProfileType aProfileType):base(SAB, aMethodRID, aMethodType, aProfileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			_constraintsLoaded = false;
			_audit = SAB.ApplicationServerSession.Audit; // MID Track 4372 Generic Size Constraints
            _genCurveHash = new Hashtable();             // MID Track 4372 Generic Size Constraints
			_genConstraintHash = new Hashtable();       // MID Track 4372 Generic Size Constraints
			_sizeCurveData = new MIDRetail.Data.SizeCurve();  // MID Track 4372 Generic Size Constraints
			_sizeModelData = new SizeModelData(); // MID Track 4372 Generic Size Contraints
            _merchHierData = new MerchandiseHierarchyData(); // TT#413 - add Node Curve Name 
		}

        // restore the saved rules if needed
        override public bool CleanUp()
        {
            TransactionData td = new TransactionData();
            try
            {
                if (_dsBackup != null
                    && ConstraintRollback
                   )
                {
                    if (!td.ConnectionIsOpen)
                    {
                        td.OpenUpdateConnection();
                    }
                    MethodConstraints = DataSetBackup;
                    InsertUpdateMethodRules(td);

                    if (td.ConnectionIsOpen)
                    {
                        td.CommitData();
                        td.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception)
            {
                if (td.ConnectionIsOpen)
                {
                    td.Rollback();
                    td.CloseUpdateConnection();
                }
            }

            return true;
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            return false;
        }

        internal bool CheckSizeMethodForUserData()
        {
            if (IsStoreGroupUser(SG_RID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(GenCurveHnRID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(GenConstraintHnRID))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        ///// <summary>
        ///// Initializes parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //virtual public MIDDbParameter[] InitSetInParams()
        //{
        //    MIDDbParameter[] inParams  = { new MIDDbParameter("@METHODRID", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SGLRID", eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@RULE",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@QTY",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@ROWTYPEID",eDbType.Int,0,"", eParameterDirection.Input)
        //                              } ;

        //    return inParams;
        //}


        ///// <summary>
        ///// Initializes parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //virtual public MIDDbParameter[] InitSetInParams(ItemRuleSet pItem)
        //{
        //    MIDDbParameter[] inParams  = InitSetInParams();

        //    #region SET PARAMETER VALUES
        //    inParams[0].Value = pItem.MethodRid;

        //    inParams[1].Value = pItem.SglRid;

        //    if (pItem.Rule != Include.UndefinedRule)
        //    {
        //        inParams[2].Value = pItem.Rule;
        //    }

        //    if (pItem.Qty != Include.UndefinedQuantity)
        //    {
        //        inParams[3].Value = pItem.Qty;
        //    }

        //    inParams[4].Value = pItem.RowTypeID;
        //    #endregion

        //    return inParams;
        //}


        ///// <summary>
        ///// Creates parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //virtual public MIDDbParameter[] InitRuleInParams()
        //{
        //    MIDDbParameter[] inParams  = { new MIDDbParameter("@METHODRID", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SGLRID", eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@COLORCODERID", eDbType.Int, 0, "", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SIZESRID",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@SIZECODERID",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@RULE",eDbType.Int,1,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@QTY",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@ROWTYPEID",eDbType.Int,0,"", eParameterDirection.Input),
        //                                  new MIDDbParameter("@DIMENSIONS_RID",eDbType.Int,0,"", eParameterDirection.Input)
        //                              } ;

        //    return inParams;
        //}


        ///// <summary>
        ///// Initializes parameters for stored procedure.
        ///// </summary>
        ///// <returns></returns>
        //virtual public MIDDbParameter[] InitRuleInParams(RuleItemBase pItem)
        //{
        //    MIDDbParameter[] inParams  = InitRuleInParams();

        //    #region SET PARAMETER VALUES
        //    inParams[0].Value = pItem.MethodRid;

        //    inParams[1].Value = pItem.SglRid;

        //    inParams[2].Value = pItem.ColorCodeRid;

        //    inParams[3].Value = pItem.SizesRid;

        //    inParams[4].Value = pItem.SizeCodeRid;

	
        //    if (pItem.Rule != Include.UndefinedRule)
        //    {
        //        inParams[5].Value = pItem.Rule;
        //    }

        //    if (pItem.Qty != Include.UndefinedQuantity)
        //    {
        //        inParams[6].Value = pItem.Qty;
        //    }

        //    inParams[7].Value = pItem.RowTypeID;

        //    inParams[8].Value = pItem.DimensionsRid;
        //    #endregion

        //    return inParams;

        //}


		/// <summary>
		/// Fills the constraint collection.  Collection can be retrieved with CollectionSets property.
		/// </summary>
		virtual public void FillCollections()
		{
			int setIdx;
			int allColorIdx;
			int colorIdx;
			int allColorSizeIdx;
			int colorSizeIdx;
			int	colorSizeDimIdx;
			int allColorSizeDimIdx;

			if (_SetsCollection != null)
			{
				_SetsCollection.Clear();
			}

			_SetsCollection = new CollectionRuleSets();

			if (MethodConstraints.Tables["SetLevel"].Rows.Count > 0)
			{
				#region FOR EACH SET LEVEL
				foreach (DataRow drSet in MethodConstraints.Tables["SetLevel"].Rows)
				{
					setIdx = RulesCollection.Add(new ItemRuleSet(base.Key, drSet));

					#region ALL COLOR
					foreach (DataRow drAllColor in drSet.GetChildRows("SetAllColor"))
					{
						allColorIdx = RulesCollection[setIdx].collectionRuleAllColors.Add(new ItemRuleAllColor(base.Key, drAllColor));

						#region SIZE DIMENSION
						if (MethodConstraints.Relations.Contains("AllColorSizeDimension"))
						{
							foreach (DataRow drACSizeDim in drAllColor.GetChildRows("AllColorSizeDimension"))
							{
								allColorSizeDimIdx = RulesCollection[setIdx].collectionRuleAllColors[allColorIdx].collectionRuleSizeDimensions.Add(new ItemRuleSizeDimension(base.Key, drACSizeDim));
									
								#region SIZE
								foreach (DataRow drACSize in drACSizeDim.GetChildRows("AllColorSize"))
								{
									allColorSizeIdx = RulesCollection[setIdx].collectionRuleAllColors[allColorIdx].collectionRuleSizeDimensions[allColorSizeDimIdx].collectionRuleSizes.Add(new ItemRuleSize(base.Key, drACSize));
								}
								#endregion SIZE
							}
						}
						#endregion SIZE DIMENSION
					}
					#endregion ALL COLOR

					#region COLOR
					foreach (DataRow drColor in drSet.GetChildRows("SetColor"))
					{
						colorIdx = RulesCollection[setIdx].collectionRuleColors.Add(new ItemRuleColor(base.Key, drColor));

						#region SIZE DIMENSION
						if (MethodConstraints.Relations.Contains("ColorSizeDimension"))
						{
							foreach (DataRow drCSizeDim in drColor.GetChildRows("ColorSizeDimension"))
							{
								colorSizeDimIdx = RulesCollection[setIdx].collectionRuleColors[colorIdx].collectionRuleSizeDimensions.Add(new ItemRuleSizeDimension(base.Key, drCSizeDim));
									
								#region SIZE
								foreach (DataRow drCSize in drCSizeDim.GetChildRows("ColorSize"))
								{
									colorSizeIdx = RulesCollection[setIdx].collectionRuleColors[colorIdx].collectionRuleSizeDimensions[colorSizeDimIdx].collectionRuleSizes.Add(new ItemRuleSize(base.Key, drCSize));
								}
								#endregion SIZE
							}
						}
						#endregion SIZE DIMENSION
					}
					#endregion COLOR
				}
				#endregion FOR EACH SET LEVEL
			}

			DebugSetsCollection();
		}


		/// <summary>
		/// Fills the MethodConstraints DataSet.
		/// </summary>
		/// <returns></returns>
		virtual public bool CreateConstraintData()
		{
			bool Success = true;

			try
			{
				MethodConstraints = MIDEnvironment.CreateDataSet();


				int rid = Include.NoRID;  //will hold SizeGroupRID or SizeCurveGroupRID
				switch (GetSizesUsing)
				{
					case eGetSizes.SizeGroupRID:
						if (SizeGroupRid != Include.NoRID)
						{
							GetSizes = true;
							rid = SizeGroupRid;
						}
						else
						{
							GetSizes = false;
						}
						break;
					case eGetSizes.SizeCurveGroupRID:
						if (SizeCurveGroupRid != Include.NoRID)
						{
							GetSizes = true;
							rid = SizeCurveGroupRid;
						}
						else
						{
							GetSizes = false;
						}
						break;
				}

                // Begin TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
                //MethodConstraints = _sizeMethodBaseData.GetChildData(base.Key, SG_RID, GetSizes);
                MethodConstraints = _sizeMethodBaseData.GetChildData(base.Key, SG_RID, GetSizes, StoreMgmt.StoreGroup_GetVersion(SG_RID));
                // End TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
						
				// Begin Issue 3685 - stodd 2/10/2006
				if (GetSizes)
				{
					DataTable dtSizes = GetSizesDataTable(rid, GetSizesUsing);

					DataTable dtSizeCollection = MethodConstraints.Tables["ColorSize"];
					SetSizeSequence(dtSizes, dtSizeCollection);
					dtSizeCollection = MethodConstraints.Tables["AllColorSize"];
					SetSizeSequence(dtSizes, dtSizeCollection);
					dtSizeCollection = MethodConstraints.Tables["AllColorSizeDimension"];
					SetSizeSequence(dtSizes, dtSizeCollection);
					dtSizeCollection = MethodConstraints.Tables["ColorSizeDimension"];
					SetSizeSequence(dtSizes, dtSizeCollection);
				}
				// End Issue 3685 - stodd 2/10/2006

				FillCollections();
				_constraintsLoaded = true;

			}
			catch
			{
				Success = false;
				throw;
			}
			return Success;

		}

        /// <summary>
		/// Sets the size sequences in the MethodConstraints DataSet.
		/// </summary>
		/// <returns></returns>
		public bool SetSizeCodeSequences()
        {
            bool Success = true;

            try
            {
                int rid = Include.NoRID;  //will hold SizeGroupRID or SizeCurveGroupRID
                switch (GetSizesUsing)
                {
                    case eGetSizes.SizeGroupRID:
                        if (SizeGroupRid != Include.NoRID)
                        {
                            GetSizes = true;
                            rid = SizeGroupRid;
                        }
                        else
                        {
                            GetSizes = false;
                        }
                        break;
                    case eGetSizes.SizeCurveGroupRID:
                        if (SizeCurveGroupRid != Include.NoRID)
                        {
                            GetSizes = true;
                            rid = SizeCurveGroupRid;
                        }
                        else
                        {
                            GetSizes = false;
                        }
                        break;
                }

                if (GetSizes)
                {
                    DataTable dtSizes = GetSizesDataTable(rid, GetSizesUsing);

                    DataTable dtSizeCollection = MethodConstraints.Tables["ColorSize"];
                    SetSizeSequence(dtSizes, dtSizeCollection);
                    dtSizeCollection = MethodConstraints.Tables["AllColorSize"];
                    SetSizeSequence(dtSizes, dtSizeCollection);
                    dtSizeCollection = MethodConstraints.Tables["AllColorSizeDimension"];
                    SetSizeSequence(dtSizes, dtSizeCollection);
                    dtSizeCollection = MethodConstraints.Tables["ColorSizeDimension"];
                    SetSizeSequence(dtSizes, dtSizeCollection);
                }

            }
            catch
            {
                Success = false;
                throw;
            }
            return Success;

        }

        /// <summary>
        /// Finds the sizeCodeRid on the dtSizes table and returns it size sequence
        /// </summary>
        /// <param name="dtSizes"></param>
        /// <param name="sizeCodeRid"></param>
        /// <returns></returns>
        private int GetSizeCodeSequence(DataTable dtSizes, int sizeCodeRid)
		{
			int seq = int.MaxValue;
			DataRow [] rows = dtSizes.Select("SIZE_CODE_RID = " + sizeCodeRid.ToString(CultureInfo.CurrentUICulture));
			if (rows.Length > 0)
			{
				seq = Convert.ToInt32(rows[0]["PRIMARY_SEQ"], CultureInfo.CurrentUICulture); 
			}
			return seq;
		}
		/// <summary>
		/// Finds the dimensionRid on the dtSizes table and returns it size sequence
		/// </summary>
		/// <param name="dtSizes"></param>
		/// <param name="dimensionRid"></param>
		/// <returns></returns>
		private int GetDimensionSequence(DataTable dtSizes, int dimensionRid)
		{
			int seq = int.MaxValue;
			DataRow [] rows = dtSizes.Select("DIMENSIONS_RID = " + dimensionRid.ToString(CultureInfo.CurrentUICulture));
			if (rows.Length > 0)
			{
				seq = Convert.ToInt32(rows[0]["SECONDARY_SEQ"], CultureInfo.CurrentUICulture); 
			}
			return seq;
		}

		/// <summary>
		/// Loops through the size collection and, using the dtSizes table, assigns
		/// the size seq to each row.
		/// </summary>
		/// <param name="dtSizes"></param>
		/// <param name="dtSizeCollection"></param>
		private void SetSizeSequence(DataTable dtSizes, DataTable dtSizeCollection)
		{
			if (dtSizeCollection != null)
			{
				foreach (DataRow aRow in dtSizeCollection.Rows)
				{
					int sizeCodeRid = Convert.ToInt32(aRow["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
					int dimensionsRid = Convert.ToInt32(aRow["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);
					if (sizeCodeRid != Include.NoRID)
					{
						aRow["SIZE_SEQ"] = GetSizeCodeSequence(dtSizes, sizeCodeRid);
					}
					else if (dimensionsRid != Include.NoRID)
					{
						aRow["SIZE_SEQ"] = GetDimensionSequence(dtSizes, dimensionsRid);
					}
				}
				dtSizeCollection.DefaultView.Sort = "SIZE_SEQ";	
			}
		}

		/// <summary>
		/// fills and returns dtSizes table
		/// </summary>
        /// <param name="rid"></param>
		/// <param name="getSizes"></param>
		/// <returns></returns>
		private DataTable GetSizesDataTable(int rid, eGetSizes getSizes)
		{
			DataTable dtSizes = BuildSizesDataTable();

			if (getSizes == eGetSizes.SizeGroupRID)
			{
				SizeGroupProfile sgp = new SizeGroupProfile(rid);
				foreach (SizeCodeProfile scp in sgp.SizeCodeList.ArrayList)
				{
					DataRow newRow = dtSizes.NewRow();
					newRow["SIZES_RID"] = scp.SizeCodePrimaryRID;
					newRow["SIZE_CODE_PRIMARY"] = scp.SizeCodePrimary;
					newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
					newRow["SIZE_CODE_RID"] = scp.Key;
					newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
					newRow["PRIMARY_SEQ"] = scp.PrimarySequence;
					newRow["SECONDARY_SEQ"] = scp.SecondarySequence;
					dtSizes.Rows.Add(newRow);
				}

			}
			else  // (getSizes == eGetSizes.SizeGroupRID)
			{
				SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(rid);
				foreach (SizeCodeProfile scp in scgp.SizeCodeList.ArrayList)
				{
					DataRow newRow = dtSizes.NewRow();
					newRow["SIZES_RID"] = scp.SizeCodePrimaryRID;
					newRow["SIZE_CODE_PRIMARY"] = scp.SizeCodePrimary;
					newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
					newRow["SIZE_CODE_RID"] = scp.Key;
					newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
					newRow["PRIMARY_SEQ"] = scp.PrimarySequence;
					newRow["SECONDARY_SEQ"] = scp.SecondarySequence;
					dtSizes.Rows.Add(newRow);
				}
			}
			return dtSizes;
		}


		private DataTable BuildSizesDataTable()
		{
			DataTable dtSizes = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZES_RID";
			dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZE_CODE_RID";
			dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "DIMENSIONS_RID";
			dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_PRIMARY";
			dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_SECONDArY";
			dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "PRIMARY_SEQ";
			dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SECONDARY_SEQ";
			dtSizes.Columns.Add(dataColumn);

			return dtSizes;
		}

		public void DebugSetsCollection()
		{
			//PROCESS SETS AND ALL DESCENDANTS
			Debug.WriteLine("---------------------------------------------");
			foreach (ItemRuleSet oItemSet in _SetsCollection)
			{
				Debug.WriteLine("RID " + oItemSet.MethodRid.ToString() +
					" SGL " + oItemSet.SglRid.ToString() + " RULE " + oItemSet.Rule.ToString() +
					" QTY " + oItemSet.Qty.ToString());
									
				//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
				foreach (ItemRuleAllColor oItemAllColor in oItemSet.collectionRuleAllColors)
				{
					DebugItem(1, oItemAllColor);

					foreach (ItemRuleSizeDimension oItemSizeDimension in oItemAllColor.collectionRuleSizeDimensions)
					{
						DebugItem(2, oItemSizeDimension);

						foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
						{
							DebugItem(3, oItemSize);
						}
					}
				}

				//PROCESS COLOR LEVEL AND ALL DESCENDANTS
				foreach (ItemRuleColor oItemColor in oItemSet.collectionRuleColors)
				{
					DebugItem(1, oItemColor);

					foreach (ItemRuleSizeDimension oItemSizeDimension in oItemColor.collectionRuleSizeDimensions)
					{
						DebugItem(2, oItemSizeDimension);

						foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
						{
							DebugItem(3, oItemSize);
						}

					}
				}
			}


		}

		private void DebugItem(int level, RuleItemBase pItem)
		{
			string sLevel = string.Empty;
			switch (level)
			{
				case 1:
					sLevel = "  ";
					break;
				case 2: 
					sLevel = "    ";
					break;
				case 3:
					sLevel = "      ";
					break;
				default:
					sLevel = "--";
					break;
			}
			Debug.WriteLine(sLevel + "SGL " + pItem.SglRid.ToString() +
				" COLOR " + pItem.ColorCodeRid.ToString() +
				" DIM " + pItem.DimensionsRid.ToString() +
				" SIZE/SIZES " + pItem.SizeCodeRid.ToString() + "/" + pItem.SizesRid.ToString() +
				" Rule " + pItem.Rule.ToString() +
				" QTY " + pItem.Qty.ToString());
		}
		/// <summary>
		/// Public method that will delete method rules data using
		/// SizeMethodBaseData.DeleteMethodConstraints
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <remarks>
		/// If connection is closed, method will handle opening, commit|rollback, and closing.
		/// </remarks>
		/// <returns></returns>
		virtual public bool DeleteMethodRules(TransactionData td)
		{
			bool Successful;
			Successful = true;
			bool IsConnectionOpen = td.ConnectionIsOpen;

			try
			{
				if (!IsConnectionOpen)
				{
					td.OpenUpdateConnection();
				}

				Successful = _sizeMethodBaseData.DeleteMethodRules(base.Key, td);

				if (!IsConnectionOpen)
				{
					td.CommitData();
					td.CloseUpdateConnection();
				}
			}
			catch(Exception e)
			{
				if (!IsConnectionOpen)
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}
				string exceptionMessage = e.Message;
				Successful = false;
				throw;
			}

			return Successful;

		}


		/// <summary>
		/// Public method that will insert method rule data using
		/// SizeMethodBaseData.ProcessGroupLevel and SizeMethodBaseData.ProcessRules
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <remarks>
		/// If connection is closed, method will handle opening, commit|rollback, and closing.
		/// </remarks>
		/// <returns></returns>
		virtual public bool InsertUpdateMethodRules(TransactionData td)
		{
			bool Successfull = true;
			bool IsConnectionOpen = td.ConnectionIsOpen;

			try
			{	
				FillCollections();
					
				if (!IsConnectionOpen)
				{
					td.OpenUpdateConnection();
				}

				DeleteMethodRules(td);

				//PROCESS SETS AND ALL DESCENDANTS
				foreach (ItemRuleSet oItemSet in RulesCollection)
				{
					ProcessSet(td, oItemSet);
									
					//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
					foreach (ItemRuleAllColor oItemAllColor in oItemSet.collectionRuleAllColors)
					{
						ProcessRules(td, oItemAllColor);

						foreach (ItemRuleSizeDimension oItemSizeDimension in oItemAllColor.collectionRuleSizeDimensions)
						{
							ProcessRules(td, oItemSizeDimension);

							foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
							{
								ProcessRules(td, oItemSize);
							}
						}
					}

					//PROCESS COLOR LEVEL AND ALL DESCENDANTS
					foreach (ItemRuleColor oItemColor in oItemSet.collectionRuleColors)
					{
						ProcessRules(td, oItemColor);

						foreach (ItemRuleSizeDimension oItemSizeDimension in oItemColor.collectionRuleSizeDimensions)
						{
							ProcessRules(td, oItemSizeDimension);

							foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
							{
								ProcessRules(td, oItemSize);
							}

						}
					}
				}
					
				if (!IsConnectionOpen)
				{
					td.CommitData();
					td.CloseUpdateConnection();
				}

			}
			catch(Exception e)
			{
				if (!IsConnectionOpen)
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}
				string exceptionMessage = e.Message;
				Successfull = false;
				throw;
			}

			return Successfull;

		}

		public void BuildRulesDecoder(int storeGroupRid)
		{
			_timer.Start();
			try
			{
				if (storeGroupRid != Include.NoRID)
				{
					if (_rulesStoreGroupLevelHash == null)
                        _rulesStoreGroupLevelHash = StoreMgmt.GetStoreGroupLevelHashTable(storeGroupRid);  //SAB.StoreServerSession.GetStoreGroupLevelHashTable(storeGroupRid);
                    _rulesDecoder = new CollectionDecoder(this._trans, this.RulesCollection, _rulesStoreGroupLevelHash);    // TT#1432 - Size Dimension Constraints not working
                    //_rulesDecoder = new CollectionDecoder(this.RulesCollection, _rulesStoreGroupLevelHash);  // TT#1432 - Size Dimension Constraints not working
				}
				else
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_SizeNeedAlgorithmInvalidStoreAttr,
						MIDText.GetText(eMIDTextCode.msg_al_SizeNeedAlgorithmInvalidStoreAttr));
				}
			}
			catch (Exception ex)
			{
				this.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_al_BasisSizeAlgorithmInvalidStoreAttr, ex.Message, this.GetType().Name); // MID Track 5778 - Scheduler 'Run Now' feature errors in audit
				throw;
			}
			finally
			{
				_timer.Stop("Build Rules Decoder");
			}
		}

		/// <summary>
		/// checks the rule collection to see if the store is excluded for this size and color
		/// </summary>
		/// <param name="storeRid">RID that identifies the store</param>
		/// <param name="colorRid">RID that identifies the color</param>
		/// <param name="aSizeCodeProfile">Profile that describes the size</param>
		/// <returns></returns>
		//public bool IsStoreExcluded(int storeRid, int colorRid, int sizeRid) // MID Track 3492 Size Need with constraints not allocating correctly
        //public bool IsStoreExcluded(int storeRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
        public bool IsStoreExcluded(int storeRid, int colorRid, int sizeCodeRID)                   // TT#1391 - TMW New Action 
		{
			bool exclude = false;
			//int sglRid = (int)_rulesStoreGroupLevelHash[storeRid];  //Issue 4244

			if (_rulesDecoder != null)
			{
				//RuleItemBase aRule = (RuleItemBase)_rulesDecoder.GetItem(sglRid, colorRid , sizeRid); // MID Track 3492 Size Need with constraints not allocating correctly
                //RuleItemBase aRule = (RuleItemBase)_rulesDecoder.GetItemForStore(storeRid, colorRid, aSizeCodeProfile); // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
                RuleItemBase aRule = (RuleItemBase)_rulesDecoder.GetItemForStore(storeRid, colorRid, sizeCodeRID);        // TT#1391 - TMW New Action
				if (aRule.Rule == (int)eSizeRuleType.Exclude)  // MID Track 3619 Remove Fringe
				{
					exclude = true;
				}
			}

			return exclude;
		}

		/// <summary>
		/// Processes ItemColor, ItemAllColor, ItemSize, or ItemSizeDimension objects from CollectionSets
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <param name="pItem">Object of type RuleItemBase</param>
		/// <returns></returns>
		virtual public bool ProcessRules(TransactionData td, RuleItemBase pItem)
		{
			bool Successfull = true;

			try
			{
				//MIDDbParameter[] inParams  = InitRuleInParams(pItem);

				//_sizeMethodBaseData.ProcessRules(td, inParams);
                //Begin TT#1298-MD -jsobek -Null Rules are being populated with 0's and 1's causing the Method to not process correctly
                //int RULE = 1;
                int? RULE_Nullable = null;
                if (pItem.Rule != Include.UndefinedRule) RULE_Nullable = pItem.Rule;

                //int QTY = 0;
                int? QTY_Nullable = null;
                if (pItem.Qty != Include.UndefinedQuantity) QTY_Nullable = pItem.Qty;

                _sizeMethodBaseData.ProcessRules(td,
                                                 METHODRID: pItem.MethodRid,
                                                 SGLRID: pItem.SglRid,
                                                 COLORCODERID: pItem.ColorCodeRid,
                                                 SIZESRID: pItem.SizesRid,
                                                 SIZECODERID: pItem.SizeCodeRid,
                                                 RULE: RULE_Nullable,
                                                 QTY: QTY_Nullable,
                                                 ROWTYPEID: (int)pItem.RowTypeID,
                                                 DIMENSIONS_RID: pItem.DimensionsRid
                                                );
                //End TT#1298-MD -jsobek -Null Rules are being populated with 0's and 1's causing the Method to not process correctly
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


		/// <summary>
		/// Processes ItemSet object from CollectionSets
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <param name="pItem">Object of type ItemSet</param>
		/// <returns></returns>
		virtual public bool ProcessSet(TransactionData td, ItemRuleSet pItem)
		{
			bool Successfull = true;

			try
			{
                //MIDDbParameter[] inParams  = InitSetInParams(pItem);
                //_sizeMethodBaseData.ProcessGroupLevel(td, inParams);

                //Begin TT#1298-MD -jsobek -Null Rules are being populated with 0's and 1's causing the Method to not process correctly
                //int RULE = 0; 
                int? RULE_Nullable = null;
                if (pItem.Rule != Include.UndefinedRule) RULE_Nullable = pItem.Rule;

                //int QTY = 0;
                int? QTY_Nullable = null;
                if (pItem.Qty != Include.UndefinedQuantity) QTY_Nullable = pItem.Qty;

                _sizeMethodBaseData.ProcessGroupLevel(td, 
                                                      METHODRID: pItem.MethodRid, 
                                                      SGLRID: pItem.SglRid,
                                                      RULE: RULE_Nullable,
                                                      QTY: QTY_Nullable, 
                                                      ROWTYPEID: (int)pItem.RowTypeID
                                                     );
                //End TT#1298-MD -jsobek -Null Rules are being populated with 0's and 1's causing the Method to not process correctly
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

		// add Generic Size Curve
		#region GenericSizeCurve

		virtual public bool OkToProcess(ApplicationSessionTransaction aTrans, AllocationProfile ap)
		{
			bool okToProcess = true;
			_trans = aTrans; // MID Track 4372 Generic Size Constraints
			try
			{
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                {
                    okToProcess = false;
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                    aTrans.SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                }
                else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                {
                    okToProcess = false;
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                    aTrans.SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                }
                // End TT#1966-MD - JSmith- DC Fulfillment
                if (GenericSizeCurveDefined())
                {
                    //_trans = aTrans; // MID Track 4372 Generic Size Constraints
                    if (!GenercSizeCurvesFound(ap))
                    {
                        okToProcess = false;
                    }
                }
             
				// begin MID Track 4372 Generic Size Constraints
				if (this.GenericSizeConstraintsDefined())
				{
					if (!this.GenericSizeConstraintsFound(ap))
					{
						okToProcess = false;
					}
				}
				// end MID Track 4372 Generic Size Constraints
			}
			catch
			{
				okToProcess = false;
				throw;
			}
			return  okToProcess;
		}		

		virtual public bool GenericSizeCurveDefined()
		{
			bool genCurveDefined = false;
			try
			{
                if (this.GenCurveCharGroupRID != Include.NoRID || this.GenCurveNsccdRID != Include.NoRID || // TT#413 - RMatelic add GenCurveNsccdRID
					this.GenCurveMerchType != eMerchandiseType.Undefined ||
                    this.GenCurveColorInd || this.UseDefaultCurve)           // TT#438 - RMatelic add UseDefaultCurve
				{
					genCurveDefined = true;
				}
			}
			catch
			{
				genCurveDefined = false;
				throw;
			}
			return  genCurveDefined;
		}	
	
		private bool GenercSizeCurvesFound(AllocationProfile ap)
		{
			bool genCurvesFound = true;
			try
			{
				_genCurveHash = new Hashtable();
				_header = new Header();

				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				// do not call client to eliminate serialization issues
//				_delimiter = SAB.ClientServerSession.GlobalOptions.ProductLevelDelimiter;
				_delimiter = GlobalOptions.ProductLevelDelimiter;
				// END MID Track #4826

				if (_genCurveCharGroupRID != Include.NoRID)
				{
					HeaderCharGroupProfileList hcgpl = SAB.HeaderServerSession.GetHeaderCharGroups();
					foreach (HeaderCharGroupProfile hcgp in hcgpl)
					{
						if (hcgp.Key == _genCurveCharGroupRID)
						{
							_genCurveCharGroupID = hcgp.ID;
							break;
						}
					}
				}

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings
                _genCurveNsccdCurveName = string.Empty;
                if (_genCurveNsccdRID != Include.NoRID)
                {
                    DataTable dt = _merchHierData.SizeCurveName_Read(_genCurveNsccdRID);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr= dt.Rows[0];
                        _genCurveNsccdCurveName = Convert.ToString(dr["CURVE_NAME"], CultureInfo.CurrentUICulture).Trim();
                    }

                }
                // End TT#413 

				//_sizeCurveData = new MIDRetail.Data.SizeCurve();  // MID Track 4372 Generic Size Constraints

				//				AllocationProfileList headerList = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.Allocation);
				//				foreach (AllocationProfile ap in headerList)
				//				{
				if (!ValidGenericSizeCurveName(ap))
				{
					genCurvesFound = false;  
					//						break;	
				}
				//				}
			}
			catch
			{
				genCurvesFound = false;
				throw;
			}
			return  genCurvesFound;
		}
		public bool ValidGenericSizeCurveName(AllocationProfile ap)
		{
			// begin MID Track 4372 Generic Size Constraints
			bool validGenericSizeCurveName = true;
			string sizeCurveName = string.Empty;
			string errMessage;
			try
			{
				int genericCurveColorRID = Include.NoRID;
				if (_genCurveColorInd)
				{
					if (ap.BulkColors.Count == 1)
					{
						foreach (int colorRID in ap.BulkColors.Keys)															
						{
							genericCurveColorRID = colorRID;
						}
					}
					else if (ap.BulkColors.Count == 0)
					{
						validGenericSizeCurveName = false;
						errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoGenericSizeColorForHeader),ap.HeaderID);
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_NoGenericSizeColorForHeader,errMessage); 

					}
					else
					{
						validGenericSizeCurveName = false;
						errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_MoreThanOneColorForHeader),ap.HeaderID);
						_audit.Add_Msg(
							eMIDMessageLevel.Severe,
							eMIDTextCode.msg_al_MoreThanOneColorForHeader,
							errMessage,
							this.GetType().Name);
					}
				}
				if (validGenericSizeCurveName)
				{
					sizeCurveName = 
						this._trans.GetGenericSizeName
						(ap,
						_genCurveCharGroupRID,
						_genCurveMerchType,
						_genCurve,
                        genericCurveColorRID, _genCurveNsccdCurveName, _useDefaultCurve, true);     // TT#413 - RMatelic - add  _genCurveNsccdCurveName; TT#438 - add _useDefaultCurve & true
					if (sizeCurveName == null
						|| sizeCurveName == string.Empty)
					{
						validGenericSizeCurveName = false;
					}
					else
					{
						int sizeCurveGroupRID = _sizeCurveData.GetSizeCurveGroupKey(sizeCurveName);
						if (sizeCurveGroupRID != Include.NoRID)
						{
 							// begin TT#1168 - MD - Jellis - Group ALlocation Null Reference when process Size Need
                            AllocationProfile[] apList;
                            // Begin TT#4988 - BVaughan - Performance
                            #if DEBUG
                            if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                            {
                                throw new Exception("Object does not match AssortmentProfile in ValidGenericSizeCurveName()");
                            }
                            #endif

                            // if (ap is AssortmentProfile)
                            if (ap.isAssortmentProfile)
                            // End TT#4988 - BVaughan - Performance
                            {
                                apList = new AllocationProfile[((AssortmentProfile)ap).AssortmentMembers.Length + 1];
                                apList[0] = ap;
                                ((AssortmentProfile)ap).AssortmentMembers.CopyTo(apList, 1);
                            }
                            else
                            {
                                apList = new AllocationProfile[1];
                                apList[0] = ap;
                            }
                            foreach (AllocationProfile allocationProfile in apList)
                            {
                                try
                                {
                                    _genCurveHash.Remove(allocationProfile.Key);
                                    _genCurveHash.Add(allocationProfile.Key, sizeCurveGroupRID);
                                }
                                finally
                                {
                                }
                            }
                            //try
                            //{
                            //    _genCurveHash.Add(ap.Key,sizeCurveGroupRID);
                            //}
                            //catch (ArgumentException)
                            //{
                            //    _genCurveHash.Remove(ap.Key);
                            //    _genCurveHash.Add(ap.Key,sizeCurveGroupRID);
                            //}
                            // end TT#1168 - MD - Jellis - Group Allocation Null Reference when process Size need
						}
						else
						{
							validGenericSizeCurveName = false;
                            // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                            eMIDTextCode textCode;
                            if (this.UseDefaultCurve)
                            {
                                errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoDefaultSizeCurveForHeader), sizeCurveName, ap.HeaderID);
                                textCode = eMIDTextCode.msg_al_NoDefaultSizeCurveForHeader;
                            }
                            else
                            // End TT#438
                            {
                                errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader), sizeCurveName, ap.HeaderID);
                                textCode = eMIDTextCode.msg_al_NoSizeCurveForHeader;
                            }
							_audit.Add_Msg(
								eMIDMessageLevel.Severe,
                                //eMIDTextCode.msg_al_NoSizeCurveForHeader, // Begin TT#438
                                textCode,                                   // End TT#438
								errMessage,
								this.GetType().Name);
						}
					}
				}
			}
			catch (Exception ex)
			{
				validGenericSizeCurveName = false;
				_audit.Log_Exception(
					ex,
					this.GetType().Name,
					eExceptionLogging.logAllInnerExceptions);
			}
			return validGenericSizeCurveName;
		}
				
			//	switch(_genCurveMerchType)
			//	{
			//		case eMerchandiseType.Undefined:
			//			break;
			//		case eMerchandiseType.OTSPlanLevel:
			//			hnp = SAB.HierarchyServerSession.GetNodeData(ap.PlanHnRID);
			//			sizeCurveName = AddToSizeCurveName(sizeCurveName,hnp.NodeID);
			//			break;
			//		case eMerchandiseType.Node:
			//			hnp = SAB.HierarchyServerSession.GetNodeData(_genCurve.MdseHnRID);
			//			sizeCurveName = AddToSizeCurveName(sizeCurveName,hnp.NodeID);
			//			break;
			//		case eMerchandiseType.HierarchyLevel:
			//			hnp = SAB.HierarchyServerSession.GetAncestorDataByLevel(_genCurve.ProductHnLvlRID,ap.StyleHnRID,_genCurve.ProductHnLvlSeq);
			//			sizeCurveName = AddToSizeCurveName(sizeCurveName,hnp.NodeID);
			//			break;
			//	}
            //
			//	if (_genCurveColorInd)
			//	{
            //
			//		DataTable dtBulkColors = _header.GetBulkColors(ap.Key);
			//		if (dtBulkColors.Rows.Count == 1)
			//		{
			//			DataRow colorRow = dtBulkColors.Rows[0];
			//			int colorCodeRID = Convert.ToInt32(colorRow["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
			//			
			//			ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeRID);
			//			sizeCurveName = AddToSizeCurveName(sizeCurveName, ccp.ColorCodeID);
			//		}
			//		else if (dtBulkColors.Rows.Count == 0)
			//		{
			//			validSizeCurveName = false;
			//			errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeCurveColorForHeader),ap.HeaderID);
			//			throw new MIDException (eErrorLevel.severe,
			//				(int)eMIDTextCode.msg_al_NoSizeCurveColorForHeader,errMessage); 
			//		}
			//		else if (dtBulkColors.Rows.Count > 1)
			//		{
			//			validSizeCurveName = false;
			//			errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_MoreThanOneColorForHeader),ap.HeaderID);
			//			throw new MIDException (eErrorLevel.severe,
			//				(int)eMIDTextCode.msg_al_MoreThanOneColorForHeader,errMessage); 
			//		}
			//	}	
            //
			//	if (validSizeCurveName)
			//	{
			//		int sizeCurveGroupRID = _sizeCurveData.GetSizeCurveGroupKey(sizeCurveName);
			//		if (sizeCurveGroupRID != Include.NoRID)
			//		{ 
			//			if (!_genCurveHash.ContainsKey(ap.Key))
			//			{
			//				_genCurveHash.Add(ap.Key,sizeCurveGroupRID);
			//			}
			//		}
			//		else
			//		{
			//			validSizeCurveName = false;
//			 			errMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader),sizeCurveName,ap.HeaderID);
			//			errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeCurveForHeader),sizeCurveName,ap.HeaderID);
			//			throw new MIDException (eErrorLevel.severe,
			//				(int)eMIDTextCode.msg_al_NoSizeCurveForHeader,errMessage);
			//		}
			//	}
			//}	
			//catch  
			//{
			//	validSizeCurveName = false;
			//	throw;
			//}
			//return validSizeCurveName ;
			// end MID 4372 Generic Size Constraints
		//}
        //
		//private string AddToSizeCurveName(string aSizeCurveName, string addParm)
		//{
		//	try
		//	{
		//		if (aSizeCurveName == null)
		//		{
		//			aSizeCurveName = addParm;
		//		}
		//		else
		//		{
		//			aSizeCurveName += (_delimiter + addParm);
		//		}
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//	return aSizeCurveName;
		//}
		// end MID 4372 Generic Size Constraints
		#endregion GenericSizeCurve

        // begin MID Track 4372 Generic Size Constraint
		#region Generic Size Constraints

		virtual public bool GenericSizeConstraintsDefined()
		{
			bool genConstraintDefined = false;
			try
			{
				if (this.GenConstraintCharGroupRID != Include.NoRID ||
					this.GenConstraintMerchType != eMerchandiseType.Undefined ||
					this.GenConstraintColorInd)
				{
					genConstraintDefined = true;
				}
			}
			catch
			{
				genConstraintDefined = false;
				throw;
			}
			return  genConstraintDefined;
		}	
	
		private bool GenericSizeConstraintsFound(AllocationProfile ap)
		{
			bool genConstraintFound = true;
			try
			{
				if (!ValidGenericSizeConstraintsName(ap))
				{
					genConstraintFound = false;  
				}
			}
			catch
			{
				genConstraintFound = false;
				throw;
			}
			return  genConstraintFound;
		}
		public bool ValidGenericSizeConstraintsName(AllocationProfile ap)
		{
			bool validGenericSizeConstraintsName = true;
			string sizeConstraintsName = string.Empty;
			string errMessage;
			try
			{
				int genericConstraintsColorRID = Include.NoRID;
				if (_genConstraintColorInd)
				{
					if (ap.BulkColors.Count == 1)
					{
						foreach (int colorRID in ap.BulkColors.Keys)															
						{
							genericConstraintsColorRID = colorRID;
						}
					}
					else if (ap.BulkColors.Count == 0)
					{
						validGenericSizeConstraintsName = false;
						errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoGenericSizeColorForHeader),ap.HeaderID);
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_al_NoGenericSizeColorForHeader,errMessage); 

					}
					else
					{
						validGenericSizeConstraintsName = false;
						errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_MoreThanOneColorForHeader),ap.HeaderID);
						_audit.Add_Msg(
							eMIDMessageLevel.Severe,
							eMIDTextCode.msg_al_MoreThanOneColorForHeader,
							errMessage,
							this.GetType().Name);
					}
				}
				if (validGenericSizeConstraintsName)
				{
                    _genCurveNsccdCurveName = string.Empty;                       // TT#413 - RMatelic - add _genCurveNsccdCurveName: set to empty-not applicable to constraints
					sizeConstraintsName = 
						this._trans.GetGenericSizeName
						(ap,
						_genConstraintCharGroupRID,
						_genConstraintMerchType,
						_genConstraint,
                        genericConstraintsColorRID, _genCurveNsccdCurveName, _useDefaultCurve, false);     // TT#413 - RMatelic - add  _genCurveNsccdCurveName; TT#438 - add _useDefaultCurve & false
					if (sizeConstraintsName == null
						|| sizeConstraintsName == string.Empty)
					{
						validGenericSizeConstraintsName = false;
					}
					else
					{
						int sizeConstraintsGroupRID = _sizeModelData.GetSizeConstraintModelRID(sizeConstraintsName);
						if (sizeConstraintsGroupRID != Include.NoRID)
						{
                            // begin TT#1168 - MD - Jellis - Group ALlocation Null Reference when process Size Need
                            AllocationProfile[] apList;
                            // Begin TT#4988 - BVaughan - Performance
                            #if DEBUG
                            if ((ap is AssortmentProfile && !ap.isAssortmentProfile) || (!(ap is AssortmentProfile) && ap.isAssortmentProfile))
                            {
                                throw new Exception("Object does not match AssortmentProfile in ValidGenericSizeConstraintsName()");
                            }
                            #endif
                            // if (ap is AssortmentProfile)
                            if (ap.isAssortmentProfile)
                            // End TT#4988 - BVaughan - Performance
                            {
                                apList = new AllocationProfile[((AssortmentProfile)ap).AssortmentMembers.Length + 1];
                                apList[0] = ap;
                                ((AssortmentProfile)ap).AssortmentMembers.CopyTo(apList, 1);
                            }
                            else
                            {
                                apList = new AllocationProfile[1];
                                apList[0] = ap;
                            }
                            foreach (AllocationProfile allocationProfile in apList)
                            {
                                try
                                {
                                    _genConstraintHash.Remove(allocationProfile.Key);
                                    _genConstraintHash.Add(allocationProfile.Key, sizeConstraintsGroupRID);
                                }
                                finally
                                {
                                }
                            }
                            //try
                            //{
                            //    _genConstraintHash.Add(ap.Key,sizeConstraintsGroupRID);
                            //}
                            //catch (ArgumentException)
                            //{
                            //    _genConstraintHash.Remove(ap.Key);
                            //    _genConstraintHash.Add(ap.Key,sizeConstraintsGroupRID);
                            //}
                            // end TT#11168 - MD - Jellis - Group ALlocation Null Reference when process Size Need
						}
						else
						{
							validGenericSizeConstraintsName = false;
							errMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_NoSizeConstraintForHeader),sizeConstraintsName,ap.HeaderID);
							_audit.Add_Msg(
								eMIDMessageLevel.Severe,
								eMIDTextCode.msg_al_NoSizeConstraintForHeader,
								errMessage,
								this.GetType().Name);
						}
					}
				}
			}
			catch (Exception ex)
			{
				validGenericSizeConstraintsName = false;
				_audit.Log_Exception(
					ex,
					this.GetType().Name,
					eExceptionLogging.logAllInnerExceptions);
			}
			return validGenericSizeConstraintsName;
		}
		#endregion Generic Size Constraints
		// end MID Track 4372 Generic Size Constratint

		#region AllocationBaseMethod overrides
		public override void ProcessMethod(ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{

		}

		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aAllocationWorkFlowStep, 
			Profile aAllocationProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
				
		}
		#endregion


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
			AllocationSizeBaseMethod newAllocationSizeBaseMethod = null;

			try
			{
				newAllocationSizeBaseMethod = (AllocationSizeBaseMethod)this.MemberwiseClone();
				return newAllocationSizeBaseMethod;
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

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalSizeOverride);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserSizeOverride);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            throw new NotImplementedException("MethodGetData is not implemented");
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(ROOverrideLowLevel overrideLowLevel, out bool successful, ref string message)
        {
            successful = true;

            throw new NotImplementedException("MethodGetOverrideModelList is not implemented");
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
