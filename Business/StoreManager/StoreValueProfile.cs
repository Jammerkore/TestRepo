//using System;
//using System.Collections;
//using System.Data;

//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//using MIDRetail.Data;

//namespace MIDRetail.Business
//{
//    /// <summary>
//    /// Holds a stores values by store_rid.
//    /// </summary>
//    public class StoreValueProfile : Profile
//    {

//        int _set;
//        string _grade;
//        double _value;
//        bool _locked;
//        bool _eligible;

//        // constructor -- set up defaults
//        public StoreValueProfile(int aKey)
//            : base(aKey)
//        {
		
//        }

//        #region Properties
//        public int Set
//        {
//            get { return _set; }
//            set { _set = value; }
//        }
//        public string Grade
//        {
//            get { return _grade; }
//            set { _grade = value; }
//        }
//        public double Value
//        {
//            get { return _value; }
//            set { _value = value; }
//        }
//        public bool Locked
//        {
//            get { return _locked; }
//            set { _locked = value; }
//        }
//        public bool Eligible
//        {
//            get { return _eligible; }
//            set { _eligible = value; }
//        }
//        #endregion


//        /// <summary>
//        /// Returns the eProfileType of this profile.
//        /// </summary>
//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.StoreValue;
//            }
//        }

//    }
	
//}
