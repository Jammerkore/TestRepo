using System;
using System.Collections;
using System.Collections.Generic; // TT#59 Implement Temp Locks
using System.Globalization;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using MIDRetail.Data;
using MIDRetail.Common;

namespace MIDRetail.Business
{
	#region Allocation Wafer Coordinate Classes
	#region AllocationWaferCoordinate
    /// <summary>
	/// The AllocationWaferCoordinate defines a single eWaferCoordinateType and int pair that describe a global, row, or column coordinate.
	/// </summary>
	/// <remarks>
	/// This class is used to define a specific coordinate for a global, row, or column entry.  The collection of all of the Cell's
	/// AllocationWaferCoordinates from the global entry and its specific row and column entries, define the logical coordinates for the Cell.
	/// </remarks>

	[Serializable]
	public class AllocationWaferCoordinate
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private eAllocationCoordinateType _coordinateType;
		private int _coordinateSubType;
		private int _key;
		private string _packName;
		private string _subtotalPackName;
		private string _volumeGrade;
		private string _label;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationWaferCoordinate using the given eWaferCoordinateType and integer.
		/// </summary>
		/// <param name="aCoordinateType">
		/// The eWaferCoordinateType that identifies the type of profile for this coordinate.
		/// </param>
		/// <param name="aKey">
		/// The integer that identifies the logical RID of this coordinate.
		/// </param>

		private AllocationWaferCoordinate(
			eAllocationCoordinateType aCoordinateType,
			int aCoordinateSubType, 
			int aKey, 
			string aPackName,
			string aSubtotalPackName,
			string aVolumeGrade,
			string aLabel
			)
		{
			Initialize(aCoordinateType, aCoordinateSubType, aKey, aPackName, aSubtotalPackName, aVolumeGrade, aLabel);
		}

		public AllocationWaferCoordinate(eAllocationCoordinateType aCoordinateType, int aKey, string aLabel)
		{
			switch (aCoordinateType)
			{
				case eAllocationCoordinateType.None:
				case eAllocationCoordinateType.Header:
				case eAllocationCoordinateType.HeaderTotal:
				case eAllocationCoordinateType.BalanceChainToHeader:
				case eAllocationCoordinateType.Size:
				case eAllocationCoordinateType.SizesTotal:
				case eAllocationCoordinateType.VolumeGrade:
				case eAllocationCoordinateType.PrimarySize:
				case eAllocationCoordinateType.SecondarySize:
				case eAllocationCoordinateType.SecondarySizeTotal:
				{
					Initialize(aCoordinateType, 0, aKey, "", "", "", aLabel);
					break;
				}
					// begin MID track 4326 cannot manually enter size in Size Review
				case (eAllocationCoordinateType.SecondarySizeNone):
				{
					Initialize(eAllocationCoordinateType.SecondarySize, (int)aCoordinateType, aKey,"", "", "", aLabel);
					break;
				}
					// end MID Track 4326 cannot manually enter size in Size Review
				default:
					throw new Exception ("Invalid coordinate type for this method");
			}
		}

		public AllocationWaferCoordinate(eComponentType aComponentType, int aComponentKey, string aLabel)
		{
			switch (aComponentType)
			{
				case eComponentType.SpecificPack:
                    throw new Exception ("Invalid Coordinate Setup Constructor Method for pack");
//					Initialize(eAllocationCoordinateType.PackName, (int)aComponentType, aComponentKey, aLabel,aLabel);
//					break;
				default:
					Initialize(eAllocationCoordinateType.Component, (int)aComponentType, aComponentKey, "", "", "", aLabel);
					break;
			}
		}
		

		public AllocationWaferCoordinate(eAllocationWaferVariable aVariable)
		{
			string variableLabel;
			variableLabel = AllocationWaferVariables.GetVariableProfile(aVariable).DefaultLabel;
			Initialize (eAllocationCoordinateType.Variable, 0, (int)aVariable, "", "", "", variableLabel);
		}

//		public AllocationWaferCoordinate(eAllocationNode aNode, int aNodeKey, string aLabel)
//		{
//			Initialize (eAllocationCoordinateType.AllocatonNode, (int)aNode, aNodeKey, "", aLabel);
//		}

		public AllocationWaferCoordinate(eStoreAllocationNode aNode, int aNodeKey, string aLabel)
		{
			Initialize (eAllocationCoordinateType.StoreAllocationNode, (int)aNode,  aNodeKey, "", "", "", aLabel);
		}

//		public AllocationWaferCoordinate(eHeaderNode aNode, int aNodeKey)
//		{
//			Initialize (eAllocationCoordinateType.HeaderNode, (int)aNode, aNodeKey, "");
//		}

		public AllocationWaferCoordinate(string aPackName, string aSubtotalPackName, string aLabel)
		{
			Initialize (eAllocationCoordinateType.PackName, (int)eComponentType.SpecificPack, 0, aPackName, aSubtotalPackName, "", aLabel);
		}

		public AllocationWaferCoordinate(string aVolumeGrade, string aLabel)
		{
			Initialize (eAllocationCoordinateType.VolumeGrade, 0, 0, "", "", aVolumeGrade, aLabel);
		}

		private void Initialize(
			eAllocationCoordinateType aCoordinateType,
			int aCoordinateSubType, 
			int aKey, 
			string aPackName,
			string aSubtotalName,
			string aVolumeGrade,
			string aLabel
			)
		{
			_coordinateType = aCoordinateType;
			_coordinateSubType = aCoordinateSubType;
			_key = aKey;
			_packName = aPackName;
			_subtotalPackName = aSubtotalName;
			_volumeGrade = aVolumeGrade;
			_label = aLabel;
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eWaferCoordinateType of this coordinate.
		/// </summary>

		public eAllocationCoordinateType CoordinateType
		{
			get	{ return _coordinateType;	}
		}


		public int CoordinateSubType
		{
			get	{ return _coordinateSubType;	}
		}

		/// <summary>
		/// Gets the integer key of this coordinate.
		/// </summary>

		public int Key
		{
			get { return _key; }
		}

		public string PackName
		{
			get { return _packName; }
		}
									
		public string SubtotalPackName
		{
			get { return _subtotalPackName; }
		}

		public string VolumeGrade
		{
			get { return _volumeGrade; }
		}

		public string Label
		{
			get { return _label; }
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region Copy
		/// <summary>
		/// Creates a copy of this AllocationWaferCoordinate.
		/// </summary>
		/// <returns>
		/// A new instance of AllocationWaferCoordinate with a copy of this objects information.
		/// </returns>

		public AllocationWaferCoordinate Clone()
		{
			return new AllocationWaferCoordinate(_coordinateType, _coordinateSubType,_key,_packName, _subtotalPackName, _volumeGrade, _label);
		}
		#endregion Copy
		#endregion Methods
	}
	#endregion AllocationWaferCoordinate

	#region AllocationWaferCoordinateList
	/// <summary>
	/// The AllocationWaferCoordinateList class contains a collection of AllocationWaferCoordinate objects.
	/// </summary>
	/// <remarks>
	/// Each global, row, and column of a grid can identify a AllocationWaferCoordinate or AllocationWaferCoordinates for a Cell.  The collection of these AllocationWaferCoordinates is
	/// defined in this AllocationWaferCoordinateList class.
	/// </remarks>

	[Serializable]
	public class AllocationWaferCoordinateList : System.Collections.ArrayList
	{
		#region Fields
		//=======
		// FIELDS
		//=======

		ApplicationSessionTransaction _trans;
		private int _allocationWaferColumn;
		private bool _buildOverride = false;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationWaferCoordinateList.
		/// </summary>

		public AllocationWaferCoordinateList(ApplicationSessionTransaction aTrans)
			: base()
		{
			_trans = aTrans;
		}

		/// <summary>
		/// Creates a new instance of AllocationWaferCoordinateList.
		/// </summary>

		public AllocationWaferCoordinateList(ApplicationSessionTransaction aTrans, int aAllocationWaferColumn)
			: base()
		{
			_trans = aTrans;
			_allocationWaferColumn = aAllocationWaferColumn;
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the flag identifying if the build override is set.
		/// </summary>

		public bool BuildOverride
		{
			get { return _buildOverride; }
			set { _buildOverride = value; }
		}

		/// <summary>
		/// Gets or sets the column of the wafer.
		/// </summary>

		public int AllocationWaferColumn
		{
			get { return _allocationWaferColumn; }
			set { _allocationWaferColumn = value; }
		}

		/// <summary>
		/// Returns the AllocationWaferCoordinate at the given integer index.
		/// </summary>

		new public AllocationWaferCoordinate this[int aIndex]
		{
			get 
			{ 
				try 
				{
					return (AllocationWaferCoordinate)base[aIndex]; 
				}
				catch (Exception e	)
				{
					string exceptionMessage = e.Message;
					return null;
				}
			}
		}

		#region getCell
		public AllocationWaferCell Cell
		{
			get 
			{
				AllocationWaferCell cell;
				cell = new AllocationWaferCell();
				AllocationWaferInfo allocationWaferInfo =  intInspectWaferCoordinates();

				bool buildColumn = true;
				if (!BuildOverride &&
					_trans.BuildWaferColumns.Count > 0)
				{
					buildColumn = false;
					foreach(BuildWaferColumn bwc in _trans.BuildWaferColumns)
					{
						if (AllocationWaferColumn == bwc.waferColumn &&
							allocationWaferInfo.VariableKey == bwc.AllocationWaferVariable)
						{
							buildColumn = true;
							break;
						}
					}
				}
                // BEGIN TT#1124 - AGallagher - Average by Header or Component is zero when Style Review is opened before Summary Review
                if (_trans.AllocationViewType != eAllocationSelectionViewType.Summary)
                {
                // END TT#1124 - AGallagher - Average by Header or Component is zero when Style Review is opened before Summary Review
                    if (!buildColumn)
                    {
                        return cell;
                    }
                } // TT#1124 - AGallagher - Average by Header or Component is zero when Style Review is opened before Summary Review
				
				//====== put in transaction; return cell =====/
				AllocationProfileList headerList = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.Allocation);
				AllocationProfile header = null;
				AllocationSubtotalProfile grandTotal = this._trans.GetAllocationGrandTotalProfile();

				if (allocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
				{
					header = (AllocationProfile)headerList.FindKey(allocationWaferInfo.HeaderKey);
				}
				bool cellCanChange = true;
				if (this._trans.DataState == eDataState.ReadOnly)
				{
					cellCanChange = false;
				}
				else
				{
					if (allocationWaferInfo.HeaderType == eAllocationCoordinateType.Header
						|| allocationWaferInfo.HeaderType == eAllocationCoordinateType.HeaderTotal)
					{
						// BEGIN MID Change j.ellis Size Total Allocated Balance wrong
						if (this._trans.AllocationViewType == eAllocationSelectionViewType.Size
							&& 
							(allocationWaferInfo.ComponentType == eComponentType.ColorAndSize
							|| allocationWaferInfo.VariableKey == eAllocationWaferVariable.SizeTotalAllocated))
						{
							cellCanChange = !grandTotal.GetUpdatesSizeIntransitForAll(allocationWaferInfo.Component);
						}
						else
						{																		
							cellCanChange = !grandTotal.GetUpdatesStyleIntransitForAll(header, allocationWaferInfo.Component);
						}
						// END MID Change j.ellis
						if (allocationWaferInfo.ComponentType == eComponentType.AllPacks
							// || allocationWaferInfo.ComponentType == eComponentType.AllColors  //MID Track 3326 Cannot modify "all dim"
							|| allocationWaferInfo.ComponentType == eComponentType.AllGenericPacks
							|| allocationWaferInfo.ComponentType == eComponentType.AllNonGenericPacks
							|| allocationWaferInfo.ComponentType == eComponentType.AllSizes)
						{
							cellCanChange = false;
						}
						else if (allocationWaferInfo.ComponentType == eComponentType.ColorAndSize)
						{
							// BEGIN MID Track #2468 Cannot Change Size Total
//							if (allocationWaferInfo.SizeKey == Include.NoRID)
//							{
//								cellCanChange = false;
//							}
//							else if (!grandTotal.WorkUpBulkSizeBuy(header)
							if (!grandTotal.WorkUpBulkSizeBuy(header)
                            // END MID Track #2468
								&& grandTotal.GetQtyToAllocate(header, allocationWaferInfo.Component) < 1)
							{
								cellCanChange = false;
							}
						}
						// BEGIN MID Track # 2468 Cannot Change Size Total
//						else if (allocationWaferInfo.ComponentType == eComponentType.SpecificColor)
//						{
//							if (this._trans.AllocationViewType == eAllocationSelectionViewType.Size)
//							{
//								if (allocationWaferInfo.PrimarySizeKey != Include.NoRID
//									&& allocationWaferInfo.SecondarySizeKey != Include.NoRID)
//								{
//									cellCanChange = false;
//								}
//							}
//						}
						// END MID Track # 2468

					}
					else
					{
						cellCanChange = false;
					}
				}
				cell.StoreExceedsCapacity = false;
				switch (allocationWaferInfo.StoreNodeType)
				{
					case eStoreAllocationNode.Store:
					{
						ProcessGetStore
							(
							grandTotal,
							header,
							allocationWaferInfo,
							cell, 
							cellCanChange
							);
						break;
					}
					case eStoreAllocationNode.All:
					{
						ProcessGetStoreList
							(
							grandTotal,
							header,
							allocationWaferInfo,
							cell, 
							_trans.AllocationStoreAttributeID,
							Include.AllStoreTotal,
							cellCanChange
							);
						break;
					}

					case eStoreAllocationNode.Set:
					{
						ProcessGetStoreList
							(
							grandTotal,
							header,
							allocationWaferInfo,
							cell, 
							_trans.AllocationStoreAttributeID,
							allocationWaferInfo.SetKey,
							cellCanChange
							);
						break;
					}
					default:
					{
						// Must be balance line.
						if (allocationWaferInfo.HeaderType == eAllocationCoordinateType.None ||
							!Enum.IsDefined(typeof(eComponentType), (int)allocationWaferInfo.ComponentType))
						{
							cell.CellIsValid = false;
							cell.CellCanBeChanged = false;
						}
						else
						{
							#region Allocation Balance
							switch (allocationWaferInfo.VariableKey)
							{
								case (eAllocationWaferVariable.OriginalQuantityAllocated):
								{
									switch (allocationWaferInfo.SecondaryVariableKey)
									{
										case (eAllocationWaferVariable.None):
										case (eAllocationWaferVariable.Total):
										{
											cell.Value = grandTotal.GetOrigAllocatedBalance(header, allocationWaferInfo.Component);
											cell.CellCanBeChanged = false;
											break;
										}
										default:
										{
											cell.CellIsValid = false;
											cell.CellCanBeChanged = false;
											break;
										}
									}
									break;
								}
								case (eAllocationWaferVariable.QuantityAllocated):
								{
									switch (allocationWaferInfo.SecondaryVariableKey)
									{
										case (eAllocationWaferVariable.None):
										case (eAllocationWaferVariable.Total):
										{
											//											BEGIN MID Track #2468 Cannot Change Size Total
											//											cell.Value = grandTotal.GetAllocatedBalance(header, allocationWaferInfo.Component);
											//											cell.CellCanBeChanged = cellCanChange;
											GeneralComponent component;
											if (allocationWaferInfo.ComponentType == eComponentType.ColorAndSize
												&& this._trans.AllocationViewType == eAllocationSelectionViewType.Size
												&& allocationWaferInfo.SecondarySizeKey != Include.NoRID
												&& allocationWaferInfo.PrimarySizeKey == Include.NoRID)
											{
												if (this._trans.AllocationCriteriaSecondarySizeCount > 1)
												{
													cell.CellIsValid = false;
													cell.CellCanBeChanged = false;
												}
												component = ((AllocationColorSizeComponent)allocationWaferInfo.Component).ColorComponent;
											}
											else
											{
												component = allocationWaferInfo.Component;
											}
											if (cell.CellIsValid)
											{
												cell.Value = grandTotal.GetAllocatedBalance(header, component);
												cell.CellCanBeChanged = cellCanChange;
											}
											//											END MID Track #2468
											break;
										}
										default:
										{
											cell.CellIsValid = false;
											cell.CellCanBeChanged = false;
											break;
										}
									}
									break;
								}
                                // BEGIN TT#1401 - AGallagher - Reservation Stores
                                case (eAllocationWaferVariable.StoreItemQuantityAllocated):
                                {
                                    cell.CellIsValid = false;
                                    cell.CellCanBeChanged = false;
                                    break;
                                }
                                case (eAllocationWaferVariable.StoreIMOQuantityAllocated):
                                {
                                    cell.CellIsValid = false;
                                    cell.CellCanBeChanged = false;
                                    break;
                                }
                                case (eAllocationWaferVariable.StoreIMOMaxQuantityAllocated):
                                {
                                    cell.CellIsValid = false;
                                    cell.CellCanBeChanged = false;
                                    break;
                                }
                                case (eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated):
                                {
                                    cell.CellIsValid = false;
                                    cell.CellCanBeChanged = false;
                                    break;
                                }
                                // END TT#1401 - AGallagher - Reservation Stores
								case (eAllocationWaferVariable.VelocityRuleResult):
								{
									switch (allocationWaferInfo.SecondaryVariableKey)
									{
										case (eAllocationWaferVariable.None):
										case (eAllocationWaferVariable.Total):
										{
											cell.Value = grandTotal.GetVelocityRuleBalance(header);
											cell.CellCanBeChanged = false;
											break;
										}
										default:
										{
											cell.CellIsValid = false;
											cell.CellCanBeChanged = false;
											break;
										}
									}
									break;
								}
								case (eAllocationWaferVariable.Transfer):
								{
									switch (allocationWaferInfo.SecondaryVariableKey)
									{
										case (eAllocationWaferVariable.None):
										case (eAllocationWaferVariable.Total):
										{
											cell.CellIsValid = false;
											cell.CellCanBeChanged = false;
											break;
										}
										default:
										{
											cell.CellIsValid = false;
											cell.CellCanBeChanged = false;
											break;
										}
									}
									break;
								}
//								BEGIN MID Track #2468 Cannot Change Size Total
								case (eAllocationWaferVariable.SizeTotalAllocated):
								{
									switch (allocationWaferInfo.SecondaryVariableKey)
									{
										case (eAllocationWaferVariable.None):
										case (eAllocationWaferVariable.Total):
										{
											// BEGIN MID Change j.ellis Balance for Size Total Allocated not correct
											AllocationColorSizeComponent colorSizeComponent;
											switch (allocationWaferInfo.ComponentType)
											{
												case (eComponentType.SpecificColor):
												{
													colorSizeComponent = new AllocationColorSizeComponent
														(allocationWaferInfo.Component, 
														new GeneralComponent(eGeneralComponentType.AllSizes));
													break;
												}
												case (eComponentType.ColorAndSize):
												{
													colorSizeComponent = (AllocationColorSizeComponent)allocationWaferInfo.Component;
													break;
												}
												case (eComponentType.AllColors):
												{
													colorSizeComponent = new AllocationColorSizeComponent
														(allocationWaferInfo.Component,
														new GeneralComponent(eGeneralComponentType.AllSizes));
													break;
												}
												default:
												{
													throw new MIDException(eErrorLevel.fatal,
														(int)eMIDTextCode.msg_al_UnknownComponentType,
														MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
												}
											}
											cell.Value = grandTotal.GetAllocatedBalance(header, colorSizeComponent);
//											cell.Value = grandTotal.GetAllocatedBalance(header, allocationWaferInfo.Component);
											// END MID Change j.ellis
											cell.CellCanBeChanged = cellCanChange;
											break;
										}
										default:
										{
											cell.CellIsValid = false;
											cell.CellCanBeChanged = false;
											break;
										}
									}
									break;
								}
//								END MID Track #2468
								default:
								{
									cell.CellIsValid = false;
									cell.CellCanBeChanged = false;
									break;
								}
							}
						}
						break;
					}
						#endregion Allocation Balance
				}
				return cell; 
			}
		}
        
		#region specific Store
		/// <summary>
		/// Gets the selected cell's value for the specified Store
		/// </summary>
		/// <param name="aTotalProfile">Allocation Subtotal Profile</param>
		/// <param name="aHeaderProfile">Allocation Profile for selected Header.</param>
		/// <param name="aAllocationWaferInfo">Allocation Wafer Info</param>
		/// <param name="aCell">Cell</param>
		/// <param name="aCellCanChange">True: cell may be changed; False: cell may not change</param>
		private void ProcessGetStore
			(
			AllocationSubtotalProfile aTotalProfile,
			AllocationProfile aHeaderProfile,
			AllocationWaferInfo aAllocationWaferInfo,
			AllocationWaferCell aCell, 
			bool aCellCanChange
			)
		{
			IntransitKeyType ikt;
			// begin MID Track 3466 Store at capacity did not highlight
			aCell.StoreExceedsCapacity = aTotalProfile.GetStoreExceedsCapacity(aAllocationWaferInfo.StoreKey);
            aCell.StoreAllocationOutOfBalance = false;
			// end MID Track 3466 Store at capacity did not highlight
			switch (aAllocationWaferInfo.VariableKey)
			{
				case eAllocationWaferVariable.OriginalQuantityAllocated:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aTotalProfile.GetStoreOrigQtyAllocated
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aAllocationWaferInfo.StoreKey
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.QuantityAllocated:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
                            GeneralComponent component;
                            if (aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize 
                                && this._trans.AllocationViewType == eAllocationSelectionViewType.Size
                                && aAllocationWaferInfo.SecondarySizeKey != Include.NoRID
                                && aAllocationWaferInfo.PrimarySizeKey == Include.NoRID)
                            {
                                if (this._trans.AllocationCriteriaSecondarySizeCount > 1)
                                {
                                    aCell.CellIsValid = false;
                                    aCell.CellCanBeChanged = false;
                                }
                                component = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
                            }
                            else
                            {
                                component = aAllocationWaferInfo.Component;
                            }
							if (aCell.CellIsValid)
							{
								aCell.Value = 
									aTotalProfile.GetStoreQtyAllocated
									(
									aHeaderProfile,
									component,
									aAllocationWaferInfo.StoreKey
									);
                                // begin TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28A
                                //if (!aTotalProfile.GetIncludeStoreInAllocation(aHeaderProfile, aAllocationWaferInfo.StoreKey))
                                //{
                                //    aCell.CellCanBeChanged = false;
                                //}
                                //else
                                //{
                                    // end TT#1401 - Jellis - Urban Virtual Store Warehouse pt 28A
                                    aCell.CellCanBeChanged = aCellCanChange;
                                //}   // TT#1401 - Jellis - Urban Virtual Store warehouse pt 28A
                                aCell.MayExceedCapacityMaximum = 
									aTotalProfile.GetStoreMayExceedCapacity
									(
									aHeaderProfile,
									aAllocationWaferInfo.StoreKey
									);
								aCell.MayExceedGradeMaximum = 
									aTotalProfile.GetStoreMayExceedMax
									(
									aHeaderProfile,
									aAllocationWaferInfo.StoreKey
									);
								aCell.MayExceedPrimaryMaximum =
									aTotalProfile.GetStoreMayExceedPrimaryMaximum
									(
									aHeaderProfile,
									aAllocationWaferInfo.StoreKey
									);
								aCell.GradeMaximumValue = double.MaxValue;
								aCell.PrimaryMaximumValue =
									aTotalProfile.GetStorePrimaryMaximum
									(
									aHeaderProfile,
									aAllocationWaferInfo.Component,
									aAllocationWaferInfo.StoreKey
									);
								aCell.MinimumValue = 0;
								// begin MID Track 3466 Store at capacity did not highlight
								//aCell.StoreExceedsCapacity = aTotalProfile.GetStoreExceedsCapacity(aAllocationWaferInfo.StoreKey);
								// end MID Track 3466 Store at capacity did not highlight
						 	    // BEGIN MID Track # 1511 Highlight Stores that are Out Of Balance
								if (component.ComponentType != eComponentType.SpecificColor                 // MID Track 3966 store at capacity not highlighted
									|| this._trans.AllocationViewType == eAllocationSelectionViewType.Size) // MID Track 3966 store at capacity not highlighted
								{
									aCell.StoreAllocationOutOfBalance = aTotalProfile.IsStoreAllocationOutOfBalance
										(
										aHeaderProfile,
										component,
										aAllocationWaferInfo.StoreKey
										);
								}
								 // END MID Track # 1511
							}
							break;
						}
                        case (eAllocationWaferVariable.PctToTotal):
						{
							GeneralComponent colorComponent;
							GeneralComponent sizeComponent;
							switch (aAllocationWaferInfo.Component.ComponentType)
							{
								case (eComponentType.SpecificColor):
								{
									colorComponent = aAllocationWaferInfo.Component;
									sizeComponent = new GeneralComponent(eComponentType.AllSizes);
									aCell.CellIsValid = false;
									break;
								}
								case (eComponentType.ColorAndSize):
								{
									colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
									sizeComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
                                    // begin TT#1521 - JEllis Size Review Response Time
                                    if (this._trans.AllocationViewType == eAllocationSelectionViewType.Size)
                                    {
                                        aCell.CellIsValid = false;
                                    }
                                    //if (this._trans.AllocationViewType == eAllocationSelectionViewType.Size
                                    //    && aAllocationWaferInfo.SecondarySizeKey != Include.NoRID
                                    //    && aAllocationWaferInfo.PrimarySizeKey == Include.NoRID
                                    //    && this._trans.AllocationCriteriaSecondarySizeCount > 1)
                                    //{
                                    //    aCell.CellIsValid = false;
                                    //}
                                    // end TT#1521 - JEllis Size Review Response Time
									break;
								}
								case (eComponentType.AllColors):
								{
									colorComponent = aAllocationWaferInfo.Component;
									sizeComponent = new GeneralComponent(eComponentType.AllSizes);
									break;
								}
								default:
								{
									throw new MIDException(eErrorLevel.fatal,
										(int)eMIDTextCode.msg_al_UnknownComponentType,
										MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
								}
							}

							if (aCell.CellIsValid)
							{
								aCell.Value = aTotalProfile.GetStoreSizeAllocatedPctToColorAllocated
									(aHeaderProfile,
									new AllocationColorSizeComponent(colorComponent, sizeComponent),
									this._trans.StoreIndexRID(aAllocationWaferInfo.StoreKey));
							}
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // BEGIN TT#1401 - AGallagher - Reservation Stores
                case eAllocationWaferVariable.StoreItemQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                aCell.Value =
                                    aTotalProfile.GetStoreItemQtyAllocated
                                    (
                                    aHeaderProfile,
                                    aAllocationWaferInfo.Component,
                                    aAllocationWaferInfo.StoreKey
                                    );
								// begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F -- allow manual change	
                                //// BEGIN TT#1401 - AGallagher - VSW - temp code to stop entering value in Store Item Qty
                                //// aCell.CellCanBeChanged = aCellCanChange;   
                                //aCell.CellCanBeChanged = false;
                                //// END TT#1401 - AGallagher - VSW - temp code to stop entering value in Store Item Qty
                                aCell.CellCanBeChanged = aCellCanChange;
								// end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28F - allow manual change
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case eAllocationWaferVariable.StoreIMOQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                aCell.Value =
                                    aTotalProfile.GetStoreImoQtyAllocated
                                    (
                                    aHeaderProfile,
                                    aAllocationWaferInfo.Component,
                                    aAllocationWaferInfo.StoreKey
                                    );
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case eAllocationWaferVariable.StoreIMOMaxQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
								int iMax = aTotalProfile.GetStoreImoMaxValue
                                    (
                                    aHeaderProfile,
                                    aAllocationWaferInfo.Component,
                                    aAllocationWaferInfo.StoreKey
                                    );
                                // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
                                //if (iMax == int.MaxValue)
                                //{
                                //    aCell.Value = 0;
                                //    aCell.ValueAsString = " ";
                                //}
                                //else
                                //{
                                //    string iMaxs = Convert.ToString(iMax);
                                //    aCell.ValueAsString = iMaxs;
                                //    aCell.Value =
                                //        aTotalProfile.GetStoreImoMaxValue
                                //        (
                                //        aHeaderProfile,
                                //        aAllocationWaferInfo.Component,
                                //        aAllocationWaferInfo.StoreKey
                                //        );
                                //}
                                aCell.Value = iMax;
                                // End TT#2225
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#1401 - AGallagher - Reservation Stores
				case eAllocationWaferVariable.AppliedRule:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							if (aHeaderProfile == null)
							{
								aCell.CellIsValid = false;
							}
							else
							{
								//	aCell.Value = (double)aHeaderProfile.GetStoreChosenRuleType(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
								aCell.ValueAsString = MIDText.GetTextOnly((int)aHeaderProfile.GetStoreChosenRuleType(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey));
							}
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.RuleResults:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							aCell.Value = 
								aTotalProfile.GetStoreQtyAllocatedByRule
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component, 
								aAllocationWaferInfo.StoreKey
								);
							aCell.CellCanBeChanged = false; 
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

                // BEGIN TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working
                case (eAllocationWaferVariable.AssortmentGrade):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                // need new code here
                                aCell.ValueAsString = aTotalProfile.GetStoreAssortmentGrade(aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#2066-MD - AGallagher - Ship to Date validation.  Is this how it should be working

				case eAllocationWaferVariable.StoreGrade:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.ValueAsString = aTotalProfile.GetStoreGrade(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
// (CSMITH) - BEG MID Track #3125: Select grade and nothing displays by store
						case (eAllocationWaferVariable.Total):
						{
							GeneralComponent colorComponent = null;

							if (aAllocationWaferInfo.ComponentType != eComponentType.AllColors)
							{
								if (aAllocationWaferInfo.Component.ComponentType == eComponentType.SpecificColor)
								{
									colorComponent = aAllocationWaferInfo.Component;
								}
								else if (aAllocationWaferInfo.Component.ComponentType == eComponentType.ColorAndSize)
								{
									colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
								}

								if (colorComponent != null)
								{								
									if (aTotalProfile.BulkColors.Count > 1)
									{
										if (aTotalProfile.SubtotalMembers.Count > 1)
										{
											if (aHeaderProfile != null)
											{
												aCell.CellIsValid = false;
												aCell.CellCanBeChanged = false;
											}
										}

										if (_trans.AllocationCriteriaSecondarySizeCount == 1)
										{
											if (colorComponent.ComponentType == eComponentType.SpecificColor)
											{
												aCell.CellIsValid = false;
												aCell.CellCanBeChanged = false;
											}
										}
										else if (_trans.AllocationCriteriaSecondarySizeCount > 1)
										{
											if (aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize ||
												aAllocationWaferInfo.ComponentType == eComponentType.SpecificColor)
											{
												aCell.CellIsValid = false;
												aCell.CellCanBeChanged = false;
											}
										}
									}
									else
									{
										if (aTotalProfile.SubtotalMembers.Count > 1)
										{
											if (aHeaderProfile != null)
											{
												aCell.CellIsValid = false;
												aCell.CellCanBeChanged = false;
											}
										}

										if (_trans.AllocationCriteriaSecondarySizeCount > 1 &&
											aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize)
										{
											aCell.CellIsValid = false;
											aCell.CellCanBeChanged = false;
										}
									}
								}
							}
							else
							{
								if (aTotalProfile.SubtotalMembers.Count > 1)
								{
									if (aHeaderProfile != null)
									{
										aCell.CellIsValid = false;
										aCell.CellCanBeChanged = false;
									}
								}

								if (_trans.AllocationCriteriaSecondarySizeCount > 1 &&
									aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize)
								{
									aCell.CellIsValid = false;
									aCell.CellCanBeChanged = false;
								}
							}

							if (aCell.CellIsValid)
							{
								aCell.ValueAsString = aTotalProfile.GetStoreGrade(aAllocationWaferInfo.StoreKey);
								aCell.CellCanBeChanged = false;
							}

							break;
						}
// (CSMITH) - END MID Track #3125
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                case (eAllocationWaferVariable.BasisGrade):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                if (this._trans.BasisCriteriaExists)
                                {
                                    aCell.ValueAsString = aTotalProfile.GetStoreBasisGrade(aAllocationWaferInfo.StoreKey);
                                }
                                // Begin TT#672 - RMatelic - Style View - Object Referrence error when selecting Basis columns when no basis exists
                                else
                                {
                                    aCell.ValueAsString = " ";
                                }
                                // End TT#672 
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // End TT#638
				case eAllocationWaferVariable.OnHand:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{		
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreOnHand(aHeaderProfile, ikt, aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // BEGIN TT#1401 - AGallagher - Reservation Stores
                case eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
                                //BEGIN TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                                if (aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
                                {
                                    aCell.Value = aTotalProfile.GetStoreImoHistory(aHeaderProfile, ikt, aAllocationWaferInfo.StoreKey, aTotalProfile.VelocityStyleHnRID);
                                }
                                else
                                {
                                    aCell.Value = aTotalProfile.GetStoreImoHistory(aHeaderProfile, ikt, aAllocationWaferInfo.StoreKey);
                                }
                                //END TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#1401 - AGallagher - Reservation Stores
				case eAllocationWaferVariable.InTransit:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreInTransit(ikt, aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.Sales:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreSalesPlan(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.Stock:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreStockPlan(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.Need:
				case eAllocationWaferVariable.OpenToShip:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							// same as OpenToShip (ie. need after allocation)
							aCell.Value = aTotalProfile.GetStoreUnitNeed(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.PercentNeed:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStorePercentNeed(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.OTSVariance:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							// Need before allocation
							aCell.Value = 
								aTotalProfile.GetStoreUnitNeed(aAllocationWaferInfo.StoreKey)
								- aTotalProfile.GetStoreQtyAllocated(new GeneralComponent(eComponentType.Total), aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.PctSellThru):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = this._trans.GetStorePctSellThru(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.PctSellThruIdx):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = this._trans.GetStorePctSellThruIdx(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// begin MID Track 3880 Add Ship To Day as variable for Style and Size Review
				case (eAllocationWaferVariable.ShipToDay):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): // MID Track 3880 Add Ship Day Variable to Style and Size Need
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										aCell.ValueAsString = (aTotalProfile.GetStoreShipDay(aHeaderProfile, aAllocationWaferInfo.StoreKey)).ToShortDateString();
										aCell.CellIsValid = true;
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												AllocationColorSizeComponent colorSize = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												colorComponent = colorSize.ColorComponent;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorComponent = aAllocationWaferInfo.Component;
												break;
											}
											default:
											{
												colorComponent = null;
												break;
											}
										}
										if (aHeaderProfile.BulkColors.Count == 1)
										{
											if (colorComponent != null 
												&& colorComponent.ComponentType == eComponentType.SpecificColor)
											{
												aCell.ValueAsString = (aTotalProfile.GetStoreShipDay(aHeaderProfile, aAllocationWaferInfo.StoreKey)).ToShortDateString();
												aCell.CellIsValid = true;
											}
										}
										else
										{
											if (colorComponent != null 
												&& colorComponent.ComponentType == eComponentType.AllColors)
											{
												aCell.ValueAsString = (aTotalProfile.GetStoreShipDay(aHeaderProfile, aAllocationWaferInfo.StoreKey)).ToShortDateString();
												aCell.CellIsValid = true;
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
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 3880 Add Ship To Day as variable for Style and Size Review
				// begin MID Track 4291 Add Fill Variables to Size Review
				case (eAllocationWaferVariable.NeedDay):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										aCell.ValueAsString = (aTotalProfile.GetStoreNeedDay(aHeaderProfile, aAllocationWaferInfo.StoreKey)).ToShortDateString();
										aCell.CellIsValid = true;
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												AllocationColorSizeComponent colorSize = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												colorComponent = colorSize.ColorComponent;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorComponent = aAllocationWaferInfo.Component;
												break;
											}
											default:
											{
												colorComponent = null;
												break;
											}
										}
										if (aHeaderProfile.BulkColors.Count == 1)
										{
											if (colorComponent != null 
												&& colorComponent.ComponentType == eComponentType.SpecificColor)
											{
												aCell.ValueAsString = (aTotalProfile.GetStoreNeedDay(aHeaderProfile, aAllocationWaferInfo.StoreKey)).ToShortDateString();
												aCell.CellIsValid = true;
											}
										}
										else
										{
											if (colorComponent != null 
												&& colorComponent.ComponentType == eComponentType.AllColors)
											{
												aCell.ValueAsString = (aTotalProfile.GetStoreNeedDay(aHeaderProfile, aAllocationWaferInfo.StoreKey)).ToShortDateString();
												aCell.CellIsValid = true;
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
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeOwnPlan):   //  MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreSizePlan(
												aHeaderProfile, 
												aAllocationWaferInfo.StoreKey, 
												colorSizeComponent, 
												true, 
												false);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeOwnNeed):  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                // begin MID Track 4921 Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreSizeNeed(
												aHeaderProfile, 
												aAllocationWaferInfo.StoreKey, 
												colorSizeComponent, 
												true, 
												false);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // end MID Track 4921 Fill to Size Plan Enhancement
				case (eAllocationWaferVariable.FillSizeOwnPctNeed):   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
					// begin MID Track 4921 Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreSizePctNeed(
												aHeaderProfile, 
												aAllocationWaferInfo.StoreKey, 
												colorSizeComponent, 
												true, 
												false);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4291 Add Fill Variables to Size Review
					// begin MID Track 4921 Anf#666 Fill to Size Plan Enhancement
				case (eAllocationWaferVariable.FillSizeFwdForecastPlan):  
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreSizePlan(
												aHeaderProfile, aAllocationWaferInfo.StoreKey,
												colorSizeComponent, 
												true, 
												true);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeFwdForecastNeed): 
                // begin MID Track 4921 AnF Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreSizeNeed(
												aHeaderProfile, aAllocationWaferInfo.StoreKey,
												colorSizeComponent, 
												true, 
												true);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4921 AnF Fill to Size Plan Enhancement
				case (eAllocationWaferVariable.FillSizeFwdForecastPctNeed): 
					// begin MID Track 4921 AnF Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreSizePctNeed(
												aHeaderProfile, aAllocationWaferInfo.StoreKey,
												colorSizeComponent, 
												true, 
												true);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4921 AnF Fill to Size Plan Enhancement
				case (eAllocationWaferVariable.FillSizeFwdForecastSales): 
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorComponent = aAllocationWaferInfo.Component;
												break;
											}
											default:
											{
												colorComponent = null;
												break;
											}
										}
										if (colorComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreFillSizeBasisPlan(
												aHeaderProfile,
												colorComponent,
												aAllocationWaferInfo.StoreKey,
												true);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeFwdForecastStock): 
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorComponent = aAllocationWaferInfo.Component;
												break;
											}
											default:
											{
												colorComponent = null;
												break;
											}
										}
										if (colorComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreFillSizeBasisPlan(
												aHeaderProfile,
												colorComponent,
												aAllocationWaferInfo.StoreKey,
												false);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				case (eAllocationWaferVariable.VelocityGrade):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.ValueAsString = aTotalProfile.GetStoreVelocityGrade(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                
				case (eAllocationWaferVariable.VelocityRuleResult):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							//										aCell.Value = aTotalProfile.GetStoreVelocityRuleResult(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//							aCell.Value = (int)this._trans.Velocity.GetStoreVelocityRuleResult(aAllocationWaferInfo.StoreKey);
							aCell.Value = (int)this._trans.Velocity.GetStoreVelocityRuleResult(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
// (CSMITH) - END MID Track #2410
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.VelocityRuleQty):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							//	aCell.Value = aTotalProfile.GetStoreVelocityRuleQty(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//							aCell.Value = this._trans.Velocity.GetStoreVelocityRuleQty(aAllocationWaferInfo.StoreKey);
							aCell.Value = this._trans.Velocity.GetStoreVelocityRuleQty(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
// (CSMITH) - END MID Track #2410
                            // aCell.CellCanBeChanged = true;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                            aCell.CellCanBeChanged = false;  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

                // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                case (eAllocationWaferVariable.VelocityRuleTypeQty):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = this._trans.Velocity.GetStoreVelocityRuleTypeQty(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = true; 
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)

                case (eAllocationWaferVariable.VelocityInitialRuleQty): //tt #152 - Velocity balance - apicchetti
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = this._trans.Velocity.GetStoreVelocityInitialRuleQty(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }

                case (eAllocationWaferVariable.VelocityInitialWillShip): //tt #152 - Velocity balance - apicchetti
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = this._trans.Velocity.GetStoreVelocityInitialWillShip(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }

				case (eAllocationWaferVariable.VelocityRuleType):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							//	aCell.ValueAsString = MIDText.GetTextOnly((int)aTotalProfile.GetStoreVelocityRuleType(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey));
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//							aCell.Value = (double)this._trans.Velocity.GetStoreVelocityRuleType(aAllocationWaferInfo.StoreKey);
							aCell.Value = (double)this._trans.Velocity.GetStoreVelocityRuleType(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
// (CSMITH) - END MID Track #2410
							aCell.CellCanBeChanged = true;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

                case (eAllocationWaferVariable.VelocityInitialRuleType): //tt #152 - Velocity balance - apicchetti
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = (double)this._trans.Velocity.GetStoreVelocityInitialRuleType(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = true;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                case (eAllocationWaferVariable.VelocityInitialRuleTypeQty): //tt #152 - Velocity balance - apicchetti
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = (double)this._trans.Velocity.GetStoreVelocityInitialRuleTypeQty(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = false;  
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
				case (eAllocationWaferVariable.Transfer):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//							aCell.Value = this._trans.Velocity.GetStoreVelocityTransferQty(aAllocationWaferInfo.StoreKey);
							aCell.Value = this._trans.Velocity.GetStoreVelocityTransferQty(aHeaderProfile.HeaderRID, aAllocationWaferInfo.StoreKey);
// (CSMITH) - END MID Track #2410: Allow interactive processing with multiple headers
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreBasisInTransit(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreBasisOnHand(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
                        {
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                //BEGIN TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                case (eAllocationWaferVariable.BasisVSWOnHand):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = aTotalProfile.GetStoreBasisImoHistory(aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                //END TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                case (eAllocationWaferVariable.StyleOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreStyleOnHand(ikt, aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.StyleInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreStyleInTransit(ikt, aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.AvgWeeklySales):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreAvgWeeklySales(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

				case (eAllocationWaferVariable.AvgWeeklyStock):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreAvgWeeklyStock(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

                // BEGIN TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
                case (eAllocationWaferVariable.AvgWeeksOfSupply):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = this._trans.Velocity.GetStoreAvgWeeklySupply(aAllocationWaferInfo.StoreKey);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

				case (eAllocationWaferVariable.BasisSales):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreBasisSales(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisStock):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreBasisStock(aAllocationWaferInfo.StoreKey);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeCurvePct):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreSizeCurvePct(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey,
								this.GetSizeComponent(aAllocationWaferInfo));
								// aAllocationWaferInfo.Component);
							// END MID Track #2937 Size Onhand Incorrect

							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // BEGIN TT#1401 - AGallagher - VSW
                case (eAllocationWaferVariable.SizeVSWOnHand):
				{
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
                            //aCell.Value = aTotalProfile.GetStoreListTotalImoHistory(aHeaderProfile, aAllocationWaferInfo.StoreKey, this.GetSizeComponent(aAllocationWaferInfo), false); 
                            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 22
                            ArrayList iktArray = GetSizeIntransitKeyTypes(aTotalProfile, aAllocationWaferInfo);
                            // Begin TT#5026 - JSmith - Question about Size Alternates
                            bool includeSizeAlternates = false;
                            if (aAllocationWaferInfo.Component is AllocationColorSizeComponent)
                            {
                                AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
                                if (acsc.SizeComponent.ComponentType == eComponentType.SpecificSize)
                                {
                                    includeSizeAlternates = true;
                                }
                            }
                            else if (aAllocationWaferInfo.Component is AllocationColorOrSizeComponent)
                            {
                                AllocationColorOrSizeComponent acsc = (AllocationColorOrSizeComponent)aAllocationWaferInfo.Component;
                                if (acsc.ComponentType == eComponentType.SpecificSize)
                                {
                                    includeSizeAlternates = true;
                                }
                            }
                            // End TT#5026 - JSmith - Question about Size Alternates
                            int cellValue = 0;
                            foreach (IntransitKeyType iKT in iktArray)
                            {
                                // Begin TT#5026 - JSmith - Question about Size Alternates
                                //cellValue += aTotalProfile.GetStoreImoHistory(aHeaderProfile, iKT, aAllocationWaferInfo.StoreKey);
                                cellValue += aTotalProfile.GetStoreImoHistory(aHeaderProfile, iKT, aAllocationWaferInfo.StoreKey, Include.NoRID, includeSizeAlternates);
                                // End TT#5026 - JSmith - Question about Size Alternates
                            }
                            aCell.Value = cellValue; 
                            //aCell.Value = 99;
                            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 22
							aCell.CellCanBeChanged = false;
							break;

						}
                        default:
                        {
                            aCell.CellCanBeChanged = false;
                            aCell.CellIsValid = false;
                            break;
                        }
                    }
                    break;
                }
                // END TT#1401 - AGallagher - VSW
				case (eAllocationWaferVariable.SizeInTransit):
				{
					// GeneralComponent colorComponent;
					// GeneralComponent sizeComponent;
					// AllocationColorSizeComponent colorSizeComponent;
					// switch (aAllocationWaferInfo.ComponentType)
					// {
					//	case (eComponentType.ColorAndSize):
					//	{
					//		colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
					//		break;
					//	}
					//	case (eComponentType.AllColors):
					//	case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
					//	{
					//		colorComponent = aAllocationWaferInfo.Component;
					//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
					//		colorSizeComponent = new AllocationColorSizeComponent(colorComponent, sizeComponent);
					//		break;
					//	}
					//	default:
					//	{
					//		throw new MIDException(eErrorLevel.fatal,
					//			(int)eMIDTextCode.msg_al_UnknownComponentType,
					//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
					//	}
					// }
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{	
							aCell.Value = aTotalProfile.GetStoreSizeInTransit(aHeaderProfile, aAllocationWaferInfo.StoreKey, this.GetSizeComponent(aAllocationWaferInfo), false); // MID Track 3209 Show "actual" size OH and IT in Size Review
							aCell.CellCanBeChanged = false;
							break;

						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreSizeInTransitPctToColorInTransit(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aAllocationWaferInfo.StoreKey);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}										
					}
					break;
				}
				case (eAllocationWaferVariable.SizeOnHand):
				{
					// GeneralComponent colorComponent;
					// GeneralComponent sizeComponent;
					// AllocationColorSizeComponent colorSizeComponent;
					// switch (aAllocationWaferInfo.ComponentType)
					// {
					//	case (eComponentType.ColorAndSize):
					//	{
					//		colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
					//		break;
					//	}
					//	case (eComponentType.AllColors):
					//	case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
					//	{
					//		colorComponent = aAllocationWaferInfo.Component;
					//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
					//		colorSizeComponent = new AllocationColorSizeComponent(colorComponent, sizeComponent);
					//		break;
					//	}
					//	default:
					//	{
					//		throw new MIDException(eErrorLevel.fatal,
					//			(int)eMIDTextCode.msg_al_UnknownComponentType,
					//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
					//	}
					// }
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = aTotalProfile.GetStoreSizeOnHand(aHeaderProfile, aAllocationWaferInfo.StoreKey, this.GetSizeComponent(aAllocationWaferInfo), false); // MID Track 3209 Show actual OH and IT on Size Review
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreSizeOnHandPctToColorOnHand(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aAllocationWaferInfo.StoreKey);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeOnHandPlusIT):
				{
					// GeneralComponent colorComponent;
					// GeneralComponent sizeComponent;
					// AllocationColorSizeComponent colorSizeComponent;
					// switch (aAllocationWaferInfo.ComponentType)
					// {
					//	case (eComponentType.ColorAndSize):
					//	{
					//		colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
					//		break;
					//	}
					//	case (eComponentType.AllColors):
					//	case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
					//	{
					//		colorComponent = aAllocationWaferInfo.Component;
					//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
					//		colorSizeComponent = new AllocationColorSizeComponent(colorComponent, sizeComponent);
					//		break;
					//	}
					//	default:
					//	{
					//		throw new MIDException(eErrorLevel.fatal,
					//			(int)eMIDTextCode.msg_al_UnknownComponentType,
					//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
					//	}
					// }
					AllocationColorSizeComponent colorSizeComponent = this.GetSizeComponent(aAllocationWaferInfo);
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aTotalProfile.GetStoreSizeOnHand(aHeaderProfile, aAllocationWaferInfo.StoreKey, colorSizeComponent, false)       // MID Track 3209 Show actual OH and IT on Size Review
								+ aTotalProfile.GetStoreSizeInTransit(aHeaderProfile, aAllocationWaferInfo.StoreKey, colorSizeComponent, false); // MID Track 3209 Show actual OH and IT in Size Review
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreSize_OHplusIT_PctToColorOHplusIT(
                            //    aHeaderProfile,
                            //    colorSizeComponent,
                            //    aAllocationWaferInfo.StoreKey);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
//							BEGIN MID Track #2468 Cannot Change Size Total
//							aCell.CellCanBeChanged = false;
//							break;
//							END MID Track #2468
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreSizeNeed(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey,
								this.GetSizeComponent(aAllocationWaferInfo), // MID track 4291 add fill variables to size review
								false,                                     // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false);                                    // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
//							double thisValue = 0.0;
//							double totalValue = 0.0;
//							if (totalValue > 0)
//							{
//								aCell.Value = thisValue * 100 / totalValue;
//							}
//							else
//							{
//								aCell.Value = 0.0;
//							}
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

				case (eAllocationWaferVariable.SizePctNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreSizePctNeed(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey,
								this.GetSizeComponent(aAllocationWaferInfo),   // MID track 4291 add fill variables to size review
								false,                                        // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false);                                       // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizePlan):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreSizePlan(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey,
								this.GetSizeComponent(aAllocationWaferInfo),   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false, // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false);   // MID track 4921 AnF#666 Fill to Size Plan Enhancement
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
//							double thisValue = 0.0;
//							double totalValue = 0.0;
//							if (totalValue > 0)
//							{
//								aCell.Value = thisValue * 100 / totalValue;
//							}
//							else
//							{
//								aCell.Value = 0.0;
//							}
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeTotalAllocated):
				{
					// GeneralComponent colorComponent;
					// GeneralComponent sizeComponent;
					// switch (aAllocationWaferInfo.Component.ComponentType)
					// {
					//	case (eComponentType.SpecificColor):
					//	{
					//		colorComponent = aAllocationWaferInfo.Component;
					//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
					//		break;
					//	}
					//	case (eComponentType.ColorAndSize):
					//	{
					//		colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
					//		sizeComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
					//		break;
					//	}
					//	case (eComponentType.AllColors):
					//	{
					//		colorComponent = aAllocationWaferInfo.Component;
					// 		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
					//		break;
					//	}
					//	default:
					//	{
					//		throw new MIDException(eErrorLevel.fatal,
					//			(int)eMIDTextCode.msg_al_UnknownComponentType,
					//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
					//	}
					// }
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
//							BEGIN MID Track #2468 Cannot Change Size Total
//							AllocationColorSizeComponent acsc = 
//								new AllocationColorSizeComponent(colorComponent, sizeComponent);
//							aCell.Value = 
//								aTotalProfile.GetStoreQtyAllocated
//								(
//								aHeaderProfile,
//								acsc,
//								aAllocationWaferInfo.StoreKey
//								);
//							aCell.CellCanBeChanged = false;
//							// BEGIN MID Track # 1511 Highlight Stores that are Out Of Balance
//							aCell.StoreAllocationOutOfBalance = aTotalProfile.IsStoreAllocationOutOfBalance
//								(
//								aHeaderProfile,
//								colorComponent,
//								aAllocationWaferInfo.StoreKey
//								);
//							// END MID Track # 1511
							AllocationColorSizeComponent acsc =
								this.GetSizeComponent(aAllocationWaferInfo);
								// new AllocationColorSizeComponent(colorComponent, sizeComponent);
							aCell.Value = 
								aTotalProfile.GetStoreQtyAllocated
								(
								aHeaderProfile,
								acsc,
								aAllocationWaferInfo.StoreKey
								);
							aCell.CellCanBeChanged = aCellCanChange;
                            aCell.MayExceedCapacityMaximum = 
								aTotalProfile.GetStoreMayExceedCapacity
								(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey
								);
							aCell.MayExceedGradeMaximum = 
								aTotalProfile.GetStoreMayExceedMax
								(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey
								);
							aCell.MayExceedPrimaryMaximum =
								aTotalProfile.GetStoreMayExceedPrimaryMaximum
								(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey
								);
							aCell.GradeMaximumValue = double.MaxValue;
							aCell.PrimaryMaximumValue =
								aTotalProfile.GetStorePrimaryMaximum
								(
								aHeaderProfile,
								acsc,
								aAllocationWaferInfo.StoreKey
								);
							aCell.MinimumValue = 0;
							aCell.StoreExceedsCapacity = aTotalProfile.GetStoreExceedsCapacity(aAllocationWaferInfo.StoreKey);
							aCell.StoreAllocationOutOfBalance = aTotalProfile.IsStoreAllocationOutOfBalance
								(
								aHeaderProfile,
								acsc,
								aAllocationWaferInfo.StoreKey
								);
//							END MID Track #2468
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							aCell.Value = aTotalProfile.GetStoreSizeAllocatedPctToColorAllocated
								(
								aHeaderProfile,
								this.GetSizeComponent(aAllocationWaferInfo),
								// new AllocationColorSizeComponent(colorComponent, sizeComponent),
								this._trans.StoreIndexRID(aAllocationWaferInfo.StoreKey)
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizePositiveNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreSizePosNeed(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey,
								this.GetSizeComponent(aAllocationWaferInfo));
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							//	AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
//							double thisValue = 0.0;
//							double totalValue = 0.0;
//							if (totalValue > 0)
//							{
//								aCell.Value = thisValue * 100 / totalValue;
//							}
//							else
//							{
//								aCell.Value = 0.0;
//							}
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizePositivePctNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreSizePosPctNeed(
								aHeaderProfile,
								aAllocationWaferInfo.StoreKey,
								this.GetSizeComponent(aAllocationWaferInfo));
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.CurrentWeekToDaySales):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
	                    	// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreWeekToDaySales(ikt, aAllocationWaferInfo.StoreKey);
							// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review	
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // BEGIN MID Track 3209 show "actual" onhand and Intransit on Size Review
				case (eAllocationWaferVariable.CurveAdjdSizeInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{	
							aCell.Value = aTotalProfile.GetStoreSizeInTransit(aHeaderProfile, aAllocationWaferInfo.StoreKey, this.GetSizeComponent(aAllocationWaferInfo), true); // MID Track 3209 Show actual OH and IT on Size Review
							aCell.CellCanBeChanged = false;
							break;

						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreSizeInTransitPctToColorInTransit(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aAllocationWaferInfo.StoreKey);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}										
					}
					break;
				}
				case (eAllocationWaferVariable.CurveAdjdSizeOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = aTotalProfile.GetStoreSizeOnHand(aHeaderProfile, aAllocationWaferInfo.StoreKey, this.GetSizeComponent(aAllocationWaferInfo), true); // MID Track 3209 Show actual OH and IT in Size Review
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreSizeOnHandPctToColorOnHand(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aAllocationWaferInfo.StoreKey);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.CurveAdjdSizeOnHandPlusIT):
				{
					AllocationColorSizeComponent colorSizeComponent = this.GetSizeComponent(aAllocationWaferInfo);
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aTotalProfile.GetStoreSizeOnHand(aHeaderProfile, aAllocationWaferInfo.StoreKey, colorSizeComponent, true)       // MID Track 3209 Show actual OH and IT on Size Review
								+ aTotalProfile.GetStoreSizeInTransit(aHeaderProfile, aAllocationWaferInfo.StoreKey, colorSizeComponent, true); // MID Track 3209 Show actual OH and IT on Size Review
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreSize_OHplusIT_PctToColorOHplusIT(
                            //    aHeaderProfile,
                            //    colorSizeComponent,
                            //    aAllocationWaferInfo.StoreKey);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // END MID Track 3209 show "actual" onhand and Intransit on Size Review
					// begin MID Track 4282 Velocity overlays Fill Size Holes Allocation
				case (eAllocationWaferVariable.PreSizeAllocated):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Size):
									{
										break;
									}
									case (eAllocationSelectionViewType.Velocity):
            						{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.Total):
											case(eComponentType.Bulk):
											{
												colorSizeComponent = new AllocationColorSizeComponent
													(new GeneralComponent(eGeneralComponentType.AllColors),
													new GeneralComponent(eGeneralComponentType.AllSizes));
												break;
											}
											case (eComponentType.DetailType):
											{
												if (aHeaderProfile.BulkIsDetail)
												{
													colorSizeComponent = new AllocationColorSizeComponent
														(new GeneralComponent(eGeneralComponentType.AllColors),
														new GeneralComponent(eGeneralComponentType.AllSizes));
												}
												else
												{
													colorSizeComponent = null;
												}
												break;
											}
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreQtyAllocated(aHeaderProfile, colorSizeComponent, aAllocationWaferInfo.StoreKey);
											aCell.CellIsValid = true;
                                    	}
										else
										{
											break;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4282 Velocity overlays Fill Size Holes Allocation
                    // begin TT#59 Implement Store Temp Locks
                case (eAllocationWaferVariable.StorePriority):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreAllocationPriority(aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.AvailableCapacity):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreAvailableCapacity(aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.CapacityExceedByPct):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    double capacityExceedByPct = aHeaderProfile.GetStoreCapacityExceedByPct(aAllocationWaferInfo.StoreKey);
                                    aCell.Value = capacityExceedByPct;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.CapacityMaximum):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    //int storeCapacityMaximum = aHeaderProfile.GetStoreCapacityMaximum(new GeneralComponent(eComponentType.Total), aAllocationWaferInfo.StoreKey, false); // TT#1074 - MD - Jellis - Group Allocation Inventory  Min Max Broken
                                    int storeCapacityMaximum = aHeaderProfile.GetStoreTotalCapacity(aAllocationWaferInfo.StoreKey); // TT#1074 - MD - Jellis - Group Allocation Inventory  Min Max Broken
                                    aCell.Value = storeCapacityMaximum;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.CapacityMaximumReached):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreCapacityMaximumReached(aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreMayExceedCapacity):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreMayExceedCapacity(aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreMayExceedMaximum):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreMayExceedMax(aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreUsedCapacity):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreUsedCapacity(aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StorePercentNeedLimitReached):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStorePercentNeedLimitReached(aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreMaximum):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    // begin TT#1176 - MD - Jellis - Group ALlocation Size need not observing inv min max
                                    //int storeMaximum = aHeaderProfile.GetStoreMaximum(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey, false); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                                    int storeMaximum;
                                    eMIDTextCode statusReasonCode;
                                    if (aHeaderProfile.TryGetStoreMaximum(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey, false, out storeMaximum, out statusReasonCode))
                                    {
                                        // end TT#1176 - MD - Jellis- Group Allocation Size need not observing inv min Max
                                        aCell.Value = storeMaximum;
                                        aCell.CellCanBeChanged = false;
                                        // begin TT#1176 - MD - Jellis - Group Allocation Size need not observing inv min Max
                                    }
                                    else
                                    {
                                        aCell.CellIsValid = false;
                                    }
                                    // end TT#1176 - MD - Jellis- Group ALlocation Size need not observing inv min max
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreMinimum):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    // begin TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                                    int minimum;
                                    eMIDTextCode aStatusReasonCode;
                                    if (aHeaderProfile.TryGetStoreMinimum(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey, false, out minimum, out aStatusReasonCode))
                                    {
                                        aCell.Value = minimum;
                                        aCell.CellCanBeChanged = false;
                                    }
                                    else
                                    {
                                        aCell.CellIsValid = false;
                                    }
                                    //aCell.Value =
                                    //    aHeaderProfile.GetStoreMinimum(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey, false); // TT#1074 - MD - Jellis - Group ALlocation Inventory Min Max Broken
                                    //aCell.CellCanBeChanged = false;
                                    // end TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.AllocationFromBottomUpSize):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreAllocationFromBottomUpSize(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.AllocationFromSizeBreakout):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreAllocationFromBulkSizeBreakOut(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.AllocationFromPackNeed):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreAllocationFromPackContentBreakOut(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.AllocationModifiedAftMultiSplit):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreAllocationModifiedAfterMultiHeaderSplit(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreColorMaximum):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null
                                    || aAllocationWaferInfo.ColorCodeRID == Include.NoRID)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreColorMaximum(aAllocationWaferInfo.ColorCodeRID, aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreColorMinimum):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null
                                    || aAllocationWaferInfo.ColorCodeRID == Include.NoRID)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreColorMinimum(aAllocationWaferInfo.ColorCodeRID, aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreFilledSizeHoles):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null
                                    || aAllocationWaferInfo.ColorCodeRID == Include.NoRID)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    // begin TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                    GeneralComponent component = aAllocationWaferInfo.Component;
                                    if (component.ComponentType == eComponentType.ColorAndSize)
                                    {
                                        GeneralComponent sizeComponent = ((AllocationColorSizeComponent)component).SizeComponent;
                                        switch (sizeComponent.ComponentType)
                                        {
                                            case (eComponentType.SpecificSize):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SizeRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    aCell.CellIsValid = true;
                                                    break;
                                                }
                                        }
                                    }
                                    if (aCell.CellIsValid)
                                    {
                                        // end TT#1443 (related 1445)  - Size Reveiw Gets Msg Unknown Component
                                        aCell.ValueAsString =
                                            aHeaderProfile.GetStoreFilledSizeHole(aAllocationWaferInfo.ColorCodeRID, aAllocationWaferInfo.StoreKey).ToString();
                                        aCell.CellCanBeChanged = false;
                                    }   // TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreHadNeed):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreHadNeed(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.StoreManuallyAllocated):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                                                        // begin TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                    GeneralComponent component = aAllocationWaferInfo.Component;
                                    if (component.ComponentType == eComponentType.ColorAndSize)
                                    {
                                        GeneralComponent sizeComponent = ((AllocationColorSizeComponent)component).SizeComponent;
                                        switch (sizeComponent.ComponentType)
                                        {
                                            case (eComponentType.SpecificSize):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SizeRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    aCell.CellIsValid = true;
                                                    break;
                                                }
                                        }
                                    }
                                    if (aCell.CellIsValid)
                                    {
                                        // end TT#1443 (related 1445)  - Size Reveiw Gets Msg Unknown Component
                                        aCell.ValueAsString =
                                            aHeaderProfile.GetStoreIsManuallyAllocated(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                        aCell.CellCanBeChanged = false;
                                    }   // TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.RuleAllocationFromChild):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreRuleAllocationFromChildComponent(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.RuleAllocationFromParent):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreRuleAllocationFromParentComponent(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.RuleAllocationFromChosenRule):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.ValueAsString =
                                        aHeaderProfile.GetStoreRuleAllocationFromChosenRule(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.ShippingStatus):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    // begin TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                    GeneralComponent component = aAllocationWaferInfo.Component;
                                    if (component.ComponentType == eComponentType.ColorAndSize)
                                    {
                                        GeneralComponent sizeComponent = ((AllocationColorSizeComponent)component).SizeComponent;
                                        switch (sizeComponent.ComponentType)
                                        {
                                            case (eComponentType.SpecificSize):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SizeRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    aCell.CellIsValid = true;
                                                    break;
                                                }
                                        }
                                    }
                                    if (aCell.CellIsValid)
                                    {
                                        // end TT#1443 (related 1445)  - Size Reveiw Gets Msg Unknown Component
                                        aCell.ValueAsString =
                                           MIDText.GetTextOnly((int)aHeaderProfile.GetStoreShippingStatus(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey));
                                        aCell.CellCanBeChanged = false;
                                    }   // TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.UnitNeedBefore):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreUnitNeedBefore(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.PercentNeedBefore):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStorePercentNeedBefore(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.QtyAllocatedByAuto):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    // begin TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                    GeneralComponent component = aAllocationWaferInfo.Component;
                                    if (component.ComponentType == eComponentType.ColorAndSize)
                                    {
                                        GeneralComponent sizeComponent = ((AllocationColorSizeComponent)component).SizeComponent;
                                        switch (sizeComponent.ComponentType)
                                        {
                                            case (eComponentType.SpecificSize):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SizeRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    aCell.CellIsValid = true;
                                                    break;
                                                }
                                        }
                                    }
                                    if (aCell.CellIsValid)
                                    {
                                        // end TT#1443 (related 1445)  - Size Reveiw Gets Msg Unknown Component
                                        aCell.Value =
                                            aHeaderProfile.GetStoreQtyAllocatedByAuto(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
                                        aCell.CellCanBeChanged = false;
                                    }   // TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.QtyAllocatedByRule):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreQtyAllocatedByRule(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.QtyShipped):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {
                                    aCell.Value =
                                        aHeaderProfile.GetStoreQtyShipped(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey);
                                    aCell.CellCanBeChanged = false;
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case (eAllocationWaferVariable.WasAutoAllocated):
                {
                    aCell.CellCanBeChanged = false;
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                if (aHeaderProfile == null)
                                {
                                    aCell.CellIsValid = false;
                                }
                                else
                                {

                                    // begin TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                    GeneralComponent component = aAllocationWaferInfo.Component;
                                    if (component.ComponentType == eComponentType.ColorAndSize)
                                    {
                                        GeneralComponent sizeComponent = ((AllocationColorSizeComponent)component).SizeComponent;
                                        switch (sizeComponent.ComponentType)
                                        {
                                            case (eComponentType.SpecificSize):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SizeRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizePrimaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).PrimarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            case (eComponentType.SpecificSizeSecondaryDim):
                                                {
                                                    if (((AllocationColorOrSizeComponent)sizeComponent).SecondarySizeDimRID == Include.NoRID)
                                                    {
                                                        aCell.CellIsValid = false;
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    aCell.CellIsValid = true;
                                                    break;
                                                }
                                        }
                                    }
                                    if (aCell.CellIsValid)
                                    {
                                        // end TT#1443 (related 1445)  - Size Reveiw Gets Msg Unknown Component
                                        aCell.ValueAsString =
                                        aHeaderProfile.GetStoreWasAutoAllocated(aAllocationWaferInfo.Component, aAllocationWaferInfo.StoreKey).ToString();
                                        aCell.CellCanBeChanged = false;
                                    }  // TT#1443 (related 1445) - Size Review Gets Msg Unknown Component
                                }
                                break;
                            }
                        default:
                            {
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // end TT#59 Implement Store Temp Locks
				default:
				{
					aCell.CellIsValid = false;
					aCell.CellCanBeChanged = false;
					break;
				}
			}
		}
		private AllocationColorSizeComponent GetSizeComponent(AllocationWaferInfo aAllocationWaferInfo)
		{
			GeneralComponent colorComponent;
			GeneralComponent sizeComponent;
			AllocationColorSizeComponent colorSizeComponent;
			switch (aAllocationWaferInfo.ComponentType)
			{
				case (eComponentType.ColorAndSize):
				{
					colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
					break;
				}
				case (eComponentType.AllColors):
				case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
				{
					colorComponent = aAllocationWaferInfo.Component;
					sizeComponent = new GeneralComponent(eComponentType.AllSizes);
					colorSizeComponent = new AllocationColorSizeComponent(colorComponent, sizeComponent);
					break;
				}
				default:
				{
					throw new MIDException(eErrorLevel.fatal,
						(int)eMIDTextCode.msg_al_UnknownComponentType,
						MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
				}
			}
			return colorSizeComponent;
		}
		#endregion ProcessGetStore


		#region ProcessGetStoreList
		/// <summary>
		/// Gets the selected cell's value for the specified StoreList
		/// </summary>
		/// <param name="aTotalProfile">Allocation Subtotal Profile</param>
		/// <param name="aHeaderProfile">Allocation Profile for selected Header.</param>
		/// <param name="aAllocationWaferInfo">Allocation Wafer Info</param>
		/// <param name="aCell">Cell</param>
		/// <param name="aStoreGrpRID">Store Group RID</param>
		/// <param name="aStoreGrpLvlRID">Store Group Level RID</param>
		/// <param name="aCellCanChange">True: cell may be changed; False: cell may not change</param>
		private void ProcessGetStoreList (
			AllocationSubtotalProfile aTotalProfile, 
			AllocationProfile aHeaderProfile, 
			AllocationWaferInfo aAllocationWaferInfo, 
			AllocationWaferCell aCell, 
			int aStoreGrpRID, 
			int aStoreGrpLvlRID,
			bool aCellCanChange
			)
		{
			IntransitKeyType ikt;
			switch (aAllocationWaferInfo.VariableKey)
			{
				case eAllocationWaferVariable.OriginalQuantityAllocated:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalOrigQtyAllocated
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID, 
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.Balance):
						{
							aCell.Value = 
								aTotalProfile.GetOrigAllocatedBalance
								(
								aHeaderProfile, 
								aAllocationWaferInfo.Component
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
					}
					break;
				}											
				case eAllocationWaferVariable.QuantityAllocated:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							GeneralComponent component;
							// begin MID Track 3644 "dimension" shows quantity allocated
                            // // BEGIN MID Track #2468 Cannot Change Size Total
							//if (aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize) // MID Track 3326 cannot manually key size qty when no secondary dimension
							//	// && this._trans.AllocationViewType == eAllocationSelectionViewType.Size) // MID Track 3326 cannot manually key size qty when no secondary dimension
							//{
							//	if (aAllocationWaferInfo.PrimarySizeKey == Include.NoRID)
						    //  {
							//		// begin MID Track 3326 cannot manually key size qty when no secondary dimension
							//		//if (aAllocationWaferInfo.SecondarySizeKey != Include.NoRID
							//		//	&& this._trans.AllocationCriteriaSecondarySizeCount > 1)
							//		//{
							//		//	aCell.CellIsValid = false;
							//		//	aCell.CellCanBeChanged = false;
							//		//}
							//		// end MID Track 3326 cannot manually key size qty when no secondary dimension
							//		component = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							//	}
							//	else
							//	{
							//		component = aAllocationWaferInfo.Component;
							//	}
							//}
							//else
							//{
                            //   component = aAllocationWaferInfo.Component;
							//} 
                            // // END MID Track #2468
							if (aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize
								&& this._trans.AllocationViewType == eAllocationSelectionViewType.Size
								&& aAllocationWaferInfo.SecondarySizeKey != Include.NoRID
								&& aAllocationWaferInfo.PrimarySizeKey == Include.NoRID)
							{
								if (this._trans.AllocationCriteriaSecondarySizeCount > 1)
								{
									aCell.CellIsValid = false;
									aCell.CellCanBeChanged = false;
								}
								component = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							}
							else
							{
								component = aAllocationWaferInfo.Component;
							}
							// end MID Track 3644 "dimension" shows quantity allocated
							if (aCell.CellIsValid)
							{
								int cellvalue = 
									aTotalProfile.GetStoreListTotalQtyAllocated
									(
									aHeaderProfile,
									component,
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade
									);
                                aCell.Value = cellvalue;
								if ((aCellCanChange == true &&
									aTotalProfile.GetStoreListTotalStoreCount
									(
									aHeaderProfile,
									component,
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade
									) == 0)) 
								{
									aCell.CellCanBeChanged = false;
								}
								else
								{
									aCell.CellCanBeChanged = aCellCanChange;
								}
							}
							break;
						}
                        case (eAllocationWaferVariable.AverageStore):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalAvgAllocated
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							if (aCellCanChange == true &&
								aTotalProfile.GetStoreListTotalStoreCount
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								) == 0)
							{
								aCell.CellCanBeChanged = false;
							}
							else
							{
								aCell.CellCanBeChanged = aCellCanChange;
							}
							break;
						}
						case (eAllocationWaferVariable.StoreCount):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalStoreCount
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							GeneralComponent colorComponent;
							GeneralComponent sizeComponent;
							switch (aAllocationWaferInfo.Component.ComponentType)
							{
								case (eComponentType.SpecificColor):
								{
									colorComponent = aAllocationWaferInfo.Component;
									sizeComponent = new GeneralComponent(eComponentType.AllSizes);
                                    aCell.CellIsValid = false;
									break;
								}
								case (eComponentType.ColorAndSize):
								{
									colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
									sizeComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
									if (this._trans.AllocationViewType == eAllocationSelectionViewType.Size
										&& aAllocationWaferInfo.SecondarySizeKey != Include.NoRID
										&& aAllocationWaferInfo.PrimarySizeKey == Include.NoRID
										&& this._trans.AllocationCriteriaSecondarySizeCount > 1)
									{
										aCell.CellIsValid = false;
										aCell.CellCanBeChanged = false;
									}
									break;
								}
								case (eComponentType.AllColors):
								{
									colorComponent = aAllocationWaferInfo.Component;
									sizeComponent = new GeneralComponent(eComponentType.AllSizes);
									break;
								}
								default:
								{
									throw new MIDException(eErrorLevel.fatal,
										(int)eMIDTextCode.msg_al_UnknownComponentType,
										MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
								}
							}
							if (aCell.CellIsValid)
							{
								aCell.Value = aTotalProfile.GetStoreListSizeAllocatedPctToColorAllocated
									(
									aHeaderProfile,
									new AllocationColorSizeComponent(colorComponent, sizeComponent),
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade
									);
								aCell.CellCanBeChanged = false;
							}
							break;
						}
						default:
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
					}
					break;
				}
                // BEGIN TT#1401 - JEllis - Urban Reservation Stores
                case eAllocationWaferVariable.StoreItemQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                aCell.Value =
                                    aTotalProfile.GetStoreListTotalItemQtyAllocated
                                    (
                                    aHeaderProfile,
                                    aAllocationWaferInfo.Component,
                                    aStoreGrpRID,
                                    aStoreGrpLvlRID,
                                    aAllocationWaferInfo.VolumeGrade
                                    );
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                case eAllocationWaferVariable.StoreIMOQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                aCell.Value =
                                    aTotalProfile.GetStoreListTotalImoQtyAllocated
                                    (
                                    aHeaderProfile,
                                    aAllocationWaferInfo.Component,
                                    aStoreGrpRID,
                                    aStoreGrpLvlRID,
                                    aAllocationWaferInfo.VolumeGrade
                                    );
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#1401 - AGallagher - Reservation Stores
           		case eAllocationWaferVariable.RuleResults:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalQtyAllocatedByRule
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component, 
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false; 
							break;
						}
						default:
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.StoreCount:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.InTransit):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalStoreCount
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade,
								false
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.Need):
						case (eAllocationWaferVariable.OpenToShip):
						{
							aCell.CellCanBeChanged = false;
							aCell.Value = 
								aTotalProfile.GetStoreListTotalStoreCount
								(
								true,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							break;
						}
						case (eAllocationWaferVariable.OnHand):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalStoreCount
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade,
								true
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.OTSVariance):
						{
							aCell.CellCanBeChanged = false;
							aCell.Value = 
								aTotalProfile.GetStoreListTotalStoreCount
								(
								false,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							break;
						}
						case (eAllocationWaferVariable.QuantityAllocated):
						case (eAllocationWaferVariable.None):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalStoreCount
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.AverageStore:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.InTransit):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalAvgInTransit
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.Need):
						case (eAllocationWaferVariable.OpenToShip):
						{
							aCell.CellCanBeChanged = false;
							aCell.Value = 
								aTotalProfile.GetStoreListTotalAvgNeed
								(
								true,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							break;
						}
						case (eAllocationWaferVariable.OnHand):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalAvgOnHand
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.OTSVariance):
						{
							aCell.CellCanBeChanged = false;
							aCell.Value = 
								aTotalProfile.GetStoreListTotalAvgNeed
								(
								false,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							break;
						}
						case (eAllocationWaferVariable.QuantityAllocated):
                        case (eAllocationWaferVariable.None):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalAvgAllocated
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							if (aCellCanChange == true &&
								aTotalProfile.GetStoreListTotalStoreCount
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								) == 0)
							{
								aCell.CellCanBeChanged = false;
							}
							else
							{
								aCell.CellCanBeChanged = aCellCanChange;
							}
							break;
						}
						default:
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.QtyReceived:
				{
					if (Enum.IsDefined(typeof(eComponentType), (int)aAllocationWaferInfo.ComponentType)) 
					{
						switch (aAllocationWaferInfo.SecondaryVariableKey)
						{
							case (eAllocationWaferVariable.None):
							{
								aCell.Value = aTotalProfile.GetQtyToAllocate
									(
									aHeaderProfile,
									aAllocationWaferInfo.Component
									);
								break;
							}
							default:
							{
								aCell.CellIsValid = false;
								break;
							}
						}
					}
					else
					{
						aCell.CellIsValid = false;
					}
					aCell.CellCanBeChanged = false;
					break;
				}
                // BEGIN TT#1401 - AGallagher - Reservation Stores
                case eAllocationWaferVariable.StoreIMOHistoryMaxQuantityAllocated:
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
                                //BEGIN TT#4409-VSuart--Velocity - Detail Section - VSW On Hand- All Store and Set-MID
                                if (aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
                                {
                                    aCell.Value = aTotalProfile.GetStoreListTotalImoHistory(ikt, aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade, aTotalProfile.VelocityStyleHnRID);
                                }
                                else
                                {
                                    aCell.Value = aTotalProfile.GetStoreListTotalImoHistory(ikt, aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
                                }
                                //END TT#4409-VSuart--Velocity - Detail Section - VSW On Hand- All Store and Set-MID
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#1401 - AGallagher - Reservation Stores
				case eAllocationWaferVariable.OnHand:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalOnHand(ikt, aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.StoreCount):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalStoreCount
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade,
								true
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.AverageStore):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalAvgOnHand
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.InTransit:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalInTransit(ikt, aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.StoreCount):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalStoreCount
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade,
								false
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.AverageStore):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalAvgInTransit
								(
								ikt, 
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.Sales:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							aCell.Value = aTotalProfile.GetStoreListTotalSalesPlan(aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.Stock:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							aCell.Value = aTotalProfile.GetStoreListTotalStockPlan(aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.Need:
				case eAllocationWaferVariable.OpenToShip:
				{
					// same as OpenToShip (ie. need after allocation)
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = aTotalProfile.GetStoreListTotalUnitNeed(aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.StoreCount):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalStoreCount
								(
								true,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.AverageStore):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalAvgNeed
								(
								true,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.PercentNeed:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							aCell.Value = aTotalProfile.GetStoreListTotalPercentNeed(aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.OTSVariance:
				{
					// Need before allocation occurs.
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalUnitNeed(aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade)
								- aTotalProfile.GetStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.StoreCount):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalStoreCount
								(
								false,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.AverageStore):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalAvgNeed
								(
								false,
								aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.Balance):
				{
					if (Enum.IsDefined(typeof(eComponentType), (int)aAllocationWaferInfo.ComponentType)) 
					{
						switch (aAllocationWaferInfo.SecondaryVariableKey)
						{
							case (eAllocationWaferVariable.None):
							{
								aCell.Value = 
									aTotalProfile.GetAllocatedBalance
									(
									aHeaderProfile, 
									aAllocationWaferInfo.Component
									);
								aCell.CellCanBeChanged = aCellCanChange;
								break;
							}
							default:
							{
								aCell.CellCanBeChanged = false;
								aCell.CellIsValid = false;
								break;
							}
						}
					}
					else
					{
						aCell.CellCanBeChanged = false;
						aCell.CellIsValid = false;
					}
					break;
				}
				case (eAllocationWaferVariable.Total):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							int cellvalue = 
								aTotalProfile.GetStoreListTotalQtyAllocated
								(
								aHeaderProfile, 
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.Value = cellvalue;       
							aCell.CellCanBeChanged = aCellCanChange;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							aCell.Value = aTotalProfile.GetStoreListSizeAllocatedPctToColorAllocated
								(
								aHeaderProfile,
								(AllocationColorSizeComponent)aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.PctSellThru):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
                            //aCell.Value = this._trans.GetStoreGrpPctSellThru(aStoreGrpRID, aStoreGrpLvlRID);   // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            aCell.Value = this._trans.Velocity.GetStoreGrpPctSellThru(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.VelocityRuleResult):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//							aCell.Value = aTotalProfile.GetStoreGrpVelocityRuleResult(aAllocationWaferInfo.Component, aStoreGrpRID,	aStoreGrpLvlRID);
							aCell.Value = aTotalProfile.GetStoreGrpVelocityRuleResult(aHeaderProfile.HeaderRID, aAllocationWaferInfo.Component, aStoreGrpRID,	aStoreGrpLvlRID);
// (CSMITH) - END MID Track #2410
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreGrpBasisInTransit(ikt, aStoreGrpRID, aStoreGrpLvlRID);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{	
							aCell.Value = aTotalProfile.GetStoreListBasisOnHand(aStoreGrpRID, aStoreGrpLvlRID);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                //BEGIN TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
                case (eAllocationWaferVariable.BasisVSWOnHand):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = aTotalProfile.GetStoreListBasisImoHistory(aStoreGrpRID, aStoreGrpLvlRID);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                //END TT#4262-VStuart-Velocity-VSW On Hand at the plan level is used when calculating ship up to, wos and fwos-MID
				case (eAllocationWaferVariable.StyleOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreGrpStyleOnHand(ikt, aStoreGrpRID, aStoreGrpLvlRID);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.StyleInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreGrpStyleInTransit(ikt, aStoreGrpRID, aStoreGrpLvlRID);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.AvgWeeklySales):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
                            // aCell.Value = aTotalProfile.GetStoreGrpAvgWeeklySales(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                            //aCell.Value = this._trans.Velocity.GetStoreGrpAvgWeeklySales(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            if (this._trans.VelocityCriteriaExists)
                            {
                                aCell.Value = this._trans.Velocity.GetStoreGrpAvgWeeklySales(aStoreGrpRID, aStoreGrpLvlRID);
                            }
                            else if (this._trans.BasisCriteriaExists)
                            {
                                aCell.Value = aTotalProfile.GetStoreGrpAvgWeeklySales(aStoreGrpRID, aStoreGrpLvlRID);
                            }
                            // End TT#638
                            aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

                // BEGIN TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)
                case (eAllocationWaferVariable.AvgWeeksOfSupply):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                            {
                                aCell.Value = this._trans.Velocity.GetStoreGrpAvgWeeklySupply(aStoreGrpRID, aStoreGrpLvlRID);
                                aCell.CellCanBeChanged = false;
                                break;
                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // END TT#508 - AGallagher - Velocity - Add WOS to Store Detail (#66)

				case (eAllocationWaferVariable.AvgWeeklyStock):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
                            // aCell.Value = aTotalProfile.GetStoreGrpAvgWeeklyStock(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                            //aCell.Value = this._trans.Velocity.GetStoreGrpAvgWeeklySales(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            if (this._trans.VelocityCriteriaExists)
                            {
                                aCell.Value = this._trans.Velocity.GetStoreGrpAvgWeeklyStock(aStoreGrpRID, aStoreGrpLvlRID);
                            }
                            else if (this._trans.BasisCriteriaExists)
                            {
                                aCell.Value = aTotalProfile.GetStoreGrpAvgWeeklyStock(aStoreGrpRID, aStoreGrpLvlRID);
                            }
                            // End TT#638
                            aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
            	case (eAllocationWaferVariable.BasisSales):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
                            // aCell.Value = aTotalProfile.GetStoreGrpBasisSales(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                            //aCell.Value = this._trans.Velocity.GetStoreGrpBasisSales(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            if (this._trans.VelocityCriteriaExists)
                            {
                                aCell.Value = this._trans.Velocity.GetStoreGrpBasisSales(aStoreGrpRID, aStoreGrpLvlRID);
                            }
                            else if (this._trans.BasisCriteriaExists)
                            {
                                aCell.Value = aTotalProfile.GetStoreGrpBasisSales(aStoreGrpRID, aStoreGrpLvlRID); 
                            }
                            // End TT#638
                            aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.BasisStock):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
                            // aCell.Value = aTotalProfile.GetStoreGrpBasisStock(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                            aCell.Value = this._trans.Velocity.GetStoreGrpBasisSales(aStoreGrpRID, aStoreGrpLvlRID);  // TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeTotalAllocated):
				{
					GeneralComponent colorComponent;
					GeneralComponent sizeComponent;
					switch (aAllocationWaferInfo.Component.ComponentType)
					{
						case (eComponentType.SpecificColor):
						{
							colorComponent = aAllocationWaferInfo.Component;
							sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							break;
						}
						case (eComponentType.ColorAndSize):
						{
							colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							sizeComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
							break;
						}
						case (eComponentType.AllColors):
						{
							colorComponent = aAllocationWaferInfo.Component;
							sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							break;
						}
						default:
						{
							throw new MIDException(eErrorLevel.fatal,
								(int)eMIDTextCode.msg_al_UnknownComponentType,
								MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
						}
					}
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							AllocationColorSizeComponent acsc = 
								new AllocationColorSizeComponent(colorComponent, sizeComponent);
							aCell.Value = aTotalProfile.GetStoreListTotalQtyAllocated(
								aHeaderProfile,
								acsc,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade);
//							BEGIN MID Track #2468 Cannot Change Size Total
//							aCell.CellCanBeChanged = false;
							aCell.CellCanBeChanged = aCellCanChange;
//							END MID Track #2468
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							aCell.Value = aTotalProfile.GetStoreListSizeAllocatedPctToColorAllocated
								(
								aHeaderProfile,
								new AllocationColorSizeComponent(colorComponent, sizeComponent),
								aStoreGrpRID,
								aStoreGrpLvlRID,
                                aAllocationWaferInfo.VolumeGrade
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeCurvePct):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreListTotalSizeCurvePct(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo));
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
                // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 22
                case (eAllocationWaferVariable.SizeVSWOnHand):
                {
                    switch (aAllocationWaferInfo.SecondaryVariableKey)
                    {
                        case (eAllocationWaferVariable.None):
                        case (eAllocationWaferVariable.Total):
                            {
                                ArrayList iktArray = GetSizeIntransitKeyTypes(aTotalProfile, aAllocationWaferInfo);
                                int cellValue = 0;
                                foreach (IntransitKeyType iKT in iktArray)
                                {
                                    cellValue += aTotalProfile.GetStoreListTotalImoHistory(aHeaderProfile, iKT,aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade);
                                }
                                aCell.Value = cellValue;
                                aCell.CellCanBeChanged = false;
                                break;

                            }
                        default:
                            {
                                aCell.CellCanBeChanged = false;
                                aCell.CellIsValid = false;
                                break;
                            }
                    }
                    break;
                }
                // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 22
                case (eAllocationWaferVariable.SizeInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
										
							aCell.Value = aTotalProfile.GetStoreListTotalSizeInTransit(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo), // MID Track 3209 Show "actual" onhand and IT in Size Review
								false);  // MID Track 3209 Show "actual" onhand and IT in Size Review
							aCell.CellCanBeChanged = false;
							break;

						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							// GeneralComponent colorComponent;
							// GeneralComponent sizeComponent;
							// switch (aAllocationWaferInfo.ComponentType)
							// {
							//	case (eComponentType.ColorAndSize):
							//	{
							//		colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							//		sizeComponent  = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
							//		break;
							//	}
							//	case (eComponentType.AllColors):
						    //  case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
							//	{
							//		colorComponent = aAllocationWaferInfo.Component;
							//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							//		break;
							//	}
							//	default:
							//	{
							//		throw new MIDException(eErrorLevel.fatal,
							//			(int)eMIDTextCode.msg_al_UnknownComponentType,
							//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
							//	}
							// }
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreListSizeInTransitPctToColorInTransit(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aStoreGrpRID,
                            //    aStoreGrpLvlRID,
                            //    aAllocationWaferInfo.VolumeGrade);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
										
					}
					break;
				}
				case (eAllocationWaferVariable.SizeOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = aTotalProfile.GetStoreListTotalSizeOnHand(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo), // MID Track 3209 Show actual OH and IT on Size Review
								false);                                      // MID Track 3209 Show actual OH and IT on Size Review
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							// GeneralComponent colorComponent;
							// GeneralComponent sizeComponent;
							// switch (aAllocationWaferInfo.ComponentType)
							// {
							//	case (eComponentType.ColorAndSize):
							//	{
							//		colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							//		sizeComponent  = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
							//		break;
							//	}
							//	case (eComponentType.AllColors):
							//	case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
							//	{
							//		colorComponent = aAllocationWaferInfo.Component;
							//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							//		break;
							//	}
							//	default:
							//	{
							//		throw new MIDException(eErrorLevel.fatal,
							//			(int)eMIDTextCode.msg_al_UnknownComponentType,
							//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
							//	}
							// }
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreListSizeOnHandPctToColorOnHand(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aStoreGrpRID,
                            //    aStoreGrpLvlRID,
                            //    aAllocationWaferInfo.VolumeGrade);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeOnHandPlusIT):
				{
					ArrayList IKT = this.GetSizeIntransitKeyTypes(aTotalProfile, aAllocationWaferInfo);
					AllocationColorSizeComponent colorSizeComponent = this.GetSizeComponent(aAllocationWaferInfo);
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aTotalProfile.GetStoreListTotalSizeOnHand
								(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								colorSizeComponent,  // MID Track 3209 Show actual OH and IT on Size Review
								false                // MID Track 3209 Show actual OH and IT on Size Review
								)
								+ aTotalProfile.GetStoreListTotalSizeInTransit
								(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								colorSizeComponent, // MID Track 3209 Show "actual" Onhand and IT in Size Review
								false               // MID Track 3209 Show "actual" OnHand and IT in Size Review
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							// GeneralComponent colorComponent;
							// GeneralComponent sizeComponent;
							// switch (aAllocationWaferInfo.ComponentType)
							// {
							//	case (eComponentType.ColorAndSize):
							//	{
							//		colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							//		sizeComponent  = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
							//		break;
							//	}
							//	case (eComponentType.AllColors):
							//	case (eComponentType.SpecificColor):  // MID Track 3076 No Summary Variables in Size Review
							//	{
							//		colorComponent = aAllocationWaferInfo.Component;
							//		sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							//		break;
							//	}
							//	default:
							//	{
							//		throw new MIDException(eErrorLevel.fatal,
							//			(int)eMIDTextCode.msg_al_UnknownComponentType,
							//			MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
							//	}
							// }
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreListSize_OHplusIT_PctToColorOHplusIT(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aStoreGrpRID,
                            //    aStoreGrpLvlRID,
                            //    aAllocationWaferInfo.VolumeGrade);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review REsponse Time
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizeNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreListTotalSizeNeed(
								aHeaderProfile,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo), // MID Track 4291 add fill variables to size review
								false,                                      // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false);                                     // MID track 4921 AnF#666 Fill to Size Plan Enhancement
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

				case (eAllocationWaferVariable.SizePctNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreListTotalSizePctNeed(
								aHeaderProfile,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo), // MID Track 4291 add fill variables to size review
								false,                                      // MID Track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false);                                     // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.SizePlan):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreListTotalSizePlan(
								aHeaderProfile,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo), // MID Track 4291 add fill variables to size review
								false,                                      // MID track 4291 add fill variables to size review // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
								false);                                     // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// begin MID Track 4291 add fill variables to size review
				case (eAllocationWaferVariable.FillSizeOwnNeed):  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalSizeNeed(
												aHeaderProfile, 
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												colorSizeComponent, 
												true, 
												false);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

				case (eAllocationWaferVariable.FillSizeOwnPctNeed):  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalSizePctNeed(
												aHeaderProfile, 
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												colorSizeComponent, 
												true, 
												false);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeOwnPlan):   // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalSizePlan(
												aHeaderProfile, 
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												colorSizeComponent, 
												true, 
												false);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4291 add fill variables to size review
				// begin MID Track 4921 AnF#666 Fill to Size plan Enhancement
				case (eAllocationWaferVariable.FillSizeFwdForecastNeed): 
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalSizeNeed(
												aHeaderProfile, 
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												colorSizeComponent, 
												true, 
												true);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}

				case (eAllocationWaferVariable.FillSizeFwdForecastPctNeed): 
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalSizePctNeed(
												aHeaderProfile, 
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												colorSizeComponent, 
												true, 
												true);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeFwdForecastPlan):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorSizeComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorSizeComponent =
													new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
												break;
											}
											default:
											{
												colorSizeComponent = null;
												break;
											}
										}
										if (colorSizeComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalSizePlan(
												aHeaderProfile, 
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												colorSizeComponent, 
												true, 
												true);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeFwdForecastSales): 
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorComponent = aAllocationWaferInfo.Component;
												break;
											}
											default:
											{
												colorComponent = null;
												break;
											}
										}
										if (colorComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalFillSizeBasisPlan(
												aHeaderProfile,
												colorComponent,
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
											    true);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.FillSizeFwdForecastStock):
				{
					// there is no call for this value!!!!
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{						
						case (eAllocationWaferVariable.None): 
						case (eAllocationWaferVariable.Total):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							if (aHeaderProfile !=null
								&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
							{
								switch (this._trans.AllocationViewType)
								{
									case (eAllocationSelectionViewType.Style):
									case (eAllocationSelectionViewType.Velocity):
									{
										break;
									}
									case (eAllocationSelectionViewType.Size):
									{
										GeneralComponent colorComponent;
										switch (aAllocationWaferInfo.ComponentType)
										{
											case (eComponentType.ColorAndSize):
											{
												colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
												break;
											}
											case (eComponentType.AllColors):
											case (eComponentType.SpecificColor):
											{
												colorComponent = aAllocationWaferInfo.Component;
												break;
											}
											default:
											{
												colorComponent = null;
												break;
											}
										}
										if (colorComponent != null)
										{
											aCell.Value = aTotalProfile.GetStoreListTotalFillSizeBasisPlan(
												aHeaderProfile,
												colorComponent,
												aStoreGrpRID,
												aStoreGrpLvlRID,
												aAllocationWaferInfo.VolumeGrade,
												false);
											aCell.CellIsValid = true;
										}
										break;
									}
									default:
									{
										break;
									}
								}
							}
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// end MID Track 4921 AnF#666 Fill to Size Plan Enhancement
				case (eAllocationWaferVariable.SizePositiveNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreListTotalSizePosNeed(
								aHeaderProfile,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo));
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
							aCell.CellIsValid = false;
							aCell.CellCanBeChanged = false;
							//										AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)allocationWaferInfo.Component;
//							double thisValue = 0.0;
//							double totalValue = 0.0;
//							if (totalValue > 0)
//							{
//								aCell.Value = thisValue * 100 / totalValue;
//							}
//							else
//							{
//								aCell.Value = 0.0;
//							}
//							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// begin MID Track 4282 Velocity overlays fill size holes allocation
				case (eAllocationWaferVariable.PreSizeAllocated):
				{
					aCell.CellIsValid = false;
					aCell.CellCanBeChanged = false;
					if (aHeaderProfile !=null
						&& aAllocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
					{
						switch (this._trans.AllocationViewType)
						{
							case (eAllocationSelectionViewType.Style):
							case (eAllocationSelectionViewType.Size):
							{
								break;
							}
							case (eAllocationSelectionViewType.Velocity):
							{
								GeneralComponent colorSizeComponent;
								switch (aAllocationWaferInfo.ComponentType)
								{
									case (eComponentType.Total):
									case(eComponentType.Bulk):
									{
										colorSizeComponent = new AllocationColorSizeComponent
											(new GeneralComponent(eGeneralComponentType.AllColors),
											new GeneralComponent(eGeneralComponentType.AllSizes));
										break;
									}
									case (eComponentType.DetailType):
									{
										if (aHeaderProfile.BulkIsDetail)
										{
											colorSizeComponent = new AllocationColorSizeComponent
												(new GeneralComponent(eGeneralComponentType.AllColors),
												new GeneralComponent(eGeneralComponentType.AllSizes));
										}
										else
										{
											colorSizeComponent = null;
										}
										break;
									}
									case (eComponentType.ColorAndSize):
									{
										colorSizeComponent = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
										break;
									}
									case (eComponentType.AllColors):
									case (eComponentType.SpecificColor):
									{
										colorSizeComponent =
											new AllocationColorSizeComponent(aAllocationWaferInfo.Component, new GeneralComponent(eComponentType.AllSizes));
										break;
									}
									default:
									{
										colorSizeComponent = null;
										break;
									}
								}
								if (colorSizeComponent != null)
								{
									aCell.Value = aTotalProfile.GetStoreListTotalQtyAllocated(
										aHeaderProfile,
										colorSizeComponent, 
										aStoreGrpRID,
										aStoreGrpLvlRID,
										aAllocationWaferInfo.VolumeGrade);
									aCell.CellIsValid = true;
								}
								break;
							}
							default:
							{
								break;
							}
						}
					}
					break;
				}
					// end MID Track 4282 Velocity overlays fill size holes allocation
				case (eAllocationWaferVariable.SizePositivePctNeed):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// BEGIN MID Track #2937 Size Onhand Incorrect
							// aCell.Value = 0.0;  
							aCell.Value = aTotalProfile.GetStoreListTotalSizePosPctNeed(
								aHeaderProfile,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo));
							// END MID Track #2937 Size Onhand Incorrect
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
		// BEGIN MID Track #2230   Display Current Week-to-Day Sales on Sku Review
				case (eAllocationWaferVariable.CurrentWeekToDaySales):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							ikt = new IntransitKeyType(Include.IntransitKeyTypeNoColor, Include.IntransitKeyTypeNoSize);
							aCell.Value = aTotalProfile.GetStoreListTotalWeekToDaySales
								(ikt,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade);
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
		// END MID Track #2230   Display Current Week-to-Day Sales on Sku Review
				// BEGIN MID Track # 3183 Transfer column has no total
				case (eAllocationWaferVariable.Transfer):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						{
							double transferUnits = 0;
							ProfileList storeList = 
								this._trans.GetAllocationGrandTotalProfile().GetStoresInVolumeGrade
								(aStoreGrpRID,
								aStoreGrpLvlRID, 
								aAllocationWaferInfo.VolumeGrade);
							foreach (StoreProfile sp in storeList)
							{
								transferUnits += this._trans.Velocity.GetStoreVelocityTransferQty
									(aHeaderProfile.HeaderRID,
									sp.Key);
							}
							aCell.Value = transferUnits;
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				// END MID Track # 3183 Transfer column has no total
					// BEGIN MID Track 3209 show "actual" onhand and Intransit on Size Review
				case (eAllocationWaferVariable.CurveAdjdSizeInTransit):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							AllocationColorSizeComponent colorSizeComponent = this.GetSizeComponent(aAllocationWaferInfo);

							aCell.Value = 
								aTotalProfile.GetStoreListTotalSizeInTransit
								(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								colorSizeComponent,  // MID Track 3209 Show actual OH and IT on Size Review
								true
							    );
							aCell.CellCanBeChanged = false;
							break;

						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1443 Size Review Get Msg Store RID cannot be neg
                            //aCell.Value = aTotalProfile.GetStoreSizeInTransitPctToColorInTransit(
                                 //(aHeaderProfile,
                                //this.GetSizeComponent(aAllocationWaferInfo),
                                //aAllocationWaferInfo.StoreKey);
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreListSizeInTransitPctToColorInTransit
                            //    (aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aStoreGrpRID,
                            //    aStoreGrpLvlRID,
                            //    aAllocationWaferInfo.VolumeGrade);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
                            // end TT#1443 Size Review Get Msg Store RID cannot be neg
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}										
					}
					break;
				}
				case (eAllocationWaferVariable.CurveAdjdSizeOnHand):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
                            // begin TT#1443 - Size Review Get Msg Store RID cannot be Neg
                            //aCell.Value = aTotalProfile.GetStoreSizeOnHand(aHeaderProfile, aAllocationWaferInfo.StoreKey, this.GetSizeComponent(aAllocationWaferInfo), true); // MID Track 3209 Show actual OH and IT in Size Review
                            // end TT#1443 - Size Review Get Msg Store RID cannot be Neg
							aCell.Value = 
								aTotalProfile.GetStoreListTotalSizeOnHand
								(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								this.GetSizeComponent(aAllocationWaferInfo),  // MID Track 3209 Show actual OH and IT on Size Review
								true               // MID Track 3209 Show actual OH and IT on Size Review
								);
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1443 - Size Review Get Msg Store RID cannot be Neg
                            //aCell.Value = aTotalProfile.GetStoreSizeOnHandPctToColorOnHand(
                            //    aHeaderProfile,
                            //    this.GetSizeComponent(aAllocationWaferInfo),
                            //    aAllocationWaferInfo.StoreKey);
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value =
                            //    aTotalProfile.GetStoreListSizeOnHandPctToColorOnHand(
                            //    aHeaderProfile,
                            //    GetSizeComponent(aAllocationWaferInfo),
                            //    aStoreGrpRID,
                            //    aStoreGrpLvlRID,
                            //    aAllocationWaferInfo.VolumeGrade);
                            aCell.CellIsValid = false;
                            // end TT#1521 - JEllis - Size Review Response Time
                            // end TT#1443 - Size Review Get Msg Store RID cannot be Neg
                            aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.CurveAdjdSizeOnHandPlusIT):
				{
					AllocationColorSizeComponent colorSizeComponent = this.GetSizeComponent(aAllocationWaferInfo);
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							aCell.Value = 
								aCell.Value = 
								aTotalProfile.GetStoreListTotalSizeOnHand
								(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								colorSizeComponent,  // MID Track 3209 Show actual OH and IT on Size Review
								true                 // MID Track 3209 Show actual OH and IT on Size Review
								)
								+ aTotalProfile.GetStoreListTotalSizeInTransit
								(
								aHeaderProfile,
								aStoreGrpRID, 
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								colorSizeComponent, // MID Track 3209 Show "actual" Onhand and IT in Size Review
								true                // MID Track 3209 Show "actual" OnHand and IT in Size Review
								);	
							aCell.CellCanBeChanged = false;
							break;
						}
						case (eAllocationWaferVariable.PctToTotal):
						{
                            // begin TT#1443 - Size Review Get Msg Store RID cannot be Neg
                            //aCell.Value = aTotalProfile.GetStoreSize_OHplusIT_PctToColorOHplusIT(
                            //    aHeaderProfile,
                            //    colorSizeComponent,
                            //    aAllocationWaferInfo.StoreKey);
                            // begin TT#1521 - JEllis - Size Review Response Time
                            //aCell.Value = aTotalProfile.GetStoreListSize_OHplusIT_PctToColorOHplusIT(
                            //    aHeaderProfile,
                            //    colorSizeComponent,
                            //    aStoreGrpRID,
                            //    aStoreGrpLvlRID,
                            //    aAllocationWaferInfo.VolumeGrade);
                            aCell.CellIsValid = false;
                            // end TT1521 - JEllis - Size Review Response Time
                            // end TT#1443 - Size Reveiw Get Msg Store RID cannot be Neg
							aCell.CellCanBeChanged = false;
							break;
						}
						default:
						{
							aCell.CellCanBeChanged = false;
							aCell.CellIsValid = false;
							break;
						}
					}
					break;
				}
					// END MID Track 3209 show "actual" onhand and Intransit on Size Review
				default:
				{
					aCell.CellIsValid = false;
					aCell.CellCanBeChanged = false;
					break;
				}
			}
		}
		#endregion ProcessGetStoreList
		#endregion getCell

		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========

		#region setCell
		/// <summary>
		/// Sets the cell specified by the given coordinates to the given value.
		/// </summary>
		/// <param name="aValue">
		/// The double value that the cell will be updated with.
		/// </param>
		
		public void SetCellValue(double aValue)
		{
			int intValue = 0;
			try
			{
				intValue = Convert.ToInt32(aValue, CultureInfo.CurrentUICulture);
			}
			catch
			{
			}
			AllocationProfileList headerList = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.Allocation);
			AllocationProfile header = null;
			AllocationSubtotalProfile grandTotal = this._trans.GetAllocationSubtotalProfile(MIDText.GetTextOnly((int) eHeaderNode.GrandTotal));

			AllocationWaferInfo allocationWaferInfo =  intInspectWaferCoordinates();
	
			// begin TT#59 Temp Lock - lock fails when error
            if (allocationWaferInfo.HeaderType == eAllocationCoordinateType.Header)
			{
				header = (AllocationProfile)headerList.FindKey(allocationWaferInfo.HeaderKey);
                _trans.HoldHeaderAllocation(header);
			}
            // end TT#59 Temp Lock - lock fails when error
			switch (allocationWaferInfo.StoreNodeType)
			{
				case eStoreAllocationNode.All:
				{
                    ProcessSetStoreList 
						(
			            grandTotal,
						header,
                        allocationWaferInfo,
                        _trans.AllocationStoreAttributeID,
						Include.AllStoreTotal, 
						intValue
						);
					break;
				}		
				case eStoreAllocationNode.Set:
				{
					ProcessSetStoreList 
						(
						grandTotal,
						header,
						allocationWaferInfo,
						_trans.AllocationStoreAttributeID,
						allocationWaferInfo.SetKey, 
						intValue
						);
					break;
				}
				case eStoreAllocationNode.Store:
				{
					#region specific Store
					switch (allocationWaferInfo.VariableKey)
					{
						case eAllocationWaferVariable.QuantityAllocated:
							GeneralComponent component = allocationWaferInfo.Component;
							if (allocationWaferInfo.ComponentType == eComponentType.ColorAndSize
								&& this._trans.AllocationViewType == eAllocationSelectionViewType.Size
								&& allocationWaferInfo.PrimarySizeKey == Include.NoRID
								&& allocationWaferInfo.SecondarySizeKey != Include.NoRID
								&& this._trans.AllocationCriteriaSecondarySizeCount == 1)
							{
								component = ((AllocationColorSizeComponent)allocationWaferInfo.Component).ColorComponent;
							}
							grandTotal.SetStoreQtyAllocated(header, component, allocationWaferInfo.StoreKey, intValue);
							break;

						case eAllocationWaferVariable.StoreItemQuantityAllocated:
							GeneralComponent component1 = allocationWaferInfo.Component;
							if (allocationWaferInfo.ComponentType == eComponentType.ColorAndSize
								&& this._trans.AllocationViewType == eAllocationSelectionViewType.Size
								&& allocationWaferInfo.PrimarySizeKey == Include.NoRID
								&& allocationWaferInfo.SecondarySizeKey != Include.NoRID
								&& this._trans.AllocationCriteriaSecondarySizeCount == 1)
							{
								component1 = ((AllocationColorSizeComponent)allocationWaferInfo.Component).ColorComponent;
							}
							grandTotal.SetStoreItemQtyAllocated(header, component1, allocationWaferInfo.StoreKey, intValue);
							break;
//						BEGIN MID Track #2468 Cannot Change Size Total
						case (eAllocationWaferVariable.SizeTotalAllocated):
						{
							GeneralComponent colorComponent;
							GeneralComponent sizeComponent;
							switch (allocationWaferInfo.Component.ComponentType)
							{
								case (eComponentType.SpecificColor):
								{
									colorComponent = allocationWaferInfo.Component;
									sizeComponent = new GeneralComponent(eComponentType.AllSizes);
									break;
								}
								case (eComponentType.ColorAndSize):
								{
									colorComponent = ((AllocationColorSizeComponent)allocationWaferInfo.Component).ColorComponent;
									sizeComponent = ((AllocationColorSizeComponent)allocationWaferInfo.Component).SizeComponent;
									break;
								}
								case (eComponentType.AllColors):
								{
									colorComponent = allocationWaferInfo.Component;
									sizeComponent = new GeneralComponent(eComponentType.AllSizes);
									break;
								}
								default:
								{
									throw new MIDException(eErrorLevel.fatal,
										(int)eMIDTextCode.msg_al_UnknownComponentType,
										MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
								}
							}
							AllocationColorSizeComponent acsc = 
								new AllocationColorSizeComponent(colorComponent, sizeComponent);
							grandTotal.SetStoreQtyAllocated(header, acsc, allocationWaferInfo.StoreKey, intValue);
							break;
						}
//						END MID Track #2468
						case eAllocationWaferVariable.VelocityRuleType:
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
//							this._trans.Velocity.SetStoreVelocityRuleType(allocationWaferInfo.StoreKey, (eVelocityRuleType)intValue);  // , 0);
							this._trans.Velocity.SetStoreVelocityRuleType(header.HeaderRID, allocationWaferInfo.StoreKey, (eVelocityRuleType)intValue);  // , 0);
// (CSMITH) - END MID Track #2410
							break;
						case eAllocationWaferVariable.VelocityRuleQty:
// (CSMITH) - BEG MID Track #2410: Allow interactive processing with multiple headers
							this._trans.Velocity.SetStoreVelocityRuleQty(header.HeaderRID, allocationWaferInfo.StoreKey, aValue);
// (CSMITH) - END MID Track #2410
							break;
                        // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
                        case eAllocationWaferVariable.VelocityRuleTypeQty:
                            this._trans.Velocity.SetStoreVelocityRuleTypeQty(header.HeaderRID, allocationWaferInfo.StoreKey, aValue);
                            break;
                        // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57)
						default:
							break;
					}
					break;
					#endregion
				}
				default:
				{
					// must be balance
					#region Balance
					switch (allocationWaferInfo.VariableKey)
					{

						case eAllocationWaferVariable.QuantityAllocated:
						{
							GeneralComponent component = allocationWaferInfo.Component;
							if (allocationWaferInfo.ComponentType == eComponentType.ColorAndSize
								&& this._trans.AllocationViewType == eAllocationSelectionViewType.Size
								&& allocationWaferInfo.PrimarySizeKey == Include.NoRID
								&& allocationWaferInfo.SecondarySizeKey != Include.NoRID
								&& this._trans.AllocationCriteriaSecondarySizeCount == 1)
							{
								component = ((AllocationColorSizeComponent)allocationWaferInfo.Component).ColorComponent;
							}
							grandTotal.SetStoreListTotalQtyAllocated(
								header,
								component,
								_trans.AllocationStoreAttributeID, 
								Include.AllStoreTotal,
								null,
								grandTotal.GetQtyToAllocate(header, allocationWaferInfo.Component) - intValue
								);
							break;
						}
						default:
							break;
					}
					break;
					#endregion Balance
				}
			}
		}

		#region ProcessSetStoreList
		/// <summary>
		/// Sets the selected cell's value for the specified StoreList
		/// </summary>
		/// <param name="aTotalProfile">Allocation Subtotal Profile</param>
		/// <param name="aHeaderProfile">Allocation Profile for selected Header.</param>
		/// <param name="aAllocationWaferInfo">Allocation Wafer Info</param>
		/// <param name="aStoreGrpRID">Store Group RID</param>
		/// <param name="aStoreGrpLvlRID">Store Group Level RID</param>
		/// <param name="aValue">new value of cell</param>
		private void ProcessSetStoreList (
			AllocationSubtotalProfile aTotalProfile, 
			AllocationProfile aHeaderProfile, 
			AllocationWaferInfo aAllocationWaferInfo, 
			int aStoreGrpRID, 
			int aStoreGrpLvlRID,
	        int aValue
			)
		{
			switch (aAllocationWaferInfo.VariableKey)
			{
				case eAllocationWaferVariable.QuantityAllocated:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.None):
						case (eAllocationWaferVariable.Total):
						{
							// begin MID Track 3326 cannot manually key size qty when no secondary dimension
							GeneralComponent component = aAllocationWaferInfo.Component;
							if (aAllocationWaferInfo.ComponentType == eComponentType.ColorAndSize
								&& this._trans.AllocationViewType == eAllocationSelectionViewType.Size
								&& aAllocationWaferInfo.PrimarySizeKey == Include.NoRID
								&& aAllocationWaferInfo.SecondarySizeKey != Include.NoRID
								&& this._trans.AllocationCriteriaSecondarySizeCount == 1)
							{
								component = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							}
							// end MID Track 3326 cannot manually key size qty when no secondary dimension
							if (aTotalProfile.GetStoreListTotalQtyAllocated 
								(aHeaderProfile,
								//aAllocationWaferInfo.Component,  // MID Track 3326 cannot manually key size qty when no secondary dimension
								component,                         // MID Track 3326 cannot manually key size qty when no secondary dimension
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade) == 0)
							{
								int storeCount = aTotalProfile.GetStoreListTotalEligibleCount(
									aHeaderProfile,
									//aAllocationWaferInfo.Component,  // MID Track 3326 cannot manually key size qty when no secondary dimension
									component,                         // MID Track 3326 cannot manually key size qty when no secondary dimension
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade);
								int averageValue = 0;
								if (storeCount > 0)
								{
									averageValue = (int)(((double)aValue / (double)storeCount) + .05d);
								}
								aTotalProfile.SetStoreListTotalAvgAllocated(
									aHeaderProfile,
									//aAllocationWaferInfo.Component,  // MID Track 3326 cannot manually key size qty when no secondary dimension
									component,                         // MID Track 3326 cannot manually key size qty when no secondary dimension
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade,
									averageValue
									);
							}
							else
							{
								aTotalProfile.SetStoreListTotalQtyAllocated
									(
									aHeaderProfile,
									//aAllocationWaferInfo.Component,  // MID Track 3326 cannot manually key size qty when no secondary dimension
									component,                         // MID Track 3326 cannot manually key size qty when no secondary dimension
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade,
									aValue
									);
							}
							break;
						}
						case (eAllocationWaferVariable.AverageStore):
						{
							aTotalProfile.SetStoreListTotalAvgAllocated
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								aValue
								);
							break;
						}
					}
					break;
				}
				case eAllocationWaferVariable.AverageStore:
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.QuantityAllocated):
						case (eAllocationWaferVariable.None):
						{
							aTotalProfile.SetStoreListTotalAvgAllocated
								(
								aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade,
								aValue
								);
							break;
						}
					}
					break;
				}
				case (eAllocationWaferVariable.Balance):
				{
					if (Enum.IsDefined(typeof(eComponentType), (int)aAllocationWaferInfo.ComponentType)) 
					{
						switch (aAllocationWaferInfo.SecondaryVariableKey)
						{
							case (eAllocationWaferVariable.None):
							case (eAllocationWaferVariable.QuantityAllocated):
							{
	                            aTotalProfile.SetStoreListTotalQtyAllocated
									(
									aHeaderProfile,
									aAllocationWaferInfo.Component,
									aStoreGrpRID, 
									aStoreGrpLvlRID,
									null,
									aTotalProfile.GetQtyToAllocate(aHeaderProfile, aAllocationWaferInfo.Component) - aValue
									);
								break;
							}
						}
					}
					break;
				}
				case (eAllocationWaferVariable.Total):
				{
					switch (aAllocationWaferInfo.SecondaryVariableKey)
					{
						case (eAllocationWaferVariable.QuantityAllocated):
						case (eAllocationWaferVariable.None):
						{
							if (aTotalProfile.GetStoreListTotalQtyAllocated 
								(aHeaderProfile,
								aAllocationWaferInfo.Component,
								aStoreGrpRID,
								aStoreGrpLvlRID,
								aAllocationWaferInfo.VolumeGrade) == 0)
							{
								int storeCount = aTotalProfile.GetStoreListTotalEligibleCount(
									aHeaderProfile,
									aAllocationWaferInfo.Component,
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade);
								int averageValue = 0;
								if (storeCount > 0)
								{
									averageValue = (int)(((double)aValue / (double)storeCount) + .05d);
								}
								aTotalProfile.SetStoreListTotalAvgAllocated(
									aHeaderProfile,
									aAllocationWaferInfo.Component,
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade,
									averageValue
									);
							}
							else
							{
								aTotalProfile.SetStoreListTotalQtyAllocated
									(
									aHeaderProfile, 
									aAllocationWaferInfo.Component,
									aStoreGrpRID,
									aStoreGrpLvlRID,
									aAllocationWaferInfo.VolumeGrade,
									aValue
									);
							}
                            break;
						}
					}
					break;
				}
//				BEGIN MID Track #2468 Cannot Change Size Total
				case (eAllocationWaferVariable.SizeTotalAllocated):
				{
					GeneralComponent colorComponent;
					GeneralComponent sizeComponent;
					switch (aAllocationWaferInfo.Component.ComponentType)
					{
						case (eComponentType.SpecificColor):
						{
							colorComponent = aAllocationWaferInfo.Component;
							sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							break;
						}
						case (eComponentType.ColorAndSize):
						{
							colorComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).ColorComponent;
							sizeComponent = ((AllocationColorSizeComponent)aAllocationWaferInfo.Component).SizeComponent;
							break;
						}
						case (eComponentType.AllColors):
						{
							colorComponent = aAllocationWaferInfo.Component;
							sizeComponent = new GeneralComponent(eComponentType.AllSizes);
							break;
						}
						default:
						{
							throw new MIDException(eErrorLevel.fatal,
								(int)eMIDTextCode.msg_al_UnknownComponentType,
								MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
						}
					}
					AllocationColorSizeComponent acsc = new AllocationColorSizeComponent(colorComponent, sizeComponent);
					aTotalProfile.SetStoreListTotalQtyAllocated(aHeaderProfile, acsc, aStoreGrpRID, aStoreGrpLvlRID, aAllocationWaferInfo.VolumeGrade, aValue);
					break;
				}
//				END MID Track #2468
			}
		}
		#endregion ProcessSetStoreList
		#endregion setCell
			
		#region GetSizeIntransitKeyTypes
        // begin TT#1055 - MD - JEllis - Size Need on Size Review does not Include effect of VSW Onhand
        private ArrayList GetSizeIntransitKeyTypes(AllocationSubtotalProfile aTotalProfile, AllocationWaferInfo aAllocationWaferInfo)
        {
            return _trans.GetSizeIntransitKeyTypes(aAllocationWaferInfo.Component);
        }
        private ArrayList GetTotalIntransitKeyTypes(AllocationSubtotalProfile aTotalProfile, AllocationWaferInfo aAllocationWaferInfo)
        {
            return _trans.GetTotalSizeIntransitKeyTypes(aAllocationWaferInfo.Component);
        }
//        /// <summary>
//        /// Gets an arraylist of size intransit key types associated with a cell
//        /// </summary>
//        /// <param name="aAllocationWaferInfo">Wafer cell coordinate information</param>
//        /// <returns>Arraylist of "size" IntransitKeyTypes associated with the cell</returns>
//        private ArrayList GetSizeIntransitKeyTypes(AllocationSubtotalProfile aTotalProfile, AllocationWaferInfo aAllocationWaferInfo)
//        {
//            IntransitKeyType ikt;
//            ArrayList IKT = new ArrayList();
//            switch (aAllocationWaferInfo.ComponentType)
//            {
//                case (eComponentType.SpecificColor):
//                {
//                    ikt = new IntransitKeyType(
//                        ((AllocationColorOrSizeComponent)aAllocationWaferInfo.Component).ColorRID,
//                        Include.IntransitKeyTypeNoSize);
//                    IKT.Add(ikt);
//                    break;
//                }
//                case (eComponentType.ColorAndSize):
//                {
//                    AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
//                    AllocationColorOrSizeComponent sizeComponent;
//                    switch (acsc.ColorComponent.ComponentType)
//                    {
//                        case (eComponentType.SpecificColor):
//                        {
//                            HdrColorBin hcb = aTotalProfile.GetSubtotalHdrColorBin(((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID);
//                            switch (acsc.SizeComponent.ComponentType)
//                            {
//                                case (eComponentType.SpecificSize):
//                                {
//                                    ikt = new IntransitKeyType(hcb.ColorCodeRID, ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID);
//                                    IKT.Add(ikt);
//                                    break;
//                                }
//                                case (eComponentType.SpecificSizePrimaryDim):
//                                {
//                                    sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
//                                    SizeCodeList scl = this._trans.GetSizeCodeByPrimaryDim(sizeComponent.PrimarySizeDimRID);
//                                    foreach (SizeCodeProfile scp in scl)
//                                    {
//                                        // if (hcb.SizeIsInColor(scp.Key))
//                                        // {
//                                            ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
//                                            IKT.Add(ikt);
//                                        // }
//                                    }
//                                    break;
//                                }
//                                case (eComponentType.SpecificSizeSecondaryDim):
//                                {
//                                    sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
//                                    SizeCodeList scl = this._trans.GetSizeCodeBySecondaryDim(sizeComponent.SecondarySizeDimRID);
//                                    foreach (SizeCodeProfile scp in scl)
//                                    {
//                                        // if (hcb.SizeIsInColor(scp.Key))
//                                        // {
//                                            ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
//                                            IKT.Add(ikt);
//                                        // }
//                                    }
//                                    break;
//                                }
//                                default:
//                                {
//                                    // assume all sizes
//                                    foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
//                                    {
//                                        ikt = new IntransitKeyType(hcb.ColorCodeRID, hsb.SizeCodeRID); // Assortment: color/size changes
//                                        IKT.Add(ikt);
//                                    }
//                                    break;
//                                }
//                            }
//                            break;
//                        }
//                        default:
//                        {
//                            // assume All Colors
//                            foreach (HdrColorBin hcb in aTotalProfile.BulkColors.Values)
//                            {
//                                switch (acsc.SizeComponent.ComponentType)
//                                {
//                                    case (eComponentType.SpecificSize):
//                                    {
//                                        int sizeRID = ((AllocationColorOrSizeComponent)acsc.SizeComponent).SizeRID;
//                                        // if (hcb.SizeIsInColor(sizeRID))
//                                        // {
//                                            ikt = new IntransitKeyType(hcb.ColorCodeRID, sizeRID);
//                                            IKT.Add(ikt);
//                                        // }
//                                        break;
//                                    }
//                                    case (eComponentType.SpecificSizePrimaryDim):
//                                    {
//                                        sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
//                                        SizeCodeList scl = this._trans.GetSizeCodeByPrimaryDim(sizeComponent.PrimarySizeDimRID);
//                                        foreach (SizeCodeProfile scp in scl)
//                                        {
//                                            // if (hcb.SizeIsInColor(scp.Key))
//                                            // {
//                                                ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
//                                                IKT.Add(ikt);
//                                            // }
//                                        }
//                                        break;
//                                    }
//                                    case (eComponentType.SpecificSizeSecondaryDim):
//                                    {
//                                        sizeComponent = (AllocationColorOrSizeComponent)acsc.SizeComponent;
//                                        SizeCodeList scl = this._trans.GetSizeCodeBySecondaryDim(sizeComponent.SecondarySizeDimRID);
//                                        foreach (SizeCodeProfile scp in scl)
//                                        {
//                                            // if (hcb.SizeIsInColor(scp.Key))
//                                            // {
//                                                ikt = new IntransitKeyType(hcb.ColorCodeRID, scp.Key);
//                                                IKT.Add(ikt);
//                                            // }
//                                        }
//                                        break;
//                                    }
//                                    default:
//                                    {
//                                        // assume all sizes
//                                        foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
//                                        {
//                                            ikt = new IntransitKeyType(hcb.ColorCodeRID, hsb.SizeCodeRID); // Assortment: color/size changes
//                                            IKT.Add(ikt);
//                                        }
//                                        break;
//                                    }
//                                }
////								foreach (HdrSizeBin hsb in hcb.ColorSizes.Values)
////								{
////									ikt = new IntransitKeyType(hcb.ColorKey, hsb.SizeKey);
////									IKT.Add(ikt);
////								}
//                            }
//                            break;
//                        }
//                    }
//                    break;
//                }
//                default:
//                {
//                    ikt = new IntransitKeyType(
//                        Include.IntransitKeyTypeNoColor,
//                        Include.IntransitKeyTypeNoSize);
//                    IKT.Add(ikt);
//                    break;
//                }
//            }
//            return IKT;
//        }
//        private ArrayList GetTotalSizeIntransitKeyTypes(AllocationSubtotalProfile aTotalProfile, AllocationWaferInfo aAllocationWaferInfo)
//        {
//            IntransitKeyType ikt;
//            ArrayList IKTcolor = new ArrayList();
//            switch (aAllocationWaferInfo.ComponentType)
//            {
//                case (eComponentType.SpecificColor):
//                {
//                    ikt = new IntransitKeyType(
//                        ((AllocationColorOrSizeComponent)aAllocationWaferInfo.Component).ColorRID,
//                        Include.IntransitKeyTypeNoSize);
//                    IKTcolor.Add(ikt);
//                    break;
//                }
//                case (eComponentType.ColorAndSize):
//                {
//                    AllocationColorSizeComponent acsc = (AllocationColorSizeComponent)aAllocationWaferInfo.Component;
//                    switch (acsc.ColorComponent.ComponentType)
//                    {
//                        case (eComponentType.SpecificColor):
//                        {
//                            HdrColorBin hcb = aTotalProfile.GetSubtotalHdrColorBin(((AllocationColorOrSizeComponent)acsc.ColorComponent).ColorRID);
//                            ikt = new IntransitKeyType(hcb.ColorCodeRID, Include.IntransitKeyTypeNoSize);
//                            IKTcolor.Add(ikt);
//                            break;
//                        }
//                        default:
//                        {
//                            // assume All Colors
//                            foreach (HdrColorBin hcb in aTotalProfile.BulkColors.Values)
//                            {
//                                ikt = new IntransitKeyType(hcb.ColorCodeRID, Include.IntransitKeyTypeNoSize);
//                                IKTcolor.Add(ikt);
//                            }
//                            break;
//                        }
//                    }
//                    break;
//                }
//                default:
//                {
//                    IKTcolor.Add(new IntransitKeyType(
//                        Include.IntransitKeyTypeNoColor,
//                        Include.IntransitKeyTypeNoSize));
//                    break;
//                }
//            }
//            return IKTcolor;
//        }
        // end TT#1055 - MD - JEllis - Size Need on Size Review does not Include effect of VSW Onhand
		#endregion GetSizeIntransitKeyTypes

		#region ExamineCubeWaferCoordinateList
		/// <summary>
		/// Private method that examines the contents of a CubeWaferCoordinateList and sets cooresponding cube and value wafer flags.
		/// </summary>
		/// <returns>
		/// A PlanCubeGroupWaferInfo object containing the cube and value wafer flags.
		/// </returns>

		private AllocationWaferInfo intInspectWaferCoordinates()
		{
			AllocationWaferInfo waferInfo;

			waferInfo = new AllocationWaferInfo();

            string sizeLabel = string.Empty;    // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
			// evaluate the coordinates
			ProfileList scpl;
			foreach (AllocationWaferCoordinate coord in this)
			{

				switch (coord.CoordinateType)
				{
					case eAllocationCoordinateType.None:
						break;
					case eAllocationCoordinateType.Header:
						waferInfo.HeaderType = eAllocationCoordinateType.Header;
						waferInfo.HeaderKey = coord.Key;
						break;
					case eAllocationCoordinateType.PackName:
						waferInfo.ComponentType = eComponentType.SpecificPack;
						waferInfo.PackName = coord.PackName;
						waferInfo.SubtotalPackName = coord.SubtotalPackName;
						break;
					case eAllocationCoordinateType.Component:
						waferInfo.ComponentType = (eComponentType)coord.CoordinateSubType;
						if (waferInfo.ComponentType == eComponentType.SpecificColor)
						{
							waferInfo.ColorCodeRID = coord.Key;
						}
						break;
					case eAllocationCoordinateType.StoreAllocationNode:
					{
						waferInfo.StoreNodeType = (eStoreAllocationNode)coord.CoordinateSubType;
						switch ((eStoreAllocationNode)coord.CoordinateSubType)
						{
							case eStoreAllocationNode.Store:
								waferInfo.StoreKey = coord.Key;
								break;
							case eStoreAllocationNode.Set:
								waferInfo.SetKey = coord.Key;
								break;
							case eStoreAllocationNode.All:
								break;
						}
						break;
					}
					case (eAllocationCoordinateType.VolumeGrade):
					{
						waferInfo.VolumeGrade = coord.VolumeGrade;
						break;
					}
					case eAllocationCoordinateType.Size:
						waferInfo.SizeKey = coord.Key;
						break;
					case eAllocationCoordinateType.HeaderTotal:
						waferInfo.HeaderType = eAllocationCoordinateType.HeaderTotal;
						break;
					case eAllocationCoordinateType.BalanceChainToHeader:
						break;
					case eAllocationCoordinateType.Variable:
						// begin MID Track 4708 Size Performance Slow
						//if (Enum.IsDefined(typeof(eAllocationSecondaryWaferVariable), (int)coord.Key))
						//{
						//	waferInfo.SecondaryVariableKey = (eAllocationWaferVariable)coord.Key;
						//}
						//else if (waferInfo.VariableKey == eAllocationWaferVariable.None)
						//{
						//	waferInfo.VariableKey = (eAllocationWaferVariable)coord.Key;
						//}
						//else
						//{
						//	waferInfo.SecondaryVariableKey = (eAllocationWaferVariable)coord.Key;
						//}
						if (waferInfo.VariableKey == eAllocationWaferVariable.None)
						{
							if (CoordKeyIsSecondaryVariable(coord.Key))
							{
								waferInfo.SecondaryVariableKey = (eAllocationWaferVariable)coord.Key;
							}
							else 
							{
								waferInfo.VariableKey = (eAllocationWaferVariable)coord.Key;
							}
						}
						else
						{
							waferInfo.SecondaryVariableKey = (eAllocationWaferVariable)coord.Key;
						}
						// end MID Track 4708 Size Performance Slow
						break;
					case eAllocationCoordinateType.PrimarySize:
						waferInfo.PrimarySizeKey = coord.Key;
                        sizeLabel = coord.Label;      // TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)

//						if (waferInfo.SecondarySizeKey != Include.NoRID
//							&& coord.Key != Include.NoRID)
//						{
//							scpl =  this._trans.GetSizeCodeByPrimarySecondary(waferInfo.PrimarySizeKey, waferInfo.SecondarySizeKey);
//							if (scpl.Count > 0)
//							{
//								waferInfo.ComponentType = eComponentType.ColorAndSize;
//								waferInfo.SizeKey = ((SizeCodeProfile)scpl.ArrayList[0]).Key;
//							}
//						}
						break;
					case eAllocationCoordinateType.SecondarySize:
						// begin MID Track 4326 Cannot manually enter size in Size review
						waferInfo.SecondarySizeIsNone = false;
						if ((eAllocationCoordinateType)coord.CoordinateSubType == eAllocationCoordinateType.SecondarySizeNone)
						{
							waferInfo.SecondarySizeIsNone = true;
						}
						// end MID Track 4326 Cannot manually enter size in Size Review
						waferInfo.SecondarySizeKey = coord.Key;
//						if (waferInfo.PrimarySizeKey != Include.NoRID
//							&& coord.Key != Include.NoRID)
//						{
//							scpl =  this._trans.GetSizeCodeByPrimarySecondary(waferInfo.PrimarySizeKey, waferInfo.SecondarySizeKey);
//							if (scpl.Count > 0)
//							{
//								waferInfo.ComponentType = eComponentType.ColorAndSize;
//								waferInfo.SizeKey = ((SizeCodeProfile)scpl.ArrayList[0]).Key;
//							}						
//						}
						break;
					case eAllocationCoordinateType.SecondarySizeTotal:
					default:
						break;
				}
			}
			if (waferInfo.PrimarySizeKey != Include.NoRID
				|| waferInfo.SecondarySizeKey != Include.NoRID)
			{
				waferInfo.ComponentType = eComponentType.ColorAndSize;
                GeneralComponent sizeComponent;   
				if (waferInfo.PrimarySizeKey == Include.NoRID)
				{
					sizeComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSizeSecondaryDim, waferInfo.SecondarySizeKey);
				}
				else
				{
					if (waferInfo.SecondarySizeKey == Include.NoRID    // MID Track 4326 Cannot manually enter size in size review
						&& !waferInfo.SecondarySizeIsNone)                   // MID Track 4326 Cannot manually enter size in size review
					{
						sizeComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSizePrimaryDim, waferInfo.PrimarySizeKey);
					}
					else
					{
                       	scpl = this._trans.GetSizeCodeByPrimarySecondary(waferInfo.PrimarySizeKey, waferInfo.SecondarySizeKey);
						if (scpl.Count == 1)
						{
							waferInfo.SizeKey = ((SizeCodeProfile)scpl.ArrayList[0]).Key;
						}
						else if (scpl.Count > 1)
						{
                            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
                            //waferInfo.SizeKey = Include.NoRID; // want to return just the key for waferInfo header
                            string idText = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DupSizeNameSeparator);
                            foreach (SizeCodeProfile scp in scpl)
                            {
                                if (scp.SizeCodePrimary == sizeLabel)
                                {
                                    waferInfo.SizeKey = scp.Key;
                                    break;
                                }
                                else
                                {
                                    string sizeID;
                                    int index = sizeLabel.IndexOf(idText);
                                    sizeID = sizeLabel.Substring(index + idText.Length, sizeLabel.Length - (index + idText.Length));
                                    if (sizeID.Length > 0 && sizeID == scp.SizeCodeID)
                                    {
                                        waferInfo.SizeKey = scp.Key;
                                        break;
                                    }
                                }
                            }
                        }   // End TT#234  
						else
						{
							waferInfo.SizeKey = Include.NoRID;
						}
                        sizeComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSize, waferInfo.SizeKey);
                    }    
				}
                // BEGIN MID Track #2412 Cannot Open Sku or Size Review with multiple headers with color
                GeneralComponent colorComponent;
                if (waferInfo.ColorCodeRID == Include.NoRID)
                {
                    colorComponent = new GeneralComponent(eGeneralComponentType.AllColors);
                }
                else
                {
                    colorComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, waferInfo.ColorCodeRID);
                }
                // END MID Track #2412 
                waferInfo.Component = new AllocationColorSizeComponent(colorComponent, sizeComponent);
			}

			if (waferInfo.VariableKey == eAllocationWaferVariable.None)
			{
				waferInfo.VariableKey = waferInfo.SecondaryVariableKey;
				waferInfo.SecondaryVariableKey = eAllocationWaferVariable.None;
			}

			return waferInfo;
		}
		// begin MID Track 4708 Size performance slow
		private static Hashtable _secondaryWaferVariables = null;       // MID Track xxxx Size Performance
		private static bool CoordKeyIsSecondaryVariable(int aCoordKey)  // MID Track xxxx Size Performance
		{
			if (_secondaryWaferVariables == null)
			{
				_secondaryWaferVariables = new Hashtable();
				Array al_int = Enum.GetValues(typeof(eAllocationSecondaryWaferVariable));
				string[] al_name = Enum.GetNames(typeof(eAllocationSecondaryWaferVariable));
				for (int i=0; i<al_int.Length; i++) 
				{
					_secondaryWaferVariables.Add((int)al_int.GetValue(i), al_name[i]);
				}
			}
			return _secondaryWaferVariables.Contains(aCoordKey);
		}
		// end MID track 4708 Size Performance slow
		#endregion ExamineCubeWaferCoordinateList

		#region Clone or Copy Coordinates
		/// <summary>
		/// Creates a new coordinate list with a clone of the coordinates.
		/// </summary>
		/// <returns>
		/// A new instance of AllocationWaferCoordinateList with a clone of this objects information.
		/// </returns>

		new public AllocationWaferCoordinateList Clone()
		{
			AllocationWaferCoordinateList waferCoordinateList = new AllocationWaferCoordinateList(_trans);
			foreach (AllocationWaferCoordinate waferCoordinate in this)
			{
				waferCoordinateList.Add(waferCoordinate.Clone());
			}
			return waferCoordinateList;
		}
		
		/// <summary>
		/// make a new coordinate list that contains the same coordinates.
		/// </summary>
		/// <returns></returns>
		public AllocationWaferCoordinateList Copy()
		{
			AllocationWaferCoordinateList waferCoordinateList = new AllocationWaferCoordinateList(_trans);
			foreach (AllocationWaferCoordinate waferCoordinate in this)
			{
				waferCoordinateList.Add(waferCoordinate);
			}
			return waferCoordinateList;
		}
		#endregion Clone or Copy Coordinates

		#region Find
		public AllocationWaferCoordinate FindByType(eAllocationCoordinateType aType)
		{
			foreach( AllocationWaferCoordinate coord in this)
			{
				if (coord.CoordinateType == aType)
				{
					return coord;
				}
			}
			throw new Exception("Requested type not found in AllocationWaferCellCoordinates");			
		}
		#endregion Find
		#endregion Methods
	}
	#endregion AllocationWaferCoordinateList

	#region AllocationWaferCoordinateListGroup
	/// <summary>
	/// The AllocationWaferCoordinateListGroup class contains a collection of AllocationWaferCoordinateList objects.
	/// </summary>
	/// <remarks>
	/// This class allows for each column and row to have different collections of AllocationWaferCoordinateList objects.
	/// </remarks>

	[Serializable]
	public class AllocationWaferCoordinateListGroup : System.Collections.ArrayList
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationWaferCoordinateListGroup.
		/// </summary>

		public AllocationWaferCoordinateListGroup()
			: base()
		{
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the AllocationWaferCoordinateList at the given integer index.
		/// </summary>
		
		new public AllocationWaferCoordinateList this[int aIndex]
		{
			get	{ return (AllocationWaferCoordinateList)base[aIndex]; }
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region Copy or Clone
		/// <summary>
		/// builds a new coordinate list group with the same coordinate lists
		/// 
		/// /// </summary>
		/// 
		/// <returns></returns>
		public AllocationWaferCoordinateListGroup Copy()
		{
			AllocationWaferCoordinateListGroup lg = new AllocationWaferCoordinateListGroup();
			foreach (AllocationWaferCoordinateList list in this)
			{
				lg.Add(list);
			}
			return lg;
		}

		/// <summary>
		/// builds a new coordinate list group with a clone of the coordinate lists 
		/// </summary>
		/// <returns></returns>
		new public AllocationWaferCoordinateListGroup Clone()
		{
			AllocationWaferCoordinateListGroup lg = new AllocationWaferCoordinateListGroup();
			foreach (AllocationWaferCoordinateList list in this)
			{
				lg.Add(list.Clone());
			}
			return lg;
		}
		#endregion Copy or Clone
		#endregion Methods
	}
	#endregion AllocationWaferCoordinateListGroup
	#endregion Allocation Wafer Coordinate Classes

	#region Allocation Wafer Classes
	#region AllocationWafer
	/// <summary>
	/// The AllocationWafer class defines the entire request for a wafer of data from a PlanAllocationGroup.
	/// </summary>
	/// <remarks>
	/// This class is used to define the layout of an array of Cells that will be returned from a call to the PlanAllocationGroup.  This class consists of: 1) a
	/// eAllocationType field that indicates the Allocation type to retrieve the data from; 2) a global WaferCoordinateList that holds all WaferCoordinates that are global
	/// to all requested Cells; and 3) a WaferCoordinateListGroup for both rows and columns that contain a WaferCoordinateList for each row or column.
	/// </remarks>
	[Serializable]
	public class AllocationWafer
	{
		#region Fields
		//=======
		// FIELDS
		//=======

//		private ApplicationSessionTransaction _trans;
//		private AllocationWaferCoordinateList _commonCoordinates;
		private AllocationWaferCoordinateListGroup _columns;
		private AllocationWaferCoordinateListGroup _rows;
		string[,] _rowLabels;
		string[,] _columnLabels;
		AllocationWaferCell[,] _cells;
      	#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationWafer.
		/// </summary>

		public AllocationWafer()
		{
			_columns = new AllocationWaferCoordinateListGroup();
			_rows = new AllocationWaferCoordinateListGroup();
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the row's WaferCoordinateListGroup.
		/// </summary>

		public AllocationWaferCoordinateListGroup Columns
		{
			get { return _columns; }
			set { _columns = value; }
		}

		/// <summary>
		/// Gets the row's WaferCoordinateListGroup.
		/// </summary>

		public AllocationWaferCoordinateListGroup Rows
		{
			get { return _rows; }
			set { _rows = value; }
		}

		public string[,] RowLabels
		{
			get
			{
				return _rowLabels;
			}
			set
			{
				_rowLabels = value;
			}
		}

		public string[,] ColumnLabels
		{
			get
			{
				return _columnLabels;
			}
			set
			{
				_columnLabels = value;
			}
		}

		public AllocationWaferCell[,] Cells
		{
			get
			{
				return _cells;
			}
			set
			{
				_cells = value;
			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#endregion Methods
	}
	#endregion AllocationWafer

	#region AllocationWaferGroup
	[Serializable]
	public class AllocationWaferGroup
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private AllocationWafer[,] _wafers;
		#endregion Fields

		#region Constructors		
		// ============
		// Constructors 
		// ============

		/// <summary>
		/// Creates an instance of AllocationWaferGroup
		/// </summary>
		/// <param name="columnCount">Number of columns in the wafer group</param>
		/// <param name="rowCount">Number of rows in the wafer group</param>
		public AllocationWaferGroup(int rowCount, int columnCount)
		{
			int rowSub;
			int colSub;
			
			_rowCount = rowCount;
			_colCount = columnCount;
			_wafers = new AllocationWafer[rowCount, columnCount];
			
			for (rowSub=0; rowSub<rowCount; rowSub++)
			{
				for (colSub=0; colSub<columnCount; colSub++)
				{
					_wafers[rowSub, colSub] = new AllocationWafer();
				}
			}
		}
		#endregion Constructors

		#region Properties
		// ==========
		// Properties 
		// ==========

		public AllocationWafer[,] Wafers
		{
			get
			{
				return _wafers;
			}
		}

		int _rowCount;
		public int RowCount
		{
			get { return _rowCount; }
		}

		int _colCount;
		public int ColumnCount
		{
			get { return _colCount; }
		}

		public AllocationWafer this[int row, int column]
		{
			get { return _wafers[row, column]; }
			set { _wafers[row, column] = value; }
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#endregion Methods
	}
    #endregion AllocationWaferGroup
	#endregion Allocation Wafer Classes

	#region Allocation Wafer Builders
	#region AllocationWaferBuilder
	/// <summary>
	/// The AllocationWaferBuilder class defines the entire request for a wafer of data from a PlanAllocationGroup.
	/// </summary>
	/// <remarks>
	/// This class is used to define the layout of an array of Cells that will be returned from a call to the PlanAllocationGroup.  This class consists of: 1) a
	/// eAllocationType field that indicates the Allocation type to retrieve the data from; 2) a global WaferCoordinateList that holds all WaferCoordinates that are global
	/// to all requested Cells; and 3) a WaferCoordinateListGroup for both rows and columns that contain a WaferCoordinateList for each row or column.
	/// </remarks>
	public class AllocationWaferBuilder
	{
		#region Fields
		//=======
		// FIELDS
		//=======

		private ApplicationSessionTransaction _trans;
		private AllocationWaferCoordinateList _commonCoordinates;
		private AllocationWaferCoordinateListGroup _columns;
		private AllocationWaferCoordinateListGroup _rows;
		private int _allocationWaferColumn;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AllocationWafer.
		/// </summary>

		public AllocationWaferBuilder(ApplicationSessionTransaction aTrans)
		{
			_trans = aTrans;
			_commonCoordinates = new AllocationWaferCoordinateList(_trans);
			_columns = new AllocationWaferCoordinateListGroup();
			_rows = new AllocationWaferCoordinateListGroup();
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the column of the wafer.
		/// </summary>

		public int AllocationWaferColumn
		{
			get { return _allocationWaferColumn; }
			set { _allocationWaferColumn = value; }
		}

		/// <summary>
		/// Gets the global WaferCoordinateList.
		/// </summary>

		public AllocationWaferCoordinateList CommonCoordinates
		{
			get { return _commonCoordinates; }
			set { _commonCoordinates = value; }
		}

		/// <summary>
		/// Gets the row's WaferCoordinateListGroup.
		/// </summary>

		public AllocationWaferCoordinateListGroup Columns
		{
			get { return _columns; }
			set { _columns = value; }
		}

		/// <summary>
		/// Gets the row's WaferCoordinateListGroup.
		/// </summary>

		public AllocationWaferCoordinateListGroup Rows
		{
			get { return _rows; }
			set { _rows = value; }
		}


		public string[,] RowLabels
		{
			get
			{
				string [,] labels;
				int colSub;
				int rowSub;
				int maxCols = 0;
				int rowCount = _rows.Count;
				int colCount;
				foreach (AllocationWaferCoordinateList list in _rows)
				{
					maxCols = Math.Max(maxCols,list.Count);
				}
				colCount = maxCols;
				labels = new string[rowCount,colCount];
				for (colSub=0;colSub<maxCols;colSub++)
				{
					for (rowSub=0; rowSub<rowCount; rowSub++)
					{
						if (colSub < _rows[rowSub].Count)
						{
							labels[rowSub,colSub] = _rows[rowSub][colSub].Label;
						}
					}
				}
				return labels;
			}
		}

		public string[,] ColumnLabels
		{
			get
			{
				string [,] labels;
				int colSub;
				int rowSub;
				int maxRows = 0;
				int rowCount;
				int colCount = _columns.Count;
				foreach (AllocationWaferCoordinateList list in _columns)
				{
					maxRows = Math.Max(maxRows,list.Count);
				}
				rowCount = maxRows;
				labels = new string[rowCount,colCount];
				for (colSub=0;colSub<colCount;colSub++)
				{
					for (rowSub=0; rowSub<rowCount; rowSub++)
					{
						if (rowSub < _columns[colSub].Count)
						{
							labels[rowSub,colSub] = _columns[colSub][rowSub].Label;
						}
					}
				}
				return labels;
			}
		}

		public AllocationWaferCell[,] Cells
		{
			get 
			{
				int rowSub;
				int colSub;
				int rowCount = _rows.Count;
				int colCount = _columns.Count;

//				MIDRetail.DataCommon.eStoreNodeType storeNodeType;
//				int storeNodeKey;
				
				AllocationWaferCoordinateList coordList;
			
				AllocationWaferCell[,] cellArray;
				cellArray = new AllocationWaferCell[rowCount,colCount];
				for (rowSub=0; rowSub<rowCount; rowSub++)
				{
					for (colSub=0; colSub<colCount; colSub++)
					{
						coordList = new AllocationWaferCoordinateList(_trans, AllocationWaferColumn);
						foreach(AllocationWaferCoordinate coord in _commonCoordinates)
						{
							coordList.Add(coord);
						}
						foreach(AllocationWaferCoordinate coord in _rows[rowSub])
						{
							coordList.Add(coord);
						}
						foreach(AllocationWaferCoordinate coord in _columns[colSub])
						{
							coordList.Add(coord);
						}
						// always build the first column because flags are hidden in this column
						if (colSub == 0)
						{
							coordList.BuildOverride = true;
						}
						else
						{
							coordList.BuildOverride = false;
						}
						cellArray[rowSub,colSub] = coordList.Cell;
					}
				}
				return cellArray;
			}	
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#region SetCellValue
		/// <summary>
		/// Updates the Allocation profile with the provided value
		/// </summary>
        // begin TT#59 Implement Temp Locks
		//public void SetCellValue(int aGridRow, int aGridCol, double aValue)
        public void SetCellValue(List<AllocationWaferCellChange> aCellChangeList)
            // end TT#59 Implement Temp Locks
		{
            try
            {
                AllocationWaferCoordinateList coordList;

                // begin TT#59 Implement Temp Locks
                foreach (AllocationWaferCellChange awcc in aCellChangeList)
                {
                    if (awcc.WaferRow != aCellChangeList[0].WaferRow
                        || awcc.WaferCol != aCellChangeList[0].WaferCol)
                    {
                        throw new Exception("Wafer Inconsistent");
                    }
                    // end TT#59 Implement Temp Locks
                    coordList = new AllocationWaferCoordinateList(_trans);
                    foreach (AllocationWaferCoordinate coord in _commonCoordinates)
                    {
                        coordList.Add(coord);
                    }
                    //foreach(AllocationWaferCoordinate coord in _rows[aGridRow])  // TT#59 Implement Temp Locks
                    foreach (AllocationWaferCoordinate coord in _rows[awcc.CellRowInWafer])  // TT#59 Implement Temp Locks
                    {
                        coordList.Add(coord);
                    }
                    //foreach(AllocationWaferCoordinate coord in _columns[aGridCol])  // TT#59 Implement Temp Locks
                    foreach (AllocationWaferCoordinate coord in _columns[awcc.CellColInWafer]) // TT#59 Implement Temp Locks
                    {
                        coordList.Add(coord);
                    }
                    //coordList.SetCellValue(aValue); // TT#59 Implement Temp Locks
                    coordList.SetCellValue(awcc.WaferCellValue);  // TT#59 Implement Temp Locks
                }   // TT#59 Implement Temp Locks
            }
            catch (Exception e)
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_al_AllocationWaferTrappedException,
                    MIDText.GetText(eMIDTextCode.msg_al_AllocationWaferTrappedException),
                    e);
            }
		}
		#endregion SetCellValue

		#region Copy or Clone
		/// <summary>
		/// Builds a new wafer with the same coordinate list groups
		/// </summary>
		public AllocationWaferBuilder Copy()
		{
			AllocationWaferBuilder wafer = new AllocationWaferBuilder(_trans);
			wafer._commonCoordinates = this._commonCoordinates;
			wafer._columns = this._columns;
			wafer._rows = this._rows;
			return wafer;
		}

		/// <summary>
		/// Builds a new wafer with a clone of the coordinate list groups
		/// </summary>
		public AllocationWaferBuilder Clone()
		{
			AllocationWaferBuilder wafer = new AllocationWaferBuilder(_trans);
			wafer._commonCoordinates = this._commonCoordinates.Clone()	;
			wafer._columns = this._columns.Clone();
			wafer._rows = this._rows.Clone();
			return wafer;
		}
		#endregion Copy

		#region SubWafer
		/// <summary>
		/// builds a new wafer with a subset of the original wafers rows and columns
		/// </summary>
		/// <param name="startingColumn"></param>
		/// <param name="endingColumn"></param>
		/// <param name="startingRow"></param>
		/// <param name="endingRow"></param>
		/// <returns></returns>
		public AllocationWaferBuilder SubWafer(int startingColumn, int endingColumn, int startingRow, int endingRow)
		{
			AllocationWaferBuilder wafer = new AllocationWaferBuilder(_trans);
			int i;
			wafer.CommonCoordinates = _commonCoordinates;
			for (i=startingColumn; i<=endingColumn; i++)
			{
				wafer._columns.Add(_columns[i]);
			}
			for (i=startingRow; i<=endingRow; i++)
			{
				wafer._rows.Add(_rows[i]);
			}
			return wafer;
		}
		#endregion SubWafer

		#region CellCoordinates
		public AllocationWaferCoordinateList CellCoordinates(int column, int row)
		{
			AllocationWaferCoordinateList cellCoordinates ;
			cellCoordinates = new AllocationWaferCoordinateList(_trans);
			foreach (AllocationWaferCoordinate coord in _columns[column])
			{
				cellCoordinates.Add(coord);
			}
			foreach (AllocationWaferCoordinate coord in _rows[row])
			{
				cellCoordinates.Add(coord);
			}
			return cellCoordinates;
		}
		#endregion CellCoordinates
		#endregion Methods
	}
	#endregion AllocationWaferBuilder

	#region AllocationWaferBuilderGroup
	public class AllocationWaferBuilderGroup
	{
		#region Fields
		//=======
		// FIELDS
		//=======
		private AllocationWaferBuilder[,] _wafers;
		private int _rowCount;
		private int _colCount;
		#endregion Fields

		#region Constructors
		// ============
		// Constructors 
		// ===========
		/// <summary>
		/// Creates an instance of AllocationWaferBuilderGroup
		/// </summary>
		/// <param name="aTrans">Transaction associated with this wafer builder group</param>
		/// <param name="columnCount">Number of columns</param>
		/// <param name="rowCount">Number of rows</param>
		public AllocationWaferBuilderGroup(ApplicationSessionTransaction aTrans, int rowCount, int columnCount)
		{
			int rowSub;
			int colSub;
			ApplicationSessionTransaction _trans = aTrans;

			_rowCount = rowCount;
			_colCount = columnCount;
			_wafers = new AllocationWaferBuilder[rowCount, columnCount];
			
			for (rowSub=0; rowSub<rowCount; rowSub++)
			{
				for (colSub=0; colSub<columnCount; colSub++)
				{
					_wafers[rowSub, colSub] = new AllocationWaferBuilder(_trans);
				}
			}
		}
		#endregion Constructors

		#region Properties
		// ==========
		// Properties 
		// ==========
		/// <summary>
		/// Gets number of rows in the Allocation Wafer Builder matrix
		/// </summary>
		public int RowCount
		{
			get { return _rowCount; }
		}

        /// <summary>
        /// Gets the number of column in the Allocation Wafer Builder matrix
        /// </summary>
		public int ColumnCount
		{
			get { return _colCount; }
		}

		/// <summary>
		/// Gets or sets the Allocation Wafer Builder for the specified row and column
		/// </summary>
		public AllocationWaferBuilder this[int row, int column]
		{
			get { return _wafers[row, column]; }
			set { _wafers[row, column] = value; }
		}
		#endregion Properties
		
		#region Methods
		//========
		// METHODS
		//========
		#endregion Methods
	}
	#endregion AllocationWaferBuilderGroup
	#endregion Allocation Wafer Builders

	#region AllocationWaferInfo
	public class AllocationWaferInfo
	{
		#region Fields
		//=======
		// FIELDS
		//=======

		private eAllocationCoordinateType _headerType;
		private int _headerKey;
				
		private MIDRetail.DataCommon.eStoreAllocationNode _storeNodeType;
		private int _storeKey;
		private int _setKey;
		private string _volumeGrade;
				
		private MIDRetail.DataCommon.eComponentType _componentType;
		private int _colorKey;
		private string _packName;
		private string _subtotalPackName;
		private int _sizeKey;
		private int _primarySizeKey;
		private int _secondarySizeKey;
		private bool _secondarySizeIsNone;         // MID Track 4326 Cannot manually enter size in Size Review
		private GeneralComponent _component = null;
		private eAllocationWaferVariable _variableKey;
		private eAllocationWaferVariable _variable2Key;
		#endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates a new instance of AllocationWaferInfo.
		/// </summary>
		public AllocationWaferInfo()
		{
			_headerType = eAllocationCoordinateType.None;
			_storeNodeType = (eStoreAllocationNode)(-1);
			_componentType = (eComponentType)(-1);
			_headerKey = Include.NoRID;
			_storeKey = Include.NoRID;
			_setKey = Include.NoRID;
			_colorKey = Include.NoRID;
			_packName = "";
			_subtotalPackName = "";
			_primarySizeKey = Include.NoRID;
			_secondarySizeKey = Include.NoRID;
			_secondarySizeIsNone = false; // MID Track 4326 Cannot manually enter size in Size Review
			_sizeKey = Include.NoRID;
			_variableKey = eAllocationWaferVariable.None;
			_variable2Key = eAllocationWaferVariable.None;
			_volumeGrade = null;
		}
		#endregion Constructors

		#region Properties
		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the header type for the wafer coordinate
		/// </summary>
		public eAllocationCoordinateType HeaderType
		{
			get
			{
				return _headerType;
			}
			set
			{
				_headerType = value;
			}
		}

		/// <summary>
		/// Gets or sets the Header key for the wafer coordinate
		/// </summary>
		public int HeaderKey
		{
			get
			{
				return _headerKey;
			}
			set
			{
				_headerKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the store node type for the wafer coordinate
		/// </summary>
		public eStoreAllocationNode StoreNodeType
		{
			get
			{
				return _storeNodeType;
			}
			set
			{
				_storeNodeType = value;
			}
		}

		/// <summary>
		/// Gets or sets the store key for the wafer coordinate
		/// </summary>
		public int StoreKey
		{
			get
			{
				return _storeKey;
			}
			set
			{
				_storeKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the store group level (attribute set) key for the wafer coordinate
		/// </summary>
		public int SetKey
		{
			get
			{
				return _setKey;
			}
			set
			{
				_setKey = value;
			}
		}


		/// <summary>
		/// Gets or sets the component type fot the wafer coordinate
		/// </summary>
		public eComponentType ComponentType
		{
			get
			{
				return _componentType;
			}
			set
			{
				_componentType = value;
			}
		}

		/// <summary>
		/// Gets a description of the component
		/// </summary>
		public GeneralComponent Component
		{
			get
			{
				if (_component == null)
				{
					switch (ComponentType)
					{
						case (eComponentType.Total):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case (eComponentType.GenericType):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case(eComponentType.DetailType):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case(eComponentType.Bulk):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case (eComponentType.AllSizes):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case (eComponentType.AllPacks):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case(eComponentType.AllNonGenericPacks):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case(eComponentType.AllGenericPacks):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case (eComponentType.AllColors):
						{
							_component = new GeneralComponent(ComponentType);
							break;
						}
						case (eComponentType.ColorAndSize):
						{
							break;
						}
						case (eComponentType.SpecificColor):
						{
							_component = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, this.ColorCodeRID);
							break;
						}
						case (eComponentType.SpecificPack):
						{
							if (this.HeaderType == eAllocationCoordinateType.Header)
							{
								_component = new AllocationPackComponent(this.PackName);
							}
							else
							{
								_component = new AllocationPackComponent(this.SubtotalPackName);
							}
							break;
						}
						case (eComponentType.SpecificSize):
						{ 
							_component = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSize, this.SizeKey);
							break;
						}
						default:
						{
							_component = new GeneralComponent(eComponentType.Total);
							break;
//							throw new MIDException(eErrorLevel.severe,
//								(int)eMIDTextCode.msg_al_UnknownComponentType,
//								MIDText.GetText(eMIDTextCode.msg_al_UnknownComponentType));
						}
					}
				}
				return _component;
			}
			set
			{
				_component = value;
			}
		}

		/// <summary>
		/// Gets or sets the color key for the wafer coordinate
		/// </summary>
		public int ColorCodeRID
		{
			get
			{
				return _colorKey;
			}
			set
			{
				_colorKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the pack name for the wafer coordinate
		/// </summary>
		public string PackName
		{
			get
			{
				return _packName;
			}
			set
			{
				_packName = value;
			}
		}

		/// <summary>
		/// Gets or sets the subtotal pack name for the wafer coordinate
		/// </summary>
		public string SubtotalPackName
		{
			get
			{
				return _subtotalPackName;
			}
			set
			{
				_subtotalPackName = value;
			}
		}

		/// <summary>
		/// Gets or sets the primary size key for the wafer coordinate
		/// </summary>
		public int PrimarySizeKey
		{
			get
			{
				return _primarySizeKey;
			}
			set
			{
				_primarySizeKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the secondary size key for the wafer coordinate
		/// </summary>
		public int SecondarySizeKey
		{
			get
			{
				return _secondarySizeKey;
			}
			set
			{
				_secondarySizeKey = value;
			}
		}

		// begin MID Track 4326 Cannot manually enter size in Size Review
		public bool SecondarySizeIsNone
		{
			get
			{
				return _secondarySizeIsNone;
			}
			set
			{
				_secondarySizeIsNone = value;
			}
		}
		// end MID Track 4326 Cannot manually enter size in Size Review

		/// <summary>
		/// Gets or sets the size key for the wafer coordinate
		/// </summary>
		public int SizeKey
		{
			get
			{
				return _sizeKey;
			}
			set
			{
				_sizeKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the variable key for the wafer coordinate
		/// </summary>
		public eAllocationWaferVariable VariableKey
		{
			get
			{
				return _variableKey;
			}
			set
			{
				_variableKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the secondary variable key for the wafer coordinate
		/// </summary>
		public eAllocationWaferVariable SecondaryVariableKey
		{
			get
			{
				return _variable2Key;
			}
			set
			{
				_variable2Key = value;
			}
		}

		/// <summary>
		/// Gets or sets the volume grade for the wafer coordinate
		/// </summary>
		public string VolumeGrade
		{
			get
			{
				return _volumeGrade;
			}
			set
			{
				_volumeGrade = value;
			}
		}
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		#endregion Methods
	}
	#endregion AllocationWaferInfo
}
   
