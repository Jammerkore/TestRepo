using System;
using System.Collections;
using System.Data;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;
//using MIDRetail.Business;
//using MIDRetail.Business.Allocation;
using MIDRetail.Common;

namespace MIDRetail.Common
{
	#region SizeConstraintModelProfile
	/// <summary>
	/// Summary description for SizeModelProfiles.
	/// </summary>
	[Serializable()]
	public class SizeConstraintModelProfile : ModelProfile
	{
		private string _sizeConstraintName;
		private int _storeGroupRid;
		private int _sizeGroupRid;
		private int _sizeCurveGroupRid;
		//private eChangeType _modelChangeType;		// MID Track #5092 - Serialization error
		private CollectionSets _collectionSets;
		private DataSet _dsConstraints;
		//private SizeModelData _sizeModelData;		// MID Track #5092 - Serialization error

		#region Properties
		public string SizeConstraintName
		{
			get	{return _sizeConstraintName;}
			set	{_sizeConstraintName = value;}
		}
		public int StoreGroupRid
		{
			get	{return _storeGroupRid;}
			set	{_storeGroupRid = value;}
		}
		public int SizeGroupRid
		{
			get	{return _sizeGroupRid;}
			set	{_sizeGroupRid = value;}
		}
		public int SizeCurveGroupRid
		{
			get	{return _sizeCurveGroupRid;}
			set	{_sizeCurveGroupRid = value;}
		}
//		public eChangeType ModelChangeType			// BEGIN MID Track #5092 - Serialization error
//		{
//			get	{return _modelChangeType;}
//			set	{_modelChangeType = value;}			// END MID Track #5092  
//		}
        // Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
        //public CollectionSets CollectionSets
        //{
        //    get	
        //    {
        //        if (_collectionSets == null)
        //        {
        //            FillCollectionSets();
        //        }
        //        return _collectionSets;
        //    }
        //    //set	{_collectionSets = value;}
        //}
        public CollectionSets CollectionSets(int SG_Version)
        {
            if (_collectionSets == null)
            {
                FillCollectionSets(SG_Version);
            }
            return _collectionSets;

        }
		// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
//		public SizeModelData SizeModelData			// BEGIN MID Track #5092 - Serialization error
//		{
//			get	
//			{
//				if (_sizeModelData == null)
//				{
//					_sizeModelData = new SizeModelData();
//				}
//				return _sizeModelData;
//			}
//			//set	{_collectionSets = value;}
//		}											// END MID Track #5092 
		#endregion

		public SizeConstraintModelProfile(int aKey) 
			: base(aKey)
		{
			if (aKey == Include.NoRID)
			{
				Initialize();
			}
			else
			{	// BEGIN MID Track #5092 - Serialization error
				//DataTable dt = SizeModelData.SizeConstraintModel_Read(this.Key);
				SizeModelData sizeModelData = new SizeModelData();
				DataTable dt = sizeModelData.SizeConstraintModel_Read(this.Key);
				// END MID Track #5092  
				if (dt.Rows.Count == 0)
				{
					Initialize();
				}
				else
				{
					Fill(dt);
				}
			}
		}

		private void Initialize()
		{
			_sizeConstraintName = string.Empty;
			_storeGroupRid = Include.NoRID;
			_sizeGroupRid = Include.NoRID;
			_sizeCurveGroupRid = Include.NoRID;
			ModelChangeType = eChangeType.add;		// BEGIN MID Track #5092 - Serialization error (unrelated)
			_collectionSets = null;

		}

		private void Fill(DataTable dt)
		{
			DataRow aRow = dt.Rows[0];
			_sizeConstraintName = (string)aRow["SIZE_CONSTRAINT_NAME"];
			_sizeConstraintName = _sizeConstraintName.Trim();

			if (aRow["SG_RID"] == DBNull.Value)
				_storeGroupRid = Include.NoRID;
			else
				_storeGroupRid = Convert.ToInt32(aRow["SG_RID"], CultureInfo.CurrentUICulture);

			if (aRow["SIZE_GROUP_RID"] == DBNull.Value)
				_sizeGroupRid = Include.NoRID;
			else
				_sizeGroupRid = Convert.ToInt32(aRow["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);

			if (aRow["SIZE_CURVE_GROUP_RID"] == DBNull.Value)
				_sizeCurveGroupRid = Include.NoRID;
			else
				_sizeCurveGroupRid = Convert.ToInt32(aRow["SIZE_CURVE_GROUP_RID"], CultureInfo.CurrentUICulture);
			ModelChangeType = eChangeType.update;	//  MID Track #5092 - Serialization error (unrelated)
		}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeConstraintModel;
			}
		}


        // Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		//private bool FillCollectionSets()
		private bool FillCollectionSets(int SG_Version)
		// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		{
		    // Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
			//bool success = GetConstraintData();
            bool success = GetConstraintData(SG_Version);
			// ENd TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
			FillCollections();
			return success;
		}


		/// <summary>
		/// Fills the _dsConstraints DataSet.
		/// </summary>
		/// <returns></returns>
		// Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		//private bool GetConstraintData()
		private bool GetConstraintData(int SG_Version)
		// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
		{
			bool Success = true;

			try
			{
				_dsConstraints = MIDEnvironment.CreateDataSet();
				bool getSizes = false;
				
				if (SizeGroupRid != Include.NoRID)
				{
					getSizes = true;
				}
				if (SizeCurveGroupRid != Include.NoRID)
				{
					getSizes = true;
				}
				// BEGIN MID Track #5092 - Serialization error 
				//_dsConstraints = SizeModelData.GetChildData(_key, _storeGroupRid, getSizes);
				SizeModelData sizeModelData = new SizeModelData();
				DataTable dt = sizeModelData.SizeConstraintModel_Read(this.Key);
				// Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
				//_dsConstraints = sizeModelData.GetChildData(_key, _storeGroupRid, getSizes);
                _dsConstraints = sizeModelData.GetChildData(_key, _storeGroupRid, getSizes, SG_Version);
				// End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
				// END MID Track #5092  				
			}
			catch
			{
				Success = false;
				throw;
			}
			return Success;

		}

		/// <summary>
		/// Fills the constraint collection.  Collection can be retrieved with CollectionSets property.
		/// </summary>
		private void FillCollections()
		{
			int setIdx;
			int allColorIdx;
			int colorIdx;
			int allColorSizeIdx;
			int colorSizeIdx;
			int	colorSizeDimIdx;
			int allColorSizeDimIdx;

			if (_collectionSets != null)
			{
				_collectionSets.Clear();
			}

			_collectionSets = new CollectionSets();
			_collectionSets.StoreGroupRid = _storeGroupRid;  // Issue 4244

			if (_dsConstraints.Tables["SetLevel"].Rows.Count > 0)
			{
				//FOR EACH SET LEVEL
				foreach (DataRow drSet in _dsConstraints.Tables["SetLevel"].Rows)
				{
					setIdx = _collectionSets.Add(new ItemSet(_key, drSet));

					//ALL COLOR
					foreach (DataRow drAllColor in drSet.GetChildRows("SetAllColor"))
					{
						allColorIdx = _collectionSets[setIdx].collectionAllColors.Add(new ItemAllColor(_key, drAllColor));

						//SIZE DIMENSION
						if (_dsConstraints.Relations.Contains("AllColorSizeDimension"))
						{
							foreach (DataRow drACSizeDim in drAllColor.GetChildRows("AllColorSizeDimension"))
							{
								allColorSizeDimIdx = _collectionSets[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions.Add(new ItemSizeDimension(_key, drACSizeDim));
									
								//SIZE
								foreach (DataRow drACSize in drACSizeDim.GetChildRows("AllColorSize"))
								{
									allColorSizeIdx = _collectionSets[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions[allColorSizeDimIdx].collectionSizes.Add(new ItemSize(_key, drACSize));
								}
							}
						}
					}

					//COLOR
					foreach (DataRow drColor in drSet.GetChildRows("SetColor"))
					{
						colorIdx = _collectionSets[setIdx].collectionColors.Add(new ItemColor(_key, drColor));

						//SIZE DIMENSION
						if (_dsConstraints.Relations.Contains("ColorSizeDimension"))
						{
							foreach (DataRow drCSizeDim in drColor.GetChildRows("ColorSizeDimension"))
							{
								colorSizeDimIdx = _collectionSets[setIdx].collectionColors[colorIdx].collectionSizeDimensions.Add(new ItemSizeDimension(_key, drCSizeDim));
									
								//SIZE
								foreach (DataRow drCSize in drCSizeDim.GetChildRows("ColorSize"))
								{
									colorSizeIdx = _collectionSets[setIdx].collectionColors[colorIdx].collectionSizeDimensions[colorSizeDimIdx].collectionSizes.Add(new ItemSize(_key, drCSize));
								}
							}
						}
					}
				}
			}
		}
	}
	#endregion

	#region SizeAltModelProfile
	/// <summary>
	/// Size Alternate Model Profile
	/// </summary>
	[Serializable()]
	public class SizeAltModelProfile : ModelProfile
	{
		private string _sizeAltName;
		private int _primSizeCurveRid;
		private int _altSizeCurveRid;
		private ArrayList _alternateList; // list of SizeAlternatePrimary classes
		//private eChangeType _modelChangeType;  MID Track #4970 now inherited from ModelProfile
		//private SizeModelData _sizeModelData;	// MID Track #5092 - Serialization error

		
		#region Properties
		public string SizeAlternateName
		{
			get	{return _sizeAltName;}
			set	{_sizeAltName = value;}
		}
		public int PrimarySizeCurveRid
		{
			get	{return _primSizeCurveRid;}
			set	{_primSizeCurveRid = value;}
		}
		public int AlternateSizeCurveRid
		{
			get	{return _altSizeCurveRid;}
			set	{_altSizeCurveRid = value;}
		}
		/// <summary>
		/// A list of SizeAlternate classes.
		/// </summary>
		public ArrayList AlternateSizeList
		{
			get	{return _alternateList;}
			set	{_alternateList = value;}
		}
		//public eChangeType ModelChangeType	MID Track #4970 now inherited from ModelProfile
		//{
		//	get	{return _modelChangeType;}	
		//	set	{_modelChangeType = value;}
		//}
//		public SizeModelData SizeModelData		// BEGIN MID Track #5092 - Serialization error
//		{
//			get	
//			{
//				if (_sizeModelData == null)
//				{
//					_sizeModelData = new SizeModelData();
//				}
//				return _sizeModelData;
//			}
//		}										// END MID Track #5092  
		#endregion

		public SizeAltModelProfile(int aKey)
			: base(aKey)
		{
			if (aKey == Include.NoRID)
			{
				Initialize();
			}
			else
			{	// BEGIN MID Track #5092 - Serialization error
				//DataTable dt = SizeModelData.SizeAlternateModel_Read(this.Key);
				SizeModelData sizeModelData = new SizeModelData();
				DataTable dt = sizeModelData.SizeAlternateModel_Read(this.Key);
				// END MID Track #5092  
				if (dt.Rows.Count == 0)
				{
					Initialize();
				}
				else
				{
					Fill(dt, sizeModelData);	// MID Track #5092 - Serialization error
				}
			}
		}

		private void Initialize()
		{
			_sizeAltName = string.Empty;
			_primSizeCurveRid = Include.NoRID;
			_altSizeCurveRid = Include.NoRID;
			//_modelChangeType = eChangeType.add;	MID Track #4970 now inherited from ModelProfile
			ModelChangeType = eChangeType.add;
			_alternateList = new ArrayList();
		}

		//private void Fill(DataTable dt)	// BEGIN MID Track #5092 - Serialization error
		private void Fill(DataTable dt, SizeModelData aSizeModelData)	// END MID Track #5092 
		{
			DataRow aRow = dt.Rows[0];
			_sizeAltName = (string)aRow["SIZE_ALTERNATE_NAME"];
			_sizeAltName = _sizeAltName.Trim();
			ModelID = _sizeAltName;

			if (aRow["PRIMARY_SIZE_CURVE_RID"] == DBNull.Value)
				this._primSizeCurveRid = Include.NoRID;
			else
				_primSizeCurveRid = Convert.ToInt32(aRow["PRIMARY_SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);

			if (aRow["ALTERNATE_SIZE_CURVE_RID"] == DBNull.Value)
				_altSizeCurveRid = Include.NoRID;
			else
				_altSizeCurveRid = Convert.ToInt32(aRow["ALTERNATE_SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);

			_alternateList = new ArrayList();
			//FillAltSizeList();						// BEGIN MID Track #5092 - Serialization error
			FillAltSizeList(aSizeModelData);			// END MID Track #5092  
			//_modelChangeType = eChangeType.update;	MID Track #4970 now inherited from ModelProfile
			ModelChangeType = eChangeType.update;
		}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeAlternateModel;
			}
		}
		
		public void ClearAltSizeList()
		{
			try
			{
				foreach (SizeAlternatePrimary sap in _alternateList)
				{
					sap.AlternateList.Clear();
				}
				_alternateList.Clear();
			}
			catch
			{
				throw;
			}
		}

		//private void FillAltSizeList(SizeModelData aSizeModelData)	// BEGIN MID Track #5092 - Serialization error
		private void FillAltSizeList(SizeModelData aSizeModelData)		// END MID Track #5092 
		{
			try
			{	// BEGIN MID Track #5092 - Serialization error
				//DataTable dtPrimary = SizeModelData.SizeAltPrimarySize_Read(this.Key);
				//DataTable dtAlternate = SizeModelData.SizeAltAlternateSize_Read(this.Key);
				DataTable dtPrimary = aSizeModelData.SizeAltPrimarySize_Read(this.Key);
				DataTable dtAlternate = aSizeModelData.SizeAltAlternateSize_Read(this.Key);
				// END MID Track #5092  

				foreach (DataRow aRow in dtPrimary.Rows)
				{
					SizeAlternatePrimary aPrimary = new SizeAlternatePrimary();
					aPrimary.Sequence = Convert.ToInt32(aRow["PRIMARY_SIZE_SEQ"], CultureInfo.CurrentUICulture);
					aPrimary.SizeRid = Convert.ToInt32(aRow["SIZES_RID"], CultureInfo.CurrentUICulture);
					aPrimary.DimensionRid = Convert.ToInt32(aRow["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);

					DataRow [] altRows = dtAlternate.Select("PRIMARY_SIZE_SEQ = " + aPrimary.Sequence.ToString(CultureInfo.CurrentUICulture));

					foreach (DataRow aAlt in altRows)
					{	
						SizeAlternate aAlternate = new SizeAlternate();
						aAlternate.Sequence = Convert.ToInt32(aAlt["PRIMARY_SIZE_SEQ"], CultureInfo.CurrentUICulture);
						aAlternate.SizeRid = Convert.ToInt32(aAlt["SIZES_RID"], CultureInfo.CurrentUICulture);
						aAlternate.DimensionRid = Convert.ToInt32(aAlt["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);					
						aPrimary.AlternateList.Add(aAlternate);
					}
					this._alternateList.Add(aPrimary);
				}
			}
			catch
			{
				throw;
			}
		}

	}
	#endregion

	// begin MID Track 3619 Remove Fringe
	//#region SizeFringeModelProfile
	// /// <summary>
	// /// Size Alternate Model Profile
	// /// </summary>
	//public class SizeFringeModelProfile : Profile
	//{
	//	private string _sizeFringeName;
	//	private int _sizeCurveGroupRid;
	//	private ArrayList _fringeList; // list of SizeAlternatePrimary classes
	//	private ArrayList _filterList; // list of SizeAlternatePrimary classes
	//	private eFringeOverrideSort _sortOrder;
	//	private eChangeType _modelChangeType;
	//	private SizeModelData _sizeModelData;
    //
	//	
	//	#region Properties
	//	public string SizeFringeName
	//	{
	//		get	{return _sizeFringeName;}
	//		set	{_sizeFringeName = value;}
	//	}
	//	public int SizeCurveGroupRid
	//	{
	//		get	{return _sizeCurveGroupRid;}
	//		set	{_sizeCurveGroupRid = value;}
	//	}
	//	
	//	/// <summary>
	//	/// A list of SizeFringe classes.
	//	/// </summary>
	//	public ArrayList FringeList
	//	{
	//		get	{return _fringeList;}
	//		set	{_fringeList = value;}
	//	}
	//	/// <summary>
	//	/// A list of SizeFringeFilter classes.
	//	/// </summary>
	//	public ArrayList FilterList
	//	{
	//		get	{return _filterList;}
	//		set	{_filterList = value;}
	//	}
	//	public eFringeOverrideSort SortOrder
	//	{
	//		get	{return _sortOrder;}
	//		set	{_sortOrder = value;}
	//	}
	//	public eChangeType ModelChangeType
	//	{
	//		get	{return _modelChangeType;}
	//		set	{_modelChangeType = value;}
	//	}
	//	public SizeModelData SizeModelData
	//	{
	//		get	
	//		{
	//			if (_sizeModelData == null)
	//			{
	//				_sizeModelData = new SizeModelData();
	//			}
	//			return _sizeModelData;
	//		}
	//	}
	//	#endregion
    //
	//	public SizeFringeModelProfile(int aKey)
	//		: base(aKey)
	//	{
	//		if (aKey == Include.NoRID)
	//		{
	//			Initialize();
	//		}
	//		else
	//		{
	//			DataTable dt = SizeModelData.SizeFringeModel_Read(this.Key);
	//			if (dt.Rows.Count == 0)
	//			{
	//				Initialize();
	//			}
	//			else
	//			{
	//				Fill(dt);
	//			}
	//		}
	//	}
    //
	//	private void Initialize()
	//	{
	//		_sizeFringeName = string.Empty;
	//		_sizeCurveGroupRid = Include.NoRID;
	//		_fringeList = new ArrayList();
	//		_filterList = new ArrayList();
	//		_sortOrder = eFringeOverrideSort.Ascending;
    //
	//		_modelChangeType = eChangeType.add;
	//	}
    //
	//	private void Fill(DataTable dt)
	//	{
	//		DataRow aRow = dt.Rows[0];
	//		_sizeFringeName = (string)aRow["SIZE_FRINGE_NAME"];
	//		_sizeFringeName = _sizeFringeName.Trim();
    //
	//		if (aRow["SIZE_CURVE_RID"] == DBNull.Value)
	//			_sizeCurveGroupRid = Include.NoRID;
	//		else
	//			_sizeCurveGroupRid = Convert.ToInt32(aRow["SIZE_CURVE_RID"], CultureInfo.CurrentUICulture);
    //
	//		if (aRow["FRINGE_SORT_ORDER"] == DBNull.Value)
	//			_sortOrder = eFringeOverrideSort.Ascending;
	//		else
	//			_sortOrder = (eFringeOverrideSort)Convert.ToInt32(aRow["FRINGE_SORT_ORDER"], CultureInfo.CurrentUICulture);
    //
	//		_fringeList = new ArrayList();
	//		_filterList = new ArrayList();
    //
	//		FillFringeList();
	//		FillFilterList();
    //
	//		_modelChangeType = eChangeType.update;
	//	}
    //
	//	override public eProfileType ProfileType
	//	{
	//		get
	//		{
	//			return eProfileType.SizeAlternateModel;
	//		}
	//	}
    //
	//	private void FillFringeList()
	//	{
	//		try
	//		{
	//			DataTable dtFringe = SizeModelData.SizeFringe_Read(this.Key);
    //
	//			foreach (DataRow aRow in dtFringe.Rows)
	//			{
	//				SizeFringe aFringe = new SizeFringe();
	//				aFringe.SizeTypeRid = Convert.ToInt32(aRow["SIZE_TYPE_RID"], CultureInfo.CurrentUICulture);
	//				aFringe.FringeInd = Include.ConvertCharToBool(Convert.ToChar(aRow["FRINGE_IND"], CultureInfo.CurrentUICulture));
	//				aFringe.SizeType = (eEquateOverrideSizeType)Convert.ToInt32(aRow["SIZE_TYPE"], CultureInfo.CurrentUICulture);
	//				this._fringeList.Add(aFringe);
	//			}
	//		}
	//		catch
	//		{
	//			throw;
	//		}
	//	}
    //
	//	private void FillFilterList()
	//	{
	//		try
	//		{
	//			DataTable dtFringeFilter = SizeModelData.SizeFringeFilter_Read(this.Key);
    //
	//			foreach (DataRow aRow in dtFringeFilter.Rows)
	//			{
	//				SizeFringeFilter aFilter = new SizeFringeFilter();
	//				aFilter.Criteria = (eFringeOverrideUnitCriteria)Convert.ToInt32(aRow["FRINGE_CRITERIA_ENUM"], CultureInfo.CurrentUICulture);
	//				aFilter.Condition = (eFringeOverrideCondition)Convert.ToInt32(aRow["FRINGE_CONDITION_ENUM"], CultureInfo.CurrentUICulture);
	//				aFilter.Value = Convert.ToDecimal(aRow["FRINGE_FILTER_VALUE"], CultureInfo.CurrentUICulture);
	//				aFilter.ValueType = (eFringeFilterValueType)Convert.ToInt32(aRow["FRINGE_FILTER_VALUE_TYPE_ENUM"], CultureInfo.CurrentUICulture);
	//				this._filterList.Add(aFilter);
	//			}
	//		}
	//		catch
	//		{
	//			throw;
	//		}
	//	}
    //
	//	public void UpdateFringeList(int sizeTypeRid, eEquateOverrideSizeType sizeType, int fringeInd)
	//	{
	//		bool foundSize = false;
	//		foreach (SizeFringe aFringe in _fringeList)
	//		{
	//			if (sizeTypeRid == aFringe.SizeTypeRid && sizeType == aFringe.SizeType)
	//			{
	//				foundSize = true;
	//				switch (fringeInd)
	//				{
	//					case -1:
	//						_fringeList.Remove(aFringe);
	//						break;
	//					case 1:
	//						aFringe.FringeInd = true;
	//						break;
	//					case 0:
	//						aFringe.FringeInd = false;
	//						break;
	//				}
	//			}
	//			if (foundSize)
	//				break;
	//		}
    //
	//		if (fringeInd != Include.NoRID && !foundSize)
	//		{
	//			bool bFringeInd = ((fringeInd == 1) ? true : false);
    //
	//			SizeFringe aFringe = new SizeFringe(sizeTypeRid, bFringeInd, sizeType );
	//			_fringeList.Add(aFringe);
	//		}
	//	}
    //
	//	/// <summary>
	//	/// Returns the FringeInd as an int: true = 1, false = 0, and not found = -1.
	//	/// </summary>
	//	/// <param name="sizeTypeRid"></param>
	//	/// <param name="sizeType"></param>
	//	/// <returns></returns>
	//	public int GetFringeInd(int sizeTypeRid, eEquateOverrideSizeType sizeType)
	//	{
	//		int fringeInd = Include.NoRID;
	//		foreach (SizeFringe aFringe in this._fringeList)
	//		{
	//			if (sizeTypeRid == aFringe.SizeTypeRid && sizeType == aFringe.SizeType)
	//			{
	//				if (aFringe.FringeInd)
	//					fringeInd = 1;
	//				else
	//					fringeInd = 0;
	//				aFringe.Used = true;
	//				break;
	//			}
	//		}
    //
	//		return fringeInd;
	//	}
    //
    //
	//	/// <summary>
	//	/// It's possible for fringes to be defined, then have those sizes later removed from the 
	//	/// size curve.  This removes those finge definitions no longer used from the fingelist.
	//	/// </summary>
	//	public void CleanupFringeList()
	//	{
	//		ArrayList delFringeList = new ArrayList();
	//		foreach (SizeFringe aFringe in this._fringeList)
	//		{
	//			if (!aFringe.Used)
	//			{
	//				delFringeList.Add(aFringe);
	//			}
	//		}
    //
	//		foreach (SizeFringe aFringe in delFringeList)
	//		{
	//			_fringeList.Remove(aFringe);
	//		}
	//	}
    //
	//}
	//#endregion
    // end MID Track 3619 Remove Fringe
}

