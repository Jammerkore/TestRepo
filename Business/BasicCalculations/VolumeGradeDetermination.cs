using System;
using System.Collections;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
 	/// <summary>
	/// User defined index ranges that dynamically partition stores into grades
	/// </summary>
	/// <remarks>
	/// <para>
	/// Volume Grades are user defined index ranges that dynamically partition the stores 
	/// relative to the average store volume.  Volume may measure sales, inventory or other 
	/// relevant information by store.  
	/// </para><para>
	/// The Volume Grade definition consists of codes associated with unique low boundary 
	/// index values.   The definition requires that one code be associated with the low 
	/// boundary of zero.  Typically, the Volume Grades are presented in descending order 
	/// by low boundary index. 
	/// </para><para>
	/// A store belongs to a Volume Grade if the volume grade’s low boundary index is the 
	/// largest low boundary index less than or equal to the store’s volume index. 
	/// </para>
	/// </remarks>
	public class VolumeGradeDetermination: MIDAlgorithm
	{
		int _eligibleStores;
		double _eligibleTotal;
		double _ineligibleTotal;
		double _averageQuantity;
		ArrayList _storeList;
		ArrayList _volumeGradeList;
        
        // Begin TT#1243 - JSmith - Audit Performance
        //public VolumeGradeDetermination()
        //{
        //}
        public VolumeGradeDetermination(MIDRetail.Common.Audit aAudit) :
            base(aAudit)
        {
        }
        // End TT#1243

		#region Properties
		public ArrayList VolumeGradeList
		{
			get { return _volumeGradeList; }
			set { _volumeGradeList = value; }
		}
		public ArrayList StoreList
		{
			get { return _storeList; }
			set { _storeList = value; }
		}
		public int EligibleStores
		{
			get { return _eligibleStores; }
		}
		public double AvgQuantity
		{
			get { return _averageQuantity; }
		}
		public double EligibleTotal
		{
			get { return _eligibleTotal; }
		}
		public double IneligibleTotal
		{
			get { return _ineligibleTotal; }
		}
		#endregion

		public override void Calculate()
		{
			// first sort the volume grade list
			if (_volumeGradeList == null)
			{
				throw new Exception("Error:  VolumeGradeList is empty");
			}
			// sort the volume grades descending
			_volumeGradeList.Sort(new VGDescendingComparer());

			// Check to make sure the low boundary of the last grade is zero
			if (((VolumeGrade)_volumeGradeList[_volumeGradeList.Count-1]).LowBoundary != 0.0)
			{
				//_warnings.Add(new VGDefinitionsIncomplete());
				_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_VGDefinitionsIncomplete), this.ToString());
				return;
			}

			// next calculate the index
            // Begin TT#1243 - JSmith - Audit Performance
            //Index I = new Index();
            Index I = new Index(Warnings);
            // End TT#1243
			I.SummandList = _storeList;
			I.Calculate();

			// set some results
			_eligibleStores = I.EligibleSummands;
			_eligibleTotal = I.EligibleTotal;
			_ineligibleTotal = I.IneligibleTotal;
			_averageQuantity = I.AvgQuantity;
			_warnings = I.Warnings;

			// determine volume grade
			VolumeGrade dummyVG = new VolumeGrade();
			foreach (Store S in _storeList)
				if (S.Eligible)
				{
					// use a dummy volume group for the binary search
					dummyVG.LowBoundary = S.Result;
					int result = _volumeGradeList.BinarySearch(dummyVG, new VGDescendingComparer()); 
					VolumeGrade vg = (VolumeGrade)_volumeGradeList[Math.Max(Math.Abs(result)-1,0)];
					vg.Stores++;
					S.VolumeGrade = vg.Code;
				}
				else 
					S.VolumeGrade = VolumeGradeCode.vgNA;   // not applicable
		}

	}
	

}
