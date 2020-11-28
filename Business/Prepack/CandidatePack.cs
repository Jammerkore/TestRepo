// TT#787  - Vendor Minimum applies only to packs 
//  removed this obsolete module


//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;

//namespace MID.MRS.Business
//{
//    public class CandidatePack
//    {
//        int _packMultiple = 0;
//        /// <summary>
//        /// Gets or sets the pack multiple for the candidate pack configuration
//        /// </summary>
//        public int PackMulitple
//        {
//            get
//            {
//                return _packMultiple;
//            }
//            set
//            {
//                _packMultiple = value;
//            }
//        }

//        int _totalPacks = 0;
//        /// <summary>
//        /// Gets or sets the total number of packs that fit the candidate pack configuration
//        /// </summary>
//        public int TotalPacks
//        {
//            get
//            {
//                return _totalPacks;
//            }
//            set
//            {
//                _totalPacks = value;
//            }
//        }

//        Hashtable _sizeConfigurations = new Hashtable();
//        ///<summary>
//        ///Collection (hashtable) of SizeConfigurations included in this pack
//        ///</summary>
//        Hashtable SizeConfigurations
//        {
//            get
//            {
//                return _sizeConfigurations;
//            }
//            set
//            {
//                _sizeConfigurations = value;
//            }
//        }

//        public class SizeConfiguration
//        {
//            string _size = "";
//            ///<summary>
//            ///Gets of sets the size configuration's size name
//            ///</summary>
//            public string Size
//            {
//                get
//                {
//                    return _size;
//                }
//                set
//                {
//                    _size = value;
//                }
//            }

//            int _configuration = 0;
//            ///<summary>
//            ///Gets or sets the size configuration
//            ///</summary>
//            public int Configuration
//            {
//                get
//                {
//                    return _configuration;
//                }
//                set
//                {
//                    _configuration = value;
//                }
//            }
//        }
//    }
//}
