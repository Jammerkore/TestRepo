using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for SizeCurveGroupProfile.
	/// </summary>
	[Serializable()]
	public class SizeCurveGroupProfile : ModelProfile
	{
		//private SizeCurve _sizeCurveData;			// BEGIN MID Track #5092 - Serialization error
		//private SizeGroup _sizeGroupData;			// END MID Track #5092  

		private bool _filled;
		private string _sizeCurveGroupName;
		private int _defaultSizeCurveRid;
		private int _definedSizeGroupRID;
		private SizeCurveProfile _defaultSizeCurve;
		private ProfileList _sizeCurveList;		// array of SizeCurveProfiles;
		private Hashtable _storeSizeCurveHash;	// hash keyed by store rid and holds size curve RID;
		private ProfileList _sizeCodeList;		// array of unique SizeCodeProfiles admist all curves;



		#region Properties
		// BEGIN MID Track #5092 - Serialization error
//		private SizeCurve SizeCurveData
//		{
//			get
//			{
//				if (_sizeCurveData == null)
//				{
//					_sizeCurveData = new SizeCurve();
//				}
//				return _sizeCurveData;
//			}
//		}
//		private SizeGroup SizeGroupData
//		{
//			get
//			{
//				if (_sizeGroupData == null)
//				{
//					_sizeGroupData = new SizeGroup();
//				}
//				return _sizeGroupData;
//			}
//		}
		// END MID Track #5092  
		public bool Filled 
		{
			get { return _filled ; }
			set { _filled = value; }
		}
		public string SizeCurveGroupName 
		{
			get { return _sizeCurveGroupName ; }
			set { _sizeCurveGroupName = (value == null) ? value : value.Trim(); }
		}
		public int DefaultSizeCurveRid 
		{
			get { return _defaultSizeCurveRid ; }
			set { _defaultSizeCurveRid = value; }
		}
		public int DefinedSizeGroupRID 
		{
			get { return _definedSizeGroupRID ; }
			set { _definedSizeGroupRID = value; }
		}
		public SizeCurveProfile DefaultSizeCurve 
		{
			get { return _defaultSizeCurve ; }
			set { _defaultSizeCurve = value; }
		}
		public ProfileList SizeCurveList 
		{
			get { return _sizeCurveList ; }
			set { _sizeCurveList = value; }
		}
		public Hashtable StoreSizeCurveHash 
		{
			get { return _storeSizeCurveHash ; }
		}
		#endregion

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCurveGroup;
			}
		}

		/// <summary>
		/// Returns unique size code list across all size curves.
		/// </summary>
		public ProfileList SizeCodeList
		{
			get
			{
				if (_sizeCodeList == null)
					return GetSizeCodeList();
				else
					return _sizeCodeList;
			}
		}

		public SizeCurveGroupProfile(int sizeCurveGroupRid)
			:base(sizeCurveGroupRid)
		{
			_filled = false;
			_sizeCurveList = new SizeCodeList(eProfileType.SizeCode);
			_storeSizeCurveHash = new Hashtable();
			_defaultSizeCurveRid = Include.NoRID;
			_definedSizeGroupRID = Include.NoRID;

			if (sizeCurveGroupRid != Include.NoRID)
			{
				Populate(sizeCurveGroupRid);
			}
		}

		private void Populate(int sizeCurveGroupRid)
		{
			try
			{
				SizeCurve SizeCurveData = new SizeCurve();	// MID Track #5092 - Serialization error
				//==================
				// Fill Size Curves
				//==================

                //Begin TT#827-MD -jsobek -Allocation Reviews Performance

				//DataTable dtSizeCurves = SizeCurveData.GetSizeCurvesForSizeCurveGroup(sizeCurveGroupRid);
                //for (int i = 0; i < dtSizeCurves.Rows.Count; i++)
                //{
                //    DataRow curveRow = dtSizeCurves.Rows[i];
                //    if (i == 0) // first row
                //    {
                //        _sizeCurveGroupName = curveRow["SIZE_CURVE_GROUP_NAME"].ToString();
                //        if (curveRow["DEFAULT_SIZE_CURVE_RID"] != DBNull.Value)
                //            _defaultSizeCurveRid = Convert.ToInt32(curveRow["DEFAULT_SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);
                //        if (curveRow["DEFINED_SIZE_GROUP_RID"] != DBNull.Value)
                //            _definedSizeGroupRID = Convert.ToInt32(curveRow["DEFINED_SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
                //        _filled = true;
                //    }
                //    if (curveRow["SIZE_CURVE_RID"] != DBNull.Value)
                //    {
                //        int sizeCurveRid = Convert.ToInt32(curveRow["SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);
                //        SizeCurveProfile scp = new SizeCurveProfile(sizeCurveRid);
                //        _sizeCurveList.Add(scp);
                //    }
                //}

                DataSet dsSizeCurves = SizeCurveData.GetSizeCurvesForGroup(sizeCurveGroupRid);
                DataTable dtGroupNameAndDefaultCurveRID = dsSizeCurves.Tables["GroupNameAndDefaultCurveRID"];
                DataTable dtSizeCurveData = dsSizeCurves.Tables["SizeCurveData"];

                for (int i = 0; i < dtGroupNameAndDefaultCurveRID.Rows.Count; i++)
				{
					if (i==0) // first row
					{
                        _sizeCurveGroupName = dtGroupNameAndDefaultCurveRID.Rows[0]["SIZE_CURVE_GROUP_NAME"].ToString();
                        if (dtGroupNameAndDefaultCurveRID.Rows[0]["DEFAULT_SIZE_CURVE_RID"] != DBNull.Value)
                            _defaultSizeCurveRid = Convert.ToInt32(dtGroupNameAndDefaultCurveRID.Rows[0]["DEFAULT_SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);
                        if (dtGroupNameAndDefaultCurveRID.Rows[0]["DEFINED_SIZE_GROUP_RID"] != DBNull.Value)
                            _definedSizeGroupRID = Convert.ToInt32(dtGroupNameAndDefaultCurveRID.Rows[0]["DEFINED_SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
						_filled = true;
					}
                    if (dtGroupNameAndDefaultCurveRID.Rows[i]["SIZE_CURVE_RID"] != DBNull.Value)
					{
                        int sizeCurveRid = Convert.ToInt32(dtGroupNameAndDefaultCurveRID.Rows[i]["SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);

                        DataRow[] sizeCurveDataRows = dtSizeCurveData.Select("SIZE_CURVE_RID=" + sizeCurveRid);
                        SizeCurveProfile scp = new SizeCurveProfile(sizeCurveRid, sizeCurveDataRows);
						_sizeCurveList.Add(scp);
					}
				}

                //End TT#827-MD -jsobek -Allocation Reviews Performance

				if (_defaultSizeCurveRid != Include.NoRID)
				{
					_defaultSizeCurve = (SizeCurveProfile)_sizeCurveList.FindKey(_defaultSizeCurveRid);
					if (_defaultSizeCurve == null)
					{
						_defaultSizeCurve = new SizeCurveProfile(_defaultSizeCurveRid);
						_defaultSizeCurve.SizeCurveName = "Default Curve";
					}
				}

				//========================
				// Set Order of Size Codes
				//========================
				Resequence();

				//=========================
				// Fill Store Size Curves
				//=========================
				DataTable dtStoreSizeCurves = SizeCurveData.GetStoreSizeCurvesForSizeCurveGroup(sizeCurveGroupRid);

				int rowCount = dtStoreSizeCurves.Rows.Count;
				for (int i=0;i<rowCount;i++)
				{
					DataRow storeCurveRow = dtStoreSizeCurves.Rows[i];
				
					if (storeCurveRow["ST_RID"] != DBNull.Value)
					{
						int storeRid = Convert.ToInt32(storeCurveRow["ST_RID"], CultureInfo.CurrentUICulture);
						int storeSizeCurveRid = Convert.ToInt32(storeCurveRow["SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);
						SizeCurveProfile scp = (SizeCurveProfile)_sizeCurveList.FindKey(storeSizeCurveRid);
						_storeSizeCurveHash.Add(storeRid, scp);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private Hashtable GetSizes()
		{
			DataView priView;
			DataView secView;
			SizeCodeSortInfo scsi;
			SizeGroup SizeGroupData = new SizeGroup();		// MID Track #5092 - Serialization error
			try
			{
				DataTable dtSizes = null;
				// Find all sizes that might be contained in this Size Curve Group
				if (_definedSizeGroupRID == Include.NoRID)
				{
					dtSizes = MIDEnvironment.CreateDataTable();
					dtSizes.Columns.Add("SIZE_CODE_RID", typeof(int));
					dtSizes.Columns.Add("SIZE_CODE_PRIMARY", typeof(string));
					dtSizes.Columns.Add("SIZE_CODE_SECONDARY", typeof(string));
					dtSizes.PrimaryKey = new DataColumn[] { dtSizes.Columns["SIZE_CODE_RID"] };

					foreach (SizeCurveProfile sizeCurveProf in SizeCurveList)
					{
						foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
						{
							if (!dtSizes.Rows.Contains(sizeCodeProf.Key))
							{
								dtSizes.Rows.Add(new object[] { sizeCodeProf.Key, sizeCodeProf.SizeCodePrimary, sizeCodeProf.SizeCodeSecondary });
							}
						}
					}

					if (DefaultSizeCurve != null)
					{
						foreach (SizeCodeProfile sizeCodeProf in DefaultSizeCurve.SizeCodeList)
						{
							if (!dtSizes.Rows.Contains(sizeCodeProf.Key))
							{
								dtSizes.Rows.Add(new object[] { sizeCodeProf.Key, sizeCodeProf.SizeCodePrimary, sizeCodeProf.SizeCodeSecondary });
							}
						}
					}

					priView = new DataView(dtSizes);
					secView = new DataView(dtSizes);

					priView.Sort = "SIZE_CODE_PRIMARY";
					secView.Sort = "SIZE_CODE_SECONDARY";
				}
				else
				{
					dtSizes = SizeGroupData.GetSizeGroup(_definedSizeGroupRID);
					priView = dtSizes.DefaultView;
					secView = dtSizes.DefaultView;
				}

				Hashtable primCodeSeqHash	= new Hashtable();
				Hashtable secCodeSeqHash	= new Hashtable();
				Hashtable sizesHash			= new Hashtable();
				int nextPrimSeq = 1;
				int nextSecSeq = 1;

				foreach (DataRowView drv in priView)
				{
					DataRow sizeRow = drv.Row;
					if (sizeRow["SIZE_CODE_RID"] != DBNull.Value)
					{
						int sizeCodeRid = Convert.ToInt32(sizeRow["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
						scsi = new SizeCodeSortInfo();

						//=============================================================================================
						// this takes the size rows and assigns a primary sequence based upon the primary size code
						//=============================================================================================
						string sizeCodePrimary = sizeRow["SIZE_CODE_PRIMARY"].ToString();
						if (primCodeSeqHash.ContainsKey(sizeCodePrimary))
						{
							scsi.PrimarySequence = (int)primCodeSeqHash[sizeCodePrimary];
						}
						else
						{
							scsi.PrimarySequence = nextPrimSeq;
							primCodeSeqHash.Add(sizeCodePrimary, nextPrimSeq);
							nextPrimSeq++;
						}

						sizesHash.Add(sizeCodeRid, scsi);
					}
				}

				foreach (DataRowView drv in secView)
				{
					DataRow sizeRow = drv.Row;
					if (sizeRow["SIZE_CODE_RID"] != DBNull.Value)
					{
						int sizeCodeRid = Convert.ToInt32(sizeRow["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
						scsi = (SizeCodeSortInfo)sizesHash[sizeCodeRid];

						//=============================================================================================
						// this takes the size rows and assigns a secondary sequence based up the secondary size code
						//=============================================================================================
						string sizeCodeSecondary = sizeRow["SIZE_CODE_SECONDARY"].ToString();
						if (secCodeSeqHash.ContainsKey(sizeCodeSecondary))
						{
							scsi.SecondarySequence = (int)secCodeSeqHash[sizeCodeSecondary];
						}
						else
						{
							scsi.SecondarySequence = nextSecSeq;
							secCodeSeqHash.Add(sizeCodeSecondary, nextSecSeq);
							nextSecSeq++;
						}
					}
				}

				return sizesHash;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Using the the Defined Size Group (or lack there of), resequences the sizes in the curves
		/// based upon the Defined Size Group sequence.
		/// </summary>
		public void Resequence()
		{
			//====================================================================================
			// returns all sizes in for Curve Group and has assigned primary/secondary sequences
			//====================================================================================
			Hashtable sizesHash;

			try
			{
				sizesHash = GetSizes();

				//=============================
				// Set sequences of Size Curves
				//=============================
				foreach (SizeCurveProfile sizeCurveProf in SizeCurveList)
				{
					foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
					{
						SetSequence(sizeCodeProf, sizesHash);
					}
					sizeCurveProf.Sort();
				}

				//=====================================
				// Set sequences of Default Size Curves
				//=====================================
				if (DefaultSizeCurve != null)
				{
					foreach (SizeCodeProfile sizeCodeProf in DefaultSizeCurve.SizeCodeList)
					{
						SetSequence(sizeCodeProf, sizesHash);
					}
					DefaultSizeCurve.Sort();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Set the sequences of a given Size Code Profile with the values in the given Hashtable of
		/// sequences.
		/// </summary>
		private void SetSequence(SizeCodeProfile aSizeCodeProf, Hashtable aSizesHash)
		{
			SizeCodeSortInfo scsi;

			try
			{
				if (aSizesHash.ContainsKey(aSizeCodeProf.Key))
				{
					scsi = (SizeCodeSortInfo)aSizesHash[aSizeCodeProf.Key];
					aSizeCodeProf.PrimarySequence = scsi.PrimarySequence;
					aSizeCodeProf.SecondarySequence = scsi.SecondarySequence;
				}
				else
				{
					aSizeCodeProf.PrimarySequence = int.MaxValue;
					aSizeCodeProf.SecondarySequence = int.MaxValue;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Write the curve group to the database
		/// </summary>
		// BEGIN MID Track #5268 - Size Curve Add/Update slow 
		//public void WriteSizeCurveGroup(SessionAddressBlock aSAB)
		//Begin TT#301 - Begin - SZ HST-> Size curve disappears in the Size need method after processing the Size curve method
		public void WriteSizeCurveGroup(SessionAddressBlock aSAB, bool aCurvesInGroupUpdated)
		{
			WriteSizeCurveGroup(aSAB, aCurvesInGroupUpdated, false);
		}

		public void WriteSizeCurveGroup(SessionAddressBlock aSAB, bool aCurvesInGroupUpdated, bool aClearGroup)
		//End TT#301 - Begin - SZ HST-> Size curve disappears in the Size need method after processing the Size curve method
		{
			//SizeCurveProfile strSizeCurveProf;
			//IDictionaryEnumerator hashEnum;
			// END MID Track #5268 
			try
			{
				SizeCurve SizeCurveData = new SizeCurve();		// MID Track #5092 - Serialization error
				SizeCurveData.OpenUpdateConnection();
				try
				{
					// Update the default SizeCurve object

					_defaultSizeCurve.WriteSizeCurve(SizeCurveData, true, aSAB);

					// Update the SizeCurveGroup objects

					if (this.Key < 0)
					{
						this.Key = SizeCurveData.InsertSizeCurveGroup(_sizeCurveGroupName, _defaultSizeCurve.Key, _definedSizeGroupRID);
                        // Begin TT#3122 - JSmith - Size curve upload failed due to integrity constraint issue
                        SizeCurveData.CommitData();
                        // End TT#3122 - JSmith - Size curve upload failed due to integrity constraint issue
					}
					else
					{
						//Begin TT#301 - Begin - SZ HST-> Size curve disappears in the Size need method after processing the Size curve method
						if (aClearGroup)
						{
							SizeCurveData.ClearSizeCurveGroup(_sizeCurveGroupName);
						}

						//End TT#301 - Begin - SZ HST-> Size curve disappears in the Size need method after processing the Size curve method
						SizeCurveData.DeleteSizeCurveGroupJoin(this.Key);
						SizeCurveData.UpdateSizeCurveGroup(this.Key, _sizeCurveGroupName, _defaultSizeCurve.Key, _definedSizeGroupRID);
					}
                    // begin MID Track xxxx Default Size Key not in SizeCurveGroupJoin
					//if (_defaultSizeCurve.Key != Include.NoRID)
					//{
					//	SizeCurveData.InsertSizeCurveGroupJoin(this.Key, _defaultSizeCurve.Key);  
					//}
                    // end MID Track xxxx Default Size Key not in SizeCurveGroupJoin

					// Update the SizeCurve objects

					foreach (SizeCurveProfile sizeCurveProf in _sizeCurveList)
					{
						// BEGIN MID Track #5268 - Size Curve Add/Update slow
						//sizeCurveProf.WriteSizeCurve(SizeCurveData, false, aSAB);
						if (!aCurvesInGroupUpdated)
						{
							sizeCurveProf.WriteSizeCurve(SizeCurveData, false, aSAB);
						}
						// END MID Track #5268  
						SizeCurveData.InsertSizeCurveGroupJoin(this.Key, sizeCurveProf.Key);
					}

					// Update the default StoreSizeCurveByGroup objects

					// BEGIN MID Track #5268 - Size Curve Add/Update slow - move to separate method
					//SizeCurveData.DeleteStoreSizeCurveByGroup(this.Key);

					//hashEnum = _storeSizeCurveHash.GetEnumerator();

					//while (hashEnum.MoveNext())
					//{
					//	strSizeCurveProf = (SizeCurveProfile)hashEnum.Value;
					//	SizeCurveData.InsertStoreSizeCurveByGroup(Convert.ToInt32(hashEnum.Key), this.Key, strSizeCurveProf.Key);
					//}

					WriteStoreSizeCurveByGroup(SizeCurveData);
					// END MID Track #5268 

					SizeCurveData.CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					SizeCurveData.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		// BEGIN MID Track #5268 - Size Curve Add/Update slow
		public void UpdateStoreSizeCurveByGroup()
		{
			SizeCurve SizeCurveData = new SizeCurve();
			try
			{
				SizeCurveData.OpenUpdateConnection();
				WriteStoreSizeCurveByGroup(SizeCurveData);

				SizeCurveData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				SizeCurveData.CloseUpdateConnection();
			}
		}

		private void WriteStoreSizeCurveByGroup(SizeCurve aSizeCurveData)
		{
			SizeCurveProfile strSizeCurveProf;
			IDictionaryEnumerator hashEnum;
			try
			{
				aSizeCurveData.DeleteStoreSizeCurveByGroup(this.Key);

				hashEnum = _storeSizeCurveHash.GetEnumerator();

				while (hashEnum.MoveNext())
				{
					strSizeCurveProf = (SizeCurveProfile)hashEnum.Value;
					aSizeCurveData.InsertStoreSizeCurveByGroup(Convert.ToInt32(hashEnum.Key), this.Key, strSizeCurveProf.Key);
				}
			}
			catch
			{
				throw;
			}
		}
		// END MID Track #5268 

		// BEGIN MID Track #5240 - size curve group delete too slow
		public bool SizeCurveGroupIsInUse()
		{
			SizeCurve SizeCurveData = new SizeCurve();
			try
			{
				SizeCurveData.OpenUpdateConnection();

				return (SizeCurveData.SizeCurveGroupIsInUse(this.Key));
			}
			catch
			{
				throw;
			}
			finally
			{
				SizeCurveData.CloseUpdateConnection();
			}
		}
		// END MID Track #5240 

		/// <summary>
		/// Delete the curve group from the database
		/// </summary>
		public void DeleteSizeCurveGroup()
		{
			try
			{
				if (this.Key < 0)
				{
					return;
				}
				SizeCurve SizeCurveData = new SizeCurve();	//  MID Track #5092 - Serialization error
				SizeCurveData.OpenUpdateConnection();

				try
				{// BEGIN MID Track #5240 - size curve group delete too slow
					//SizeCurveData.DeleteStoreSizeCurveByGroup(this.Key);
					//SizeCurveData.DeleteSizeCurveGroupJoin(this.Key);

					//foreach (SizeCurveProfile sizeCurveProf in _sizeCurveList)
					//{
					//	sizeCurveProf.DeleteSizeCurve(SizeCurveData);
					//}

					SizeCurveData.DeleteSizeCurveGroup(this.Key);
					//_defaultSizeCurve.DeleteSizeCurve(SizeCurveData);
					
					// END MID Track #5240
					SizeCurveData.CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					SizeCurveData.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sizeCurveRid"></param>
		/// <returns></returns>
		public SizeCurveProfile GetSizeCurveProfile(int sizeCurveRid)
		{
			try
			{
				return (SizeCurveProfile)_sizeCurveList.FindKey(sizeCurveRid);
			}
			catch
			{
				throw;
			}
		}

		public SizeCurveProfile GetStoreSizeCurveProfile(int storeRid)
		{
			SizeCurveProfile scp;

			try
			{
				if (_storeSizeCurveHash.ContainsKey(storeRid))
				{
					scp = (SizeCurveProfile)_storeSizeCurveHash[storeRid];
				}
				else
				{
					scp = _defaultSizeCurve;
				}
			
				return scp;
			}
			catch
			{
				throw;
			}
		}

		public void SetStoreSizeCurveProfile(int storeRid, int sizeCurveRid)
		{
			SizeCurveProfile scp;

			try
			{
				scp = (SizeCurveProfile)_sizeCurveList.FindKey(sizeCurveRid);

				if (scp != null)
				{
					_storeSizeCurveHash[storeRid] = scp;
				}
			}
			catch
			{
				throw;
			}
		}

		public void DeleteStoreSizeCurveProfile(int storeRid)
		{
			try
			{
				if (_storeSizeCurveHash.Contains(storeRid))
				{
					_storeSizeCurveHash.Remove(storeRid);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// returns a profile list of all unique size codes throughout all the curves in the size curve group.
		/// </summary>
		/// <returns></returns>
		public ProfileList GetSizeCodeList()
		{
			try
			{
				if (_sizeCodeList == null)
				{
					ProfileList sizeCodeList = new ProfileList(eProfileType.SizeCode);

					foreach (SizeCurveProfile sizeCurveProf in SizeCurveList)
					{
						foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
						{
							if (!sizeCodeList.Contains(sizeCodeProf.Key))
								sizeCodeList.Add(sizeCodeProf);
						}
					}

					if (DefaultSizeCurve != null)
					{
						foreach (SizeCodeProfile sizeCodeProf in DefaultSizeCurve.SizeCodeList)
						{
							if (!sizeCodeList.Contains(sizeCodeProf.Key))
								sizeCodeList.Add(sizeCodeProf);
						}
					}

					// Begin MID ISSUE # 3078 - stodd
					MIDGenericSortItem[] sortedSize = new MIDGenericSortItem[sizeCodeList.Count];
					for (int s=0; s<sizeCodeList.Count; s++)
					{
						SizeCodeProfile scp = (SizeCodeProfile)sizeCodeList[s];
						sortedSize[s].Item = s;
						sortedSize[s].SortKey = new double[2];
						sortedSize[s].SortKey[0] = Convert.ToDouble(scp.PrimarySequence, CultureInfo.CurrentUICulture);
						sortedSize[s].SortKey[1] = Convert.ToDouble(scp.SecondarySequence, CultureInfo.CurrentUICulture);
					}

                    Array.Sort(sortedSize, new MIDGenericSortAscendingComparer());  // TT#1143 - MD - Jellis - Group ALlocation Min Broken
				
					ProfileList sortedSizeList = new ProfileList(eProfileType.SizeCode);
					foreach (MIDGenericSortItem mgsiSize in sortedSize)
					{
						int index = mgsiSize.Item;
						sortedSizeList.Add(sizeCodeList[index]);
					}

					_sizeCodeList = sortedSizeList;
				}

				return _sizeCodeList;
				// End MID ISSUE # 3078 - stodd
			}
			catch
			{
				throw;
			}
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

			int cnt = this.SizeCodeList.Count; 


			// Returns all size codes matching the Dimension
			if (sizesRid == Include.NoRID)
			{
				for (int s=0;s<cnt;s++)
				{
					SizeCodeProfile scp = (SizeCodeProfile)this.SizeCodeList[s];
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
					SizeCodeProfile scp = (SizeCodeProfile)this.SizeCodeList[s];
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
					SizeCodeProfile scp = (SizeCodeProfile)this.SizeCodeList[s];
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
	
	public class SizeCodeSortInfo
	{
		private int _primarySequence;
		private int _secondarySequence;

		public SizeCodeSortInfo()
		{
			_primarySequence = int.MaxValue;
			_secondarySequence = int.MaxValue;
		}

		public int PrimarySequence
		{
			get
			{
				return _primarySequence;
			}
			set
			{
				_primarySequence = value;
			}
		}

		public int SecondarySequence
		{
			get
			{
				return _secondarySequence;
			}
			set
			{
				_secondarySequence = value;
			}
		}
	}

	/// <summary>
	/// Used to retrieve the list of store curve groups 
	/// </summary>
	[Serializable()]
	public class SizeCurveGroupList : ProfileList
	{
		// BEGIN MID Track #5092 - Serialization error
		//private MIDRetail.Data.SizeCurve _sizeCurveData;	// used to read/write profile to the data layer
		// END MID Track #5092 

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveGroupList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//_sizeCurveData = null;		// MID Track #5092 - Serialization error
		
		}

		public void LoadAll(bool IncludeUndefinedGroup)
		{
			SizeCurveGroupProfile scgp;
			// BEGIN MID Track #5092 - Serialization error
			//if (_sizeCurveData == null) 
			//	_sizeCurveData = new MIDRetail.Data.SizeCurve();
			
			//System.Data.DataTable dt = _sizeCurveData.GetSizeCurveGroups();
			SizeCurve sizeCurveData = new SizeCurve();
			System.Data.DataTable dt = sizeCurveData.GetSizeCurveGroups();
			// END MID Track #5092 
			if (IncludeUndefinedGroup)
			{
				scgp = new SizeCurveGroupProfile(Include.NoRID);
				this.Add(scgp);
			}

			foreach(System.Data.DataRow dr in dt.Rows)
			{
				scgp = new SizeCurveGroupProfile(Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"], CultureInfo.CurrentUICulture));
				this.Add(scgp);
			}
		}

	}
}
