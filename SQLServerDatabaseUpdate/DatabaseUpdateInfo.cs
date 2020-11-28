using System;
using System.Globalization;
using System.Data;
using System.Collections;
using System.IO;
using System.IO.IsolatedStorage;

using MIDRetail.DataCommon;

namespace MIDRetail.DatabaseUpdate
{
	/// <summary>
	/// Summary description for DatabaseUpdateInfo.
	/// </summary>
	public class DatabaseUpdateInfo
	{
		Hashtable _servers;
		IsolatedStorageFile isoStore = null;
		string _user = string.Empty;
		string _selectedServer = string.Empty;
		char _separaterChar = ';';

		public string User 
		{
			get { return _user ; }
		}

		public string SelectedServer 
		{
			get { return _selectedServer ; }
		}

		public char SeparaterChar 
		{
			get { return _separaterChar ; }
		}
		
		public DatabaseUpdateInfo()
		{
			// read the user info
			isoStore =  IsolatedStorageFile.GetStore
				( IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null );
			try
			{
				_servers = new Hashtable();

				IsolatedStorageFileStream iStream = 
					new IsolatedStorageFileStream( Include.MIDDatabaseUpdateFile, FileMode.Open, isoStore );

				StreamReader reader = new StreamReader( iStream );
			
				String line = String.Empty;

				_user = reader.ReadLine().Trim();
				_selectedServer = reader.ReadLine().Trim();

				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					string[] entries = line.Split(';');
					Hashtable databases = new Hashtable();
					for (int i = 1; i < entries.Length; i++)
					{
						databases.Add(entries[i], null);
					}
					if (entries[0].Trim().Length > 0)
					{
						_servers.Add(entries[0], databases);
					}
				}
				iStream.Close();
			}
			catch 
			{
			}
		}

		public bool IsDatabaseSelected(string aServer, string aDatabase)
		{
			try
			{
				Hashtable databases = (Hashtable)_servers[aServer];
				if (databases != null)
				{
					if (databases.Contains(aDatabase))
					{
						return true;
					}
				}
				return false;
			}
			catch
			{
				throw;
			}
		}

		public void UpdateDatabaseUpdateInfo(string aLine)
		{
			try
			{
				bool newServer = false;
				string[] entries = aLine.Split(';');
				Hashtable databases = (Hashtable)_servers[entries[0]];
				if (databases == null)
				{
					databases = new Hashtable();
					newServer = true;
				}

				databases.Clear();
				for (int i=1; i<entries.Length; i++)
				{
					databases.Add(entries[i], null);
				}

				if (newServer)
				{
					_servers.Add(entries[0], databases);
				}
				else
				{
					_servers[entries[0]] = databases;
				}
			}
			catch
			{
				throw;
			}
		}

		public void WriteDatabaseUpdateInfo(string aUser, string aServer)
		{
			// save the user info
			IsolatedStorageFileStream oStream = 
				new IsolatedStorageFileStream( Include.MIDDatabaseUpdateFile, FileMode.Create, isoStore );
			StreamWriter writer = new StreamWriter( oStream );
			writer.WriteLine(aUser);
			writer.WriteLine(aServer);
			foreach (DictionaryEntry serverEntry in _servers)
			{
				string line = (string)serverEntry.Key;
				Hashtable databases = (Hashtable)serverEntry.Value;
				foreach (string database in databases.Keys)
				{
					line += ";" + database;
				}
				writer.WriteLine(line);
			}

			writer.Close();
		}
	}
}
