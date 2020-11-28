using System;
using System.Data;
using System.Text;
using System.Globalization;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for MethodBasisSize.
	/// </summary>
	public class MethodBasisSizeData : SizeMethodBaseData
	{
		#region "Member Variables"
		
		private int _SizeGroupRid;
		private int _SizeCurveGroupRid;
		private int _StoreFilterRid;
		private int _BasisHdrRid;
		private int _BasisClrRid;
		private int _Rule;
		private int _quantity;
		private int _HeaderComponent;
		private int _sizeConstraintRid;
		private ArrayList _substituteList;  //list of BasisSizeSubstitute values
        private bool _includeReserveInd;
		#endregion

		#region "Public Properties"


		public int HeaderComponent
		{
			get {return _HeaderComponent;}
			set {_HeaderComponent = value;}
		}


		public int SizeGroupRid
		{
			get{return _SizeGroupRid;}
			set{_SizeGroupRid = value;}
		}

		public int SizeCurveGroupRid
		{
			get{return _SizeCurveGroupRid;}
			set{_SizeCurveGroupRid = value;}
		}

		public int StoreFilterRid
		{
			get{return _StoreFilterRid;}
			set{_StoreFilterRid = value;}
		}

		public int BasisHdrRid
		{
			get {return _BasisHdrRid;}
			set {_BasisHdrRid = value;}
		}


		public int BasisClrRid
		{
			get {return _BasisClrRid;}
			set {_BasisClrRid = value;}
		}


		public int Rule
		{
			get {return _Rule;}
			set {_Rule = value;}
		}


		public int RuleQuantity
		{
			get {return _quantity;}
			set {_quantity = value;}
		}

		/// <summary>
		/// Gets or sets the Size Constraint RID.
		/// </summary>
		public int SizeConstraintRid
		{
			get {return _sizeConstraintRid;}
			set {_sizeConstraintRid = value;}
		}

		public ArrayList SubstituteList
		{
			get {return _substituteList;}
			set {_substituteList = value;}
		}

        public bool IncludeReserveInd
        {
            get { return _includeReserveInd; }
            set { _includeReserveInd = value; }
        }

 
		#endregion


		#region "Constructors"

		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		public MethodBasisSizeData() : base(eMethodType.BasisSizeAllocation)
		{
			_substituteList = new ArrayList();
		}


		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		public MethodBasisSizeData(TransactionData td) : base(eMethodType.BasisSizeAllocation)
		{
			_dba = td.DBA;
			_substituteList = new ArrayList();
		}

		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="method_RID">The record ID of the method</param>
		public MethodBasisSizeData(TransactionData td, int method_RID) : base(eMethodType.BasisSizeAllocation)
		{
			_dba = td.DBA;
			_substituteList = new ArrayList();
		}

		/// <summary>
		/// Creates an instance of the MethodBasisSizeData class.
		/// </summary>
		/// <param name="method_RID">ID of the method to retrieve</param>
		/// <param name="changeType"></param>
		public MethodBasisSizeData(int method_RID, eChangeType changeType) : base(eMethodType.BasisSizeAllocation)
		{
			_substituteList = new ArrayList();
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateSizeBasis(method_RID);
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
		public bool PopulateSizeBasis(int method_RID)
		{
			try
			{
				if (PopulateMethod(method_RID))
				{
                    //MID Track # 2354 - removed nolock because it causes concurrency issues
					DataTable dtSizeBasis = MIDEnvironment.CreateDataTable();
                    dtSizeBasis = StoredProcedures.MID_METHOD_SIZE_BASIS_ALLOCATION_READ.Read(_dba, METHOD_RID: method_RID);
						
					if(dtSizeBasis.Rows.Count != 0)
					{
						DataRow dr = dtSizeBasis.Rows[0];
									
						_Rule = Include.UndefinedRule;
						if (dr["SIZE_BASIS_RULE"] != System.DBNull.Value)
						{
							_Rule = Convert.ToInt32(dr["SIZE_BASIS_RULE"].ToString(),CultureInfo.CurrentUICulture);
						}

						_quantity = 0;
						if (dr["RULE_QUANTITY"] != System.DBNull.Value)
						{
							_quantity = Convert.ToInt32(dr["RULE_QUANTITY"].ToString(),CultureInfo.CurrentUICulture);
						}

						_BasisHdrRid = Include.NoRID;
						if (dr["SIZE_BASIS_HDR_RID"] != System.DBNull.Value)
						{
							_BasisHdrRid = Convert.ToInt32(dr["SIZE_BASIS_HDR_RID"].ToString(),CultureInfo.CurrentUICulture);
						}

						_BasisClrRid = Include.NoRID;
						if (dr["COLOR_CODE_RID"] != System.DBNull.Value)
						{
							_BasisClrRid = Convert.ToInt32(dr["COLOR_CODE_RID"].ToString(),CultureInfo.CurrentUICulture);
						}

						_SizeGroupRid = Include.NoRID;
						if (dr["SIZE_GROUP_RID"] != System.DBNull.Value)
						{
							_SizeGroupRid = Convert.ToInt32(dr["SIZE_GROUP_RID"].ToString(),CultureInfo.CurrentUICulture);
						}

						_SizeCurveGroupRid = Include.NoRID;
						if (dr["SIZE_CURVE_GROUP_RID"] != System.DBNull.Value)
						{
							_SizeCurveGroupRid = Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"].ToString(),CultureInfo.CurrentUICulture);
						}

						_StoreFilterRid = Include.NoRID;
						if (dr["STORE_FILTER_RID"] != System.DBNull.Value)
						{
							_StoreFilterRid = Convert.ToInt32(dr["STORE_FILTER_RID"],CultureInfo.CurrentUICulture);
						}

						_HeaderComponent = Include.NoRID;
						if (dr["HEADER_COMPONENT"] != System.DBNull.Value)
						{
							_HeaderComponent = Convert.ToInt32(dr["HEADER_COMPONENT"], CultureInfo.CurrentUICulture);
						}

						_quantity = Include.UndefinedQuantity;
						if (dr["RULE_QUANTITY"] != System.DBNull.Value)
						{
							_quantity = Convert.ToInt32(dr["RULE_QUANTITY"], CultureInfo.CurrentUICulture);
						}

						// BEGIN MID ISSUE # 3045 stodd
						_sizeConstraintRid = Include.NoRID;
						// END MID ISSUE # 3045 stodd
						if (dr["SIZE_CONSTRAINT_RID"] != System.DBNull.Value)
						{
							this._sizeConstraintRid = Convert.ToInt32(dr["SIZE_CONSTRAINT_RID"].ToString(),CultureInfo.CurrentUICulture);
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

                        if (dr["INCLUDE_RESERVE_IND"] != System.DBNull.Value)
                        {
                            IncludeReserveInd = Include.ConvertCharToBool(Convert.ToChar(dr["INCLUDE_RESERVE_IND"], CultureInfo.CurrentUICulture));
                        }


						PopulateSubstitutes(method_RID);

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
            
            int? RULE_Nullable = null;
            if (_Rule != Include.UndefinedRule) RULE_Nullable = _Rule;

            int? STOREFILTERRID_Nullable = null;
            if (_StoreFilterRid != Include.NoRID) STOREFILTERRID_Nullable = _StoreFilterRid;

            int? SIZEGROUPRID_Nullable = null;
            if (_SizeGroupRid != Include.NoRID) SIZEGROUPRID_Nullable = _SizeGroupRid;

            int? HEADERCOMPONENT_Nullable = null;
            if (_HeaderComponent != Include.NoRID) HEADERCOMPONENT_Nullable = _HeaderComponent;

            int? RULEQUANTITY_Nullable = null;
            if (_quantity != Include.UndefinedQuantity) RULEQUANTITY_Nullable = _quantity;

            int? SIZECONSTRAINTRID_Nullable = null;
            if (_sizeConstraintRid != Include.NoRID) SIZECONSTRAINTRID_Nullable = _sizeConstraintRid;

            int? SIZECURVEGROUPRID_Nullable = null;
            if (_SizeCurveGroupRid != Include.NoRID) SIZECURVEGROUPRID_Nullable = _SizeCurveGroupRid;

            int? GENCURVE_HCG_RID_Nullable = null;
            if (this.GenCurveHcgRID != Include.NoRID) GENCURVE_HCG_RID_Nullable = this.GenCurveHcgRID;

            int? GENCURVE_HN_RID_Nullable = null;
            if (this.GenCurveHnRID != Include.NoRID) GENCURVE_HN_RID_Nullable = this.GenCurveHnRID;

            int? GENCURVE_PH_RID_Nullable = null;
            if (this.GenCurvePhRID != Include.NoRID) GENCURVE_PH_RID_Nullable = this.GenCurvePhRID;

            int? GENCURVE_PHL_SEQUENCE_Nullable = null;
            if (this.GenCurvePhRID != Include.NoRID) GENCURVE_PHL_SEQUENCE_Nullable = this.GenCurvePhlSequence;

            char? GENCURVE_COLOR_IND_Nullable = null;
            if (this.GenCurveColorInd) GENCURVE_COLOR_IND_Nullable = Include.ConvertBoolToChar(this.GenCurveColorInd);

            int? GENCURVE_MERCH_TYPE_Nullable = null;
            if (this.GenCurveMerchType != eMerchandiseType.Undefined) GENCURVE_MERCH_TYPE_Nullable = (int)this.GenCurveMerchType;

            int? GENCONSTRAINT_HCG_RID_Nullable = null;
            if (this.GenConstraintHcgRID != Include.NoRID) GENCONSTRAINT_HCG_RID_Nullable = this.GenConstraintHcgRID;

            int? GENCONSTRAINT_HN_RID_Nullable = null;
            if (this.GenConstraintHnRID != Include.NoRID) GENCONSTRAINT_HN_RID_Nullable = this.GenConstraintHnRID;

            int? GENCONSTRAINT_PH_RID_Nullable = null;
            if (this.GenConstraintPhRID != Include.NoRID) GENCONSTRAINT_PH_RID_Nullable = this.GenConstraintPhRID;

            int? GENCONSTRAINT_PHL_SEQUENCE_Nullable = null;
            if (this.GenConstraintPhRID != Include.NoRID) GENCONSTRAINT_PHL_SEQUENCE_Nullable = this.GenConstraintPhlSequence;

            char? GENCONSTRAINT_COLOR_IND_Nullable = null;
            if (this.GenConstraintColorInd) GENCONSTRAINT_COLOR_IND_Nullable = Include.ConvertBoolToChar(this.GenConstraintColorInd);

            int? GENCONSTRAINT_MERCH_TYPE_Nullable = null;
            if (this.GenConstraintMerchType != eMerchandiseType.Undefined) GENCONSTRAINT_MERCH_TYPE_Nullable = (int)this.GenConstraintMerchType;

            int? GENCURVE_NSCCD_RID_Nullable = null;
            if (this.GenCurveNsccdRID != Include.NoRID) GENCURVE_NSCCD_RID_Nullable = this.GenCurveNsccdRID;

            StoredProcedures.SP_MID_MTH_SIZE_BASIS_UPD_INS.Update(td.DBA,
                                                              METHODRID: method_RID,
                                                              BASISHDRRID: _BasisHdrRid,
                                                              BASISCLRRID: _BasisClrRid,
                                                              RULE: RULE_Nullable,
                                                              STOREFILTERRID: STOREFILTERRID_Nullable,
                                                              SIZEGROUPRID: SIZEGROUPRID_Nullable,
                                                              HEADERCOMPONENT: HEADERCOMPONENT_Nullable,
                                                              RULEQUANTITY: RULEQUANTITY_Nullable,
                                                              SIZECONSTRAINTRID: SIZECONSTRAINTRID_Nullable,
                                                              SIZECURVEGROUPRID: SIZECURVEGROUPRID_Nullable,
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
                                                              USE_DEFAULT_CURVE_IND: Include.ConvertBoolToChar(UseDefaultCurve),
                                                              GENCURVE_NSCCD_RID: GENCURVE_NSCCD_RID_Nullable,
                                                              APPLY_RULES_ONLY_IND: Include.ConvertBoolToChar(ApplyRulesOnly),
                                                              INCLUDE_RESERVE_IND: Include.ConvertBoolToChar(IncludeReserveInd)
                                                              );
		}


		public void PopulateSubstitutes(int method_RID)
		{
			try
			{
				_substituteList.Clear();
               
				DataTable dtSizeSub = MIDEnvironment.CreateDataTable();
                dtSizeSub = StoredProcedures.MID_METHOD_SIZE_BASIS_SUBSTITUTES_READ.Read(_dba, METHOD_RID: method_RID);
						
				if(dtSizeSub.Rows.Count != 0)
				{
					foreach (DataRow aRow in dtSizeSub.Rows)
					{
						BasisSizeSubstitute aSizeSub = new BasisSizeSubstitute(); 
						if (aRow["SIZE_TYPE_RID"] != System.DBNull.Value)
						{
							aSizeSub.SizeTypeRid = Convert.ToInt32(aRow["SIZE_TYPE_RID"].ToString(),CultureInfo.CurrentUICulture);
						}
						if (aRow["SUBSTITUTE_RID"] != System.DBNull.Value)
						{
							aSizeSub.SubstituteRid = Convert.ToInt32(aRow["SUBSTITUTE_RID"].ToString(),CultureInfo.CurrentUICulture);
						}
						if (aRow["SIZE_TYPE"] != System.DBNull.Value)
						{
							aSizeSub.SizeType = (eEquateOverrideSizeType)Convert.ToInt32(aRow["SIZE_TYPE"].ToString(),CultureInfo.CurrentUICulture);
						}
						_substituteList.Add(aSizeSub);
					}
				}

			}
			catch
			{
				throw;
			}
		}

		private void PerformInsertSubstitutes(int method_RID, TransactionData td)
		{
			try
			{
				DeleteSubstitutes(method_RID, td);
				foreach (BasisSizeSubstitute aSizeSub in _substituteList)
				{
					if (aSizeSub.SubstituteRid != Include.NoRID)
					{
                       
                        StoredProcedures.MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT.Insert(td.DBA,
                                                                                 METHOD_RID: method_RID,
                                                                                 SIZE_TYPE_RID: aSizeSub.SizeTypeRid,
                                                                                 SUBSTITUTE_RID: aSizeSub.SubstituteRid,
                                                                                 SIZE_TYPE: (int)aSizeSub.SizeType
                                                                                 );
					}
				}
			}
			catch
			{	
				throw;
			}
		}

		private void DeleteSubstitutes(int method_RID, TransactionData td)
		{
			try
			{
                StoredProcedures.MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE.Delete(td.DBA, METHOD_RID: method_RID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}


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
				PerformInsertSubstitutes(method_RID, td);
	
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
				// substitutes are always deleted and then inserted...
				PerformInsertSubstitutes(method_RID, td);

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
                StoredProcedures.SP_MID_MTH_SIZE_BASIS_DEL.Delete(td.DBA, METHODRID: method_RID);
		
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
