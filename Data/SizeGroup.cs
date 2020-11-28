using System;
using System.Data;
using System.Globalization;
//using System.Collections;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for Size.
	/// </summary>
	public partial class SizeGroup : DataLayer
	{
		string NoSizeDimensionLbl;  // MID Track 3914 Constraints displays "-1" for dimension when no dims

		public SizeGroup() : base()
		{
			NoSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize); // MID Track 3914 Constraints displays "-1" for dimension when no dim
		}

		public DataTable GetSizeGroups(bool IncludeUndefinedGroup)
		{
			try
			{
				if (IncludeUndefinedGroup)
				{
                    // MID Track # 2354 - removed nolock because it causes concurrency issues
                    // TT#2966 - JSmith - Size Groups will not appear in the application for use
                    return StoredProcedures.MID_SIZE_GROUP_READ_ALL.Read(_dba);
				}
				else
				{
                    return StoredProcedures.MID_SIZE_GROUP_READ_ALL_DEFINED.Read(_dba);
				}
						
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetSizeGroup(int sizeGroupRID)
		{
			try
			{
                return StoredProcedures.MID_SIZE_GROUP_READ.Read(_dba, SIZE_GROUP_RID: sizeGroupRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		public DataTable GetSizeGroup(string sizeGroupName)
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_GROUP_READ_FROM_GROUP_NAME.Read(_dba, SIZE_GROUP_NAME: sizeGroupName);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#1510 - JSmith - Add size group to release output
        public string GetSizeGroupName(int sizeGroupRID)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_SIZE_GROUP_READ_NAME.Read(_dba, SIZE_GROUP_RID: sizeGroupRID);

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToString(dt.Rows[0]["SIZE_GROUP_NAME"]);
                }
                else
                {
                    return "Unknown";
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1510
		
		// BEGION ANF Generic Size Constraint
       
        public DataTable SizeGroup_FilterReadCaseSensitive(string sizeGroupName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_GROUP_READ_FROM_NAME.Read(_dba, SIZE_GROUP_NAME: sizeGroupName);
            }
            catch
            {
                throw;
            }
        }
        public DataTable SizeGroup_FilterReadCaseInsensitive(string sizeGroupName)
        {
            try
            {
                return StoredProcedures.MID_SIZE_GROUP_READ_FROM_NAME_UPPER.Read(_dba, SIZE_GROUP_NAME: sizeGroupName);
            }
            catch
            {
                throw;
            }
        }
		// END ANF Generic Size Constraint

		public int CreateSizeGroup(DataTable dt)
		{
			try
			{
				if (dt.Rows.Count < 1)
					return -1;

				DataRow dr = dt.Rows[0];
                int sizeGroupRID = StoredProcedures.SP_MID_SIZE_GROUP_INSERT.InsertAndReturnRID(_dba,
                                                                                                SIZE_GROUP_NAME: Convert.ToString(dr["SIZE_GROUP_NAME"], CultureInfo.CurrentUICulture),
                                                                                                SIZE_GROUP_DESCRIPTION: Convert.ToString(dr["SIZE_GROUP_DESCRIPTION"], CultureInfo.CurrentUICulture),
                                                                                                WIDTH_ACROSS_IND: Convert.ToChar(dr["WIDTH_ACROSS_IND"], CultureInfo.CurrentUICulture)
                                                                                                );
				AddSizeGroupCodes(sizeGroupRID, dt);

				return sizeGroupRID;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}



		public bool UpdateSizeGroup(DataTable dt)
		{
			try
			{
				if (dt.Rows.Count < 1)
					return false;

				DataRow dr = dt.Rows[0];

				// first delete the existing Size Group
				int sizeGroupRID = Convert.ToInt32(dr["SIZE_GROUP_RID"],CultureInfo.CurrentUICulture );
				DeleteSizeGroupCodes(sizeGroupRID);
				AddSizeGroupCodes(sizeGroupRID, dt);

                char WIDTH_ACROSS_IND = Convert.ToString(dr["WIDTH_ACROSS_IND"], CultureInfo.CurrentUICulture).ToCharArray()[0];

                int rowsUpdated = StoredProcedures.MID_SIZE_GROUP_UPDATE.Update(_dba,
                                                                                SIZE_GROUP_RID: sizeGroupRID,
                                                                                SIZE_GROUP_NAME: Convert.ToString(dr["SIZE_GROUP_NAME"], CultureInfo.CurrentUICulture),
                                                                                SIZE_GROUP_DESCRIPTION: Convert.ToString(dr["SIZE_GROUP_DESCRIPTION"], CultureInfo.CurrentUICulture),
                                                                                WIDTH_ACROSS_IND: WIDTH_ACROSS_IND
                                                                                );
                return (rowsUpdated > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void DeleteSizeGroup(int sizeGroupRID)
		{
			try
			{
                StoredProcedures.SP_MID_SIZE_GROUP_DELETE.Delete(_dba, SIZE_GROUP_RID: sizeGroupRID);
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		private void DeleteSizeGroupCodes(int sizeGroupRID)
		{
            StoredProcedures.MID_SIZE_GROUP_JOIN_DELETE.Delete(_dba, SIZE_GROUP_RID: sizeGroupRID);
		}

		private void AddSizeGroupCodes(int sizeGroupRID, DataTable dt)
		{
			int seq = 1;
			foreach(DataRow dr in dt.Rows)
			{
                int SIZE_CODE_RID;
                int.TryParse(dr["SIZE_CODE_RID"].ToString(), out SIZE_CODE_RID);
                seq++;
                StoredProcedures.MID_SIZE_GROUP_JOIN_INSERT.Insert(_dba,
                                                                   SIZE_GROUP_RID: sizeGroupRID,
                                                                   SIZE_CODE_RID: SIZE_CODE_RID,
                                                                   SEQ: seq
                                                                   );
			}
		}

		public DataTable Sizes_Read()
		{
			try
			{
				//MID Track # 2354 - removed nolock because it causes concurrency issues
				//MID Track 3844 Size Constraints and Rules not working
                return StoredProcedures.MID_SIZE_CODE_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int Size_Add(string sizeID, string primary, string secondary,
			string productCategory)
		{
			try
			{
				// set the secondary to null if there is no value
				if (secondary != null)
				{
					if (secondary.Trim().Length == 0)
					{
						secondary = null;
					}
				}

                return StoredProcedures.SP_MID_SIZE_INSERT.InsertAndReturnRID(_dba,
                                                                             SIZE_CODE_ID: sizeID,
                                                                             SIZE_CODE_PRIMARY: primary,
                                                                             SIZE_CODE_SECONDARY: secondary,
                                                                             SIZE_CODE_PRODUCT_CATEGORY: productCategory
                                                                             );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Size_Update(int sizeCodeRID, string sizeID, string primary, string secondary,
			string productCategory)
		{
			try
			{
				// set the secondary to null if there is no value
				if (secondary != null)
				{
					if (secondary.Trim().Length == 0)
					{
						secondary = null;
					}
				}
                StoredProcedures.MID_SIZE_CODE_UPDATE.Update(_dba,
                                                             SIZE_CODE_RID: sizeCodeRID,
                                                             SIZE_CODE_ID: sizeID,
                                                             SIZE_CODE_PRIMARY: primary,
                                                             SIZE_CODE_SECONDARY: secondary,
                                                             SIZE_CODE_PRODUCT_CATEGORY: productCategory
                                                             );
				CheckSizesTable(primary);
				CheckDimensionsTable(secondary);
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable ProductCategory_Read()
		{
			try
			{
				//MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable PrimarySizes_Read()
		{
			try
			{
				// MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SIZES_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetPrimarySizeRID(string aPrimary)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SIZES_READ_SIZE_CODE_PRIMARY.Read(_dba, SIZE_CODE_PRIMARY: aPrimary);
				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["SIZES_RID"], CultureInfo.CurrentUICulture);
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

		public void CheckSizesTable(string primary)
		{
			try
			{
				// MID Track # 2354 - removed nolock because it causes concurrency issues
                int count = StoredProcedures.MID_SIZES_READ_COUNT.ReadRecordCount(_dba, SIZE_CODE_PRIMARY: primary);
				if (count == 0)
				{
                    StoredProcedures.MID_SIZES_INSERT.Insert(_dba, SIZE_CODE_PRIMARY: primary);
				}
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		public DataTable SecondarySizes_Read()
		{
			try
			{
				// MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_DIMENSIONS_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetSecondarySizeRID(string aSecondary)
		{
			try
			{
				if (aSecondary == null || aSecondary.Trim().Length == 0)
				{
					return Include.NoRID;
				}

                DataTable dt = StoredProcedures.MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY.Read(_dba, SIZE_CODE_SECONDARY: aSecondary);
				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);
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

		public void CheckDimensionsTable(string aSecondary)
		{
			try
			{
				if (aSecondary == null)
				{
					return;
				}

				// MID Track # 2354 - removed nolock because it causes concurrency issues
                int count = StoredProcedures.MID_DIMENSIONS_READ_COUNT.ReadRecordCount(_dba, SIZE_CODE_SECONDARY: aSecondary);
				if (count == 0)
				{
                    StoredProcedures.MID_DIMENSIONS_INSERT.Insert(_dba, SIZE_CODE_SECONDARY: aSecondary);
				}
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void Size_Delete(int sizeCodeRID)
		{
			try
			{
                StoredProcedures.MID_SIZE_CODE_DELETE.Delete(_dba, SIZE_CODE_RID: sizeCodeRID);
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool Size_Exists(string sizeID)
		{
			try
			{
                return (StoredProcedures.MID_SIZE_CODE_READ_COUNT.ReadRecordCount(_dba, SIZE_CODE_ID: sizeID) > 0);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public DataTable GetSizeForPrimarySecondary(string aPrimaryID, string aSecondaryID)
		{
			try
			{
				// MID Track 3844 Size Constraints and Rules not working
                return StoredProcedures.MID_SIZE_CODE_READ_PRIMARY_AND_SECONDARY.Read(_dba,
                                                                                      SIZE_CODE_PRIMARY: aPrimaryID,
                                                                                      SIZE_CODE_SECONDARY: aSecondaryID
                                                                                      );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public int GetSizeCodeRID(string aSizeID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SIZE_CODE_READ_RID.Read(_dba, SIZE_CODE_ID: aSizeID);
				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
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

        public void GetSizeCodeID(int SizeCodeRID, out string SizeCodePrimary, out string SizeCodeSecondary)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_SIZE_CODE_READ.Read(_dba, SIZE_CODE_RID: SizeCodeRID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    SizeCodePrimary = dr["SIZE_CODE_PRIMARY"].ToString().Trim();
                    SizeCodeSecondary = dr["SIZE_CODE_SECONDARY"].ToString().Trim();
                }
                else
                {
                    SizeCodePrimary = "";
                    SizeCodeSecondary = "";
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

	}
}
