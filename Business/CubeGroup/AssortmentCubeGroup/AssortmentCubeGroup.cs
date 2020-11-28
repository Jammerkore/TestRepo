using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using System.Diagnostics;

namespace MIDRetail.Business
{
	public class AssortmentCubeGroup : ComputationCubeGroup
	{
		//=======
		// FIELDS
		//=======

		private eAssortmentWindowType _windowType;
		private int _numSummaryLevels;
		private AssortmentDetailData _assrtDtlData;
		private Hashtable _headerHash;
		private Hashtable _packMultipleHash;
		private Hashtable _colorMultipleHash;
		private int _headerIndex;
		private AllocationProfile _defaultAlocProfile;
		private bool _defaultAlocProfileInited;
		private AssortmentComponentVariables _componentVariables;
		private AllocationHeaderProfileList _headerList;
		private AllocationHeaderProfileList _assortmentList;
		private AllocationHeaderProfileList _placeholderList;
		private AllocationHeaderProfileList _receiptList;
		private DataTable _dtComponents;
		private PackColorProfileXRef _packColorXRef;
		private Hashtable _fixedHash;
		private DataTable _dtPlaceholderComponents;
		private DataTable _dtHeaderComponents;
		private AssortmentComputations _assrtComputations;
		private Hashtable _blockedList;
		private Queue _blockRetotalQueue;
		private Hashtable _blockRetotalHash;
		private bool _colorDefined;		// TT#1225 - stodd
		private bool _styleSkuDefined;	// TT#2402 - stodd
		// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
		private eAssortmentType _assortmentType;
		// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct

        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
        private HierarchyProfile _mainHp;
        // End TT#1489
	    private ArrayList _selectableTotalColumnHeaders; 	// TT#3848 - stodd - Locked cell not able to be changed after unlocking
	    private ArrayList _selectableDetailColumnHeaders;	// TT#3848 - stodd - Locked cell not able to be changed after unlocking

        private Dictionary<HashSet<int>, ProfileList> _storesBySetAndGrade = null;	// TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentCubeGroup using the given SessionAddressBlock and Transaction.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to the SessionAddressBlock for this user session.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to the Transaction for this functional transaction.
		/// </param>

		public AssortmentCubeGroup(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, eAssortmentWindowType aWindowType)
			: base(aSAB, aTransaction)
        {
			try
			{
				_windowType = aWindowType;
				_numSummaryLevels = -1;
				_assrtDtlData = new AssortmentDetailData();
				_headerHash = new Hashtable();
				_packMultipleHash = new Hashtable();
				_colorMultipleHash = new Hashtable();
                _storesBySetAndGrade = null;	// TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again
                // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                _mainHp = SAB.HierarchyServerSession.GetMainHierarchyData();
                // End TT#1489  
				// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
				_assortmentType = eAssortmentType.Undefined;
				// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
                // Begin TT#3842 - stodd - Total % not recalculating when total units change
                if (aWindowType == eAssortmentWindowType.GroupAllocation)
                {
                    _assortmentType = eAssortmentType.GroupAllocation;
                }
                // End TT#3842 - stodd - Total % not recalculating when total units change
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

		public bool isHeaderDefined
		{
			get
			{
				return _headerIndex != -1;
			}
		}

		// Begin TT#1225 - stodd
		public bool isColorDefined
		{
			get
			{
				return _colorDefined;
			}
		}
		// End TT#1225 - stodd

		// Begin TT#2402 - stodd
		public bool StyleSkuDefined
		{
			get
			{
				return _styleSkuDefined;
			}
		}
		// End TT#2402 - stodd

		public ApplicationSessionTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}
		// END TT#2 - stodd - assortment

		// BEGIN TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct
		public eAssortmentType AssortmentType
		{
			get
			{
				return _assortmentType;
			}
			set
			{
				_assortmentType = value;
			}
		}
		// END TT#500-MD - stodd -  Assortment Matrix - Total % values are not correct

		//internal AllocationProfile DefaultAllocationProfile
		public AllocationProfile DefaultAllocationProfile
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			get
			{
				AllocationProfile alocProf;

				if (!_defaultAlocProfileInited)
				{
					_defaultAlocProfileInited = true;

					// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
					AllocationHeaderProfileList ahp = GetHeaderList();	

					foreach (AllocationHeaderProfile hdrProf in ahp)
					{
						// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
						//alocProf = _transaction.GetAllocationProfile(hdrProf.Key);
						alocProf = _transaction.GetAssortmentMemberProfile(hdrProf.Key);
						// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
					// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                        if ((_windowType == eAssortmentWindowType.Assortment || _windowType == eAssortmentWindowType.GroupAllocation) && alocProf.HeaderType == eHeaderType.Assortment) // TT#952 - MD - Add Matrix to Group Allocation - 
						{
							_defaultAlocProfile = alocProf;
							break;
						}
                        // RMatelic - comment out next if
                        //else if (_windowType == eAssortmentWindowType.Assortment && alocProf.AsrtRID != Include.NoRID)
                        //{
                        //    _defaultAlocProfile = alocProf;
                        //    break;
                        //}
						else if (_windowType == eAssortmentWindowType.AllocationSummary && alocProf.HeaderType != eHeaderType.Assortment)
						{
							_defaultAlocProfile = alocProf;
							break;
						}
					}
				}

				return _defaultAlocProfile;
			}
		}

		/// <summary>
		/// Makes a copy of the AssortmentComponentVariables and adds the Characteristic variables to it.
		/// </summary>

		public AssortmentComponentVariables AssortmentComponentVariables
		{
			get
			{
				ProfileList charProfList;

				try
				{
					if (_componentVariables == null)
					{
						charProfList = new ProfileList(eProfileType.ProductCharacteristic);

						foreach (AllocationHeaderProfile plcProf in GetPlaceholderList())
						{
							foreach (NodeCharProfile charProf in SAB.HierarchyServerSession.GetProductCharacteristics(plcProf.StyleHnRID))
							{
								if (!charProfList.Contains(charProf.Key))
								{
									charProfList.Add(charProf);
								}
							}
						}

						_componentVariables = (AssortmentComponentVariables)AssortmentComputations.AssortmentComponentVariables.Copy();

                        // Begin TT#1489 - RMatelic - Add Hierarchy levels above style as default displayable items
                        for (int level = 1; level <= _mainHp.HierarchyLevels.Count; level++)
                        {
                            HierarchyLevelProfile hlp = (HierarchyLevelProfile)_mainHp.HierarchyLevels[level];
							if (hlp.LevelType == eHierarchyLevelType.Color)
							{
								break;
							}
							else
                            {   
                                // To prevent duplicate profile types being added in the following statement, the keys are manipulated 
                                // ( by adding  - 5 - hlp.Key) to prevent the duplicates, similar to what the charactertics do. 
                                // This results in non-appplicable profile types being added to a profile list and hashtable. This doesn't
                                // appear to affect the program flow, but may look odd if looking at the profile list or hashtable as to
                                // what is contained in them. An alternative solution may need to be developed in the future.
                                _componentVariables.AddVariableProfile(
                                new AssortmentComponentVariableProfile(
                                    (int)eAssortmentComponentVariables.HierarchyLevel - 5 - hlp.Key,
                                    // Begin TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.
                                    //(eProfileType)((int)eProfileType.HierarchyLevel - 5 - hlp.Key),
                                    (eProfileType)((int)eProfileType.HierarchyLevel + hlp.Key),
                                    // End TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.
                                    hlp.LevelID,
                                    "HIERARCHYLEVEL" + hlp.Level,
                                    false,
                                    false));
                            }
                        }
                        // End TT#1489

						foreach (NodeCharProfile charProf in charProfList)
						{
							// BEGIN TT#101 - stodd -  no values after processing "create Placeholders"
                            //==============================================================================================================================
                            // Why add 8? The eProfileTypes for characteristics are based upon the positive offset from eProfileType.ProductCharacteristic 
                            // in enums.cs. This number happens to be 31. The first product characteristic assigned to a node gets a profileType of 32, 
                            // the next gets 33, and so on. The resulting profile types assigned to each characteristic is unimportant. It’s only important 
                            // that each have their own unique assignment. The problem comes when the number assigned actually equals an eProfileType that 
                            // is already being used in the cubes. In the case of TT#101, number 39 resolves to an eProfileType of StoreGroupLevel. StoreGroupLevel 
                            // is already a profile type being used by the cubes. Later this causes the details for the totals to not return any values. 
                            // Adding 8, causes the resulting eProfileType to be outside the range used by the cubes.
                            //==============================================================================================================================
							_componentVariables.AddVariableProfile(
								new AssortmentComponentVariableProfile(
									(int)eAssortmentComponentVariables.Characteristic + charProf.Key,
									(eProfileType)((int)eProfileType.ProductCharacteristic + 8 + charProf.Key),
									charProf.ProductCharID,
									"CHARACTERISTIC" + charProf.Key,
									false,
									false));
							// END TT#101 - stodd -  no values after processing "create Placeholders"
						}
					}

					return _componentVariables;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public int NumberOfSummaryLevels
		{
			get
			{
				return _numSummaryLevels;
			}
		}

		public AssortmentDetailData AssortmentDetailData
		{
			get
			{
				return _assrtDtlData;
			}
		}

		public AssortmentComputations AssortmentComputations
		{
			get
			{
				if (_assrtComputations == null)
				{
                    if (_windowType == eAssortmentWindowType.Assortment || _windowType == eAssortmentWindowType.GroupAllocation)  // TT#952 - MD - Add Matrix to Group Allocation - 
					{
						_assrtComputations = new AssortmentViewComputations();
					}
					else
					{
						_assrtComputations = new AllocationSummaryComputations();
					}
				}

				return _assrtComputations;
			}
		}

		public Hashtable BlockedList
		{
			get
			{
				if (_blockedList == null)
				{
					_blockedList = new Hashtable();
				}

				return _blockedList;
			}

			// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
            set
            {
                _blockedList = value;
            }
			// End TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
		}

		public Queue BlockRetotalQueue
		{
			get
			{
				if (_blockRetotalQueue == null)
				{
					_blockRetotalQueue = new Queue();
				}

				return _blockRetotalQueue;
			}
		}

		public Hashtable BlockRetotalHash
		{
			get
			{
				if (_blockRetotalHash == null)
				{
					_blockRetotalHash = new Hashtable();
				}

				return _blockRetotalHash;
			}
		}

		public eAssortmentVariableType AssortmentVariableType
		{
			get
			{
                if ((_windowType == eAssortmentWindowType.Assortment || _windowType == eAssortmentWindowType.GroupAllocation) && DefaultAllocationProfile != null)  // TT#952 - MD - Add Matrix to Group Allocation - 
				{
					return ((AssortmentProfile)DefaultAllocationProfile).AssortmentVariableType;
				}
				else
				{
					return eAssortmentVariableType.None;
				}
			}
		}

		public int AssortmentStoreGroupRID
		{
			get
			{
                if ((_windowType == eAssortmentWindowType.Assortment || _windowType == eAssortmentWindowType.GroupAllocation) && DefaultAllocationProfile != null)	// TT#952 - MD - Add Matrix to Group Allocation - 
				{
                    return ((AssortmentProfile)DefaultAllocationProfile).AssortmentStoreGroupRID;
				}
				else
				{
					return Include.NoRID;
				}
			}
		}

		public eAssortmentWindowType WindowType
		{
			get
			{
				return _windowType;
			}
		}

		// Begin TT#3848 - stodd - Locked cell not able to be changed after unlocking
        public ArrayList SelectableTotalColumnHeaders
        {
            get
            {
                return _selectableTotalColumnHeaders;
            }
            set
            {
                _selectableTotalColumnHeaders = value;
            }
        }

        public ArrayList SelectableDetailColumnHeaders
        {
            get
            {
                return _selectableDetailColumnHeaders;
            }
            set
            {
                _selectableDetailColumnHeaders = value;
            }
        }
		// End TT#3848 - stodd - Locked cell not able to be changed after unlocking

		//========
		// METHODS
		//========

		override public ProfileList GetProfileList(eProfileType aProfileType)
		{
			try
			{
				switch (aProfileType)
				{
					case eProfileType.AssortmentSummaryVariable:
						return AssortmentComputations.AssortmentSummaryVariables.VariableProfileList;

					case eProfileType.AssortmentTotalVariable:
						return AssortmentComputations.AssortmentTotalVariables.VariableProfileList;

					case eProfileType.AssortmentDetailVariable:
						return AssortmentComputations.AssortmentDetailVariables.VariableProfileList;

					case eProfileType.AssortmentQuantityVariable:
						return AssortmentComputations.AssortmentQuantityVariables.VariableProfileList;

					default:
						return base.GetProfileList(aProfileType);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a ComputationCubeGroupWaferInfo object from a CubeWaferCoordinateList object.
		/// </summary>
		/// <param name="aCoorList">
		/// The list of wafer coordinates to create a ComputationCubeGroupWaferInfo with.
		/// </param>
		/// <returns></returns>

		override protected ComputationCubeGroupWaferInfo CreateWaferInfo(CubeWaferCoordinateList aCoorList)
		{
			AssortmentCubeGroupWaferInfo waferInfo;

			try
			{
				waferInfo = new AssortmentCubeGroupWaferInfo();
				waferInfo.ProcessWaferCoordinates(aCoorList);
				return waferInfo;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Private method that determines the eCubeType that is specified by the given ComputationCubeGroupWaferInfo objects.
		/// </summary>
		/// <param name="aGlobalWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the global cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aRowWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the row cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aColWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the col cube flags that are used to determine the eCubeType.
		/// </param>
		/// <returns>
		/// The eCubeType of the cube that is described by the given ComputationCubeGroupWaferInfo objects.
		/// </returns>

		override protected eCubeType DetermineCubeType(ComputationCubeGroupWaferInfo aGlobalWaferInfo, ComputationCubeGroupWaferInfo aRowWaferInfo, ComputationCubeGroupWaferInfo aColWaferInfo)
		{
			int subTotLvl;

			try
			{
				AssortmentCubeGroupWaferCubeFlags cumulatedFlags;

				cumulatedFlags = new AssortmentCubeGroupWaferCubeFlags();
				cumulatedFlags.CubeFlags = (ushort)(aGlobalWaferInfo.CubeFlagValues | aRowWaferInfo.CubeFlagValues | aColWaferInfo.CubeFlagValues);

				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				if (cumulatedFlags.isPlanning)
				{
					if (cumulatedFlags.isGrade)
					{
						return eCubeType.StoreBasisGradeTotalDateTotal;
					}
					else if (cumulatedFlags.isAttributeSet)
					{
						return eCubeType.StoreBasisGroupTotalDateTotal;
					}
					else if (cumulatedFlags.isTotal)
					{
						return eCubeType.StoreBasisStoreTotalDateTotal;
					}
					else
					{
						return eCubeType.None;
					}
				}
				else
				{
				//End TT#2 - JScott - Assortment Planning - Phase 2
					if (cumulatedFlags.isSummary)
					{
						if (cumulatedFlags.isTotal)
						{
							return eCubeType.AssortmentSummaryTotal;
						}
						else
						{
							if (cumulatedFlags.isAttributeSet)
							{
								return eCubeType.AssortmentSummaryGroupLevel;
							}
							else
							{
								return eCubeType.AssortmentSummaryGrade;
							}
						}
					}
					else
					{
						if (cumulatedFlags.isTotal)
						{
							if (cumulatedFlags.isSubTotal)
							{
								subTotLvl = ((AssortmentCubeGroupWaferInfo)aGlobalWaferInfo).SubTotalLevel;

								if (subTotLvl == Include.NoRID)
								{
									subTotLvl = ((AssortmentCubeGroupWaferInfo)aRowWaferInfo).SubTotalLevel;
								}

								if (subTotLvl == Include.NoRID)
								{
									subTotLvl = ((AssortmentCubeGroupWaferInfo)aColWaferInfo).SubTotalLevel;
								}

								if (cumulatedFlags.isPlaceholder)
								{
									return new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, subTotLvl);
								}
								else
								{
									return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, subTotLvl);
								}
							}
							else
							{
								if (cumulatedFlags.isPlaceholder)
								{
									return eCubeType.AssortmentComponentPlaceholderTotal;
								}
								else
								{
									return eCubeType.AssortmentComponentHeaderTotal;
								}
							}
						}
						else
						{
							if (cumulatedFlags.isSubTotal)
							{
								subTotLvl = ((AssortmentCubeGroupWaferInfo)aGlobalWaferInfo).SubTotalLevel;

								if (subTotLvl == Include.NoRID)
								{
									subTotLvl = ((AssortmentCubeGroupWaferInfo)aRowWaferInfo).SubTotalLevel;
								}

								if (subTotLvl == Include.NoRID)
								{
									subTotLvl = ((AssortmentCubeGroupWaferInfo)aColWaferInfo).SubTotalLevel;
								}

								if (cumulatedFlags.isPlaceholder)
								{
									if (cumulatedFlags.isAttributeSet)
									{
										return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, subTotLvl);
									}
									else
									{
										return new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, subTotLvl);
									}
								}
								else
								{
									if (cumulatedFlags.isAttributeSet)
									{
										return new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, subTotLvl);
									}
									else
									{
										return new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, subTotLvl);
									}
								}
							}
							else
							{
								if (cumulatedFlags.isPlaceholder)
								{
									if (cumulatedFlags.isAttributeSet)
									{
										return eCubeType.AssortmentComponentPlaceholderGroupLevel;
									}
									else
									{
										return eCubeType.AssortmentComponentPlaceholderGrade;
									}
								}
								else
								{
									if (cumulatedFlags.isAttributeSet)
									{
										return eCubeType.AssortmentComponentHeaderGroupLevel;
									}
									else
									{
										return eCubeType.AssortmentComponentHeaderGrade;
									}
								}
							}
						}
					}
				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				}
				//End TT#2 - JScott - Assortment Planning - Phase 2
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method converts a set of common, row, and column CubeWaferCoordinateList objects for a given eCubeType into the corresponding
		/// ComputationCellReference.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType that identifies the ComputationCube this Cell exists in.
		/// </param>
		/// <param name="aCommonWaferList">
		/// The CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// The CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// The CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <returns>
		/// The ComputationCellReference identifying the Cell.
		/// </returns>

		override protected ComputationCellReference ConvertCubeWaferInfoToCellReference(
			eCubeType aCubeType,
			CubeWaferCoordinateList aCommonWaferList,
			CubeWaferCoordinateList aRowWaferList,
			CubeWaferCoordinateList aColWaferList,
			ComputationCubeGroupWaferInfo aGlobalWaferInfo,
			ComputationCubeGroupWaferInfo aRowWaferInfo,
			ComputationCubeGroupWaferInfo aColWaferInfo)
		{
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			//AssortmentCellReference assrtCellRef = null;
			ComputationCellReference assrtCellRef = null;
			Cube assrtCube;
			//End TT#2 - JScott - Assortment Planning - Phase 2

			try
			{
				if (aCubeType != eCubeType.None)
				{
					//Begin TT#2 - JScott - Assortment Planning - Phase 2
					//assrtCellRef = (AssortmentCellReference)((AssortmentCube)GetCube(aCubeType)).CreateCellReference();
					assrtCube = GetCube(aCubeType);

					if (assrtCube == null)
					{
						assrtCube = ((AssortmentProfile)DefaultAllocationProfile).BasisReader.AssortmentPlanCubeGroup.GetCube(aCubeType);
					}

					if (assrtCube != null)
					{
						assrtCellRef = (ComputationCellReference)assrtCube.CreateCellReference();
					//End TT#2 - JScott - Assortment Planning - Phase 2

						foreach (CubeWaferCoordinate waferCoordinate in aCommonWaferList)
						{
							intLoadWaferCoordinates(waferCoordinate, assrtCellRef);
						}

						foreach (CubeWaferCoordinate waferCoordinate in aRowWaferList)
						{
							intLoadWaferCoordinates(waferCoordinate, assrtCellRef);
						}

						foreach (CubeWaferCoordinate waferCoordinate in aColWaferList)
						{
							intLoadWaferCoordinates(waferCoordinate, assrtCellRef);
						}
					//Begin TT#2 - JScott - Assortment Planning - Phase 2
					}
					//End TT#2 - JScott - Assortment Planning - Phase 2
				}

				return assrtCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that creates a new CustomStoreFilter object for the given Filter ID.
		/// </summary>
		/// <param name="aFilterID">
		/// The ID of the CustomStoreFilter to create.
		/// </param>
		/// <returns>
		/// A new CustomStoreFilter object.
		/// </returns>

        //override protected CustomStoreFilter CreateCustomStoreFilter(int aFilterID)
        //{
        //    return new CustomStoreFilter(_SAB, _transaction, _SAB.ApplicationServerSession, null, aFilterID);
        //}

        protected override filter CreateCustomStoreFilter(int filterRID)
        {
            filterDataHelper.SetVariableKeysFromTransaction(_transaction);

            filter f = filterDataHelper.LoadExistingFilter(filterRID);
            f.SetExtraInfoForCubes(_SAB, _transaction, null);
            return f;
        }

		// BEGIN TT#2 - stodd

		public void ClearPlaceholderList()
		{
			_placeholderList = null;
		}

		public void ClearTotalCubes(bool reloadFromDB)
		{
			base.ClearStoreTotalCubes();
			ClearGroupTotalCubes(reloadFromDB);
		}


		override public void ClearGroupTotalCubes()
		{
			ClearGroupTotalCubes(false);
		}
		// END TT#2 - stodd


		public void ClearGroupTotalCubes(bool reload)
		{
			DataTable dtSummary;

			try
			{
				base.ClearGroupTotalCubes();

                if (DefaultAllocationProfile != null && ((AssortmentProfile)DefaultAllocationProfile).AssortmentSummaryProfile != null)
				{
					//DefaultAllocationProfile.
					dtSummary = DefaultAllocationProfile.GetSummaryInformation(this, CurrentStoreGroupProfile.Key, reload);

					((AssortmentSummaryGrade)GetCube(eCubeType.AssortmentSummaryGrade)).LoadCube(dtSummary, reload);
					((AssortmentSummaryGroupLevel)GetCube(eCubeType.AssortmentSummaryGroupLevel)).LoadCube(dtSummary, reload);
					((AssortmentSummaryTotal)GetCube(eCubeType.AssortmentSummaryTotal)).LoadCube(dtSummary, reload);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int GetPackMultiple(int aHeaderRID, int aPackRID)
		{
			object hashEntry;
			int multiple;
			AllocationProfile alocProf;
			PackHdr packHdr;

			try
			{
				hashEntry = _packMultipleHash[Include.CreateHashKey(aHeaderRID, aPackRID)];

				if (hashEntry == null)
				{
					multiple = 1;
					// BEGIN TT#771-MD - Stodd - null reference exception
					//alocProf = Transaction.GetAllocationProfile(aHeaderRID);
					alocProf = Transaction.GetAssortmentMemberProfile(aHeaderRID);
					// END TT#771-MD - Stodd - null reference exception

					if (aPackRID == int.MaxValue)
					{
						multiple = alocProf.AllocationMultiple;
					}
					else
					{
						packHdr = alocProf.GetPackHdr(aPackRID);

						if (packHdr != null)
						{
							multiple = packHdr.PackMultiple;
						}
						else
						{
							multiple = 0;
						}
					}

					_packMultipleHash.Add(Include.CreateHashKey(aHeaderRID, aPackRID), multiple);
				}
				else
				{
					multiple = (int)hashEntry;
				}

				return multiple;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int GetColorMultiple(int aHeaderRID, int aPackRID, int aColorRID)
		{
			object hashEntry;
			int multiple;
			AllocationProfile alocProf;
			PackHdr packHdr;

			try
			{
				hashEntry = _packMultipleHash[Include.CreateHashKey(aHeaderRID, aPackRID, aColorRID)];

				if (hashEntry == null)
				{
					multiple = 1;
					// BEGIN TT#771-MD - Stodd - null reference exception
					//alocProf = Transaction.GetAllocationProfile(aHeaderRID);
					alocProf = Transaction.GetAssortmentMemberProfile(aHeaderRID);
					// END TT#771-MD - Stodd - null reference exception

					if (aPackRID == int.MaxValue)
					{
						if (aColorRID == int.MaxValue)
						{
							multiple = alocProf.AllocationMultiple;
						}
						else
						{
							multiple = 0;
						}
					}
					else
					{
						packHdr = alocProf.GetPackHdr(aPackRID);

						if (packHdr != null)
						{
							if (aColorRID == int.MaxValue)
							{
								multiple = packHdr.PackMultiple;
							}
							else if (packHdr.ColorIsInPack(aColorRID))
							{
								multiple = packHdr.GetColorBin(aColorRID).ColorUnitsInPack;
							}
							else
							{
								multiple = 0;
							}
						}
						else
						{
							multiple = 0;
						}
					}

					_packMultipleHash.Add(Include.CreateHashKey(aHeaderRID, aPackRID, aColorRID), multiple);
				}
				else
				{
					multiple = (int)hashEntry;
				}

				return multiple;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the list of headers associated with the Assortments.
		/// </summary>
		/// <returns>
		/// The list of headers associated with the Assortments.
		/// </returns>

		public AllocationHeaderProfileList GetHeaderList()
		{
			try
			{
				if (_headerList == null)
				{
					intGetHeaders();
				}

				return _headerList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the list of assortment associated with the Assortments.
		/// </summary>
		/// <returns>
		/// The list of assortment associated with the Assortments.
		/// </returns>

		public AllocationHeaderProfileList GetAssortmentList()
		{
			try
			{
				if (_assortmentList == null)
				{
					intGetHeaders();
				}

				return _assortmentList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the list of placeholders associated with the Assortments.
		/// </summary>
		/// <returns>
		/// The list of placeholders associated with the Assortments.
		/// </returns>

		public AllocationHeaderProfileList GetPlaceholderList()
		{
			try
			{
				if (_placeholderList == null)
				{
					intGetHeaders();
				}

				return _placeholderList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the list of receipts associated with the Assortments.
		/// </summary>
		/// <returns>
		/// The list of receipts associated with the Assortments.
		/// </returns>

		public AllocationHeaderProfileList GetReceiptList()
		{
			try
			{
				if (_receiptList == null)
				{
					intGetHeaders();
				}

				return _receiptList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the components that make up a assortment.
		/// </summary>
		/// <returns>
		/// A DataTable containing the Components of the Assortment.
		/// </returns>

		public DataTable GetAssortmentComponents()
		{
			AllocationProfile alocProf;

			try
			{
				if (_dtComponents == null)
				{
					if (_windowType == eAssortmentWindowType.Assortment || _windowType == eAssortmentWindowType.GroupAllocation) // TT#952 - MD - Add Matrix to Group Allocation - 
					{
						_dtComponents = AssortmentProfile.CreateAssortmentComponentTable(AssortmentComponentVariables);
						_packColorXRef = new PackColorProfileXRef();

						foreach (AllocationHeaderProfile hdrProf in GetHeaderList())
						{
							alocProf = _transaction.GetAssortmentMemberProfile(hdrProf.Key);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

							if (alocProf.HeaderType == eHeaderType.Assortment)
							{
								((AssortmentProfile)alocProf).GetAssortmentComponents(this, _dtComponents, _packColorXRef);
							}
						}
					}
					else
					{
						_dtComponents = AllocationProfile.CreateAllocationComponentTable(AssortmentComponentVariables);

						foreach (AllocationHeaderProfile hdrProf in GetHeaderList())
						{
							// BEGIN TT#771-MD - Stodd - null reference exception
							//alocProf = _transaction.GetAllocationProfile(hdrProf.Key);
							alocProf = _transaction.GetAssortmentMemberProfile(hdrProf.Key);
							// END TT#771-MD - Stodd - null reference exception

							if (alocProf.HeaderType != eHeaderType.Assortment)
							{
								((AssortmentProfile)alocProf).GetAllocationComponents(this, _dtComponents);
							}
						}
					}

				}

				return _dtComponents;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public bool isPackColorReadOnly(int aPlaceholderRID, int aHeaderRID, int aPackRID, int aColorRID)
		{
			try
			{
				if (_fixedHash == null)
				{
					_fixedHash = new Hashtable();

					foreach (DataRow row in GetAssortmentComponents().Rows)
					{
						if (Convert.ToBoolean(row["READONLY"]))
						{
							_fixedHash[Include.CreateHashKey(
								Convert.ToInt32(row[((AssortmentViewComponentVariables)AssortmentComponentVariables).Placeholder.RIDColumnName]),
								Convert.ToInt32(row[((AssortmentViewComponentVariables)AssortmentComponentVariables).HeaderID.RIDColumnName]),
								Convert.ToInt32(row[((AssortmentViewComponentVariables)AssortmentComponentVariables).Pack.RIDColumnName]),
								Convert.ToInt32(row[((AssortmentViewComponentVariables)AssortmentComponentVariables).Color.RIDColumnName]))] = null;
						}
					}
				}

				return _fixedHash.Contains(Include.CreateHashKey(aPlaceholderRID, aHeaderRID, aPackRID, aColorRID));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the components that make up a placeholder.
		/// </summary>
		/// <returns>
		/// A DataTable containing the Components of the Placeholder.
		/// </returns>

		public DataTable GetPlaceholderComponents()
		{
			try
			{
				if (_dtPlaceholderComponents == null)
				{
					_dtPlaceholderComponents = GetAssortmentComponents().Copy();

					foreach (DataRow row in _dtPlaceholderComponents.Rows)
					{
						if (Convert.ToInt32(row[((AssortmentViewComponentVariables)AssortmentComponentVariables).HeaderID.RIDColumnName]) != int.MaxValue)
						{
							row.Delete();
						}
					}

					_dtPlaceholderComponents.AcceptChanges();
				}

				return _dtPlaceholderComponents;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the components that make up a header.
		/// </summary>
		/// <returns>
		/// A DataTable containing the Components of the Header.
		/// </returns>

		public DataTable GetHeaderComponents()
		{
			try
			{
				if (_dtHeaderComponents == null)
				{
					_dtHeaderComponents = GetAssortmentComponents().Copy();

					foreach (DataRow row in _dtHeaderComponents.Rows)
					{
						if (Convert.ToInt32(row[((AssortmentViewComponentVariables)AssortmentComponentVariables).HeaderID.RIDColumnName]) == int.MaxValue)
						{
							row.Delete();
						}
					}

					_dtHeaderComponents.AcceptChanges();
				}

				return _dtHeaderComponents;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the components that make up a header.
		/// </summary>
		/// <param name="aHeaderRID">
		/// RID of the header.
		/// </param>
		/// <returns>
		/// A DataTable containing the Components of the Header.
		/// </returns>

		public ProfileList GetStoreGrades(int aHeaderRID)
		{
			try
			{
				return ((AssortmentProfile)_transaction.GetAssortmentMemberProfile(aHeaderRID)).GetAssortmentStoreGrades();	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member	// TT#488-MD - STodd - Group Allocation
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#2 - stodd - assortment
		public ProfileList GetStoresInSetGrade(int setRid, int storeGrade)
		{
			ProfileList storeList = ((AssortmentProfile)DefaultAllocationProfile).GetStoresInSetGrade(setRid, storeGrade);
			return storeList;
		}
		// END TT#2 - stodd - assortment

		// Begin TT#1205-MD - stodd - Lock graphic on Matrix Grid disappears after running an action- 
        public ProfileList GetStoresInSet(int setRid)
        {
            ProfileList storeList = ((AssortmentProfile)DefaultAllocationProfile).GetStoresInSet(setRid);
            return storeList;
        }

        public ProfileList GetAllStores()
        {
            ProfileList storeList = ((AssortmentProfile)DefaultAllocationProfile).AssortmentSummaryProfile.StoreList;
            return storeList;
        }
		// End TT#1205-MD - stodd - Lock graphic on Matrix Grid disappears after running an action- 

		// Begin TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
        public StoreProfile GetStore(int storeRid)
        {
            ProfileList storeList = ((AssortmentProfile)DefaultAllocationProfile).AssortmentSummaryProfile.StoreList;
            return (StoreProfile)storeList.FindKey(storeRid);
        }
		// End TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

		/// <summary>
		/// Opens a AssortmentCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
		/// </summary>
        /// <param name="aAssrtOpenParms">
		/// The AssortmentOpenParms object that contains information about the assortment.
		/// </param>

		public void OpenCubeGroup(AssortmentOpenParms aAssrtOpenParms)
		{
			AssortmentOpenParms assrtOpenParms;
			//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//ComponentProfileXRef hdrColorDtlCompXRef;
			//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
            //ComponentProfileXRef hdrPackDtlCompXRef;
            //End TT#2 - JScott - Assortment Planning - Phase 2
			//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//ComponentProfileXRef hdrTotCompXRef;
			//ComponentProfileXRef plcColorDtlCompXRef;
			//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
            //ComponentProfileXRef plcPackDtlCompXRef;
            //End TT#2 - JScott - Assortment Planning - Phase 2
			//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//ComponentProfileXRef plcTotCompXRef;
			//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			ProfileXRef profXRef;
			ArrayList profTypeList;
			//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//ArrayList varRIDList;
			//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//ArrayList planEnqueueList;
			//StoreVariableFilter varFilter;
			//EligibilityFilter eligFilter;
			//ProfileList planWeekList;
			//ProfileList planPeriodList;
			//ProfileList storeProfileList;
			//ProfileList versionProfileList;
			//ProfileList hierarchyNodeProfileList;
			//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			//DataTable dtPlaceholderComponents;
			//DataTable dtHeaderComponents;
			//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
			CubeDefinition cubeDef;
			AssortmentCube assrtCube;
			//int weekCubeSize;
			//Begin TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
			eProfileType[] plcGrdTotTypeArr;
			ComponentProfileXRef plcGrdTotCompXRef;
			//End TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube


			try
			{
				//==================
				// Initialize fields
				//==================

				assrtOpenParms = aAssrtOpenParms;

				//========================================
				// Set Master Profile Lists and XRef Lists
				//========================================

				// BEGIN TT#2504 - stodd - Assortment is blank after header is assigned
				AssortmentProfile asp = (AssortmentProfile)DefaultAllocationProfile;
				ProfileList gradeList = asp.GetAssortmentStoreGrades();	// TT#488-MD - STodd - Group Allocation
				SetMasterProfileList(gradeList);

				// This does the same thing as the long, convoluted statement below
				//AllocationHeaderProfileList ahpl = (AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader);
				//AllocationHeaderProfile ahp = (AllocationHeaderProfile)ahpl[0];
				//AllocationProfile ap = _transaction.GetAllocationProfile(ahp.Key);
				//ProfileList gradeList = ap.GetStoreGrades();
				// This is the long convolutated statement...
				//SetMasterProfileList(_transaction.GetAllocationProfile(((AllocationHeaderProfile)((AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader))[0]).Key).GetStoreGrades());
				// END TT#2504 - stodd - Assortment is blank after header is assigned

				//===========================
				// Build ComponentProfileXRef
				//===========================

				//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//dtPlaceholderComponents = GetPlaceholderComponents();
				//dtHeaderComponents = GetHeaderComponents();

				//profTypeList = new ArrayList();
				//varRIDList = new ArrayList();

				//foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
				//{
				//    profTypeList.Add(varProf.ProfileListType);
				//}

				//plcColorDtlCompXRef = new ComponentProfileXRef(eCubeType.AssortmentPlaceholderColorDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));
				//plcTotCompXRef = new ComponentProfileXRef(eCubeType.AssortmentPlaceholderTotalDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));

				//foreach (DataRow row in dtPlaceholderComponents.Rows)
				//{
				//    varRIDList.Clear();

				//    foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
				//    {
				//        varRIDList.Add(Convert.ToInt32(row[varProf.RIDColumnName]));
				//    }

				//    plcColorDtlCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
				//    plcTotCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
				//}

				//SetProfileXRef(plcColorDtlCompXRef);
				//SetProfileXRef(plcTotCompXRef);

				//hdrColorDtlCompXRef = new ComponentProfileXRef(eCubeType.AssortmentHeaderColorDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));
				//hdrTotCompXRef = new ComponentProfileXRef(eCubeType.AssortmentHeaderTotalDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));

				//foreach (DataRow row in dtHeaderComponents.Rows)
				//{
				//    varRIDList.Clear();

				//    foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
				//    {
				//        varRIDList.Add(Convert.ToInt32(row[varProf.RIDColumnName]));
				//    }

				//    hdrColorDtlCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
				//    hdrTotCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
				//}

				//SetProfileXRef(hdrColorDtlCompXRef);
				//SetProfileXRef(hdrTotCompXRef);

				//profTypeList.Clear();
				BuildComponentProfileXRef();
				//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.

				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				//foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
				//{
				//    if (varProf.Key != ((AssortmentViewComponentVariables)AssortmentComponentVariables).Color.Key)
				//    {
				//        profTypeList.Add(varProf.ProfileListType);
				//    }
				//}

				//plcPackDtlCompXRef = new ComponentProfileXRef(eCubeType.AssortmentPlaceholderPackDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));

                //foreach (DataRow row in dtPlaceholderComponents.Rows)
                //{
                //    varRIDList.Clear();

                //    foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
                //    {
                //        if (varProf.Key != ((AssortmentViewComponentVariables)AssortmentComponentVariables).Color.Key)
                //        {
                //            varRIDList.Add(Convert.ToInt32(row[varProf.RIDColumnName]));
                //        }
                //    }

                //    plcPackDtlCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
                //}

                //SetProfileXRef(plcPackDtlCompXRef);

                //hdrPackDtlCompXRef = new ComponentProfileXRef(eCubeType.AssortmentHeaderPackDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));

                //foreach (DataRow row in dtHeaderComponents.Rows)
                //{
                //    varRIDList.Clear();

                //    foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
                //    {
                //        if (varProf.Key != ((AssortmentViewComponentVariables)AssortmentComponentVariables).Color.Key)
                //        {
                //            varRIDList.Add(Convert.ToInt32(row[varProf.RIDColumnName]));
                //        }
                //    }

                //    hdrPackDtlCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
                //}

                //SetProfileXRef(hdrPackDtlCompXRef);

                //End TT#2 - JScott - Assortment Planning - Phase 2
				//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				////=====================================
				//// Build Placeholder/Header ProfileXRef
				////=====================================

				//profXRef = new ProfileXRef(eProfileType.PlaceholderHeader, eProfileType.AllocationHeader);

				//foreach (AllocationHeaderProfile hdrProf in GetHeaderList())
				//{
				//    if (hdrProf.PlaceHolderRID != Include.NoRID)
				//    {
				//        profXRef.AddXRefIdEntry(hdrProf.PlaceHolderRID, hdrProf.Key);
				//    }
				//}

				//SetProfileXRef(profXRef);
				//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.

				//===================================================================
				// Build AssortmentTotalVariable/AssortmentDetailVariable ProfileXRef
				//===================================================================

				profXRef = new ProfileXRef(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable);

				//Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
				profXRef.AddXRefIdEntry(((AssortmentViewTotalVariables)AssortmentComputations.AssortmentTotalVariables).TotalPct.Key, ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalPct.Key);
				//End TT#1143 - JScott - Total % change receives Nothing to Spread exception
				profXRef.AddXRefIdEntry(((AssortmentViewTotalVariables)AssortmentComputations.AssortmentTotalVariables).TotalUnits.Key, ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key);
				//Begin TT#1143 - JScott - Total % change receives Nothing to Spread exception
				profXRef.AddXRefIdEntry(((AssortmentViewTotalVariables)AssortmentComputations.AssortmentTotalVariables).AvgUnits.Key, ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).AvgUnits.Key);
				//End TT#1143 - JScott - Total % change receives Nothing to Spread exception
				profXRef.AddXRefIdEntry(((AssortmentViewTotalVariables)AssortmentComputations.AssortmentTotalVariables).UnitCost.Key, ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).UnitCost.Key);
				profXRef.AddXRefIdEntry(((AssortmentViewTotalVariables)AssortmentComputations.AssortmentTotalVariables).UnitRetail.Key, ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).UnitRetail.Key);

				SetProfileXRef(profXRef);

				//================================================
				// Create AssortmentHeaderColorDetail in CubeGroup
				//================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.AllocationHeader, 5),
					new DimensionDefinition(eProfileType.HeaderPack, 5),
					new DimensionDefinition(eProfileType.HeaderPackColor, 5),
					new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
					new DimensionDefinition(eProfileType.StoreGrade, 5),
					new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderColorDetail);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentHeaderColorDetail(SAB, Transaction, this, cubeDef, 1, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentHeaderColorDetail, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				profTypeList = new ArrayList();

				profTypeList.Add(eProfileType.AllocationHeader);
				profTypeList.Add(eProfileType.HeaderPack);
				profTypeList.Add(eProfileType.HeaderPackColor);

				// Begin TT#2 - stodd
				//assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
				//    new CubeComponentRelationshipItem(
				//        (eProfileType[])profTypeList.ToArray(typeof(eProfileType)),
				//        (eProfileType[])profTypeList.ToArray(typeof(eProfileType)))));
				// End TT#2 - stodd

				//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
				//    new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				assrtCube.AddTotalCube(eCubeType.AssortmentPlaceholderColorDetail);

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                ////===============================================
                //// Create AssortmentHeaderPackDetail in CubeGroup
                ////===============================================

                //cubeDef = new CubeDefinition(
                //    new DimensionDefinition(eProfileType.AllocationHeader, 5),
                //    new DimensionDefinition(eProfileType.HeaderPack, 5),
                //    new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
                //    new DimensionDefinition(eProfileType.StoreGrade, 5),
                //    new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5),
                //    new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

                //assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderPackDetail);

                //if (assrtCube == null)
                //{
                //    assrtCube = new AssortmentHeaderPackDetail(SAB, Transaction, this, cubeDef, int.MaxValue, (Transaction.DataState == eDataState.ReadOnly), false);
                //    assrtCube.InitializeCube();
                //    AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
                //    SetCube(eCubeType.AssortmentHeaderPackDetail, assrtCube);
                //}
                //else
                //{
                //    assrtCube.ExpandDimensionSize(cubeDef);
                //}

                //assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
                //    new CubeComponentRelationshipItem(
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack },
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //assrtCube.SetComponentDetailCube(eCubeType.AssortmentHeaderColorDetail);
                //assrtCube.SetSpreadDetailCube(eCubeType.AssortmentHeaderColorDetail);

                //assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderColorDetail);

                //assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderPackDetail,
                //    new CubeComponentRelationshipItem(
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack },
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //assrtCube.AddTotalCube(eCubeType.AssortmentHeaderPackDetail);

                //End TT#2 - JScott - Assortment Planning - Phase 2
                //==========================================
				// Create AssortmentHeaderTotalDetail in CubeGroup
				//==========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.AllocationHeader, 5),
					new DimensionDefinition(eProfileType.HeaderPack, 5),
					new DimensionDefinition(eProfileType.HeaderPackColor, 5),
					new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderTotalDetail);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentHeaderTotalDetail(SAB, Transaction, this, cubeDef, 1, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentHeaderTotalDetail, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				//=====================================================
				// Create AssortmentPlaceholderColorDetail in CubeGroup
				//=====================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.PlaceholderHeader, 5),
					new DimensionDefinition(eProfileType.HeaderPack, 5),
					new DimensionDefinition(eProfileType.HeaderPackColor, 5),
					new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
					new DimensionDefinition(eProfileType.StoreGrade, 5),
					new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderColorDetail);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentPlaceholderColorDetail(SAB, Transaction, this, cubeDef, 1, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentPlaceholderColorDetail, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				profTypeList = new ArrayList();

				profTypeList.Add(eProfileType.PlaceholderHeader);
				profTypeList.Add(eProfileType.HeaderPack);
				profTypeList.Add(eProfileType.HeaderPackColor);

				// Begin TT#2 - stodd
				//assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
				//    new CubeComponentRelationshipItem(
				//        (eProfileType[])profTypeList.ToArray(typeof(eProfileType)),
				//        (eProfileType[])profTypeList.ToArray(typeof(eProfileType)))));
				// Begin TT#2 - stodd

				//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
				//    new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//End TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
				assrtCube.AddTotalCube(eCubeType.AssortmentPlaceholderGradeTotal);

				//End TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
				//Begin TT#2 - JScott - Assortment Planning - Phase 2
                ////====================================================
                //// Create AssortmentPlaceholderPackDetail in CubeGroup
                ////====================================================

                //cubeDef = new CubeDefinition(
                //    new DimensionDefinition(eProfileType.PlaceholderHeader, 5),
                //    new DimensionDefinition(eProfileType.HeaderPack, 5),
                //    new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
                //    new DimensionDefinition(eProfileType.StoreGrade, 5),
                //    new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5),
                //    new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

                //assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderPackDetail);

                //if (assrtCube == null)
                //{
                //    assrtCube = new AssortmentPlaceholderPackDetail(SAB, Transaction, this, cubeDef, int.MaxValue, (Transaction.DataState == eDataState.ReadOnly), false);
                //    assrtCube.InitializeCube();
                //    AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
                //    SetCube(eCubeType.AssortmentPlaceholderPackDetail, assrtCube);
                //}
                //else
                //{
                //    assrtCube.ExpandDimensionSize(cubeDef);
                //}

                //assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
                //    new CubeComponentRelationshipItem(
                //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack },
                //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //assrtCube.SetComponentDetailCube(eCubeType.AssortmentPlaceholderColorDetail);
                //assrtCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderColorDetail);

                //assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderColorDetail);

                //assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderPackDetail,
                //    new CubeComponentRelationshipItem(
                //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack },
                //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //assrtCube.AddTotalCube(eCubeType.AssortmentPlaceholderPackDetail);

                //End TT#2 - JScott - Assortment Planning - Phase 2
                //===============================================
				// Create AssortmentPlaceholderTotalDetail in CubeGroup
				//===============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.PlaceholderHeader, 5),
					new DimensionDefinition(eProfileType.HeaderPack, 5),
					new DimensionDefinition(eProfileType.HeaderPackColor, 5),
					new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderTotalDetail);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentPlaceholderTotalDetail(SAB, Transaction, this, cubeDef, 1, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentPlaceholderTotalDetail, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				//Begin TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
				//=====================================================
				// Create AssortmentPlaceholderGradeTotal in CubeGroup
				//=====================================================

				cubeDef = new CubeDefinition(
					  new DimensionDefinition(eProfileType.PlaceholderHeader, 5),
					  new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
					  new DimensionDefinition(eProfileType.StoreGrade, 5),
					  new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5),
					  new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderGradeTotal);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentPlaceholderGradeTotal(SAB, Transaction, this, cubeDef, 2, true, false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentPlaceholderGradeTotal, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				plcGrdTotTypeArr = new eProfileType[1];
				plcGrdTotTypeArr[0] = eProfileType.PlaceholderHeader;

				plcGrdTotCompXRef = new ComponentProfileXRef(eCubeType.None, plcGrdTotTypeArr);

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentPlaceholderGradeTotal, plcGrdTotCompXRef));

				assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
					new CubeComponentRelationshipItem(plcGrdTotTypeArr, new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

				assrtCube.SetComponentDetailCube(eCubeType.AssortmentPlaceholderColorDetail);
				assrtCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderColorDetail);

				//End TT#2 - JScott - Add AssortmentPlaceholderGradeTotal Cube
				//===========================================
				// Create AssortmentSummaryGrade in CubeGroup
				//===========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
					new DimensionDefinition(eProfileType.StoreGrade, 5),
					new DimensionDefinition(eProfileType.AssortmentSummaryVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentSummaryGrade);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentSummaryGrade(SAB, Transaction, this, cubeDef, 2, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentSummaryGrade, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				assrtCube.AddTotalCube(eCubeType.AssortmentSummaryTotal);

				//======================================================
				// Create AssortmentSummaryGroupLevel in CubeGroup
				//======================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.StoreGroupLevel, 5),
					new DimensionDefinition(eProfileType.AssortmentSummaryVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentSummaryGroupLevel);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentSummaryGroupLevel(SAB, Transaction, this, cubeDef, 3, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentSummaryGroupLevel, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				assrtCube.AddTotalCube(eCubeType.AssortmentSummaryTotal);

				//=================================================
				// Create AssortmentSummaryTotal in CubeGroup
				//=================================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.AssortmentSummaryVariable, 5),
					new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				assrtCube = (AssortmentCube)GetCube(eCubeType.AssortmentSummaryTotal);

				if (assrtCube == null)
				{
					assrtCube = new AssortmentSummaryTotal(SAB, Transaction, this, cubeDef, 4, (Transaction.DataState == eDataState.ReadOnly), false);
					assrtCube.InitializeCube();
					AssortmentComputations.VariableInitializations.InitializeVariables(assrtCube);
					SetCube(eCubeType.AssortmentSummaryTotal, assrtCube);
				}
				else
				{
					assrtCube.ExpandDimensionSize(cubeDef);
				}

				assrtCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentSummaryGroupLevel,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master)));

				assrtCube.SetComponentDetailCube(eCubeType.AssortmentSummaryGroupLevel);
				assrtCube.SetSpreadDetailCube(eCubeType.AssortmentSummaryGroupLevel);

				//ReadData();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
		private void BuildComponentProfileXRef()
		{
			DataTable dtPlaceholderComponents;
			DataTable dtHeaderComponents;
			ArrayList profTypeList;
			ArrayList varRIDList;
			ComponentProfileXRef plcColorDtlCompXRef;
			ComponentProfileXRef plcTotCompXRef;
			ComponentProfileXRef hdrColorDtlCompXRef;
			ComponentProfileXRef hdrTotCompXRef;

			try
			{
				dtPlaceholderComponents = GetPlaceholderComponents();
				dtHeaderComponents = GetHeaderComponents();

				profTypeList = new ArrayList();
				varRIDList = new ArrayList();

				foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
				{
					profTypeList.Add(varProf.ProfileListType);
				}

				plcColorDtlCompXRef = new ComponentProfileXRef(eCubeType.AssortmentPlaceholderColorDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));
				plcTotCompXRef = new ComponentProfileXRef(eCubeType.AssortmentPlaceholderTotalDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));

				foreach (DataRow row in dtPlaceholderComponents.Rows)
				{
					varRIDList.Clear();

					foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
					{
						varRIDList.Add(Convert.ToInt32(row[varProf.RIDColumnName]));
					}

					plcColorDtlCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
					plcTotCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
				}

				SetProfileXRef(plcColorDtlCompXRef);
				SetProfileXRef(plcTotCompXRef);

				hdrColorDtlCompXRef = new ComponentProfileXRef(eCubeType.AssortmentHeaderColorDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));
				hdrTotCompXRef = new ComponentProfileXRef(eCubeType.AssortmentHeaderTotalDetail, (eProfileType[])profTypeList.ToArray(typeof(eProfileType)));

				foreach (DataRow row in dtHeaderComponents.Rows)
				{
					varRIDList.Clear();

					foreach (AssortmentComponentVariableProfile varProf in AssortmentComponentVariables.VariableProfileList)
					{
						varRIDList.Add(Convert.ToInt32(row[varProf.RIDColumnName]));
					}

					hdrColorDtlCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
					hdrTotCompXRef.AddXRefIdEntry((int[])varRIDList.ToArray(typeof(int)));
				}

				SetProfileXRef(hdrColorDtlCompXRef);
				SetProfileXRef(hdrTotCompXRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
		public void DefineTotalCubes(SortedList aSortedComponentColumnHeaders)
		{
			int i;
			int j;

			DataTable dtPlaceholderComponents;
			DataTable dtHeaderComponents;

			ArrayList masterDimList;
			ArrayList masterTypeList;
			ArrayList summedDimList;
			ArrayList summedTypeList;

			//bool colorDefined;		// TT#1225 - stodd
			RowColProfileHeader varHdr;
			AssortmentComponentVariableProfile varProf;

			int[] componentIds;
			ComponentProfileXRef hdrCompXRef;
			ComponentProfileXRef plcCompXRef;
			ComponentProfileXRef tmpCompXRef;
			ComponentProfileXRef[] subTotHdrCompXRef;
			ComponentProfileXRef[] subTotPlcCompXRef;
			ArrayList tmpTypeList;
			ArrayList[] subTotDimList;
			ArrayList[] subTotTypeList;

            bool[] subTotContainsColor;
            //Begin TT#2 - JScott - Assortment Planning - Phase 2
            //bool[] subTotContainsPack;
            //End TT#2 - JScott - Assortment Planning - Phase 2
            bool containsColor;
            //Begin TT#2 - JScott - Assortment Planning - Phase 2
            //bool containsPack;
            //End TT#2 - JScott - Assortment Planning - Phase 2

            ArrayList headerDimList;
			ArrayList headerTypeList;
			eCubeType headerCubeType;
			AssortmentCube headerCube;

			ArrayList placeholderDimList;
			ArrayList placeholderTypeList;
			eCubeType placeholderCubeType;
			AssortmentCube placeholderCube;

			ArrayList lastHeaderDetailTypeList = null;
			eCubeType lastHeaderDetailCubeType = null;
			AssortmentCube lastHeaderDetailCube = null;

			ArrayList lastPlaceholderDetailTypeList = null;
			eCubeType lastPlaceholderDetailCubeType = null;
			AssortmentCube lastPlaceholderDetailCube = null;

			AssortmentCube colorDetailCube;
            //Begin TT#2 - JScott - Assortment Planning - Phase 2
            //AssortmentCube packDetailCube;
            //End TT#2 - JScott - Assortment Planning - Phase 2

			eCubeType gradeSubTotalCubeType;
			eCubeType groupLevelSubTotalCubeType;
			AssortmentCube gradeCube;
			AssortmentCube gradeSubTotalCube;
			AssortmentCube groupLevelCube;
			AssortmentCube groupLevelSubTotalCube;

			try
			{
				dtPlaceholderComponents = GetPlaceholderComponents();
				dtHeaderComponents = GetHeaderComponents();

				masterDimList = new ArrayList();
				masterTypeList = new ArrayList();
				summedDimList = new ArrayList();
				summedTypeList = new ArrayList();

				_headerIndex = -1;
                //packDefined = false;
				_colorDefined = false;	// TT#1225 - stodd
				_styleSkuDefined = false;	//TT#2402 - stodd
				i = 0;

				// Create lists of DimensionDefinitions and ProfileTypes for the summarized variables

				foreach (DictionaryEntry varEntry in aSortedComponentColumnHeaders)
				{
					varHdr = (RowColProfileHeader)varEntry.Value;
					varProf = (AssortmentComponentVariableProfile)varHdr.Profile;

					masterDimList.Add(new DimensionDefinition(varProf.ProfileListType, 5));
					masterTypeList.Add(varProf.ProfileListType);

					if (varHdr.IsSummarized)
					{
						summedDimList.Add(new DimensionDefinition(varProf.ProfileListType, 5));
						summedTypeList.Add(varProf.ProfileListType);
					}

					i++;

					if (varProf.Key == ((AssortmentViewComponentVariables)AssortmentComponentVariables).HeaderID.Key)
					{
					    _headerIndex = i;
					}
					else if (varProf.Key == ((AssortmentViewComponentVariables)AssortmentComponentVariables).Pack.Key)
					{
                        //packDefined = true;
					}
					else if (varProf.Key == ((AssortmentViewComponentVariables)AssortmentComponentVariables).Color.Key)
					{
						_colorDefined = true;	// TT#1225 - stodd 
					}
					// Begin TT#1225 - stodd
					else if (varProf.Key == 804593)
					{
						_styleSkuDefined = true;	
					}
					// End TT#2402 - stodd
				}

				_numSummaryLevels = summedTypeList.Count;

				// Create ComponentProfileXRef from the header components

				hdrCompXRef = new ComponentProfileXRef(eCubeType.None, (eProfileType[])masterTypeList.ToArray(typeof(eProfileType)));
				componentIds = new int[masterTypeList.Count];

				foreach (DataRow row in dtHeaderComponents.Rows)
				{
					for (i = 0; i < masterTypeList.Count; i++)
					{
						componentIds[i] = Convert.ToInt32(row[AssortmentComponentVariables.GetVariableProfileByProfileType((eProfileType)masterTypeList[i]).RIDColumnName]);
					}

					hdrCompXRef.AddXRefIdEntry(componentIds);
				}

				// Create ComponentProfileXRef from the placeholder components

				plcCompXRef = new ComponentProfileXRef(eCubeType.None, (eProfileType[])masterTypeList.ToArray(typeof(eProfileType)));
				componentIds = new int[masterTypeList.Count];

				foreach (DataRow row in dtPlaceholderComponents.Rows)
				{
					for (i = 0; i < masterTypeList.Count; i++)
					{
						componentIds[i] = Convert.ToInt32(row[AssortmentComponentVariables.GetVariableProfileByProfileType((eProfileType)masterTypeList[i]).RIDColumnName]);
					}

					plcCompXRef.AddXRefIdEntry(componentIds);
				}

				// Create lists of DimensionDefinitions and ProfileTypes for the summarized variables at each sub-total level

				subTotDimList = new ArrayList[summedTypeList.Count];
				subTotTypeList = new ArrayList[summedTypeList.Count];
                subTotContainsColor = new bool[summedTypeList.Count];
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //subTotContainsPack = new bool[summedTypeList.Count];
                //End TT#2 - JScott - Assortment Planning - Phase 2

				for (i = 0; i < summedTypeList.Count; i++)
				{
					subTotDimList[summedTypeList.Count - i - 1] = new ArrayList();
					subTotTypeList[summedTypeList.Count - i - 1] = new ArrayList();

                    containsColor = false;
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //containsPack = false;
                    //End TT#2 - JScott - Assortment Planning - Phase 2

                    for (j = 0; j < summedTypeList.Count - i; j++)
					{
						subTotDimList[summedTypeList.Count - i - 1].Add(summedDimList[j]);
						subTotTypeList[summedTypeList.Count - i - 1].Add(summedTypeList[j]);

                        if ((eProfileType)summedTypeList[j] == eProfileType.HeaderPackColor)
                        {
                            containsColor = true;
                        }
                        //Begin TT#2 - JScott - Assortment Planning - Phase 2
                        //else if ((eProfileType)summedTypeList[j] == eProfileType.HeaderPack)
                        //{
                        //    containsPack = true;
                        //}
                        //End TT#2 - JScott - Assortment Planning - Phase 2
                    }

                    subTotContainsColor[summedTypeList.Count - i - 1] = containsColor;
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //subTotContainsPack[summedTypeList.Count - i - 1] = containsPack;
                    //End TT#2 - JScott - Assortment Planning - Phase 2
                }

				// Create ComponentProfileXRef from the header components at each sub-total level

				subTotHdrCompXRef = new ComponentProfileXRef[summedTypeList.Count];

				for (i = 0; i < summedTypeList.Count; i++)
				{
					tmpTypeList = subTotTypeList[summedTypeList.Count - i - 1];
					tmpCompXRef = new ComponentProfileXRef(eCubeType.None, (eProfileType[])tmpTypeList.ToArray(typeof(eProfileType)));
					componentIds = new int[tmpTypeList.Count];

					foreach (DataRow row in dtHeaderComponents.Rows)
					{
						for (j = 0; j < tmpTypeList.Count; j++)
						{
							componentIds[j] = Convert.ToInt32(row[AssortmentComponentVariables.GetVariableProfileByProfileType((eProfileType)tmpTypeList[j]).RIDColumnName]);
						}

						tmpCompXRef.AddXRefIdEntry(componentIds);
					}

					subTotHdrCompXRef[summedTypeList.Count - i - 1] = tmpCompXRef;
				}

				// Create ComponentProfileXRef from the placeholder components at each sub-total level

				subTotPlcCompXRef = new ComponentProfileXRef[summedTypeList.Count];

				for (i = 0; i < summedTypeList.Count; i++)
				{
					tmpTypeList = subTotTypeList[summedTypeList.Count - i - 1];
					tmpCompXRef = new ComponentProfileXRef(eCubeType.None, (eProfileType[])tmpTypeList.ToArray(typeof(eProfileType)));
					componentIds = new int[tmpTypeList.Count];

					foreach (DataRow row in dtPlaceholderComponents.Rows)
					{
						for (j = 0; j < tmpTypeList.Count; j++)
						{
							componentIds[j] = Convert.ToInt32(row[AssortmentComponentVariables.GetVariableProfileByProfileType((eProfileType)tmpTypeList[j]).RIDColumnName]);
						}

						tmpCompXRef.AddXRefIdEntry(componentIds);
					}

					subTotPlcCompXRef[summedTypeList.Count - i - 1] = tmpCompXRef;
				}

				//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//===================
				//===================
				// COLOR DETAIL CUBES
				//===================
				//===================

				headerCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderColorDetail);

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				placeholderCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderColorDetail);

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
				//============
				//============
				// GRADE CUBES
				//============
				//============

				//===========================================
				// Define AssortmentComponentHeaderGrade cube
				//===========================================

				// Build cube parameters

				headerDimList = (ArrayList)masterDimList.Clone();

				headerDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.StoreGrade, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Build ComponentProfileXRef

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentComponentHeaderGrade, hdrCompXRef));

				// Define cube

				headerCube = new AssortmentComponentHeaderGrade(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
					2,
					(Transaction.DataState == eDataState.ReadOnly),
					false);

				AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
				SetCube(eCubeType.AssortmentComponentHeaderGrade, headerCube);

				// Get Related Cubes
				
				colorDetailCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderColorDetail);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //packDetailCube = (AssortmentCube)GetCube(eCubeType.AssortmentHeaderPackDetail);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				// Define Relationships

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
					new CubeComponentRelationshipItem(
						(eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor } )));

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderPackDetail,
                //    new CubeComponentRelationshipItem(
                //        (eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack })));

                //End TT#2 - JScott - Assortment Planning - Phase 2
                colorDetailCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGrade,
					new CubeComponentRelationshipItem(
						(eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //if (!colorDefined)
                //{
                //    packDetailCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGrade,
                //    new CubeComponentRelationshipItem(
                //        (eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack })));
                //}

                //End TT#2 - JScott - Assortment Planning - Phase 2
                headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGrade,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //headerCube.SetComponentDetailCube(eCubeType.AssortmentHeaderColorDetail);

                //if (colorDefined)
                //{
                //    headerCube.SetSpreadDetailCube(eCubeType.AssortmentHeaderColorDetail);
                //}
                //else
                //{
                //    headerCube.SetSpreadDetailCube(eCubeType.AssortmentHeaderPackDetail);
                //    packDetailCube.AddTotalCube(eCubeType.AssortmentComponentHeaderGrade);
                //}

                //colorDetailCube.AddTotalCube(eCubeType.AssortmentComponentHeaderGrade);
                headerCube.SetComponentDetailCube(eCubeType.AssortmentHeaderColorDetail);
                headerCube.SetSpreadDetailCube(eCubeType.AssortmentHeaderColorDetail);

                colorDetailCube.AddTotalCube(eCubeType.AssortmentComponentHeaderGrade);

                //End TT#2 - JScott - Assortment Planning - Phase 2
                headerCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderGrade);

				//================================================
				// Define AssortmentComponentPlaceholderGrade cube
				//================================================

				// Build cube parameters

				placeholderDimList = (ArrayList)masterDimList.Clone();

				placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGrade, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Build ComponentProfileXRef

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentComponentPlaceholderGrade, plcCompXRef));

				// Define cube

				placeholderCube = new AssortmentComponentPlaceholderGrade(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
					2,
					(Transaction.DataState == eDataState.ReadOnly),
					false);

				AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
				SetCube(eCubeType.AssortmentComponentPlaceholderGrade, placeholderCube);

				// Get Related Cubes

				colorDetailCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderColorDetail);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //packDetailCube = (AssortmentCube)GetCube(eCubeType.AssortmentPlaceholderPackDetail);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				// Define Relationships

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
					new CubeComponentRelationshipItem(
						(eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor } )));

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderPackDetail,
                //    new CubeComponentRelationshipItem(
                //        (eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
                //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack })));

                //End TT#2 - JScott - Assortment Planning - Phase 2
                colorDetailCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGrade,
					new CubeComponentRelationshipItem(
						(eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //if (!colorDefined)
                //{
                //    packDetailCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGrade,
                //        new CubeComponentRelationshipItem(
                //            (eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
                //            new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack })));
                //}

                //End TT#2 - JScott - Assortment Planning - Phase 2
                placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGrade,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //placeholderCube.SetComponentDetailCube(eCubeType.AssortmentPlaceholderColorDetail);

                //if (colorDefined)
                //{
                //    placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderColorDetail);
                //}
                //else
                //{
                //    placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderPackDetail);
                //    packDetailCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderGrade);
                //}

                //colorDetailCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderGrade);

                placeholderCube.SetComponentDetailCube(eCubeType.AssortmentPlaceholderColorDetail);
                placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderColorDetail);

                colorDetailCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderGrade);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				// Set variables for next cubes

				lastHeaderDetailTypeList = masterTypeList;
				lastHeaderDetailCubeType = eCubeType.AssortmentComponentHeaderGrade;
				lastHeaderDetailCube = headerCube;

				lastPlaceholderDetailTypeList = masterTypeList;
				lastPlaceholderDetailCubeType = eCubeType.AssortmentComponentPlaceholderGrade;
				lastPlaceholderDetailCube = placeholderCube;

				//======================
				//======================
				// GRADE SUBTOTALS CUBES
				//======================
				//======================

				for (i = _numSummaryLevels; i > 0; i--)
				{
					//===================================================
					// Define AssortmentComponentHeaderGradeSubTotal cube
					//===================================================

					// Build cube parameters

					headerCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i);

					if (i > _headerIndex)
					{
						gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i - 1);
					}
					else
					{
						gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i - 1);
					}

					headerTypeList = (ArrayList)subTotTypeList[i - 1].Clone();
					headerDimList = (ArrayList)subTotDimList[i - 1].Clone();

					headerDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
					headerDimList.Add(new DimensionDefinition(eProfileType.StoreGrade, 5));
					headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
					headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

					// Build ComponentProfileXRef

					SetProfileXRef(new ComponentProfileXRef(headerCubeType, subTotHdrCompXRef[i - 1]));

					// Define cube

					headerCube = new AssortmentComponentHeaderGradeSubTotal(
						SAB,
						Transaction,
						this,
						new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
						5 * (i + 1),
						(Transaction.DataState == eDataState.ReadOnly),
						false,
						headerCubeType,
						gradeSubTotalCubeType);

					AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
					SetCube(headerCubeType, headerCube);

					// Define Relationships

					headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderPackDetail,
                    //    new CubeComponentRelationshipItem(
                    //        (eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
                    //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack })));

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    headerCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastHeaderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					headerCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i),
						new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

					// Connect Cubes

					headerCube.SetComponentDetailCube(lastHeaderDetailCubeType);
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2

                    //if (subTotContainsColor[i - 1])
                    //{
                    //    headerCube.SetSpreadDetailCube(eCubeType.AssortmentHeaderColorDetail);
                    //    ((AssortmentComponentCube)headerCube).SpreadToDetail = true;
                    //}
                    //else if (subTotContainsPack[i - 1])
                    //{
                    //    headerCube.SetSpreadDetailCube(eCubeType.AssortmentHeaderPackDetail);
                    //    ((AssortmentComponentCube)headerCube).SpreadToDetail = true;
                    //}
                    //else
                    //{
                    //    headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);
                    //    ((AssortmentComponentCube)headerCube).SpreadToDetail = false;
                    //}
                    headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    lastHeaderDetailCube.AddTotalCube(headerCubeType);
					headerCube.AddTotalCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i));

					//========================================================
					// Define AssortmentComponentPlaceholderGradeSubTotal cube
					//========================================================

					// Build cube parameters

					placeholderCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i);

					placeholderTypeList = (ArrayList)subTotTypeList[i - 1].Clone();
					placeholderDimList = (ArrayList)subTotDimList[i - 1].Clone();

					placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
					placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGrade, 5));
					placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
					placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

					// Build ComponentProfileXRef

					SetProfileXRef(new ComponentProfileXRef(placeholderCubeType, subTotPlcCompXRef[i - 1]));

					// Define cube

					placeholderCube = new AssortmentComponentPlaceholderGradeSubTotal(
						SAB,
						Transaction,
						this,
						new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
						5 * (i + 1),
						(Transaction.DataState == eDataState.ReadOnly),
						false,
						placeholderCubeType,
						new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i - 1));

					AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
					SetCube(placeholderCubeType, placeholderCube);

					// Define Relationships

					placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderPackDetail,
                    //    new CubeComponentRelationshipItem(
                    //        (eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
                    //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack })));

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    placeholderCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastHeaderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					placeholderCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i),
						new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

					// Connect Cubes

					placeholderCube.SetComponentDetailCube(lastPlaceholderDetailCubeType);
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2

                    //if (subTotContainsColor[i - 1])
                    //{
                    //    placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderColorDetail);
                    //    ((AssortmentComponentCube)placeholderCube).SpreadToDetail = true;
                    //}
                    //else if (subTotContainsPack[i - 1])
                    //{
                    //    placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentPlaceholderPackDetail);
                    //    ((AssortmentComponentCube)placeholderCube).SpreadToDetail = true;
                    //}
                    //else
                    //{
                    //    placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);
                    //    ((AssortmentComponentCube)placeholderCube).SpreadToDetail = false;
                    //}
                    placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);

                    //End TT#2 - JScott - Assortment Planning - Phase 2
                    lastPlaceholderDetailCube.AddTotalCube(placeholderCubeType);

					// Set variables for next cubes

					lastHeaderDetailTypeList = headerTypeList;
					lastHeaderDetailCubeType = headerCubeType;
					lastHeaderDetailCube = headerCube;

					lastPlaceholderDetailTypeList = placeholderTypeList;
					lastPlaceholderDetailCubeType = placeholderCubeType;
					lastPlaceholderDetailCube = placeholderCube;
				}

				//===================
				//===================
				// GRADE TOTALS CUBES
				//===================
				//===================

				//===============================================================
				// Define Grand Total AssortmentComponentHeaderGradeSubTotal cube
				//===============================================================

				// Build cube parameters

				headerCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, 0);
				headerDimList = new ArrayList();
				headerTypeList = new ArrayList();

				headerDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.StoreGrade, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Define cube

				headerCube = new AssortmentComponentHeaderGradeSubTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
					5,
					(Transaction.DataState == eDataState.ReadOnly),
					false,
					headerCubeType,
					eCubeType.None);

				AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
				SetCube(headerCubeType, headerCube);

				// Define Relationships

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderColorDetail,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderPackDetail,
                //    new CubeComponentRelationshipItem(
                //        (eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
                //        new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack })));

                //End TT#2 - JScott - Assortment Planning - Phase 2
                headerCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastHeaderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				headerCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0),
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				headerCube.SetComponentDetailCube(lastHeaderDetailCubeType);
				headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //((AssortmentComponentCube)headerCube).SpreadToDetail = false;
                //End TT#2 - JScott - Assortment Planning - Phase 2

				lastHeaderDetailCube.AddTotalCube(headerCubeType);
				headerCube.AddTotalCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0));

				//====================================================================
				// Define Grand Total AssortmentComponentPlaceholderGradeSubTotal cube
				//====================================================================

				// Build cube parameters

				placeholderCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0);
				placeholderDimList = new ArrayList();
				placeholderTypeList = new ArrayList();

				placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGrade, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Define cube

				placeholderCube = new AssortmentComponentPlaceholderGradeSubTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
					5,
					(Transaction.DataState == eDataState.ReadOnly),
					false,
					placeholderCubeType,
					eCubeType.None);

				AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
				SetCube(placeholderCubeType, placeholderCube);

				// Define Relationships

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderColorDetail,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor })));

                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentPlaceholderPackDetail,
                //    new CubeComponentRelationshipItem(
                //        (eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
                //        new eProfileType[] { eProfileType.PlaceholderHeader, eProfileType.HeaderPack })));

                //End TT#2 - JScott - Assortment Planning - Phase 2
                placeholderCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastHeaderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				placeholderCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, 0),
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				placeholderCube.SetComponentDetailCube(lastPlaceholderDetailCubeType);
				placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //((AssortmentComponentCube)placeholderCube).SpreadToDetail = false;
                //End TT#2 - JScott - Assortment Planning - Phase 2

				lastPlaceholderDetailCube.AddTotalCube(placeholderCubeType);

				//==================
				//==================
				// GROUP LEVEL CUBES
				//==================
				//==================

				//======================================================
				// Define AssortmentComponentHeaderDetailGroupLevel cube
				//======================================================

				// Build cube parameters

				headerDimList = (ArrayList)masterDimList.Clone();

				headerDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Build ComponentProfileXRef

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentComponentHeaderGroupLevel, hdrCompXRef));

				// Define cube

				headerCube = new AssortmentComponentHeaderGroupLevel(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
					3,
					(Transaction.DataState == eDataState.ReadOnly),
					false);

				AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
				SetCube(eCubeType.AssortmentComponentHeaderGroupLevel, headerCube);

				// Get Related Cubes

				gradeCube = (AssortmentCube)GetCube(eCubeType.AssortmentComponentHeaderGrade);

				// Define Relationships

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGrade,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				gradeCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGroupLevel,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGroupLevel,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				headerCube.SetComponentDetailCube(eCubeType.AssortmentComponentHeaderGrade);
				headerCube.SetSpreadDetailCube(eCubeType.AssortmentComponentHeaderGrade);

				gradeCube.AddTotalCube(eCubeType.AssortmentComponentHeaderGroupLevel);
				headerCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderGroupLevel);

				//===========================================================
				// Define AssortmentComponentPlaceholderGroupLevel cube
				//===========================================================

				// Build cube parameters

				placeholderDimList = (ArrayList)masterDimList.Clone();

				placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Build ComponentProfileXRef

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentComponentPlaceholderGroupLevel, plcCompXRef));

				// Define cube

				placeholderCube = new AssortmentComponentPlaceholderGroupLevel(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
					3,
					(Transaction.DataState == eDataState.ReadOnly),
					false);

				AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
				SetCube(eCubeType.AssortmentComponentPlaceholderGroupLevel, placeholderCube);

				// Get Related Cubes

				gradeCube = (AssortmentCube)GetCube(eCubeType.AssortmentComponentPlaceholderGrade);

				// Define Relationships

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGrade,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				gradeCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGroupLevel,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGroupLevel,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				placeholderCube.SetComponentDetailCube(eCubeType.AssortmentComponentPlaceholderGrade);
				placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentComponentPlaceholderGrade);

				gradeCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderGroupLevel);

				// Set variables for next cubes

				lastHeaderDetailTypeList = masterTypeList;
				lastHeaderDetailCubeType = eCubeType.AssortmentComponentHeaderGroupLevel;
				lastHeaderDetailCube = headerCube;

				lastPlaceholderDetailTypeList = masterTypeList;
				lastPlaceholderDetailCubeType = eCubeType.AssortmentComponentPlaceholderGroupLevel;
				lastPlaceholderDetailCube = placeholderCube;

				//============================
				//============================
				// GROUP LEVEL SUBTOTALS CUBES
				//============================
				//============================

				for (i = _numSummaryLevels; i > 0; i--)
				{
					//========================================================
					// Define AssortmentComponentHeaderGroupLevelSubTotal cube
					//========================================================

					// Build cube parameters

					headerCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, i);
					gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i);

					if (i > _headerIndex)
					{
						groupLevelSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, i - 1);
					}
					else
					{
						groupLevelSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, i - 1);
					}

					headerTypeList = (ArrayList)subTotTypeList[i - 1].Clone();
					headerDimList = (ArrayList)subTotDimList[i - 1].Clone();

					headerDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
					headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
					headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

					// Build ComponentProfileXRef

					SetProfileXRef(new ComponentProfileXRef(headerCubeType, subTotHdrCompXRef[i - 1]));

					// Define cube

					headerCube = new AssortmentComponentHeaderGroupLevelSubTotal(
						SAB,
						Transaction,
						this,
						new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
						6 * (i + 1),
						(Transaction.DataState == eDataState.ReadOnly),
						false,
						headerCubeType,
						groupLevelSubTotalCubeType);

					AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
					SetCube(headerCubeType, headerCube);

					// Get Related Cubes

					gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);

					// Define Relationships

					headerCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

					gradeSubTotalCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

					headerCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastHeaderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					headerCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i),
						new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

					// Connect Cubes

					headerCube.SetComponentDetailCube(lastHeaderDetailCubeType);
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //headerCube.SetSpreadDetailCube(gradeSubTotalCubeType);
                    headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);
                    //End TT#2 - JScott - Assortment Planning - Phase 2

					gradeSubTotalCube.AddTotalCube(headerCubeType);
					lastHeaderDetailCube.AddTotalCube(headerCubeType);
					headerCube.AddTotalCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i));

					//=============================================================
					// Define AssortmentComponentPlaceholderGroupLevelSubTotal cube
					//=============================================================

					// Build cube parameters

					placeholderCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, i);
					gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i);

					placeholderTypeList = (ArrayList)subTotTypeList[i - 1].Clone();
					placeholderDimList = (ArrayList)subTotDimList[i - 1].Clone();

					placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
					placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
					placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

					// Build ComponentProfileXRef

					SetProfileXRef(new ComponentProfileXRef(placeholderCubeType, subTotPlcCompXRef[i - 1]));

					// Define cube

					placeholderCube = new AssortmentComponentPlaceholderGroupLevelSubTotal(
						SAB,
						Transaction,
						this,
						new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
						6 * (i + 1),
						(Transaction.DataState == eDataState.ReadOnly),
						false,
						placeholderCubeType,
						new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, i - 1));

					AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
					SetCube(placeholderCubeType, placeholderCube);

					// Get Related Cubes

					gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);

					// Define Relationships

					placeholderCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

					gradeSubTotalCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

					placeholderCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastHeaderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					placeholderCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i),
						new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

					// Connect Cubes

					placeholderCube.SetComponentDetailCube(lastPlaceholderDetailCubeType);
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //placeholderCube.SetSpreadDetailCube(gradeSubTotalCubeType);
                    placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);
                    //End TT#2 - JScott - Assortment Planning - Phase 2

					gradeSubTotalCube.AddTotalCube(placeholderCubeType);
					lastPlaceholderDetailCube.AddTotalCube(placeholderCubeType);

					// Set variables for next cubes

					lastHeaderDetailTypeList = headerTypeList;
					lastHeaderDetailCubeType = headerCubeType;
					lastHeaderDetailCube = headerCube;

					lastPlaceholderDetailTypeList = placeholderTypeList;
					lastPlaceholderDetailCubeType = placeholderCubeType;
					lastPlaceholderDetailCube = placeholderCube;
				}

				//=========================
				//=========================
				// GROUP LEVEL TOTALS CUBES
				//=========================
				//=========================

				//====================================================================
				// Define Grand Total AssortmentComponentHeaderGroupLevelSubTotal cube
				//====================================================================

				// Build cube parameters

				headerCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, 0);
				headerDimList = new ArrayList();
				headerTypeList = new ArrayList();

				headerDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Define cube

				headerCube = new AssortmentComponentHeaderGroupLevelSubTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
					6,
					(Transaction.DataState == eDataState.ReadOnly),
					false,
					headerCubeType,
					eCubeType.None);

				AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
				SetCube(headerCubeType, headerCube);

				// Get Related Cubes

				gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, 0);
				gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);

				// Define Relationships

				headerCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				gradeSubTotalCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastHeaderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				headerCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0),
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				headerCube.SetComponentDetailCube(lastHeaderDetailCubeType);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //headerCube.SetSpreadDetailCube(gradeSubTotalCubeType);
                headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				gradeSubTotalCube.AddTotalCube(headerCubeType);
				lastHeaderDetailCube.AddTotalCube(headerCubeType);
				headerCube.AddTotalCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0));

				//=========================================================================
				// Define Grand Total AssortmentComponentPlaceholderGroupLevelSubTotal cube
				//=========================================================================

				// Build cube parameters

				placeholderCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, 0);
				placeholderDimList = new ArrayList();
				placeholderTypeList = new ArrayList();

				placeholderDimList.Add(new DimensionDefinition(eProfileType.StoreGroupLevel, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentDetailVariable, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Define cube

				placeholderCube = new AssortmentComponentPlaceholderGroupLevelSubTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
					6,
					(Transaction.DataState == eDataState.ReadOnly),
					false,
					placeholderCubeType,
					eCubeType.None);

				AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
				SetCube(placeholderCubeType, placeholderCube);

				// Get Related Cubes

				gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0);
				gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);

				// Define Relationships

				placeholderCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				gradeSubTotalCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master)));

				placeholderCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastHeaderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				placeholderCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, 0),
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				placeholderCube.SetComponentDetailCube(lastPlaceholderDetailCubeType);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //placeholderCube.SetSpreadDetailCube(gradeSubTotalCubeType);
                placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				gradeSubTotalCube.AddTotalCube(placeholderCubeType);
				lastPlaceholderDetailCube.AddTotalCube(placeholderCubeType);

				//============
				//============
				// TOTAL CUBES
				//============
				//============

				//===========================================
				// Define AssortmentComponentHeaderTotal cube
				//===========================================

				// Build cube parameters

				headerDimList = (ArrayList)masterDimList.Clone();

				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Build ComponentProfileXRef

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentComponentHeaderTotal, hdrCompXRef));

				// Define cube

				headerCube = new AssortmentComponentHeaderTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
					4,
					(Transaction.DataState == eDataState.ReadOnly),
					false);

				AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
				SetCube(eCubeType.AssortmentComponentHeaderTotal, headerCube);

				// Get Related Cubes

				gradeCube = (AssortmentCube)GetCube(eCubeType.AssortmentComponentHeaderGrade);
				groupLevelCube = (AssortmentCube)GetCube(eCubeType.AssortmentComponentHeaderGroupLevel);

				// Define Relationships

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentHeaderTotalDetail,
					new CubeComponentRelationshipItem(
						(eProfileType[])masterTypeList.ToArray(typeof(eProfileType)),
						new eProfileType[] { eProfileType.AllocationHeader, eProfileType.HeaderPack, eProfileType.HeaderPackColor }),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGrade,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderGroupLevel,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				gradeCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				groupLevelCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderTotal,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				headerCube.SetComponentDetailCube(eCubeType.AssortmentComponentHeaderGroupLevel);
				headerCube.SetSpreadDetailCube(eCubeType.AssortmentComponentHeaderGroupLevel);

				gradeCube.AddTotalCube(eCubeType.AssortmentComponentHeaderTotal);
				groupLevelCube.AddTotalCube(eCubeType.AssortmentComponentHeaderTotal);
				headerCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderTotal);

				//================================================
				// Define AssortmentComponentPlaceholderTotal cube
				//================================================

				// Build cube parameters

				placeholderDimList = (ArrayList)masterDimList.Clone();

				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Build ComponentProfileXRef

				SetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentComponentPlaceholderTotal, plcCompXRef));

				// Define cube

				placeholderCube = new AssortmentComponentPlaceholderTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
					4,
					(Transaction.DataState == eDataState.ReadOnly),
					false);

				AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
				SetCube(eCubeType.AssortmentComponentPlaceholderTotal, placeholderCube);

				// Get Related Cubes

				gradeCube = (AssortmentCube)GetCube(eCubeType.AssortmentComponentPlaceholderGrade);
				groupLevelCube = (AssortmentCube)GetCube(eCubeType.AssortmentComponentPlaceholderGroupLevel);

				// Define Relationships

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGrade,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				gradeCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderGroupLevel,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				groupLevelCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentPlaceholderTotal,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				placeholderCube.AddRelationship(new CubeRelationship(eCubeType.AssortmentComponentHeaderTotal,
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				placeholderCube.SetComponentDetailCube(eCubeType.AssortmentComponentPlaceholderGroupLevel);
				placeholderCube.SetSpreadDetailCube(eCubeType.AssortmentComponentPlaceholderGroupLevel);

				gradeCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderTotal);
				groupLevelCube.AddTotalCube(eCubeType.AssortmentComponentPlaceholderTotal);

				// Set variables for next cubes

				lastHeaderDetailTypeList = masterTypeList;
				lastHeaderDetailCubeType = eCubeType.AssortmentComponentHeaderTotal;
				lastHeaderDetailCube = headerCube;

				lastPlaceholderDetailTypeList = masterTypeList;
				lastPlaceholderDetailCubeType = eCubeType.AssortmentComponentPlaceholderTotal;
				lastPlaceholderDetailCube = placeholderCube;

				//======================
				//======================
				// TOTAL SUBTOTALS CUBES
				//======================
				//======================

				for (i = _numSummaryLevels; i > 0; i--)
				{
					//===================================================
					// Define AssortmentComponentHeaderTotalSubTotal cube
					//===================================================

					// Build cube parameters

					headerCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, i);
					groupLevelSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, i);

					if (i > _headerIndex)
					{
						gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, i - 1);
					}
					else
					{
						gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, i - 1);
					}

					headerTypeList = (ArrayList)subTotTypeList[i - 1].Clone();
					headerDimList = (ArrayList)subTotDimList[i - 1].Clone();

					headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5));
					headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

					// Build ComponentProfileXRef

					SetProfileXRef(new ComponentProfileXRef(headerCubeType, subTotHdrCompXRef[i - 1]));

					// Define cube

					headerCube = new AssortmentComponentHeaderTotalSubTotal(
						SAB,
						Transaction,
						this,
						new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
						7 * (i + 1),
						(Transaction.DataState == eDataState.ReadOnly),
						false,
						headerCubeType,
						gradeSubTotalCubeType);

					AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
					SetCube(headerCubeType, headerCube);

					// Get Related Cubes

					gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i);
					gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);
					groupLevelSubTotalCube = (AssortmentCube)GetCube(groupLevelSubTotalCubeType);

					// Define Relationships

					headerCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					gradeSubTotalCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					headerCube.AddRelationship(new CubeRelationship(groupLevelSubTotalCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					groupLevelSubTotalCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					headerCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastHeaderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					headerCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					headerCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i),
						new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

					// Connect Cubes

					headerCube.SetComponentDetailCube(lastHeaderDetailCubeType);
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //headerCube.SetSpreadDetailCube(groupLevelSubTotalCubeType);
                    headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);
                    //End TT#2 - JScott - Assortment Planning - Phase 2

					gradeSubTotalCube.AddTotalCube(headerCubeType);
					groupLevelSubTotalCube.AddTotalCube(headerCubeType);
					lastHeaderDetailCube.AddTotalCube(headerCubeType);
					headerCube.AddTotalCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i));

					//========================================================
					// Define AssortmentComponentPlaceholderTotalSubTotal cube
					//========================================================

					// Build cube parameters

					placeholderCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, i);
					groupLevelSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, i);

					placeholderTypeList = (ArrayList)subTotTypeList[i - 1].Clone();
					placeholderDimList = (ArrayList)subTotDimList[i - 1].Clone();

					placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5));
					placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

					// Build ComponentProfileXRef

					SetProfileXRef(new ComponentProfileXRef(placeholderCubeType, subTotPlcCompXRef[i - 1]));

					// Define cube

					placeholderCube = new AssortmentComponentPlaceholderTotalSubTotal(
						SAB,
						Transaction,
						this,
						new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
						7 * (i + 1),
						(Transaction.DataState == eDataState.ReadOnly),
						false,
						placeholderCubeType,
						new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, i - 1));

					AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
					SetCube(placeholderCubeType, placeholderCube);

					// Get Related Cubes

					gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, i);
					gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);
					groupLevelSubTotalCube = (AssortmentCube)GetCube(groupLevelSubTotalCubeType);

					// Define Relationships

					placeholderCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					gradeSubTotalCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					placeholderCube.AddRelationship(new CubeRelationship(groupLevelSubTotalCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					groupLevelSubTotalCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
						new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

					placeholderCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

					placeholderCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					lastHeaderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
						new CubeComponentRelationshipItem(
							(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
							(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

					placeholderCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, i),
						new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

					// Connect Cubes

					placeholderCube.SetComponentDetailCube(lastPlaceholderDetailCubeType);
                    //Begin TT#2 - JScott - Assortment Planning - Phase 2
                    //placeholderCube.SetSpreadDetailCube(groupLevelSubTotalCubeType);
                    placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);
                    //End TT#2 - JScott - Assortment Planning - Phase 2

					gradeSubTotalCube.AddTotalCube(placeholderCubeType);
					groupLevelSubTotalCube.AddTotalCube(placeholderCubeType);
					lastPlaceholderDetailCube.AddTotalCube(placeholderCubeType);

					// Set variables for next cubes

					lastHeaderDetailTypeList = headerTypeList;
					lastHeaderDetailCubeType = headerCubeType;
					lastHeaderDetailCube = headerCube;

					lastPlaceholderDetailTypeList = placeholderTypeList;
					lastPlaceholderDetailCubeType = placeholderCubeType;
					lastPlaceholderDetailCube = placeholderCube;
				}

				//===================
				//===================
				// TOTAL TOTALS CUBES
				//===================
				//===================

				//===============================================================
				// Define Grand Total AssortmentComponentHeaderTotalSubTotal cube
				//===============================================================

				// Build cube parameters

				headerCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, 0);
				headerDimList = new ArrayList();
				headerTypeList = new ArrayList();

				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5));
				headerDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Define cube

				headerCube = new AssortmentComponentHeaderTotalSubTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])headerDimList.ToArray(typeof(DimensionDefinition))),
					7,
					(Transaction.DataState == eDataState.ReadOnly),
					false,
					headerCubeType,
					eCubeType.None);

				AssortmentComputations.VariableInitializations.InitializeVariables(headerCube);
				SetCube(headerCubeType, headerCube);

				// Get Related Cubes

				gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, 0);
				gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);
				groupLevelSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentHeaderGroupLevelSubTotal, 0);
				groupLevelSubTotalCube = (AssortmentCube)GetCube(groupLevelSubTotalCubeType);

				// Define Relationships

				headerCube.AddRelationship(new CubeRelationship(groupLevelSubTotalCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				groupLevelSubTotalCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				gradeSubTotalCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				headerCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastHeaderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				headerCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(headerCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])headerTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				headerCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0),
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				headerCube.SetComponentDetailCube(lastHeaderDetailCubeType);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //headerCube.SetSpreadDetailCube(groupLevelSubTotalCubeType);
                headerCube.SetSpreadDetailCube(lastHeaderDetailCubeType);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				gradeSubTotalCube.AddTotalCube(headerCubeType);
				groupLevelSubTotalCube.AddTotalCube(headerCubeType);
				lastHeaderDetailCube.AddTotalCube(headerCubeType);
				headerCube.AddTotalCube(new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0));

				//====================================================================
				// Define Grand Total AssortmentComponentPlaceholderTotalSubTotal cube
				//====================================================================

				// Build cube parameters

				placeholderCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderTotalSubTotal, 0);
				placeholderDimList = new ArrayList();
				placeholderTypeList = new ArrayList();

				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentTotalVariable, 5));
				placeholderDimList.Add(new DimensionDefinition(eProfileType.AssortmentQuantityVariable, 5));

				// Define cube

				placeholderCube = new AssortmentComponentPlaceholderTotalSubTotal(
					SAB,
					Transaction,
					this,
					new CubeDefinition((DimensionDefinition[])placeholderDimList.ToArray(typeof(DimensionDefinition))),
					7,
					(Transaction.DataState == eDataState.ReadOnly),
					false,
					placeholderCubeType,
					eCubeType.None);

				AssortmentComputations.VariableInitializations.InitializeVariables(placeholderCube);
				SetCube(placeholderCubeType, placeholderCube);

				// Get Related Cubes

				gradeSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGradeSubTotal, 0);
				gradeSubTotalCube = (AssortmentCube)GetCube(gradeSubTotalCubeType);
				groupLevelSubTotalCubeType = new eCubeType(eCubeType.cAssortmentComponentPlaceholderGroupLevelSubTotal, 0);
				groupLevelSubTotalCube = (AssortmentCube)GetCube(groupLevelSubTotalCubeType);

				// Define Relationships

				placeholderCube.AddRelationship(new CubeRelationship(groupLevelSubTotalCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				groupLevelSubTotalCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				placeholderCube.AddRelationship(new CubeRelationship(gradeSubTotalCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				gradeSubTotalCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGroupLevel, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.None, eProfileType.StoreGrade, eProfileListType.Master),
					new CubeSingleRelationshipItem(eProfileType.AssortmentTotalVariable, eProfileType.AssortmentDetailVariable, eProfileListType.Master)));

				placeholderCube.AddRelationship(new CubeRelationship(lastPlaceholderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastPlaceholderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastPlaceholderDetailTypeList.ToArray(typeof(eProfileType)))));

				placeholderCube.AddRelationship(new CubeRelationship(lastHeaderDetailCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				lastHeaderDetailCube.AddRelationship(new CubeRelationship(placeholderCubeType,
					new CubeComponentRelationshipItem(
						(eProfileType[])placeholderTypeList.ToArray(typeof(eProfileType)),
						(eProfileType[])lastHeaderDetailTypeList.ToArray(typeof(eProfileType)))));

				placeholderCube.AddRelationship(new CubeRelationship(new eCubeType(eCubeType.cAssortmentComponentHeaderGradeSubTotal, 0),
					new CubePlaceholderHeaderRelationshipItem(_packColorXRef)));

				// Connect Cubes

				placeholderCube.SetComponentDetailCube(lastPlaceholderDetailCubeType);
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                //placeholderCube.SetSpreadDetailCube(groupLevelSubTotalCubeType);
                placeholderCube.SetSpreadDetailCube(lastPlaceholderDetailCubeType);
                //End TT#2 - JScott - Assortment Planning - Phase 2

				gradeSubTotalCube.AddTotalCube(placeholderCubeType);
				groupLevelSubTotalCube.AddTotalCube(placeholderCubeType);
				lastPlaceholderDetailCube.AddTotalCube(placeholderCubeType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
		// Begin TT#1954-MD - JSmith - Assortment
		//public void ComponentsChanged(SortedList aSortedComponentColumnHeaders)
		public void ComponentsChanged(SortedList aSortedComponentColumnHeaders, bool reloadHeaders = true)
		// End TT#1954-MD - JSmith - Assortment
		{
			try
			{
                // Begin TT#1954-MD - JSmith - Assortment
				if (reloadHeaders)
                {
				// End TT#1954-MD - JSmith - Assortment
                    _dtComponents = null;
                    _dtHeaderComponents = null;
                    _dtPlaceholderComponents = null;
                    _headerList = null;
				// Begin TT#1954-MD - JSmith - Assortment
                }
				// End TT#1954-MD - JSmith - Assortment

				BuildComponentProfileXRef();
				DefineTotalCubes(aSortedComponentColumnHeaders);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
        public void ReadData(bool reload)
        {
            ReadData(reload, true);
        }
		// End TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
		
		//End TT#1173 - JScott - Add Total % to the view as the first column on a new Assortment receives Inavlid total relationship error.
		// BEGIN TT#2 - stodd
		public void ReadData()
		{
			ReadData(false, true);	// TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
		}

		/// <summary>
		/// Method that reads the values for the detail cubes from the database.
		/// </summary>

		public void ReadData(bool reload, bool reloadBlockedCells)	// TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
		{
			try
			{
				//==============================
				// Load Cube with initial values
				//==============================

				((AssortmentHeaderColorDetail)GetCube(eCubeType.AssortmentHeaderColorDetail)).ReadAndLoadCube(reload);
				((AssortmentPlaceholderColorDetail)GetCube(eCubeType.AssortmentPlaceholderColorDetail)).ReadAndLoadCube(reload);

				//===========================
				// Read Blocked Placheholders
				//===========================

				// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
                // If we are in the midst of the blocked cells changing, we don't want to 
                // re-read the blocked cells from the DB. That's what this switch controls.
                if (reloadBlockedCells)
                {
                    intReadBlockedPlaceholders();
                    intReadBlockedHeaders();
                }
				// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#2 - stodd

		// BEGIN TT#1441 - stodd - need won't process
		public void ReapplyBlockedPlaceholders()
		{
			try
			{
				//===========================
				// Read Blocked Placheholders
				//===========================
				intReadBlockedPlaceholders();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#1441 - stodd - need won't process

        public void ReapplyBlockedHeaders()
        {
            try
            {
                //===========================
                // Read Blocked Headers
                //===========================
                intReadBlockedHeaders();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		// Begin TT#705 - MD - stodd - total not spread error
		public void ReReadHeaders()
		{
			ReReadHeaders(false);
		}
		// END TT#705 - MD - stodd - total not spread error

		// BEGIN TT#2 - stodd
		public void ReReadHeaders(bool withStores)
		{
			AllocationProfile alocProf;
			foreach (AllocationHeaderProfile hdrProf in GetHeaderList())
			{
				alocProf = _transaction.GetAssortmentMemberProfile(hdrProf.Key);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                //BEGIN TT#883 - MD - DOConnell - General Allocation - Unable to delete a placeholder from an assortment
				if (alocProf != null)
                {
                    // Begin TT#705 - MD - stodd - total not spread error
                    if (withStores)
                    {
                        alocProf.ReReadHeaderWithStores();
                    }
                    else
                    {
                        alocProf.ReReadHeader();
                    }
                    // END TT#705 - MD - stodd - total not spread error
                }
				//END TT#883 - MD - DOConnell - General Allocation - Unable to delete a placeholder from an assortment
			}
			//_defaultAlocProfileInited = false;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
		}
		// END TT#2 - stodd

		/// <summary>
		/// Method that causes the data in the cube group to be refreshed from the database.
		/// </summary>

		public void Refresh()
		{
			IDictionaryEnumerator iDictEnum;
			Cube cube;

			try
			{
				iDictEnum = _cubeTable.GetEnumerator();

				while (iDictEnum.MoveNext())
				{
					cube = (Cube)iDictEnum.Value;
					cube.Clear();
				}
                _dtComponents = null;   // TT#2 - Ron Matelic - Assortment Planning                
				ReadData();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        public void NullHeaderObjects()
        {
            _dtComponents = null;
            //_headerList = null;
            //_assortmentList = null;
        }

        // Begin TT#2065-MD - JSmith - Total Units when changing Store Attributes is not consistent
        public void CopyBlockedList(int aFromKey, int aToKey)
        {
            Hashtable blockedHash = (Hashtable)BlockedList.Clone();
            BlockedListHashKey blockedKey;
            IDictionaryEnumerator iEnum;
            iEnum = blockedHash.GetEnumerator();

            while (iEnum.MoveNext())
            {
                blockedKey = (BlockedListHashKey)iEnum.Key;
                if (blockedKey.PlaceholderRID == aFromKey)
                {
                    AddPlaceholderToBlockedList(aToKey, blockedKey.StrGrpLvlRID, blockedKey.GradeRID);
                }
            }
        }
        // End TT#2065-MD - JSmith - Total Units when changing Store Attributes is not consistent
    
		/// <summary>
		/// Adds a Placeholder key, StoreGroupLevel key, and Grade key to the Blocked list.
		/// </summary>
		/// <param name="aPlaceholder">
		/// The Style key to add.
		/// </param>
		/// <param name="aStrGrpLvl">
		/// The StoreGroupLevel key to add.
		/// </param>
		/// <param name="aGrade">
		/// The Grade key to add.
		/// </param>

		public void AddPlaceholderToBlockedList(int aPlaceholder, int aStrGrpLvl, int aGrade)
		{
			BlockedListHashKey blockedObj;

			try
			{
				blockedObj = new BlockedListHashKey(aPlaceholder, aStrGrpLvl, aGrade);
				BlockedList[blockedObj] = blockedObj;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Removes a Placeholder key, StoreGroupLevel key, and Grade key from the Blocked list.
		/// </summary>
		/// <param name="aPlaceholder">
		/// The Style key to remove.
		/// </param>
		/// <param name="aStrGrpLvl">
		/// The StoreGroupLevel key to remove.
		/// </param>
		/// <param name="aGrade">
		/// The Grade key to remove.
		/// </param>

		public void RemovePlaceholderFromBlockedList(int aPlaceholder, int aStrGrpLvl, int aGrade)
		{
			BlockedListHashKey blockedObj;

			try
			{
				blockedObj = new BlockedListHashKey(aPlaceholder, aStrGrpLvl, aGrade);

				if (BlockedList.Contains(blockedObj))
				{
					BlockedList.Remove(blockedObj);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Checks to see if a Placeholder key, StoreGroupLevel key, and Grade key are on the Blocked list.
		/// </summary>
        /// <param name="aPlaceholder">
		/// The place holder key to check.
		/// </param>
		/// <param name="aGrade">
		/// The Grade key to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the Sytle key and Grade key are on the Blocked list.
		/// </returns>

		public bool isPlaceholderBlocked(int aPlaceholder, int aStrGrpLvl, int aGrade)
		{
			try
			{
				return BlockedList.Contains(new BlockedListHashKey(aPlaceholder, aStrGrpLvl, aGrade));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#1205-MD - stodd - Lock graphic on Matrix Grid disappears after running an action- 
        public bool isHeaderLocked(int aHdrRid, int aPackRid, int aColorRid, int storeGroupLevelRid, int grade)
        {
            bool isLocked = false;
            try
            {
                AllocationProfile ap = _transaction.GetAssortmentMemberProfile(aHdrRid);
                if (ap != null)
                {
                    ProfileList storeList = GetStoreList(ap, storeGroupLevelRid, grade);

                    if (storeList.Count > 0)
                    {
                        if (aPackRid != int.MaxValue)
                        {
                            if (aColorRid != int.MaxValue)
                            {
                                // pack and color
                                // Can't do Pack / Color
                                PackHdr aPack = ap.GetPackHdr(aPackRid);
                                isLocked = ap.GetStoreListTotalLocked(aPack, storeList);
                            }
                            else
                            {
                                // Pack
                                PackHdr aPack = ap.GetPackHdr(aPackRid);
                                isLocked = ap.GetStoreListTotalLocked(aPack, storeList);
                            }
                        }
                        else
                        {
                            if (aColorRid != int.MaxValue)
                            {
                                // color
                                isLocked = ap.GetStoreListTotalLocked(aColorRid, storeList);
                            }
                            else
                            {
                                // no pack, no color
                                isLocked = ap.GetStoreListTotalLocked(eAllocationSummaryNode.Total, storeList);
                            }
                        }
                    }
                }

                return isLocked;
                //return BlockedList.Contains(new BlockedListHashKey(aPlaceholder, aStrGrpLvl, aGrade));
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		// Begin TT#1204-MD - stodd - Provide a message within Matrix when there are no stores available to spread to -
        /// <summary>
        /// Returns if cell is fixed and cannot be changed due to all stores being outted by rules.
        /// </summary>
        /// <param name="aHdrRid"></param>
        /// <param name="aPackRid"></param>
        /// <param name="aColorRid"></param>
        /// <param name="storeGroupLevelRid"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public bool isCellFixed(int aHdrRid, int aPackRid, int aColorRid, int storeGroupLevelRid, int grade)
        {
            bool isFixed = false;
            try
            {
                AllocationProfile ap = _transaction.GetAssortmentMemberProfile(aHdrRid);
                if (ap != null)
                {
                    ProfileList storeList = GetStoreList(ap, storeGroupLevelRid, grade);

                    if (storeList.Count > 0)
                    {
                        if (aPackRid != int.MaxValue)
                        {
                            if (aColorRid != int.MaxValue)
                            {
                                // pack and color
                                // Can't do Pack / Color
                                PackHdr aPack = ap.GetPackHdr(aPackRid);
                                isFixed = ap.GetStoreListTotalOut(aPack, storeList);
                            }
                            else
                            {
                                // Pack
                                PackHdr aPack = ap.GetPackHdr(aPackRid);
                                isFixed = ap.GetStoreListTotalOut(aPack, storeList);
                            }
                        }
                        else
                        {
                            if (aColorRid != int.MaxValue)
                            {
                                // color
                                isFixed = ap.GetStoreListTotalOut(aColorRid, storeList);
                            }
                            else
                            {
                                // no pack, no color
                                isFixed = ap.GetStoreListTotalOut(eAllocationSummaryNode.Total, storeList);
                            }
                        }
                    }
                }

                return isFixed;
                //return BlockedList.Contains(new BlockedListHashKey(aPlaceholder, aStrGrpLvl, aGrade));
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1204-MD - stodd - Provide a message within Matrix when there are no stores available to spread to -

        private ProfileList GetStoreList(AllocationProfile ap, int storeGroupLevelRid, int grade)
        {
            ProfileList storeList = null;
			// Begin TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again
            if (_storesBySetAndGrade == null)
            {
                _storesBySetAndGrade = new Dictionary<HashSet<int>, ProfileList>(HashSet<int>.CreateSetComparer());
            }

            HashSet<int> groupGradeHash = new HashSet<int>();
            groupGradeHash.Add(storeGroupLevelRid);
            groupGradeHash.Add(grade);
			// End TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again
           
            try
            {
				// Begin TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again
                if (_storesBySetAndGrade.ContainsKey(groupGradeHash))
                {
                    storeList = _storesBySetAndGrade[groupGradeHash];
                    return storeList;
                }
                else
                {
				// End TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again
                    if (storeGroupLevelRid != Include.NoRID)
                    {
                        if (grade != -1)
                        {
                            // Set / Grade
                            storeList = GetStoresInSetGrade(storeGroupLevelRid, grade);
                        }
                        else
                        {
                            // Set
                            storeList = GetStoresInSet(storeGroupLevelRid);
                        }
                    }
                    else
                    {
                        // All stores
                        storeList = GetAllStores();
                    }

                    _storesBySetAndGrade.Add(groupGradeHash, storeList);	// TT#1553-MD - stodd - Add store list cache to assortment to preclude looking up stores in sets and grades over and over again

                    return storeList;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1205-MD - stodd - Lock graphic on Matrix Grid disappears after running an action- 
		
		/// <summary>
		/// Returns a boolean indicating the if Header referred to by the given Header RID is allocated or not.
		/// </summary>
		/// <param name="aHeaderRID">
		/// The Header RID to check.
		/// </param>
		/// <returns>
		/// A boolean indicating the if Header referred to by the given Header RID is allocated or not.
		/// </returns>

		public bool useHeaderAllocation(int aHeaderRID)
		{
			object hashEntry;
			eHeaderAllocationStatus hdrStatus;

			try
			{
				if (WindowType == eAssortmentWindowType.Assortment)
				{
					if (aHeaderRID > 0 && aHeaderRID != int.MaxValue)
					{
						hashEntry = _headerHash[aHeaderRID];

						if (hashEntry == null)
						{
							// BEGIN TT#771-MD - Stodd - null reference exception
							//hashEntry = Transaction.GetAllocationProfile(aHeaderRID).HeaderAllocationStatus;
							hashEntry = Transaction.GetAssortmentMemberProfile(aHeaderRID).HeaderAllocationStatus;
							// END TT#771-MD - Stodd - null reference exception
							_headerHash[aHeaderRID] = hashEntry;
						}

						hdrStatus = (eHeaderAllocationStatus)hashEntry;

						switch (hdrStatus)
						{
							case eHeaderAllocationStatus.ReceivedInBalance:
							case eHeaderAllocationStatus.ReceivedOutOfBalance:

								return false;

							default:

								return true;
						}
					}
					else
					{
						return false;
					}
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the block status of a cell specified by the given CubeWaferCoordinateLists.
		/// </summary>
		/// <param name="aCommonWaferList">
		/// A CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// A CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// A CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <param name="aBlockStatus">
		/// A boolean indicating if the Cell should be marked blocked or unblocked.
		/// </param>

		public void SetCellBlockStatus(CubeWaferCoordinateList aCommonWaferList, CubeWaferCoordinateList aRowWaferList, CubeWaferCoordinateList aColWaferList, bool aBlockStatus)
		{
			AssortmentCellReference asrtCellRef;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			ComputationCubeGroupWaferInfo rowWaferInfo;
			ComputationCubeGroupWaferInfo colWaferInfo;
			eCubeType cubeType;
			ComponentProfileXRef compXRef;
			AssortmentCellReference dtlPlcCellRef;
			AssortmentCellReference dtlHdrCellRef;
            ArrayList cellRefList = null;   // TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error. 
			//int placeholderRid = Include.NoRID;	// TT#1322 - stodd  *REMOVED*

			try
			{
				if (aCommonWaferList != null)
				{
					globalWaferInfo = CreateWaferInfo(aCommonWaferList);

					if (aRowWaferList != null)
					{
						rowWaferInfo = CreateWaferInfo(aRowWaferList);

						if (aColWaferList != null)
						{
							colWaferInfo = CreateWaferInfo(aColWaferList);
							cubeType = DetermineCubeType(globalWaferInfo, rowWaferInfo, colWaferInfo);
							asrtCellRef = (AssortmentCellReference)ConvertCubeWaferInfoToCellReference(
								cubeType,
								aCommonWaferList,
								aRowWaferList,
								aColWaferList,
								globalWaferInfo,
								rowWaferInfo,
								colWaferInfo);

							if (asrtCellRef != null && asrtCellRef.isCellValid && !asrtCellRef.isCellNull && !asrtCellRef.isCellHidden)
							{
								if (asrtCellRef.isCellReadOnly)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsReadOnly,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsReadOnly));
								}
								else if (asrtCellRef.isCellDisplayOnly)
								{
									throw new MIDException(eErrorLevel.severe,
										(int)eMIDTextCode.msg_pl_CellIsDisplayOnly,
										MIDText.GetText(eMIDTextCode.msg_pl_CellIsDisplayOnly));
								}
                                asrtCellRef.SetEntryCellValue(0);	// TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 

								BlockRetotalHash.Clear();
								BlockRetotalQueue.Clear();
								//Begin Enhancement - JScott - Add Balance Low Levels functionality
								//intGetCurrUndoList();
								CreateUndoRestorePoint();
								//End Enhancement - JScott - Add Balance Low Levels functionality

								// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                bool styleOnHeader = false;
                                if (asrtCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader) > -1
                                        && asrtCellRef[eProfileType.AllocationHeader] != int.MaxValue)
                                {
                                    compXRef = (ComponentProfileXRef)GetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentHeaderColorDetail));

                                    dtlPlcCellRef = (AssortmentCellReference)GetCube(eCubeType.AssortmentHeaderColorDetail).CreateCellReference(asrtCellRef);
                                    dtlPlcCellRef[eProfileType.HeaderPack] = Include.NoRID;
                                    dtlPlcCellRef[eProfileType.HeaderPackColor] = Include.NoRID;
                                    dtlPlcCellRef[eProfileType.AssortmentDetailVariable] = Include.NoRID;
                                    dtlPlcCellRef[eProfileType.AssortmentQuantityVariable] = Include.NoRID;

                                    cellRefList = compXRef.GetTotalList(dtlPlcCellRef);
                                    styleOnHeader = true;
                                }
                                else
                                {
                                    compXRef = (ComponentProfileXRef)GetProfileXRef(new ComponentProfileXRef(eCubeType.AssortmentPlaceholderColorDetail));

                                    dtlPlcCellRef = (AssortmentCellReference)GetCube(eCubeType.AssortmentPlaceholderColorDetail).CreateCellReference(asrtCellRef);
                                    dtlPlcCellRef[eProfileType.HeaderPack] = Include.NoRID;
                                    dtlPlcCellRef[eProfileType.HeaderPackColor] = Include.NoRID;
                                    dtlPlcCellRef[eProfileType.AssortmentDetailVariable] = Include.NoRID;
                                    dtlPlcCellRef[eProfileType.AssortmentQuantityVariable] = Include.NoRID;

                                    cellRefList = compXRef.GetTotalList(dtlPlcCellRef);
                                }
								// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                

								dtlPlcCellRef = (AssortmentCellReference)GetCube(eCubeType.AssortmentPlaceholderColorDetail).CreateCellReference(asrtCellRef);
								dtlHdrCellRef = (AssortmentCellReference)GetCube(eCubeType.AssortmentHeaderColorDetail).CreateCellReference(asrtCellRef);
                                // Begin TT#802-MD - RMatelic - Matrix tab - right click and close row received an invalid cast exception 
								//foreach (Hashtable profKeyHash in plcCellRefList)
                                foreach (Dictionary<eProfileType, int> profKeyHash in cellRefList)	// TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                // End TT#802-MD
								{
									if ((int)profKeyHash[eProfileType.AllocationHeader] == int.MaxValue)
									{
										dtlPlcCellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
										dtlPlcCellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
										// Begin TT#1227 - stodd - sorting  *REMOVED*
										//dtlPlcCellRef[eProfileType.PlaceholderHeader] = (int)profKeyHash[eProfileType.PlaceholderHeader];
										//placeholderRid = (int)profKeyHash[eProfileType.PlaceholderHeader];
										// End TT#1227 - stodd - sorting
										foreach (AssortmentDetailVariableProfile varProf in AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
										{
											if (varProf.isDatabaseVariable(eVariableCategory.Store, -1, eCalendarDateType.Week))
											{
												dtlPlcCellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;
												dtlPlcCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
												//Begin TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
                                                if (dtlPlcCellRef[eProfileType.StoreGrade] != Include.NoRID)
                                                {
												//End TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
                                                    if (dtlPlcCellRef.doesCellExist)
                                                    {
                                                        if (dtlPlcCellRef.isCellLocked)
                                                        {
                                                            intSetRecursiveCellLock(dtlPlcCellRef, false);
                                                        }
                                                        if (dtlPlcCellRef.CurrentCellValue != 0)
                                                        {
                                                            dtlPlcCellRef.SetEntryCellValue(0);
                                                        }
												//Begin TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
                                                    }
                                                }
                                                else
                                                {
                                                    break;
												//End TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
                                                }
											}
										}
									}
									else
									{
										dtlHdrCellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
										dtlHdrCellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
										dtlHdrCellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
										// Begin TT#1227 - stodd - sorting *REMOVED*
										//dtlPlcCellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
										// End TT#1227 - stodd - sorting

										foreach (AssortmentDetailVariableProfile varProf in AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
										{
											if (varProf.isDatabaseVariable(eVariableCategory.Store, -1, eCalendarDateType.Week))
											{
												dtlHdrCellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;
												dtlHdrCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

												// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                                if (dtlPlcCellRef[eProfileType.StoreGrade] != Include.NoRID)
                                                {
												// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                                    if (dtlHdrCellRef.doesCellExist)
                                                    {
                                                        if (dtlHdrCellRef.isCellLocked)
                                                        {
                                                            intSetRecursiveCellLock(dtlHdrCellRef, false);
                                                        }
                                                        if (dtlHdrCellRef.CurrentCellValue != 0)
                                                        {
                                                            dtlHdrCellRef.SetEntryCellValue(0);
                                                        }
                                                    }
                                                }
												// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                                else
                                                {
                                                    break;
                                                }
												// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
											}
										}
									}
								}

								try
								{
									intProcessTotalCellLockSum();
                                    RecomputeCubes(false);

                                    // Begin TT#802-MD - RMatelic - Matrix tab - right click and close row received an invalid cast exception 
                                    //foreach (Hashtable profKeyHash in plcCellRefList)
                                    foreach (Dictionary<eProfileType, int> profKeyHash in cellRefList)	// TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                    // End TT#802-MD
									{
										if ((int)profKeyHash[eProfileType.AllocationHeader] == int.MaxValue)
										{
											dtlPlcCellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
											dtlPlcCellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];

											foreach (AssortmentDetailVariableProfile varProf in AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
											{
												dtlPlcCellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

												foreach (QuantityVariableProfile qvarProf in AssortmentComputations.AssortmentQuantityVariables.VariableProfileList)
												{
													dtlPlcCellRef[eProfileType.AssortmentQuantityVariable] = qvarProf.Key;
													//Begin TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception													
                                                    if (dtlPlcCellRef[eProfileType.StoreGrade] != Include.NoRID)
                                                    {
                                                        if (dtlPlcCellRef.doesCellExist)
                                                        {
                                                            intSetRecursiveCellBlock(dtlPlcCellRef, aBlockStatus);
															// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
                                                            if (dtlPlcCellRef.CurrentCellValue != 0)
                                                            {
                                                                dtlPlcCellRef.SetEntryCellValue(0);
                                                            }
															// End TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
                                                        }
													//End TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
                                                    }
												}
											}
										}
										else
										{
											dtlHdrCellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
											dtlHdrCellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
											dtlHdrCellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];

											foreach (AssortmentDetailVariableProfile varProf in AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
											{
												dtlHdrCellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

												foreach (QuantityVariableProfile qvarProf in AssortmentComputations.AssortmentQuantityVariables.VariableProfileList)
												{
													dtlHdrCellRef[eProfileType.AssortmentQuantityVariable] = qvarProf.Key;

													if (dtlHdrCellRef.doesCellExist)
													{
														intSetRecursiveCellBlock(dtlHdrCellRef, aBlockStatus);
														// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
                                                        if (dtlHdrCellRef.CurrentCellValue != 0)
                                                        {
                                                            dtlHdrCellRef.SetEntryCellValue(0);
                                                        }
														// End TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
													}
												}
											}
										}
									}

									intProcessTotalCellBlockSum();

									//Begin TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
                                    if (dtlPlcCellRef[eProfileType.StoreGrade] != Include.NoRID)
                                    {
										// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                        if (styleOnHeader)
                                        {
                                            // This is actually adding or removing the HEADER from the blocked list
                                            if (aBlockStatus)
                                            {
                                                AddPlaceholderToBlockedList(asrtCellRef[eProfileType.AllocationHeader], asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
                                            }
                                            else
                                            {
                                                RemovePlaceholderFromBlockedList(asrtCellRef[eProfileType.AllocationHeader], asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
                                            }
                                        }
                                        else
                                        {
                                            // adding or removing the PLACEHOLDER from the blocked list
                                            if (aBlockStatus)
                                            {
                                                AddPlaceholderToBlockedList(asrtCellRef[eProfileType.PlaceholderHeader], asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
                                            }
                                            else
                                            {
                                                RemovePlaceholderFromBlockedList(asrtCellRef[eProfileType.PlaceholderHeader], asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
                                            }
                                        }
										// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                                    }
									//End TT#1944 - DOConnell - Assortment Tab Close Row Data Common Exception
								}
								catch
								{
									SAB.MessageCallback.HandleMessage(
										MIDText.GetText(eMIDTextCode.msg_as_CannotCloseStyle),
										MIDText.GetText(eMIDTextCode.msg_as_GenericMessageBoxTitle),
										System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
								}
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
		/// Saves this AssortmenttCubeGroup to the Allocation Profile.
        /// Also updates closed styles.
		/// </summary>

		public void SaveCubeGroup()
		{
			AssortmentHeaderColorDetail headerCube;
            AssortmentComponentHeaderTotal headerReserveCube;	// TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
			AssortmentPlaceholderColorDetail placeholderCube;
			IDictionaryEnumerator iEnum;
			ArrayList placeholderList;
			BlockedListHashKey blockedKey;

			try
			{
				// Save Header data

				headerCube = (AssortmentHeaderColorDetail)GetCube(eCubeType.AssortmentHeaderColorDetail);

				if (headerCube != null)
				{
					headerCube.SaveCube(true);
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_CubeNotDefined,
						MIDText.GetText(eMIDTextCode.msg_pl_CubeNotDefined));
				}

				// Begin TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 
                // Save Header data - RESERVE

                headerReserveCube = (AssortmentComponentHeaderTotal)GetCube(eCubeType.AssortmentComponentHeaderTotal);

                if (headerReserveCube != null)
                {
                    headerReserveCube.SaveCube(true);
                    //headerReserveCube.ReadAndLoadCube(true);
                }
                else
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_pl_CubeNotDefined,
                        MIDText.GetText(eMIDTextCode.msg_pl_CubeNotDefined));
                }
				// End TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units - 

				// Save Placeholder data

				placeholderCube = (AssortmentPlaceholderColorDetail)GetCube(eCubeType.AssortmentPlaceholderColorDetail);

				if (placeholderCube != null)
				{
					placeholderCube.SaveCube(true);
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_CubeNotDefined,
						MIDText.GetText(eMIDTextCode.msg_pl_CubeNotDefined));
				}

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
        public void SaveBlockedStyles()
        {
            // Save Blocked Placeholders
            IDictionaryEnumerator iEnum;
            ArrayList placeholderList;
            BlockedListHashKey blockedKey;

            AssortmentDetailData.OpenUpdateConnection();

            try
            {
                placeholderList = new ArrayList();

                foreach (AllocationHeaderProfile hdrProf in GetPlaceholderList())
                {
                    placeholderList.Add(hdrProf.Key);
                }

                // Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
                // Also add HEADERS
                foreach (AllocationHeaderProfile hdrProf in GetHeaderList())
                {
                    placeholderList.Add(hdrProf.Key);
                }
                // End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.

                AssortmentDetailData.AssortmentStyleClosed_Delete(placeholderList);

                iEnum = BlockedList.GetEnumerator();

                while (iEnum.MoveNext())
                {
                    blockedKey = (BlockedListHashKey)iEnum.Key;
                    AssortmentDetailData.AssortmentStyleClosed_InsertClosed(blockedKey.PlaceholderRID, blockedKey.StrGrpLvlRID, blockedKey.GradeRID);
                }

                AssortmentDetailData.CommitData();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                if (AssortmentDetailData.ConnectionIsOpen)
                {
                    AssortmentDetailData.CloseUpdateConnection();
                }
            }

        }
		// End TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
		
		/// <summary>
		/// Closes this PlanCubeGroup.
		/// </summary>

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		//public void CloseCubeGroup()
		override public void CloseCubeGroup()
		//End Enhancement - JScott - Add Balance Low Levels functionality
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

		/// <summary>
		/// Determines if changes have occurred to the header cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the header cube has changed.
		/// </returns>

		public bool hasHeaderCubeChanged()
		{
			AssortmentHeaderColorDetail cube;

			try
			{
				cube = (AssortmentHeaderColorDetail)GetCube(eCubeType.AssortmentHeaderColorDetail);

				if (cube != null)
				{
					return cube.hasCubeChanged();
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if changes have occurred to the placeholder cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the placeholder cube has changed.
		/// </returns>

		public bool hasPlaceholderCubeChanged()
		{
			AssortmentPlaceholderColorDetail cube;

			try
			{
				cube = (AssortmentPlaceholderColorDetail)GetCube(eCubeType.AssortmentPlaceholderColorDetail);

				if (cube != null)
				{
					return cube.hasCubeChanged();
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets a 2-dimensional array of values contained in the requested AssortmentWafer.
		/// </summary>
		/// <remarks>
		/// GetAssortmentCellValues allows the user to retrieve arrays of cell values.  This is a much more efficient means of retrieving values,
		/// as it reduces the number of remoted calls required to retrive large sets of data.
		/// </remarks>
		/// <param name="aAssortmentWafer">
		/// The AssortmentWafer indicating which cells are to be returned.
		/// </param>
		/// <returns>
		/// A double array of values contained in the Cells requested in the AssortmentWafer.
		/// </returns>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//public AssortmentWaferCell[,] GetAssortmentWaferCellValues(CubeWafer aAssortmentWafer)
		public WaferCell[,] GetAssortmentWaferCellValues(CubeWafer aAssortmentWafer)
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			int maxRows, maxCols;
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			//AssortmentWaferCell[,] cellTable;
			WaferCell[,] cellTable;
			//End TT#2 - JScott - Assortment Planning - Phase 2
			ComputationCubeGroupWaferInfo[] rowWaferInfo;
			ComputationCubeGroupWaferInfo[] colWaferInfo;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			int i, j;

			try
			{
				maxRows = System.Math.Max(aAssortmentWafer.RowWaferCoordinateListGroup.Count, 1);
				maxCols = System.Math.Max(aAssortmentWafer.ColWaferCoordinateListGroup.Count, 1);

				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				//cellTable = new AssortmentWaferCell[maxRows, maxCols];
				cellTable = new WaferCell[maxRows, maxCols];
				//End TT#2 - JScott - Assortment Planning - Phase 2

				if (aAssortmentWafer.CommonWaferCoordinateList != null)
				{
					rowWaferInfo = new AssortmentCubeGroupWaferInfo[maxRows];
					colWaferInfo = new AssortmentCubeGroupWaferInfo[maxCols];

					globalWaferInfo = CreateWaferInfo(aAssortmentWafer.CommonWaferCoordinateList);

					for (i = 0; i < maxRows; i++)
					{
						rowWaferInfo[i] = CreateWaferInfo(aAssortmentWafer.RowWaferCoordinateListGroup[i]);
					}

					for (i = 0; i < maxCols; i++)
					{
						colWaferInfo[i] = CreateWaferInfo(aAssortmentWafer.ColWaferCoordinateListGroup[i]);
					}

					for (i = 0; i < maxRows; i++)
					{
						if (i < aAssortmentWafer.RowWaferCoordinateListGroup.Count && aAssortmentWafer.RowWaferCoordinateListGroup[i] != null)
						{
							for (j = 0; j < maxCols; j++)
							{
								if (j < aAssortmentWafer.ColWaferCoordinateListGroup.Count && aAssortmentWafer.ColWaferCoordinateListGroup[j] != null)
								{
									//Debug.WriteLine("ROW " + i + " COL " + j);
									cellTable[i, j] = intGetCellValue(
										aAssortmentWafer.CommonWaferCoordinateList,
										aAssortmentWafer.RowWaferCoordinateListGroup[i],
										aAssortmentWafer.ColWaferCoordinateListGroup[j],
										(AssortmentCubeGroupWaferInfo)globalWaferInfo,
										(AssortmentCubeGroupWaferInfo)rowWaferInfo[i],
										(AssortmentCubeGroupWaferInfo)colWaferInfo[j]);
								}
							}
						}
					}
				}

				return cellTable;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        /// <summary>
        /// Rebuilds the BlockedList from the database for Placeholders.
        /// </summary>
		private void intReadBlockedPlaceholders()
		{
			ArrayList placeholderList;
			DataTable dtBlocked;
			int plchldrRID;
			int strgrplvlRID;
			int gradeRID;

			try
			{
				placeholderList = new ArrayList();

				foreach (AllocationHeaderProfile hdrProf in GetPlaceholderList())
				{
					placeholderList.Add(hdrProf.Key);
				}

				dtBlocked = AssortmentDetailData.AssortmentStyleClosed_Read(placeholderList);

				foreach (DataRow row in dtBlocked.Rows)
				{
					plchldrRID = Convert.ToInt32(row["HDR_RID"]);
					strgrplvlRID = Convert.ToInt32(row["SGL_RID"]);
					gradeRID = Convert.ToInt32(row["GRADE"]);

					if (Convert.ToChar(row["CLOSED"]) == '1')
					{
						AddPlaceholderToBlockedList(plchldrRID, strgrplvlRID, gradeRID);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
        /// <summary>
        /// Rebuilds the BlockedList from the database for Headers.
        /// </summary>
        private void intReadBlockedHeaders()
        {
            ArrayList headerList;
            DataTable dtBlocked;
            int hdrchldrRID;
            int strgrplvlRID;
            int gradeRID;

            try
            {
                headerList = new ArrayList();

                foreach (AllocationHeaderProfile hdrProf in GetHeaderList())
                {
                    headerList.Add(hdrProf.Key);
                }

                dtBlocked = AssortmentDetailData.AssortmentStyleClosed_Read(headerList);

                foreach (DataRow row in dtBlocked.Rows)
                {
                    hdrchldrRID = Convert.ToInt32(row["HDR_RID"]);
                    strgrplvlRID = Convert.ToInt32(row["SGL_RID"]);
                    gradeRID = Convert.ToInt32(row["GRADE"]);

                    if (Convert.ToChar(row["CLOSED"]) == '1')
                    {
                        // Actually Adds the 
                        AddPlaceholderToBlockedList(hdrchldrRID, strgrplvlRID, gradeRID);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error.
        
        // Begin TT#1737 - RMatelic - Assortment Contents-Delete Placeholder, get Foreign Key violation
        public void RemoveDeletedHeaderFromCubeGroup(int headerRID)
        {
            try
			{
                AllocationHeaderProfile ahp;
                if (_placeholderList.Contains(headerRID))
                {
                    ahp = (AllocationHeaderProfile)_placeholderList.FindKey(headerRID);
                    _placeholderList.Remove(ahp);
                }
                else if (_receiptList.Contains(headerRID))
                {
                    ahp = (AllocationHeaderProfile)_receiptList.FindKey(headerRID);
                    _receiptList.Remove(ahp);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // End TT#1737

		/// <summary>
		/// Gets the list of headers associate with the Assortments and catagorizes them into receipt and placeholder lists.
		/// </summary>

		private void intGetHeaders()
		{
		// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
			//AllocationProfile alocProf;

			try
			{
				AllocationProfileList apl = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.AssortmentMember);

				//_headerList = (AllocationHeaderProfileList)_transaction.GetMasterProfileList(eProfileType.AllocationHeader);
				_headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
				_assortmentList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
				_placeholderList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
				_receiptList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

				//foreach (AllocationHeaderProfile hdrProf in _headerList)
				foreach (AllocationProfile alocProf in apl)
				{
					//alocProf = _transaction.GetAssortmentMemberProfile(hdrProf.Key);
					AllocationHeaderProfile hdrProf = SAB.HeaderServerSession.GetHeaderData(alocProf.Key, true, true, true);
					_headerList.Add(hdrProf);
		// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
					switch (alocProf.HeaderType)
					{
						case eHeaderType.Assortment:
							_assortmentList.Add(hdrProf);
							break;

						case eHeaderType.Placeholder:
							_placeholderList.Add(hdrProf);
							break;

						default:
							_receiptList.Add(hdrProf);
							break;
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
		/// Private method that retrieves the AssortmentWaferCell specified by the passed parameters.
		/// </summary>
		/// <param name="aGlobalCoordinates">
		/// The AssortmentWaferCoordinateList object that contains the global coordinate list for the cell.
		/// </param>
		/// <param name="aRowCoordinates">
		/// The AssortmentWaferCoordinateList object that contains the row coordinate list for the cell.
		/// </param>
		/// <param name="aColCoordinates">
		/// The AssortmentWaferCoordinateList object that contains the col coordinate list for the cell.
		/// </param>
		/// <param name="aGlobalWaferInfo">
		/// The AssortmentWaferInfo object that contains the global flags for the cell.
		/// </param>
		/// <param name="aRowWaferInfo">
		/// The AssortmentWaferInfo object that contains the row flags for the cell.
		/// </param>
		/// <param name="aColWaferInfo">
		/// The AssortmentWaferInfo object that contains the col flags for the cell.
		/// </param>
		/// <returns>
		/// A new AssortmentWaferCell.
		/// </returns>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//private AssortmentWaferCell intGetCellValue(
		private WaferCell intGetCellValue(
		//End TT#2 - JScott - Assortment Planning - Phase 2
			CubeWaferCoordinateList aGlobalCoordinates,
			CubeWaferCoordinateList aRowCoordinates,
			CubeWaferCoordinateList aColCoordinates,
			AssortmentCubeGroupWaferInfo aGlobalWaferInfo,
			AssortmentCubeGroupWaferInfo aRowWaferInfo,
			AssortmentCubeGroupWaferInfo aColWaferInfo)
		{
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			//AssortmentWaferCell waferCell;
			WaferCell waferCell;
			//End TT#2 - JScott - Assortment Planning - Phase 2
			eCubeType cubeType;
			//Begin TT#2 - JScott - Assortment Planning - Phase 2
			//AssortmentCellReference assrtCellRef;
			ComputationCellReference assrtCellRef;
			//End TT#2 - JScott - Assortment Planning - Phase 2

			try
			{
				waferCell = null;

				cubeType = DetermineCubeType(aGlobalWaferInfo, aRowWaferInfo, aColWaferInfo);

				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				//assrtCellRef = (AssortmentCellReference)ConvertCubeWaferInfoToCellReference(
				assrtCellRef = ConvertCubeWaferInfoToCellReference(
				//End TT#2 - JScott - Assortment Planning - Phase 2
					cubeType,
					aGlobalCoordinates,
					aRowCoordinates,
					aColCoordinates,
					aGlobalWaferInfo,
					aRowWaferInfo,
					aColWaferInfo);

				if (assrtCellRef != null && assrtCellRef.isCellValid)
				{
					//Begin TT#2 - JScott - Assortment Planning - Phase 2
					//waferCell = new AssortmentWaferCell(assrtCellRef, assrtCellRef.CurrentCellValue);
					if (assrtCellRef.GetType() == typeof(PlanCellReference))
					{
						waferCell = new PlanWaferFlagCell((PlanCellReference)assrtCellRef, assrtCellRef.CurrentCellValue, "1", "1", true);
					}
					else
					{
						waferCell = new AssortmentWaferCell((AssortmentCellReference)assrtCellRef, assrtCellRef.CurrentCellValue);

                        //if (assrtCellRef.GetDimensionProfileTypeIndex(eProfileType.AssortmentDetailVariable) > -1 && assrtCellRef[eProfileType.AssortmentDetailVariable] == 804650 && assrtCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGrade) > -1 && assrtCellRef[eProfileType.StoreGrade] == 200 && assrtCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader) > -1 && assrtCellRef[eProfileType.AllocationHeader] == 3756)
                        //{
                        //Debug.WriteLine("intGetCellValue()  Cube Type: " + cubeType.Id + " level: " + cubeType.Level
                        //    + " Curr Val: " + assrtCellRef.CurrentCellValue
                        //    //  + " Prev Val: " + assrtCellRef.PreviousCellValue
                        //    //   + " Hidd Val: " + assrtCellRef.HiddenCurrentCellValue
                        //    + " Init Flag: " + assrtCellRef.isCellInitialized
                        //    + " Lock Flag: " + assrtCellRef.isCellLocked
                        //    + " Block Flag: " + assrtCellRef.isCellBlocked
                        //    + "  " + ((AssortmentCellReference)assrtCellRef).CellKeys);
                        //}
					}
					//End TT#2 - JScott - Assortment Planning - Phase 2
				}

				return waferCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#101 - stodd -  no values after processing "create Placeholders"
        /// <summary>
        /// Read the AssortmentPlaceholderColorDetail cube for a specific value to help with debugging disappearing values.
        /// </summary>
        /// <returns></returns>
        public double DebugAssortmentPlaceholderColorDetail()
        {
            AssortmentPlaceholderColorDetail assrtCube = (AssortmentPlaceholderColorDetail)GetCube(eCubeType.AssortmentPlaceholderColorDetail);
            AssortmentCellReference aCellRef = (AssortmentCellReference)assrtCube.CreateCellReference();
            aCellRef[eProfileType.PlaceholderHeader] = 1186;
            //aCellRef[eProfileType.AllocationHeader] = int.MaxValue;
            aCellRef[eProfileType.AssortmentQuantityVariable] = this.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
            aCellRef[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
            aCellRef[eProfileType.HeaderPack] = int.MaxValue;
            aCellRef[eProfileType.HeaderPackColor] = int.MaxValue;
            aCellRef[eProfileType.StoreGroupLevel] = 1;
            aCellRef[eProfileType.StoreGrade] = 200;  // 264

            return aCellRef.CurrentCellValue;
        }

        public double DebugAssortmentComponentPlaceholderGrade()
        {
            AssortmentComponentPlaceholderGrade assrtCube = (AssortmentComponentPlaceholderGrade)GetCube(eCubeType.AssortmentComponentPlaceholderGrade);
            AssortmentCellReference aCellRef = (AssortmentCellReference)assrtCube.CreateCellReference();
            aCellRef[eProfileType.PlaceholderHeader] = 1186;
            //aCellRef[eProfileType.AllocationHeader] = int.MaxValue;
            aCellRef[eProfileType.AssortmentQuantityVariable] = this.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
            aCellRef[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
            //aCellRef[eProfileType.HeaderPack] = int.MaxValue;
            //aCellRef[eProfileType.HeaderPackColor] = int.MaxValue;
            aCellRef[eProfileType.StoreGroupLevel] = 1;
            aCellRef[eProfileType.StoreGrade] = 200;  // 264

            return aCellRef.CurrentCellValue;
        }

        public double DebugAssortmentPlaceholderGradeTotal()
        {
            AssortmentPlaceholderGradeTotal assrtCube = (AssortmentPlaceholderGradeTotal)GetCube(eCubeType.AssortmentPlaceholderGradeTotal);
            AssortmentCellReference aCellRef = (AssortmentCellReference)assrtCube.CreateCellReference();
            aCellRef[eProfileType.PlaceholderHeader] = 1186;
            //aCellRef[eProfileType.AllocationHeader] = int.MaxValue;
            aCellRef[eProfileType.AssortmentQuantityVariable] = this.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
            aCellRef[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
            //aCellRef[eProfileType.HeaderPack] = int.MaxValue;
            //aCellRef[eProfileType.HeaderPackColor] = int.MaxValue;
            aCellRef[eProfileType.StoreGroupLevel] = 1;
            aCellRef[eProfileType.StoreGrade] = 200;  // 264

            return aCellRef.CurrentCellValue;
        }

        public double DebugAssortmentComponentPlaceholderGroupLevel()
        {
            AssortmentComponentPlaceholderGroupLevel assrtCube = (AssortmentComponentPlaceholderGroupLevel)GetCube(eCubeType.AssortmentComponentPlaceholderGroupLevel);
            AssortmentCellReference aCellRef = (AssortmentCellReference)assrtCube.CreateCellReference();
            aCellRef[eProfileType.PlaceholderHeader] = 1186;
            //aCellRef[eProfileType.AllocationHeader] = int.MaxValue;
            aCellRef[eProfileType.AssortmentQuantityVariable] = this.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
            aCellRef[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
            //aCellRef[eProfileType.HeaderPack] = int.MaxValue;
            //aCellRef[eProfileType.HeaderPackColor] = int.MaxValue;
            aCellRef[eProfileType.StoreGroupLevel] = 1;
            //aCellRef[eProfileType.StoreGrade] = 200;  // 264

            return aCellRef.CurrentCellValue;
        }

        public double DebugAssortmentPlaceholderColorDetail(int grade)
        {
            AssortmentPlaceholderColorDetail assrtCube = (AssortmentPlaceholderColorDetail)GetCube(eCubeType.AssortmentPlaceholderColorDetail);
            AssortmentCellReference aCellRef = (AssortmentCellReference)assrtCube.CreateCellReference();
            aCellRef[eProfileType.PlaceholderHeader] = 1922;
            //aCellRef[eProfileType.AllocationHeader] = int.MaxValue;
            aCellRef[eProfileType.AssortmentQuantityVariable] = this.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
            aCellRef[eProfileType.AssortmentDetailVariable] = ((AssortmentViewDetailVariables)AssortmentComputations.AssortmentDetailVariables).TotalUnits.Key;
            aCellRef[eProfileType.HeaderPack] = int.MaxValue;
            aCellRef[eProfileType.HeaderPackColor] = int.MaxValue;
            aCellRef[eProfileType.StoreGroupLevel] = 1;
            aCellRef[eProfileType.StoreGrade] = grade;  // 264

            return aCellRef.CurrentCellValue;
        }
		// END TT#101 - stodd -  no values after processing "create Placeholders"
		
		/// <summary>
		/// This private method inspecta a given CubeWaferCoordinate and assigns it's value to the given AssortmentCellReference, if the AssortmentCellReference
		/// has a dimension of the type specified in the CubeWaferCoordinate object.
		/// </summary>
		/// <param name="aWaferCoordinate">
		/// The CubeWaferCoordinate object that is to be assigned to the AssortmentCellReference.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference that is to be modified.
		/// </param>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//private void intLoadWaferCoordinates(CubeWaferCoordinate aWaferCoordinate, AssortmentCellReference aAssrtCellRef)
		private void intLoadWaferCoordinates(CubeWaferCoordinate aWaferCoordinate, ComputationCellReference aAssrtCellRef)
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			try
			{
				intSetDimensionIdx(aAssrtCellRef, aWaferCoordinate.WaferCoordinateType, aWaferCoordinate.Key);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method substitutes the given key into the given AssortmentCellReference in the coordinate specified by the given eProfileType.  If the eProfileType
		/// is not found to be a coordinate in the AssortmentCellReference, the value is not moved.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference that is to be modified.
		/// </param>
		/// <param name="aProfileType">
		/// The eProfileType that identifies the coordinate in the AssortmentCellReference to update.
		/// </param>
		/// <param name="aKey">
		/// The key to be substituted in the AssortmentCellReference.
		/// </param>

		//Begin TT#2 - JScott - Assortment Planning - Phase 2
		//private void intSetDimensionIdx(AssortmentCellReference aAssrtCellRef, eProfileType aProfileType, int aKey)
		private void intSetDimensionIdx(ComputationCellReference aAssrtCellRef, eProfileType aProfileType, int aKey)
		//End TT#2 - JScott - Assortment Planning - Phase 2
		{
			int coordinateIdx;

			try
			{
				//Begin TT#2 - JScott - Assortment Planning - Phase 2
				if (aProfileType == eProfileType.Variable)
				{
					aKey -= Include.AssortmentPlanVariableKeyOffset;
				}

				//End TT#2 - JScott - Assortment Planning - Phase 2
				coordinateIdx = aAssrtCellRef.GetDimensionProfileTypeIndex(aProfileType);
				if (coordinateIdx != -1)
				{
					aAssrtCellRef[coordinateIdx] = aKey;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets a AssortmentCellReference blocked status.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to set the block status for.
		/// </param>
		/// <param name="aBlock">
		/// The block status to set.
		/// </param>

		private void intSetRecursiveCellBlock(AssortmentCellReference aAssrtCellRef, bool aBlock)
		{
			ArrayList detailCellRefList;

			try
			{
				aAssrtCellRef.SetCellBlock(aBlock);
                aAssrtCellRef.SetEntryCellValue(0);		// TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 

				detailCellRefList = aAssrtCellRef.GetComponentDetailCellRefArray(false);

				foreach (AssortmentCellReference assrtCellRef in detailCellRefList)
				{
					intSetRecursiveCellBlock(assrtCellRef, aBlock);
				}

				intQueueTotalCellBlockSum(aAssrtCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds an AssortmentCellReference to the queue for Block summing.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to queue for Block summing.
		/// </param>

		private void intQueueTotalCellBlockSum(AssortmentCellReference aAssrtCellRef)
		{
			ArrayList totalCellRefList;
			AssortmentCellReference totCellRef;

			try
			{
				totalCellRefList = aAssrtCellRef.GetTotalCellRefArray();

				foreach (AssortmentCellReference totalCellRef in totalCellRefList)
				{
					foreach (QuantityVariableProfile quanVar in totalCellRef.AssortmentCube.QuantityVariableProfileList)
					{
						totCellRef = (AssortmentCellReference)totalCellRef.Copy();
						totCellRef[totCellRef.AssortmentCube.QuantityVariableProfileType] = quanVar.Key;

						if (totCellRef.GetVariableScopeVariableProfile().VariableScope == eVariableScope.Static)
						{
							if (!BlockRetotalHash.Contains(totCellRef))
							{
								BlockRetotalHash[totCellRef] = null;
								BlockRetotalQueue.Enqueue(totCellRef);
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
		/// Processes the Block summing Queue.
		/// </summary>

		protected void intProcessTotalCellBlockSum()
		{
			AssortmentCellReference asrtCellRef;

			try
			{
				while (BlockRetotalQueue.Count > 0)
				{
					asrtCellRef = (AssortmentCellReference)BlockRetotalQueue.Dequeue();
					asrtCellRef.SumDetailCellBlocked();
					intQueueTotalCellBlockSum(asrtCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


		// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need
        /// <summary>
        /// Sets a ComputationCellReference locked status. Overriden to update allocation profile for locks.
        /// </summary>
        /// <param name="aCompCellRef">
        /// The ComputationCellReference to set the lock status for.
        /// </param>
        /// <param name="aLock">
        /// The lock status to set.
        /// </param>

        override protected void intSetRecursiveCellLock(ComputationCellReference aCompCellRef, bool aLock)
        {
            ArrayList detailCellRefList;

            try
            {
                aCompCellRef.SetCellLock(aLock);

                detailCellRefList = aCompCellRef.GetComponentDetailCellRefArray(false);

                foreach (ComputationCellReference compCellRef in detailCellRefList)
                {
                    intSetRecursiveCellLock(compCellRef, aLock);

                    int hdrIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                    int phIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);	// TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
                    if (hdrIndex != -1 || phIndex != -1)	// TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
                    {
                        SetLockUnlockStoreList(compCellRef, aLock);
                    }
                }

                intQueueTotalCellLockSum(aCompCellRef);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetLockUnlockStoreList(ComputationCellReference aCompCellRef, bool aLock)
        {
            AllocationProfile ap = null;
            ProfileList storeList = null;
            try
            {
                //ComputationCellReference cellRef = GetCell(aGrid, row, col);

                // cellRef Indexes
                int hdrIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                int phIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.PlaceholderHeader);	// TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
                int storeGroupLevelIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);
                int gradeIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);
                int packColorIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
                int packIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);

                // cellRef values
                int storeGroupLevelRid = -1;
                int grade = -1;

                if (hdrIndex != -1)
                {
                    ap = _transaction.GetAssortmentMemberProfile(aCompCellRef[eProfileType.AllocationHeader]);
                }
				// Begin TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
                else if (phIndex != -1)
                {
                    ap = _transaction.GetAssortmentMemberProfile(aCompCellRef[eProfileType.PlaceholderHeader]);
                }
				// End TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
                if (storeGroupLevelIndex != -1)
                {
                    storeGroupLevelRid = aCompCellRef[eProfileType.StoreGroupLevel];
                }
                if (gradeIndex != -1)
                {
                    grade = aCompCellRef[eProfileType.StoreGrade];
                }

                // Only process if there is a valid header.
                // NOTE: May need to add PLACEHOLDER logic too
                if (ap != null)
                {
                    storeList = GetStoreList(ap, storeGroupLevelRid, grade);

                    if (packIndex != -1 && aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    {
                        if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                        {
                            // pack and color
                            // Can't do Pack / Color
                            int packRid = aCompCellRef[eProfileType.HeaderPack];
                            PackHdr aPack = ap.GetPackHdr(packRid);
                            ap.SetStoreListTotalLocked(aPack, storeList, aLock);
                        }
                        else
                        {
                            // Pack
                            int packRid = aCompCellRef[eProfileType.HeaderPack];
                            PackHdr aPack = ap.GetPackHdr(packRid);
                            ap.SetStoreListTotalLocked(aPack, storeList, aLock);
                        }
                    }
                    else
                    {
                        if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                        {
                            // color
                            int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                            ap.SetStoreListTotalLocked(colorRid, storeList, aLock);
                        }
                        else
                        {
                            // no pack, no color
                            ap.SetStoreListTotalLocked(eAllocationSummaryNode.Total, storeList, aLock);
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
		// End TT#3809 - stodd - Locked Cell doesn't save when processing Need

		// Begin TT#4294 - stodd - Average Units in Matrix Enahancement
        public int GetCountOfStoresWithAllocation(ComputationCellReference aCompCellRef, out ProfileList outStoreList)
        {
			// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
            bool doesSetGradeContainStores = false;
            return GetCountOfStoresWithAllocation(aCompCellRef, false, out outStoreList, out doesSetGradeContainStores);
			// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
        }

        public int GetCountOfStoresWithAllocation(ComputationCellReference aCompCellRef, bool ignoreGrade, out ProfileList aOutStoreList, out bool doesSetGradeContainStores)
        {
            AllocationProfile ap = null;
            ProfileList storeList = null;
            ProfileList outStoreList = null;
            doesSetGradeContainStores = false;	// TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid

            int storeCnt = 0;
            try
            {
                //ComputationCellReference cellRef = GetCell(aGrid, row, col);

                // cellRef Indexes
                int hdrIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                int storeGroupLevelIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);
                int gradeIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);
                int packColorIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
                int packIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);

                // cellRef values
                int storeGroupLevelRid = -1;
                int grade = -1;

                if (hdrIndex != -1)
                {
                    ap = _transaction.GetAssortmentMemberProfile(aCompCellRef[eProfileType.AllocationHeader]);
                }
                if (storeGroupLevelIndex != -1)
                {
                    storeGroupLevelRid = aCompCellRef[eProfileType.StoreGroupLevel];
                }

                // This gets called with detail cells that include the grade. 
                // Sometimes we want to ignore the grade when determining the number of stores.
                if (!ignoreGrade)
                {
                    if (gradeIndex != -1)
                    {
                        grade = aCompCellRef[eProfileType.StoreGrade];
                    }
                }

                // Only process if there is a valid header.
                // NOTE: May need to add PLACEHOLDER logic too
                if (ap != null)
                {
                    storeList = GetStoreList(ap, storeGroupLevelRid, grade);
					// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                    if (storeList.Count > 0)
                    {
                        doesSetGradeContainStores = true;
                    }
					// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid

                    if (packIndex != -1)
                    {
                        if (aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                        {
                            if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                            {
                                // pack and color
                                // Can't do Pack / Color
                                int packRid = aCompCellRef[eProfileType.HeaderPack];
                                PackHdr aPack = ap.GetPackHdr(packRid);
                                storeCnt = ap.GetCountOfStoreListWithAllocation(aPack, storeList, out outStoreList);
                            }
                            else
                            {
                                // Pack
                                int packRid = aCompCellRef[eProfileType.HeaderPack];
                                PackHdr aPack = ap.GetPackHdr(packRid);
                                storeCnt = ap.GetCountOfStoreListWithAllocation(aPack, storeList, out outStoreList);
                            }
                        }
                        else
                        {
                            if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)     // TT#1694-MD - stodd - GA gets "Color not defined for Bulk" for headers with no components
                            {
                                int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                                storeCnt = ap.GetCountOfStoreListWithAllocation(colorRid, storeList, out outStoreList);
                            }
                            else
                            {
                                storeCnt = ap.GetCountOfStoreListWithAllocation(eAllocationSummaryNode.Total, storeList, out outStoreList);     // TT#1678-MD - stodd - GA Allocation -> Matrix Tab-> enter Average Units and apply-> changes to zero

                            }
                        }
                    }
                    else
                    {
                        if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                        {
                            // color
                            int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                            if (ap.BulkColors.ContainsKey(colorRid))
                            {
                                storeCnt = ap.GetCountOfStoreListWithAllocation(colorRid, storeList, out outStoreList);
                            }
                        }
                        else
                        {
                            // no pack, no color
                            storeCnt = ap.GetCountOfStoreListWithAllocation(eAllocationSummaryNode.Total, storeList, out outStoreList);
                        }
                    }
                }
                else
                {
                    // If has a pack but no header, find the header. (Packs are unique.)
                    // Then we can determine the number of allocation stores.
                    if (packIndex != -1 && aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    {
                        ap = FindHeaderWithPack(aCompCellRef[eProfileType.HeaderPack]);
                    }

                    if (ap != null)
                    {
                        storeList = GetStoreList(ap, storeGroupLevelRid, grade);
                        PackHdr aPack = ap.GetPackHdr(aCompCellRef[eProfileType.HeaderPack]);
                        storeCnt = ap.GetCountOfStoreListWithAllocation(aPack, storeList, out outStoreList);
                    }
                }

                aOutStoreList = outStoreList;
                return storeCnt;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int GetCountOfStoresWithAllocation(ArrayList aCompCellRefList)
        {
            return GetCountOfStoresWithAllocation(aCompCellRefList, false);
        }

        public int GetCountOfStoresWithAllocation(ArrayList aCompCellRefList, bool ignoreGrade)
        {
            ProfileList outStoreList = null;
            ProfileList mergedStoreList = new ProfileList(eProfileType.Store);
            foreach (ComputationCellReference aCompCellRef in aCompCellRefList)
            {
				// Begin TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                bool doesSetGradeContainStores = false;	
                int storeCnt = GetCountOfStoresWithAllocation(aCompCellRef, ignoreGrade, out outStoreList, out doesSetGradeContainStores);
				// End TT#1756 - stodd - Receive NothingToSpread error when entering data on Group Allocation Matrix grid
                foreach (StoreProfile sp in outStoreList.ArrayList)
                {
                    if (!mergedStoreList.Contains(sp.Key))
                    {
                        mergedStoreList.Add(sp);
                    }
                }
            }

            return mergedStoreList.Count;
        }
		// End TT#4294 - stodd - Average Units in Matrix Enahancement

		// Begin TT#4369 - stodd - Get "Bulk color not defined" error when Pack is removed from heading columns and one of the headers contains a pack color  
        public int GetAllocationBalance(ComputationCellReference aCompCellRef)
        {
            return GetAllocationBalance(aCompCellRef, false);
        }
        public int GetAllocationBalance(ComputationCellReference aCompCellRef, bool ignoreGrade)
        {
            AllocationProfile ap = null;
            ProfileList storeList = null;

            int balance = 0;
            int tempValue = 0;
            try
            {
                // cellRef Indexes
                int hdrIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                int storeGroupLevelIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);
                int gradeIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);
                int packColorIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
                int packIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);

                // cellRef values
                int storeGroupLevelRid = -1;
                int grade = -1;

                if (hdrIndex != -1)
                {
                    ap = _transaction.GetAssortmentMemberProfile(aCompCellRef[eProfileType.AllocationHeader]);
                }
                if (storeGroupLevelIndex != -1)
                {
                    storeGroupLevelRid = aCompCellRef[eProfileType.StoreGroupLevel];
                }

                // This gets called with detail cells that include the grade. 
                // Sometimes we want to ignore the grade when determining the number of stores.
                if (!ignoreGrade)
                {
                    if (gradeIndex != -1)
                    {
                        grade = aCompCellRef[eProfileType.StoreGrade];
                    }
                }

                // Only process if there is a valid header.
                // NOTE: May need to add PLACEHOLDER logic too
                if (ap != null)
                {
                    storeList = GetStoreList(ap, storeGroupLevelRid, grade);

                    if (packIndex != -1)
                    {
                        if (aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                        {
                            if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                            {
                                // pack and color
                                // Can't do Pack / Color
                                int packRid = aCompCellRef[eProfileType.HeaderPack];
                                PackHdr aPack = ap.GetPackHdr(packRid);
                                tempValue = ap.GetAllocatedBalance(aPack);
                                balance = tempValue * aPack.PackMultiple;
                            }
                            else
                            {
                                // Pack
                                int packRid = aCompCellRef[eProfileType.HeaderPack];
                                PackHdr aPack = ap.GetPackHdr(packRid);
                                tempValue = ap.GetAllocatedBalance(aPack);
                                balance = tempValue * aPack.PackMultiple;
                            }
                        }
                        else
                        {
                            if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)   // TT#1694-MD - stodd - GA gets "Color not defined for Bulk" for headers with no components
                            {
                                int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                                balance = ap.GetAllocatedBalance(colorRid);
                            }
                            else
                            {
                                balance = ap.GetAllocatedBalance(eAllocationSummaryNode.Total); 	// TT#1678-MD - stodd - GA Allocation -> Matrix Tab-> enter Average Units and apply-> changes to zero

                            }
                        }
                    }
                    else
                    {
                        if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                        {
                            // color
                            int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                            if (ap.BulkColors.ContainsKey(colorRid))
                            {
                                balance = ap.GetAllocatedBalance(colorRid);
                            }
                        }
                        else
                        {
                            // no pack, no color
                            balance = ap.GetAllocatedBalance(eAllocationSummaryNode.Total);
                        }
                    }
                }
                else
                {
                    // If has a pack but no header, find the header. (Packs are unique.)
                    // Then we can determine the number of allocation stores.
                    if (packIndex != -1 && aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    {
                        ap = FindHeaderWithPack(aCompCellRef[eProfileType.HeaderPack]);
                    }

                    if (ap != null)
                    {
                        storeList = GetStoreList(ap, storeGroupLevelRid, grade);
                        PackHdr aPack = ap.GetPackHdr(aCompCellRef[eProfileType.HeaderPack]);
                        tempValue = ap.GetAllocatedBalance(aPack);
                        balance = tempValue * aPack.PackMultiple;
                    }
                }

                return balance;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int GetAllocationBalance(ArrayList aCompCellRefList)
        {
            int newBalance = 0;
            foreach (ComputationCellReference aCompCellRef in aCompCellRefList)
            {
                int balance = GetAllocationBalance(aCompCellRef);
                newBalance += balance;
            }

            return newBalance;
        }

        public int GetReserve(ComputationCellReference aCompCellRef)
        {
            return GetReserve(aCompCellRef, false);
        }
        public int GetReserve(ComputationCellReference aCompCellRef, bool ignoreGrade)
        {
            AllocationProfile ap = null;
            ProfileList storeList = null;

            int reserve = 0;
            int tempValue = 0;
            try
            {
                Index_RID reserveStore = _transaction.ReserveStore;
                if (reserveStore.RID == Include.NoRID)
                {
                    return 0;
                }

                // cellRef Indexes
                int hdrIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader);
                int storeGroupLevelIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGroupLevel);
                int gradeIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.StoreGrade);
                int packColorIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
                int packIndex = aCompCellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);

                // cellRef values
                int storeGroupLevelRid = -1;
                int grade = -1;

                if (hdrIndex != -1)
                {
                    ap = _transaction.GetAssortmentMemberProfile(aCompCellRef[eProfileType.AllocationHeader]);
                }
                if (storeGroupLevelIndex != -1)
                {
                    storeGroupLevelRid = aCompCellRef[eProfileType.StoreGroupLevel];
                }

                // This gets called with detail cells that include the grade. 
                // Sometimes we want to ignore the grade when determining the number of stores.
                if (!ignoreGrade)
                {
                    if (gradeIndex != -1)
                    {
                        grade = aCompCellRef[eProfileType.StoreGrade];
                    }
                }

                // Only process if there is a valid header.
                // NOTE: May need to add PLACEHOLDER logic too
                if (ap != null)
                {
                    storeList = GetStoreList(ap, storeGroupLevelRid, grade);

                    if (packIndex != -1)
                    {
                        if (aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                        {
                            if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                            {
                                // pack and color
                                // Can't do Pack / Color
                                int packRid = aCompCellRef[eProfileType.HeaderPack];
                                PackHdr aPack = ap.GetPackHdr(packRid);
                                tempValue = ap.GetStoreQtyAllocated(aPack.PackName, reserveStore);
                                reserve = tempValue * aPack.PackMultiple;
                            }
                            else
                            {
                                // Pack
                                int packRid = aCompCellRef[eProfileType.HeaderPack];
                                PackHdr aPack = ap.GetPackHdr(packRid);
                                tempValue = ap.GetStoreQtyAllocated(aPack.PackName, reserveStore);
                                reserve = tempValue * aPack.PackMultiple;
                            }
                        }
                        else
                        {
                            if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue) // TT#1694-MD - stodd - GA gets "Color not defined for Bulk" for headers with no components
                            {
                                int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                                reserve = ap.GetStoreQtyAllocated(colorRid, reserveStore);
                            }
                            else
                            {
                                reserve = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, reserveStore);
                            }
                        }
                    }
                    else
                    {
                        if (packColorIndex != -1 && aCompCellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                        {
                            // color
                            int colorRid = aCompCellRef[eProfileType.HeaderPackColor];
                            if (ap.BulkColors.ContainsKey(colorRid))
                            {
                                reserve = ap.GetStoreQtyAllocated(colorRid, reserveStore);
                            }
                        }
                        else
                        {
                            // no pack, no color
                            reserve = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStore);
                        }
                    }
                }
                else
                {
                    // If has a pack but no header, find the header. (Packs are unique.)
                    // Then we can determine the number of allocation stores.
                    if (packIndex != -1 && aCompCellRef[eProfileType.HeaderPack] != int.MaxValue)
                    {
                        ap = FindHeaderWithPack(aCompCellRef[eProfileType.HeaderPack]);
                    }

                    if (ap != null)
                    {
                        storeList = GetStoreList(ap, storeGroupLevelRid, grade);
                        PackHdr aPack = ap.GetPackHdr(aCompCellRef[eProfileType.HeaderPack]);
                        tempValue = ap.GetStoreQtyAllocated(aPack.PackName, reserveStore);
                        reserve = tempValue * aPack.PackMultiple;
                    }
                }

                return reserve;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int GetReserve(ArrayList aCompCellRefList)
        {
            int newReserve = 0;
            foreach (ComputationCellReference aCompCellRef in aCompCellRefList)
            {
                int reserve = GetReserve(aCompCellRef);
                newReserve += reserve;
            }

            return newReserve;
        }
		// End TT#4369 - stodd - Get "Bulk color not defined" error when Pack is removed from heading columns and one of the headers contains a pack color  

        private AllocationProfile FindHeaderWithPack(int aPackRid)
        {
            AllocationProfile ap = null;
            foreach (AllocationHeaderProfile ahp in _headerList.ArrayList)
            {
                if (ahp.HeaderType != eHeaderType.Assortment && ahp.HeaderType != eHeaderType.Placeholder)
                {
                    foreach (HeaderPackProfile pack in ahp.Packs.Values)    // TT#488 - MD - Jellis - Group Allocation
                    {
                        if (pack.Key == aPackRid)
                        {
                            ap = _transaction.GetAssortmentMemberProfile(ahp.Key);
                            break;
                        }
                    }
                    if (ap != null)
                    {
                        break;
                    }
                }
            }

            return ap;
        }
		// End TT#4294 - stodd - Average Units in Matrix Enahancement

		// Begin TT#3848 - stodd - Locked cell not able to be changed after unlocking
        public bool IsDetailVariableDisplayed(int aDetailVariableKey)
        {
            bool isDisplayed = false;
            if (_selectableDetailColumnHeaders == null)
            {
                isDisplayed = true;
            }
            else
            {
                for (int i = 0; i < _selectableDetailColumnHeaders.Count; i++)
                {
                    RowColProfileHeader rcph = (RowColProfileHeader)_selectableDetailColumnHeaders[i];
                    if (rcph.Profile.Key == aDetailVariableKey)
                    {
                        if (rcph.IsDisplayed)
                        {
                            isDisplayed = true;

                        }
                        break;
                    }
                }
            }
            return isDisplayed;
        }
		// End TT#3848 - stodd - Locked cell not able to be changed after unlocking

		// Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
        public List<int> GetStyleList()
        {
            List<int> styleList = new List<int>();

            AllocationProfileList apl = this.Transaction.GetAssortmentMemberProfileList();

            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                {
                    continue;
                }

                if (!styleList.Contains(ap.StyleHnRID))
                {
                    styleList.Add(ap.StyleHnRID);
                }
            }

            return styleList;
        }


        public List<int> GetParentOfStyleList()
        {
            List<int> parentOfStyleList = new List<int>();

            AllocationProfileList apl = this.Transaction.GetAssortmentMemberProfileList();

            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                {
                    continue;
                }

                HierarchyNodeProfile hnParent = ap.AppSessionTransaction.GetParentNodeData(ap.StyleHnRID);

                if (!parentOfStyleList.Contains(hnParent.Key))
                {
                    parentOfStyleList.Add(hnParent.Key);
                }
            }

            return parentOfStyleList;
        }
		// End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
        
	}

	/// <summary>
	/// Class that defines the Hash key for the BlockedList.
	/// </summary>

	public class BlockedListHashKey
	{
		//=======
		// FIELDS
		//=======

		private int _placeholderRID;
		private int _strGrpLvlRID;
		private int _gradeRID;

		//=============
		// CONSTRUCTORS
		//=============

		public BlockedListHashKey(int aPlaceholderRID, int aStrGrpLvlRID, int aGradeRID)
		{
			_placeholderRID = aPlaceholderRID;
			_strGrpLvlRID = aStrGrpLvlRID;
			_gradeRID = aGradeRID;
		}

		//===========
		// PROPERTIES
		//===========

		public int PlaceholderRID
		{
			get
			{
				return _placeholderRID;
			}
		}

		public int StrGrpLvlRID
		{
			get
			{
				return _strGrpLvlRID;
			}
		}

		public int GradeRID
		{
			get
			{
				return _gradeRID;
			}
		}

		//========
		// METHODS
		//========

		public override int GetHashCode()
		{
			return Include.CreateHashKey(_placeholderRID, _strGrpLvlRID, _gradeRID);
		}

		public override bool Equals(object obj)
		{
			try
			{
				if (obj.GetType() == typeof(BlockedListHashKey))
				{
					return ((BlockedListHashKey)obj)._placeholderRID == _placeholderRID &&
						((BlockedListHashKey)obj)._strGrpLvlRID == _strGrpLvlRID &&
						((BlockedListHashKey)obj)._gradeRID == _gradeRID;
				}
				else
				{
					return false;
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
