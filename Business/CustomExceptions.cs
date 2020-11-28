using System;
using System.Collections;
using System.Runtime.Serialization;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	[Serializable]
	public class PlanInUseException : Exception
	{
		private string _inUseMessage;

		public PlanInUseException()
			: base()
		{
		}

		public PlanInUseException(string aInUseMessage)
			: base()
		{
			_inUseMessage = aInUseMessage;
		}

		protected PlanInUseException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}

		public string InUseMessage
		{
			get
			{
				return _inUseMessage;
			}
		}
	}

	[Serializable]
	public class CancelProcessException : Exception
	{
		public CancelProcessException()
			:base()
		{
		}
		
		public CancelProcessException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}	

	[Serializable]
	public class JobDoesNotExist : Exception
	{
		public JobDoesNotExist()
			: base()
		{
		}

		protected JobDoesNotExist(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	[Serializable]
	public class InvalidJobStatusForAction : Exception
	{
		public InvalidJobStatusForAction()
			: base()
		{
		}

		protected InvalidJobStatusForAction(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	[Serializable]
	public class CellUnavailableException : Exception
	{
		public CellUnavailableException()
			:base()
		{
		}
		
		public CellUnavailableException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}	

	[Serializable]
	public class FormulaConflictException : Exception
	{
	}

	[Serializable]
	public class CellPendingException : Exception
	{
		private ComputationCellReference _compCellRef;

		public CellPendingException(ComputationCellReference aCompCellRef)
			: base()
		{
			_compCellRef = aCompCellRef;
		}

		public ComputationCellReference ComputationCellReference
		{
			get
			{
				return _compCellRef;
			}
		}
	}

	[Serializable]
	public class CellCompChangedException : Exception
	{
	}

	[Serializable]
	public class CellNotAvailableException : Exception
	{
		public CellNotAvailableException(string aMessage)
			: base(aMessage)
		{
		}
	}

	[Serializable]
	public class CircularReferenceException : Exception
	{
	}

	[Serializable]
	public class NoCellsToSpreadTo : Exception
	{
	}

	[Serializable]
	abstract public class CustomUserErrorException : Exception
	{
		public CustomUserErrorException(string aMessage)
			: base(aMessage)
		{
		}
	}

	[Serializable]
	public class PlanConflictException : Exception
	{
	}

	[Serializable]
	public class NothingToSpreadException : Exception
	{
	}

    // Begin TT#189 MD - JSmith - Filter Performance
    //[Serializable]
    //public class EndOfOperandsException : Exception
    //{
    //}
    // End TT#189 MD

	[Serializable]
	public class FilterUsesCurrentPlanException : Exception
	{
	}

	[Serializable]
	public class FilterContainsDataException : Exception
	{
	}

	[Serializable]
	public class AccessingNullFormException : Exception
	{
		public AccessingNullFormException()
			: base("Attempting to access a null form in the filter.")
		{
		}
	}

	[Serializable]
	public class EditErrorException : Exception
	{
		public EditErrorException(string aMessage)
			: base(aMessage)
		{
		}
	}

	[Serializable]
	public class EndPageLoadThreadException : Exception
	{
	}

	[Serializable]
	public class WaitPageLoadException : Exception
	{
	}

	[Serializable]
	public class EndCellFormatThreadException : Exception
	{
	}
	
	[Serializable]
	public class HeaderConflictException : Exception
	{
	}

    //[Serializable]
    //public class FilterSyntaxErrorException : Exception
    //{
    //    private eFilterListType _filterListType;
    //    private QueryOperand _errorOperand;

    //    public FilterSyntaxErrorException(string aMessage, eFilterListType aFilterListType, QueryOperand aErrorOperand)
    //        : base(aMessage)
    //    {
    //        _filterListType = aFilterListType;
    //        _errorOperand = aErrorOperand;
    //    }

    //    public eFilterListType FilterListType
    //    {
    //        get
    //        {
    //            return _filterListType;
    //        }
    //    }

    //    public QueryOperand ErrorOperand
    //    {
    //        get
    //        {
    //            return _errorOperand;
    //        }
    //    }
    //}

    //[Serializable]
    //public class FilterOperandErrorException : Exception
    //{
    //    public FilterOperandErrorException(string aMessage)
    //        : base(aMessage)
    //    {
    //    }
    //}

	[Serializable]
	public class EndScheduleProcessThreadException : Exception
	{
	}

	[Serializable]
	public class EndScheduleBrowserRefreshThreadException : Exception
	{
	}

	[Serializable]
	public class UserCancelledException : Exception
	{
	}

	[Serializable]
	public class SpreadFailed : Exception
	{
		private eSpreadType _spreadType;
		private PlanProfile _planProfile;
		private DateProfile _dateProfile;
		private StoreProfile _storeProfile;
		private int _recomputesProcessed = 0;

		public SpreadFailed(string aMessage, DateProfile aDateProfile, PlanProfile aPlanProfile, int aRecomputesProcessed)
			: base(aMessage)
		{
			_spreadType = eSpreadType.ChainToStore;
			_planProfile = aPlanProfile;
			_dateProfile = aDateProfile;
			_storeProfile = null;
			 _recomputesProcessed = aRecomputesProcessed;
		}

		public SpreadFailed(string aMessage, DateProfile aDateProfile, StoreProfile aStoreProfile, int aRecomputesProcessed)
			: base(aMessage)
		{
			_spreadType = eSpreadType.HighLevelToLowLevel;
			_planProfile = null;
			_dateProfile = aDateProfile;
			_storeProfile = aStoreProfile;
			_recomputesProcessed = aRecomputesProcessed;
		}

		protected SpreadFailed(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		public eSpreadType SpreadType
		{
			get
			{
				return _spreadType;
			}
		}

		public PlanProfile PlanProfile
		{
			get
			{
				return _planProfile;
			}
		}

		public DateProfile DateProfile
		{
			get
			{
				return _dateProfile;
			}
		}

		public StoreProfile StoreProfile
		{
			get
			{
				return _storeProfile;
			}
		}

		public int RecomputesProcessed
		{
			get
			{
				return _recomputesProcessed;
			}
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// Thrown when Header selected by a method is 'in use' by a multi-header.
	/// </summary>
	[Serializable]
	public class HeaderInUseException : Exception
	{
		private ArrayList _headerList;

		public HeaderInUseException(string aMessage, ArrayList headerList)
			: base(aMessage)
		{
			_headerList = headerList;
		}

		public ArrayList HeaderList
		{
			get
			{
				return _headerList;
			}
		}
	}

	// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
    /// <summary>
    /// Thrown when Header selected by a method belongs to Group Allocation.
    /// </summary>
    [Serializable]
    public class HeaderInGroupAllocationException : Exception
    {
        private ArrayList _headerList;

        public HeaderInGroupAllocationException(string aMessage, ArrayList headerList)
            : base(aMessage)
        {
            _headerList = headerList;
        }

        public ArrayList HeaderList
        {
            get
            {
                return _headerList;
            }
        }
    }
	// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 

    // Begin TT#1019 - MD - stodd - interactive velocity vs read-only group allocation -  
    /// <summary>
    /// Thrown when a method is processed against a read only Group Allocation or Assortment.
    /// Currently only used by interactive Velocity.
    /// </summary>
    [Serializable]
    public class GroupAllocationAssortmentReadOnlyException : Exception
    {
        private string _groupType;
        private string _groupName;

        public GroupAllocationAssortmentReadOnlyException(string aMessage, string groupType, string groupName)
            : base(aMessage)
        {
            _groupType = groupType;
            _groupName = groupName;
        }

        public string GroupType
        {
            get
            {
                return _groupType;
            }
        }
        public string GroupName
        {
            get
            {
                return _groupName;
            }
        }
    }
    // End TT#1019 - MD - stodd - interactive velocity vs read-only group allocation -  
	
	[Serializable]
	public class EndSearchThreadException : Exception
	{
	}
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	public class ServiceUnavailable : Exception
	{
		public ServiceUnavailable(string aService)
			: base("A call to the " + aService + " service has been attempted and retried, but cannot complete.  The service appears to be unavailable.")
		{
		}

		public ServiceUnavailable(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}
	//End TT#708 - JScott - Services need a Retry availalbe.
}
