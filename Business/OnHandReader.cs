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
	/// Reader for OnHand.
	/// </summary>
	[Serializable()]
	public class OnHandReader:Profile
	{
		//========
		// FIELDS 
		//========
		ApplicationSessionTransaction _transaction;
		ProfileList _allStoreList;
		Hashtable _storeHash;
		Hashtable _hierarchyNodeHash;
		Hashtable _KeyTypeHash;

		//==============
		// CONSTRUCTORS
		//==============
		public OnHandReader(ApplicationSessionTransaction aTransaction):base(-1)
		{
			_transaction = aTransaction;
			_allStoreList = _transaction.GetMasterProfileList(eProfileType.Store);
			_storeHash = new Hashtable();
			_hierarchyNodeHash = new Hashtable();
			_KeyTypeHash = null;
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
				return eProfileType.OnHand;
			}
		}

		//=========
		// METHODS
		//=========
		#region SetOnHandScope
		/// <summary>
		/// Sets the OnHand scope (pre-reads OnHand for a given group of days, merchandise by total, color, color-size or size).
		/// </summary>
		/// <param name="aHierarchyNodeList">Profile list of merchandise hierarchy nodes for which OnHand is to be set.</param>
		/// <param name="aOnHandBy">OnHand detail scope: Total, Color, Color-Size or Size.</param>
		/// <param name="aColorRIDList">List of color RIDs for which OnHand is to be set (ignored when scope is Total or Size).</param>
		/// <param name="aSizeRIDList">List of size RIDs for which OnHand is to be set (ignored when scope is Total or Color).</param>
		/// <remarks>Used to pre-read OnHand from the database.  Not required.</remarks>
		public void SetOnHandScope(
			ProfileList aHierarchyNodeList, 
			eIntransitBy aOnHandBy,
			ArrayList aColorRIDList, 
			ArrayList aSizeRIDList)
		{
			IntransitKeyType ikt;
			ArrayList IntransitKeyTypeList = new ArrayList();
			switch (aOnHandBy)
			{
				case(eIntransitBy.Total):
				{
					ikt = new IntransitKeyType(0, 0);
					IntransitKeyTypeList.Add(ikt);
					break;
				}
				case (eIntransitBy.Color):
				{
					foreach (int cRID in aColorRIDList)
					{
						ikt = new IntransitKeyType(cRID, 0);
						IntransitKeyTypeList.Add(ikt);
					}
					break;
				}
				case (eIntransitBy.SizeWithinColors):
				{
					foreach (int cRID in aColorRIDList)
					{
						foreach (int sRID in aSizeRIDList)
						{
							ikt = new IntransitKeyType(cRID, sRID);
							IntransitKeyTypeList.Add(ikt);
						}
					}
					break;
				}
				case (eIntransitBy.Size):
				{
					foreach (int sRID in aSizeRIDList)
					{
						ikt = new IntransitKeyType(0, sRID);
						IntransitKeyTypeList.Add(ikt);
					}
					break;
				}
				default:
				{
					ikt = new IntransitKeyType(0,0);
					IntransitKeyTypeList.Add(ikt);
					break;
				}
			}
			SetOnHandScope(
				aHierarchyNodeList,
				IntransitKeyTypeList);
		}

		/// <summary>
		/// Sets the OnHand scope (pre-reads OnHand for a given group of days, merchandise and IntransitKeyTypes).
		/// </summary>
		/// <param name="aHierarchyNodeList">Profile list of merchandise hierarchy nodes for which OnHand is to be set.</param>
		/// <param name="aOnHandKeyTypeList">ArrayList of OnHand detail scopes to pre-read.</param>
		/// <remarks>Used to pre-read OnHand from the database.  Not required.</remarks>
		public void SetOnHandScope(
			ProfileList aHierarchyNodeList,
			ArrayList aOnHandKeyTypeList)
		{
			LoadOnHand(
				aHierarchyNodeList.ArrayList,
				aOnHandKeyTypeList);
		}
		#endregion SetOnHandScope

		#region GetCurrentOnHand
		#region StoreTotalCurrentOnHand
		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the total OnHand for the specified stores for the specified day aggregated across the specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aHnProfileList">Merhchandise Hierarchy Node List for which OnHand is desired.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>Total OnHand value for each store aggregated across the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfileList,
				aStoreList.ArrayList);
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand

		/// <summary>
		/// Gets the total OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>Total OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfile, 
				aStoreList.ArrayList);
		}

		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			ArrayList aStoreRID_ArrayList)
		{
			int[] aHnRIDArray = new int[aHnProfileList.Count];
			for (int i=0; i<aHnProfileList.Count; i++)
			{
				HierarchyNodeProfile hnp = (HierarchyNodeProfile)aHnProfileList.ArrayList[i];
				aHnRIDArray[i] = hnp.Key;
			}
			return GetCurrentOnHand(
				aHnRIDArray,
				aStoreRID_ArrayList);
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the total OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Total OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentOnHand(
				aHnProfile.HierarchyRID, 
				aStoreRID_ArrayList);
		}

		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the total OnHand for the specified stores for the sepecified day aggregated across the specified merchandise hierarchy nodes
		/// </summary>
		/// <param name="aMdseHnRIDArray"></param>
		/// <param name="aStoreRID_ArrayList"></param>
		/// <returns></returns>
		public int[] GetCurrentOnHand(
			int[] aMdseHnRIDArray,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			//Begin TT#918 - JScott - On-Hand Reader bug, not calculating Set and All store Totals correctly
			// for (int s=0; s < aMdseHnRIDArray.Length; s++)
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			//End TT#918 - JScott - On-Hand Reader bug, not calculating Set and All store Totals correctly
			{
				for (int m=0; m<aMdseHnRIDArray.Length; m++)
				{
					storeOnHand[s] +=
						GetCurrentOnHand((int)aMdseHnRIDArray[m], (int)aStoreRID_ArrayList[s]);
				}
			}
			return storeOnHand;
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the total OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Total OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			int aMdseHnRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeOnHand[s] = 
					GetCurrentOnHand(aMdseHnRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeOnHand;
		}
	
		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the total OnHand for the specified stores for the specified day aggregated across the specified merchandise hierarchy nodes.
		/// </summary>
		/// <param name="aMdseHnRID_Array">Array of Mdse Hierarchy Node RIDs for which onhand is to be accumulated</param>
		/// <param name="aStoreList">Profile List of stores</param>
		/// <returns>Total onhand for the specified stores for the specified day aggregated across the specified merchandise hierarchy nodes</returns>
		public int[] GetCurrentOnHand(
			int[] aMdseHnRID_Array,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aMdseHnRID_Array,
				aStoreList.ArrayList);
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the total OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aStoreList">ProfileList of Stores for which OnHand is desired.</param>
		/// <returns>Total OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			int aMdseHnRID,
			ProfileList aStoreList)
		{
			int[] storeOnHand = new int[aStoreList.Count];
			storeOnHand.Initialize();
			for (int s=0; s < aStoreList.Count; s++)
			{
				storeOnHand[s] = 
					GetCurrentOnHand(aMdseHnRID, aStoreList[s].Key);
			}
			return storeOnHand;
		}

		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store total OnHand aggregated across the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfileList">HierarchyNodeProfileList that describes Merchandise hierarchy nodes for which OnHand is desired.</param>
		/// <param name="aStoreProfile">StoreProfile for the store</param>
		/// <returns>Store total OnHand accumulated value across the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			HierarchyNodeList aMdseHnProfileList,
			StoreProfile aStoreProfile)
		{
			int storeOnHand = 0;
			foreach (HierarchyNodeProfile hnp in aMdseHnProfileList)
			{
				storeOnHand += GetCurrentOnHand(hnp, aStoreProfile);
			}
			return storeOnHand;
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store total OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfile">HierarchyNodeProfile that describes Merchandise hierarchy node for which OnHand is desired.</param>
		/// <param name="aStoreProfile">StoreProfile for the store</param>
		/// <returns>Store total OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			HierarchyNodeProfile aMdseHnProfile,
			StoreProfile aStoreProfile)
		{
			return GetCurrentOnHand(
				aMdseHnProfile.HierarchyRID,
				aStoreProfile.Key);
		}

		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store total OnHand for the specified day aggregated across the specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aMdseHnRIDs">Array of Merchandise Hierarchy Node RIDs</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Store total OnHand aggregated value across the specified merchandise RIDs on the specified day</returns>
		public int GetCurrentOnHand(
			int[] aMdseHnRIDs,
			int aStoreRID)
		{
			int storeOnHand = 0;
			for (int i=0; i<aMdseHnRIDs.Length; i++)
			{
				storeOnHand += GetCurrentOnHand(aMdseHnRIDs[i], aStoreRID);
			}
			return storeOnHand;
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store total OnHand for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">RID for Merhchandise hierarchy node for which OnHand is desired.</param>
		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store total OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			int aMdseHnRID,
			int aStoreRID)
		{
			IntransitKeyType ikt = new IntransitKeyType(0, 0);
			return GetCurrentOnHand(
				aMdseHnRID,
				ikt,
				aStoreRID);
		}
		#endregion StoreTotalDayOnHand
		 
		#region StoreColorDayOnHand
		// begin MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color OnHand for the specified stores for the specified day aggregated across the specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aHnProfileList">Hierarchy Node List describing the merchandise hierarchy nodes</param>
		/// <param name="aColorRID">Color code RID</param>
		/// <param name="aStoreList">Profile List of the stores for which OnHand is desired.</param>
		/// <returns></returns>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			int aColorRID,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfileList,
				aColorRID,
                aStoreList.ArrayList);
		}
		// end MID Track 4309 OTS basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>Color OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfile, 
				aColorRID, 
				aStoreList.ArrayList);
		}

		// begin MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color OnHand for the specified stores for the specified day aggregated across specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aHnProfileList">Hierarchy Node List of merchandise for which color onhand is required</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aStoreRID_ArrayList">Array List of store profiles for which color onhand is required</param>
		/// <returns>Color OnHand for the specified stores for the specified day aggregated across specified merchandise hierarchy nodes</returns>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			int aColorRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] hnRIDs = new int[aHnProfileList.Count];
			for (int i=0; i<aHnProfileList.Count; i++)
			{
				HierarchyNodeProfile hnp = (HierarchyNodeProfile)aHnProfileList.ArrayList[i];
				hnRIDs[i] = hnp.Key;
			}
            return GetCurrentOnHand(
				hnRIDs,
				aColorRID,
                aStoreRID_ArrayList);
		}
		// end MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentOnHand(
				aHnProfile.HierarchyRID, 
				aColorRID, 
				aStoreRID_ArrayList);
		}

		// begin MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRIDs">Array of Merchandise Hierarchy Node RIDs</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aStoreRID_ArrayList">Array List of store RIDs</param>
		/// <returns></returns>
		public int[] GetCurrentOnHand(
			int[] aMdseHnRIDs,
			int aColorRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s<aStoreRID_ArrayList.Count; s++)
			{
				for (int m=0; m<aMdseHnRIDs.Length; m++)
				{
					storeOnHand[s] +=GetCurrentOnHand(
						aMdseHnRIDs[m], aColorRID, (int)aStoreRID_ArrayList[s]);
				}
			}
			return storeOnHand;
		}
		// end MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>		
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			int aMdseHnRID,
			int aColorRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeOnHand[s] = 
					GetCurrentOnHand
					(aMdseHnRID, aColorRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeOnHand;
		}
	
		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color OnHand total aggreagated across the specified merchandise hierarchy nodes on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfileList">Profile List of Hierarchy Node Profiles</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aStoreProfile">Profile List of stores</param>
		/// <returns>Store Color OnHand total aggregated across the specified hierarchy nodes on the specified day</returns>
		public int GetCurrentOnHand(
			HierarchyNodeList aMdseHnProfileList,
			int aColorRID,
			StoreProfile aStoreProfile)
		{
			int storeOnHand = 0;
			foreach (HierarchyNodeProfile hnp in aMdseHnProfileList)
			{
				storeOnHand += GetCurrentOnHand(
					hnp, aColorRID, aStoreProfile);
			}
			return storeOnHand;
		}
		// end MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfile">Merchandise hierarchy node profile that describes the merchandise for which OnHand is desired.</param>
		/// <param name="aColorRID">RID of the color</param>
		/// <param name="aStoreProfile">StoreProfile that describes the store</param>
		/// <returns>Store color OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			HierarchyNodeProfile aMdseHnProfile,
			int aColorRID,
			StoreProfile aStoreProfile)
		{
			return GetCurrentOnHand(
				aMdseHnProfile,
				aColorRID,
				0,
				aStoreProfile);
		}

		// begin MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color OnHand accumulated across the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnRIDs">Array of Merchandise Hierarchy Node RIDs</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Store color OnHand accumulated across the specified merchandise hierarchy nodes on the specified day</returns>
		public int GetCurrentOnHand(
			int[] aMdseHnRIDs,
			int aColorRID,
			int aStoreRID)
		{
			int storeOnHand = 0;
			foreach (int mhRID in aMdseHnRIDs)
			{
				storeOnHand += GetCurrentOnHand
					(mhRID, aColorRID, aStoreRID);
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID.</param>
		/// <param name="aColorRID">RID of the color</param>
		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store color OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			int aMdseHnRID,
			int aColorRID,
			int aStoreRID)
		{
			return GetCurrentOnHand(
				aMdseHnRID,
				aColorRID,
				0,
				aStoreRID);
		}
		#endregion StoreColorDayOnHand

		#region StoreColorSizeDayOnHand
		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the specified day accumulated across the specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aHnProfileList">Hierarchy Node List</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aSizeRID">Size Code RID</param>
		/// <param name="aStoreList">Store Profile List</param>
		/// <returns>The Color-size OnHand for each of the specified stores for the specified day accumulated across the specified merchandise hierarchy nodes</returns>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			int aColorRID,
			int aSizeRID,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfileList,
				aColorRID,
				aSizeRID,
                aStoreList.ArrayList);
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>Total OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			int aSizeRID,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfile, 
				aColorRID, 
				aSizeRID, 
				aStoreList.ArrayList);
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the specified day accumulated across the specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aHnProfileList">Hierarchy Node List</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aSizeRID">Size Code RID</param>
		/// <param name="aStoreRID_ArrayList">Array of Store RIDs</param>
		/// <returns>The color-size OnHand for teh specified stores for the specified day accumulated across the specified merchandise hierarchy nodes</returns>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] hnRIDs = new int[aHnProfileList.Count];
			for (int m=0; m<aHnProfileList.Count; m++)
			{
				HierarchyNodeProfile hnp = (HierarchyNodeProfile)aHnProfileList[m];
				hnRIDs[m] = hnp.Key;
			}
			return GetCurrentOnHand
				(hnRIDs, aColorRID, aSizeRID, aStoreRID_ArrayList);
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color-size OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentOnHand(
				aHnProfile.HierarchyRID,
				aColorRID,  
				aSizeRID,
				aStoreRID_ArrayList);
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the specified day accumulated across the specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aMdseHnRIDs">Array of Merchandise Hierarchy Nodes</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aSizeRID">Size Code RID</param>
		/// <param name="aStoreRID_ArrayList">Array List of Store RIDs</param>
		/// <returns>Color-Size OnHand for each of the specified stores for the specified day accumulated across the specified merchandise hierarchy nodes.</returns>
		public int[] GetCurrentOnHand(
			int[] aMdseHnRIDs,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s<aStoreRID_ArrayList.Count; s++)
			{
				for (int m=0; m<aMdseHnRIDs.Length; m++)
				{
					storeOnHand[s] += 
						GetCurrentOnHand(aMdseHnRIDs[m], aColorRID, aSizeRID, (int)aStoreRID_ArrayList[s]);
				}
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the color-size OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color-size OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			int aMdseHnRID,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeOnHand[s] = 
					GetCurrentOnHand
					(aMdseHnRID, aColorRID, aSizeRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeOnHand;
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color-size OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfileList">Hierarchy Node List</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aSizeRID">Size Code RID</param>
		/// <param name="aStoreProfile">Store Profile that identifies the store</param>
		/// <returns></returns>
		public int GetCurrentOnHand(
			HierarchyNodeList aMdseHnProfileList,
			int aColorRID,
			int aSizeRID,
			StoreProfile aStoreProfile)
		{
			int storeOnHand = 0;
			foreach (HierarchyNodeProfile hnp in aMdseHnProfileList)
			{
				storeOnHand += GetCurrentOnHand(
					hnp,
					aColorRID,
					aSizeRID,
					aStoreProfile);
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color-size OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfile">Merhchandise hierarchy node profile that describes the merchandise for which OnHand is desired.</param>
		/// <param name="aColorRID">RID of the color</param>
		/// <param name="aSizeRID">RID of the size</param>
		/// <param name="aStoreProfile">Store Profile that describes the store for which OnHand is desired.</param>
		/// <returns>Store color-size OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			HierarchyNodeProfile aMdseHnProfile, 
			int aColorRID, 
			int aSizeRID,
			StoreProfile aStoreProfile)
		{
			return GetCurrentOnHand (
				aMdseHnProfile.HierarchyRID, 
				aColorRID, 
				aSizeRID, 
				aStoreProfile.Key);
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color-size OnHand accumulated across the specified merchandise hierarchy nodes on the specified day. 
		/// </summary>
		/// <param name="aMdseHnRIDs">Array of Merchandise Hierarchy Node RIDs</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aSizeRID">Size Code RID</param>
		/// <param name="aStoreRID">Store RID</param>
		/// <returns>Store color-size OnHand accumulated across the specified merchandise hierarchy nodes on the specified day.</returns>
		public int GetCurrentOnHand(
			int[] aMdseHnRIDs,
			int aColorRID,
			int aSizeRID,
			int aStoreRID)
		{
			IntransitKeyType IntransitKeyType = new IntransitKeyType(aColorRID, aSizeRID);
			int storeOnHand = 0;
			foreach (int mRID in aMdseHnRIDs)
			{
				storeOnHand += GetCurrentOnHand(mRID, IntransitKeyType, aStoreRID);
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color-size OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnRID">RID of the merchandise hierarchy node.</param>
		/// <param name="aColorRID">RID for the Color.</param>
		/// <param name="aSizeRID">RID for the Size.  </param>
		/// <param name="aStoreRID">RID for the store.</param>
		/// <returns>Store color-size OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			int aMdseHnRID,
			int aColorRID,
			int aSizeRID,
			int aStoreRID)
		{
			IntransitKeyType IntransitKeyType = new IntransitKeyType(aColorRID, aSizeRID);
			return GetCurrentOnHand(
				aMdseHnRID, 
				IntransitKeyType, 
				aStoreRID);
		}
		#endregion StoreColorSizeDayOnHand

		#region StoreDayOnHandBy
		// begin MID Track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the specified OnHand for the specified stores for the specified day accumulated across the specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfileList">Hierarchy Node List that idetifies merchandise for which accumulated onhand is desired</param>
		/// <param name="aOnHandBy">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">Color Code RID (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">Size Code RID (ignored when total or color OnHand requested.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>OnHand value for each store accumulated across the specified merchandise on the specified day.</returns>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfileList,
				aOnHandBy,
				aColorRID,
				aSizeRID,
				aStoreList.ArrayList);
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the specified OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aOnHandBy">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">RID for the size (ignored when total or color OnHand requested.</param>
		/// <param name="aStoreList">ProfileList of StoreProfiles for which OnHand is desired.</param>
		/// <returns>OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the associated ArrayList in the store ProfileList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			ProfileList aStoreList)
		{
			return GetCurrentOnHand(
				aHnProfile, 
				aOnHandBy, 
				aColorRID, 
				aSizeRID, 
				aStoreList.ArrayList);
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the specified OnHand for the specified stores for the specified day accumulated across specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aHnProfileList">Hierarchy Node List that identifies the merchandise over which to accumulate onhand</param>
		/// <param name="aOnHandBy">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">Color Code RID</param>
		/// <param name="aSizeRID">Size Code RID</param>
		/// <param name="aStoreRID_ArrayList">Array List of store RIDs</param>
		/// <returns>Specified onhand for each of the specified stores for the specified day accumulated across the specified hierarchy nodes.</returns>
		public int[] GetCurrentOnHand(
			HierarchyNodeList aHnProfileList,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] hnRIDs = new int[aHnProfileList.Count];
			for (int m=0; m<aHnProfileList.Count; m++)
			{
				HierarchyProfile hnp = (HierarchyProfile)aHnProfileList[m];
				hnRIDs[m] = hnp.Key;
			}
			return GetCurrentOnHand(
				hnRIDs, aOnHandBy, aColorRID, aSizeRID, aStoreRID_ArrayList);
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the specified OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aHnProfile">Merhchandise hierarchy node profile for which OnHand is desired.</param>
		/// <param name="aOnHandBy">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>Color OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			HierarchyNodeProfile aHnProfile,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			return GetCurrentOnHand(
				aHnProfile.HierarchyRID, 
				aOnHandBy, 
				aColorRID, 
				aSizeRID, 
				aStoreRID_ArrayList);
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the specified OnHand for the specified stores for the specified day accumulated across specified merchandise hierarchy nodes. 
		/// </summary>
		/// <param name="aMdseHnRIDs">Hierarchy Node RID array</param>
		/// <param name="aOnHandBy">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>OnHand value for each store accumulated across the specified merchandise on the specified day.</returns>
		public int[] GetCurrentOnHand(
			int[] aMdseHnRIDs,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				for (int m=0; m<aMdseHnRIDs.Length; m++)
				{
					storeOnHand[s] += 
						GetCurrentOnHand
						(aMdseHnRIDs[m], aOnHandBy, aColorRID, aSizeRID, (int)aStoreRID_ArrayList[s]);
				}
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the specified OnHand for the specified stores for the specified day in specified merchandise hierarchy node. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID for which OnHand is desired.</param>
		/// <param name="aOnHandBy">Identifies the desired OnHand scope (total, color, color-size or size).</param>
		/// <param name="aColorRID">RID for the color.</param>
		/// <param name="aSizeRID">RID for the size.</param>
		/// <param name="aStoreRID_ArrayList">ArrayList of StoreRIDs for which OnHand is desired.</param>
		/// <returns>OnHand value for each store for the specified merchandise on the specified day.</returns>
		/// <remarks>OnHand values are returned in the same order as the stores in the ArrayList</remarks>
		public int[] GetCurrentOnHand(
			int aMdseHnRID,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			ArrayList aStoreRID_ArrayList)
		{
			int[] storeOnHand = new int[aStoreRID_ArrayList.Count];
			storeOnHand.Initialize();
			for (int s=0; s < aStoreRID_ArrayList.Count; s++)
			{
				storeOnHand[s] = 
					GetCurrentOnHand
					(aMdseHnRID, aOnHandBy, aColorRID, aSizeRID, (int)aStoreRID_ArrayList[s]);
			}
			return storeOnHand;
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store specified OnHand accumulated across the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfileList">Hierarchy Node Profile List for which onhand is to be accumulated</param>
		/// <param name="aOnHandBy">eIntransitBy value that describes the desired OnHand detail.</param>
		/// <param name="aColorRID">Color Code RID (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">Size Code RID (ignored when total or color OnHand requested).</param>
		/// <param name="aStoreProfile">StoreProfile that describes the store</param>
		/// <returns>Store specified OnHand value accumulated across the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			HierarchyNodeList aMdseHnProfileList,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			StoreProfile aStoreProfile)
		{
			int storeOnHand = 0;
			foreach (HierarchyNodeProfile hnp in aMdseHnProfileList)
			{
				storeOnHand += GetCurrentOnHand(
					hnp.Key,
					aOnHandBy,
					aColorRID,
					aSizeRID,
					aStoreProfile.Key);
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store specified OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnProfile">Merhchandise hierarchy node profile that describes the merchandise for which OnHand is desired.</param>
		/// <param name="aOnHandBy">eIntransitBy value that describes the desired OnHand detail.</param>
		/// <param name="aColorRID">RID of the color (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">RID of the size (ignored when total or color OnHand requested).</param>
		/// <param name="aStoreProfile">StoreProfile that describes the store</param>
		/// <returns>Store specified OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			HierarchyNodeProfile aMdseHnProfile,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			StoreProfile aStoreProfile)
		{
			return GetCurrentOnHand(
				aMdseHnProfile.HierarchyRID,
				aOnHandBy,
				aColorRID,
				aSizeRID,
				aStoreProfile.Key);
		}

		// begin MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color OnHand accumulated across the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnRIDs">List of Hierarchy Node RIDs for which an accumulated onhand is desired</param>
		/// <param name="aOnHandBy">eIntransitBy value that describes the desired OnHand detail.</param>
		/// <param name="aColorRID">Color Code RID (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">Size Code RID (ignored when total or color OnHand requested).</param>		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store color size OnHand value accumulated across the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			int[] aMdseHnRIDs,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			int aStoreRID)
		{
			int storeOnHand = 0;
			foreach (int m in aMdseHnRIDs)
			{
				storeOnHand += GetCurrentOnHand(m, aOnHandBy, aColorRID, aSizeRID, aStoreRID);
			}
			return storeOnHand;
		}
		// end MID track 4309 OTS Basis onhand is not sum of basis onhand
		/// <summary>
		/// Gets the store color OnHand for the specified merchandise hierarchy node on the specified day. 
		/// </summary>
		/// <param name="aMdseHnRID">Merhchandise hierarchy node RID that describes the merchandise for which OnHand is desired.</param>
		/// <param name="aOnHandBy">eIntransitBy value that describes the desired OnHand detail.</param>
		/// <param name="aColorRID">RID of the color (ignored when total OnHand requested).</param>
		/// <param name="aSizeRID">RID of the size (ignored when total or color OnHand requested).</param>		/// <param name="aStoreRID">RID for the store</param>
		/// <returns>Store color OnHand value for the specified merchandise on the specified day.</returns>
		public int GetCurrentOnHand(
			int aMdseHnRID,
			eIntransitBy aOnHandBy,
			int aColorRID,
			int aSizeRID,
			int aStoreRID)
		{
			IntransitKeyType ikt;
			switch (aOnHandBy)
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
			return GetCurrentOnHand(
				aMdseHnRID,
				ikt,
				aStoreRID);
		}
		#endregion StoreDayOnHandBy

		#region StoreKeyTypeCurrentOnHand
		/// <summary>
		/// Gets store IntransitKeyType OnHand value for the merchandise hierarchy node on the specified day.
		/// </summary>
		/// <param name="aMdseHnRID">RID of merchandise hierarchy node.</param>
		/// <param name="aOnHandKeyType">IntransitKeyType that describes the type of OnHand (total, color, size or color-size)</param>
		/// <param name="aStoreRID">RID of the store for which OnHand is desired.</param>
		/// <returns>Store OnHandKeyType OnHand value for the specified merchandise hierarchy node on the specified day</returns>
		private int GetCurrentOnHand(
			int aMdseHnRID,
			IntransitKeyType aOnHandKeyType,
			int aStoreRID)
		{
			if (!_hierarchyNodeHash.Contains(aMdseHnRID))
			{
				LoadOnHand (
					aMdseHnRID,
					aOnHandKeyType);
			}
			_KeyTypeHash = (Hashtable)_hierarchyNodeHash[aMdseHnRID];
			if (!_KeyTypeHash.Contains(aOnHandKeyType.IntransitTypeKey))
			{
				LoadOnHand (
					aMdseHnRID,
					aOnHandKeyType);
			}
			_storeHash = (Hashtable)_KeyTypeHash[aOnHandKeyType.IntransitTypeKey];
			if (_storeHash.Contains(aStoreRID))
			{
				return (int)_storeHash[aStoreRID];
			}
			return 0;
		}
		#endregion StoreKeyTypeCurrentOnHand
		#endregion GetCurrentOnHand

		#region LoadOnHand
		/// <summary>
		/// Loads OnHand from the database.
		/// </summary>
		/// <param name="aMdseHnRID">RID for the merchandise hierarchy node to load.</param>
		/// <param name="aColorRID">RID for the color to load.</param>
		/// <param name="aSizeRID">RID for the size to load.</param>
		/// <remarks>
		/// To get "total store" OnHand, both color and size RIDs must be zero.  When color
		/// RID is zero and SizeRID is not, a store's total size OnHand across all colors in 
		/// the requested merchandise hierarchy nodes will be loaded.  When size RID is zero and 
		/// ColorRID is not, a store's total color OnHand across all sizes will be loaded.  
		/// </remarks>
		private void LoadOnHand (
			int aMdseHnRID,
			int aColorRID,
			int aSizeRID)
		{
			IntransitKeyType ikt = new IntransitKeyType(aColorRID, aSizeRID);
			LoadOnHand(
				aMdseHnRID,
				ikt);
		}

		/// <summary>
		/// Loads OnHand for the specified day, merchandise hierarchy node and OnHand key type.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise Hierarchy Node for which store OnHand is desired.</param>
		/// <param name="aOnHandKeyType">OnHand key type for which store OnHand is desired.</param>
		private void LoadOnHand (
			int aMdseHnRID,
			IntransitKeyType aOnHandKeyType)
		{
			ArrayList hnArray = new ArrayList();
			hnArray.Add(aMdseHnRID);
			ArrayList keyTypeArray = new ArrayList();
			keyTypeArray.Add(aOnHandKeyType);
			LoadOnHand(hnArray, keyTypeArray);   
		}

		/// <summary>
		/// Loads store OnHand for the specified days, merchandise hierarchy nodes and OnHand key types.
		/// </summary>
		/// <param name="aMdseHnRID">ArrayList of merchandise hierarchy nodes for which OnHand is desired.</param>
		/// <param name="aOnHandKeyType">ArrayList of IntransitKeyTypes for which OnHand is desired.</param>
		private void LoadOnHand(
			ArrayList aMdseHnRID,
			ArrayList aOnHandKeyType)
		{
			int[,,] storeOHvalue = GetOh(				
				aMdseHnRID,
				aOnHandKeyType,
				_allStoreList.ArrayList);// MID Change j.ellis "onhand" hard coded for "regular" inventory
				// eOnHandVariable.InventoryRegular);	// TO DO -- hardcoded to regular // MID Change j.ellis "onhand" hard coded for "regular" inventory
			IntransitKeyType ikt;
			for (int h=0; h < aMdseHnRID.Count; h++)
			{
				if (_hierarchyNodeHash.Contains(aMdseHnRID[h]))
				{
					_KeyTypeHash = (Hashtable)_hierarchyNodeHash[aMdseHnRID[h]];
				}
				else
				{
					_KeyTypeHash = new Hashtable();
					_hierarchyNodeHash.Add(aMdseHnRID[h], _KeyTypeHash);
				}
				for(int i=0; i < aOnHandKeyType.Count; i++)
				{
					ikt = (IntransitKeyType)aOnHandKeyType[i];
					if (_KeyTypeHash.Contains(ikt.IntransitTypeKey))
					{
						_KeyTypeHash.Remove(ikt.IntransitTypeKey);
					}
					_storeHash = new Hashtable();
					_KeyTypeHash.Add(ikt.IntransitTypeKey, _storeHash);
					for (int s=0; s < _allStoreList.Count; s++)
					{
						StoreProfile sp = (StoreProfile)_allStoreList.ArrayList[s];
						_storeHash.Add(sp.Key, storeOHvalue[h, i, s]);
					}
				}
			}
		}
	



		/// <summary>
		/// Gets store OnHand for the specified times, hierarchy nodes, key types and stores.
		/// </summary>
		/// <param name="aHnRIDList">ArrayList of Merchandise Hierarchy Node RIDs for which store OnHand is desired.</param>
		/// <param name="aOnHandKeyTypeList">ArrayList of IntransitKeyTypes for which store OnHand is desired.</param>
		/// <param name="aStoreRIDList">ArrayList of store RIDs for which OnHand is to be retrieved.</param>
		/// <returns>3-dimensional array of accumulated OnHand values. Dimension 1 = hnRIDList, dimension 2 = IntransitKeyTypeList, dimension 3 = storeRIDList</returns>
		private int[,,] GetOh(
			ArrayList aHnRIDList,
			ArrayList aOnHandKeyTypeList,
			ArrayList aStoreRIDList)
			// eOnHandVariable aOnHandVariable)
		{
			//string[] onHandColumn;  // MID change J.Ellis Total OnHand is zero
			ProfileList onHandProfileList; // MID change J.Ellis Total OnHand is zero
			//Begin Track #4637 - JSmith - Split variables by type
			ProfileList dailyOnHandProfileList = new ProfileList(eProfileType.Variable);
			ProfileList weeklyOnHandProfileList = new ProfileList(eProfileType.Variable); 
			//End Track #4637
			// BEGIN MID Change j.ellis "onhand" hard coded for "regular" inventory
            // eOnHandVariable onHandVariable;  // MID Track 3312 Total OnHand incorrect

			//if (aOnHandVariable == eOnHandVariable.InventoryTotal) 
			//{
			//	onHandColumn = _transaction.Variables.InventoryTotalUnitsVariable.DatabaseColumnName;
			//}
			//else
			//{
			//	onHandColumn = _transaction.Variables.InventoryRegularUnitsVariable.DatabaseColumnName;
			//}
			// END MID Change j.ellis "onhand" hard coded for "regular" inventory

			int[,,] OnHandValues = 
				new int [aHnRIDList.Count, aOnHandKeyTypeList.Count, aStoreRIDList.Count];
			OnHandValues.Initialize();

            // Begin TT#5124 - JSmith - Performance
            //VariablesData dbOnHand = new VariablesData();
            VariablesData dbOnHand = new VariablesData(_transaction.SAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
            // End TT#5124 - JSmith - Performance
            
			for (int h=0; h < aHnRIDList.Count; h++)
			{
				int hnRID = (int)aHnRIDList[h];
				HierarchyNodeProfile hnp = _transaction.GetNodeData(hnRID);

				if (hnp.Key == -1)
				{
					throw new MIDException(eErrorLevel.severe, 0, "Hierarchy node not found");
				}
				// BEGIN MID Change j.ellis "onhand" hard coded for "regular" inventory
				if (eOTSPlanLevelType.Regular == hnp.OTSPlanLevelType)
				{
					onHandProfileList = new ProfileList(eProfileType.Variable);
					onHandProfileList.Add(_transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable); // MID Change J.ellis Total OnHand zero
					// onHandColumn[0] = _transaction.Variables.InventoryRegularUnitsVariable.DatabaseColumnName; // MID Track 3312 Total OnHand incorrect
					// onHandVariable = eOnHandVariable.InventoryRegular; // MID Track 3312 Total OnHand Incorrect
				}
				else
				{
					onHandProfileList = _transaction.PlanComputations.PlanVariables.GetTotalOnHandVariableList(); // MID Change J.ellis Total OnHand zero
					//onHandColumn = new string[2]; // MID Track 3312 Total OnHand incorrect
					//onHandColumn[0] = _transaction.Variables.InventoryRegularUnitsVariable.DatabaseColumnName; // MID Track 3312 Total OnHand incorrect
					//onHandColumn[1] = _transaction.Variables.InventoryMarkdownUnits.DatabaseColumnName; // MID Track 3312 Total OnHand incorrect
					//onHandColumn = _transaction.Variables.InventoryTotalUnits.DatabaseColumnName;   // MID Track 3312 Total OnHand incorrect
					//onHandVariable = eOnHandVariable.InventoryTotal; // MID Track 3312 Total OnHand Incorrect
				}
				// END MID Change j.ellis "onhand" hard coded for "regular" inventory

				//Begin Track #4637 - JSmith - Split variables by type
				// Determine if variables are saved by day, week or both
				foreach (VariableProfile vp in onHandProfileList)
				{
					if (vp.isDatabaseVariable(eVariableCategory.Store, Include.FV_ActualRID, eCalendarDateType.Day))
					{
						dailyOnHandProfileList.Add(vp);
					}
					if (vp.isDatabaseVariable(eVariableCategory.Store, Include.FV_ActualRID, eCalendarDateType.Week))
					{
						weeklyOnHandProfileList.Add(vp);
					}
				} 
				//End Track #4637

				for (int k=0; k < aOnHandKeyTypeList.Count; k++)
				{
					IntransitKeyType ikt = (IntransitKeyType)aOnHandKeyTypeList[k];
					switch (ikt.IntransitType)
					{
						case(eIntransitBy.Total):
						{
							//Begin Track #4637 - JSmith - Split variables by type
//							DataTable dt = dbOnHand.GetOhByTimeByNode
//								(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnp.Key, onHandProfileList); // MID Change j.ellis "onhand" hard coded for "regular" inventory
                            //Begin TT#1684 - JSmith - Showing incorrect on hand data
                            //DataTable dt = dbOnHand.GetOhByTimeByNode
                            //    (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnp.Key, dailyOnHandProfileList, weeklyOnHandProfileList);
                            DataTable dt = dbOnHand.GetOhByTimeByNode
                                (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnp.Key, dailyOnHandProfileList, weeklyOnHandProfileList, _transaction.SAB.ApplicationServerSession.DailyOnhandExists, hnp.LevelType);
                            //End TT#1684
							//End Track #4637
							//	(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnp.Key, onHandVariable); // MID Change j.ellis "onhand" hard coded for "regular" inventory
							//Begin Track #4637 - JSmith - Split variables by type
							if (dt != null)
							{
								//End Track #4637
								foreach(DataRow dr in dt.Rows)
								{
									// begin MID Track 5953 Null Reference when executing General Method
									//Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
									Index_RID storeIdxRID = _transaction.GetStoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
									// end MID Track 5953 Null Reference when executing General Method
									if (storeIdxRID.RID != Include.UndefinedStoreRID)
									{
										// BEGIN MID Track 3312 Total OnHand incorrect
										// BEGIN MID Change J.ellis Total OnHand zero
										foreach (VariableProfile vp in onHandProfileList) 
										{
											OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
										}
										//for (int i=0; i < onHandColumn.Length; i++) 
										//{
										//OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[onHandColumn[i]], CultureInfo.CurrentUICulture);
										//}
										// END MID Change J.ellis Total OnHand zero
										// END MID Track 3312 Total OnHand incorrect

									}
									else
									{
										// store not in list -- must have been added.
									}
								}
							//Begin Track #4637 - JSmith - Split variables by type
							}
							//End Track #4637
//							if ((hnp.LevelType == eHierarchyLevelType.Style) || (hnp.IsParentOfStyle))
//							{
//								// node is at style or "parent of style" level, so don't have to worry about descendants
//								DataTable dt = dbOnHand.GetOhByTimeByNode
//									(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, hnp.Key, aOnHandVariable);
//								foreach(DataRow dr in dt.Rows)
//								{
//									Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
//									if (storeIdxRID.RID != Include.UndefinedStoreRID)
//									{
//										OnHandValues[h,k,storeIdxRID.Index] = Convert.ToInt32(dr["UNITS"], CultureInfo.CurrentUICulture);
//									}
//									else
//									{
//										// store not in list -- must have been added.
//									}
//								}
//							}
//							else
//							{
//								// TODO -- for performance, change the following call to get the "parent of style" descendants
//								// this level is not defined at this time.
//								HierarchyNodeList hnlTotal = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Style);
//								foreach(HierarchyNodeProfile hnpTotal in hnlTotal)
//								{
//									// add up, for descendants, for store list
//									DataTable dt = dbOnHand.GetOhByTimeByNode
//										(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, hnpTotal.Key, aOnHandVariable);
//									foreach(DataRow dr in dt.Rows)
//									{
//										Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
//										if (storeIdxRID.RID != Include.UndefinedStoreRID)
//										{
//											OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr["UNITS"], CultureInfo.CurrentUICulture);
//										}
//										else
//										{
//											// store not in list -- must have been added
//										}
//									}
//								}
//
//							}
							break;
						}
						case (eIntransitBy.Color):
						{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
                            // begin MID Track 5338 Color Onhands zero (% Need Limit not observed)
							HierarchyNodeList hnlColor;
							if (hnp.LevelType == eHierarchyLevelType.Color)
							{
								hnlColor = new HierarchyNodeList(eProfileType.HierarchyNode);
								hnlColor.Add(hnp);
							}
							else if (hnp.LevelType == eHierarchyLevelType.Size)
							{
								hnlColor = new HierarchyNodeList(eProfileType.HierarchyNode);
                                // BEGIN MID Track #6077 - Alloc Review Analysis Only errors
                                //hnlColor.Add((HierarchyNodeProfile)hnp.Parents[0]);
                                int parentRID = (int)hnp.Parents[0];
                                HierarchyNodeProfile parentHnp = _transaction.GetNodeData(parentRID);
                                hnlColor.Add(parentHnp);
                                // END MID Track #6077
							}
							else

							{
								hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color, eNodeSelectType.All);
							}
							//HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color, eNodeSelectType.All); // MID Change j.ellis Performance--cache ancestor and descendant data
                            // end MID Track 5338 Color Onhands zero (%Need Limit not observed)
//							HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
							//HierarchyNodeList hnlColor = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
							foreach(HierarchyNodeProfile hnpColor in hnlColor)
							{
								if (hnpColor.ColorOrSizeCodeRID == ikt.ColorRID)
								{
									// color match -- add to this bucket, for store list
									//Begin Track #4637 - JSmith - Split variables by type
									//									DataTable dt = dbOnHand.GetOhByTimeByNode
									//										(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpColor.Key, onHandProfileList); // MID Track 3312 Total OnHand incorrect
                                    //Begin TT#1684 - JSmith - Showing incorrect on hand data
                                    //DataTable dt = dbOnHand.GetOhByTimeByNode
                                    //    (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpColor.Key, dailyOnHandProfileList, weeklyOnHandProfileList);
                                    DataTable dt = dbOnHand.GetOhByTimeByNode
                                        (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpColor.Key, dailyOnHandProfileList, weeklyOnHandProfileList, _transaction.SAB.ApplicationServerSession.DailyOnhandExists, hnpColor.LevelType);
                                    //End TT#1684
									//End Track #4637
									//	(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpColor.Key, onHandVariable); // MID Change j.ellis "onhand" hard coded for "regular" inventory
									//Begin Track #4637 - JSmith - Split variables by type
									if (dt != null)
									{
										//End Track #4637
										foreach(DataRow dr in dt.Rows)
										{
											// begin MID Track 5953 Null Reference when executing General Method
											//Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
											Index_RID storeIdxRID = _transaction.GetStoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
											// end MID Track 5953 Null Reference when executing General Method
											if (storeIdxRID.RID != Include.UndefinedStoreRID)
											{
												// Begin MID Track 3312 Total OnHand incorrect
												// Begin MID Change j.ellis total onhand zero
												foreach (VariableProfile vp in onHandProfileList)
												{
													OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
												}
												//for (int i=0; i<onHandColumn.Length; i++)
												//{
												//	OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[onHandColumn[i]], CultureInfo.CurrentUICulture);
												//}
												// END MID Change j.ellis total onhand zero
												// End MID Track 3312 Total OnHand incorrect
											}
											else
											{
												// store not in list -- must have been added
											}
										}
									//Begin Track #4637 - JSmith - Split variables by type
									}
									//End Track #4637
								}
							}
							break;
						}
						case (eIntransitBy.SizeWithinColors):
						{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
							// begin MID Track 5338 Color Onhands zero (% Need Limit Not observed)
							HierarchyNodeList hnlColor;
							if (hnp.LevelType == eHierarchyLevelType.Color)
							{
								hnlColor = new HierarchyNodeList(eProfileType.HierarchyNode);
								hnlColor.Add(hnp);
							}
							else if (hnp.LevelType == eHierarchyLevelType.Size)
							{
								hnlColor = new HierarchyNodeList(eProfileType.HierarchyNode);
                                // BEGIN MID Track #6077 - Alloc Review Analysis Only errors
                                //hnlColor.Add((HierarchyNodeProfile)hnp.Parents[0]);
                                int parentRID = (int)hnp.Parents[0];
                                HierarchyNodeProfile parentHnp = _transaction.GetNodeData(parentRID);
                                hnlColor.Add(parentHnp);
                                // END MID Track #6077
							}
							else

							{
								hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color, eNodeSelectType.All);
							}
							//HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color, eNodeSelectType.All); // MID Change j.ellis Performance--cache ancestor and descendant data
							// end MID Track 5338 Color Onhands zero (% Need Limit Not Observed)
//							HierarchyNodeList hnlColor = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
							//HierarchyNodeList hnlColor = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Color); // MID Change j.ellis Performance--cache ancestor and descendant data
							foreach(HierarchyNodeProfile hnpColor in hnlColor)
							{
								if (hnpColor.ColorOrSizeCodeRID == ikt.ColorRID)
								{
									// color match
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
									HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size, eNodeSelectType.All); // MID Change j.ellis Performance--cache ancestor and descendant data
//									HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size); // MID Change j.ellis Performance--cache ancestor and descendant data
//End Track #4037
									//HierarchyNodeList hnlSize = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size); // MID Change j.ellis Performance--cache ancestor and descendant data
									foreach(HierarchyNodeProfile hnpSize in hnlSize)
									{
										if (hnpSize.ColorOrSizeCodeRID == ikt.SizeRID)
										{
											// size match -- add to this bucket, for all store list
											//Begin Track #4637 - JSmith - Split variables by type
//											DataTable dt = dbOnHand.GetOhByTimeByNode
//												(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, onHandProfileList); // MID Track 3312 Total OnHand incorrect
                                            //Begin TT#1684 - JSmith - Showing incorrect on hand data
                                            //DataTable dt = dbOnHand.GetOhByTimeByNode
                                            //    (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, dailyOnHandProfileList, weeklyOnHandProfileList);
                                            DataTable dt = dbOnHand.GetOhByTimeByNode
                                                (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, dailyOnHandProfileList, weeklyOnHandProfileList, _transaction.SAB.ApplicationServerSession.DailyOnhandExists, hnpSize.LevelType);
                                            //End TT#1684
											//End Track #4637
											//	(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, onHandVariable); // MID Change j.ellis "onhand" hard coded for "regular" inventory
											//Begin Track #4637 - JSmith - Split variables by type
											if (dt != null)
											{
											//End Track #4637
												foreach(DataRow dr in dt.Rows)
												{
												// begin MID Track 5953 Null Reference when executing General Method
												//Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
												Index_RID storeIdxRID = _transaction.GetStoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
												// end MID Track 5953 Null Reference when executing General Method
													if (storeIdxRID.RID != Include.UndefinedStoreRID)
													{
														//Begin MID Track 3312 Total OnHand incorrect
														// BEGIN MID Change j.ellis total onhand zero
														foreach (VariableProfile vp in onHandProfileList)
														{
															OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
														}
														//for (int i=0; i<onHandColumn.Length; i++)
														//{
														//	OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[onHandColumn[i]], CultureInfo.CurrentUICulture);
														//}
														// END MID Change j.ellis total onhand zero
														// End MID Track 3312 Total OnHand incorrect
													}
													else
													{
														// store not in list -- must have been added
													}
												}
											//Begin Track #4637 - JSmith - Split variables by type
											}
											//End Track #4637
										}
									}
								}
							}
							break;
						}
						case (eIntransitBy.Size):
						{
//Begin Track #4037 - JSmith - Optionally include dummy color in child list
							// begin MID Track 5338 Color Onhands zero (% Need Limit Not Observed)
							HierarchyNodeList hnlSize;
                            if (hnp.LevelType == eHierarchyLevelType.Size)
							{
								hnlSize = new HierarchyNodeList(eProfileType.HierarchyNode);
								hnlSize.Add(hnp);
							}
							else
							{
								hnlSize = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Size, eNodeSelectType.All);
							}
							//HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Size, eNodeSelectType.All);
							// end MID Track 5338 Color Onhands zero (% Need Limit Not Observed)
//							HierarchyNodeList hnlSize = _transaction.GetDescendantData(hnRID, eHierarchyLevelType.Size);
//End Track #4037
							//HierarchyNodeList hnlSize = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnRID, eHierarchyLevelType.Size); 									HierarchyNodeList hnlSize = _transaction.SAB.HierarchyServerSession.GetDescendantData(hnpColor.Key, eHierarchyLevelType.Size); // MID Change j.ellis Performance--cache ancestor and descendant data
							foreach(HierarchyNodeProfile hnpSize in hnlSize)
							{
								if (hnpSize.ColorOrSizeCodeRID == ikt.SizeRID)
								{
									// size match -- add to this bucket, for all store list
									//Begin Track #4637 - JSmith - Split variables by type
//									DataTable dt = dbOnHand.GetOhByTimeByNode
//										(_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, onHandProfileList); // MID Track 3312 Total OnHand incorrect
                                    //Begin TT#1684 - JSmith - Showing incorrect on hand data
                                    //DataTable dt = dbOnHand.GetOhByTimeByNode
                                    //    (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, dailyOnHandProfileList, weeklyOnHandProfileList);
                                    DataTable dt = dbOnHand.GetOhByTimeByNode
                                        (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, dailyOnHandProfileList, weeklyOnHandProfileList, _transaction.SAB.ApplicationServerSession.DailyOnhandExists, hnpSize.LevelType);
                                    //End TT#1684
									//End Track #4637
									// (_transaction.SAB.ApplicationServerSession.Calendar.CurrentDate.YearDay, _transaction.SAB.ApplicationServerSession.Calendar.CurrentWeek.Key, hnpSize.Key, onHandVariable); // MID Change j.ellis "onhand" hard coded for "regular" inventory

									//Begin Track #4637 - JSmith - Split variables by type
									if (dt != null)
									{
									//End Track #4637
										foreach(DataRow dr in dt.Rows)
										{
										// begin MID Track 5953 Null Reference when executing General Method
										//Index_RID storeIdxRID = _transaction.StoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
										Index_RID storeIdxRID = _transaction.GetStoreIndexRID(Convert.ToInt32(dr["ST_RID"], CultureInfo.CurrentUICulture));
										// end MID Track 5953 Null Reference when executing General Method
											if (storeIdxRID.RID != Include.UndefinedStoreRID)
											{
												// Begin MID Track 3312 Total OnHand incorrect
												// BEGIN MID Change j.ellis Total OnHand zero
												foreach (VariableProfile vp in onHandProfileList)
												{
													OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[vp.DatabaseColumnName], CultureInfo.CurrentUICulture);
												}
												//for (int i=0; i<onHandColumn.Length ; i++)
												//{
												//	OnHandValues[h,k,storeIdxRID.Index] += Convert.ToInt32(dr[onHandColumn[i]], CultureInfo.CurrentUICulture);
												//}
												// END MID Change j.ellis Total OnHand zero
												// End MID Track 3312 Total OnHand incorrect
											}
											else
											{
												// store not in list -- must have been added
											}
										}
									//Begin Track #4637 - JSmith - Split variables by type
									}
									//End Track #4637
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
			return OnHandValues;
		}

		#endregion LoadOnHand

		#region RemoveOnHand
		/// <summary>
		/// Removes OnHand from the hashtables for the specified merchandise hierarchy nodes on the specified day.
		/// </summary>
		/// <param name="aMdseHnRIDs">ICollection of Merchandise Hierarchy Node RIDs to remove</param>
		internal void RemoveDayOnHand (ICollection aMdseHnRIDs)
		{
			foreach(int hn in aMdseHnRIDs)
			{
				RemoveDayOnHand (hn);
			}
		}

		/// <summary>
		/// Removes OnHand from the hashtables for the specified merchandise hierarchy node on the specified day.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise Hierarchy RID whose OnHand is to be removed.</param>
		internal void RemoveDayOnHand (int aMdseHnRID)
		{
			if (_hierarchyNodeHash.Contains(aMdseHnRID))
			{
				_hierarchyNodeHash.Remove(aMdseHnRID);
			}
		}

		/// <summary>
		/// Removes OnHand from hashtable for the specified OnHand key types within the specified merchandise hierarchy nodes on the specified days.
		/// </summary>
		/// <param name="aMdseHn">ICollection of Merchandise hierarchy node RIDs to remove</param>
		/// <param name="aOnHandKeyType">ICollection of IntransitKeyTypes to remove</param>
		internal void RemoveDayOnHand (ICollection aMdseHn, ICollection aOnHandKeyType)
		{
			foreach (int hn in aMdseHn)
			{
				RemoveDayOnHand (hn, aOnHandKeyType);
			}
		}

		/// <summary>
		/// Removes OnHand from hashtable for the specified OnHand key types for the specified merchandise hierarchy nodes on the specified days.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise hierarchy node RID to remove</param>
		/// <param name="aOnHandKeyType">ICollection of IntransitKeyTypes to remove</param>
		internal void RemoveDayOnHand (int aMdseHnRID, ICollection aOnHandKeyType)
		{
			foreach (IntransitKeyType ikt in aOnHandKeyType)
			{
				RemoveDayOnHand (aMdseHnRID, ikt.IntransitTypeKey);
			}
		}
		/// <summary>
		/// Removes OnHand from hashtable for the specified OnHand key types for the specified merchandise hierarchy nodes on the specified days.
		/// </summary>
		/// <param name="aMdseHnRID">Merchandise hierarchy node RID to remove</param>
		/// <param name="aOnHandKeyType">IntransitKeyType to remove</param>
		internal void RemoveDayOnHand (int aMdseHnRID, long aOnHandKeyType)
		{
			if (_hierarchyNodeHash.Contains(aMdseHnRID))
			{
				_hierarchyNodeHash.Remove(aOnHandKeyType);
			}
		}
		#endregion RemoveOnHand
	}
}
