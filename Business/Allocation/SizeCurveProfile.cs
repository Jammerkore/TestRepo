using System;
using System.Collections;
using System.Data;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for SizeCurveProfile.
	/// </summary>
// BEGIN MID Track #2486 - Add curve copy
//	public class SizeCurveProfile : Profile
	[Serializable]								// MID Track #5092 - Serialization error
	public class SizeCurveProfile : Profile, ICloneable
// END MID Track #2486 - Add curve copy
	{
		//private SizeCurve _sizeCurveData;		// MID Track #5092 - Serialization error
		private string _sizeCurveName;
		private SizeCodeList _sizeCodeList; // array of SizeCodeProfiles;
		private bool _filled;

		#region Properties
		/// <summary>
		/// Gets the Size Curve Name.
		/// </summary>
		public string SizeCurveName 
		{
			get { return _sizeCurveName ; }
			set { _sizeCurveName = (value == null) ? value : value.Trim(); }
		}
		// BEGIN MID Track #5092 - Serialization error
		//private SizeCurve SizeCurveData					
		//{
		//	get
		//	{
		//		if (_sizeCurveData == null)
		//		{
		//			_sizeCurveData = new SizeCurve();
		//		}
		//		return _sizeCurveData;
		//	}
		//}
		// END MID Track #5092
		public bool Filled 
		{
			get { return _filled ; }
			//set { _sizeCurveName = value; }
		}
		/// <summary>
		/// Gets a list of SizeCurveProfiles
		/// </summary>
		public SizeCodeList SizeCodeList
		{
			get { return _sizeCodeList ; }
			//set { _sizeCurveName = value; }
		}
		
		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCurve;
			}
		}
		#endregion 

		/// <summary>
		/// CONSTRUCTOR
		/// </summary>
		/// <param name="sizeCurveRid"></param>
		public SizeCurveProfile(int sizeCurveRid)
			:base(sizeCurveRid)
		{
			_filled = false;
			_sizeCodeList = new SizeCodeList(eProfileType.SizeCode);

			if (sizeCurveRid >= 0)
			{
				Populate();
			}
		}

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        public SizeCurveProfile(int sizeCurveRid, DataRow[] sizeCurveRows)
            : base(sizeCurveRid)
        {
            _filled = false;
            _sizeCodeList = new SizeCodeList(eProfileType.SizeCode);
 
            if (sizeCurveRid >= 0)
            {
                SizeCurve SizeCurveData = new SizeCurve();
                PopulateFromDataRows(sizeCurveRows);
            }
        }

        public SizeCurveProfile(int sizeCurveRid, bool doPopulate)
            : base(sizeCurveRid)
        {
            _filled = false;
            _sizeCodeList = new SizeCodeList(eProfileType.SizeCode);

            if (doPopulate)
            {
                if (sizeCurveRid >= 0)
                {
                    Populate();
                }
            }
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance

// BEGIN MID Track #2486 - Add curve copy
		public object Clone()
		{
			SizeCurveProfile newSizeCurveProf;

			newSizeCurveProf = new SizeCurveProfile(_key);
			newSizeCurveProf._sizeCurveName = _sizeCurveName;
			newSizeCurveProf._filled = _filled;
			newSizeCurveProf._sizeCodeList = new SizeCodeList(eProfileType.SizeCode);

			foreach (SizeCodeProfile sizeCodeProf in _sizeCodeList)
			{
				newSizeCurveProf._sizeCodeList.Add((SizeCodeProfile)sizeCodeProf.Clone());
			}

			return newSizeCurveProf;
		}

// END MID Track #2486 - Add curve copy

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        public object CloneNoCopy()
        {
            SizeCurveProfile newSizeCurveProf;

            newSizeCurveProf = new SizeCurveProfile(_key, false);
            newSizeCurveProf._sizeCurveName = _sizeCurveName;
            newSizeCurveProf._filled = _filled;
            newSizeCurveProf._sizeCodeList = new SizeCodeList(eProfileType.SizeCode);


            return newSizeCurveProf;
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance



		/// <summary>
		/// fills this SizeCurveProfile
		/// </summary>
		private void Populate()
		{	// BEGIN MID Track #5092 - Serialization error
			SizeCurve SizeCurveData = new SizeCurve();		
			// END MID Track #5092 

			DataTable dtSizeCurve = SizeCurveData.GetSizeCurve(this.Key);
            PopulateFromDataRows(dtSizeCurve.Select());
		}
       
        private void PopulateFromDataRows(DataRow[] sizeCurveRows)
        {
            try
            {
                //int rowCount = dtSizeCurve.Rows.Count;
                int rowCount = sizeCurveRows.Length;
                for (int i = 0; i < rowCount; i++)
                {
                    //DataRow sizeRow = dtSizeCurve.Rows[i];
                    DataRow sizeRow = sizeCurveRows[i];
                    if (i == 0) // first row
                    {
                        _sizeCurveName = sizeRow["SIZE_CURVE_NAME"].ToString();
                        _filled = true;
                    }
                    if (sizeRow["SIZE_CODE_RID"] != DBNull.Value)
                    {
                        int sizeCodeRid = Convert.ToInt32(sizeRow["SIZE_CODE_RID"]);
                        SizeCodeProfile scp = new SizeCodeProfile(sizeCodeRid);
                        scp.SizeCodeID = sizeRow["SIZE_CODE_ID"].ToString();
                        scp.SizeCodePrimary = sizeRow["SIZE_CODE_PRIMARY"].ToString();
                        scp.SizeCodeSecondary = sizeRow["SIZE_CODE_SECONDARY"].ToString();
                        scp.SizeCodeProductCategory = sizeRow["SIZE_CODE_PRODUCT_CATEGORY"].ToString();
                        //						scp.SizeCodeTableName = sizeRow["SIZE_CODE_TABLE_NAME"].ToString();
                        //						scp.SizeCodeHeading1 = sizeRow["SIZE_CODE_HEADING1"].ToString();
                        //						scp.SizeCodeHeading2 = sizeRow["SIZE_CODE_HEADING2"].ToString();
                        //						scp.SizeCodeHeading3 = sizeRow["SIZE_CODE_HEADING3"].ToString();
                        //						scp.SizeCodeHeading4 = sizeRow["SIZE_CODE_HEADING4"].ToString();
                        //						scp.SizeCodeMultipleSizeCode = Convert.ToInt32(sizeRow["SIZE_CODE_MULTIPLE_SIZE_CODE"]);
                        scp.SizeCodePrimaryRID = Convert.ToInt32(sizeRow["SIZES_RID"]);
                        scp.SizeCodeSecondaryRID = Convert.ToInt32(sizeRow["DIMENSIONS_RID"]);
                        scp.SizeCodePercent = Convert.ToSingle(sizeRow["PERCENT"]);
                        AddSizeCode(scp);
                    }
                }
            }
            catch
            {
                throw;
            }

        }

		/// <summary>
		/// Write the curve to the database
		/// </summary>
		public void WriteSizeCurve(bool aSaveZeroValues, SessionAddressBlock aSAB)
		{
			SizeCurve dlSizeCurve;

			try
			{
				dlSizeCurve = new SizeCurve();
				dlSizeCurve.OpenUpdateConnection();

				try
				{
					WriteSizeCurve(dlSizeCurve, aSaveZeroValues, aSAB);
					dlSizeCurve.CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					dlSizeCurve.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Write the curve to the database
		/// </summary>
		public void WriteSizeCurve(SizeCurve aSizeCurveData, bool aSaveZeroValues, SessionAddressBlock aSAB)
		{
			//SizeGroup dlSizeGroup;					// BEGIN MID Track #5268 - Size Curve Add/Update slow

			try
			{
				//dlSizeGroup = new SizeGroup();
				//dlSizeGroup.OpenUpdateConnection();	// END MID Track #5268  

				try
				{
					if (this.Key < 0)
					{
						this.Key = aSizeCurveData.InsertSizeCurve(_sizeCurveName);
                        // Begin TT#3122 - JSmith - Size curve upload failed due to integrity constraint issue
                        aSizeCurveData.CommitData();
                        // End TT#3122 - JSmith - Size curve upload failed due to integrity constraint issue
					}
					else
					{
						aSizeCurveData.DeleteSizeCurveJoin(this.Key);
						aSizeCurveData.UpdateSizeCurve(this.Key, _sizeCurveName);
					}

					foreach (SizeCodeProfile sizeCodeProf in _sizeCodeList)
					{
						if (sizeCodeProf.Key < 0)
						{
//							sizeCodeProf.Key = dlSizeGroup.Size_Add(sizeCodeProf.SizeCodeID, sizeCodeProf.SizeCodePrimary, sizeCodeProf.SizeCodeSecondary,
//								sizeCodeProf.SizeCodeProductCategory);
							SizeCodeProfile scp = aSAB.HierarchyServerSession.SizeCodeUpdate(sizeCodeProf);
							sizeCodeProf.Key = scp.Key;
						}

						if (aSaveZeroValues || sizeCodeProf.SizeCodePercent != 0)
						{
							aSizeCurveData.InsertSizeCurveJoin(this.Key, sizeCodeProf.Key, sizeCodeProf.SizeCodePercent);
						}
					}

					//dlSizeGroup.CommitData();			// BEGIN/END MID Track #5268 - Size Curve Add/Update slow
				}
				catch
				{
					throw;
				}
				// BEGIN MID Track #5268 - Size Curve Add/Update slow
				//finally
				//{
				//	dlSizeGroup.CloseUpdateConnection();
				//}
				// END MID Track #5268  
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Delete the curve to the database
		/// </summary>
		public void DeleteSizeCurve()
		{
			SizeCurve dlSizeCurve;

			try
			{
				dlSizeCurve = new SizeCurve();
				dlSizeCurve.OpenUpdateConnection();

				try
				{
					DeleteSizeCurve(dlSizeCurve);
					dlSizeCurve.CommitData();
				}
				catch
				{
					throw;
				}
				finally
				{
					dlSizeCurve.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Delete the curve from the database
		/// </summary>
		public void DeleteSizeCurve(SizeCurve aSizeCurveData)
		{
			try
			{
				if (this.Key < 0)
				{
					return;
				}

				// BEGIN MID Track #5268 - Size Curve Add/Update slow
				//aSizeCurveData.DeleteSizeCurveJoin(this.Key);
				// END MID Track #5268 
				aSizeCurveData.DeleteSizeCurve(this.Key);
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// adds a SizeCodeProfile to the Size Curve
		/// </summary>
		/// <param name="scp"></param>
		public void AddSizeCode(SizeCodeProfile scp)
		{
			_sizeCodeList.Add(scp);
		}

		/// <summary>
		/// returns a specific Size Code Profile
		/// </summary>
		/// <param name="sizeCodeRid"></param>
		/// <returns></returns>
		public SizeCodeProfile GetSizeCode(int sizeCodeRid)
		{
			SizeCodeProfile scp = (SizeCodeProfile)_sizeCodeList.FindKey(sizeCodeRid);
			return scp;
		}

		/// <summary>
		/// Returns the percent for a specific size
		/// </summary>
		/// <param name="sizeCodeRid"></param>
		/// <returns></returns>
		public float GetSizePercent(int sizeCodeRid)
		{
			SizeCodeProfile scp = GetSizeCode(sizeCodeRid);
			if (scp != null)
			{
				return scp.SizeCodePercent;
			}
			return 0.0f;
		}

		/// <summary>
		/// Sorts sizes in size curve by primary/seconday sequence.
		/// </summary>
		public void Sort()
		{
			_sizeCodeList.ArrayList.Sort(new SizeCurveSort());
		}

	}

	/// <summary>
	/// Places Sizes in Size curve in order by primary and secondary seq.
	/// </summary>
	class SizeCurveSort:IComparer
	{
		public SizeCurveSort()
		{
		}

		public int Compare(object x,object y)
		{
			int result = 0;
			result = ((SizeCodeProfile)x).PrimarySequence.CompareTo(((SizeCodeProfile)y).PrimarySequence);
			if (result != 0)
			{
				return result;
			} 
			result = ((SizeCodeProfile)x).SecondarySequence.CompareTo(((SizeCodeProfile)y).SecondarySequence);
			return result;
		}		
	}
}
