using System;

namespace MIDRetail.DataCommon
{
	[Serializable]
	abstract public class MethodOverride
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of MethodOverride.
		/// </summary>
		public MethodOverride()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Summary description for ForecastingOverride.
	/// </summary>
	[Serializable]
	abstract public class OTSPlanOverride : MethodOverride
	{
		//=======
		// FIELDS
		//=======
		private int		_hierarchyNodeRid;
		private int		_forecastVersionRid;
		private string	_computationMode;
		private int		_variableNumber;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ForecastingOverride.
		/// </summary>
		public OTSPlanOverride()
		{
			_hierarchyNodeRid = Include.NoRID;
			_forecastVersionRid = Include.NoRID;
			_computationMode = null;
			_variableNumber = 0;
		}
		//Begin TT#155 - JScott - Size Curve Method
		public OTSPlanOverride(int aNodeRID)
		{
			_hierarchyNodeRid = aNodeRID;
			_forecastVersionRid = Include.NoRID;
			_computationMode = null;
			_variableNumber = 0;
		}
		//End TT#155 - JScott - Size Curve Method
		public OTSPlanOverride(int aNodeRID, int aVersionRID)
		{
			_hierarchyNodeRid = aNodeRID;
			_forecastVersionRid = aVersionRID;
			_computationMode = null;
			_variableNumber = 0;
		}
		public OTSPlanOverride(int aNodeRID, int aVersionRID, string aComputationMode)
		{
			_hierarchyNodeRid = aNodeRID;
			_forecastVersionRid = aVersionRID;
			_computationMode = aComputationMode;
			_variableNumber = 0;
		}
		public OTSPlanOverride(int aNodeRID, int aVersionRID, string aComputationMode, int aVariableNumber)
		{
			_hierarchyNodeRid = aNodeRID;
			_forecastVersionRid = aVersionRID;
			_computationMode = aComputationMode;
			_variableNumber = aVariableNumber;
		}
		
		//===========
		// PROPERTIES
		//===========

		public int HierarchyNodeRid
		{
			get	{return _hierarchyNodeRid;}
			set	{_hierarchyNodeRid = value;	}
		}
		public int ForecastVersionRid
		{
			get	{return _forecastVersionRid;}
			set	{_forecastVersionRid = value;	}
		}
		public string ComputationMode
		{
			get	{return _computationMode;}
			set	{_computationMode = value;	}
		}
		public int VariableNumber
		{
			get	{return _variableNumber;}
			set	{_variableNumber = value;	}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Summary description for ForecastingOverride.
	/// </summary>
	[Serializable]
	public class ForecastingOverride : OTSPlanOverride
	{
		//=======
		// FIELDS
		//=======
		private bool	_overrideBalance;	// this flag identifies if the balance override is to be used
		private bool	_balance;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ForecastingOverride.
		/// </summary>
		public ForecastingOverride()
		{
			_overrideBalance = false;
		}
		public ForecastingOverride(int aNodeRID, int aVersionRID) 
			: base (aNodeRID, aVersionRID)
		{
			_overrideBalance = false;
		}
		public ForecastingOverride(int aNodeRID, int aVersionRID, string aComputationMode) 
			: base (aNodeRID, aVersionRID, aComputationMode)
		{
			_overrideBalance = false;
		}
		public ForecastingOverride(int aNodeRID, int aVersionRID, string aComputationMode, int aVariableNumber) 
			: base (aNodeRID, aVersionRID, aComputationMode, aVariableNumber)
		{
			_overrideBalance = false;
		}
		public ForecastingOverride(int aNodeRID, int aVersionRID, string aComputationMode, int aVariableNumber,
			bool aOverrideBalance, bool aBalance) 
			: base (aNodeRID, aVersionRID, aComputationMode, aVariableNumber)
		{
			_overrideBalance = aOverrideBalance;
			_balance = aBalance;
		}

		//===========
		// PROPERTIES
		//===========

		public bool OverrideBalance
		{
			get	{return _overrideBalance;}
			set	{_overrideBalance = value;	}
		}
		public bool Balance
		{
			get	{return _balance;}
			set	{_balance = value;	}
		}

		public bool HasOverrides()
		{
			bool hasOverrides = false;

			if (base.ForecastVersionRid != Include.NoRID)
				hasOverrides = true;
			else if (base.HierarchyNodeRid != Include.NoRID)
				hasOverrides = true;
			else if (base.VariableNumber != Include.NoRID)
				hasOverrides = true;

			return hasOverrides;
		}

		//========
		// METHODS
		//========
	}
	//Begin TT#155 - JScott - Size Curve Method

	/// <summary>
	/// Summary description for SizeCurveGenOverride.
	/// </summary>
	[Serializable]
	public class SizeCurveGenOverride : OTSPlanOverride
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SizeCurveGenOverride.
		/// </summary>
		public SizeCurveGenOverride()
		{
		}
		public SizeCurveGenOverride(int aNodeRID)
			: base(aNodeRID)
		{
		}

		//===========
		// PROPERTIES
		//===========

		public bool HasOverrides()
		{
			bool hasOverrides = false;

			if (base.HierarchyNodeRid != Include.NoRID)
				hasOverrides = true;

			return hasOverrides;
		}

		//========
		// METHODS
		//========
	}
	//End TT#155 - JScott - Size Curve Method
}
