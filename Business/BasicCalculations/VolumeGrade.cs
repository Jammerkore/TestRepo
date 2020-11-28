using System;
using System.Collections;

namespace MIDRetail.Business
{

	/// <summary>
	/// This class houses the Volume Grade data
	/// for the <see cref="VolumeGradeDetermination"/> calculation
	/// </summary>
	public enum VolumeGradeCode
	{
		vgNA,vg1,vgS,vgA,vgB,vgC,vgD,vgE,vgF,vgG
	}

	public class VolumeGrade
	{
		VolumeGradeCode _code;
		double _lowBoundary;
		int _stores;

		public VolumeGrade(VolumeGradeCode code, double lowBoundary)
		{
			_code = code;
			_lowBoundary = lowBoundary;
			_stores = 0;
		}
		public VolumeGrade()
		{
			_stores = 0;
		}

		#region Properties
		public VolumeGradeCode Code
		{
			get { return _code; }
			set { _code = value; }
		}
		public double LowBoundary
		{
			get { return _lowBoundary; }
			set { _lowBoundary = value; }
		}
		public int Stores
		{
			get { return _stores; }
			set { _stores = value; }
		}
		#endregion
	
		// CompareTo method for sorting
		public int CompareTo(VolumeGrade x)    
		{      
			return (this._lowBoundary.CompareTo(x._lowBoundary));
		}
	}

	/// <summary>
	/// Store Compare Classes for ascending and descending
	/// sorts and binary searches    
	/// </summary>
	public class VGAscendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is VolumeGrade) && !(y is VolumeGrade))        
			{          
				throw new ArgumentException("only allows VolumeGrade objects");        
			}        
			return ((VolumeGrade)x).CompareTo((VolumeGrade)y);      
		}    
	}
	public class VGDescendingComparer:IComparer    
	{      
		public int Compare(object x, object y)      
		{        
			if (!(x is VolumeGrade) && !(y is VolumeGrade))        
			{          
				throw new ArgumentException("only allows VolumeGrade objects");        
			}        
			return (-((VolumeGrade)x).CompareTo((VolumeGrade)y));      
		}    
	}
	
}
