using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROCalendarTimePeriod", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarTimePeriod
    {
        [DataMember(IsRequired = true)]
        private int _sequence;
        [DataMember(IsRequired = true)]
        private string _name;
        [DataMember(IsRequired = true)]
        private string _abbreviation;
        [DataMember(IsRequired = true)]
        private int _timePeriodsCount;
        [DataMember(IsRequired = true)]
        private eROCalendarModelPeriodType _modelPeriodType;

        public int Sequence { get { return _sequence; } }

        public string Name { get { return _name; } }
        
        public string Abbreviation { get { return _abbreviation; } }
        
        public int TimePeriodsCount { get { return _timePeriodsCount; } }
        
        public eROCalendarModelPeriodType ModelPeriodType { get { return _modelPeriodType; } }

        public ROCalendarTimePeriod(int sequence, string name, string abbreviation, int timePeriodsCount, eROCalendarModelPeriodType periodType)
        {
            _sequence = sequence;
            _name = name;
            _abbreviation = abbreviation;
            _timePeriodsCount = timePeriodsCount;
            _modelPeriodType = periodType;
        }
    }

    [DataContract(Name = "ROCalendarTimePeriodModel", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarTimePeriodModel
    {
        [DataMember(IsRequired = true)]
        private string _modelName;
        [DataMember(IsRequired = true)]
        private DateTime _startDate;
        [DataMember(IsRequired = true)]
        private int _fiscalYear;
        [DataMember(IsRequired = true)]
        private int _lastModelYear;
        [DataMember(IsRequired = true)]
        private List<ROCalendarTimePeriod> _months;
        [DataMember(IsRequired = true)]
        private List<ROCalendarTimePeriod> _quarters;
        [DataMember(IsRequired = true)]
        private List<ROCalendarTimePeriod> _seasons;

        public IEnumerable<ROCalendarTimePeriod> MonthPeriods { get { return _months; } }

        public IEnumerable<ROCalendarTimePeriod> QuarterPeriods { get { return _quarters; } }

        public IEnumerable<ROCalendarTimePeriod> SeasonPeriods { get { return _seasons; } }

        public string ModelName { get { return _modelName; } }

        public DateTime StartDate { get { return _startDate; } }

        public int FiscalYear { get { return _fiscalYear; } }

        public int LastModelYear { get { return _lastModelYear; } }

        public ROCalendarTimePeriodModel(string modelName, DateTime startDate, int fiscalYear, int lastModelYear, 
                               IEnumerable<ROCalendarTimePeriod> months, IEnumerable<ROCalendarTimePeriod> quarters, IEnumerable<ROCalendarTimePeriod> seasons)
        {
            _modelName = modelName;
            _startDate = new DateTime(startDate.Ticks);
            _fiscalYear = fiscalYear;
            _lastModelYear = lastModelYear;
            _months = new List<ROCalendarTimePeriod>(months);
            _quarters = new List<ROCalendarTimePeriod>(quarters);
            _seasons = new List<ROCalendarTimePeriod>(seasons);
        }
    }

    [DataContract(Name = "ROCalendarTimePeriodModels", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarTimePeriodModels : ROOut
    {
        [DataMember(IsRequired = true)]
        List<ROCalendarTimePeriodModel> _calendarModels;

        public ROCalendarTimePeriodModels(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, IEnumerable<ROCalendarTimePeriodModel> calendarModels) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _calendarModels = new List<ROCalendarTimePeriodModel>(calendarModels);
        }

        public IEnumerable<ROCalendarTimePeriodModel> CalendarModels {  get { return _calendarModels;  } }
    }

    [DataContract(Name = "ROCalendarDate", Namespace = "http://Logility.ROWeb/")]
    public class ROCalendarDate : ROOut
    {
        [DataMember(IsRequired = true)]
        private int _dateKey;
        [DataMember(IsRequired = true)]
        private string _displayDate;

        public int DateKey { get { return _dateKey; } }

        public int DisplayDate { get { return _dateKey; } }

        public ROCalendarDate(
            eROReturnCode ROReturnCode, 
            string sROMessage, 
            long ROInstanceID, 
            int dateKey, 
            string displayDate
            ) :
            base(
                ROReturnCode, 
                sROMessage, 
                ROInstanceID
                )
        {
            _dateKey = dateKey;
            _displayDate = displayDate;
        }
    }
}