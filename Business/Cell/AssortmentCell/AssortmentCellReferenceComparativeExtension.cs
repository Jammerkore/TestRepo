using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The AssortmentCellReference class defines the interface to the AssortmentCell/AssortmentCube relationship for comparative-type variables.
	/// </summary>
	/// <remarks>
	/// The AssortmentCellReference defines interface properties and methods that allow the owner to access fields and functionality in the AssortmentCell
	/// and AssortmentCube classes of comparative-type variables.
	/// </remarks>

	public class AssortmentCellReferenceComparativeExtension : ComputationCellReferenceComparativeExtension
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

		public AssortmentCellReferenceComparativeExtension(AssortmentCellReference aAssortmentCellRef)
			: base(aAssortmentCellRef)
		{
			_assortmentCellRef = aAssortmentCellRef;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the value of the IneligibleInited flag.
		/// </summary>

		private bool isCellByCubeInited
		{
			get
			{
				try
				{
					return ((_initFlags & ComparativeExtensionFlagValues.CellByCubeInited) > 0);
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
						_initFlags = (byte)(_initFlags | ComparativeExtensionFlagValues.CellByCubeInited);
					}
					else
					{
						_initFlags = (byte)(_initFlags & ~ComparativeExtensionFlagValues.CellByCubeInited);
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

		override internal void InitCellBlockedFlag()
		{
			try
			{
				intInitCellValueByCube();
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
				intInitCellValueByCube();
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
		/// Creates a copy of the AssortmentCellReferenceExtension.
		/// </summary>
		/// <returns>
		/// A new instance of AssortmentCellReferenceExtension with a copy of this objects information.
		/// </returns>

		override public ComputationCellReferenceExtension Copy()
		{
			AssortmentCellReferenceComparativeExtension assortmentCellRefCompExt;

			try
			{
				assortmentCellRefCompExt = new AssortmentCellReferenceComparativeExtension(_assortmentCellRef);
				assortmentCellRefCompExt.CopyFrom(this);

				return assortmentCellRefCompExt;
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
			FormulaProfile initFormula;
			ComputationScheduleFormulaEntry scheduleEntry;

			try
			{
				if (!_assortmentCellRef.AssortmentCell.isInitialized)
				{
					_assortmentCellRef.AssortmentCell.isInitialized = true;

					InitCellDisplayOnlyFlag();

					initFormula = _assortmentCellRef.AssortmentCube.GetInitFormulaProfile(_assortmentCellRef);

					if (initFormula != null)
					{
						scheduleEntry = _assortmentCellRef.CreateScheduleFormulaEntry(null, initFormula, eComputationScheduleEntryType.Formula, 0, 0);
						initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.PostInit, eSetCellMode.Initialize, "AssortmentCellReferenceComparativeExtension::InitCellValue::1");

						//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						//if (_assortmentCellRef.AssortmentCube.AssortmentCubeGroup.UserChanged)
						if (_assortmentCellRef.AssortmentCube.AssortmentCubeGroup.UserChanged &&
							_assortmentCellRef.canCellBeScheduled)
						//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
						{
							//Begin Track #5665 - JScott - Change Calculation of STS ratio by week
							//initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Current, eSetCellMode.InitializeCurrent, "AssortmentCellReferenceComparativeExtension::InitCellValue::2");
							initFormula.ExecuteCalc(scheduleEntry, eGetCellMode.Previous, eSetCellMode.InitializeCurrent, "AssortmentCellReferenceComparativeExtension::InitCellValue::2");
							//End Track #5665 - JScott - Change Calculation of STS ratio by week
						}
					}

					intInitCellValueByCube();
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
		/// <summary>
		/// Sets the Value of a ComputationCell's block flag.
		/// </summary>
        /// <param name="aBlock">
		/// A boolean containing the value of the flag.
		/// </param>

		override public void SetCellBlock(bool aBlock)
		{
			try
			{
				InitCellValue();

				if (!_assortmentCellRef.AssortmentCell.isNull)
				{
					if (aBlock != _assortmentCellRef.AssortmentCell.isBlocked)
					{
						_assortmentCellRef.AssortmentCell.isBlocked = aBlock;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void intInitCellValueByCube()
		{
			try
			{
				if (!isCellByCubeInited)
				{
					isCellByCubeInited = true;
					_assortmentCellRef.AssortmentCube.InitCellValue(_assortmentCellRef);
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
