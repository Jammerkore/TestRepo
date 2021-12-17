using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for Models.
	/// </summary>
	public partial class ModelsData : DataLayer
	{

		public ModelsData() : base()
		{
		}

		#region FORECAST_BAL_MODEL
		public DataTable ForecastBalModel_Read()
		{
			try
			{
                return StoredProcedures.MID_FORECAST_BAL_MODEL_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ForecastBalModel_Read(int aModelRID)
		{
			try
			{
                return StoredProcedures.MID_FORECAST_BAL_MODEL_READ_WITH_RID.Read(_dba, FBMOD_RID: aModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ForecastBalModel_Read(string aModelID)
		{
			try
			{
                return StoredProcedures.MID_FORECAST_BAL_MODEL_READ_WITH_ID.Read(_dba, FBMOD_ID: aModelID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int ForecastBalModel_Add(string aModelID, eMatrixType aMatrixType, eIterationType aIterationType,
			int aIterationCount, eBalanceMode aBalanceMode, string aCalcMode)
		{
			try
			{
                return StoredProcedures.SP_MID_FOR_BAL_MODEL_INSERT.InsertAndReturnRID(_dba,
                                                                                  FBMOD_ID: aModelID,
                                                                                  MATRIX_TYPE: Convert.ToInt32(aMatrixType),
                                                                                  ITERATIONS_TYPE: Convert.ToInt32(aIterationType),
                                                                                  ITERATIONS_COUNT: Convert.ToInt32(aIterationCount),
                                                                                  BALANCE_MODE: Convert.ToInt32(aBalanceMode),
                                                                                  CALC_MODE: aCalcMode
                                                                                  );

			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastBalModel_Update(int aModelRID, string aModelID, eMatrixType aMatrixType, eIterationType aIterationType,
			int aIterationCount, eBalanceMode aBalanceMode, string aCalcMode)
		{
			try
			{
                StoredProcedures.MID_FORECAST_BAL_MODEL_UPDATE.Update(_dba,
                                                                      FBMOD_RID: aModelRID,
                                                                      FBMOD_ID: aModelID,
                                                                      MATRIX_TYPE: Convert.ToInt32(aMatrixType),
                                                                      ITERATIONS_TYPE: Convert.ToInt32(aIterationType),
                                                                      ITERATIONS_COUNT: Convert.ToInt32(aIterationCount),
                                                                      BALANCE_MODE: Convert.ToInt32(aBalanceMode),
                                                                      CALC_MODE: aCalcMode
                                                                      );
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastBalModel_Delete(int aModelRID)
		{
			try
			{
                StoredProcedures.MID_FORECAST_BAL_MODEL_DELETE.Delete(_dba, FBMOD_RID: aModelRID);
				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion

		#region FORECAST_BAL_MODEL_VARIABLE
		public DataTable ForecastBalModelVariable_Read(int aModelRID)
		{
			try
			{
                //string SQLCommand = "SELECT * FROM FORECAST_BAL_MODEL_VARIABLE WHERE FBMOD_RID = " + aModelRID +
                //    " order by VARIABLE_SEQUENCE";;
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "FORECAST_BAL_MODEL" );

                return StoredProcedures.MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID.Read(_dba, FBMOD_RID: aModelRID);
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastBalModelVariable_Add(int aModelRID, int aSequence, int aVariableNumber)
		{
			try
			{
                //string SQLCommand;

                //SQLCommand = "INSERT INTO FORECAST_BAL_MODEL_VARIABLE(FBMOD_RID, VARIABLE_SEQUENCE, VARIABLE_NUMBER) "
                //    + " VALUES ("
                //    + aModelRID + "," 
                //    + aSequence + "," 
                //    + aVariableNumber + ")";
                //_dba.ExecuteNonQuery(SQLCommand);

                StoredProcedures.MID_FORECAST_BAL_MODEL_VARIABLE_INSERT.Insert(_dba,
                                                                               FBMOD_RID: aModelRID,
                                                                               VARIABLE_SEQUENCE: aSequence,
                                                                               VARIABLE_NUMBER: aVariableNumber
                                                                               );
				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastBalModelVariable_Delete(int aModelRID)
		{
			try
			{
                //string SQLCommand;

                //SQLCommand = "DELETE FROM FORECAST_BAL_MODEL_VARIABLE "
                //    + " WHERE FBMOD_RID = " + aModelRID;

                //_dba.ExecuteNonQuery(SQLCommand);
                StoredProcedures.MID_FORECAST_BAL_MODEL_VARIABLE_DELETE.Delete(_dba, FBMOD_RID: aModelRID);
				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion

		#region FORECAST_MODEL
		public DataTable ForecastModel_Read()
		{
			try
			{
                return StoredProcedures.MID_FORECAST_MODEL_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ForecastModel_Read(int aModelRID)
		{
			try
			{
                return StoredProcedures.MID_FORECAST_MODEL_READ_ALL_WITH_RID.Read(_dba, MODEL_RID: aModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ForecastModel_Read(string aModelID)
		{
			try
			{
                return StoredProcedures.MID_FORECAST_MODEL_READ_ALL_WITH_ID.Read(_dba, MODEL_ID: aModelID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int ForecastModel_Add(string aModelID, bool isDefault, string aCalcMode)
		{
			try
			{

                char defaultIND = (char)Convert.ToInt32(isDefault);

                int modelRID = StoredProcedures.SP_MID_FORECAST_MODEL_INSERT.InsertAndReturnRID(_dba,
                                                                                                  FORECAST_MOD_ID: aModelID,
                                                                                                  DEFAULT_IND: defaultIND,
                                                                                                  CALC_MODE: aCalcMode
                                                                                                  );
                return modelRID;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastModel_Update(int aModelRID, string aModelID, bool isDefault, string aCalcMode)
		{
			try
			{
                char defaultIND = (char)Convert.ToInt32(isDefault);

                StoredProcedures.MID_FORECAST_MODEL_UPDATE.Update(_dba,
                                                                    FORECAST_MOD_RID: aModelRID,
                                                                    FORECAST_MOD_ID: aModelID,
                                                                    DEFAULT_IND: defaultIND,
                                                                    CALC_MODE: aCalcMode
                                                                    );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastModel_Delete(int aModelRID)
		{
			try
			{
                StoredProcedures.MID_FORECAST_MODEL_DELETE.Delete(_dba, FORECAST_MOD_RID: aModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion

		#region FORECAST_MODEL_VARIABLE
		public DataTable ForecastModelVariable_Read(int aModelRID)
		{
			try
			{
                //string SQLCommand = "SELECT * FROM FORECAST_MODEL_VARIABLE WHERE FORECAST_MOD_RID = " + aModelRID +
                //    " order by VARIABLE_SEQUENCE";;
				
                //return _dba.ExecuteSQLQuery( SQLCommand, "FORECAST_MODEL_VARIABLE" );
                return StoredProcedures.MID_FORECAST_MODEL_VARIABLE_READ_ALL.Read(_dba, FORECAST_MOD_RID: aModelRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastModelVariable_Add(int aModelRID, 
			                                  int aSequence,
			                                  int aVariableNumber,
			                                  int aForecastFormula,
			                                  int aAssocSalesVar,
			                                  bool aGradeWOSIDX,
			                                  bool aStockModifier,
			                                  bool aFWOSOverride,
			                                  bool aStockMin, 
			                                  bool aStockMax, 
			                                  bool aMinPlusSales, 
			                                  bool aSalesModifier,
											  bool aUsePlan,
											  bool aAllowChainNegatives)	// Track 6271 stodd
		{
			try
			{
                //string SQLCommand;

                //// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
                //SQLCommand = "INSERT INTO FORECAST_MODEL_VARIABLE(" +
                //             "FORECAST_MOD_RID, VARIABLE_SEQUENCE, VARIABLE_NUMBER, " +
                //             "FORECAST_FORMULA, ASSOC_VARIABLE, GRADE_WOS_IDX, " +
                //             "STOCK_MODIFIER, FWOS_OVERRIDE, STOCK_MIN, STOCK_MAX, " +
                //             "MIN_PLUS_SALES, SALES_MODIFIER, USE_PLAN, ALLOW_CHAIN_NEGATIVES) " +		// Track #6187	// Track 6271
                //    " VALUES (@FORECAST_MOD_RID, @VARIABLE_SEQUENCE, @VARIABLE_NUMBER, " +
                //             "@FORECAST_FORMULA, @ASSOC_VARIABLE, @GRADE_WOS_IDX, " +
                //             "@STOCK_MODIFIER, @FWOS_OVERRIDE, @STOCK_MIN, @STOCK_MAX, " +
                //             "@MIN_PLUS_SALES, @SALES_MODIFIER, @USE_PLAN, @ALLOW_CHAIN_NEGATIVES)";	// Track #6187

                //MIDDbParameter[] InParams = {new MIDDbParameter("@FORECAST_MOD_RID", Convert.ToInt32(aModelRID, CultureInfo.CurrentCulture), eDbType.Int, eParameterDirection.Input),
                //                             new MIDDbParameter("@VARIABLE_SEQUENCE", Convert.ToInt32(aSequence, CultureInfo.CurrentCulture), eDbType.Int, eParameterDirection.Input),
                //                             new MIDDbParameter("@VARIABLE_NUMBER", Convert.ToInt32(aVariableNumber, CultureInfo.CurrentCulture), eDbType.Int, eParameterDirection.Input),
                //                             new MIDDbParameter("@FORECAST_FORMULA", Convert.ToInt32(aForecastFormula, CultureInfo.CurrentCulture), eDbType.Int, eParameterDirection.Input),
                //                             new MIDDbParameter("@ASSOC_VARIABLE", Convert.ToInt32(aAssocSalesVar, CultureInfo.CurrentCulture), eDbType.Int, eParameterDirection.Input),
                //                             new MIDDbParameter("@GRADE_WOS_IDX", Include.ConvertBoolToChar(aGradeWOSIDX), eDbType.Char, eParameterDirection.Input),
                //                             new MIDDbParameter("@STOCK_MODIFIER", Include.ConvertBoolToChar(aStockModifier), eDbType.Char, eParameterDirection.Input),
                //                             new MIDDbParameter("@FWOS_OVERRIDE", Include.ConvertBoolToChar(aFWOSOverride), eDbType.Char, eParameterDirection.Input),
                //                             new MIDDbParameter("@STOCK_MIN", Include.ConvertBoolToChar(aStockMin), eDbType.Char, eParameterDirection.Input),
                //                             new MIDDbParameter("@STOCK_MAX", Include.ConvertBoolToChar(aStockMax), eDbType.Char, eParameterDirection.Input),
                //                             new MIDDbParameter("@MIN_PLUS_SALES", Include.ConvertBoolToChar(aMinPlusSales), eDbType.Char, eParameterDirection.Input),
                //                             // Begin Track #6187 stodd
                //                             new MIDDbParameter("@SALES_MODIFIER", Include.ConvertBoolToChar(aSalesModifier), eDbType.Char, eParameterDirection.Input),
                //                             new MIDDbParameter("@USE_PLAN", Include.ConvertBoolToChar(aUsePlan), eDbType.Char, eParameterDirection.Input),
                //                             // End Track #6187 stodd
                //                             // Begin Track #6187 stodd
                //                             new MIDDbParameter("@ALLOW_CHAIN_NEGATIVES", Include.ConvertBoolToChar(aAllowChainNegatives), eDbType.Char, eParameterDirection.Input)
                //                             // End Track #6187 stodd
                //                          };

                //_dba.ExecuteNonQuery( SQLCommand, InParams );

                StoredProcedures.MID_FORECAST_MODEL_VARIABLE_INSERT.Insert(_dba,
                                                                                FORECAST_MOD_RID: aModelRID,
                                                                                VARIABLE_SEQUENCE: aSequence,
                                                                                VARIABLE_NUMBER: aVariableNumber,
                                                                                FORECAST_FORMULA: aForecastFormula,
                                                                                ASSOC_VARIABLE: aAssocSalesVar,
                                                                                GRADE_WOS_IDX: Include.ConvertBoolToChar(aGradeWOSIDX),
                                                                                STOCK_MODIFIER: Include.ConvertBoolToChar(aStockModifier),
                                                                                FWOS_OVERRIDE: Include.ConvertBoolToChar(aFWOSOverride),
                                                                                STOCK_MIN: Include.ConvertBoolToChar(aStockMin),
                                                                                STOCK_MAX: Include.ConvertBoolToChar(aStockMax),
                                                                                MIN_PLUS_SALES: Include.ConvertBoolToChar(aMinPlusSales),
                                                                                SALES_MODIFIER: Include.ConvertBoolToChar(aSalesModifier),
                                                                                USE_PLAN: Include.ConvertBoolToChar(aUsePlan),
                                                                                ALLOW_CHAIN_NEGATIVES: Include.ConvertBoolToChar(aAllowChainNegatives)
                                                                                );
				// END MID Track #5773

				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void ForecastModelVariable_Delete(int aModelRID)
		{
			try
			{
                //string SQLCommand;

                //SQLCommand = "DELETE FROM FORECAST_MODEL_VARIABLE "
                //    + " WHERE FORECAST_MOD_RID = " + aModelRID;

                //_dba.ExecuteNonQuery(SQLCommand);
                StoredProcedures.MID_FORECAST_MODEL_VARIABLE_DELETE.Delete(_dba, FORECAST_MOD_RID: aModelRID);
				return;
			
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		#endregion

        #region Override Low Levels Model

        // Begin TT#1126 - JSmith - Cannot have override low level models with the same name for global and user
        public bool OverrideLowLevelsModelName_Exist(string aModelName, int aUserRID)
        {
            try
            {
                return (StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_COUNT.ReadRecordCount(_dba,
                                                                                             NAME: aModelName,
                                                                                             USER_RID: aUserRID
                                                                                             ) > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1126

		public bool OverrideLowLevelsModel_IsCustom(int aModelRid)
		{
			bool isCustom = false;
			int userRid = Include.NoRID;
			try
			{
				DataTable dt = OverrideLowLevelsModel_Read(aModelRid);
				if (dt.Rows.Count > 0)
				{
					DataRow aRow = dt.Rows[0];
					if (aRow["USER_RID"] != DBNull.Value)
						userRid = Convert.ToInt32(aRow["USER_RID"]);
				}
				if (userRid == Include.CustomUserRID)
					isCustom = true;

				return isCustom;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable OverrideLowLevelsModel_Read(int aOllRID)
        {
            try
            {
                return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL.Read(_dba,
                                                                                    OLL_RID: aOllRID,
                                                                                    ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                    );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable OverrideLowLevelsModel_ReadAll(int aOllRID, int aUserRID, bool globalAllowView, bool userAllowView, int customOllRID)
		{
			try
			{
				if (userAllowView && globalAllowView)
				{
     

                return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_AND_USER_VIEWS.Read(_dba,
                                                                                                     OLL_RID: aOllRID,
                                                                                                     USER_RID: aUserRID,
                                                                                                     customOLL_rid: customOllRID,
                                                                                                     ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                                     );
				}
				else if (!userAllowView && globalAllowView) // ONLY User
				{

                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_GLOBAL_VIEW.Read(_dba,
                                                                                               OLL_RID: aOllRID,
                                                                                               USER_RID: aUserRID,
                                                                                               customOLL_rid: customOllRID,
                                                                                               ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                               );
				}
				else if (userAllowView && !globalAllowView) // ONLY Global
				{

                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_USER_VIEW.Read(_dba,
                                                                                             OLL_RID: aOllRID,
                                                                                             USER_RID: aUserRID,
                                                                                             customOLL_rid: customOllRID,
                                                                                             ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                             );
				}
                //else if (!userAllowView && !globalAllowView) // Neither Global or User
                else
				{;
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_ALL_NO_VIEW.Read(_dba,
                                                                                            OLL_RID: aOllRID,
                                                                                            USER_RID: aUserRID,
                                                                                            customOLL_rid: customOllRID,
                                                                                            ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                            );
				}

			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public string OverrideLowLevelsModel_GetModelName(int aOllRID)
		{
			try
			{
                object ollName = StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_GET_NAME.Read(_dba, OLL_RID: aOllRID);
				return ollName.ToString();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public int OverrideLowLevelsModel_GetModelKey(string aOllName)
        {
            DataTable dt;
            DataRow dr;
            try
            {
                dt = StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_READ_WITH_NAME.Read(_dba, NAME: aOllName);
                if (dt.Rows.Count == 0)
                {
                    return Include.NoRID;
                }
                else
                {
                    dr = dt.Rows[0];
                    return Convert.ToInt32(dr["OLL_RID"]);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



        // TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        public int OverrideLowLevelsModel_Add(string aName, int aHN_RID, int aHigh_Level_HN_RID, int aUser_RID,
                                              int aHighLevelSeq, int aHighLevelOffset, eHighLevelsType aHighLevelType,
                                              int aLowLevelSeq, int aLowLevelOffset, eLowLevelsType aLowLevelType, bool aActiveOnlyInd)
        {
            int modelRID;
            try
            {
                
                modelRID = StoredProcedures.SP_MID_OVERRIDE_LL_MODEL_INSERT.InsertAndReturnRID(_dba,
                                                                                        NAME: aName,
                                                                                        HN_RID: aHN_RID,
                                                                                        HIGH_LEVEL_HN_RID: aHigh_Level_HN_RID,
                                                                                        HIGH_LEVEL_SEQ: aHighLevelSeq,
                                                                                        HIGH_LEVEL_OFFSET: aHighLevelOffset,
                                                                                        HIGH_LEVEL_TYPE: (int)aHighLevelType,
                                                                                        LOW_LEVEL_SEQ: aLowLevelSeq,
                                                                                        LOW_LEVEL_OFFSET: aLowLevelOffset,
                                                                                        LOW_LEVEL_TYPE: (int)aLowLevelType,
                                                                                        ACTIVE_ONLY_IND: Include.ConvertBoolToChar(aActiveOnlyInd)
                                                                                        );

                if (aUser_RID != Include.CustomUserRID)
                {
                    //----Make Modification If Global Or User-----------
                    //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                    if (ConnectionIsOpen)
                    {
                        SecurityAdmin sa = new SecurityAdmin(_dba);
                        sa.AddUserItem(aUser_RID, (int)eProfileType.OverrideLowLevelModel, modelRID, aUser_RID);
                    }

                    //SecurityAdmin sa = new SecurityAdmin();
                    //try
                    //{
                    //    sa.OpenUpdateConnection();
                    //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                    //    //sa.AddUserItem(aUser_RID, (int)eSharedDataType.OverrideLowLevelModel, modelRID, aUser_RID);
                    //    sa.AddUserItem(aUser_RID, (int)eProfileType.OverrideLowLevelModel, modelRID, aUser_RID);
                    //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                    //    sa.CommitData();
                    //}
                    //catch
                    //{
                    //    throw;
                    //}
                    //finally
                    //{
                    //    sa.CloseUpdateConnection();
                    //}
                    //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

                }
                return modelRID;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        public void OverrideLowLevelsModel_Update(int aModelRID, string aName, int aHN_RID, int aHigh_Level_HN_RID, int aUser_RID,
                                                  int aHighLevelSeq, int aHighLevelOffset, eHighLevelsType aHighLevelType,
                                                  int aLowLevelSeq, int aLowLevelOffset, eLowLevelsType aLowLevelType, bool aActiveOnlyInd)
        {
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_UPDATE.Update(_dba,
                                                                            NAME: aName,
                                                                            OLL_RID: aModelRID,
                                                                            HN_RID: aHN_RID,
                                                                            HIGH_LEVEL_HN_RID: aHigh_Level_HN_RID,
                                                                            HIGH_LEVEL_SEQ: aHighLevelSeq,
                                                                            HIGH_LEVEL_OFFSET: aHighLevelOffset,
                                                                            HIGH_LEVEL_TYPE: (int)aHighLevelType,
                                                                            LOW_LEVEL_SEQ: aLowLevelSeq,
                                                                            LOW_LEVEL_OFFSET: aLowLevelOffset,
                                                                            LOW_LEVEL_TYPE: (int)aLowLevelType,
                                                                            ACTIVE_ONLY_IND: Include.ConvertBoolToChar(aActiveOnlyInd)
                                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

            //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
            if (ConnectionIsOpen)
            {
                SecurityAdmin sa = new SecurityAdmin(_dba);
                sa.DeleteUserItemByTypeAndRID((int)eProfileType.OverrideLowLevelModel, aModelRID);
                if (aUser_RID != Include.CustomUserRID)
                {
                    sa.AddUserItem(aUser_RID, (int)eProfileType.OverrideLowLevelModel, aModelRID, aUser_RID);
                }
            }
            //SecurityAdmin sa = new SecurityAdmin();
            //try
            //{
            //    sa.OpenUpdateConnection();
            //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.OverrideLowLevelModel, aModelRID);
            //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.OverrideLowLevelModel, aModelRID);
            //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    if (aUser_RID != Include.CustomUserRID)
            //    {
            //        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //        //sa.AddUserItem(aUser_RID, (int)eSharedDataType.OverrideLowLevelModel, aModelRID, aUser_RID);
            //        sa.AddUserItem(aUser_RID, (int)eProfileType.OverrideLowLevelModel, aModelRID, aUser_RID);
            //        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    }
            //    sa.CommitData();
            //}
            //catch
            //{
            //    throw;
            //}
            //finally
            //{
            //    sa.CloseUpdateConnection();
            //}
            //End TT#1564 - DOConnell - Missing Tasklist record prevents Login
            return;

        }

        public void OverrideLowLevelsModel_Delete(int aModelRID)
        {
            try
            {
                StoredProcedures.MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK.Update(_dba, OLL_RID: aModelRID);
                StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_DELETE.Delete(_dba, OLL_RID: aModelRID);

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.DeleteUserItemByTypeAndRID((int)eProfileType.OverrideLowLevelModel, aModelRID);
                }

                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.OverrideLowLevelModel, aModelRID);
                //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.OverrideLowLevelModel, aModelRID);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login
                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable OverrideLowLevelsDetail_Read(int aModelRID)
        {
            try
            {
                return StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_READ_ALL.Read(_dba, OLL_RID: aModelRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public eChangeType OverrideLowLevelsDetail_Action(int aModelRID, int aHN_RID, int aVersionRID, bool aExcludeInd)
        {
            try
            {
                //==========================================
                // Determine whether to INSERT or UPDATE
                //==========================================
                int OVERRIDE_LL_MODEL_DETAIL_COUNT = StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT.ReadRecordCount(_dba,
                                                                                         OLL_RID: aModelRID,
                                                                                         HN_RID: aHN_RID
                                                                                         );
                // UPDATE
                if (OVERRIDE_LL_MODEL_DETAIL_COUNT > 0)
                {
                    return eChangeType.update;
                }
                else // INSERT
                {
                    return eChangeType.add;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void OverrideLowLevelsDetail_Add(eChangeType changeType, int aModelRID, int aHN_RID, int aVersionRID, bool aExcludeInd)
        {
            try
            {
				////==========================================
				//// Determine whether to INSERT or UPDATE
				////==========================================
    //            int OVERRIDE_LL_MODEL_DETAIL_COUNT = StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT.ReadRecordCount(_dba,
    //                                                                                     OLL_RID: aModelRID,
    //                                                                                     HN_RID: aHN_RID
    //                                                                                     );
				// UPDATE
                //if (OVERRIDE_LL_MODEL_DETAIL_COUNT > 0)
                if (changeType == eChangeType.update)
				{
              

                    int? VERSION_RID_Nullable = null;
                    if (aVersionRID != Include.NoRID) VERSION_RID_Nullable = aVersionRID;

                    StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_UPDATE.Update(_dba, 
				                                                    OLL_RID: aModelRID,
				                                                    HN_RID: aHN_RID,
				                                                    VERSION_RID: VERSION_RID_Nullable,
				                                                    EXCLUDE_IND: Include.ConvertBoolToChar(aExcludeInd)
				                                                    );
				}
				else // INSERT
				{
                    int? VERSION_RID_Nullable = null;
                    if (aVersionRID != Include.NoRID) VERSION_RID_Nullable = aVersionRID;

                    StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_INSERT.Insert(_dba,
                                                                            OLL_RID: aModelRID,
                                                                            HN_RID: aHN_RID,
                                                                            VERSION_RID: VERSION_RID_Nullable,
                                                                            EXCLUDE_IND: Include.ConvertBoolToChar(aExcludeInd)
                                                                            );
				}
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



        public void OverrideLowLevelsDetail_Delete(int aModelRID, int aHN_RID)
        {
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_DELETE_WITH_NODE.Delete(_dba,
                                                                                      OLL_RID: aModelRID,
                                                                                      HN_RID: aHN_RID
                                                                                      );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void OverrideLowLevelsDetail_DeleteCustomOLLRidForiegnKeys(int customOLLRid)
        {
            try
            {
                if (customOLLRid != Include.NoRID)
                {
                    // Removes from Method and from User Plan
                    StoredProcedures.MID_METHOD_USER_PLAN_UPDATE_CUSTOM_OLL_RID.Update(_dba, OLL_RID: customOLLRid);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void OverrideLowLevelsDetail_Delete(int aModelRID)
        {
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_DELETE.Delete(_dba, OLL_RID: aModelRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // BEGIN Track #4985 - John Smith - Override Models
        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level


        public DataTable GetOverridesByOffset(int aModelRid, int aNodeRID, int aLevelOffset, int aHighLevelNodeRID,
            bool aMaintainingModels)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_OVERRIDES_BY_OFFSET.Read(_dba,
                                                                         MODEL_RID: aModelRid,
                                                                         HN_RID: aNodeRID,
                                                                         LEVEL_OFFSET: aLevelOffset,
                                                                         HIGH_LEVEL_HN_RID: aHighLevelNodeRID,
                                                                         MAINTENANCE: Include.ConvertBoolToChar(aMaintainingModels)
                                                                         );

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable GetOverridesByLevel(int aModelRid, int aNodeRID, int aLevelSeq, int aHighLevelNodeRID,
            bool aMaintainingModels)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_OVERRIDES_BY_LEVEL.Read(_dba,
                                                                        MODEL_RID: aModelRid,
                                                                        HN_RID: aNodeRID,
                                                                        LEVEL_SEQ: aLevelSeq,
                                                                        HIGH_LEVEL_HN_RID: aHighLevelNodeRID,
                                                                        MAINTENANCE: Include.ConvertBoolToChar(aMaintainingModels)
                                                                        );

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // END Track #6107

		//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
		public DataTable GetOverridesByType(int aModelRid, int aNodeRID, eHierarchyLevelType aLevelType, int aHighLevelNodeRID, bool aMaintainingModels)
        {
            try
            {
                return StoredProcedures.SP_MID_GET_OVERRIDES_BY_TYPE.Read(_dba,
                                                                            MODEL_RID: aModelRid,
                                                                            HN_RID: aNodeRID,
                                                                            LEVEL_TYPE: Convert.ToInt32(aLevelType),
                                                                            HIGH_LEVEL_HN_RID: aHighLevelNodeRID,
                                                                            MAINTENANCE: Include.ConvertBoolToChar(aMaintainingModels)
                                                                            );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		//End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
        public void DeleteOverrides(int aModelRid, int aNodeRID)
        {
            try
            {
                StoredProcedures.SP_MID_DELETE_OVERRIDES.Delete(_dba,
                                                            MODEL_RID: aModelRid,
                                                            HN_RID: aNodeRID
                                                            );

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // END Track #4985

		public void OverrideLowLevelsModel_CopyToWorkTables(int aOllRID)
		{
			try
			{
				//========================================================================
				// Delete and provious records for this model setting on the work table
				//========================================================================
				//OverrideLowLevelsDetailWork_Delete(aOllRID);
                //OverrideLowLevelsModelWork_Delete(aOllRID);

               
				//===================
				// Copy model HEADER
				//===================
                StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT_COPY_TO_WORK_TABLE.Insert(_dba,
                                                                                                    OLL_RID: aOllRID,
                                                                                                    ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                                    );

				//===================
				// Copy model DETAIL
				//===================
                StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT_COPY_MODEL_DETAIL.Insert(_dba, OLL_RID: aOllRID);

			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
        #endregion
   
	    #region Override Low Levels Model Work Table



		public bool OverrideLowLevelsModelWork_IsCustom(int aModelRid)
		{
			bool isCustom = false;
			int userRid = Include.NoRID;
			try
			{
				DataTable dt = OverrideLowLevelsModel_Read(aModelRid);
				if (dt.Rows.Count > 0)
				{
					DataRow aRow = dt.Rows[0];
					if (aRow["USER_RID"] != DBNull.Value)
						userRid = Convert.ToInt32(aRow["USER_RID"]);
				}
				if (userRid == Include.CustomUserRID)
					isCustom = true;

				return isCustom;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable OverrideLowLevelsModelWork_Read(int aOllRID)
        {
            try
            {
                return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL.Read(_dba,
                                                                                        OLL_RID: aOllRID,
                                                                                        ITEM_TYPE: (int)eProfileType.OverrideLowLevelModel
                                                                                        );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable OverrideLowLevelsModelWork_ReadAll(int aOllRID, int aUserRID, bool globalAllowView, bool userAllowView, int customOllRID)
		{
			try
			{
				if (userAllowView && globalAllowView)
				{
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL_AND_USER.Read(_dba,
                                                                                                                OLL_RID: aOllRID,
                                                                                                                USER_RID: aUserRID,
                                                                                                                customOLL_rid: customOllRID
                                                                                                                );

				}
				else if (!userAllowView && globalAllowView) // ONLY User
				{
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_GLOBAL.Read(_dba,
                                                                                                        OLL_RID: aOllRID,
                                                                                                        customOLL_rid: customOllRID
                                                                                                        );
				}
				else if (userAllowView && !globalAllowView) // ONLY Global
				{
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_USER.Read(_dba,
                                                                                             OLL_RID: aOllRID,
                                                                                             USER_RID: aUserRID,
                                                                                             customOLL_rid: customOllRID
                                                                                             );
				                                                                             
				}
                //else if (!userAllowView && !globalAllowView) // Neither Global or User
                else
				{
                    return StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_ALL_NONE.Read(_dba,
                                                                                             OLL_RID: aOllRID,
                                                                                             customOLL_rid: customOllRID
                                                                                             );
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public string OverrideLowLevelsModelWork_GetModelName(int aOllRID)
		{
			try
			{
                object ollName =  StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ_NAME.Read(_dba, OLL_RID: aOllRID);
				return ollName.ToString();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public int OverrideLowLevelsModelWork_GetModelKey(string aOllName)
        {
            DataTable dt;
            DataRow dr;
            try
            {
                dt = StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ.Read(_dba, NAME: aOllName);
                if (dt.Rows.Count == 0)
                {
                    return Include.NoRID;
                }
                else
                {
                    dr = dt.Rows[0];
                    return Convert.ToInt32(dr["OLL_RID"]);
                }
                
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Mod
        //public int OverrideLowLevelsModelWork_Add(string aName, int aHN_RID, int aHigh_Level_HN_RID, int aUser_RID,
        //                                      int aHighLevelSeq, int aHighLevelOffset, eHighLevelsType aHighLevelType,
        //                                      int aLowLevelSeq, int aLowLevelOffset, eLowLevelsType aLowLevelType, int clientUserRid)
        // End TT#988
        public int OverrideLowLevelsModelWork_Add(string aName, int aHN_RID, int aHigh_Level_HN_RID, int aUser_RID,
                                              int aHighLevelSeq, int aHighLevelOffset, eHighLevelsType aHighLevelType,
                                              int aLowLevelSeq, int aLowLevelOffset, eLowLevelsType aLowLevelType, int clientUserRid, bool aActiveOnlyInd)
        {

            int rndNum = RandomNumber(0, 99999);
            string rndString = rndNum.ToString("00000");
            string userString = clientUserRid.ToString("0000");
            string keyString = rndString + userString;
            int modelRid = Convert.ToInt32(keyString);
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_INSERT.Insert(_dba,
                                                                                 NAME: aName,
                                                                                 OLL_RID: modelRid,
                                                                                 HN_RID: aHN_RID,
                                                                                 HIGH_LEVEL_HN_RID: aHigh_Level_HN_RID,
                                                                                 HIGH_LEVEL_SEQ: aHighLevelSeq,
                                                                                 HIGH_LEVEL_OFFEST: aHighLevelOffset,
                                                                                 HIGH_LEVEL_TYPE: (int)aHighLevelType,
                                                                                 LOW_LEVEL_SEQ: aLowLevelSeq,
                                                                                 LOW_LEVEL_OFFEST: aLowLevelOffset,
                                                                                 LOW_LEVEL_TYPE: (int)aLowLevelType,
                                                                                 USER_RID: aUser_RID, //TT#1279-MD -jsobek -Save As changes ownership from global to user
                                                                                 ACTIVE_ONLY_IND: Include.ConvertBoolToChar(aActiveOnlyInd)
                                                                                 );       
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
			return modelRid;
        }

		private int RandomNumber(int min, int max)
		{
			Random random = new Random();
			return random.Next(min, max);
		}

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        //public void OverrideLowLevelsModelWork_Update(int aModelRID, string aName, int aHN_RID, int aHigh_Level_HN_RID, int aUser_RID, 
        //                                          int aHighLevelSeq, int aHighLevelOffset, eHighLevelsType aHighLevelType,
        //                                          int aLowLevelSeq, int aLowLevelOffset, eLowLevelsType aLowLevelType)
        public void OverrideLowLevelsModelWork_Update(int aModelRID, string aName, int aHN_RID, int aHigh_Level_HN_RID, int aUser_RID,
                                              int aHighLevelSeq, int aHighLevelOffset, eHighLevelsType aHighLevelType,
                                              int aLowLevelSeq, int aLowLevelOffset, eLowLevelsType aLowLevelType, bool aActiveOnlyInd)
        // End TT#988
        {
            try
            {
                

                StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_UPDATE.Update(_dba,
                                                                                NAME: aName,
                                                                                OLL_RID: aModelRID,
                                                                                HN_RID: aHN_RID,
                                                                                HIGH_LEVEL_HN_RID: aHigh_Level_HN_RID,
                                                                                HIGH_LEVEL_SEQ: aHighLevelSeq,
                                                                                HIGH_LEVEL_OFFEST: aHighLevelOffset,
                                                                                HIGH_LEVEL_TYPE: (int)aHighLevelType,
                                                                                LOW_LEVEL_SEQ: aLowLevelSeq,
                                                                                LOW_LEVEL_OFFEST: aLowLevelOffset,
                                                                                LOW_LEVEL_TYPE: (int)aLowLevelType,
                                                                                USER_RID: aUser_RID,
                                                                                ACTIVE_ONLY_IND: Include.ConvertBoolToChar(aActiveOnlyInd)
                                                                                );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

            //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
            if (ConnectionIsOpen)
            {
                SecurityAdmin sa = new SecurityAdmin(_dba);
                sa.DeleteUserItemByTypeAndRID((int)eProfileType.OverrideLowLevelModel, aModelRID);
                if (aUser_RID != Include.CustomUserRID)
                {
                    sa.AddUserItem(aUser_RID, (int)eProfileType.OverrideLowLevelModel, aModelRID, aUser_RID);
                }
            }
            //SecurityAdmin sa = new SecurityAdmin();
            //try
            //{
            //    sa.OpenUpdateConnection();
            //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.OverrideLowLevelModel, aModelRID);
            //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.OverrideLowLevelModel, aModelRID);
            //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    if (aUser_RID != Include.CustomUserRID)
            //    {
            //        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //        //sa.AddUserItem(aUser_RID, (int)eSharedDataType.OverrideLowLevelModel, aModelRID, aUser_RID);
            //        sa.AddUserItem(aUser_RID, (int)eProfileType.OverrideLowLevelModel, aModelRID, aUser_RID);
            //        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    }
            //    sa.CommitData();
            //}
            //catch
            //{
            //    throw;
            //}
            //finally
            //{
            //    sa.CloseUpdateConnection();
            //}
            //End TT#1564 - DOConnell - Missing Tasklist record prevents Login


            return;

        }

        public void OverrideLowLevelsModelWork_Delete(int aModelRID)
        {
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_WORK_DELETE.Delete(_dba, OLL_RID: aModelRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public DataTable OverrideLowLevelsDetailWork_Read(int aModelRID)
        {
            try
            {
                return StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ.Read(_dba, OLL_RID: aModelRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void OverrideLowLevelsDetailWork_Add(int aModelRID, int aHN_RID, int aVersionRID, bool aExcludeInd)
        {
            try
            {
                char aEXCLUDE_IND = '0';

				//==========================================
				// Determine whether to INSERT or UPDATE
				//==========================================
                int OVERRIDE_LL_MODEL_DETAIL_WORK_COUNT = StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_READ_COUNT.ReadRecordCount(_dba,
                                                                                              OLL_RID: aModelRID,
                                                                                              HN_RID: aHN_RID
                                                                                              );

                //Begin TT#1277-MD -jsobek -Saving new model after changing Exclude for merchandise gets database error
                //set forecast version to null if not specified
                int? VERSION_RID_Nullable = null;
                if (aVersionRID != Include.NoRID) VERSION_RID_Nullable = aVersionRID;
                //End TT#1277-MD -jsobek -Saving new model after changing Exclude for merchandise gets database error

                // UPDATE
                if (OVERRIDE_LL_MODEL_DETAIL_WORK_COUNT > 0) 
				{
  
                    if (aExcludeInd == true) aEXCLUDE_IND = '1';
                    
                    StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_UPDATE.Update(_dba,
                                                                                 OLL_RID: aModelRID,
                                                                                 HN_RID: aHN_RID,
                                                                                 VERSION_RID: VERSION_RID_Nullable, //TT#1277-MD -jsobek -Saving new model after changing Exclude for merchandise gets database error
                                                                                 EXCLUDE_IND: aEXCLUDE_IND
                                                                                 );
				}
				else // INSERT
				{
                   
                    if (aExcludeInd == true) aEXCLUDE_IND = '1';


                    StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_INSERT.Insert(_dba,
                                                                                 OLL_RID: aModelRID,
                                                                                 HN_RID: aHN_RID,
                                                                                 VERSION_RID: VERSION_RID_Nullable, //TT#1277-MD -jsobek -Saving new model after changing Exclude for merchandise gets database error
                                                                                 EXCLUDE_IND: aEXCLUDE_IND
                                                                                 );
				}
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



        public void OverrideLowLevelsDetailWork_Delete(int aModelRID, int aHN_RID)
        {
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE_FROM_NODE.Delete(_dba,
                                                                                           OLL_RID: aModelRID,
                                                                                           HN_RID: aHN_RID
                                                                                           );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void OverrideLowLevelsDetailWork_Delete(int aModelRID)
        {
            try
            {
                StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_WORK_DELETE.Delete(_dba, OLL_RID: aModelRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public int OverrideLowLevelsModelWork_CopyToPermanentTable(int aOllRID)
        {
            bool isUpdate = false;
            string name = string.Empty;
            int nodeRid = Include.NoRID;
            int highLevelNodeRid = Include.NoRID;
            int userRid = Include.NoRID;
            int LowLevelSeq = 0;
            int LowLevelOffset = 0;
            eLowLevelsType LowLevelType = eLowLevelsType.None;
            int HighLevelSeq = 0;
            int HighLevelOffset = 0;
            eHighLevelsType HighLevelType = eHighLevelsType.None;
            int newKey = aOllRID;
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            bool ActiveOnlyInd = false;
            // End TT#988

            try
            {
                //==========================================================================
                // Determine whether records exist on permanent tables. If so, delete them.
                //==========================================================================
          
                int OverrideLowLevelModelWorkCount = StoredProcedures.MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT.ReadRecordCount(_dba, OLL_RID: aOllRID);
                if (OverrideLowLevelModelWorkCount > 0) isUpdate = true;

                DataTable dt = OverrideLowLevelsModelWork_Read(aOllRID);

                if (dt.Rows.Count > 0)
                {
                    //===================
                    // Copy model HEADER
                    //===================
                    System.Data.DataRow dr = dt.Rows[0];
                    name = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                    nodeRid = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                    highLevelNodeRid = Convert.ToInt32(dr["HIGH_LEVEL_HN_RID"], CultureInfo.CurrentUICulture);
                    userRid = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
                    LowLevelSeq = Convert.ToInt32(dr["LOW_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                    LowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                    LowLevelType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                    HighLevelSeq = Convert.ToInt32(dr["HIGH_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                    HighLevelOffset = Convert.ToInt32(dr["HIGH_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                    HighLevelType = (eHighLevelsType)Convert.ToInt32(dr["HIGH_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    if (dr["ACTIVE_ONLY_IND"] == DBNull.Value)
                    {
                        ActiveOnlyInd = false;
                    }
                    else
                    {
                        ActiveOnlyInd = Include.ConvertCharToBool(Convert.ToChar(dr["ACTIVE_ONLY_IND"], CultureInfo.CurrentUICulture));
                    }
                    // End TT#988

                    if (isUpdate)
                    {
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        //OverrideLowLevelsModel_Update(aOllRID, name, nodeRid, highLevelNodeRid, userRid, HighLevelSeq, HighLevelOffset,
                        //    HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                        OverrideLowLevelsModel_Update(aOllRID, name, nodeRid, highLevelNodeRid, userRid, HighLevelSeq, HighLevelOffset,
                            HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, ActiveOnlyInd);
                        // End TT#988

                        OverrideLowLevelsDetail_Delete(aOllRID);
                        //===================
                        // COPY model DETAIL
                        //===================
                       
                        StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL.Insert(_dba, OLL_RID: aOllRID);

                    }
                    else
                    {
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        //newKey = OverrideLowLevelsModel_Add(name, nodeRid, highLevelNodeRid, userRid, HighLevelSeq, HighLevelOffset,
                        //    HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                        newKey = OverrideLowLevelsModel_Add(name, nodeRid, highLevelNodeRid, userRid, HighLevelSeq, HighLevelOffset,
                            HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, ActiveOnlyInd);
                        // End TT#988

                        //===================
                        // ADD model DETAIL
                        //===================

                        StoredProcedures.MID_OVERRIDE_LL_MODEL_DETAIL_INSERT_COPY_NEW_DETAIL_WORK_OVERRIDE_LL_MODEL_DETAIL.Insert(_dba,
                                                                                                                          OLL_RID: aOllRID,
                                                                                                                          NEW_OLL_RID: newKey
                                                                                                                          );
                    }
                }
                else
                {


                }
                return newKey;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        #endregion
    }

}
