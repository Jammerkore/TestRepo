using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MIDRetail.DatabaseUpdate
{
	public class SQLLocator
	{
		[DllImport("odbc32.dll")]
		private static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);

		[DllImport("odbc32.dll")]
		private static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);

		[DllImport("odbc32.dll")]
		private static extern short SQLFreeHandle(short hType, IntPtr handle); 

		[DllImport("odbc32.dll",CharSet=CharSet.Ansi)]
		private static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, short inStringLength, StringBuilder outString, short outStringLength, out short outLengthNeeded);

		private const short SQL_HANDLE_ENV = 1;
		private const short SQL_HANDLE_DBC = 2;
		private const int SQL_ATTR_ODBC_VERSION = 200;
		private const int SQL_OV_ODBC3 = 3;
		private const short SQL_SUCCESS = 0;
		
		private const short SQL_NEED_DATA = 99;
		private const short DEFAULT_RESULT_SIZE = 1024;
		private const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";
	
		public static string[] GetServers()
		{
			string[] retVal = null;
			string txt = string.Empty;
			IntPtr henv = IntPtr.Zero;
			IntPtr hconn = IntPtr.Zero;
			StringBuilder inString = new StringBuilder(SQL_DRIVER_STR);
			StringBuilder outString = new StringBuilder(DEFAULT_RESULT_SIZE);
			short inStringLength = (short) inString.Length;
			short lenNeeded = 0;

			try
			{
				if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_ENV, henv, out henv))
				{
					if (SQL_SUCCESS == SQLSetEnvAttr(henv,SQL_ATTR_ODBC_VERSION,(IntPtr)SQL_OV_ODBC3,0))
					{
						if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_DBC, henv, out hconn))
						{
							if (SQL_NEED_DATA ==  SQLBrowseConnect(hconn, inString, inStringLength, outString, DEFAULT_RESULT_SIZE, out lenNeeded))
							{
								if (DEFAULT_RESULT_SIZE < lenNeeded)
								{
									outString.Capacity = lenNeeded;
									if (SQL_NEED_DATA != SQLBrowseConnect(hconn, inString, inStringLength, outString, lenNeeded, out lenNeeded))
										throw new ApplicationException("Unabled to aquire SQL Servers from ODBC driver.");
								}
								txt = outString.ToString();
								int start = txt.IndexOf("{") + 1;
								int len = txt.IndexOf("}") - start;
								if ((start > 0) && (len > 0))
									txt = txt.Substring(start,len);
								else
									txt = string.Empty;
							}						
						}
					}
				}
			}
			catch (Exception ex)
			{
				//Throw away any error if we are not in debug mode
#if (DEBUG)
				MessageBox.Show(ex.Message,"Acquire SQL Servier List Error");
#endif 
				txt = string.Empty;
			}
			finally
			{
				if (hconn != IntPtr.Zero)
					SQLFreeHandle(SQL_HANDLE_DBC,hconn);
				if (henv != IntPtr.Zero)
					SQLFreeHandle(SQL_HANDLE_ENV,hconn);
			}
	
			if (txt.Length > 0)
				retVal = txt.Split(",".ToCharArray());

			return retVal;
		}

		public static string[] GetCatalogs(string SQLServer, string UserName, string Password)
		{
			string[] retVal = null;
			string txt = string.Empty;
			IntPtr henv = IntPtr.Zero;
			IntPtr hconn = IntPtr.Zero;
			StringBuilder cnString = new StringBuilder(SQL_DRIVER_STR);
			StringBuilder inString = new StringBuilder("SERVER=" + SQLServer + ";UID=" + UserName + ";PWD=" + Password + ";");
			StringBuilder outString = new StringBuilder(DEFAULT_RESULT_SIZE);
			short cnStringLength = (short) cnString.Length;
			short inStringLength = (short) inString.Length;
			short lenNeeded = 0;

			try
			{
				if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_ENV, henv, out henv))
				{
					if (SQL_SUCCESS == SQLSetEnvAttr(henv,SQL_ATTR_ODBC_VERSION,(IntPtr)SQL_OV_ODBC3,0))
					{
						if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_DBC, henv, out hconn))
						{
							if (SQL_NEED_DATA ==  SQLBrowseConnect(hconn, cnString, cnStringLength, outString, DEFAULT_RESULT_SIZE, out lenNeeded))
							{
								if (SQL_NEED_DATA ==  SQLBrowseConnect(hconn, inString, inStringLength, outString, DEFAULT_RESULT_SIZE, out lenNeeded))
								{
									if (DEFAULT_RESULT_SIZE < lenNeeded)
									{
										outString.Capacity = lenNeeded;
										if (SQL_NEED_DATA != SQLBrowseConnect(hconn, inString, inStringLength, outString, lenNeeded, out lenNeeded))
											throw new ApplicationException("Unabled to aquire SQL Catalogs from ODBC driver.");
									}
									txt = outString.ToString();
									int start = txt.IndexOf("{") + 1;
									int len = txt.IndexOf("}") - start;
									if ((start > 0) && (len > 0))
										txt = txt.Substring(start,len);
									else
										txt = string.Empty;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				//Throw away any error if we are not in debug mode
#if (DEBUG)
				MessageBox.Show(ex.Message,"Acquire SQL Catalog List Error");
#endif 
				txt = string.Empty;
			}
			finally
			{
				if (hconn != IntPtr.Zero)
					SQLFreeHandle(SQL_HANDLE_DBC,hconn);
				if (henv != IntPtr.Zero)
					SQLFreeHandle(SQL_HANDLE_ENV,hconn);
			}
	
			if (txt.Length > 0)
				retVal = txt.Split(",".ToCharArray());

			return retVal;
		}
	}
}