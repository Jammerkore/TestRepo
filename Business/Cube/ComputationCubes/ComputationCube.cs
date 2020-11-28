using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public delegate bool InitConditionDelegate(ComputationCellReference aCompCellRef);

	public class InitConditionOperand
	{
		//=======
		// FIELDS
		//=======

		InitConditionDelegate _initConditionDelegate;
		eConjunctionType _conjunctionType;

		//=============
		// CONSTRUCTORS
		//=============

		public InitConditionOperand(InitConditionDelegate aInitConditionDelegate)
		{
			_initConditionDelegate = aInitConditionDelegate;
			_conjunctionType = eConjunctionType.None;
		}

		public InitConditionOperand(InitConditionDelegate aInitConditionDelegate, eConjunctionType aConjunctionType)
		{
			_initConditionDelegate = aInitConditionDelegate;
			_conjunctionType = aConjunctionType;
		}

		//===========
		// PROPERTIES
		//===========

		public InitConditionDelegate InitConditionDelegate
		{
			get
			{
				return _initConditionDelegate;
			}
		}

		public eConjunctionType ConjunctionType
		{
			get
			{
				return _conjunctionType;
			}
		}

		//========
		// METHODS
		//========
	}

	public class InitCondition
	{
		//=======
		// FIELDS
		//=======

		InitConditionOperand[] _initConditionOperands;
		FormulaProfile _trueFormula;
		FormulaProfile _falseFormula;

		//=============
		// CONSTRUCTORS
		//=============

		public InitCondition(FormulaProfile aTrueFormula, FormulaProfile aFalseFormula, InitConditionOperand aInitConditionOperand)
		{
			_trueFormula = aTrueFormula;
			_falseFormula = aFalseFormula;
			_initConditionOperands = new InitConditionOperand[1];
			_initConditionOperands[0] = aInitConditionOperand;
		}

		public InitCondition(FormulaProfile aTrueFormula, FormulaProfile aFalseFormula, params InitConditionOperand[] aInitConditionOperands)
		{
			_trueFormula = aTrueFormula;
			_falseFormula = aFalseFormula;
			_initConditionOperands = aInitConditionOperands;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		public FormulaProfile Evaluate(ComputationCellReference aCompCellRef)
		{
			try
			{
				if (intCheckCondition(_initConditionOperands, 0, aCompCellRef))
				{
					return _trueFormula;
				}
				else
				{
					return _falseFormula;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool intCheckCondition(InitConditionOperand[] aInitConditionOperands, int aConditionIdx, ComputationCellReference aCompCellRef)
		{
			InitConditionOperand initConditionOperand;

			try
			{
				initConditionOperand = aInitConditionOperands[aConditionIdx];

				if (aConditionIdx < aInitConditionOperands.Length - 1)
				{
					switch (initConditionOperand.ConjunctionType)
					{
						case eConjunctionType.And:
							return initConditionOperand.InitConditionDelegate(aCompCellRef) && intCheckCondition(aInitConditionOperands, aConditionIdx + 1, aCompCellRef);

						case eConjunctionType.Or:
							return initConditionOperand.InitConditionDelegate(aCompCellRef) || intCheckCondition(aInitConditionOperands, aConditionIdx + 1, aCompCellRef);

						default:
							return initConditionOperand.InitConditionDelegate(aCompCellRef);
					}
				}
				else
				{
					return initConditionOperand.InitConditionDelegate(aCompCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The CompRuleTableEntry class contains the initialization and change rule delegates for a given Variable and QuantityVariable combination.
	/// </summary>

	public class CompRuleTableEntry
	{
		//=======
		// FIELDS
		//=======

		private int _variableKey;
		private int _quantityVariableKey;
		private InitCondition _initCondition;
		private FormulaProfile _initFormulaProfile;
		private ChangeMethodProfile _primaryChangeMethodProfile;
		private ChangeMethodProfile _secondaryChangeMethodProfile;
		private ChangeMethodProfile _autototalChangeMethodProfile;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CompRuleTableEntry using the given Variable key and QuantityVariable key.  The rule delegates are initialized.
		/// </summary>
		/// <param name="aVariableKey">
		/// The key to the Variable Profile object.
		/// </param>
		/// <param name="aQuantityVariableKey">
		/// The key to the QuantityVariable Profile object.
		/// </param>

		public CompRuleTableEntry(int aVariableKey, int aQuantityVariableKey)
		{
			_variableKey = aVariableKey;
			_quantityVariableKey = aQuantityVariableKey;
			_initCondition = null;
			_initFormulaProfile = null;
			_primaryChangeMethodProfile = null;
			_secondaryChangeMethodProfile = null;
			_autototalChangeMethodProfile = null;
		}

		/// <summary>
		/// Creates a new instance of CompRuleTableEntry using the given Variable key and QuantityVariable key.  The rule delegates are initialized to
		/// the given InitFormulaProfile, primary ChangeMethodProfile, secondary ChangeMethodProfile, and autototal ChangeMethodProfile.
		/// </summary>
		/// <param name="aVariableKey">
		/// The key to the Variable Profile object.
		/// </param>
		/// <param name="aQuantityVariableKey">
		/// The key to the QuantityVariable Profile object.
		/// </param>
		/// <param name="aInitCondition">
		/// The InitCondition pointing to the condition object.
		/// </param>
		/// <param name="aInitFormulaProfile">
		/// The FormulaSpreadProfile delegate pointing to the initialization routine.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile pointing to the primary change routine.
		/// </param>
		/// <param name="aSecondaryChangeMethodProfile">
		/// The ChangeMethodProfile pointing to the secondary change routine.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile pointing to the autototal change routine.
		/// </param>

		public CompRuleTableEntry(
			int aVariableKey,
			int aQuantityVariableKey,
			InitCondition aInitCondition,
			FormulaProfile aInitFormulaProfile,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aSecondaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			_variableKey = aVariableKey;
			_quantityVariableKey = aQuantityVariableKey;
			_initCondition = aInitCondition;
			_initFormulaProfile = aInitFormulaProfile;
			_primaryChangeMethodProfile = aPrimaryChangeMethodProfile;
			_secondaryChangeMethodProfile = aSecondaryChangeMethodProfile;
			_autototalChangeMethodProfile = aAutototalChangeMethodProfile;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the FormulaProfile for the init formula.
		/// </summary>

		public FormulaProfile InitFormulaProfile
		{
			get
			{
				return _initFormulaProfile;
			}
			//Begin BonTon Calculations - JScott - Apply Changes to 20.94 version of Computation Document - Part 3
			set
			{
				_initFormulaProfile = value;
				_initCondition = null;
			}
			//End BonTon Calculations - JScott - Apply Changes to 20.94 version of Computation Document - Part 3
		}

		//Begin BonTon Calculations - JScott - Implement Phase 3
		public InitCondition InitCondition
		{
			get
			{
				return _initCondition;
			}
			set
			{
				_initCondition = value;
				_initFormulaProfile = null;
			}
		}

		//End BonTon Calculations - JScott - Implement Phase 3
		/// <summary>
		/// Gets the ChangeMethodProfile for the primary change rule.
		/// </summary>

		public ChangeMethodProfile PrimaryChangeMethodProfile
		{
			get
			{
				return _primaryChangeMethodProfile;
			}
			//Begin BonTon Calculations - JScott - Apply Changes to 20.94 version of Computation Document - Part 3
			set
			{
				_primaryChangeMethodProfile = value;
			}
			//End BonTon Calculations - JScott - Apply Changes to 20.94 version of Computation Document - Part 3
		}

		/// <summary>
		/// Gets the ChangeMethodProfile for the secondary change rule.
		/// </summary>

		public ChangeMethodProfile SecondaryChangeMethodProfile
		{
			get
			{
				return _secondaryChangeMethodProfile;
			}
		}

		/// <summary>
		/// Gets the ChangeMethodProfile for the autototal change rule.
		/// </summary>

		public ChangeMethodProfile AutototalChangeMethodProfile
		{
			get
			{
				return _autototalChangeMethodProfile;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override. Gets the hash code for this object.  The hash code is a bit combination of the Variable and QuantityVariable keys.
		/// </summary>
		/// <returns>
		/// The hash code for this object.
		/// </returns>

		override public int GetHashCode()
		{
			return ((_variableKey & 0xFF) << 16) | (_quantityVariableKey & 0xFF);
		}

		/// <summary>
		/// Override.  Returns a boolean indicating if the given object is equal to the current object.
		/// </summary>
		/// <param name="obj">
		/// The object to compare to the current object.
		/// </param>
		/// <returns>
		/// A boolean indicating whether the two objects are equal.
		/// </returns>

		override public bool Equals(object obj)
		{
			try
			{
				return _variableKey == ((CompRuleTableEntry)obj)._variableKey &&
					_quantityVariableKey == ((CompRuleTableEntry)obj)._quantityVariableKey;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the FormulaSpreadProfile delegate.
		/// </summary>

		public FormulaProfile GetInitFormulaProfile(ComputationCellReference aCompCellRef)
		{
			try
			{
				if (_initCondition != null)
				{
					return _initCondition.Evaluate(aCompCellRef);
				}
				else
				{
					return _initFormulaProfile;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The ComputationCube is an UndoCube that provides computation functionality.
	/// </summary>

	abstract public class ComputationCube : Cube
	{
		//=======
		// FIELDS
		//=======

		private MRSCalendar _calendar;
		private ushort _cubeAttributes;
		private int _cubePriority;
		private bool _readOnly;
		private bool _checkNodeSecurity;
		private ComputationExtensionCube _extCube;
		private System.Collections.Hashtable _ruleTable;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComputationCube, using the given SessionAddressBlock, Transaction, ComputationCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this ComputationCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this ComputationCube is a part of.
		/// </param>
		/// <param name="aCompCubeGroup">
		/// A reference to a ComputationCubeGroup that this ComputationCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this ComputationCube.
		/// </param>

		public ComputationCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, ComputationCubeGroup aCompCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aCompCubeGroup, aCubeDefinition)
		{
			try
			{
				_calendar = aSAB.ApplicationServerSession.Calendar;
				_cubeAttributes = aCubeAttributes;
				_cubePriority = aCubePriority;
				_readOnly = aReadOnly;
				_checkNodeSecurity = aCheckNodeSecurity;
				_extCube = new ComputationExtensionCube(aSAB, aTransaction, aCompCubeGroup, aCubeDefinition);
				_ruleTable = new System.Collections.Hashtable();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Abstract property returns the eProfileType for the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		abstract public eProfileType VariableProfileType { get; }

		/// <summary>
		/// Abstract property returns the eProfileType for the QuantityVariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		abstract public eProfileType QuantityVariableProfileType { get; }

		/// <summary>
		/// Abstract property returns the ProfileList for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		abstract public ProfileList QuantityVariableProfileList { get; }

		/// <summary>
		/// Gets a reference to the MRSCalendar.
		/// </summary>

		public MRSCalendar Calendar
		{
			get
			{
				return _calendar;
			}
		}

		/// <summary>
		/// Gets a reference to the PlanCubeGroup.
		/// </summary>

		public ComputationCubeGroup ComputationCubeGroup
		{
			get
			{
				return (ComputationCubeGroup)_cubeGroup;
			}
		}

		/// <summary>
		/// Gets the Cube priority of this ComputationCube.
		/// </summary>

		public int CubeAttributes
		{
			get
			{
				return _cubeAttributes;
			}
		}

		/// <summary>
		/// Gets the Cube priority of this ComputationCube.
		/// </summary>

		public int CubePriority
		{
			get
			{
				return _cubePriority;
			}
		}

		/// <summary>
		/// Gets the Read Only flag of this ComputationCube.
		/// </summary>

		public bool ReadOnly
		{
			get
			{
				return _readOnly;
			}
		}

		/// <summary>
		/// Gets the Read Only flag of this ComputationCube.
		/// </summary>

		public bool CheckNodeSecurity
		{
			get
			{
				return _checkNodeSecurity;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Group Total type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Group Total type cube.
		/// </returns>

		public bool isGroupTotalCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & CubeAttributesFlagValues.GroupTotal) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Store Total type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Store Total type cube.
		/// </returns>

		public bool isStoreTotalCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & CubeAttributesFlagValues.StoreTotal) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database.
		/// </summary>
		/// <param name="aVarProf">
		/// The VariableProfile containing the variable to inspect
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		abstract public bool isDatabaseVariable(ComputationVariableProfile aVarProf, ComputationCellReference aPlanCellRef);

		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given ComputationCellReference is displayable.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		abstract public bool isCellDisplayable(ComputationCellReference aCompCellRef);

		//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns a boolean indicating if the cell referenced can be scheduled for calculation.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		abstract public bool canCellBeScheduled(ComputationCellReference aCompCellRef);

		//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Abstract method that returns the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		abstract public ComputationVariableProfile GetVariableProfile(ComputationCellReference aCompCellRef);

		/// <summary>
		/// Returns a string describing the given ComputationCellReference
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A string describing the ComputationCellReference.
		/// </returns>

		virtual public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			return "No Cell Description Available";
		}

		/// <summary>
		/// Allows a cube to specify custom initializations for a Cell.  Occurs after the standard Cell initialization.
		/// </summary>
        /// <param name="aCompCellRef">
		/// The PlanCellReference to initialize.
		/// </param>

		virtual public void InitCellValue(ComputationCellReference aCompCellRef)
		{
		}

		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given ComputationCellReference is valid to display.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is valid to display.
		/// </returns>

		virtual public bool isCellValid(ComputationCellReference aCompCellRef)
		{
			ComputationVariableProfile varProf;
			ComputationVariableProfile quantityVarProf;

			try
			{
				varProf = GetVariableProfile(aCompCellRef);

				if (varProf != null)
				{
					//Begin Track #6011 - JScott - Displaying % Chg on Variables that should not display % Chg
					//if (!isDatabaseVariable(varProf, aCompCellRef))
					//{
					//    quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);

					//    if (_ruleTable[new CompRuleTableEntry(varProf.Key, quantityVarProf.Key)] == null)
					//    {
					//        return false;
					//    }
					//}
					quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);

					if (!isDatabaseVariable(varProf, aCompCellRef) ||
						quantityVarProf.Key != CubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
					{
						if (_ruleTable[new CompRuleTableEntry(varProf.Key, quantityVarProf.Key)] == null)
						{
							return false;
						}
					}
					//End Track #6011 - JScott - Displaying % Chg on Variables that should not display % Chg
				}

				if (aCompCellRef.isCellNull)
				{
					return false;
				}

				if (aCompCellRef.isCellHidden)
				{
					return false;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a CellCoordinates are created.
		/// </summary>
		/// <returns>
		/// A reference to a new CellCoordinates object.
		/// </returns>

		override public CellCoordinates CreateCellCoordinates(int aNumIndices)
		{
			try
			{
				return new ComputationCellCoordinates(aNumIndices);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the CompRuleTableEntry for the given Variable and QuantityVariable keys.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference that points to the cell to look up the rule for.
		/// </param>
		/// <returns>
		/// The CompRuleTableEntry object of the given keys.
		/// </returns>

		public CompRuleTableEntry GetRuleTableEntry(ComputationCellReference aCompCellRef)
		{
			ComputationVariableProfile varProf;
			ComputationVariableProfile quantityVarProf;

			try
			{
				varProf = GetVariableProfile(aCompCellRef);
				if (varProf != null)
				{
					quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);
					return (CompRuleTableEntry)_ruleTable[new CompRuleTableEntry(varProf.Key, quantityVarProf.Key)];
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the VariableProfile that contains the format type for the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to find the format type for.
		/// </param>
		/// <returns>
		/// The VariableProfile that contains the format type.
		/// </returns>

		public ComputationVariableProfile GetFormatTypeVariableProfile(ComputationCellReference aCompCellRef)
		{
			ComputationVariableProfile varProf;
			ComputationVariableProfile quantityVarProf;

			try
			{
				varProf = GetVariableProfile(aCompCellRef);
				if (varProf != null)
				{
					//quantityVarProf = GetQuantityVariableProfile(aCompCellRef);
					quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);
					if (quantityVarProf.FormatType != eValueFormatType.None)
					{
						return quantityVarProf;
					}
					else
					{
						return varProf;
					}
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the VariableProfile that contains the variable scope for the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to find the variable scope for.
		/// </param>
		/// <returns>
		/// The VariableProfile that contains the variable scope.
		/// </returns>

		public ComputationVariableProfile GetVariableScopeVariableProfile(ComputationCellReference aCompCellRef)
		{
			ComputationVariableProfile varProf;
			ComputationVariableProfile quantityVarProf;

			try
			{
				varProf = GetVariableProfile(aCompCellRef);
				if (varProf != null)
				{
					quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);
					if (quantityVarProf.VariableScope != eVariableScope.Overridden)
					{
						return quantityVarProf;
					}
					else
					{
						return varProf;
					}
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the VariableProfile that contains the variable style for the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to find the variable style for.
		/// </param>
		/// <returns>
		/// The VariableProfile that contains the variable style.
		/// </returns>

		public ComputationVariableProfile GetVariableStyleVariableProfile(ComputationCellReference aCompCellRef)
		{
			ComputationVariableProfile varProf;
			ComputationVariableProfile quantityVarProf;

			try
			{
				varProf = GetVariableProfile(aCompCellRef);
				if (varProf != null)
				{
					quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);
					if (quantityVarProf.VariableStyle != eVariableStyle.Overridden)
					{
						return quantityVarProf;
					}
					else
					{
						return varProf;
					}
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the VariableProfile that contains the variable access for the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to find the variable access for.
		/// </param>
		/// <returns>
		/// The VariableProfile that contains the variable access.
		/// </returns>

		public ComputationVariableProfile GetVariableAccessVariableProfile(ComputationCellReference aCompCellRef)
		{
			ComputationVariableProfile varProf;
			ComputationVariableProfile quantityVarProf;

			try
			{
				varProf = GetVariableProfile(aCompCellRef);
				if (varProf != null)
				{
					quantityVarProf = (QuantityVariableProfile)QuantityVariableProfileList.FindKey(aCompCellRef[QuantityVariableProfileType]);
					if (quantityVarProf.VariableAccess != eVariableAccess.Overridden)
					{
						return quantityVarProf;
					}
					else
					{
						return varProf;
					}
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Add Initialization and Changes Rule to the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the primary change method.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile that points to the autototal change method.
		/// </param>

		public void AddRule(
			ComputationVariableProfile aVariable,
			ComputationVariableProfile aQuantityVariable,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			try
			{
				AddRule(aVariable, aQuantityVariable, null, null, aPrimaryChangeMethodProfile, null, aAutototalChangeMethodProfile);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Add Initialization and Changes Rule to the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aInitFormulaProfile">
		/// The FormulaSpreadProfile that points to the initialization method.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the primary change method.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile that points to the autototal change method.
		/// </param>

		public void AddRule(
			ComputationVariableProfile aVariable,
			ComputationVariableProfile aQuantityVariable,
			FormulaProfile aInitFormulaProfile,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			try
			{
				AddRule(aVariable, aQuantityVariable, null, aInitFormulaProfile, aPrimaryChangeMethodProfile, null, aAutototalChangeMethodProfile);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Add Initialization and Changes Rule to the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aInitCondition">
		/// The InitCondition that points to the condition object.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the primary change method.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile that points to the autototal change method.
		/// </param>

		public void AddRule(
			ComputationVariableProfile aVariable,
			ComputationVariableProfile aQuantityVariable,
			InitCondition aInitCondition,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			try
			{
				AddRule(aVariable, aQuantityVariable, aInitCondition, null, aPrimaryChangeMethodProfile, null, aAutototalChangeMethodProfile);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Add Initialization and Changes Rule to the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aInitFormulaProfile">
		/// The FormulaSpreadProfile that points to the initialization method.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the primary change method.
		/// </param>
		/// <param name="aSecondaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the secondary change method.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile that points to the autototal change method.
		/// </param>

		public void AddRule(
			ComputationVariableProfile aVariable,
			ComputationVariableProfile aQuantityVariable,
			FormulaProfile aInitFormulaProfile,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aSecondaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			try
			{
				AddRule(aVariable, aQuantityVariable, null, aInitFormulaProfile, aPrimaryChangeMethodProfile, aSecondaryChangeMethodProfile, aAutototalChangeMethodProfile);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Add Initialization and Changes Rule to the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aInitCondition">
		/// The InitCondition that points to the condition object.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the primary change method.
		/// </param>
		/// <param name="aSecondaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the secondary change method.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile that points to the autototal change method.
		/// </param>

		public void AddRule(
			ComputationVariableProfile aVariable,
			ComputationVariableProfile aQuantityVariable,
			InitCondition aInitCondition,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aSecondaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			try
			{
				AddRule(aVariable, aQuantityVariable, aInitCondition, null, aPrimaryChangeMethodProfile, aSecondaryChangeMethodProfile, aAutototalChangeMethodProfile);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Add Initialization and Changes Rule to the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aInitCondition">
		/// The InitCondition that points to the condition object.
		/// </param>
		/// <param name="aPrimaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the primary change method.
		/// </param>
		/// <param name="aSecondaryChangeMethodProfile">
		/// The ChangeMethodProfile that points to the secondary change method.
		/// </param>
		/// <param name="aAutototalChangeMethodProfile">
		/// The ChangeMethodProfile that points to the autototal change method.
		/// </param>

		private void AddRule(
			ComputationVariableProfile aVariable,
			ComputationVariableProfile aQuantityVariable,
			InitCondition aInitCondition,
			FormulaProfile aInitFormulaProfile,
			ChangeMethodProfile aPrimaryChangeMethodProfile,
			ChangeMethodProfile aSecondaryChangeMethodProfile,
			ChangeMethodProfile aAutototalChangeMethodProfile)
		{
			CompRuleTableEntry ruleEntry;

			try
			{
				ruleEntry = new CompRuleTableEntry(
					aVariable.Key,
					aQuantityVariable.Key,
					aInitCondition,
					aInitFormulaProfile,
					aPrimaryChangeMethodProfile,
					aSecondaryChangeMethodProfile,
					aAutototalChangeMethodProfile);

				_ruleTable[ruleEntry] = ruleEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Removes Initialization and Changes Rule from the RuleTable.
		/// </summary>
		/// <param name="aVariable">
		/// The VariableProfile of the variable that the initialization rule is to be assigned to.
		/// </param>
		/// <param name="aQuantityVariable">
		/// The VariableProfile of the quantity variable that the initialization rule is to be assigned to.
		/// </param>

		public void RemoveRule(ComputationVariableProfile aVariable, ComputationVariableProfile aQuantityVariable)
		{
			try
			{
				_ruleTable.Remove(new CompRuleTableEntry(aVariable.Key, aQuantityVariable.Key));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin BonTon Calculations - JScott - Apply Changes to 20.94 version of Computation Document - Part 3
		public void OverrideInitRule(ComputationVariableProfile aVariable, ComputationVariableProfile aQuantityVariable, FormulaProfile aInitFormulaProfile)
		{
			CompRuleTableEntry ruleEntry;

			try
			{
				ruleEntry = (CompRuleTableEntry)_ruleTable[new CompRuleTableEntry(aVariable.Key, aQuantityVariable.Key)];

				if (ruleEntry == null)
				{
					AddRule(aVariable, aQuantityVariable, null, aInitFormulaProfile, null, null, null);
				}
				else
				{
					ruleEntry.InitFormulaProfile = aInitFormulaProfile;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin BonTon Calculations - JScott - Implement Phase 3
		public void OverrideInitRule(ComputationVariableProfile aVariable, ComputationVariableProfile aQuantityVariable, InitCondition aInitCondition)
		{
			CompRuleTableEntry ruleEntry;

			try
			{
				ruleEntry = (CompRuleTableEntry)_ruleTable[new CompRuleTableEntry(aVariable.Key, aQuantityVariable.Key)];

				if (ruleEntry == null)
				{
					AddRule(aVariable, aQuantityVariable, aInitCondition, null, null, null, null);
				}
				else
				{
					ruleEntry.InitCondition = aInitCondition;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End BonTon Calculations - JScott - Implement Phase 3
		public void OverridePrimaryChangeRule(ComputationVariableProfile aVariable, ComputationVariableProfile aQuantityVariable, ChangeMethodProfile aPrimaryChangeMethodProfile)
		{
			CompRuleTableEntry ruleEntry;

			try
			{
				ruleEntry = (CompRuleTableEntry)_ruleTable[new CompRuleTableEntry(aVariable.Key, aQuantityVariable.Key)];

				if (ruleEntry == null)
				{
					AddRule(aVariable, aQuantityVariable, null, null, aPrimaryChangeMethodProfile, null, null);
				}
				else
				{
					ruleEntry.PrimaryChangeMethodProfile = aPrimaryChangeMethodProfile;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End BonTon Calculations - JScott - Apply Changes to 20.94 version of Computation Document - Part 3

        //Begin BonTon Calculations - JScott - Implement Phase 3
        public void RemoveInitRule(ComputationVariableProfile aVariable, ComputationVariableProfile aQuantityVariable)
        {
            CompRuleTableEntry ruleEntry;

            try
            {
                ruleEntry = (CompRuleTableEntry)_ruleTable[new CompRuleTableEntry(aVariable.Key, aQuantityVariable.Key)];

                if (ruleEntry != null)
                {
                    ruleEntry.InitCondition = null;
                    ruleEntry.InitFormulaProfile = null;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void RemovePrimaryChangeRule(ComputationVariableProfile aVariable, ComputationVariableProfile aQuantityVariable)
        {
            CompRuleTableEntry ruleEntry;

            try
            {
                ruleEntry = (CompRuleTableEntry)_ruleTable[new CompRuleTableEntry(aVariable.Key, aQuantityVariable.Key)];

                if (ruleEntry != null)
                {
                    ruleEntry.PrimaryChangeMethodProfile = null;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //End BonTon Calculations - JScott - Implement Phase 3

		/// <summary>
		/// Returns the ChangeMethodProfile for the primary change rule for the cell identified by the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference object that identifies the cell.
		/// </param>
		/// <returns>
		/// The ChangeMethodProfile for the primary change rule for the cell.
		/// </returns>

		public ChangeMethodProfile GetPrimaryChangeMethodProfile(ComputationCellReference aCompCellRef)
		{
			CompRuleTableEntry compRule;

			try
			{
				compRule = GetRuleTableEntry(aCompCellRef);
				if (compRule != null)
				{
					return compRule.PrimaryChangeMethodProfile;
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the ChangeMethodProfile for the secondary change rule for the cell identified by the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference object that identifies the cell.
		/// </param>
		/// <returns>
		/// The ChangeMethodProfile for the secondary change rule for the cell.
		/// </returns>

		public ChangeMethodProfile GetSecondaryChangeMethodProfile(ComputationCellReference aCompCellRef)
		{
			CompRuleTableEntry compRule;

			try
			{
				compRule = GetRuleTableEntry(aCompCellRef);
				if (compRule != null)
				{
					return compRule.SecondaryChangeMethodProfile;
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the ChangeMethodProfile for the autototal change rule for the cell identified by the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference object that identifies the cell.
		/// </param>
		/// <returns>
		/// The ChangeMethodProfile for the autototal change rule for the cell.
		/// </returns>

		public ChangeMethodProfile GetAutototalChangeMethodProfile(ComputationCellReference aCompCellRef)
		{
			CompRuleTableEntry compRule;

			try
			{
				compRule = GetRuleTableEntry(aCompCellRef);
				if (compRule != null)
				{
					return compRule.AutototalChangeMethodProfile;
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the FormulaSpreadProfile for ComputationCubeCell identified by the given ComputationCellReference.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference object that identifies the ComputationCubeCell.
		/// </param>
		/// <returns>
		/// The FormulaSpreadProfile for the ComputationCubeCell.
		/// </returns>

		public FormulaProfile GetInitFormulaProfile(ComputationCellReference aCompCellRef)
		{
			CompRuleTableEntry compRule;

			try
			{
				compRule = GetRuleTableEntry(aCompCellRef);
				if (compRule != null)
				{
					return compRule.GetInitFormulaProfile(aCompCellRef);
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method creates a ExtensionCellReference object in the ExtensionCube.
		/// </summary>
        /// <param name="aCompCellRef"></param>
		/// The ComputationCellReference object to convert from.
		/// <returns></returns>

		public bool doesExtensionCellExist(ComputationCellReference aCompCellRef)
		{
			try
			{
				return _extCube.doesCellExist(aCompCellRef.CellCoordinates);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method creates a ExtensionCellReference object in the ExtensionCube.
		/// </summary>
        /// <param name="aCompCellRef"></param>
		/// The ComputationCellReference object to convert from.
		/// <returns></returns>

		public ExtensionCell GetExtensionCell(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (ExtensionCell)_extCube.GetCell(aCompCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the changes in the Cells after a save.
		/// </summary>

		public void ClearChanges()
		{
			try
			{
				RecurseCubeExisting(new RecurseCubeArguments(new RecurseCallbackDelegate(ClearCellChanges), null));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method is called from the RecurseCubeExisting method in the base Cube.  This method clears the changes in the Cells after a save.
		/// </summary>
		/// <param name="aCompCellRef">
		/// A reference to a CellReference object that identifies the Cell's position in the Cube.
		/// </param>
		/// <param name="aCallbackArguments">
		/// An object that contains arguments passed to the RecurseCubeExisting method that were intended for the Callback routine.  In this case, it is null.
		/// </param>

		public void ClearCellChanges(CellReference aCompCellRef, RecurseCubeArguments aCallbackArguments)
		{
			try
			{
				((ComputationCellReference)aCompCellRef).ClearCellChanges();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}