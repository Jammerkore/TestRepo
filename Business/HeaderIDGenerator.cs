using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

//using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public class HeaderIDGenerator
    {
        private SessionAddressBlock _sab;
        private string _colorCodeId = null;
        private string _packName = null;		
        private string _bulkOrPack = string.Empty;
        private EditMsgs _em = null;

        // This list indicates which keys are from the header. Any other keys are considered to be a Header Characteristic.
        //private static List<string> _headerKeyList = new List<string>() { "STYLEID", "DISTCENTER", "VENDOR", "PURCHASEORDER", "VSWID"};
        // User selected header ID key list
        private static List<string> _headerIdKeyList;
        private static List<string> _headerKeysToMatchList;
        private const string _color = "COLOR";
        private const string _pack = "PACK NAME";	// TT#1605-MD - stodd - Adding 'sequence' to other header ID keys causes sequence to replace all other keys.
        private const string _sequence = "SEQUENCE";
        private HeaderMatching _headerMatching;
        private int _headerIdSequenceLength;
        private string _headerIdDelimiter;
        private int _iheaderIdSequenceLength = 5;  // TT#1966-MD - JSmith- DC Fulfillment
        private string _sHeaderDelimiter = "-";    // TT#1966-MD - JSmith- DC Fulfillment

        private Dictionary<int, HierarchyNodeProfile> _dicHierarchyNodes = new Dictionary<int,HierarchyNodeProfile>();
        private Dictionary<int, ColorCodeProfile> _dicColorCodes = new Dictionary<int,ColorCodeProfile>();
        private Dictionary<int, SizeCodeProfile> _dicSizeCodes = new Dictionary<int,SizeCodeProfile>();

        public HeaderIDGenerator(SessionAddressBlock sab)
		{
            _sab = sab;

            // Begin TT#1966-MD - JSmith- DC Fulfillment
            // Read information needed to generate Master Header IDs.
            
            string strHeaderIdSequenceLength = MIDConfigurationManager.AppSettings["HeaderIdSequenceLength"];
            if (strHeaderIdSequenceLength != null)
            {
                try
                {
                    _iheaderIdSequenceLength = Convert.ToInt32(strHeaderIdSequenceLength);
                }
                catch
                {
                    _iheaderIdSequenceLength = 7;    // Default is 7
                }
            }

            
            string headerIdDelimiter = MIDConfigurationManager.AppSettings["HeaderIdDelimiter"];
            if (headerIdDelimiter != null)
            {
                try
                {
                    _sHeaderDelimiter = headerIdDelimiter;
                }
                catch
                {

                }
            }
            // End TT#1966-MD - JSmith- DC Fulfillment
		}

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        //public eReturnCode GetHeaderId(HeadersHeader hdrTran, int transNo, ref string aHeaderId, ref bool isResetRemove, List<string> headerIdKeyList, List<string> headerKeysToMatchList, int headerIdSequenceLength, string headerIdDelimiter, ref EditMsgs em)
		// Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
		//public eReturnCode GetHeaderId(HeadersHeader hdrTran, int transNo, ref string aHeaderId, ref bool isResetRemove, List<string> headerIdKeyList, List<string> headerKeysToMatchList, int headerIdSequenceLength, string headerIdDelimiter, ref EditMsgs em, bool bLookForMatchingHeader = true)
        public eReturnCode GetHeaderId(Session aSession, HeadersHeader hdrTran, int transNo, ref string aHeaderId, ref bool isResetRemove, List<string> headerIdKeyList, List<string> headerKeysToMatchList, int headerIdSequenceLength, string headerIdDelimiter, ref EditMsgs em, bool bLookForMatchingHeader = true)
		// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
        // End TT#1966-MD - JSmith- DC Fulfillment
        {
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            if (headerIdSequenceLength == 0)
            {
                headerIdSequenceLength = _iheaderIdSequenceLength;
            }
            if (headerIdDelimiter == null)
            {
                headerIdDelimiter = _sHeaderDelimiter;
            }
            // End TT#1966-MD - JSmith- DC Fulfillment
            _em = em;
            eReturnCode rc = eReturnCode.successful;
            aHeaderId = hdrTran.HeaderId;
            _headerIdKeyList = headerIdKeyList;
            _headerKeysToMatchList = headerKeysToMatchList;
            _headerIdSequenceLength = headerIdSequenceLength;
            _headerIdDelimiter = headerIdDelimiter;
            string errMsg = string.Empty;

            _headerMatching = new HeaderMatching(_sab, _headerIdSequenceLength);

            //========================================================================
            // Header ID is in transaction.
            // This process just returns it back to the calling program.
            //========================================================================
            if (hdrTran.HeaderId != null && hdrTran.HeaderId.Trim() != string.Empty)
            {
                //errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranContainsHeaderId);
                //errMsg = errMsg.Replace("{0}", transNo.ToString());
                //errMsg = errMsg.Replace("{1}", aHeaderId);
                //errMsg += System.Environment.NewLine;
                //AddMessage(errMsg, eMIDTextCode.msg_HeaderTranContainsHeaderId);
                return rc;
            }

            //====================================================================================================
            // Looks for a match against the current system headers
            // If a header ID cannot be determined then maxSequence is used when creating the new header ID.
            //====================================================================================================
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            //rc = _headerMatching.FindMatchingHeader(hdrTran, ref aHeaderId, _headerKeysToMatchList, transNo);
            if (bLookForMatchingHeader)
            {
                // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
				//rc = _headerMatching.FindMatchingHeader(hdrTran, ref aHeaderId, _headerKeysToMatchList, transNo);
				rc = _headerMatching.FindMatchingHeader(aSession, hdrTran, ref aHeaderId, _headerKeysToMatchList, transNo);
				// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
            }
            // End TT#1966-MD - JSmith- DC Fulfillment

            if (rc == eReturnCode.successful)
            {
                if (string.IsNullOrEmpty(aHeaderId))
                {
                    //=====================================================================================
                    // ValidateHeaderHeaderIdKeys() 
                    //   sets _colorCodeId to the color ID on header and sets _bulkOrPack, if applicable.
                    //   Sets _packName to pack name on header, if applicable.
                    //=====================================================================================
                    bool isValid = ValidateHeaderHeaderIdKeys(hdrTran, transNo);
                    if (isValid)
                    {
                        rc = GenerateHeaderId(hdrTran, ref aHeaderId, transNo);
                    }
                    else
                    {
                        rc = eReturnCode.editErrors;
                    }
                }
            }


            //DataTable dt = _sab.HeaderServerSession.GetMatchingHeader(styleRid, hdrTran.DistCenter, hdrTran.PurchaseOrder, (int)eHeaderType, _colorCodeId, _bulkOrPack, hdrTran.HeaderAction.ToString().ToUpper());

            //if (dt.Rows.Count == 0)
            //{
            //    if (hdrTran.HeaderAction.ToString().ToUpper() == "REMOVE")
            //    {
            //        errMsg = MIDText.GetText(eMIDTextCode.msg_FoundNoHeaderMatches);
            //        errMsg = errMsg.Replace("{0}", transNo.ToString());
            //        errMsg += System.Environment.NewLine;
            //        AddMessage(errMsg, eMIDTextCode.msg_FoundNoHeaderMatches);
            //        rc = eReturnCode.editErrors;
            //    }
            //    else
            //    {
            //        int seq = _sab.HeaderServerSession.GetNextHeaderSequenceNumber();
            //        aHeaderId = seq.ToString("000000000");
            //    }
            //}
            //else if (dt.Rows.Count == 1)
            //{
            //    aHeaderId = dt.Rows[0]["HDR_ID"].ToString();

            //    if (hdrTran.HeaderAction.ToString().ToUpper() == "REMOVE")
            //    {
            //        DateTime releasedApprovedDate = Include.UndefinedDate;
            //        DateTime releaseDate = Include.UndefinedDate;
            //        bool shippingComplete = false;

            //        ConvertdataRowToFields(dt.Rows[0], ref releasedApprovedDate, ref releaseDate, ref shippingComplete);

            //        if ((releasedApprovedDate != Include.UndefinedDate || releaseDate != Include.UndefinedDate) && !shippingComplete)
            //        {
            //            //============================================
            //            // Do RESET and then continue with REMOVE
            //            //============================================
            //            hdrTran.HeaderId = aHeaderId;
            //            hdrTran.HeaderAction = HeadersHeaderHeaderAction.Reset;
            //            isResetRemove = true;
            //            //try
            //            //{
            //            //    rc = ProcessOneHeaderDelegate(hdrTran);
            //            //}
            //            //finally
            //            //{
            //            //    modifyOnReset = true;
            //            //}
            //            //if (rc == eReturnCode.successful)
            //            //{
            //            //    hdrTran.HeaderAction = HeadersHeaderHeaderAction.Remove;
            //            //    errMsg = MIDText.GetText(eMIDTextCode.msg_IntermediateResetSuccessful);
            //            //    errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            //    errMsg = errMsg.Replace("{1}", aHeaderId);
            //            //    errMsg += System.Environment.NewLine;
            //            //    AddMessage(errMsg, eMIDTextCode.msg_IntermediateResetSuccessful);
            //            //}
            //            //else
            //            //{
            //            //    errMsg = MIDText.GetText(eMIDTextCode.msg_IntermediateResetFailed);
            //            //    errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            //    errMsg = errMsg.Replace("{1}", aHeaderId);
            //            //    errMsg += System.Environment.NewLine;
            //            //    AddMessage(errMsg, eMIDTextCode.msg_IntermediateResetFailed);
            //            //}
            //        }
            //    }
            //    // Begin TT#4025 - JSmith - Workup Total Buy - Release Issue
            //    // put total units in transaction so allocated quantities not deleted
            //    else if (hdrTran.HeaderType == HeadersHeaderHeaderType.WorkupTotalBuy &&
            //        !hdrTran.TotalUnitsSpecified)
            //    {
            //        hdrTran.TotalUnitsSpecified = true;
            //        hdrTran.TotalUnits = Convert.ToInt32(dt.Rows[0]["UNITS_RECEIVED"]);
            //    }
            //    // End TT#4025 - JSmith - Workup Total Buy - Release Issue
            //}
            //else // Possible Error. Multiple matches
            //{
            //    if (hdrTran.HeaderAction.ToString().ToUpper() == "REMOVE")
            //    {
            //        // Sometimes it's possible to process on multiple matches...
            //        rc = HandleMutipleRowsForRemove(ref hdrTran, dt, transNo);
            //        if (rc == eReturnCode.successful)
            //        {
            //            aHeaderId = hdrTran.HeaderId;
            //        }
            //    }
            //    else
            //    {
            //        errMsg = MIDText.GetText(eMIDTextCode.msg_FoundMultipleHeaderMatches);
            //        errMsg = errMsg.Replace("{0}", transNo.ToString());
            //        errMsg += System.Environment.NewLine;
            //        AddMessage(errMsg, eMIDTextCode.msg_FoundMultipleHeaderMatches);
            //        rc = eReturnCode.editErrors;
            //    }
            //}
            return rc;
        }

        /// <summary>
        /// hdrTran validation.
        /// </summary>
        /// <param name="hdrTran"></param>
        /// <param name="transNo"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool ValidateHeaderHeaderIdKeys(HeadersHeader hdrTran, int transNo)
        {
            bool isValid = true;
            string errMsg = string.Empty;
            string aHeaderKeyUpper;
            _bulkOrPack = string.Empty;
            _colorCodeId = string.Empty;
            _packName = string.Empty;

            //if (hdrTran.HeaderAction != HeadersHeaderHeaderAction.Remove && hdrTran.HeaderAction != HeadersHeaderHeaderAction.Modify)
            //{
            //    errMsg = MIDText.GetText(eMIDTextCode.msg_InvalidHeaderAction);
            //    errMsg = errMsg.Replace("{0}", transNo.ToString());
            //    errMsg = errMsg.Replace("{1}", hdrTran.HeaderAction.ToString());
            //    errMsg += System.Environment.NewLine;
            //    AddMessage(errMsg, eMIDTextCode.msg_InvalidHeaderAction);
            //    return false;
            //}

            //if (!ValidHeaderType(hdrTran.HeaderType.ToString().ToUpper()))
            //{
            //    errMsg = MIDText.GetText(eMIDTextCode.msg_FoundInvalidHeaderType);
            //    errMsg = errMsg.Replace("{0}", transNo.ToString());
            //    errMsg = errMsg.Replace("{1}", hdrTran.HeaderType.ToString());
            //    errMsg += System.Environment.NewLine;
            //    AddMessage(errMsg, eMIDTextCode.msg_FoundInvalidHeaderType);
            //    return false;
            //}

            foreach (string headerIdKey in _headerIdKeyList)
            {
                if (string.IsNullOrEmpty(headerIdKey))
                {
                    continue;
                }
                aHeaderKeyUpper = headerIdKey.ToUpper();
                eHeaderMatchingKeyType aHeaderKeyType = GetHeaderMatchKeyType(headerIdKey);
                switch (aHeaderKeyType)
                {
                    case eHeaderMatchingKeyType.Sequence:
                    case eHeaderMatchingKeyType.Text:  // TT#1966-MD - JSmith- DC Fulfillment
                        {
                            // always valid
                            break;
                        }

                    case eHeaderMatchingKeyType.HeaderField:
                        {
                            isValid = ValidHeaderKeyField(headerIdKey, hdrTran, transNo, ref errMsg);
                            break;
                        }

                    case eHeaderMatchingKeyType.Color:
                        {
                            isValid = ValidHeaderColorKeyField(headerIdKey, hdrTran, transNo, ref errMsg);
                            break;
                        }

                    case eHeaderMatchingKeyType.Pack:
                        {
                            isValid = ValidHeaderPackKeyField(headerIdKey, hdrTran, transNo, ref errMsg);
                            break;
                        }

                    case eHeaderMatchingKeyType.Characteristic:
                        {
                            if (hdrTran.Characteristic == null)
                            {
                                //string msgText = "Processing Transaction #" + transNo.ToString() + ".";
                                //msgText += System.Environment.NewLine;
                                //msgText += "A Header Characteristic (" + aHeaderKeyType + ") was chosen to be used to generate the Header ID, but this transaction contains no characteristics.";
                                //_sab.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                isValid = true;
                            }
                            else
                            {
                                isValid = ValidHeaderCharacteristicKeyField(headerIdKey, hdrTran, transNo, ref errMsg);
                            }
                            break;
                        }

                    default:
                        break;
                }

                if (!isValid)
                {
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// To determine Header ID, only one color can be on a header.
        /// This can be in bulk, in pack, or in both.
        /// </summary>
        /// <param name="colorCodeRid"></param>
        /// <returns></returns>
        private bool IsColorMatch(string colorCodeId, int transNo)
        {
            bool isMatch = true;
            string errMsg = string.Empty;
            if (string.IsNullOrEmpty(_colorCodeId))
            {
                _colorCodeId = colorCodeId;
            }
            else if (colorCodeId != _colorCodeId)
            {
                errMsg = MIDText.GetText(eMIDTextCode.msg_FoundMoreThanOneColor);
                errMsg = errMsg.Replace("{0}", transNo.ToString());
                errMsg += System.Environment.NewLine;
                AddMessage(errMsg, eMIDTextCode.msg_FoundMoreThanOneColor);
                isMatch = false;
            }
            return isMatch;
        }

        /// <summary>
        /// Only "RESERVE" and "WORKUPTOTALBUY" headers are valid.
        /// </summary>
        /// <param name="headerType"></param>
        /// <returns></returns>
        private bool ValidHeaderType(string headerType)
        {
            bool validHdrType = true;

            if (headerType == "RESERVE" || headerType == "WORKUPTOTALBUY")
            {
                // Valid
            }
            else
            {
                validHdrType = false;
            }
            return validHdrType;
        }

        private bool ValidHeaderKeyField(string aField, HeadersHeader hdrTran, int transNo, ref string errMsg)
        {
            bool isValid = true;

            // Begin TT#1640-MD - stodd - Header load rejects the transaction when a header field (Style, PO, DC, Vendor, etc) is selected for the header ID and that field(s) is missing
            //switch (aField)
            //{
            //    case "STYLEID":

            //        if (hdrTran.StyleId == null || hdrTran.StyleId.ToString().Trim() == string.Empty)
            //        {
            //            errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            errMsg = errMsg.Replace("{1}", "StyleId");
            //            errMsg += System.Environment.NewLine;
            //            AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            return false;
            //        }
            //        break;
            //    case "DISTCENTER":

            //        if (hdrTran.DistCenter == null || hdrTran.DistCenter.ToString().Trim() == string.Empty)
            //        {
            //            errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            errMsg = errMsg.Replace("{1}", "DistCenter");
            //            errMsg += System.Environment.NewLine;
            //            AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            return false;
            //        }
            //        break;
            //    case "PURCHASEORDER":

            //        if (hdrTran.PurchaseOrder == null || hdrTran.PurchaseOrder.ToString().Trim() == string.Empty)
            //        {
            //            errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            errMsg = errMsg.Replace("{1}", "PurchaseOrder");
            //            errMsg += System.Environment.NewLine;
            //            AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            return false;
            //        }
            //        break;

            //    case "VENDOR":

            //        if (hdrTran.Vendor == null || hdrTran.Vendor.ToString().Trim() == string.Empty)
            //        {
            //            errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            errMsg = errMsg.Replace("{1}", "Vendor");
            //            errMsg += System.Environment.NewLine;
            //            AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            return false;
            //        }
            //        break;

            //    case "VSWID":

            //        if (hdrTran.VSWID == null || hdrTran.VSWID.ToString().Trim() == string.Empty)
            //        {
            //            errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            errMsg = errMsg.Replace("{0}", transNo.ToString());
            //            errMsg = errMsg.Replace("{1}", "VSWID");
            //            errMsg += System.Environment.NewLine;
            //            AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
            //            return false;
            //        }
            //        break;
            //}
            // End TT#1640-MD - stodd - Header load rejects the transaction when a header field (Style, PO, DC, Vendor, etc) is selected for the header ID and that field(s) is missing

            return isValid;
        }

        private bool ValidHeaderColorKeyField(string aField, HeadersHeader hdrTran, int transNo, ref string errMsg)
        {
            bool isValid = true;
            _bulkOrPack = string.Empty;

            // BULK color
            if (hdrTran.BulkColor != null)
            {
                if (hdrTran.BulkColor.Length > 1)
                {
                    errMsg = MIDText.GetText(eMIDTextCode.msg_FoundMoreThanOneColor);
                    errMsg = errMsg.Replace("{0}", transNo.ToString());
                    errMsg += System.Environment.NewLine;
                    AddMessage(errMsg, eMIDTextCode.msg_FoundMoreThanOneColor);
                    return false;
                }
                else if (hdrTran.BulkColor.Length == 1)
                {
                    HeadersHeaderBulkColor hdrTranBulkColor = hdrTran.BulkColor[0];
                    isValid = IsColorMatch(hdrTranBulkColor.ColorCodeID, transNo);
                    if (isValid)
                    {
                        _bulkOrPack = "BULK";
                    }
                }
            }

            // PACK Color
            if (hdrTran.Pack != null)
            {
                if (hdrTran.Pack.Length > 0)
                {
                    foreach (HeadersHeaderPack hdrTranPack in hdrTran.Pack)
                    {
                        if (hdrTranPack.PackColor != null)
                        {
                            if (hdrTranPack.PackColor.Length > 1)
                            {
                                errMsg = MIDText.GetText(eMIDTextCode.msg_FoundMoreThanOneColorOnPack);
                                errMsg = errMsg.Replace("{0}", transNo.ToString());
                                errMsg += System.Environment.NewLine;
                                AddMessage(errMsg, eMIDTextCode.msg_FoundMoreThanOneColorOnPack);
                                return false;
                            }
                            else if (hdrTranPack.PackColor.Length == 1)
                            {
                                HeadersHeaderPackPackColor hdrTranPackColor = hdrTranPack.PackColor[0];
                                isValid = IsColorMatch(hdrTranPackColor.ColorCodeID, transNo);
                                if (isValid)
                                {
                                    _bulkOrPack = "PACK";
                                }
                            }
                        }
                    }
                }
            }

            //=========================
            // No color found
            //=========================
            //if (isValid && _bulkOrPack == string.Empty)
            //{
            //    errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
            //    errMsg = errMsg.Replace("{0}", transNo.ToString());
            //    errMsg = errMsg.Replace("{1}", "a color or pack/color");
            //    errMsg += System.Environment.NewLine;
            //    AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
            //    return false;
            //}

            return isValid;
        }

        private bool ValidHeaderPackKeyField(string aField, HeadersHeader hdrTran, int transNo, ref string errMsg)
        {
            bool isValid = true;
            _packName = string.Empty;

            if (hdrTran.Pack != null)
            {
                //if (hdrTran.Pack.Length == 0)
                //{
                //    errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingKeyField);
                //    errMsg = errMsg.Replace("{0}", transNo.ToString());
                //    errMsg = errMsg.Replace("{1}", "a pack");
                //    errMsg += System.Environment.NewLine;
                //    AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingKeyField);
                //    return false;
                //}
                //else 
                if (hdrTran.Pack.Length > 1)
                {
                    errMsg = MIDText.GetText(eMIDTextCode.msg_FoundMoreThanOneColorOnPack);
                    errMsg = errMsg.Replace("{0}", transNo.ToString());
                    errMsg += System.Environment.NewLine;
                    AddMessage(errMsg, eMIDTextCode.msg_FoundMoreThanOneColorOnPack);
                    return false;
                }
                else if (hdrTran.Pack.Length == 1)
                {
                    HeadersHeaderPack hdrTranPack = hdrTran.Pack[0];
                    _packName = hdrTranPack.Name;
                    isValid = true;
                }
            }

            return isValid;
        }


        private bool ValidHeaderCharacteristicKeyField(string aField, HeadersHeader hdrTran, int transNo, ref string errMsg)
        {
            bool isValid = true;

            try
            {
				// Begin TT#1657-MD - stodd - Generated Header ID does not match on Header Date 
                //HeadersHeaderCharacteristic aChar = Array.Find(hdrTran.Characteristic, c => c.Name.ToUpper() == aField);

                //if (aChar != null)
                //{
                //    if (string.IsNullOrEmpty(aChar.Value))
                //    {
                //        errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingCharacteristicKeyField);
                //        errMsg = errMsg.Replace("{0}", transNo.ToString());
                //        errMsg = errMsg.Replace("{1}", aField);
                //        errMsg += System.Environment.NewLine;
                //        AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingCharacteristicKeyField);
                //        isValid = false;
                //    }
                //}
                //else
                //{
                //    errMsg = MIDText.GetText(eMIDTextCode.msg_HeaderTranMissingCharacteristicKeyField);
                //    errMsg = errMsg.Replace("{0}", transNo.ToString());
                //    errMsg = errMsg.Replace("{1}", aField);
                //    errMsg += System.Environment.NewLine;
                //    AddMessage(errMsg, eMIDTextCode.msg_HeaderTranMissingCharacteristicKeyField);
                //    isValid = false;
                //}
				// End TT#1657-MD - stodd - Generated Header ID does not match on Header Date 
            }
            catch
            {
                throw;
            }


            return isValid;
        }

        private eHeaderMatchingKeyType GetHeaderMatchKeyType(string aHeaderkey)
        {
            if (aHeaderkey.ToUpper() == _sequence)
            {
                return eHeaderMatchingKeyType.Sequence;
            }
            else if (aHeaderkey.ToUpper() == _color)
            {
                return eHeaderMatchingKeyType.Color;
            }
            else if (aHeaderkey.ToUpper() == _pack)
            {
                return eHeaderMatchingKeyType.Pack;
            }
            else if (Include.HeaderIDKeyList.Contains(aHeaderkey.ToUpper()))
            {
                return eHeaderMatchingKeyType.HeaderField;
            }
            // Begin TT#1966-MD - JSmith- DC Fulfillment
            else if (aHeaderkey.Trim().StartsWith("\""))
            {
                return eHeaderMatchingKeyType.Text;
            }
            // End TT#1966-MD - JSmith- DC Fulfillment
            else
            {
                return eHeaderMatchingKeyType.Characteristic;
            }
        }

        private eReturnCode GenerateHeaderId(HeadersHeader hdrTran, ref string aHeaderId, int transNo)
        {
            eReturnCode returnCode = eReturnCode.successful;
            string aHeaderKeyUpper;

            foreach (string headerIdKey in _headerIdKeyList)
            {
                if (string.IsNullOrEmpty(headerIdKey))
                {
                    continue;
                }
                aHeaderKeyUpper = headerIdKey.ToUpper();
                eHeaderMatchingKeyType aHeaderKeyType = GetHeaderMatchKeyType(headerIdKey);
                switch (aHeaderKeyType)
                {
                    case eHeaderMatchingKeyType.Sequence:
                        {
                            int seq = _sab.HeaderServerSession.GetNextHeaderSequenceNumber(9);
                            aHeaderId = seq.ToString("000000000");
                            break;
                        }

                    case eHeaderMatchingKeyType.HeaderField:
                        {
                            AppendHeaderIdHeaderValue(hdrTran, headerIdKey, ref aHeaderId, transNo);
                            break;
                        }

                    case eHeaderMatchingKeyType.Color:
                        {
                            aHeaderId += _colorCodeId;
                            break;
                        }

                    case eHeaderMatchingKeyType.Pack:
                        {
                            aHeaderId += _packName;
                            break;
                        }

                    case eHeaderMatchingKeyType.Characteristic:
                        {
                            if (hdrTran.Characteristic == null)
                            {
                                aHeaderId += "";  // empty key
                            }
                            else
                            {
                                HeadersHeaderCharacteristic aChar = Array.Find(hdrTran.Characteristic, c => c.Name.ToUpper() == headerIdKey);
								// Begin TT#1657-MD - stodd - Generated Header ID does not match on Header Date 
                                if (aChar != null)
                                {
                                    aHeaderId += aChar.Value;
                                }
								// End TT#1657-MD - stodd - Generated Header ID does not match on Header Date 
                            }
                            break;
                        }

                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    case eHeaderMatchingKeyType.Text:
                        {
                            aHeaderId += headerIdKey.Replace("\"", "");
                            break;
                        }
                    // End TT#1966-MD - JSmith- DC Fulfillment

                    default:
                        break;
                }

                // append delimiter
                if (aHeaderKeyType != eHeaderMatchingKeyType.Sequence)
                {
                    aHeaderId += _headerIdDelimiter;
                }

            }

            returnCode = AppendHeaderIdSequenceNumber(hdrTran, ref aHeaderId, transNo);

            return returnCode;
        }

        private eReturnCode AppendHeaderIdHeaderValue(HeadersHeader hdrTran, string headerIdKey, ref string aHeaderId, int transNo)
        {
            eReturnCode returnCode = eReturnCode.successful;

            switch (headerIdKey)
            {
                case "STYLEID":
                    aHeaderId += hdrTran.StyleId;
                    break;

                case "DISTCENTER":
                    aHeaderId += hdrTran.DistCenter;
                    break;

                case "PURCHASEORDER":
                    aHeaderId += hdrTran.PurchaseOrder;
                    break;

                case "VENDOR":
                    aHeaderId += hdrTran.Vendor;
                    break;

                case "VSWID":
                    aHeaderId += hdrTran.VSWID;
                    break;
                // Begin TT#1657-MD - stodd - Generated Header ID does not match on Header Date
                case "HEADERDATE":
                    if (hdrTran.HeaderDateSpecified)
                    {
                        aHeaderId += hdrTran.HeaderDate.ToString("yyyy-MM-dd");
                    }
                    break;
                // End TT#1657-MD - stodd - Generated Header ID does not match on Header Date
            }

            return returnCode;
        }

        /// <summary>
        /// Increments maxSequence and then appends the maxSequence to the new header ID for a length indicated by _headerIdSequenceLength.
        /// </summary>
        /// <param name="hdrTran"></param>
        /// <param name="aHeaderId"></param>
        /// <param name="transNo"></param>
        /// <param name="maxSequence"></param>
        /// <returns></returns>
        private eReturnCode AppendHeaderIdSequenceNumber(HeadersHeader hdrTran, ref string aHeaderId, int transNo)
        {
            eReturnCode returnCode = eReturnCode.successful;
            try
            {
                int sequenceNo = _sab.HeaderServerSession.GetNextHeaderSequenceNumber(_headerIdSequenceLength);
                string sequenceMask = string.Empty;
                sequenceMask = sequenceMask.PadRight(_headerIdSequenceLength, '0');
                aHeaderId += sequenceNo.ToString(sequenceMask);
            }
            catch
            {
                throw;
            }
            return returnCode;
        }

        /// <summary>
        /// Converts string header type to eHeaderType.
        /// </summary>
        /// <param name="headerType"></param>
        /// <returns></returns>
        private eHeaderType ConvertHeaderType(string headerType)
        {
            eHeaderType eHeaderType = DataCommon.eHeaderType.Receipt;
            switch (headerType)
            {
                case "VSW":
                    eHeaderType = eHeaderType.IMO;
                    break;
                case "RECEIPT":
                    eHeaderType = eHeaderType.Receipt;
                    break;
                case "PO":
                    eHeaderType = eHeaderType.PurchaseOrder;
                    break;
                case "ASN":
                    eHeaderType = eHeaderType.ASN;
                    break;
                case "DUMMY":
                    eHeaderType = eHeaderType.Receipt;
                    break;
                case "DROPSHIP":
                    eHeaderType = eHeaderType.DropShip;
                    break;
                case "RESERVE":
                    eHeaderType = eHeaderType.Reserve;
                    break;
                case "WORKUPTOTALBUY":
                    eHeaderType = eHeaderType.WorkupTotalBuy;
                    break;
                default:
                    //returnStatus = false;
                    //aMessageLevel = eMIDMessageLevel.Severe;
                    //aTypeMessage = "Allocation Header " + aHeaderID;
                    //aTypeMessage += " has an invalid Header Type [" + aHeaderType + "]";
                    //aTypeMessage += " - current value NOT changed" + System.Environment.NewLine;
                    break;
            }
            return eHeaderType;
        }

        /// <summary>
        /// Handles when multiple matching header rows are found for a "REMOVE" transaction.
        /// If one is not released and one IS released, the unreleased header is the chosen header ID.
        /// </summary>
        /// <param name="hdrTran"></param>
        /// <param name="headerDt"></param>
        /// <returns></returns>
        private eReturnCode HandleMutipleRowsForRemove(ref HeadersHeader hdrTran, DataTable headerDt, int transNo)
        {
            eReturnCode rtnCode = eReturnCode.editErrors;
            string errMsg = string.Empty;
            string headerIdNotReleased = string.Empty;
            string headerIdReleased = string.Empty;
            int releaseCnt = HowManyHeadersReleased(headerDt, ref headerIdReleased);
            int notreleasedCnt = HowManyHeadersNotReleased(headerDt, ref headerIdNotReleased);
            if (releaseCnt == 1 && notreleasedCnt == 1)
            {

                hdrTran.HeaderId = headerIdNotReleased;
                errMsg = MIDText.GetText(eMIDTextCode.msg_FoundTwoHeaderMatches);
                errMsg = errMsg.Replace("{0}", transNo.ToString());
                errMsg = errMsg.Replace("{1}", headerIdNotReleased);
                errMsg = errMsg.Replace("{2}", headerIdReleased);
                errMsg += System.Environment.NewLine;
                AddMessage(errMsg, eMIDTextCode.msg_FoundTwoHeaderMatches);
                rtnCode = eReturnCode.successful;
            }
            else
            {
                errMsg = MIDText.GetText(eMIDTextCode.msg_FoundMultipleHeaderMatches);
                errMsg = errMsg.Replace("{0}", transNo.ToString());
                errMsg += System.Environment.NewLine;
                AddMessage(errMsg, eMIDTextCode.msg_FoundMultipleHeaderMatches);
                rtnCode = eReturnCode.editErrors;
            }

            return rtnCode;
        }

        /// <summary>
        /// Reads through matching headers counting the unreleased headers.
        /// </summary>
        /// <param name="headerDt"></param>
        /// <param name="headerId"></param>
        /// <returns></returns>
        private int HowManyHeadersNotReleased(DataTable headerDt, ref string headerId)
        {
            int cnt = 0;

            foreach (DataRow aRow in headerDt.Rows)
            {
                if (Convert.IsDBNull(aRow["RELEASE_APPROVED_DATETIME"]) && Convert.IsDBNull(aRow["RELEASE_DATETIME"]))
                {
                    headerId = aRow["HDR_ID"].ToString();
                    cnt++;
                }
            }


            return cnt;
        }

        private int HowManyHeadersReleased(DataTable headerDt, ref string headerId)
        {
            int cnt = 0;

            foreach (DataRow aRow in headerDt.Rows)
            {
                if (!Convert.IsDBNull(aRow["RELEASE_APPROVED_DATETIME"]) || !Convert.IsDBNull(aRow["RELEASE_DATETIME"]))
                {
                    headerId = aRow["HDR_ID"].ToString();
                    cnt++;
                }
            }


            return cnt;
        }

        /// <summary>
        /// Reads through matching headers counting the released headers.
        /// </summary>
        /// <param name="aRow"></param>
        /// <param name="releaseApprovedDate"></param>
        /// <param name="releaseDate"></param>
        /// <param name="shippingComplete"></param>
        /// <returns></returns>
        private string ConvertdataRowToFields(DataRow aRow, ref DateTime releaseApprovedDate, ref DateTime releaseDate, ref bool shippingComplete)
        {
            string headerId = aRow["HDR_ID"].ToString();
            releaseApprovedDate = Include.UndefinedDate;
            if (!Convert.IsDBNull(aRow["RELEASE_APPROVED_DATETIME"]))
            {
                releaseApprovedDate = Convert.ToDateTime(aRow["RELEASE_APPROVED_DATETIME"]);
            }
            releaseDate = Include.UndefinedDate;
            if (!Convert.IsDBNull(aRow["RELEASE_DATETIME"]))
            {
                releaseDate = Convert.ToDateTime(aRow["RELEASE_DATETIME"]);
            }
            shippingComplete = Include.ConvertStringToBool(aRow["ShippingComplete"].ToString());

            return headerId;
        }

        private void AddMessage(string msgText, eMIDTextCode textCode)
        {
            eMIDMessageLevel msgLevel = MIDText.GetMessageLevel((int)textCode);
            _em.AddMsg(msgLevel, msgText, "HeaderLoadProcess.Custom");
        }

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        public HeadersHeader ConvertAllocationProfileToTransaction(AllocationProfile ap)
        {
            ArrayList alWork, alWork2, alWork3;
            HierarchyNodeProfile hnp;
            ColorCodeProfile ccp;
            SizeCodeProfile scp;

            HeadersHeader hdrTran = new HeadersHeader();
            HeadersHeaderCharacteristic headerCharacteristic;
            HeadersHeaderBulkColor headerBulkColor;
            HeadersHeaderBulkColorBulkColorSize headerBulkColorSize;
            HeadersHeaderPack headerPack;
            HeadersHeaderPackPackColor headerPackColor;
            HeadersHeaderPackPackColorPackColorSize headerPackColorSize;
            HeadersHeaderPackPackSize headerPackSize;

            hdrTran.HeaderAction = HeadersHeaderHeaderAction.Create;
            hdrTran.HeaderActionSpecified = true;
            hdrTran.HeaderId = ap.HeaderID;
            hdrTran.HeaderDescription = ap.HeaderDescription;
            hdrTran.HeaderDate = ap.HeaderDay;
            hdrTran.HeaderDateSpecified = true;

            switch (ap.HeaderType)
            {
                case eHeaderType.ASN:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.ASN;
                    break;
                case eHeaderType.DropShip:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.DropShip;
                    break;
                case eHeaderType.Dummy:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.Dummy;
                    break;
                case eHeaderType.IMO:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.VSW;
                    break;
                case eHeaderType.PurchaseOrder:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.PO;
                    break;
                case eHeaderType.Receipt:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.Receipt;
                    break;
                case eHeaderType.Reserve:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.Reserve;
                    break;
                case eHeaderType.WorkupTotalBuy:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.WorkupTotalBuy;
                    break;
                default:
                    hdrTran.HeaderType = HeadersHeaderHeaderType.Receipt;
                    break;
            }

            hdrTran.VSWID = ap.ImoID;

            hdrTran.VSWProcess = HeadersHeaderVSWProcess.Replace;
            if (ap.AdjustVSW_OnHand)
            {
                hdrTran.VSWProcess = HeadersHeaderVSWProcess.Adjust;
            }
            else
            {
                if (ap.IMO)
                {
                    hdrTran.VSWProcess = HeadersHeaderVSWProcess.Replace;
                }
                else
                {
                    hdrTran.VSWProcess = HeadersHeaderVSWProcess.Replace;
                }
            }

            hdrTran.HeaderTypeSpecified = true;
            if (!_dicHierarchyNodes.TryGetValue(ap.StyleHnRID, out hnp))
            {
                hnp = _sab.HierarchyServerSession.GetNodeData(ap.StyleHnRID);
                _dicHierarchyNodes.Add(ap.StyleHnRID, hnp);
            }
            hdrTran.StyleId = hnp.NodeID;

            int homeHierarchyParentRID = hnp.HomeHierarchyParentRID;
            if (!_dicHierarchyNodes.TryGetValue(homeHierarchyParentRID, out hnp))
            {
                hnp = _sab.HierarchyServerSession.GetNodeData(homeHierarchyParentRID);
                _dicHierarchyNodes.Add(homeHierarchyParentRID, hnp);
            }
            hdrTran.ParentOfStyleId = hnp.NodeID;

            if (ap.SizeGroupRID != Include.NoRID)
            {
                SizeGroup sizeGroupData = new SizeGroup();
                hdrTran.SizeGroupName = sizeGroupData.GetSizeGroupName(ap.SizeGroupRID);
            }
            if (ap.BulkMultiple > 0)
            {
                hdrTran.BulkMultiple = ap.BulkMultiple;
                hdrTran.BulkMultipleSpecified = true;
            }
            hdrTran.PurchaseOrder = ap.PurchaseOrder;
            hdrTran.Vendor = ap.Vendor;
            hdrTran.DistCenter = ap.DistributionCenter;
            hdrTran.Notes = ap.AllocationNotes;

            if (ap.UnitRetail > 0)
            {
                hdrTran.UnitRetail = ap.UnitRetail;
                hdrTran.UnitRetailSpecified = true;
            }
            if (ap.UnitCost > 0)
            {
                hdrTran.UnitCost = ap.UnitCost;
                hdrTran.UnitCostSpecified = true;
            }

            // copy characteristics
            if (ap.Characteristics != null)
            {
                alWork = new ArrayList();
                foreach (CharacteristicsBin cb in ap.Characteristics.Values)
                {
                    headerCharacteristic = new HeadersHeaderCharacteristic();
                    headerCharacteristic.Name = cb.Name;
                    headerCharacteristic.Value = cb.Value;
                    alWork.Add(headerCharacteristic);
                }
                hdrTran.Characteristic = new HeadersHeaderCharacteristic[alWork.Count];
                alWork.CopyTo(0, hdrTran.Characteristic, 0, alWork.Count);
            }

            hdrTran.TotalUnits = ap.TotalUnitsToAllocate;
            hdrTran.TotalUnitsSpecified = true;

            if (ap.BulkColors.Count > 0)
            {
                alWork = new ArrayList();
                foreach (HdrColorBin bulkColor in ap.BulkColors.Values)
                {
                    if (bulkColor.ColorUnitsToAllocate > 0)
                    {
                        headerBulkColor = new HeadersHeaderBulkColor();

                        if (!_dicColorCodes.TryGetValue(bulkColor.ColorCodeRID, out ccp))
                        {
                            ccp = _sab.HierarchyServerSession.GetColorCodeProfile(bulkColor.ColorCodeRID);
                            _dicColorCodes.Add(bulkColor.ColorCodeRID, ccp);
                        }
                        headerBulkColor.ColorCodeID = ccp.ColorCodeID;

                        headerBulkColor.Units = bulkColor.ColorUnitsToAllocate;
                        if (bulkColor.ColorSizes != null)
                        {
                            alWork2 = new ArrayList();
                            foreach (HdrSizeBin bulkColorSize in bulkColor.ColorSizes.Values)
                            {
                                if (bulkColorSize.SizeUnitsToAllocate > 0)
                                {
                                    headerBulkColorSize = new HeadersHeaderBulkColorBulkColorSize();
                                    if (!_dicSizeCodes.TryGetValue(bulkColorSize.SizeCodeRID, out scp))
                                    {
                                        scp = _sab.HierarchyServerSession.GetSizeCodeProfile(bulkColorSize.SizeCodeRID);
                                        _dicSizeCodes.Add(bulkColorSize.SizeCodeRID, scp);
                                    }
                                    headerBulkColorSize.SizeCodeID = scp.SizeCodeID;
                                    headerBulkColorSize.Units = bulkColorSize.SizeUnitsToAllocate;
                                    alWork2.Add(headerBulkColorSize);
                                }
                            }
                            if (alWork2.Count > 0)
                            {
                                headerBulkColor.BulkColorSize = new HeadersHeaderBulkColorBulkColorSize[alWork2.Count];
                                alWork2.CopyTo(0, headerBulkColor.BulkColorSize, 0, alWork2.Count);
                            }
                        }
                        alWork.Add(headerBulkColor);
                    }
                }
                if (alWork.Count > 0)
                {
                    hdrTran.BulkColor = new HeadersHeaderBulkColor[alWork.Count];
                    alWork.CopyTo(0, hdrTran.BulkColor, 0, alWork.Count);
                }
            }

            if (ap.Packs.Count > 0)
            {
                alWork = new ArrayList();
                foreach (PackHdr pack in ap.Packs.Values)
                {
                    if (pack.PacksToAllocate > 0)
                    {
                        headerPack = new HeadersHeaderPack();
                        headerPack.Name = pack.PackName;
                        headerPack.IsGeneric = pack.GenericPack;
                        headerPack.Multiple = pack.PackMultiple;
                        headerPack.Packs = pack.PacksToAllocate;

                        // process pack/color/sizes
                        if (pack.PackColors != null)
                        {
                            alWork2 = new ArrayList();
                            foreach (PackColorSize packColor in pack.PackColors.Values)
                            {
                                if (packColor.ColorUnitsInPack > 0
                                    && packColor.ColorCodeRID != Include.DummyColorRID)
                                {
                                    headerPackColor = new HeadersHeaderPackPackColor();
                                    if (!_dicColorCodes.TryGetValue(packColor.ColorCodeRID, out ccp))
                                    {
                                        ccp = _sab.HierarchyServerSession.GetColorCodeProfile(packColor.ColorCodeRID);
                                        _dicColorCodes.Add(packColor.ColorCodeRID, ccp);
                                    }
                                    headerPackColor.ColorCodeID = ccp.ColorCodeID;
                                    headerPackColor.Units = packColor.ColorUnitsInPack;
                                    if (packColor.ColorSizes != null)
                                    {
                                        alWork3 = new ArrayList();
                                        foreach (PackSizeBin packColorSize in packColor.ColorSizes.Values)
                                        {
                                            if (packColorSize.ContentUnits > 0)
                                            {
                                                headerPackColorSize = new HeadersHeaderPackPackColorPackColorSize();
                                                if (!_dicSizeCodes.TryGetValue(packColorSize.ContentCodeRID, out scp))
                                                {
                                                    scp = _sab.HierarchyServerSession.GetSizeCodeProfile(packColorSize.ContentCodeRID);
                                                    _dicSizeCodes.Add(packColorSize.ContentCodeRID, scp);
                                                }
                                                headerPackColorSize.SizeCodeID = scp.SizeCodeID;
                                                headerPackColorSize.Units = packColorSize.ContentUnits;
                                                alWork3.Add(headerPackColorSize);
                                            }
                                        }
                                        if (alWork3.Count > 0)
                                        {
                                            headerPackColor.PackColorSize = new HeadersHeaderPackPackColorPackColorSize[alWork3.Count];
                                            alWork3.CopyTo(0, headerPackColor.PackColorSize, 0, alWork3.Count);
                                        }
                                    }
                                    alWork2.Add(headerPackColor);
                                }
                            }
                            if (alWork2.Count > 0)
                            {
                                headerPack.PackColor = new HeadersHeaderPackPackColor[alWork2.Count];
                                alWork2.CopyTo(0, headerPack.PackColor, 0, alWork2.Count);
                            }

                            // process pack sizes
                            alWork2 = new ArrayList();
                            foreach (PackColorSize packColor in pack.PackColors.Values)
                            {
                                if (packColor.ColorUnitsInPack > 0
                                    && packColor.ColorCodeRID == Include.DummyColorRID)
                                {
                                    if (packColor.ColorSizes != null)
                                    {
                                        foreach (PackSizeBin packColorSize in packColor.ColorSizes.Values)
                                        {
                                            if (packColorSize.ContentUnits > 0)
                                            {
                                                headerPackSize = new HeadersHeaderPackPackSize();
                                                if (!_dicSizeCodes.TryGetValue(packColorSize.ContentCodeRID, out scp))
                                                {
                                                    scp = _sab.HierarchyServerSession.GetSizeCodeProfile(packColorSize.ContentCodeRID);
                                                    _dicSizeCodes.Add(packColorSize.ContentCodeRID, scp);
                                                }
                                                headerPackSize.SizeCodeID = scp.SizeCodeID;
                                                headerPackSize.Units = packColorSize.ContentUnits;
                                                alWork2.Add(headerPackSize);
                                            }
                                        }
                                    }
                                }
                            }
                            if (alWork2.Count > 0)
                            {
                                headerPack.PackSize = new HeadersHeaderPackPackSize[alWork2.Count];
                                alWork2.CopyTo(0, headerPack.PackSize, 0, alWork2.Count);
                            }

                        }

                        alWork.Add(headerPack);
                    }
                }
                if (alWork.Count > 0)
                {
                    hdrTran.Pack = new HeadersHeaderPack[alWork.Count];
                    alWork.CopyTo(0, hdrTran.Pack, 0, alWork.Count);
                }
            }


            return hdrTran;
        }
        // End TT#1966-MD - JSmith- DC Fulfillment
    }
}
