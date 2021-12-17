using System;
using System.Collections;
using System.Data;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Common
{
	/// <summary>
	/// Used to retrieve and update information about a model
	/// </summary>
	[Serializable()]
	abstract public class ModelProfile : Profile
	{
		private eChangeType					_modelChangeType;
		private eLockStatus					_modelLockStatus;
		private string						_modelID;
		private DateTime					_updateDateTime;
		private bool						_needsRebuilt;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelProfile(int aKey)
			: base(aKey)
		{
			_modelChangeType = eChangeType.none;
		}

		/// <summary>
		/// Gets or sets the type of change for the model.
		/// </summary>
		public eChangeType ModelChangeType 
		{
			get { return _modelChangeType ; }
			set { _modelChangeType = value; }
		}
		/// <summary>
		/// Gets or sets the status of lock for the model.
		/// </summary>
		public eLockStatus ModelLockStatus 
		{
			get { return _modelLockStatus ; }
			set { _modelLockStatus = value; }
		}
		/// <summary>
		/// Gets or sets the id of the  model.
		/// </summary>
		public string ModelID 
		{
			get { return _modelID ; }
			set { _modelID = value; }
		}
		/// <summary>
		/// Gets or sets the date and time stamp of the last time this model was updated.
		/// </summary>
		public DateTime	UpdateDateTime 
		{
			get { return _updateDateTime ; }
			set { _updateDateTime = value; }
		}
		/// <summary>
		/// Gets the string value for date and time stamp of the last time this model was updated.
		/// </summary>
		public string UpdateDateTimeString 
		{
			get 
			{ 
				return _updateDateTime.ToShortDateString() + " " + _updateDateTime.ToShortTimeString() ; 
			}
		}
		/// <summary>
		/// Gets or sets a flag indentifying if the model date information needs rebuilt before it can be used.
		/// </summary>
		public bool NeedsRebuilt 
		{
			get { return _needsRebuilt ; }
			set { _needsRebuilt = value; }
		}
	}

	/// <summary>
	/// Used to retrieve and update information about an eligibility model
	/// </summary>
	[Serializable()]
	public class ForecastBalanceProfile : ModelProfile
	{
		//=============
		// FIELDS
		//=============
		private eBalanceMode				_balanceMode;
		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private eMatrixType					_matrixType;
		// END MID Track #5647
		private eIterationType				_iterationType;
		private int							_iterationCount;
		private string						_computationMode;
		private ProfileList					_variables;
		private MIDRetail.Data.ModelsData		_modelsData;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ForecastBalanceProfile(int aKey)
			: base(aKey)
		{
			_variables = new ProfileList(eProfileType.ModelVariable);
            if (aKey != Include.NoRID)
            {
                LoadProfile(aKey);
            }
            // Begin TT#1748 - JSmith - Matrix balance method gives an error message when attempting to process
            else
            {
                _computationMode = "Default";
            }
            // End TT#1748
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ForecastBalanceProfile(string aValue)
			: base(Include.NoRID)
		{
			_variables = new ProfileList(eProfileType.ModelVariable);
			LoadProfile(aValue);
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
				return eProfileType.ForecastBalanceModel;
			}
		}

		/// <summary>
		/// Gets or sets the balance mode of the model.
		/// </summary>
		public eBalanceMode BalanceMode 
		{
			get { return _balanceMode ; }
			set { _balanceMode = value; }
		}

		/// <summary>
		/// Gets or sets the matrix type of the model.
		/// </summary>
		/// <remarks>
		/// This is the matrix model type
		/// </remarks>
		/// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		public eMatrixType MatrixType 
		{
			get { return _matrixType ; }
			set { _matrixType = value; }
		}
		/// END MID Track #5647	

		/// <summary>
		/// Gets or sets the iteration type of the model.
		/// </summary>
		/// <remarks>
		/// This is an ArrayList of inforamation for the type of model
		/// </remarks>
		public eIterationType IterationType 
		{
			get { return _iterationType ; }
			set { _iterationType = value; }
		}

		/// <summary>
		/// Gets or sets the number of iterations for the model.
		/// </summary>
		public int IterationCount 
		{
			get { return _iterationCount ; }
			set { _iterationCount = value; }
		}
		/// <summary>
		/// Gets or sets the computation mode for the model.
		/// </summary>
		public string ComputationMode 
		{
			get { return _computationMode ; }
			set {_computationMode = value; }
		}
		/// <summary>
		/// Gets or sets the list of variables that are to be processed by this model.
		/// </summary>
		public ProfileList Variables 
		{
			get { return _variables ; }
			set {_variables = value; }
		}

		//===========
		// METHODS
		//===========

		public void LoadProfile(int aKey)
		{
			try
			{
				if (_modelsData == null) _modelsData = new ModelsData();
				System.Data.DataTable dt = _modelsData.ForecastBalModel_Read(this.Key);
				if (dt.Rows.Count < 1)
				{
					return;
				}
				System.Data.DataRow dr = dt.Rows[0];
				this.ModelID = Convert.ToString(dr["FBMOD_ID"], CultureInfo.CurrentUICulture);
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				if (dr["MATRIX_TYPE"] != System.DBNull.Value) {
					this.MatrixType = (eMatrixType)Convert.ToInt32(dr["MATRIX_TYPE"], CultureInfo.CurrentUICulture);
				}
				// END MID Track #5647
				this.IterationType = (eIterationType)Convert.ToInt32(dr["ITERATIONS_TYPE"], CultureInfo.CurrentUICulture);
				this.IterationCount = Convert.ToInt32(dr["ITERATIONS_COUNT"], CultureInfo.CurrentUICulture);
				this.BalanceMode = (eBalanceMode)Convert.ToInt32(dr["BALANCE_MODE"], CultureInfo.CurrentUICulture);
				this.ComputationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);
				LoadVariables();
			}
			catch
			{
				throw;
			}
		}

		public void LoadProfile(string aValue)
		{
			try
			{
				if (_modelsData == null) _modelsData = new ModelsData();
				System.Data.DataTable dt = _modelsData.ForecastBalModel_Read(aValue);
				if (dt.Rows.Count < 1)
				{
					this.Key = Include.NoRID;
					return;
				}
				System.Data.DataRow dr = dt.Rows[0];
				this.Key = Convert.ToInt32(dr["FBMOD_RID"], CultureInfo.CurrentUICulture);
				this.ModelID = Convert.ToString(dr["FBMOD_ID"], CultureInfo.CurrentUICulture);
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				if (dr["MATRIX_TYPE"] != System.DBNull.Value) 
				{
					this.MatrixType = (eMatrixType)Convert.ToInt32(dr["MATRIX_TYPE"], CultureInfo.CurrentUICulture);
				}
				// END MID Track #5647
				this.IterationType = (eIterationType)Convert.ToInt32(dr["ITERATIONS_TYPE"], CultureInfo.CurrentUICulture);
				this.IterationCount = Convert.ToInt32(dr["ITERATIONS_COUNT"], CultureInfo.CurrentUICulture);
				this.BalanceMode = (eBalanceMode)Convert.ToInt32(dr["BALANCE_MODE"], CultureInfo.CurrentUICulture);
				this.ComputationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);
				LoadVariables();
			}
			catch
			{
				throw;
			}
		}

		private void LoadVariables()
		{
			try
			{
				DataTable dt = _modelsData.ForecastBalModelVariable_Read(Key);
				if (dt.Rows.Count < 1)
				{
					return;
				}
				foreach (DataRow dr in dt.Rows)
				{
					ModelVariableProfile mvp = new ModelVariableProfile(Convert.ToInt32(dr["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture));
					mvp.IsSelected = true;
					mvp.VariableProfile = null;
					Variables.Add(mvp);
				}
			}
			catch
			{
				throw;
			}
		}


		public int WriteProfile()
		{
			try
			{
				if (_modelsData == null) _modelsData = new ModelsData();
				
				_modelsData.OpenUpdateConnection();
				if (ModelChangeType == eChangeType.add)
				{
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					Key = _modelsData.ForecastBalModel_Add(ModelID, MatrixType, IterationType, IterationCount,
						BalanceMode, ComputationMode);
					// END MID Track #5647
					int count = 0;
					foreach (ModelVariableProfile mvp in Variables)
					{
						_modelsData.ForecastBalModelVariable_Add(Key, count, mvp.VariableProfile.Key);
						++count;
					}
				}
				else if (ModelChangeType == eChangeType.update)
				{
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					_modelsData.ForecastBalModel_Update(Key, ModelID, MatrixType, IterationType, IterationCount,
						BalanceMode, ComputationMode);
					// END MID Track #5647
					_modelsData.ForecastBalModelVariable_Delete(Key);
					int count = 0;
					foreach (ModelVariableProfile mvp in Variables)
					{
						_modelsData.ForecastBalModelVariable_Add(Key, count, mvp.VariableProfile.Key);
						++count;
					}
				}
				else if (ModelChangeType == eChangeType.delete)
				{
					_modelsData.ForecastBalModel_Delete(Key);
				}
				_modelsData.CommitData();
				return Key;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_modelsData != null &&
					_modelsData.ConnectionIsOpen)
				{
					_modelsData.CloseUpdateConnection();
				}
			}
		}
	}

	/// <summary>
	/// Used to retrieve a list of forecast falance model profiles
	/// </summary>
	[Serializable()]
	public class ForecastBalanceProfileList : ProfileList
	{
		//=======
		// FIELDS
		//=======
		private MIDRetail.Data.ModelsData _modelsData;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ForecastBalanceProfileList(bool aLoadProfiles)
			: base(eProfileType.ForecastBalanceModel)
		{
			try
			{
				if (aLoadProfiles)
				{
					LoadAll();
				}
			}
			catch
			{
				throw;
			}
		}

		//===========
		// METHODS
		//===========

		public void LoadAll()
		{
			try
			{
				ForecastBalanceProfile fbp;
				if (_modelsData == null) _modelsData = new ModelsData();

				System.Data.DataTable dt = _modelsData.ForecastBalModel_Read();
				foreach(System.Data.DataRow dr in dt.Rows)
				{
					fbp = new ForecastBalanceProfile(Convert.ToInt32(dr["FBMOD_RID"], CultureInfo.CurrentUICulture));
					this.Add(fbp);
				}
			}
			catch
			{
				throw;
			}
		}
	}


	/// <summary>
	/// Used to retrieve and update information about a Forecasting model
	/// </summary>
	[Serializable()]
	public class ForecastModelProfile : ModelProfile
	{
		//=============
		// FIELDS
		//=============
		private bool						_isDefault;
		private string						_computationMode;
		private ProfileList					_variables;
		private MIDRetail.Data.ModelsData		_modelsData;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ForecastModelProfile(int aKey)
			: base(aKey)
		{
			_variables = new ProfileList(eProfileType.ModelVariable);
			if (aKey != Include.NoRID)
			{
				LoadProfile(aKey);
			}
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ForecastModelProfile(string aValue)
			: base(Include.NoRID)
		{
			_variables = new ProfileList(eProfileType.ModelVariable);
			LoadProfile(aValue);
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
				return eProfileType.ForecastingModel;
			}
		}

		/// <summary>
		/// Gets or sets the whether this model is the default model.
		/// </summary>
		public bool IsDefault 
		{
			get { return _isDefault ; }
			set { _isDefault = value; }
		}

		/// <summary>
		/// Gets or sets the computation mode for the model.
		/// </summary>
		public string ComputationMode 
		{
			get { return _computationMode ; }
			set {_computationMode = value; }
		}
		/// <summary>
		/// Gets or sets the list of variables that are to be processed by this model.
		/// </summary>
		public ProfileList Variables 
		{
			get { return _variables ; }
			set {_variables = value; }
		}

		//===========
		// METHODS
		//===========

		public void LoadProfile(int aKey)
		{
			try
			{
				if (_modelsData == null) _modelsData = new ModelsData();
				System.Data.DataTable dt = _modelsData.ForecastModel_Read(this.Key);
				if (dt.Rows.Count < 1)
				{
					return;
				}
				System.Data.DataRow dr = dt.Rows[0];
				this.ModelID = Convert.ToString(dr["FORECAST_MOD_ID"], CultureInfo.CurrentUICulture);

				if (Convert.ToInt32(dr["DEFAULT_IND"], CultureInfo.CurrentUICulture) == 1)
					IsDefault = true;
				else
					IsDefault = false;

				this.ComputationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);
				LoadVariables();
			}
			catch
			{
				throw;
			}
		}

		public void LoadProfile(string aValue)
		{
			try
			{
				if (_modelsData == null) _modelsData = new ModelsData();
				System.Data.DataTable dt = _modelsData.ForecastModel_Read(aValue);
				if (dt.Rows.Count < 1)
				{
					this.Key = Include.NoRID;
					return;
				}
				System.Data.DataRow dr = dt.Rows[0];
				this.Key = Convert.ToInt32(dr["FORECAST_MOD_RID"], CultureInfo.CurrentUICulture);
				this.ModelID = Convert.ToString(dr["FORECAST_MOD_ID"], CultureInfo.CurrentUICulture);
				this.IsDefault = Include.ConvertCharToBool(Convert.ToChar(Convert.ToInt32(dr["DEFAULT_IND"], CultureInfo.CurrentUICulture),CultureInfo.CurrentUICulture) );
				this.ComputationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);
				LoadVariables();
			}
			catch
			{
				throw;
			}
		}

		private void LoadVariables()
		{
			try
			{
				DataTable dt = _modelsData.ForecastModelVariable_Read(Key);
				if (dt.Rows.Count < 1)
				{
					return;
				}
				foreach (DataRow dr in dt.Rows)
				{
					ModelVariableProfile mvp = new ModelVariableProfile(Convert.ToInt32(dr["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture));
					mvp.IsSelected = true;
					mvp.VariableProfile = null;

					// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
					if (dr["FORECAST_MOD_RID"] == DBNull.Value)
						mvp.ForcastModelRID = Include.NoRID;
					else
					    mvp.ForcastModelRID = Convert.ToInt32(dr["FORECAST_MOD_RID"], CultureInfo.CurrentUICulture);

					if (dr["VARIABLE_SEQUENCE"] == DBNull.Value)
						mvp.VariableSEQ = Include.NoRID;
					else
						mvp.VariableSEQ = Convert.ToInt32(dr["VARIABLE_SEQUENCE"], CultureInfo.CurrentUICulture);

					if (dr["FORECAST_FORMULA"] == DBNull.Value)
						mvp.ForecastFormula = Include.NoRID;
					else
						mvp.ForecastFormula = Convert.ToInt32(dr["FORECAST_FORMULA"], CultureInfo.CurrentUICulture);

					if (dr["ASSOC_VARIABLE"] == DBNull.Value)
						mvp.AssocVariable = Include.NoRID;
					else
						mvp.AssocVariable = Convert.ToInt32(dr["ASSOC_VARIABLE"], CultureInfo.CurrentUICulture);

					mvp.GradeWOSIDX = dr["GRADE_WOS_IDX"].Equals("1");
					mvp.StockModifier = dr["STOCK_MODIFIER"].Equals("1");
					mvp.FWOSOverride = dr["FWOS_OVERRIDE"].Equals("1");
					mvp.StockMin = dr["STOCK_MIN"].Equals("1");
					mvp.StockMax = dr["STOCK_MAX"].Equals("1");
					mvp.MinPlusSales = dr["MIN_PLUS_SALES"].Equals("1");
					mvp.SalesModifier = dr["SALES_MODIFIER"].Equals("1");
					// END MID Track #5773
					// begin MID Track #6187
					mvp.UsePlan = dr["USE_PLAN"].Equals("1");
					// END MID Track #6187
					// begin MID Track #6271 stodd
					mvp.AllowChainNegatives = dr["ALLOW_CHAIN_NEGATIVES"].Equals("1");
					// END MID Track #6271
					Variables.Add(mvp);
				}
			}
			catch
			{
				throw;
			}
		}


		public int WriteProfile()
		{
			try
			{
				if (_modelsData == null) _modelsData = new ModelsData();
				
				_modelsData.OpenUpdateConnection();
				if (ModelChangeType == eChangeType.add)
				{
					// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
					Key = _modelsData.ForecastModel_Add(ModelID, this.IsDefault, ComputationMode);
					int count = 0;
					foreach (ModelVariableProfile mvp in Variables)
					{
						_modelsData.ForecastModelVariable_Add(Key, 
							                                  count,
							                                  mvp.Key,
			                                                  mvp.ForecastFormula,
			                                                  mvp.AssocVariable,
			                                                  mvp.GradeWOSIDX,
			                                                  mvp.StockModifier,
			                                                  mvp.FWOSOverride,
			                                                  mvp.StockMin, 
			                                                  mvp.StockMax, 
			                                                  mvp.MinPlusSales, 
			                                                  mvp.SalesModifier,
															  mvp.UsePlan,		// Track #6187
															  mvp.AllowChainNegatives);		// Track #6271
						++count;
					}
					// END MID Track #5773
				}
				else if (ModelChangeType == eChangeType.update)
				{
					// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
					_modelsData.ForecastModel_Update(Key, ModelID, this.IsDefault, ComputationMode);
					_modelsData.ForecastModelVariable_Delete(Key);
					int count = 0;
					foreach (ModelVariableProfile mvp in Variables)
					{
						_modelsData.ForecastModelVariable_Add(Key, 
							                                  count,
 							                                  mvp.Key,
															  mvp.ForecastFormula,
															  mvp.AssocVariable,
															  mvp.GradeWOSIDX,
															  mvp.StockModifier,
															  mvp.FWOSOverride,
															  mvp.StockMin, 
															  mvp.StockMax, 
															  mvp.MinPlusSales, 
															  mvp.SalesModifier,
															  mvp.UsePlan,	// track 6187
															  mvp.AllowChainNegatives);		// Track #6271
						++count;
					}
					// END MID Track #5773
				}
				else if (ModelChangeType == eChangeType.delete)
				{
					_modelsData.ForecastModel_Delete(Key);
				}
				_modelsData.CommitData();
				return Key;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (_modelsData != null &&
					_modelsData.ConnectionIsOpen)
				{
					_modelsData.CloseUpdateConnection();
				}
			}
		}
	}

	/// <summary>
	/// Used to retrieve a list of forecasting model profiles
	/// </summary>
	[Serializable()]
	public class ForecastModelProfileList : ProfileList
	{
		//=======
		// FIELDS
		//=======
		private MIDRetail.Data.ModelsData _modelsData;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ForecastModelProfileList(bool aLoadProfiles)
			: base(eProfileType.ForecastingModel)
		{
			try
			{
				if (aLoadProfiles)
				{
					LoadAll();
				}
			}
			catch
			{
				throw;
			}
		}

		//===========
		// METHODS
		//===========

		public void LoadAll()
		{
			try
			{
				ForecastModelProfile fmp;
				if (_modelsData == null) _modelsData = new ModelsData();

				System.Data.DataTable dt = _modelsData.ForecastModel_Read();
				foreach(System.Data.DataRow dr in dt.Rows)
				{
					fmp = new ForecastModelProfile(Convert.ToInt32(dr["FORECAST_MOD_RID"], CultureInfo.CurrentUICulture));
					this.Add(fmp);
				}
			}
			catch
			{
				throw;
			}
		}
	}


	/// <summary>
	/// Used to retrieve and update information about a variable referenced in a model
	/// </summary>
	[Serializable()]
	public class ModelVariableProfile : Profile
	{
		private VariableProfile	_variableProfile;
		private bool _isSelected;
		// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
		private int _forcastModelRID;
		private int _variableSEQ;
		private int _forecastFormula;
		private int _assocVariable;
		private bool _gradeWOSIDX;
		private bool _stockModifier;
		private bool _fwosOverride;
		private bool _stockMin; 
		private bool _stockMax; 
		private bool _minPlusSales; 
		private bool _salesModifier;
		// END MID Track #5773
		// BEGIN MID Track #6187
		private bool _usePlan;
		// ENd MID Track #6187
		// BEGIN MID Track #6271
		private bool _allowChainNegatives;
		// END MID Track #6271

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public ModelVariableProfile(int aKey)
			: base(aKey)
		{
			_isSelected = false;
			// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
			_forcastModelRID = Include.NoRID;
			_variableSEQ = Include.NoRID;
			_forecastFormula = Include.NoRID;
			_assocVariable = Include.NoRID;
			// Begin Track #6187
			_gradeWOSIDX = true;
			_stockModifier = true;
			_fwosOverride = true;
			_stockMin = true;
			_stockMax = true;
			_minPlusSales = true;
			_salesModifier = true;
			// END MID Track #6187
			// END MID Track #5773
			// BEGIN MID Track #6187
			_usePlan = false;
			// ENd MID Track #6187
			// Begin Track #6271 stodd
			_allowChainNegatives = false;
			// End Track #6271
		}

		// BEGIN Issue 4962 stodd 11.27.2007
		public ModelVariableProfile(VariableProfile variableProfile)
			: base(variableProfile.Key)
		{
			_variableProfile = variableProfile;
			_isSelected = false;
			// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
			_forcastModelRID = Include.NoRID;
			_variableSEQ = Include.NoRID;
			_forecastFormula = Include.NoRID;
			_assocVariable = Include.NoRID;
			// Begin Track #6187 stodd
			_gradeWOSIDX = true;
			_stockModifier = true;
			_fwosOverride = true;
			_stockMin = true;
			_stockMax = true;
			_minPlusSales = true;
			_salesModifier = true;
			// END MID Track #6187
			// END MID Track #5773
			// BEGIN MID Track #6187
			_usePlan = false;
			// ENd MID Track #6187
			// Begin Track #6271 stodd
			_allowChainNegatives = false;
			// End Track #6271
		}
		// BEGIN Issue 4962

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ModelVariable;
			}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the variable is selected for the model.
		/// </summary>
		public bool IsSelected 
		{
			get { return _isSelected ; }
			set { _isSelected = value; }
		}

		// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
		/// <summary>
		/// Gets or sets ForcastModelRID.
		/// </summary>
		public int ForcastModelRID
		{
			get { return _forcastModelRID ; }
			set { _forcastModelRID = value; }
		}

		/// <summary>
		/// Gets or sets VariableSEQ.
		/// </summary>
		public int VariableSEQ
		{
			get { return _variableSEQ ; }
			set { _variableSEQ = value; }
		}

		/// <summary>
		/// Gets or sets ForecastFormula.
		/// </summary>
		public int ForecastFormula 
		{
			get { return _forecastFormula ; }
			set { _forecastFormula = value; }
		}

		/// <summary>
		/// Gets or sets AssocSalesVar.
		/// </summary>
		public int AssocVariable 
		{
			get { return _assocVariable ; }
			set { _assocVariable = value; }
		}

		/// <summary>
		/// Gets or sets GradeWOSIDX.
		/// </summary>
		public bool GradeWOSIDX 
		{
			get { return _gradeWOSIDX ; }
			set { _gradeWOSIDX = value; }
		}

		/// <summary>
		/// Gets or sets StockModifier.
		/// </summary>
		public bool StockModifier 
		{
			get { return _stockModifier ; }
			set { _stockModifier = value; }
		}

		/// <summary>
		/// Gets or sets FWOSOverride.
		/// </summary>
		public bool FWOSOverride 
		{
			get { return _fwosOverride ; }
			set { _fwosOverride = value; }
		}

		/// <summary>
		/// Gets or sets StockMin.
		/// </summary>
		public bool StockMin 
		{
			get { return _stockMin ; }
			set { _stockMin = value; }
		}

		/// <summary>
		/// Gets or sets StockMax.
		/// </summary>
		public bool StockMax 
		{
			get { return _stockMax ; }
			set { _stockMax = value; }
		}

		/// <summary>
		/// Gets or sets MinPlusSales.
		/// </summary>
		public bool MinPlusSales 
		{
			get { return _minPlusSales ; }
			set { _minPlusSales = value; }
		}

		/// <summary>
		/// Gets or sets SalesModifier.
		/// </summary>
		public bool SalesModifier 
		{
			get { return _salesModifier ; }
			set { _salesModifier = value; }
		}
		// END MID Track #5773

		/// <summary>
		/// Gets or sets the VariableProfile of the variable.
		/// </summary>
		public VariableProfile VariableProfile 
		{
			get { return _variableProfile ; }
			set { _variableProfile = value; }
		}

		// END MID Track #6187
		/// <summary>
		/// Gets or sets SalesModifier.
		/// </summary>
		public bool UsePlan
		{
			get { return _usePlan; }
			set { _usePlan = value; }
		}
		// END MID Track #6187
		// Begin MID Track #6271
		/// <summary>
		/// Gets or sets SalesModifier.
		/// </summary>
		public bool AllowChainNegatives
		{
			get { return _allowChainNegatives; }
			set { _allowChainNegatives = value; }
		}
		// END MID Track #6271
	}

    /// <summary>
    /// Used to retrieve and update information about an override low level model
    /// </summary>
    [Serializable()]
    public class OverrideLowLevelProfile : ModelProfile
    {
        //=============
        // FIELDS
        //=============
        private string _name;
        private int _hn_RID;
        private int _high_level_hn_RID;
        private int _highLevelSeq;
        private int _highLevelOffset;
        private eHighLevelsType _highLevelType;
        private int _lowLevelSeq;
        private int _lowLevelOffset;
        private eLowLevelsType _lowLevelType;
        private int _method_RID;
        private int _user_RID;
        private MIDRetail.Data.ModelsData _modelsData;
        private ProfileList _detailList = new ProfileList(eProfileType.OverrideLowLevelDetail);
        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        private bool _activeOnlyInd;
        // End TT#988

        //=============
        // CONSTRUCTORS
        //=============
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public OverrideLowLevelProfile(int aKey)
            : base(aKey)
        {
            _hn_RID = Include.NoRID;
            _high_level_hn_RID =Include.NoRID;
            _highLevelSeq = 0;
            _highLevelOffset = 0;
            _highLevelType = eHighLevelsType.None;
            _lowLevelSeq = 0;
            _lowLevelOffset = 0;
            _lowLevelType = eLowLevelsType.None;
            _method_RID = Include.NoRID;
            _user_RID = Include.NoRID;
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            _activeOnlyInd = false;
            // End TT#988

            if (aKey != Include.NoRID)
            {
                LoadProfile(aKey);
            }
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
                return eProfileType.OverrideLowLevelModel;
            }
        }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets the high level Sequence for the model.
        /// </summary>
        public int HighLevelSeq
        {
            get { return _highLevelSeq; }
            set { _highLevelSeq = value; }
        }
        /// <summary>
        /// Gets or sets the high level Offset for the model.
        /// </summary>
        public int HighLevelOffset
        {
            get { return _highLevelOffset; }
            set { _highLevelOffset = value; }
        }
        /// <summary>
        /// Gets or sets the high level Type for the model.
        /// </summary>
        public eHighLevelsType HighLevelType
        {
            get { return _highLevelType; }
            set { _highLevelType = value; }
        }
        /// <summary>
        /// Gets or sets the low level for the model.
        /// </summary>
        public int LowLevelSeq
        {
            get { return _lowLevelSeq; }
            set { _lowLevelSeq = value; }
        }
        /// <summary>
        /// Gets or sets the low level Offset for the model.
        /// </summary>
        public int LowLevelOffset
        {
            get { return _lowLevelOffset; }
            set { _lowLevelOffset = value; }
        }
        /// <summary>
        /// Gets or sets the low level Type for the model.
        /// </summary>
        public eLowLevelsType LowLevelType
        {
            get { return _lowLevelType; }
            set { _lowLevelType = value; }
        }
        /// <summary>
        /// Gets or sets the hierachy node for the model.
        /// </summary>
        public int HN_RID
        {
            get { return _hn_RID; }
            set { _hn_RID = value; }
        }
        /// <summary>
        /// Gets or sets the high level hierachy node for the model.
        /// </summary>
        public int High_Level_HN_RID
        {
            get { return _high_level_hn_RID; }
            set { _high_level_hn_RID = value; }
        }
        /// <summary>
        /// Gets or sets the specific method for this model.  A value of NoRID indicates a global model.
        /// </summary>
        public int Method_RID
        {
            get { return _method_RID; }
            set { _method_RID = value; }
        }

        /// <summary>
        /// Gets or sets the specific user ID for this model.
        /// </summary>
        public int User_RID
        {
            get { return _user_RID; }
            set { _user_RID = value; }
        }

        // Begin TT#1168 - JSmith - Performance
        //public ProfileList DetailList
        //{
        //    get { return _detailList; }
        //    set { _detailList = value; }
        //}
        public ProfileList DetailList
        {
            get 
            {
                if (_detailList == null)
                {
                    _detailList = LoadDetailProfiles(Key);
                }

                return _detailList; 
            }
            set { _detailList = value; }
        }
        // End TT#1168

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        /// <summary>
        /// Gets or sets the flag that identifies if only active nodes are to be used.
        /// </summary>
        public bool ActiveOnlyInd
        {
            get { return _activeOnlyInd; }
            set { _activeOnlyInd = value; }
        }
        // End TT#988

        //===========
        // METHODS
        //===========

        public void LoadProfile(int aKey)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                System.Data.DataTable dt = _modelsData.OverrideLowLevelsModel_Read(this.Key);
                if (dt.Rows.Count < 1)
                    return;

                System.Data.DataRow dr = dt.Rows[0];
                this.Name = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                this.HN_RID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                this.High_Level_HN_RID = Convert.ToInt32(dr["HIGH_LEVEL_HN_RID"], CultureInfo.CurrentUICulture);
                this.User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
                this.LowLevelSeq = Convert.ToInt32(dr["LOW_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                this.LowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                this.LowLevelType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                this.HighLevelSeq = Convert.ToInt32(dr["HIGH_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                this.HighLevelOffset = Convert.ToInt32(dr["HIGH_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                this.HighLevelType = (eHighLevelsType)Convert.ToInt32(dr["HIGH_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                DetailList = LoadDetailProfiles(aKey);
                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                if (dr["ACTIVE_ONLY_IND"] == DBNull.Value)
                {
                    this.ActiveOnlyInd = false;
                }
                else
                {
                    this.ActiveOnlyInd = Include.ConvertCharToBool(Convert.ToChar(dr["ACTIVE_ONLY_IND"], CultureInfo.CurrentUICulture));
                }
                // End TT#988
            }
            catch
            {
                throw;
            }
        }

        public void LoadProfileWork(int aKey)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                System.Data.DataTable dt = _modelsData.OverrideLowLevelsModelWork_Read(this.Key);
                if (dt.Rows.Count < 1)
                    return;

                System.Data.DataRow dr = dt.Rows[0];
                this.Name = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                this.HN_RID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                this.High_Level_HN_RID = Convert.ToInt32(dr["HIGH_LEVEL_HN_RID"], CultureInfo.CurrentUICulture);
                this.User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
                this.LowLevelSeq = Convert.ToInt32(dr["LOW_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                this.LowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                this.LowLevelType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                this.HighLevelSeq = Convert.ToInt32(dr["HIGH_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                this.HighLevelOffset = Convert.ToInt32(dr["HIGH_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                this.HighLevelType = (eHighLevelsType)Convert.ToInt32(dr["HIGH_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                this.DetailList = LoadDetailProfilesWork(this.Key);
                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                if (dr["ACTIVE_ONLY_IND"] == DBNull.Value)
                {
                    this.ActiveOnlyInd = false;
                }
                else
                {
                    this.ActiveOnlyInd = Include.ConvertCharToBool(Convert.ToChar(dr["ACTIVE_ONLY_IND"], CultureInfo.CurrentUICulture));
                }
                // End TT#988

            }
            catch
            {
                throw;
            }
        }
        public static ProfileList LoadAllProfiles(int aOllRID, int aUserRID)
        {
            try
            {
				ProfileList returnList = LoadAllProfiles(aOllRID, aUserRID, true, true, Include.NoRID);

                return returnList;
            }
            catch
            {
                throw;
            }
        }

        public static ProfileList LoadAllProfiles(int aOllRID, int aUserRID, bool globalAllowView, bool userAllowView, int customOllRID)
		{
			try
			{
				ModelsData modelsData = new ModelsData();
				DataTable dt;
                dt = modelsData.OverrideLowLevelsModel_ReadAll(aOllRID, aUserRID, globalAllowView, userAllowView, customOllRID);

				ProfileList returnList = new ProfileList(eProfileType.OverrideLowLevelModel);

                //-----Look For Custom First And Put It At The Top Of The List-----
				foreach (DataRow dr in dt.Rows)
				{
                    if (Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture) == Include.CustomUserRID) 
                    {
                        OverrideLowLevelProfile ollp = new OverrideLowLevelProfile(Include.NoRID);
                        ollp.Key = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                        //ollp.Key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                        ollp.Name = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                        ollp.HN_RID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                        ollp.High_Level_HN_RID = Convert.ToInt32(dr["HIGH_LEVEL_HN_RID"], CultureInfo.CurrentUICulture);
                        ollp.User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
                        ollp.LowLevelSeq = Convert.ToInt32(dr["LOW_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                        ollp.LowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        ollp.LowLevelType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        ollp.HighLevelSeq = Convert.ToInt32(dr["HIGH_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                        ollp.HighLevelOffset = Convert.ToInt32(dr["HIGH_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        ollp.HighLevelType = (eHighLevelsType)Convert.ToInt32(dr["HIGH_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        // Begin TT#1168 - JSmith - Performance
                        //ollp._detailList = LoadDetailProfiles(ollp.Key);
                        ollp.DetailList = null;
                        // End TT#1168
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        if (dr["ACTIVE_ONLY_IND"] == DBNull.Value)
                        {
                            ollp.ActiveOnlyInd = false;
                        }
                        else
                        {
                            ollp.ActiveOnlyInd = Include.ConvertCharToBool(Convert.ToChar(dr["ACTIVE_ONLY_IND"], CultureInfo.CurrentUICulture));
                        }
                        // End TT#988
                        returnList.Add(ollp);
                    } 
				}

                //-----Look For User & Global Records And Put Them After The Custom Record-----
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture) != Include.CustomUserRID)
                    {
                        OverrideLowLevelProfile ollp = new OverrideLowLevelProfile(Include.NoRID);
                        ollp.Key = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                        //ollp.Key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                        ollp.Name = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                        ollp.HN_RID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                        ollp.High_Level_HN_RID = Convert.ToInt32(dr["HIGH_LEVEL_HN_RID"], CultureInfo.CurrentUICulture);
                        ollp.User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
                        ollp.LowLevelSeq = Convert.ToInt32(dr["LOW_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                        ollp.LowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        ollp.LowLevelType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        ollp.HighLevelSeq = Convert.ToInt32(dr["HIGH_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
                        ollp.HighLevelOffset = Convert.ToInt32(dr["HIGH_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
                        ollp.HighLevelType = (eHighLevelsType)Convert.ToInt32(dr["HIGH_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
                        // Begin TT#1168 - JSmith - Performance
                        //ollp._detailList = LoadDetailProfiles(ollp.Key);
                        ollp.DetailList = null;
                        // End TT#1168
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        if (dr["ACTIVE_ONLY_IND"] == DBNull.Value)
                        {
                            ollp.ActiveOnlyInd = false;
                        }
                        else
                        {
                            ollp.ActiveOnlyInd = Include.ConvertCharToBool(Convert.ToChar(dr["ACTIVE_ONLY_IND"], CultureInfo.CurrentUICulture));
                        }
                        // End TT#988
                        returnList.Add(ollp);
                    }
                }

				return returnList;
			}
			catch
			{
				throw;
			}
		}

        private static ProfileList LoadDetailProfiles(int aModelRID)
        {
            try
            {
                ModelsData modelsData = new ModelsData();
                DataTable dt = modelsData.OverrideLowLevelsDetail_Read(aModelRID);

                ProfileList returnList = new ProfileList(eProfileType.OverrideLowLevelDetail);

                foreach (DataRow dr in dt.Rows)
                {
                    OverrideLowLevelDetailProfile olldp = new OverrideLowLevelDetailProfile(Include.NoRID);
                  //olldp.Key = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                    olldp.Key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                    olldp.Model_RID = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                    if (dr["VERSION_RID"] != System.DBNull.Value)
                    {
                        olldp.Version_RID = Convert.ToInt32(dr["VERSION_RID"], CultureInfo.CurrentUICulture);
                    }
                    olldp.Exclude_Ind = Include.ConvertCharToBool(Convert.ToChar(dr["EXCLUDE_IND"], CultureInfo.CurrentUICulture));
                    returnList.Add(olldp);
                }

                return returnList;
            }
            catch
            {
                throw;
            }
        }

        private static ProfileList LoadDetailProfilesWork(int aModelRID)
        {
            try
            {
                ModelsData modelsData = new ModelsData();
                DataTable dt = modelsData.OverrideLowLevelsDetailWork_Read(aModelRID);

                ProfileList returnList = new ProfileList(eProfileType.OverrideLowLevelDetail);

                foreach (DataRow dr in dt.Rows)
                {
                    OverrideLowLevelDetailProfile olldp = new OverrideLowLevelDetailProfile(Include.NoRID);
                    //olldp.Key = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                    olldp.Key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                    olldp.Model_RID = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                    if (dr["VERSION_RID"] != System.DBNull.Value)
                    {
                        olldp.Version_RID = Convert.ToInt32(dr["VERSION_RID"], CultureInfo.CurrentUICulture);
                    }
                    olldp.Exclude_Ind = Include.ConvertCharToBool(Convert.ToChar(dr["EXCLUDE_IND"], CultureInfo.CurrentUICulture));
                    returnList.Add(olldp);
                }

                return returnList;
            }
            catch
            {
                throw;
            }
        }

        public OverrideLowLevelProfile Copy()
        {
            OverrideLowLevelProfile ollp = new OverrideLowLevelProfile(_key);

            ollp._name = this._name;
            ollp._hn_RID = this._hn_RID;
            ollp._high_level_hn_RID = this._high_level_hn_RID;
            ollp._highLevelSeq = this._highLevelSeq;
            ollp._highLevelOffset = this._highLevelOffset;
            ollp._highLevelType = this._highLevelType;
            ollp._lowLevelSeq = this._lowLevelSeq;
            ollp._lowLevelOffset = this._lowLevelOffset;
            ollp._lowLevelType = this._lowLevelType;
            ollp._method_RID = this._method_RID;
            ollp._user_RID = this._user_RID;
            ollp._modelsData = this._modelsData;
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            ollp._activeOnlyInd = this._activeOnlyInd;
            // End TT#988

            // Begin TT#1168 - JSmith - Performance
            //if (_detailList != null)
            //{
            //    ollp._detailList = new ProfileList(eProfileType.OverrideLowLevelDetail);
            //    foreach (OverrideLowLevelDetailProfile ollpd in this._detailList)
            //    {
            //        OverrideLowLevelDetailProfile ollpdClone = ollpd.Copy();
            //        ollp._detailList.Add(ollpdClone);
            //    }
            //}
            if (DetailList != null)
            {
                ollp.DetailList = new ProfileList(eProfileType.OverrideLowLevelDetail);
                foreach (OverrideLowLevelDetailProfile ollpd in this.DetailList)
                {
                    OverrideLowLevelDetailProfile ollpdClone = ollpd.Copy();
                    ollp.DetailList.Add(ollpdClone);
                }
            }
            // End TT#1168
            return ollp;
        }

        // Begin TT#1126 - JSmith - Cannot have override low level models with the same name for global and user
        //public bool ModelNameExists(string ModelName)
        //{
        //    ModelsData modelsData = new ModelsData();
        //    return modelsData.OverrideLowLevelsModelName_Exist(ModelName);
        //}
        public bool ModelNameExists(string ModelName, int aUserRID)
        {
            ModelsData modelsData = new ModelsData();
            return modelsData.OverrideLowLevelsModelName_Exist(ModelName, aUserRID);
        }
        // End TT#1126

        public bool CustomExists(int ModelRID)
        {
            ModelsData modelsData = new ModelsData();
            return modelsData.OverrideLowLevelsModel_IsCustom(ModelRID);
        }

        public bool CustomExistsWork(int ModelRID)
        {
            ModelsData modelsData = new ModelsData();
            return modelsData.OverrideLowLevelsModelWork_IsCustom(ModelRID);
        }

        public int DeleteModel(int ModelRID)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if (ModelRID != Include.NoRID)
                {
                    //--Look To See If We Have A Custom OLLRID--------
                    if (User_RID == Include.CustomUserRID)
                    {
                        _modelsData.OverrideLowLevelsDetail_DeleteCustomOLLRidForiegnKeys(Key);
                        _modelsData.CommitData();
                    }
                    _modelsData.OverrideLowLevelsDetail_Delete(ModelRID);
                    _modelsData.OverrideLowLevelsModel_Delete(ModelRID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int DeleteModelWork(int ModelRID)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if (ModelRID != Include.NoRID)
                {
                    //--Look To See If We Have A Custom OLLRID--------
					// BEGIN TT#696 - Overrid Low Level Model going blank on Close.
					//if (User_RID == Include.CustomUserRID)
					//{
					//    _modelsData.OverrideLowLevelsDetail_DeleteCustomOLLRidForiegnKeys(Key);
					//    _modelsData.CommitData();
					//}
					// End TT#696 - Overrid Low Level Model going blank on Close.

                    _modelsData.OverrideLowLevelsDetailWork_Delete(ModelRID);
                    _modelsData.OverrideLowLevelsModelWork_Delete(ModelRID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int DeleteModelDetails(int ModelRID, int HN_RID)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if ((ModelRID != Include.NoRID) && (HN_RID != Include.NoRID))
                {
                    _modelsData.OverrideLowLevelsDetail_Delete(ModelRID, HN_RID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int DeleteModelDetailsWork(int ModelRID, int HN_RID)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if ((ModelRID != Include.NoRID) && (HN_RID != Include.NoRID))
                {
                    _modelsData.OverrideLowLevelsDetailWork_Delete(ModelRID, HN_RID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int DeleteModelDetails(int ModelRID)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if (ModelRID != Include.NoRID)
                {
                    _modelsData.OverrideLowLevelsDetail_Delete(ModelRID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int DeleteModelDetailsWork(int ModelRID)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if (ModelRID != Include.NoRID)
                {
                    _modelsData.OverrideLowLevelsDetailWork_Delete(ModelRID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int WriteProfile(ref int last_custom_user_RID, int clientUserRid)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();

                //==========================================
                // Determine whether to INSERT or UPDATE
                //==========================================
                if (ModelChangeType == eChangeType.update)
                {
                    foreach (OverrideLowLevelDetailProfile olldp in DetailList)
                    {
                        olldp.ModelChangeType = _modelsData.OverrideLowLevelsDetail_Action(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    }
                }

                _modelsData.OpenUpdateConnection();
                if (ModelChangeType == eChangeType.add)
                {
                    //---Look For Key In The Database-----------
                    bool foundIt = false;
                    ProfileList overrideProfiles = LoadAllProfiles(last_custom_user_RID, User_RID);
                    foreach (OverrideLowLevelProfile ollp in overrideProfiles) 
                    {
                        if (ollp.Key == Key || ollp.Key == last_custom_user_RID) 
                        {
                            foundIt = true;
                            break;
                        }
                    }

                    if (Key == Include.NoRID || !foundIt)
                    {
                        //-----Create Blank Record---------------------------
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        //Key = _modelsData.OverrideLowLevelsModel_Add(Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                        Key = _modelsData.OverrideLowLevelsModel_Add(Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, ActiveOnlyInd);
                        // End TT#988

                        //--Look To See If We Have A Custom OLLRID--------
                        if (User_RID == Include.CustomUserRID)
                        {
                            if ((last_custom_user_RID != Include.NoRID) && (last_custom_user_RID != Key))
                            {
                                _modelsData.OverrideLowLevelsDetail_Delete(last_custom_user_RID);
                                _modelsData.OverrideLowLevelsModel_Delete(last_custom_user_RID);
                            }
                            last_custom_user_RID = Key;
                        }
                    }
                    else
                    {
                        //--Look To See If We Have A Custom OLLRID--------
                        if (User_RID == Include.CustomUserRID)
                        {
                            if ((last_custom_user_RID != Include.NoRID) && (last_custom_user_RID != Key))
                            {
                                _modelsData.OverrideLowLevelsDetail_Delete(last_custom_user_RID);
                                _modelsData.OverrideLowLevelsModel_Delete(last_custom_user_RID);
                            }
                            last_custom_user_RID = Key;
                        }

                        //-----Update Record---------------------------
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        //_modelsData.OverrideLowLevelsModel_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                        _modelsData.OverrideLowLevelsModel_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, ActiveOnlyInd);
                        // End TT#988
                    }

                    _modelsData.OverrideLowLevelsDetail_Delete(Key);
                    // Begin TT#1168 - JSmith - Performance
                    //foreach(OverrideLowLevelDetailProfile olldp in _detailList)
                    //    _modelsData.OverrideLowLevelsDetail_Add(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    foreach (OverrideLowLevelDetailProfile olldp in DetailList)
                        _modelsData.OverrideLowLevelsDetail_Add(eChangeType.add, Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    // End TT#1168
                }
                else if (ModelChangeType == eChangeType.update)
                {
                    //---Update Master (Header) Record---------------------
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    //_modelsData.OverrideLowLevelsModel_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                    _modelsData.OverrideLowLevelsModel_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, ActiveOnlyInd);
                    // End TT#988

                    //---Add Records That Are Changed---
                    // Begin TT#1168 - JSmith - Performance
                    //foreach (OverrideLowLevelDetailProfile olldp in _detailList)
                    //{
                    //    _modelsData.OverrideLowLevelsDetail_Add(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    //}
                    foreach (OverrideLowLevelDetailProfile olldp in DetailList)
                    {
                        _modelsData.OverrideLowLevelsDetail_Add(olldp.ModelChangeType, Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    }
                    // End TT#1168
                }
                else if (ModelChangeType == eChangeType.delete)
                {
                    //--Look To See If We Have A Custom OLLRID--------
                    if (User_RID == Include.CustomUserRID) 
                    {
                        _modelsData.OverrideLowLevelsDetail_DeleteCustomOLLRidForiegnKeys(Key);
                        _modelsData.CommitData();
                    }
                    _modelsData.OverrideLowLevelsDetail_Delete(Key);
                    _modelsData.OverrideLowLevelsModel_Delete(Key);

                    if (User_RID == Include.CustomUserRID)
                    {
                        last_custom_user_RID = Include.NoRID;
                    }
                }
                _modelsData.CommitData();
                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int WriteProfileWork(ref int last_custom_user_RID, int clientUserRid)
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();

                _modelsData.OpenUpdateConnection();
                if (ModelChangeType == eChangeType.add)
                {
                    ////---Look For Key In The Database-----------
                    //bool foundIt = false;
                    //ProfileList overrideProfiles = LoadAllProfiles(last_custom_user_RID, User_RID);
                    //foreach (OverrideLowLevelProfile ollp in overrideProfiles)
                    //{
                    //    if (ollp.Key == Key || ollp.Key == last_custom_user_RID)
                    //    {
                    //        foundIt = true;
                    //        break;
                    //    }
                    //}

                    //if (Key == Include.NoRID || !foundIt)
                    //{

                        //-----Create Blank Record---------------------------
                    //  Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Mod
                        //Key = _modelsData.OverrideLowLevelsModelWork_Add(Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, clientUserRid);
                        Key = _modelsData.OverrideLowLevelsModelWork_Add(Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, clientUserRid, ActiveOnlyInd);
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Mod

                    //    //--Look To See If We Have A Custom OLLRID--------
                    //    if (User_RID == Include.CustomUserRID)
                    //    {
                    //        if ((last_custom_user_RID != Include.NoRID) && (last_custom_user_RID != Key))
                    //        {
                    //            _modelsData.OverrideLowLevelsDetailWork_Delete(last_custom_user_RID);
                    //            _modelsData.OverrideLowLevelsModelWork_Delete(last_custom_user_RID);
                    //        }
                    //        last_custom_user_RID = Key;
                    //    }
                    //}
                    //else
                    //{
                    //    //--Look To See If We Have A Custom OLLRID--------
                    //    if (User_RID == Include.CustomUserRID)
                    //    {
                    //        if ((last_custom_user_RID != Include.NoRID) && (last_custom_user_RID != Key))
                    //        {
                    //            _modelsData.OverrideLowLevelsDetailWork_Delete(last_custom_user_RID);
                    //            _modelsData.OverrideLowLevelsModelWork_Delete(last_custom_user_RID);
                    //        }
                    //        last_custom_user_RID = Key;
                    //    }

                    //    //-----Update Record---------------------------
                    //    _modelsData.OverrideLowLevelsModelWork_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                    //}

                    _modelsData.OverrideLowLevelsDetailWork_Delete(Key);
                    // Begin TT#1168 - JSmith - Performance
                    //foreach (OverrideLowLevelDetailProfile olldp in _detailList)
                    //    _modelsData.OverrideLowLevelsDetailWork_Add(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    foreach (OverrideLowLevelDetailProfile olldp in DetailList)
                        _modelsData.OverrideLowLevelsDetailWork_Add(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    // End TT#1168
                }
                else if (ModelChangeType == eChangeType.update)
                {
                    //---Update Master (Header) Record---------------------
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    //_modelsData.OverrideLowLevelsModelWork_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType);
                    _modelsData.OverrideLowLevelsModelWork_Update(Key, Name, HN_RID, High_Level_HN_RID, User_RID, HighLevelSeq, HighLevelOffset, HighLevelType, LowLevelSeq, LowLevelOffset, LowLevelType, ActiveOnlyInd);
                    // End TT#988

                    //---Add Records That Are Changed---
                    // Begin TT#1168 - JSmith - Performance
                    //foreach (OverrideLowLevelDetailProfile olldp in _detailList)
                    //{
                    //    _modelsData.OverrideLowLevelsDetailWork_Add(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    //}
                    foreach (OverrideLowLevelDetailProfile olldp in DetailList)
                    {
                        _modelsData.OverrideLowLevelsDetailWork_Add(Key, olldp.Key, olldp.Version_RID, olldp.Exclude_Ind);
                    }
                    // End TT#1168
                }
                else if (ModelChangeType == eChangeType.delete)
                {
                    //--Look To See If We Have A Custom OLLRID--------
                    _modelsData.OverrideLowLevelsDetailWork_Delete(Key);
                    _modelsData.OverrideLowLevelsModelWork_Delete(Key);

                    if (User_RID == Include.CustomUserRID)
                    {
                        last_custom_user_RID = Include.NoRID;
                    }
                }
                _modelsData.CommitData();
                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }

        public int CopyToPermanentTable(int ModelRID)
        {
			try
			{
                int newKey = Include.NoRID;
				if (_modelsData == null) _modelsData = new ModelsData();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if (ModelRID != Include.NoRID)
                {
                    _modelsData.OpenUpdateConnection();
                    newKey = _modelsData.OverrideLowLevelsModelWork_CopyToPermanentTable(ModelRID);
                    _modelsData.CommitData();
                }
                //End Track #5921

                return newKey;
			}
			catch (Exception)
			{
				
				throw;
			}
			finally
			{
				if (_modelsData != null &&
					_modelsData.ConnectionIsOpen)
				{
					_modelsData.CloseUpdateConnection();
				}
			}
        }

        public void CopyToWorkTables(int ModelRID)
        {
			try
			{
                if (_modelsData == null) _modelsData = new ModelsData();
                _modelsData.OpenUpdateConnection();

                //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                if (ModelRID != Include.NoRID)
                {
                    _modelsData.OverrideLowLevelsDetailWork_Delete(ModelRID);
                    _modelsData.OverrideLowLevelsModelWork_Delete(ModelRID);
                    _modelsData.CommitData();
                    _modelsData.OverrideLowLevelsModel_CopyToWorkTables(ModelRID);
                    _modelsData.CommitData();
                }
                //End Track #5921
            }
			catch (Exception)
			{

				throw;
			}
			finally
			{
				if (_modelsData != null &&
					_modelsData.ConnectionIsOpen)
				{
					_modelsData.CloseUpdateConnection();
				}
			}
        }

    }

    /// <summary>
    /// Used to retrieve and update information about an override low level model
    /// </summary>
    [Serializable()]
    public class OverrideLowLevelDetailProfile : ModelProfile
    {
        //=============
        // FIELDS
        //=============
        private int _model_RID;
        private int _version_RID;
        private bool _exclude_Ind;
        private MIDRetail.Data.ModelsData _modelsData;

        //=============
        // CONSTRUCTORS
        //=============
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public OverrideLowLevelDetailProfile(int aKey)
            : base(aKey)
        {
            _version_RID = Include.NoRID;
            _exclude_Ind = false;
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
                return eProfileType.OverrideLowLevelDetail;
            }
        }

        /// <summary>
        /// Gets or sets the hierachy node for the detail.
        /// </summary>
        public int Model_RID
        {
            get { return _model_RID; }
            set { _model_RID = value; }
        }
        /// <summary>
        /// Gets or sets the version for this detail.
        /// </summary>
        public int Version_RID
        {
            get { return _version_RID; }
            set { _version_RID = value; }
        }

        /// <summary>
        /// Gets or sets the exclude for this detail.
        /// </summary>
        public bool Exclude_Ind
        {
            get { return _exclude_Ind; }
            set { _exclude_Ind = value; }
        }

        //===========
        // METHODS
        //===========

        public static ProfileList LoadAllProfiles(int aModelRID)
        {
            try
            {
                ModelsData modelsData = new ModelsData();
                DataTable dt = modelsData.OverrideLowLevelsDetail_Read(aModelRID);

                ProfileList returnList = new ProfileList(eProfileType.OverrideLowLevelDetail);

                foreach (DataRow dr in dt.Rows)
                {
                    OverrideLowLevelDetailProfile ollp = new OverrideLowLevelDetailProfile(Include.NoRID);
                    ollp.Model_RID = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                    ollp.Key = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                    if (dr["VERSION_RID"] != System.DBNull.Value)
                    {
                        ollp.Version_RID = Convert.ToInt32(dr["VERSION_RID"]);
                    }
                    if (dr["EXCLUDE_IND"] != System.DBNull.Value)
                    {
                        ollp.Exclude_Ind = Include.ConvertCharToBool(Convert.ToChar(dr["EXCLUDE_IND"], CultureInfo.CurrentUICulture));
                    }
                    returnList.Add(ollp);
                }

                return returnList;
            }
            catch
            {
                throw;
            }
        }

        public OverrideLowLevelDetailProfile Copy()
        {
            OverrideLowLevelDetailProfile copy = new OverrideLowLevelDetailProfile(this._key);
            copy._exclude_Ind = this._exclude_Ind;
            copy._model_RID = this._model_RID;
            copy._modelsData = this._modelsData;
            copy._version_RID = this._version_RID;
            return copy;
        }

        public int WriteProfile()
        {
            try
            {
                if (_modelsData == null) _modelsData = new ModelsData();

                _modelsData.OpenUpdateConnection();
                if (ModelChangeType == eChangeType.add)
                    _modelsData.OverrideLowLevelsDetailWork_Add(Model_RID, Key, Version_RID, Exclude_Ind);

                _modelsData.CommitData();
                return Key;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_modelsData != null &&
                    _modelsData.ConnectionIsOpen)
                {
                    _modelsData.CloseUpdateConnection();
                }
            }
        }
    }
}
