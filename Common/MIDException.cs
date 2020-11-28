

// TT#370 Build Pack Enhancment -- J.Ellis
//   MOVED entire class to DataCommon into MIDExceptions.cs class.

//using System;
//using System.Runtime.Serialization;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Common
//{
    ///// <summary>
    ///// Summary description for MIDException.
    ///// </summary>
    //[Serializable()]
    //public class MIDException : System.Exception
    //{
    //    private eErrorLevel _errorLevel;
    //    private int _errorNumber;
    //    private Audit _audit;

    //    // Constructors

    //    public MIDException(eErrorLevel aErrorLevel, int aErrorNumber, string aErrorMessage) : base(aErrorMessage)
    //    {
    //        _errorLevel = aErrorLevel;
    //        _errorNumber = aErrorNumber;
    //        _audit = new Audit();
    //    }

    //    public MIDException(eErrorLevel aErrorLevel, int aErrorNumber, string aErrorMessage, Exception innerException) : base(aErrorMessage, innerException)
    //    {
    //        _errorLevel = aErrorLevel;
    //        _errorNumber = aErrorNumber;
    //        _audit = new Audit();
    //    }
    //    // The following 2 methods are added for remote serialization issues
    //    public MIDException(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
    //        : base(aInfo, aContext)
    //    {
    //    }

    //    override public void GetObjectData(System.Runtime.Serialization.SerializationInfo aInfo, System.Runtime.Serialization.StreamingContext aContext)
    //    {
    //        base.GetObjectData(aInfo, aContext);
    //    }

    //    // Public properties

    //    public eErrorLevel ErrorLevel
    //    {
    //        get
    //        {
    //            return _errorLevel;
    //        }
    //    }

    //    public int ErrorNumber
    //    {
    //        get
    //        {
    //            return _errorNumber;
    //        }
    //    }

    //    public string ErrorMessage
    //    {
    //        get
    //        {
    //            return Message;
    //        }
    //    }
    //}
//}
