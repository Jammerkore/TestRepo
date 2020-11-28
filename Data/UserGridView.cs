using System;
using System.Diagnostics;
using System.Data;
using System.Text;
using MIDRetail.DataCommon;
using System.ComponentModel;
using System.Globalization;

namespace MIDRetail.Data
{
    public partial class UserGridView : DataLayer
    {
        public UserGridView() : base()
        {
        }

        public int UserGridView_Read(int aUserRID, eLayoutID aLayoutID)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_USER_CURRENT_GRID_VIEW_READ.Read(_dba,
                                                                             USER_RID: aUserRID,
                                                                             LAYOUT_ID: (int)aLayoutID
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void UserGridView_Update(int aUserRID, eLayoutID aLayoutID, int aViewRID)
        {
            try
            {
                _dba.OpenUpdateConnection();

                try
                {
                    UserGridView_Delete(aUserRID, (int)aLayoutID);
                    if (aViewRID != Include.NoRID)
                    {
                        GridViewData gvd = new GridViewData();
                        DataRow row = gvd.GridView_Read(aViewRID); // View may have been deleted
                        if (row != null)
                        {
                            UserGridView_Insert(aUserRID, (int)aLayoutID, aViewRID);
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

        private void UserGridView_Delete(int aUserRID, int aLayoutID) 
        {
            try
            {
                StoredProcedures.MID_USER_CURRENT_GRID_VIEW_DELETE.Delete(_dba,
                                                                          USER_RID: aUserRID,
                                                                          LAYOUT_ID: aLayoutID
                                                                          );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void UserGridView_Insert(int aUserRID, int aLayoutID, int aViewRID)
        {
            try
            {
                StoredProcedures.MID_USER_CURRENT_GRID_VIEW_INSERT.Insert(_dba,
                                                                          USER_RID: aUserRID,
                                                                          LAYOUT_ID: aLayoutID,
                                                                          VIEW_RID: aViewRID
                                                                          );
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }
}
