using System;
using System.Data;
using System.Globalization;
using System.Text;
using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for SizeCurve.
	/// </summary>
	public partial class SizeCurve : DataLayer
	{
		#region "Member Variables"
		string NoSizeDimensionLbl;  // MID Track 3914 Constraints displays "-1" for dimension when no dims
		#endregion

		#region "Properties"
		#endregion

		#region "Constructors"
			public SizeCurve() : base()
			{
				NoSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize); // MID Track 3914 Constraints displays "-1" for dimension when no dim
			}
		#endregion

		#region "Methods"
		/// <summary>
		/// Returns a datatable all size curve groups defined in table SIZE_CURVE_GROUP
		/// </summary>
		/// <returns></returns>
		public DataTable GetSizeCurveGroups()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_CURVE_GROUP_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}

		}
		// BEGIN ANF Generic Size Constraint
		/// <summary>
		/// Returns a datatable containing the requested size curve group
		/// </summary>
		/// <returns></returns>
		public DataTable GetSpecificSizeCurveGroup(int aSizeCurveGroupRID)
		{
			try
			{
                return StoredProcedures.MID_SIZE_CURVE_GROUP_READ.Read(_dba, SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			
		}


        public DataTable GetFilteredSizeCurveGroupsCaseSensitive(string sizeCurveGroupName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_CURVE_GROUP_READ_FROM_NAME.Read(_dba, SIZE_CURVE_GROUP_NAME: sizeCurveGroupName);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }
        public DataTable GetFilteredSizeCurveGroupsCaseInsensitive(string sizeCurveGroupName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_CURVE_GROUP_READ_FROM_NAME_UPPER.Read(_dba, SIZE_CURVE_GROUP_NAME: sizeCurveGroupName);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }

        }
		// END ANF Generic Size Constraint

       

		/// <summary>
		/// Returns the key of the given size curve group name
		/// </summary>
		/// <returns></returns>
		public int GetSizeCurveGroupKey(string aSizeCurveGroupName)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SIZE_CURVE_GROUP_READ_FROM_NAME_EXACT.Read(_dba, SIZE_CURVE_GROUP_NAME: aSizeCurveGroupName);
				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["SIZE_CURVE_GROUP_RID"]));
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

       


		/// <summary>
		/// Insert the given parameters in the SIZE_CURVE_GROUP table
		/// </summary>
		/// <returns></returns>
		public int InsertSizeCurveGroup(string aSizeCurveGroupName, int aDefaultSizeCurveRID, int aDefinedSizeGroupRID)
		{
			try
			{
                int? DEFINED_SIZE_GROUP_RID_Nullable = null;
                if (aDefinedSizeGroupRID != Include.NoRID) DEFINED_SIZE_GROUP_RID_Nullable = aDefinedSizeGroupRID;
                return StoredProcedures.SP_MID_SIZE_CURVE_GROUP_INSERT.InsertAndReturnRID(_dba,
                                                                                          SIZE_CURVE_GROUP_NAME: aSizeCurveGroupName,
                                                                                          DEFAULT_SIZE_CURVE_RID: aDefaultSizeCurveRID,
                                                                                          DEFINED_SIZE_GROUP_RID: DEFINED_SIZE_GROUP_RID_Nullable
                                                                                          );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Update the requested Size Curve Group
		/// </summary>
		/// <returns></returns>
		public void UpdateSizeCurveGroup(int aSizeCurveGroupRID, string aSizeCurveGroupName, int aDefaultSizeCurveRID, int aDefinedSizeGroupRID)
		{
			try
			{
                int? DEFINED_SIZE_GROUP_RID_Nullable = null;
                if (aDefinedSizeGroupRID != Include.NoRID) DEFINED_SIZE_GROUP_RID_Nullable = aDefinedSizeGroupRID;
                StoredProcedures.MID_SIZE_CURVE_GROUP_UPDATE.Update(_dba,
                                                                    SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID,
                                                                    SIZE_CURVE_GROUP_NAME: aSizeCurveGroupName,
                                                                    DEFAULT_SIZE_CURVE_RID: aDefaultSizeCurveRID,
                                                                    DEFINED_SIZE_GROUP_RID: DEFINED_SIZE_GROUP_RID_Nullable
                                                                    );
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#301 - Begin - SZ HST-> Size curve disappears in the Size need method after processing the Size curve method
		/// <summary>
		/// Delete the requested Size Curve
		/// </summary>
		/// <returns></returns>
		public void ClearSizeCurveGroup(string aSizeCurveGroupName)
		{
			try
			{
                StoredProcedures.SP_MID_REMOVE_ALL_SIZE_CURVES_FROM_GROUP.Delete(_dba, inSizeCurveGroup: aSizeCurveGroupName);
			}
			catch
			{
				throw;
			}
		}

		//End TT#301 - Begin - SZ HST-> Size curve disappears in the Size need method after processing the Size curve method
		// BEGIN MID Track #5240 - slow Size Curve Group delete   
		public bool SizeCurveGroupIsInUse(int aSizeCurveGroupRID)
		{
			bool isInUse = false;
			try
			{

                int returnCode = StoredProcedures.SP_MID_IS_SIZE_CURVE_GROUP_IN_USE.ReadValue(_dba, SCG_RID: aSizeCurveGroupRID);
                //returnCode = (int)StoredProcedures.SP_MID_IS_SIZE_CURVE_GROUP_IN_USE.ReturnCode.Value;

				isInUse = Include.ConvertIntToBool(returnCode);
			
				return isInUse;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		// END MID Track #5240

		/// <summary>
		/// Delete the requested Size Curve Group
		/// </summary>
		/// <returns></returns>
		public void DeleteSizeCurveGroup(int aSizeCurveGroupRID)
		{
			try
			{
				// BEGIN MID Track #5240 - size curve group delete too slow
                StoredProcedures.SP_MID_SIZE_CURVE_GROUP_DELETE.Delete(_dba, SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID);
				// END MID Track #5240  
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Insert the given parameters in the SIZE_CURVE_GROUP_JOIN table
		/// </summary>
		/// <returns></returns>
		public void InsertSizeCurveGroupJoin(int aSizeCurveGroupRID, int aSizeCurveRID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_GROUP_JOIN_INSERT.Insert(_dba,
                                                                         SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID,
                                                                         SIZE_CURVE_RID: aSizeCurveRID
                                                                         );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes all records from the SIZE_CURVE_GROUP_JOIN table for the requested Size Curve Group
		/// </summary>
		/// <returns></returns>
		public void DeleteSizeCurveGroupJoin(int aSizeCurveGroupRID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_GROUP_JOIN_DELETE.Delete(_dba, SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID);
			}
			catch
			{
				throw;
			}
		}


  


		/// <summary>
		/// returns a datatable that contains the size curve requested plus the size codes within the curve
		/// </summary>
		/// <param name="sizeCurveRid"></param>
		/// <returns></returns>
		public DataTable GetSizeCurve(int sizeCurveRid)
		{
			try
			{
				//MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_CURVE_READ.Read(_dba,
                                                                 SIZE_CURVE_RID: sizeCurveRid,
                                                                 SIZE_CODE_SECONDARY: this.NoSizeDimensionLbl
                                                                 );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

      



        //Begin TT#827-MD -jsobek -Allocation Reviews Performance

        /// <summary>
        /// Gets all size curve data for a size curve group in one dataset
        /// </summary>
        /// <param name="headerRid"></param>
        /// <returns></returns>
        public DataSet GetSizeCurvesForGroup(int sizeCurveGroupRID)
        {
            try
            {
                return StoredProcedures.MID_GET_SIZE_CURVES_FOR_GROUP_READ.ReadAsDataSet(_dba, SizeCurveGroupRid: sizeCurveGroupRID);
            }
            catch
            {
                throw;
            }
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance

		/// <summary>
		/// Returns a datatable that contains the stores and size curve assigned to each within the size curve group
		/// </summary>
		/// <param name="sizeCurveGroupRid"></param>
		/// <returns></returns>
		public DataTable GetStoreSizeCurvesForSizeCurveGroup(int sizeCurveGroupRid)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_CURVE_GROUP_READ_FOR_SIZE_CURVE_GROUP.Read(_dba, SIZE_CURVE_RID: sizeCurveGroupRid);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the key of the given size curve name
		/// </summary>
		/// <returns></returns>
		public int GetSizeCurveKey(string aSizeCurveName)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SIZE_CURVE_READ_KEY.Read(_dba, SIZE_CURVE_NAME: aSizeCurveName);

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["SIZE_CURVE_RID"]));
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

		/// <summary>
		/// Insert the given parameters in the SIZE_CURVE table
		/// </summary>
		/// <returns></returns>
		public int InsertSizeCurve(string aSizeCurveName)
		{
			try
			{
                return StoredProcedures.SP_MID_SIZE_CURVE_INSERT.InsertAndReturnRID(_dba, SIZE_CURVE_NAME: aSizeCurveName);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Update the requested Size Curve
		/// </summary>
		/// <returns></returns>
		public void UpdateSizeCurve(int aSizeCurveRID, string aSizeCurveName)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_UPDATE.Update(_dba,
                                                              SIZE_CURVE_RID: aSizeCurveRID,
                                                              SIZE_CURVE_NAME: aSizeCurveName
                                                              );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Delete the requested Size Curve
		/// </summary>
		/// <returns></returns>
		public void DeleteSizeCurve(int aSizeCurveRID)
		{
			try
			{	// BEGIN MID Track #5268 - Size Curve Add/Update slow
                StoredProcedures.SP_MID_SIZE_CURVE_DELETE.Delete(_dba, SIZE_CURVE_RID: aSizeCurveRID);
				// END MID Track #5268  
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Insert the given parameters in the SIZE_CURVE_JOIN table
		/// </summary>
		/// <returns></returns>
		public void InsertSizeCurveJoin(int aSizeCurveRID, int aSizeCodeRID, float aPercent)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_JOIN_INSERT.Insert(_dba,
                                                                        SIZE_CURVE_RID: aSizeCurveRID,
                                                                        SIZE_CODE_RID: aSizeCodeRID,
                                                                        PERCENT: aPercent
                                                                        );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes all records from the SIZE_CURVE_JOIN table for the requested Size Curve
		/// </summary>
		/// <returns></returns>
		public void DeleteSizeCurveJoin(int aSizeCurveRID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_JOIN_DELETE.Delete(_dba, SIZE_CURVE_RID: aSizeCurveRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Insert the given parameters in the STORE_SIZE_CURVE_BY_GROUP table
		/// </summary>
		/// <returns></returns>
		public void InsertStoreSizeCurveByGroup(int aStoreRID, int aSizeCurveGroupRID, int aSizeCurveRID)
		{
			try
			{
                StoredProcedures.MID_STORE_SIZE_CURVE_BY_GROUP_INSERT.Insert(_dba,
                                                                             ST_RID: aStoreRID,
                                                                             SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID,
                                                                             SIZE_CURVE_RID: aSizeCurveRID
                                                                             );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes all records from the STORE_SIZE_CURVE_BY_GROUP table for the requested Size Curve Group
		/// </summary>
		/// <returns></returns>
		public void DeleteStoreSizeCurveByGroup(int aSizeCurveGroupRID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CURVE_GROUP_RID_DELETE.Delete(_dba, SIZE_CURVE_GROUP_RID: aSizeCurveGroupRID);
			}
			catch
			{
				throw;
			}
		}

		#endregion
	}
}
