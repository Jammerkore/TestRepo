using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.StoreLoad
{
	/// <summary>
	/// Base class to provide batch load capabilities to sessions.
	/// Marked abstract because some manipulation of the SAB object
	/// is required prior to calling BatchLoad.
	/// </summary>
	public abstract class LoadBase
	{
		#region Internal Variables
		private SessionAddressBlock _SAB;
		private SessionSponsor _sponsor;
		private IMessageCallback _messageCallback;
        //private BinaryServerFormatterSinkProvider _provider;
        //private Hashtable _port;
		private System.Runtime.Remoting.Channels.IChannel _channel;
		private string _fileLocation = null;
		private string _exceptionFile = null;
		private char[] _delimiter = null;
        // Begin TT#796 - JSmith - Security violation running application on Windows Server 2008
        private string _eventLogID;
        // End TT#796
        // Begin TT#166 - JSmith - Store Characteristics auto add
        private bool _autoAddCharacteristics = false;
        private char[] _characteristicDelimiter = null;
        // End TT#166
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//		private int _processRID = -1;
		private int _processId = Include.NoRID;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		private int _commitLimit = -1;
//		private bool _clientServerInitialized = false;
		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
        private bool _useCharacteristicTransaction = false;
		// END TT#1401 - stodd - add resevation stores (IMO)
		#endregion
		#region Initializers
		/// <summary>
		/// Initializes the eventlog and other base properties.
		/// </summary>
        // Begin TT#796 - JSmith - Security violation running application on Windows Server 2008
        //public LoadBase()
        public LoadBase(string aEventLogID)
        // End TT#796
		{
            // Begin TT#796 - JSmith - Security violation running application on Windows Server 2008
            eventLogID = aEventLogID;
            // End TT#796
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
				_channel = _SAB.OpenCallbackChannel();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
				throw;
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// A reference to the internal SessionAddressBlock.
		/// The client session must be created and logged in prior to calling BatchLoad
		/// </summary>
		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}
		/// <summary>
		/// A reference to the internal SessionSponsor.
		/// </summary>
		public SessionSponsor sponsor
		{
			get
			{
				return _sponsor;
			}
		}
		/// <summary>
		/// A reference to the internal MessageCallback
		/// </summary>
		public IMessageCallback messageCallback
		{
			get
			{
				return _messageCallback;
			}
		}
		/// <summary>
		/// The eventLogID that will be used for logging
		/// </summary>
        // Begin TT#796 - JSmith - Security violation running application on Windows Server 2008
        //public string eventLogID
        //{
        //    get
        //    {
        //        return "MRS" + this.ToString();
        //    }
        //}
        public string eventLogID
		{
			get
			{
                return _eventLogID;
			}
            set
            {
                _eventLogID = value;
            }
		}
        // End TT#796
		/// <summary>
		/// The sourceModule that will be used for logging
		/// </summary>
		public string sourceModule
		{
			get
			{
				return this.ToString() + ".cs";
			}
		}
		/// <summary>
		/// The fileLocation used for the batch load.
		/// Must be set if not defined as 'InputFile' in the config file.
		/// </summary>
		public string fileLocation
		{
			get
			{
				if(_fileLocation == null)
					if(MIDConfigurationManager.AppSettings["InputFile"] != null)
						_fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
				return _fileLocation;
			}
			set
			{
				_fileLocation = value;
			}
		}
		/// <summary>
		///  The exceptionFile that will be used for the batch load.
		///  Defaults to .\exceptionFile.txt if 'ExceptionFile' is not defined in the config file.
		/// </summary>
		public string exceptionFile
		{
			get
			{
				if(_exceptionFile == null)
					if(MIDConfigurationManager.AppSettings["ExceptionFile"] != null)
						_exceptionFile = MIDConfigurationManager.AppSettings["ExceptionFile"];
					else
						_exceptionFile = @".\exceptionFile.txt";
				return _exceptionFile;
			}
			set
			{
				_exceptionFile = value;
			}
		}
		/// <summary>
		/// The commitLimit used by batch load.
		/// Defaults to 1000 if 'CommitLimit' is not defined in the config file.
		/// </summary>
		public int commitLimit
		{
			get
			{
				if(_commitLimit == -1)
					if(MIDConfigurationManager.AppSettings["CommitLimit"] != null)
						_commitLimit = Convert.ToInt32(MIDConfigurationManager.AppSettings["CommitLimit"]);
					else
						_commitLimit = 1000;
				return _commitLimit;
			}
			set
			{
				_commitLimit = value;
			}
		}
		/// <summary>
		/// The delimiter used by batch load.
		/// Defaults to ~.
		/// Ignored when processing .XML files.
		/// </summary>
		public char[] delimiter
		{
			get
			{
				if(_delimiter == null)
					if(MIDConfigurationManager.AppSettings["Delimiter"] != null)
						_delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
					else
						_delimiter = new char[] {'~'};
				return _delimiter;
			}
			set
			{
				_delimiter = value;
			}
		}

        // Begin TT#166 - JSmith - Store Characteristics auto add
        /// <summary>
        /// Identifies is characteristics are to be auto added
        /// </summary>
        /// <remarks>Defaults to False</remarks>
        public bool autoAddCharacteristics
        {
            get
            {
                if (MIDConfigurationManager.AppSettings["AutoAddCharacteristics"] != null)
                {
                    _autoAddCharacteristics = Convert.ToBoolean(MIDConfigurationManager.AppSettings["AutoAddCharacteristics"]);
                }
                return _autoAddCharacteristics;
            }
            set
            {
                _autoAddCharacteristics = value;
            }
        }

        /// <summary>
        /// The delimiter used to delimite characteristic types from values.
        /// Defaults to \.
        /// </summary>
        public char[] characteristicDelimiter
        {
            get
            {
                if (_characteristicDelimiter == null)
                    if (MIDConfigurationManager.AppSettings["CharacteristicDelimiter"] != null)
                        _characteristicDelimiter = MIDConfigurationManager.AppSettings["CharacteristicDelimiter"].ToCharArray();
                    else
                        _characteristicDelimiter = new char[] { '\\' };
                return _characteristicDelimiter;
            }
            set
            {
                _characteristicDelimiter = value;
            }
        }
        // End TT#166

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		/// <summary>
		/// Indicates whether the newer transaction types ("S", "C") will be used.
		/// Defaults to False.
		/// </summary>
        public bool UseCharacteristicTransaction
		{
			get
			{
                if (MIDConfigurationManager.AppSettings["UseCharacteristicTransaction"] != null)
				{
                    _useCharacteristicTransaction = Convert.ToBoolean(MIDConfigurationManager.AppSettings["UseCharacteristicTransaction"]);
				}
                return _useCharacteristicTransaction;
			}
			set
			{
                _useCharacteristicTransaction = value;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//		/// <summary>
//		/// The processRID that will be used to initalize the ClientServerSession.
//		/// Defaults to -1.
//		/// Ignored if -1.
//		/// </summary>
//		public int processRID
//		{
//			get
//			{
//				return _processRID;
//			}
//			set
//			{
//				_processRID = value;
//			}
//		}
		/// <summary>
		/// The processId that will be used to initalize the ClientServerSession.
		/// Defaults to -1.
		/// </summary>
		public int processId
		{
			get
			{
				return _processId;
			}
		}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		#endregion
		#region Methods
		/// <summary>
		/// Performs a batch load.
		/// Will pass the file specified by the fileLocation property with the delimiter and commitLimit specified
		/// to the passed in batchSession.
		/// </summary>
		/// <param name="batchSession">Any class that implements the BatchLoadData interface</param>
		/// <returns>True on success, false on failure</returns>
		public bool BatchLoad(IBatchLoadData batchSession)
		{
			string message = null;
//			if(!_clientServerInitialized)
//			{
//				if(_processRID == -1)
//				{
//					SAB.ClientServerSession.Initialize(_processRID);
//					SAB.ClientServerSession.Audit.UpdateHeader(eProcessStatus.running, eMIDTextCode.sum_Running, "");
//				}
//				else
//					SAB.ClientServerSession.Initialize();
//				_clientServerInitialized = true;
//			}
// (CSMITH) - BEG MID Track #2979: Empty Input File
			// A little sanity check. Without these two items, we will always fail.
//			if(fileLocation == null || batchSession == null)
//			{
//				if(fileLocation == null)
//				{
//					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.sum_InputFileNotFound, sourceModule);
//				}
//				return false;
//			}
//			message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);
//			message = message.Replace("{0}", fileLocation);
//			SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
//			if(fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
//				return batchSession.LoadXMLTransFile(SAB, fileLocation, commitLimit, exceptionFile);
//			else
//				return batchSession.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile);

			if (batchSession == null)
			{
				message = "Batch Session NOT specified" + System.Environment.NewLine;
				message += "Store Load Process NOT run";

				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);

				return false;
			}

			message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

			if (fileLocation == "" || fileLocation == null)
			{
				message = message.Replace("{0}.", "[" + fileLocation + "] NOT specified" + System.Environment.NewLine);
				message += "Store Load Process NOT run";

				// Begin Track #5035 - JSmith - file not found message level inconsistent
//				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule);
				// End Track #5035

				return false;
			}
			else
			{
				if (!File.Exists(fileLocation))
				{
					message = message.Replace("{0}.", "[" + fileLocation + "] does NOT exist" + System.Environment.NewLine);
					message += "Store Load Process NOT run";

					SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);

					return false;
				}
				else
				{
					FileInfo txnFileInfo = new FileInfo(fileLocation);

					if (txnFileInfo.Length == 0)
					{
						message = message.Replace("{0}.", "[" + fileLocation + "] is an empty file" + System.Environment.NewLine);
						message += "Store Load Process NOT run";

						// Begin Track #5035 - JSmith - file not found message level inconsistent
//						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.NothingToDo, message, sourceModule);
						// End Track #5035

						return false;
					}
					else
					{
						message = message.Replace("{0}", "[" + fileLocation + "]");

						SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);

						// BEGIN TT#739-MD - STodd - delete stores
						bool errorFound = false;
						// END TT#739-MD - STodd - delete stores
						if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
						{
                            // Begin TT#166 - JSmith - Store Characteristics auto add
                            //return batchSession.LoadXMLTransFile(SAB, fileLocation, commitLimit, exceptionFile);
                            errorFound = batchSession.LoadXMLTransFile(SAB, fileLocation, commitLimit, exceptionFile,
								autoAddCharacteristics);
                            // End TT#166
							// BEGIN TT#739-MD - STodd - delete stores
							// Removed from here to place in it's own API
							//if (!errorFound)
							//{
							//    errorFound = batchSession.DeleteStoreBatchProcess(SAB);
							//}
							return errorFound;
							// END TT#739-MD - STodd - delete stores
						}
						else
						{
							// BEGIN TT#1401 - stodd - add resevation stores (IMO)
                            // Begin TT#166 - JSmith - Store Characteristics auto add
                            //return batchSession.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile);
                            errorFound = batchSession.LoadDelimitedTransFile(SAB, fileLocation, delimiter, commitLimit, exceptionFile,
								autoAddCharacteristics, characteristicDelimiter, UseCharacteristicTransaction);
                            // End TT#166
							// END TT#1401 - stodd - add resevation stores (IMO)
							// BEGIN TT#739-MD - STodd - delete stores
							// Removed from here to place in it's own API
							//if (!errorFound)
							//{
							//    errorFound = batchSession.DeleteStoreBatchProcess(SAB);
							//}
							return errorFound;
							// END TT#739-MD - STodd - delete stores
						}
					}
				}
			}
// (CSMITH) - END MID Track #2979
		}
		/// <summary>
		/// Performs a BatchLoad.
		/// Takes an args list typically entered from the command line and extracts the appropriate
		/// parameters for use in calling the passed batch session objects Load method. Un-set args
		/// will be loaded from the configuration file.
		/// </summary>
		/// <param name="args">args[0]=filename, args[1]=delimiter, args[2]=processRID</param>
		/// <param name="batchSession">Any class that implements the BatchLoadData interface</param>
		/// <returns>True on success, false on failure</returns>
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//		public bool BatchLoad(string[] args, IBatchLoadData batchSession)
		public void ProcessArgs(string[] args)
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		{
			if (args.Length > 0)
			{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				if (args[0] == Include.SchedulerID)
				{
					_fileLocation = args[1];
					_processId = Convert.ToInt32(args[2]);
					_delimiter = delimiter;
				}
				else
				{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					_fileLocation = args[0];
					if (args.Length > 1)
					{
						_delimiter = args[1].ToCharArray();
						if (args.Length > 2)
						{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//							_processRID = Convert.ToInt32(args[2]);
							_processId = Convert.ToInt32(args[2]);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
						}
					}
					else	// delimiter un-specified
					{
						_delimiter = delimiter;
					}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
				}
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			}
			else	// file unspecified
			{
//				SAB.ClientServerSession.Initialize();
				_fileLocation = fileLocation;
				_delimiter = delimiter;
			}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//			return BatchLoad(batchSession);
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		}
		#endregion
	}
}
