using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for OTS_Plan
	/// </summary>
	public class OTSPlanMethodData: MethodBaseData
	{
		private int _Plan_HN_RID;
		private int _Plan_FV_RID;
		private int _CDR_RID;
		private int _Chain_FV_RID;
		private char _Bal_Sales_Ind;
		private char _Bal_Stock_Ind;
		private int _forecastModelRid;
		// BEGIN MID Track #4371 - Justin Bolles - Low Level Forecast
		private char _High_Level_Ind;
		private char _Low_Levels_Ind;
		private int _Low_Level_Type;
		private int _Low_Level_Seq;
		private int _Low_Level_Offset;
		private ProfileList _lowlevelExcludeList;
		// END MID Track #4371
		private int _overrideLLRid;
        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        private char _ApplyTrendOptionsInd;
        private float _ApplyTrendOptionsWOSValue;
        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

		public int Plan_HN_RID
		{
			get
			{return _Plan_HN_RID;}
			set
			{_Plan_HN_RID = value;	}
		}

		public int Plan_FV_RID
		{
			get
			{return _Plan_FV_RID;}
			set
			{_Plan_FV_RID = value;	}
		}
		public int CDR_RID
		{
			get
			{return _CDR_RID;}
			set
			{_CDR_RID = value;	}
		}
		public int Chain_FV_RID
		{
			get
			{return _Chain_FV_RID;}
			set
			{_Chain_FV_RID = value;	}
		}

		public char Bal_Sales_Ind
		{
			get
			{if (_Bal_Sales_Ind == 0)
				 return '0';
			 else
				 return _Bal_Sales_Ind;}
			set
			{_Bal_Sales_Ind = value;	}
		}

		public char Bal_Stock_Ind
		{
			get
			{if (_Bal_Stock_Ind == 0)
				 return '0';
			 else
				 return _Bal_Stock_Ind;}
			set
			{_Bal_Stock_Ind = value;	}
		}

		public int ForecastModelRid
		{
			get	{ return _forecastModelRid;}
			set	{_forecastModelRid = value;	}
		}

		// BEGIN MID Track #4371 - Justin Bolles - Low Level Forecast
		public char High_Level_Ind
		{
			get { return _High_Level_Ind; }
			set { _High_Level_Ind = value; }
		}

		public char Low_Levels_Ind
		{
			get { return _Low_Levels_Ind; }
			set { _Low_Levels_Ind = value; }
		}

		public int Low_Level_Type
		{
			get { return _Low_Level_Type; }
			set { _Low_Level_Type = value; }
		}

		public int Low_Level_Seq
		{
			get { return _Low_Level_Seq; }
			set { _Low_Level_Seq = value; }
		}

		public int Low_Level_Offset
		{
			get { return _Low_Level_Offset; }
			set { _Low_Level_Offset = value; }
		}

		public int OverrideLowLevelRid
		{
			get { return _overrideLLRid ; }
			set { _overrideLLRid = value; }
		}

        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
        public char ApplyTrendOptionsInd
        {
            get { return _ApplyTrendOptionsInd; }
            set { _ApplyTrendOptionsInd = value; }
        }

        public float ApplyTrendOptionsWOSValue
        {
            get { return _ApplyTrendOptionsWOSValue; }
            set { _ApplyTrendOptionsWOSValue = value; }
        }
        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 

		public ProfileList LowlevelExcludeList 
		{
			get 
			{ 
				if (_lowlevelExcludeList == null)
				{
					_lowlevelExcludeList = new ProfileList(eProfileType.LowLevelExclusions);
				}
				return _lowlevelExcludeList ; 
			}
			set { _lowlevelExcludeList = value; }
		}
		// END MID Track #4371

		public OTSPlanMethodData()
		{
			
		}

		public OTSPlanMethodData(int method_RID, eChangeType changeType)
		{
			switch (changeType)
			{
				case eChangeType.populate:
					PopulateOTSPlan(method_RID);
					break;
			}
		}
		
		public bool PopulateOTSPlan(int method_RID)
		{
			try
			{
				if (PopulateMethod(method_RID))
				{
                    //MID Track # 2354 - removed nolock because it causes concurrency issues
					DataTable dtOTSPlan = MIDEnvironment.CreateDataTable();
                    dtOTSPlan = StoredProcedures.MID_OTS_PLAN_READ_FROM_METHOD.Read(_dba, METHOD_RID: method_RID);

					if(dtOTSPlan.Rows.Count != 0)
					{
						DataRow dr = dtOTSPlan.Rows[0];
						if (dr["PLAN_HN_RID"] == DBNull.Value)
							_Plan_HN_RID = Include.NoRID;
						else
							_Plan_HN_RID = Convert.ToInt32(dr["PLAN_HN_RID"], CultureInfo.CurrentUICulture);
						if (dr["PLAN_FV_RID"] == DBNull.Value)
							_Plan_FV_RID = Include.NoRID;
						else
							_Plan_FV_RID = Convert.ToInt32(dr["PLAN_FV_RID"], CultureInfo.CurrentUICulture);
						_CDR_RID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						if (dr["CHAIN_FV_RID"] == DBNull.Value)
							_Chain_FV_RID = Include.NoRID;
						else
							_Chain_FV_RID = Convert.ToInt32(dr["CHAIN_FV_RID"], CultureInfo.CurrentUICulture);
						_Bal_Sales_Ind = Convert.ToChar(dr["BAL_SALES_IND"], CultureInfo.CurrentUICulture);
						_Bal_Stock_Ind = Convert.ToChar(dr["BAL_STOCK_IND"], CultureInfo.CurrentUICulture);
						if (dr["FORECAST_MOD_RID"] == DBNull.Value)
							_forecastModelRid = Include.NoRID;
						else
							_forecastModelRid = Convert.ToInt32(dr["FORECAST_MOD_RID"], CultureInfo.CurrentUICulture);
						
						// BEGIN MID Track #4371 - Justin Bolles - Low Level Forecast
						if (dr["HIGH_LEVEL_IND"] == DBNull.Value)
							High_Level_Ind = '0';
						else
							High_Level_Ind = Convert.ToChar(dr["HIGH_LEVEL_IND"], CultureInfo.CurrentUICulture);

						if(dr["LOW_LEVELS_IND"] == DBNull.Value)
							Low_Levels_Ind = '0';
						else
							Low_Levels_Ind = Convert.ToChar(dr["LOW_LEVELS_IND"], CultureInfo.CurrentUICulture);
						
						if (dr["LOW_LEVEL_TYPE"] == DBNull.Value)
							Low_Level_Type = Include.NoRID;
						else
							Low_Level_Type = Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);

						if (dr["LOW_LEVEL_SEQ"] == DBNull.Value)
							Low_Level_Seq = Include.NoRID;
						else
							Low_Level_Seq = Convert.ToInt32(dr["LOW_LEVEL_SEQ"], CultureInfo.CurrentUICulture);

						if (dr["LOW_LEVEL_OFFSET"] == DBNull.Value)
							Low_Level_Offset = Include.NoRID;
						else
							Low_Level_Offset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						// END MID Track #4371

						if (dr["OLL_RID"] == DBNull.Value)
							OverrideLowLevelRid = Include.NoRID;
						else
							OverrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);

                        // BEGIN TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
                        if (dr["TREND_OPTIONS_IND"] == DBNull.Value)
                            ApplyTrendOptionsInd = 'C';
                        else
                            ApplyTrendOptionsInd = Convert.ToChar(dr["TREND_OPTIONS_IND"], CultureInfo.CurrentUICulture);

                        if (dr["TREND_OPTIONS_PLUG_CHAIN_WOS"] == DBNull.Value)
                            ApplyTrendOptionsWOSValue = 0;
                        else
                            ApplyTrendOptionsWOSValue = (float)Convert.ToDouble(dr["TREND_OPTIONS_PLUG_CHAIN_WOS"], CultureInfo.CurrentUICulture);
                        // END TT#619 - AGallagher - OTS Forecast - Chain Plan not required (#46) 
					}
					else
						return false;
				}
				else
					return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

			return true;
		}

	

     

		public DataTable GetOTSPlanUIWorkflows(int method_RID)
		{ 
			try
			{	
				WorkflowBaseData wbd = new WorkflowBaseData();
				// Begin MID ISssue #3501 - stodd
				return wbd.GetOTSMethodPropertiesUIWorkflows(method_RID);
				// End MID ISssue #3501 - stodd

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		/// <summary>
		/// Insert a row in the OTS_Plan table
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		/// <param name="method_RID">The record ID of the method</param>
		public bool InsertOTSPlan(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;
			try
			{	
				// sets fields to NULL when 'NoRid' is found.

                int? PLAN_HN_RID_Nullable = null;
                if (Plan_HN_RID != Include.NoRID) PLAN_HN_RID_Nullable = Plan_HN_RID;

                int? PLAN_FV_RID_Nullable = null;
                if (Plan_FV_RID != Include.NoRID) PLAN_FV_RID_Nullable = Plan_FV_RID;

                int? CHAIN_FV_RID_Nullable = null;
                if (Chain_FV_RID != Include.NoRID) CHAIN_FV_RID_Nullable = Chain_FV_RID;

                int? FORECAST_MOD_RID_Nullable = null;
                if (ForecastModelRid != Include.NoRID) FORECAST_MOD_RID_Nullable = ForecastModelRid;

                int? LOW_LEVEL_TYPE_Nullable = null;
                if (Low_Level_Type != Include.NoRID) LOW_LEVEL_TYPE_Nullable = Low_Level_Type;

                int? LOW_LEVEL_SEQ_Nullable = null;
                if (Low_Level_Seq != Include.NoRID) LOW_LEVEL_SEQ_Nullable = Low_Level_Seq;

                int? LOW_LEVEL_OFFSET_Nullable = null;
                if (Low_Level_Offset != Include.NoRID) LOW_LEVEL_OFFSET_Nullable = Low_Level_Offset;

                int? OLL_RID_Nullable = null;
                if (OverrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = OverrideLowLevelRid;

                StoredProcedures.MID_OTS_PLAN_INSERT.Insert(td.DBA,
                                                            METHOD_RID: method_RID,
                                                            PLAN_HN_RID: PLAN_HN_RID_Nullable,
                                                            PLAN_FV_RID: PLAN_FV_RID_Nullable,
                                                            CDR_RID: CDR_RID,
                                                            CHAIN_FV_RID: CHAIN_FV_RID_Nullable,
                                                            BAL_SALES_IND: Bal_Sales_Ind,
                                                            BAL_STOCK_IND: Bal_Stock_Ind,
                                                            FORECAST_MOD_RID: FORECAST_MOD_RID_Nullable,
                                                            HIGH_LEVEL_IND: High_Level_Ind,
                                                            LOW_LEVELS_IND: Low_Levels_Ind,
                                                            LOW_LEVEL_TYPE: LOW_LEVEL_TYPE_Nullable,
                                                            LOW_LEVEL_SEQ: LOW_LEVEL_SEQ_Nullable,
                                                            LOW_LEVEL_OFFSET: LOW_LEVEL_OFFSET_Nullable,
                                                            OLL_RID: OLL_RID_Nullable,
                                                            TREND_OPTIONS_IND: ApplyTrendOptionsInd,
                                                            TREND_OPTIONS_PLUG_CHAIN_WOS: ApplyTrendOptionsWOSValue
                                                            );

				// BEGIN Override Low Level Enhancement
				//foreach(LowLevelExcludeProfile exclude in LowlevelExcludeList)
				//{
				//    if(exclude.Exclude)
				//        InsertOTSPlanLowLevelExclude(method_RID, exclude.Key, td);
				//}
				// END Override Low Level Enhancement
	
				InsertSuccessful = true;
			}
			catch
			{
				InsertSuccessful = false;
				throw;
			}
			return InsertSuccessful;
		}

		//To be moved to MethodRelations
		public bool UpdateOTSPlan(int method_RID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			try
			{
                //// sets fields to NULL when 'NoRid' is found.
               
                int? PLAN_HN_RID_Nullable = null;
                if (Plan_HN_RID != Include.NoRID) PLAN_HN_RID_Nullable = Plan_HN_RID;

                int? PLAN_FV_RID_Nullable = null;
                if (Plan_FV_RID != Include.NoRID) PLAN_FV_RID_Nullable = Plan_FV_RID;

                int? CHAIN_FV_RID_Nullable = null;
                if (Chain_FV_RID != Include.NoRID) CHAIN_FV_RID_Nullable = Chain_FV_RID;

                int? FORECAST_MOD_RID_Nullable = null;
                if (ForecastModelRid != Include.NoRID) FORECAST_MOD_RID_Nullable = ForecastModelRid;

                int? LOW_LEVEL_TYPE_Nullable = null;
                if (Low_Level_Type != Include.NoRID) LOW_LEVEL_TYPE_Nullable = Low_Level_Type;

                int? LOW_LEVEL_SEQ_Nullable = null;
                if (Low_Level_Seq != Include.NoRID) LOW_LEVEL_SEQ_Nullable = Low_Level_Seq;

                int? LOW_LEVEL_OFFSET_Nullable = null;
                if (Low_Level_Offset != Include.NoRID) LOW_LEVEL_OFFSET_Nullable = Low_Level_Offset;

                int? OLL_RID_Nullable = null;
                if (OverrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = OverrideLowLevelRid;

                StoredProcedures.MID_OTS_PLAN_UPDATE.Update(td.DBA,
                                                            METHOD_RID: method_RID,
                                                            PLAN_HN_RID: PLAN_HN_RID_Nullable,
                                                            PLAN_FV_RID: PLAN_FV_RID_Nullable,
                                                            CDR_RID: CDR_RID,
                                                            CHAIN_FV_RID: CHAIN_FV_RID_Nullable,
                                                            BAL_SALES_IND: Bal_Sales_Ind,
                                                            BAL_STOCK_IND: Bal_Stock_Ind,
                                                            FORECAST_MOD_RID: FORECAST_MOD_RID_Nullable,
                                                            HIGH_LEVEL_IND: High_Level_Ind,
                                                            LOW_LEVELS_IND: Low_Levels_Ind,
                                                            LOW_LEVEL_TYPE: LOW_LEVEL_TYPE_Nullable,
                                                            LOW_LEVEL_SEQ: LOW_LEVEL_SEQ_Nullable,
                                                            LOW_LEVEL_OFFSET: LOW_LEVEL_OFFSET_Nullable,
                                                            OLL_RID: OLL_RID_Nullable,
                                                            TREND_OPTIONS_IND: ApplyTrendOptionsInd,
                                                            TREND_OPTIONS_PLUG_CHAIN_WOS: ApplyTrendOptionsWOSValue
                                                            );

				// BEGIN Override Low level Enhancement
				//this.DeleteOTSPlanLowLevelExclude(method_RID, td);
				//foreach(LowLevelExcludeProfile exclude in LowlevelExcludeList)
				//{
				//    if(exclude.Exclude)
				//        InsertOTSPlanLowLevelExclude(method_RID, exclude.Key, td);
				//}
				// END Override Low level Enhancement

//				_dba.CommitData();
				UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			return UpdateSuccessful;
		}

		//To be moved to MethodRelations
		public bool DeleteOTSPlan(int method_RID, TransactionData td)
		{
			bool DeleteSuccessful = true;
			try
			{
                StoredProcedures.MID_OTS_PLAN_DELETE.Delete(td.DBA, METHOD_RID: method_RID);
	
				DeleteSuccessful = true;
			}
			catch
			{
				DeleteSuccessful = false;
				throw;
			}

			return DeleteSuccessful;
		}

	
		public DataTable GetMethodsByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_OTS_PLAN_READ_METHODS_BY_NODE.Read(_dba, PLAN_HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        //public DataTable GetChainSetPercentByDate(int aYearWeekId, int bYearWeekId, int plnHN_RID, int StoreGroupRID)
        public DataTable GetChainSetPercentByDate(int aYearWeekId, int bYearWeekId, int plnHN_RID, int StoreGroupRID, int sg_Version)
        // End TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
        {

            try
            {
                return StoredProcedures.SP_MID_CHAIN_SET_PERCENT_SET_RETURN.Read(_dba,
                                                                                   BEG_WEEK: aYearWeekId,
                                                                                   END_WEEK: bYearWeekId,
                                                                                   HN_RID: plnHN_RID,
                                                                                   SG_RID: StoreGroupRID,
                                                                                   SG_VERSION: sg_Version  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                                                                   );
            }
            catch
            {
                throw;
            }
        }
        //End TT#1628 - DOConnell - Chain Set Percentage (API) - CSP/API does not insert zero percentages nor balance out on the OTS Forecast Review.
	}
}
