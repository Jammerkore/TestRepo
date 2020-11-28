using System;
using System.Data;
//using System.Collections;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	public partial class ColorData : DataLayer
	{
		public ColorData() : base()
		{
            
		}


		public DataTable Colors_Read()
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_COLOR_CODE_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public int Color_Add(string colorID, string colorName, string colorGroup, bool virtualInd, ePurpose aPurpose)
        {
            try
            { 
                return StoredProcedures.SP_MID_COLOR_INSERT.InsertAndReturnRID(_dba, 
                                                                                COLOR_CODE_ID: colorID,
                                                                                COLOR_CODE_NAME: colorName,
                                                                                COLOR_CODE_GROUP: colorGroup,
                                                                                VIRTUAL_IND: Include.ConvertBoolToChar(virtualInd),
                                                                                PURPOSE: Convert.ToInt32(virtualInd)
                                                                                );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void Color_Update(int colorCodeRID, string colorID, string colorName, string colorGroup, bool virtualInd, ePurpose aPurpose)
        {
            try
            {
                StoredProcedures.MID_COLOR_CODE_UPDATE.Update(_dba, 
                                                              COLOR_CODE_RID: colorCodeRID,
                                                              COLOR_CODE_ID: colorID,
                                                              COLOR_CODE_NAME: colorName,
                                                              COLOR_CODE_GROUP: colorGroup,
                                                              VIRTUAL_IND: Include.ConvertBoolToChar(virtualInd),
                                                              PURPOSE: Convert.ToInt32(aPurpose)
                                                              );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		
		public void Color_Delete(int colorCodeRID)
		{
			try
			{
                StoredProcedures.MID_COLOR_CODE_DELETE.Delete(_dba, COLOR_CODE_RID: colorCodeRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool Color_Exists(string colorID)
		{
			try
			{
                return (StoredProcedures.MID_COLOR_CODE_ID_EXISTS.ReadRecordCount(_dba, COLOR_CODE_ID: colorID) > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public int GetColorCodeRID(string colorID) 
		{
			try
			{
                DataTable dt = StoredProcedures.MID_COLOR_CODE_READ_RID_FROM_ID.Read(_dba, COLOR_CODE_ID: colorID);
				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					return Include.NoRID;
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetColorGroups()
		{
			try
			{
                // only retrieve groups for default purpose    
                return StoredProcedures.MID_COLOR_CODE_READ_COLOR_GROUPS.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetColorsForGroup(string aGroupName)
		{
			try
			{
                return StoredProcedures.MID_COLOR_CODE_READ_COLORS_FROM_GROUP.Read(_dba, COLOR_CODE_GROUP: aGroupName);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
