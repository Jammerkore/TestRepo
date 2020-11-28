using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;

namespace MIDRetail.DataCommon
{
	abstract public class MIDMarshalByRefObject : MarshalByRefObject, IDisposable
	{
		//=======
		// FIELDS
		//=======

		private ISponsor _sponsor;
		private int _leaseTime;
		private bool _leaseInitialized;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of MIDMarshalByRefObject using the given ISponsor as the sponsoring object.
		/// </summary>
		/// <param name="aSponsor">
		/// The ISponsor object that will become the sponsor for the inheriting object.
		/// </param>

		public MIDMarshalByRefObject(ISponsor aSponsor)
		{
			_sponsor = aSponsor;
			_leaseTime = 60;
			_leaseInitialized = false;
		}

		/// <summary>
		/// Creates a new instance of MIDMarshalByRefObject using the given ISponsor as the sponsoring object.
		/// </summary>
		/// <param name="aSponsor">
		/// The ISponsor object that will become the sponsor for the inheriting object.
		/// </param>

		public MIDMarshalByRefObject(ISponsor aSponsor, int aLeaseTimeInSeconds)
		{
			_sponsor = aSponsor;
			_leaseTime = aLeaseTimeInSeconds;
			_leaseInitialized = false;
		}

		#region IDisposable Members

        // Begin TT#1440 - JSmith - Memory Issues
        //public void Dispose()
        virtual public void Dispose()
        // Begin TT#1440
		{
			Dispose(true);
			UnregisterLease();
			System.GC.SuppressFinalize(this);
		}

		virtual protected void Dispose(bool disposing)
		{
		}

		~MIDMarshalByRefObject()
		{
			Dispose(false);
		}

		#endregion

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes lifetime services for remoting.  This method also Registers the home Session as the sponsor for this object.
		/// </summary>
		/// <returns>
		/// The lease for this object.
		/// </returns>

		override public object InitializeLifetimeService()
		{
			ILease lease;

			try
			{
				lease = (ILease)base.InitializeLifetimeService();

				if (lease.CurrentState == LeaseState.Initial)  
				{
					lease.InitialLeaseTime = TimeSpan.FromSeconds(_leaseTime);
					lease.SponsorshipTimeout = TimeSpan.FromMinutes(1);
					lease.RenewOnCallTime = TimeSpan.FromSeconds(_leaseTime);
					lease.Register(_sponsor);
				}

				_leaseInitialized = true;

				return lease;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void UnregisterLease()
		{
			ILease lease;

			try
			{
				if (_leaseInitialized)
				{
					lease = (ILease)base.InitializeLifetimeService();
					lease.Unregister(_sponsor);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
