using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for SizeMethodBase.
	/// </summary>
	public class SizeMethodBaseData : MethodBaseData
	{
		private DataSet _ChildDataDS;
		private int _genCurveHcgRID;				  // begin Generic Size Curve data
		private int _genCurveHnRID;
		private int _genCurvePhRID;
		private int _genCurvePhlSequence;
		private bool _genCurveColorInd;   
		private eMerchandiseType _genCurveMerchType;  // end Generic Size Curve data
		private int _genConstraintHcgRID;				   // begin Generic Size Constraint data
		private int _genConstraintHnRID;
		private int _genConstraintPhRID;
		private int _genConstraintPhlSequence;
		private bool _genConstraintColorInd;   
		private eMerchandiseType _genConstraintMerchType;  // end Generic Size Constraint data
        private eVSWSizeConstraints _vSWSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options  
		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		private bool _normalizeSizeCurvesDefaultIsOverridden;
		private bool _normalizeSizeCurves;
		// END MID Track #4826
        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        private bool _useDefaultCurve;
        // End TT#413
        private bool _overrideVSWSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        private bool _applyRulesOnly;
        // end TT#2155 - JEllis - Fill Size Holes Null Reference
        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        private int _genCurveNsccdRID;
        // End TT#413

		#region Public Properties
		/// <summary>
		/// Gets or sets the Generic Curve Header Characteristic Group RID.
		/// </summary>
		public int GenCurveHcgRID
		{
			get {return _genCurveHcgRID;}
			set {_genCurveHcgRID = value;}
		}
			
		/// <summary>
		/// Gets or sets the Generic Curve Hierarchy Node RID.
		/// </summary>
		public int GenCurveHnRID
		{
			get {return _genCurveHnRID;}
			set {_genCurveHnRID = value;}
		}

		/// <summary>
		/// Gets or sets the Generic Curve Product Hierarchy RID.
		/// </summary>
		public int GenCurvePhRID
		{
			get {return _genCurvePhRID;}
			set {_genCurvePhRID = value;}
		} 
        
		/// <summary>
		/// Gets or sets the Generic Curve Product Hierarchy Level sequence 
		/// </summary>
		public int GenCurvePhlSequence
		{
			get {return _genCurvePhlSequence;}
			set {_genCurvePhlSequence = value;}
		} 
		
		/// <summary>
		/// Gets or sets the Generic Curve Color Code bool
		/// </summary>
		public bool GenCurveColorInd
		{
			get {return _genCurveColorInd;}
			set {_genCurveColorInd = value;}
		} 

		/// <summary>
		/// Gets or sets the Generic Curve Merchandise Type 
		/// </summary>
		public eMerchandiseType GenCurveMerchType
		{
			get{return _genCurveMerchType;}
			set{_genCurveMerchType = value;}
		}
		 
		/// <summary>
		/// Gets or sets the Generic Constraint Header Characteristic Group RID.
		/// </summary>
		public int GenConstraintHcgRID
		{
			get {return _genConstraintHcgRID;}
			set {_genConstraintHcgRID = value;}
		}
			
		/// <summary>
		/// Gets or sets the Generic Constraint Hierarchy Node RID.
		/// </summary>
		public int GenConstraintHnRID
		{
			get {return _genConstraintHnRID;}
			set {_genConstraintHnRID = value;}
		}

		/// <summary>
		/// Gets or sets the Generic Constraint Product Hierarchy RID.
		/// </summary>
		public int GenConstraintPhRID
		{
			get {return _genConstraintPhRID;}
			set {_genConstraintPhRID = value;}
		} 
        
		/// <summary>
		/// Gets or sets the Generic Constraint Product Hierarchy Level sequence 
		/// </summary>
		public int GenConstraintPhlSequence
		{
			get {return _genConstraintPhlSequence;}
			set {_genConstraintPhlSequence = value;}
		} 
		
		/// <summary>
		/// Gets or sets the Generic Constraint Color Code bool
		/// </summary>
		public bool GenConstraintColorInd
		{
			get {return _genConstraintColorInd;}
			set {_genConstraintColorInd = value;}
		} 

		/// <summary>
		/// Gets or sets the Generic Constraint Merchandise Type 
		/// </summary>
		public eMerchandiseType GenConstraintMerchType
		{
			get{return _genConstraintMerchType;}
			set{_genConstraintMerchType = value;}
		}

        // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
        public eVSWSizeConstraints VSWSizeConstraints
        {
            get { return _vSWSizeConstraints; }
            set { _vSWSizeConstraints = value; }
        }
        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options

		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		public bool NormalizeSizeCurvesDefaultIsOverridden
		{
			get	{ return _normalizeSizeCurvesDefaultIsOverridden;}
			set	{ _normalizeSizeCurvesDefaultIsOverridden = value;	}
		}
		public bool NormalizeSizeCurves
		{
			get	{ return _normalizeSizeCurves;}
			set	{ _normalizeSizeCurves = value;	}
		}
		// END MID Track #4826

        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
        public bool UseDefaultCurve
        {
            get { return _useDefaultCurve; }
            set { _useDefaultCurve = value; }
        }
        // End TT#413

        // Begin TT#2155 - JEllis - Fill Size Holes Null Reference
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

        // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        public bool OverrideVSWSizeConstraints
        {
            get { return _overrideVSWSizeConstraints; }
            set { _overrideVSWSizeConstraints = value; }
        }
        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

		#endregion

		public SizeMethodBaseData()
		{
		}

		public SizeMethodBaseData(eMethodType aMethodType)
		{
			base.Method_Type_ID = aMethodType;
		}
	
		/// <summary>
		/// Deletes child data associated with the basis size method
		/// </summary>
		/// <param name="method_RID">ID of the method to delete</param>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <returns></returns>
		virtual public bool DeleteMethodRules(int method_RID, TransactionData td)
		{
			bool Successfull = true;
			try
			{

                StoredProcedures.SP_MID_MTH_RULE_DEL_CHILDREN.Delete(td.DBA, METHODRID: method_RID);

				Successfull = true;
			}
			catch(Exception e)
			{
				string exceptionMessage = e.Message;
				Successfull = false;
				//throw;
			}

			return Successfull;

		}


		/// <summary>
		/// Initializes the child data used in the datagrid for the Fill Size Holes Method.
		/// </summary>
		/// <param name="method_RID"></param>
		/// <returns></returns>
        // Begin TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
        //virtual public DataSet GetChildData(int method_RID, int sg_RID, bool getSizes)
        virtual public DataSet GetChildData(int method_RID, int sg_RID, bool getSizes, int SG_Version)
        // End TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
		{
			_ChildDataDS = MIDEnvironment.CreateDataSet();
			_ChildDataDS.EnforceConstraints = true;

			//GROUP LEVEL
            // Begin TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
            //_ChildDataDS.Tables.Add(SetupGroupLevelRuleTable(method_RID, sg_RID));
            _ChildDataDS.Tables.Add(SetupGroupLevelRuleTable(method_RID, sg_RID, SG_Version));
            // End TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error

            // Begin TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
			//ALL COLOR
//            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.AllColor));

//            //COLOR
//            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.Color));

////			if (getSizes)
////			{
//                //ALL COLOR SIZE
//                _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.AllColorSize));
//                //COLOR SIZE
//                _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.ColorSize));

//                //COLOR SIZE DIMENSION
//                _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.ColorSizeDimension));

//                //ALL COLOR SIZE DIMENSION
//                _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.AllColorSizeDimension));
////			}

            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.AllColor, SG_Version));

            //COLOR
            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.Color, SG_Version));

            //ALL COLOR SIZE
            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.AllColorSize, SG_Version));

            //COLOR SIZE
            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.ColorSize, SG_Version));

            //COLOR SIZE DIMENSION
            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.ColorSizeDimension, SG_Version));

            //ALL COLOR SIZE DIMENSION
            _ChildDataDS.Tables.Add(SetupRuleData(method_RID, sg_RID, eSizeMethodRowType.AllColorSizeDimension, SG_Version));
            // End TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message

			//CREATE THE RELATIONSHIPS FOR DATA HIERARCHY
			//BuildChildDataRelationships();
			CreatePrimaryKeys();
			CreateForeignKeys();
			CreateRelationships();

			//BuildChildDataRelationships(getSizes);

			//REMOVE RELATIONS IF SIZE IS NOT AVAILABLE
			if (!getSizes)
			{
				_ChildDataDS.Relations.Remove("ColorSize");
				_ChildDataDS.Relations.Remove("AllColorSize");
				_ChildDataDS.Relations.Remove("AllColorSizeDimension");
				_ChildDataDS.Relations.Remove("ColorSizeDimension");
			}


			return _ChildDataDS;

		}


		/// <summary>
		/// Creates primary keys on the size method constraint data.
		/// </summary>
		virtual public void CreatePrimaryKeys()
		{
			DataTable dt;

			#region SETLEVEL
				dt = (DataTable)_ChildDataDS.Tables["SetLevel"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"]};
			#endregion

			#region ALLCOLOR
                dt = (DataTable)_ChildDataDS.Tables["AllColor"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												dt.Columns["COLOR_CODE_RID"]};
			#endregion

			#region COLOR
                dt = (DataTable)_ChildDataDS.Tables["Color"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												dt.Columns["COLOR_CODE_RID"]};
			#endregion

			#region ALLCOLORSIZE
                dt = (DataTable)_ChildDataDS.Tables["AllColorSize"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												dt.Columns["COLOR_CODE_RID"],
												dt.Columns["DIMENSIONS_RID"],
												dt.Columns["SIZE_CODE_RID"]};
			#endregion

			#region COLORSIZE
                dt = (DataTable)_ChildDataDS.Tables["ColorSize"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												dt.Columns["COLOR_CODE_RID"],
												dt.Columns["DIMENSIONS_RID"],
												dt.Columns["SIZE_CODE_RID"]};
			#endregion

			#region COLORSIZEDIMENSION
                dt = (DataTable)_ChildDataDS.Tables["ColorSizeDimension"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												dt.Columns["COLOR_CODE_RID"],
												dt.Columns["DIMENSIONS_RID"]};
			#endregion

			#region ALLCOLORSIZEDIMENSION
                dt = (DataTable)_ChildDataDS.Tables["AllColorSizeDimension"];
				dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												dt.Columns["COLOR_CODE_RID"],
												dt.Columns["DIMENSIONS_RID"]};
			#endregion

		}


		/// <summary>
		/// Creates foreign keys on the size method constraint data.
		/// </summary>
		virtual public void CreateForeignKeys()
		{
			//CREATE CONSTRAINTS
			ForeignKeyConstraint fkc;

			#region SETALLCOLOR
				fkc = new ForeignKeyConstraint("SetAllColor", 
					new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
									}, 
					new DataColumn[] { _ChildDataDS.Tables["AllColor"].Columns["SGL_RID"] 
									});
				fkc.AcceptRejectRule = AcceptRejectRule.None;
				fkc.DeleteRule = Rule.Cascade;
				fkc.UpdateRule = Rule.Cascade;
				_ChildDataDS.Tables["AllColor"].Constraints.Add(fkc);
			#endregion

			#region SETCOLOR
				fkc = new ForeignKeyConstraint("SetColor", 
					new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
									}, 
					new DataColumn[] { _ChildDataDS.Tables["Color"].Columns["SGL_RID"]
									});
				fkc.AcceptRejectRule = AcceptRejectRule.None;
				fkc.DeleteRule = Rule.Cascade;
				fkc.UpdateRule = Rule.Cascade;
				_ChildDataDS.Tables["Color"].Constraints.Add(fkc);
			#endregion

			#region ALLCOLORSIZE
				fkc = new ForeignKeyConstraint("AllColorSize", 
					new DataColumn[] {	_ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
											_ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"],
											_ChildDataDS.Tables["AllColorSizeDimension"].Columns["DIMENSIONS_RID"]
										}, 
					new DataColumn[] {	_ChildDataDS.Tables["AllColorSize"].Columns["SGL_RID"],
											_ChildDataDS.Tables["AllColorSize"].Columns["COLOR_CODE_RID"],
											_ChildDataDS.Tables["AllColorSize"].Columns["DIMENSIONS_RID"]
										});
				fkc.AcceptRejectRule = AcceptRejectRule.None;
				fkc.DeleteRule = Rule.Cascade;
				fkc.UpdateRule = Rule.Cascade;
				_ChildDataDS.Tables["AllColorSize"].Constraints.Add(fkc);
			#endregion

			#region COLORSIZE
				fkc = new ForeignKeyConstraint("ColorSize", 
					new DataColumn[] {	_ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
											_ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"],
											_ChildDataDS.Tables["ColorSizeDimension"].Columns["DIMENSIONS_RID"]
										}, 
					new DataColumn[] {	_ChildDataDS.Tables["ColorSize"].Columns["SGL_RID"],
											_ChildDataDS.Tables["ColorSize"].Columns["COLOR_CODE_RID"],
											_ChildDataDS.Tables["ColorSize"].Columns["DIMENSIONS_RID"]
										});
				fkc.AcceptRejectRule = AcceptRejectRule.None;
				fkc.DeleteRule = Rule.Cascade;
				fkc.UpdateRule = Rule.Cascade;
				_ChildDataDS.Tables["ColorSize"].Constraints.Add(fkc);
			#endregion

			#region COLORSIZEDIMENSION
				fkc = new ForeignKeyConstraint("ColorSizeDimension", 
					new DataColumn[] {	_ChildDataDS.Tables["Color"].Columns["SGL_RID"],
											_ChildDataDS.Tables["Color"].Columns["COLOR_CODE_RID"]
										}, 
					new DataColumn[] {	_ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
											_ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"]
										});
				fkc.AcceptRejectRule = AcceptRejectRule.None;
				fkc.DeleteRule = Rule.Cascade;
				fkc.UpdateRule = Rule.Cascade;
				_ChildDataDS.Tables["ColorSizeDimension"].Constraints.Add(fkc);
			#endregion

			#region ALLCOLORSIZEDIMENSION
				fkc = new ForeignKeyConstraint("AllColorSizeDimension", 
					new DataColumn[] {	_ChildDataDS.Tables["AllColor"].Columns["SGL_RID"],
											_ChildDataDS.Tables["AllColor"].Columns["COLOR_CODE_RID"]
										}, 
					new DataColumn[] {	_ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
											_ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"]
										});
				fkc.AcceptRejectRule = AcceptRejectRule.None;
				fkc.DeleteRule = Rule.Cascade;
				fkc.UpdateRule = Rule.Cascade;
				_ChildDataDS.Tables["AllColorSizeDimension"].Constraints.Add(fkc);
			#endregion

		}


		/// <summary>
		/// Creates relationships on the size method constraint data.
		/// </summary>
		virtual public void CreateRelationships()
		{

			//BUILD RELATIONS
			#region SETALLCOLOR
			_ChildDataDS.Relations.Add(new DataRelation("SetAllColor",
				new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _ChildDataDS.Tables["AllColor"].Columns["SGL_RID"]
								 }));
			#endregion

			#region SETCOLOR
			_ChildDataDS.Relations.Add(new DataRelation("SetColor",
				new DataColumn[] { _ChildDataDS.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _ChildDataDS.Tables["Color"].Columns["SGL_RID"]
								 }));
			#endregion

			#region ALLCOLORSIZEDIMENSION
			_ChildDataDS.Relations.Add(new DataRelation( "AllColorSizeDimension", 
				new DataColumn[] { _ChildDataDS.Tables["AllColor"].Columns["SGL_RID"], 
										_ChildDataDS.Tables["AllColor"].Columns["COLOR_CODE_RID"] 
									},
				new DataColumn[] { _ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"], 
										_ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"] 
									}));
			#endregion

			#region COLORSIZEDIMENSION
			_ChildDataDS.Relations.Add(new DataRelation( "ColorSizeDimension", 
				new DataColumn[] { _ChildDataDS.Tables["Color"].Columns["SGL_RID"],
										_ChildDataDS.Tables["Color"].Columns["COLOR_CODE_RID"] 
									},
				new DataColumn[] { _ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
										_ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"] 
									}));
			#endregion

			#region ALLCOLORSIZE
			_ChildDataDS.Relations.Add(new DataRelation("AllColorSize", 
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
										_ChildDataDS.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"],
										_ChildDataDS.Tables["AllColorSizeDimension"].Columns["DIMENSIONS_RID"] 
									},
				new DataColumn[] {	_ChildDataDS.Tables["AllColorSize"].Columns["SGL_RID"],
										_ChildDataDS.Tables["AllColorSize"].Columns["COLOR_CODE_RID"],
										_ChildDataDS.Tables["AllColorSize"].Columns["DIMENSIONS_RID"] 
									}));
			#endregion

			#region COLORSIZE
			_ChildDataDS.Relations.Add(new DataRelation("ColorSize", 
				new DataColumn[] {	_ChildDataDS.Tables["ColorSizeDimension"].Columns["SGL_RID"],
										_ChildDataDS.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"],
										_ChildDataDS.Tables["ColorSizeDimension"].Columns["DIMENSIONS_RID"] 
									},
				new DataColumn[] {  _ChildDataDS.Tables["ColorSize"].Columns["SGL_RID"],
										_ChildDataDS.Tables["ColorSize"].Columns["COLOR_CODE_RID"],
										_ChildDataDS.Tables["ColorSize"].Columns["DIMENSIONS_RID"] 
									}));
			#endregion

		}


		/// <summary>
		/// Gets a datatable from SIZE_MTH_CONST_MINMAX
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sg_RID"></param>
		/// <param name="RowTypeId"></param>
		/// <returns></returns>
        // Begin TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
        //virtual public DataTable SetupRuleData(int method_RID, int sg_RID, eSizeMethodRowType RowTypeId)
        virtual public DataTable SetupRuleData(int method_RID, int sg_RID, eSizeMethodRowType RowTypeId, int sg_Version)
        // End TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
		{
			string dtTableName = "";

			switch (RowTypeId)
			{
				case eSizeMethodRowType.AllColor:
					dtTableName = "AllColor";
					break;
				case eSizeMethodRowType.Color:
					dtTableName = "Color";
					break;
				case eSizeMethodRowType.AllColorSize:
					dtTableName = "AllColorSize";
					break;
				case eSizeMethodRowType.AllColorSizeDimension:
					dtTableName = "AllColorSizeDimension";
					break;
				case eSizeMethodRowType.ColorSize:
					dtTableName = "ColorSize";
					break;
				case eSizeMethodRowType.ColorSizeDimension:
					dtTableName = "ColorSizeDimension";
					break;
			}

			try
			{
				DataColumn column;
				DataTable dtMinmax = MIDEnvironment.CreateDataTable();
                dtMinmax = StoredProcedures.SP_MID_MTH_RULE_GET_COLOR_SZ.Read(_dba,
                                                                            METHODRID: method_RID,
                                                                            ROWTYPEID: (int)RowTypeId,
                                                                            SGRID: sg_RID,
                                                                            SG_VERSION: sg_Version  // TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
                                                                            );
                dtMinmax.TableName = dtTableName;
				column = dtMinmax.Columns["ROW_TYPE_ID"];
				column.DefaultValue = RowTypeId;
					
				switch (RowTypeId)
				{
					case eSizeMethodRowType.AllColor:
						column = dtMinmax.Columns["BAND_DSC"];
						column.ReadOnly = true;
						break;
				}

				return dtMinmax;
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Gets a datatable from SIZE_MTH_CONST_GRPLVL
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sg_RID"></param>
		/// <returns></returns>
        // Begin TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
        //virtual public DataTable SetupGroupLevelRuleTable(int method_RID, int sg_RID)
        virtual public DataTable SetupGroupLevelRuleTable(int method_RID, int sg_RID, int sg_Version)
        // End TT#1892-MD - JSmith - Versioning Test - Select Size Need Method receive SystemArgumentException Error
		{
			try
			{
				DataColumn column;
				DataTable dtSetLevel = MIDEnvironment.CreateDataTable();
                dtSetLevel = StoredProcedures.SP_MID_MTH_RULE_GET_GRPLVL.Read(_dba,
                                                                              METHODRID: method_RID,
                                                                              SGRID: sg_RID,
                                                                              SG_VERSION: sg_Version
                                                                              );
                dtSetLevel.TableName = "SetLevel";
				//Add columns to DataTable because Oracle does not support bit/boolean
				//data types.  Data will be retrieved as Char and be converted below.
				//=====================================================================
//				column = dtSetLevel.Columns.Add("FSH_IND", System.Type.GetType("System.Boolean"));
//				column.DefaultValue = true;
//
//				column = dtSetLevel.Columns.Add("FZ_IND", System.Type.GetType("System.Boolean"));
//				column.DefaultValue = false;
				//=====================================================================

				column = dtSetLevel.Columns["BAND_DSC"];
				column.ReadOnly = true;

				//Convert the char columns to boolean, set the default fill sequence.
//				foreach(DataRow dr in dtSetLevel.Rows)
//				{
//					dr["FSH_IND"] = Include.ConvertCharToBool(Convert.ToChar(dr["FILL_SIZE_HOLES_IND"]));
//					dr["FZ_IND"] = Include.ConvertCharToBool(Convert.ToChar(dr["FILL_ZEROS_IND"]));
//
//					switch (base.Method_Type_ID)
//					{
//						case eMethodType.BasisSizeAllocation:
//							dr["FILL_SEQUENCE"] = (dr["FILL_SEQUENCE"] != System.DBNull.Value) ? dr["FILL_SEQUENCE"] : (int)eBasisSizeSort.Descending;
//							break;
//						case eMethodType.FillSizeHolesAllocation:
//							dr["FILL_SEQUENCE"] = (dr["FILL_SEQUENCE"] != System.DBNull.Value) ? dr["FILL_SEQUENCE"] : (int)eFillSizeHolesSort.Ascending;
//							break;
//					}
//				}

				//COLUMNS THAT GET CONVERTED FROM CHAR TO BOOLEAN ARE NO LONGER NEEDED, TAKE 
				//THEM OFF THE TABLE.
//				dtSetLevel.Columns.Remove("FILL_SIZE_HOLES_IND");
//				dtSetLevel.Columns.Remove("FILL_ZEROS_IND");

				dtSetLevel.AcceptChanges();

				return dtSetLevel;
			}
			catch 
			{
				throw;
			}

		}
		
		/// <summary>
        /// Executes SP_MID_MTH_RULE_CS_INS to insert constraint data.
		/// </summary>
		/// <param name="td"></param>
		/// <param name="inParams"></param>
		/// <returns></returns>
        virtual public bool ProcessRules(TransactionData td, int METHODRID, int SGLRID, int COLORCODERID, int SIZESRID, int SIZECODERID, int? RULE, int? QTY, int ROWTYPEID, int DIMENSIONS_RID)
		{
			bool Successfull = true;

			try
			{
                StoredProcedures.SP_MID_MTH_RULE_CS_INS.Insert(td.DBA,
                                                               METHODRID: METHODRID,
                                                               SGLRID: SGLRID,
                                                               COLORCODERID: COLORCODERID,
                                                               SIZESRID: SIZESRID,
                                                               SIZECODERID: SIZECODERID,
                                                               RULE: RULE,
                                                               QTY: QTY,
                                                               ROWTYPEID: ROWTYPEID,
                                                               DIMENSIONS_RID: DIMENSIONS_RID
                                                               );
		
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
        /// Executes SP_MID_MTH_RULE_GL_INS to insert constraint data.
		/// </summary>
		/// <param name="td"></param>
		/// <param name="inParams"></param>
		/// <returns></returns>
        virtual public bool ProcessGroupLevel(TransactionData td, int METHODRID, int SGLRID, int? RULE, int? QTY, int ROWTYPEID)
		{
			bool Successfull = true;

			try
			{
                StoredProcedures.SP_MID_MTH_RULE_GL_INS.Insert(td.DBA,
                                                               METHODRID: METHODRID,
                                                               SGLRID: SGLRID,
                                                               RULE: RULE,
                                                               QTY: QTY,
                                                               ROWTYPEID: ROWTYPEID
                                                               );
			
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


	}
}
