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
	/// Summary description for SizeCurveLoadProcess.
	/// </summary>
	public class SizeCurveLoadProcess
	{
//		private string sourceModule = "SizeCurveLoadProcess.cs";

		SessionAddressBlock _SAB;

		private int _groupsRead = 0;
		private int _groupsFail = 0;
		private int _groupsLoad = 0;

		private Audit _audit = null;

		private ProfileList _spl = null;

		private string _comment = null;
		private string _statusFile = null;

		private Hashtable _storeRIDs = null;

		private XmlTextWriter _xmlWriter = null;

        //private StoreServerSession _strSrvrSess = null; //TT#1517-MD -jsobek -Store Service Optimization

		private int _storeRID = Include.UndefinedStoreRID;

		private ApplicationSessionTransaction _appSessTrans = null;

        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
        public int CurvesRead
        {
            get { return _groupsRead; }
        }
        public int CurvesError
        {
            get { return _groupsFail; }
        }
        public int CurvesCreate
        {
            get { return _groupsLoad; }
        }
        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

		public SizeCurveLoadProcess(SessionAddressBlock SAB, ref bool scErrorFound)
		{
			try
			{
				_SAB = SAB;

                //_strSrvrSess = _SAB.StoreServerSession; //TT#1517-MD -jsobek -Store Service Optimization

				_audit = _SAB.ClientServerSession.Audit;

				_appSessTrans = new ApplicationSessionTransaction(_SAB);

				_spl = _appSessTrans.GetMasterProfileList(eProfileType.Store);

				if (_spl.Count > 0)
				{
					_storeRIDs = new Hashtable();
					_storeRIDs.Clear();

					foreach (StoreProfile sp in _spl)
					{
						if (!_storeRIDs.ContainsKey(sp.StoreId))
						{
                            _storeRID = StoreMgmt.StoreProfile_GetStoreRidFromId(sp.StoreId); //_strSrvrSess.GetStoreRID(sp.StoreId);

							if (_storeRID != Include.UndefinedStoreRID)
							{
								_storeRIDs.Add(sp.StoreId, _storeRID);
							}
						}
					}
				}
				else
				{
					scErrorFound = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    string msgText = null;
                    msgText = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderNoStores);
                    msgText = msgText + System.Environment.NewLine;
                    _audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);

                    //string msgText= "No stores are currently defined";
					//_audit.Add_Msg(eMIDMessageLevel.Severe, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

				}
			}

			catch (Exception Ex)
			{
				scErrorFound = true;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
			}
		}

		// ====================================================
		// Serialize and process the xml input transaction file
		// ====================================================
		public eReturnCode ProcessCurveTrans(string sizeCurveTransFile, ref bool scErrorFound)
		{
			string msgText = null;

			TextReader xmlReader = null;

			XmlSerializer xmlSerial = null;

			SizeCurves sizeCurveTrans = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			try
			{
				try
				{
					xmlSerial = new XmlSerializer(typeof(SizeCurves));

					xmlReader = new StreamReader(sizeCurveTransFile);

					sizeCurveTrans = (SizeCurves)xmlSerial.Deserialize(xmlReader);

                    // Begin Track #4229 - JSmith - API locks .XML input file
                    //xmlReader.Close();
                    // End Track #4229
				}

				catch (Exception Ex)
				{
					scErrorFound = true;

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

				if (!scErrorFound)
				{
					if (sizeCurveTrans.SizeCurve != null)
					{
						// ===============================================
						// Create an xml writer and set formatting options
						// ===============================================
						_statusFile = Path.ChangeExtension(sizeCurveTransFile, ".out.xml");
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
						_comment = "Size Curve Load Status for: " + Path.GetFileName(sizeCurveTransFile) + " submitted on " + DateTime.Now.ToString("U");
						_xmlWriter.WriteComment(_comment);

						// ======================
						// Write the root element
						// ======================
						_xmlWriter.WriteStartElement("SizeCurveLoadStatus", "http://tempuri.org/SizeCurveLoadStatusSchema.xsd");

						foreach (SizeCurvesSizeCurve sizeCurveTran in sizeCurveTrans.SizeCurve)
						{
							++_groupsRead;

							rtnCode = ProcessOneGroup(sizeCurveTran);

							if (rtnCode != eReturnCode.successful)
							{
								++_groupsFail;
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

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderNoSizeCurve);
                        msgText = msgText + System.Environment.NewLine;
                        _audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);

                        //msgText = "No Size Curve Group transactions found in input file" + System.Environment.NewLine;
                        //_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
 
					}
				}
			}

			catch (Exception Ex)
			{
				scErrorFound = true;

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
                //_SAB.ClientServerSession.Audit.SizeCurveLoadAuditInfo_Add(_groupsRead, _groupsFail, _groupsLoad, 0);
                //msgText = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderGroupInfo);
                //msgText = msgText.Replace("{0}", _groupsRead.ToString() + System.Environment.NewLine);
                //msgText = msgText.Replace("{1}", _groupsFail.ToString() + System.Environment.NewLine);
                //msgText = msgText.Replace("{2}", _groupsLoad.ToString() + System.Environment.NewLine);

                ////msgText = "Groups Re:ad " + _groupsRead.ToString() + System.Environment.NewLine;
                ////msgText += "Groups In Error: " + _groupsFail.ToString() + System.Environment.NewLine;
                ////msgText += "Groups Loaded: " + _groupsLoad.ToString() + System.Environment.NewLine;
                //_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
                //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                /*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

				// ===================================================================================
				// This statement is currently inactive - requires extensive modifications to activate
				// ===================================================================================
				//_audit.CurveLoadAuditInfo_Add(_groupsRead, _groupsFail, _groupsLoad);
			}

			return rtnCode;
		}

		// ========================================
		// Process one size curve group transaction
		// ========================================
		public eReturnCode ProcessOneGroup(SizeCurvesSizeCurve sizeCurveTran)
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
//				Thread.Sleep(5);
				// BEGIN MID Track 4279 - trim spaces off string fields
				Include.TrimStringsInObject(sizeCurveTran);
				// END MID Track 4279

				if (sizeCurveTran.SizeCurveGroupName == "" || sizeCurveTran.SizeCurveGroupName == null)
				{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    msgText = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderNoSizeCurveGrpName);
                    msgText = msgText.Replace("{0}", _groupsRead.ToString());
                    msgText += System.Environment.NewLine;
                    //msgText = "Size Curve Transaction #" + _groupsRead.ToString();
					//msgText += " has NO Size Curve Group Name" + System.Environment.NewLine;
					em.AddMsg(eMIDMessageLevel.Severe, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

					WriteCurveLoadStatus(sizeCurveTran.SizeCurveGroupName, false);

					rtnCode = eReturnCode.severe;
				}
				else
				{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    msgText = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderBeginProcessing);
                    msgText = msgText.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    msgText = msgText.Replace("{1}", _groupsRead.ToString());
                    msgText += System.Environment.NewLine;

                    //msgText = "Size Curve Group " + sizeCurveTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					//msgText += " - Begin Processing" + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    //tranImg = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessageSCNotDefined);
                    //tranImg = msgText.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    //tranImg = msgText + System.Environment.NewLine + System.Environment.NewLine;
                    tranImg = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessageSCNotDefined);
                    tranImg = tranImg.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    tranImg = tranImg + System.Environment.NewLine;
                    _audit.Add_Msg(eMIDMessageLevel.Information, tranImg, GetType().Name);
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

                    //tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveTran.SizeCurveGroupName + "]"; //TT780
                    //tranImg += System.Environment.NewLine + System.Environment.NewLine;   //TT780
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

					if (sizeCurveTran.StoreSizeCurve != null)
					{
						sizeCurve = new SizeCurve();

						groupRID = sizeCurve.GetSizeCurveGroupKey(sizeCurveTran.SizeCurveGroupName);

						if (groupRID != Include.NoRID)
						{
							sizeCurveGroupProfile = new SizeCurveGroupProfile(groupRID);

							rtnCode = ProcessGroupStores(sizeCurveTran, sizeCurveGroupProfile);

							if (rtnCode == eReturnCode.successful)
							{
								++_groupsLoad;
							}
						}
						else
						{

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessageSCNotDefinedMessage1);
                            msgText = msgText + System.Environment.NewLine;

                            //msgText = tranImg + "Message: Size Curve Group is NOT defined" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Warning, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

							WriteCurveLoadStatus(sizeCurveTran.SizeCurveGroupName, false);

							rtnCode = eReturnCode.editErrors;
						}
					}
					else
					{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessageSCNotDefinedMessage2);
                        msgText = msgText + System.Environment.NewLine;

                        //msgText = tranImg + "Message: No store transactions found to process" + System.Environment.NewLine;
						em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

						WriteCurveLoadStatus(sizeCurveTran.SizeCurveGroupName, false);

						rtnCode = eReturnCode.editErrors;
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
                    msgText = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderEndProcessing);
                    msgText = msgText.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    msgText = msgText.Replace("{1}", _groupsRead.ToString());
                    msgText = msgText + System.Environment.NewLine;

                    //msgText = "Size Curve Group " + sizeCurveTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					//msgText += " - End Processing" + System.Environment.NewLine;
					_audit.Add_Msg(eMIDMessageLevel.Information, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
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

                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    //_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
                    _audit.Add_Msg(emm.messageLevel, emm.msg, emm.module);
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
				}
			}

			return rtnCode;
		}

		// ===============================================
		// Process one size curve group store transactions
		// ===============================================
		public eReturnCode ProcessGroupStores(SizeCurvesSizeCurve sizeCurveTran, SizeCurveGroupProfile sizeCurveGroupProfile)
		{
			EditMsgs em = null;

			string msgText = null;
			string tranImg = null;
			string exceptMsg = null;

			bool editError = false;
			bool processOkay = true;
			bool storeError = false;
			bool wasAssigned = false;

			int newCurveRID = Include.NoRID;
			int oldCurveRID = Include.NoRID;
			int storeRID = Include.UndefinedStoreRID;

			SizeCurveProfile newSizeCurveProfile = null;
			SizeCurveProfile oldSizeCurveProfile = null;

			eReturnCode rtnCode = eReturnCode.successful;

			// ================
			// Begin processing
			// ================
			em = new EditMsgs();

			try
			{
				// =====================================
				// Edit the store id and size curve name
				// =====================================
				foreach (SizeCurvesSizeCurveStoreSizeCurve storeTran in sizeCurveTran.StoreSizeCurve)
				{
					storeError = false;

					wasAssigned = false;

					storeRID = Include.UndefinedStoreRID;
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    //tranImg = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessageH);
                    //tranImg += tranImg.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    //tranImg += tranImg.Replace("{1}", storeTran.StoreID);
                    //tranImg += tranImg.Replace("{2}", storeTran.SizeCurveName);
                    //tranImg += System.Environment.NewLine + System.Environment.NewLine;
                    tranImg = MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessageH);
                    tranImg = tranImg.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    tranImg = tranImg.Replace("{1}", storeTran.StoreID);
                    tranImg = tranImg.Replace("{2}", storeTran.SizeCurveName);
                    tranImg += System.Environment.NewLine;
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

					//tranImg = "Transaction: Size Curve Group Name: [" + sizeCurveTran.SizeCurveGroupName + "], ";
					//tranImg += "Store ID: [" + storeTran.StoreID + "], ";
					//tranImg += "Size Curve Name: [" + storeTran.SizeCurveName + "]";
					//tranImg += System.Environment.NewLine + System.Environment.NewLine;
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */
					if ((storeTran.StoreID == "" || storeTran.StoreID == null) &&
						(storeTran.SizeCurveName == "" || storeTran.SizeCurveName == null))
					{
						storeError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessage1);
                        msgText = msgText + System.Environment.NewLine;

						//msgText = tranImg + "Message: Store transaction is empty" + System.Environment.NewLine;
						em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

					}
					else
					{
						// =================
						// Edit the store id
						// =================
						if (storeTran.StoreID == "" || storeTran.StoreID == null)
						{
							storeError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                            msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessage2);
                            msgText = msgText + System.Environment.NewLine;
 
                            //msgText = tranImg + "Message: Store ID is required" + System.Environment.NewLine;
							em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

						}
						else
						{
							if (!_storeRIDs.ContainsKey(storeTran.StoreID))
							{
								storeError = true;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessage3);
                                msgText = msgText + System.Environment.NewLine;

								//msgText = tranImg + "Message: Store ID is NOT defined" + System.Environment.NewLine;
								em.AddMsg(eMIDMessageLevel.Edit, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

							}
							else
							{
								storeRID = (int)_storeRIDs[storeTran.StoreID];

								if (!sizeCurveGroupProfile.StoreSizeCurveHash.ContainsKey(storeRID))
								{
									wasAssigned = false;

									oldCurveRID = sizeCurveGroupProfile.DefaultSizeCurveRid;

									oldSizeCurveProfile = (SizeCurveProfile)sizeCurveGroupProfile.DefaultSizeCurve;
								}
								else
								{
									wasAssigned = true;

									oldSizeCurveProfile = (SizeCurveProfile)sizeCurveGroupProfile.StoreSizeCurveHash[storeRID];

									oldCurveRID = oldSizeCurveProfile.Key;
								}

								// ========================
								// Edit the size curve name
								// ========================
								if (storeTran.SizeCurveName == "" || storeTran.SizeCurveName == null)
								{
									newCurveRID = sizeCurveGroupProfile.DefaultSizeCurveRid;

									newSizeCurveProfile = (SizeCurveProfile)sizeCurveGroupProfile.DefaultSizeCurve;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                    msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessage4);
                                    msgText = msgText + System.Environment.NewLine;

									//msgText = tranImg + "Message: Size Curve Name NOT specified - Store assigned to default Size Curve Name" + System.Environment.NewLine;
									em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

								}
								else
								{
									newSizeCurveProfile = null;

									newCurveRID = Include.NoRID;

									foreach (SizeCurveProfile scp in sizeCurveGroupProfile.SizeCurveList)
									{
										if (storeTran.SizeCurveName == scp.SizeCurveName)
										{
											newCurveRID = scp.Key;

											newSizeCurveProfile = scp;
											break;
										}
									}

									if (newCurveRID == Include.NoRID)
									{
										newCurveRID = oldCurveRID;

										newSizeCurveProfile = oldSizeCurveProfile;

/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                                        msgText = tranImg + MIDText.GetText(eMIDTextCode.msg_sclp_HeaderTransactionMessage5);
                                        msgText = msgText + System.Environment.NewLine;
                                        //msgText = tranImg + "Message: Size Curve Name does NOT exist in Size Curve Group - Store size curve assignment NOT changed" + System.Environment.NewLine;
										em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

									}
								}
							}
						}
					}

					if (storeError)
					{
						editError = true;
					}
					else
					{
						// ===========================================
						// Update the store / size curve profile table
						// ===========================================
						if (wasAssigned)
						{
							if (newCurveRID != oldCurveRID)
							{
								if (newCurveRID == sizeCurveGroupProfile.DefaultSizeCurveRid)
								{
									sizeCurveGroupProfile.StoreSizeCurveHash.Remove(storeRID);
								}
								else
								{
									sizeCurveGroupProfile.StoreSizeCurveHash[storeRID] = newSizeCurveProfile;
								}
							}
						}
						else
						{
							if (newCurveRID != sizeCurveGroupProfile.DefaultSizeCurveRid)
							{
								sizeCurveGroupProfile.StoreSizeCurveHash.Add(storeRID, newSizeCurveProfile);
							}
						}
					}
				}

				// ==========================================
				// Write the updated size curve group profile
				// ==========================================
				try
				{
					// BEGIN MID Track #5268 - Size Curve Add/Update slow
					//sizeCurveGroupProfile.WriteSizeCurveGroup(_SAB);
					sizeCurveGroupProfile.UpdateStoreSizeCurveByGroup();
					// END MID Track #5268 
				}

				catch (Exception Ex)
				{
					processOkay = false;

					exceptMsg = Ex.ToString();
				}
			}

			catch (Exception Ex)
			{
				processOkay = false;

				_audit.Add_Msg(eMIDMessageLevel.Severe, Ex.ToString(), GetType().Name);
			}

			finally
			{
				if (processOkay)
				{
/*Begin TT780 - FIX - RBeck - Change hard coded messages to soft text */
                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    //msgText = MIDText.GetText(eMIDTextCode.msg_sclp_ProcessGroupStoresFinal);
                    //msgText += msgText.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    //msgText += msgText.Replace("{1}", _groupsRead.ToString());
                    msgText = MIDText.GetText(eMIDTextCode.msg_sclp_ProcessGroupStoresFinal);
                    msgText = msgText.Replace("{0}", sizeCurveTran.SizeCurveGroupName);
                    msgText = msgText.Replace("{1}", _groupsRead.ToString());
                    msgText = msgText + System.Environment.NewLine; 
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

                    //msgText = "Size Curve Group Name " + sizeCurveTran.SizeCurveGroupName;
					//msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					//msgText += " successfully processed";
					if (editError)
					{
                        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                        //msgText += System.Environment.NewLine;
                        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                        msgText += MIDText.GetText(eMIDTextCode.msg_sclp_ProcessGroupStoresFinalError1);
                        //msgText += System.Environment.NewLine + "     One or more store transactions encountered edit errors";
                        //msgText = msgText + System.Environment.NewLine;
					}
                    msgText += System.Environment.NewLine;
                    em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);
/*End   TT780 - FIX - RBeck - Change hard coded messages to soft text */

					WriteCurveLoadStatus(sizeCurveTran.SizeCurveGroupName, true);

					rtnCode = eReturnCode.successful;
				}
				else
				{
					msgText = "Size Curve Group Name " + sizeCurveTran.SizeCurveGroupName;
					msgText += " (Transaction #" + _groupsRead.ToString() + ")";
					if (exceptMsg == null)
					{
                        msgText += MIDText.GetText(eMIDTextCode.msg_sclp_ProcessGroupStoresFinalError2);
                        msgText += System.Environment.NewLine;
                        //msgText += " encountered an update error" + System.Environment.NewLine;                        
					}
					else
					{
                        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                        //msgText += MIDText.GetText(eMIDTextCode.msg_sclp_ProcessGroupStoresFinalError3);
                        msgText = MIDText.GetText(eMIDTextCode.msg_sclp_ProcessGroupStoresFinalError3);
                        msgText = msgText.Replace("{0}", exceptMsg);
                        msgText = msgText + System.Environment.NewLine;
                        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                        //msgText += " encountered an update error: " + exceptMsg + System.Environment.NewLine;
					}
					em.AddMsg(eMIDMessageLevel.Information, msgText, GetType().Name);

					WriteCurveLoadStatus(sizeCurveTran.SizeCurveGroupName, false);

					rtnCode = eReturnCode.severe;
				}

				for (int e = 0; e < em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];

                    //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
                    //_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.module);
                    _audit.Add_Msg(emm.messageLevel, emm.msg, emm.module);
                    //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
				}
			}

			return rtnCode;
		}

		// ================================
		// Write the size curve load status
		// ================================
		private void WriteCurveLoadStatus(string aGroupName, bool aLoadStatus)
		{
			// =================================
			// Write the SizeCurveStatus element
			// =================================
			_xmlWriter.WriteStartElement("SizeCurveStatus");

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

			// =================================
			// Close the SizeCurveStatus element
			// =================================
			_xmlWriter.WriteEndElement();
		}
	}
}
