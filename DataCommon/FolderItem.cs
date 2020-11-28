using System;
using System.Data;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	public class FolderItem : ICloneable
	{
		//=======
		// FIELDS
		//=======

		private int _parentRID;
		private int _itemRID;
		private int _userRID;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//private eFolderChildType _itemType;
		private eProfileType _itemType;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private FunctionSecurityProfile _functionSecurityProfile;
		private object _childObject;

		//=============
		// CONSTRUCTORS
		//=============

		public FolderItem(DataRow aRow, FunctionSecurityProfile aFunctionSecurityProfile, object aChildObject)
		{
			try
			{
				LoadFromDataRow(aRow);

				_functionSecurityProfile = aFunctionSecurityProfile;
				_childObject = aChildObject;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public FolderItem(int aParentRID, int aItemRID, int aUserRID, eFolderChildType aItemType, FunctionSecurityProfile aFunctionSecurityProfile, object aChildObject)
		public FolderItem(int aParentRID, int aItemRID, int aUserRID, eProfileType aItemType, FunctionSecurityProfile aFunctionSecurityProfile, object aChildObject)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
			_parentRID = aParentRID;
			_itemRID = aItemRID;
			_userRID = aUserRID;
			_itemType = aItemType;
			_functionSecurityProfile = aFunctionSecurityProfile;
			_childObject = aChildObject;
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public eFolderChildType ItemType
		public eProfileType ItemType
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
			get
			{
				return _itemType;
			}
			set
			{
				_itemType = value;
			}
		}

		public int UserRID
		{
			get
			{
				return _userRID;
			}
			set
			{
				_userRID = value;
			}
		}

		public int ItemRID
		{
			get
			{
				return _itemRID;
			}
			set
			{
				_itemRID = value;
			}
		}

		public int ParentRID
		{
			get
			{
				return _parentRID;
			}
			set
			{
				_parentRID = value;
			}
		}

		public FunctionSecurityProfile FunctionSecurityProfile
		{
			get
			{
				return _functionSecurityProfile;
			}
			set
			{
				_functionSecurityProfile = value;
			}
		}

		public object ChildObject
		{
			get
			{
				return _childObject;
			}
			set
			{
				_childObject = value;
			}
		}

		//========
		// METHODS
		//========

		public object Clone()
		{
			try
			{
				return new FolderItem(_parentRID, _itemRID, _userRID, _itemType, _functionSecurityProfile, _childObject);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadFromDataRow(DataRow aRow) 
		{
			if (aRow["PARENT_FOLDER_RID"] != DBNull.Value)
			{
				_parentRID = Convert.ToInt32(aRow["PARENT_FOLDER_RID"], CultureInfo.CurrentUICulture);
			}
			else
			{
				_parentRID = -1;
			}

			_userRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
			_itemRID = Convert.ToInt32(aRow["CHILD_ITEM_RID"], CultureInfo.CurrentUICulture);
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_itemType = (eFolderChildType)Convert.ToInt32(aRow["CHILD_ITEM_TYPE"], CultureInfo.CurrentUICulture);
			_itemType = (eProfileType)Convert.ToInt32(aRow["CHILD_ITEM_TYPE"], CultureInfo.CurrentUICulture);
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		}
 	}
}