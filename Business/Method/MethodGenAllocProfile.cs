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
	/// Summary description for MethodGenAllocProfile.
	/// </summary>
	[Serializable()] 
	public class MethodGenAllocProfile : Profile, IComparable , ICloneable
	{
		private int _Begin_CDR_RID;
		private int _Ship_To_CDR_RID;
		private int _Merch_HN_RID;
		private int _Merch_PH_RID;
		private int _Merch_PHL_Sequence;
		private int _Gen_Alloc_HDR_RID;
		private double _Reserve;
		private bool _Percent_Ind;
		
		private eChangeType	_GenAllocMethod_Change_Type;
		private bool _filled;

		public int Begin_CDR_RID
		{
			get	{return _Begin_CDR_RID;}
			set	{_Begin_CDR_RID = value;}
		}

		public int Ship_To_CDR_RID
		{
			get	{return _Ship_To_CDR_RID;}
			set	{_Ship_To_CDR_RID = value;}
		}

		public int Merch_HN_RID 
		{
			get	{return _Merch_HN_RID;}
			set	{_Merch_HN_RID = value;}
		}

		public int Merch_PH_RID
		{
			get	{return _Merch_PH_RID;}
			set	{_Merch_PH_RID = value;	}
		}

		public int Merch_PHL_Sequence
		{
			get	{return _Merch_PHL_Sequence;}
			set	{_Merch_PHL_Sequence = value;}
		}

		public int Gen_Alloc_HDR_RID
		{
			get	{return _Gen_Alloc_HDR_RID;}
			set	{_Gen_Alloc_HDR_RID = value;}
		}

		public double Reserve
		{
			get	{return _Reserve;}
			set	{_Reserve = value;	}
		}

		public bool Percent_Ind
		{
			get	{return _Percent_Ind;}
			set	{_Percent_Ind = value;	}
		}

		public eChangeType GenAllocMethod_Change_Type 
		{
			get { return _GenAllocMethod_Change_Type ; }
			set { _GenAllocMethod_Change_Type = value; }
		}

		public bool Filled 
		{
			get { return _filled ; }
			set { _filled = value; }
		}

		public override int GetHashCode()
		{
			return this.Key;
		}

		/// <summary>
		/// overrided Equals
		/// </summary>
		/// <param name="obj">MethodGenAllocProfile</param>
		/// <returns>Bool</returns>
		public override bool Equals(Object obj) 
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) 
				return false;

			return (this.Key == ((MethodGenAllocProfile)obj).Key);
		} 

		//public int IComparable.CompareTo(object obj)
		public int CompareTo(object obj)
		{ 
			return Key - ((MethodGenAllocProfile)obj).Key; 
		} 
 
		public static bool operator<(MethodGenAllocProfile lhs, MethodGenAllocProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) < 0;
		}

		public static bool operator<=(MethodGenAllocProfile lhs, MethodGenAllocProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) <= 0;
		}

		public static bool operator>(MethodGenAllocProfile lhs, MethodGenAllocProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) > 0;
		}

		public static bool operator>=(MethodGenAllocProfile lhs, MethodGenAllocProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) >= 0;
		}

		public object Clone()
		{
			return this;
		}
  
		public MethodGenAllocProfile(int aKey)
			: base(aKey)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodGeneralAllocation;
			}
		}

		/// <summary>
		/// Unloads MethodOTSPlanProfile in to field by field object array.
		/// </summary>
		/// <returns>Object array</returns>
		public object [] ItemArray()
		{
			object [] ar = new object[9];
			ar[0] = this.Key;
			ar[1] = this.Begin_CDR_RID;
			ar[2] = this.Ship_To_CDR_RID;
			ar[3] = this.Merch_HN_RID;
			ar[4] = this.Merch_PH_RID;
			ar[5] = this.Merch_PHL_Sequence;
			ar[6] = this.Gen_Alloc_HDR_RID;
			ar[7] = this.Reserve;
            ar[8] = Include.ConvertBoolToChar(this.Percent_Ind);	
		
			return ar;
		}
	}
}
