using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
// 
// GENERATED:                4/10/2017 11:59:04 AM
// Logility - RO PCF VERSION Version:3.5.6165.11347  Compiled:2016-11-17T06:18:14
// SOURCE DB SERVER:         MIDRetail14
// SOURCE DATABASE:          MID_PCF_Dev
// SOURCE CLIENT:            Groupe Dynamite
// LAST CHANGED BY:          LOGILITY\vgallagher  ON: 4/10/2017 11:31:14 AM
// CODEGEN COMPUTER:         MIDPCFVS2013
// VARIABLE COUNT:           69
// BUILD VERSION:            30319
// TARGET VERSION:           8.6
// 
namespace MIDRetail.ForecastComputations
{
    
    /// <summary>
    /// The ComputationsCollection class creates a collection for Computations objects.
    /// </summary>
    public class PlanComputationsCollection : BasePlanComputationsCollection
    {
        // =======
        //  FIELDS
        // =======
        // =============
        //  CONSTRUCTORS
        // =============
        public PlanComputationsCollection()
        {
            try
            {
                AddComputation(new DefaultComputations(), true);
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
    /// <summary>
    /// The PlanComputationWorkArea class stores PlanCubeGroup-level work fields.
    /// </summary>
    public class PlanComputationWorkArea
    {
    }
    /// <summary>
    /// The DefaultComputations class creates most of the pieces required by the computation process.
    /// </summary>
    public class DefaultComputations : BasePlanComputations
    {
        // =======
        //  FIELDS
        // =======
        // =============
        //  CONSTRUCTORS
        // =============
        /// <summary>
        /// Creates a new instance of Computations.
        /// </summary>
        public DefaultComputations() : 
                base()
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
        // ===========
        //  PROPERTIES
        // ===========
        /// <summary>
        /// Gets the name of this Computations object.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Default";
            }
        }
    }
    /// <summary>
    /// The PlanVariables class is where the variables are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the variables are defined.
    /// </remarks>
    public class PlanVariables : BasePlanVariables
    {
        public VariableProfile NoDaysOHRefiller;
        public VariableProfile AdjPerFcast;
        public VariableProfile DailySalesAdj;
        public VariableProfile GrossSalesRefiller;
        public VariableProfile OnHandWeeklySales;
        public VariableProfile WeeklySalesRefiller;
        public VariableProfile RefillerOnHand;
        public VariableProfile TargetWOSRefiller;
        public PlanVariables() : 
                base(true)
        {
            SalesTotalUnits = new VariableProfile(SalesTotalUnits.Key, "Sales", eVariableCategory.Both, eVariableType.Sales, "SALES", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eEligibilityType.Sales, eSimilarStoreDateType.Sales, eVariableTimeTotalType.All, eVariableForecastType.Sales, eValueFormatType.GenericNumeric, 0, true, true, false, eLevelRollType.Sum, eDayToWeekRollType.Sum, eStoreToChainRollType.Sum, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 1, "Sales");
            NoDaysOHRefiller = new VariableProfile(NextSequence, "# Days OH", eVariableCategory.Store, eVariableType.None, "NO_DAYS_OH_REFILLER", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Average, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            AdjPerFcast = new VariableProfile(NextSequence, "Adj Per Forecast", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            DailySalesAdj = new VariableProfile(NextSequence, "Daily Sales Adj", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 3, false, false, false, eLevelRollType.Average, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            GrossSalesRefiller = new VariableProfile(NextSequence, "Gross Sales", eVariableCategory.Store, eVariableType.None, "GROSS_SALES_REFILLER", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            OnHandWeeklySales = new VariableProfile(NextSequence, "OH/Wkly Sales", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 1, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            WeeklySalesRefiller = new VariableProfile(NextSequence, "Weekly Sales", eVariableCategory.Store, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 3, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            RefillerOnHand = new VariableProfile(NextSequence, "Refiller OH", eVariableCategory.Store, eVariableType.None, "REFILL_ONHAND", eVariableDatabaseType.Integer, eVariableDatabaseType.Integer, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            TargetWOSRefiller = new VariableProfile(NextSequence, "Target WOS", eVariableCategory.Store, eVariableType.None, "TARGETWOS_REFILLER", eVariableDatabaseType.Real, eVariableDatabaseType.Real, eVariableStyle.Percentage, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.Plug, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 1, false, false, false, eLevelRollType.Average, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, eVariableDatabaseModelType.All, 0, "Refiller");
            _variableGroupings = new ArrayList();
            _variableGroupings.Add("Sales");
            _variableGroupings.Add("Stock");
            _variableGroupings.Add("Other");
            _variableGroupings.Add("Refiller");
        }
        protected override void InitializeVariables(BasePlanTimeTotalVariables aBasePlanTimeTotalVariables)
        {
            base.InitializeVariables(aBasePlanTimeTotalVariables);
            PlanTimeTotalVariables PlanTimeTotals = (PlanTimeTotalVariables)aBasePlanTimeTotalVariables;
            VariableProfileList.Add(NoDaysOHRefiller);
            VariableProfileList.Add(AdjPerFcast);
            VariableProfileList.Add(DailySalesAdj);
            VariableProfileList.Add(GrossSalesRefiller);
            GrossSalesRefiller.AddTimeTotalVariable(PlanTimeTotals.GrossSalesRefillerT1);
            GrossSalesRefiller.TotalTimeTotalVariableProfile = PlanTimeTotals.GrossSalesRefillerT1;

            VariableProfileList.Add(OnHandWeeklySales);
            VariableProfileList.Add(WeeklySalesRefiller);
            VariableProfileList.Add(RefillerOnHand);
            VariableProfileList.Add(TargetWOSRefiller);
        }
    }
    /// <summary>
    /// The PlanTimeTotalVariables class is where the time-total variables are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the time-total variables are defined.
    /// </remarks>
    public class PlanTimeTotalVariables : BasePlanTimeTotalVariables
    {
        public TimeTotalVariableProfile GrossSalesRefillerT1;
        public PlanTimeTotalVariables() : 
                base()
        {
            SalesTotalUnitsT1 = new TimeTotalVariableProfile(SalesTotalUnitsT1.Key, "Tot Sales", eVariableCategory.Both, eVariableStyle.Units, eVariableAccess.Editable, eVariableScope.Static, eVariableSpreadType.PctContribution, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
            GrossSalesRefillerT1 = new TimeTotalVariableProfile(NextSequence, "Tot Gross Sales", eVariableCategory.Store, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eVariableTimeTotalType.All, eValueFormatType.GenericNumeric, 0);
        }
        protected override void InitializeVariables(BasePlanVariables aBasePlanVariables)
        {
            base.InitializeVariables(aBasePlanVariables);
            PlanVariables PlanVars = (PlanVariables)aBasePlanVariables;
            TimeTotalVariableProfileList.Add(GrossSalesRefillerT1);
            GrossSalesRefillerT1.ParentVariableProfile = PlanVars.GrossSalesRefiller;

        }
    }
    /// <summary>
    /// The PlanQuantityVariables class is where the quantity variables are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the quantity variables are defined.
    /// </remarks>
    public class PlanQuantityVariables : BasePlanQuantityVariables
    {
        public PlanQuantityVariables() : 
                base()
        {
        }
        protected override void InitializeVariables()
        {
            base.InitializeVariables();
        }
    }
    /// <summary>
    /// The DefaultPlanChangeMethods class is where the change routines are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the change routines are defined.
    /// </remarks>
    public class DefaultPlanChangeMethods : BasePlanChangeMethods
    {
        public PlanChangeMethodProfile Change_TargetWOSRefiller;
		public DefaultComputations DefaultComputations
		{
			get
			{
				return (DefaultComputations)BasePlanComputations;
			}
		}

        public DefaultPlanChangeMethods(BasePlanComputations aBasePlanComputations) : 
                base(aBasePlanComputations)
        {
            Change_TargetWOSRefiller = new clsChange_TargetWOSRefiller(aBasePlanComputations, NextSequence);
        }
        protected class clsChange_TargetWOSRefiller : PlanChangeMethodProfile
        {
			public clsChange_TargetWOSRefiller(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "TargetWOSRefiller Change Method")
			{
			}

            public override void Execute(ComputationSchedule aCompSchd, PlanCellReference aPlanCellRef)
            {
                try
                {
                    BasePlanToolBox.InsertInitFormula(aCompSchd, aPlanCellRef, ((PlanVariables)BasePlanVariables).AdjPerFcast);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
    }
    /// <summary>
    /// The DefaultPlanCubeInitialization class is where the "Default" cube initialization routines are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the "Default" cube initialization routines are defined.
    /// </remarks>
    public class DefaultPlanCubeInitialization : BasePlanCubeInitialization
    {
        public DefaultPlanCubeInitialization(BasePlanComputations aBasePlanComputations) : 
                base(aBasePlanComputations)
        {
        }
    }
    /// <summary>
    /// The DefaultPlanFormulasAndSpreads class is where the "Default" formulas and spreads are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the "Default" formulas and spreads are defined.
    /// </remarks>
    public class DefaultPlanFormulasAndSpreads : BasePlanFormulasAndSpreads
    {
        protected FormulaProfile _init_AvgDetailNoValueSubstitution;
        protected FormulaProfile _init_DailySalesAdj;
        protected FormulaProfile _init_WeeklySalesRefiller;
        protected FormulaProfile _init_OHWklySalesRefiller;
        protected FormulaProfile _init_SalesRegularUnits;
        protected FormulaProfile _init_InventoryRegularUnits;
        protected FormulaProfile _init_AdjPerFcast;
				public FormulaProfile Init_AvgDetailNoValueSubstitution { get { return _init_AvgDetailNoValueSubstitution; } }
				public FormulaProfile Init_DailySalesAdj { get { return _init_DailySalesAdj; } }
				public FormulaProfile Init_WeeklySalesRefiller { get { return _init_WeeklySalesRefiller; } }
				public FormulaProfile Init_OHWklySalesRefiller { get { return _init_OHWklySalesRefiller; } }
				public FormulaProfile Init_SalesRegularUnits { get { return _init_SalesRegularUnits; } }
				public FormulaProfile Init_InventoryRegularUnits { get { return _init_InventoryRegularUnits; } }
				public FormulaProfile Init_AdjPerFcast { get { return _init_AdjPerFcast; } }

		private class clsInit_AvgDetailNoValueSubstitution : clsFormula_Average
{
    public clsInit_AvgDetailNoValueSubstitution(BasePlanComputations aBasePlanComputations, int aKey)
        : base(aBasePlanComputations, aKey, "AvgDetailNoValueSubstitution Init")
    {
    }
}
        public DefaultPlanFormulasAndSpreads(BasePlanComputations aBasePlanComputations) : 
                base(aBasePlanComputations)
        {
            _init_AvgDetailNoValueSubstitution = new clsInit_AvgDetailNoValueSubstitution(aBasePlanComputations, NextSequence);
            _init_DailySalesAdj = new clsInit_DailySalesAdj(aBasePlanComputations, NextSequence);
            _init_WeeklySalesRefiller = new clsInit_WeeklySalesRefiller(aBasePlanComputations, NextSequence);
            _init_OHWklySalesRefiller = new clsInit_OHWklySalesRefiller(aBasePlanComputations, NextSequence);
            _init_SalesRegularUnits = new clsInit_SalesRegularUnits(aBasePlanComputations, NextSequence);
            _init_InventoryRegularUnits = new clsInit_InventoryRegularUnits(aBasePlanComputations, NextSequence);
            _init_AdjPerFcast = new clsInit_AdjPerFcast(aBasePlanComputations, NextSequence);
        }
        private class clsInit_DailySalesAdj : PlanFormulaProfile
        {
			public clsInit_DailySalesAdj(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "DailySalesAdj Init")
			{
			}

            public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
				double newValue;
                try
                {
                    newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).GrossSalesRefiller, aSchdEntry.PlanCellRef.isCellHidden) 
					/ BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).NoDaysOHRefiller, aSchdEntry.PlanCellRef.isCellHidden) ;

                    BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        private class clsInit_WeeklySalesRefiller : PlanFormulaProfile
        {
			public clsInit_WeeklySalesRefiller(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "WeeklySalesRefiller Init")
			{
			}

            public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
				double newValue;
                try
                {
                    newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).DailySalesAdj, aSchdEntry.PlanCellRef.isCellHidden) 
					* 7 ;

                    BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        private class clsInit_OHWklySalesRefiller : PlanFormulaProfile
        {
			public clsInit_OHWklySalesRefiller(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "OHWklySalesRefiller Init")
			{
			}

            public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
				double newValue;
                try
                {
                    newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).RefillerOnHand, aSchdEntry.PlanCellRef.isCellHidden) 
					/ BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).WeeklySalesRefiller, aSchdEntry.PlanCellRef.isCellHidden) ;

                    BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        private class clsInit_SalesRegularUnits : PlanFormulaProfile
        {
			public clsInit_SalesRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "SalesRegularUnits Init")
			{
			}

            public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
                try
                {
                    if ( ((DefaultPlanToolBox)BasePlanToolBox).isRefillerVersion(aSchdEntry.PlanCellRef)  &&  BasePlanToolBox.isStore(aSchdEntry.PlanCellRef)  &&  BasePlanToolBox.GetCurrentPlanTimeDetail(aSchdEntry.PlanCellRef) == aSchdEntry.PlanCellRef.Cube.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key )
                    {
                        BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, 0) ;
                    }
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        private class clsInit_InventoryRegularUnits : PlanFormulaProfile
        {
			public clsInit_InventoryRegularUnits(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "InventoryRegularUnits Init")
			{
			}

            public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
				double newValue;
                int currMinusOneTimeId;
                try
                {
                    currMinusOneTimeId = BasePlanToolBox.GetCurrentPlanTimeDetail(aSchdEntry.PlanCellRef, -1);
                    if ( BasePlanToolBox.isStore(aSchdEntry.PlanCellRef)  &&  BasePlanToolBox.GetCurrentPlanTimeDetail(aSchdEntry.PlanCellRef) > aSchdEntry.PlanCellRef.Cube.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key  &&  ((DefaultPlanToolBox)BasePlanToolBox).isRefillerVersion(aSchdEntry.PlanCellRef) )
                    {
                        newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).AdjPerFcast, currMinusOneTimeId, aSchdEntry.PlanCellRef.isCellHidden) ;

                        BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                    }
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
        private class clsInit_AdjPerFcast : PlanFormulaProfile
        {
			public clsInit_AdjPerFcast(BasePlanComputations aBasePlanComputations, int aKey)
				 : base(aBasePlanComputations, aKey, "AdjPerFcast Init")
			{
			}

            public override eComputationFormulaReturnType Execute(PlanScheduleFormulaEntry aSchdEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode)
            {
				double newValue;
                try
                {
                    if ( BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).OnHandWeeklySales, aSchdEntry.PlanCellRef.isCellHidden) < 1.0 )
                    {
                        newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).WeeklySalesRefiller, aSchdEntry.PlanCellRef.isCellHidden) 
					* BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).TargetWOSRefiller, aSchdEntry.PlanCellRef.isCellHidden) )
					* 1.2 ;

                        BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                    }
                    else
                    {
                        if ( BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).OnHandWeeklySales, aSchdEntry.PlanCellRef.isCellHidden)  >= 1.5 )
                        {
                            newValue = BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).WeeklySalesRefiller, aSchdEntry.PlanCellRef.isCellHidden) 
					* BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).TargetWOSRefiller, aSchdEntry.PlanCellRef.isCellHidden) ;

                            BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                        }
                        else
                        {
                            newValue = (BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).WeeklySalesRefiller, aSchdEntry.PlanCellRef.isCellHidden) 
					* BasePlanToolBox.GetOperandCellValue(aSchdEntry, aGetCellMode, aSetCellMode, aSchdEntry.PlanCellRef, ((PlanVariables)BasePlanVariables).TargetWOSRefiller, aSchdEntry.PlanCellRef.isCellHidden) )
					* 1.1 ;

                            BasePlanToolBox.SetCellValue(aSetCellMode, aSchdEntry.PlanCellRef, newValue);
                        }
                    }
                    return eComputationFormulaReturnType.Successful;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
    }
    /// <summary>
    /// This class is where the variable initialization routines are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the variable initialization routines are defined.
    /// </remarks>
    public class DefaultPlanVariableInitialization : BasePlanVariableInitialization
    {
        public DefaultPlanVariableInitialization(BasePlanComputations aBasePlanComputations) : 
                base(aBasePlanComputations)
        {
        }
        protected PlanVariables DefaultPlanVariables
        {
            get
            {
                return (PlanVariables)BasePlanVariables;
            }
        }
        protected PlanTimeTotalVariables DefaultPlanTimeTotalVariables
        {
            get
            {
                return (PlanTimeTotalVariables)BasePlanTimeTotalVariables;
            }
        }
        protected DefaultPlanChangeMethods DefaultPlanChangeMethods
        {
            get
            {
                return (DefaultPlanChangeMethods)BasePlanChangeMethods;
            }
        }
        protected DefaultPlanFormulasAndSpreads DefaultPlanFormulasAndSpreads
        {
            get
            {
                return (DefaultPlanFormulasAndSpreads)BasePlanFormulasAndSpreads;
            }
        }
        public override void ChainBasisDetail(PlanCube aPlanCube)
        {
            try
            {
                base.ChainBasisDetail(aPlanCube);
                aPlanCube.OverrideInitRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_SalesRegularUnits);
					aPlanCube.OverrideInitRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_InventoryRegularUnits);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainBasisWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.ChainBasisWeekDetail(aPlanCube);
                ChainTimeDetailPctChange(aPlanCube);
                ChainTimeDetailPctToTimePeriod(aPlanCube);
                ChainTimeDetailPctToLowLevelTotal(aPlanCube);
                ChainTimeDetailDifference(aPlanCube);
                ChainTimeDetailPctChangeToPlan(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailPctChangeToPlan(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainPlanWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.ChainPlanWeekDetail(aPlanCube);
                ChainTimeDetailPctChange(aPlanCube);
                ChainTimeDetailPctToTimePeriod(aPlanCube);
                ChainTimeDetailPctToLowLevelTotal(aPlanCube);
                ChainTimeDetailDifference(aPlanCube);
                ChainTimeDetailPctChangeToPlan(aPlanCube);
                aPlanCube.OverrideInitRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_SalesRegularUnits);
					aPlanCube.OverrideInitRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_InventoryRegularUnits);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainPlanLowLevelTotalWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.ChainPlanLowLevelTotalWeekDetail(aPlanCube);
                ChainTimeDetailPctChange(aPlanCube);
                ChainTimeDetailPctToTimePeriod(aPlanCube);
                ChainTimeDetailLowLevelAverage(aPlanCube);
                ChainTimeDetailBalance(aPlanCube);
                ChainTimeDetailPctChangeToPlan(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainPlanPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.ChainPlanPeriodDetail(aPlanCube);
                ChainTimeDetailPctChange(aPlanCube);
                ChainTimeDetailPctToTimePeriod(aPlanCube);
                ChainTimeDetailPctToLowLevelTotal(aPlanCube);
                ChainTimeDetailDifference(aPlanCube);
                ChainTimeDetailPctChangeToPlan(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainPlanLowLevelTotalPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.ChainPlanLowLevelTotalPeriodDetail(aPlanCube);
                ChainTimeDetailPctChange(aPlanCube);
                ChainTimeDetailPctToTimePeriod(aPlanCube);
                ChainTimeDetailLowLevelAverage(aPlanCube);
                ChainTimeDetailBalance(aPlanCube);
                ChainTimeDetailPctChangeToPlan(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailPctChange(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailPctToTimePeriod(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailPctToLowLevelTotal(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailLowLevelAverage(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailDifference(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeDetailBalance(PlanCube aPlanCube)
        {
            try
            {
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainPlanDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.ChainPlanDateTotal(aPlanCube);
                ChainTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                ChainTimeTotalPctChange(aPlanCube);
                ChainTimeTotalPctChangeToPlan(aPlanCube);
                ChainTimeTotalDifference(aPlanCube);
                ChainTimeTotalPctToLowLevelTotal(aPlanCube);
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void ChainPlanLowLevelTotalDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.ChainPlanLowLevelTotalDateTotal(aPlanCube);
                ChainTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                ChainTimeTotalPctChange(aPlanCube);
                ChainTimeTotalPctChangeToPlan(aPlanCube);
                ChainTimeTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);
                ChainTimeTotalBalance(aPlanCube);
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_ChainBalance, BasePlanChangeMethods.Change_ChainLowLevelBalance, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeTotalValue(PlanCube aPlanCube, QuantityVariableProfile aQuantVarProf)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, aQuantVarProf, BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
                if ( !  aPlanCube.isLowLevelTotalCube  &&  aQuantVarProf.Key != BasePlanQuantityVariables.LowLevelAverage.Key )
                {
                    aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, aQuantVarProf, BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeTotalPctChange(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeTotalPctChangeToPlan(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeTotalBalance(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_ChainBalance, null, BasePlanChangeMethods.Change_AutototalInitOnly);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeTotalDifference(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Difference, BasePlanFormulasAndSpreads.Init_Difference, null, BasePlanChangeMethods.Change_AutototalInitOnly);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void ChainTimeTotalPctToLowLevelTotal(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, null, BasePlanChangeMethods.Change_AutototalInitOnly);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StoreBasisDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StoreBasisDetail(aPlanCube);
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_AdjPerFcast, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_DailySalesAdj, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_OHWklySalesRefiller, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_WeeklySalesRefiller, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.OverrideInitRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_SalesRegularUnits);
					aPlanCube.OverrideInitRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_InventoryRegularUnits);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StoreBasisWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StoreBasisWeekDetail(aPlanCube);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToSet(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_AvgDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_AvgDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, null, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanWeekDetail(aPlanCube);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToSet(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_AdjPerFcast, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_DailySalesAdj, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_OHWklySalesRefiller, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_WeeklySalesRefiller, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.Value, null, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.Value, DefaultPlanChangeMethods.Change_TargetWOSRefiller, null);
					aPlanCube.OverrideInitRule(BasePlanVariables.SalesRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_SalesRegularUnits);
					aPlanCube.OverrideInitRule(BasePlanVariables.InventoryRegularUnits, BasePlanQuantityVariables.Value, DefaultPlanFormulasAndSpreads.Init_InventoryRegularUnits);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalWeekDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_LowLevelTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToSet(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailLowLevelAverage(aPlanCube);
                StoreTimeDetailBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanGroupTotalWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanGroupTotalWeekDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
                StoreTimeDetailStoreAverage(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_SetCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_SetNonCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_SetNewTotal);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalGroupTotalWeekDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailStoreAverage(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_SetCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_SetNonCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_SetNewTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);
                StoreTimeDetailBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanStoreTotalWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanStoreTotalWeekDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
                StoreTimeDetailStoreAverage(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_AllCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_AllNonCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_AllNewTotal);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalStoreTotalWeekDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailStoreAverage(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumCompStore, BasePlanChangeMethods.Change_AllCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumNonCompStore, BasePlanChangeMethods.Change_AllNonCompTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumNewStore, BasePlanChangeMethods.Change_AllNewTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);
                StoreTimeDetailBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanPeriodDetail(aPlanCube);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToSet(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodAvgDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodAvgDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_PeriodSumDetail, BasePlanChangeMethods.Change_PeriodTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalPeriodDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_LowLevelTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToSet(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailLowLevelAverage(aPlanCube);
                StoreTimeDetailBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanGroupTotalPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanGroupTotalPeriodDetail(aPlanCube);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalGroupTotalPeriodDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToAllStore(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_SetTotal);
                StoreTimeDetailBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanStoreTotalPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanStoreTotalPeriodDetail(aPlanCube);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailPctToLowLevelTotal(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalStoreTotalPeriodDetail(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Value, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);
                StoreTimeDetailPctChange(aPlanCube);
                StoreTimeDetailPctChangeToPlan(aPlanCube);
                StoreTimeDetailPctToTimePeriod(aPlanCube);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.Comp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.New, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_TotalPeriodToWeeks);
                StoreTimeDetailTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_AllTotal);
                StoreTimeDetailBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailTotalValue(PlanCube aPlanCube, QuantityVariableProfile aQuantVarProf, FormulaProfile aInitSumFormula, ChangeMethodProfile aPrimaryChangeMethod)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, aQuantVarProf, aInitSumFormula, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, aQuantVarProf, BasePlanFormulasAndSpreads.Init_AvgDetail, aPrimaryChangeMethod, BasePlanChangeMethods.Change_AutototalSpreadLock);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailPctChange(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailPctChangeToPlan(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailPctToSet(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailPctToAllStore(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailPctToTimePeriod(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.PctToTimePeriod, BasePlanFormulasAndSpreads.Init_PctToTimePeriod, BasePlanChangeMethods.Change_PctToTimeTotal, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailPctToLowLevelTotal(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailLowLevelAverage(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.LowLevelAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_LowLevelAverage, BasePlanChangeMethods.Change_AutototalSpreadLock);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailBalance(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeDetailStoreAverage(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanVariables.NoDaysOHRefiller, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.AdjPerFcast, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.DailySalesAdj, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.GrossSalesRefiller, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.OnHandWeeklySales, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.WeeklySalesRefiller, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.RefillerOnHand, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
					aPlanCube.AddRule(DefaultPlanVariables.TargetWOSRefiller, BasePlanQuantityVariables.StoreAverage, BasePlanFormulasAndSpreads.Init_AvgDetail, BasePlanChangeMethods.Change_SetAverage, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanDateTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                StoreTimeTotalPctChange(aPlanCube);
                StoreTimeTotalPctChangeToPlan(aPlanCube);
                StoreTimeTotalPctToSet(aPlanCube);
                StoreTimeTotalPctToAllStore(aPlanCube);
                StoreTimeTotalPctToLowLevelTotal(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalDateTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                StoreTimeTotalPctChange(aPlanCube);
                StoreTimeTotalPctChangeToPlan(aPlanCube);
                StoreTimeTotalPctToSet(aPlanCube);
                StoreTimeTotalPctToAllStore(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);
                StoreTimeTotalBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanGroupTotalDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanGroupTotalDateTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.StoreAverage);
                StoreTimeTotalPctChange(aPlanCube);
                StoreTimeTotalPctChangeToPlan(aPlanCube);
                StoreTimeTotalPctToAllStore(aPlanCube);
                StoreTimeTotalPctToLowLevelTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Comp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.New);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalGroupTotalDateTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.StoreAverage);
                StoreTimeTotalPctChange(aPlanCube);
                StoreTimeTotalPctChangeToPlan(aPlanCube);
                StoreTimeTotalPctToAllStore(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Comp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.New);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);
                StoreTimeTotalBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanStoreTotalDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanStoreTotalDateTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.StoreAverage);
                StoreTimeTotalPctChange(aPlanCube);
                StoreTimeTotalPctChangeToPlan(aPlanCube);
                StoreTimeTotalPctToLowLevelTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Comp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.New);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public override void StorePlanLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube)
        {
            try
            {
                base.StorePlanLowLevelTotalStoreTotalDateTotal(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Value);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.StoreAverage);
                StoreTimeTotalPctChange(aPlanCube);
                StoreTimeTotalPctChangeToPlan(aPlanCube);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.Comp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.NonComp);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.New);
                StoreTimeTotalValue(aPlanCube, BasePlanQuantityVariables.LowLevelAverage);
                StoreTimeTotalBalance(aPlanCube);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalValue(PlanCube aPlanCube, QuantityVariableProfile aQuantVarProf)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, aQuantVarProf, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, aQuantVarProf, BasePlanFormulasAndSpreads.Init_SumDetail, BasePlanChangeMethods.Change_DateTotal, BasePlanChangeMethods.Change_AutototalSpreadLock);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalPctChange(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, BasePlanQuantityVariables.PctChange, BasePlanFormulasAndSpreads.Init_PctChange, BasePlanChangeMethods.Change_PctChange, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalPctChangeToPlan(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, BasePlanQuantityVariables.PctChangeToPlan, BasePlanFormulasAndSpreads.Init_PctChangeToPlan, BasePlanChangeMethods.Change_PctChange, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalPctToSet(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, BasePlanQuantityVariables.PctToSet, BasePlanFormulasAndSpreads.Init_PctToSet, BasePlanChangeMethods.Change_PctToSet, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalPctToAllStore(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, BasePlanQuantityVariables.PctToAllStore, BasePlanFormulasAndSpreads.Init_PctToAll, BasePlanChangeMethods.Change_PctToAll, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalPctToLowLevelTotal(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, BasePlanQuantityVariables.PctToLowLevelTotal, BasePlanFormulasAndSpreads.Init_PctToLowLevel, BasePlanChangeMethods.Change_PctToLowLevel, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private void StoreTimeTotalBalance(PlanCube aPlanCube)
        {
            try
            {
                aPlanCube.AddRule(DefaultPlanTimeTotalVariables.SalesTotalUnitsT1, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
					aPlanCube.AddRule(DefaultPlanTimeTotalVariables.GrossSalesRefillerT1, BasePlanQuantityVariables.Balance, BasePlanFormulasAndSpreads.Init_StoreBalance, BasePlanChangeMethods.Change_StoreLowLevelBalance, null);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }
    /// <summary>
    /// The DefaultPlanToolBox class is where the 'Default' variable initialization routines are defined.
    /// </summary>
    /// <remarks>
    /// This class is where the "Default" variable initialization routines are defined.
    /// </remarks>
    public class DefaultPlanToolBox : BasePlanToolBox
    {

		//=======
		// FIELDS
		//=======

		//Begin TT#824 - JScott - Dots - Custom Variables/Calcs - DocVer 6.0
		private ArrayList _lookupLock;
		private string _lastNodeIdLookup;
		private int _lastNodeRIDLookup;

		//End TT#824 - JScott - Dots - Custom Variables/Calcs - DocVer 6.0
		//=============
		// CONSTRUCTORS
		//=============

		public DefaultPlanToolBox(BasePlanComputations aBasePlanComputations)
			: base(aBasePlanComputations)
		{
			//Begin TT#824 - JScott - Dots - Custom Variables/Calcs - DocVer 6.0
			_lookupLock = new ArrayList();
			_lastNodeIdLookup = string.Empty;
			//End TT#824 - JScott - Dots - Custom Variables/Calcs - DocVer 6.0
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//Begin TT#824 - JScott - Dots - Custom Variables/Calcs - DocVer 6.0
		public bool isHierarchyNode(PlanCellReference aPlanCellRef, string aNodeId)
		{
			int nodeRID;

			try
			{
				lock (_lookupLock.SyncRoot)
				{
					if (_lastNodeIdLookup != aNodeId)
					{
						_lastNodeIdLookup = aNodeId;
						_lastNodeRIDLookup = aPlanCellRef.PlanCube.SAB.HierarchyServerSession.GetNodeRID(aNodeId);
					}

					nodeRID = _lastNodeRIDLookup;
				}

				if (nodeRID != Include.NoRID)
				{
					if (aPlanCellRef[aPlanCellRef.PlanCube.GetHierarchyNodeType()] == nodeRID)
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception exc)
			{
				throw;
			}
		}

		//End TT#824 - JScott - Dots - Custom Variables/Calcs - DocVer 6.0

                                public bool isRefillerVersion(PlanCellReference aPlanCellRef)
                               {
                                     int currVersionRID = aPlanCellRef.GetVersionProfileOfData().Key;
                                     return currVersionRID == GetVersionRID(aPlanCellRef.PlanCube.Transaction, "Refiller");
                               }
		
		public int GetVersionRID(ApplicationSessionTransaction aTransaction, string aVersion)
                              {
                              ProfileList verProfList;

                               try
                              {
                               verProfList = aTransaction.GetProfileList(eProfileType.Version);

                               foreach (VersionProfile verProf in verProfList)
                             {
                               if (verProf.Description == aVersion)
                             {      
                                 return verProf.Key;
                             }
                             }

                             return Include.NoRID;
                            }
                            catch (Exception exc)
                           {
                           string message = exc.ToString();
                           throw;
                           }
                           }
		//========
		// CLASSES
		//========

    }
}
