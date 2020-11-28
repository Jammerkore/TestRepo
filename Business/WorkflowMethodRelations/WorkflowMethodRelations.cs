//using System;
//using System.IO;
//using System.Data;
//using System.Collections;
//using System.Reflection.Emit;
//
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.DataCommon;
//
//namespace MIDRetail.Business
//{
//	/// <summary>
//	/// Summary description for WorkflowMethodRelations.
//	/// </summary>
//	public class WorkflowMethodRelations
//	{
//		private WorkflowRelations _workflowRel = null;
//		private MethodRelations _methodRel = null;
//		private int _userRID;
//
//		public int UserRID
//		{
//			get { return _userRID ; }
//		}
//
//		public WorkflowMethodRelations(int userRID, bool includeVirtual)
//		{
//			_userRID = userRID;
//			_workflowRel = new WorkflowRelations();
//			_methodRel = new MethodRelations(includeVirtual);
//		}
//
//		#region Methods
//		//8-15-03 sjd keep
//		public DataTable GetMethodList(eGlobalUserType globalUserType, eMethodType MethodType)
//		{
//			return _methodRel.GetMethodList(UserRID, globalUserType, MethodType);
//		}
//
//		public MethodProfile GetMethodProfile(int key)
//		{
//			return _methodRel.GetMethodProfile(key);
//		}
//
//		public MIDRetail.Business.OTSPlanMethod GetMethodProfile(SessionAddressBlock sab, int key)
//		{
//			return _methodRel.GetMethodProfile(sab, key);
//		}
//
////		public int AddMethod(MethodProfile methodProfile)
////		{
////			return _methodRel.AddMethod(methodProfile);
////		}
//
//		public int AddMethod(TransactionData td, Profile methodProfile)
//		{
//			return _methodRel.AddMethod(td, methodProfile);
//		}
//
//		#endregion
//	}
//}
