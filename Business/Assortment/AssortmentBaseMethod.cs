using System;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
namespace MIDRetail.Business
{
	/// <summary>
	/// The base assortment business method.
	/// </summary>
	/// <remarks>
	/// This method inherits from the base application method.  
	/// </remarks>
	abstract public class AssortmentBaseMethod:ApplicationBaseMethod
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of Assortment Base Method
		/// </summary>
		/// <param name="aMethodRID">RID for the Method.</param>
		/// <param name="aMethodType">Method Type.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public AssortmentBaseMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType aMethodType):base(SAB, aMethodRID, aMethodType)
		public AssortmentBaseMethod(SessionAddressBlock SAB, int aMethodRID, eMethodType aMethodType, eProfileType aProfileType):base(SAB, aMethodRID, aMethodType, aProfileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{

		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		internal override void VerifyAction(eMethodType aMethodType)
		{
			if (!Enum.IsDefined(typeof(eAssortmentMethodType),Convert.ToInt32(base.MethodType, CultureInfo.CurrentUICulture)))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
					MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
			}
		}
	}

}
