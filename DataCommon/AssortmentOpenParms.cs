using System;
using System.Collections;

namespace MIDRetail.DataCommon
{
	public class AssortmentOpenParms
	{
		//=======
		// FIELDS
		//=======

		private int _storeGroupRID;
		private int _filterRID;
		private eAllocationAssortmentViewGroupBy _groupBy;
		private int _viewRID;
		private string _viewName;
		private int _viewUserID;

        private eAssortmentVariableType _variableType;
        private int _variableNumber;
        private bool _inclOnhand;
        private bool _inclIntransit;
        private bool _inclSimStores;
        private bool _inclCommitted;
        private eStoreAverageBy _averageBy;
        private eGradeBoundary _gradeBoundary;

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentOpenParms()
		{
			_storeGroupRID = Include.NoRID;
			_filterRID = Include.NoRID;
			_groupBy = eAllocationAssortmentViewGroupBy.StoreGrade;
			_viewRID = Include.DefaultAssortmentViewRID;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the RID of the Store Group.
		/// </summary>

		public int StoreGroupRID
		{
			get
			{
				return _storeGroupRID;
			}
			set
			{
				_storeGroupRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the RID of the Filter.
		/// </summary>

		public int FilterRID
		{
			get
			{
				return _filterRID;
			}
			set
			{
				_filterRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the eAllocationAssortmentViewGroupBy by which the data is to be grouped.
		/// </summary>

		public eAllocationAssortmentViewGroupBy GroupBy
		{
			get
			{
				return _groupBy;
			}
			set
			{
				_groupBy = value;
			}
		}

		/// <summary>
		/// Gets or sets the RID of View.
		/// </summary>

		public int ViewRID
		{
			get
			{
				return _viewRID;
			}
			set
			{
				_viewRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of View.
		/// </summary>

		public string ViewName
		{
			get
			{
				return _viewName;
			}
			set
			{
				_viewName = value;
			}
		}

		/// <summary>
		/// Gets or sets the User ID of View.
		/// </summary>

		public int ViewUserID
		{
			get
			{
				return _viewUserID;
			}
			set
			{
				_viewUserID = value;
			}
		}

        /// <summary>
        /// Gets or sets the Assortment Variable Type.
        /// </summary>

        public eAssortmentVariableType VariableType
        {
            get
            {
                return _variableType;
            }
            set
            {
                _variableType = value;
            }
        }

        /// <summary>
        /// Gets or sets the Assortment Variable Number.
        /// </summary>

        public int VariableNumber
        {
            get
            {
                return _variableNumber;
            }
            set
            {
                _variableNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets Include OnHand.
        /// </summary>

        public bool InclOnhand
        {
            get
            {
                return _inclOnhand;
            }
            set
            {
                _inclOnhand = value;
            }
        }

        /// <summary>
        /// Gets or sets Include Intransit.
        /// </summary>

        public bool InclIntransit
        {
            get
            {
                return _inclIntransit;
            }
            set
            {
                _inclIntransit = value;
            }
        }

        /// <summary>
        /// Gets or sets Include Similar Stores.
        /// </summary>

        public bool InclSimStores
        {
            get
            {
                return _inclSimStores;
            }
            set
            {
                _inclSimStores = value;
            }
        }

        /// <summary>
        /// Gets or sets Include Committed.
        /// </summary>

        public bool InclCommitted
        {
            get
            {
                return _inclCommitted;
            }
            set
            {
                _inclCommitted = value;
            }
        }

        /// <summary>
        /// Gets or sets Store Average By.
        /// </summary>

        public eStoreAverageBy AverageBy
        {
            get
            {
                return _averageBy;
            }
            set
            {
                _averageBy = value;
            }
        }
         
        /// <summary>
        /// Gets or sets Grade Boundary.
        /// </summary>

        public eGradeBoundary GradeBoundary
        {
            get
            {
                return _gradeBoundary;
            }
            set
            {
                _gradeBoundary = value;
            }
        }
	}
}
