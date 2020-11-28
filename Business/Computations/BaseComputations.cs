using System;
using System.Collections.Generic;
using System.Text;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Abstract.  The PlanComputations class is a base class for Computations classes.
	/// </summary>

	abstract public class BaseComputations
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public BaseComputations()
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the ToolBox object.
		/// </summary>

		abstract public BaseToolBox ToolBox { get; }

		//========
		// METHODS
		//========

		public static string GetUserErrorMessage(int aMsgNumber, string aMessage)
		{
			string customMsg;

			try
			{
				customMsg = MIDText.GetTextOnly((int)eMIDTextCode.msg_CustomUserException + aMsgNumber);

				if (customMsg == string.Empty || customMsg != aMessage)
				{
					MIDText.OpenUpdateConnection();
					MIDText.Update((int)eMIDTextCode.msg_CustomUserException + aMsgNumber, aMessage, (int)eMIDMessageLevel.Error);
					MIDText.CommitData();
					customMsg = aMessage;
				}

				return customMsg;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
