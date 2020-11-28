// TT#787  - Vendor Minimum applies only to packs 
//  removed this obsolete module

//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MID.MRS.Business
//{
//    public class PrepackOption
//    {
//        int _qtyPerPack = 0;
//        /// <summary>
//        /// Gets or sets the Prepack Quantity for this option
//        /// </summary>
//        public int QuantityPerPack
//        {
//            get
//            {
//                return _qtyPerPack;
//            }

//            set
//            {
//                _qtyPerPack = value;
//            }
//        }

//        int _numberOfPacks = 0;
//        ///<summary>
//        /// Gets or sets the Number of Packs for this option
//        ///</summary>
//        public int NumberOfPacks
//        {
//            get
//            {
//                return _numberOfPacks;
//            }
//            set
//            {
//                _numberOfPacks = value;
//            }
//        }

//        int _numberOfPackUnits = 0;
//        /// <summary>
//        /// Gets or sets the Number of Pack Units for this option
//        /// </summary>
//        public int NumberOfPackUnits
//        {
//            get
//            {
//                return _numberOfPackUnits;
//            }
//            set
//            {
//                _numberOfPackUnits = value;
//            }
//        }

//        double _percentOfUnitsInPacks = 0;
//        /// <summary>
//        /// Gets or sets the Percent of Units In Packs for this option
//        /// </summary>
//        public double PercentOfUnitsInPacks
//        {
//            get
//            {
//                return _percentOfUnitsInPacks;
//            }
//            set
//            {
//                _percentOfUnitsInPacks = value;
//            }
//        }

//        int _numberOfStoresSatisfied = 0;
//        /// <summary>
//        /// Gets of sets the Number of Stores Satisfied by this prepack option
//        /// </suammary>
//        public int NumberOfStoresSatisfied
//        {
//            get
//            {
//                return _numberOfStoresSatisfied;
//            }
//            set
//            {
//                _numberOfStoresSatisfied = value;
//            }
//        }

//        double _percentOfStoresSatisfied = 0;
//        /// <summary>
//        /// Gets of sets the Percent of Stores Satisfied by this prepack option
//        /// </suammary>
//        public double PercentOfStoresSatisfied
//        {
//            get
//            {
//                return _percentOfStoresSatisfied;
//            }
//            set
//            {
//                _percentOfStoresSatisfied = value;
//            }
//        }

//        double _avgErrorPerSize = 0;
//        /// <summary>
//        /// Gets or sets the Average Error Per Size in this option
//        /// </summary>
//        public double AverageErrorPerSize
//        {
//            get
//            {
//                return _avgErrorPerSize;
//            }
//            set
//            {
//                _avgErrorPerSize = value;
//            }
//        }

//        double _avgErrorPerPack = 0;
//        /// <summary>
//        /// Gets or sets the Average Error Per Pack in this option
//        /// </summary>
//        public double AverageErrorPerPack
//        {
//            get
//            {
//                return _avgErrorPerPack;
//            }
//            set
//            {
//                _avgErrorPerPack = value;
//            }
//        }

//        int _numberOfBulkUnits = 0;
//        /// <summary>
//        /// Gets or sets the Number of Bulk Units in this option
//        /// </summary>
//        public int NumberOfBulkUnits
//        {
//            get
//            {
//                return _numberOfBulkUnits;
//            }
//            set
//            {
//                _numberOfBulkUnits = value;
//            }
//        }

//        double _percentInBulk = 0;
//        /// <summary>
//        /// Gets or sets the Percent In Bulk for this option
//        /// </summary>
//        public double PercentInBulk
//        {
//            get
//            {
//                return _percentInBulk;
//            }
//            set
//            {
//                _percentInBulk = value;
//            }
//        }

//        int _numberOfStoresWithBulk = 0;
//        /// <summary>
//        /// Gets or sets the Number of Stores With Bulk for this option
//        /// </summary>
//        public int NumberOfStoresWithBulk
//        {
//            get
//            {
//                return _numberOfStoresWithBulk;
//            }
//            set
//            {
//                _numberOfStoresWithBulk = value;
//            }
//        }

//        double _bulkReservePercent = 0;
//        /// <summary>
//        /// Gets or sets the Bulk Reserve Percentage for this option
//        /// </summary>
//        public double BulkReservePercentage
//        {
//            get
//            {
//                return _bulkReservePercent;
//            }
//            set
//            {
//                _bulkReservePercent = value;
//            }
//        }
//    }

//}
