using System;
using System.Collections.Generic;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Class that defines the Component Variables for Assortment.
	/// </summary>

	public class AssortmentComponentVariableProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _profListType;
		private string _varName;
		private string _colName;
		private bool _mainComponent;
		private bool _headerComponent;
		private bool _useAlternate;
		// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		private bool _hideComponent;
		// END TT#490-MD - stodd -  post-receipts should not show placeholders

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentComponentVariableProfile(int aKey, eProfileType aProfListType, string aVariableName, string aColumnName, bool aMainComponent, bool aHeaderComponent)
			: base(aKey)
		{
			_profListType = aProfListType;
			_varName = aVariableName;
			_colName = aColumnName;
			_mainComponent = aMainComponent;
			_headerComponent = aHeaderComponent;
			_useAlternate = false;
			// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
			_hideComponent = false;
			// END TT#490-MD - stodd -  post-receipts should not show placeholders
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentComponentVariable;
			}
		}

		public eProfileType ProfileListType
		{
			get
			{
				return _profListType;
			}
		}

		public string VariableName
		{
			get
			{
				return _varName;
			}
		}

		public bool MainComponent
		{
			get
			{
				return _mainComponent;
			}
		}

		public bool HeaderComponent
		{
			get
			{
				return _headerComponent;
			}
		}

		public string TextColumnName
		{
			get
			{
				return _colName;
			}
		}

		public string AlternateTextColumnName
		{
			get
			{
				return _colName + "_ALTERNATE";
			}
		}

		public string DisplayTextColumnName
		{
			get
			{
				if (_useAlternate)
				{
					return AlternateTextColumnName;
				}
				else
				{
					return TextColumnName;
				}
			}
		}

		public string RIDColumnName
		{
			get
			{
				return _colName + "_RID";
			}
		}

		public bool UseAlternateTextColumnName
		{
			get
			{
				return _useAlternate;
			}
			set
			{
				_useAlternate = value;
			}
		}

		// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
		public bool HideComponent
		{
			get
			{
				return _hideComponent;
			}
			set
			{
				_hideComponent = value;
			}
		}
		// END TT#490-MD - stodd -  post-receipts should not show placeholders

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the Assortment Variables for Assortment.
	/// </summary>

	abstract public class AssortmentVariableProfile : ComputationVariableProfile
	{
		//=======
		// FIELDS
		//=======

		private string _databaseColumnName;

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database 
		/// for the given type of data.
		/// </summary>
		/// <param name="aVarCateg">
		/// The eVariableCategory setting that identifies the type of data.
		/// </param>
		/// <param name="aVersionRID">
		/// The key of the version for the type of data.
		/// </param>
		/// <param name="aCalDateType">
		/// The eCalendarDateType for the type of data.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is eligible.
		/// </returns>

		override public bool isDatabaseVariable(eVariableCategory aVarCateg, int aVersionRID, eCalendarDateType aCalDateType)
		{
			if (_databaseColumnName != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public string DatabaseColumnName
		{
			get
			{
				return _databaseColumnName;
			}
		}

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentVariableProfile(int aKey, string aVariableName, string aDatabaseColumnName, eVariableStyle aStyle, eVariableAccess aAccess, eVariableScope aScope, eVariableSpreadType aSpreadType, eValueFormatType aFormatType, int aNumDecimals)
			: base(aKey, aVariableName, eVariableCategory.None, aStyle, aAccess, aScope, aSpreadType, eVariableWeekType.None, eVariableTimeTotalType.None, aFormatType, aNumDecimals)
		{
			_databaseColumnName = aDatabaseColumnName;
		}
	}

	/// <summary>
	/// Class that defines the Summary Variables for Assortment.
	/// </summary>

	public class AssortmentSummaryVariableProfile : AssortmentVariableProfile
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentSummaryVariableProfile(int aKey, string aVariableName, string aDatabaseColumnName, eVariableStyle aStyle, eVariableAccess aAccess, eVariableScope aScope, eVariableSpreadType aSpreadType, eValueFormatType aFormatType, int aNumDecimals)
			: base(aKey, aVariableName, aDatabaseColumnName, aStyle, aAccess, aScope, aSpreadType, aFormatType, aNumDecimals)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentSummaryVariable;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the Total Variables for Assortment.
	/// </summary>

	public class AssortmentTotalVariableProfile : AssortmentVariableProfile
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentTotalVariableProfile(int aKey, string aVariableName, string aDatabaseColumnName, eVariableStyle aStyle, eVariableAccess aAccess, eVariableScope aScope, eVariableSpreadType aSpreadType, eValueFormatType aFormatType, int aNumDecimals)
			: base(aKey, aVariableName, aDatabaseColumnName, aStyle, aAccess, aScope, aSpreadType, aFormatType, aNumDecimals)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentTotalVariable;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Class that defines the Detail Variables for Assortment.
	/// </summary>

	public class AssortmentDetailVariableProfile : AssortmentVariableProfile
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentDetailVariableProfile(int aKey, string aVariableName, string aDatabaseColumnName, eVariableStyle aStyle, eVariableAccess aAccess, eVariableScope aScope, eVariableSpreadType aSpreadType, eValueFormatType aFormatType, int aNumDecimals)
			: base(aKey, aVariableName, aDatabaseColumnName, aStyle, aAccess, aScope, aSpreadType, aFormatType, aNumDecimals)
		{
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentDetailVariable;
			}
		}

		//========
		// METHODS
		//========
	}
}
