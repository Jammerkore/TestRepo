using System;
using System.Collections;
using System.Globalization;
namespace MIDRetail.DataCommon
{
	/// <summary>
	/// This class contains the values of the QuantityLevel flag bits.
	/// </summary>

	public class QuantityLevelFlagValues
	{
		//=======
		// FIELDS
		//=======

		// Level Flags

		public static readonly ushort HighLevel = 0x0001;
		public static readonly ushort LowLevel = 0x0002;
		public static readonly ushort LowLevelTotal = 0x0004;

		// Cube Flags

		public static readonly ushort StoreDetailCube = 0x0008;
		public static readonly ushort StoreSetCube = 0x0010;
        public static readonly ushort StoreTotalCube = 0x0020;
		public static readonly ushort ChainDetailCube = 0x0040;

		// Accumulated Flags

		public static readonly ushort None = 0x0000;
		public static readonly ushort All = 0xFFFF;
		public static readonly ushort AllLevels = 0x0007;

		// View Flags

		public static readonly ushort ChainSingleView = 0x0001;
		public static readonly ushort ChainMultiView = 0x0002;
		public static readonly ushort StoreSingleView = 0x0004;
		public static readonly ushort StoreMultiView = 0x0008;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}
		
	/// <summary>
	/// The QuantityLevelFlags class describes the QuantityLevel flag bits container.
	/// </summary>

	[Serializable]
	public struct QuantityLevelFlags
	{
		//=======
		// FIELDS
		//=======

		ushort _cubeFlags;
		ushort _viewFlags;

		//=============
		// CONSTRUCTORS
		//=============

		public QuantityLevelFlags(ushort aCubeFlags, ushort aViewFlags)
		{
			_cubeFlags = aCubeFlags;
			_viewFlags = aViewFlags;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the HighLevel flag.
		/// </summary>

		public bool isHighLevel
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.HighLevel, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.HighLevel);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.HighLevel);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the LowLevel flag.
		/// </summary>

		public bool isLowLevel
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.LowLevel, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.LowLevel);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.LowLevel);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the LowLevelTotal flag.
		/// </summary>

		public bool isLowLevelTotal
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.LowLevelTotal, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.LowLevelTotal);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.LowLevelTotal);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Store flag.
		/// </summary>

		public bool isStoreDetailCube
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.StoreDetailCube, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.StoreDetailCube);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.StoreDetailCube);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StoreSet flag.
		/// </summary>

		public bool isStoreSetCube
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.StoreSetCube, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.StoreSetCube);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.StoreSetCube);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StoreTotal flag.
		/// </summary>

		public bool isStoreTotalCube
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.StoreTotalCube, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.StoreTotalCube);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.StoreTotalCube);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the Chain flag.
		/// </summary>

		public bool isChainDetailCube
		{
			get
			{
				return Convert.ToBoolean(_cubeFlags & QuantityLevelFlagValues.ChainDetailCube, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_cubeFlags = (ushort)(_cubeFlags | QuantityLevelFlagValues.ChainDetailCube);
				}
				else
				{
					_cubeFlags = (ushort)(_cubeFlags & ~QuantityLevelFlagValues.ChainDetailCube);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the ChainSingleView flag.
		/// </summary>

		public bool isChainSingleView
		{
			get
			{
				return Convert.ToBoolean(_viewFlags & QuantityLevelFlagValues.ChainSingleView, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_viewFlags = (ushort)(_viewFlags | QuantityLevelFlagValues.ChainSingleView);
				}
				else
				{
					_viewFlags = (ushort)(_viewFlags & ~QuantityLevelFlagValues.ChainSingleView);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the ChainMultiView flag.
		/// </summary>

		public bool isChainMultiView
		{
			get
			{
				return Convert.ToBoolean(_viewFlags & QuantityLevelFlagValues.ChainMultiView, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_viewFlags = (ushort)(_viewFlags | QuantityLevelFlagValues.ChainMultiView);
				}
				else
				{
					_viewFlags = (ushort)(_viewFlags & ~QuantityLevelFlagValues.ChainMultiView);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StoreSingleView flag.
		/// </summary>

		public bool isStoreSingleView
		{
			get
			{
				return Convert.ToBoolean(_viewFlags & QuantityLevelFlagValues.StoreSingleView, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_viewFlags = (ushort)(_viewFlags | QuantityLevelFlagValues.StoreSingleView);
				}
				else
				{
					_viewFlags = (ushort)(_viewFlags & ~QuantityLevelFlagValues.StoreSingleView);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the StoreMultiView flag.
		/// </summary>

		public bool isStoreMultiView
		{
			get
			{
				return Convert.ToBoolean(_viewFlags & QuantityLevelFlagValues.StoreMultiView, CultureInfo.CurrentUICulture);
			}
			set
			{
				if (value)
				{
					_viewFlags = (ushort)(_viewFlags | QuantityLevelFlagValues.StoreMultiView);
				}
				else
				{
					_viewFlags = (ushort)(_viewFlags & ~QuantityLevelFlagValues.StoreMultiView);
				}
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Clears all flags.
		/// </summary>

		public void Clear()
		{
			_cubeFlags = QuantityLevelFlagValues.None;
			_viewFlags = QuantityLevelFlagValues.None;
		}

		/// <summary>
		/// Returns a boolean indicating if this instance of QuantityLevelFlags is valid for the given cube flag values.
		/// </summary>
		/// <param name="aCubeFlags">
		/// The cube flag values to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if this instance of QuantityLevelFlags is valid.
		/// </returns>

		public bool IsValidCube(ushort aCubeFlags)
		{
			try
			{
				return ((_cubeFlags & aCubeFlags) == aCubeFlags);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The VersionProfile class identifies the Version profile.
	/// </summary>

	[Serializable]
	public class VersionProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _description;
		VersionSecurityProfile _chainSecurity;
		VersionSecurityProfile _storeSecurity;
		private bool _protectHistory;
		eForecastBlendType _blendType;
		int _actualVersionRID;
		int _forecastVersionRID;
		bool _blendCurrentByMonth;
		bool _allowSimilarStore;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of VersionProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public VersionProfile(int aKey)
			: base(aKey)
		{
			_description = "";
			_blendType = eForecastBlendType.None;
			_actualVersionRID = Include.NoRID;
			_forecastVersionRID = Include.NoRID;
			_blendCurrentByMonth = false;
			_allowSimilarStore = true;
		}

		/// <summary>
		/// Creates a new instance of VersionProfile using the given Id and description.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aDescription">
		/// The text description for this profile.
		/// </param>

		public VersionProfile(int aKey, string aDescription, bool aProtectHistory)
			: base(aKey)
		{
			_description = aDescription;
			_protectHistory = aProtectHistory;
			_blendType = eForecastBlendType.None;
			_actualVersionRID = Include.NoRID;
			_forecastVersionRID = Include.NoRID;
			_blendCurrentByMonth = false;
			_allowSimilarStore = true;
		}

		/// <summary>
		/// Creates a new instance of VersionProfile using the given Id and description.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>
		/// <param name="aDescription">
		/// The text description for this profile.
		/// </param>
		/// <param name="aBlendType">
		/// The forecast version blend type for this profile.
		/// </param>
		/// <param name="aActualVersionRID">
		/// The forecast version actual version for this profile.
		/// </param>
		/// <param name="aForecastVersionRID">
		/// The forecast version forecast version for this profile.
		/// </param>
		/// <param name="aBlendCurrentByMonth">
		/// A flag identifying if the forecast is to be blended by month rather than by week for this profile.
		/// </param>

		public VersionProfile(int aKey, string aDescription, bool aProtectHistory, eForecastBlendType aBlendType,
			int aActualVersionRID, int aForecastVersionRID, bool aBlendCurrentByMonth, bool aAllowSimilarStore)
			: base(aKey)
		{
			_description = aDescription;
			_protectHistory = aProtectHistory;
			_blendType = aBlendType;
			_actualVersionRID = aActualVersionRID;
			_forecastVersionRID = aForecastVersionRID;
			_blendCurrentByMonth = aBlendCurrentByMonth;
			_allowSimilarStore = aAllowSimilarStore;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Version;
			}
		}

		/// <summary>
		/// Returns the description of this profile.
		/// </summary>

		public string Description
		{
			get
			{
				return _description;
			}
		}

		/// <summary>
		/// Returns the user's chain security profile.
		/// </summary>

		public VersionSecurityProfile ChainSecurity
		{
			get
			{
				return _chainSecurity;
			}
			set
			{
				_chainSecurity = value;
			}
		}

		/// <summary>
		/// Returns the user's store security profile.
		/// </summary>

		public VersionSecurityProfile StoreSecurity
		{
			get
			{
				return _storeSecurity;
			}
			set
			{
				_storeSecurity = value;
			}
		}

		/// <summary>
		/// Gets or sets the protect history setting of this profile.
		/// </summary>

		public bool ProtectHistory
		{
			get
			{
				return _protectHistory;
			}
			set
			{
				_protectHistory = value;
			}
		}

		public bool IsBlendedVersion
		{
			get
			{
				if (_blendType == eForecastBlendType.None)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		public eForecastBlendType BlendType
		{
			get
			{
				return _blendType;
			}
			set
			{
				_blendType = value;
			}
		}

		public int ActualVersionRID
		{
			get
			{
				return _actualVersionRID;
			}
			set
			{
				_actualVersionRID = value;
			}
		}

		public int ForecastVersionRID
		{
			get
			{
				return _forecastVersionRID;
			}
			set
			{
				_forecastVersionRID = value;
			}
		}

		public bool BlendCurrentByMonth
		{
			get
			{
				return _blendCurrentByMonth;
			}
			set
			{
				_blendCurrentByMonth = value;
			}
		}

		public bool AllowSimilarStore
		{
			get
			{
				return _allowSimilarStore;
			}
			set
			{
				_allowSimilarStore = value;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Used to retrieve and update a list of version profiles
	/// </summary>
	[Serializable()]
	public class VersionProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public VersionProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// The VariableProfile class identifies the Variable profile.
	/// </summary>

	[Serializable]
	public class VariableProfile : ComputationVariableProfile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		private System.Collections.ArrayList _timeTotalVariables;
		private System.Collections.ArrayList _timeTotalChainVariables;
		private System.Collections.ArrayList _timeTotalStoreVariables;
		private ProfileXRef _timeTotalXRef;
		private eVariableType _variableType;
		private eClientCustomVariableType _customVariableType;
		private string _databaseColumnName;
		private int _databaseColumnPosition;
		private eVariableDatabaseType _chainDatabaseVariableType;
		private eVariableDatabaseType _storeDatabaseVariableType;
		private eEligibilityType _eligibilityType;
		private eSimilarStoreDateType _similarStoreDateType;
		private eVariableForecastType _variableForecastType;
		private VariableProfile _associatedForecastVariable;
		private ProfileList _actualWTDSalesVariableList;
		private TimeTotalVariableProfile _totalTimeTotalVarProf;
		private TimeTotalVariableProfile _avgTimeTotalVarProf;
		private bool _allowForecastBalance;
		private bool _allowOTSForecast;
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		private bool _allowAssortmentPlanning;
		//End TT#2 - JScott - Assortment Planning - Phase 2
		private eLevelRollType _levelRollType;
		private eDayToWeekRollType _dayToWeekRollType;
		private eStoreToChainRollType _storeToChainRollType;
		private int _chainHistoryRollLevel;
		private int _storeDailyHistoryRollLevel;
		private int _storeWeeklyHistoryRollLevel;
		private int _chainForecastRollLevel;
		private int _storeForecastRollLevel;
		private eVariableDatabaseModelType _chainHistoryModelType;
		private eVariableDatabaseModelType _chainForecastModelType;
		private eVariableDatabaseModelType _storeDailyHistoryModelType;
		private eVariableDatabaseModelType _storeWeeklyHistoryModelType;
		private eVariableDatabaseModelType _storeForecastModelType;
// Begin Track #4868 - JSmith - Variable Groupings
        private string _groupings;
// End Track #4868

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
		/// <param name="aVariableType">
		/// The database table where the values are found.
		/// </param>
		/// <param name="aDatabaseColumnName">
		/// The database column name of this profile.
		/// </param>
		/// <param name="aChainDatabaseVariableType">
		/// The type of chain database variable of this profile.
		/// </param>
		/// <param name="aStoreDatabaseVariableType">
		/// The type of store database variable of this profile.
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
		/// <param name="aEligibilityType">
		/// The eEligibilityType that describes which eligiblity type this Variable follows.
		/// </param>
		/// <param name="aSimilarStoreDateType">
		/// The eSimilarStoreDateType that describes which similar store type this Variable follows.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>
		/// <param name="aAllowForecastBalance">
		/// A flag that identifies if forecast balance, matrix, is allowed for this variable.
		/// </param>
		/// <param name="aAllowOTSForecast">
		/// A flag that identifies if the OTS forecast process is allowed for this variable.
		/// </param>
		/// <param name="aAllowAssortmentPlanning">
		/// A flag that identifies if Assortment Planning is allowed for this variable.
		/// </param>
		/// <param name="aLevelRollType">
		/// The level rollup type for this variable
		/// </param>
		/// <param name="aDayToWeekRollType">
		/// The day to week rollup type for this variable
		/// </param>
		/// <param name="aStoreToChainRollType">
		/// The store to chain rollup type for this variable
		/// </param>
		/// <param name="aChainForecastRollLevel">
		/// The level at which chain forcast values should be rolled
		/// </param>
		/// <param name="aChainHistoryRollLevel">
		/// The level at which chain history values should be rolled
		/// </param>
		/// <param name="aStoreForecastRollLevel">
		/// The level at which store forcast values should be rolled
		/// </param>
		/// <param name="aStoreDailyHistoryRollLevel">
		/// The level at which store daily history values should be rolled
		/// </param>
		/// <param name="aStoreWeeklyHistoryRollLevel">
		/// The level at which store weekly history values should be rolled
		/// </param>
		/// <param name="aChainHistoryModelType">
		/// The model type to be used for chain history values
		/// </param>
		/// <param name="aChainForecastModelType">
		/// The model type to be used for chain forecast values
		/// </param>
		/// <param name="aStoreWeeklyHistoryModelType">
		/// The model type to be used for store weekly history values
		/// </param>
		/// <param name="aStoreDailyHistoryModelType">
		/// The model type to be used for store daily history values
		/// </param>
		/// <param name="aStoreForecastModelType">
		/// The model type to be used for store forecast values
		/// </param>
        /// <param name="aGroupings">
        /// The levels of organization used to display the variable in the chooser
        /// </param>
		
		public VariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableType aVariableType,
			string aDatabaseColumnName,
			eVariableDatabaseType aChainDatabaseVariableType,
			eVariableDatabaseType aStoreDatabaseVariableType,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableSpreadType aVariableSpreadType,
			eVariableWeekType aVariableWeekType,
			eEligibilityType aEligibilityType,
			eSimilarStoreDateType aSimilarStoreDateType,
			eVariableTimeTotalType aVariableTimeTotalType,
			eVariableForecastType aVariableForecastType,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals,
			double aNumDecimals,
			//End BonTon Calcs - JScott - Add Display Precision
			bool aAllowForecastBalance,
			bool aAllowOTSForecast,
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			bool aAllowAssortmentPlanning,
			//End TT#2 - JScott - Assortment Planning - Phase 2
			eLevelRollType aLevelRollType,
			eDayToWeekRollType aDayToWeekRollType,
			eStoreToChainRollType aStoreToChainRollType,
			int aChainHistoryRollLevel,
			int aChainForecastRollLevel,
			int aStoreDailyHistoryRollLevel,
			int aStoreWeeklyHistoryRollLevel,
			int aStoreForecastRollLevel,
			eVariableDatabaseModelType aChainHistoryModelType,
			eVariableDatabaseModelType aChainForecastModelType,
			eVariableDatabaseModelType aStoreDailyHistoryModelType,
			eVariableDatabaseModelType aStoreWeeklyHistoryModelType,
// Begin Track #4868 - JSmith - Variable Groupings
			//eVariableDatabaseModelType aStoreForecastModelType)
			eVariableDatabaseModelType aStoreForecastModelType,
            string aGroupings)
// End Track #4868
			: base(aKey, aVariableName, aVariableCategory, aVariableStyle, aVariableAccess, aVariableScope, aVariableSpreadType, aVariableWeekType, aVariableTimeTotalType, aFormatType, aNumDecimals)
		{
			_timeTotalVariables = new System.Collections.ArrayList();
			_timeTotalChainVariables = new System.Collections.ArrayList();
			_timeTotalStoreVariables = new System.Collections.ArrayList();
			_timeTotalXRef = new ProfileXRef(eProfileType.Variable, eProfileType.TimeTotalVariable);
			_variableType = aVariableType;
			_customVariableType = eClientCustomVariableType.None;
			_databaseColumnName = aDatabaseColumnName;
			_databaseColumnPosition = Include.NoColumnPosition;
			_chainDatabaseVariableType = aChainDatabaseVariableType;
			_storeDatabaseVariableType = aStoreDatabaseVariableType;
			_eligibilityType = aEligibilityType;
			_similarStoreDateType = aSimilarStoreDateType;
			_variableForecastType = aVariableForecastType;
			_associatedForecastVariable = null;
			_totalTimeTotalVarProf = null;
			_avgTimeTotalVarProf = null;
			_allowForecastBalance = aAllowForecastBalance;
			_allowOTSForecast = aAllowOTSForecast;
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			_allowAssortmentPlanning = aAllowAssortmentPlanning;
			//End TT#2 - JScott - Assortment Planning - Phase 2
			_levelRollType = aLevelRollType;
			_dayToWeekRollType = aDayToWeekRollType;
			_storeToChainRollType = aStoreToChainRollType;
			_chainHistoryRollLevel = aChainHistoryRollLevel;
			_chainForecastRollLevel = aChainForecastRollLevel;
			_storeDailyHistoryRollLevel = aStoreDailyHistoryRollLevel;
			_storeWeeklyHistoryRollLevel = aStoreWeeklyHistoryRollLevel;
			_storeForecastRollLevel = aStoreForecastRollLevel;
			if (_chainDatabaseVariableType == eVariableDatabaseType.None)
			{
				_chainHistoryModelType = eVariableDatabaseModelType.None;
				_chainForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_chainHistoryModelType = aChainHistoryModelType;
				_chainForecastModelType = aChainForecastModelType;
			}
			if (_storeDatabaseVariableType == eVariableDatabaseType.None)
			{
				_storeDailyHistoryModelType = eVariableDatabaseModelType.None;
				_storeWeeklyHistoryModelType = eVariableDatabaseModelType.None;
				_storeForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_storeDailyHistoryModelType = aStoreDailyHistoryModelType;
				_storeWeeklyHistoryModelType = aStoreWeeklyHistoryModelType;
				_storeForecastModelType = aStoreForecastModelType;
			}
// Begin Track #4868 - JSmith - Variable Groupings
            _groupings = aGroupings;
// End Track #4868
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
		/// <param name="aVariableType">
		/// The database table where the values are found.
		/// </param>
		/// <param name="aCustomVariableType">
		/// Any client custom variable type assigned to this variable.
		/// </param>
		/// <param name="aDatabaseColumnName">
		/// The database column name of this profile.
		/// </param>
        /// <param name="aChainDatabaseVariableType">
		/// The type of chain database variable of this profile.
		/// </param>
        /// <param name="aStoreDatabaseVariableType">
        /// The type of store database variable of this profile.
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
		/// <param name="aEligibilityType">
		/// The eEligibilityType that describes which eligiblity type this Variable follows.
		/// </param>
		/// <param name="aSimilarStoreDateType">
		/// The eSimilarStoreDateType that describes which similar store type this Variable follows.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>
		/// <param name="aAllowForecastBalance">
		/// A flag that identifies if forecast balance, matrix, is allowed for this variable.
		/// </param>
		/// <param name="aAllowOTSForecast">
		/// A flag that identifies if the OTS forecast process is allowed for this variable.
		/// </param>
		/// <param name="aAllowAssortmentPlanning">
		/// A flag that identifies if Assortment Planning is allowed for this variable.
		/// </param>
		/// <param name="aLevelRollType">
		/// The level rollup type for this variable
		/// </param>
		/// <param name="aDayToWeekRollType">
		/// The day to week rollup type for this variable
		/// </param>
		/// <param name="aStoreToChainRollType">
		/// The store to chain rollup type for this variable
		/// </param>
		/// <param name="aChainForecastRollLevel">
		/// The level at which chain forcast values should be rolled
		/// </param>
		/// <param name="aChainHistoryRollLevel">
		/// The level at which chain history values should be rolled
		/// </param>
		/// <param name="aStoreForecastRollLevel">
		/// The level at which store forcast values should be rolled
		/// </param>
        /// <param name="aStoreDailyHistoryRollLevel">
		/// The level at which store daily history values should be rolled
		/// </param>
        /// <param name="aStoreWeeklyHistoryRollLevel">
        /// The level at which store weekly history values should be rolled
        /// </param>
        /// <param name="aGroupings">
        /// The levels of organization used to display the variable in the chooser
        /// </param>

		public VariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableType aVariableType,
			eClientCustomVariableType aCustomVariableType,
			string aDatabaseColumnName,
			eVariableDatabaseType aChainDatabaseVariableType,
			eVariableDatabaseType aStoreDatabaseVariableType,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableSpreadType aVariableSpreadType,
			eVariableWeekType aVariableWeekType,
			eEligibilityType aEligibilityType,
			eSimilarStoreDateType aSimilarStoreDateType,
			eVariableTimeTotalType aVariableTimeTotalType,
			eVariableForecastType aVariableForecastType,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals,
			double aNumDecimals,
			//End BonTon Calcs - JScott - Add Display Precision
			bool aAllowForecastBalance,
			bool aAllowOTSForecast,
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			bool aAllowAssortmentPlanning,
			//End TT#2 - JScott - Assortment Planning - Phase 2
			eLevelRollType aLevelRollType,
			eDayToWeekRollType aDayToWeekRollType,
			eStoreToChainRollType aStoreToChainRollType,
			int aChainHistoryRollLevel,
			int aChainForecastRollLevel,
			int aStoreDailyHistoryRollLevel,
			int aStoreWeeklyHistoryRollLevel,
			int aStoreForecastRollLevel,
			eVariableDatabaseModelType aChainHistoryModelType,
			eVariableDatabaseModelType aChainForecastModelType,
			eVariableDatabaseModelType aStoreDailyHistoryModelType,
			eVariableDatabaseModelType aStoreWeeklyHistoryModelType,
// Begin Track #4868 - JSmith - Variable Groupings
			//eVariableDatabaseModelType aStoreForecastModelType)
			eVariableDatabaseModelType aStoreForecastModelType,
            string aGroupings)
// End Track #4868
			: base(aKey, aVariableName, aVariableCategory, aVariableStyle, aVariableAccess, aVariableScope, aVariableSpreadType, aVariableWeekType, aVariableTimeTotalType, aFormatType, aNumDecimals)
		{
			_timeTotalVariables = new System.Collections.ArrayList();
			_timeTotalChainVariables = new System.Collections.ArrayList();
			_timeTotalStoreVariables = new System.Collections.ArrayList();
			_timeTotalXRef = new ProfileXRef(eProfileType.Variable, eProfileType.TimeTotalVariable);
			_variableType = aVariableType;
			_customVariableType = aCustomVariableType;
			_databaseColumnName = aDatabaseColumnName;
			_databaseColumnPosition = Include.NoColumnPosition;
			_chainDatabaseVariableType = aChainDatabaseVariableType;
			_storeDatabaseVariableType = aStoreDatabaseVariableType;
			_eligibilityType = aEligibilityType;
			_similarStoreDateType = aSimilarStoreDateType;
			_variableForecastType = aVariableForecastType;
			_associatedForecastVariable = null;
			_totalTimeTotalVarProf = null;
			_avgTimeTotalVarProf = null;
			_allowForecastBalance = aAllowForecastBalance;
			_allowOTSForecast = aAllowOTSForecast;
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			_allowAssortmentPlanning = aAllowAssortmentPlanning;
			//End TT#2 - JScott - Assortment Planning - Phase 2
			_levelRollType = aLevelRollType;
			_dayToWeekRollType = aDayToWeekRollType;
			_storeToChainRollType = aStoreToChainRollType;
			_chainHistoryRollLevel = aChainHistoryRollLevel;
			_chainForecastRollLevel = aChainForecastRollLevel;
			_storeDailyHistoryRollLevel = aStoreDailyHistoryRollLevel;
			_storeWeeklyHistoryRollLevel = aStoreWeeklyHistoryRollLevel;
			_storeForecastRollLevel = aStoreForecastRollLevel;
			if (_chainDatabaseVariableType == eVariableDatabaseType.None)
			{
				_chainHistoryModelType = eVariableDatabaseModelType.None;
				_chainForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_chainHistoryModelType = aChainHistoryModelType;
				_chainForecastModelType = aChainForecastModelType;
			}
			if (_storeDatabaseVariableType == eVariableDatabaseType.None)
			{
				_storeDailyHistoryModelType = eVariableDatabaseModelType.None;
				_storeWeeklyHistoryModelType = eVariableDatabaseModelType.None;
				_storeForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_storeDailyHistoryModelType = aStoreDailyHistoryModelType;
				_storeWeeklyHistoryModelType = aStoreWeeklyHistoryModelType;
				_storeForecastModelType = aStoreForecastModelType;
			}
// Begin Track #4868 - JSmith - Variable Groupings
            _groupings = aGroupings;
// End Track #4868
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
		/// <param name="aVariableType">
		/// The database table where the values are found.
		/// </param>
		/// <param name="aDatabaseColumnName">
		/// The database column name of this profile.
		/// </param>
		/// <param name="aChainDatabaseVariableType">
		/// The type of chain database variable of this profile.
		/// </param>
		/// <param name="aStoreDatabaseVariableType">
		/// The type of store database variable of this profile.
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
		/// <param name="aEligibilityType">
		/// The eEligibilityType that describes which eligiblity type this Variable follows.
		/// </param>
		/// <param name="aSimilarStoreDateType">
		/// The eSimilarStoreDateType that describes which similar store type this Variable follows.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>
		/// <param name="aAllowForecastBalance">
		/// A flag that identifies if forecast balance, matrix, is allowed for this variable.
		/// </param>
		/// <param name="aAllowOTSForecast">
		/// A flag that identifies if the OTS forecast process is allowed for this variable.
		/// </param>
		/// <param name="aAllowAssortmentPlanning">
		/// A flag that identifies if Assortment Planning is allowed for this variable.
		/// </param>
		/// <param name="aLevelRollType">
		/// The level rollup type for this variable
		/// </param>
		/// <param name="aDayToWeekRollType">
		/// The day to week rollup type for this variable
		/// </param>
		/// <param name="aStoreToChainRollType">
		/// The store to chain rollup type for this variable
		/// </param>
		/// <param name="aChainForecastRollLevel">
		/// The level at which chain forcast values should be rolled
		/// </param>
		/// <param name="aChainHistoryRollLevel">
		/// The level at which chain history values should be rolled
		/// </param>
		/// <param name="aStoreForecastRollLevel">
		/// The level at which store forcast values should be rolled
		/// </param>
		/// <param name="aStoreDailyHistoryRollLevel">
		/// The level at which store daily history values should be rolled
		/// </param>
		/// <param name="aStoreWeeklyHistoryRollLevel">
		/// The level at which store weekly history values should be rolled
		/// </param>
		/// <param name="aDisplaySequence">
		/// The position the variable is to be displayed
		/// </param>
        /// <param name="aGroupings">
        /// The levels of organization used to display the variable in the chooser
        /// </param>
		
		public VariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableType aVariableType,
			string aDatabaseColumnName,
			eVariableDatabaseType aChainDatabaseVariableType,
			eVariableDatabaseType aStoreDatabaseVariableType,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableSpreadType aVariableSpreadType,
			eVariableWeekType aVariableWeekType,
			eEligibilityType aEligibilityType,
			eSimilarStoreDateType aSimilarStoreDateType,
			eVariableTimeTotalType aVariableTimeTotalType,
			eVariableForecastType aVariableForecastType,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals,
			double aNumDecimals,
			//End BonTon Calcs - JScott - Add Display Precision
			bool aAllowForecastBalance,
			bool aAllowOTSForecast,
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			bool aAllowAssortmentPlanning,
			//End TT#2 - JScott - Assortment Planning - Phase 2
			eLevelRollType aLevelRollType,
			eDayToWeekRollType aDayToWeekRollType,
			eStoreToChainRollType aStoreToChainRollType,
			int aChainHistoryRollLevel,
			int aChainForecastRollLevel,
			int aStoreDailyHistoryRollLevel,
			int aStoreWeeklyHistoryRollLevel,
			int aStoreForecastRollLevel,
			eVariableDatabaseModelType aChainHistoryModelType,
			eVariableDatabaseModelType aChainForecastModelType,
			eVariableDatabaseModelType aStoreDailyHistoryModelType,
			eVariableDatabaseModelType aStoreWeeklyHistoryModelType,
			eVariableDatabaseModelType aStoreForecastModelType,
// Begin Track #4868 - JSmith - Variable Groupings
			//int aDisplaySequence)
			int aDisplaySequence,
            string aGroupings)
// End Track #4868
			: base(aKey, aVariableName, aVariableCategory, aVariableStyle, aVariableAccess, aVariableScope, aVariableSpreadType, aVariableWeekType, aVariableTimeTotalType, aFormatType, aNumDecimals, aDisplaySequence)
		{
			_timeTotalVariables = new System.Collections.ArrayList();
			_timeTotalChainVariables = new System.Collections.ArrayList();
			_timeTotalStoreVariables = new System.Collections.ArrayList();
			_timeTotalXRef = new ProfileXRef(eProfileType.Variable, eProfileType.TimeTotalVariable);
			_variableType = aVariableType;
			_customVariableType = eClientCustomVariableType.None;
			_databaseColumnName = aDatabaseColumnName;
			_databaseColumnPosition = Include.NoColumnPosition;
			_chainDatabaseVariableType = aChainDatabaseVariableType;
			_storeDatabaseVariableType = aStoreDatabaseVariableType;
			_eligibilityType = aEligibilityType;
			_similarStoreDateType = aSimilarStoreDateType;
			_variableForecastType = aVariableForecastType;
			_associatedForecastVariable = null;
			_totalTimeTotalVarProf = null;
			_avgTimeTotalVarProf = null;
			_allowForecastBalance = aAllowForecastBalance;
			_allowOTSForecast = aAllowOTSForecast;
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			_allowAssortmentPlanning = aAllowAssortmentPlanning;
			//End TT#2 - JScott - Assortment Planning - Phase 2
			_levelRollType = aLevelRollType;
			_dayToWeekRollType = aDayToWeekRollType;
			_storeToChainRollType = aStoreToChainRollType;
			_chainHistoryRollLevel = aChainHistoryRollLevel;
			_chainForecastRollLevel = aChainForecastRollLevel;
			_storeDailyHistoryRollLevel = aStoreDailyHistoryRollLevel;
			_storeWeeklyHistoryRollLevel = aStoreWeeklyHistoryRollLevel;
			_storeForecastRollLevel = aStoreForecastRollLevel;
			if (_chainDatabaseVariableType == eVariableDatabaseType.None)
			{
				_chainHistoryModelType = eVariableDatabaseModelType.None;
				_chainForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_chainHistoryModelType = aChainHistoryModelType;
				_chainForecastModelType = aChainForecastModelType;
			}
			if (_storeDatabaseVariableType == eVariableDatabaseType.None)
			{
				_storeDailyHistoryModelType = eVariableDatabaseModelType.None;
				_storeWeeklyHistoryModelType = eVariableDatabaseModelType.None;
				_storeForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_storeDailyHistoryModelType = aStoreDailyHistoryModelType;
				_storeWeeklyHistoryModelType = aStoreWeeklyHistoryModelType;
				_storeForecastModelType = aStoreForecastModelType;
			}
// Begin Track #4868 - JSmith - Variable Groupings
            _groupings = aGroupings;
// End Track #4868
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
		/// <param name="aVariableType">
		/// The database table where the values are found.
		/// </param>
		/// <param name="aCustomVariableType">
		/// Any client custom variable type assigned to this variable.
		/// </param>
		/// <param name="aDatabaseColumnName">
		/// The database column name of this profile.
		/// </param>
        /// <param name="aChainDatabaseVariableType">
		/// The type of chain database variable of this profile.
		/// </param>
        /// <param name="aStoreDatabaseVariableType">
        /// The type of store database variable of this profile.
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
		/// <param name="aEligibilityType">
		/// The eEligibilityType that describes which eligiblity type this Variable follows.
		/// </param>
		/// <param name="aSimilarStoreDateType">
		/// The eSimilarStoreDateType that describes which similar store type this Variable follows.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>
		/// <param name="aAllowForecastBalance">
		/// A flag that identifies if forecast balance, matrix, is allowed for this variable.
		/// </param>
		/// <param name="aAllowOTSForecast">
		/// A flag that identifies if the OTS forecast process is allowed for this variable.
		/// </param>
		/// <param name="aAllowAssortmentPlanning">
		/// A flag that identifies if Assortment Planning is allowed for this variable.
		/// </param>
		/// <param name="aLevelRollType">
		/// The level rollup type for this variable
		/// </param>
		/// <param name="aDayToWeekRollType">
		/// The day to week rollup type for this variable
		/// </param>
		/// <param name="aStoreToChainRollType">
		/// The store to chain rollup type for this variable
		/// </param>
		/// <param name="aChainForecastRollLevel">
		/// The level at which chain forcast values should be rolled
		/// </param>
		/// <param name="aChainHistoryRollLevel">
		/// The level at which chain history values should be rolled
		/// </param>
		/// <param name="aStoreForecastRollLevel">
		/// The level at which store forcast values should be rolled
		/// </param>
		/// <param name="aStoreDailyHistoryRollLevel">
		/// The level at which store daily history values should be rolled
		/// </param>
		/// <param name="aStoreWeeklyHistoryRollLevel">
		/// The level at which store weekly history values should be rolled
		/// </param>
		/// <param name="aDisplaySequence">
		/// The position the variable is to be displayed
		/// </param>
        /// <param name="aGroupings">
        /// The levels of organization used to display the variable in the chooser
        /// </param>
		
		public VariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableType aVariableType,
			eClientCustomVariableType aCustomVariableType,
			string aDatabaseColumnName,
			eVariableDatabaseType aChainDatabaseVariableType,
			eVariableDatabaseType aStoreDatabaseVariableType,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableSpreadType aVariableSpreadType,
			eVariableWeekType aVariableWeekType,
			eEligibilityType aEligibilityType,
			eSimilarStoreDateType aSimilarStoreDateType,
			eVariableTimeTotalType aVariableTimeTotalType,
			eVariableForecastType aVariableForecastType,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals,
			double aNumDecimals,
			//End BonTon Calcs - JScott - Add Display Precision
			bool aAllowForecastBalance,
			bool aAllowOTSForecast,
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			bool aAllowAssortmentPlanning,
			//End TT#2 - JScott - Assortment Planning - Phase 2
			eLevelRollType aLevelRollType,
			eDayToWeekRollType aDayToWeekRollType,
			eStoreToChainRollType aStoreToChainRollType,
			int aChainHistoryRollLevel,
			int aChainForecastRollLevel,
			int aStoreDailyHistoryRollLevel,
			int aStoreWeeklyHistoryRollLevel,
			int aStoreForecastRollLevel,
			eVariableDatabaseModelType aChainHistoryModelType,
			eVariableDatabaseModelType aChainForecastModelType,
			eVariableDatabaseModelType aStoreDailyHistoryModelType,
			eVariableDatabaseModelType aStoreWeeklyHistoryModelType,
			eVariableDatabaseModelType aStoreForecastModelType,
// Begin Track #4868 - JSmith - Variable Groupings
			//int aDisplaySequence)
			int aDisplaySequence,
            string aGroupings)
// End Track #4868
			: base(aKey, aVariableName, aVariableCategory, aVariableStyle, aVariableAccess, aVariableScope, aVariableSpreadType, aVariableWeekType, aVariableTimeTotalType, aFormatType, aNumDecimals, aDisplaySequence)
		{
			_timeTotalVariables = new System.Collections.ArrayList();
			_timeTotalChainVariables = new System.Collections.ArrayList();
			_timeTotalStoreVariables = new System.Collections.ArrayList();
			_timeTotalXRef = new ProfileXRef(eProfileType.Variable, eProfileType.TimeTotalVariable);
			_variableType = aVariableType;
			_customVariableType = aCustomVariableType;
			_databaseColumnName = aDatabaseColumnName;
			_databaseColumnPosition = Include.NoColumnPosition;
			_chainDatabaseVariableType = aChainDatabaseVariableType;
			_storeDatabaseVariableType = aStoreDatabaseVariableType;
			_eligibilityType = aEligibilityType;
			_similarStoreDateType = aSimilarStoreDateType;
			_variableForecastType = aVariableForecastType;
			_associatedForecastVariable = null;
			_totalTimeTotalVarProf = null;
			_avgTimeTotalVarProf = null;
			_allowForecastBalance = aAllowForecastBalance;
			_allowOTSForecast = aAllowOTSForecast;
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			_allowAssortmentPlanning = aAllowAssortmentPlanning;
			//End TT#2 - JScott - Assortment Planning - Phase 2
			_levelRollType = aLevelRollType;
			_dayToWeekRollType = aDayToWeekRollType;
			_storeToChainRollType = aStoreToChainRollType;
			_chainHistoryRollLevel = aChainHistoryRollLevel;
			_chainForecastRollLevel = aChainForecastRollLevel;
			_storeDailyHistoryRollLevel = aStoreDailyHistoryRollLevel;
			_storeWeeklyHistoryRollLevel = aStoreWeeklyHistoryRollLevel;
			_storeForecastRollLevel = aStoreForecastRollLevel;
			if (_chainDatabaseVariableType == eVariableDatabaseType.None)
			{
				_chainHistoryModelType = eVariableDatabaseModelType.None;
				_chainForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_chainHistoryModelType = aChainHistoryModelType;
				_chainForecastModelType = aChainForecastModelType;
			}
			if (_storeDatabaseVariableType == eVariableDatabaseType.None)
			{
				_storeDailyHistoryModelType = eVariableDatabaseModelType.None;
				_storeWeeklyHistoryModelType = eVariableDatabaseModelType.None;
				_storeForecastModelType = eVariableDatabaseModelType.None;
			}
			else
			{
				_storeDailyHistoryModelType = aStoreDailyHistoryModelType;
				_storeWeeklyHistoryModelType = aStoreWeeklyHistoryModelType;
				_storeForecastModelType = aStoreForecastModelType;
			}
// Begin Track #4868 - JSmith - Variable Groupings
            _groupings = aGroupings;
// End Track #4868
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
		
		override public bool isDatabaseVariable(eVariableCategory aVarCateg, int aVersionRID, eCalendarDateType aCalDateType)
		{
			if (_databaseColumnName != null)
			{
				if (aVarCateg == eVariableCategory.Store)
				{
					if (aVersionRID == Include.FV_ActualRID)
					{
						if (aCalDateType == eCalendarDateType.Day)
						{
							if (StoreDailyHistoryModelType == eVariableDatabaseModelType.All)
							{
								return true;
							}
						}
						else
						{
							if (StoreWeeklyHistoryModelType == eVariableDatabaseModelType.All)
							{
								return true;
							}
						}
					}
					else
					{
						if (aCalDateType == eCalendarDateType.Day)
						{
							return false;
						}
						else
						{
							if (StoreForecastModelType == eVariableDatabaseModelType.All)
							{
								return true;
							}
						}
					}
				}
				else if (aVarCateg == eVariableCategory.Chain)
				{
					if (aVersionRID == Include.FV_ActualRID)
					{
						if (aCalDateType == eCalendarDateType.Day)
						{
							return false;
						}
						else
						{
							if (ChainHistoryModelType == eVariableDatabaseModelType.All)
							{
								return true;
							}
						}
					}
					else
					{
						if (aCalDateType == eCalendarDateType.Day)
						{
							return false;
						}
						else
						{
							if (ChainForecastModelType == eVariableDatabaseModelType.All)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Variable;
			}
		}

		/// <summary>
		/// Gets the database table of this profile.
		/// </summary>

		public eVariableType VariableType
		{
			get
			{
				return _variableType;
			}
		}

		/// <summary>
		/// Gets the client custom variable type of this profile.
		/// </summary>

		public eClientCustomVariableType CustomVariableType
		{
			get
			{
				return _customVariableType;
			}
		}

		/// <summary>
		/// Gets the database column name of this profile.
		/// </summary>

		public string DatabaseColumnName
		{
			get
			{
				return _databaseColumnName;
			}
		}

		/// <summary>
		/// Gets the database column position of this profile.
		/// </summary>

		public int DatabaseColumnPosition
		{
			get
			{
				return _databaseColumnPosition;
			}
			set
			{
				_databaseColumnPosition = value;
			}
		}

		/// <summary>
		/// Gets the database column position of this profile.
		/// </summary>

		public eVariableDatabaseType ChainDatabaseVariableType
		{
			get
			{
				return _chainDatabaseVariableType;
			}
		}

		/// <summary>
		/// Gets the database column position of this profile.
		/// </summary>

		public eVariableDatabaseType StoreDatabaseVariableType
		{
			get
			{
				return _storeDatabaseVariableType;
			}
		}

		/// <summary>
		/// Gets the eEligibilityType for this profile.
		/// </summary>

		public eEligibilityType EligibilityType
		{
			get
			{
				return _eligibilityType;
			}
		}

		/// <summary>
		/// Gets the eSimilarStoreDateType for this profile.
		/// </summary>

		public eSimilarStoreDateType SimilarStoreDateType
		{
			get
			{
				return _similarStoreDateType;
			}
		}

		/// <summary>
		/// Gets the eVariableForecastType for this profile.
		/// </summary>

		public eVariableForecastType VariableForecastType
		{
			get
			{
				return _variableForecastType;
			}
		}

		/// <summary>
		/// Gets the VariableProfile assocated Forecast Variable for this profile.
		/// </summary>

		public VariableProfile AssociatedForecastVariable
		{
			get
			{
				return _associatedForecastVariable;
			}
			set
			{
				_associatedForecastVariable = value;
			}
		}

		/// <summary>
		/// Gets the ArrayList containing all Total Variables for this profile.
		/// </summary>

		public System.Collections.ArrayList TimeTotalVariables
		{
			get
			{
				return _timeTotalVariables;
			}
		}

		/// <summary>
		/// Gets the ArrayList containing all Store Total Variables for this profile.
		/// </summary>

		public System.Collections.ArrayList TimeTotalStoreVariables
		{
			get
			{
				return _timeTotalStoreVariables;
			}
		}

		/// <summary>
		/// Gets the ArrayList containing all Chain Total Variables for this profile.
		/// </summary>

		public System.Collections.ArrayList TimeTotalChainVariables
		{
			get
			{
				return _timeTotalChainVariables;
			}
		}

		/// <summary>
		/// Gets or sets the ProfileList of variables used to retrieve the Actual WTD Sales.
		/// </summary>

		public ProfileList ActualWTDSalesVariableList
		{
			get
			{
				return _actualWTDSalesVariableList;
			}
			set
			{
				_actualWTDSalesVariableList = value;
			}
		}

		/// <summary>
		/// Gets or sets the total TimeTotalVariableProfile for this profile.
		/// </summary>

		public TimeTotalVariableProfile TotalTimeTotalVariableProfile
		{
			get
			{
				return _totalTimeTotalVarProf;
			}
			set
			{
				_totalTimeTotalVarProf = value;
			}
		}

		/// <summary>
		/// Gets or sets the average TimeTotalVariableProfile for this profile.
		/// </summary>

		public TimeTotalVariableProfile AverageTimeTotalVariableProfile
		{
			get
			{
				return _avgTimeTotalVarProf;
			}
			set
			{
				_avgTimeTotalVarProf = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if forecast balance should be allowed for this variable.
		/// </summary>

		public bool AllowForecastBalance
		{
			get
			{
				return _allowForecastBalance;
			}
			set
			{
				_allowForecastBalance = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if OTD Forecast should be allowed for this variable.
		/// </summary>

		public bool AllowOTSForecast
		{
			get
			{
				return _allowOTSForecast;
			}
			set
			{
				_allowOTSForecast = value;
			}
		}

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// Gets or sets the flag identifying if Assortment Planning should be allowed for this variable.
		/// </summary>

		public bool AllowAssortmentPlanning
		{
			get
			{
				return _allowAssortmentPlanning;
			}
			set
			{
				_allowAssortmentPlanning = value;
			}
		}

		//End TT#2 - JScott - Assortment Planning - Phase 2
		/// <summary>
		/// Gets or sets the level rollup type for this variable.
		/// </summary>

		public eLevelRollType LevelRollType
		{
			get
			{
				return _levelRollType;
			}
			set
			{
				_levelRollType = value;
			}
		}

		/// <summary>
		/// Gets or sets the day to week rollup type for this variable.
		/// </summary>

		public eDayToWeekRollType DayToWeekRollType
		{
			get
			{
				return _dayToWeekRollType;
			}
			set
			{
				_dayToWeekRollType = value;
			}
		}

		/// <summary>
		/// Gets or sets the store to chain rollup type for this variable.
		/// </summary>

		public eStoreToChainRollType StoreToChainRollType
		{
			get
			{
				return _storeToChainRollType;
			}
			set
			{
				_storeToChainRollType = value;
			}
		}

		/// <summary>
		/// Gets or sets the level at which chain history values should be rolled.
		/// </summary>

		public int ChainHistoryRollLevel
		{
			get
			{
				return _chainHistoryRollLevel;
			}
			set
			{
				_chainHistoryRollLevel = value;
			}
		}

		/// <summary>
		/// Gets or sets the level at which store daily history values should be rolled.
		/// </summary>

		public int StoreDailyHistoryRollLevel
		{
			get
			{
				return _storeDailyHistoryRollLevel;
			}
			set
			{
				_storeDailyHistoryRollLevel = value;
			}
		}

		/// <summary>
		/// Gets or sets the level at which store weekly history values should be rolled.
		/// </summary>

		public int StoreWeeklyHistoryRollLevel
		{
			get
			{
				return _storeWeeklyHistoryRollLevel;
			}
			set
			{
				_storeWeeklyHistoryRollLevel = value;
			}
		}

		/// <summary>
		/// Gets or sets the level at which chain forcast values should be rolled.
		/// </summary>

		public int ChainForecastRollLevel
		{
			get
			{
				return _chainForecastRollLevel;
			}
			set
			{
				_chainForecastRollLevel = value;
			}
		}

		/// <summary>
		/// Gets or sets the level at which store forcast values should be rolled.
		/// </summary>

		public int StoreForecastRollLevel
		{
			get
			{
				return _storeForecastRollLevel;
			}
			set
			{
				_storeForecastRollLevel = value;
			}
		}

		public eVariableDatabaseModelType ChainHistoryModelType
		{
			get
			{
				return _chainHistoryModelType;
			}
			set
			{
				_chainHistoryModelType = value;
			}
		}

		public eVariableDatabaseModelType ChainForecastModelType
		{
			get
			{
				return _chainForecastModelType;
			}
			set
			{
				_chainForecastModelType = value;
			}
		}

		public eVariableDatabaseModelType StoreDailyHistoryModelType
		{
			get
			{
				return _storeDailyHistoryModelType;
			}
			set
			{
				_storeDailyHistoryModelType = value;
			}
		}

		public eVariableDatabaseModelType StoreWeeklyHistoryModelType
		{
			get
			{
				return _storeWeeklyHistoryModelType;
			}
			set
			{
				_storeWeeklyHistoryModelType = value;
			}
		}

		public eVariableDatabaseModelType StoreForecastModelType
		{
			get
			{
				return _storeForecastModelType;
			}
			set
			{
				_storeForecastModelType = value;
			}
		}

// Begin Track #4868 - JSmith - Variable Groupings
        public string Groupings
        {
            get
            {
                return _groupings;
            }
            set
            {
                _groupings = value;
            }
        }
// End Track #4868

		//========
		// METHODS
		//========

		public object Clone()
		{
			try
			{
				VariableProfile variableProfile = (VariableProfile)this.MemberwiseClone();
				return variableProfile;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method adds a VariableProfile to the total variable list.
		/// </summary>
		/// <param name="aTimeTotalVariables">
		/// A parameter list of total VariableProfiles to add. 
		/// </param>

		public void AddTimeTotalVariable(params TimeTotalVariableProfile[] aTimeTotalVariables)
		{
			int i;

			for (i = 0; i < aTimeTotalVariables.Length; i++)
			{
				_timeTotalVariables.Add(aTimeTotalVariables[i]);
				_timeTotalXRef.AddXRefIdEntry(aTimeTotalVariables[i].Key, _timeTotalVariables.Count);

				switch (((TimeTotalVariableProfile)aTimeTotalVariables[i]).VariableCategory)
				{
					case eVariableCategory.Both:
						_timeTotalStoreVariables.Add(aTimeTotalVariables[i]);
						_timeTotalChainVariables.Add(aTimeTotalVariables[i]);
						break;

					case eVariableCategory.Store:
						_timeTotalStoreVariables.Add(aTimeTotalVariables[i]);
						break;

					case eVariableCategory.Chain:
						_timeTotalChainVariables.Add(aTimeTotalVariables[i]);
						break;
				}
			}
		}

		/// <summary>
		/// This method returns the variable profile of the TimeTotalVariable associated with this index.
		/// </summary>
		/// <param name="aTimeTotalIndex">
		/// The Time Total index.
		/// </param>
		/// <returns>
		/// The VariableProfile associated with this Variable for the given index.  Null if key does not exist.
		/// </returns>

		public TimeTotalVariableProfile GetTimeTotalVariable(int aTimeTotalIndex)
		{
			int timeTotIdx;

			timeTotIdx = aTimeTotalIndex - 1;
			if (timeTotIdx < _timeTotalVariables.Count)
			{
				return (TimeTotalVariableProfile)_timeTotalVariables[timeTotIdx];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// This method returns the variable profile of the Chain TimeTotalVariable associated with this index.
		/// </summary>
		/// <param name="aTimeTotalIndex">
		/// The Time Total index.
		/// </param>
		/// <returns>
		/// The VariableProfile associated with this Variable for the given index.  Null if key does not exist.
		/// </returns>

		public TimeTotalVariableProfile GetChainTimeTotalVariable(int aTimeTotalIndex)
		{
			int timeTotIdx;

			timeTotIdx = 0;

			foreach (TimeTotalVariableProfile timeTotVarProf in _timeTotalVariables)
			{
				if (timeTotVarProf.VariableCategory == eVariableCategory.Both || timeTotVarProf.VariableCategory == eVariableCategory.Chain)
				{
					timeTotIdx++;
				}

				if (timeTotIdx == aTimeTotalIndex)
				{
					return timeTotVarProf;
				}
			}

			return null;
		}

		/// <summary>
		/// This method returns the variable profile of the Store TimeTotalVariable associated with this index.
		/// </summary>
		/// <param name="aTimeTotalIndex">
		/// The Time Total index.
		/// </param>
		/// <returns>
		/// The VariableProfile associated with this Variable for the given index.  Null if key does not exist.
		/// </returns>

		public TimeTotalVariableProfile GetStoreTimeTotalVariable(int aTimeTotalIndex)
		{
			int timeTotIdx;

			timeTotIdx = 0;

			foreach (TimeTotalVariableProfile timeTotVarProf in _timeTotalVariables)
			{
				if (timeTotVarProf.VariableCategory == eVariableCategory.Both || timeTotVarProf.VariableCategory == eVariableCategory.Store)
				{
					timeTotIdx++;
				}

				if (timeTotIdx == aTimeTotalIndex)
				{
					return timeTotVarProf;
				}
			}

			return null;
		}

		/// <summary>
		/// This method returns the Time Total Index of the given TimeTotalVariableProfile.
		/// </summary>
		/// <param name="aTimeTotalVarProf">
		/// The VariableProfile of the Time Total to lookup.
		/// </param>
		/// <returns>
		/// The Time Total Index of the given TimeTotalVariableProfile.
		/// </returns>

		public int GetTimeTotalIndex(TimeTotalVariableProfile aTimeTotalVarProf)
		{
			ArrayList xRefList;

			xRefList = _timeTotalXRef.GetDetailList(aTimeTotalVarProf.Key);
			if (xRefList != null && xRefList.Count > 0)
			{
				return (int)xRefList[0];
			}
			else
			{
				return -1;
			}
		}
	}

	[Serializable]
	public class TimeTotalVariableProfile : ComputationVariableProfile
	{
		//=======
		// FIELDS
		//=======

		private VariableProfile _parentVarProf;

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
		/// The eVariableScope that describes the variable access of this Variable.
		/// </param>
		/// <param name="aVariableScope">
		/// The eVariableScope that describes the variable scope of this Variable.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>

		public TimeTotalVariableProfile(
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
			: base(aKey, aVariableName, aVariableCategory, aVariableStyle, aVariableAccess, aVariableScope, aVariableSpreadType, aVariableWeekType, aVariableTimeTotalType, aFormatType, aNumDecimals)
		{
			_parentVarProf = null;
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
		
		override public bool isDatabaseVariable(eVariableCategory aVarCateg, int aVersionRID, eCalendarDateType aCalDateType)
		{
			return false;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.TimeTotalVariable;
			}
		}

		/// <summary>
		/// Gets the VariableProfile containing all parent VariableProfile for this profile.
		/// </summary>

		public VariableProfile ParentVariableProfile
		{
			get
			{
				return _parentVarProf;
			}
			set
			{
				_parentVarProf = value;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The QuantityVariableProfile class identifies the QuantityVariable profile.
	/// </summary>

	[Serializable]
	public class QuantityVariableProfile : ComputationVariableProfile
	{
		//=======
		// FIELDS
		//=======

		private QuantityLevelFlags _levelFlags;
		private bool _isSelectable;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of QuantityVariableProfile using the given Id, variable name, eVariableAccess, eVariableScope, eFormatType,
		/// and number of decimals.
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
		/// The eVariableScope that describes the variable access of this Variable.
		/// </param>
		/// <param name="aVariableScope">
		/// The eVariableScope that describes the variable scope of this Variable.
		/// </param>
		/// <param name="aFormatType">
		/// The eFormatType that identifies the way to format the value as a string.
		/// </param>
		/// <param name="aNumDecimals">
		/// The number of decimals used for a eFormatType.GenericNumeric format.
		/// </param>

		public QuantityVariableProfile(
			int aKey,
			string aVariableName,
			eVariableCategory aVariableCategory,
			eVariableStyle aVariableStyle,
			eVariableAccess aVariableAccess,
			eVariableScope aVariableScope,
			eVariableWeekType aVariableWeekType,
			ushort aCubeFlags,
			ushort aViewFlags,
			bool aIsSelectable,
			eValueFormatType aFormatType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//int aNumDecimals)
			double aNumDecimals)
			//End BonTon Calcs - JScott - Add Display Precision
				: base(aKey, aVariableName, aVariableCategory, aVariableStyle, aVariableAccess, aVariableScope, eVariableSpreadType.None, aVariableWeekType, eVariableTimeTotalType.None, aFormatType, aNumDecimals)
		{
			_levelFlags = new QuantityLevelFlags(aCubeFlags, aViewFlags);
			_isSelectable = aIsSelectable;
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
		
		override public bool isDatabaseVariable(eVariableCategory aVarCateg, int aVersionRID, eCalendarDateType aCalDateType)
		{
			return false;
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.QuantityVariable;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for high-level.
		/// </summary>

		public bool isHighLevel
		{
			get
			{
				return _levelFlags.isHighLevel;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for low-level.
		/// </summary>

		public bool isLowLevel
		{
			get
			{
				return _levelFlags.isLowLevel;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for low-level total.
		/// </summary>

		public bool isLowLevelTotal
		{
			get
			{
				return _levelFlags.isLowLevelTotal;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for store detail.
		/// </summary>

		public bool isStoreDetailCube
		{
			get
			{
				return _levelFlags.isStoreDetailCube;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for store sets.
		/// </summary>

		public bool isStoreSetCube
		{
			get
			{
				return _levelFlags.isStoreSetCube;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for store totals.
		/// </summary>

		public bool isStoreTotalCube
		{
			get
			{
				return _levelFlags.isStoreTotalCube;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for chain.
		/// </summary>

		public bool isChainDetailCube
		{
			get
			{
				return _levelFlags.isChainDetailCube;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for chain single views.
		/// </summary>

		public bool isChainSingleView
		{
			get
			{
				return _levelFlags.isChainSingleView;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for chain multi views.
		/// </summary>

		public bool isChainMultiView
		{
			get
			{
				return _levelFlags.isChainMultiView;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for store single views.
		/// </summary>

		public bool isStoreSingleView
		{
			get
			{
				return _levelFlags.isStoreSingleView;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is displayed for store multi views.
		/// </summary>

		public bool isStoreMultiView
		{
			get
			{
				return _levelFlags.isStoreMultiView;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if this QuantityVariable is selectable on the window.
		/// </summary>

		public bool isSelectable
		{
			get
			{
				return _isSelectable;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns a boolean indicating if this instance of QuantityLevelFlags is valid for the given cube flag values.
		/// </summary>
		/// <param name="aCubeFlags">
		/// The cube flag values to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if this instance of QuantityLevelFlags is valid.
		/// </returns>

		public bool IsValidQuantityCube(ushort aCubeFlags)
		{
			try
			{
				return _levelFlags.IsValidCube(aCubeFlags);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	[Serializable]
	public class PlanValueProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanValueProfile using the given Id.
		/// </summary>
		/// <param name="aKey">
		/// The Id of this profile.
		/// </param>

		public PlanValueProfile(int aKey)
			: base(aKey)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.PlanValue;
			}
		}

		//========
		// METHODS
		//========
	}
}
