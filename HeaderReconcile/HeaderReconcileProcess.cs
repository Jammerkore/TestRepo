using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace HeaderReconcile
{
    public partial class HeaderReconcileProcess
    {
        #region Variables
        private SessionAddressBlock _SAB;
        private int _processId;
        private string _removeTransactionsFileName;
        private string _removeTransactionsTriggerSuffix;
        private string _triggerSuffix;
        private List<string> _headerKeysToMatchList;
        private List<string> _headerTypeList;
        private string _outputDirectory;
        private bool _allowDeletes;
        private Audit _audit;
        private XmlSerializer _xmlSerializer;
        private XmlTextWriter _xmlTextWriter = null;
        private StreamWriter _xmlStreamWriter = null;
		// Begin TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
        private Dictionary<int, string> _dictWorkflows = new Dictionary<int,string>();
        private WorkflowBaseData _workflowData = new WorkflowBaseData();
		// End TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating

        private List<int> _skippedList;
        private List<int> _rejectedList;
        private List<string> _processedHeaderList;
        private List<string> _notModifiedHeaderList; // TT#4839 - JSmith - Header Reconcile - Question why some headers were removed
        private HeadersHeader _transactionHeader;
        private Dictionary<string, int> _duplicateHeaderList;       // Dup header list for headers with IDs
        private Dictionary<HeaderDuplicateKey, int> _duplicateNoIdHeaderList;   // Dup header list for headers with no IDs
        private AllocationHeaderProfile _allocationHeaderProfile = null;
        private Header _headerData;
        private HeaderMatching _headerMatching;

        private int _numHeaderRecsRead;
        private int _numHeaderRecsWritten;
        private int _numAddUpdateRecsWritten;
        private int _numRemoveRecsWritten;
        private int _numResetRecsWritten;
        private int _numDuplicateRecsFound;
        private int _numSkippedRecs;        // Due to matching current header information on database or duplicate found

        private int _totalFilesProcessed;
        private int _totalFilesWritten;
        private int _totalHeaderRecsRead;
        private int _totalHeaderRecsWritten;
        private int _totalDuplicateRecsFound;
        private int _totalRejectedRecs;
        private int _totalSkippedRecs;
        private int _totalRemoveRecsWritten;
        private int _totalRemoveFilesWritten;



        #endregion Variables

        #region Properties
        public int TotalFilesProcessed
        {
            get
            {
                return _totalFilesProcessed;
            }
        }

        public int TotalFilesWritten
        {
            get
            {
                return _totalFilesWritten;
            }
        }


        public int TotalHeaderRecsRead
        {
            get
            {
                return _totalHeaderRecsRead;
            }
        }

        public int TotalHeaderRecsWritten
        {
            get
            {
                return _totalHeaderRecsWritten;
            }
        }

        public int TotalRejectedRecs
        {
            get
            {
                return _totalRejectedRecs;
            }
        }

        public int TotalSkippedRecs
        {
            get
            {
                return _totalSkippedRecs;
            }
        }


        public int TotalDuplicateRecsFound
        {
            get
            {
                return _totalDuplicateRecsFound;
            }
        }

        public int TotalRemoveFilesWritten
        {
            get
            {
                return _totalRemoveFilesWritten;
            }
        }

        public int TotalRemoveRecsWritten
        {
            get
            {
                return _totalRemoveRecsWritten;
            }
        }



        #endregion Properties

        #region Constructors
        public HeaderReconcileProcess(
			SessionAddressBlock aSAB,
			int aProcessId,
			string aTriggerSuffix,
            List<string> headerKeysToMatchList,
            List<string> headerTypeList,
			string aOutputDirectory,
            string removeTransactionsFileName,
            string aRemoveTransactionsTriggerSuffix
            )
		{
			_SAB = aSAB;
            _audit = _SAB.ClientServerSession.Audit;
			_processId = aProcessId;
			_triggerSuffix = aTriggerSuffix;
            _headerKeysToMatchList = headerKeysToMatchList;
            _headerTypeList = headerTypeList;
			_outputDirectory = aOutputDirectory;
            _processedHeaderList = new List<string>();
            _notModifiedHeaderList = new List<string>(); // TT#4839 - JSmith - Header Reconcile - Question why some headers were removed
            _removeTransactionsFileName = removeTransactionsFileName;
            _removeTransactionsTriggerSuffix = aRemoveTransactionsTriggerSuffix;
            _headerMatching = new HeaderMatching(_SAB);

            _totalFilesProcessed = 0;
            _totalHeaderRecsRead = 0;
            _totalHeaderRecsWritten = 0;
            _totalDuplicateRecsFound = 0;
            _totalSkippedRecs = 0;
            _totalRejectedRecs = 0;
            _totalRemoveFilesWritten = 0;
            _totalRemoveRecsWritten = 0;

		}

        #endregion Constructors

        #region Methods

        public eReturnCode ProcessTransactionFile(string aInputFile)
        {
            _totalFilesProcessed++;
            _numHeaderRecsRead = 0;
            _numHeaderRecsWritten = 0;
            _numAddUpdateRecsWritten = 0;
            _numRemoveRecsWritten = 0;
            _numDuplicateRecsFound = 0;
            _numSkippedRecs = 0;

            string msgText = null;
            Headers headers = null;
            TextReader xmlReader = null;
            XmlSerializer xmlSerial = null;
            eReturnCode rtnCode = eReturnCode.successful;
            bool errorFound = false;
            StreamWriter xmlWriter = null;
            _skippedList = new List<int>();
            _rejectedList = new List<int>();

            // ================
            // Begin processing
            // ================
            try
            {

                try
                {
                    xmlSerial = new XmlSerializer(typeof(Headers));
                    xmlReader = new StreamReader(aInputFile);
                    headers = (Headers)xmlSerial.Deserialize(xmlReader);
                }
                catch (Exception Ex)
                {
                    errorFound = true;
                    _audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                    rtnCode = eReturnCode.severe;
                }
                finally
                {
                    if (xmlReader != null)
                        xmlReader.Close();
                }


                if (!errorFound)
                {
                    if (headers.Header != null)
                    {
                        msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
                        msgText = msgText.Replace("{0}", "[" + aInputFile + "]");
                        msgText += " - Begin Processing" + System.Environment.NewLine;
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name, true);

                        // ===============================================
                        // Create an xml writer
                        // ===============================================
                        string aFileName = Path.GetFileName(aInputFile);
                        string aOutFile = _outputDirectory + @"\" + aFileName;
                        XmlSerializer _xmlWriter = new XmlSerializer(typeof(Headers));

                        //==========================
                        // Process each Header
                        //==========================
                        foreach (HeadersHeader aHeaderTran in headers.Header)
                        {
                            ++_numHeaderRecsRead;

                            //=====================================================================================================================
                            // Is this a duplicate header WITH a header ID? We only want to send on the last transaction we find for a header id.
                            //=====================================================================================================================
                            bool dupFound = false;
                            int dupHeaderCnt = 0;
                            if (aHeaderTran.HeaderId != null && aHeaderTran.HeaderId.Trim() != string.Empty && _duplicateHeaderList.Count > 0)
                            {
                                string headerId = aHeaderTran.HeaderId.Trim();
                                if (_duplicateHeaderList.ContainsKey(headerId))
                                {
                                    dupHeaderCnt = _duplicateHeaderList[headerId];
                                    if (dupHeaderCnt > 1)
                                    {
                                        dupFound = true;
                                        _numDuplicateRecsFound++;
                                        _duplicateHeaderList[headerId]--;
                                    }
                                    else
                                    {
                                        _duplicateHeaderList.Remove(headerId);
                                    }
                                }
                            }
                            //=====================================================================================================================
                            // Is this a duplicate header WITHOUT a header ID? We only want to send on the last transaction we find for a header id.
                            //=====================================================================================================================
                            dupFound = false;
                            dupHeaderCnt = 0;
                            if (string.IsNullOrEmpty(aHeaderTran.HeaderId) && _duplicateNoIdHeaderList.Count > 0)
                            {
                                HeaderDuplicateKey headerDupKey = null;
                                GetHeaderDuplicateKey(aHeaderTran, ref headerDupKey);
                                if (_duplicateNoIdHeaderList.ContainsKey(headerDupKey))
                                {
                                    dupHeaderCnt = _duplicateNoIdHeaderList[headerDupKey];
                                    if (dupHeaderCnt > 1)
                                    {
                                        dupFound = true;
                                        _numDuplicateRecsFound++;
                                        _duplicateNoIdHeaderList[headerDupKey]--;
                                    }
                                    else
                                    {
                                        _duplicateNoIdHeaderList.Remove(headerDupKey);
                                    }
                                }
                            }

                            if (dupFound)
                            {
                                _skippedList.Add(_numHeaderRecsRead - 1);
                                msgText = "Duplicate Header Found: " + aHeaderTran.HeaderId;
                                msgText += " (Transaction #" + _numHeaderRecsRead.ToString() + ")";
                                msgText += System.Environment.NewLine;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
                            }
                            else
                            {
                                msgText = "Header " + aHeaderTran.HeaderId;
                                msgText += " (Transaction #" + _numHeaderRecsRead.ToString() + ")";
                                msgText += " - Begin Processing" + System.Environment.NewLine;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msgText, GetType().Name);

                                rtnCode = ProcessTransactionForHeader(headers, aHeaderTran);

                                if (rtnCode != eReturnCode.successful)
                                {
                                    _rejectedList.Add(_numHeaderRecsRead - 1);
                                }
                                //else
                                //{
                                //    ++_numHeaderRecsWritten;
                                //}

                                msgText = "Header " + aHeaderTran.HeaderId;
                                msgText += " (Transaction #" + _numHeaderRecsRead.ToString() + ")";
                                msgText += " - End Processing" + System.Environment.NewLine;
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msgText, GetType().Name);
                            }
                        }

                        //==============================================================================================
                        // This removes the skipped header transactions from the transaction file that will be written.
                        //==============================================================================================
                        foreach (int idx in _skippedList)
                        {
                            headers.Header[idx] = null;
                            _numSkippedRecs++;
                        }

                        //=========================================
                        // Count records to be written
                        //=========================================
                        foreach (HeadersHeader aHeaderTran in headers.Header)
                        {
                            if (aHeaderTran != null)
                            {
                                _numHeaderRecsWritten++;
                            }
                        }

                        //======================================================================
                        // If there are no transactions left in the file, do not write the file
                        //======================================================================
                        if (_numHeaderRecsWritten > 0)
                        {
                            //==========================================================
                            // Create stream writer, write out XML file, close stream
                            //==========================================================
                            xmlWriter = new StreamWriter(aOutFile);
                            _xmlWriter.Serialize(xmlWriter, headers);
                            xmlWriter.Close();
                            _totalFilesWritten++;

                            //=====================
                            // Write trigger file
                            //=====================
                            if (_triggerSuffix != string.Empty)
                            {
                                StreamWriter trgFile = new StreamWriter(aOutFile + _triggerSuffix);
                                trgFile.Close();
                            }
                        }
                    }
                    else
                    {
                        msgText = "No Header transactions found in input file" + System.Environment.NewLine;
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
                    }
                }
            }

            catch (Exception Ex)
            {
                errorFound = true;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                rtnCode = eReturnCode.severe;
            }

            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }

                if (xmlWriter != null)
                {
                    xmlWriter.Close();
                }

                _totalHeaderRecsRead += _numHeaderRecsRead;
                _totalHeaderRecsWritten += _numHeaderRecsWritten;
                _totalDuplicateRecsFound += _numDuplicateRecsFound;
                _totalSkippedRecs += _numSkippedRecs;

                msgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
                msgText = msgText.Replace("{0}", "[" + aInputFile + "]");
                msgText += " - End Processing" + System.Environment.NewLine;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
            }

            return rtnCode;
        }

        public eReturnCode PreProcessTransactions(List<string> inFiles)
        {
            eReturnCode returnCode = eReturnCode.successful;
            string msgText = string.Empty;

            try
            {
                msgText = "Begin Pre-Processing." + System.Environment.NewLine;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                // Used in header matching
                _headerData = new Header();

                // Find duplicates
                _duplicateHeaderList = new Dictionary<string, int>();
                _duplicateNoIdHeaderList = new Dictionary<HeaderDuplicateKey, int>();
                foreach (string inFile in inFiles)
                {
                    if (ValidTransactionFile(inFile))
                    {
                        PreProcessTransactionFile(inFile);
                    }
                }

                // Remove non-duplicates from list. 
                // Note: value is greater than 1 for non-duplicates.
                var nonDuplciates = _duplicateHeaderList.Where(f => f.Value == 1).ToArray();
                foreach (var item in nonDuplciates)
                {
                    _duplicateHeaderList.Remove(item.Key);
                }

                // Count number of duplicates (does not include original)
                foreach (var item in _duplicateHeaderList)
                {
                    _totalDuplicateRecsFound += item.Value - 1;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                msgText = "End Pre-Processing." + System.Environment.NewLine;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
            }

            return returnCode;
        }

        public bool ValidTransactionFile(string transFile)
        {
            bool isValid = true;
            string msgText = string.Empty;
            try
            {
                string fileMsgText = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

                if (transFile.Substring(transFile.Length - 4).ToUpper() != ".XML")
                {
                    msgText = fileMsgText.Replace("{0}.", "[" + transFile + "] unknown file type" + System.Environment.NewLine);
                    msgText += "Header Reconcile Process NOT run.";
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, GetType().Name, true);
                }
                else
                {
                    if (!File.Exists(transFile))
                    {
                        msgText = fileMsgText.Replace("{0}.", "[" + transFile + "] does NOT exist" + System.Environment.NewLine);
                        msgText += "Header Reconcile Process NOT run.";
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, GetType().Name, true);
                    }
                    else
                    {
                        FileInfo hdrFileInfo = new FileInfo(transFile);

                        if (transFile.Length == 0)
                        {
                            msgText = fileMsgText.Replace("{0}.", "[" + transFile + "] is an empty file" + System.Environment.NewLine);
                            msgText += "Header Reconcile Process NOT run.";
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, msgText, GetType().Name, true);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return isValid;
        }

        public eReturnCode PreProcessTransactionFile(string aInputFile)
        {
            eReturnCode returnCode = eReturnCode.successful;
            bool errorFound = false;
            string msgText = null;
            Headers headers = null;
            TextReader xmlReader = null;
            XmlSerializer xmlSerial = null;
            eReturnCode rtnCode = eReturnCode.successful;
            HeaderDuplicateKey headerDupKey = null;

            try
            {
                try
                {
                    xmlSerial = new XmlSerializer(typeof(Headers));
                    xmlReader = new StreamReader(aInputFile);
                    headers = (Headers)xmlSerial.Deserialize(xmlReader);
                }
                catch (Exception Ex)
                {
                    errorFound = true;
                    _audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                    rtnCode = eReturnCode.severe;
                }
                finally
                {
                    if (xmlReader != null)
                        xmlReader.Close();
                }

                if (!errorFound)
                {
                    if (headers.Header != null)
                    {
                        //==========================
                        // Process each Header
                        //==========================
                        foreach (HeadersHeader aHeaderTran in headers.Header)
                        {
                            if (aHeaderTran.HeaderId == null || aHeaderTran.HeaderId.Trim() == string.Empty)
                            {
                                headerDupKey = null;
                                GetHeaderDuplicateKey(aHeaderTran, ref headerDupKey);
                                if (_duplicateNoIdHeaderList.ContainsKey(headerDupKey))
                                {
                                    _duplicateNoIdHeaderList[headerDupKey]++;
                                }
                                else
                                {
                                    _duplicateNoIdHeaderList.Add(headerDupKey, 1);
                                }
                            }
                            else
                            {
                                string headerId = aHeaderTran.HeaderId.Trim();
                                if (_duplicateHeaderList.ContainsKey(headerId))
                                {
                                    _duplicateHeaderList[headerId]++;
                                }
                                else
                                {
                                    _duplicateHeaderList.Add(headerId, 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        msgText = "No Header transactions found in input file" + System.Environment.NewLine;
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
                    }
                }

            }
            catch (Exception ex)
            {
                errorFound = true;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), GetType().Name);
                returnCode = eReturnCode.severe;
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
            }

            return returnCode;
        }

        private eReturnCode ProcessTransactionForHeader(Headers headers, HeadersHeader aHeader)
        {
            eReturnCode returnCode = eReturnCode.successful;
            string headerId = null;
            bool isModified = false;
            try
            {
                _transactionHeader = aHeader;

                headerId = _transactionHeader.HeaderId;

                if (!_transactionHeader.HeaderTypeSpecified
                    || (_transactionHeader.HeaderTypeSpecified && IsMatchingProcessHeaderTypes(_transactionHeader.HeaderType)))
                {
                    switch (_transactionHeader.HeaderAction)
                    {
                        case HeadersHeaderHeaderAction.Create:
                            if (!string.IsNullOrEmpty(headerId))
                            {
                                _processedHeaderList.Add(headerId);
                            }
                            _numAddUpdateRecsWritten++;
                            // Write out the transaction
                            break;

                        case HeadersHeaderHeaderAction.Modify:
                            if (string.IsNullOrEmpty(headerId))
                            {
                                if (ValidHeaderMatchingFields(_transactionHeader))
                                {
                                    // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
									//returnCode = _headerMatching.FindMatchingHeader(_transactionHeader, ref headerId, _headerKeysToMatchList, _numHeaderRecsRead);
									returnCode = _headerMatching.FindMatchingHeader(_SAB.ClientServerSession, _transactionHeader, ref headerId, _headerKeysToMatchList, _numHeaderRecsRead);
									// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
                                }

                                if (returnCode == eReturnCode.successful)
                                {
                                    // No header match was found
                                    if (string.IsNullOrEmpty(headerId))
                                    {
                                        _numAddUpdateRecsWritten++;
                                        // Write out the transaction
                                    }
                                    else // A header match was found, so now we know the headerId
                                    {
                                        _transactionHeader.HeaderId = headerId;
                                        _allocationHeaderProfile = _SAB.HeaderServerSession.GetHeaderData(_transactionHeader.HeaderId, true, true, true);

                                        isModified = IsTransactionModifyingHeader(ref returnCode);

                                        if (!isModified)
                                        {
                                            // adds this header's position to an array for removal later
                                            _skippedList.Add(_numHeaderRecsRead - 1);
                                            _notModifiedHeaderList.Add(_transactionHeader.HeaderId); // TT#4839 - JSmith - Header Reconcile - Question why some headers were removed
                                        }
                                        else
                                        {
                                            _numAddUpdateRecsWritten++;
                                            _processedHeaderList.Add(headerId);
                                        }
                                    }
                                }
                            }
                            else // transaction HAS headerId. Send it on.
                            {
                                _processedHeaderList.Add(headerId);
                                _numAddUpdateRecsWritten++;
                            }
                            break;

                        case HeadersHeaderHeaderAction.Remove:
                            if (!string.IsNullOrEmpty(headerId))
                            {
                                _processedHeaderList.Add(headerId);
                            }
                            _numRemoveRecsWritten++;
                            // Write out the transaction
                            break;

                        case HeadersHeaderHeaderAction.Reset:
                            if (!string.IsNullOrEmpty(headerId))
                            {
                                _processedHeaderList.Add(headerId);
                            }
                            _numResetRecsWritten++;
                            // Write out the transaction
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // Transaction skipped because it doesn't match the processing header types.
                    _skippedList.Add(_numHeaderRecsRead - 1);
                }
            }
            catch (Exception ex)
            {
                string msgText = "Processing Header: " + _transactionHeader.HeaderId;
                msgText += " (Transaction #" + _numHeaderRecsRead.ToString() + ")";
                msgText += System.Environment.NewLine;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText + ex.ToString(), GetType().Name);
                returnCode = eReturnCode.severe;
            }
            finally
            {

            }

            return returnCode;
        }

        private bool IsMatchingProcessHeaderTypes(HeadersHeaderHeaderType headerType)
        {
            // Begin TT#4839 - JSmith - Header Reconcile - Question why some headers were removed
            //bool isMatching = true;
            bool isMatching = false;
            // End TT#4839 - JSmith - Header Reconcile - Question why some headers were removed

            if (_headerTypeList.Contains("ALL"))
            {
                // "Yes" it matches.
                isMatching = true;  // TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
            }
            else if (headerType == null)
            {
                // "Yes"? it's missing.
            }
            else
            {
                switch (headerType)
                {
                    case HeadersHeaderHeaderType.Receipt:
                        {
                            if (_headerTypeList.Contains("RECEIPT"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.PO:
                        {
                            if (_headerTypeList.Contains("PO"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.ASN:
                        {
                            if (_headerTypeList.Contains("ASN"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.Dummy:
                        {
                            if (_headerTypeList.Contains("DUMMY"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.DropShip:
                        {
                            if (_headerTypeList.Contains("DROPSHIP"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.Reserve:
                        {
                            if (_headerTypeList.Contains("RESERVE"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.WorkupTotalBuy:
                        {
                            if (_headerTypeList.Contains("WORKUPTOTALBUY"))
                            {
                                isMatching = true;
                            }
                            break;
                        }
                    case HeadersHeaderHeaderType.VSW:
                        {
                            if (_headerTypeList.Contains("VSW"))
                            {
                                isMatching = true;
                            }
                            break;
                        }

                    default:
                        {
                            string msgText = "Processing Header: " + _transactionHeader.HeaderId;
                            msgText += " (Transaction #" + _numHeaderRecsRead.ToString() + ")";
                            msgText += System.Environment.NewLine;
                            msgText += "has an unknown Header Type of " + headerType.ToString();
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msgText, GetType().Name);
                            isMatching = false;
                            break;
                        }
                }

            }
            return isMatching;
        }

        /// <summary>
        /// Creates the "Remove" transactions for headers not processed.
        /// </summary>
        /// <returns></returns>
        public eReturnCode PostProcessing()
        {
            eReturnCode returnCode = eReturnCode.successful;
            string msgText = string.Empty;
            string headerId = string.Empty;
            List<string> removeHeaderList = new List<string>();

            try
            {
                msgText = "Begin Post-Processing." + System.Environment.NewLine;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                Header headerData = new Header();
                DataTable dt = GetHeaderTypeListForNonReleasedHeaders();
                DataTable headerNonReleasedData = headerData.GetNonReleasedHeadersForReconcile(dt);
                foreach (DataRow aRow in headerNonReleasedData.Rows)
                {
                    headerId = aRow["HDR_ID"].ToString();
                    // Begin TT#4839 - JSmith - Header Reconcile - Question why some headers were removed
                    //if (!_processedHeaderList.Contains(headerId))
                    if (!_processedHeaderList.Contains(headerId) &&
                        !_notModifiedHeaderList.Contains(headerId))
                    // End TT#4839 - JSmith - Header Reconcile - Question why some headers were removed
                    {
                        removeHeaderList.Add(headerId);
                    }
                }

                //=========================================
                // Write Remove header reconcile file
                //=========================================
                if (removeHeaderList.Count > 0)
                {
                    Headers headers = new Headers();
                    HeadersHeader headersHeader = new HeadersHeader();
                    headers.Header = new HeadersHeader[removeHeaderList.Count];
                    int h = 0;
                    foreach (string aHeaderId in removeHeaderList)
                    {
                        headersHeader = new HeadersHeader();
                        headersHeader.HeaderAction = HeadersHeaderHeaderAction.Remove;
                        headersHeader.HeaderActionSpecified = true;
                        headersHeader.HeaderId = aHeaderId;
                        headers.Header[h] = headersHeader;
                        h++;
                        _totalRemoveRecsWritten++;
                    }

                    WriteHeaderReconcileRemoveFile(headers);
                    _totalRemoveFilesWritten++;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                msgText = "End Post-Processing." + System.Environment.NewLine;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
            }

            return returnCode;
        }

        private DataTable GetHeaderTypeListForNonReleasedHeaders()
        {
            DataTable dtHeaderTypes = new DataTable();
            dtHeaderTypes.Columns.Add("DISPLAY_TYPE", typeof(int));

            if (_headerTypeList.Contains("ALL"))
            {
                // Load ALL applicable Header types to be processed into table
                // Begin TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
                // Remove Receipt and IMO from exclude list
                //DataTable dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Receipt), Convert.ToInt32(eHeaderType.IMO));
                DataTable dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue);
                // End TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
                // Remove types never wanted
                for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dRow = dtTypes.Rows[i];
                    if (Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Assortment)
                        || Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Placeholder)
                        || Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.Master)  // TT#1966-MD - JSmith - DC Fulfillment
                        || Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture) == Convert.ToInt32(eHeaderType.MultiHeader)
                        )
                    {
                        dtTypes.Rows.Remove(dRow);
                    }
                }
                // Load Header types to be processed into table
                foreach (DataRow headerTypeRow in dtTypes.Rows)
                {
                    int displayType = int.Parse(headerTypeRow["TEXT_CODE"].ToString());
                    DataRow dr = dtHeaderTypes.NewRow();
                    dr["DISPLAY_TYPE"] = displayType;
                    dtHeaderTypes.Rows.Add(dr);
                }
            }
            else
            {
                // Load Header types to be processed into table
                foreach (string headerType in _headerTypeList)
                {
                    eHeaderType aHeaderType = _headerMatching.GetHeaderType(headerType);
                    DataRow dr = dtHeaderTypes.NewRow();
                    dr["DISPLAY_TYPE"] = (int)aHeaderType;
                    dtHeaderTypes.Rows.Add(dr);
                }
            }

            return dtHeaderTypes;
        }
        private bool ValidHeaderMatchingFields(HeadersHeader aHeader)
        {
            bool isValid = true;

            return isValid;
        }

        public void WriteHeaderReconcileRemoveFile(Headers headers)
        {
            bool needToFileOpen;
            int attemptCount;
            string fileName = _removeTransactionsFileName;
            string newFileName = string.Empty;
            string filePath = string.Empty;
            try
            {
                fileName = MIDMath.ValidAndReplaceFileName(fileName);
                needToFileOpen = true;
                attemptCount = 0;
                while (needToFileOpen)
                {
                    try
                    {
                        ++attemptCount;
                        if (attemptCount == 1)
                        {
                            newFileName = fileName + ".xml";
                        }
                        else
                        {
                            newFileName = fileName + attemptCount.ToString() + ".xml";
                        }

                        filePath = _outputDirectory + @"\" + newFileName;

                        _xmlSerializer = new XmlSerializer(typeof(Headers));
                        _xmlStreamWriter = new StreamWriter(filePath);
                        _xmlSerializer.Serialize(_xmlStreamWriter, headers);
                        needToFileOpen = false;
                    }
                    catch
                    {
                        if (attemptCount == 10)
                        {
                            needToFileOpen = false;
                            throw;
                        }
                    }
                    finally
                    {
                        if (_xmlStreamWriter != null)
                        {
                            _xmlStreamWriter.Close();
                        }
                    }
                }

                //=====================
                // Write trigger file
                //=====================
                if (_triggerSuffix != string.Empty)
                {
                    StreamWriter trgFile = new StreamWriter(filePath + _removeTransactionsTriggerSuffix);
                    trgFile.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Cleanup()
        {

        }
        #endregion Methods
    }



}
