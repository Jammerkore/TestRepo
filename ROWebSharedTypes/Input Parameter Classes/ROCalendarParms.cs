using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROCalendarDeleteParams", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarDeleteParams : ROParms 
    {
        [DataMember(IsRequired = true)]
        private int _iDateRangeRID;

        public ROCalendarDeleteParams(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        int iDateRangeRID)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _iDateRangeRID = iDateRangeRID;
        }

        public int DateRangeRID { get { return _iDateRangeRID; } }
    }

    [DataContract(Name = "ROCalendarSelectorParms", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarSelectorParms : ROCalendarDeleteParams
    {
        [DataMember(IsRequired = true)]
        private int _iAnchorDateRangeRID;
        [DataMember(IsRequired = true)]
        private int _iAnchorDateRelativeTo;
        [DataMember(IsRequired = true)]
        private bool _bRestrictToOnlyWeeks;
        [DataMember(IsRequired = true)]
        private bool _bRestrictToOnlyPeriods;
        [DataMember(IsRequired = true)]
        private bool _bAllowReoccurring;
        [DataMember(IsRequired = true)]
        private bool _bAllowDynamicSwitch;
        [DataMember(IsRequired = true)]
        private bool _bAllowDynamicToCurrent;
        [DataMember(IsRequired = true)]
        private bool _bAllowDynamicToPlan;
        [DataMember(IsRequired = true)]
        private bool _bAllowDynamicToStoreOpen;

        public ROCalendarSelectorParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
            int iDateRangeRID, int iAnchorDateRangeRID, int iAnchorDateRelativeTo, bool bRestrictToOnlyWeeks, bool bRestrictToOnlyPeriods, 
            bool bAllowReoccurring, bool bAllowDynamicSwitch, bool bAllowDynamicToCurrent, bool bAllowDynamicToPlan, bool bAllowDynamicToStoreOpen)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, iDateRangeRID)
        {
            _iAnchorDateRangeRID = iAnchorDateRangeRID;
            _iAnchorDateRelativeTo = iAnchorDateRelativeTo;
            _bRestrictToOnlyWeeks = bRestrictToOnlyWeeks;
            _bRestrictToOnlyPeriods = bRestrictToOnlyPeriods;
            _bAllowReoccurring = bAllowReoccurring;
            _bAllowDynamicSwitch = bAllowDynamicSwitch;
            _bAllowDynamicToCurrent = bAllowDynamicToCurrent;
            _bAllowDynamicToPlan = bAllowDynamicToPlan;
            _bAllowDynamicToStoreOpen = bAllowDynamicToStoreOpen;
        }

        public int AnchorDateRangeRID { get { return _iAnchorDateRangeRID; } }
        public int AnchorDateRelativeTo { get { return _iAnchorDateRelativeTo; } }
        public bool RestrictToOnlyWeeks { get { return _bRestrictToOnlyWeeks; } }
        public bool RestrictToOnlyPeriods { get { return _bRestrictToOnlyPeriods; } }
        public bool AllowReoccurring { get { return _bAllowReoccurring; } }
        public bool AllowDynamicSwitch { get { return _bAllowDynamicSwitch; } }
        public bool AllowDynamicToCurrent { get { return _bAllowDynamicToCurrent; } }
        public bool AllowDynamicToPlan { get { return _bAllowDynamicToPlan; } }
        public bool AllowDynamicToStoreOpen { get { return _bAllowDynamicToStoreOpen; } }
    };

    [DataContract(Name = "ROCalendarSaveParms", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarSaveParms : ROCalendarDeleteParams
    {
        [DataMember(IsRequired = true)]
        private bool _bSaveDateRange;
        [DataMember(IsRequired = true)]
        private int _iDateRangeRID;
        [DataMember(IsRequired = true)]
        private string _sDateRangeName;
        [DataMember(IsRequired = true)]
        private int _iStartDate;
        [DataMember(IsRequired = true)]
        private int _iEndDate;
        [DataMember(IsRequired = true)]
        private int _iDateType;
        [DataMember(IsRequired = true)]
        private int _iDateRangeType;
        [DataMember(IsRequired = true)]
        private int _iRelativeTo;

        public ROCalendarSaveParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                    bool bSaveDateRange, string sDateRangeName, int iDateRangeRID, int iStartDate, int iEndDate, 
                                    int iDateType, int iDateRangeType, int iRelativeTo)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID, iDateRangeRID)
        {
            _bSaveDateRange = bSaveDateRange;
            _iDateRangeRID = iDateRangeRID;
            if (sDateRangeName == null)
            {
                _sDateRangeName = string.Empty;
            }
            else
            {
                _sDateRangeName = sDateRangeName;
            }
            _iStartDate = iStartDate;
            _iEndDate = iEndDate;
            _iDateType = iDateType;
            _iDateRangeType = iDateRangeType;
            _iRelativeTo = iRelativeTo;
        }

        public bool bSaveDateRange { get { return _bSaveDateRange; } }
        public int iCDRRID { get { return _iDateRangeRID; } }
        public string sDateRangeName { get { return _sDateRangeName; } }
        public int iStartDate { get { return _iStartDate; } }
        public int iEndDate { get { return _iEndDate; } }
        public int iDateType { get { return _iDateType; } }
        public int iDateRangeType { get { return _iDateRangeType; } }
        public int iRelativeTo { get { return _iRelativeTo; } }
    }

    [DataContract(Name = "ROCalendarDateCalculationParms", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarDateCalculationParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private int _dateKey;
        [DataMember(IsRequired = true)]
        private int _baseDateKey;
        [DataMember(IsRequired = true)]
        private eDateCalculationType _dateCalculationType;

        public ROCalendarDateCalculationParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID,
            eDateCalculationType dateCalculationType,
            int dateKey, 
            int baseDateKey
            )
            : base(
                  sROUserID, 
                  sROSessionID, 
                  ROClass, 
                  RORequest, 
                  ROInstanceID
                  )
        {
            _dateKey = dateKey;
            _baseDateKey = baseDateKey;
            _dateCalculationType = dateCalculationType;
        }

        public int DateKey { get { return _dateKey; } }
        public int BaseDateKey { get { return _baseDateKey; } }
        public eDateCalculationType DateCalculationType { get { return _dateCalculationType; } }
    }
}
