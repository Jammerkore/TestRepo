using System;
using System.Collections;
// Begin MID Track #4906 - JSmith - database errors during autoadd
//using MIDRetail.DataCommon;
// End MID Track #4906

// Begin MID Track #4906 - JSmith - database errors during autoadd
//namespace MIDRetail.Common
namespace MIDRetail.DataCommon
// End MID Track #4906
{
	/// <summary>
	/// Summary description for EditMsgs.
	/// </summary>
	// Begin MID Track #4906 - JSmith - database errors during autoadd
	[Serializable()]
	// End MID Track #4906
	public class EditMsgs
	{
		bool _errorFound = false;
		//Begin TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
		bool _errorToDisplay = false;
		//End TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
        bool _displayPrompt = false;
		eChangeType _changeType;
		ArrayList _editMessages = new ArrayList();
		// Begin MID Track #5092 - JSmith - Serialization error
		[Serializable()]
		// End MID Track #5092
		public struct Message
		{
			public eMIDMessageLevel messageLevel;
			public bool messageByCode;
			public eMIDTextCode code;
			public string msg;
			public int lineNumber;
			public string module;
		}

//		public eMIDMessageLevel MessageLevel
//		{
//			get { return Message.messageLevel ; }
//			set { Message.messageLevel = value; }
//		}
//
//		public bool MessageByCode
//		{
//			get { return messageByCode ; }
//			set { messageByCode = value; }
//		}
//
//		public eMIDTextCode TextCode
//		{
//			get { return code ; }
//			set { code = value; }
//		}
//
//		public string Msg
//		{
//			get { return msg ; }
//			set { msg = value; }
//		}
//
//		public int LineNumber
//		{
//			get { return lineNumber ; }
//			set { lineNumber = value; }
//		}
//
//		public string Module
//		{
//			get { return module ; }
//			set { module = value; }
//		}

		public EditMsgs()
		{
			_changeType = eChangeType.none;
		}

		public bool ErrorFound
		{
			get { return _errorFound ; }
			set { _errorFound = value; }
		}

		//Begin TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
		public bool ErrorToDisplay
		{
			get { return _errorToDisplay; }
			set { _errorToDisplay = value; }
		}

        public bool DisplayPrompt
        {
            get { return _displayPrompt; }
            set { _displayPrompt = value; }
        }

		//End TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
		public ArrayList EditMessages 
		{
			get { return _editMessages ; }
			set { _editMessages = value; }
		}

		public eChangeType ChangeType 
		{
			get { return _changeType ; }
			set { _changeType = value; }
		}

		public void ClearMsgs()
		{
			EditMessages.Clear();
			ErrorFound = false;
		}

//		public void AddMsg(eErrorLevel messageLevel, eMIDTextCode msgCode, string msg)
		public void AddMsg(eMIDMessageLevel messageLevel, eMIDTextCode msgCode, string reportingModule)
		{
			EditMsgs.Message message = new EditMsgs.Message();
			message.messageLevel = messageLevel;
			message.code = msgCode;
			message.messageByCode = true;
// line number is only available if during a debug compile 
#if (DEBUG)
			message.lineNumber = new System.Diagnostics.StackFrame(1,true).GetFileLineNumber();
#endif
			message.module = reportingModule;
			// if no module, attempt to get from diagnostics
			if (message.module == null)
			{
				message.module = new System.Diagnostics.StackFrame(1,true).GetFileName();
			}
//			message.msg = msg;
			EditMessages.Add(message);
			if (messageLevel == eMIDMessageLevel.Edit ||
				messageLevel == eMIDMessageLevel.Error ||
				messageLevel == eMIDMessageLevel.Severe)
			{
				ErrorFound = true;
			}
		}

		public void AddMsg(eMIDMessageLevel messageLevel, eMIDTextCode msgCode, string msg, string reportingModule)
		{
			EditMsgs.Message message = new EditMsgs.Message();
			message.messageLevel = messageLevel;
			message.msg = msg;
			message.code = msgCode;
			message.messageByCode = true;
// line number is only available if during a debug compile
#if (DEBUG)
			message.lineNumber = new System.Diagnostics.StackFrame(1,true).GetFileLineNumber();
#endif
			message.module = reportingModule;
			// if no module, attempt to get from diagnostics
			if (message.module == null)
			{
				message.module = new System.Diagnostics.StackFrame(1,true).GetFileName();
			}
			//			message.msg = msg;
			EditMessages.Add(message);
			if (messageLevel == eMIDMessageLevel.Edit ||
				messageLevel == eMIDMessageLevel.Error ||
				messageLevel == eMIDMessageLevel.Severe)
			{
				ErrorFound = true;
			}
		}

		public void AddMsg(eMIDMessageLevel messageLevel, string msg, string reportingModule)
		{
			EditMsgs.Message message = new EditMsgs.Message();
			message.messageLevel = messageLevel;
			message.msg = msg;
			message.messageByCode = false;
// line number is only available if during a debug compile
#if (DEBUG)
			message.lineNumber = new System.Diagnostics.StackFrame(1,true).GetFileLineNumber();
#endif
			message.module = reportingModule;
			// if no module, attempt to get from diagnostics
			if (message.module == null)
			{
				message.module = new System.Diagnostics.StackFrame(1,true).GetFileName();
			}
			EditMessages.Add(message);
			if (messageLevel == eMIDMessageLevel.Edit ||
				//Begin TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
				messageLevel == eMIDMessageLevel.HandledEdit ||
				//End TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
				messageLevel == eMIDMessageLevel.Error ||
				messageLevel == eMIDMessageLevel.Severe)
			{
				ErrorFound = true;
				//Begin TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.

				if (messageLevel != eMIDMessageLevel.HandledEdit)
				{
					ErrorToDisplay = true;
				}
				//End TT#429 - JScott - Size Curve- Node Properties When requesting different size groups for the same level need a Conflict Warning.
			}
		}
	}
}
