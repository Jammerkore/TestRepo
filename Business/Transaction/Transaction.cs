using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Transaction class defines the "local" workspace for a series of functions in the client application.
	/// </summary>
	/// <remarks>
	/// This class gives the user the ability to store information that is "local", or unique, to a series of screens or functions.  This allows a Client
	/// application to open multiple functions of the same type, yet each has its own copy of information contained in this class.
	/// </remarks>

	abstract public class Transaction : MIDMarshalByRefObject
	{
		//=======
		// FIELDS
		//=======

		private SessionAddressBlock _SAB;
		private TransactionData _TD;
		private GlobalOptionsProfile _globalOptions;
		private long _transactionID;   // MID Track 4297 Style Review Error when select mult hdrs with at least one released

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public Transaction(SessionAddressBlock aSAB)
			: base(aSAB.Sponsor)
		{
			_SAB = aSAB;
			_TD = null;
			_globalOptions = null;
			_transactionID = DateTime.Now.Ticks;  // MID Track 4297 Style Review gets error when select mult hdr with at least one released
		}

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public Transaction(SessionAddressBlock aSAB, int aLeaseTimeInSeconds)
			: base(aSAB.Sponsor, aLeaseTimeInSeconds)
		{
			_SAB = aSAB;
			_TD = null;
			_globalOptions = null;
			_transactionID = DateTime.Now.Ticks;  // MID Track 4297 Style Review gets error when select mult hdr with at least one released
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the SessionAddressBlock of this Transaction.
		/// </summary>

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		public TransactionData DataAccess
		{
			get
			{
				try
				{
					if (_TD == null)
					{
						_TD = new TransactionData();
					}

					return _TD;
				}
				catch
				{
					throw;
				}
			}
		}

		public GlobalOptionsProfile GlobalOptions
		{
			get
			{
				try
				{
					if (_globalOptions == null)
					{
						_globalOptions = SAB.ApplicationServerSession.GlobalOptions;
					}

					return this._globalOptions;
				}
				catch
				{
					throw;
				}
			}
		}

		// begin MID Track 4297 Style Review gets error when select mult hdr with at least one released
		public long TransactionID
		{
			get
			{
				return _transactionID; 
			}
		}
        // end MID Track 4297 Style Review gets error when select mult hdr with at least one released
		//========
		// METHODS
		//========

		/// <summary>
		/// Gets random number.
		/// </summary>
		/// <returns>Random Double</returns>
		
		public double GetRandomDouble() 
		{
			try
			{
				return MIDMath.GetRandomDouble();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Gets random number.
		/// </summary>
		/// <returns>Random positive integer.</returns>
		
		public int GetRandomInteger()
		{
			try
			{
				return MIDMath.GetRandomInteger();
			}
			catch
			{
				throw;
			}
		}
	}
}
