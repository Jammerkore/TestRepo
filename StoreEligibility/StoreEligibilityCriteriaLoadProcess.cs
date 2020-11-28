using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.StoreEligibilityCriteriaLoad
{
    public class StoreEligibilityCriteriaLoadProcess
    {
        private string sourceModule = "StoreEligibilityCriteriaLoadProcess.cs";
        private StoreEligibilityList _StoreEligibilityList;
        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
        protected Hashtable _stores = null;
        ProfileList _storeList;
        protected Hashtable _nodes = null;
        // End TT#2621 - JSmith - Duplicate weeks in daily sales
        private HierarchyNodeProfile _hnp;
        private ProfileList _DPweeks;
        private StoreEligibility se = null;
        SessionAddressBlock _SAB;
        //protected StoreEligibilityCriteriaData _dpcd = null;
        protected Audit _audit = null;
        protected int _recordsRead = 0;
        protected int _recordsWithErrors = 0;
        protected int _recordsAddedUpdated = 0;
        // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
        HierarchyMaintenance _hm;
        // End TT#2657 - JSmith - Severe error during DailySalesPercentages load

        public class StoreEligibilityValues
        {

            private int Hn_RID;
            private int St_RID;
            private DateRangeProfile Date_Range_Profile;
            private int End_date;
            private decimal Day1;
            private decimal Day2;
            private decimal Day3;
            private decimal Day4;
            private decimal Day5;
            private decimal Day6;
            private decimal Day7;


            public int HN_RID
            {
                get
                {
                    return this.Hn_RID;
                }
                set
                {
                    this.Hn_RID = value;
                }
            }

            public int ST_RID
            {
                get
                {
                    return this.St_RID;
                }
                set
                {
                    this.St_RID = value;
                }
            }

            public DateRangeProfile dR_PROF
            {
                get
                {
                    return this.Date_Range_Profile;
                }
                set
                {
                    this.Date_Range_Profile = value;
                }
            }

            public int END_DATE
            {
                get
                {
                    return this.End_date;
                }
                set
                {
                    this.End_date = value;
                }
            }

            public decimal DAY1
            {
                get
                {
                    return this.Day1;
                }
                set
                {
                    this.Day1 = value;
                }
            }

            public decimal DAY2
            {
                get
                {
                    return this.Day2;
                }
                set
                {
                    this.Day2 = value;
                }
            }

            public decimal DAY3
            {
                get
                {
                    return this.Day3;
                }
                set
                {
                    this.Day3 = value;
                }
            }

            public decimal DAY4
            {
                get
                {
                    return this.Day4;
                }
                set
                {
                    this.Day4 = value;
                }
            }

            public decimal DAY5
            {
                get
                {
                    return this.Day5;
                }
                set
                {
                    this.Day5 = value;
                }
            }

            public decimal DAY6
            {
                get
                {
                    return this.Day6;
                }
                set
                {
                    this.Day6 = value;
                }
            }

            public decimal DAY7
            {
                get
                {
                    return this.Day7;
                }
                set
                {
                    this.Day7 = value;
                }
            }


            public StoreEligibilityValues(int HNRID, int STRID, DateRangeProfile DRPROF, decimal D1, decimal D2, decimal D3, decimal D4, decimal D5, decimal D6, decimal D7)
            {

                Hn_RID = HNRID;
                St_RID = STRID;
                dR_PROF = DRPROF;
                Day1 = D1;
                Day2 = D2;
                Day3 = D3;
                Day4 = D4;
                Day5 = D5;
                Day6 = D6;
                Day7 = D7;

            }

        }

        public class DailyKeyValues : IComparable<DailyKeyValues>
        {
            private int _index;
            private int _value;
            private double _Pct;
            private int _Week;

            public int CompareTo(DailyKeyValues other)
            {

                return _index.CompareTo(other._index);
            }

            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }

            public int Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public Double Pct
            {
                get { return _Pct; }
                set { _Pct = value; }
            }

            public int Week
            {
                get { return _Week; }
                set { _Week = value; }
            }

            public DailyKeyValues(int index, int aValue, double bValue, int cValue)
            {
                _index = index;
                _value = aValue;
                _Pct = bValue;
                _Week = cValue;

            }

        }

        public class XMLStoreEligibilityProcessException : Exception
        {
            /// <summary>
            /// Used when throwing exceptions in the XML ColorCode Load Class
            /// </summary>
            /// <param name="message">The error message to display</param>
            public XMLStoreEligibilityProcessException(string message)
                : base(message)
            {
            }
            public XMLStoreEligibilityProcessException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        public StoreEligibilityCriteriaLoadProcess(SessionAddressBlock SAB, ref bool errorFound)
        {
            try
            {
                _SAB = SAB;
                _audit = _SAB.ClientServerSession.Audit;
                //_dpcd = new StoreEligibilityCriteriaData();
                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                _stores = _SAB.StoreServerSession.GetStoreIDHash();
                _nodes = _SAB.HierarchyServerSession.GetNodeIDHash();
                _storeList = _SAB.StoreServerSession.GetAllStoresList();
                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                _hm = new HierarchyMaintenance(_SAB);
                // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
            }
            catch (Exception ex)
            {
                errorFound = true;
                _audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                throw;
            }
        }

        public eReturnCode ProcessVariableFile(string fileLocation, char[] delimiter, ref bool errorFound)
        {
            //BEGIN TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
            // lots of changes under this TT please use the diff function to find them all
            StreamReader reader = null;
            string line = null;
            string message = null;

            eReturnCode returnCode = eReturnCode.successful;
            List<string> lines = new List<string>();
            // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
            //HierarchyMaintenance _hm = new HierarchyMaintenance(_SAB);
            // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
            EditMsgs em = new EditMsgs();
            reader = new StreamReader(fileLocation);  //opens the file

            try
            {
                while ((line = reader.ReadLine()) != null)
                {

                    returnCode = eReturnCode.successful;
                    string[] fields = MIDstringTools.Split(line, delimiter[0], true);
                    if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                    {
                        continue;
                    }
                    lines.Add(line);
                }
                reader.Close();
                returnCode = ValidateFileData(lines, delimiter, ref errorFound);
            }
            catch (FileNotFoundException fileNotFound_error)
            {
                string exceptionMessage = fileNotFound_error.Message;
                errorFound = true;
                message = " : " + fileLocation;
                _audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
                _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
            }


            return returnCode;
        }

        public eReturnCode ProcessXMLTransFile(string fileLocation, char[] delimiter, ref bool errorFound)
        {
            var KeyList = new List<DailyKeyValues>();
            List<string> lines = new List<string>();
            string merchId = string.Empty;
            string storeAttribute = string.Empty;
            string storeAttributeSet = string.Empty;
            string line = string.Empty;
            string message = string.Empty;
            eReturnCode returnCode = eReturnCode.successful;

            EditMsgs em = new EditMsgs();

            if (!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
            {
                throw new XMLStoreEligibilityProcessException(String.Format("Can not find the file located at '{0}'", fileLocation));
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
            try
            {


                XmlSerializer s = new XmlSerializer(typeof(StoreEligibility));	// Create a Serializer

                r = new StreamReader(fileLocation);			// Load the Xml File

                se = (StoreEligibility)s.Deserialize(r);	// Deserialize the Xml File to a strongly typed object

            }
            catch (Exception ex)
            {
                throw new XMLStoreEligibilityProcessException(String.Format("Error encountered during deserialization of the file '{0}'", fileLocation), ex);
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            try
            {
                foreach (StoreEligibilityProduct sep in se.Product)
                {
                    foreach (StoreEligibilityProductStore seps in sep.Store)
                    {
                        try
                        {
                            int count = 0;
                            string productId = Convert.ToString(sep.ID);
                            string storeRID = Convert.ToString(seps.ID);
                            bool eligible = seps.Ineligible;
                            string simstores = null;
                            string index = null;
                            string fromDate = null;
                            string toDate = null;
                            if (seps.SimilarStore != null)
                            {
                                simstores = Convert.ToString(seps.SimilarStore.ID);
                                index = Convert.ToString(seps.SimilarStore.Index);
                                fromDate = Convert.ToString(seps.SimilarStore.FromDate);
                                toDate = Convert.ToString(seps.SimilarStore.UntilDate);
                            }
                            line = productId + delimiter[0] + storeRID + delimiter[0] + eligible + delimiter[0] + simstores + delimiter[0] + index + delimiter[0] + fromDate + delimiter[0] + toDate + delimiter[0];

                            if (seps.Types != null)
                            {
                                foreach (StoreEligibilityProductStoreTypes sepst in seps.Types)
                                {
                                    count = count + 1;
                                    string type = Convert.ToString(sepst.Type);
                                    string value;
                                    if (type == "MinPlusSales")
                                    {
                                        //BEGIN TT#3611 - DOConnell - issue with store eligibility node properties upload
                                        if (sepst.Value.Length > 0)
                                        {
                                            //END TT#3611 - DOConnell - issue with store eligibility node properties upload
                                            if (Convert.ToInt32(sepst.Value) == 1)
                                            {
                                                value = "true";
                                            }
                                            else
                                            {
                                                value = "false";
                                            }
                                        }
                                        else
                                        {
                                            value = Convert.ToString(sepst.Value);
                                        }
                                        //BEGIN TT#3611 - DOConnell - issue with store eligibility node properties upload
                                    }
                                    else
                                    {
                                        value = Convert.ToString(sepst.Value);
                                    }
                                    //END TT#3611 - DOConnell - issue with store eligibility node properties upload

                                    string model = Convert.ToString(sepst.Model);

                                    line = line + type + delimiter[0] + value + delimiter[0] + model;
                                    if (count != seps.Types.Length)
                                        line = line + delimiter[0];
                                }
                            }
                            lines.Add(line);
                        }
                        catch (Exception ex)
                        {
                            throw new XMLStoreEligibilityProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
                        }
                    }

                }

                returnCode = ValidateFileData(lines, delimiter, ref errorFound);

            }
            catch (Exception ex)
            {
                throw new XMLStoreEligibilityProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
            }
            return returnCode;
        }

        public eReturnCode ValidateFileData(List<string> lines, char[] delimiter, ref bool errorFound)
        {
            int nodeRID = Include.NoRID;
            int storeRID = Include.NoRID;
            string nodeId = null;
            string oldNodeID = null;
            int oldNodeRID = Include.NoRID;
            string storeID = null;
			//BEGIN TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.
            //string FWOSmodifierModel = Convert.ToString(Include.NoRID);
            //string eligibilityModel = Convert.ToString(Include.NoRID);
            //string stockmodifierModel = Convert.ToString(Include.NoRID);
            //string salesmodifierModel = Convert.ToString(Include.NoRID);
			//END TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.

            int fromYYYYWW = 0;
            int untilYYYYWW = 0;
            WeekProfile beginWeek = null;
            WeekProfile endWeek = null;
            int dateRangeKey = Include.NoRID;
            string dateRangeID = null;

            bool EOF = false; 
            bool similarStoreChanged = false;
            string message = null;
            eReturnCode returnCode = eReturnCode.successful;
            List<string> lineData = new List<string>();
            ArrayList alAction = new ArrayList();
            object objRID = null;
            ArrayList ErrorList = new ArrayList();
            StoreEligibilityList _storeEligList;
            StoreEligibilityProfile sep;
            DateRangeProfile sedrp;
            ArrayList sepSimStores = null;
            StoreProfile sp = null;
            string error = null;
            try
            {
                System.Data.DataSet _storeEligibilityDataSet = MIDEnvironment.CreateDataSet("storeEligibilityDataSet");
                storeEligibilityDataSet seds = new storeEligibilityDataSet();
                _storeEligibilityDataSet = seds.StoreEligibility_Define(_storeEligibilityDataSet);
                _storeList = _SAB.StoreServerSession.GetAllStoresList();

                if (lines != null && lines.Count > 0)
                {
                    lines.Sort();
                    for (int s = 0; s < lines.Count; s++)
                    {

                        _recordsRead++;
                        string type = null;
                        int eligModKey = Include.NoRID;
                        bool Ineligible = false;
                        int slsModKey = Include.NoRID;
                        int stkModKey = Include.NoRID;
                        int FWOSModKey = Include.NoRID;
                        string FWOSmodifierValue = null;
                        string eligibilityValue = null;
                        string stockmodifierValue = null;
                        string salesmodifierValue = null;
                        string simStores = null;
                        string simStoresIndex = string.Empty;
                        bool MinPlusSales = false;
						//BEGIN TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.
                        string FWOSmodifierModel = Convert.ToString(Include.NoRID);
                        string eligibilityModel = Convert.ToString(Include.NoRID);
                        string stockmodifierModel = Convert.ToString(Include.NoRID);
                        string salesmodifierModel = Convert.ToString(Include.NoRID);
						//END TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.
                        int i = 4;

                        if (s == lines.Count - 1) EOF = true;
                        string[] fields = MIDstringTools.Split(lines[s], delimiter[0], true);
                        if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                        {
                            continue;
                        }
                        returnCode = eReturnCode.successful;
                        similarStoreChanged = false;
                        sp = null;

                        EditMsgs em = new EditMsgs();
                        message = lines[s];
                        nodeId = Convert.ToString(fields[0]);
                        try
                        {
                            objRID = _nodes[nodeId];
                            if (objRID != null)
                            {
                                nodeRID = Convert.ToInt32(objRID);
                            }
                            else
                            {
                                _hnp = _hm.NodeLookup(ref em, nodeId, false, false);
                                nodeRID = _hnp.Key;
                            }

                            if (nodeRID == Include.NoRID)
                            {
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                            else
                            {
                                nodeId = Convert.ToString(nodeRID);
                            }

                        }
                        catch (Exception)
                        {
                            message = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidChainSetPercentMerchID);
                            message = message.Replace("{0}", fields[0]);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                            returnCode = eReturnCode.severe;
                            ++_recordsWithErrors;
                        }

                        if (oldNodeID != null && oldNodeID != nodeId)
                        {
                            //BEGIN TT#3622 - DOConnell - Severe error when running store eligibility load
                            //_storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_storeList, oldNodeRID, true, false);
                            _storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_storeList, nodeRID, true, false);
                            //END TT#3622 - DOConnell - Severe error when running store eligibility load
                            ErrorList = _hm.ValidEligibilityData(_storeList, _storeEligibilityDataSet, _storeEligList, ErrorList);
                            for (int e = 0; e < ErrorList.Count; e++)
                            {
                                StoreEligibilityErrors see = (StoreEligibilityErrors)ErrorList[e];
                                if (see.typeErr == true || see.simStoreErr == true)
                                {
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidArgument, see.message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    _storeEligList = see.sel;
                                }
                            }
                            //BEGIN TT#3622 - DOConnell - Severe error when running store eligibility load
                            //_SAB.HierarchyServerSession.StoreEligibilityUpdate(Convert.ToInt32(oldNodeID), _storeEligList, false);
                            _SAB.HierarchyServerSession.StoreEligibilityUpdate(Convert.ToInt32(nodeRID), _storeEligList, false);
                            //END TT#3622 - DOConnell - Severe error when running store eligibility load
                            _storeEligibilityDataSet.Tables["Stores"].Rows.Clear();
                            ErrorList.Clear();
                            oldNodeID = nodeId;
                            oldNodeRID = nodeRID;
                        }
                        else if (oldNodeID == null)
                        {
                            oldNodeID = nodeId;
                            oldNodeRID = nodeRID;
                        }

                        _storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_storeList, nodeRID, true, false);
                        storeID = Convert.ToString(fields[1]);
                        try
                        {
                            objRID = _stores[storeID];
                            if (objRID != null)
                            {
                                //BEGIN TT#3622 - DOConnell - Severe error when running store eligibility load
                                //storeRID = Convert.ToInt32(storeID);
                                storeRID = Convert.ToInt32(objRID);
                                //END TT#3622 - DOConnell - Severe error when running store eligibility load
                            }
                            else
                            {
                                sp = _SAB.StoreServerSession.GetStoreProfile(storeID);
                                storeRID = sp.Key;
                                storeID = sp.Text;
                            }

                            if (storeRID == Include.NoRID)
                            {
                                error = MIDText.GetText(eMIDTextCode.msg_InvalidStoreField);
                                error.Replace("{0}", Convert.ToString(fields[1]));
                                //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, message, sourceModule);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, error, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                            else
                            {
                                storeID = Convert.ToString(storeRID);
                            }

                            if (sp == null)
                            {
                                sp = _SAB.StoreServerSession.GetStoreProfile(storeID);
                                storeRID = sp.Key;
                                storeID = sp.Text;
                            }

                            sep = (StoreEligibilityProfile)_storeEligList.FindKey(sp.Key);

                            if (sep != null)
                            {
                                sepSimStores = sep.SimStores;
                            }
                        }
                        catch (Exception)
                        {
                            error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidStoreField);
                            error = error.Replace("{0}", fields[1]);
                            //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, error, sourceModule);
                            returnCode = eReturnCode.severe;
                            ++_recordsWithErrors;
                        }

                        if (Convert.ToBoolean(fields[2]) == true)
                        {
                            Ineligible = true;
                        }
                        else
                        {
                            Ineligible = false;
                        }

                        simStores = fields[3];
                        if (simStores != null)
                        {
                            char _delimiter = _SAB.ClientServerSession.GlobalOptions.ProductLevelDelimiter;
                            simStores = simStores.Replace(_delimiter, delimiter[0]);
                            similarStoreChanged = true;
                            i = 7;
                        }

                        if (fields[4] != null)
                        {
                            simStoresIndex = Convert.ToString(fields[4]);
                        }
                        else
                        {
                            simStoresIndex = string.Empty;
                        }

                        if (fields[5] != null)
                        {
                            fromYYYYWW = Convert.ToInt32(fields[5]);
                        }
                        else
                        {
                            if (fields[6] != null)
                            {
                                fromYYYYWW = Convert.ToInt32(fields[6]);
                            }
                            else
                            {
                                fromYYYYWW = 0;
                            }
                        }

                        if (fields[6] != null)
                        {
                            untilYYYYWW = Convert.ToInt32(fields[5]);
                        }
                        else if (fromYYYYWW != 0)
                        {
                            untilYYYYWW = fromYYYYWW;
                        }

                        //BEGIN TT#837 - MD - DOConnell - Store Eligibility error
                        if (fromYYYYWW == 0 && untilYYYYWW != 0)
                        {
                            fromYYYYWW = untilYYYYWW;
                        }
                        else if (untilYYYYWW == 0 && fromYYYYWW != 0)
                        {
                            untilYYYYWW = fromYYYYWW;
                        }
                        //END TT#837 - MD - DOConnell - Store Eligibility error

                        if (fromYYYYWW != 0 && untilYYYYWW != 0)
                        {
                            try
                            {
                                beginWeek = _SAB.ClientServerSession.Calendar.GetFiscalWeek(fromYYYYWW);

                            }
                            catch (Exception)
                            {
                                message = "Invalid From Date " + message;
                                string until = fields[5];
                                
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                            try
                            {
                                endWeek = _SAB.ClientServerSession.Calendar.GetFiscalWeek(untilYYYYWW);
                            }
                            catch (Exception)
                            {
                                message = "Invalid Until Date " + message;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }

                        if (returnCode == eReturnCode.successful)
                        {
                            //BEGIN TT#837 - MD - DOConnell - Store Eligibility error
                            if (beginWeek != null && endWeek != null)
                            {
                                sedrp = _SAB.ClientServerSession.Calendar.AddDateRangeFromWeeks(beginWeek.YearWeek, endWeek.YearWeek);
                                dateRangeKey = sedrp.Key;
                                dateRangeID = sedrp.DisplayDate;
                            }
                            //END TT#837 - MD - DOConnell - Store Eligibility error
                        }


                        for (int j = i; j < fields.Length; j++)
                        {
                            type = (String)fields[j];

                            switch (type)
                            { 
                                case "Eligibility":
                                    if (fields[j + 1] != null)
                                    {
                                        
                                        eligibilityValue = Convert.ToString(fields[j + 1]);
                                        eligModKey = Include.NoRID;
                                        eligibilityModel = Convert.ToString(eligModKey);
                                        j = j + 1;
                                    }
                                    else if (fields[j + 2] != null)
                                    {
                                        eligibilityModel = fields[j + 2];
                                        EligModelProfile eligModProf = _SAB.HierarchyServerSession.GetEligModelData(eligibilityModel);
                                        eligModKey = eligModProf.Key;

                                        if (eligModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                            error = error.Replace("{0}", eModelType.Eligibility + " - ");
                                            message = error + message;
                                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                        else
                                        {
                                            eligibilityValue = eligibilityModel; // TT#3611 - JSmith - issue with store eligibility node properties upload
                                            eligibilityModel = Convert.ToString(eligModKey);
                                        }
                                        j = j + 2;
                                    }
                                    else
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //eligibilityModel = null;
                                        eligibilityModel = Convert.ToString(Include.NoRID);
                                        eligibilityValue = null;
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;
                                case "SalesModifier":
                                    if (fields[j + 1] != null)
                                    {
                                        salesmodifierValue = Convert.ToString(fields[j + 1]);
                                        slsModKey = Include.NoRID;
                                        salesmodifierModel = Convert.ToString(slsModKey);
                                        j = j + 1;
                                    }
                                    else if (fields[j + 2] != null)
                                    {
                                        salesmodifierModel = fields[j + 2];
                                        SlsModModelProfile slsModProf = _SAB.HierarchyServerSession.GetSlsModModelData(salesmodifierModel);
                                        slsModKey = slsModProf.Key;
                                        if (slsModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                            error = error.Replace("{0}", eModelType.SalesModifier + " - ");
                                            message = error + message;
                                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                        else
                                        {
                                            salesmodifierValue = salesmodifierModel; // TT#3611 - JSmith - issue with store eligibility node properties upload
                                            salesmodifierModel = Convert.ToString(slsModKey);
                                        }
                                        j = j + 2;
                                    }
                                    else
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //salesmodifierModel = null;
                                        salesmodifierModel = Convert.ToString(Include.NoRID);
                                        salesmodifierValue = null;
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;
                                case "StockModifier":
                                    if (fields[j + 1] != null)
                                    {
                                        stockmodifierValue = Convert.ToString(fields[j + 1]);
                                        stkModKey = Include.NoRID;
                                        stockmodifierModel = Convert.ToString(stkModKey);
                                        j = j + 1;
                                    }
                                    else if (fields[j + 2] != null)
                                    {
                                        stockmodifierModel = fields[j + 2];
                                        StkModModelProfile stkModProf = _SAB.HierarchyServerSession.GetStkModModelData(stockmodifierModel);
                                        stkModKey = stkModProf.Key;
                                        if (stkModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                            error = error.Replace("{0}", eModelType.StockModifier + " - ");
                                            message = error + message;
                                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                        else
                                        {
                                            stockmodifierValue = stockmodifierModel; // TT#3611 - JSmith - issue with store eligibility node properties upload
                                            stockmodifierModel = Convert.ToString(stkModKey);
                                        }
                                        j = j + 2;
                                    }
                                    else
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //stockmodifierModel = null;
                                        stockmodifierModel = Convert.ToString(Include.NoRID);
                                        stockmodifierValue = null;
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;
                                case "FWOSModifier":
                                    if (fields[j + 1] != null)
                                    {
                                        FWOSmodifierValue = Convert.ToString(fields[j + 1]);
                                        FWOSModKey = Include.NoRID;
                                        FWOSmodifierModel = Convert.ToString(FWOSModKey);
                                        j = j + 1;
                                    }
                                    else if (fields[j + 2] != null)
                                    {
                                        FWOSmodifierModel = fields[j + 2];
                                        FWOSModModelProfile fWOSModProf = _SAB.HierarchyServerSession.GetFWOSModModelData(FWOSmodifierModel);
                                        FWOSModKey = fWOSModProf.Key;
                                        if (fWOSModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                            error = error.Replace("{0}", eModelType.FWOSModifier + " - ");
                                            message = error + message;
                                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                        }
                                        else
                                        {
                                            FWOSmodifierValue = FWOSmodifierModel; // TT#3611 - JSmith - issue with store eligibility node properties upload
                                            FWOSmodifierModel = Convert.ToString(FWOSModKey);
                                        }
                                        j = j + 2;
                                    }
                                    else
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //FWOSmodifierModel = null;
                                        FWOSmodifierModel = Convert.ToString(Include.NoRID);
                                        FWOSmodifierValue = null;
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;
                                case "MinPlusSales":
                                    try
                                    {
                                        if (Convert.ToBoolean(fields[j + 1]) == true)
                                        {
                                            MinPlusSales = true;
                                        }
                                        else
                                        {
                                            MinPlusSales = false;
                                        }
                                    }
                                    catch
                                    {
                                        error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                        error = error.Replace("{0}", fields[j + 1] + " - ");
                                        message = error + message;
                                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                        returnCode = eReturnCode.severe;
                                        ++_recordsWithErrors;
                                    }
                                    continue;
                            }
                        }



                        if (returnCode == eReturnCode.successful)
                        {


                            _storeEligibilityDataSet.Tables["Stores"].Rows.Add(new object[] {null, false, Include.NoRID, true, storeRID, storeID, 
                                                                                          eligModKey, eligibilityModel, Ineligible, 
                                                                                          MinPlusSales, false, Include.NoRID,
                                                                                          false, Include.NoRID, stockmodifierModel, stockmodifierValue,
                                                                                          false, Include.NoRID, salesmodifierModel, salesmodifierValue,
                                                                                          false, Include.NoRID, FWOSmodifierModel, FWOSmodifierValue,
                                                                                          false, Include.NoRID, 
                                                                                          false, similarStoreChanged, sepSimStores, simStores, simStoresIndex, dateRangeKey, dateRangeID});

                            //_storeEligibilityDataSet.Tables["Stores"].Rows.Add(new object[] {sglp.Name, sep.EligIsInherited, sep.EligInheritedFromNodeRID, updated, storeProfile.Key, storeProfile.Text, sep.EligModelRID, sep.EligModelName,
                            //                                                                sep.StoreIneligible, 
                            //                                                                PMPlusSales, sep.PresPlusSalesIsInherited, sep.PresPlusSalesInheritedFromNodeRID,
                            //                                                                sep.StkModIsInherited, sep.StkModInheritedFromNodeRID, sep.StkModModelRID, stkMod, 
                            //                                                                sep.SlsModIsInherited, sep.SlsModInheritedFromNodeRID, sep.SlsModModelRID, slsMod,
                            //                                                                sep.FWOSModIsInherited, sep.FWOSModInheritedFromNodeRID, sep.FWOSModModelRID, FWOSMod,
                            //                                                                sep.SimStoreIsInherited, sep.SimStoreInheritedFromNodeRID,
                            //                                                                usedStoreSelector, similarStoreChanged, sep.SimStores, simStores, ratio, simStoreUntilDateRangeRID, until});
                            _recordsAddedUpdated++;
                        }
                        else
                        {
                            returnCode = eReturnCode.successful;
                        }




                        if (EOF)
                        {
                            ErrorList = _hm.ValidEligibilityData(_storeList, _storeEligibilityDataSet, _storeEligList, ErrorList);

                            for (int e = 0; i < ErrorList.Count; i++)
                            {
                                StoreEligibilityErrors see = (StoreEligibilityErrors)ErrorList[e];
                                if (see.typeErr == true || see.simStoreErr == true)
                                {
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_InvalidArgument, see.message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    _storeEligList = see.sel;
                                }
                            }

                            _SAB.HierarchyServerSession.StoreEligibilityUpdate(nodeRID, _storeEligList, false);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                errorFound = true;
                _audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                throw;
            }
            finally
            {
                _audit.StoreEligibilityCriteriaLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _recordsAddedUpdated);
            }
            return returnCode;
        }
    }
}
