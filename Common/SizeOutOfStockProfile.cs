using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the size out of stock for a group level for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeOutOfStockHeaderProfile : Profile
	{
		// Fields

		private eChangeType _changeType;
		private bool _recordExists;

		private bool _strGrpIsInherited;
		private int _strGrpIsInheritedFromNodeRID;
		private int _strGrpRID;
		private bool _sizeGrpIsInherited;
		private int _sizeGrpIsInheritedFromNodeRID;
		private int _sizeGrpRID;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		//public SizeOutOfStockProfile(int aNodeRID)
		public SizeOutOfStockHeaderProfile()
			: base(0)
		{
			_changeType = eChangeType.none;
			_recordExists = false;

			_strGrpIsInherited = false;
			_strGrpIsInheritedFromNodeRID = Include.NoRID;
			_strGrpRID = Include.NoRID;
			_sizeGrpIsInherited = false;
			_sizeGrpIsInheritedFromNodeRID = Include.NoRID;
			_sizeGrpRID = Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeOutOfStockHeader;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a group level's out of stock information.
		/// </summary>
		public eChangeType ChangeType
		{
			get { return _changeType; }
			set { _changeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a group level's out of stock information is inherited.
		/// </summary>
		public bool RecordExists
		{
			get { return _recordExists; }
			set { _recordExists = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a group level's out of stock information is inherited.
		/// </summary>
		public bool StrGrpIsInherited
		{
			get { return _strGrpIsInherited; }
			set { _strGrpIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the group level's out of stock information is inherited from.
		/// </summary>
		public int StrGrpIsInheritedFromNodeRID
		{
			get { return _strGrpIsInheritedFromNodeRID; }
			set { _strGrpIsInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the store group identified for the node.
		/// </summary>
		public int StoreGrpRID
		{
			get { return _strGrpRID; }
			set { _strGrpRID = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a group level's out of stock information is inherited.
		/// </summary>
		public bool SizeGrpIsInherited
		{
			get { return _sizeGrpIsInherited; }
			set { _sizeGrpIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the group level's out of stock information is inherited from.
		/// </summary>
		public int SizeGrpIsInheritedFromNodeRID
		{
			get { return _sizeGrpIsInheritedFromNodeRID; }
			set { _sizeGrpIsInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the size group identified for the node.
		/// </summary>
		public int SizeGrpRID
		{
			get { return _sizeGrpRID; }
			set { _sizeGrpRID = value; }
		}
	}

	/// <summary>
	/// Contains the information about the size out of stock for a group level for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeOutOfStockProfile : SizeOutOfStockHeaderProfile
	{
		// Fields

		private DataSet _dsValues;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		//public SizeOutOfStockProfile(int aNodeRID)
		public SizeOutOfStockProfile()
			: base()
		{
			_dsValues = null;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeOutOfStock;
			}
		}

		/// <summary>
		/// Gets or sets the size group identified for the node.
		/// </summary>
		public DataSet dsValues
		{
			get { return _dsValues; }
			set { _dsValues = value; }
		}
	}

	/// <summary>
	/// Contains the information about the size sell thru for a group level for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeSellThruProfile : Profile
	{
		// Fields

		private eChangeType _changeType;
		private bool _recordExists;

		private bool _sellThruLimitIsInherited;
		private int _sellThruLimitInheritedFromNodeRID;
		private float _sellThruLimit;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeSellThruProfile()
			: base(0)
		{
			_changeType = eChangeType.none;
			_recordExists = false;

			_sellThruLimitIsInherited = false;
			_sellThruLimitInheritedFromNodeRID = Include.NoRID;
			_sellThruLimit = Include.Undefined;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeSellThru;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a group level's sell thru information.
		/// </summary>
		public eChangeType ChangeType
		{
			get { return _changeType; }
			set { _changeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a group level's sell thru information is inherited.
		/// </summary>
		public bool RecordExists
		{
			get { return _recordExists; }
			set { _recordExists = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a group level's sell thru information is inherited.
		/// </summary>
		public bool SellThruLimitIsInherited
		{
			get { return _sellThruLimitIsInherited; }
			set { _sellThruLimitIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the node where the group level's out of stock information is inherited from.
		/// </summary>
		public int SellThruLimitInheritedFromNodeRID
		{
			get { return _sellThruLimitInheritedFromNodeRID; }
			set { _sellThruLimitInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the store group identified for the node.
		/// </summary>
		public float SellThruLimit
		{
			get { return _sellThruLimit; }
			set { _sellThruLimit = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of out of stock information
	/// </summary>
	[Serializable()]
	public class SizeOutOfStockList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeOutOfStockList()
			: base(eProfileType.SizeOutOfStock)
		{
		}
	}

	/// <summary>
	/// Used to retrieve a list of sell thru information
	/// </summary>
	[Serializable()]
	public class SizeSellThruList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeSellThruList()
			: base(eProfileType.SizeSellThru)
		{
		}
	}

	#region SizeOutOfStockLookupProfile
	/// <summary>
	/// Summary description for OutOfStockLookupProfile.
	/// </summary>
	[Serializable()]
	public class SizeOutOfStockLookupProfile : Profile
	{
		SizeOutOfStockProfile _sizeOutOfStockProfile;
		string _nodeId;
		//private int _sizeGroupRid;
		//private int _sizeCurveGroupRid;
		private CollectionOOSLookupSets _collectionSets;
		private DataSet _dsSizeOutOfStockLookup;


		#region Properties
		public string NodeId
		{
			get { return _nodeId; }
			set { _nodeId = value; }
		}
		public int StoreGroupRid
		{
			get { return _sizeOutOfStockProfile.StoreGrpRID; }
			set { _sizeOutOfStockProfile.StoreGrpRID = value; }
		}
		//public int SizeGroupRid
		//{
		//    get { return _sizeGroupRid; }
		//    set { _sizeGroupRid = value; }
		//}
		//public int SizeCurveGroupRid
		//{
		//    get { return _sizeCurveGroupRid; }
		//    set { _sizeCurveGroupRid = value; }
		//}
		public CollectionOOSLookupSets CollectionSets
		{
			get
			{
				if (_collectionSets == null)
				{
					FillCollectionSets();
				}
				return _collectionSets;
			}
		}
		#endregion

		public SizeOutOfStockLookupProfile(SizeOutOfStockProfile sizeOutOfStockProfile)
			:base(sizeOutOfStockProfile.Key)
		{
			_sizeOutOfStockProfile = sizeOutOfStockProfile;
			//if (hnRid == Include.NoRID)
			//{
			//    Initialize();
			//}
			//else
			{	
				//SizeModelData sizeModelData = new SizeModelData();

				//MerchandiseHierarchyData nodeData = new MerchandiseHierarchyData();
				//DataTable dt = nodeData.OutOfStockLookup_Read(hnRid);

				//SizeOutOfStockProfile _sizeOutOfStockProfile =  _sab.HierarchyServerSession.GetSizeOutOfStockProfile(nodeRID, aStrGrpRID, aSzGrpRID, false);

				//if (dt.Rows.Count == 0)
				//{
				//    Initialize();
				//}
				//else
				//{
				//    Fill(dt);
				//}
			}
		}

		private void Initialize()
		{
			//_nodeId = string.Empty;
			//_storeGroupRid = Include.NoRID;
			//_collectionSets = null;

		}

		//private void Fill(DataTable dt)
		//{
		//    DataRow aRow = dt.Rows[0];
		//    _sizeConstraintName = (string)aRow["SIZE_CONSTRAINT_NAME"];
		//    _sizeConstraintName = _sizeConstraintName.Trim();

		//    if (aRow["SG_RID"] == DBNull.Value)
		//        _storeGroupRid = Include.NoRID;
		//    else
		//        _storeGroupRid = Convert.ToInt32(aRow["SG_RID"], CultureInfo.CurrentUICulture);

		//    if (aRow["SIZE_GROUP_RID"] == DBNull.Value)
		//        _sizeGroupRid = Include.NoRID;
		//    else
		//        _sizeGroupRid = Convert.ToInt32(aRow["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);

		//    if (aRow["SIZE_CURVE_GROUP_RID"] == DBNull.Value)
		//        _sizeCurveGroupRid = Include.NoRID;
		//    else
		//        _sizeCurveGroupRid = Convert.ToInt32(aRow["SIZE_CURVE_GROUP_RID"], CultureInfo.CurrentUICulture);
		//    ModelChangeType = eChangeType.update;	//  MID Track #5092 - Serialization error (unrelated)
		//}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeOutOfStockLookup;
			}
		}


		private bool FillCollectionSets()
		{
			bool success = GetSizeOutOfStockData();
			FillCollections();
			
			return success;
		}


		/// <summary>
		/// Fills the _dsSizeOutOfStockLookup DataSet.
		/// </summary>
		/// <returns></returns>
		private bool GetSizeOutOfStockData()
		{
			bool Success = true;

			try
			{
				//_dsSizeOutOfStockLookup = _sizeOutOfStockProfile.dsValues;
				_dsSizeOutOfStockLookup = GetSizeOutOfStockData(_sizeOutOfStockProfile.dsValues);

				//_dsSizeOutOfStockLookup = MIDEnvironment.CreateDataSet();
				//bool getSizes = false;

				//if (SizeGroupRid != Include.NoRID)
				//{
				//    getSizes = true;
				//}
				//if (SizeCurveGroupRid != Include.NoRID)
				//{
				//    getSizes = true;
				//}
				//SizeModelData sizeModelData = new SizeModelData();
				//DataTable dt = sizeModelData.SizeConstraintModel_Read(this.Key);
				//_dsConstraints = sizeModelData.GetChildData(_key, _storeGroupRid, getSizes);			
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
			int colorSizeDimIdx;
			int allColorSizeDimIdx;

			if (_collectionSets != null)
			{
				_collectionSets.Clear();
			}

			_collectionSets = new CollectionOOSLookupSets();
			_collectionSets.StoreGroupRid = _sizeOutOfStockProfile.StoreGrpRID;  // Issue 4244

			if (_dsSizeOutOfStockLookup.Tables["SetLevel"].Rows.Count > 0)
			{
				//FOR EACH SET LEVEL
				foreach (DataRow drSet in _dsSizeOutOfStockLookup.Tables["SetLevel"].Rows)
				{
					setIdx = _collectionSets.Add(new ItemOOSLookupSet(_key, drSet));

					//ALL COLOR
					foreach (DataRow drAllColor in drSet.GetChildRows("SetAllColor"))
					{
						allColorIdx = _collectionSets[setIdx].collectionAllColors.Add(new ItemOOSLookupAllColor(_key, drAllColor));

						//SIZE DIMENSION
						if (_dsSizeOutOfStockLookup.Relations.Contains("AllColorSizeDimension"))
						{
							foreach (DataRow drACSizeDim in drAllColor.GetChildRows("AllColorSizeDimension"))
							{
								allColorSizeDimIdx = _collectionSets[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions.Add(new ItemOOSLookupSizeDimension(_key, drACSizeDim));

								//SIZE
								foreach (DataRow drACSize in drACSizeDim.GetChildRows("AllColorSize"))
								{
									allColorSizeIdx = _collectionSets[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions[allColorSizeDimIdx].collectionSizes.Add(new ItemOOSLookupSize(_key, drACSize));
								}
							}
						}
					}

					//COLOR
					foreach (DataRow drColor in drSet.GetChildRows("SetColor"))
					{
						colorIdx = _collectionSets[setIdx].collectionColors.Add(new ItemOOSLookupColor(_key, drColor));

						//SIZE DIMENSION
						if (_dsSizeOutOfStockLookup.Relations.Contains("ColorSizeDimension"))
						{
							foreach (DataRow drCSizeDim in drColor.GetChildRows("ColorSizeDimension"))
							{
								colorSizeDimIdx = _collectionSets[setIdx].collectionColors[colorIdx].collectionSizeDimensions.Add(new ItemOOSLookupSizeDimension(_key, drCSizeDim));

								//SIZE
								foreach (DataRow drCSize in drCSizeDim.GetChildRows("ColorSize"))
								{
									colorSizeIdx = _collectionSets[setIdx].collectionColors[colorIdx].collectionSizeDimensions[colorSizeDimIdx].collectionSizes.Add(new ItemOOSLookupSize(_key, drCSize));
								}
							}
						}
					}
				}
			}
		}

		virtual public DataSet GetSizeOutOfStockData(DataSet dsSizeOutOfStock)
		{
			_dsSizeOutOfStockLookup = MIDEnvironment.CreateDataSet();
			_dsSizeOutOfStockLookup.EnforceConstraints = true;

			//GROUP LEVEL
			_dsSizeOutOfStockLookup.Tables.Add(SetupGroupLevelTable(dsSizeOutOfStock));

			//ALL COLOR
			_dsSizeOutOfStockLookup.Tables.Add(SetupSizeOutOfStockData(dsSizeOutOfStock, eSizeMethodRowType.AllColor));

			//COLOR
			_dsSizeOutOfStockLookup.Tables.Add(SetupSizeOutOfStockData(dsSizeOutOfStock, eSizeMethodRowType.Color));

			//			if (getSizes)
			//			{
			//ALL COLOR SIZE
			_dsSizeOutOfStockLookup.Tables.Add(SetupSizeOutOfStockData(dsSizeOutOfStock, eSizeMethodRowType.AllColorSize));
			//COLOR SIZE
			_dsSizeOutOfStockLookup.Tables.Add(SetupSizeOutOfStockData(dsSizeOutOfStock, eSizeMethodRowType.ColorSize));

			//COLOR SIZE DIMENSION
			_dsSizeOutOfStockLookup.Tables.Add(SetupSizeOutOfStockData(dsSizeOutOfStock, eSizeMethodRowType.ColorSizeDimension));

			//ALL COLOR SIZE DIMENSION
			_dsSizeOutOfStockLookup.Tables.Add(SetupSizeOutOfStockData(dsSizeOutOfStock, eSizeMethodRowType.AllColorSizeDimension));
			//			}


			//CREATE THE RELATIONSHIPS FOR DATA HIERARCHY
			//BuildChildDataRelationships();
			CreatePrimaryKeys();
			CreateForeignKeys();
			CreateRelationships();

			//BuildChildDataRelationships(getSizes);

			//REMOVE RELATIONS IF SIZE IS NOT AVAILABLE
			//if (!getSizes)
			//{
			//    _ChildDataDS.Relations.Remove("ColorSize");
			//    _ChildDataDS.Relations.Remove("AllColorSize");
			//    _ChildDataDS.Relations.Remove("AllColorSizeDimension");
			//    _ChildDataDS.Relations.Remove("ColorSizeDimension");
			//}


			return _dsSizeOutOfStockLookup;

		}


		/// <summary>
		/// Creates primary keys on the size method constraint data.
		/// </summary>
		virtual public void CreatePrimaryKeys()
		{
			DataTable dt;

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["SetLevel"];
			dt.PrimaryKey = new DataColumn[] { dt.Columns["SGL_RID"] };

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["AllColor"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"]};

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["Color"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"]};

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["AllColorSize"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"],
												 dt.Columns["SIZE_CODE_RID"]};

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["ColorSize"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"],
												 dt.Columns["SIZE_CODE_RID"]};

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["ColorSizeDimension"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"]};

			dt = (DataTable)_dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"];
			dt.PrimaryKey = new DataColumn[] {dt.Columns["SGL_RID"], 
												 dt.Columns["COLOR_CODE_RID"],
												 dt.Columns["DIMENSIONS_RID"]};


		}


		/// <summary>
		/// Creates foreign keys on the size method constraint data.
		/// </summary>
		virtual public void CreateForeignKeys()
		{
			//CREATE CONSTRAINTS
			ForeignKeyConstraint fkc;

			//SETALLCOLOR
			fkc = new ForeignKeyConstraint("SetAllColor",
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["AllColor"].Columns["SGL_RID"] 
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_dsSizeOutOfStockLookup.Tables["AllColor"].Constraints.Add(fkc);

			//SETCOLOR
			fkc = new ForeignKeyConstraint("SetColor",
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["Color"].Columns["SGL_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_dsSizeOutOfStockLookup.Tables["Color"].Constraints.Add(fkc);

			//ALLCOLORSIZE
			fkc = new ForeignKeyConstraint("AllColorSize",
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["DIMENSIONS_RID"]
								 },
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["AllColorSize"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSize"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSize"].Columns["DIMENSIONS_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_dsSizeOutOfStockLookup.Tables["AllColorSize"].Constraints.Add(fkc);

			//COLORSIZE
			fkc = new ForeignKeyConstraint("ColorSize",
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["DIMENSIONS_RID"]
								 },
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["ColorSize"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSize"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSize"].Columns["DIMENSIONS_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_dsSizeOutOfStockLookup.Tables["ColorSize"].Constraints.Add(fkc);

			//COLORSIZEDIMENSION
			fkc = new ForeignKeyConstraint("ColorSizeDimension",
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["Color"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["Color"].Columns["COLOR_CODE_RID"]
								 },
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Constraints.Add(fkc);

			//ALLCOLORSIZEDIMENSION
			fkc = new ForeignKeyConstraint("AllColorSizeDimension",
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["AllColor"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColor"].Columns["COLOR_CODE_RID"]
								 },
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"]
								 });
			fkc.AcceptRejectRule = AcceptRejectRule.None;
			fkc.DeleteRule = Rule.Cascade;
			fkc.UpdateRule = Rule.Cascade;
			_dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Constraints.Add(fkc);
		}


		/// <summary>
		/// Creates relationships on the size method constraint data.
		/// </summary>
		virtual public void CreateRelationships()
		{

			//BUILD RELATIONS
			//SETALLCOLOR
			_dsSizeOutOfStockLookup.Relations.Add(new DataRelation("SetAllColor",
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["AllColor"].Columns["SGL_RID"]
								 }));
			//SETCOLOR
			_dsSizeOutOfStockLookup.Relations.Add(new DataRelation("SetColor",
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["SetLevel"].Columns["SGL_RID"]
								 },
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["Color"].Columns["SGL_RID"]
								 }));

			//ALLCOLORSIZEDIMENSION
			_dsSizeOutOfStockLookup.Relations.Add(new DataRelation("AllColorSizeDimension",
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["AllColor"].Columns["SGL_RID"], 
									 _dsSizeOutOfStockLookup.Tables["AllColor"].Columns["COLOR_CODE_RID"] 
								 },
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["SGL_RID"], 
									 _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"] 
								 }));

			//COLORSIZEDIMENSION
			_dsSizeOutOfStockLookup.Relations.Add(new DataRelation("ColorSizeDimension",
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["Color"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["Color"].Columns["COLOR_CODE_RID"] 
								 },
				new DataColumn[] { _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"] 
								 }));

			//ALLCOLORSIZE
			_dsSizeOutOfStockLookup.Relations.Add(new DataRelation("AllColorSize",
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSizeDimension"].Columns["DIMENSIONS_RID"] 
								 },
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["AllColorSize"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSize"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["AllColorSize"].Columns["DIMENSIONS_RID"] 
								 }));

			//COLORSIZE
			_dsSizeOutOfStockLookup.Relations.Add(new DataRelation("ColorSize",
				new DataColumn[] {	_dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSizeDimension"].Columns["DIMENSIONS_RID"] 
								 },
				new DataColumn[] {  _dsSizeOutOfStockLookup.Tables["ColorSize"].Columns["SGL_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSize"].Columns["COLOR_CODE_RID"],
									 _dsSizeOutOfStockLookup.Tables["ColorSize"].Columns["DIMENSIONS_RID"] 
								 }));
		}


		/// <summary>
		/// Gets a datatable from SIZE_MTH_CONST_MINMAX
		/// </summary>
		/// <param name="method_RID"></param>
		/// <param name="sg_RID"></param>
		/// <param name="RowTypeId"></param>
		/// <returns></returns>
		virtual public DataTable SetupSizeOutOfStockData(DataSet dsSizeOutofStock, eSizeMethodRowType RowTypeId)
		{
			string dtTableName = "";

			switch (RowTypeId)
			{
				case eSizeMethodRowType.AllColor:
					dtTableName = "AllColor";
					break;
				case eSizeMethodRowType.Color:
					dtTableName = "Color";
					break;
				case eSizeMethodRowType.AllColorSize:
					dtTableName = "AllColorSize";
					break;
				case eSizeMethodRowType.AllColorSizeDimension:
					dtTableName = "AllColorSizeDimension";
					break;
				case eSizeMethodRowType.ColorSize:
					dtTableName = "ColorSize";
					break;
				case eSizeMethodRowType.ColorSizeDimension:
					dtTableName = "ColorSizeDimension";
					break;
			}

			try
			{
				//DataColumn column;
				DataTable dtSizeOutOfStockLookup = dsSizeOutofStock.Tables[1].Clone();
				dtSizeOutOfStockLookup.TableName = dtTableName;

				DataRow[] rows = dsSizeOutofStock.Tables[1].Select("ROW_TYPE_ID = " + (int)RowTypeId);

				foreach (DataRow dr in rows)
				{
					dtSizeOutOfStockLookup.ImportRow(dr);
				}
				//MIDDbParameter[] InParams = { new MIDDbParameter("@SIZECONSTRAINTRID",  sizeConstraintRid, eDbType.Int,eParameterDirection.Input),
				//                              new MIDDbParameter("@ROWTYPEID", RowTypeId, eDbType.Int, eParameterDirection.Input),
				//                              new MIDDbParameter("@SGRID", sg_RID, eDbType.Int, eParameterDirection.Input) };

				//dtMinmax = _dba.ExecuteQuery("SP_MID_SIZE_CONST_GET_MINMAX", dtTableName, InParams);

				//column = dtMinmax.Columns["ROW_TYPE_ID"];
				//column.DefaultValue = RowTypeId;

				//switch (RowTypeId)
				//{
				//    case eSizeMethodRowType.AllColor:
				//        column = dtMinmax.Columns["BAND_DSC"];
				//        column.ReadOnly = true;
				//        break;

				//    // BEGIN MID Track #4989 - dimensions & sizes not sequenced; add sequence column
				//    case eSizeMethodRowType.AllColorSize:
				//    case eSizeMethodRowType.AllColorSizeDimension:
				//    case eSizeMethodRowType.ColorSizeDimension:
				//    case eSizeMethodRowType.ColorSize:
				//        DataColumn dataColumn = new DataColumn();
				//        dataColumn.DataType = System.Type.GetType("System.Int32");
				//        dataColumn.ColumnName = "SIZE_SEQ";
				//        dataColumn.DefaultValue = 0;
				//        dtMinmax.Columns.Add(dataColumn);
				//        dtMinmax.DefaultView.Sort = "SIZE_SEQ";
				//        break;
				//}	// END MID Track #4989


				return dtSizeOutOfStockLookup;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds SetLevel table to SizeOutOfStockLookup dataset.
		/// </summary>
		/// <param name="dsOutOfStock"></param>
		/// <returns></returns>
		virtual public DataTable SetupGroupLevelTable(DataSet dsSizeOutOfStock)
		{
			try
			{
				//DataColumn column;
				DataTable dtSetLevel = MIDEnvironment.CreateDataTable();

				dtSetLevel = dsSizeOutOfStock.Tables[0].Copy();
				dtSetLevel.TableName = "SetLevel";

				//MIDDbParameter[] InParams = { new MIDDbParameter("@SIZECONSTRAINTRID",  sizeConstraintRid, eDbType.Int,eParameterDirection.Input),
				//                              new MIDDbParameter("@SGRID", sg_RID,eDbType.Int,eParameterDirection.Input)};

				//dtSetLevel = _dba.ExecuteQuery("SP_MID_SIZE_CONST_GET_GRPLVL", "SetLevel", InParams);



				//column = dtSetLevel.Columns["BAND_DSC"];
				//column.ReadOnly = true;

				dtSetLevel.AcceptChanges();

				return dtSetLevel;
			}
			catch
			{
				throw;
			}

		}
	}
	#endregion
}
