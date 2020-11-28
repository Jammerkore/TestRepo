using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Used to retrieve a list of store grades
	/// </summary>
	[Serializable()]
	public class SizeGroupList : ProfileList
	{	
		// BEGIN MID Track #5092 - Serialization error
		//private MIDRetail.Data.SizeGroup _sizeGroupData;	// used to read/write profile to the data layer
		// END MID Track #5092 

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeGroupList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//_sizeGroupData = null;	// MID Track #5092 - Serialization error
		
		}

        public void LoadAll(bool IncludeUndefinedGroup, bool doReadSizeCodeListFromDatabase = true) //TT#1313-MD -jsobek -Header Filters -performance
		{
			SizeGroupProfile sgp;
			// BEGIN MID Track #5092 - Serialization error
			//if (_sizeGroupData == null) _sizeGroupData = new SizeGroup();
			
			//System.Data.DataTable dt = _sizeGroupData.GetSizeGroups(IncludeUndefinedGroup);

			SizeGroup sizeGroupData = new SizeGroup();
            System.Data.DataTable dt = sizeGroupData.GetSizeGroups(IncludeUndefinedGroup);
			// END MID Track #5092  

			foreach(System.Data.DataRow dr in dt.Rows)
			{
                sgp = new SizeGroupProfile(Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture), doReadSizeCodeListFromDatabase: doReadSizeCodeListFromDatabase); //TT#1313-MD -jsobek -Header Filters -performance
               
                // Begin TT#2966 - JSmith - Size Groups will not appear in the application for use
                // Begin TT#1313-MD - RMatelic - Header Filters 
                //sgp.SizeGroupName = Convert.ToString(dr["SIZE_GROUP_NAME"], CultureInfo.CurrentUICulture);
                if (Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture) != Include.UndefinedSizeGroupRID)
                {
                    sgp.SizeGroupName = Convert.ToString(dr["SIZE_GROUP_NAME"], CultureInfo.CurrentUICulture);
                }
                // End TT#1313-MD
                sgp.SizeGroupDescription = Convert.ToString(dr["SIZE_GROUP_DESCRIPTION"], CultureInfo.CurrentUICulture);
                // End TT#2966 - JSmith - Size Groups will not appear in the application for use
				this.Add(sgp);
			}
		}

//Begin Track #3607 - JScott - Duplicate key during save of new Group
		public SizeGroupProfile FindGroupName(string aGroupName)
		{
			try
			{
				foreach (SizeGroupProfile sgp in ArrayList)
				{
					if (sgp.SizeGroupName == aGroupName)
					{
						return sgp;
					}
				}

				return null;
			}
			catch (Exception exc)
			{
				string message = exc.Message;
				throw;
			}
		}
//End Track #3607 - JScott - Duplicate key during save of new Group
	}



	[Serializable]
	public class SizeGroupProfile : ModelProfile
	{
		//=======
		// FIELDS
		//=======
		// BEGIN MID Track #5092 - Serialization error
		//private MIDRetail.Data.SizeGroup _sizeGroupData;	// used to read/write profile to the data layer
		// END MID Track #5092  
		private string _sizeGroupName;
		private string _sizeGroupDesc;
		private SizeCodeList _sizeCodeList;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// 
		/// </summary>
		/// <param name="aKey"></param>
        public SizeGroupProfile(int aKey, bool doReadSizeCodeListFromDatabase = true) //TT#1313-MD -jsobek -Header Filters -performance
			:base(aKey)
		{
            // Begin TT#1313-MD - RMatelic - Header Filters 
            //this.Key = Include.UndefinedSizeGroupRID;
            this.Key = aKey;
            // End TT#11313-MD
            // Begin TT#1378-MD- RMatelic - Add soft text label for Unspecified Size Group
            //this.SizeGroupName = "Unspecified";
            this.SizeGroupName = MIDText.GetTextOnly(eMIDTextCode.lbl_Unspecified);
            // End TT#1378-MD
          	this.SizeCodeList = new SizeCodeList(eProfileType.SizeCode);
//Begin Track #3607 - JScott - Duplicate key during save of new Group
//			if (aKey != -1)
//			{
//				ReadSizeGroup(aKey);
//			}
//End Track #3607 - JScott - Duplicate key during save of new Group
            //Begin TT#1313-MD -jsobek -Header Filters
            if (doReadSizeCodeListFromDatabase)
            {
                ReadSizeGroup(aKey); //why go to the database everytime in the constructor?
            }
            //End TT#1313-MD -jsobek -Header Filters
		}

		//Begin TT#450 - JScott - SZ CURVE- node properties->tolerance->minimum average per size - store 1101 should get style curve, instead getting style/color chain curve - not correct
		public SizeGroupProfile()
			: base(Include.NoRID)
		{
			this.Key = Include.UndefinedSizeGroupRID;
            // Begin TT#1378-MD-Add soft text label for Unspecified Size Group
            //this.SizeGroupName = "None";
            this.SizeGroupName = MIDText.GetTextOnly(eMIDTextCode.lbl_Unspecified);
            // End TT#1378-MD
			this.SizeCodeList = new SizeCodeList(eProfileType.SizeCode);
		}

		//End TT#450 - JScott - SZ CURVE- node properties->tolerance->minimum average per size - store 1101 should get style curve, instead getting style/color chain curve - not correct
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeGroup;
			}
		}

		public string SizeGroupName
		{
			get
			{
				return _sizeGroupName;
			}
			set
			{
				_sizeGroupName = value;
			}
		}
		public string SizeGroupDescription
		{
			get
			{
				return _sizeGroupDesc;
			}
			set
			{
				_sizeGroupDesc = value;
			}
		}


		public SizeCodeList SizeCodeList
		{
			get
			{
				return _sizeCodeList;
			}
			set
			{
				_sizeCodeList = value;
			}
		}


		public void ReadSizeGroup()
		{
			try
			{	// BEGIN MID Track #5092 - Serialization error
				//if (_sizeGroupData == null) _sizeGroupData = new SizeGroup();
				//System.Data.DataTable dt = _sizeGroupData.GetSizeGroup(this.Key);

				SizeGroup sizeGroupData = new SizeGroup();
				System.Data.DataTable dt = sizeGroupData.GetSizeGroup(this.Key);
				// END MID Track #5092 
				LoadSizeGroup(dt);
			}
			catch
			{
				throw;
			}
		}
		public void ReadSizeGroup(int sizeGroupRID)
		{
			try
			{
				this.Key = sizeGroupRID;
				ReadSizeGroup();
			}
			catch
			{
				throw;
			}
		}
		public void ReadSizeGroup(string sizeGroupName)
		{
			try
			{	// BEGIN MID Track #5092 - Serialization error
				//if (_sizeGroupData == null) _sizeGroupData = new SizeGroup();
				//System.Data.DataTable dt = _sizeGroupData.GetSizeGroup(sizeGroupName);
				SizeGroup sizeGroupData = new SizeGroup();
				System.Data.DataTable dt = sizeGroupData.GetSizeGroup(sizeGroupName);
				// END MID Track #5092  
				LoadSizeGroup(dt);
			}
			catch
			{
				throw;
			}
		}

		private void LoadSizeGroup(System.Data.DataTable dt)
		{
			try
			{
				if (dt.Rows.Count < 1)
				{
					return;
				}
				System.Data.DataRow dr = dt.Rows[0];
				this.Key = Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
				_sizeGroupName = Convert.ToString(dr["SIZE_GROUP_NAME"], CultureInfo.CurrentUICulture);
				_sizeGroupDesc = Convert.ToString(dr["SIZE_GROUP_DESCRIPTION"], CultureInfo.CurrentUICulture);

				Hashtable primCodeSeqHash = new Hashtable();
				Hashtable secCodeSeqHash	= new Hashtable();
				int nextPrimSeq = 1;
				int nextSecSeq = 1;

				foreach (System.Data.DataRow drSize in dt.Rows)
				{
					SizeCodeProfile scp = new SizeCodeProfile(-1);
					scp.Key = Convert.ToInt32(drSize["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
					scp.SizeCodeID	= (string)drSize["SIZE_CODE_ID"];
					scp.SizeCodePrimary = (string)drSize["SIZE_CODE_PRIMARY"];
					scp.SizeCodeSecondary = (drSize["SIZE_CODE_SECONDARY"] != DBNull.Value) ? (string)drSize["SIZE_CODE_SECONDARY"] : string.Empty;
//					scp.SizeCodeSecondary = (string)drSize["SIZE_CODE_SECONDARY"];
					scp.SizeCodeProductCategory = (string)drSize["SIZE_CODE_PRODUCT_CATEGORY"];
//					scp.SizeCodeTableName = (string)drSize["SIZE_CODE_TABLE_NAME"];
//					scp.SizeCodeHeading1 = (drSize["SIZE_CODE_HEADING1"] != DBNull.Value) ? (string)drSize["SIZE_CODE_HEADING1"] : "";
//					scp.SizeCodeHeading2 = (drSize["SIZE_CODE_HEADING2"] != DBNull.Value) ? (string)drSize["SIZE_CODE_HEADING2"] : "";
//					scp.SizeCodeHeading3 = (drSize["SIZE_CODE_HEADING3"] != DBNull.Value) ? (string)drSize["SIZE_CODE_HEADING3"] : "";
//					scp.SizeCodeHeading4 = (drSize["SIZE_CODE_HEADING4"] != DBNull.Value) ? (string)drSize["SIZE_CODE_HEADING4"] : "";
					scp.SizeCodePrimaryRID = Convert.ToInt32(drSize["SIZES_RID"], CultureInfo.CurrentUICulture);
					scp.SizeCodeSecondaryRID = Convert.ToInt32(drSize["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);
//					if (drSize["SIZE_CODE_MULTIPLE_SIZE_CODE"] != DBNull.Value)
//						scp.SizeCodeMultipleSizeCode = Convert.ToInt32(drSize["SIZE_CODE_MULTIPLE_SIZE_CODE"], CultureInfo.CurrentUICulture);
//					else
//						scp.SizeCodeMultipleSizeCode = 0;

					//=============================================================================================
					// this takes the size rows and assigns a primary sequence, based upon the primary size code
					// and a secondary sequence based up the secondary size code
					//=============================================================================================
					//=============================================================================================
					// this takes the size rows and assigns a primary sequence, based upon the primary size code
					// and a secondary sequence based up the secondary size code
					//=============================================================================================
					if (primCodeSeqHash.ContainsKey(scp.SizeCodePrimary))
					{
						scp.PrimarySequence = (int)primCodeSeqHash[scp.SizeCodePrimary];
					}
					else
					{
						scp.PrimarySequence = nextPrimSeq;
						primCodeSeqHash.Add(scp.SizeCodePrimary, nextPrimSeq);
						nextPrimSeq++;
					}
					if (secCodeSeqHash.ContainsKey(scp.SizeCodeSecondary))
					{
						scp.SecondarySequence = (int)secCodeSeqHash[scp.SizeCodeSecondary];
					}
					else
					{
						scp.SecondarySequence = nextSecSeq;
						secCodeSeqHash.Add(scp.SizeCodeSecondary, nextSecSeq);
						nextSecSeq++;
					}

					_sizeCodeList.Add(scp);
				}
			}
			catch
			{
				throw;
			}
		}

		public void WriteSizeGroup()
		{
			// BEGIN MID Track #5092 - Serialization error
			//if (_sizeGroupData == null) _sizeGroupData = new SizeGroup();
			SizeGroup sizeGroupData = new SizeGroup();
			// END MID Track #5092  
			try
			{
				System.Data.DataTable dt = MIDEnvironment.CreateDataTable("SizeGroup");
				
				dt.Columns.Add("SIZE_GROUP_RID");		
				dt.Columns.Add("SIZE_GROUP_NAME");		
				dt.Columns.Add("SIZE_GROUP_DESCRIPTION");
				dt.Columns.Add("WIDTH_ACROSS_IND");
				dt.Columns.Add("SIZE_CODE_RID");

				foreach (SizeCodeProfile scp in this.SizeCodeList)
				{
					dt.Rows.Add(new object[] { this.Key, this.SizeGroupName, this.SizeGroupDescription, "0", scp.Key } );
				}
				// BEGIN MID Track #5092 - Serializable error
//				_sizeGroupData.OpenUpdateConnection();
//				bool createFlag = (this.Key < 1);
//				if (createFlag)
//				{
//					this.Key = _sizeGroupData.CreateSizeGroup(dt);
//				}
//				else
//				{
//					bool success = _sizeGroupData.UpdateSizeGroup(dt);
//				}
//
//
//				_sizeGroupData.CommitData();
				
				sizeGroupData.OpenUpdateConnection();
				bool createFlag = (this.Key < 1);
				if (createFlag)
				{
					this.Key = sizeGroupData.CreateSizeGroup(dt);
				}
				else
				{
					bool success = sizeGroupData.UpdateSizeGroup(dt);
				}


				sizeGroupData.CommitData();
			}	// END MID Track #5092  
			catch
			{
				throw;
			}
			finally
			{
				//_sizeGroupData.CloseUpdateConnection();	// BEGIN MID Track #5092 - Serialization error
				sizeGroupData.CloseUpdateConnection();		// END MID Track #5092  
			}
		}

		public void DeleteSizeGroup()
		{
			// BEGIN MID Track #5092 - Serialization error
			//if (_sizeGroupData == null) _sizeGroupData = new SizeGroup();
			SizeGroup  sizeGroupData = new SizeGroup();
			 
			try
			{
				sizeGroupData.OpenUpdateConnection();
				if(this.Key < 1)
				{
					return;
				}
				sizeGroupData.DeleteSizeGroup(this.Key);
				sizeGroupData.CommitData();

			}
			catch
			{
				throw;
			}
			finally
			{
				sizeGroupData.CloseUpdateConnection();
			}
			// END MID Track #5092 
		}

		// returns the requested size code profile out of the group's
		// sizeCodeList.  Returns null if size code not found.
		public SizeCodeProfile GetSizeCode(int sizeCodeRid)
		{
			SizeCodeProfile scp = (SizeCodeProfile)_sizeCodeList.FindKey(sizeCodeRid);
			return scp;
		}

		/// <summary>
		/// Returns an ArrayList of SizeCodes that match the Size and Dimension passed in.
		/// Normally this will be a single SizeCode. But if the size or dimension is -1, then
		/// every size code matching the other paramter will be returned.
		/// </summary>
		/// <param name="sizesRid"></param>
		/// <param name="dimensionsRid"></param>
		/// <returns></returns>
		public ArrayList GetSizeCodeList(int sizesRid, int dimensionsRid)
		{
			ArrayList sizeCodeList = new ArrayList();

			int cnt = _sizeCodeList.Count;


			// Returns all size codes matching the Dimension
			if (sizesRid == Include.NoRID)
			{
				for (int s=0;s<cnt;s++)
				{
					SizeCodeProfile scp = (SizeCodeProfile)_sizeCodeList[s];
					if (dimensionsRid == scp.SizeCodeSecondaryRID)
					{
						sizeCodeList.Add(scp);
					}
				}
			}
			// returns all size codes matching the size
			else if (dimensionsRid == Include.NoRID)
			{
				for (int s=0;s<cnt;s++)
				{
					SizeCodeProfile scp = (SizeCodeProfile)_sizeCodeList[s];
					if (sizesRid == scp.SizeCodePrimaryRID)
					{
						sizeCodeList.Add(scp);
					}
				}
			}
			// returns the specific size code requested
			else
			{
				for (int s=0;s<cnt;s++)
				{
					SizeCodeProfile scp = (SizeCodeProfile)_sizeCodeList[s];
					if (sizesRid == scp.SizeCodePrimaryRID
						&& dimensionsRid == scp.SizeCodeSecondaryRID)
					{
						sizeCodeList.Add(scp);
						break;
					}
				}
			}
			
			return sizeCodeList;
		}

		
	}
}
