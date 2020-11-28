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

namespace MIDRetail.Business
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

        /// <summary>
        /// Local class's exception type.
        /// </summary>
        [Serializable]
        public class XMLStoreEligibilityProcessException : Exception
        {
            /// <summary>
            /// Used when throwing exceptions in the XML Hierarchy Load Class
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

            public XMLStoreEligibilityProcessException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
                : base(aInfo, aContext)
            {
            }
            override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
            {
                base.GetObjectData(aInfo, aContext);
            }
        }

        public StoreEligibilityCriteriaLoadProcess(SessionAddressBlock SAB, ref bool errorFound)
        {
            try
            {
                _SAB = SAB;
                //_audit = _SAB.ApplicationServerSession.Audit;
                //_dpcd = new StoreEligibilityCriteriaData();
                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                //_stores = StoreMgmt.GetStoreIDHash(); //_SAB.StoreServerSession.GetStoreIDHash();
                _stores = _SAB.StoreServerSession.GetStoreIDHash();
                // End TT#1902-MD - JSmith - Store Services - VSW API Error
                _nodes = _SAB.HierarchyServerSession.GetNodeIDHash();
                // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                //_storeList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_SAB.StoreServerSession.GetActiveStoresList();
                _storeList = _SAB.StoreServerSession.GetActiveStoresList();
                // End TT#1902-MD - JSmith - Store Services - VSW API Error
                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                _hm = new HierarchyMaintenance(_SAB, SAB.HierarchyServerSession);
                // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
            }
            catch (Exception ex)
            {
                errorFound = true;
                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
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
                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
                _SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
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
			//BEGIN TT#3705 - DOConnell - Try to load  store eligibility xml without any record and it gives errors
                if (se.Product != null)
                {
                    foreach (StoreEligibilityProduct sep in se.Product)
                    {
						//BEGIN TT#3705 - DOConnell - Try to load  store eligibility xml without any record and it gives errors
                        if (sep.Store != null)
                        {
                            foreach (StoreEligibilityProductStore seps in sep.Store)
                            {
                                try
                                {
                                    int count = 0;
                                    string productId = Convert.ToString(sep.ID);
                                    string storeRID = Convert.ToString(seps.ID);
                                    //BEGIN TT#3663 - DOConnell - Ineligible will not clear
                                    //bool eligible = seps.Ineligible;
                                    string eligible = seps.Ineligible;
                                    //END TT#3663 - DOConnell - Ineligible will not clear
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
                                    //BEGIN TT#3668 - DOConnell - Clearing SimStores and Index fields only clears Index field
                                    if (seps.Ineligible != null && seps.Ineligible == string.Empty)
                                    {
                                        eligible = " ";
                                    }
                                    else
                                    {
                                        eligible = Convert.ToString(seps.Ineligible);
                                    }
                                    if (seps.SimilarStore != null)
                                    {
                                        if (seps.SimilarStore.ID != null && seps.SimilarStore.ID == string.Empty)
                                        {
                                            simstores = " ";
                                        }
                                        else
                                        {
                                            simstores = Convert.ToString(seps.SimilarStore.ID);
                                        }

                                        if (seps.SimilarStore.Index != null && seps.SimilarStore.Index == string.Empty)
                                        {
                                            index = " ";
                                        }
                                        else
                                        {
                                            index = Convert.ToString(seps.SimilarStore.Index);
                                        }
                                        if (seps.SimilarStore.FromDate != null && seps.SimilarStore.FromDate == string.Empty)
                                        {
                                            fromDate = " ";
                                        }
                                        else
                                        {
                                            fromDate = Convert.ToString(seps.SimilarStore.FromDate);
                                        }
                                        if (seps.SimilarStore.UntilDate != null && seps.SimilarStore.UntilDate == string.Empty)
                                        {
                                            toDate = " ";
                                        }
                                        else
                                        {
                                            toDate = Convert.ToString(seps.SimilarStore.UntilDate);
                                        }
                                    }
                                    //END TT#3668 - DOConnell - Clearing SimStores and Index fields only clears Index field
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
                                                //BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
                                                if (sepst.Value != null && sepst.Value == string.Empty)
                                                {
                                                    value = " ";
                                                }
                                                else
                                                {
                                                    value = Convert.ToString(sepst.Value);
                                                }
                                                ////BEGIN TT#3611 - DOConnell - issue with store eligibility node properties upload
                                                //if (sepst.Value.Length > 0)
                                                //{
                                                //    //END TT#3611 - DOConnell - issue with store eligibility node properties upload
                                                //    // Begin TT#1158 - JSmith - MinPlusSales errors when a decimal is entered in the Value field - ANF
                                                //    try
                                                //    {
                                                //        // End TT#1158 - JSmith - MinPlusSales errors when a decimal is entered in the Value field - ANF
                                                //        if (Convert.ToInt32(sepst.Value) == 1)
                                                //        {
                                                //            value = "true";
                                                //        }
                                                //        else
                                                //        {
                                                //            value = "false";
                                                //        }
                                                //        // Begin TT#1158 - JSmith - MinPlusSales errors when a decimal is entered in the Value field - ANF
                                                //    }
                                                //    catch
                                                //    {
                                                //        value = "false";
                                                //    }
                                                //    // End TT#1158 - JSmith - MinPlusSales errors when a decimal is entered in the Value field - ANF
                                                //}
                                                //else
                                                //{
                                                //    value = Convert.ToString(sepst.Value);
                                                //}
                                                ////BEGIN TT#3611 - DOConnell - issue with store eligibility node properties upload
                                                //END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
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
						//END TT#3705 - DOConnell - Try to load  store eligibility xml without any record and it gives errors
                        }
                    }
                }
                else
                {
                    string error = MIDText.GetText(eMIDTextCode.msg_MerchandiseRequired);
                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, error, sourceModule);
                    returnCode = eReturnCode.severe;
                }
				//END TT#3705 - DOConnell - Try to load  store eligibility xml without any record and it gives errors
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
            int readNodeRID = Include.NoRID;
            //BEGIN TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.
            //string FWOSmodifierModel = Convert.ToString(Include.NoRID);
            //string eligibilityModel = Convert.ToString(Include.NoRID);
            //string stockmodifierModel = Convert.ToString(Include.NoRID);
            //string salesmodifierModel = Convert.ToString(Include.NoRID);
            //END TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.
			
			//BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
            //int fromYYYYWW = 0;
            //int untilYYYYWW = 0;
            //WeekProfile beginWeek = null;
            //WeekProfile endWeek = null;
            //int dateRangeKey = Include.NoRID;
            //string dateRangeID = null;
			//END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
			
            bool EOF = false;
            bool similarStoreChanged = false;
            string message = null;
            eReturnCode returnCode = eReturnCode.successful;
            List<string> lineData = new List<string>();
            ArrayList alAction = new ArrayList();
            object objRID = null;
            ArrayList ErrorList = new ArrayList();
            StoreEligibilityList _storeEligList = null;
            StoreEligibilityProfile sep = null;
            string currentID = null;
            DateRangeProfile sedrp;
            ArrayList sepSimStores = null;
            StoreProfile sp = null;
            string error = null;
            CustomBusinessRoutines buisnessRoutines = new CustomBusinessRoutines(null, null);
            try
            {
                System.Data.DataSet _storeEligibilityDataSet = MIDEnvironment.CreateDataSet("storeEligibilityDataSet");
                storeEligibilityDataSet seds = new storeEligibilityDataSet();
                _storeEligibilityDataSet = seds.StoreEligibility_Define(_storeEligibilityDataSet);
                // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                //_storeList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_SAB.StoreServerSession.GetActiveStoresList();
                _storeList = _SAB.StoreServerSession.GetActiveStoresList();
                // End TT#1902-MD - JSmith - Store Services - VSW API Error

                if (lines != null && lines.Count > 0)
                {
                    lines.Sort();
                    for (int s = 0; s < lines.Count; s++)
                    {

                        _recordsRead++;
                        string type = null;
                        object eligModKey = System.DBNull.Value;
                        object Ineligible = System.DBNull.Value;
                        object slsModKey = System.DBNull.Value;
                        object stkModKey = System.DBNull.Value;
                        object FWOSModKey = System.DBNull.Value;
                        object FWOSmodifierValue = System.DBNull.Value;
                        object eligibilityValue = System.DBNull.Value;
                        object stockmodifierValue = System.DBNull.Value;
                        object salesmodifierValue = System.DBNull.Value;
                        object simStores = System.DBNull.Value;
                        object simStoresIndex = System.DBNull.Value;
                        object MinPlusSales = System.DBNull.Value;
                        //BEGIN TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.
                        //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                        //string FWOSmodifierModel = Convert.ToString(Include.NoRID);
                        //string eligibilityModel = Convert.ToString(Include.NoRID);
                        //string stockmodifierModel = Convert.ToString(Include.NoRID);
                        //string salesmodifierModel = Convert.ToString(Include.NoRID);
                        object FWOSmodifierModel = System.DBNull.Value;
                        object eligibilityModel = System.DBNull.Value;
                        object stockmodifierModel = System.DBNull.Value;
                        object salesmodifierModel = System.DBNull.Value;
                        //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                        //END TT#828 - MD - DOConnell - Getting System.NullReferenceException: Object reference not set to an instance of an object. While running Store Eligibility Load with an invalid stock modifier model name.

                        string ineligibleStr = string.Empty; ////BEGIN TT#3663 - DOConnell - Ineligible will not clear
						
						//BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
                        int fromYYYYWW = 0;
                        int untilYYYYWW = 0;
                        WeekProfile beginWeek = null;
                        WeekProfile endWeek = null;
                        int dateRangeKey = Include.NoRID;
                        string dateRangeID = null;
						//END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
                        bool MinPlusSalesUpdating = false; //TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales

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
                            if (currentID != nodeId)
                            {
                                objRID = _nodes[nodeId];
                                if (objRID != null)
                                {
                                    nodeRID = Convert.ToInt32(objRID);
                                    currentID = nodeId;
                                }
                                else
                                {
                                    _hnp = _hm.NodeLookup(ref em, nodeId, false, false);
                                    nodeRID = _hnp.Key;
                                    if (nodeRID != Include.NoRID)
                                    {
                                        _nodes.Add(nodeId, nodeRID);
                                        currentID = nodeId;
                                    }
                                }
                            }

                            if (nodeRID == Include.NoRID)
                            {
                                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, message, sourceModule);
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
                            message = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentMerchID);
                            message = message.Replace("{0}", fields[0]);
                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                            returnCode = eReturnCode.severe;
                            ++_recordsWithErrors;
                        }

                        if (oldNodeID != null && oldNodeID != nodeId)
                        {
                            //BEGIN TT#3622 - DOConnell - Severe error when running store eligibility load
                            if (readNodeRID != oldNodeRID)
                            {
                                _storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_storeList, oldNodeRID, true, false);
                                readNodeRID = oldNodeRID;
                            }
                            //_storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_storeList, nodeRID, true, false);
                            //END TT#3622 - DOConnell - Severe error when running store eligibility load
                            ErrorList = _hm.ValidEligibilityData(_storeList, _storeEligibilityDataSet, _storeEligList, ErrorList);
                            for (int e = 0; e < ErrorList.Count; e++)
                            {
                                StoreEligibilityErrors see = (StoreEligibilityErrors)ErrorList[e];

                                if (see.typeErr == true || see.simStoreErr == true)
                                {
                                    //Begin TT#4375 - DOConnell - Store eligibility error
                                    string errMessage = MIDText.GetText(eMIDTextCode.msg_InvalidArgument, false);
                                    string detMessage = see.message;
                                    eMIDTextCode errRID = eMIDTextCode.msg_InvalidArgument;
                                    if (see.simStoreErr)
                                    {
                                        // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                        //StoreProfile sp1 = StoreMgmt.StoreProfile_Get(see.storeRID); // _SAB.StoreServerSession.GetStoreProfile(see.storeRID);
                                        StoreProfile sp1 = (StoreProfile)_storeList.FindKey(see.storeRID);
                                        // End TT#1902-MD - JSmith - Store Services - VSW API Error
                                        string errNodeID = _SAB.HierarchyServerSession.GetNodeID(oldNodeRID);
                                        errRID = eMIDTextCode.msg_StoreNotFound;
                                        detMessage = detMessage.Replace("{0}", see.dataString);
                                        detMessage = detMessage.Replace("{1}", errNodeID);
                                        detMessage = detMessage.Replace("{2}", sp1.StoreId.ToString());
                                    }
                                    //_SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidArgument, see.message, sourceModule);
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, detMessage, sourceModule);
                                    //End TT#4375 - DOConnell - Store eligibility error
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    _storeEligList = see.sel;
                                }
                            }
                            //BEGIN TT#3622 - DOConnell - Severe error when running store eligibility load
							// Begin TT#3691 - JSmith - Combination of product found and product not found causes error
                            //_SAB.HierarchyServerSession.StoreEligibilityUpdate(Convert.ToInt32(oldNodeID), _storeEligList, false);
                            if (oldNodeRID != Include.NoRID)
                            {
                                _SAB.HierarchyServerSession.StoreEligibilityUpdate(Convert.ToInt32(oldNodeID), _storeEligList, false);
                            }
							// End TT#3691 - JSmith - Combination of product found and product not found causes error
                            //_SAB.HierarchyServerSession.StoreEligibilityUpdate(Convert.ToInt32(nodeRID), _storeEligList, false);
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

                        if (readNodeRID != nodeRID)
                        {
                            _storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_storeList, nodeRID, true, false);
                            readNodeRID = nodeRID; 
                        }
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
						        // Begin TT#3691 - JSmith - Combination of product found and product not found causes error
                                // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                //sp = StoreMgmt.StoreProfile_Get(storeID); //_SAB.StoreServerSession.GetStoreProfile(storeID);
                                objRID = _stores[storeID];
                                // End TT#1902-MD - JSmith - Store Services - VSW API Error
                                if (_storeList != null)
                                {
                                    sp = (StoreProfile)_storeList.FindKey(Convert.ToInt32(objRID));
                                }
                                if (sp == null)
                                {
                                    // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                    //sp = StoreMgmt.StoreProfile_Get(storeID); //_SAB.StoreServerSession.GetStoreProfile(storeID);
                                    sp = Include.GetUnknownStoreProfile();
                                    // End TT#1902-MD - JSmith - Store Services - VSW API Error
                                }
								// End TT#3691 - JSmith - Combination of product found and product not found causes error
                                storeRID = sp.Key;
                                storeID = sp.Text;
                            }

                            if (storeRID == Include.NoRID)
                            {
							    // Begin TT#3691 - JSmith - Combination of product found and product not found causes error
                                //error = MIDText.GetText(eMIDTextCode.msg_InvalidStoreField);
                                //error.Replace("{0}", Convert.ToString(fields[1]));
                                ////_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, message, sourceModule);
                                //_SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, error, sourceModule);
                                //returnCode = eReturnCode.severe;
                                //++_recordsWithErrors;

                                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
								// End TT#3691 - JSmith - Combination of product found and product not found causes error
                            }
                            // Begin TT#5592 - AGallagher - Store Eligibility load failure not writing appropriate error message in the Audit
                            else
                                if (_storeList != null && storeRID > 0)
                                {
                                    sp = (StoreProfile)_storeList.FindKey(Convert.ToInt32(objRID));
                                }
                                if (sp == null)
                                {
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                            // End TT#5592 - AGallagher - Store Eligibility load failure not writing appropriate error message in the Audit
                            //else
                            //{
                            //    storeID = Convert.ToString(storeRID);
                            //}

                            //if (sp == null)
                            //{
                            //    sp = _SAB.StoreServerSession.GetStoreProfile(storeID);
                            //    storeRID = sp.Key;
                            //    storeID = sp.Text;
                            //}

                            sep = (StoreEligibilityProfile)_storeEligList.FindKey(storeRID);

                            if (sep != null)
                            {
                                sepSimStores = sep.SimStores;
								//BEGIN TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                simStoresIndex = sep.SimStoreRatio;
                                dateRangeID = sep.SimStoreDisplayDate;
                                dateRangeKey = sep.SimStoreUntilDateRangeRID;
                                for (int ss = 0; ss < sep.SimStores.Count; ss++)
                                {
                                    int SSRID = Convert.ToInt32(sep.SimStores[ss]);
									// Begin TT#3691 - JSmith - Combination of product found and product not found causes error
									//StoreProfile SSsp = _SAB.StoreServerSession.GetStoreProfile(SSRID);
                                    StoreProfile SSsp = null;
                                    if (_storeList != null)
                                    {
                                        SSsp = (StoreProfile)_storeList.FindKey(Convert.ToInt32(SSRID));
                                    }
                                    if (SSsp == null)
                                    {
                                        // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                        //SSsp = StoreMgmt.StoreProfile_Get(SSRID); //_SAB.StoreServerSession.GetStoreProfile(SSRID);
                                        SSsp = Include.GetUnknownStoreProfile();
                                        // End TT#1902-MD - JSmith - Store Services - VSW API Error
                                    }
									// End TT#3691 - JSmith - Combination of product found and product not found causes error
                                    if (ss > 0 && ss != sep.SimStores.Count)
                                    {
                                        simStores = simStores + ",";
                                    }
                                    simStores = simStores + SSsp.StoreId;
                                }
								//END TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                // Begin TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                                if (!sep.EligIsInherited)
                                {
                                    eligModKey = sep.EligModelRID;
                                    Ineligible = sep.StoreIneligible;
                                    eligibilityModel = sep.EligModelName;
                                }
                                if (!sep.SlsModIsInherited)
                                {
                                    if (sep.SlsModType == eModifierType.Model)
                                    {
                                        slsModKey = sep.SlsModModelRID;
                                        salesmodifierModel = sep.SlsModModelRID;
                                        salesmodifierValue = sep.SlsModModelName;
                                    }
                                    else if (sep.SlsModType == eModifierType.Percent)
                                    {
                                        salesmodifierValue = sep.SlsModPct;
                                    }
                                }
                                if (!sep.StkModIsInherited)
                                {
                                    if (sep.StkModType == eModifierType.Model)
                                    {
                                        stkModKey = sep.StkModModelRID;
                                        stockmodifierModel = sep.StkModModelRID;
                                        stockmodifierValue = sep.StkModModelName;
                                    }
                                    else if (sep.StkModType == eModifierType.Percent)
                                    {
                                        stockmodifierValue = sep.StkModPct;
                                    }
                                }
                                if (!sep.FWOSModIsInherited)
                                {
                                    if (sep.FWOSModType == eModifierType.Model)
                                    {
                                        FWOSModKey = sep.FWOSModModelRID;
                                        FWOSmodifierModel = sep.FWOSModModelRID;
                                        FWOSmodifierValue = sep.FWOSModModelName;
                                    }
                                    else if (sep.FWOSModType == eModifierType.Percent)
                                    {
                                        FWOSmodifierValue = sep.FWOSModPct;
                                    }
                                }
                                if (!sep.PresPlusSalesIsInherited)
                                {
                                    MinPlusSales = sep.PresPlusSalesInd;
                                }
                                if (!sep.SimStoreIsInherited)
                                {
                                    sepSimStores = sep.SimStores; // TT#3683 - JSmith - API transactions not accepting 1 or 0 for the Ineligible field 
                                    simStoresIndex = sep.SimStoreRatio;
                                }
                                // End TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                            }
                            //TT#3675 - DOConnell - Store Eligibility - Sending file without Similar Store ID does not error
                            else
                            {
                                sep = new StoreEligibilityProfile(storeRID);
                                if (sep.SimStoreRatio == 0)
                                {
                                    sep.SimStoreRatio = Include.DefaultSimilarStoreRatio;
                                }
                                // Begin TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                                Ineligible = false;
                                // End TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                            }
							//END TT#3675 - DOConnell - Store Eligibility - Sending file without Similar Store ID does not error
                        }
                        catch (Exception)
                        {
						    // Begin TT#3691 - JSmith - Combination of product found and product not found causes error
                            //error = MIDText.GetText(eMIDTextCode.msg_InvalidStoreField);
                            //error = error.Replace("{0}", fields[1]);
                            ////_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                            //_SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, error, sourceModule);
                            //returnCode = eReturnCode.severe;
                            //++_recordsWithErrors;

                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidStoreField, message, sourceModule);
                            returnCode = eReturnCode.severe;
                            ++_recordsWithErrors;
							// End TT#3691 - JSmith - Combination of product found and product not found causes error
                        }
						////BEGIN TT#3663 - DOConnell - Ineligible will not clear

                        // Begin TT#3681 - JSmith - Duplicate store transaction for same merchandise causes error
                        // update store information if duplicate transactions for store
                        DataRow[] drStoreEligibilityRows = _storeEligibilityDataSet.Tables["Stores"].Select("[Store RID] = " + storeRID);
                        if (drStoreEligibilityRows.Length > 0)
                        {
                            DataRow drStoreEligibility = drStoreEligibilityRows[0];

                            sep.EligIsInherited = (drStoreEligibility["Inherited"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["Inherited"]) : false;
                            sep.EligInheritedFromNodeRID = (drStoreEligibility["Inherited RID"] != DBNull.Value) ? Convert.ToInt32(drStoreEligibility["Inherited RID"]) : Include.NoRID;
                            eligModKey = drStoreEligibility["Eligibility RID"];
                            eligibilityModel = drStoreEligibility["Eligibility"];
                            Ineligible = drStoreEligibility["Ineligible"];
                            MinPlusSales = drStoreEligibility["PMPlusSales"];
                            sep.PresPlusSalesIsInherited = (drStoreEligibility["PMPlusSales Inherited"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["PMPlusSales Inherited"]) : false;
                            sep.PresPlusSalesInheritedFromNodeRID = (drStoreEligibility["PMPlusSales Inherited RID"] != DBNull.Value) ? Convert.ToInt32(drStoreEligibility["PMPlusSales Inherited RID"]) : Include.NoRID;
                            sep.StkModIsInherited = (drStoreEligibility["Stk Mod Inherited"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["Stk Mod Inherited"]) : false;
                            sep.StkModInheritedFromNodeRID = (drStoreEligibility["Stock Modifier Inherited RID"] != DBNull.Value) ? Convert.ToInt32(drStoreEligibility["Stock Modifier Inherited RID"]) : Include.NoRID;
                            stockmodifierModel = drStoreEligibility["Stock Modifier RID"];
                            stockmodifierValue = drStoreEligibility["Stock Modifier"];
                            sep.SlsModIsInherited = (drStoreEligibility["Sls Mod Inherited"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["Sls Mod Inherited"]) : false;
                            sep.SlsModInheritedFromNodeRID =(drStoreEligibility["Sales Modifier Inherited RID"] != DBNull.Value) ?  Convert.ToInt32(drStoreEligibility["Sales Modifier Inherited RID"]) : Include.NoRID;
                            salesmodifierModel = drStoreEligibility["Sales Modifier RID"];
                            salesmodifierValue = drStoreEligibility["Sales Modifier"];
                            sep.FWOSModIsInherited = (drStoreEligibility["FWOS Mod Inherited"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["FWOS Mod Inherited"]) : false;
                            sep.FWOSModInheritedFromNodeRID = (drStoreEligibility["FWOS Modifier Inherited RID"] != DBNull.Value) ? Convert.ToInt32(drStoreEligibility["FWOS Modifier Inherited RID"]) : Include.NoRID;
                            FWOSmodifierModel = drStoreEligibility["FWOS Modifier RID"];
                            FWOSmodifierValue = drStoreEligibility["FWOS Modifier"];
                            sep.SimStoreIsInherited = (drStoreEligibility["Sim Str Inherited"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["Sim Str Inherited"]) : false;
                            sep.SimStoreInheritedFromNodeRID = (drStoreEligibility["Similar Store Inherited RID"] != DBNull.Value) ? Convert.ToInt32(drStoreEligibility["Similar Store Inherited RID"]) : Include.NoRID;
                            similarStoreChanged =  (drStoreEligibility["Similar Store Changed"] != DBNull.Value) ? Convert.ToBoolean(drStoreEligibility["Similar Store Changed"]) : false;
                            sepSimStores = (drStoreEligibility["SimilarStoresArrayList"] != DBNull.Value) ? (ArrayList)(drStoreEligibility["SimilarStoresArrayList"]) : new ArrayList();
                            simStores = drStoreEligibility["Similar Store"];
                            simStoresIndex = drStoreEligibility["Ratio"];
                            dateRangeKey =  (drStoreEligibility["Until RID"] != DBNull.Value) ? Convert.ToInt32(drStoreEligibility["Until RID"]) : Include.NoRID;
                            dateRangeID = (drStoreEligibility["Until"] != DBNull.Value) ? Convert.ToString(drStoreEligibility["Until RID"]) : string.Empty;

                            _storeEligibilityDataSet.Tables["Stores"].Rows.Remove(drStoreEligibility);
                        }
                        // End TT#3681 - JSmith - Duplicate store transaction for same merchandise causes error

                        if (fields[2] != null)
                        {
							//BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
                            try
                            {
                                ineligibleStr = Convert.ToString(fields[2]).Trim();
                                if (ineligibleStr.Trim().Length > 0)
                                {
                                    if ((ineligibleStr != "0" && ineligibleStr != "1"))
                                    {
                                        if (ineligibleStr.Length > 0 && Convert.ToBoolean(ineligibleStr) == true)
                                        //if (Convert.ToBoolean(fields[2]) == true)
                                        {
                                            Ineligible = true;
                                        }
                                        else
                                        {
                                            Ineligible = false;
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(ineligibleStr) == 1)
                                        {
                                            Ineligible = true;
                                        }
                                        else
                                        {
                                            Ineligible = false;
                                        }
                                    }
                                }
								//BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field
                                else
                                {
                                    Ineligible = false;
                                }
								//END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field
                            }
                            catch
                            {
                                error = MIDText.GetText(eMIDTextCode.msg_InvalidArgument);
                                error = error.Replace("{0}", "Ineligible " + ineligibleStr + " - ");
                                message = error + " - " + message;
                                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }



                            //if (ineligibleStr.Length > 0 && Convert.ToBoolean(ineligibleStr) == true)
                            ////if (Convert.ToBoolean(fields[2]) == true)
                            //{
                            //    Ineligible = true;
                            //}
                            //else
                            //{
                            //    Ineligible = false;
                            //}
							//END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
						////BEGIN TT#3663 - DOConnell - Ineligible will not clear
                        }
                        // Begin TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                        //else
                        //{
                        //    Ineligible = false;
                        //}
                        // End TT#3642 - JSmith - Store Eligibility upload is cancelling out all values

                        //simStores = fields[3];
                        bool clearingSimilarStores = false;  //TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                        if (fields[3] != null)
                        {
							//BEGIN TT#3668 - DOConnell - Clearing SimStores and Index fields only clears Index field
                            if (fields[3] == string.Empty)
                            {
                                sepSimStores = new ArrayList();
                                simStores = string.Empty;
                                simStoresIndex = string.Empty; //TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                                dateRangeID = string.Empty; //TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                                clearingSimilarStores = true;  //TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                            }
                            else
                            {
                                simStores = fields[3];
                                char _delimiter = _SAB.HierarchyServerSession.GlobalOptions.ProductLevelDelimiter;
                                simStores = Convert.ToString(simStores).Replace(_delimiter, delimiter[0]);
                            }
							//END TT#3668 - DOConnell - Clearing SimStores and Index fields only clears Index field
                            similarStoreChanged = true;
                            i = 7;
                            
                        }

                        if (fields[4] != null)
                        {
                            simStoresIndex = Convert.ToString(fields[4]);
                            similarStoreChanged = true; //TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
							//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                            if (Convert.ToString(simStoresIndex).Trim().Length == 0)
                            {
                                if (sep.SimStoreIsInherited)
                                {
                                    simStoresIndex = sep.SimStoreRatio;
                                }
                                else
                                {
								//BEGIN TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
                                    //simStoresIndex = Include.DefaultSimilarStoreRatio;
                                    simStoresIndex = string.Empty;
									//END TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
                                }
                            }
						//BEGIN TT#3662 - DOConnell - Store Eligibility Transaction defaults similar store index to zero instead of 100
                        }
                        // Begin TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                        //else if (!clearingSimilarStores) //TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                        //{
                        //    simStoresIndex = sep.SimStoreRatio;
                        //}
                        // End TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
						//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
						//END TT#3662 - DOConnell - Store Eligibility Transaction defaults similar store index to zero instead of 100
                        //else
                        //{
                        //    simStoresIndex = string.Empty;
                        //}
						//BEGIN TT#3663 - DOConnell - Ineligible will not clear

                        // Begin TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                        if ((fields[5] != null && Convert.ToString(fields[5]).Trim().Length > 0) ||
                            (fields[6] != null && Convert.ToString(fields[6]).Trim().Length > 0))
                        {
                        // End TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                            beginWeek = null;
                            endWeek = null;
                            dateRangeKey = Include.NoRID;
                            //END TT#3663 - DOConnell - Ineligible will not clear
                            if (fields[5] != null && Convert.ToString(fields[5]).Trim().Length > 0)
                            {
                                fromYYYYWW = Convert.ToInt32(fields[5]);
                                similarStoreChanged = true; //TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                            }
                            else
                            {
                                if (fields[6] != null && Convert.ToString(fields[6]).Trim().Length > 0)
                                {
                                    fromYYYYWW = Convert.ToInt32(fields[6]);
                                    similarStoreChanged = true; //TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                }
                                else
                                {
                                    fromYYYYWW = 0;
                                }
                            }

                            if (fields[6] != null && Convert.ToString(fields[6]).Trim().Length > 0)
                            {
                                untilYYYYWW = Convert.ToInt32(fields[6]);
                                similarStoreChanged = true; //TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                            }
                            //BEGIN TT#3663 - DOConnell - Ineligible will not clear
                            else
                            {
                                untilYYYYWW = 0;
                            }
                            //else if (fromYYYYWW != 0)
                            //{
                            //    untilYYYYWW = fromYYYYWW;
                            //}
                            //END TT#3663 - DOConnell - Ineligible will not clear

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
                                    beginWeek = _SAB.HierarchyServerSession.Calendar.GetFiscalWeek(fromYYYYWW);

                                }
                                catch (Exception)
                                {
                                    message = "Invalid From Date " + message;
                                    string until = fields[5];

                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                try
                                {
                                    endWeek = _SAB.HierarchyServerSession.Calendar.GetFiscalWeek(untilYYYYWW);
                                }
                                catch (Exception)
                                {
                                    message = "Invalid Until Date " + message;
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                            }

                            if (returnCode == eReturnCode.successful)
                            {
                                //BEGIN TT#837 - MD - DOConnell - Store Eligibility error
                                if (beginWeek != null && endWeek != null)
                                {
								//BEGIN TT#3692 - DOConnell - System Null Reference Exception Error - Node Properties Date
                                    if (endWeek.YearWeek >= beginWeek.YearWeek)
                                    {
                                        sedrp = _SAB.HierarchyServerSession.Calendar.AddDateRangeFromWeeks(beginWeek.YearWeek, endWeek.YearWeek);
                                        dateRangeKey = sedrp.Key;
                                        dateRangeID = sedrp.DisplayDate;
                                    }
                                    else
                                    {
                                        message = "Until Date less than From Date " + message;
                                        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                        returnCode = eReturnCode.severe;
                                        ++_recordsWithErrors;
                                    }
								//END TT#3692 - DOConnell - System Null Reference Exception Error - Node Properties Date
                                }
                                //BEGIN TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
                                else
                                {
                                    dateRangeKey = Include.NoRID;
                                    dateRangeID = string.Empty; //TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
                                }
                                //END TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
                                //END TT#837 - MD - DOConnell - Store Eligibility error
                            }
                        }  // TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
                        int curr_j = 0;

                        for (int j = i; j < fields.Length; j++)
                        {
                            curr_j = j;
                            type = (String)fields[j];

                            switch (type)
                            {
                                case "Eligibility":
                                    if (fields[j + 1] != null)
                                    {
										//BEGIN TT#3703 - DOConnell - Min Plus Sales should error if model is provided
                                        error = MIDText.GetText(eMIDTextCode.msg_Data1NotValidData2);
                                            error = error.Replace("{0}", "Value");
                                            error = error.Replace("{1}", "Option for Eligibility");
                                            message = error + " - " + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                        //try
                                        //{
                                        //    eligibilityValue = Convert.ToString(fields[j + 1]);
                                        //    //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        //    if (Convert.ToString(eligibilityValue) != string.Empty)
                                        //    {
                                        //        //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        //        if (Convert.ToDecimal(eligibilityValue) > Include.NoRID)
                                        //        {
                                        //            eligModKey = Include.NoRID;
                                        //            eligibilityModel = Convert.ToString(eligModKey);
                                        //            //BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //            //if (fields[j + 2] != null)
                                        //            if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length > 0)
                                        //            //END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //            {
                                        //                error = MIDText.GetText(eMIDTextCode.msg_CannotHaveBoth);
                                        //                error = error.Replace("{0}", "Eligibility ");
                                        //                message = error + " - " + message;
                                        //                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                        //                returnCode = eReturnCode.severe;
                                        //                ++_recordsWithErrors;
                                        //            }
                                        //            //BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //            //else
                                        //            //{
                                        //            //    j = j + 1;
                                        //            //}
                                        //            //END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //        }
                                        //        //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        //    }
                                        //    //BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //    //else
                                        //    //{
                                        //    //    j = j + 1;
                                        //    //}
                                        //    //END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //    //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        //}
                                        //catch
                                        //{
                                        //    error = MIDText.GetText(eMIDTextCode.msg_InvalidEligiblityValue);
                                        //    error = error.Replace("{0}", eligibilityValue + " - ");
                                        //    message = error + message;
                                        //    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                        //    returnCode = eReturnCode.severe;
                                        //    ++_recordsWithErrors;
                                        //    break;
                                        //}
										//END TT#3703 - DOConnell - Min Plus Sales should error if model is provided
                                    }
									//BEGIN TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
                                    //else if (fields[j + 2] != null)
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                        //else if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //END TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
									{
                                        eligibilityModel = fields[j + 2];
                                        EligModelProfile eligModProf = _SAB.HierarchyServerSession.GetEligModelData(Convert.ToString(eligibilityModel));
                                        eligModKey = eligModProf.Key;

                                        if (eligModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvaidEligibilityModel);
                                            error = error.Replace("{0}", eligibilityModel + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                        else
                                        {
                                            eligibilityValue = eligibilityModel; // TT#3611 - JSmith - issue with store eligibility node properties upload
                                            eligibilityModel = Convert.ToString(eligModKey);
                                            // Begin TT#5511 - JSmith - Eligibility API - some not working
                                            if (Ineligible != null
                                                && !Convert.ToBoolean(Ineligible))
                                            {
                                                sep.EligIsInherited = false;
                                                sep.EligInheritedFromNodeRID = Include.NoRID;
                                            }
                                            // End TT#5511 - JSmith - Eligibility API - some not working
                                        }
                                        j = j + 2;
                                    }
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
									//else
                                    if ((fields[curr_j + 1] == null || Convert.ToString(fields[curr_j + 1]).Trim().Length == 0) &&
                                        (fields[curr_j + 2] == null || Convert.ToString(fields[curr_j + 2]).Trim().Length == 0))  // TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //eligibilityModel = null;
                                        eligibilityModel = Convert.ToString(Include.NoRID);
                                        eligModKey = Include.NoRID;
										//BEGIN TT#3676 - DOConnell - Sales Modifier value not clearing
                                        //eligibilityValue = null;
                                        eligibilityValue = DBNull.Value;
										//END TT#3676 - DOConnell - Sales Modifier value not clearing
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;

                                case "SalesModifier":
                                    if (fields[j + 1] != null)
                                    {
                                        try
                                        {
                                            salesmodifierValue = Convert.ToString(fields[j + 1]);
                                            //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            if (Convert.ToString(salesmodifierValue) != string.Empty)
                                            {
                                                if (Convert.ToDecimal(salesmodifierValue) > Include.NoRID)
                                                {
                                                    slsModKey = Include.NoRID;
                                                    salesmodifierModel = Convert.ToString(slsModKey);
													//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    //if (fields[j + 2] != null)
                                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length > 0)
													//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    {
                                                        error = MIDText.GetText(eMIDTextCode.msg_CannotHaveBoth);
                                                        error = error.Replace("{0}", "Sales Modifier ");
                                                        message = error + " - " + message;
                                                        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                                        returnCode = eReturnCode.severe;
                                                        ++_recordsWithErrors;
                                                    }
													//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    //else
                                                    //{
                                                    //    j = j + 1;
                                                    //}
													//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                }
                                            }
											//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                            //else
                                            //{
                                            //    j = j + 1;
                                            //}
											//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                            //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        }
                                        catch
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvalidSalesModValue);
                                            error = error.Replace("{0}", salesmodifierValue + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                    }
									//BEGIN TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
                                    //else if (fields[j + 2] != null)
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //else if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //END TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
									{
                                        salesmodifierModel = fields[j + 2];
                                        SlsModModelProfile slsModProf = _SAB.HierarchyServerSession.GetSlsModModelData(Convert.ToString(salesmodifierModel));
                                        slsModKey = slsModProf.Key;
                                        if (slsModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvalidSalesModModel);
                                            error = error.Replace("{0}", salesmodifierModel + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
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
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
									//else
                                    if ((fields[curr_j + 1] == null || Convert.ToString(fields[curr_j + 1]).Trim().Length == 0) &&
                                        (fields[curr_j + 2] == null || Convert.ToString(fields[curr_j + 2]).Trim().Length == 0))  // TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //salesmodifierModel = null;
                                        salesmodifierModel = Convert.ToString(Include.NoRID);
										//BEGIN TT#3676 - DOConnell - Sales Modifier value not clearing
                                        //salesmodifierValue = null;
                                        salesmodifierValue = DBNull.Value;
										//END TT#3676 - DOConnell - Sales Modifier value not clearing
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;

                                case "StockModifier":
                                    if (fields[j + 1] != null)
                                    {
                                        try
                                        {
                                            stockmodifierValue = Convert.ToString(fields[j + 1]);
                                            //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            if (Convert.ToString(stockmodifierValue) != string.Empty)
                                            {
                                                if (Convert.ToDecimal(stockmodifierValue) > Include.NoRID)
                                                {
                                                    stkModKey = Include.NoRID;
                                                    stockmodifierModel = Convert.ToString(stkModKey);
													//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    //if (fields[j + 2] != null)
                                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length > 0)
													//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    {
                                                        error = MIDText.GetText(eMIDTextCode.msg_CannotHaveBoth);
                                                        error = error.Replace("{0}", "Stock Modifier ");
                                                        message = error + " - " + message;
                                                        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                                        returnCode = eReturnCode.severe;
                                                        ++_recordsWithErrors;
                                                    }
													//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    //else
                                                    //{
                                                    //    j = j + 1;
                                                    //}
													//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                }
                                            }
											//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                            //else
                                            //{
                                            //    j = j + 1;
                                            //}
											//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                            //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        }
                                        catch
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvalidStockModValue);
                                            error = error.Replace("{0}", stockmodifierValue + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                    }
									//BEGIN TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
                                    //else if (fields[j + 2] != null)
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //else if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //END TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
									{
                                        stockmodifierModel = fields[j + 2];
                                        StkModModelProfile stkModProf = _SAB.HierarchyServerSession.GetStkModModelData(Convert.ToString(stockmodifierModel));
                                        stkModKey = stkModProf.Key;
                                        if (stkModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvaidStockModModel);
                                            error = error.Replace("{0}", stockmodifierModel + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
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
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
									//else
                                    if ((fields[curr_j + 1] == null || Convert.ToString(fields[curr_j + 1]).Trim().Length == 0) &&
                                        (fields[curr_j + 2] == null || Convert.ToString(fields[curr_j + 2]).Trim().Length == 0))  // TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //stockmodifierModel = null;
                                        stockmodifierModel = Convert.ToString(Include.NoRID);
										//BEGIN TT#3676 - DOConnell - Sales Modifier value not clearing
                                        //stockmodifierValue = null;
                                        stockmodifierValue = DBNull.Value;
										//END TT#3676 - DOConnell - Sales Modifier value not clearing
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;

                                case "FWOSModifier":
                                    if (fields[j + 1] != null)
                                    {
                                        try
                                        {
                                            FWOSmodifierValue = Convert.ToString(fields[j + 1]);
                                            //BEGIN TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                            if (Convert.ToString(FWOSmodifierValue) != string.Empty)
                                            {
                                                if (Convert.ToDecimal(FWOSmodifierValue) > Include.NoRID)
                                                {
                                                    FWOSModKey = Include.NoRID;
                                                    FWOSmodifierModel = Convert.ToString(FWOSModKey);
													//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    //if (fields[j + 2] != null)
                                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length > 0)
													//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    {
                                                        error = MIDText.GetText(eMIDTextCode.msg_CannotHaveBoth);
                                                        error = error.Replace("{0}", "FWOS Modifier ");
                                                        message = error + " - " + message;
                                                        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                                        returnCode = eReturnCode.severe;
                                                        ++_recordsWithErrors;
                                                    }
													//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                    //else
                                                    //{
                                                    //    j = j + 1;
                                                    //}
													//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                                }
                                            }
											//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                            //else
                                            //{
                                            //    j = j + 1;
                                            //}
											//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                            //END TT#3642 - DOConnell - Store Eligibility upload is cancelling out all values
                                        }
                                        catch
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvalidFWOSValue);
                                            error = error.Replace("{0}", FWOSmodifierValue + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                            break;
                                        }
                                    }
									//BEGIN TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
                                    //else if (fields[j + 2] != null)
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //else if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
                                    if (fields[j + 2] != null && Convert.ToString(fields[j + 2]).Trim().Length != 0)
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    //END TT#3665 - DOConnell - Get Invalid Sales Modifier Model when trying to clear model
									{
                                        FWOSmodifierModel = fields[j + 2];
                                        FWOSModModelProfile fWOSModProf = _SAB.HierarchyServerSession.GetFWOSModModelData(Convert.ToString(FWOSmodifierModel));
                                        FWOSModKey = fWOSModProf.Key;
                                        if (fWOSModProf.Key == Include.NoRID)
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvaidFWOSModel);
                                            error = error.Replace("{0}", FWOSmodifierModel + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
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
									//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
									//else
                                    if ((fields[curr_j + 1] == null || Convert.ToString(fields[curr_j + 1]).Trim().Length == 0) &&
                                         (fields[curr_j + 2] == null || Convert.ToString(fields[curr_j + 2]).Trim().Length == 0))  // TT#3642 - JSmith - Store Eligibility upload is cancelling out all values
									//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    {
                                        // Begin TT#3611 - JSmith - issue with store eligibility node properties upload
                                        //FWOSmodifierModel = null;
                                        FWOSmodifierModel = Convert.ToString(Include.NoRID);
										//BEGIN TT#3676 - DOConnell - Sales Modifier value not clearing
                                        //FWOSmodifierValue = null;
                                        FWOSmodifierValue = DBNull.Value;
										//END TT#3676 - DOConnell - Sales Modifier value not clearing
                                        // End TT#3611 - JSmith - issue with store eligibility node properties upload
                                    }
                                    continue;

                                case "MinPlusSales":
                                    if (!buisnessRoutines.AllowElibigilityPMPlusSales())
                                    {
                                        error = MIDText.GetText(eMIDTextCode.msg_InvalidType);
                                        error = error.Replace("{0}", type + " - ");
                                        message = error + message;
                                        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                        returnCode = eReturnCode.severe;
                                        ++_recordsWithErrors;
                                    }
                                    else
                                    {
                                        //If fields[j + 1] is set to 1 or true then the checkbox will be set which means that Min Plus sales will be included in Forecasting
                                        //If fields[j + 1] is set to 0 or false the checkbox will be greyed out on the screen and any inheritance will be broken. This means that 
                                        //any inheritance is ignored and Min Plus Sales will not be included in Forecasting. 
                                        //If fields[j + 1] is set to “” or “ “ the checkbox will be cleared and any inheritance will be reestablished.
                                        try
                                        {
                                            if (fields[j + 1] != null)
                                            {
                                                string minPlsSlsStr = Convert.ToString(fields[j + 1]).Trim();
                                                MinPlusSalesUpdating = true; //TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales

                                                try
                                                {
                                                    if (minPlsSlsStr.Trim().Length > 0)
                                                    {
                                                        if ((minPlsSlsStr != "0" && minPlsSlsStr != "1"))
                                                        {
                                                            if (minPlsSlsStr.Length > 0 && Convert.ToBoolean(minPlsSlsStr) == true)
                                                            {
                                                                MinPlusSales = true;
                                                            }
                                                            else
                                                            {
                                                                //BEGIN TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                                                                //MinPlusSales = false;
                                                                MinPlusSales = DBNull.Value;
                                                                //END TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToInt32(minPlsSlsStr) == 1)
                                                            {
                                                                MinPlusSales = true;
                                                            }
                                                            else
                                                            {
                                                                //BEGIN TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                                                                //MinPlusSales = false;
                                                                MinPlusSales = DBNull.Value;
                                                                //END TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MinPlusSales = false;
                                                    }
                                                }
                                                catch
                                                {
                                                    error = MIDText.GetText(eMIDTextCode.msg_InvalidArgument);
                                                    error = error.Replace("{0}", "MinPlusSales " + minPlsSlsStr + " - ");
                                                    message = error + " - " + message;
                                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                                    returnCode = eReturnCode.severe;
                                                    ++_recordsWithErrors;
                                                }
                                            }
                                            // Begin TT#3703 - JSmith - Min Plus Sales should error if model is provided
                                            //else
                                            //{
                                            //    MinPlusSales = false;
                                            //}
                                            if (fields[j + 2] != null)
                                            {
                                                error = MIDText.GetText(eMIDTextCode.msg_Data1NotValidData2);
                                                error = error.Replace("{0}", "Model");
                                                error = error.Replace("{1}", "Option for Min Plus Sales");
                                                message = error + " - " + message;
                                                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                                returnCode = eReturnCode.severe;
                                                ++_recordsWithErrors;
                                            }
                                            // End TT#3703 - JSmith - Min Plus Sales should error if model is provided
                                        }
                                        catch
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvalidMinPlusSalesValue);
                                            error = error.Replace("{0}", fields[j + 1] + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                        }
                                    }
                                    continue;

                                default:
                                    {
                                        if ((j - 6) % 3 == 1 && type != null)
                                        {
                                            error = MIDText.GetText(eMIDTextCode.msg_InvalidType);
                                            error = error.Replace("{0}", type + " - ");
                                            message = error + message;
                                            _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                            returnCode = eReturnCode.severe;
                                            ++_recordsWithErrors;
                                        }
                                    }
                                    continue;
                            }
                        }
                        //BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                        if (sep == null)
                        {
                            sep = new StoreEligibilityProfile(storeRID);
                        }
                        //END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.

						//BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.

						////BEGIN TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
                        //if (simStores != DBNull.Value && dateRangeKey == Include.NoRID && sep.SimStoreUntilDateRangeRID == Include.NoRID)
                        //{
                        //    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                        //    error = error.Replace("{0}", " UntilDate ");
                        //    message = error + " - " + message;
                        //    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                        //    returnCode = eReturnCode.severe;
                        //    ++_recordsWithErrors;
                        //}
						////BEGIN TT#3668 - DOConnell - Clearing SimStores and Index fields only clears Index field
                        //else if (similarStoreChanged && dateRangeKey != Include.NoRID && sep.SimStoreUntilDateRangeRID != Include.NoRID)
                        //{
                        //    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                        //    error = error.Replace("{0}", " SimilarStore ID ");
                        //    message = error + " - " + message;
                        //    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                        //    returnCode = eReturnCode.severe;
                        //    ++_recordsWithErrors;
                        //}
                        ////else if (dateRangeKey != Include.NoRID && simStores == DBNull.Value && sep.SimStores.Count == 0)
                        ////{
                        ////    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                        ////    error = error.Replace("{0}", " SimilarStore ID ");
                        ////    message = error + " - " + message;
                        ////    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                        ////    returnCode = eReturnCode.severe;
                        ////    ++_recordsWithErrors;
                        ////}
						////END TT#3668 - DOConnell - Clearing SimStores and Index fields only clears Index field
                        ////BEGIN TT#3662 - DOConnell - Store Eligibility Transaction defaults similar store index to zero instead of 100
                        ////else if (simStoresIndex != DBNull.Value && simStores == DBNull.Value && sep.SimStores.Count == 0 && dateRangeKey == Include.NoRID && sep.SimStoreUntilDateRangeRID == Include.NoRID)
                        //else if (Convert.ToString(simStoresIndex) != string.Empty
                        //            && Convert.ToDouble(simStoresIndex) != Include.DefaultSimilarStoreRatio
                        //            && simStores == DBNull.Value && sep.SimStores.Count == 0
                        //            && dateRangeKey == Include.NoRID && sep.SimStoreUntilDateRangeRID == Include.NoRID)
                        //END TT#3662 - DOConnell - Store Eligibility Transaction defaults similar store index to zero instead of 100
                        //{
                        //    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                        //    error = error.Replace("{0}", " SimilarStore ID and UntilDate ");
                        //    message = error + " - " + message;
                        //    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                        //    returnCode = eReturnCode.severe;
                        //    ++_recordsWithErrors;
                        //}
						////END TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range

                        if (!clearingSimilarStores)  //TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                        {
                            if (similarStoreChanged)
                            {
                                //BEGIN TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                //if (simStores != DBNull.Value && Convert.ToString(dateRangeID) == string.Empty && sep.SimStoreUntilDateRangeRID == Include.NoRID)
                                //BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
								//if ((simStores != DBNull.Value && Convert.ToString(simStores) != string.Empty) && (Convert.ToString(dateRangeID) == string.Empty && sep.SimStoreUntilDateRangeRID == Include.NoRID))
                                if ((simStores != DBNull.Value && Convert.ToString(simStores) != string.Empty) && (dateRangeID == null || (Convert.ToString(dateRangeID) == string.Empty && sep.SimStoreUntilDateRangeRID == Include.NoRID)))
                                //END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field 
								//END TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                {
                                    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                                    error = error.Replace("{0}", " UntilDate ");
                                    message = error + " - " + message;
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                if (Convert.ToString(simStoresIndex) != string.Empty
                                    && Convert.ToDouble(simStoresIndex) != Include.DefaultSimilarStoreRatio
                                    && simStores == DBNull.Value
                                    && sep.SimStores.Count == 0
                                    && Convert.ToString(dateRangeID) == string.Empty
                                    && sep.SimStoreUntilDateRangeRID == Include.NoRID)
                                {
                                    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                                    error = error.Replace("{0}", " SimilarStore ID and UntilDate ");
                                    message = error + " - " + message;
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                //BEGIN TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                if (returnCode != eReturnCode.severe &&
									//BEGIN TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field
									//(dateRangeID != null || Convert.ToString(dateRangeID).Trim().Length != 0)
                                    (dateRangeID != null && Convert.ToString(dateRangeID).Trim().Length != 0)
									//END TT#3683 - DOConnell - API transactions not accepting 1 or 0 for the Ineligible field
                                    && ((simStores == DBNull.Value && Convert.ToString(simStores).Trim().Length == 0)))
                                {
                                    error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                                    error = error.Replace("{0}", " SimilarStore ID ");
                                    message = error + " - " + message;
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                            }
                        }
						//END TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                        //else
                        //{
						//BEGIN TT#3675 - DOConnell - Store Eligibility - Sending file without Similar Store ID does not error
						//if ((dateRangeID != null || Convert.ToString(dateRangeID) == string.Empty) && (simStores != DBNull.Value || sepSimStores.Count != 0))
                        //    if ((dateRangeID != null || Convert.ToString(dateRangeID) == string.Empty) && (simStores == DBNull.Value || sepSimStores.Count != 0))
                        //    //END if ((dateRangeID != null || Convert.ToString(dateRangeID) == string.Empty) && (simStores != DBNull.Value || sepSimStores.Count != 0))
                        //    {
                        //        error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                        //        error = error.Replace("{0}", " SimilarStore ID ");
                        //        message = error + " - " + message;
                        //        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                        //        returnCode = eReturnCode.severe;
                        //        ++_recordsWithErrors;
                        //    }
                        //    if (Convert.ToString(simStoresIndex) != string.Empty
                        //        && Convert.ToDouble(simStoresIndex) != Include.DefaultSimilarStoreRatio
                        //        && simStores == DBNull.Value 
                        //        && sep.SimStores.Count == 0
                        //        && Convert.ToString(dateRangeID) == string.Empty 
                        //        && sep.SimStoreUntilDateRangeRID == Include.NoRID)
                        //    {
                        //        error = MIDText.GetText(eMIDTextCode.msg_mustHaveTimePeriod);
                        //        error = error.Replace("{0}", " SimilarStore ID and UntilDate ");
                        //        message = error + " - " + message;
                        //        _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                        //        returnCode = eReturnCode.severe;
                        //        ++_recordsWithErrors;
                        //    }
                        //}

						//END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.



                        if (returnCode == eReturnCode.successful)
                        {
							//BEGIN TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
                            if (sep == null)
                            {
                                sep = new StoreEligibilityProfile(storeRID);
                            }
							//END TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
							//TT#4197 - DOConnell - Cannot apply a Blank Ineligible check to a column
                            if (sep.EligType != eEligibilitySettingType.SetIneligible)
                            {
                                if (eligModKey != System.DBNull.Value || eligibilityModel != System.DBNull.Value || Ineligible != System.DBNull.Value)
                                {
                                    sep.EligIsInherited = false;
                                    sep.EligInheritedFromNodeRID = Include.NoRID;
                                }
                            }
							//TT#4197 - DOConnell - Cannot apply a Blank Ineligible check to a column
							//BEGIN TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                            //if (MinPlusSales != System.DBNull.Value)
                            if (MinPlusSalesUpdating || MinPlusSales != System.DBNull.Value)
							//END TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                            {
                                sep.PresPlusSalesIsInherited = false;
                                sep.PresPlusSalesInheritedFromNodeRID = Include.NoRID;
                            }
                            if (stockmodifierModel != System.DBNull.Value || stockmodifierValue != System.DBNull.Value)
                            {
                                sep.StkModIsInherited = false;
                                sep.StkModInheritedFromNodeRID = Include.NoRID;
                            }
                            if (salesmodifierModel != System.DBNull.Value || salesmodifierValue != System.DBNull.Value)
                            {
                                sep.SlsModIsInherited = false;
                                sep.SlsModInheritedFromNodeRID = Include.NoRID;
                            }
                            if (FWOSmodifierModel != System.DBNull.Value || FWOSmodifierValue != System.DBNull.Value)
                            {
                                sep.FWOSModIsInherited = false;
                                sep.FWOSModInheritedFromNodeRID = Include.NoRID;
                            }
							//BEGIN TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                            if (!sep.EligIsInherited)
                            {
                                if (Ineligible == System.DBNull.Value && eligibilityModel != System.DBNull.Value)
                                {
                                    Ineligible = false;
                                }
                            }
							//END TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
							//BEGIN TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
							//if (simStores != System.DBNull.Value)
							//BEGIN TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales
                            if (similarStoreChanged)
                            {
                                if (simStores != System.DBNull.Value && Convert.ToString(simStores).Trim().Length > 0)
                                {
                                    //BEGIN TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range
                                    //BEGIN TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                    //if (simStoresIndex == DBNull.Value || Convert.ToDouble(simStoresIndex) == 0.0)
                                    if (simStoresIndex != DBNull.Value && Convert.ToString(simStoresIndex).Trim().Length != 0)
                                    {
                                        if (Convert.ToDouble(simStoresIndex) == 0.0)
                                        {
                                            if (sep.SimStoreRatio > 0.0)
                                            {
                                                simStoresIndex = sep.SimStoreRatio;
                                            }
                                            else
                                            {
                                                simStoresIndex = Include.DefaultSimilarStoreRatio;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        simStoresIndex = Include.DefaultSimilarStoreRatio;
                                    }
                                    //END TT#3679 - DOConnell - Store Eligibility - Updating  Stock Modifier Errors
                                    if (dateRangeKey == Include.NoRID && sep.SimStoreUntilDateRangeRID != Include.NoRID)
                                    {
                                        dateRangeKey = sep.SimStoreUntilDateRangeRID;
                                        dateRangeID = sep.SimStoreDisplayDate; //TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    }
                                    sep.SimStoreIsInherited = false;
                                    sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                }
                                else if (Convert.ToString(simStores).Trim().Length == 0)
                                {
                                    // Begin TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                                    if (sep.SimStoreType != eSimilarStoreType.None)
                                    {
                                        sep.SimStoresChanged = true;
                                        sep.SimStoreType = eSimilarStoreType.None;
                                        sep.SimStores = null;
                                        sep.SimStoreRatio = Include.Undefined;
                                        sep.SimStoreUntilDateRangeRID = Include.NoRID;
                                        sep.SimStoreIsInherited = false;
                                        sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                    }
                                    // End TT#3678 - JSmith - Similar Store information not cleared consistently with Node Properties
                                }
                                //END //BEGIN TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
                                //BEGIN TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                ////BEGIN TT#3667 - DOConnell - Clearing Ineligible checkbox also clears other fields
                                //else if (simStores == System.DBNull.Value && Convert.ToString(simStoresIndex) != string.Empty && sep.SimStoreRatio != Include.DefaultSimilarStoreRatio)
                                //{
                                //    simStoresIndex = sep.SimStoreRatio;
                                //}
                                ////END TT#3667 - DOConnell - Clearing Ineligible checkbox also clears other fields
                                //END TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                if (dateRangeKey != Include.NoRID)
                                {
                                    if (simStores == DBNull.Value && sep.SimStores != null)
                                    {
                                        simStores = sep.SimStores;
                                    }
                                    if (simStoresIndex == DBNull.Value)
                                    {
                                        simStoresIndex = sep.SimStoreRatio;
                                    }
                                    sep.SimStoreIsInherited = false;
                                    sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                }
                                //BEGIN TT#3667 - DOConnell - Clearing Ineligible checkbox also clears other fields
                                //BEGIN TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
                                //else if (sep.SimStoreUntilDateRangeRID != Include.NoRID)
                                //{
                                //    dateRangeKey = sep.SimStoreUntilDateRangeRID;
                                //    dateRangeID = sep.SimStoreDisplayDate;
                                //}
                                //END TT#3678 - DOConnell - Similar Store information not cleared consistently with Node Properties
                                //END TT#3667 - DOConnell - Clearing Ineligible checkbox also clears other fields
                                if (Convert.ToString(simStoresIndex) != string.Empty && Convert.ToDouble(simStoresIndex) != Include.DefaultSimilarStoreRatio)
                                {
                                    if (simStores == DBNull.Value && sep.SimStores != null)
                                    {
                                        simStores = sep.SimStores;
                                    }
                                    if (dateRangeKey == Include.NoRID && sep.SimStoreUntilDateRangeRID != Include.NoRID)
                                    {
                                        dateRangeKey = sep.SimStoreUntilDateRangeRID;
                                        dateRangeID = sep.SimStoreDisplayDate; //TT#3664 - DOConnell - Input string was not in a correct format when clearing similar stores.
                                    }
                                    sep.SimStoreIsInherited = false;
                                    sep.SimStoreInheritedFromNodeRID = Include.NoRID;
                                }
                            }
							//END TT#3699 - DOConnell - Uploading Sales Modifier Model turned off Min+Sales 
							//END TT#3661 - DOConnell - Eligibility Upload - Similar Store Date Range

                            try
                            {
                                _storeEligibilityDataSet.Tables["Stores"].Rows.Add(new object[] {null, sep.EligIsInherited, sep.EligInheritedFromNodeRID, true, storeRID, storeID, 
                                                                                          eligModKey, eligibilityModel, Ineligible, 
                                                                                          MinPlusSales, sep.PresPlusSalesIsInherited, sep.PresPlusSalesInheritedFromNodeRID,
                                                                                          sep.StkModIsInherited, sep.StkModInheritedFromNodeRID, stockmodifierModel, stockmodifierValue,
                                                                                          sep.SlsModIsInherited, sep.SlsModInheritedFromNodeRID, salesmodifierModel, salesmodifierValue,
                                                                                          sep.FWOSModIsInherited, sep.FWOSModInheritedFromNodeRID, FWOSmodifierModel, FWOSmodifierValue,
                                                                                          sep.SimStoreIsInherited, sep.SimStoreInheritedFromNodeRID, 
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
                            catch (Exception err)
                            {
                                message = err.ToString();
                                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }
                        else
                        {
                            returnCode = eReturnCode.successful;
                        }




                        if (EOF)
                        {
                            ErrorList = _hm.ValidEligibilityData(_storeList, _storeEligibilityDataSet, _storeEligList, ErrorList);
                            
                            for (int e = 0; e < ErrorList.Count; e++)
                            {
                                StoreEligibilityErrors see = (StoreEligibilityErrors)ErrorList[e];
                                if (see.typeErr == true || see.simStoreErr == true)
                                {
                                    //Begin TT#4375 - DOConnell - Store eligibility error
                                    string errMessage = MIDText.GetText(eMIDTextCode.msg_InvalidArgument, false);
                                    string detMessage = see.message;
                                    eMIDTextCode errRID = eMIDTextCode.msg_InvalidArgument;
                                    if (see.simStoreErr)
                                    {
                                        // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                        //StoreProfile sp1 = StoreMgmt.StoreProfile_Get(see.storeRID); // _SAB.StoreServerSession.GetStoreProfile(see.storeRID);
                                        StoreProfile sp1 = (StoreProfile)_storeList.FindKey(see.storeRID);
                                        // End TT#1902-MD - JSmith - Store Services - VSW API Error
                                        string errNodeID = _SAB.HierarchyServerSession.GetNodeID(nodeRID);
                                        errRID = eMIDTextCode.msg_StoreNotFound;
                                        detMessage = detMessage.Replace("{0}", see.dataString);
                                        detMessage = detMessage.Replace("{1}", errNodeID);
                                        detMessage = detMessage.Replace("{2}", sp1.StoreId.ToString());
                                    }
                                    //_SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidArgument, see.message, sourceModule);
                                    _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, detMessage, sourceModule);
                                    //End TT#4375 - DOConnell - Store eligibility error
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    _storeEligList = see.sel;
                                }
                            }

                            // Begin TT#3691 - JSmith - Combination of product found and product not found causes error
                            //_SAB.HierarchyServerSession.StoreEligibilityUpdate(nodeRID, _storeEligList, false);
                            if (nodeRID != Include.NoRID)
                            {
                                _SAB.HierarchyServerSession.StoreEligibilityUpdate(nodeRID, _storeEligList, false);
                            }
                            // End TT#3691 - JSmith - Combination of product found and product not found causes error
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                errorFound = true;
                _SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                throw;
            }
            finally
            {
                _SAB.HierarchyServerSession.Audit.StoreEligibilityCriteriaLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _recordsAddedUpdated);
            }
            return returnCode;
        }
    }
}
