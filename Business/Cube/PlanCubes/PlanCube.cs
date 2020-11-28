using System;
using System.Collections;
using System.Globalization;	
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCube is an UndoCube that provides computation functionality.
	/// </summary>
	/// <remarks>
	/// For Planning, there is additional functionality that is required in addition to the UndoCube functionality.  This functionality includes
	/// keeping track of changed cells and temporary computation Cell work areas.
	/// </remarks>

	abstract public class PlanCube : ComputationCube
	{
		//=======
		// FIELDS
		//=======

		protected ReadLog _planReadLog;
		private ProfileList _masterVariableProfileList;
		private ProfileList _masterQuantityVariableProfileList;
		private ProfileList _masterTimeTotalVariableProfileList;
		private ProfileList _masterVersionProfileList;
		private ProfileList _masterNodeProfileList;
		private int _versionIdx;
		private int _nodeIdx;
		private int _weekIdx;
		private int _quantityVarIdx;
		private int _varIdx;
		private int _storeIdx;
		private int _periodIdx;
		private int _timeTotalVarIdx;
		private int _basisIdx;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanCube, using the given SessionAddressBlock, Transaction, PlanCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this PlanCube.
		/// </param>

		public PlanCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, PlanCubeGroup aPlanCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDefinition, aCubeAttributes, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			try
			{
				_planReadLog = new ReadLog();
				_versionIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Version);
				_nodeIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.HierarchyNode);
				_weekIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Week);
				_quantityVarIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.QuantityVariable);
				_varIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Variable);
				_storeIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Store);
				_periodIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Period);
				_timeTotalVarIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.TimeTotalVariable);
				_basisIdx = CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Basis);
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
		/// Abstract method that returns for the eProfileType for the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public eProfileType VariableProfileType
		{
			get
			{
				return eProfileType.Variable;
			}
		}

		/// <summary>
		/// Abstract method that returns for the eProfileType for the QuantityVariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public eProfileType QuantityVariableProfileType
		{
			get
			{
				return eProfileType.QuantityVariable;
			}
		}

		/// <summary>
		/// Abstract property returns the ProfileList for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public ProfileList QuantityVariableProfileList
		{
			get
			{
				return MasterQuantityVariableProfileList;
			}
		}

		/// <summary>
		/// Gets a reference to the PlanCubeGroup.
		/// </summary>

		public PlanCubeGroup PlanCubeGroup
		{
			get
			{
				return (PlanCubeGroup)_cubeGroup;
			}
		}

		/// <summary>
		/// Gets the master Variable ProfileList.
		/// </summary>

		public ProfileList MasterVariableProfileList
		{
			get
			{
				if (_masterVariableProfileList == null)
				{
					_masterVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.Variable);
				}

				return _masterVariableProfileList;
			}
		}

		/// <summary>
		/// Gets the master Variable ProfileList.
		/// </summary>

		public ProfileList MasterQuantityVariableProfileList
		{
			get
			{
				if (_masterQuantityVariableProfileList == null)
				{
					_masterQuantityVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.QuantityVariable);
				}

				return _masterQuantityVariableProfileList;
			}
		}

		/// <summary>
		/// Gets the master Variable ProfileList.
		/// </summary>

		public ProfileList MasterTimeTotalVariableProfileList
		{
			get
			{
				if (_masterTimeTotalVariableProfileList == null)
				{
					_masterTimeTotalVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.TimeTotalVariable);
				}

				return _masterTimeTotalVariableProfileList;
			}
		}

		/// <summary>
		/// Gets the master Version ProfileList.
		/// </summary>

		public ProfileList MasterVersionProfileList
		{
			get
			{
				if (_masterVersionProfileList == null)
				{
					_masterVersionProfileList = CubeGroup.GetMasterProfileList(eProfileType.Version);
				}

				return _masterVersionProfileList;
			}
		}

		/// <summary>
		/// Gets the master Version ProfileList.
		/// </summary>

		public ProfileList MasterNodeProfileList
		{
			get
			{
				if (_masterNodeProfileList == null)
				{
					_masterNodeProfileList = CubeGroup.GetMasterProfileList(eProfileType.HierarchyNode);
				}

				return _masterNodeProfileList;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Plan type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Plan type cube.
		/// </returns>

		public bool isPlanCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.Plan) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Basis type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Basis type cube.
		/// </returns>

		public bool isBasisCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.Basis) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Chain type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Chain type cube.
		/// </returns>

		public bool isChainCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.Chain) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Store type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Store type cube.
		/// </returns>

		public bool isStoreCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.Store) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Week Detail type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Week Detail type cube.
		/// </returns>

		public bool isWeekDetailCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.WeekDetail) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Period Detail type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Period Detail type cube.
		/// </returns>

		public bool isPeriodDetailCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.PeriodDetail) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Time Detail type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Time Detail type cube.
		/// </returns>

		public bool isTimeDetailCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.WeekDetail) > 0 || (CubeAttributes & PlanCubeAttributesFlagValues.PeriodDetail) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Date Total type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Date Total type cube.
		/// </returns>

		public bool isDateTotalCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.DateTotal) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the cube is a Low-level Total type cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the cube is a Low-level Total type cube.
		/// </returns>

		public bool isLowLevelTotalCube
		{
			get
			{
				try
				{
					return ((CubeAttributes & PlanCubeAttributesFlagValues.LowLevelTotal) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that returns the eCubeType of the Chain Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Chain Detail cube.
		/// </returns>

		abstract public eCubeType GetChainDetailCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Low-Level Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Detail cube.
		/// </returns>

		abstract public eCubeType GetLowLevelDetailCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the week Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the week Detail cube.
		/// </returns>

		abstract public eCubeType GetWeekDetailCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Low-Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Low-Level Total cube.
		/// </returns>

		abstract public eCubeType GetLowLevelTotalCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Detail cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Detail cube.
		/// </returns>

		abstract public eCubeType GetStoreDetailCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Group Total cube.
		/// </returns>

		abstract public eCubeType GetGroupTotalCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Store Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Store Total cube.
		/// </returns>

		abstract public eCubeType GetStoreTotalCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Date Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Date Total cube.
		/// </returns>

		abstract public eCubeType GetDateTotalCubeType();

		//Begin Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Abstract method that returns the eCubeType of the Plan cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Plan cube.
		/// </returns>

		abstract public eCubeType GetPlanCubeType();

		//End Track #6010 - JScott - Bad % Change on Basis2
		/// <summary>
		/// Abstract method that returns the eCubeType of the Basis cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Basis cube.
		/// </returns>

		abstract public eCubeType GetBasisCubeType();

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Abstract method that returns a boolean indicating if the given PlanCellReference PlanCell should be marked as read-only.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to get the read-only status for.
		/// </param>
		/// <returns>
		/// A boolean indicating if the given PlanCellReference PlanCell should be marked as read-only.
		/// </returns>

		abstract public bool isPlanCellReadOnly(PlanCellReference aPlanCellRef);

		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Method that returns the VersionProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The VersionProfile for the given PlanCellReference.
		/// </returns>

		abstract public VersionProfile GetVersionProfile(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Method that returns the HierarchyNodeProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The HierarchyNodeProfile for the given PlanCellReference.
		/// </returns>

		abstract public HierarchyNodeProfile GetHierarchyNodeProfile(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Method that returns a ProfileList of the weeks for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
		/// <returns>
		/// A ProfileList of time.
		/// </returns>

		abstract public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Method that returns the AverageDivisor for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The AverageDivisor for the given PlanCellReference.
		/// </returns>

		abstract public double GetAverageDivisor(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Method that returns a boolean indicating if the given PlanCellReference contains the current week.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the given PlanCellReference contains the current week.
		/// </returns>

		abstract public bool ContainsCurrentWeek(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Method that returns a ProfileList of the weeks for the given PlanCellReference and eProfileType.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
        /// <param name="aDateProfileType">
		/// The eProfileType of the date type to look up.
		/// </param>
		/// <returns>
		/// A ProfileList of time.
		/// </returns>

		abstract public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef, eProfileType aDateProfileType);

		/// <summary>
		/// Method that returns the store status for the Cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to process.
		/// </param>
		/// <returns>
		/// The eStoreStatus value that describes the store status of the Cell.
		/// </returns>

		abstract public eStoreStatus GetStoreStatus(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a Cell is read.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to read.
		/// </param>

		abstract public void ReadCell(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is eligible.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that point to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is eligible.
		/// </returns>

		abstract public bool isStoreIneligible(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is protected.
		/// </summary>
		/// <param name="aPlanCellRef"></param>
		/// <returns></returns>

		abstract public bool isVersionProtected(PlanCellReference aPlanCellRef);

		//Begin Track #5669 - JScott - BMU %
		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is display-only.
		/// </summary>
		/// <param name="aPlanCellRef"></param>
		/// <returns></returns>

		abstract public bool isDisplayOnly(PlanCellReference aPlanCellRef);

		//End Track #5669 - JScott - BMU %
		/// <summary>
		/// Returns true if any cell for the given ProfileList of PlanProfiles has changed.
		/// </summary>
		/// <param name="aPlanProfileList">
		/// A ProfileList of PlanProfiles to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		abstract public bool hasAnyPlanChanged(ProfileList aPlanProfileList);

		/// <summary>
		/// Returns true if any cell for the given PlanProfile has changed.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if any cell has changed.
		/// </returns>

		abstract public bool hasPlanChanged(PlanProfile aPlanProfile);

		/// <summary>
		/// Returns a boolean value indicating if the store of the Cell pointed to by the given PlanCellReference is closed.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the store for the given PlanCellReference is closed.
		/// </returns>

		abstract public bool isStoreClosed(PlanCellReference aPlanCellRef);

		/// <summary>
		/// Returns an eProfileType of the time dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the time dimension.
		/// </returns>

		abstract public eProfileType GetTimeType();

		/// <summary>
		/// Returns an eProfileType of the version dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the version dimension.
		/// </returns>

		abstract public eProfileType GetVersionType();

		/// <summary>
		/// Returns an eProfileType of the hierarchy node dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the Hierarchy Node dimension.
		/// </returns>

		abstract public eProfileType GetHierarchyNodeType();

		/// <summary>
		/// Returns an incremented Time key.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <param name="aIncrement">
		/// The amount to increment the time key by.
		/// </param>
		/// <returns>
		/// The incremented time key.
		/// </returns>

		abstract public int IncrementTimeKey(PlanCellReference aPlanCellRef, int aIncrement);

		/// <summary>
		/// Returns an incremented Time key.
		/// </summary>
		/// <param name="aTimeKey">
		/// The key of the time index.
		/// </param>
		/// <param name="aIncrement">
		/// The amount to increment the time key by.
		/// </param>
		/// <returns>
		/// The incremented time key.
		/// </returns>

		abstract public int IncrementTimeKey(int aTimeKey, int aIncrement);

		//Begin TT#894 - JScott - Locks are not "bolded" or showing held in OTS FC Review 
		///// <summary>
		///// Allows a cube to specify custom initializations for a Cell.  Occurs after the standard Cell initialization.
		///// </summary>
		///// <param name="aPlanCellRef">
		///// The PlanCellReference to initialize.
		///// </param>

		//virtual public void InitCellValue(PlanCellReference aPlanCellRef)
		//{
		//}

		//End TT#894 - JScott - Locks are not "bolded" or showing held in OTS FC Review
		//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
		virtual public void ClearCubeForHierarchyVersion(PlanCellReference aCellRef)
		{
		}

		//End Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
		/// <summary>
		/// Creates a new instance of a PlanCubeCell.
		/// </summary>
		/// <returns>
		/// A reference to a new PlanCubeCell.
		/// </returns>

		override public Cell CreateCell(CellReference aCellReference)
		{
			PlanCell planCell;
			HierarchyNodeProfile nodeProf;
			VersionProfile versProf;

			try
			{
				planCell = new PlanCell();

				if (ReadOnly)
				{
					planCell.isReadOnly = true;
				}
				//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
				else if (isPlanCellReadOnly((PlanCellReference)aCellReference))
				{
					planCell.isReadOnly = true;
				}
				//End Track #5121 - JScott - Add Year/Season/Quarter totals
				else if (CheckNodeSecurity)
				{
					nodeProf = GetHierarchyNodeProfile((PlanCellReference)aCellReference);

					if (isStoreCube)
					{
						if (nodeProf.StoreSecurityProfile.AccessDenied)
						{
							planCell.isNull = true;
						}
						else if (!nodeProf.StoreSecurityProfile.AllowUpdate)
						{
							planCell.isReadOnly = true;
						}
						else
						{
							versProf = this.GetVersionProfile((PlanCellReference)aCellReference);

							if (versProf.StoreSecurity.AccessDenied)
							{
								planCell.isNull = true;
							}
							else if (!versProf.StoreSecurity.AllowUpdate)
							{
								planCell.isReadOnly = true;
							}
						}
					}
					else
					{
						if (nodeProf.ChainSecurityProfile.AccessDenied)
						{
							planCell.isNull = true;
						}
						else if (!nodeProf.ChainSecurityProfile.AllowUpdate)
						{
							planCell.isReadOnly = true;
						}
						else
						{
							versProf = this.GetVersionProfile((PlanCellReference)aCellReference);

							if (versProf.ChainSecurity.AccessDenied)
							{
								planCell.isNull = true;
							}
							else if (!versProf.ChainSecurity.AllowUpdate)
							{
								planCell.isReadOnly = true;
							}
						}
					}
				}

				return planCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a PlanCellReference.
		/// </summary>
		/// <returns>
		/// A reference to a new PlanCellReference.
		/// </returns>

		override public CellReference CreateCellReference()
		{
			try
			{
				return new PlanCellReference(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a PlanCellReference using the given CellCoordinates.
		/// </summary>
		/// <param name="aCoordinates">
		/// The CellCoordinates object that describes the PlanCubeCell's position in this PlanCube.
		/// </param>
		/// <returns>
		/// A reference to a new PlanCellReference.
		/// </returns>

		override public CellReference CreateCellReference(CellCoordinates aCoordinates)
		{
			try
			{
				return new PlanCellReference(this, aCoordinates);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a PlanCellReference using the given CellReference.
		/// </summary>
		/// <param name="aCellRef">
		/// The CellReference object that describes the PlanCubeCell's position in this PlanCube.  This indices in this object will be translated to 
		/// cooresponding coordinates in this PlanCube.
		/// </param>
		/// <returns>
		/// A reference to a new PlanCellReference.
		/// </returns>

		override public CellReference CreateCellReference(CellReference aCellRef)
		{
			try
			{
				return new PlanCellReference(this, aCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the dimension index for the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType to find the index for.
		/// </param>
		/// <returns>
		/// The dimension index of the given eProfileType.
		/// </returns>

		override public int GetDimensionProfileTypeIndex(eProfileType aProfileType)
		{
			try
			{
				switch (aProfileType)
				{
					case eProfileType.Version:

						return _versionIdx;

					case eProfileType.HierarchyNode:

						return _nodeIdx;

					case eProfileType.Week:

						return _weekIdx;

					case eProfileType.QuantityVariable:

						return _quantityVarIdx;

					case eProfileType.Variable:

						return _varIdx;

					case eProfileType.Store:

						return _storeIdx;

					case eProfileType.Period:

						return _periodIdx;

					case eProfileType.TimeTotalVariable:

						return _timeTotalVarIdx;

					case eProfileType.Basis:

						return _basisIdx;

					default:

						return base.GetDimensionProfileTypeIndex(aProfileType);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public override void Clear()
		{
			try
			{
				_planReadLog.Clear();
				base.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
		/// <summary>
		/// Returns the Time key of the data for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the Time key of.
		/// </param>
		/// <returns>
		/// The Time key of the data for the given PlanCellReference.
		/// </returns>

		abstract public int GetWeekKeyOfData(PlanCellReference aPlanCellRef);

		//End Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
		//Begin Track #5741 - JScott - OTS Forecast Review Combined Version In Store Inventory not appearing for Actuals.
		public VersionProfile GetVersionProfileOfData(PlanCellReference aPlanCellRef)
		{
			VersionProfile verProf;
			VariableProfile varProf;
			int weekRID;
			int currWeekRID;
			//Begin Track #5904 - JScott - B OTB version missing some actuals for competed month
			int dataVerRID;
			WeekProfile weekProf;
			PeriodProfile perProf;
			//End Track #5904 - JScott - B OTB version missing some actuals for competed month

			try
			{
				verProf = (VersionProfile)aPlanCellRef.GetVersionProfile();

				if (verProf.IsBlendedVersion && aPlanCellRef.PlanCube.isWeekDetailCube)
				{
					varProf = (VariableProfile)MasterVariableProfileList.FindKey(aPlanCellRef[eProfileType.Variable]);
					//Begin Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
					//weekRID = aPlanCellRef[eProfileType.Week];
					weekRID = GetWeekKeyOfData(aPlanCellRef);
					//End Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
					currWeekRID = _SAB.ApplicationServerSession.Calendar.CurrentWeek.Key;

					//Begin Track #5904 - JScott - B OTB version missing some actuals for competed month
					//if (weekRID < currWeekRID || (varProf.VariableType == eVariableType.BegStock && weekRID == currWeekRID))
					//{
					//    return (VersionProfile)MasterVersionProfileList.FindKey(verProf.ActualVersionRID);
					//}
					//else
					//{
					//    return (VersionProfile)MasterVersionProfileList.FindKey(verProf.ForecastVersionRID);
					//}
					if (verProf.BlendType == eForecastBlendType.Week)
					{
						if (weekRID < currWeekRID || (varProf.VariableType == eVariableType.BegStock && weekRID == currWeekRID))
						{
							dataVerRID = verProf.ActualVersionRID;
						}
						else
						{
							dataVerRID = verProf.ForecastVersionRID;
						}
					}
					else // verProf.BlendType == eForecastBlendType.Month
					{
						weekProf = _SAB.ApplicationServerSession.Calendar.GetWeek(weekRID);
						perProf = _SAB.ApplicationServerSession.Calendar.CurrentWeek.Period;

						if ((weekProf.Period.FiscalYear < perProf.FiscalYear) ||
							(weekProf.Period.FiscalYear == perProf.FiscalYear && weekProf.Period.FiscalPeriod < perProf.FiscalPeriod))
						{
							dataVerRID = verProf.ActualVersionRID;
						}
						else if (varProf.VariableType == eVariableType.BegStock &&
							weekProf.Period.FiscalYear == perProf.FiscalYear &&
							weekProf.Period.FiscalPeriod == perProf.FiscalPeriod)
						{
							if (verProf.BlendCurrentByMonth)
							{
								if (weekProf.WeekInPeriod <= Calendar.CurrentWeek.WeekInPeriod)
								{
									dataVerRID = verProf.ActualVersionRID;
								}
								else
								{
									dataVerRID = verProf.ForecastVersionRID;
								}
							}
							else if (weekProf.WeekInPeriod == 1)
							{
								dataVerRID = verProf.ActualVersionRID;
							}
							else
							{
								dataVerRID = verProf.ForecastVersionRID;
							}
						}
						else
						{
							dataVerRID = verProf.ForecastVersionRID;
						}
					}

					return (VersionProfile)MasterVersionProfileList.FindKey(dataVerRID);
					//End Track #5904 - JScott - B OTB version missing some actuals for competed month
				}
				else
				{
					return verProf;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5741 - JScott - OTS Forecast Review Combined Version In Store Inventory not appearing for Actuals.
		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given PlanCellReference is an Actual version.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is an Actual version.
		/// </returns>

		public bool isCellActual(PlanCellReference aPlanCellRef)
		{
			int key;

			try
			{
				//Begin Track #5741 - JScott - OTS Forecast Review Combined Version In Store Inventory not appearing for Actuals.
				//key = aPlanCellRef.GetVersionProfile().Key;
				key = aPlanCellRef.GetVersionProfileOfData().Key;
				//End Track #5741 - JScott - OTS Forecast Review Combined Version In Store Inventory not appearing for Actuals.
				return (key == Include.FV_ActualRID || key == Include.FV_ModifiedRID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears the changes in the Cells after a save.
		/// </summary>
		
		public void ClearChanges(PlanProfile aPlanProfile)
		{
			try
			{
				RecurseCubeExisting(new RecurseCubeArguments(new RecurseCallbackDelegate(ClearCellChangesForNode), aPlanProfile));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method is called from the RecurseCubeExisting method in the base Cube.  This method clears the changes in the Cells after a save.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A reference to a CellReference object that identifies the Cell's position in the Cube.
		/// </param>
        /// <param name="aRecurseArguments">
		/// An object that contains arguments passed to the RecurseCubeExisting method that were intended for the Callback routine.  In this case, it is null.
		/// </param>

		public void ClearCellChangesForNode(CellReference aPlanCellRef, RecurseCubeArguments aRecurseArguments)
		{
			try
			{
				PlanProfile planProfile = (PlanProfile)aRecurseArguments.CallbackArguments;
				if (aPlanCellRef[eProfileType.HierarchyNode] == planProfile.NodeProfile.Key)
				{
					((PlanCellReference)aPlanCellRef).ClearCellChanges();
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
