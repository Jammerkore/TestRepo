using System;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// An interface that provides the ability for a session to receive real-time message handling from the client.
	/// </summary>

	public interface IMessageCallback
	{
		/// <summary>
		/// Requests the owner to handle a particular error.
		/// </summary>

// Begin Alert Events Code -- DO NOT REMOVE
//		void HandleAlert(AlertEventArgs aAlertEventArgs);
// End Alert Events Code -- DO NOT REMOVE
		System.Windows.Forms.DialogResult HandleMessage(string aText, string aCaption, System.Windows.Forms.MessageBoxButtons aButtons, System.Windows.Forms.MessageBoxIcon aIcon);
		System.Windows.Forms.DialogResult HandleMessage(eMIDTextCode aMsgCode, string aCaption, System.Windows.Forms.MessageBoxButtons aButtons, System.Windows.Forms.MessageBoxIcon aIcon);
	}

	public interface IBatchLoadData
	{
        // Begin TT#166 - JSmith - Store Characteristics auto add
        //bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile);
        bool LoadXMLTransFile(SessionAddressBlock SAB, string fileLocation, int commitLimit, string exceptionFile,
            bool autoAddCharacteristics);
        //bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile);
		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
        bool LoadDelimitedTransFile(SessionAddressBlock SAB, string fileLocation, char[] delimiter, int commitLimit, string exceptionFile,
            bool autoAddCharacteristics, char[] characteristicDelimiter, bool useCharacteristicTransaction);
        // End TT#166
		// BEGIN TT#1401 - stodd - add resevation stores (IMO)

		bool DeleteStoreBatchProcess(SessionAddressBlock SAB);	// TT#739-MD - STodd - delete stores
	}

	public interface IPlanComputationCubeInitialization
	{
		void ChainBasisDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainBasisWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainBasisDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainBasisPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainBasisLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainBasisLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainBasisLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainPlanWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainPlanDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainPlanPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainPlanLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainPlanLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void ChainPlanLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StoreBasisLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalGroupTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalGroupTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalGroupTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalStoreTotalWeekDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalStoreTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		void StorePlanLowLevelTotalStoreTotalPeriodDetail(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		void StoreBasisGradeTotalDateTotal(PlanCube aPlanCube, ePlanDisplayType aPlanDisplayType);
		//End TT#2 - JScott - Assortment Planning - Phase 2
	}

	public interface IPlanComputationsCollection
	{
		IPlanComputations GetComputations(string aName);
		IPlanComputations GetDefaultComputations();
		IPlanComputations[] GetComputationList();
	}

	public interface IPlanComputations
	{
		string Name { get; }
		IPlanComputationCubeInitialization PlanCubeInitialization { get; }
		IPlanComputationVariables PlanVariables { get; }
		IPlanComputationTimeTotalVariables PlanTimeTotalVariables { get; }
		IPlanComputationQuantityVariables PlanQuantityVariables { get; }
		object CreatePlanComputationWorkArea();
	}

	public interface IPlanComputationVariables
	{
		VariableProfile SalesTotalUnitsVariable { get; }
		VariableProfile SalesRegularUnitsVariable { get; }
		VariableProfile SalesPromoUnitsVariable { get; }
		VariableProfile SalesMarkdownUnitsVariable { get; }
		VariableProfile SalesRegPromoUnitsVariable { get; }
		VariableProfile SalesTotalSetIndexVariable { get; }
		VariableProfile SalesTotalAllStoreIndexVariable { get; }
		VariableProfile SalesRegPromoSetIndexVariable { get; }
		VariableProfile SalesRegPromoAllStoreIndexVariable { get; }
		VariableProfile InventoryTotalUnitsVariable { get; }
		VariableProfile InventoryRegularUnitsVariable { get; }
        VariableProfile InventoryRegularAllStoreIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements 
        VariableProfile InventoryTotalAllStoreIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements 
        VariableProfile InventoryRegularSetIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements 
        VariableProfile InventoryTotalSetIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements 
		VariableProfile SalesStockRatioTotalVariable { get; }  // Changes for Modify Sales Method-stodd
		VariableProfile SalesStockRatioRegPromoVariable { get; }	// Changes for Modify Sales Method-stodd
		VariableProfile SalesStockRatioMarkdownVariable { get; }	// Changes for Modify Sales Method-stodd
		VariableProfile SellThruPctTotalVariable { get; }
		VariableProfile SellThruPctTotalSetIndexVariable { get; }
		VariableProfile SellThruPctTotalAllStoreIndexVariable { get; }
		VariableProfile SellThruPctRegPromoVariable { get; }
		VariableProfile SellThruPctRegPromoSetIndexVariable { get; }
		VariableProfile SellThruPctRegPromoAllStoreIndexVariable { get; }
		VariableProfile IntransitVariable { get; }
		VariableProfile ReceiptRegularUnitsVariable { get; }
		VariableProfile ReceiptTotalUnitsVariable { get; }
		// BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
		VariableProfile WOSTotalVariable { get; }
		VariableProfile WOSRegPromoVariable { get; }
		// END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
        VariableProfile GradeTotalVariable { get; }                  // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        VariableProfile GradeRegPromoVariable { get; }               // End TT#638  

		bool CustomVariablesDefined { get; }
		int NumVariables { get; }
		int MaxTimeTotalVariables { get; }
		int MaxStoreTimeTotalVariables { get; }
		int MaxChainTimeTotalVariables { get; }
		ProfileXRef TimeTotalXRef { get; }
		ProfileList VariableProfileList { get; }

		ProfileList GetVariableProfilesByType(eVariableType aVariableType);
		ProfileList GetVariableProfilesByCustomType(eClientCustomVariableType aCustomVariableType);	// Issue 4827 stodd
		ProfileList GetTotalOnHandVariableList();
		ProfileList GetTotalWeekToDaySalesVariableList();
		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		ProfileList GetAssortmentPlanningVariableList();
		//End TT#2 - JScott - Assortment Planning - Phase 2
		VariableProfile GetVariableProfileByName(string aName);
		VariableProfile GetDatabaseColumnProfile(string aDatabaseColumn);
		int GetMaximumDatabaseColumnPosition();
		ProfileList GetDatabaseVariableList();
        VariableProfile GetVariableProfileByRegOrTotal(VariableProfile aVP, eOTSPlanLevelType aOTSPlanLevelType);  // TT#1560 - JSmith - Create filter to Out stores w no OH-put in Rule, processed. Vel still alloc
	
		//Get Database variable in all tables
		ProfileList GetStoreWeeklyHistoryDatabaseVariableList();
		ProfileList GetStoreDailyHistoryDatabaseVariableList();
		ProfileList GetStoreWeeklyForecastDatabaseVariableList();
		ProfileList GetChainWeeklyHistoryDatabaseVariableList();
		ProfileList GetChainWeeklyForecastDatabaseVariableList();

		//Get Database variable in Style tables
		ProfileList GetStoreWeeklyStyleHistoryDatabaseVariableList();
		ProfileList GetStoreDailyStyleHistoryDatabaseVariableList();
		ProfileList GetStoreWeeklyStyleForecastDatabaseVariableList();
		ProfileList GetChainWeeklyStyleHistoryDatabaseVariableList();
		ProfileList GetChainWeeklyStyleForecastDatabaseVariableList();

		//Get Database variable in Color tables
		ProfileList GetStoreWeeklyColorHistoryDatabaseVariableList();
		ProfileList GetStoreDailyColorHistoryDatabaseVariableList();
		ProfileList GetStoreWeeklyColorForecastDatabaseVariableList();
		ProfileList GetChainWeeklyColorHistoryDatabaseVariableList();
		ProfileList GetChainWeeklyColorForecastDatabaseVariableList();

		//Get Database variable in Size tables
		ProfileList GetStoreWeeklySizeHistoryDatabaseVariableList();
		ProfileList GetStoreDailySizeHistoryDatabaseVariableList();
		ProfileList GetStoreWeeklySizeForecastDatabaseVariableList();
		ProfileList GetChainWeeklySizeHistoryDatabaseVariableList();
		ProfileList GetChainWeeklySizeForecastDatabaseVariableList();

// Begin Track #4868 - JSmith - Variable Groupings
        ArrayList GetVariableGroupings();
// End Track #4868
	}

	public interface IPlanComputationTimeTotalVariables
	{
		TimeTotalVariableProfile TotalSalesTotalUnitsVariable { get; }
		TimeTotalVariableProfile AverageSalesTotalUnitsVariable { get; }
		TimeTotalVariableProfile TotalSalesRegPromoUnitsVariable { get; }
		TimeTotalVariableProfile AverageSalesRegPromoUnitsVariable { get; }
		TimeTotalVariableProfile TotalSalesTotalSetIndexVariable { get; }
		TimeTotalVariableProfile TotalSalesTotalAllStoreIndexVariable { get; }
		TimeTotalVariableProfile TotalSalesRegPromoSetIndexVariable { get; }
		TimeTotalVariableProfile TotalSalesRegPromoAllStoreIndexVariable { get; }
        TimeTotalVariableProfile TotalInventoryRegularAllStoreIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements
        TimeTotalVariableProfile TotalInventoryTotalAllStoreIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements 
        TimeTotalVariableProfile TotalInventoryRegularSetIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements 
        TimeTotalVariableProfile TotalInventoryTotalSetIndexVariable { get; } //TT#855-MD -jsobek -Velocity Enhancements     
		TimeTotalVariableProfile BeginningInventoryTotalUnitsVariable { get; }
		TimeTotalVariableProfile AverageInventoryTotalUnitsVariable { get; }
		TimeTotalVariableProfile BeginningInventoryRegularUnitsVariable { get; }
		TimeTotalVariableProfile AverageInventoryRegularUnitsVariable { get; }
		TimeTotalVariableProfile TotalSellThruPctTotalVariable { get; }
		TimeTotalVariableProfile TotalSellThruPctTotalSetIndexVariable { get; }
		TimeTotalVariableProfile TotalSellThruPctTotalAllStoreIndexVariable { get; }
		TimeTotalVariableProfile TotalSellThruPctRegPromoVariable { get; }
		TimeTotalVariableProfile TotalSellThruPctRegPromoSetIndexVariable { get; }
		TimeTotalVariableProfile TotalSellThruPctRegPromoAllStoreIndexVariable { get; }
		TimeTotalVariableProfile TotalReceiptRegularUnitsVariable { get; }
		TimeTotalVariableProfile TotalReceiptTotalUnitsVariable { get; }
		TimeTotalVariableProfile TotalIntransitVariable { get; }
		TimeTotalVariableProfile TotalOnHandRegularUnitsVariable { get; }
		TimeTotalVariableProfile TotalOnHandTotalUnitsVariable { get; }
		TimeTotalVariableProfile TotalNeedTotalVariable { get; }
		TimeTotalVariableProfile TotalNeedRegularVariable { get; }
        TimeTotalVariableProfile TotalGradeTotalVariable { get; }                  // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        TimeTotalVariableProfile TotalGradeRegPromoVariable { get; }               // End TT#638  

		int NumTimeTotalVariables { get; }
		ProfileList TimeTotalVariableProfileList { get; }
	}

	public interface IPlanComputationQuantityVariables
	{
		QuantityVariableProfile ValueQuantity { get; }
		QuantityVariableProfile StoreAverageQuantity { get; }
		QuantityVariableProfile PctChangeQuantity { get; }
		//Begin Track #6010 - JScott - Bad % Change on Basis2
		QuantityVariableProfile PctChangeToPlanQuantity { get; }
		//End Track #6010 - JScott - Bad % Change on Basis2
		QuantityVariableProfile BalanceQuantity { get; }
		QuantityVariableProfile DifferenceQuantity { get; }

		int NumQuantityVariables { get; }
		ProfileList QuantityVariableProfileList { get; }
		int GetQuantityVariableCountByCube(params ushort[] aCubeFlags1);
	}
}
