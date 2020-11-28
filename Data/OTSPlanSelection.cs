using System;
using System.Diagnostics;
using System.Data;
using System.Text;
using MIDRetail.DataCommon;
using System.ComponentModel;
using System.Globalization;

namespace MIDRetail.Data
{
	public partial class OTSPlanSelection: DataLayer
	{
		public OTSPlanSelection() : base()
		{
        }

		#region Save Plan Selection

		public void SavePlanSelection(int aUserRID, PlanOpenParmsData aOpenParms, DataSet dsSource)
		{
			try
			{
				_dba.OpenUpdateConnection();
			
				try
				{
					//Delete the database records first. We're saving everything anew.
					DeleteSelectionData(aUserRID);
					DeleteBasisDetails(aUserRID);
					DeleteBasis(aUserRID);

					//Save the Plan Selection Data.
					InsertSelectionData(aUserRID, aOpenParms);

					//Loop through the Basis table and add everything to db.
					for (int BasisID = 0; BasisID < dsSource.Tables["Basis"].Rows.Count; BasisID++)
					{
						//Since the underlying BasisID might not be increments of 1 all the 
						//way (maybe user did lots of inserts and deletes and thus created gaps), 
						//we will manually assign the BasisID regardless what was on the GUI.
						InsertBasis(aUserRID, BasisID);

						//Get all children (BasisDetails rows that have matching IDs)
						DataView dv = new DataView();
						dv.Table = dsSource.Tables["BasisDetails"];
						dv.RowFilter = "BasisID = " + dsSource.Tables["Basis"].Rows[BasisID]["BasisID"];

						//Loop through the Basis Details table and save everything to db.
						for (int seq = 0; seq < dv.Count; seq++)
						{
							InsertBasisDetails(aUserRID, BasisID, seq, dv[seq]);
						}
					}

					_dba.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					_dba.RollBack();
					throw;
				}
				finally
				{
					_dba.CloseUpdateConnection();
				}

				return;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DeleteSelectionData(int aUserRID)
		{
			try
			{
                StoredProcedures.MID_USER_PLAN_DELETE_FORM_USER.Delete(_dba, USER_RID: aUserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DeleteBasisDetails(int aUserRID)
		{
			try
			{
                StoredProcedures.MID_USER_PLAN_BASIS_DETAILS_DELETE_FORM_USER.Delete(_dba, USER_RID: aUserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DeleteBasis(int aUserRID)
		{
			try
			{
                StoredProcedures.MID_USER_PLAN_BASIS_DELETE_FORM_USER.Delete(_dba, USER_RID: aUserRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void InsertSelectionData(int aUserRID, PlanOpenParmsData aOpenParms)
		{
			try
			{
				//insert the user plan, which contains the main selection data.
              

                int? SG_RID_Nullable = null;
                if (aOpenParms.StoreGroupRID != Include.NoRID) SG_RID_Nullable = aOpenParms.StoreGroupRID;

                int? VIEW_RID_Nullable = null;
                if (aOpenParms.ViewRID != Include.NoRID) VIEW_RID_Nullable = aOpenParms.ViewRID;

                int? STORE_HN_RID_Nullable = null;
                if (aOpenParms.StoreHLPlanProfile.NodeProfile.Key != Include.NoRID) STORE_HN_RID_Nullable = aOpenParms.StoreHLPlanProfile.NodeProfile.Key;

                int? STORE_FV_RID_Nullable = null;
                if (aOpenParms.StoreHLPlanProfile.VersionProfile != null && aOpenParms.StoreHLPlanProfile.VersionProfile.Key != Include.NoRID) STORE_FV_RID_Nullable = aOpenParms.StoreHLPlanProfile.VersionProfile.Key;

                int? TIME_PERIOD_CDR_RID_Nullable = null;
                if (aOpenParms.DateRangeProfile.Key != Include.NoRID) TIME_PERIOD_CDR_RID_Nullable = aOpenParms.DateRangeProfile.Key;

                int? CHAIN_HN_RID_Nullable = null;
                if (aOpenParms.ChainHLPlanProfile.NodeProfile.Key != Include.NoRID) CHAIN_HN_RID_Nullable = aOpenParms.ChainHLPlanProfile.NodeProfile.Key;

                int? CHAIN_FV_RID_Nullable = null;
                if (aOpenParms.ChainHLPlanProfile.VersionProfile != null && aOpenParms.ChainHLPlanProfile.VersionProfile.Key != Include.NoRID) CHAIN_FV_RID_Nullable = aOpenParms.ChainHLPlanProfile.VersionProfile.Key;

                int? LOW_LEVEL_FV_RID_Nullable = null;
                if (aOpenParms.LowLevelVersionDefault != null && aOpenParms.LowLevelVersionDefault.Key != Include.NoRID) LOW_LEVEL_FV_RID_Nullable = aOpenParms.LowLevelVersionDefault.Key;

                int? OLL_RID_Nullable = null;
                if (aOpenParms.OverrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = aOpenParms.OverrideLowLevelRid;

                int? CUSTOM_OLL_RID_Nullable = null;
                if (aOpenParms.CustomOverrideLowLevelRid != Include.NoRID) CUSTOM_OLL_RID_Nullable = aOpenParms.CustomOverrideLowLevelRid;

                StoredProcedures.MID_USER_PLAN_INSERT.Insert(_dba,
                                                             USER_RID: aUserRID,
                                                             SG_RID: SG_RID_Nullable,
                                                             FILTER_RID: aOpenParms.FilterRID,
                                                             GROUP_BY_ID: Convert.ToInt32(aOpenParms.GroupBy, CultureInfo.CurrentUICulture),
                                                             VIEW_RID: VIEW_RID_Nullable,
                                                             STORE_HN_RID: STORE_HN_RID_Nullable,
                                                             STORE_FV_RID: STORE_FV_RID_Nullable,
                                                             TIME_PERIOD_CDR_RID: TIME_PERIOD_CDR_RID_Nullable,
                                                             DISPLAY_TIME_BY_ID: Convert.ToInt32(aOpenParms.DisplayTimeBy, CultureInfo.CurrentUICulture),
                                                             CHAIN_HN_RID: CHAIN_HN_RID_Nullable,
                                                             CHAIN_FV_RID: CHAIN_FV_RID_Nullable,
                                                             INCLUDE_INELIGIBLE_STORES_IND: Include.ConvertBoolToChar(aOpenParms.IneligibleStores),
                                                             INCLUDE_SIMILAR_STORES_IND: Include.ConvertBoolToChar(aOpenParms.SimilarStores),
                                                             SESSION_TYPE: Convert.ToInt32(aOpenParms.PlanSessionType, CultureInfo.CurrentUICulture),
                                                             LOW_LEVEL_FV_RID: LOW_LEVEL_FV_RID_Nullable,
                                                             LOW_LEVEL_TYPE: Convert.ToInt32(aOpenParms.LowLevelsType, CultureInfo.CurrentUICulture),
                                                             LOW_LEVEL_OFFSET: aOpenParms.LowLevelsOffset,
                                                             LOW_LEVEL_SEQUENCE: aOpenParms.LowLevelsSequence,
                                                             CALC_MODE: aOpenParms.ComputationsMode,
                                                             OLL_RID: OLL_RID_Nullable,
                                                             CUSTOM_OLL_RID: CUSTOM_OLL_RID_Nullable,
                                                             IS_LADDER: Convert.ToInt32(aOpenParms.IsLadder),
                                                             TOT_RIGHT: Convert.ToInt32(aOpenParms.IsTotRT),
                                                             IS_MULTI: Convert.ToInt32(aOpenParms.IsMulti)
                                                             );
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void InsertBasis(int aUserID, int aBasisID)
		{
			try
			{
				//insert the user plan basis
                StoredProcedures.MID_USER_PLAN_BASIS_INSERT.Insert(_dba,
                                                                   USER_RID: aUserID,
                                                                   BASIS_RID: aBasisID
                                                                   );
				                                                   
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void InsertBasisDetails(int aUserID, int aBasisID, int aSeq, DataRowView aRowView)
		{
			try
			{
                int? HN_RID_Nullable = null;
                if (aRowView["MerchandiseID"] != DBNull.Value) HN_RID_Nullable = Convert.ToInt32(aRowView["MerchandiseID"], CultureInfo.CurrentUICulture);

                int? FV_RID_Nullable = null;
                if (aRowView["VersionID"] != DBNull.Value) FV_RID_Nullable = Convert.ToInt32(aRowView["VersionID"], CultureInfo.CurrentUICulture);

                int? CDR_RID_Nullable = null;
                if (aRowView["DateRangeID"] != DBNull.Value) CDR_RID_Nullable = Convert.ToInt32(aRowView["DateRangeID"], CultureInfo.CurrentUICulture);

                float? WEIGHT_Nullable = null;
                if (aRowView["Weight"] != DBNull.Value) WEIGHT_Nullable = Convert.ToSingle(aRowView["Weight"], CultureInfo.CurrentUICulture);
                StoredProcedures.MID_USER_PLAN_BASIS_DETAILS_INSERT.Insert(_dba,
                                                                           USER_RID: aUserID,
                                                                           BASIS_RID: aBasisID,
                                                                           SEQ_ID: aSeq,
                                                                           HN_RID: HN_RID_Nullable,
                                                                           FV_RID: FV_RID_Nullable,
                                                                           CDR_RID: CDR_RID_Nullable,
                                                                           WEIGHT: WEIGHT_Nullable,
                                                                           IS_INCLUDED_IND: Include.ConvertBoolToChar(Convert.ToBoolean(aRowView["IsIncluded"], CultureInfo.CurrentUICulture))
                                                                           );
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Updates Custom OLL rid behind the scenes for OTS selection
		/// </summary>
		/// <param name="methodRid"></param>
		/// <param name="customOLLRid"></param>
		public void UpdateUserPlanCustomOLLRid(TransactionData td, int userRid, int customOLLRid)
		{
			try
			{
                int rowCount = StoredProcedures.MID_USER_PLAN_READ_COUNT.ReadRecordCount(_dba, USER_RID: userRid);
                bool rowExists = (rowCount > 0);

				if (rowExists)
				{
                    int? CUSTOM_OLL_RID_Nullable = null;
                    if (customOLLRid != Include.NoRID) CUSTOM_OLL_RID_Nullable = customOLLRid;

                    StoredProcedures.MID_USER_PLAN_UPDATE_CUSTOM_OLL.Update(td.DBA,
                                                                        USER_RID: userRid,
                                                                        CUSTOM_OLL_RID: CUSTOM_OLL_RID_Nullable
                                                                        );
				}
			}
			catch (Exception ex)
			{
                string s = ex.ToString();
				throw;
			}
		}

   

		#endregion

		#region Get Plan Selection

		public DataTable GetPlanSelectionMainInfo(int UserID)
		{
			try
			{
                return StoredProcedures.MID_USER_PLAN_READ.Read(_dba, USER_RID: UserID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataSet GetPlanSelectionBasis(int UserID)
		{
			DataSet ds;
			DataTable dtBasis;
			DataTable dtDetails;
			StringBuilder stmt;

			try
			{
				ds = MIDEnvironment.CreateDataSet();

				dtBasis = SetupBasisTable();
				dtDetails = SetupBasisDetailsTable();

                dtBasis = StoredProcedures.MID_USER_PLAN_BASIS_READ.Read(_dba, USER_RID: UserID);
					
                dtDetails = StoredProcedures.MID_USER_PLAN_BASIS_DETAILS_READ.Read(_dba, USER_RID: UserID);

				TouchUpBasisTable(dtBasis);
				TouchUpDetailsTable(dtDetails);

				ds.Tables.Add(dtBasis);
				ds.Tables.Add(dtDetails);

				return ds;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataTable SetupBasisTable()
		{
			DataTable dt;

			try
			{
				dt = MIDEnvironment.CreateDataTable("Basis");
				dt.Columns.Add("BasisID", System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("BasisName", System.Type.GetType("System.String"));

				return dt;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataTable SetupBasisDetailsTable()
		{
			DataTable dt;

			try
			{
				dt = MIDEnvironment.CreateDataTable("BasisDetails");
			
				dt.Columns.Add("BasisID",			System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("Merchandise",		System.Type.GetType("System.String"));
				dt.Columns.Add("MerchandiseID",	System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("Version",			System.Type.GetType("System.String"));
				dt.Columns.Add("VersionID",		System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("DateRange",		System.Type.GetType("System.String"));
				dt.Columns.Add("DateRangeID",		System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("Picture",			System.Type.GetType("System.String"));	//picture column
				dt.Columns.Add("Weight",			System.Type.GetType("System.Decimal"));
				dt.Columns.Add("IsIncluded",		System.Type.GetType("System.Boolean")); //this column will be hidden. We'll use the buttons column for display.
				dt.Columns.Add("IncludeButton",	System.Type.GetType("System.String")); //button column for include/exclude

				return dt;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void TouchUpBasisTable(DataTable dt)
		{
			int basisID;

			try
			{
				dt.TableName = "Basis";
				dt.Columns[0].ColumnName = "BasisID";
				dt.Columns[1].ColumnName = "BasisName";

				foreach(DataRow dr in dt.Rows)
				{
					basisID = (Convert.ToInt32(dr["BasisID"], CultureInfo.CurrentUICulture) + 1);
					dr["BasisName"] = "Basis " + basisID.ToString(CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void TouchUpDetailsTable(DataTable dt)
		{
			try
			{
				dt.TableName = "BasisDetails";
				dt.Columns[0].ColumnName = "BasisID"		;
				dt.Columns[1].ColumnName = "Merchandise"	;
				dt.Columns[2].ColumnName = "MerchandiseID";
				dt.Columns[3].ColumnName = "Version"		;
				dt.Columns[4].ColumnName = "VersionID"	;
				dt.Columns[5].ColumnName = "DateRange"	;
				dt.Columns[6].ColumnName = "DateRangeID"	;
				dt.Columns[7].ColumnName = "Picture"		;
				dt.Columns[8].ColumnName = "Weight"		;
				dt.Columns[9].ColumnName = "IsIncluded"	;
				dt.Columns[10].ColumnName = "IncludeButton";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		#region Build dataset for the actual plan data (the full grid)

		public DataSet BuildPlanData(OTSPlanSelectionData PlanSelection, DataSet dsBasis)
		{
			DataSet dsPlan;

			try
			{
				dsPlan = MIDEnvironment.CreateDataSet();
				return dsPlan;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		public DataTable GetDistinctChainNodeUsersSelection(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_USER_PLAN_READ_DISTINCT_CHAIN.Read(_dba, CHAIN_HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetDistinctStoreNodeUsersSelection(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_USER_PLAN_READ_DISTINCT_STORE.Read(_dba, STORE_HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetDistinctBasisNodeUsersSelection(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_USER_PLAN_READ_DISTINCT_CHAIN.Read(_dba, CHAIN_HN_RID: aNodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
