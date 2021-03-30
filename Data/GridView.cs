using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using Infragistics.Win.UltraWinGrid;
using C1.Win.C1FlexGrid;
using MIDRetail.DataCommon;

using MIDRetail.Data; //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
namespace MIDRetail.Data
{
    public partial class GridViewData : DataLayer
    {
        public GridViewData() : base()
		{

		}
      
        public DataTable GridView_Read(int aLayoutID, ArrayList aUserRIDList)
        {
            return GridView_Read(aLayoutID, aUserRIDList, false);
        }

        public DataTable GridView_Read(int aLayoutID, ArrayList aUserRIDList, bool aAddGlobalUserLabel)
        {
            try
            {

                DataTable dtUserList = null;
                if (aUserRIDList != null)
                {
                    dtUserList = new DataTable();
                    dtUserList.Columns.Add("USER_RID", typeof(int));
                    foreach (int userRID in aUserRIDList)
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

                if (aAddGlobalUserLabel)
                {
                    return StoredProcedures.MID_GRID_VIEW_READ_WITH_GLOBAL_USER.Read(_dba,
                                                                                 LAYOUT_ID: aLayoutID,
                                                                                 USER_RID_LIST: dtUserList
                                                                                 );
                }
                else
                {
                    return StoredProcedures.MID_GRID_VIEW_READ_FROM_USERS.Read(_dba,
                                                                           LAYOUT_ID: aLayoutID,
                                                                           USER_RID_LIST: dtUserList
                                                                           );
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#1117

    

		public DataRow GridView_Read(int aViewRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_GRID_VIEW_READ.Read(_dba, VIEW_RID: aViewRID);

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

        public int GridView_Insert(int aUserRID, int aLayoutID, string aViewID, bool aShowDetails, int aGroupBy, int aGroupBySecondary, bool aIsSequential, int workspaceFilterRID, bool aUseFilterSorting)  //TT#1313-MD -jsobek -Header Filter
        {
            try
            {
                //object groupBy = null;
                //object groupBySecondary = null;
                //object isSequential = null;
                int? groupBy = null;
                int? groupBySecondary = null;
                char? isSequential = null;
                if (aGroupBy > Include.NoRID)
                {
                    groupBy = aGroupBy;
                }
                if (aGroupBySecondary > Include.NoRID)
                {
                    groupBySecondary = aGroupBySecondary;
                    isSequential = Include.ConvertBoolToChar(aIsSequential);
                }

                int useFilterSorting;
                if (aUseFilterSorting)
                {
                    useFilterSorting = 1;
                }
                else
                {
                    useFilterSorting = 0;
                }
         
                return StoredProcedures.MID_GRID_VIEW_INSERT.InsertAndReturnRID(_dba,
                                                                                   USER_RID: aUserRID,
                                                                                   LAYOUT_ID: aLayoutID,
                                                                                   VIEW_ID: aViewID,
                                                                                   SHOW_DETAILS: (aShowDetails) ? '1' : '0',
                                                                                   GROUP_BY: groupBy,
                                                                                   GROUP_BY_SECONDARY: groupBySecondary,
                                                                                   IS_SEQUENTIAL: isSequential,
                                                                                   WORKSPACE_FILTER_RID: workspaceFilterRID,
                                                                                   USE_FILTER_SORTING: useFilterSorting
                                                                                   );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#456 - RMatelic - Add views to Size Review - save added columns
        public void GridView_Update(int aViewRID, bool aShowDetails, int aGroupBy, int aGroupBySecondary, bool aIsSequential, int workspaceFilterRID, bool aUseFilterSorting)  //TT#1313-MD -jsobek -Header Filter
        {
            try
            {
                //object groupBy = null;
                //object groupBySecondary = null;
                //object isSequential = null;
                int? groupBy = null;
                int? groupBySecondary = null;
                char? isSequential = null;
                if (aGroupBy > Include.NoRID)
                {
                    groupBy = aGroupBy;
                }
                if (aGroupBySecondary > Include.NoRID)
                {
                    groupBySecondary = aGroupBySecondary;
                    isSequential = Include.ConvertBoolToChar(aIsSequential);
                }

                int useFilterSorting;
                if (aUseFilterSorting)
                {
                    useFilterSorting = 1;
                }
                else
                {
                    useFilterSorting = 0;
                }
                
                StoredProcedures.MID_GRID_VIEW_UPDATE.Update(_dba,
                                                             VIEW_RID: aViewRID,
                                                             SHOW_DETAILS: Include.ConvertBoolToChar(aShowDetails),
                                                             GROUP_BY: groupBy,
                                                             GROUP_BY_SECONDARY: groupBySecondary,
                                                             IS_SEQUENTIAL: isSequential,
                                                             WORKSPACE_FILTER_RID: workspaceFilterRID,
                                                             USE_FILTER_SORTING: useFilterSorting
                                                             );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#456

        public int GridView_GetKey(int aUserRID, int aLayoutID, string aViewID)
        {
            try
            {
                // Begin TT#1556 - RMatelic - Save as in User View creates new view by appending user signon
                string userName = string.Empty;
                if (aUserRID != Include.GlobalUserRID)
                {
                    //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                    //SecurityAdmin secAdmin = new SecurityAdmin();
                    //userName = "(" + secAdmin.GetUserName(aUserRID) + ")";
                    userName = "(" + UserNameStorage.GetUserName(aUserRID) + ")";
                    //End TT#827-MD -jsobek -Allocation Reviews Performance
                    if (aViewID.EndsWith(userName))
                    {
                        int nameIndex = aViewID.IndexOf(userName);
                        if (nameIndex > -1)
                        {
                            aViewID = aViewID.Substring(0, nameIndex).TrimEnd();
                        }
                    }
                }
                // End TT#1556

                DataTable dt = StoredProcedures.MID_GRID_VIEW_READ_FROM_ID_AND_USER.Read(_dba,
                                                                                 USER_RID: aUserRID,
                                                                                 LAYOUT_ID: aLayoutID,
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

        public DataTable GridViewDetail_Read(int aViewRID)
        {
            try
            {
			    return StoredProcedures.MID_GRID_VIEW_DETAIL_READ.Read(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

       

        // Begin TT#1909 - JSmith - Column Position Not Saving In Allocation Workspace
        public DataTable GridViewDetail_Read_ByPosition(int aViewRID)
        {
            try
            {
                return StoredProcedures.MID_GRID_VIEW_DETAIL_READ_SORT_BY_POSITION.Read(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#1909

        public void GridViewDetail_Insert(int aViewRID, Infragistics.Win.UltraWinGrid.UltraGrid aGrid)
        {
            try
            {
                int visiblePosition, sortDirection, sortSequence, width;
                string bandKey, colKey;
                bool isHidden, isGroupByCol;
            //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                string sysColType;
                string columnType;
                int hcg_RID;
                int result;
                HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
            //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                foreach (UltraGridBand band in aGrid.DisplayLayout.Bands)
                {
                    bandKey = band.Key;
                    foreach (UltraGridColumn column in band.Columns)
                    {
                        colKey = column.Key;
                        visiblePosition = column.Header.VisiblePosition;
                        isHidden = column.Hidden;
                        isGroupByCol = false;
                        sortDirection = (int)eSortDirection.None;
                        sortSequence = -1;
                        width = column.Width;
                        // Begin TT#3052 - RMatelic - Allocation Workspace View >> this code is moved to after the SortedColumns.Exists check. The sorted check needs to be
                        //                           done before colKey = HC_RID is changed to HCG_ID     
                    //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                        //hcg_RID = -1;                       
                        //columnType = "B";
                        //if (column.Tag != null && bandKey == "Header")
                        //{
                        //    sysColType = column.Tag.GetType().Name.ToString();
                        //    if (sysColType == "HeaderCharGroupProfile")
                        //    {
                        //        int.TryParse(colKey, out result);                                                                 
                        //        hcg_RID = result;
                        //        DataTable HCGdt = headerCharacteristicsData.HeaderCharGroup_Read(hcg_RID);
                        //        DataRow HCGdr = HCGdt.Rows[0];
                        //        colKey = (string)HCGdr["HCG_ID"];
                        //        columnType = "C";
                        //    }
                        //}
                    //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                        // End TT#3052
                        if (band.SortedColumns.Exists(colKey))
                        {
                            isGroupByCol = band.SortedColumns[colKey].IsGroupByColumn;
                            switch (band.SortedColumns[colKey].SortIndicator)
                            {
                                case SortIndicator.Ascending:
                                    sortDirection = (int)eSortDirection.Ascending;
                                    break;

                                case SortIndicator.Descending:
                                    sortDirection = (int)eSortDirection.Descending;
                                    break;
                            }
                            for (int i = 0; i < band.SortedColumns.Count; i++)
                            {
                                if (band.SortedColumns[i].Key == colKey)
                                {
                                    sortSequence = i;
                                    break;
                                }
                            }
                        }
                        // Begin TT#3052 - RMatelic - Allocation Workspace View >> the following code was moved from before the SortedColumns.Exists check  
                        //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                        hcg_RID = -1;
                        columnType = "B";
                        if (column.Tag != null && bandKey == "Header")
                        {
                            sysColType = column.Tag.GetType().Name.ToString();
                            if (sysColType == "HeaderCharGroupProfile")
                            {
                                int.TryParse(colKey, out result);
                                hcg_RID = result;
                                DataTable HCGdt = headerCharacteristicsData.HeaderCharGroup_Read(hcg_RID);
                                DataRow HCGdr = HCGdt.Rows[0];
                                colKey = (string)HCGdr["HCG_ID"];
                                columnType = "C";
                            }
                        }
                        //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                        // End TT#3052 
                        GridViewDetail_Insert(aViewRID, bandKey, colKey, visiblePosition, isHidden,
                                              isGroupByCol, sortDirection, sortSequence, width,
                                              columnType, hcg_RID); //TT#440 - MD - RBeck _ Make Header Characteristics references by RID 
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }    
        }

        public void GridViewDetail_Insert(int aViewRID, ArrayList aFlexGrids)
        {
            try
            {
                int visiblePosition, sortDirection, sortSequence, width;
                string   colKey;
                bool isHidden, isGroupByCol;
            //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                string sysColType;
                string columnType;
                int hcg_RID;
                int result;
                HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
            //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                ArrayList colList = new ArrayList();

                foreach (C1.Win.C1FlexGrid.C1FlexGrid grid in aFlexGrids)
                {
                    colList.Clear();
                    foreach (C1.Win.C1FlexGrid.Column column in grid.Cols)
                    {
                    //TT#440 - MD - RBeck _ Make Header Characteristics references by RID     
                        colKey = column.Name;
                        hcg_RID = -1;
                        columnType = "B";
                        if (column.GetType() != null && grid.Name == "Header")
                        {
                            sysColType = column.GetType().Name.ToString();
                            if (sysColType == "HeaderCharGroupProfile")
                            {
                                int.TryParse(colKey, out result);
                                hcg_RID = result;
                                DataTable HCGdt = headerCharacteristicsData.HeaderCharGroup_Read(hcg_RID);
                                DataRow HCGdr = HCGdt.Rows[0];
                                colKey = (string)HCGdr["HCG_ID"];
                                columnType = "C";
                            }
                        }
                    //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                        if (column.Name == null || column.Name == string.Empty)
                        {
                            continue;
                        }
                        colKey = column.Name;
                        if (!colList.Contains(colKey))
                        {
                            colList.Add(colKey);
                        }
                        else
                        {
                            continue;
                        }
                        visiblePosition = column.Index;
                        isHidden = !column.Visible;
                        isGroupByCol = false;
                        sortDirection = (int)eSortDirection.None;
                        sortSequence = -1;
                        width = column.Width;
                   
                        SortFlags sortFlags = column.Sort;
                        switch (sortFlags)
                        {
                            case SortFlags.Ascending:
                                sortDirection = (int)eSortDirection.Ascending;
                                break;

                            case SortFlags.Descending:
                                sortDirection = (int)eSortDirection.Descending;
                                break;
                        }
 
                        GridViewDetail_Insert(aViewRID, grid.Name, colKey, visiblePosition, isHidden,
                                              isGroupByCol, sortDirection, sortSequence, width,
                                              columnType, hcg_RID);//TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                   
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#710-MD - JSmith - Adding view for Style Component with multiple header selected causes IndexOutOfRangeException
        public void GridViewDetail_Insert(int aViewRID, ArrayList aFlexGrids, int aDetailComponentCount)
        {
            try
            {
                int visiblePosition, sortDirection, sortSequence, width;
                string colKey;
                bool isHidden, isGroupByCol;
                //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                string sysColType;
                string columnType;
                int hcg_RID;
                int result;
                HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
                //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                ArrayList colList = new ArrayList();

                foreach (C1.Win.C1FlexGrid.C1FlexGrid grid in aFlexGrids)
                {
                    colList.Clear();
                    foreach (C1.Win.C1FlexGrid.Column column in grid.Cols)
                    {
                        //TT#440 - MD - RBeck _ Make Header Characteristics references by RID     
                        colKey = column.Name;
                        hcg_RID = -1;
                        columnType = "B";
                        if (column.GetType() != null && grid.Name == "Header")
                        {
                            sysColType = column.GetType().Name.ToString();
                            if (sysColType == "HeaderCharGroupProfile")
                            {
                                int.TryParse(colKey, out result);
                                hcg_RID = result;
                                DataTable HCGdt = headerCharacteristicsData.HeaderCharGroup_Read(hcg_RID);
                                DataRow HCGdr = HCGdt.Rows[0];
                                colKey = (string)HCGdr["HCG_ID"];
                                columnType = "C";
                            }
                        }
                        //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
                        if (column.Name == null || column.Name == string.Empty)
                        {
                            continue;
                        }
                        colKey = column.Name;
                        if (!colList.Contains(colKey))
                        {
                            colList.Add(colKey);
                        }
                        else
                        {
                            continue;
                        }
                        if (grid.Name == "g3")
                        {
                            visiblePosition = column.Index / aDetailComponentCount;
                        }
                        else
                        {
                            visiblePosition = column.Index;
                        }
                        isHidden = !column.Visible;
                        isGroupByCol = false;
                        sortDirection = (int)eSortDirection.None;
                        sortSequence = -1;
                        width = column.Width;

                        SortFlags sortFlags = column.Sort;
                        switch (sortFlags)
                        {
                            case SortFlags.Ascending:
                                sortDirection = (int)eSortDirection.Ascending;
                                break;

                            case SortFlags.Descending:
                                sortDirection = (int)eSortDirection.Descending;
                                break;
                        }

                        GridViewDetail_Insert(aViewRID, grid.Name, colKey, visiblePosition, isHidden,
                                              isGroupByCol, sortDirection, sortSequence, width,
                                              columnType, hcg_RID);//TT#440 - MD - RBeck _ Make Header Characteristics references by RID

                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#710-MD - JSmith - Adding view for Style Component with multiple header selected causes IndexOutOfRangeException

        public void GridViewDetail_Insert(int aViewRID,string aBandKey, string aColumnKey
			                            , int aVisiblePosition, bool aIsHidden, bool aIsGroupByCol
                                        , int aSortDirection, int aSortSequence, int aWidth
                                        , string aColumnType, int aHcgRid) //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
	    {
        //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
            int? aHcgRidNullAble;
            if (aHcgRid==-1)
                {
                    aHcgRidNullAble = null;
                }
            else
                {
                  aHcgRidNullAble = aHcgRid;
                }
        //TT#440 - MD - RBeck _ Make Header Characteristics references by RID
            try
            {
		         //object sortSeq = null;
                int? sortSeq = null;
                 if (aSortSequence > -1)
                 {
                     sortSeq = aSortSequence;
                 }
                
                 StoredProcedures.MID_GRID_VIEW_DETAIL_INSERT.Insert(_dba,
                                                                     VIEW_RID: aViewRID,
                                                                     BAND_KEY: aBandKey,
                                                                     COLUMN_KEY: aColumnKey,
                                                                     VISIBLE_POSITION: aVisiblePosition,
                                                                     IS_HIDDEN: Include.ConvertBoolToChar(aIsHidden),
                                                                     IS_GROUPBY_COL: Include.ConvertBoolToChar(aIsGroupByCol),
                                                                     SORT_DIRECTION: aSortDirection,
                                                                     SORT_SEQUENCE: sortSeq,
                                                                     WIDTH: aWidth,
                                                                     COLUMN_TYPE: aColumnType,
                                                                     HCG_RID: aHcgRidNullAble
                                                                     );
			}
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void GridViewDetail_Delete(int aViewRID)
        {
            try
            {
                StoredProcedures.MID_GRID_VIEW_DETAIL_DELETE.Delete(_dba, VIEW_RID: aViewRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int GridView_Delete(int aViewRID)
        {
            try
            {
                return StoredProcedures.MID_GRID_VIEW_DELETE.Delete(_dba, VIEW_RID: aViewRID); //TT#1313-MD -jsobek -Header Filters
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public DataTable GridViewSplitter_Read(
            int viewRID,
            int userRID,
            eLayoutID layoutID)
        {
            try
            {
                return StoredProcedures.MID_GRID_VIEW_SPLITTER_READ.Read(_dba,
                    VIEW_RID: viewRID,
                    USER_RID: userRID,
                    LAYOUT_ID: (int)layoutID
                    );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        public void GridViewSplitter_Insert(
            int viewRID,
            int userRID,
            eLayoutID layoutID = eLayoutID.NotDefined,
            char splitterTypeIndicator = 'V',
            int splitterSequence = Include.Undefined,
            double splitterPercentage = 0
            )
        {
            try
            {
                StoredProcedures.MID_GRID_VIEW_SPLITTER_INSERT.Insert(_dba,
                                                                    VIEW_RID: viewRID,
                                                                    USER_RID: userRID,
                                                                    LAYOUT_ID: (int)layoutID,
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

        public void GridViewSplitter_Delete(
            int viewRID,
            int userRID,
             eLayoutID layoutID
            )
        {
            try
            {
                StoredProcedures.MID_GRID_VIEW_SPLITTER_DELETE.Delete(
                    _dba,
                    VIEW_RID: viewRID,
                    USER_RID: userRID,
                    LAYOUT_ID: (int)layoutID
                    );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #region Grid View Filters

        //Begin TT#1313-MD -jsobek -Header Filters
        //public bool GridViewFilterExists(int aViewRID)
        //{
        //    try
        //    {
        //        //string SQLCommand = "SELECT COUNT (*) MyCount FROM GRID_VIEW_FILTER WHERE VIEW_RID = " + Convert.ToString(aViewRID,CultureInfo.CurrentUICulture);
        //        //return (_dba.ExecuteRecordCount(SQLCommand) > 0);
        //        int rowCount = StoredProcedures.MID_GRID_VIEW_FILTER_READ_COUNT.ReadRecordCount(_dba, VIEW_RID: aViewRID);
        //        return (rowCount > 0);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Returns the header or assortmnet workspace filter rid (it will return a -1 for nulls)
        /// </summary>
        /// <param name="aViewRID"></param>
        /// <returns></returns>
        public int GridViewReadWorkspaceFilterRID(int aViewRID, ref bool useFilterSorting)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_GRID_VIEW_READ_WORKSPACE_FILTER_RID.Read(_dba, VIEW_RID: aViewRID);
                int workspaceFilterRid = Include.NoRID;
                useFilterSorting = false;
                if (dt.Rows.Count > 0)
                {
                    workspaceFilterRid = (int)dt.Rows[0]["WORKSPACE_FILTER_RID"];

                    if (dt.Rows[0]["USE_FILTER_SORTING"] != DBNull.Value && Convert.ToInt32(dt.Rows[0]["USE_FILTER_SORTING"]) == 1)
                    {
                        useFilterSorting = true;
                    }
                }
                return workspaceFilterRid;
            }
            catch
            {
                throw;
            }
        }

        //public MIDDbParameter[] GridViewFilter_Delete(int aViewRID)
        //public int GridViewFilter_Delete(int aViewRID)
        //{
        //    //MIDDbParameter[] inParams;
        //    try
        //    {
        //        //inParams = new MIDDbParameter[1];
        //        //inParams[0] = new MIDDbParameter("@VIEW_RID", aViewRID);
        //        //inParams[0].DbType = eDbType.Int;
        //        //inParams[0].Direction = eParameterDirection.Input;

        //        //return _dba.ExecuteStoredProcedure("SP_MID_GRID_VIEW_FILTER_DELETE", inParams);
        //        return StoredProcedures.SP_MID_GRID_VIEW_FILTER_DELETE.Delete(_dba, VIEW_RID: aViewRID);
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    } 
        //}

        //public void GridViewFilter_Add(int aViewRID, int aUserRID)
        //{
        //    try
        //    {
        //        GridViewFilter_Insert(aViewRID, aUserRID);
        //        GridViewFilterTypes_Insert(aViewRID, aUserRID);
        //        GridViewFilterStatus_Insert(aViewRID, aUserRID);
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}

        //private void GridViewFilter_Insert(int aViewRID, int aUserRID)
        //{
        //    try
        //    {
        //        //string SQLCommand = "INSERT INTO GRID_VIEW_FILTER SELECT " + Convert.ToString(aViewRID, CultureInfo.CurrentUICulture)  
        //        //  + ",HN_RID,HEADER_DATE_TYPE,HEADER_DATE_BETWEEN_FROM,HEADER_DATE_BETWEEN_TO,"
        //        //  + "HEADER_DATE_FROM,HEADER_DATE_TO,"
        //        //  + "RELEASE_DATE_TYPE,RELEASE_DATE_BETWEEN_FROM,RELEASE_DATE_BETWEEN_TO,"
        //        //  + "RELEASE_DATE_FROM,RELEASE_DATE_TO,MAXIMUM_HEADERS "
        //        //  + "FROM AL_WRKSP_FILTER WHERE USER_RID = " + Convert.ToString(aUserRID, CultureInfo.CurrentUICulture); 

        //        //_dba.ExecuteNonQuery(SQLCommand);
        //        StoredProcedures.MID_GRID_VIEW_FILTER_INSERT.Insert(_dba,
        //                                                            VIEW_RID: aViewRID,
        //                                                            USER_RID: aUserRID
        //                                                            );

        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}

        //private void GridViewFilterTypes_Insert(int aViewRID, int aUserRID)
        //{
        //    try
        //    {
        //        //string SQLCommand = "INSERT INTO GRID_VIEW_FILTER_TYPES SELECT " + Convert.ToString(aViewRID, CultureInfo.CurrentUICulture)  
        //        //    + ",HEADER_TYPE,TYPE_SELECTED"
        //        //    + " FROM AL_WRKSP_FILTER_TYPES WHERE USER_RID = " + Convert.ToString(aUserRID, CultureInfo.CurrentUICulture); 
      
        //        //_dba.ExecuteNonQuery(SQLCommand);
        //        StoredProcedures.MID_GRID_VIEW_FILTER_TYPES_INSERT.Insert(_dba,
        //                                                                  VIEW_RID: aViewRID,
        //                                                                  USER_RID: aUserRID
        //                                                                  );
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        //private void GridViewFilterStatus_Insert(int aViewRID, int aUserRID)
        //{
        //    try
        //    {
        //        //string SQLCommand = "INSERT INTO GRID_VIEW_FILTER_STATUS SELECT " + Convert.ToString(aViewRID, CultureInfo.CurrentUICulture)
        //        //     + ",HEADER_STATUS,STATUS_SELECTED"
        //        //     + " FROM AL_WRKSP_FILTER_STATUS WHERE USER_RID = " + Convert.ToString(aUserRID, CultureInfo.CurrentUICulture);  

        //        //_dba.ExecuteNonQuery(SQLCommand);
        //        StoredProcedures.MID_GRID_VIEW_FILTER_STATUS_INSERT.Insert(_dba,
        //                                                                   VIEW_RID: aViewRID,
        //                                                                   USER_RID: aUserRID
        //                                                                   );
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters

        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance - unused function
        //public DataTable GridViewFilter_Read(int aViewRID)
        //{
        //    try
        //    {
        //        string SQLCommand = "SELECT * FROM GRID_VIEW_FILTER  "
        //            + "WHERE VIEW_RID = " + aViewRID.ToString(CultureInfo.CurrentUICulture);
             
        //        return _dba.ExecuteSQLQuery(SQLCommand, "GridViewFilter");
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        

        //public DataTable GridViewFilterTypes_Read(int aViewRID)
        //{
        //    try
        //    {
        //        string SQLCommand = "SELECT * FROM GRID_VIEW_FILTER_TYPES  "
        //            + "WHERE VIEW_RID = " + aViewRID.ToString(CultureInfo.CurrentUICulture);
             
        //        return _dba.ExecuteSQLQuery(SQLCommand, "GridViewFilterTypes");
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        

        //public DataTable GridViewFilterStatus_Read(int aViewRID)
        //{
        //    try
        //    {
        //        string SQLCommand = "SELECT * FROM GRID_VIEW_FILTER_STATUS  "
        //            + "WHERE VIEW_RID = " + aViewRID.ToString(CultureInfo.CurrentUICulture);
              
        //        return _dba.ExecuteSQLQuery(SQLCommand, "GridViewFilterStatus");
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance - unused function

   
        #endregion

      
    }
}
