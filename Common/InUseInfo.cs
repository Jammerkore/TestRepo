using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for InUseInfo.
	/// </summary>
	[Serializable]
	public class InUseInfo
	{
		//=======
		// FIELDS
		//=======

		private ArrayList _captions;
		private ArrayList _items;
		private DataTable _dtInUse;
        private eProfileType _itemProfileType;  // Begin TT#110 - RMatelic -In Use Tool
        private int _itemRid;                   // TT#110-MD-VStuart - In Use Tool
        private ArrayList _itemRids;             // TT#110-MD-VStuart - In Use Tool
        private string _itemName;               // End TT#110
        private string _itemTitle;              // TT#110-MD-VStuart - In Use Tool
        private bool _itemDisplay;
		//=============
		// CONSTRUCTORS
		//=============

		public InUseInfo(string itemCaptions)
		{
			try
			{
				_dtInUse = MIDEnvironment.CreateDataTable("inUse");
				_items = new ArrayList();

				string[] tempList;
				_captions = new ArrayList();

				if (itemCaptions != null)
				{
					tempList = itemCaptions.Split(new char[] {','});

					foreach (string aCaption in tempList)
					{
						if (aCaption.Trim().Length > 0)
						{
							_captions.Add(aCaption);
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

        public InUseInfo(int itemRid, eProfileType itemType)
        {
            try
            {
                _itemRid = itemRid;
                _itemProfileType = itemType;
            }
            catch
            {
                throw;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
         public InUseInfo(ArrayList itemRids, eProfileType itemType, string itemTitle)
        {
            try
            {
                _itemRids = itemRids;
                _itemProfileType = itemType;
                _itemTitle = itemTitle;
            }
            catch
            {
                throw;
            }
        }
        //END TT#110-MD-VStuart - In Use Tool

        //===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the Id of the Profile.
		/// </summary>

		public DataTable InUseTable
		{
			get
			{
				if (_items.Count != _dtInUse.Rows.Count)
				{
					return BuildInUseTable();
				}
				else
				{
					return _dtInUse;
				}
			}
		}

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Gets or sets the Item Name or title of the item being checked.
        /// </summary>
        public string ItemTitle
        {
            get { return _itemTitle; }
            set { _itemTitle = value; }
        }
        //END TT#110-MD-VStuart - In Use Tool

        // Begin TT#110 - RMatelic -In Use Tool
        /// <summary>
        /// Gets or sets the RID of the item being checked.
        /// </summary>
        public int ItemRID
        {
            get { return _itemRid; }
            set { _itemRid = value; }
        }

        /// <summary>
        /// Gets or sets the array of RIDs of the items being checked.
        /// </summary>
        public ArrayList ItemRIDs
        {
            get { return _itemRids; }
            set { _itemRids = value; }
        }

        /// <summary>
        /// Gets or sets the eProfileType of the item being checked.
        /// </summary>
        public eProfileType ItemProfileType
        {
            get { return _itemProfileType; }
            set { _itemProfileType = value; }
        }

        /// <summary>
        /// Gets or sets the Name of the item being checked.
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }
        // End TT#110  

		//========
		// METHODS
		//========

        public void AddItem(ArrayList values)
		{
			bool match = false;
			foreach (ArrayList aItem in _items)
			{
				if (values.Count == aItem.Count)
				{
					bool check = true;
					for (int i=0;i<aItem.Count;i++)
					{
						if ((string)aItem[i] != (string)values[i])
							check = false;
					}

					if (check)
					{
						match = true;
						break;
					}
				}
				if (match)
					break;
			}

			if (!match)
				_items.Add(values);
		}

		public void AddItem(string[] values)
		{
			ArrayList valueList = new ArrayList();
			foreach (string aValue in values)
			{
				valueList.Add(aValue);
			}
			AddItem(valueList);
		}

		//================================================
		// Convenience methods for comma delimited items
		//================================================
		public void AddItem(string aItem)
		{
			ArrayList valueList = new ArrayList();
			valueList.Add(aItem);
			AddItem(valueList);
		}
		public void AddItem(string aItem1, string aItem2)
		{
			ArrayList valueList = new ArrayList();
			valueList.Add(aItem1);
			valueList.Add(aItem2);
			AddItem(valueList);
		}
		public void AddItem(string aItem1, string aItem2, string aItem3)
		{
			ArrayList valueList = new ArrayList();
			valueList.Add(aItem1);
			valueList.Add(aItem2);
			valueList.Add(aItem3);
			AddItem(valueList);
		}

		private DataTable BuildInUseTable()
		{
			int maxColumns = MaxColumns();

			//================
			// Create Columns
			//================
			for (int i=0;i<maxColumns;i++)
			{
				DataColumn newColumn = new DataColumn();
				if (i<_captions.Count)
				{
					newColumn.Caption = _captions[i].ToString(); 
					newColumn.ColumnName = _captions[i].ToString(); 
				}
				else
				{
					newColumn.Caption = "Unknown" + i.ToString(CultureInfo.CurrentUICulture); 
					newColumn.ColumnName = "Unknown" + i.ToString(CultureInfo.CurrentUICulture); 
				}
				newColumn.AllowDBNull = true; 
				newColumn.ReadOnly = true;
				newColumn.DataType = System.Type.GetType("System.String"); 	
				_dtInUse.Columns.Add(newColumn); 
			}

			//=============
			// Fill rows
			//=============
			foreach (ArrayList aItem in _items)
			{
				DataRow newRow = _dtInUse.NewRow();
				for (int i=0;i<aItem.Count;i++)
				{
					newRow[i] = aItem[i].ToString();
				}
				_dtInUse.Rows.Add(newRow);
			}

			return _dtInUse;
		}

		private int MaxColumns()
		{
			int maxCount = 0;

			foreach (ArrayList aItem in _items)
			{
				if (aItem.Count > maxCount)
					maxCount = aItem.Count;
			}

			if (_captions.Count > maxCount)
				maxCount = _captions.Count;

			return maxCount;		
		}

		
	}
}
