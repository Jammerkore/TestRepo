using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    public partial class AssortmentViewData : DataLayer
    {
        public AssortmentViewData() : base()
        {
            
        }

        public DataTable AssortmentView_Read(ArrayList aUserRIDList, eAssortmentViewType viewType) //TT#1268-MD -jsobek -5.4 Merge
        {
            try
            {
                DataTable dtUserList = new DataTable();
                dtUserList.Columns.Add("USER_RID", typeof(int));

                for (int i = 0; i < aUserRIDList.Count; i++)
                {
                    if (dtUserList.Select("USER_RID=" + aUserRIDList[i].ToString()).Length == 0)
                    {
                        DataRow dr = dtUserList.NewRow();
                        dr["USER_RID"] = aUserRIDList[i];
                        dtUserList.Rows.Add(dr);
                    }
                }
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //return StoredProcedures.MID_ASSORTMENT_VIEW_READ_ALL.Read(_dba, USER_RID_LIST: dtUserList);
                return StoredProcedures.MID_ASSORTMENT_VIEW_READ_BY_TYPE.Read(_dba, USER_RID_LIST: dtUserList, ASSORTMENT_VIEW_TYPE: (int)viewType);
                //End TT#1268-MD -jsobek -5.4 Merge
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		public DataRow AssortmentView_Read(int aViewRID)
		{
			DataTable dt;

			try
			{
                dt = StoredProcedures.MID_ASSORTMENT_VIEW_READ_VIEW_RID.Read(_dba, VIEW_RID: aViewRID);

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable AssortmentView_ReadPostReceipt(ArrayList aUserRIDList)
		{
			string userList = string.Empty;
			try
			{
				userList = BuildUserList(aUserRIDList);
                return StoredProcedures.SP_MID_GET_POST_RECEIPT_ASRT_VIEWS.Read(_dba, USER_RID_LIST: userList);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
		// Begin TT#4247 - stodd - Store attribute not being saved in matrix view
        public int AssortmentView_Insert(int aUserRID, string aViewID, eAllocationAssortmentViewGroupBy aAsstGroupBy, int aAssortSgRid, int theClientUserRid, eAssortmentViewType viewType) //TT#1268-MD -jsobek -5.4 Merge
		// End TT#4247 - stodd - Store attribute not being saved in matrix view
		// END TT#359-MD - stodd - add GroupBy to Asst View
        {

			// BEGIN TT#402-MD - stodd - view getting reset to "Default"
			int viewRid = Include.NoRID;
			// END TT#402-MD - stodd - view getting reset to "Default"
            try
            {


                viewRid = StoredProcedures.SP_MID_ASSORTMENT_VIEW_INSERT.InsertAndReturnRID(_dba,
                                                                            USER_RID: aUserRID,
                                                                            VIEW_ID: aViewID,
                                                                            GROUP_BY_ID: (int)aAsstGroupBy,
                                                                            SG_RID: aAssortSgRid,	// TT#4247 - stodd - Store attribute not being saved in matrix view
                                                                            ASSORTMENT_VIEW_TYPE: (int)viewType //TT#1268-MD -jsobek -5.4 Merge
                                                                            );

                StoredProcedures.MID_USER_ASSORTMENT_UPDATE.Update(_dba,
                                                                   VIEW_RID: viewRid,
                                                                   USER_RID: theClientUserRid
                                                                   );

				return viewRid;
				// END TT#402-MD - stodd - view getting reset to "Default"
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#594-MD - stodd - Saving Assortment "Default View" gets a foreign key constraint
		// Begin TT#4247 - stodd - Store attribute not being saved in matrix view
        public void AssortmentView_Update(int aViewRid, int aViewUserRID, string aViewID, eAllocationAssortmentViewGroupBy aAsstGroupBy, int aSgRid, eAssortmentViewType viewType)  // TT#1077 - MD - stodd - cannot create GA views -
		// End TT#4247 - stodd - Store attribute not being saved in matrix view
        {
            try
            {
                //Begin TT#1268-MD -jsobek -5.4 Merge
                //// Begin TT#1077 - MD - stodd - cannot create GA views -
                //string SQLCommand = "UPDATE ASSORTMENT_VIEW SET "
                //    + "USER_RID=@USER_RID,"
                //    + "VIEW_ID=@VIEW_ID,"
                //    + "GROUP_BY_ID=@GROUP_BY_ID,"
                //    + "ASSORTMENT_VIEW_TYPE=@ASSORTMENT_VIEW_TYPE"
                //    + " WHERE VIEW_RID = @VIEW_RID";

                //MIDDbParameter[] InParams = {   new MIDDbParameter("@VIEW_RID", aViewRid, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@USER_RID", aViewUserRID, eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@VIEW_ID", aViewID, eDbType.Text, eParameterDirection.Input),
                //                              new MIDDbParameter("@GROUP_BY_ID", Convert.ToInt32(aAsstGroupBy), eDbType.Int, eParameterDirection.Input),
                //                              new MIDDbParameter("@ASSORTMENT_VIEW_TYPE", Convert.ToInt32(viewType), eDbType.Int, eParameterDirection.Input)
                //                          };
                //// End TT#1077 - MD - stodd - cannot create GA views -
                //_dba.ExecuteNonQuery(SQLCommand, InParams);


                StoredProcedures.MID_ASSORTMENT_VIEW_UPDATE.Update(_dba,
                                                                 VIEW_RID: aViewRid,
                                                                 USER_RID: aViewUserRID,
                                                                 VIEW_ID: aViewID,
                                                                 GROUP_BY_ID: Convert.ToInt32(aAsstGroupBy),
                                                                 SG_RID: aSgRid,	// TT#4247 - stodd - Store attribute not being saved in matrix view
                                                                 ASSORTMENT_VIEW_TYPE: Convert.ToInt32(viewType)
                                                                 );
                //End TT#1268-MD -jsobek -5.4 Merge
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#594-MD - stodd - Saving Assortment "Default View" gets a foreign key constraint

        public int AssortmentView_GetKey(int aUserRID, string aViewID, eAssortmentViewType aViewType)	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
        {
            DataTable dt;

            try
            {
                dt = StoredProcedures.MID_ASSORTMENT_VIEW_READ_GET_KEY.Read(_dba,
                                                                              USER_RID: aUserRID,
                                                                              VIEW_ID: aViewID,
                                                                              ASSORTMENT_VIEW_TYPE: aViewType	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.
				                                                              );

                if (dt.Rows.Count == 1)
                {
                    return (Convert.ToInt32(dt.Rows[0]["VIEW_RID"], CultureInfo.CurrentUICulture));
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable AssortmentViewDetail_Read(int aViewRID)
        {
            try
            {
                return StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_READ.Read(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void AssortmentViewDetail_Insert(
			int aViewRID,
			ArrayList aComponentHeaders,
			ArrayList aSummaryColHeaders,
			ArrayList aTotalColHeaders,
			ArrayList aDetailColHeaders,
			ArrayList aDetailRowHeaders)
        {
            try
            {
				foreach (RowColProfileHeader header in aComponentHeaders)
                {
					if (header.IsDisplayed)
                    {

                        StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                                VIEW_RID: aViewRID,
                                                                                AXIS: (int)eAssortmentViewAxis.Component,
                                                                                AXIS_SEQUENCE: header.Sequence,
                                                                                PROFILE_TYPE: (int)header.Profile.ProfileType,
                                                                                PROFILE_KEY: header.Profile.Key,
                                                                                SUMMARIZED_IND: Include.ConvertBoolToChar(header.IsSummarized)
                                                                                );
                    }
                }

				foreach (RowColProfileHeader header in aSummaryColHeaders)
                {
					if (header.IsDisplayed)
                    {
                        StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                            VIEW_RID: aViewRID,
                                                                            AXIS: (int)eAssortmentViewAxis.SummaryColumn,
                                                                            AXIS_SEQUENCE: header.Sequence,
                                                                            PROFILE_TYPE: (int)header.Profile.ProfileType,
                                                                            PROFILE_KEY: header.Profile.Key,
                                                                            SUMMARIZED_IND: Include.ConvertBoolToChar(header.IsSummarized)
                                                                            );
                    }
                }

				foreach (RowColProfileHeader header in aTotalColHeaders)
				{
					if (header.IsDisplayed)
					{
                        StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                        VIEW_RID: aViewRID,
                                                                        AXIS: (int)eAssortmentViewAxis.TotalColumn,
                                                                        AXIS_SEQUENCE: header.Sequence,
                                                                        PROFILE_TYPE: (int)header.Profile.ProfileType,
                                                                        PROFILE_KEY: header.Profile.Key,
                                                                        SUMMARIZED_IND: Include.ConvertBoolToChar(header.IsSummarized)
                                                                        );
					}
				}

				foreach (RowColProfileHeader header in aDetailColHeaders)
				{
					if (header.IsDisplayed)
					{
                        StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_INSERT.Insert(_dba,
                                                        VIEW_RID: aViewRID,
                                                        AXIS: (int)eAssortmentViewAxis.DetailColumn,
                                                        AXIS_SEQUENCE: header.Sequence,
                                                        PROFILE_TYPE: (int)header.Profile.ProfileType,
                                                        PROFILE_KEY: header.Profile.Key,
                                                        SUMMARIZED_IND: Include.ConvertBoolToChar(header.IsSummarized)
                                                        );
					}
				}

				foreach (RowColProfileHeader header in aDetailRowHeaders)
				{
					if (header.IsDisplayed)
					{
                        StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                                VIEW_RID: aViewRID,
                                                                                AXIS: (int)eAssortmentViewAxis.DetailRow,
                                                                                AXIS_SEQUENCE: header.Sequence,
                                                                                PROFILE_TYPE: (int)header.Profile.ProfileType,
                                                                                PROFILE_KEY: header.Profile.Key,
                                                                                SUMMARIZED_IND: Include.ConvertBoolToChar(header.IsSummarized)
                                                                                );
				                                                              
					}
				}
			}
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#1181 - RMatelic - Assortment View Maintenance
        public void AssortmentView_Delete(int aViewRID, eAssortmentViewType viewType)	// TT#1077 - MD - stodd - cannot create GA views  //TT#1268-MD -jsobek -5.4 Merge
        {
            try
            {
                int DEFAULT_VIEW_RID;
                if (viewType == eAssortmentViewType.Assortment)
                {
                    DEFAULT_VIEW_RID = Include.DefaultAssortmentViewRID;
                }
                else
                {
                    DEFAULT_VIEW_RID = Include.DefaultGroupAllocationViewRID;
                }

                //_dba.ExecuteNonQuery("UPDATE ASSORTMENT_USER_VIEW_JOIN SET VIEW_RID = @DefaultViewRID WHERE VIEW_RID = @ViewRID", inParams);


                StoredProcedures.MID_USER_ASSORTMENT_UPDATE_FOR_VIEW_DELETE.Update(_dba,
                                                                                   VIEW_RID: aViewRID,
                                                                                   DEFAULT_VIEW_RID: DEFAULT_VIEW_RID
                                                                                   );  //TT#1268-MD -jsobek -5.4 Merge -Modified to update ASSORTMENT_USER_VIEW_JOIN as well

                AssortmentViewDetail_Delete(aViewRID);
                StoredProcedures.MID_ASSORTMENT_VIEW_DELETE.Delete(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }    
        // End TT#1181  

        public void AssortmentViewDetail_Delete(int aViewRID)
        {
            try
            {
                StoredProcedures.MID_ASSORTMENT_VIEW_DETAIL_DELETE.Delete(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private string BuildUserList(ArrayList aUserRIDList)
        {
            string SQLCommand = "";
            int users;

            try
            {
                if (aUserRIDList.Count == 1)
                {
                    SQLCommand += "USER_RID = " + Convert.ToString(aUserRIDList[0], CultureInfo.CurrentUICulture);
                }
                else
                {
                    SQLCommand += "USER_RID IN (";

                    users = 0;
                    foreach (int userRID in aUserRIDList)
                    {
                        if (users > 0)
                        {
                            SQLCommand += ",";
                        }

                        SQLCommand += Convert.ToString(userRID, CultureInfo.CurrentUICulture);
                        users++;
                    }
                    SQLCommand += ")";
                }

                return SQLCommand;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }
}
