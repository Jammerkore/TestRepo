using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml;
using System.Xml.Serialization;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.SizeCodesLoad
{
	/// <summary>
	/// Entry point for size codes load.
	/// </summary>
	class SizeCodesLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			SizeLoadWorker sw = new SizeLoadWorker();
			return sw.LoadSizes(args);
		}
	}

	public class SizeLoadWorker
	{
		private Sizes _sizes = null;
		string sourceModule = "SizeCodesLoad.cs";
		string eventLogID = "MIDSizeCodesLoad";
		SessionAddressBlock _SAB;
		SessionSponsor _sponsor;
		IMessageCallback _messageCallback;
        Dictionary<string, int> _sizeIDHash;
        Dictionary<string, int> _priSecHash;
		string _noSecondarySizeStr = null;
		int _recordsRead = 0;
		int _addRecords = 0;
		int _updateRecords = 0;
		int _recordsWithErrors = 0;
		eMIDMessageLevel highestMessage;

		string fileLocation = null;
		char[] delimiter = {'~'};
		string message = null;
		bool errorFound = false;
        //BinaryServerFormatterSinkProvider provider;
        //Hashtable port;
		System.Runtime.Remoting.Channels.IChannel channel;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

		public int LoadSizes(string[] args)
		{

			try
			{
				_messageCallback = new BatchMessageCallback();
				_sponsor = new SessionSponsor();
				_SAB = new SessionAddressBlock(_messageCallback, _sponsor);

				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}

				// Register callback channel

				try
				{
					channel = _SAB.OpenCallbackChannel();
				}
				catch (Exception ex)
				{
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + ex.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				// Create Sessions

				try
				{
					_SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Hierarchy);
				}
				catch (Exception ex)
				{
					errorFound = true;
					Exception innerE = ex;
					while (innerE.InnerException != null)
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
					MIDConfigurationManager.AppSettings["Password"], eProcesses.sizeCodeLoad);

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                //BEGIN TT#1644-VSuart-Process Control-MID
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1644-VSuart-Process Control-MID
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				_SAB.ClientServerSession.Initialize();
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				if (args.Length > 0)
				{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					if (args[0] == Include.SchedulerID)
					{
						fileLocation = args[1];
						_processId = Convert.ToInt32(args[2]);
                        // Begin TT#1054 - JSmith - Relieve Intransit not working.
                        //delimiter = ConfigurationSettings.AppSettings["Delimiter"].ToCharArray();
                        delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                        // End TT#1054
					}
					else
					{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						fileLocation = args[0];
						if (args.Length > 1)
						{
							delimiter = args[1].ToCharArray();
						}
						else
						{
							delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
						}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
				else
				{
					fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
					string strDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
					if (strDelimiter != null)
					{
						delimiter = strDelimiter.ToCharArray();
					}
				}

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
				if (_processId != Include.NoRID)
				{
					_SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					_SAB.ClientServerSession.Initialize();
				}

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

        //Begin TT#425 - MD - Remove Exception File logic RBeck            
                //string exceptionFile = MIDConfigurationManager.AppSettings["ExceptionFile"];
                //if (exceptionFile == null)
                //{
                //    exceptionFile = @".\exceptionFile.txt";
                //}
                string exceptionFile = null;
        //End   TT#425 - MD - Remove Exception File logic RBeck


				message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

				if (fileLocation == "" || fileLocation == null)
				{
					message = message.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
					message += "Size Codes Load Process NOT run";

					// Begin Track #5035 - JSmith - file not found message level inconsistent
//					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
					// End Track #5035

					_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", _SAB.GetHighestAuditMessageLevel());

					return Convert.ToInt32(_SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
				}
				else
				{
					if (!File.Exists(fileLocation))
					{
						message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
						message += "Size Codes Load Process NOT run";

						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);

						_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", _SAB.GetHighestAuditMessageLevel());

						return Convert.ToInt32(_SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
					}
					else
					{
						FileInfo txnFileInfo = new FileInfo(fileLocation);

						if (txnFileInfo.Length == 0)
						{
							message = message.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
							message += "Size Codes Load Process NOT run";

							// Begin Track #5035 - JSmith - file not found message level inconsistent
//							_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule, true);
							_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);
							// End Track #5035

							_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_InputFileNotFound, "", _SAB.GetHighestAuditMessageLevel());

							return Convert.ToInt32(_SAB.ClientServerSession.Audit.HighestMessageLevel, CultureInfo.CurrentUICulture);
						}
						else
						{
							_SAB.HierarchyServerSession.Initialize();

							message = message.Replace("{0}", "[" + fileLocation + "]");

							_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);

							if (!errorFound)
							{
								_sizeIDHash = _SAB.HierarchyServerSession.GetSizeCodeListByID();

								_priSecHash = _SAB.HierarchyServerSession.GetSizeCodeListByPriSec();

								_noSecondarySizeStr = MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize);

								if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
								{
									errorFound = LoadXMLTransFile(fileLocation);
								}
								else
								{
									errorFound = LoadDelimitedTransFile(fileLocation, delimiter, ref errorFound, exceptionFile);
								}
							}
						}
					}
				}
			}

			catch ( Exception ex )
			{
				errorFound = true;
				message = "";
				while(ex != null)
				{
					message += " -- " + ex.Message;
					ex = ex.InnerException;
				}
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
			}
			finally
			{
				if (!errorFound)
				{
					if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
					{
						_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
						_SAB.ClientServerSession.Audit.SizeCodeLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords);
					}
				}
				else
				{
					if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
					{
                        //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        //_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                        _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                        //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
						_SAB.ClientServerSession.Audit.SizeCodeLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords);
					}
				}

				highestMessage = _SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}

		public bool LoadDelimitedTransFile(string fileLocation, char[] delimiter, ref bool errorFound,
			string exceptionFileName)
		{
			StreamReader reader = null;
			StreamWriter exceptionFile = null;
			string line = null;
			string message = null;
			eMIDMessageLevel returnCode = eMIDMessageLevel.None;
			
			try
			{
                //exceptionFile = new StreamWriter(exceptionFileName); // TT#425 - MD - Remove Exception File logic RBeck

				reader = new StreamReader(fileLocation);  //opens the file

				while ((line = reader.ReadLine()) != null)
				{
					string[] fields = MIDstringTools.Split(line,delimiter[0],true);
					if (fields.Length == 1 && fields[0] == "")  // skip blank line
					{
						continue;
					}
					++_recordsRead;;
					// BEGIN Issue 4667 stodd 9.18.2007
					if (fields.Length < 3)
					{
						string msgDetails = "Delimiter defined as " + delimiter[0].ToString(CultureInfo.CurrentUICulture) + " in CONFIG file.";
						++_recordsWithErrors;
						returnCode = eMIDMessageLevel.Error;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, msgDetails, sourceModule);
						continue;
					}
					// END Issue 4667
					returnCode = AddSizeRecord(fields);
				}
			}

			catch ( FileNotFoundException fileNotFound_error )
			{
				string exceptionMessage = fileNotFound_error.Message;
				errorFound = true;
				message = " : " + fileLocation;
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message);
				_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
			}
			catch ( Exception ex )
			{
				errorFound = true;
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
				_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());

				throw;
			}
			finally
			{

				if (reader != null)
				{
					reader.Close();
				}
				if (exceptionFile != null)
				{
					exceptionFile.Close();
				}
			}
			return false;
		}

		private eMIDMessageLevel AddSizeRecord(string[] fields)
		{
			eMIDMessageLevel returnCode = eMIDMessageLevel.None;
			string message;
			EditMsgs em = new EditMsgs();

			try
			{
				SizeCodeProfile scp = new SizeCodeProfile(-1);
				
				message =  "Code: " + ((fields[0] != null && fields[0].Length != 0) ? fields[0] : " ") 
					+ "; Primary: " + ((fields.Length > 1 && fields[1] != null && fields[1].Trim().Length > 0) ? fields[1] : " ") 
					+ "; Secondary: " + ((fields.Length > 2 && fields[2] != null && fields[2].Trim().Length > 0) ? fields[2] : " ") 
					+ "; Category: " + ((fields.Length > 3 && fields[3] != null && fields[3].Trim().Length > 0) ? fields[3] : " ");

				if (fields[0] == null || fields[0].Trim().Length == 0)
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CodeIsRequired, message, GetType().Name);
				}
				else
				{
					scp.SizeCodeID = fields[0].Trim();
				}

				if (fields.Length > 1 && fields[1] != null && fields[1].Trim().Length > 0)
				{
					scp.SizeCodePrimary = fields[1].Trim();
				}
				if (fields.Length > 2 && fields[2] != null && fields[2].Trim().Length > 0)
				{
					scp.SizeCodeSecondary = fields[2].Trim();
				}
				else
				{
					scp.SizeCodeSecondary = null;
				}
				if (fields.Length > 3 && fields[3] != null && fields[3].Trim().Length > 0)
				{
					scp.SizeCodeProductCategory = fields[3].Trim();
				}
				else
				{
					scp.SizeCodeProductCategory = MIDText.GetTextOnly(eMIDTextCode.lbl_Unassigned);
				}

				if (em.ErrorFound)  
				{
					++_recordsWithErrors;
					returnCode = eMIDMessageLevel.Error;
					for (int e=0; e<em.EditMessages.Count; e++)
					{
						EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
						_SAB.ClientServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
					}
				}
				else
				{
					returnCode = AddRecord(scp);
				}
			}
			catch ( Exception ex )
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
				returnCode = eMIDMessageLevel.Edit;
			}

			return returnCode;

		}


		public bool LoadXMLTransFile(string fileLocation)
		{
			string message = null;
			if(!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
			{
				throw new XMLSizeCodeLoadProcessException(String.Format("Can not find the file located at '{0}'", fileLocation));
			}
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
			try
			{
				XmlSerializer s = new XmlSerializer(typeof(Sizes));	// Create a Serializer
                // Begin Track #4229 - JSmith - API locks .XML input file
                //TextReader r = new StreamReader(fileLocation);			// Load the Xml File
                r = new StreamReader(fileLocation);			// Load the Xml File
                // End Track #4229
				_sizes = (Sizes)s.Deserialize(r);						// Deserialize the Xml File to a strongly typed object
                // Begin Track #4229 - JSmith - API locks .XML input file
                //r.Close();												// Close the input file.
                // End Track #4229
			}
			catch(Exception ex)
			{
				throw new XMLSizeCodeLoadProcessException(String.Format("Error encountered during deserialization of the file '{0}'", fileLocation), ex);
			}
            // Begin Track #4229 - JSmith - API locks .XML input file
            finally
            {
                if (r != null)
                    r.Close();
            }
            // End Track #4229

			try
			{
                //bool errorFound = false;
				SizeCodeProfile scp;
				foreach(SizesSize c in _sizes.Size)
				{
					EditMsgs em = new EditMsgs();
					message =  "Code: " + ((c.Code != null) ? c.Code : " ") + "; Primary: " + ((c.Primary != null) ? c.Primary : " ") + "; Secondary: " + ((c.Secondary != null) ? c.Secondary : " ") + "; Category: " + ((c.ProductCategory != null) ? c.ProductCategory : " ");
					errorFound = false;
					++_recordsRead;

					scp = new SizeCodeProfile(-1);	// Create a new color code profile

					scp.SizeCodeID = c.Code.Trim();
//					if (c.Secondary != null &&
//						c.Secondary.Trim().Length > 0)
//					{
//						scp.SizeCodeName = c.Primary.Trim() + " " + c.Secondary.Trim();
//						scp.SizeCodeName = scp.SizeCodeName.Trim();
//					}
//					else
//					{
//						scp.SizeCodeName = c.Primary.Trim();
//					}

					scp.SizeCodePrimary = c.Primary;
					scp.SizeCodeSecondary = c.Secondary;
					if (c.ProductCategory != null)
					{
						scp.SizeCodeProductCategory = c.ProductCategory;
					}
					else
					{
						scp.SizeCodeProductCategory = MIDText.GetTextOnly(eMIDTextCode.lbl_Unassigned);
					}

					scp.SizeCodeName = Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID);

					if (em.ErrorFound)  
					{
						++_recordsWithErrors;
						for (int e=0; e<em.EditMessages.Count; e++)
						{
							EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
							_SAB.ClientServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
						}
					}
					else
					{
						AddRecord(scp);
					}
				}
			}
			catch(Exception ex)
			{
				throw new XMLSizeCodeLoadProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
			}
			return false;
		}

		public eMIDMessageLevel AddRecord(SizeCodeProfile aSizeCodeProf)
		{
			eMIDMessageLevel returnCode = eMIDMessageLevel.None;
			int sizeCodeRID;
			string message;
			SizeCodeProfile prevSizeCodeProf = null;
			SizeCodeProfile newSizeCodeProf;
			string hashEntry;
			string prevHashEntry;

			try
			{
				hashEntry = Include.GetSizeKey(aSizeCodeProf.SizeCodeProductCategory, aSizeCodeProf.SizeCodePrimary, aSizeCodeProf.SizeCodeSecondary);
                //if (_sizeIDHash.Contains(aSizeCodeProf.SizeCodeID))
                if (_sizeIDHash.TryGetValue(aSizeCodeProf.SizeCodeID, out sizeCodeRID))
				{
                    //sizeCodeRID = (int)_sizeIDHash[aSizeCodeProf.SizeCodeID];
					prevSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeRID);
					prevHashEntry = Include.GetSizeKey(prevSizeCodeProf.SizeCodeProductCategory, prevSizeCodeProf.SizeCodePrimary, prevSizeCodeProf.SizeCodeSecondary);

					aSizeCodeProf.Key = sizeCodeRID;
					aSizeCodeProf.SizeCodeChangeType = eChangeType.update;
					newSizeCodeProf = _SAB.HierarchyServerSession.SizeCodeUpdate(aSizeCodeProf);
					_priSecHash.Remove(prevHashEntry);
					_priSecHash.Add(hashEntry, newSizeCodeProf.Key);
				}
				else
				{
					aSizeCodeProf.SizeCodeChangeType = eChangeType.add;
					newSizeCodeProf = _SAB.HierarchyServerSession.SizeCodeUpdate(aSizeCodeProf);
					_sizeIDHash.Add(aSizeCodeProf.SizeCodeID, newSizeCodeProf.Key);
					_priSecHash.Add(hashEntry, newSizeCodeProf.Key);
				}
			}
			catch (SizeCatgPriSecNotUniqueException exc)
			{
				message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeCatgPriSecNotUnique, false);
				message = message.Replace("{0}", aSizeCodeProf.SizeCodeProductCategory);
				message = message.Replace("{1}", aSizeCodeProf.SizeCodePrimary);
				if (aSizeCodeProf.SizeCodeSecondary != null &&
					aSizeCodeProf.SizeCodeSecondary.Trim().Length > 1)
				{
					message = message.Replace("{2}", aSizeCodeProf.SizeCodeSecondary);
				}
				else
				{
					message = message.Replace("{2}", _noSecondarySizeStr);
				}
				message = message.Replace("{3}", exc.Message);
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, GetType().Name);
				returnCode = eMIDMessageLevel.Edit;
			}
			catch (SizePrimaryRequiredException)
			{
				message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizePrimaryRequired, false) + ":" + aSizeCodeProf.SizeCodeID;
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, GetType().Name);
				returnCode = eMIDMessageLevel.Edit;
			}
			catch
			{
				throw;
			}

			if (returnCode != eMIDMessageLevel.None)
			{
				++_recordsWithErrors;
			}
			else
				if (aSizeCodeProf.SizeCodeChangeType == eChangeType.add)
			{
				++_addRecords;
			}
			else
			{
				++_updateRecords;
			}
						
			return returnCode;
		}

	}

	/// <summary>
	/// Local class's exception type.
	/// </summary>
	public class XMLSizeCodeLoadProcessException : Exception
	{
		/// <summary>
		/// Used when throwing exceptions in the XML SizeCode Load Class
		/// </summary>
		/// <param name="message">The error message to display</param>
		public XMLSizeCodeLoadProcessException(string message): base(message)
		{
		}
		public XMLSizeCodeLoadProcessException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}
