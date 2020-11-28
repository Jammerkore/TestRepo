using System;
using System.Collections;
using System.Collections.Generic;	// TT#936 - MD - Prevent the saving of empty Group Allocations
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// HeaderServerGlobal is a static class that contains fields that are global to all HeaderServerSession objects.
	/// </summary>
	/// <remarks>
	/// The HeaderServerGlobal class is used to store information that is global to all HeaderServerSession objects
	/// within the process.  A common use for this class would be to cache static information from the database in order to
	/// reduce accesses to the database.
	/// </remarks>

	public class HeaderServerGlobal : Global
	{
		// Fields that are global to the Header Service

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private ArrayList _loadLock;
		static private bool _loaded;
		static private Audit _audit;

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private MIDReaderWriterLock header_rwl = new MIDReaderWriterLock();
		static private MIDReaderWriterLock enqueue_rwl = new MIDReaderWriterLock();
		static private MIDReaderWriterLock headerChar_rwl = new MIDReaderWriterLock();

		static private Header _headerData = null;
		static private HeaderCharacteristicsData _headerCharacteristicsData = null;
        //static private Hashtable _headerGroupsByRID = new Hashtable();  //  contains all info by record id //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -hashtable not used
        //static private Hashtable _headerGroupsByID = new Hashtable();  //  xref from name to record id //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -hashtable not used

		// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
        //static private Hashtable _headersByRID = new Hashtable();   //  contains all info by record id 
        static private DataTable _dtHeaderStyles; //TT#1477-MD -jsobek -Header Filter Sort on Workspace
        //static private Hashtable _headersByID = new Hashtable();   //  xref from name to record id
        //static private Hashtable _packsByRID = new Hashtable();   //  contains all info by record id 

        static private Dictionary<int, HeaderInfo> _headersByRIDAllocation = new Dictionary<int,HeaderInfo>();   //  contains all info by record id 
        //static private Dictionary<int, List<int>> _headersByStyle = new Dictionary<int,List<int>>();   //  contains all headers for a style 
        static private Dictionary<string, int> _headersByIDAllocation = new Dictionary<string,int>();   //  xref from name to record id
        static private Dictionary<int, HeaderPackInfo> _packsByRID = new Dictionary<int,HeaderPackInfo>();   //  contains all info by record id 

        static private Dictionary<int, HeaderInfo> _headersByRIDAssortment = new Dictionary<int,HeaderInfo>();   //  contains all info by record id 
        static private Dictionary<string, int> _headersByIDAssortment = new Dictionary<string,int>();   //  xref from name to record id
		// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers

        //static private ArrayList _assortmentRIDs = new ArrayList(); // TT#2 - Ron Matelic - Assortment Planning 4.0  //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -no reason to cache assortment RIDS

		static private Hashtable _headerCharGroupsByRID = new Hashtable();  //  contains all info by record id
		static private SortedList _headerCharGroupsByID = new SortedList();  //  xref from name to record id
		static private Hashtable _headerCharsByRID = new Hashtable();  //  xref from name to record id
		
		/// <summary>
		/// Creates a new instance of HeaderServerGlobal
		/// </summary>

		static HeaderServerGlobal()
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				_loadLock = new ArrayList();
				_loaded = false;

				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				if (!EventLog.SourceExists("MIDHeaderService"))
				{
					EventLog.CreateEventSource("MIDHeaderService", null);
				}
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDHeaderService", ex.Message, EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// The Load method is called by the service or client to trigger the instantiation of the static HeaderServerGlobal
		/// object.  
		/// </summary>

        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
        //static public void Load()
        static public void Load(bool aLocal)
        // End TT#189
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//LoadBase(eProcesses.headerService);
				lock (_loadLock.SyncRoot)
				{
					if (!_loaded)
					{
                        //Begin TT#5320-VStuart-deadlock issues-FinishLine
                        if (!aLocal)
                        {
                            MarkRunningProcesses(eProcesses.headerService);  // TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
                        }
                        //End TT#5320-VStuart-deadlock issues-FinishLine

                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //_audit = new Audit(eProcesses.headerService, Include.AdministratorUserRID);
                        if (!aLocal)
                        {
                            _audit = new Audit(eProcesses.headerService, Include.AdministratorUserRID);
                        }
                        // End TT#189  
                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        int messagingInterval = Include.Undefined;
                        object parm = MIDConfigurationManager.AppSettings["MessagingInterval"];
                        if (parm != null)
                        {
                            messagingInterval = Convert.ToInt32(parm);
                        }
                        //LoadBase();
                        LoadBase(eMIDMessageSenderRecepient.headerService, messagingInterval, aLocal, eProcesses.headerService);
                        // End TT#2307
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
						_headerData = new Header(); // create data layer object
						_headerCharacteristicsData = new HeaderCharacteristicsData(); // create data layer object
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_StartedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        if (Audit != null)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_StartedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        }
                        else
                        {
                            //string message = "Started building global area";
                            // Begin TT#797 - JSmith - Remove Windows Event Log messages from Header Service
                            //EventLog.WriteEntry("MIDHeaderService", message, EventLogEntryType.Information);
                            // End TT#797
                        }
                        // End TT#189

						DateTime startTime = DateTime.Now;
						BuildHeaderCharacteristicData();
						//BuildHeaderData();
						TimeSpan duration = DateTime.Now.Subtract(startTime);
						string strDuration = Convert.ToString(duration, CultureInfo.CurrentUICulture);
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
						//Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_FinishedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        if (Audit != null)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_FinishedBuildingGlobal, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        }
                        else
                        {
                            //string message = "Finished building global area";
                            // Begin TT#797 - JSmith - Remove Windows Event Log messages from Header Service
                            //EventLog.WriteEntry("MIDHeaderService", message, EventLogEntryType.Information);
                            // End TT#797
                        }
                        // End TT#189
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        // Begin TT#195 MD - JSmith - Add environment authentication
                        if (!aLocal)
                        {
                            RegisterServiceStart();
                        }
                        // End TT#195 MD

						_loaded = true;
					}
				}
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDHeaderService", ex.Message, EventLogEntryType.Information);
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        static public void CloseAudit()
        {
            try
            {
				// Begin TT#1303 - stodd - null ref
				if (Audit != null)
				{
					Audit.CloseUpdateConnection();
				}
				// End TT#1303 - stodd - null ref
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		/// <summary>
		/// Cleans up all resources for the service
		/// </summary>

		static public void CleanUp()
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                if (isExecutingLocal &&
                    MessageProcessor.isListeningForMessages)
                {
                    StopMessageListener();
                }
                // End TT#2307

				if (Audit != null)
				{
					Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", Audit.HighestMessageLevel);
                    // Begin TT#1243 - JSmith - Audit Performance
                    Audit.CloseUpdateConnection();
                    // End TT#1243
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		// Properties to access the global fields

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private Audit Audit
		{
			get
			{
				return _audit;
			}
		}

		static public bool Loaded
		{
			get
			{
				return _loaded;
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

        //Begin TT#1313-MD -jsobek -Header Filters -unused property
		///// <summary>
		///// Gets the HeaderInfo object for the requested index.
		///// </summary>
		///// 
        //public HeaderInfo this[int HiIdx]
        //{
        //    get
        //    {
        //        return (HeaderInfo)_headersByRID[HiIdx];
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused property

        //Begin TT#1313-MD -jsobek -Header Filters -unused function
		///// <summary>
		///// Requests a lock on the enqueue resource
		///// </summary>
        //static public void AcquireEnqueueWriterLock()
        //{
        //    try
        //    {
        //        enqueue_rwl.AcquireWriterLock(WriterLockTimeOut);		
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // End TT#189
        //        // The writer lock request timed out.
        //        EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:AcquireEnqueueWriterLock writer lock has timed out", EventLogEntryType.Error);
        //        throw new MIDException (eErrorLevel.severe,	0, "MIDHeaderService:AcquireEnqueueWriterLock writer lock has timed out");
        //    }
        //}
        

		///// <summary>
		///// Releases a lock on the enqueue resource
		///// </summary>
        //static public void ReleaseEnqueueWriterLock()
        //{
        //    try
        //    {
        //        enqueue_rwl.ReleaseWriterLock();		
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // End TT#189
        //        // The writer lock request timed out.
        //        EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:ReleaseEnqueueWriterLock writer lock has timed out", EventLogEntryType.Error);
        //        throw new MIDException (eErrorLevel.severe,	0, "MIDHeaderService:ReleaseEnqueueWriterLock writer lock has timed out");
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function

		static public void SetPostingDate(DateTime postingDate)
		{
			try
			{
				if (Calendar.PostDate == null ||
					postingDate.Date != Calendar.PostDate.Date.Date)
				{
					Calendar.SetPostingDate(postingDate);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Reads the header from the database and populates the header cross reference tables
		/// </summary>
		/// <remarks>
		/// This routine only build header data for the user since this is not functioning as a shared service
		/// </remarks>
        private static void BuildHeaderDataForWorkspace(int headerFilterRID, FilterHeaderOptions headerFilterOptions) 
		{
			try
			{
                //Clear the header cross reference tables
				// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                //_headersByRID.Clear();
                //_headersByStyle.Clear();
                //_headersByID.Clear();
                //_packsByRID.Clear();
				// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers

                bool masterHeadersExist = false;
                Hashtable HashMasterHeader = null;
                Hashtable HashSubordinateHeader = null;
                int masterHeaderCount = 0;	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                //if ((bool)isMasterAllocationInstalled)
                //{
                //    DataTable DtMasterSubordinate = _headerData.GetMasterSubordinates();
                //    if (DtMasterSubordinate.Rows.Count > 0)
                //    {
                //        masterHeadersExist = true;
                //        HashMasterHeader = new Hashtable();
                //        HashSubordinateHeader = new Hashtable();
                //        foreach (DataRow dr in DtMasterSubordinate.Rows)
                //        {
                //            int masterRID = Convert.ToInt32(dr["MASTER_HDR_RID"], CultureInfo.CurrentUICulture);
                //            int subordinateRID = Convert.ToInt32(dr["SUBORD_HDR_RID"], CultureInfo.CurrentUICulture);
                //            // Begin TT#1746-MD - RMatelic - Rule Method with master header receives null reference when Process button is clicked 
                //            //                             >>> semi-related - multiple rows caused duplicate hash key error 
                //            //HashMasterHeader.Add(masterRID, null);
                //            //HashSubordinateHeader.Add(subordinateRID, null);
                //            if (!HashMasterHeader.ContainsKey(masterRID))
                //            {
                //                HashMasterHeader.Add(masterRID, null);
                //            }
                //            if (!HashSubordinateHeader.ContainsKey(subordinateRID))
                //            {
                //                HashSubordinateHeader.Add(subordinateRID, null);
                //            }
                //            // End TT#1746-MD
                //        }
                //        masterHeaderCount = HashMasterHeader.Count + HashSubordinateHeader.Count;	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                //    }
                //}
                // End TT#1966-MD - JSmith- DC Fulfillment

                DataTable dt = _headerData.GetHeadersFromFilter(headerFilterRID, headerFilterOptions);
				// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                //Begin TT#1477-MD -jsobek -Header Filter Sort on Workspace
                _dtHeaderStyles = dt.DefaultView.ToTable(false, new string[] { "HDR_RID", "STYLE_HNRID" });
                //End TT#1477-MD -jsobek -Header Filter Sort on Workspace

                bool forAssortment = false;                
                if (headerFilterOptions.filterType == filterTypes.HeaderFilter)
                {
                    _headersByRIDAllocation = new Dictionary<int, HeaderInfo>(dt.Rows.Count + masterHeaderCount);
                    _headersByIDAllocation = new Dictionary<string, int>(dt.Rows.Count + masterHeaderCount);
                    _packsByRID = new Dictionary<int, HeaderPackInfo>();
                }
                else
                {
                    _headersByRIDAssortment = new Dictionary<int, HeaderInfo>(dt.Rows.Count);
                    _headersByIDAssortment = new Dictionary<string, int>(dt.Rows.Count);
                    forAssortment = true;
                }
				// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                foreach (DataRow dr in dt.Rows)
                {
                    PopulateHeaderCrossReferenceFromDataRow(dr, masterHeadersExist, HashMasterHeader, HashSubordinateHeader, forAssortment);	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                }
			}
			catch ( Exception ex )
			{
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
				throw;
			}
		}

        private static void PopulateHeaderCrossReferenceFromDataRow(DataRow dr, bool masterHeadersExist, Hashtable HashMasterHeader, Hashtable HashSubordinateHeader, bool forAssortment)
        {
            try
            {
				// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                if (!forAssortment && _headersByRIDAllocation == null)
                {
                    forAssortment = true;
                }
				// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
				
                HeaderInfo hi = new HeaderInfo();

                hi.HeaderRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
                hi.HeaderID = dr["HDR_ID"].ToString();
                hi.HeaderDescription = dr["HDR_DESC"].ToString();
                hi.AllocationNotes = dr["HEADER_NOTES"].ToString();
                if (!Convert.IsDBNull(dr["HDR_DAY"]))
                {
                    hi.HeaderDay = Convert.ToDateTime(dr["HDR_DAY"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["ORIG_DAY"]))
                {
                    hi.OriginalReceiptDay = Convert.ToDateTime(dr["ORIG_DAY"], CultureInfo.CurrentUICulture);
                }
                hi.UnitRetail = Convert.ToDouble(dr["UNIT_RETAIL"], CultureInfo.CurrentUICulture);
                hi.UnitCost = Convert.ToDouble(dr["UNIT_COST"], CultureInfo.CurrentUICulture);
                hi.TotalUnitsToAllocate = Convert.ToInt32(dr["UNITS_RECEIVED"], CultureInfo.CurrentUICulture);
                if (!Convert.IsDBNull(dr["STYLE_HNRID"]))
                {
                    hi.StyleHnRID = Convert.ToInt32(dr["STYLE_HNRID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.StyleHnRID = Include.NoRID;
                }
                hi.PlanHnRID = Convert.ToInt32(dr["PLAN_HNRID"], CultureInfo.CurrentUICulture);
                hi.OnHandHnRID = Convert.ToInt32(dr["ON_HAND_HNRID"], CultureInfo.CurrentUICulture);
                hi.BulkMultiple = Convert.ToInt32(dr["BULK_MULTIPLE"], CultureInfo.CurrentUICulture);
                hi.AllocationMultiple = Convert.ToInt32(dr["ALLOCATION_MULTIPLE"], CultureInfo.CurrentUICulture);
                if (Convert.IsDBNull(dr["ALLOCATION_MULTIPLE_DEFAULT"]))
                {
                    hi.AllocationMultipleDefault = 1;
                }
                else
                {
                    hi.AllocationMultipleDefault = Convert.ToInt32(dr["ALLOCATION_MULTIPLE_DEFAULT"], CultureInfo.CurrentUICulture); // MID Track 5761 Allocation Multiple not saved to Database
                }
                hi.Vendor = dr["VENDOR"].ToString();
                hi.DistributionCenter = dr["DIST_CENTER"].ToString();
                hi.PurchaseOrder = dr["PURCHASE_ORDER"].ToString();
                if (!Convert.IsDBNull(dr["BEGIN_DAY"]))
                {
                    hi.BeginDay = Convert.ToDateTime(dr["BEGIN_DAY"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["NEED_DAY"]))
                {
                    hi.LastNeedDay = Convert.ToDateTime(dr["NEED_DAY"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["SHIP_TO_DAY"]))
                {
                    hi.ShipDay = Convert.ToDateTime(dr["SHIP_TO_DAY"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["EARLIEST_SHIP_DAY"]))
                {
                    hi.EarliestShipDay = Convert.ToDateTime(dr["EARLIEST_SHIP_DAY"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["RELEASE_DATETIME"]))
                {
                    hi.ReleaseDate = Convert.ToDateTime(dr["RELEASE_DATETIME"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["RELEASE_APPROVED_DATETIME"]))
                {
                    hi.ReleaseApprovedDate = Convert.ToDateTime(dr["RELEASE_APPROVED_DATETIME"], CultureInfo.CurrentUICulture);
                }
                hi.HeaderGroupRID = Convert.ToInt32(dr["HDR_GROUP_RID"], CultureInfo.CurrentUICulture);
                hi.SizeGroupRID = Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
                hi.WorkflowRID = Convert.ToInt32(dr["WORKFLOW_RID"], CultureInfo.CurrentUICulture);
                if (!dr.IsNull("WORKFLOW_TRIGGER"))
                {
                    hi.WorkflowTrigger = Include.ConvertCharToBool(Convert.ToChar(dr["WORKFLOW_TRIGGER"], CultureInfo.CurrentUICulture));
                }
                if (!dr.IsNull("API_WORKFLOW_RID"))
                {
                    hi.API_WorkflowRID = Convert.ToInt32(dr["API_WORKFLOW_RID"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("API_WORKFLOW_TRIGGER"))
                {
                    hi.API_WorkflowTrigger = Include.ConvertCharToBool(Convert.ToChar(dr["API_WORKFLOW_TRIGGER"], CultureInfo.CurrentUICulture));
                }
                hi.MethodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
                hi._allocationStatusFlags.AllFlags = Convert.ToUInt32(dr["ALLOCATION_STATUS_FLAGS"], CultureInfo.CurrentUICulture); // TT#246 - MD - JEllis - AnF VSW Size In Store Minimums
                hi._balanceStatusFlags.AllFlags = Convert.ToUInt16(dr["BALANCE_STATUS_FLAGS"], CultureInfo.CurrentUICulture);
                hi._shippingStatusFlags.AllFlags = Convert.ToByte(dr["SHIPPING_STATUS_FLAGS"], CultureInfo.CurrentUICulture);
                hi._allocationTypeFlags.AllFlags = Convert.ToUInt32(dr["ALLOCATION_TYPE_FLAGS"], CultureInfo.CurrentUICulture);
                hi._intransitUpdateStatusFlags.AllFlags = Convert.ToByte(dr["INTRANSIT_STATUS_FLAGS"], CultureInfo.CurrentUICulture);
                if (!dr.IsNull("PERCENT_NEED_LIMIT"))
                {
                    hi.PercentNeedLimit = Convert.ToDouble(dr["PERCENT_NEED_LIMIT"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("PLAN_PERCENT_FACTOR"))
                {
                    hi.PlanFactor = Convert.ToDouble(dr["PLAN_PERCENT_FACTOR"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("RESERVE_UNITS"))
                {
                    hi.ReserveUnits = Convert.ToInt32(dr["RESERVE_UNITS"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["ALLOCATED_UNITS"]))
                {
                    hi.AllocatedUnits = Convert.ToInt32(dr["ALLOCATED_UNITS"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["ORIG_ALLOCATED_UNITS"]))
                {
                    hi.OrigAllocatedUnits = Convert.ToInt32(dr["ORIG_ALLOCATED_UNITS"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["RSV_ALLOCATED_UNITS"]))
                {
                    hi.RsvAllocatedUnits = Convert.ToInt32(dr["RSV_ALLOCATED_UNITS"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["RELEASE_COUNT"]))
                {
                    hi.ReleaseCount = Convert.ToInt32(dr["RELEASE_COUNT"], CultureInfo.CurrentUICulture);
                }
                if (!Convert.IsDBNull(dr["GRADE_WEEK_COUNT"]))
                {
                    hi.GradeWeekCount = Convert.ToInt32(dr["GRADE_WEEK_COUNT"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("DISPLAY_STATUS"))
                {
                    hi.HeaderAllocationStatus = (eHeaderAllocationStatus)Convert.ToInt32(dr["DISPLAY_STATUS"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("DISPLAY_TYPE"))
                {
                    hi.HeaderType = (eHeaderType)Convert.ToInt32(dr["DISPLAY_TYPE"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("DISPLAY_INTRANSIT"))
                {
                    hi.HeaderIntransitStatus = (eHeaderIntransitStatus)Convert.ToInt32(dr["DISPLAY_INTRANSIT"], CultureInfo.CurrentUICulture);
                }
                if (!dr.IsNull("DISPLAY_SHIP_STATUS"))
                {
                    hi.HeaderShipStatus = (eHeaderShipStatus)Convert.ToInt32(dr["DISPLAY_SHIP_STATUS"], CultureInfo.CurrentUICulture);
                }

                if (!dr.IsNull("ASRT_RID"))
                {
                    hi.AsrtRID = Convert.ToInt32(dr["ASRT_RID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.AsrtRID = Include.NoRID;
                }




                if (!dr.IsNull("PLACEHOLDER_RID"))
                {
                    hi.PlaceHolderRID = Convert.ToInt32(dr["PLACEHOLDER_RID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.PlaceHolderRID = Include.NoRID;
                }
                if (!dr.IsNull("ASRT_TYPE"))
                {
                    hi.AsrtType = Convert.ToInt32(dr["ASRT_TYPE"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.AsrtType = 0;
                }

                // Begin TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
                if (!dr.IsNull("ASRT_TYPE_PARENT"))
                {
                    hi.AsrtTypeForParentAsrt = Convert.ToInt32(dr["ASRT_TYPE_PARENT"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.AsrtTypeForParentAsrt = 0;
                }
                // End TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
                // Begin TT#1227 - stodd assortment
                if (!dr.IsNull("ASRT_PLACEHOLDER_SEQ"))
                {
                    hi.AsrtPlaceholderSeq = Convert.ToInt32(dr["ASRT_PLACEHOLDER_SEQ"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.AsrtPlaceholderSeq = Include.UndefinedPlaceholderSeq;
                }
                if (!dr.IsNull("ASRT_HEADER_SEQ"))
                {
                    hi.AsrtHeaderSeq = Convert.ToInt32(dr["ASRT_HEADER_SEQ"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.AsrtHeaderSeq = Include.UndefinedHeaderSeq;
                }
                // End TT#1227 - stodd assortment

                // Begin TT#1966-MD - JSmith- DC Fulfillment
                //if (masterHeadersExist)
                //{
                //    DataTable masterdt;
                //    if (HashSubordinateHeader.ContainsKey(hi.HeaderRID))
                //    {
                //        masterdt = _headerData.GetComponentsForSubord(hi.HeaderRID);  //TO DO 1170-MD -performance issue -do not call for each header
                //        if (masterdt.Rows.Count > 0)
                //        {
                //            hi.MasterRID = Convert.ToInt32(masterdt.Rows[0]["MASTER_HDR_RID"], CultureInfo.CurrentUICulture);
                //            hi.MasterID = Convert.ToString(masterdt.Rows[0]["MASTER_HDR_ID"], CultureInfo.CurrentUICulture);
                //        }
                //    }
                //    else if (HashMasterHeader.ContainsKey(hi.HeaderRID))
                //    {
                //        masterdt = _headerData.GetComponentsForMaster(hi.HeaderRID); //TO DO 1170-MD -performance issue -do not call for each header
                //        if (masterdt.Rows.Count > 0)
                //        {
                //            hi.SubordinateRID = Convert.ToInt32(masterdt.Rows[0]["SUBORD_HDR_RID"], CultureInfo.CurrentUICulture);
                //            hi.SubordinateID = Convert.ToString(masterdt.Rows[0]["SUBORD_HDR_ID"], CultureInfo.CurrentUICulture);
                //        }
                //    }
                //}

                hi.MasterRID = Convert.ToInt32(dr["MASTER_RID"], CultureInfo.CurrentUICulture);
                hi.MasterID = Convert.ToString(dr["MASTER_ID"], CultureInfo.CurrentUICulture);
                DataTable dt = _headerData.GetComponentsForMaster(hi.HeaderRID);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow drSub in dt.Rows)
                    {
                        hi.SubordinateRIDs.Add(Convert.ToInt32(drSub["SUBORD_HDR_RID"], CultureInfo.CurrentUICulture));
                    }
                }
               // End TT#1966-MD - JSmith- DC Fulfillment

                if (!dr.IsNull("MANUALLYCHGDSTRSTYLALOCTNCNT"))
                {
                    hi.StoreStyleAllocationManuallyChangedCount = Convert.ToInt32(dr["MANUALLYCHGDSTRSTYLALOCTNCNT"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.StoreStyleAllocationManuallyChangedCount = 0;
                }
                if (!dr.IsNull("MANUALLYCHGDSTRSIZEALOCTNCNT"))
                {
                    hi.StoreSizeAllocationManuallyChangedCount = Convert.ToInt32(dr["MANUALLYCHGDSTRSIZEALOCTNCNT"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.StoreSizeAllocationManuallyChangedCount = 0;
                }
                if (!dr.IsNull("STORES_WITH_ALOCTN_COUNT"))
                {
                    hi.StoresWithAllocationCount = Convert.ToInt32(dr["STORES_WITH_ALOCTN_COUNT"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.StoresWithAllocationCount = 0;

                }

                if (!dr.IsNull("MANUALLYCHGDSTRSTYLEALOCTN"))
                {
                    hi.StoreStyleManualAllocationTotal = Convert.ToInt32(dr["MANUALLYCHGDSTRSTYLEALOCTN"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.StoreStyleManualAllocationTotal = 0;
                }
                if (!dr.IsNull("MANUALLYCHGDSTRSIZEALOCTN"))
                {
                    hi.StoreSizeManualAllocationTotal = Convert.ToInt32(dr["MANUALLYCHGDSTRSIZEALOCTN"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.StoreSizeManualAllocationTotal = 0;
                }
                if (!dr.IsNull("HORIZON_OVERRIDE"))
                {
                    hi.HorizonOverride = Include.ConvertCharToBool(Convert.ToChar(dr["HORIZON_OVERRIDE"], CultureInfo.CurrentUICulture));
                }


                //Begin TT#1313-MD -jsobek -Header Filters
                if (dr.Table.Columns.Contains("PackCount"))
                {
                    hi.PackCount = Convert.ToInt32(dr["PackCount"], CultureInfo.CurrentUICulture);
                    hi.BulkColorCount = Convert.ToInt32(dr["ColorCount"], CultureInfo.CurrentUICulture);
                    hi.BulkColorSizeCount = Convert.ToInt32(dr["SizeCount"], CultureInfo.CurrentUICulture);
                }
                if (dr.Table.Columns.Contains("NodeDisplayOtsForecast"))
                {
                    hi.NodeDisplayForOtsForecast = Convert.ToString(dr["NodeDisplayOtsForecast"], CultureInfo.CurrentUICulture);
                    hi.NodeDisplayForOnHand = Convert.ToString(dr["NodeDisplayForOnHand"], CultureInfo.CurrentUICulture);
                    hi.NodeDisplayForGradeInvBasis = Convert.ToString(dr["NodeDisplayForGradeInvBasis"], CultureInfo.CurrentUICulture);
                    hi.WorkflowName = Convert.ToString(dr["WorkflowName"], CultureInfo.CurrentUICulture);
                    hi.HeaderMethodName = Convert.ToString(dr["HeaderMethodName"], CultureInfo.CurrentUICulture);
                    hi.APIWorkflowName = Convert.ToString(dr["APIWorkflowName"], CultureInfo.CurrentUICulture);
                }
                //End TT#1313-MD -jsobek -Header Filters



                if (!dr.IsNull("GRADE_SG_RID"))
                {
                    hi.GradeSG_RID = Convert.ToInt32(dr["GRADE_SG_RID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.GradeSG_RID = Include.AllStoreGroupRID;
                }

                if (dr.IsNull("GRADE_INVENTORY_HNRID"))
                {
                    hi.GradeInventoryBasisHnRID = Include.NoRID;
                    hi.GradeInventoryMinMax = false;
                }
                else
                {
                    hi.GradeInventoryBasisHnRID = Convert.ToInt32(dr["GRADE_INVENTORY_HNRID"], CultureInfo.CurrentUICulture);
                    if (hi.GradeInventoryBasisHnRID > 0)
                    {
                        hi.GradeInventoryMinMax = Include.ConvertCharToBool(Convert.ToChar(dr["GRADE_INVENTORY_IND"], CultureInfo.CurrentUICulture));
                    }
                    else
                    {
                        hi.GradeInventoryMinMax = false;
                    }
                }

                hi.ImoID = Convert.ToString(dr["IMO_ID"], CultureInfo.CurrentUICulture);

                if (!dr.IsNull("ITEM_UNITS_ALLOCATED"))
                {
                    hi.ItemUnitsAllocated = Convert.ToInt32(dr["ITEM_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.ItemUnitsAllocated = 0;
                }
                if (!dr.IsNull("ITEM_ORIG_UNITS_ALLOCATED"))
                {
                    hi.ItemOrigUnitsAllocated = Convert.ToInt32(dr["ITEM_ORIG_UNITS_ALLOCATED"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.ItemOrigUnitsAllocated = 0;
                }


                if (dr.Table.Columns.Contains("ASRT_ID") && !dr.IsNull("ASRT_ID"))	// TT#880 - MD - stodd - Augment exception w/o assortment installed
                {
                    hi.AssortmentID = Convert.ToString(dr["ASRT_ID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.ItemOrigUnitsAllocated = 0;
                }

                // Begin TT#1652-MD - RMatelic - DC Carton Rounding
                if (!dr.IsNull("UNITS_PER_CARTON"))
                {
                    hi.UnitsPerCarton = Convert.ToInt32(dr["UNITS_PER_CARTON"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    hi.UnitsPerCarton = 0;
                }
                // End TT#1652-MD

                // Begin TT#1966-MD - JSmith- DC Fulfillment
                if (!dr.IsNull("DC_FULFILLMENT_PROCESSED_IND"))
                {
                    hi.DCFulfillmentProcessed = Include.ConvertCharToBool(Convert.ToChar(dr["DC_FULFILLMENT_PROCESSED_IND"], CultureInfo.CurrentUICulture));
                }
                else
                {
                    hi.DCFulfillmentProcessed = false;
                }
                // End TT#1966-MD - JSmith- DC Fulfillment

				// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                if (forAssortment)
                {
                    if (_headersByRIDAssortment.ContainsKey(hi.HeaderRID))
                    {
                        _headersByRIDAssortment.Remove(hi.HeaderRID);
                    }
                    _headersByRIDAssortment.Add(hi.HeaderRID, hi);
                    //_headersByID.Add(hi.HeaderID, hi.HeaderRID);        
                    if (!_headersByIDAssortment.ContainsKey(hi.HeaderID))
                    {
                        _headersByIDAssortment.Add(hi.HeaderID, hi.HeaderRID);
                    }
                }
                else
                {

                    if (_headersByRIDAllocation.ContainsKey(hi.HeaderRID))
                    {
                        _headersByRIDAllocation.Remove(hi.HeaderRID);
                    }
                    _headersByRIDAllocation.Add(hi.HeaderRID, hi);
                    //_headersByID.Add(hi.HeaderID, hi.HeaderRID);        
                    if (!_headersByIDAllocation.ContainsKey(hi.HeaderID))
                    {
                        _headersByIDAllocation.Add(hi.HeaderID, hi.HeaderRID);
                    }
                }

                // add header to style cross reference
                //List<int> styleHeaders = new List<int>();
                //if (_headersByStyle.ContainsKey(hi.StyleHnRID))
                //{   
                //    styleHeaders = (List<int>)_headersByStyle[hi.StyleHnRID];
                //    if (!styleHeaders.Contains(hi.HeaderRID))
                //    {
                //        styleHeaders.Add(hi.HeaderRID);
                //    }
                //}   
                //else
                //{
                //    //styleHeaders = new List<int>();
                //    styleHeaders.Add(hi.HeaderRID);
                //    _headersByStyle.Add(hi.StyleHnRID, styleHeaders);
                //}
                //else
                //{   // Begin TT#2 - Ron Matelic - Assortment Planning: add if...
                //    //styleHeaders.Add(hi.HeaderRID);
                //    if (!styleHeaders.Contains(hi.HeaderRID))
                //    {
                //        styleHeaders.Add(hi.HeaderRID);
                //    }
                //}   // End TT#2
				// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
            }
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
        }

        // Begin Track #5841 - JSmith - Performance
        static private void BuildComponents(HeaderInfo aHeaderInfo)
        {
            try
            {
                header_rwl.AcquireWriterLock(WriterLockTimeOut);
                try
                {
                    BuildHeaderPacks(aHeaderInfo.HeaderRID);
                    BuildHeaderPackColors(aHeaderInfo.HeaderRID);
                    BuildHeaderPackColorSizes(aHeaderInfo.HeaderRID);
                    BuildHeaderBulkColors(aHeaderInfo.HeaderRID);
                    BuildHeaderBulkColorSizes(aHeaderInfo.HeaderRID);
                    aHeaderInfo.AreComponentsLoaded = true;
                }
                catch (Exception ex)
                {
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
                    throw;
                }
                finally
                {
                    // Ensure that the lock is released.
                    header_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
                // The writer lock request timed out.
                EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:BuildComponents writer lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:BuildComponents writer lock has timed out");
            }

        }
        // End Track #5841

		/// <summary>
		/// Rebuilds the headers from the database.  
		/// </summary>
        static public void RebuildHeaderCharacteristicData(int headerFilterRID, FilterHeaderOptions headerFilterOptions) 
		{
			try
			{
				headerChar_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					BuildHeaderCharacteristicData();			
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					headerChar_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:ReloadHeaders writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHeaderService:ReloadHeaders writer lock has timed out");
			}

			try
			{
				header_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
                    BuildHeaderDataForWorkspace(headerFilterRID, headerFilterOptions);	//TT#1313-MD -jsobek -Header Filters		
				}
				catch ( Exception ex )
				{
                    // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                    if (Audit != null)
                    {
                        Audit.Log_Exception(ex);
                    }
                    // End TT#189
					throw;
				}
				finally
				{
					// Ensure that the lock is released.
					header_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The writer lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:ReloadHeaders writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDHeaderService:ReloadHeaders writer lock has timed out");
			}

		}

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -function no longer needed

        ///// <summary>
        ///// Reads the header groups from the database and builds HeaderGroupInfo objects for each header group.  
        ///// </summary>
        ///// <remarks>
        ///// Also constructs two cross-references.  
        ///// One from Header record id to a HeaderGroupInfo object and the other from the Header Group ID 
        ///// to the Header Group record id. 
        ///// </remarks>
        //static private void BuildHeaderGroups()
        //{
        //    HeaderGroupInfo hgi;

        //    try
        //    {	// MultiHeader modification
        //        DataTable dt = _headerData.GetHeaderGroups();
        //        int groupRID, prevGroupRID = 0, hdrRID = 0;
        //        hgi = new HeaderGroupInfo();
				
        //        foreach(DataRow dr in dt.Rows)
        //        {
        //            groupRID = Convert.ToInt32(dr["HDR_GROUP_RID"], CultureInfo.CurrentUICulture);
        //            hdrRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
        //            if (groupRID != prevGroupRID)
        //            {
        //                if (hgi.HeaderGroupRID > 1)
        //                {
        //                    //_headerGroupsByRID.Add(hgi.HeaderGroupRID, hgi); //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -hashtable not used
        //                    hgi = new HeaderGroupInfo();	
        //                }
        //                hgi.HeaderGroupRID	= groupRID;
        //                prevGroupRID = groupRID;  
        //            }	
        //            if (!hgi.Headers.Contains(hdrRID))
        //                hgi.Headers.Add(hdrRID);
					
        //            //hgi.HeaderGroupDescription	= Convert.ToString(dr["HDR_GROUP_DESC"], CultureInfo.CurrentUICulture);
        //            //_headerGroupsByRID.Add(hgi.HeaderGroupRID, hgi);
        //            //_headerGroupsByID.Add(hgi.HeaderGroupID, hgi.HeaderGroupRID);
        //        }
        //        // Begin TT#2 - Ron Matelic - Assortmen Planning
        //        //if (hgi.HeaderGroupRID > 1)
        //        //    _headerGroupsByRID.Add(hgi.HeaderGroupRID, hgi);


        //        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -hashtable not used
        //        //if (hgi.HeaderGroupRID > 1 && !_headerGroupsByRID.ContainsKey(hgi.HeaderGroupRID))
        //        //{
        //        //    _headerGroupsByRID.Add(hgi.HeaderGroupRID, hgi);
        //        //}
        //        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -hashtable not used
        //    }   // End TT#2
        //    catch ( Exception ex )
        //    {
        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // End TT#189
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -function no longer needed






        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
   

      

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        /// <summary>
        /// Reads the headers from the database that have not been released and builds HeaderInfo objects for each header.  
        /// </summary>
        /// <remarks>
        /// Also constructs two cross-references.  
        /// One from header record id to a HeaderInfo object and the other from the header ID to the header record id. 
        /// </remarks>
        //static private void BuildNonReleasedHeaders()
        //{
        //    bool masterHeadersExist = false;
        //    Hashtable HashMasterHeader = null;
        //    Hashtable HashSubordinateHeader = null;
        //    // Begin TT#827 - JSmith - Allocation Performance
        //    //DataTable DtMasterSubordinate = null;
        //    //int masterRID;
        //    //int subordinateRID;
        //    // End TT#827 - JSmith - Allocation Performance

        //    try
        //    {
        //        // Begin TT#827 - JSmith - Allocation Performance
        //        //// For performance, load master and subordinates to Hashtables so only headers that are part
        //        //// of header-subordinate relationship are resolved later
        //        //DtMasterSubordinate = _headerData.GetMasterSubordinates();
        //        //if (DtMasterSubordinate.Rows.Count > 0)
        //        //{
        //        //    masterHeadersExist = true;
        //        //    HashMasterHeader = new Hashtable();
        //        //    HashSubordinateHeader = new Hashtable();
        //        //    foreach (DataRow dr in DtMasterSubordinate.Rows)
        //        //    {
        //        //        masterRID = Convert.ToInt32(dr["MASTER_HDR_RID"], CultureInfo.CurrentUICulture);
        //        //        subordinateRID = Convert.ToInt32(dr["SUBORD_HDR_RID"], CultureInfo.CurrentUICulture);
        //        //        HashMasterHeader.Add(masterRID, null);
        //        //        HashSubordinateHeader.Add(subordinateRID, null);
        //        //    }
        //        //}
        //        masterHeadersExist = BuildMasterHeaders(HashMasterHeader, HashSubordinateHeader);
        //        // End TT#827 - JSmith - Allocation Performance

        //        DataTable dt = null;
        //        dt = _headerData.GetNonReleasedHeaders();

        //        LoadHeaders(dt, masterHeadersExist);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

 
        // End TT#1065

    

		// Begin TT#931 - MD - When deleting a Group Allocation there is no "Are you sure?" message
        /// <summary>
        /// Adjusts header array lists after an Assortment (Group Allocation) header is removed from the workspace.
        /// </summary>
        /// <param name="aHeaderRID"></param>
        static public void DeleteAssortmentHeader(int aHeaderRID, string aHeaderID)
        {
            headerChar_rwl.AcquireWriterLock(WriterLockTimeOut);

            try
            {
                if (_headersByIDAssortment.ContainsKey(aHeaderID))	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                {
                    _headersByIDAssortment.Remove(aHeaderID);	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                }
                // Begin TT#1970-MD - JSmith - Receive Unhandled Exception when re-creating a Group Allocation
                if (_headersByIDAllocation.ContainsKey(aHeaderID))	
                {
                    _headersByIDAllocation.Remove(aHeaderID);
                }
                // End TT#1970-MD - JSmith - Receive Unhandled Exception when re-creating a Group Allocation
                //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -no reason to cache assortment RIDS
                //if (_assortmentRIDs.Contains(aHeaderRID))
                //{
                //    _assortmentRIDs.Remove(aHeaderRID);
                //}
                //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -no reason to cache assortment RIDS
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                headerChar_rwl.ReleaseWriterLock();
            }

        }
		// End TT#931 - MD - When deleting a Group Allocation there is no "Are you sure?" message

		// Begin TT#936 - MD - Prevent the saving of empty Group Allocations
        static public void DeleteEmptyGroupAllocationHeader(int aHeaderRID)
        {
            headerChar_rwl.AcquireWriterLock(WriterLockTimeOut);

            try
            {
                try
                {
                    string enqMessage = string.Empty;
					// BEGIN TT#953 - MD - create GA same as one previously deleted and get unhandled exception -
                    AllocationHeaderProfile ahp = GetAllocationHeaderProfile(aHeaderRID, false, false, true);
                    string aHeaderID = ahp.HeaderID;
                    // Delete GA
                    _headerData.OpenUpdateConnection();
                    _headerData.DeleteGroupAllocation(aHeaderRID);
                    _headerData.CommitData();

                    // Removes assortment/GA header from various lists
                    DeleteAssortmentHeader(aHeaderRID, aHeaderID);
					// END TT#953 - MD - create GA same as one previously deleted and get unhandled exception -
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (_headerData.ConnectionIsOpen)
                    {
                        _headerData.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                headerChar_rwl.ReleaseWriterLock();
            }

        }

        static public void DeleteEmptyGroupAllocationHeaders(List<int> asrtKeyList, SessionAddressBlock aSAB)
        {
            headerChar_rwl.AcquireWriterLock(WriterLockTimeOut);
            ApplicationSessionTransaction newTrans = new ApplicationSessionTransaction(aSAB);

            try
            {
                foreach (int asrtRid in asrtKeyList)
                {
                    bool hasHeaders = false;
                    AllocationHeaderProfile ashp = GetAllocationHeaderProfile(asrtRid, false, false, true);
                    if (ashp.AsrtType == (int)eAssortmentType.GroupAllocation)
                    {
                        ArrayList hdrList = GetHeadersInAssortment(ashp.Key);
                        foreach (int hdrRid in hdrList)
                        {
                            AllocationHeaderProfile ahp = GetAllocationHeaderProfile(hdrRid, false, false, true);
                            if (ahp.HeaderType != eHeaderType.Placeholder && ahp.HeaderType != eHeaderType.Assortment)
                            {
                                hasHeaders = true;
                                break;
                            }
                        }
                    }

                    if (!hasHeaders)
                    {
                        try
                        {
                            string errMsg;
                            if (EnqueueHeader(newTrans, asrtRid, out errMsg))
                            {
                                DeleteEmptyGroupAllocationHeader(asrtRid);
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            DequeueHeaders(newTrans);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                throw;
            }
            finally
            {
                // Ensure that the lock is released.
                headerChar_rwl.ReleaseWriterLock();
            }

        }

        static private bool EnqueueHeader(ApplicationSessionTransaction aTrans, int aHdrRid, out string aErrorMsg)
        {
            List<int> hdrRidList = new List<int>();
            hdrRidList.Add(aHdrRid);
            return EnqueueHeader(aTrans, hdrRidList, out aErrorMsg);
        }

        static private bool EnqueueHeader(ApplicationSessionTransaction aTrans, List<int> aHdrRidList, out string aErrorMsg)
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
            }
            return enqueueStatus;
        }

        static private void DequeueHeaders(ApplicationSessionTransaction aTrans)
        {
            aTrans.DequeueHeaders();
        }
		// End TT#936 - MD - Prevent the saving of empty Group Allocations

		/// <summary>
		/// Reads the header Packs from the database and builds HeaderPackInfo objects for each pack.  
		/// </summary>
		/// <remarks>
		/// It also adds the record ID of the pack to an ArrayList in the header
		/// </remarks>
        // Begin Track #5841 - JSmith - Performance
        //static private void BuildHeaderPacks()
        static private void BuildHeaderPacks(int aHeaderRID)
        // End Track #5841
		{
			HeaderInfo hi = null;
			HeaderPackInfo hpi = null;
			int currentHeaderRID = Include.NoRID;

			try
			{
				DataTable dt = null;
                // Begin Track #5841 - JSmith - Performance
                //dt = _headerData.GetPacks();
                dt = _headerData.GetPacks(aHeaderRID);
                // End Track #5841

                // Begin TT#1966-MD - JSmith - DC Fulfillment
                DataTable dtPackAssociation = _headerData.GetAssociatedPacks(aHeaderRID);
                // End TT#1966-MD - JSmith - DC Fulfillment

				foreach(DataRow dr in dt.Rows)
				{
					hpi = new HeaderPackInfo();	
			
					hpi.HeaderPackRID = Convert.ToInt32(dr["HDR_PACK_RID"], CultureInfo.CurrentUICulture);
					hpi.HeaderRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
					hpi.HeaderPackName = Convert.ToString(dr["HDR_PACK_NAME"], CultureInfo.CurrentUICulture);
					hpi.Packs = Convert.ToInt32(dr["PACKS"], CultureInfo.CurrentUICulture);
					hpi.Multiple = Convert.ToInt32(dr["MULTIPLE"], CultureInfo.CurrentUICulture);
					hpi.ReservePacks = Convert.ToInt32(dr["RESERVE_PACKS"], CultureInfo.CurrentUICulture);
					if (!dr.IsNull("GENERIC_IND"))
					{
						hpi.GenericInd = Include.ConvertCharToBool(Convert.ToChar(dr["GENERIC_IND"], CultureInfo.CurrentUICulture));
					}
					// MultiHeader
					if (!dr.IsNull("ASSOCIATED_PACK_RID"))
					{
						hpi.AssociatedPackRID = Convert.ToInt32(dr["ASSOCIATED_PACK_RID"], CultureInfo.CurrentUICulture);
					}
                    // Assortment
                    if (!dr.IsNull("SEQ"))
                    {
                        hpi.Sequence = Convert.ToInt32(dr["SEQ"], CultureInfo.CurrentUICulture);
                    }
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    foreach (DataRow drPackAssociation in dtPackAssociation.Select("HDR_PACK_RID=" + hpi.HeaderPackRID))
                    {
                        hpi.AssociatedPackRIDs.Add(Convert.ToInt32(drPackAssociation["ASSOCIATED_PACK_RID"], CultureInfo.CurrentUICulture));
                        hpi.AssociatedPackName = Convert.ToString(drPackAssociation["HDR_PACK_NAME"], CultureInfo.CurrentUICulture);  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment

                    // Begin TT#2 - RMatelic - Assortment Planning
                    if (_packsByRID.ContainsKey(hpi.HeaderPackRID))
                    {
                        _packsByRID.Remove(hpi.HeaderPackRID);
                    }
                    // End TT#2
                    
					_packsByRID.Add(hpi.HeaderPackRID, hpi);


					// add pack to header
					if (hpi.HeaderRID != currentHeaderRID)
					{
						hi = (HeaderInfo)_headersByRIDAllocation[hpi.HeaderRID];	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
						currentHeaderRID = hpi.HeaderRID;
					}

					if (hi != null)
					{
						hi.Packs.Add(hpi.HeaderPackRID);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the header pack colors from the database and builds HeaderPackColorInfo objects for each pack/color.  
		/// </summary>
        // Begin Track #5841 - JSmith - Performance
        //static private void BuildHeaderPackColors()
        static private void BuildHeaderPackColors(int aHeaderRID)
        // End Track #5841
		{
			HeaderPackInfo hpi = null;
			HeaderPackColorInfo hpci = null;
			int headerPackRID = Include.NoRID;
			int currentHeaderPackRID = Include.NoRID;

			try
			{
				DataTable dt = null;
                // Begin Track #5841 - JSmith - Performance
                //dt = _headerData.GetPackColors();
                dt = _headerData.GetPackColorsForHeader(aHeaderRID);
                // End Track #5841

				foreach(DataRow dr in dt.Rows)
				{
					hpci = new HeaderPackColorInfo();	
			
					headerPackRID = Convert.ToInt32(dr["HDR_PACK_RID"], CultureInfo.CurrentUICulture);
                    hpci.ColorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
					hpci.Units = Convert.ToInt32(dr["UNITS"], CultureInfo.CurrentUICulture);
					hpci.Sequence = Convert.ToInt32(dr["SEQ"], CultureInfo.CurrentUICulture);
                 
                    // Assortment BEGIN
                    hpci.HdrPCRID = Convert.ToInt32(dr["HDR_PC_RID"], CultureInfo.CurrentUICulture);
                    hpci.ColorName = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                    hpci.ColorDescription = Convert.ToString(dr["DESCRIPTION"], CultureInfo.CurrentUICulture);
                    hpci.Last_PCSZ_Key_Used = Convert.ToInt32(dr["LAST_PCSZ_KEY_USED"], CultureInfo.CurrentUICulture);
                    // Assortment END
					
					// add color to pack
					if (headerPackRID != currentHeaderPackRID)
					{
						hpi = (HeaderPackInfo)_packsByRID[headerPackRID];
						currentHeaderPackRID = headerPackRID;
					}

					if (hpi != null)
					{
						hpi.Colors.Add(hpci.ColorCodeRID, hpci);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the header pack colors from the database and builds HeaderPackColorInfo objects for each pack/color.  
		/// </summary>
        // Begin Track #5841 - JSmith - Performance
        //static private void BuildHeaderPackColorSizes()
        static private void BuildHeaderPackColorSizes(int aHeaderRID)
        // End Track #5841
		{
			HeaderPackInfo hpi = null;
			HeaderPackColorInfo hpci = null;
			HeaderPackColorSizeInfo hpcsi = null;
			int headerPackRID = Include.NoRID;
			int currentHeaderPackRID = Include.NoRID;
			int colorCodeRID = Include.NoRID;
			int currentColorCodeRID = Include.NoRID;

			try
			{
				DataTable dt = null;
                // Begin Track #5841 - JSmith - Performance
                //dt = _headerData.GetPackColorSizes();
                dt = _headerData.GetPackColorSizesForHeader(aHeaderRID);
                // End Track #5841

				foreach(DataRow dr in dt.Rows)
				{
					hpcsi = new HeaderPackColorSizeInfo();	
			        
					headerPackRID = Convert.ToInt32(dr["HDR_PACK_RID"], CultureInfo.CurrentUICulture);
					colorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                    hpcsi.HDR_PCSZ_Key = Convert.ToInt32(dr["HDR_PCSZ_KEY"], CultureInfo.CurrentUICulture); // Assortment: Color/size change
					hpcsi.SizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
					hpcsi.Units = Convert.ToInt32(dr["UNITS"], CultureInfo.CurrentUICulture);
					hpcsi.Sequence = Convert.ToInt32(dr["SEQ"], CultureInfo.CurrentUICulture);
					
					
					// get correct pack
					if (headerPackRID != currentHeaderPackRID)
					{
						hpi = (HeaderPackInfo)_packsByRID[headerPackRID];
						currentHeaderPackRID = headerPackRID;
						// reset color code RID since new pack
						currentColorCodeRID = Include.NoRID;
					}
					// get correct color
					if (colorCodeRID != currentColorCodeRID)
					{
						if (hpi != null)
						{
							hpci = (HeaderPackColorInfo)hpi.Colors[colorCodeRID];
							currentColorCodeRID = colorCodeRID;
						}
					}

					if (hpi != null && hpci != null)
					{
						hpci.Sizes.Add(hpcsi.SizeCodeRID, hpcsi);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the header bulk colors from the database and builds HeaderBulkColorInfo objects for each bulk color.  
		/// </summary>
        // Begin Track #5841 - JSmith - Performance
        //static private void BuildHeaderBulkColors()
        static private void BuildHeaderBulkColors(int aHeaderRID)
        // End Track #5841
		{
			HeaderInfo hi = null;
			HeaderBulkColorInfo hbci = null;
			int headerRID = Include.NoRID;
			int currentHeaderRID = Include.NoRID;

			try
			{
				DataTable dt = null;
                // Begin Track #5841 - JSmith - Performance
                //dt = _headerData.GetBulkColors();
                dt = _headerData.GetBulkColors(aHeaderRID);
                // End Track #5841

				foreach(DataRow dr in dt.Rows)
				{
					hbci = new HeaderBulkColorInfo();	
			
					headerRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
                    hbci.ColorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
					hbci.Units = Convert.ToInt32(dr["UNITS"], CultureInfo.CurrentUICulture);
					hbci.Multiple = Convert.ToInt32(dr["MULTIPLE"], CultureInfo.CurrentUICulture);
					hbci.Minimum = Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture);
					hbci.Maximum = Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture);
					hbci.ReserveUnits = Convert.ToInt32(dr["RESERVE_UNITS"], CultureInfo.CurrentUICulture);
					hbci.Sequence = Convert.ToInt32(dr["SEQ"], CultureInfo.CurrentUICulture);
                    // Assortment BEGIN
                    hbci.HdrBCRID = Convert.ToInt32(dr["HDR_BC_RID"], CultureInfo.CurrentUICulture);
                    hbci.Last_BCSZ_Key_Used = Convert.ToInt32(dr["LAST_BCSZ_KEY_USED"], CultureInfo.CurrentUICulture); 
                    hbci.Name = Convert.ToString(dr["NAME"], CultureInfo.CurrentUICulture);
                    hbci.Description = Convert.ToString(dr["DESCRIPTION"], CultureInfo.CurrentUICulture);
                    if (!Convert.IsDBNull(dr["ASRT_BC_RID"]))
                    {
                        hbci.AsrtBCRID = Convert.ToInt32(dr["ASRT_BC_RID"], CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        hbci.AsrtBCRID = Include.NoRID;
                    }
                    // Assortment END

                    // begin TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
                    ColorStatusFlags colorStatusFlags = new ColorStatusFlags();
                    if (Convert.IsDBNull(dr["COLOR_STATUS_FLAGS"]))
                    {
                        colorStatusFlags.AllFlags = 0;
                    }
                    else
                    {
                        colorStatusFlags.AllFlags = Convert.ToUInt32(dr["COLOR_STATUS_FLAGS"], CultureInfo.CurrentUICulture);
                    }                   
                    hbci.ColorStatusFlags = colorStatusFlags;
                    // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5

					// add color to header
					if (headerRID != currentHeaderRID)
					{
						hi = (HeaderInfo)_headersByRIDAllocation[headerRID];	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
						currentHeaderRID = headerRID;
					}

					if (hi != null)
					{
						hi.BulkColors.Add(hbci.ColorCodeRID, hbci);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the header pack colors from the database and builds HeaderPackColorInfo objects for each pack/color.  
		/// </summary>
        // Begin Track #5841 - JSmith - Performance
        //static private void BuildHeaderBulkColorSizes()
        static private void BuildHeaderBulkColorSizes(int aHeaderRID)
        // End Track #5841
		{
			HeaderInfo hi = null;
			HeaderBulkColorInfo hbci = null;
			HeaderBulkColorSizeInfo hbcsi = null;
			int headerRID = Include.NoRID;
			int currentHeaderRID = Include.NoRID;
			int colorCodeRID = Include.NoRID;
			int currentColorCodeRID = Include.NoRID;

			try
			{
				DataTable dt = null;
                // Begin Track #5841 - JSmith - Performance
                //dt = _headerData.GetBulkColorSizes();
                dt = _headerData.GetBulkColorSizesForHeader(aHeaderRID);
                // End Track #5841

				foreach(DataRow dr in dt.Rows)
				{
					hbcsi = new HeaderBulkColorSizeInfo();	
			
					headerRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
					colorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
                    hbcsi.HDR_BCSZ_KEY = Convert.ToInt32(dr["HDR_BCSZ_KEY"], CultureInfo.CurrentUICulture);  // Assortment: Color/size change
					hbcsi.SizeCodeRID = Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
					hbcsi.Units = Convert.ToInt32(dr["UNITS"], CultureInfo.CurrentUICulture);
					hbcsi.Multiple = Convert.ToInt32(dr["MULTIPLE"], CultureInfo.CurrentUICulture);
					hbcsi.Minimum = Convert.ToInt32(dr["MINIMUM"], CultureInfo.CurrentUICulture);
					hbcsi.Maximum = Convert.ToInt32(dr["MAXIMUM"], CultureInfo.CurrentUICulture);
					hbcsi.ReserveUnits = Convert.ToInt32(dr["RESERVE_UNITS"], CultureInfo.CurrentUICulture);
					hbcsi.Sequence = Convert.ToInt32(dr["SEQ"], CultureInfo.CurrentUICulture);
					
					// get correct header
					if (headerRID != currentHeaderRID)
					{
						hi = (HeaderInfo)_headersByRIDAllocation[headerRID];	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
						if (hi != null)
						{
							currentHeaderRID = headerRID;
							// get correct collor
							hbci = (HeaderBulkColorInfo)hi.BulkColors[colorCodeRID];
							currentColorCodeRID = colorCodeRID;
						}
					}
					// get current color
					else if (colorCodeRID != currentColorCodeRID)
					{
						if (hi != null)
						{
							hbci = (HeaderBulkColorInfo)hi.BulkColors[colorCodeRID];
							currentColorCodeRID = colorCodeRID;
						}
					}

					if (hi != null && hbci != null)
					{
						hbci.Sizes.Add(hbcsi.SizeCodeRID, hbcsi);
					}
				}
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Reads the header characteristics from the database and builds objects for each characteristic.  
		/// </summary>
		/// </param>
		static private void BuildHeaderCharacteristicData() 
		{

			try
			{
				_headerCharGroupsByRID.Clear();
				_headerCharGroupsByID.Clear();
				_headerCharsByRID.Clear();

				//BuildHeaderCharGroups();
                // Reads the header characteristic groups from the database and builds HeaderCharGroupInfo objects for each header group.   
                // Also constructs two cross-references.  
                // One from Header char group record id to a HeaderCharGroupInfo object and the other from the Header Char Group ID 
                //to the Header Char Group record id. 
                HeaderCharGroupInfo hcgi = null;

          
                    DataTable dt = _headerCharacteristicsData.HeaderCharGroup_Read();

                    foreach (DataRow dr in dt.Rows)
                    {
                        hcgi = new HeaderCharGroupInfo();
                        hcgi.RID = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
                        hcgi.ID = Convert.ToString(dr["HCG_ID"], CultureInfo.CurrentUICulture);
                        hcgi.Type = (eHeaderCharType)(Convert.ToInt32(dr["HCG_TYPE"], CultureInfo.CurrentUICulture));
                        hcgi.ListInd = Include.ConvertCharToBool(Convert.ToChar(dr["HCG_LIST_IND"], CultureInfo.CurrentUICulture));
                        hcgi.ProtectInd = Include.ConvertCharToBool(Convert.ToChar(dr["HCG_PROTECT_IND"], CultureInfo.CurrentUICulture));
                        _headerCharGroupsByRID.Add(hcgi.RID, hcgi);
                        _headerCharGroupsByID.Add(hcgi.ID, hcgi.RID);
                    }
      



				//BuildHeaderChars(aCharsForListGroupsOnly);
                hcgi = null;
                    //HeaderCharInfo hci = null;
                    int currentCharHeaderGroupRID = Include.NoRID;
                    dt = null;
                    //if (aCharsForListGroupsOnly)
                    //{
                        dt = _headerCharacteristicsData.HeaderCharList_Read();
                    //}
                    //else
                    //{
                    //    dt = _headerCharacteristicsData.HeaderChar_Read();
                    //}

                    foreach (DataRow dr in dt.Rows)
                    {
                        HeaderCharInfo hci = new HeaderCharInfo();

                        hci.RID = Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture);
                        hci.GroupRID = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
                        if (!Convert.IsDBNull(dr["TEXT_VALUE"]))
                        {
                            hci.TextValue = Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(dr["DATE_VALUE"]))
                        {
                            hci.DateValue = Convert.ToDateTime(dr["DATE_VALUE"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(dr["NUMBER_VALUE"]))
                        {
                            hci.NumberValue = Convert.ToDouble(dr["NUMBER_VALUE"], CultureInfo.CurrentUICulture);
                        }
                        if (!Convert.IsDBNull(dr["DOLLAR_VALUE"]))
                        {
                            hci.DollarValue = Convert.ToDouble(dr["DOLLAR_VALUE"], CultureInfo.CurrentUICulture);
                        }

                        _headerCharsByRID.Add(hci.RID, hci);

                        // add characteristic to header characteristic group
                        if (hci.GroupRID != currentCharHeaderGroupRID)
                        {
                            hcgi = (HeaderCharGroupInfo)_headerCharGroupsByRID[hci.GroupRID];
                            currentCharHeaderGroupRID = hci.GroupRID;
                        }
                        hcgi.Characteristics.Add(hci.RID, hci);
                    }

			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		

		/// <summary>
		/// Reads the header characteristics from the database and builds HeaderCharInfo objects for each characteristics.  
		/// </summary>
		/// <remarks>
		/// Also constructs a cross-reference from characteristic record id to a HeaderCharInfo object.
		/// </remarks>
        //static private void BuildHeaderChars(bool aCharsForListGroupsOnly)
        //{
        //    HeaderCharGroupInfo hcgi = null;
        //    HeaderCharInfo hci = null;
        //    int currentCharHeaderGroupRID = Include.NoRID;

        //    try
        //    {
        //        DataTable dt = null;
        //        if (aCharsForListGroupsOnly)
        //        {
        //            dt = _headerCharacteristicsData.HeaderCharList_Read();
        //        }
        //        else
        //        {
        //            dt = _headerCharacteristicsData.HeaderChar_Read();
        //        }

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            hci = new HeaderCharInfo();

        //            hci.RID = Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture);
        //            hci.GroupRID = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
        //            if (!Convert.IsDBNull(dr["TEXT_VALUE"]))
        //            {
        //                hci.TextValue = Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);
        //            }
        //            if (!Convert.IsDBNull(dr["DATE_VALUE"]))
        //            {
        //                hci.DateValue = Convert.ToDateTime(dr["DATE_VALUE"], CultureInfo.CurrentUICulture);
        //            }
        //            if (!Convert.IsDBNull(dr["NUMBER_VALUE"]))
        //            {
        //                hci.NumberValue = Convert.ToDouble(dr["NUMBER_VALUE"], CultureInfo.CurrentUICulture);
        //            }
        //            if (!Convert.IsDBNull(dr["DOLLAR_VALUE"]))
        //            {
        //                hci.DollarValue = Convert.ToDouble(dr["DOLLAR_VALUE"], CultureInfo.CurrentUICulture);
        //            }

        //            _headerCharsByRID.Add(hci.RID, hci);

        //            // add characteristic to header characteristic group
        //            if (hci.GroupRID != currentCharHeaderGroupRID)
        //            {
        //                hcgi = (HeaderCharGroupInfo)_headerCharGroupsByRID[hci.GroupRID];
        //                currentCharHeaderGroupRID = hci.GroupRID;
        //            }
        //            hcgi.Characteristics.Add(hci.RID, hci);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // End TT#189
        //        throw;
        //    }
        //}

		/// <summary>
		/// Reads the header characteristic records from the database and associates the characteristic with the header
		/// </summary>
        static private void BuildHeaderCharJoins(int aHeaderRID, DataTable dtProvidedCharValuesForHeaderList)
		{
			try
			{
                HeaderInfo hi = GetHeaderInfoFromCrossReference(aHeaderRID);
				if (hi != null)
				{
					hi.Characteristics.Clear();

                    //Begin TT#1313-MD -jsobek -Header Filters 
                    DataRow[] drCharValuesForHeader;
                    if (dtProvidedCharValuesForHeaderList != null)
                    {
                        drCharValuesForHeader = dtProvidedCharValuesForHeaderList.Select("HDR_RID=" + aHeaderRID); //load header characteristics in bulk for better performance
                    }
                    else
                    {
                        DataTable dt = _headerCharacteristicsData.HeaderJoin_Read(aHeaderRID);
                        drCharValuesForHeader = dt.Select();
                    }
                    //End TT#1313-MD -jsobek -Header Filters

                    foreach (DataRow dr in drCharValuesForHeader) //TT#1313-MD -jsobek -Header Filters
					{
						int headerRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
						int charRID = Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture);

						// add characteristic to header 
						hi.Characteristics.Add(charRID);

						if (!_headerCharsByRID.ContainsKey(charRID))
						{
                            HeaderCharInfo hci = new HeaderCharInfo();

							hci.RID = charRID;
							hci.GroupRID = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture);
							if (!Convert.IsDBNull(dr["TEXT_VALUE"]))
							{
								hci.TextValue = Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);
							}
							if (!Convert.IsDBNull(dr["DATE_VALUE"]))
							{
								hci.DateValue = Convert.ToDateTime(dr["DATE_VALUE"], CultureInfo.CurrentUICulture);
							}
							if (!Convert.IsDBNull(dr["NUMBER_VALUE"]))
							{
								hci.NumberValue = Convert.ToDouble(dr["NUMBER_VALUE"], CultureInfo.CurrentUICulture);
							}
							if (!Convert.IsDBNull(dr["DOLLAR_VALUE"]))
							{
								hci.DollarValue = Convert.ToDouble(dr["DOLLAR_VALUE"], CultureInfo.CurrentUICulture);
							}

							_headerCharsByRID.Add(hci.RID, hci);
						}
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

//        /// <summary>
//        /// Requests the session get all information associated with headers for which a user is authorized 
//        /// to reference.
//        /// </summary>
//        /// <param name="userRID">The record id of the user</param>
//        /// <param name="aSAB">The session address block to be used to communicate with other sessions</param>
//        /// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
//        /// <param name="aIncludeCharacteristics">A flag identifying if characteristics are to be included</param>
//        /// <returns>An ArrayList of AllocationHeaderProfiles</returns>
//        // BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
//        public static ArrayList GetHeadersForWorkspace(int headerFilterRID, FilterHeaderOptions headerFilterOptions, SessionAddressBlock aSAB) //Begin TT#1313-MD -jsobek -Header Filters
//        // END MID Track #4357
//        {
//            try
//            {
//                bool aGetInterfacedHeaders = true;
//                bool aGetNonInterfacedHeaders = true;
//                bool aIncludeComponents = false;
//                bool aIncludeCharacteristics = true;

//                bool aGetAssortmentHeaders = false;
//                if (headerFilterOptions.filterType == filterTypes.AssortmentFilter)
//                {
//                    aGetAssortmentHeaders = true;
//                }


//                BuildHeaderData(headerFilterRID, headerFilterOptions, aGetAssortmentHeaders); 
//                header_rwl.AcquireReaderLock(ReaderLockTimeOut);
//                try
//                {

//                    //AllocationWorkspaceFilterProfile awfp = new AllocationWorkspaceFilterProfile(userRID);
//                    ArrayList headers = new ArrayList();
//                    //ProfileList hpl = new ProfileList();
				 
//                    // get a list of unique style node RIDs to determine security
//                    ArrayList styles = new ArrayList();
//                    // filter styles 
//                    // no longer necessary since stored procedure filters the styles
////                    if (awfp.HnRID > Include.NoRID)
////                    {
//////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////                        NodeDescendantList ndl = aSAB.HierarchyServerSession.GetNodeDescendantList(awfp.HnRID, eHierarchyLevelType.Style, eNodeSelectType.NoVirtual);
//////						NodeDescendantList ndl = aSAB.HierarchyServerSession.GetNodeDescendantList(awfp.HnRID, eHierarchyLevelType.Style);
//////End Track #4037
////                        foreach (int styleRID in _headersByStyle.Keys)
////                        {
////                            if (!styles.Contains(styleRID))
////                            {
////                                if (ndl.Contains(styleRID))
////                                {
////                                    styles.Add(styleRID);
////                                }
////                            }
////                        }
////                    }
////                    else
////                    {
//                        foreach (int styleRID in _headersByStyle.Keys)
//                        {
//                            if (!styles.Contains(styleRID))
//                            {
//                                styles.Add(styleRID);
//                            }
//                        }
//                    //}

//                    // no longer necessary since stored procedure filters the dates
//                    //DateTime headerDateBetweenFrom = DateTime.MinValue;
//                    //DateTime headerDateBetweenTo = DateTime.MinValue;
//                    //DateTime releaseDateBetweenFrom = DateTime.MinValue;
//                    //DateTime releaseDateBetweenTo = DateTime.MinValue;
//                        bool selectHeader = true;
//                    //if (awfp.HeaderDateType == eFilterDateType.between)
//                    //{
//                    //    headerDateBetweenFrom = DateTime.Now.Add(new TimeSpan(awfp.HeaderDateBetweenFrom,0,0,0,0));
//                    //    headerDateBetweenTo = DateTime.Now.Add(new TimeSpan(awfp.HeaderDateBetweenTo,0,0,0,0));
//                    //}
//                    //if (awfp.ReleaseDateType == eFilterDateType.between)
//                    //{
//                    //    releaseDateBetweenFrom = DateTime.Now.Add(new TimeSpan(awfp.ReleaseDateBetweenFrom,0,0,0,0));
//                    //    releaseDateBetweenTo = DateTime.Now.Add(new TimeSpan(awfp.ReleaseDateBetweenTo,0,0,0,0));
//                    //}
												
//                    // call client session to get security for each style
////					ArrayList authorizedStyles = aSAB.ClientServerSession.GetMyUserNodes(styles, (int)eSecurityTypes.Allocation);

//                    //ArrayList hdrRIDs = new ArrayList(); // MultiHeader //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -variable not used
				 
//                    ArrayList authorizedStyles = aSAB.HierarchyServerSession.GetMyAllocationStyles(styles);
//                    foreach (int styleRID in authorizedStyles) 
//                    {
//                        // get headers for style from cross reference
//                        ArrayList styleHeaders = (ArrayList)_headersByStyle[styleRID];
//                        if (styleHeaders != null)
//                        {
//                            foreach (int headerRID in styleHeaders)
//                            {
//                                selectHeader = true;
//                                AllocationHeaderProfile ahp = GetHeaderData(headerRID, aIncludeComponents, aIncludeCharacteristics);

//                                //// check header date
//                                //switch (awfp.HeaderDateType)
//                                //{
//                                //    case eFilterDateType.today:
//                                //        if (ahp.HeaderDay.Date != DateTime.Now.Date)
//                                //        {
//                                //            selectHeader = false;
//                                //        }
//                                //        break;
//                                //    case eFilterDateType.between:
//                                //        if (ahp.HeaderDay.Date < headerDateBetweenFrom.Date ||
//                                //            ahp.HeaderDay.Date > headerDateBetweenTo.Date)
//                                //        {
//                                //            selectHeader = false;
//                                //        }
//                                //        break;
//                                //    case eFilterDateType.specify:
//                                //        if (ahp.HeaderDay.Date < awfp.HeaderDateFrom.Date ||
//                                //            ahp.HeaderDay.Date > awfp.HeaderDateTo.Date)
//                                //        {
//                                //            selectHeader = false;
//                                //        }
//                                //        break;
//                                //}

//                                //// check release date
//                                //if (selectHeader &&
//                                //    ahp.ReleaseDate != DateTime.MinValue)
//                                //{
//                                //    switch (awfp.ReleaseDateType)
//                                //    {
//                                //        case eFilterDateType.today:
//                                //            if (ahp.ReleaseDate.Date != DateTime.Now.Date)
//                                //            {
//                                //                selectHeader = false;
//                                //            }
//                                //            break;
//                                //        case eFilterDateType.between:
//                                //            if (ahp.ReleaseDate.Date < releaseDateBetweenFrom.Date ||
//                                //                ahp.ReleaseDate.Date > releaseDateBetweenTo.Date)
//                                //            {
//                                //                selectHeader = false;
//                                //            }
//                                //            break;
//                                //        case eFilterDateType.specify:
//                                //            if (ahp.ReleaseDate.Date < awfp.ReleaseDateFrom.Date ||
//                                //                ahp.ReleaseDate.Date > awfp.ReleaseDateTo.Date)

//                                // BEGIN MID Track #4357 - security for interfaced and non-interfaced headers

//                                //TT#1313-MD -jsobek -
//                                if (ahp.AllocationTypeFlags.IsInterfaced)
//                                {
//                                    if (!aGetInterfacedHeaders)
//                                    {
//                                        selectHeader = false;
//                                    }
//                                }
//                                else if (!aGetNonInterfacedHeaders)
//                                {
//                                    selectHeader = false;
//                                }

//                                // END MID Track #4357

//                                // BEGIN MID Track #2391 - workspace filter
//                                // can not check here; type is not correct
//                                // check header type
////								if (selectHeader)
////								{
////									HeaderType ht = (HeaderType)awfp.SelectedTypes[ahp.HeaderType];
////									if (ht == null)
////									{
////										selectHeader = false;
////									}
////									else if (!ht.IsDisplayed)
////									{
////										selectHeader = false;
////									}
////								}
//                                // END MID Track #2391

//                                // can not check here; status is not correct
//                                //									// check header status
//                                //									if (selectHeader)
//                                //									{
//                                //										HeaderStatus hs = (HeaderStatus)awfp.SelectedStatuses[ahp.DetermineHeaderAllocationStatus];
//                                //										if (hs == null)
//                                //										{
//                                //											selectHeader = false;
//                                //										}
//                                //										else if (!hs.IsDisplayed)
//                                //										{
//                                //											selectHeader = false;
//                                //										}
//                                //									}

//                                if (selectHeader)
//                                {   // Begin TT#2 - Ron Matelic - Assortment Planning
//                                    if (aGetAssortmentHeaders)
//                                    {
//                                        if (ahp.HeaderType == eHeaderType.Assortment)
//                                        {
//                                            //if (_assortmentRIDs.Contains(ahp.Key)) //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -no reason to cache assortment RIDS
//                                            //{
//                                                headers.Add(ahp);
//                                            //}
//                                        }
//                                    }
//                                    else
//                                    {
//                                        headers.Add(ahp);
//                                        //hdrRIDs.Add(ahp.Key); // MuiltiHeader //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -variable not used
//                                    }
//                                }   // End TT#2
//                            }
//                        }
//                    }
//                    // Begin TT#1036 - RMatelic  Mulit header won't clear from workspace once released : commented out following  code - no longer applies  
//                    // MuiltiHeader  - if any any of the headers in a Multi group is already selected, 
//                    // then all of the headers in the group need to be selected
//                    //if (_headerGroupsByRID.Count > 0 && !aGetAssortmentHeaders) // TT#2 - Assprtment Planning
//                    //{
//                    //    foreach (int groupRID in _headerGroupsByRID.Keys) 
//                    //    {
//                    //        bool selectAll = false;
//                    //        HeaderGroupInfo hgi = (HeaderGroupInfo)_headerGroupsByRID[groupRID]; 
						
//                    //        if (hdrRIDs.Contains(groupRID))
//                    //        {
//                    //            selectAll = true;
//                    //        }
//                    //        else
//                    //        {
//                    //            foreach (int hdrRID in hgi.Headers)
//                    //            {
//                    //                if (hdrRIDs.Contains(hdrRID))
//                    //                {
//                    //                    selectAll = true;
//                    //                    break;
//                    //                }
//                    //            }
//                    //        }
//                    //        if (selectAll)
//                    //        {
//                    //            AllocationHeaderProfile ahp; 
//                    //            if (!hdrRIDs.Contains(groupRID))
//                    //            {
//                    //                ahp = GetHeaderData(groupRID, aIncludeComponents, aIncludeCharacteristics);
//                    //                if (ahp.Key != Include.NoRID)
//                    //                {
//                    //                    headers.Add(ahp);
//                    //                    //Begin Track #6036 - JSmith - Index already added error
//                    //                    hdrRIDs.Add(ahp.Key);
//                    //                    //End Track #6036
//                    //                }
//                    //            }
//                    //            foreach (int hdrRID in hgi.Headers)
//                    //            {
//                    //                if (!hdrRIDs.Contains(hdrRID))
//                    //                {
//                    //                    ahp = GetHeaderData(hdrRID, aIncludeComponents, aIncludeCharacteristics);
//                    //                    if (ahp.Key != Include.NoRID)
//                    //                    {
//                    //                        headers.Add(ahp);
//                    //                    }
//                    //                }
//                    //            }
//                    //        }
//                    //    }
//                    //}	
//                    // End TT#1036
//                    return headers;
//                }      
//                finally
//                {
//                    // Ensure that the lock is released.
//                    header_rwl.ReleaseReaderLock();
//                }
//            }
//            catch (ApplicationException ex)
//            {
//                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
//                if (Audit != null)
//                {
//                    Audit.Log_Exception(ex);
//                }
//                // End TT#189
//                // The reader lock request timed out.
//                EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:GetHeaderData reader lock has timed out", EventLogEntryType.Error);
//                throw new MIDException (eErrorLevel.severe,	0, "MIDHeaderService:GetHeaderData reader lock has timed out");
//            }
//        }

        //private static ArrayList _authorizedStyles = null;  //TT#1313-MD -jsobek - Cache security styles
        private static bool? isMasterAllocationInstalled = null; //TT#1313-MD -jsobek - Cache security styles
        /// <summary>
        /// Requests the session get all information associated with headers for which a user is authorized to reference.
        /// </summary>
        public static ArrayList GetHeadersForWorkspace(int headerFilterRID, FilterHeaderOptions headerFilterOptions, SessionAddressBlock aSAB) //TT#1313-MD -jsobek -Header Filters     // MID Track #4357 - security for interfaced and non-interfaced headers
        {
            try
            {
                if (isMasterAllocationInstalled == null)
                {
                    isMasterAllocationInstalled = aSAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled;
                }
                BuildHeaderDataForWorkspace(headerFilterRID, headerFilterOptions);

                //Debug.WriteLine("GetHeadersForWorkspace: ALLOC" + _headersByRIDAllocation.Count);
                //Debug.WriteLine("GetHeadersForWorkspace: ASRTM" + _headersByRIDAssortment.Count);

                header_rwl.AcquireReaderLock(ReaderLockTimeOut);
                try
                {
                    ArrayList styles = new ArrayList(); // get a list of unique style node RIDs to determine security
                    //Begin TT#1477-MD -jsobek -Header Filter Sort on Workspace
                    DataTable dtStyles = _dtHeaderStyles.DefaultView.ToTable(true, "STYLE_HNRID");
                    foreach (DataRow drStyle in dtStyles.Rows)
                    {
                        if (drStyle["STYLE_HNRID"] != DBNull.Value)
                        {
                            int styleRID = (int)drStyle["STYLE_HNRID"];
                            styles.Add(styleRID);
                        }
                    }
                    //foreach (int styleRID in _headersByStyle.Keys)
                    //{
                    //    if (!styles.Contains(styleRID))
                    //    {
                    //        styles.Add(styleRID);
                    //    }
                    //}
                    //End TT#1477-MD -jsobek -Header Filter Sort on Workspace
                    DataTable dtHeaderRids = new DataTable();
                    dtHeaderRids.Columns.Add("HDR_RID", typeof(int));

                    //if (_authorizedStyles == null)
                    //{
                    ArrayList _authorizedStyles = aSAB.HierarchyServerSession.GetMyAllocationStyles(styles);
                    //}

                    //Begin TT#1477-MD -jsobek -Header Filter Sort on Workspace
                    //foreach (int styleRID in _authorizedStyles)
                    //{
                    //    ArrayList styleHeaders = (ArrayList)_headersByStyle[styleRID]; // get headers for style from cross reference
                    //    if (styleHeaders != null)
                    //    {
                    //        foreach (int headerRID in styleHeaders)
                    //        {
                    //            //We are always getting interfaced and non-interfaced headers for the workspace, so there is no need to get additional info here
                    //            //Place authorized headers in the datatable
                    //            if (dtHeaderRids.Select("HDR_RID=" + headerRID.ToString()).Length == 0) //ensure HDR_RIDs are distinct, and only added to the datatable one time
                    //            {
                    //                DataRow dr = dtHeaderRids.NewRow();
                    //                dr["HDR_RID"] = headerRID;
                    //                dtHeaderRids.Rows.Add(dr);
                    //            }
                    //        }
                    //    }
                    //}

                    foreach (DataRow drHeader in _dtHeaderStyles.Rows)
                    {
                        bool allowHeader = false;
                        if (drHeader["STYLE_HNRID"] != DBNull.Value)
                        {
                            int styleRID = (int)drHeader["STYLE_HNRID"];
                            if (_authorizedStyles.Contains(styleRID))
                            {
                                allowHeader = true;
                            }
                        }
                        else
                        {
                            allowHeader = true;
                        }
                        if (allowHeader)
                        {
                            DataRow dr = dtHeaderRids.NewRow();
                            dr["HDR_RID"] = drHeader["HDR_RID"];
                            dtHeaderRids.Rows.Add(dr);
                        }
                                   
                    }
                    
                    //foreach (int styleRID in _authorizedStyles)
                    //{
                    //    //ArrayList styleHeaders = (ArrayList)_headersByStyle[styleRID]; // get headers for style from cross reference
                    //    if (styleHeaders != null)
                    //    {
                    //        foreach (int headerRID in styleHeaders.
                    //        {
                    //            //We are always getting interfaced and non-interfaced headers for the workspace, so there is no need to get additional info here
                    //            //Place authorized headers in the datatable
                    //            if (dtHeaderRids.Select("HDR_RID=" + headerRID.ToString()).Length == 0) //ensure HDR_RIDs are distinct, and only added to the datatable one time
                    //            {
                    //                DataRow dr = dtHeaderRids.NewRow();
                    //                dr["HDR_RID"] = headerRID;
                    //                dtHeaderRids.Rows.Add(dr);
                    //            }
                    //        }
                    //    }
                    //}
                    //End TT#1477-MD -jsobek -Header Filter Sort on Workspace

                    HeaderCharacteristicsData hcd = new HeaderCharacteristicsData();
                    DataTable dtHeaderChar = hcd.ReadHeaderCharacteristicsForHeaderRidList(dtHeaderRids);
                    ArrayList headers = new ArrayList();
                    foreach (DataRow drHeader in dtHeaderRids.Rows)
                    {
                        int headerRID = (int)drHeader["HDR_RID"];
                        AllocationHeaderProfile ahp = GetAllocationHeaderProfile(headerRID, aIncludeComponents: false, aIncludeCharacteristics: true, blForceGet: false, dtProvidedCharValuesForHeaderList: dtHeaderChar);
                        headers.Add(ahp);
                    }
           
                    return headers;
                }
                finally
                {
                    header_rwl.ReleaseReaderLock(); // Ensure that the lock is released.
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:GetHeadersForWorkspace reader lock has timed out", EventLogEntryType.Error);  // The reader lock request timed out.
                throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:GetHeadersForWorkspace reader lock has timed out");
            }
        }

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        /// <summary>
        /// Requests the session get all information associated with headers that have not been released.
        /// </summary>
        /// <returns>An ArrayList of AllocationHeaderProfiles</returns>
        //static public ArrayList GetNonReleasedHeaders()
        //{
        //    try
        //    {
        //        BuildNonReleasedHeaders();
        //        header_rwl.AcquireReaderLock(ReaderLockTimeOut);
        //        try
        //        {

        //            ArrayList headers = new ArrayList();

        //            foreach (int headerRID in _headersByRID.Keys)
        //            {

        //                AllocationHeaderProfile ahp = GetHeaderData(headerRID, false, false);

        //                headers.Add(ahp);
        //            }

        //            return headers;
        //        }
        //        finally
        //        {
        //            // Ensure that the lock is released.
        //            header_rwl.ReleaseReaderLock();
        //        }
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        if (Audit != null)
        //        {
        //            Audit.Log_Exception(ex);
        //        }
        //        // The reader lock request timed out.
        //        EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:GetNonReleasedHeaders reader lock has timed out", EventLogEntryType.Error);
        //        throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:GetNonReleasedHeaders reader lock has timed out");
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        // End TT#1065

        //Begin TT#1313-MD -jsobek -Header Filters
        // Begin TT#827 - JSmith - Allocation Performance
        /// <summary>
        /// Requests the session get all headers to be processed by the Task List.
        /// </summary>
        /// <param name="aTaskListRID">The key of the TaskList</param>
        /// <param name="aTaskSeq">The sequence of the TaskList</param>
        /// <returns>A DataTable of headers</returns>
        //static public DataTable GetHeadersForTaskList(int aTaskListRID, int aTaskSeq)
        //{
        //    Hashtable HashMasterHeader = null;
        //    Hashtable HashSubordinateHeader = null;
        //    bool masterHeadersExist = false;
        //    try
        //    {
        //        masterHeadersExist = BuildMasterHeaders(HashMasterHeader, HashSubordinateHeader);

        //        DataTable dt = null;
        //        dt = _headerData.GetHeadersForTaskList(aTaskListRID, aTaskSeq);

        //        LoadHeaders(dt, masterHeadersExist);

        //        return dt;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters

       
        // End TT#827 - JSmith - Allocation Performance

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        ///// <summary>
        ///// Gets all information associated with a Header's definition using the Header name.
        ///// </summary>
        ///// <param name="HeaderID">The id of the Header</param>
        ///// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
        ///// <param name="aIncludeCharacteristics">A flag identifying if characteristics are to be included</param>
        ///// <returns>An instance of the AllocationHeaderProfile class containing the information for the Header</returns>
        //static public AllocationHeaderProfile GetHeaderData(string HeaderID, bool aIncludeComponents,
        //    bool aIncludeCharacteristics)
        //{
        //    return GetHeaderData(HeaderID, aIncludeComponents, aIncludeCharacteristics, false);
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

		/// <summary>
		/// Gets all information associated with a Header's definition using the Header name.
		/// </summary>
		/// <param name="HeaderID">The id of the Header</param>
		/// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
		/// <param name="aIncludeCharacteristics">A flag identifying if characteristics are to be included</param>
		/// <returns>An instance of the AllocationHeaderProfile class containing the information for the Header</returns>
		public static AllocationHeaderProfile GetHeaderData(string HeaderID, bool aIncludeComponents, bool aIncludeCharacteristics, bool blForceGet)
		{
			try
			{
				header_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
                    // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
                    // Begin TT#2 - Ron Matelic - Assortment Planning - remove hashtable check
                    //if (!_headersByID.Contains(HeaderID) &&
                    //     blForceGet)
                    if (blForceGet)
                    // End TT#2  
                    {
                        //BuildHeader(HeaderID);
                        DataTable dt = _headerData.GetHeader(HeaderID);
                        //LoadHeaders(dt, false);
                        Hashtable HashMasterHeader = null;
                        Hashtable HashSubordinateHeader = null;

                        foreach (DataRow dr in dt.Rows)
                        {
                            PopulateHeaderCrossReferenceFromDataRow(dr, false, HashMasterHeader, HashSubordinateHeader, false);		// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                        }
                    }
                    // End TT#1065

					if (_headersByIDAllocation.ContainsKey(HeaderID))	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
					{
                        int HeaderRID = (int)_headersByIDAllocation[HeaderID];	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
						//return GetHeaderData(HeaderRID, aIncludeComponents, aIncludeCharacteristics);
                        return GetAllocationHeaderProfile(HeaderRID, aIncludeComponents: aIncludeComponents, aIncludeCharacteristics: aIncludeCharacteristics, blForceGet: false);
					}
					else
					{
						AllocationHeaderProfile hp = new AllocationHeaderProfile(Include.NoRID);
						return hp;
					}
				}
				finally
				{
					// Ensure that the lock is released.
					header_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:GetHeaderData reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:GetHeaderData reader lock has timed out");
			}
		}

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        ///// <summary>
        ///// Gets all information associated with a Header's definition using the Header record id.
        ///// </summary>
        ///// <param name="HeaderRID">The record id of the Header</param>
        ///// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
        ///// <param name="aIncludeCharacteristics">A flag identifying if characteristics are to be included</param>
        ///// <returns>An instance of the AllocationHeaderProfile class containing the information for the Header</returns>
        //static public AllocationHeaderProfile GetHeaderData(int HeaderRID, bool aIncludeComponents,
        //    bool aIncludeCharacteristics)
        //{
        //    return GetHeaderData(HeaderRID, aIncludeComponents, aIncludeCharacteristics, false);
        //}

		/// <summary>
		/// Gets all information associated with a Header's definition using the Header record id.
		/// </summary>
		/// <param name="HeaderRID">The record id of the Header</param>
		/// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
		/// <param name="aIncludeCharacteristics">A flag identifying if characteristics are to be included</param>
		/// <returns>An instance of the AllocationHeaderProfile class containing the information for the Header</returns>
        public static AllocationHeaderProfile GetAllocationHeaderProfile(int HeaderRID, bool aIncludeComponents, bool aIncludeCharacteristics, bool blForceGet, DataTable dtProvidedCharValuesForHeaderList = null) //TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
		{
			try
			{
				HeaderPackInfo hpi;
				HeaderPackProfile hpp;
				HeaderPackColorProfile hpcp;
				HeaderPackColorSizeProfile hpcsp;
				HeaderBulkColorProfile hbcp;
				HeaderBulkColorSizeProfile hbcsp;
				AllocationHeaderProfile ahp;

                // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
                // Begin TT#2 - Ron Matelic - Assortmen Planning - remove hashtable check
                //if (!_headersByRID.Contains(HeaderRID) &&
                //     blForceGet)
                if (blForceGet)
                // End TT#2  
                {
                    //BuildHeader(HeaderRID);
                    // Begin TT#1607-MD - JSmith - Workflow Header Field not populated with workflow name after processing on line.  Have to do a Tools>Refresh then name appears.  This does not happen in 5.21.
                    //DataTable dt = _headerData.GetHeader(HeaderRID);
                    DataTable dt = _headerData.GetHeaderForWorkspace(HeaderRID);
                    // End TT#1607-MD - JSmith - Workflow Header Field not populated with workflow name after processing on line.  Have to do a Tools>Refresh then name appears.  This does not happen in 5.21.
                    //LoadHeaders(dt, false);
                    Hashtable HashMasterHeader = null;
                    Hashtable HashSubordinateHeader = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        PopulateHeaderCrossReferenceFromDataRow(dr, false, HashMasterHeader, HashSubordinateHeader, false);	// TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                    }
                }
                // End TT#1065

				HeaderInfo hi = GetHeaderInfoFromCrossReference(HeaderRID);
				if (hi == null)
				{
					ahp = new AllocationHeaderProfile(Include.NoRID);
				}
				else
				{
					ahp = new AllocationHeaderProfile(hi.HeaderRID);

                    
					ahp.HeaderID = hi.HeaderID;
					ahp.HeaderDescription = hi.HeaderDescription;
					ahp.HeaderDay = hi.HeaderDay;
					ahp.OriginalReceiptDay = hi.OriginalReceiptDay;
					ahp.UnitRetail = hi.UnitRetail;
					ahp.UnitCost = hi.UnitCost;
					ahp.TotalUnitsToAllocate = hi.TotalUnitsToAllocate;
					ahp.StyleHnRID = hi.StyleHnRID;
					ahp.PlanHnRID = hi.PlanHnRID;
					ahp.OnHandHnRID = hi.OnHandHnRID;
					ahp.BulkMultiple = hi.BulkMultiple;
					ahp.AllocationMultiple = hi.AllocationMultiple;
                    ahp.AllocationMultipleDefault = hi.AllocationMultipleDefault; // MID Track 5761 Allocation Multiple not saved to Database
					ahp.Vendor = hi.Vendor;
					ahp.PurchaseOrder = hi.PurchaseOrder;
					ahp.BeginDay = hi.BeginDay;
					ahp.LastNeedDay = hi.LastNeedDay;
					ahp.ShipToDay = hi.ShipDay;
					ahp.ReleaseDate = hi.ReleaseDate;
					ahp.ReleaseApprovedDate = hi.ReleaseApprovedDate;
					ahp.HeaderGroupRID = hi.HeaderGroupRID;
					ahp.SizeGroupRID = hi.SizeGroupRID;
					ahp.WorkflowRID = hi.WorkflowRID;
					ahp.API_WorkflowRID = hi.API_WorkflowRID;
					ahp.MethodRID = hi.MethodRID;
					ahp.AllocationStatusFlags = hi.AllocationStatusFlags;
					ahp.BalanceStatusFlags = hi.BalanceStatusFlags;
					ahp.ShippingStatusFlags = hi.ShippingStatusFlags;
					ahp.AllocationTypeFlags = hi.AllocationTypeFlags;
					ahp.IntransitUpdateStatusFlags = hi.IntransitUpdateStatusFlags;
					ahp.PercentNeedLimit = hi.PercentNeedLimit;
					ahp.PlanFactor = hi.PlanFactor;
					ahp.ReserveUnits = hi.ReserveUnits;
					ahp.GradeWeekCount = hi.GradeWeekCount;
					ahp.PrimarySecondaryRID = hi.PrimarySecondaryRID;
					ahp.DistributionCenter = hi.DistributionCenter;
					ahp.AllocationNotes = hi.AllocationNotes;
					ahp.WorkflowTrigger = hi.WorkflowTrigger;
					ahp.API_WorkflowTrigger = hi.API_WorkflowTrigger;
					ahp.EarliestShipDay = hi.EarliestShipDay;
					// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					ahp.AllocatedUnits = hi.AllocatedUnits;
					ahp.OrigAllocatedUnits = hi.OrigAllocatedUnits;
					ahp.RsvAllocatedUnits = hi.RsvAllocatedUnits;
					ahp.ReleaseCount = hi.ReleaseCount;
					// (CSMITH) - END MID Track #3219
					ahp.HeaderAllocationStatus = hi.HeaderAllocationStatus;
					ahp.HeaderType = hi.HeaderType;
					ahp.HeaderIntransitStatus = hi.HeaderIntransitStatus;
					ahp.HeaderShipStatus = hi.HeaderShipStatus;
					ahp.MasterRID = hi.MasterRID;
					ahp.MasterID = hi.MasterID;
					ahp.SubordinateRID = hi.SubordinateRID;
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    foreach (int subordinateRID in hi.SubordinateRIDs)
                    {
                        ahp.SubordinateRIDs.Add(subordinateRID);
                    }
                    ahp.DCFulfillmentProcessed = hi.DCFulfillmentProcessed;
                    // End TT#1966-MD - JSmith - DC Fulfillment
					ahp.SubordinateID = hi.SubordinateID;
                    ahp.AsrtRID = hi.AsrtRID;
                    ahp.PlaceHolderRID = hi.PlaceHolderRID;
                    ahp.AsrtType = hi.AsrtType;
					// begin MID Track 4448 AnF Audit Enhancement
					ahp.StoreSizeManualAllocationTotal = hi.StoreSizeManualAllocationTotal;
					ahp.StoreStyleManualAllocationTotal = hi.StoreStyleManualAllocationTotal;
					ahp.StoreStyleAllocationManuallyChangedCount = hi.StoreStyleAllocationManuallyChangedCount;
					ahp.StoreSizeAllocationManuallyChangedCount = hi.StoreSizeAllocationManuallyChangedCount;
					ahp.StoresWithAllocationCount = hi.StoresWithAllocationCount;
					ahp.HorizonOverride = hi.HorizonOverride;
					// end MID Track 4448 AnF Audit Enhancement

                    // BEGIN Workspace Usability Enhancement - Ron Matelic
                    ahp.PackCount = hi.PackCount;
                    ahp.BulkColorCount = hi.BulkColorCount;
                    ahp.BulkColorSizeCount = hi.BulkColorSizeCount;
                    // END Workspace Usability Enhancement  

                    //Begin TT#1313-MD -jsobek -Header Filters
                    ahp.NodeDisplayForOtsForecast = hi.NodeDisplayForOtsForecast;
                    ahp.NodeDisplayForOnHand = hi.NodeDisplayForOnHand;
                    ahp.NodeDisplayForGradeInvBasis = hi.NodeDisplayForGradeInvBasis;
                    ahp.WorkflowName = hi.WorkflowName;
                    ahp.APIWorkflowName = hi.APIWorkflowName;
                    ahp.HeaderMethodName = hi.HeaderMethodName;
                    //End TT#1313-MD -jsobek -Header Filters

                    ahp.GradeSG_RID = hi.GradeSG_RID;       // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)`
                    ahp.GradeInventoryBasisHnRID = hi.GradeInventoryBasisHnRID; // TT#1287 - JEllis - Inventory Min Max
                    ahp.GradeInventoryMinimumMaximum = hi.GradeInventoryMinMax; // TT#1287 - JEllis - Inventory Min Max

                    // Begin TT#1401 - RMatelic - Reservation Stores
                    ahp.ImoID = hi.ImoID;
                    ahp.TotalItemUnitsAllocated = hi.ItemUnitsAllocated;
                    ahp.TotalItemOrigUnitsAllocated = hi.ItemOrigUnitsAllocated;
                    // End TT#1401 
					ahp.AsrtPlaceholderSeq = hi.AsrtPlaceholderSeq;	// TT#1227 stodd - assortment header seq
					ahp.AsrtHeaderSeq = hi.AsrtHeaderSeq;	// TT#1227 stodd - assortment header seq
                    ahp.AssortmentID = hi.AssortmentID;		//TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace
                    ahp.AsrtTypeForParentAsrt= hi.AsrtTypeForParentAsrt;	// TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
                    ahp.UnitsPerCarton = hi.UnitsPerCarton;                 // TT#1652-MD - RMatelic - DC Carton Rounding
    
                    if (aIncludeComponents)
					{
                        // Begin Track #5841 - JSmith - Performance
                        if (!hi.AreComponentsLoaded)
                        {
                            BuildComponents(hi);
                        }
                        // End Track #5841

						// add packs
						foreach (int packRID in hi.Packs)
						{
							hpi = (HeaderPackInfo)_packsByRID[packRID];
							hpp = new HeaderPackProfile(packRID);
							hpp.HeaderPackName = hpi.HeaderPackName;
							hpp.HeaderRID = hpi.HeaderRID;
							hpp.Multiple = hpi.Multiple;
							hpp.Packs = hpi.Packs;
							hpp.ReservePacks = hpi.ReservePacks;
							hpp.GenericInd = hpi.GenericInd;
							hpp.AssociatedPackRID = hpi.AssociatedPackRID;
                            hpp.Sequence = hpi.Sequence;
                            // Begin TT#1966-MD - JSmith - DC Fulfillment
                            foreach (int associatedPackRID in hpi.AssociatedPackRIDs)
                            {
                                hpp.AssociatedPackRIDs.Add(associatedPackRID);
                                hpp.AssociatedPackName = hpi.AssociatedPackName;  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
                            }
                            // End TT#1966-MD - JSmith - DC Fulfillment

							// add colors to packs
							foreach (DictionaryEntry color in hpi.Colors)
							{
								int colorRID = (int)color.Key;
								HeaderPackColorInfo hpci = (HeaderPackColorInfo)color.Value;

								hpcp = new HeaderPackColorProfile(colorRID);
								hpcp.Units = hpci.Units;
								hpcp.Sequence = hpci.Sequence;
                                
                                // Assortment BEGIN
                                hpcp.HdrPCRID = hpci.HdrPCRID;
                                hpcp.ColorName = hpci.ColorName;
                                hpcp.ColorDescription = hpci.ColorDescription;
                                hpcp.Last_PCSZ_Key_Used = hpci.Last_PCSZ_Key_Used;
                                // Assortment END
							
                                //add sizes to pack color
								hpcp.Sizes = new Hashtable();
								foreach (DictionaryEntry size in hpci.Sizes)
								{
									int sizeRID = (int)size.Key;
									HeaderPackColorSizeInfo hpcsi = (HeaderPackColorSizeInfo)size.Value;

									hpcsp = new HeaderPackColorSizeProfile(sizeRID);
                                    hpcsp.HDR_PCSZ_Key = hpcsi.HDR_PCSZ_Key; // Assortment: Color/Size Change
									hpcsp.Units = hpcsi.Units;
									hpcsp.Sequence = hpcsi.Sequence;
									hpcp.Sizes.Add(hpcsp.Key, hpcsp);
								}
								hpp.Colors.Add(hpcp.Key, hpcp);
							}

							ahp.Packs.Add(hpp.Key, hpp);
						}

						// add bulk colors
						foreach (DictionaryEntry color in hi.BulkColors)
						{
							int colorRID = (int)color.Key;
							HeaderBulkColorInfo hbci = (HeaderBulkColorInfo)color.Value;

							hbcp = new HeaderBulkColorProfile(colorRID);
							hbcp.Units = hbci.Units;
							hbcp.Multiple = hbci.Multiple;
							hbcp.Minimum = hbci.Multiple;
							hbcp.Maximum = hbci.Maximum;
							hbcp.ReserveUnits = hbci.ReserveUnits;
							hbcp.Sequence = hbci.Sequence;

                            // Assortment BEGIN
                            hbcp.HdrBCRID = hbci.HdrBCRID;
                            hbcp.Last_BCSZ_Key_Used = hbci.Last_BCSZ_Key_Used;
                            hbcp.Name = hbci.Name;
                            hbcp.Description = hbci.Description;
                            hbcp.AsrtBCRID = hbci.AsrtBCRID;
                            // Assortment END

                            hbcp.ColorStatusFlags = hbci.ColorStatusFlags; // TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5

                            //add sizes to bulk color
							hbcp.Sizes = new Hashtable();
							foreach (DictionaryEntry size in hbci.Sizes)
							{
								int sizeRID = (int)size.Key;
								HeaderBulkColorSizeInfo hbcsi = (HeaderBulkColorSizeInfo)size.Value;

								hbcsp = new HeaderBulkColorSizeProfile(sizeRID);
                                hbcsp.HDR_BCSZ_Key = hbcsi.HDR_BCSZ_KEY; // Assortment: Color/Size Change
                                hbcsp.Units = hbcsi.Units;
								hbcsp.Multiple = hbcsi.Multiple;
								hbcsp.Minimum = hbcsi.Multiple;
								hbcsp.Maximum = hbcsi.Maximum;
								hbcsp.ReserveUnits = hbcsi.ReserveUnits;
								hbcsp.Sequence = hbcsi.Sequence;
								hbcp.Sizes.Add(hbcsp.Key, hbcsp);
							}
							ahp.BulkColors.Add(hbcp.Key, hbcp);
						}
					}

					if (aIncludeCharacteristics)
					{
						BuildHeaderCharJoins(HeaderRID, dtProvidedCharValuesForHeaderList);




						HeaderCharGroupProfileList hcgpl = GetHeaderCharGroups(); //reads characteristic groups from a hashtable
						HeaderCharProfile hcp = null;
						ahp.Characteristics = new HeaderCharProfileList(eProfileType.HeaderChar);
						// create a list of all groups
						foreach (HeaderCharGroupProfile hcgp in hcgpl)
						{
							hcp = new HeaderCharProfile(hcgp.Key);
                            // BEGIN MID Track #5488 
							//hcp.GroupRID = hcgp.Key;
                            hcp.HeaderCharType = hcgp.Type;     // MID Track #5488 - characteristic error
                            // END MID Track #5488
                            ahp.Characteristics.Add(hcp);
						}
						// get value for each group of any
						foreach (int headerCharRID in hi.Characteristics)
						{
							HeaderCharInfo hci = GetHeaderChararacteristic(headerCharRID);
							if (hci != null)
							{
								HeaderCharGroupProfile hcgp = (HeaderCharGroupProfile)hcgpl.FindKey(hci.GroupRID);
								hcp = (HeaderCharProfile)ahp.Characteristics.FindKey(hci.GroupRID);
								// BEGIN MID Track #5488 
                                //hcp.Key = hci.RID;
								//hcp.GroupRID = hci.GroupRID;
                                //hcp.GroupRID = hci.RID;
                                hcp.CharRID = hci.RID;
                                // END MID Track #5488
								hcp.HeaderCharType = hcgp.Type;
								hcp.DateValue = hci.DateValue;
								hcp.DollarValue = hci.DollarValue;
								hcp.NumberValue = hci.NumberValue;
								hcp.TextValue = hci.TextValue;
								switch (hcgp.Type)
								{
									case eHeaderCharType.boolean:
										//hcp.Text = hcp.TextValue;
										break;
									case eHeaderCharType.date:
										hcp.Text = hcp.DateValue.ToShortDateString();
										break;
									case eHeaderCharType.dollar:
										hcp.Text = hcp.DollarValue.ToString();
										break;
									case eHeaderCharType.number:
                                        // BEGIN MID Track #5488 - characteristic error
                                        //hcp.Text = hcp.TextValue;
                                        hcp.Text = hcp.NumberValue.ToString();
                                        // END MID Track #5488
                                        break;
									case eHeaderCharType.text:
										hcp.Text = hcp.TextValue;
										break;
									case eHeaderCharType.list:
										break;
								}
							}
						}
					}
				}

				return ahp;

			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets a HeaderCharGroupProfile.
		/// </summary>
		/// <returns>
		/// An instance of the HeaderCharGroupProfile class containing the information for the 
		/// Header Characteristics Group
		/// </returns>
		static public HeaderCharGroupProfile GetHeaderCharGroup(int aHeaderCharGroupRID)
		{
			try
			{
				HeaderCharGroupProfile hcgp = new HeaderCharGroupProfile(aHeaderCharGroupRID);
				HeaderCharGroupInfo hcgi = GetHeaderCharGroupInfoFromCrossReference(aHeaderCharGroupRID);
				if (hcgi != null)
				{
					hcgp.ID = hcgi.ID;
					hcgp.Type = hcgi.Type;
					hcgp.ListInd = hcgi.ListInd;
					hcgp.ProtectInd = hcgi.ProtectInd;
					if (hcgp.ListInd)
					{
						hcgp.Characteristics = hcgi.Characteristics;
					}
				}
				return hcgp;

			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets a list of all header characteristic groups.
		/// </summary>
		/// <returns>
		/// An instance of the HeaderCharGroupProfileList class containing the information for the 
		/// Header Characteristics Groups
		/// </returns>
		static public HeaderCharGroupProfileList GetHeaderCharGroups()
		{
			try
			{
				HeaderCharGroupProfileList hcgpl = new HeaderCharGroupProfileList(eProfileType.HeaderCharGroup);
				foreach (int groupRID in _headerCharGroupsByID.Values)
				{
					hcgpl.Add(GetHeaderCharGroup(groupRID));
				}
				return hcgpl;

			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Gets a HeaderInfo object.
		/// </summary>
		/// <returns>
		/// An instance of the HeaderInfo class
		/// </returns>
		static private HeaderInfo GetHeaderInfoFromCrossReference(int aHeaderRID)
		{
			try
			{
				header_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                    if (_headersByRIDAllocation.ContainsKey(aHeaderRID))
                    {
                        return (HeaderInfo)_headersByRIDAllocation[aHeaderRID];
                    }
                    if (_headersByRIDAssortment.ContainsKey(aHeaderRID))
                    {
                        return (HeaderInfo)_headersByRIDAssortment[aHeaderRID];
                    }
                    return null;
					// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
				}
				finally
				{
					// Ensure that the lock is released.
					header_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:GetHeaderInfo reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:GetHeaderInfo reader lock has timed out");
			}
		}

        // Begin TT#5697 - JSmith - Subordinate header release fails in Style Review
        /// <summary>
        /// Deletes a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="aHeaderRID">The record id of the Header</param>
        static public void DeleteHeader(int aHeaderRID)
        {
            try
            {
                header_rwl.AcquireWriterLock(ReaderLockTimeOut);
                try
                {
                    if (_headersByRIDAllocation.ContainsKey(aHeaderRID))
                    {
                        _headersByRIDAllocation.Remove(aHeaderRID);
                    }
                    if (_headersByRIDAssortment.ContainsKey(aHeaderRID))
                    {
                        _headersByRIDAssortment.Remove(aHeaderRID);
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    header_rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // The reader lock request timed out.
                EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:DeleteHeader reader lock has timed out", EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:DeleteHeader reader lock has timed out");
            }
        }
        // End TT#5697 - JSmith - Subordinate header release fails in Style Review

		/// <summary>
		/// Gets a list of all header characteristic groups.
		/// </summary>
		/// <returns>
		/// An instance of the HeaderCharGroupInfo class
		/// </returns>
		static private HeaderCharGroupInfo GetHeaderCharGroupInfoFromCrossReference(int aHeaderCharGroupRID)
		{
			try
			{
				headerChar_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					return (HeaderCharGroupInfo)_headerCharGroupsByRID[aHeaderCharGroupRID];
				}
				finally
				{
					// Ensure that the lock is released.
					headerChar_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:GetHeaderCharGroupInfo reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:GetHeaderCharGroupInfo reader lock has timed out");
			}
		}

		/// <summary>
		/// Gets a list of all header characteristic groups.
		/// </summary>
		/// <returns>
		/// An instance of the HeaderCharInfo class containing the information for the 
		/// Header Characteristics
		/// </returns>
		static private HeaderCharInfo GetHeaderChararacteristic(int aHeaderCharRID)
		{
			try
			{
				headerChar_rwl.AcquireReaderLock(ReaderLockTimeOut);
				try
				{
					return (HeaderCharInfo)_headerCharsByRID[aHeaderCharRID];
				}
				finally
				{
					// Ensure that the lock is released.
					headerChar_rwl.ReleaseReaderLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:HeaderCharGroupProfileList reader lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:HeaderCharGroupProfileList reader lock has timed out");
			}
		}

		/// <summary>
		/// Gets the key for the header type in the characteristic group.
		/// </summary>
		/// <param name="aHeaderCharGroupRID">The key (RID) of the header characteristic group</param>
		/// <param name="aCharValue">The value for the characteristic</param>
		/// <returns>
		/// The key (RID) of the header characteristic if the type exists, Include.NoRID (-1) if it doesn't exist
		/// </returns>
		static public int GetCharForCharGroup(int aHeaderCharGroupRID, object aCharValue)
		{
			try
			{
				HeaderCharGroupProfile hcgp = GetHeaderCharGroup(aHeaderCharGroupRID);

				DataTable dt = _headerCharacteristicsData.HeaderChar_Read(aHeaderCharGroupRID, hcgp.Type, aCharValue);
				if (dt.Rows.Count > 0)
				{
					DataRow dr = dt.Rows[0];
					return Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentUICulture);
				}

				return Include.NoRID;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Add a characteristic to a group.
		/// </summary>
		/// <param name="aHeaderCharGroupRID">The key (RID) of the header characteristic group</param>
		/// <param name="aHeaderCharRID">The key (RID) of the header characteristic</param>
		/// <param name="aCharValue">The value for the characteristic</param>
		static public void UpdateCharInCharGroup(int aHeaderCharGroupRID, int aHeaderCharRID,
			object aCharValue)
		{
			HeaderCharGroupInfo hcgi = null;
			HeaderCharInfo hci = null;

			try
			{
				headerChar_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					hcgi = (HeaderCharGroupInfo)_headerCharGroupsByRID[aHeaderCharGroupRID];
					hci = new HeaderCharInfo();

					hci.RID = aHeaderCharRID;
					hci.GroupRID = aHeaderCharGroupRID;
					switch (hcgi.Type)
					{
						case eHeaderCharType.text:
							hci.TextValue = Convert.ToString(aCharValue, CultureInfo.CurrentUICulture);
							break;
						case eHeaderCharType.date:
							hci.DateValue = Convert.ToDateTime(aCharValue, CultureInfo.CurrentUICulture);
							break;
						case eHeaderCharType.number:
                            // Begin TT#3839 - JSmith - Header Characteristic created in numeric format- type in 5.50 and receive a system format error.  Expect to be able to typ in the 5.50
                            //hci.NumberValue = Convert.ToInt32(aCharValue, CultureInfo.CurrentUICulture);
                            hci.NumberValue = Convert.ToDouble(aCharValue, CultureInfo.CurrentUICulture);
                            // End TT#3839 - JSmith - Header Characteristic created in numeric format- type in 5.50 and receive a system format error.  Expect to be able to typ in the 5.50
							break;
						case eHeaderCharType.dollar:
                            // Begin TT#3839 - JSmith - Header Characteristic created in numeric format- type in 5.50 and receive a system format error.  Expect to be able to typ in the 5.50
                            //hci.DollarValue = Convert.ToInt32(aCharValue, CultureInfo.CurrentUICulture);
                            hci.DollarValue = Convert.ToDouble(aCharValue, CultureInfo.CurrentUICulture);
                            // End TT#3839 - JSmith - Header Characteristic created in numeric format- type in 5.50 and receive a system format error.  Expect to be able to typ in the 5.50
							break;
					}
					
					_headerCharsByRID.Add(hci.RID, hci);

					hcgi.Characteristics.Add(hci.RID, hci);
				}
				finally
				{
					// Ensure that the lock is released.
					headerChar_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:UpdateCharInCharGroup writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:UpdateCharInCharGroup writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Refreshes the characteristic list for a header.
		/// </summary>
		/// <param name="aHeaderRID">The key (RID) of the header</param>
		/// <param name="aCharacteristics">A list of the characteristics for the header</param>
		static public void RefreshHeaderCharacteristicsOnCrossReference(int aHeaderRID, ArrayList aCharacteristics)
		{
			try
			{
				header_rwl.AcquireWriterLock(WriterLockTimeOut);
				try
				{
					// Begin TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
                    HeaderInfo hi = null;
                    if (_headersByRIDAllocation.ContainsKey(aHeaderRID))
                    {
                        hi = (HeaderInfo)_headersByRIDAllocation[aHeaderRID];
                    }
                    if (_headersByRIDAssortment.ContainsKey(aHeaderRID))
                    {
                        hi = (HeaderInfo)_headersByRIDAssortment[aHeaderRID];
                    }
					// End TT#1438-MD - stodd - When Assortment is installed, the header server session only caches the assortment workspace headers
					
					if (hi != null)
					{
						hi.Characteristics.Clear();
						foreach (int charRID in aCharacteristics)
						{
							hi.Characteristics.Add(charRID);
						}
					}
				}
				finally
				{
					// Ensure that the lock is released.
					header_rwl.ReleaseWriterLock();
				}
			}
			catch (ApplicationException ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				// The reader lock request timed out.
				EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService:RefreshHeaderCharacteristics writer lock has timed out", EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDHeaderService:RefreshHeaderCharacteristics writer lock has timed out");
			}
			catch (Exception ex)
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

        // Begin TT#2 - Ron Matelic - Assortment Planning 4.0 
        /// <summary>
        /// Gets the list of HDR_RIDs within an Assortment including the Assortment
        /// </summary>
        /// <param name="aAsrtRID">The key (RID) of the assortment</param>
        /// </returns>
        /// The list of keys (HDR_RIDs) of the assortment including the assortment 
        /// </returns>
        static public ArrayList GetHeadersInAssortment(int aAsrtRID)
        {
            try
            {
                ArrayList al = new ArrayList();
                DataTable dt = _headerData.GetHeadersInAssortment(aAsrtRID);
             
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    al.Add(Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture));
                }
                return al;
            }
            catch
            {
                throw;
            }
        }
        // End TT#2

		// Begin TT#1581-MD - stodd - Header Reconcile API
        static public int GetNextHeaderSequenceNumber(int seqLength)
        {
            try
            {
                _headerData.OpenUpdateConnection();
                int seq = _headerData.GetNextHeaderSequenceNumber(seqLength);
                _headerData.CommitData();

                return seq;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_headerData.ConnectionIsOpen)
                {
                    _headerData.CloseUpdateConnection();
                }
            }
        }



        static public DataTable GetMatchingHeader(int styleNodeRID, string distCenter, string purchaseOrder, int headerDisplayType, string colorCodeId, string bulkOrPack, string headerAction)
        {
            try
            {
                DataTable dt = null;

                //DataTable dt = _headerData.GetMatchingHeader(styleNodeRID, distCenter, purchaseOrder, headerDisplayType, colorCodeId, bulkOrPack, headerAction);

                return dt;
            }
            catch
            {
                throw;
            }
        }
		// Begin TT#1581- md - stodd - Header Reconcile API
	}

	/// <summary>
	/// Contains the information about a header group. 
	/// </summary>
	[Serializable()]
	public class HeaderGroupInfo
	{
		// Fields
		private int			_headerGroupRID;
		private int			_headerRID;
		private string		_headerGroupDescription;
		private ArrayList	_headers;
	
		public HeaderGroupInfo()
		{
			_headers = new ArrayList();
		}

		// Properties

		/// <summary>
		/// Gets or sets the record id for the header group.
		/// </summary>
		public int HeaderGroupRID 
		{
			get { return _headerGroupRID ; }
			set { _headerGroupRID = value; }
		}
		/// <summary>
		/// Gets or sets the id of the header group.
		/// </summary>
		public int HeaderRID 
		{
			get { return _headerRID ; }
			set { _headerRID = value; }
		}
		/// <summary>
		/// Gets or sets the description of the header group.
		/// </summary>
		public string HeaderGroupDescription 
		{
			get { return _headerGroupDescription ; }
			set { _headerGroupDescription = value; }
		}
		/// <summary>
		/// Gets or sets the list of headers in the group.
		/// </summary>
		public ArrayList Headers 
		{
			get { return _headers ; }
			set { _headers = value; }
		}
	}

	/// <summary>
	/// Contains the information about a header. 
	/// </summary>
    [Serializable()]
    public class HeaderInfo
    {
        // Fields
        private int _headerRID;
        private string _headerID;
        private string _headerDescription;
        private DateTime _headerDay;
        private DateTime _originalReceiptDay;
        private double _unitRetail;
        private double _unitCost;
        private int _totalUnitsToAllocate;
        private int _styleHnRID;
        private int _planHnRID;
        private int _onHandHnRID;
        private int _bulkMultiple;
        private int _allocationMultiple;
        private int _allocationMultipleDefault; // MID Track 5761 Allocation Multiple not maintained on Database
        private string _vendor;
        private string _purchaseOrder;
        private DateTime _beginDay;
        private DateTime _lastNeedDay;
        private DateTime _shipDay;
        private DateTime _releaseDate;
        private DateTime _releaseApprovedDate;
        private int _headerGroupRID;
        private int _sizeGroupRID;
        private int _workflowRID;
        private int _apiWorkflowRID;
        private int _methodRID;
        public AllocationTypeFlags _allocationTypeFlags;
        public AllocationStatusFlags _allocationStatusFlags;
        public BalanceStatusFlags _balanceStatusFlags;
        public ShippingStatusFlags _shippingStatusFlags;
        public IntransitUpdateStatusFlags _intransitUpdateStatusFlags;
        private double _percentNeedLimit;
        private double _planFactor;
        private int _reserveUnits;
        private int _gradeWeekCount;
        private int _primarySecondaryRID;
        private string _distributionCenter;
        private string _allocationNotes;
        private bool _workflowTrigger;
        private bool _apiWorkflowTrigger;
        private DateTime _earliestShipDay;
        private ArrayList _packs;				// list of pack record IDs for the header
        private Hashtable _bulkColors;
        private ArrayList _characteristics;
        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
        private int _allocatedUnits;
        private int _origAllocatedUnits;
        private int _rsvAllocatedUnits;
        private int _releaseCount;
        // (CSMITH) - END MID Track #3219
        private eHeaderAllocationStatus _headerAllocationStatus;
        private eHeaderType _headerType;
        private eHeaderIntransitStatus _headerIntransitStatus;
        private eHeaderShipStatus _headerShipStatus;
        private int _masterRID;
        private string _masterID;
        private int _subordinateRID;
        // Begin TT#1966-MD - JSmith- DC Fulfillment
        private List<int> _subordinateRIDs = null;
        // End TT#1966-MD - JSmith- DC Fulfillment
        private string _subordinateID;
        // Assortment BEGIN
        private int _asrtRID;
        private int _placeHolderRID;
        private int _asrtType;
        private int _asrtPlaceholderSeq;
        private int _asrtHeaderSeq;
        // Assortment END
        private int _storeStyleAllocationManuallyChgdCnt; // MID Track 4448 ANF Audit Enhancement
        private int _storeSizeAllocationManuallyChgdCnt; // MID Track 4448 ANF Audit Enhancement
        private int _storeStyleManualAllocationTotal;    // MID Track 4448 AnF Audit Enhancement
        private int _storeSizeManualAllocationTotal;     // MID Track 4448 AnF Audit Enhancement 
        private int _storesWithAllocationCount;          // MID Track 4448 AnF Audit Enhancement
        private bool _horizonOverride;                   // MID Track 4448 AnF Audit Enhancement
        // Begin Track #5841 - JSmith - Performance
        private bool _areComponentsLoaded;
        // End Track #5841
        private int _packCount;                         // Workspace Usability Enhancement - Ron Matelic
        private int _bulkColorCount;                    // Workspace Usability Enhancement  
        private int _bulkColorSizeCount;                // Workspace Usability Enhancement  
        private string _nodeDisplayForOtsForecast; //TT#1313-MD -jsobek -Header Filters
        private string _nodeDisplayForOnHand; //TT#1313-MD -jsobek -Header Filters
        private string _nodeDisplayForGradeInvBasis; //TT#1313-MD -jsobek -Header Filters
        private string _workflowName; //TT#1313-MD -jsobek -Header Filters
        private string _headerMethodName; //TT#1313-MD -jsobek -Header Filters
        private string _apiWorkflowName; //TT#1313-MD -jsobek -Header Filters
        private int _gradeSG_RID;                       // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)`
        private bool _gradeInventoryMinMax;       // TT#1287 - JEllis - Inventory Minimum Maximum
        private int _gradeInventoryHnRID;         // TT#1287 - JEllis - Inventory Minimum Maximum 
        private string _imoID;                    // Begin TT#1401 - RMatelic - Reservation Stores 
        private int _itemUnitsAllocated;
        private int _itemOrigUnitsAllocated;      // End TT#1401  
        private string _asrtID;                 //TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace
        private int _asrtTypeForParentAsrt;		// TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
        private int _unitsPerCarton;            // TT#1652-MD - RMatelic - DC Carton Rounding
        private bool _DCFulfillmentProcessed = false;   // TT#1966-MD - JSmith- DC Fulfillment

        // Properties

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public HeaderInfo()
        {
            _packs = new ArrayList();
            _bulkColors = new Hashtable();
            _characteristics = new ArrayList();
            _workflowTrigger = false;
            _apiWorkflowTrigger = false;
            _apiWorkflowRID = Include.UndefinedWorkflowRID;
            _workflowRID = Include.UndefinedWorkflowRID;
            // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
            _allocatedUnits = 0;
            _origAllocatedUnits = 0;
            _rsvAllocatedUnits = 0;
            _releaseCount = 0;
            // (CSMITH) - END MID Track #3219
            _masterID = string.Empty;
            _masterRID = Include.NoRID;
            _subordinateID = string.Empty;
            _subordinateRID = Include.NoRID;
            // Begin Track #5841 - JSmith - Performance
            _areComponentsLoaded = false;
            // End Track #5841
        }

        /// <summary>
        /// Gets or sets the headers's record id.
        /// </summary>
        public int HeaderRID
        {
            get { return _headerRID; }
            set { _headerRID = value; }
        }
        /// <summary>
        /// Gets or sets the id of the header.
        /// </summary>
        public string HeaderID
        {
            get { return _headerID; }
            set { _headerID = value; }
        }
        /// <summary>
        /// Gets or sets the description the header.
        /// </summary>
        public string HeaderDescription
        {
            get { return _headerDescription; }
            set { _headerDescription = value; }
        }
        /// <summary>
        /// Gets or sets the day the header.
        /// </summary>
        public DateTime HeaderDay
        {
            get { return _headerDay; }
            set { _headerDay = value; }
        }
        /// <summary>
        /// Gets or sets the original receipt day the header.
        /// </summary>
        public DateTime OriginalReceiptDay
        {
            get { return _originalReceiptDay; }
            set { _originalReceiptDay = value; }
        }
        /// <summary>
        /// Gets or sets the unit retail of the header.
        /// </summary>
        public double UnitRetail
        {
            get { return _unitRetail; }
            set { _unitRetail = value; }
        }
        /// <summary>
        /// Gets or sets the unit cost of the header.
        /// </summary>
        public double UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }
        }
        /// <summary>
        /// Gets or sets the units received in of the header.
        /// </summary>
        public int TotalUnitsToAllocate
        {
            get { return _totalUnitsToAllocate; }
            set { _totalUnitsToAllocate = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the style in the header.
        /// </summary>
        public int StyleHnRID
        {
            get { return _styleHnRID; }
            set { _styleHnRID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the plan node in the header.
        /// </summary>
        public int PlanHnRID
        {
            get { return _planHnRID; }
            set { _planHnRID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the node to use for on-hand in the header.
        /// </summary>
        public int OnHandHnRID
        {
            get { return _onHandHnRID; }
            set { _onHandHnRID = value; }
        }
        /// <summary>
        /// Gets or sets the bulk multiple of the header.
        /// </summary>
        public int BulkMultiple
        {
            get { return _bulkMultiple; }
            set { _bulkMultiple = value; }
        }
        /// <summary>
        /// Gets or sets the allocation multiple of the header.
        /// </summary>
        public int AllocationMultiple
        {
            get { return _allocationMultiple; }
            set { _allocationMultiple = value; }
        }

        // begin MID Track 5761 Allocation Multiple not saved to Database
        public int AllocationMultipleDefault
        {
            get { return _allocationMultipleDefault; }
            set { _allocationMultipleDefault = value; }
        }
        // end MID Track 5761 Allocation Multiple not saved to Database

        /// <summary>
        /// Gets or sets the vendor of the header.
        /// </summary>
        public string Vendor
        {
            get { return _vendor; }
            set { _vendor = value; }
        }
        /// <summary>
        /// Gets or sets the purchase order information of the header.
        /// </summary>
        public string PurchaseOrder
        {
            get { return _purchaseOrder; }
            set { _purchaseOrder = value; }
        }
        /// <summary>
        /// Gets or sets the begin day of the header.
        /// </summary>
        public DateTime BeginDay
        {
            get { return _beginDay; }
            set { _beginDay = value; }
        }
        /// <summary>
        /// Gets or sets the last need day of the header.
        /// </summary>
        public DateTime LastNeedDay
        {
            get { return _lastNeedDay; }
            set { _lastNeedDay = value; }
        }
        /// <summary>
        /// Gets or sets the ship day of the header.
        /// </summary>
        public DateTime ShipDay
        {
            get { return _shipDay; }
            set { _shipDay = value; }
        }
        /// <summary>
        /// Gets or sets the release date and time of the header.
        /// </summary>
        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; }
        }
        /// <summary>
        /// Gets or sets the approval date for the release of the header.
        /// </summary>
        public DateTime ReleaseApprovedDate
        {
            get { return _releaseApprovedDate; }
            set { _releaseApprovedDate = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the header group to which the header belongs.
        /// </summary>
        public int HeaderGroupRID
        {
            get { return _headerGroupRID; }
            set { _headerGroupRID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the size group to which the header belongs.
        /// </summary>
        public int SizeGroupRID
        {
            get { return _sizeGroupRID; }
            set { _sizeGroupRID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the workflow to process for the header.
        /// </summary>
        public int WorkflowRID
        {
            get { return _workflowRID; }
            set { _workflowRID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the API_workflow for the header.
        /// </summary>
        public int API_WorkflowRID
        {
            get { return _apiWorkflowRID; }
            set { _apiWorkflowRID = value; }
        }
        /// <summary>
        /// Gets or sets the record ID of the method to process for the header.
        /// </summary>
        public int MethodRID
        {
            get { return _methodRID; }
            set { _methodRID = value; }
        }
        /// <summary>
        /// Gets or sets the allocation status flags for the header.
        /// </summary>
        public AllocationStatusFlags AllocationStatusFlags
        {
            get { return _allocationStatusFlags; }
            set { _allocationStatusFlags = value; }
        }
        /// <summary>
        /// Gets or sets the balance status flags for the header.
        /// </summary>
        public BalanceStatusFlags BalanceStatusFlags
        {
            get { return _balanceStatusFlags; }
            set { _balanceStatusFlags = value; }
        }
        /// <summary>
        /// Gets or sets the shipping status flags for the header.
        /// </summary>
        public ShippingStatusFlags ShippingStatusFlags
        {
            get { return _shippingStatusFlags; }
            set { _shippingStatusFlags = value; }
        }
        /// <summary>
        /// Gets or sets the allocation type flags for the header.
        /// </summary>
        public AllocationTypeFlags AllocationTypeFlags
        {
            get { return _allocationTypeFlags; }
            set { _allocationTypeFlags = value; }
        }
        /// <summary>
        /// Gets or sets the intransit status flags for the header.
        /// </summary>
        public IntransitUpdateStatusFlags IntransitUpdateStatusFlags
        {
            get { return _intransitUpdateStatusFlags; }
            set { _intransitUpdateStatusFlags = value; }
        }
        /// <summary>
        /// Gets or sets the percent need limit for the header.
        /// </summary>
        public double PercentNeedLimit
        {
            get { return _percentNeedLimit; }
            set { _percentNeedLimit = value; }
        }
        /// <summary>
        /// Gets or sets the plan percent factor for the header.
        /// </summary>
        public double PlanFactor
        {
            get { return _planFactor; }
            set { _planFactor = value; }
        }
        /// <summary>
        /// Gets or sets the reserve units for the header.
        /// </summary>
        public int ReserveUnits
        {
            get { return _reserveUnits; }
            set { _reserveUnits = value; }
        }
        /// <summary>
        /// Gets or sets the grade week count for the header.
        /// </summary>
        public int GradeWeekCount
        {
            get { return _gradeWeekCount; }
            set { _gradeWeekCount = value; }
        }
        /// <summary>
        /// Gets or sets the primary secondary record ID for the header.
        /// </summary>
        public int PrimarySecondaryRID
        {
            get { return _primarySecondaryRID; }
            set { _primarySecondaryRID = value; }
        }
        /// <summary>
        /// Gets or sets the distribution center for the header.
        /// </summary>
        public string DistributionCenter
        {
            get { return _distributionCenter; }
            set { _distributionCenter = value; }
        }
        /// <summary>
        /// Gets or sets the notes for the header.
        /// </summary>
        public string AllocationNotes
        {
            get { return _allocationNotes; }
            set { _allocationNotes = value; }
        }
        /// <summary>
        /// Gets or sets the flag identifying if the workflow should be executed for the header.
        /// </summary>
        public bool WorkflowTrigger
        {
            get { return _workflowTrigger; }
            set { _workflowTrigger = value; }
        }
        /// <summary>
        /// Gets or sets the flag identifying if the API workflow should be executed for the header at API time.
        /// </summary>
        public bool API_WorkflowTrigger
        {
            get { return _apiWorkflowTrigger; }
            set { _apiWorkflowTrigger = value; }
        }
        /// <summary>
        /// Gets or sets the earliest ship day for the header.
        /// </summary>
        public DateTime EarliestShipDay
        {
            get { return _earliestShipDay; }
            set { _earliestShipDay = value; }
        }
        /// <summary>
        /// Gets or sets the ArrayList containing the record IDs of the packs in the header.
        /// </summary>
        public ArrayList Packs
        {
            get { return _packs; }
            set { _packs = value; }
        }
        /// <summary>
        /// Gets or sets the Hashtable containing the bulk colors in the header.
        /// </summary>
        public Hashtable BulkColors
        {
            get { return _bulkColors; }
            set { _bulkColors = value; }
        }
        /// <summary>
        /// Gets or sets the ArrayList containing the record IDs of the characteristics in the header.
        /// </summary>
        public ArrayList Characteristics
        {
            get { return _characteristics; }
            set { _characteristics = value; }
        }
        // (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
        /// <summary>
        /// Gets or sets AllocatedUnits.
        /// </summary>
        public int AllocatedUnits
        {
            get
            {
                return _allocatedUnits;
            }

            set
            {
                _allocatedUnits = value;
            }
        }

        /// <summary>
        /// Gets or sets OrigAllocatedUnits.
        /// </summary>
        public int OrigAllocatedUnits
        {
            get
            {
                return _origAllocatedUnits;
            }

            set
            {
                _origAllocatedUnits = value;
            }
        }

        /// <summary>
        /// Gets or sets RsvAllocatedUnits.
        /// </summary>
        public int RsvAllocatedUnits
        {
            get
            {
                return _rsvAllocatedUnits;
            }

            set
            {
                _rsvAllocatedUnits = value;
            }
        }

        /// <summary>
        /// Gets or sets ReleaseCount.
        /// </summary>
        public int ReleaseCount
        {
            get
            {
                return _releaseCount;
            }

            set
            {
                _releaseCount = value;
            }
        }
        // (CSMITH) - END MID Track #3219

        /// <summary>
        /// Gets or sets HeaderAllocationStatus.
        /// </summary>
        public eHeaderAllocationStatus HeaderAllocationStatus
        {
            get { return _headerAllocationStatus; }
            set { _headerAllocationStatus = value; }
        }

        /// <summary>
        /// Gets or sets HeaderType.
        /// </summary>
        public eHeaderType HeaderType
        {
            get { return _headerType; }
            set { _headerType = value; }
        }

        /// <summary>
        /// Gets or sets HeaderIntransitStatus.
        /// </summary>
        public eHeaderIntransitStatus HeaderIntransitStatus
        {
            get { return _headerIntransitStatus; }
            set { _headerIntransitStatus = value; }
        }

        /// <summary>
        /// Gets or sets HeaderShipStatus.
        /// </summary>
        public eHeaderShipStatus HeaderShipStatus
        {
            get { return _headerShipStatus; }
            set { _headerShipStatus = value; }
        }

        /// <summary>
        /// Gets or sets header MasterRID.
        /// </summary>
        public int MasterRID
        {
            get { return _masterRID; }
            set { _masterRID = value; }
        }

        /// <summary>
        /// Gets or sets header MasterID.
        /// </summary>
        public string MasterID
        {
            get { return _masterID; }
            set { _masterID = value; }
        }

        /// <summary>
        /// Gets or sets header SubordinateRID.
        /// </summary>
        public int SubordinateRID
        {
            get { return _subordinateRID; }
            set { _subordinateRID = value; }
        }

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        /// <summary>
        /// Gets or sets header SubordinateRID.
        /// </summary>
        public List<int> SubordinateRIDs
        {
            get 
            {
                if (_subordinateRIDs == null)
                {
                    _subordinateRIDs = new List<int>();
                }
                return _subordinateRIDs; 
            }
            set { _subordinateRIDs = value; }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment

        /// <summary>
        /// Gets or sets header SubordinateID.
        /// </summary>
        public string SubordinateID
        {
            get { return _subordinateID; }
            set { _subordinateID = value; }
        }

        /// <summary>
        /// Gets or sets the Assortment RID.
        /// </summary>
        public int AsrtRID
        {
            get { return _asrtRID; }
            set { _asrtRID = value; }
        }

        /// <summary>
        /// Gets or sets the PlaceHolder RID.
        /// </summary>
        public int PlaceHolderRID
        {
            get { return _placeHolderRID; }
            set { _placeHolderRID = value; }
        }

        /// <summary>
        /// Gets or sets the Assortment Type
        /// </summary>
        public int AsrtType
        {
            get { return _asrtType; }
            set { _asrtType = value; }
        }
        // Begin TT#1227 - stodd - assortment
        /// <summary>
        /// Gets or sets the Assortment Placeholder Seq
        /// </summary>
        public int AsrtPlaceholderSeq
        {
            get { return _asrtPlaceholderSeq; }
            set { _asrtPlaceholderSeq = value; }
        }
        /// <summary>
        /// Gets or sets the Assortment Header Seq
        /// </summary>
        public int AsrtHeaderSeq
        {
            get { return _asrtHeaderSeq; }
            set { _asrtHeaderSeq = value; }
        }
        // End TT#1227 - stodd - assortment

        // Begin TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace
        /// <summary>
        /// Assortment Type for the assortment this header is attached to.
        /// </summary>
        public int AsrtTypeForParentAsrt
        {
            get { return _asrtTypeForParentAsrt; }
            set { _asrtTypeForParentAsrt = value; }
        }
        // End TT#893 - MD - stodd - Add "Group Allocation ID" Column to Allocation Workspace

        // begin MID Track 4448 AnF Audit Enhancement
        /// <summary>
        /// Gets the number of stores having an allocation
        /// </summary>
        public int StoresWithAllocationCount
        {
            get
            {
                return this._storesWithAllocationCount;
            }
            set
            {
                this._storesWithAllocationCount = value;
            }
        }/// <summary>
        /// Gets or sets the total store style allocation of the manually changed stores.
        /// </summary>
        public int StoreStyleManualAllocationTotal
        {
            get
            {
                return this._storeStyleManualAllocationTotal;
            }
            set
            {
                this._storeStyleManualAllocationTotal = value;
            }
        }
        /// <summary>
        /// Gets or sets the total store size allocation of the manually changed store sizes
        /// </summary>
        public int StoreSizeManualAllocationTotal
        {
            get
            {
                return this._storeSizeManualAllocationTotal;
            }
            set
            {
                this._storeSizeManualAllocationTotal = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of stores whose style/color allocation has been manually changed
        /// </summary>
        public int StoreStyleAllocationManuallyChangedCount
        {
            get
            {
                return this._storeStyleAllocationManuallyChgdCnt;
            }
            set
            {
                this._storeStyleAllocationManuallyChgdCnt = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of stores whose color-size allocation has been manually changed
        /// </summary>
        public int StoreSizeAllocationManuallyChangedCount
        {
            get
            {
                return this._storeSizeAllocationManuallyChgdCnt;
            }
            set
            {
                this._storeSizeAllocationManuallyChgdCnt = value;
            }
        }
        /// <summary>
        /// Gets or sets Horizon Override Indicator:  True: Horizon has been overridden; False: Horizon is the default.
        /// </summary>
        public bool HorizonOverride
        {
            get
            {
                return this._horizonOverride;
            }
            set
            {
                this._horizonOverride = value;
            }
        }
        // end MID Track 4448 AnF Audit Enhancement

        // Begin Track #5841 - JSmith - Performance
        /// <summary>
        /// Gets or sets the flag identifying if the components are loaded
        /// </summary>
        public bool AreComponentsLoaded
        {
            get { return _areComponentsLoaded; }
            set { _areComponentsLoaded = value; }
        }
        // End Track #5841
        // BEGIN Workspace Usability Enhancement - Ron Matelic
        /// <summary>
        /// Gets or sets Pack Count.
        /// </summary>
        public int PackCount
        {
            get { return _packCount; }
            set { _packCount = value; }
        }
        /// <summary>
        /// Gets or sets Bulk Color Count.
        /// </summary>
        public int BulkColorCount
        {
            get { return _bulkColorCount; }
            set { _bulkColorCount = value; }
        }
        /// <summary>
        /// Gets or sets Bulk Color Size Count.
        /// </summary>
        public int BulkColorSizeCount
        {
            get { return _bulkColorSizeCount; }
            set { _bulkColorSizeCount = value; }
        }




        // END Workspace Usability Enhancement  


        //Begin TT#1313-MD -jsobek -Header Filters
        public string NodeDisplayForOtsForecast
        {
            get { return _nodeDisplayForOtsForecast; }
            set { _nodeDisplayForOtsForecast = value; }
        }
        public string NodeDisplayForOnHand
        {
            get { return _nodeDisplayForOnHand; }
            set { _nodeDisplayForOnHand = value; }
        }
        public string NodeDisplayForGradeInvBasis
        {
            get { return _nodeDisplayForGradeInvBasis; }
            set { _nodeDisplayForGradeInvBasis = value; }
        }
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }
        public string APIWorkflowName
        {
            get { return _apiWorkflowName; }
            set { _apiWorkflowName = value; }
        }
        public string HeaderMethodName
        {
            get { return _headerMethodName; }
            set { _headerMethodName = value; }
        }
        //End TT#1313-MD -jsobek -Header Filters

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        /// <summary>
        /// Gets or sets Grade Attribute.
        /// </summary>
        public int GradeSG_RID
        {
            get { return _gradeSG_RID; }
            set { _gradeSG_RID = value; }
        }
        // End TT#618
        // begin TT#1287 - Inventory Min Max
        /// <summary>
        /// Gets or sets whether Grade Minimums/Maximums are Inventory or Allocation
        /// </summary>
        public bool GradeInventoryMinMax
        {
            get { return _gradeInventoryMinMax; }
            set { _gradeInventoryMinMax = value; }
        }
        /// <summary>
        /// Gets or sets the grade inventory minimum/maximum basis
        /// </summary>
        public int GradeInventoryBasisHnRID
        {
            get { return _gradeInventoryHnRID; }
            set { _gradeInventoryHnRID = value; }
        }
        // end TT#1287 - Inventory Min Max

        // Begin TT#1401 - RMatelic - Reservation Stores
        /// <summary>
        /// Gets or sets the IMO ID
        /// </summary>
        public string ImoID
        {
            get { return _imoID; }
            set { _imoID = value; }
        }

        /// <summary>
        /// Gets or sets the ItemUnitsAllocated  >>> added 'Total' prefix to match AllocationProfile
        /// </summary>
        public int ItemUnitsAllocated
        {
            get { return _itemUnitsAllocated; }
            set { _itemUnitsAllocated = value; }
        }

        /// Gets or sets the ItemUnitsAllocated  >>> added 'Total' prefix to match AllocationProfile
        /// </summary>
        public int ItemOrigUnitsAllocated
        {
            get { return _itemOrigUnitsAllocated; }
            set { _itemOrigUnitsAllocated = value; }
        }
        // End TT#1401 

        //BEGIN TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace
        /// <summary>
        /// Gets or sets the id of the Assortment where the Header is attached.
        /// </summary>
        public string AssortmentID
        {
            get { return _asrtID; }
            set { _asrtID = value; }
        }
        //END TT#403 - MD - DOConnell - Assortment ID is not displaying correctly in the Allocation Workspace

        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
        public int UnitsPerCarton
        {
            get { return _unitsPerCarton; }
            set { _unitsPerCarton = value; }
        }
        // End TT#1652-MD 

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        public bool DCFulfillmentProcessed
        {
            get { return _DCFulfillmentProcessed; }
            set { _DCFulfillmentProcessed = value; }
        }
        // End TT#1966-MD - JSmith- DC Fulfillment
    }    
    /// <summary>
	/// Contains the information about the packs of a Header
	/// </summary>
	[Serializable()]
	public class HeaderPackInfo
	{
		// Fields
		private int							_headerPackRID;
		private int							_headerRID;
		private string						_headerPackName;
		private int							_packs;
		private int							_multiple;
		private int							_reservePacks;
		private bool						_genericInd;
		private int							_associatedPackRID;
        private int                         _sequence;      // Assortment
       	private Hashtable					_colors;
        private List<int> _associatedPackRIDs = null; // TT#1966-MD - JSmith - DC Fulfillment
        private string _associatedPackName;  // TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderPackInfo()
		{
			_colors = new Hashtable();
		}
		
		// Properties

		/// <summary>
		/// Gets or sets if the record ID of the pack.
		/// </summary>
		public int HeaderPackRID 
		{
			get { return _headerPackRID ; }
			set { _headerPackRID = value; }
		}
		/// <summary>
		/// Gets or sets the record ID of the header to which the pack belongs.
		/// </summary>
		public int HeaderRID 
		{
			get { return _headerRID ; }
			set { _headerRID = value; }
		}
		/// <summary>
		/// Gets or sets the ID (name) of the pack name.
		/// </summary>
		public string HeaderPackName 
		{
			get { return _headerPackName ; }
			set { _headerPackName = value; }
		}
		/// <summary>
		/// Gets or sets the pack count.
		/// </summary>
		public int Packs 
		{
			get { return _packs ; }
			set { _packs = value; }
		}
		/// <summary>
		/// Gets or sets the pack multiple.
		/// </summary>
		public int Multiple 
		{
			get { return _multiple ; }
			set { _multiple = value; }
		}
		/// <summary>
		/// Gets or sets the number of packs to keep in reserve.
		/// </summary>
		public int ReservePacks 
		{
			get { return _reservePacks ; }
			set { _reservePacks = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the pack is a generic pack.
		/// </summary>
		public bool GenericInd 
		{
			get { return _genericInd ; }
			set { _genericInd = value; }
		}
		/// <summary>
		/// Gets or sets if the record ID of the associated pack.
		/// </summary>
		public int AssociatedPackRID 
		{
			get { return _associatedPackRID ; }
			set { _associatedPackRID = value; }
		}
        // Begin TT#1966-MD - JSmith - DC Fulfillment
        /// <summary>
        /// Gets or sets if the record ID of the associated pack.
        /// </summary>
        public List<int> AssociatedPackRIDs
        {
            get 
            {
                if (_associatedPackRIDs == null)
                {
                    _associatedPackRIDs = new List<int>();
                }
                return _associatedPackRIDs; 
            }
        }

        // Begin TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
		/// <summary>
        /// Gets or sets the ID (name) of the associated pack.
        /// </summary>
        public string AssociatedPackName
        {
            get { return _associatedPackName; }
            set { _associatedPackName = value; }
        }
		// End TT#2119-MD - JSmith - Cancelling allocation on subordinate headers on master runs forever if subordinates have same pack names with different size runs
		
        // End TT#1966-MD - JSmith - DC Fulfillment
        /// <summary>
		/// Gets or sets the colors in the pack.
		/// </summary>
		public Hashtable Colors 
		{
			get { return _colors ; }
			set { _colors = value; }
		}
        /// <summary>
        /// Gets or sets the pack sequence.
        /// </summary>
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }
	}

	/// <summary>
	/// Contains the information about the colors of a pack
	/// </summary>
	[Serializable()]
	public class HeaderPackColorInfo
	{
		// Fields
		private int							_colorCodeRID;
		private int							_units;
		private int							_sequence;
        private int                         _pcRID;         // Assortment
        private string                      _name;          // Assortment
        private string                      _description;   // Assortment
		private Hashtable					_sizes;
        private int                         _last_PCSZ_Key_Used; // Assortment
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderPackColorInfo()
		{
			_sizes = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Gets or sets if the record ID of the color.
		/// </summary>
		public int ColorCodeRID 
		{
			get { return _colorCodeRID ; }
			set { _colorCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the number of units for the color.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
		/// <summary>
		/// Gets or sets the sizes in the color.
		/// </summary>
		public Hashtable Sizes 
		{
			get { return _sizes ; }
			set { _sizes = value; }
		}
        /// <summary>
        /// Gets or sets the database RID of the pack color.
        /// </summary>
        public int HdrPCRID 
        {
            get { return _pcRID; }
            set { _pcRID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the pack color.
        /// </summary>
        public string ColorName
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets the description of the pack color.
        /// </summary>
        public string ColorDescription
        {
            get { return _description; }
            set { _description = value; }
        }


        // begin Assortment: Color/Size Change
        public int Last_PCSZ_Key_Used
        {
            get { return _last_PCSZ_Key_Used; }
            set { _last_PCSZ_Key_Used = value; }
        }
        // end Assortment: Color/Size Change
	}

	/// <summary>
	/// Contains the information about the sizes of a color in a pack
	/// </summary>
	[Serializable()]
	public class HeaderPackColorSizeInfo
	{
		// Fields
        private int                         _hdr_PCSZ_Key; // Assortment: Color/Size Change
		private int							_sizeCodeRID;
		private int							_units;
		private int							_sequence;
		
		// Properties

        // begin Assortment: Color/Size change
        /// <summary>
        /// Gets or sets if the PCSZ_Key of the size.
        /// </summary>
        public int HDR_PCSZ_Key
        {
            get { return _hdr_PCSZ_Key; }
            set { _hdr_PCSZ_Key = value; }
        }
        // end Assortment: Color/Size Change
        /// <summary>
		/// Gets or sets if the record ID of the size.
		/// </summary>
		public int SizeCodeRID 
		{
			get { return _sizeCodeRID ; }
			set { _sizeCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the number of units for the size.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the size of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
	}

	/// <summary>
	/// Contains the information about the bulk colors of a header
	/// </summary>
	[Serializable()]
	public class HeaderBulkColorInfo
	{
		// Fields
        private int                         _hdrBCRID;           // Assortment
        private int                         _colorCodeRID;
		private int							_units;
		private int							_multiple;
		private int							_minimum;
		private int							_maximum;
		private int							_reserveUnits;
		private int							_sequence;
        private string                      _name;              // Assortment
        private string                      _description;       // Assortment
        private int                         _asrtBCRID;         // Assortment   
		private Hashtable					_sizes;
        private int                         _last_BCSZ_Key_Used; // Assortment
        private ColorStatusFlags            _colorStatusFlags;   // TT#246 - MD - Jellis - AnF VSW In STore Minimums pt 5
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderBulkColorInfo()
		{
			_sizes = new Hashtable();
		}

		// Properties
        /// <summary>
        /// Gets or sets if the database record ID of the color.
        /// </summary>
        public int HdrBCRID
        {
            get { return _hdrBCRID; }
            set { _hdrBCRID = value; }
        }
		/// <summary>
		/// Gets or sets if the record ID of the color.
		/// </summary>
		public int ColorCodeRID 
		{
			get { return _colorCodeRID ; }
			set { _colorCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the number of units for the color.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the bulk multiple of the color.
		/// </summary>
		public int Multiple 
		{
			get { return _multiple ; }
			set { _multiple = value; }
		}
		/// <summary>
		/// Gets or sets the minimum value for the color.
		/// </summary>
		public int Minimum 
		{
			get { return _minimum ; }
			set { _minimum = value; }
		}
		/// <summary>
		/// Gets or sets the maximum value of the color.
		/// </summary>
		public int Maximum 
		{
			get { return _maximum ; }
			set { _maximum = value; }
		}
		/// <summary>
		/// Gets or sets the reserve units for the color.
		/// </summary>
		public int ReserveUnits 
		{
			get { return _reserveUnits ; }
			set { _reserveUnits = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
        /// <summary>
        /// Gets or sets the name of a placeholder color.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets the description of a placeholder color.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Gets or sets the corresponding placeholder RID of the color.
        /// </summary>
        public int AsrtBCRID
        {
            get { return _asrtBCRID; }
            set { _asrtBCRID = value; }
        }
		/// <summary>
		/// Gets or sets the sizes in the color.
		/// </summary>
		public Hashtable Sizes 
		{
			get { return _sizes ; }
			set { _sizes = value; }
		}

        // begin Assortment: Color/Size Change
        public int Last_BCSZ_Key_Used
        {
            get { return _last_BCSZ_Key_Used; }
            set { _last_BCSZ_Key_Used = value; }
        }
        // end Assortment: Color/Size Change
         
        // begin TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
        public ColorStatusFlags ColorStatusFlags
        {
            get { return _colorStatusFlags; }
            set { _colorStatusFlags = value; }
        }
        // end TT#246 - MD - Jellis - AnF VSW In Store Minimums pt 5
	}

	/// <summary>
	/// Contains the information about the bulk color sizes of a header
	/// </summary>
	[Serializable()]
	public class HeaderBulkColorSizeInfo
	{
		// Fields
        private int                         _hdr_BCSZ_Key; // Assortment: Color/Size Change
        private int							_sizeCodeRID;
		private int							_units;
		private int							_multiple;
		private int							_minimum;
		private int							_maximum;
		private int							_reserveUnits;
		private int							_sequence;
		
		// Properties

        // begin Assortment: Color/Size change
        /// <summary>
        /// Gets or sets if the BCSZ_Key of the size.
        /// </summary>
        public int HDR_BCSZ_KEY
        {
            get { return _hdr_BCSZ_Key; }
            set { _hdr_BCSZ_Key = value; }
        }
        // end Assortment: Color/Size change

        /// <summary>
		/// Gets or sets if the record ID of the size.
		/// </summary>
		public int SizeCodeRID 
		{
			get { return _sizeCodeRID ; }
			set { _sizeCodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the number of units for the color.
		/// </summary>
		public int Units 
		{
			get { return _units ; }
			set { _units = value; }
		}
		/// <summary>
		/// Gets or sets the bulk multiple of the color.
		/// </summary>
		public int Multiple 
		{
			get { return _multiple ; }
			set { _multiple = value; }
		}
		/// <summary>
		/// Gets or sets the minimum value for the color.
		/// </summary>
		public int Minimum 
		{
			get { return _minimum ; }
			set { _minimum = value; }
		}
		/// <summary>
		/// Gets or sets the maximum value of the color.
		/// </summary>
		public int Maximum 
		{
			get { return _maximum ; }
			set { _maximum = value; }
		}
		/// <summary>
		/// Gets or sets the reserve units for the color.
		/// </summary>
		public int ReserveUnits 
		{
			get { return _reserveUnits ; }
			set { _reserveUnits = value; }
		}
		/// <summary>
		/// Gets or sets the sequence number of the color in the pack.
		/// </summary>
		public int Sequence 
		{
			get { return _sequence ; }
			set { _sequence = value; }
		}
	}

	/// <summary>
	/// Contains the information about a header characteristic group. 
	/// </summary>
	[Serializable()]
	public class HeaderCharGroupInfo
	{
		// Fields
		private int				_RID;
		private string			_ID;
		private eHeaderCharType	_type;
		private bool			_listInd;
		private bool			_protectInd;
		private Hashtable		_characteristics;
	
		public HeaderCharGroupInfo()
		{
			_characteristics = new Hashtable();
			_RID = Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Gets or sets the record id for the header characteristic group.
		/// </summary>
		public int RID 
		{
			get { return _RID ; }
			set { _RID = value; }
		}
		/// <summary>
		/// Gets or sets the id of the header characteristic group.
		/// </summary>
		public string ID 
		{
			get { return _ID ; }
			set { _ID = value; }
		}
		/// <summary>
		/// Gets or sets the type of the header characteristic group.
		/// </summary>
		public eHeaderCharType Type 
		{
			get { return _type ; }
			set { _type = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the header characteristic group is to be listed.
		/// </summary>
		public bool ListInd 
		{
			get { return _listInd ; }
			set { _listInd = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the header characteristic group is to be protected.
		/// </summary>
		public bool ProtectInd 
		{
			get { return _protectInd ; }
			set { _protectInd = value; }
		}
		/// <summary>
		/// Gets or sets the list of header characteristics in the group.
		/// </summary>
		public Hashtable Characteristics 
		{
			get { return _characteristics ; }
			set { _characteristics = value; }
		}
	}

	/// <summary>
	/// Contains the information about a header characteristics. 
	/// </summary>
	[Serializable()]
	public class HeaderCharInfo
	{
		// Fields
		private int			_RID;
		private int			_groupRID;
		private string		_textValue;
		private DateTime	_dateValue;
		private double		_numberValue;
		private double		_dollarValue;
		
		

		// Properties

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderCharInfo()
		{
			_RID = Include.NoRID;
			_groupRID = Include.NoRID;
		}

		/// <summary>
		/// Gets or sets the header characteristic's record id.
		/// </summary>
		public int RID 
		{
			get { return _RID ; }
			set { _RID = value; }
		}
		/// <summary>
		/// Gets or sets the header characteristics group's record id.
		/// </summary>
		public int GroupRID 
		{
			get { return _groupRID ; }
			set { _groupRID = value; }
		}
		/// <summary>
		/// Gets or sets the text value for the header characteristic.
		/// </summary>
		public string TextValue 
		{
			get { return _textValue ; }
			set { _textValue = value; }
		}
		/// <summary>
		/// Gets or sets the date value for the header characteristic.
		/// </summary>
		public DateTime	DateValue 
		{
			get { return _dateValue ; }
			set { _dateValue = value; }
		}
		/// <summary>
		/// Gets or sets the number value for the header characteristic.
		/// </summary>
		public double NumberValue 
		{
			get { return _numberValue ; }
			set { _numberValue = value; }
		}
		/// <summary>
		/// Gets or sets the dollar value for the header characteristic.
		/// </summary>
		public double DollarValue 
		{
			get { return _dollarValue ; }
			set { _dollarValue = value; }
		}
	}

	//Begin TT#708 - JScott - Services need a Retry availalbe.
	//public class HeaderServerSession : Session
	public class HeaderServerSessionRemote : SessionRemote
	//End TT#708 - JScott - Services need a Retry availalbe.
	{
		private Header _headerData = null;
		private int _userID = Include.NoRID;
		private System.Collections.Hashtable _profileHash;
		private int _maxStoreRID = 0;
		
		
		/// <summary>
		/// Creates a new instance of HeaderSessionGlobal as either local or remote, depending on the value of aLocal
		/// </summary>
		/// <param name="aLocal">
		/// A boolean that indicates whether this class is being instantiated in the Client or in a remote service.
		/// </param>

		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//public HeaderServerSession(bool aLocal)
		public HeaderServerSessionRemote(bool aLocal)
		//End TT#708 - JScott - Services need a Retry availalbe.
			: base(aLocal)
		{
			_headerData = new Header();  // create data layer object for the session
			_profileHash = new System.Collections.Hashtable();
		}

		// Session Properties
		public int UserID
		{
			get
			{
				if (_userID == Include.NoRID)
				{
					_userID = SessionAddressBlock.ClientServerSession.UserRID;
				}
				return _userID;
			}
		}

		// Session Methods

		/// <summary>
		/// Initializes the session.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
            // Begin TT#340 - RMatelic - Task List not executing when the merchandise selected from the Alternate Hierarchy is a color node.
            Calendar = HeaderServerGlobal.Calendar;
            // End TT#340  
			CreateAudit();
			DateTime postingDate = SessionAddressBlock.HierarchyServerSession.GetPostingDate();
			Calendar.SetPostingDate(postingDate);
			HeaderServerGlobal.SetPostingDate(postingDate);
		}

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                HeaderServerGlobal.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

		/// <summary>
		/// Identifies resources to release as the session expires.
		/// </summary>
		protected override void ExpiredCleanup()
		{
            // Begin TT#1243 - JSmith - Audit Performance
            base.ExpiredCleanup();
            // End TT#1243

			if (_headerData != null)
			{
				if (_headerData.ConnectionIsOpen)
				{
					CloseUpdateConnection();
				}
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public override void CloseSession()
        {
            try
            {
                base.CloseSession();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flush buffer and close audit
        /// </summary>
        public override void CloseAudit()
        {
            try
            {
                base.CloseAudit();
                HeaderServerGlobal.CloseAudit();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		/// <summary>
		/// Clean up the global resources
		/// </summary>
		public void CleanUpGlobal()
		{
			HeaderServerGlobal.CleanUp();
		}

		/// <summary>
		/// Clears all cached areas in the session.
		/// </summary>
		public void Refresh()
		{
			try
			{
				if (_profileHash != null)
				{
					_profileHash.Clear();
				}
				RefreshBase();
			}
			catch ( Exception ex )
			{
                // Begin TT#189 - RMatelic - Remove excessive entries from the Audit - Add if... condition
                if (Audit != null)
                {
                    Audit.Log_Exception(ex);
                }
                // End TT#189
				throw;
			}
		}

		/// <summary>
		/// Opens an update connection for the Header data layer.
		/// </summary>
		public void OpenUpdateConnection()
		{
			_headerData.OpenUpdateConnection();
		}

		/// <summary>
		/// Commits data for the database transaction for the Header data layer.
		/// </summary>
		public void CommitData()
		{
			_headerData.CommitData();
			CloseUpdateConnection();
			OpenUpdateConnection();
		}

		/// <summary>
		/// Closes an update connection for the Header data layer.
		/// </summary>
		public void CloseUpdateConnection()
		{
			_headerData.CloseUpdateConnection();
		}

		/// <summary>
		/// Rolls back the data for the database transaction.
		/// </summary>
		public void Rollback()
		{
			_headerData.Rollback();

		}

		/// <summary>
		/// Creates and returns a new Transaction object.
		/// </summary>
		/// <returns>
		/// The newly created Transaction object that points to this Session.
		/// </returns>
		public HeaderSessionTransaction CreateTransaction()
		{
			return new HeaderSessionTransaction(SessionAddressBlock);
		}

        //Begin TT#1517-MD -jsobek -Store Service Optimization -unused function
        //public ProfileList GetProfileList(eProfileType aProfileType)
        //{
        //    ProfileList profileList;

        //    profileList = (ProfileList)_profileHash[aProfileType];

        //    if (profileList == null)
        //    {
        //        switch (aProfileType)
        //        {
        //            case eProfileType.Store:

        //                profileList = StoreMgmt.GetActiveStoresList(); //SessionAddressBlock.StoreServerSession.GetActiveStoresList();
        //                _profileHash.Add(profileList.ProfileType, profileList);
        //                _maxStoreRID = profileList.MaxValue;

        //                break;

        //            case eProfileType.StoreGroup:

        //                profileList = new ProfileList(eProfileType.StoreGroup);

        //                // TODO: Load StoreGroup Profiles with dummy values for now.  Add retrieval from StoreSession
        //                profileList.Add(new StoreGroupProfile(0));
        //                // End TODO

        //                _profileHash.Add(profileList.ProfileType, profileList);

        //                break;

        //        }
        //    }

        //    return profileList;
        //}
        //End TT#1517-MD -jsobek -Store Service Optimization -unused function

		/// <summary>
		/// Rebuilds the headers in the global area
		/// </summary>
        public void RebuildHeaderCharacteristicData(int headerFilterRID, FilterHeaderOptions headerFilterOptions) 
		{
			try
			{
                HeaderServerGlobal.RebuildHeaderCharacteristicData(headerFilterRID, headerFilterOptions); //TT#1313-MD -jsobek -Header Filters
			}
			catch
			{
				throw;
			}
		}

		public void RefreshCalendar(DateTime refreshDate)
		{
			if (refreshDate != HeaderServerGlobal.CalendarRefreshDate)
			{
				HeaderServerGlobal.Calendar.Refresh();
				HeaderServerGlobal.CalendarRefreshDate = refreshDate;
			}

			// Refresh the Calendar of THIS session
			Calendar.Refresh();
		}

		/// <summary>
		/// Updates the posting date.
		/// </summary>
		/// <param name="postingDate">The posting date</param>
		/// <remarks>Currently updates the posting date for the organizational hierarchy</remarks>
		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				Calendar.SetPostingDate(postingDate);
				HeaderServerGlobal.SetPostingDate(postingDate);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        // Begin TT#2 - Ron Matelic - Assortment Planning
        /// <summary>
        /// Requests the session get all information associated with assortment headers for which a user is authorized 
        /// to reference.
        /// </summary>
        /// <param name="userRID">The record id of the user</param>
        /// <remarks>
        /// Includes characteristics but not color and size component information
        /// </remarks>
        /// <returns>An ArrayList of AllocationHeaderProfiles</returns>
        //public ArrayList GetAssortmentHeadersForUser(int userRID)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeadersForUser(userRID, this.SessionAddressBlock, true, true, false, true, true);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#2
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

		// Begin TT#931 - MD - When deleting a Group Allocation there is no "Are you sure?" message
        /// <summary>
        /// Adjusts header array lists after an Assortment (Group Allocation) header is removed from the workspace.
        /// </summary>
        /// <param name="headerRID"></param>
        public void DeleteAssortmentHeader(int headerRID, string headerID)
        {
            try
            {
                HeaderServerGlobal.DeleteAssortmentHeader(headerRID, headerID);
            }
            catch
            {
                throw;
            }
        }
		// End TT#931 - MD - When deleting a Group Allocation there is no "Are you sure?" message

		// Begin TT#936 - MD - Prevent the saving of empty Group Allocations
        public void DeleteEmptyGroupAllocationHeader(int headerRID)
        {
            try
            {
                HeaderServerGlobal.DeleteEmptyGroupAllocationHeader(headerRID);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteEmptyGroupAllocationHeaders(List<int> asrtKeyList, SessionAddressBlock aSAB)
        {
            try
            {
                HeaderServerGlobal.DeleteEmptyGroupAllocationHeaders(asrtKeyList, aSAB);
            }
            catch
            {
                throw;
            }
        }
		// End TT#936 - MD - Prevent the saving of empty Group Allocations

		/// <summary>
		/// Requests the session get all information associated with headers for which a user is authorized 
		/// to reference.
		/// </summary>
		/// <param name="userRID">The record id of the user</param>
		/// <remarks>
		/// Includes characteristics but not color and size component information
		/// </remarks>
		/// <returns>An ArrayList of AllocationHeaderProfiles</returns>
        public ArrayList GetHeadersForWorkspace(int headerFilterRID, FilterHeaderOptions headerFilterOptions)
		{
			try
			{
                return HeaderServerGlobal.GetHeadersForWorkspace(headerFilterRID, headerFilterOptions, this.SessionAddressBlock); //TT#1313-MD -jsobek -Header Filters
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function  
        ///// <summary>
        ///// Requests the session get all information associated with headers for which a user is authorized 
        ///// to reference.
        ///// </summary>
        ///// <param name="userRID">The record id of the user</param>
        ///// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
        ///// <param name="aIncludeCharacteristics">A flag identifying if characteristics are to be included</param>
        ///// <returns>An ArrayList of AllocationHeaderProfiles</returns>
        //public ArrayList GetHeadersForUser(int userRID, int headerFilterRID, FilterHeaderOptions headerFilterOptions, bool aIncludeComponents, bool aIncludeCharacteristics)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeadersForUser(userRID, headerFilterRID, headerFilterOptions, this.SessionAddressBlock, true, true, aIncludeComponents, aIncludeCharacteristics, false); //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
      

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        /// <summary>
        /// Requests the session get all information associated with headers that have not been released.
        /// </summary>
        /// <returns>An ArrayList of AllocationHeaderProfiles</returns>
        //public ArrayList GetNonReleasedHeaders()
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetNonReleasedHeaders();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        // End TT#1065

        //Begin TT#1313-MD -jsobek -Header Filters
        // Begin TT#827 - JSmith - Allocation Performance
        /// <summary>
        /// Requests the session get all headers to be processed by the Task List.
        /// </summary>
        /// <param name="aTaskListRID">The key of the TaskList</param>
        /// <param name="aTaskSeq">The sequence of the TaskList</param>
        /// <returns>A DataTable of headers</returns>
        //public DataTable GetHeadersForTaskList(int aTaskListRID, int aTaskSeq)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeadersForTaskList(aTaskListRID, aTaskSeq);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#827 - JSmith - Allocation Performance
        //End TT#1313-MD -jsobek -Header Filters

        //Begin TT#1313-MD -jsobek -Header Filters -Unused function
        ///// <summary>
        ///// Requests the session get all information associated with a Header's definition from the 
        ///// Header global area using the Header record id.
        ///// </summary>
        ///// <param name="HeaderRID">The record id of the Header</param>
        ///// <remarks>
        ///// Includes neither characteristics nor color and size component information
        ///// </remarks>
        ///// <returns>
        ///// An instance of the AllocationHeaderProfile object contain information about the requested header
        ///// </returns>
        //public AllocationHeaderProfile GetHeaderData(int HeaderRID)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeaderData(HeaderRID, false, false);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters -Unused function

		/// <summary>
		/// Requests the session get all information associated with a Header's definition from the 
		/// Header global area using the Header record id.
		/// </summary>
		/// <param name="HeaderRID">The record id of the Header</param>
		/// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
		/// <returns>
		/// An instance of the AllocationHeaderProfile object contain information about the requested header
		/// </returns>
        //public AllocationHeaderProfile GetHeaderData(int HeaderRID, bool aIncludeComponents, bool aIncludeCharacteristics)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeaderData(HeaderRID, aIncludeComponents: aIncludeComponents, aIncludeCharacteristics: aIncludeCharacteristics, blForceGet: false);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		/// <summary>
		/// Requests the session get all information associated with a Header Characteristic groups definition 
		/// from the Header global area using the Header record id.
		/// </summary>
		/// <returns></returns>
		public HeaderCharGroupProfileList GetHeaderCharGroups()
		{
			try
			{
				return HeaderServerGlobal.GetHeaderCharGroups();
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        ///// <summary>
        ///// Requests the session get all information associated with a Header's definition from the 
        ///// Header global area using the Header record id.
        ///// </summary>
        ///// <param name="HeaderID">The id of the Header</param>
        ///// <remarks>
        ///// Includes characteristics but not color and size component information
        ///// </remarks>
        ///// <returns>
        ///// An instance of the AllocationHeaderProfile object contain information about the requested header
        ///// </returns>
        //public AllocationHeaderProfile GetHeaderData(string HeaderID)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeaderData(HeaderID, false, true);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Requests the session get all information associated with a Header's definition from the 
        ///// Header global area using the Header record id.
        ///// </summary>
        ///// <param name="HeaderID">The id of the Header</param>
        ///// <returns>
        ///// An instance of the AllocationHeaderProfile object contain information about the requested header
        ///// </returns>
        //public AllocationHeaderProfile GetHeaderData(string HeaderID, bool aIncludeComponents,
        //    bool aIncludeCharacteristics)
        //{
        //    try
        //    {
        //        return HeaderServerGlobal.GetHeaderData(HeaderID, aIncludeComponents, aIncludeCharacteristics);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        /// <summary>
        /// Requests the session get all information associated with a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="HeaderID">The id of the Header</param>
        /// <returns>
        /// An instance of the AllocationHeaderProfile object contain information about the requested header
        /// </returns>
        public AllocationHeaderProfile GetHeaderData(string HeaderID, bool aIncludeComponents, bool aIncludeCharacteristics, bool blForceGet)
        {
            try
            {
                return HeaderServerGlobal.GetHeaderData(HeaderID, aIncludeComponents, aIncludeCharacteristics, blForceGet);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the session get all information associated with a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="HeaderRID">The record id of the Header</param>
        /// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
        /// <returns>
        /// An instance of the AllocationHeaderProfile object contain information about the requested header
        /// </returns>
        public AllocationHeaderProfile GetHeaderData(int HeaderRID, bool aIncludeComponents, bool aIncludeCharacteristics, bool blForceGet)
        {
            try
            {
                return HeaderServerGlobal.GetAllocationHeaderProfile(HeaderRID, aIncludeComponents, aIncludeCharacteristics, blForceGet);
            }
            catch
            {
                throw;
            }
        }

        // End TT#1065

        // Begin TT#5697 - JSmith - Subordinate header release fails in Style Review
        /// <summary>
        /// Requests the session delete a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="aHeaderRID">The record id of the Header</param>
        public void DeleteHeader(int aHeaderRID)
        {
            try
            {
                HeaderServerGlobal.DeleteHeader(aHeaderRID);
            }
            catch
            {
                throw;
            }
        }
        // End TT#5697 - JSmith - Subordinate header release fails in Style Review

		/// <summary>
		/// Requests the session get the key for the header type in the characteristic group.
		/// </summary>
		/// <param name="aHeaderCharGroupRID">The key (RID) of the header characteristic group</param>
		/// <param name="aCharValue">The value for the characteristic</param>
		/// <returns>
		/// The key (RID) of the header characteristic if the type exists, Include.NoRID (-1) if it doesn't exist
		/// </returns>
		public int GetCharForCharGroup(int aHeaderCharGroupRID, object aCharValue)
		{
			try
			{
				return HeaderServerGlobal.GetCharForCharGroup(aHeaderCharGroupRID, aCharValue);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Add a characteristic to a group.
		/// </summary>
		/// <param name="aHeaderCharGroupRID">The key (RID) of the header characteristic group</param>
		/// <param name="aCharValue">The value for the characteristic</param>
		/// <returns>
		/// The key (RID) of the header characteristic
		/// </returns>
		public int UpdateCharInCharGroup(int aHeaderCharGroupRID, object aCharValue)
		{
			HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
			int headerCharRID = Include.NoRID;
			try
			{
				HeaderCharGroupProfile hcgp = HeaderServerGlobal.GetHeaderCharGroup(aHeaderCharGroupRID);
				if (hcgp.Key > Include.NoRID &&
					aCharValue != null)
				{
					headerCharacteristicsData.OpenUpdateConnection();
					headerCharRID = headerCharacteristicsData.HeaderCharInsert(aHeaderCharGroupRID, hcgp.Type,
						aCharValue);
					headerCharacteristicsData.CommitData();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (headerCharacteristicsData.ConnectionIsOpen)
				{
					headerCharacteristicsData.CloseUpdateConnection();
				}
			}
			HeaderServerGlobal.UpdateCharInCharGroup(aHeaderCharGroupRID, headerCharRID, aCharValue);
			return headerCharRID;
		}

		/// <summary>
		/// Refreshes the characteristic list for a header.
		/// </summary>
		/// <param name="aHeaderRID">The key (RID) of the header</param>
		/// <param name="aCharacteristics">A list of the characteristics RIDs for the header</param>
		public void RefreshHeaderCharacteristics(int aHeaderRID, ArrayList aCharacteristics)
		{
			HeaderCharacteristicsData headerCharacteristicsData = new HeaderCharacteristicsData();
			try
			{
				headerCharacteristicsData.OpenUpdateConnection();
				headerCharacteristicsData.deleteAllJoinsByHeaderID(aHeaderRID);
				foreach (int characteristicRID in aCharacteristics)
				{
					headerCharacteristicsData.HeaderJoinInsert(aHeaderRID, characteristicRID);
				}
				headerCharacteristicsData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				if (headerCharacteristicsData.ConnectionIsOpen)
				{
					headerCharacteristicsData.CloseUpdateConnection();
				}
			}
			HeaderServerGlobal.RefreshHeaderCharacteristicsOnCrossReference(aHeaderRID, aCharacteristics);
		}

 // Begin TT#2 - Ron Matelic - Assortment Planning 4.0 
        /// <summary>
        /// Gets all header rows for an ASRT_RID.
        /// </summary>
        /// <param name="aAsrtRID">The key (RID) of the assortment</param>
        /// </returns>
        /// The list of keys (HDR_RIDs) of the assortment including the assortment 
        /// </returns>
        public ArrayList GetHeadersInAssortment(int aAsrtRID)
        {
            try
            {
                return HeaderServerGlobal.GetHeadersInAssortment(aAsrtRID);
            }
            catch
            {
                throw;
            }
        }
		
		// Begin TT#1581-MD - stodd - Header Reconcile API
        public int GetNextHeaderSequenceNumber(int seqLength)
        {
            try
            {
                return HeaderServerGlobal.GetNextHeaderSequenceNumber(seqLength);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetMatchingHeader(int styleNodeRID, string distCenter, string purchaseOrder, int headerDisplayType, string colorCodeId, string bulkOrPack, string headerAction)
        {
            try
            {
                return HeaderServerGlobal.GetMatchingHeader(styleNodeRID, distCenter, purchaseOrder, headerDisplayType, colorCodeId, bulkOrPack, headerAction);
            }
            catch
            {
                throw;
            }
        }
		// End TT#1581-MD - stodd - Header Reconcile API

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return HeaderServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#195 MD
    }   // End TT#2 
	
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	public class HeaderServerSession : Session
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public HeaderServerSession(HeaderServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
			: base(aSessionRemote, eProcesses.headerService, aServiceRetryCount, aServiceRetryInterval)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        //public int UserID
        //{
        //    get
        //    {
        //        try
        //        {
        //            for (int i = 0; i < ServiceRetryCount; i++)
        //            {
        //                try
        //                {
        //                    return HeaderServerSessionRemote.UserID;
        //                }
        //                catch (Exception exc)
        //                {
        //                    if (isServiceRetryException(exc))
        //                    {
        //                        Thread.Sleep(ServiceRetryInterval);
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //            }

        //            throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

		//========
		// METHODS
		//========

		public void Initialize()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.Initialize();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CleanUpGlobal()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.CleanUpGlobal();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        public void CloseSession()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.CloseSession();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void CloseAudit()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.CloseAudit();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		public void Refresh()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.Refresh();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void OpenUpdateConnection()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.OpenUpdateConnection();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CommitData()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.CommitData();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CloseUpdateConnection()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.CloseUpdateConnection();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void Rollback()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.Rollback();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public HeaderSessionTransaction CreateTransaction()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HeaderServerSessionRemote.CreateTransaction();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1517-MD -jsobek -Store Service Optimization -unused function
        //public ProfileList GetProfileList(eProfileType aProfileType)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetProfileList(aProfileType);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1517-MD -jsobek -Store Service Optimization -unused function

        public void RebuildHeaderCharacteristicData(int headerFilterRID, FilterHeaderOptions headerFilterOptions)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        HeaderServerSessionRemote.RebuildHeaderCharacteristicData(headerFilterRID, headerFilterOptions); //TT#1313-MD -jsobek -Header Filters
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void RefreshCalendar(DateTime refreshDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.RefreshCalendar(refreshDate);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.PostingDateUpdate(postingDate);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        public ArrayList GetHeadersForWorkspace(int headerFilterRID, FilterHeaderOptions headerFilterOptions)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
                        return HeaderServerSessionRemote.GetHeadersForWorkspace(headerFilterRID, headerFilterOptions);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}


        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        //public ArrayList GetHeadersForUser(int userRID, int headerFilterRID, FilterHeaderOptions headerFilterOptions, bool aIncludeComponents, bool aIncludeCharacteristics)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetHeadersForUser(userRID, headerFilterRID, headerFilterOptions, aIncludeComponents, aIncludeCharacteristics); //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        //public ArrayList GetNonReleasedHeaders()
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetNonReleasedHeaders();
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions -unused function
        // End TT#1065

        //Begin TT#1313-MD -jsobek -Header Filters
        // Begin TT#827 - JSmith - Allocation Performance
        /// <summary>
        /// Requests the session get all headers to be processed by the Task List.
        /// </summary>
        /// <param name="aTaskListRID">The key of the TaskList</param>
        /// <param name="aTaskSeq">The sequence of the TaskList</param>
        /// <returns>A DataTable of headers</returns>
        //public DataTable GetHeadersForTaskList(int aTaskListRID, int aTaskSeq)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetHeadersForTaskList(aTaskListRID, aTaskSeq);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // End TT#827 - JSmith - Allocation Performance
        //End TT#1313-MD -jsobek -Header Filters

        //Begin TT#1313-MD -jsobek -Header Filters
		// BEGIN TT#2 - stodd -assortment
        //public ArrayList GetAssortmentHeadersForUser(int headerFilterRID, FilterHeaderOptions headerFilterOptions)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerGlobal.GetHeadersForWorkspace(headerFilterRID, headerFilterOptions, this.SessionAddressBlock, aGetInterfacedHeaders: true, aGetNonInterfacedHeaders: true, aIncludeComponents: false, aIncludeCharacteristics: true, aGetAssortmentHeaders: true); //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
		// END TT#2 - stodd -assortment
        //End TT#1313-MD -jsobek -Header Filters

        //Begin TT#1313-MD -jsobek -Header Filters -unused function
        //public AllocationHeaderProfile GetHeaderData(int HeaderRID)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetHeaderData(HeaderRID);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1313-MD -jsobek -Header Filters -unused function

        //public AllocationHeaderProfile GetHeaderData(int HeaderRID, bool aIncludeComponents, bool aIncludeCharacteristics)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetHeaderData(HeaderRID, aIncludeComponents, aIncludeCharacteristics);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		public HeaderCharGroupProfileList GetHeaderCharGroups()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HeaderServerSessionRemote.GetHeaderCharGroups();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions - unused function
        //public AllocationHeaderProfile GetHeaderData(string HeaderID)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetHeaderData(HeaderID);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public AllocationHeaderProfile GetHeaderData(string HeaderID, bool aIncludeComponents, bool aIncludeCharacteristics)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return HeaderServerSessionRemote.GetHeaderData(HeaderID, aIncludeComponents, aIncludeCharacteristics);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions - unused function

        // Begin TT#1065 - JSmith - Style 12340 Color WSN was allocated with incorrect workflow
        /// <summary>
        /// Requests the session get all information associated with a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="HeaderID">The id of the Header</param>
        /// <param name="blForceGet">Requests the header to be read from the database if not found in cache</param>
        /// <returns>
        /// An instance of the AllocationHeaderProfile object contain information about the requested header
        /// </returns>
        /// 
        public AllocationHeaderProfile GetHeaderData(string HeaderID, bool aIncludeComponents, bool aIncludeCharacteristics, bool blForceGet)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HeaderServerSessionRemote.GetHeaderData(HeaderID, aIncludeComponents, aIncludeCharacteristics, blForceGet);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        

        /// <summary>
        /// Requests the session get all information associated with a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="HeaderRID">The record id of the Header</param>
        /// <param name="aIncludeComponents">A flag identifying if color and pack information is to be included</param>
        /// <param name="blForceGet">Requests the header to be read from the database if not found in cache</param>
        /// <returns>
        /// An instance of the AllocationHeaderProfile object contain information about the requested header
        /// </returns>
        /// 
        public AllocationHeaderProfile GetHeaderData(int HeaderRID, bool aIncludeComponents, bool aIncludeCharacteristics, bool blForceGet)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HeaderServerSessionRemote.GetHeaderData(HeaderRID, aIncludeComponents, aIncludeCharacteristics, blForceGet);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1065

        // Begin TT#5697 - JSmith - Subordinate header release fails in Style Review
        /// <summary>
        /// Requests the session delete a Header's definition from the 
        /// Header global area using the Header record id.
        /// </summary>
        /// <param name="aHeaderRID">The record id of the Header</param>
        public void DeleteHeader(int aHeaderRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.DeleteHeader(aHeaderRID);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#5697 - JSmith - Subordinate header release fails in Style Review

		public int GetCharForCharGroup(int aHeaderCharGroupRID, object aCharValue)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HeaderServerSessionRemote.GetCharForCharGroup(aHeaderCharGroupRID, aCharValue);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#931 - MD - When deleting a Group Allocation there is no "Are you sure?" message
        /// <summary>
        /// Adjusts header array lists after an Assortment (Group Allocation) header is removed from the workspace.
        /// </summary>
        /// <param name="headerRID"></param>
        public void DeleteAssortmentHeader(int headerRID, string headerID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.DeleteAssortmentHeader(headerRID, headerID);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
		// End TT#931 - MD - When deleting a Group Allocation there is no "Are you sure?" message

		// Begin TT#936 - MD - Prevent the saving of empty Group Allocations
        /// <summary>
        /// Used to delete "empty" Group Allocation Headers.
        /// </summary>
        /// <param name="headerRID"></param>
        public void DeleteEmptyGroupAllocationtHeader(int headerRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.DeleteEmptyGroupAllocationHeader(headerRID);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void DeleteEmptyGroupAllocationHeaders(List<int> asrtKeyList, SessionAddressBlock aSAB)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.DeleteEmptyGroupAllocationHeaders(asrtKeyList, aSAB);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
		// End TT#936 - MD - Prevent the saving of empty Group Allocations

		public int UpdateCharInCharGroup(int aHeaderCharGroupRID, object aCharValue)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HeaderServerSessionRemote.UpdateCharInCharGroup(aHeaderCharGroupRID, aCharValue);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void RefreshHeaderCharacteristics(int aHeaderRID, ArrayList aCharacteristics)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						HeaderServerSessionRemote.RefreshHeaderCharacteristics(aHeaderRID, aCharacteristics);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#2 - stodd - assoretment
		public ArrayList GetHeadersInAssortment(int aAsrtRID)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return HeaderServerSessionRemote.GetHeadersInAssortment(aAsrtRID);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}
		// END TT#2 - stodd - assoretment

		// Begin TT#1581-MD - stodd - Header Reconcile API
        public int GetNextHeaderSequenceNumber(int seqLength)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HeaderServerSessionRemote.GetNextHeaderSequenceNumber(seqLength);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetMatchingHeader(int styleNodeRID, string distCenter, string purchaseOrder, int headerDisplayType, string colorCodeId, string bulkOrPack, string headerAction)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HeaderServerSessionRemote.GetMatchingHeader(styleNodeRID, distCenter, purchaseOrder, headerDisplayType, colorCodeId, bulkOrPack, headerAction);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
		// Begin TT#1581-MD - stodd - Header Reconcile API

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return HeaderServerSessionRemote.GetServiceProfile();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        HeaderServerSessionRemote.VerifyEnvironment(aClientProfile);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

	}
	//End TT#708 - JScott - Services need a Retry availalbe.
}
