using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{

	public partial class ForecastVersion: DataLayer
	{
		public ForecastVersion() : base()
		{

		}

		public bool DeleteVersion(int fvRid)
		{
			try
			{
                int rowsDeleted = StoredProcedures.MID_FORECAST_VERSION_DELETE.Delete(_dba, FV_RID: fvRid); //TT#1268-MD -jsobek -5.4 Merge
                return (rowsDeleted > 0);
			}
			catch (Exception e)
			{
				throw (e);
			}
		}


		public bool UpdateVersion(int fvRid, string versionID, string description, bool protectHistory, bool isActive, eForecastBlendType aBlendType, int aBlendActualVersionRID, int aBlendForecastVersionRID, bool aCurrentBlendInd, bool aSimilarStoreInd)
		{
			try
			{
                
                int? aBlendActualVersionRID_Nullable = aBlendActualVersionRID;
                int? aBlendForecastVersionRID_Nullable = aBlendForecastVersionRID;
                char? CURRENT_BLEND_IND_Nullable = Include.ConvertBoolToChar(aCurrentBlendInd);
                if (aBlendType == eForecastBlendType.None)
                {
                    aBlendActualVersionRID_Nullable = null;
                    aBlendForecastVersionRID_Nullable = null;
                    CURRENT_BLEND_IND_Nullable = null;

                }
                int rowsUpdated = StoredProcedures.MID_FORECAST_VERSION_UPDATE.Update(_dba,
                                                                    FV_RID: fvRid,
                                                                    FV_ID: versionID.ToCharArray()[0],
                                                                    DESCRIPTION: description,
                                                                    PROTECT_HISTORY_IND: Include.ConvertBoolToChar(protectHistory),
                                                                    ACTIVE_IND: Include.ConvertBoolToChar(isActive),
                                                                    BLEND_TYPE: (int)aBlendType,
                                                                    ACTUAL_FV_RID: aBlendActualVersionRID_Nullable,
                                                                    FORECAST_FV_RID: aBlendForecastVersionRID,
                                                                    CURRENT_BLEND_IND: CURRENT_BLEND_IND_Nullable,
                                                                    SIMILAR_STORE_IND: Include.ConvertBoolToChar(aSimilarStoreInd)
                                                                    );
                return (rowsUpdated > 0);
			}
			catch (Exception e)
			{
				throw (e);
			}
		}

		public int CreateVersion(string versionID, string description, bool protectHistory, bool isActive,
			eForecastBlendType aBlendType, int aBlendActualVersionRID, int aBlendForecastVersionRID,
			bool aCurrentBlendInd, bool aSimilarStoreInd)
		{
			try
			{
                int? aBlendActualVersionRID_Nullable = aBlendActualVersionRID;
                int? aBlendForecastVersionRID_Nullable = aBlendForecastVersionRID;
                char? CURRENT_BLEND_IND_Nullable = Include.ConvertBoolToChar(aCurrentBlendInd);
                if (aBlendType == eForecastBlendType.None)
                {
                    aBlendActualVersionRID_Nullable = null;
                    aBlendForecastVersionRID_Nullable = null;
                    CURRENT_BLEND_IND_Nullable = null;
                }
                return StoredProcedures.SP_MID_FORECAST_VERSION_INSERT.InsertAndReturnRID(_dba,
                                                                                          FV_ID: versionID.ToCharArray()[0],
                                                                                          DESCRIPTION: description,
                                                                                          PROTECT_HISTORY_IND: Include.ConvertBoolToChar(protectHistory),
                                                                                          ACTIVE_IND: Include.ConvertBoolToChar(isActive),
                                                                                          BLEND_TYPE: Convert.ToInt32(aBlendType),
                                                                                          ACTUAL_FV_RID: aBlendActualVersionRID_Nullable,
                                                                                          FORECAST_FV_RID: aBlendForecastVersionRID_Nullable,
                                                                                          CURRENT_BLEND_IND: CURRENT_BLEND_IND_Nullable,
                                                                                          SIMILAR_STORE_IND: Include.ConvertBoolToChar(aSimilarStoreInd)
                                                                                          );

			}
			catch (Exception e)
			{
				throw (e);
			}
		}

		public DataTable GetForecastVersions()
		{
			return GetForecastVersions(false);
		}
 
		public DataTable GetForecastVersions(bool IncludeInActiveVersions)
		{
			try
			{
                //Begin TT#1286-MD -jsobek -5.4 OTS Plan Versions button has wrong label
                DataTable dt;
                if (IncludeInActiveVersions)
                {
                    dt = StoredProcedures.MID_FORECAST_VERSION_READ_ALL.Read(_dba);
                }
                else
                {
                    dt = StoredProcedures.MID_FORECAST_VERSION_READ_ALL_ACTIVE.Read(_dba);
                }
                dt.TableName = "Plan Version"; //used for a button label on the global options screen under the OTS Plan Versions tab
                return dt;
                //End TT#1286-MD -jsobek -5.4 OTS Plan Versions button has wrong label
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        public DataTable GetForecastVersions_ReadAllSortedByVersion()
        {
            try
            {
                return StoredProcedures.MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

		//Begin Track #4457 - JSmith - Add forecast versions
		public DataTable GetForecastVersion(int aVersionRID)
		{
			try
			{
                return StoredProcedures.MID_FORECAST_VERSION_READ.Read(_dba, FV_RID: aVersionRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		public string GetVersionText(int VersionID)
		{
			string description = "Unknown";
			//MID Track # 2354 - removed nolock because it causes concurrency issues
            object tempDesc = StoredProcedures.MID_FORECAST_VERSION_READ_DESCRIPTION.ReadValue(_dba, FV_RID: VersionID);
			if (tempDesc != DBNull.Value)
				description = tempDesc.ToString();

			return description;
		}

		// Begin Issue 4047 - stodd
		public bool GetIsVersionProtected(int VersionID)
		{
			bool isProtected = false;
            object pInd = StoredProcedures.MID_FORECAST_VERSION_READ_PROTECTED.ReadValue(_dba, FV_RID: VersionID);
			if (pInd == DBNull.Value)
			{
				isProtected = false;
			}
			else
			{
				isProtected = Include.ConvertCharToBool(pInd.ToString()[0]);
			}

			return isProtected;
		}
		// End Issue 4047 - stodd


		/// <summary>
		/// Provides Hashtable containing forecast versions
		/// </summary>
		/// <param name="aIncludeInactiveVersions">a flag identifying if inactive versions are to be returned</param>
		/// <param name="aSetToLower">a flag identifying if the version name is to be set to lower</param>
		/// <returns>a Hashtable with the version name as the key with the RID as the data</returns>
		public Hashtable GetForecastVersionsHashtable(bool aIncludeInactiveVersions, bool aSetToLower)
		{
			try
			{
				Hashtable forecastVersions = new Hashtable();
				string description = null;
				int RID;
				DataTable dt = GetForecastVersions(aIncludeInactiveVersions);
				foreach(DataRow dr in dt.Rows)
				{
					RID			= Convert.ToInt32(dr["FV_RID"]);
					description	= (string)dr["DESCRIPTION"];
					if (aSetToLower)
					{
						description	= description.ToLower();
					}
					forecastVersions.Add(description,RID);
				}
				return forecastVersions;
			}
			catch
			{
				throw;
			}
		}
	}
}
