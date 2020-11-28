using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// This exception is thrown when the application can not access the database
	/// </summary>
	[Serializable()]
	public class MIDDatabaseUnavailableException : System.Exception
	{
		public MIDDatabaseUnavailableException (string aMessage) : base (aMessage)
	{
	}
	
		public MIDDatabaseUnavailableException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
	{
	}

		protected MIDDatabaseUnavailableException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
	{
	}
	}

	/// <summary>
	/// This exception is thrown when the database violates a foreign key constraint
	/// </summary>
	[Serializable()]
	public class DatabaseForeignKeyViolation : System.Exception
	{
		public DatabaseForeignKeyViolation (string aMessage) : base (aMessage)
		{
		}
	
		public DatabaseForeignKeyViolation (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected DatabaseForeignKeyViolation(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

//	/// <summary>
//	/// This exception is thrown when the database deadlock is encountered
//	/// </summary>
//	[Serializable()]
//	public class DatabaseDeadlockViolation : System.Exception
//	{
//		public DatabaseDeadlockViolation (string aMessage) : base (aMessage)
//		{
//		}
//	
//		public DatabaseDeadlockViolation (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
//		{
//		}
//
//		protected DatabaseDeadlockViolation(SerializationInfo aInfo, StreamingContext aContext)
//			: base(aInfo, aContext)
//		{
//		}
//
//		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
//		{
//			base.GetObjectData(aInfo, aContext);
//		}
//	}

	/// <summary>
	/// This exception is thrown when the database blocking error is encountered
	/// </summary>
	[Serializable()]
	public class DatabaseRetryableViolation : System.Exception
	{
        // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        string _midErrorMessage;
        public DatabaseRetryableViolation(string aMessage, string aMidErrorMessage)
            : base(aMessage)
		{
            _midErrorMessage = aMidErrorMessage;
		}
	
		public DatabaseRetryableViolation (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}
        // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue

		protected DatabaseRetryableViolation(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}

        // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        /// <summary>
        /// When "true", expand error number to get message text; when "false" error number already expanded.
        /// </summary>
        public string MidErrorMessage
        {
            get
            {
                return _midErrorMessage;
            }
        }
        // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
	}

	/// <summary>
	/// This exception is thrown when the database unique index constraint is violated
	/// </summary>
	[Serializable()]
	public class DatabaseUniqueIndexConstriantViolation : System.Exception
	{
		public DatabaseUniqueIndexConstriantViolation (string aMessage) : base (aMessage)
		{
		}
	
		public DatabaseUniqueIndexConstriantViolation (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected DatabaseUniqueIndexConstriantViolation(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when the database unique index constraint is violated
	/// </summary>
	[Serializable()]
	public class DatabaseUniqueIndexConstriantViolation2 : System.Exception
	{
		public DatabaseUniqueIndexConstriantViolation2 (string aMessage) : base (aMessage)
		{
		}
	
		public DatabaseUniqueIndexConstriantViolation2 (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected DatabaseUniqueIndexConstriantViolation2(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when the database is invalid
	/// </summary>
	[Serializable()]
	public class InvalidDatabase : System.Exception
	{
		public InvalidDatabase (string aMessage) : base (aMessage)
		{
		}
	
		public InvalidDatabase (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected InvalidDatabase(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when the application is unable to log into the database
	/// </summary>
	[Serializable()]
	public class DatabaseLoginFailed : System.Exception
	{
		public DatabaseLoginFailed (string aMessage) : base (aMessage)
		{
		}
	
		public DatabaseLoginFailed (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected DatabaseLoginFailed(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}


	//Begin Track #6269 - JScott - Error logging on after auto upgrade
	///// <summary>
	///// This exception is thrown when a relationship between two items is not found
	///// </summary>
	//[Serializable()]
	//public class RelationshipNotFoundException : System.Exception
	//{
	//    public RelationshipNotFoundException () : base ()
	//    {
	//    }

	//    public RelationshipNotFoundException (string aMessage) : base (aMessage)
	//    {
	//    }
	
	//    public RelationshipNotFoundException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
	//    {
	//    }

	//    protected RelationshipNotFoundException(SerializationInfo aInfo, StreamingContext aContext)
	//        : base(aInfo, aContext)
	//    {
	//    }

	//    override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
	//    {
	//        base.GetObjectData(aInfo, aContext);
	//    }
	//}

	//End Track #6269 - JScott - Error logging on after auto upgrade
	/// <summary>
	/// This exception is thrown when a size definition's category/primary/secondary is not unique
	/// </summary>
	[Serializable()]
	public class SizeCatgPriSecNotUniqueException : System.Exception
	{
		public SizeCatgPriSecNotUniqueException () : base ()
		{
		}

		public SizeCatgPriSecNotUniqueException (string aMessage) : base (aMessage)
		{
		}
	
		public SizeCatgPriSecNotUniqueException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected SizeCatgPriSecNotUniqueException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a size definition's primary is missing
	/// </summary>
	[Serializable()]
	public class SizePrimaryRequiredException : System.Exception
	{
		public SizePrimaryRequiredException () : base ()
		{
		}

		public SizePrimaryRequiredException (string aMessage) : base (aMessage)
		{
		}
	
		public SizePrimaryRequiredException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected SizePrimaryRequiredException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a product is not found
	/// </summary>
	[Serializable()]
	public class ProductNotFoundException : System.Exception
	{
		public ProductNotFoundException () : base ()
		{
		}

		public ProductNotFoundException (string aMessage) : base (aMessage)
		{
		}
	
		public ProductNotFoundException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected ProductNotFoundException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when the first level in a product is not a style
	/// </summary>
	[Serializable()]
	public class FirstLevelNotStyleException : System.Exception
	{
		public FirstLevelNotStyleException () : base ()
		{
		}

		public FirstLevelNotStyleException (string aMessage) : base (aMessage)
		{
		}
	
		public FirstLevelNotStyleException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected FirstLevelNotStyleException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when the format of data is invalid
	/// </summary>
	[Serializable()]
	public class FormatInvalidException : System.Exception
	{
		public FormatInvalidException () : base ()
		{
		}

		public FormatInvalidException (string aMessage) : base (aMessage)
		{
		}
	
		public FormatInvalidException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected FormatInvalidException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a color level is not found in the hierarchy level definitions
	/// </summary>
	[Serializable()]
	public class ColorLevelNotFoundException : System.Exception
	{
		public ColorLevelNotFoundException () : base ()
		{
		}

		public ColorLevelNotFoundException (string aMessage) : base (aMessage)
		{
		}
	
		public ColorLevelNotFoundException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected ColorLevelNotFoundException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a color code does not lie within the constraints of hierarchy color level definitions
	/// </summary>
	[Serializable()]
	public class ColorCodeNotValidException : System.Exception
	{
		public ColorCodeNotValidException () : base ()
		{
		}

		public ColorCodeNotValidException (string aMessage) : base (aMessage)
		{
		}
	
		public ColorCodeNotValidException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected ColorCodeNotValidException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when an invalid hierarchy level length type is found
	/// </summary>
	[Serializable()]
	public class InvalidLevelLengthTypeException : System.Exception
	{
		public InvalidLevelLengthTypeException () : base ()
		{
		}

		public InvalidLevelLengthTypeException (string aMessage) : base (aMessage)
		{
		}
	
		public InvalidLevelLengthTypeException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected InvalidLevelLengthTypeException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a size level is not found in the hierarchy level definitions
	/// </summary>
	[Serializable()]
	public class SizeLevelNotFoundException : System.Exception
	{
		public SizeLevelNotFoundException () : base ()
		{
		}

		public SizeLevelNotFoundException (string aMessage) : base (aMessage)
		{
		}
	
		public SizeLevelNotFoundException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected SizeLevelNotFoundException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a size code does not lie within the constraints of hierarchy size level definitions
	/// </summary>
	[Serializable()]
	public class SizeCodeNotValidException : System.Exception
	{
		public SizeCodeNotValidException () : base ()
		{
		}

		public SizeCodeNotValidException (string aMessage) : base (aMessage)
		{
		}
	
		public SizeCodeNotValidException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected SizeCodeNotValidException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when a node ID exceeds the constraints of hierarchy level definitions
	/// </summary>
	[Serializable()]
	public class NodeIDTooLargeException : System.Exception
	{
		public NodeIDTooLargeException () : base ()
		{
		}

		public NodeIDTooLargeException (string aMessage) : base (aMessage)
		{
		}
	
		public NodeIDTooLargeException (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected NodeIDTooLargeException(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

	/// <summary>
	/// This exception is thrown when the object being referenced is not in the database catalog
	/// </summary>
	[Serializable()]
	public class DatabaseNotInCatalog : System.Exception
	{
		public DatabaseNotInCatalog (string aMessage) : base (aMessage)
		{
		}
	
		public DatabaseNotInCatalog (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected DatabaseNotInCatalog(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}

    // Begin Track #6304 - JSmith - query processor could not start the necessary thread resources for parallel query 
    /// <summary>
    /// This exception is thrown when the query processor could not start the necessary thread resources for parallel query
    /// </summary>
    [Serializable()]
    public class ParallelQueryThreadError : System.Exception
    {
        public ParallelQueryThreadError(string aMessage)
            : base(aMessage)
        {
        }

        public ParallelQueryThreadError(string aMessage, Exception aInnerException)
            : base(aMessage, aInnerException)
        {
        }

        protected ParallelQueryThreadError(SerializationInfo aInfo, StreamingContext aContext)
            : base(aInfo, aContext)
        {
        }

        override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
        {
            base.GetObjectData(aInfo, aContext);
        }
    }
    // End Track #6304

	// BEGIN Issue 4535 - 8.1.07
	/// <summary>
	/// This exception is thrown when an empty store list is sent to Data
	/// </summary>
	[Serializable()]
	public class EmptyStoreList : System.Exception
	{
		public EmptyStoreList (string aMessage) : base (aMessage)
		{
		}
	
		public EmptyStoreList (string aMessage, Exception aInnerException) : base (aMessage, aInnerException)
		{
		}

		protected EmptyStoreList(SerializationInfo aInfo, StreamingContext aContext)
			: base(aInfo, aContext)
		{
		}

		override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
		{
			base.GetObjectData(aInfo, aContext);
		}
	}
	// END Issue 4535 - 8.1.07

    // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
    /// <summary>
    /// This exception is thrown when an empty store list is sent to Data
    /// </summary>
    [Serializable()]
    public class DuplicateOverrideListEntry : System.Exception
    {
        public DuplicateOverrideListEntry(string aMessage)
            : base(aMessage)
        {
        }

        public DuplicateOverrideListEntry(string aMessage, Exception aInnerException)
            : base(aMessage, aInnerException)
        {
        }

        protected DuplicateOverrideListEntry(SerializationInfo aInfo, StreamingContext aContext)
            : base(aInfo, aContext)
        {
        }

        override public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
        {
            base.GetObjectData(aInfo, aContext);
        }
    }
    // End TT#2281

    //Begin TT#1313-MD -Header Filters -unused class
    // begin TT#173  Provide database container for large data collections
    //[Serializable]
    //public class EnqueueConflictException : Exception
    //{
    //}
    // end TT#173  Provide database container for large data collections
    //End TT#1313-MD -Header Filters -unused class

    // begin TT#370 Build Packs Enhancement -- J.Ellis 
    /// <summary>
    /// Summary description for MIDException.
    /// </summary>
    [Serializable()]
    public class MIDException : System.Exception
    {
        private eErrorLevel _errorLevel;
        private int _errorNumber;
        private bool _expandErrorMessage;  // TT#370 Build Packs Enhancement -- J.Ellis
        private string _memberName;
        private string _sourceFilePath;
        private int _sourceLineNumber;
        //private Audit _audit;

        // Constructors
        public MIDException(eErrorLevel aErrorLevel, int aErrorNumber, string aErrorMessage = "",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            : base(aErrorMessage == "" ? "MID Error#" + aErrorNumber.ToString() : aErrorMessage)
        {
            _expandErrorMessage = false;  // TT#370 Build Packs Enhancement
            _errorLevel = aErrorLevel;
            _errorNumber = aErrorNumber;
            _memberName = memberName;
            _sourceFilePath = sourceFilePath;
            _sourceLineNumber = sourceLineNumber;
            //_audit = new Audit();
        }

        public MIDException(eErrorLevel aErrorLevel, int aErrorNumber, string aErrorMessage, Exception innerException,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            : base(aErrorMessage, innerException)
        {
            _expandErrorMessage = false; // TT#370 Build Packs Enhancement
            _errorLevel = aErrorLevel;
            _errorNumber = aErrorNumber;
            _memberName = memberName;
            _sourceFilePath = sourceFilePath;
            _sourceLineNumber = sourceLineNumber;
            //_audit = new Audit();
        }
        // begin TT#370 added constructors to delay expansion of MID Error message
        //public MIDException(eErrorLevel aErrorLevel, int aErrorNumber,
        //[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        //[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        //[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        //    : base("MID Error#" + aErrorNumber.ToString(CultureInfo.CurrentUICulture))
        //{
        //    _expandErrorMessage = true;
        //    _errorLevel = aErrorLevel;
        //    _errorNumber = aErrorNumber;
        //    _memberName = memberName;
        //    _sourceFilePath = sourceFilePath;
        //    _sourceLineNumber = sourceLineNumber;
        //}
        public MIDException(eErrorLevel aErrorLevel, int aErrorNumber, Exception innerException,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            : base("MID Error#" + aErrorNumber.ToString(CultureInfo.CurrentUICulture), innerException)
        {
            _expandErrorMessage = true;
            _errorLevel = aErrorLevel;
            _errorNumber = aErrorNumber;
            _memberName = memberName;
            _sourceFilePath = sourceFilePath;
            _sourceLineNumber = sourceLineNumber;
        }
        // end TT#370 added constructors to delay expansion of MID Error Message

        // The following 2 methods are added for remote serialization issues
        public MIDException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            : base(aInfo, aContext)
        {
            _memberName = memberName;
            _sourceFilePath = sourceFilePath;
            _sourceLineNumber = sourceLineNumber;
        }

        override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
        {
            base.GetObjectData(aInfo, aContext);
        }

        // Public properties
        // begin TT#370 Build Pack Enhancement -- J.Ellis
        
        /// <summary>
        /// When "true", expand error number to get message text; when "false" error number already expanded.
        /// </summary>
        public bool ExpandMidErrorMessage
        {
            get
            {
                return _expandErrorMessage;
            }
        }
        // end TT#370 Build Pack Enhancement -- J.Ellis
        public eErrorLevel ErrorLevel
        {
            get
            {
                return _errorLevel;
            }
        }

        public int ErrorNumber
        {
            get
            {
                return _errorNumber;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return Message;
            }
        }

        public string MemberName
        {
            get
            {
                return _memberName;
            }
        }

        public string SourceFilePath
        {
            get
            {
                return _sourceFilePath;
            }
        }

        public int SourceLineNumber
        {
            get
            {
                return _sourceLineNumber;
            }
        }
    }
    // end TT#370 Build Packs Enhancement -- J.Ellis
}
