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

namespace MIDRetail.VSWCriteriaLoad
{
    public class VSWCriteriaLoadProcess
    {
        private string sourceModule = "VSWLoadCriteriaLoadProcess.cs";
        private IMOProfileList _VSWLoadList;
        // Begin TT#2621 - JSmith - Duplicate weeks in daily sales
        protected Hashtable _stores = null;
        ProfileList _storeList;
        protected Hashtable _nodes = null;
        // End TT#2621 - JSmith - Duplicate weeks in daily sales
        private HierarchyNodeProfile _hnp;
        private ProfileList _DPweeks;
        private VSW vsw = null;
        SessionAddressBlock _SAB;
        //protected VSWLoadCriteriaData _dpcd = null;
        protected Audit _audit = null;
        protected int _recordsRead = 0;
        protected int _recordsWithErrors = 0;
        protected int _recordsAddedUpdated = 0;
        // Begin TT#2657 - JSmith - Severe error during DailySalesPercentages load
        HierarchyMaintenance _hm;
        // End TT#2657 - JSmith - Severe error during DailySalesPercentages load

        public class VSWLoadValues
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


            public VSWLoadValues(int HNRID, int STRID, DateRangeProfile DRPROF, decimal D1, decimal D2, decimal D3, decimal D4, decimal D5, decimal D6, decimal D7)
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

        public class XMLVSWLoadProcessException : Exception
        {
            /// <summary>
            /// Used when throwing exceptions in the XML ColorCode Load Class
            /// </summary>
            /// <param name="message">The error message to display</param>
            public XMLVSWLoadProcessException(string message)
                : base(message)
            {
            }
            public XMLVSWLoadProcessException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        public VSWCriteriaLoadProcess(SessionAddressBlock SAB, ref bool errorFound)
        {
            try
            {
                _SAB = SAB;
                _audit = _SAB.ClientServerSession.Audit;
                //_dpcd = new VSWLoadCriteriaData();
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
                throw new XMLVSWLoadProcessException(String.Format("Can not find the file located at '{0}'", fileLocation));
            }
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
            try
            {


                XmlSerializer s = new XmlSerializer(typeof(VSW));	// Create a Serializer

                r = new StreamReader(fileLocation);			// Load the Xml File

                vsw = (VSW)s.Deserialize(r);	// Deserialize the Xml File to a strongly typed object

            }
            catch (Exception ex)
            {
                throw new XMLVSWLoadProcessException(String.Format("Error encountered during deserialization of the file '{0}'", fileLocation), ex);
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            try
            {
                foreach (VSWMerchandise VSWMerch in vsw.Merchandise)
                {
                    foreach ( VSWMerchandiseStore VSWstore in VSWMerch.Store)
                    {
                        try
                        {

                        string productId = Convert.ToString(VSWMerch.ID);
                        string storeRID = Convert.ToString(VSWstore.ID);
                        string minShipQty = Convert.ToString(VSWstore.MinShipQty);
                        string pctThreshold = Convert.ToString(VSWstore.PctThreshold);
                        string itemMax = Convert.ToString(VSWstore.ItemMax);
                        string FWOSMaxValue = Convert.ToString(VSWstore.FWOSMaxValue);
                        string FWOSMaxModel = Convert.ToString(VSWstore.FWOSMaxModel);
                        string pushToBkStk = Convert.ToString(VSWstore.PushToBkStk);

                        line = productId + delimiter[0] + storeRID + delimiter[0] + minShipQty + delimiter[0] 
                                + pctThreshold + delimiter[0] + itemMax + delimiter[0] + FWOSMaxValue + delimiter[0] 
                                + FWOSMaxModel + delimiter[0] + pushToBkStk;
                        lines.Add(line);

                        }
                        catch (Exception ex)
                        {
                            throw new XMLVSWLoadProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
                        }
                    }
                }

                returnCode = ValidateFileData(lines, delimiter, ref errorFound);

            }
            catch (Exception ex)
            {
                throw new XMLVSWLoadProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
            }
            return returnCode;
        }

        public eReturnCode ValidateFileData(List<string> lines, char[] delimiter, ref bool errorFound)
        {

            string nodeId = null;
            int nodeRID = Include.NoRID;
            string oldNodeID = null;
            int oldNodeRID = Include.NoRID;
            string storeID = null;
            int storeRID = Include.NoRID;


            bool EOF = false; 
            string message = null;
            eReturnCode returnCode = eReturnCode.successful;
            List<string> lineData = new List<string>();
            object objRID = null;
            ArrayList ErrorList = new ArrayList();
            IMOProfileList _imoProfileList;
            IMOProfile IMOP;
            StoreProfile sp = null;
            string error = null;

            try
            {
                System.Data.DataSet _IMODataSet = MIDEnvironment.CreateDataSet("VSWLoadDataSet");
                IMODataSet IMOds = new IMODataSet();
                _IMODataSet = IMOds.Reservation_Define(_IMODataSet);
                _storeList = _SAB.StoreServerSession.GetAllStoresList();

                if (lines != null && lines.Count > 0)
                {
                    lines.Sort();
                    for (int s = 0; s < lines.Count; s++)
                    {
                        _recordsRead++;
                        string storeProf_IMO_ID = String.Empty;
                        string minShipQty = String.Empty;
                        string pctThreshold = String.Empty;
                        string itemMax = String.Empty;
                        string FWOSMaxValue = String.Empty;
                        string FWOSMaxModel = String.Empty;
                        string pushToBkStk = String.Empty;
                        //int i = 4;

                        if (s == lines.Count - 1) EOF = true;
                        string[] fields = MIDstringTools.Split(lines[s], delimiter[0], true);
                        if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
                        {
                            continue;
                        }
                        returnCode = eReturnCode.successful;
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
                            _imoProfileList = _SAB.HierarchyServerSession.GetNodeIMOList(_storeList, oldNodeRID);
                            ErrorList = _hm.ValidIMOData(oldNodeRID, _storeList, _IMODataSet, _imoProfileList, ErrorList);
                            for (int e = 0; e < ErrorList.Count; e++)
                            {
                                IMOErrors IMOe = (IMOErrors)ErrorList[e];
                                if (IMOe.Error == true)
                                {
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_InvalidArgument, IMOe.Message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    _imoProfileList = IMOe.ProfList;
                                }
                            }
                            _SAB.HierarchyServerSession.IMOUpdate(Convert.ToInt32(oldNodeID), _imoProfileList, false);
                            _IMODataSet.Tables["Stores"].Rows.Clear();
                            ErrorList.Clear();
                            oldNodeID = nodeId;
                            oldNodeRID = nodeRID;
                        }
                        else if (oldNodeID == null)
                        {
                            oldNodeID = nodeId;
                            oldNodeRID = nodeRID;
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
                                sp = _SAB.StoreServerSession.GetStoreProfile(storeID);
                                storeRID = sp.Key;
                                storeID = sp.Text;
                            }

                            if (storeRID == Include.NoRID)
                            {
                                error = MIDText.GetText(eMIDTextCode.msg_InvalidStoreField);
                                error.Replace("{0}", Convert.ToString(fields[1]));
                                //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, message, sourceModule);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, error, sourceModule);
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
                                storeProf_IMO_ID = sp.IMO_ID;
                            }
                        }
                        catch (Exception)
                        {
                            error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidStoreField);
                            error = error.Replace("{0}", fields[1]);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, error, sourceModule);
                            returnCode = eReturnCode.severe;
                            ++_recordsWithErrors;
                        }

                        minShipQty = Convert.ToString(fields[2]);
                        if (minShipQty != string.Empty)
                        {
                            try
                            {
                                Convert.ToInt32(minShipQty);
                            }
                            catch
                            {
                                error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                error = error.Replace("{0}", minShipQty);
                                message = error + " - " + message;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }
                        pctThreshold = Convert.ToString(fields[3]);
                        if (pctThreshold != string.Empty)
                        {
                            try
                            {
                                Convert.ToDouble(pctThreshold);
                            }
                            catch
                            {
                                error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                error = error.Replace("{0}", pctThreshold);
                                message = error + " - " + message;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }
                        itemMax = Convert.ToString(fields[4]);
                        if (itemMax != string.Empty)
                        {
                            try
                            {
                                Convert.ToInt32(itemMax);
                            }
                            catch
                            {
                                error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                error = error.Replace("{0}", itemMax);
                                message = error + " - " + message;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }
                        FWOSMaxValue = Convert.ToString(fields[5]);
                        if (FWOSMaxValue != string.Empty)
                        {
                            try
                            {
                                Convert.ToInt32(FWOSMaxValue);
                            }
                            catch
                            {
                                error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                error = error.Replace("{0}", FWOSMaxValue);
                                message = error + " - " + message;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }
                        FWOSMaxModel = Convert.ToString(fields[6]);
                        if (FWOSMaxModel != null)
                        {
                            FWOSMaxModelProfile FWOSMaxModProf = _SAB.HierarchyServerSession.GetFWOSMaxModelData(FWOSMaxModel);
                            if (FWOSMaxModProf.Key == Include.NoRID)
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
                                FWOSMaxModel = Convert.ToString(FWOSMaxModProf.Key);
                            }
                        }
                        pushToBkStk = Convert.ToString(fields[7]);
                        if (pushToBkStk != string.Empty)
                        {
                            try
                            {
                                Convert.ToInt32(pushToBkStk);
                            }
                            catch
                            {
                                error = MIDText.GetTextOnly(eMIDTextCode.msg_InvalidArgument);
                                error = error.Replace("{0}", pushToBkStk);
                                message = error + " - " + message;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                                returnCode = eReturnCode.severe;
                                ++_recordsWithErrors;
                            }
                        }
                        if (returnCode == eReturnCode.successful)
                        {

                            _IMODataSet.Tables["Stores"].Rows.Add(new Object[] {null, 
                                                                                false, 
                                                                                Include.NoRID, 
                                                                                true, 
                                                                                storeRID, 
                                                                                storeID, 
                                                                                (storeProf_IMO_ID ?? string.Empty),  
                                                                                ((minShipQty == null)) ? String.Empty : minShipQty,
                                                                                ((pctThreshold == null)) ? String.Empty : pctThreshold,    
                                                                                ((itemMax == null)) ? String.Empty : itemMax, 
                                                                                ((FWOSMaxValue == null)) ? String.Empty : FWOSMaxValue, 
                                                                                ((FWOSMaxModel == null)) ? String.Empty : FWOSMaxModel, 
                                                                                ((pushToBkStk == null)) ? String.Empty : pushToBkStk
                                                                                });

                        //_imoDataSet.Tables["Stores"].Rows.Add(new object[] {sglp.Name, 
                        //    imop.IMOIsInherited, 
                        //    imop.IMOInheritedFromNodeRID, 
                        //    false, 
                        //    imop.IMOStoreRID, 
                        //    storeProfile.Text,
                        //    (storeProfile.IMO_ID ?? String.Empty),
                        //    (((imop.IMOIsDefault == true) || (imop.IMOMinShipQty == 0)) ? String.Empty : imop.IMOMinShipQty.ToString()),
                        //    (((imop.IMOIsDefault == true) || (imop.IMOPackQty == .5)) ? String.Empty : (imop.IMOPackQty*100).ToString()),    
                        //    (((imop.IMOIsDefault == true) || (imop.IMOMaxValue == int.MaxValue)) ? String.Empty : imop.IMOMaxValue.ToString()), 
                        //    FWOSMax,
                        //    (((imop.IMOIsDefault == true) || (imop.IMOFWOS_MaxModelRID == int.MaxValue)) ? String.Empty : imop.IMOFWOS_MaxModelRID.ToString()),
                        //    (((imop.IMOIsDefault == true) || (imop.IMOPshToBackStock < 0)) ? String.Empty : imop.IMOPshToBackStock.ToString())
                            _recordsAddedUpdated++;
                        }
                        else
                        {
                            returnCode = eReturnCode.successful;
                        }

                        if (EOF)
                        {
                            _imoProfileList = _SAB.HierarchyServerSession.GetNodeIMOList(_storeList, nodeRID);
                            ErrorList = _hm.ValidIMOData(oldNodeRID, _storeList, _IMODataSet, _imoProfileList, ErrorList);

                            for (int i = 0; i < ErrorList.Count; i++)
                            {
                                IMOErrors IMOe = (IMOErrors)ErrorList[i];
                                if (IMOe.Error == true)
                                {
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_InvalidArgument, IMOe.Message, sourceModule);
                                    returnCode = eReturnCode.severe;
                                    ++_recordsWithErrors;
                                }
                                else
                                {
                                    _imoProfileList = IMOe.ProfList;
                                }
                            }

                            _SAB.HierarchyServerSession.IMOUpdate(nodeRID, _imoProfileList, false);
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
                _audit.VSWCriteriaLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _recordsAddedUpdated);
            }
            return returnCode;
        }
    }
}
