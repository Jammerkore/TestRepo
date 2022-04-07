using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The ComputationsCollection class creates a collection for Computations objects.
	/// </summary>

	public class PlanComputationsCollection : BasePlanComputationsCollection
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public PlanComputationsCollection()
		{
			try
			{
				AddComputation(new DefaultComputations(), true);
			}
			catch (Exception)
			{
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The PlanComputationWorkArea class stores PlanCubeGroup-level work fields.
	/// </summary>

	public class PlanComputationWorkArea
	{
		//=======
		// FIELDS
		//=======

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
	/// The DefaultComputations class creates all the pieces required by the "default" computation process.
	/// </summary>

	public class DefaultComputations : BasePlanComputations
	{
		//=======
		// FIELDS
		//=======

		/// <example>
		/// public class TestException : CustomUserErrorException
		/// {
		/// 	public TestException()
		/// 		 : base(GetUserErrorMessage(1, "Test Error"))
		/// 	{
		/// 	}
		/// }
		/// </example>

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Computations.
		/// </summary>

		public DefaultComputations()
			: base ()
		{
			_basePlanVariables = new PlanVariables();
			_basePlanTimeTotalVariables = new PlanTimeTotalVariables();
			_basePlanQuantityVariables = new PlanQuantityVariables();
			_basePlanFormulasAndSpreads = new DefaultPlanFormulasAndSpreads(this);
			_basePlanChangeMethods = new DefaultPlanChangeMethods(this);
			_basePlanVariableInitialization = new DefaultPlanVariableInitialization(this);
			_basePlanCubeInitialization = new DefaultPlanCubeInitialization(this);
			_basePlanToolBox = new DefaultPlanToolBox(this);

			_basePlanVariables.Initialize(_basePlanTimeTotalVariables);
			_basePlanTimeTotalVariables.Initialize(_basePlanVariables);
			_basePlanQuantityVariables.Initialize();
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the name of this Computations object.
		/// </summary>

		override public string Name
		{
			get
			{
				return "Default";
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The PlanVariables class is where the variables are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the variables are defined.
	/// </remarks>

	public class PlanVariables : BasePlanVariables
	{
		//=======
		// FIELDS
		//=======

		/// <example>
		/// public VariableProfile UserVariable;
		/// </example>

		//=============
		// CONSTRUCTORS
		//=============

		public PlanVariables()
			: base(false)
		{
			/// <example>
			/// UserVariable = new VariableProfile(NextSequence, "User Var", eVariableCategory.Both, eVariableType.Sales, "USER_VAR", eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.Sales, eValueFormatType.GenericNumeric, 0, true, true, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum,0,0,0,0,0);
			/// </example>

            /// <example>
            /// Add variable grouping hierarchy for chooser.  Levels are separated by "|".
            /// _variableGroupings = new ArrayList();
            /// _variableGroupings.Add("Sales|Units");
            /// _variableGroupings.Add("Sales|Dollars");
            /// _variableGroupings.Add("Stock|Units");
            /// _variableGroupings.Add("Stock|Dollars");
            /// _variableGroupings.Add("Other");
            /// </example>
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override protected void InitializeVariables(BasePlanTimeTotalVariables aBasePlanTimeTotalVariables)
		{
			base.InitializeVariables(aBasePlanTimeTotalVariables);

			/// <example>
			/// VariableProfileList.Add(UserVariable);
			/// UserVariable.AddTimeTotalVariable(aBasePlanTimeTotalVariables.UserVariableT1);
			/// UserVariable.TotalTimeTotalVariableProfile = aBasePlanTimeTotalVariables.UserVariableT1;
			/// </example>
		}

		//========
		// CLASSES
		//========
	}

	/// <summary>
	/// The PlanTimeTotalVariables class is where the time-total variables are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the time-total variables are defined.
	/// </remarks>

	public class PlanTimeTotalVariables : BasePlanTimeTotalVariables
	{
		//=======
		// FIELDS
		//=======

		/// <example>
		/// public TimeTotalVariableProfile UserVariableT1;
		/// </example>

		//=============
		// CONSTRUCTORS
		//=============

		public PlanTimeTotalVariables()
			: base()
		{
			/// <example>
			/// UserVariableT1 = new TimeTotalVariableProfile(1, "User Var T1", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
			/// </example>
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		protected override void InitializeVariables(BasePlanVariables aBasePlanVariables)
		{
			base.InitializeVariables(aBasePlanVariables);

			/// <example>
			/// TimeTotalVariableProfileList.Add(UserVariableT1);
			/// UserVariableT1.ParentVariableProfile = aBasePlanVariables.UserVariable;
			/// </example>
		}

		//========
		// CLASSES
		//========
	}

	/// <summary>
	/// The PlanQuantityVariables class is where the quantity variables are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the quantity variables are defined.
	/// </remarks>

	public class PlanQuantityVariables : BasePlanQuantityVariables
	{
		//=======
		// FIELDS
		//=======

		/// <example>
		/// public QuantityVariableProfile UserQuantityVariable;
		/// </example>

		//=============
		// CONSTRUCTORS
		//=============

		public PlanQuantityVariables()
			: base()
		{
			/// <example>
			/// UserQuantityVariable = new QuantityVariableProfile(1, "User Qty Var", eVariableCategory.Both, eVariableStyle.Overridden, eVariableAccess.Overridden, eVariableScope.Overridden, eVariableWeekType.Overridden, QuantityLevelFlagValues.All, QuantityLevelFlagValues.All, false, eValueFormatType.None, 0);
			/// </example>
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		protected override void InitializeVariables()
		{
			base.InitializeVariables();

			/// <example>
			/// QuantityVariableProfileList.Add(UserQuantityVariable);
			/// </example>
		}

		//========
		// CLASSES
		//========
	}

	/// <summary>
	/// The DefaultChangeMethods class is where the "Default" change routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the "Default" change routines are defined.
	/// </remarks>

	public class DefaultPlanChangeMethods : BasePlanChangeMethods
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DefaultPlanChangeMethods(BasePlanComputations aBasePlanComputations)
			: base(aBasePlanComputations)
		{
			/// <example>
			/// _change_SalesTotalUnits = new clsCustomChange_SalesTotalUnits(aBasePlanComputations, NextSequence);
			/// _change_SalesRegularUnits = new clsCustomChange_SalesTotalUnits(aBasePlanComputations, NextSequence);
			/// </example>
		}

		//===========
		// PROPERTIES
		//===========

		public DefaultComputations DefaultComputations
		{
			get
			{
				return (DefaultComputations)BasePlanComputations;
			}
		}

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========

		/// <example>
		/// private class clsCustomChange_SalesTotalUnits : PlanChangeMethodProfile
		/// {
		/// 	public clsCustomChange_SalesTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
		/// 		: base(aBasePlanComputations, aKey, "SalesTotalUnits Change Method")
		/// 	{
		/// 	}
		/// 
		/// 	override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
		/// 	{
		/// 		throw new DefaultComputations.TestException();
		/// 	}
		/// }
		/// </example>
	}

	/// <summary>
	/// The DefaultPlanCubeInitialization class is where the "Default" cube initialization routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the "Default" cube initialization routines are defined.
	/// </remarks>

	public class DefaultPlanCubeInitialization : BasePlanCubeInitialization
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DefaultPlanCubeInitialization(BasePlanComputations aBasePlanComputations)
			: base(aBasePlanComputations)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========
	}

	/// <summary>
	/// The DefaultPlanFormulasAndSpreads class is where the "Default" formulas and spreads are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the "Default" formulas and spreads are defined.
	/// </remarks>

	public class DefaultPlanFormulasAndSpreads : BasePlanFormulasAndSpreads
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DefaultPlanFormulasAndSpreads(BasePlanComputations aBasePlanComputations)
			: base(aBasePlanComputations)
		{
			/// <example>
			/// _init_SalesTotalUnits = new clsInit_UserInit(aBasePlanComputations, _seq++);
			/// </example>
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <example>
		/// private class clsInit_SalesTotalUnits : PlanFormulaProfile
		/// {
		/// 	public clsInit_SalesTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
		/// 		: base(aBasePlanComputations, aKey, "SalesTotalUnits Init")
		/// 	{
		/// 	}
		/// 
		/// 	override public eComputationFormulaReturnType Execute(ComputationScheduleFormulaEntry aCompSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
		/// 	{
		/// 	}
		/// }
		/// </example>

		//========
		// CLASSES
		//========
	}

	/// <summary>
	/// The DefaultPlanVariableInitialization class is where the "Default" variable initialization routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the "Default" variable initialization routines are defined.
	/// </remarks>

	public class DefaultPlanVariableInitialization : BasePlanVariableInitialization
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DefaultPlanVariableInitialization(BasePlanComputations aBasePlanComputations)
			: base(aBasePlanComputations)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========
	}

	/// <summary>
	/// The DefaultPlanToolBox class is where the "Default" variable initialization routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the "Default" variable initialization routines are defined.
	/// </remarks>

	public class DefaultPlanToolBox : BasePlanToolBox
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public DefaultPlanToolBox(BasePlanComputations aBasePlanComputations)
			: base(aBasePlanComputations)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========
	}
}
