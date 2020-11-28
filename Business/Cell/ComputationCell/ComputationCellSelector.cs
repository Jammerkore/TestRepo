using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Diagnostics;

namespace MIDRetail.Business
{
	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellSelector : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private bool _useHiddenValues;
		private ArrayList _cellRefList;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellSelector(bool aUseHiddenValues)
		{
			_useHiddenValues = aUseHiddenValues;
			_cellRefList = new ArrayList();
		}

		//===========
		// PROPERTIES
		//===========

		public ArrayList CellRefList
		{
			get
			{
				return _cellRefList;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				if (_useHiddenValues || !((ComputationCellReference)aCellRef).isCellHidden)
				{
					_cellRefList.Add(aCellRef.Copy());
                    //if (aCellRef is AssortmentCellReference)
                    //{
                    //    Debug.WriteLine("Check Item1 Val" + ((AssortmentCellReference)aCellRef).CurrentCellValue + " " + ((AssortmentCellReference)aCellRef).CellKeys);
                    //}
				}
				aCancel = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellCounter : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private bool _useHiddenValues;
		private int _itemCount;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellCounter(bool aUseHiddenValues)
		{
			_useHiddenValues = aUseHiddenValues;
			_itemCount = 0;
		}

		//===========
		// PROPERTIES
		//===========

		public int ItemCount
		{
			get
			{
				return _itemCount;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				if (_useHiddenValues || !((ComputationCellReference)aCellRef).isCellHidden)
				{
					_itemCount++;
				}
				aCancel = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellSum : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private ComputationScheduleEntry _scheduleEntry;
		private eGetCellMode _getCellMode;
		private eSetCellMode _setCellMode;
		private bool _useHiddenValues;
		private double _sum;
		private int _count;
		private ComputationCellReference _lastCompCellReference;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellSum(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, bool aUseHiddenValues)
		{
			_scheduleEntry = aScheduleEntry;
			_getCellMode = aGetCellMode;
			_setCellMode = aSetCellMode;
			_useHiddenValues = aUseHiddenValues;
			_sum = 0;
			_count = 0;
			_lastCompCellReference = null;
		}

		//===========
		// PROPERTIES
		//===========

		public double Sum
		{
			get
			{
				return _sum;
			}
		}

		public int Count
		{
			get
			{
				return _count;
			}
		}

		public ComputationCellReference LastComputationCellReference
		{
			get
			{
				return _lastCompCellReference;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			double cellValue;
			ComputationCellReference compCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCellRef;

				if (_useHiddenValues || !compCellRef.isCellHidden)
				{
					if (_scheduleEntry != null &&
                        //Begin TT#2 - JScott - Assortment Planning - Phase 2
                        //(_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal || _setCellMode == eSetCellMode.ForcedReInit) &&
                        (_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal) &&
                        //End TT#2 - JScott - Assortment Planning - Phase 2
                        (compCellRef.isCellFormulaPending(_scheduleEntry) || compCellRef.isCellSpreadPending(_scheduleEntry)))
					{
						throw new CellPendingException(compCellRef);
					}

					cellValue = compCellRef.GetCellValue(_getCellMode, _useHiddenValues);
                    //if (compCellRef is AssortmentCellReference)
                    //{
                    //    Debug.WriteLine("Check Item2 Val" + ((AssortmentCellReference)compCellRef).CurrentCellValue + " " + ((AssortmentCellReference)compCellRef).CellKeys + "  " + (int)compCellRef.Cube.CubeType.Id);
                    //}

					_sum += cellValue;
					_count++;
					_lastCompCellReference = compCellRef;
				}
				aCancel = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellAverageSum : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private ComputationScheduleEntry _scheduleEntry;
		private eGetCellMode _getCellMode;
		private eSetCellMode _setCellMode;
		private ComputationVariableProfile _varProf;
		private bool _useHiddenValues;
		private double _sum;
		private int _count;
		private int _nonZeroCount;
		private ComputationCellReference _lastCompCellReference;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellAverageSum(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, ComputationVariableProfile aVarProf, bool aUseHiddenValues)
		{
			_scheduleEntry = aScheduleEntry;
			_getCellMode = aGetCellMode;
			_setCellMode = aSetCellMode;
			_varProf = aVarProf;
			_useHiddenValues = aUseHiddenValues;
			_sum = 0;
			_nonZeroCount = 0;
			_lastCompCellReference = null;
		}

		//===========
		// PROPERTIES
		//===========

		public double Sum
		{
			get
			{
				return _sum;
			}
		}

		public int Count
		{
			get
			{
				return _count;
			}
		}

		public int NonZeroCount
		{
			get
			{
				return _nonZeroCount;
			}
		}

		public ComputationCellReference LastComputationCellReference
		{
			get
			{
				return _lastCompCellReference;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			double cellValue;
			ComputationCellReference compCellRef;
			ComputationCellReference otherCompCellRef;

			try
			{
				compCellRef = (ComputationCellReference)aCellRef;

				if (_useHiddenValues || !compCellRef.isCellHidden)
				{
					if (!compCellRef.isCellIneligible && !compCellRef.isCellBlocked)
					{
						if (_scheduleEntry != null &&
                            //Begin TT#2 - JScott - Assortment Planning - Phase 2
                            //(_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal || _setCellMode == eSetCellMode.ForcedReInit) &&
                            (_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal) &&
                            //End TT#2 - JScott - Assortment Planning - Phase 2
                            (compCellRef.isCellFormulaPending(_scheduleEntry) || compCellRef.isCellSpreadPending(_scheduleEntry)))
						{
							throw new CellPendingException(compCellRef);
						}

						cellValue = compCellRef.GetCellValue(_getCellMode, _useHiddenValues);

						if (cellValue > 0)
						{
							_sum += cellValue;
							_nonZeroCount++;
						}
						else
						{
							if (_varProf != null)
							{
								otherCompCellRef = (ComputationCellReference)compCellRef.Copy();
								otherCompCellRef[eProfileType.Variable] = _varProf.Key;
								cellValue = otherCompCellRef.GetCellValue(_getCellMode, _useHiddenValues);

								if (cellValue > 0)
								{
									_nonZeroCount++;
								}
							}
						}

						_count++;
						_lastCompCellReference = compCellRef;
					}
				}
				aCancel = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellFirstSelector : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private bool _useHiddenValues;
		private ComputationCellReference _firstCompCellRef;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellFirstSelector(bool aUseHiddenValues)
		{
			_useHiddenValues = aUseHiddenValues;
			_firstCompCellRef = null;
		}

		//===========
		// PROPERTIES
		//===========

		public ComputationCellReference FirstComputationCellReference
		{
			get
			{
				return _firstCompCellRef;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				if (_firstCompCellRef == null && (_useHiddenValues || !((ComputationCellReference)aCellRef).isCellHidden))
				{
					_firstCompCellRef = (ComputationCellReference)aCellRef.Copy();
				}
				aCancel = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellLastSelector : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private bool _useHiddenValues;
		private ComputationCellReference _lastCompCellRef;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellLastSelector(bool aUseHiddenValues)
		{
			_useHiddenValues = aUseHiddenValues;
			_lastCompCellRef = null;
		}

		//===========
		// PROPERTIES
		//===========

		public ComputationCellReference LastCompCellReference
		{
			get
			{
				return _lastCompCellRef;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				if (_useHiddenValues || !((ComputationCellReference)aCellRef).isCellHidden)
				{
					_lastCompCellRef = (ComputationCellReference)aCellRef;
				}
				aCancel = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellStoreClosed : CellSelector
	{
		//=======
		// FIELDS
		//=======

		bool _isStoreClosed;
		bool _itemChecked;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellStoreClosed()
		{
			_isStoreClosed = true;
			_itemChecked = false;
		}

		//===========
		// PROPERTIES
		//===========

		public bool isStoreClosed
		{
			get
			{
				return _itemChecked && _isStoreClosed;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				_itemChecked = true;

				if (!((ComputationCellReference)aCellRef).isCellClosed)
				{
					_isStoreClosed = false;
					aCancel = true;
				}
				else
				{
					aCancel = false;
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
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellStoreIneligible : CellSelector
	{
		//=======
		// FIELDS
		//=======

		bool _isStoreIneligible;
		bool _itemChecked;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellStoreIneligible()
		{
			_isStoreIneligible = true;
			_itemChecked = false;
		}

		//===========
		// PROPERTIES
		//===========

		public bool isStoreIneligible
		{
			get
			{
				return _itemChecked && _isStoreIneligible;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				_itemChecked = true;

				if (!((ComputationCellReference)aCellRef).isCellIneligible)
				{
					_isStoreIneligible = false;
					aCancel = true;
				}
				else
				{
					aCancel = false;
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
	/// A CellSelector that selects Cells depending on the value of their hidden flag.
	/// </summary>

	public class ComputationCellVersionProtected : CellSelector
	{
		//=======
		// FIELDS
		//=======

		bool _isVersionProtected;
		bool _itemChecked;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellVersionProtected()
		{
			_isVersionProtected = true;
			_itemChecked = false;
		}

		//===========
		// PROPERTIES
		//===========

		public bool isVersionProtected
		{
			get
			{
				return _itemChecked && _isVersionProtected;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				_itemChecked = true;

				if (!((ComputationCellReference)aCellRef).isCellProtected)
				{
					_isVersionProtected = false;
					aCancel = true;
				}
				else
				{
					aCancel = false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	//Begin Track #5669 - JScott - BMU %

	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their display-only flag.
	/// </summary>

	public class ComputationCellDisplayOnly : CellSelector
	{
		//=======
		// FIELDS
		//=======

		bool _isDisplayOnly;
		bool _itemChecked;

		//=============
		// CONSTRUCTORS
		//=============

		public ComputationCellDisplayOnly()
		{
			_isDisplayOnly = true;
			_itemChecked = false;
		}

		//===========
		// PROPERTIES
		//===========

		public bool isDisplayOnly
		{
			get
			{
				return _itemChecked && _isDisplayOnly;
			}
		}

		//========
		// METHODS
		//========

		override public void CheckItem(CellReference aCellRef, out bool aCancel)
		{
			try
			{
				_itemChecked = true;

				if (!((ComputationCellReference)aCellRef).isCellDisplayOnly)
				{
					_isDisplayOnly = false;
					aCancel = true;
				}
				else
				{
					aCancel = false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	//End Track #5669 - JScott - BMU %
}
