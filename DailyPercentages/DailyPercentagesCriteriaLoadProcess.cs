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

namespace MIDRetail.DailyPercentagesCriteriaLoad
{
    public class DailyPercentagesCriteriaLoadProcess
    {
        private string sourceModule = "DailyPercentagesCriteriaLoadProcess.cs";
        private StoreDailyPercentagesList _storeDailyPercentagesList;
        private ProfileList _storeList;
        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
        protected Hashtable _stores = null;
        protected Hashtable _nodes = null;
        // End TT#2621 - JSmith - Duplicate weeks in daily sales
        private HierarchyNodeProfile _hnp;
        private ProfileList _DPweeks;
        private DailyPercentages _cspl = null;
        SessionAddressBlock _SAB;
        protected DailyPercentagesCriteriaData _dpcd = null;
        protected Audit _audit = null;
        protected int _recordsRead = 0;
        protected int _recordsWithErrors = 0;
        protected int _recordsAddedUpdated = 0;
        // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
        HierarchyMaintenance _hm;
        // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
        
        public class DailyPercentagesValues
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


            public DailyPercentagesValues(int HNRID, int STRID, DateRangeProfile DRPROF, decimal D1, decimal D2, decimal D3, decimal D4, decimal D5, decimal D6, decimal D7)
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

        public class XMLDailyPercentagesProcessException : Exception
        {
            /// <summary>
            /// Used when throwing exceptions in the XML ColorCode Load Class
            /// </summary>
            /// <param name="message">The error message to display</param>
            public XMLDailyPercentagesProcessException(string message)
                : base(message)
            {
            }
            public XMLDailyPercentagesProcessException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        public DailyPercentagesCriteriaLoadProcess(SessionAddressBlock SAB, ref bool errorFound)
        {
            try
            {
                _SAB = SAB;
                _audit = _SAB.ClientServerSession.Audit;
                _dpcd = new DailyPercentagesCriteriaData();
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
            string merchId = null;
            string storeAttribute = null;
            int yearWeek = 0;
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
                    if (returnCode != eReturnCode.successful)
                    {
                        //for (int s = 0; s < lines.Count; s++)
                        //{
                        //    if (s <= 1)
                        //    {
                        //        _dpcd.OpenUpdateConnection();

                        //        string[] fields2 = MIDstringTools.Split(lines[s], delimiter[0], true);
                        //        yearWeek = Convert.ToInt32(fields2[0]);
                        //        merchId = Convert.ToString(fields2[1]);
                        //        storeAttribute = Convert.ToString(fields2[2]);
                        //        HierarchyNodeProfile hnp = _hm.NodeLookup(ref em, merchId, false, false);
                        //        try
                        //        {
                        //            if (_dpcd.DailyPercentagesCriteria_Delete(hnp.Key, yearWeek) > 1)
                        //            {
                        //                message = MIDText.GetText(eMIDTextCode.msg_DeleteFailed);
                        //                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_DeleteFailed, message, sourceModule);
                        //                returnCode = eReturnCode.severe;
                        //            }
                                    
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            errorFound = true;
                        //            _audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                        //            _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                        //            throw;
                        //        }
                        //    }
                        //}
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
            var KeyList = new List<DailyKeyValues>();
            List<string> lines = new List<string>();
            int yearWeek = 0;
            string merchId = string.Empty;
            string storeAttribute = string.Empty;
            string storeAttributeSet = string.Empty;
            string line = string.Empty;
            string message = string.Empty;
            eReturnCode returnCode = eReturnCode.successful;
            // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
            //HierarchyMaintenance _hm = new HierarchyMaintenance(_SAB);
            // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
            EditMsgs em = new EditMsgs();

            if (!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
            {
                throw new XMLDailyPercentagesProcessException(String.Format("Can not find the file located at '{0}'", fileLocation));
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
            try
            {
                //XmlRootAttribute xRoot = new XmlRootAttribute();
                //xRoot.ElementName = "DailyPercentages";
                //xRoot.IsNullable = true;

                XmlSerializer s = new XmlSerializer(typeof(DailyPercentages));	// Create a Serializer
                //XmlSerializer s = new XmlSerializer(typeof(DailyPercentages), xRoot);	// Create a Serializer

                r = new StreamReader(fileLocation);			// Load the Xml File

                _cspl = (DailyPercentages)s.Deserialize(r);	// Deserialize the Xml File to a strongly typed object

            }
            catch (Exception ex)
            {
                throw new XMLDailyPercentagesProcessException(String.Format("Error encountered during deserialization of the file '{0}'", fileLocation), ex);
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
                foreach (DailyPercentagesProduct dpp in _cspl.Product)
                {
                    foreach (DailyPercentagesProductStore dpps in dpp.Store)
                    {
                        foreach (DailyPercentagesProductStoreFiscalPeriod fp in dpps.FiscalPeriod)
                        {
                            try
                            {
                                line = Convert.ToString(dpp.ID) + delimiter[0] + Convert.ToString(dpps.ID) + delimiter[0] + Convert.ToString(fp.BeginDate) + delimiter[0] + Convert.ToString(fp.EndDate) + delimiter[0] +
                                    Convert.ToDecimal(fp.Day1).ToString() + delimiter[0] + Convert.ToDecimal(fp.Day2).ToString() + delimiter[0] + Convert.ToDecimal(fp.Day3).ToString() + delimiter[0] +
                                    Convert.ToDecimal(fp.Day4).ToString() + delimiter[0] + Convert.ToDecimal(fp.Day5).ToString() + delimiter[0] + Convert.ToDecimal(fp.Day6).ToString() + delimiter[0] +
                                    Convert.ToDecimal(fp.Day7).ToString();

                                lines.Add(line);
                            }
                            catch (Exception ex)
                            {
                                throw new XMLDailyPercentagesProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
                            }
                        }
                    }

                }
                returnCode = ValidateFileData(lines, delimiter, ref errorFound);
                // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                //if (returnCode != eReturnCode.successful)
                //{
                //    for (int s = 0; s < lines.Count; s++)
                //    {
                //        if (s <= 1)
                //        {
                //            string[] fields2 = MIDstringTools.Split(lines[s], delimiter[0], true);
                //            yearWeek = Convert.ToInt32(fields2[0]);
                //            merchId = Convert.ToString(fields2[1]);
                //            storeAttribute = Convert.ToString(fields2[2]);
                //            HierarchyNodeProfile hnp = _hm.NodeLookup(ref em, merchId, false, false);
                //            //int deletecode = _dpcd.DailyPercentagesCriteria_Delete(hnp.Key, yearWeek);
                //        }
                //    }
                //    message = ("Error Inserting data for " + merchId + " - " + yearWeek + " - " + storeAttribute + " transaction has been canceled");
                //    _audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
                //}
                // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
            }
            catch (Exception ex)
            {
                throw new XMLDailyPercentagesProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
            }
            return returnCode;
        }

        public eReturnCode ValidateFileData(List<string> lines, char[] delimiter, ref bool errorFound)
        {
            //BEGIN TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
            // lots of changes under this TT please use the diff function to find them all
            int nodeRID;
            int beginYYYYWW = 0;
            int endYYYYWW = 0;
            // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
            //int WeekKey = 0;
            // End TT#2621 - JSmith - Duplicate weeks in daily sales
            string nodeId = null;
            string newNode = null;
            string storeID = null;
            string newStore = null;
            WeekProfile beginWeek = null;
            WeekProfile endWeek = null;
            WeekProfile newBeginWeek = null; //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
            WeekProfile newEndWeek = null; //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
            bool EOF = false;
            bool defaultPct = false; //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
            string message = null;
            decimal pct = 0;
            eReturnCode returnCode = eReturnCode.successful;
            List<string> lineData = new List<string>();
            // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
            //DateRangeProfile drp = null;
            //Hashtable actionHashTable = new Hashtable();
            ArrayList alAction = new ArrayList();
            // End TT#2621 - JSmith - Duplicate weeks in daily sales
            // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
            object objRID = null;
            // End TT#2657 - JSmith - Severe error during DailySalesPercentages load

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

                        // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                        message = "Product: " + ((fields.Length > 0) ? fields[0] : " ") + "; Store: " + ((fields.Length > 1) ? fields[1] : " ")
                            + "; Begin Date: " + ((fields.Length > 2) ? fields[2] : " ") + "; End Date: " + ((fields.Length > 3) ? fields[3] : " ")
                            + "; Day1: " + ((fields.Length > 4) ? fields[4] : " ") + "; Day2: " + ((fields.Length > 5) ? fields[5] : " ")
                            + "; Day3: " + ((fields.Length > 6) ? fields[6] : " ") + "; Day4: " + ((fields.Length > 7) ? fields[7] : " ")
                            + "; Day5: " + ((fields.Length > 8) ? fields[8] : " ") + "; Day6: " + ((fields.Length > 9) ? fields[9] : " ")
                            + "; Day7: " + ((fields.Length > 10) ? fields[10] : " ");

                        EditMsgs em = new EditMsgs();
                        // End TT#2657 - JSmith - Severe error during DailySalesPercentages load

                        if (fields.Length == 11)
                        {
                            try
                            {
                                beginYYYYWW = Convert.ToInt32(fields[2]);
                            }
                            catch
                            {
                                try
                                {
                                    string sStartDay = Convert.ToString(fields[2]);
                                    DateTime dt = Convert.ToDateTime(sStartDay);
                                    //DayProfile startDay = _SAB.ApplicationServerSession.Calendar.GetFiscalDay(Convert.ToInt32(dt));
                                    WeekProfile startWeek = _SAB.ApplicationServerSession.Calendar.GetWeek(dt);
                                    beginYYYYWW = startWeek.YearWeek;
                                    // Begin TT#378-MD - JSmith - Daily Percentages fails when calendar dates are used
                                    lines[s] = lines[s].Replace(fields[2], beginYYYYWW.ToString());
                                    // End TT#378-MD - JSmith - Daily Percentages fails when calendar dates are used
                                }
                                catch
                                {
                                    // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    //message = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate);
                                    //message = message.Replace("{0}", fields[0]);
                                    //message = message.Replace("{1}", fields[1]);
                                    //message = message.Replace("{2}", fields[2]);
                                    //message = message.Replace("{3}", fields[3]);
                                    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                    // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                            }

                            try
                            {
                                endYYYYWW = Convert.ToInt32(fields[3]);
                            }
                            catch
                            {
                                try
                                {
                                    DateTime dt = Convert.ToDateTime(Convert.ToString(fields[3]));
                                    WeekProfile endingWeek = _SAB.ApplicationServerSession.Calendar.GetWeek(dt);
                                    endYYYYWW = endingWeek.YearWeek;
                                    // Begin TT#378-MD - JSmith - Daily Percentages fails when calendar dates are used
                                    lines[s] = lines[s].Replace(fields[3], endYYYYWW.ToString());
                                    // End TT#378-MD - JSmith - Daily Percentages fails when calendar dates are used
                                }
                                catch
                                {
                                    // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    //message = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate);
                                    //message = message.Replace("{0}", fields[0]);
                                    //message = message.Replace("{1}", fields[1]);
                                    //message = message.Replace("{2}", fields[2]);
                                    //message = message.Replace("{3}", fields[3]);
                                    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                    // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                            }

                            if (beginYYYYWW != 0 && endYYYYWW != 0)
                            {
                                try
                                {
                                    beginWeek = _SAB.ClientServerSession.Calendar.GetFiscalWeek(beginYYYYWW);

                                }
                                catch (Exception)
                                {
                                    // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    //message = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate);
                                    //message = message.Replace("{0}", fields[0]);
                                    //message = message.Replace("{1}", fields[1]);
                                    //message = message.Replace("{2}", fields[2]);
                                    //message = message.Replace("{3}", fields[3]);
                                    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                    // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                try
                                {
                                    endWeek = _SAB.ClientServerSession.Calendar.GetFiscalWeek(endYYYYWW);
                                }
                                catch (Exception)
                                {
                                    // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    //message = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDate);
                                    //message = message.Replace("{0}", fields[0]);
                                    //message = message.Replace("{1}", fields[1]);
                                    //message = message.Replace("{2}", fields[2]);
                                    //message = message.Replace("{3}", fields[3]);
                                    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                    // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                    returnCode = eReturnCode.editErrors;
                                    ++_recordsWithErrors;
                                }
                            }
							//BEGIN TT#3931 - DOConnell - Daily Percentages - Object Reference Error
                            else if ((beginYYYYWW != 0 && endYYYYWW == 0) || (beginYYYYWW == 0 && endYYYYWW != 0))
                            {
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                                returnCode = eReturnCode.editErrors;
                                ++_recordsWithErrors;
                            }

                            //try
                            //{
                            //    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                            //    //if (returnCode == eReturnCode.successful)
                            //    //{
                            //    //    if (beginYYYYWW == 0 && endYYYYWW == 0)
                            //    //    {
                            //    //        drp = new DateRangeProfile(Include.NoRID);
                            //    //        drp.Key = -1;
                            //    //    }
                            //    //    else
                            //    //    {
                            //    //        drp = _SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(beginYYYYWW, endYYYYWW);
                            //    //    }
                            //    //}
                            //    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                            //}
                            //catch (Exception)
                            //{
                            //    // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                            //    //message = MIDText.GetTextOnly(eMIDTextCode.msg_NoRelativeDateRangeProfile);
                            //    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                            //    //returnCode = eReturnCode.severe;
                            //    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidDate, message, sourceModule);
                            //    returnCode = eReturnCode.editErrors;
                            //    // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                            //    ++_recordsWithErrors;
                            //}
							//END TT#3931 - DOConnell - Daily Percentages - Object Reference Error
							
                            // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                            bool percentagesValid = true;
                            decimal percent;
                            for (int i = 4; i < 11; i++)
                            {
                                if (!decimal.TryParse(Convert.ToString(fields[i]), out percent)) 
                                {
                                    percentagesValid = false;
                                    break;
                                }
                            }
                            if (!percentagesValid)
                            {
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DailyPctInvalidPercentage, message, sourceModule);
                                returnCode = eReturnCode.editErrors;
                                ++_recordsWithErrors;
                            }
                            // End TT#2657 - JSmith - Severe error during DailySalesPercentages load

                            nodeId = Convert.ToString(fields[0]);    
                            try
                            {
                                // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                //nodeRID = _SAB.HierarchyServerSession.GetNodeRID(nodeId);
                                objRID = _nodes[nodeId];
                                if (objRID != null)
                                {
                                    nodeRID = Convert.ToInt32(objRID);
                                }
                                else
                                {
                                    HierarchyNodeProfile hnp = _hm.NodeLookup(ref em, nodeId, false, false);
                                    nodeRID = hnp.Key;
                                }
                                // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                if (nodeRID == Include.NoRID)
                                {
                                    //message = MIDText.GetTextOnly(eMIDTextCode.msg_DailyPctInvalidNode);
                                    //message = message.Replace("{0}", fields[0]);
                                    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
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

                            storeID = Convert.ToString(fields[1]);
                            try
                            {
                                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                //ProfileList storeList = _SAB.StoreServerSession.GetAllStoresList();

                                //int stRID = _SAB.StoreServerSession.GetStoreRID(storeID);
                                //StoreProfile stProf = _SAB.StoreServerSession.GetStoreProfile(stRID);
                                StoreProfile stProf = null;
                                int stRID = Include.NoRID;
                                object storeRID = _stores[storeID];
                                if (storeRID != null)
                                {
                                    stRID = Convert.ToInt32(storeRID);
                                }
                                // End TT#2621 - JSmith - Duplicate weeks in daily sales

                                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                //if (!storeList.Contains(stProf.Key))
                                // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                //if (stRID == Include.NoRID)
                                if (stRID == Include.NoRID ||
                                    !_storeList.Contains(stRID))
                                // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                {
                                    //message = MIDText.GetTextOnly(eMIDTextCode.msg_StoreNotFound);
                                    //message = message.Replace("{0}", fields[1]);
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                else
                                {
                                    stProf = (StoreProfile)_storeList.FindKey(stRID);
                                }
                                // Else TT#2621 - JSmith - Duplicate weeks in daily sales
                            }
                            catch (Exception)
                            {
                                // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                // message = MIDText.GetTextOnly(eMIDTextCode.msg_StoreNotActive);
                                //message = message.Replace("{0}", fields[1]);
                                //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InputInvalid, message, sourceModule);
                                //returnCode = eReturnCode.severe;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, message, sourceModule);
                                returnCode = eReturnCode.editErrors;
                                // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
                                ++_recordsWithErrors;
                            }

                            ++_recordsRead;
                            if (returnCode == eReturnCode.successful)
                            {
                                if ((newNode == null | nodeId == newNode) && (newStore == null | storeID == newStore))
                                {
                                    newNode = nodeId;
                                    newStore = storeID;
									//BEGIN TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    if (!defaultPct)
                                    {
                                        newBeginWeek = beginWeek;
                                        newEndWeek = endWeek;
                                    }
									//END TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    lineData.Add(lines[s]);
                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //if (drp.Key == Include.NoRID)
                                    //{
                                    //    WeekKey = 0;
                                    //}
                                    //else
                                    //{
                                    //    WeekKey = drp.Key;
                                    //}
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //actionHashTable.Add(WeekKey, lines[s]);
                                    alAction.Add(lines[s]);
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
									//BEGIN TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //if (drp.Key == Include.NoRID)
                                    if (beginYYYYWW == 0 && endYYYYWW == 0)
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    {
                                        defaultPct = true;
                                    }
									//BEGIN TT#3931 - DOConnell - Daily Percentages - Object Reference Error
                                    else
                                    {
                                        newBeginWeek = beginWeek;
                                        newEndWeek = endWeek;
                                    }
									//END TT#3931 - DOConnell - Daily Percentages - Object Reference Error
                                }
                                else
                                {
                                    if (!defaultPct)
                                    {
                                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                        //returnCode = SeparateDelimitedData(actionHashTable, delimiter, newBeginWeek.YearWeek, newEndWeek.YearWeek, true); //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                        returnCode = SeparateDelimitedData(alAction, delimiter, newBeginWeek.YearWeek, newEndWeek.YearWeek, true); //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    }
                                    else
                                    {
                                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                        //returnCode = SeparateDelimitedData(actionHashTable, delimiter, 0, 0, true);
                                        returnCode = SeparateDelimitedData(alAction, delimiter, 0, 0, true);
                                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    }

                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //if (drp.Key == Include.NoRID)
                                    if (beginYYYYWW == 0 && endYYYYWW == 0)
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    {
                                        defaultPct = true;
                                    }
									//BEGIN TT#3931 - DOConnell - Daily Percentages - Object Reference Error
                                    //else
                                    //{
                                    //    defaultPct = false;
									//	//BEGIN TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    //    newBeginWeek = beginWeek;
                                    //    newEndWeek = endWeek;
									//	//END TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    //}
									//END TT#3931 - DOConnell - Daily Percentages - Object Reference Error
									//END TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    lineData.Clear();
                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //actionHashTable.Clear();
                                    alAction = new ArrayList();
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    lineData.Add(lines[s]);
                                    newNode = nodeId;
                                    newStore = storeID;

                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //if (drp.Key == Include.NoRID)
                                    //{
                                    //    WeekKey = 0;
                                    //}
                                    //else
                                    //{
                                    //    WeekKey = drp.Key;
                                    //}
                                    //actionHashTable.Add(WeekKey, lines[s]);
                                    alAction.Add(lines[s]);
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                }
                                if (EOF)
                                {
									//BEGIN TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    if (!defaultPct)
                                    {
                                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                        //returnCode = SeparateDelimitedData(actionHashTable, delimiter, newBeginWeek.YearWeek, newEndWeek.YearWeek, true);
                                        returnCode = SeparateDelimitedData(alAction, delimiter, newBeginWeek.YearWeek, newEndWeek.YearWeek, true);
                                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    }
                                    else
                                    {
                                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                        //returnCode = SeparateDelimitedData(actionHashTable, delimiter, 0, 0, true);
                                        returnCode = SeparateDelimitedData(alAction, delimiter, 0, 0, true);
                                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    }
                                    //returnCode = SeparateDelimitedData(actionHashTable, delimiter, beginYYYYWW, endYYYYWW, true);
									//END TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    lineData.Clear();
                                    lineData.Add(lines[s]);
                                    newNode = nodeId;
                                    newStore = storeID;
                                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                    //actionHashTable.Clear();
                                    alAction = new ArrayList();
                                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                }
                            }
                            else 
                            {
                                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                //if (EOF && actionHashTable.Count > 0)
                                if (EOF && alAction.Count > 0)
                                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                {
									//BEGIN TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                    if (!defaultPct)
                                    {
                                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                        //returnCode = SeparateDelimitedData(actionHashTable, delimiter, newBeginWeek.YearWeek, newEndWeek.YearWeek, true);
                                        returnCode = SeparateDelimitedData(alAction, delimiter, newBeginWeek.YearWeek, newEndWeek.YearWeek, true);
                                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    }
                                    else
                                    {
                                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                                        //returnCode = SeparateDelimitedData(actionHashTable, delimiter, 0, 0, true);
                                        returnCode = SeparateDelimitedData(alAction, delimiter, 0, 0, true);
                                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                                    }
                                    //returnCode = SeparateDelimitedData(actionHashTable, delimiter, beginYYYYWW, endYYYYWW, true);
									//END TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                }
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
                _audit.DailyPercentagesCriteriaLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _recordsAddedUpdated);
                _dpcd.CloseUpdateConnection();
            }
            return returnCode;
        }

        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
        //private eReturnCode SeparateDelimitedData(Hashtable actionHashTable, char[] delimiter, int beginYYYYWW, int endYYYYWW, bool delete)
        private eReturnCode SeparateDelimitedData(ArrayList alAction, char[] delimiter, int beginYYYYWW, int endYYYYWW, bool delete)
        // End TT#2621 - JSmith - Duplicate weeks in daily sales
        {
            //BEGIN TT#280 - MD - DOConnell - Daily Percentages error messages are incorrect
            // lots of changes under this TT please use the diff function to find them all
            eReturnCode returnCode = eReturnCode.successful;
            int nodeRID = 0;
            int storeRID = 0;
            decimal Day1 = 0;
            decimal Day2 = 0;
            decimal Day3 = 0;
            decimal Day4 = 0;
            decimal Day5 = 0;
            decimal Day6 = 0;
            decimal Day7 = 0;
            decimal percent;
            decimal totPct = 0;
            string returnMessage;
            string nodeId;
            string storeID;
            // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
            //HierarchyMaintenance _hm = new HierarchyMaintenance(_SAB);
            // End TT#2657 - JSmith - Severe error during DailySalesPercentages load
            EditMsgs em = new EditMsgs();
            SortedList changeList = new SortedList();
            StoreProfile StProf = null;
            StoreDailyPercentagesProfile sdpp = null;
            ProfileList datelist = new ProfileList(eProfileType.DateRange);
            // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
            //string procString = null;
            // End TT#2621 - JSmith - Duplicate weeks in daily sales
            ArrayList inValues = new ArrayList();
            ArrayList outValues;
            BasicSpread spreader = new BasicSpread();
            DateRangeProfile drp;
            // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
            object objRID = null;
            // End TT#2621 - JSmith - Duplicate weeks in daily sales

            try
            {
                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                //_storeList = _SAB.StoreServerSession.GetAllStoresList();
                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                //if (actionHashTable != null && actionHashTable.Count > 0)
                if (alAction != null && alAction.Count > 0)
                // End TT#2621 - JSmith - Duplicate weeks in daily sales
                {
                    // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                    //foreach (DictionaryEntry de in actionHashTable)
                    foreach (string procString in alAction)
                    // End TT#2621 - JSmith - Duplicate weeks in daily sales
                    {
                        returnCode = eReturnCode.successful;
                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                        //procString = Convert.ToString(actionHashTable[de.Key]);
                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                        string[] fields = MIDstringTools.Split(procString, delimiter[0], true);

                        nodeId = Convert.ToString(fields[0]);
                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                        //nodeRID = _SAB.HierarchyServerSession.GetNodeRID(nodeId);
                        objRID = _nodes[nodeId];
                        if (objRID != null)
                        {
                            nodeRID = Convert.ToInt32(objRID);
                        }
                        else
                        {
                            nodeRID = _SAB.HierarchyServerSession.GetNodeRID(nodeId);
                        }
                        // End TT#2621 - JSmith - Duplicate weeks in daily sales
                        storeID = Convert.ToString(fields[1]);
                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                        //storeRID = _SAB.StoreServerSession.GetStoreRID(storeID);
                        //StProf = _SAB.StoreServerSession.GetStoreProfile(storeRID);
                        objRID = _stores[storeID];
                        if (objRID != null)
                        {
                            storeRID = Convert.ToInt32(objRID);
                        }
                        else
                        {
                            // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                            //storeRID = StoreMgmt.StoreProfile_GetStoreRidFromId(storeID); //_SAB.StoreServerSession.GetStoreRID(storeID);
                            StoreProfile sp = (StoreProfile)_stores[storeID];
                            storeRID = sp.Key;
                            // End TT#1902-MD - JSmith - Store Services - VSW API Error
                        }
                        StProf = (StoreProfile)_storeList.FindKey(storeRID);

                        beginYYYYWW = Convert.ToInt32(fields[2]);
                        endYYYYWW = Convert.ToInt32(fields[3]);
                        // End TT#2621 - JSmith - Duplicate weeks in daily sales

                        if (beginYYYYWW == 0 && endYYYYWW == 0)
                        {
                            drp = new DateRangeProfile(Include.NoRID);
                            drp.Key = -1;
                        }
                        else
                        {
                            drp = _SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(beginYYYYWW, endYYYYWW);
                        }

                        for (int i = 4; i < 11; i++)
                        {
                            if (decimal.TryParse(Convert.ToString(fields[i]), out percent)) { }
                            totPct = totPct + percent;
                            switch (i)
                            {
                                case 4:
                                    Day1 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                                case 5:
                                    Day2 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                                case 6:
                                    Day3 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                                case 7:
                                    Day4 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                                case 8:
                                    Day5 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                                case 9:
                                    Day6 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                                case 10:
                                    Day7 = percent;
                                    if (Convert.IsDBNull(percent))	// cell is empty
                                    {
                                        inValues.Add(0);
                                    }
                                    else
                                    {
                                        inValues.Add(Convert.ToDouble(percent, CultureInfo.CurrentUICulture));
                                    }
                                    break;
                            }
                        }

                        if (totPct != 100 && totPct > 0)
                        {
                            spreader.ExecuteSimpleSpread(100, inValues, 3, out outValues);

                            _recordsWithErrors = _recordsWithErrors + changeList.Count;

                            if (drp.Key != Include.NoRID)
                            {
                                returnMessage = MIDText.GetTextOnly(eMIDTextCode.msg_DailyPctMustEq100);
                                returnMessage = returnMessage.Replace("{0}", Convert.ToString(nodeId));
                                returnMessage = returnMessage.Replace("{1}", Convert.ToString(StProf.StoreId));
                                returnMessage = returnMessage.Replace("{2}", Convert.ToString(beginYYYYWW));
                                returnMessage = returnMessage.Replace("{3}", Convert.ToString(endYYYYWW));
                                returnMessage = returnMessage.Replace("{4}", Convert.ToString(totPct));
                            }
                            else
                            {
                                returnMessage = MIDText.GetTextOnly(eMIDTextCode.msg_DailyPctDefaultMustEq100);
                                returnMessage = returnMessage.Replace("{0}", Convert.ToString(nodeId));
                                returnMessage = returnMessage.Replace("{1}", Convert.ToString(StProf.StoreId));
                                returnMessage = returnMessage.Replace("{2}", Convert.ToString(totPct));
                            }
                            _audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_TotAdjTo100, returnMessage, sourceModule);
                            _audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.msg_TotAdjTo100, "", eMIDMessageLevel.Warning);
                            //returnCode = eReturnCode.severe;

                            for (int v = 0; v < outValues.Count; v++)
                            {
                                switch (v)
                                {
                                    case 0:
                                        Day1 = Convert.ToDecimal(outValues[v]);
                                        break;
                                    case 1:
                                        Day2 = Convert.ToDecimal(outValues[v]);
                                        break;
                                    case 2:
                                        Day3 = Convert.ToDecimal(outValues[v]);
                                        break;
                                    case 3:
                                        Day4 = Convert.ToDecimal(outValues[v]);
                                        break;
                                    case 4:
                                        Day5 = Convert.ToDecimal(outValues[v]);
                                        break;
                                    case 5:
                                        Day6 = Convert.ToDecimal(outValues[v]);
                                        break;
                                    case 6:
                                        Day7 = Convert.ToDecimal(outValues[v]);
                                        break;
                                }
                            }
                            inValues.Clear();
                            totPct = 0;
                        }
                        else
                        {
                            inValues.Clear();
                        }

                        //else
                        //{
                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                            //if (beginYYYYWW == 0 || endYYYYWW == 0)
                            //{

                            //    drp = new DateRangeProfile(Include.NoRID);
                            //    drp.Key = -1;
                            //}
                            //else
                            //{
                            //    drp = _SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(beginYYYYWW, endYYYYWW);
                            //}
                        // End TT#2621 - JSmith - Duplicate weeks in daily sales

                        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                            //StProf = _SAB.StoreServerSession.GetStoreProfile(storeID);
                            objRID = _stores[storeID];
                            if (objRID != null)
                            {
                                int stRID = Convert.ToInt32(storeRID);
                                StProf = (StoreProfile)_storeList.FindKey(stRID);
                            }
                            else
                            {
                                // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                                //StProf = StoreMgmt.StoreProfile_Get(storeID); //_SAB.StoreServerSession.GetStoreProfile(storeID);
                                StProf = (StoreProfile)_stores[storeID];
                                // End TT#1902-MD - JSmith - Store Services - VSW API Error
                            }
                            // End TT#2621 - JSmith - Duplicate weeks in daily sales

                            int changeKey = drp.Key;
                            DailyPercentagesValues changeValueList = new DailyPercentagesValues(nodeRID, StProf.Key, drp, Day1, Day2, Day3, Day4, Day5, Day6, Day7);
                            if (!changeList.Contains(changeKey))
                                changeList.Add(changeKey, changeValueList);
                            totPct = 0;

                        //}
                    }
                }

                if (changeList.Count > 0)
                {
                    _storeDailyPercentagesList = _SAB.HierarchyServerSession.GetStoreDailyPercentagesList(_storeList, nodeRID);

                    int currDate = _SAB.ApplicationServerSession.Calendar.CurrentDate.Key;

                    foreach (int clKey in changeList.Keys)
                    {
                        DailyPercentagesValues dpv = (DailyPercentagesValues)changeList[clKey];

                        if (_storeDailyPercentagesList.Contains(dpv.ST_RID))
                        {
                            sdpp = (StoreDailyPercentagesProfile)_storeDailyPercentagesList.FindKey(dpv.ST_RID);
                        }
                        else
                        {
                            sdpp = new StoreDailyPercentagesProfile(dpv.ST_RID);
                            sdpp.DailyPercentagesList = new DailyPercentagesList(eProfileType.StoreDailyPercentages);
                            _storeDailyPercentagesList.Add(sdpp);
                        }


                        if (dpv.dR_PROF.Key == Include.NoRID)
                        {
                            if (!sdpp.HasDefaultValues)
                            {
                                sdpp.StoreDailyPercentagesDefaultChangeType = eChangeType.add;
                            }
                            else
                            {
                                sdpp.StoreDailyPercentagesDefaultChangeType = eChangeType.update;
                            }
                            sdpp.StoreDailyPercentagesIsInherited = false;
                            sdpp.HasDefaultValues = true;
                            sdpp.Day1Default = Convert.ToDouble(dpv.DAY1);
                            sdpp.Day2Default = Convert.ToDouble(dpv.DAY2);
                            sdpp.Day3Default = Convert.ToDouble(dpv.DAY3);
                            sdpp.Day4Default = Convert.ToDouble(dpv.DAY4);
                            sdpp.Day5Default = Convert.ToDouble(dpv.DAY5);
                            sdpp.Day6Default = Convert.ToDouble(dpv.DAY6);
                            sdpp.Day7Default = Convert.ToDouble(dpv.DAY7);
                        }
                        else
                        {
                            DailyPercentagesProfile dpp = null;
                            foreach (DailyPercentagesProfile datelist_dpp in sdpp.DailyPercentagesList)
                            {
                                if (datelist_dpp.DateRange.StartDateKey == dpv.dR_PROF.StartDateKey && datelist_dpp.DateRange.EndDateKey == dpv.dR_PROF.EndDateKey)
                                {
                                    dpp = datelist_dpp;
                                    break;
                                }
                            }

                            if (dpp == null)
                            {
                                dpp = new DailyPercentagesProfile(dpv.dR_PROF.Key);
                                dpp.DateRange = dpv.dR_PROF; //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                                dpp.DailyPercentagesChangeType = eChangeType.add;
                            }
                            else
                            {
                                dpp.DailyPercentagesChangeType = eChangeType.update;
                            }

                            //dpp.DateRange = dpv.dR_PROF; //TT#288 - MD - DOConnell - Daily %'s not loaded correctly
                            dpp.Day1 = Convert.ToDouble(dpv.DAY1);
                            dpp.Day2 = Convert.ToDouble(dpv.DAY2);
                            dpp.Day3 = Convert.ToDouble(dpv.DAY3);
                            dpp.Day4 = Convert.ToDouble(dpv.DAY4);
                            dpp.Day5 = Convert.ToDouble(dpv.DAY5);
                            dpp.Day6 = Convert.ToDouble(dpv.DAY6);
                            dpp.Day7 = Convert.ToDouble(dpv.DAY7);

                            if (dpp.DailyPercentagesChangeType == eChangeType.add)
                            {
                                sdpp.DailyPercentagesList.Add(dpp);
                            }
                            else
                            {
                                sdpp.DailyPercentagesList.Update(dpp);
                            }
                        }
					}

                }

                _SAB.HierarchyServerSession.StoreDailyPercentagesUpdate(nodeRID, _storeDailyPercentagesList, false);
                // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
                //_recordsAddedUpdated = _recordsAddedUpdated + actionHashTable.Count;
                _recordsAddedUpdated = _recordsAddedUpdated + alAction.Count;
                // End TT#2621 - JSmith - Duplicate weeks in daily sales
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