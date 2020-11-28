using System;
using System.Runtime.Remoting.Lifetime;

namespace MIDRetail.Common
{
	/// <summary>
	/// SessionSponsor is the class that is created to sponsor remoted objects.
	/// </summary>
	/// <remarks>
	/// The SessionSponsor class should be used as the sponsor on any remoted objects that are created in any client application
	/// (include batch clients).  The Renewal defaults to 1 minute.
	/// </remarks>

	public class SessionSponsor : MarshalByRefObject, ISponsor
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of SessionSponsor
		/// </summary>

		public SessionSponsor()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gives the SessionSponsor an infinite lifetime by preventing a lease from being created.
		/// </summary>
		/// <returns>
		/// Always a null reference.
		/// </returns>

		override public object InitializeLifetimeService()
		{
			return null;
		}

		/// <summary>
		/// Requests a sponsoring client to renew the lease for the specified object.
		/// </summary>
		/// <param name="aLease">
		/// The lifetime lease of the object that requires lease renewal. 
		/// </param>
		/// <returns>
		/// The additional lease time for the specified object.
		/// </returns>

		public TimeSpan Renewal(ILease aLease)
		{
			return TimeSpan.FromMinutes(1);
		}
	}
}
