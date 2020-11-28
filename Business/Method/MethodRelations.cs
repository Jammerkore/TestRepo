//using System;
//using System.IO;
//using System.Collections;
//using System.Data;
//using System.Diagnostics;
//
//using MIDRetail.Data;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//using System.Reflection.Emit;
//
//namespace MIDRetail.Business
//{
//	/// <summary>
//	/// Summary description for MethodRelations.
//	/// </summary>
//	public class MethodRelations
//	{
//		private Method _method = null;
//		private MethodBase _methodBase = null;
//
//		private DataTable _dtAllMethods = null;
//
//		private Hashtable _allMethodsList = new Hashtable();
//		private Hashtable _allMethodsByLookupKeysList = new Hashtable();
//
//		private static int uniqueID = -1;
//
//		public static int GetUniqueID()
//		{
//			return uniqueID++;
//		}
//
//		public Hashtable AllMethodsList
//		{
//			get {return _allMethodsList;}
//		}
//
//		public Hashtable AllMethodsByLookupKeysList
//		{
//			get {return _allMethodsByLookupKeysList;}
//		}
//
//		public MethodRelations(bool includeVirtual)
//		{
//			_method = new Method();
//			_methodBase = new MethodBase();
//
//			_dtAllMethods = _method.GetAllMethods();
//
//
//			PopulateMethodList(includeVirtual);
//		}
//
//		//Populate MethodKeyProfile & MethodProfile & creates hashtable for each
//		//MethodKeyProfile contains ProfileList of MethodProfiles
//		private void PopulateMethodList(bool includeVirtual)
//		{
//			_allMethodsList.Clear();
//			_allMethodsByLookupKeysList.Clear();
//
//			//Generic int Key for MethodKeyProfile
//			//int i = 0;
//
//			DataView dv = new DataView(_dtAllMethods);
//			dv.RowFilter = "VIRTUAL_IND = '" + Include.ConvertBoolToChar(includeVirtual) + "'";
//			DataRowView methodDataRow;
//
//			for (int i = 0; i < dv.Count; i++)
//			{	methodDataRow = dv[i];
//				MethodKeyProfile methodKey = ConvertToMethodKeyProfile(methodDataRow, GetUniqueID());		
//				MethodProfile currMethod = ConvertToMethodProfile(methodDataRow);
//
//				//8-14-03
//				//Add Method Profiles to hashtables
//				//MethodKeyProfile holds method key to lookup list of associated methods
//				if (_allMethodsByLookupKeysList.Contains(methodKey.StructMethodLookupPK))
//				{	
//					MethodKeyProfile currKey = (MethodKeyProfile)_allMethodsByLookupKeysList[methodKey.StructMethodLookupPK];
//					currKey.MethodList.Add(currMethod);
//				}
//					
//				else
//				{	
//					methodKey.MethodList.Add(currMethod);
//					_allMethodsByLookupKeysList.Add(methodKey.StructMethodLookupPK, methodKey);
//				}
//
//				//8-15-03
//				_allMethodsList.Add(currMethod.Key, currMethod);
//			}
//		}
//
//		private MethodKeyProfile ConvertToMethodKeyProfile(DataRowView dr, int key)
//		{
//			structMethodAltKey lookupKey = BuildMethodKey(dr);
//			MethodKeyProfile mkp = new MethodKeyProfile(key);
//			mkp.StructMethodLookupPK = lookupKey;
//			return mkp;
//		}
//		private MethodKeyProfile ConvertToMethodKeyProfile(DataRow dr, int key)
//		{
//			structMethodAltKey lookupKey = BuildMethodKey(dr);
//			MethodKeyProfile mkp = new MethodKeyProfile(key);
//			mkp.StructMethodLookupPK = lookupKey;
//			return mkp;
//		}
//		//Structure to lookup Methods by 2 of 3 Alt keys (Method_Type_ID + User_RID)
//		private structMethodAltKey BuildMethodKey(DataRowView dr)
//		{
//			structMethodAltKey key = new structMethodAltKey();
//
//			if (dr["METHOD_TYPE_ID"] != DBNull.Value)
//				key.Method_Type_ID = (eMethodTypeUI)Convert.ToInt32(dr["METHOD_TYPE_ID"]);
//			
//			if (dr["USER_RID"] != DBNull.Value)
//				key.User_RID = Convert.ToInt32(dr["USER_RID"]);
//
//			return key;
//		}
//
//		//Structure to lookup Methods by 2 of 3 Alt keys (Method_Type_ID + User_RID)
//		private structMethodAltKey BuildMethodKey(DataRow dr)
//		{
//			structMethodAltKey key = new structMethodAltKey();
//
//			if (dr["METHOD_TYPE_ID"] != DBNull.Value)
//				key.Method_Type_ID = (eMethodTypeUI)Convert.ToInt32(dr["METHOD_TYPE_ID"]);
//			
//			if (dr["USER_RID"] != DBNull.Value)
//				key.User_RID = Convert.ToInt32(dr["USER_RID"]);
//
//			return key;
//		}
//
//		private ApplicationBaseMethod ConvertToMethodProfile(DataRowView dr)
//		{
//			if (dr["METHOD_RID"] != DBNull.Value && dr["METHOD_TYPE_ID"] != DBNull.Value)
//			{
//				//MethodProfile mp = new MethodProfile(Convert.ToInt32(dr["METHOD_RID"]), (eMethodType)Convert.ToInt32(dr["METHOD_TYPE_ID"]));
//				ApplicationBaseMethod mp = new ApplicationBaseMethod(Convert.ToInt32(dr["METHOD_RID"]));
//
//				if (dr["METHOD_NAME"] != DBNull.Value)
//					mp.Name = dr["METHOD_NAME"].ToString();
//
//				if (dr["USER_RID"] != DBNull.Value)
//					mp.User_RID = Convert.ToInt32(dr["USER_RID"]);
//
//				if (dr["METHOD_TYPE_ID"] != DBNull.Value)
//					mp.Method_Type_ID = (eMethodType)Convert.ToInt32(dr["METHOD_TYPE_ID"]);
//
//				if (dr["METHOD_DESCRIPTION"] != DBNull.Value)
//					mp.Method_Description	= (string)dr["METHOD_DESCRIPTION"];
//
//				if (dr["SG_RID"] != DBNull.Value)
//					mp.SG_RID = Convert.ToInt32(dr["SG_RID"]);
//
//				mp.Method_Change_Type = eChangeType.none;
//				mp.Filled = true;
//		
//				return mp;
//			}
//			else
//				//return new MethodProfile(-1, eMethodType.OTSPlan);
//				return new ApplicationBaseMethod(Include.NoRID);
//		}
//	
//
//		//was internal - Get MethodProfileList for WorkflowMethodExplorer
//		public DataTable GetMethodList(int UserRID, eGlobalUserType globalUserType, eMethodType methodType)
//		{
//			//ProfileList returnList = new ProfileList(eProfileType.Method);
//		
//			//Populate lookup key to get method list
//			//structMethodAltKey keyVal = new structMethodAltKey();
//
//			//keyVal.Method_Type_ID = MethodType;
//			//keyVal.User_RID = Include.EvalGlobalUserTypeUserRID(globalUserType, UserRID);
//
//			//MethodKeyProfile currKey = new MethodKeyProfile(Include.NoRID);
//			
//			//if (_allMethodsByLookupKeysList.Contains(keyVal))
//			//	currKey = (MethodKeyProfile)_allMethodsByLookupKeysList[keyVal];
//			
//			//returnList = currKey.MethodList;
//
//			//return returnList;
//			
//			//SJD 10-2-03 - disregard everything I've done - just hit the DB each time...(and call it progress :))
//
//			MethodBase methodBase = new MethodBase();
//			
//			return methodBase.GetMethods(methodType,Include.EvalGlobalUserTypeUserRID(globalUserType, UserRID),false);
//		}
//
//		public ApplicationBaseMethod GetApplicationBaseMethod(int key)
//		{
//			if (_allMethodsList.Contains(key))
//				return (ApplicationBaseMethod)_allMethodsList[key];
//			else
//			{
//				//MethodProfile methodProfile = new MethodProfile(-1, eMethodType.OTSPlan);
//				ApplicationBaseMethod applicationBaseMethod = new ApplicationBaseMethod(Include.NoRID);
//				return applicationBaseMethod;
//			}
//		}
//
//		public MIDRetail.Business.OTSPlanMethod GetApplicationBaseMethod(SessionAddressBlock sab, int key)
//		{
//			return new OTSPlanMethod(sab,key);
//		}
//
////		public Profile GetMethodTypeProfile(int methodRID)
////		{
////			MethodProfile methodProfile = null;
////
////			if (_allMethodsList.Contains(methodRID))
////				methodProfile = (MethodProfile)_allMethodsList[methodRID];
////
////			return methodProfile.MethodTypeProfile;
////
////		}
////
////		public void PopulateMethodTypeProfile(int methodRID)
////		{
////			MethodProfile methodProfile = null;
////
////			if (_allMethodsList.Contains(methodRID))
////				methodProfile = (MethodProfile)_allMethodsList[methodRID];
////
////			//Populate methodProfile.MethodTypeProfile;
////		}
//
//		//Copied from StoreGroupLevel
//		public void DeleteGroupLevel(int SGL_RID)
//		{
////			try
////			{
////				// delete from Database
////				_storeData.OpenUpdateConnection();
////				_storeData.StoreGroupLevel_Delete(SGL_RID);
////				_storeData.CommitData();
////				_storeData.CloseUpdateConnection();
////
////				// delete from data table
////				DataRow dr = _dtStoreGroupLevel.Rows.Find(SGL_RID);
////				if (dr != null)
////					_dtStoreGroupLevel.Rows.Remove(dr);
////
////				//SJD - delete from hashtable?
////			}
////			catch ( Exception err )
////			{
////				throw;
////			}
//		}
//		 
//
//		public int AddMethod(TransactionData td, Profile methodProfile)
//		{
//			try
//			{
//				if (methodProfile.GetType() == typeof(MIDRetail.Business.OTSPlanMethod))
//				{
//					MIDRetail.Business.OTSPlanMethod otsPlanMethod = (MIDRetail.Business.OTSPlanMethod)methodProfile;
//					otsPlanMethod.Update(td);
//					return otsPlanMethod.Key;
//				}
//				else
//					return Include.NoRID;
//			}
//			catch ( Exception err )
//			{
//				HandleException(err);
//				return (int)eGenericDBError.GenericDBError;
//			}
//		}
//
//		public int AddMethod(MethodProfile methodProfile)
//		{
//			MethodRelationData mrd = new MethodRelationData();
//			
//			int method_RID = -1;
//
//			try
//			{	
//				mrd.OpenUpdateConnection();
//				
//				//Populate MethodBase member variables (required to check for AK existance)
//				method_RID = mrd.InsertMethod(methodProfile.Name, methodProfile.Method_Type_ID,
//					methodProfile.User_RID, methodProfile.Method_Description, methodProfile.SG_RID,
//					Include.ConvertBoolToChar(methodProfile.Virtual_IND));
//				
//				if (method_RID != (int)eGenericDBError.DuplicateKey)
//				{
//					switch (methodProfile.Method_Type_ID)
//					{
//						case eMethodType.OTSPlan:
//							
//							//Load MethodOTSPlanProfile from MethodProfile
//							MethodOTSPlanProfile OTSMethProf = (MethodOTSPlanProfile)methodProfile.MethodTypeProfile;
//							
//							//Load MethodOTSPlanProfile ItemArray
//							OTSMethProf.Key = method_RID;
//							Object [] OTSoa = OTSMethProf.ItemArray();
//
//							if(!mrd.InsertOTSPlan(OTSoa))
//							{
//								return (int)eGenericDBError.GenericDBError;
//							}
//
//							if (methodProfile.GLFProfileList.Count > 0)
//							{
//								//Load each GroupLevelFunctionProfile from MethodProfile
//								foreach(GroupLevelFunctionProfile GLFProf in methodProfile.GLFProfileList)
//								{
//									//Load MethodOTSPlanProfile ItemArray
//									Object [] GLFoa = GLFProf.ItemArray();
//									if (!mrd.InsertGLF(GLFoa, method_RID))
//									{
//										return (int)eGenericDBError.GenericDBError;
//									}
//								}
//							}
//							break;
//
//						case eMethodType.GeneralAllocation:
//							//Load MethodGenAllocProfile from MethodProfile
//							MethodGenAllocProfile GenAllocProf = (MethodGenAllocProfile)methodProfile.MethodTypeProfile;
//							
//							//Load MethodGenAllocProfile ItemArray
//							GenAllocProf.Key = method_RID;
//							Object [] GAMoa = GenAllocProf.ItemArray();
//
//							if(!mrd.InsertGeneralAllocation(GAMoa))
//							{
//								return (int)eGenericDBError.GenericDBError;
//							}
//							break;
//
//						default:
//							return (int)eGenericDBError.GenericDBError;
//					}
//
//					mrd.CommitData();
//
//					// ...and Add to Methods Data Table
//					DataRow newDataRow = _dtAllMethods.NewRow();
//
//					//newDataRow = _methodBase.GetLoadedMethodBaseNewRow(_dtAllMethods);
//					newDataRow = GetLoadedMethodBaseNewRow(methodProfile, method_RID);
//					_dtAllMethods.Rows.Add(newDataRow);
//
//					_dtAllMethods.AcceptChanges();
//
//					//Add to hashtables
//					if (_allMethodsList.Contains(method_RID))
//						_allMethodsList.Remove(methodProfile);
//	
//					//_allMethodsList.Add(methodProfile, method_RID);
//					_allMethodsList.Add(method_RID,methodProfile);
//
//					MethodKeyProfile methodKey = ConvertToMethodKeyProfile(newDataRow, GetUniqueID());
//
//					methodProfile.Key = method_RID;
//					if (_allMethodsByLookupKeysList.Contains(methodKey.StructMethodLookupPK))
//					{	
//						MethodKeyProfile currKey = (MethodKeyProfile)_allMethodsByLookupKeysList[methodKey.StructMethodLookupPK];
//						currKey.MethodList.Add(methodProfile);
//					}
//				
//					else
//					{	
//						methodKey.MethodList.Add(methodProfile);
//						_allMethodsByLookupKeysList.Add(methodKey.StructMethodLookupPK, methodKey);
//					}
//
//					return method_RID;
//				}
//				else
//					return (int)eGenericDBError.DuplicateKey;
//			}
//			catch ( Exception err )
//			{
//				HandleException(err);
//				return (int)eGenericDBError.GenericDBError;
//			}
//			finally
//			{
//				mrd.CloseUpdateConnection();
//			}
//		}
//
//		public DataRow GetLoadedMethodBaseNewRow(MethodProfile methodProfile, int method_RID)
//		{
//			DataRow newDataRow = _dtAllMethods.NewRow();
//			
//			newDataRow["METHOD_RID"]			= method_RID;
//			newDataRow["METHOD_NAME"]			= methodProfile.Name;
//			newDataRow["METHOD_TYPE_ID"]		= (int)methodProfile.Method_Type_ID;
//			newDataRow["USER_RID"]				= methodProfile.User_RID;
//			newDataRow["METHOD_DESCRIPTION"]	= methodProfile.Method_Description;
//			newDataRow["SG_RID"]				= methodProfile.SG_RID;
//			newDataRow["VIRTUAL_IND"]			= methodProfile.Virtual_IND;
//			
//			return newDataRow;
//		}
//
//		#region Exception Handling
//		private void HandleException(Exception ex)
//		{
//			System.Windows.Forms.MessageBox.Show(ex.ToString(), "OTSPlanMethod.cs");
//			Debug.WriteLine(ex.ToString());
//		}
//		#endregion
//	}
//}
