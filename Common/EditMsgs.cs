using System;
using System.Collections;
using MID.MRS.DataCommon;

namespace MID.MRS.Common
{
	/// <summary>
	/// Summary description for EditMsgs.
	/// </summary>
	public class EditMsgs
	{
		bool _errorFound = false;
		eChangeType _changeType;
		ArrayList _editMessages = new ArrayList();
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
				messageLevel == eMIDMessageLevel.Error ||
				messageLevel == eMIDMessageLevel.Severe)
			{
				ErrorFound = true;
			}
		}
	}
}
