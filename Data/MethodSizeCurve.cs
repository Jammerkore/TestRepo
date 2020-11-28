using System;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    /// <summary>
    /// Summary description for MethodSizeCurve.
    /// </summary>
    public class MethodSizeCurveData : SizeMethodBaseData
    {

        #region "Member Variables"

        private int _sizeGroupRID;
		//Begin TT#1076 - JScott - Size Curves by Set
		private eSizeCurvesByType _sizeCurvesByType;
		private int _sizeCurvesBySGRID;
		//End TT#1076 - JScott - Size Curves by Set
		private bool _merchBasisEqualizeWeight;
		private bool _curveBasisEqualizeWeight;
		private bool _applyLostSales;
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private double _tolerMinAvgPerSize;
        private double _tolerSalesTolerance;
		private eNodeChainSalesType _tolerIndexUnitsType;
        private double _tolerMinTolerancePct;
        private double _tolerMaxTolerancePct;
		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private DataTable _dtMerchBasisDetail;
		private DataTable _dtCurveBasisDetail;
        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        private bool _applyMinToZeroTolerance;
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue

        #endregion

        #region "Public Properties"

        public int SizeGroupRID
        {
            get { return _sizeGroupRID; }
            set { _sizeGroupRID = value; }
        }

		//Begin TT#1076 - JScott - Size Curves by Set
		public eSizeCurvesByType SizeCurvesByType
		{
			get { return _sizeCurvesByType; }
			set { _sizeCurvesByType = value; }
		}

		public int SizeCurvesBySGRID
		{
			get { return _sizeCurvesBySGRID; }
			set { _sizeCurvesBySGRID = value; }
		}

		//End TT#1076 - JScott - Size Curves by Set
		public bool MerchBasisEqualizeWeight
        {
			get { return _merchBasisEqualizeWeight; }
			set { _merchBasisEqualizeWeight = value; }
        }

		public bool CurveBasisEqualizeWeight
        {
			get { return _curveBasisEqualizeWeight; }
			set { _curveBasisEqualizeWeight = value; }
        }

		public bool ApplyLostSales
		{
			get { return _applyLostSales; }
			set { _applyLostSales = value; }
		}

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		public double TolerMinAvgPerSize
		{
			get { return _tolerMinAvgPerSize; }
			set { _tolerMinAvgPerSize = value; }
		}

        public double TolerSalesTolerance
		{
			get { return _tolerSalesTolerance; }
			set { _tolerSalesTolerance = value; }
		}

		public eNodeChainSalesType TolerIndexUnitsType
		{
			get { return _tolerIndexUnitsType; }
			set { _tolerIndexUnitsType = value; }
		}

        public double TolerMinTolerancePct
		{
			get { return _tolerMinTolerancePct; }
			set { _tolerMinTolerancePct = value; }
		}

        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        public bool ApplyMinToZeroTolerance
        {
            get { return _applyMinToZeroTolerance; }
            set { _applyMinToZeroTolerance = value; }
        }
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue

        public double TolerMaxTolerancePct
		{
			get { return _tolerMaxTolerancePct; }
			set { _tolerMaxTolerancePct = value; }
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		public DataTable DTMerchBasisDetail
		{
			get { return _dtMerchBasisDetail; }
			set { _dtMerchBasisDetail = value; }
		}

		public DataTable DTCurveBasisDetail
		{
			get { return _dtCurveBasisDetail; }
			set { _dtCurveBasisDetail = value; }
		}

		#endregion

        #region "Constructors"

        /// <summary>
        /// Creates an instance of the MethodBasisSizeData class.
        /// </summary>
        public MethodSizeCurveData()
            : base(eMethodType.SizeCurve)
        {
            //
            // TODO: Add constructor logic here
            //
        }


        /// <summary>
        /// Creates an instance of the MethodBasisSizeData class.
        /// </summary>
        /// <param name="td">An instance of the TransactionData class containing the database connection.</param>
        public MethodSizeCurveData(TransactionData td)
            : base(eMethodType.SizeCurve)
        {
            _dba = td.DBA;
        }

        /// <summary>
        /// Creates an instance of the MethodBasisSizeData class.
        /// </summary>
        /// <param name="td">An instance of the TransactionData class containing the database connection.</param>
        /// <param name="method_RID">The record ID of the method</param>
        public MethodSizeCurveData(TransactionData td, int method_RID)
            : base(eMethodType.SizeCurve)
        {
            _dba = td.DBA;
        }

        /// <summary>
        /// Creates an instance of the MethodBasisSizeData class.
        /// </summary>
        /// <param name="method_RID">ID of the method to retrieve</param>
        /// <param name="changeType"></param>
        public MethodSizeCurveData(int method_RID, eChangeType changeType)
            : base(eMethodType.SizeCurve)
        {
            switch (changeType)
            {
                case eChangeType.populate:
                    PopulateSizeCurve(method_RID);
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
        public bool PopulateSizeCurve(int aMethodRID)
        {
            try
            {
                if (PopulateMethod(aMethodRID))
                {


                    DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
                    dtSizeCurve = StoredProcedures.MID_METHOD_SIZE_CURVE_READ.Read(_dba,
                                                                        METHOD_RID: aMethodRID,
                                                                        INDEX_UNITS_TYPE: (int)eNodeChainSalesType.None
                                                                        );
                    if (dtSizeCurve.Rows.Count != 0)
                    {
                        DataRow dr = dtSizeCurve.Rows[0];

                        _sizeGroupRID = Convert.ToInt32(dr["SIZE_GROUP_RID"]);
						//Begin TT#1076 - JScott - Size Curves by Set
						_sizeCurvesByType = (eSizeCurvesByType)Convert.ToInt32(dr["SIZE_CURVES_BY_TYPE"], CultureInfo.CurrentUICulture);
						_sizeCurvesBySGRID = Convert.ToInt32(dr["SIZE_CURVES_BY_SG_RID"], CultureInfo.CurrentUICulture);
						//End TT#1076 - JScott - Size Curves by Set
                        //Begin TT#5438 - AGallagher - Equalize Weighing button on Size Curve Method not staying stable
						//_merchBasisEqualizeWeight = Convert.ToBoolean(dr["MERCH_BASIS_EQUAL_WEIGHT_IND"]);
                        _merchBasisEqualizeWeight = Include.ConvertCharToBool(Convert.ToChar(dr["MERCH_BASIS_EQUAL_WEIGHT_IND"], CultureInfo.CurrentUICulture));
                        //End TT#5438 - AGallagher - Equalize Weighing button on Size Curve Method not staying stable
						_curveBasisEqualizeWeight = Convert.ToBoolean(dr["CURVE_BASIS_EQUAL_WEIGHT_IND"]);
						_applyLostSales = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_LOST_SALES_IND"], CultureInfo.CurrentUICulture));
						//Begin TT#155 - JScott - Add Size Curve info to Node Properties
                        _tolerMinAvgPerSize = Convert.ToDouble(dr["MINIMUM_AVERAGE"]);
						_tolerSalesTolerance = Convert.ToDouble(dr["SALES_TOLERANCE"]);
						_tolerIndexUnitsType = (eNodeChainSalesType)Convert.ToInt32(dr["INDEX_UNITS_TYPE"], CultureInfo.CurrentUICulture);
                        _tolerMinTolerancePct = Convert.ToDouble(dr["MIN_TOLERANCE"]);
                        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                        //Begin TT#4866 - AGallagher - Size Curve Method -  Apply Minimun to Zero Tolarance Check Box- when saved as Blank and Re open method the box is checked.
                        //_applyMinToZeroTolerance = Include.ConvertCharToBool(Convert.ToChar(dr["APPLY_MIN_TO_ZERO_TOLERANCE_IND"], CultureInfo.CurrentUICulture));
                        _applyMinToZeroTolerance = Convert.ToBoolean(dr["APPLY_MIN_TO_ZERO_TOLERANCE_IND"]);
                        //End TT#4866 - AGallagher - Size Curve Method -  Apply Minimun to Zero Tolarance Check Box- when saved as Blank and Re open method the box is checked.
                        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                        _tolerMaxTolerancePct = Convert.ToDouble(dr["MAX_TOLERANCE"]);
						//End TT#155 - JScott - Add Size Curve info to Node Properties

						_dtMerchBasisDetail = GetMerchBasisData(aMethodRID);
						_dtCurveBasisDetail = GetCurveBasisData(aMethodRID);
						return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

		public DataTable GetMerchBasisData(int aMethodRID)
        {
            try
            {
                return StoredProcedures.MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ.Read(_dba,
                                                                                     METHOD_RID: aMethodRID,
                                                                                     MERCH_TYPE: eMerchandiseType.Node.GetHashCode()
                                                                                     );
            }

            catch (Exception Ex)
            {
                string message = Ex.ToString();
                throw;
            }
        }

		public DataTable GetCurveBasisData(int aMethodRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_READ.Read(_dba, METHOD_RID: aMethodRID);
			}

			catch (Exception Ex)
			{
				string message = Ex.ToString();
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
           
            int? SIZE_CURVES_BY_SG_RID_Nullable = null;
            if (_sizeCurvesBySGRID != Include.NoRID) SIZE_CURVES_BY_SG_RID_Nullable = _sizeCurvesBySGRID;
            StoredProcedures.SP_MID_MTH_SZ_CURVE_UPD_INS.Insert(td.DBA,
                                                                METHOD_RID: method_RID,
                                                                SIZE_GROUP_RID: _sizeGroupRID,
                                                                SIZE_CURVES_BY_TYPE: (int)_sizeCurvesByType,
                                                                SIZE_CURVES_BY_SG_RID: SIZE_CURVES_BY_SG_RID_Nullable,
                                                                MERCH_BASIS_EQUAL_WEIGHT_IND: Include.ConvertBoolToChar(_merchBasisEqualizeWeight),
                                                                CURVE_BASIS_EQUAL_WEIGHT_IND: Include.ConvertBoolToChar(_curveBasisEqualizeWeight),
                                                                APPLY_LOST_SALES_IND: Include.ConvertBoolToChar(_applyLostSales),
                                                                MINIMUM_AVERAGE: _tolerMinAvgPerSize,
                                                                SALES_TOLERANCE: _tolerSalesTolerance,
                                                                INDEX_UNITS_TYPE: (int)_tolerIndexUnitsType,
                                                                MIN_TOLERANCE: _tolerMinTolerancePct,
                                                                MAX_TOLERANCE: _tolerMaxTolerancePct,
                                                                APPLY_MIN_TO_ZERO_TOLERANCE_IND: Include.ConvertBoolToChar(_applyMinToZeroTolerance)
                                                                );
        }

		private bool UpdateCurveBasisData(int aMethodRID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			
			try
			{
                StoredProcedures.MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_DELETE.Delete(td.DBA, METHOD_RID: aMethodRID);

				if (_dtCurveBasisDetail == null)
				{
					return UpdateSuccessful;
				}

				foreach (DataRow dr in _dtCurveBasisDetail.Rows)
				{
                    StoredProcedures.MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT.Insert(td.DBA,
                                                                                  METHOD_RID: aMethodRID,
                                                                                  BASIS_SEQ: Convert.ToInt32(dr["BASIS_SEQ"]),
                                                                                  SIZE_CURVE_GROUP_RID: Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"]),
                                                                                  WEIGHT: Convert.ToDouble(dr["WEIGHT"])
                                                                                  );
				}

				return UpdateSuccessful;
			}
			catch (Exception Ex)
			{
				string exceptionMessage = Ex.Message;
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
		}

        //Begin TT#1235-MD -jsobek -Size Curve Null Reference Exception (Object Reference)

        /// <summary>
        /// Inserts data associated with the method
        /// </summary>
        /// <param name="method_RID">ID of the method to insert</param>
        /// <param name="td">An instance of the TransactionData class containing the database connection.</param>
        /// <returns></returns>
        public bool InsertMethod(int aMethodRID, TransactionData td)
        {
            bool InsertSuccessful = true;

            try
            {
                PerformUpdateInsert(aMethodRID, td);

                if (!UpdateMerchBasisData(aMethodRID, td))
                {
                    InsertSuccessful = false;
                }

                if (!UpdateCurveBasisData(aMethodRID, td))
                {
                    InsertSuccessful = false;
                }

                InsertSuccessful = true;

                return InsertSuccessful;
            }
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                InsertSuccessful = false;
                throw;
            }
        }


        /// <summary>
        /// Updates data associated with the method
        /// </summary>
        /// <param name="method_RID">ID of the method to update</param>
        /// <param name="td">An instance of the TransactionData class containing the database connection.</param>
        /// <returns></returns>
        public bool UpdateMethod(int aMethodRID, TransactionData td)
        {
            bool UpdateSuccessful = true;
            try
            {
                PerformUpdateInsert(aMethodRID, td);

                if (!UpdateMerchBasisData(aMethodRID, td))
                {
                    UpdateSuccessful = false;
                }

                if (!UpdateCurveBasisData(aMethodRID, td))
                {
                    UpdateSuccessful = false;
                }

                UpdateSuccessful = true;

                return UpdateSuccessful;
            }
            catch
            {
                UpdateSuccessful = false;
                throw;
            }
            finally
            {
            }
        }

        private bool UpdateMerchBasisData(int aMethodRID, TransactionData td)
        {
            //StringBuilder SQLCommand;
            bool UpdateSuccessful = true;
            //object phRID, phlSequence, offset, ollRID, custOllRID; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
            //object ollRID, custOllRID; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
            try
            {
                //SQLCommand = new StringBuilder();
                //SQLCommand.Append("DELETE FROM METHOD_SIZE_CURVE_MRCH_BAS_DET ");
                //SQLCommand.Append("WHERE METHOD_RID = @METHOD_RID");

                //MIDDbParameter[] inParams = { new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input) };

                //td.DBA.ExecuteNonQuery(SQLCommand.ToString(), inParams);
                StoredProcedures.MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_DELETE.Delete(td.DBA, aMethodRID);



                if (_dtMerchBasisDetail == null)
                {
                    return UpdateSuccessful;
                }

                foreach (DataRow dr in _dtMerchBasisDetail.Rows)
                {
					//BEGIN TT#4313 - DOConnell - Override Low Level Model problem
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
					//END TT#4313 - DOConnell - Override Low Level Model problem
                    //SQLCommand = new StringBuilder();
                    //SQLCommand.Append("INSERT INTO METHOD_SIZE_CURVE_MRCH_BAS_DET (");
                    //SQLCommand.Append("METHOD_RID, BASIS_SEQ, HN_RID, FV_RID, CDR_RID, ");
                    //SQLCommand.Append("WEIGHT, MERCH_TYPE, OLL_RID, CUSTOM_OLL_RID) ");
                    //SQLCommand.Append("VALUES (");
                    //SQLCommand.Append("@METHOD_RID, @BASIS_SEQ, @HN_RID, @FV_RID, @CDR_RID, ");
                    //SQLCommand.Append("@WEIGHT, @MERCH_TYPE, @OLL_RID, @CUSTOM_OLL_RID)");

                    //if (dr["OLL_RID"] == DBNull.Value ||
                    //    Convert.ToInt32(dr["OLL_RID"]) == Include.NoRID)
                    //{
                    //    ollRID = DBNull.Value;
                    //}
                    //else
                    //{
                    //    ollRID = Convert.ToInt32(dr["OLL_RID"]);
                    //}

                    //if (dr["CUSTOM_OLL_RID"] == DBNull.Value ||
                    //    Convert.ToInt32(dr["CUSTOM_OLL_RID"]) == Include.NoRID)
                    //{
                    //    custOllRID = DBNull.Value;
                    //}
                    //else
                    //{
                    //    custOllRID = Convert.ToInt32(dr["CUSTOM_OLL_RID"]);
                    //}

                    //MIDDbParameter[] inParams2 = { new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@BASIS_SEQ", Convert.ToInt32(dr["BASIS_SEQ"]), eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@HN_RID", Convert.ToInt32(dr["HN_RID"]), eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@FV_RID", Convert.ToInt32(dr["FV_RID"]), eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@CDR_RID", Convert.ToInt32(dr["CDR_RID"]), eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@WEIGHT", Convert.ToDouble(dr["WEIGHT"]), eDbType.Float, eParameterDirection.Input),
                    //                        new MIDDbParameter("@MERCH_TYPE", Convert.ToInt32(dr["MERCH_TYPE"]), eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@OLL_RID", ollRID, eDbType.Int, eParameterDirection.Input),
                    //                        new MIDDbParameter("@CUSTOM_OLL_RID", custOllRID, eDbType.Int, eParameterDirection.Input)
                    //                     };

                    //td.DBA.ExecuteNonQuery(SQLCommand.ToString(), inParams2);

                    int? OLL_RID_Nullable = null;
                    if (dr["OLL_RID"] != DBNull.Value && Convert.ToInt32(dr["OLL_RID"]) != Include.NoRID) OLL_RID_Nullable = Convert.ToInt32(dr["OLL_RID"]);
                    int? CUSTOM_OLL_RID_Nullable = null;
                    if (dr["CUSTOM_OLL_RID"] != DBNull.Value && Convert.ToInt32(dr["CUSTOM_OLL_RID"]) != Include.NoRID) CUSTOM_OLL_RID_Nullable = Convert.ToInt32(dr["CUSTOM_OLL_RID"]);

                    StoredProcedures.MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT.Insert(td.DBA,
                                                                                      METHOD_RID: aMethodRID,
                                                                                      BASIS_SEQ: Convert.ToInt32(dr["BASIS_SEQ"]),
                                                                                      HN_RID: Convert.ToInt32(dr["HN_RID"]),
                                                                                      FV_RID: Convert.ToInt32(dr["FV_RID"]),
                                                                                      CDR_RID: Convert.ToInt32(dr["CDR_RID"]),
                                                                                      WEIGHT: Convert.ToDouble(dr["WEIGHT"]),
                                                                                      MERCH_TYPE: Convert.ToInt32(dr["MERCH_TYPE"]),
                                                                                      OLL_RID: OLL_RID_Nullable,
                                                                                      CUSTOM_OLL_RID: CUSTOM_OLL_RID_Nullable
                                                                                      );
                }

                return UpdateSuccessful;
            }
            catch (Exception Ex)
            {
                string exceptionMessage = Ex.Message;
                UpdateSuccessful = false;
                throw;
            }
            finally
            {
            }
        }
        //End TT#1235-MD -jsobek -Size Curve Null Reference Exception (Object Reference)




		/// <summary>
        /// Deletes data and child data associated with the basis size curve method
        /// </summary>
        /// <param name="method_RID">ID of the method to delete</param>
        /// <param name="td">An instance of the TransactionData class containing the database connection.</param>
        /// <returns></returns>
        public bool DeleteMethod(int aMethodRID, TransactionData td)
        {
            bool Successfull = true;

            try
            {
                //MIDDbParameter[] inParams = { new MIDDbParameter("@METHOD_RID", aMethodRID, eDbType.Int, eParameterDirection.Input) };

                //td.DBA.ExecuteStoredProcedure("SP_MID_MTH_SZ_CURVE_DEL_MTH", inParams);
                StoredProcedures.SP_MID_MTH_SZ_CURVE_DEL_MTH.Delete(td.DBA, METHOD_RID: aMethodRID);

                Successfull = true;

				return Successfull;
			}
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                Successfull = false;
                throw;
            }
        }

		public DataTable ReadSizeData(HierarchyNodeProfile aNodeProf, ProfileList aWeekProfList, int aOLLRID)
		{
            try
            {
                // Build the week XML file

                //variableXML = new StringBuilder();
                //variableXML.Append("<root> ");

                //foreach (WeekProfile weekProf in aWeekProfList)
                //{
                //    variableXML.Append(" <time ID=\"");
                //    variableXML.Append(weekProf.Key.ToString());
                //    variableXML.Append("\"/> ");
                //}

                //variableXML.Append(" </root>");

                DataTable dtTimeIDList = new DataTable();
                dtTimeIDList.Columns.Add("TIME_ID", typeof(int));
                foreach (WeekProfile weekProf in aWeekProfList)
                {
                    //ensure time ids are distinct, and only added to the datatable one time
                    if (dtTimeIDList.Select("TIME_ID=" + weekProf.Key.ToString()).Length == 0)
                    {
                        DataRow dr = dtTimeIDList.NewRow();
                        dr["TIME_ID"] = weekProf.Key;
                        dtTimeIDList.Rows.Add(dr);
                    }
                }

                return StoredProcedures.MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK.Read(_dba,
                                                                                                 TIME_ID_LIST: dtTimeIDList,
                                                                                                 SELECTED_NODE_RID: aNodeProf.Key,
                                                                                                 OLL_RID: aOLLRID,
                                                                                                 USE_REG_SALES: (aNodeProf.OTSPlanLevelType == eOTSPlanLevelType.Regular) ? 1 : 0
                                                                                                 );

            }
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                throw;
            }
            finally
            {
            }
		}

        #endregion

      
    }
}
