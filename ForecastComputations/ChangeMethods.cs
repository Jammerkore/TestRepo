using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.ForecastComputations
{
	/// <summary>
	/// The ChangeMethods class is where the change routines are defined.
	/// </summary>
	/// <remarks>
	/// This class is where the change routines are defined.  A change routine is defined as a ChangeMethodDelegate that points to a
	/// method within this class that executes the change rules.  This method will contain all the logic to update appropriate values when a Cell is changed.
	/// </remarks>

	abstract public class BasePlanChangeMethods
	{
		//=======
		// FIELDS
		//=======

		private BasePlanComputations _basePlanComputations;
		private int _seq;

		//-------------------------------------------
		#region Autototals
		//-------------------------------------------

		protected PlanChangeMethodProfile _change_AutototalInitOnly;
		protected PlanChangeMethodProfile _change_AutototalSpreadLock;

		//-------------------------------------------
		#endregion
		//-------------------------------------------
		
		//-------------------------------------------
		#region Total Spreads
		//-------------------------------------------

		protected PlanChangeMethodProfile _change_SetTotal;
		protected PlanChangeMethodProfile _change_SetAverage;
		protected PlanChangeMethodProfile _change_SetCompTotal;
		protected PlanChangeMethodProfile _change_SetNonCompTotal;
		protected PlanChangeMethodProfile _change_SetNewTotal;
		protected PlanChangeMethodProfile _change_AllTotal;
		protected PlanChangeMethodProfile _change_AllAverage;
		protected PlanChangeMethodProfile _change_AllCompTotal;
		protected PlanChangeMethodProfile _change_AllNonCompTotal;
		protected PlanChangeMethodProfile _change_AllNewTotal;
		protected PlanChangeMethodProfile _change_LowLevelTotal;
		protected PlanChangeMethodProfile _change_LowLevelAverage;
		protected PlanChangeMethodProfile _change_PeriodTotal;
		protected PlanChangeMethodProfile _change_DateTotal;
		protected PlanChangeMethodProfile _change_DateAverage;
		protected PlanChangeMethodProfile _change_TotalPeriodToWeeks;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Rules
		//-------------------------------------------

		protected PlanChangeMethodProfile _change_SalesTotalUnits;
		protected PlanChangeMethodProfile _change_SalesTotalSetIndex;
		protected PlanChangeMethodProfile _change_SalesTotalAllIndex;
		protected PlanChangeMethodProfile _change_SalesRegularUnits;
		protected PlanChangeMethodProfile _change_SalesRegularSetIndex;
		protected PlanChangeMethodProfile _change_SalesRegularAllIndex;
		protected PlanChangeMethodProfile _change_SalesPromoUnits;
		protected PlanChangeMethodProfile _change_SalesPromoAllIndex;
		protected PlanChangeMethodProfile _change_SalesPromoSetIndex;
		protected PlanChangeMethodProfile _change_SalesRegPromoUnits;
		protected PlanChangeMethodProfile _change_SalesRegPromoSetIndex;
		protected PlanChangeMethodProfile _change_SalesRegPromoAllIndex;
		protected PlanChangeMethodProfile _change_InventoryTotalUnits;
		protected PlanChangeMethodProfile _change_InventoryTotalSetIndex;
		protected PlanChangeMethodProfile _change_InventoryTotalAllIndex;
		protected PlanChangeMethodProfile _change_InventoryRegularUnits;
		protected PlanChangeMethodProfile _change_InventoryRegularSetIndex;
		protected PlanChangeMethodProfile _change_InventoryRegularAllIndex;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Change Rules
		//-------------------------------------------

		protected PlanChangeMethodProfile _change_SalesTotalUnitsAverage;
		protected PlanChangeMethodProfile _change_AllSalesTotalUnitsAverage;
		protected PlanChangeMethodProfile _change_SalesRegularUnitsAverage;
		protected PlanChangeMethodProfile _change_SalesPromoUnitsAverage;
		protected PlanChangeMethodProfile _change_SalesRegPromoUnitsAverage;
		protected PlanChangeMethodProfile _change_AllSalesRegPromoUnitsAverage;
		protected PlanChangeMethodProfile _change_InventoryTotalUnitsAverage;
		protected PlanChangeMethodProfile _change_InventoryRegularUnitsAverage;
		protected PlanChangeMethodProfile _change_SalesTotalUnitsAverage_LowLevelTotal;
		protected PlanChangeMethodProfile _change_SalesRegularUnitsAverage_LowLevelTotal;
		protected PlanChangeMethodProfile _change_SalesPromoUnitsAverage_LowLevelTotal;
		protected PlanChangeMethodProfile _change_SalesRegPromoUnitsAverage_LowLevelTotal;
		protected PlanChangeMethodProfile _change_InventoryTotalUnitsAverage_LowLevelTotal;
		protected PlanChangeMethodProfile _change_InventoryRegularUnitsAverage_LowLevelTotal;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparatives
		//-------------------------------------------

		protected PlanChangeMethodProfile _change_ChainLowLevelBalance;
		protected PlanChangeMethodProfile _change_StoreLowLevelBalance;
		protected PlanChangeMethodProfile _change_ChainStoreDifference;
		protected PlanChangeMethodProfile _change_PctChange;
		protected PlanChangeMethodProfile _change_PctToLowLevel;
		protected PlanChangeMethodProfile _change_PctToSet;
		protected PlanChangeMethodProfile _change_PctToAll;
		protected PlanChangeMethodProfile _change_PctToTimeTotal;

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Static.  Creates a new instance of ChangeMethods.
		/// </summary>

		public BasePlanChangeMethods(BasePlanComputations aBasePlanComputations)
		{
			_basePlanComputations = aBasePlanComputations;
			_seq = 1;

			//-------------------------------------------
			#region Autototals
			//-------------------------------------------

			_change_AutototalInitOnly = new clsChange_AutototalInitOnly(aBasePlanComputations, NextSequence);
			_change_AutototalSpreadLock = new clsChange_AutototalSpreadLock(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------
		
			//-------------------------------------------
			#region Total Spreads
			//-------------------------------------------

			_change_SetTotal = new clsChange_SetTotal(aBasePlanComputations, NextSequence);
			_change_SetAverage = new clsChange_SetAverage(aBasePlanComputations, NextSequence);
			_change_SetCompTotal = new clsChange_SetCompTotal(aBasePlanComputations, NextSequence);
			_change_SetNonCompTotal = new clsChange_SetNonCompTotal(aBasePlanComputations, NextSequence);
			_change_SetNewTotal = new clsChange_SetNewTotal(aBasePlanComputations, NextSequence);
			_change_AllTotal = new clsChange_AllTotal(aBasePlanComputations, NextSequence);
			_change_AllAverage = new clsChange_AllAverage(aBasePlanComputations, NextSequence);
			_change_AllCompTotal = new clsChange_AllCompTotal(aBasePlanComputations, NextSequence);
			_change_AllNonCompTotal = new clsChange_AllNonCompTotal(aBasePlanComputations, NextSequence);
			_change_AllNewTotal = new clsChange_AllNewTotal(aBasePlanComputations, NextSequence);
			_change_LowLevelTotal = new clsChange_LowLevelTotal(aBasePlanComputations, NextSequence);
			_change_LowLevelAverage = new clsChange_LowLevelAverage(aBasePlanComputations, NextSequence);
			_change_PeriodTotal = new clsChange_PeriodTotal(aBasePlanComputations, NextSequence);
			_change_DateTotal = new clsChange_DateTotal(aBasePlanComputations, NextSequence);
			_change_DateAverage = new clsChange_DateAverage(aBasePlanComputations, NextSequence);
			_change_TotalPeriodToWeeks = new clsChange_TotalPeriodToWeeks(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Variable Change Rules
			//-------------------------------------------

			_change_SalesTotalUnits = new clsChange_SalesTotalUnits(aBasePlanComputations, NextSequence);
			_change_SalesTotalSetIndex = new clsChange_SalesTotalSetIndex(aBasePlanComputations, NextSequence);
			_change_SalesTotalAllIndex = new clsChange_SalesTotalAllIndex(aBasePlanComputations, NextSequence);
			_change_SalesRegularUnits = new clsChange_SalesRegularUnits(aBasePlanComputations, NextSequence);
			_change_SalesRegularSetIndex = new clsChange_SalesRegularSetIndex(aBasePlanComputations, NextSequence);
			_change_SalesRegularAllIndex = new clsChange_SalesRegularAllIndex(aBasePlanComputations, NextSequence);
			_change_SalesPromoUnits = new clsChange_SalesPromoUnits(aBasePlanComputations, NextSequence);
			_change_SalesPromoSetIndex = new clsChange_SalesPromoSetIndex(aBasePlanComputations, NextSequence);
			_change_SalesPromoAllIndex = new clsChange_SalesPromoAllIndex(aBasePlanComputations, NextSequence);
			_change_SalesRegPromoUnits = new clsChange_SalesRegPromoUnits(aBasePlanComputations, NextSequence);
			_change_SalesRegPromoSetIndex = new clsChange_SalesRegPromoSetIndex(aBasePlanComputations, NextSequence);
			_change_SalesRegPromoAllIndex = new clsChange_SalesRegPromoAllIndex(aBasePlanComputations, NextSequence);
			_change_InventoryTotalUnits = new clsChange_InventoryTotalUnits(aBasePlanComputations, NextSequence);
			_change_InventoryTotalSetIndex = new clsChange_InventoryTotalSetIndex(aBasePlanComputations, NextSequence);
			_change_InventoryTotalAllIndex = new clsChange_InventoryTotalAllIndex(aBasePlanComputations, NextSequence);
			_change_InventoryRegularUnits = new clsChange_InventoryRegularUnits(aBasePlanComputations, NextSequence);
			_change_InventoryRegularSetIndex = new clsChange_InventoryRegularSetIndex(aBasePlanComputations, NextSequence);
			_change_InventoryRegularAllIndex = new clsChange_InventoryRegularAllIndex(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Period Change Rules
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Time Total Change Rules
			//-------------------------------------------

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Set/Store Change Rules
			//-------------------------------------------

			_change_SalesTotalUnitsAverage = new clsChange_SalesTotalUnitsAverage(aBasePlanComputations, NextSequence);
			_change_AllSalesTotalUnitsAverage = new clsChange_AllSalesTotalUnitsAverage(aBasePlanComputations, NextSequence);
			_change_SalesRegularUnitsAverage = new clsChange_SalesRegularUnitsAverage(aBasePlanComputations, NextSequence);
			_change_SalesPromoUnitsAverage = new clsChange_SalesPromoUnitsAverage(aBasePlanComputations, NextSequence);
			_change_SalesRegPromoUnitsAverage = new clsChange_SalesRegPromoUnitsAverage(aBasePlanComputations, NextSequence);
			_change_AllSalesRegPromoUnitsAverage = new clsChange_AllSalesRegPromoUnitsAverage(aBasePlanComputations, NextSequence);
			_change_InventoryTotalUnitsAverage = new clsChange_InventoryTotalUnitsAverage(aBasePlanComputations, NextSequence);
			_change_InventoryRegularUnitsAverage = new clsChange_InventoryRegularUnitsAverage(aBasePlanComputations, NextSequence);
			_change_SalesTotalUnitsAverage_LowLevelTotal = new clsChange_SalesTotalUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_change_SalesRegularUnitsAverage_LowLevelTotal = new clsChange_SalesRegularUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_change_SalesPromoUnitsAverage_LowLevelTotal = new clsChange_SalesPromoUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_change_SalesRegPromoUnitsAverage_LowLevelTotal = new clsChange_SalesRegPromoUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_change_InventoryTotalUnitsAverage_LowLevelTotal = new clsChange_InventoryTotalUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);
			_change_InventoryRegularUnitsAverage_LowLevelTotal = new clsChange_InventoryRegularUnitsAverage_LowLevelTotal(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------

			//-------------------------------------------
			#region Comparatives
			//-------------------------------------------

			_change_ChainLowLevelBalance = new clsChange_ChainLowLevelBalance(aBasePlanComputations, NextSequence);
			_change_StoreLowLevelBalance = new clsChange_StoreLowLevelBalance(aBasePlanComputations, NextSequence);
			_change_ChainStoreDifference = new clsChange_ChainStoreDifference(aBasePlanComputations, NextSequence);
			_change_PctChange = new clsChange_PctChange(aBasePlanComputations, NextSequence);
			_change_PctToLowLevel = new clsChange_PctToLowLevel(aBasePlanComputations, NextSequence);
			_change_PctToSet = new clsChange_PctToSet(aBasePlanComputations, NextSequence);
			_change_PctToAll = new clsChange_PctToAll(aBasePlanComputations, NextSequence);
			_change_PctToTimeTotal = new clsChange_PctToTimeTotal(aBasePlanComputations, NextSequence);

			//-------------------------------------------
			#endregion
			//-------------------------------------------
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the next sequence number for creating Change Methods
		/// </summary>

		protected int NextSequence
		{
			get
			{
				return _seq++;
			}
		}
		
		/// <summary>
		/// Gets the BasePlanComputations
		/// </summary>

		public BasePlanComputations BasePlanComputations
		{
			get
			{
				return _basePlanComputations;
			}
		}

		//-------------------------------------------
		#region Autototals
		//-------------------------------------------

		public PlanChangeMethodProfile Change_AutototalInitOnly { get { return _change_AutototalInitOnly; } }
		public PlanChangeMethodProfile Change_AutototalSpreadLock { get { return _change_AutototalSpreadLock; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------
		
		//-------------------------------------------
		#region Total Spreads
		//-------------------------------------------

		public PlanChangeMethodProfile Change_SetTotal { get { return _change_SetTotal; } }
		public PlanChangeMethodProfile Change_SetAverage { get { return _change_SetAverage; } }
		public PlanChangeMethodProfile Change_SetCompTotal { get { return _change_SetCompTotal; } }
		public PlanChangeMethodProfile Change_SetNonCompTotal { get { return _change_SetNonCompTotal; } }
		public PlanChangeMethodProfile Change_SetNewTotal { get { return _change_SetNewTotal; } }
		public PlanChangeMethodProfile Change_AllTotal { get { return _change_AllTotal; } }
		public PlanChangeMethodProfile Change_AllAverage { get { return _change_AllAverage; } }
		public PlanChangeMethodProfile Change_AllCompTotal { get { return _change_AllCompTotal; } }
		public PlanChangeMethodProfile Change_AllNonCompTotal { get { return _change_AllNonCompTotal; } }
		public PlanChangeMethodProfile Change_AllNewTotal { get { return _change_AllNewTotal; } }
		public PlanChangeMethodProfile Change_LowLevelTotal { get { return _change_LowLevelTotal; } }
		public PlanChangeMethodProfile Change_LowLevelAverage { get { return _change_LowLevelAverage; } }
		public PlanChangeMethodProfile Change_PeriodTotal { get { return _change_PeriodTotal; } }
		public PlanChangeMethodProfile Change_DateTotal { get { return _change_DateTotal; } }
		public PlanChangeMethodProfile Change_DateAverage { get { return _change_DateAverage; } }
		public PlanChangeMethodProfile Change_TotalPeriodToWeeks { get { return _change_TotalPeriodToWeeks; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Rules
		//-------------------------------------------

		public PlanChangeMethodProfile Change_SalesTotalUnits { get { return _change_SalesTotalUnits; } }
		public PlanChangeMethodProfile Change_SalesTotalSetIndex { get { return _change_SalesTotalSetIndex; } }
		public PlanChangeMethodProfile Change_SalesTotalAllIndex { get { return _change_SalesTotalAllIndex; } }
		public PlanChangeMethodProfile Change_SalesRegularUnits { get { return _change_SalesRegularUnits; } }
		public PlanChangeMethodProfile Change_SalesRegularSetIndex { get { return _change_SalesRegularSetIndex; } }
		public PlanChangeMethodProfile Change_SalesRegularAllIndex { get { return _change_SalesRegularAllIndex; } }
		public PlanChangeMethodProfile Change_SalesPromoUnits { get { return _change_SalesPromoUnits; } }
		public PlanChangeMethodProfile Change_SalesPromoSetIndex { get { return _change_SalesPromoSetIndex; } }
		public PlanChangeMethodProfile Change_SalesPromoAllIndex { get { return _change_SalesPromoAllIndex; } }
		public PlanChangeMethodProfile Change_SalesRegPromoUnits { get { return _change_SalesRegPromoUnits; } }
		public PlanChangeMethodProfile Change_SalesRegPromoSetIndex { get { return _change_SalesRegPromoSetIndex; } }
		public PlanChangeMethodProfile Change_SalesRegPromoAllIndex { get { return _change_SalesRegPromoAllIndex; } }
		public PlanChangeMethodProfile Change_InventoryTotalUnits { get { return _change_InventoryTotalUnits; } }
		public PlanChangeMethodProfile Change_InventoryTotalSetIndex { get { return _change_InventoryTotalSetIndex; } }
		public PlanChangeMethodProfile Change_InventoryTotalAllIndex { get { return _change_InventoryTotalAllIndex; } }
		public PlanChangeMethodProfile Change_InventoryRegularUnits { get { return _change_InventoryRegularUnits; } }
		public PlanChangeMethodProfile Change_InventoryRegularSetIndex { get { return _change_InventoryRegularSetIndex; } }
		public PlanChangeMethodProfile Change_InventoryRegularAllIndex { get { return _change_InventoryRegularAllIndex; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Change Rules
		//-------------------------------------------

		public PlanChangeMethodProfile Change_SalesTotalUnitsAverage { get { return _change_SalesTotalUnitsAverage; } }
		public PlanChangeMethodProfile Change_AllSalesTotalUnitsAverage { get { return _change_AllSalesTotalUnitsAverage; } }
		public PlanChangeMethodProfile Change_SalesRegularUnitsAverage { get { return _change_SalesRegularUnitsAverage; } }
		public PlanChangeMethodProfile Change_SalesPromoUnitsAverage { get { return _change_SalesPromoUnitsAverage; } }
		public PlanChangeMethodProfile Change_SalesRegPromoUnitsAverage { get { return _change_SalesRegPromoUnitsAverage; } }
		public PlanChangeMethodProfile Change_AllSalesRegPromoUnitsAverage { get { return _change_AllSalesRegPromoUnitsAverage; } }
		public PlanChangeMethodProfile Change_InventoryTotalUnitsAverage { get { return _change_InventoryTotalUnitsAverage; } }
		public PlanChangeMethodProfile Change_InventoryRegularUnitsAverage { get { return _change_InventoryRegularUnitsAverage; } }
		public PlanChangeMethodProfile Change_SalesTotalUnitsAverage_LowLevelTotal { get { return _change_SalesTotalUnitsAverage_LowLevelTotal; } }
		public PlanChangeMethodProfile Change_SalesRegularUnitsAverage_LowLevelTotal { get { return _change_SalesRegularUnitsAverage_LowLevelTotal; } }
		public PlanChangeMethodProfile Change_SalesPromoUnitsAverage_LowLevelTotal { get { return _change_SalesPromoUnitsAverage_LowLevelTotal; } }
		public PlanChangeMethodProfile Change_SalesRegPromoUnitsAverage_LowLevelTotal { get { return _change_SalesRegPromoUnitsAverage_LowLevelTotal; } }
		public PlanChangeMethodProfile Change_InventoryTotalUnitsAverage_LowLevelTotal { get { return _change_InventoryTotalUnitsAverage_LowLevelTotal; } }
		public PlanChangeMethodProfile Change_InventoryRegularUnitsAverage_LowLevelTotal { get { return _change_InventoryRegularUnitsAverage_LowLevelTotal; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparatives
		//-------------------------------------------

		public PlanChangeMethodProfile Change_ChainLowLevelBalance { get { return _change_ChainLowLevelBalance; } }
		public PlanChangeMethodProfile Change_StoreLowLevelBalance { get { return _change_StoreLowLevelBalance; } }
		public PlanChangeMethodProfile Change_ChainStoreDifference { get { return _change_ChainStoreDifference; } }
		public PlanChangeMethodProfile Change_PctChange { get { return _change_PctChange; } }
		public PlanChangeMethodProfile Change_PctToLowLevel { get { return _change_PctToLowLevel; } }
		public PlanChangeMethodProfile Change_PctToSet { get { return _change_PctToSet; } }
		public PlanChangeMethodProfile Change_PctToAll { get { return _change_PctToAll; } }
		public PlanChangeMethodProfile Change_PctToTimeTotal { get { return _change_PctToTimeTotal; } }

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//========
		// METHODS
		//========

		//========
		// CLASSES
		//========

		//-------------------------------------------
		#region Base Classes
		//-------------------------------------------

		abstract public class PlanChangeMethodProfile : ChangeMethodProfile
		{
			//=======
			// FIELDS
			//=======

			BasePlanComputations _basePlanComputations;

			//=============
			// CONSTRUCTORS
			//=============

			public PlanChangeMethodProfile(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aKey, aName)
			{
				_basePlanComputations = aBasePlanComputations;
			}

			//===========
			// PROPERTIES
			//===========

			protected BasePlanVariables BasePlanVariables
			{
				get
				{
					return _basePlanComputations.BasePlanVariables;
				}
			}

			protected BasePlanQuantityVariables BasePlanQuantityVariables
			{
				get
				{
					return _basePlanComputations.BasePlanQuantityVariables;
				}
			}

			protected BasePlanTimeTotalVariables BasePlanTimeTotalVariables
			{
				get
				{
					return _basePlanComputations.BasePlanTimeTotalVariables;
				}
			}

			protected BasePlanFormulasAndSpreads BasePlanFormulasAndSpreads
			{
				get
				{
					return _basePlanComputations.BasePlanFormulasAndSpreads;
				}
			}

			protected BasePlanChangeMethods BasePlanChangeMethods
			{
				get
				{
					return _basePlanComputations.BasePlanChangeMethods;
				}
			}

			protected BasePlanToolBox BasePlanToolBox
			{
				get
				{
					return _basePlanComputations.BasePlanToolBox;
				}
			}

			override public void Execute(ComputationSchedule aCompSchd, ComputationCellReference aCompCellRef)
			{
				try
				{
					Execute(aCompSchd, (PlanCellReference)aCompCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}

			abstract public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef);
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Abstract Change Rules
		//-------------------------------------------

		abstract protected class clsChange_SetIdx : PlanChangeMethodProfile
		{
			abstract protected VariableProfile MainVarProf { get; }

			public clsChange_SetIdx(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;

				try
				{
					planCellRef = BasePlanToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef, eCubeType.GroupTotal, MainVarProf, BasePlanQuantityVariables.StoreAverage);
					BasePlanToolBox.SetCellCompLocked(planCellRef);

					newValue = aPlanCellRef.CurrentCellValue * planCellRef.CurrentCellValue / 100;
					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, MainVarProf, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		abstract protected class clsChange_AllIdx : PlanChangeMethodProfile
		{
			abstract protected VariableProfile MainVarProf { get; }

			public clsChange_AllIdx(BasePlanComputations aBasePlanComputations, int aKey, string aName)
				: base(aBasePlanComputations, aKey, aName)
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;

				try
				{
					planCellRef = BasePlanToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef, eCubeType.StoreTotal, MainVarProf, BasePlanQuantityVariables.StoreAverage);
					BasePlanToolBox.SetCellCompLocked(planCellRef);

					newValue = aPlanCellRef.CurrentCellValue * planCellRef.CurrentCellValue / 100;
					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, MainVarProf, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------
		
		//-------------------------------------------
		#region Autototals
		//-------------------------------------------

		protected class clsChange_AutototalInitOnly : PlanChangeMethodProfile
		{
			public clsChange_AutototalInitOnly(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AutototalInitOnly Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertAutoTotalFormula(aCompSchd, aPlanCellRef);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AutototalSpreadLock : PlanChangeMethodProfile
		{
			public clsChange_AutototalSpreadLock(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AutototalSpreadLock Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				ChangeMethodProfile chngProf;

				try
				{
					if (aPlanCellRef.isCellLocked || aPlanCellRef.isCellCompLocked)
					{
						chngProf = aPlanCellRef.PlanCube.GetPrimaryChangeMethodProfile(aPlanCellRef);
						
						if (chngProf != null)
						{
							chngProf.ExecuteChangeMethod(aCompSchd, aPlanCellRef, this.Name);
						}
					}
					else
					{
						BasePlanToolBox.InsertAutoTotalFormula(aCompSchd, aPlanCellRef);
					}
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------
		
		//-------------------------------------------
		#region Total Spreads
		//-------------------------------------------

		protected class clsChange_SetTotal : PlanChangeMethodProfile
		{
			public clsChange_SetTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SetTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_Set);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SetAverage : PlanChangeMethodProfile
		{
			public clsChange_SetAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SetAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SetAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SetCompTotal : PlanChangeMethodProfile
		{
			public clsChange_SetCompTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SetCompTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SetComp);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SetNonCompTotal : PlanChangeMethodProfile
		{
			public clsChange_SetNonCompTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SetNonCompTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SetNonComp);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SetNewTotal : PlanChangeMethodProfile
		{
			public clsChange_SetNewTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SetNewTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SetNew);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllTotal : PlanChangeMethodProfile
		{
			public clsChange_AllTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AllTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_All);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllAverage : PlanChangeMethodProfile
		{
			public clsChange_AllAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AllAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_AllAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllCompTotal : PlanChangeMethodProfile
		{
			public clsChange_AllCompTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AllCompTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_AllComp);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllNonCompTotal : PlanChangeMethodProfile
		{
			public clsChange_AllNonCompTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AllNonCompTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_AllNonComp);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllNewTotal : PlanChangeMethodProfile
		{
			public clsChange_AllNewTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AllNewTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_AllNew);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_LowLevel);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_LowLevelAverage : PlanChangeMethodProfile
		{
			public clsChange_LowLevelAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "LowLevelAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_LowLevelAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_PeriodTotal : PlanChangeMethodProfile
		{
			public clsChange_PeriodTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PeriodTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_Period);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_DateTotal : PlanChangeMethodProfile
		{
			public clsChange_DateTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "DateTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_Date);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_DateAverage : PlanChangeMethodProfile
		{
			public clsChange_DateAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "DateAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_DateAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_TotalPeriodToWeeks : PlanChangeMethodProfile
		{
			public clsChange_TotalPeriodToWeeks(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "TotalPeriodToWeeks Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_TotalPeriodToWeeks);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Variable Change Rules
		//-------------------------------------------

		protected class clsChange_SalesTotalUnits : PlanChangeMethodProfile
		{
			public clsChange_SalesTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				int begTime;
				int endTime;
				int timeId;
				PlanCellReference planCellRef;

				try
				{
					begTime = BasePlanToolBox.GetBeginPlanTimeDetail(aPlanCellRef);
					endTime = BasePlanToolBox.GetEndPlanTimeDetail(aPlanCellRef);

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ReceiptTotalUnits);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesStockRatioTotal);

					planCellRef = (PlanCellReference)aPlanCellRef.PlanCube.CreateCellReference(aPlanCellRef);

					for (timeId = begTime; timeId <= endTime; timeId = aPlanCellRef.PlanCube.IncrementTimeKey(timeId, 1))
					{
						planCellRef[aPlanCellRef.PlanCube.GetTimeType()] = timeId;

						if (!planCellRef.isCellProtected)
						{
							BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ForwardWOSTotal, timeId, endTime);
							break;
						}
					}

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SellThruPctTotal);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.GradeTotal);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT6);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryTotalUnitsT7);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesTotalSetIndex : clsChange_SetIdx
		{
			public clsChange_SalesTotalSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalSetIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesTotalUnits;
				}
			}
		}

		protected class clsChange_SalesTotalAllIndex : clsChange_AllIdx
		{
			public clsChange_SalesTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalAllIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesTotalUnits;
				}
			}
		}

		protected class clsChange_SalesRegularUnits : PlanChangeMethodProfile
		{
			public clsChange_SalesRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				int begTime;
				int endTime;
				int timeId;
				PlanCellReference planCellRef;

				try
				{
					begTime = BasePlanToolBox.GetBeginPlanTimeDetail(aPlanCellRef);
					endTime = BasePlanToolBox.GetEndPlanTimeDetail(aPlanCellRef);

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesRegPromoUnits);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ReceiptRegularUnits);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesStockRatioRegPromo);

					planCellRef = (PlanCellReference)aPlanCellRef.PlanCube.CreateCellReference(aPlanCellRef);

					for (timeId = begTime; timeId <= endTime; timeId = aPlanCellRef.PlanCube.IncrementTimeKey(timeId, 1))
					{
						planCellRef[aPlanCellRef.PlanCube.GetTimeType()] = timeId;

						if (!planCellRef.isCellProtected)
						{
							BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ForwardWOSRegPromo, timeId, endTime);
							break;
						}
					}

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SellThruPctRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.GradeRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT6);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT7);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesRegularSetIndex : clsChange_SetIdx
		{
			public clsChange_SalesRegularSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularSetIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesRegularUnits;
					}
			}
		}

		protected class clsChange_SalesRegularAllIndex : clsChange_AllIdx
		{
			public clsChange_SalesRegularAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularAllIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesRegularUnits;
				}
			}
		}

		protected class clsChange_SalesPromoUnits : PlanChangeMethodProfile
		{
			public clsChange_SalesPromoUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				int begTime;
				int endTime;
				int timeId;
				PlanCellReference planCellRef;

				try
				{
					begTime = BasePlanToolBox.GetBeginPlanTimeDetail(aPlanCellRef);
					endTime = BasePlanToolBox.GetEndPlanTimeDetail(aPlanCellRef);

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesRegPromoUnits);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ReceiptRegularUnits);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesStockRatioRegPromo);

					planCellRef = (PlanCellReference)aPlanCellRef.PlanCube.CreateCellReference(aPlanCellRef);

					for (timeId = begTime; timeId <= endTime; timeId = aPlanCellRef.PlanCube.IncrementTimeKey(timeId, 1))
					{
						planCellRef[aPlanCellRef.PlanCube.GetTimeType()] = timeId;

						if (!planCellRef.isCellProtected)
						{
							BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ForwardWOSRegPromo, timeId, endTime);
							break;
						}
					}

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SellThruPctRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.GradeRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT6);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT7);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesPromoSetIndex : clsChange_SetIdx
		{
			public clsChange_SalesPromoSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoSetIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesPromoUnits;
				}
			}
		}

		protected class clsChange_SalesPromoAllIndex : clsChange_AllIdx
		{
			public clsChange_SalesPromoAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoAllIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesPromoUnits;
				}
			}
		}

		protected class clsChange_SalesRegPromoUnits : PlanChangeMethodProfile
		{
			public clsChange_SalesRegPromoUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				int begTime;
				int endTime;
				int timeId;
				PlanCellReference planCellRef;

				try
				{
					begTime = BasePlanToolBox.GetBeginPlanTimeDetail(aPlanCellRef);
					endTime = BasePlanToolBox.GetEndPlanTimeDetail(aPlanCellRef);

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ReceiptRegularUnits);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesStockRatioRegPromo);

					planCellRef = (PlanCellReference)aPlanCellRef.PlanCube.CreateCellReference(aPlanCellRef);

					for (timeId = begTime; timeId <= endTime; timeId = aPlanCellRef.PlanCube.IncrementTimeKey(timeId, 1))
					{
						planCellRef[aPlanCellRef.PlanCube.GetTimeType()] = timeId;

						if (!planCellRef.isCellProtected)
						{
							BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ForwardWOSRegPromo, timeId, endTime);
							break;
						}
					}

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SellThruPctRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.GradeRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT6);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanTimeTotalVariables.InventoryRegularUnitsT7);

                    // Begin TT#1722 - RMatelic - Open Sales R/P to be editable
                    BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesRegPromoUnits);
                    // End TT#1722
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesRegPromoSetIndex : clsChange_SetIdx
		{
			public clsChange_SalesRegPromoSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoSetIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesRegPromoUnits;
				}
			}
		}

		protected class clsChange_SalesRegPromoAllIndex : clsChange_AllIdx
		{
			public clsChange_SalesRegPromoAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoAllIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.SalesRegPromoUnits;
				}
			}
		}

		protected class clsChange_InventoryTotalUnits : PlanChangeMethodProfile
		{
			public clsChange_InventoryTotalUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				int currTimeId;
				int currMinusOneTimeId;
				int endTimeId;

				try
				{
					currTimeId = BasePlanToolBox.GetCurrentPlanTimeDetail(aPlanCellRef);
					currMinusOneTimeId = BasePlanToolBox.GetCurrentPlanTimeDetail(aPlanCellRef, -1);
					endTimeId = BasePlanToolBox.GetEndPlanTimeDetail(aPlanCellRef);

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ReceiptTotalUnits, currMinusOneTimeId, currTimeId);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesStockRatioTotal);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ForwardWOSTotal, currTimeId, endTimeId);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SellThruPctTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_InventoryTotalSetIndex : clsChange_SetIdx
		{
			public clsChange_InventoryTotalSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalSetIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.InventoryTotalUnits;
				}
			}
		}

		protected class clsChange_InventoryTotalAllIndex : clsChange_AllIdx
		{
			public clsChange_InventoryTotalAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalAllIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.InventoryTotalUnits;
				}
			}
		}

		protected class clsChange_InventoryRegularUnits : PlanChangeMethodProfile
		{
			public clsChange_InventoryRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnits Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				int currTimeId;
				int currMinusOneTimeId;
				int endTimeId;

				try
				{
					currTimeId = BasePlanToolBox.GetCurrentPlanTimeDetail(aPlanCellRef);
					currMinusOneTimeId = BasePlanToolBox.GetCurrentPlanTimeDetail(aPlanCellRef, -1);
					endTimeId = BasePlanToolBox.GetEndPlanTimeDetail(aPlanCellRef);

					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ReceiptRegularUnits, currMinusOneTimeId, currTimeId);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SalesStockRatioRegPromo);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.ForwardWOSRegPromo, currTimeId, endTimeId);
					BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, BasePlanVariables.SellThruPctRegPromo);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_InventoryRegularSetIndex : clsChange_SetIdx
		{
			public clsChange_InventoryRegularSetIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularSetIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.InventoryRegularUnits;
				}
			}
		}

		protected class clsChange_InventoryRegularAllIndex : clsChange_AllIdx
		{
			public clsChange_InventoryRegularAllIndex(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularAllIndex Change Method")
			{
			}

			override protected VariableProfile MainVarProf
			{
				get
				{
					return BasePlanVariables.InventoryRegularUnits;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Period Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Time Total Change Rules
		//-------------------------------------------

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Set/Store Change Rules
		//-------------------------------------------

		protected class clsChange_SalesTotalUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_SalesTotalUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesTotalUnitsAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllSalesTotalUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_AllSalesTotalUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "AllSalesTotalUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				ArrayList cellRefArray;

				cellRefArray = BasePlanToolBox.GetComponentDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);

				foreach (PlanCellReference planCellRef in cellRefArray)
				{
					BasePlanToolBox.InsertInitFormula(
						aCompSchd,
						planCellRef,
						BasePlanVariables.GradeTotal);
				}
			}
		}

		protected class clsChange_SalesRegularUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_SalesRegularUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesRegularUnitsAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesPromoUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_SalesPromoUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesPromoUnitsAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_AllSalesRegPromoUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_AllSalesRegPromoUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "clsChange_AllSalesRegPromoUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				ArrayList cellRefArray;

				cellRefArray = BasePlanToolBox.GetComponentDetailCellRefArray(aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden);

				foreach (PlanCellReference planCellRef in cellRefArray)
				{
					BasePlanToolBox.InsertInitFormula(
						aCompSchd,
						planCellRef,
						BasePlanVariables.GradeRegPromo);
				}
			}
		}

		protected class clsChange_SalesRegPromoUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_SalesRegPromoUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesRegPromoUnitsAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_InventoryTotalUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_InventoryTotalUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_InventoryTotalUnitsAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_InventoryRegularUnitsAverage : PlanChangeMethodProfile
		{
			public clsChange_InventoryRegularUnitsAverage(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsAverage Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_InventoryRegularUnitsAverage);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesTotalUnitsAverage_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_SalesTotalUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesTotalUnitsAverage_LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesTotalUnitsAverage_LowLevelTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesRegularUnitsAverage_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_SalesRegularUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegularUnitsAverage_LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesRegularUnitsAverage_LowLevelTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesPromoUnitsAverage_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_SalesPromoUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesPromoUnitsAverage_LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesPromoUnitsAverage_LowLevelTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_SalesRegPromoUnitsAverage_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_SalesRegPromoUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "SalesRegPromoUnitsAverage_LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_SalesRegPromoUnitsAverage_LowLevelTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_InventoryTotalUnitsAverage_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_InventoryTotalUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryTotalUnitsAverage_LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_InventoryTotalUnitsAverage_LowLevelTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_InventoryRegularUnitsAverage_LowLevelTotal : PlanChangeMethodProfile
		{
			public clsChange_InventoryRegularUnitsAverage_LowLevelTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "InventoryRegularUnitsAverage_LowLevelTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				try
				{
					BasePlanToolBox.InsertSpread(aCompSchd, aPlanCellRef, BasePlanFormulasAndSpreads.Spread_InventoryRegularUnitsAverage_LowLevelTotal);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------

		//-------------------------------------------
		#region Comparatives
		//-------------------------------------------

		protected class clsChange_ChainLowLevelBalance : PlanChangeMethodProfile
		{
			public clsChange_ChainLowLevelBalance(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ChainLowLevelBalance Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetHighLevelChainOperandCellValue(null, eGetCellMode.Current, eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden) - aPlanCellRef.CurrentCellValue;

					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_StoreLowLevelBalance : PlanChangeMethodProfile
		{
			public clsChange_StoreLowLevelBalance(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "StoreLowLevelBalance Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				double newValue;

				try
				{
					newValue = BasePlanToolBox.GetHighLevelStoreOperandCellValue(null, eGetCellMode.Current, eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden) - aPlanCellRef.CurrentCellValue;

					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_ChainStoreDifference : PlanChangeMethodProfile
		{
			public clsChange_ChainStoreDifference(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "ChainStoreDifference Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;

				try
				{
					planCellRef = BasePlanToolBox.GetStoreTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value);

					newValue = BasePlanToolBox.GetOperandCellValue(null, eGetCellMode.Current, eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, aPlanCellRef.isCellHidden) + aPlanCellRef.CurrentCellValue;

					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, planCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_PctChange : PlanChangeMethodProfile
		{
			public clsChange_PctChange(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctChange Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				double newValue;
				double basisValue;

				try
				{
					basisValue = BasePlanToolBox.GetBasisOperandCellValueForPctChange(null, eGetCellMode.Current, eSetCellMode.Entry, aPlanCellRef, aPlanCellRef.isCellHidden);
					newValue = ((aPlanCellRef.CurrentCellValue * basisValue) / 100) + basisValue;

					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_PctToLowLevel : PlanChangeMethodProfile
		{
			public clsChange_PctToLowLevel(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToLowLevel Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;

				try
				{
					planCellRef = BasePlanToolBox.GetLowLevelTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef);
					newValue = aPlanCellRef.CurrentCellValue * planCellRef.CurrentCellValue / 100;

					BasePlanToolBox.SetCellCompLocked(planCellRef);
					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_PctToSet : PlanChangeMethodProfile
		{
			public clsChange_PctToSet(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToSet Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;

				try
				{
					planCellRef = BasePlanToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef, eCubeType.GroupTotal, BasePlanQuantityVariables.Value);
					newValue = aPlanCellRef.CurrentCellValue * planCellRef.CurrentCellValue / 100;

					BasePlanToolBox.SetCellCompLocked(planCellRef);
					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_PctToAll : PlanChangeMethodProfile
		{
			public clsChange_PctToAll(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToAll Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;

				try
				{
					planCellRef = BasePlanToolBox.GetTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef, eCubeType.StoreTotal, BasePlanQuantityVariables.Value);
					newValue = aPlanCellRef.CurrentCellValue * planCellRef.CurrentCellValue / 100;

					BasePlanToolBox.SetCellCompLocked(planCellRef);
					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		protected class clsChange_PctToTimeTotal : PlanChangeMethodProfile
		{
			public clsChange_PctToTimeTotal(BasePlanComputations aBasePlanComputations, int aKey)
				: base(aBasePlanComputations, aKey, "PctToTimeTotal Change Method")
			{
			}

			override public void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
			{
				PlanCellReference planCellRef;
				double newValue;
				VariableProfile varProf;

				try
				{
					varProf = (VariableProfile)BasePlanVariables.VariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
					planCellRef = BasePlanToolBox.GetTimeTotalOperandCell(null, eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, varProf.TotalTimeTotalVariableProfile);
					newValue = aPlanCellRef.CurrentCellValue * planCellRef.CurrentCellValue / 100;

					BasePlanToolBox.SetCellCompLocked(planCellRef);
					BasePlanToolBox.SetCellValue(eSetCellMode.Entry, aPlanCellRef, BasePlanQuantityVariables.Value, newValue);
				}
				catch (Exception exc)
				{
					throw;
				}
			}
		}

		//-------------------------------------------
		#endregion
		//-------------------------------------------
	}
}