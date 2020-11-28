using System;
using System.IO;
using System.Collections;
using System.Data;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Reflection.Emit;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for Method.
	/// </summary>
	public class Method
	{
		public Method()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataTable GetAllMethods()
		{
			MethodBaseData methodBase = new MethodBaseData();
			return methodBase.GetMethods();	 
		}

//		public DataTable GetAllOTSPlanMethods()
//		{
//			MethodOTSPlan methodOTSPlan = new MethodOTSPlan();
//			return methodOTSPlan.GetAllOTSPlans();	 
//		}		

		//GetAllGroupLevelFunctions by Method_RID
		//GetGroupLevelFunction by Method_RID & SGL_RID

		//Get Basis_Plan & Get Basis_Range
	}
}
