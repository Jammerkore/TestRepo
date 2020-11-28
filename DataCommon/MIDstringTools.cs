using System;
using System.Collections;
using System.Collections.Generic;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for MIDstring.
	/// </summary>
	public class MIDstringTools
	{
		//private MIDstringTools() {} added 12.17.03 - RS
		//public class MIDstringTools violates FXCop Rule: "Types with only static members should not have public or protected constructors"
		private MIDstringTools() {}

        /// <summary>
        /// Splits a string that may contain fields delimited by quotes and returns it into a generic list.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delimiter"></param>
        /// <param name="trimLeadingAndTrailingSpaces"></param>
        /// <returns></returns>
        public static List<string> SplitGeneric(string text, char delimiter, bool trimLeadingAndTrailingSpaces)
        {
            List<string> fields = new List<string>();

            string[] sFields = Split(text, delimiter, trimLeadingAndTrailingSpaces);

            fields.AddRange(sFields);

            return fields;
        }



		/// <summary>
		/// Splits a string that may contain fields delimited by quotes 
		/// </summary>
		/// <param name="text">The string to be parsed.</param>
		/// <param name="delimiter">The delimiter used to seperate the fields.</param>
		/// <param name="trimLeadingAndTrailingSpaces">This flag identifies it leading and trailing spaces should be removed.</param>
		/// <returns></returns>
		public static string[] Split(string text, char delimiter, bool trimLeadingAndTrailingSpaces)
		{
			try
			{
				// check to see if enclosed in double quotes
				if (text.Length > 1)
				{
					// if enclosed in double quotes, strip them off
					if (text[0] == '\"' && text[text.Length - 1] == '\"')
					{
						text = text.Substring(1, text.Length - 2);
					}
				}

				int i=0;
				string field = null;
				ArrayList al = new ArrayList(); 
				for (i=0; i<text.Length; i++)
				{
					if (text[i] == delimiter)
					{
						if (field != null && trimLeadingAndTrailingSpaces)
						{
							field = field.Trim();
						}
						al.Add(field);
						field = null;
					}
					else
					{
						field += text[i];
					}
				}
				// add last field
				if (field != null && trimLeadingAndTrailingSpaces)
				{
					field = field.Trim();
				}

				al.Add(field);

				// move arraylist to array of strings
				string[] fields;
				fields = new string[al.Count];
				i = 0;
				foreach(string s in al)
				{
					fields[i] = s;
					++i;
				}

				return fields;
			}
			catch
			{
				throw;
			}
		}
	}

}
