using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanCellReference class defines the interface to the PlanCell/PlanCube relationship for comparative-type variables.
	/// </summary>
	/// <remarks>
	/// The PlanCellReference defines interface properties and methods that allow the owner to access fields and functionality in the PlanCell
	/// and PlanCube classes of comparative-type variables.
	/// </remarks>

	public class PlanCellReferenceComparativeExtension : ComputationCellReferenceComparativeExtension
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

		public PlanCellReferenceComparativeExtension(PlanCellReference aPlanCellRef)
			: base(aPlanCellRef)
		{
			_planCellRef = aPlanCellRef;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the IneligibleInited flag.
		/// </summary>

		private bool isIneligibleInited
		{
			get
			{
				try
				{
					return ((_initFlags & ComparativeExtensionFlagValues.IneligibleInited) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_initFlags = (byte)(_initFlags | ComparativeExtensionFlagValues.IneligibleInited);
					}
					else
					{
						_initFlags = (byte)(_initFlags & ~ComparativeExtensionFlagValues.IneligibleInited);
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
		/// Gets or sets the value of the ClosedInited flag.
		/// </summary>

		private bool isClosedInited
		{
			get
			{
				try
				{
					return ((_initFlags & ComparativeExtensionFlagValues.ClosedInited) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_initFlags = (byte)(_initFlags | ComparativeExtensionFlagValues.ClosedInited);
					}
					else
					{
						_initFlags = (byte)(_initFlags & ~ComparativeExtensionFlagValues.ClosedInited);
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
		/// Gets or sets the value of the ProtectedInited flag.
		/// </summary>

		private bool isProtectedInited
		{
			get
			{
				try
				{
					return ((_initFlags & ComparativeExtensionFlagValues.ProtectedInited) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_initFlags = (byte)(_initFlags | ComparativeExtensionFlagValues.ProtectedInited);
					}
					else
					{
						_initFlags = (byte)(_initFlags & ~ComparativeExtensionFlagValues.ProtectedInited);
					}
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
		/// Initializes this Cell's ineligible flag.
		/// </summary>

		override internal void InitCellIneligibleFlag()
		{
			try
			{
				if (!isIneligibleInited)
				{
					isIneligibleInited = true;
					_planCellRef.PlanCell.isIneligible = _planCellRef.PlanCube.isStoreIneligible(_planCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes this Cell's closed flag.
		/// </summary>

		override internal void InitCellClosedFlag()
		{
			try
			{
				if (!isClosedInited)
				{
					isClosedInited = true;
					_planCellRef.PlanCell.isClosed = _planCellRef.PlanCube.isStoreClosed(_planCellRef);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initializes this Cell's protected flag.
		/// </summary>

		override internal void InitCellProtectedFlag()
		{
			try
			{
				if (!isProtectedInited)
				{
					isProtectedInited = true;
					_planCellRef.PlanCell.isProtected = _planCellRef.PlanCube.isVersionProtected(_planCellRef);
				}
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
				if (_extCell == null && aAllocate)
				{
					_extCell = new ExtensionCell();
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
		/// Creates a copy of the PlanCellReferenceExtension.
		/// </summary>
		/// <returns>
		/// A new instance of PlanCellReferenceExtension with a copy of this objects information.
		/// </returns>

		override public ComputationCellReferenceExtension Copy()
		{
			PlanCellReferenceComparativeExtension planCellRefCompExt;

			try
			{
				planCellRefCompExt = new PlanCellReferenceComparativeExtension(_planCellRef);
				planCellRefCompExt.CopyFrom(this);

				return planCellRefCompExt;
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
			FormulaProfile initFormula;
			ComputationScheduleFormulaEntry scheduleEntry;

			try
			{
				if (!_planCellRef.PlanCell.isInitialized)
				{
					_planCellRef.PlanCell.isInitialized = true;

					InitCellIneligibleFlag();
					InitCellClosedFlag();
					InitCellProtectedFlag();
					InitCellDisplayOnlyFlag();

					initFormula = _planCellRef.PlanCube.GetInitFormulaProfile(_planCellRef);

					if (initFormula != null)
					{
						scheduleEntry = _planCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
						initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.PostInit, eSetCellMode.Initialize, "PlanCellReferenceComparativeExtension::InitCellValue::1");

						//Begin TT#80 - JScott - % Change to Plan incorrect for variables XFER, XFER Cst
						////Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						//////Begin Enhancement - JScott - Add Balance Low Levels functionality
						//////if (!_planCellRef.PlanCell.isIneligible && _planCellRef.PlanCube.PlanCubeGroup.UserChanged)
						////if (!_planCellRef.PlanCell.isIneligible && (_planCellRef.PlanCube.PlanCubeGroup.UserChanged || _planCellRef.PlanCube.PlanCubeGroup.ForceCurrentInit))
						//////End Enhancement - JScott - Add Balance Low Levels functionality
						//if (!_planCellRef.PlanCell.isIneligible && 
						//    (_planCellRef.PlanCube.PlanCubeGroup.UserChanged || _planCellRef.PlanCube.PlanCubeGroup.ForceCurrentInit) &&
						//    _planCellRef.canCellBeScheduled)
						////End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						if (!_planCellRef.PlanCell.isIneligible &&
							(_planCellRef.PlanCube.PlanCubeGroup.UserChanged || _planCellRef.PlanCube.PlanCubeGroup.ForceCurrentInit))
						//End TT#80 - JScott - % Change to Plan incorrect for variables XFER, XFER Cst
						{
							//Begin Track #5665 - JScott - Change Calculation of STS ratio by week
							//initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, "PlanCellReferenceComparativeExtension::InitCellValue::2");
							initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Previous, eSetCellMode.InitializeCurrent, "PlanCellReferenceComparativeExtension::InitCellValue::2");
							//End Track #5665 - JScott - Change Calculation of STS ratio by week
						}
					}

//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis
//					InitPeriodTotalLock(_planCellRef);
					_planCellRef.PlanCube.InitCellValue(_planCellRef);
//End Track #4309 - JScott - Onhand incorrect for multiple detail basis
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
		}

		//End Track #5712 - JScott - Issue with calcs with copy method - Part 2
	}
}
