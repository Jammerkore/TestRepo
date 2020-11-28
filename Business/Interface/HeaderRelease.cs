using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
// (CSMITH) - BEG MID
//
// Module reorganized during size logic modifications
//
// (CSMITH) - END MID
	/// <summary>
	/// Summary description for HeaderRelease.
	/// </summary>
	public class HeaderRelease
	{
//		private string sourceModule = "HeaderRelease.cs";

		//=======
		// Fields
		//=======
		private int multiple;
		private int qtyAllocated;
		private int unitsInLevel;
		private int qtyPacksUnits;

		private double unitCostRetail;

		private string comment = null;
		private string msgText = null;
		private string tmpString = null;
		private string hdrReleaseFile = null;


		private bool alreadyReleased = false;

		private AllocationProfile ap = null;

		private AllocationProfileList apl = null;

		private ApplicationSessionTransaction ast = null;

        //Begin TT#1510 - JSmith - Add size group to release output
        SizeGroup sizeGroupData;
        //End TT#1510

		public HeaderRelease()
		{
		}

		//=====================================
		// Process a List of Allocation Headers
		//=====================================
		public bool ReleaseHeader(AllocationProfileList _aProfileList, ApplicationSessionTransaction _aAppSessTrans)
		{
            //bool deleteOkay = true;
			bool releaseOkay = true;

			// ================
			// Begin processing
			// ================
			apl = _aProfileList;

			ast = _aAppSessTrans;

            //Begin TT#1510 - JSmith - Add size group to release output
            sizeGroupData = new SizeGroup();
            //End TT#1510

			foreach (AllocationProfile ap in apl)
			{
				if (!ReleaseHeader(ap, ast))
					releaseOkay = false;
			}

			return releaseOkay;
		}

		//===================================
		// Process a Single Allocation Header
		//===================================
		public bool ReleaseHeader(AllocationProfile _aProfile, ApplicationSessionTransaction _aAppSessTrans)
		{
			bool deleteOkay = true;
			bool releaseOkay = true;

			// ================
			// Begin processing
			// ================
			ap = _aProfile;

			ast = _aAppSessTrans;

            //Begin TT#1510 - JSmith - Add size group to release output
            sizeGroupData = new SizeGroup();
            //End TT#1510

			if (ap.ReleaseApproved &&
				ast.SAB.ClientServerSession.GlobalOptions.IsReleaseable(ap.HeaderType))
			{
				try
				{
					hdrReleaseFile = ast.ReleaseFileLocation(ap);

					releaseOkay = WriteXMLFile(ap);

					if (!releaseOkay)
					{
						deleteOkay = ast.DeleteReleaseFile(ap);
					}
				}

				catch (Exception Ex)
				{
					releaseOkay = false;

					if (Ex.GetType() != typeof(MIDException))
					{
						msgText = Ex.ToString() + System.Environment.NewLine;
					}
					else
					{
						MIDException MIDEx = (MIDException)Ex;

						msgText = MIDEx.ErrorMessage + System.Environment.NewLine;
					}

					ast.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, GetType().Name);

					msgText = "Allocation Header " + ap.HeaderID + " NOT Released" + System.Environment.NewLine;
				}
			}
			else
			{
				releaseOkay = false;

				msgText = "Allocation Header " + ap.HeaderID + " has NOT been Release Approved" + System.Environment.NewLine;
			}
			

			if (releaseOkay)
			{
				ast.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                //ast.SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", ast.SAB.GetHighestAuditMessageLevel());
                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
			}
			else
			{
				ast.SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);

                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                //ast.SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", ast.SAB.GetHighestAuditMessageLevel());
                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
			}

			return releaseOkay;
		}

		//=================================================
		// Generate the Released Allocation Header XML File
		//=================================================
		public bool WriteXMLFile(AllocationProfile ap)
		{
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement

			string userName = null;
			string masterID = null;
			// begin MID Track 4029 ReWork MasterPO Logic
            int userRID = Include.UndefinedUserRID;
//			string masterPack = null;
//			string subordPack = null;
//
//			bool subordStatus = false;
//			bool procMasterPack = false;
//			bool procSubordPack = false;
//			bool masterReleased = false;
//			bool masterIntransit = false;
//
//			// begin MID Track 3548 Release Date of PO is changing
//			DateTime masterReleaseApprovedDate = Include.UndefinedDate;
//			DateTime masterReleaseDate = Include.UndefinedDate;
//            // end MID Track 3548 Release Date of PO is changing
//			int tmpUnits;
//			int tmpPacks; // MID Track 3543 Master Allocation changes on subordinate removal 
//			int masterPackMult;
//			int subordPackMult;
//			int masterPacksAloctd;
//			int masterUnitsAloctd;
//			int subordPacksAloctd;
//			int subordUnitsAloctd;
//			int masterPacksToAloct;
//			int masterUnitsToAloct;
//			int subordPacksToAloct;
//			int subordUnitsToAloct;
//			int masterRsvPacksAloctd;
//			int masterRsvUnitsAloctd;
//			int masterStrPacksAloctd;
//			int masterStrUnitsAloctd;
//			int subordRsvPacksAloctd;
//			int subordRsvUnitsAloctd;
//			int subordStrPacksAloctd;
//			int subordStrUnitsAloctd;
//			int masterTotUnitsToAloct;
//			int masterRID = Include.NoRID;
//			int subordRID = Include.NoRID;
//			int masterPackRID = Include.NoRID;
//			int	masterColrRID = Include.NoRID;
//			int subordPackRID = Include.NoRID;
//			int	subordColrRID = Include.NoRID;
//			int userRID = Include.UndefinedUserRID;
//
//			eComponentType masterCompType;
//			eComponentType subordCompType;
//
//			AllocationProfile map = null;
//
//			GeneralComponent totalComp = null;
			// end MID Track 4029 ReWork MasterPO Logic
// (CSMITH) - END MID Track #3219
			EditMsgs em = null;

			bool releaseOkay = true;

			XmlTextWriter xmlWriter = null;

			HierarchyNodeProfile chnp = null;					// Color Hierarchy Node Profile
			HierarchyNodeProfile phnp = null;					// Parent Hierarchy Node Profile
			HierarchyNodeProfile shnp = null;					// Style Hierarchy Node Profile
			HierarchyNodeProfile zhnp = null;					// Size Hierarchy Node Profile
//Begin Track #4302 - JScott - Size Codes in wrong order afer release
			SortedList packColorList;
			SortedList packColorSizeList;
			SortedList bulkColorList;
			SortedList bulkColorSizeList;
//End Track #4302 - JScott - Size Codes in wrong order afer release
			//Begin TT#1234 - JScott - Add trigger file for release file
			string releaseFileTriggerExtension;
			StreamWriter trgFile;
			//End TT#1234 - JScott - Add trigger file for release file

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();
            bool writeHeaderSuccess = false; // TT#2671 - Jellis - Release Action Fails yet released ?
			try
			{
				comment = null;

				Index_RID reserveStoreIndexRID = ast.ReserveStore;

				ProfileList spl = ast.GetMasterProfileList(eProfileType.Store);

				if (spl.Count == 0)
				{
					releaseOkay = false;

					msgText = "No stores are currently defined" + System.Environment.NewLine;

					em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
				}

				if (releaseOkay)
				{

					shnp = GetStyleNodeProfile(ap.StyleHnRID, ref em);

					if (shnp.Key == Include.NoRID)
					{
						releaseOkay = false;

						msgText = "Style NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

						em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
					}
					else
					{
						phnp = GetStyleParentNodeProfile(shnp.HomeHierarchyParentRID, ref em);

						if (phnp.Key == Include.NoRID)
						{
							releaseOkay = false;

							msgText = "Parent of style NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

							em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
						}
					}
				}
				// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				// begin MID Track 4029 ReWork MasterPO Logic
				//				// ==================================
				//				// Set Subordinate RID for future use
				//				// ==================================
				//				subordRID = ap.HeaderRID;
				// end MID track 4029 ReWork MasterPO Logic

				// ===================================
				// Retrieve the User RID and User Name
				// ===================================
				userRID = ast.SAB.ClientServerSession.UserRID;
				userName = ast.SAB.ClientServerSession.GetUserName(userRID);
				// (CSMITH) - END MID Track #3219
				if (releaseOkay)
				{
					// =================================
					// Create a writer and open the file
					// =================================
					xmlWriter = new XmlTextWriter(hdrReleaseFile, System.Text.Encoding.UTF8);

					xmlWriter.Formatting = Formatting.Indented;

					xmlWriter.Indentation = 4;

					// =================
					// Open the document
					// =================
					
					xmlWriter.WriteStartDocument();
					// begin MID Track 4029 ReWork MasterPO Logic
					if (ap.MasterRID != Include.NoRID)
					{
						masterID = ap.MasterID;
					}
					// end MID Track 4029 ReWork MasterPO Logic
					if (!ap.Released)
					{
						ap.SetReleased(true);
						alreadyReleased = false; // MID Track 4024 Release Approved headers cannot be released.
					}
					else
					{
						alreadyReleased = true;
					}

					// ===============
					// Write a comment
					// ===============
					//					tmpString = ap.ReleaseDate.ToString("U");
					tmpString = ap.ReleaseDate.ToString("dddd, MMMM dd, yyyy HH:mm:ss tt", CultureInfo.CurrentCulture);
					comment = "Allocation Header " + ap.HeaderID + " Released On " + tmpString;
					xmlWriter.WriteComment(comment);

					// ======================
					// Write the Root element
					// ======================
					xmlWriter.WriteStartElement("Releases", "http://tempuri.org/HeaderReleaseSchema.xsd");

					// =========================
					// Write the Release element
					// =========================
					xmlWriter.WriteStartElement("Release");

					// ============================
					// Write the HeaderId attribute
					// ============================
					xmlWriter.WriteStartAttribute(null, "HeaderId", null);
					xmlWriter.WriteString(ap.HeaderID);
					xmlWriter.WriteEndAttribute();

					// =====================================
					// Write the HeaderDescription attribute
					// =====================================
					xmlWriter.WriteStartAttribute(null, "HeaderDescription", null);
					xmlWriter.WriteString(ap.HeaderDescription);
					xmlWriter.WriteEndAttribute();

					// ==============================
					// Write the HeaderDate attribute
					// ==============================
					xmlWriter.WriteStartAttribute(null, "HeaderDate", null);
					tmpString = ap.HeaderDay.ToString("yyyy-MM-dd");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// ===============================
					// Write the ReleaseDate attribute
					// ===============================
					xmlWriter.WriteStartAttribute(null, "ReleaseDate", null);
					tmpString = ap.ReleaseDate.ToString("yyyy-MM-dd");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					// ===================
					// Write the User Name
					// ===================
					xmlWriter.WriteStartAttribute(null, "UserName", null);
					xmlWriter.WriteString(userName);
					xmlWriter.WriteEndAttribute();
					// (CSMITH) - END MID Track #3219
					// ==============================
					// Write the UnitRetail attribute
					// ==============================
					xmlWriter.WriteStartAttribute(null, "UnitRetail", null);
					unitCostRetail = ap.UnitRetail;
					tmpString = unitCostRetail.ToString("0.00");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// ============================
					// Write the UnitCost attribute
					// ============================
					xmlWriter.WriteStartAttribute(null, "UnitCost", null);
					unitCostRetail = ap.UnitCost;
					tmpString = unitCostRetail.ToString("0.00");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// ======================================
					// Write the TotalReceivedUnits attribute
					// ======================================
					xmlWriter.WriteStartAttribute(null, "TotalReceivedUnits", null);
					qtyPacksUnits = ap.TotalUnitsToAllocate;
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// BEGIN TT#1401 - stodd - add VSW (IMO)
					// =======================================
					// Write the TotalVSWAllocatedUnits attribute
					// =======================================
					xmlWriter.WriteStartAttribute(null, "TotalVSWAllocatedUnits", null);
					qtyPacksUnits = ap.TotalImoUnitsAllocated;
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();
					// END TT#1401 - stodd - add VSW (IMO)

					// =======================================
					// Write the TotalAllocatedUnits attribute
					// =======================================
					xmlWriter.WriteStartAttribute(null, "TotalAllocatedUnits", null);
					qtyPacksUnits = ap.TotalUnitsAllocated;
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// =====================================
					// Write the TotalReserveUnits attribute
					// =====================================
					xmlWriter.WriteStartAttribute(null, "TotalReserveUnits", null);
					qtyPacksUnits = 0;
					// (CSMITH) - BEG MID Track #3535: TootalReserveUnits = 0 when reserve store has units
					//					if (reserveStoreIndexRID.RID == Include.UndefinedStoreRID)
					if (reserveStoreIndexRID.RID != Include.UndefinedStoreRID)
						// (CSMITH) - END MID Track #3535
					{
						qtyPacksUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStoreIndexRID);
					}
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// ===========================
					// Write the StyleId attribute
					// ===========================
					xmlWriter.WriteStartAttribute(null, "StyleId", null);
					xmlWriter.WriteString(shnp.NodeID);
					xmlWriter.WriteEndAttribute();

					// ===================================
					// Write the ParentOfStyleId attribute
					// ===================================
					xmlWriter.WriteStartAttribute(null, "ParentOfStyleId", null);
					xmlWriter.WriteString(phnp.NodeID);
					xmlWriter.WriteEndAttribute();

                    //Begin TT#1510 - JSmith - Add size group to release output
                    // ===================================
                    // Write the SizeGroupName attribute
                    // ===================================
                    if (ap.SizeGroupRID != Include.UndefinedSizeGroupRID)
                    {
                        xmlWriter.WriteStartAttribute(null, "SizeGroupName", null);
                        xmlWriter.WriteString(sizeGroupData.GetSizeGroupName(ap.SizeGroupRID));
                        xmlWriter.WriteEndAttribute();
                    }

                    // ===================================
                    // Write the BulkMultiple attribute
                    // ===================================
                    xmlWriter.WriteStartAttribute(null, "BulkMultiple", null);
                    xmlWriter.WriteString(ap.BulkMultiple.ToString());
                    xmlWriter.WriteEndAttribute();
                    //End TT#1510

					// ==============================
					// Write the HeaderType attribute
					// ==============================
					xmlWriter.WriteStartAttribute(null, "HeaderType", null);
					if (ap.Receipt)
					{
						xmlWriter.WriteString("Receipt");
					}
					else if (ap.IsPurchaseOrder)
					{
						xmlWriter.WriteString("PO");
					}
					else if (ap.ASN)
					{
						xmlWriter.WriteString("ASN");
					}
					else if (ap.IsDummy)
					{
						xmlWriter.WriteString("Dummy");
					}
					else if (ap.DropShip)
					{
						xmlWriter.WriteString("DropShip");
					}
					else if (ap.Reserve)
					{
						xmlWriter.WriteString("Reserve");
					}
					// BEGIN TT#1401 - stodd - add resevation stores (IMO)
					else if (ap.IMO)
					{
						xmlWriter.WriteString("VSW");
					}
					// END TT#1401 - stodd - add resevation stores (IMO)
					else
					{
						xmlWriter.WriteString("WorkupTotalBuy");
					}
					xmlWriter.WriteEndAttribute();

					// ==============================
					// Write the DistCenter attribute
					// ==============================
					xmlWriter.WriteStartAttribute(null, "DistCenter", null);
					xmlWriter.WriteString(ap.DistributionCenter);
					xmlWriter.WriteEndAttribute();

					// ==========================
					// Write the Vendor attribute
					// ==========================
					xmlWriter.WriteStartAttribute(null, "Vendor", null);
					xmlWriter.WriteString(ap.Vendor);
					xmlWriter.WriteEndAttribute();

					// =================================
					// Write the PurchaseOrder attribute
					// =================================
					xmlWriter.WriteStartAttribute(null, "PurchaseOrder", null);
					xmlWriter.WriteString(ap.PurchaseOrder);
					xmlWriter.WriteEndAttribute();

					// BEGIN TT#1401 - stodd - add resevation stores (IMO)
					// =================================
					// Write the VSWID attribute
					// =================================
					xmlWriter.WriteStartAttribute(null, "VSWID", null);
					xmlWriter.WriteString(ap.ImoID);
					xmlWriter.WriteEndAttribute();
					// END TT#1401 - stodd - add resevation stores (IMO)

					// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
					// =================================
					// Write the AdjustVSW attribute
					// =================================
                    // Begin TT#3022 - JSmith - Header Release file contains wrong tag for VSWProcess
                    //xmlWriter.WriteStartAttribute(null, "AdjustVSW", null);
                    xmlWriter.WriteStartAttribute(null, "VSWProcess", null);
                    // End TT#3022 - JSmith - Header Release file contains wrong tag for VSWProcess
					if (ap.AdjustVSW_OnHand)
					{
						xmlWriter.WriteString("Adjust");
					}
					else
					{
						if (ap.IMO)
						{
							xmlWriter.WriteString("Replace");
						}
						else
						{
							xmlWriter.WriteString("");
						}
					}
					xmlWriter.WriteEndAttribute();
					// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

                    // Begin TT#1652-MD - stodd - DC Carton Rounding
                    // =================================
                    // Write the Units Per Carton attribute
                    // =================================
                    xmlWriter.WriteStartAttribute(null, "UnitsPerCarton", null);
                    xmlWriter.WriteString(ap.UnitsPerCarton.ToString());
                    xmlWriter.WriteEndAttribute();
                    // Begin TT#1652-MD - stodd - DC Carton Rounding


					// =========================
					// Write the Notes attribute
					// =========================
					xmlWriter.WriteStartAttribute(null, "Notes", null);
					xmlWriter.WriteString(ap.AllocationNotes);
					xmlWriter.WriteEndAttribute();

					// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					// ================
					// Write the Master
					// ================
					// begin MID Track 4029 Re-Work Master-PO Logic
					//				header = new Header();
					//				xmlWriter.WriteStartAttribute(null, "Master", null);
					//				masterRID = header.GetMasterRID(subordRID);
					//				if (masterRID != Include.NoRID)
					//				{
					//					masterID = header.GetMasterID(masterRID);
					//				}
					xmlWriter.WriteStartAttribute(null, "Master", null);
					//                masterRID = ap.MasterRID;
					//				if (masterRID != Include.NoRID)
					//				{
					//					masterID = ap.MasterID;
					//				}
					// end MID Track 4029 Re-Work Master-PO Logic
					xmlWriter.WriteString(masterID);
					xmlWriter.WriteEndAttribute();

					// =========================
					// Write the Allocated Units
					// =========================
					xmlWriter.WriteStartAttribute(null, "AllocatedUnits", null);
                    // BEGIN TT#678 - AGallagher - Alloc Workspace - Allocated Units, Original Allocated Units, and Reserve Allocated Units displaying "0" for allocated headers
                    // process is now done in the Business AllocationProfile WriteHeaderData 
                    //ap.AllocatedUnits = 0;
                    //foreach(StoreProfile sp in spl)
                    //{
                    //    Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

                    //    if (storeIndexRID.RID != reserveStoreIndexRID.RID)
                    //    {
                    //        ap.AllocatedUnits += ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID.RID);
                    //    }
                    //}
                    // END TT#678 - AGallagher - Alloc Workspace - Allocated Units, Original Allocated Units, and Reserve Allocated Units displaying "0" for allocated headers
                    // begin MID Track 4029 Re-Work Master PO Logic
                    if (ap.ReleaseCount < 1)
                    {
                        ap.OrigAllocatedUnits = ap.AllocatedUnits;
                    }
                    // end MID Track 4029 Re-Work Master PO Logic
                    qtyPacksUnits = ap.AllocatedUnits;
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
					// =========================
					// Write the VSW Allocated Units
					// =========================
					xmlWriter.WriteStartAttribute(null, "VSWAllocatedUnits", null);
					//if (ap.ReleaseCount < 1)
					//{
					//    ap.OrigAllocatedUnits = ap.AllocatedUnits;
					//}
					// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
					//qtyPacksUnits = ap.ImoAllocatedUnits;
					qtyPacksUnits = ap.TotalImoUnitsAllocated;
					// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// ==================================
					// Write the Original Allocated Units
					// ==================================
					xmlWriter.WriteStartAttribute(null, "OrigAllocatedUnits", null);
					// begin MID Track 4029 Re-work Master-PO Logic
					//if (ap.ReleaseCount < 1)
					//{
					//ap.OrigAllocatedUnits = 0;
					//
					//foreach(StoreProfile sp in spl)
					//{
					//	Index_RID storeIndexRID = ap.StoreIndex(sp.Key);
					//
					//	if (storeIndexRID.RID != reserveStoreIndexRID.RID)
					//	{
					//		ap.OrigAllocatedUnits += ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID.RID);
					//	}
					//}
					//}
					qtyPacksUnits = ap.OrigAllocatedUnits;
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();

					// =======================
					// Write the Reserve Units
					// =======================
					xmlWriter.WriteStartAttribute(null, "ReserveUnits", null);
                    // BEGIN TT#678 - AGallagher - Alloc Workspace - Allocated Units, Original Allocated Units, and Reserve Allocated Units displaying "0" for allocated headers
                    //if (reserveStoreIndexRID.RID == Include.UndefinedStoreRID)
                    //{
                    //    ap.RsvAllocatedUnits = 0;
                    //}
                    //else
                    //{
                    //    ap.RsvAllocatedUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStoreIndexRID);
                    //}
                    // END TT#678 - AGallagher - Alloc Workspace - Allocated Units, Original Allocated Units, and Reserve Allocated Units displaying "0" for allocated headers
					qtyPacksUnits = ap.RsvAllocatedUnits;
					tmpString = qtyPacksUnits.ToString("0");
					xmlWriter.WriteString(tmpString);
					xmlWriter.WriteEndAttribute();
					// (CSMITH) - END MID Track #3219
					// =========================
					// Write the Characteristics
					// =========================
					foreach (CharacteristicsBin cb in ap.Characteristics.Values)
					{
						// ===============================
						// Open the Characteristic element
						// ===============================
						xmlWriter.WriteStartElement("Characteristic");

						// ========================
						// Write the Name attribute
						// ========================
						xmlWriter.WriteStartAttribute(null, "Name", null);
						xmlWriter.WriteString(cb.Name);
						xmlWriter.WriteEndAttribute();

						// =========================
						// Write the Value attribute
						// =========================
						switch (cb.DataType)
						{
							case eStoreCharType.date:
								xmlWriter.WriteStartAttribute(null, "Value", null);
								if (cb.Value == Include.UndefinedDate.ToString() || cb.Value.Trim() == string.Empty)
								{
									xmlWriter.WriteString("");
								}
								else
								{
									xmlWriter.WriteString((Convert.ToDateTime(cb.Value)).ToString("yyyy-MM-dd"));
								}
								xmlWriter.WriteEndAttribute();
								break;

							default:
								xmlWriter.WriteStartAttribute(null, "Value", null);
								xmlWriter.WriteString(cb.Value);
								xmlWriter.WriteEndAttribute();
								break;
						}

						// ================================
						// Close the Characteristic element
						// ================================
						xmlWriter.WriteEndElement();
					}

					// ===============
					// Write the Packs
					// ===============
					foreach (PackHdr pack in ap.Packs.Values)
					{
						// =====================
						// Open the Pack element
						// =====================
						xmlWriter.WriteStartElement("Pack");

						// ========================
						// Write the Name attribute
						// ========================
						xmlWriter.WriteStartAttribute(null, "Name", null);
						xmlWriter.WriteString(pack.PackName);
						xmlWriter.WriteEndAttribute();

						// ============================
						// Write the Multiple attribute
						// ============================
						xmlWriter.WriteStartAttribute(null, "Multiple", null);
						multiple = pack.PackMultiple;
						tmpString = multiple.ToString("0");
						xmlWriter.WriteString(tmpString);
						xmlWriter.WriteEndAttribute();

                        //Begin TT#1510 - JSmith - Add size group to release output
                        // ============================
                        // Write the IsGeneric attribute
                        // ============================
                        xmlWriter.WriteStartAttribute(null, "IsGeneric", null);
                        xmlWriter.WriteString(pack.GenericPack.ToString().ToLower());
                        xmlWriter.WriteEndAttribute();
                        //End TT#1510

						// =====================================
						// Write the PackReceivedPacks attribute
						// =====================================
						xmlWriter.WriteStartAttribute(null, "PackReceivedPacks", null);
						qtyPacksUnits = pack.PacksToAllocate;
						tmpString = qtyPacksUnits.ToString("0");
						xmlWriter.WriteString(tmpString);
						xmlWriter.WriteEndAttribute();

						// ======================================
						// Write the PackAllocatedPacks attribute
						// ======================================
						xmlWriter.WriteStartAttribute(null, "PackAllocatedPacks", null);
						qtyPacksUnits = pack.PacksAllocated;
						tmpString = qtyPacksUnits.ToString("0");
						xmlWriter.WriteString(tmpString);
						xmlWriter.WriteEndAttribute();

						// ====================================
						// Write the PackReservePacks attribute
						// ====================================
						xmlWriter.WriteStartAttribute(null, "PackReservePacks", null);
						qtyPacksUnits = 0;
						if (reserveStoreIndexRID.RID != Include.UndefinedStoreRID)
						{
							qtyPacksUnits = ap.GetStoreQtyAllocated(pack.PackName, reserveStoreIndexRID);
						}
						tmpString = qtyPacksUnits.ToString("0");
						xmlWriter.WriteString(tmpString);
						xmlWriter.WriteEndAttribute();

						// =====================
						// Write the Pack Stores
						// =====================
						foreach(StoreProfile sp in spl)
						{
							qtyPacksUnits = 0;

							Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

							if (storeIndexRID.RID != reserveStoreIndexRID.RID)
							{
								// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
								// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
								qtyPacksUnits = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID)
									+ ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
								// END TT#2347 - stodd - Release files not showing stores with only VSW values
								// END TT#1401 - stodd - add VSWVSW (IMO)

								if (qtyPacksUnits > 0)
								{
									// ==========================
									// Open the PackStore element
									// ==========================
									xmlWriter.WriteStartElement("PackStore");

									// ===========================
									// Write the StoreID attribute
									// ===========================
									xmlWriter.WriteStartAttribute(null, "StoreID", null);
									xmlWriter.WriteString(sp.StoreId);
									xmlWriter.WriteEndAttribute();

									// =========================
									// Write the Packs attribute
									// =========================
									xmlWriter.WriteStartAttribute(null, "Packs", null);
									// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
									qtyPacksUnits = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID);
									// END TT#2347 - stodd - Release files not showing stores with only VSW values
									tmpString = qtyPacksUnits.ToString("0");
									xmlWriter.WriteString(tmpString);
									xmlWriter.WriteEndAttribute();

									// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
									// ===========================
									// Write the VSWID attribute
									// ===========================
									xmlWriter.WriteStartAttribute(null, "VSWID", null);
									xmlWriter.WriteString(sp.IMO_ID);
									xmlWriter.WriteEndAttribute();

									// =========================
									// Write the VSWPacks attribute
									// =========================
									xmlWriter.WriteStartAttribute(null, "VSWPacks", null);
									// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
									qtyPacksUnits = ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
									// END TT#2347 - stodd - Release files not showing stores with only VSW values
									tmpString = qtyPacksUnits.ToString("0");
									xmlWriter.WriteString(tmpString);
									xmlWriter.WriteEndAttribute();
									// BEGIN TT#1401 - stodd - add VSWVSW (IMO)

									// ===========================
									// Close the PackStore element
									// ===========================
									xmlWriter.WriteEndElement();
								}
							}
						}

						// =====================
						// Write the Pack Colors
						// =====================
						//Begin Track #4302 - JScott - Size Codes in wrong order afer release
						//						foreach (PackColorSize packColor in pack.PackColors.Values)
						//						{
						packColorList = new SortedList();

						foreach (PackColorSize packColor in pack.PackColors.Values)
						{
							packColorList.Add(packColor, packColor);
						}

						foreach (PackColorSize packColor in packColorList.Values)
						{
							//End Track #4302 - JScott - Size Codes in wrong order afer release
							if (packColor.ColorCodeRID != Include.DummyColorRID)
							{
								ColorCodeProfile ccp = GetColorCodeProfile(packColor.ColorCodeRID, ref em);

								if (ccp.Key == Include.NoRID)
								{
									releaseOkay = false;

									msgText = "Color " + ccp.ColorCodeID + " NOT defined on the color table" + System.Environment.NewLine;

									em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
								}
								else
								{
									chnp = GetColorNodeProfile(shnp.HierarchyRID, shnp.Key, ccp.ColorCodeID, ref em);

									if (chnp.Key == Include.NoRID)
									{
										releaseOkay = false;

										msgText = "Color " + ccp.ColorCodeID + " NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

										em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
									}
									else
									{
										// ==========================
										// Open the PackColor element
										// ==========================
										xmlWriter.WriteStartElement("PackColor");

										// ===============================
										// Write the ColorCodeID attribute
										// ===============================
										xmlWriter.WriteStartAttribute(null, "ColorCodeID", null);
										xmlWriter.WriteString(ccp.ColorCodeID);
										xmlWriter.WriteEndAttribute();

										// =================================
										// Write the ColorCodeName attribute
										// =================================
										xmlWriter.WriteStartAttribute(null, "ColorCodeName", null);
										xmlWriter.WriteString(ccp.ColorCodeName);
										xmlWriter.WriteEndAttribute();

										// ========================================
										// Write the ColorCodeDescription attribute
										// ========================================
										xmlWriter.WriteStartAttribute(null, "ColorCodeDescription", null);
										xmlWriter.WriteString(chnp.NodeDescription);
										xmlWriter.WriteEndAttribute();

										// ==============================
										// Write the ColorUnits attribute
										// ==============================
										xmlWriter.WriteStartAttribute(null, "ColorUnits", null);
										qtyPacksUnits = packColor.ColorUnitsInPack;
										tmpString = qtyPacksUnits.ToString("0");
										xmlWriter.WriteString(tmpString);
										xmlWriter.WriteEndAttribute();

										// ===========================
										// Write the Pack Color Stores
										// ===========================
										foreach(StoreProfile sp in spl)
										{
											qtyPacksUnits = 0;

											Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

											if (storeIndexRID.RID != reserveStoreIndexRID.RID)
											{
												// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
												// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
												qtyAllocated = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID)
													+ ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
												// END TT#2347 - stodd - Release files not showing stores with only VSW values
												// END TT#1401 - stodd - add VSWVSW (IMO)
												unitsInLevel = ap.GetColorUnitsInPack(pack.PackName, ccp.Key);
												qtyPacksUnits = unitsInLevel * qtyAllocated;

												if (qtyPacksUnits > 0)
												{
													// ===============================
													// Open the PackColorStore element
													// ===============================
													xmlWriter.WriteStartElement("PackColorStore");

													// ===========================
													// Write the StoreID attribute
													// ===========================
													xmlWriter.WriteStartAttribute(null, "StoreID", null);
													xmlWriter.WriteString(sp.StoreId);
													xmlWriter.WriteEndAttribute();

													// =========================
													// Write the Units attribute
													// =========================
													xmlWriter.WriteStartAttribute(null, "Units", null);
													// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
													qtyAllocated = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID);
													qtyPacksUnits = unitsInLevel * qtyAllocated;
													// END TT#2347 - stodd - Release files not showing stores with only VSW values
													tmpString = qtyPacksUnits.ToString("0");
													xmlWriter.WriteString(tmpString);
													xmlWriter.WriteEndAttribute();

													// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
													// ===========================
													// Write the VSWID attribute
													// ===========================
													xmlWriter.WriteStartAttribute(null, "VSWID", null);
													xmlWriter.WriteString(sp.IMO_ID);
													xmlWriter.WriteEndAttribute();

													// =========================
													// Write the VSWUnits attribute
													// =========================
													xmlWriter.WriteStartAttribute(null, "VSWUnits", null);
													// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
													qtyAllocated = ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
													qtyPacksUnits = unitsInLevel * qtyAllocated;
													// END TT#2347 - stodd - Release files not showing stores with only VSW values
													tmpString = qtyPacksUnits.ToString("0");
													xmlWriter.WriteString(tmpString);
													xmlWriter.WriteEndAttribute();
													// END TT#1401 - stodd - add VSWVSW (IMO)

													// ================================
													// Close the PackColorStore element
													// ================================
													xmlWriter.WriteEndElement();
												}
											}
										}

										// ==========================
										// Write the Pack Color Sizes
										// ==========================
										//Begin Track #4302 - JScott - Size Codes in wrong order afer release
										//										foreach (PackContentBin packColorSize in packColor.ColorSizes.Values)
										//										{
										packColorSizeList = new SortedList();

                                        foreach (PackSizeBin packColorSize in packColor.ColorSizes.Values) // Assortment: added pack size bin
										{
											packColorSizeList.Add(packColorSize, packColorSize);
										}

                                        foreach (PackSizeBin packColorSize in packColorSizeList.Values) // Assortment: added pack size bin
										{
											//End Track #4302 - JScott - Size Codes in wrong order afer release
											SizeCodeProfile scp = GetSizeCodeProfile(packColorSize.ContentCodeRID, ref em);

											if (scp.Key == Include.NoRID)
											{
												releaseOkay = false;

												msgText = "Size " + scp.SizeCodeID + " NOT defined on the size table" + System.Environment.NewLine;

												em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
											}
											else
											{
												zhnp = GetSizeNodeProfile(shnp.HierarchyRID, chnp.Key, scp.SizeCodeID, ref em);

												if (zhnp.Key == Include.NoRID)
												{
													releaseOkay = false;

													msgText = "Size " + scp.SizeCodeID + " NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

													em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
												}
												else
												{
													// ==============================
													// Open the PackColorSize element
													// ==============================
													xmlWriter.WriteStartElement("PackColorSize");

													// ==============================
													// Write the SizeCodeID attribute
													// ==============================
													xmlWriter.WriteStartAttribute(null, "SizeCodeID", null);
													xmlWriter.WriteString(scp.SizeCodeID);
													xmlWriter.WriteEndAttribute();

													// ================================
													// Write the SizeCodeName attribute
													// ================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeName", null);
													xmlWriter.WriteString(scp.SizeCodeName);
													xmlWriter.WriteEndAttribute();

													// =======================================
													// Write the SizeCodeDescription attribute
													// =======================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeDescription", null);
													xmlWriter.WriteString(zhnp.NodeDescription);
													xmlWriter.WriteEndAttribute();

													// ===================================
													// Write the SizeCodePrimary attribute
													// ===================================
													xmlWriter.WriteStartAttribute(null, "SizeCodePrimary", null);
													xmlWriter.WriteString(scp.SizeCodePrimary);
													xmlWriter.WriteEndAttribute();

													// =====================================
													// Write the SizeCodeSecondary attribute
													// =====================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeSecondary", null);
													xmlWriter.WriteString(scp.SizeCodeSecondary);
													xmlWriter.WriteEndAttribute();

													// ===========================================
													// Write the SizeCodeProductCategory attribute
													// ===========================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeProductCategory", null);
													xmlWriter.WriteString(scp.SizeCodeProductCategory);
													xmlWriter.WriteEndAttribute();

													// =============================
													// Write the SizeUnits attribute
													// =============================
													xmlWriter.WriteStartAttribute(null, "SizeUnits", null);
													qtyPacksUnits = packColorSize.ContentUnits;
													tmpString = qtyPacksUnits.ToString("0");
													xmlWriter.WriteString(tmpString);
													xmlWriter.WriteEndAttribute();

													// ================================
													// Write the Pack Color Size Stores
													// ================================
													foreach(StoreProfile sp in spl)
													{
														qtyPacksUnits = 0;

														Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

														if (storeIndexRID.RID != reserveStoreIndexRID.RID)
														{
															// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
															// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
															qtyAllocated = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID)
																+ ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
															// END TT#2347 - stodd - Release files not showing stores with only VSW values
															// END TT#1401 - stodd - add VSWVSW (IMO)
															unitsInLevel = ap.GetPackColorSizeUnits(pack.PackName, ccp.Key, scp.Key);
															qtyPacksUnits = unitsInLevel * qtyAllocated;

															if (qtyPacksUnits > 0)
															{
																// ===================================
																// Open the PackColorSizeStore element
																// ===================================
																xmlWriter.WriteStartElement("PackColorSizeStore");

																// ===========================
																// Write the StoreID attribute
																// ===========================
																xmlWriter.WriteStartAttribute(null, "StoreID", null);
																xmlWriter.WriteString(sp.StoreId);
																xmlWriter.WriteEndAttribute();

																// =========================
																// Write the Units attribute
																// =========================
																xmlWriter.WriteStartAttribute(null, "Units", null);
																// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
																qtyAllocated = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID);
																qtyPacksUnits = unitsInLevel * qtyAllocated;
																// END TT#2347 - stodd - Release files not showing stores with only VSW values
																tmpString = qtyPacksUnits.ToString("0");
																xmlWriter.WriteString(tmpString);
																xmlWriter.WriteEndAttribute();

																// BEGIN TT#1401 - stodd - add VSW (IMO)
																// ===========================
																// Write the VSWID attribute
																// ===========================
																xmlWriter.WriteStartAttribute(null, "VSWID", null);
																xmlWriter.WriteString(sp.IMO_ID);
																xmlWriter.WriteEndAttribute();

																// =========================
																// Write the VSWUnits attribute
																// =========================
																xmlWriter.WriteStartAttribute(null, "VSWUnits", null);
																// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
																qtyAllocated = ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
																qtyPacksUnits = unitsInLevel * qtyAllocated;
																// END TT#2347 - stodd - Release files not showing stores with only VSW values
																tmpString = qtyPacksUnits.ToString("0");
																xmlWriter.WriteString(tmpString);
																xmlWriter.WriteEndAttribute();
																// END TT#1401 - stodd - add VSW (IMO)

																// ====================================
																// Close the PackColorSizeStore element
																// ====================================
																xmlWriter.WriteEndElement();
															}
														}
													}

													// ===============================
													// Close the PackColorSize element
													// ===============================
													xmlWriter.WriteEndElement();
												}
											}
										}

										// ===========================
										// Close the PackColor element
										// ===========================
										xmlWriter.WriteEndElement();
									}
								}
							}
						}

						// ========================
						// Write the Pack Size Info
						// ========================
						//Begin Track #4302 - JScott - Size Codes in wrong order afer release
						//						foreach (PackColorSize packColor in pack.PackColors.Values)
						//						{
						packColorList = new SortedList();

						foreach (PackColorSize packColor in pack.PackColors.Values)
						{
							packColorList.Add(packColor, packColor);
						}

						foreach (PackColorSize packColor in packColorList.Values)
						{
							//End Track #4302 - JScott - Size Codes in wrong order afer release
							if (packColor.ColorCodeRID == Include.DummyColorRID)
							{
								ColorCodeProfile ccp = GetColorCodeProfile(packColor.ColorCodeRID, ref em);

								if (ccp.Key == Include.NoRID)
								{
									releaseOkay = false;

									msgText = "Color " + ccp.ColorCodeID + " NOT defined on the color table" + System.Environment.NewLine;

									em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
								}
								else
								{
									chnp = GetColorNodeProfile(shnp.HierarchyRID, shnp.Key, ccp.ColorCodeID, ref em);

									if (chnp.Key == Include.NoRID)
									{
										releaseOkay = false;

										msgText = "Color " + ccp.ColorCodeID + " NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

										em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
									}
									else
									{
										// ====================
										// Write the Pack Sizes
										// ====================
										//Begin Track #4302 - JScott - Size Codes in wrong order afer release
										//										foreach (PackContentBin packColorSize in packColor.ColorSizes.Values)
										//										{
										packColorSizeList = new SortedList();

                                        foreach (PackSizeBin packColorSize in packColor.ColorSizes.Values)// Assortment: added pack size bin
										{
											packColorSizeList.Add(packColorSize, packColorSize);
										}

                                        foreach (PackSizeBin packColorSize in packColorSizeList.Values) // Assortment: added pack size bin
										{
											//End Track #4302 - JScott - Size Codes in wrong order afer release
											SizeCodeProfile scp = GetSizeCodeProfile(packColorSize.ContentCodeRID, ref em);

											if (scp.Key == Include.NoRID)
											{
												releaseOkay = false;

												msgText = "Size " + scp.SizeCodeID + " NOT defined on the size table" + System.Environment.NewLine;

												em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
											}
											else
											{
												zhnp = GetSizeNodeProfile(shnp.HierarchyRID, chnp.Key, scp.SizeCodeID, ref em);

												if (zhnp.Key == Include.NoRID)
												{
													releaseOkay = false;

													msgText = "Size " + scp.SizeCodeID + " NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

													em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
												}
												else
												{
													// =========================
													// Open the PackSize element
													// =========================
													xmlWriter.WriteStartElement("PackSize");

													// ==============================
													// Write the SizeCodeID attribute
													// ==============================
													xmlWriter.WriteStartAttribute(null, "SizeCodeID", null);
													xmlWriter.WriteString(scp.SizeCodeID);
													xmlWriter.WriteEndAttribute();

													// ================================
													// Write the SizeCodeName attribute
													// ================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeName", null);
													xmlWriter.WriteString(scp.SizeCodeName);
													xmlWriter.WriteEndAttribute();

													// =======================================
													// Write the SizeCodeDescription attribute
													// =======================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeDescription", null);
													xmlWriter.WriteString(zhnp.NodeDescription);
													xmlWriter.WriteEndAttribute();

													// ===================================
													// Write the SizeCodePrimary attribute
													// ===================================
													xmlWriter.WriteStartAttribute(null, "SizeCodePrimary", null);
													xmlWriter.WriteString(scp.SizeCodePrimary);
													xmlWriter.WriteEndAttribute();

													// =====================================
													// Write the SizeCodeSecondary attribute
													// =====================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeSecondary", null);
													xmlWriter.WriteString(scp.SizeCodeSecondary);
													xmlWriter.WriteEndAttribute();

													// ===========================================
													// Write the SizeCodeProductCategory attribute
													// ===========================================
													xmlWriter.WriteStartAttribute(null, "SizeCodeProductCategory", null);
													xmlWriter.WriteString(scp.SizeCodeProductCategory);
													xmlWriter.WriteEndAttribute();

													// =============================
													// Write the SizeUnits attribute
													// =============================
													xmlWriter.WriteStartAttribute(null, "SizeUnits", null);
													qtyPacksUnits = packColorSize.ContentUnits;
													tmpString = qtyPacksUnits.ToString("0");
													xmlWriter.WriteString(tmpString);
													xmlWriter.WriteEndAttribute();

													// ==========================
													// Write the Pack Size Stores
													// ==========================
													foreach(StoreProfile sp in spl)
													{
														qtyPacksUnits = 0;

														Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

														if (storeIndexRID.RID != reserveStoreIndexRID.RID)
														{
															// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
															// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
															qtyAllocated = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID)
																+ ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
															// END TT#2347 - stodd - Release files not showing stores with only VSW values
															// END TT#1401 - stodd - add VSWVSW (IMO)=
															unitsInLevel = ap.GetPackColorSizeUnits(pack.PackName, ccp.Key, scp.Key);
															qtyPacksUnits = unitsInLevel * qtyAllocated;

															if (qtyPacksUnits > 0)
															{
																// ==============================
																// Open the PackSizeStore element
																// ==============================
																xmlWriter.WriteStartElement("PackSizeStore");

																// ===========================
																// Write the StoreID attribute
																// ===========================
																xmlWriter.WriteStartAttribute(null, "StoreID", null);
																xmlWriter.WriteString(sp.StoreId);
																xmlWriter.WriteEndAttribute();

																// =========================
																// Write the Units attribute
																// =========================
																xmlWriter.WriteStartAttribute(null, "Units", null);
																// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
																qtyAllocated = ap.GetStoreItemQtyAllocated(pack.PackName, storeIndexRID);
																qtyPacksUnits = unitsInLevel * qtyAllocated;
																// END TT#2347 - stodd - Release files not showing stores with only VSW values
																tmpString = qtyPacksUnits.ToString("0");
																xmlWriter.WriteString(tmpString);
																xmlWriter.WriteEndAttribute();

																// BEGIN TT#1401 - stodd - add VSW (IMO)
																// ===========================
																// Write the VSWID attribute
																// ===========================
																xmlWriter.WriteStartAttribute(null, "VSWID", null);
																xmlWriter.WriteString(sp.IMO_ID);
																xmlWriter.WriteEndAttribute();

																// =========================
																// Write the VSWUnits attribute
																// =========================
																xmlWriter.WriteStartAttribute(null, "VSWUnits", null);
																// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
																qtyAllocated = ap.GetStoreImoQtyAllocated(pack.PackName, storeIndexRID);
																qtyPacksUnits = unitsInLevel * qtyAllocated;
																// END TT#2347 - stodd - Release files not showing stores with only VSW values
																tmpString = qtyPacksUnits.ToString("0");
																xmlWriter.WriteString(tmpString);
																xmlWriter.WriteEndAttribute();
																// END TT#1401 - stodd - add VSW (IMO)

																// ===============================
																// Close the PackSizeStore element
																// ===============================
																xmlWriter.WriteEndElement();
															}
														}
													}

													// ==========================
													// Close the PackSize element
													// ==========================
													xmlWriter.WriteEndElement();
												}
											}
										}
									}
								}
							}
						}

						// ======================
						// Close the Pack element
						// ======================
						xmlWriter.WriteEndElement();
					}

					// =========================
					// Write the Bulk Color Info
					// =========================
					//Begin Track #4302 - JScott - Size Codes in wrong order afer release
					//					foreach (HdrColorBin bulkColor in ap.BulkColors.Values)
					//					{
					bulkColorList = new SortedList();

					foreach (HdrColorBin bulkColor in ap.BulkColors.Values)
					{
						bulkColorList.Add(bulkColor, bulkColor);
					}

					foreach (HdrColorBin bulkColor in bulkColorList.Values)
					{
						//End Track #4302 - JScott - Size Codes in wrong order afer release
						ColorCodeProfile ccp = GetColorCodeProfile(bulkColor.ColorCodeRID, ref em);

						if (ccp.Key == Include.NoRID)
						{
							releaseOkay = false;

							msgText = "Color " + ccp.ColorCodeID + " NOT defined on the color table" + System.Environment.NewLine;

							em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
						}
						else
						{
							chnp = GetColorNodeProfile(shnp.HierarchyRID, shnp.Key, ccp.ColorCodeID, ref em);

							if (chnp.Key == Include.NoRID)
							{
								releaseOkay = false;

								msgText = "Color " + ccp.ColorCodeID + " NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

								em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
							}
							else
							{
								// ==========================
								// Open the BulkColor element
								// ==========================
								xmlWriter.WriteStartElement("BulkColor");

								// ===============================
								// Write the ColorCodeID attribute
								// ===============================
								xmlWriter.WriteStartAttribute(null, "ColorCodeID", null);
								xmlWriter.WriteString(ccp.ColorCodeID);
								xmlWriter.WriteEndAttribute();

								// =================================
								// Write the ColorCodeName attribute
								// =================================
								xmlWriter.WriteStartAttribute(null, "ColorCodeName", null);
								xmlWriter.WriteString(ccp.ColorCodeName);
								xmlWriter.WriteEndAttribute();

								// ========================================
								// Write the ColorCodeDescription attribute
								// ========================================
								xmlWriter.WriteStartAttribute(null, "ColorCodeDescription", null);
								xmlWriter.WriteString(chnp.NodeDescription);
								xmlWriter.WriteEndAttribute();

								// ======================================
								// Write the ColorReceivedUnits attribute
								// ======================================
								xmlWriter.WriteStartAttribute(null, "ColorReceivedUnits", null);
								qtyPacksUnits = bulkColor.ColorUnitsToAllocate;
								tmpString = qtyPacksUnits.ToString("0");
								xmlWriter.WriteString(tmpString);
								xmlWriter.WriteEndAttribute();

								// =======================================
								// Write the ColorAllocatedUnits attribute
								// =======================================
								xmlWriter.WriteStartAttribute(null, "ColorAllocatedUnits", null);
								qtyPacksUnits = bulkColor.ColorUnitsAllocated;
								tmpString = qtyPacksUnits.ToString("0");
								xmlWriter.WriteString(tmpString);
								xmlWriter.WriteEndAttribute();

								// =====================================
								// Write the ColorReserveUnits attribute
								// =====================================
								xmlWriter.WriteStartAttribute(null, "ColorReserveUnits", null);
								qtyPacksUnits = 0;
								if (reserveStoreIndexRID.RID != Include.UndefinedStoreRID)
								{
									qtyPacksUnits = ap.GetStoreQtyAllocated(ccp.Key, reserveStoreIndexRID);
								}
								tmpString = qtyPacksUnits.ToString("0");
								xmlWriter.WriteString(tmpString);
								xmlWriter.WriteEndAttribute();

								// ========================================
								// Write the Bulk Color Allocation by Store
								// ========================================
								foreach (StoreProfile sp in spl)
								{
									qtyPacksUnits = 0;

									Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

									if (storeIndexRID.RID != reserveStoreIndexRID.RID)
									{
										// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
										// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
										qtyPacksUnits = ap.GetStoreItemQtyAllocated(ccp.Key, storeIndexRID)
											+ ap.GetStoreImoQtyAllocated(ccp.Key, storeIndexRID);
										// END TT#2347 - stodd - Release files not showing stores with only VSW values
										// END TT#1401 - stodd - add VSWVSW (IMO)

										if (qtyPacksUnits > 0)
										{
											// ===============================
											// Open the BulkColorStore element
											// ===============================
											xmlWriter.WriteStartElement("BulkColorStore");

											// ===========================
											// Write the StoreID attribute
											// ===========================
											xmlWriter.WriteStartAttribute(null, "StoreID", null);
											xmlWriter.WriteString(sp.StoreId);
											xmlWriter.WriteEndAttribute();

											// =========================
											// Write the Units attribute
											// =========================
											xmlWriter.WriteStartAttribute(null, "Units", null);
											// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
											qtyPacksUnits = ap.GetStoreItemQtyAllocated(ccp.Key, storeIndexRID);
											// END TT#2347 - stodd - Release files not showing stores with only VSW values
											tmpString = qtyPacksUnits.ToString("0");
											xmlWriter.WriteString(tmpString);
											xmlWriter.WriteEndAttribute();

											// BEGIN TT#1401 - stodd - add VSW (IMO)
											// ===========================
											// Write the VSWID attribute
											// ===========================
											xmlWriter.WriteStartAttribute(null, "VSWID", null);
											xmlWriter.WriteString(sp.IMO_ID);
											xmlWriter.WriteEndAttribute();

											// =========================
											// Write the VSWUnits attribute
											// =========================
											xmlWriter.WriteStartAttribute(null, "VSWUnits", null);
											// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
											qtyPacksUnits = ap.GetStoreImoQtyAllocated(ccp.Key, storeIndexRID);
											// END TT#2347 - stodd - Release files not showing stores with only VSW values
											tmpString = qtyPacksUnits.ToString("0");
											xmlWriter.WriteString(tmpString);
											xmlWriter.WriteEndAttribute();
											// END TT#1401 - stodd - add VSW (IMO)

											// ================================
											// Close the BulkColorStore element
											// ================================
											xmlWriter.WriteEndElement();
										}
									}
								}

								// ==============================
								// Write the Bulk Color Size Info
								// ==============================
								//Begin Track #4302 - JScott - Size Codes in wrong order afer release
								//								foreach (HdrSizeBin bulkColorSize in bulkColor.ColorSizes.Values)
								//								{
								bulkColorSizeList = new SortedList();

								foreach (HdrSizeBin bulkColorSize in bulkColor.ColorSizes.Values)
								{
									bulkColorSizeList.Add(bulkColorSize, bulkColorSize);
								}

								foreach (HdrSizeBin bulkColorSize in bulkColorSizeList.Values)
								{
									//End Track #4302 - JScott - Size Codes in wrong order afer release
                                    SizeCodeProfile scp = GetSizeCodeProfile(bulkColorSize.SizeCodeRID, ref em); // Assortment: color/size changes

									if (scp.Key == Include.NoRID)
									{
										releaseOkay = false;

										msgText = "Size " + scp.SizeCodeID + " NOT defined on the color table" + System.Environment.NewLine;

										em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
									}
									else
									{
										zhnp = GetSizeNodeProfile(shnp.HierarchyRID, chnp.Key, scp.SizeCodeID, ref em);

										if (zhnp.Key == Include.NoRID)
										{
											releaseOkay = false;

											msgText = "Size " + scp.SizeCodeID + " NOT defined on the merchandise hierarchy" + System.Environment.NewLine;

											em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
										}
										else
										{
											// ==============================
											// Open the BulkColorSize element
											// ==============================
											xmlWriter.WriteStartElement("BulkColorSize");

											// ==============================
											// Write the SizeCodeID attribute
											// ==============================
											xmlWriter.WriteStartAttribute(null, "SizeCodeID", null);
											xmlWriter.WriteString(scp.SizeCodeID);
											xmlWriter.WriteEndAttribute();

											// ================================
											// Write the SizeCodeName attribute
											// ================================
											xmlWriter.WriteStartAttribute(null, "SizeCodeName", null);
											xmlWriter.WriteString(scp.SizeCodeName);
											xmlWriter.WriteEndAttribute();

											// =======================================
											// Write the SizeCodeDescription attribute
											// =======================================
											xmlWriter.WriteStartAttribute(null, "SizeCodeDescription", null);
											xmlWriter.WriteString(zhnp.NodeDescription);
											xmlWriter.WriteEndAttribute();

											// ===================================
											// Write the SizeCodePrimary attribute
											// ===================================
											xmlWriter.WriteStartAttribute(null, "SizeCodePrimary", null);
											xmlWriter.WriteString(scp.SizeCodePrimary);
											xmlWriter.WriteEndAttribute();

											// =====================================
											// Write the SizeCodeSecondary attribute
											// =====================================
											xmlWriter.WriteStartAttribute(null, "SizeCodeSecondary", null);
											xmlWriter.WriteString(scp.SizeCodeSecondary);
											xmlWriter.WriteEndAttribute();

											// ===========================================
											// Write the SizeCodeProductCategory attribute
											// ===========================================
											xmlWriter.WriteStartAttribute(null, "SizeCodeProductCategory", null);
											xmlWriter.WriteString(scp.SizeCodeProductCategory);
											xmlWriter.WriteEndAttribute();

											// =====================================
											// Write the SizeReceivedUnits attribute
											// =====================================
											xmlWriter.WriteStartAttribute(null, "SizeReceivedUnits", null);
											qtyPacksUnits = bulkColorSize.SizeUnitsToAllocate;
											tmpString = qtyPacksUnits.ToString("0");
											xmlWriter.WriteString(tmpString);
											xmlWriter.WriteEndAttribute();

											// ======================================
											// Write the SizeAllocatedUnits attribute
											// ======================================
											xmlWriter.WriteStartAttribute(null, "SizeAllocatedUnits", null);
											qtyPacksUnits = bulkColorSize.SizeUnitsAllocated;
											tmpString = qtyPacksUnits.ToString("0");
											xmlWriter.WriteString(tmpString);
											xmlWriter.WriteEndAttribute();

											// ====================================
											// Write the SizeReserveUnits attribute
											// ====================================
											xmlWriter.WriteStartAttribute(null, "SizeReserveUnits", null);
											qtyPacksUnits = 0;
											if (reserveStoreIndexRID.RID != Include.UndefinedStoreRID)
											{
												qtyPacksUnits = ap.GetStoreQtyAllocated(ccp.Key, scp.Key, reserveStoreIndexRID);
											}
											tmpString = qtyPacksUnits.ToString("0");
											xmlWriter.WriteString(tmpString);
											xmlWriter.WriteEndAttribute();

											// ================================
											// Write the Bulk Color Size Stores
											// ================================
											foreach(StoreProfile sp in spl)
											{
												qtyPacksUnits = 0;

												Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

												if (storeIndexRID.RID != reserveStoreIndexRID.RID)
												{
													// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
													// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
													qtyPacksUnits = ap.GetStoreItemQtyAllocated(ccp.Key, scp.Key, storeIndexRID)
														+ ap.GetStoreImoQtyAllocated(ccp.Key, scp.Key, storeIndexRID);
													// END TT#2347 - stodd - Release files not showing stores with only VSW values
													// END TT#1401 - stodd - add VSWVSW (IMO)

													if (qtyPacksUnits > 0)
													{
														// ===================================
														// Open the BulkColorSizeStore element
														// ===================================
														xmlWriter.WriteStartElement("BulkColorSizeStore");

														// ===========================
														// Write the StoreID attribute
														// ===========================
														xmlWriter.WriteStartAttribute(null, "StoreID", null);
														xmlWriter.WriteString(sp.StoreId);
														xmlWriter.WriteEndAttribute();

														// =========================
														// Write the Units attribute
														// =========================
														xmlWriter.WriteStartAttribute(null, "Units", null);
														// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
														qtyPacksUnits = ap.GetStoreItemQtyAllocated(ccp.Key, scp.Key, storeIndexRID);
														// END TT#2347 - stodd - Release files not showing stores with only VSW values
														tmpString = qtyPacksUnits.ToString("0");
														xmlWriter.WriteString(tmpString);
														xmlWriter.WriteEndAttribute();

														// BEGIN TT#1401 - stodd - add VSW (IMO)
														// ===========================
														// Write the VSWID attribute
														// ===========================
														xmlWriter.WriteStartAttribute(null, "VSWID", null);
														xmlWriter.WriteString(sp.IMO_ID);
														xmlWriter.WriteEndAttribute();

														// =========================
														// Write the VSWUnits attribute
														// =========================
														xmlWriter.WriteStartAttribute(null, "VSWUnits", null);
														// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
														qtyPacksUnits = ap.GetStoreImoQtyAllocated(ccp.Key, scp.Key, storeIndexRID);
														// END TT#2347 - stodd - Release files not showing stores with only VSW values
														tmpString = qtyPacksUnits.ToString("0");
														xmlWriter.WriteString(tmpString);
														xmlWriter.WriteEndAttribute();
														// END TT#1401 - stodd - add VSW (IMO)

														// ====================================
														// Close the BulkColorSizeStore element
														// ====================================
														xmlWriter.WriteEndElement();
													}
												}
											}

											// ===============================
											// Close the BulkColorSize element
											// ===============================
											xmlWriter.WriteEndElement();
										}
									}
								}

								// ===========================
								// Close the BulkColor element
								// ===========================
								xmlWriter.WriteEndElement();
							}
						}
					}
					// begin MID Track 4439 Released Header with no components not detailing store allocations
					// ==============================
					// Write the TOTAL Component Info
					// ==============================
					if (this.ap.Packs.Count == 0
						&& this.ap.BulkColors.Count == 0)
					{
						// ==========================
						// Open the Total element
						// ==========================
						xmlWriter.WriteStartElement("Total");
						// ============================
						// Write the Multiple attribute
						// ============================
						xmlWriter.WriteStartAttribute(null, "Multiple", null);
						multiple = ap.AllocationMultiple;
						tmpString = multiple.ToString("0");
						xmlWriter.WriteString(tmpString);
						xmlWriter.WriteEndAttribute();

						// ========================================
						// Write the Total Allocation by Store
						// ========================================
						foreach (StoreProfile sp in spl)
						{
							Index_RID storeIndexRID = ap.StoreIndex(sp.Key);

							if (storeIndexRID.RID != reserveStoreIndexRID.RID)
							{
								// BEGIN TT#1401 - stodd - add VSWVSW (IMO)
								// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
								qtyPacksUnits = ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID)
									+ ap.GetStoreImoQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID);
								// END TT#2347 - stodd - Release files not showing stores with only VSW values
								// END TT#1401 - stodd - add VSWVSW (IMO)

								if (qtyPacksUnits > 0)
								{
									// ===============================
									// Open the TotalStore element
									// ===============================
									xmlWriter.WriteStartElement("TotalStore");

									// ===========================
									// Write the StoreID attribute
									// ===========================
									xmlWriter.WriteStartAttribute(null, "StoreID", null);
									xmlWriter.WriteString(sp.StoreId);
									xmlWriter.WriteEndAttribute();

									// =========================
									// Write the Units attribute
									// =========================
									xmlWriter.WriteStartAttribute(null, "Units", null);
									// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
									qtyPacksUnits = ap.GetStoreItemQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID);
									// END TT#2347 - stodd - Release files not showing stores with only VSW values
									tmpString = qtyPacksUnits.ToString("0");
									xmlWriter.WriteString(tmpString);
									xmlWriter.WriteEndAttribute();

									// BEGIN TT#1401 - stodd - add VSW (IMO)
									// ===========================
									// Write the VSWID attribute
									// ===========================
									xmlWriter.WriteStartAttribute(null, "VSWID", null);
									xmlWriter.WriteString(sp.IMO_ID);
									xmlWriter.WriteEndAttribute();

									// =========================
									// Write the VSWUnits attribute
									// =========================
									xmlWriter.WriteStartAttribute(null, "VSWUnits", null);
									// BEGIN TT#2347 - stodd - Release files not showing stores with only VSW values
									qtyPacksUnits = ap.GetStoreImoQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID);
									// END TT#2347 - stodd - Release files not showing stores with only VSW values
									tmpString = qtyPacksUnits.ToString("0");
									xmlWriter.WriteString(tmpString);
									xmlWriter.WriteEndAttribute();
									// END TT#1401 - stodd - add VSW (IMO)

									// ================================
									// Close the TotalStore element
									// ================================
									xmlWriter.WriteEndElement();
								}
							}
						}

						// ===========================
						// Close the Total element
						// ===========================
						xmlWriter.WriteEndElement();
					}			
					// end MID Track 4439 Released Header with no components not detailing store allocations



					// =========================
					// Close the Release element
					// =========================
					xmlWriter.WriteEndElement();

					// ======================
					// Close the Root element
					// ======================
					xmlWriter.WriteEndElement();

					// ==================
					// Close the document
					// ==================
					xmlWriter.WriteEndDocument();

// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
//				// ================================================
//				// Update Master Allocated and Intransit Quantities
//				// ================================================
				// begin MID Track 4029 ReWork MasterPO Logic
				//if (!alreadyReleased)
				//{
				//	if (masterRID != Include.NoRID)
				//	{
				//		DataTable dtSubordData = header.GetComponentsForSubord(subordRID);
                //
				//		if (dtSubordData.Rows.Count > 0)
				//		{
				//			masterTotUnitsToAloct = 0;
                // 
				//			// begin MID Track 3587 Subtotal Qty Allocated wrong on Style Review after sub release
				//			//map = new AllocationProfile(ast, masterID, masterRID, ast.SAB.ApplicationServerSession);
				//			AllocationSubtotalProfile asp = ast.GetAllocationGrandTotalProfile();
				//			map = null;
				//			if (asp != null)
				//			{
				//				map = (AllocationProfile)asp.SubtotalMembers.FindKey(masterRID);
				//			}
				//			if (map == null)
				//			{
				//				map = new AllocationProfile(ast, masterID, masterRID, ast.SAB.ApplicationServerSession, false);
				//			}
				//			// end MID Track 3587 Subtotal Qty Allocated wrong on Style Review after sub release
                //
				//			if (map.Released)
				//			{
				//				masterReleased = true;
                //
				//				// begin MID Track 3548 Release date changing on PO header
				//				masterReleaseApprovedDate = map.ReleaseApprovedDate;
				//				masterReleaseDate = map.ReleaseDate;
				//				// end MID Track 3548 Release date changing on PO header
                //
				//				map.SetReleased(false);
                //
				//				map.SetReleaseApproved(false);
				//			}
                //   
				//			if (map.StyleIntransitUpdated)
				//			{
				//				masterIntransit = true;
                //  
				//				totalComp = new GeneralComponent(eComponentType.Total);
                //
				//				map.Action(eAllocationMethodType.BackoutStyleIntransit, totalComp, 0.0, Include.UndefinedStoreFilter, true);
				//			}
                //
				//			masterTotUnitsToAloct = map.TotalUnitsToAllocate;
                // 
				//			Hashtable mColors = map.BulkColors;      // MID Track 4029 Re-work Master-PO Logic
				//			HdrColorBin mHCB;                        // MID Track 4029 Re-work Master-PO Logic
				//			Hashtable mPacks = new Hashtable();      // MID Track 4029 Re-work Master-PO Logic
				//			foreach (PackHdr ph in map.Packs.Values) // MID Track 4029 Re-work Master-PO Logic
				//			{                                        // MID Track 4029 Re-work Master-PO Logic
				//				mPacks.Add(ph.PackRID, ph);          // MID Track 4029 Re-work Master-PO Logic 
				//			}                                        // MID Track 4029 Re-work Master-PO Logic
				//			PackHdr mPACK;                           // MID Track 4029 Re-work Master-PO Logic
                //            
				//			Hashtable sColors = ap.BulkColors;       // MID Track 4029 Re-work Master-PO Logic
				//			HdrColorBin sHCB;                        // MID Track 4029 Re-work Master-PO Logic
				//			Hashtable sPacks = new Hashtable();      // MID Track 4029 Re-work Master-PO Logic
				//			foreach (PackHdr ph in ap.Packs.Values)  // MID Track 4029 Re-work Master-PO Logic
				//			{                                        // MID Track 4029 Re-work Master-PO Logic
				//				sPacks.Add(ph.PackRID, ph);          // MID Track 4029 Re-work Master-PO Logic 
				//			}                                        // MID Track 4029 Re-work Master-PO Logic
				//			PackHdr sPACK;                           // MID Track 4029 Re-work Master-PO Logic
                //
				//			foreach (DataRow drSubordData in dtSubordData.Rows)
				//			{
				//				procMasterPack = false;
				//				procSubordPack = false;
                //  
				//				masterPack = string.Empty;
				//				subordPack = string.Empty;
                //  
				//				masterPackMult = 1;
				//				subordPackMult = 1;
				//				masterPacksAloctd = 0;
				//				masterUnitsAloctd = 0;
				//				subordPacksAloctd = 0;
				//				subordUnitsAloctd = 0;
				//				masterPacksToAloct = 0;
				//				masterUnitsToAloct = 0;
				//				subordPacksToAloct = 0;
				//				subordUnitsToAloct = 0;
				//				masterRsvPacksAloctd = 0;
				//				masterRsvUnitsAloctd = 0;
				//				subordRsvPacksAloctd = 0;
				//				subordRsvUnitsAloctd = 0;
				//				// begin MID Track 4029 Re-work Master PO Logic
				//				mPACK = null;
				//				mHCB = null;
				//				sPACK = null;
				//				sHCB = null;
				//				//masterPackRID = Include.NoRID;
				//				//masterColrRID = Include.NoRID;
				//				//subordPackRID = Include.NoRID;
				//				//subordColrRID = Include.NoRID;
                //
				//				//if (!Convert.IsDBNull(drSubordData["MASTER_PACK_RID"]))
				//				//{
				//				//	masterPackRID = Convert.ToInt32(drSubordData["MASTER_PACK_RID"], CultureInfo.CurrentUICulture);
				//				//}
                //
				//				//if (!Convert.IsDBNull(drSubordData["MASTER_COLOR_CODE_RID"]))
				//				//{
				//				//	masterColrRID = Convert.ToInt32(drSubordData["MASTER_COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
				//				//}
				//				masterPackRID = Convert.ToInt32(drSubordData["MASTER_PACK_RID"], CultureInfo.CurrentUICulture);
				//				masterColrRID = Convert.ToInt32(drSubordData["MASTER_COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
				//				// end MID Track 4029 Re-work Master PO Logic
                //
				//				masterCompType = (eComponentType)(Convert.ToInt32(drSubordData["MASTER_COMPONENT"], CultureInfo.CurrentUICulture));
                //  
				//				if (masterPackRID == Include.NoRID)
				//				{
				//					if (masterColrRID == Include.NoRID)
				//					{
				//						msgText = "Neither Master Pack NOR Color specified";
                //
				//						throw new Exception(msgText);
				//					}
				//					else
				//					{
				//						//Hashtable mColors = map.BulkColors;  // MID Track 4029 Re-work Master-PO Logic
                //
				//						// begin MID track 4029 Master-PO Logic
				//						//foreach (HdrColorBin mHCB in mColors.Values)
				//						//{
				//						//	if (mHCB.ColorKey == masterColrRID)
				//						//	{
                //                        //
				//						//     procMasterPack = false;
                //                        //
				//						//     masterUnitsAloctd = map.GetColorUnitsAllocated(mHCB.ColorKey);
                //                        //
				//						//     masterUnitsToAloct = map.GetColorUnitsToAllocate(mHCB.ColorKey);
                //                        //
				//						//     break;
				//						//  }
				//						//}
				//						mHCB = (HdrColorBin)mColors[masterColrRID];
				//						if (mHCB != null)
				//						{
				//							procMasterPack = false;
				//							masterUnitsAloctd = mHCB.ColorUnitsAllocated;
				//							masterUnitsToAloct = mHCB.ColorUnitsToAllocate;
				//						}
				//					}
				//				}
				//				else
				//				{
				//					if (masterColrRID != Include.NoRID)
				//					{
				//						msgText = "Both Master Pack AND Color specified";
                //
				//						throw new Exception(msgText);
				//					}
				//					else
				//					{
				//						//string [] mPackNames = map.GetPackNames();  // MID Track 4029 Re-work Master-PO Logic
                //
				//						// begin MID Track 4029 Re-work Master-PO Logic
//				//						foreach (string mPackName in mPackNames)
//				//						{
//				//							int packRID = map.GetPackRID(mPackName);
//              //
//				//							if (packRID == masterPackRID)
//				//							{
//				//								procMasterPack = true;
//              //
//				//								masterPack = mPackName;
//              //
//				//								masterPackMult = map.GetPackMultiple(mPackName);
//              //
//				//								masterPacksAloctd = map.GetPacksAllocated(mPackName);
//              //
//				//								masterPacksToAloct = map.GetPacksToAllocate(mPackName);
//              //
//				//								break;
//				//							}
//				//						}
				//						mPACK = (PackHdr)mPacks[masterPackRID];
				//						if (mPACK != null)
				//						{
				//							procMasterPack = true;
				//							masterPack = mPACK.PackName;
				//							masterPackMult = mPACK.PackMultiple;
				//							masterPacksAloctd = mPACK.PacksAllocated;
				//							masterPacksToAloct = mPACK.PacksToAllocate;
				//						}
				//						// end MID Track 4029 Re-work Master-PO Logic
                //
				//						masterUnitsAloctd = masterPacksAloctd * masterPackMult;
                //
				//						masterUnitsToAloct = masterPacksToAloct * masterPackMult;
				//					}
				//				}
                //
				//				// begin MID Track 4029 Re-Work Master-PO Logic
//				//				if (!Convert.IsDBNull(drSubordData["SUBORD_PACK_RID"]))
//				//				{
//				//					subordPackRID = Convert.ToInt32(drSubordData["SUBORD_PACK_RID"], CultureInfo.CurrentUICulture);
//				//				}
//              //
//				//				if (!Convert.IsDBNull(drSubordData["SUBORD_COLOR_CODE_RID"]))
//				//				{
//				//					subordColrRID = Convert.ToInt32(drSubordData["SUBORD_COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
//				//				}
				//				subordPackRID = Convert.ToInt32(drSubordData["SUBORD_PACK_RID"], CultureInfo.CurrentUICulture);
                //              subordColrRID = Convert.ToInt32(drSubordData["SUBORD_COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
				//				// end MID Track 4029 Re-Work Master-PO Logic
                //    
				//				subordCompType = (eComponentType)(Convert.ToInt32(drSubordData["SUBORD_COMPONENT"], CultureInfo.CurrentUICulture));
                //
				//				if (subordPackRID == Include.NoRID)
				//				{
				//					if (subordColrRID == Include.NoRID)
				//					{
				//						msgText = "Neither Subordinate Pack NOR Color specified";
                //
				//						throw new Exception(msgText);
				//					}
				//					else
				//					{
				//						//Hashtable sColors = ap.BulkColors; // MID Track 4029 Re-Work Master-PO Logic
                //  
				//						// begin MID Track 4029 Re-Work Master PO Logic
//				//						foreach (HdrColorBin sHCB in sColors.Values)
//				//						{
//				//							if (sHCB.ColorKey == subordColrRID)
//				//							{
//				//								procSubordPack = false;
//              //
//				//								subordUnitsAloctd = ap.GetColorUnitsAllocated(sHCB.ColorKey);
//              //
//				//								subordUnitsToAloct = ap.GetColorUnitsToAllocate(sHCB.ColorKey);
//              // 
//				//								break;
//				//							}
//				//						}
				//						sHCB = (HdrColorBin)sColors[subordColrRID];
				//						if (sHCB != null)
				//						{
				//							procSubordPack = false;
				//							subordUnitsAloctd = sHCB.ColorUnitsAllocated;
				//							subordUnitsToAloct = sHCB.ColorUnitsToAllocate;
				//						}
				//						// end MID Track 4029 Re-Work Master PO Logic
				//					}
				//				}
				//				else
				//				{
				//					if (subordColrRID != Include.NoRID)
				//					{
				//						msgText = "Both Subordinate Pack AND Color specified";
                //
				//						throw new Exception(msgText);
				//					}
				//					else
				//					{
				//						// begin MID Track 4029 Re-Work Master-PO Logic
//				//						string [] sPackNames = ap.GetPackNames();
//              //
//				//						foreach (string sPackName in sPackNames)
//				//						{
//				//							int packRID = ap.GetPackRID(sPackName);
//              //
//				//							if (packRID == subordPackRID)
//				//							{
//				//								procSubordPack = true;
//              //
//				//								subordPack = sPackName;
//              //
//				//								subordPackMult = ap.GetPackMultiple(sPackName);
//              //
//				//								subordPacksAloctd = ap.GetPacksAllocated(sPackName);
//              //
//				//								subordPacksToAloct = ap.GetPacksToAllocate(sPackName);
//              //
//				//								break;
//				//							}
//				//						}
				//						sPACK = (PackHdr)sPacks[subordPackRID];
				//						if (sPACK != null)
				//						{
				//							procSubordPack = true;
				//							subordPack = sPACK.PackName;
				//							subordPackMult = sPACK.PackMultiple;
				//							subordPacksAloctd = sPACK.PacksAllocated;
				//							subordPacksToAloct = sPACK.PacksToAllocate;
				//						}
                //
				//						subordUnitsAloctd = subordPacksAloctd * subordPackMult;
                //
				//						subordUnitsToAloct = subordPacksToAloct * subordPackMult;
				//					}
				//				}
                //  
				//				if (procMasterPack)
				//				{
				//					masterRsvPacksAloctd = map.GetStoreQtyAllocated(masterPack, reserveStoreIndexRID.RID);
				//					masterRsvUnitsAloctd = masterRsvPacksAloctd * masterPackMult;
                // 
				//					masterPacksAloctd -= masterRsvPacksAloctd;
				//					masterUnitsAloctd -= masterRsvUnitsAloctd;
                //
				//					masterPacksToAloct -= masterRsvPacksAloctd;
				//					masterUnitsToAloct -= masterRsvUnitsAloctd;
				//				}
				//				else
				//				{
				//					masterRsvUnitsAloctd = map.GetStoreQtyAllocated(masterColrRID, reserveStoreIndexRID.RID);
                //
				//					masterUnitsAloctd -= masterRsvUnitsAloctd;
                // 
				//					masterUnitsToAloct -= masterRsvUnitsAloctd;
				//				}
                //  
				//				masterTotUnitsToAloct -= masterRsvUnitsAloctd;
                //
				//				if (procSubordPack)
				//				{
				//					//subordRsvPacksAloctd = ap.GetStoreQtyAllocated(subordPack, reserveStoreIndexRID.RID);  // MID Track 4029 Re-Work Master-PO Logic
				//					subordRsvPacksAloctd = ap.GetStoreQtyAllocated(sPACK, reserveStoreIndexRID);             // MID Track 4029 Re-Work Master-PO Logic
				//					subordRsvUnitsAloctd = subordRsvPacksAloctd * subordPackMult;
                //
				//					subordPacksAloctd -= subordRsvPacksAloctd;
				//					subordUnitsAloctd -= subordRsvUnitsAloctd;
                //
				//					subordPacksToAloct -= subordRsvPacksAloctd;
				//					subordUnitsToAloct -= subordRsvUnitsAloctd;
				//				}
				//				else
				//				{
				//					//subordRsvUnitsAloctd = ap.GetStoreQtyAllocated(subordColrRID, reserveStoreIndexRID.RID); // MID Track 4029 Re-Work Master-PO Logic
				//					subordRsvUnitsAloctd = ap.GetStoreQtyAllocated(sHCB, reserveStoreIndexRID);                // MID Track 4029 Re-Work Master-PO Logic
                // 
				//					subordUnitsAloctd -= subordRsvUnitsAloctd;
                //
				//					subordUnitsToAloct -= subordRsvUnitsAloctd;
				//				}
                //
				//				// begin MID Track 4029 Re-Work Master-PO Logic
				//				//foreach(StoreProfile sp in spl)
				//				//{
				//				foreach (Index_RID storeIndexRID in ast.StoreIndexRIDArray())
				//				{
				//					// end MID Track 4029 Re-Work Master-PO Logic
				//					tmpUnits = 0;
				//					tmpPacks = 0; // MID Track 3543 Master Allocation changes on subordinate removal 
                //
				//					masterStrPacksAloctd = 0;
				//					masterStrUnitsAloctd = 0;
                //
				//					subordStrPacksAloctd = 0;
				//					subordStrUnitsAloctd = 0;
                //   
				//					//Index_RID storeIndexRID = ap.StoreIndex(sp.Key); // MID Track 4029 Re-Work Master-PO Logic
                //
				//					if (storeIndexRID.RID != reserveStoreIndexRID.RID)
				//					{
				//						if (procSubordPack)
				//						{
				//							//subordStrPacksAloctd = ap.GetStoreQtyAllocated(subordPack, storeIndexRID.RID); // MID Track 4029 Re-Work Master-PO Logic
				//							subordStrPacksAloctd = ap.GetStoreQtyAllocated(sPACK, storeIndexRID);            // MID Track 4029 Re-Work Master-PO Logic
				//							subordStrUnitsAloctd = subordStrPacksAloctd * subordPackMult;
				//						}
				//						else
				//						{
				//							//subordStrUnitsAloctd = ap.GetStoreQtyAllocated(subordColrRID, storeIndexRID.RID); // MID Track 4029 Re-Work Master-PO Logic
				//							subordStrUnitsAloctd = ap.GetStoreQtyAllocated(sHCB, storeIndexRID);                // MID Track 4029 Re-Work Master-PO Logic
				//						}
                //
				//						if (subordStrUnitsAloctd > 0)
				//						{
				//							if (procMasterPack)
				//							{
				//								//masterStrPacksAloctd = map.GetStoreQtyAllocated(masterPack, storeIndexRID.RID); // MID Track 4029 Re-Work Master-PO Logic
				//								masterStrPacksAloctd = map.GetStoreQtyAllocated(mPACK, storeIndexRID);            // MID Track 4029 Re-Work Master-PO Logic
				//								masterStrUnitsAloctd = masterStrPacksAloctd * masterPackMult;
				//							}
				//							else
				//							{
				//								//masterStrUnitsAloctd = map.GetStoreQtyAllocated(masterColrRID, storeIndexRID.RID); // MID Track 4029 Re-Work Master-PO Logic
				//								masterStrUnitsAloctd = map.GetStoreQtyAllocated(mHCB, storeIndexRID);                // MID Track 4029 Re-Work Master-PO Logic
				//							}
                //
				//							// begin MID Track 3543 Master Allocation changes on subordinate removal 
				//							//tmpUnits = subordStrPacksAloctd * masterPackMult;
                //                            //
				//							//masterPacksAloctd -= subordStrPacksAloctd;
                //                            //
				//							//if (masterPacksAloctd < 0)
				//							//{
				//							//	masterPacksAloctd = 0;
				//							//}
                //                          // 
				//							//masterUnitsAloctd -= tmpUnits;
                //                            //
				//							//if (masterUnitsAloctd < 0)
  				//							//{
				//							//	masterUnitsAloctd = 0;
				//							//}
                //                            //
				//							//masterPacksToAloct -= subordStrPacksAloctd;
                //                            //
				//							//if (masterPacksToAloct < 0)
				//							//{
				//							//	masterPacksToAloct = 0;
				//							//}
                //                            //
				//							//masterUnitsToAloct -= tmpUnits;
                //                            //
				//							//if (masterUnitsToAloct < 0)
				//							//{
				//							//	masterUnitsToAloct = 0;
				//							//}
				//                          //
				//							//masterStrPacksAloctd -= subordStrPacksAloctd;
                //                            //
				//							//if (masterStrPacksAloctd < 0)
				//							//{
				//							//	masterStrPacksAloctd = 0;
				//							//}
                //                          //
				//							//masterStrUnitsAloctd -= tmpUnits;
                //                            //
				//							//if (masterStrUnitsAloctd < 0)
				//							//{
				//							//	masterStrUnitsAloctd = 0;
				//							//}
                //                            //
				//							//masterTotUnitsToAloct -= tmpUnits;
                //                            //
				//							//if (masterTotUnitsToAloct < 0)
				//							//{
				//							//	masterTotUnitsToAloct = 0;
				//							//}
                //                            //
				//							tmpUnits = subordStrUnitsAloctd;
				//							if (tmpUnits > masterStrUnitsAloctd)
				//							{
				//								tmpUnits = masterStrUnitsAloctd;
				//								tmpPacks = masterStrPacksAloctd;
				//							}
				//							else
				//							{
				//								tmpPacks = 
				//									tmpUnits
				//									/ masterPackMult;
				//								if (tmpPacks * masterPackMult != tmpUnits)
				//								{
				//									throw new Exception("Units allocated in subordinate not evenly divisible by the master for at least one store");
				//								}
				//							}
				//							masterStrUnitsAloctd -= tmpUnits;
				//							masterUnitsAloctd -= tmpUnits;
				//							masterUnitsToAloct -= tmpUnits;
                //                            masterTotUnitsToAloct -= tmpUnits;
				//							masterStrPacksAloctd -= tmpPacks;
				//							masterPacksAloctd -= tmpPacks;
				//							masterPacksToAloct -= tmpPacks;
				//							// end MID Track 3543 Master Allocation changes on subordinate removal
				//							if (procMasterPack)
				//							{
				//								//map.SetStoreQtyAllocated(masterPack, storeIndexRID.RID, masterStrPacksAloctd);                       // MID Track 4029 Re-Work Master-PO Logic
				//								map.SetStoreQtyAllocated(mPACK, storeIndexRID, masterStrPacksAloctd, eDistributeChange.ToAll, false);  // MID Track 4029 Re-Work Master-PO Logic
				//							}
				//							else
				//							{
				//								//map.SetStoreQtyAllocated(masterColrRID, storeIndexRID.RID, masterStrUnitsAloctd);                     // MID Track 4029 Re-Work Master-PO Logic
				//								map.SetStoreQtyAllocated(mHCB, storeIndexRID, masterStrUnitsAloctd, eDistributeChange.ToParent, false); // MID track 4029 Re-Work Master-PO Logic
				//							}
				//						}
				//					}
				//				}
                //
				//				if (procMasterPack)
				//				{
				//					masterPacksAloctd += masterRsvPacksAloctd;
                // 
				//					masterPacksToAloct += masterRsvPacksAloctd;
                //
				//					//map.SetPacksAllocated(masterPack, masterPacksAloctd); // MID Track 3543 Master Allocation changes on subordinate removal
                //
				//					map.SetPacksToAllocate(masterPack, masterPacksToAloct);
				//				}
				//				else
				//				{
				//					masterUnitsAloctd += masterRsvUnitsAloctd;
                //
				//					masterUnitsToAloct += masterRsvUnitsAloctd;
                //
				//					//map.SetColorUnitsAllocated(masterColrRID, masterUnitsAloctd); // MID Track 3543 Master Allocation changes on subordinate removal
                // 
				//					map.SetColorUnitsToAllocate(masterColrRID, masterUnitsToAloct);
				//				}
                //
				//				masterTotUnitsToAloct += masterRsvUnitsAloctd;
				//			}
                //
				//			map.AllocatedUnits = 0;
                //  
				//			foreach(StoreProfile sp in spl)
				//			{
				//				Index_RID storeIndexRID = map.StoreIndex(sp.Key);
                //
				//				if (storeIndexRID.RID != reserveStoreIndexRID.RID)
				//				{
				//					map.AllocatedUnits += map.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndexRID.RID);
				//				}
				//			}
                // 
				//			map.TotalUnitsToAllocate = masterTotUnitsToAloct;
                //
				//			if (masterIntransit)
				//			{
				//				map.Action(eAllocationMethodType.ChargeIntransit, totalComp, 0.0, Include.UndefinedStoreFilter, true);
				//			}
                //
				//			if (masterReleased)
				//			{
				//				// begin MID Track 3548 Release date changing on PO headers
				//				//map.SetReleaseApproved(true); 
                //                //
				//				//map.SetReleased(true);
				//				map.SetReleaseApproved(true, masterReleaseApprovedDate); 
				//				map.SetReleased(true, masterReleaseDate);
				//				// end MID Track 3548 Release date changing on PO headers
				//			}
                //
				//			if (!map.WriteHeader())
				//			{
				//				msgText = "Error updating Master " + map.HeaderID;
                //
				//				throw new Exception(msgText);
				//			}
				//		}
				//	}
				//}

				// // =================
				// // Delete the Master
				// // =================
				//if (!alreadyReleased)
				//{
				//	if (masterRID != Include.NoRID)
				//	{
				//		try
				//		{
				//			header.OpenUpdateConnection();
                //
				//			try
				//			{
				//				subordStatus = header.DeleteSubordMaster(subordRID);
                //
				//				header.CommitData();
				//			}
                //
				//			catch (Exception)
				//			{
				//				throw;
				//			}
                //
				//			finally
				//			{
				//				if (header.ConnectionIsOpen)
				//				{
				//					header.CloseUpdateConnection();
				//				}
				//			}
                //
				//			if (subordStatus)
				//			{
				//				ast.SetAllocationActionStatus(subordRID, eAllocationActionStatus.ActionCompletedSuccessfully);
				//			}
				//		}
                // 
				//		catch (Exception)
				//		{
				//			throw;
				//		}
				//	}
				//}
                // end MID track 4029 ReWork MasterPO Logic
// (CSMITH) - END MID Track #3219
					// ========================
					// Update the Header status
					// ========================
                    // begin TT#111 - MD - JEllis - Header Release Fails Yet Header Released
//                    if (!alreadyReleased)
//                    {
//// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
//                    ap.ReleaseCount++;

////					ap.WriteHeader();
//                    //BeginTT#1767 - DOConnell - Allocation Header API Error Lock transaction timeout
//                    //if (!ap.WriteHeader(true))  // MID Track 4029 ReWork MasterPO Logic
//                    if (!ap.WriteHeader(true, false))  // MID Track 4029 ReWork MasterPO Logic
//                    //EndTT#1767 - DOConnell - Allocation Header API Error Lock transaction timeout
//                    {
//                        msgText = "Error updating Header " + ap.HeaderID;

//                        throw new Exception(msgText);
//                    }
//// (CSMITH) - END MID Track #3219
//                    }

//                    //Begin TT#1234 - JScott - Add trigger file for release file
//                    if (releaseOkay)
//                    {
//                        releaseFileTriggerExtension = ast.SAB.ApplicationServerSession.ReleaseFileTriggerExtension;

//                        if (releaseFileTriggerExtension != null && releaseFileTriggerExtension != string.Empty)
//                        {
//                            try
//                            {
//                                trgFile = new StreamWriter(hdrReleaseFile + "." + releaseFileTriggerExtension, false);
//                                trgFile.Close();
//                            }
//                            catch
//                            {
//                                releaseOkay = false;
//                                msgText = "Unable to write Release Trigger File: " + hdrReleaseFile + "." + releaseFileTriggerExtension + System.Environment.NewLine;
//                                em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
//                            }
//                        }
//                    }

//                    //End TT#1234 - JScott - Add trigger file for release file
                    trgFile = null;
                    if (releaseOkay)
                    {
                        releaseFileTriggerExtension = ast.SAB.ApplicationServerSession.ReleaseFileTriggerExtension;
                        // begin TT#2671 - Jellis - Release Action Fails yet released?
                        //if (releaseFileTriggerExtension != null
                        //    && releaseFileTriggerExtension != string.Empty)
                        //{
                        //    try
                        //    {
                        //        trgFile = new StreamWriter(hdrReleaseFile + "." + releaseFileTriggerExtension, false);
                        //    }
                        //    catch
                        //    {
                        //        releaseOkay = false;
                        //        msgText = "Unable to write Release Trigger File: " + hdrReleaseFile + "." + releaseFileTriggerExtension + System.Environment.NewLine;
                        //        em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                        //    }
                        //}
                        // end TT#2671 - Jellis - Release Action Fails yet released?
                        if (releaseOkay)
                        {
                            if (!alreadyReleased)
                            {
                                ap.ReleaseCount++;
                                // Begin TT#1966-MD - JSmith - DC Fulfillment
                                //releaseOkay = ap.WriteHeader(true, false);
                                releaseOkay = ap.WriteHeader(false, false);
                                // Begin TT#1966-MD - JSmith - DC Fulfillment
                                if (!releaseOkay)
                                {
                                    msgText = "Error updating Header " + ap.HeaderID;
                                    throw new Exception(msgText);
                                }
                                writeHeaderSuccess = true; // TT#2671 - Jellis - Release Action Fails yet released ?
                            }
                            //if (trgFile != null) // TT#2671 - Jellis - Release Action Fails yet released ?
                            if (releaseFileTriggerExtension != null              // TT#2671 - Jellis - Release Action Fails yet released ?
                                && releaseFileTriggerExtension != string.Empty)  // TT#2671 - Jellis - Release Action Fails yet released ? 
                            {
                                try
                                {
                                    // begin TT#2671 - Jellis - Release Action Fails yet released ?
                                    trgFile = new StreamWriter(hdrReleaseFile + "." + releaseFileTriggerExtension, false);
                                    // end TT#2671 - Jellis - Release Action Fails yet released ?
                                    trgFile.Close();
                                }
                                //catch                // TT#2671 - Jellis - Release Action Fails yet released?
                                catch (Exception Ex)   // TT#2671 - Jellis - Release Action Fails yet released?   
                                {
                                    releaseOkay = false;
                                    // begin TT#2671 - Jellis - Release Action Fails yet released?
                                    //msgText = "Unable to close Release Trigger File: " + hdrReleaseFile + "." + releaseFileTriggerExtension + System.Environment.NewLine;
                                    msgText = "Unable to write Release Trigger File: " + hdrReleaseFile + "." + releaseFileTriggerExtension + System.Environment.NewLine;
                                    while (Ex != null)
                                    {
                                        msgText += " -- " + Ex.ToString();

                                        Ex = Ex.InnerException;
                                    }
                                    em.AddMsg(eMIDMessageLevel.Error, msgText, GetType().Name);
                                    // end TT#2671 - Jellis - Release Action Fails yet released?
                                }
                            }
                        }
                    }
                    // end TT#111 - MD - Jellis - Header Release Fails Yet Header Released
					if (!releaseOkay)
					{
						msgText = "Allocation Header " + ap.HeaderID + " NOT released" + System.Environment.NewLine;
					}
					else
					{
						msgText = "Allocation Header " + ap.HeaderID + " (Release File: " + hdrReleaseFile + ") released";
					}
				}
			}

			catch (Exception Ex)
			{
				releaseOkay = false;

				msgText = "Allocation Header " + ap.HeaderID + " NOT released" + System.Environment.NewLine;

				while (Ex != null)
				{
					msgText += " -- " + Ex.ToString();

					Ex = Ex.InnerException;
				}
			}

			finally
			{
                // begin TT#2671 - Release Action Fails yet released?
                if (!releaseOkay
                    && !alreadyReleased
                    && writeHeaderSuccess)
                {
                    ap.SetReleased(false);
                    ap.ReleaseCount--;
                    ap.WriteHeader(false, false);
                }
                // end TT#2671 - Release Action Fails yet released ?
				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					ast.SAB.ApplicationServerSession.Audit.Add_Msg(emm.messageLevel, emm.msg, emm.module);
				}

				// ===================================
				// Flush the buffer and close the file
				// ===================================
				if (xmlWriter != null)
				{
					xmlWriter.Flush();

					xmlWriter.Close();
				}
			}

			return releaseOkay;
		}

		// =======================================
		// Retrieve a style hierarchy node profile
		// =======================================
		private HierarchyNodeProfile GetStyleNodeProfile(int aStyleHnRID, ref EditMsgs aEm)
		{
			bool styleOkay = true;

			HierarchyNodeProfile styleHnp = null;
			HierarchyNodeProfile undefHnp = null;

			// ================
			// Begin processing
			// ================
			undefHnp = new HierarchyNodeProfile(Include.NoRID);

			try
			{
				// ===================================
				// Retrieve a hierarchy node profile
				// ===================================
				//styleHnp = ast.SAB.HierarchyServerSession.GetNodeData(aStyleHnRID);  // MID Track #### Performance
				styleHnp = ast.GetNodeData(aStyleHnRID);                               // MID Track #### Performance
			}

			catch (Exception Ex)
			{
				styleOkay = false;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
				}
			}

			if (styleOkay)
			{
				return styleHnp;
			}
			else
			{
				return undefHnp;
			}
		}

		// ==============================================
		// Retrieve a style parent hierarchy node profile
		// ==============================================
		private HierarchyNodeProfile GetStyleParentNodeProfile(int aParentHnRID, ref EditMsgs aEm)
		{
			bool parentOkay = true;

			HierarchyNodeProfile parentHnp = null;
			HierarchyNodeProfile undefHnp = null;

			// ================
			// Begin processing
			// ================
			undefHnp = new HierarchyNodeProfile(Include.NoRID);

			try
			{
				// =================================
				// Retrieve a hierarchy node profile
				// =================================
				//parentHnp = ast.SAB.HierarchyServerSession.GetNodeData(aParentHnRID); // MID Track #### Performance
				parentHnp = ast.GetNodeData(aParentHnRID);                              // MID Track #### Performance
			}

			catch (Exception Ex)
			{
				parentOkay = false;

				if (Ex.GetType() != typeof(MIDException))
				{
					aEm.AddMsg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
				}
				else
				{
					MIDException MIDEx = (MIDException)Ex;

					aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
				}
			}

			if (parentOkay)
			{
				return parentHnp;
			}
			else
			{
				return undefHnp;
			}
		}

		// =============================
		// Retrieve a color code profile
		// =============================
		private ColorCodeProfile GetColorCodeProfile(int aColorCodeRID, ref EditMsgs aEm)
		{
			bool colorOkay = true;

			ColorCodeProfile colorCodeProfile = null;
			ColorCodeProfile undefColorCodeProfile = null;

			// ================
			// Begin processing
			// ================
			undefColorCodeProfile = new ColorCodeProfile(Include.NoRID);

			try
			{
				// ==================
				// Retrieve a profile
				// ==================
				//colorCodeProfile = ast.SAB.HierarchyServerSession.GetColorCodeProfile(aColorCodeRID); // MID Track #### Performance
				colorCodeProfile = ast.GetColorCodeProfile(aColorCodeRID);                              // MID Track #### Performance
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

					aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
				}
			}

			if (colorOkay)
			{
				return colorCodeProfile;
			}
			else
			{
				return undefColorCodeProfile;
			}
		}

		// =======================================
		// Retrieve a color hierarchy node profile
		// =======================================
		private HierarchyNodeProfile GetColorNodeProfile(int aHierarchyRID, int aStyleHnRID, string aColorCodeID, ref EditMsgs aEm)
		{
			bool colorOkay = true;

			int colorNodeRID = Include.NoRID;

			HierarchyNodeProfile colorHnp = null;
			HierarchyNodeProfile undefHnp = null;

			// ================
			// Begin processing
			// ================
			undefHnp = new HierarchyNodeProfile(Include.NoRID);

			try
			{
				// ==============================
				// Does a color exist for a style
				// ==============================
				if (ast.SAB.HierarchyServerSession.ColorExistsForStyle(aHierarchyRID, aStyleHnRID, aColorCodeID, null, ref colorNodeRID))
				{
					try
					{
						// =================================
						// Retrieve a hierarchy node profile
						// =================================
						//colorHnp = ast.SAB.HierarchyServerSession.GetNodeData(colorNodeRID); // MID Track #### Performance
						colorHnp = ast.GetNodeData(colorNodeRID);   // MID Track #### Performance
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

							aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
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

					aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
				}
			}

			if (colorOkay)
			{
				return colorHnp;
			}
			else
			{
				return undefHnp;
			}
		}

		// ============================
		// Retrieve a size code profile
		// ============================
		private SizeCodeProfile GetSizeCodeProfile(int aSizeCodeRID, ref EditMsgs aEm)
		{
			bool sizeOkay = true;

			SizeCodeProfile sizeCodeProfile = null;
			SizeCodeProfile undefSizeCodeProfile = null;

			// ================
			// Begin processing
			// ================
			undefSizeCodeProfile = new SizeCodeProfile(Include.NoRID);

			try
			{
				// ==================
				// Retrieve a profile
				// ==================
				//sizeCodeProfile = ast.SAB.HierarchyServerSession.GetSizeCodeProfile(aSizeCodeRID);  // MID Track #### Performance
				sizeCodeProfile = ast.GetSizeCodeProfile(aSizeCodeRID);                               // MID Track #### Performance
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

					aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
				}
			}

			if (sizeOkay)
			{
				return sizeCodeProfile;
			}
			else
			{
				return undefSizeCodeProfile;
			}
		}

		// ======================================
		// Retrieve a size hierarchy node profile
		// ======================================
		private HierarchyNodeProfile GetSizeNodeProfile(int aHierarchyRID, int aColorNodeRID, string aSizeCodeID, ref EditMsgs aEm)
		{
			bool sizeOkay = true;

			int sizeNodeRID = Include.NoRID;

			HierarchyNodeProfile sizeHnp = null;
			HierarchyNodeProfile undefHnp = null;

			// ================
			// Begin processing
			// ================
			undefHnp = new HierarchyNodeProfile(Include.NoRID);

			try
			{
				// =============================
				// Does a size exist for a color
				// =============================
				if (ast.SAB.HierarchyServerSession.SizeExistsForColor(aHierarchyRID, aColorNodeRID, aSizeCodeID, null, ref sizeNodeRID))
				{
					try
					{
						// =================================
						// Retrieve a hierarchy node profile
						// =================================
						//sizeHnp = ast.SAB.HierarchyServerSession.GetNodeData(sizeNodeRID);  // MID Track #### Performance
						sizeHnp = ast.GetNodeData(sizeNodeRID);    // MID Track #### Performance
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

							aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
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

					aEm.AddMsg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
				}
			}

			if (sizeOkay)
			{
				return sizeHnp;
			}
			else
			{
				return undefHnp;
			}
		}
	}
}

