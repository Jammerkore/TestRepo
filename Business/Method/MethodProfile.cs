//using System;
//using System.IO;
//using System.Collections;
//using System.Data;
//
//using MIDRetail.Data;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//using System.Reflection.Emit;
//
//namespace MIDRetail.Business
//{
//	/// <summary>
//	/// Used to hold the information for a single Method.  To create a new method,
//	/// set instantiate using Include.NoRID and set the Method_Change_Type to
//	/// eChangeType.add.  To populate the method, instantiate using the key, fill
//	/// in the base method values (we could have it populate itself...later), set 
//	/// Method_Change_Type to eChageType.none then set Filled to true.  Setting 
//	/// Filled to true will trigger a method that will then populate the 
//	/// MethodTypeProfile and GroupLevelFunctionProfileList...
//	/// </summary>\
//	[Serializable()]
//	public class MethodProfile : Profile, IComparable , ICloneable
//	{			
//		private string _method_Name;
//		private eMethodType _method_Type_ID;
//		private int _user_RID;
//		private string _method_Description;
//		private int _sg_RID;
//		private bool _virtual_IND;
//		private eChangeType		_method_Change_Type;
//		private Profile _methodTypeProfile;
//		private ProfileList _GLFProfileList;
//		private bool _filled;
//
//		public bool Filled 
//		{
//			get { return _filled ; }
//			set { _filled = value; 
//				PopulateMethod();}
//		}
//		public string Name 
//		{
//			get { return _method_Name ; }
//			set { _method_Name = value; }
//		}
//		public eMethodType Method_Type_ID 
//		{
//			get { return _method_Type_ID ; }
//			set { _method_Type_ID = value;}
//		}
//		public int User_RID 
//		{
//			get { return _user_RID ; }
//			set { _user_RID = value; }
//		}
//		public eGlobalUserType GlobalUserType
//		{
//			get {if (User_RID == Include.GeteGlobalUserTypeUserRID())
//					 return eGlobalUserType.Global;
//				else
//					return eGlobalUserType.User; }
//		}
//		public string Method_Description 
//		{
//			get { return _method_Description ; }
//			set { _method_Description = value; }
//		}
//		public int SG_RID 
//		{
//			get { return _sg_RID ; }
//			set { _sg_RID = value; }
//		}
//		public bool Virtual_IND 
//		{
//			get { return _virtual_IND ; }
//			set { _virtual_IND = value; }
//		}
//		public Profile MethodTypeProfile 
//		{
//			get { return _methodTypeProfile ; }
//			set { _methodTypeProfile = value; }
//		}
//		public ProfileList GLFProfileList 
//		{
//			get { return _GLFProfileList ; }
//			set { _GLFProfileList = value; }
//		}
//		/// <summary>
//		/// Gets or sets the type of change for the Method.
//		/// </summary>
//		public eChangeType Method_Change_Type 
//		{
//			get { return _method_Change_Type ; }
//			set { _method_Change_Type = value; }
//		}
//
//		public override int GetHashCode()
//		{
//			return this.Key;
//		}
//
//	/// <summary>
//	/// overrided Equals
//	/// </summary>
//	/// <param name="obj">MethodProfile</param>
//	/// <returns>Bool</returns>
//	public override bool Equals(Object obj) 
//	{
//		// Check for null values and compare run-time types.
//		if (obj == null || GetType() != obj.GetType()) 
//			return false;
//
//		return (this.Key == ((MethodProfile)obj).Key);
//	}
//
//	public int CompareTo(object obj)
//	{ 
//		return Key - ((MethodProfile)obj).Key; 
//	} 
// 
//	public static bool operator<(MethodProfile lhs, MethodProfile rhs)
//	{ 
//		return ((IComparable)lhs).CompareTo(rhs) < 0;
//	}
//
//	public static bool operator<=(MethodProfile lhs, MethodProfile rhs)
//	{ 
//		return ((IComparable)lhs).CompareTo(rhs) <= 0;
//	}
//
//	public static bool operator>(MethodProfile lhs, MethodProfile rhs)
//	{ 
//		return ((IComparable)lhs).CompareTo(rhs) > 0;
//	}
//
//	public static bool operator>=(MethodProfile lhs, MethodProfile rhs)
//	{ 
//		return ((IComparable)lhs).CompareTo(rhs) >= 0;
//	}
//
//	public object Clone()
//	{
//		return this;
//	}  
//
//	private void PopulateMethod()
//	{
//		if (_filled)
//		{
//			if (this.Key != Include.NoRID || Method_Change_Type != eChangeType.add)
//			{
//				SetMethodTypeProfile();
//				SetGroupLevelFunctionProfileList();
//			}
//		}
//	}
//
//	private void SetGroupLevelFunctionProfileList()
//	{
//		GroupLevelFunction glf = new GroupLevelFunction();
//		DataTable dt = glf.GetAllGroupLevelFunctions(this.Key);
//		foreach(DataRow dr in dt.Rows)
//		{
//			GroupLevelFunctionProfile GLFP = new GroupLevelFunctionProfile(Convert.ToInt32(dr["SGL_RID"]));
//			GLFP.Default_IND = Include.ConvertCharToBool(Convert.ToChar(dr["DEFAULT_IND"]));
//			GLFP.Plan_IND = Include.ConvertCharToBool(Convert.ToChar(dr["PLAN_IND"]));
//			GLFP.Use_Default_IND = Include.ConvertCharToBool(Convert.ToChar(dr["USE_DEFAULT_IND"]));
//			GLFP.Clear_IND = Include.ConvertCharToBool(Convert.ToChar(dr["CLEAR_IND"]));
//			GLFP.Season_IND = Include.ConvertCharToBool(Convert.ToChar(dr["SEASON_IND"]));
//			GLFP.Season_HN_RID = Convert.ToInt32(dr["SEASON_HN_RID"]);
//			GLFP.GLFT_ID = (eGroupLevelFunctionType)Convert.ToInt32(dr["GLFT_ID"]);
//			GLFP.GLSB_ID = (eGroupLevelSmoothBy)Convert.ToInt32(dr["GLSB_ID"]);
//			GLFP.GLF_Change_Type = eChangeType.none;
//			GLFP.Filled = true;
//
//            _GLFProfileList.Add(GLFP);
//		}
//	}
//
//	private void SetMethodTypeProfile()
//	{
//		if (Enum.IsDefined(typeof(eMethodType), Method_Type_ID))
//		{
//			MethodTypeProfileInfo mtpInfo = new MethodTypeProfileInfo(Method_Type_ID,this.Key);
//			_methodTypeProfile = mtpInfo.GetProfile();
//		}
//	}
//
//		/// <summary>
//		/// Constructor
//		/// </summary>
//		/// <param name="aKey">int</param>
//		public MethodProfile(int aKey)
//			: base(aKey)
//		{
//			_GLFProfileList = new ProfileList(eProfileType.GroupLevelFunction);
//		}
//
//
//	/// <summary>
//	/// Returns the eProfileType of this profile.
//	/// </summary>
//	override public eProfileType ProfileType
//	{
//		get
//		{
//			return eProfileType.Method;
//		}
//	}
//
//	/// <summary>
//	/// Unloads MethodProfile in to field by field object array.
//	/// </summary>
//	/// <returns>Object array</returns>
//	public object [] ItemArray()
//	{
//		object [] ar = new object[10];
//		ar[0] = this.Key;
//		ar[1] = this.Name;
//		ar[2] = this.Method_Type_ID;
//		ar[3] = this.User_RID;
//		ar[4] = this.Method_Description;
//		ar[5] = this.SG_RID;
//		ar[6] = Include.ConvertBoolToChar(this.Virtual_IND);
//		ar[7] = this.GlobalUserType;
//		ar[8] = this.MethodTypeProfile;
//		ar[9] = this.GLFProfileList;
//		
//		return ar;
//	}
//}
// 
//	public class MethodTypeProfileInfo
//	{
//		private eMethodType _methodType;
//		private int _key = Include.NoRID;
//
//		public MethodTypeProfileInfo(eMethodType methodType, int key)
//		{
//			_methodType = methodType;
//			_key = key;
//		}
//
//		public Profile GetProfile()
//		{
//			Profile methodTypeProfile = null;
//			switch (_methodType)
//			{
//				case eMethodType.OTSPlan:
//					methodTypeProfile = ConvertToOTSPlanProfile();
//					return methodTypeProfile;
//				case eMethodType.GeneralAllocation:
//					methodTypeProfile = ConvertToGenAllocProfile();
//					return methodTypeProfile;
//				default:
//					return methodTypeProfile;
//			}
//		}
//
//		internal MethodOTSPlanProfile ConvertToOTSPlanProfile()
//		{
//			MethodOTSPlanProfile mp = new MethodOTSPlanProfile(_key);
//
//			MethodOTSPlan OTSPlan = new MethodOTSPlan();
//
//			if (!OTSPlan.PopulateOTSPlan(_key))
//				return mp;
//			
//			mp.Bal_Sales_Ind = Include.ConvertCharToBool(OTSPlan.Bal_Sales_Ind);
//			mp.Bal_Stock_Ind = Include.ConvertCharToBool(OTSPlan.Bal_Stock_Ind);
//			mp.CDR_RID = OTSPlan.CDR_RID;
//			mp.Chain_FV_RID = OTSPlan.Chain_FV_RID;
//			mp.Plan_FV_RID = OTSPlan.Plan_FV_RID;
//			mp.Plan_HN_RID = OTSPlan.Plan_HN_RID;
//			mp.OTSPlan_Method_Change_Type = eChangeType.none;			
//			mp.Filled = true;
//
//			return mp;
//		}
//
//		internal MethodGenAllocProfile ConvertToGenAllocProfile()
//		{
//			MethodGenAllocProfile mp = new MethodGenAllocProfile(_key);
//
//			MethodGeneralAllocation GenAlloc = new MethodGeneralAllocation(_key);
//
//			if (!GenAlloc.PopulateGeneralAllocation(_key))
//				return mp;
//		 
//			mp.Begin_CDR_RID = GenAlloc.Begin_CDR_RID;
//			mp.Ship_To_CDR_RID = GenAlloc.Ship_To_CDR_RID;
//			mp.Merch_HN_RID = GenAlloc.Merch_HN_RID;
//			mp.Merch_PH_RID = GenAlloc.Merch_PH_RID;
//			mp.Merch_PHL_Sequence = GenAlloc.Merch_PHL_SEQ;
//			mp.Gen_Alloc_HDR_RID = 	GenAlloc.Gen_Alloc_HDR_RID;
//			mp.Reserve = GenAlloc.Reserve;
//			mp.Percent_Ind = Include.ConvertCharToBool(GenAlloc.Percent_Ind);
//			mp.GenAllocMethod_Change_Type = eChangeType.none;			
//			mp.Filled = true;
//
//			return mp;
//		}
//	}
//
//	/// <summary>
//	/// Used to hold the information for a single MethodKey to lookup a MethodList for the 
//	/// Workflow/Method Explorer.
//	/// </summary>\
//	[Serializable()]
//	public class MethodKeyProfile : Profile, IComparable , ICloneable
//	{
//		//Key for profile lookup
//		private structMethodAltKey _structMethodLookupKey;
//		private ProfileList _methodList;
//		//private static int uniqueID = 0;
//	
//		//private ArrayList _workFlows;		//  ArrayList of Workflows in Join
//		bool _filled;
//
//		public structMethodAltKey StructMethodLookupPK 
//		{
//			get { return _structMethodLookupKey ; }
//			set { _structMethodLookupKey = value; }
//		}
//		public ProfileList MethodList 
//		{
//			get { return _methodList ; }
//			set { _methodList = value; }
//		}
//
//		public bool Filled 
//		{
//			get { return _filled ; }
//			set { _filled = value; }
//		}
// 
//		public override int GetHashCode()
//		{
//			return this.Key;
//		}
//
//		//		public static int GetUniqueID()
//		//		{
//		//				return uniqueID++; // returns zero at start
//		//		}
//
//		/// <summary>
//		/// overrided Equals
//		/// </summary>
//		/// <param name="obj">MethodKeyProfile</param>
//		/// <returns>Bool</returns>
//		public override bool Equals(Object obj) 
//		{
//			// Check for null values and compare run-time types.
//			if (obj == null || GetType() != obj.GetType()) 
//				return false;
//
//			return (this.Key == ((MethodKeyProfile)obj).Key);
//		}
//
//		//public int IComparable.CompareTo(object obj)
//		public int CompareTo(object obj)
//		{ 
//			return Key - ((MethodKeyProfile)obj).Key; 
//		} 
// 
//		public static bool operator<(MethodKeyProfile lhs, MethodKeyProfile rhs)
//		{ 
//			return ((IComparable)lhs).CompareTo(rhs) < 0;
//		}
//
//		public static bool operator<=(MethodKeyProfile lhs, MethodKeyProfile rhs)
//		{ 
//			return ((IComparable)lhs).CompareTo(rhs) <= 0;
//		}
//
//		public static bool operator>(MethodKeyProfile lhs, MethodKeyProfile rhs)
//		{ 
//			return ((IComparable)lhs).CompareTo(rhs) > 0;
//		}
//
//		public static bool operator>=(MethodKeyProfile lhs, MethodKeyProfile rhs)
//		{ 
//			return ((IComparable)lhs).CompareTo(rhs) >= 0;
//		}
//
//		public object Clone()
//		{
//			return this;
//		}  
//
//
//		/// <summary>
//		/// Constructor
//		/// </summary>
//		/// <param name="aKey">int</param>
//		public MethodKeyProfile(int aKey)
//			: base(aKey)
//		{
//			_methodList = new ProfileList(eProfileType.Method);
//			_structMethodLookupKey = new structMethodAltKey();
//		}
//
//		/// <summary>
//		/// Returns the eProfileType of this profile.
//		/// </summary>
//		override public eProfileType ProfileType
//		{
//			get
//			{
//				return eProfileType.MethodKey;
//			}
//		}
//
//		/// <summary>
//		/// Unloads MethodKey in to field by field object array.
//		/// </summary>
//		/// <returns>Object array</returns>
//		public object [] ItemArray()
//		{
//			object [] ar = new object[3];
//			ar[0] = this.Key;
//			ar[1] = this.StructMethodLookupPK;
//			ar[2] = this.MethodList;
//
//			return ar;
//		}
//	}
//}