using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// A CellSelector that selects Cells depending on the value of their hidden flag and store status.
	/// </summary>

	public class PlanCellStatusSelector : CellSelector
	{
		//=======
		// FIELDS
		//=======

		private eStoreStatus _storeStatus;
		private bool _useHiddenValues;
		ArrayList _cellRefList;

		//=============
		// CONSTRUCTORS
		//=============

		public PlanCellStatusSelector(eStoreStatus aStoreStatus, bool aUseHiddenValues)
		{
			_storeStatus = aStoreStatus;
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
				if (((PlanCellReference)aCellRef).GetStoreStatus() == _storeStatus)
				{
					if (_useHiddenValues || !((PlanCellReference)aCellRef).isCellHidden)
					{
						_cellRefList.Add(aCellRef.Copy());
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

    public class PlanCellSum : CellSelector
    {
        //=======
        // FIELDS
        //=======

		private ComputationScheduleEntry _scheduleEntry;
        private eGetCellMode _getCellMode;
        private eSetCellMode _setCellMode;
        private eStoreStatus _storeStatus;
        private bool _useHiddenValues;
        private double _sum;
        private int _count;
        private PlanCellReference _lastPlanCellReference;

        //=============
        // CONSTRUCTORS
        //=============

        public PlanCellSum(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, eStoreStatus aStoreStatus, bool aUseHiddenValues)
        {
			_scheduleEntry = aScheduleEntry;
            _getCellMode = aGetCellMode;
            _setCellMode = aSetCellMode;
            _storeStatus = aStoreStatus;
            _useHiddenValues = aUseHiddenValues;
            _sum = 0;
            _count = 0;
            _lastPlanCellReference = null;
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

        public PlanCellReference LastPlanCellReference
        {
            get
            {
                return _lastPlanCellReference;
            }
        }

        //========
        // METHODS
        //========

        override public void CheckItem(CellReference aCellRef, out bool aCancel)
        {
            double cellValue;
            PlanCellReference planCellRef;

            try
            {
                planCellRef = (PlanCellReference)aCellRef;

                if (_storeStatus == eStoreStatus.None || planCellRef.GetStoreStatus() == _storeStatus)
                {
                    if (_useHiddenValues || !planCellRef.isCellHidden)
                    {
						if (_scheduleEntry != null &&
                            //Begin TT#2 - JScott - Assortment Planning - Phase 2
                            //(_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal || _setCellMode == eSetCellMode.ForcedReInit) &&
                            (_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal) &&
                            //End TT#2 - JScott - Assortment Planning - Phase 2
                            (planCellRef.isCellFormulaPending(_scheduleEntry) || planCellRef.isCellSpreadPending(_scheduleEntry)))
                        {
                            throw new CellPendingException(planCellRef);
                        }

                        cellValue = planCellRef.GetCellValue(_getCellMode, _useHiddenValues);

                        _sum += cellValue;
                        _count++;
                        _lastPlanCellReference = planCellRef;
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

    public class PlanCellAverageSum : CellSelector
    {
        //=======
        // FIELDS
        //=======

		private ComputationScheduleEntry _scheduleEntry;
		private eGetCellMode _getCellMode;
        private eSetCellMode _setCellMode;
        private eStoreStatus _storeStatus;
        private VariableProfile _varProf;
        private bool _useHiddenValues;
        private double _sum;
        private int _count;
        private int _nonZeroCount;
        private PlanCellReference _lastPlanCellReference;

        //=============
        // CONSTRUCTORS
        //=============

		public PlanCellAverageSum(ComputationScheduleEntry aScheduleEntry, eGetCellMode aGetCellMode, eSetCellMode aSetCellMode, eStoreStatus aStoreStatus, VariableProfile aVarProf, bool aUseHiddenValues)
        {
			_scheduleEntry = aScheduleEntry;
			_getCellMode = aGetCellMode;
            _setCellMode = aSetCellMode;
            _storeStatus = aStoreStatus;
            _varProf = aVarProf;
            _useHiddenValues = aUseHiddenValues;
            _sum = 0;
            _nonZeroCount = 0;
            _lastPlanCellReference = null;
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

        public PlanCellReference LastPlanCellReference
        {
            get
            {
                return _lastPlanCellReference;
            }
        }

        //========
        // METHODS
        //========

        override public void CheckItem(CellReference aCellRef, out bool aCancel)
        {
            double cellValue;
            PlanCellReference planCellRef;
            PlanCellReference otherPlanCellRef;

            try
            {
                planCellRef = (PlanCellReference)aCellRef;

                if (_storeStatus == eStoreStatus.None || planCellRef.GetStoreStatus() == _storeStatus)
                {
                    if (_useHiddenValues || !planCellRef.isCellHidden)
                    {
                        if (!planCellRef.isCellIneligible)
                        {
							if (_scheduleEntry != null &&
                                //Begin TT#2 - JScott - Assortment Planning - Phase 2
                                //(_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal || _setCellMode == eSetCellMode.ForcedReInit) &&
                                (_setCellMode == eSetCellMode.Computation || _setCellMode == eSetCellMode.AutoTotal) &&
                                //End TT#2 - JScott - Assortment Planning - Phase 2
                                (planCellRef.isCellFormulaPending(_scheduleEntry) || planCellRef.isCellSpreadPending(_scheduleEntry)))
							{
                                throw new CellPendingException(planCellRef);
                            }

                            cellValue = planCellRef.GetCellValue(_getCellMode, _useHiddenValues);

							if (cellValue > 0 &&
								(cellValue != 999 || ((VariableProfile)planCellRef.PlanCube.MasterVariableProfileList.FindKey(planCellRef[eProfileType.Variable])).VariableType != eVariableType.FWOS))
							{
                                _sum += cellValue;
                                _nonZeroCount++;
                            }
                            else
                            {
                                if (_varProf != null)
                                {
                                    otherPlanCellRef = (PlanCellReference)planCellRef.Copy();
                                    otherPlanCellRef[eProfileType.Variable] = _varProf.Key;
                                    cellValue = otherPlanCellRef.GetCellValue(_getCellMode, _useHiddenValues);

                                    if (cellValue > 0)
                                    {
                                        _nonZeroCount++;
                                    }
                                }
                            }

                            _count++;
                            _lastPlanCellReference = planCellRef;
                        }
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
}
