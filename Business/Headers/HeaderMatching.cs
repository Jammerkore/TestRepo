using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace MIDRetail.Business
{
    //==========================================================================
    // HeaderMatching (Used in Header Reconcile & Header Load)
    // Houses all of the code that determines if the transaction header
    // with a missing header ID matches an existing header.
    // Builds the SQL to determine the above.
    //==========================================================================


    public partial class HeaderMatching
    {
        //private static List<string> _headerKeyList = new List<string>() { "HEADERDESCRIPTION","HEADERDATE", "UNITRETAIL", "UNITCOST", "STYLEID", "PARENTOFSTYLEID",
        //        "SIZEGROUPNAME", "HEADERTYPE", "DISTCENTER", "VENDOR", "PURCHASEORDER", "WORKFLOW", "VSWID"};
        private HeaderCharacteristicsData _hdrCharData = null;
        private static int _tabLevel = 0;
        private SessionAddressBlock _sab;
        private HeadersHeader _transactionHeader;
        private List<string> _headerKeysToMatchList;
        private int _transactionNumber;
        private int _headerIdSequenceLength;
        private bool HeaderTypeSw;  // TT#5805 - AGallagher - The Header Reconcile process does not create a correct header ID (based on the HeaderKeys) when the style ID exists on different header type

        public HeaderMatching(SessionAddressBlock sab)
		{
            _sab = sab;
        }

        public HeaderMatching(SessionAddressBlock sab, int headerIdSequenceLength)
		{
            _sab = sab;
            _headerIdSequenceLength = headerIdSequenceLength;
        }


        private HeaderCharacteristicsData HeaderCharacteristicsData
        {
            get
            {
                if (_hdrCharData == null)
                {
                    _hdrCharData = new HeaderCharacteristicsData();
                }
                return _hdrCharData;
            }
        }


        // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
		//public eReturnCode FindMatchingHeader(HeadersHeader headerTran, ref string headerId, List<string> headerKeysToMatchList, int transactionNumber)
		public eReturnCode FindMatchingHeader(Session aSession, HeadersHeader headerTran, ref string headerId, List<string> headerKeysToMatchList, int transactionNumber)
		// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
        {
            eReturnCode returnCode = eReturnCode.successful;
            string matchSql = string.Empty;
            _transactionHeader = headerTran;
            _headerKeysToMatchList = headerKeysToMatchList;
            _transactionNumber = transactionNumber;
            bool isReset = false;

            try
            {
                Header headerData = new Header();

                returnCode = BuildHeaderMatchSql(ref matchSql);

                if (returnCode == eReturnCode.successful)
                {
                    // Execute SQL
                    DataTable dt = headerData.ExecuteCommand(matchSql);

                    if (_transactionHeader.HeaderActionSpecified && _transactionHeader.HeaderAction.ToString().Trim().ToUpper() == "RESET")
                    {
                        isReset = true;
                    }
					// Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
					//returnCode = ExamineResults(dt, isReset, matchSql, ref headerId);
                    returnCode = ExamineResults(aSession, dt, isReset, matchSql, ref headerId);
					// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
                }
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildHeaderMatchSql(ref string matchSql)
        {
            eReturnCode returnCode = eReturnCode.successful;
            matchSql = "SELECT HDR_ID, Released, ReleaseApproved, ShippingStarted FROM VW_GET_HEADERS h WHERE ";	// TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction 
            string aHeaderKeyUpper = string.Empty;

            try
            {
                foreach (string aHeaderKey in _headerKeysToMatchList)
                {
                    if (string.IsNullOrEmpty(aHeaderKey))
                    {
                        continue;
                    }
                    aHeaderKeyUpper = aHeaderKey.ToUpper();
                    eHeaderMatchingKeyType aHeaderKeyType = GetHeaderMatchKeyType(aHeaderKey);
                    switch (aHeaderKeyType)
                    {
                        case eHeaderMatchingKeyType.HeaderField:
                            {
                                returnCode = BuildSqlForHeaderFields(aHeaderKeyUpper, ref matchSql);
                                break;
                            }

                        case eHeaderMatchingKeyType.Color:
                            {
                                returnCode = BuildSqlForHeaderColors(ref matchSql);
                                break;
                            }

                        case eHeaderMatchingKeyType.Pack:
                            {
                                returnCode = BuildSqlForHeaderPacks(ref matchSql);
                                break;
                            }


                        case eHeaderMatchingKeyType.Characteristic:
                            {
                                //if (_transactionHeader.Characteristic == null)
                                //{
                                //    string msgText = "Processing Transaction #" + _transactionNumber.ToString() + ".";
                                //    msgText += System.Environment.NewLine;
                                //    msgText += "A Header Characteristic (" + aHeaderKey + ") was chosen to match headers on, but this transaction contains no characteristics to use for match.";
                                //    _sab.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);
                                //    returnCode = eReturnCode.severe;
                                //}
                                //else
                                //{
                                    returnCode = BuildSqlForHeaderCharacteristics(aHeaderKeyUpper, ref matchSql);
                                //}
                                break;
                            }

                        default:
                            break;
                    }

                    if (returnCode != eReturnCode.successful)
                    {
                        break;
                    }

                }

                // Trim off trailing "AND"
                string hangingAnd = " AND ";
                if (matchSql.EndsWith(hangingAnd))
                {
                    matchSql = matchSql.Substring(0, matchSql.LastIndexOf(hangingAnd));
                }

                // Begin TT#5805 - AGallagher - The Header Reconcile process does not create a correct header ID (based on the HeaderKeys) when the style ID exists on different header type
                if (HeaderTypeSw == false)
                {
                    // TT#5805 - AGallagher - *** if a new header type is added to the RO software it may need to be added line of code below  ***
                    matchSql = matchSql + " AND h.DISPLAY_TYPE IN (800730,800731,800732,800733,800735,800737,800738,800741)\r\n"; 
                }
                // End TT#5805 - AGallagher - The Header Reconcile process does not create a correct header ID (based on the HeaderKeys) when the style ID exists on different header type

                Debug.Write("BuildHeaderMatchSql:" + Environment.NewLine + matchSql);
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eHeaderMatchingKeyType GetHeaderMatchKeyType(string aHeaderkey)
        {
            if (aHeaderkey.ToUpper() == "COLOR")
            {
                return eHeaderMatchingKeyType.Color;
            }
            else if (aHeaderkey.ToUpper() == "PACK NAME")				// TT#1605-MD - stodd - Adding 'sequence' to other header ID keys causes sequence to replace all other keys.

            {
                return eHeaderMatchingKeyType.Pack;
            }
            else if (Include.HeaderKeyList.Contains(aHeaderkey.ToUpper()))
            {
                return eHeaderMatchingKeyType.HeaderField;
            }
            else
            {
                return eHeaderMatchingKeyType.Characteristic;
            }
        }

        private eReturnCode BuildSqlForHeaderFields(string aHeaderKey, ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                if (aHeaderKey == "HEADERDESCRIPTION")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.HeaderDescription))
                    {
                        aSql += "(h.HDR_DESC = '' OR h.HDR_DESC is null)" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.HDR_DESC = '" + _transactionHeader.HeaderDescription + "'" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "HEADERDATE")
                {
                    if (_transactionHeader.HeaderDateSpecified)
                    {
                        aSql += "h.HDR_DAY = '" + _transactionHeader.HeaderDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.HDR_DAY = '" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "UNITRETAIL")
                {
                    if (_transactionHeader.UnitRetailSpecified)
                    {
                        aSql += "h.UNIT_RETAIL = " + _transactionHeader.UnitRetail + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.UNIT_RETAIL = 0" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "UNITCOST")
                {
                    if (_transactionHeader.UnitCostSpecified)
                    {
                        aSql += "h.UNIT_COST = " + _transactionHeader.UnitCost + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.UNIT_COST = 0" + Environment.NewLine;
                    }
                }
                //else if (aHeaderKey == "PARENTOFSTYLEID")
                //{
                //    if (organizationalPhRID == null)
                //    {
                //        MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                //        organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                //    }
                //    string nodeDisplay = "(SELECT [dbo].[UDF_MID_GET_NODE_DISPLAY] ((SELECT PARENT_HN_RID FROM HIER_NODE_JOIN hnj WITH (NOLOCK) WHERE hnj.HN_RID=h.STYLE_HNRID AND hnj.PH_RID=" + organizationalPhRID + ")))";
                //    BuildSqlForStringComparison(nodeDisplay, fc, ref aSql);
                //}
                else if (aHeaderKey == "STYLEID")
                {
                    // Begin TT#1655-MD - stodd - Invalid Column Name DB Error when Style does not exist (needs to be auto-added)
                    //aSql += "h.STYLE_HNRID in (SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_ID] (" + _transactionHeader.StyleId + "))" + Environment.NewLine;
                    aSql += "h.STYLE_HNRID in (SELECT [dbo].[UDF_HIERARCHY_GET_NODE_RID_FROM_BASE_ID] ('" + _transactionHeader.StyleId + "'))" + Environment.NewLine;
                    // End TT#1655-MD - stodd - Invalid Column Name DB Error when Style does not exist (needs to be auto-added)
                }
                else if (aHeaderKey == "SIZEGROUPNAME")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.SizeGroupName))
                    {
                        aSql += "h.SIZE_GROUP_RID = 1" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.SIZE_GROUP_RID in (SELECT sg.SIZE_GROUP_RID FROM SIZE_GROUP sg WHERE SIZE_GROUP_NAME = '" + _transactionHeader.SizeGroupName + "')" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "BULKMULTIPLE")
                {
                    if (_transactionHeader.BulkMultipleSpecified)
                    {
                        aSql += "h.BULK_MULTIPLE = " + _transactionHeader.BulkMultiple + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.BULK_MULTIPLE = 0" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "HEADERTYPE")
                {
                    int headerType = (int)GetHeaderType();
                    aSql += "h.DISPLAY_TYPE = " + headerType + Environment.NewLine;
                    HeaderTypeSw = true;  // TT#5805 - AGallagher - The Header Reconcile process does not create a correct header ID (based on the HeaderKeys) when the style ID exists on different header type
                }
                else if (aHeaderKey == "VENDOR")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.Vendor))
                    {
                        aSql += "(h.VENDOR = '' OR h.VENDOR is null)" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.VENDOR = '" + _transactionHeader.Vendor + "'" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "DISTCENTER")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.DistCenter))
                    {
                        aSql += "(h.DIST_CENTER = '' OR h.DIST_CENTER is null)" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.DIST_CENTER = '" + _transactionHeader.DistCenter + "'" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "PURCHASEORDER")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.PurchaseOrder))
                    {
                        aSql += "(h.PURCHASE_ORDER = '' OR h.PURCHASE_ORDER is null)" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.PURCHASE_ORDER = '" + _transactionHeader.PurchaseOrder + "'" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "WORKFLOW")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.Workflow))
                    {
                        aSql += "h.WORKFLOW_RID = 1" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.WORKFLOW_RID in (SELECT wf.WORKFLOW_RID FROM WORKFLOW wf WHERE WORK_FLOW_NAME = '" + _transactionHeader.Workflow + "')" + Environment.NewLine;
                    }
                }
                else if (aHeaderKey == "VSWID")
                {
                    if (string.IsNullOrEmpty(_transactionHeader.VSWID))
                    {
                        aSql += "(h.IMO_ID = '' OR h.IMO_ID is null)" + Environment.NewLine;
                    }
                    else
                    {
                        aSql += "h.IMO_ID = '" + _transactionHeader.VSWID + "'" + Environment.NewLine;
                    }
                }

                aSql += " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildSqlForHeaderColors(ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                if (_transactionHeader.BulkColor != null)
                {
                    foreach (HeadersHeaderBulkColor aTransBulkColor in _transactionHeader.BulkColor)
                    {
                        returnCode = BuildSqlForHeaderBulkColor(aTransBulkColor, ref aSql);
                    }
                }
                else
                {
                    returnCode = BuildSqlForNoHeaderBulkColor(ref aSql);
                }

                if (_transactionHeader.Pack != null)
                {
                    foreach (HeadersHeaderPack aTransPack in _transactionHeader.Pack)
                    {
                        if (aTransPack.PackColor != null)
                        {
                            foreach (HeadersHeaderPackPackColor aHdrPackColor in aTransPack.PackColor)
                            {
                                returnCode = BuildSqlForHeaderPackColor(aTransPack, aHdrPackColor, ref aSql);
                            }
                        }
                        else
                        {
                            returnCode = BuildSqlForNoHeaderPackColor(aTransPack, ref aSql);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildSqlForHeaderBulkColor(HeadersHeaderBulkColor aTransBulkColor, ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                aSql += "h.HDR_RID IN " + System.Environment.NewLine;
                aSql += GetTab() + "( " + System.Environment.NewLine;
                _tabLevel++;
                aSql += GetTab() + "SELECT h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "FROM " + System.Environment.NewLine;
                aSql += GetTab() + "HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                aSql += GetTab() + "join BASE_NODE bn on h.STYLE_HNRID = bn.HN_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join COLOR_NODE cn on cn.STYLE_NODE_ID = bn.BN_ID " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_BULK_COLOR hbc on hbc.HDR_RID = h.HDR_RID and hbc.COLOR_CODE_RID = cn.COLOR_CODE_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join COLOR_CODE cc on cc.COLOR_CODE_RID = cn.COLOR_CODE_RID " + System.Environment.NewLine;
                aSql += GetTab() + "WHERE COLOR_CODE_ID = '" + aTransBulkColor.ColorCodeID + "'" + System.Environment.NewLine;
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += GetTab() + " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        /// <summary>
        /// Build SQL for bulk color when there is NO bulk color in the header transaction.
        /// </summary>
        /// <param name="aSql"></param>
        /// <returns></returns>
        private eReturnCode BuildSqlForNoHeaderBulkColor(ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                aSql += "h.HDR_RID NOT IN " + System.Environment.NewLine;
                aSql += GetTab() + "( " + System.Environment.NewLine;
                _tabLevel++;
                aSql += GetTab() + "SELECT h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "FROM " + System.Environment.NewLine;
                aSql += GetTab() + "HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_BULK_COLOR hbc on hbc.HDR_RID = h.HDR_RID " + System.Environment.NewLine;
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += GetTab() + " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }



        private eReturnCode BuildSqlForHeaderPackColor(HeadersHeaderPack aTransPack,  HeadersHeaderPackPackColor aHdrPackColor, ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                aSql += "h.HDR_RID IN " + System.Environment.NewLine;
                aSql += GetTab() + "( " + System.Environment.NewLine;
                _tabLevel++;
                aSql += GetTab() + "SELECT h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "FROM " + System.Environment.NewLine;
                aSql += GetTab() + "HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                aSql += GetTab() + "join BASE_NODE bn on h.STYLE_HNRID = bn.HN_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join COLOR_NODE cn on cn.STYLE_NODE_ID = bn.BN_ID " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_PACK hp on hp.HDR_RID = h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_PACK_COLOR hpc on hpc.HDR_PACK_RID = hp.HDR_PACK_RID and hpc.COLOR_CODE_RID = cn.COLOR_CODE_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join COLOR_CODE cc on cc.COLOR_CODE_RID = cn.COLOR_CODE_RID " + System.Environment.NewLine;
                aSql += GetTab() + "WHERE COLOR_CODE_ID = '" + aHdrPackColor.ColorCodeID + "'" + System.Environment.NewLine;
                aSql += GetTab() + "AND hp.HDR_PACK_NAME = '" + aTransPack.Name + "'" + System.Environment.NewLine;
                if (aTransPack.IsGenericSpecified)
                {
                    if (aTransPack.IsGeneric)
                    {
                        aSql += GetTab() + "AND hp.GENERIC_IND = '" + "1" + "'" + System.Environment.NewLine;
                    }
                    else
                    {
                        aSql += GetTab() + "AND hp.GENERIC_IND = '" + "0" + "'" + System.Environment.NewLine;
                    }
                }
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += GetTab() + " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildSqlForNoHeaderPackColor(HeadersHeaderPack aTransPack, ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                aSql += "h.HDR_RID NOT IN " + System.Environment.NewLine;
                aSql += GetTab() + "( " + System.Environment.NewLine;
                _tabLevel++;
                aSql += GetTab() + "SELECT h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "FROM " + System.Environment.NewLine;
                aSql += GetTab() + "HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_PACK hp on hp.HDR_RID = h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_PACK_COLOR hpc on hpc.HDR_PACK_RID = hp.HDR_PACK_RID " + System.Environment.NewLine;
                aSql += GetTab() + "WHERE hp.HDR_PACK_NAME = '" + aTransPack.Name + "'" + System.Environment.NewLine;
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += GetTab() + " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildSqlForHeaderPacks(ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                if (_transactionHeader.Pack != null)
                {
                    foreach (HeadersHeaderPack aTransPack in _transactionHeader.Pack)
                    {
                        returnCode = BuildSqlForHeaderPack(aTransPack, ref aSql);

                        if (aTransPack.PackColor != null)
                        {
                            foreach (HeadersHeaderPackPackColor aHdrPackColor in aTransPack.PackColor)
                            {
                                returnCode = BuildSqlForHeaderPackColor(aTransPack, aHdrPackColor, ref aSql);
                            }
                        }
                        else
                        {
                            returnCode = BuildSqlForNoHeaderPackColor(aTransPack, ref aSql);
                        }
                    }
                }
                else
                {
                    returnCode = BuildSqlForNoHeaderPack(ref aSql);
                }
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildSqlForHeaderPack(HeadersHeaderPack aTransPack, ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                aSql += "h.HDR_RID IN " + System.Environment.NewLine;
                aSql += GetTab() + "( " + System.Environment.NewLine;
                _tabLevel++;
                aSql += GetTab() + "SELECT h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "FROM " + System.Environment.NewLine;
                aSql += GetTab() + "HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                aSql += GetTab() + "join BASE_NODE bn on h.STYLE_HNRID = bn.HN_RID " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_PACK hp on hp.HDR_RID = h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "AND hp.HDR_PACK_NAME = '" + aTransPack.Name + "'" + System.Environment.NewLine;
                if (aTransPack.IsGenericSpecified)
                {
                    if (aTransPack.IsGeneric)
                    {
                        aSql += GetTab() + "AND hp.GENERIC_IND = '" + "1" + "'" + System.Environment.NewLine;
                    }
                    else
                    {
                        aSql += GetTab() + "AND hp.GENERIC_IND = '" + "0" + "'" + System.Environment.NewLine;
                    }
                }
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += GetTab() + " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        private eReturnCode BuildSqlForNoHeaderPack(ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;

            try
            {
                aSql += "h.HDR_RID NOT IN " + System.Environment.NewLine;
                aSql += GetTab() + "( " + System.Environment.NewLine;
                _tabLevel++;
                aSql += GetTab() + "SELECT h.HDR_RID " + System.Environment.NewLine;
                aSql += GetTab() + "FROM " + System.Environment.NewLine;
                aSql += GetTab() + "HEADER h WITH (NOLOCK) " + System.Environment.NewLine;
                aSql += GetTab() + "join HEADER_PACK hp on hp.HDR_RID = h.HDR_RID " + System.Environment.NewLine;
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += GetTab() + " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }


        private eReturnCode BuildSqlForHeaderCharacteristics(string aHeaderKey, ref string aSql)
        {
            eReturnCode returnCode = eReturnCode.successful;
            eHeaderCharType valueType = eHeaderCharType.unknown;
            int hdrCharGroupRid = Include.NoRID;
            GetHeaderCharacteristicGroupInfo(aHeaderKey, ref valueType, ref hdrCharGroupRid);

            try
            {
                HeadersHeaderCharacteristic aChar = null;
                if (_transactionHeader.Characteristic != null)
                {
                    aChar = Array.Find(_transactionHeader.Characteristic, c => c.Name.ToUpper() == aHeaderKey);
                }

                if (aChar != null)
                {
                    if (valueType == eHeaderCharType.date || valueType == eHeaderCharType.dollar || valueType == eHeaderCharType.number || valueType == eHeaderCharType.text)
                    {
                        aSql += "h.HDR_RID IN " + System.Environment.NewLine;
                        aSql += GetTab() + "( " + System.Environment.NewLine;
                        _tabLevel++;
                        aSql += GetTab() + "SELECT HDR_RID " + System.Environment.NewLine;
                        aSql += GetTab() + "FROM " + System.Environment.NewLine;
                        aSql += GetTab() + "HEADER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;
                        aSql += GetTab() + "WHERE HC_RID IN " + System.Environment.NewLine;
                        _tabLevel++;
                        aSql += GetTab() + "( " + System.Environment.NewLine;
                        aSql += GetTab() + "SELECT HC_RID " + System.Environment.NewLine;
                        aSql += GetTab() + "FROM HEADER_CHAR hc WITH (NOLOCK) " + System.Environment.NewLine;
                        aSql += GetTab() + "WHERE HCG_RID = " + hdrCharGroupRid.ToString() + System.Environment.NewLine;
                        if (valueType == eHeaderCharType.date)
                        {
                            aSql += GetTab() + "AND ";
                            aSql += "hc.DATE_VALUE = '" + aChar.Value + "'";
                            aSql += System.Environment.NewLine;
                        }
                        else if (valueType == eHeaderCharType.dollar)
                        {
                            aSql += GetTab() + "AND ";
                            aSql += "hc.DOLLAR_VALUE = " + aChar.Value + "";
                            aSql += System.Environment.NewLine;
                        }
                        else if (valueType == eHeaderCharType.number)
                        {
                            aSql += GetTab() + "AND ";
                            // Begin TT#1966-MD - JSmith- DC Fulfillment
                            //aSql += "hc.NUMBER_VALUE = " + aChar.Value + "";
                            aSql += "hc.NUMBER_VALUE = ";
                            if (string.IsNullOrWhiteSpace(aChar.Value))
                            {
                                aSql += "null";
                            }
                            else
                            {
                                aSql += aChar.Value + "";
                            }
                            // End TT#1966-MD - JSmith- DC Fulfillment
                            aSql += System.Environment.NewLine;
                        }
                        else if (valueType == eHeaderCharType.text)
                        {
                            aSql += GetTab() + "AND ";
                            aSql += "UPPER(hc.TEXT_VALUE) = '" + aChar.Value.ToUpper() + "'";
                            aSql += System.Environment.NewLine;
                        }
                    }
                }
                else
                {
                    //========================================================================
                    // SQL for the header when it does not have a value for the characteristic.
                    //========================================================================
                    aSql += "h.HDR_RID NOT IN " + System.Environment.NewLine;
                    aSql += GetTab() + "( " + System.Environment.NewLine;
                    _tabLevel++;
                    aSql += GetTab() + "SELECT HDR_RID " + System.Environment.NewLine;
                    aSql += GetTab() + "FROM " + System.Environment.NewLine;
                    aSql += GetTab() + "HEADER_CHAR_JOIN hcj WITH (NOLOCK) " + System.Environment.NewLine;
                    aSql += GetTab() + "WHERE HC_RID IN " + System.Environment.NewLine;
                    _tabLevel++;
                    aSql += GetTab() + "( " + System.Environment.NewLine;
                    aSql += GetTab() + "SELECT HC_RID " + System.Environment.NewLine;
                    aSql += GetTab() + "FROM HEADER_CHAR hc WITH (NOLOCK) " + System.Environment.NewLine;
                    aSql += GetTab() + "WHERE HCG_RID = " + hdrCharGroupRid.ToString() + System.Environment.NewLine;
                }

                aSql += GetTab() + ") " + System.Environment.NewLine;
                _tabLevel--;
                _tabLevel--;
                aSql += GetTab() + ") " + System.Environment.NewLine;
                aSql += " AND ";
            }
            catch
            {
                throw;
            }

            return returnCode;
        }

        // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
		//private eReturnCode ExamineResults(DataTable dtResults, bool isReset, string matchSql, ref string headerId)
		private eReturnCode ExamineResults(Session aSession, DataTable dtResults, bool isReset, string matchSql, ref string headerId)
		// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
        {
            eReturnCode returnCode = eReturnCode.successful;
            int noOfHeaderMatches = 0;
            int noOfNotReleasedMatches = 0;		// TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction

            foreach (DataRow aRow in dtResults.Rows)
            {
                bool released = bool.Parse(aRow["Released"].ToString());
                bool releaseApproved = bool.Parse(aRow["ReleaseApproved"].ToString());
                bool shippingStarted = bool.Parse(aRow["ShippingStarted"].ToString()); 	// TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction

                if (isReset)    // Look are Released/Released Approved
                {
					// Begin TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction
                    if (!shippingStarted && (released || releaseApproved))
                    {
                        noOfHeaderMatches++;
                        headerId = aRow["HDR_ID"].ToString();
                    }
                    else
                    {
                        // We also want to know if there were any non-released matches
                        if (!released && !releaseApproved)
                        {
                            noOfNotReleasedMatches++;
                        }
                    }
					// End TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction
                }
                else            // else look at headers not released/Released Approved
                {
                    if (!released && !releaseApproved)
                    {
                        noOfHeaderMatches++;
                        headerId = aRow["HDR_ID"].ToString();
                    }
                }

                //maxSequence = GetMaxHeaderIdSequence(aRow["HDR_ID"].ToString(), maxSequence);
            }

            if (noOfHeaderMatches == 0)
            {
                // No Match
                //if (dtResults.Rows.Count > 0)
                //{
                //    //================================================================================
                //    // If rows were found, then their seq numbers where gathered to find the highest.
                //    // Increment that number by 1.
                //    //================================================================================
                //    maxSequence++;
                //}
            }
            else if (noOfHeaderMatches == 1)
            {
				// Begin TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction
                // On a Reset transaction, if we found a match, we also want to be sure we didn't find any non-released matches
                if (isReset)
                {
                    if (noOfNotReleasedMatches > 0)
                    {
                        string msgText = "Processing Transaction #" + _transactionNumber.ToString() + ".";
                        msgText += System.Environment.NewLine;

                        // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
						//msgText += string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_HeaderCannotBeReset,false), noOfNotReleasedMatches);
						msgText += string.Format(aSession.Audit.GetText(eMIDTextCode.msg_al_HeaderCannotBeReset, false), noOfNotReleasedMatches);
						// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed

                        //msgText += " Found a matching released header for this Reset transaction. Also found " + noOfNotReleasedMatches + " matching non-released header(s).";
                        //msgText += " Reset is rejected because it will cause duplicate non-released headers.";
                        // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
						//_sab.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, GetType().Name);	// TT#1679-MD - stodd - Header Load without Header ID returns Severe error when multiple headers are found to match - stopping processing  
                        aSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        // End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
                        msgText = "Processing Transaction #" + _transactionNumber.ToString() + ".";
                        msgText += " SQL:" + Environment.NewLine + matchSql;
                        // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
						//_sab.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msgText, GetType().Name);
                        aSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msgText, GetType().Name);
                        // End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed

                        returnCode = eReturnCode.severe;
                    }
                }
				// End TT#1649-MD - stodd - Change Header matching logic to look at "Shipping Started" indicator for a Reset transaction
				
                // Match
                _transactionHeader.HeaderId = headerId;
            }
            else
            {   // More than 1 match
                string msgText = "Processing Transaction #" + _transactionNumber.ToString() + ".";
                msgText += System.Environment.NewLine;
                msgText += " More than one header match was found for this transaction.";
                // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
				//_sab.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, GetType().Name);	// TT#1679-MD - stodd - Header Load without Header ID returns Severe error when multiple headers are found to match - stopping processing  
                aSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, GetType().Name);
                // End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
                msgText = "Processing Transaction #" + _transactionNumber.ToString() + ".";
                msgText += " SQL:" + Environment.NewLine + matchSql;
                // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
				//_sab.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msgText, GetType().Name);
                aSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msgText, GetType().Name);
                // End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed

                returnCode = eReturnCode.severe;
            }


            return returnCode;
        }

        /// <summary>
        /// determines a new header ID max sequence.
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="currMaxSequence"></param>
        /// <returns></returns>
        private int GetMaxHeaderIdSequence(string headerId, int currMaxSequence)
        {
            int maxSequence = currMaxSequence;
            int HeaderSequence = GetHeaderIdSequence(headerId);

            if (HeaderSequence > currMaxSequence)
            {
                maxSequence = HeaderSequence;
            }

            return maxSequence;
        }

        /// <summary>
        /// Strips the header ID sequence from the header ID.
        /// </summary>
        /// <param name="headerId"></param>
        /// <returns></returns>
        private int GetHeaderIdSequence(string headerId)
        {
            int headerSequence = 1;

            string headerSeq = headerId.Substring(headerId.Length - _headerIdSequenceLength, _headerIdSequenceLength);

            try
            {
                headerSequence = int.Parse(headerSeq);
            }
            catch
            {
                headerSequence = 1;
            }

            return headerSequence;
        }


        private void GetHeaderCharacteristicGroupInfo(string aHeaderCharGroupName, ref eHeaderCharType HdrCharGrpValueType, ref int hdrCharGroupRid)
        {
            DataTable HdrCharGrpTable = HeaderCharacteristicsData.HeaderCharGroup_Read(aHeaderCharGroupName);
            if (HdrCharGrpTable.Rows.Count > 0)
            {
                hdrCharGroupRid = int.Parse(HdrCharGrpTable.Rows[0]["HCG_RID"].ToString());
                HdrCharGrpValueType = (eHeaderCharType)int.Parse(HdrCharGrpTable.Rows[0]["HCG_TYPE"].ToString());
            }
            else
            {
                hdrCharGroupRid = Include.UndefinedHeaderGroupRID;
                HdrCharGrpValueType = eHeaderCharType.unknown;
            }
        }
        public eHeaderType GetHeaderType()
        {
            return GetHeaderType(_transactionHeader.HeaderType.ToString().ToUpper());
        }

        public eHeaderType GetHeaderType(string strHeaderType)
        {
            eHeaderType headerType = eHeaderType.Placeholder;
            //string strHeaderType = _transactionHeader.HeaderType.ToString().ToUpper();

            switch (strHeaderType)
            {
                case "RECEIPT":
                {
                    headerType = eHeaderType.Receipt;
                    break;
                }
                case "PO":
                {
                    headerType = eHeaderType.PurchaseOrder;
                    break;
                }
                case "ASN":
                {
                    headerType = eHeaderType.ASN;
                    break;
                }
                case "DUMMY":
                {
                    headerType = eHeaderType.Dummy;
                    break;
                }
                case "DROPSHIP":
                {
                    headerType = eHeaderType.DropShip;
                    break;
                }
                case "RESERVE":
                {
                    headerType = eHeaderType.Reserve;
                    break;
                }
                case "WORKUPTOTALBUY":
                {
                    headerType = eHeaderType.WorkupTotalBuy;
                    break;
                }
                case "VSW":
                {
                    headerType = eHeaderType.IMO;
                    break;
                }

                default:
                {
                    break;
                }
            }

            return headerType;
        }

        private static string GetTab()
        {
            string htab = string.Empty;
            for (int i = 1; i <= _tabLevel; i++)
            {
                htab += "\t";
            }
            return htab;
        }

    }

   
}
