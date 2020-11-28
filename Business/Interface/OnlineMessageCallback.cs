using System;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	public class OnlineMessageCallback : MarshalByRefObject, IMessageCallback
	{
// Begin Alert Events Code -- DO NOT REMOVE
//		public void HandleAlert(AlertEventArgs aAlertEventArgs)
//		{
//			MessageBox.Show(aAlertEventArgs.Message);
//		}
//
// End Alert Events Code -- DO NOT REMOVE
		public DialogResult HandleMessage(string aText, string aCaption, MessageBoxButtons aButtons, MessageBoxIcon aIcon)
		{
			return MessageBox.Show(aText, aCaption, aButtons, aIcon, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
		}

		public DialogResult HandleMessage(eMIDTextCode aMsgCode, string aCaption, MessageBoxButtons aButtons, MessageBoxIcon aIcon)
		{
			return MessageBox.Show(MIDText.GetTextOnly(aMsgCode), aCaption, aButtons, aIcon, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);;
		}
		
		override public object InitializeLifetimeService()
		{
			return null;
		}
	}
}
