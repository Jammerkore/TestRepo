using System;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// The ServerPing class is used to indicate that an MRS server is alive.
	/// </summary>
	/// <remarks>
	/// A ServerPing object is created after the server has come up to allow the Control Server to check to see if the server is active.
	/// </remarks>

	[Serializable]
	public class ServerPing : MarshalByRefObject
	{
		//=======
		// FIELDS
		//=======

		eServerType _serverType;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ServerPing.
		/// </summary>

		public ServerPing()
		{
		}

		/// <summary>
		/// Creates a new instance of ServerPing and sets the eServerType for this server.
		/// </summary>
		/// <param name="aServerType">
		/// The eServerType to assign to this server.
		/// </param>

		public ServerPing(eServerType aServerType)
		{
			_serverType = aServerType;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eServerType of this server.
		/// </summary>

		public eServerType ServerType
		{
			get
			{
				return _serverType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Gives the ServerPing an infinite lifetime by preventing a lease from being created.
		/// </summary>
		/// <returns>
		/// Always a null reference.
		/// </returns>

		override public object InitializeLifetimeService()
		{
			return null;
		}

		/// <summary>
		/// Returns the IP Address of the calling client
		/// </summary>
		/// <returns>
		/// The IP Address of the calling client.
		/// </returns>

		public IPAddress GetClientIPAddress()
		{
			try
			{
				return (IPAddress)CallContext.GetData("ClientIP");
			}
			catch (Exception exc)
			{
				string message = exc.Message;
				throw;
			}
		}
	}
}
