using System;
using System.Data;
using System.Collections;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
    // Begin Track #6368 – JSmith - Invalid object name 'dbo.FAVORITES'
    ///// <summary>
    ///// Summary description for FavoritesData.
    ///// </summary>
    //public class FavoritesData : DataLayer
    //{
    //    public FavoritesData()
    //    {
    //        //
    //        // TODO: Add constructor logic here
    //        //
    //    }

    //    /// <summary>
    //    /// returns the item RID of the FAVORITES table as an array based upon
    //    /// the favorites type and the user RID.
    //    /// </summary>
    //    /// <param name="favoritesType"></param>
    //    /// <param name="userRID"></param>
    //    /// <returns></returns>
    //    public ArrayList GetFavoritesArray(eFavoritesType favoritesType, int userRID)
    //    {
    //        try
    //        {
    //            ArrayList favoritesList = new ArrayList();

    //            DataTable dtFavorites = GetFavorites((int)favoritesType, userRID);

    //            //				int idx = 0;
    //            foreach (DataRow row in dtFavorites.Rows)
    //            {
    //                int rid = Convert.ToInt32(row["FAV_ITEM_RID"], CultureInfo.CurrentUICulture);
    //                favoritesList.Add(rid);
    //            }

    //            return favoritesList;
    //        }
    //        catch (Exception err)
    //        {
    //            string message = err.ToString();
    //            throw;
    //        }
    //    }

    //    /// <summary>
    //    /// returns the item RID of the FAVORITES table as a dataTable based upon
    //    /// the favorites type and the user RID.		
    //    /// </summary>
    //    /// <param name="favoritesType"></param>
    //    /// <param name="userRID"></param>
    //    /// <returns></returns>
    //    public DataTable GetFavorites(int favoritesType, int userRID)
    //    {
    //        try
    //        {
    //            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //            //string SQLCommand = "SELECT FAV_ITEM_RID ";
    //            //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
    //            //SQLCommand += "FROM FAVORITES " +
    //            //    "WHERE FAV_TYPE = " + favoritesType.ToString(CultureInfo.CurrentUICulture) +
    //            //    " AND USER_RID = " + userRID.ToString(CultureInfo.CurrentUICulture) +
    //            //    " ORDER BY FAV_ITEM_RID";
    //            //// end MID Track # 2354

    //            //return _dba.ExecuteSQLQuery(SQLCommand, "FAVORITES");
    //            DataTable newTable = newTable = new DataTable("FAVORITES");
    //            newTable.Columns.Add("FAV_TYPE", typeof(int));
    //            newTable.Columns.Add("USER_RID", typeof(int));
    //            newTable.Columns.Add("FAV_ITEM_RID", typeof(int));
    //            return newTable;
    //            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

    //        }
    //        catch (Exception err)
    //        {
    //            string message = err.ToString();
    //            throw;
    //        }
    //    }

    //    public void DeleteFromFavorites(int favoritesType, int userRID, int itemRID)
    //    {
    //        try
    //        {
    //            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //            //string SqlStatement = "DELETE FROM FAVORITES " +
    //            //    "WHERE FAV_TYPE = " + favoritesType.ToString(CultureInfo.CurrentUICulture) +
    //            //    " AND USER_RID = " + userRID.ToString(CultureInfo.CurrentUICulture) +
    //            //    " AND FAV_ITEM_RID = " + itemRID.ToString(CultureInfo.CurrentUICulture);

    //            //_dba.ExecuteNonQuery(SqlStatement);

    //            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //            return;
    //        }
    //        catch (Exception err)
    //        {
    //            string message = err.ToString();
    //            throw;
    //        }
    //    }

    //    public void InsertIntoFavorites(int favoritesType, int userRID, int itemRID)
    //    {
    //        try
    //        {
    //            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //            //string SqlStatement = "INSERT INTO FAVORITES(FAV_TYPE, USER_RID, FAV_ITEM_RID) " +
    //            //    "VALUES(" + favoritesType.ToString(CultureInfo.CurrentUICulture) + "," +
    //            //    userRID.ToString(CultureInfo.CurrentUICulture) + "," +
    //            //    itemRID.ToString(CultureInfo.CurrentUICulture) + ")";

    //            //_dba.ExecuteNonQuery(SqlStatement);

    //            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //            return;
    //        }
    //        catch (Exception err)
    //        {
    //            string message = err.ToString();
    //            throw;
    //        }
    //    }

    //    /// <summary>
    //    /// Returns a bool identifying if the favorited is assigned to the user.		
    //    /// </summary>
    //    /// <param name="favoritesType"></param>
    //    /// <param name="userRID"></param>
    //    /// <param name="itemRID"
    //    /// <returns></returns>
    //    public bool FavoriteAssigned(int favoritesType, int userRID, int itemRID)
    //    {
    //        try
    //        {
    //            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //            //// begin MID Track # 2354 - removed nolock because it causes concurrency issues
    //            //string SQLCommand = "SELECT COUNT(*) AS MyCount FROM FAVORITES " +
    //            //    "WHERE FAV_TYPE = " + favoritesType.ToString(CultureInfo.CurrentUICulture) +
    //            //    " AND USER_RID = " + userRID.ToString(CultureInfo.CurrentUICulture) +
    //            //    " AND FAV_ITEM_RID = " + itemRID.ToString(CultureInfo.CurrentUICulture);
    //            //// end MID Track # 2354

    //            //int count = _dba.ExecuteRecordCount(SQLCommand);

    //            //if (count > 0)
    //            //{
    //            //    return true;
    //            //}
    //            //else
    //            //{
    //            //    return false;
    //            //}
    //            return false;
    //            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
    //        }
    //        catch (Exception err)
    //        {
    //            string message = err.ToString();
    //            throw;
    //        }
    //    }
    //}
    // End Track #6368
}
