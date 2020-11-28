using System;
using System.Data;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	public class FolderShortcut : ICloneable
	{
		//=======
		// FIELDS
		//=======

		private int _parentRID;
		private int _shortcutRID;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//private int _userRID;
		//private eFolderType _shortcutType;
		private eProfileType _shortcutType;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

		//=============
		// CONSTRUCTORS
		//=============

		public FolderShortcut()
		{
			_parentRID = -1;
			_shortcutRID = -1;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_userRID = -1;
			//_shortcutType = eFolderType.None;
			_shortcutType = eProfileType.None;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		}

		public FolderShortcut(DataRow aRow)
		{
			try
			{
				LoadFromDataRow(aRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public FolderShortcut(int aParentRID, int aShortcutRID, int aUserRID, eFolderType aShortcutType)
		public FolderShortcut(int aParentRID, int aShortcutRID, eProfileType aShortcutType)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
			_parentRID = aParentRID;
			_shortcutRID = aShortcutRID;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_userRID = aUserRID;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			_shortcutType = aShortcutType;
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public eFolderType ShortcutType
		public eProfileType ShortcutType
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		{
			get
			{
				return _shortcutType;
			}
			set
			{
				_shortcutType = value;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public int UserRID
		//{
		//    get
		//    {
		//        return _userRID;
		//    }
		//    set
		//    {
		//        _userRID = value;
		//    }
		//}

		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		public int ShortcutId
		{
			get
			{
				return _shortcutRID;
			}
			set
			{
				_shortcutRID = value;
			}
		}

		public int ParentFolderId
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

		//========
		// METHODS
		//========

		public object Clone()
		{
			try
			{
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//return new FolderShortcut(_parentRID, _shortcutRID, _userRID, _shortcutType);
				return new FolderShortcut(_parentRID, _shortcutRID, _shortcutType);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
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

			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//_userRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
			//_shortcutRID = Convert.ToInt32(aRow["CHILD_SHORTCUT_RID"], CultureInfo.CurrentUICulture);
			//_shortcutType = (eFolderType)Convert.ToInt32(aRow["CHILD_SHORTCUT_TYPE"], CultureInfo.CurrentUICulture);
			_shortcutRID = Convert.ToInt32(aRow["CHILD_SHORTCUT_RID"], CultureInfo.CurrentUICulture);
			_shortcutType = (eProfileType)Convert.ToInt32(aRow["CHILD_SHORTCUT_TYPE"], CultureInfo.CurrentUICulture);
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		}
	}
}
