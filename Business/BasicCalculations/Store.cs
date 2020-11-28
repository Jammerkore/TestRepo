using System;

namespace MIDRetail.Business
{
	
	/// <summary>
	/// This class houses the observation data for the 
	/// <see cref="VolumeGradeDetermination"/>
	/// calculation
	/// <seealso cref="Summand"/>
	/// </summary>
	public class Store : Summand
	{
		VolumeGradeCode _vg; 

		// constructor -- set up defaults
		public Store()
		{
		}
		
		public VolumeGradeCode VolumeGrade
		{
			get { return _vg; }
			set { _vg = value; }
		}
		
	}
	
}
