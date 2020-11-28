using System;

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for MIDClipboard.
	/// </summary>
	public class MIDClipboard
	{
		static private char[] _MIDClipboardDelimiter = {'^'};
		static private string _MIDClipboardNode = "NODE";
		static MIDClipboard()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		static public char[] MIDClipboardDelimiter
		{
			get
			{
				return _MIDClipboardDelimiter;
			}
		}

		static public string MIDClipboardStringDelimiter
		{
			get
			{
				return new string(MIDClipboard.MIDClipboardDelimiter);
			}
		}

		static public string MIDClipboardNode
		{
			get
			{
				return _MIDClipboardNode;
			}
		}
	}
}
