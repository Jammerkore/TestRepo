using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;  // TT#1185 - Verify ENQ before Update
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.HeaderLoad
{
	/// <summary>
	/// Summary description for HeaderLoadProcess.
	/// </summary>
	public class HeaderLoadProcess
	{
//		private string sourceModule = "HeaderLoadProcess.cs";

		SessionAddressBlock _SAB;

		private int _headersRead = 0;
		private int _headersError = 0;
		private int _headersReset = 0;
		private int _headersCreate = 0;
		private int _headersModify = 0;
		private int _headersRemove = 0;

		private int _totalheadersRead = 0;
		private int _totalheadersError = 0;
		private int _totalheadersReset = 0;
		private int _totalheadersCreate = 0;
		private int _totalheadersModify = 0;
		private int _totalheadersRemove = 0;

		private Audit _audit = null;

		private string _comment = null;
		private string _statusFile = null;

		private int _headerRID = Include.NoRID;
        private bool _headersEnqueued;    // TT#1185 - Verfiry Enq before Update

		private XmlTextWriter _xmlWriter = null;

        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
        bool _methodsDefined = false;
        int _deleteWorkFlowRID = Include.UndefinedWorkflowRID;
        Hashtable _actionHashTable = new Hashtable();
        // END MID Track #6336

		private HierarchyMaintenance _hierMaint = null;
		// BEGIN MID Track #3595 - Update Style Description
		bool _updateStyleDescription = false;
		// END MID Trac #3595
		// BEGIN MID Track #4264 - Create header on modify if not found
		bool _createOnModify = false;
		// END MID Track #4264
        bool _autoAddCharacteristics = false;  // TT#168 - RMatelic - Header characteristics auto add
        private bool _allowHeaderDataUpdate = true;		// TT#712 - RMatelic - Multi Header Release - In Use by Multi Header
		// Begin TT#1581-MD - stodd - Header Reconcile API
        private List<string> _headerIdKeyList;
        private List<string> _headerKeysToMatchList;
        private int _headerIdSequenceLength;
        private string _headerIdDelimiter;
        private bool _generateHeaderID;

		// End TT#1581-MD - stodd - Header Reconcile API

        // Begin TT#1218 - JSmith - API - Header Load Performance
        Hashtable _htStyles = new Hashtable();
        Hashtable _htColorCodes = new Hashtable();
        Hashtable _htSizeCodes = new Hashtable();
        Hashtable _htMethods = new Hashtable();
        GlobalOptionsProfile _globalOptions = null;
        ApplicationSessionTransaction aTrans = null;
        // End TT#1218

        //private bool _modifyOnReset = true;
        private bool _isResetRemove = false;	// TT#1581-MD - stodd - Header Reconcile API

		// BEGIN MID Track #4264 - Create header on modify if not found
//		public HeaderLoadProcess(SessionAddressBlock SAB, ref bool hdrErrorFound)
        // Begin TT#168 - RMatelic - Header characteristics auto add
        //public HeaderLoadProcess(SessionAddressBlock SAB, ref bool hdrErrorFound, bool aCreateOnModify)
		// END MID Track #4264
		// Begin TT#1581-MD - stodd - Header Reconcile API
        public HeaderLoadProcess(SessionAddressBlock SAB, ref bool hdrErrorFound, bool aCreateOnModify, bool aAutoAddCharacteristics, int headerIdSequenceLength, 
            List<string> headerIdKeyList, List<string> headerKeysToMatchList, string headerIdDelimiter, bool generateHeaderID)
        // End TT#168 
		// End TT#1581-MD - stodd - Header Reconcile API
		{
			try
			{
				_SAB = SAB;

				_audit = _SAB.ClientServerSession.Audit;

				_hierMaint = new HierarchyMaintenance(_SAB);
				// BEGIN MID Track #4264 - Create header on modify if not found
				_createOnModify = aCreateOnModify;
				// END MID Track #4264

                _autoAddCharacteristics = aAutoAddCharacteristics;  // TT#168 - RMatelic - Header characteristics auto add
				// Begin TT#1581-MD - stodd - Header Reconcile API
                _headerIdSequenceLength = headerIdSequenceLength;
                _headerIdDelimiter = headerIdDelimiter;
                _headerIdKeyList = headerIdKeyList;
                _headerKeysToMatchList = headerKeysToMatchList;
                _generateHeaderID = generateHeaderID;
				// End TT#1581-MD - stodd - Header Reconcile API
                _headersEnqueued = false;                           // TT#1185 - Verify ENQ before update
                 // Begin TT#1218 - JSmith - API - Header Load Performance
                 _globalOptions = _SAB.ClientServerSession.GlobalOptions;
                 aTrans = _SAB.ApplicationServerSession.CreateTransaction();
                // End TT#1218
			}

			catch (Exception Ex)
			{
				hdrErrorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
			}
		}

		// ====================================================
		// Serialize and process the xml input transaction file
		// ====================================================
		public eReturnCode SerializeVariableFile(string hdrTransFile, bool updateStyleDesc, ref bool hdrErrorFound)
		{
			_headersRead = 0;
			_headersError = 0;
			_headersReset = 0;
			_headersCreate = 0;
			_headersModify = 0;
			_headersRemove = 0;

			string msgText = null;

			Headers headers = null;

			TextReader xmlReader = null;

			XmlSerializer xmlSerial = null;

			eReturnCode rtnCode = eReturnCode.successful;
			// BEGIN MID Track #3595 - Update Style Description
			_updateStyleDescription = updateStyleDesc;
			// END MID Track #3595 

			// ================
			// Begin processing
			// ================
			try
			{
				try
				{
					xmlSerial = new XmlSerializer(typeof(Headers));

					xmlReader = new StreamReader(hdrTransFile);

					headers = (Headers)xmlSerial.Deserialize(xmlReader);

                    // Begin Track #4229 - JSmith - API locks .XML input file
                    //xmlReader.Close();
                    // End Track #4229
				}

				catch (Exception Ex)
				{
					hdrErrorFound = true;

					_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);

					rtnCode = eReturnCode.severe;
				}
                // Begin Track #4229 - JSmith - API locks .XML input file
                finally
                {
                    if (xmlReader != null)
                        xmlReader.Close();
                }
                // End Track #4229

				if (!hdrErrorFound)
				{
					if (headers.Header != null)
					{
						// ===============================================
						// Create an xml writer and set formatting options
						// ===============================================
						_statusFile = Path.ChangeExtension(hdrTransFile, ".out.xml");
						_xmlWriter = new XmlTextWriter(_statusFile, System.Text.Encoding.UTF8);

						_xmlWriter.Formatting = Formatting.Indented;

						_xmlWriter.Indentation = 4;

						// =================
						// Open the document
						// =================
						_xmlWriter.WriteStartDocument();

						// ===============
						// Write a comment
						// ===============
						// Begin Track #5182 - JSmith - Time does not match file create time
						//_comment = "Header Load Status for: " + Path.GetFileName(hdrTransFile) + " submitted on " + DateTime.Now.ToString("U");
						_comment = "Header Load Status for: " + Path.GetFileName(hdrTransFile) + " submitted on " + DateTime.Now.ToString("F");
						// End Track #5182
						_xmlWriter.WriteComment(_comment);

						// ======================
						// Write the root element
						// ======================
						_xmlWriter.WriteStartElement("HeaderLoadStatus", "http://tempuri.org/HeaderLoadStatusSchema.xsd");

                        // Begin TT#168 - RMatelic - Header characteristics auto add
                        if (headers.Options != null)
                        {
                            if (headers.Options.AutoAddCharacteristicsSpecified)
                            {
                                _autoAddCharacteristics = headers.Options.AutoAddCharacteristics;
                            }
                        }
                        // End TT#168  

						foreach (HeadersHeader hdrTran in headers.Header)
						{
							++_headersRead;

							rtnCode = ProcessOneHeader(hdrTran);

							if (rtnCode != eReturnCode.successful)
							{
                                hdrErrorFound = true;   // TT#4693 - stodd - Completion Status is not correct based on Max Return setting in the Task List
								++_headersError;
							}
						}

						// ======================
						// Close the root element
						// ======================
						_xmlWriter.WriteEndElement();

						// ==================
						// Close the document
						// ==================
						_xmlWriter.WriteEndDocument();

						// ===================================
						// Flush the buffer and close the file
						// ===================================
						_xmlWriter.Flush();

						_xmlWriter.Close();
					}
					else
					{
						msgText = "No Header transactions found in input file" + System.Environment.NewLine;
						_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
					}
				}
			}

			catch (Exception Ex)
			{
				hdrErrorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
				if (xmlReader != null)
				{
					xmlReader.Close();
				}

				if (_xmlWriter != null)
				{
					_xmlWriter.Close();
				}

                // Begin TT#3636 - JSmith - Header Load Audit Summary should not be included in the Audit Details
                //msgText = "Headers Read: " + _headersRead.ToString() + System.Environment.NewLine;
                //msgText += "Headers In Error: " + _headersError.ToString() + System.Environment.NewLine;
                //msgText += "Headers Created: " + _headersCreate.ToString() + System.Environment.NewLine;
                //msgText += "Headers Modified: " + _headersModify.ToString() + System.Environment.NewLine;
                //msgText += "Headers Removed: " + _headersRemove.ToString() + System.Environment.NewLine;
                //msgText += "Headers Reset: " + _headersReset.ToString() + System.Environment.NewLine;
                // End TT#3636 - JSmith - Header Load Audit Summary should not be included in the Audit Details

				_totalheadersRead += _headersRead;
				_totalheadersError += _headersError;
				_totalheadersReset += _headersReset;
				_totalheadersCreate += _headersCreate;
				_totalheadersModify += _headersModify;
				_totalheadersRemove += _headersRemove;

                // Begin TT#3636 - JSmith - Header Load Audit Summary should not be included in the Audit Details
                //_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
                // End TT#3636 - JSmith - Header Load Audit Summary should not be included in the Audit Details

//				_audit.HeaderLoadAuditInfo_Add(_headersRead, _headersError, _headersCreate, _headersModify, _headersRemove, _headersReset);
			}

			return rtnCode;
		}

		public eReturnCode WriteRecordCounts()
		{
			eReturnCode rtnCode = eReturnCode.successful;
			try
			{
				_audit.HeaderLoadAuditInfo_Add(_totalheadersRead, _totalheadersError, _totalheadersCreate, 
					_totalheadersModify, _totalheadersRemove, _totalheadersReset);
			}
			catch
			{
				throw;
			}
			return rtnCode;
		}

		// ==============================
		// Process one header transaction
		// ==============================
		public eReturnCode ProcessOneHeader(HeadersHeader hdrTran)
		{
			EditMsgs em = null;

			string msgText = null;

			eReturnCode rtnCode = eReturnCode.successful;
			eReturnCode rtnModify = eReturnCode.successful;

            // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors
            Header dataLock = null;
            // End TT#1453

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();
            _allowHeaderDataUpdate = true;		// TT#712 - RMatelic - Multi Header Release - In Use by Multi Header
            AllocationProfile ap = null;		// TT#936 - MD - Prevent the saving of empty Group Allocations
			try
			{
//				Thread.Sleep(5);
				// BEGIN MID Track 4279 - trim spaces off string fields
				Include.TrimStringsInObject(hdrTran);
				// END MID Track 4279

				// Begin TT#1581-MD - stodd - Header Reconcile API
                HeaderIDGenerator headerIDGenerator = new HeaderIDGenerator(_SAB);
                //==========================================================================
                // If generate header ID is true, process will try to generate a header ID
                // for those headers with a missing or null header ID.
                // Headers with header IDs are ignored.
                //==========================================================================
                if (_generateHeaderID)
                {
                    string hdrId = hdrTran.HeaderId;
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    //rtnCode = headerIDGenerator.GetHeaderId(hdrTran, _headersRead, ref hdrId, ref _isResetRemove, _headerIdKeyList, _headerKeysToMatchList, _headerIdSequenceLength, _headerIdDelimiter, ref em);
                    rtnCode = _SAB.ControlServerSession.GetHeaderId(hdrTran, _headersRead, ref hdrId, ref _isResetRemove, _headerIdKeyList, _headerKeysToMatchList, _headerIdSequenceLength, _headerIdDelimiter, ref em);
                    // End TT#1966-MD - JSmith - DC Fulfillment
                    if (rtnCode == eReturnCode.successful)
                    {
                        hdrTran.HeaderId = hdrId;
                    }
                }
                // Informational message from GenerateHeaderId()
                //if (rtnCode == eReturnCode.successful && msgText != null && msgText.Trim() != string.Empty)
                //{
                //    em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
                //}

                if (rtnCode != eReturnCode.successful)
                {
                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                }
                else if (hdrTran.HeaderId == null || hdrTran.HeaderId.Trim() == string.Empty)
				// Begin TT#1581-MD - stodd - Header Reconcile API
                {
                    msgText = "Allocation Header Transaction #" + _headersRead.ToString();
                    msgText += " has NO Header ID" + System.Environment.NewLine;
                    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);

                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                    rtnCode = eReturnCode.severe;
                }
                else
                {	// BEGIN MID Track #3560 - trim HeaderId; added StyleId and ParentOfStyleId, and HeaderDescription
                    hdrTran.HeaderId = hdrTran.HeaderId.Trim();

                    // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors
                    dataLock = new Header();
                    dataLock.OpenUpdateConnection(eLockType.Header, hdrTran.HeaderId);
                    // End TT#1453

                    // begin MID Track 4100 Invalid Header ID causes unsightly message in the API
                    if (!MIDMath.ValidFileName(hdrTran.HeaderId))
                    {
                        msgText = "Allocation Header " + hdrTran.HeaderId;
                        msgText += " (Transaction #" + _headersRead.ToString() + ")";
                        msgText += string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_HeaderNameInvalid), hdrTran.HeaderId, Include.HeaderNameExcludedCharacters);
                        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                        rtnCode = eReturnCode.severe;
                    }
                    // end MID Track 4100 Invalid Header ID causes unsightly message in the API
                    if (hdrTran.StyleId != null)
                        hdrTran.StyleId = hdrTran.StyleId.Trim();

                    if (hdrTran.ParentOfStyleId != null)
                        hdrTran.ParentOfStyleId = hdrTran.ParentOfStyleId.Trim();

                    if (hdrTran.HeaderDescription != null)
                        hdrTran.HeaderDescription = hdrTran.HeaderDescription.Trim();
                    // END MID Track #3560

                    msgText = "Allocation Header " + hdrTran.HeaderId;
                    msgText += " (Transaction #" + _headersRead.ToString() + ")";
                    msgText += " - Begin Processing" + System.Environment.NewLine;
                    _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                    ApplicationSessionTransaction aTrans = _SAB.ApplicationServerSession.CreateTransaction();

                    // Begin TT#936 - MD - Prevent the saving of empty Group Allocations
                    //AllocationProfile ap = new AllocationProfile(aTrans, hdrTran.HeaderId, Include.NoRID, _SAB.ApplicationServerSession);
                    //ap = new AllocationProfile(aTrans, hdrTran.HeaderId, Include.NoRID, _SAB.ApplicationServerSession);	// TT#1040 - MD - stodd - header load API for Group Allocation 
                    // End TT#936 - MD - Prevent the saving of empty Group Allocations

                    // Begin TT#1040 - MD - stodd - header load API for Group Allocation 
                    AllocationHeaderProfile ahp = _SAB.HeaderServerSession.GetHeaderData(hdrTran.HeaderId, false, false, true);
                    AllocationHeaderProfile asrthp = null;
                    AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);

					// Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                    string exceptMsg = string.Empty;
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    //rtnCode = EnqueueHeader(aTrans, ahp.Key, hdrTran, ref em);
                    if (ahp.IsSubordinateHeader)
                    {
                        List<int> hdrRidList = new List<int>();
                        hdrRidList.Add(ahp.MasterRID);
                        MasterHeaderProfile mhp = new MasterHeaderProfile(aTrans, null, ahp.MasterRID, _SAB.ApplicationServerSession);
                        foreach (int subordinateRID in mhp.SubordinateRIDs)
                        {
                            hdrRidList.Add(subordinateRID);
                        }
                        if (!EnqueueHeader(aTrans, hdrRidList, out exceptMsg)) 
                        {
                            rtnCode = eReturnCode.warning;
                            em.AddMsg(eMIDMessageLevel.Edit, exceptMsg, GetType().Name); 
                        }
                    }
                    else
                    {
                        rtnCode = EnqueueHeader(aTrans, ahp.Key, hdrTran, ref em);
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment

                    if (rtnCode == eReturnCode.successful)
                    {
					// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                        // Get assortment header information
                        if (ahp.AsrtRID != Include.NoRID)
                        {
                            asrthp = _SAB.HeaderServerSession.GetHeaderData(ahp.AsrtRID, false, false, true);
                            // Begin TT#2089-MD - JSmith - Header Load modification error on Group Allocation Headers
							//aTrans.LoadHeadersInTransaction(ahp.Key); //TT#1500 - MD - DOConnell - ASST - Tried to Modify(with a transaction) a header attached to an Assortment and recieved a Null Reference Exception.
                            if (asrthp.AsrtType != (int)eAssortmentType.GroupAllocation)
                            {
                                aTrans.LoadHeadersInTransaction(ahp.Key); //TT#1500 - MD - DOConnell - ASST - Tried to Modify(with a transaction) a header attached to an Assortment and recieved a Null Reference Exception.
                            }
                            // End TT#2089-MD - JSmith - Header Load modification error on Group Allocation Headers
                        }

                        // If this is a header that belong to a group allocation, load the header information this way
                        if (asrthp != null && asrthp.AsrtType == (int)eAssortmentType.GroupAllocation)
                        {
                            aTrans.LoadHeadersInTransaction(ahp.Key);
                            ap = aTrans.GetAllocationProfile(ahp.Key);
                        }
                        else
                        {
                            // Begin TT#4370 - JSmith - Header Load Error - Color not defined for bulk
                            //ap = new AllocationProfile(aTrans, hdrTran.HeaderId, Include.NoRID, _SAB.ApplicationServerSession);
                            ap = new AllocationProfile(aTrans, hdrTran.HeaderId, Include.NoRID, _SAB.ApplicationServerSession, false);
                            // End TT#4370 - JSmith - Header Load Error - Color not defined for bulk
                            apl.Add(ap);
                            aTrans.SetMasterProfileList(apl);
                        }


                        // End TT#1040 - MD - stodd - header load API for Group Allocation 

                        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
                        // ================
                        // Determine methods
                        // ================
                        _methodsDefined = false;
                        if (hdrTran.Methods != null)
                        {
                            GetActions(ref em);
                            bool foundHeaderMethodError = false;
                            foreach (HeadersHeaderMethod hdrTranMethod in hdrTran.Methods)
                            {
                                //---We have Methods at this point-------
                                if (!hdrTranMethod.TypeSpecified)
                                {
                                    //---No TypeSpecified-------
                                    msgText = MIDText.GetText(eMIDTextCode.msg_hl_NoTypeSpecified);
                                    msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                    msgText = msgText.Replace("{1}", hdrTranMethod.Name.ToString());
                                    msgText = msgText + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                    foundHeaderMethodError = true;
                                }
                                else if (hdrTranMethod.Type.ToString().ToUpper() != "GENERALALLOCATION" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "ALLOCATIONOVERRIDE" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "RULE" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "VELOCITY" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "FILLSIZES" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "BASISSIZE" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "SIZENEED" &&
                                         hdrTranMethod.Type.ToString().ToUpper() != "ACTION")
                                {
                                    //---The Method.Type did not match any of the items above-------
                                    msgText = MIDText.GetText(eMIDTextCode.msg_hl_InvalidTypeSpecified);
                                    msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                    msgText = msgText.Replace("{1}", hdrTranMethod.Name.ToString());
                                    msgText = msgText.Replace("{2}", hdrTranMethod.Type.ToString());
                                    msgText = msgText + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                    foundHeaderMethodError = true;
                                }
                                else if (hdrTranMethod.Type.ToString().ToUpper() == "ACTION" &&
                                         !_actionHashTable.ContainsKey(hdrTranMethod.Name.ToString().ToUpper().Replace(" ", "")))
                                {
                                    //---The Method.Action did not match any of the items above-------
                                    msgText = MIDText.GetText(eMIDTextCode.msg_hl_InvalidActionSpecified);
                                    msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                    msgText = msgText.Replace("{1}", hdrTranMethod.Type.ToString());
                                    msgText = msgText.Replace("{2}", hdrTranMethod.Name.ToString());
                                    msgText = msgText + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                    foundHeaderMethodError = true;
                                }
                                else
                                {
                                    _methodsDefined = true;
                                }
                            }
                            if (foundHeaderMethodError)
                            {
                                //---Write Error Out To XML Status File-----------------------
                                WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                                rtnCode = eReturnCode.editErrors;
                            }
                        }
                        // END MID Track #6336

                        // ================
                        // Determine action
                        // ================
                        if (!hdrTran.HeaderActionSpecified)
                        {
                            msgText = "Allocation Header " + hdrTran.HeaderId;
                            msgText += " has NO Header Action" + System.Environment.NewLine;
                            em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                            rtnCode = eReturnCode.editErrors;
                        }
                        else
                        {
                            if (hdrTran.HeaderAction.ToString().ToUpper() != "CREATE" &&
                                hdrTran.HeaderAction.ToString().ToUpper() != "MODIFY" &&
                                hdrTran.HeaderAction.ToString().ToUpper() != "REMOVE" &&
                                hdrTran.HeaderAction.ToString().ToUpper() != "RESET")
                            {
                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " has an invalid Header Action [" + hdrTran.HeaderAction.ToString() + "]" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                                WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                                rtnCode = eReturnCode.editErrors;
                            }
                        }

                        //==========================
                        // Multi-header check
                        //==========================
                        if (rtnCode == eReturnCode.successful)
                        {
                            if (ap.InUseByMulti)
                            {
                                // Begin TT#712 - RMatelic - Multi Header Release - In Use by Multi Header
                                // begin MID Track 4033 allow children of multi to change
                                //if (ap.ReleaseApproved 
                                //	&& hdrTran.HeaderAction.ToString().ToUpper() != "RESET")
                                //{
                                //	// end MID TRack 4033 allow children of multi to change
                                //	string aMsg = this._audit.GetText(eMIDTextCode.msg_HeaderInUseByMultiHeader, false) + System.Environment.NewLine;
                                //	msgText = aMsg.Replace("{0}",hdrTran.HeaderId);
                                //	em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                //
                                //	WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                                //
                                //	rtnCode = eReturnCode.editErrors;
                                //} // MID Track 4033 allow children of multi to change
                                if (ap.ReleaseApproved || ap.Released)
                                {
                                    if (hdrTran.Characteristic == null && (hdrTran.Notes == null || hdrTran.Notes == string.Empty))
                                    {
                                        if (hdrTran.HeaderAction.ToString().ToUpper() != "RESET")
                                        {
                                            string aMsg = this._audit.GetText(eMIDTextCode.msg_HeaderInUseByMultiHeader, false) + System.Environment.NewLine;
                                            msgText = aMsg.Replace("{0}", hdrTran.HeaderId);
                                            em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                                            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                                            rtnCode = eReturnCode.editErrors;
                                        }
                                    }
                                    else if (hdrTran.HeaderAction.ToString().ToUpper() != "RESET")
                                    {
                                        _allowHeaderDataUpdate = false;
                                        string msg = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_CanOnlyModifyNotesAndChars), hdrTran.HeaderId) + System.Environment.NewLine;
                                        em.AddMsg(eMIDMessageLevel.Information, msg, GetType().Name);
                                    }
                                }
                                // End TT#712
                            }
                            else if (ap.HeaderType == eHeaderType.MultiHeader)
                            {
                                if (hdrTran.HeaderAction.ToString().ToUpper() != "RESET")
                                {
                                    string aMsg = this._audit.GetText(eMIDTextCode.msg_InvalidMultiHeaderProcess, false) + System.Environment.NewLine;
                                    msgText = aMsg.Replace("{0}", hdrTran.HeaderId);
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                                    rtnCode = eReturnCode.editErrors;
                                }
                            }
                        }

                        // Begin TT#5048 - JSmith - Allows header type to be changed when group allocation
                        if (ap.HeaderType == eHeaderType.Assortment ||
                            ap.HeaderType == eHeaderType.Placeholder ||
                            ap.IsMasterHeader ||     // TT#1966-MD - JSmith - DC Fulfillment
                            ap.HeaderType == eHeaderType.MultiHeader)
                        {
                            string aMsg = this._audit.GetText(eMIDTextCode.msg_CannotProcessHeaderType, false) + System.Environment.NewLine;
                            msgText = aMsg.Replace("{0}", hdrTran.HeaderId);
                            if (asrthp != null && asrthp.AsrtType == (int)eAssortmentType.GroupAllocation)
                            {
                                msgText = msgText.Replace("{1}", "Group Allocation");
                            }
                            else if (ap.IsMasterHeader)
                            {
                                msgText = msgText.Replace("{1}", "Master");
                            }
                            else
                            {
                                msgText = msgText.Replace("{1}", ap.HeaderType.ToString());
                            }
                            em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                            rtnCode = eReturnCode.editErrors;
                        }
                        // End TT#5048 - JSmith - Allows header type to be changed when group allocation

                        if (rtnCode == eReturnCode.successful)
                        {
                            // ============================================================================
                            // If header already exists, this is not a create but a modify / remove / reset
                            // ============================================================================
                            _headerRID = ap.GetHeaderRID(ap.HeaderID);

                            if (_headerRID > 0)
                            {
                                if (hdrTran.HeaderAction.ToString().ToUpper() == "CREATE")
                                {
                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " Action '" + hdrTran.HeaderAction.ToString();
                                    msgText += "' invalid when header already exists" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                                    rtnCode = eReturnCode.editErrors;
                                }
                                else
                                {
                                    ap.Key = _headerRID;

                                    if (hdrTran.HeaderAction.ToString().ToUpper() == "MODIFY")
                                    {
                                        ap.HeaderChangeType = eChangeType.update;

                                        rtnCode = ModifyHeader(hdrTran, ap, aTrans);

                                        if (rtnCode == eReturnCode.successful)
                                        {
                                            ++_headersModify;
                                        }
                                    }
                                    else if (hdrTran.HeaderAction.ToString().ToUpper() == "REMOVE")
                                    {
                                        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
                                        Header header = new Header();

                                        int subordRID = header.GetSubordForMaster(_headerRID);

                                        if (subordRID != Include.NoRID)
                                        {
                                            rtnCode = eReturnCode.editErrors;
                                            //// ==============================
                                            //// Write the HeaderStatus element
                                            //// ==============================
                                            //_xmlWriter.WriteStartElement("HeaderStatus");
                                            //// ===================
                                            //// Write the Header ID
                                            //// ===================
                                            //_xmlWriter.WriteStartAttribute(null, "HeaderId", null);
                                            //	_xmlWriter.WriteString(ap.HeaderID);
                                            //_xmlWriter.WriteEndAttribute();
                                            //// =======================
                                            //// Write the Header Action
                                            //// =======================
                                            //_xmlWriter.WriteStartAttribute(null, "HeaderAction", null);
                                            //	_xmlWriter.WriteString("Remove");
                                            //_xmlWriter.WriteEndAttribute();
                                            //// =====================
                                            //// Write the Load Status
                                            //// =====================
                                            //_xmlWriter.WriteStartAttribute(null, "LoadStatus", null);
                                            //	_xmlWriter.WriteString("false");
                                            //_xmlWriter.WriteEndAttribute();
                                            //// ==============================
                                            //// Close the HeaderStatus element
                                            //// ==============================
                                            //_xmlWriter.WriteEndElement();
                                            //msgText = "Allocation Header " + ap.HeaderID + " action '" + action + "' invalid when header is an active Mastr" + System.Environment.NewLine;
                                            //em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                                        }
                                        else
                                        {
                                            // Begin TT#1040 - MD - stodd - header load API for Group Allocation 
                                            // Begin TT#4755 - JSmith - Cannot Remove header from Group
                                            //if (ap.AsrtRID != Include.NoRID)
                                            //{
                                            //    AllocationProfile asp = aTrans.GetAssortmentMemberProfile(ap.AsrtRID);
                                            //    if (asp != null && asp.AllocationStarted && !asp.AllUnitsInReserve)
                                            //    {
                                            //        msgText = "Allocation Header " + hdrTran.HeaderId;
                                            //        msgText += " Action '" + hdrTran.HeaderAction.ToString();
                                            //        msgText += "'. " + MIDText.GetTextOnly(eMIDTextCode.msg_al_HeadersCannotBeRemovedFromGroupAllocation) + System.Environment.NewLine;
                                            //        em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                                            //        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                                            //        rtnCode = eReturnCode.editErrors;

                                            //        // CANNOT REMOVE
                                            //    }
                                            //}
                                            // End TT#4755 - JSmith - Cannot Remove header from Group

                                            if (rtnCode == eReturnCode.successful)
                                            {
                                                ap.HeaderChangeType = eChangeType.delete;
                                                rtnCode = RemoveHeader(hdrTran, ap, aTrans);
                                                if (rtnCode == eReturnCode.successful)
                                                {
                                                    ++_headersRemove;
                                                }
                                            }
                                            // End TT#1040 - MD - stodd - header load API for Group Allocation 
                                        }
                                        // (CSMITH) - END MID Track #3219
                                    }
                                    else
                                    {
                                        ap.HeaderChangeType = eChangeType.update;

                                        rtnCode = ResetHeader(hdrTran, ap, aTrans);

                                        if (rtnCode == eReturnCode.successful)
                                        {
                                            ++_headersReset;

										// Begin TT#1581-MD - stodd - Header Reconcile API
                                        if (_isResetRemove)
                                        {
                                            _isResetRemove = false;
                                            ap.HeaderChangeType = eChangeType.delete;
                                            rtnCode = RemoveHeader(hdrTran, ap, aTrans);
                                            if (rtnCode == eReturnCode.successful)
                                            {
                                                ++_headersRemove;
                                            }
                                        }
                                        else
                                        {
										// End TT#1581-MD - stodd - Header Reconcile API
                                            ap.HeaderChangeType = eChangeType.update;
                                            _allowHeaderDataUpdate = true;		// TT#712 - RMatelic - Multi Header Release - In Use by Multi Header
                                            rtnModify = ModifyHeader(hdrTran, ap, aTrans);

                                            if (rtnModify == eReturnCode.successful)
                                            {
                                                ++_headersModify;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            else
                            {
                                // BEGIN MID Track #4264 - Create header on modify if not found
                                //if (_createOnModify) // BEGIN MID Track #5087 - Wrong msg in Audit and out.xml on Remove when _createOnModify = true
                                if (_createOnModify
                                    && hdrTran.HeaderAction == HeadersHeaderHeaderAction.Modify) // END MID Track #5087
                                {
                                    hdrTran.HeaderAction = HeadersHeaderHeaderAction.Create;
                                }
                                // END MID Track #4264


                                if (hdrTran.HeaderAction.ToString().ToUpper() == "CREATE")
                                {
                                    ap.HeaderChangeType = eChangeType.add;

                                    rtnCode = CreateHeader(hdrTran, ap, aTrans);

                                    if (rtnCode == eReturnCode.successful)
                                    {
                                        ++_headersCreate;
                                    }
                                }
                                else
                                {
                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " Action '" + hdrTran.HeaderAction.ToString();
                                    msgText += "' invalid when header does not exist" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

                                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                                    rtnCode = eReturnCode.editErrors;
                                }
                            }
                        }

                        for (int e = 0; e < em.EditMessages.Count; e++)
                        {
                            EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

                            _audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
                        }

                        em.ClearMsgs();

                        //					Thread.Sleep(5);

                        msgText = "Allocation Header " + hdrTran.HeaderId;
                        msgText += " (Transaction #" + _headersRead.ToString() + ")";
                        msgText += " - End Processing" + System.Environment.NewLine;
                        _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                        aTrans.RemoveMasterProfileList(apl);
                        apl.Clear();
                        ap.Dispose();

                        // Begin TT#936 - MD - Prevent the saving of empty Group Allocations
                        if (ap != null && hdrTran.HeaderAction.ToString().ToUpper() == "REMOVE")
                        {
                            if (ap.AsrtRID > 0) // Header being deleted belongs to an assortment/group allocation
                            {
                                // Begin TT#1181-MD - stodd - error removing all headers from a group - 
                                AssortmentProfile asp = (AssortmentProfile)aTrans.GetAssortmentMemberProfile(ap.AsrtRID);
                                //AssortmentProfile asp = new AssortmentProfile(aTrans, null, ap.AsrtRID, _SAB.ApplicationServerSession);
                                if (asp != null)
                                {
                                    int hdrCnt = asp.GetAssortmentHeaderCount();
                                    if (hdrCnt == 0 && asp.AsrtType == (int)eAssortmentType.GroupAllocation)
                                    {
                                        rtnCode = RemoveGroupAllocation(hdrTran, aTrans, ref apl, asp);
                                    }
                                    else if (ap.PlaceHolderRID > 0) // Header being deleted belongs to a placeholder allocation
                                    {
                                        AllocationProfile pap = (AllocationProfile)aTrans.GetAssortmentMemberProfile(ap.PlaceHolderRID);
                                        //AllocationProfile pap = new AllocationProfile(aTrans, null, ap.PlaceHolderRID, _SAB.ApplicationServerSession);
                                        if (pap != null)
                                        {
                                            int plHdrCnt = pap.GetPlaceholderHeaderCount();
                                            if (plHdrCnt == 0)
                                            {
                                                RemovePlaceholder(hdrTran, aTrans, ref apl, pap);
                                            }
                                        }
                                    }
                                }
                                // End TT#1181-MD - stodd - error removing all headers from a group - 
                            }

                        }
                        // End TT#936 - MD - Prevent the saving of empty Group Allocations

                        DequeueHeaders(aTrans);		// TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 

                        aTrans.Dispose();
					// Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                    }
                    else   // Enqueue failed
                    {
                        aTrans.Dispose();
                    }
					// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                }
			}

			catch (Exception Ex)
			{
				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
                // Begin TT#1453 - JSmith - Multiple Loads -> Foreign Key Errors
                // Ensure that the lock is released.
                if (dataLock != null && dataLock.ConnectionIsOpen)
                {
                    dataLock.CloseUpdateConnection();
                }
                // End TT#1453
                DequeueHeaders(aTrans);	// TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// Begin TT#1581-MD - stodd - Header Reconcile API
        private eMIDMessageLevel ConvertReturnCodeToMessageLevel(eReturnCode rc)
        {
            eMIDMessageLevel msgLevel = eMIDMessageLevel.Severe;

             switch (rc)
                {
                    case eReturnCode.severe:
                        msgLevel = eMIDMessageLevel.Severe;
                        break;
                    case eReturnCode.editErrors:
                        msgLevel = eMIDMessageLevel.Edit;
                        break;
                     case eReturnCode.warning:
                        msgLevel = eMIDMessageLevel.Warning;
                        break;
                    default:
                        break;
                }

            return msgLevel;
        }
		// End TT#1581-MD - stodd - Header Reconcile API

//		// BEGIN MID Track 4279 - trim spaces off string fields
//		private void TrimStringsInObject(object aObject)
//		{
//			try
//			{
//				Type t = aObject.GetType();
//				FieldInfo[] fi = t.GetFields();
//				foreach(FieldInfo field in fi)
//				{
////					string name = field.Name;
////					string fieldType = field.FieldType.ToString();
////					Debug.WriteLine(fieldType);
//					object fieldValue = field.GetValue(aObject);
//					if (field.FieldType == typeof(System.String))
//					{
//						if (fieldValue != null)
//						{
//							string trimStrValue = (Convert.ToString(fieldValue)).Trim();
//							field.SetValue(aObject, trimStrValue);
//						}
//					}
//					else if (field.FieldType.BaseType == typeof(System.Array))
//					{
//						if (fieldValue != null)
//						{
//							object[] objects = (object[])fieldValue;
//							foreach (object Object in objects)
//							{
//								TrimStringsInObject(Object);
//							}
//						}
//					}
////					else if (field.FieldType == typeof(HeadersHeaderBulkColor[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderBulkColor[] bulkColors = (HeadersHeaderBulkColor[])fieldValue;
////							foreach (HeadersHeaderBulkColor bulkColor in bulkColors)
////							{
////								TrimStringsInObject(bulkColor);
////							}
////						}
////					}
////					else if (field.FieldType == typeof(HeadersHeaderBulkColorBulkColorSize[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderBulkColorBulkColorSize[] bulkColorSizes = (HeadersHeaderBulkColorBulkColorSize[])fieldValue;
////							foreach (HeadersHeaderBulkColorBulkColorSize bulkColorSize in bulkColorSizes)
////							{
////								TrimStringsInObject(bulkColorSize);
////							}
////						}
////					}
////					else if (field.FieldType == typeof(HeadersHeaderPack[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderPack[] packs = (HeadersHeaderPack[])fieldValue;
////							foreach (HeadersHeaderPack pack in packs)
////							{
////								TrimStringsInObject(pack);
////							}
////						}
////					}
////					else if (field.FieldType == typeof(HeadersHeaderPackPackColor[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderPackPackColor[] packColors = (HeadersHeaderPackPackColor[])fieldValue;
////							foreach (HeadersHeaderPackPackColor packColor in packColors)
////							{
////								TrimStringsInObject(packColor);
////							}
////						}
////					}
////					else if (field.FieldType == typeof(HeadersHeaderPackPackColorPackColorSize[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderPackPackColorPackColorSize[] packColorSizes = (HeadersHeaderPackPackColorPackColorSize[])fieldValue;
////							foreach (HeadersHeaderPackPackColorPackColorSize packColorSize in packColorSizes)
////							{
////								TrimStringsInObject(packColorSize);
////							}
////						}
////					}
////					else if (field.FieldType == typeof(HeadersHeaderPackPackSize[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderPackPackSize[] packSizes = (HeadersHeaderPackPackSize[])fieldValue;
////							foreach (HeadersHeaderPackPackSize packSize in packSizes)
////							{
////								TrimStringsInObject(packSize);
////							}
////						}
////					}
////					else if (field.FieldType == typeof(HeadersHeaderCharacteristic[]))
////					{
////						if (fieldValue != null)
////						{
////							HeadersHeaderCharacteristic[] characteristics = (HeadersHeaderCharacteristic[])fieldValue;
////							foreach (HeadersHeaderCharacteristic characteristic in characteristics)
////							{
////								TrimStringsInObject(characteristic);
////							}
////						}
////					}
//				}
//			}
//			catch(FieldAccessException e)
//			{
//				_audit.Add_Msg(eMIDMessageLevel.Severe, "FieldAccessException : " + e.Message, GetType().Name);
//				throw;
//			}
//			catch(TargetException e)
//			{
//				_audit.Add_Msg(eMIDMessageLevel.Severe, "TargetException : " + e.Message, GetType().Name);
//				throw;
//			}
//			catch(ExecutionEngineException e)
//			{
//				_audit.Add_Msg(eMIDMessageLevel.Severe, "ExecutionEngineException : " + e.Message, GetType().Name);
//				throw;
//			}
//			catch(MemberAccessException e)
//			{
//				_audit.Add_Msg(eMIDMessageLevel.Severe, "MemberAccessException : " + e.Message, GetType().Name);
//				throw;
//			}
//			catch(Exception e)
//			{
//				_audit.Add_Msg(eMIDMessageLevel.Severe, "Exception : " + e.Message, GetType().Name);
//				throw;
//			}
//		}
//		// END MID Track 4279

		// =================
		// Create new header
		// =================
        public eReturnCode CreateHeader(HeadersHeader hdrTran, AllocationProfile ap, ApplicationSessionTransaction aTrans)
		{
			int totQty = 0;
			int colrQty = 0;
			int packQty = 0;
			int sizeQty = 0;
			int bulkMult = 1;
			int charsRtn = 0;
			int packMult = 0;

			EditMsgs em = null;

			double unitCst = 0.0;
			double unitRtl = 0.0;

			string msgText = null;
			string exceptMsg = null;

			bool charError = false;
			bool editError = false;
			bool typeError = false;
			bool createOkay = true;
            //bool hdrEnqueued = false;  // TT#1185 - Verify ENQ before Update
			bool workFlowTrigger = false;
            // Begin TT#3220 - JSmith - PROD - Issue while header create\modify
            bool generateWorkflow = false;
            // End TT#3220 - JSmith - PROD - Issue while header create\modify

			SizeCodeProfile scp = null;

			ColorCodeProfile ccp = null;

			// Begin MID Track #4958 - JSmith - header description using style ID
//			int styleRID = Include.NoRID;
			HierarchyNodeProfile styleHnp = null;
			// End MID Track #4958
			int sizeHnRID = Include.NoRID;
			int colorHnRID = Include.NoRID;

			bool bulkColorError = false;
			bool bulkColorSizeError = false;

			bool packError = false;
			bool packGeneric = true;
			bool packSizeError = false;
			bool packColorError = false;
			bool packColorSizeError = false;

			eReturnCode rtnCode = eReturnCode.successful;

			//Begin TT#1252 - JScott - Duplicate key in Workspace when opening application
			string hdrCharGrpID;

			//End TT#1252 - JScott - Duplicate key in Workspace when opening application
			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{
				// =============================================================================
				// Headers are NOT enqueued during CREATE processing since the key being used is
				// the header RID and it has been set to -1 indicating a new header causing a
				// potential conflict with other headers being created
				//
				// hdrEnqueued will be set to "true" to simulate an enqueue and allow logic to
				// flow as it should
				// =============================================================================
				// ==============
				// Enqueue Header
				// ==============
				//hdrEnqueued = true;  // TT#1185 - Verify ENQ before Update

                // removed unnecessary comments // TT#1185 - Verify ENQ before update
                
				//if (hdrEnqueued)   // TT#1185 - Verify ENQ before update
                //{                  // TT#1185 - Verify ENQ before update

                List<int> hdrRidList = new List<int>();   // TT#1185 - Verify ENQ before update
                if (EnqueueHeader(aTrans, hdrRidList, out exceptMsg))   // TT#1185 - Verify ENQ before update
                {                                                       // TT#1185 - Verify ENQ before update
                    // ap.ReReadHeader(); (unnecessary for "create")            // TT#1185 - Verify ENQ before update
					// ==========================
					// Process header description
					// ==========================
					if (hdrTran.HeaderDescription == null || hdrTran.HeaderDescription == string.Empty)
					{
						ap.HeaderDescription = null;
					}
					else
					{
						ap.HeaderDescription = hdrTran.HeaderDescription;
					}

					// ===================
					// Process header date
					// ===================
					if (hdrTran.HeaderDateSpecified)
					{
						if (hdrTran.HeaderDate == DateTime.MinValue)
						{
							ap.HeaderDay = DateTime.Today;
						}
						else
						{
							ap.HeaderDay = hdrTran.HeaderDate;
						}
					}
					else
					{
						ap.HeaderDay = DateTime.Today;
					}

					// ==============================
					// Process style and style parent
					// ==============================
					if (hdrTran.StyleId == null || hdrTran.StyleId == string.Empty)
					{
						editError = true;

						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " has NO Style Id" + System.Environment.NewLine;
						em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
					}
					else
					{
						if (hdrTran.ParentOfStyleId == null || hdrTran.ParentOfStyleId == string.Empty)
						{
							editError = true;

							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " has NO Parent of Style Id" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
						}
						else
						{	// BEGIN MID Track 3595 - Update Style Description  - add HeaderDescription parm 
							// BEGIN MID Track 3592 - Update Style Name  - add StyleName parm 
							//styleRID = ValidateStyle(hdrTran.HeaderId, hdrTran.StyleId, hdrTran.ParentOfStyleId, ref em);
							// Begin MID Track #4958 - JSmith - header description using style ID
//							styleRID = ValidateStyle(hdrTran.HeaderId, hdrTran.StyleId, hdrTran.HeaderDescription, hdrTran.StyleName, hdrTran.ParentOfStyleId, ref em);
							styleHnp = ValidateStyle(hdrTran.HeaderId, hdrTran.StyleId, hdrTran.HeaderDescription, hdrTran.StyleName, hdrTran.ParentOfStyleId, ref em);
							// END MID Track #3595
							// END MID Track #3592

//							if (styleRID == Include.NoRID)
//							{
//								editError = true;
//							}
//
//							try
//							{
//								ap.StyleHnRID = styleRID;
//							}
							if (styleHnp.Key == Include.NoRID)
							{
								editError = true;
							}

							try
							{
								ap.StyleHnRID = styleHnp.Key;
							}
							// End MID Track #4958

							catch (Exception Ex)
							{
								editError = true;

								if (Ex.GetType() != typeof(MIDException))
								{
									em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
								}
								else
								{
									MIDException MIDEx = (MIDException)Ex;

									//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
									em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
									//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
								}
							}
						}
					}

					// ==========================
					// Process header description
					// ==========================
					if (ap.HeaderDescription == null)
					{ 
						// Begin MID Track #4958 - JSmith - header description using style ID
//						ap.HeaderDescription = hdrTran.StyleId;
						ap.HeaderDescription = styleHnp.NodeDescription;
						// End MID Track #4958
					}

					// ===================
					// Process unit retail
					// ===================
					if (!hdrTran.UnitRetailSpecified)
					{
						unitRtl = 0.0;
					}
					else
					{
						try
						{
							unitRtl = Convert.ToDouble(hdrTran.UnitRetail, CultureInfo.CurrentUICulture);

							if (unitRtl < 0.0)
							{
								unitRtl = 0.0;
							}
						}

						catch (InvalidCastException)
						{
							unitRtl = 0.0;
						}
					}

					try
					{
						ap.UnitRetail = unitRtl;
					}

					catch (Exception Ex)
					{
						editError = true;

						if (Ex.GetType() != typeof(MIDException))
						{
							em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
						}
						else
						{
							MIDException MIDEx = (MIDException)Ex;

							//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
							//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
							em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
							//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
						}
					}

					// =================
					// Process unit cost
					// =================
					if (!hdrTran.UnitCostSpecified)
					{
						unitCst = 0.0;
					}
					else
					{
						try
						{
							unitCst = Convert.ToDouble(hdrTran.UnitCost, CultureInfo.CurrentUICulture);

							if (unitCst < 0.0)
							{
								unitCst = 0.0;
							}
						}

						catch (InvalidCastException)
						{
							unitCst = 0.0;
						}
					}

					try
					{
						ap.UnitCost = unitCst;
					}

					catch (Exception Ex)
					{
						editError = true;

						if (Ex.GetType() != typeof(MIDException))
						{
							em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
						}
						else
						{
							MIDException MIDEx = (MIDException)Ex;

							//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
							//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
							em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
							//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
						}
					}

					// ==================
					// Process size group
					// ==================
					if (hdrTran.SizeGroupName == "" || hdrTran.SizeGroupName == null)
					{
						ap.SizeGroupRID = Include.UndefinedSizeGroupRID;
					}
					else
					{
						SizeGroupProfile sgp = new SizeGroupProfile(Include.UndefinedSizeGroupRID);

						try
						{
							sgp.ReadSizeGroup(hdrTran.SizeGroupName);

							// begin MID Track # 3818 - correct error checking
							if (sgp.Key == Include.UndefinedSizeGroupRID)
							{
								ap.SizeGroupRID = Include.UndefinedSizeGroupRID;

								msgText = "Allocation Header " + hdrTran.HeaderId;
								msgText += " Size Group Name " + hdrTran.SizeGroupName;
								msgText += " NOT defined" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
							}
							else
							{
								ap.SizeGroupRID = sgp.Key;
							}
						}

						catch (Exception ex)
						{
							em.AddMsg(eMIDMessageLevel.Severe, ex.Message, GetType().Name);
						}
						// end MID Track # 3818 - change query to retrieve additional fields
					}

					// =====================
					// Process bulk multiple
					// =====================
					if (!hdrTran.BulkMultipleSpecified)
					{
						bulkMult = 1;
					}
					else
					{
						try
						{
							bulkMult = Convert.ToInt32(hdrTran.BulkMultiple, CultureInfo.CurrentUICulture);

							if (bulkMult <= 0)
							{
								bulkMult = 1;
							}
						}

						catch (InvalidCastException)
						{
							bulkMult = 1;
						}
					}

					ap.BulkMultiple = bulkMult;

					// ===================
					// Process header type
					// ===================
					typeError = false;

					if (!hdrTran.HeaderTypeSpecified)
					{
						editError = true;

						typeError = true;

						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " has NO Header Type" + System.Environment.NewLine;
						em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
					}
					else
					{
                        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28b
                        //if (hdrTran.HeaderType.ToString().ToUpper() == "RECEIPT")
                        //{
                        //    ap.ASN = false;
                        //    ap.IsDummy = false;
                        //    ap.Reserve = false;
                        //    ap.DropShip = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    ap.Receipt = true;
                        //}
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "PO")
                        //{
                        //    ap.ASN = false;
                        //    ap.IsDummy = false;
                        //    ap.Receipt = false;
                        //    ap.Reserve = false;
                        //    ap.DropShip = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    ap.IsPurchaseOrder = true;
                        //}
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "ASN")
                        //{
                        //    ap.IsDummy = false;
                        //    ap.Receipt = false;
                        //    ap.Reserve = false;
                        //    ap.DropShip = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    ap.ASN = true;
                        //}
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "DUMMY")
                        //{
                        //    ap.ASN = false;
                        //    ap.Receipt = false;
                        //    ap.Reserve = false;
                        //    ap.DropShip = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    ap.IsDummy = true;
                        //}
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "DROPSHIP")
                        //{
                        //    ap.ASN = false;
                        //    ap.IsDummy = false;
                        //    ap.Receipt = false;
                        //    ap.Reserve = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    ap.DropShip = true;
                        //}
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "RESERVE")
                        //{
                        //    ap.ASN = false;
                        //    ap.IsDummy = false;
                        //    ap.Receipt = false;
                        //    ap.DropShip = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    ap.Reserve = true;
                        //}
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "WORKUPTOTALBUY")
                        //{
                        //    ap.ASN = false;
                        //    ap.IsDummy = false;
                        //    ap.Receipt = false;
                        //    ap.Reserve = false;
                        //    ap.DropShip = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                        //    try
                        //    {
                        //        ap.WorkUpTotalBuy = true;
                        //    }

                        //    catch (Exception Ex)
                        //    {
                        //        editError = true;

                        //        typeError = true;

                        //        if (Ex.GetType() != typeof(MIDException))
                        //        {
                        //            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                        //        }
                        //        else
                        //        {
                        //            MIDException MIDEx = (MIDException)Ex;

                        //            //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                        //            //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                        //            em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                        //            //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                        //        }
                        //    }
                        //}
                        //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                        //else if (hdrTran.HeaderType.ToString().ToUpper() == "VSW")
                        //{
                        //    ap.ASN = false;
                        //    ap.IsDummy = false;
                        //    ap.Receipt = false;
                        //    ap.DropShip = false;
                        //    ap.WorkUpTotalBuy = false;
                        //    ap.IsPurchaseOrder = false;
                        //    ap.Reserve = false;

                        //    ap.IMO = true;	
                        //}
                        //// END TT#1401 - stodd - add resevation stores (IMO)
                        //else
                        //{
                        //    editError = true;

                        //    typeError = true;

                        //    msgText = "Allocation Header " + hdrTran.HeaderId;
                        //    msgText += " has an invalid Header Type [" + hdrTran.HeaderType + "]" + System.Environment.NewLine;
                        //    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                        //}
                        string vsw_id = hdrTran.VSWID;
                        string strHeaderType = hdrTran.HeaderType.ToString().ToUpper();
                        // begin TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                        bool vsw_AdjustOnHand = false;
						// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
						if (hdrTran.VSWProcess == HeadersHeaderVSWProcess.Adjust)
						{
							vsw_AdjustOnHand = true;
						}
						else
						{
							vsw_AdjustOnHand = false;
						}
						// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
                        // end TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                        string typeMessage;
                        eMIDMessageLevel messageLevel;
                        if (!SetHeaderType(hdrTran.HeaderId, ap, strHeaderType, vsw_id, vsw_AdjustOnHand, out messageLevel, out typeMessage)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                        {
                            editError = true;
                            typeError = true;
                            em.AddMsg(messageLevel, typeMessage, GetType().Name);
                        }
                        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
					}

					// ===================
					// Process total units
					// ===================
					if (!typeError)
					{
						if (!hdrTran.TotalUnitsSpecified)
						{
							totQty = 0;
						}
						else
						{
							try
							{
								totQty = Convert.ToInt32(hdrTran.TotalUnits, CultureInfo.CurrentUICulture);

								if (totQty < 0)
								{
									totQty = 0;
								}
							}

							catch (InvalidCastException)
							{
								totQty = 0;
							}
						}

                        // begin TT#368 Do Proportional Spread when Work Up Buy changes total quantity
                        //if (ap.WorkUpTotalBuy)
                        //{
                        //    if (totQty != 0)
                        //    {
                        //        editError = true;

                        //        msgText = "Allocation Header " + hdrTran.HeaderId;
                        //        msgText += " Total Units must be = 0 when Header Type 'WorkUpTotalBuy'" + System.Environment.NewLine;
                        //        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                        //    }
                        //        // begin MID Track 4404 Work Up Buy Force Cancel on Zero Units To Allocate
                        //    else
                        //    {
                        //        ap.Action(
                        //            eAllocationMethodType.BackoutAllocation,
                        //            new GeneralComponent(eGeneralComponentType.Total),
                        //            0.0d, 
                        //            Include.AllStoreFilterRID,
                        //            true);
                        //    }
                        //    // end MID Track 4404 Work Up Buy Force Cancel on Zero Units To Allocate
                        //}
                        //else
                        //{
                        // end TT#368 Do Proportional Spread when Work Up Buy changes total quantity
							if (totQty > 0)
							{
								try
								{
									ap.TotalUnitsToAllocate = totQty;
								}

								catch (Exception Ex)
								{
									editError = true;

									if (Ex.GetType() != typeof(MIDException))
									{
										em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
									}
									else
									{
										MIDException MIDEx = (MIDException)Ex;

										//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
										em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
										//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									}
								}
							}
                            // begin TT#368 Do Proportional Spread when Work Up Buy changes Total Quantity
                            else if (ap.WorkUpTotalBuy)
                            {
                                if (totQty == 0)
                                {
                                    // Begin TT#4322 - JSmith - Creating Work Up Buy header causes database error when parent of style is not on hierarchy
                                    // Nothing to back out on a header create
                                    //ap.Action(
                                    //    eAllocationMethodType.BackoutAllocation,
                                    //    new GeneralComponent(eGeneralComponentType.Total),
                                    //    0.0d, 
                                    //    Include.AllStoreFilterRID,
                                    //    true);
                                    // End TT#4322 - JSmith - Creating Work Up Buy header causes database error when parent of style is not on hierarchy
                                }
                                else
                                {
                                    editError = true;
        							msgText = "Allocation Header " + hdrTran.HeaderId;
		    						msgText += " Total Units must be non-negative for a work up total buy" + System.Environment.NewLine;
			    					em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                }
                            }
                            // end TT#368 Do Proportional Spread when Work Up Buy changes Total Quantity
							else 
							{
								editError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderTotalsUnitsGT0);
                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                msgText = msgText + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
    
								//msgText = "Allocation Header " + hdrTran.HeaderId;
								//msgText += " Total Units must be > 0" + System.Environment.NewLine;
								//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

							}
                        //}   TT#368 Do Proportional Spread when Work Up Buy changes Total Quantity
					}

                    // begin TT#1401 - JEllis - Urban Virutal Store Warehouse pt 28B
                    //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                    //// ===========================
                    //// Process IMO ID
                    //// ===========================
                    //if (hdrTran.VSWID == "" || hdrTran.VSWID == null)
                    //{
                    //    ap.ImoID = null;
                    //    if (ap.HeaderType == eHeaderType.IMO)
                    //    {
                    //        editError = true;
					
                    //        msgText = MIDText.GetText(eMIDTextCode.msg_al_IMOIdRequired);
                    //        msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                    //        msgText = msgText + System.Environment.NewLine;
                    //        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                    //    }
                    //}
                    //else
                    //{
                    //    if (ap.HeaderType != eHeaderType.IMO)
                    //    {
                    //        msgText = MIDText.GetText(eMIDTextCode.msg_al_IMOIdWarning);
                    //        msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                    //        msgText = msgText + System.Environment.NewLine;
                    //        em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
                    //        ap.ImoID = null;
                    //    }
                    //    else
                    //    {
                    //        if (!_SAB.StoreServerSession.DoesIMOExist(hdrTran.VSWID))
                    //        {
                    //            editError = true;

                    //            msgText = MIDText.GetText(eMIDTextCode.msg_al_IMOIdNotFound);
                    //            msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                    //            msgText = msgText + System.Environment.NewLine;
                    //            em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                    //        }
                    //        else
                    //        {
                    //            ap.ImoID = hdrTran.VSWID;
                    //        }
                    //    }
                    //}
                    //// END TT#1401 - stodd - add resevation stores (IMO)
                    // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28

					// ===========================
					// Process distribution center
					// ===========================
					if (hdrTran.DistCenter == "" || hdrTran.DistCenter == null)
					{
						ap.DistributionCenter = null;
					}
					else
					{
						ap.DistributionCenter = hdrTran.DistCenter;
					}

					// ==============
					// Process vendor
					// ==============
					if (hdrTran.Vendor == "" || hdrTran.Vendor == null)
					{
						ap.Vendor = null;
					}
					else
					{
						ap.Vendor = hdrTran.Vendor;
					}

					// ======================
					// Process purchase order
					// ======================
					if (hdrTran.PurchaseOrder == "" || hdrTran.PurchaseOrder == null)
					{
						ap.PurchaseOrder = null;
					}
					else
					{
						ap.PurchaseOrder = hdrTran.PurchaseOrder;
					}

					// ================
					// Process workflow
					// ================
                    // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
                    if (hdrTran.Workflow == "" || hdrTran.Workflow == null)
                    {
                        // Begin TT#3220 - JSmith - PROD - Issue while header create\modify
                        generateWorkflow = true;
                        //ap.WorkflowTrigger = false;
                        //ap.WorkflowRID = Include.UndefinedWorkflowRID;
                        //QuickCreateWorkflowSteps(ap, hdrTran, ref em);
                        // End TT#3220 - JSmith - PROD - Issue while header create\modify
                    }
                    else
                    {
                        if (_methodsDefined)
                        {
                            //---Workflow Specified (Error, both a Workflow & Method(s) defined at this point)
                            ap.WorkflowTrigger = false;
                            ap.WorkflowRID = Include.UndefinedWorkflowRID;
                            ap.API_WorkflowRID = ap.WorkflowRID;
                            ap.API_WorkflowTrigger = ap.WorkflowTrigger;

                            msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderHasWorkflowAndMethods);
                            msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                            msgText = msgText.Replace("{1}", hdrTran.Workflow);
                            msgText = msgText + System.Environment.NewLine;
                            em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        }
                        else
                        {
                            WorkflowBaseData workflowBase = new WorkflowBaseData();

                            //Begin TT#1764 - DOConnell - Duplicate Workflow Naming Convention
                            //DataTable dtWorkflow = workflowBase.GetWorkflow(hdrTran.Workflow)
                            DataTable dtWorkflow = workflowBase.GetWorkflow(hdrTran.Workflow, Include.GetGlobalUserRID());
                            //End TT#1764 - DOConnell - Duplicate Workflow Naming Convention

                            if (dtWorkflow.Rows.Count > 0)
                            {
                                DataRow drWorkflow = dtWorkflow.Rows[0];

                                ap.WorkflowRID = Convert.ToInt32(drWorkflow["WORKFLOW_RID"], CultureInfo.CurrentUICulture);

                                if (!hdrTran.WorkflowTriggerSpecified)
                                {
                                    ap.WorkflowTrigger = false;
                                }
                                else
                                {
                                    try
                                    {
                                        workFlowTrigger = Convert.ToBoolean(hdrTran.WorkflowTrigger, CultureInfo.CurrentUICulture);

                                        ap.WorkflowTrigger = workFlowTrigger;
                                    }

                                    catch (InvalidCastException)
                                    {
                                        ap.WorkflowTrigger = false;
                                    }
                                }
                            }
                            else
                            {
                                ap.WorkflowTrigger = false;
                                ap.WorkflowRID = Include.UndefinedWorkflowRID;

                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderHasInvalidMethodName);
                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                msgText = msgText.Replace("{1}", hdrTran.Workflow);
                                msgText = msgText + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
                            }
                        }
                    }
                    // END MID Track #6336

					ap.API_WorkflowRID = ap.WorkflowRID;
					ap.API_WorkflowTrigger = ap.WorkflowTrigger;

					// =============
					// Process notes
					// =============
					if (hdrTran.Notes == "" || hdrTran.Notes == null)
					{
						ap.AllocationNotes = null;
					}
					else
					{
						ap.AllocationNotes = hdrTran.Notes;
					}

                    // Begin TT#1652-MD - RMatelic - DC Carton Rounding
					// Begin TT#1703-MD - stodd - Error when Units Per Carton field is blank
                    if (hdrTran.UnitsPerCarton != null)
                    {
                        int unitsPerCarton = 0;
                        if (ValidUnitsPerCarton(ap, hdrTran.UnitsPerCarton, ref em, ref unitsPerCarton))   // Validate error may be Warning only
                        {
                            ap.UnitsPerCarton = unitsPerCarton;
                        }
                        else
                        {
                            editError = true;
                        }
                    }
					// End TT#1703-MD - stodd - Error when Units Per Carton field is blank
                    // End TT#1652-MD

					// =============
					// Process packs
					// =============
                    // Begin TT#368 - RMatelic - Allow WorkupTotalBuy colors and sizes - unrelated - disallow packs on WorkupBuy
                    if (hdrTran.Pack != null && ap.WorkUpTotalBuy)
                    {
                        editError = true;

                        packError = true;

                        msgText += string.Format(MIDText.GetText(eMIDTextCode.msg_al_PackNotAllowedOnWorkupBuy), hdrTran.HeaderId) + System.Environment.NewLine;
                        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                    }
                    //if (hdrTran.Pack != null) foreach (HeadersHeaderPack hdrTranPack in hdrTran.Pack)
                    if (!packError && hdrTran.Pack != null)
                    {
                        foreach (HeadersHeaderPack hdrTranPack in hdrTran.Pack)
                        // End TT#368 
                        {
                            packQty = 0;
                            packMult = 0;

                            packError = false;

                            packGeneric = true;

                            // ==================
                            // Add pack to header
                            // ==================
                            if (hdrTranPack.Name == "" || hdrTranPack.Name == null)
                            {
                                editError = true;

                                packError = true;

                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " has missing Pack Name" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                            }

                            if (!hdrTranPack.IsGenericSpecified)
                            {
                                packGeneric = true;
                            }
                            else
                            {
                                try
                                {
                                    packGeneric = Convert.ToBoolean(hdrTranPack.IsGeneric, CultureInfo.CurrentUICulture);
                                }

                                catch (InvalidCastException)
                                {
                                    packGeneric = true;
                                }
                            }

                            try
                            {
                                packQty = Convert.ToInt32(hdrTranPack.Packs, CultureInfo.CurrentUICulture);

                                if (packQty < 0)
                                {
                                    packQty = 0;
                                }
                            }

                            catch (InvalidCastException)
                            {
                                packQty = 0;
                            }

                            if (packQty <= 0)
                            {
                                editError = true;

                                packError = true;

                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " Pack " + hdrTranPack.Name;
                                msgText += " Packs must be > 0" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                            }

                            try
                            {
                                packMult = Convert.ToInt32(hdrTranPack.Multiple, CultureInfo.CurrentUICulture);

                                if (packMult < 0)
                                {
                                    packMult = 0;
                                }
                            }

                            catch (InvalidCastException)
                            {
                                packMult = 0;
                            }

                            if (packMult <= 0)
                            {
                                editError = true;

                                packError = true;

                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " Pack " + hdrTranPack.Name;
                                msgText += " Multiple must be > 0" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                            }

                            if (!packError)
                            {
                                try
                                {
                                    ap.AddPack(hdrTranPack.Name, (packGeneric) ? eAllocationType.GenericType : eAllocationType.DetailType, packMult, packQty, -1);
                                }

                                catch (Exception Ex)
                                {
                                    editError = true;

                                    packError = true;

                                    if (Ex.GetType() != typeof(MIDException))
                                    {
                                        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                    }
                                    else
                                    {
                                        MIDException MIDEx = (MIDException)Ex;

										//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
										em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
										//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									}
                                }
                            }

                            if (!packError)
                            {
                                // ===================
                                // Process pack colors
                                // ===================
                                if (hdrTranPack.PackColor != null) foreach (HeadersHeaderPackPackColor hdrTranPackColor in hdrTranPack.PackColor)
                                    {
                                        colrQty = 0;

                                        packColorError = false;

                                        // ================
                                        // Does color exist
                                        // ================
                                        if (hdrTranPackColor.ColorCodeID == "" || hdrTranPackColor.ColorCodeID == null)
                                        {
                                            editError = true;

                                            packColorError = true;

                                            msgText = "Allocation Header " + hdrTran.HeaderId;
                                            msgText += " Pack " + hdrTranPack.Name;
                                            msgText += " has missing Color Code ID" + System.Environment.NewLine;
                                            em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                        }
                                        else
                                        {
                                            ccp = ValidateColor(hdrTranPackColor.ColorCodeID,
                                                                hdrTranPackColor.ColorCodeName,
                                                                hdrTranPackColor.ColorCodeGroup,
                                                                ref em);

                                            if (ccp.Key == Include.NoRID)
                                            {
                                                editError = true;

                                                packColorError = true;

                                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                                msgText += " Pack " + hdrTranPack.Name;
                                                msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                msgText += " NOT added to color table" + System.Environment.NewLine;
                                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                            }
                                            else if (ap.StyleHnRID != Include.NoRID)
                                            {
                                                // ======================
                                                // Add color to hierarchy
                                                // ======================
												if (hdrTranPackColor.ColorCodeDescription == "" || hdrTranPackColor.ColorCodeDescription == null)
												{
													//Begin TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
													//hdrTranPackColor.ColorCodeDescription = hdrTranPackColor.ColorCodeID;
													hdrTranPackColor.ColorCodeDescription = ccp.ColorCodeName;
													//End TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
												}

                                                colorHnRID = QuickAddColor(ap.StyleHnRID,
                                                                           hdrTranPackColor.ColorCodeID,
                                                                           hdrTranPackColor.ColorCodeDescription,
                                                                           ref em);

                                                if (colorHnRID == Include.NoRID)
                                                {
                                                    editError = true;

                                                    packColorError = true;

                                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                                    msgText += " Pack " + hdrTranPack.Name;
                                                    msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                    msgText += " NOT added to hierarchy" + System.Environment.NewLine;
                                                    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                }
                                            }
                                        }

                                        if (!packColorError)
                                        {
                                            // =================
                                            // Add color to pack
                                            // =================
                                            try
                                            {
                                                colrQty = Convert.ToInt32(hdrTranPackColor.Units, CultureInfo.CurrentUICulture);

                                                if (colrQty < 0)
                                                {
                                                    colrQty = 0;
                                                }
                                            }

                                            catch (InvalidCastException)
                                            {
                                                colrQty = 0;
                                            }

                                            if (colrQty <= 0)
                                            {
                                                editError = true;

                                                packColorError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderColorUnitsGT0);
                                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                                msgText = msgText.Replace("{1}", hdrTranPack.Name);
                                                msgText = msgText.Replace("{2}", hdrTranPackColor.ColorCodeID);
                                                msgText = msgText + System.Environment.NewLine;
                                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

                                                //msgText = "Allocation Header " + hdrTran.HeaderId;
                                                //msgText += " Pack " + hdrTranPack.Name;
                                                //msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                //msgText += " Units must be > 0" + System.Environment.NewLine;
                                                //em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

                                            }
                                            else
                                            {
                                                try
                                                {
                                                    ap.AddColorToPack(hdrTranPack.Name, ccp.Key, colrQty, 0);
                                                }

                                                catch (Exception Ex)
                                                {
                                                    editError = true;

                                                    packColorError = true;

                                                    if (Ex.GetType() != typeof(MIDException))
                                                    {
                                                        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                                    }
                                                    else
                                                    {
                                                        MIDException MIDEx = (MIDException)Ex;

														//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
														//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
														em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
														//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													}
                                                }
                                            }
                                        }

                                        if (!packColorError)
                                        {
                                            // ========================
                                            // Process pack color sizes
                                            // ========================
                                            if (hdrTranPackColor.PackColorSize != null) foreach (HeadersHeaderPackPackColorPackColorSize hdrTranPackColorSize in hdrTranPackColor.PackColorSize)
                                                {
                                                    sizeQty = 0;

                                                    packColorSizeError = false;

                                                    // ===============
                                                    // Does size exist
                                                    // ===============
                                                    if (hdrTranPackColorSize.SizeCodeID == "" || hdrTranPackColorSize.SizeCodeID == null)
                                                    {
                                                        editError = true;

                                                        packColorSizeError = true;

                                                        msgText = "Allocation Header " + hdrTran.HeaderId;
                                                        msgText += " Pack " + hdrTranPack.Name;
                                                        msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                        msgText += " has missing Size Code ID" + System.Environment.NewLine;
                                                        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                    }
                                                    else
                                                    {
                                                        scp = ValidateSize(hdrTranPackColorSize.SizeCodeID,
                                                                           hdrTranPackColorSize.SizeCodeName,
                                                                           hdrTranPackColorSize.SizeCodePrimary,
                                                                           hdrTranPackColorSize.SizeCodeSecondary,
                                                                           hdrTranPackColorSize.SizeCodeProductCategory,
                                                            //															   hdrTranPackColorSize.SizeCodeTableName,
                                                            //															   hdrTranPackColorSize.SizeCodeHeading1,
                                                            //															   hdrTranPackColorSize.SizeCodeHeading2,
                                                            //															   hdrTranPackColorSize.SizeCodeHeading3,
                                                            //															   hdrTranPackColorSize.SizeCodeHeading4,
                                                                           ref em);

                                                        if (scp.Key == Include.NoRID)
                                                        {
                                                            editError = true;

                                                            packColorSizeError = true;

                                                            msgText = "Allocation Header " + hdrTran.HeaderId;
                                                            msgText += " Pack " + hdrTranPack.Name;
                                                            msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                            msgText += " Size Code ID " + hdrTranPackColorSize.SizeCodeID;
                                                            msgText += " NOT added to size table" + System.Environment.NewLine;
                                                            em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                        }
                                                        else if (colorHnRID != Include.NoRID)
                                                        {
                                                            // =====================
                                                            // Add size to hierarchy
                                                            // =====================
                                                            if (hdrTranPackColorSize.SizeCodeDescription == "" || hdrTranPackColorSize.SizeCodeDescription == null)
                                                            {
                                                                hdrTranPackColorSize.SizeCodeDescription = null;
                                                            }

                                                            sizeHnRID = QuickAddSize(colorHnRID,
                                                                                     hdrTranPackColorSize.SizeCodeID,
                                                                                     hdrTranPackColorSize.SizeCodeDescription,
                                                                                     ref em);

                                                            if (sizeHnRID == Include.NoRID)
                                                            {
                                                                editError = true;

                                                                packColorSizeError = true;

                                                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                                                msgText += " Pack " + hdrTranPack.Name;
                                                                msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                                msgText += " Size Code ID " + hdrTranPackColorSize.SizeCodeID;
                                                                msgText += " NOT added to hierarchy" + System.Environment.NewLine;
                                                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                            }
                                                        }
                                                    }

                                                    if (!packColorSizeError)
                                                    {
                                                        // ======================
                                                        // Add size to pack color
                                                        // ======================
                                                        try
                                                        {
                                                            sizeQty = Convert.ToInt32(hdrTranPackColorSize.Units, CultureInfo.CurrentUICulture);

                                                            if (sizeQty < 0)
                                                            {
                                                                sizeQty = 0;
                                                            }
                                                        }

                                                        catch (InvalidCastException)
                                                        {
                                                            sizeQty = 0;
                                                        }

                                                        if (sizeQty <= 0)
                                                        {
                                                            editError = true;

                                                            packColorSizeError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                            msgText = MIDText.GetText(eMIDTextCode.msg_hl_Header3ColorSizeUnitsGT0);
                                                            msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                                            msgText = msgText.Replace("{1}", hdrTranPack.Name);
                                                            msgText = msgText.Replace("{2}", hdrTranPackColor.ColorCodeID);
                                                            msgText = msgText.Replace("{3}", hdrTranPackColorSize.SizeCodeID);
                                                            msgText = msgText + System.Environment.NewLine;
                                                            em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

                                                            //msgText = "Allocation Header " + hdrTran.HeaderId;
                                                            //msgText += " Pack " + hdrTranPack.Name;
                                                            //msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
                                                            //msgText += " Size Code ID " + hdrTranPackColorSize.SizeCodeID;
                                                            //msgText += " Units must be > 0" + System.Environment.NewLine;
                                                            //em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

                                                       }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                // BEGIN MID Track #3634 - display sizes in order added
                                                                //ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, 0);
                                                                ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, -1);
                                                                // END MID Track #3634
                                                            }
                                                            catch (Exception Ex)
                                                            {
                                                                editError = true;

                                                                packColorSizeError = true;

                                                                if (Ex.GetType() != typeof(MIDException))
                                                                {
                                                                    em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                                                }
                                                                else
                                                                {
                                                                    MIDException MIDEx = (MIDException)Ex;

																	//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
																	//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
																	em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
																	//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
																}
                                                            }
                                                        }
                                                    }
                                                }
                                        }
                                    }
                            }

                            if (!packError)
                            {
                                // ==================
                                // Process pack sizes
                                // ==================
                                if (hdrTranPack.PackSize != null)
                                {
                                    packSizeError = false;
                                    // Begin TT#2035 - JSmith - Add color/size edit for VSW headers with packs
                                    if (hdrTran.HeaderType.ToString().ToUpper() == "VSW")
                                    {
                                        editError = true;
                                        packSizeError = true;
                                        msgText = "Allocation Header " + hdrTran.HeaderId;
                                        msgText += " Pack " + hdrTranPack.Name;
                                        msgText += " Color required to add sizes to packs in VSW header" + System.Environment.NewLine;
                                        em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                    }
                                    else
                                    {
                                    // End TT#2035 - JSmith - Add color/size edit for VSW headers with packs
                                        ccp = ValidateColor(Include.DummyColorID,
                                                            Include.DummyColorID,
                                                            Include.DummyColorID,
                                                            ref em);

                                        if (ccp.Key == Include.NoRID)
                                        {
                                            editError = true;

                                            packSizeError = true;

                                            msgText = "Allocation Header " + hdrTran.HeaderId;
                                            msgText += " Pack " + hdrTranPack.Name;
                                            msgText += " Color Code ID " + Include.DummyColorID;
                                            msgText += " NOT added to color table" + System.Environment.NewLine;
                                            em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                        }
                                        else if (ap.StyleHnRID != Include.NoRID)
                                        {
                                            // ======================
                                            // Add color to hierarchy
                                            // ======================
                                            colorHnRID = QuickAddColor(ap.StyleHnRID,
                                                                       Include.DummyColorID,
                                                                       Include.DummyColorID,
                                                                       ref em);

                                            if (colorHnRID == Include.NoRID)
                                            {
                                                editError = true;

                                                packSizeError = true;

                                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                                msgText += " Pack " + hdrTranPack.Name;
                                                msgText += " Color Code ID " + Include.DummyColorID;
                                                msgText += " NOT added to hierarchy" + System.Environment.NewLine;
                                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                            }
                                        }

                                        if (!packSizeError)
                                        {
                                            foreach (HeadersHeaderPackPackSize hdrTranPackSize in hdrTranPack.PackSize)
                                            {
                                                sizeQty = 0;

                                                packSizeError = false;

                                                // ===============
                                                // Does size exist
                                                // ===============
                                                if (hdrTranPackSize.SizeCodeID == "" || hdrTranPackSize.SizeCodeID == null)
                                                {
                                                    editError = true;

                                                    packSizeError = true;

                                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                                    msgText += " Pack " + hdrTranPack.Name;
                                                    msgText += " has missing Size Code ID" + System.Environment.NewLine;
                                                    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                }
                                                else
                                                {
                                                    scp = ValidateSize(hdrTranPackSize.SizeCodeID,
                                                                       hdrTranPackSize.SizeCodeName,
                                                                       hdrTranPackSize.SizeCodePrimary,
                                                                       hdrTranPackSize.SizeCodeSecondary,
                                                                       hdrTranPackSize.SizeCodeProductCategory,
                                                        //															   hdrTranPackSize.SizeCodeTableName,
                                                        //															   hdrTranPackSize.SizeCodeHeading1,
                                                        //															   hdrTranPackSize.SizeCodeHeading2,
                                                        //															   hdrTranPackSize.SizeCodeHeading3,
                                                        //															   hdrTranPackSize.SizeCodeHeading4,
                                                                       ref em);

                                                    if (scp.Key == Include.NoRID)
                                                    {
                                                        editError = true;

                                                        packSizeError = true;

                                                        msgText = "Allocation Header " + hdrTran.HeaderId;
                                                        msgText += " Pack " + hdrTranPack.Name;
                                                        msgText += " Size Code ID " + hdrTranPackSize.SizeCodeID;
                                                        msgText += " NOT added to size table" + System.Environment.NewLine;
                                                        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                    }
                                                    else if (colorHnRID != Include.NoRID)
                                                    {
                                                        // =====================
                                                        // Add size to hierarchy
                                                        // =====================
                                                        if (hdrTranPackSize.SizeCodeDescription == "" || hdrTranPackSize.SizeCodeDescription == null)
                                                        {
                                                            hdrTranPackSize.SizeCodeDescription = null;
                                                        }

                                                        sizeHnRID = QuickAddSize(colorHnRID,
                                                                                 hdrTranPackSize.SizeCodeID,
                                                                                 hdrTranPackSize.SizeCodeDescription,
                                                                                 ref em);

                                                        if (sizeHnRID == Include.NoRID)
                                                        {
                                                            editError = true;

                                                            packSizeError = true;

                                                            msgText = "Allocation Header " + hdrTran.HeaderId;
                                                            msgText += " Pack " + hdrTranPack.Name;
                                                            msgText += " Size Code ID " + hdrTranPackSize.SizeCodeID;
                                                            msgText += " NOT added to hierarchy" + System.Environment.NewLine;
                                                            em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                        }
                                                    }
                                                }

                                                if (!packSizeError)
                                                {
                                                    // ================
                                                    // Add size to pack
                                                    // ================
                                                    try
                                                    {
                                                        sizeQty = Convert.ToInt32(hdrTranPackSize.Units, CultureInfo.CurrentUICulture);

                                                        if (sizeQty < 0)
                                                        {
                                                            sizeQty = 0;
                                                        }
                                                    }

                                                    catch (InvalidCastException)
                                                    {
                                                        sizeQty = 0;
                                                    }

                                                    if (sizeQty <= 0)
                                                    {
                                                        editError = true;

                                                        packSizeError = true;

                                                        // TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                        msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderSizeUnitsGT0);
                                                        msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                                        msgText = msgText.Replace("{1}", hdrTranPack.Name);
                                                        msgText = msgText.Replace("{2}", hdrTranPackSize.SizeCodeID);
                                                        msgText = msgText + System.Environment.NewLine;
                                                        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

                                                        //msgText = "Allocation Header " + hdrTran.HeaderId;
                                                        //msgText += " Pack " + hdrTranPack.Name;
                                                        //msgText += " Size Code ID " + hdrTranPackSize.SizeCodeID;
                                                        //msgText += " Units must be > 0" + System.Environment.NewLine;
                                                        //em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                                                        // TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            // BEGIN MID Track #3634 - display sizes in order added
                                                            //ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, 0);
                                                            ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, -1);
                                                            // END MID Track #3634
                                                        }

                                                        catch (Exception Ex)
                                                        {
                                                            editError = true;

                                                            packSizeError = true;

                                                            if (Ex.GetType() != typeof(MIDException))
                                                            {
                                                                em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                                            }
                                                            else
                                                            {
                                                                MIDException MIDEx = (MIDException)Ex;

                                                                //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                                                                em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                                                                //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }  // TT#2035 - JSmith - Add color/size edit for VSW headers with packs
                                }
                            }
                        }
                    }  // TT#368 Allow WorkupTotalBuy colors and sizes -

					// ===================
					// Process bulk colors
					// ===================
					if (hdrTran.BulkColor != null) foreach (HeadersHeaderBulkColor hdrTranBulkColor in hdrTran.BulkColor)
					{
						colrQty = 0;

						bulkColorError = false;

						// ================
						// Does color exist
						// ================
						if (hdrTranBulkColor.ColorCodeID == null || hdrTranBulkColor.ColorCodeID == string.Empty)
						{
							editError = true;

							bulkColorError = true;

							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " has missing Color Code ID" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						}
						else
						{

							ccp = ValidateColor(hdrTranBulkColor.ColorCodeID,
												hdrTranBulkColor.ColorCodeName,
												hdrTranBulkColor.ColorCodeGroup,
												ref em);

							if (ccp.Key == Include.NoRID)
							{
								editError = true;

								bulkColorError = true;

								msgText = "Allocation Header " + hdrTran.HeaderId;
								msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
								msgText += " NOT added to color table" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
							}
                            else if (ap.StyleHnRID != Include.NoRID)
							{
								// ======================
								// Add color to hierarchy
								// ======================
								if (hdrTranBulkColor.ColorCodeDescription == "" || hdrTranBulkColor.ColorCodeDescription == null)
								{
									//Begin TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
									//hdrTranBulkColor.ColorCodeDescription = hdrTranBulkColor.ColorCodeID;
									hdrTranBulkColor.ColorCodeDescription = ccp.ColorCodeName;
									//End TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
								}

								colorHnRID = QuickAddColor(ap.StyleHnRID,
														   hdrTranBulkColor.ColorCodeID,
														   hdrTranBulkColor.ColorCodeDescription,
														   ref em);

								if (colorHnRID == Include.NoRID)
								{
									editError = true;

									bulkColorError = true;

									msgText = "Allocation Header " + hdrTran.HeaderId;
									msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
									msgText += " NOT added to hierarchy" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
								}
							}
						}

						if (!bulkColorError)
						{
							// ===================
							// Add color to header
							// ===================
							try
							{
								colrQty = Convert.ToInt32(hdrTranBulkColor.Units, CultureInfo.CurrentUICulture);

								if (colrQty < 0)
								{
									colrQty = 0;
								}
							}

							catch (InvalidCastException)
							{
								colrQty = 0;
							}

                            // Begin TT#368 - RMatelic - Allow WorkupTotalBuy colors and sizes
                            //if (colrQty <= 0)
                            //if (colrQty <= 0 && !ap.WorkUpTotalBuy)
                            if (colrQty < 0
                                || colrQty == 0 && !ap.WorkUpTotalBuy)
                            // End TT#368
							{
								editError = true;

								bulkColorError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_Header2ColorUnitsGT0);
                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                msgText = msgText.Replace("{1}", hdrTranBulkColor.ColorCodeID);
                                msgText = msgText + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
 
								//msgText = "Allocation Header " + hdrTran.HeaderId;
								//msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
								//msgText += " Units must be > 0" + System.Environment.NewLine;
								//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

							}
							else
							{
								try
								{
									ap.AddBulkColor(ccp.Key, colrQty, 0);
								}

								catch (Exception Ex)
								{
									editError = true;

									bulkColorError = true;

									if (Ex.GetType() != typeof(MIDException))
									{
										em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
									}
									else
									{
										MIDException MIDEx = (MIDException)Ex;

										//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
										em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
										//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									}
								}
							}

							if (!bulkColorError)
							{
								// ========================
								// Process bulk color sizes
								// ========================
								if (hdrTranBulkColor.BulkColorSize != null) foreach (HeadersHeaderBulkColorBulkColorSize hdrTranBulkColorSize in hdrTranBulkColor.BulkColorSize)
								{
									sizeQty = 0;

									bulkColorSizeError = false;

									// ===============
									// Does size exist
									// ===============
									if (hdrTranBulkColorSize.SizeCodeID == "" || hdrTranBulkColorSize.SizeCodeID == null)
									{
										editError = true;

										bulkColorSizeError = true;

										msgText = "Allocation Header " + hdrTran.HeaderId;
										msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
										msgText += " has missing Size Code ID" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
									}
									else
									{
										scp = ValidateSize(hdrTranBulkColorSize.SizeCodeID,
														   hdrTranBulkColorSize.SizeCodeName,
														   hdrTranBulkColorSize.SizeCodePrimary,
														   hdrTranBulkColorSize.SizeCodeSecondary,
														   hdrTranBulkColorSize.SizeCodeProductCategory,
//														   hdrTranBulkColorSize.SizeCodeTableName,
//														   hdrTranBulkColorSize.SizeCodeHeading1,
//														   hdrTranBulkColorSize.SizeCodeHeading2,
//														   hdrTranBulkColorSize.SizeCodeHeading3,
//														   hdrTranBulkColorSize.SizeCodeHeading4,
														   ref em);

										if (scp.Key == Include.NoRID)
										{
											editError = true;

											bulkColorSizeError = true;

											msgText = "Allocation Header " + hdrTran.HeaderId;
											msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
											msgText += " Size Code ID " + hdrTranBulkColorSize.SizeCodeID;
											msgText += " NOT added to size table" + System.Environment.NewLine;
											em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
										}
                                        else if (colorHnRID != Include.NoRID)
										{
											// =====================
											// Add size to hierarchy
											// =====================
											if (hdrTranBulkColorSize.SizeCodeDescription == "" || hdrTranBulkColorSize.SizeCodeDescription == null)
											{
												hdrTranBulkColorSize.SizeCodeDescription = null;
											}

											sizeHnRID = QuickAddSize(colorHnRID,
																	 hdrTranBulkColorSize.SizeCodeID,
																	 hdrTranBulkColorSize.SizeCodeDescription,
																	 ref em);

											if (sizeHnRID == Include.NoRID)
											{
												editError = true;

												bulkColorSizeError = true;

												msgText = "Allocation Header " + hdrTran.HeaderId;
												msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
												msgText += " Size Code ID " + hdrTranBulkColorSize.SizeCodeID;
												msgText += " NOT added to hierarchy" + System.Environment.NewLine;
												em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
											}
										}
									}

									if (!bulkColorSizeError)
									{
										// =================
										// Add size to color
										// =================
										try
										{
											sizeQty = Convert.ToInt32(hdrTranBulkColorSize.Units, CultureInfo.CurrentUICulture);

											if (sizeQty < 0)
											{
												sizeQty = 0;
											}
										}

										catch (InvalidCastException)
										{
											sizeQty = 0;
										}

                                        // Begin TT#368 - RMatelic - Allow WorkupTotalBuy colors and sizes
                                        //if (sizeQty <= 0)
                                        //if (sizeQty <= 0 && !ap.WorkUpTotalBuy)
                                        if (sizeQty < 0
                                            || (!ap.WorkUpTotalBuy
                                                && sizeQty == 0))
                                        // End TT#368
										{
											editError = true;

											bulkColorSizeError = true;
  
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                            msgText = MIDText.GetText(eMIDTextCode.msg_hl_Header2ColorSizeUnitsGT0);
                                            msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                            msgText = msgText.Replace("{1}",  hdrTranBulkColor.ColorCodeID);
                                            msgText = msgText.Replace("{2}", hdrTranBulkColorSize.SizeCodeID);
                                            msgText = msgText + System.Environment.NewLine;
                                            em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                                            
                                            //msgText = "Allocation Header " + hdrTran.HeaderId;
											//msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
											//msgText += " Size Code ID " + hdrTranBulkColorSize.SizeCodeID;
											//msgText += " Units must be > 0" + System.Environment.NewLine;
											//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

										}
										else
										{
											try
											{
												// BEGIN MID Track #3634 - display sizes in order added
												//ap.AddBulkSizeToColor(ccp.Key, scp.Key, sizeQty, 0);
												ap.AddBulkSizeToColor(ccp.Key, scp.Key, sizeQty, -1);
												// END MID Track #3634
											}

											catch (Exception Ex)
											{
												editError = true;

												bulkColorSizeError = true;

												if (Ex.GetType() != typeof(MIDException))
												{
													em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
												}
												else
												{
													MIDException MIDEx = (MIDException)Ex;

													//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
													em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
													//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												}
											}
										}
									}
								}
							}
						}
					}

                    // BEGIN TT#5442 - AGallagher - Enable Total Level Multiple Changes via XML
                    if (hdrTran.BulkMultipleSpecified && hdrTran.Pack == null && hdrTran.BulkColor == null)
                    {
                        bulkMult = Convert.ToInt32(hdrTran.BulkMultiple, CultureInfo.CurrentUICulture);
                        ap.AllocationMultiple = bulkMult;
                    }
                    // END TT#5442 - AGallagher - Enable Total Level Multiple Changes via XML

                    // Begin TT#2469 - JSmith - MIssing Header Characteristics on Distro
                    bool workflowTrigger = ap.WorkflowTrigger;
                    ap.WorkflowTrigger = false;
                    // End TT#2469 - JSmith - MIssing Header Characteristics on Distro

					if (!editError)
					{
                        // Begin TT#3220 - JSmith - PROD - Issue while header create\modify
                        if (generateWorkflow)
                        {
                            ap.WorkflowTrigger = false;
                            ap.WorkflowRID = Include.UndefinedWorkflowRID;
                            WorkflowBaseData wbd = new WorkflowBaseData();

                            int workFlowRID = wbd.GetWorkflowRID(hdrTran.HeaderId + "(generated)");
                            if (workFlowRID != Include.NoRID)
                            {
                                QuickModifyWorkflowSteps(ap, hdrTran, aTrans, ref em);
                                ap.API_WorkflowRID = ap.WorkflowRID;
                                ap.API_WorkflowTrigger = ap.WorkflowTrigger;
                            }
                            else
                            {
                                QuickCreateWorkflowSteps(ap, hdrTran, ref em);
                                // Begin TT#3374 - JSmith - PROD - API Workflow not set correctly while creating new header in MID
                                ap.API_WorkflowRID = ap.WorkflowRID;
                                ap.API_WorkflowTrigger = ap.WorkflowTrigger;
                                // End TT#3374 - JSmith - PROD - API Workflow not set correctly while creating new header in MID
                            }
                        }
                        // End TT#3220 - JSmith - PROD - Issue while header create\modify

						ap.IsInterfaced = true;

                        // begin TT#3939 - Urban - Jellis - Group has Received Out of Balance Status after API update
                        //try
                        //{
                        ////BeginTT#1767 - DOConnell - Allocation Header API Error Lock transaction timeout
                        //    //if (ap.WriteHeader())
                        //    if (ap.WriteHeader(true))
                        ////EndTT#1767 - DOConnell - Allocation Header API Error Lock transaction timeout
                        try
                        {
                            bool writeSuccess;
                            if (ap.AssortmentProfile != null
                                && ap.AssortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation)
                            {
                                writeSuccess = ap.AssortmentProfile.WriteHeader(true);
                            }
                            else
                            {
                                writeSuccess = ap.WriteHeader(true);
                            }
                            if (writeSuccess)
                        // end TT#3939 - Jellis - Group has Received Out of Balance Status after API update
							{
                                // begin MID Track 5725: AnF Defect 1713 -- Foreign Key Violation
                                //HeaderEnqueue headerEnqueue = null;  // TT#1185 - Verify ENQ before Update
                                try
                                {
                                    // begin TT#1185 - Verify ENQ before Update
                                    hdrRidList.Clear();
                                    hdrRidList.Add(ap.Key);
                                    if (EnqueueHeader(aTrans, hdrRidList, out exceptMsg))
                                    {
                                        ap.ReReadHeader();
                                        //headerEnqueue = new HeaderEnqueue(aTrans, ap.Key);
                                        // headerEnqueue.EnqueueHeaderRID();
                                        // end TT#1185 - Verify ENQ before Update


                                        if (ap.GetHeaderRID(ap.HeaderID) == ap.Key)
                                        {
                                            // =======================================================
                                            // Characterics processing is done at this point since the
                                            // header RID is NOT assigned until WriteHeader is called
                                            // =======================================================
                                            if (hdrTran.Characteristic != null)
                                            {
                                                foreach (HeadersHeaderCharacteristic hdrTranChars in hdrTran.Characteristic)
                                                {
                                                    try
                                                    {
                                                        // Begin TT#168 - RMatelic - Header characteristics auto add
                                                        //charsRtn = ap.ProcessHeaderLoadCharacteristic(hdrTranChars.Name, hdrTranChars.Value, ap.Key, "CREATE");
                                                        //eHeaderCharType headerCharType = (hdrTranChars.CharType != null) ? (eHeaderCharType)hdrTranChars.CharType : eHeaderCharType.text;
                                                        //bool protectChar = (hdrTranChars.Protect != null) ? hdrTranChars.Protect : true;
                                                        eHeaderCharType headerCharType = (eHeaderCharType)hdrTranChars.CharType;
                                                        bool protectChar = hdrTranChars.Protect;
													//Begin TT#1252 - JScott - Duplicate key in Workspace when opening application
													//charsRtn = ap.ProcessHeaderLoadCharacteristic(hdrTranChars.Name, hdrTranChars.Value, ap.Key, "CREATE", _autoAddCharacteristics, headerCharType, protectChar);
                                                        // End TT#168 
													hdrCharGrpID = hdrTranChars.Name;
													charsRtn = ap.ProcessHeaderLoadCharacteristic(ref hdrCharGrpID, hdrTranChars.Value, ap.Key, "CREATE", _autoAddCharacteristics, headerCharType, protectChar);
													hdrTranChars.Name = hdrCharGrpID;

													//End TT#1252 - JScott - Duplicate key in Workspace when opening application
													// End TT#168 
                                                        if ((eReturnCode)charsRtn != eReturnCode.successful)
                                                        {
                                                            charError = true;

                                                            SetCharacteristicMessages(hdrTran.HeaderId, hdrTranChars, charsRtn, ref em);
                                                        }
                                                    }

                                                    catch (Exception Ex)
                                                    {
                                                        charError = true;

                                                        em.AddMsg(eMIDMessageLevel.Edit, Ex.ToString(), GetType().Name);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            createOkay = false;

                                            exceptMsg = "ap.HeaderID(" + ap.HeaderID.ToString() + ") is not equal to ap.Key(" + ap.Key.ToString() + ")";
                                        }
                                        // begin TT#1185 - Verify ENQ Before Update
                                    }
                                    else
                                    {
                                        em.AddMsg(eMIDMessageLevel.Edit, exceptMsg, GetType().Name);  
                                        createOkay = false;
                                    }
                                    // end TT#1185 - Verify ENQ before Update
                                }
                                catch (Exception Ex)
                                {
                                    createOkay = false;

                                    exceptMsg = Ex.ToString();
                                }
                                finally
                                {
                                    // Begin TT#391-MD - JSmith - Header Load with an API Workflow does not perform the workflow actions.
                                    //DequeueHeaders(aTrans);                  // TT#1185 - Verify ENQ before Update
                                    // End TT#391-MD - JSmith - Header Load with an API Workflow does not perform the workflow actions.
                                    //headerEnqueue.DequeueHeaders();  // TT#1185 - Verify ENQ before Update
                                }
                                // end MID Track 5725: AnF Defect 1713 -- Foreign Key Violation
							}
							else
							{
								createOkay = false;
							}
						}

						catch (Exception Ex)
						{
							createOkay = false;

							exceptMsg = Ex.ToString();
						}
					}
                    // Begin TT#3220 - JSmith - PROD - Issue while header create\modify
                    else
                    {
                        createOkay = false;
                    }
                    // End TT#3220 - JSmith - PROD - Issue while header create\modify

                    // Begin TT#2469 - JSmith - MIssing Header Characteristics on Distro
                    if (createOkay &&
                        workflowTrigger)
                    {
                        ap.Action(eAllocationMethodType.ApplyAPI_Workflow, new GeneralComponent(eGeneralComponentType.Total), 0.0d, Include.NoRID, true);
                    }
                    // End TT#2469 - JSmith - MIssing Header Characteristics on Distro

					// ==========================================================================
					// Since headers are NOT enqueued during CREATE processing there is NO reason
					// to dequeue them
					//
					// hdrEnqueued will be set to "false" to simulate a dequeue and allow logic
					// to flow as it should
					// ==========================================================================
					// ==============
					// Dequeue Header
					// ==============
//					headerEnqueue.DequeueHeaders();

                    //hdrEnqueued = false;  // TT#1185 - Verify Enqueue before update

					if (createOkay)
					{
						if (!editError)
						{
							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " (Transaction #" + _headersRead.ToString() + ")";
							msgText += " successfully created" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

							if (charError)
							{
								msgText = "Allocation Header " + hdrTran.HeaderId;
								msgText += " (Transaction #" + _headersRead.ToString() + ")";
								msgText += " encountered characteristic errors" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
							}

							WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), true);

							rtnCode = eReturnCode.successful;
						}
						else
						{
							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " (Transaction #" + _headersRead.ToString() + ")";
							msgText += " encountered edit errors" + System.Environment.NewLine;
                            // Begin TT#1773 - JSmith - Header Load return information level message after database error.
                            //em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
                            em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            // End TT#1773

							WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

							rtnCode = eReturnCode.editErrors;
						}
					}
					else
					{
						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " (Transaction #" + _headersRead.ToString() + ")";
						if (exceptMsg == null)
						{
							msgText += " encountered an update error" + System.Environment.NewLine;
						}
						else
						{
							msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
						}
                        // Begin TT#1773 - JSmith - Header Load return information level message after database error.
                        //em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
                        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        // End TT#1773

						WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

						rtnCode = eReturnCode.severe;
					}
				}
			}

			catch (Exception Ex)
			{
				WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

				_audit.Log_Exception(Ex, GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
                // begin TT#1185 - Verify ENQ before Update
//                if (hdrEnqueued)
//                {
//                    // ==========================================================================
//                    // Since headers are NOT enqueued during CREATE processing there is NO reason
//                    // to dequeue them
//                    //
//                    // hdrEnqueued will be set to "false" to simulate a dequeue and allow logic
//                    // to flow as it should
//                    // ==========================================================================
////					headerEnqueue.DequeueHeaders();

//                    hdrEnqueued = false;
//                }
                // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                //DequeueHeaders(aTrans);  //  Enqueue could occur during process after header RID is created
				// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                // end TT#1185 - Verify ENQ before Update

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// ======================
		// Modify existing header
		// ======================
		public eReturnCode ModifyHeader(HeadersHeader hdrTran, AllocationProfile ap, ApplicationSessionTransaction aTrans)
		{
			int totQty = 0;
			int colrQty = 0;
			int packQty = 0;
			int sizeQty = 0;
			int bulkMult = 1;
			int charsRtn = 0;
			int packMult = 0;

			EditMsgs em = null;

			double unitCst = 0.0;
			double unitRtl = 0.0;

			string msgText = null;
			string exceptMsg = null;

			bool charError = false;
			bool editError = false;
			bool typeError = false;
			bool modifyOkay = true;
            //bool hdrEnqueued = false;  // TT#1185 - Verify ENQ before Update
			bool workFlowTrigger = false;
            bool bRebuildMaster = false;   // TT#1966-MD - JSmith - DC Fulfillment

			SizeCodeProfile scp = null;

			ColorCodeProfile ccp = null;

			// Begin MID Track #4958 - JSmith - header description using style ID
//			int styleRID = Include.NoRID;
			HierarchyNodeProfile styleHnp = null;
			// End MID Track #4958
			int sizeHnRID = Include.NoRID;
			int colorHnRID = Include.NoRID;

			bool bulkColorError = false;
			bool bulkColorSizeError = false;

			bool packError = false;
			bool packGeneric = true;
			bool packSizeError = false;
			bool packColorError = false;
			bool packColorSizeError = false;

			//HeaderEnqueue headerEnqueue = null; // TT#1185 - Verify ENQ before Update

			eReturnCode rtnCode = eReturnCode.successful;

			//Begin TT#1252 - JScott - Duplicate key in Workspace when opening application
			string hdrCharGrpID;

			//End TT#1252 - JScott - Duplicate key in Workspace when opening application
			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{
				// ==============
				// Enqueue Header
				// ==============
				// Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                //try
                //{
                //    List<int> hdrRidList = new List<int>();
                //    hdrRidList.Add(ap.Key);

                //    //=======================================================================
                //    // Note: If the header being processed belongs to a group allocation,
                //    // all headers in that group allocation will be enqueued.
                //    //=======================================================================
                //    if (EnqueueHeader(aTrans, hdrRidList, out exceptMsg))
                //    {
                //        //// Begin TT#1040 - MD - stodd - header load API for Group Allocation 
                //        //ProfileList ampl = aTrans.GetAssortmentMemberProfileList();
                //        //if (ampl.Count > 0)
                //        //{
                //        //    foreach (AllocationProfile ap1 in ampl.ArrayList)
                //        //    {
                //        //        ap1.ReReadHeader();
                //        //    }
                //        //}
                //        //else
                //        //{
                //        //    ap.ReReadHeader();
                //        //}
                //        //// End TT#1040 - MD - stodd - header load API for Group Allocation 
                //    }
                //    else
                //    {
                //        em.AddMsg(
                //            eMIDMessageLevel.Information,
                //            eMIDTextCode.msg_al_HeaderEnqFailed,
                //            MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed),
                //            GetType().Name);
                //        em.AddMsg(eMIDMessageLevel.Edit, exceptMsg, GetType().Name);
                //        if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                //        {
                //            WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
                //        }
                //        else
                //        {
                //            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                //        }
                //        rtnCode = eReturnCode.warning;
                //    }
                //}
                //catch (Exception Ex)
                //{
                //    DequeueHeaders(aTrans);
                //    if (Ex.GetType() != typeof(MIDException))
                //    {
                //        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                //    }
                //    else
                //    {
                //        MIDException MIDEx = (MIDException)Ex;
                //        em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                //    }
                //    if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                //    {
                //        WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
                //    }
                //    else
                //    {
                //        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                //    }
                //    rtnCode = eReturnCode.severe;
                //}
				// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
//                AllocationHeaderProfile ahp = new AllocationHeaderProfile(ap.HeaderID, ap.Key);

//                AllocationHeaderProfileList ahpList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

//                try
//                {
//                    ahpList.Clear();

//                    ahpList.Add(ahp);

//                    headerEnqueue = new HeaderEnqueue(aTrans, ahpList);

//                    headerEnqueue.EnqueueHeaders();

//                    ap.ReReadHeader();

//                    hdrEnqueued = true;
//                }

//                catch (HeaderConflictException)
//                {
//                    SecurityAdmin secAdmin = new SecurityAdmin();

//                    msgText = "Allocation Header " + ahp.HeaderID + " is currently in use by User(s): ";
//                    foreach (HeaderConflict hc in headerEnqueue.HeaderConflictList)
//                    {
//                        msgText += System.Environment.NewLine + secAdmin.GetUserName(hc.UserRID);
//                    }
//                    msgText += System.Environment.NewLine;
//                    em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);

//// (CSMITH) - BEG: Incorrect action when Modify performed from Reset
//                    if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
//                    {
//                        WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
//                    }
//                    else
//                    {
//                        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
//                    }
//// (CSMITH) - END: Incorrect action when Modify performed from Reset

//                    rtnCode = eReturnCode.warning;
//                }

//                catch (Exception Ex)
//                {
//                    if (Ex.GetType() != typeof(MIDException))
//                    {
//                        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
//                    }
//                    else
//                    {
//                        MIDException MIDEx = (MIDException)Ex;

//                        //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
//                        //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
//                        em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
//                        //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
//                    }

//// (CSMITH) - BEG: Incorrect action when Modify performed from Reset
//                    if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
//                    {
//                        WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
//                    }
//                    else
//                    {
//                        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
//                    }
//// (CSMITH) - END: Incorrect action when Modify performed from Reset

//                    rtnCode = eReturnCode.severe;
//                }
                // end TT#1185 - Verify ENQ before Update

				//if (hdrEnqueued) // TT#1185 - Verify ENQ before Update
                if (_headersEnqueued) // TT#1185 - Verify ENQ before Update
				{
					// =====================
					// Process new header id
					// =====================
                    // Begin TT#712 - RMatelic - Multi Header Release-In Use by Multi Header - add '_allowHeaderDataUpdate'
                    //if (hdrTran.NewHeaderId != null && hdrTran.NewHeaderId.Trim() != string.Empty)
                    if (hdrTran.NewHeaderId != null && hdrTran.NewHeaderId.Trim() != string.Empty && _allowHeaderDataUpdate)
                    // End TT#712
					{
						if (ap.ReleaseApproved || ap.Released)
						{
							editError = true;

							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " attempt to change Allocation Header Id to " + hdrTran.NewHeaderId;
							msgText += " NOT successful - Allocation Header already released" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
						}
						else
						{
							Header hdr = new Header();
							
							hdrTran.NewHeaderId = hdrTran.NewHeaderId.Trim();

							if (!hdr.HeaderExists(hdrTran.NewHeaderId))
							{
								try
								{
									ap.HeaderID = hdrTran.NewHeaderId;
								}

								catch (Exception Ex)
								{
									editError = true;

									if (Ex.GetType() != typeof(MIDException))
									{
										em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
									}
									else
									{
										MIDException MIDEx = (MIDException)Ex;

										//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
										em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
										//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									}
								}
							}
							else
							{
								editError = true;

								msgText = "Allocation Header " + hdrTran.HeaderId;
								msgText += " attempt to change Allocation Header Id to " + hdrTran.NewHeaderId;
								msgText += " NOT successful - Header Id already in use" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
							}
						}
					}

                    // Begin TT#712 - RMatelic - Multi Header Release-In Use by Multi Header - add '_allowHeaderDataUpdate'
                    if (_allowHeaderDataUpdate)
                    {
                        // ==========================
                        // Process header description
                        // ==========================
                        if (hdrTran.HeaderDescription != "" && hdrTran.HeaderDescription != null)
                        {
                            ap.HeaderDescription = hdrTran.HeaderDescription;
                        }

                        // BEGIN TT#5442 - AGallagher - Enable Total Level Multiple Changes via XML
                        if (hdrTran.BulkMultipleSpecified && hdrTran.Pack == null && hdrTran.BulkColor == null)
                        {
                            bulkMult = Convert.ToInt32(hdrTran.BulkMultiple, CultureInfo.CurrentUICulture);
                            ap.AllocationMultiple = bulkMult;
                        }
                        // END TT#5442 - AGallagher - Enable Total Level Multiple Changes via XML

                        // ===================
                        // Process header date
                        // ===================
                        if (hdrTran.HeaderDateSpecified)
                        {
                            if (hdrTran.HeaderDate != DateTime.MinValue)
                            {
                                ap.HeaderDay = hdrTran.HeaderDate;
                            }
                        }

                        // ==============================
                        // Process style and style parent
                        // ==============================
                        if (hdrTran.StyleId != null && hdrTran.StyleId != string.Empty)
                        {
                            if (hdrTran.ParentOfStyleId == null || hdrTran.ParentOfStyleId == string.Empty)
                            {
                                editError = true;

                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " has NO Parent of Style Id" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                            }
                            else
                            {
                                //BEGIN TT#1046 - WUB Header Load failing with System.NullReferenceException: Object reference not set to an instance of an object. - apicchetti - 2/3/2011
                                Header currentHdr = new Header();
                                DataTable dtCurrentHdr = currentHdr.GetHeader(_headerRID);
                                int intStyleHNRid = Convert.ToInt32(dtCurrentHdr.Rows[0]["STYLE_HNRID"]);
                                HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(intStyleHNRid);
                                string current_styleID = hnp.NodeID;

                                if (current_styleID == hdrTran.StyleId)
                                {
                                //END TT#1046 - WUB Header Load failing with System.NullReferenceException: Object reference not set to an instance of an object. - apicchetti - 2/3/2011

                                    // BEGIN MID Track 3595 - Update Style Description  - add HeaderDescription parm 
                                    // BEGIN MID Track 3592 - Update Style Name  - add StyleName parm 
                                    //styleRID = ValidateStyle(hdrTran.HeaderId, hdrTran.StyleId, hdrTran.ParentOfStyleId, ref em);
                                    // Begin MID Track #4958 - JSmith - header description using style ID
                                    //							styleRID = ValidateStyle(hdrTran.HeaderId, hdrTran.StyleId, hdrTran.HeaderDescription, hdrTran.StyleName, hdrTran.ParentOfStyleId, ref em);
                                    styleHnp = ValidateStyle(hdrTran.HeaderId, hdrTran.StyleId, hdrTran.HeaderDescription, hdrTran.StyleName, hdrTran.ParentOfStyleId, ref em);
                                    // END MID Track #3595
                                    // END MID Track #3592

                                    //							if (styleRID == Include.NoRID)
                                    //							{
                                    //								editError = true;
                                    //							}
                                    //
                                    //							try
                                    //							{
                                    //								ap.StyleHnRID = styleRID;
                                    //							}
                                    if (styleHnp.Key == Include.NoRID)
                                    {
                                        editError = true;
                                    }

                                    try
                                    {
                                        ap.StyleHnRID = styleHnp.Key;
                                    }
                                    // End MID Track #4958

                                    catch (Exception Ex)
                                    {
                                        editError = true;

                                        if (Ex.GetType() != typeof(MIDException))
                                        {
                                            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                        }
                                        else
                                        {
                                            MIDException MIDEx = (MIDException)Ex;

                                            //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                            //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                                            em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                                            //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                        }
                                    }
                                
                                //BEGIN TT#1046 - WUB Header Load failing with System.NullReferenceException: Object reference not set to an instance of an object. - apicchetti - 2/3/2011
                                }
                                else
                                {
                                    editError = true;

                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " cannot be moved to another style" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name); //change to edit error!
                                }
                                //END TT#1046 - WUB Header Load failing with System.NullReferenceException: Object reference not set to an instance of an object. - apicchetti - 2/3/2011
                            }
                        }

                        // ==========================
                        // Process header description
                        // ==========================
                        if (ap.HeaderDescription == null)
                        {
                            ap.HeaderDescription = hdrTran.StyleId;
                        }

                        // ===================
                        // Process unit retail
                        // ===================
                        if (hdrTran.UnitRetailSpecified)
                        {
                            try
                            {
                                unitRtl = Convert.ToDouble(hdrTran.UnitRetail, CultureInfo.CurrentUICulture);

                                if (unitRtl < 0.0)
                                {
                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " Unit Retail < 0.00 - current value NOT changed" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                }
                                else
                                {
                                    try
                                    {
                                        ap.UnitRetail = unitRtl;
                                    }

                                    catch (Exception Ex)
                                    {
                                        editError = true;

                                        if (Ex.GetType() != typeof(MIDException))
                                        {
                                            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                        }
                                        else
                                        {
                                            MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
                                    }
                                }
                            }

                            catch (InvalidCastException)
                            {
                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " Unit Retail has invalid value [" + hdrTran.UnitRetail.ToString() + "]";
                                msgText += " - current value NOT changed" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            }
                        }

                        // =================
                        // Process unit cost
                        // =================
                        if (hdrTran.UnitCostSpecified)
                        {
                            try
                            {
                                unitCst = Convert.ToDouble(hdrTran.UnitCost, CultureInfo.CurrentUICulture);

                                if (unitCst < 0.0)
                                {
                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " Unit Cost < 0.00 - current value NOT changed" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                }
                                else
                                {
                                    try
                                    {
                                        ap.UnitCost = unitCst;
                                    }

                                    catch (Exception Ex)
                                    {
                                        editError = true;

                                        if (Ex.GetType() != typeof(MIDException))
                                        {
                                            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                        }
                                        else
                                        {
                                            MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
                                    }
                                }
                            }

                            catch (InvalidCastException)
                            {
                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " Unit Cost has invalid value [" + hdrTran.UnitCost.ToString() + "]";
                                msgText += " - current value NOT changed" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            }
                        }

                        // ==================
                        // Process size group
                        // ==================
                        if (hdrTran.SizeGroupName != "" && hdrTran.SizeGroupName != null)
                        {
                            SizeGroupProfile sgp = new SizeGroupProfile(Include.UndefinedSizeGroupRID);

                            try
                            {
                                sgp.ReadSizeGroup(hdrTran.SizeGroupName);

                                ap.SizeGroupRID = sgp.Key;
                            }

                            catch (Exception)
                            {
                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " Size Group Name " + hdrTran.SizeGroupName;
                                msgText += " NOT defined - current value NOT changed" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
                            }
                        }

                        // =====================
                        // Process bulk multiple
                        // =====================
                        if (hdrTran.BulkMultipleSpecified)
                        {
                            try
                            {
                                bulkMult = Convert.ToInt32(hdrTran.BulkMultiple, CultureInfo.CurrentUICulture);

                                if (bulkMult <= 0)
                                {
                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " Bulk Multiple <= 0 - current value NOT changed" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                }
                                else
                                {
                                    ap.BulkMultiple = bulkMult;
                                }
                            }

                            catch (InvalidCastException)
                            {
                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                msgText += " Bulk Multiple has invalid value [" + hdrTran.BulkMultiple.ToString() + "]";
                                msgText += " - current value NOT changed" + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            }
                        }

                        // ===================
                        // Process header type
                        // ===================
                        typeError = false;

                        if (hdrTran.HeaderTypeSpecified)
                        {
                            // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
                            //if (hdrTran.HeaderType.ToString().ToUpper() == "RECEIPT")
                            //{

                            //    ap.ASN = false;
                            //    ap.IsDummy = false;
                            //    ap.Reserve = false;
                            //    ap.DropShip = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    ap.Receipt = true;
                            //}
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "PO")
                            //{
                            //    ap.ASN = false;
                            //    ap.IsDummy = false;
                            //    ap.Receipt = false;
                            //    ap.Reserve = false;
                            //    ap.DropShip = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    ap.IsPurchaseOrder = true;
                            //}
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "ASN")
                            //{
                            //    ap.IsDummy = false;
                            //    ap.Receipt = false;
                            //    ap.Reserve = false;
                            //    ap.DropShip = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    ap.ASN = true;
                            //}
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "DUMMY")
                            //{
                            //    ap.ASN = false;
                            //    ap.Receipt = false;
                            //    ap.Reserve = false;
                            //    ap.DropShip = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    ap.IsDummy = true;
                            //}
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "DROPSHIP")
                            //{
                            //    ap.ASN = false;
                            //    ap.IsDummy = false;
                            //    ap.Receipt = false;
                            //    ap.Reserve = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    ap.DropShip = true;
                            //}
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "RESERVE")
                            //{
                            //    ap.ASN = false;
                            //    ap.IsDummy = false;
                            //    ap.Receipt = false;
                            //    ap.DropShip = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    ap.Reserve = true;
                            //}
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "WORKUPTOTALBUY")
                            //{
                            //    ap.ASN = false;
                            //    ap.IsDummy = false;
                            //    ap.Receipt = false;
                            //    ap.Reserve = false;
                            //    ap.DropShip = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.IMO = false;		// TT#1401 - stodd - add resevation stores (IMO)

                            //    try
                            //    {
                            //        ap.WorkUpTotalBuy = true;
                            //    }

                            //    catch (Exception Ex)
                            //    {
                            //        editError = true;

                            //        typeError = true;

                            //        if (Ex.GetType() != typeof(MIDException))
                            //        {
                            //            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                            //        }
                            //        else
                            //        {
                            //            MIDException MIDEx = (MIDException)Ex;

                            //            //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                            //            //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                            //            em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                            //            //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                            //        }
                            //    }
                            //}
                            //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                            //else if (hdrTran.HeaderType.ToString().ToUpper() == "VSW")
                            //{
                            //    ap.ASN = false;
                            //    ap.IsDummy = false;
                            //    ap.Receipt = false;
                            //    ap.DropShip = false;
                            //    ap.WorkUpTotalBuy = false;
                            //    ap.IsPurchaseOrder = false;
                            //    ap.Reserve = false;

                            //    ap.IMO = true;
                            //}
                            //// END TT#1401 - stodd - add resevation stores (IMO)
                            //else
                            //{
                            //    editError = true;

                            //    typeError = true;

                            //    msgText = "Allocation Header " + hdrTran.HeaderId;
                            //    msgText += " has an invalid Header Type [" + hdrTran.HeaderType + "]";
                            //    msgText += " - current value NOT changed" + System.Environment.NewLine;
                            //    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                            //}
                            string vsw_id = hdrTran.VSWID;
                            string strHeaderType = hdrTran.HeaderType.ToString().ToUpper();
                            // begin TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                            bool vsw_AdjustOnHand = ap.AdjustVSW_OnHand;
							// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
							if (hdrTran.VSWProcess == HeadersHeaderVSWProcess.Adjust)
							{
								vsw_AdjustOnHand = true;
							}
							else
							{
								vsw_AdjustOnHand = false;
							}
							// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
                            // end TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                            string typeMessage;
                            eMIDMessageLevel messageLevel;
                            // Begin TT#5201 - JSmith - Headers being pulled in as out of balance
                            //if (!SetHeaderType(hdrTran.HeaderId, ap, strHeaderType, vsw_id, vsw_AdjustOnHand, out messageLevel, out typeMessage)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                            //{
                            //    editError = true;
                            //    typeError = true;
                            //    em.AddMsg(messageLevel, typeMessage, GetType().Name);
                            //}
                            bool blHeaderTypeChanged = false;

                            switch (strHeaderType)
                            {
                                case "VSW":
                                    {
                                        if (ap.HeaderType != eHeaderType.IMO)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "RECEIPT":
                                    {
                                        if (ap.HeaderType != eHeaderType.Receipt)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "PO":
                                    {
                                        if (ap.HeaderType != eHeaderType.PurchaseOrder)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "ASN":
                                    {
                                        if (ap.HeaderType != eHeaderType.ASN)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "DUMMY":
                                    {
                                        if (ap.HeaderType != eHeaderType.Dummy)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "DROPSHIP":
                                    {
                                        if (ap.HeaderType != eHeaderType.DropShip)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "RESERVE":
                                    {
                                        if (ap.HeaderType != eHeaderType.Reserve)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                case "WORKUPTOTALBUY":
                                    {
                                        if (ap.HeaderType != eHeaderType.WorkupTotalBuy)
                                        {
                                            blHeaderTypeChanged = true;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }

                            if (blHeaderTypeChanged)
                            {
                                if (!SetHeaderType(hdrTran.HeaderId, ap, strHeaderType, vsw_id, vsw_AdjustOnHand, out messageLevel, out typeMessage)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                {
                                    editError = true;
                                    typeError = true;
                                    em.AddMsg(messageLevel, typeMessage, GetType().Name);
                                }
                            }
                            // End TT#5201 - JSmith - Headers being pulled in as out of balance

                            // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
                        }

                        // ===================
                        // Process total units
                        // ===================
                        if (!typeError)
                        {
                            if (hdrTran.TotalUnitsSpecified)
                            {
                                try
                                {
                                    totQty = Convert.ToInt32(hdrTran.TotalUnits, CultureInfo.CurrentUICulture);

                                    if (totQty < 0)
                                    {
                                        totQty = -1;

                                        msgText = "Allocation Header " + hdrTran.HeaderId;
                                        msgText += " Total Units < 0 - current value NOT changed" + System.Environment.NewLine;
                                        em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                    }
                                }

                                catch (InvalidCastException)
                                {
                                    totQty = -1;

                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                    msgText += " Total Units has invalid value [" + hdrTran.TotalUnits.ToString() + "]";
                                    msgText += " - current value NOT changed" + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                }

                            // begin TT#368 Changes to Work Up Buy units to allocate should be spread
                            //if (ap.WorkUpTotalBuy)
                            //{
                            //    if (totQty != 0)
                            //    {
                            //        editError = true;

                            //        msgText = "Allocation Header " + hdrTran.HeaderId;
                            //        msgText += " Total Units must be = 0 when Header Type 'WorkUpTotalBuy'" + System.Environment.NewLine;
                            //        em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            //    }
                            //        // begin MID Track 4404 Work Up Buy Force Cancel on Zero Units To Allocate
                            //    else
                            //    {
                            //        ap.Action(
                            //            eAllocationMethodType.BackoutAllocation,
                            //            new GeneralComponent(eGeneralComponentType.Total),
                            //            0.0d, 
                            //            Include.AllStoreFilterRID,
                            //            true);
                            //    }
                            //    // end MID Track 4404 Work Up Buy Force Cancel on Zero Units To Allocate
                            //}
                            //else
                            //{
                            // end TT#368 Changes to WOrk Up Buy units to allocate should be spread
							if (totQty > 0)
							{
								try
								{
                                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                                    if (ap.TotalUnitsToAllocate != totQty)
                                    {
                                        bRebuildMaster = true;
                                    }
                                    // End TT#1966-MD - JSmith - DC Fulfillment
									ap.TotalUnitsToAllocate = totQty;
								}

								catch (Exception Ex)
								{
									editError = true;

									if (Ex.GetType() != typeof(MIDException))
									{
										em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
									}
									else
									{
										MIDException MIDEx = (MIDException)Ex;

										//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
										em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
										//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									}
								}
							}
                            // begin TT#368 Do Proportional Spread when Work Up Buy changes Total Quantity
                            else if (ap.WorkUpTotalBuy)
                            {
                                if (totQty == 0)
                                {
                                    ap.Action(
                                        eAllocationMethodType.BackoutAllocation,
                                        new GeneralComponent(eGeneralComponentType.Total),
                                        0.0d, 
                                        Include.AllStoreFilterRID,
                                        true);
                                }
                                else
                                {
                                    editError = true;
    							    msgText = "Allocation Header " + hdrTran.HeaderId;
	    						    msgText += " Total Units must be non-negative for a work up total buy" + System.Environment.NewLine;
		    					    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                }
                            }
                            // end TT#368 Do Proportional Spread when Work Up Buy changes Total Quantity
							else 
							{
								editError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderTotalsUnitsGT0);
                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                msgText = msgText + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

                                //msgText = "Allocation Header " + hdrTran.HeaderId;
                                //msgText += " Total Units must be > 0" + System.Environment.NewLine;
                                //em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

							}
							//}  TT#368 Do Proportional Spread when Work Up Buy changes Total quantity
						}
                        }

                        //// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                        // ===========================
                        // Process IMO ID
                        // ===========================
                        if (hdrTran.VSWID == "" || hdrTran.VSWID == null)
                        {
                        }
                        else
                        {
                            if (ap.HeaderType != eHeaderType.IMO)
                            {
                                msgText = MIDText.GetText(eMIDTextCode.msg_al_IMOIdWarning);
                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                msgText = msgText + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
                            }
                            else
                            {
                                if (!StoreMgmt.DoesIMOExist(hdrTran.VSWID)) //_SAB.StoreServerSession.DoesIMOExist(hdrTran.VSWID))
                                {
                                    editError = true;

                                    msgText = MIDText.GetText(eMIDTextCode.msg_al_IMOIdNotFound);
                                    msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                    msgText = msgText + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                }
                                else
                                {
                                    // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
                                    //ap.ImoID = hdrTran.VSWID;
                                    MIDException midException;
                                    // begin TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    bool vsw_AdjustOnHand = ap.AdjustVSW_OnHand;
									// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
									if (hdrTran.VSWProcess == HeadersHeaderVSWProcess.Adjust)
									{
										vsw_AdjustOnHand = true;
									}
									else
									{
										vsw_AdjustOnHand = false;
									}
									// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
                                    // end TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    if (!ap.SetHeaderType(ap.HeaderType, hdrTran.VSWID, vsw_AdjustOnHand, out midException))  // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        editError = true;
                                        em.AddMsg(Include.TranslateErrorLevel(midException.ErrorLevel), midException.ErrorMessage, GetType().Name);
                                    }
                                    // end TT#t1401 - JEllis - Urban Virtual Store warehouse pt 28B
                                }
                            }
                        }
                        // END TT#1401 - stodd - add resevation stores (IMO)

                        // ===========================
                        // Process distribution center
                        // ===========================
                        if (hdrTran.DistCenter != "" && hdrTran.DistCenter != null)
                        {
                            ap.DistributionCenter = hdrTran.DistCenter;
                        }

                        // ==============
                        // Process vendor
                        // ==============
                        if (hdrTran.Vendor != "" && hdrTran.Vendor != null)
                        {
                            ap.Vendor = hdrTran.Vendor;
                        }

                        // ======================
                        // Process purchase order
                        // ======================
                        if (hdrTran.PurchaseOrder != "" && hdrTran.PurchaseOrder != null)
                        {
                            ap.PurchaseOrder = hdrTran.PurchaseOrder;
                        }

                        // ================
                        // Process workflow
                        // ================
                        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
                        _deleteWorkFlowRID = Include.UndefinedWorkflowRID;
                        // Begin Track #6500 - JSmith - API Workflows Missing 
                        //					if (hdrTran.Workflow == "" || hdrTran.Workflow == null)
                        // Begin TT#4320 - JSmith - We send in a header to allocate and method rule is not running.
                        //if (hdrTran.Workflow == "")
                        //// End Track #6500
                        if (_methodsDefined &&
                            (hdrTran.Workflow == "" || hdrTran.Workflow == null))
                        // End TT#4320 - JSmith - We send in a header to allocate and method rule is not running.
                        {
                            ap.WorkflowTrigger = false;
                            ap.WorkflowRID = Include.UndefinedWorkflowRID;
                            QuickModifyWorkflowSteps(ap, hdrTran, aTrans, ref em);
                            ap.API_WorkflowRID = ap.WorkflowRID;
                            ap.API_WorkflowTrigger = ap.WorkflowTrigger;
                        }
                        else
                        {
                            // Begin TT#2182 - JSmith - MDO Cluster clearance information header fails on MID
                            //if (_methodsDefined)
                            if (_methodsDefined &&
                                hdrTran.Workflow != null)
                            // End TT#2182
                            {
                                //---Workflow Specified (Error, both a Workflow & Method(s) defined at this point)
                                ap.WorkflowTrigger = false;
                                ap.WorkflowRID = Include.UndefinedWorkflowRID;
                                ap.API_WorkflowRID = ap.WorkflowRID;
                                ap.API_WorkflowTrigger = ap.WorkflowTrigger;

                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderHasWorkflowAndMethods);
                                msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                msgText = msgText.Replace("{1}", hdrTran.Workflow);
                                msgText = msgText + System.Environment.NewLine;
                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                            }
                            // Begin TT#4320 - JSmith - We send in a header to allocate and method rule is not running.
                            else if (hdrTran.Workflow == "")
                            {
                                ap.WorkflowTrigger = false;
                                ap.WorkflowRID = Include.UndefinedWorkflowRID;
                                ap.API_WorkflowTrigger = false;
                                ap.API_WorkflowRID = Include.UndefinedWorkflowRID;
                            }
                            // End TT#4320 - JSmith - We send in a header to allocate and method rule is not running.
                            // Begin Track #6500 - JSmith - API Workflows Missing
                            //						else 
                            else if (hdrTran.Workflow != null)
                            // End Track #6500
                            {
                                WorkflowBaseData workflowBase = new WorkflowBaseData();

                                //Begin TT#1764 - DOConnell - Duplicate Workflow Naming Convention
                                //DataTable dtWorkflow = workflowBase.GetWorkflow(hdrTran.Workflow)
                                DataTable dtWorkflow = workflowBase.GetWorkflow(hdrTran.Workflow, Include.GetGlobalUserRID());
                                //End TT#1764 - DOConnell - Duplicate Workflow Naming Convention

                                if (dtWorkflow.Rows.Count > 0)
                                {
                                    DataRow drWorkflow = dtWorkflow.Rows[0];

                                    ap.WorkflowRID = Convert.ToInt32(drWorkflow["WORKFLOW_RID"], CultureInfo.CurrentUICulture);

                                    ap.API_WorkflowRID = ap.WorkflowRID;

                                    if (!hdrTran.WorkflowTriggerSpecified)
                                    {
                                        ap.WorkflowTrigger = false;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            workFlowTrigger = Convert.ToBoolean(hdrTran.WorkflowTrigger, CultureInfo.CurrentUICulture);

                                            ap.WorkflowTrigger = workFlowTrigger;
                                        }

                                        catch (InvalidCastException)
                                        {
                                            ap.WorkflowTrigger = false;
                                        }
                                    }

                                    ap.API_WorkflowTrigger = ap.WorkflowTrigger;
                                }
                                else
                                {
                                    ap.WorkflowTrigger = false;
                                    ap.WorkflowRID = Include.UndefinedWorkflowRID;

                                    msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderHasInvalidMethodName);
                                    msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                    msgText = msgText.Replace("{1}", hdrTran.Workflow);
                                    msgText = msgText + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
                                }
                            }
                        }
                        // END MID Track #6336
                    }   
                    // End TT#712

					// =============
					// Process notes
					// =============
					if (hdrTran.Notes != "" && hdrTran.Notes != null)
					{
						ap.AllocationNotes = hdrTran.Notes;
					}
                    
                    // Begin TT#1652-MD - RMatelic - DC Carton Rounding
					// Begin TT#1703-MD - stodd - Error when Units Per Carton field is blank
                    if (hdrTran.UnitsPerCarton != null)
                    {
                        int unitsPerCarton = 0;
                        if (ValidUnitsPerCarton(ap, hdrTran.UnitsPerCarton, ref em, ref unitsPerCarton))   // Validate error may be Warning only
                        {
                            ap.UnitsPerCarton = unitsPerCarton;
                        }
                        else
                        {
                            editError = true;
                        }
                    }
					// End TT#1703-MD - stodd - Error when Units Per Carton field is blank
                    // End TT#1652-MD

					// =============
					// Process packs
					// =============
                    // Begin TT#712 - RMatelic - Multi Header Release-In Use by Multi Header - add '_allowHeaderDataUpdate'
                    //if (hdrTran.Pack != null) foreach (HeadersHeaderPack hdrTranPack in hdrTran.Pack)
                    if (hdrTran.Pack != null && _allowHeaderDataUpdate) foreach (HeadersHeaderPack hdrTranPack in hdrTran.Pack)
                    // End TT#712  
					{
						packQty = 0;
						packMult = 0;

						packError = false;

						if (hdrTranPack.Name == "" || hdrTranPack.Name == null)
						{
							editError = true;

							packError = true;

							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " has missing Pack Name" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
						}

						if (!packError)
						{
							if (!ap.PackIsOnHeader(hdrTranPack.Name))
							{
								// ==================
								// Add pack to header
								// ==================
								if (!hdrTranPack.IsGenericSpecified)
								{
									packGeneric = true;
								}
								else
								{
									try
									{
										packGeneric = Convert.ToBoolean(hdrTranPack.IsGeneric, CultureInfo.CurrentUICulture);
									}

									catch (InvalidCastException)
									{
										packGeneric = true;
									}
								}

								try
								{
									packQty = Convert.ToInt32(hdrTranPack.Packs, CultureInfo.CurrentUICulture);

									if (packQty < 0)
									{
										packQty = 0;
									}
								}

								catch (InvalidCastException)
								{
									packQty = 0;
								}

								if (packQty <= 0)
								{
									editError = true;

									packError = true;

									msgText = "Allocation Header " + hdrTran.HeaderId;
									msgText += " Pack " + hdrTranPack.Name;
									msgText += " Packs must be > 0" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
								}

								try
								{
									packMult = Convert.ToInt32(hdrTranPack.Multiple, CultureInfo.CurrentUICulture);

									if (packMult < 0)
									{
										packMult = 0;
									}
								}

								catch (InvalidCastException)
								{
									packMult = 0;
								}

								if (packMult <= 0)
								{
									editError = true;

									packError = true;

									msgText = "Allocation Header " + hdrTran.HeaderId;
									msgText += " Pack " + hdrTranPack.Name;
									msgText += " Multiple must be > 0" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
								}

								if (!packError)
								{
									try
									{
										ap.AddPack(hdrTranPack.Name, (packGeneric) ? eAllocationType.GenericType : eAllocationType.DetailType, packMult, packQty, -1);
                                        bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
									}

									catch (Exception Ex)
									{
										editError = true;

										packError = true;

										if (Ex.GetType() != typeof(MIDException))
										{
											em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
										}
										else
										{
											MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
									}
								}
							}
							else
							{
								// ===============================
								// Modify or Remove pack on header
								// ===============================
								try
								{
									packQty = Convert.ToInt32(hdrTranPack.Packs, CultureInfo.CurrentUICulture);

									if (packQty < 0)
									{
										packQty = -1;
									}
								}

								catch (InvalidCastException)
								{
									packQty = -1;
								}

								try
								{
									packMult = Convert.ToInt32(hdrTranPack.Multiple, CultureInfo.CurrentUICulture);

									if (packMult < 0)
									{
										packMult = -1;
									}
								}

								catch (InvalidCastException)
								{
									packMult = -1;
								}

								if (packQty == 0 && packMult == 0)
								{
									// =======================
									// Remove pack from header
									// =======================
									try
									{
										ap.RemovePack(hdrTranPack.Name);
                                        bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
									}

									catch (Exception Ex)
									{
										editError = true;

										packError = true;

										if (Ex.GetType() != typeof(MIDException))
										{
											em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
										}
										else
										{
											MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
									}
								}
								else
								{
									// =====================
									// Modify pack on header
									// =====================
									if (hdrTranPack.IsGenericSpecified)
									{
										try
										{
											packGeneric = Convert.ToBoolean(hdrTranPack.IsGeneric, CultureInfo.CurrentUICulture);

											try
											{
												ap.SetPackGeneric(hdrTranPack.Name, packGeneric);
											}

											catch (Exception Ex)
											{
												editError = true;

												packError = true;

												if (Ex.GetType() != typeof(MIDException))
												{
													em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
												}
												else
												{
													MIDException MIDEx = (MIDException)Ex;

													//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
													em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
													//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												}
											}
										}

										catch (InvalidCastException)
										{
										}
									}

									if (packQty > 0)
									{
										try
										{
                                            // Begin TT#1966-MD - JSmith - DC Fulfillment
                                            if (ap.GetPacksToAllocate(hdrTranPack.Name) != packQty)
                                            {
                                                bRebuildMaster = true;
                                            }
                                            // End TT#1966-MD - JSmith - DC Fulfillment

											ap.SetPacksToAllocate(hdrTranPack.Name, packQty);
										}

										catch (Exception Ex)
										{
											editError = true;

											packError = true;

											if (Ex.GetType() != typeof(MIDException))
											{
												em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
											}
											else
											{
												MIDException MIDEx = (MIDException)Ex;

												//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
												em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
												//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											}
										}
									}

									if (packMult > 0)
									{
										try
										{
											ap.SetPackMultiple(hdrTranPack.Name, packMult);
										}

										catch (Exception Ex)
										{
											editError = true;

											packError = true;

											if (Ex.GetType() != typeof(MIDException))
											{
												em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
											}
											else
											{
												MIDException MIDEx = (MIDException)Ex;

												//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
												em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
												//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											}
										}
									}
								}
							}

							if (!packError)
							{
								// ===================
								// Process pack colors
								// ===================
								if (hdrTranPack.PackColor != null) foreach (HeadersHeaderPackPackColor hdrTranPackColor in hdrTranPack.PackColor)
								{
									colrQty = 0;

									packColorError = false;

									// ================
									// Does color exist
									// ================
									if (hdrTranPackColor.ColorCodeID == "" || hdrTranPackColor.ColorCodeID == null)
									{
										editError = true;

										packColorError = true;

										msgText = "Allocation Header " + hdrTran.HeaderId;
										msgText += " Pack " + hdrTranPack.Name;
										msgText += " has missing Color Code ID" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
									}
									else
									{
										ccp = ValidateColor(hdrTranPackColor.ColorCodeID,
															hdrTranPackColor.ColorCodeName,
															hdrTranPackColor.ColorCodeGroup,
															ref em);

										if (ccp.Key == Include.NoRID)
										{
											editError = true;

											packColorError = true;

											msgText = "Allocation Header " + hdrTran.HeaderId;
											msgText += " Pack " + hdrTranPack.Name;
											msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
											msgText += " NOT added to color table" + System.Environment.NewLine;
											em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
										}
                                        else if (ap.StyleHnRID != Include.NoRID)
										{
											// ======================
											// Add color to hierarchy
											// ======================
											if (hdrTranPackColor.ColorCodeDescription == "" || hdrTranPackColor.ColorCodeDescription == null)
											{
												//Begin TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
												//hdrTranPackColor.ColorCodeDescription = hdrTranPackColor.ColorCodeID;
												hdrTranPackColor.ColorCodeDescription = ccp.ColorCodeName;
												//End TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
											}

											colorHnRID = QuickAddColor(ap.StyleHnRID,
																	   hdrTranPackColor.ColorCodeID,
																	   hdrTranPackColor.ColorCodeDescription,
																	   ref em);

											if (colorHnRID == Include.NoRID)
											{
												editError = true;

												packColorError = true;

												msgText = "Allocation Header " + hdrTran.HeaderId;
												msgText += " Pack " + hdrTranPack.Name;
												msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
												msgText += " NOT added to hierarchy" + System.Environment.NewLine;
												em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
											}
										}
									}

									if (!packColorError)
									{
										if (!ap.ColorIsOnPack(hdrTranPack.Name, ccp.Key))
										{
											// =================
											// Add color to pack
											// =================
											try
											{
												colrQty = Convert.ToInt32(hdrTranPackColor.Units, CultureInfo.CurrentUICulture);

												if (colrQty < 0)
												{
													colrQty = 0;
												}
											}

											catch (InvalidCastException)
											{
												colrQty = 0;
											}

											if (colrQty <= 0)
											{
												editError = true;

												packColorError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderColorUnitsGT0);
                                                msgText = msgText.Replace("{0}",hdrTran.HeaderId);
                                                msgText = msgText.Replace("{1}", hdrTranPack.Name);
                                                msgText = msgText.Replace("{2}",hdrTranPackColor.ColorCodeID);
                                                msgText = msgText + System.Environment.NewLine;
                                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

												//msgText = "Allocation Header " + hdrTran.HeaderId;
												//msgText += " Pack " + hdrTranPack.Name;
												//msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
												//msgText += " Units must be > 0" + System.Environment.NewLine;
												//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

											}
											else
											{
												try
												{
													ap.AddColorToPack(hdrTranPack.Name, ccp.Key, colrQty, 0);
												}

												catch (Exception Ex)
												{
													editError = true;

													packColorError = true;

													if (Ex.GetType() != typeof(MIDException))
													{
														em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
													}
													else
													{
														MIDException MIDEx = (MIDException)Ex;

														//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
														//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
														em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
														//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													}
												}
											}
										}
										else
										{
											// ==============================
											// Modify or Remove color on pack
											// ==============================
											try
											{
												colrQty = Convert.ToInt32(hdrTranPackColor.Units, CultureInfo.CurrentUICulture);

												if (colrQty < 0)
												{
													colrQty = -1;
												}
											}

											catch (InvalidCastException)
											{
												colrQty = -1;
											}

											if (colrQty == 0)
											{
												// ======================
												// Remove color from pack
												// ======================
												try
												{
													ap.RemovePackColor(hdrTranPack.Name, ccp.Key);
												}

												catch (Exception Ex)
												{
													editError = true;

													packColorError = true;

													if (Ex.GetType() != typeof(MIDException))
													{
														em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
													}
													else
													{
														MIDException MIDEx = (MIDException)Ex;

														//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
														//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
														em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
														//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													}
												}
											}
											else
											{
												if (colrQty > 0)
												{
													// ====================
													// Modify color on pack
													// ====================
													try
													{
                                                        // Begin TT#1512 - RMatelic - InTransit Zero'd Out w\When Header Characteristic is modified
                                                        //ap.SetColorUnitsInPack(hdrTranPack.Name, ccp.Key, colrQty);
                                                        if (ap.GetColorUnitsInPack(hdrTranPack.Name, ccp.Key) != colrQty)
                                                        {
                                                            ap.SetColorUnitsInPack(hdrTranPack.Name, ccp.Key, colrQty);
                                                        }
                                                        // End TT#1502
													}

													catch (Exception Ex)
													{
														editError = true;

														packColorError = true;

														if (Ex.GetType() != typeof(MIDException))
														{
															em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
														}
														else
														{
															MIDException MIDEx = (MIDException)Ex;

															//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
															//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
															em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
															//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
														}
													}
												}
											}
										}
									}

									if (!packColorError)
									{
										// ========================
										// Process pack color sizes
										// ========================
										if (hdrTranPackColor.PackColorSize != null) foreach (HeadersHeaderPackPackColorPackColorSize hdrTranPackColorSize in hdrTranPackColor.PackColorSize)
										{
											sizeQty = 0;

											packColorSizeError = false;

											// ===============
											// Does size exist
											// ===============
											if (hdrTranPackColorSize.SizeCodeID == "" || hdrTranPackColorSize.SizeCodeID == null)
											{
												editError = true;

												packColorSizeError = true;

												msgText = "Allocation Header " + hdrTran.HeaderId;
												msgText += " Pack " + hdrTranPack.Name;
												msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
												msgText += " has missing Size Code ID" + System.Environment.NewLine;
												em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
											}
											else
											{
												scp = ValidateSize(hdrTranPackColorSize.SizeCodeID,
																   hdrTranPackColorSize.SizeCodeName,
																   hdrTranPackColorSize.SizeCodePrimary,
																   hdrTranPackColorSize.SizeCodeSecondary,
																   hdrTranPackColorSize.SizeCodeProductCategory,
//																   hdrTranPackColorSize.SizeCodeTableName,
//																   hdrTranPackColorSize.SizeCodeHeading1,
//																   hdrTranPackColorSize.SizeCodeHeading2,
//																   hdrTranPackColorSize.SizeCodeHeading3,
//																   hdrTranPackColorSize.SizeCodeHeading4,
																   ref em);

												if (scp.Key == Include.NoRID)
												{
													editError = true;

													packColorSizeError = true;

													msgText = "Allocation Header " + hdrTran.HeaderId;
													msgText += " Pack " + hdrTranPack.Name;
													msgText += " Color Code Id " + hdrTranPackColor.ColorCodeID;
													msgText += " Size Code ID " + hdrTranPackColorSize.SizeCodeID;
													msgText += " NOT added to size table" + System.Environment.NewLine;
													em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
												}
                                                else if (colorHnRID != Include.NoRID)
												{
													// =====================
													// Add size to hierarchy
													// =====================
													if (hdrTranPackColorSize.SizeCodeDescription == "" || hdrTranPackColorSize.SizeCodeDescription == null)
													{
														hdrTranPackColorSize.SizeCodeDescription = null;
													}

													sizeHnRID = QuickAddSize(colorHnRID,
																			 hdrTranPackColorSize.SizeCodeID,
																			 hdrTranPackColorSize.SizeCodeDescription,
																			 ref em);

													if (sizeHnRID == Include.NoRID)
													{
														editError = true;

														packColorSizeError = true;

														msgText = "Allocation Header " + hdrTran.HeaderId;
														msgText += " Pack " + hdrTranPack.Name;
														msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
														msgText += " Size Code ID " + hdrTranPackColorSize.SizeCodeID;
														msgText += " NOT added to hierarchy" + System.Environment.NewLine;
														em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
													}
												}
											}

											if (!packColorSizeError)
											{
												if (!ap.SizeIsOnPackColor(hdrTranPack.Name, ccp.Key, scp.Key))
												{
													// ======================
													// Add size to pack color
													// ======================
													try
													{
														sizeQty = Convert.ToInt32(hdrTranPackColorSize.Units, CultureInfo.CurrentUICulture);

														if (sizeQty < 0)
														{
															sizeQty = 0;
														}
													}

													catch (InvalidCastException)
													{
														sizeQty = 0;
													}

													if (sizeQty <= 0)
													{
														editError = true;

														packColorSizeError = true;

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                        msgText = MIDText.GetText(eMIDTextCode.msg_hl_Header3ColorSizeUnitsGT0);
                                                        msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                                        msgText = msgText.Replace("{1}", hdrTranPack.Name);
                                                        msgText = msgText.Replace("{2}", hdrTranPackColor.ColorCodeID);
                                                        msgText = msgText.Replace("{3}", hdrTranPackColorSize.SizeCodeID);
                                                        msgText = msgText + System.Environment.NewLine;
                                                        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

														//msgText = "Allocation Header " + hdrTran.HeaderId;
                                                        //msgText += " Pack " + hdrTranPack.Name;
														//msgText += " Color Code ID " + hdrTranPackColor.ColorCodeID;
														//msgText += " Size Code ID " + hdrTranPackColorSize.SizeCodeID;
														//msgText += " Units must be > 0" + System.Environment.NewLine;
														//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

													}
													else
													{
														try
														{
															// BEGIN MID Track #3634 - display sizes in order added
															//ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, 0);
															ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, -1);
															// END MID Track #3634
														}

														catch (Exception Ex)
														{
															editError = true;

															packColorSizeError = true;

															if (Ex.GetType() != typeof(MIDException))
															{
																em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
															}
															else
															{
																MIDException MIDEx = (MIDException)Ex;

																//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
																//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
																em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
																//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
															}
														}
													}
												}
												else
												{
													// ===================================
													// Modify or Remove size on pack color
													// ===================================
													try
													{
														sizeQty = Convert.ToInt32(hdrTranPackColorSize.Units, CultureInfo.CurrentUICulture);

														if (sizeQty < 0)
														{
															sizeQty = -1;
														}
													}

													catch (InvalidCastException)
													{
														sizeQty = -1;
													}

													if (sizeQty == 0)
													{
														// ===========================
														// Remove size from pack color
														// ===========================
														try
														{
															ap.RemovePackColorSize(hdrTranPack.Name, ccp.Key, scp.Key);
														}

														catch (Exception Ex)
														{
															editError = true;

															packColorSizeError = true;

															if (Ex.GetType() != typeof(MIDException))
															{
																em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
															}
															else
															{
																MIDException MIDEx = (MIDException)Ex;

																//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
																//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
																em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
																//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
															}
														}
													}
													else
													{
														if (sizeQty > 0)
														{
															// =========================
															// Modify size on pack color
															// =========================
															try
															{
                                                                // Begin TT#1512 - RMatelic - InTransit Zero'd Out w\When Header Characteristic is modified
                                                                //ap.SetPackColorSizeUnits(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty);
                                                                if (ap.GetPackColorSizeUnits(hdrTranPack.Name, ccp.Key, scp.Key) != sizeQty)
                                                                {
                                                                    ap.SetPackColorSizeUnits(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty);
                                                                }
                                                                // End TT#1512
															}

															catch (Exception Ex)
															{
																editError = true;

																packColorSizeError = true;

																if (Ex.GetType() != typeof(MIDException))
																{
																	em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
																}
																else
																{
																	MIDException MIDEx = (MIDException)Ex;

																	//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
																	//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
																	em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
																	//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}

							if (!packError)
							{
								// ==================
								// Process pack sizes
								// ==================
								if (hdrTranPack.PackSize != null)
								{
                                    // Begin TT#2035 - JSmith - Add color/size edit for VSW headers with packs
                                    if (hdrTran.HeaderType.ToString().ToUpper() == "VSW")
                                    {
                                        editError = true;
                                        packSizeError = true;
                                        msgText = "Allocation Header " + hdrTran.HeaderId;
                                        msgText += " Pack " + hdrTranPack.Name;
                                        msgText += " Color required to add sizes to packs in VSW header" + System.Environment.NewLine;
                                        em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                                    }
                                    else
                                    {
                                        // End TT#2035 - JSmith - Add color/size edit for VSW headers with packs

                                        ccp = ValidateColor(Include.DummyColorID,
                                                            Include.DummyColorID,
                                                            Include.DummyColorID,
                                                            ref em);

                                        if (ccp.Key == Include.NoRID)
                                        {
                                            editError = true;

                                            packSizeError = true;

                                            msgText = "Allocation Header " + hdrTran.HeaderId;
                                            msgText += " Pack " + hdrTranPack.Name;
                                            msgText += " Color Code ID " + Include.DummyColorID;
                                            msgText += " NOT added to color table" + System.Environment.NewLine;
                                            em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                        }
                                        else if (ap.StyleHnRID != Include.NoRID)
                                        {
                                            // ======================
                                            // Add color to hierarchy
                                            // ======================
                                            colorHnRID = QuickAddColor(ap.StyleHnRID,
                                                                        Include.DummyColorID,
                                                                        Include.DummyColorID,
                                                                        ref em);

                                            if (colorHnRID == Include.NoRID)
                                            {
                                                editError = true;

                                                packSizeError = true;

                                                msgText = "Allocation Header " + hdrTran.HeaderId;
                                                msgText += " Pack " + hdrTranPack.Name;
                                                msgText += " Color Code ID " + Include.DummyColorID;
                                                msgText += " NOT added to hierarchy" + System.Environment.NewLine;
                                                em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                            }
                                        }


                                        if (!packSizeError)
                                        {
                                            foreach (HeadersHeaderPackPackSize hdrTranPackSize in hdrTranPack.PackSize)
                                            {
                                                sizeQty = 0;

                                                packSizeError = false;

                                                // ===============
                                                // Does size exist
                                                // ===============
                                                if (hdrTranPackSize.SizeCodeID == "" || hdrTranPackSize.SizeCodeID == null)
                                                {
                                                    editError = true;

                                                    packSizeError = true;

                                                    msgText = "Allocation Header " + hdrTran.HeaderId;
                                                    msgText += " Pack " + hdrTranPack.Name;
                                                    msgText += " has missing Size Code ID" + System.Environment.NewLine;
                                                    em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                }
                                                else
                                                {
                                                    scp = ValidateSize(hdrTranPackSize.SizeCodeID,
                                                                        hdrTranPackSize.SizeCodeName,
                                                                        hdrTranPackSize.SizeCodePrimary,
                                                                        hdrTranPackSize.SizeCodeSecondary,
                                                                        hdrTranPackSize.SizeCodeProductCategory,
                                                        //																   hdrTranPackSize.SizeCodeTableName,
                                                        //																   hdrTranPackSize.SizeCodeHeading1,
                                                        //																   hdrTranPackSize.SizeCodeHeading2,
                                                        //																   hdrTranPackSize.SizeCodeHeading3,
                                                        //																   hdrTranPackSize.SizeCodeHeading4,
                                                                        ref em);

                                                    if (scp.Key == Include.NoRID)
                                                    {
                                                        editError = true;

                                                        packSizeError = true;

                                                        msgText = "Allocation Header " + hdrTran.HeaderId;
                                                        msgText += " Pack " + hdrTranPack.Name;
                                                        msgText += " Size Code ID " + hdrTranPackSize.SizeCodeID;
                                                        msgText += " NOT added to size table" + System.Environment.NewLine;
                                                        em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                    }
                                                    else if (colorHnRID != Include.NoRID)
                                                    {
                                                        // =====================
                                                        // Add size to hierarchy
                                                        // =====================
                                                        if (hdrTranPackSize.SizeCodeDescription == "" || hdrTranPackSize.SizeCodeDescription == null)
                                                        {
                                                            hdrTranPackSize.SizeCodeDescription = null;
                                                        }

                                                        sizeHnRID = QuickAddSize(colorHnRID,
                                                                                    hdrTranPackSize.SizeCodeID,
                                                                                    hdrTranPackSize.SizeCodeDescription,
                                                                                    ref em);

                                                        if (sizeHnRID == Include.NoRID)
                                                        {
                                                            editError = true;

                                                            packSizeError = true;

                                                            msgText = "Allocation Header " + hdrTran.HeaderId;
                                                            msgText += " Pack " + hdrTranPack.Name;
                                                            msgText += " Size Code ID " + hdrTranPackSize.SizeCodeID;
                                                            msgText += " NOT added to hierarchy" + System.Environment.NewLine;
                                                            em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                                        }
                                                    }
                                                }

                                                if (!packSizeError)
                                                {
                                                    if (!ap.SizeIsOnPackColor(hdrTranPack.Name, ccp.Key, scp.Key))
                                                    {
                                                        // ======================
                                                        // Add size to pack color
                                                        // ======================
                                                        try
                                                        {
                                                            sizeQty = Convert.ToInt32(hdrTranPackSize.Units, CultureInfo.CurrentUICulture);

                                                            if (sizeQty < 0)
                                                            {
                                                                sizeQty = 0;
                                                            }
                                                        }

                                                        catch (InvalidCastException)
                                                        {
                                                            sizeQty = 0;
                                                        }

                                                        if (sizeQty <= 0)
                                                        {
                                                            editError = true;

                                                            packSizeError = true;

                                                            // TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                                            msgText = MIDText.GetText(eMIDTextCode.msg_hl_HeaderSizeUnitsGT0);
                                                            msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                                            msgText = msgText.Replace("{1}", hdrTranPack.Name);
                                                            msgText = msgText.Replace("{2}", hdrTranPackSize.SizeCodeID);
                                                            msgText = msgText + System.Environment.NewLine;
                                                            em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

                                                            //msgText = "Allocation Header " + hdrTran.HeaderId;
                                                            //msgText += " Pack " + hdrTranPack.Name;
                                                            //msgText += " Size Code ID " + hdrTranPackSize.SizeCodeID;
                                                            //msgText += " Units must be > 0" + System.Environment.NewLine;
                                                            //em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);                                                       
                                                            // TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

                                                        }
                                                        else
                                                        {
                                                            try
                                                            {
                                                                // BEGIN MID Track #3634 - display sizes in order added
                                                                //ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, 0);
                                                                ap.AddSizeToPackColor(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty, -1);
                                                                // END MID Track #3634
                                                            }

                                                            catch (Exception Ex)
                                                            {
                                                                editError = true;

                                                                packSizeError = true;

                                                                if (Ex.GetType() != typeof(MIDException))
                                                                {
                                                                    em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                                                }
                                                                else
                                                                {
                                                                    MIDException MIDEx = (MIDException)Ex;

                                                                    //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                    //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                                                                    em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                                                                    //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // ===================================
                                                        // Modify or Remove size on pack color
                                                        // ===================================
                                                        try
                                                        {
                                                            sizeQty = Convert.ToInt32(hdrTranPackSize.Units, CultureInfo.CurrentUICulture);

                                                            if (sizeQty < 0)
                                                            {
                                                                sizeQty = -1;
                                                            }
                                                        }

                                                        catch (InvalidCastException)
                                                        {
                                                            sizeQty = -1;
                                                        }

                                                        if (sizeQty == 0)
                                                        {
                                                            // ===========================
                                                            // Remove size from pack color
                                                            // ===========================
                                                            try
                                                            {
                                                                ap.RemovePackColorSize(hdrTranPack.Name, ccp.Key, scp.Key);
                                                            }

                                                            catch (Exception Ex)
                                                            {
                                                                editError = true;

                                                                packSizeError = true;

                                                                if (Ex.GetType() != typeof(MIDException))
                                                                {
                                                                    em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                                                }
                                                                else
                                                                {
                                                                    MIDException MIDEx = (MIDException)Ex;

                                                                    //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                    //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                                                                    em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                                                                    //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (sizeQty > 0)
                                                            {
                                                                // =========================
                                                                // Modify size on pack color
                                                                // =========================
                                                                try
                                                                {
                                                                    // Begin TT#1512 - RMatelic - InTransit Zero'd Out w\When Header Characteristic is modified
                                                                    //ap.SetPackColorSizeUnits(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty);
                                                                    if (ap.GetPackColorSizeUnits(hdrTranPack.Name, ccp.Key, scp.Key) != sizeQty)
                                                                    {
                                                                        ap.SetPackColorSizeUnits(hdrTranPack.Name, ccp.Key, scp.Key, sizeQty);
                                                                    }
                                                                    // End TT#1512
                                                                }

                                                                catch (Exception Ex)
                                                                {
                                                                    editError = true;

                                                                    packSizeError = true;

                                                                    if (Ex.GetType() != typeof(MIDException))
                                                                    {
                                                                        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                                                                    }
                                                                    else
                                                                    {
                                                                        MIDException MIDEx = (MIDException)Ex;

                                                                        //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                        //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                                                                        em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                                                                        //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }  // TT#2035 - JSmith - Add color/size edit for VSW headers with packs
                                }
                            }
                        }
                    }

					// ===================
					// Process bulk colors
					// ===================
                    // Begin TT#712 - RMatelic - Multi Header Release-In Use by Multi Header - add '_allowHeaderDataUpdate'
                    //if (hdrTran.BulkColor != null) foreach (HeadersHeaderBulkColor hdrTranBulkColor in hdrTran.BulkColor)
                    if (hdrTran.BulkColor != null && _allowHeaderDataUpdate) foreach (HeadersHeaderBulkColor hdrTranBulkColor in hdrTran.BulkColor)
                    // End TT#712 
					{
						colrQty = 0;

						bulkColorError = false;

						// ================
						// Does color exist
						// ================
						if (hdrTranBulkColor.ColorCodeID == "" || hdrTranBulkColor.ColorCodeID == null)
						{
							editError = true;

							bulkColorError = true;

							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " has missing Color Code ID" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						}
						else
						{
							ccp = ValidateColor(hdrTranBulkColor.ColorCodeID,
												hdrTranBulkColor.ColorCodeName,
												hdrTranBulkColor.ColorCodeGroup,
												ref em);

							if (ccp.Key == Include.NoRID)
							{
								editError = true;

								bulkColorError = true;

								msgText = "Allocation Header " + hdrTran.HeaderId;
								msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
								msgText += " NOT added to color table" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
							}
                            else if (ap.StyleHnRID != Include.NoRID)
							{
								// ======================
								// Add color to hierarchy
								// ======================
								if (hdrTranBulkColor.ColorCodeDescription == "" || hdrTranBulkColor.ColorCodeDescription == null)
								{
									//Begin TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
									//hdrTranBulkColor.ColorCodeDescription = hdrTranBulkColor.ColorCodeID;
									hdrTranBulkColor.ColorCodeDescription = ccp.ColorCodeName;
									//End TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
								}

								colorHnRID = QuickAddColor(ap.StyleHnRID,
														   hdrTranBulkColor.ColorCodeID,
														   hdrTranBulkColor.ColorCodeDescription,
														   ref em);

								if (colorHnRID == Include.NoRID)
								{
									editError = true;

									bulkColorError = true;

									msgText = "Allocation Header " + hdrTran.HeaderId;
									msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
									msgText += " NOT added to hierarchy" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
								}
							}
						}

						if (!bulkColorError)
						{
							if (!ap.BulkColorIsOnHeader(ccp.Key))
							{
								// ===================
								// Add color to header
								// ===================
								try
								{
									colrQty = Convert.ToInt32(hdrTranBulkColor.Units, CultureInfo.CurrentUICulture);

									if (colrQty < 0)
									{
										colrQty = 0;
									}
								}

								catch (InvalidCastException)
								{
									colrQty = 0;
								}

                                // Begin TT#368 - RMatelic - Allow WorkupTotalBuy colors and sizes
                                //if (colrQty <= 0)
                                //if (colrQty <= 0 && !ap.WorkUpTotalBuy)
                                if (colrQty < 0
                                    || colrQty == 0 && !ap.WorkUpTotalBuy)
                                // End TT#368
								{
									editError = true;

									bulkColorError = true; 
  
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                    msgText = MIDText.GetText(eMIDTextCode.msg_hl_Header2ColorUnitsGT0);
                                    msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                    msgText = msgText.Replace("{1}", hdrTranBulkColor.ColorCodeID);
                                    msgText = msgText + System.Environment.NewLine;
                                    em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                                                     
									//msgText = "Allocation Header " + hdrTran.HeaderId;
									//msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
									//msgText += " Units must be > 0" + System.Environment.NewLine;
									//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);

// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

								}
								else
								{
									try
									{
										ap.AddBulkColor(ccp.Key, colrQty, 0);
                                        bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
									}

									catch (Exception Ex)
									{
										editError = true;

										bulkColorError = true;

										if (Ex.GetType() != typeof(MIDException))
										{
											em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
										}
										else
										{
											MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
									}
								}
							}
							else
							{
								// ================================
								// Modify or Remove color on header
								// ================================
								try
								{
									colrQty = Convert.ToInt32(hdrTranBulkColor.Units, CultureInfo.CurrentUICulture);

									if (colrQty < 0)
									{
										colrQty = -1;
									}
								}

								catch (InvalidCastException)
								{
									colrQty = -1;
								}

								if (colrQty == 0)
								{
									// ========================
									// Remove color from header
									// ========================
									try
									{
										ap.RemoveBulkColor(ccp.Key);
                                        bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
									}

									catch (Exception Ex)
									{
										editError = true;

										bulkColorError = true;

										if (Ex.GetType() != typeof(MIDException))
										{
											em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
										}
										else
										{
											MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
									}
								}
								else
								{
									if (colrQty > 0)
									{
										// ======================
										// Modify color on header
										// ======================
										try
										{
                                            // Begin TT#1512 - RMatelic - InTransit Zero'd Out w\When Header Characteristic is modified
                                            //ap.SetColorUnitsToAllocate(ccp.Key, colrQty);
                                            if (ap.GetColorUnitsToAllocate(ccp.Key) != colrQty)
                                            {
                                                ap.SetColorUnitsToAllocate(ccp.Key, colrQty);
                                                bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
                                            }
                                            // End TT#1512
										}

										catch (Exception Ex)
										{
											editError = true;

											bulkColorError = true;

											if (Ex.GetType() != typeof(MIDException))
											{
												em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
											}
											else
											{
												MIDException MIDEx = (MIDException)Ex;

												//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
												em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
												//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											}
										}
									}
								}
							}
						}

						if (!bulkColorError)
						{
							// ========================
							// Process bulk color sizes
							// ========================
							if (hdrTranBulkColor.BulkColorSize != null) foreach (HeadersHeaderBulkColorBulkColorSize hdrTranBulkColorSize in hdrTranBulkColor.BulkColorSize)
							{
								sizeQty = 0;

								bulkColorSizeError = false;

								// ===============
								// Does size exist
								// ===============
								if (hdrTranBulkColorSize.SizeCodeID == "" || hdrTranBulkColorSize.SizeCodeID == null)
								{
									editError = true;

									bulkColorSizeError = true;

									msgText = "Allocation Header " + hdrTran.HeaderId;
									msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
									msgText += " has missing Size Code ID" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
								}
								else
								{
									scp = ValidateSize(hdrTranBulkColorSize.SizeCodeID,
													   hdrTranBulkColorSize.SizeCodeName,
													   hdrTranBulkColorSize.SizeCodePrimary,
													   hdrTranBulkColorSize.SizeCodeSecondary,
													   hdrTranBulkColorSize.SizeCodeProductCategory,
//													   hdrTranBulkColorSize.SizeCodeTableName,
//													   hdrTranBulkColorSize.SizeCodeHeading1,
//													   hdrTranBulkColorSize.SizeCodeHeading2,
//													   hdrTranBulkColorSize.SizeCodeHeading3,
//													   hdrTranBulkColorSize.SizeCodeHeading4,
													   ref em);

									if (scp.Key == Include.NoRID)
									{
										editError = true;

										bulkColorSizeError = true;

										msgText = "Allocation Header " + hdrTran.HeaderId;
										msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
										msgText += " Size Code ID " + hdrTranBulkColorSize.SizeCodeID;
										msgText += " NOT added to size table" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
									}
                                    else if (colorHnRID != Include.NoRID)
									{
										// =====================
										// Add size to hierarchy
										// =====================
										if (hdrTranBulkColorSize.SizeCodeDescription == "" || hdrTranBulkColorSize.SizeCodeDescription == null)
										{
											hdrTranBulkColorSize.SizeCodeDescription = null;
										}

										sizeHnRID = QuickAddSize(colorHnRID,
																 hdrTranBulkColorSize.SizeCodeID,
																 hdrTranBulkColorSize.SizeCodeDescription,
																 ref em);

										if (sizeHnRID == Include.NoRID)
										{
											editError = true;

											bulkColorSizeError = true;

											msgText = "Allocation Header " + hdrTran.HeaderId;
											msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
											msgText += " Size Code ID " + hdrTranBulkColorSize.SizeCodeID;
											msgText += " NOT added to hierarchy" + System.Environment.NewLine;
											em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
										}
									}
								}

								if (!bulkColorSizeError)
								{
									if (!ap.SizeIsOnBulkColor(ccp.Key, scp.Key))
									{
										// =================
										// Add size to color
										// =================
										try
										{
											sizeQty = Convert.ToInt32(hdrTranBulkColorSize.Units, CultureInfo.CurrentUICulture);

											if (sizeQty < 0)
											{
												sizeQty = 0;
											}
										}

										catch (InvalidCastException)
										{
											sizeQty = 0;
										}

                                        // Begin TT#368 - RMatelic - Allow WorkupTotalBuy colors and sizes
                                        //if (sizeQty <= 0)
                                        //if (sizeQty <= 0 && !ap.WorkUpTotalBuy)
                                        if (sizeQty < 0
                                            || sizeQty == 0 && !ap.WorkUpTotalBuy)
                                        // End TT#368
										{
											editError = true;

											bulkColorSizeError = true;
  
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load
                                            msgText = MIDText.GetText(eMIDTextCode.msg_hl_Header2ColorSizeUnitsGT0);
                                            msgText = msgText.Replace("{0}", hdrTran.HeaderId);
                                            msgText = msgText.Replace("{1}", hdrTranBulkColor.ColorCodeID);
                                            msgText = msgText.Replace("{2}", hdrTranBulkColorSize.SizeCodeID);
                                            msgText = msgText + System.Environment.NewLine;
                                            em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
 
											//msgText = "Allocation Header " + hdrTran.HeaderId;
											//msgText += " Color Code ID " + hdrTranBulkColor.ColorCodeID;
											//msgText += " Size Code ID " + hdrTranBulkColorSize.SizeCodeID;
											//msgText += " Units must be > 0" + System.Environment.NewLine;
											//em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
// TT2280 - FIX - RBeck - Change "Total units must be > 0" error in header load

										}
										else
										{
											try
											{
												// BEGIN MID Track #3634 - display sizes in order added
												//ap.AddBulkSizeToColor(ccp.Key, scp.Key, sizeQty, 0);
												ap.AddBulkSizeToColor(ccp.Key, scp.Key, sizeQty, -1);
												// END MID Track #3634
                                                bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
											}

											catch (Exception Ex)
											{
												editError = true;

												bulkColorSizeError = true;

												if (Ex.GetType() != typeof(MIDException))
												{
													em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
												}
												else
												{
													MIDException MIDEx = (MIDException)Ex;

													//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
													em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
													//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												}
											}
										}
									}
									else
									{
										// ==============================
										// Modify or Remove size on color
										// ==============================
										try
										{
											sizeQty = Convert.ToInt32(hdrTranBulkColorSize.Units, CultureInfo.CurrentUICulture);

											if (sizeQty < 0)
											{
												sizeQty = -1;
											}
										}

										catch (InvalidCastException)
										{
											sizeQty = -1;
										}

										if (sizeQty == 0)
										{
											// ======================
											// Remove size from color
											// ======================
											try
											{
												ap.RemoveBulkColorSize(ccp.Key, scp.Key);
                                                bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
											}

											catch (Exception Ex)
											{
												editError = true;

												bulkColorSizeError = true;

												if (Ex.GetType() != typeof(MIDException))
												{
													em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
												}
												else
												{
													MIDException MIDEx = (MIDException)Ex;

													//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
													em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
													//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
												}
											}
										}
										else
										{
											if (sizeQty > 0)
											{
												// ====================
												// Modify size on color
												// ====================
												try
												{
                                                    // Begin TT#1512 - RMatelic - InTransit Zero'd Out w\When Header Characteristic is modified
                                                    //ap.SetSizeUnitsToAllocate(ccp.Key, scp.Key, sizeQty);
                                                    if (ap.GetSizeUnitsToAllocate(ccp.Key, scp.Key) != sizeQty)
                                                    {
                                                        ap.SetSizeUnitsToAllocate(ccp.Key, scp.Key, sizeQty);
                                                        bRebuildMaster = true;  // TT#1966-MD - JSmith - DC Fulfillment
                                                    }
                                                    // End TT#1512
												}

												catch (Exception Ex)
												{
													editError = true;

													bulkColorSizeError = true;

													if (Ex.GetType() != typeof(MIDException))
													{
														em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
													}
													else
													{
														MIDException MIDEx = (MIDException)Ex;

														//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
														//em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
														em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
														//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
													}
												}
											}
										}
									}
								}
							}
						}
					}

                    // Begin TT#2469 - JSmith - MIssing Header Characteristics on Distro
                    bool workflowTrigger = ap.WorkflowTrigger;
                    ap.WorkflowTrigger = false;
                    // End TT#2469 - JSmith - MIssing Header Characteristics on Distro

					if (!editError)
					{
						// begin MID Track 4404 Work Up Buy Force Cancel on 0 Total Units
						if (!ap.WorkUpTotalBuy)
						{
							ap.IsInterfaced = true;
						}
						// end MID Track 4404 Work Up By Force Cancel on 0 Total Units

                        // begin TT#3939 - Jellis - Group has Received Out of Balance Status after API update
                        //try
                        //{
						////BeginTT#1767 - DOConnell - Allocation Header API Error Lock transaction timeout
						//	//if (ap.WriteHeader())
                        //    if (ap.WriteHeader(true))

                        ////EndTT#1767 - DOConnell - Allocation Header API Error Lock transaction timeout
                        //    {
                        //        // BEGIN MID Change j.ellis -- allow characteristic modifications anytime
                        //        // // =======================================================
                        //        // // Characterics processing is done at this point since the
                        //        // // header RID is NOT assigned until WriteHeader is called
                        //        // // =======================================================
                        //        //if (hdrTran.Characteristic != null) foreach (HeadersHeaderCharacteristic hdrTranChars in hdrTran.Characteristic)
                        //        //{
                        //        //	try
                        //        //	{
                        //        //		charsRtn = ap.ProcessHeaderLoadCharacteristic(hdrTranChars.Name, hdrTranChars.Value, ahp.Key, "MODIFY");
                        //        //
                        //        //		if ((eReturnCode)charsRtn != eReturnCode.successful)
                        //        //		{
                        //        //			charError = true;
                        //        //
                        //        //			SetCharacteristicMessages(hdrTran.HeaderId, hdrTranChars, charsRtn, ref em);
                        //        //		}
                        //        //	}
                        //        //
                        //        //	catch (Exception Ex)
                        //        //	{
                        //        //		charError = true;
                        //        //
                        //        // 		em.AddMsg(eMIDMessageLevel.Edit, Ex.ToString(), GetType().Name);
                        //        //	}
                        //        //}
                        //        // END MID Change j.ellis -- allow characteristic modifications anytime
                        //    }
                        //    else
                        //    {
                        //        modifyOkay = false;
                        //    }
                        //}
                        try
                        {
                            if (ap.AssortmentProfile != null
                                && ap.AssortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation)
                            {
                                modifyOkay = ap.AssortmentProfile.WriteHeader(true);
                            }
                            else
                            {
                                modifyOkay = ap.WriteHeader(true);
                                // Begin TT#1966-MD - JSmith - DC Fulfillment
                                if (ap.IsSubordinateHeader
                                    && !ap.DCFulfillmentProcessed
                                    && bRebuildMaster)
                                {
                                    try
                                    {
                                        ap.MasterHeader.ReReadHeader();
                                        ap.MasterHeader.RebuildMaster(aTrans);
                                        ap.MasterHeader.HeaderDataRecord.OpenUpdateConnection();
                                        ap.MasterHeader.WriteHeader(true);
                                        ap.MasterHeader.HeaderDataRecord.CommitData();
                                    }
                                    catch (Exception)
                                    {
                                        
                                        throw;
                                    }
                                    finally
                                    {
                                        if (ap.MasterHeader.HeaderDataRecord.ConnectionIsOpen)
                                        {
                                            ap.MasterHeader.HeaderDataRecord.CloseUpdateConnection();
                                        }
                                    }
                                }
                                // End TT#1966-MD - JSmith - DC Fulfillment
                            }
                        }
                        // end TT#3939 - Jellis - Group has Received Out of Balance Status after API update
                        catch (Exception Ex)
                        {
                            modifyOkay = false;

                            exceptMsg = Ex.ToString();
                        }
					}

					// BEGIN MID Change j.ellis -- allow characteristic modifications anytime
					// =======================================================
					// Characterics processing is done at this point since the
					// header RID is NOT assigned until WriteHeader is called
					// =======================================================
					if (hdrTran.Characteristic != null) 
					{
						foreach (HeadersHeaderCharacteristic hdrTranChars in hdrTran.Characteristic)
						{
							try
							{
                                // Begin TT#168 - RMatelic - Header characteristics auto add
                                //charsRtn = ap.ProcessHeaderLoadCharacteristic(hdrTranChars.Name, hdrTranChars.Value, ahp.Key, "MODIFY");
                                //eHeaderCharType headerCharType = (hdrTranChars.CharType != null) ? (eHeaderCharType)hdrTranChars.CharType : eHeaderCharType.text;
                                //bool protectChar = (hdrTranChars.Protect != null) ? hdrTranChars.Protect : true;
                                eHeaderCharType headerCharType = (eHeaderCharType)hdrTranChars.CharType;
                                bool protectChar = hdrTranChars.Protect;
								//Begin TT#1252 - JScott - Duplicate key in Workspace when opening application
								//charsRtn = ap.ProcessHeaderLoadCharacteristic(hdrTranChars.Name, hdrTranChars.Value, ahp.Key, "MODIFY", _autoAddCharacteristics, headerCharType, protectChar);

								hdrCharGrpID = hdrTranChars.Name;
								charsRtn = ap.ProcessHeaderLoadCharacteristic(ref hdrCharGrpID, hdrTranChars.Value, ap.Key, "MODIFY", _autoAddCharacteristics, headerCharType, protectChar);
								hdrTranChars.Name = hdrCharGrpID;

								//End TT#1252 - JScott - Duplicate key in Workspace when opening application
								// End TT#168  
								if ((eReturnCode)charsRtn != eReturnCode.successful)
								{
									charError = true;
					
									SetCharacteristicMessages(hdrTran.HeaderId, hdrTranChars, charsRtn, ref em);
								}
							}
					
							catch (Exception Ex)
							{
								charError = true;
					
								em.AddMsg(eMIDMessageLevel.Edit, Ex.ToString(), GetType().Name);
							}
						}
					}
					// END MID Change j.ellis -- allow characteristic modifications anytime

                    // Begin TT#2469 - JSmith - MIssing Header Characteristics on Distro
                    if (modifyOkay &&
                        workflowTrigger)
                    {
                        ap.Action(eAllocationMethodType.ApplyAPI_Workflow, new GeneralComponent(eGeneralComponentType.Total), 0.0d, Include.NoRID, true);
                    }
                    // End TT#2469 - JSmith - MIssing Header Characteristics on Distro

					// ==============
					// Dequeue Header
					// ==============
					//headerEnqueue.DequeueHeaders(); // TT#1185 - Verify ENQ before update
                    // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                    //DequeueHeaders(aTrans);          // TT#1185 - Verify ENQ before update
					// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 

					//hdrEnqueued = false;           // TT#1185 - Verify ENQ before Update

					if (modifyOkay)
					{
						if (!editError)
						{
							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " (Transaction #" + _headersRead.ToString() + ")";
							msgText += " successfully modified" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

							if (charError)
							{
								msgText = "Allocation Header " + hdrTran.HeaderId;
								msgText += " (Transaction #" + _headersRead.ToString() + ")";
								msgText += " encountered characteristic errors" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
							}

// (CSMITH) - BEG: Incorrect action when Modify performed from Reset
							if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
							{
								WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", true);
							}
							else
							{
								WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), true);
							}
// (CSMITH) - END: Incorrect action when Modify performed from Reset

							rtnCode = eReturnCode.successful;
						}
						else
						{
							msgText = "Allocation Header " + hdrTran.HeaderId;
							msgText += " (Transaction #" + _headersRead.ToString() + ")";
							msgText += " encountered edit errors" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

// (CSMITH) - BEG: Incorrect action when Modify performed from Reset
							if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
							{
								WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
							}
							else
							{
								WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
							}
// (CSMITH) - END: Incorrect action when Modify performed from Reset

							rtnCode = eReturnCode.editErrors;
						}
					}
					else
					{
						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " (Transaction #" + _headersRead.ToString() + ")";
						if (exceptMsg == null)
						{
							msgText += " encountered an update error" + System.Environment.NewLine;
						}
						else
						{
							msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
						}
                        // Begin TT#1773 - JSmith - Header Load return information level message after database error.
                        //em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
                        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        // End TT#1773

// (CSMITH) - BEG: Incorrect action when Modify performed from Reset
						if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
						{
							WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
						}
						else
						{
							WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
						}
// (CSMITH) - END: Incorrect action when Modify performed from Reset

						rtnCode = eReturnCode.severe;
					}
				}
			}

			catch (Exception Ex)
			{
// (CSMITH) - BEG: Incorrect action when Modify performed from Reset
				if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
				{
					WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
				}
				else
				{
					WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
				}
// (CSMITH) - END: Incorrect action when Modify performed from Reset

				_audit.Log_Exception(Ex, GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
                // begin TT#1185 - Verify ENQ before Update
                //if (hdrEnqueued)
                //{
                //    headerEnqueue.DequeueHeaders();

                //    hdrEnqueued = false;
                //}
                // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                //DequeueHeaders(aTrans);
				// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                // end TT#1185 - Verify ENQ before Update

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}

                // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
                if (_deleteWorkFlowRID != Include.UndefinedWorkflowRID)
                {
                    //---Load Old WorkFlow-------------
                    AllocationWorkFlow allocationWorkflow = new AllocationWorkFlow(_SAB, _deleteWorkFlowRID, Include.SystemUserRID, false);

                    //---Delete WorkFlow-------------
                    allocationWorkflow.Workflow_Change_Type = eChangeType.delete;

                    //---Save WorkFlow-------------
                    SaveWorkflow(allocationWorkflow, ref em);
                }
                // END MID Track #6336
			}

			return rtnCode;
		}

        // begin TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B
        /// <summary>
        /// Sets the header type
        /// </summary>
        /// <param name="aHeaderID">Header ID (should match the ID inthe Allocation Profile</param>
        /// <param name="aAllocationProfile">AllocationProfile</param>
        /// <param name="aHeaderType">Header Type for this header</param>
        /// <param name="vsw_id">The VSW ID for this header if type is VSW (IMO); null otherwise</param>
        /// <param name="aMessageLevel">When set of header type fails (return false), this contains the message level of the returned message</param>
        /// <param name="aTypeMessage">Error message returned when Set of header type fails.</param>
        /// <returns></returns>
        private bool SetHeaderType(string aHeaderID, AllocationProfile aAllocationProfile, string aHeaderType, string vsw_id, bool aAdjustVSW_OnHand, out eMIDMessageLevel aMessageLevel, out string aTypeMessage) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
        {
            MIDException midException;
            bool returnStatus = true;
            eHeaderType headerType;
            aMessageLevel = eMIDMessageLevel.None;
            aTypeMessage = string.Empty;
            try
            {
                switch (aHeaderType)
                {
                    case "VSW":
                    {
                        headerType = eHeaderType.IMO;                        
                        if (vsw_id == null
                            || vsw_id.Trim() == string.Empty)
                        {
                            returnStatus = false;
                            // BEGIN TT#2225 - AGallagher - VSW Modifcations Enhancement
                            // aMessageLevel = eMIDMessageLevel.Severe;
                            aMessageLevel = eMIDMessageLevel.Error;
                            // END TT#2225 - AGallagher - VSW Modifcations Enhancement
                            aTypeMessage = MIDText.GetText(eMIDTextCode.msg_al_IMOIdRequired);
                            aTypeMessage = aTypeMessage.Replace("{0}", aHeaderID);
                            aTypeMessage = aTypeMessage + System.Environment.NewLine;
                        }
                        else if (!StoreMgmt.DoesIMOExist(vsw_id)) //_SAB.StoreServerSession.DoesIMOExist(vsw_id))
                        {
                            returnStatus = false;
                            // BEGIN TT#2225 - AGallagher - VSW Modifcations Enhancement
                            // aMessageLevel = eMIDMessageLevel.Severe;
                            aMessageLevel = eMIDMessageLevel.Error;
                            // END TT#2225 - AGallagher - VSW Modifcations Enhancement
                            aTypeMessage = MIDText.GetText(eMIDTextCode.msg_al_IMOIdNotFound);
                            aTypeMessage = aTypeMessage.Replace("{0}", aHeaderID);
                            aTypeMessage = aTypeMessage + System.Environment.NewLine;
                        }
                        else if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                        {
                            returnStatus = false;
                            aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                            aTypeMessage = midException.ErrorMessage;
                        }
                        break;
                    }
                    default:
                    {
                        if (vsw_id != null
                            && vsw_id.Trim() != string.Empty)
                        {
                            returnStatus = false;
                            // BEGIN TT#2225 - AGallagher - VSW Modifcations Enhancement
                            // aMessageLevel = eMIDMessageLevel.Severe;
                            aMessageLevel = eMIDMessageLevel.Error;
                            // END TT#2225 - AGallagher - VSW Modifcations Enhancement
                            aTypeMessage = MIDText.GetText(eMIDTextCode.msg_al_IMOIdWarning);
                            aTypeMessage = aTypeMessage.Replace("{0}", aHeaderID);
                            aTypeMessage = aTypeMessage + System.Environment.NewLine;
                        }
                        else
                        {
                            switch (aHeaderType)
                            {
                                case "RECEIPT":
                                {
                                    headerType = eHeaderType.Receipt;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException))  // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                case "PO":
                                {
                                    headerType = eHeaderType.PurchaseOrder;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                case "ASN":
                                {
                                    headerType = eHeaderType.ASN;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                case "DUMMY":
                                {
                                    headerType = eHeaderType.Receipt;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                case "DROPSHIP":
                                {
                                    headerType = eHeaderType.DropShip;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                case "RESERVE":
                                {
                                    headerType = eHeaderType.Reserve;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                case "WORKUPTOTALBUY":
                                {
                                    headerType = eHeaderType.WorkupTotalBuy;
                                    if (!aAllocationProfile.SetHeaderType(headerType, vsw_id, aAdjustVSW_OnHand, out midException)) // TT#2225 - Jellis - AnF VSW FSWOS Max Enhancement pt 1
                                    {
                                        returnStatus = false;
                                        aMessageLevel = Include.TranslateErrorLevel(midException.ErrorLevel);
                                        aTypeMessage = midException.ErrorMessage;   
                                    }
                                    break;
                                }
                                default:
                                {
                                    returnStatus = false;
                                    aMessageLevel = eMIDMessageLevel.Severe;
                                    aTypeMessage = "Allocation Header " + aHeaderID;
                                    aTypeMessage += " has an invalid Header Type [" + aHeaderType + "]";
                                    aTypeMessage += " - current value NOT changed" + System.Environment.NewLine;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                   returnStatus = false;
                   if (Ex.GetType() != typeof(MIDException))
                   {
                       aMessageLevel = eMIDMessageLevel.Severe;
                       aTypeMessage = Ex.ToString();
                   }
                   else
                   {
                       MIDException MIDEx = (MIDException)Ex;
                       aMessageLevel = Include.TranslateErrorLevel(MIDEx.ErrorLevel);
                       aTypeMessage = MIDEx.ErrorMessage;
                   }
            }
            return returnStatus;
        }
        // end TT#1401 - JEllis - Urban Virtual Store Warehouse pt 28B

		// ======================
		// Remove existing header
		// ======================
		public eReturnCode RemoveHeader(HeadersHeader hdrTran, AllocationProfile ap, ApplicationSessionTransaction aTrans)
		{
			EditMsgs em = null;

			string msgText = null;
			string exceptMsg = null;

			bool removeOkay = true;
            //bool hdrEnqueued = false;           // TT#1185 - Verify ENQ before update

            //HeaderEnqueue headerEnqueue = null; // TT#1185 - Verify ENQ before update

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

            try
            {
                // ==============
                // Enqueue Header
                // ==============
			// Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
            //    // begin TT#1185 - Verify ENQ before Update
            //    try
            //    {
            //        List<int> hdrRidList = new List<int>();
            //        hdrRidList.Add(ap.Key);
            //        if (EnqueueHeader(aTrans, hdrRidList, out exceptMsg))
            //        {
            //            //// Begin TT#1040 - MD - stodd - header load API for Group Allocation 
            //            //ProfileList ampl = aTrans.GetAssortmentMemberProfileList();
            //            //if (ampl.Count > 0)
            //            //{
            //            //    foreach (AllocationProfile ap1 in ampl.ArrayList)
            //            //    {
            //            //        ap1.ReReadHeader();
            //            //    }
            //            //}
            //            //else
            //            //{
            //            //    ap.ReReadHeader();
            //            //}
            //            //// End TT#1040 - MD - stodd - header load API for Group Allocation 
            //        }
            //        else
            //        {
            //            em.AddMsg(eMIDMessageLevel.Edit, exceptMsg, GetType().Name);
            //            if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
            //            {
            //                WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
            //            }
            //            else
            //            {
            //                WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
            //            }
            //            rtnCode = eReturnCode.warning;
            //        }
            //    }
            //    catch (Exception Ex)
            //    {
            //        DequeueHeaders(aTrans);
            //        if (Ex.GetType() != typeof(MIDException))
            //        {
            //            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
            //        }
            //        else
            //        {
            //            MIDException MIDEx = (MIDException)Ex;
            //            em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
            //        }
            //        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
            //        rtnCode = eReturnCode.severe;
            //    }
			// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                //AllocationHeaderProfile ahp = new AllocationHeaderProfile(ap.HeaderID, ap.Key);

                //AllocationHeaderProfileList ahpList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

                //try
                //{
                //    ahpList.Clear();

                //    ahpList.Add(ahp);

                //    headerEnqueue = new HeaderEnqueue(aTrans, ahpList);

                //    headerEnqueue.EnqueueHeaders();

                //    ap.ReReadHeader();

                //    hdrEnqueued = true;
                //}

                //catch (HeaderConflictException)
                //{
                //    SecurityAdmin secAdmin = new SecurityAdmin();

                //    msgText = "Allocation Header " + ahp.HeaderID + " is currently in use by User(s): ";
                //    foreach (HeaderConflict hc in headerEnqueue.HeaderConflictList)
                //    {
                //        msgText += System.Environment.NewLine + secAdmin.GetUserName(hc.UserRID);
                //    }
                //    msgText += System.Environment.NewLine;
                //    em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);

                //    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                //    rtnCode = eReturnCode.warning;
                //}

                //catch (Exception Ex)
                //{
                //    if (Ex.GetType() != typeof(MIDException))
                //    {
                //        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                //    }
                //    else
                //    {
                //        MIDException MIDEx = (MIDException)Ex;

                //        //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                //        //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
                //        em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                //        //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
                //    }

                //    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

                //    rtnCode = eReturnCode.severe;
                //}

                if (_headersEnqueued)
				{
					try
					{
                        // Begin TT#393 - JSmith - Delete header when in multi does not update multi
                        //if (!ap.DeleteHeader())
                        //{
                        //    removeOkay = ap.DeleteHeader();
                        //}
                        // Begin TT#4755 - JSmith - Cannot Remove header from Group
                        //removeOkay = ap.DeleteHeader(true);
                        bool forceDelete = false;
                        if (ap.AsrtRID != Include.NoRID)
                        {
                            forceDelete = true;
                        }
                        removeOkay = ap.DeleteHeader(true, forceDelete);
                        // Begin TT#4755 - JSmith - Cannot Remove header from Group
                        // End TT#393
					}

					catch (Exception Ex)
					{
						removeOkay = false;

						exceptMsg = Ex.ToString();
					}

					// ==============
					// Dequeue Header
					// ==============
					//headerEnqueue.DequeueHeaders(); // TT#1185 - Verify ENQ before Update
                    //hdrEnqueued = false;            // TT#1185 - Verify ENQ before Update
                    // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                    // DequeueHeaders(aTrans);           // TT#1185 - Verify ENQ before Update
					// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 


					if (removeOkay)
					{
						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " (Transaction #" + _headersRead.ToString() + ")";
						msgText += " successfully removed" + System.Environment.NewLine;
						em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
						
						// Begin TT#1581-MD - stodd - Header Reconcile API
                        if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                        {
                            WriteHeaderLoadStatus(hdrTran.HeaderId, "Remove", true);
                        }
                        else
                        {
                            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), true);
                        }
						// End TT#1581-MD - stodd - Header Reconcile API 

						rtnCode = eReturnCode.successful;
					}
					else
					{
						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " (Transaction #" + _headersRead.ToString() + ")";
						if (exceptMsg == null)
						{
							msgText += " encountered an update error" + System.Environment.NewLine;
						}
						else
						{
							msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
						}
                        // Begin TT#1773 - JSmith - Header Load return information level message after database error.
                        //em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
                        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        // End TT#1773

						// Begin TT#1581-MD - stodd - Header Reconcile API 
                        if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                        {
                            WriteHeaderLoadStatus(hdrTran.HeaderId, "Remove", false);
                        }
                        else
                        {
                            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                        }
						// End TT#1581-MD - stodd - Header Reconcile API 

						rtnCode = eReturnCode.severe;
					}
			    }
			}

			catch (Exception Ex)
			{
				// Begin TT#1581-MD - stodd - Header Reconcile API 
                if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                {
                    WriteHeaderLoadStatus(hdrTran.HeaderId, "Remove", false);
                }
                else
                {
                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                }
				// End TT#1581-MD - stodd - Header Reconcile API 

				_audit.Log_Exception(Ex, GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
                // begin TT#1185 -Verify ENQ before Update
                //if (hdrEnqueued)
                //{
                //    headerEnqueue.DequeueHeaders(); 
                //    hdrEnqueued = false;
                //}
                // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                // DequeueHeaders(aTrans);
				// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                // end TT#1185 - Verify ENQ before Update

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}


        private eReturnCode RemoveGroupAllocation(HeadersHeader hdrTran, ApplicationSessionTransaction aTrans, ref AllocationProfileList apl, AssortmentProfile asp)
        {
            eReturnCode rtnCode = eReturnCode.successful;
            string msgText = "Group Allocation Header " + asp.HeaderID;
            msgText += " removal";
            msgText += " - Begin Processing" + System.Environment.NewLine;
            _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);


            apl = new AllocationProfileList(eProfileType.Allocation);

            apl.Add(asp);

            aTrans.SetMasterProfileList(apl);

            rtnCode = RemoveHeader(hdrTran, asp, aTrans);
            if (rtnCode == eReturnCode.successful)
            {
                msgText = "Group Allocation Header " + asp.HeaderID;
                msgText += " removed due to last remaining header on it was removed.";
                msgText += " - End Processing" + System.Environment.NewLine;
                _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                // Cleanup
                aTrans.RemoveMasterProfileList(apl);
                apl.Clear();
                asp.Dispose();
            }
            return rtnCode;
        }

        public eReturnCode RemovePlaceholder(HeadersHeader hdrTran, ApplicationSessionTransaction aTrans, ref AllocationProfileList apl, AllocationProfile ap)
        {
            eReturnCode rtnCode = eReturnCode.successful;
            string msgText = "Placeholder Header " + ap.HeaderID;
            msgText += " removal";
            msgText += " - Begin Processing" + System.Environment.NewLine;
            _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

            apl = new AllocationProfileList(eProfileType.Allocation);

            apl.Add(ap);

            aTrans.SetMasterProfileList(apl);

            rtnCode = RemoveHeader(hdrTran, ap, aTrans);
            if (rtnCode == eReturnCode.successful)
            {
                msgText = "Placeholder Header " + ap.HeaderID;
                msgText += " removed due to last remaining header on it was removed.";
                msgText += " - End Processing" + System.Environment.NewLine;
                _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                // Cleanup
                aTrans.RemoveMasterProfileList(apl);
                apl.Clear();
                ap.Dispose();
            }
            return rtnCode;
        }



		// ======================
		// Reset existing header
		// ======================
		public eReturnCode ResetHeader(HeadersHeader hdrTran, AllocationProfile ap, ApplicationSessionTransaction aTrans)
		{
			EditMsgs em = null;

			string msgText = null;
			string exceptMsg = null;

			bool resetOkay = true;
            //bool hdrEnqueued = false;  // TT#1185 - Verfiy ENQ before Update

			GeneralComponent Component = null;

            //HeaderEnqueue headerEnqueue = null; // TT#1185 - Verify ENQ before Update

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

            // begin TT#1185 - Verify ENQ before update
            //try
            //{
            //    // ==============
            //    // Enqueue Header
            //    // ==============
            //    AllocationHeaderProfile ahp = new AllocationHeaderProfile(ap.HeaderID, ap.Key);

            //    AllocationHeaderProfileList ahpList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

            //    try
            //    {
            //        ahpList.Clear();

            //        ahpList.Add(ahp);

            //        headerEnqueue = new HeaderEnqueue(aTrans, ahpList);

            //        headerEnqueue.EnqueueHeaders();

            //        ap.ReReadHeader();

            //        hdrEnqueued = true;
            //    }

            //    catch (HeaderConflictException)
            //    {
            //        SecurityAdmin secAdmin = new SecurityAdmin();

            //        msgText = "Allocation Header " + ahp.HeaderID + " is currently in use by User(s): ";
            //        foreach (HeaderConflict hc in headerEnqueue.HeaderConflictList)
            //        {
            //            msgText += System.Environment.NewLine + secAdmin.GetUserName(hc.UserRID);
            //        }
            //        msgText += System.Environment.NewLine;
            //        em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);

            //        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

            //        rtnCode = eReturnCode.warning;
            //    }

            //    catch (Exception Ex)
            //    {
            //        if (Ex.GetType() != typeof(MIDException))
            //        {
            //            em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
            //        }
            //        else
            //        {
            //            MIDException MIDEx = (MIDException)Ex;

            //            //Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
            //            //em.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
            //            em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
            //            //End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
            //        }

            //        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

            //        rtnCode = eReturnCode.severe;
            //    }

            //    if (hdrEnqueued)
            try
            {
				// Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                //try
                //{
                //    List<int> hdrRidList = new List<int>();
                //    hdrRidList.Add(ap.Key);
                //    if (EnqueueHeader(aTrans, hdrRidList, out exceptMsg))
                //    {
                //        //ap.ReReadHeader();
                //    }
                //    else
                //    {
                //        em.AddMsg(eMIDMessageLevel.Edit, exceptMsg, GetType().Name);
                //        if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                //        {
                //            WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
                //        }
                //        else
                //        {
                //            WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                //        }
                //        rtnCode = eReturnCode.warning;
                //    }
                //}
                //catch (Exception Ex)
                //{
                //    DequeueHeaders(aTrans);
                //    if (Ex.GetType() != typeof(MIDException))
                //    {
                //        em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                //    }
                //    else
                //    {
                //        MIDException MIDEx = (MIDException)Ex;
                //        em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                //    }
                //    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                //    rtnCode = eReturnCode.severe;
                //}
				// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                if (_headersEnqueued)
            // end TT#1185 - Verify ENQ before Update
				{
					try
					{
						Component = new GeneralComponent(eGeneralComponentType.Total);

						if (!ap.Action(eAllocationMethodType.Reset, Component, 0.0, Include.NoRID, true))
						{
							resetOkay = false;
						}
					}

					catch (Exception Ex)
					{
						resetOkay = false;

						exceptMsg = Ex.ToString();
					}

					// ==============
					// Dequeue Header
					// ==============
                    //headerEnqueue.DequeueHeaders();  // TT#1185 - Verify ENQ before Update

                    //hdrEnqueued = false;  // TT#1185 - Verify ENQ before Update
                    // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                    //DequeueHeaders(aTrans); // TT#1185 - Verfiy ENQ before Update
					// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 

					if (resetOkay)
					{
						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " (Transaction #" + _headersRead.ToString() + ")";
						msgText += " successfully reset" + System.Environment.NewLine;
						em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

						WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), true);

						rtnCode = eReturnCode.successful;
					}
					else
					{
						msgText = "Allocation Header " + hdrTran.HeaderId;
						msgText += " (Transaction #" + _headersRead.ToString() + ")";
						if (exceptMsg == null)
						{
							msgText += " encountered an update error" + System.Environment.NewLine;
						}
						else
						{
							msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
						}
                        // Begin TT#1773 - JSmith - Header Load return information level message after database error.
                        //em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
                        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        // End TT#1773

						WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

						rtnCode = eReturnCode.severe;
					}
				}
			}

			catch (Exception Ex)
			{
				WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);

				_audit.Log_Exception(Ex, GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
                // begin TT#1185 - Verify ENQ before Update
                //if (hdrEnqueued)
                //{
                //    //headerEnqueue.DequeueHeaders();
                //    hdrEnqueued = false;
                //}
                // Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                // DequeueHeaders(aTrans); 
				// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
                // end TT#1185 - Verify ENQ before Update

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// Begin TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 
        private eReturnCode EnqueueHeader(ApplicationSessionTransaction aTrans, int hdrRid, HeadersHeader hdrTran, ref EditMsgs em)
        {
            eReturnCode rtnCode = eReturnCode.successful;
            string exceptMsg = string.Empty;
            try
            {
                List<int> hdrRidList = new List<int>();
                if (hdrRid > 0)
                {
                    hdrRidList.Add(hdrRid);
                }

                //=======================================================================
                // Note: If the header being processed belongs to a group allocation,
                // all headers in that group allocation will be enqueued.
                //=======================================================================
                if (EnqueueHeader(aTrans, hdrRidList, out exceptMsg))
                {
                    rtnCode = eReturnCode.successful;
                }
                else
                {
                    em.AddMsg(
                        eMIDMessageLevel.Information,
                        eMIDTextCode.msg_al_HeaderEnqFailed,
                        MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed),
                        GetType().Name);
                    em.AddMsg(eMIDMessageLevel.Edit, exceptMsg, GetType().Name);
                    if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                    {
                        WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
                    }
                    else
                    {
                        WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                    }
                    rtnCode = eReturnCode.warning;
                }
            }
            catch (Exception Ex)
            {
                DequeueHeaders(aTrans);
                if (Ex.GetType() != typeof(MIDException))
                {
                    em.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                }
                else
                {
                    MIDException MIDEx = (MIDException)Ex;
                    em.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                }
                if (hdrTran.HeaderAction.ToString().ToUpper() == "RESET")
                {
                    WriteHeaderLoadStatus(hdrTran.HeaderId, "Modify", false);
                }
                else
                {
                    WriteHeaderLoadStatus(hdrTran.HeaderId, hdrTran.HeaderAction.ToString(), false);
                }
                rtnCode = eReturnCode.severe;
            }

            return rtnCode;
        }
		// End TT#1096 - MD - stodd - header load error modifying header belonging to a group allocation 

        // begin TT#1185 - Verify ENQ before Update 
        private bool EnqueueHeader(ApplicationSessionTransaction aTrans, List<int> aHdrRidList, out string aErrorMsg)
        {
            aErrorMsg = string.Empty;
            bool enqueueStatus;
            List<int> hdrRidList = new List<int>();
            foreach (int hdrRID in aHdrRidList)
            {
                if (hdrRID > 0)       // remove any negative valued RIDs
                {
                    hdrRidList.Add(hdrRID);
                }
            }
            if (hdrRidList.Count == 0)
            {
                enqueueStatus = true;  // new headers are not enqueued
            }
            else
            {
                enqueueStatus = aTrans.EnqueueHeaders(aTrans.GetHeadersToEnqueue(aHdrRidList), out aErrorMsg);
                _headersEnqueued = enqueueStatus;
            }
            return enqueueStatus;
            // end TT#1185 - Verify ENQ before Update
        }
        private void DequeueHeaders(ApplicationSessionTransaction aTrans)
        {
            aTrans.DequeueHeaders();
            _headersEnqueued = false;
        }

		// =================================
		// Validate a style and style parent
		// =================================
		// Begin MID Track #4958 - JSmith - header description using style ID
//		private int ValidateStyle(string aHeaderId, string aStyleId, string aStyleDescription, string aStyleName, string aParentId, ref EditMsgs aEm)
		private HierarchyNodeProfile ValidateStyle(string aHeaderId, string aStyleId, string aStyleDescription, string aStyleName, string aParentId, ref EditMsgs aEm)
		// End MID Track #4958
		{
			string msgText = null;

			HierarchyProfile hp = null;

			HierarchyLevelProfile hlp = null;

			HierarchyNodeProfile styleHnp = null;
			HierarchyNodeProfile styleParentHnp = null;

			int styleRID = Include.NoRID;
			int styleParentHomeHierarchyRID = Include.NoRID;

			try
			{
				// =============================
				// Does style exist on hierarchy
				// =============================
                // Begin TT#1218 - JSmith - API - Header Load Performance
                styleHnp = (HierarchyNodeProfile)_htStyles[aStyleId];
                if (styleHnp != null)
                {
                    return styleHnp;
                }
                // End TT#1218

				styleHnp = _SAB.HierarchyServerSession.GetNodeData(aStyleId);

				if (styleHnp.Key != Include.NoRID)
				{
                    // Begin TT#967 - JSmith - Allows non style node to be associated with a header
                    if (styleHnp.LevelType != eHierarchyLevelType.Style)
                    {
                        aEm.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_al_MustBeValidStyle, GetType().Name);
                        styleHnp = null;
                    }
                    else
                    {
                    // End TT#967
                        styleRID = styleHnp.Key;
                        // BEGIN MID Track #3595 - Update Style Description
                        if (_updateStyleDescription)
                        {
                            if (aStyleDescription != null && aStyleDescription.Trim() != string.Empty)
                            {
                                styleHnp.NodeChangeType = eChangeType.update;
                                styleHnp.NodeDescription = aStyleDescription;
                                _hierMaint.ProcessNodeProfileInfo(ref aEm, styleHnp);
                            }
                        }
                        // END MID Track #3595 
                    }
				}
				else
				{
					try
					{
						// ====================================
						// Style does not exist on hierarchy
						// Does style parent exist on hierarchy
						// ====================================
						styleParentHnp = _SAB.HierarchyServerSession.GetNodeData(aParentId);

						if (styleParentHnp.Key == Include.NoRID)
						{
							msgText = "Allocation Header " + aHeaderId;
							msgText += " Parent Of Style Id " + aParentId;
							msgText += " NOT found on Merchandise Tree" + System.Environment.NewLine;
							aEm.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
						}
						else
						{
							try
							{
								// ===========================================
								// Verify style parent is at the correct level
								// ===========================================
								styleParentHomeHierarchyRID = styleParentHnp.HomeHierarchyRID;

								hp = _SAB.HierarchyServerSession.GetHierarchyData(styleParentHomeHierarchyRID);

								hlp = (HierarchyLevelProfile)hp.HierarchyLevels[styleParentHnp.HomeHierarchyLevel + 1];

								if (hlp.LevelType != eHierarchyLevelType.Style)
								{
									msgText = "Allocation Header " + aHeaderId;
									msgText += " Parent Of Style Id " + aParentId;
									msgText += " is at the wrong hierarchy level" + System.Environment.NewLine;
									aEm.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
								}
								else
								{
									try
									{
										// ======================
										// Add style to hierarchy
										// ======================
									
										// BEGIN MID Track #3595 - Update Style Description
										// BEGIN MID Track #3592 - Add Style Name
										//styleRID = _hierMaint.QuickAdd(ref aEm, styleParentHnp.Key, aStyleId);
										bool nodeAdded = true;
 										styleRID = _hierMaint.QuickAdd(ref aEm, styleParentHnp.Key, aStyleId, aStyleDescription, aStyleName, null, null, null, ref nodeAdded);

										// END MID Track #3595 
										// END MID Track #3592 

										if (styleRID == Include.NoRID)
										{
											msgText = "Allocation Header " + aHeaderId;
											msgText += " Style Id " + aStyleId;
											msgText += " NOT added to hierarchy" + System.Environment.NewLine;
											aEm.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
										}
										// BEGIN MID Track #5207 - JSmith - Color not added to hierarchy error
										else
										{
											styleHnp = _SAB.HierarchyServerSession.GetNodeData(styleRID);
										}
										// END MID Track #5207
									}

									catch (Exception Ex)
									{
										styleRID = Include.NoRID;

										if (Ex.GetType() != typeof(MIDException))
										{
											aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
										}
										else
										{
											MIDException MIDEx = (MIDException)Ex;

											//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
											//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
											aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
											//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
										}
									}
								}
							}

							catch (Exception Ex)
							{
								styleRID = Include.NoRID;

								if (Ex.GetType() != typeof(MIDException))
								{
									aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
								}
								else
								{
									MIDException MIDEx = (MIDException)Ex;

									//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
									//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
									aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
									//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
								}
							}
						}
					}

					catch (Exception Ex)
					{
						styleRID = Include.NoRID;

						if (Ex.GetType() != typeof(MIDException))
						{
							aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
						}
						else
						{
							MIDException MIDEx = (MIDException)Ex;

							//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
							//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
							aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
							//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
						}
					}
				}
			}

			catch (Exception Ex)
			{
				styleRID = Include.NoRID;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
			}

			// Begin MID Track #4958 - JSmith - header description using style ID
//			return styleRID;
			if (styleHnp == null)
			{
				return new HierarchyNodeProfile(Include.NoRID);
			}
			else
			{
                // Begin TT#1218 - JSmith - API - Header Load Performance
                _htStyles.Add(aStyleId, styleHnp); 
                // End TT#1218
				return styleHnp;
			}
			// End MID Track #4958
		}

		// ================
		// Validate a color
		// ================
		private ColorCodeProfile ValidateColor(string aColorCodeId, string aColorCodeName, string aColorCodeGroup, ref EditMsgs aEm)
		{
			bool colorOkay = true;

			ColorCodeProfile colorCodeProfile = null;
			ColorCodeProfile undefColorCodeProfile = null;

			undefColorCodeProfile = new ColorCodeProfile(Include.NoRID);

			try
			{
				// =============================
				// Does color exist on hierarchy
				// =============================
                // Begin TT#1218 - JSmith - API - Header Load Performance
                colorCodeProfile = (ColorCodeProfile)_htColorCodes[aColorCodeId];
                if (colorCodeProfile != null)
                {
                    return colorCodeProfile;
                }
                // End TT#1218

				colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aColorCodeId);

				if (colorCodeProfile.Key == Include.NoRID)
				{
					// ===========================================
					// Color doesn't exist - add it to color table
					// ===========================================
					colorCodeProfile.ColorCodeID = aColorCodeId;
					colorCodeProfile.ColorCodeName = (aColorCodeName != null) ? aColorCodeName : aColorCodeId;
					colorCodeProfile.ColorCodeGroup = aColorCodeGroup;
					colorCodeProfile.ColorCodeChangeType = eChangeType.add;

					try
					{
						colorCodeProfile = _SAB.HierarchyServerSession.ColorCodeUpdate(colorCodeProfile);

						if (colorCodeProfile.Key == Include.NoRID)
						{
							colorOkay = false;
						}
					}

					catch (Exception Ex)
					{
						colorOkay = false;

						if (Ex.GetType() != typeof(MIDException))
						{
							aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
						}
						else
						{
							MIDException MIDEx = (MIDException)Ex;

							//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
							//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
							aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
							//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
						}
					}
				}
			}

			catch (Exception Ex)
			{
				colorOkay = false;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
			}

			if (colorOkay)
			{
                // Begin TT#1218 - JSmith - API - Header Load Performance
                _htColorCodes.Add(aColorCodeId, colorCodeProfile);
                // End TT#1218
				return colorCodeProfile;
			}
			else
			{
				return undefColorCodeProfile;
			}
		}

        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
        // ==========================
        // Quick create a workflow steps
        // ==========================
        private void QuickCreateWorkflowSteps(AllocationProfile ap, HeadersHeader hdrTran, ref EditMsgs aEm)
        {
            try
            {
                if (_methodsDefined)
                {
                    AllocationWorkFlow allocationWorkflow = null;

                    //---Create New WorkFlow-----------------------
                    allocationWorkflow = new AllocationWorkFlow(_SAB, Include.NoRID, Include.SystemUserRID, false);
                    allocationWorkflow.Workflow_Change_Type = eChangeType.add;
                    bool fillWorkflowSuccessfull = FillWorkflow(hdrTran, allocationWorkflow, ref aEm);

                    if (fillWorkflowSuccessfull)
                    {
                        //---Save WorkFlow-------------
                        SaveWorkflow(allocationWorkflow, ref aEm);

                        //---Set Allocation Profile RID To New Allocation Workflow Key---
                        ap.WorkflowRID = allocationWorkflow.Key;

                        //---Set WorkFlow Trigger-------------
                        SetWorkFlowTrigger(ap, hdrTran, ref aEm);
                    }
                }
            }
            catch (Exception Ex)
            {
                if (Ex.GetType() != typeof(MIDException))
                {
                    aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                }
                else
                {
                    MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
            }
        }

        // ==========================
        // Quick modify a workflow steps
        // ==========================
        private void QuickModifyWorkflowSteps(AllocationProfile ap, HeadersHeader hdrTran, ApplicationSessionTransaction aTrans, ref EditMsgs aEm)
        {
            try
            {
                AllocationWorkFlow allocationWorkflow = null;
                // Begin TT#1218 - JSmith - API - Header Load Performance
                //int workFlowRID = Include.UndefinedWorkflowRID;
                int workFlowRID = Include.NoRID;
                // End TT#1218

                WorkflowBaseData wbd = new WorkflowBaseData();

                // Begin TT#1218 - JSmith - API - Header Load Performance
                //DataTable getWorkflow = wbd.GetWorkflow(hdrTran.HeaderId + "(generated)");
                //foreach (DataRow dr in getWorkflow.Rows)
                //{
                //    workFlowRID = Convert.ToInt32(dr["WORKFLOW_RID"], CultureInfo.CurrentUICulture);
                //    break;
                //}
                workFlowRID = wbd.GetWorkflowRID(hdrTran.HeaderId + "(generated)");

                // End TT#1218

                // Begin TT#1218 - JSmith - API - Header Load Performance
                //if (workFlowRID != Include.UndefinedWorkflowRID)
                if (workFlowRID != Include.NoRID)
                // End TT#1218
                {
                    //---Load Old WorkFlow-------------
                    allocationWorkflow = new AllocationWorkFlow(_SAB, workFlowRID, Include.SystemUserRID, false);

                    if ((allocationWorkflow != null) && allocationWorkflow.Filled)
                    {
                        if ((hdrTran.Methods == null) || (hdrTran.Methods.Length == 0))
                        {
                            ap.HeaderChangeType = eChangeType.update;
                            ap.API_WorkflowTrigger = false;
                            ap.API_WorkflowRID = Include.UndefinedWorkflowRID;
                            ap.WorkflowTrigger = false;
                            ap.WorkflowRID = Include.UndefinedWorkflowRID;
                            //rtnCode = ModifyHeader(hdrTran, ap, aTrans);

                            _deleteWorkFlowRID = workFlowRID;

                            allocationWorkflow.Key = Include.UndefinedWorkflowRID;

                            //---Set Allocation Profile RID To New Allocation Workflow Key---
                            ap.WorkflowRID = allocationWorkflow.Key;

                            //---Set WorkFlow Trigger-------------
                            SetWorkFlowTrigger(ap, hdrTran, ref aEm);
                        }
                        else
                        {
                            //---Update WorkFlow-------------
                            allocationWorkflow.Workflow_Change_Type = eChangeType.update;
                            bool fillWorkflowSuccessfull = FillWorkflow(hdrTran, allocationWorkflow, ref aEm);

                            if (fillWorkflowSuccessfull)
                            {
                                //---Save WorkFlow-------------
                                SaveWorkflow(allocationWorkflow, ref aEm);

                                //---Set Allocation Profile RID To New Allocation Workflow Key---
                                ap.WorkflowRID = allocationWorkflow.Key;

                                //---Set WorkFlow Trigger-------------
                                SetWorkFlowTrigger(ap, hdrTran, ref aEm);
                            }
                        }
                    }
                }
                else
                {
                    if (_methodsDefined)
                    {
                        //---Create New WorkFlow-----------------------
                        allocationWorkflow = new AllocationWorkFlow(_SAB, Include.NoRID, Include.SystemUserRID, false);
                        allocationWorkflow.Workflow_Change_Type = eChangeType.add;
                        bool fillWorkflowSuccessfull = FillWorkflow(hdrTran, allocationWorkflow, ref aEm);

                        if (fillWorkflowSuccessfull)
                        {
                            //---Save WorkFlow-------------
                            SaveWorkflow(allocationWorkflow, ref aEm);

                            //---Set Allocation Profile RID To New Allocation Workflow Key---
                            ap.WorkflowRID = allocationWorkflow.Key;

                            //---Set WorkFlow Trigger-------------
                            SetWorkFlowTrigger(ap, hdrTran, ref aEm);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                if (Ex.GetType() != typeof(MIDException))
                {
                    aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                }
                else
                {
                    MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
            }
        }

        private bool SetWorkFlowTrigger(AllocationProfile ap, HeadersHeader hdrTran, ref EditMsgs aEm)
        {
            bool setWorkFlowTriggerSuccess = true;
            try
            {
                bool workFlowTrigger = false;

                //---Set WorkflowTrigger----------------
                if (!hdrTran.WorkflowTriggerSpecified)
                {
                    ap.WorkflowTrigger = false;
                }
                else
                {
                    try
                    {
                        workFlowTrigger = Convert.ToBoolean(hdrTran.WorkflowTrigger, CultureInfo.CurrentUICulture);
                        ap.WorkflowTrigger = workFlowTrigger;
                    }

                    catch (InvalidCastException)
                    {
                        ap.WorkflowTrigger = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                setWorkFlowTriggerSuccess = false;
            }
            return setWorkFlowTriggerSuccess;
        }

        private bool SaveWorkflow(AllocationWorkFlow allocationWorkflow, ref EditMsgs aEm)
        {
            bool saveWorkflowSuccess = true;
            ClientSessionTransaction ClientTransaction = _SAB.ClientServerSession.CreateTransaction();
            try
            {
                string msgText = "";
                ClientTransaction.DataAccess.OpenUpdateConnection();

                allocationWorkflow.Update(ClientTransaction.DataAccess);

                switch (allocationWorkflow.Key)
                {
                    case (int)eGenericDBError.GenericDBError:
                        msgText = MIDText.GetText(eMIDTextCode.msg_GenericMethodInsertError);
                        aEm.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                        return true;
                    case (int)eGenericDBError.DuplicateKey:
                        // Begin TT#1742 - JSmith - Displays wrong message for duplicate workflow
                        //msgText = MIDText.GetText(eMIDTextCode.msg_DuplicateMethod);
                        msgText = MIDText.GetText(eMIDTextCode.msg_DuplicateWorkflow);
                        // End TT#1742
                        aEm.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                        return true;
                    default:
                        break;
                }
                ClientTransaction.DataAccess.CommitData();
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }
            catch (Exception Ex)
            {
                if (ClientTransaction.DataAccess.ConnectionIsOpen)
                {
                    ClientTransaction.DataAccess.Rollback();
                    ClientTransaction.DataAccess.CloseUpdateConnection();
                }
                aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                saveWorkflowSuccess = false;
            }
            return saveWorkflowSuccess;
        }

        private bool GetActions(ref EditMsgs aEm)
        {
            bool getActionsSuccess = true;
            try
            {
                // Begin TT#1218 - JSmith - API - Header Load Performance
                if (_actionHashTable.Count > 0)
                {
                    return true;
                }
                // End TT#1218

                // Begin TT#768 - JSmith - Receive Duplicate insert on Workflow_Header_FK1
                _actionHashTable.Clear();
                // End TT#768

                // Begin TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
                //DataTable dtActions = MIDText.GetLabels((int) eAllocationActionType.StyleNeed, (int)eAllocationActionType.ApplyAPI_Workflow);
                // begin TT#843 - New Size Constraint Balance
                //DataTable dtActions = MIDText.GetLabels((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.ReapplyTotalAllocation);
                // begin TT#794 - New Size Balance for Wet Seal
                //DataTable dtActions = MIDText.GetLabels((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceSizeWithConstraints);  // TT#1391 - JEllis - Balance Size With Constraint Other Options
                //DataTable dtActions = MIDText.GetLabels((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceSizeBilaterally);
                // end TT#794 - New Size Balance for Wet Seal
				DataTable dtActions = MIDText.GetLabels((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
                // end TT#843 - New Size Constraint Balance
                // End TT#785 
                DataRow dr;
                Hashtable removeEntry = new Hashtable();
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutDetailPackAllocation), eAllocationActionType.BackoutDetailPackAllocation);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.ChargeSizeIntransit), eAllocationActionType.ChargeSizeIntransit);
                removeEntry.Add(Convert.ToInt32(eAllocationActionType.DeleteHeader), eAllocationActionType.DeleteHeader);
                 // Begin TT#1218 - JSmith - API - Header Load Performance
                //if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                if (_globalOptions.AppConfig.SizeInstalled)
                // End TT#1218
                {
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeAllocation), eAllocationActionType.BackoutSizeAllocation);
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeIntransit), eAllocationActionType.BackoutSizeIntransit);
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeNoSubs), eAllocationActionType.BalanceSizeNoSubs);
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithSubs), eAllocationActionType.BalanceSizeWithSubs);
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceived), eAllocationActionType.BreakoutSizesAsReceived);
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeWithConstraints); // TT#843 - New Size Constraint Balance
                    // BEGIN TT#1726 - AGallagher - During a header load, receive a Item has already been added. Key in dictionary error.
                    //removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeBilaterally); // TT#794 - New Size Balance for Wet Seal
				    //removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeBilaterally), eAllocationActionType.BalanceSizeBilaterally); // TT#794 - New Size Balance for Wet Seal
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceivedWithConstraints), eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
                    // END TT#1726 - AGallagher - During a header load, receive a Item has already been added. Key in dictionary error.
                }
                int codeValue;
                for (int i = dtActions.Rows.Count - 1; i >= 0; i--)
                {
                    dr = dtActions.Rows[i];
                    codeValue = Convert.ToInt32(dr["TEXT_CODE"]);
                    if (removeEntry.Contains(codeValue))
                    {
                        dtActions.Rows.Remove(dr);
                    }
                    else if (!Enum.IsDefined(typeof(eAllocationActionType), (eAllocationActionType)codeValue))
                        dtActions.Rows.Remove(dr);
                }

                for (int i = dtActions.Rows.Count - 1; i >= 0; i--)
                {
                    dr = dtActions.Rows[i];
                    _actionHashTable.Add(Convert.ToString(dr["TEXT_VALUE"]).ToUpper().Replace(" ", ""), Convert.ToInt32(dr["TEXT_CODE"]));
                }
            }
            catch (Exception Ex)
            {
                aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                getActionsSuccess = false;
            }

            return getActionsSuccess;
        }

        private bool FillWorkflow(HeadersHeader hdrTran, AllocationWorkFlow allocationWorkflow, ref EditMsgs aEm)
        {
            bool fillWorkflowSuccess = true;
            try
            {
                int workFlowStepKey = 0;
                AllocationWorkFlowStep awfs = null;
                WorkflowMethodManager wmManager;
                int methodRID = Include.NoRID;
                bool reviewFlag = false;
                bool useSystemTolerancePercent = false;
                double tolerancePercent = 0.0;
                int storeFilter = Include.UndefinedStoreFilter;
                ApplicationBaseAction method = null;
                GetMethods getMethods = new GetMethods(_SAB);
                GeneralComponent generalComponent = new GeneralComponent(eGeneralComponentType.Total);

                allocationWorkflow.WorkFlowType = eWorkflowType.Allocation;
                allocationWorkflow.WorkFlowName = hdrTran.HeaderId + "(generated)";
                allocationWorkflow.WorkFlowDescription = hdrTran.HeaderId + "(generated)";
                allocationWorkflow.UserRID = Include.SystemUserRID;
                allocationWorkflow.StoreFilterRID = storeFilter;
                allocationWorkflow.Workflow_Steps.Clear();

                //---Loop Through Methods In XML File And Add Workflow Steps To AllocationWorkflow----------
                foreach (HeadersHeaderMethod hdrTranMethod in hdrTran.Methods)
                {
                    eMethodType methodType = eMethodType.NotSpecified;
                    switch (hdrTranMethod.Type.ToString().ToUpper())
                    {
                        case "GENERALALLOCATION":
                            methodType = eMethodType.GeneralAllocation;
                            break;
                        case "ALLOCATIONOVERRIDE":
                            methodType = eMethodType.AllocationOverride;
                            break;
                        case "RULE":
                            methodType = eMethodType.Rule;
                            break;
                        case "VELOCITY":
                            methodType = eMethodType.Velocity;
                            break;
                        case "FILLSIZES":
                            methodType = eMethodType.FillSizeHolesAllocation;
                            break;
                        case "BASISSIZE":
                            methodType = eMethodType.BasisSizeAllocation;
                            break;
                        case "SIZENEED":
                            methodType = eMethodType.SizeNeedAllocation;
                            break;
                        case "ACTION":
                            foreach (DictionaryEntry de in _actionHashTable)
                            {
                                if (Convert.ToString(de.Key) == hdrTranMethod.Name.ToString().ToUpper().Replace(" " , ""))
                                {
                                    methodType = (eMethodType)Convert.ToInt32(Convert.ToInt32(de.Value), CultureInfo.CurrentUICulture);
                                    break;
                                }
                            }
                            break;
                        default:
                            fillWorkflowSuccess = false;
                            break;
                    }

                    if (hdrTranMethod.Type.ToString().ToUpper() != "ACTION")
                    {
                        wmManager = new WorkflowMethodManager(_SAB.ClientServerSession.UserRID);
                        DataTable DtMethods = wmManager.GetMethodList(hdrTranMethod.Name, methodType, Include.GlobalUserRID);

                        if (DtMethods.Rows.Count == 0)
                        {
                            //---Method Does NOT Exist-----------------------------
                            string msgText = MIDText.GetText(eMIDTextCode.msg_hl_InvalidMethodName);
                            msgText = msgText.Replace("{0}", hdrTranMethod.Name);
                            msgText = msgText.Replace("{1}", hdrTranMethod.Type.ToString());
                            msgText = msgText + System.Environment.NewLine;
                            aEm.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                            fillWorkflowSuccess = false;
                            break;
                        }
                        else
                        {
                            foreach (DataRow dr in DtMethods.Rows)
                            {
                                methodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
                                if (methodRID == Include.NoRID)
                                {
                                    //---Method Does NOT Exist-----------------------------
                                    string msgText = MIDText.GetText(eMIDTextCode.msg_hl_InvalidMethodName);
                                    msgText = msgText.Replace("{0}", hdrTranMethod.Name);
                                    msgText = msgText.Replace("{1}", hdrTranMethod.Type.ToString());
                                    msgText = msgText + System.Environment.NewLine;
                                    aEm.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                                    fillWorkflowSuccess = false;
                                    break;
                                }
                                else
                                {
                                    //---Method Exist--------------------------------------
                                    // Begin TT#1218 - JSmith - API - Header Load Performance
                                    //method = getMethods.GetMethod(methodRID, methodType);
                                    method = (ApplicationBaseAction)_htMethods[methodRID];
                                    if (method == null)
                                    {
                                        method = getMethods.GetMethod(methodRID, methodType);
                                        _htMethods.Add(methodRID, method); 
                                    }
                                    // Begin TT#1218
                                    awfs = new AllocationWorkFlowStep(method, generalComponent, reviewFlag,
                                                                      useSystemTolerancePercent, tolerancePercent,
                                                                      storeFilter, workFlowStepKey);

                                    allocationWorkflow.Workflow_Steps.Add(awfs);
                                    ++workFlowStepKey;
                                }
                            }
                        }
                    }
                    else 
                    {
                        method = new AllocationAction(methodType);
                        awfs = new AllocationWorkFlowStep(method, generalComponent, reviewFlag,
                                                          useSystemTolerancePercent, tolerancePercent,
                                                          storeFilter, workFlowStepKey);

                        allocationWorkflow.Workflow_Steps.Add(awfs);
                        ++workFlowStepKey;
                    }

                    if (!fillWorkflowSuccess)
                    {
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                fillWorkflowSuccess = false;
            }
            return fillWorkflowSuccess;
        }
        // END MID Track #6336

		// =================
		// Quick add a color
		// =================
		private int QuickAddColor(int aStyleHnRID, string aColorCodeId, string aColorCodeDesc, ref EditMsgs aEm)
		{
			int colorHnRID = Include.NoRID;

			//Begin TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
			//if (aColorCodeDesc == "" || aColorCodeDesc == null)
			//{
			//    aColorCodeDesc = aColorCodeId;
			//}

			//End TT#1435 - JScott - Color Description Shows as Color Code in the Hierarchy and Header
			try
			{
				// create temp EditMsgs object for adding node
				EditMsgs tempem = new EditMsgs();
				if (aStyleHnRID != Include.NoRID)
				{
					colorHnRID = _hierMaint.QuickAdd(ref tempem, aStyleHnRID, aColorCodeId, aColorCodeDesc);
					if (tempem.ErrorFound)
					{
						// transfer errors to main error class
						for (int e=0; e<tempem.EditMessages.Count; e++)
						{
							EditMsgs.Message emm = (EditMsgs.Message) tempem.EditMessages[e];
							aEm.AddMsg(emm.messageLevel, emm.code, emm.msg, emm.module);
						}
					}
				}
			}

			catch (Exception Ex)
			{
				colorHnRID = Include.NoRID;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
			}

			return colorHnRID;
		}

		// ===============
		// Validate a size
		// ===============
		private SizeCodeProfile ValidateSize(string aSizeCodeId, string aSizeCodeName,
											 string aSizeCodePrimary, string aSizeCodeSecondary,
											 string aSizeCodeProductCategory,
											 ref EditMsgs aEm)
		{
			bool sizeOkay = true;

			SizeCodeProfile sizeCodeProfile = null;
			SizeCodeProfile undefSizeCodeProfile = null;

			undefSizeCodeProfile = new SizeCodeProfile(Include.NoRID);

			try
			{
				// ============================
				// Does size exist on hierarchy
				// ============================
                // Begin TT#1218 - JSmith - API - Header Load Performance
                sizeCodeProfile = (SizeCodeProfile)_htSizeCodes[aSizeCodeId];
                if (sizeCodeProfile != null)
                {
                    return sizeCodeProfile;
                }
                // End TT#1218

				sizeCodeProfile = _SAB.HierarchyServerSession.GetSizeCodeProfile(aSizeCodeId);

				if (sizeCodeProfile.Key == Include.NoRID)
				{
					// =========================================
					// Size doesn't exist - add it to size table
					// =========================================
					sizeCodeProfile.SizeCodeID = aSizeCodeId;
					//Begin Track #4833 - JSmith - Adding size code without product category
//					sizeCodeProfile.SizeCodeName = (aSizeCodeName != null) ? aSizeCodeName : aSizeCodeId;
					sizeCodeProfile.SizeCodeName = (aSizeCodeName != null) ? aSizeCodeName : null;
					//End Track #4833
					sizeCodeProfile.SizeCodePrimary = (aSizeCodePrimary != null) ? aSizeCodePrimary : sizeCodeProfile.SizeCodeName;
					sizeCodeProfile.SizeCodeSecondary = aSizeCodeSecondary;
					sizeCodeProfile.SizeCodeProductCategory = aSizeCodeProductCategory;
					sizeCodeProfile.SizeCodeName = Include.GetSizeName(sizeCodeProfile.SizeCodePrimary, sizeCodeProfile.SizeCodeSecondary, sizeCodeProfile.SizeCodeID);
					sizeCodeProfile.SizeCodeChangeType = eChangeType.add;

					try
					{
						sizeCodeProfile = _SAB.HierarchyServerSession.SizeCodeUpdate(sizeCodeProfile);

						if (sizeCodeProfile.Key == Include.NoRID)
						{
							sizeOkay = false;
						}
					}

					catch (SizePrimaryRequiredException)
					{
						sizeOkay = false;
						aEm.AddMsg(eMIDMessageLevel.Edit, MIDText.GetText(eMIDTextCode.msg_SizePrimaryRequired), GetType().Name);
					}
					catch (SizeCatgPriSecNotUniqueException exc)
					{
						sizeOkay = false;
						string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeCatgPriSecNotUnique, false);
						message = message.Replace("{0}", sizeCodeProfile.SizeCodeProductCategory);
						message = message.Replace("{1}", sizeCodeProfile.SizeCodePrimary);
						if (sizeCodeProfile.SizeCodeSecondary != null &&
							sizeCodeProfile.SizeCodeSecondary.Trim().Length > 1)
						{
							message = message.Replace("{2}", sizeCodeProfile.SizeCodeSecondary);
						}
						else
						{
							message = message.Replace("{2}", MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize));
						}
						message = message.Replace("{3}", exc.Message);
						aEm.AddMsg(eMIDMessageLevel.Edit, message, GetType().Name);
					}
					//Begin Track #4833 - JSmith - Adding size code without product category
					catch (SizeCodeNotValidException exc)
					{
						sizeOkay = false;
						aEm.AddMsg(eMIDMessageLevel.Edit, exc.Message, GetType().Name);
					}
					//End Track #4833
					catch (Exception Ex)
					{
						sizeOkay = false;

						if (Ex.GetType() != typeof(MIDException))
						{
							aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
						}
						else
						{
							MIDException MIDEx = (MIDException)Ex;

							//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
							//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
							aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
							//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
						}
					}
				}
			}

			catch (Exception Ex)
			{
				sizeOkay = false;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
			}

			if (sizeOkay)
			{
                // Begin TT#1218 - JSmith - API - Header Load Performance
                _htSizeCodes.Add(aSizeCodeId, sizeCodeProfile);
                // End TT#1218
				return sizeCodeProfile;
			}
			else
			{
				return undefSizeCodeProfile;
			}
		}

        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
        // =====================================
        // Validate Units Per Carton vs Multiple
        // =====================================
        private bool ValidUnitsPerCarton(AllocationProfile ap, string transUnitsPerCarton, ref EditMsgs aEm, ref int aUnitsPerCarton)  // TT#1703-MD - stodd - Error when Units Per Carton field is blank
        {
            bool isValid = true;
            string msgText;		// TT#1703-MD - stodd - Error when Units Per Carton field is blank
            try
            {
				// Begin TT#1703-MD - stodd - Error when Units Per Carton field is blank
                try
                {
                    if (transUnitsPerCarton.Trim() == string.Empty)
                    {
                        aUnitsPerCarton = 0;
                    }
                    else
                    {
                        aUnitsPerCarton = int.Parse(transUnitsPerCarton.Trim());
                    }
                }
                catch
                {
                    isValid = false;
                    msgText = string.Format(MIDText.GetText(eMIDTextCode.msg_al_UnitsPerCartonInvalid), ap.HeaderID);
                    aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                }
				// End TT#1703-MD - stodd - Error when Units Per Carton field is blank
                if (isValid)	// TT#1703-MD - stodd - Error when Units Per Carton field is blank
                {
                    switch (ap.HeaderAllocationStatus)
                    {
                        // Begin TT#4729 - stodd - Request to Allow Updates to Units Per Carton
                        //case eHeaderAllocationStatus.AllInBalance:
                        //case eHeaderAllocationStatus.AllocatedInBalance:
                        //case eHeaderAllocationStatus.AllocatedOutOfBalance:
                        //case eHeaderAllocationStatus.PartialSizeInBalance:
                        //case eHeaderAllocationStatus.PartialSizeOutOfBalance:
                        //case eHeaderAllocationStatus.SizesOutOfBalance:
                        // End TT#4729 - stodd - Request to Allow Updates to Units Per Carton
                        case eHeaderAllocationStatus.ReleaseApproved:
                        case eHeaderAllocationStatus.Released:
                        case eHeaderAllocationStatus.InUseByMultiHeader:
                            isValid = false;
                            msgText = string.Format(MIDText.GetText(eMIDTextCode.msg_al_UnitsPerCartonInvalidForStatus), ap.HeaderID, ap.HeaderAllocationStatus.ToString());
                            aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            break;
                        default:
                            // Begin TT#5046 - JSmith - Updating Units Per Carton on Group Allocation Skus
                            //if (ap.HeaderType != eHeaderType.Assortment && ap.HeaderType != eHeaderType.Placeholder && ap.AsrtRID > 0)    // this is a real header in an Assorment
                            if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                            // End TT#5046 - JSmith - Updating Units Per Carton on Group Allocation Skus
                            {
                                isValid = false;
                                msgText = string.Format(MIDText.GetText(eMIDTextCode.msg_al_UnitsPerCartonInvalidForType), ap.HeaderID);
                                aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            }
                            break;
                    }
                }

                if (isValid)
                {
                    switch (ap.HeaderType)
                    {
                        case eHeaderType.MultiHeader:
                        case eHeaderType.Assortment:
                        case eHeaderType.Placeholder:
                        case eHeaderType.IMO:
                            isValid = false;
                            msgText = string.Format(MIDText.GetText(eMIDTextCode.msg_al_UnitsPerCartonInvalidForType), ap.HeaderID);
                            aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
                            break;
                    }
                }
                if (isValid)
                {
                    if (ap.AllocationMultiple > 1)
                    {
                        if (aUnitsPerCarton < ap.AllocationMultiple || (aUnitsPerCarton % ap.AllocationMultiple > 0))
                        {
                            aEm.AddMsg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_al_UnitsPerCartonNotMultiple), GetType().Name);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                isValid = false;

                if (Ex.GetType() != typeof(MIDException))
                {
                    aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
                }
                else
                {
                    MIDException MIDEx = (MIDException)Ex;
                    aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
                }
            }
            return isValid;
        }
        // End TT#1652-MD

		// ================
		// Quick add a size
		// ================
		private int QuickAddSize(int aColorHnRID, string aSizeCodeId, string aSizeCodeDesc, ref EditMsgs aEm)
		{
			int sizeHnRID = Include.NoRID;

			try
			{
				// create temp EditMsgs object for adding node
				EditMsgs tempem = new EditMsgs();
				if (aColorHnRID != Include.NoRID)
				{
					sizeHnRID = _hierMaint.QuickAdd(ref tempem, aColorHnRID, aSizeCodeId, aSizeCodeDesc);
					if (tempem.ErrorFound)
					{
						// transfer errors to main error class
						for (int e=0; e<tempem.EditMessages.Count; e++)
						{
							EditMsgs.Message emm = (EditMsgs.Message) tempem.EditMessages[e];
							aEm.AddMsg(emm.messageLevel, emm.code, emm.msg, emm.module);
						}
					}
				}
			}

			catch (Exception Ex)
			{
				sizeHnRID = Include.NoRID;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
					//aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					aEm.AddMsg(Include.TranslateErrorLevel(MIDEx.ErrorLevel), MIDEx.ErrorMessage, GetType().Name);
					//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				}
			}

			return sizeHnRID;
		}

		// ==========================
		// Log characteristics errors
		// ==========================
		private void SetCharacteristicMessages(string aHeaderId, HeadersHeaderCharacteristic aHdrTranChars, int acharsRtn, ref EditMsgs aEm)
		{
			string msgText = null;

			switch (acharsRtn)
			{
				case -1:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += " - unknown error occured during processing" + System.Environment.NewLine;
					break;
				case 1:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += ", Value: " + aHdrTranChars.Value;
					msgText += " - could NOT be converted to datetime value" + System.Environment.NewLine;
					break;
				case 2:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += ", Value: " + aHdrTranChars.Value;
					msgText += " - could NOT be converted to number value" + System.Environment.NewLine;
					break;
				case 3:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += ", Value: " + aHdrTranChars.Value;
					msgText += " - could NOT be converted to dollar value" + System.Environment.NewLine;
					break;
				case 4:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += ", Value: " + aHdrTranChars.Value;
					msgText += " - characteristic data type is unknown - please verify characteristic exists" + System.Environment.NewLine;
					break;

                //Begin TT#238 - Header Characteristic Enhancement - modify transaction accepted a change to a CharType for a characteristic from "Text" to "Date"
                case 5:
                    msgText = "Allocation Header " + aHeaderId;
                    msgText += " - Characteristic: " + aHdrTranChars.Name;
                    msgText += ", Value: " + aHdrTranChars.Value;
                    msgText += " - characteristic data type cannot be changed. A change like that would corrupt or loose data." + System.Environment.NewLine;
                    break;
                //End TT#238 - Header Characteristic Enhancement - modify transaction accepted a change to a CharType for a characteristic from "Text" to "Date"

				case 10:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += " - could NOT be found" + System.Environment.NewLine;
					break;
				case 20:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += ", Value: " + aHdrTranChars.Value;
					msgText += " - value NOT found in list" + System.Environment.NewLine;
					break;
				// Begin - John Smith - Linked headers
				case 30:
					msgText = "Allocation Header " + aHeaderId;
					msgText += " - Characteristic: " + aHdrTranChars.Name;
					msgText += ", Value: " + aHdrTranChars.Value;
					msgText += " - link characteristic can not be changed for the current status" + System.Environment.NewLine;
					break;
				// End - Linked headers
				default:
					break;
			}

			if (msgText.Length > 0)
			{
				aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
			}
		}

		// ============================
		// Write the header load status
		// ============================
		private void WriteHeaderLoadStatus(string aHeaderID, string aHeaderAction, bool aLoadStatus)
		{

			// ==============================
			// Write the HeaderStatus element
			// ==============================
			_xmlWriter.WriteStartElement("HeaderStatus");

			// ===================
			// Write the Header ID
			// ===================
			_xmlWriter.WriteStartAttribute(null, "HeaderId", null);

			if (aHeaderID != "" && aHeaderID != null)
			{
				_xmlWriter.WriteString(aHeaderID);
			}
			else
			{
				_xmlWriter.WriteString("[No Header ID]");
			}

			_xmlWriter.WriteEndAttribute();

			// =======================
			// Write the Header Action
			// =======================
			_xmlWriter.WriteStartAttribute(null, "HeaderAction", null);

			if (aHeaderAction != "" && aHeaderAction != null)
			{
				_xmlWriter.WriteString(aHeaderAction);
			}
			else
			{
				_xmlWriter.WriteString("[No Header Action]");
			}

			_xmlWriter.WriteEndAttribute();

			// =====================
			// Write the Load Status
			// =====================
			_xmlWriter.WriteStartAttribute(null, "LoadStatus", null);

			if (aLoadStatus)
			{
				_xmlWriter.WriteString("true");
			}
			else
			{
				_xmlWriter.WriteString("false");
			}

			_xmlWriter.WriteEndAttribute();

			// ==============================
			// Close the HeaderStatus element
			// ==============================
			_xmlWriter.WriteEndElement();
		}
	}

	public class StringTrim
	{
		private string myString;
		public StringTrim()
		{
			myString = null;
		}

		public string StringProperty
		{
			get
			{
				return myString;
			}
		}
	}

}
