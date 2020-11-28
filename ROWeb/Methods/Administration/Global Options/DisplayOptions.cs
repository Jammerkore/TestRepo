using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetProductDisplayList()
        {
            MIDTextDataHandler textDataHandler = new MIDTextDataHandler("Product Display", "ID", "Name");

            return textDataHandler.GetUITextTable(eMIDTextType.eHierarchyDisplayOptions, eMIDTextOrderBy.TextCode, (int)eHierarchyDisplayOptions.DoNotDisplay);
        }

        private DataTable GetStoreDisplayList()
        {
            MIDTextDataHandler textDataHandler = new MIDTextDataHandler("Store Display", "ID", "Name");

            return textDataHandler.GetUITextTable(eMIDTextType.eStoreDisplayOptions, eMIDTextOrderBy.TextCode);
        }

        private DataTable GetDisplayOptions()
        {
            DisplayOptionsRowHandler rowHandler = DisplayOptionsRowHandler.GetInstance(_GlobalOptionsProfile, _iActivityMsgUpperLimit);
            DataTable dt = BuildDisplayOptionsDataTable(rowHandler);

            if (this._GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddDisplayData(rowHandler, dt);
            }

            return dt;
        }

        private DataTable BuildDisplayOptionsDataTable(DisplayOptionsRowHandler rowHandler)
        {
            DataTable dt = new DataTable("Display Options");

            rowHandler.AddUITableColumns(dt);
            return dt;
        }

        private void AddDisplayData(DisplayOptionsRowHandler rowHandler, DataTable dt)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);

            dt.Rows.Add(dr);
        }

        private void UpdateDisplayOptions(DataTable dtDisplayOptions)
        {
            DataRow dr = dtDisplayOptions.Rows[0];

            DisplayOptionsRowHandler rowHandler = DisplayOptionsRowHandler.GetInstance(_GlobalOptionsProfile, _iActivityMsgUpperLimit);

            rowHandler.ParseUIRow(dr);
            _iActivityMsgUpperLimit = rowHandler.iActivityMsgUpperLimit;
        }
    }

    public class DisplayOptionsRowHandler : RowHandler
    {
        private static DisplayOptionsRowHandler _Instance;

        public static DisplayOptionsRowHandler GetInstance(GlobalOptionsProfile GlobalOptionsProfile, int iActivityMsgUpperLimit)
        {
            if (_Instance == null)
            {
                _Instance = new DisplayOptionsRowHandler(GlobalOptionsProfile, iActivityMsgUpperLimit);
            }
            else
            {
                _Instance._GlobalOptionsProfile = GlobalOptionsProfile;
                _Instance._iActivityMsgUpperLimit = iActivityMsgUpperLimit;
            }

            return _Instance;
        }

        private GlobalOptionsProfile _GlobalOptionsProfile;
        private int _iActivityMsgUpperLimit;

        public int iActivityMsgUpperLimit { get { return _iActivityMsgUpperLimit; } }

        private TypedColumnHandler<int> _ProductDisplay = new TypedColumnHandler<int>("Product Display", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _StoreDisplay = new TypedColumnHandler<int>("Store Display", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _OTSStoreAttribute = new TypedColumnHandler<int>("OTS Store Attribute", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _AllocStoreAttribute = new TypedColumnHandler<int>("Allocation Store Attribute", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _ActivityMsgUpperLimit = new TypedColumnHandler<int>("Activity Message Upper Limit", eMIDTextCode.Unassigned, false, 100000);
        private TypedColumnHandler<int> _NewStorePeriodBegin = new TypedColumnHandler<int>("New Store Period Begin", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _NewStorePeriodEnd = new TypedColumnHandler<int>("New Store Period End", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _NonCompStorePeriodBegin = new TypedColumnHandler<int>("Non-comp Store Period Begin", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _NonCompStorePeriodEnd = new TypedColumnHandler<int>("Non-comp Store Period End", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<bool> _FilterOnSizeCurve = new TypedColumnHandler<bool>("Size Curve", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<string> _SizeCurveCharMask = new TypedColumnHandler<string>("Size Curve Character Mask", eMIDTextCode.Unassigned, false, string.Empty);
        private TypedColumnHandler<bool> _FilterOnSizeGroup = new TypedColumnHandler<bool>("Size Group", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<string> _SizeGroupCharMask = new TypedColumnHandler<string>("Size Group Character Mask", eMIDTextCode.Unassigned, false, string.Empty);
        private TypedColumnHandler<bool> _FilterOnSizeAlternates = new TypedColumnHandler<bool>("Size Alternates", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<string> _SizeAlternatesCharMask = new TypedColumnHandler<string>("Size Alternates Character Mask", eMIDTextCode.Unassigned, false, string.Empty);
        private TypedColumnHandler<bool> _FilterOnSizeConstraint = new TypedColumnHandler<bool>("Size Constraints", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<string> _SizeConstraintCharMask = new TypedColumnHandler<string>("Size Constraints Character Mask", eMIDTextCode.Unassigned, false, string.Empty);

        protected DisplayOptionsRowHandler(GlobalOptionsProfile GlobalOptionsProfile, int iActivityMsgUpperLimit)
        {
            _GlobalOptionsProfile = GlobalOptionsProfile;
            _iActivityMsgUpperLimit = iActivityMsgUpperLimit;

            _aColumnHandlers = new ColumnHandler[] { _ProductDisplay, _StoreDisplay, _OTSStoreAttribute, _AllocStoreAttribute,
                                                        _ActivityMsgUpperLimit, _NewStorePeriodBegin, _NewStorePeriodEnd, _NonCompStorePeriodBegin, _NonCompStorePeriodEnd,
                                                        _FilterOnSizeCurve, _SizeCurveCharMask, _FilterOnSizeGroup, _SizeGroupCharMask, _FilterOnSizeAlternates,
                                                        _SizeAlternatesCharMask, _FilterOnSizeConstraint, _SizeConstraintCharMask };
        }

        public override void ParseUIRow(DataRow dr)
        {
            bool bFilterOnSizeCurve = _FilterOnSizeCurve.ParseUIColumn(dr);
            bool bFilterOnSizeGroup = _FilterOnSizeGroup.ParseUIColumn(dr);
            bool bFilterOnSizeAlternates = _FilterOnSizeAlternates.ParseUIColumn(dr);
            bool bFilterOnSizeConstraint = _FilterOnSizeConstraint.ParseUIColumn(dr);

            _GlobalOptionsProfile.ProductLevelDisplay = (eHierarchyDisplayOptions) _ProductDisplay.ParseUIColumn(dr);
            _GlobalOptionsProfile.StoreDisplay = (eStoreDisplayOptions) _StoreDisplay.ParseUIColumn(dr);
            _GlobalOptionsProfile.OTSPlanStoreGroupRID = _OTSStoreAttribute.ParseUIColumn(dr);
            _GlobalOptionsProfile.AllocationStoreGroupRID = _AllocStoreAttribute.ParseUIColumn(dr);
            _GlobalOptionsProfile.NewStorePeriodBegin = _NewStorePeriodBegin.ParseUIColumn(dr);
            _GlobalOptionsProfile.NewStorePeriodEnd = _NewStorePeriodEnd.ParseUIColumn(dr);
            _GlobalOptionsProfile.NonCompStorePeriodBegin = _NonCompStorePeriodBegin.ParseUIColumn(dr);
            _GlobalOptionsProfile.NonCompStorePeriodEnd = _NonCompStorePeriodEnd.ParseUIColumn(dr);
            _GlobalOptionsProfile.SizeCurveCharMask = bFilterOnSizeCurve ?  _SizeCurveCharMask.ParseUIColumn(dr) : string.Empty;
            _GlobalOptionsProfile.SizeGroupCharMask = bFilterOnSizeGroup ? _SizeGroupCharMask.ParseUIColumn(dr) : string.Empty;
            _GlobalOptionsProfile.SizeAlternateCharMask = bFilterOnSizeAlternates ?  _SizeAlternatesCharMask.ParseUIColumn(dr) : string.Empty;
            _GlobalOptionsProfile.SizeConstraintCharMask = bFilterOnSizeConstraint ?  _SizeConstraintCharMask.ParseUIColumn(dr) : string.Empty;
            _iActivityMsgUpperLimit = _ActivityMsgUpperLimit.ParseUIColumn(dr);
        }

        public override void FillUIRow(DataRow dr)
        {
            _ProductDisplay.SetUIColumn(dr, (int)_GlobalOptionsProfile.ProductLevelDisplay);
            _StoreDisplay.SetUIColumn(dr, (int) _GlobalOptionsProfile.StoreDisplay);
            _OTSStoreAttribute.SetUIColumn(dr, _GlobalOptionsProfile.OTSPlanStoreGroupRID);
            _AllocStoreAttribute.SetUIColumn(dr, _GlobalOptionsProfile.AllocationStoreGroupRID);
            _NewStorePeriodBegin.SetUIColumn(dr, _GlobalOptionsProfile.NewStorePeriodBegin);
            _NewStorePeriodEnd.SetUIColumn(dr, _GlobalOptionsProfile.NewStorePeriodEnd);
            _NonCompStorePeriodBegin.SetUIColumn(dr, _GlobalOptionsProfile.NonCompStorePeriodBegin);
            _NonCompStorePeriodEnd.SetUIColumn(dr, _GlobalOptionsProfile.NonCompStorePeriodEnd);
            _SizeCurveCharMask.SetUIColumn(dr, _GlobalOptionsProfile.SizeCurveCharMask);
            _FilterOnSizeCurve.SetUIColumn(dr, _GlobalOptionsProfile.SizeCurveCharMask != string.Empty);
            _SizeGroupCharMask.SetUIColumn(dr, _GlobalOptionsProfile.SizeGroupCharMask);
            _FilterOnSizeGroup.SetUIColumn(dr, _GlobalOptionsProfile.SizeGroupCharMask != string.Empty);
            _SizeAlternatesCharMask.SetUIColumn(dr, _GlobalOptionsProfile.SizeAlternateCharMask);
            _FilterOnSizeAlternates.SetUIColumn(dr, _GlobalOptionsProfile.SizeAlternateCharMask != string.Empty);
            _SizeConstraintCharMask.SetUIColumn(dr, _GlobalOptionsProfile.SizeConstraintCharMask);
            _FilterOnSizeConstraint.SetUIColumn(dr, _GlobalOptionsProfile.SizeConstraintCharMask != string.Empty);
            _ActivityMsgUpperLimit.SetUIColumn(dr, _iActivityMsgUpperLimit);
        }
    }

    
}
