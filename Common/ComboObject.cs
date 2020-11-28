using System;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
using MIDRetail.DataCommon;
//End Track #3863 - JScott - OTS Forecast Level Defaults

namespace MIDRetail.Common
{
	/// <summary>
	/// Class that defines the basic object stored in a ComboBox list.
	/// </summary>

	public class ComboObject
	{
		private int _key;
		private string _value;

		public ComboObject(int aKey, string aValue)
		{
			_key = aKey;
			_value = aValue;
		}

		override public string ToString()
		{
			return _value;
		}

		public override bool Equals(object obj)
		{
			return (((ComboObject)obj)._key == _key && ((ComboObject)obj)._value == _value);
		}

		public override int GetHashCode() 
		{
			try
			{
				return base.GetHashCode();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int Key
		{
			get
			{
				return _key;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
		}
	}

	/// <summary>
	/// Class that defines the numeric object stored in a ComboBox list.
	/// </summary>

	public class NumericComboObject
	{
		private int _key;
		private int _value;

		public NumericComboObject(int aKey, int aValue)
		{
			_key = aKey;
			_value = aValue;
		}

		override public string ToString()
		{
			return Convert.ToString(_value);
		}

		public override bool Equals(object obj)
		{
			return (((NumericComboObject)obj)._key == _key && ((NumericComboObject)obj)._value == _value);
		}

		public override int GetHashCode() 
		{
			try
			{
				return base.GetHashCode();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int Key
		{
			get
			{
				return _key;
			}
		}

		public int Value
		{
			get
			{
				return _value;
			}
		}

		public string ValueAsString
		{
			get
			{
				return Convert.ToString(_value);
			}
		}
	}
//Begin Track #3863 - JScott - OTS Forecast Level Defaults

	public class HierarchyLevelComboObject
	{
		//=======
		// FIELDS
		//=======

		private int _levelIndex;
		private ePlanLevelLevelType _planLevelLevelType;
		private int _hierarchyRID;
		private int _level;
		private string _levelName;
		private string _displayName;

		//===========
		// PROPERTIES
		//===========

		public int LevelIndex
		{
			get
			{
				return _levelIndex;
			}
		}

		public ePlanLevelLevelType PlanLevelLevelType
		{
			get
			{
				return _planLevelLevelType;
			}
		}

		public int HierarchyRID
		{
			get
			{
				return _hierarchyRID;
			}
			set
			{
				_hierarchyRID = value;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		public string LevelName
		{
			get
			{
				return _levelName;
			}
			set
			{
				_levelName = value;
			}
		}

		public string DisplayName
		{
			get
			{
				return _displayName;
			}
		}

		//=============
		// CONSTRUCTORS
		//=============

		public HierarchyLevelComboObject(int aLevelIndex, ePlanLevelLevelType aPlanLevelLevelType, int aHierarchyRID, int aLevel, string aLevelName)
		{
			CommonLoad( aLevelIndex, aPlanLevelLevelType, aHierarchyRID, aLevel, aLevelName, null);
		}

		public HierarchyLevelComboObject(int aLevelIndex, ePlanLevelLevelType aPlanLevelLevelType, int aHierarchyRID, int aLevel, string aLevelName,
			string aAltDisplayName)
		{
			CommonLoad( aLevelIndex, aPlanLevelLevelType, aHierarchyRID, aLevel, aLevelName, aAltDisplayName);
		}

		private void CommonLoad(int aLevelIndex, ePlanLevelLevelType aPlanLevelLevelType, int aHierarchyRID, int aLevel, string aLevelName,
			string aAltDisplayName)
		{
			_levelIndex = aLevelIndex;
			_planLevelLevelType = aPlanLevelLevelType;
			HierarchyRID = aHierarchyRID;
			Level = aLevel;
			LevelName = aLevelName;
			if (aAltDisplayName != null)
			{
				_displayName = aAltDisplayName;
			}
			else if (PlanLevelLevelType == ePlanLevelLevelType.LevelOffset)
			{
				_displayName = "+" + _level.ToString();
			}
			else
			{
				_displayName = _levelName;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _displayName;
		}
	}
//End Track #3863 - JScott - OTS Forecast Level Defaults

	public class HierarchyComboObject
	{
		//=======
		// FIELDS
		//=======

		private eHierarchyType _hierarchyType;
		private int _hierarchyRID;
		private string _hierarchyName;

		//===========
		// PROPERTIES
		//===========

		public eHierarchyType HierarchyType
		{
			get
			{
				return _hierarchyType;
			}
		}

		public int HierarchyRID
		{
			get
			{
				return _hierarchyRID;
			}
			set
			{
				_hierarchyRID = value;
			}
		}

		public string HierarchyName
		{
			get
			{
				return _hierarchyName;
			}
			set
			{
				_hierarchyName = value;
			}
		}

		//=============
		// CONSTRUCTORS
		//=============

		public HierarchyComboObject(eHierarchyType aHierarchyType, int aHierarchyRID, string aHierarchyName)
		{
			_hierarchyType = aHierarchyType;
			HierarchyRID = aHierarchyRID;
			_hierarchyName = aHierarchyName;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _hierarchyName;
		}
	}
}
