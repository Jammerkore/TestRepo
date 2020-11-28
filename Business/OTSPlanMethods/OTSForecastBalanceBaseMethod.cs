using System;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The base OTS Plan business method
	/// </summary>
	/// <remarks>
	/// This method inherits from the base application method
	/// </remarks>
	public abstract class OTSForecastBalanceBaseMethod : ApplicationBaseMethod
	{
		/// <summary>
		/// Creates an instance of OTS Plan Base Method
		/// </summary>
		/// <param name="SAB">SessionAddressBlock</param>
		/// <param name="aMethodRID">RID for the Method.</param>
		/// <param name="aMethodType">Method Type.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public OTSForecastBalanceBaseMethod(SessionAddressBlock SAB, int aMethodRID, 
		//    eMethodType aMethodType):base(SAB, aMethodRID, aMethodType)
		public OTSForecastBalanceBaseMethod(SessionAddressBlock SAB, int aMethodRID, 
			eMethodType aMethodType, eProfileType aProfileType):base(SAB, aMethodRID, aMethodType, aProfileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		internal override void VerifyAction(eMethodType aMethodType)
		{
			if (!Enum.IsDefined(typeof(eForecastMethodType), Convert.ToInt32(base.MethodType, CultureInfo.CurrentUICulture)))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
					MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
			}
		}
	}
}
