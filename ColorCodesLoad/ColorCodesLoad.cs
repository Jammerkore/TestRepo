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

namespace MIDRetail.ColorCodesLoad
{
	/// <summary>
	/// Entry point for color codes load.
	/// </summary>
	class ColorCodesLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			ColorLoadWorker cw = new ColorLoadWorker();
			return cw.LoadColors(args);
		}
	}

	public class ColorLoadWorker
	{
		private Colors _colors = null;
		string sourceModule = "ColorCodesLoad.cs";
		string eventLogID = "MIDColorCodesLoad";
		string message = null;
		SessionAddressBlock _SAB;
		SessionSponsor _sponsor;
		IMessageCallback _messageCallback;
        Dictionary<string, int> _colorIDHash;
		int _recordsRead = 0;
		int _addRecords = 0;
		int _updateRecords = 0;
		int _recordsWithErrors = 0;
		eMIDMessageLevel highestMessage;

		string fileLocation = null;
		char[] delimiter = {'~'};
		bool errorFound = false;
		System.Runtime.Remoting.Channels.IChannel channel;
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

		public int LoadColors(string[] args)
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
					MIDConfigurationManager.AppSettings["Password"], eProcesses.colorCodeLoad);

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
                            // Begin TT#1054 - JSmith - Relieve Intransit not working.
                            //delimiter = ConfigurationSettings.AppSettings["Delimiter"].ToCharArray();
                            delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                            // End TT#1054
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
					message += "Color Codes Load Process NOT run";

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
						message += "Color Codes Load Process NOT run";

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
							message += "Color Codes Load Process NOT run";

							_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule, true);

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
								_colorIDHash = _SAB.HierarchyServerSession.GetColorCodeListByID();

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
						_SAB.ClientServerSession.Audit.ColorCodeLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords);
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
						_SAB.ClientServerSession.Audit.ColorCodeLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords);
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
			eMIDMessageLevel returnCode =eMIDMessageLevel.None;

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
					++_recordsRead;
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

					returnCode = AddColorRecord(fields);

					if (returnCode != eMIDMessageLevel.None)
					{
						++_recordsWithErrors;
						exceptionFile.WriteLine(line);
					}
				}
			}

			catch ( FileNotFoundException )
			{
				errorFound = true;
				message = " : " + fileLocation;
                // Begin TT#604 - JSmith - Audit not marked as serializable
                //_SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message);
                //_SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message);
                _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
                // End TT#604
			}
			catch ( Exception ex )
			{
				errorFound = true;
                // Begin TT#604 - JSmith - Audit not marked as serializable
                //_SAB.HierarchyServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                //_SAB.HierarchyServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                // End TT#604

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

		private eMIDMessageLevel AddColorRecord(string[] fields)
		{
			eMIDMessageLevel returnCode = eMIDMessageLevel.None;
			string message = null;
			EditMsgs em = new EditMsgs();

			try
			{
				ColorCodeProfile ccp = new ColorCodeProfile(-1);
                //Begin TT#819 - JSmith - Object reference errors generated in the Color Codes load
                //message =  "Code: " + ((fields[0].Trim().Length != 0) ? fields[0] : " ") 
                //    + "; Name: " + ((fields.Length > 1 && fields[1] != null && fields[1].Trim().Length > 0) ? fields[1] : " ") 
                //    + "; Group: " + ((fields.Length > 2 && fields[2] != null && fields[2].Trim().Length > 0) ? fields[2] : " ");
                message = "Code: " + ((fields[0] != null && fields[0].Trim().Length != 0) ? fields[0] : " ")
                    + "; Name: " + ((fields.Length > 1 && fields[1] != null && fields[1].Trim().Length > 0) ? fields[1] : " ")
                    + "; Group: " + ((fields.Length > 2 && fields[2] != null && fields[2].Trim().Length > 0) ? fields[2] : " ");
                //End TT#819

                //Begin TT#819 - JSmith - Object reference errors generated in the Color Codes load
                //if (fields[0].Length == 0)
                if (fields[0] == null || fields[0].Trim().Length == 0)
                //End TT#819
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CodeIsRequired, message, GetType().Name);
				}
				else
				{
					ccp.ColorCodeID = fields[0].Trim();
				}

				if (fields.Length > 1 && fields[1] != null && fields[1].Trim().Length > 0)
				{
					ccp.ColorCodeName = fields[1].Trim();
				}
				else
				{
					ccp.ColorCodeName = ccp.ColorCodeID;
				}

				if (fields.Length > 2 && fields[2] != null && fields[2].Trim().Length > 0)
				{
					ccp.ColorCodeGroup = fields[2].Trim();
				}
				else
				{
					ccp.ColorCodeGroup = null;
				}

				if (ccp.ColorCodeID != null)
				{
                    //if (_colorIDHash.Contains(ccp.ColorCodeID))
                    int key;
                    if (_colorIDHash.TryGetValue(ccp.ColorCodeID, out key))
					{
                        //ccp.Key = (int)_colorIDHash[ccp.ColorCodeID];
                        ccp.Key = key;
						ccp.ColorCodeChangeType = eChangeType.update;
						++_updateRecords;
					}
					else
					{
						ccp.ColorCodeChangeType = eChangeType.add;
						++_addRecords;
					}
				}
				
				if (em.ErrorFound)  
				{
					returnCode = eMIDMessageLevel.Error;
					for (int e=0; e<em.EditMessages.Count; e++)
					{
						EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
						_SAB.ClientServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
					}
				}
				else
				{
					_SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
				}

			}
			catch (Exception ex)
			{
                message = ex.ToString();
				throw;
			}

			return returnCode;

		}


		public bool LoadXMLTransFile(string fileLocation)
		{

			string message = null;
			if(!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
			{
				throw new XMLColorCodeLoadProcessException(String.Format("Can not find the file located at '{0}'", fileLocation));
			}
            // Begin Track #4229 - JSmith - API locks .XML input file
            TextReader r = null;
            // End Track #4229
			try
			{
				XmlSerializer s = new XmlSerializer(typeof(Colors));	// Create a Serializer
                // Begin Track #4229 - JSmith - API locks .XML input file
                //TextReader r = new StreamReader(fileLocation);			// Load the Xml File
                r = new StreamReader(fileLocation);			// Load the Xml File
                // End Track #4229
				_colors = (Colors)s.Deserialize(r);						// Deserialize the Xml File to a strongly typed object
                // Begin Track #4229 - JSmith - API locks .XML input file
                //r.Close();												// Close the input file.
                // End Track #4229
			}
			catch(Exception ex)
			{
				throw new XMLColorCodeLoadProcessException(String.Format("Error encountered during deserialization of the file '{0}'", fileLocation), ex);
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
				ColorCodeProfile ccp;
				foreach(ColorsColor c in _colors.Color)
				{
					EditMsgs em = new EditMsgs();
					++_recordsRead;
					ccp = new ColorCodeProfile(-1);	// Create a new color code profile
					message =  "Code: " + ((c.Code != null) ? c.Code : " ") + "; Name: " + ((c.Name != null) ? c.Name : " ") + "; Group: " + ((c.Group != null) ? c.Group : " ");
					
                    //if (c.Code == null)
					if (c.Code == null || (c.Code.Trim()).Length == 0) //TT#439 - MD - A zero length strig value of the CodeID is also an error - RBeck
					{
						em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CodeIsRequired, message, GetType().Name);

                        ccp.ColorCodeID = "Unknown";    //TT#439 - MD - A zero length strig value of the CodeID is also an error - RBeck

					}
					else
					{
						ccp.ColorCodeID = c.Code.Trim();
					}
					if (c.Name != null)
					{
						ccp.ColorCodeName = c.Name.Trim();
					}
					else
					{
						ccp.ColorCodeName = ccp.ColorCodeID;
					}

                    //ccp.ColorCodeGroup = c.Group.Trim(); //TT#264- MD - Trim an allowed null causes error - RBeck
                    ccp.ColorCodeGroup = ((c.Group != null) ? c.Group.Trim() : null);

                    //if (_colorIDHash.Contains(ccp.ColorCodeID))
                    int key;
                    if (_colorIDHash.TryGetValue(ccp.ColorCodeID, out key))
					{
                        //ccp.Key = (int)_colorIDHash[ccp.ColorCodeID];
                        ccp.Key = key;
						ccp.ColorCodeChangeType = eChangeType.update;
						++_updateRecords;
					}
					else
					{
						ccp.ColorCodeChangeType = eChangeType.add;
						++_addRecords;
					}

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
						_SAB.HierarchyServerSession.ColorCodeUpdate(ccp); // Add the color
					}
				}
			}
			catch(Exception ex)
			{
				throw new XMLColorCodeLoadProcessException(String.Format("Error encountered while processing the file '{0}'", fileLocation), ex);
			}
			return false;
		}


	}

	/// <summary>
	/// Local class's exception type.
	/// </summary>
	public class XMLColorCodeLoadProcessException : Exception
	{
		/// <summary>
		/// Used when throwing exceptions in the XML ColorCode Load Class
		/// </summary>
		/// <param name="message">The error message to display</param>
		public XMLColorCodeLoadProcessException(string message): base(message)
		{
		}
		public XMLColorCodeLoadProcessException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}
