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



namespace MIDRetail.ChainSetPercentCriteriaLoad
{
    /// <summary>
    /// Summary description for ChainSetPercentCriteriaLoadProcess.
    /// </summary>
    public class ChainSetPercentCriteriaLoadProcess
    {
        private string sourceModule = "ChainSetPercentCriteriaLoadProcess.cs";
        private ChainSetPercentList _chainSetPercentList;
        private ProfileList _storeList;
        private HierarchyNodeProfile _hnp;
        private ProfileList _CSPweeks;
        private ChainSetPercent _cspl = null;
        SessionAddressBlock _SAB;
        protected ChainSetPercentCriteriaData _cspcd = null;
        protected Audit _audit = null;
        protected int _recordsRead = 0;
        protected int _recordsWithErrors = 0;
        protected int _recordsAddedUpdated = 0;

        public class ChainSetPercentValues
        {
            private int _yearWeek;
            private int _nodeRID;
            private int _SG_RID;
            private int _SGL_RID;
            private decimal _percent;
            private string _SG_ID;
            private string _SGL_ID;



            public int yearWeek
            {
                get { return _yearWeek; }
                set { _yearWeek = value; }
            }

            public int nodeRID
            {
                get { return _nodeRID; }
                set { _nodeRID = value; }
            }

            public int SG_RID
            {
                get { return _SG_RID; }
                set { _SG_RID = value; }
            }

            public int SGL_RID
            {
                get { return _SGL_RID; }
                set { _SGL_RID = value; }
            }

            public decimal percent
            {
                get { return _percent; }
                set { _percent = value; }
            }

            public string SG_ID
            {
                get { return _SG_ID; }
                set { _SG_ID = value; }
            }

            public string SGL_ID
            {
                get { return _SGL_ID; }
                set { _SGL_ID = value; }
            }

            public ChainSetPercentValues(int yearWeek, int nodeRID, int SG_RID, int SGL_RID, decimal percent, string SG_ID, string SGL_ID)
            {
                _yearWeek = yearWeek;
                _nodeRID = nodeRID;
                _SG_RID = SG_RID;
                _SGL_RID = SGL_RID;
                _percent = percent;
                _SG_ID = SG_ID;
                _SGL_ID = SGL_ID;

            }

        }

        public class ChainSetKeyValues : IComparable<ChainSetKeyValues>
        {
            private int _index;
            private int _value;
            private double _Pct;
            private int _Week;

            public int CompareTo(ChainSetKeyValues other)
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

            public ChainSetKeyValues(int index, int aValue, double bValue, int cValue)
            {
                _index = index;
                _value = aValue;
                _Pct = bValue;
                _Week = cValue;

            }

        }

        public class XMLChainSetPercentProcessException : Exception
        {
            /// <summary>
            /// Used when throwing exceptions in the XML ColorCode Load Class
            /// </summary>
            /// <param name="message">The error message to display</param>
            public XMLChainSetPercentProcessException(string message)
                : base(message)
            {
            }
            public XMLChainSetPercentProcessException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        public ChainSetPercentCriteriaLoadProcess(SessionAddressBlock SAB, ref bool errorFound)
        {
            try
            {
                _SAB = SAB;
                _audit = _SAB.ClientServerSession.Audit;
                _cspcd = new ChainSetPercentCriteriaData();
                StoreMgmt.LoadInitialStoresAndGroups(_SAB, _SAB.ClientServerSession, true);    // TT#5810 - JSmith - Size Constraint API failing
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
            StreamReader reader = null;
            string line = null;
            string message = null;
            string merchId = null;
            string storeAttribute = null;
            int yearWeek = 0;
            eReturnCode returnCode = eReturnCode.successful;
            List<string> lines = new List<string>();
            HierarchyMaintenance _hm = new HierarchyMaintenance(_SAB);
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
                    if (returnCode != eReturnCode.successful)
                    {
                        //Begin TT#1391-MD -jsobek -Object Reference Error - Chain Set Percent
                        _cspcd.OpenUpdateConnection();
                        try
                        {
                            for (int s = 0; s < lines.Count; s++)
                            {
                                if (s <= 1)
                                {
                                    string[] fields2 = MIDstringTools.Split(lines[s], delimiter[0], true);
                                    yearWeek = Convert.ToInt32(fields2[0]);
                                    merchId = Convert.ToString(fields2[1]);
                                    storeAttribute = Convert.ToString(fields2[2]);
                                    HierarchyNodeProfile hnp = _hm.NodeLookup(ref em, merchId, false, false);
                                    int deletecode = _cspcd.ChainSetPercentCriteria_Delete(hnp.Key, yearWeek);
                                }
                            }
                            _cspcd.CommitData();
                        }
                        finally
                        {
                            _cspcd.CloseUpdateConnection();
                        }
                        //End TT#1391-MD -jsobek -Object Reference Error - Chain Set Percent                      
                    }

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
            var KeyList = new List<ChainSetKeyValues>();
            List<string> lines = new List<string>();
            int yearWeek = 0;
            string merchId = string.Empty;
            string storeAttribute = string.Empty;
            string storeAttributeSet = string.Empty;
            string line = string.Empty;
            string message = string.Empty;
            eReturnCode returnCode = eReturnCode.successful;
            HierarchyMaintenance _hm = new HierarchyMaintenance(_SAB);
            EditMsgs em = new EditMsgs();

            if (!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
            {
                throw new XMLChainSetPercentProcessException(String.Format("Can not find the file located at '{0}'", fileLocation));
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
            try
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "ChainSetPercent";
                xRoot.IsNullable = true;

                XmlSerializer s = new XmlSerializer(typeof(ChainSetPercent), xRoot);	// Create a Serializer

                r = new StreamReader(fileLocation);			// Load the Xml File

                _cspl = (ChainSetPercent)s.Deserialize(r);	// Deserialize the Xml File to a strongly typed object

            }
            catch (Exception ex)
            {
                throw new XMLChainSetPercentProcessException(String.Format("Error encountered during deserialization of the file '{0}'", fileLocation), ex);
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            finally
            {
                if (r != null)
                    r.Close();
            }
            // End Track #4229

            try
            {
                foreach (ChainSetPercentLine l in _cspl.Items)
                {
                    try
                    {
                        line = l.yearWeek + delimiter[0] + l.nodeId + delimiter[0] + l.storeAttribute + delimiter[0] + l.storeAttributeSet + delimiter[0] + l.percentage;
                        lines.Add(line);
                    }
                    catch (Exception ex)
                    {
                        throw new XMLChainSetPercentProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
                    }

                }
                returnCode = ValidateFileData(lines, delimiter, ref errorFound);
                if (returnCode != eReturnCode.successful)
                {
                    // Begin TT#1342-MD - JSmith - Chain Set Percent API Error
                    //for (int s = 0; s < lines.Count; s++)
                    //{
                    //    if (s <= 1)
                    //    {
                    //        string[] fields2 = MIDstringTools.Split(lines[s], delimiter[0], true);
                    //        yearWeek = Convert.ToInt32(fields2[0]);
                    //        merchId = Convert.ToString(fields2[1]);
                    //        storeAttribute = Convert.ToString(fields2[2]);
                    //        HierarchyNodeProfile hnp = _hm.NodeLookup(ref em, merchId, false, false);
                    //        int deletecode = _cspcd.ChainSetPercentCriteria_Delete(hnp.Key, yearWeek);
                    //    }
                    //}
                    _cspcd.OpenUpdateConnection();
                    try
                    {
                        for (int s = 0; s < lines.Count; s++)
                        {
                            if (s <= 1)
                            {
                                string[] fields2 = MIDstringTools.Split(lines[s], delimiter[0], true);
                                yearWeek = Convert.ToInt32(fields2[0]);
                                merchId = Convert.ToString(fields2[1]);
                                storeAttribute = Convert.ToString(fields2[2]);
                                HierarchyNodeProfile hnp = _hm.NodeLookup(ref em, merchId, false, false);
                                int deletecode = _cspcd.ChainSetPercentCriteria_Delete(hnp.Key, yearWeek);
                            }
                        }
                        _cspcd.CommitData();
                    }
                    finally
                    {
                        _cspcd.CloseUpdateConnection();
                    }
                    // End TT#1342-MD - JSmith - Chain Set Percent API Error

                    message = ("Error Inserting data for " + merchId + " - " + yearWeek + " - " + storeAttribute + " transaction has been canceled");
                    _audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
                }
            }
            catch (Exception ex)
            {
                throw new XMLChainSetPercentProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
            }
            return returnCode;
        }

        public eReturnCode ValidateFileData(List<string> lines, char[] delimiter, ref bool errorFound)
        {
            string nodeId = null;
            string attribute = null;
            string newAttribute = null;
            bool EOF = false;
            string newNode = null;
            string message = null;
            int yearWeek = 0;
            int newWeek = 0;
            decimal pct = 0;
            ArrayList curYearWeek = new ArrayList();
            eReturnCode returnCode = eReturnCode.successful;
            eReturnCode methodReturnCode = eReturnCode.successful;  // TT#1942-MD - JSmith - Chain Set Percent API Error
            List<string> lineData = new List<string>();

            try
            {
                if (lines != null && lines.Count > 0)
                {
                    for (int s = 0; s < lines.Count; s++)
                    {
                        if (s == lines.Count - 1) EOF = true;
                        string[] fields = MIDstringTools.Split(lines[s], delimiter[0], true);
                        if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                        {
                            continue;
                        }
                        returnCode = eReturnCode.successful;
                        if (fields.Length == 5)
                        {
                            try
                            {
                                if (fields[0].Length != 6)
                                {
                                    message = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentCriteriaName);
                                    message = message.Replace("{0}", fields[0]);
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    
                                    yearWeek = Convert.ToInt32(fields[0]);
                                    try
                                    {
                                        _SAB.ClientServerSession.Calendar.GetFiscalWeek(yearWeek);
                                    }
                                    catch (Exception)
                                    {
                                        message = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentCriteriaName);
                                        message = message.Replace("{0}", fields[0]);
                                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                        returnCode = eReturnCode.severe;
                                        ++_recordsWithErrors;
                                    }
                                }
                                
                            }
                            catch (Exception)
                            {
                                message = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentCriteriaName);
                                message = message.Replace("{0}", fields[0]);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                                
                                nodeId = Convert.ToString(fields[1]);
                                attribute = Convert.ToString(fields[2]);
                            try
                            {
                                pct = Convert.ToDecimal(fields[4]);
                            }
                            catch
                            {
                                message = ("Set Percentage " + fields[4] + " Changed to 0");
                                _audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
                                pct = 0;
                            }

                            // Begin TT#1942-MD - JSmith - Chain Set Percent API Error
                            if (returnCode != eReturnCode.successful
                                && methodReturnCode == eReturnCode.successful)
                            {
                                methodReturnCode = returnCode;
                            }
                            // End TT#1942-MD - JSmith - Chain Set Percent API Error

                            ++_recordsRead;
                            if ((newNode == null | nodeId == newNode) && (newWeek == 0 | yearWeek == newWeek) && (newAttribute == null | attribute == newAttribute))
                            {
                                newNode = nodeId;
                                newWeek = yearWeek;
                                newAttribute = attribute;
                                lineData.Add(lines[s]);
                            }
                            else
                            {
                                returnCode = SeparateDelimitedData(lineData, delimiter, true);
                                // Begin TT#1942-MD - JSmith - Chain Set Percent API Error
                                if (returnCode != eReturnCode.successful
                                    && methodReturnCode == eReturnCode.successful)
                                {
                                    methodReturnCode = returnCode;
                                }
                                // End TT#1942-MD - JSmith - Chain Set Percent API Error

                                    lineData.Clear();
                                    lineData.Add(lines[s]);
                                    newNode = nodeId;
                                    newWeek = yearWeek;
                                    newAttribute = attribute;
                                
                            }
                            if (EOF)
                            {
                                returnCode = SeparateDelimitedData(lineData, delimiter, true);
                                // Begin TT#1942-MD - JSmith - Chain Set Percent API Error
                                if (returnCode != eReturnCode.successful
                                    && methodReturnCode == eReturnCode.successful)
                                {
                                    methodReturnCode = returnCode;
                                }
                                // End TT#1942-MD - JSmith - Chain Set Percent API Error

                                lineData.Clear();
                                lineData.Add(lines[s]);
                                newNode = nodeId;
                                newWeek = yearWeek;
                                newAttribute = attribute;
                            }
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
                _audit.ChainSetPercentCriteriaLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _recordsAddedUpdated);
                _cspcd.CloseUpdateConnection();
            }
            // Begin TT#1942-MD - JSmith - Chain Set Percent API Error
            //return returnCode;
            return methodReturnCode;
            // End TT#1942-MD - JSmith - Chain Set Percent API Error
        }

        private eReturnCode SeparateDelimitedData(List<string> lines, char[] delimiter, bool delete)
        {
            eReturnCode returnCode = eReturnCode.successful;
            int yearweek = 0;
            int nodeRID = 0;
            int SGL_RID = 0;
            int SG_RID = 0;
            int hnReturn = -1;
            decimal totPct = 0;
            string merchId = string.Empty;
            string storeAttribute = string.Empty;
            string storeAttributeSet = string.Empty;
            decimal percent = 0;
            string returnMessage;
            HierarchyMaintenance _hm = new HierarchyMaintenance(_SAB);
            EditMsgs em = new EditMsgs();
            SortedList changeList = new SortedList();
            

            try
            {
                if (lines != null && lines.Count > 0)
                {
                    for (int s = 0; s < lines.Count; s++)
                    {
                        returnCode = eReturnCode.successful;
                        string[] fields = MIDstringTools.Split(lines[s], delimiter[0], true);

                        yearweek = Convert.ToInt32(fields[0]);
                        merchId = Convert.ToString(fields[1]);
                        storeAttribute = Convert.ToString(fields[2]);
                        storeAttributeSet = Convert.ToString(fields[3]);
                        if (decimal.TryParse(Convert.ToString(fields[4]), out percent)) { }
                        totPct = totPct + percent;

                        try
                        {
                            nodeRID = _SAB.HierarchyServerSession.GetNodeRID(merchId);
                        }
                        catch
                        {
                            returnMessage = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentMerchID);
                            returnMessage = returnMessage.Replace("{0}", Convert.ToString(nodeRID));
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_pl_InvalidNodeIdSpecified, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;
                        }

                        SG_RID = StoreMgmt.StoreGroup_GetRidFromId(storeAttribute); //_SAB.StoreServerSession.GetStoreGroupRID(storeAttribute);
                        if (SG_RID > Include.NoRID && StoreMgmt.DoesGroupLevelNameExist(SG_RID, storeAttributeSet)) //_SAB.StoreServerSession.DoesGroupLevelNameExist(SG_RID, storeAttributeSet))
                        {
                            ProfileList sglpl = StoreMgmt.StoreGroup_GetLevelListViewList(SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(SG_RID);
                            foreach (StoreGroupLevelListViewProfile sglp in sglpl)
                            {
                                if (sglp.Name == storeAttributeSet)
                                {
                                    SGL_RID = sglp.Key;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            returnMessage = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentStoreAtt);
                            returnMessage = returnMessage.Replace("{0}", storeAttribute);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InvalidAttributeName, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;


                        }
                        // Begin TT#1719 - JSmith - Chain Set Pct:  System Overflow exception in Node Properties
                        //int changeKey = Convert.ToInt32(fields[0] + Convert.ToString(SGL_RID));
                        int changeKey = Convert.ToInt32((Convert.ToInt32(fields[0]) - 200000) + Convert.ToString(SGL_RID));
                        // End TT#1719
                        ChainSetPercentValues changeValueList = new ChainSetPercentValues(yearweek, nodeRID, SG_RID, SGL_RID, percent, storeAttribute, storeAttributeSet);
                        if (!changeList.Contains(changeKey))
                            changeList.Add(changeKey, changeValueList);


                    }
                }

                if (totPct != 100 && totPct > 0)
                {
                    _recordsWithErrors = _recordsWithErrors + changeList.Count;
                    returnMessage = (" Merchandise Id " + merchId + " Percentages (" + totPct + ") for Store Attribute " + storeAttributeSet + " and Plan Week " + yearweek + " must equal 100 or 0");
                    _audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_MustEqual100, returnMessage, sourceModule);
                    _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.msg_MustEqual100, "", _SAB.GetHighestAuditMessageLevel());
                    returnCode = eReturnCode.severe;

                }

                else
                {
                    if (returnCode == eReturnCode.successful)
                    {
                        int aYearWeekId = _SAB.ApplicationServerSession.Calendar.GetWeekKey(yearweek);
                        WeekProfile wp = _SAB.ApplicationServerSession.Calendar.GetWeek(aYearWeekId);
                        _CSPweeks = new ProfileList(eProfileType.Week);
                        _CSPweeks.Add(wp);
                        _storeList = StoreMgmt.StoreGroup_GetLevelListFilled(SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelList(SG_RID);

                        _chainSetPercentList = _SAB.HierarchyServerSession.GetChainSetPercentList(_storeList, nodeRID, false, false, true, _CSPweeks);

                        foreach (int clKey in changeList.Keys)
                        {
                            if (_chainSetPercentList.Contains(Convert.ToInt32(clKey)))
                            {
                                foreach (ChainSetPercentProfiles cspp in _chainSetPercentList)
                                {
                                    if (cspp.Key == Convert.ToInt32(clKey))
                                    {
                                        _SAB.HierarchyServerSession.DeleteChainSetPercentData(cspp.NodeRID, cspp.TimeID);
                                        ChainSetPercentValues cspv = (ChainSetPercentValues)changeList[clKey];

                                        if (totPct == 100)
                                        {
                                            cspp.ChainSetPercent = cspv.percent;
                                            cspp.StoreGroupID = cspv.SG_ID;
                                            cspp.StoreGroupVersion = StoreMgmt.StoreGroup_GetVersion(cspv.SG_RID);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                            cspp.ChainSetPercentChangeType = eChangeType.update;

                                        }
                                        else if (totPct != 100 && totPct > 0)
                                        {
                                            cspp.ChainSetPercentChangeType = eChangeType.none;


                                        }
                                        else if (totPct <= 0)
                                        {
                                            cspp.ChainSetPercentChangeType = eChangeType.delete;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ChainSetPercentValues cspv = (ChainSetPercentValues)changeList[clKey];
                                ChainSetPercentProfiles cspp = new ChainSetPercentProfiles(clKey);
                                cspp.ChainSetPercent = cspv.percent;
                                if (totPct > 0)
                                {
                                    cspp.ChainSetPercentChangeType = eChangeType.add;
                                }
                                else
                                {
                                    cspp.ChainSetPercentChangeType = eChangeType.delete;
                                }
                                cspp.Key = clKey;
                                cspp.NodeID = merchId;
                                cspp.NodeRID = cspv.nodeRID;
                                cspp.StoreGroupID = cspv.SG_ID;
                                cspp.StoreGroupRID = cspv.SG_RID;
                                cspp.StoreGroupVersion = StoreMgmt.StoreGroup_GetVersion(cspp.StoreGroupRID);  // TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
                                cspp.StoreGroupLevelID = cspv.SGL_ID;
                                cspp.StoreGroupLevelRID = cspv.SGL_RID;
                                cspp.TimeID = yearweek;

                                _chainSetPercentList.Add(cspp);
                            }
                        }

                        hnReturn = _SAB.HierarchyServerSession.ChainSetPercentUpdate(nodeRID, _chainSetPercentList, aYearWeekId, false); // TT#2015 - gtaylor - apply changes to lower levels
                        if (hnReturn <= 0)
                        {
                            returnCode = eReturnCode.successful;
                            _recordsAddedUpdated = _recordsAddedUpdated + lines.Count;
                        }

                        else if (hnReturn == 1)
                        {
                            returnMessage = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentMerchID);
                            returnMessage = returnMessage.Replace("{0}", Convert.ToString(nodeRID));
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_pl_InvalidNodeIdSpecified, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;
                        }
                        else if (hnReturn == 2)
                        {
                            returnMessage = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentPlanWeek);
                            returnMessage = returnMessage.Replace("{0}", Convert.ToString(yearweek));
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;
                        }
                        else if (hnReturn == 3)
                        {
                            returnMessage = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentStoreAtt);
                            returnMessage = returnMessage.Replace("{0}", storeAttribute);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InvalidAttributeName, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;
                        }
                        else if (hnReturn == 4)
                        {
                            returnMessage = MIDText.GetText(eMIDTextCode.msg_InvalidChainSetPercentStoreAttSet);
                            returnMessage = returnMessage.Replace("{0}", storeAttributeSet);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InvalidAttributeSetName, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;
                        }

                        else if (hnReturn == 5)
                        {
                            returnMessage = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteCancelled);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_DeleteCancelled, returnMessage, sourceModule);
                            returnCode = eReturnCode.severe;
                        }
                        
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            
            return returnCode;
        }

        private decimal CalculatePercentTotal(List<string> lineData, string[] fields, char[] delimiter)
        {
            decimal pct = 0;
            decimal pctTotal = 0;
            for (int p = 0; p < lineData.Count; p++)
            {
                string[] fields2 = MIDstringTools.Split(lineData[p], delimiter[0], true);

                try
                {
                    pct = Convert.ToDecimal(fields2[4]);
                }
                catch
                {
                    pct = 0;
                }
                pctTotal = pctTotal + pct;
            }
            return pctTotal;
        }

    }
}
