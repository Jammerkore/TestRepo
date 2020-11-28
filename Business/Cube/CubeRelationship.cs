using System;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The CubeRelationship class identifies the relationship between total and detail profiles.
	/// </summary>
	/// <remarks>
	/// Profiles in cubes can be connected in a hierarchial fashion, identifying profiles that are totals of other profiles.  By identifying
	/// these relationships, values of cells in a total profile can be automatically summed from the detail cells in its related profile.  This
	/// class provides the means of identifying the relationship between the total and detail profiles.  It can be used to define both the total-to-detail
	/// and detail-to-total relationship.
	/// </remarks>

	public class CubeRelationship
	{
		//=======
		// FIELDS
		//=======

		private eCubeType _cubeType;
		private CubeRelationshipItem[] _relationshipItems;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeRelationship using the given eCubeType, total eProfileType, and detail eProfileType.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType of the related cube, which is either the detail or total depending on the current Cube.
		/// </param>

		public CubeRelationship(eCubeType aCubeType, params CubeRelationshipItem[] aRelationshipItems)
		{
			try
			{
				if (aRelationshipItems.Length == 0)
				{
					throw new Exception("Must specify at least one CubeRelationshipItem during construction");
				}

				_cubeType = aCubeType;
				_relationshipItems = (CubeRelationshipItem[])aRelationshipItems.Clone();
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
		/// Gets the eCubeType of the related Cube.
		/// </summary>

		public eCubeType CubeType
		{
			get
			{
				return _cubeType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Routine that creates a list of CellReferences by using the detail-to-total ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		public void ProcessTotalSelector(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			try
			{
				_relationshipItems[0].RecurseTotalItems(aSourceCellRef, aCellRef, _relationshipItems, 0, aCellSelector, out aCancel);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Routine that creates a list of CellReferences by using the total-to-detail ProfileXRef for each CubeRelationshipItem on the total
		/// cube list.
		/// </summary>
		/// <param name="aSourceCellRef">
		/// The starting CellReference.
		/// </param>
		/// <param name="aCellRef">
		/// The current CellReference being built.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		public void ProcessDetailSelector(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CellSelector aCellSelector,
			out bool aCancel)
		{
			try
			{
				_relationshipItems[0].RecurseDetailItems(aSourceCellRef, aCellRef, _relationshipItems, 0, aCellSelector, out aCancel);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	abstract public class CubeRelationshipItem
	{
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
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		abstract public void RecurseTotalItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel);

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
        /// <param name="aRelationshipItems">
		/// An array of CubeRelationshipItem being processed.
		/// </param>
		/// <param name="aCellSelector">
		/// The CellSelector used to process the current CellReference.
		/// </param>
		/// <param name="aCancel">
		/// A parameter indicating the CellSelector has cancelled the selection process.
		/// </param>

		abstract public void RecurseDetailItems(
			CellReference aSourceCellRef,
			CellReference aCellRef,
			CubeRelationshipItem[] aRelationshipItems,
			int aCurrRelationshipItem,
			CellSelector aCellSelector,
			out bool aCancel);
	}

	/// <summary>
	/// The CubeRelationshipItem class identifies a total-detail relationship between two profile types.
	/// </summary>

	public class CubeSingleRelationshipItem : CubeRelationshipItem
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _totalProfileType;
		private eProfileType _detailProfileType;
		private eProfileListType _profileListType;

		//=============
		// CONSTRUCTORS
		//=============

		public CubeSingleRelationshipItem(eProfileType aTotalProfileType, eProfileType aDetailProfileType, eProfileListType aProfileListType)
		{
			try
			{
				_totalProfileType = aTotalProfileType;
				_detailProfileType = aDetailProfileType;
				_profileListType = aProfileListType;

				if (_profileListType == eProfileListType.None && (_totalProfileType == eProfileType.None || _detailProfileType == eProfileType.None))
				{
					throw new Exception("Cannot specify eProfileListType.None when either Total or Detail eProfileType = eProfileType.None");
				}
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
		/// Gets the eProfileType of the total profile.
		/// </summary>

		public eProfileType TotalProfileType
		{
			get
			{
				return _totalProfileType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the detail profile.
		/// </summary>

		public eProfileType DetailProfileType
		{
			get
			{
				return _detailProfileType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the detail profile.
		/// </summary>

		public eProfileListType ProfileListType
		{
			get
			{
				return _profileListType;
			}
		}

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
        /// <param name="aRelationshipItems">
		/// The list of CubeRelationshipItem being processed.
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
			ProfileXRef compXRef;
			ArrayList totalList;
			ProfileList profileList;

			try
			{
				aCancel = false;

				switch (_profileListType)
				{
					case eProfileListType.Master:
						profileList = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(_totalProfileType);
						break;
					case eProfileListType.Filtered:
						profileList = aSourceCellRef.Cube.CubeGroup.GetFilteredProfileList(_totalProfileType);
						break;
					default:
						profileList = null;
						break;
				}

				if (_totalProfileType != eProfileType.None &&
					_detailProfileType != eProfileType.None)
				{
					compXRef = (ProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(_totalProfileType, _detailProfileType));

					if (compXRef != null)
					{
						totalList = (ArrayList)compXRef.GetTotalList(aSourceCellRef[_detailProfileType]);

						if (totalList != null)
						{
							foreach (int profileKey in totalList)
							{
								if (profileList == null || profileList.Contains(profileKey))
								{
									aCellRef[_totalProfileType] = profileKey;

									if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
									{
										aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
									}
									else
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
				}
				else if (profileList != null && _totalProfileType != eProfileType.None)
				{
					foreach (Profile profile in profileList)
					{
						aCellRef[_totalProfileType] = profile.Key;

						if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
						{
							aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
						}
						else
						{
							aCellSelector.CheckItem(aCellRef, out aCancel);
						}

						if (aCancel)
						{
							return;
						}
					}
				}
				else
				{
					if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
					{
						aRelationshipItems[aCurrRelationshipItem + 1].RecurseTotalItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
					}
					else
					{
						aCellSelector.CheckItem(aCellRef, out aCancel);
					}

					if (aCancel)
					{
						return;
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
			ProfileXRef compXRef;
			ArrayList detailList;
			ProfileList profileList;

			try
			{
				aCancel = false;

				switch (_profileListType)
				{
					case eProfileListType.Master:
						profileList = aSourceCellRef.Cube.CubeGroup.GetMasterProfileList(_detailProfileType);
						break;
					case eProfileListType.Filtered:
						profileList = aSourceCellRef.Cube.CubeGroup.GetFilteredProfileList(_detailProfileType);
						break;
					default:
						profileList = null;
						break;
				}

				if (_totalProfileType != eProfileType.None &&
					_detailProfileType != eProfileType.None)
				{
					compXRef = (ProfileXRef)aSourceCellRef.Cube.CubeGroup.GetProfileXRef(new ProfileXRef(_totalProfileType, _detailProfileType));

					if (compXRef != null)
					{
						detailList = (ArrayList)compXRef.GetDetailList(aSourceCellRef[_totalProfileType]);

						if (detailList != null)
						{
							foreach (int profileKey in detailList)
							{
								if (profileList == null || profileList.Contains(profileKey))
								{
									aCellRef[_detailProfileType] = profileKey;

									if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
									{
										aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
									}
									else
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
				}
				else if (profileList != null && _detailProfileType != eProfileType.None)
				{
					foreach (Profile profile in profileList)
					{
						aCellRef[_detailProfileType] = profile.Key;

						if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
						{
							aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
						}
						else
						{
							aCellSelector.CheckItem(aCellRef, out aCancel);
						}

						if (aCancel)
						{
							return;
						}
					}
				}
				else
				{
					if (aCurrRelationshipItem < aRelationshipItems.Length - 1)
					{
						aRelationshipItems[aCurrRelationshipItem + 1].RecurseDetailItems(aSourceCellRef, aCellRef, aRelationshipItems, aCurrRelationshipItem + 1, aCellSelector, out aCancel);
					}
					else
					{
						aCellSelector.CheckItem(aCellRef, out aCancel);
					}

					if (aCancel)
					{
						return;
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
