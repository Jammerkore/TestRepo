using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentCube is an cube used to store Assortment values.
	/// </summary>

	abstract public class AssortmentCube : ComputationCube
	{
		//=======
		// FIELDS
		//=======

		private int _colorDimIdx;
		private int _packDimIdx;
		private ProfileList _masterAssortmentSummaryVariableProfileList;
		private ProfileList _masterAssortmentTotalVariableProfileList;
		private ProfileList _masterAssortmentDetailVariableProfileList;
		private ProfileList _masterAssortmentQuantityVariableProfileList;
		private bool _loadValuesFromHeader;	// TT#2 - stodd - assortment

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentCube.
		/// </param>

		public AssortmentCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, aCubeAttributes, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			try
			{
				_colorDimIdx = int.MaxValue;
				_packDimIdx = int.MaxValue;
				_loadValuesFromHeader = false;	// TT#2 - stodd - assortment
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
		/// Returns the eProfileType of the Header dimension
		/// </summary>

		abstract public eProfileType HeaderProfileType { get; }

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
				return eProfileType.AssortmentQuantityVariable;
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
				return MasterAssortmentQuantityVariableProfileList;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given PlanCellReference is displayable.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool isCellDisplayable(ComputationCellReference aCompCellRef)
		{
			return true;
		}

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database.
		/// </summary>
		/// <param name="aVarProf">
		/// The VariableProfile containing the variable to inspect
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		override public bool isDatabaseVariable(ComputationVariableProfile aVarProf, ComputationCellReference aPlanCellRef)
		{
			return aVarProf.isDatabaseVariable(eVariableCategory.Store, -1, eCalendarDateType.Week);
		}

		/// <summary>
		/// Gets a boolean indicating if the HeaderPackColor dimension is defined.
		/// </summary>

		public bool isColorDefined
		{
			get
			{
				if (_colorDimIdx == int.MaxValue)
				{
					_colorDimIdx = _cubeDef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor);
				}

				return _colorDimIdx != -1;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if the HeaderPack dimension is defined.
		/// </summary>

		public bool isPackDefined
		{
			get
			{
				if (_packDimIdx == int.MaxValue)
				{
					_packDimIdx = _cubeDef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack);
				}

				return _packDimIdx != -1;
			}
		}

		public bool hasCubeChanged()
		{
			CheckCellChangedParms chngdParms;

			try
			{
				chngdParms = new CheckCellChangedParms();

				RecurseCubeExisting(new RecurseCubeArguments(new RecurseCallbackDelegate(CheckCellChanged), chngdParms));

				return chngdParms.Changed;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets a reference to the AssortmentCubeGroup.
		/// </summary>

		public AssortmentCubeGroup AssortmentCubeGroup
		{
			get
			{
				return (AssortmentCubeGroup)_cubeGroup;
			}
		}

		/// <summary>
		/// Gets the master AssortmentSummaryVariable ProfileList.
		/// </summary>

		public ProfileList MasterAssortmentSummaryVariableProfileList
		{
			get
			{
				if (_masterAssortmentSummaryVariableProfileList == null)
				{
					_masterAssortmentSummaryVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.AssortmentSummaryVariable);
				}

				return _masterAssortmentSummaryVariableProfileList;
			}
		}

		/// <summary>
		/// Gets the master AssortmentTotalVariable ProfileList.
		/// </summary>

		public ProfileList MasterAssortmentTotalVariableProfileList
		{
			get
			{
				if (_masterAssortmentTotalVariableProfileList == null)
				{
					_masterAssortmentTotalVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.AssortmentTotalVariable);
				}

				return _masterAssortmentTotalVariableProfileList;
			}
		}

		/// <summary>
		/// Gets the master AssortmentDetailVariable ProfileList.
		/// </summary>

		public ProfileList MasterAssortmentDetailVariableProfileList
		{
			get
			{
				if (_masterAssortmentDetailVariableProfileList == null)
				{
					_masterAssortmentDetailVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.AssortmentDetailVariable);
				}

				return _masterAssortmentDetailVariableProfileList;
			}
		}

		/// <summary>
		/// Gets the master AssortmentQuantityVariable ProfileList.
		/// </summary>

		public ProfileList MasterAssortmentQuantityVariableProfileList
		{
			get
			{
				if (_masterAssortmentQuantityVariableProfileList == null)
				{
					_masterAssortmentQuantityVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.AssortmentQuantityVariable);
				}

				return _masterAssortmentQuantityVariableProfileList;
			}
		}

		// Begin TT#2 - stodd - assortment

		/// <summary>
		/// Gets whether the cell values should be loaded from a header or not
		/// </summary>
		/// 
		public bool LoadValuesFromHeader
		{
			get
			{
				return _loadValuesFromHeader;
			}
			set
			{
				_loadValuesFromHeader = value;
			}
		}

		// End TT#2 - stodd - assortment

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that initializes the Cube.
		/// </summary>

		abstract public void InitializeCube();

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Abstract method that returns the eCubeType of the corresponding Pack cube for this cube.
        ///// </summary>
        ///// <returns>
        ///// The eCubeType of the Sub-total cube.
        ///// </returns>

        //abstract public eCubeType GetDetailPackCubeType();

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Color cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		abstract public eCubeType GetDetailColorCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Level Total cube.
		/// </returns>

		abstract public eCubeType GetComponentGroupLevelCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Total cube.
		/// </returns>

		abstract public eCubeType GetComponentTotalCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Placeholder cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		abstract public eCubeType GetComponentPlaceholderCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		abstract public eCubeType GetComponentHeaderCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		abstract public eCubeType GetSummaryCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		abstract public eCubeType GetSubTotalCubeType();

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		abstract public eCubeType GetTotalCubeType();

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a Cell is read.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A AssortmentCellReference object that identifies the AssortmentCubeCell to read.
		/// </param>

		abstract public void ReadCell(AssortmentCellReference aAssrtCellRef);

		/// <summary>
		/// Creates a new instance of a AssortmentCubeCell.
		/// </summary>
		/// <returns>
		/// A reference to a new AssortmentCubeCell.
		/// </returns>

		override public Cell CreateCell(CellReference aCellReference)
		{
			AssortmentCell assrtCell;

			try
			{
				assrtCell = new AssortmentCell();

				if (ReadOnly)
				{
					assrtCell.isReadOnly = true;
				}

				return assrtCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a AssortmentCellReference.
		/// </summary>
		/// <returns>
		/// A reference to a new AssortmentCellReference.
		/// </returns>

		override public CellReference CreateCellReference()
		{
			try
			{
				return new AssortmentCellReference(this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a AssortmentCellReference using the given CellCoordinates.
		/// </summary>
		/// <param name="aCoordinates">
		/// The CellCoordinates object that describes the AssortmentCubeCell's position in this AssortmentCube.
		/// </param>
		/// <returns>
		/// A reference to a new AssortmentCellReference.
		/// </returns>

		override public CellReference CreateCellReference(CellCoordinates aCoordinates)
		{
			try
			{
				return new AssortmentCellReference(this, aCoordinates);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of a AssortmentCellReference using the given CellReference.
		/// </summary>
		/// <param name="aCellRef">
		/// The CellReference object that describes the AssortmentCubeCell's position in this AssortmentCube.  This indices in this object will be translated to 
		/// cooresponding coordinates in this AssortmentCube.
		/// </param>
		/// <returns>
		/// A reference to a new AssortmentCellReference.
		/// </returns>

		override public CellReference CreateCellReference(CellReference aCellRef)
		{
			try
			{
				return new AssortmentCellReference(this, aCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns a boolean indicating if the cell referenced can be scheduled for calculation.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool canCellBeScheduled(ComputationCellReference aCompCellRef)
		{
			return true;
		}

		//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given ComputationCellReference is valid to display.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is valid to display.
		/// </returns>

		override public bool isCellValid(ComputationCellReference aCompCellRef)
		{
			try
			{
				if (((AssortmentCellReference)aCompCellRef).isCellBlocked)
				{
					return true;
				}
				else
				{
					return base.isCellValid(aCompCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method is called from the RecurseCubeExisting method in the base Cube.  This method saves and clears the changed cells.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A reference to a CellReference object that identifies the Cell's position in the Cube.
		/// </param>
        /// <param name="aRecurseArguments">
		/// An object that contains arguments passed to the RecurseCubeExisting method that were intended for the Callback routine.  In this case, it is null.
		/// </param>

		public void CheckCellChanged(CellReference aAssrtCellRef, RecurseCubeArguments aRecurseArguments)
		{
			AssortmentCellReference cellRef;

			try
			{
				cellRef = (AssortmentCellReference)aAssrtCellRef;

				if (((AssortmentVariableProfile)cellRef.GetCalcVariableProfile()).DatabaseColumnName != null && cellRef.isCellChanged)
				{
					((CheckCellChangedParms)aRecurseArguments.CallbackArguments).Changed = true;
					aRecurseArguments.Cancel = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Allows a cube to specify custom initializations for a Cell.  Occurs after the standard Cell initialization.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to initialize.
		/// </param>

		public override void InitCellValue(ComputationCellReference aCompCellRef)
		{
			try
			{
				aCompCellRef.SumDetailCellLocks();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// Begin TT#3809 - stodd - Locked Cell doesn't save when processing Need - 
        public void SumDetailCellLocks(ComputationCellReference aCompCellRef)
        {
            try
            {
                aCompCellRef.SumDetailCellLocks();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#3809 - stodd - Locked Cell doesn't save when processing Need - 
	}

	public class CheckCellChangedParms
	{
		//=======
		// FIELDS
		//=======

		private bool _changed;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		public bool Changed
		{
			get
			{
				return _changed;
			}
			set
			{
				_changed = value;
			}
		}
	}
}