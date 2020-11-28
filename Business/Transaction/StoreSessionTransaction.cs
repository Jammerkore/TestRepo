using System;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Transaction class defines the "local" workspace for a series of functions in the client application.
	/// </summary>
	/// <remarks>
	/// This class gives the user the ability to store information that is "local", or unique, to a series of screens or functions.  This allows a Client
	/// application to open multiple functions of the same type, yet each has its own copy of information contained in this class.
	/// </remarks>

	public class StoreSessionTransaction : Transaction
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Transaction under the given SessionAddressBlock.
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock that owns this Transaction.
		/// </param>

		public StoreSessionTransaction(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
