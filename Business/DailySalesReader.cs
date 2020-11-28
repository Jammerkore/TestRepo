		// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
	/// <summary>
	/// Reader for Daily Sales.
	/// </summary>
	[Serializable()]
	public class DailySalesReader:Profile
	{
		//========
		// FIELDS 
		//========
		ApplicationSessionTransaction _transaction;
		ProfileList _allStoreList;
		//Hashtable _storeHash;
		Hashtable _hierarchyNodeHash;
		//Hashtable _KeyTypeHash;

		//==============
		// CONSTRUCTORS
		//==============
		public DailySalesReader(ApplicationSessionTransaction aTransaction):base(-1)
		{
			_transaction = aTransaction;
			_allStoreList = _transaction.GetMasterProfileList(eProfileType.Store);
			//_storeHash = new Hashtable();
			_hierarchyNodeHash = new Hashtable();
			//_KeyTypeHash = null;
		}

		//============
		// PROPERTIES
		//============
		/// <summary>
		/// Abstract.  Gets the ProfileType of the Profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.DailySalesRdr;
			}
		}

		//=========
		// METHODS
		//=========
		#region GetCurrentDailySales
		#region GetStoreCurrentWeekToDaySales
		/// <summary>
		/// Gets the total daily sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which daily sales is desired.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which daily sales is desired.</param>
		/// <returns>Total Daily Sales value for each store for the specified merchandise on the current sales week.</returns>
		/// <remarks>Daily Sales week-to-day values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			ProfileList aStoreList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile, 
				aStoreList.ArrayList);
		}

		/// <summary>
		/// Gets the total daily sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which daily sales is desired.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which daily sales is desired.</param>
		/// <returns>Total Daily Sales value for each store for the specified merchandise on the current sales week.</returns>
		/// <remarks>Daily Sales week-to-day values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile.HierarchyRID, 
				aStoreRID_ArrayList);
		}

		/// <summary>
		/// Gets the total daily sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Total Daily Sales value for each store for the specified merchandise on the current sales week.</returns>
		/// <remarks>Daily Sales week-to-day values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			int aMdseHnRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeWeekToDaySales = new int[aStoreRID_ArrayList.Count];
			storeWeekToDaySales.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeWeekToDaySales[s] = 
					GetCurrentWeekToDaySales(aMdseHnRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeWeekToDaySales;
		}
	
		/// <summary>
		/// Gets the total daily sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which daily sales  is desired.</param>
		/// <param name="aStoreList">ProfileList of Stores for which daily sales  is desired.</param>
		/// <returns>Total daily sales  value for each store for the current sales week.</returns>
		/// <remarks>daily sales values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			int aMdseHnRID,
			ProfileList aStoreList)
		{
			int[] storeWeekToDaySales = new int[aStoreList.Count];
			storeWeekToDaySales.Initialize();
			for (int s=0; s < aStoreList.Count; s++)
			{
				storeWeekToDaySales[s] = 
					GetCurrentWeekToDaySales(aMdseHnRID, aStoreList[s].Key);
			}
			return storeWeekToDaySales;
		}

		/// <summary>
		/// Gets the store total daily sales for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnProfile">HierarchyNodeProfile that describes Merchandise hierarchy node for which OnHand is desired.</param>
		/// <param name="aStoreProfile">StoreProfile for the store</param>
		/// <returns>Store total daily sales value for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			HierarchyNodeProfile aMdseHnProfile,
			StoreProfile aStoreProfile)
		{
			return GetCurrentWeekToDaySales(
				aMdseHnProfile.HierarchyRID,
				aStoreProfile.Key);
		}

		/// <summary>
		/// Gets the store total daily sales for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">RID for Merhchandise hierarchy node for which daily sales is desired.</param>
		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store total Daily Sales value for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			int aMdseHnRID,
			int aStoreRID)
		{
			//IntransitKeyType ikt = new IntransitKeyType(0, 0);
			return GetCurrentWeekToDaySales(
				aMdseHnRID, eIntransitBy.Total,
				Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize,
				aStoreRID);
		}
		#endregion GetStoreCurrentWeekToDaySales
		 
		#region StoreColorWeekToDaySales
		/// <summary>
		/// Gets the color week-to-day sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which week-to-day sales is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which week-to-day sales is desired.</param>
		/// <returns>Color week-to-day sales value for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day sales values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			ProfileList aStoreList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile, 
				aColorRID, 
				aStoreList.ArrayList);
		}

		/// <summary>
		/// Gets the color week-to-day sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which week-to-day sales is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile.HierarchyRID, 
				aColorRID, 
				aStoreRID_ArrayList);
		}

		/// <summary>
		/// Gets the color OnHand for the specified stores for the current sales weekin specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>		
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			int aMdseHnRID,
			int aColorRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeWeekToDaySales = new int[aStoreRID_ArrayList.Count];
			storeWeekToDaySales.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeWeekToDaySales[s] = 
					GetCurrentWeekToDaySales
					(aMdseHnRID, aColorRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeWeekToDaySales;
		}
	
		/// <summary>
		/// Gets the store color OnHand for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnProfile">Merchandise hierarchy node profile that describes the merchandise for which OnHand is desired.</param>
		/// <param name="aColorRID">RID of the color</param>
		/// <param name="aStoreProfile">StoreProfile that describes the store</param>
		/// <returns>Store color Week-to-day Sales for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			HierarchyNodeProfile aMdseHnProfile,
			int aColorRID,
			StoreProfile aStoreProfile)
		{
			return GetCurrentWeekToDaySales(
				aMdseHnProfile,
				aColorRID,
				0,
				aStoreProfile);
		}

		/// <summary>
		/// Gets the store color OnHand for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID.</param>
		/// <param name="aColorRID">RID of the color</param>
		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store color Week-to-day Sales for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			int aMdseHnRID,
			int aColorRID,
			int aStoreRID)
		{
			return GetCurrentWeekToDaySales(
				aMdseHnRID,
				aColorRID,
				0,
				aStoreRID);
		}
		#endregion StoreColorWeekToDaySales

		#region StoreColorSizeWeekToDaySales
		/// <summary>
		/// Gets the color-size Week-To-Day Sales for the specified stores for the current sales weekin specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>Total Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			int aSizeRID,
			ProfileList aStoreList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile, 
				aColorRID, 
				aSizeRID, 
				aStoreList.ArrayList);
		}

		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the current sales weekin specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color-size Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile.HierarchyRID,
				aColorRID,  
				aSizeRID,
				aStoreRID_ArrayList);
		}

		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the current sales weekin specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color-size Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			int aMdseHnRID,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeWeekToDaySales = new int[aStoreRID_ArrayList.Count];
			storeWeekToDaySales.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeWeekToDaySales[s] = 
					GetCurrentWeekToDaySales
					(aMdseHnRID, aColorRID, aSizeRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeWeekToDaySales;
		}

		/// <summary>
		/// Gets the store color-size Week-To-Day Sales for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnProfile">Merhchandise hierarchy node profile that describes the merchandise for which Sales is desired.</param>
		/// <param name="aColorRID">RID of the color</param>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreProfile">Store Profile that describes the store for which Sales is desired.</param>
		/// <returns>Store color-size Week-to-day Sales for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			HierarchyNodeProfile aMdseHnProfile, 
			int aColorRID, 
			int aSizeRID,
			StoreProfile aStoreProfile)
		{
			return GetCurrentWeekToDaySales (
				aMdseHnProfile.HierarchyRID, 
				aColorRID, 
				aSizeRID, 
				aStoreProfile.Key);
		}

		/// <summary>
		/// Gets the store color-size Week-To-Day Sales for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnRID">RID of the merchandise hierarchy node.</param>
		/// <param name="aColorRID">RID for the Color.</param>
		/// <param name="aSizeRID">RID for the Size.  </param>
		/// <param name="aStoreRID">RID for the store.</param>
		/// <returns>Store color-size Week-to-day Sales for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			int aMdseHnRID,
			int aColorRID,
			int aSizeRID,
			int aStoreRID)
		{
			//IntransitKeyType IntransitKeyType = new IntransitKeyType(aColorRID, aSizeRID);
			if (aColorRID == Include.IntransitKeyTypeNoColor)
			{
				if (aSizeRID == Include.IntransitKeyTypeNoSize)
				{
					return GetCurrentWeekToDaySales(
						aMdseHnRID, eIntransitBy.Size,
						aColorRID, aSizeRID,
						aStoreRID);
				}
				return GetCurrentWeekToDaySales(
					aMdseHnRID, eIntransitBy.Total,
					aColorRID, aSizeRID,
					aStoreRID);
			}
			if (aSizeRID == Include.IntransitKeyTypeNoSize)
			{
				return GetCurrentWeekToDaySales(
					aMdseHnRID, eIntransitBy.Color,
					aColorRID, aSizeRID,
					aStoreRID);
			}
			return GetCurrentWeekToDaySales(
				aMdseHnRID, eIntransitBy.SizeWithinColors,
				aColorRID, aSizeRID,
				aStoreRID);
		}
		#endregion StoreColorSizeWeekToDaySales

		#region StoreWeekToDaySalesBy
		/// <summary>
		/// Gets the specified Week-To-Day Sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which Sales is desired.</param>
		/// <param name="aWeekToDayKeyType">Identifies the desired Week-To-Day Sales scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">RID for the size (ignored when total or color Sales requested.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which Sales is desired.</param>
		/// <returns>Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			eIntransitBy aWeekToDayKeyType,
			int aColorRID,
			int aSizeRID,
			ProfileList aStoreList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile, 
				aWeekToDayKeyType, 
				aColorRID, 
				aSizeRID, 
				aStoreList.ArrayList);
		}

		/// <summary>
		/// Gets the specified Week-To-Day Sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which Sales is desired.</param>
		/// <param name="aWeekToDayKeyType">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which Sales is desired.</param>
		/// <returns>Color Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			HierarchyNodeProfile aHnProfile,
			eIntransitBy aWeekToDayKeyType,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentWeekToDaySales(
				aHnProfile.HierarchyRID, 
				aWeekToDayKeyType, 
				aColorRID, 
				aSizeRID, 
				aStoreRID_ArrayList);
		}

		/// <summary>
		/// Gets the specified Week-To-Day Sales for the specified stores for the current sales week in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which Sales is desired.</param>
		/// <param name="aWeekToDayKeyType">Identifies the desired Sales scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which Sales is desired.</param>
		/// <returns>Week-to-day Sales for each store for the specified merchandise in the current sales week.</returns>
		/// <remarks>Week-to-day Sales are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentWeekToDaySales(
			int aMdseHnRID,
			eIntransitBy aWeekToDayKeyType,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeWeekToDaySales = new int[aStoreRID_ArrayList.Count];
			storeWeekToDaySales.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeWeekToDaySales[s] = 
					GetCurrentWeekToDaySales
					(aMdseHnRID, aWeekToDayKeyType, aColorRID, aSizeRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeWeekToDaySales;
		}

		/// <summary>
		/// Gets the store specified Week-To-Day Sales for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnProfile">Merhchandise hierarchy node profile that describes the merchandise for which OnHand is desired.</param>
		/// <param name="aWeekToDayKeyType">eIntransitBy value that describes the desired Sales detail.</param>
		/// <param name="aColorRID">RID of the color (ignored when total Sales requested).</param>
		/// <param name="aSizeRID">RID of the size (ignored when total or color Sales requested).</param>
		/// <param name="aStoreProfile">StoreProfile that describes the store</param>
		/// <returns>Store specified Week-to-day Sales for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			HierarchyNodeProfile aMdseHnProfile,
			eIntransitBy aWeekToDayKeyType,
			int aColorRID,
			int aSizeRID,
			StoreProfile aStoreProfile)
		{
			return GetCurrentWeekToDaySales(
				aMdseHnProfile.HierarchyRID,
				aWeekToDayKeyType,
				aColorRID,
				aSizeRID,
				aStoreProfile.Key);
		}

		/// <summary>
		/// Gets the store color Week-To-Day Sales for the specified merchandise hierarchy node in the current sales week. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID that describes the merchandise for which Sales is desired.</param>
		/// <param name="aWeekToDayKeyType">eIntransitBy value that describes the desired Sales detail.</param>
		/// <param name="aColorRID">RID of the color (ignored when total Sales requested).</param>
		/// <param name="aSizeRID">RID of the size (ignored when total or color Sales requested).</param>		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store color Week-to-day Sales for the specified merchandise in the current sales week.</returns>
		public int GetCurrentWeekToDaySales(
			int aMdseHnRID,
			eIntransitBy aWeekToDayKeyType,
			int aColorRID,
			int aSizeRID,
			int aStoreRID)
		{
			IntransitKeyType ikt;
			switch (aWeekToDayKeyType)
			{
				case (eIntransitBy.Total):
				{
					ikt = new IntransitKeyType(0,0);
					break;
				}
				case (eIntransitBy.Color):
				{
					ikt = new IntransitKeyType(aColorRID, 0);
					break;
				}
				case (eIntransitBy.SizeWithinColors):
				{
					ikt = new IntransitKeyType(aColorRID, aSizeRID);
					break;
				}
				case (eIntransitBy.Size):
				{
					ikt = new IntransitKeyType(0,aSizeRID);
					break;
				}
				default:
				{
					ikt = new IntransitKeyType(0,0);
					break;
				}
			}
			ProfileList variableList = new ProfileList(eProfileType.Variable); // MID Track 3547 Null Column Name Causes Database error
			HierarchyNodeProfile hnp = _transaction.GetNodeData(aMdseHnRID);
			if (eOTSPlanLevelType.Regular == hnp.OTSPlanLevelType)
			{
				// begin MID Track 3547 Null Column Name causes Database error
				//variableList = new ProfileList(eProfileType.Variable);
				//variableList.Add(_transaction.Variables.SalesRegularUnitsVariable);
				//variableList.Add(_transaction.Variables.SalesPromoUnitsVariable);
				VariableProfile vp = _transaction.PlanComputations.PlanVariables.SalesRegularUnitsVariable;
				//Begin Track #4637 - JSmith - Split variables by type
//				if (vp.DatabaseColumnName != null)
				if (vp.isDatabaseVariable(eVariableCategory.Store, Include.FV_ActualRID, eCalendarDateType.Day))
				//End Track #4637
				{
					variableList.Add(vp);
				}
				vp = _transaction.PlanComputations.PlanVariables.SalesPromoUnitsVariable;
				//Begin Track #4637 - JSmith - Split variables by type
//				if (vp.DatabaseColumnName != null)
				if (vp.isDatabaseVariable(eVariableCategory.Store, Include.FV_ActualRID, eCalendarDateType.Day))
				//End Track #4637
				{
					variableList.Add(vp);
				}
				// end MID Track 3547 Null Column Name causes Database error
			}
			else
			{
				// begin MID Track 3547 Null Column Name causes Database error
				//variableList = _transaction.Variables.GetTotalWeekToDaySalesVariableList();
				ProfileList pl = _transaction.PlanComputations.PlanVariables.GetTotalWeekToDaySalesVariableList();
				foreach (VariableProfile vp in pl)
				{
					if (vp.DatabaseColumnName != null)
					{
						variableList.Add(vp);
					}
				}
				// end MID Track 3547 Null Column Name causes Database error
			}
			// begin MID Track 3547 Null Column Name causes Database error
			if (variableList.Count < 1)
			{
				return 0;
			}
			// end MID Track 3547 Null Column Name causes Database error
			return GetCurrentWeekToDaySales(
				aMdseHnRID,
				ikt,
				aStoreRID,
				variableList);
		}
		#endregion StoreWeekToDaySalesBy

		#region StoreKeyTypeCurrentWeekToDaySales
		/// <summary>
		/// Gets store IntransitKeyType Week-to-day Sales for the merchandise hierarchy node in the current sales week.
		/// </summary>
		/// <param name="aMdseHnRID">RID of merchandise hierarchy node.</param>
		/// <param name="aWeekToDayKeyType">IntransitKeyType that describes the type of OnHand (total, color, size or color-size)</param>
		/// <param name="aStoreRID">RID of the store for which OnHand is desired.</param>
		/// <returns>Store OnHandKeyType Week-to-day Sales for the specified merchandise hierarchy node in the current sales week</returns>
		internal int GetCurrentWeekToDaySales(
			int aMdseHnRID,
			IntransitKeyType aWeekToDayKeyType,
			int aStoreRID,
			ProfileList aVariableList)
		{
			if (!_hierarchyNodeHash.Contains(aMdseHnRID))
			{
				LoadCurrentWeekDailySales(
					aMdseHnRID,
					aWeekToDayKeyType,
					aVariableList);
			}
			Hashtable keyTypeHash = (Hashtable)_hierarchyNodeHash[aMdseHnRID];
			if (!keyTypeHash.Contains(aWeekToDayKeyType.IntransitTypeKey))
			{
				LoadCurrentWeekDailySales(
					aMdseHnRID,
					aWeekToDayKeyType,
					aVariableList);
			}
			Hashtable variableStoreHash = (Hashtable)keyTypeHash[aWeekToDayKeyType.IntransitTypeKey];
			ProfileList vpl = new ProfileList(eProfileType.Variable);
			foreach (VariableProfile vp in aVariableList)
			{
				if (!variableStoreHash.Contains(vp.Key))
				{
					vpl.Add(vp);
				}
			}
			if (vpl.Count > 0)
			{
				LoadCurrentWeekDailySales(
					aMdseHnRID,
					aWeekToDayKeyType,
					vpl);
			}
            int storeValue = 0;
			Hashtable storeValueHash;
			foreach (VariableProfile vp in aVariableList)
			{
				if (variableStoreHash.Contains(vp.Key))
				{
					storeValueHash = (Hashtable)variableStoreHash[vp.Key];
					if (storeValueHash.Contains(aStoreRID))
					{
						storeValue += (int)storeValueHash[aStoreRID];
					}
				}
			}
            return storeValue;
		}
		#endregion StoreKeyTypeCurrentOnHand
		#endregion GetCurrentWeekToDaySales

		#region LoadCurrentWeekDailySales
		/// <summary>
		/// Loads Current Week's Daily Sales from the database.
		/// </summary>
		/// <param name="aMdseHnRID">RID for the merchandise hierarchy node to load.</param>
		/// <param name="aColorRID">RID for the color to load.</param>
		/// <param name="aSizeRID">RID for the size to load.</param>
		/// <param name="aVariableList">A list of variable profiles</param>
		/// <remarks>
		/// To get "total store" daily sales, both color and size RIDs must be zero.  When color
		/// RID is zero and SizeRID is not, a store's total size OnHand across all colors in 
		/// the requested merchandise hierarchy nodes will be loaded.  When size RID is zero and 
		/// ColorRID is not, a store's total color daily sales across all sizes will be loaded.  
		/// </remarks>
		private void LoadCurrentWeekDailySales (
			int aMdseHnRID,
			int aColorRID,
			int aSizeRID,              // MID Change Maintain Wk to Day for each Varuable
		    ProfileList aVariableList) // MID Change Maintain Wk to Day for each Varuable  
		{
			IntransitKeyType ikt = new IntransitKeyType(aColorRID, aSizeRID);
			LoadCurrentWeekDailySales(
				aMdseHnRID,
				ikt,            // MID Change Maintain Wk to Day for each Varuable
				aVariableList); // MID Change Maintain Wk to Day for each Varuable
		}

		/// <summary>
		/// Loads Current Week Daily Sales for the merchandise hierarchy node and WeekToDay key type.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise Hierarchy Node for which store sales is desired.</param>
		/// <param name="aWeekToDayKeyType">OnHand key type for which store sales is desired.</param>
		/// <param name="aVariableList">List of variables</param>
		private void LoadCurrentWeekDailySales (
			int aMdseHnRID,
			IntransitKeyType aWeekToDayKeyType, // MID Change Maintain Wk to Day for each Varuable
			ProfileList aVariableList) // MID Change Maintain Wk to Day for each Varuable
		{
			ArrayList hnArray = new ArrayList();
			hnArray.Add(aMdseHnRID);
			ArrayList keyTypeArray = new ArrayList();
			keyTypeArray.Add(aWeekToDayKeyType);
			LoadCurrentWeekDailySales(hnArray, keyTypeArray, aVariableList);  // MID Change Maintain Wk to Day for each Varuable  
		}

		/// <summary>
		/// Loads store current week daily sales for merchandise hierarchy nodes and WeekToDay key types.
		/// </summary>
		/// <param name="aMdseHnRID">ArrayList of merchandise hierarchy nodes for which OnHand is desired.</param>
		/// <param name="aWeekToDayKeyType">ArrayList of IntransitKeyTypes for which OnHand is desired.</param>
		/// <param name="aVariableList">Profile List of Variables to load</param>
		private void LoadCurrentWeekDailySales(
			ArrayList aMdseHnRID,
			ArrayList aWeekToDayKeyType,        // MID Change Maintain Wk to Day for each variable
		    ProfileList aVariableList)  // MID Change Maintain Wk to Day for each variable

		{
			int[,,,] storeWeekToDaySalesvalue = GetWeekToDaySales(	// MID Change Maintain Wk to Day for each variable 			
				aMdseHnRID,
				aWeekToDayKeyType,
				_allStoreList.ArrayList, // MID Change Maintain Wk to Day for each variable
				aVariableList); // MID Change Maintain Wk to Day for each variable

			IntransitKeyType ikt;
			for (int h=0; h < aMdseHnRID.Count; h++)
			{
				Hashtable keyTypeHash;
				if (_hierarchyNodeHash.Contains(aMdseHnRID[h]))
				{
					keyTypeHash = (Hashtable)_hierarchyNodeHash[aMdseHnRID[h]];
				}
				else
				{
					keyTypeHash = new Hashtable();
					_hierarchyNodeHash.Add(aMdseHnRID[h], keyTypeHash);
				}
				for(int i=0; i < aWeekToDayKeyType.Count; i++)
				{
					ikt = (IntransitKeyType)aWeekToDayKeyType[i];
					//if (_KeyTypeHash.Contains(ikt.IntransitTypeKey))
					//{
				 	//	_KeyTypeHash.Remove(ikt.IntransitTypeKey);
					//}
					Hashtable variableStoreHash = new Hashtable();
					if (keyTypeHash.Contains(ikt.IntransitTypeKey))
					{
                        variableStoreHash = (Hashtable)keyTypeHash[ikt.IntransitTypeKey];
					}
				    else
					{
						variableStoreHash = new Hashtable();
						keyTypeHash.Add(ikt.IntransitTypeKey, variableStoreHash);
					}
    				for (int s=0; s < _allStoreList.Count; s++)
					{
						Hashtable storeValueHash;
						StoreProfile sp = (StoreProfile)_allStoreList.ArrayList[s];
						VariableProfile vp;
						for (int v=0; v < aVariableList.Count; v++)
						{
							vp = (VariableProfile)aVariableList[v];
							if (variableStoreHash.Contains(vp.Key))
							{
								storeValueHash = (Hashtable)variableStoreHash[vp.Key];
							}
							else
							{
								storeValueHash = new Hashtable();
								variableStoreHash.Add(vp.Key, storeValueHash);
							}

							if (storeValueHash.Contains(sp.Key))
							{
								storeValueHash.Remove(sp.Key);
							}							
							storeValueHash.Add(sp.Key, storeWeekToDaySalesvalue[h, i, s, v]);
						}
					}
				}
			}
		}
	
		/// <summary>
		/// Gets store week-to-day sales for the current week, hierarchy nodes, key types and stores.
		/// </summary>
		/// <param name="aHnRIDList">ArrayList of Merchandise Hierarchy Node RIDs for which store sales is desired.</param>
		/// <param name="aWeekToDayKeyTypeList">ArrayList of IntransitKeyTypes for which store sales is desired.</param>
		/// <param name="aStoreRIDList">ArrayList of store RIDs for which sales is to be retrieved.</param>
		/// <param name="aVariableList">Profile list of variables</param>
		/// <returns>4-dimensional array of accumulated Week-to-day Sales. Dimension 1 = hnRIDList, dimension 2 = IntransitKeyTypeList, dimension 3 = storeRIDList, dimension 4 = variable</returns>
		private int[,,,] GetWeekToDaySales(      // MID Change Maintain Wk to Day for each variable
			ArrayList aHnRIDList,
			ArrayList aWeekToDayKeyTypeList,
			ArrayList aStoreRIDList,            // MID Change Maintain Wk to Day for each variable
			ProfileList aVariableList)  // MID Change Maintain Wk to Day for each variable
		{
			// Begin MID Change Maintain Wk to Day for each variable
			// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
			// string weekToDaySalesColumn;
			// bool planLevelRegular = true;
			//string salesRegColumn = _transaction.Variables.SalesRegularUnitsVariable.DatabaseColumnName;
			//string salesPromoColumn = _transaction.Variables.SalesPromoUnitsVariable.DatabaseColumnName;
			//string salesMkdnsColumn = _transaction.Variables.SalesMarkdownUnitsVariable.DatabaseColumnName;
			// End MID Track #2446
			// End MID Change Maintain Wk to Day for each variable

			int[,,,] weekToDaySalesValues =   // MID Change Maintain Wk to Day for each variable
				new int [aHnRIDList.Count, aWeekToDayKeyTypeList.Count, aStoreRIDList.Count, aVariableList.Count]; // MID Change Maintain Wk to Day for each variable
			weekToDaySalesValues.Initialize();

            // Begin TT#5124 - JSmith - Performance
            //VariablesData dbWeekToDaySales = new VariablesData(); 
            VariablesData dbWeekToDaySales = new VariablesData(_transaction.SAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
            // End TT#5124 - JSmith - Performance
			//Begin Track #4637 - JSmith - Split variables by type
//			ArrayList variableList = aVariableList.ArrayList;       // MID Change Maintain Wk to Day for each variable
			ArrayList variableList = new ArrayList();
			// Determine if variables are saved by day
			foreach (VariableProfile vp in aVariableList)
			{
				if (vp.isDatabaseVariable(eVariableCategory.Store, Include.FV_ActualRID, eCalendarDateType.Day))
				{
					variableList.Add(vp);
				}
			} 
			//End Track #4637
//Begin Track #3985 - JScott - Current WTD Sales wrong
//			DayProfile postDate = this._transaction.SAB.ApplicationServerSession.Calendar.PostDate;
			DayProfile postDate = this._transaction.SAB.ApplicationServerSession.Calendar.CurrentDate;
//End Track #3985 - JScott - Current WTD Sales wrong
			WeekProfile currentWK = postDate.Week;
			for (int h=0; h < aHnRIDList.Count; h++)
			{
				int hnRID = (int)aHnRIDList[h];
				HierarchyNodeProfile hnp = _transaction.GetNodeData(hnRID);
				// Begin MID Change Maintain Wk to day sales by variable
				//if (eOTSPlanLevelType.Regular == hnp.OTSPlanLevelType)
				//{
					// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
					// weekToDaySalesColumn = Include.SalesRegularUnitsColumn;
				//	planLevelRegular = true;
					// End MID Track #2446
				//}
				//else
				//{
					// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
					// weekToDaySalesColumn = Include.SalesTotalUnitsColumn;
				//	planLevelRegular = false;
					// End MID Track #2446
				//}
				// End MID Change Maintain Wk to day sales by varluble
				if (hnp.Key == -1)
				{
					throw new MIDException(eErrorLevel.severe, 0, "Hierarchy node not found");
				}
				int v;	// Begin MID Change Maintain Wk to Day for each variable
				for (int k=0; k < aWeekToDayKeyTypeList.Count; k++)
				{
					IntransitKeyType ikt = (IntransitKeyType)aWeekToDayKeyTypeList[k];
					switch (ikt.IntransitType)
					{
						case(eIntransitBy.Total):
						{
							foreach (DayProfile dayProfile in currentWK.Days)
							{
								if (dayProfile.Date <= postDate.Date)
								{
									DataTable dt = 
										dbWeekToDaySales.GetCurrentWKtoDaySalesByTimeByNode
										(dayProfile.YearDay,
										currentWK.Key,
										hnp.Key);
									foreach(DataRow dr in dt.Rows)
									{
										Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
										if (storeIdxRID.RID != Include.UndefinedStoreRID)
										{
											// Begin MID Change Maintain Wk to Day for each variable
											v = 0;
											foreach (VariableProfile vp in variableList)
											{
												weekToDaySalesValues[h, k, storeIdxRID.Index, v] +=
													Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
												v++;
											}
											// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
											// weekToDaySalesValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[weekToDaySalesColumn], CultureInfo.CurrentUICulture);
											//if (planLevelRegular)
											//{
											//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
											//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture);
											//}
											//else
											//{
											//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
											//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture) +
											//													 Convert.ToInt32(dr[salesMkdnsColumn], CultureInfo.CurrentUICulture);
											//}
											// End MID Track #2446
											// End MID Change Maintain Wk to Day for each variable
										}

									}	
								}
								else
								{
									break;
								}
							}
							break;
						}
						case (eIntransitBy.Color):
						{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
							HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color, eNodeSelectType.All);  // MID Change j.ellis Performance--cache ancestor and descendant data
//							HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color);  // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
							//HierarchyNodeList hnlColor = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Color); // MID Change Performance--cache ancestor and descendant data
							foreach(HierarchyNodeProfile hnpColor in hnlColor)
							{
								if (hnpColor.ColorOrSizeCodeRID == ikt.ColorRID)
								{
									// color match -- add to this bucket, for store list
									foreach (DayProfile dayProfile in currentWK.Days)
									{
										if (dayProfile.Date <= postDate.Date)
										{
											DataTable dt = 
												dbWeekToDaySales.GetCurrentWKtoDaySalesByTimeByNode
												(dayProfile.YearDay,
												currentWK.Key,
												hnpColor.Key);
											foreach(DataRow dr in dt.Rows)
											{
												Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
												if (storeIdxRID.RID != Include.UndefinedStoreRID)
												{
													// Begin MID Change Maintain Wk to Day for each variable
													v = 0;
													foreach (VariableProfile vp in variableList)
													{
														weekToDaySalesValues[h, k, storeIdxRID.Index, v] +=
															Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
														v++;
													}

													// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
													// weekToDaySalesValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[weekToDaySalesColumn], CultureInfo.CurrentUICulture);
													//if (planLevelRegular)
													//{
													//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
													//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture);
													//}
													//else
													//{
													//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
													//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture) +
													//													 Convert.ToInt32(dr[salesMkdnsColumn], CultureInfo.CurrentUICulture);
													//}
													// End MID Track #2446
													// End MID Change Maintain Wk to Day for each variable
												}
											}
										}
										else
										{
											break;
										}
									}
								}
							}
							break;
						}
						case (eIntransitBy.SizeWithinColors):
						{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
							HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color, eNodeSelectType.All);  // MID Change j.ellis Performance--cache ancestor and descendant data
//							HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color);  // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
							//HierarchyNodeList hnlColor = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Color); // MID Change Performance--cache ancestor and descendant data
							foreach(HierarchyNodeProfile hnpColor in hnlColor)
							{
								if (hnpColor.ColorOrSizeCodeRID == ikt.ColorRID)
								{
									// color match
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
									HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size, eNodeSelectType.All);  // MID Change j.ellis Performance--cache ancestor and descendant data
//									HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size);  // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
									//HierarchyNodeList hnlSize = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size); // MID Change Performance--cache ancestor and descendant data
									foreach(HierarchyNodeProfile hnpSize in hnlSize)
									{
										if (hnpSize.ColorOrSizeCodeRID == ikt.SizeRID)
										{
											// size match -- add to this bucket, for all store list
											foreach (DayProfile dayProfile in currentWK.Days)
											{
												if (dayProfile.Date <= postDate.Date)
												{
													DataTable dt = 
														dbWeekToDaySales.GetCurrentWKtoDaySalesByTimeByNode
														(dayProfile.YearDay,
														currentWK.Key,
														hnpSize.Key);
													foreach(DataRow dr in dt.Rows)
													{
														Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
														if (storeIdxRID.RID != Include.UndefinedStoreRID)
														{
															// Begin MID Change Maintain Wk to Day for each variable
															v = 0;
															foreach (VariableProfile vp in variableList)
															{
																weekToDaySalesValues[h, k, storeIdxRID.Index, v] +=
																	Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
																v++;
															}
															// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
															// weekToDaySalesValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[weekToDaySalesColumn], CultureInfo.CurrentUICulture);
															//if (planLevelRegular)
															//{
															//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
															//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture);
															//}
															//else
															//{
															//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
															//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture) +
															//													 Convert.ToInt32(dr[salesMkdnsColumn], CultureInfo.CurrentUICulture);
															//}
															// End MID Track #2446
															// End MID Change Maintain Wk to Day for each variable
														}
													}
												}
												else
												{
													break;
												}
											}
										}
									}
								}
							}
							break;
						}
						case (eIntransitBy.Size):
						{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
							HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Size, eNodeSelectType.All);  // MID Change j.ellis Performance--cache ancestor and descendant data
//							HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Size);  // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
							//HierarchyNodeList hnlSize = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Size); // MID Change Performance--cache ancestor and descendant data
							foreach(HierarchyNodeProfile hnpSize in hnlSize)
							{
								if (hnpSize.ColorOrSizeCodeRID == ikt.SizeRID)
								{
									// size match -- add to this bucket, for all store list
									foreach (DayProfile dayProfile in currentWK.Days)
									{
										if (dayProfile.Date <= postDate.Date)
										{
											DataTable dt = 
												dbWeekToDaySales.GetCurrentWKtoDaySalesByTimeByNode
												(dayProfile.YearDay,
												currentWK.Key,
												hnpSize.Key);
											foreach(DataRow dr in dt.Rows)
											{
												Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
												if (storeIdxRID.RID != Include.UndefinedStoreRID)
												{
													// Begin MID Change Maintain Wk to Day for each variable
													v = 0;
													foreach (VariableProfile vp in variableList)
													{
														weekToDaySalesValues[h, k, storeIdxRID.Index, v] +=
															Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
														v++;
													}
													// Begin MID Track #2446   Current Week to Day Sales values not showing in Velocity detail
													// weekToDaySalesValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[weekToDaySalesColumn], CultureInfo.CurrentUICulture);
													//if (planLevelRegular)
													//{
													//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
													//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture);
													//}
													//else
													//{
													//	weekToDaySalesValues[h, k, storeIdxRID.Index] += Convert.ToInt32(dr[salesRegColumn], CultureInfo.CurrentUICulture) +
													//													 Convert.ToInt32(dr[salesPromoColumn], CultureInfo.CurrentUICulture) +
													//													 Convert.ToInt32(dr[salesMkdnsColumn], CultureInfo.CurrentUICulture);
													//}
													// End MID Track #2446
													// End MID Change Maintain Wek to Day for each variable
												}
											}
										}
										else
										{
											break;
										}
									}
								}
							}
							break;
						}
						default:
						{
							break;
						}
					}
				}
			}
			return weekToDaySalesValues;
		}

		#endregion LoadCurrentWeekDailySales

		#region RemoveCurrentWeekDailySales
		/// <summary>
		/// Removes CurrentWeekDailySales from the hashtables for the specified merchandise hierarchy nodes in the current sales week.
		/// </summary>
		/// <param name="aMdseHnRIDs">ICollection of Merchandise Hierarchy Node RIDs to remove</param>
		internal void RemoveCurrentWeekDailySales (ICollection aMdseHnRIDs)
		{
			foreach(int hn in aMdseHnRIDs)
			{
				RemoveCurrentWeekDailySales (hn);
			}
		}

		/// <summary>
		/// Removes CurrentWeekDailySales from the hashtables for the specified merchandise hierarchy node in the current sales week.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise Hierarchy RID whose OnHand is to be removed.</param>
		internal void RemoveCurrentWeekDailySales (int aMdseHnRID)
		{
			if (_hierarchyNodeHash.Contains(aMdseHnRID))
			{
				_hierarchyNodeHash.Remove(aMdseHnRID);
			}
		}

		/// <summary>
		/// Removes CurrentWeekDailySales from hashtable for the specified OnHand key types within the specified merchandise hierarchy nodes in the current sales weeks.
		/// </summary>
		/// <param name="aMdseHn">ICollection of Merchandise hierarchy node RIDs to remove</param>
		/// <param name="aWeekToDayKeyType">ICollection of IntransitKeyTypes to remove</param>
		internal void RemoveCurrentWeekDailySales (ICollection aMdseHn, ICollection aWeekToDayKeyType)
		{
			foreach (int hn in aMdseHn)
			{
				RemoveCurrentWeekDailySales(hn, aWeekToDayKeyType);
			}
		}

		/// <summary>
		/// Removes CurrentWeekDailySales from hashtable for the specified OnHand key types for the specified merchandise hierarchy nodes in the current sales weeks.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise hierarchy node RID to remove</param>
		/// <param name="aWeekToDayKeyType">ICollection of IntransitKeyTypes to remove</param>
		internal void RemoveCurrentWeekDailySales (int aMdseHnRID, ICollection aWeekToDayKeyType)
		{
			foreach (IntransitKeyType ikt in aWeekToDayKeyType)
			{
				RemoveCurrentWeekDailySales (aMdseHnRID, ikt.IntransitTypeKey);
			}
		}
		/// <summary>
		/// Removes OnHand from hashtable for the specified OnHand key types for the specified merchandise hierarchy nodes in the current sales weeks.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise hierarchy node RID to remove</param>
		/// <param name="aWeekToDayKeyType">IntransitKeyType to remove</param>
		internal void RemoveCurrentWeekDailySales (int aMdseHnRID, long aWeekToDayKeyType)
		{
			if (_hierarchyNodeHash.Contains(aMdseHnRID))
			{
				_hierarchyNodeHash.Remove(aWeekToDayKeyType);
			}
		}
		#endregion RemoveCurrentWeekDailySales

	}
}
		// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review
