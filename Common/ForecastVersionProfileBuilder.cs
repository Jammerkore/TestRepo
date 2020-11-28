using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for ForecastVersionProfileBuilder.
	/// </summary>
	public class ForecastVersionProfileBuilder
	{
		ForecastVersion _forecastVersionData = new ForecastVersion();

		public ForecastVersionProfileBuilder()
		{
			_forecastVersionData = new ForecastVersion();
		}

		public VersionProfile Build(int aVersionRID)
		{
			int actualVersionRID = Include.NoRID;
            int forecastVersionRID = Include.NoRID;
            bool currentBlendInd = false;
            // BEGIN Override Low Level Changes  stodd
            string description = string.Empty;
            bool prectectHistoryInd = false;
            bool similarStoreInd = false;
			DataTable dt = _forecastVersionData.GetForecastVersion(aVersionRID);
            DataRow dr = null;
            eForecastBlendType blendType = eForecastBlendType.None;
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
                blendType = (eForecastBlendType)dr["BLEND_TYPE"];
                if (blendType != eForecastBlendType.None)
                {
                    actualVersionRID = Convert.ToInt32(dr["ACTUAL_FV_RID"]);
                    forecastVersionRID = Convert.ToInt32(dr["FORECAST_FV_RID"]);
                    currentBlendInd = Include.ConvertCharToBool(Convert.ToChar(dr["CURRENT_BLEND_IND"], CultureInfo.CurrentUICulture));
                }
                description = Convert.ToString(dr["DESCRIPTION"], CultureInfo.CurrentUICulture);
                prectectHistoryInd = Include.ConvertCharToBool(Convert.ToChar(dr["PROTECT_HISTORY_IND"], CultureInfo.CurrentUICulture));
                similarStoreInd = Include.ConvertCharToBool(Convert.ToChar(dr["SIMILAR_STORE_IND"], CultureInfo.CurrentUICulture));
            }

			return new VersionProfile(aVersionRID,
				description,
				prectectHistoryInd,
				//Begin Track #4547 - JSmith - Add similar stores by forecast versions
//				blendType, actualVersionRID, forecastVersionRID, currentBlendInd);
				blendType, actualVersionRID, forecastVersionRID, currentBlendInd,
				similarStoreInd);
				//End Track #4547
            // END Override Low Level Changes
		}
	}
}
