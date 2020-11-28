using System;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Services;

using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The ServerObjectFactory class serves two purposes: 1) It defines the type of server that is running; 2) Provides a call
	/// to create a Session for the server type.
	/// </summary>

	public class ServerObjectFactory : MarshalByRefObject
	{
		//=======
		// FIELDS
		//=======

		eServerType _serverType;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ServerObjectFactory and sets the eServerType for this server.
		/// </summary>
		/// <param name="aServerType">
		/// The eServerType to assign to this server.
		/// </param>

		public ServerObjectFactory(eServerType aServerType)
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
		/// Creates a new Session of the current eServerType and returns.
		/// </summary>
		/// <param name="aObservedIP">
		/// The IP Address that the Client knows the serer as
		/// </param>
		/// <returns></returns>

		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//public Session CreateSession(string aObservedIP)
		public SessionRemote CreateSession(string aObservedIP)
		//End TT#708 - JScott - Services need a Retry availalbe.
		{
			try
			{
				CallContext.SetData("ObservedIP", aObservedIP);

				switch (_serverType)
				{
					case eServerType.Control:
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//return new ControlServerSession(false);
						return new ControlServerSessionRemote(false);
						//End TT#708 - JScott - Services need a Retry availalbe.

					case eServerType.Store:
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//return new StoreServerSession(false);
						return new StoreServerSessionRemote(false);
						//End TT#708 - JScott - Services need a Retry availalbe.

					case eServerType.Hierarchy:
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//return new HierarchyServerSession(false);
						return new HierarchyServerSessionRemote(false);
						//End TT#708 - JScott - Services need a Retry availalbe.

					case eServerType.Application:
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//return new ApplicationServerSession(false);
						return new ApplicationServerSessionRemote(false);
						//End TT#708 - JScott - Services need a Retry availalbe.

					case eServerType.Scheduler:
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//return new SchedulerServerSession(false);
						return new SchedulerServerSessionRemote(false);
						//End TT#708 - JScott - Services need a Retry availalbe.

					case eServerType.Header:
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//return new HeaderServerSession(false);
						return new HeaderServerSessionRemote(false);
						//End TT#708 - JScott - Services need a Retry availalbe.

					default:
						throw new Exception("Invalid eServerType defined in ServerObjectFactory");
				}
			}
			catch (Exception exc)
			{
				string message = exc.Message;
				throw;
			}
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

		/// <summary>
		/// Overriden call for inializing the Lifetime Service.  Returning null creates a permanent lifetime.
		/// </summary>
		/// <returns>
		/// Null.
		/// </returns>

		override public object InitializeLifetimeService()
		{
			return null;
		}
	}

	/// <summary>
	/// This TrackingHandler is used to replace the IP Address in the MarshaledObject with the one that the Client knows
	/// the server as.
	/// </summary>

	public class SessionTrackingHandler : ITrackingHandler
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		public void MarshaledObject(object aObj, ObjRef aObjRef)
		{
			object dataObj;
			string observedIP;
			int i;
			int charPos;
			string newURI;
			string[] URIArray;

			try
			{
				if (aObjRef.ChannelInfo != null)
				{
					dataObj = CallContext.GetData("ObservedIP");

					if (dataObj != null)
					{
						observedIP = (string)dataObj;

						for (i = aObjRef.ChannelInfo.ChannelData.GetLowerBound(0); i <= aObjRef.ChannelInfo.ChannelData.GetUpperBound(0); i++)
						{
							if (aObjRef.ChannelInfo.ChannelData[i] is ChannelDataStore)
							{
								foreach (string URI in ((ChannelDataStore)aObjRef.ChannelInfo.ChannelData[i]).ChannelUris)
								{
									charPos = URI.IndexOf("//") + 2;
									newURI = URI.Substring(0, charPos);
									newURI += observedIP;

									charPos = URI.IndexOf(":", charPos);
									newURI += URI.Substring(charPos, URI.Length - charPos);
									URIArray = new string[1] { newURI };

									aObjRef.ChannelInfo.ChannelData[i] = new ChannelDataStore(URIArray);
								}
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.Message;
				throw;
			}
		}

		public void DisconnectedObject(object aObj)
		{
		}

		public void UnmarshaledObject(object aObj, ObjRef aObjRef)
		{
		}
	}
}
