using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Reflection.Emit;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for WorkflowMethodManager.
	/// </summary>
	public class WorkflowMethodManager
	{
		private WorkflowMethodData _WMData = null;
		private static int uniqueID = -1;

		public static int GetUniqueID()
		{
			return uniqueID++;
		}

		//TODO - Delete this
//		public WorkflowMethodRelations WMRelations
//		{
//			get	{return _WMRelations;}
//		}


//		public int UserRID
//		{
//			get { return _userRID ; }
//		}
		
		public WorkflowMethodManager(int userRID)
		{
			
			_WMData = new WorkflowMethodData();
		}

		#region WorkflowMethodExplorer Static Nodes

		public int GetUniqueWMExpID()
		{
			return GetUniqueID();
		}

        //Begin TT#1383-MD jsobek -Remove unused table WM_EXP_NODES_STRUCT
        //public ProfileList GetWorkflowMethodStaticList()
        //{
        //    //ArrayList alWMEStaticList = PopulateWorkflowMethodStaticNodeList();

        //    ProfileList pl = new ProfileList(eProfileType.WorkflowMethodExpStaticNode, PopulateWorkflowMethodStaticNodeList());
        //    return pl;
        //}

        //private ArrayList PopulateWorkflowMethodStaticNodeList()
        //{
        //    try
        //    {
        //        ArrayList alTemp = new ArrayList();
        //        WorkflowMethodData WMEData = new WorkflowMethodData();
        //        DataTable dtWMEStaticNodes = WMEData.GetWM_Exp_Static_Nodes();
				
        //        WorkflowMethodExpStaticNodeProfile wmExpStaticNode = null;
				
        //        foreach(DataRow drStaticNode in dtWMEStaticNodes.Rows)
        //        {
        //            structWMExpStaticNode wmStaticNode = new structWMExpStaticNode();
        //            wmStaticNode.WM_Static_Node_RID = Convert.ToInt32(drStaticNode["WM_Static_Node_RID"], CultureInfo.CurrentUICulture);
        //            wmStaticNode.Text_Value = drStaticNode["Text_Value"].ToString();
        //            wmStaticNode.Node_Enum_ID = Convert.ToInt32(drStaticNode["Node_Enum_ID"], CultureInfo.CurrentUICulture);
        //            wmStaticNode.Level_1 = Convert.ToInt32(drStaticNode["Level_1"], CultureInfo.CurrentUICulture);
        //            wmStaticNode.Level_2 = Convert.ToInt32(drStaticNode["Level_2"], CultureInfo.CurrentUICulture);
        //            wmStaticNode.Level_3 = Convert.ToInt32(drStaticNode["Level_3"], CultureInfo.CurrentUICulture);
        //            wmStaticNode.Level_4 = Convert.ToInt32(drStaticNode["Level_4"], CultureInfo.CurrentUICulture);

        //            wmExpStaticNode = NewBaseNode(wmStaticNode);
        //            alTemp.Add(wmExpStaticNode);
        //        }
        //        return alTemp;
        //    }
        //    catch ( Exception err )
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
        //End TT#1383-MD jsobek -Remove unused table WM_EXP_NODES_STRUCT

		//Populate the Profile based on the Structure contents
		private WorkflowMethodExpStaticNodeProfile NewBaseNode(structWMExpStaticNode wmExpStaticNode)
		{
			WorkflowMethodExpStaticNodeProfile BaseNode = new WorkflowMethodExpStaticNodeProfile(wmExpStaticNode.WM_Static_Node_RID);
			BaseNode.TextValue = wmExpStaticNode.Text_Value;
			BaseNode.NodeEnumID = wmExpStaticNode.Node_Enum_ID;
			BaseNode.Level_1 = wmExpStaticNode.Level_1;
			BaseNode.Level_2 = wmExpStaticNode.Level_2;
			BaseNode.Level_3 = wmExpStaticNode.Level_3;
			BaseNode.Level_4 = wmExpStaticNode.Level_4;

			return BaseNode;
		}

		#endregion

		public int MethodHasRows(eMethodTypeUI aMethodType, int aUserRID)
		{
			try
			{
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//return _WMData.Method_Row_Count(aMethodType, aUserRID) +
				//    _WMData.Method_Favorite_Row_Count(aMethodType, aUserRID);
				return _WMData.Method_Row_Count(aMethodType, aUserRID);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
			catch
			{
				throw;
			}
		}

		public int WorkflowHasRows(eWorkflowType aWorkflowType, int aUserRID)
		{
			try
			{
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//return _WMData.Workflow_Row_Count(aWorkflowType, aUserRID) +
				//    _WMData.Workflow_Favorite_Row_Count(aWorkflowType, aUserRID);
				return _WMData.Workflow_Row_Count(aWorkflowType, aUserRID);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
			catch
			{
				throw;
			}
		}

		public bool CheckForDuplicateMethodID(int aUserRID, ApplicationBaseMethod aABM)
		{
			try
			{
				int userRID;
				if (aABM.GlobalUserType == eGlobalUserType.Global)
				{
					userRID = Include.GlobalUserRID; // Issue 3806
				}
				else
				{
					userRID = aUserRID;
				}

				if (_WMData.Method_Row_Count(aABM.MethodType, aABM.Name, userRID) > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		public bool CheckForDuplicateWorkflowID(int aUserRID, ApplicationBaseWorkFlow aABW)
		{
			try
			{
				int userRID;
				if (aABW.GlobalUserType == eGlobalUserType.Global)
				{
					userRID = Include.GlobalUserRID;  // Issue 3806
				}
				else
				{
					userRID = aUserRID;
				}

				if (_WMData.Workflow_Row_Count(aABW.WorkFlowType, aABW.WorkFlowName, userRID) > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

        // BEGIN MID Track #6336 - KJohnson - Header Load API Enhancement
        public DataTable GetMethodList(string aMethodName, eMethodType aMethodType, int aUserRID)
        {
            try
            {
                return _WMData.GetMethodList(aMethodName, aMethodType, aUserRID);
            }
            catch
            {
                throw;
            }
        }
        // END MID Track #6336

		public DataTable GetMethodList(eMethodTypeUI aMethodType)
		{
			try
			{
				return _WMData.GetMethodList(aMethodType);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetMethodList(eMethodTypeUI aMethodType, int aUserRID)
		{
			try
			{
				return _WMData.GetMethodList(aMethodType, aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetMethodList()
		{
			try
			{
				return _WMData.GetMethodList();
			}
			catch
			{
				throw;
			}
		}

	}

#region WorkflowMethodExpStaticNodeProfile

	////////////////////////////////
	/// <summary>
	/// Used to hold the Static Node information.
	/// </summary>
	//8-15-03 sjd keep
	[Serializable()]
	public class WorkflowMethodExpStaticNodeProfile : Profile
	{
		public string _textValue;
		public int _nodeEnumID;
		public int _level_1;
		public int _level_2;
		public int _level_3;
		public int _level_4;

		public string TextValue 
		{
			get { return _textValue ; }
			set { _textValue = value; }
		}
		public int NodeEnumID 
		{
			get { return _nodeEnumID ; }
			set { _nodeEnumID = value; }
		}
		public int Level_1 
		{
			get { return _level_1 ; }
			set { _level_1 = value; }
		}
		public int Level_2
		{
			get { return _level_2 ; }
			set { _level_2 = value; }
		}
		public int Level_3
		{
			get { return _level_3 ; }
			set { _level_3 = value; }
		}
		public int Level_4 
		{
			get { return _level_4 ; }
			set { _level_4 = value; }
		}
		
		public override bool Equals(object obj)
		{
			return (Key == ((WorkflowMethodExpStaticNodeProfile)obj).Key); 
		}

		public override int GetHashCode()
		{
			return Key;
		}
		
		public WorkflowMethodExpStaticNodeProfile(int aKey)
			: base(aKey)
		{

		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.WorkflowMethodExpStaticNode;
			}
		}

	}
}

#endregion


