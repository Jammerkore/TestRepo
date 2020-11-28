using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The CubeComponentRelationshipItem class identifies a the relationship between allocation components.
	/// </summary>

	public class CubeComponentRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		private eProfileType[] _totalRelItems;
		private eProfileType[] _detailRelItems;

		//=============
		// CONSTRUCTORS
		//=============

		public CubeComponentRelationshipItem(eProfileType[] aTotalRelItems, eProfileType[] aDetailRelItems)
		{
			try
			{
				_totalRelItems = aTotalRelItems;
				_detailRelItems = aDetailRelItems;
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

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
			//Hashtable profileLists;
			Dictionary<eProfileType, ProfileList> profileLists;
			// END TT#773-MD - Stodd - replace hashtable with dictionary
			ComponentProfileXRef compXRef;
			ArrayList totalList;
			bool validKey;
			ProfileList profileList;
			int profKey;

			try
			{
				aCancel = false;
				// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
				//profileLists = new Hashtable();
				profileLists = new Dictionary<eProfileType, ProfileList>();
				// END TT#773-MD - Stodd - replace hashtable with dictionary

				foreach (eProfileType profType in _totalRelItems)
				{
					profileLists[profType] = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(profType);
				}

				compXRef = (ComponentProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ComponentProfileXRef(aSourceCellRef.Cube.CubeType));

				if (compXRef != null)
				{
					totalList = (ArrayList)compXRef.GetTotalList((AssortmentCellReference)aSourceCellRef);

					if (totalList != null)
					{
						// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
						//foreach (Hashtable profKeyHash in totalList)
						foreach (Dictionary<eProfileType, int> profKeyHash in totalList)
						// END TT#773-MD - Stodd - replace hashtable with dictionary
						{
							validKey = true;

							foreach (eProfileType profType in _totalRelItems)
							{
                                // Begin TT#2006-MD - JSmith - Matrix - Moved Splitter Bar and Locked - Selected File>Save - receive error
                                if (!profKeyHash.ContainsKey(profType)
                                    || !profileLists.ContainsKey(profType))
                                {
                                    validKey = false;
                                    break;
                                }
                                // End TT#2006-MD - JSmith - Matrix - Moved Splitter Bar and Locked - Selected File>Save - receive error
								profKey = (int)profKeyHash[profType];
								profileList = (ProfileList)profileLists[profType];

								if (profKey == int.MaxValue || profileList == null || profileList.Contains(profKey))
								{
									aCellRef[profType] = profKey;
								}
								else
								{
									validKey = false;
									break;
								}
							}

							if (validKey)
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
			//Hashtable profileLists;
			Dictionary<eProfileType, ProfileList> profileLists;
			// END TT#773-MD - Stodd - replace hashtable with dictionary
			ComponentProfileXRef compXRef;
			ArrayList detailList;
			//int[] totalKeys;
			bool validKey;

			ProfileList profileList;
			int profKey;

			try
			{
				aCancel = false;
				// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
				//profileLists = new Hashtable();
				profileLists = new Dictionary<eProfileType, ProfileList>();
				// END TT#773-MD - Stodd - replace hashtable with dictionary

				foreach (eProfileType profType in _detailRelItems)
				{
					profileLists[profType] = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(profType);
				}

				compXRef = (ComponentProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ComponentProfileXRef(aCellRef.Cube.CubeType));

				if (compXRef != null)
				{
					detailList = (ArrayList)compXRef.GetDetailList((AssortmentCellReference)aSourceCellRef);

					if (detailList != null)
					{
						// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
						//foreach (Hashtable profKeyHash in detailList)
						foreach (Dictionary<eProfileType, int> profKeyHash in detailList)
						// END TT#773-MD - Stodd - replace hashtable with dictionary
						{
							validKey = true;

							foreach (eProfileType profType in _detailRelItems)
							{
								profKey = (int)profKeyHash[profType];
								profileList = (ProfileList)profileLists[profType];

								if (profKey == int.MaxValue || profileList == null || profileList.Contains(profKey))
								{
									aCellRef[profType] = profKey;
								}
								else
								{
									validKey = false;
									break;
								}
							}

							if (validKey)
							{
								aCellSelector.CheckItem(aCellRef, out aCancel);
							}

							if (aCancel)
							{
								return;
							}
						}
					}
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
	/// The CubePlaceholderHeaderRelationshipItem class identifies a placeholder to header relationship.
	/// </summary>

	public class CubePlaceholderHeaderRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		private PackColorProfileXRef _packColorXRef;

		//=============
		// CONSTRUCTORS
		//=============

		public CubePlaceholderHeaderRelationshipItem(PackColorProfileXRef aPackColorXRef)
		{
			try
			{
				_packColorXRef = aPackColorXRef;
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

		//========
		// METHODS
		//========

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			Hashtable totalList;
			IDictionaryEnumerator dictEnum;
			PackColorProfileXRefEntry profEntry;

			try
			{
				aCancel = false;

                // Begin TT#2 - RMatelic - Assortment Planning - adjusting header units gets error >>> if HeaderPack or HeaderPackColor dimension is missing, exit 
                if (aSourceCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPack) == -1 ||
                    aSourceCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) == -1 ||	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                    aSourceCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader) == -1)	// TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
                {
                    return;
                }
                // End TT#2

				totalList = _packColorXRef.GetPlaceholderList(aSourceCellRef[eProfileType.AllocationHeader], aSourceCellRef[eProfileType.HeaderPack], aSourceCellRef[eProfileType.HeaderPackColor]);

				if (totalList != null)
				{
					dictEnum = totalList.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						profEntry = (PackColorProfileXRefEntry)dictEnum.Value;
						// BEGIN TT#621-MD - stodd - Changing a matrix value for a post assortment gets a "Dimension not defined on cube" error
						int placeholderIndex = aCellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);
						if (placeholderIndex != -1)
						{
							aCellRef[eProfileType.PlaceholderHeader] = profEntry.PlaceholderHeaderId;
						}
						// END TT#621-MD - stodd - Changing a matrix value for a post assortment gets a "Dimension not defined on cube" error
						aCellRef[eProfileType.HeaderPack] = profEntry.PackId;
						aCellRef[eProfileType.HeaderPackColor] = profEntry.ColorId;

						aCellSelector.CheckItem(aCellRef, out aCancel);

						if (aCancel)
						{
							return;
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Protected routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCurrRelationshipItem">
		/// The index to the CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		override public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			Hashtable detailList;
			IDictionaryEnumerator dictEnum;
			PackColorProfileXRefEntry profEntry;

			try
			{
				aCancel = false;
				detailList = _packColorXRef.GetHeaderList(aSourceCellRef[eProfileType.PlaceholderHeader], aSourceCellRef[eProfileType.HeaderPack], aSourceCellRef[eProfileType.HeaderPackColor]);

				if (detailList != null)
				{
					dictEnum = detailList.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						profEntry = (PackColorProfileXRefEntry)dictEnum.Value;

						aCellRef[eProfileType.AllocationHeader] = profEntry.PlaceholderHeaderId;
						aCellRef[eProfileType.HeaderPack] = profEntry.PackId;
						aCellRef[eProfileType.HeaderPackColor] = profEntry.ColorId;

						aCellSelector.CheckItem(aCellRef, out aCancel);

						if (aCancel)
						{
							return;
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
