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
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.SizeCurveLoad
{
	/// <summary>
	/// Summary description for SizeCurveGroupLoadProcess.
	/// </summary>
	public class SizeCurveGroupLoadProcess
	{
//		private string sourceModule = "SizeCurveGroupLoadProcess.cs";

		SessionAddressBlock _SAB;

		//Begin TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
		const float sizeValueOne = 1.0F;

		//End TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
		private int _groupsRead = 0;
		private int _groupsError = 0;
		private int _groupsCreated = 0;
		private int _groupsModify = 0;
		private int _groupsRemoved = 0;

		private Audit _audit = null;

		private string _comment = null;
		private string _statusFile = null;

		private bool _codesPresent = false;

		private XmlTextWriter _xmlWriter = null;

		private ArrayList _prodCategoryList = null;

        //private ApplicationSessionTransaction _appSessTrans = null; // TT#1185 - JEllis - Verify ENQ before Update (part 2)
		// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
		private bool _createOnModify;
		// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found

		// BEGIN MID Track #5268 - RonM - Size Curve Add/Update slow
		private bool _curvesInGroupUpdated;
		// END MID Track #5268  

        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
        public int GroupsRead
        {
            get { return _groupsRead; }
        }
        public int GroupsWithErrors
        {
            get { return _groupsError; }
        }
        public int GroupsCreated
        {
            get { return _groupsCreated; }
        }
        public int GroupsModify
        {
            get { return _groupsModify; }
        }
        public int GroupsRemoved
        {
            get { return _groupsRemoved; }
        }
        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

		// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
//		public SizeCurveGroupLoadProcess(SessionAddressBlock SAB, bool scCodesPresent, ref bool scgErrorFound)
		public SizeCurveGroupLoadProcess(SessionAddressBlock SAB, bool scCodesPresent, bool aCreateOnModify, ref bool scgErrorFound)
		// END MID Track #5153
		{
			try
			{
				_SAB = SAB;

				_codesPresent = scCodesPresent;
				// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
				_createOnModify = aCreateOnModify;
				// END MID Track #5153

				_audit = _SAB.ClientServerSession.Audit;

				//_appSessTrans = new ApplicationSessionTransaction(_SAB); // TT#1185 - JEllis - Verify ENQ before Update (part 2)

				_prodCategoryList = new ArrayList();

				try
				{
					_prodCategoryList = _SAB.HierarchyServerSession.GetSizeProductCategoryList();

					if (_prodCategoryList.Count == 0)
					{
						scgErrorFound = true;

						//string msgText= "No product categories are currently defined";
                        string msgText = MIDText.GetText(eMIDTextCode.msg_scglp_NoProduct);
						_audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);
					}
				}

				catch (Exception Ex)
				{
					scgErrorFound = true;

					if (Ex.GetType() != typeof(MIDException))
					{
						_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
					}
					else
					{
						MIDException MIDEx = (MIDException)Ex;

						_audit.Add_Msg(eMIDMessageLevel.Severe, MIDEx.ErrorMessage, GetType().Name);
					}
				}
			}

			catch (Exception Ex)
			{
				scgErrorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
			}
		}

		// ====================================================
		// Serialize and process the xml input transaction file
		// ====================================================
		public eReturnCode ProcessGroupTrans(string sizeCurveGroupTransFile, ref bool scgErrorFound)
		{
			string msgText = null;

			TextReader xmlReader = null;

			XmlSerializer xmlSerial = null;

			SizeCurveGroups sizeCurveGroupTrans = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			try
			{
				try
				{
					xmlSerial = new XmlSerializer(typeof(SizeCurveGroups));

					xmlReader = new StreamReader(sizeCurveGroupTransFile);

					sizeCurveGroupTrans = (SizeCurveGroups)xmlSerial.Deserialize(xmlReader);

                    // Begin Track #4229 - JSmith - API locks .XML input file
                    //xmlReader.Close();
                    // End Track #4229
				}

				catch (Exception Ex)
				{
					scgErrorFound = true;

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

				if (!scgErrorFound)
				{
					if (sizeCurveGroupTrans.SizeCurveGroup != null)
					{
						// ===============================================
						// Create an xml writer and set formatting options
						// ===============================================
						_statusFile = Path.ChangeExtension(sizeCurveGroupTransFile, ".out.xml");
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
						_comment = "Size Curve Group Load Status for: " + Path.GetFileName(sizeCurveGroupTransFile) + " submitted on " + DateTime.Now.ToString("U");
						_xmlWriter.WriteComment(_comment);

						// ======================
						// Write the root element
						// ======================
						_xmlWriter.WriteStartElement("SizeCurveGroupLoadStatus", "http://tempuri.org/SizeCurveGroupLoadStatusSchema.xsd");

						foreach (SizeCurveGroupsSizeCurveGroup sizeCurveGroupTran in sizeCurveGroupTrans.SizeCurveGroup)
						{
							++_groupsRead;

							rtnCode = ProcessOneGroup(sizeCurveGroupTran);

							if (rtnCode != eReturnCode.successful)
							{
								++_groupsError;
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
						//msgText = "No Size Curve Group transactions found in input file" + System.Environment.NewLine;
                        msgText = MIDText.GetText(eMIDTextCode.msg_scglp_NoSize);
                        msgText += System.Environment.NewLine;
						_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
					}
				}
			}

			catch (Exception Ex)
			{
				scgErrorFound = true;

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

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                //msgText = MIDText.GetText(eMIDTextCode.msg_scglp_ProcessGroupTransFinal);
                //msgText = msgText.Replace("{0}", _groupsRead.ToString() + System.Environment.NewLine);
                //msgText = msgText.Replace("{1}", _groupsError.ToString() + System.Environment.NewLine);
                //msgText = msgText.Replace("{2}", _groupsCreated.ToString() + System.Environment.NewLine);
                //msgText = msgText.Replace("{3}", _groupsModify.ToString() + System.Environment.NewLine);
                //msgText = msgText.Replace("{4}", _groupsRemoved.ToString() + System.Environment.NewLine);
                //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

				//msgText  = "Groups Read: " + _groupsRead.ToString() + System.Environment.NewLine;
				//msgText += "Groups In Error: " + _groupsError.ToString() + System.Environment.NewLine;
				//msgText += "Groups Created: " + _groupsCreate.ToString() + System.Environment.NewLine;
				//msgText += "Groups Modified: " + _groupsModify.ToString() + System.Environment.NewLine;
				//msgText += "Groups Removed: " + _groupsRemove.ToString() + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

                //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                //_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
                //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

				// ===================================================================================
				// This statement is currently inactive - requires extensive modifications to activate
				// ===================================================================================
				//_audit.CurveGroupLoadAuditInfo_Add(_groupsRead, _groupsError, _groupsCreate, _groupsModify, _groupsRemove);
			}

			return rtnCode;
		}

		// ========================================
		// Process one size curve group transaction
		// ========================================
		public eReturnCode ProcessOneGroup(SizeCurveGroupsSizeCurveGroup sizeCurveGroupTran)
		{
			EditMsgs em = null;

			string msgText = null;
			string tranImg = null;

			SizeCurve sizeCurve = null;

			int groupRID = Include.NoRID;

			eReturnCode rtnCode = eReturnCode.successful;

			SizeCurveGroupProfile sizeCurveGroupProfile = null;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{
				// BEGIN MID Track 4279 - trim spaces off string fields
//				Thread.Sleep(5);
				Include.TrimStringsInObject(sizeCurveGroupTran);
				// END MID Track 4279

				_curvesInGroupUpdated = false;	// MID Track #5268 - RonM - Size Curve Add/Update slow

				if (sizeCurveGroupTran.SizeCurveGroupName == "" || sizeCurveGroupTran.SizeCurveGroupName == null)
				{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    msgText = MIDText.GetText(eMIDTextCode.msg_scglp_ProcessOneGroup);
                    msgText = msgText.Replace("{0}", _groupsRead.ToString());
                    msgText += System.Environment.NewLine;

                    //msgText = "Size Curve Group Transaction #" + _groupsRead.ToString();
					//msgText += " has NO Size Curve Group Name" + System.Environment.NewLine;
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */

					em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

					WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

					rtnCode = eReturnCode.editErrors;
				}
				else
				{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    msgText = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessing);
                    msgText = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    msgText = msgText.Replace("{1}", _groupsRead.ToString());
                    msgText += System.Environment.NewLine;
                    
                    //msgText = "Size Curve Group " + sizeCurveGroupTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					//msgText += " - Begin Processing" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
					_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveGroupAction);
                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
					//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                    tranImg += System.Environment.NewLine;

                    //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
					//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "]";
					//tranImg += System.Environment.NewLine + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

					// ================
					// Determine action
					// ================
					if (!sizeCurveGroupTran.SizeCurveGroupActionSpecified)
					{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_ActionRequired);
                        msgText += System.Environment.NewLine;
                        //msgText = tranImg + "Message: Size Curve Group Action is required" + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
						em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

						WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

						rtnCode = eReturnCode.editErrors;
					}
					else
					{
						if (sizeCurveGroupTran.SizeCurveGroupAction.ToString().ToUpper() != "CREATE" &&
							sizeCurveGroupTran.SizeCurveGroupAction.ToString().ToUpper() != "MODIFY" &&
							sizeCurveGroupTran.SizeCurveGroupAction.ToString().ToUpper() != "REMOVE")
						{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_ActionInvalid);
                            msgText += System.Environment.NewLine;

                            //msgText = tranImg + "Message: Size Curve Group Action is invalid" + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
							em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

							WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

							rtnCode = eReturnCode.editErrors;
						}
					}

					if (rtnCode == eReturnCode.successful)
					{
						sizeCurve = new SizeCurve();

						groupRID = sizeCurve.GetSizeCurveGroupKey(sizeCurveGroupTran.SizeCurveGroupName);

						sizeCurveGroupProfile = new SizeCurveGroupProfile(groupRID);

						// ==============================================================================
						// If size curve group already exists, this is not a create but a modify / remove
						// ==============================================================================
						if (groupRID != Include.NoRID)
						{
							if (sizeCurveGroupTran.SizeCurveGroupAction.ToString().ToUpper() == "CREATE")
							{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_AlreadyDefined);
                                msgText += System.Environment.NewLine;
                                // msgText = tranImg + "Message: Action invalid when Size Curve Group already defined" + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                
								em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

								WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

								rtnCode = eReturnCode.editErrors;
							}
							else
							{
								if (sizeCurveGroupTran.SizeCurveGroupAction.ToString().ToUpper() == "MODIFY")
								{
									if (sizeCurveGroupTran.SizeCurve == null)
									{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeCurveTransactions);
                                        msgText += System.Environment.NewLine;
                                        // msgText = tranImg + "Message: No size curve transactions found to process" + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                     
										em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

										WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

										rtnCode = eReturnCode.editErrors;
									}
									else
									{
										rtnCode = ModifyGroup(sizeCurveGroupTran, sizeCurveGroupProfile);

										if (rtnCode == eReturnCode.successful)
										{
											++_groupsModify;
										}
									}
								}
								else
								{
									rtnCode = RemoveGroup(sizeCurveGroupTran, sizeCurveGroupProfile);

									if (rtnCode == eReturnCode.successful)
									{
										++_groupsRemoved;
									}
								}
							}
						}
						else
						{
							// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
							if (_createOnModify 
								&& sizeCurveGroupTran.SizeCurveGroupAction == SizeCurveGroupsSizeCurveGroupSizeCurveGroupAction.Modify)
							{
								sizeCurveGroupTran.SizeCurveGroupAction = SizeCurveGroupsSizeCurveGroupSizeCurveGroupAction.Create;
							}
							// END MID Track #5153

							if (sizeCurveGroupTran.SizeCurveGroupAction.ToString().ToUpper() == "CREATE")
							{
								if (sizeCurveGroupTran.SizeCurve == null)
								{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeCurveTransactions);
                                    msgText += System.Environment.NewLine;
                                    // msgText = tranImg + "Message: No size curve transactions found to process" + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

                                    em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

									WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

									rtnCode = eReturnCode.editErrors;
								}
								else
								{
									rtnCode = CreateGroup(sizeCurveGroupTran, sizeCurveGroupProfile);

									if (rtnCode == eReturnCode.successful)
									{
										++_groupsCreated;
									}
								}
							}
							else
							{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveGroupNotDefined);
                                msgText += System.Environment.NewLine;
                                //  msgText = tranImg + "Message: Action invalid when Size Curve Group is NOT defined" + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
                               
								em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);

								WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

								rtnCode = eReturnCode.editErrors;
							}
						}
					}

					for (int e = 0; e < em.EditMessages.Count; e++)
					{
						EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

                        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                        //_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
                        _audit.Add_Msg(emm.messageLevel, emm.msg, emm.module);
                        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    }

					em.ClearMsgs();

//					Thread.Sleep(5);
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    msgText = MIDText.GetText(eMIDTextCode.msg_scglp_EndProcessing);
                    msgText = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    msgText = msgText.Replace("{1}", _groupsRead.ToString());
                    msgText += System.Environment.NewLine;

					//msgText = "Size Curve Group " + sizeCurveGroupTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					//msgText += " - End Processing" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

					_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

				}
			}

			catch (Exception Ex)
			{
				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);

				rtnCode = eReturnCode.severe;
			}

			finally
			{
				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// =========================
		// Create a size curve group
		// =========================
		public eReturnCode CreateGroup(SizeCurveGroupsSizeCurveGroup sizeCurveGroupTran, SizeCurveGroupProfile sizeCurveGroupProfile)
		{
			EditMsgs em = null;

			double szVal = 0.0;
			double sizeValue = 0.0;

			string msgText = null;
			string tranImg = null;
			string exceptMsg = null;

			bool defCurve = false;
			bool skipSize = false;
			bool editError = false;
			bool sizeError = false;
			bool createOkay = true;
			bool spreadOkay = true;
			bool curveError = false;

			//Begin TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
			//float sizeValueOne = 1.0F;
			//End TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
			float sizeValueZero = 0.0F;

			int sizeRID = Include.NoRID;
			Hashtable sizeProfiles = null;						// Size Code Profiles keyed by Size Code RID
			Hashtable curveProfiles = null;						// Size Curve Profiles keyed by Size Curve Name
			Hashtable allSizeProfiles = null;					// Size Code Profiles keyed by Size Code RID (for default)

			SizeCodeProfile sizeCodeProfile = null;
			SizeCodeProfile sizeCodeProfileClone = null;

			SizeCurveProfile defSizeCurveProfile = null;
			SizeCurveProfile tmpSizeCurveProfile = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{	
				sizeProfiles = new Hashtable();

				curveProfiles = new Hashtable();
				curveProfiles.Clear();

				allSizeProfiles = new Hashtable();
				allSizeProfiles.Clear();

				// ================================
				// Edit the size curve transactions
				// ================================
				foreach (SizeCurveGroupsSizeCurveGroupSizeCurve sizeCurveTran in sizeCurveGroupTran.SizeCurve)
				{
					defCurve = false;
					curveError = false;
					
					sizeProfiles.Clear();

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
					//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                    //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                    //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                    tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                    tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                    tranImg += System.Environment.NewLine + System.Environment.NewLine;

					//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
					//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
					//tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
					//tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
					//tranImg += System.Environment.NewLine + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

					// ========================
					// Edit the size curve name
					// ========================
					defCurve = (sizeCurveTran.SizeCurveName == "" || sizeCurveTran.SizeCurveName == null) ? true : false;

					// ==========================
					// Edit the size curve action
					// ==========================
					if (!sizeCurveTran.SizeCurveActionSpecified)
					{
                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveActionMissing);//TT780 
                        //msgText = tranImg + "Message: Size Curve Action is missing - defaulting to 'Create'" + System.Environment.NewLine;//TT780
						em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
					}
					else
					{
						if (sizeCurveTran.SizeCurveAction.ToString().ToUpper() != "CREATE")
						{
                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveActionInvalid);//TT780
                            msgText += System.Environment.NewLine + System.Environment.NewLine;
                            //msgText = tranImg + "Message: Size Curve Action is invalid - defaulting to 'Create'" + System.Environment.NewLine;//TT780
							em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
						}
					}

					// =================================================
					// If the incoming size curve has already been
					// processed by earlier transactions, the size curve
					// is re-loaded in anticipation of being extended
					// =================================================
					if (defCurve)
					{
						if (defSizeCurveProfile != null)
						{
							tmpSizeCurveProfile = defSizeCurveProfile;

							foreach (SizeCodeProfile scp in tmpSizeCurveProfile.SizeCodeList)
							{
								sizeRID = scp.Key;
								sizeProfiles.Add(sizeRID, scp);
							}	 
						}
					}
					else
					{
						if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
						{
							tmpSizeCurveProfile = (SizeCurveProfile)curveProfiles[sizeCurveTran.SizeCurveName];

							foreach (SizeCodeProfile scp in tmpSizeCurveProfile.SizeCodeList)
							{
								sizeRID = scp.Key;
								sizeProfiles.Add(sizeRID, scp);
							}	 
						}
					}

					// ==========================
					// Edit the size transactions
					// ==========================
					if (sizeCurveTran.Size != null) foreach (SizeCurveGroupsSizeCurveGroupSizeCurveSize sizeTran in sizeCurveTran.Size)
					{
						skipSize = false;
						sizeError = false;

						sizeRID = Include.NoRID;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
						//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                        //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                        //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                        //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                        tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                        tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                        tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                        tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

 						//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
						//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
						//tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
						//tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "], ";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

						if (_codesPresent)
						{
                            tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup1);//TT780 
                            tranImg = tranImg.Replace("{0}", sizeTran.CodeID);//TT780 
                            //tranImg += "Code: [" + sizeTran.CodeID + "], ";//TT780 
						}
						else
						{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup2);
                            // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
							//tranImg = msgText.Replace("{0}", sizeCurveTran.ProductCategory);
                            //tranImg = msgText.Replace("{1}", sizeTran.Primary);
                            //tranImg = msgText.Replace("{2}", sizeTran.Secondary);
                            tranImg = tranImg.Replace("{0}", sizeCurveTran.ProductCategory);
                            tranImg = tranImg.Replace("{1}", sizeTran.Primary);
                            tranImg = tranImg.Replace("{2}", sizeTran.Secondary);
                            // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                            //tranImg += "Product Category: [" + sizeCurveTran.ProductCategory + "], ";
							//tranImg += "Primary: [" + sizeTran.Primary + "], ";
							//tranImg += "Secondary: [" + sizeTran.Secondary + "], ";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
						}

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup3);
                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
						//tranImg = msgText.Replace("{0}", sizeTran.SizeAction.ToString());
                        //tranImg = msgText.Replace("{1}", sizeTran.Value.ToString());
                        tranImg = tranImg.Replace("{0}", sizeTran.SizeAction.ToString());
                        tranImg = tranImg.Replace("{1}", sizeTran.Value.ToString());
                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

						//tranImg += "Size Action: [" + sizeTran.SizeAction.ToString() + "], ";
						//tranImg += "Value: [" + sizeTran.Value.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
						tranImg += System.Environment.NewLine + System.Environment.NewLine;
 
						// ====================
						// Edit the size action
						// ====================
						if (!sizeTran.SizeActionSpecified)
						{
                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_MissingDefaultToCreate);//TT780
                            msgText += System.Environment.NewLine;//TT780
                            //msgText = tranImg + "Message: Size Action is missing - defaulting to 'Create'" + System.Environment.NewLine;//TT780
							em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
						}
						else
						{
							if (sizeTran.SizeAction.ToString().ToUpper() != "CREATE")
							{
                                msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_InvalidDefaultToCreate);//TT780
                                msgText += System.Environment.NewLine;//TT780
                                //msgText = tranImg + "Message: Size Action is invalid - defaulting to 'Create'" + System.Environment.NewLine;//TT780
								em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
							}
						}

						// ==================
						// Edit the size code
						// ==================
						sizeCodeProfile = ValidateSize(tranImg,
													   sizeTran.CodeID,
													   sizeCurveTran.ProductCategory,
													   sizeTran.Primary,
													   sizeTran.Secondary,
													   ref em);

						if (sizeCodeProfile.Key == Include.NoRID)
						{
							sizeError = true;
						}
						else
						{
							foreach (SizeCodeProfile scp in allSizeProfiles.Values)
							{
								if ((sizeCodeProfile.SizeCodeID == scp.SizeCodeID) ||
									(sizeCodeProfile.SizeCodePrimary == scp.SizeCodePrimary &&
									sizeCodeProfile.SizeCodeSecondary == scp.SizeCodeSecondary &&
									sizeCodeProfile.SizeCodeProductCategory == scp.SizeCodeProductCategory))
								{
									if (sizeCodeProfile.Key != scp.Key)
									{
										skipSize = true;
                                        
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        msgText = MIDText.GetText(eMIDTextCode.msg_scglp_SizeProfiles);
                                        msgText = msgText.Replace("{0}", System.Environment.NewLine);
                                        msgText = msgText.Replace("{1}", scp.SizeCodeID);
                                        msgText = msgText.Replace("{2}", scp.SizeCodeProductCategory);
                                        msgText = msgText.Replace("{3}", scp.SizeCodePrimary);
                                        msgText = msgText.Replace("{4}", scp.SizeCodeSecondary);
                                        msgText += System.Environment.NewLine;

										//msgText = tranImg + "Message: Size already present with the following criteria" + System.Environment.NewLine;
										//msgText += "Code ID: [" + scp.SizeCodeID + "], ";
										//msgText += "Product Category: [" + scp.SizeCodeProductCategory + "], ";
										//msgText += "Primary: [" + scp.SizeCodePrimary + "], ";
										//msgText += "Secondary: [" + scp.SizeCodeSecondary + "]" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

                                        em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
										break;
									}
								}
							}

							if (!skipSize)
							{
								sizeRID = sizeCodeProfile.Key;
								//sizeCode = sizeCodeProfile.SizeCodeID;

								if (!sizeTran.ValueSpecified)
								{
									szVal = 0.0;
								}
								else
								{
									try
									{
										szVal = Convert.ToDouble(sizeTran.Value, CultureInfo.CurrentUICulture);
									}

									catch (InvalidCastException)
									{
										szVal = 0.0;

                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_InvalidCastException);//TT780
                                        msgText += System.Environment.NewLine;//TT780
                                        //msgText = tranImg + " Value has invalid value - setting to 0.00" + System.Environment.NewLine;//TT780
										em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
									}
								}

								sizeValue = EditSizeValue(tranImg, szVal, ref em);
							
								if (!sizeProfiles.ContainsKey(sizeRID))
								{
									sizeCodeProfile.SizeCodePercent = (float)sizeValue;
									sizeProfiles.Add(sizeRID, sizeCodeProfile);
								}
								else
								{
									sizeCodeProfile = (SizeCodeProfile)sizeProfiles[sizeRID];
									sizeCodeProfile.SizeCodePercent = (float)sizeValue;
									sizeProfiles[sizeRID] = sizeCodeProfile;
								}

								if (!allSizeProfiles.ContainsKey(sizeRID))
								{
									allSizeProfiles.Add(sizeRID, sizeCodeProfile);
								}
								else
								{
									sizeCodeProfile = (SizeCodeProfile)allSizeProfiles[sizeRID];
									sizeCodeProfile.SizeCodePercent = (float)sizeValue;
									allSizeProfiles[sizeRID] = sizeCodeProfile;
								}
							}
						}

						if (sizeError)
						{
							curveError = true;
						}
					}
					else
					{
						curveError = true;

                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeTransactionsFound);//TT780
                        msgText += System.Environment.NewLine;//TT780
                        //msgText = tranImg + "Message: No size transactions found to process" + System.Environment.NewLine;//TT780
						em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					}

					if (curveError)
					{
						editError = true;
					}
					else
					{
						if (sizeProfiles.Count > 0)
						{
							// =======================
							// Equalize values to 100%
							// =======================
							spreadOkay = EqualizeSizeValues(ref sizeProfiles);

							if (spreadOkay)
							{	 
								// ==========================
								// Build a size curve profile
								// ==========================
								if (defCurve)
								{
									if (defSizeCurveProfile != null)
									{
										tmpSizeCurveProfile.SizeCodeList.Clear();
									}
									else
									{
										tmpSizeCurveProfile = new SizeCurveProfile(Include.NoRID);
										tmpSizeCurveProfile.SizeCurveName = sizeCurveTran.SizeCurveName;
									}
								}
								else
								{
									if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
									{
										tmpSizeCurveProfile.SizeCodeList.Clear();
									}
									else
									{
										tmpSizeCurveProfile = new SizeCurveProfile(Include.NoRID);
										tmpSizeCurveProfile.SizeCurveName = sizeCurveTran.SizeCurveName;
									}
								}

								foreach (SizeCodeProfile sizeProfile in sizeProfiles.Values)
								{
									sizeCodeProfileClone = (SizeCodeProfile)sizeProfile.Clone();
									tmpSizeCurveProfile.SizeCodeList.Add(sizeCodeProfileClone);
								}

								if (defCurve)
								{
									defSizeCurveProfile = tmpSizeCurveProfile;
								}
								else
								{
									if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
									{
										curveProfiles[sizeCurveTran.SizeCurveName] = tmpSizeCurveProfile;
									}
									else
									{
										curveProfiles.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);
									}
								}
							}	 
							else
							{
								editError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
								//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
								//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
								//tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
								//tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                tranImg += System.Environment.NewLine + System.Environment.NewLine;

                                msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_EqualizingSize);//TT780
                                msgText += System.Environment.NewLine;//TT780
                                //msgText = tranImg + "Message: Error equalizing size values to 100%" + System.Environment.NewLine;//TT780
								em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
							}
						}
						else
						{
							editError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                            // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
							//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                            //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                            //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                            //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                            tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                            tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                            tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                            tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                            // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

							//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
							//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
							//tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
							//tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
							tranImg += System.Environment.NewLine + System.Environment.NewLine;

                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeToProcess);//TT780
                            msgText += System.Environment.NewLine;//TT780
                            //msgText = tranImg + "Message: No size transactions found to process" + System.Environment.NewLine;//TT780
							em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
						}
					}
				}

				if (!editError)
				{
					if (defSizeCurveProfile != null)
					{
						// ====================================================
						// Verify that the default size curve contains an entry
						// for all size codes and add each entry when required
						// ====================================================
						foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
						{
							if (!defSizeCurveProfile.SizeCodeList.Contains(allSizeProfile.Key))
							{
								sizeCodeProfileClone = (SizeCodeProfile)allSizeProfile.Clone();
								sizeCodeProfileClone.SizeCodePercent = (float)sizeValueZero;
								defSizeCurveProfile.SizeCodeList.Add(sizeCodeProfileClone);
							}
						}
					}
					else
					{
						// =======================================================================
						// Create a default size curve, set all values to 1.0 and equalize to 100%
						// =======================================================================
						foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
						{
							allSizeProfile.SizeCodePercent = (float)sizeValueOne;
						}

						spreadOkay = EqualizeSizeValues(ref allSizeProfiles);

						if (spreadOkay)
						{	// BEGIN MID Track #5268  - Size Curve Add/Update slow
							//tmpSizeCurveProfile = new SizeCurveProfile(Include.NoRID);
							//foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
							//{
							//	sizeCodeProfileClone = (SizeCodeProfile)allSizeProfile.Clone();
							//	tmpSizeCurveProfile.SizeCodeList.Add(sizeCodeProfileClone);
							//}
							//defSizeCurveProfile = tmpSizeCurveProfile;
							
							defSizeCurveProfile = new SizeCurveProfile(Include.NoRID);
							foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
							{
								defSizeCurveProfile.SizeCodeList.Add(allSizeProfile);
							}
						}	// END MID Track #5268
						else
						{
							editError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveGroupAction);
                            // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
							//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                            //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                            tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                            tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                            // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

							//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
							//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
							tranImg += System.Environment.NewLine + System.Environment.NewLine;

                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_ErrorEqualizingDefault);//TT780
                            msgText += System.Environment.NewLine;//TT780
                            //msgText = tranImg + "Message: Error equalizing default curve size values to 100%" + System.Environment.NewLine;//TT780
							em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
						}
					}

					if (!editError)
					{
						// ==========================================
						// Build a size curve group profile and write
						// ==========================================
						sizeCurveGroupProfile.SizeCurveGroupName = sizeCurveGroupTran.SizeCurveGroupName;
						sizeCurveGroupProfile.DefinedSizeGroupRID = Include.NoRID;
						sizeCurveGroupProfile.DefaultSizeCurveRid = Include.NoRID;
						sizeCurveGroupProfile.DefaultSizeCurve = defSizeCurveProfile;
						sizeCurveGroupProfile.SizeCurveList.Clear();

						if (curveProfiles.Count > 0)
						{
							// ====================================================
							// Newly created size curves must be written here since
							// the RID is required when adding to the group list
							// ====================================================
							try
							{
								foreach (SizeCurveProfile curveProfile in curveProfiles.Values)
								{
									curveProfile.WriteSizeCurve(false, _SAB);
									// BEGIN MID Track #5268 - RonM - Size Curve Add/Update slow
									sizeCurveGroupProfile.SizeCurveList.Add(curveProfile);
									//_curvesInGroupUpdated = true;	 
								}
								_curvesInGroupUpdated = true;
							}	// END MID Track #5268 
	
							catch (Exception Ex)
							{
								createOkay = false;

								exceptMsg = Ex.ToString();
							}
							// BEGIN MID Track #5268 - RonM - Size Curve Add/Update slow; moved above 
							//if (createOkay)
							//{
							//	foreach (SizeCurveProfile curveProfile in curveProfiles.Values)
							//	{
							//		sizeCurveGroupProfile.SizeCurveList.Add(curveProfile);
							//	}
							//}
						}	// END MID Track #5268

						if (createOkay)
						{
							try
							{	// BEGIN MID Track #5268 - Size Curve Add/Update slow 
								//sizeCurveGroupProfile.WriteSizeCurveGroup(_SAB);
								sizeCurveGroupProfile.WriteSizeCurveGroup(_SAB, _curvesInGroupUpdated);
							}	// END MID Track #5268

							catch (Exception Ex)
							{
								createOkay = false;

								exceptMsg = Ex.ToString();
							}
						}
					}
				}
			}

			catch (Exception Ex)
			{
				createOkay = false;

				_audit.Log_Exception(Ex, GetType().Name);
			}

			finally
			{
				if (createOkay)
				{
					if (!editError)
					{
                        
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText = MIDText.GetText(eMIDTextCode.msg_scglp_TransactionSuccessfullyCreated);
                        msgText = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                        msgText = msgText.Replace("{1}", _groupsRead.ToString());
                        msgText += System.Environment.NewLine;

                        //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
						//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
						//msgText += " successfully created" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
						em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

						WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), true);

						rtnCode = eReturnCode.successful;
					}
					else
					{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText = MIDText.GetText(eMIDTextCode.msg_scglp_TransactionEncounteredEeditErrors);
                        msgText = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                        msgText = msgText.Replace("{1}", _groupsRead.ToString());
                        msgText += System.Environment.NewLine;

                        //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
                        //msgText += " (Transaction #" + _groupsRead.ToString() + ")";
                        //msgText += " encountered edit errors" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
 						em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

						WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

						rtnCode = eReturnCode.editErrors;
					}
				}
				else
				{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText  = MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors);
                        msgText  = msgText.Replace("{0}",sizeCurveGroupTran.SizeCurveGroupName);
                        msgText  = msgText.Replace("{1}", _groupsRead.ToString());

                    //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

  					if (exceptMsg == null)
					{
						msgText += MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors1); //TT780
                        msgText +=  System.Environment.NewLine;//TT780
                        //msgText += " encountered an update error" + System.Environment.NewLine;//TT780
					}
					else
					{
						msgText += MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors2); //TT780
                        msgText += exceptMsg + System.Environment.NewLine;		//TT780				
                        //msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;//TT780
					}
					em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

					WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

					rtnCode = eReturnCode.severe;
				}

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// =========================
		// Modify a size curve group
		// =========================
		public eReturnCode ModifyGroup(SizeCurveGroupsSizeCurveGroup sizeCurveGroupTran, SizeCurveGroupProfile sizeCurveGroupProfile)
		{
			EditMsgs em = null;

			double szVal = 0.0;
			double sizeValue = 0.0;

			string msgText = null;
			string tranImg = null;
			string exceptMsg = null;

			bool defCurve = false;
			bool skipSize = false;
			bool editError = false;
			bool sizeError = false;
			bool modifyOkay = true;
			bool spreadOkay = true;
			bool curveError = false;
// (CSMITH) - BEG MID Track #3173: Remove Curve from Group causes FK violation
			bool curveFound = false;
// (CSMITH) - END MID Track #3173:

			float sizeValueZero = 0.0F;
		
			Hashtable sizeProfiles = null;						// Size Code Profiles keyed by Size Code RID
			Hashtable curveProfiles = null;						// Size Curve Profiles keyed by Size Curve Name
			Hashtable curvesCreated = null;						// Size Curve Profiles Created keyed by Size Curve Name
			Hashtable curvesCurrent = null;						// Current Size Curve Profiles keyed by Size Curve Name
			Hashtable curvesRemoved = null;						// Size Curve Profiles Removed keyed by Size Curve Name
			Hashtable allSizeProfiles = null;					// Size Code Profiles keyed by Size Code RID (for default)
// (CSMITH) - BEG MID Track #3173: Remove Curve from Group causes FK violation
			Hashtable strSizeCurveHash = null;					// Size Curve RIDs keyed by Store RID
// (CSMITH) - END MID Track #3173:

			int sizeRID = Include.NoRID;
			int defCurveRID = Include.NoRID;
			SizeCodeProfile sizeCodeProfile = null;
			SizeCodeProfile sizeCodeProfileClone = null;

			SizeCurveProfile defSizeCurveProfile = null;
// (CSMITH) - BEG MID Track #3173: Remove Curve from Group causes FK violation
			SizeCurveProfile strSizeCurveProfile = null;
// (CSMITH) - END MID Track #3173:
			SizeCurveProfile tmpSizeCurveProfile = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{	
				sizeProfiles = new Hashtable();

				curveProfiles = new Hashtable();
				curveProfiles.Clear();

				curvesCreated = new Hashtable();
				curvesCreated.Clear();

				curvesCurrent = new Hashtable();
				curvesCurrent.Clear();

				curvesRemoved = new Hashtable();
				curvesRemoved.Clear();

				allSizeProfiles = new Hashtable();
				allSizeProfiles.Clear();

				defCurveRID = sizeCurveGroupProfile.DefaultSizeCurveRid;
				defSizeCurveProfile = sizeCurveGroupProfile.DefaultSizeCurve;

                // Begin TT#1366 - JSmith - Object reference error after removing all size curves with stored procedure
                //foreach (SizeCodeProfile scp in defSizeCurveProfile.SizeCodeList)
                //{
                //    allSizeProfiles.Add(scp.Key, scp);
                //}
                if (defSizeCurveProfile != null)
                {
                    foreach (SizeCodeProfile scp in defSizeCurveProfile.SizeCodeList)
                    {
                        allSizeProfiles.Add(scp.Key, scp);
                    }
                }
                // End TT#1366

				foreach (SizeCurveProfile sizeCurveProfile in sizeCurveGroupProfile.SizeCurveList)
				{
					curveProfiles.Add(sizeCurveProfile.SizeCurveName, sizeCurveProfile);
					curvesCurrent.Add(sizeCurveProfile.SizeCurveName, sizeCurveProfile);
				}
				
				// ================================
				// Edit the size curve transactions
				// ================================
				foreach (SizeCurveGroupsSizeCurveGroupSizeCurve sizeCurveTran in sizeCurveGroupTran.SizeCurve)
				{
					defCurve = false;
					curveError = false;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
					//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                    //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                    //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                    tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                    tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                    //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
					//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
					//tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
					//tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
					tranImg += System.Environment.NewLine + System.Environment.NewLine;

					// ========================
					// Edit the size curve name
					// ========================
					defCurve = (sizeCurveTran.SizeCurveName == "" || sizeCurveTran.SizeCurveName == null) ? true : false;

					// ==========================
					// Edit the size curve action
					// ==========================
					if (!sizeCurveTran.SizeCurveActionSpecified)
					{
						curveError = true;

                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveActionRequired);//TT780
                        msgText += System.Environment.NewLine;//TT780
                        //msgText = tranImg + "Message: Size Curve Action is required" + System.Environment.NewLine;//TT780
						em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					}
					else
					{
						if (sizeCurveTran.SizeCurveAction.ToString().ToUpper() != "CREATE" &&
							sizeCurveTran.SizeCurveAction.ToString().ToUpper() != "MODIFY" &&
							sizeCurveTran.SizeCurveAction.ToString().ToUpper() != "REMOVE")
						{
							curveError = true;

                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveActionInvalid2);//TT780
                            msgText += System.Environment.NewLine;//TT780
                            //msgText = tranImg + "Message: Size Curve Action is invalid" + System.Environment.NewLine;//TT780
							em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
						}
					}

					if (!curveError)
					{
						// BEGIN MID Track #5153 - JSmith - Create curve on modify if not found
						if (defCurveRID == Include.NoRID)
						{
							if (_createOnModify 
								&& sizeCurveTran.SizeCurveAction == SizeCurveGroupsSizeCurveGroupSizeCurveSizeCurveAction.Modify)
							{
								sizeCurveTran.SizeCurveAction = SizeCurveGroupsSizeCurveGroupSizeCurveSizeCurveAction.Create;
							}
						}
						// END MID Track #5153
						// BEGIN MID Track #5278  - RonM - ANF Defect 1336 Size Curve Total < 100%
						else if (!defCurve && !curvesCurrent.ContainsKey(sizeCurveTran.SizeCurveName))
						{
							if (_createOnModify 
								&& sizeCurveTran.SizeCurveAction == SizeCurveGroupsSizeCurveGroupSizeCurveSizeCurveAction.Modify)
							{
								sizeCurveTran.SizeCurveAction = SizeCurveGroupsSizeCurveGroupSizeCurveSizeCurveAction.Create;
							}
						}
						// END MID Track #5278  

						if (sizeCurveTran.SizeCurveAction.ToString().ToUpper() == "CREATE")
						{
							// ===================
							// Create a size curve
							// ===================
							if (defCurve)
							{
								if (defCurveRID != Include.NoRID)
								{
									curveError = true;

                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_DefaultSizeCurveAlreadyDefined);//TT780
                                    msgText += System.Environment.NewLine;//TT780
                                    //msgText = tranImg + "Message: Action invalid when default Size Curve already defined" + System.Environment.NewLine;//TT780
									em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}
							}
							else
							{
								if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
								{
									curveError = true;

                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveAlreadyDefined);//TT780
                                    msgText += System.Environment.NewLine;//TT780
                                    //msgText = tranImg + "Message: Action invalid when Size Curve already defined" + System.Environment.NewLine;//TT780
									em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}
								else
								{
									// ========================================================
									// If a size curve was removed by an earlier transaction,
									// it MUST be taken out of the REMOVE list, placed back
									// into the curve list and a notification message generated
									// ========================================================
									if (curvesRemoved.ContainsKey(sizeCurveTran.SizeCurveName))
									{
										tmpSizeCurveProfile = (SizeCurveProfile)curvesRemoved[sizeCurveTran.SizeCurveName];

										curvesRemoved.Remove(sizeCurveTran.SizeCurveName);

										curveProfiles.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);

										if (!curvesCurrent.ContainsKey(sizeCurveTran.SizeCurveName))
										{
											curvesCreated.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);
										}

                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_RestoredFromREMOVEList);//TT780
                                        msgText += System.Environment.NewLine;//TT780                                
                                        //msgText = tranImg + "Message: Size Curve removed by earlier transaction - restored from REMOVE list" + System.Environment.NewLine;//TT780
										em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
									
										sizeProfiles.Clear();

										foreach (SizeCodeProfile scp in tmpSizeCurveProfile.SizeCodeList)
										{
											sizeRID = scp.Key;
											sizeProfiles.Add(sizeRID, scp);
										}
									}
								}
							}

							if (!curveError)
							{
								if (!curvesCreated.ContainsKey(sizeCurveTran.SizeCurveName))
								{	
									sizeProfiles.Clear();
								}

								// ==========================
								// Edit the size transactions
								// ==========================
								if (sizeCurveTran.Size != null) foreach (SizeCurveGroupsSizeCurveGroupSizeCurveSize sizeTran in sizeCurveTran.Size)
								{
									skipSize = false;
									sizeError = false;

									sizeRID = Include.NoRID;
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
									//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

									//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
									//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
									//tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
									//tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "], ";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

									if (_codesPresent)
									{
                                        tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup1);//TT780 
                                        tranImg = tranImg.Replace("{0}", sizeTran.CodeID);//TT780 
                                        //tranImg += "Code: [" + sizeTran.CodeID + "], ";//TT780
									}
									else
									{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup2);
                                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
										//tranImg = msgText.Replace("{0}", sizeCurveTran.ProductCategory);
                                        //tranImg = msgText.Replace("{1}", sizeTran.Primary);
                                        //tranImg = msgText.Replace("{2}", sizeTran.Secondary);
                                        tranImg = tranImg.Replace("{0}", sizeCurveTran.ProductCategory);
                                        tranImg = tranImg.Replace("{1}", sizeTran.Primary);
                                        tranImg = tranImg.Replace("{2}", sizeTran.Secondary);
                                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                        //tranImg += "Product Category: [" + sizeCurveTran.ProductCategory + "], ";
                                        //tranImg += "Primary: [" + sizeTran.Primary + "], ";
                                        //tranImg += "Secondary: [" + sizeTran.Secondary + "], ";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
									}
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup3);
                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
									//tranImg = msgText.Replace("{0}", sizeTran.SizeAction.ToString());
                                    //tranImg = msgText.Replace("{1}", sizeTran.Value.ToString());
                                    tranImg = tranImg.Replace("{0}", sizeTran.SizeAction.ToString());
                                    tranImg = tranImg.Replace("{1}", sizeTran.Value.ToString());
                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                    //tranImg += "Size Action: [" + sizeTran.SizeAction.ToString() + "], ";
                                    //tranImg += "Value: [" + sizeTran.Value.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
									tranImg += System.Environment.NewLine + System.Environment.NewLine;

									// ====================
									// Edit the size action
									// ====================
									if (!sizeTran.SizeActionSpecified)
									{
                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_MissingDefaultToCreate);//TT780
                                        msgText += System.Environment.NewLine;//TT780

                                        //msgText = tranImg + "Message: Size Action is missing - defaulting to 'Create'" + System.Environment.NewLine;//TT780
										//Begin TT#757 - JScott - Change "Create On Modify" information from Edit to Information
										//em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
										em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
										//End TT#757 - JScott - Change "Create On Modify" information from Edit to Information
									}
									else
									{
										if (sizeTran.SizeAction.ToString().ToUpper() != "CREATE")
										{
                                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_InvalidDefaultToCreate);//TT780
                                            msgText += System.Environment.NewLine;//TT780

                                            //msgText = tranImg + "Message: Size Action is invalid - defaulting to 'Create'" + System.Environment.NewLine;//TT780
											//Begin TT#757 - JScott - Change "Create On Modify" information from Edit to Information
											//em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
											em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
											//End TT#757 - JScott - Change "Create On Modify" information from Edit to Information
										}
									}

									// ==================
									// Edit the size code
									// ==================
									sizeCodeProfile = ValidateSize(tranImg,
																   sizeTran.CodeID,
																   sizeCurveTran.ProductCategory,
																   sizeTran.Primary, sizeTran.Secondary,
																   ref em);

									if (sizeCodeProfile.Key == Include.NoRID)
									{
										sizeError = true;
									}
									else
									{
										foreach (SizeCodeProfile scp in allSizeProfiles.Values)
										{
											if ((sizeCodeProfile.SizeCodeID == scp.SizeCodeID) ||
												(sizeCodeProfile.SizeCodePrimary == scp.SizeCodePrimary &&
												sizeCodeProfile.SizeCodeSecondary == scp.SizeCodeSecondary &&
												sizeCodeProfile.SizeCodeProductCategory == scp.SizeCodeProductCategory))
											{
												if (sizeCodeProfile.Key != scp.Key)
												{
													skipSize = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
													//msgText = MIDText.GetText(eMIDTextCode.msg_scglp_SizeProfiles2);
                                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeProfiles2);
                                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                                                    msgText = msgText.Replace("{0}", System.Environment.NewLine);
                                                    msgText = msgText.Replace("{1}", scp.SizeCodeID);
                                                    msgText = msgText.Replace("{2}", scp.SizeCodeProductCategory);
                                                    msgText = msgText.Replace("{3}", scp.SizeCodePrimary);
                                                    msgText = msgText.Replace("{4}", scp.SizeCodeSecondary);
                                                    msgText += System.Environment.NewLine;

                                                    //msgText = tranImg + "Message: Size already selected with the following criteria" + System.Environment.NewLine;
                                                    //msgText += "Code ID: [" + scp.SizeCodeID + "], ";
                                                    //msgText += "Product Category: [" + scp.SizeCodeProductCategory + "], ";
                                                    //msgText += "Primary: [" + scp.SizeCodePrimary + "], ";
                                                    //msgText += "Secondary: [" + scp.SizeCodeSecondary + "]" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

 													em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
													break;
												}
											}
										}

										if (!skipSize)
										{
											sizeRID = sizeCodeProfile.Key;
										
											if (!sizeTran.ValueSpecified)
											{
												szVal = 0.0;
											}
											else
											{
												try
												{
													szVal = Convert.ToDouble(sizeTran.Value, CultureInfo.CurrentUICulture);
												}

												catch (InvalidCastException)
												{
													szVal = 0.0;

                                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_InvalidCastException);//TT780
                                                    msgText += System.Environment.NewLine;//TT780
                                                    //msgText = tranImg + " Value has invalid value - setting to 0.00" + System.Environment.NewLine;//TT780
													em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
												}
											}

											sizeValue = EditSizeValue(tranImg, szVal, ref em);
											
											if (!sizeProfiles.ContainsKey(sizeRID))
											{
												sizeCodeProfile.SizeCodePercent = (float)sizeValue;
												sizeProfiles.Add(sizeRID, sizeCodeProfile);
											}
											else
											{
												sizeCodeProfile = (SizeCodeProfile)sizeProfiles[sizeRID];
												sizeCodeProfile.SizeCodePercent = (float)sizeValue;
												sizeProfiles[sizeRID] = sizeCodeProfile;
											}
											//Begin TT#1366 - JScott - Object reference error after removing all size curves with stored procedure

											if (!allSizeProfiles.ContainsKey(sizeRID))
											{
												allSizeProfiles.Add(sizeRID, sizeCodeProfile);
											}
											else
											{
												sizeCodeProfile = (SizeCodeProfile)allSizeProfiles[sizeRID];
												sizeCodeProfile.SizeCodePercent = (float)sizeValue;
												allSizeProfiles[sizeRID] = sizeCodeProfile;
											}
											//End TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
										}
									}

									if (sizeError)
									{
										curveError = true;
									}
								}
								else
								{
									curveError = true;
                                    
                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeToProcess);//TT780
                                    msgText += System.Environment.NewLine;//TT780
 
									//msgText = tranImg + "Message: No size transactions found to process" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}
							}

							if (curveError)
							{
								editError = true;
							}
							else
							{
								if (sizeProfiles.Count > 0)
								{
									// =======================
									// Equalize values to 100%
									// =======================
									spreadOkay = EqualizeSizeValues(ref sizeProfiles);

									if (spreadOkay)
									{
										// ==========================
										// Build a size curve profile
										// ==========================
										tmpSizeCurveProfile = new SizeCurveProfile(Include.NoRID);
										tmpSizeCurveProfile.SizeCurveName = sizeCurveTran.SizeCurveName;

										foreach (SizeCodeProfile sizeProfile in sizeProfiles.Values)
										{
											sizeCodeProfileClone = (SizeCodeProfile)sizeProfile.Clone();
											tmpSizeCurveProfile.SizeCodeList.Add(sizeCodeProfileClone);
										}

										if (defCurve)
										{
											defSizeCurveProfile = tmpSizeCurveProfile;
										}
										else
										{
											if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
											{
												curveProfiles[sizeCurveTran.SizeCurveName] = tmpSizeCurveProfile;
											}
											else
											{
												curveProfiles.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);
											}

											if (curvesCreated.ContainsKey(sizeCurveTran.SizeCurveName))
											{
												curvesCreated[sizeCurveTran.SizeCurveName] = tmpSizeCurveProfile;
											}
											else
											{
												curvesCreated.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);
											}
										}
									}
									else
									{
										editError = true;
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
										//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                        //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                        //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                        //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                        tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                        tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                        tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                        tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                        //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
                                        //tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
                                        //tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
                                        //tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
 										tranImg += System.Environment.NewLine + System.Environment.NewLine;

										msgText = tranImg + "Message: Error equalizing size values to 100%" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
									}
								}
								else
								{
									editError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
									//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                    //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
                                    //tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
                                    //tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
                                    //tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
 									tranImg += System.Environment.NewLine + System.Environment.NewLine;


                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeToProcess);//TT780
                                    msgText += System.Environment.NewLine;//TT780
                                    //msgText = tranImg + "Message: No size transactions found to process" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
								}
							}
						}
						else if (sizeCurveTran.SizeCurveAction.ToString().ToUpper() == "MODIFY")
						{
							// ===================
							// Modify a size curve
							// ===================
							
							sizeProfiles.Clear();

							if (defCurve)
							{
								if (defCurveRID != Include.NoRID)
								{
									foreach (SizeCodeProfile scp in defSizeCurveProfile.SizeCodeList)
									{
										sizeRID = scp.Key;
										sizeProfiles.Add(sizeRID, scp);
									}
								}
								else
								{
									curveError = true;

                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_DefaultSizeCurveNotDefined);//TT780
                                    msgText += System.Environment.NewLine;//TT780
									//msgText = tranImg + "Message: Action invalid when default Size Curve is NOT defined" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}
							}
							else
							{
								if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
								{
									tmpSizeCurveProfile = (SizeCurveProfile)curveProfiles[sizeCurveTran.SizeCurveName];
									foreach (SizeCodeProfile scp in tmpSizeCurveProfile.SizeCodeList)
									{
										sizeRID = scp.Key;
										sizeProfiles.Add(sizeRID, scp);
									}
								}
								else
								{
									// ======================================================
									// If a size curve was removed by an earlier transaction,
									// it MUST be taken out of the REMOVE list, placed back
									// into the curve list, a notification message generated
									// and then processed normally
									// ======================================================
									if (curvesRemoved.ContainsKey(sizeCurveTran.SizeCurveName))
									{
										tmpSizeCurveProfile = (SizeCurveProfile)curvesRemoved[sizeCurveTran.SizeCurveName];
										curvesRemoved.Remove(sizeCurveTran.SizeCurveName);
										curveProfiles.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);

                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_RestoredFromREMOVEList);//TT780
                                        msgText += System.Environment.NewLine;//TT780
										//msgText = tranImg + "Message: Size Curve removed by earlier transaction - restored from REMOVE list" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

										foreach (SizeCodeProfile scp in tmpSizeCurveProfile.SizeCodeList)
										{
											sizeRID = scp.Key;
											sizeProfiles.Add(sizeRID, scp);
										}	 
									}
									else
									{
										curveError = true;
                                        
                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveNotDefined);//TT780
                                        msgText += System.Environment.NewLine;//TT780
										//msgText = tranImg + "Message: Action invalid when Size Curve is NOT defined" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
									}
								}
							}

							if (!curveError)
							{
								if (sizeCurveTran.Size != null) foreach (SizeCurveGroupsSizeCurveGroupSizeCurveSize sizeTran in sizeCurveTran.Size)
								{
									// ==========================
									// Edit the size transactions
									// ==========================
									sizeError = false;
                                    
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
									//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                    //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
                                    //tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
                                    //tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
                                    //tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "], ";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
 
									if (_codesPresent)
									{
										tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup1);//TT780
                                        tranImg = tranImg.Replace("{0}", sizeTran.CodeID);//TT780
                                        //tranImg += "Code: [" + sizeTran.CodeID + "], ";//TT780
									}
									else
									{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        tranImg += MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup2);
                                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
										//tranImg = msgText.Replace("{0}", sizeCurveTran.ProductCategory);
                                        //tranImg = msgText.Replace("{1}", sizeTran.Primary);
                                        //tranImg = msgText.Replace("{2}", sizeTran.Secondary); 
                                        tranImg = tranImg.Replace("{0}", sizeCurveTran.ProductCategory);
                                        tranImg = tranImg.Replace("{1}", sizeTran.Primary);
                                        tranImg = tranImg.Replace("{2}", sizeTran.Secondary);
                                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                        //tranImg += "Product Category: [" + sizeCurveTran.ProductCategory + "], ";
										//tranImg += "Primary: [" + sizeTran.Primary + "], ";
										//tranImg += "Secondary: [" + sizeTran.Secondary + "], ";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
									}

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup3);
                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
									//tranImg = msgText.Replace("{0}", sizeTran.SizeAction.ToString());
                                    //tranImg = msgText.Replace("{1}", sizeTran.Value.ToString());
                                    tranImg = tranImg.Replace("{0}", sizeTran.SizeAction.ToString());
                                    tranImg = tranImg.Replace("{1}", sizeTran.Value.ToString());
                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                                    tranImg += System.Environment.NewLine;//TT780;

                                    //tranImg += "Size Action: [" + sizeTran.SizeAction.ToString() + "], ";
									//tranImg += "Value: [" + sizeTran.Value.ToString() + "]";
									//tranImg += System.Environment.NewLine + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

									// ====================
									// Edit the size action
									// ====================
									if (!sizeTran.SizeActionSpecified)
									{
										sizeError = true;

										msgText  = tranImg +  MIDText.GetText(eMIDTextCode.msg_scglp_SizeActionRequired);
                                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
										//tranImg += System.Environment.NewLine;//TT780;
                                        msgText += System.Environment.NewLine;//TT780;
                                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                                        //msgText  = tranImg + "Message: Size Action is required" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
									}
									else
									{
										if (sizeTran.SizeAction.ToString().ToUpper() != "CREATE" &&
											sizeTran.SizeAction.ToString().ToUpper() != "MODIFY" &&
											sizeTran.SizeAction.ToString().ToUpper() != "REMOVE")
										{
											sizeError = true;

                                            msgText  = tranImg +  MIDText.GetText(eMIDTextCode.msg_scglp_SizeActionInvalid);
                                            // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
											//tranImg += System.Environment.NewLine;//TT780;
                                            msgText += System.Environment.NewLine;//TT780;
                                            // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
											//msgText = tranImg + "Message: Size Action is invalid" + System.Environment.NewLine;
											em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
										}
									}

									if (!sizeError)
									{
										// ==================
										// Edit the size code
										// ==================
										sizeCodeProfile = ValidateSize(tranImg,
																	   sizeTran.CodeID,
																	   sizeCurveTran.ProductCategory,
																	   sizeTran.Primary,
																	   sizeTran.Secondary,
																	   ref em);

										if (sizeCodeProfile.Key == Include.NoRID)
										{
											sizeError = true;
										}
										else
										{
											foreach (SizeCodeProfile scp in allSizeProfiles.Values)
											{
												if ((sizeCodeProfile.SizeCodeID == scp.SizeCodeID) ||
													(sizeCodeProfile.SizeCodePrimary == scp.SizeCodePrimary &&
													sizeCodeProfile.SizeCodeSecondary == scp.SizeCodeSecondary &&
													sizeCodeProfile.SizeCodeProductCategory == scp.SizeCodeProductCategory))
												{
													if (sizeCodeProfile.Key != scp.Key)
													{
														skipSize = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
														//msgText = MIDText.GetText(eMIDTextCode.msg_scglp_SizeProfiles2);
                                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_SizeProfiles2);
                                                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                                                        msgText = msgText.Replace("{0}", System.Environment.NewLine);
                                                        msgText = msgText.Replace("{1}", scp.SizeCodeID);
                                                        msgText = msgText.Replace("{2}", scp.SizeCodeProductCategory);
                                                        msgText = msgText.Replace("{3}", scp.SizeCodePrimary);
                                                        msgText = msgText.Replace("{4}", scp.SizeCodeSecondary);
                                                        msgText += System.Environment.NewLine;

                                                        //msgText = tranImg + "Message: Size already selected with the following criteria" + System.Environment.NewLine;
                                                        //msgText += "Code ID: [" + scp.SizeCodeID + "], ";
                                                        //msgText += "Product Category: [" + scp.SizeCodeProductCategory + "], ";
                                                        //msgText += "Primary: [" + scp.SizeCodePrimary + "], ";
                                                        //msgText += "Secondary: [" + scp.SizeCodeSecondary + "]" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
														
														em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
														break;
													}
												}
											}

											if (!skipSize)
											{
												sizeRID = sizeCodeProfile.Key;
												if (!sizeTran.ValueSpecified)
												{
													szVal = 0.0;
												}
												else
												{
													try
													{
														szVal = Convert.ToDouble(sizeTran.Value, CultureInfo.CurrentUICulture);
													}

													catch (InvalidCastException)
													{
														szVal = 0.0;

                                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_InvalidCastException);//TT780
                                                        msgText += System.Environment.NewLine;//TT780
                                                        //msgText = tranImg + " Value has invalid value - setting to 0.00" + System.Environment.NewLine;//TT780
														em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
													}
												}

												sizeValue = EditSizeValue(tranImg, szVal, ref em);

												if (sizeTran.SizeAction.ToString().ToUpper() == "CREATE" ||
													sizeTran.SizeAction.ToString().ToUpper() == "MODIFY")
												{
													// ====================================
													// Create or Modify a size code profile
													// ====================================
													if (!sizeProfiles.ContainsKey(sizeRID))
													{
														sizeCodeProfile.SizeCodePercent = (float)sizeValue;
														sizeProfiles.Add(sizeRID, sizeCodeProfile);
													}
													else
													{
														sizeCodeProfile = (SizeCodeProfile)sizeProfiles[sizeRID];
														sizeCodeProfile.SizeCodePercent = (float)sizeValue;
														sizeProfiles[sizeRID] = sizeCodeProfile;
													}
												}
												else
												{
													// ==========================
													// Remove a size code profile
													// ==========================
													if (defCurve)
													{
														sizeCodeProfile = (SizeCodeProfile)sizeProfiles[sizeRID];
														sizeCodeProfile.SizeCodePercent = (float)sizeValueZero;
														sizeProfiles[sizeRID] = sizeCodeProfile;
													}
													else if (sizeProfiles.ContainsKey(sizeRID))
													{	
														sizeProfiles.Remove(sizeRID);
													}
												}
											}
										}
									}

									if (sizeError)
									{
										curveError = true;
									}
								}
								else
								{
									curveError = true;

                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_NoSizeToProcess);//TT780
                                    msgText += System.Environment.NewLine;//TT780
                                    //msgText = tranImg + "Message: No size transactions found to process" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}

							}

							if (curveError)
							{
								editError = true;
							}
							else
							{
								if (sizeProfiles.Count > 0)
								{
									// =======================
									// Equalize values to 100%
									// =======================
									spreadOkay = EqualizeSizeValues(ref sizeProfiles);

									if (spreadOkay)
									{
										// ==========================
										// Build a size curve profile
										// ==========================
										tmpSizeCurveProfile = new SizeCurveProfile(Include.NoRID);

										foreach (SizeCodeProfile sizeProfile in sizeProfiles.Values)
										{
											sizeCodeProfileClone = (SizeCodeProfile)sizeProfile.Clone();
											tmpSizeCurveProfile.SizeCodeList.Add(sizeCodeProfileClone);
										}

										if (defCurve)
										{
											defSizeCurveProfile = tmpSizeCurveProfile;
										}
										else
										{	// BEGIN MID Track #5278  - RonM - ANF Defect 1336 Size Curve Total < 100%
											tmpSizeCurveProfile.SizeCurveName = sizeCurveTran.SizeCurveName;
											// END MID Track #5278  
											if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
											{
												// BEGIN MID Track 4257 - Getting error accessing API loaded size curves;
												//	 the profile Key is getting overlaid with -1 when the curveProfile 
												//   is replaced with tmpSizeCurveProfile resulting in another duplicate curve
												//	 being added later on. Fix is to retain current Profile Key 	  	
												SizeCurveProfile scp = (SizeCurveProfile)curveProfiles[sizeCurveTran.SizeCurveName];
												tmpSizeCurveProfile.Key = scp.Key; 
												// END MID Track 4257
												curveProfiles[sizeCurveTran.SizeCurveName] = tmpSizeCurveProfile;
											}
											else
											{
												curveProfiles.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);
											}
										}
									}
									else
									{
										editError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                        // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
										//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                        //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                        //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                        //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                        tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                        tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                        tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                        tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                        // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                        //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
                                        //tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
                                        //tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
                                        //tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
 										tranImg += System.Environment.NewLine + System.Environment.NewLine;

                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_EqualizingSize);//TT780
                                        msgText += System.Environment.NewLine;//TT780
										//msgText = tranImg + "Message: Error equalizing size values to 100%" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
									}
								}
								else
								{
									curveProfiles.Remove(sizeCurveTran.SizeCurveName);

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_BeginProcessingCreateGroup);
                                    // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
									//tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    //tranImg = msgText.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    //tranImg = msgText.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                                    tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                                    tranImg = tranImg.Replace("{2}", sizeCurveTran.SizeCurveName);
                                    tranImg = tranImg.Replace("{3}", sizeCurveTran.SizeCurveAction.ToString());
                                    // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

                                    //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
                                    //tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "], ";
                                    //tranImg += "Size Curve Name: [" + sizeCurveTran.SizeCurveName + "], ";
                                    //tranImg += "Size Curve Action: [" + sizeCurveTran.SizeCurveAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_AllSizesRemoved);//TT780
                                    msgText += System.Environment.NewLine;//TT780
                                    //msgText = tranImg + "Message: All sizes removed from Size Curve - Size Curve removed from Size Curve Group" + System.Environment.NewLine;//TT780
									em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
								}
							}
						}
						else
						{
							// ===================
							// Remove a size curve
							// ===================
							if (defCurve)
							{
								curveError = true;

								msgText = tranImg + "Message: Default Size Curve can NOT be removed" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
							}
							else
							{
								if (curveProfiles.ContainsKey(sizeCurveTran.SizeCurveName))
								{
									tmpSizeCurveProfile = (SizeCurveProfile)curveProfiles[sizeCurveTran.SizeCurveName];
									curveProfiles.Remove(sizeCurveTran.SizeCurveName);

									if (curvesCreated.ContainsKey(sizeCurveTran.SizeCurveName))
									{
										curvesCreated.Remove(sizeCurveTran.SizeCurveName);
									}
									else
									{
										curvesRemoved.Add(sizeCurveTran.SizeCurveName, tmpSizeCurveProfile);
									}
								}
								else
								{
									curveError = true;

                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_DefaultSizeCurveNOTRemoved);//TT780
                                    msgText += System.Environment.NewLine;//TT780
                                    //msgText = tranImg + "Message: Size Curve Name is NOT defined in Size Curve Group" + System.Environment.NewLine;//TT780
									em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}
							}

							if (curveError)
							{
								editError = true;
							}
						}
					}
				}

				if (!editError)
				{
					//Begin TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
					if (defSizeCurveProfile != null)
					{
					//End TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
						// ====================================================
						// Verify that the default size curve contains an entry
						// for all size codes and add each entry when required
						// ====================================================
						foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
						{
							if (!defSizeCurveProfile.SizeCodeList.Contains(allSizeProfile.Key))
							{
								sizeCodeProfileClone = (SizeCodeProfile)allSizeProfile.Clone();
								sizeCodeProfileClone.SizeCodePercent = (float)sizeValueZero;
								defSizeCurveProfile.SizeCodeList.Add(sizeCodeProfileClone);
							}
						}
					//Begin TT#1366 - JScott - Object reference error after removing all size curves with stored procedure
					}
					else
					{
						// =======================================================================
						// Create a default size curve, set all values to 1.0 and equalize to 100%
						// =======================================================================
						foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
						{
							allSizeProfile.SizeCodePercent = (float)sizeValueOne;
						}

						spreadOkay = EqualizeSizeValues(ref allSizeProfiles);

						if (spreadOkay)
						{
							defSizeCurveProfile = new SizeCurveProfile(Include.NoRID);
							foreach (SizeCodeProfile allSizeProfile in allSizeProfiles.Values)
							{
								defSizeCurveProfile.SizeCodeList.Add(allSizeProfile);
							}
						}
						else
						{
							editError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            tranImg = MIDText.GetText(eMIDTextCode.msg_scglp_SizeCurveGroupAction);
                            // Begin TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load
                            //tranImg = msgText.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                            //tranImg = msgText.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                            tranImg = tranImg.Replace("{0}", sizeCurveGroupTran.SizeCurveGroupName);
                            tranImg = tranImg.Replace("{1}", sizeCurveGroupTran.SizeCurveGroupAction.ToString());
                            // End TT#336 - JSmith - ANF Batch Error on Header and Size Curve Load

							//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveGroupTran.SizeCurveGroupName + "], ";
							//tranImg += "Size Curve Group Action: [" + sizeCurveGroupTran.SizeCurveGroupAction.ToString() + "]";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
							tranImg += System.Environment.NewLine + System.Environment.NewLine;

                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_scglp_ErrorEqualizingDefault);//TT780
                            msgText += System.Environment.NewLine;//TT780
                            //msgText = tranImg + "Message: Error equalizing default curve size values to 100%" + System.Environment.NewLine;//TT780
							em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
						}
					}
					//End TT#1366 - JScott - Object reference error after removing all size curves with stored procedure

					// ==========================================
					// Build a size curve group profile and write
					// ==========================================
					sizeCurveGroupProfile.DefaultSizeCurve = defSizeCurveProfile;

					if (curveProfiles.Count == 0)
					{
						sizeCurveGroupProfile.SizeCurveList.Clear();
					}
					else
					{
						// ====================================================
						// Newly created size curves must be written here since
						// the RID is required when adding to the group list
						// ====================================================
						try
						{
							foreach (SizeCurveProfile curveProfile in curveProfiles.Values)
							{
								if (curveProfile.Key == Include.NoRID)
								{
									curveProfile.WriteSizeCurve(false, _SAB);
								}
							}
						}

						catch (Exception Ex)
						{
							modifyOkay = false;

							exceptMsg = Ex.ToString();
						}

						if (modifyOkay)
						{
							foreach (SizeCurveProfile curveProfile in curveProfiles.Values)
							{
								if (!sizeCurveGroupProfile.SizeCurveList.Contains(curveProfile.Key))
								{
									sizeCurveGroupProfile.SizeCurveList.Add(curveProfile);
								}
								else	// BEGIN MID Track #5278  - RonM - ANF Defect 1336 Size Curve Total < 100%
								{
									sizeCurveGroupProfile.SizeCurveList.Remove(curveProfile);
									sizeCurveGroupProfile.SizeCurveList.Add(curveProfile);
								}		// END MID Track #5278   
							}
						}
					}

					if (modifyOkay)
					{
						try
						{
// (CSMITH) - BEG MID Track #3173: Remove Curve from Group causes FK violation
							if (sizeCurveGroupProfile.StoreSizeCurveHash.Count > 0)
							{
								if (curvesRemoved.Count > 0)
								{
									// ===============================
									// Remove stores that have been
									// assigned to removed Size Curves
									// ===============================
									strSizeCurveHash = new Hashtable();
									strSizeCurveHash.Clear();

									foreach (int storeRID in sizeCurveGroupProfile.StoreSizeCurveHash.Keys)
									{
										curveFound = false;
										strSizeCurveProfile = (SizeCurveProfile)sizeCurveGroupProfile.StoreSizeCurveHash[storeRID];

										foreach (SizeCurveProfile curveProfile in curvesRemoved.Values)
										{
											if (strSizeCurveProfile.Key == curveProfile.Key)
											{
												curveFound = true;
												break;
											}
										}

										if (!curveFound)
										{
											strSizeCurveHash.Add(storeRID, strSizeCurveProfile);
										}
									}

									sizeCurveGroupProfile.StoreSizeCurveHash.Clear();

									if (strSizeCurveHash.Count > 0)
									{
										foreach (int storeRID in strSizeCurveHash.Keys)
										{
											strSizeCurveProfile = (SizeCurveProfile)strSizeCurveHash[storeRID];
											sizeCurveGroupProfile.StoreSizeCurveHash.Add(storeRID, strSizeCurveProfile);
										}
									}
								}
							}

							// ==================================
							// Update Size Curve Group list with
							// Size Curves that have been removed
							// ==================================
							foreach (SizeCurveProfile curveProfile in curvesRemoved.Values)
							{
								if (sizeCurveGroupProfile.SizeCurveList.Contains(curveProfile.Key))
								{
									sizeCurveGroupProfile.SizeCurveList.Remove(curveProfile);
								}
							}

// (CSMITH) - END MID Track #3173:
							// BEGIN MID Track #5268 - Size Curve Add/Update slow 
							//sizeCurveGroupProfile.WriteSizeCurveGroup(_SAB);
							sizeCurveGroupProfile.WriteSizeCurveGroup(_SAB, _curvesInGroupUpdated);
						 	// END MID Track #5268
						}

						catch (Exception Ex)
						{
							modifyOkay = false;

							exceptMsg = Ex.ToString();
						}
					}

					if (modifyOkay)
					{
						if (curvesRemoved.Count > 0)
						{
							// ==========================
							// Delete removed size curves
							// ==========================
							try
							{
								foreach (SizeCurveProfile curveProfile in curvesRemoved.Values)
								{
									curveProfile.DeleteSizeCurve();
								}
							}

							catch (Exception Ex)
							{
								modifyOkay = false;

								exceptMsg = Ex.ToString();
							}
						}
					}
				}
			}

			catch (Exception Ex)
			{
				modifyOkay = false;

				_audit.Log_Exception(Ex, GetType().Name);
			}

			finally
			{
				if (modifyOkay)
				{
					if (!editError)
					{
                        
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText  = MIDText.GetText(eMIDTextCode.msg_scglp_SuccessfullyModified);
                        msgText  = msgText.Replace("{0}",sizeCurveGroupTran.SizeCurveGroupName);
                        msgText  = msgText.Replace("{1}", _groupsRead.ToString());
                        msgText += System.Environment.NewLine;

                        //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
						//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
						//msgText += " successfully modified" + System.Environment.NewLine;
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
						em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

						WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), true);

						rtnCode = eReturnCode.successful;
					}
					else
					{
						
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText  = MIDText.GetText(eMIDTextCode.msg_scglp_EncounteredEditErrors);
                        msgText  = msgText.Replace("{0}",sizeCurveGroupTran.SizeCurveGroupName);
                        msgText  = msgText.Replace("{1}", _groupsRead.ToString());
                        msgText += System.Environment.NewLine;

                        //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
						//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
						//msgText += " encountered edit errors" + System.Environment.NewLine; 
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

						em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);

						WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

						rtnCode = eReturnCode.editErrors;
					}
				}
				else
				{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText  = MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors);
                        msgText  = msgText.Replace("{0}",sizeCurveGroupTran.SizeCurveGroupName);
                        msgText  = msgText.Replace("{1}", _groupsRead.ToString());

                        //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
					    //msgText += " (Transaction #" + _groupsRead.ToString() + ")";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */

					if (exceptMsg == null)
					{
						msgText += MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors1); 
                        msgText +=  System.Environment.NewLine;
                        //msgText += " encountered an update error" + System.Environment.NewLine;
					}
					else
					{
						msgText += MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors2); 
                        msgText += exceptMsg + System.Environment.NewLine;						
                        //msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
					}
					em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);

					WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

					rtnCode = eReturnCode.severe;
				}

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// =========================
		// Remove a size curve group
		// =========================
		public eReturnCode RemoveGroup(SizeCurveGroupsSizeCurveGroup sizeCurveGroupTran, SizeCurveGroupProfile sizeCurveGroupProfile)
		{
			EditMsgs em = null;

			bool removeOkay = true;

			string msgText = null;
			string exceptMsg = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{
				try
				{	// BEGIN MID Track #5240 - size curve groupo delete too slow
					//sizeCurveGroupProfile.DeleteSizeCurveGroup();
					if (sizeCurveGroupProfile.SizeCurveGroupIsInUse())
					{
						removeOkay = false;
						exceptMsg = MIDText.GetText((int)eMIDTextCode.msg_DeleteFailedDataInUse);
					}
					else
					{
						sizeCurveGroupProfile.DeleteSizeCurveGroup();
					}
				}	// END MID Track #5240

				catch (Exception Ex)
				{
					removeOkay = false;

					exceptMsg = Ex.ToString();
				}
			}

			catch (Exception Ex)
			{
				removeOkay = false;

				_audit.Log_Exception(Ex, GetType().Name);
			}

			finally
			{
				if (removeOkay)
				{
					msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
					msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					msgText += " successfully removed" + System.Environment.NewLine;
					em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

					WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), true);

					rtnCode = eReturnCode.successful;
				}
				else
				{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText  = MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors);
                        msgText  = msgText.Replace("{0}",sizeCurveGroupTran.SizeCurveGroupName);
                        msgText  = msgText.Replace("{1}", _groupsRead.ToString());

                    //msgText = "Size Curve Group Name " + sizeCurveGroupTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
/*End TT780 - FIX - RBeck - Change hard coded messages to soft text */
  
					if (exceptMsg == null)
					{
						msgText += MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors1); 
                        msgText +=  System.Environment.NewLine;
                        //msgText += " encountered an update error" + System.Environment.NewLine;
					}
					else
					{
                        msgText += MIDText.GetText(eMIDTextCode.msg_scglp_UpdateErrors2); 
                        msgText += exceptMsg + System.Environment.NewLine;						
                        //msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
					}
					em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);

					WriteGroupLoadStatus(sizeCurveGroupTran.SizeCurveGroupName, sizeCurveGroupTran.SizeCurveGroupAction.ToString(), false);

					rtnCode = eReturnCode.severe;
				}

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
				}
			}

			return rtnCode;
		}

		// ===============
		// Validate a size
		// ===============
		private SizeCodeProfile ValidateSize(string aTranImg,
											 string aCodeID,
											 string aCategory,
											 string aPrimary,
											 string aSecondary,
											 ref EditMsgs aEm)
		{
			bool sizeOkay = true;

			string msgText = null;

			int tmpRID = Include.NoRID;

			SizeCodeList sizeCodeList = null;

			SizeCodeProfile sizeCodeProfile = null;
			SizeCodeProfile undefSizeCodeProfile = null;

			undefSizeCodeProfile = new SizeCodeProfile(Include.NoRID);

			if (_codesPresent)
			{
				// ==================
				// Edit the size code
				// ==================
				if (aCodeID == "" || aCodeID == null)
				{
					sizeOkay = false;

                    msgText = MIDText.GetText(eMIDTextCode.msg_scglp_CodeIDRequired);
                    msgText +=  System.Environment.NewLine;						
                    //msgText = aTranImg + "Message: Code ID is required" + System.Environment.NewLine;
					aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
				}
				else
				{
					try
					{
						sizeCodeProfile = _SAB.HierarchyServerSession.GetSizeCodeProfile(aCodeID);

						if (sizeCodeProfile.Key == Include.NoRID)
						{
							sizeOkay = false;

                            msgText = MIDText.GetText(eMIDTextCode.msg_scglp_CodeIDNOTDefined);
                            msgText += System.Environment.NewLine;						
							//msgText = aTranImg + "Message: Code ID is NOT defined" + System.Environment.NewLine;
							aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
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
				}
			}
			else
			{
				// =========================
				// Edit the product category
				// =========================
				if (aCategory == "" || aCategory == null)
				{
					sizeOkay = false;

                    msgText = MIDText.GetText(eMIDTextCode.msg_scglp_CategoryRequired);
                    msgText += System.Environment.NewLine;						
					//msgText = aTranImg + "Message: Product Category is required" + System.Environment.NewLine;
					aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
				}
				else
				{
					if (!_prodCategoryList.Contains(aCategory))
					{
						sizeOkay = false;

                        msgText = MIDText.GetText(eMIDTextCode.msg_scglp_CategoryNOTDefined);
                        msgText += System.Environment.NewLine;						
						//msgText = aTranImg + "Message: Product Category is NOT defined" + System.Environment.NewLine;
						aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					}
				}

				if (sizeOkay)
				{
					// ======================================
					// Edit the primary / secondary and value
					// ======================================
					if (aPrimary == "" || aPrimary == null)
					{
						sizeOkay = false;

                        msgText = MIDText.GetText(eMIDTextCode.msg_scglp_PrimarySecondaryRequired);
                        msgText += System.Environment.NewLine;						
						//msgText = aTranImg + "Message: Primary (and Secondary) is required" + System.Environment.NewLine;
						aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
					}
					else
					{
						try
						{
							sizeCodeList = _SAB.HierarchyServerSession.GetExactSizeCodeList(aCategory, aPrimary, aSecondary);

							if (sizeCodeList.Count > 0)
							{
								tmpRID = Include.NoRID;

								foreach (SizeCodeProfile scp in sizeCodeList)
								{
									if (scp.SizeCodePrimary == aPrimary && scp.SizeCodeSecondary == aSecondary)
									{
										tmpRID = scp.Key;
										break;
									}
								}

								if (tmpRID != Include.NoRID)
								{
									try
									{
										sizeCodeProfile = _SAB.HierarchyServerSession.GetSizeCodeProfile(tmpRID);

										if (sizeCodeProfile.Key == Include.NoRID)
										{
											sizeOkay = false;

                                            msgText = MIDText.GetText(eMIDTextCode.msg_scglp_PrimarySecondaryNOTDefined);
                                            msgText += System.Environment.NewLine;						
											//msgText = aTranImg + "Message: Primary / Secondary is NOT defined" + System.Environment.NewLine;
											aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
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
								}
								else
								{
									sizeOkay = false;

                                    msgText = MIDText.GetText(eMIDTextCode.msg_scglp_PrimarySecondaryNOTDefinedInCat);
                                    msgText += System.Environment.NewLine;						
									//msgText = aTranImg + "Message: Primary / Secondary not defined in Product Category" + System.Environment.NewLine;
									aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
								}
							}
							else
							{
								sizeOkay = false;

                                msgText = MIDText.GetText(eMIDTextCode.msg_scglp_SizesNOTDefinedInCat);
                                msgText += System.Environment.NewLine;						
								//msgText = aTranImg + "Message: No sizes defined in Product Category" + System.Environment.NewLine;
								aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
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
					}
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

		// =================
		// Edit a size value
		// =================
		private double EditSizeValue(string aTranImg, double aSizeValue, ref EditMsgs aEm)
		{
			string msgText = null;

			double sizeValue = 0.0;

			if (aSizeValue < 0.0)
			{
				sizeValue = 0.0;

                msgText = MIDText.GetText(eMIDTextCode.msg_scglp_ValueNotNeg);
                msgText += System.Environment.NewLine;
                //msgText = aTranImg + "Message: Value cannot be negative - setting to 0.00" + System.Environment.NewLine;
				aEm.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
			}
			else
			{
				sizeValue = Math.Round(aSizeValue, 3);
			}

			return sizeValue;
		}

		// ================================
		// Equalize the size values to 100%
		// ================================
		private bool EqualizeSizeValues(ref Hashtable aSizeProfiles)
		{
			int newValIdx = 0;

			bool spreadOkay = true;

			ArrayList newValues = null;
			ArrayList oldValues = null;

			BasicSpread pctSpread = null;

			try
			{
				newValIdx = 0;

				oldValues = new ArrayList();

				foreach (SizeCodeProfile sizeProfile in aSizeProfiles.Values)
				{
					oldValues.Add(Convert.ToDouble(sizeProfile.SizeCodePercent, CultureInfo.CurrentUICulture));
				}

				pctSpread = new BasicSpread();

				pctSpread.ExecuteSimpleSpread(100.0, oldValues, 3, out newValues);

				foreach (SizeCodeProfile sizeProfile in aSizeProfiles.Values)
				{
					sizeProfile.SizeCodePercent = (float)Convert.ToDouble(newValues[newValIdx++], CultureInfo.CurrentUICulture);
				}
			}

			catch (Exception Ex)
			{
				spreadOkay = false;

				_audit.Log_Exception(Ex, GetType().Name);
			}

			return spreadOkay;
		}

		// ======================================
		// Write the size curve group load status
		// ======================================
		private void WriteGroupLoadStatus(string aGroupName, string aGroupAction, bool aLoadStatus)
		{
			// ======================================
			// Write the SizeCurveGroupStatus element
			// ======================================
			_xmlWriter.WriteStartElement("SizeCurveGroupStatus");

			// ===============================
			// Write the Size Curve Group Name
			// ===============================
			_xmlWriter.WriteStartAttribute(null, "SizeCurveGroupName", null);

			if (aGroupName != "" && aGroupName != null)
			{
				_xmlWriter.WriteString(aGroupName);
			}
			else
			{
				_xmlWriter.WriteString("[No Group Name]");
			}

			_xmlWriter.WriteEndAttribute();

			// =================================
			// Write the Size Curve Group Action
			// =================================
			_xmlWriter.WriteStartAttribute(null, "SizeCurveGroupAction", null);

			if (aGroupAction != "" && aGroupAction != null)
			{
				_xmlWriter.WriteString(aGroupAction);
			}
			else
			{
				_xmlWriter.WriteString("[No Group Action]");
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

			// ======================================
			// Close the SizeCurveGroupStatus element
			// ======================================
			_xmlWriter.WriteEndElement();
		}
	}
}
