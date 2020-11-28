using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentCellReference class defines the interface to the AssortmentCell/AssortmentCube relationship for value-type variables.
	/// </summary>
	/// <remarks>
	/// The AssortmentCellReference defines interface properties and methods that allow the owner to access fields and functionality in the AssortmentCell
	/// and AssortmentCube classes of value-type variables.
	/// </remarks>

	public class AssortmentCellReferenceValueExtension : ComputationCellReferenceValueExtension
	{
		//=======
		// FIELDS
		//=======

		private AssortmentCellReference _assortmentCellRef;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the AssortmentCellReference class using the given AssortmentCube.
		/// </summary>

		public AssortmentCellReferenceValueExtension(AssortmentCellReference aAssortmentCellRef)
			: base(aAssortmentCellRef)
		{
			_assortmentCellRef = aAssortmentCellRef;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override internal void InitCellBlockedFlag()
		{
			try
			{
				InitCellValue();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override internal void InitCellFixedFlag()
		{
			try
			{
				InitCellValue();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override internal ExtensionCell GetExtensionCell(bool aAllocate)
		{
			try
			{
				if (_extCell == null)
				{
					if (_assortmentCellRef.isCellExtensionCreated)
					{
						_extCell = (ExtensionCell)_assortmentCellRef.AssortmentCube.GetExtensionCell(_assortmentCellRef);
					}
					else if (aAllocate)
					{
						_extCell = (ExtensionCell)_assortmentCellRef.AssortmentCube.GetExtensionCell(_assortmentCellRef);
						_assortmentCellRef.isCellExtensionCreated = true;
					}
				}

				return _extCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a copy of the AssortmentCellReference.  The AssortmentCube is a shallow copy, while the AssortmentCell is a deep copy.
		/// </summary>
		/// <returns>
		/// A new instance of AssortmentCellReference with a copy of this objects information.
		/// </returns>

		override public ComputationCellReferenceExtension Copy()
		{
			AssortmentCellReferenceValueExtension assortmentCellRefValExt;

			try
			{
				assortmentCellRefValExt = new AssortmentCellReferenceValueExtension(_assortmentCellRef);
				assortmentCellRefValExt.CopyFrom(this);

				return assortmentCellRefValExt;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes the AssortmentCell.
		/// </summary>

		override public void InitCellValue()
		{
			AssortmentCell assrtCell;
			AssortmentCube assrtCube;
			AssortmentCubeGroup assrtCubeGroup;
			FormulaProfile initFormula = null;
			ComputationScheduleFormulaEntry scheduleEntry = null;
			ComputationVariableProfile baseVarProf;
			AssortmentCellReference valueCellRef;

            bool stop = false;  // TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.
			try
			{
				assrtCell = _assortmentCellRef.AssortmentCell;
				assrtCube = _assortmentCellRef.AssortmentCube;
				assrtCubeGroup = assrtCube.AssortmentCubeGroup;
				// Begin TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.
                // for debugging
                //string cellKeys = ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellKeys;
                //if (!assrtCell.isInitialized)
                //{
                //    if (((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(0) == 5465
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(1) == int.MaxValue
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(2) == 1
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(3) == 200
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(4) == 804651
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(5) == 1
                //        )
                //    {
                //        stop = true;
                //    }
                //    else if (((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(0) == 5465
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(1) == 2342
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(2) == 1
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(3) == 200
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(4) == 804651
                //        && ((MIDRetail.Business.ComputationCellReference)(_assortmentCellRef)).CellCoordinates.GetRawCoordinate(5) == 1
                //        )
                //    {
                //        stop = true;
                //    }
                //}
				// End TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.

				if (!assrtCell.isInitialized)
				{
					assrtCell.isInitialized = true;
					assrtCell.isCurrentInitialized = true;

					baseVarProf = _assortmentCellRef.GetVariableAccessVariableProfile();
					if (baseVarProf != null)
					{
						if (baseVarProf.VariableAccess == eVariableAccess.DisplayOnly)
						{
							assrtCell.isDisplayOnly = true;
						}
					}

					if (_assortmentCellRef[eProfileType.AssortmentQuantityVariable] != assrtCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key)
					{
						valueCellRef = (AssortmentCellReference)assrtCube.CreateCellReference(_assortmentCellRef);
						valueCellRef[eProfileType.AssortmentQuantityVariable] = assrtCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

						assrtCell.isDisplayOnly = valueCellRef.isCellDisplayOnly;
					}
					else
					{
						if (!assrtCell.isLoadedFromDB)
						{
							assrtCube.ReadCell(_assortmentCellRef);
						}
					}

					initFormula = assrtCube.GetInitFormulaProfile(_assortmentCellRef);

					if (initFormula != null)
					{
						scheduleEntry = _assortmentCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
						initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.PostInit, eSetCellMode.Initialize, "AssortmentCellReferenceValueExtension::InitCellValue::1");

						//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						//if (assrtCubeGroup.UserChanged && assrtCube.GetAutototalChangeMethodProfile(_assortmentCellRef) != null)
						if (assrtCubeGroup.UserChanged &&
							assrtCube.GetAutototalChangeMethodProfile(_assortmentCellRef) != null &&
							_assortmentCellRef.canCellBeScheduled)
						//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						{
							//Begin Track #5665 - JScott - Change Calculation of STS ratio by week
							//initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, "AssortmentCellReferenceValueExtension::InitCellValue::2");
							initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Previous, eSetCellMode.InitializeCurrent, "AssortmentCellReferenceValueExtension::InitCellValue::2");
							//End Track #5665 - JScott - Change Calculation of STS ratio by week
						}
					}

					assrtCube.InitCellValue(_assortmentCellRef);
					// Begin TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.
                    // for debugging
                    //if (stop)
                    //{
                    //    bool stop2 = true;
                    //}
					// End TT#1766-MD - JSmith - GA Matrix - Total Section - Select All Display other variables and they are  0.
				}
                else if (!assrtCell.isCurrentInitialized)
                {
                    assrtCell.isCurrentInitialized = true;

                    //Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
                    //if (assrtCubeGroup.UserChanged && assrtCube.GetAutototalChangeMethodProfile(_assortmentCellRef) != null)
                    if (assrtCubeGroup.UserChanged &&
                        assrtCube.GetAutototalChangeMethodProfile(_assortmentCellRef) != null &&
                        _assortmentCellRef.canCellBeScheduled)
                    //End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
                    {
                        initFormula = assrtCube.GetInitFormulaProfile(_assortmentCellRef);

                        if (initFormula != null)
                        {
                            scheduleEntry = _assortmentCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
                            //Begin Track #5665 - JScott - Change Calculation of STS ratio by week
                            //initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, "AssortmentCellReferenceValueExtension::InitCellValue::2");
                            initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Previous, eSetCellMode.InitializeCurrent, "AssortmentCellReferenceValueExtension::InitCellValue::2");
                            //End Track #5665 - JScott - Change Calculation of STS ratio by week
                        }
                    }
                }
                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                else if (assrtCell.isReinit)
                {
					assrtCell.isReinit = false;
					initFormula = assrtCube.GetInitFormulaProfile(_assortmentCellRef);

                    if (initFormula != null)
                    {
                        scheduleEntry = _assortmentCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
                        initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.Initialize, "AssortmentCellReferenceValueExtension::InitCellValue::3");
                    }
                }
                //End TT#2 - JScott - Assortment Planning - Phase 2
            }
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Reads the ComputationCell.
		/// </summary>

		override public void ReadCellValue()
		{
			try
			{
				if (!_assortmentCellRef.AssortmentCell.isLoadedFromDB)
				{
					_assortmentCellRef.AssortmentCube.ReadCell(_assortmentCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Sets the Value of a ComputationCell's block flag.
		/// </summary>
        /// <param name="aBlock">
		/// A boolean containing the new block flag.
		/// </param>

		override public void SetCellBlock(bool aBlock)
		{
			try
			{
			    // Begin TT#1954-MD - JSmith - Assortment Performance
				// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
				//_assortmentCellRef.AssortmentCube.AssortmentCubeGroup.BackupCellForPendingUndo(_assortmentCellRef.AssortmentCell, (AssortmentCell)_assortmentCellRef.CellCopy(), null, null);
                _assortmentCellRef.AssortmentCube.AssortmentCubeGroup.BackupCellForPendingUndo(_assortmentCellRef.AssortmentCell, (AssortmentCell)_assortmentCellRef.CellCopy(), null, null, eSetCellMode.Entry);
				// End RO-4741 - JSmith - Need to scroll to variables prior to making change
				// End TT#1954-MD - JSmith - Assortment Performance

				InitCellValue();

				if (!_assortmentCellRef.AssortmentCell.isNull)
				{
					if (aBlock != _assortmentCellRef.AssortmentCell.isBlocked)
					{
						// Begin RO-4741 - JSmith - Need to scroll to variables prior to making change
						//_assortmentCellRef.AssortmentCube.AssortmentCubeGroup.BackupCellForUndo(_assortmentCellRef.AssortmentCell, (AssortmentCell)_assortmentCellRef.CellCopy());
						_assortmentCellRef.AssortmentCube.AssortmentCubeGroup.BackupCellForUndo(_assortmentCellRef.AssortmentCell, (AssortmentCell)_assortmentCellRef.CellCopy(), eSetCellMode.Entry);
						// End RO-4741 - JSmith - Need to scroll to variables prior to making change
						_assortmentCellRef.AssortmentCell.isBlocked = aBlock;
						_assortmentCellRef.AssortmentCell.isChanged = true;
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
