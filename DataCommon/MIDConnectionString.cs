using System;
//Begin Track #3727 - JScott - Write plans out in smaller chunks
using System.Configuration;
using System.Globalization;
//End Track #3727 - JScott - Write plans out in smaller chunks

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for MIDConnectionString.
	/// </summary>
	public class MIDConnectionString
	{
//Begin Track #3727 - JScott - Write plans out in smaller chunks
		static MIDConnectionString()
		{
			string commitLimitStr;
			
			try
			{
                //commitLimitStr = MIDConfigurationManager.AppSettings["CommitLimit"];
                commitLimitStr = MIDConfigurationManager.AppSettings["CommitLimit"];

				if (commitLimitStr != null)
				{
					if (commitLimitStr.ToUpper(CultureInfo.CurrentCulture) == "UNLIMITED")
					{
						CommitLimit = int.MaxValue;
					}
					else
					{
						CommitLimit = Convert.ToInt32(commitLimitStr);
					}
				}
				else
				{
					CommitLimit = Include.DefaultCommitLimit;
				}
			}
			catch
			{
				CommitLimit = Include.DefaultCommitLimit;
			}
		}

		public static int CommitLimit = Include.DefaultCommitLimit;
//End Track #3727 - JScott - Write plans out in smaller chunks
		public static string ConnectionString  = string.Empty;
        public static int ThreadID;  // TT#739-MD -JSmith - Delete Stores
	}
}