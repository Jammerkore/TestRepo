using System;

namespace MIDRetail.Business
{
	[Serializable]
	public class PlanSaveParms
	{
		//=======
		// FIELDS
		//=======

		private bool _saveStoreHighLevel;
		private bool _saveStoreLowLevel;
		private bool _saveChainHighLevel;
		private bool _saveChainLowLevel;
		private bool _saveView;
		private bool _overrideStoreLowLevel;
		private bool _overrideChainLowLevel;
		private bool _saveHighLevelAllStoreAsChain;
		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		private bool _saveLowLevelChainAsChain;
		//End Enhancement - JScott - Add Balance Low Levels functionality
		private bool _saveLowLevelAllStoreAsChain;
		private int _storeHighLevelNodeRID;
		private int _storeHighLevelVersionRID;
		private int _storeHighLevelDateRangeRID;
		private int _storeLowLevelVersionRID;
		private int _storeLowLevelDateRangeRID;
		private int _chainHighLevelNodeRID;
		private int _chainHighLevelVersionRID;
		private int _chainHighLevelDateRangeRID;
		private int _chainLowLevelVersionRID;
		private int _chainLowLevelDateRangeRID;
		private int _viewUserRID;
		private string _viewName;
		private bool _saveLocks;

		//=============
		// CONSTRUCTORS
		//=============

		public PlanSaveParms()
		{
			_saveStoreHighLevel = false;
			_saveStoreLowLevel = false;
			_saveChainHighLevel = false;
			_saveChainLowLevel = false;
			_saveView = false;
			_overrideStoreLowLevel = false;
			_overrideChainLowLevel = false;
			_saveHighLevelAllStoreAsChain = false;
			//Begin Enhancement - JScott - Add Balance Low Levels functionality
			_saveLowLevelChainAsChain = false;
			//End Enhancement - JScott - Add Balance Low Levels functionality
			_saveLowLevelAllStoreAsChain = false;
			_saveLocks = true;
		}

		//===========
		// PROPERTIES
		//===========

		public bool SaveStoreHighLevel
		{
			get
			{
				return _saveStoreHighLevel;
			}
			set
			{
				_saveStoreHighLevel = value;
			}
		}

		public bool SaveStoreLowLevel
		{
			get
			{
				return _saveStoreLowLevel;
			}
			set
			{
				_saveStoreLowLevel = value;
			}
		}

		public bool SaveChainHighLevel
		{
			get
			{
				return _saveChainHighLevel;
			}
			set
			{
				_saveChainHighLevel = value;
			}
		}

		public bool SaveChainLowLevel
		{
			get
			{
				return _saveChainLowLevel;
			}
			set
			{
				_saveChainLowLevel = value;
			}
		}

		public bool SaveView
		{
			get
			{
				return _saveView;
			}
			set
			{
				_saveView = value;
			}
		}

		public bool OverrideStoreLowLevel
		{
			get
			{
				return _overrideStoreLowLevel;
			}
			set
			{
				_overrideStoreLowLevel = value;
			}
		}

		public bool OverrideChainLowLevel
		{
			get
			{
				return _overrideChainLowLevel;
			}
			set
			{
				_overrideChainLowLevel = value;
			}
		}

		public bool SaveHighLevelAllStoreAsChain
		{
			get
			{
				return _saveHighLevelAllStoreAsChain;
			}
			set
			{
				_saveHighLevelAllStoreAsChain = value;
			}
		}

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		public bool SaveLowLevelChainAsChain
		{
			get
			{
				return _saveLowLevelChainAsChain;
			}
			set
			{
				_saveLowLevelChainAsChain = value;
			}
		}

		public bool SaveLowLevelAllStoreAsChain
		{
			get
			{
				return _saveLowLevelAllStoreAsChain;
			}
			set
			{
				_saveLowLevelAllStoreAsChain = value;
			}
		}

		public int StoreHighLevelNodeRID
		{
			get
			{
				return _storeHighLevelNodeRID;
			}
			set
			{
				_storeHighLevelNodeRID = value;
			}
		}

		public int StoreHighLevelVersionRID
		{
			get
			{
				return _storeHighLevelVersionRID;
			}
			set
			{
				_storeHighLevelVersionRID = value;
			}
		}

		public int StoreHighLevelDateRangeRID
		{
			get
			{
				return _storeHighLevelDateRangeRID;
			}
			set
			{
				_storeHighLevelDateRangeRID = value;
			}
		}

		public int StoreLowLevelVersionRID
		{
			get
			{
				return _storeLowLevelVersionRID;
			}
			set
			{
				_storeLowLevelVersionRID = value;
			}
		}

		public int StoreLowLevelDateRangeRID
		{
			get
			{
				return _storeLowLevelDateRangeRID;
			}
			set
			{
				_storeLowLevelDateRangeRID = value;
			}
		}

		public int ChainHighLevelNodeRID
		{
			get
			{
				return _chainHighLevelNodeRID;
			}
			set
			{
				_chainHighLevelNodeRID = value;
			}
		}

		public int ChainHighLevelVersionRID
		{
			get
			{
				return _chainHighLevelVersionRID;
			}
			set
			{
				_chainHighLevelVersionRID = value;
			}
		}

		public int ChainHighLevelDateRangeRID
		{
			get
			{
				return _chainHighLevelDateRangeRID;
			}
			set
			{
				_chainHighLevelDateRangeRID = value;
			}
		}

		public int ChainLowLevelVersionRID
		{
			get
			{
				return _chainLowLevelVersionRID;
			}
			set
			{
				_chainLowLevelVersionRID = value;
			}
		}

		public int ChainLowLevelDateRangeRID
		{
			get
			{
				return _chainLowLevelDateRangeRID;
			}
			set
			{
				_chainLowLevelDateRangeRID = value;
			}
		}

		public int ViewUserRID
		{
			get
			{
				return _viewUserRID;
			}
			set
			{
				_viewUserRID = value;
			}
		}

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

		public bool SaveLocks
		{
			get
			{
				return _saveLocks;
			}
			set
			{
				_saveLocks = value;
			}
		}

		//========
		// METHODS
		//========
	}
}
