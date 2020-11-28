using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCellReference class defines the interface to the PlanCell/PlanCube relationship for value-type variables.
	/// </summary>
	/// <remarks>
	/// The PlanCellReference defines interface properties and methods that allow the owner to access fields and functionality in the PlanCell
	/// and PlanCube classes of value-type variables.
	/// </remarks>

	public class PlanCellReferenceValueExtension : ComputationCellReferenceValueExtension
	{
		//=======
		// FIELDS
		//=======

		private PlanCellReference _planCellRef;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the PlanCellReference class using the given PlanCube.
		/// </summary>

		public PlanCellReferenceValueExtension(PlanCellReference aPlanCellRef)
			: base(aPlanCellRef)
		{
			_planCellRef = aPlanCellRef;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override internal void InitCellIneligibleFlag()
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

		override internal void InitCellClosedFlag()
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

		override internal void InitCellProtectedFlag()
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
					if (_planCellRef.isCellExtensionCreated)
					{
						_extCell = (ExtensionCell)_planCellRef.PlanCube.GetExtensionCell(_planCellRef);
					}
					else if (aAllocate)
					{
						_extCell = (ExtensionCell)_planCellRef.PlanCube.GetExtensionCell(_planCellRef);
						_planCellRef.isCellExtensionCreated = true;
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
		/// Creates a copy of the PlanCellReference.  The PlanCube is a shallow copy, while the PlanCell is a deep copy.
		/// </summary>
		/// <returns>
		/// A new instance of PlanCellReference with a copy of this objects information.
		/// </returns>

		override public ComputationCellReferenceExtension Copy()
		{
			PlanCellReferenceValueExtension planCellRefValExt;

			try
			{
				planCellRefValExt = new PlanCellReferenceValueExtension(_planCellRef);
				planCellRefValExt.CopyFrom(this);

				return planCellRefValExt;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes the PlanCell.
		/// </summary>

		override public void InitCellValue()
		{
			PlanCell planCell;
			PlanCube planCube;
			PlanCubeGroup planCubeGroup;
			FormulaProfile initFormula = null;
			ComputationScheduleFormulaEntry scheduleEntry = null;
			//Begin Track #5781 - JScott - Can't enter Grs Rec IMU% in current month for OTB version
			//ComputationVariableProfile baseVarProf;
			//VariableProfile varProf;
			//bool protect;
			//End Track #5781 - JScott - Can't enter Grs Rec IMU% in current month for OTB version
			PlanCellReference valueCellRef;
			//Begin Init Performance Benchmarking -- DO NOT REMOVE
			//PerfInitHashEntry perfInitHashEntry;
			//PerfTimer perfTimer;
			//End Init Performance Benchmarking -- DO NOT REMOVE

			try
			{
				planCell = _planCellRef.PlanCell;
				planCube = _planCellRef.PlanCube;
				planCubeGroup = planCube.PlanCubeGroup;

				if (!planCell.isInitialized)
				{
					planCell.isInitialized = true;
					planCell.isCurrentInitialized = true;

					//Begin Track #5669 - JScott - BMU %
					//baseVarProf = _planCellRef.GetVariableAccessVariableProfile();
					//if (baseVarProf != null)
					//{
					//    if (baseVarProf.VariableAccess == eVariableAccess.DisplayOnly)
					//    {
					//        planCell.isDisplayOnly = true;
					//    }
					//}

					//End Track #5669 - JScott - BMU %
					if (_planCellRef[eProfileType.QuantityVariable] != planCube.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key)
					{
						valueCellRef = (PlanCellReference)planCube.CreateCellReference(_planCellRef);
						valueCellRef[eProfileType.QuantityVariable] = planCube.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

						planCell.isIneligible = valueCellRef.isCellIneligible;
						planCell.isClosed = valueCellRef.isCellClosed;
						planCell.isProtected = valueCellRef.isCellProtected;
						planCell.isDisplayOnly = valueCellRef.isCellDisplayOnly;
					}
					else
					{
						planCell.isIneligible = planCube.isStoreIneligible(_planCellRef);
						planCell.isClosed = planCube.isStoreClosed(_planCellRef);
						//Begin Track #5781 - JScott - Can't enter Grs Rec IMU% in current month for OTB version

						//protect = planCube.isVersionProtected(_planCellRef);
						//varProf = (VariableProfile)planCube.MasterVariableProfileList.FindKey(_planCellRef[eProfileType.Variable]);

						////Begin Track #5669 - JScott - BMU %
						////if (varProf.DatabaseColumnName != null)
						//planCell.isDisplayOnly = planCube.isDisplayOnly(_planCellRef);

						//if (_planCellRef.PlanCube.isDatabaseVariable(varProf, _planCellRef))
						////End Track #5669 - JScott - BMU %
						//{
						//    planCell.isProtected = protect;
						//}
						//else
						//{
						//    planCell.isDisplayOnly |= protect;
						//}
						planCell.isDisplayOnly = planCube.isDisplayOnly(_planCellRef);
						planCell.isProtected = planCube.isVersionProtected(_planCellRef);
						//End Track #5781 - JScott - Can't enter Grs Rec IMU% in current month for OTB version

						//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
						//if (!planCell.isIneligible && !planCell.isLoadedFromDB)
						if (!planCell.isLoadedFromDB && (_planCellRef.isCellActual || !planCell.isIneligible))
						//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
						{
							planCube.ReadCell(_planCellRef);
						}
					}

					initFormula = planCube.GetInitFormulaProfile(_planCellRef);

					if (initFormula != null)
					{
						scheduleEntry = _planCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
						//Begin Init Performance Benchmarking -- DO NOT REMOVE

						//perfInitHashEntry = (PerfInitHashEntry)_planCellRef.PlanCube.PlanCubeGroup.PerfInitHash[scheduleEntry.FormulaSpreadProfile.Name];

						//if (perfInitHashEntry == null)
						//{
						//    perfInitHashEntry = new PerfInitHashEntry(_planCellRef.PlanCube.PlanCubeGroup.PerfInitHash, scheduleEntry.FormulaSpreadProfile);
						//    _planCellRef.PlanCube.PlanCubeGroup.PerfInitHash[scheduleEntry.FormulaSpreadProfile.Name] = perfInitHashEntry;
						//}

						//perfTimer = perfInitHashEntry.StartTimer();

						//End Init Performance Benchmarking -- DO NOT REMOVE
						initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.PostInit, eSetCellMode.Initialize, "PlanCellReferenceValueExtension::InitCellValue::1");
						//Begin Init Performance Benchmarking -- DO NOT REMOVE

						//perfInitHashEntry.StopTimer(perfTimer, PerfStopType.Completed);
						//End Init Performance Benchmarking -- DO NOT REMOVE

						//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						////Begin Enhancement - JScott - Add Balance Low Levels functionality
						////if (!planCell.isIneligible && planCubeGroup.UserChanged && planCube.GetAutototalChangeMethodProfile(_planCellRef) != null)
						//if (!planCell.isIneligible &&
						//    ((planCubeGroup.UserChanged && planCube.GetAutototalChangeMethodProfile(_planCellRef) != null) ||
						//    planCubeGroup.ForceCurrentInit))
						////End Enhancement - JScott - Add Balance Low Levels functionality
						if (!planCell.isIneligible && 
							((planCubeGroup.UserChanged && planCube.GetAutototalChangeMethodProfile(_planCellRef) != null) || planCubeGroup.ForceCurrentInit) &&
							_planCellRef.canCellBeScheduled)
						//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						{
							//Begin Init Performance Benchmarking -- DO NOT REMOVE
							//perfTimer = perfInitHashEntry.StartTimer();

							//End Init Performance Benchmarking -- DO NOT REMOVE
							//Begin Track #5665 - JScott - Change Calculation of STS ratio by week
							//initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, "PlanCellReferenceValueExtension::InitCellValue::2");
							initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Previous, eSetCellMode.InitializeCurrent, "PlanCellReferenceValueExtension::InitCellValue::2");
							//End Track #5665 - JScott - Change Calculation of STS ratio by week
							//Begin Init Performance Benchmarking -- DO NOT REMOVE

							//perfInitHashEntry.StopTimer(perfTimer, PerfStopType.Completed);
							//End Init Performance Benchmarking -- DO NOT REMOVE
						}
					}

					//Begin Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
					//if (planCell.isIneligible && !planCell.isNull)
					if (!planCell.isNull && !_planCellRef.isCellActual && planCell.isIneligible)
					//End Track #6226 - JScott - Modified version not honoring similiar store in OTS forecast review
					{
						SetCompCellValue(eSetCellMode.Initialize, 0);
					}

//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis
//					InitPeriodTotalLock(_planCellRef);
					planCube.InitCellValue(_planCellRef);
//End Track #4309 - JScott - Onhand incorrect for multiple detail basis
				}
				else if (!planCell.isCurrentInitialized)
				{
					planCell.isCurrentInitialized = true;

					//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					////if (!planCell.isIneligible && planCubeGroup.UserChanged && planCube.GetAutototalChangeMethodProfile(_planCellRef) != null)
					//if (!planCell.isIneligible &&
					//    ((planCubeGroup.UserChanged && planCube.GetAutototalChangeMethodProfile(_planCellRef) != null) ||
					//    planCubeGroup.ForceCurrentInit))
					////End Enhancement - JScott - Add Balance Low Levels functionality
					if (!planCell.isIneligible && 
						((planCubeGroup.UserChanged && planCube.GetAutototalChangeMethodProfile(_planCellRef) != null) || planCubeGroup.ForceCurrentInit) &&
						_planCellRef.canCellBeScheduled)
					//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
					{
						initFormula = planCube.GetInitFormulaProfile(_planCellRef);

						if (initFormula != null)
						{
							scheduleEntry = _planCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
							//Begin Track #5665 - JScott - Change Calculation of STS ratio by week
							//initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, "PlanCellReferenceValueExtension::InitCellValue::2");
							initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Previous, eSetCellMode.InitializeCurrent, "PlanCellReferenceValueExtension::InitCellValue::2");
							//End Track #5665 - JScott - Change Calculation of STS ratio by week
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
		//Begin Track #5712 - JScott - Issue with calcs with copy method - Part 2
		/// <summary>
		/// Reads the ComputationCell.
		/// </summary>

		override public void ReadCellValue()
		{
			try
			{
				if (!_planCellRef.PlanCell.isLoadedFromDB)
				{
					_planCellRef.PlanCube.ReadCell(_planCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5712 - JScott - Issue with calcs with copy method - Part 2
	}
}
