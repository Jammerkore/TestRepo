using System;
using System.Runtime.Remoting;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// ServerList processes a list of server names and provides functionality for load balancing.
	/// </summary>
	/// <remarks>
	/// The ServerList class assists in determining which server is available for use.  Currently, it
	/// distributes servers in a round-robin fashion without any load balancing attempted.
	/// Future implementation will distribute servers based upon a chosen load balancing strategy.
	/// </remarks>

	public class ServerList
	{
		//=======
		// FIELDS
		//=======

		private int _lastServer;
		private System.Collections.ArrayList _serverList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of ServerList with a given list of server names
		/// </summary>
		/// <param name="aServerNameList">
		/// String containing a list of server names.  A server name is denoted in a "IP Address:Port" format, with
		/// multiple server names separated by ",".
		/// </param>

		public ServerList(string aServerNameList)
		{
			string[] serverArray;

			try
			{
				_lastServer = -1;
				_serverList = new System.Collections.ArrayList();

				if (aServerNameList != null)
				{
					serverArray = aServerNameList.Split(new char[] { ',' });

					foreach (string server in serverArray)
					{
						if (server.Trim().Length > 0)
						{
							_serverList.Add(server);
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns the next available server for the type requested.
		/// </summary>
		/// <param name="aServerType">
		/// Specifies the type of server requested.  For example, Control, Application, Store, etc.
		/// </param>
		/// <returns>
		/// String containing a server name for the requested type.  The server name is denoted in a
		///  "IP Address:Port" format. 
		/// </returns>

		public string GetServer(eServerType aServerType)
		{
			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//ServerPing serverPing;
			ServerObjectFactory soFactory;
			//End Fix - JScott - Correct Group Dynamite Conneciton problem
			string server;
			int i;
			int currServer;

			try
			{
				server = System.String.Empty;

				for (i = 0; i < _serverList.Count; i++)
				{
					currServer = (_lastServer + i + 1) % _serverList.Count;

					try
					{
						//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
						//serverPing = (ServerPing)RemotingServices.Connect(typeof(ServerPing), "tcp://" + (string)_serverList[currServer] + "/MRSServerPing.rem");
						soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + (string)_serverList[currServer] + "/MRSServerObjectFactory.rem");
						//End Fix - JScott - Correct Group Dynamite Conneciton problem

						try
						{
							//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
							//if (serverPing.ServerType == aServerType)
							if (soFactory.ServerType == aServerType)
							//End Fix - JScott - Correct Group Dynamite Conneciton problem
							{
								server = (string)_serverList[currServer];
								_lastServer = currServer;
								break;
							}
						}
						catch (System.Net.Sockets.SocketException)
						{
						}
					}
					catch (System.Runtime.Remoting.RemotingException)
					{
					}
				}

				return server;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
