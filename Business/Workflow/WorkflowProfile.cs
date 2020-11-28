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
	/// Used to hold the information for a single WorkflowProfile.
	/// </summary>\
	[Serializable()]
	public class WorkflowProfile : Profile, IComparable , ICloneable
	{
		//Key for profile lookup
		private eMethodTypeUI _methodTypeID;
		
//		private int _methodID;
		private string _name;
		private int _userRID;
		private eGlobalUserType _globalUserType;
		
		private string _methodDescription;
		private int _sgRID;

		//private ArrayList _workFlows;		//  ArrayList of Workflows in Join
		bool _filled;

		//		public int MethodID 
		//		{
		//			get { return _methodID ; }
		//			set { _methodID = value; }
		//		}

		public string Name 
		{
			get { return _name ; }
			set { _name = value; }
		}
		public int UserRID 
		{
			get { return _userRID ; }
			set { _userRID = value; }
		}
		public eGlobalUserType GlobalUserType
		{
			get { return _globalUserType ; }
			set { _globalUserType = value; }
		}
		public eMethodTypeUI MethodTypeID 
		{
			get { return _methodTypeID ; }
			set { _methodTypeID = value; }
		}
		public string MethodDescription 
		{
			get { return _methodDescription ; }
			set { _methodDescription = value; }
		}
		public int SGRID 
		{
			get { return _sgRID ; }
			set { _sgRID = value; }
		}

		//		public ArrayList WorkFlows 
		//		{
		//			get { return _workFlows ; }
		//			set { _workFlows = value; }
		//		}

		//		public ProfileList WorkflowJoin 
		//		{
		//			get { return _workflowJoin ; }
		//			set { _workflowJoin = value; }
		//		}
		public bool Filled 
		{
			get { return _filled ; }
			set { _filled = value; }
		}
		
		//		public override bool Equals(object obj)
		//		{
		//			return (Key == ((MethodProfile)obj).Key); 
		//		}

		public override int GetHashCode()
		{
			return this.Key;
		}

		/// <summary>
		/// overrided Equals
		/// </summary>
		/// <param name="obj">MethodProfile</param>
		/// <returns>Bool</returns>
		public override bool Equals(Object obj) 
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) 
				return false;

			return (this.Key == ((WorkflowProfile)obj).Key);
		}

		//public int IComparable.CompareTo(object obj)
		public int CompareTo(object obj)
		{ 
			return Key - ((WorkflowProfile)obj).Key; 
		} 
 
		public static bool operator<(WorkflowProfile lhs, WorkflowProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) < 0;
		}

		public static bool operator<=(WorkflowProfile lhs, WorkflowProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) <= 0;
		}

		public static bool operator>(WorkflowProfile lhs, WorkflowProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) > 0;
		}

		public static bool operator>=(WorkflowProfile lhs, WorkflowProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) >= 0;
		}

		public object Clone()
		{
			return this;
		}  


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aKey">int</param>
		public WorkflowProfile(int aKey)
			: base(aKey)
	{
		//_workFlows = new ArrayList();
	}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Workflow;
			}
		}

		/// <summary>
		/// Unloads WorkflowProfile in to field by field object array.
		/// </summary>
		/// <returns>Object array</returns>
		public object [] ItemArray()
		{
			object [] ar = new object[7];
			ar[0] = this.Key;
			ar[1] = this.Name;
			ar[2] = this.UserRID;
			ar[3] = this.GlobalUserType;
			ar[4] = this.MethodTypeID;
			ar[5] = this.MethodDescription;
			ar[6] = this.SGRID;

			return ar;
		}
	}
}
