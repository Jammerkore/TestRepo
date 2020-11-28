// TT#787  - Vendor Minimum applies only to packs 
//  removed this obsolete module

//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MID.MRS.Business
//{

//    public class PackConstraints
//    {
//        public enum reserveType
//        {
//            percent,
//            units
//        }

//        public enum evaluationType
//        {
//            single,
//            multiple
//        }

//        int _reserve = 0;
//        /// <summary>
//        /// Gets or sets the reserve amount
//        /// </summary>
//        public int Reserve
//        {
//            get
//            {
//                return _reserve;
//            }
//            set
//            {
//                _reserve = value;
//            }
//        }

//        reserveType _reserveType = reserveType.percent;
//        /// <summary>
//        /// Gets or set the reserve type (units or percent)
//        /// </summary>
//        public reserveType ReserveType
//        {
//            get
//            {
//                return _reserveType;
//            }
//            set
//            {
//                _reserveType = value;
//            }
//        }

//        int _reserveAsBulk = 0;
//        /// <summary>
//        /// Gets or sets the reserve as bulk amount
//        /// </summary>
//        public int ReserveAsBulk
//        {
//            get
//            {
//                return _reserveAsBulk;
//            }
//            set
//            {
//                _reserveAsBulk = value;
//            }
//        }

//        reserveType _reserveAsBulkType = reserveType.percent;
//        /// <summary>
//        /// Gets or sets the reserve as bulk type (units or percent)
//        /// </summary>
//        public reserveType ReserveAsBulkType
//        {
//            get
//            {
//                return _reserveAsBulkType;
//            }
//            set
//            {
//                _reserveAsBulkType = value;
//            }
//        }

//        int _reserveAsPacks = 0;
//        /// <summary>
//        /// Gets or sets the reserve as packs amount
//        /// </summary>
//        public int ReserveAsPacks
//        {
//            get
//            {
//                return _reserveAsPacks;
//            }
//            set
//            {
//                _reserveAsPacks = value;
//            }
//        }

//        reserveType _reserveAsPacksType = reserveType.percent;
//        /// <summary>
//        /// Gets or sets the reserve as packs type (units or percent)
//        /// </summary>
//        public reserveType ReserveAsPacksType
//        {
//            get
//            {
//                return _reserveAsPacksType;
//            }
//            set
//            {
//                _reserveAsPacksType = value;
//            }
//        }

//        int _vendorComponentMin = 0;
//        /// <summary>
//        /// Gets or sets the vendor component minimum amount
//        /// </summary>
//        public int VendorComponentMinimum
//        {
//            get
//            {
//                return _vendorComponentMin;
//            }
//            set
//            {
//                _vendorComponentMin = value;
//            }
//        }

//        int _bulkSizeMin = 0;
//        /// <summary>
//        /// Gets or sets the bulk size minimum amout
//        /// </summary>
//        public int BulkSizeMinimum
//        {
//            get
//            {
//                return _bulkSizeMin;
//            }
//            set
//            {
//                _bulkSizeMin = value;
//            }
//        }

//        int _avgPackDeviationsTolerance = 0;
//        /// <summary>
//        /// Gets or sets the average pack deviation tolerance amount
//        /// </summary>
//        public int AveragePackDeviationTolerance
//        {
//            get
//            {
//                return _avgPackDeviationsTolerance;
//            }
//            set
//            {
//                _avgPackDeviationsTolerance = value;
//            }
//        }

//        int _maxPackAllocNeedTolerance = 0;
//        /// <summary>
//        /// Gets or sets the maximum pack allocations need tolerance amount
//        /// </summary>
//        public int MaximumPackAllocationNeedTolerance
//        {
//            get
//            {
//                return _maxPackAllocNeedTolerance;
//            }
//            set
//            {
//                _maxPackAllocNeedTolerance = value;
//            }
//        }

//        evaluationType _evaluateBy = evaluationType.single;
//        /// <summary>
//        /// Gets or sets the evaluated by options
//        /// </summary>
//        public evaluationType EvaluateBy
//        {
//            get
//            {
//                return _evaluateBy;
//            }
//            set
//            {
//                _evaluateBy = value;
//            }
//        }

//    }
//}
