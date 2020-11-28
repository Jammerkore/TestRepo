using System;
using System.Windows.Forms;

using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class BatchMessageCallback : MarshalByRefObject, IMessageCallback
	{
		public DialogResult HandleMessage(string aText, string aCaption, MessageBoxButtons aButtons, MessageBoxIcon aIcon)
		{
			return DetermineDefaultResult(aButtons);
		}
		
		public DialogResult HandleMessage(eMIDTextCode aMsgCode, string aCaption, MessageBoxButtons aButtons, MessageBoxIcon aIcon)
		{
			switch (aMsgCode)
			{
				case eMIDTextCode.msg_ContinueWithoutScheduler :
					return DialogResult.OK;

				default:
					return DetermineDefaultResult(aButtons);
			}
		}
		
		override public object InitializeLifetimeService()
		{
			return null;
		}

		private DialogResult DetermineDefaultResult(MessageBoxButtons aButtons)
		{
			try
			{
				switch (aButtons)
				{
					case MessageBoxButtons.OK :
						return DialogResult.OK;

					case MessageBoxButtons.OKCancel :
					case MessageBoxButtons.RetryCancel :
					case MessageBoxButtons.YesNoCancel :
						return DialogResult.Cancel;

					case MessageBoxButtons.YesNo :
						return DialogResult.No;

					case MessageBoxButtons.AbortRetryIgnore :
						return DialogResult.Abort;

					default :
						return DialogResult.None;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
