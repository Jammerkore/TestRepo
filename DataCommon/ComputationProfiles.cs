using System;
using System.Collections;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// The ComputationVariableProfile class identifies the BaseVariable profile.
	/// </summary>

	[Serializable]
	abstract public class ComputationVariableProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _variableName;
		private eVariableCategory _variableCategory;
		private eVariableStyle _variableStyle;
		private eVariableAccess _variableAccess;
		private eVariableScope _variableScope;
		private eVariableSpreadType _variableSpreadType;
		private eVariableWeekType _variableWeekType;
		private eVariableTimeTotalType _variableTimeTotalType;
		private eValueFormatType _formatType;
		private int _numDecimals;
		//Begin BonTon Calcs - JScott - Add Display Precision
		private int _numDisplayDecimals;
		//End BonTon Calcs - JScott - Add Display Precision
		private int _displaySequence;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of VariableProfile using the given Id, variable name, database column name, eVariableAccess, eVariableScope, 
		/// eEligiblityType, eFormatType, and number of decimals.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aVariableName">
		/// The display name of this Variable definition.
		/// </param>
		/// <param name="aVariableCategory">
		/// The eVariableCategory that describes the variable category of this Variable.
		/// </param>
		/// <param name="aVariableStyle">
		/// The eVariableStyle that describes the variable style of this Variable.
		/// </param>
		/// <param name="aVariableAccess">
		/// The eVariableAccess that describes the variable access of this Variable.
		/// </param>
		/// <param name="aVariableScope">
		/// The eVariableScope that describes the variable scope of this Variable.
		/// </param>
		/// <param name="aVariableWeekType">
		/// The eVariableWeekType that describes the variable week type of this Variable.
		/// </param>
		/// <param name="aVariableTimeTotalType">
		/// The eVariableTimeTotalType that describes the variable time total type of this Variable.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>

		protected ComputationVariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableSpreadType aVariableSpreadType,
			eVariableWeekType aVariableWeekType,
			eVariableTimeTotalType aVariableTimeTotalType,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals)
			double aNumDecimals)
			//End BonTon Calcs - JScott - Add Display Precision
			: base(aKey)
		{
			_variableName = aVariableName;
			_variableCategory = aVariableCategory;
			_variableStyle = aVariableStyle;
			_variableAccess = aVariableAccess;
			_variableScope = aVariableScope;
			_variableWeekType = aVariableWeekType;
			_variableTimeTotalType = aVariableTimeTotalType;
			_variableSpreadType = aVariableSpreadType;
			_formatType = aFormatType;
			//Begin BonTon Calcs - JScott - Add Display Precision
			//_numDecimals = aNumDecimals;
			SplitNumDecimals(aNumDecimals);
			//End BonTon Calcs - JScott - Add Display Precision
			_displaySequence = 0;
		}

		/// <summary>
		/// Creates a new instance of VariableProfile using the given Id, variable name, database column name, eVariableAccess, eVariableScope, 
		/// eEligiblityType, eFormatType, and number of decimals.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aVariableName">
		/// The display name of this Variable definition.
		/// </param>
		/// <param name="aVariableCategory">
		/// The eVariableCategory that describes the variable category of this Variable.
		/// </param>
		/// <param name="aVariableStyle">
		/// The eVariableStyle that describes the variable style of this Variable.
		/// </param>
		/// <param name="aVariableAccess">
		/// The eVariableAccess that describes the variable access of this Variable.
		/// </param>
		/// <param name="aVariableScope">
		/// The eVariableScope that describes the variable scope of this Variable.
		/// </param>
		/// <param name="aVariableWeekType">
		/// The eVariableWeekType that describes the variable week type of this Variable.
		/// </param>
		/// <param name="aVariableTimeTotalType">
		/// The eVariableTimeTotalType that describes the variable time total type of this Variable.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>

		protected ComputationVariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableSpreadType aVariableSpreadType,
			eVariableWeekType aVariableWeekType,
			eVariableTimeTotalType aVariableTimeTotalType,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals,
			double aNumDecimals,
			//End BonTon Calcs - JScott - Add Display Precision
			int aDisplaySequence)
			: base(aKey)
		{
			_variableName = aVariableName;
			_variableCategory = aVariableCategory;
			_variableStyle = aVariableStyle;
			_variableAccess = aVariableAccess;
			_variableScope = aVariableScope;
			_variableWeekType = aVariableWeekType;
			_variableTimeTotalType = aVariableTimeTotalType;
			_variableSpreadType = aVariableSpreadType;
			_formatType = aFormatType;
			//Begin BonTon Calcs - JScott - Add Display Precision
			//_numDecimals = aNumDecimals;
			SplitNumDecimals(aNumDecimals);
			//End BonTon Calcs - JScott - Add Display Precision
			_displaySequence = aDisplaySequence;
		}

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

		abstract public bool isDatabaseVariable(eVariableCategory aVarCateg, int aVersionRID, eCalendarDateType aCalDateType);

		/// <summary>
		/// Gets the name of this profile.
		/// </summary>

		public string VariableName
		{
			get
			{
				return _variableName;
			}
		}

		/// <summary>
		/// Gets the eVariableCategory for this profile.
		/// </summary>

		public eVariableCategory VariableCategory
		{
			get
			{
				return _variableCategory;
			}
		}

		/// <summary>
		/// Gets the eVariableStyle for this profile.
		/// </summary>

		public eVariableStyle VariableStyle
		{
			get
			{
				return _variableStyle;
			}
		}

		/// <summary>
		/// Gets the eVariableAccess for this profile.
		/// </summary>

		public eVariableAccess VariableAccess
		{
			get
			{
				return _variableAccess;
			}
		}

		/// <summary>
		/// Gets the eVariableScope for this profile.
		/// </summary>

		public eVariableScope VariableScope
		{
			get
			{
				return _variableScope;
			}
		}

		/// <summary>
		/// Gets the eVariableScope for this profile.
		/// </summary>

		public eVariableWeekType VariableWeekType
		{
			get
			{
				return _variableWeekType;
			}
		}

		/// <summary>
		/// Gets the eVariableTimeTotalType for this profile.
		/// </summary>

		public eVariableTimeTotalType VariableTimeTotalType
		{
			get
			{
				return _variableTimeTotalType;
			}
            // Begin TT#3416 -  RMatelic Monthly Change Rules cahnged to "plug" for month spread type
            set
			{
				_variableTimeTotalType = value;
			}
            // End TT#3416
		}

		/// <summary>
		/// Gets the eVariableSpreadType for this profile.
		/// </summary>

		public eVariableSpreadType VariableSpreadType
		{
			get
			{
				return _variableSpreadType;
			}
		}

		/// <summary>
		/// Gets the eValueFormatType for this profile.
		/// </summary>

		public eValueFormatType FormatType
		{
			get
			{
				return _formatType;
			}
		}

		/// <summary>
		/// Gets the int value of number of decimals for this profile.
		/// </summary>

		public int NumDecimals
		{
			get
			{
				return _numDecimals;
			}
		}
		//Begin BonTon Calcs - JScott - Add Display Precision
		/// <summary>
		/// Gets the int value of number of displayed decimals for this profile.
		/// </summary>

		public int NumDisplayDecimals
		{
			get
			{
				return _numDisplayDecimals;
			}
		}

		//End BonTon Calcs - JScott - Add Display Precision
		/// <summary>
		/// Gets the int value of number of decimals for this profile.
		/// </summary>

		public int DisplaySequence
		{
			get
			{
				return _displaySequence;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _variableName;
		}
		//Begin BonTon Calcs - JScott - Add Display Precision

		private void SplitNumDecimals(double aNumDecimals)
		{
			string[] decimals;

			try
			{
				decimals = aNumDecimals.ToString().Split('.');
				_numDecimals = Convert.ToInt32(decimals[0]);

				if (decimals.Length > 1)
				{
					_numDisplayDecimals = Convert.ToInt32(decimals[decimals.Length - 1]);
				}
				else
				{
					_numDisplayDecimals = _numDecimals;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End BonTon Calcs - JScott - Add Display Precision
	}
}
