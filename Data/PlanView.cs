using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class PlanViewData : DataLayer
	{
        public PlanViewData() : base()
        {
        }

		public DataTable PlanView_Read(ArrayList aUserRIDList)
		{
			try
			{
                return StoredProcedures.MID_PLAN_VIEW_READ_FROM_USER_LIST.Read(_dba, USER_LIST: BuildUserListAsDataset(aUserRIDList));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	
		public int PlanView_Insert(int aUserRID, string aViewID, eStorePlanSelectedGroupBy aGroupBy)
		{
			try
			{
                int viewRID = StoredProcedures.SP_MID_PLAN_VIEW_INSERT.InsertAndReturnRID(_dba,
                                                                                          USER_RID: aUserRID,
                                                                                          VIEW_ID: aViewID,
                                                                                          GROUPBY_TYPE: (int)aGroupBy
                                                                                          );

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(aUserRID, (int)eProfileType.PlanView, viewRID, aUserRID);
                }

                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.AddUserItem(aUserRID, (int)eSharedDataType.PlanView, viewRID, aUserRID);
                //    sa.AddUserItem(aUserRID, (int)eProfileType.PlanView, viewRID, aUserRID);
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

				return viewRID;
				//End Track #4815
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int PlanView_Delete(int aViewRID)
		{
			try
			{
				this.PlanViewDetail_Delete(aViewRID);
                this.PlanViewFormat_Delete(aViewRID);
			}
			catch
			{
				return -1;
			}
			

            StoredProcedures.MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION.Update(_dba, VIEW_RID: aViewRID);
            int result = StoredProcedures.MID_PLAN_VIEW_DELETE.Delete(_dba, VIEW_RID: aViewRID);

            //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
            if (ConnectionIsOpen)
            {
                SecurityAdmin sa = new SecurityAdmin(_dba);
                sa.DeleteUserItemByTypeAndRID((int)eProfileType.PlanView, aViewRID);
            }

            //SecurityAdmin sa = new SecurityAdmin();
            //try
            //{
            //    sa.OpenUpdateConnection();
            //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
            //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.PlanView, aViewRID);
            //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.PlanView, aViewRID);
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

			return result;
			//End Track #4815
		}

		public int PlanView_GetKey(int aUserRID, string aViewID)
		{
			DataTable dt;

			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                dt = StoredProcedures.MID_PLAN_VIEW_READ_KEY.Read(_dba,
                                                                    USER_RID: aUserRID,
                                                                    VIEW_ID: aViewID
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

		public DataTable PlanViewDetail_Read(int aViewRID)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_PLAN_VIEW_DETAIL_READ.Read(_dba, VIEW_RID: aViewRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		
		public void PlanViewDetail_Insert(int aViewRID, ArrayList aVariableHeaders, ArrayList aQuantityHeaders, ArrayList aPeriodHeaders)
		{
			try
			{
				foreach (RowColProfileHeader varHeader in aVariableHeaders)
				{
					if (varHeader.IsDisplayed)
					{
                        StoredProcedures.MID_PLAN_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                    VIEW_RID: aViewRID,
                                                                    AXIS: (int)eViewAxis.Variable,
                                                                    AXIS_SEQUENCE: varHeader.Sequence,
                                                                    PROFILE_TYPE: (int)varHeader.Profile.ProfileType,
                                                                    PROFILE_KEY: varHeader.Profile.Key
                                                                    );
					}
				}

				foreach (RowColProfileHeader quantHeader in aQuantityHeaders)
				{
					if (quantHeader.IsDisplayed)
					{
                        StoredProcedures.MID_PLAN_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                    VIEW_RID: aViewRID,
                                                                    AXIS: (int)eViewAxis.Quantity,
                                                                    AXIS_SEQUENCE: quantHeader.Sequence,
                                                                    PROFILE_TYPE: (int)quantHeader.Profile.ProfileType,
                                                                    PROFILE_KEY: quantHeader.Profile.Key
                                                                    );
					}
				}

				foreach (RowColProfileHeader perHeader in aPeriodHeaders)
				{
					if (perHeader.IsDisplayed)
					{
                        StoredProcedures.MID_PLAN_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                    VIEW_RID: aViewRID,
                                                                    AXIS: (int)eViewAxis.Period,
                                                                    AXIS_SEQUENCE: perHeader.Sequence,
                                                                    PROFILE_TYPE: (int)eProfileType.Period,
                                                                    PROFILE_KEY: 0
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
		//End Track #5121 - JScott - Add Year/Season/Quarter totals

		public void PlanViewDetail_Delete(int aViewRID)
		{
			try
			{
                StoredProcedures.MID_PLAN_VIEW_DETAIL_DELETE.Delete(_dba, VIEW_RID: aViewRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        public DataTable PlanViewFormat_Read(int aViewRID)
        {
            try
            {
                return StoredProcedures.MID_PLAN_VIEW_FORMAT_READ.Read(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        public void PlanViewFormat_Insert(
            int aViewRID, 
            int planBasisType = Include.Undefined, 
            int variableNumber = Include.Undefined, 
            int quantityVariableKey = Include.Undefined,
            int timePeriodType = Include.Undefined,
            int timePeriodKey = Include.Undefined,
            int variableTotalKey = Include.Undefined,
            int width = Include.DefaultColumnWidth
            )
        {
            try
            {
                StoredProcedures.MID_PLAN_VIEW_FORMAT_INSERT.Insert(_dba,
                                                                    VIEW_RID: aViewRID,
                                                                    PLAN_BASIS_TYPE: planBasisType,
                                                                    VARIABLE_NUMBER: variableNumber,
                                                                    QUANTITY_VARIABLE_KEY: quantityVariableKey,
                                                                    TIME_PERIOD_TYPE: timePeriodType,
                                                                    TIME_PERIOD_KEY: timePeriodKey,
                                                                    VARIABLE_TOTAL_KEY: variableTotalKey,
                                                                    WIDTH: width
                                                                    );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void PlanViewFormat_Delete(int aViewRID)
        {
            try
            {
                StoredProcedures.MID_PLAN_VIEW_FORMAT_DELETE.Delete(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public DataTable PlanViewSplitter_Read(
            int viewRID,
            int userRID,
            ePlanSessionType planSessionType)
        {
            try
            {
                return StoredProcedures.MID_PLAN_VIEW_SPLITTER_READ.Read(_dba, 
                    VIEW_RID:viewRID,
                    USER_RID: userRID,
                    PLAN_SESSION_TYPE: (int)planSessionType
                    );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        public void PlanViewSplitter_Insert(
            int viewRID,
            int userRID,
            ePlanSessionType planSessionType = ePlanSessionType.None,
            char splitterTypeIndicator = 'V',
            int splitterSequence = Include.Undefined,
            double splitterPercentage = 0
            )
        {
            try
            {
                StoredProcedures.MID_PLAN_VIEW_SPLITTER_INSERT.Insert(_dba,
                                                                    VIEW_RID: viewRID,
                                                                    USER_RID: userRID,
                                                                    PLAN_SESSION_TYPE: (int)planSessionType,
                                                                    SPLITTER_TYPE_IND: splitterTypeIndicator,
                                                                    SPLITTER_SEQUENCE: splitterSequence,
                                                                    SPLITTER_PERCENTAGE: splitterPercentage
                                                                    );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

        }

        public void PlanViewSplitter_Delete(
            int viewRID,
            int userRID,
             ePlanSessionType planSessionType
            )
        {
            try
            {
                StoredProcedures.MID_PLAN_VIEW_SPLITTER_DELETE.Delete(
                    _dba, 
                    VIEW_RID: viewRID,
                    USER_RID: userRID,
                    PLAN_SESSION_TYPE: (int)planSessionType
                    );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private DataTable BuildUserListAsDataset(ArrayList aValueList)
        {
            DataTable dtUserList = null;
            if (aValueList != null)
            {
                dtUserList = new DataTable();
                dtUserList.Columns.Add("USER_RID", typeof(int));
                foreach (int userRID in aValueList)
                {
                    //ensure userRIDs are distinct, and only added to the datatable one time
                    if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0)
                    {
                        DataRow dr = dtUserList.NewRow();
                        dr["USER_RID"] = userRID;
                        dtUserList.Rows.Add(dr);
                    }
                }
            }
            return dtUserList;
        }

	}
}
