using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetail.Business
{

    public class filterEntrySettings
    {
        public string groupTitle;
        public bool isVisibleInToolbar;
        public string infoInstructions;
        public FilterLoadListDelegate loadFieldList;
        public FilterLoadListDelegate loadValueList;
        public FilterLoadValueListFromFieldDelegate loadValueListFromField;
        public string fieldForData;
        public string fieldForDisplay;
        public string valueFieldForData;
        public string valueFieldForDisplay;
        public FilterGetDataTypeFromFieldIndexDelegate GetDataTypeFromFieldIndex;
        public FilterGetNameFromFieldIndexDelegate GetNameFromField;
        public filterListValueTypes listValueType;
    }

}
