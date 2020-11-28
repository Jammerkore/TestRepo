using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for MIDUserInfo.
	/// </summary>
	public class MIDUserInfo
	{
		string _user = null;
		bool _showLogin = true;
		IsolatedStorageFile isoStore = null;
		
		public string User 
		{
			get { return _user ; }
			set { _user = value; }
		}
        // Begin Track #5755 - JSmith - Windows login changes
        //public bool ShowLogin 
        //{
        //    get { return _showLogin ; }
        //    set {_showLogin = value;}
        //}
        // End Track #5755
		public MIDUserInfo()
		{
//			// Begin MID Track #5106 - JSmith - Error writing Isolated Storage
//			// read the user info
//			isoStore =  IsolatedStorageFile.GetStore
//				( IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null );
//			// End MID Track #5106
			try
			{
				// Begin MID Track #5106 - JSmith - Error writing Isolated Storage
				// read the user info
				isoStore =  IsolatedStorageFile.GetStore
					( IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null );
				// End MID Track #5106

				IsolatedStorageFileStream iStream = 
					new IsolatedStorageFileStream( Include.MIDUserNameFile, FileMode.Open, isoStore );

				StreamReader reader = new StreamReader( iStream );

				String line = reader.ReadLine();
				if (line != null )
				{
					_user = line;
				}
				line = reader.ReadLine();
				if (line != null )
				{
					_showLogin = Convert.ToBoolean(line, CultureInfo.CurrentUICulture);
				}
				iStream.Close();
			}
			catch
			{
				// Begin MID Track #5106 - JSmith - Error writing Isolated Storage
				_user = string.Empty;
				_showLogin = true;
				// End MID Track #5106
			}
		}

		public void WriteUserInfo()
		{
			// Begin MID Track #5106 - JSmith - Error writing Isolated Storage
//			// save the user info
//			IsolatedStorageFileStream oStream = 
//				new IsolatedStorageFileStream( Include.MIDUserNameFile, FileMode.Create, isoStore );
//			StreamWriter writer = new StreamWriter( oStream );
//			writer.WriteLine(_user);
//			writer.WriteLine(Convert.ToString(_showLogin, CultureInfo.CurrentUICulture));
//			writer.Close();
			try
			{
				// save the user info
				IsolatedStorageFileStream oStream = 
					new IsolatedStorageFileStream( Include.MIDUserNameFile, FileMode.Create, isoStore );
				StreamWriter writer = new StreamWriter( oStream );
				writer.WriteLine(_user);
				writer.WriteLine(Convert.ToString(_showLogin, CultureInfo.CurrentUICulture));
				writer.Close();
			}
			catch
			{
			}
			// End MID Track #5106
		}
	}
}
