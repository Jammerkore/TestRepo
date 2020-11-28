using System;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ExplorerAddressBlock.
	/// </summary>
	[Serializable]
	public class ExplorerAddressBlock
	{
		private MIDRetail.Windows.Explorer _explorer;
		private MIDRetail.Windows.MerchandiseExplorer	_merchandiseExplorer;
		private MIDRetail.Windows.WorkflowMethodExplorer _workflowMethodExplorer;
		private MIDRetail.Windows.StoreGroupExplorer _storeGroupExplorer;
		private MIDRetail.Windows.AllocationWorkspaceExplorer _allocationWorkspaceExplorer;
		private MIDRetail.Windows.FilterStoreExplorer _filterExplorerStore;
        private MIDRetail.Windows.FilterHeaderExplorer _filterExplorerHeader; //TT#1313-MD -jsobek -Header Filters
        private MIDRetail.Windows.FilterAssortmentExplorer _filterExplorerAssortment; //TT#1313-MD -jsobek -Header Filters
        private MIDRetail.Windows.TaskListExplorer _taskListExplorer;
		private MIDRetail.Windows.AssortmentExplorer _assortmentExplorer;	// Track #5835
        private MIDRetail.Windows.AssortmentWorkspaceExplorer _assortmentWorkspaceExplorer; // TT#2 Assortment Planning
        private MIDRetail.Windows.UserActivityExplorer _UserActivityExplorer;  // TT#46 MD - JSmith - User Dashboard 

		public ExplorerAddressBlock()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns the reference to the main explorer
		/// </summary>

		public MIDRetail.Windows.Explorer Explorer
		{
			get
			{
				return _explorer;
			}
			set
			{
				_explorer = value;
			}
		}

		/// <summary>
		/// Returns the reference to the merchandise explorer
		/// </summary>

		public MIDRetail.Windows.MerchandiseExplorer MerchandiseExplorer
		{
			get
			{
				return _merchandiseExplorer;
			}
			set
			{
				_merchandiseExplorer = value;
			}
		}

		/// <summary>
		/// Returns the reference to the workflow/method explorer
		/// </summary>

		public MIDRetail.Windows.WorkflowMethodExplorer WorkflowMethodExplorer
		{
			get
			{
				return _workflowMethodExplorer;
			}
			set
			{
				_workflowMethodExplorer = value;
			}
		}

		/// <summary>
		/// Returns the reference to the store explorer
		/// </summary>

		public MIDRetail.Windows.StoreGroupExplorer StoreGroupExplorer
		{
			get
			{
				return _storeGroupExplorer;
			}
			set
			{
				_storeGroupExplorer = value;
			}
		}

		/// <summary>
		/// Returns the reference to the allocation workspace explorer
		/// </summary>

		public MIDRetail.Windows.AllocationWorkspaceExplorer AllocationWorkspaceExplorer
		{
			get
			{
				return _allocationWorkspaceExplorer;
			}
			set
			{
				_allocationWorkspaceExplorer = value;
			}
		}

		/// <summary>
		/// Returns the reference to the store filter explorer
		/// </summary>
		public MIDRetail.Windows.FilterStoreExplorer FilterExplorerStore
		{
			get
			{
				return _filterExplorerStore;
			}
			set
			{
				_filterExplorerStore = value;
			}
		}

        //Begin TT#1313-MD -jsobek -Header Filters
        /// <summary>
        /// Returns the reference to the header filter explorer
        /// </summary>
        public MIDRetail.Windows.FilterHeaderExplorer FilterExplorerHeader
        {
            get
            {
                return _filterExplorerHeader;
            }
            set
            {
                _filterExplorerHeader = value;
            }
        }

        /// <summary>
        /// Returns the reference to the assortment filter explorer
        /// </summary>
        public MIDRetail.Windows.FilterAssortmentExplorer FilterExplorerAssortment
        {
            get
            {
                return _filterExplorerAssortment;
            }
            set
            {
                _filterExplorerAssortment = value;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

		/// <summary>
		/// Returns the reference to the task list explorer
		/// </summary>

		public MIDRetail.Windows.TaskListExplorer TaskListExplorer
		{
			get
			{
				return _taskListExplorer;
			}
			set
			{
				_taskListExplorer = value;
			}
		}
		// Begin Track #5835
		/// <summary>
		/// Returns the reference to the Assortment explorer
		/// </summary>

		public MIDRetail.Windows.AssortmentExplorer AssortmentExplorer
		{
			get
			{
				return _assortmentExplorer;
			}
			set
			{
				_assortmentExplorer = value;
			}
		}
		// End Track 5835

        // Begin TT#2 Assortment Planning
        /// <summary>
        /// Returns the reference to the assortment workspace explorer
        /// </summary>

        public MIDRetail.Windows.AssortmentWorkspaceExplorer AssortmentWorkspaceExplorer
        {
            get
            {
                return _assortmentWorkspaceExplorer;
            }
            set
            {
                _assortmentWorkspaceExplorer = value;
            }
        }
        // End TT#2  

        // Begin TT#46 MD - JSmith - User Dashboard 
		/// <summary>
        /// Returns the reference to the user dashboard explorer
        /// </summary>

        public MIDRetail.Windows.UserActivityExplorer UserActivityExplorer
        {
            get
            {
                return _UserActivityExplorer;
            }
            set
            {
                _UserActivityExplorer = value;
            }
        }
        // End TT#46 MD
	}
}
