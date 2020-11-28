using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace StoreBinConverter
{
    public static class TaskCommon
    {

        public delegate void UpdateLogDelegate(string logMsg);
        public delegate void UpdateTotalTimeDelegate();

        public enum TaskType
        {
            Daily,
            Weekly,
            VSW
        }

        public class stepParameters
        {
            public string taskName;
            public string stepName;
            public string databaseConnectionString;
            public int numberOfHistoryTables;

            public TaskType taskType;
            public int[] storeRIDs;
        
            public SessionAddressBlock SAB;
            public ProfileList weekList;
            public System.Collections.ArrayList varKeyList;

            public TaskCommon.UpdateLogDelegate UpdateLog;
        }
    }
}
