using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
    /// <summary>
    /// Defines the general criteria that drives the allocation process.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CreateMasterHeadersMethod : AllocationBaseMethod
    {
        //=======
        // FIELDS
        //=======
        private CreateMasterHeadersMethodData _methodData;
        private bool _useSelectedHeaders;
        private DataTable _dtMerchandise = null;
        private DataTable _dtOverride = null;
        private Audit _audit;

        //=============
        // CONSTRUCTORS
        //=============
        public CreateMasterHeadersMethod(SessionAddressBlock SAB, int aMethodRID)
            : base(SAB, aMethodRID, eMethodType.CreateMasterHeaders, eProfileType.MethodCreateMasterHeaders)
        {
            if (base.Filled)
            {
                _methodData = new CreateMasterHeadersMethodData(base.Key, eChangeType.populate);
                _useSelectedHeaders = _methodData.UseSelectedHeaders;
                _dtMerchandise = _methodData.dtMerchandise;
                _dtOverride = _methodData.dtOverride;
            }
            else
            {
                _methodData = new CreateMasterHeadersMethodData(base.Key, eChangeType.populate);
                _useSelectedHeaders = _methodData.UseSelectedHeaders;
                _dtMerchandise = _methodData.dtMerchandise;
                _dtOverride = _methodData.dtOverride;
            }
        }

        //============
        // PROPERTIES
        //============
        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodCreateMasterHeaders;
            }
        }

        /// <summary>
        /// Gets or sets the Use Selected Headers flag.
        /// </summary>
        public bool UseSelectedHeaders
        {
            get { return _useSelectedHeaders; }
            set { _useSelectedHeaders = value; }
        }

        public DataTable dtMerchandise
        {
            get { return _dtMerchandise; }
            set { _dtMerchandise = value; }
        }

        public DataTable dtOverride
        {
            get { return _dtOverride; }
            set { _dtOverride = value; }
        }

        //========
        // METHODS
        //========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            foreach (DataRow dr in _dtMerchandise.Rows)
            {
                if (dr["FILTER_RID"] != DBNull.Value 
                    && IsFilterUser(Convert.ToInt32(dr["FILTER_RID"])))
                {
                    return true;
                }

                if (dr["HN_RID"] != DBNull.Value
                    && IsHierarchyNodeUser(Convert.ToInt32(dr["HN_RID"])))
                {
                    return true;
                }
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        public override void ProcessMethod(
            ApplicationSessionTransaction aApplicationTransaction,
            int aStoreFilter, Profile methodProfile)
        {
            WriteBeginProcessingMessage();

            aApplicationTransaction.ResetAllocationActionStatus();            

            AllocationWorkFlowStep awfs =
                            new AllocationWorkFlowStep(
                            this,
                            null, 
                            false,
                            true,
                            aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
                            aStoreFilter,
                            -1);
            this.ProcessAction(
                aApplicationTransaction.SAB,
                aApplicationTransaction,
                awfs,
                null,
                true,
                Include.NoRID);

            WriteEndProcessingMessage(aApplicationTransaction);
        }

        /// <summary>
        /// Processes the action associated with this method.
        /// </summary>
        /// <param name="aSAB">Session Address Block</param>
        /// <param name="aApplicationTransaction">An instance of the Application Transaction object</param>
        /// <param name="aAllocationWorkFlowStep">Workflow Step that describes parameters associated with this action.</param>
        /// <param name="aAllocationProfile">Allocation Profile to which to apply this action</param>
        /// <param name="WriteToDB">True: write results of action to database; False: Do not write results of action to database.</param>
        public override void ProcessAction(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aAllocationWorkFlowStep,
            Profile aAllocationProfile,
            bool WriteToDB,
            int aStoreFilterRID)
        {
            int apKey = 0;
            bool actionSuccess = true;

            _audit = aSAB.ApplicationServerSession.Audit;

            try
            {
                if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)aAllocationWorkFlowStep._method.MethodType))
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_WorkflowTypeInvalid),
                        MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
                }

                if (UseSelectedHeaders)
                {
                    if (!CreateMasterHeadersUsingSelectedHeaders(aApplicationTransaction))
                    {
                        actionSuccess = false;
                    }
                }
                else
                {
                    if (!CreateMasterHeadersUsingFilters(aApplicationTransaction))
                    {
                        actionSuccess = false;
                    }
                }

                if (actionSuccess)
                {
                    aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.ActionCompletedSuccessfully);
                }
                else
                {
                    aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.ActionFailed);
                }
            }
            catch (Exception error)
            {
                aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.ActionFailed);
                string message = error.ToString();
                throw;
            }
            finally
            {
                //ap.ResetTempLocks(true);
            }
        }

        private bool CreateMasterHeadersUsingSelectedHeaders(ApplicationSessionTransaction aApplicationTransaction)
        {
            try
            {
                AllocationProfileList headerList = (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation);
                if (headerList == null
                    || headerList.Count == 0)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_as_NoHeadersSelected, this.GetType().Name);
                    return false;
                }

                string message;
                if (!aApplicationTransaction.EnqueueSelectedHeaders(out message))
                {
                    _audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
                    return false;
                }

                return CreateMasterHeaders(aApplicationTransaction, headerList);
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                aApplicationTransaction.DequeueHeaders();
            }
        }

        private bool CreateMasterHeadersUsingFilters(ApplicationSessionTransaction aApplicationTransaction)
        {
            Header headerData = new Header();

            AllocationProfileList headerList = new AllocationProfileList(eProfileType.AllocationHeader);

            AllocationHeaderProfile headerProfile;
            AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

            foreach (DataRow dr in dtMerchandise.Rows)
            {
                int filterRID = Include.NoRID;
                if (dr["FILTER_RID"] != DBNull.Value && Convert.ToInt32(dr["FILTER_RID"], CultureInfo.CurrentUICulture) != Include.NoRID)
                {
                    filterRID = Convert.ToInt32(dr["FILTER_RID"], CultureInfo.CurrentUICulture);
                }

                int merchandiseOverrideHnRID = Include.NoRID;
                if (dr["HN_RID"] != DBNull.Value)
                {
                    merchandiseOverrideHnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                }

                FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                headerFilterOptions.USE_WORKSPACE_FIELDS = false;
                headerFilterOptions.filterType = filterTypes.HeaderFilter;
                headerFilterOptions.HN_RID_OVERRIDE = merchandiseOverrideHnRID;
                DataTable dtHeaders = headerData.GetHeadersFromFilter(filterRID, headerFilterOptions);

                foreach (DataRow fdr in dtHeaders.Rows)
                {
                    //headerProfile = new AllocationHeaderProfile(
                    //                                                Convert.ToString(fdr["HDR_ID"], CultureInfo.CurrentUICulture),
                    //                                                Convert.ToInt32(fdr["HDR_RID"], CultureInfo.CurrentUICulture));

                    headerProfile = SAB.HeaderServerSession.GetHeaderData(Convert.ToInt32(fdr["HDR_RID"], CultureInfo.CurrentUICulture), false, false, true);
                    if (headerProfile.HeaderGroupRID > 1  // cannot be in a group
                    || headerProfile.AsrtRID > 1  // cannot be in an assortment
                    || headerProfile.MultiHeader  // cannot be a multi header
                    || headerProfile.Placeholder  // cannot be a placeholder
                    || headerProfile.Assortment  // cannot be an assortment
                    || headerProfile.IsSubordinateHeader  // cannot already be in another master
                    || headerProfile.IsMasterHeader  // cannot be a master header
                    )
                    {
                        continue;
                    }

                    ahpl.Add(headerProfile);
                }
            }

            headerList.LoadHeaders(aApplicationTransaction, ahpl, SAB.ApplicationServerSession);

            return CreateMasterHeaders(aApplicationTransaction, headerList);
        }

        private bool CreateMasterHeaders(ApplicationSessionTransaction aApplicationTransaction, AllocationProfileList headerList)
        {
            bool actionSuccess = true; 

            // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
			//Dictionary<int, Dictionary<List<int>, List<AllocationProfile>>> dicHeadersForMaster = DetermineHeaders(aApplicationTransaction, headerList);
			Dictionary<int, Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>>> dicHeadersForMaster = DetermineHeaders(aApplicationTransaction, headerList);
			// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
            if (dicHeadersForMaster.Count == 0)
            {
                return true;
            }

            List<string> lMasterHeaderIdKeysList = new List<string>();
            List<string> lHeaderKeysToMatchList = new List<string>();
            if (!LoadHeaderKeys(ref lMasterHeaderIdKeysList, ref lHeaderKeysToMatchList))
            {
                return false;
            }

            HeaderIDGenerator headerIDGenerator = new HeaderIDGenerator(SAB);

            List<AllocationProfile> lHeaders = new List<AllocationProfile>();
            List<int> lColors = new List<int>();
			// Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
			//foreach (KeyValuePair<int, Dictionary<List<int>, List<AllocationProfile>>> styleColorHeaders in dicHeadersForMaster)
            //{
            //    int styleHNRID = styleColorHeaders.Key;
				
            Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>> dicStyleComponentHeaders; 

            foreach (KeyValuePair<int, Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>>> styleComponentHeaders in dicHeadersForMaster)
            {
                int styleHNRID = styleComponentHeaders.Key;
                dicStyleComponentHeaders = styleComponentHeaders.Value;
                foreach (KeyValuePair<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>> styleColorHeaders in dicStyleComponentHeaders)
                { 
				// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
                    Dictionary<List<int>, List<AllocationProfile>> dicColorHeaders = styleColorHeaders.Value;
                    foreach (KeyValuePair<List<int>, List<AllocationProfile>> colorHeaders in dicColorHeaders)
                    {
                        lColors = colorHeaders.Key;
                        lHeaders = colorHeaders.Value;
                        if (lHeaders.Count > 1)  // Do not create Master Header unless 2 or more like headers
                        {
                            actionSuccess = true;
                            string hdrId = null;
                            GenerateHeaderID(headerIDGenerator, lMasterHeaderIdKeysList, lHeaderKeysToMatchList, 0, null, lHeaders[0], ref hdrId);
                            if (_audit.LoggingLevel == eMIDMessageLevel.Debug)
                            {
                                WriteDebugInfo(styleHNRID, hdrId, lColors, lHeaders);
                            }

                            MasterHeaderProfile mhp = new MasterHeaderProfile(aApplicationTransaction, hdrId, Include.NoRID, SAB.ApplicationServerSession);

                            if (mhp.UpdatePropertiesFromHeader(lHeaders))
                            {
                                Header header = new Header();
                                try
                                {
                                    mhp.HeaderDataRecord = header;

                                    if (mhp.AddSubordinateHeaders(lHeaders))
                                    {
                                        try
                                        {
                                            header.OpenUpdateConnection();
                                            mhp.WriteHeader();  // need to issue Write to get key of master to tie to subordinates
                                            mhp.AddSubordinateHeaderReferences(lHeaders, header);
                                        }
                                        catch (Exception)
                                        {
                                            actionSuccess = false;
                                            throw;
                                        }
                                        finally
                                        {
                                            if (actionSuccess)
                                            {
                                                header.CommitData();
                                                header.CloseUpdateConnection();
                                            }
                                        }
                                        if (actionSuccess)
                                        {
                                            mhp.UpdateCharacteristics(lHeaders);
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }

                            if (actionSuccess)
                            {
                                aApplicationTransaction.WriteAllocationAuditInfo
                                    (mhp.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    eComponentType.Total,
                                    null,
                                    null,
                                    0,
                                    0
                                    );
                                foreach (AllocationProfile subordinate in lHeaders)
                                {
                                    aApplicationTransaction.WriteAllocationAuditInfo
                                    (subordinate.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    eComponentType.Total,
                                    null,
                                    null,
                                    0,
                                    0
                                    );
                                }
                            }
                        }
                    }
                }
            }  // TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header 

            return true;
        }

        private void WriteDebugInfo(int styleHNRID,
            string hdrId,
            List<int> lColors,
            List<AllocationProfile> lHeaders
            )
        {
            // debug code to write out headers to be used for master headers
            HierarchyNodeProfile hnp;
            ColorCodeProfile ccp;
            string message;

            hnp = SAB.HierarchyServerSession.GetNodeData(styleHNRID);
            message = "Master for style:" + hnp.Text;
            int iCnt = 0;
            if (lColors.Count == 0)
            {
                message += ";No Colors";
            }
            else
            {
                message += ";Colors:";
                foreach (int iColorCodeRID in lColors)
                {
                    if (iCnt > 0)
                    {
                        message += ",";
                    }
                    ccp = SAB.HierarchyServerSession.GetColorCodeProfile(iColorCodeRID);

                    message += ccp.ColorCodeID;
                    ++iCnt;
                }
            }
            _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);
            System.Threading.Thread.Sleep(1000);  // Wait a little so entries will sort right

            _audit.Add_Msg(eMIDMessageLevel.Debug, "Master Header ID:" + hdrId, this.GetType().Name);
            System.Threading.Thread.Sleep(1000);  // Wait a little so entries will sort right
            message = "Headers:";

            iCnt = 0;
            foreach (AllocationProfile ap in lHeaders)
            {
                if (iCnt > 0)
                {
                    message += ",";
                }
                message += ap.HeaderID;
                ++iCnt;
            }
            _audit.Add_Msg(eMIDMessageLevel.Debug, message, this.GetType().Name);

        }

        private bool LoadHeaderKeys(ref List<string> lMasterHeaderIdKeysList, ref List<string> lHeaderKeysToMatchList)
        {
            string errorMessage = string.Empty;
            List<string> lHeaderIdKeysList = new List<string>(); 
            bool headerKeyLoadedSuccessfully = SAB.ControlServerSession.LoadHeaderKeys(ref lHeaderIdKeysList, ref lMasterHeaderIdKeysList, ref lHeaderKeysToMatchList, ref errorMessage);
            if (!headerKeyLoadedSuccessfully)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
            }

            return headerKeyLoadedSuccessfully;
        }

        private bool GenerateHeaderID(
            HeaderIDGenerator headerIDGenerator, 
            List<string> lMasterHeaderIdKeysList, 
            List<string> lHeaderKeysToMatchList, 
            int iheaderIdSequenceLength,
            string sHeaderDelimiter,
            AllocationProfile ap,
            ref string hdrId
            )
        {
            bool isResetRemove = false;
            EditMsgs em = null;
            HeadersHeader hdrTrans = headerIDGenerator.ConvertAllocationProfileToTransaction(ap);
            hdrTrans.HeaderId = string.Empty;
            //eReturnCode rtnCode = headerIDGenerator.GetHeaderId(hdrTrans, 0, ref hdrId, ref isResetRemove, lMasterHeaderIdKeysList, lHeaderKeysToMatchList, iheaderIdSequenceLength, sHeaderDelimiter, ref em, false);
            eReturnCode rtnCode = SAB.ControlServerSession.GetHeaderId(hdrTrans, 0, ref hdrId, ref isResetRemove, lMasterHeaderIdKeysList, lHeaderKeysToMatchList, iheaderIdSequenceLength, sHeaderDelimiter, ref em, false);
            if (rtnCode == eReturnCode.successful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Separates headers by style and color.
        /// </summary>
        /// <param name="aApplicationTransaction">Application transaction</param>
        /// <param name="headerList">List of headers</param>
        /// <returns>Dictionary containing AllocationProfiles separated by style and color</returns>
        // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
		//private Dictionary<int, Dictionary<List<int>, List<AllocationProfile>>> DetermineHeaders(ApplicationSessionTransaction aApplicationTransaction, AllocationProfileList headerList)
		private Dictionary<int, Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>>> DetermineHeaders(ApplicationSessionTransaction aApplicationTransaction, AllocationProfileList headerList)
		// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
        {
            // Dictionary is keyed by style RID.  Data is a Dictionary of header component type with a dictionary with list of colors as key and list of AllocationProfile as data
            // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
			//Dictionary<int, Dictionary<List<int>, List<AllocationProfile>>> dicHeadersForMaster = new Dictionary<int, Dictionary<List<int>, List<AllocationProfile>>>();
			Dictionary<int, Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>>> dicHeadersForMaster = new Dictionary<int, Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>>>();
			// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
            Dictionary<List<int>, List<AllocationProfile>> dicStyleColorHeaders;
            List<AllocationProfile> lHeaders = new List<AllocationProfile>();
            List<int> lColors = new List<int>();
			// Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
			Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>> dicStyleComponentHeaders;
            eHeaderComponentType headerComponentType;
			// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header

            foreach (AllocationProfile ap in headerList)
            {
                // Make sure header is valid to be put in a master
                if (!ap.ReceivedInBalance
                    || ap.AllocationStarted
                    || ap.HeaderGroupRID > 1  // cannot be in a group
                    || ap.AsrtRID > 1  // cannot be in an assortment
                    || ap.MultiHeader  // cannot be a multi header
                    || ap.Placeholder  // cannot be a placeholder
                    || ap.Assortment  // cannot be an assortment
                    || ap.IsSubordinateHeader  // cannot already be in another master
                    || ap.IsMasterHeader  // cannot be a master header
                    )
                {
                    continue;
                }

                // Check if entry for style.  If not, create one
				// Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
                //if (!dicHeadersForMaster.TryGetValue(ap.StyleHnRID, out dicStyleColorHeaders))
                //{
                //    dicStyleColorHeaders = new Dictionary<List<int>, List<AllocationProfile>>();
                //    dicHeadersForMaster.Add(ap.StyleHnRID, dicStyleColorHeaders);
                //}

                if (!dicHeadersForMaster.TryGetValue(ap.StyleHnRID, out dicStyleComponentHeaders))
                {
                    dicStyleComponentHeaders = new Dictionary<eHeaderComponentType, Dictionary<List<int>, List<AllocationProfile>>>();
                    dicHeadersForMaster.Add(ap.StyleHnRID, dicStyleComponentHeaders);
                }
				// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header

                bool bContainsSizes = false;  // TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
                // Get list of all colors on header
                List<int> lColorCodeRIDs = new List<int>();
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    if (!lColorCodeRIDs.Contains(hcb.ColorCodeRID))
                    {
                        lColorCodeRIDs.Add(hcb.ColorCodeRID);
                        // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
						if (hcb.ColorSizes.Count > 0)
                        {
                            bContainsSizes = true;
                        }
						// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
                    }
                }
                foreach (PackHdr ph in ap.Packs.Values)
                {
                    foreach (PackColorSize packColor in ph.PackColors.Values)
                    {
                        if (!lColorCodeRIDs.Contains(packColor.ColorCodeRID))
                        {
                            lColorCodeRIDs.Add(packColor.ColorCodeRID);
                            // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
							if (packColor.ColorSizes.Count > 0)
                            {
                                bContainsSizes = true;
                            }
							// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
                        }
                    }
                }

                // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
				headerComponentType = eHeaderComponentType.DetailType;
                if (ap.BulkColorCount == 0
                    && ap.PackCount == 0)
                {
                    headerComponentType = eHeaderComponentType.Total;
                }
                else if (!bContainsSizes)
                {
                    headerComponentType = eHeaderComponentType.NoSize;
                }

                if (!dicStyleComponentHeaders.TryGetValue(headerComponentType, out dicStyleColorHeaders))
                {
                    dicStyleColorHeaders = new Dictionary<List<int>, List<AllocationProfile>>();
                    dicStyleComponentHeaders.Add(headerComponentType, dicStyleColorHeaders);
                }
				// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header

                // Search colors in list and add to AllocationProfile list
                bool bFoundMatch = false;
                foreach (KeyValuePair<List<int>, List<AllocationProfile>> colorHeaders in dicStyleColorHeaders)
                {
                    bFoundMatch = true;
                    lColors = colorHeaders.Key;
                    lHeaders = colorHeaders.Value;
                    // if the number of colors are not the same, no reason to compare
                    if (lColors.Count != lColorCodeRIDs.Count)
                    {
                        bFoundMatch = false;
                        continue;
                    }
                    // compare the colors
                    foreach (int iColorHeadersRID in lColors)
                    {
                        if (!lColorCodeRIDs.Contains(iColorHeadersRID))
                        {
                            bFoundMatch = false;
                            break;
                        }
                    }
                    if (bFoundMatch)
                    {
                        break;
                    }
                }

                if (!bFoundMatch)
                {
                    lHeaders = new List<AllocationProfile>();
                    dicStyleColorHeaders.Add(lColorCodeRIDs, lHeaders);
                }

                lHeaders.Add(ap);

            }

            return dicHeadersForMaster;
        }


        override public void Update(TransactionData td)
        {
            if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _methodData = new CreateMasterHeadersMethodData(td);
            }
            dtMerchandise.AcceptChanges();
            dtOverride.AcceptChanges();
            _methodData.UseSelectedHeaders = UseSelectedHeaders;
            _methodData.dtMerchandise = dtMerchandise;
            _methodData.dtOverride = dtOverride;

            try
            {

                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _methodData.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _methodData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _methodData.DeleteMethod(base.Key, td);
                        base.Update(td);
                        break;
                }
            }
            catch (Exception e)
            {
                string message = e.ToString();
                throw;
            }
            finally
            {
                //TO DO:  whatever has to be done after an update or exception.
            }
        }

        public override bool WithinTolerance(double aTolerancePercent)
        {
            return true;
        }

        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aCloneDateRanges">
        /// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
        /// <returns>
        /// A copy of the object.
        /// </returns>
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        {
            CreateMasterHeadersMethod newAllocationGeneralMethod = null;

            try
            {
                newAllocationGeneralMethod = (CreateMasterHeadersMethod)this.MemberwiseClone();
                newAllocationGeneralMethod.SG_RID = SG_RID;
                newAllocationGeneralMethod.UseSelectedHeaders = UseSelectedHeaders;
                newAllocationGeneralMethod.dtMerchandise = dtMerchandise.Copy();
                newAllocationGeneralMethod.dtOverride = dtOverride.Copy();

                return newAllocationGeneralMethod;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
        {
            return true;
        }

        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            return true;
        }

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalCreateMasterHeaders);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserCreateMasterHeaders);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROMethodCreateMasterHeadersProperties method = new ROMethodCreateMasterHeadersProperties(
               method: GetName.GetMethod(method: this),
               description: Method_Description,
               userKey: User_RID,
               useSelectedHeaders: _methodData.UseSelectedHeaders,
               listMerchandise: new System.Collections.Generic.List<ROMethodCreateMasterHeadersMerchandise>(),
               isTemplate: Template_IND
               );

            // Loop through each set
            ROMethodCreateMasterHeadersMerchandise myMerchandise = null;
            int seq = 0;
            foreach (DataRow dr in _methodData.dtMerchandise.Rows)
            {
                int merchandiseRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                int filterRID = Convert.ToInt32(dr["FILTER_RID"], CultureInfo.CurrentUICulture);
                seq += 1;
                myMerchandise = new ROMethodCreateMasterHeadersMerchandise(
                    sequence: seq,
                    merchandise: GetName.GetMerchandiseName(merchandiseRID, SAB),
                    filter: GetName.GetFilterName(filterRID)
                    );
                //myMerchandise.Merchandise = GetName.GetMerchandiseName(merchandiseRID, SAB);
                //myMerchandise.Filter = GetName.GetFilterName(filterRID);
                //myMerchandise.Sequence = seq;
                method.ListMerchandise.Add(myMerchandise);
            }

            return method;
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodCreateMasterHeadersProperties roMethodCreateMasterHeadersProperties = (ROMethodCreateMasterHeadersProperties)methodProperties;
            try
            {
                Template_IND = methodProperties.IsTemplate;
                _methodData.UseSelectedHeaders = roMethodCreateMasterHeadersProperties.UseSelectedHeaders;
                UseSelectedHeaders = roMethodCreateMasterHeadersProperties.UseSelectedHeaders;
                _methodData.Method_Name = roMethodCreateMasterHeadersProperties.Method.Value;

                // Building the dtMerchandise dataTable 
                _methodData.dtMerchandise.Rows.Clear();
                int i = 0;

                foreach (ROMethodCreateMasterHeadersMerchandise listMerchandise in roMethodCreateMasterHeadersProperties.ListMerchandise)
                {
                    i += 1;
                    // The dtMerchandise Data Table has Filter RID in columns 4 and 5
                    _methodData.dtMerchandise.Rows.Add(new object[] { roMethodCreateMasterHeadersProperties.Method.Key, i, listMerchandise.Merchandise.Value, listMerchandise.Merchandise.Key, listMerchandise.Filter.Key, listMerchandise.Filter.Key });
                }


                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            //    throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }

    // Begin TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header
    public enum eHeaderComponentType
    {
        Total,
        NoSize,
        DetailType
    }
	// End TT#2084-MD - JSmith - Header with incompatible components are combining to the same master header


}

